//******************************************************************************************************
//  Table.tsx - Gbtc
//
//  Copyright © 2018, Grid Protection Alliance.  All Rights Reserved.
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
//  08/02/2018 - Billy Ernest
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as _ from 'lodash';

export default class Table extends React.Component<any, any> {

    props: { cols: Array<any>, data: Array<any>, onClick: Function, sortField: string, ascending: boolean, onSort: Function };
    constructor(props) {
        super(props);
    }

    componentDidUpdate(prevProps, prevState) {
    }

    render() {
        var rowComponents = this.generateRows();
        var headerComponents = this.generateHeaders();
        return (
            <table className="table table-condensed table-hover">
                <thead>{headerComponents}</thead>
                <tbody>{rowComponents}</tbody>
            </table>
        );
    }

    generateHeaders() {
        if (this.props.cols.length == 0) return null;

        var cells = this.props.cols.map(colData => {
            colData.headerStyle.cursor = 'pointer';

            return <th key={colData.key} style={colData.headerStyle} onClick={this.handleSort.bind(this, {col: colData.key, ascending: this.props.ascending})}>{colData.label}{(this.props.sortField == colData.key ? <span className={"glyphicon " + (this.props.ascending ? "glyphicon-triangle-top" : "glyphicon-triangle-bottom")}></span>: null)}</th>
        });

        return <tr>{cells}</tr>;
    }

    generateRows() {
        if (this.props.data.length == 0) return null;

        return this.props.data.map((item, index) => {
            var cells = this.props.cols.map(colData => <td key={index.toString() + item[colData.key] + colData.key} onClick={this.handleClick.bind(this, { col: colData.key, row: item, data: item[colData.key] })}>{item[colData.key]}</td>);

            return <tr style={{cursor: 'pointer'}} key={index.toString()}>{cells}</tr>;
        });
    }

    handleClick(data, event) {
        this.props.onClick(data);
    }

    handleSort(data, event) {
        this.props.onSort(data);
    }
};
