﻿//******************************************************************************************************
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
import './../../Content/BigTable.css';

export default class BigTable extends React.Component<any, any> {

    props: { cols: Array<any>, data: Array<any>, onClick: Function, sortField: string, ascending: boolean, onSort: Function, tableClass?: string, theadStyle?: object, tbodyStyle?: object };
    constructor(props) {
        super(props);
    }


    render() {
        var rowComponents = this.generateRows(this.props.data);
        var headerComponents = this.generateHeaders();
        return (
            <div className={'divTable ' + (this.props.tableClass != undefined ? this.props.tableClass : '')} >
                <div className={'divTableHeading'} style={this.props.theadStyle}>{headerComponents}</div>
                <div className={'divTableBody'} style={this.props.tbodyStyle}>{rowComponents}</div>
            </div>
        );
    }

    generateHeaders() {
        if (this.props.cols == null || this.props.cols.length == 0) return null;

        var cells = this.props.cols.map(colData => {
            var style = colData.headerStyle;
            if(style.cursor == undefined)
                style.cursor = 'pointer';

            return <div className='divTableHead' key={colData.key} style={style} onClick={this.handleSort.bind(this, {col: colData.key, ascending: this.props.ascending})}>{colData.label}{(this.props.sortField == colData.key ? <span className={"glyphicon " + (this.props.ascending ? "glyphicon-triangle-top" : "glyphicon-triangle-bottom")}></span>: null)}</div>
        });

        return <div className='divTableRow'>{cells}</div>;
    }

    generateRows(data) {
        if (data == null || data.length == 0) return null;

        return data.map((item, index) => {
            var cells = this.props.cols.map(colData => {
                var style = _.clone(colData.rowStyle);
                return <div
                    className='divTableCell'
                    key={index.toString() + item[colData.key] + colData.key}
                    style={style}
                    onClick={this.handleClick.bind(this, { col: colData.key, row: item, data: item[colData.key] })}
                >
                    {colData.content != undefined ? colData.content(this.round(item[colData.key]), style) : this.round(item[colData.key])}
                </div>
            });

            var style = { cursor: 'pointer' };

            return <div className='divTableRow' style={style} key={index.toString()}>{cells}</div>;
        });
    }

    round(value) {
        if (Number.isInteger(value) || isNaN(Number.parseFloat(value)))
            return value;
        else if (value > 100)
            return Math.round(value);
        else if (value > 1)
            return value.toFixed(1);
        else
            return value.toPrecision(2);
    }

    handleClick(data, event) {
        this.props.onClick(data);
    }

    handleSort(data, event) {
        this.props.onSort(data);
    }
};