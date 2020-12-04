//******************************************************************************************************
//  Filter.tsx - Gbtc
//
//  Copyright © 2020, Grid Protection Alliance.  All Rights Reserved.
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
//  10/20/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import _ from 'lodash';
import { CheckBox, Select } from '@gpa-gemstone/react-forms'
import { Filter } from '../global';

interface IProps<T> {
    CollumnList: Array<Filter.IField<T>>, Id: string, SetFilter: (filters: Filter.IFilter<T>[]) => void, defaultCollumn?: Filter.IField<T>, Direction?: 'left' | 'right'}

export default function Filter<T>(props: IProps<T>) {
    const [hover, setHover] = React.useState<boolean>(false);
    const [filters, setFilters] = React.useState<Filter.IFilter<T>[]>([]);
    const [filter, setFilter] = React.useState<Filter.IFilter<T>>({ FieldName: props.CollumnList[0].key, SearchText: '', Operator: 'LIKE', Type: props.CollumnList[0].type });

    const [search, setSearch] = React.useState<string>("");
    const [searchFilter, setSearchFilter] = React.useState<Filter.IFilter<T>>(null);

    // Update SearchFilter if there are 3+ Character and only do it every 500ms to avoid hammering the server while typing
    React.useEffect(() => {
        let handle = null;
        if (search.length > 3)
            handle = setTimeout(() => {
                setSearchFilter({ FieldName: props.defaultCollumn.key, Operator: 'LIKE', Type: props.defaultCollumn.type, SearchText: (search + '*') })
            }, 500);
        else
            handle = setTimeout(() => {
                setSearchFilter(null)
            }, 500);

        return () => { if (handle != undefined) clearTimeout(handle); };
    }, [search]);

    // Call props.setFilter when SearchFilter updates
    React.useEffect(() => {
        if (searchFilter != undefined)
            props.SetFilter([...filters, searchFilter]);
        else
            props.SetFilter(filters);
        return () => { }
    }, [searchFilter])

    function deleteFilter(f: Filter.IFilter<T>) {
        let index = filters.findIndex(fs => fs == f);
        let filts = _.cloneDeep(filters);
        filts.splice(index, 1);
        setFilters(filts);
        setHover(false);
        if (props.defaultCollumn != undefined && searchFilter != undefined)
            props.SetFilter([...filts, searchFilter]);
        else
            props.SetFilter(filts);
    }

    function addFilter() {
        let oldFilters = _.cloneDeep(filters);
        oldFilters.push(filter);
        setFilters(oldFilters);
        setFilter({ FieldName: props.CollumnList[0].key, SearchText: '', Operator: 'LIKE', Type: props.CollumnList[0].type });
        if (props.defaultCollumn != undefined && searchFilter != undefined)
            props.SetFilter([...oldFilters, searchFilter]);
        else
            props.SetFilter(oldFilters);

        ($('#' + props.Id) as any).modal('hide')
        
    }


    return (
        <div style={{ width: '100%' }}>
            <nav className="navbar navbar-expand-lg navbar-light bg-light">
                <div className="collapse navbar-collapse" style={{ width: '100%' }}>
                    <ul className="navbar-nav mr-auto" style={{ width: '100%' }}>
                        <li className="nav-item" style={{ width: '85%', paddingRight: 10 }}>
                            {props.defaultCollumn != undefined ?
                                <div className="form-inline my-2 my-lg-0">
                                    <input className="form-control mr-sm-2" type="search" placeholder={"Search " + props.defaultCollumn.label} onChange={(event) => setSearch(event.target.value as string)} />
                                </div> : null}
                        </li>
                        <li className="nav-item" style={{ width: '15%', paddingRight: 10 }}>
                            <div style={{ position: 'relative', display: 'inline-block' }}>
                                <button className="btn btn-primary" onClick={(evt) => { evt.preventDefault(); ($('#' + props.Id) as any).modal('toggle');}} onMouseEnter={() => setHover(true)} onMouseLeave={() => setHover(false)}>Add Filter</button>
                                <div style={{ width: window.innerWidth / 3, display: hover ? 'block' : 'none', position: 'absolute', backgroundColor: '#f1f1f1', boxShadow: '0px 8px 16px 0px rgba(0,0,0,0.2)', zIndex: 1, right: (props.Direction == 'right' ? 0 : null), left: (props.Direction == 'left' ? 0: null) }} onMouseEnter={() => setHover(true)} onMouseLeave={() => setHover(false)}>
                                    <table className='table'>
                                        <thead>
                                            <tr><th>Column</th><th>Operator</th><th>Search Text</th><th>Remove</th></tr>
                                        </thead>
                                        <tbody>
                                            {filters.map((f, i) => <tr key={i}><td>{f.FieldName}</td><td>{f.Operator}</td><td>{f.SearchText}</td><td><button className="btn btn-sm" onClick={(e) => deleteFilter(f)}><span><i className="fa fa-trash"></i></span></button></td></tr>)}
                                        </tbody>

                                    </table>
                                </div>
                            </div>
                        </li>
                    </ul>
                </div>
            </nav>

            <div className="modal" id={props.Id}>
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h4 className="modal-title">Add Filter</h4>
                            <button type="button" className="close" onClick={() => ($('#' + props.Id)as any).modal('hide')}>&times;</button>
                        </div>
                        <div className="modal-body">
                            <Select<Filter.IFilter<T>> Record={filter} Field='FieldName' Options={props.CollumnList.map(fl => ({ Value: fl.key as string, Label: fl.label }))} Setter={(record) => {
                                let operator = "IN" as any;
                                let column = props.CollumnList.find(fl => fl.key == record.FieldName)
                                if (column.type == 'string')
                                    operator = "LIKE";

                                setFilter((prevFilter) => ({ ...prevFilter, FieldName: record.FieldName, SearchText: '', Operator: operator, Type: column.type }))
                            }} Label='Column' />
                            <FilterCreator Filter={filter} Field={props.CollumnList.find(fl => fl.key == filter.FieldName)} Setter={(record) => setFilter(record)} />
                        </div>

                        <div className="modal-footer">
                            <button type="button" className="btn btn-primary" onClick={() => addFilter()} >Add</button>
                            <button type="button" className="btn btn-danger" onClick={() => ($('#' + props.Id) as any).modal('hide')}>Close</button>
                        </div>

                    </div>
                </div>

            </div>

        </div>
    );
}

interface IPropsFilterCreator<T> { Filter: Filter.IFilter<T>, Setter: (filter: React.SetStateAction<Filter.IFilter<T>>) => void, Field: Filter.IField<T> }

function FilterCreator<T>(props: IPropsFilterCreator<T> ) {

    if (props.Field == undefined)
        return null;
    if (props.Field.type == "string") {
        return (
            <>
                <label>Column type is string. Wildcard (*) can be used with 'LIKE' and 'NOT LIKE'</label>
                <div className='row'>
                    <div className='col-4'>
                        <select className='form-control' value={props.Filter.Operator} onChange={(evt) => {
                            let value = evt.target.value as 'LIKE' | 'NOT LIKE' | '=';
                            props.Setter((prevState) => ({ ...prevState, Operator: value }));
                        }}>
                            <option value='LIKE'>LIKE</option>
                            <option value='NOT LIKE'>NOT LIKE</option>
                            <option value='='>=</option>
                        </select>
                    </div>
                    <div className='col'>
                        <input className='form-control' value={props.Filter.SearchText} onChange={(evt) => {
                            let value = evt.target.value as string;
                            props.Setter((prevState) => ({ ...prevState, SearchText: value }));
                        }} />
                    </div>

                </div>
            </>
        );
    }
    else if (props.Field.type == "integer" || props.Field.type == "number" || props.Field.type == "datetime") {
        return (
            <>
                <label>Column type is {props.Field.type}.</label>
                <div className='row'>
                    <div className='col-4'>
                        <select className='form-control' value={props.Filter.Operator} onChange={(evt) => {
                            let value = evt.target.value as '=' | '<>' | '>' | '<' | '>=' | '<=';
                            props.Setter((prevState) => ({ ...prevState, Operator: value }));
                        }}>
                            <option value='='>=</option>
                            <option value='<>'>!=</option>
                            <option value='>'>{`>`}</option>
                            <option value='>='>{`>=`}</option>
                            <option value='<'>{`<`}</option>
                            <option value='>='>{`>=`}</option>
                        </select>
                    </div>
                    <div className='col'>
                        <input className='form-control' value={props.Filter.SearchText} onChange={(evt) => {
                            let value = evt.target.value as string;
                            props.Setter((prevState) => ({ ...prevState, SearchText: value }));
                        }} />
                    </div>

                </div>
            </>
        );
    }
    else if (props.Field.type == "boolean") {
        return <CheckBox Record={props.Filter} Field='SearchText' Setter={(filter: Filter.IFilter<T>) => {
            props.Setter((prevFilter) => ({ ...prevFilter, Operator: '=', SearchText: filter.SearchText.toString() == 'true' ? '1' : '0' }))
        }} Label="Column type is boolean. Yes/On is checked." />
    }
    else {
        let valueList = [];
        props.Field.enum.forEach((key, value) => valueList.push({ ID: key, Text: value }));
        return (
            <>
                <label>Column type is enumerable. Select from below.</label>
                <ul style={{ listStyle: 'none' }}>
                    <li ><div className="form-check">
                        <input type="checkbox" className="form-check-input" style={{ zIndex: 1 }} onChange={(evt) => {
                            if (evt.target.checked)
                                props.Setter(prevSetter => ({ ...prevSetter, SearchText: `(${valueList.map(x => x.Text).join(',')})` }));
                            else
                                props.Setter(prevSetter => ({ ...prevSetter, SearchText: '' }));
                        }} defaultValue='off' />
                        <label className="form-check-label" >Select All</label>

                    </div></li>
                    {valueList.map(vli => <li key={vli.ID} ><div className="form-check">
                        <input type="checkbox" className="form-check-input" style={{ zIndex: 1 }} onChange={(evt) => {
                            if (evt.target.checked) {
                                let list = props.Filter.SearchText.replace('(', '').replace(')', '').split(',');
                                list = list.filter(x => x != "")
                                list.push(vli.Text)
                                let text = `(${list.join(',')})`;
                                props.Setter(prevSetter => ({ ...prevSetter, SearchText: text }));
                            }
                            else {
                                let list = props.Filter.SearchText.replace('(', '').replace(')', '').split(',');
                                list = list.filter(x => x != "")
                                list = list.filter(x => x != vli.Text)
                                let text = `(${list.join(',')})`;
                                props.Setter(prevSetter => ({ ...prevSetter, SearchText: text }));
                            }

                        }} value={props.Filter.SearchText.indexOf(vli.Text) >= 0 ? 'on' : 'off'} checked={props.Filter.SearchText.indexOf(vli.Text) >= 0 ? true : false} />
                        <label className="form-check-label" >{vli.Text}</label>

                    </div></li>)}
                </ul>
            </>
        );
    }
}

/* Custom React Hook for using functions with Cleanup => it will call whatever the function returns before it calls the function again 
  function useFunctionEffect(fxn) {
    const [flag, setFlag] = React.useState<boolean>(true);
    React.useEffect(() => { let cleanup = fxn(); return cleanup; }, [flag]);

    return () => setFlag(!flag)
}*/