using Autofac;
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
        private readonly IDataSource dataSource;
        protected static readonly ExpressionContext converter = new ExpressionContext();
        protected static readonly ExpressionParser parser = new ExpressionParser();
        private static readonly Regex calcRegex = new Regex(@"\$\{(((?:\w|\s|_|\.|,|\(|\)|\""|\^|\+|\-|\*|\/|\%|\<|\>|\=|\!|\||\&)*)(?:@(\d+))?)\}");

        private readonly Dictionary<string, RegisteredFormat> registeredFormats = new Dictionary<string, RegisteredFormat>();

        public DefaultFormatter(ILogger<DefaultFormatter> logger, IDataSource dataSource)
        {
            this.logger = logger;
            this.dataSource = dataSource;
            dataSource.OnMultiDataChanged += OnDataUpdate;
        }

        private void OnDataUpdate(IEnumerable<string> changedPropertyNames)
        {
            var notifyFormats = registeredFormats.Where(x => x.Value.Format.RequestVariables.Intersect(changedPropertyNames).Any());

            foreach (var variable in dataSource.GetVariables(changedPropertyNames))
            {
                //update variable for context.
                converter.Variables[variable.Name] = variable.Value switch {
                    DataSource.Variables.Value.NumberValue number => ValueBase.Create(number.Value),
                    DataSource.Variables.Value.StringValue str => ValueBase.Create(str.Value),
                    _ => ValueBase.Create($"<UNK VAR:{variable.Name} TYPE:{variable.Value.GetType().Name}>")
                };
            }

            foreach (var format in notifyFormats)
            {
                logger.LogDebug($"notify variables updated for format (#{format.Value.Format.Id})");
                //todo 通知更新
            }
        }

        public DefaultFormat Build(string format)
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

            return new DefaultFormat()
            {
                FormatFunction = formatFunc,
                RequestVariables = mayRequestVariables,
            };
        }

        public string Format(string format)
        {
            if (!registeredFormats.TryGetValue(format,out var registerFormat))
            {
                /*
                func = Parse(format);
                logger.LogInformation($"parsed format");
                cachedFormatFunctions[format] = func;
                */

                return "<FORMAT NOT REGISTER>";
            }

            return registerFormat.Format.FormatFunction();
            /*
            try
            {
                return registerFormat.FormatFunction();
            }
            catch (Exception e)
            {
                logger.LogError($"Can't format content: {e.Message}");
                return "<FORMAT ERROR>";
            }*/
        }

        #region (Un)Register formats methods implement

        public IEnumerable<string> GetRegisteredFormat<TContainer>() where TContainer : IFormatContainer
        {
            var type = typeof(TContainer);
            return registeredFormats.Where(x => type == x.Value.FormatInfo.ContainerType).Select(x => x.Key).ToList();
        }

        public bool IsRegistered<TContainer>(string format) where TContainer : IFormatContainer
        {
            return registeredFormats.ContainsKey(format);
        }

        public void Register<TContainer>(ILifetimeScope scope, string format) where TContainer : IFormatContainer
        {
            registeredFormats[format] = new RegisteredFormat()
            {
                Format = Build(format),
                FormatInfo = new FormatInfo(format, scope, typeof(TContainer))
            };
            logger.LogDebug("register new format " + registeredFormats[format].Format.Id);
        }

        public void Unregister<TContainer>(string format) where TContainer : IFormatContainer
        {
            if (registeredFormats.TryGetValue(format, out var registerFormat))
            {
                registeredFormats.Remove(format);
                logger.LogDebug("unregister new format " + registerFormat.Format.Id);
            }
        }

        public void UnregisterAll<TContainer>() where TContainer : IFormatContainer
        {
            registeredFormats.Clear();
            logger.LogDebug("unregister all formats");
        }

        #endregion
    }
}
