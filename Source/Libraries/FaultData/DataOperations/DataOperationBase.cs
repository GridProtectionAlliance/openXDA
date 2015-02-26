//******************************************************************************************************
//  DataOperationBase.cs - Gbtc
//
//  Copyright © 2015, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  02/26/2015 - Stephen C. Wills
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaultData.Database;
using FaultData.DataSets;
using GSF;

namespace FaultData.DataOperations
{
    public abstract class DataOperationBase<T> : IDataOperation where T : class, IDataSet
    {
        public event EventHandler<EventArgs<string>> StatusMessage;
        public event EventHandler<EventArgs<Exception>> ProcessException;

        public abstract void Prepare(DbAdapterContainer dbAdapterContainer);
        public abstract void Execute(T dataSet);
        public abstract void Load(DbAdapterContainer dbAdapterContainer);

        public void Execute(IDataSet dataSet)
        {
            T dataSetAsT = dataSet as T;

            if ((object)dataSetAsT != null)
                Execute(dataSetAsT);
        }

        protected void OnStatusMessage(string message)
        {
            if ((object)StatusMessage != null)
                StatusMessage(this, new EventArgs<string>(message));
        }

        protected void OnProcessException(Exception ex)
        {
            if ((object)ProcessException != null)
                ProcessException(this, new EventArgs<Exception>(ex));
        }
    }
}
