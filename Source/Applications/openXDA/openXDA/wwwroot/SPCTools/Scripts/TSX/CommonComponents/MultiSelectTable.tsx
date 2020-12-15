//******************************************************************************************************
//  MultiSelectTable.tsx - Gbtc
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
//  10/26/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import _ from 'lodash';
import { CheckBox, Select } from '@gpa-gemstone/react-forms'
import Table, { TableProps } from '@gpa-gemstone/react-table';

interface IProps<T> extends TableProps<T>{ updateSelection: (selected: Array<T>) => void, primaryKey: keyof T, selectAll: boolean,  }

export default function MultiSelectTable<T>(props: IProps<T>) {
    const [selected, setSelected] = React.useState<Array<T>>([]);

    React.useEffect(() => {
        if (props.selectAll) 
            props.updateSelection(props.data);
        
        else
            props.updateSelection(props.data.filter((item, index) => selected.findIndex(i => i[props.primaryKey] == item[props.primaryKey]) > -1));

    }, [props.selectAll])

    React.useEffect(() => {
        props.updateSelection(props.data.filter((item, index) => selected.findIndex(i => i[props.primaryKey] == item[props.primaryKey]) > -1));
    }, [selected]);

    let tableprops = _.cloneDeep(props);
    delete tableprops.updateSelection;
    delete tableprops.primaryKey;

    tableprops.cols = [
        {
            key: null, label: '', headerStyle: { width: '1.5em' }, rowStyle: { width: '1.5em' }, content: (item, key, style) => {
                let index = selected.findIndex(i => item[props.primaryKey] == i[props.primaryKey]);
                let include = (index < 0 ? false : true)

                return ((include || props.selectAll) ? <div style={{ marginTop: '2px', textAlign: 'center' }}><i className="fa fa-check-square-o fa-1x" aria-hidden="true"></i></div> : null)
            }
        },
        ...props.cols
    ];

    tableprops.onClick = (data, event) => {
        if (props.selectAll)
            return;

        let index = selected.findIndex(i => data.row[props.primaryKey] == i[props.primaryKey]);

        if (index == -1)
            setSelected((old) => {
                let updated = _.cloneDeep(old);
                updated.push(data.row);
                return updated;
            });
        else
            setSelected((old) => {
                let updated = _.cloneDeep(old);
                updated.splice(index, 1);
                return updated;
            });

        props.onClick(data, event);
    }
    return (
        <Table<T> {...tableprops} />
    );
}
