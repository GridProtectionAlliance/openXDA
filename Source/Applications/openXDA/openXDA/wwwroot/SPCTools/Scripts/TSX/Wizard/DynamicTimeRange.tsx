//******************************************************************************************************
//  DynamicTimeRange.tsx - Gbtc
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
//  12/03/2020 - C. Lackner
//       Generated original version of source code.
//
//******************************************************************************************************

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import _ from 'lodash';
import { DynamicWizzard } from '../global';
import { useSelector, useDispatch } from 'react-redux';
import { SelectAlarmValues, updateAlarmValues, SelectThresholdValues, SelectAllowSlice } from './DynamicWizzardSlice';


export const DynamicTimeRange = (props: { alarmDayID: number, setter: ((selectedStartHour: number) => void) }) => {
    const dispatch = useDispatch();

    const allowSlice = useSelector(SelectAllowSlice)
    const [edit, setEdit] = React.useState<boolean>(false);
    const [editTstart, setEditTstart] = React.useState<number>(undefined);
    const [editTend, setEditTend] = React.useState<number>(undefined);

    const [data, setData] = React.useState<DynamicWizzard.IAlarmvalue[]>([]);
    const [selected, setSelected] = React.useState<number>(0);

    const memAlarValuesSelector = React.useMemo<(state) => DynamicWizzard.IAlarmvalue[]>( () =>  (state) => SelectAlarmValues(state, props.alarmDayID), [props.alarmDayID])
    const savedAlarmValues = useSelector(memAlarValuesSelector)

    const alarmValueThresholds = useSelector(SelectThresholdValues);

    React.useEffect(() => {
        let tmp = savedAlarmValues
        if (savedAlarmValues.length == 0) {
            tmp = [{ ID: 0, AlarmDayID: props.alarmDayID, AlarmID: -1, EndHour: 24, StartHour: 0, Formula: "", Value: undefined }]
            dispatch(updateAlarmValues({ alarmDayID: props.alarmDayID, alarmValues: tmp }));
        }
        setData(tmp);
    }, [savedAlarmValues])

    React.useEffect(() => {
      
        if (edit) {
            setEditTstart(data[selected].StartHour);
            setEditTend((data[selected].EndHour == undefined ? 24 : data[selected].EndHour));
            return;
        }
        if (editTend == undefined || editTstart == undefined)
            return;
        
        let result = _.cloneDeep(data);

        // If user edited the end Time
        if (editTend != data[selected].EndHour && selected != (data.length-1)) {
            let nextJ = selected + 1
            while (editTend > data[nextJ].EndHour && nextJ < data.length)
                nextJ = nextJ + 1;
            result.splice(selected + 1, (nextJ - selected - 1));

            result[selected].EndHour = editTend;
            if (result[selected + 1].StartHour > editTend)
                result = [...result.slice(0, selected + 1),
                    { ID: 0, AlarmDayID: props.alarmDayID, AlarmID: -1, EndHour: result[selected + 1].StartHour, StartHour: editTend, Formula: "", Value: undefined },
                    ...result.slice(selected+1)]
            else
                result[selected + 1].StartHour = editTend;
            
        }
        // If user edited the final EndTime
        if (editTend != data[selected].EndHour && selected == (data.length-1)) {
            result[selected].EndHour = editTend;
            if (editTend != 24)
                result.push({ ID: 0, AlarmDayID: props.alarmDayID, AlarmID: -1, EndHour: 24, StartHour: editTend, Formula: "", Value: undefined });
        }
        // If user edited the start Time
        if (editTstart != data[selected].StartHour && selected != 0) {
            let prevJ = selected - 1
            while (editTstart < data[prevJ].StartHour && prevJ > 0)
                prevJ = prevJ - 1;

            let updateSelected = selected;

            if (result[prevJ].EndHour < editTstart) {
                result = [...result.slice(0, prevJ + 1),
                    { ID: 0, AlarmDayID: props.alarmDayID, AlarmID: -1, EndHour: editTstart, StartHour: result[prevJ].EndHour, Formula: "", Value: undefined },
                    ...result.slice(selected)
                ]
                updateSelected = updateSelected + 1; 
            }
            else
                result[prevJ].EndHour = editTstart;

            result[updateSelected].StartHour = editTstart;
            result.splice(prevJ + 1, (selected - prevJ - 1));
        }
        // If user edited the intial StartTime
        if (editTstart != data[selected].StartHour && selected == 0) {
            result[selected].StartHour = editTstart;
            if (editTstart != 0)
                result = [{ ID: 0, AlarmDayID: props.alarmDayID, AlarmID: -1, EndHour: editTstart, StartHour: 0, Formula: "", Value: undefined }, ...result];
        }
        dispatch(updateAlarmValues({ alarmDayID: props.alarmDayID, alarmValues: result }));

        
    }, [edit])

    React.useEffect(() => {
        if (data[selected] != undefined)
            props.setter(data[selected].StartHour)
    }, [selected, data])

    const TendInValid = (editTend == undefined || editTend < 1 || editTend > 24 || (!(editTend > editTstart)));
    const TstartInValid = (editTstart == undefined || editTstart < 0 || editTstart > 23);


    return <div style={{ overflowY: 'scroll', overflowX: 'hidden' }}>
        {data.map((item, index) => <div key={index} className="row" onClick={() => { if (!edit) setSelected(index); }}>
            <div className="col" style={{ background: (index == selected ? 'yellow' : '') }}>
                {edit && index == selected ?
                    <div className="input-group input-group-sm">
                        <input type="number" className={"form-control form-control-sm" + (TstartInValid ? ' is-invalid' : '')}
                            value={editTstart} onChange={(evt) => setEditTstart(parseInt(evt.target.value))} />
                        <input type="number" className={"form-control form-control-sm" + (TendInValid ? ' is-invalid' : '')}
                            value={editTend} onChange={(evt) => setEditTend(parseInt(evt.target.value))} />
                        <div className="input-group-append">
                            <span className="input-group-text">
                                <i style={{
                                    marginRight: '10px',
                                    color: (TstartInValid || TendInValid ? '#bbbbbb' : '#28A745')
                                }}
                                    className="fa fa-check" onClick={() => { if (!TstartInValid && !TendInValid) setEdit(false); }}>
                                </i>
                            </span>
                        </div>
                    </div>
                    :
                    <p>
                        {alarmValueThresholds.find(threshold => threshold.AlarmDayID == props.alarmDayID && threshold.StartHour == item.StartHour) != undefined &&
                            alarmValueThresholds.find(threshold => threshold.AlarmDayID == props.alarmDayID && threshold.StartHour == item.StartHour).Value.some(item => !isNaN(item.Value)) &&
                            (alarmValueThresholds.find(threshold => threshold.AlarmDayID == props.alarmDayID && threshold.StartHour == item.StartHour).IsScalar || allowSlice) ?
                            <i style={{ marginRight: '10px', color: '#28A745' }} className="fa fa-check-circle"></i> :
                            <i style={{ marginRight: '10px', color: '#dc3545' }} className="fa fa-exclamation-circle"></i>}
                        {(item.StartHour < 10 ? '0' : '') + item.StartHour}:00 - {(item.EndHour == undefined ? '24' : ((item.EndHour < 10 ? '0' : '') + item.EndHour))}:00
                        <i style={{ marginLeft: '10px' }} className="fa fa-edit" onClick={() => setEdit(true)}></i>
                    </p>}
            </div>
        </div>)}

       
    </div>
}

