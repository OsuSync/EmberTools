using Autofac;
using EmberKernel.Services.Statistic.DataSource.Variables;
using EmberKernel.Services.Statistic.Format;
using EmberKernel.Services.Statistic.Formatter.DefaultImpl.FormatExpression;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EmberKernel.Services.Statistic.Formatter.DefaultImpl
{
    public class DefaultFormatter : IFormatter
    {
        private readonly ILogger<DefaultFormatter> logger;
        protected static readonly ExpressionContext converter = new ExpressionContext();
        protected static readonly ExpressionParser parser = new ExpressionParser();
        private static readonly Regex calcRegex = new Regex(@"\$\{(((?:\w|\s|_|\.|,|\(|\)|\""|\^|\+|\-|\*|\/|\%|\<|\>|\=|\!|\||\&)*)(?:@(\d+))?)\}");

        private readonly Dictionary<string, RegisteredFormat> registeredFormats = new Dictionary<string, RegisteredFormat>();

        public DefaultFormatter(ILogger<DefaultFormatter> logger, IDataSource dataSource)
        {
            this.logger = logger;
            dataSource.OnMultiDataChanged += OnDataUpdate;
        }

        private void OnDataUpdate(IEnumerable<Variable> changedVariables)
        {
            var changedVariableNames = changedVariables.Select((variable) => variable.Id);
            var notifyFormats = registeredFormats.Where(x => x.Value.Format.RequestVariables.Intersect(changedVariableNames).Any());

            foreach (var variable in changedVariables)
            {
                //update variable for context.
                converter.Variables[variable.Id] = variable.Value switch {
                    DataSource.Variables.Value.NumberValue number => ValueBase.Create(number.Value),
                    DataSource.Variables.Value.StringValue str => ValueBase.Create(str.Value),
                    _ => ValueBase.Create($"<UNK VAR:{variable.Id} TYPE:{variable.Value.GetType().Name}>")
                };
            }

            foreach (var (_, registerdFormat) in notifyFormats)
            {
                logger.LogDebug($"notify variables updated for format (#{registerdFormat.Format.Id})");
                var info = registerdFormat.FormatInfo;
                if (!(info.Scope.ResolveOptional(info.ContainerType) is IFormatContainer notifier))
                {
                    logger.LogWarning($"Can't resolve container '{info.ContainerType.Name}' of '{info.Format}'");
                    continue;
                }
                notifier.FormatUpdated(registerdFormat.FormatInfo.Format, registerdFormat.Format.FormatFunction());
            }
        }

        public DefaultFormat Build(string id, string format)
        {
            var formatArray = new List<Func<string>>();
            var mayRequestVariables = new HashSet<string>();
            var rawFormatContent = format;

            while (true)
            {
                var match = calcRegex.Match(format);

                if (!match.Success)
                    break;

                if (match.Index != 0)
                {
                    var part = format.Substring(0, match.Index);
                    formatArray.Add(() => part);
                    format = format.Substring(match.Index);
                }

                var expr = match.Groups[2].Value;
                var astNode = parser.Parse(expr);
                var exprFunc = converter.ConvertAstToComplexLambdaWithDefault(astNode);

                foreach (var variable in parser.AnalyseMayRequestVariables(astNode))
                    mayRequestVariables.Add(variable);

                var roundLength = int.TryParse(match.Groups[3].Value, out var d) ? d : -1;

                formatArray.Add(() => {
                    var val = exprFunc();

                    if (val.ValueType == ValueBase.Type.String || roundLength < 0)
                        return val.ValueToString();

                    return ((NumberValue)val).Value.ToString("F" + roundLength);
                });

                format = format.Substring(match.Length);
            }

            if(format.Length > 0)
                formatArray.Add(() => format);

            var sb = new StringBuilder();

            var formatFunc = new Func<string>( () =>
            {
                sb.Clear();

                foreach (var func in formatArray)
                    sb.Append(func());

                return sb.ToString();
            });

            return new DefaultFormat(id, mayRequestVariables, formatFunc);
        }

        public string Format(string format)
        {
            try
            {
                return Build(string.Empty, format).FormatFunction();
            }
            catch
            {
                return "<Invalid Format!>";
            }
        }

        #region (Un)Register formats methods implement

        public IEnumerable<string> GetRegisteredFormat<TContainer>() where TContainer : IFormatContainer
        {
            var type = typeof(TContainer);
            return registeredFormats.Where(x => type == x.Value.FormatInfo.ContainerType).Select(x => x.Key).ToList();
        }

        public bool IsRegistered<TContainer>(string id) where TContainer : IFormatContainer
        {
            return registeredFormats.ContainsKey(id);
        }

        public void Register<TContainer>(ILifetimeScope scope, string id, string format) where TContainer : IFormatContainer
        {
            registeredFormats[id] = new RegisteredFormat()
            {
                Format = Build(id, format),
                FormatInfo = new FormatInfo(format, scope, typeof(TContainer))
            };
            logger.LogDebug("register new format " + registeredFormats[id].Format.Id);
        }

        public void Unregister<TContainer>(string id) where TContainer : IFormatContainer
        {
            if (registeredFormats.TryGetValue(id, out var registerFormat))
            {
                registeredFormats.Remove(id);
                logger.LogDebug("unregister new format " + registerFormat.Format.Id);
            }
        }

        public void UnregisterAll<TContainer>() where TContainer : IFormatContainer
        {
            registeredFormats.Clear();
            logger.LogDebug("unregister all formats");
        }

        public void Update<TContainer>(string id, string format)
        {
            if (!registeredFormats.ContainsKey(id)) return;
            registeredFormats[id].Format = Build(id, format);
            registeredFormats[id].FormatInfo.Format = format;
        }

        #endregion
    }
}
