import React, { Component } from 'react';
import authService from './api-authorization/AuthorizeService'

export class MyPlugins extends Component {
  static displayName = MyPlugins.name;

  constructor(props) {
    super(props);
    this.state = { plugins: [], loading: true };
  }

  componentDidMount() {
    this.populatePluginsData();
  }

  static renderPluginsTable(plugins) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Name</th>
            <th>Version</th>
            <th>SourceCode</th>
            <th>EmberVersion</th>
            <th>Detail</th>
          </tr>
        </thead>
        <tbody>
          {plugins.map(plugin =>
            <tr key={plugin.id}>
              <td>{plugin.name}</td>
              <td>{plugin.latestVersion}</td>
              <td>{plugin.sourceUrl}</td>
              <td>{plugin.emberVersion}</td>
              <td><a href={`/plugins/versions/${plugin.id}`}>Detail</a></td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : MyPlugins.renderPluginsTable(this.state.plugins);

    return (
      <div>
        <h1 id="tabelLabel" >My Plugins</h1>
            <p>The plugins you created.</p>
            <p>Use <a href='https://github.com/OsuSync/EmberTools' target='_blank' rel="noopener noreferrer">EmberAuthorTools</a> to upload and manage your plugin</p>
        {contents}
      </div>
    );
  }

  async populatePluginsData() {
    const token = await authService.getAccessToken();
    const response = await fetch('plugins/my', {
      headers: !token ? {} : { 'Authorization': `Bearer ${token}` }
    });
    const data = await response.json();
    this.setState({ plugins: data, loading: false });
  }
}
