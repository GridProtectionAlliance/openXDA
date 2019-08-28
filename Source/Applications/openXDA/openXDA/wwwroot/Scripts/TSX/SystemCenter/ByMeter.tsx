//******************************************************************************************************
//  ByMeter.tsx - Gbtc
//
//  Copyright © 2019, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/22/2019 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import Table from '../Table';
import * as _ from 'lodash';

declare var homePath: string;

export default class ByMeter extends React.Component<{}, { searchText: string, dfr: boolean, pqMeter: boolean, data: Array<{ ID: number, AssetKey: string, Name: string, Location: string, Type: 'DFR' | 'PQMeter', Make: string, Model: string }>, sortField: string, ascending: boolean }, {}>{
    jqueryHandle: JQuery.jqXHR;
    constructor(props, context) {
        super(props, context);
        this.state = {
            searchText: '',
            dfr: true,
            pqMeter: true,
            data: [],
            sortField: 'AssetName',
            ascending: true
        }
    }

    getMeters(): JQuery.jqXHR {
        if (this.jqueryHandle !== undefined)
            this.jqueryHandle.abort();

        this.jqueryHandle = $.ajax({
            type: "GET",
            url: `${homePath}api/SystemCenter/Meters`,
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: true,
            async: true
        });

        return this.jqueryHandle;
    }

    componentDidMount() {
        this.getMeters().done(data => this.setState({ data: data }));
    }

    render() {
        var windowHeight = window.innerHeight;

        var data = this.state.data.filter(x => (x.Name.toLowerCase().indexOf(this.state.searchText) >= 0 || x.AssetKey.toLowerCase().indexOf(this.state.searchText) >= 0 || x.Make.toLowerCase().indexOf(this.state.searchText) >= 0) && ((this.state.pqMeter ? x.Type == 'PQMeter' : x.Type != 'PQMeter') || (this.state.dfr ? x.Type == 'DFR' : x.Type != 'DFR') ))
        return (
            <div style={{ width: '100%', height: '100%' }}>
                <nav className="navbar navbar-expand-lg navbar-light bg-light">
                    <div className="collapse navbar-collapse" id="navbarSupportedContent" style={{ width: '100%' }}>
                        <ul className="navbar-nav mr-auto" style={{ width: '100%' }}>
                            <li className="nav-item" style={{ width: '50%', paddingRight: 10 }}>
                                <fieldset className="border" style={{ padding: '10px', height: '100%' }}>
                                    <legend className="w-auto" style={{ fontSize: 'large' }}>Text Search:</legend>
                                    <form>
                                        <input className='form-control' type='text' placeholder='Search...' value={this.state.searchText} onChange={(evt) => this.setState({ searchText: evt.target.value })} />
                                    </form>
                                </fieldset>
                            </li>
                            <li className="nav-item" style={{ width: '15%' }}>
                                <fieldset className="border" style={{ padding: '10px', height: '100%' }}>
                                    <legend className="w-auto" style={{ fontSize: 'large' }}>Meter Types:</legend>
                                    <form>
                                        <ul style={{ listStyleType: 'none', padding: 0 }}>
                                            <li><label><input type="checkbox" onChange={() => this.setState({ dfr: !this.state.dfr })} checked={this.state.dfr} />  DFR</label></li>
                                            <li><label><input type="checkbox" onChange={() => this.setState({ pqMeter: !this.state.pqMeter })} checked={this.state.pqMeter} />  PQMeter</label></li>
                                        </ul>
                                    </form>
                                </fieldset>
                            </li>

                        </ul>
                    </div>
                </nav>

                <div style={{ width: '100%', height: 'calc( 100% - 136px)' }}>
                    <Table
                        cols={[
                            { key: 'AssetKey', label: 'AssetKey', headerStyle: { width: '30%' }, rowStyle: { width: '30%' } },
                            { key: 'Name', label: 'Name', headerStyle: { width: '30%' }, rowStyle: { width: '30%' } },
                            { key: 'Location', label: 'Location', headerStyle: { width: '10%' }, rowStyle: { width: '10%' } },
                            { key: 'Type', label: 'Type', headerStyle: { width: '10%' }, rowStyle: { width: '10%' } },
                            { key: 'Make', label: 'Make', headerStyle: { width: '10%' }, rowStyle: { width: '10%' } },
                            { key: 'Model', label: 'Model', headerStyle: { width: 'calc(10%)' }, rowStyle: { width: 'calc(10% - 17px)' } },
                        ]}
                        tableClass="table table-hover"
                        data={data}
                        sortField={this.state.sortField}
                        ascending={this.state.ascending}
                        onSort={(d) => {
                            if (d.col == this.state.sortField) {
                                var ordered = _.orderBy(this.state.data, [d.col], [(!this.state.ascending ? "asc" : "desc")]);
                                this.setState({ ascending: !this.state.ascending, data: ordered });
                            }
                            else {
                                var ordered = _.orderBy(this.state.data, [d.col], ["asc"]);
                                this.setState({ ascending: true, data: ordered, sortField: d.col });
                            }
                        }}
                        onClick={(item) => { window.location.href = homePath + 'SystemCenter/index.cshtml?name=Meter&meterId=' + item.row.ID}}
                        theadStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                        tbodyStyle={{ display: 'block', overflowY: 'auto', maxHeight: window.innerHeight - 182, width: '100%'  }}
                        rowStyle={{ fontSize: 'smaller', display: 'table', tableLayout: 'fixed', width: '100%' }}
                        selected={(item) => false}
                    />
                </div>
            </div>
        )
    }
}

