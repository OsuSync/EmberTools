import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  constructor(props) {
    super(props);
    this.state = { plugins: [], loading: true };
  }

  componentDidMount() {
    this.populatePluginsData();
  }

  static renderPlugins(plugins) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Name</th>
            <th>Author</th>
            <th>Version</th>
            <th>SourceCode</th>
            <th>EmberVersion</th>
          </tr>
        </thead>
        <tbody>
          {plugins.map(plugin =>
            <tr key={plugin.id}>
              <td>{plugin.name}</td>
              <td>{plugin.author}</td>
              <td>{plugin.latestVersion}</td>
              <td>{plugin.sourceUrl}</td>
              <td>{plugin.emberVersion}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render () {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : Home.renderPlugins(this.state.plugins);
    return (
      <div>
        <h1>Ember Tools</h1>
        <h3>Plugins</h3>
        <p>Install just type 'plugins install {"<Name>"}' in ember tools</p>
        {contents}
      </div>
    );
    }


  async populatePluginsData() {
    const response = await fetch('plugins/all');
    const data = await response.json();
    this.setState({ plugins: data, loading: false });
  }
}
