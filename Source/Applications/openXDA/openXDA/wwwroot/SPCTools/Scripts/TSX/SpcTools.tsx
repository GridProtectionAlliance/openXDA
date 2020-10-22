//******************************************************************************************************
//  SPCTools.tsx - Gbtc
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
import NavBar from './NavBar';

declare var homePath: string;
declare var userIsAdmin: boolean;

type Page = ('Home'|'Static'|'Dynamic'|'Meter'|'Channel')
const SPCTools: React.FunctionComponent = (props: {}) => {
    const [page, setPage] = React.useState<Page>('Home');

    return (
        <>
            <NavBar page={page} pageSetter={(pg) => setPage(pg as Page)} />
            <div className="container theme-showcase" role="main" id="bodyContainer">
                <div className="screen" style={{ height: (window.innerHeight - 60), width: window.innerWidth, position: 'absolute', top: '60px' }}>
                    <div className="row" style={{ height: '100%' }}>
                        <div className="col" style={{ width: '100%', height: 'inherit', padding: '0 0 0 0', overflow: 'hidden' }}>
                            <React.Suspense fallback={<div>Loading...</div>}>
                                {(page == 'Home' ? <p> Home </p> : null)}
                                {(page == 'Static' ? <p> New static Alarm </p> : null)}
                                {(page == 'Dynamic' ? <p> New Dynamic Alarm </p> : null)}
                                {(page == 'Meter' ? <p> Meter Overview </p> : null)}
                                {(page == 'Channel' ? <p> Channel Overview </p> : null)}                               
                            </React.Suspense>
                        </div>
                    </div>
                </div>
            </div>
        </>
       
    );
}

ReactDOM.render(<SPCTools/>, document.getElementById('body'));
