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
import './../../Content/BigTable.css';

export default class BigTable extends React.Component<any, any> {

    props: { cols: Array<any>, data: Array<any>, onClick: Function, sortField: string, ascending: boolean, onSort: Function, tableClass?: string, theadStyle?: object, tbodyStyle?: object };
    constructor(props) {
        super(props);
        this.state = {
            headers: this.generateHeaders(),
            rows: this.generateRows(props.data)
        }
    }

    static getDerivedStateFromProps(props, state) {

    }

    shouldComponentUpdate(nextProps, nextState) {
        var newRows = this.generateRows(nextProps.data);
        if (newRows != null && JSON.stringify(nextProps.data) !== JSON.stringify(this.props.data)) {
            console.log(newRows);
            nextState.rows = newRows;
            return true;
        }
        else
            return false;
    }

    render() {
        var rowComponents = this.generateRows(this.props.data);
        var headerComponents = this.generateHeaders();
        return (
            <div className={'divTable ' + (this.props.tableClass != undefined ? this.props.tableClass : '')} >
                <div className={'divTableHeading'} style={this.props.theadStyle}>{this.state.headers}</div>
                <div className={'divTableBody'} style={this.props.tbodyStyle}>{this.state.rows}</div>
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
                    {colData.content != undefined ? colData.content(item, colData.key, style) : item[colData.key]}
                </div>
            });

            var style = { cursor: 'pointer' };

            return <div className='divTableRow' style={style} key={index.toString()}>{cells}</div>;
        });
    }

    handleClick(data, event) {
        this.props.onClick(data);
    }

    handleSort(data, event) {
        this.props.onSort(data);
    }
};

const HeaderRow = (props) => {
    if (props.cols == null || props.cols.length == 0) return null;
    var cells = props.cols.map(colData => {
        var style = colData.headerStyle;
        if (style.cursor == undefined)
            style.cursor = 'pointer';
        if (style.height == undefined)
            style.height = 50;

        return <HeaderCell key={colData.key} cellKey={colData.key} style={style} label={colData.label} onClick={props.handleSort.bind(this, { col: colData.key, ascending: props.ascending })} sortField={props.sortField} ascending={props.ascending}></HeaderCell>
    });

    return <div className='divTableRow'>{cells}</div>;
}

const HeaderCell = (props) => {
    return <div className='divTableHead' style={props.style} onClick={props.onClick}>{props.label}{(props.sortField == props.cellKey ? <span className={"glyphicon " + (props.ascending ? "glyphicon-triangle-top" : "glyphicon-triangle-bottom")}></span> : null)}</div>
}

const DataRow = (props) => {
    if (props.data == null || props.data.length == 0) return null;

    var cells = this.props.cols.map(colData => {
        var style = _.clone(colData.rowStyle);
        return <DataCell
                    key={props.index.toString() + props.item[colData.key] + colData.key}
                    colData={colData}
                    item={props.item}
                    style={style}
                    onClick={this.handleClick.bind(this, { col: colData.key, row: props.item, data: props.item[colData.key] })}
                />
    });

    return <div className='divTableRow' style={props.style} key={props.index.toString()}>{cells}</div>;
}

const DataCell = (props) => {
    return <div
                className='divTableCell'
                style={props.style}
                onClick={props.onClick}
                >
                {props.colData.content != undefined ? props.colData.content(props.item, props.colData.key, props.style) : props.item[props.colData.key]}
            </div>;
}

