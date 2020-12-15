//******************************************************************************************************
//  RadioBox.tsx - Gbtc
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

export default function ArrayRadioSelect<T>(props: {
    Record: T;
    Field: keyof T;
    Setter: (record: T) => void;
    Options: { ID: any; Label: string }[];
    Label?: string;
}) {
    
    return (
        <div className="form-group">
            <label>{props.Label == null ? props.Field : props.Label}</label>
            <br />
            {props.Options.map((cb, i) => (
                <div key={i} className="form-check form-check-inline">
                    <input
                        className="form-check-input"
                        type="radio"
                        checked={(props.Record[props.Field] as any) == cb.ID}
                        onChange={(evt) =>
                            props.Setter({ ...props.Record, [props.Field]: cb.ID })
                        }
                    />
                    <label className="form-check-label">{cb.Label}</label>
                </div>
            ))}
        </div>
    );
}

