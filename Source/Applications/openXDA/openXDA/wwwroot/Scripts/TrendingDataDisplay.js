/******/ (function(modules) { // webpackBootstrap
/******/ 	// The module cache
/******/ 	var installedModules = {};
/******/
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/
/******/ 		// Check if module is in cache
/******/ 		if(installedModules[moduleId]) {
/******/ 			return installedModules[moduleId].exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = installedModules[moduleId] = {
/******/ 			i: moduleId,
/******/ 			l: false,
/******/ 			exports: {}
/******/ 		};
/******/
/******/ 		// Execute the module function
/******/ 		modules[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/
/******/ 		// Flag the module as loaded
/******/ 		module.l = true;
/******/
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/
/******/
/******/ 	// expose the modules object (__webpack_modules__)
/******/ 	__webpack_require__.m = modules;
/******/
/******/ 	// expose the module cache
/******/ 	__webpack_require__.c = installedModules;
/******/
/******/ 	// define getter function for harmony exports
/******/ 	__webpack_require__.d = function(exports, name, getter) {
/******/ 		if(!__webpack_require__.o(exports, name)) {
/******/ 			Object.defineProperty(exports, name, { enumerable: true, get: getter });
/******/ 		}
/******/ 	};
/******/
/******/ 	// define __esModule on exports
/******/ 	__webpack_require__.r = function(exports) {
/******/ 		if(typeof Symbol !== 'undefined' && Symbol.toStringTag) {
/******/ 			Object.defineProperty(exports, Symbol.toStringTag, { value: 'Module' });
/******/ 		}
/******/ 		Object.defineProperty(exports, '__esModule', { value: true });
/******/ 	};
/******/
/******/ 	// create a fake namespace object
/******/ 	// mode & 1: value is a module id, require it
/******/ 	// mode & 2: merge all properties of value into the ns
/******/ 	// mode & 4: return value when already ns object
/******/ 	// mode & 8|1: behave like require
/******/ 	__webpack_require__.t = function(value, mode) {
/******/ 		if(mode & 1) value = __webpack_require__(value);
/******/ 		if(mode & 8) return value;
/******/ 		if((mode & 4) && typeof value === 'object' && value && value.__esModule) return value;
/******/ 		var ns = Object.create(null);
/******/ 		__webpack_require__.r(ns);
/******/ 		Object.defineProperty(ns, 'default', { enumerable: true, value: value });
/******/ 		if(mode & 2 && typeof value != 'string') for(var key in value) __webpack_require__.d(ns, key, function(key) { return value[key]; }.bind(null, key));
/******/ 		return ns;
/******/ 	};
/******/
/******/ 	// getDefaultExport function for compatibility with non-harmony modules
/******/ 	__webpack_require__.n = function(module) {
/******/ 		var getter = module && module.__esModule ?
/******/ 			function getDefault() { return module['default']; } :
/******/ 			function getModuleExports() { return module; };
/******/ 		__webpack_require__.d(getter, 'a', getter);
/******/ 		return getter;
/******/ 	};
/******/
/******/ 	// Object.prototype.hasOwnProperty.call
/******/ 	__webpack_require__.o = function(object, property) { return Object.prototype.hasOwnProperty.call(object, property); };
/******/
/******/ 	// __webpack_public_path__
/******/ 	__webpack_require__.p = "";
/******/
/******/
/******/ 	// Load entry module and return exports
/******/ 	return __webpack_require__(__webpack_require__.s = "./TSX/TrendingDataDisplay.tsx");
/******/ })
/************************************************************************/
/******/ ({

/***/ "../../node_modules/@gpa-gemstone/helper-functions/lib/CreateGuid.js":
/*!***********************************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/CreateGuid.js ***!
  \***********************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

// ******************************************************************************************************
//  CreateGuid.ts - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  https://stackoverflow.com/questions/105034/how-to-create-a-guid-uuid
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  01/04/2021 - Billy Ernest
//       Generated original version of source code.
//
// ******************************************************************************************************
Object.defineProperty(exports, "__esModule", { value: true });
exports.CreateGuid = void 0;
/**
 * This function generates a GUID
 */
function CreateGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0;
        var v = c === 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}
exports.CreateGuid = CreateGuid;


/***/ }),

/***/ "../../node_modules/@gpa-gemstone/helper-functions/lib/GetNodeSize.js":
/*!************************************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/GetNodeSize.js ***!
  \************************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

// ******************************************************************************************************
//  GetNodeSize.tsx - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  01/15/2021 - C. Lackner
//       Generated original version of source code.
//
// ******************************************************************************************************
Object.defineProperty(exports, "__esModule", { value: true });
exports.GetNodeSize = void 0;
/**
 * GetNodeSize returns the dimensions of an html element
 * @param node: a HTML element, or null can be passed through
 */
function GetNodeSize(node) {
    if (node === null)
        return {
            height: 0,
            width: 0,
            top: 0,
            left: 0,
        };
    var _a = node.getBoundingClientRect(), height = _a.height, width = _a.width, top = _a.top, left = _a.left;
    return {
        height: parseInt(height.toString(), 10),
        width: parseInt(width.toString(), 10),
        top: parseInt(top.toString(), 10),
        left: parseInt(left.toString(), 10),
    };
}
exports.GetNodeSize = GetNodeSize;


/***/ }),

/***/ "../../node_modules/@gpa-gemstone/helper-functions/lib/GetTextHeight.js":
/*!**************************************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/GetTextHeight.js ***!
  \**************************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

// ******************************************************************************************************
//  GetTextHeight.tsx - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  03/12/2021 - c. Lackner
//       Generated original version of source code.
//
// ******************************************************************************************************
Object.defineProperty(exports, "__esModule", { value: true });
exports.GetTextHeight = void 0;
/**
 * This function returns the height of a piece of text given a font, fontsize, and a word
 * @param font: Determines font of given text
 * @param fontSize: Determines size of given font
 * @param word: Text to measure
 */
function GetTextHeight(font, fontSize, word) {
    var text = document.createElement("span");
    document.body.appendChild(text);
    text.style.font = font;
    text.style.fontSize = fontSize;
    text.style.height = 'auto';
    text.style.width = 'auto';
    text.style.position = 'absolute';
    text.style.whiteSpace = 'no-wrap';
    text.innerHTML = word;
    var height = Math.ceil(text.clientHeight);
    document.body.removeChild(text);
    return height;
}
exports.GetTextHeight = GetTextHeight;


/***/ }),

/***/ "../../node_modules/@gpa-gemstone/helper-functions/lib/GetTextWidth.js":
/*!*************************************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/GetTextWidth.js ***!
  \*************************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

// ******************************************************************************************************
//  GetTextWidth.tsx - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  01/07/2021 - Billy Ernest
//       Generated original version of source code.
//
// ******************************************************************************************************
Object.defineProperty(exports, "__esModule", { value: true });
exports.GetTextWidth = void 0;
/**
 * GetTextWidth returns the width of a piece of text given its font, fontSize, and content.
 * @param font: Determines font of given text
 * @param fontSize: Determines size of given font
 * @param word: Text to measure
 */
function GetTextWidth(font, fontSize, word) {
    var text = document.createElement("span");
    document.body.appendChild(text);
    text.style.font = font;
    text.style.fontSize = fontSize;
    text.style.height = 'auto';
    text.style.width = 'auto';
    text.style.position = 'absolute';
    text.style.whiteSpace = 'no-wrap';
    text.innerHTML = word;
    var width = Math.ceil(text.clientWidth);
    document.body.removeChild(text);
    return width;
}
exports.GetTextWidth = GetTextWidth;


/***/ }),

/***/ "../../node_modules/@gpa-gemstone/helper-functions/lib/IsInteger.js":
/*!**********************************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/IsInteger.js ***!
  \**********************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

// ******************************************************************************************************
//  IsInteger.tsx - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  07/16/2021 - C. Lackner
//       Generated original version of source code.
//
// ******************************************************************************************************
Object.defineProperty(exports, "__esModule", { value: true });
exports.IsInteger = void 0;
/**
 * IsInteger checks if value passed through is an integer, returning a true or false
 * @param value: value is the input passed through the IsInteger function
 */
function IsInteger(value) {
    var regex = /^-?[0-9]+$/;
    return value.toString().match(regex) != null;
}
exports.IsInteger = IsInteger;


/***/ }),

/***/ "../../node_modules/@gpa-gemstone/helper-functions/lib/IsNumber.js":
/*!*********************************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/IsNumber.js ***!
  \*********************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

// ******************************************************************************************************
//  IsNumber.tsx - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  01/07/2021 - Billy Ernest
//       Generated original version of source code.
//
// ******************************************************************************************************
Object.defineProperty(exports, "__esModule", { value: true });
exports.IsNumber = void 0;
/**
 * This function checks if any value is a number, returning true or false
 * @param value: value is the input passed through the IsNumber function
 */
function IsNumber(value) {
    var regex = /^-?[0-9]+(\.[0-9]+)?$/;
    return value.toString().match(regex) != null;
}
exports.IsNumber = IsNumber;


/***/ }),

/***/ "../../node_modules/@gpa-gemstone/helper-functions/lib/RandomColor.js":
/*!************************************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/RandomColor.js ***!
  \************************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

// ******************************************************************************************************
//  RandomColor.tsx - Gbtc
//
//  Copyright © 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  01/15/2021 - C. Lackner
//       Generated original version of source code.
//
// ******************************************************************************************************
Object.defineProperty(exports, "__esModule", { value: true });
exports.RandomColor = void 0;
/**
 * This function returns a random color
 */
function RandomColor() {
    return '#' + Math.random().toString(16).substr(2, 6).toUpperCase();
}
exports.RandomColor = RandomColor;


/***/ }),

/***/ "../../node_modules/@gpa-gemstone/helper-functions/lib/index.js":
/*!******************************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/index.js ***!
  \******************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

// ******************************************************************************************************
//  index.ts - Gbtc
//
//  Copyright � 2021, Grid Protection Alliance.  All Rights Reserved.
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
//  https://stackoverflow.com/questions/105034/how-to-create-a-guid-uuid
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  01/04/2021 - Billy Ernest
//       Generated original version of source code.
//
// ******************************************************************************************************
Object.defineProperty(exports, "__esModule", { value: true });
exports.IsInteger = exports.IsNumber = exports.GetTextHeight = exports.RandomColor = exports.GetNodeSize = exports.GetTextWidth = exports.CreateGuid = void 0;
var CreateGuid_1 = __webpack_require__(/*! ./CreateGuid */ "../../node_modules/@gpa-gemstone/helper-functions/lib/CreateGuid.js");
Object.defineProperty(exports, "CreateGuid", { enumerable: true, get: function () { return CreateGuid_1.CreateGuid; } });
var GetTextWidth_1 = __webpack_require__(/*! ./GetTextWidth */ "../../node_modules/@gpa-gemstone/helper-functions/lib/GetTextWidth.js");
Object.defineProperty(exports, "GetTextWidth", { enumerable: true, get: function () { return GetTextWidth_1.GetTextWidth; } });
var GetTextHeight_1 = __webpack_require__(/*! ./GetTextHeight */ "../../node_modules/@gpa-gemstone/helper-functions/lib/GetTextHeight.js");
Object.defineProperty(exports, "GetTextHeight", { enumerable: true, get: function () { return GetTextHeight_1.GetTextHeight; } });
var GetNodeSize_1 = __webpack_require__(/*! ./GetNodeSize */ "../../node_modules/@gpa-gemstone/helper-functions/lib/GetNodeSize.js");
Object.defineProperty(exports, "GetNodeSize", { enumerable: true, get: function () { return GetNodeSize_1.GetNodeSize; } });
var RandomColor_1 = __webpack_require__(/*! ./RandomColor */ "../../node_modules/@gpa-gemstone/helper-functions/lib/RandomColor.js");
Object.defineProperty(exports, "RandomColor", { enumerable: true, get: function () { return RandomColor_1.RandomColor; } });
var IsNumber_1 = __webpack_require__(/*! ./IsNumber */ "../../node_modules/@gpa-gemstone/helper-functions/lib/IsNumber.js");
Object.defineProperty(exports, "IsNumber", { enumerable: true, get: function () { return IsNumber_1.IsNumber; } });
var IsInteger_1 = __webpack_require__(/*! ./IsInteger */ "../../node_modules/@gpa-gemstone/helper-functions/lib/IsInteger.js");
Object.defineProperty(exports, "IsInteger", { enumerable: true, get: function () { return IsInteger_1.IsInteger; } });


/***/ }),

/***/ "../../node_modules/create-react-class/factory.js":
/*!****************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/create-react-class/factory.js ***!
  \****************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 *
 */



var _assign = __webpack_require__(/*! object-assign */ "../../node_modules/object-assign/index.js");

var emptyObject = __webpack_require__(/*! fbjs/lib/emptyObject */ "../../node_modules/fbjs/lib/emptyObject.js");
var _invariant = __webpack_require__(/*! fbjs/lib/invariant */ "../../node_modules/fbjs/lib/invariant.js");

if (true) {
  var warning = __webpack_require__(/*! fbjs/lib/warning */ "../../node_modules/fbjs/lib/warning.js");
}

var MIXINS_KEY = 'mixins';

// Helper function to allow the creation of anonymous functions which do not
// have .name set to the name of the variable being assigned to.
function identity(fn) {
  return fn;
}

var ReactPropTypeLocationNames;
if (true) {
  ReactPropTypeLocationNames = {
    prop: 'prop',
    context: 'context',
    childContext: 'child context'
  };
} else {}

function factory(ReactComponent, isValidElement, ReactNoopUpdateQueue) {
  /**
   * Policies that describe methods in `ReactClassInterface`.
   */

  var injectedMixins = [];

  /**
   * Composite components are higher-level components that compose other composite
   * or host components.
   *
   * To create a new type of `ReactClass`, pass a specification of
   * your new class to `React.createClass`. The only requirement of your class
   * specification is that you implement a `render` method.
   *
   *   var MyComponent = React.createClass({
   *     render: function() {
   *       return <div>Hello World</div>;
   *     }
   *   });
   *
   * The class specification supports a specific protocol of methods that have
   * special meaning (e.g. `render`). See `ReactClassInterface` for
   * more the comprehensive protocol. Any other properties and methods in the
   * class specification will be available on the prototype.
   *
   * @interface ReactClassInterface
   * @internal
   */
  var ReactClassInterface = {
    /**
     * An array of Mixin objects to include when defining your component.
     *
     * @type {array}
     * @optional
     */
    mixins: 'DEFINE_MANY',

    /**
     * An object containing properties and methods that should be defined on
     * the component's constructor instead of its prototype (static methods).
     *
     * @type {object}
     * @optional
     */
    statics: 'DEFINE_MANY',

    /**
     * Definition of prop types for this component.
     *
     * @type {object}
     * @optional
     */
    propTypes: 'DEFINE_MANY',

    /**
     * Definition of context types for this component.
     *
     * @type {object}
     * @optional
     */
    contextTypes: 'DEFINE_MANY',

    /**
     * Definition of context types this component sets for its children.
     *
     * @type {object}
     * @optional
     */
    childContextTypes: 'DEFINE_MANY',

    // ==== Definition methods ====

    /**
     * Invoked when the component is mounted. Values in the mapping will be set on
     * `this.props` if that prop is not specified (i.e. using an `in` check).
     *
     * This method is invoked before `getInitialState` and therefore cannot rely
     * on `this.state` or use `this.setState`.
     *
     * @return {object}
     * @optional
     */
    getDefaultProps: 'DEFINE_MANY_MERGED',

    /**
     * Invoked once before the component is mounted. The return value will be used
     * as the initial value of `this.state`.
     *
     *   getInitialState: function() {
     *     return {
     *       isOn: false,
     *       fooBaz: new BazFoo()
     *     }
     *   }
     *
     * @return {object}
     * @optional
     */
    getInitialState: 'DEFINE_MANY_MERGED',

    /**
     * @return {object}
     * @optional
     */
    getChildContext: 'DEFINE_MANY_MERGED',

    /**
     * Uses props from `this.props` and state from `this.state` to render the
     * structure of the component.
     *
     * No guarantees are made about when or how often this method is invoked, so
     * it must not have side effects.
     *
     *   render: function() {
     *     var name = this.props.name;
     *     return <div>Hello, {name}!</div>;
     *   }
     *
     * @return {ReactComponent}
     * @required
     */
    render: 'DEFINE_ONCE',

    // ==== Delegate methods ====

    /**
     * Invoked when the component is initially created and about to be mounted.
     * This may have side effects, but any external subscriptions or data created
     * by this method must be cleaned up in `componentWillUnmount`.
     *
     * @optional
     */
    componentWillMount: 'DEFINE_MANY',

    /**
     * Invoked when the component has been mounted and has a DOM representation.
     * However, there is no guarantee that the DOM node is in the document.
     *
     * Use this as an opportunity to operate on the DOM when the component has
     * been mounted (initialized and rendered) for the first time.
     *
     * @param {DOMElement} rootNode DOM element representing the component.
     * @optional
     */
    componentDidMount: 'DEFINE_MANY',

    /**
     * Invoked before the component receives new props.
     *
     * Use this as an opportunity to react to a prop transition by updating the
     * state using `this.setState`. Current props are accessed via `this.props`.
     *
     *   componentWillReceiveProps: function(nextProps, nextContext) {
     *     this.setState({
     *       likesIncreasing: nextProps.likeCount > this.props.likeCount
     *     });
     *   }
     *
     * NOTE: There is no equivalent `componentWillReceiveState`. An incoming prop
     * transition may cause a state change, but the opposite is not true. If you
     * need it, you are probably looking for `componentWillUpdate`.
     *
     * @param {object} nextProps
     * @optional
     */
    componentWillReceiveProps: 'DEFINE_MANY',

    /**
     * Invoked while deciding if the component should be updated as a result of
     * receiving new props, state and/or context.
     *
     * Use this as an opportunity to `return false` when you're certain that the
     * transition to the new props/state/context will not require a component
     * update.
     *
     *   shouldComponentUpdate: function(nextProps, nextState, nextContext) {
     *     return !equal(nextProps, this.props) ||
     *       !equal(nextState, this.state) ||
     *       !equal(nextContext, this.context);
     *   }
     *
     * @param {object} nextProps
     * @param {?object} nextState
     * @param {?object} nextContext
     * @return {boolean} True if the component should update.
     * @optional
     */
    shouldComponentUpdate: 'DEFINE_ONCE',

    /**
     * Invoked when the component is about to update due to a transition from
     * `this.props`, `this.state` and `this.context` to `nextProps`, `nextState`
     * and `nextContext`.
     *
     * Use this as an opportunity to perform preparation before an update occurs.
     *
     * NOTE: You **cannot** use `this.setState()` in this method.
     *
     * @param {object} nextProps
     * @param {?object} nextState
     * @param {?object} nextContext
     * @param {ReactReconcileTransaction} transaction
     * @optional
     */
    componentWillUpdate: 'DEFINE_MANY',

    /**
     * Invoked when the component's DOM representation has been updated.
     *
     * Use this as an opportunity to operate on the DOM when the component has
     * been updated.
     *
     * @param {object} prevProps
     * @param {?object} prevState
     * @param {?object} prevContext
     * @param {DOMElement} rootNode DOM element representing the component.
     * @optional
     */
    componentDidUpdate: 'DEFINE_MANY',

    /**
     * Invoked when the component is about to be removed from its parent and have
     * its DOM representation destroyed.
     *
     * Use this as an opportunity to deallocate any external resources.
     *
     * NOTE: There is no `componentDidUnmount` since your component will have been
     * destroyed by that point.
     *
     * @optional
     */
    componentWillUnmount: 'DEFINE_MANY',

    /**
     * Replacement for (deprecated) `componentWillMount`.
     *
     * @optional
     */
    UNSAFE_componentWillMount: 'DEFINE_MANY',

    /**
     * Replacement for (deprecated) `componentWillReceiveProps`.
     *
     * @optional
     */
    UNSAFE_componentWillReceiveProps: 'DEFINE_MANY',

    /**
     * Replacement for (deprecated) `componentWillUpdate`.
     *
     * @optional
     */
    UNSAFE_componentWillUpdate: 'DEFINE_MANY',

    // ==== Advanced methods ====

    /**
     * Updates the component's currently mounted DOM representation.
     *
     * By default, this implements React's rendering and reconciliation algorithm.
     * Sophisticated clients may wish to override this.
     *
     * @param {ReactReconcileTransaction} transaction
     * @internal
     * @overridable
     */
    updateComponent: 'OVERRIDE_BASE'
  };

  /**
   * Similar to ReactClassInterface but for static methods.
   */
  var ReactClassStaticInterface = {
    /**
     * This method is invoked after a component is instantiated and when it
     * receives new props. Return an object to update state in response to
     * prop changes. Return null to indicate no change to state.
     *
     * If an object is returned, its keys will be merged into the existing state.
     *
     * @return {object || null}
     * @optional
     */
    getDerivedStateFromProps: 'DEFINE_MANY_MERGED'
  };

  /**
   * Mapping from class specification keys to special processing functions.
   *
   * Although these are declared like instance properties in the specification
   * when defining classes using `React.createClass`, they are actually static
   * and are accessible on the constructor instead of the prototype. Despite
   * being static, they must be defined outside of the "statics" key under
   * which all other static methods are defined.
   */
  var RESERVED_SPEC_KEYS = {
    displayName: function(Constructor, displayName) {
      Constructor.displayName = displayName;
    },
    mixins: function(Constructor, mixins) {
      if (mixins) {
        for (var i = 0; i < mixins.length; i++) {
          mixSpecIntoComponent(Constructor, mixins[i]);
        }
      }
    },
    childContextTypes: function(Constructor, childContextTypes) {
      if (true) {
        validateTypeDef(Constructor, childContextTypes, 'childContext');
      }
      Constructor.childContextTypes = _assign(
        {},
        Constructor.childContextTypes,
        childContextTypes
      );
    },
    contextTypes: function(Constructor, contextTypes) {
      if (true) {
        validateTypeDef(Constructor, contextTypes, 'context');
      }
      Constructor.contextTypes = _assign(
        {},
        Constructor.contextTypes,
        contextTypes
      );
    },
    /**
     * Special case getDefaultProps which should move into statics but requires
     * automatic merging.
     */
    getDefaultProps: function(Constructor, getDefaultProps) {
      if (Constructor.getDefaultProps) {
        Constructor.getDefaultProps = createMergedResultFunction(
          Constructor.getDefaultProps,
          getDefaultProps
        );
      } else {
        Constructor.getDefaultProps = getDefaultProps;
      }
    },
    propTypes: function(Constructor, propTypes) {
      if (true) {
        validateTypeDef(Constructor, propTypes, 'prop');
      }
      Constructor.propTypes = _assign({}, Constructor.propTypes, propTypes);
    },
    statics: function(Constructor, statics) {
      mixStaticSpecIntoComponent(Constructor, statics);
    },
    autobind: function() {}
  };

  function validateTypeDef(Constructor, typeDef, location) {
    for (var propName in typeDef) {
      if (typeDef.hasOwnProperty(propName)) {
        // use a warning instead of an _invariant so components
        // don't show up in prod but only in __DEV__
        if (true) {
          warning(
            typeof typeDef[propName] === 'function',
            '%s: %s type `%s` is invalid; it must be a function, usually from ' +
              'React.PropTypes.',
            Constructor.displayName || 'ReactClass',
            ReactPropTypeLocationNames[location],
            propName
          );
        }
      }
    }
  }

  function validateMethodOverride(isAlreadyDefined, name) {
    var specPolicy = ReactClassInterface.hasOwnProperty(name)
      ? ReactClassInterface[name]
      : null;

    // Disallow overriding of base class methods unless explicitly allowed.
    if (ReactClassMixin.hasOwnProperty(name)) {
      _invariant(
        specPolicy === 'OVERRIDE_BASE',
        'ReactClassInterface: You are attempting to override ' +
          '`%s` from your class specification. Ensure that your method names ' +
          'do not overlap with React methods.',
        name
      );
    }

    // Disallow defining methods more than once unless explicitly allowed.
    if (isAlreadyDefined) {
      _invariant(
        specPolicy === 'DEFINE_MANY' || specPolicy === 'DEFINE_MANY_MERGED',
        'ReactClassInterface: You are attempting to define ' +
          '`%s` on your component more than once. This conflict may be due ' +
          'to a mixin.',
        name
      );
    }
  }

  /**
   * Mixin helper which handles policy validation and reserved
   * specification keys when building React classes.
   */
  function mixSpecIntoComponent(Constructor, spec) {
    if (!spec) {
      if (true) {
        var typeofSpec = typeof spec;
        var isMixinValid = typeofSpec === 'object' && spec !== null;

        if (true) {
          warning(
            isMixinValid,
            "%s: You're attempting to include a mixin that is either null " +
              'or not an object. Check the mixins included by the component, ' +
              'as well as any mixins they include themselves. ' +
              'Expected object but got %s.',
            Constructor.displayName || 'ReactClass',
            spec === null ? null : typeofSpec
          );
        }
      }

      return;
    }

    _invariant(
      typeof spec !== 'function',
      "ReactClass: You're attempting to " +
        'use a component class or function as a mixin. Instead, just use a ' +
        'regular object.'
    );
    _invariant(
      !isValidElement(spec),
      "ReactClass: You're attempting to " +
        'use a component as a mixin. Instead, just use a regular object.'
    );

    var proto = Constructor.prototype;
    var autoBindPairs = proto.__reactAutoBindPairs;

    // By handling mixins before any other properties, we ensure the same
    // chaining order is applied to methods with DEFINE_MANY policy, whether
    // mixins are listed before or after these methods in the spec.
    if (spec.hasOwnProperty(MIXINS_KEY)) {
      RESERVED_SPEC_KEYS.mixins(Constructor, spec.mixins);
    }

    for (var name in spec) {
      if (!spec.hasOwnProperty(name)) {
        continue;
      }

      if (name === MIXINS_KEY) {
        // We have already handled mixins in a special case above.
        continue;
      }

      var property = spec[name];
      var isAlreadyDefined = proto.hasOwnProperty(name);
      validateMethodOverride(isAlreadyDefined, name);

      if (RESERVED_SPEC_KEYS.hasOwnProperty(name)) {
        RESERVED_SPEC_KEYS[name](Constructor, property);
      } else {
        // Setup methods on prototype:
        // The following member methods should not be automatically bound:
        // 1. Expected ReactClass methods (in the "interface").
        // 2. Overridden methods (that were mixed in).
        var isReactClassMethod = ReactClassInterface.hasOwnProperty(name);
        var isFunction = typeof property === 'function';
        var shouldAutoBind =
          isFunction &&
          !isReactClassMethod &&
          !isAlreadyDefined &&
          spec.autobind !== false;

        if (shouldAutoBind) {
          autoBindPairs.push(name, property);
          proto[name] = property;
        } else {
          if (isAlreadyDefined) {
            var specPolicy = ReactClassInterface[name];

            // These cases should already be caught by validateMethodOverride.
            _invariant(
              isReactClassMethod &&
                (specPolicy === 'DEFINE_MANY_MERGED' ||
                  specPolicy === 'DEFINE_MANY'),
              'ReactClass: Unexpected spec policy %s for key %s ' +
                'when mixing in component specs.',
              specPolicy,
              name
            );

            // For methods which are defined more than once, call the existing
            // methods before calling the new property, merging if appropriate.
            if (specPolicy === 'DEFINE_MANY_MERGED') {
              proto[name] = createMergedResultFunction(proto[name], property);
            } else if (specPolicy === 'DEFINE_MANY') {
              proto[name] = createChainedFunction(proto[name], property);
            }
          } else {
            proto[name] = property;
            if (true) {
              // Add verbose displayName to the function, which helps when looking
              // at profiling tools.
              if (typeof property === 'function' && spec.displayName) {
                proto[name].displayName = spec.displayName + '_' + name;
              }
            }
          }
        }
      }
    }
  }

  function mixStaticSpecIntoComponent(Constructor, statics) {
    if (!statics) {
      return;
    }

    for (var name in statics) {
      var property = statics[name];
      if (!statics.hasOwnProperty(name)) {
        continue;
      }

      var isReserved = name in RESERVED_SPEC_KEYS;
      _invariant(
        !isReserved,
        'ReactClass: You are attempting to define a reserved ' +
          'property, `%s`, that shouldn\'t be on the "statics" key. Define it ' +
          'as an instance property instead; it will still be accessible on the ' +
          'constructor.',
        name
      );

      var isAlreadyDefined = name in Constructor;
      if (isAlreadyDefined) {
        var specPolicy = ReactClassStaticInterface.hasOwnProperty(name)
          ? ReactClassStaticInterface[name]
          : null;

        _invariant(
          specPolicy === 'DEFINE_MANY_MERGED',
          'ReactClass: You are attempting to define ' +
            '`%s` on your component more than once. This conflict may be ' +
            'due to a mixin.',
          name
        );

        Constructor[name] = createMergedResultFunction(Constructor[name], property);

        return;
      }

      Constructor[name] = property;
    }
  }

  /**
   * Merge two objects, but throw if both contain the same key.
   *
   * @param {object} one The first object, which is mutated.
   * @param {object} two The second object
   * @return {object} one after it has been mutated to contain everything in two.
   */
  function mergeIntoWithNoDuplicateKeys(one, two) {
    _invariant(
      one && two && typeof one === 'object' && typeof two === 'object',
      'mergeIntoWithNoDuplicateKeys(): Cannot merge non-objects.'
    );

    for (var key in two) {
      if (two.hasOwnProperty(key)) {
        _invariant(
          one[key] === undefined,
          'mergeIntoWithNoDuplicateKeys(): ' +
            'Tried to merge two objects with the same key: `%s`. This conflict ' +
            'may be due to a mixin; in particular, this may be caused by two ' +
            'getInitialState() or getDefaultProps() methods returning objects ' +
            'with clashing keys.',
          key
        );
        one[key] = two[key];
      }
    }
    return one;
  }

  /**
   * Creates a function that invokes two functions and merges their return values.
   *
   * @param {function} one Function to invoke first.
   * @param {function} two Function to invoke second.
   * @return {function} Function that invokes the two argument functions.
   * @private
   */
  function createMergedResultFunction(one, two) {
    return function mergedResult() {
      var a = one.apply(this, arguments);
      var b = two.apply(this, arguments);
      if (a == null) {
        return b;
      } else if (b == null) {
        return a;
      }
      var c = {};
      mergeIntoWithNoDuplicateKeys(c, a);
      mergeIntoWithNoDuplicateKeys(c, b);
      return c;
    };
  }

  /**
   * Creates a function that invokes two functions and ignores their return vales.
   *
   * @param {function} one Function to invoke first.
   * @param {function} two Function to invoke second.
   * @return {function} Function that invokes the two argument functions.
   * @private
   */
  function createChainedFunction(one, two) {
    return function chainedFunction() {
      one.apply(this, arguments);
      two.apply(this, arguments);
    };
  }

  /**
   * Binds a method to the component.
   *
   * @param {object} component Component whose method is going to be bound.
   * @param {function} method Method to be bound.
   * @return {function} The bound method.
   */
  function bindAutoBindMethod(component, method) {
    var boundMethod = method.bind(component);
    if (true) {
      boundMethod.__reactBoundContext = component;
      boundMethod.__reactBoundMethod = method;
      boundMethod.__reactBoundArguments = null;
      var componentName = component.constructor.displayName;
      var _bind = boundMethod.bind;
      boundMethod.bind = function(newThis) {
        for (
          var _len = arguments.length,
            args = Array(_len > 1 ? _len - 1 : 0),
            _key = 1;
          _key < _len;
          _key++
        ) {
          args[_key - 1] = arguments[_key];
        }

        // User is trying to bind() an autobound method; we effectively will
        // ignore the value of "this" that the user is trying to use, so
        // let's warn.
        if (newThis !== component && newThis !== null) {
          if (true) {
            warning(
              false,
              'bind(): React component methods may only be bound to the ' +
                'component instance. See %s',
              componentName
            );
          }
        } else if (!args.length) {
          if (true) {
            warning(
              false,
              'bind(): You are binding a component method to the component. ' +
                'React does this for you automatically in a high-performance ' +
                'way, so you can safely remove this call. See %s',
              componentName
            );
          }
          return boundMethod;
        }
        var reboundMethod = _bind.apply(boundMethod, arguments);
        reboundMethod.__reactBoundContext = component;
        reboundMethod.__reactBoundMethod = method;
        reboundMethod.__reactBoundArguments = args;
        return reboundMethod;
      };
    }
    return boundMethod;
  }

  /**
   * Binds all auto-bound methods in a component.
   *
   * @param {object} component Component whose method is going to be bound.
   */
  function bindAutoBindMethods(component) {
    var pairs = component.__reactAutoBindPairs;
    for (var i = 0; i < pairs.length; i += 2) {
      var autoBindKey = pairs[i];
      var method = pairs[i + 1];
      component[autoBindKey] = bindAutoBindMethod(component, method);
    }
  }

  var IsMountedPreMixin = {
    componentDidMount: function() {
      this.__isMounted = true;
    }
  };

  var IsMountedPostMixin = {
    componentWillUnmount: function() {
      this.__isMounted = false;
    }
  };

  /**
   * Add more to the ReactClass base class. These are all legacy features and
   * therefore not already part of the modern ReactComponent.
   */
  var ReactClassMixin = {
    /**
     * TODO: This will be deprecated because state should always keep a consistent
     * type signature and the only use case for this, is to avoid that.
     */
    replaceState: function(newState, callback) {
      this.updater.enqueueReplaceState(this, newState, callback);
    },

    /**
     * Checks whether or not this composite component is mounted.
     * @return {boolean} True if mounted, false otherwise.
     * @protected
     * @final
     */
    isMounted: function() {
      if (true) {
        warning(
          this.__didWarnIsMounted,
          '%s: isMounted is deprecated. Instead, make sure to clean up ' +
            'subscriptions and pending requests in componentWillUnmount to ' +
            'prevent memory leaks.',
          (this.constructor && this.constructor.displayName) ||
            this.name ||
            'Component'
        );
        this.__didWarnIsMounted = true;
      }
      return !!this.__isMounted;
    }
  };

  var ReactClassComponent = function() {};
  _assign(
    ReactClassComponent.prototype,
    ReactComponent.prototype,
    ReactClassMixin
  );

  /**
   * Creates a composite component class given a class specification.
   * See https://facebook.github.io/react/docs/top-level-api.html#react.createclass
   *
   * @param {object} spec Class specification (which must define `render`).
   * @return {function} Component constructor function.
   * @public
   */
  function createClass(spec) {
    // To keep our warnings more understandable, we'll use a little hack here to
    // ensure that Constructor.name !== 'Constructor'. This makes sure we don't
    // unnecessarily identify a class without displayName as 'Constructor'.
    var Constructor = identity(function(props, context, updater) {
      // This constructor gets overridden by mocks. The argument is used
      // by mocks to assert on what gets mounted.

      if (true) {
        warning(
          this instanceof Constructor,
          'Something is calling a React component directly. Use a factory or ' +
            'JSX instead. See: https://fb.me/react-legacyfactory'
        );
      }

      // Wire up auto-binding
      if (this.__reactAutoBindPairs.length) {
        bindAutoBindMethods(this);
      }

      this.props = props;
      this.context = context;
      this.refs = emptyObject;
      this.updater = updater || ReactNoopUpdateQueue;

      this.state = null;

      // ReactClasses doesn't have constructors. Instead, they use the
      // getInitialState and componentWillMount methods for initialization.

      var initialState = this.getInitialState ? this.getInitialState() : null;
      if (true) {
        // We allow auto-mocks to proceed as if they're returning null.
        if (
          initialState === undefined &&
          this.getInitialState._isMockFunction
        ) {
          // This is probably bad practice. Consider warning here and
          // deprecating this convenience.
          initialState = null;
        }
      }
      _invariant(
        typeof initialState === 'object' && !Array.isArray(initialState),
        '%s.getInitialState(): must return an object or null',
        Constructor.displayName || 'ReactCompositeComponent'
      );

      this.state = initialState;
    });
    Constructor.prototype = new ReactClassComponent();
    Constructor.prototype.constructor = Constructor;
    Constructor.prototype.__reactAutoBindPairs = [];

    injectedMixins.forEach(mixSpecIntoComponent.bind(null, Constructor));

    mixSpecIntoComponent(Constructor, IsMountedPreMixin);
    mixSpecIntoComponent(Constructor, spec);
    mixSpecIntoComponent(Constructor, IsMountedPostMixin);

    // Initialize the defaultProps property after all mixins have been merged.
    if (Constructor.getDefaultProps) {
      Constructor.defaultProps = Constructor.getDefaultProps();
    }

    if (true) {
      // This is a tag to indicate that the use of these method names is ok,
      // since it's used with createClass. If it's not, then it's likely a
      // mistake so we'll warn you to use the static property, property
      // initializer or constructor respectively.
      if (Constructor.getDefaultProps) {
        Constructor.getDefaultProps.isReactClassApproved = {};
      }
      if (Constructor.prototype.getInitialState) {
        Constructor.prototype.getInitialState.isReactClassApproved = {};
      }
    }

    _invariant(
      Constructor.prototype.render,
      'createClass(...): Class specification must implement a `render` method.'
    );

    if (true) {
      warning(
        !Constructor.prototype.componentShouldUpdate,
        '%s has a method called ' +
          'componentShouldUpdate(). Did you mean shouldComponentUpdate()? ' +
          'The name is phrased as a question because the function is ' +
          'expected to return a value.',
        spec.displayName || 'A component'
      );
      warning(
        !Constructor.prototype.componentWillRecieveProps,
        '%s has a method called ' +
          'componentWillRecieveProps(). Did you mean componentWillReceiveProps()?',
        spec.displayName || 'A component'
      );
      warning(
        !Constructor.prototype.UNSAFE_componentWillRecieveProps,
        '%s has a method called UNSAFE_componentWillRecieveProps(). ' +
          'Did you mean UNSAFE_componentWillReceiveProps()?',
        spec.displayName || 'A component'
      );
    }

    // Reduce time spent doing lookups by setting these on the prototype.
    for (var methodName in ReactClassInterface) {
      if (!Constructor.prototype[methodName]) {
        Constructor.prototype[methodName] = null;
      }
    }

    return Constructor;
  }

  return createClass;
}

module.exports = factory;


/***/ }),

/***/ "../../node_modules/create-react-class/index.js":
/*!**************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/create-react-class/index.js ***!
  \**************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 *
 */



var React = __webpack_require__(/*! react */ "react");
var factory = __webpack_require__(/*! ./factory */ "../../node_modules/create-react-class/factory.js");

if (typeof React === 'undefined') {
  throw Error(
    'create-react-class could not find the React object. If you are using script tags, ' +
      'make sure that React is being loaded before create-react-class.'
  );
}

// Hack to grab NoopUpdateQueue from isomorphic React
var ReactNoopUpdateQueue = new React.Component().updater;

module.exports = factory(
  React.Component,
  React.isValidElement,
  ReactNoopUpdateQueue
);


/***/ }),

/***/ "../../node_modules/css-loader/dist/cjs.js!../../node_modules/react-datetime/css/react-datetime.css":
/*!**************************************************************************************************************************************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/css-loader/dist/cjs.js!C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/css/react-datetime.css ***!
  \**************************************************************************************************************************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(/*! ../../css-loader/dist/runtime/api.js */ "../../node_modules/css-loader/dist/runtime/api.js")(false);
// Module
exports.push([module.i, "/*!\n * https://github.com/YouCanBookMe/react-datetime\n */\n\n.rdt {\n  position: relative;\n}\n.rdtPicker {\n  display: none;\n  position: absolute;\n  width: 250px;\n  padding: 4px;\n  margin-top: 1px;\n  z-index: 99999 !important;\n  background: #fff;\n  box-shadow: 0 1px 3px rgba(0,0,0,.1);\n  border: 1px solid #f9f9f9;\n}\n.rdtOpen .rdtPicker {\n  display: block;\n}\n.rdtStatic .rdtPicker {\n  box-shadow: none;\n  position: static;\n}\n\n.rdtPicker .rdtTimeToggle {\n  text-align: center;\n}\n\n.rdtPicker table {\n  width: 100%;\n  margin: 0;\n}\n.rdtPicker td,\n.rdtPicker th {\n  text-align: center;\n  height: 28px;\n}\n.rdtPicker td {\n  cursor: pointer;\n}\n.rdtPicker td.rdtDay:hover,\n.rdtPicker td.rdtHour:hover,\n.rdtPicker td.rdtMinute:hover,\n.rdtPicker td.rdtSecond:hover,\n.rdtPicker .rdtTimeToggle:hover {\n  background: #eeeeee;\n  cursor: pointer;\n}\n.rdtPicker td.rdtOld,\n.rdtPicker td.rdtNew {\n  color: #999999;\n}\n.rdtPicker td.rdtToday {\n  position: relative;\n}\n.rdtPicker td.rdtToday:before {\n  content: '';\n  display: inline-block;\n  border-left: 7px solid transparent;\n  border-bottom: 7px solid #428bca;\n  border-top-color: rgba(0, 0, 0, 0.2);\n  position: absolute;\n  bottom: 4px;\n  right: 4px;\n}\n.rdtPicker td.rdtActive,\n.rdtPicker td.rdtActive:hover {\n  background-color: #428bca;\n  color: #fff;\n  text-shadow: 0 -1px 0 rgba(0, 0, 0, 0.25);\n}\n.rdtPicker td.rdtActive.rdtToday:before {\n  border-bottom-color: #fff;\n}\n.rdtPicker td.rdtDisabled,\n.rdtPicker td.rdtDisabled:hover {\n  background: none;\n  color: #999999;\n  cursor: not-allowed;\n}\n\n.rdtPicker td span.rdtOld {\n  color: #999999;\n}\n.rdtPicker td span.rdtDisabled,\n.rdtPicker td span.rdtDisabled:hover {\n  background: none;\n  color: #999999;\n  cursor: not-allowed;\n}\n.rdtPicker th {\n  border-bottom: 1px solid #f9f9f9;\n}\n.rdtPicker .dow {\n  width: 14.2857%;\n  border-bottom: none;\n  cursor: default;\n}\n.rdtPicker th.rdtSwitch {\n  width: 100px;\n}\n.rdtPicker th.rdtNext,\n.rdtPicker th.rdtPrev {\n  font-size: 21px;\n  vertical-align: top;\n}\n\n.rdtPrev span,\n.rdtNext span {\n  display: block;\n  -webkit-touch-callout: none; /* iOS Safari */\n  -webkit-user-select: none;   /* Chrome/Safari/Opera */\n  -khtml-user-select: none;    /* Konqueror */\n  -moz-user-select: none;      /* Firefox */\n  -ms-user-select: none;       /* Internet Explorer/Edge */\n  user-select: none;\n}\n\n.rdtPicker th.rdtDisabled,\n.rdtPicker th.rdtDisabled:hover {\n  background: none;\n  color: #999999;\n  cursor: not-allowed;\n}\n.rdtPicker thead tr:first-child th {\n  cursor: pointer;\n}\n.rdtPicker thead tr:first-child th:hover {\n  background: #eeeeee;\n}\n\n.rdtPicker tfoot {\n  border-top: 1px solid #f9f9f9;\n}\n\n.rdtPicker button {\n  border: none;\n  background: none;\n  cursor: pointer;\n}\n.rdtPicker button:hover {\n  background-color: #eee;\n}\n\n.rdtPicker thead button {\n  width: 100%;\n  height: 100%;\n}\n\ntd.rdtMonth,\ntd.rdtYear {\n  height: 50px;\n  width: 25%;\n  cursor: pointer;\n}\ntd.rdtMonth:hover,\ntd.rdtYear:hover {\n  background: #eee;\n}\n\n.rdtCounters {\n  display: inline-block;\n}\n\n.rdtCounters > div {\n  float: left;\n}\n\n.rdtCounter {\n  height: 100px;\n}\n\n.rdtCounter {\n  width: 40px;\n}\n\n.rdtCounterSeparator {\n  line-height: 100px;\n}\n\n.rdtCounter .rdtBtn {\n  height: 40%;\n  line-height: 40px;\n  cursor: pointer;\n  display: block;\n\n  -webkit-touch-callout: none; /* iOS Safari */\n  -webkit-user-select: none;   /* Chrome/Safari/Opera */\n  -khtml-user-select: none;    /* Konqueror */\n  -moz-user-select: none;      /* Firefox */\n  -ms-user-select: none;       /* Internet Explorer/Edge */\n  user-select: none;\n}\n.rdtCounter .rdtBtn:hover {\n  background: #eee;\n}\n.rdtCounter .rdtCount {\n  height: 20%;\n  font-size: 1.2em;\n}\n\n.rdtMilli {\n  vertical-align: middle;\n  padding-left: 8px;\n  width: 48px;\n}\n\n.rdtMilli input {\n  width: 100%;\n  font-size: 1.2em;\n  margin-top: 37px;\n}\n\n.rdtTime td {\n  cursor: default;\n}\n", ""]);


/***/ }),

/***/ "../../node_modules/css-loader/dist/runtime/api.js":
/*!*****************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/css-loader/dist/runtime/api.js ***!
  \*****************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


/*
  MIT License http://www.opensource.org/licenses/mit-license.php
  Author Tobias Koppers @sokra
*/
// css base code, injected by the css-loader
// eslint-disable-next-line func-names
module.exports = function (useSourceMap) {
  var list = []; // return the list of modules as css string

  list.toString = function toString() {
    return this.map(function (item) {
      var content = cssWithMappingToString(item, useSourceMap);

      if (item[2]) {
        return "@media ".concat(item[2], "{").concat(content, "}");
      }

      return content;
    }).join('');
  }; // import a list of modules into the list
  // eslint-disable-next-line func-names


  list.i = function (modules, mediaQuery) {
    if (typeof modules === 'string') {
      // eslint-disable-next-line no-param-reassign
      modules = [[null, modules, '']];
    }

    var alreadyImportedModules = {};

    for (var i = 0; i < this.length; i++) {
      // eslint-disable-next-line prefer-destructuring
      var id = this[i][0];

      if (id != null) {
        alreadyImportedModules[id] = true;
      }
    }

    for (var _i = 0; _i < modules.length; _i++) {
      var item = modules[_i]; // skip already imported module
      // this implementation is not 100% perfect for weird media query combinations
      // when a module is imported multiple times with different media queries.
      // I hope this will never occur (Hey this way we have smaller bundles)

      if (item[0] == null || !alreadyImportedModules[item[0]]) {
        if (mediaQuery && !item[2]) {
          item[2] = mediaQuery;
        } else if (mediaQuery) {
          item[2] = "(".concat(item[2], ") and (").concat(mediaQuery, ")");
        }

        list.push(item);
      }
    }
  };

  return list;
};

function cssWithMappingToString(item, useSourceMap) {
  var content = item[1] || ''; // eslint-disable-next-line prefer-destructuring

  var cssMapping = item[3];

  if (!cssMapping) {
    return content;
  }

  if (useSourceMap && typeof btoa === 'function') {
    var sourceMapping = toComment(cssMapping);
    var sourceURLs = cssMapping.sources.map(function (source) {
      return "/*# sourceURL=".concat(cssMapping.sourceRoot).concat(source, " */");
    });
    return [content].concat(sourceURLs).concat([sourceMapping]).join('\n');
  }

  return [content].join('\n');
} // Adapted from convert-source-map (MIT)


function toComment(sourceMap) {
  // eslint-disable-next-line no-undef
  var base64 = btoa(unescape(encodeURIComponent(JSON.stringify(sourceMap))));
  var data = "sourceMappingURL=data:application/json;charset=utf-8;base64,".concat(base64);
  return "/*# ".concat(data, " */");
}

/***/ }),

/***/ "../../node_modules/decode-uri-component/index.js":
/*!****************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/decode-uri-component/index.js ***!
  \****************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var token = '%[a-f0-9]{2}';
var singleMatcher = new RegExp(token, 'gi');
var multiMatcher = new RegExp('(' + token + ')+', 'gi');

function decodeComponents(components, split) {
	try {
		// Try to decode the entire string first
		return decodeURIComponent(components.join(''));
	} catch (err) {
		// Do nothing
	}

	if (components.length === 1) {
		return components;
	}

	split = split || 1;

	// Split the array in 2 parts
	var left = components.slice(0, split);
	var right = components.slice(split);

	return Array.prototype.concat.call([], decodeComponents(left), decodeComponents(right));
}

function decode(input) {
	try {
		return decodeURIComponent(input);
	} catch (err) {
		var tokens = input.match(singleMatcher);

		for (var i = 1; i < tokens.length; i++) {
			input = decodeComponents(tokens, i).join('');

			tokens = input.match(singleMatcher);
		}

		return input;
	}
}

function customDecodeURIComponent(input) {
	// Keep track of all the replacements and prefill the map with the `BOM`
	var replaceMap = {
		'%FE%FF': '\uFFFD\uFFFD',
		'%FF%FE': '\uFFFD\uFFFD'
	};

	var match = multiMatcher.exec(input);
	while (match) {
		try {
			// Decode as big chunks as possible
			replaceMap[match[0]] = decodeURIComponent(match[0]);
		} catch (err) {
			var result = decode(match[0]);

			if (result !== match[0]) {
				replaceMap[match[0]] = result;
			}
		}

		match = multiMatcher.exec(input);
	}

	// Add `%C2` at the end of the map to make sure it does not replace the combinator before everything else
	replaceMap['%C2'] = '\uFFFD';

	var entries = Object.keys(replaceMap);

	for (var i = 0; i < entries.length; i++) {
		// Replace all decoded components
		var key = entries[i];
		input = input.replace(new RegExp(key, 'g'), replaceMap[key]);
	}

	return input;
}

module.exports = function (encodedURI) {
	if (typeof encodedURI !== 'string') {
		throw new TypeError('Expected `encodedURI` to be of type `string`, got `' + typeof encodedURI + '`');
	}

	try {
		encodedURI = encodedURI.replace(/\+/g, ' ');

		// Try the built in decoder first
		return decodeURIComponent(encodedURI);
	} catch (err) {
		// Fallback to a more advanced decoder
		return customDecodeURIComponent(encodedURI);
	}
};


/***/ }),

/***/ "../../node_modules/fbjs/lib/emptyFunction.js":
/*!************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/fbjs/lib/emptyFunction.js ***!
  \************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 *
 * 
 */

function makeEmptyFunction(arg) {
  return function () {
    return arg;
  };
}

/**
 * This function accepts and discards inputs; it has no side effects. This is
 * primarily useful idiomatically for overridable function endpoints which
 * always need to be callable, since JS lacks a null-call idiom ala Cocoa.
 */
var emptyFunction = function emptyFunction() {};

emptyFunction.thatReturns = makeEmptyFunction;
emptyFunction.thatReturnsFalse = makeEmptyFunction(false);
emptyFunction.thatReturnsTrue = makeEmptyFunction(true);
emptyFunction.thatReturnsNull = makeEmptyFunction(null);
emptyFunction.thatReturnsThis = function () {
  return this;
};
emptyFunction.thatReturnsArgument = function (arg) {
  return arg;
};

module.exports = emptyFunction;

/***/ }),

/***/ "../../node_modules/fbjs/lib/emptyObject.js":
/*!**********************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/fbjs/lib/emptyObject.js ***!
  \**********************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 *
 */



var emptyObject = {};

if (true) {
  Object.freeze(emptyObject);
}

module.exports = emptyObject;

/***/ }),

/***/ "../../node_modules/fbjs/lib/invariant.js":
/*!********************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/fbjs/lib/invariant.js ***!
  \********************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 *
 */



/**
 * Use invariant() to assert state which your program assumes to be true.
 *
 * Provide sprintf-style format (only %s is supported) and arguments
 * to provide information about what broke and what you were
 * expecting.
 *
 * The invariant message will be stripped in production, but the invariant
 * will remain to ensure logic does not differ in production.
 */

var validateFormat = function validateFormat(format) {};

if (true) {
  validateFormat = function validateFormat(format) {
    if (format === undefined) {
      throw new Error('invariant requires an error message argument');
    }
  };
}

function invariant(condition, format, a, b, c, d, e, f) {
  validateFormat(format);

  if (!condition) {
    var error;
    if (format === undefined) {
      error = new Error('Minified exception occurred; use the non-minified dev environment ' + 'for the full error message and additional helpful warnings.');
    } else {
      var args = [a, b, c, d, e, f];
      var argIndex = 0;
      error = new Error(format.replace(/%s/g, function () {
        return args[argIndex++];
      }));
      error.name = 'Invariant Violation';
    }

    error.framesToPop = 1; // we don't care about invariant's own frame
    throw error;
  }
}

module.exports = invariant;

/***/ }),

/***/ "../../node_modules/fbjs/lib/warning.js":
/*!******************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/fbjs/lib/warning.js ***!
  \******************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2014-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 *
 */



var emptyFunction = __webpack_require__(/*! ./emptyFunction */ "../../node_modules/fbjs/lib/emptyFunction.js");

/**
 * Similar to invariant but only logs a warning if the condition is not met.
 * This can be used to log issues in development environments in critical
 * paths. Removing the logging code for production environments will keep the
 * same logic and follow the same code paths.
 */

var warning = emptyFunction;

if (true) {
  var printWarning = function printWarning(format) {
    for (var _len = arguments.length, args = Array(_len > 1 ? _len - 1 : 0), _key = 1; _key < _len; _key++) {
      args[_key - 1] = arguments[_key];
    }

    var argIndex = 0;
    var message = 'Warning: ' + format.replace(/%s/g, function () {
      return args[argIndex++];
    });
    if (typeof console !== 'undefined') {
      console.error(message);
    }
    try {
      // --- Welcome to debugging React ---
      // This error was thrown as a convenience so that you can use this stack
      // to find the callsite that caused this warning to fire.
      throw new Error(message);
    } catch (x) {}
  };

  warning = function warning(condition, format) {
    if (format === undefined) {
      throw new Error('`warning(condition, format, ...args)` requires a warning ' + 'message argument');
    }

    if (format.indexOf('Failed Composite propType: ') === 0) {
      return; // Ignore CompositeComponent proptype check.
    }

    if (!condition) {
      for (var _len2 = arguments.length, args = Array(_len2 > 2 ? _len2 - 2 : 0), _key2 = 2; _key2 < _len2; _key2++) {
        args[_key2 - 2] = arguments[_key2];
      }

      printWarning.apply(undefined, [format].concat(args));
    }
  };
}

module.exports = warning;

/***/ }),

/***/ "../../node_modules/history/DOMUtils.js":
/*!******************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/history/DOMUtils.js ***!
  \******************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
var canUseDOM = exports.canUseDOM = !!(typeof window !== 'undefined' && window.document && window.document.createElement);

var addEventListener = exports.addEventListener = function addEventListener(node, event, listener) {
  return node.addEventListener ? node.addEventListener(event, listener, false) : node.attachEvent('on' + event, listener);
};

var removeEventListener = exports.removeEventListener = function removeEventListener(node, event, listener) {
  return node.removeEventListener ? node.removeEventListener(event, listener, false) : node.detachEvent('on' + event, listener);
};

var getConfirmation = exports.getConfirmation = function getConfirmation(message, callback) {
  return callback(window.confirm(message));
}; // eslint-disable-line no-alert

/**
 * Returns true if the HTML5 history API is supported. Taken from Modernizr.
 *
 * https://github.com/Modernizr/Modernizr/blob/master/LICENSE
 * https://github.com/Modernizr/Modernizr/blob/master/feature-detects/history.js
 * changed to avoid false negatives for Windows Phones: https://github.com/reactjs/react-router/issues/586
 */
var supportsHistory = exports.supportsHistory = function supportsHistory() {
  var ua = window.navigator.userAgent;

  if ((ua.indexOf('Android 2.') !== -1 || ua.indexOf('Android 4.0') !== -1) && ua.indexOf('Mobile Safari') !== -1 && ua.indexOf('Chrome') === -1 && ua.indexOf('Windows Phone') === -1) return false;

  return window.history && 'pushState' in window.history;
};

/**
 * Returns true if browser fires popstate on hash change.
 * IE10 and IE11 do not.
 */
var supportsPopStateOnHashChange = exports.supportsPopStateOnHashChange = function supportsPopStateOnHashChange() {
  return window.navigator.userAgent.indexOf('Trident') === -1;
};

/**
 * Returns false if using go(n) with hash history causes a full page reload.
 */
var supportsGoWithoutReloadUsingHash = exports.supportsGoWithoutReloadUsingHash = function supportsGoWithoutReloadUsingHash() {
  return window.navigator.userAgent.indexOf('Firefox') === -1;
};

/**
 * Returns true if a given popstate event is an extraneous WebKit event.
 * Accounts for the fact that Chrome on iOS fires real popstate events
 * containing undefined state when pressing the back button.
 */
var isExtraneousPopstateEvent = exports.isExtraneousPopstateEvent = function isExtraneousPopstateEvent(event) {
  return event.state === undefined && navigator.userAgent.indexOf('CriOS') === -1;
};

/***/ }),

/***/ "../../node_modules/history/LocationUtils.js":
/*!***********************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/history/LocationUtils.js ***!
  \***********************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
exports.locationsAreEqual = exports.createLocation = undefined;

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _resolvePathname = __webpack_require__(/*! resolve-pathname */ "../../node_modules/resolve-pathname/index.js");

var _resolvePathname2 = _interopRequireDefault(_resolvePathname);

var _valueEqual = __webpack_require__(/*! value-equal */ "../../node_modules/value-equal/index.js");

var _valueEqual2 = _interopRequireDefault(_valueEqual);

var _PathUtils = __webpack_require__(/*! ./PathUtils */ "../../node_modules/history/PathUtils.js");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var createLocation = exports.createLocation = function createLocation(path, state, key, currentLocation) {
  var location = void 0;
  if (typeof path === 'string') {
    // Two-arg form: push(path, state)
    location = (0, _PathUtils.parsePath)(path);
    location.state = state;
  } else {
    // One-arg form: push(location)
    location = _extends({}, path);

    if (location.pathname === undefined) location.pathname = '';

    if (location.search) {
      if (location.search.charAt(0) !== '?') location.search = '?' + location.search;
    } else {
      location.search = '';
    }

    if (location.hash) {
      if (location.hash.charAt(0) !== '#') location.hash = '#' + location.hash;
    } else {
      location.hash = '';
    }

    if (state !== undefined && location.state === undefined) location.state = state;
  }

  try {
    location.pathname = decodeURI(location.pathname);
  } catch (e) {
    if (e instanceof URIError) {
      throw new URIError('Pathname "' + location.pathname + '" could not be decoded. ' + 'This is likely caused by an invalid percent-encoding.');
    } else {
      throw e;
    }
  }

  if (key) location.key = key;

  if (currentLocation) {
    // Resolve incomplete/relative pathname relative to current location.
    if (!location.pathname) {
      location.pathname = currentLocation.pathname;
    } else if (location.pathname.charAt(0) !== '/') {
      location.pathname = (0, _resolvePathname2.default)(location.pathname, currentLocation.pathname);
    }
  } else {
    // When there is no prior location and pathname is empty, set it to /
    if (!location.pathname) {
      location.pathname = '/';
    }
  }

  return location;
};

var locationsAreEqual = exports.locationsAreEqual = function locationsAreEqual(a, b) {
  return a.pathname === b.pathname && a.search === b.search && a.hash === b.hash && a.key === b.key && (0, _valueEqual2.default)(a.state, b.state);
};

/***/ }),

/***/ "../../node_modules/history/PathUtils.js":
/*!*******************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/history/PathUtils.js ***!
  \*******************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;
var addLeadingSlash = exports.addLeadingSlash = function addLeadingSlash(path) {
  return path.charAt(0) === '/' ? path : '/' + path;
};

var stripLeadingSlash = exports.stripLeadingSlash = function stripLeadingSlash(path) {
  return path.charAt(0) === '/' ? path.substr(1) : path;
};

var hasBasename = exports.hasBasename = function hasBasename(path, prefix) {
  return new RegExp('^' + prefix + '(\\/|\\?|#|$)', 'i').test(path);
};

var stripBasename = exports.stripBasename = function stripBasename(path, prefix) {
  return hasBasename(path, prefix) ? path.substr(prefix.length) : path;
};

var stripTrailingSlash = exports.stripTrailingSlash = function stripTrailingSlash(path) {
  return path.charAt(path.length - 1) === '/' ? path.slice(0, -1) : path;
};

var parsePath = exports.parsePath = function parsePath(path) {
  var pathname = path || '/';
  var search = '';
  var hash = '';

  var hashIndex = pathname.indexOf('#');
  if (hashIndex !== -1) {
    hash = pathname.substr(hashIndex);
    pathname = pathname.substr(0, hashIndex);
  }

  var searchIndex = pathname.indexOf('?');
  if (searchIndex !== -1) {
    search = pathname.substr(searchIndex);
    pathname = pathname.substr(0, searchIndex);
  }

  return {
    pathname: pathname,
    search: search === '?' ? '' : search,
    hash: hash === '#' ? '' : hash
  };
};

var createPath = exports.createPath = function createPath(location) {
  var pathname = location.pathname,
      search = location.search,
      hash = location.hash;


  var path = pathname || '/';

  if (search && search !== '?') path += search.charAt(0) === '?' ? search : '?' + search;

  if (hash && hash !== '#') path += hash.charAt(0) === '#' ? hash : '#' + hash;

  return path;
};

/***/ }),

/***/ "../../node_modules/history/createBrowserHistory.js":
/*!******************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/history/createBrowserHistory.js ***!
  \******************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;

var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };

var _extends = Object.assign || function (target) { for (var i = 1; i < arguments.length; i++) { var source = arguments[i]; for (var key in source) { if (Object.prototype.hasOwnProperty.call(source, key)) { target[key] = source[key]; } } } return target; };

var _warning = __webpack_require__(/*! warning */ "../../node_modules/warning/browser.js");

var _warning2 = _interopRequireDefault(_warning);

var _invariant = __webpack_require__(/*! invariant */ "../../node_modules/invariant/browser.js");

var _invariant2 = _interopRequireDefault(_invariant);

var _LocationUtils = __webpack_require__(/*! ./LocationUtils */ "../../node_modules/history/LocationUtils.js");

var _PathUtils = __webpack_require__(/*! ./PathUtils */ "../../node_modules/history/PathUtils.js");

var _createTransitionManager = __webpack_require__(/*! ./createTransitionManager */ "../../node_modules/history/createTransitionManager.js");

var _createTransitionManager2 = _interopRequireDefault(_createTransitionManager);

var _DOMUtils = __webpack_require__(/*! ./DOMUtils */ "../../node_modules/history/DOMUtils.js");

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var PopStateEvent = 'popstate';
var HashChangeEvent = 'hashchange';

var getHistoryState = function getHistoryState() {
  try {
    return window.history.state || {};
  } catch (e) {
    // IE 11 sometimes throws when accessing window.history.state
    // See https://github.com/ReactTraining/history/pull/289
    return {};
  }
};

/**
 * Creates a history object that uses the HTML5 history API including
 * pushState, replaceState, and the popstate event.
 */
var createBrowserHistory = function createBrowserHistory() {
  var props = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : {};

  (0, _invariant2.default)(_DOMUtils.canUseDOM, 'Browser history needs a DOM');

  var globalHistory = window.history;
  var canUseHistory = (0, _DOMUtils.supportsHistory)();
  var needsHashChangeListener = !(0, _DOMUtils.supportsPopStateOnHashChange)();

  var _props$forceRefresh = props.forceRefresh,
      forceRefresh = _props$forceRefresh === undefined ? false : _props$forceRefresh,
      _props$getUserConfirm = props.getUserConfirmation,
      getUserConfirmation = _props$getUserConfirm === undefined ? _DOMUtils.getConfirmation : _props$getUserConfirm,
      _props$keyLength = props.keyLength,
      keyLength = _props$keyLength === undefined ? 6 : _props$keyLength;

  var basename = props.basename ? (0, _PathUtils.stripTrailingSlash)((0, _PathUtils.addLeadingSlash)(props.basename)) : '';

  var getDOMLocation = function getDOMLocation(historyState) {
    var _ref = historyState || {},
        key = _ref.key,
        state = _ref.state;

    var _window$location = window.location,
        pathname = _window$location.pathname,
        search = _window$location.search,
        hash = _window$location.hash;


    var path = pathname + search + hash;

    (0, _warning2.default)(!basename || (0, _PathUtils.hasBasename)(path, basename), 'You are attempting to use a basename on a page whose URL path does not begin ' + 'with the basename. Expected path "' + path + '" to begin with "' + basename + '".');

    if (basename) path = (0, _PathUtils.stripBasename)(path, basename);

    return (0, _LocationUtils.createLocation)(path, state, key);
  };

  var createKey = function createKey() {
    return Math.random().toString(36).substr(2, keyLength);
  };

  var transitionManager = (0, _createTransitionManager2.default)();

  var setState = function setState(nextState) {
    _extends(history, nextState);

    history.length = globalHistory.length;

    transitionManager.notifyListeners(history.location, history.action);
  };

  var handlePopState = function handlePopState(event) {
    // Ignore extraneous popstate events in WebKit.
    if ((0, _DOMUtils.isExtraneousPopstateEvent)(event)) return;

    handlePop(getDOMLocation(event.state));
  };

  var handleHashChange = function handleHashChange() {
    handlePop(getDOMLocation(getHistoryState()));
  };

  var forceNextPop = false;

  var handlePop = function handlePop(location) {
    if (forceNextPop) {
      forceNextPop = false;
      setState();
    } else {
      var action = 'POP';

      transitionManager.confirmTransitionTo(location, action, getUserConfirmation, function (ok) {
        if (ok) {
          setState({ action: action, location: location });
        } else {
          revertPop(location);
        }
      });
    }
  };

  var revertPop = function revertPop(fromLocation) {
    var toLocation = history.location;

    // TODO: We could probably make this more reliable by
    // keeping a list of keys we've seen in sessionStorage.
    // Instead, we just default to 0 for keys we don't know.

    var toIndex = allKeys.indexOf(toLocation.key);

    if (toIndex === -1) toIndex = 0;

    var fromIndex = allKeys.indexOf(fromLocation.key);

    if (fromIndex === -1) fromIndex = 0;

    var delta = toIndex - fromIndex;

    if (delta) {
      forceNextPop = true;
      go(delta);
    }
  };

  var initialLocation = getDOMLocation(getHistoryState());
  var allKeys = [initialLocation.key];

  // Public interface

  var createHref = function createHref(location) {
    return basename + (0, _PathUtils.createPath)(location);
  };

  var push = function push(path, state) {
    (0, _warning2.default)(!((typeof path === 'undefined' ? 'undefined' : _typeof(path)) === 'object' && path.state !== undefined && state !== undefined), 'You should avoid providing a 2nd state argument to push when the 1st ' + 'argument is a location-like object that already has state; it is ignored');

    var action = 'PUSH';
    var location = (0, _LocationUtils.createLocation)(path, state, createKey(), history.location);

    transitionManager.confirmTransitionTo(location, action, getUserConfirmation, function (ok) {
      if (!ok) return;

      var href = createHref(location);
      var key = location.key,
          state = location.state;


      if (canUseHistory) {
        globalHistory.pushState({ key: key, state: state }, null, href);

        if (forceRefresh) {
          window.location.href = href;
        } else {
          var prevIndex = allKeys.indexOf(history.location.key);
          var nextKeys = allKeys.slice(0, prevIndex === -1 ? 0 : prevIndex + 1);

          nextKeys.push(location.key);
          allKeys = nextKeys;

          setState({ action: action, location: location });
        }
      } else {
        (0, _warning2.default)(state === undefined, 'Browser history cannot push state in browsers that do not support HTML5 history');

        window.location.href = href;
      }
    });
  };

  var replace = function replace(path, state) {
    (0, _warning2.default)(!((typeof path === 'undefined' ? 'undefined' : _typeof(path)) === 'object' && path.state !== undefined && state !== undefined), 'You should avoid providing a 2nd state argument to replace when the 1st ' + 'argument is a location-like object that already has state; it is ignored');

    var action = 'REPLACE';
    var location = (0, _LocationUtils.createLocation)(path, state, createKey(), history.location);

    transitionManager.confirmTransitionTo(location, action, getUserConfirmation, function (ok) {
      if (!ok) return;

      var href = createHref(location);
      var key = location.key,
          state = location.state;


      if (canUseHistory) {
        globalHistory.replaceState({ key: key, state: state }, null, href);

        if (forceRefresh) {
          window.location.replace(href);
        } else {
          var prevIndex = allKeys.indexOf(history.location.key);

          if (prevIndex !== -1) allKeys[prevIndex] = location.key;

          setState({ action: action, location: location });
        }
      } else {
        (0, _warning2.default)(state === undefined, 'Browser history cannot replace state in browsers that do not support HTML5 history');

        window.location.replace(href);
      }
    });
  };

  var go = function go(n) {
    globalHistory.go(n);
  };

  var goBack = function goBack() {
    return go(-1);
  };

  var goForward = function goForward() {
    return go(1);
  };

  var listenerCount = 0;

  var checkDOMListeners = function checkDOMListeners(delta) {
    listenerCount += delta;

    if (listenerCount === 1) {
      (0, _DOMUtils.addEventListener)(window, PopStateEvent, handlePopState);

      if (needsHashChangeListener) (0, _DOMUtils.addEventListener)(window, HashChangeEvent, handleHashChange);
    } else if (listenerCount === 0) {
      (0, _DOMUtils.removeEventListener)(window, PopStateEvent, handlePopState);

      if (needsHashChangeListener) (0, _DOMUtils.removeEventListener)(window, HashChangeEvent, handleHashChange);
    }
  };

  var isBlocked = false;

  var block = function block() {
    var prompt = arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : false;

    var unblock = transitionManager.setPrompt(prompt);

    if (!isBlocked) {
      checkDOMListeners(1);
      isBlocked = true;
    }

    return function () {
      if (isBlocked) {
        isBlocked = false;
        checkDOMListeners(-1);
      }

      return unblock();
    };
  };

  var listen = function listen(listener) {
    var unlisten = transitionManager.appendListener(listener);
    checkDOMListeners(1);

    return function () {
      checkDOMListeners(-1);
      unlisten();
    };
  };

  var history = {
    length: globalHistory.length,
    action: 'POP',
    location: initialLocation,
    createHref: createHref,
    push: push,
    replace: replace,
    go: go,
    goBack: goBack,
    goForward: goForward,
    block: block,
    listen: listen
  };

  return history;
};

exports.default = createBrowserHistory;

/***/ }),

/***/ "../../node_modules/history/createTransitionManager.js":
/*!*********************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/history/createTransitionManager.js ***!
  \*********************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


exports.__esModule = true;

var _warning = __webpack_require__(/*! warning */ "../../node_modules/warning/browser.js");

var _warning2 = _interopRequireDefault(_warning);

function _interopRequireDefault(obj) { return obj && obj.__esModule ? obj : { default: obj }; }

var createTransitionManager = function createTransitionManager() {
  var prompt = null;

  var setPrompt = function setPrompt(nextPrompt) {
    (0, _warning2.default)(prompt == null, 'A history supports only one prompt at a time');

    prompt = nextPrompt;

    return function () {
      if (prompt === nextPrompt) prompt = null;
    };
  };

  var confirmTransitionTo = function confirmTransitionTo(location, action, getUserConfirmation, callback) {
    // TODO: If another transition starts while we're still confirming
    // the previous one, we may end up in a weird state. Figure out the
    // best way to handle this.
    if (prompt != null) {
      var result = typeof prompt === 'function' ? prompt(location, action) : prompt;

      if (typeof result === 'string') {
        if (typeof getUserConfirmation === 'function') {
          getUserConfirmation(result, callback);
        } else {
          (0, _warning2.default)(false, 'A history needs a getUserConfirmation function in order to use a prompt message');

          callback(true);
        }
      } else {
        // Return false from a transition hook to cancel the transition.
        callback(result !== false);
      }
    } else {
      callback(true);
    }
  };

  var listeners = [];

  var appendListener = function appendListener(fn) {
    var isActive = true;

    var listener = function listener() {
      if (isActive) fn.apply(undefined, arguments);
    };

    listeners.push(listener);

    return function () {
      isActive = false;
      listeners = listeners.filter(function (item) {
        return item !== listener;
      });
    };
  };

  var notifyListeners = function notifyListeners() {
    for (var _len = arguments.length, args = Array(_len), _key = 0; _key < _len; _key++) {
      args[_key] = arguments[_key];
    }

    listeners.forEach(function (listener) {
      return listener.apply(undefined, args);
    });
  };

  return {
    setPrompt: setPrompt,
    confirmTransitionTo: confirmTransitionTo,
    appendListener: appendListener,
    notifyListeners: notifyListeners
  };
};

exports.default = createTransitionManager;

/***/ }),

/***/ "../../node_modules/invariant/browser.js":
/*!*******************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/invariant/browser.js ***!
  \*******************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



/**
 * Use invariant() to assert state which your program assumes to be true.
 *
 * Provide sprintf-style format (only %s is supported) and arguments
 * to provide information about what broke and what you were
 * expecting.
 *
 * The invariant message will be stripped in production, but the invariant
 * will remain to ensure logic does not differ in production.
 */

var invariant = function(condition, format, a, b, c, d, e, f) {
  if (true) {
    if (format === undefined) {
      throw new Error('invariant requires an error message argument');
    }
  }

  if (!condition) {
    var error;
    if (format === undefined) {
      error = new Error(
        'Minified exception occurred; use the non-minified dev environment ' +
        'for the full error message and additional helpful warnings.'
      );
    } else {
      var args = [a, b, c, d, e, f];
      var argIndex = 0;
      error = new Error(
        format.replace(/%s/g, function() { return args[argIndex++]; })
      );
      error.name = 'Invariant Violation';
    }

    error.framesToPop = 1; // we don't care about invariant's own frame
    throw error;
  }
};

module.exports = invariant;


/***/ }),

/***/ "../../node_modules/object-assign/index.js":
/*!*********************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/object-assign/index.js ***!
  \*********************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/*
object-assign
(c) Sindre Sorhus
@license MIT
*/


/* eslint-disable no-unused-vars */
var getOwnPropertySymbols = Object.getOwnPropertySymbols;
var hasOwnProperty = Object.prototype.hasOwnProperty;
var propIsEnumerable = Object.prototype.propertyIsEnumerable;

function toObject(val) {
	if (val === null || val === undefined) {
		throw new TypeError('Object.assign cannot be called with null or undefined');
	}

	return Object(val);
}

function shouldUseNative() {
	try {
		if (!Object.assign) {
			return false;
		}

		// Detect buggy property enumeration order in older V8 versions.

		// https://bugs.chromium.org/p/v8/issues/detail?id=4118
		var test1 = new String('abc');  // eslint-disable-line no-new-wrappers
		test1[5] = 'de';
		if (Object.getOwnPropertyNames(test1)[0] === '5') {
			return false;
		}

		// https://bugs.chromium.org/p/v8/issues/detail?id=3056
		var test2 = {};
		for (var i = 0; i < 10; i++) {
			test2['_' + String.fromCharCode(i)] = i;
		}
		var order2 = Object.getOwnPropertyNames(test2).map(function (n) {
			return test2[n];
		});
		if (order2.join('') !== '0123456789') {
			return false;
		}

		// https://bugs.chromium.org/p/v8/issues/detail?id=3056
		var test3 = {};
		'abcdefghijklmnopqrst'.split('').forEach(function (letter) {
			test3[letter] = letter;
		});
		if (Object.keys(Object.assign({}, test3)).join('') !==
				'abcdefghijklmnopqrst') {
			return false;
		}

		return true;
	} catch (err) {
		// We don't expect any of the above to throw, but better to be safe.
		return false;
	}
}

module.exports = shouldUseNative() ? Object.assign : function (target, source) {
	var from;
	var to = toObject(target);
	var symbols;

	for (var s = 1; s < arguments.length; s++) {
		from = Object(arguments[s]);

		for (var key in from) {
			if (hasOwnProperty.call(from, key)) {
				to[key] = from[key];
			}
		}

		if (getOwnPropertySymbols) {
			symbols = getOwnPropertySymbols(from);
			for (var i = 0; i < symbols.length; i++) {
				if (propIsEnumerable.call(from, symbols[i])) {
					to[symbols[i]] = from[symbols[i]];
				}
			}
		}
	}

	return to;
};


/***/ }),

/***/ "../../node_modules/prop-types/checkPropTypes.js":
/*!***************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/prop-types/checkPropTypes.js ***!
  \***************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



if (true) {
  var invariant = __webpack_require__(/*! fbjs/lib/invariant */ "../../node_modules/fbjs/lib/invariant.js");
  var warning = __webpack_require__(/*! fbjs/lib/warning */ "../../node_modules/fbjs/lib/warning.js");
  var ReactPropTypesSecret = __webpack_require__(/*! ./lib/ReactPropTypesSecret */ "../../node_modules/prop-types/lib/ReactPropTypesSecret.js");
  var loggedTypeFailures = {};
}

/**
 * Assert that the values match with the type specs.
 * Error messages are memorized and will only be shown once.
 *
 * @param {object} typeSpecs Map of name to a ReactPropType
 * @param {object} values Runtime values that need to be type-checked
 * @param {string} location e.g. "prop", "context", "child context"
 * @param {string} componentName Name of the component for error messages.
 * @param {?Function} getStack Returns the component stack.
 * @private
 */
function checkPropTypes(typeSpecs, values, location, componentName, getStack) {
  if (true) {
    for (var typeSpecName in typeSpecs) {
      if (typeSpecs.hasOwnProperty(typeSpecName)) {
        var error;
        // Prop type validation may throw. In case they do, we don't want to
        // fail the render phase where it didn't fail before. So we log it.
        // After these have been cleaned up, we'll let them throw.
        try {
          // This is intentionally an invariant that gets caught. It's the same
          // behavior as without this statement except with a better message.
          invariant(typeof typeSpecs[typeSpecName] === 'function', '%s: %s type `%s` is invalid; it must be a function, usually from ' + 'the `prop-types` package, but received `%s`.', componentName || 'React class', location, typeSpecName, typeof typeSpecs[typeSpecName]);
          error = typeSpecs[typeSpecName](values, typeSpecName, componentName, location, null, ReactPropTypesSecret);
        } catch (ex) {
          error = ex;
        }
        warning(!error || error instanceof Error, '%s: type specification of %s `%s` is invalid; the type checker ' + 'function must return `null` or an `Error` but returned a %s. ' + 'You may have forgotten to pass an argument to the type checker ' + 'creator (arrayOf, instanceOf, objectOf, oneOf, oneOfType, and ' + 'shape all require an argument).', componentName || 'React class', location, typeSpecName, typeof error);
        if (error instanceof Error && !(error.message in loggedTypeFailures)) {
          // Only monitor this failure once because there tends to be a lot of the
          // same error.
          loggedTypeFailures[error.message] = true;

          var stack = getStack ? getStack() : '';

          warning(false, 'Failed %s type: %s%s', location, error.message, stack != null ? stack : '');
        }
      }
    }
  }
}

module.exports = checkPropTypes;


/***/ }),

/***/ "../../node_modules/prop-types/factoryWithTypeCheckers.js":
/*!************************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/prop-types/factoryWithTypeCheckers.js ***!
  \************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var emptyFunction = __webpack_require__(/*! fbjs/lib/emptyFunction */ "../../node_modules/fbjs/lib/emptyFunction.js");
var invariant = __webpack_require__(/*! fbjs/lib/invariant */ "../../node_modules/fbjs/lib/invariant.js");
var warning = __webpack_require__(/*! fbjs/lib/warning */ "../../node_modules/fbjs/lib/warning.js");
var assign = __webpack_require__(/*! object-assign */ "../../node_modules/object-assign/index.js");

var ReactPropTypesSecret = __webpack_require__(/*! ./lib/ReactPropTypesSecret */ "../../node_modules/prop-types/lib/ReactPropTypesSecret.js");
var checkPropTypes = __webpack_require__(/*! ./checkPropTypes */ "../../node_modules/prop-types/checkPropTypes.js");

module.exports = function(isValidElement, throwOnDirectAccess) {
  /* global Symbol */
  var ITERATOR_SYMBOL = typeof Symbol === 'function' && Symbol.iterator;
  var FAUX_ITERATOR_SYMBOL = '@@iterator'; // Before Symbol spec.

  /**
   * Returns the iterator method function contained on the iterable object.
   *
   * Be sure to invoke the function with the iterable as context:
   *
   *     var iteratorFn = getIteratorFn(myIterable);
   *     if (iteratorFn) {
   *       var iterator = iteratorFn.call(myIterable);
   *       ...
   *     }
   *
   * @param {?object} maybeIterable
   * @return {?function}
   */
  function getIteratorFn(maybeIterable) {
    var iteratorFn = maybeIterable && (ITERATOR_SYMBOL && maybeIterable[ITERATOR_SYMBOL] || maybeIterable[FAUX_ITERATOR_SYMBOL]);
    if (typeof iteratorFn === 'function') {
      return iteratorFn;
    }
  }

  /**
   * Collection of methods that allow declaration and validation of props that are
   * supplied to React components. Example usage:
   *
   *   var Props = require('ReactPropTypes');
   *   var MyArticle = React.createClass({
   *     propTypes: {
   *       // An optional string prop named "description".
   *       description: Props.string,
   *
   *       // A required enum prop named "category".
   *       category: Props.oneOf(['News','Photos']).isRequired,
   *
   *       // A prop named "dialog" that requires an instance of Dialog.
   *       dialog: Props.instanceOf(Dialog).isRequired
   *     },
   *     render: function() { ... }
   *   });
   *
   * A more formal specification of how these methods are used:
   *
   *   type := array|bool|func|object|number|string|oneOf([...])|instanceOf(...)
   *   decl := ReactPropTypes.{type}(.isRequired)?
   *
   * Each and every declaration produces a function with the same signature. This
   * allows the creation of custom validation functions. For example:
   *
   *  var MyLink = React.createClass({
   *    propTypes: {
   *      // An optional string or URI prop named "href".
   *      href: function(props, propName, componentName) {
   *        var propValue = props[propName];
   *        if (propValue != null && typeof propValue !== 'string' &&
   *            !(propValue instanceof URI)) {
   *          return new Error(
   *            'Expected a string or an URI for ' + propName + ' in ' +
   *            componentName
   *          );
   *        }
   *      }
   *    },
   *    render: function() {...}
   *  });
   *
   * @internal
   */

  var ANONYMOUS = '<<anonymous>>';

  // Important!
  // Keep this list in sync with production version in `./factoryWithThrowingShims.js`.
  var ReactPropTypes = {
    array: createPrimitiveTypeChecker('array'),
    bool: createPrimitiveTypeChecker('boolean'),
    func: createPrimitiveTypeChecker('function'),
    number: createPrimitiveTypeChecker('number'),
    object: createPrimitiveTypeChecker('object'),
    string: createPrimitiveTypeChecker('string'),
    symbol: createPrimitiveTypeChecker('symbol'),

    any: createAnyTypeChecker(),
    arrayOf: createArrayOfTypeChecker,
    element: createElementTypeChecker(),
    instanceOf: createInstanceTypeChecker,
    node: createNodeChecker(),
    objectOf: createObjectOfTypeChecker,
    oneOf: createEnumTypeChecker,
    oneOfType: createUnionTypeChecker,
    shape: createShapeTypeChecker,
    exact: createStrictShapeTypeChecker,
  };

  /**
   * inlined Object.is polyfill to avoid requiring consumers ship their own
   * https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/is
   */
  /*eslint-disable no-self-compare*/
  function is(x, y) {
    // SameValue algorithm
    if (x === y) {
      // Steps 1-5, 7-10
      // Steps 6.b-6.e: +0 != -0
      return x !== 0 || 1 / x === 1 / y;
    } else {
      // Step 6.a: NaN == NaN
      return x !== x && y !== y;
    }
  }
  /*eslint-enable no-self-compare*/

  /**
   * We use an Error-like object for backward compatibility as people may call
   * PropTypes directly and inspect their output. However, we don't use real
   * Errors anymore. We don't inspect their stack anyway, and creating them
   * is prohibitively expensive if they are created too often, such as what
   * happens in oneOfType() for any type before the one that matched.
   */
  function PropTypeError(message) {
    this.message = message;
    this.stack = '';
  }
  // Make `instanceof Error` still work for returned errors.
  PropTypeError.prototype = Error.prototype;

  function createChainableTypeChecker(validate) {
    if (true) {
      var manualPropTypeCallCache = {};
      var manualPropTypeWarningCount = 0;
    }
    function checkType(isRequired, props, propName, componentName, location, propFullName, secret) {
      componentName = componentName || ANONYMOUS;
      propFullName = propFullName || propName;

      if (secret !== ReactPropTypesSecret) {
        if (throwOnDirectAccess) {
          // New behavior only for users of `prop-types` package
          invariant(
            false,
            'Calling PropTypes validators directly is not supported by the `prop-types` package. ' +
            'Use `PropTypes.checkPropTypes()` to call them. ' +
            'Read more at http://fb.me/use-check-prop-types'
          );
        } else if ( true && typeof console !== 'undefined') {
          // Old behavior for people using React.PropTypes
          var cacheKey = componentName + ':' + propName;
          if (
            !manualPropTypeCallCache[cacheKey] &&
            // Avoid spamming the console because they are often not actionable except for lib authors
            manualPropTypeWarningCount < 3
          ) {
            warning(
              false,
              'You are manually calling a React.PropTypes validation ' +
              'function for the `%s` prop on `%s`. This is deprecated ' +
              'and will throw in the standalone `prop-types` package. ' +
              'You may be seeing this warning due to a third-party PropTypes ' +
              'library. See https://fb.me/react-warning-dont-call-proptypes ' + 'for details.',
              propFullName,
              componentName
            );
            manualPropTypeCallCache[cacheKey] = true;
            manualPropTypeWarningCount++;
          }
        }
      }
      if (props[propName] == null) {
        if (isRequired) {
          if (props[propName] === null) {
            return new PropTypeError('The ' + location + ' `' + propFullName + '` is marked as required ' + ('in `' + componentName + '`, but its value is `null`.'));
          }
          return new PropTypeError('The ' + location + ' `' + propFullName + '` is marked as required in ' + ('`' + componentName + '`, but its value is `undefined`.'));
        }
        return null;
      } else {
        return validate(props, propName, componentName, location, propFullName);
      }
    }

    var chainedCheckType = checkType.bind(null, false);
    chainedCheckType.isRequired = checkType.bind(null, true);

    return chainedCheckType;
  }

  function createPrimitiveTypeChecker(expectedType) {
    function validate(props, propName, componentName, location, propFullName, secret) {
      var propValue = props[propName];
      var propType = getPropType(propValue);
      if (propType !== expectedType) {
        // `propValue` being instance of, say, date/regexp, pass the 'object'
        // check, but we can offer a more precise error message here rather than
        // 'of type `object`'.
        var preciseType = getPreciseType(propValue);

        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type ' + ('`' + preciseType + '` supplied to `' + componentName + '`, expected ') + ('`' + expectedType + '`.'));
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createAnyTypeChecker() {
    return createChainableTypeChecker(emptyFunction.thatReturnsNull);
  }

  function createArrayOfTypeChecker(typeChecker) {
    function validate(props, propName, componentName, location, propFullName) {
      if (typeof typeChecker !== 'function') {
        return new PropTypeError('Property `' + propFullName + '` of component `' + componentName + '` has invalid PropType notation inside arrayOf.');
      }
      var propValue = props[propName];
      if (!Array.isArray(propValue)) {
        var propType = getPropType(propValue);
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type ' + ('`' + propType + '` supplied to `' + componentName + '`, expected an array.'));
      }
      for (var i = 0; i < propValue.length; i++) {
        var error = typeChecker(propValue, i, componentName, location, propFullName + '[' + i + ']', ReactPropTypesSecret);
        if (error instanceof Error) {
          return error;
        }
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createElementTypeChecker() {
    function validate(props, propName, componentName, location, propFullName) {
      var propValue = props[propName];
      if (!isValidElement(propValue)) {
        var propType = getPropType(propValue);
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type ' + ('`' + propType + '` supplied to `' + componentName + '`, expected a single ReactElement.'));
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createInstanceTypeChecker(expectedClass) {
    function validate(props, propName, componentName, location, propFullName) {
      if (!(props[propName] instanceof expectedClass)) {
        var expectedClassName = expectedClass.name || ANONYMOUS;
        var actualClassName = getClassName(props[propName]);
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type ' + ('`' + actualClassName + '` supplied to `' + componentName + '`, expected ') + ('instance of `' + expectedClassName + '`.'));
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createEnumTypeChecker(expectedValues) {
    if (!Array.isArray(expectedValues)) {
       true ? warning(false, 'Invalid argument supplied to oneOf, expected an instance of array.') : undefined;
      return emptyFunction.thatReturnsNull;
    }

    function validate(props, propName, componentName, location, propFullName) {
      var propValue = props[propName];
      for (var i = 0; i < expectedValues.length; i++) {
        if (is(propValue, expectedValues[i])) {
          return null;
        }
      }

      var valuesString = JSON.stringify(expectedValues);
      return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of value `' + propValue + '` ' + ('supplied to `' + componentName + '`, expected one of ' + valuesString + '.'));
    }
    return createChainableTypeChecker(validate);
  }

  function createObjectOfTypeChecker(typeChecker) {
    function validate(props, propName, componentName, location, propFullName) {
      if (typeof typeChecker !== 'function') {
        return new PropTypeError('Property `' + propFullName + '` of component `' + componentName + '` has invalid PropType notation inside objectOf.');
      }
      var propValue = props[propName];
      var propType = getPropType(propValue);
      if (propType !== 'object') {
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type ' + ('`' + propType + '` supplied to `' + componentName + '`, expected an object.'));
      }
      for (var key in propValue) {
        if (propValue.hasOwnProperty(key)) {
          var error = typeChecker(propValue, key, componentName, location, propFullName + '.' + key, ReactPropTypesSecret);
          if (error instanceof Error) {
            return error;
          }
        }
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createUnionTypeChecker(arrayOfTypeCheckers) {
    if (!Array.isArray(arrayOfTypeCheckers)) {
       true ? warning(false, 'Invalid argument supplied to oneOfType, expected an instance of array.') : undefined;
      return emptyFunction.thatReturnsNull;
    }

    for (var i = 0; i < arrayOfTypeCheckers.length; i++) {
      var checker = arrayOfTypeCheckers[i];
      if (typeof checker !== 'function') {
        warning(
          false,
          'Invalid argument supplied to oneOfType. Expected an array of check functions, but ' +
          'received %s at index %s.',
          getPostfixForTypeWarning(checker),
          i
        );
        return emptyFunction.thatReturnsNull;
      }
    }

    function validate(props, propName, componentName, location, propFullName) {
      for (var i = 0; i < arrayOfTypeCheckers.length; i++) {
        var checker = arrayOfTypeCheckers[i];
        if (checker(props, propName, componentName, location, propFullName, ReactPropTypesSecret) == null) {
          return null;
        }
      }

      return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` supplied to ' + ('`' + componentName + '`.'));
    }
    return createChainableTypeChecker(validate);
  }

  function createNodeChecker() {
    function validate(props, propName, componentName, location, propFullName) {
      if (!isNode(props[propName])) {
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` supplied to ' + ('`' + componentName + '`, expected a ReactNode.'));
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createShapeTypeChecker(shapeTypes) {
    function validate(props, propName, componentName, location, propFullName) {
      var propValue = props[propName];
      var propType = getPropType(propValue);
      if (propType !== 'object') {
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type `' + propType + '` ' + ('supplied to `' + componentName + '`, expected `object`.'));
      }
      for (var key in shapeTypes) {
        var checker = shapeTypes[key];
        if (!checker) {
          continue;
        }
        var error = checker(propValue, key, componentName, location, propFullName + '.' + key, ReactPropTypesSecret);
        if (error) {
          return error;
        }
      }
      return null;
    }
    return createChainableTypeChecker(validate);
  }

  function createStrictShapeTypeChecker(shapeTypes) {
    function validate(props, propName, componentName, location, propFullName) {
      var propValue = props[propName];
      var propType = getPropType(propValue);
      if (propType !== 'object') {
        return new PropTypeError('Invalid ' + location + ' `' + propFullName + '` of type `' + propType + '` ' + ('supplied to `' + componentName + '`, expected `object`.'));
      }
      // We need to check all keys in case some are required but missing from
      // props.
      var allKeys = assign({}, props[propName], shapeTypes);
      for (var key in allKeys) {
        var checker = shapeTypes[key];
        if (!checker) {
          return new PropTypeError(
            'Invalid ' + location + ' `' + propFullName + '` key `' + key + '` supplied to `' + componentName + '`.' +
            '\nBad object: ' + JSON.stringify(props[propName], null, '  ') +
            '\nValid keys: ' +  JSON.stringify(Object.keys(shapeTypes), null, '  ')
          );
        }
        var error = checker(propValue, key, componentName, location, propFullName + '.' + key, ReactPropTypesSecret);
        if (error) {
          return error;
        }
      }
      return null;
    }

    return createChainableTypeChecker(validate);
  }

  function isNode(propValue) {
    switch (typeof propValue) {
      case 'number':
      case 'string':
      case 'undefined':
        return true;
      case 'boolean':
        return !propValue;
      case 'object':
        if (Array.isArray(propValue)) {
          return propValue.every(isNode);
        }
        if (propValue === null || isValidElement(propValue)) {
          return true;
        }

        var iteratorFn = getIteratorFn(propValue);
        if (iteratorFn) {
          var iterator = iteratorFn.call(propValue);
          var step;
          if (iteratorFn !== propValue.entries) {
            while (!(step = iterator.next()).done) {
              if (!isNode(step.value)) {
                return false;
              }
            }
          } else {
            // Iterator will provide entry [k,v] tuples rather than values.
            while (!(step = iterator.next()).done) {
              var entry = step.value;
              if (entry) {
                if (!isNode(entry[1])) {
                  return false;
                }
              }
            }
          }
        } else {
          return false;
        }

        return true;
      default:
        return false;
    }
  }

  function isSymbol(propType, propValue) {
    // Native Symbol.
    if (propType === 'symbol') {
      return true;
    }

    // 19.4.3.5 Symbol.prototype[@@toStringTag] === 'Symbol'
    if (propValue['@@toStringTag'] === 'Symbol') {
      return true;
    }

    // Fallback for non-spec compliant Symbols which are polyfilled.
    if (typeof Symbol === 'function' && propValue instanceof Symbol) {
      return true;
    }

    return false;
  }

  // Equivalent of `typeof` but with special handling for array and regexp.
  function getPropType(propValue) {
    var propType = typeof propValue;
    if (Array.isArray(propValue)) {
      return 'array';
    }
    if (propValue instanceof RegExp) {
      // Old webkits (at least until Android 4.0) return 'function' rather than
      // 'object' for typeof a RegExp. We'll normalize this here so that /bla/
      // passes PropTypes.object.
      return 'object';
    }
    if (isSymbol(propType, propValue)) {
      return 'symbol';
    }
    return propType;
  }

  // This handles more types than `getPropType`. Only used for error messages.
  // See `createPrimitiveTypeChecker`.
  function getPreciseType(propValue) {
    if (typeof propValue === 'undefined' || propValue === null) {
      return '' + propValue;
    }
    var propType = getPropType(propValue);
    if (propType === 'object') {
      if (propValue instanceof Date) {
        return 'date';
      } else if (propValue instanceof RegExp) {
        return 'regexp';
      }
    }
    return propType;
  }

  // Returns a string that is postfixed to a warning about an invalid type.
  // For example, "undefined" or "of type array"
  function getPostfixForTypeWarning(value) {
    var type = getPreciseType(value);
    switch (type) {
      case 'array':
      case 'object':
        return 'an ' + type;
      case 'boolean':
      case 'date':
      case 'regexp':
        return 'a ' + type;
      default:
        return type;
    }
  }

  // Returns class name of the object, if any.
  function getClassName(propValue) {
    if (!propValue.constructor || !propValue.constructor.name) {
      return ANONYMOUS;
    }
    return propValue.constructor.name;
  }

  ReactPropTypes.checkPropTypes = checkPropTypes;
  ReactPropTypes.PropTypes = ReactPropTypes;

  return ReactPropTypes;
};


/***/ }),

/***/ "../../node_modules/prop-types/index.js":
/*!******************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/prop-types/index.js ***!
  \******************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

if (true) {
  var REACT_ELEMENT_TYPE = (typeof Symbol === 'function' &&
    Symbol.for &&
    Symbol.for('react.element')) ||
    0xeac7;

  var isValidElement = function(object) {
    return typeof object === 'object' &&
      object !== null &&
      object.$$typeof === REACT_ELEMENT_TYPE;
  };

  // By explicitly using `prop-types` you are opting into new development behavior.
  // http://fb.me/prop-types-in-prod
  var throwOnDirectAccess = true;
  module.exports = __webpack_require__(/*! ./factoryWithTypeCheckers */ "../../node_modules/prop-types/factoryWithTypeCheckers.js")(isValidElement, throwOnDirectAccess);
} else {}


/***/ }),

/***/ "../../node_modules/prop-types/lib/ReactPropTypesSecret.js":
/*!*************************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/prop-types/lib/ReactPropTypesSecret.js ***!
  \*************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright (c) 2013-present, Facebook, Inc.
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */



var ReactPropTypesSecret = 'SECRET_DO_NOT_PASS_THIS_OR_YOU_WILL_BE_FIRED';

module.exports = ReactPropTypesSecret;


/***/ }),

/***/ "../../node_modules/query-string/index.js":
/*!********************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/query-string/index.js ***!
  \********************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var strictUriEncode = __webpack_require__(/*! strict-uri-encode */ "../../node_modules/strict-uri-encode/index.js");
var objectAssign = __webpack_require__(/*! object-assign */ "../../node_modules/object-assign/index.js");
var decodeComponent = __webpack_require__(/*! decode-uri-component */ "../../node_modules/decode-uri-component/index.js");

function encoderForArrayFormat(opts) {
	switch (opts.arrayFormat) {
		case 'index':
			return function (key, value, index) {
				return value === null ? [
					encode(key, opts),
					'[',
					index,
					']'
				].join('') : [
					encode(key, opts),
					'[',
					encode(index, opts),
					']=',
					encode(value, opts)
				].join('');
			};

		case 'bracket':
			return function (key, value) {
				return value === null ? encode(key, opts) : [
					encode(key, opts),
					'[]=',
					encode(value, opts)
				].join('');
			};

		default:
			return function (key, value) {
				return value === null ? encode(key, opts) : [
					encode(key, opts),
					'=',
					encode(value, opts)
				].join('');
			};
	}
}

function parserForArrayFormat(opts) {
	var result;

	switch (opts.arrayFormat) {
		case 'index':
			return function (key, value, accumulator) {
				result = /\[(\d*)\]$/.exec(key);

				key = key.replace(/\[\d*\]$/, '');

				if (!result) {
					accumulator[key] = value;
					return;
				}

				if (accumulator[key] === undefined) {
					accumulator[key] = {};
				}

				accumulator[key][result[1]] = value;
			};

		case 'bracket':
			return function (key, value, accumulator) {
				result = /(\[\])$/.exec(key);
				key = key.replace(/\[\]$/, '');

				if (!result) {
					accumulator[key] = value;
					return;
				} else if (accumulator[key] === undefined) {
					accumulator[key] = [value];
					return;
				}

				accumulator[key] = [].concat(accumulator[key], value);
			};

		default:
			return function (key, value, accumulator) {
				if (accumulator[key] === undefined) {
					accumulator[key] = value;
					return;
				}

				accumulator[key] = [].concat(accumulator[key], value);
			};
	}
}

function encode(value, opts) {
	if (opts.encode) {
		return opts.strict ? strictUriEncode(value) : encodeURIComponent(value);
	}

	return value;
}

function keysSorter(input) {
	if (Array.isArray(input)) {
		return input.sort();
	} else if (typeof input === 'object') {
		return keysSorter(Object.keys(input)).sort(function (a, b) {
			return Number(a) - Number(b);
		}).map(function (key) {
			return input[key];
		});
	}

	return input;
}

function extract(str) {
	var queryStart = str.indexOf('?');
	if (queryStart === -1) {
		return '';
	}
	return str.slice(queryStart + 1);
}

function parse(str, opts) {
	opts = objectAssign({arrayFormat: 'none'}, opts);

	var formatter = parserForArrayFormat(opts);

	// Create an object with no prototype
	// https://github.com/sindresorhus/query-string/issues/47
	var ret = Object.create(null);

	if (typeof str !== 'string') {
		return ret;
	}

	str = str.trim().replace(/^[?#&]/, '');

	if (!str) {
		return ret;
	}

	str.split('&').forEach(function (param) {
		var parts = param.replace(/\+/g, ' ').split('=');
		// Firefox (pre 40) decodes `%3D` to `=`
		// https://github.com/sindresorhus/query-string/pull/37
		var key = parts.shift();
		var val = parts.length > 0 ? parts.join('=') : undefined;

		// missing `=` should be `null`:
		// http://w3.org/TR/2012/WD-url-20120524/#collect-url-parameters
		val = val === undefined ? null : decodeComponent(val);

		formatter(decodeComponent(key), val, ret);
	});

	return Object.keys(ret).sort().reduce(function (result, key) {
		var val = ret[key];
		if (Boolean(val) && typeof val === 'object' && !Array.isArray(val)) {
			// Sort object keys, not values
			result[key] = keysSorter(val);
		} else {
			result[key] = val;
		}

		return result;
	}, Object.create(null));
}

exports.extract = extract;
exports.parse = parse;

exports.stringify = function (obj, opts) {
	var defaults = {
		encode: true,
		strict: true,
		arrayFormat: 'none'
	};

	opts = objectAssign(defaults, opts);

	if (opts.sort === false) {
		opts.sort = function () {};
	}

	var formatter = encoderForArrayFormat(opts);

	return obj ? Object.keys(obj).sort(opts.sort).map(function (key) {
		var val = obj[key];

		if (val === undefined) {
			return '';
		}

		if (val === null) {
			return encode(key, opts);
		}

		if (Array.isArray(val)) {
			var result = [];

			val.slice().forEach(function (val2) {
				if (val2 === undefined) {
					return;
				}

				result.push(formatter(key, val2, result.length));
			});

			return result.join('&');
		}

		return encode(key, opts) + '=' + encode(val, opts);
	}).filter(function (x) {
		return x.length > 0;
	}).join('&') : '';
};

exports.parseUrl = function (str, opts) {
	return {
		url: str.split('?')[0] || '',
		query: parse(extract(str), opts)
	};
};


/***/ }),

/***/ "../../node_modules/react-datetime/DateTime.js":
/*!*************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/DateTime.js ***!
  \*************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var assign = __webpack_require__(/*! object-assign */ "../../node_modules/react-datetime/node_modules/object-assign/index.js"),
	PropTypes = __webpack_require__(/*! prop-types */ "../../node_modules/prop-types/index.js"),
	createClass = __webpack_require__(/*! create-react-class */ "../../node_modules/create-react-class/index.js"),
	moment = __webpack_require__(/*! moment */ "moment"),
	React = __webpack_require__(/*! react */ "react"),
	CalendarContainer = __webpack_require__(/*! ./src/CalendarContainer */ "../../node_modules/react-datetime/src/CalendarContainer.js")
	;

var viewModes = Object.freeze({
	YEARS: 'years',
	MONTHS: 'months',
	DAYS: 'days',
	TIME: 'time',
});

var TYPES = PropTypes;
var Datetime = createClass({
	propTypes: {
		// value: TYPES.object | TYPES.string,
		// defaultValue: TYPES.object | TYPES.string,
		// viewDate: TYPES.object | TYPES.string,
		onFocus: TYPES.func,
		onBlur: TYPES.func,
		onChange: TYPES.func,
		onViewModeChange: TYPES.func,
		locale: TYPES.string,
		utc: TYPES.bool,
		input: TYPES.bool,
		// dateFormat: TYPES.string | TYPES.bool,
		// timeFormat: TYPES.string | TYPES.bool,
		inputProps: TYPES.object,
		timeConstraints: TYPES.object,
		viewMode: TYPES.oneOf([viewModes.YEARS, viewModes.MONTHS, viewModes.DAYS, viewModes.TIME]),
		isValidDate: TYPES.func,
		open: TYPES.bool,
		strictParsing: TYPES.bool,
		closeOnSelect: TYPES.bool,
		closeOnTab: TYPES.bool
	},

	getInitialState: function() {
		var state = this.getStateFromProps( this.props );

		if ( state.open === undefined )
			state.open = !this.props.input;

		state.currentView = this.props.dateFormat ?
			(this.props.viewMode || state.updateOn || viewModes.DAYS) : viewModes.TIME;

		return state;
	},

	parseDate: function (date, formats) {
		var parsedDate;

		if (date && typeof date === 'string')
			parsedDate = this.localMoment(date, formats.datetime);
		else if (date)
			parsedDate = this.localMoment(date);

		if (parsedDate && !parsedDate.isValid())
			parsedDate = null;

		return parsedDate;
	},

	getStateFromProps: function( props ) {
		var formats = this.getFormats( props ),
			date = props.value || props.defaultValue,
			selectedDate, viewDate, updateOn, inputValue
			;

		selectedDate = this.parseDate(date, formats);

		viewDate = this.parseDate(props.viewDate, formats);

		viewDate = selectedDate ?
			selectedDate.clone().startOf('month') :
			viewDate ? viewDate.clone().startOf('month') : this.localMoment().startOf('month');

		updateOn = this.getUpdateOn(formats);

		if ( selectedDate )
			inputValue = selectedDate.format(formats.datetime);
		else if ( date.isValid && !date.isValid() )
			inputValue = '';
		else
			inputValue = date || '';

		return {
			updateOn: updateOn,
			inputFormat: formats.datetime,
			viewDate: viewDate,
			selectedDate: selectedDate,
			inputValue: inputValue,
			open: props.open
		};
	},

	getUpdateOn: function( formats ) {
		if ( formats.date.match(/[lLD]/) ) {
			return viewModes.DAYS;
		} else if ( formats.date.indexOf('M') !== -1 ) {
			return viewModes.MONTHS;
		} else if ( formats.date.indexOf('Y') !== -1 ) {
			return viewModes.YEARS;
		}

		return viewModes.DAYS;
	},

	getFormats: function( props ) {
		var formats = {
				date: props.dateFormat || '',
				time: props.timeFormat || ''
			},
			locale = this.localMoment( props.date, null, props ).localeData()
			;

		if ( formats.date === true ) {
			formats.date = locale.longDateFormat('L');
		}
		else if ( this.getUpdateOn(formats) !== viewModes.DAYS ) {
			formats.time = '';
		}

		if ( formats.time === true ) {
			formats.time = locale.longDateFormat('LT');
		}

		formats.datetime = formats.date && formats.time ?
			formats.date + ' ' + formats.time :
			formats.date || formats.time
		;

		return formats;
	},

	componentWillReceiveProps: function( nextProps ) {
		var formats = this.getFormats( nextProps ),
			updatedState = {}
		;

		if ( nextProps.value !== this.props.value ||
			formats.datetime !== this.getFormats( this.props ).datetime ) {
			updatedState = this.getStateFromProps( nextProps );
		}

		if ( updatedState.open === undefined ) {
			if ( typeof nextProps.open !== 'undefined' ) {
				updatedState.open = nextProps.open;
			} else if ( this.props.closeOnSelect && this.state.currentView !== viewModes.TIME ) {
				updatedState.open = false;
			} else {
				updatedState.open = this.state.open;
			}
		}

		if ( nextProps.viewMode !== this.props.viewMode ) {
			updatedState.currentView = nextProps.viewMode;
		}

		if ( nextProps.locale !== this.props.locale ) {
			if ( this.state.viewDate ) {
				var updatedViewDate = this.state.viewDate.clone().locale( nextProps.locale );
				updatedState.viewDate = updatedViewDate;
			}
			if ( this.state.selectedDate ) {
				var updatedSelectedDate = this.state.selectedDate.clone().locale( nextProps.locale );
				updatedState.selectedDate = updatedSelectedDate;
				updatedState.inputValue = updatedSelectedDate.format( formats.datetime );
			}
		}

		if ( nextProps.utc !== this.props.utc ) {
			if ( nextProps.utc ) {
				if ( this.state.viewDate )
					updatedState.viewDate = this.state.viewDate.clone().utc();
				if ( this.state.selectedDate ) {
					updatedState.selectedDate = this.state.selectedDate.clone().utc();
					updatedState.inputValue = updatedState.selectedDate.format( formats.datetime );
				}
			} else {
				if ( this.state.viewDate )
					updatedState.viewDate = this.state.viewDate.clone().local();
				if ( this.state.selectedDate ) {
					updatedState.selectedDate = this.state.selectedDate.clone().local();
					updatedState.inputValue = updatedState.selectedDate.format(formats.datetime);
				}
			}
		}

		if ( nextProps.viewDate !== this.props.viewDate ) {
			updatedState.viewDate = moment(nextProps.viewDate);
		}
		//we should only show a valid date if we are provided a isValidDate function. Removed in 2.10.3
		/*if (this.props.isValidDate) {
			updatedState.viewDate = updatedState.viewDate || this.state.viewDate;
			while (!this.props.isValidDate(updatedState.viewDate)) {
				updatedState.viewDate = updatedState.viewDate.add(1, 'day');
			}
		}*/
		this.setState( updatedState );
	},

	onInputChange: function( e ) {
		var value = e.target === null ? e : e.target.value,
			localMoment = this.localMoment( value, this.state.inputFormat ),
			update = { inputValue: value }
			;

		if ( localMoment.isValid() && !this.props.value ) {
			update.selectedDate = localMoment;
			update.viewDate = localMoment.clone().startOf('month');
		} else {
			update.selectedDate = null;
		}

		return this.setState( update, function() {
			return this.props.onChange( localMoment.isValid() ? localMoment : this.state.inputValue );
		});
	},

	onInputKey: function( e ) {
		if ( e.which === 9 && this.props.closeOnTab ) {
			this.closeCalendar();
		}
	},

	showView: function( view ) {
		var me = this;
		return function() {
			me.state.currentView !== view && me.props.onViewModeChange( view );
			me.setState({ currentView: view });
		};
	},

	setDate: function( type ) {
		var me = this,
			nextViews = {
				month: viewModes.DAYS,
				year: viewModes.MONTHS,
			}
		;
		return function( e ) {
			me.setState({
				viewDate: me.state.viewDate.clone()[ type ]( parseInt(e.target.getAttribute('data-value'), 10) ).startOf( type ),
				currentView: nextViews[ type ]
			});
			me.props.onViewModeChange( nextViews[ type ] );
		};
	},

	addTime: function( amount, type, toSelected ) {
		return this.updateTime( 'add', amount, type, toSelected );
	},

	subtractTime: function( amount, type, toSelected ) {
		return this.updateTime( 'subtract', amount, type, toSelected );
	},

	updateTime: function( op, amount, type, toSelected ) {
		var me = this;

		return function() {
			var update = {},
				date = toSelected ? 'selectedDate' : 'viewDate'
			;

			update[ date ] = me.state[ date ].clone()[ op ]( amount, type );

			me.setState( update );
		};
	},

	allowedSetTime: ['hours', 'minutes', 'seconds', 'milliseconds'],
	setTime: function( type, value ) {
		var index = this.allowedSetTime.indexOf( type ) + 1,
			state = this.state,
			date = (state.selectedDate || state.viewDate).clone(),
			nextType
			;

		// It is needed to set all the time properties
		// to not to reset the time
		date[ type ]( value );
		for (; index < this.allowedSetTime.length; index++) {
			nextType = this.allowedSetTime[index];
			date[ nextType ]( date[nextType]() );
		}

		if ( !this.props.value ) {
			this.setState({
				selectedDate: date,
				inputValue: date.format( state.inputFormat )
			});
		}
		this.props.onChange( date );
	},

	updateSelectedDate: function( e, close ) {
		var target = e.target,
			modifier = 0,
			viewDate = this.state.viewDate,
			currentDate = this.state.selectedDate || viewDate,
			date
			;

		if (target.className.indexOf('rdtDay') !== -1) {
			if (target.className.indexOf('rdtNew') !== -1)
				modifier = 1;
			else if (target.className.indexOf('rdtOld') !== -1)
				modifier = -1;

			date = viewDate.clone()
				.month( viewDate.month() + modifier )
				.date( parseInt( target.getAttribute('data-value'), 10 ) );
		} else if (target.className.indexOf('rdtMonth') !== -1) {
			date = viewDate.clone()
				.month( parseInt( target.getAttribute('data-value'), 10 ) )
				.date( currentDate.date() );
		} else if (target.className.indexOf('rdtYear') !== -1) {
			date = viewDate.clone()
				.month( currentDate.month() )
				.date( currentDate.date() )
				.year( parseInt( target.getAttribute('data-value'), 10 ) );
		}

		date.hours( currentDate.hours() )
			.minutes( currentDate.minutes() )
			.seconds( currentDate.seconds() )
			.milliseconds( currentDate.milliseconds() );

		if ( !this.props.value ) {
			var open = !( this.props.closeOnSelect && close );
			if ( !open ) {
				this.props.onBlur( date );
			}

			this.setState({
				selectedDate: date,
				viewDate: date.clone().startOf('month'),
				inputValue: date.format( this.state.inputFormat ),
				open: open
			});
		} else {
			if ( this.props.closeOnSelect && close ) {
				this.closeCalendar();
			}
		}

		this.props.onChange( date );
	},

	openCalendar: function( e ) {
		if ( !this.state.open ) {
			this.setState({ open: true }, function() {
				this.props.onFocus( e );
			});
		}
	},

	closeCalendar: function() {
		this.setState({ open: false }, function () {
			this.props.onBlur( this.state.selectedDate || this.state.inputValue );
		});
	},

	handleClickOutside: function() {
		if ( this.props.input && this.state.open && !this.props.open && !this.props.disableOnClickOutside ) {
			this.setState({ open: false }, function() {
				this.props.onBlur( this.state.selectedDate || this.state.inputValue );
			});
		}
	},

	localMoment: function( date, format, props ) {
		props = props || this.props;
		var momentFn = props.utc ? moment.utc : moment;
		var m = momentFn( date, format, props.strictParsing );
		if ( props.locale )
			m.locale( props.locale );
		return m;
	},

	componentProps: {
		fromProps: ['value', 'isValidDate', 'renderDay', 'renderMonth', 'renderYear', 'timeConstraints'],
		fromState: ['viewDate', 'selectedDate', 'updateOn'],
		fromThis: ['setDate', 'setTime', 'showView', 'addTime', 'subtractTime', 'updateSelectedDate', 'localMoment', 'handleClickOutside']
	},

	getComponentProps: function() {
		var me = this,
			formats = this.getFormats( this.props ),
			props = {dateFormat: formats.date, timeFormat: formats.time}
			;

		this.componentProps.fromProps.forEach( function( name ) {
			props[ name ] = me.props[ name ];
		});
		this.componentProps.fromState.forEach( function( name ) {
			props[ name ] = me.state[ name ];
		});
		this.componentProps.fromThis.forEach( function( name ) {
			props[ name ] = me[ name ];
		});

		return props;
	},

	render: function() {
		// TODO: Make a function or clean up this code,
		// logic right now is really hard to follow
		var className = 'rdt' + (this.props.className ?
                  ( Array.isArray( this.props.className ) ?
                  ' ' + this.props.className.join( ' ' ) : ' ' + this.props.className) : ''),
			children = [];

		if ( this.props.input ) {
			var finalInputProps = assign({
				type: 'text',
				className: 'form-control',
				onClick: this.openCalendar,
				onFocus: this.openCalendar,
				onChange: this.onInputChange,
				onKeyDown: this.onInputKey,
				value: this.state.inputValue,
			}, this.props.inputProps);
			if ( this.props.renderInput ) {
				children = [ React.createElement('div', { key: 'i' }, this.props.renderInput( finalInputProps, this.openCalendar, this.closeCalendar )) ];
			} else {
				children = [ React.createElement('input', assign({ key: 'i' }, finalInputProps ))];
			}
		} else {
			className += ' rdtStatic';
		}

		if ( this.state.open )
			className += ' rdtOpen';

		return React.createElement( 'div', { className: className }, children.concat(
			React.createElement( 'div',
				{ key: 'dt', className: 'rdtPicker' },
				React.createElement( CalendarContainer, { view: this.state.currentView, viewProps: this.getComponentProps(), onClickOutside: this.handleClickOutside })
			)
		));
	}
});

Datetime.defaultProps = {
	className: '',
	defaultValue: '',
	inputProps: {},
	input: true,
	onFocus: function() {},
	onBlur: function() {},
	onChange: function() {},
	onViewModeChange: function() {},
	timeFormat: true,
	timeConstraints: {},
	dateFormat: true,
	strictParsing: true,
	closeOnSelect: false,
	closeOnTab: true,
	utc: false
};

// Make moment accessible through the Datetime class
Datetime.moment = moment;

module.exports = Datetime;


/***/ }),

/***/ "../../node_modules/react-datetime/css/react-datetime.css":
/*!************************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/css/react-datetime.css ***!
  \************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {


var content = __webpack_require__(/*! !../../css-loader/dist/cjs.js!./react-datetime.css */ "../../node_modules/css-loader/dist/cjs.js!../../node_modules/react-datetime/css/react-datetime.css");

if(typeof content === 'string') content = [[module.i, content, '']];

var transform;
var insertInto;



var options = {"hmr":true}

options.transform = transform
options.insertInto = undefined;

var update = __webpack_require__(/*! ../../style-loader/lib/addStyles.js */ "../../node_modules/style-loader/lib/addStyles.js")(content, options);

if(content.locals) module.exports = content.locals;

if(false) {}

/***/ }),

/***/ "../../node_modules/react-datetime/node_modules/object-assign/index.js":
/*!*************************************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/node_modules/object-assign/index.js ***!
  \*************************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var propIsEnumerable = Object.prototype.propertyIsEnumerable;

function ToObject(val) {
	if (val == null) {
		throw new TypeError('Object.assign cannot be called with null or undefined');
	}

	return Object(val);
}

function ownEnumerableKeys(obj) {
	var keys = Object.getOwnPropertyNames(obj);

	if (Object.getOwnPropertySymbols) {
		keys = keys.concat(Object.getOwnPropertySymbols(obj));
	}

	return keys.filter(function (key) {
		return propIsEnumerable.call(obj, key);
	});
}

module.exports = Object.assign || function (target, source) {
	var from;
	var keys;
	var to = ToObject(target);

	for (var s = 1; s < arguments.length; s++) {
		from = arguments[s];
		keys = ownEnumerableKeys(Object(from));

		for (var i = 0; i < keys.length; i++) {
			to[keys[i]] = from[keys[i]];
		}
	}

	return to;
};


/***/ }),

/***/ "../../node_modules/react-datetime/src/CalendarContainer.js":
/*!**************************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/src/CalendarContainer.js ***!
  \**************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var React = __webpack_require__(/*! react */ "react"),
	createClass = __webpack_require__(/*! create-react-class */ "../../node_modules/create-react-class/index.js"),
	DaysView = __webpack_require__(/*! ./DaysView */ "../../node_modules/react-datetime/src/DaysView.js"),
	MonthsView = __webpack_require__(/*! ./MonthsView */ "../../node_modules/react-datetime/src/MonthsView.js"),
	YearsView = __webpack_require__(/*! ./YearsView */ "../../node_modules/react-datetime/src/YearsView.js"),
	TimeView = __webpack_require__(/*! ./TimeView */ "../../node_modules/react-datetime/src/TimeView.js")
	;

var CalendarContainer = createClass({
	viewComponents: {
		days: DaysView,
		months: MonthsView,
		years: YearsView,
		time: TimeView
	},

	render: function() {
		return React.createElement( this.viewComponents[ this.props.view ], this.props.viewProps );
	}
});

module.exports = CalendarContainer;


/***/ }),

/***/ "../../node_modules/react-datetime/src/DaysView.js":
/*!*****************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/src/DaysView.js ***!
  \*****************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var React = __webpack_require__(/*! react */ "react"),
	createClass = __webpack_require__(/*! create-react-class */ "../../node_modules/create-react-class/index.js"),
	moment = __webpack_require__(/*! moment */ "moment"),
	onClickOutside = __webpack_require__(/*! react-onclickoutside */ "../../node_modules/react-onclickoutside/dist/react-onclickoutside.es.js").default
	;

var DateTimePickerDays = onClickOutside( createClass({
	render: function() {
		var footer = this.renderFooter(),
			date = this.props.viewDate,
			locale = date.localeData(),
			tableChildren
			;

		tableChildren = [
			React.createElement('thead', { key: 'th' }, [
				React.createElement('tr', { key: 'h' }, [
					React.createElement('th', { key: 'p', className: 'rdtPrev', onClick: this.props.subtractTime( 1, 'months' )}, React.createElement('span', {}, '‹' )),
					React.createElement('th', { key: 's', className: 'rdtSwitch', onClick: this.props.showView( 'months' ), colSpan: 5, 'data-value': this.props.viewDate.month() }, locale.months( date ) + ' ' + date.year() ),
					React.createElement('th', { key: 'n', className: 'rdtNext', onClick: this.props.addTime( 1, 'months' )}, React.createElement('span', {}, '›' ))
				]),
				React.createElement('tr', { key: 'd'}, this.getDaysOfWeek( locale ).map( function( day, index ) { return React.createElement('th', { key: day + index, className: 'dow'}, day ); }) )
			]),
			React.createElement('tbody', { key: 'tb' }, this.renderDays())
		];

		if ( footer )
			tableChildren.push( footer );

		return React.createElement('div', { className: 'rdtDays' },
			React.createElement('table', {}, tableChildren )
		);
	},

	/**
	 * Get a list of the days of the week
	 * depending on the current locale
	 * @return {array} A list with the shortname of the days
	 */
	getDaysOfWeek: function( locale ) {
		var days = locale._weekdaysMin,
			first = locale.firstDayOfWeek(),
			dow = [],
			i = 0
			;

		days.forEach( function( day ) {
			dow[ (7 + ( i++ ) - first) % 7 ] = day;
		});

		return dow;
	},

	renderDays: function() {
		var date = this.props.viewDate,
			selected = this.props.selectedDate && this.props.selectedDate.clone(),
			prevMonth = date.clone().subtract( 1, 'months' ),
			currentYear = date.year(),
			currentMonth = date.month(),
			weeks = [],
			days = [],
			renderer = this.props.renderDay || this.renderDay,
			isValid = this.props.isValidDate || this.alwaysValidDate,
			classes, isDisabled, dayProps, currentDate
			;

		// Go to the last week of the previous month
		prevMonth.date( prevMonth.daysInMonth() ).startOf( 'week' );
		var lastDay = prevMonth.clone().add( 42, 'd' );

		while ( prevMonth.isBefore( lastDay ) ) {
			classes = 'rdtDay';
			currentDate = prevMonth.clone();

			if ( ( prevMonth.year() === currentYear && prevMonth.month() < currentMonth ) || ( prevMonth.year() < currentYear ) )
				classes += ' rdtOld';
			else if ( ( prevMonth.year() === currentYear && prevMonth.month() > currentMonth ) || ( prevMonth.year() > currentYear ) )
				classes += ' rdtNew';

			if ( selected && prevMonth.isSame( selected, 'day' ) )
				classes += ' rdtActive';

			if ( prevMonth.isSame( moment(), 'day' ) )
				classes += ' rdtToday';

			isDisabled = !isValid( currentDate, selected );
			if ( isDisabled )
				classes += ' rdtDisabled';

			dayProps = {
				key: prevMonth.format( 'M_D' ),
				'data-value': prevMonth.date(),
				className: classes
			};

			if ( !isDisabled )
				dayProps.onClick = this.updateSelectedDate;

			days.push( renderer( dayProps, currentDate, selected ) );

			if ( days.length === 7 ) {
				weeks.push( React.createElement('tr', { key: prevMonth.format( 'M_D' )}, days ) );
				days = [];
			}

			prevMonth.add( 1, 'd' );
		}

		return weeks;
	},

	updateSelectedDate: function( event ) {
		this.props.updateSelectedDate( event, true );
	},

	renderDay: function( props, currentDate ) {
		return React.createElement('td',  props, currentDate.date() );
	},

	renderFooter: function() {
		if ( !this.props.timeFormat )
			return '';

		var date = this.props.selectedDate || this.props.viewDate;

		return React.createElement('tfoot', { key: 'tf'},
			React.createElement('tr', {},
				React.createElement('td', { onClick: this.props.showView( 'time' ), colSpan: 7, className: 'rdtTimeToggle' }, date.format( this.props.timeFormat ))
			)
		);
	},

	alwaysValidDate: function() {
		return 1;
	},

	handleClickOutside: function() {
		this.props.handleClickOutside();
	}
}));

module.exports = DateTimePickerDays;


/***/ }),

/***/ "../../node_modules/react-datetime/src/MonthsView.js":
/*!*******************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/src/MonthsView.js ***!
  \*******************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var React = __webpack_require__(/*! react */ "react"),
	createClass = __webpack_require__(/*! create-react-class */ "../../node_modules/create-react-class/index.js"),
	onClickOutside = __webpack_require__(/*! react-onclickoutside */ "../../node_modules/react-onclickoutside/dist/react-onclickoutside.es.js").default
	;

var DateTimePickerMonths = onClickOutside( createClass({
	render: function() {
		return React.createElement('div', { className: 'rdtMonths' }, [
			React.createElement('table', { key: 'a' }, React.createElement('thead', {}, React.createElement('tr', {}, [
				React.createElement('th', { key: 'prev', className: 'rdtPrev', onClick: this.props.subtractTime( 1, 'years' )}, React.createElement('span', {}, '‹' )),
				React.createElement('th', { key: 'year', className: 'rdtSwitch', onClick: this.props.showView( 'years' ), colSpan: 2, 'data-value': this.props.viewDate.year() }, this.props.viewDate.year() ),
				React.createElement('th', { key: 'next', className: 'rdtNext', onClick: this.props.addTime( 1, 'years' )}, React.createElement('span', {}, '›' ))
			]))),
			React.createElement('table', { key: 'months' }, React.createElement('tbody', { key: 'b' }, this.renderMonths()))
		]);
	},

	renderMonths: function() {
		var date = this.props.selectedDate,
			month = this.props.viewDate.month(),
			year = this.props.viewDate.year(),
			rows = [],
			i = 0,
			months = [],
			renderer = this.props.renderMonth || this.renderMonth,
			isValid = this.props.isValidDate || this.alwaysValidDate,
			classes, props, currentMonth, isDisabled, noOfDaysInMonth, daysInMonth, validDay,
			// Date is irrelevant because we're only interested in month
			irrelevantDate = 1
			;

		while (i < 12) {
			classes = 'rdtMonth';
			currentMonth =
				this.props.viewDate.clone().set({ year: year, month: i, date: irrelevantDate });

			noOfDaysInMonth = currentMonth.endOf( 'month' ).format( 'D' );
			daysInMonth = Array.from({ length: noOfDaysInMonth }, function( e, i ) {
				return i + 1;
			});

			validDay = daysInMonth.find(function( d ) {
				var day = currentMonth.clone().set( 'date', d );
				return isValid( day );
			});

			isDisabled = ( validDay === undefined );

			if ( isDisabled )
				classes += ' rdtDisabled';

			if ( date && i === date.month() && year === date.year() )
				classes += ' rdtActive';

			props = {
				key: i,
				'data-value': i,
				className: classes
			};

			if ( !isDisabled )
				props.onClick = ( this.props.updateOn === 'months' ?
					this.updateSelectedMonth : this.props.setDate( 'month' ) );

			months.push( renderer( props, i, year, date && date.clone() ) );

			if ( months.length === 4 ) {
				rows.push( React.createElement('tr', { key: month + '_' + rows.length }, months ) );
				months = [];
			}

			i++;
		}

		return rows;
	},

	updateSelectedMonth: function( event ) {
		this.props.updateSelectedDate( event );
	},

	renderMonth: function( props, month ) {
		var localMoment = this.props.viewDate;
		var monthStr = localMoment.localeData().monthsShort( localMoment.month( month ) );
		var strLength = 3;
		// Because some months are up to 5 characters long, we want to
		// use a fixed string length for consistency
		var monthStrFixedLength = monthStr.substring( 0, strLength );
		return React.createElement('td', props, capitalize( monthStrFixedLength ) );
	},

	alwaysValidDate: function() {
		return 1;
	},

	handleClickOutside: function() {
		this.props.handleClickOutside();
	}
}));

function capitalize( str ) {
	return str.charAt( 0 ).toUpperCase() + str.slice( 1 );
}

module.exports = DateTimePickerMonths;


/***/ }),

/***/ "../../node_modules/react-datetime/src/TimeView.js":
/*!*****************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/src/TimeView.js ***!
  \*****************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var React = __webpack_require__(/*! react */ "react"),
	createClass = __webpack_require__(/*! create-react-class */ "../../node_modules/create-react-class/index.js"),
	assign = __webpack_require__(/*! object-assign */ "../../node_modules/react-datetime/node_modules/object-assign/index.js"),
	onClickOutside = __webpack_require__(/*! react-onclickoutside */ "../../node_modules/react-onclickoutside/dist/react-onclickoutside.es.js").default
	;

var DateTimePickerTime = onClickOutside( createClass({
	getInitialState: function() {
		return this.calculateState( this.props );
	},

	calculateState: function( props ) {
		var date = props.selectedDate || props.viewDate,
			format = props.timeFormat,
			counters = []
			;

		if ( format.toLowerCase().indexOf('h') !== -1 ) {
			counters.push('hours');
			if ( format.indexOf('m') !== -1 ) {
				counters.push('minutes');
				if ( format.indexOf('s') !== -1 ) {
					counters.push('seconds');
				}
			}
		}

		var hours = date.format( 'H' );
		
		var daypart = false;
		if ( this.state !== null && this.props.timeFormat.toLowerCase().indexOf( ' a' ) !== -1 ) {
			if ( this.props.timeFormat.indexOf( ' A' ) !== -1 ) {
				daypart = ( hours >= 12 ) ? 'PM' : 'AM';
			} else {
				daypart = ( hours >= 12 ) ? 'pm' : 'am';
			}
		}

		return {
			hours: hours,
			minutes: date.format( 'mm' ),
			seconds: date.format( 'ss' ),
			milliseconds: date.format( 'SSS' ),
			daypart: daypart,
			counters: counters
		};
	},

	renderCounter: function( type ) {
		if ( type !== 'daypart' ) {
			var value = this.state[ type ];
			if ( type === 'hours' && this.props.timeFormat.toLowerCase().indexOf( ' a' ) !== -1 ) {
				value = ( value - 1 ) % 12 + 1;

				if ( value === 0 ) {
					value = 12;
				}
			}
			return React.createElement('div', { key: type, className: 'rdtCounter' }, [
				React.createElement('span', { key: 'up', className: 'rdtBtn', onTouchStart: this.onStartClicking('increase', type), onMouseDown: this.onStartClicking( 'increase', type ), onContextMenu: this.disableContextMenu }, '▲' ),
				React.createElement('div', { key: 'c', className: 'rdtCount' }, value ),
				React.createElement('span', { key: 'do', className: 'rdtBtn', onTouchStart: this.onStartClicking('decrease', type), onMouseDown: this.onStartClicking( 'decrease', type ), onContextMenu: this.disableContextMenu }, '▼' )
			]);
		}
		return '';
	},

	renderDayPart: function() {
		return React.createElement('div', { key: 'dayPart', className: 'rdtCounter' }, [
			React.createElement('span', { key: 'up', className: 'rdtBtn', onTouchStart: this.onStartClicking('toggleDayPart', 'hours'), onMouseDown: this.onStartClicking( 'toggleDayPart', 'hours'), onContextMenu: this.disableContextMenu }, '▲' ),
			React.createElement('div', { key: this.state.daypart, className: 'rdtCount' }, this.state.daypart ),
			React.createElement('span', { key: 'do', className: 'rdtBtn', onTouchStart: this.onStartClicking('toggleDayPart', 'hours'), onMouseDown: this.onStartClicking( 'toggleDayPart', 'hours'), onContextMenu: this.disableContextMenu }, '▼' )
		]);
	},

	render: function() {
		var me = this,
			counters = []
		;

		this.state.counters.forEach( function( c ) {
			if ( counters.length )
				counters.push( React.createElement('div', { key: 'sep' + counters.length, className: 'rdtCounterSeparator' }, ':' ) );
			counters.push( me.renderCounter( c ) );
		});

		if ( this.state.daypart !== false ) {
			counters.push( me.renderDayPart() );
		}

		if ( this.state.counters.length === 3 && this.props.timeFormat.indexOf( 'S' ) !== -1 ) {
			counters.push( React.createElement('div', { className: 'rdtCounterSeparator', key: 'sep5' }, ':' ) );
			counters.push(
				React.createElement('div', { className: 'rdtCounter rdtMilli', key: 'm' },
					React.createElement('input', { value: this.state.milliseconds, type: 'text', onChange: this.updateMilli } )
					)
				);
		}

		return React.createElement('div', { className: 'rdtTime' },
			React.createElement('table', {}, [
				this.renderHeader(),
				React.createElement('tbody', { key: 'b'}, React.createElement('tr', {}, React.createElement('td', {},
					React.createElement('div', { className: 'rdtCounters' }, counters )
				)))
			])
		);
	},

	componentWillMount: function() {
		var me = this;
		me.timeConstraints = {
			hours: {
				min: 0,
				max: 23,
				step: 1
			},
			minutes: {
				min: 0,
				max: 59,
				step: 1
			},
			seconds: {
				min: 0,
				max: 59,
				step: 1
			},
			milliseconds: {
				min: 0,
				max: 999,
				step: 1
			}
		};
		['hours', 'minutes', 'seconds', 'milliseconds'].forEach( function( type ) {
			assign(me.timeConstraints[ type ], me.props.timeConstraints[ type ]);
		});
		this.setState( this.calculateState( this.props ) );
	},

	componentWillReceiveProps: function( nextProps ) {
		this.setState( this.calculateState( nextProps ) );
	},

	updateMilli: function( e ) {
		var milli = parseInt( e.target.value, 10 );
		if ( milli === e.target.value && milli >= 0 && milli < 1000 ) {
			this.props.setTime( 'milliseconds', milli );
			this.setState( { milliseconds: milli } );
		}
	},

	renderHeader: function() {
		if ( !this.props.dateFormat )
			return null;

		var date = this.props.selectedDate || this.props.viewDate;
		return React.createElement('thead', { key: 'h' }, React.createElement('tr', {},
			React.createElement('th', { className: 'rdtSwitch', colSpan: 4, onClick: this.props.showView( 'days' ) }, date.format( this.props.dateFormat ) )
		));
	},

	onStartClicking: function( action, type ) {
		var me = this;

		return function() {
			var update = {};
			update[ type ] = me[ action ]( type );
			me.setState( update );

			me.timer = setTimeout( function() {
				me.increaseTimer = setInterval( function() {
					update[ type ] = me[ action ]( type );
					me.setState( update );
				}, 70);
			}, 500);

			me.mouseUpListener = function() {
				clearTimeout( me.timer );
				clearInterval( me.increaseTimer );
				me.props.setTime( type, me.state[ type ] );
				document.body.removeEventListener( 'mouseup', me.mouseUpListener );
				document.body.removeEventListener( 'touchend', me.mouseUpListener );
			};

			document.body.addEventListener( 'mouseup', me.mouseUpListener );
			document.body.addEventListener( 'touchend', me.mouseUpListener );
		};
	},

	disableContextMenu: function( event ) {
		event.preventDefault();
		return false;
	},

	padValues: {
		hours: 1,
		minutes: 2,
		seconds: 2,
		milliseconds: 3
	},

	toggleDayPart: function( type ) { // type is always 'hours'
		var value = parseInt( this.state[ type ], 10) + 12;
		if ( value > this.timeConstraints[ type ].max )
			value = this.timeConstraints[ type ].min + ( value - ( this.timeConstraints[ type ].max + 1 ) );
		return this.pad( type, value );
	},

	increase: function( type ) {
		var value = parseInt( this.state[ type ], 10) + this.timeConstraints[ type ].step;
		if ( value > this.timeConstraints[ type ].max )
			value = this.timeConstraints[ type ].min + ( value - ( this.timeConstraints[ type ].max + 1 ) );
		return this.pad( type, value );
	},

	decrease: function( type ) {
		var value = parseInt( this.state[ type ], 10) - this.timeConstraints[ type ].step;
		if ( value < this.timeConstraints[ type ].min )
			value = this.timeConstraints[ type ].max + 1 - ( this.timeConstraints[ type ].min - value );
		return this.pad( type, value );
	},

	pad: function( type, value ) {
		var str = value + '';
		while ( str.length < this.padValues[ type ] )
			str = '0' + str;
		return str;
	},

	handleClickOutside: function() {
		this.props.handleClickOutside();
	}
}));

module.exports = DateTimePickerTime;


/***/ }),

/***/ "../../node_modules/react-datetime/src/YearsView.js":
/*!******************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/src/YearsView.js ***!
  \******************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";


var React = __webpack_require__(/*! react */ "react"),
	createClass = __webpack_require__(/*! create-react-class */ "../../node_modules/create-react-class/index.js"),
	onClickOutside = __webpack_require__(/*! react-onclickoutside */ "../../node_modules/react-onclickoutside/dist/react-onclickoutside.es.js").default
	;

var DateTimePickerYears = onClickOutside( createClass({
	render: function() {
		var year = parseInt( this.props.viewDate.year() / 10, 10 ) * 10;

		return React.createElement('div', { className: 'rdtYears' }, [
			React.createElement('table', { key: 'a' }, React.createElement('thead', {}, React.createElement('tr', {}, [
				React.createElement('th', { key: 'prev', className: 'rdtPrev', onClick: this.props.subtractTime( 10, 'years' )}, React.createElement('span', {}, '‹' )),
				React.createElement('th', { key: 'year', className: 'rdtSwitch', onClick: this.props.showView( 'years' ), colSpan: 2 }, year + '-' + ( year + 9 ) ),
				React.createElement('th', { key: 'next', className: 'rdtNext', onClick: this.props.addTime( 10, 'years' )}, React.createElement('span', {}, '›' ))
			]))),
			React.createElement('table', { key: 'years' }, React.createElement('tbody',  {}, this.renderYears( year )))
		]);
	},

	renderYears: function( year ) {
		var years = [],
			i = -1,
			rows = [],
			renderer = this.props.renderYear || this.renderYear,
			selectedDate = this.props.selectedDate,
			isValid = this.props.isValidDate || this.alwaysValidDate,
			classes, props, currentYear, isDisabled, noOfDaysInYear, daysInYear, validDay,
			// Month and date are irrelevant here because
			// we're only interested in the year
			irrelevantMonth = 0,
			irrelevantDate = 1
			;

		year--;
		while (i < 11) {
			classes = 'rdtYear';
			currentYear = this.props.viewDate.clone().set(
				{ year: year, month: irrelevantMonth, date: irrelevantDate } );

			// Not sure what 'rdtOld' is for, commenting out for now as it's not working properly
			// if ( i === -1 | i === 10 )
				// classes += ' rdtOld';

			noOfDaysInYear = currentYear.endOf( 'year' ).format( 'DDD' );
			daysInYear = Array.from({ length: noOfDaysInYear }, function( e, i ) {
				return i + 1;
			});

			validDay = daysInYear.find(function( d ) {
				var day = currentYear.clone().dayOfYear( d );
				return isValid( day );
			});

			isDisabled = ( validDay === undefined );

			if ( isDisabled )
				classes += ' rdtDisabled';

			if ( selectedDate && selectedDate.year() === year )
				classes += ' rdtActive';

			props = {
				key: year,
				'data-value': year,
				className: classes
			};

			if ( !isDisabled )
				props.onClick = ( this.props.updateOn === 'years' ?
					this.updateSelectedYear : this.props.setDate('year') );

			years.push( renderer( props, year, selectedDate && selectedDate.clone() ));

			if ( years.length === 4 ) {
				rows.push( React.createElement('tr', { key: i }, years ) );
				years = [];
			}

			year++;
			i++;
		}

		return rows;
	},

	updateSelectedYear: function( event ) {
		this.props.updateSelectedDate( event );
	},

	renderYear: function( props, year ) {
		return React.createElement('td',  props, year );
	},

	alwaysValidDate: function() {
		return 1;
	},

	handleClickOutside: function() {
		this.props.handleClickOutside();
	}
}));

module.exports = DateTimePickerYears;


/***/ }),

/***/ "../../node_modules/react-onclickoutside/dist/react-onclickoutside.es.js":
/*!***************************************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-onclickoutside/dist/react-onclickoutside.es.js ***!
  \***************************************************************************************************************************************************/
/*! exports provided: IGNORE_CLASS_NAME, default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, "IGNORE_CLASS_NAME", function() { return IGNORE_CLASS_NAME; });
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! react */ "react");
/* harmony import */ var react__WEBPACK_IMPORTED_MODULE_0___default = /*#__PURE__*/__webpack_require__.n(react__WEBPACK_IMPORTED_MODULE_0__);
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! react-dom */ "react-dom");
/* harmony import */ var react_dom__WEBPACK_IMPORTED_MODULE_1___default = /*#__PURE__*/__webpack_require__.n(react_dom__WEBPACK_IMPORTED_MODULE_1__);



function _inheritsLoose(subClass, superClass) {
  subClass.prototype = Object.create(superClass.prototype);
  subClass.prototype.constructor = subClass;
  subClass.__proto__ = superClass;
}

function _objectWithoutProperties(source, excluded) {
  if (source == null) return {};
  var target = {};
  var sourceKeys = Object.keys(source);
  var key, i;

  for (i = 0; i < sourceKeys.length; i++) {
    key = sourceKeys[i];
    if (excluded.indexOf(key) >= 0) continue;
    target[key] = source[key];
  }

  if (Object.getOwnPropertySymbols) {
    var sourceSymbolKeys = Object.getOwnPropertySymbols(source);

    for (i = 0; i < sourceSymbolKeys.length; i++) {
      key = sourceSymbolKeys[i];
      if (excluded.indexOf(key) >= 0) continue;
      if (!Object.prototype.propertyIsEnumerable.call(source, key)) continue;
      target[key] = source[key];
    }
  }

  return target;
}

/**
 * Check whether some DOM node is our Component's node.
 */
function isNodeFound(current, componentNode, ignoreClass) {
  if (current === componentNode) {
    return true;
  } // SVG <use/> elements do not technically reside in the rendered DOM, so
  // they do not have classList directly, but they offer a link to their
  // corresponding element, which can have classList. This extra check is for
  // that case.
  // See: http://www.w3.org/TR/SVG11/struct.html#InterfaceSVGUseElement
  // Discussion: https://github.com/Pomax/react-onclickoutside/pull/17


  if (current.correspondingElement) {
    return current.correspondingElement.classList.contains(ignoreClass);
  }

  return current.classList.contains(ignoreClass);
}
/**
 * Try to find our node in a hierarchy of nodes, returning the document
 * node as highest node if our node is not found in the path up.
 */

function findHighest(current, componentNode, ignoreClass) {
  if (current === componentNode) {
    return true;
  } // If source=local then this event came from 'somewhere'
  // inside and should be ignored. We could handle this with
  // a layered approach, too, but that requires going back to
  // thinking in terms of Dom node nesting, running counter
  // to React's 'you shouldn't care about the DOM' philosophy.


  while (current.parentNode) {
    if (isNodeFound(current, componentNode, ignoreClass)) {
      return true;
    }

    current = current.parentNode;
  }

  return current;
}
/**
 * Check if the browser scrollbar was clicked
 */

function clickedScrollbar(evt) {
  return document.documentElement.clientWidth <= evt.clientX || document.documentElement.clientHeight <= evt.clientY;
}

// ideally will get replaced with external dep
// when rafrex/detect-passive-events#4 and rafrex/detect-passive-events#5 get merged in
var testPassiveEventSupport = function testPassiveEventSupport() {
  if (typeof window === 'undefined' || typeof window.addEventListener !== 'function') {
    return;
  }

  var passive = false;
  var options = Object.defineProperty({}, 'passive', {
    get: function get() {
      passive = true;
    }
  });

  var noop = function noop() {};

  window.addEventListener('testPassiveEventSupport', noop, options);
  window.removeEventListener('testPassiveEventSupport', noop, options);
  return passive;
};

function autoInc(seed) {
  if (seed === void 0) {
    seed = 0;
  }

  return function () {
    return ++seed;
  };
}

var uid = autoInc();

var passiveEventSupport;
var handlersMap = {};
var enabledInstances = {};
var touchEvents = ['touchstart', 'touchmove'];
var IGNORE_CLASS_NAME = 'ignore-react-onclickoutside';
/**
 * Options for addEventHandler and removeEventHandler
 */

function getEventHandlerOptions(instance, eventName) {
  var handlerOptions = null;
  var isTouchEvent = touchEvents.indexOf(eventName) !== -1;

  if (isTouchEvent && passiveEventSupport) {
    handlerOptions = {
      passive: !instance.props.preventDefault
    };
  }

  return handlerOptions;
}
/**
 * This function generates the HOC function that you'll use
 * in order to impart onOutsideClick listening to an
 * arbitrary component. It gets called at the end of the
 * bootstrapping code to yield an instance of the
 * onClickOutsideHOC function defined inside setupHOC().
 */


function onClickOutsideHOC(WrappedComponent, config) {
  var _class, _temp;

  var componentName = WrappedComponent.displayName || WrappedComponent.name || 'Component';
  return _temp = _class =
  /*#__PURE__*/
  function (_Component) {
    _inheritsLoose(onClickOutside, _Component);

    function onClickOutside(props) {
      var _this;

      _this = _Component.call(this, props) || this;

      _this.__outsideClickHandler = function (event) {
        if (typeof _this.__clickOutsideHandlerProp === 'function') {
          _this.__clickOutsideHandlerProp(event);

          return;
        }

        var instance = _this.getInstance();

        if (typeof instance.props.handleClickOutside === 'function') {
          instance.props.handleClickOutside(event);
          return;
        }

        if (typeof instance.handleClickOutside === 'function') {
          instance.handleClickOutside(event);
          return;
        }

        throw new Error("WrappedComponent: " + componentName + " lacks a handleClickOutside(event) function for processing outside click events.");
      };

      _this.__getComponentNode = function () {
        var instance = _this.getInstance();

        if (config && typeof config.setClickOutsideRef === 'function') {
          return config.setClickOutsideRef()(instance);
        }

        if (typeof instance.setClickOutsideRef === 'function') {
          return instance.setClickOutsideRef();
        }

        return Object(react_dom__WEBPACK_IMPORTED_MODULE_1__["findDOMNode"])(instance);
      };

      _this.enableOnClickOutside = function () {
        if (typeof document === 'undefined' || enabledInstances[_this._uid]) {
          return;
        }

        if (typeof passiveEventSupport === 'undefined') {
          passiveEventSupport = testPassiveEventSupport();
        }

        enabledInstances[_this._uid] = true;
        var events = _this.props.eventTypes;

        if (!events.forEach) {
          events = [events];
        }

        handlersMap[_this._uid] = function (event) {
          if (_this.componentNode === null) return;

          if (_this.props.preventDefault) {
            event.preventDefault();
          }

          if (_this.props.stopPropagation) {
            event.stopPropagation();
          }

          if (_this.props.excludeScrollbar && clickedScrollbar(event)) return;
          var current = event.target;

          if (findHighest(current, _this.componentNode, _this.props.outsideClickIgnoreClass) !== document) {
            return;
          }

          _this.__outsideClickHandler(event);
        };

        events.forEach(function (eventName) {
          document.addEventListener(eventName, handlersMap[_this._uid], getEventHandlerOptions(_this, eventName));
        });
      };

      _this.disableOnClickOutside = function () {
        delete enabledInstances[_this._uid];
        var fn = handlersMap[_this._uid];

        if (fn && typeof document !== 'undefined') {
          var events = _this.props.eventTypes;

          if (!events.forEach) {
            events = [events];
          }

          events.forEach(function (eventName) {
            return document.removeEventListener(eventName, fn, getEventHandlerOptions(_this, eventName));
          });
          delete handlersMap[_this._uid];
        }
      };

      _this.getRef = function (ref) {
        return _this.instanceRef = ref;
      };

      _this._uid = uid();
      return _this;
    }
    /**
     * Access the WrappedComponent's instance.
     */


    var _proto = onClickOutside.prototype;

    _proto.getInstance = function getInstance() {
      if (!WrappedComponent.prototype.isReactComponent) {
        return this;
      }

      var ref = this.instanceRef;
      return ref.getInstance ? ref.getInstance() : ref;
    };

    /**
     * Add click listeners to the current document,
     * linked to this component's state.
     */
    _proto.componentDidMount = function componentDidMount() {
      // If we are in an environment without a DOM such
      // as shallow rendering or snapshots then we exit
      // early to prevent any unhandled errors being thrown.
      if (typeof document === 'undefined' || !document.createElement) {
        return;
      }

      var instance = this.getInstance();

      if (config && typeof config.handleClickOutside === 'function') {
        this.__clickOutsideHandlerProp = config.handleClickOutside(instance);

        if (typeof this.__clickOutsideHandlerProp !== 'function') {
          throw new Error("WrappedComponent: " + componentName + " lacks a function for processing outside click events specified by the handleClickOutside config option.");
        }
      }

      this.componentNode = this.__getComponentNode(); // return early so we dont initiate onClickOutside

      if (this.props.disableOnClickOutside) return;
      this.enableOnClickOutside();
    };

    _proto.componentDidUpdate = function componentDidUpdate() {
      this.componentNode = this.__getComponentNode();
    };
    /**
     * Remove all document's event listeners for this component
     */


    _proto.componentWillUnmount = function componentWillUnmount() {
      this.disableOnClickOutside();
    };
    /**
     * Can be called to explicitly enable event listening
     * for clicks and touches outside of this element.
     */


    /**
     * Pass-through render
     */
    _proto.render = function render() {
      // eslint-disable-next-line no-unused-vars
      var _props = this.props,
          excludeScrollbar = _props.excludeScrollbar,
          props = _objectWithoutProperties(_props, ["excludeScrollbar"]);

      if (WrappedComponent.prototype.isReactComponent) {
        props.ref = this.getRef;
      } else {
        props.wrappedRef = this.getRef;
      }

      props.disableOnClickOutside = this.disableOnClickOutside;
      props.enableOnClickOutside = this.enableOnClickOutside;
      return Object(react__WEBPACK_IMPORTED_MODULE_0__["createElement"])(WrappedComponent, props);
    };

    return onClickOutside;
  }(react__WEBPACK_IMPORTED_MODULE_0__["Component"]), _class.displayName = "OnClickOutside(" + componentName + ")", _class.defaultProps = {
    eventTypes: ['mousedown', 'touchstart'],
    excludeScrollbar: config && config.excludeScrollbar || false,
    outsideClickIgnoreClass: IGNORE_CLASS_NAME,
    preventDefault: false,
    stopPropagation: false
  }, _class.getClass = function () {
    return WrappedComponent.getClass ? WrappedComponent.getClass() : WrappedComponent;
  }, _temp;
}


/* harmony default export */ __webpack_exports__["default"] = (onClickOutsideHOC);


/***/ }),

/***/ "../../node_modules/resolve-pathname/index.js":
/*!************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/resolve-pathname/index.js ***!
  \************************************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
function isAbsolute(pathname) {
  return pathname.charAt(0) === '/';
}

// About 1.5x faster than the two-arg version of Array#splice()
function spliceOne(list, index) {
  for (var i = index, k = i + 1, n = list.length; k < n; i += 1, k += 1) {
    list[i] = list[k];
  }

  list.pop();
}

// This implementation is based heavily on node's url.parse
function resolvePathname(to) {
  var from = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : '';

  var toParts = to && to.split('/') || [];
  var fromParts = from && from.split('/') || [];

  var isToAbs = to && isAbsolute(to);
  var isFromAbs = from && isAbsolute(from);
  var mustEndAbs = isToAbs || isFromAbs;

  if (to && isAbsolute(to)) {
    // to is absolute
    fromParts = toParts;
  } else if (toParts.length) {
    // to is relative, drop the filename
    fromParts.pop();
    fromParts = fromParts.concat(toParts);
  }

  if (!fromParts.length) return '/';

  var hasTrailingSlash = void 0;
  if (fromParts.length) {
    var last = fromParts[fromParts.length - 1];
    hasTrailingSlash = last === '.' || last === '..' || last === '';
  } else {
    hasTrailingSlash = false;
  }

  var up = 0;
  for (var i = fromParts.length; i >= 0; i--) {
    var part = fromParts[i];

    if (part === '.') {
      spliceOne(fromParts, i);
    } else if (part === '..') {
      spliceOne(fromParts, i);
      up++;
    } else if (up) {
      spliceOne(fromParts, i);
      up--;
    }
  }

  if (!mustEndAbs) for (; up--; up) {
    fromParts.unshift('..');
  }if (mustEndAbs && fromParts[0] !== '' && (!fromParts[0] || !isAbsolute(fromParts[0]))) fromParts.unshift('');

  var result = fromParts.join('/');

  if (hasTrailingSlash && result.substr(-1) !== '/') result += '/';

  return result;
}

/* harmony default export */ __webpack_exports__["default"] = (resolvePathname);

/***/ }),

/***/ "../../node_modules/strict-uri-encode/index.js":
/*!*************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/strict-uri-encode/index.js ***!
  \*************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

module.exports = function (str) {
	return encodeURIComponent(str).replace(/[!'()*]/g, function (c) {
		return '%' + c.charCodeAt(0).toString(16).toUpperCase();
	});
};


/***/ }),

/***/ "../../node_modules/style-loader/lib/addStyles.js":
/*!****************************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/style-loader/lib/addStyles.js ***!
  \****************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

/*
	MIT License http://www.opensource.org/licenses/mit-license.php
	Author Tobias Koppers @sokra
*/

var stylesInDom = {};

var	memoize = function (fn) {
	var memo;

	return function () {
		if (typeof memo === "undefined") memo = fn.apply(this, arguments);
		return memo;
	};
};

var isOldIE = memoize(function () {
	// Test for IE <= 9 as proposed by Browserhacks
	// @see http://browserhacks.com/#hack-e71d8692f65334173fee715c222cb805
	// Tests for existence of standard globals is to allow style-loader
	// to operate correctly into non-standard environments
	// @see https://github.com/webpack-contrib/style-loader/issues/177
	return window && document && document.all && !window.atob;
});

var getTarget = function (target) {
  return document.querySelector(target);
};

var getElement = (function (fn) {
	var memo = {};

	return function(target) {
                // If passing function in options, then use it for resolve "head" element.
                // Useful for Shadow Root style i.e
                // {
                //   insertInto: function () { return document.querySelector("#foo").shadowRoot }
                // }
                if (typeof target === 'function') {
                        return target();
                }
                if (typeof memo[target] === "undefined") {
			var styleTarget = getTarget.call(this, target);
			// Special case to return head of iframe instead of iframe itself
			if (window.HTMLIFrameElement && styleTarget instanceof window.HTMLIFrameElement) {
				try {
					// This will throw an exception if access to iframe is blocked
					// due to cross-origin restrictions
					styleTarget = styleTarget.contentDocument.head;
				} catch(e) {
					styleTarget = null;
				}
			}
			memo[target] = styleTarget;
		}
		return memo[target]
	};
})();

var singleton = null;
var	singletonCounter = 0;
var	stylesInsertedAtTop = [];

var	fixUrls = __webpack_require__(/*! ./urls */ "../../node_modules/style-loader/lib/urls.js");

module.exports = function(list, options) {
	if (typeof DEBUG !== "undefined" && DEBUG) {
		if (typeof document !== "object") throw new Error("The style-loader cannot be used in a non-browser environment");
	}

	options = options || {};

	options.attrs = typeof options.attrs === "object" ? options.attrs : {};

	// Force single-tag solution on IE6-9, which has a hard limit on the # of <style>
	// tags it will allow on a page
	if (!options.singleton && typeof options.singleton !== "boolean") options.singleton = isOldIE();

	// By default, add <style> tags to the <head> element
        if (!options.insertInto) options.insertInto = "head";

	// By default, add <style> tags to the bottom of the target
	if (!options.insertAt) options.insertAt = "bottom";

	var styles = listToStyles(list, options);

	addStylesToDom(styles, options);

	return function update (newList) {
		var mayRemove = [];

		for (var i = 0; i < styles.length; i++) {
			var item = styles[i];
			var domStyle = stylesInDom[item.id];

			domStyle.refs--;
			mayRemove.push(domStyle);
		}

		if(newList) {
			var newStyles = listToStyles(newList, options);
			addStylesToDom(newStyles, options);
		}

		for (var i = 0; i < mayRemove.length; i++) {
			var domStyle = mayRemove[i];

			if(domStyle.refs === 0) {
				for (var j = 0; j < domStyle.parts.length; j++) domStyle.parts[j]();

				delete stylesInDom[domStyle.id];
			}
		}
	};
};

function addStylesToDom (styles, options) {
	for (var i = 0; i < styles.length; i++) {
		var item = styles[i];
		var domStyle = stylesInDom[item.id];

		if(domStyle) {
			domStyle.refs++;

			for(var j = 0; j < domStyle.parts.length; j++) {
				domStyle.parts[j](item.parts[j]);
			}

			for(; j < item.parts.length; j++) {
				domStyle.parts.push(addStyle(item.parts[j], options));
			}
		} else {
			var parts = [];

			for(var j = 0; j < item.parts.length; j++) {
				parts.push(addStyle(item.parts[j], options));
			}

			stylesInDom[item.id] = {id: item.id, refs: 1, parts: parts};
		}
	}
}

function listToStyles (list, options) {
	var styles = [];
	var newStyles = {};

	for (var i = 0; i < list.length; i++) {
		var item = list[i];
		var id = options.base ? item[0] + options.base : item[0];
		var css = item[1];
		var media = item[2];
		var sourceMap = item[3];
		var part = {css: css, media: media, sourceMap: sourceMap};

		if(!newStyles[id]) styles.push(newStyles[id] = {id: id, parts: [part]});
		else newStyles[id].parts.push(part);
	}

	return styles;
}

function insertStyleElement (options, style) {
	var target = getElement(options.insertInto)

	if (!target) {
		throw new Error("Couldn't find a style target. This probably means that the value for the 'insertInto' parameter is invalid.");
	}

	var lastStyleElementInsertedAtTop = stylesInsertedAtTop[stylesInsertedAtTop.length - 1];

	if (options.insertAt === "top") {
		if (!lastStyleElementInsertedAtTop) {
			target.insertBefore(style, target.firstChild);
		} else if (lastStyleElementInsertedAtTop.nextSibling) {
			target.insertBefore(style, lastStyleElementInsertedAtTop.nextSibling);
		} else {
			target.appendChild(style);
		}
		stylesInsertedAtTop.push(style);
	} else if (options.insertAt === "bottom") {
		target.appendChild(style);
	} else if (typeof options.insertAt === "object" && options.insertAt.before) {
		var nextSibling = getElement(options.insertInto + " " + options.insertAt.before);
		target.insertBefore(style, nextSibling);
	} else {
		throw new Error("[Style Loader]\n\n Invalid value for parameter 'insertAt' ('options.insertAt') found.\n Must be 'top', 'bottom', or Object.\n (https://github.com/webpack-contrib/style-loader#insertat)\n");
	}
}

function removeStyleElement (style) {
	if (style.parentNode === null) return false;
	style.parentNode.removeChild(style);

	var idx = stylesInsertedAtTop.indexOf(style);
	if(idx >= 0) {
		stylesInsertedAtTop.splice(idx, 1);
	}
}

function createStyleElement (options) {
	var style = document.createElement("style");

	if(options.attrs.type === undefined) {
		options.attrs.type = "text/css";
	}

	addAttrs(style, options.attrs);
	insertStyleElement(options, style);

	return style;
}

function createLinkElement (options) {
	var link = document.createElement("link");

	if(options.attrs.type === undefined) {
		options.attrs.type = "text/css";
	}
	options.attrs.rel = "stylesheet";

	addAttrs(link, options.attrs);
	insertStyleElement(options, link);

	return link;
}

function addAttrs (el, attrs) {
	Object.keys(attrs).forEach(function (key) {
		el.setAttribute(key, attrs[key]);
	});
}

function addStyle (obj, options) {
	var style, update, remove, result;

	// If a transform function was defined, run it on the css
	if (options.transform && obj.css) {
	    result = options.transform(obj.css);

	    if (result) {
	    	// If transform returns a value, use that instead of the original css.
	    	// This allows running runtime transformations on the css.
	    	obj.css = result;
	    } else {
	    	// If the transform function returns a falsy value, don't add this css.
	    	// This allows conditional loading of css
	    	return function() {
	    		// noop
	    	};
	    }
	}

	if (options.singleton) {
		var styleIndex = singletonCounter++;

		style = singleton || (singleton = createStyleElement(options));

		update = applyToSingletonTag.bind(null, style, styleIndex, false);
		remove = applyToSingletonTag.bind(null, style, styleIndex, true);

	} else if (
		obj.sourceMap &&
		typeof URL === "function" &&
		typeof URL.createObjectURL === "function" &&
		typeof URL.revokeObjectURL === "function" &&
		typeof Blob === "function" &&
		typeof btoa === "function"
	) {
		style = createLinkElement(options);
		update = updateLink.bind(null, style, options);
		remove = function () {
			removeStyleElement(style);

			if(style.href) URL.revokeObjectURL(style.href);
		};
	} else {
		style = createStyleElement(options);
		update = applyToTag.bind(null, style);
		remove = function () {
			removeStyleElement(style);
		};
	}

	update(obj);

	return function updateStyle (newObj) {
		if (newObj) {
			if (
				newObj.css === obj.css &&
				newObj.media === obj.media &&
				newObj.sourceMap === obj.sourceMap
			) {
				return;
			}

			update(obj = newObj);
		} else {
			remove();
		}
	};
}

var replaceText = (function () {
	var textStore = [];

	return function (index, replacement) {
		textStore[index] = replacement;

		return textStore.filter(Boolean).join('\n');
	};
})();

function applyToSingletonTag (style, index, remove, obj) {
	var css = remove ? "" : obj.css;

	if (style.styleSheet) {
		style.styleSheet.cssText = replaceText(index, css);
	} else {
		var cssNode = document.createTextNode(css);
		var childNodes = style.childNodes;

		if (childNodes[index]) style.removeChild(childNodes[index]);

		if (childNodes.length) {
			style.insertBefore(cssNode, childNodes[index]);
		} else {
			style.appendChild(cssNode);
		}
	}
}

function applyToTag (style, obj) {
	var css = obj.css;
	var media = obj.media;

	if(media) {
		style.setAttribute("media", media)
	}

	if(style.styleSheet) {
		style.styleSheet.cssText = css;
	} else {
		while(style.firstChild) {
			style.removeChild(style.firstChild);
		}

		style.appendChild(document.createTextNode(css));
	}
}

function updateLink (link, options, obj) {
	var css = obj.css;
	var sourceMap = obj.sourceMap;

	/*
		If convertToAbsoluteUrls isn't defined, but sourcemaps are enabled
		and there is no publicPath defined then lets turn convertToAbsoluteUrls
		on by default.  Otherwise default to the convertToAbsoluteUrls option
		directly
	*/
	var autoFixUrls = options.convertToAbsoluteUrls === undefined && sourceMap;

	if (options.convertToAbsoluteUrls || autoFixUrls) {
		css = fixUrls(css);
	}

	if (sourceMap) {
		// http://stackoverflow.com/a/26603875
		css += "\n/*# sourceMappingURL=data:application/json;base64," + btoa(unescape(encodeURIComponent(JSON.stringify(sourceMap)))) + " */";
	}

	var blob = new Blob([css], { type: "text/css" });

	var oldSrc = link.href;

	link.href = URL.createObjectURL(blob);

	if(oldSrc) URL.revokeObjectURL(oldSrc);
}


/***/ }),

/***/ "../../node_modules/style-loader/lib/urls.js":
/*!***********************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/style-loader/lib/urls.js ***!
  \***********************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports) {


/**
 * When source maps are enabled, `style-loader` uses a link element with a data-uri to
 * embed the css on the page. This breaks all relative urls because now they are relative to a
 * bundle instead of the current page.
 *
 * One solution is to only use full urls, but that may be impossible.
 *
 * Instead, this function "fixes" the relative urls to be absolute according to the current page location.
 *
 * A rudimentary test suite is located at `test/fixUrls.js` and can be run via the `npm test` command.
 *
 */

module.exports = function (css) {
  // get current location
  var location = typeof window !== "undefined" && window.location;

  if (!location) {
    throw new Error("fixUrls requires window.location");
  }

	// blank or null?
	if (!css || typeof css !== "string") {
	  return css;
  }

  var baseUrl = location.protocol + "//" + location.host;
  var currentDir = baseUrl + location.pathname.replace(/\/[^\/]*$/, "/");

	// convert each url(...)
	/*
	This regular expression is just a way to recursively match brackets within
	a string.

	 /url\s*\(  = Match on the word "url" with any whitespace after it and then a parens
	   (  = Start a capturing group
	     (?:  = Start a non-capturing group
	         [^)(]  = Match anything that isn't a parentheses
	         |  = OR
	         \(  = Match a start parentheses
	             (?:  = Start another non-capturing groups
	                 [^)(]+  = Match anything that isn't a parentheses
	                 |  = OR
	                 \(  = Match a start parentheses
	                     [^)(]*  = Match anything that isn't a parentheses
	                 \)  = Match a end parentheses
	             )  = End Group
              *\) = Match anything and then a close parens
          )  = Close non-capturing group
          *  = Match anything
       )  = Close capturing group
	 \)  = Match a close parens

	 /gi  = Get all matches, not the first.  Be case insensitive.
	 */
	var fixedCss = css.replace(/url\s*\(((?:[^)(]|\((?:[^)(]+|\([^)(]*\))*\))*)\)/gi, function(fullMatch, origUrl) {
		// strip quotes (if they exist)
		var unquotedOrigUrl = origUrl
			.trim()
			.replace(/^"(.*)"$/, function(o, $1){ return $1; })
			.replace(/^'(.*)'$/, function(o, $1){ return $1; });

		// already a full url? no change
		if (/^(#|data:|http:\/\/|https:\/\/|file:\/\/\/|\s*$)/i.test(unquotedOrigUrl)) {
		  return fullMatch;
		}

		// convert the url to a full url
		var newUrl;

		if (unquotedOrigUrl.indexOf("//") === 0) {
		  	//TODO: should we add protocol?
			newUrl = unquotedOrigUrl;
		} else if (unquotedOrigUrl.indexOf("/") === 0) {
			// path should be relative to the base url
			newUrl = baseUrl + unquotedOrigUrl; // already starts with '/'
		} else {
			// path should be relative to current directory
			newUrl = currentDir + unquotedOrigUrl.replace(/^\.\//, ""); // Strip leading './'
		}

		// send back the fixed url(...)
		return "url(" + JSON.stringify(newUrl) + ")";
	});

	// send back the fixed css
	return fixedCss;
};


/***/ }),

/***/ "../../node_modules/value-equal/index.js":
/*!*******************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/value-equal/index.js ***!
  \*******************************************************************************************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
__webpack_require__.r(__webpack_exports__);
var _typeof = typeof Symbol === "function" && typeof Symbol.iterator === "symbol" ? function (obj) { return typeof obj; } : function (obj) { return obj && typeof Symbol === "function" && obj.constructor === Symbol && obj !== Symbol.prototype ? "symbol" : typeof obj; };

function valueEqual(a, b) {
  if (a === b) return true;

  if (a == null || b == null) return false;

  if (Array.isArray(a)) {
    return Array.isArray(b) && a.length === b.length && a.every(function (item, index) {
      return valueEqual(item, b[index]);
    });
  }

  var aType = typeof a === 'undefined' ? 'undefined' : _typeof(a);
  var bType = typeof b === 'undefined' ? 'undefined' : _typeof(b);

  if (aType !== bType) return false;

  if (aType === 'object') {
    var aValue = a.valueOf();
    var bValue = b.valueOf();

    if (aValue !== a || bValue !== b) return valueEqual(aValue, bValue);

    var aKeys = Object.keys(a);
    var bKeys = Object.keys(b);

    if (aKeys.length !== bKeys.length) return false;

    return aKeys.every(function (key) {
      return valueEqual(a[key], b[key]);
    });
  }

  return false;
}

/* harmony default export */ __webpack_exports__["default"] = (valueEqual);

/***/ }),

/***/ "../../node_modules/warning/browser.js":
/*!*****************************************************************************************************************!*\
  !*** C:/Users/bernest/Source/Repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/warning/browser.js ***!
  \*****************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";
/**
 * Copyright 2014-2015, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 */



/**
 * Similar to invariant but only logs a warning if the condition is not met.
 * This can be used to log issues in development environments in critical
 * paths. Removing the logging code for production environments will keep the
 * same logic and follow the same code paths.
 */

var warning = function() {};

if (true) {
  warning = function(condition, format, args) {
    var len = arguments.length;
    args = new Array(len > 2 ? len - 2 : 0);
    for (var key = 2; key < len; key++) {
      args[key - 2] = arguments[key];
    }
    if (format === undefined) {
      throw new Error(
        '`warning(condition, format, ...args)` requires a warning ' +
        'message argument'
      );
    }

    if (format.length < 10 || (/^[s\W]*$/).test(format)) {
      throw new Error(
        'The warning format should be able to uniquely identify this ' +
        'warning. Please, use a more descriptive format than: ' + format
      );
    }

    if (!condition) {
      var argIndex = 0;
      var message = 'Warning: ' +
        format.replace(/%s/g, function() {
          return args[argIndex++];
        });
      if (typeof console !== 'undefined') {
        console.error(message);
      }
      try {
        // This error was thrown as a convenience so that you can use this stack
        // to find the callsite that caused this warning to fire.
        throw new Error(message);
      } catch(x) {}
    }
  };
}

module.exports = warning;


/***/ }),

/***/ "./TS/Services/PeriodicDataDisplay.ts":
/*!********************************************!*\
  !*** ./TS/Services/PeriodicDataDisplay.ts ***!
  \********************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var moment = __webpack_require__(/*! moment */ "moment");
var PeriodicDataDisplayService = (function () {
    function PeriodicDataDisplayService() {
    }
    PeriodicDataDisplayService.prototype.getData = function (meterID, startDate, endDate, pixels, measurementCharacteristicID, measurementTypeID, harmonicGroup, type) {
        return $.ajax({
            type: "GET",
            url: "".concat(window.location.origin, "/api/PeriodicDataDisplay/GetData?MeterID=").concat(meterID) +
                "&startDate=".concat(moment(startDate).format('YYYY-MM-DD')) +
                "&endDate=".concat(moment(endDate).format('YYYY-MM-DD')) +
                "&pixels=".concat(pixels) +
                "&MeasurementCharacteristicID=".concat(measurementCharacteristicID) +
                "&MeasurementTypeID=".concat(measurementTypeID) +
                "&HarmonicGroup=".concat(harmonicGroup) +
                "&type=".concat(type),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: true,
            async: true
        });
    };
    PeriodicDataDisplayService.prototype.getMeters = function () {
        return $.ajax({
            type: "GET",
            url: "".concat(window.location.origin, "/api/PeriodicDataDisplay/GetMeters"),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: true,
            async: true
        });
    };
    PeriodicDataDisplayService.prototype.getMeasurementCharacteristics = function (fromStepChangeWebReport, meterID) {
        return $.ajax({
            type: "GET",
            url: "".concat(window.location.origin, "/api/PeriodicDataDisplay/GetMeasurementCharacteristics") +
                "".concat((fromStepChangeWebReport ? "?MeterID=".concat(meterID) : "")),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: true,
            async: true
        });
    };
    return PeriodicDataDisplayService;
}());
exports.default = PeriodicDataDisplayService;


/***/ }),

/***/ "./TS/Services/TrendingDataDisplay.ts":
/*!********************************************!*\
  !*** ./TS/Services/TrendingDataDisplay.ts ***!
  \********************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

Object.defineProperty(exports, "__esModule", { value: true });
var moment = __webpack_require__(/*! moment */ "moment");
var TrendingDataDisplayService = (function () {
    function TrendingDataDisplayService() {
    }
    TrendingDataDisplayService.prototype.getData = function (channelID, startDate, endDate, pixels) {
        return $.ajax({
            type: "GET",
            url: "".concat(window.location.origin, "/api/TrendingDataDisplay/GetData?ChannelID=").concat(channelID) +
                "&startDate=".concat(moment(startDate).format('YYYY-MM-DDTHH:mm')) +
                "&endDate=".concat(moment(endDate).format('YYYY-MM-DDTHH:mm')) +
                "&pixels=".concat(pixels),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: true,
            async: true
        });
    };
    TrendingDataDisplayService.prototype.getPostData = function (measurements, startDate, endDate, pixels) {
        return $.ajax({
            type: "POST",
            url: "".concat(window.location.origin, "/api/TrendingDataDisplay/GetData"),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            data: JSON.stringify({ Channels: measurements.map(function (ms) { return ms.ID; }), StartDate: startDate, EndDate: endDate, Pixels: pixels }),
            cache: true,
            async: true
        });
    };
    TrendingDataDisplayService.prototype.getMeasurements = function (meterID) {
        return $.ajax({
            type: "GET",
            url: "".concat(window.location.origin, "/api/TrendingDataDisplay/GetMeasurements?MeterID=").concat(meterID),
            contentType: "application/json; charset=utf-8",
            dataType: 'json',
            cache: true,
            async: true
        });
    };
    return TrendingDataDisplayService;
}());
exports.default = TrendingDataDisplayService;


/***/ }),

/***/ "./TSX/DateTimeRangePicker.tsx":
/*!*************************************!*\
  !*** ./TSX/DateTimeRangePicker.tsx ***!
  \*************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var React = __webpack_require__(/*! react */ "react");
var DateTime = __webpack_require__(/*! react-datetime */ "../../node_modules/react-datetime/DateTime.js");
var moment = __webpack_require__(/*! moment */ "moment");
__webpack_require__(/*! react-datetime/css/react-datetime.css */ "../../node_modules/react-datetime/css/react-datetime.css");
var DateTimeRangePicker = (function (_super) {
    __extends(DateTimeRangePicker, _super);
    function DateTimeRangePicker(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            startDate: moment(_this.props.startDate),
            endDate: moment(_this.props.endDate)
        };
        return _this;
    }
    DateTimeRangePicker.prototype.render = function () {
        var _this = this;
        return (React.createElement("div", { className: "container", style: { width: 'inherit' } },
            React.createElement("div", { className: "row" },
                React.createElement("div", { className: "form-group" },
                    React.createElement(DateTime, { isValidDate: function (date) { return date.isBefore(_this.state.endDate); }, value: this.state.startDate, timeFormat: "HH:mm", onChange: function (value) { return _this.setState({ startDate: value }, function () { return _this.stateSetter(); }); } }))),
            React.createElement("div", { className: "row" },
                React.createElement("div", { className: "form-group" },
                    React.createElement(DateTime, { isValidDate: function (date) { return date.isAfter(_this.state.startDate); }, value: this.state.endDate, timeFormat: "HH:mm", onChange: function (value) { return _this.setState({ endDate: value }, function () { return _this.stateSetter(); }); } })))));
    };
    DateTimeRangePicker.prototype.componentWillReceiveProps = function (nextProps, nextContent) {
        if (nextProps.startDate != this.state.startDate.format('YYYY-MM-DDTHH:mm') || nextProps.endDate != this.state.endDate.format('YYYY-MM-DDTHH:mm'))
            this.setState({ startDate: moment(this.props.startDate), endDate: moment(this.props.endDate) });
    };
    DateTimeRangePicker.prototype.stateSetter = function () {
        var _this = this;
        clearTimeout(this.stateSetterId);
        this.stateSetterId = setTimeout(function () {
            _this.props.stateSetter({ startDate: _this.state.startDate.format('YYYY-MM-DDTHH:mm'), endDate: _this.state.endDate.format('YYYY-MM-DDTHH:mm') });
        }, 500);
    };
    return DateTimeRangePicker;
}(React.Component));
exports.default = DateTimeRangePicker;


/***/ }),

/***/ "./TSX/MeasurementInput.tsx":
/*!**********************************!*\
  !*** ./TSX/MeasurementInput.tsx ***!
  \**********************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var React = __webpack_require__(/*! react */ "react");
var TrendingDataDisplay_1 = __webpack_require__(/*! ./../TS/Services/TrendingDataDisplay */ "./TS/Services/TrendingDataDisplay.ts");
var MeasurementInput = (function (_super) {
    __extends(MeasurementInput, _super);
    function MeasurementInput(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            options: []
        };
        _this.trendingDataDisplayService = new TrendingDataDisplay_1.default();
        return _this;
    }
    MeasurementInput.prototype.componentWillReceiveProps = function (nextProps) {
        if (this.props.meterID != nextProps.meterID)
            this.getData(nextProps.meterID);
    };
    MeasurementInput.prototype.componentDidMount = function () {
        this.getData(this.props.meterID);
    };
    MeasurementInput.prototype.getData = function (meterID) {
        var _this = this;
        this.trendingDataDisplayService.getMeasurements(meterID).done(function (data) {
            if (data.length == 0)
                return;
            var value = (_this.props.value ? _this.props.value : data[0].ID);
            var options = data.map(function (d) { return React.createElement("option", { key: d.ID, value: d.ID }, d.Name); });
            _this.setState({ options: options });
        });
    };
    MeasurementInput.prototype.render = function () {
        var _this = this;
        return (React.createElement("select", { className: 'form-control', onChange: function (e) { _this.props.onChange({ measurementID: parseInt(e.target.value), measurementName: e.target.selectedOptions[0].text }); }, value: this.props.value },
            React.createElement("option", { value: '0' }),
            this.state.options));
    };
    return MeasurementInput;
}(React.Component));
exports.default = MeasurementInput;


/***/ }),

/***/ "./TSX/MeterInput.tsx":
/*!****************************!*\
  !*** ./TSX/MeterInput.tsx ***!
  \****************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var React = __webpack_require__(/*! react */ "react");
var PeriodicDataDisplay_1 = __webpack_require__(/*! ./../TS/Services/PeriodicDataDisplay */ "./TS/Services/PeriodicDataDisplay.ts");
var MeterInput = (function (_super) {
    __extends(MeterInput, _super);
    function MeterInput(props) {
        var _this = _super.call(this, props) || this;
        _this.state = {
            options: []
        };
        _this.periodicDataDisplayService = new PeriodicDataDisplay_1.default();
        return _this;
    }
    MeterInput.prototype.componentDidMount = function () {
        var _this = this;
        this.periodicDataDisplayService.getMeters().done(function (data) {
            _this.setState({ options: data.map(function (d) { return React.createElement("option", { key: d.ID, value: d.ID }, d.Name); }) });
        });
    };
    MeterInput.prototype.render = function () {
        var _this = this;
        return (React.createElement("select", { className: 'form-control', onChange: function (e) { _this.props.onChange({ meterID: parseInt(e.target.value), meterName: e.target.selectedOptions[0].text, measurementID: null }); }, value: this.props.value },
            React.createElement("option", { value: '0' }),
            this.state.options));
    };
    return MeterInput;
}(React.Component));
exports.default = MeterInput;


/***/ }),

/***/ "./TSX/TrendingChart.tsx":
/*!*******************************!*\
  !*** ./TSX/TrendingChart.tsx ***!
  \*******************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var React = __webpack_require__(/*! react */ "react");
var moment = __webpack_require__(/*! moment */ "moment");
__webpack_require__(/*! ./../flot/jquery.flot.min.js */ "./flot/jquery.flot.min.js");
__webpack_require__(/*! ./../flot/jquery.flot.crosshair.min.js */ "./flot/jquery.flot.crosshair.min.js");
__webpack_require__(/*! ./../flot/jquery.flot.navigate.min.js */ "./flot/jquery.flot.navigate.min.js");
__webpack_require__(/*! ./../flot/jquery.flot.selection.min.js */ "./flot/jquery.flot.selection.min.js");
__webpack_require__(/*! ./../flot/jquery.flot.time.min.js */ "./flot/jquery.flot.time.min.js");
__webpack_require__(/*! ./../flot/jquery.flot.axislabels.js */ "./flot/jquery.flot.axislabels.js");
var TrendingChart = (function (_super) {
    __extends(TrendingChart, _super);
    function TrendingChart(props) {
        var _this = _super.call(this, props) || this;
        _this.hover = 0;
        _this.startDate = props.startDate;
        _this.endDate = props.endDate;
        var ctrl = _this;
        _this.options = {
            canvas: true,
            legend: { show: true },
            crosshair: { mode: "x" },
            selection: { mode: "x" },
            grid: {
                autoHighlight: false,
                clickable: true,
                hoverable: true,
                markings: []
            },
            xaxis: {
                mode: "time",
                tickLength: 10,
                reserveSpace: false,
                ticks: function (axis) {
                    var ticks = [], start = ctrl.floorInBase(axis.min, axis.delta), i = 0, v = Number.NaN, prev;
                    do {
                        prev = v;
                        v = start + i * axis.delta;
                        ticks.push(v);
                        ++i;
                    } while (v < axis.max && v != prev);
                    return ticks;
                },
                tickFormatter: function (value, axis) {
                    if (axis.delta > 3 * 24 * 60 * 60 * 1000)
                        return moment(value).utc().format("MM/DD");
                    else
                        return moment(value).utc().format("MM/DD HH:mm");
                },
                max: null,
                min: null
            },
            yaxis: {
                labelWidth: 50,
                panRange: false,
                tickLength: 10,
                tickFormatter: function (val, axis) {
                    if (axis.delta > 1000000 && (val > 1000000 || val < -1000000))
                        return ((val / 1000000) | 0) + "M";
                    else if (axis.delta > 1000 && (val > 1000 || val < -1000))
                        return ((val / 1000) | 0) + "K";
                    else
                        return val.toFixed(axis.tickDecimals);
                }
            },
            yaxes: []
        };
        return _this;
    }
    TrendingChart.prototype.createDataRows = function (props) {
        if (this.plot != undefined)
            $(this.refs.graph).children().remove();
        var startString = this.startDate;
        var endString = this.endDate;
        var newVessel = [];
        if (props.data != null) {
            $.each(props.data, function (i, measurement) {
                var _a, _b, _c;
                if (measurement.Maximum && ((_a = measurement.Data) === null || _a === void 0 ? void 0 : _a.length) > 0)
                    newVessel.push({ label: "".concat(measurement.MeasurementName, "-Max"), data: measurement.Data.map(function (d) { return [d.Time, d.Maximum]; }), color: measurement.MaxColor, yaxis: measurement.Axis });
                if (measurement.Average && ((_b = measurement.Data) === null || _b === void 0 ? void 0 : _b.length) > 0)
                    newVessel.push({ label: "".concat(measurement.MeasurementName, "-Avg"), data: measurement.Data.map(function (d) { return [d.Time, d.Average]; }), color: measurement.AvgColor, yaxis: measurement.Axis });
                if (measurement.Minimum && ((_c = measurement.Data) === null || _c === void 0 ? void 0 : _c.length) > 0)
                    newVessel.push({ label: "".concat(measurement.MeasurementName, "-Min"), data: measurement.Data.map(function (d) { return [d.Time, d.Minimum]; }), color: measurement.MinColor, yaxis: measurement.Axis });
            });
        }
        newVessel.push([[this.getMillisecondTime(startString), null], [this.getMillisecondTime(endString), null]]);
        this.options.xaxis.max = this.getMillisecondTime(endString);
        this.options.xaxis.min = this.getMillisecondTime(startString);
        this.options.yaxes = this.props.axes;
        this.plot = $.plot($(this.refs.graph), newVessel, this.options);
        this.plotSelected();
        this.plotZoom();
        this.plotHover();
    };
    TrendingChart.prototype.componentDidMount = function () {
        this.createDataRows(this.props);
    };
    TrendingChart.prototype.componentWillReceiveProps = function (nextProps) {
        this.startDate = nextProps.startDate;
        this.endDate = nextProps.endDate;
        this.createDataRows(nextProps);
    };
    TrendingChart.prototype.render = function () {
        return React.createElement("div", { ref: 'graph', style: { height: 'inherit', width: 'inherit' } });
    };
    TrendingChart.prototype.floorInBase = function (n, base) {
        return base * Math.floor(n / base);
    };
    TrendingChart.prototype.getMillisecondTime = function (date) {
        var milliseconds = moment.utc(date).valueOf();
        return milliseconds;
    };
    TrendingChart.prototype.getDateString = function (float) {
        var date = moment.utc(float).format('YYYY-MM-DDTHH:mm');
        return date;
    };
    TrendingChart.prototype.plotSelected = function () {
        var ctrl = this;
        $(this.refs.graph).off("plotselected");
        $(this.refs.graph).bind("plotselected", function (event, ranges) {
            ctrl.props.stateSetter({ startDate: ctrl.getDateString(ranges.xaxis.from), endDate: ctrl.getDateString(ranges.xaxis.to) });
        });
    };
    TrendingChart.prototype.plotZoom = function () {
        var ctrl = this;
        $(this.refs.graph).off("plotzoom");
        $(this.refs.graph).bind("plotzoom", function (event) {
            var minDelta = null;
            var maxDelta = 5;
            var xaxis = ctrl.plot.getAxes().xaxis;
            var xcenter = ctrl.hover;
            var xmin = xaxis.options.min;
            var xmax = xaxis.options.max;
            var datamin = xaxis.datamin;
            var datamax = xaxis.datamax;
            var deltaMagnitude;
            var delta;
            var factor;
            if (xmin == null)
                xmin = datamin;
            if (xmax == null)
                xmax = datamax;
            if (xmin == null || xmax == null)
                return;
            xcenter = Math.max(xcenter, xmin);
            xcenter = Math.min(xcenter, xmax);
            if (event.originalEvent.wheelDelta != undefined)
                delta = event.originalEvent.wheelDelta;
            else
                delta = -event.originalEvent.detail;
            deltaMagnitude = Math.abs(delta);
            if (minDelta == null || deltaMagnitude < minDelta)
                minDelta = deltaMagnitude;
            deltaMagnitude /= minDelta;
            deltaMagnitude = Math.min(deltaMagnitude, maxDelta);
            factor = deltaMagnitude / 10;
            if (delta > 0) {
                xmin = xmin * (1 - factor) + xcenter * factor;
                xmax = xmax * (1 - factor) + xcenter * factor;
            }
            else {
                xmin = (xmin - xcenter * factor) / (1 - factor);
                xmax = (xmax - xcenter * factor) / (1 - factor);
            }
            if (xmin == xaxis.options.xmin && xmax == xaxis.options.xmax)
                return;
            ctrl.startDate = ctrl.getDateString(xmin);
            ctrl.endDate = ctrl.getDateString(xmax);
            ctrl.createDataRows(ctrl.props);
            clearTimeout(ctrl.zoomId);
            ctrl.zoomId = setTimeout(function () {
                ctrl.props.stateSetter({ startDate: ctrl.getDateString(xmin), endDate: ctrl.getDateString(xmax) });
            }, 250);
        });
    };
    TrendingChart.prototype.plotHover = function () {
        var ctrl = this;
        $(this.refs.graph).off("plothover");
        $(this.refs.graph).bind("plothover", function (event, pos, item) {
            ctrl.hover = pos.x;
        });
    };
    return TrendingChart;
}(React.Component));
exports.default = TrendingChart;


/***/ }),

/***/ "./TSX/TrendingDataDisplay.tsx":
/*!*************************************!*\
  !*** ./TSX/TrendingDataDisplay.tsx ***!
  \*************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
var __spreadArray = (this && this.__spreadArray) || function (to, from, pack) {
    if (pack || arguments.length === 2) for (var i = 0, l = from.length, ar; i < l; i++) {
        if (ar || !(i in from)) {
            if (!ar) ar = Array.prototype.slice.call(from, 0, i);
            ar[i] = from[i];
        }
    }
    return to.concat(ar || Array.prototype.slice.call(from));
};
var _this = this;
Object.defineProperty(exports, "__esModule", { value: true });
var React = __webpack_require__(/*! react */ "react");
var ReactDOM = __webpack_require__(/*! react-dom */ "react-dom");
var TrendingDataDisplay_1 = __webpack_require__(/*! ./../TS/Services/TrendingDataDisplay */ "./TS/Services/TrendingDataDisplay.ts");
var createBrowserHistory_1 = __webpack_require__(/*! history/createBrowserHistory */ "../../node_modules/history/createBrowserHistory.js");
var queryString = __webpack_require__(/*! query-string */ "../../node_modules/query-string/index.js");
var moment = __webpack_require__(/*! moment */ "moment");
var MeterInput_1 = __webpack_require__(/*! ./MeterInput */ "./TSX/MeterInput.tsx");
var MeasurementInput_1 = __webpack_require__(/*! ./MeasurementInput */ "./TSX/MeasurementInput.tsx");
var TrendingChart_1 = __webpack_require__(/*! ./TrendingChart */ "./TSX/TrendingChart.tsx");
var DateTimeRangePicker_1 = __webpack_require__(/*! ./DateTimeRangePicker */ "./TSX/DateTimeRangePicker.tsx");
var helper_functions_1 = __webpack_require__(/*! @gpa-gemstone/helper-functions */ "../../node_modules/@gpa-gemstone/helper-functions/lib/index.js");
var TrendingDataDisplay = function () {
    var trendingDataDisplayService = new TrendingDataDisplay_1.default();
    var resizeId = React.useRef(null);
    var loader = React.useRef(null);
    var history = (0, createBrowserHistory_1.default)();
    var query = queryString.parse(history['location'].search);
    var _a = React.useState(sessionStorage.getItem('TrendingDataDisplay-measurements') == null ? [] : JSON.parse(sessionStorage.getItem('TrendingDataDisplay-measurements'))), measurements = _a[0], setMeasurements = _a[1];
    var _b = React.useState(window.innerWidth - 475), width = _b[0], setWidth = _b[1];
    var _c = React.useState(query['startDate'] != undefined ? query['startDate'] : moment().subtract(7, 'day').format('YYYY-MM-DDTHH:mm')), startDate = _c[0], setStartDate = _c[1];
    var _d = React.useState(query['endDate'] != undefined ? query['endDate'] : moment().format('YYYY-MM-DDTHH:mm')), endDate = _d[0], setEndDate = _d[1];
    var _e = React.useState(sessionStorage.getItem('TrendingDataDisplay-axes') == null ? [{ axisLabel: 'Default', color: 'black', position: 'left' }] : JSON.parse(sessionStorage.getItem('TrendingDataDisplay-axes'))), axes = _e[0], setAxes = _e[1];
    React.useEffect(function () {
        window.addEventListener("resize", handleScreenSizeChange.bind(_this));
        history['listen'](function (location, action) {
            var query = queryString.parse(history['location'].search);
            setStartDate(query['startDate'] != undefined ? query['startDate'] : moment().subtract(7, 'day').format('YYYY-MM-DDTHH:mm'));
            setEndDate(query['endDate'] != undefined ? query['endDate'] : moment().format('YYYY-MM-DDTHH:mm'));
        });
        return function () { return $(window).off('resize'); };
    }, []);
    React.useEffect(function () {
        if (measurements.length == 0)
            return;
        getData();
    }, [measurements.length, startDate, endDate]);
    React.useEffect(function () {
        history['push']('TrendingDataDisplay.cshtml?' + queryString.stringify({ startDate: startDate, endDate: endDate }, { encode: false }));
    }, [startDate, endDate]);
    React.useEffect(function () {
        sessionStorage.setItem('TrendingDataDisplay-measurements', JSON.stringify(measurements.map(function (ms) { return ({ ID: ms.ID, MeterID: ms.MeterID, MeterName: ms.MeterName, MeasurementName: ms.MeasurementName, Average: ms.Average, AvgColor: ms.AvgColor, Maximum: ms.Maximum, MaxColor: ms.MaxColor, Minimum: ms.Minimum, MinColor: ms.MinColor, Axis: ms.Axis }); })));
    }, [measurements]);
    React.useEffect(function () {
        sessionStorage.setItem('TrendingDataDisplay-axes', JSON.stringify(axes));
    }, [axes]);
    function getData() {
        $(loader.current).show();
        trendingDataDisplayService.getPostData(measurements, startDate, endDate, width).done(function (data) {
            var meas = __spreadArray([], measurements, true);
            var _loop_1 = function (key) {
                var i = meas.findIndex(function (x) { return x.ID.toString() === key; });
                meas[i].Data = data[key];
            };
            for (var _i = 0, _a = Object.keys(data); _i < _a.length; _i++) {
                var key = _a[_i];
                _loop_1(key);
            }
            setMeasurements(meas);
            $(loader.current).hide();
        });
    }
    function handleScreenSizeChange() {
        clearTimeout(this.resizeId);
        this.resizeId = setTimeout(function () {
        }, 500);
    }
    var height = window.innerHeight - $('#navbar').height();
    var menuWidth = 250;
    var sideWidth = 400;
    var top = $('#navbar').height() - 30;
    return (React.createElement("div", null,
        React.createElement("div", { className: "screen", style: { height: height, width: window.innerWidth, position: 'relative', top: top } },
            React.createElement("div", { className: "vertical-menu", style: { maxHeight: height, overflowY: 'auto' } },
                React.createElement("div", { className: "form-group" },
                    React.createElement("label", null, "Time Range: "),
                    React.createElement(DateTimeRangePicker_1.default, { startDate: startDate, endDate: endDate, stateSetter: function (obj) {
                            if (startDate != obj.startDate)
                                setStartDate(obj.startDate);
                            if (endDate != obj.endDate)
                                setEndDate(obj.endDate);
                        } })),
                React.createElement("div", { className: "form-group", style: { height: 50 } },
                    React.createElement("div", { style: { float: 'left' }, ref: loader, hidden: true },
                        React.createElement("div", { style: { border: '5px solid #f3f3f3', WebkitAnimation: 'spin 1s linear infinite', animation: 'spin 1s linear infinite', borderTop: '5px solid #555', borderRadius: '50%', width: '25px', height: '25px' } }),
                        React.createElement("span", null, "Loading..."))),
                React.createElement("div", { className: "form-group" },
                    React.createElement("div", { className: "panel-group" },
                        React.createElement("div", { className: "panel panel-default" },
                            React.createElement("div", { className: "panel-heading", style: { position: 'relative', height: 60 } },
                                React.createElement("h4", { className: "panel-title", style: { position: 'absolute', left: 15, width: '60%' } },
                                    React.createElement("a", { "data-toggle": "collapse", href: "#MeasurementCollapse" }, "Measurements:")),
                                React.createElement(AddMeasurement, { Add: function (msnt) { return setMeasurements(measurements.concat(msnt)); } })),
                            React.createElement("div", { id: 'MeasurementCollapse', className: 'panel-collapse' },
                                React.createElement("ul", { className: 'list-group' }, measurements.map(function (ms, i) { return React.createElement(MeasurementRow, { key: i, Measurement: ms, Measurements: measurements, Axes: axes, Index: i, SetMeasurements: setMeasurements }); })))))),
                React.createElement("div", { className: "form-group" },
                    React.createElement("div", { className: "panel-group" },
                        React.createElement("div", { className: "panel panel-default" },
                            React.createElement("div", { className: "panel-heading", style: { position: 'relative', height: 60 } },
                                React.createElement("h4", { className: "panel-title", style: { position: 'absolute', left: 15, width: '60%' } },
                                    React.createElement("a", { "data-toggle": "collapse", href: "#axesCollapse" }, "Axes:")),
                                React.createElement(AddAxis, { Add: function (axis) { return setAxes(axes.concat(axis)); } })),
                            React.createElement("div", { id: 'axesCollapse', className: 'panel-collapse' },
                                React.createElement("ul", { className: 'list-group' }, axes.map(function (axis, i) { return React.createElement(AxisRow, { key: i, Axes: axes, Index: i, SetAxes: setAxes }); }))))))),
            React.createElement("div", { className: "waveform-viewer", style: { width: window.innerWidth - menuWidth, height: height, float: 'left', left: 0 } },
                React.createElement(TrendingChart_1.default, { startDate: startDate, endDate: endDate, data: measurements, axes: axes, stateSetter: function (object) {
                        setStartDate(object.startDate);
                        setEndDate(object.endDate);
                    } })))));
};
var AddMeasurement = function (props) {
    var _a = React.useState({ ID: 0, MeterID: 0, MeterName: '', MeasurementName: '', Maximum: true, MaxColor: (0, helper_functions_1.RandomColor)(), Average: true, AvgColor: (0, helper_functions_1.RandomColor)(), Minimum: true, MinColor: (0, helper_functions_1.RandomColor)(), Axis: 1 }), measurement = _a[0], setMeasurement = _a[1];
    return (React.createElement(React.Fragment, null,
        React.createElement("button", { type: "button", style: { position: 'absolute', right: 15 }, className: "btn btn-info", "data-toggle": "modal", "data-target": "#measurementModal", onClick: function () {
                setMeasurement({ ID: 0, MeterID: 0, MeterName: '', MeasurementName: '', Maximum: true, MaxColor: (0, helper_functions_1.RandomColor)(), Average: true, AvgColor: (0, helper_functions_1.RandomColor)(), Minimum: true, MinColor: (0, helper_functions_1.RandomColor)(), Axis: 1 });
            } }, "Add"),
        React.createElement("div", { id: "measurementModal", className: "modal fade", role: "dialog" },
            React.createElement("div", { className: "modal-dialog" },
                React.createElement("div", { className: "modal-content" },
                    React.createElement("div", { className: "modal-header" },
                        React.createElement("button", { type: "button", className: "close", "data-dismiss": "modal" }, "\u00D7"),
                        React.createElement("h4", { className: "modal-title" }, "Add Measurement")),
                    React.createElement("div", { className: "modal-body" },
                        React.createElement("div", { className: "form-group" },
                            React.createElement("label", null, "Meter: "),
                            React.createElement(MeterInput_1.default, { value: measurement.MeterID, onChange: function (obj) { return setMeasurement(__assign(__assign({}, measurement), { MeterID: obj.meterID, MeterName: obj.meterName })); } })),
                        React.createElement("div", { className: "form-group" },
                            React.createElement("label", null, "Measurement: "),
                            React.createElement(MeasurementInput_1.default, { meterID: measurement.MeterID, value: measurement.ID, onChange: function (obj) { return setMeasurement(__assign(__assign({}, measurement), { ID: obj.measurementID, MeasurementName: obj.measurementName })); } })),
                        React.createElement("div", { className: "row" },
                            React.createElement("div", { className: "col-lg-6" },
                                React.createElement("div", { className: "checkbox" },
                                    React.createElement("label", null,
                                        React.createElement("input", { type: "checkbox", checked: measurement.Maximum, value: measurement.Maximum.toString(), onChange: function () { return setMeasurement(__assign(__assign({}, measurement), { Maximum: !measurement.Maximum })); } }),
                                        " Maximum"))),
                            React.createElement("div", { className: "col-lg-6" },
                                React.createElement("input", { type: "color", style: { textAlign: 'left' }, className: "form-control", value: measurement.MaxColor, onChange: function (evt) { return setMeasurement(__assign(__assign({}, measurement), { MaxColor: evt.target.value })); } }))),
                        React.createElement("div", { className: "row" },
                            React.createElement("div", { className: "col-lg-6" },
                                React.createElement("div", { className: "checkbox" },
                                    React.createElement("label", null,
                                        React.createElement("input", { type: "checkbox", checked: measurement.Average, value: measurement.Average.toString(), onChange: function () { return setMeasurement(__assign(__assign({}, measurement), { Average: !measurement.Average })); } }),
                                        " Average"))),
                            React.createElement("div", { className: "col-lg-6" },
                                React.createElement("input", { type: "color", style: { textAlign: 'left' }, className: "form-control", value: measurement.AvgColor, onChange: function (evt) { return setMeasurement(__assign(__assign({}, measurement), { AvgColor: evt.target.value })); } }))),
                        React.createElement("div", { className: "row" },
                            React.createElement("div", { className: "col-lg-6" },
                                React.createElement("div", { className: "checkbox" },
                                    React.createElement("label", null,
                                        React.createElement("input", { type: "checkbox", checked: measurement.Minimum, value: measurement.Minimum.toString(), onChange: function () { return setMeasurement(__assign(__assign({}, measurement), { Minimum: !measurement.Minimum })); } }),
                                        " Minimum"))),
                            React.createElement("div", { className: "col-lg-6" },
                                React.createElement("input", { type: "color", style: { textAlign: 'left' }, className: "form-control", value: measurement.MinColor, onChange: function (evt) { return setMeasurement(__assign(__assign({}, measurement), { MinColor: evt.target.value })); } })))),
                    React.createElement("div", { className: "modal-footer" },
                        React.createElement("button", { type: "button", className: "btn btn-primary", "data-dismiss": "modal", onClick: function () { return props.Add(measurement); } }, "Add"),
                        React.createElement("button", { type: "button", className: "btn btn-default", "data-dismiss": "modal" }, "Cancel")))))));
};
var MeasurementRow = function (props) {
    return (React.createElement("li", { className: 'list-group-item', key: props.Measurement.ID },
        React.createElement("div", null, props.Measurement.MeterName),
        React.createElement("button", { type: "button", style: { position: 'relative', top: -20 }, className: "close", onClick: function () {
                var meas = __spreadArray([], props.Measurements, true);
                meas.splice(props.Index, 1);
                props.SetMeasurements(meas);
            } }, "\u00D7"),
        React.createElement("div", null, props.Measurement.MeasurementName),
        React.createElement("div", null,
            React.createElement("select", { className: 'form-control', value: props.Measurement.Axis, onChange: function (evt) {
                    var meas = __spreadArray([], props.Measurements, true);
                    meas[props.Index].Axis = parseInt(evt.target.value);
                    props.SetMeasurements(meas);
                } }, props.Axes.map(function (a, ix) { return React.createElement("option", { key: ix, value: ix + 1 }, a.axisLabel); }))),
        React.createElement("div", { className: 'row' },
            React.createElement("div", { className: 'col-lg-4' },
                React.createElement("div", { className: "" },
                    React.createElement("div", { className: "checkbox" },
                        React.createElement("label", null,
                            React.createElement("input", { type: "checkbox", checked: props.Measurement.Maximum, value: props.Measurement.Maximum.toString(), onChange: function () {
                                    var meas = __spreadArray([], props.Measurements, true);
                                    meas[props.Index].Maximum = !meas[props.Index].Maximum;
                                    props.SetMeasurements(meas);
                                } }),
                            " Max"))),
                React.createElement("div", { className: "" },
                    React.createElement("input", { type: "color", className: "form-control", value: props.Measurement.MaxColor, onChange: function (evt) {
                            var meas = __spreadArray([], props.Measurements, true);
                            meas[props.Index].MaxColor = evt.target.value;
                            props.SetMeasurements(meas);
                        } }))),
            React.createElement("div", { className: 'col-lg-4' },
                React.createElement("div", { className: "" },
                    React.createElement("div", { className: "checkbox" },
                        React.createElement("label", null,
                            React.createElement("input", { type: "checkbox", checked: props.Measurement.Average, value: props.Measurement.Average.toString(), onChange: function () {
                                    var meas = __spreadArray([], props.Measurements, true);
                                    meas[props.Index].Average = !meas[props.Index].Average;
                                    props.SetMeasurements(meas);
                                } }),
                            " Avg"))),
                React.createElement("div", { className: "" },
                    React.createElement("input", { type: "color", className: "form-control", value: props.Measurement.AvgColor, onChange: function (evt) {
                            var meas = __spreadArray([], props.Measurements, true);
                            meas[props.Index].AvgColor = evt.target.value;
                            props.SetMeasurements(meas);
                        } }))),
            React.createElement("div", { className: 'col-lg-4' },
                React.createElement("div", { className: "" },
                    React.createElement("div", { className: "checkbox" },
                        React.createElement("label", null,
                            React.createElement("input", { type: "checkbox", checked: props.Measurement.Minimum, value: props.Measurement.Minimum.toString(), onChange: function () {
                                    var meas = __spreadArray([], props.Measurements, true);
                                    meas[props.Index].Minimum = !meas[props.Index].Minimum;
                                    props.SetMeasurements(meas);
                                } }),
                            " Min"))),
                React.createElement("div", { className: "" },
                    React.createElement("input", { type: "color", className: "form-control", value: props.Measurement.MinColor, onChange: function (evt) {
                            var meas = __spreadArray([], props.Measurements, true);
                            meas[props.Index].MinColor = evt.target.value;
                            props.SetMeasurements(meas);
                        } }))))));
};
var AddAxis = function (props) {
    var _a = React.useState({ position: 'left', color: 'black', axisLabel: '', axisLabelUseCanvas: true, show: true }), axis = _a[0], setAxis = _a[1];
    return (React.createElement(React.Fragment, null,
        React.createElement("button", { type: "button", style: { position: 'absolute', right: 15 }, className: "btn btn-info", "data-toggle": "modal", "data-target": "#axisModal", onClick: function () {
                setAxis({ position: 'left', color: 'black', axisLabel: '', axisLabelUseCanvas: true, show: true });
            } }, "Add"),
        React.createElement("div", { id: "axisModal", className: "modal fade", role: "dialog" },
            React.createElement("div", { className: "modal-dialog" },
                React.createElement("div", { className: "modal-content" },
                    React.createElement("div", { className: "modal-header" },
                        React.createElement("button", { type: "button", className: "close", "data-dismiss": "modal" }, "\u00D7"),
                        React.createElement("h4", { className: "modal-title" }, "Add Axis")),
                    React.createElement("div", { className: "modal-body" },
                        React.createElement("div", { className: "form-group" },
                            React.createElement("label", null, "Label: "),
                            React.createElement("input", { type: "text", className: "form-control", value: axis.axisLabel, onChange: function (evt) { return setAxis(__assign(__assign({}, axis), { axisLabel: evt.target.value })); } })),
                        React.createElement("div", { className: "form-group" },
                            React.createElement("label", null, "Position: "),
                            React.createElement("select", { className: 'form-control', value: axis.position, onChange: function (evt) { return setAxis(__assign(__assign({}, axis), { position: evt.target.value })); } },
                                React.createElement("option", { value: 'left' }, "left"),
                                React.createElement("option", { value: 'right' }, "right")))),
                    React.createElement("div", { className: "modal-footer" },
                        React.createElement("button", { type: "button", className: "btn btn-primary", "data-dismiss": "modal", onClick: function () { return props.Add(axis); } }, "Add"),
                        React.createElement("button", { type: "button", className: "btn btn-default", "data-dismiss": "modal" }, "Cancel")))))));
};
var AxisRow = function (props) {
    var _a, _b, _c, _d;
    return (React.createElement("li", { className: 'list-group-item', key: props.Index },
        React.createElement("div", null, props.Axes[props.Index].axisLabel),
        React.createElement("button", { type: "button", style: { position: 'relative', top: -20 }, className: "close", onClick: function () {
                var a = __spreadArray([], props.Axes, true);
                a.splice(props.Index, 1);
                props.SetAxes(a);
            } }, "\u00D7"),
        React.createElement("div", null,
            React.createElement("select", { className: 'form-control', value: props.Axes[props.Index].position, onChange: function (evt) {
                    var a = __spreadArray([], props.Axes, true);
                    a[props.Index].position = evt.target.value;
                    props.SetAxes(a);
                } },
                React.createElement("option", { value: 'left' }, "left"),
                React.createElement("option", { value: 'right' }, "right"))),
        React.createElement("div", { className: 'row' },
            React.createElement("div", { className: 'col-lg-6' },
                React.createElement("div", { className: 'form-group' },
                    React.createElement("label", null, "Min"),
                    React.createElement("input", { className: 'form-control', type: "number", value: (_b = (_a = props.Axes[props.Index]) === null || _a === void 0 ? void 0 : _a.min) !== null && _b !== void 0 ? _b : '', onChange: function (evt) {
                            var axes = __spreadArray([], props.Axes, true);
                            if (evt.target.value == '')
                                delete axes[props.Index].min;
                            else
                                axes[props.Index].min = parseFloat(evt.target.value);
                            props.SetAxes(axes);
                        } }))),
            React.createElement("div", { className: 'col-lg-6' },
                React.createElement("div", { className: 'form-group' },
                    React.createElement("label", null, "Max"),
                    React.createElement("input", { className: 'form-control', type: "number", value: (_d = (_c = props.Axes[props.Index]) === null || _c === void 0 ? void 0 : _c.max) !== null && _d !== void 0 ? _d : '', onChange: function (evt) {
                            var axes = __spreadArray([], props.Axes, true);
                            if (evt.target.value == '')
                                delete axes[props.Index].max;
                            else
                                axes[props.Index].max = parseFloat(evt.target.value);
                            props.SetAxes(axes);
                        } })))),
        React.createElement("button", { className: 'btn btn-info', onClick: function () {
                var axes = __spreadArray([], props.Axes, true);
                delete axes[props.Index].max;
                delete axes[props.Index].min;
                props.SetAxes(axes);
            } }, "Use Default")));
};
ReactDOM.render(React.createElement(TrendingDataDisplay, null), document.getElementById('bodyContainer'));


/***/ }),

/***/ "./flot/jquery.flot.axislabels.js":
/*!****************************************!*\
  !*** ./flot/jquery.flot.axislabels.js ***!
  \****************************************/
/*! no static exports found */
/***/ (function(module, exports) {

/*
Axis Labels Plugin for flot.
http://github.com/markrcote/flot-axislabels
Original code is Copyright (c) 2010 Xuan Luo.
Original code was released under the GPLv3 license by Xuan Luo, September 2010.
Original code was rereleased under the MIT license by Xuan Luo, April 2012.
Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:
The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

(function ($) {
    var options = {
      axisLabels: {
        show: true
      }
    };

    function canvasSupported() {
        return !!document.createElement('canvas').getContext;
    }

    function canvasTextSupported() {
        if (!canvasSupported()) {
            return false;
        }
        var dummy_canvas = document.createElement('canvas');
        var context = dummy_canvas.getContext('2d');
        return typeof context.fillText == 'function';
    }

    function css3TransitionSupported() {
        var div = document.createElement('div');
        return typeof div.style.MozTransition != 'undefined'    // Gecko
            || typeof div.style.OTransition != 'undefined'      // Opera
            || typeof div.style.webkitTransition != 'undefined' // WebKit
            || typeof div.style.transition != 'undefined';
    }


    function AxisLabel(axisName, position, padding, plot, opts) {
        this.axisName = axisName;
        this.position = position;
        this.padding = padding;
        this.plot = plot;
        this.opts = opts;
        this.width = 0;
        this.height = 0;
    }

    AxisLabel.prototype.cleanup = function() {
    };


    CanvasAxisLabel.prototype = new AxisLabel();
    CanvasAxisLabel.prototype.constructor = CanvasAxisLabel;
    function CanvasAxisLabel(axisName, position, padding, plot, opts) {
        AxisLabel.prototype.constructor.call(this, axisName, position, padding,
                                             plot, opts);
    }

    CanvasAxisLabel.prototype.calculateSize = function() {
        if (!this.opts.axisLabelFontSizePixels)
            this.opts.axisLabelFontSizePixels = 14;
        if (!this.opts.axisLabelFontFamily)
            this.opts.axisLabelFontFamily = 'sans-serif';

        var textWidth = this.opts.axisLabelFontSizePixels + this.padding;
        var textHeight = this.opts.axisLabelFontSizePixels + this.padding;
        if (this.position == 'left' || this.position == 'right') {
            this.width = this.opts.axisLabelFontSizePixels + this.padding;
            this.height = 0;
        } else {
            this.width = 0;
            this.height = this.opts.axisLabelFontSizePixels + this.padding;
        }
    };

    CanvasAxisLabel.prototype.draw = function(box) {
        if (!this.opts.axisLabelColour)
            this.opts.axisLabelColour = 'black';
        var ctx = this.plot.getCanvas().getContext('2d');
        ctx.save();
        ctx.font = this.opts.axisLabelFontSizePixels + 'px ' +
            this.opts.axisLabelFontFamily;
        ctx.fillStyle = this.opts.axisLabelColour;
        var width = ctx.measureText(this.opts.axisLabel).width;
        var height = this.opts.axisLabelFontSizePixels;
        var x, y, angle = 0;
        if (this.position == 'top') {
            x = box.left + box.width/2 - width/2;
            y = box.top + height*0.72;
        } else if (this.position == 'bottom') {
            x = box.left + box.width/2 - width/2;
            y = box.top + box.height - height*0.72;
        } else if (this.position == 'left') {
            x = box.left + height*0.72;
            y = box.height/2 + box.top + width/2;
            angle = -Math.PI/2;
        } else if (this.position == 'right') {
            x = box.left + box.width - height*0.72;
            y = box.height/2 + box.top - width/2;
            angle = Math.PI/2;
        }
        ctx.translate(x, y);
        ctx.rotate(angle);
        ctx.fillText(this.opts.axisLabel, 0, 0);
        ctx.restore();
    };


    HtmlAxisLabel.prototype = new AxisLabel();
    HtmlAxisLabel.prototype.constructor = HtmlAxisLabel;
    function HtmlAxisLabel(axisName, position, padding, plot, opts) {
        AxisLabel.prototype.constructor.call(this, axisName, position,
                                             padding, plot, opts);
        this.elem = null;
    }

    HtmlAxisLabel.prototype.calculateSize = function() {
        var elem = $('<div class="axisLabels" style="position:absolute;">' +
                     this.opts.axisLabel + '</div>');
        this.plot.getPlaceholder().append(elem);
        // store height and width of label itself, for use in draw()
        this.labelWidth = elem.outerWidth(true);
        this.labelHeight = elem.outerHeight(true);
        elem.remove();

        this.width = this.height = 0;
        if (this.position == 'left' || this.position == 'right') {
            this.width = this.labelWidth + this.padding;
        } else {
            this.height = this.labelHeight + this.padding;
        }
    };

    HtmlAxisLabel.prototype.cleanup = function() {
        if (this.elem) {
            this.elem.remove();
        }
    };

    HtmlAxisLabel.prototype.draw = function(box) {
        this.plot.getPlaceholder().find('#' + this.axisName + 'Label').remove();
        this.elem = $('<div id="' + this.axisName +
                      'Label" " class="axisLabels" style="position:absolute;">'
                      + this.opts.axisLabel + '</div>');
        this.plot.getPlaceholder().append(this.elem);
        if (this.position == 'top') {
            this.elem.css('left', box.left + box.width/2 - this.labelWidth/2 +
                          'px');
            this.elem.css('top', box.top + 'px');
        } else if (this.position == 'bottom') {
            this.elem.css('left', box.left + box.width/2 - this.labelWidth/2 +
                          'px');
            this.elem.css('top', box.top + box.height - this.labelHeight +
                          'px');
        } else if (this.position == 'left') {
            this.elem.css('top', box.top + box.height/2 - this.labelHeight/2 +
                          'px');
            this.elem.css('left', box.left + 'px');
        } else if (this.position == 'right') {
            this.elem.css('top', box.top + box.height/2 - this.labelHeight/2 +
                          'px');
            this.elem.css('left', box.left + box.width - this.labelWidth +
                          'px');
        }
    };


    CssTransformAxisLabel.prototype = new HtmlAxisLabel();
    CssTransformAxisLabel.prototype.constructor = CssTransformAxisLabel;
    function CssTransformAxisLabel(axisName, position, padding, plot, opts) {
        HtmlAxisLabel.prototype.constructor.call(this, axisName, position,
                                                 padding, plot, opts);
    }

    CssTransformAxisLabel.prototype.calculateSize = function() {
        HtmlAxisLabel.prototype.calculateSize.call(this);
        this.width = this.height = 0;
        if (this.position == 'left' || this.position == 'right') {
            this.width = this.labelHeight + this.padding;
        } else {
            this.height = this.labelHeight + this.padding;
        }
    };

    CssTransformAxisLabel.prototype.transforms = function(degrees, x, y) {
        var stransforms = {
            '-moz-transform': '',
            '-webkit-transform': '',
            '-o-transform': '',
            '-ms-transform': ''
        };
        if (x != 0 || y != 0) {
            var stdTranslate = ' translate(' + x + 'px, ' + y + 'px)';
            stransforms['-moz-transform'] += stdTranslate;
            stransforms['-webkit-transform'] += stdTranslate;
            stransforms['-o-transform'] += stdTranslate;
            stransforms['-ms-transform'] += stdTranslate;
        }
        if (degrees != 0) {
            var rotation = degrees / 90;
            var stdRotate = ' rotate(' + degrees + 'deg)';
            stransforms['-moz-transform'] += stdRotate;
            stransforms['-webkit-transform'] += stdRotate;
            stransforms['-o-transform'] += stdRotate;
            stransforms['-ms-transform'] += stdRotate;
        }
        var s = 'top: 0; left: 0; ';
        for (var prop in stransforms) {
            if (stransforms[prop]) {
                s += prop + ':' + stransforms[prop] + ';';
            }
        }
        s += ';';
        return s;
    };

    CssTransformAxisLabel.prototype.calculateOffsets = function(box) {
        var offsets = { x: 0, y: 0, degrees: 0 };
        if (this.position == 'bottom') {
            offsets.x = box.left + box.width/2 - this.labelWidth/2;
            offsets.y = box.top + box.height - this.labelHeight;
        } else if (this.position == 'top') {
            offsets.x = box.left + box.width/2 - this.labelWidth/2;
            offsets.y = box.top;
        } else if (this.position == 'left') {
            offsets.degrees = -90;
            offsets.x = box.left - this.labelWidth/2 + this.labelHeight/2;
            offsets.y = box.height/2 + box.top;
        } else if (this.position == 'right') {
            offsets.degrees = 90;
            offsets.x = box.left + box.width - this.labelWidth/2
                        - this.labelHeight/2;
            offsets.y = box.height/2 + box.top;
        }
        offsets.x = Math.round(offsets.x);
        offsets.y = Math.round(offsets.y);

        return offsets;
    };

    CssTransformAxisLabel.prototype.draw = function(box) {
        this.plot.getPlaceholder().find("." + this.axisName + "Label").remove();
        var offsets = this.calculateOffsets(box);
        this.elem = $('<div class="axisLabels ' + this.axisName +
                      'Label" style="position:absolute; ' +
                      this.transforms(offsets.degrees, offsets.x, offsets.y) +
                      '">' + this.opts.axisLabel + '</div>');
        this.plot.getPlaceholder().append(this.elem);
    };


    IeTransformAxisLabel.prototype = new CssTransformAxisLabel();
    IeTransformAxisLabel.prototype.constructor = IeTransformAxisLabel;
    function IeTransformAxisLabel(axisName, position, padding, plot, opts) {
        CssTransformAxisLabel.prototype.constructor.call(this, axisName,
                                                         position, padding,
                                                         plot, opts);
        this.requiresResize = false;
    }

    IeTransformAxisLabel.prototype.transforms = function(degrees, x, y) {
        // I didn't feel like learning the crazy Matrix stuff, so this uses
        // a combination of the rotation transform and CSS positioning.
        var s = '';
        if (degrees != 0) {
            var rotation = degrees/90;
            while (rotation < 0) {
                rotation += 4;
            }
            s += ' filter: progid:DXImageTransform.Microsoft.BasicImage(rotation=' + rotation + '); ';
            // see below
            this.requiresResize = (this.position == 'right');
        }
        if (x != 0) {
            s += 'left: ' + x + 'px; ';
        }
        if (y != 0) {
            s += 'top: ' + y + 'px; ';
        }
        return s;
    };

    IeTransformAxisLabel.prototype.calculateOffsets = function(box) {
        var offsets = CssTransformAxisLabel.prototype.calculateOffsets.call(
                          this, box);
        // adjust some values to take into account differences between
        // CSS and IE rotations.
        if (this.position == 'top') {
            // FIXME: not sure why, but placing this exactly at the top causes
            // the top axis label to flip to the bottom...
            offsets.y = box.top + 1;
        } else if (this.position == 'left') {
            offsets.x = box.left;
            offsets.y = box.height/2 + box.top - this.labelWidth/2;
        } else if (this.position == 'right') {
            offsets.x = box.left + box.width - this.labelHeight;
            offsets.y = box.height/2 + box.top - this.labelWidth/2;
        }
        return offsets;
    };

    IeTransformAxisLabel.prototype.draw = function(box) {
        CssTransformAxisLabel.prototype.draw.call(this, box);
        if (this.requiresResize) {
            this.elem = this.plot.getPlaceholder().find("." + this.axisName +
                                                        "Label");
            // Since we used CSS positioning instead of transforms for
            // translating the element, and since the positioning is done
            // before any rotations, we have to reset the width and height
            // in case the browser wrapped the text (specifically for the
            // y2axis).
            this.elem.css('width', this.labelWidth);
            this.elem.css('height', this.labelHeight);
        }
    };


    function init(plot) {
        plot.hooks.processOptions.push(function (plot, options) {

            if (!options.axisLabels.show)
                return;

            // This is kind of a hack. There are no hooks in Flot between
            // the creation and measuring of the ticks (setTicks, measureTickLabels
            // in setupGrid() ) and the drawing of the ticks and plot box
            // (insertAxisLabels in setupGrid() ).
            //
            // Therefore, we use a trick where we run the draw routine twice:
            // the first time to get the tick measurements, so that we can change
            // them, and then have it draw it again.
            var secondPass = false;

            var axisLabels = {};
            var axisOffsetCounts = { left: 0, right: 0, top: 0, bottom: 0 };

            var defaultPadding = 2;  // padding between axis and tick labels
            plot.hooks.draw.push(function (plot, ctx) {
                var hasAxisLabels = false;
                if (!secondPass) {
                    // MEASURE AND SET OPTIONS
                    $.each(plot.getAxes(), function(axisName, axis) {
                        var opts = axis.options // Flot 0.7
                            || plot.getOptions()[axisName]; // Flot 0.6

                        // Handle redraws initiated outside of this plug-in.
                        if (axisName in axisLabels) {
                            axis.labelHeight = axis.labelHeight -
                                axisLabels[axisName].height;
                            axis.labelWidth = axis.labelWidth -
                                axisLabels[axisName].width;
                            opts.labelHeight = axis.labelHeight;
                            opts.labelWidth = axis.labelWidth;
                            axisLabels[axisName].cleanup();
                            delete axisLabels[axisName];
                        }

                        if (!opts || !opts.axisLabel || !axis.show)
                            return;

                        hasAxisLabels = true;
                        var renderer = null;

                        if (!opts.axisLabelUseHtml &&
                            navigator.appName == 'Microsoft Internet Explorer') {
                            var ua = navigator.userAgent;
                            var re  = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
                            if (re.exec(ua) != null) {
                                rv = parseFloat(RegExp.$1);
                            }
                            if (rv >= 9 && !opts.axisLabelUseCanvas && !opts.axisLabelUseHtml) {
                                renderer = CssTransformAxisLabel;
                            } else if (!opts.axisLabelUseCanvas && !opts.axisLabelUseHtml) {
                                renderer = IeTransformAxisLabel;
                            } else if (opts.axisLabelUseCanvas) {
                                renderer = CanvasAxisLabel;
                            } else {
                                renderer = HtmlAxisLabel;
                            }
                        } else {
                            if (opts.axisLabelUseHtml || (!css3TransitionSupported() && !canvasTextSupported()) && !opts.axisLabelUseCanvas) {
                                renderer = HtmlAxisLabel;
                            } else if (opts.axisLabelUseCanvas || !css3TransitionSupported()) {
                                renderer = CanvasAxisLabel;
                            } else {
                                renderer = CssTransformAxisLabel;
                            }
                        }

                        var padding = opts.axisLabelPadding === undefined ?
                                      defaultPadding : opts.axisLabelPadding;

                        axisLabels[axisName] = new renderer(axisName,
                                                            axis.position, padding,
                                                            plot, opts);

                        // flot interprets axis.labelHeight and .labelWidth as
                        // the height and width of the tick labels. We increase
                        // these values to make room for the axis label and
                        // padding.

                        axisLabels[axisName].calculateSize();

                        // AxisLabel.height and .width are the size of the
                        // axis label and padding.
                        // Just set opts here because axis will be sorted out on
                        // the redraw.

                        opts.labelHeight = axis.labelHeight +
                            axisLabels[axisName].height;
                        opts.labelWidth = axis.labelWidth +
                            axisLabels[axisName].width;
                    });

                    // If there are axis labels, re-draw with new label widths and
                    // heights.

                    if (hasAxisLabels) {
                        secondPass = true;
                        plot.setupGrid();
                        plot.draw();
                    }
                } else {
                    secondPass = false;
                    // DRAW
                    $.each(plot.getAxes(), function(axisName, axis) {
                        var opts = axis.options // Flot 0.7
                            || plot.getOptions()[axisName]; // Flot 0.6
                        if (!opts || !opts.axisLabel || !axis.show)
                            return;

                        axisLabels[axisName].draw(axis.box);
                    });
                }
            });
        });
    }


    $.plot.plugins.push({
        init: init,
        options: options,
        name: 'axisLabels',
        version: '2.0'
    });
})(jQuery);

/***/ }),

/***/ "./flot/jquery.flot.crosshair.min.js":
/*!*******************************************!*\
  !*** ./flot/jquery.flot.crosshair.min.js ***!
  \*******************************************/
/*! no static exports found */
/***/ (function(module, exports) {

/* Javascript plotting library for jQuery, version 0.8.3.

Copyright (c) 2007-2014 IOLA and Ole Laursen.
Licensed under the MIT license.

*/
(function($){var options={crosshair:{mode:null,color:"rgba(170, 0, 0, 0.80)",lineWidth:1}};function init(plot){var crosshair={x:-1,y:-1,locked:false};plot.setCrosshair=function setCrosshair(pos){if(!pos)crosshair.x=-1;else{var o=plot.p2c(pos);crosshair.x=Math.max(0,Math.min(o.left,plot.width()));crosshair.y=Math.max(0,Math.min(o.top,plot.height()))}plot.triggerRedrawOverlay()};plot.clearCrosshair=plot.setCrosshair;plot.lockCrosshair=function lockCrosshair(pos){if(pos)plot.setCrosshair(pos);crosshair.locked=true};plot.unlockCrosshair=function unlockCrosshair(){crosshair.locked=false};function onMouseOut(e){if(crosshair.locked)return;if(crosshair.x!=-1){crosshair.x=-1;plot.triggerRedrawOverlay()}}function onMouseMove(e){if(crosshair.locked)return;if(plot.getSelection&&plot.getSelection()){crosshair.x=-1;return}var offset=plot.offset();crosshair.x=Math.max(0,Math.min(e.pageX-offset.left,plot.width()));crosshair.y=Math.max(0,Math.min(e.pageY-offset.top,plot.height()));plot.triggerRedrawOverlay()}plot.hooks.bindEvents.push(function(plot,eventHolder){if(!plot.getOptions().crosshair.mode)return;eventHolder.mouseout(onMouseOut);eventHolder.mousemove(onMouseMove)});plot.hooks.drawOverlay.push(function(plot,ctx){var c=plot.getOptions().crosshair;if(!c.mode)return;var plotOffset=plot.getPlotOffset();ctx.save();ctx.translate(plotOffset.left,plotOffset.top);if(crosshair.x!=-1){var adj=plot.getOptions().crosshair.lineWidth%2?.5:0;ctx.strokeStyle=c.color;ctx.lineWidth=c.lineWidth;ctx.lineJoin="round";ctx.beginPath();if(c.mode.indexOf("x")!=-1){var drawX=Math.floor(crosshair.x)+adj;ctx.moveTo(drawX,0);ctx.lineTo(drawX,plot.height())}if(c.mode.indexOf("y")!=-1){var drawY=Math.floor(crosshair.y)+adj;ctx.moveTo(0,drawY);ctx.lineTo(plot.width(),drawY)}ctx.stroke()}ctx.restore()});plot.hooks.shutdown.push(function(plot,eventHolder){eventHolder.unbind("mouseout",onMouseOut);eventHolder.unbind("mousemove",onMouseMove)})}$.plot.plugins.push({init:init,options:options,name:"crosshair",version:"1.0"})})(jQuery);

/***/ }),

/***/ "./flot/jquery.flot.min.js":
/*!*********************************!*\
  !*** ./flot/jquery.flot.min.js ***!
  \*********************************/
/*! no static exports found */
/***/ (function(module, exports) {

/* Javascript plotting library for jQuery, version 0.8.3.

Copyright (c) 2007-2014 IOLA and Ole Laursen.
Licensed under the MIT license.

*/
(function($){$.color={};$.color.make=function(r,g,b,a){var o={};o.r=r||0;o.g=g||0;o.b=b||0;o.a=a!=null?a:1;o.add=function(c,d){for(var i=0;i<c.length;++i)o[c.charAt(i)]+=d;return o.normalize()};o.scale=function(c,f){for(var i=0;i<c.length;++i)o[c.charAt(i)]*=f;return o.normalize()};o.toString=function(){if(o.a>=1){return"rgb("+[o.r,o.g,o.b].join(",")+")"}else{return"rgba("+[o.r,o.g,o.b,o.a].join(",")+")"}};o.normalize=function(){function clamp(min,value,max){return value<min?min:value>max?max:value}o.r=clamp(0,parseInt(o.r),255);o.g=clamp(0,parseInt(o.g),255);o.b=clamp(0,parseInt(o.b),255);o.a=clamp(0,o.a,1);return o};o.clone=function(){return $.color.make(o.r,o.b,o.g,o.a)};return o.normalize()};$.color.extract=function(elem,css){var c;do{c=elem.css(css).toLowerCase();if(c!=""&&c!="transparent")break;elem=elem.parent()}while(elem.length&&!$.nodeName(elem.get(0),"body"));if(c=="rgba(0, 0, 0, 0)")c="transparent";return $.color.parse(c)};$.color.parse=function(str){var res,m=$.color.make;if(res=/rgb\(\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*\)/.exec(str))return m(parseInt(res[1],10),parseInt(res[2],10),parseInt(res[3],10));if(res=/rgba\(\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]{1,3})\s*,\s*([0-9]+(?:\.[0-9]+)?)\s*\)/.exec(str))return m(parseInt(res[1],10),parseInt(res[2],10),parseInt(res[3],10),parseFloat(res[4]));if(res=/rgb\(\s*([0-9]+(?:\.[0-9]+)?)\%\s*,\s*([0-9]+(?:\.[0-9]+)?)\%\s*,\s*([0-9]+(?:\.[0-9]+)?)\%\s*\)/.exec(str))return m(parseFloat(res[1])*2.55,parseFloat(res[2])*2.55,parseFloat(res[3])*2.55);if(res=/rgba\(\s*([0-9]+(?:\.[0-9]+)?)\%\s*,\s*([0-9]+(?:\.[0-9]+)?)\%\s*,\s*([0-9]+(?:\.[0-9]+)?)\%\s*,\s*([0-9]+(?:\.[0-9]+)?)\s*\)/.exec(str))return m(parseFloat(res[1])*2.55,parseFloat(res[2])*2.55,parseFloat(res[3])*2.55,parseFloat(res[4]));if(res=/#([a-fA-F0-9]{2})([a-fA-F0-9]{2})([a-fA-F0-9]{2})/.exec(str))return m(parseInt(res[1],16),parseInt(res[2],16),parseInt(res[3],16));if(res=/#([a-fA-F0-9])([a-fA-F0-9])([a-fA-F0-9])/.exec(str))return m(parseInt(res[1]+res[1],16),parseInt(res[2]+res[2],16),parseInt(res[3]+res[3],16));var name=$.trim(str).toLowerCase();if(name=="transparent")return m(255,255,255,0);else{res=lookupColors[name]||[0,0,0];return m(res[0],res[1],res[2])}};var lookupColors={aqua:[0,255,255],azure:[240,255,255],beige:[245,245,220],black:[0,0,0],blue:[0,0,255],brown:[165,42,42],cyan:[0,255,255],darkblue:[0,0,139],darkcyan:[0,139,139],darkgrey:[169,169,169],darkgreen:[0,100,0],darkkhaki:[189,183,107],darkmagenta:[139,0,139],darkolivegreen:[85,107,47],darkorange:[255,140,0],darkorchid:[153,50,204],darkred:[139,0,0],darksalmon:[233,150,122],darkviolet:[148,0,211],fuchsia:[255,0,255],gold:[255,215,0],green:[0,128,0],indigo:[75,0,130],khaki:[240,230,140],lightblue:[173,216,230],lightcyan:[224,255,255],lightgreen:[144,238,144],lightgrey:[211,211,211],lightpink:[255,182,193],lightyellow:[255,255,224],lime:[0,255,0],magenta:[255,0,255],maroon:[128,0,0],navy:[0,0,128],olive:[128,128,0],orange:[255,165,0],pink:[255,192,203],purple:[128,0,128],violet:[128,0,128],red:[255,0,0],silver:[192,192,192],white:[255,255,255],yellow:[255,255,0]}})(jQuery);(function($){var hasOwnProperty=Object.prototype.hasOwnProperty;if(!$.fn.detach){$.fn.detach=function(){return this.each(function(){if(this.parentNode){this.parentNode.removeChild(this)}})}}function Canvas(cls,container){var element=container.children("."+cls)[0];if(element==null){element=document.createElement("canvas");element.className=cls;$(element).css({direction:"ltr",position:"absolute",left:0,top:0}).appendTo(container);if(!element.getContext){if(window.G_vmlCanvasManager){element=window.G_vmlCanvasManager.initElement(element)}else{throw new Error("Canvas is not available. If you're using IE with a fall-back such as Excanvas, then there's either a mistake in your conditional include, or the page has no DOCTYPE and is rendering in Quirks Mode.")}}}this.element=element;var context=this.context=element.getContext("2d");var devicePixelRatio=window.devicePixelRatio||1,backingStoreRatio=context.webkitBackingStorePixelRatio||context.mozBackingStorePixelRatio||context.msBackingStorePixelRatio||context.oBackingStorePixelRatio||context.backingStorePixelRatio||1;this.pixelRatio=devicePixelRatio/backingStoreRatio;this.resize(container.width(),container.height());this.textContainer=null;this.text={};this._textCache={}}Canvas.prototype.resize=function(width,height){if(width<=0||height<=0){throw new Error("Invalid dimensions for plot, width = "+width+", height = "+height)}var element=this.element,context=this.context,pixelRatio=this.pixelRatio;if(this.width!=width){element.width=width*pixelRatio;element.style.width=width+"px";this.width=width}if(this.height!=height){element.height=height*pixelRatio;element.style.height=height+"px";this.height=height}context.restore();context.save();context.scale(pixelRatio,pixelRatio)};Canvas.prototype.clear=function(){this.context.clearRect(0,0,this.width,this.height)};Canvas.prototype.render=function(){var cache=this._textCache;for(var layerKey in cache){if(hasOwnProperty.call(cache,layerKey)){var layer=this.getTextLayer(layerKey),layerCache=cache[layerKey];layer.hide();for(var styleKey in layerCache){if(hasOwnProperty.call(layerCache,styleKey)){var styleCache=layerCache[styleKey];for(var key in styleCache){if(hasOwnProperty.call(styleCache,key)){var positions=styleCache[key].positions;for(var i=0,position;position=positions[i];i++){if(position.active){if(!position.rendered){layer.append(position.element);position.rendered=true}}else{positions.splice(i--,1);if(position.rendered){position.element.detach()}}}if(positions.length==0){delete styleCache[key]}}}}}layer.show()}}};Canvas.prototype.getTextLayer=function(classes){var layer=this.text[classes];if(layer==null){if(this.textContainer==null){this.textContainer=$("<div class='flot-text'></div>").css({position:"absolute",top:0,left:0,bottom:0,right:0,"font-size":"smaller",color:"#545454"}).insertAfter(this.element)}layer=this.text[classes]=$("<div></div>").addClass(classes).css({position:"absolute",top:0,left:0,bottom:0,right:0}).appendTo(this.textContainer)}return layer};Canvas.prototype.getTextInfo=function(layer,text,font,angle,width){var textStyle,layerCache,styleCache,info;text=""+text;if(typeof font==="object"){textStyle=font.style+" "+font.variant+" "+font.weight+" "+font.size+"px/"+font.lineHeight+"px "+font.family}else{textStyle=font}layerCache=this._textCache[layer];if(layerCache==null){layerCache=this._textCache[layer]={}}styleCache=layerCache[textStyle];if(styleCache==null){styleCache=layerCache[textStyle]={}}info=styleCache[text];if(info==null){var element=$("<div></div>").html(text).css({position:"absolute","max-width":width,top:-9999}).appendTo(this.getTextLayer(layer));if(typeof font==="object"){element.css({font:textStyle,color:font.color})}else if(typeof font==="string"){element.addClass(font)}info=styleCache[text]={width:element.outerWidth(true),height:element.outerHeight(true),element:element,positions:[]};element.detach()}return info};Canvas.prototype.addText=function(layer,x,y,text,font,angle,width,halign,valign){var info=this.getTextInfo(layer,text,font,angle,width),positions=info.positions;if(halign=="center"){x-=info.width/2}else if(halign=="right"){x-=info.width}if(valign=="middle"){y-=info.height/2}else if(valign=="bottom"){y-=info.height}for(var i=0,position;position=positions[i];i++){if(position.x==x&&position.y==y){position.active=true;return}}position={active:true,rendered:false,element:positions.length?info.element.clone():info.element,x:x,y:y};positions.push(position);position.element.css({top:Math.round(y),left:Math.round(x),"text-align":halign})};Canvas.prototype.removeText=function(layer,x,y,text,font,angle){if(text==null){var layerCache=this._textCache[layer];if(layerCache!=null){for(var styleKey in layerCache){if(hasOwnProperty.call(layerCache,styleKey)){var styleCache=layerCache[styleKey];for(var key in styleCache){if(hasOwnProperty.call(styleCache,key)){var positions=styleCache[key].positions;for(var i=0,position;position=positions[i];i++){position.active=false}}}}}}}else{var positions=this.getTextInfo(layer,text,font,angle).positions;for(var i=0,position;position=positions[i];i++){if(position.x==x&&position.y==y){position.active=false}}}};function Plot(placeholder,data_,options_,plugins){var series=[],options={colors:["#edc240","#afd8f8","#cb4b4b","#4da74d","#9440ed"],legend:{show:true,noColumns:1,labelFormatter:null,labelBoxBorderColor:"#ccc",container:null,position:"ne",margin:5,backgroundColor:null,backgroundOpacity:.85,sorted:null},xaxis:{show:null,position:"bottom",mode:null,font:null,color:null,tickColor:null,transform:null,inverseTransform:null,min:null,max:null,autoscaleMargin:null,ticks:null,tickFormatter:null,labelWidth:null,labelHeight:null,reserveSpace:null,tickLength:null,alignTicksWithAxis:null,tickDecimals:null,tickSize:null,minTickSize:null},yaxis:{autoscaleMargin:.02,position:"left"},xaxes:[],yaxes:[],series:{points:{show:false,radius:3,lineWidth:2,fill:true,fillColor:"#ffffff",symbol:"circle"},lines:{lineWidth:2,fill:false,fillColor:null,steps:false},bars:{show:false,lineWidth:2,barWidth:1,fill:true,fillColor:null,align:"left",horizontal:false,zero:true},shadowSize:3,highlightColor:null},grid:{show:true,aboveData:false,color:"#545454",backgroundColor:null,borderColor:null,tickColor:null,margin:0,labelMargin:5,axisMargin:8,borderWidth:2,minBorderMargin:null,markings:null,markingsColor:"#f4f4f4",markingsLineWidth:2,clickable:false,hoverable:false,autoHighlight:true,mouseActiveRadius:10},interaction:{redrawOverlayInterval:1e3/60},hooks:{}},surface=null,overlay=null,eventHolder=null,ctx=null,octx=null,xaxes=[],yaxes=[],plotOffset={left:0,right:0,top:0,bottom:0},plotWidth=0,plotHeight=0,hooks={processOptions:[],processRawData:[],processDatapoints:[],processOffset:[],drawBackground:[],drawSeries:[],draw:[],bindEvents:[],drawOverlay:[],shutdown:[]},plot=this;plot.setData=setData;plot.setupGrid=setupGrid;plot.draw=draw;plot.getPlaceholder=function(){return placeholder};plot.getCanvas=function(){return surface.element};plot.getPlotOffset=function(){return plotOffset};plot.width=function(){return plotWidth};plot.height=function(){return plotHeight};plot.offset=function(){var o=eventHolder.offset();o.left+=plotOffset.left;o.top+=plotOffset.top;return o};plot.getData=function(){return series};plot.getAxes=function(){var res={},i;$.each(xaxes.concat(yaxes),function(_,axis){if(axis)res[axis.direction+(axis.n!=1?axis.n:"")+"axis"]=axis});return res};plot.getXAxes=function(){return xaxes};plot.getYAxes=function(){return yaxes};plot.c2p=canvasToAxisCoords;plot.p2c=axisToCanvasCoords;plot.getOptions=function(){return options};plot.highlight=highlight;plot.unhighlight=unhighlight;plot.triggerRedrawOverlay=triggerRedrawOverlay;plot.pointOffset=function(point){return{left:parseInt(xaxes[axisNumber(point,"x")-1].p2c(+point.x)+plotOffset.left,10),top:parseInt(yaxes[axisNumber(point,"y")-1].p2c(+point.y)+plotOffset.top,10)}};plot.shutdown=shutdown;plot.destroy=function(){shutdown();placeholder.removeData("plot").empty();series=[];options=null;surface=null;overlay=null;eventHolder=null;ctx=null;octx=null;xaxes=[];yaxes=[];hooks=null;highlights=[];plot=null};plot.resize=function(){var width=placeholder.width(),height=placeholder.height();surface.resize(width,height);overlay.resize(width,height)};plot.hooks=hooks;initPlugins(plot);parseOptions(options_);setupCanvases();setData(data_);setupGrid();draw();bindEvents();function executeHooks(hook,args){args=[plot].concat(args);for(var i=0;i<hook.length;++i)hook[i].apply(this,args)}function initPlugins(){var classes={Canvas:Canvas};for(var i=0;i<plugins.length;++i){var p=plugins[i];p.init(plot,classes);if(p.options)$.extend(true,options,p.options)}}function parseOptions(opts){$.extend(true,options,opts);if(opts&&opts.colors){options.colors=opts.colors}if(options.xaxis.color==null)options.xaxis.color=$.color.parse(options.grid.color).scale("a",.22).toString();if(options.yaxis.color==null)options.yaxis.color=$.color.parse(options.grid.color).scale("a",.22).toString();if(options.xaxis.tickColor==null)options.xaxis.tickColor=options.grid.tickColor||options.xaxis.color;if(options.yaxis.tickColor==null)options.yaxis.tickColor=options.grid.tickColor||options.yaxis.color;if(options.grid.borderColor==null)options.grid.borderColor=options.grid.color;if(options.grid.tickColor==null)options.grid.tickColor=$.color.parse(options.grid.color).scale("a",.22).toString();var i,axisOptions,axisCount,fontSize=placeholder.css("font-size"),fontSizeDefault=fontSize?+fontSize.replace("px",""):13,fontDefaults={style:placeholder.css("font-style"),size:Math.round(.8*fontSizeDefault),variant:placeholder.css("font-variant"),weight:placeholder.css("font-weight"),family:placeholder.css("font-family")};axisCount=options.xaxes.length||1;for(i=0;i<axisCount;++i){axisOptions=options.xaxes[i];if(axisOptions&&!axisOptions.tickColor){axisOptions.tickColor=axisOptions.color}axisOptions=$.extend(true,{},options.xaxis,axisOptions);options.xaxes[i]=axisOptions;if(axisOptions.font){axisOptions.font=$.extend({},fontDefaults,axisOptions.font);if(!axisOptions.font.color){axisOptions.font.color=axisOptions.color}if(!axisOptions.font.lineHeight){axisOptions.font.lineHeight=Math.round(axisOptions.font.size*1.15)}}}axisCount=options.yaxes.length||1;for(i=0;i<axisCount;++i){axisOptions=options.yaxes[i];if(axisOptions&&!axisOptions.tickColor){axisOptions.tickColor=axisOptions.color}axisOptions=$.extend(true,{},options.yaxis,axisOptions);options.yaxes[i]=axisOptions;if(axisOptions.font){axisOptions.font=$.extend({},fontDefaults,axisOptions.font);if(!axisOptions.font.color){axisOptions.font.color=axisOptions.color}if(!axisOptions.font.lineHeight){axisOptions.font.lineHeight=Math.round(axisOptions.font.size*1.15)}}}if(options.xaxis.noTicks&&options.xaxis.ticks==null)options.xaxis.ticks=options.xaxis.noTicks;if(options.yaxis.noTicks&&options.yaxis.ticks==null)options.yaxis.ticks=options.yaxis.noTicks;if(options.x2axis){options.xaxes[1]=$.extend(true,{},options.xaxis,options.x2axis);options.xaxes[1].position="top";if(options.x2axis.min==null){options.xaxes[1].min=null}if(options.x2axis.max==null){options.xaxes[1].max=null}}if(options.y2axis){options.yaxes[1]=$.extend(true,{},options.yaxis,options.y2axis);options.yaxes[1].position="right";if(options.y2axis.min==null){options.yaxes[1].min=null}if(options.y2axis.max==null){options.yaxes[1].max=null}}if(options.grid.coloredAreas)options.grid.markings=options.grid.coloredAreas;if(options.grid.coloredAreasColor)options.grid.markingsColor=options.grid.coloredAreasColor;if(options.lines)$.extend(true,options.series.lines,options.lines);if(options.points)$.extend(true,options.series.points,options.points);if(options.bars)$.extend(true,options.series.bars,options.bars);if(options.shadowSize!=null)options.series.shadowSize=options.shadowSize;if(options.highlightColor!=null)options.series.highlightColor=options.highlightColor;for(i=0;i<options.xaxes.length;++i)getOrCreateAxis(xaxes,i+1).options=options.xaxes[i];for(i=0;i<options.yaxes.length;++i)getOrCreateAxis(yaxes,i+1).options=options.yaxes[i];for(var n in hooks)if(options.hooks[n]&&options.hooks[n].length)hooks[n]=hooks[n].concat(options.hooks[n]);executeHooks(hooks.processOptions,[options])}function setData(d){series=parseData(d);fillInSeriesOptions();processData()}function parseData(d){var res=[];for(var i=0;i<d.length;++i){var s=$.extend(true,{},options.series);if(d[i].data!=null){s.data=d[i].data;delete d[i].data;$.extend(true,s,d[i]);d[i].data=s.data}else s.data=d[i];res.push(s)}return res}function axisNumber(obj,coord){var a=obj[coord+"axis"];if(typeof a=="object")a=a.n;if(typeof a!="number")a=1;return a}function allAxes(){return $.grep(xaxes.concat(yaxes),function(a){return a})}function canvasToAxisCoords(pos){var res={},i,axis;for(i=0;i<xaxes.length;++i){axis=xaxes[i];if(axis&&axis.used)res["x"+axis.n]=axis.c2p(pos.left)}for(i=0;i<yaxes.length;++i){axis=yaxes[i];if(axis&&axis.used)res["y"+axis.n]=axis.c2p(pos.top)}if(res.x1!==undefined)res.x=res.x1;if(res.y1!==undefined)res.y=res.y1;return res}function axisToCanvasCoords(pos){var res={},i,axis,key;for(i=0;i<xaxes.length;++i){axis=xaxes[i];if(axis&&axis.used){key="x"+axis.n;if(pos[key]==null&&axis.n==1)key="x";if(pos[key]!=null){res.left=axis.p2c(pos[key]);break}}}for(i=0;i<yaxes.length;++i){axis=yaxes[i];if(axis&&axis.used){key="y"+axis.n;if(pos[key]==null&&axis.n==1)key="y";if(pos[key]!=null){res.top=axis.p2c(pos[key]);break}}}return res}function getOrCreateAxis(axes,number){if(!axes[number-1])axes[number-1]={n:number,direction:axes==xaxes?"x":"y",options:$.extend(true,{},axes==xaxes?options.xaxis:options.yaxis)};return axes[number-1]}function fillInSeriesOptions(){var neededColors=series.length,maxIndex=-1,i;for(i=0;i<series.length;++i){var sc=series[i].color;if(sc!=null){neededColors--;if(typeof sc=="number"&&sc>maxIndex){maxIndex=sc}}}if(neededColors<=maxIndex){neededColors=maxIndex+1}var c,colors=[],colorPool=options.colors,colorPoolSize=colorPool.length,variation=0;for(i=0;i<neededColors;i++){c=$.color.parse(colorPool[i%colorPoolSize]||"#666");if(i%colorPoolSize==0&&i){if(variation>=0){if(variation<.5){variation=-variation-.2}else variation=0}else variation=-variation}colors[i]=c.scale("rgb",1+variation)}var colori=0,s;for(i=0;i<series.length;++i){s=series[i];if(s.color==null){s.color=colors[colori].toString();++colori}else if(typeof s.color=="number")s.color=colors[s.color].toString();if(s.lines.show==null){var v,show=true;for(v in s)if(s[v]&&s[v].show){show=false;break}if(show)s.lines.show=true}if(s.lines.zero==null){s.lines.zero=!!s.lines.fill}s.xaxis=getOrCreateAxis(xaxes,axisNumber(s,"x"));s.yaxis=getOrCreateAxis(yaxes,axisNumber(s,"y"))}}function processData(){var topSentry=Number.POSITIVE_INFINITY,bottomSentry=Number.NEGATIVE_INFINITY,fakeInfinity=Number.MAX_VALUE,i,j,k,m,length,s,points,ps,x,y,axis,val,f,p,data,format;function updateAxis(axis,min,max){if(min<axis.datamin&&min!=-fakeInfinity)axis.datamin=min;if(max>axis.datamax&&max!=fakeInfinity)axis.datamax=max}$.each(allAxes(),function(_,axis){axis.datamin=topSentry;axis.datamax=bottomSentry;axis.used=false});for(i=0;i<series.length;++i){s=series[i];s.datapoints={points:[]};executeHooks(hooks.processRawData,[s,s.data,s.datapoints])}for(i=0;i<series.length;++i){s=series[i];data=s.data;format=s.datapoints.format;if(!format){format=[];format.push({x:true,number:true,required:true});format.push({y:true,number:true,required:true});if(s.bars.show||s.lines.show&&s.lines.fill){var autoscale=!!(s.bars.show&&s.bars.zero||s.lines.show&&s.lines.zero);format.push({y:true,number:true,required:false,defaultValue:0,autoscale:autoscale});if(s.bars.horizontal){delete format[format.length-1].y;format[format.length-1].x=true}}s.datapoints.format=format}if(s.datapoints.pointsize!=null)continue;s.datapoints.pointsize=format.length;ps=s.datapoints.pointsize;points=s.datapoints.points;var insertSteps=s.lines.show&&s.lines.steps;s.xaxis.used=s.yaxis.used=true;for(j=k=0;j<data.length;++j,k+=ps){p=data[j];var nullify=p==null;if(!nullify){for(m=0;m<ps;++m){val=p[m];f=format[m];if(f){if(f.number&&val!=null){val=+val;if(isNaN(val))val=null;else if(val==Infinity)val=fakeInfinity;else if(val==-Infinity)val=-fakeInfinity}if(val==null){if(f.required)nullify=true;if(f.defaultValue!=null)val=f.defaultValue}}points[k+m]=val}}if(nullify){for(m=0;m<ps;++m){val=points[k+m];if(val!=null){f=format[m];if(f.autoscale!==false){if(f.x){updateAxis(s.xaxis,val,val)}if(f.y){updateAxis(s.yaxis,val,val)}}}points[k+m]=null}}else{if(insertSteps&&k>0&&points[k-ps]!=null&&points[k-ps]!=points[k]&&points[k-ps+1]!=points[k+1]){for(m=0;m<ps;++m)points[k+ps+m]=points[k+m];points[k+1]=points[k-ps+1];k+=ps}}}}for(i=0;i<series.length;++i){s=series[i];executeHooks(hooks.processDatapoints,[s,s.datapoints])}for(i=0;i<series.length;++i){s=series[i];points=s.datapoints.points;ps=s.datapoints.pointsize;format=s.datapoints.format;var xmin=topSentry,ymin=topSentry,xmax=bottomSentry,ymax=bottomSentry;for(j=0;j<points.length;j+=ps){if(points[j]==null)continue;for(m=0;m<ps;++m){val=points[j+m];f=format[m];if(!f||f.autoscale===false||val==fakeInfinity||val==-fakeInfinity)continue;if(f.x){if(val<xmin)xmin=val;if(val>xmax)xmax=val}if(f.y){if(val<ymin)ymin=val;if(val>ymax)ymax=val}}}if(s.bars.show){var delta;switch(s.bars.align){case"left":delta=0;break;case"right":delta=-s.bars.barWidth;break;default:delta=-s.bars.barWidth/2}if(s.bars.horizontal){ymin+=delta;ymax+=delta+s.bars.barWidth}else{xmin+=delta;xmax+=delta+s.bars.barWidth}}updateAxis(s.xaxis,xmin,xmax);updateAxis(s.yaxis,ymin,ymax)}$.each(allAxes(),function(_,axis){if(axis.datamin==topSentry)axis.datamin=null;if(axis.datamax==bottomSentry)axis.datamax=null})}function setupCanvases(){placeholder.css("padding",0).children().filter(function(){return!$(this).hasClass("flot-overlay")&&!$(this).hasClass("flot-base")}).remove();if(placeholder.css("position")=="static")placeholder.css("position","relative");surface=new Canvas("flot-base",placeholder);overlay=new Canvas("flot-overlay",placeholder);ctx=surface.context;octx=overlay.context;eventHolder=$(overlay.element).unbind();var existing=placeholder.data("plot");if(existing){existing.shutdown();overlay.clear()}placeholder.data("plot",plot)}function bindEvents(){if(options.grid.hoverable){eventHolder.mousemove(onMouseMove);eventHolder.bind("mouseleave",onMouseLeave)}if(options.grid.clickable)eventHolder.click(onClick);executeHooks(hooks.bindEvents,[eventHolder])}function shutdown(){if(redrawTimeout)clearTimeout(redrawTimeout);eventHolder.unbind("mousemove",onMouseMove);eventHolder.unbind("mouseleave",onMouseLeave);eventHolder.unbind("click",onClick);executeHooks(hooks.shutdown,[eventHolder])}function setTransformationHelpers(axis){function identity(x){return x}var s,m,t=axis.options.transform||identity,it=axis.options.inverseTransform;if(axis.direction=="x"){s=axis.scale=plotWidth/Math.abs(t(axis.max)-t(axis.min));m=Math.min(t(axis.max),t(axis.min))}else{s=axis.scale=plotHeight/Math.abs(t(axis.max)-t(axis.min));s=-s;m=Math.max(t(axis.max),t(axis.min))}if(t==identity)axis.p2c=function(p){return(p-m)*s};else axis.p2c=function(p){return(t(p)-m)*s};if(!it)axis.c2p=function(c){return m+c/s};else axis.c2p=function(c){return it(m+c/s)}}function measureTickLabels(axis){var opts=axis.options,ticks=axis.ticks||[],labelWidth=opts.labelWidth||0,labelHeight=opts.labelHeight||0,maxWidth=labelWidth||(axis.direction=="x"?Math.floor(surface.width/(ticks.length||1)):null),legacyStyles=axis.direction+"Axis "+axis.direction+axis.n+"Axis",layer="flot-"+axis.direction+"-axis flot-"+axis.direction+axis.n+"-axis "+legacyStyles,font=opts.font||"flot-tick-label tickLabel";for(var i=0;i<ticks.length;++i){var t=ticks[i];if(!t.label)continue;var info=surface.getTextInfo(layer,t.label,font,null,maxWidth);labelWidth=Math.max(labelWidth,info.width);labelHeight=Math.max(labelHeight,info.height)}axis.labelWidth=opts.labelWidth||labelWidth;axis.labelHeight=opts.labelHeight||labelHeight}function allocateAxisBoxFirstPhase(axis){var lw=axis.labelWidth,lh=axis.labelHeight,pos=axis.options.position,isXAxis=axis.direction==="x",tickLength=axis.options.tickLength,axisMargin=options.grid.axisMargin,padding=options.grid.labelMargin,innermost=true,outermost=true,first=true,found=false;$.each(isXAxis?xaxes:yaxes,function(i,a){if(a&&(a.show||a.reserveSpace)){if(a===axis){found=true}else if(a.options.position===pos){if(found){outermost=false}else{innermost=false}}if(!found){first=false}}});if(outermost){axisMargin=0}if(tickLength==null){tickLength=first?"full":5}if(!isNaN(+tickLength))padding+=+tickLength;if(isXAxis){lh+=padding;if(pos=="bottom"){plotOffset.bottom+=lh+axisMargin;axis.box={top:surface.height-plotOffset.bottom,height:lh}}else{axis.box={top:plotOffset.top+axisMargin,height:lh};plotOffset.top+=lh+axisMargin}}else{lw+=padding;if(pos=="left"){axis.box={left:plotOffset.left+axisMargin,width:lw};plotOffset.left+=lw+axisMargin}else{plotOffset.right+=lw+axisMargin;axis.box={left:surface.width-plotOffset.right,width:lw}}}axis.position=pos;axis.tickLength=tickLength;axis.box.padding=padding;axis.innermost=innermost}function allocateAxisBoxSecondPhase(axis){if(axis.direction=="x"){axis.box.left=plotOffset.left-axis.labelWidth/2;axis.box.width=surface.width-plotOffset.left-plotOffset.right+axis.labelWidth}else{axis.box.top=plotOffset.top-axis.labelHeight/2;axis.box.height=surface.height-plotOffset.bottom-plotOffset.top+axis.labelHeight}}function adjustLayoutForThingsStickingOut(){var minMargin=options.grid.minBorderMargin,axis,i;if(minMargin==null){minMargin=0;for(i=0;i<series.length;++i)minMargin=Math.max(minMargin,2*(series[i].points.radius+series[i].points.lineWidth/2))}var margins={left:minMargin,right:minMargin,top:minMargin,bottom:minMargin};$.each(allAxes(),function(_,axis){if(axis.reserveSpace&&axis.ticks&&axis.ticks.length){if(axis.direction==="x"){margins.left=Math.max(margins.left,axis.labelWidth/2);margins.right=Math.max(margins.right,axis.labelWidth/2)}else{margins.bottom=Math.max(margins.bottom,axis.labelHeight/2);margins.top=Math.max(margins.top,axis.labelHeight/2)}}});plotOffset.left=Math.ceil(Math.max(margins.left,plotOffset.left));plotOffset.right=Math.ceil(Math.max(margins.right,plotOffset.right));plotOffset.top=Math.ceil(Math.max(margins.top,plotOffset.top));plotOffset.bottom=Math.ceil(Math.max(margins.bottom,plotOffset.bottom))}function setupGrid(){var i,axes=allAxes(),showGrid=options.grid.show;for(var a in plotOffset){var margin=options.grid.margin||0;plotOffset[a]=typeof margin=="number"?margin:margin[a]||0}executeHooks(hooks.processOffset,[plotOffset]);for(var a in plotOffset){if(typeof options.grid.borderWidth=="object"){plotOffset[a]+=showGrid?options.grid.borderWidth[a]:0}else{plotOffset[a]+=showGrid?options.grid.borderWidth:0}}$.each(axes,function(_,axis){var axisOpts=axis.options;axis.show=axisOpts.show==null?axis.used:axisOpts.show;axis.reserveSpace=axisOpts.reserveSpace==null?axis.show:axisOpts.reserveSpace;setRange(axis)});if(showGrid){var allocatedAxes=$.grep(axes,function(axis){return axis.show||axis.reserveSpace});$.each(allocatedAxes,function(_,axis){setupTickGeneration(axis);setTicks(axis);snapRangeToTicks(axis,axis.ticks);measureTickLabels(axis)});for(i=allocatedAxes.length-1;i>=0;--i)allocateAxisBoxFirstPhase(allocatedAxes[i]);adjustLayoutForThingsStickingOut();$.each(allocatedAxes,function(_,axis){allocateAxisBoxSecondPhase(axis)})}plotWidth=surface.width-plotOffset.left-plotOffset.right;plotHeight=surface.height-plotOffset.bottom-plotOffset.top;$.each(axes,function(_,axis){setTransformationHelpers(axis)});if(showGrid){drawAxisLabels()}insertLegend()}function setRange(axis){var opts=axis.options,min=+(opts.min!=null?opts.min:axis.datamin),max=+(opts.max!=null?opts.max:axis.datamax),delta=max-min;if(delta==0){var widen=max==0?1:.01;if(opts.min==null)min-=widen;if(opts.max==null||opts.min!=null)max+=widen}else{var margin=opts.autoscaleMargin;if(margin!=null){if(opts.min==null){min-=delta*margin;if(min<0&&axis.datamin!=null&&axis.datamin>=0)min=0}if(opts.max==null){max+=delta*margin;if(max>0&&axis.datamax!=null&&axis.datamax<=0)max=0}}}axis.min=min;axis.max=max}function setupTickGeneration(axis){var opts=axis.options;var noTicks;if(typeof opts.ticks=="number"&&opts.ticks>0)noTicks=opts.ticks;else noTicks=.3*Math.sqrt(axis.direction=="x"?surface.width:surface.height);var delta=(axis.max-axis.min)/noTicks,dec=-Math.floor(Math.log(delta)/Math.LN10),maxDec=opts.tickDecimals;if(maxDec!=null&&dec>maxDec){dec=maxDec}var magn=Math.pow(10,-dec),norm=delta/magn,size;if(norm<1.5){size=1}else if(norm<3){size=2;if(norm>2.25&&(maxDec==null||dec+1<=maxDec)){size=2.5;++dec}}else if(norm<7.5){size=5}else{size=10}size*=magn;if(opts.minTickSize!=null&&size<opts.minTickSize){size=opts.minTickSize}axis.delta=delta;axis.tickDecimals=Math.max(0,maxDec!=null?maxDec:dec);axis.tickSize=opts.tickSize||size;if(opts.mode=="time"&&!axis.tickGenerator){throw new Error("Time mode requires the flot.time plugin.")}if(!axis.tickGenerator){axis.tickGenerator=function(axis){var ticks=[],start=floorInBase(axis.min,axis.tickSize),i=0,v=Number.NaN,prev;do{prev=v;v=start+i*axis.tickSize;ticks.push(v);++i}while(v<axis.max&&v!=prev);return ticks};axis.tickFormatter=function(value,axis){var factor=axis.tickDecimals?Math.pow(10,axis.tickDecimals):1;var formatted=""+Math.round(value*factor)/factor;if(axis.tickDecimals!=null){var decimal=formatted.indexOf(".");var precision=decimal==-1?0:formatted.length-decimal-1;if(precision<axis.tickDecimals){return(precision?formatted:formatted+".")+(""+factor).substr(1,axis.tickDecimals-precision)}}return formatted}}if($.isFunction(opts.tickFormatter))axis.tickFormatter=function(v,axis){return""+opts.tickFormatter(v,axis)};if(opts.alignTicksWithAxis!=null){var otherAxis=(axis.direction=="x"?xaxes:yaxes)[opts.alignTicksWithAxis-1];if(otherAxis&&otherAxis.used&&otherAxis!=axis){var niceTicks=axis.tickGenerator(axis);if(niceTicks.length>0){if(opts.min==null)axis.min=Math.min(axis.min,niceTicks[0]);if(opts.max==null&&niceTicks.length>1)axis.max=Math.max(axis.max,niceTicks[niceTicks.length-1])}axis.tickGenerator=function(axis){var ticks=[],v,i;for(i=0;i<otherAxis.ticks.length;++i){v=(otherAxis.ticks[i].v-otherAxis.min)/(otherAxis.max-otherAxis.min);v=axis.min+v*(axis.max-axis.min);ticks.push(v)}return ticks};if(!axis.mode&&opts.tickDecimals==null){var extraDec=Math.max(0,-Math.floor(Math.log(axis.delta)/Math.LN10)+1),ts=axis.tickGenerator(axis);if(!(ts.length>1&&/\..*0$/.test((ts[1]-ts[0]).toFixed(extraDec))))axis.tickDecimals=extraDec}}}}function setTicks(axis){var oticks=axis.options.ticks,ticks=[];if(oticks==null||typeof oticks=="number"&&oticks>0)ticks=axis.tickGenerator(axis);else if(oticks){if($.isFunction(oticks))ticks=oticks(axis);else ticks=oticks}var i,v;axis.ticks=[];for(i=0;i<ticks.length;++i){var label=null;var t=ticks[i];if(typeof t=="object"){v=+t[0];if(t.length>1)label=t[1]}else v=+t;if(label==null)label=axis.tickFormatter(v,axis);if(!isNaN(v))axis.ticks.push({v:v,label:label})}}function snapRangeToTicks(axis,ticks){if(axis.options.autoscaleMargin&&ticks.length>0){if(axis.options.min==null)axis.min=Math.min(axis.min,ticks[0].v);if(axis.options.max==null&&ticks.length>1)axis.max=Math.max(axis.max,ticks[ticks.length-1].v)}}function draw(){surface.clear();executeHooks(hooks.drawBackground,[ctx]);var grid=options.grid;if(grid.show&&grid.backgroundColor)drawBackground();if(grid.show&&!grid.aboveData){drawGrid()}for(var i=0;i<series.length;++i){executeHooks(hooks.drawSeries,[ctx,series[i]]);drawSeries(series[i])}executeHooks(hooks.draw,[ctx]);if(grid.show&&grid.aboveData){drawGrid()}surface.render();triggerRedrawOverlay()}function extractRange(ranges,coord){var axis,from,to,key,axes=allAxes();for(var i=0;i<axes.length;++i){axis=axes[i];if(axis.direction==coord){key=coord+axis.n+"axis";if(!ranges[key]&&axis.n==1)key=coord+"axis";if(ranges[key]){from=ranges[key].from;to=ranges[key].to;break}}}if(!ranges[key]){axis=coord=="x"?xaxes[0]:yaxes[0];from=ranges[coord+"1"];to=ranges[coord+"2"]}if(from!=null&&to!=null&&from>to){var tmp=from;from=to;to=tmp}return{from:from,to:to,axis:axis}}function drawBackground(){ctx.save();ctx.translate(plotOffset.left,plotOffset.top);ctx.fillStyle=getColorOrGradient(options.grid.backgroundColor,plotHeight,0,"rgba(255, 255, 255, 0)");ctx.fillRect(0,0,plotWidth,plotHeight);ctx.restore()}function drawGrid(){var i,axes,bw,bc;ctx.save();ctx.translate(plotOffset.left,plotOffset.top);var markings=options.grid.markings;if(markings){if($.isFunction(markings)){axes=plot.getAxes();axes.xmin=axes.xaxis.min;axes.xmax=axes.xaxis.max;axes.ymin=axes.yaxis.min;axes.ymax=axes.yaxis.max;markings=markings(axes)}for(i=0;i<markings.length;++i){var m=markings[i],xrange=extractRange(m,"x"),yrange=extractRange(m,"y");if(xrange.from==null)xrange.from=xrange.axis.min;if(xrange.to==null)xrange.to=xrange.axis.max;
if(yrange.from==null)yrange.from=yrange.axis.min;if(yrange.to==null)yrange.to=yrange.axis.max;if(xrange.to<xrange.axis.min||xrange.from>xrange.axis.max||yrange.to<yrange.axis.min||yrange.from>yrange.axis.max)continue;xrange.from=Math.max(xrange.from,xrange.axis.min);xrange.to=Math.min(xrange.to,xrange.axis.max);yrange.from=Math.max(yrange.from,yrange.axis.min);yrange.to=Math.min(yrange.to,yrange.axis.max);var xequal=xrange.from===xrange.to,yequal=yrange.from===yrange.to;if(xequal&&yequal){continue}xrange.from=Math.floor(xrange.axis.p2c(xrange.from));xrange.to=Math.floor(xrange.axis.p2c(xrange.to));yrange.from=Math.floor(yrange.axis.p2c(yrange.from));yrange.to=Math.floor(yrange.axis.p2c(yrange.to));if(xequal||yequal){var lineWidth=m.lineWidth||options.grid.markingsLineWidth,subPixel=lineWidth%2?.5:0;ctx.beginPath();ctx.strokeStyle=m.color||options.grid.markingsColor;ctx.lineWidth=lineWidth;if(xequal){ctx.moveTo(xrange.to+subPixel,yrange.from);ctx.lineTo(xrange.to+subPixel,yrange.to)}else{ctx.moveTo(xrange.from,yrange.to+subPixel);ctx.lineTo(xrange.to,yrange.to+subPixel)}ctx.stroke()}else{ctx.fillStyle=m.color||options.grid.markingsColor;ctx.fillRect(xrange.from,yrange.to,xrange.to-xrange.from,yrange.from-yrange.to)}}}axes=allAxes();bw=options.grid.borderWidth;for(var j=0;j<axes.length;++j){var axis=axes[j],box=axis.box,t=axis.tickLength,x,y,xoff,yoff;if(!axis.show||axis.ticks.length==0)continue;ctx.lineWidth=1;if(axis.direction=="x"){x=0;if(t=="full")y=axis.position=="top"?0:plotHeight;else y=box.top-plotOffset.top+(axis.position=="top"?box.height:0)}else{y=0;if(t=="full")x=axis.position=="left"?0:plotWidth;else x=box.left-plotOffset.left+(axis.position=="left"?box.width:0)}if(!axis.innermost){ctx.strokeStyle=axis.options.color;ctx.beginPath();xoff=yoff=0;if(axis.direction=="x")xoff=plotWidth+1;else yoff=plotHeight+1;if(ctx.lineWidth==1){if(axis.direction=="x"){y=Math.floor(y)+.5}else{x=Math.floor(x)+.5}}ctx.moveTo(x,y);ctx.lineTo(x+xoff,y+yoff);ctx.stroke()}ctx.strokeStyle=axis.options.tickColor;ctx.beginPath();for(i=0;i<axis.ticks.length;++i){var v=axis.ticks[i].v;xoff=yoff=0;if(isNaN(v)||v<axis.min||v>axis.max||t=="full"&&(typeof bw=="object"&&bw[axis.position]>0||bw>0)&&(v==axis.min||v==axis.max))continue;if(axis.direction=="x"){x=axis.p2c(v);yoff=t=="full"?-plotHeight:t;if(axis.position=="top")yoff=-yoff}else{y=axis.p2c(v);xoff=t=="full"?-plotWidth:t;if(axis.position=="left")xoff=-xoff}if(ctx.lineWidth==1){if(axis.direction=="x")x=Math.floor(x)+.5;else y=Math.floor(y)+.5}ctx.moveTo(x,y);ctx.lineTo(x+xoff,y+yoff)}ctx.stroke()}if(bw){bc=options.grid.borderColor;if(typeof bw=="object"||typeof bc=="object"){if(typeof bw!=="object"){bw={top:bw,right:bw,bottom:bw,left:bw}}if(typeof bc!=="object"){bc={top:bc,right:bc,bottom:bc,left:bc}}if(bw.top>0){ctx.strokeStyle=bc.top;ctx.lineWidth=bw.top;ctx.beginPath();ctx.moveTo(0-bw.left,0-bw.top/2);ctx.lineTo(plotWidth,0-bw.top/2);ctx.stroke()}if(bw.right>0){ctx.strokeStyle=bc.right;ctx.lineWidth=bw.right;ctx.beginPath();ctx.moveTo(plotWidth+bw.right/2,0-bw.top);ctx.lineTo(plotWidth+bw.right/2,plotHeight);ctx.stroke()}if(bw.bottom>0){ctx.strokeStyle=bc.bottom;ctx.lineWidth=bw.bottom;ctx.beginPath();ctx.moveTo(plotWidth+bw.right,plotHeight+bw.bottom/2);ctx.lineTo(0,plotHeight+bw.bottom/2);ctx.stroke()}if(bw.left>0){ctx.strokeStyle=bc.left;ctx.lineWidth=bw.left;ctx.beginPath();ctx.moveTo(0-bw.left/2,plotHeight+bw.bottom);ctx.lineTo(0-bw.left/2,0);ctx.stroke()}}else{ctx.lineWidth=bw;ctx.strokeStyle=options.grid.borderColor;ctx.strokeRect(-bw/2,-bw/2,plotWidth+bw,plotHeight+bw)}}ctx.restore()}function drawAxisLabels(){$.each(allAxes(),function(_,axis){var box=axis.box,legacyStyles=axis.direction+"Axis "+axis.direction+axis.n+"Axis",layer="flot-"+axis.direction+"-axis flot-"+axis.direction+axis.n+"-axis "+legacyStyles,font=axis.options.font||"flot-tick-label tickLabel",tick,x,y,halign,valign;surface.removeText(layer);if(!axis.show||axis.ticks.length==0)return;for(var i=0;i<axis.ticks.length;++i){tick=axis.ticks[i];if(!tick.label||tick.v<axis.min||tick.v>axis.max)continue;if(axis.direction=="x"){halign="center";x=plotOffset.left+axis.p2c(tick.v);if(axis.position=="bottom"){y=box.top+box.padding}else{y=box.top+box.height-box.padding;valign="bottom"}}else{valign="middle";y=plotOffset.top+axis.p2c(tick.v);if(axis.position=="left"){x=box.left+box.width-box.padding;halign="right"}else{x=box.left+box.padding}}surface.addText(layer,x,y,tick.label,font,null,null,halign,valign)}})}function drawSeries(series){if(series.lines.show)drawSeriesLines(series);if(series.bars.show)drawSeriesBars(series);if(series.points.show)drawSeriesPoints(series)}function drawSeriesLines(series){function plotLine(datapoints,xoffset,yoffset,axisx,axisy){var points=datapoints.points,ps=datapoints.pointsize,prevx=null,prevy=null;ctx.beginPath();for(var i=ps;i<points.length;i+=ps){var x1=points[i-ps],y1=points[i-ps+1],x2=points[i],y2=points[i+1];if(x1==null||x2==null)continue;if(y1<=y2&&y1<axisy.min){if(y2<axisy.min)continue;x1=(axisy.min-y1)/(y2-y1)*(x2-x1)+x1;y1=axisy.min}else if(y2<=y1&&y2<axisy.min){if(y1<axisy.min)continue;x2=(axisy.min-y1)/(y2-y1)*(x2-x1)+x1;y2=axisy.min}if(y1>=y2&&y1>axisy.max){if(y2>axisy.max)continue;x1=(axisy.max-y1)/(y2-y1)*(x2-x1)+x1;y1=axisy.max}else if(y2>=y1&&y2>axisy.max){if(y1>axisy.max)continue;x2=(axisy.max-y1)/(y2-y1)*(x2-x1)+x1;y2=axisy.max}if(x1<=x2&&x1<axisx.min){if(x2<axisx.min)continue;y1=(axisx.min-x1)/(x2-x1)*(y2-y1)+y1;x1=axisx.min}else if(x2<=x1&&x2<axisx.min){if(x1<axisx.min)continue;y2=(axisx.min-x1)/(x2-x1)*(y2-y1)+y1;x2=axisx.min}if(x1>=x2&&x1>axisx.max){if(x2>axisx.max)continue;y1=(axisx.max-x1)/(x2-x1)*(y2-y1)+y1;x1=axisx.max}else if(x2>=x1&&x2>axisx.max){if(x1>axisx.max)continue;y2=(axisx.max-x1)/(x2-x1)*(y2-y1)+y1;x2=axisx.max}if(x1!=prevx||y1!=prevy)ctx.moveTo(axisx.p2c(x1)+xoffset,axisy.p2c(y1)+yoffset);prevx=x2;prevy=y2;ctx.lineTo(axisx.p2c(x2)+xoffset,axisy.p2c(y2)+yoffset)}ctx.stroke()}function plotLineArea(datapoints,axisx,axisy){var points=datapoints.points,ps=datapoints.pointsize,bottom=Math.min(Math.max(0,axisy.min),axisy.max),i=0,top,areaOpen=false,ypos=1,segmentStart=0,segmentEnd=0;while(true){if(ps>0&&i>points.length+ps)break;i+=ps;var x1=points[i-ps],y1=points[i-ps+ypos],x2=points[i],y2=points[i+ypos];if(areaOpen){if(ps>0&&x1!=null&&x2==null){segmentEnd=i;ps=-ps;ypos=2;continue}if(ps<0&&i==segmentStart+ps){ctx.fill();areaOpen=false;ps=-ps;ypos=1;i=segmentStart=segmentEnd+ps;continue}}if(x1==null||x2==null)continue;if(x1<=x2&&x1<axisx.min){if(x2<axisx.min)continue;y1=(axisx.min-x1)/(x2-x1)*(y2-y1)+y1;x1=axisx.min}else if(x2<=x1&&x2<axisx.min){if(x1<axisx.min)continue;y2=(axisx.min-x1)/(x2-x1)*(y2-y1)+y1;x2=axisx.min}if(x1>=x2&&x1>axisx.max){if(x2>axisx.max)continue;y1=(axisx.max-x1)/(x2-x1)*(y2-y1)+y1;x1=axisx.max}else if(x2>=x1&&x2>axisx.max){if(x1>axisx.max)continue;y2=(axisx.max-x1)/(x2-x1)*(y2-y1)+y1;x2=axisx.max}if(!areaOpen){ctx.beginPath();ctx.moveTo(axisx.p2c(x1),axisy.p2c(bottom));areaOpen=true}if(y1>=axisy.max&&y2>=axisy.max){ctx.lineTo(axisx.p2c(x1),axisy.p2c(axisy.max));ctx.lineTo(axisx.p2c(x2),axisy.p2c(axisy.max));continue}else if(y1<=axisy.min&&y2<=axisy.min){ctx.lineTo(axisx.p2c(x1),axisy.p2c(axisy.min));ctx.lineTo(axisx.p2c(x2),axisy.p2c(axisy.min));continue}var x1old=x1,x2old=x2;if(y1<=y2&&y1<axisy.min&&y2>=axisy.min){x1=(axisy.min-y1)/(y2-y1)*(x2-x1)+x1;y1=axisy.min}else if(y2<=y1&&y2<axisy.min&&y1>=axisy.min){x2=(axisy.min-y1)/(y2-y1)*(x2-x1)+x1;y2=axisy.min}if(y1>=y2&&y1>axisy.max&&y2<=axisy.max){x1=(axisy.max-y1)/(y2-y1)*(x2-x1)+x1;y1=axisy.max}else if(y2>=y1&&y2>axisy.max&&y1<=axisy.max){x2=(axisy.max-y1)/(y2-y1)*(x2-x1)+x1;y2=axisy.max}if(x1!=x1old){ctx.lineTo(axisx.p2c(x1old),axisy.p2c(y1))}ctx.lineTo(axisx.p2c(x1),axisy.p2c(y1));ctx.lineTo(axisx.p2c(x2),axisy.p2c(y2));if(x2!=x2old){ctx.lineTo(axisx.p2c(x2),axisy.p2c(y2));ctx.lineTo(axisx.p2c(x2old),axisy.p2c(y2))}}}ctx.save();ctx.translate(plotOffset.left,plotOffset.top);ctx.lineJoin="round";var lw=series.lines.lineWidth,sw=series.shadowSize;if(lw>0&&sw>0){ctx.lineWidth=sw;ctx.strokeStyle="rgba(0,0,0,0.1)";var angle=Math.PI/18;plotLine(series.datapoints,Math.sin(angle)*(lw/2+sw/2),Math.cos(angle)*(lw/2+sw/2),series.xaxis,series.yaxis);ctx.lineWidth=sw/2;plotLine(series.datapoints,Math.sin(angle)*(lw/2+sw/4),Math.cos(angle)*(lw/2+sw/4),series.xaxis,series.yaxis)}ctx.lineWidth=lw;ctx.strokeStyle=series.color;var fillStyle=getFillStyle(series.lines,series.color,0,plotHeight);if(fillStyle){ctx.fillStyle=fillStyle;plotLineArea(series.datapoints,series.xaxis,series.yaxis)}if(lw>0)plotLine(series.datapoints,0,0,series.xaxis,series.yaxis);ctx.restore()}function drawSeriesPoints(series){function plotPoints(datapoints,radius,fillStyle,offset,shadow,axisx,axisy,symbol){var points=datapoints.points,ps=datapoints.pointsize;for(var i=0;i<points.length;i+=ps){var x=points[i],y=points[i+1];if(x==null||x<axisx.min||x>axisx.max||y<axisy.min||y>axisy.max)continue;ctx.beginPath();x=axisx.p2c(x);y=axisy.p2c(y)+offset;if(symbol=="circle")ctx.arc(x,y,radius,0,shadow?Math.PI:Math.PI*2,false);else symbol(ctx,x,y,radius,shadow);ctx.closePath();if(fillStyle){ctx.fillStyle=fillStyle;ctx.fill()}ctx.stroke()}}ctx.save();ctx.translate(plotOffset.left,plotOffset.top);var lw=series.points.lineWidth,sw=series.shadowSize,radius=series.points.radius,symbol=series.points.symbol;if(lw==0)lw=1e-4;if(lw>0&&sw>0){var w=sw/2;ctx.lineWidth=w;ctx.strokeStyle="rgba(0,0,0,0.1)";plotPoints(series.datapoints,radius,null,w+w/2,true,series.xaxis,series.yaxis,symbol);ctx.strokeStyle="rgba(0,0,0,0.2)";plotPoints(series.datapoints,radius,null,w/2,true,series.xaxis,series.yaxis,symbol)}ctx.lineWidth=lw;ctx.strokeStyle=series.color;plotPoints(series.datapoints,radius,getFillStyle(series.points,series.color),0,false,series.xaxis,series.yaxis,symbol);ctx.restore()}function drawBar(x,y,b,barLeft,barRight,fillStyleCallback,axisx,axisy,c,horizontal,lineWidth){var left,right,bottom,top,drawLeft,drawRight,drawTop,drawBottom,tmp;if(horizontal){drawBottom=drawRight=drawTop=true;drawLeft=false;left=b;right=x;top=y+barLeft;bottom=y+barRight;if(right<left){tmp=right;right=left;left=tmp;drawLeft=true;drawRight=false}}else{drawLeft=drawRight=drawTop=true;drawBottom=false;left=x+barLeft;right=x+barRight;bottom=b;top=y;if(top<bottom){tmp=top;top=bottom;bottom=tmp;drawBottom=true;drawTop=false}}if(right<axisx.min||left>axisx.max||top<axisy.min||bottom>axisy.max)return;if(left<axisx.min){left=axisx.min;drawLeft=false}if(right>axisx.max){right=axisx.max;drawRight=false}if(bottom<axisy.min){bottom=axisy.min;drawBottom=false}if(top>axisy.max){top=axisy.max;drawTop=false}left=axisx.p2c(left);bottom=axisy.p2c(bottom);right=axisx.p2c(right);top=axisy.p2c(top);if(fillStyleCallback){c.fillStyle=fillStyleCallback(bottom,top);c.fillRect(left,top,right-left,bottom-top)}if(lineWidth>0&&(drawLeft||drawRight||drawTop||drawBottom)){c.beginPath();c.moveTo(left,bottom);if(drawLeft)c.lineTo(left,top);else c.moveTo(left,top);if(drawTop)c.lineTo(right,top);else c.moveTo(right,top);if(drawRight)c.lineTo(right,bottom);else c.moveTo(right,bottom);if(drawBottom)c.lineTo(left,bottom);else c.moveTo(left,bottom);c.stroke()}}function drawSeriesBars(series){function plotBars(datapoints,barLeft,barRight,fillStyleCallback,axisx,axisy){var points=datapoints.points,ps=datapoints.pointsize;for(var i=0;i<points.length;i+=ps){if(points[i]==null)continue;drawBar(points[i],points[i+1],points[i+2],barLeft,barRight,fillStyleCallback,axisx,axisy,ctx,series.bars.horizontal,series.bars.lineWidth)}}ctx.save();ctx.translate(plotOffset.left,plotOffset.top);ctx.lineWidth=series.bars.lineWidth;ctx.strokeStyle=series.color;var barLeft;switch(series.bars.align){case"left":barLeft=0;break;case"right":barLeft=-series.bars.barWidth;break;default:barLeft=-series.bars.barWidth/2}var fillStyleCallback=series.bars.fill?function(bottom,top){return getFillStyle(series.bars,series.color,bottom,top)}:null;plotBars(series.datapoints,barLeft,barLeft+series.bars.barWidth,fillStyleCallback,series.xaxis,series.yaxis);ctx.restore()}function getFillStyle(filloptions,seriesColor,bottom,top){var fill=filloptions.fill;if(!fill)return null;if(filloptions.fillColor)return getColorOrGradient(filloptions.fillColor,bottom,top,seriesColor);var c=$.color.parse(seriesColor);c.a=typeof fill=="number"?fill:.4;c.normalize();return c.toString()}function insertLegend(){if(options.legend.container!=null){$(options.legend.container).html("")}else{placeholder.find(".legend").remove()}if(!options.legend.show){return}var fragments=[],entries=[],rowStarted=false,lf=options.legend.labelFormatter,s,label;for(var i=0;i<series.length;++i){s=series[i];if(s.label){label=lf?lf(s.label,s):s.label;if(label){entries.push({label:label,color:s.color})}}}if(options.legend.sorted){if($.isFunction(options.legend.sorted)){entries.sort(options.legend.sorted)}else if(options.legend.sorted=="reverse"){entries.reverse()}else{var ascending=options.legend.sorted!="descending";entries.sort(function(a,b){return a.label==b.label?0:a.label<b.label!=ascending?1:-1})}}for(var i=0;i<entries.length;++i){var entry=entries[i];if(i%options.legend.noColumns==0){if(rowStarted)fragments.push("</tr>");fragments.push("<tr>");rowStarted=true}fragments.push('<td class="legendColorBox"><div style="border:1px solid '+options.legend.labelBoxBorderColor+';padding:1px"><div style="width:4px;height:0;border:5px solid '+entry.color+';overflow:hidden"></div></div></td>'+'<td class="legendLabel">'+entry.label+"</td>")}if(rowStarted)fragments.push("</tr>");if(fragments.length==0)return;var table='<table style="font-size:smaller;color:'+options.grid.color+'">'+fragments.join("")+"</table>";if(options.legend.container!=null)$(options.legend.container).html(table);else{var pos="",p=options.legend.position,m=options.legend.margin;if(m[0]==null)m=[m,m];if(p.charAt(0)=="n")pos+="top:"+(m[1]+plotOffset.top)+"px;";else if(p.charAt(0)=="s")pos+="bottom:"+(m[1]+plotOffset.bottom)+"px;";if(p.charAt(1)=="e")pos+="right:"+(m[0]+plotOffset.right)+"px;";else if(p.charAt(1)=="w")pos+="left:"+(m[0]+plotOffset.left)+"px;";var legend=$('<div class="legend">'+table.replace('style="','style="position:absolute;'+pos+";")+"</div>").appendTo(placeholder);if(options.legend.backgroundOpacity!=0){var c=options.legend.backgroundColor;if(c==null){c=options.grid.backgroundColor;if(c&&typeof c=="string")c=$.color.parse(c);else c=$.color.extract(legend,"background-color");c.a=1;c=c.toString()}var div=legend.children();$('<div style="position:absolute;width:'+div.width()+"px;height:"+div.height()+"px;"+pos+"background-color:"+c+';"> </div>').prependTo(legend).css("opacity",options.legend.backgroundOpacity)}}}var highlights=[],redrawTimeout=null;function findNearbyItem(mouseX,mouseY,seriesFilter){var maxDistance=options.grid.mouseActiveRadius,smallestDistance=maxDistance*maxDistance+1,item=null,foundPoint=false,i,j,ps;for(i=series.length-1;i>=0;--i){if(!seriesFilter(series[i]))continue;var s=series[i],axisx=s.xaxis,axisy=s.yaxis,points=s.datapoints.points,mx=axisx.c2p(mouseX),my=axisy.c2p(mouseY),maxx=maxDistance/axisx.scale,maxy=maxDistance/axisy.scale;ps=s.datapoints.pointsize;if(axisx.options.inverseTransform)maxx=Number.MAX_VALUE;if(axisy.options.inverseTransform)maxy=Number.MAX_VALUE;if(s.lines.show||s.points.show){for(j=0;j<points.length;j+=ps){var x=points[j],y=points[j+1];if(x==null)continue;if(x-mx>maxx||x-mx<-maxx||y-my>maxy||y-my<-maxy)continue;var dx=Math.abs(axisx.p2c(x)-mouseX),dy=Math.abs(axisy.p2c(y)-mouseY),dist=dx*dx+dy*dy;if(dist<smallestDistance){smallestDistance=dist;item=[i,j/ps]}}}if(s.bars.show&&!item){var barLeft,barRight;switch(s.bars.align){case"left":barLeft=0;break;case"right":barLeft=-s.bars.barWidth;break;default:barLeft=-s.bars.barWidth/2}barRight=barLeft+s.bars.barWidth;for(j=0;j<points.length;j+=ps){var x=points[j],y=points[j+1],b=points[j+2];if(x==null)continue;if(series[i].bars.horizontal?mx<=Math.max(b,x)&&mx>=Math.min(b,x)&&my>=y+barLeft&&my<=y+barRight:mx>=x+barLeft&&mx<=x+barRight&&my>=Math.min(b,y)&&my<=Math.max(b,y))item=[i,j/ps]}}}if(item){i=item[0];j=item[1];ps=series[i].datapoints.pointsize;return{datapoint:series[i].datapoints.points.slice(j*ps,(j+1)*ps),dataIndex:j,series:series[i],seriesIndex:i}}return null}function onMouseMove(e){if(options.grid.hoverable)triggerClickHoverEvent("plothover",e,function(s){return s["hoverable"]!=false})}function onMouseLeave(e){if(options.grid.hoverable)triggerClickHoverEvent("plothover",e,function(s){return false})}function onClick(e){triggerClickHoverEvent("plotclick",e,function(s){return s["clickable"]!=false})}function triggerClickHoverEvent(eventname,event,seriesFilter){var offset=eventHolder.offset(),canvasX=event.pageX-offset.left-plotOffset.left,canvasY=event.pageY-offset.top-plotOffset.top,pos=canvasToAxisCoords({left:canvasX,top:canvasY});pos.pageX=event.pageX;pos.pageY=event.pageY;var item=findNearbyItem(canvasX,canvasY,seriesFilter);if(item){item.pageX=parseInt(item.series.xaxis.p2c(item.datapoint[0])+offset.left+plotOffset.left,10);item.pageY=parseInt(item.series.yaxis.p2c(item.datapoint[1])+offset.top+plotOffset.top,10)}if(options.grid.autoHighlight){for(var i=0;i<highlights.length;++i){var h=highlights[i];if(h.auto==eventname&&!(item&&h.series==item.series&&h.point[0]==item.datapoint[0]&&h.point[1]==item.datapoint[1]))unhighlight(h.series,h.point)}if(item)highlight(item.series,item.datapoint,eventname)}placeholder.trigger(eventname,[pos,item])}function triggerRedrawOverlay(){var t=options.interaction.redrawOverlayInterval;if(t==-1){drawOverlay();return}if(!redrawTimeout)redrawTimeout=setTimeout(drawOverlay,t)}function drawOverlay(){redrawTimeout=null;octx.save();overlay.clear();octx.translate(plotOffset.left,plotOffset.top);var i,hi;for(i=0;i<highlights.length;++i){hi=highlights[i];if(hi.series.bars.show)drawBarHighlight(hi.series,hi.point);else drawPointHighlight(hi.series,hi.point)}octx.restore();executeHooks(hooks.drawOverlay,[octx])}function highlight(s,point,auto){if(typeof s=="number")s=series[s];if(typeof point=="number"){var ps=s.datapoints.pointsize;point=s.datapoints.points.slice(ps*point,ps*(point+1))}var i=indexOfHighlight(s,point);if(i==-1){highlights.push({series:s,point:point,auto:auto});triggerRedrawOverlay()}else if(!auto)highlights[i].auto=false}function unhighlight(s,point){if(s==null&&point==null){highlights=[];triggerRedrawOverlay();return}if(typeof s=="number")s=series[s];if(typeof point=="number"){var ps=s.datapoints.pointsize;point=s.datapoints.points.slice(ps*point,ps*(point+1))}var i=indexOfHighlight(s,point);if(i!=-1){highlights.splice(i,1);triggerRedrawOverlay()}}function indexOfHighlight(s,p){for(var i=0;i<highlights.length;++i){var h=highlights[i];if(h.series==s&&h.point[0]==p[0]&&h.point[1]==p[1])return i}return-1}function drawPointHighlight(series,point){var x=point[0],y=point[1],axisx=series.xaxis,axisy=series.yaxis,highlightColor=typeof series.highlightColor==="string"?series.highlightColor:$.color.parse(series.color).scale("a",.5).toString();if(x<axisx.min||x>axisx.max||y<axisy.min||y>axisy.max)return;var pointRadius=series.points.radius+series.points.lineWidth/2;octx.lineWidth=pointRadius;octx.strokeStyle=highlightColor;var radius=1.5*pointRadius;x=axisx.p2c(x);y=axisy.p2c(y);octx.beginPath();if(series.points.symbol=="circle")octx.arc(x,y,radius,0,2*Math.PI,false);else series.points.symbol(octx,x,y,radius,false);octx.closePath();octx.stroke()}function drawBarHighlight(series,point){var highlightColor=typeof series.highlightColor==="string"?series.highlightColor:$.color.parse(series.color).scale("a",.5).toString(),fillStyle=highlightColor,barLeft;switch(series.bars.align){case"left":barLeft=0;break;case"right":barLeft=-series.bars.barWidth;break;default:barLeft=-series.bars.barWidth/2}octx.lineWidth=series.bars.lineWidth;octx.strokeStyle=highlightColor;drawBar(point[0],point[1],point[2]||0,barLeft,barLeft+series.bars.barWidth,function(){return fillStyle},series.xaxis,series.yaxis,octx,series.bars.horizontal,series.bars.lineWidth)}function getColorOrGradient(spec,bottom,top,defaultColor){if(typeof spec=="string")return spec;else{var gradient=ctx.createLinearGradient(0,top,0,bottom);for(var i=0,l=spec.colors.length;i<l;++i){var c=spec.colors[i];if(typeof c!="string"){var co=$.color.parse(defaultColor);if(c.brightness!=null)co=co.scale("rgb",c.brightness);if(c.opacity!=null)co.a*=c.opacity;c=co.toString()}gradient.addColorStop(i/(l-1),c)}return gradient}}}$.plot=function(placeholder,data,options){var plot=new Plot($(placeholder),data,options,$.plot.plugins);return plot};$.plot.version="0.8.3";$.plot.plugins=[];$.fn.plot=function(data,options){return this.each(function(){$.plot(this,data,options)})};function floorInBase(n,base){return base*Math.floor(n/base)}})(jQuery);

/***/ }),

/***/ "./flot/jquery.flot.navigate.min.js":
/*!******************************************!*\
  !*** ./flot/jquery.flot.navigate.min.js ***!
  \******************************************/
/*! no static exports found */
/***/ (function(module, exports) {

/* Javascript plotting library for jQuery, version 0.8.3.

Copyright (c) 2007-2014 IOLA and Ole Laursen.
Licensed under the MIT license.

*/
(function(a){function e(h){var k,j=this,l=h.data||{};if(l.elem)j=h.dragTarget=l.elem,h.dragProxy=d.proxy||j,h.cursorOffsetX=l.pageX-l.left,h.cursorOffsetY=l.pageY-l.top,h.offsetX=h.pageX-h.cursorOffsetX,h.offsetY=h.pageY-h.cursorOffsetY;else if(d.dragging||l.which>0&&h.which!=l.which||a(h.target).is(l.not))return;switch(h.type){case"mousedown":return a.extend(l,a(j).offset(),{elem:j,target:h.target,pageX:h.pageX,pageY:h.pageY}),b.add(document,"mousemove mouseup",e,l),i(j,!1),d.dragging=null,!1;case!d.dragging&&"mousemove":if(g(h.pageX-l.pageX)+g(h.pageY-l.pageY)<l.distance)break;h.target=l.target,k=f(h,"dragstart",j),k!==!1&&(d.dragging=j,d.proxy=h.dragProxy=a(k||j)[0]);case"mousemove":if(d.dragging){if(k=f(h,"drag",j),c.drop&&(c.drop.allowed=k!==!1,c.drop.handler(h)),k!==!1)break;h.type="mouseup"}case"mouseup":b.remove(document,"mousemove mouseup",e),d.dragging&&(c.drop&&c.drop.handler(h),f(h,"dragend",j)),i(j,!0),d.dragging=d.proxy=l.elem=!1}return!0}function f(b,c,d){b.type=c;var e=a.event.dispatch.call(d,b);return e===!1?!1:e||b.result}function g(a){return Math.pow(a,2)}function h(){return d.dragging===!1}function i(a,b){a&&(a.unselectable=b?"off":"on",a.onselectstart=function(){return b},a.style&&(a.style.MozUserSelect=b?"":"none"))}a.fn.drag=function(a,b,c){return b&&this.bind("dragstart",a),c&&this.bind("dragend",c),a?this.bind("drag",b?b:a):this.trigger("drag")};var b=a.event,c=b.special,d=c.drag={not:":input",distance:0,which:1,dragging:!1,setup:function(c){c=a.extend({distance:d.distance,which:d.which,not:d.not},c||{}),c.distance=g(c.distance),b.add(this,"mousedown",e,c),this.attachEvent&&this.attachEvent("ondragstart",h)},teardown:function(){b.remove(this,"mousedown",e),this===d.dragging&&(d.dragging=d.proxy=!1),i(this,!0),this.detachEvent&&this.detachEvent("ondragstart",h)}};c.dragstart=c.dragend={setup:function(){},teardown:function(){}}})(jQuery);(function(d){function e(a){var b=a||window.event,c=[].slice.call(arguments,1),f=0,e=0,g=0,a=d.event.fix(b);a.type="mousewheel";b.wheelDelta&&(f=b.wheelDelta/120);b.detail&&(f=-b.detail/3);g=f;void 0!==b.axis&&b.axis===b.HORIZONTAL_AXIS&&(g=0,e=-1*f);void 0!==b.wheelDeltaY&&(g=b.wheelDeltaY/120);void 0!==b.wheelDeltaX&&(e=-1*b.wheelDeltaX/120);c.unshift(a,f,e,g);return(d.event.dispatch||d.event.handle).apply(this,c)}var c=["DOMMouseScroll","mousewheel"];if(d.event.fixHooks)for(var h=c.length;h;)d.event.fixHooks[c[--h]]=d.event.mouseHooks;d.event.special.mousewheel={setup:function(){if(this.addEventListener)for(var a=c.length;a;)this.addEventListener(c[--a],e,!1);else this.onmousewheel=e},teardown:function(){if(this.removeEventListener)for(var a=c.length;a;)this.removeEventListener(c[--a],e,!1);else this.onmousewheel=null}};d.fn.extend({mousewheel:function(a){return a?this.bind("mousewheel",a):this.trigger("mousewheel")},unmousewheel:function(a){return this.unbind("mousewheel",a)}})})(jQuery);(function($){var options={xaxis:{zoomRange:null,panRange:null},zoom:{interactive:false,trigger:"dblclick",amount:1.5},pan:{interactive:false,cursor:"move",frameRate:20}};function init(plot){function onZoomClick(e,zoomOut){var c=plot.offset();c.left=e.pageX-c.left;c.top=e.pageY-c.top;if(zoomOut)plot.zoomOut({center:c});else plot.zoom({center:c})}function onMouseWheel(e,delta){e.preventDefault();onZoomClick(e,delta<0);return false}var prevCursor="default",prevPageX=0,prevPageY=0,panTimeout=null;function onDragStart(e){if(e.which!=1)return false;var c=plot.getPlaceholder().css("cursor");if(c)prevCursor=c;plot.getPlaceholder().css("cursor",plot.getOptions().pan.cursor);prevPageX=e.pageX;prevPageY=e.pageY}function onDrag(e){var frameRate=plot.getOptions().pan.frameRate;if(panTimeout||!frameRate)return;panTimeout=setTimeout(function(){plot.pan({left:prevPageX-e.pageX,top:prevPageY-e.pageY});prevPageX=e.pageX;prevPageY=e.pageY;panTimeout=null},1/frameRate*1e3)}function onDragEnd(e){if(panTimeout){clearTimeout(panTimeout);panTimeout=null}plot.getPlaceholder().css("cursor",prevCursor);plot.pan({left:prevPageX-e.pageX,top:prevPageY-e.pageY})}function bindEvents(plot,eventHolder){var o=plot.getOptions();if(o.zoom.interactive){eventHolder[o.zoom.trigger](onZoomClick);eventHolder.mousewheel(onMouseWheel)}if(o.pan.interactive){eventHolder.bind("dragstart",{distance:10},onDragStart);eventHolder.bind("drag",onDrag);eventHolder.bind("dragend",onDragEnd)}}plot.zoomOut=function(args){if(!args)args={};if(!args.amount)args.amount=plot.getOptions().zoom.amount;args.amount=1/args.amount;plot.zoom(args)};plot.zoom=function(args){if(!args)args={};var c=args.center,amount=args.amount||plot.getOptions().zoom.amount,w=plot.width(),h=plot.height();if(!c)c={left:w/2,top:h/2};var xf=c.left/w,yf=c.top/h,minmax={x:{min:c.left-xf*w/amount,max:c.left+(1-xf)*w/amount},y:{min:c.top-yf*h/amount,max:c.top+(1-yf)*h/amount}};$.each(plot.getAxes(),function(_,axis){var opts=axis.options,min=minmax[axis.direction].min,max=minmax[axis.direction].max,zr=opts.zoomRange,pr=opts.panRange;if(zr===false)return;min=axis.c2p(min);max=axis.c2p(max);if(min>max){var tmp=min;min=max;max=tmp}if(pr){if(pr[0]!=null&&min<pr[0]){min=pr[0]}if(pr[1]!=null&&max>pr[1]){max=pr[1]}}var range=max-min;if(zr&&(zr[0]!=null&&range<zr[0]&&amount>1||zr[1]!=null&&range>zr[1]&&amount<1))return;opts.min=min;opts.max=max});plot.setupGrid();plot.draw();if(!args.preventEvent)plot.getPlaceholder().trigger("plotzoom",[plot,args])};plot.pan=function(args){var delta={x:+args.left,y:+args.top};if(isNaN(delta.x))delta.x=0;if(isNaN(delta.y))delta.y=0;$.each(plot.getAxes(),function(_,axis){var opts=axis.options,min,max,d=delta[axis.direction];min=axis.c2p(axis.p2c(axis.min)+d),max=axis.c2p(axis.p2c(axis.max)+d);var pr=opts.panRange;if(pr===false)return;if(pr){if(pr[0]!=null&&pr[0]>min){d=pr[0]-min;min+=d;max+=d}if(pr[1]!=null&&pr[1]<max){d=pr[1]-max;min+=d;max+=d}}opts.min=min;opts.max=max});plot.setupGrid();plot.draw();if(!args.preventEvent)plot.getPlaceholder().trigger("plotpan",[plot,args])};function shutdown(plot,eventHolder){eventHolder.unbind(plot.getOptions().zoom.trigger,onZoomClick);eventHolder.unbind("mousewheel",onMouseWheel);eventHolder.unbind("dragstart",onDragStart);eventHolder.unbind("drag",onDrag);eventHolder.unbind("dragend",onDragEnd);if(panTimeout)clearTimeout(panTimeout)}plot.hooks.bindEvents.push(bindEvents);plot.hooks.shutdown.push(shutdown)}$.plot.plugins.push({init:init,options:options,name:"navigate",version:"1.3"})})(jQuery);

/***/ }),

/***/ "./flot/jquery.flot.selection.min.js":
/*!*******************************************!*\
  !*** ./flot/jquery.flot.selection.min.js ***!
  \*******************************************/
/*! no static exports found */
/***/ (function(module, exports) {

/* Javascript plotting library for jQuery, version 0.8.3.

Copyright (c) 2007-2014 IOLA and Ole Laursen.
Licensed under the MIT license.

*/
(function($){function init(plot){var selection={first:{x:-1,y:-1},second:{x:-1,y:-1},show:false,active:false};var savedhandlers={};var mouseUpHandler=null;function onMouseMove(e){if(selection.active){updateSelection(e);plot.getPlaceholder().trigger("plotselecting",[getSelection()])}}function onMouseDown(e){if(e.which!=1)return;document.body.focus();if(document.onselectstart!==undefined&&savedhandlers.onselectstart==null){savedhandlers.onselectstart=document.onselectstart;document.onselectstart=function(){return false}}if(document.ondrag!==undefined&&savedhandlers.ondrag==null){savedhandlers.ondrag=document.ondrag;document.ondrag=function(){return false}}setSelectionPos(selection.first,e);selection.active=true;mouseUpHandler=function(e){onMouseUp(e)};$(document).one("mouseup",mouseUpHandler)}function onMouseUp(e){mouseUpHandler=null;if(document.onselectstart!==undefined)document.onselectstart=savedhandlers.onselectstart;if(document.ondrag!==undefined)document.ondrag=savedhandlers.ondrag;selection.active=false;updateSelection(e);if(selectionIsSane())triggerSelectedEvent();else{plot.getPlaceholder().trigger("plotunselected",[]);plot.getPlaceholder().trigger("plotselecting",[null])}return false}function getSelection(){if(!selectionIsSane())return null;if(!selection.show)return null;var r={},c1=selection.first,c2=selection.second;$.each(plot.getAxes(),function(name,axis){if(axis.used){var p1=axis.c2p(c1[axis.direction]),p2=axis.c2p(c2[axis.direction]);r[name]={from:Math.min(p1,p2),to:Math.max(p1,p2)}}});return r}function triggerSelectedEvent(){var r=getSelection();plot.getPlaceholder().trigger("plotselected",[r]);if(r.xaxis&&r.yaxis)plot.getPlaceholder().trigger("selected",[{x1:r.xaxis.from,y1:r.yaxis.from,x2:r.xaxis.to,y2:r.yaxis.to}])}function clamp(min,value,max){return value<min?min:value>max?max:value}function setSelectionPos(pos,e){var o=plot.getOptions();var offset=plot.getPlaceholder().offset();var plotOffset=plot.getPlotOffset();pos.x=clamp(0,e.pageX-offset.left-plotOffset.left,plot.width());pos.y=clamp(0,e.pageY-offset.top-plotOffset.top,plot.height());if(o.selection.mode=="y")pos.x=pos==selection.first?0:plot.width();if(o.selection.mode=="x")pos.y=pos==selection.first?0:plot.height()}function updateSelection(pos){if(pos.pageX==null)return;setSelectionPos(selection.second,pos);if(selectionIsSane()){selection.show=true;plot.triggerRedrawOverlay()}else clearSelection(true)}function clearSelection(preventEvent){if(selection.show){selection.show=false;plot.triggerRedrawOverlay();if(!preventEvent)plot.getPlaceholder().trigger("plotunselected",[])}}function extractRange(ranges,coord){var axis,from,to,key,axes=plot.getAxes();for(var k in axes){axis=axes[k];if(axis.direction==coord){key=coord+axis.n+"axis";if(!ranges[key]&&axis.n==1)key=coord+"axis";if(ranges[key]){from=ranges[key].from;to=ranges[key].to;break}}}if(!ranges[key]){axis=coord=="x"?plot.getXAxes()[0]:plot.getYAxes()[0];from=ranges[coord+"1"];to=ranges[coord+"2"]}if(from!=null&&to!=null&&from>to){var tmp=from;from=to;to=tmp}return{from:from,to:to,axis:axis}}function setSelection(ranges,preventEvent){var axis,range,o=plot.getOptions();if(o.selection.mode=="y"){selection.first.x=0;selection.second.x=plot.width()}else{range=extractRange(ranges,"x");selection.first.x=range.axis.p2c(range.from);selection.second.x=range.axis.p2c(range.to)}if(o.selection.mode=="x"){selection.first.y=0;selection.second.y=plot.height()}else{range=extractRange(ranges,"y");selection.first.y=range.axis.p2c(range.from);selection.second.y=range.axis.p2c(range.to)}selection.show=true;plot.triggerRedrawOverlay();if(!preventEvent&&selectionIsSane())triggerSelectedEvent()}function selectionIsSane(){var minSize=plot.getOptions().selection.minSize;return Math.abs(selection.second.x-selection.first.x)>=minSize&&Math.abs(selection.second.y-selection.first.y)>=minSize}plot.clearSelection=clearSelection;plot.setSelection=setSelection;plot.getSelection=getSelection;plot.hooks.bindEvents.push(function(plot,eventHolder){var o=plot.getOptions();if(o.selection.mode!=null){eventHolder.mousemove(onMouseMove);eventHolder.mousedown(onMouseDown)}});plot.hooks.drawOverlay.push(function(plot,ctx){if(selection.show&&selectionIsSane()){var plotOffset=plot.getPlotOffset();var o=plot.getOptions();ctx.save();ctx.translate(plotOffset.left,plotOffset.top);var c=$.color.parse(o.selection.color);ctx.strokeStyle=c.scale("a",.8).toString();ctx.lineWidth=1;ctx.lineJoin=o.selection.shape;ctx.fillStyle=c.scale("a",.4).toString();var x=Math.min(selection.first.x,selection.second.x)+.5,y=Math.min(selection.first.y,selection.second.y)+.5,w=Math.abs(selection.second.x-selection.first.x)-1,h=Math.abs(selection.second.y-selection.first.y)-1;ctx.fillRect(x,y,w,h);ctx.strokeRect(x,y,w,h);ctx.restore()}});plot.hooks.shutdown.push(function(plot,eventHolder){eventHolder.unbind("mousemove",onMouseMove);eventHolder.unbind("mousedown",onMouseDown);if(mouseUpHandler)$(document).unbind("mouseup",mouseUpHandler)})}$.plot.plugins.push({init:init,options:{selection:{mode:null,color:"#e8cfac",shape:"round",minSize:5}},name:"selection",version:"1.1"})})(jQuery);

/***/ }),

/***/ "./flot/jquery.flot.time.min.js":
/*!**************************************!*\
  !*** ./flot/jquery.flot.time.min.js ***!
  \**************************************/
/*! no static exports found */
/***/ (function(module, exports) {

/* Javascript plotting library for jQuery, version 0.8.3.

Copyright (c) 2007-2014 IOLA and Ole Laursen.
Licensed under the MIT license.

*/
(function($){var options={xaxis:{timezone:null,timeformat:null,twelveHourClock:false,monthNames:null}};function floorInBase(n,base){return base*Math.floor(n/base)}function formatDate(d,fmt,monthNames,dayNames){if(typeof d.strftime=="function"){return d.strftime(fmt)}var leftPad=function(n,pad){n=""+n;pad=""+(pad==null?"0":pad);return n.length==1?pad+n:n};var r=[];var escape=false;var hours=d.getHours();var isAM=hours<12;if(monthNames==null){monthNames=["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"]}if(dayNames==null){dayNames=["Sun","Mon","Tue","Wed","Thu","Fri","Sat"]}var hours12;if(hours>12){hours12=hours-12}else if(hours==0){hours12=12}else{hours12=hours}for(var i=0;i<fmt.length;++i){var c=fmt.charAt(i);if(escape){switch(c){case"a":c=""+dayNames[d.getDay()];break;case"b":c=""+monthNames[d.getMonth()];break;case"d":c=leftPad(d.getDate());break;case"e":c=leftPad(d.getDate()," ");break;case"h":case"H":c=leftPad(hours);break;case"I":c=leftPad(hours12);break;case"l":c=leftPad(hours12," ");break;case"m":c=leftPad(d.getMonth()+1);break;case"M":c=leftPad(d.getMinutes());break;case"q":c=""+(Math.floor(d.getMonth()/3)+1);break;case"S":c=leftPad(d.getSeconds());break;case"y":c=leftPad(d.getFullYear()%100);break;case"Y":c=""+d.getFullYear();break;case"p":c=isAM?""+"am":""+"pm";break;case"P":c=isAM?""+"AM":""+"PM";break;case"w":c=""+d.getDay();break}r.push(c);escape=false}else{if(c=="%"){escape=true}else{r.push(c)}}}return r.join("")}function makeUtcWrapper(d){function addProxyMethod(sourceObj,sourceMethod,targetObj,targetMethod){sourceObj[sourceMethod]=function(){return targetObj[targetMethod].apply(targetObj,arguments)}}var utc={date:d};if(d.strftime!=undefined){addProxyMethod(utc,"strftime",d,"strftime")}addProxyMethod(utc,"getTime",d,"getTime");addProxyMethod(utc,"setTime",d,"setTime");var props=["Date","Day","FullYear","Hours","Milliseconds","Minutes","Month","Seconds"];for(var p=0;p<props.length;p++){addProxyMethod(utc,"get"+props[p],d,"getUTC"+props[p]);addProxyMethod(utc,"set"+props[p],d,"setUTC"+props[p])}return utc}function dateGenerator(ts,opts){if(opts.timezone=="browser"){return new Date(ts)}else if(!opts.timezone||opts.timezone=="utc"){return makeUtcWrapper(new Date(ts))}else if(typeof timezoneJS!="undefined"&&typeof timezoneJS.Date!="undefined"){var d=new timezoneJS.Date;d.setTimezone(opts.timezone);d.setTime(ts);return d}else{return makeUtcWrapper(new Date(ts))}}var timeUnitSize={second:1e3,minute:60*1e3,hour:60*60*1e3,day:24*60*60*1e3,month:30*24*60*60*1e3,quarter:3*30*24*60*60*1e3,year:365.2425*24*60*60*1e3};var baseSpec=[[1,"second"],[2,"second"],[5,"second"],[10,"second"],[30,"second"],[1,"minute"],[2,"minute"],[5,"minute"],[10,"minute"],[30,"minute"],[1,"hour"],[2,"hour"],[4,"hour"],[8,"hour"],[12,"hour"],[1,"day"],[2,"day"],[3,"day"],[.25,"month"],[.5,"month"],[1,"month"],[2,"month"]];var specMonths=baseSpec.concat([[3,"month"],[6,"month"],[1,"year"]]);var specQuarters=baseSpec.concat([[1,"quarter"],[2,"quarter"],[1,"year"]]);function init(plot){plot.hooks.processOptions.push(function(plot,options){$.each(plot.getAxes(),function(axisName,axis){var opts=axis.options;if(opts.mode=="time"){axis.tickGenerator=function(axis){var ticks=[];var d=dateGenerator(axis.min,opts);var minSize=0;var spec=opts.tickSize&&opts.tickSize[1]==="quarter"||opts.minTickSize&&opts.minTickSize[1]==="quarter"?specQuarters:specMonths;if(opts.minTickSize!=null){if(typeof opts.tickSize=="number"){minSize=opts.tickSize}else{minSize=opts.minTickSize[0]*timeUnitSize[opts.minTickSize[1]]}}for(var i=0;i<spec.length-1;++i){if(axis.delta<(spec[i][0]*timeUnitSize[spec[i][1]]+spec[i+1][0]*timeUnitSize[spec[i+1][1]])/2&&spec[i][0]*timeUnitSize[spec[i][1]]>=minSize){break}}var size=spec[i][0];var unit=spec[i][1];if(unit=="year"){if(opts.minTickSize!=null&&opts.minTickSize[1]=="year"){size=Math.floor(opts.minTickSize[0])}else{var magn=Math.pow(10,Math.floor(Math.log(axis.delta/timeUnitSize.year)/Math.LN10));var norm=axis.delta/timeUnitSize.year/magn;if(norm<1.5){size=1}else if(norm<3){size=2}else if(norm<7.5){size=5}else{size=10}size*=magn}if(size<1){size=1}}axis.tickSize=opts.tickSize||[size,unit];var tickSize=axis.tickSize[0];unit=axis.tickSize[1];var step=tickSize*timeUnitSize[unit];if(unit=="second"){d.setSeconds(floorInBase(d.getSeconds(),tickSize))}else if(unit=="minute"){d.setMinutes(floorInBase(d.getMinutes(),tickSize))}else if(unit=="hour"){d.setHours(floorInBase(d.getHours(),tickSize))}else if(unit=="month"){d.setMonth(floorInBase(d.getMonth(),tickSize))}else if(unit=="quarter"){d.setMonth(3*floorInBase(d.getMonth()/3,tickSize))}else if(unit=="year"){d.setFullYear(floorInBase(d.getFullYear(),tickSize))}d.setMilliseconds(0);if(step>=timeUnitSize.minute){d.setSeconds(0)}if(step>=timeUnitSize.hour){d.setMinutes(0)}if(step>=timeUnitSize.day){d.setHours(0)}if(step>=timeUnitSize.day*4){d.setDate(1)}if(step>=timeUnitSize.month*2){d.setMonth(floorInBase(d.getMonth(),3))}if(step>=timeUnitSize.quarter*2){d.setMonth(floorInBase(d.getMonth(),6))}if(step>=timeUnitSize.year){d.setMonth(0)}var carry=0;var v=Number.NaN;var prev;do{prev=v;v=d.getTime();ticks.push(v);if(unit=="month"||unit=="quarter"){if(tickSize<1){d.setDate(1);var start=d.getTime();d.setMonth(d.getMonth()+(unit=="quarter"?3:1));var end=d.getTime();d.setTime(v+carry*timeUnitSize.hour+(end-start)*tickSize);carry=d.getHours();d.setHours(0)}else{d.setMonth(d.getMonth()+tickSize*(unit=="quarter"?3:1))}}else if(unit=="year"){d.setFullYear(d.getFullYear()+tickSize)}else{d.setTime(v+step)}}while(v<axis.max&&v!=prev);return ticks};axis.tickFormatter=function(v,axis){var d=dateGenerator(v,axis.options);if(opts.timeformat!=null){return formatDate(d,opts.timeformat,opts.monthNames,opts.dayNames)}var useQuarters=axis.options.tickSize&&axis.options.tickSize[1]=="quarter"||axis.options.minTickSize&&axis.options.minTickSize[1]=="quarter";var t=axis.tickSize[0]*timeUnitSize[axis.tickSize[1]];var span=axis.max-axis.min;var suffix=opts.twelveHourClock?" %p":"";var hourCode=opts.twelveHourClock?"%I":"%H";var fmt;if(t<timeUnitSize.minute){fmt=hourCode+":%M:%S"+suffix}else if(t<timeUnitSize.day){if(span<2*timeUnitSize.day){fmt=hourCode+":%M"+suffix}else{fmt="%b %d "+hourCode+":%M"+suffix}}else if(t<timeUnitSize.month){fmt="%b %d"}else if(useQuarters&&t<timeUnitSize.quarter||!useQuarters&&t<timeUnitSize.year){if(span<timeUnitSize.year){fmt="%b"}else{fmt="%b %Y"}}else if(useQuarters&&t<timeUnitSize.year){if(span<timeUnitSize.year){fmt="Q%q"}else{fmt="Q%q %Y"}}else{fmt="%Y"}var rt=formatDate(d,fmt,opts.monthNames,opts.dayNames);return rt}}})})}$.plot.plugins.push({init:init,options:options,name:"time",version:"1.0"});$.plot.formatDate=formatDate;$.plot.dateGenerator=dateGenerator})(jQuery);

/***/ }),

/***/ "moment":
/*!*************************!*\
  !*** external "moment" ***!
  \*************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = moment;

/***/ }),

/***/ "react":
/*!************************!*\
  !*** external "React" ***!
  \************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = React;

/***/ }),

/***/ "react-dom":
/*!***************************!*\
  !*** external "ReactDOM" ***!
  \***************************/
/*! no static exports found */
/***/ (function(module, exports) {

module.exports = ReactDOM;

/***/ })

/******/ });
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly8vd2VicGFjay9ib290c3RyYXAiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL0BncGEtZ2Vtc3RvbmUvaGVscGVyLWZ1bmN0aW9ucy9saWIvQ3JlYXRlR3VpZC5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvYmVybmVzdC9Tb3VyY2UvUmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvQGdwYS1nZW1zdG9uZS9oZWxwZXItZnVuY3Rpb25zL2xpYi9HZXROb2RlU2l6ZS5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvYmVybmVzdC9Tb3VyY2UvUmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvQGdwYS1nZW1zdG9uZS9oZWxwZXItZnVuY3Rpb25zL2xpYi9HZXRUZXh0SGVpZ2h0LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9iZXJuZXN0L1NvdXJjZS9SZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9AZ3BhLWdlbXN0b25lL2hlbHBlci1mdW5jdGlvbnMvbGliL0dldFRleHRXaWR0aC5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvYmVybmVzdC9Tb3VyY2UvUmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvQGdwYS1nZW1zdG9uZS9oZWxwZXItZnVuY3Rpb25zL2xpYi9Jc0ludGVnZXIuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL0BncGEtZ2Vtc3RvbmUvaGVscGVyLWZ1bmN0aW9ucy9saWIvSXNOdW1iZXIuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL0BncGEtZ2Vtc3RvbmUvaGVscGVyLWZ1bmN0aW9ucy9saWIvUmFuZG9tQ29sb3IuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL0BncGEtZ2Vtc3RvbmUvaGVscGVyLWZ1bmN0aW9ucy9saWIvaW5kZXguanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2NyZWF0ZS1yZWFjdC1jbGFzcy9mYWN0b3J5LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9iZXJuZXN0L1NvdXJjZS9SZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9jcmVhdGUtcmVhY3QtY2xhc3MvaW5kZXguanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3JlYWN0LWRhdGV0aW1lL2Nzcy9yZWFjdC1kYXRldGltZS5jc3MiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2Nzcy1sb2FkZXIvZGlzdC9ydW50aW1lL2FwaS5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvYmVybmVzdC9Tb3VyY2UvUmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvZGVjb2RlLXVyaS1jb21wb25lbnQvaW5kZXguanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2ZianMvbGliL2VtcHR5RnVuY3Rpb24uanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2ZianMvbGliL2VtcHR5T2JqZWN0LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9iZXJuZXN0L1NvdXJjZS9SZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9mYmpzL2xpYi9pbnZhcmlhbnQuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2ZianMvbGliL3dhcm5pbmcuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2hpc3RvcnkvRE9NVXRpbHMuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2hpc3RvcnkvTG9jYXRpb25VdGlscy5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvYmVybmVzdC9Tb3VyY2UvUmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvaGlzdG9yeS9QYXRoVXRpbHMuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2hpc3RvcnkvY3JlYXRlQnJvd3Nlckhpc3RvcnkuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2hpc3RvcnkvY3JlYXRlVHJhbnNpdGlvbk1hbmFnZXIuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2ludmFyaWFudC9icm93c2VyLmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9iZXJuZXN0L1NvdXJjZS9SZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9vYmplY3QtYXNzaWduL2luZGV4LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9iZXJuZXN0L1NvdXJjZS9SZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9wcm9wLXR5cGVzL2NoZWNrUHJvcFR5cGVzLmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9iZXJuZXN0L1NvdXJjZS9SZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9wcm9wLXR5cGVzL2ZhY3RvcnlXaXRoVHlwZUNoZWNrZXJzLmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9iZXJuZXN0L1NvdXJjZS9SZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9wcm9wLXR5cGVzL2luZGV4LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9iZXJuZXN0L1NvdXJjZS9SZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9wcm9wLXR5cGVzL2xpYi9SZWFjdFByb3BUeXBlc1NlY3JldC5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvYmVybmVzdC9Tb3VyY2UvUmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvcXVlcnktc3RyaW5nL2luZGV4LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9iZXJuZXN0L1NvdXJjZS9SZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9yZWFjdC1kYXRldGltZS9EYXRlVGltZS5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvYmVybmVzdC9Tb3VyY2UvUmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvcmVhY3QtZGF0ZXRpbWUvY3NzL3JlYWN0LWRhdGV0aW1lLmNzcz8wMGE4Iiwid2VicGFjazovLy9DOi9Vc2Vycy9iZXJuZXN0L1NvdXJjZS9SZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9yZWFjdC1kYXRldGltZS9ub2RlX21vZHVsZXMvb2JqZWN0LWFzc2lnbi9pbmRleC5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvYmVybmVzdC9Tb3VyY2UvUmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvcmVhY3QtZGF0ZXRpbWUvc3JjL0NhbGVuZGFyQ29udGFpbmVyLmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9iZXJuZXN0L1NvdXJjZS9SZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9yZWFjdC1kYXRldGltZS9zcmMvRGF5c1ZpZXcuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3JlYWN0LWRhdGV0aW1lL3NyYy9Nb250aHNWaWV3LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9iZXJuZXN0L1NvdXJjZS9SZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9yZWFjdC1kYXRldGltZS9zcmMvVGltZVZpZXcuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3JlYWN0LWRhdGV0aW1lL3NyYy9ZZWFyc1ZpZXcuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3JlYWN0LW9uY2xpY2tvdXRzaWRlL2Rpc3QvcmVhY3Qtb25jbGlja291dHNpZGUuZXMuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3Jlc29sdmUtcGF0aG5hbWUvaW5kZXguanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2Jlcm5lc3QvU291cmNlL1JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3N0cmljdC11cmktZW5jb2RlL2luZGV4LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9iZXJuZXN0L1NvdXJjZS9SZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9zdHlsZS1sb2FkZXIvbGliL2FkZFN0eWxlcy5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvYmVybmVzdC9Tb3VyY2UvUmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvc3R5bGUtbG9hZGVyL2xpYi91cmxzLmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9iZXJuZXN0L1NvdXJjZS9SZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy92YWx1ZS1lcXVhbC9pbmRleC5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvYmVybmVzdC9Tb3VyY2UvUmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvd2FybmluZy9icm93c2VyLmpzIiwid2VicGFjazovLy8uL1RTL1NlcnZpY2VzL1BlcmlvZGljRGF0YURpc3BsYXkudHMiLCJ3ZWJwYWNrOi8vLy4vVFMvU2VydmljZXMvVHJlbmRpbmdEYXRhRGlzcGxheS50cyIsIndlYnBhY2s6Ly8vLi9UU1gvRGF0ZVRpbWVSYW5nZVBpY2tlci50c3giLCJ3ZWJwYWNrOi8vLy4vVFNYL01lYXN1cmVtZW50SW5wdXQudHN4Iiwid2VicGFjazovLy8uL1RTWC9NZXRlcklucHV0LnRzeCIsIndlYnBhY2s6Ly8vLi9UU1gvVHJlbmRpbmdDaGFydC50c3giLCJ3ZWJwYWNrOi8vLy4vVFNYL1RyZW5kaW5nRGF0YURpc3BsYXkudHN4Iiwid2VicGFjazovLy8uL2Zsb3QvanF1ZXJ5LmZsb3QuYXhpc2xhYmVscy5qcyIsIndlYnBhY2s6Ly8vLi9mbG90L2pxdWVyeS5mbG90LmNyb3NzaGFpci5taW4uanMiLCJ3ZWJwYWNrOi8vLy4vZmxvdC9qcXVlcnkuZmxvdC5taW4uanMiLCJ3ZWJwYWNrOi8vLy4vZmxvdC9qcXVlcnkuZmxvdC5uYXZpZ2F0ZS5taW4uanMiLCJ3ZWJwYWNrOi8vLy4vZmxvdC9qcXVlcnkuZmxvdC5zZWxlY3Rpb24ubWluLmpzIiwid2VicGFjazovLy8uL2Zsb3QvanF1ZXJ5LmZsb3QudGltZS5taW4uanMiLCJ3ZWJwYWNrOi8vL2V4dGVybmFsIFwibW9tZW50XCIiLCJ3ZWJwYWNrOi8vL2V4dGVybmFsIFwiUmVhY3RcIiIsIndlYnBhY2s6Ly8vZXh0ZXJuYWwgXCJSZWFjdERPTVwiIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7UUFBQTtRQUNBOztRQUVBO1FBQ0E7O1FBRUE7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7O1FBRUE7UUFDQTs7UUFFQTtRQUNBOztRQUVBO1FBQ0E7UUFDQTs7O1FBR0E7UUFDQTs7UUFFQTtRQUNBOztRQUVBO1FBQ0E7UUFDQTtRQUNBLDBDQUEwQyxnQ0FBZ0M7UUFDMUU7UUFDQTs7UUFFQTtRQUNBO1FBQ0E7UUFDQSx3REFBd0Qsa0JBQWtCO1FBQzFFO1FBQ0EsaURBQWlELGNBQWM7UUFDL0Q7O1FBRUE7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBLHlDQUF5QyxpQ0FBaUM7UUFDMUUsZ0hBQWdILG1CQUFtQixFQUFFO1FBQ3JJO1FBQ0E7O1FBRUE7UUFDQTtRQUNBO1FBQ0EsMkJBQTJCLDBCQUEwQixFQUFFO1FBQ3ZELGlDQUFpQyxlQUFlO1FBQ2hEO1FBQ0E7UUFDQTs7UUFFQTtRQUNBLHNEQUFzRCwrREFBK0Q7O1FBRXJIO1FBQ0E7OztRQUdBO1FBQ0E7Ozs7Ozs7Ozs7Ozs7QUNsRmE7QUFDYjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLGlGQUFpRjtBQUNqRjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLDhDQUE4QyxjQUFjO0FBQzVEO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLEtBQUs7QUFDTDtBQUNBOzs7Ozs7Ozs7Ozs7O0FDckNhO0FBQ2I7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxpRkFBaUY7QUFDakY7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLDhDQUE4QyxjQUFjO0FBQzVEO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOzs7Ozs7Ozs7Ozs7O0FDN0NhO0FBQ2I7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxpRkFBaUY7QUFDakY7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLDhDQUE4QyxjQUFjO0FBQzVEO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOzs7Ozs7Ozs7Ozs7O0FDN0NhO0FBQ2I7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxpRkFBaUY7QUFDakY7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLDhDQUE4QyxjQUFjO0FBQzVEO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtB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QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsYUFBYSxRQUFRO0FBQ3JCLGNBQWM7QUFDZDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxVQUFVO0FBQ1YsNkJBQTZCO0FBQzdCLFFBQVE7QUFDUjtBQUNBO0FBQ0E7QUFDQTtBQUNBLCtCQUErQixLQUFLO0FBQ3BDO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxTQUFTO0FBQ1QsNEJBQTRCO0FBQzVCLE9BQU87QUFDUDtBQUNBO0FBQ0E7O0FBRUE7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsS0FBSztBQUNMO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQSxRQUFRLElBQXFDO0FBQzdDO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxTQUFTLFVBQVUsS0FBcUM7QUFDeEQ7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsT0FBTztBQUNQO0FBQ0E7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxxQkFBcUIsc0JBQXNCO0FBQzNDO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBLE1BQU0sS0FBcUMsMEZBQTBGLFNBQU07QUFDM0k7QUFDQTs7QUFFQTtBQUNBO0FBQ0EscUJBQXFCLDJCQUEyQjtBQUNoRDtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQSxNQUFNLEtBQXFDLDhGQUE4RixTQUFNO0FBQy9JO0FBQ0E7O0FBRUEsbUJBQW1CLGdDQUFnQztBQUNuRDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQSxxQkFBcUIsZ0NBQWdDO0FBQ3JEO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSw2QkFBNkI7QUFDN0I7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxXQUFXO0FBQ1g7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsT0FBTztBQUNQO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBOzs7Ozs7Ozs7Ozs7QUM3aEJBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQSxJQUFJLElBQXFD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O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBLEdBQUc7QUFDSDtBQUNBLEdBQUc7QUFDSDtBQUNBOztBQUVBO0FBQ0EsRUFBRTs7QUFFRjtBQUNBO0FBQ0E7QUFDQTtBQUNBLElBQUk7QUFDSjtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBLElBQUk7QUFDSjtBQUNBLElBQUk7QUFDSjtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLElBQUk7QUFDSjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLEdBQUc7QUFDSDtBQUNBLEVBQUU7O0FBRUY7QUFDQTtBQUNBO0FBQ0EsYUFBYTtBQUNiOztBQUVBO0FBQ0E7QUFDQTtBQUNBLEdBQUc7QUFDSDtBQUNBOztBQUVBO0FBQ0E7QUFDQSxHQUFHO0FBQ0gsRUFBRTs7QUFFRjtBQUNBO0FBQ0E7QUFDQTtBQUNBLEVBQUU7O0FBRUY7QUFDQTtBQUNBO0FBQ0E7QUFDQSxnQkFBZ0Isb0JBQW9CO0FBQ3BDO0FBQ0EsRUFBRTs7QUFFRjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsSUFBSTtBQUNKO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7O0FBRUE7QUFDQSxrQkFBa0I7QUFDbEI7QUFDQTs7QUFFQTs7QUFFQTtBQUNBO0FBQ0EsRUFBRTs7QUFFRjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQSxRQUFRLG9DQUFvQztBQUM1QztBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQSxJQUFJO0FBQ0o7QUFDQTtBQUNBLEVBQUU7O0FBRUY7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQSxHQUFHO0FBQ0g7QUFDQTtBQUNBO0FBQ0EsR0FBRztBQUNIO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsSUFBSTtBQUNKLEdBQUc7QUFDSDtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBLEVBQUU7O0FBRUY7QUFDQTtBQUNBLGtCQUFrQixhQUFhO0FBQy9CO0FBQ0EsSUFBSTtBQUNKO0FBQ0EsRUFBRTs7QUFFRjtBQUNBLGlCQUFpQixjQUFjO0FBQy9CO0FBQ0EsR0FBRztBQUNILEVBQUU7O0FBRUY7QUFDQTtBQUNBLGtCQUFrQixjQUFjO0FBQ2hDO0FBQ0EsSUFBSTtBQUNKO0FBQ0EsRUFBRTs7QUFFRjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLEVBQUU7O0FBRUY7QUFDQTtBQUNBO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBLFlBQVk7QUFDWjs7QUFFQTtBQUNBO0FBQ0EsR0FBRztBQUNIO0FBQ0E7QUFDQSxHQUFHO0FBQ0g7QUFDQTtBQUNBLEdBQUc7O0FBRUg7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLElBQUk7QUFDSjtBQUNBLDZDQUE2QyxXQUFXO0FBQ3hELElBQUk7QUFDSixzREFBc0QsV0FBVztBQUNqRTtBQUNBLEdBQUc7QUFDSDtBQUNBOztBQUVBO0FBQ0E7O0FBRUEsc0NBQXNDLHVCQUF1QjtBQUM3RDtBQUNBLEtBQUssb0NBQW9DO0FBQ3pDLDZDQUE2Qyw2R0FBNkc7QUFDMUo7QUFDQTtBQUNBO0FBQ0EsQ0FBQzs7QUFFRDtBQUNBO0FBQ0E7QUFDQSxlQUFlO0FBQ2Y7QUFDQSx1QkFBdUI7QUFDdkIsc0JBQXNCO0FBQ3RCLHdCQUF3QjtBQUN4QixnQ0FBZ0M7QUFDaEM7QUFDQSxvQkFBb0I7QUFDcEI7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7Ozs7Ozs7Ozs7Ozs7QUN2ZEEsY0FBYyxtQkFBTyxDQUFDLDhKQUFxRDs7QUFFM0UsNENBQTRDLFFBQVM7O0FBRXJEO0FBQ0E7Ozs7QUFJQSxlQUFlOztBQUVmO0FBQ0E7O0FBRUEsYUFBYSxtQkFBTyxDQUFDLDZGQUFzQzs7QUFFM0Q7O0FBRUEsR0FBRyxLQUFVLEVBQUUsRTs7Ozs7Ozs7Ozs7O0FDbkJGO0FBQ2I7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0EsRUFBRTtBQUNGOztBQUVBO0FBQ0E7QUFDQTtBQUNBOztBQUVBLGdCQUFnQixzQkFBc0I7QUFDdEM7QUFDQTs7QUFFQSxpQkFBaUIsaUJBQWlCO0FBQ2xDO0FBQ0E7QUFDQTs7QUFFQTtBQUNBOzs7Ozs7Ozs7Ozs7O0FDdENhOztBQUViLFlBQVksbUJBQU8sQ0FBQyxvQkFBTztBQUMzQixlQUFlLG1CQUFPLENBQUMsMEVBQW9CO0FBQzNDLFlBQVksbUJBQU8sQ0FBQyxxRUFBWTtBQUNoQyxjQUFjLG1CQUFPLENBQUMseUVBQWM7QUFDcEMsYUFBYSxtQkFBTyxDQUFDLHVFQUFhO0FBQ2xDLFlBQVksbUJBQU8sQ0FBQyxxRUFBWTtBQUNoQzs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBLENBQUM7O0FBRUQ7Ozs7Ozs7Ozs7Ozs7QUN2QmE7O0FBRWIsWUFBWSxtQkFBTyxDQUFDLG9CQUFPO0FBQzNCLGVBQWUsbUJBQU8sQ0FBQywwRUFBb0I7QUFDM0MsVUFBVSxtQkFBTyxDQUFDLHNCQUFRO0FBQzFCLGtCQUFrQixtQkFBTyxDQUFDLHFHQUFzQjtBQUNoRDs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBLGlDQUFpQyxZQUFZO0FBQzdDLCtCQUErQixXQUFXO0FBQzFDLGdDQUFnQyxpRkFBaUYsZ0NBQWdDO0FBQ2pKLGdDQUFnQyxvSUFBb0k7QUFDcEssZ0NBQWdDLDRFQUE0RSxnQ0FBZ0M7QUFDNUk7QUFDQSwrQkFBK0IsVUFBVSw0REFBNEQsbUNBQW1DLG9DQUFvQyxRQUFRLEVBQUU7QUFDdEw7QUFDQSxpQ0FBaUMsWUFBWTtBQUM3Qzs7QUFFQTtBQUNBOztBQUVBLHFDQUFxQyx1QkFBdUI7QUFDNUQsa0NBQWtDO0FBQ2xDO0FBQ0EsRUFBRTs7QUFFRjtBQUNBO0FBQ0E7QUFDQSxhQUFhLE1BQU07QUFDbkI7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBLEdBQUc7O0FBRUg7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBOztBQUVBOztBQUVBO0FBQ0EsMkNBQTJDLGdDQUFnQztBQUMzRTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTs7QUFFQTs7QUFFQSx1Q0FBdUMsV0FBVztBQUNsRCwrQkFBK0I7QUFDL0IsK0JBQStCLGlGQUFpRjtBQUNoSDtBQUNBO0FBQ0EsRUFBRTs7QUFFRjtBQUNBO0FBQ0EsRUFBRTs7QUFFRjtBQUNBO0FBQ0E7QUFDQSxDQUFDOztBQUVEOzs7Ozs7Ozs7Ozs7O0FDL0lhOztBQUViLFlBQVksbUJBQU8sQ0FBQyxvQkFBTztBQUMzQixlQUFlLG1CQUFPLENBQUMsMEVBQW9CO0FBQzNDLGtCQUFrQixtQkFBTyxDQUFDLHFHQUFzQjtBQUNoRDs7QUFFQTtBQUNBO0FBQ0EscUNBQXFDLHlCQUF5QjtBQUM5RCxpQ0FBaUMsV0FBVyxpQ0FBaUMsOEJBQThCO0FBQzNHLCtCQUErQixtRkFBbUYsZ0NBQWdDO0FBQ2xKLCtCQUErQixxSUFBcUk7QUFDcEssK0JBQStCLDhFQUE4RSxnQ0FBZ0M7QUFDN0k7QUFDQSxpQ0FBaUMsZ0JBQWdCLGdDQUFnQyxXQUFXO0FBQzVGO0FBQ0EsRUFBRTs7QUFFRjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQSxxQ0FBcUMsNkNBQTZDOztBQUVsRjtBQUNBLDZCQUE2QiwwQkFBMEI7QUFDdkQ7QUFDQSxJQUFJOztBQUVKO0FBQ0E7QUFDQTtBQUNBLElBQUk7O0FBRUo7O0FBRUE7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBOztBQUVBO0FBQ0EsMENBQTBDLGlDQUFpQztBQUMzRTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBLENBQUM7O0FBRUQ7QUFDQTtBQUNBOztBQUVBOzs7Ozs7Ozs7Ozs7O0FDMUdhOztBQUViLFlBQVksbUJBQU8sQ0FBQyxvQkFBTztBQUMzQixlQUFlLG1CQUFPLENBQUMsMEVBQW9CO0FBQzNDLFVBQVUsbUJBQU8sQ0FBQyw0RkFBZTtBQUNqQyxrQkFBa0IsbUJBQU8sQ0FBQyxxR0FBc0I7QUFDaEQ7O0FBRUE7QUFDQTtBQUNBO0FBQ0EsRUFBRTs7QUFFRjtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLElBQUk7QUFDSjtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLEVBQUU7O0FBRUY7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLHNDQUFzQyxxQ0FBcUM7QUFDM0UsaUNBQWlDLHNMQUFzTDtBQUN2TixnQ0FBZ0Msa0NBQWtDO0FBQ2xFLGlDQUFpQyxzTEFBc0w7QUFDdk47QUFDQTtBQUNBO0FBQ0EsRUFBRTs7QUFFRjtBQUNBLHFDQUFxQywwQ0FBMEM7QUFDL0UsZ0NBQWdDLHFNQUFxTTtBQUNyTywrQkFBK0IsaURBQWlEO0FBQ2hGLGdDQUFnQyxxTUFBcU07QUFDck87QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQSwrQ0FBK0MsaUVBQWlFO0FBQ2hIO0FBQ0EsR0FBRzs7QUFFSDtBQUNBO0FBQ0E7O0FBRUE7QUFDQSw4Q0FBOEMsZ0RBQWdEO0FBQzlGO0FBQ0EsZ0NBQWdDLDZDQUE2QztBQUM3RSxtQ0FBbUMsMkVBQTJFO0FBQzlHO0FBQ0E7QUFDQTs7QUFFQSxxQ0FBcUMsdUJBQXVCO0FBQzVELGtDQUFrQztBQUNsQztBQUNBLGtDQUFrQyxVQUFVLDhCQUE4Qiw4QkFBOEI7QUFDeEcsaUNBQWlDLDJCQUEyQjtBQUM1RDtBQUNBO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsSUFBSTtBQUNKO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsSUFBSTtBQUNKO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsSUFBSTtBQUNKO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxHQUFHO0FBQ0g7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsbUJBQW1CLHNCQUFzQjtBQUN6QztBQUNBLEVBQUU7O0FBRUY7QUFDQTtBQUNBOztBQUVBO0FBQ0EsdUNBQXVDLFdBQVcsOEJBQThCO0FBQ2hGLDhCQUE4Qiw2RUFBNkU7QUFDM0c7QUFDQSxFQUFFOztBQUVGO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQSxLQUFLO0FBQ0wsSUFBSTs7QUFFSjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBLEVBQUU7O0FBRUY7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLEVBQUU7O0FBRUYsa0NBQWtDO0FBQ2xDO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsRUFBRTs7QUFFRjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsRUFBRTs7QUFFRjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsRUFBRTs7QUFFRjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsRUFBRTs7QUFFRjtBQUNBO0FBQ0E7QUFDQSxDQUFDOztBQUVEOzs7Ozs7Ozs7Ozs7O0FDNU9hOztBQUViLFlBQVksbUJBQU8sQ0FBQyxvQkFBTztBQUMzQixlQUFlLG1CQUFPLENBQUMsMEVBQW9CO0FBQzNDLGtCQUFrQixtQkFBTyxDQUFDLHFHQUFzQjtBQUNoRDs7QUFFQTtBQUNBO0FBQ0E7O0FBRUEscUNBQXFDLHdCQUF3QjtBQUM3RCxpQ0FBaUMsV0FBVyxpQ0FBaUMsOEJBQThCO0FBQzNHLCtCQUErQixvRkFBb0YsZ0NBQWdDO0FBQ25KLCtCQUErQiwyRkFBMkY7QUFDMUgsK0JBQStCLCtFQUErRSxnQ0FBZ0M7QUFDOUk7QUFDQSxpQ0FBaUMsZUFBZSxrQ0FBa0M7QUFDbEY7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsS0FBSywyREFBMkQ7O0FBRWhFO0FBQ0E7QUFDQTs7QUFFQTtBQUNBLDRCQUE0Qix5QkFBeUI7QUFDckQ7QUFDQSxJQUFJOztBQUVKO0FBQ0E7QUFDQTtBQUNBLElBQUk7O0FBRUo7O0FBRUE7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBOztBQUVBO0FBQ0EsMENBQTBDLFNBQVM7QUFDbkQ7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBLENBQUM7O0FBRUQ7Ozs7Ozs7Ozs7Ozs7QUN4R0E7QUFBQTtBQUFBO0FBQUE7QUFBQTtBQUFBO0FBQWlEO0FBQ1Q7O0FBRXhDO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQSxhQUFhLHVCQUF1QjtBQUNwQztBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBOztBQUVBLGVBQWUsNkJBQTZCO0FBQzVDO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLEdBQUc7QUFDSDtBQUNBO0FBQ0E7QUFDQTtBQUNBOzs7QUFHQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBLEdBQUc7QUFDSDtBQUNBO0FBQ0E7QUFDQTs7O0FBR0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQSx3Q0FBd0M7QUFDeEM7QUFDQTtBQUNBO0FBQ0EsR0FBRzs7QUFFSDs7QUFFQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7O0FBR0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUEsZUFBZSw2REFBVztBQUMxQjs7QUFFQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBLFNBQVM7QUFDVDs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBLFdBQVc7QUFDWDtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7O0FBR0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FB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SDtBQUNBO0FBQ0E7QUFDQSxFQUFFO0FBQ0Y7QUFDQSxFQUFFO0FBQ0Y7QUFDQTtBQUNBLEVBQUU7QUFDRjtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBLEVBQUU7QUFDRjs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLE1BQU07QUFDTjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBOztBQUVBOztBQUVBO0FBQ0E7O0FBRUEsRUFBRTtBQUNGO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBLEVBQUU7QUFDRjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0EsR0FBRztBQUNIO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0EsQ0FBQzs7QUFFRDtBQUNBOztBQUVBO0FBQ0E7QUFDQSxFQUFFO0FBQ0Y7QUFDQTs7QUFFQTs7QUFFQTtBQUNBO0FBQ0EsR0FBRztBQUNIO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBLEVBQUU7QUFDRjtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBLHVEQUF1RDtBQUN2RDs7QUFFQSw2QkFBNkIs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QUMsQ0FBQzs7Ozs7Ozs7Ozs7O0FDdGJuRjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7O0FBR0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7OztBQUdBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLFNBQVM7QUFDVDtBQUNBO0FBQ0EsU0FBUztBQUNUO0FBQ0E7QUFDQTtBQUNBLFNBQVM7QUFDVDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7OztBQUdBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0EsdUVBQXVFO0FBQ3ZFO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0EsNEVBQTRFO0FBQzVFO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLFNBQVM7QUFDVDtBQUNBO0FBQ0E7QUFDQTtBQUNBLFNBQVM7QUFDVDtBQUNBO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOzs7QUFHQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLFNBQVM7QUFDVDtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSx3QkFBd0IsU0FBUztBQUNqQztBQUNBO0FBQ0Esd0RBQXdEO0FBQ3hEO0FBQ0E7QUFDQSxlQUFlO0FBQ2Y7QUFDQTs7QUFFQTtBQUNBLHVCQUF1QjtBQUN2QjtBQUNBO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQTtBQUNBLFNBQVM7QUFDVDtBQUNBO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLHVEQUF1RDtBQUN2RDtBQUNBO0FBQ0E7QUFDQTs7O0FBR0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxtR0FBbUc7QUFDbkc7QUFDQTtBQUNBO0FBQ0E7QUFDQSxvQ0FBb0M7QUFDcEM7QUFDQTtBQUNBLG1DQUFtQztBQUNuQztBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsU0FBUztBQUNUO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOzs7QUFHQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0Esb0NBQW9DOztBQUVwQyxtQ0FBbUM7QUFDbkM7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsMkRBQTJEOztBQUUzRDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQSw4REFBOEQsR0FBRyxRQUFRLEdBQUc7QUFDNUU7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLDZCQUE2QjtBQUM3QjtBQUNBLDZCQUE2QjtBQUM3QjtBQUNBLDZCQUE2QjtBQUM3QjtBQUNBO0FBQ0EseUJBQXlCO0FBQ3pCO0FBQ0E7QUFDQSw2QkFBNkI7QUFDN0I7QUFDQSw2QkFBNkI7QUFDN0I7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBOztBQUVBOztBQUVBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EscUJBQXFCOztBQUVyQjtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxpQkFBaUI7QUFDakI7QUFDQTtBQUNBO0FBQ0E7QUFDQSwyREFBMkQ7QUFDM0Q7QUFDQTs7QUFFQTtBQUNBLHFCQUFxQjtBQUNyQjtBQUNBLGFBQWE7QUFDYixTQUFTO0FBQ1Q7OztBQUdBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxLQUFLO0FBQ0wsQ0FBQyxVOzs7Ozs7Ozs7OztBQzdjRDs7QUFFQTtBQUNBOztBQUVBO0FBQ0EsYUFBYSxhQUFhLFdBQVcsc0RBQXNELG9CQUFvQixlQUFlLHdCQUF3Qiw2Q0FBNkMsdUJBQXVCLEtBQUssb0JBQW9CLHNEQUFzRCxzREFBc0QsNkJBQTZCLHNDQUFzQywrQ0FBK0MsOEJBQThCLHVCQUF1QixnREFBZ0Qsd0JBQXdCLHVCQUF1QiwyQkFBMkIsb0JBQW9CLGVBQWUsNkJBQTZCLHdCQUF3QiwyQkFBMkIsMkNBQTJDLGVBQWUsT0FBTyx5QkFBeUIsbUVBQW1FLG1FQUFtRSw0QkFBNEIsc0RBQXNELDRDQUE0QyxpQ0FBaUMsbUNBQW1DLEVBQUUsK0NBQStDLGtDQUFrQyxrQkFBa0Isb0NBQW9DLFdBQVcsOENBQThDLG9CQUFvQixxREFBcUQsd0JBQXdCLDBCQUEwQixxQkFBcUIsZ0JBQWdCLDRCQUE0QixzQ0FBc0Msb0JBQW9CLGdDQUFnQyw0QkFBNEIsc0NBQXNDLG9CQUFvQiwrQkFBK0IsYUFBYSxjQUFjLEVBQUUsb0RBQW9ELDBDQUEwQyw0Q0FBNEMsRUFBRSxxQkFBcUIseURBQXlELEVBQUUsVTs7Ozs7Ozs7Ozs7QUNOMzlEOztBQUVBO0FBQ0E7O0FBRUE7QUFDQSxhQUFhLFdBQVcsK0JBQStCLFNBQVMsU0FBUyxTQUFTLFNBQVMsZ0JBQWdCLG9CQUFvQixZQUFZLFdBQVcsc0JBQXNCLHNCQUFzQixzQkFBc0IsWUFBWSxXQUFXLHNCQUFzQixzQkFBc0Isc0JBQXNCLFdBQVcseUNBQXlDLEtBQUssZ0RBQWdELHVCQUF1Qiw4QkFBOEIseUNBQXlDLCtCQUErQiwrQkFBK0IsK0JBQStCLG1CQUFtQixVQUFVLG1CQUFtQixzQ0FBc0Msc0JBQXNCLG1DQUFtQyxNQUFNLEdBQUcsOEJBQThCLGlDQUFpQyxtQkFBbUIsb0RBQW9ELHlDQUF5Qyx5QkFBeUIsNEJBQTRCLHVCQUF1Qix1QkFBdUIsSUFBSSxlQUFlLElBQUksZUFBZSxJQUFJLHdGQUF3Rix3QkFBd0IsSUFBSSxlQUFlLElBQUksZUFBZSxJQUFJLHVJQUF1SSxzTUFBc00sc1BBQXNQLHNCQUFzQixFQUFFLGNBQWMsRUFBRSxjQUFjLEVBQUUsbUZBQW1GLHVKQUF1SixtQ0FBbUMsK0NBQStDLEtBQUssZ0NBQWdDLGlDQUFpQyxrQkFBa0IsazJCQUFrMkIsVUFBVSxhQUFhLG1EQUFtRCxpQkFBaUIsdUJBQXVCLDRCQUE0QixvQkFBb0IsbUNBQW1DLEdBQUcsK0JBQStCLDJDQUEyQyxrQkFBa0IseUNBQXlDLHNCQUFzQixnQkFBZ0IsaURBQWlELHNCQUFzQix3QkFBd0IsOEJBQThCLHVEQUF1RCxLQUFLLDJOQUEyTixxQkFBcUIsa0RBQWtELGdQQUFnUCxtREFBbUQsa0RBQWtELHdCQUF3QixhQUFhLG1CQUFtQiwrQ0FBK0Msd0JBQXdCLG9GQUFvRix5RUFBeUUsc0JBQXNCLCtCQUErQiwrQkFBK0IsaUJBQWlCLHdCQUF3QixpQ0FBaUMsaUNBQWlDLG1CQUFtQixrQkFBa0IsZUFBZSxzQ0FBc0Msa0NBQWtDLG9EQUFvRCxtQ0FBbUMsMEJBQTBCLDJCQUEyQix3Q0FBd0MsaUVBQWlFLGFBQWEsZ0NBQWdDLDZDQUE2QyxvQ0FBb0MsMkJBQTJCLHdDQUF3Qyx3Q0FBd0MscUJBQXFCLHNCQUFzQixLQUFLLG9CQUFvQix1QkFBdUIsK0JBQStCLHdCQUF3QixLQUFLLHdCQUF3QixzQkFBc0IsNEJBQTRCLHdCQUF3QiwyQkFBMkIsZ0JBQWdCLGdEQUFnRCw2QkFBNkIsZ0JBQWdCLDZCQUE2QiwyREFBMkQsd0ZBQXdGLDRCQUE0QixpRUFBaUUsa0RBQWtELCtCQUErQixjQUFjLG1FQUFtRSx5Q0FBeUMsYUFBYSwyQkFBMkIsNEdBQTRHLEtBQUssZUFBZSxrQ0FBa0MscUJBQXFCLHFDQUFxQyxpQ0FBaUMscUJBQXFCLG9DQUFvQyxzQkFBc0IsZUFBZSw2Q0FBNkMsZ0RBQWdELHFDQUFxQywyQkFBMkIsYUFBYSxnQ0FBZ0MsRUFBRSxnQ0FBZ0MsdUJBQXVCLHVCQUF1Qiw4RkFBOEYsaUJBQWlCLGFBQWEsaUZBQWlGLGdGQUFnRixxQkFBcUIsZ0JBQWdCLHlCQUF5QixjQUFjLHFCQUFxQixpQkFBaUIsMEJBQTBCLGVBQWUscUJBQXFCLHNCQUFzQixLQUFLLGlDQUFpQyxxQkFBcUIsUUFBUSxVQUFVLCtGQUErRix5QkFBeUIsc0JBQXNCLHlEQUF5RCxHQUFHLGdFQUFnRSxlQUFlLHNDQUFzQyxxQkFBcUIsZ0NBQWdDLDZDQUE2QyxvQ0FBb0MsMkJBQTJCLHdDQUF3Qyx3Q0FBd0MscUJBQXFCLHNCQUFzQixLQUFLLDRCQUE0QixLQUFLLGdFQUFnRSxxQkFBcUIsc0JBQXNCLEtBQUssaUNBQWlDLDBCQUEwQixrREFBa0QsdUJBQXVCLG1FQUFtRSxrS0FBa0ssUUFBUSxnVUFBZ1UsUUFBUSxvQ0FBb0MsMkJBQTJCLFFBQVEsOEVBQThFLFFBQVEsa0RBQWtELE9BQU8sbUdBQW1HLGtDQUFrQyxPQUFPLHdTQUF3UyxjQUFjLDZCQUE2QixVQUFVLDZGQUE2Riw4QkFBOEIsaUNBQWlDLDJKQUEySixXQUFXLHFCQUFxQix5QkFBeUIsZUFBZSwrQkFBK0Isb0JBQW9CLDBCQUEwQix3QkFBd0IsOEJBQThCLG1CQUFtQixzQkFBc0Isa0JBQWtCLHVCQUF1QixtQkFBbUIsdUJBQXVCLDJCQUEyQix3QkFBd0Isc0JBQXNCLFVBQVUsd0JBQXdCLGVBQWUsd0JBQXdCLFVBQVUsR0FBRyw0Q0FBNEMsOERBQThELEVBQUUsWUFBWSx5QkFBeUIsY0FBYyx5QkFBeUIsY0FBYyw0QkFBNEIsNEJBQTRCLDJCQUEyQixnQkFBZ0IseUJBQXlCLDZCQUE2QiwrQ0FBK0MsaUNBQWlDLE9BQU8sOEpBQThKLHVCQUF1Qix3QkFBd0IsV0FBVyx1Q0FBdUMsVUFBVSxhQUFhLGFBQWEsYUFBYSxpQkFBaUIsU0FBUyxVQUFVLFNBQVMsU0FBUyxXQUFXLGNBQWMsV0FBVyx1QkFBdUIsMERBQTBELDZCQUE2Qiw4QkFBOEIsaUJBQWlCLGtCQUFrQix1QkFBdUIsZ0JBQWdCLGVBQWUsWUFBWSxPQUFPLGFBQWEsaUNBQWlDLHlCQUF5QixZQUFZLGNBQWMsNkJBQTZCLHVCQUF1QixhQUFhLGVBQWUsWUFBWSxpQkFBaUIsS0FBSyxpQkFBaUIscUJBQXFCLCtDQUErQyw0QkFBNEIsNEJBQTRCLHNCQUFzQiwyQkFBMkIsNkdBQTZHLDZHQUE2RyxxR0FBcUcscUdBQXFHLDhFQUE4RSxtSEFBbUgsdUlBQXVJLDZMQUE2TCxrQ0FBa0MsUUFBUSxZQUFZLEtBQUssNkJBQTZCLHdDQUF3Qyx3Q0FBd0MsNEJBQTRCLDRCQUE0Qiw2QkFBNkIscUJBQXFCLDRCQUE0QixnQ0FBZ0MsNEJBQTRCLHlDQUF5QyxpQ0FBaUMscUVBQXFFLGtDQUFrQyxRQUFRLFlBQVksS0FBSyw2QkFBNkIsd0NBQXdDLHdDQUF3Qyw0QkFBNEIsNEJBQTRCLDZCQUE2QixxQkFBcUIsNEJBQTRCLGdDQUFnQyw0QkFBNEIseUNBQXlDLGlDQUFpQyxxRUFBcUUsOEZBQThGLDhGQUE4RixtQkFBbUIsaUNBQWlDLCtCQUErQixnQ0FBZ0MsNkJBQTZCLDBCQUEwQiw2QkFBNkIsMkJBQTJCLG1CQUFtQixpQ0FBaUMsK0JBQStCLGtDQUFrQyw2QkFBNkIsMEJBQTBCLDZCQUE2QiwyQkFBMkIsNkVBQTZFLDRGQUE0RixtRUFBbUUsc0VBQXNFLGdFQUFnRSx5RUFBeUUscUZBQXFGLFFBQVEsdUJBQXVCLHdEQUF3RCxRQUFRLHVCQUF1Qix3REFBd0QsMkdBQTJHLDZDQUE2QyxvQkFBb0Isb0JBQW9CLHNCQUFzQixjQUFjLHNCQUFzQixXQUFXLFlBQVksV0FBVyxLQUFLLHNCQUFzQixpQkFBaUIsb0JBQW9CLGlCQUFpQixpQkFBaUIsc0JBQXNCLGlCQUFpQixpQkFBaUIsWUFBWSxXQUFXLCtCQUErQix3QkFBd0IsNEJBQTRCLDBCQUEwQixTQUFTLG1CQUFtQiw4Q0FBOEMsU0FBUyxFQUFFLGlDQUFpQyxVQUFVLFFBQVEsUUFBUSxlQUFlLEtBQUssY0FBYyxzREFBc0QsUUFBUSxlQUFlLEtBQUssY0FBYyxxREFBcUQsbUNBQW1DLG1DQUFtQyxXQUFXLGlDQUFpQyxVQUFVLFlBQVksUUFBUSxlQUFlLEtBQUssY0FBYyxvQkFBb0IsZUFBZSxxQ0FBcUMsbUJBQW1CLDRCQUE0QixRQUFRLFFBQVEsZUFBZSxLQUFLLGNBQWMsb0JBQW9CLGVBQWUscUNBQXFDLG1CQUFtQiwyQkFBMkIsUUFBUSxXQUFXLHNDQUFzQyxtQ0FBbUMsK0RBQStELDJDQUEyQyxzQkFBc0IsK0JBQStCLDZDQUE2QyxRQUFRLGdCQUFnQixLQUFLLHVCQUF1QixhQUFhLGVBQWUscUNBQXFDLGNBQWMsMkJBQTJCLHdCQUF3QixvRkFBb0YsUUFBUSxlQUFlLEtBQUssb0RBQW9ELDBCQUEwQixpQkFBaUIsaUJBQWlCLHdCQUF3QixpQkFBaUIsMEJBQTBCLHFDQUFxQyxlQUFlLFFBQVEsZ0JBQWdCLEtBQUssWUFBWSxrQkFBa0Isa0NBQWtDLFNBQVMsb0VBQW9FLHVCQUF1QixnQkFBZ0IsK0JBQStCLFdBQVcsTUFBTSwwQkFBMEIsdUJBQXVCLDRCQUE0QixpREFBaUQsa0RBQWtELHVCQUF1QixtS0FBbUssa0NBQWtDLHlEQUF5RCx3REFBd0Qsa0NBQWtDLHVCQUF1QiwwQkFBMEIsZ0JBQWdCLEVBQUUsUUFBUSxnQkFBZ0IsS0FBSyxZQUFZLGNBQWMsV0FBVywyREFBMkQsUUFBUSxnQkFBZ0IsS0FBSyxZQUFZLFlBQVksMkJBQTJCLFlBQVksVUFBVSxhQUFhLGlDQUFpQyxFQUFFLGFBQWEsaUNBQWlDLEVBQUUsNENBQTRDLHVFQUF1RSxhQUFhLHFFQUFxRSxFQUFFLHNCQUFzQixpQ0FBaUMsZ0NBQWdDLDJCQUEyQix5Q0FBeUMscUNBQXFDLDBCQUEwQiwyQkFBMkIsNENBQTRDLCtCQUErQixVQUFVLGNBQWMsV0FBVyxVQUFVLG9CQUFvQixhQUFhLFFBQVEsS0FBSyxLQUFLLFNBQVMsWUFBWSxNQUFNLHdCQUF3QixTQUFTLHVCQUF1Qix1Q0FBdUMseUNBQXlDLGNBQWMsMkJBQTJCLDRDQUE0QyxpQkFBaUIsWUFBWSxRQUFRLEtBQUssS0FBSyxnQkFBZ0IsY0FBYyxZQUFZLHdCQUF3QixRQUFRLDRCQUE0QixRQUFRLDhCQUE4QixrQkFBa0IsS0FBSywrRkFBK0YsUUFBUSxLQUFLLCtCQUErQiwyQkFBMkIsU0FBUyxRQUFRLGdCQUFnQixLQUFLLFlBQVksdURBQXVELFFBQVEsZ0JBQWdCLEtBQUssWUFBWSwyQkFBMkIsMEJBQTBCLDJCQUEyQixzRUFBc0UsUUFBUSxnQkFBZ0IsT0FBTyw0QkFBNEIsUUFBUSxLQUFLLEtBQUssZ0JBQWdCLFlBQVksMkVBQTJFLFFBQVEscUJBQXFCLHFCQUFxQixRQUFRLHFCQUFxQix1QkFBdUIsZ0JBQWdCLFVBQVUscUJBQXFCLG1CQUFtQixNQUFNLG1DQUFtQyxNQUFNLGlDQUFpQyxzQkFBc0IsWUFBWSw0QkFBNEIsS0FBSyxZQUFZLDZCQUE2Qiw4QkFBOEIsOEJBQThCLGtDQUFrQyw2Q0FBNkMsZ0RBQWdELEVBQUUseUJBQXlCLDBEQUEwRCx3RUFBd0UsV0FBVyxnRkFBZ0YsNENBQTRDLCtDQUErQyxvQkFBb0IscUJBQXFCLHdDQUF3QyxzQ0FBc0MsYUFBYSxvQkFBb0IsZ0JBQWdCLDhCQUE4QixzQkFBc0IsMkJBQTJCLG1DQUFtQyw0Q0FBNEMscURBQXFELDZDQUE2QyxvQkFBb0IsNkNBQTZDLDRDQUE0Qyw4Q0FBOEMsb0NBQW9DLDJDQUEyQyx3Q0FBd0MscUJBQXFCLFNBQVMsNEVBQTRFLHdCQUF3Qix5REFBeUQsb0NBQW9DLEtBQUssMERBQTBELEtBQUssb0NBQW9DLG9DQUFvQyxlQUFlLDBCQUEwQixrQkFBa0IsNEJBQTRCLGNBQWMsMEJBQTBCLGtCQUFrQixpQ0FBaUMseVlBQXlZLFlBQVksZUFBZSxLQUFLLGVBQWUscUJBQXFCLCtEQUErRCwyQ0FBMkMsOENBQThDLDRDQUE0QywrQ0FBK0MseUNBQXlDLDhQQUE4UCx5Q0FBeUMsZ0NBQWdDLGFBQWEsV0FBVyxrQ0FBa0MsVUFBVSxnQkFBZ0IsS0FBSyxpQkFBaUIsV0FBVyxjQUFjLEVBQUUsY0FBYyxhQUFhLHFCQUFxQiwwQkFBMEIsNENBQTRDLFlBQVksWUFBWSxrQkFBa0IsaUNBQWlDLFVBQVUsZ0RBQWdELEtBQUssVUFBVSx5Q0FBeUMsK0JBQStCLEtBQUssWUFBWSxnQkFBZ0IsVUFBVSwwQ0FBMEMsK0JBQStCLEtBQUssZ0NBQWdDLFVBQVUsK0NBQStDLGtCQUFrQiwyQkFBMkIseUJBQXlCLHlCQUF5QiwwQ0FBMEMsd0JBQXdCLGdEQUFnRCw4RUFBOEUsS0FBSywrQ0FBK0Msa0ZBQWtGLDRDQUE0QyxrREFBa0Qsb0JBQW9CLFlBQVksUUFBUSxnQkFBZ0IsMkZBQTJGLGFBQWEsK0RBQStELGtDQUFrQyxxREFBcUQseUJBQXlCLHNEQUFzRCx3REFBd0QsS0FBSywyREFBMkQsdURBQXVELEVBQUUsa0VBQWtFLHFFQUFxRSwrREFBK0Qsd0VBQXdFLHFCQUFxQixnREFBZ0QseUJBQXlCLGtDQUFrQywwREFBMEQsK0NBQStDLHlCQUF5Qiw4Q0FBOEMsc0RBQXNELEtBQUssb0RBQW9ELDZCQUE2QiwwQkFBMEIsc0RBQXNELDhFQUE4RSxlQUFlLEVBQUUsYUFBYSw2Q0FBNkMsb0NBQW9DLEVBQUUsc0NBQXNDLDBCQUEwQixlQUFlLGtDQUFrQyx3QkFBd0IsRUFBRSw2QkFBNkIsS0FBSyxnREFBZ0QsbUNBQW1DLHNDQUFzQyxpQ0FBaUMsRUFBRSx5REFBeUQsMkRBQTJELDZCQUE2QiwrQkFBK0IsRUFBRSxhQUFhLGlCQUFpQixlQUFlLHdCQUF3Qiw0SEFBNEgsYUFBYSx1QkFBdUIsNkJBQTZCLDZDQUE2QyxLQUFLLGdDQUFnQyxpQkFBaUIsbUJBQW1CLGtCQUFrQixvREFBb0QsbUJBQW1CLGtCQUFrQixzREFBc0QsYUFBYSxhQUFhLG1DQUFtQyxzQkFBc0IsWUFBWSxnRUFBZ0UsNEVBQTRFLDBHQUEwRyw2QkFBNkIsV0FBVyxnREFBZ0QsYUFBYSxPQUFPLGdCQUFnQixPQUFPLDZDQUE2QyxTQUFTLE9BQU8sa0JBQWtCLE9BQU8sS0FBSyxRQUFRLFdBQVcsa0RBQWtELHNCQUFzQixpQkFBaUIsc0RBQXNELGtDQUFrQywyQ0FBMkMsNERBQTRELHdCQUF3QixrQ0FBa0MsNkVBQTZFLEdBQUcsT0FBTyx3QkFBd0IsY0FBYyxJQUFJLDJCQUEyQixjQUFjLHdDQUF3Qyw4REFBOEQsaURBQWlELDRCQUE0QixtQ0FBbUMsdURBQXVELGdDQUFnQyw2RkFBNkYsa0JBQWtCLHdFQUF3RSxxQ0FBcUMsa0NBQWtDLDJFQUEyRSwrQ0FBK0MsdUNBQXVDLHVCQUF1QiwyREFBMkQsZ0dBQWdHLGtDQUFrQyxpQkFBaUIsUUFBUSx5QkFBeUIsS0FBSyxxRUFBcUUsaUNBQWlDLGNBQWMsY0FBYyx3Q0FBd0MsbUdBQW1HLGdHQUFnRyx3QkFBd0IsdUNBQXVDLGtGQUFrRixnQkFBZ0IsMkNBQTJDLGtCQUFrQixRQUFRLGNBQWMsUUFBUSxlQUFlLEtBQUssZUFBZSxlQUFlLHVCQUF1QixRQUFRLHlCQUF5QixVQUFVLGdEQUFnRCw4QkFBOEIsZ0JBQWdCLEdBQUcsc0NBQXNDLGlEQUFpRCxpRUFBaUUsK0ZBQStGLGdCQUFnQixnQkFBZ0IseUNBQXlDLHNCQUFzQixvREFBb0QsK0JBQStCLFdBQVcsWUFBWSxnQkFBZ0IsS0FBSywrQ0FBK0Msc0JBQXNCLCtCQUErQiw4QkFBOEIsV0FBVyxpQkFBaUIsdUJBQXVCLG9DQUFvQyxvQ0FBb0MsWUFBWSxjQUFjLEtBQUssYUFBYSwwQkFBMEIsd0JBQXdCLDRDQUE0QyxnQkFBZ0Isc0JBQXNCLGtCQUFrQixRQUFRLGlCQUFpQixrQ0FBa0MsdUJBQXVCLHFCQUFxQixrQ0FBa0MsYUFBYSxRQUFRLE9BQU8sT0FBTywyQkFBMkIsMEJBQTBCLFdBQVcsOENBQThDLHFHQUFxRyx1Q0FBdUMsY0FBYyxvQkFBb0IsaUJBQWlCLFdBQVcsOENBQThDLG1DQUFtQyxhQUFhLDJCQUEyQixvQkFBb0IseUJBQXlCLHlCQUF5Qix5QkFBeUIseUJBQXlCLHdCQUF3QixRQUFRLGtCQUFrQixLQUFLLHdFQUF3RSxpREFBaUQ7QUFDcHYrQixpREFBaUQsNkNBQTZDLDJIQUEySCxrREFBa0QsOENBQThDLGtEQUFrRCw4Q0FBOEMsa0VBQWtFLG1CQUFtQixTQUFTLHFEQUFxRCxpREFBaUQscURBQXFELGlEQUFpRCxtQkFBbUIsb0ZBQW9GLGdCQUFnQixvREFBb0Qsd0JBQXdCLFdBQVcsMkNBQTJDLHlDQUF5QyxLQUFLLDJDQUEyQyx5Q0FBeUMsYUFBYSxLQUFLLGtEQUFrRCxrRkFBa0YsZUFBZSw0QkFBNEIsWUFBWSxjQUFjLEtBQUssOERBQThELDZDQUE2QyxnQkFBZ0Isd0JBQXdCLElBQUksaURBQWlELGtFQUFrRSxLQUFLLElBQUksaURBQWlELG9FQUFvRSxvQkFBb0IsbUNBQW1DLGdCQUFnQixZQUFZLHdDQUF3Qyx1QkFBdUIscUJBQXFCLHdCQUF3QixtQkFBbUIsS0FBSyxvQkFBb0IsZ0JBQWdCLDBCQUEwQixhQUFhLHVDQUF1QyxnQkFBZ0IsUUFBUSxvQkFBb0IsS0FBSyxzQkFBc0IsWUFBWSxzSUFBc0ksd0JBQXdCLGNBQWMsNkJBQTZCLG1DQUFtQyxLQUFLLGNBQWMsNEJBQTRCLG9DQUFvQyxxQkFBcUIsMENBQTBDLHdCQUF3QixnQkFBZ0IsMEJBQTBCLGFBQWEsT0FBTyw0QkFBNEIsNkNBQTZDLHlCQUF5QixJQUFJLG1DQUFtQyx5QkFBeUIsSUFBSSxtQ0FBbUMsYUFBYSx1QkFBdUIscUJBQXFCLGdCQUFnQixpQ0FBaUMsaUNBQWlDLGFBQWEsZUFBZSx5QkFBeUIsdUJBQXVCLGdCQUFnQiwwQ0FBMEMsNENBQTRDLGFBQWEsZ0JBQWdCLDBCQUEwQix3QkFBd0IsZ0JBQWdCLHNEQUFzRCxxQ0FBcUMsYUFBYSxjQUFjLHdCQUF3QixzQkFBc0IsZ0JBQWdCLDZDQUE2QywwQkFBMEIsY0FBYyxLQUFLLGlCQUFpQix5Q0FBeUMsd0RBQXdELGNBQWMsMEJBQTBCLGtDQUFrQyxvUEFBb1AsMEJBQTBCLDJDQUEyQyxZQUFZLG9CQUFvQixLQUFLLG1CQUFtQiwwREFBMEQsd0JBQXdCLGdCQUFnQixtQ0FBbUMsNEJBQTRCLHNCQUFzQixLQUFLLGlDQUFpQyxpQkFBaUIsS0FBSyxnQkFBZ0Isa0NBQWtDLDBCQUEwQixpQ0FBaUMsZUFBZSxLQUFLLHdCQUF3QixvRUFBb0UsRUFBRSw0QkFBNEIsNkNBQTZDLDJDQUEyQywrQ0FBK0MsaUNBQWlDLDBEQUEwRCwyRUFBMkUsZ0JBQWdCLGFBQWEsZ0JBQWdCLE9BQU8sa0VBQWtFLCtCQUErQix5QkFBeUIseUJBQXlCLHFDQUFxQyxhQUFhLDhCQUE4Qix5QkFBeUIscUNBQXFDLGFBQWEseUJBQXlCLHlCQUF5QixxQ0FBcUMsYUFBYSw4QkFBOEIseUJBQXlCLHFDQUFxQyxhQUFhLHlCQUF5Qix5QkFBeUIscUNBQXFDLGFBQWEsOEJBQThCLHlCQUF5QixxQ0FBcUMsYUFBYSx5QkFBeUIseUJBQXlCLHFDQUFxQyxhQUFhLDhCQUE4Qix5QkFBeUIscUNBQXFDLGFBQWEsZ0ZBQWdGLFNBQVMsU0FBUyx3REFBd0QsYUFBYSw4Q0FBOEMsZ0tBQWdLLFlBQVksa0NBQWtDLE1BQU0sd0VBQXdFLGFBQWEsNkJBQTZCLGFBQWEsT0FBTyxPQUFPLFNBQVMsNkJBQTZCLFdBQVcsZUFBZSxPQUFPLE9BQU8sNkJBQTZCLFVBQVUsK0JBQStCLHlCQUF5Qix5QkFBeUIscUNBQXFDLGFBQWEsOEJBQThCLHlCQUF5QixxQ0FBcUMsYUFBYSx5QkFBeUIseUJBQXlCLHFDQUFxQyxhQUFhLDhCQUE4Qix5QkFBeUIscUNBQXFDLGFBQWEsY0FBYyxnQkFBZ0IsNENBQTRDLGNBQWMsaUNBQWlDLCtDQUErQywrQ0FBK0MsU0FBUyxzQ0FBc0MsK0NBQStDLCtDQUErQyxTQUFTLHNCQUFzQix3Q0FBd0MscUNBQXFDLGFBQWEsNkNBQTZDLHFDQUFxQyxhQUFhLHdDQUF3QyxxQ0FBcUMsYUFBYSw2Q0FBNkMscUNBQXFDLGFBQWEsY0FBYywyQ0FBMkMsd0NBQXdDLHdDQUF3QyxjQUFjLHdDQUF3Qyw2Q0FBNkMsV0FBVyw4Q0FBOEMscUJBQXFCLG1EQUFtRCxlQUFlLGlCQUFpQixrQ0FBa0MscUJBQXFCLDhHQUE4RyxtQkFBbUIsOEdBQThHLGlCQUFpQiw2QkFBNkIsbUVBQW1FLGNBQWMsd0JBQXdCLDBEQUEwRCxrRUFBa0UsY0FBYyxrQ0FBa0Msa0ZBQWtGLHFEQUFxRCxZQUFZLGdCQUFnQixPQUFPLDhCQUE4Qix3RUFBd0UsZ0JBQWdCLGVBQWUsc0JBQXNCLHlFQUF5RSxtQ0FBbUMsZ0JBQWdCLGNBQWMsd0JBQXdCLFdBQVcsY0FBYyxXQUFXLDhDQUE4Qyw0R0FBNEcsaUJBQWlCLGVBQWUsV0FBVyxnQkFBZ0Isa0NBQWtDLHNGQUFzRixrQ0FBa0Msb0ZBQW9GLGlCQUFpQiw2QkFBNkIsdUhBQXVILGNBQWMsOEZBQThGLG9FQUFvRSxlQUFlLGtDQUFrQyxlQUFlLE9BQU8sUUFBUSxjQUFjLGtCQUFrQixlQUFlLFVBQVUsV0FBVyxTQUFTLGNBQWMsaUJBQWlCLEtBQUssZ0NBQWdDLGlCQUFpQixlQUFlLGlCQUFpQixTQUFTLE1BQU0sZUFBZSxRQUFRLFdBQVcsV0FBVyxnQkFBZ0IsZUFBZSwyRUFBMkUsbUJBQW1CLGVBQWUsZUFBZSxvQkFBb0IsZ0JBQWdCLGdCQUFnQixxQkFBcUIsaUJBQWlCLGlCQUFpQixrQkFBa0IsY0FBYyxjQUFjLHFCQUFxQix5QkFBeUIsdUJBQXVCLG1CQUFtQixzQkFBc0IsMENBQTBDLDJDQUEyQyw0REFBNEQsY0FBYyxzQkFBc0IsK0JBQStCLHdCQUF3QiwrQkFBK0IseUJBQXlCLG9DQUFvQyw0QkFBNEIsb0NBQW9DLDJCQUEyQixZQUFZLGdDQUFnQyw2RUFBNkUscURBQXFELFlBQVksZ0JBQWdCLE9BQU8sNEJBQTRCLDRJQUE0SSxXQUFXLDhDQUE4QyxvQ0FBb0MsNkJBQTZCLFlBQVksMEJBQTBCLHFCQUFxQixNQUFNLDBDQUEwQyxNQUFNLHdDQUF3Qyw0REFBNEQseURBQXlELE1BQU0sNkdBQTZHLGNBQWMsMERBQTBELDBCQUEwQixxQkFBcUIsaUdBQWlHLGlDQUFpQyxrQ0FBa0MsY0FBYyxvQkFBb0Isd0JBQXdCLG1DQUFtQyxxQ0FBcUMsS0FBSyxxQ0FBcUMseUJBQXlCLE9BQU8sc0ZBQXNGLFlBQVksZ0JBQWdCLEtBQUssWUFBWSxZQUFZLCtCQUErQixVQUFVLGNBQWMsMEJBQTBCLElBQUksMEJBQTBCLHdDQUF3QyxvQ0FBb0MsMENBQTBDLGtCQUFrQixLQUFLLGtEQUFrRCwyQkFBMkIsMERBQTBELEdBQUcsWUFBWSxpQkFBaUIsS0FBSyxxQkFBcUIsa0NBQWtDLHNDQUFzQyx1QkFBdUIsZ0JBQWdCLCtHQUErRyxtQ0FBbUMsU0FBUyxpQ0FBaUMsb0ZBQW9GLHNDQUFzQyw4QkFBOEIsMkNBQTJDLDhEQUE4RCwwRUFBMEUsS0FBSyw2REFBNkQsc0JBQXNCLDBEQUEwRCxFQUFFLHFFQUFxRSxFQUFFLDhEQUE4RCxFQUFFLGlFQUFpRSxFQUFFLHNGQUFzRixRQUFRLG1DQUFtQyx3Q0FBd0MscUNBQXFDLFlBQVksK0JBQStCLDRDQUE0QyxrREFBa0QsTUFBTSxlQUFlLDBCQUEwQixpQ0FBaUMsd0JBQXdCLDBCQUEwQiw4QkFBOEIsZ0ZBQWdGLHFDQUFxQyxvREFBb0QsNEhBQTRILHNCQUFzQixLQUFLLEtBQUsscUNBQXFDLDJLQUEySywwQkFBMEIsd0RBQXdELHdEQUF3RCxnQ0FBZ0MsUUFBUSxnQkFBZ0IsT0FBTyw4QkFBOEIsb0JBQW9CLHlEQUF5RCx1RkFBdUYsMEJBQTBCLHNCQUFzQixnQkFBZ0IsdUJBQXVCLHFCQUFxQixxQkFBcUIscUJBQXFCLE1BQU0scUNBQXFDLE1BQU0sbUNBQW1DLGlDQUFpQyxRQUFRLGdCQUFnQixPQUFPLDRDQUE0QyxvQkFBb0IscUxBQXFMLFNBQVMsVUFBVSxVQUFVLGtDQUFrQyxPQUFPLHVHQUF1RyxZQUFZLHdCQUF3QiwyRUFBMkUsNkJBQTZCLEVBQUUseUJBQXlCLDJFQUEyRSxhQUFhLEVBQUUsb0JBQW9CLGlEQUFpRCw2QkFBNkIsRUFBRSw4REFBOEQsc0pBQXNKLHlCQUF5QixFQUFFLHNCQUFzQixzQkFBc0Isc0RBQXNELFNBQVMsNkZBQTZGLDJGQUEyRiwrQkFBK0IsWUFBWSxvQkFBb0IsS0FBSyxvQkFBb0IsaUpBQWlKLHdEQUF3RCwwQ0FBMEMsZ0NBQWdDLGdEQUFnRCxVQUFVLGNBQWMsT0FBTywwREFBMEQsdUJBQXVCLG1CQUFtQixZQUFZLGdCQUFnQiwrQ0FBK0MsU0FBUyxRQUFRLG9CQUFvQixLQUFLLGlCQUFpQiw0REFBNEQsNENBQTRDLGVBQWUsdUNBQXVDLGlDQUFpQyxrQ0FBa0MsMkJBQTJCLDhCQUE4Qix1REFBdUQsZ0NBQWdDLFVBQVUsaUJBQWlCLCtCQUErQixFQUFFLHVCQUF1Qix1Q0FBdUMsOEJBQThCLHlCQUF5QixjQUFjLHVCQUF1QixPQUFPLGtDQUFrQywyQkFBMkIsOEJBQThCLHVEQUF1RCxnQ0FBZ0MsVUFBVSx1QkFBdUIsd0JBQXdCLCtCQUErQixZQUFZLG9CQUFvQixLQUFLLG9CQUFvQiw0REFBNEQsU0FBUywwQ0FBMEMsa01BQWtNLDZEQUE2RCwrREFBK0QsMkJBQTJCLGdDQUFnQywyQkFBMkIsZUFBZSxlQUFlLGlCQUFpQix5RUFBeUUsaURBQWlELGlCQUFpQixjQUFjLHdDQUF3Qyx1S0FBdUssMEJBQTBCLHFCQUFxQixNQUFNLDBDQUEwQyxNQUFNLHdDQUF3QyxxQ0FBcUMsZ0NBQWdDLHNGQUFzRixpQkFBaUIsOEVBQThFLDBEQUEwRCxxQ0FBcUMsS0FBSyxzREFBc0QsaUNBQWlDLElBQUksS0FBSyxxQkFBcUIsdUJBQXVCLG1DQUFtQyxzREFBc0QsbUNBQW1DLGdCQUFnQixpQ0FBaUMsa0JBQWtCLDBDQUEwQyw4REFBOEQsYUFBYSx1QkFBdUIsa0JBQWtCLGlDQUFpQyw0QkFBNEIsMEJBQTBCLEdBQUcsNkJBQTZCLGdDQUFnQyxVOzs7Ozs7Ozs7OztBQ1A3eW9COztBQUVBO0FBQ0E7O0FBRUE7QUFDQSxhQUFhLGNBQWMsMEJBQTBCLHdMQUF3TCw4RUFBOEUsZUFBZSxpREFBaUQsbURBQW1ELHFFQUFxRSx1RkFBdUYsNkZBQTZGLCtCQUErQixrRkFBa0YsaUJBQWlCLHFKQUFxSixTQUFTLGtCQUFrQixTQUFTLGlDQUFpQyw2QkFBNkIsY0FBYyxxQkFBcUIsYUFBYSx1QkFBdUIsZ0JBQWdCLDJEQUEyRCxTQUFTLCtDQUErQywwQkFBMEIsNkdBQTZHLG9DQUFvQyw4REFBOEQsWUFBWSw0Q0FBNEMsTUFBTSwyR0FBMkcscUJBQXFCLHlJQUF5SSx1QkFBdUIsa0JBQWtCLHdCQUF3QixVQUFVLGFBQWEsY0FBYyxnRkFBZ0Ysb0JBQW9CLG1DQUFtQywwQkFBMEIsSUFBSSwwREFBMEQsOENBQThDLGlEQUFpRCxtQkFBbUIsdURBQXVELHNDQUFzQyx1Q0FBdUMsRUFBRSw2Q0FBNkMsNEJBQTRCLGlCQUFpQiw0Q0FBNEMsRUFBRSxvQ0FBb0MseUJBQXlCLHFCQUFxQiwrQ0FBK0MsRUFBRSx1Q0FBdUMsOEJBQThCLGFBQWEsdUJBQXVCLDhEQUE4RCwwQkFBMEIsb0NBQW9DLEVBQUUsVUFBVSxhQUFhLGFBQWEsT0FBTyw2QkFBNkIsT0FBTyxnREFBZ0QsTUFBTSwrQ0FBK0Msb0JBQW9CLGdDQUFnQyxvQkFBb0Isc0JBQXNCLG9CQUFvQix5QkFBeUIsU0FBUyxFQUFFLGdCQUFnQixTQUFTLEVBQUUsK0JBQStCLG1CQUFtQix1QkFBdUIsYUFBYSxpRUFBaUUsd0JBQXdCLDJCQUEyQiwwQ0FBMEMsa0JBQWtCLGlFQUFpRSxrQkFBa0Isa0JBQWtCLG1CQUFtQiw4Q0FBOEMsaUNBQWlDLGlDQUFpQyxVQUFVLDZDQUE2QyxFQUFFLGtCQUFrQixrQkFBa0IsZ0JBQWdCLGtCQUFrQixzQkFBc0IsZUFBZSx5QkFBeUIsZ0JBQWdCLCtDQUErQyxVQUFVLDZDQUE2QyxFQUFFLHNDQUFzQyx3QkFBd0IsdUJBQXVCLHlDQUF5QyxxQ0FBcUMsc0JBQXNCLDhCQUE4QixZQUFZLGNBQWMsZ0NBQWdDLHVDQUF1Qyw0QkFBNEIsaUJBQWlCLDBEQUEwRCwwQkFBMEIsaUJBQWlCLHlCQUF5QixpQkFBaUIsbUdBQW1HLFNBQVMsa0JBQWtCLG1DQUFtQyxHQUFHLGtEQUFrRCxJQUFJLGtEQUFrRCx1Q0FBdUMsdUhBQXVILHFCQUFxQixrQkFBa0Isa0JBQWtCLFlBQVksWUFBWSxRQUFRLFFBQVEsT0FBTywyQkFBMkIsVUFBVSwyQkFBMkIsV0FBVyxrQkFBa0IsdUZBQXVGLGFBQWEsYUFBYSxFQUFFLGlCQUFpQixZQUFZLDZFQUE2RSx3QkFBd0IsV0FBVywwQkFBMEIsNEJBQTRCLDRCQUE0Qix1Q0FBdUMsc0RBQXNELHNFQUFzRSxxQkFBcUIscUJBQXFCLE9BQU8sMkJBQTJCLFlBQVksT0FBTyxPQUFPLDJCQUEyQixZQUFZLE9BQU8sUUFBUSxhQUFhLGFBQWEsRUFBRSxpQkFBaUIsWUFBWSw0RUFBNEUsb0NBQW9DLCtEQUErRCw4Q0FBOEMsNENBQTRDLGtDQUFrQyx3Q0FBd0MsdUNBQXVDLHVDQUF1QyxtQ0FBbUMscUJBQXFCLHdEQUF3RCxFQUFFLFU7Ozs7Ozs7Ozs7O0FDTmp4TTs7QUFFQTtBQUNBOztBQUVBO0FBQ0EsYUFBYSxvQkFBb0IsZUFBZSxPQUFPLFVBQVUsU0FBUyxVQUFVLDBCQUEwQixxQkFBcUIsd0JBQXdCLHdCQUF3QixxQkFBcUIsbUJBQW1CLGlFQUFpRSx3QkFBd0IscUJBQXFCLHNCQUFzQiwwRUFBMEUsbURBQW1ELGtDQUFrQyxjQUFjLDREQUE0RCxxQ0FBcUMsMkJBQTJCLGNBQWMsbUNBQW1DLHNCQUFzQiwyQkFBMkIsY0FBYywwQ0FBMEMsc0JBQXNCLG9CQUFvQix5RkFBeUYsb0VBQW9FLHVCQUF1QixtQkFBbUIsNENBQTRDLEtBQUssbURBQW1ELHNEQUFzRCxhQUFhLHdCQUF3QixrQ0FBa0MsK0JBQStCLFFBQVEsd0NBQXdDLDBDQUEwQyxjQUFjLG9FQUFvRSxTQUFTLDBDQUEwQyxFQUFFLFNBQVMsZ0NBQWdDLHFCQUFxQixrREFBa0QsK0RBQStELDREQUE0RCxHQUFHLDhCQUE4Qix5Q0FBeUMsZ0NBQWdDLHdCQUF3QiwwQ0FBMEMsb0NBQW9DLGdFQUFnRSwrREFBK0QsbUVBQW1FLG9FQUFvRSw4QkFBOEIsMEJBQTBCLHNDQUFzQyxzQkFBc0Isb0JBQW9CLDRCQUE0QiwwQkFBMEIsc0NBQXNDLG1CQUFtQixxQkFBcUIsNEJBQTRCLHFFQUFxRSxvQ0FBb0MseUNBQXlDLG1CQUFtQixhQUFhLDBCQUEwQix3QkFBd0IsNENBQTRDLGdCQUFnQixzQkFBc0Isa0JBQWtCLFFBQVEsaUJBQWlCLHNEQUFzRCx1QkFBdUIscUJBQXFCLGtDQUFrQyxhQUFhLFFBQVEsT0FBTyxPQUFPLDJCQUEyQiwyQ0FBMkMsbUNBQW1DLDBCQUEwQixvQkFBb0IsZ0NBQWdDLEtBQUssK0JBQStCLDZDQUE2Qyw0Q0FBNEMsMEJBQTBCLG9CQUFvQixpQ0FBaUMsS0FBSywrQkFBK0IsNkNBQTZDLDRDQUE0QyxvQkFBb0IsNEJBQTRCLDJEQUEyRCwyQkFBMkIsZ0RBQWdELHdIQUF3SCxtQ0FBbUMsK0JBQStCLCtCQUErQixzREFBc0Qsd0JBQXdCLDJCQUEyQixtQ0FBbUMsb0NBQW9DLEVBQUUsK0NBQStDLHNDQUFzQyxvQ0FBb0Msd0JBQXdCLFdBQVcsOENBQThDLHVDQUF1QywyQ0FBMkMsZ0JBQWdCLCtCQUErQix5Q0FBeUMsa05BQWtOLHNCQUFzQix3QkFBd0IsZUFBZSxFQUFFLG9EQUFvRCw0Q0FBNEMsNENBQTRDLCtEQUErRCxFQUFFLHFCQUFxQixtQkFBbUIsV0FBVyxtREFBbUQsZ0NBQWdDLEVBQUUsVTs7Ozs7Ozs7Ozs7QUNOaGdLOztBQUVBO0FBQ0E7O0FBRUE7QUFDQSxhQUFhLGFBQWEsT0FBTyxzRUFBc0UsNkJBQTZCLCtCQUErQiwrQ0FBK0Msa0NBQWtDLHVCQUF1Qiw0QkFBNEIsT0FBTywyQkFBMkIsNEJBQTRCLFNBQVMsaUJBQWlCLHVCQUF1QixrQkFBa0IscUJBQXFCLHFGQUFxRixtQkFBbUIscURBQXFELFlBQVksYUFBYSxpQkFBaUIsa0JBQWtCLFdBQVcsS0FBSyxjQUFjLFlBQVksYUFBYSxLQUFLLG9CQUFvQixXQUFXLFVBQVUsa0NBQWtDLE1BQU0sc0NBQXNDLE1BQU0sK0JBQStCLE1BQU0sbUNBQW1DLE1BQU0saUNBQWlDLE1BQU0sMkJBQTJCLE1BQU0sK0JBQStCLE1BQU0sa0NBQWtDLE1BQU0sa0NBQWtDLE1BQU0sNENBQTRDLE1BQU0sa0NBQWtDLE1BQU0sdUNBQXVDLE1BQU0sNkJBQTZCLE1BQU0sK0JBQStCLE1BQU0sK0JBQStCLE1BQU0sd0JBQXdCLE1BQU0sVUFBVSxhQUFhLEtBQUssV0FBVyxZQUFZLEtBQUssWUFBWSxrQkFBa0IsMkJBQTJCLHVFQUF1RSxtQ0FBbUMsMkRBQTJELFNBQVMsUUFBUSwwQkFBMEIsNENBQTRDLDBDQUEwQywwQ0FBMEMsdUZBQXVGLFlBQVksZUFBZSxLQUFLLHVEQUF1RCx1REFBdUQsV0FBVyxnQ0FBZ0MsNkJBQTZCLG9CQUFvQiw4Q0FBOEMsb0NBQW9DLDZFQUE2RSwwQkFBMEIsNkJBQTZCLGNBQWMsU0FBUyxLQUFLLHFDQUFxQyxrQkFBa0IscUlBQXFJLDhSQUE4UixxRUFBcUUsMkVBQTJFLG9CQUFvQixzREFBc0QsOENBQThDLHNCQUFzQixzQkFBc0Isa0NBQWtDLGFBQWEsbUNBQW1DLGNBQWMsZ0lBQWdJLDJCQUEyQixtQ0FBbUMsc0JBQXNCLEtBQUssK0RBQStELFlBQVksZ0JBQWdCLEtBQUssNklBQTZJLE9BQU8sb0JBQW9CLG9CQUFvQixpQkFBaUIsd0RBQXdELHFDQUFxQyxLQUFLLG1GQUFtRiwyQ0FBMkMsYUFBYSxPQUFPLGdCQUFnQixPQUFPLGtCQUFrQixPQUFPLEtBQUssUUFBUSxXQUFXLFdBQVcsUUFBUSx5Q0FBeUMsOEJBQThCLHNCQUFzQixxQ0FBcUMsbUJBQW1CLG1EQUFtRCx3QkFBd0IsbURBQW1ELHNCQUFzQiwrQ0FBK0MsdUJBQXVCLCtDQUErQyx5QkFBeUIsbURBQW1ELHNCQUFzQixxREFBcUQscUJBQXFCLDhCQUE4QixnQkFBZ0IsNEJBQTRCLGdCQUFnQiwyQkFBMkIsY0FBYyw2QkFBNkIsYUFBYSwrQkFBK0Isd0NBQXdDLGlDQUFpQyx3Q0FBd0MsNEJBQTRCLGNBQWMsWUFBWSxpQkFBaUIsU0FBUyxHQUFHLE9BQU8sY0FBYyxjQUFjLG1DQUFtQyxlQUFlLGFBQWEsc0JBQXNCLCtDQUErQyxvQkFBb0IsMERBQTBELG1CQUFtQixjQUFjLEtBQUsseURBQXlELHNCQUFzQix3Q0FBd0MsS0FBSyxtQkFBbUIsMkJBQTJCLGNBQWMsb0NBQW9DLG9DQUFvQywwQkFBMEIsbUVBQW1FLDZJQUE2SSxzREFBc0QsMkJBQTJCLHlDQUF5Qyw0Q0FBNEMsUUFBUSwwQkFBMEIsNkJBQTZCLDRCQUE0Qiw0QkFBNEIsMEJBQTBCLEtBQUssb0NBQW9DLDhCQUE4QixZQUFZLGdGQUFnRiwyQkFBMkIsU0FBUyxLQUFLLGFBQWEsMENBQTBDLDJCQUEyQixVQUFVLEtBQUssY0FBYyxLQUFLLFNBQVMsdURBQXVELFlBQVksRUFBRSxFQUFFLHFCQUFxQixvREFBb0QsRUFBRSw2QkFBNkIsbUNBQW1DLFU7Ozs7Ozs7Ozs7O0FDTnBsTix3Qjs7Ozs7Ozs7Ozs7QUNBQSx1Qjs7Ozs7Ozs7Ozs7QUNBQSwwQiIsImZpbGUiOiJUcmVuZGluZ0RhdGFEaXNwbGF5LmpzIiwic291cmNlc0NvbnRlbnQiOlsiIFx0Ly8gVGhlIG1vZHVsZSBjYWNoZVxuIFx0dmFyIGluc3RhbGxlZE1vZHVsZXMgPSB7fTtcblxuIFx0Ly8gVGhlIHJlcXVpcmUgZnVuY3Rpb25cbiBcdGZ1bmN0aW9uIF9fd2VicGFja19yZXF1aXJlX18obW9kdWxlSWQpIHtcblxuIFx0XHQvLyBDaGVjayBpZiBtb2R1bGUgaXMgaW4gY2FjaGVcbiBcdFx0aWYoaW5zdGFsbGVkTW9kdWxlc1ttb2R1bGVJZF0pIHtcbiBcdFx0XHRyZXR1cm4gaW5zdGFsbGVkTW9kdWxlc1ttb2R1bGVJZF0uZXhwb3J0cztcbiBcdFx0fVxuIFx0XHQvLyBDcmVhdGUgYSBuZXcgbW9kdWxlIChhbmQgcHV0IGl0IGludG8gdGhlIGNhY2hlKVxuIFx0XHR2YXIgbW9kdWxlID0gaW5zdGFsbGVkTW9kdWxlc1ttb2R1bGVJZF0gPSB7XG4gXHRcdFx0aTogbW9kdWxlSWQsXG4gXHRcdFx0bDogZmFsc2UsXG4gXHRcdFx0ZXhwb3J0czoge31cbiBcdFx0fTtcblxuIFx0XHQvLyBFeGVjdXRlIHRoZSBtb2R1bGUgZnVuY3Rpb25cbiBcdFx0bW9kdWxlc1ttb2R1bGVJZF0uY2FsbChtb2R1bGUuZXhwb3J0cywgbW9kdWxlLCBtb2R1bGUuZXhwb3J0cywgX193ZWJwYWNrX3JlcXVpcmVfXyk7XG5cbiBcdFx0Ly8gRmxhZyB0aGUgbW9kdWxlIGFzIGxvYWRlZFxuIFx0XHRtb2R1bGUubCA9IHRydWU7XG5cbiBcdFx0Ly8gUmV0dXJuIHRoZSBleHBvcnRzIG9mIHRoZSBtb2R1bGVcbiBcdFx0cmV0dXJuIG1vZHVsZS5leHBvcnRzO1xuIFx0fVxuXG5cbiBcdC8vIGV4cG9zZSB0aGUgbW9kdWxlcyBvYmplY3QgKF9fd2VicGFja19tb2R1bGVzX18pXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLm0gPSBtb2R1bGVzO1xuXG4gXHQvLyBleHBvc2UgdGhlIG1vZHVsZSBjYWNoZVxuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5jID0gaW5zdGFsbGVkTW9kdWxlcztcblxuIFx0Ly8gZGVmaW5lIGdldHRlciBmdW5jdGlvbiBmb3IgaGFybW9ueSBleHBvcnRzXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLmQgPSBmdW5jdGlvbihleHBvcnRzLCBuYW1lLCBnZXR0ZXIpIHtcbiBcdFx0aWYoIV9fd2VicGFja19yZXF1aXJlX18ubyhleHBvcnRzLCBuYW1lKSkge1xuIFx0XHRcdE9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBuYW1lLCB7IGVudW1lcmFibGU6IHRydWUsIGdldDogZ2V0dGVyIH0pO1xuIFx0XHR9XG4gXHR9O1xuXG4gXHQvLyBkZWZpbmUgX19lc01vZHVsZSBvbiBleHBvcnRzXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLnIgPSBmdW5jdGlvbihleHBvcnRzKSB7XG4gXHRcdGlmKHR5cGVvZiBTeW1ib2wgIT09ICd1bmRlZmluZWQnICYmIFN5bWJvbC50b1N0cmluZ1RhZykge1xuIFx0XHRcdE9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBTeW1ib2wudG9TdHJpbmdUYWcsIHsgdmFsdWU6ICdNb2R1bGUnIH0pO1xuIFx0XHR9XG4gXHRcdE9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCAnX19lc01vZHVsZScsIHsgdmFsdWU6IHRydWUgfSk7XG4gXHR9O1xuXG4gXHQvLyBjcmVhdGUgYSBmYWtlIG5hbWVzcGFjZSBvYmplY3RcbiBcdC8vIG1vZGUgJiAxOiB2YWx1ZSBpcyBhIG1vZHVsZSBpZCwgcmVxdWlyZSBpdFxuIFx0Ly8gbW9kZSAmIDI6IG1lcmdlIGFsbCBwcm9wZXJ0aWVzIG9mIHZhbHVlIGludG8gdGhlIG5zXG4gXHQvLyBtb2RlICYgNDogcmV0dXJuIHZhbHVlIHdoZW4gYWxyZWFkeSBucyBvYmplY3RcbiBcdC8vIG1vZGUgJiA4fDE6IGJlaGF2ZSBsaWtlIHJlcXVpcmVcbiBcdF9fd2VicGFja19yZXF1aXJlX18udCA9IGZ1bmN0aW9uKHZhbHVlLCBtb2RlKSB7XG4gXHRcdGlmKG1vZGUgJiAxKSB2YWx1ZSA9IF9fd2VicGFja19yZXF1aXJlX18odmFsdWUpO1xuIFx0XHRpZihtb2RlICYgOCkgcmV0dXJuIHZhbHVlO1xuIFx0XHRpZigobW9kZSAmIDQpICYmIHR5cGVvZiB2YWx1ZSA9PT0gJ29iamVjdCcgJiYgdmFsdWUgJiYgdmFsdWUuX19lc01vZHVsZSkgcmV0dXJuIHZhbHVlO1xuIFx0XHR2YXIgbnMgPSBPYmplY3QuY3JlYXRlKG51bGwpO1xuIFx0XHRfX3dlYnBhY2tfcmVxdWlyZV9fLnIobnMpO1xuIFx0XHRPYmplY3QuZGVmaW5lUHJvcGVydHkobnMsICdkZWZhdWx0JywgeyBlbnVtZXJhYmxlOiB0cnVlLCB2YWx1ZTogdmFsdWUgfSk7XG4gXHRcdGlmKG1vZGUgJiAyICYmIHR5cGVvZiB2YWx1ZSAhPSAnc3RyaW5nJykgZm9yKHZhciBrZXkgaW4gdmFsdWUpIF9fd2VicGFja19yZXF1aXJlX18uZChucywga2V5LCBmdW5jdGlvbihrZXkpIHsgcmV0dXJuIHZhbHVlW2tleV07IH0uYmluZChudWxsLCBrZXkpKTtcbiBcdFx0cmV0dXJuIG5zO1xuIFx0fTtcblxuIFx0Ly8gZ2V0RGVmYXVsdEV4cG9ydCBmdW5jdGlvbiBmb3IgY29tcGF0aWJpbGl0eSB3aXRoIG5vbi1oYXJtb255IG1vZHVsZXNcbiBcdF9fd2VicGFja19yZXF1aXJlX18ubiA9IGZ1bmN0aW9uKG1vZHVsZSkge1xuIFx0XHR2YXIgZ2V0dGVyID0gbW9kdWxlICYmIG1vZHVsZS5fX2VzTW9kdWxlID9cbiBcdFx0XHRmdW5jdGlvbiBnZXREZWZhdWx0KCkgeyByZXR1cm4gbW9kdWxlWydkZWZhdWx0J107IH0gOlxuIFx0XHRcdGZ1bmN0aW9uIGdldE1vZHVsZUV4cG9ydHMoKSB7IHJldHVybiBtb2R1bGU7IH07XG4gXHRcdF9fd2VicGFja19yZXF1aXJlX18uZChnZXR0ZXIsICdhJywgZ2V0dGVyKTtcbiBcdFx0cmV0dXJuIGdldHRlcjtcbiBcdH07XG5cbiBcdC8vIE9iamVjdC5wcm90b3R5cGUuaGFzT3duUHJvcGVydHkuY2FsbFxuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5vID0gZnVuY3Rpb24ob2JqZWN0LCBwcm9wZXJ0eSkgeyByZXR1cm4gT2JqZWN0LnByb3RvdHlwZS5oYXNPd25Qcm9wZXJ0eS5jYWxsKG9iamVjdCwgcHJvcGVydHkpOyB9O1xuXG4gXHQvLyBfX3dlYnBhY2tfcHVibGljX3BhdGhfX1xuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5wID0gXCJcIjtcblxuXG4gXHQvLyBMb2FkIGVudHJ5IG1vZHVsZSBhbmQgcmV0dXJuIGV4cG9ydHNcbiBcdHJldHVybiBfX3dlYnBhY2tfcmVxdWlyZV9fKF9fd2VicGFja19yZXF1aXJlX18ucyA9IFwiLi9UU1gvVHJlbmRpbmdEYXRhRGlzcGxheS50c3hcIik7XG4iLCJcInVzZSBzdHJpY3RcIjtcclxuLy8gKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbi8vICBDcmVhdGVHdWlkLnRzIC0gR2J0Y1xyXG4vL1xyXG4vLyAgQ29weXJpZ2h0IMKpIDIwMjEsIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZS4gIEFsbCBSaWdodHMgUmVzZXJ2ZWQuXHJcbi8vXHJcbi8vICBMaWNlbnNlZCB0byB0aGUgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlIChHUEEpIHVuZGVyIG9uZSBvciBtb3JlIGNvbnRyaWJ1dG9yIGxpY2Vuc2UgYWdyZWVtZW50cy4gU2VlXHJcbi8vICB0aGUgTk9USUNFIGZpbGUgZGlzdHJpYnV0ZWQgd2l0aCB0aGlzIHdvcmsgZm9yIGFkZGl0aW9uYWwgaW5mb3JtYXRpb24gcmVnYXJkaW5nIGNvcHlyaWdodCBvd25lcnNoaXAuXHJcbi8vICBUaGUgR1BBIGxpY2Vuc2VzIHRoaXMgZmlsZSB0byB5b3UgdW5kZXIgdGhlIE1JVCBMaWNlbnNlIChNSVQpLCB0aGUgXCJMaWNlbnNlXCI7IHlvdSBtYXkgbm90IHVzZSB0aGlzXHJcbi8vICBmaWxlIGV4Y2VwdCBpbiBjb21wbGlhbmNlIHdpdGggdGhlIExpY2Vuc2UuIFlvdSBtYXkgb2J0YWluIGEgY29weSBvZiB0aGUgTGljZW5zZSBhdDpcclxuLy9cclxuLy8gICAgICBodHRwOi8vb3BlbnNvdXJjZS5vcmcvbGljZW5zZXMvTUlUXHJcbi8vXHJcbi8vICBVbmxlc3MgYWdyZWVkIHRvIGluIHdyaXRpbmcsIHRoZSBzdWJqZWN0IHNvZnR3YXJlIGRpc3RyaWJ1dGVkIHVuZGVyIHRoZSBMaWNlbnNlIGlzIGRpc3RyaWJ1dGVkIG9uIGFuXHJcbi8vICBcIkFTLUlTXCIgQkFTSVMsIFdJVEhPVVQgV0FSUkFOVElFUyBPUiBDT05ESVRJT05TIE9GIEFOWSBLSU5ELCBlaXRoZXIgZXhwcmVzcyBvciBpbXBsaWVkLiBSZWZlciB0byB0aGVcclxuLy8gIExpY2Vuc2UgZm9yIHRoZSBzcGVjaWZpYyBsYW5ndWFnZSBnb3Zlcm5pbmcgcGVybWlzc2lvbnMgYW5kIGxpbWl0YXRpb25zLlxyXG4vLyAgXHJcbi8vICBodHRwczovL3N0YWNrb3ZlcmZsb3cuY29tL3F1ZXN0aW9ucy8xMDUwMzQvaG93LXRvLWNyZWF0ZS1hLWd1aWQtdXVpZFxyXG4vL1xyXG4vLyAgQ29kZSBNb2RpZmljYXRpb24gSGlzdG9yeTpcclxuLy8gIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuLy8gIDAxLzA0LzIwMjEgLSBCaWxseSBFcm5lc3RcclxuLy8gICAgICAgR2VuZXJhdGVkIG9yaWdpbmFsIHZlcnNpb24gb2Ygc291cmNlIGNvZGUuXHJcbi8vXHJcbi8vICoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJfX2VzTW9kdWxlXCIsIHsgdmFsdWU6IHRydWUgfSk7XHJcbmV4cG9ydHMuQ3JlYXRlR3VpZCA9IHZvaWQgMDtcclxuLyoqXHJcbiAqIFRoaXMgZnVuY3Rpb24gZ2VuZXJhdGVzIGEgR1VJRFxyXG4gKi9cclxuZnVuY3Rpb24gQ3JlYXRlR3VpZCgpIHtcclxuICAgIHJldHVybiAneHh4eHh4eHgteHh4eC00eHh4LXl4eHgteHh4eHh4eHh4eHh4Jy5yZXBsYWNlKC9beHldL2csIGZ1bmN0aW9uIChjKSB7XHJcbiAgICAgICAgdmFyIHIgPSBNYXRoLnJhbmRvbSgpICogMTYgfCAwO1xyXG4gICAgICAgIHZhciB2ID0gYyA9PT0gJ3gnID8gciA6IChyICYgMHgzIHwgMHg4KTtcclxuICAgICAgICByZXR1cm4gdi50b1N0cmluZygxNik7XHJcbiAgICB9KTtcclxufVxyXG5leHBvcnRzLkNyZWF0ZUd1aWQgPSBDcmVhdGVHdWlkO1xyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxuLy8gKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbi8vICBHZXROb2RlU2l6ZS50c3ggLSBHYnRjXHJcbi8vXHJcbi8vICBDb3B5cmlnaHQgwqkgMjAyMSwgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlLiAgQWxsIFJpZ2h0cyBSZXNlcnZlZC5cclxuLy9cclxuLy8gIExpY2Vuc2VkIHRvIHRoZSBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UgKEdQQSkgdW5kZXIgb25lIG9yIG1vcmUgY29udHJpYnV0b3IgbGljZW5zZSBhZ3JlZW1lbnRzLiBTZWVcclxuLy8gIHRoZSBOT1RJQ0UgZmlsZSBkaXN0cmlidXRlZCB3aXRoIHRoaXMgd29yayBmb3IgYWRkaXRpb25hbCBpbmZvcm1hdGlvbiByZWdhcmRpbmcgY29weXJpZ2h0IG93bmVyc2hpcC5cclxuLy8gIFRoZSBHUEEgbGljZW5zZXMgdGhpcyBmaWxlIHRvIHlvdSB1bmRlciB0aGUgTUlUIExpY2Vuc2UgKE1JVCksIHRoZSBcIkxpY2Vuc2VcIjsgeW91IG1heSBub3QgdXNlIHRoaXNcclxuLy8gIGZpbGUgZXhjZXB0IGluIGNvbXBsaWFuY2Ugd2l0aCB0aGUgTGljZW5zZS4gWW91IG1heSBvYnRhaW4gYSBjb3B5IG9mIHRoZSBMaWNlbnNlIGF0OlxyXG4vL1xyXG4vLyAgICAgIGh0dHA6Ly9vcGVuc291cmNlLm9yZy9saWNlbnNlcy9NSVRcclxuLy9cclxuLy8gIFVubGVzcyBhZ3JlZWQgdG8gaW4gd3JpdGluZywgdGhlIHN1YmplY3Qgc29mdHdhcmUgZGlzdHJpYnV0ZWQgdW5kZXIgdGhlIExpY2Vuc2UgaXMgZGlzdHJpYnV0ZWQgb24gYW5cclxuLy8gIFwiQVMtSVNcIiBCQVNJUywgV0lUSE9VVCBXQVJSQU5USUVTIE9SIENPTkRJVElPTlMgT0YgQU5ZIEtJTkQsIGVpdGhlciBleHByZXNzIG9yIGltcGxpZWQuIFJlZmVyIHRvIHRoZVxyXG4vLyAgTGljZW5zZSBmb3IgdGhlIHNwZWNpZmljIGxhbmd1YWdlIGdvdmVybmluZyBwZXJtaXNzaW9ucyBhbmQgbGltaXRhdGlvbnMuXHJcbi8vXHJcbi8vICBDb2RlIE1vZGlmaWNhdGlvbiBIaXN0b3J5OlxyXG4vLyAgLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG4vLyAgMDEvMTUvMjAyMSAtIEMuIExhY2tuZXJcclxuLy8gICAgICAgR2VuZXJhdGVkIG9yaWdpbmFsIHZlcnNpb24gb2Ygc291cmNlIGNvZGUuXHJcbi8vXHJcbi8vICoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJfX2VzTW9kdWxlXCIsIHsgdmFsdWU6IHRydWUgfSk7XHJcbmV4cG9ydHMuR2V0Tm9kZVNpemUgPSB2b2lkIDA7XHJcbi8qKlxyXG4gKiBHZXROb2RlU2l6ZSByZXR1cm5zIHRoZSBkaW1lbnNpb25zIG9mIGFuIGh0bWwgZWxlbWVudFxyXG4gKiBAcGFyYW0gbm9kZTogYSBIVE1MIGVsZW1lbnQsIG9yIG51bGwgY2FuIGJlIHBhc3NlZCB0aHJvdWdoXHJcbiAqL1xyXG5mdW5jdGlvbiBHZXROb2RlU2l6ZShub2RlKSB7XHJcbiAgICBpZiAobm9kZSA9PT0gbnVsbClcclxuICAgICAgICByZXR1cm4ge1xyXG4gICAgICAgICAgICBoZWlnaHQ6IDAsXHJcbiAgICAgICAgICAgIHdpZHRoOiAwLFxyXG4gICAgICAgICAgICB0b3A6IDAsXHJcbiAgICAgICAgICAgIGxlZnQ6IDAsXHJcbiAgICAgICAgfTtcclxuICAgIHZhciBfYSA9IG5vZGUuZ2V0Qm91bmRpbmdDbGllbnRSZWN0KCksIGhlaWdodCA9IF9hLmhlaWdodCwgd2lkdGggPSBfYS53aWR0aCwgdG9wID0gX2EudG9wLCBsZWZ0ID0gX2EubGVmdDtcclxuICAgIHJldHVybiB7XHJcbiAgICAgICAgaGVpZ2h0OiBwYXJzZUludChoZWlnaHQudG9TdHJpbmcoKSwgMTApLFxyXG4gICAgICAgIHdpZHRoOiBwYXJzZUludCh3aWR0aC50b1N0cmluZygpLCAxMCksXHJcbiAgICAgICAgdG9wOiBwYXJzZUludCh0b3AudG9TdHJpbmcoKSwgMTApLFxyXG4gICAgICAgIGxlZnQ6IHBhcnNlSW50KGxlZnQudG9TdHJpbmcoKSwgMTApLFxyXG4gICAgfTtcclxufVxyXG5leHBvcnRzLkdldE5vZGVTaXplID0gR2V0Tm9kZVNpemU7XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuLy8gIEdldFRleHRIZWlnaHQudHN4IC0gR2J0Y1xyXG4vL1xyXG4vLyAgQ29weXJpZ2h0IMKpIDIwMjEsIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZS4gIEFsbCBSaWdodHMgUmVzZXJ2ZWQuXHJcbi8vXHJcbi8vICBMaWNlbnNlZCB0byB0aGUgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlIChHUEEpIHVuZGVyIG9uZSBvciBtb3JlIGNvbnRyaWJ1dG9yIGxpY2Vuc2UgYWdyZWVtZW50cy4gU2VlXHJcbi8vICB0aGUgTk9USUNFIGZpbGUgZGlzdHJpYnV0ZWQgd2l0aCB0aGlzIHdvcmsgZm9yIGFkZGl0aW9uYWwgaW5mb3JtYXRpb24gcmVnYXJkaW5nIGNvcHlyaWdodCBvd25lcnNoaXAuXHJcbi8vICBUaGUgR1BBIGxpY2Vuc2VzIHRoaXMgZmlsZSB0byB5b3UgdW5kZXIgdGhlIE1JVCBMaWNlbnNlIChNSVQpLCB0aGUgXCJMaWNlbnNlXCI7IHlvdSBtYXkgbm90IHVzZSB0aGlzXHJcbi8vICBmaWxlIGV4Y2VwdCBpbiBjb21wbGlhbmNlIHdpdGggdGhlIExpY2Vuc2UuIFlvdSBtYXkgb2J0YWluIGEgY29weSBvZiB0aGUgTGljZW5zZSBhdDpcclxuLy9cclxuLy8gICAgICBodHRwOi8vb3BlbnNvdXJjZS5vcmcvbGljZW5zZXMvTUlUXHJcbi8vXHJcbi8vICBVbmxlc3MgYWdyZWVkIHRvIGluIHdyaXRpbmcsIHRoZSBzdWJqZWN0IHNvZnR3YXJlIGRpc3RyaWJ1dGVkIHVuZGVyIHRoZSBMaWNlbnNlIGlzIGRpc3RyaWJ1dGVkIG9uIGFuXHJcbi8vICBcIkFTLUlTXCIgQkFTSVMsIFdJVEhPVVQgV0FSUkFOVElFUyBPUiBDT05ESVRJT05TIE9GIEFOWSBLSU5ELCBlaXRoZXIgZXhwcmVzcyBvciBpbXBsaWVkLiBSZWZlciB0byB0aGVcclxuLy8gIExpY2Vuc2UgZm9yIHRoZSBzcGVjaWZpYyBsYW5ndWFnZSBnb3Zlcm5pbmcgcGVybWlzc2lvbnMgYW5kIGxpbWl0YXRpb25zLlxyXG4vL1xyXG4vLyAgQ29kZSBNb2RpZmljYXRpb24gSGlzdG9yeTpcclxuLy8gIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuLy8gIDAzLzEyLzIwMjEgLSBjLiBMYWNrbmVyXHJcbi8vICAgICAgIEdlbmVyYXRlZCBvcmlnaW5hbCB2ZXJzaW9uIG9mIHNvdXJjZSBjb2RlLlxyXG4vL1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuT2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFwiX19lc01vZHVsZVwiLCB7IHZhbHVlOiB0cnVlIH0pO1xyXG5leHBvcnRzLkdldFRleHRIZWlnaHQgPSB2b2lkIDA7XHJcbi8qKlxyXG4gKiBUaGlzIGZ1bmN0aW9uIHJldHVybnMgdGhlIGhlaWdodCBvZiBhIHBpZWNlIG9mIHRleHQgZ2l2ZW4gYSBmb250LCBmb250c2l6ZSwgYW5kIGEgd29yZFxyXG4gKiBAcGFyYW0gZm9udDogRGV0ZXJtaW5lcyBmb250IG9mIGdpdmVuIHRleHRcclxuICogQHBhcmFtIGZvbnRTaXplOiBEZXRlcm1pbmVzIHNpemUgb2YgZ2l2ZW4gZm9udFxyXG4gKiBAcGFyYW0gd29yZDogVGV4dCB0byBtZWFzdXJlXHJcbiAqL1xyXG5mdW5jdGlvbiBHZXRUZXh0SGVpZ2h0KGZvbnQsIGZvbnRTaXplLCB3b3JkKSB7XHJcbiAgICB2YXIgdGV4dCA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoXCJzcGFuXCIpO1xyXG4gICAgZG9jdW1lbnQuYm9keS5hcHBlbmRDaGlsZCh0ZXh0KTtcclxuICAgIHRleHQuc3R5bGUuZm9udCA9IGZvbnQ7XHJcbiAgICB0ZXh0LnN0eWxlLmZvbnRTaXplID0gZm9udFNpemU7XHJcbiAgICB0ZXh0LnN0eWxlLmhlaWdodCA9ICdhdXRvJztcclxuICAgIHRleHQuc3R5bGUud2lkdGggPSAnYXV0byc7XHJcbiAgICB0ZXh0LnN0eWxlLnBvc2l0aW9uID0gJ2Fic29sdXRlJztcclxuICAgIHRleHQuc3R5bGUud2hpdGVTcGFjZSA9ICduby13cmFwJztcclxuICAgIHRleHQuaW5uZXJIVE1MID0gd29yZDtcclxuICAgIHZhciBoZWlnaHQgPSBNYXRoLmNlaWwodGV4dC5jbGllbnRIZWlnaHQpO1xyXG4gICAgZG9jdW1lbnQuYm9keS5yZW1vdmVDaGlsZCh0ZXh0KTtcclxuICAgIHJldHVybiBoZWlnaHQ7XHJcbn1cclxuZXhwb3J0cy5HZXRUZXh0SGVpZ2h0ID0gR2V0VGV4dEhlaWdodDtcclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbi8vICoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG4vLyAgR2V0VGV4dFdpZHRoLnRzeCAtIEdidGNcclxuLy9cclxuLy8gIENvcHlyaWdodCDCqSAyMDIxLCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxyXG4vL1xyXG4vLyAgTGljZW5zZWQgdG8gdGhlIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZSAoR1BBKSB1bmRlciBvbmUgb3IgbW9yZSBjb250cmlidXRvciBsaWNlbnNlIGFncmVlbWVudHMuIFNlZVxyXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxyXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xyXG4vLyAgZmlsZSBleGNlcHQgaW4gY29tcGxpYW5jZSB3aXRoIHRoZSBMaWNlbnNlLiBZb3UgbWF5IG9idGFpbiBhIGNvcHkgb2YgdGhlIExpY2Vuc2UgYXQ6XHJcbi8vXHJcbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxyXG4vL1xyXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxyXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXHJcbi8vICBMaWNlbnNlIGZvciB0aGUgc3BlY2lmaWMgbGFuZ3VhZ2UgZ292ZXJuaW5nIHBlcm1pc3Npb25zIGFuZCBsaW1pdGF0aW9ucy5cclxuLy9cclxuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XHJcbi8vICAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vICAwMS8wNy8yMDIxIC0gQmlsbHkgRXJuZXN0XHJcbi8vICAgICAgIEdlbmVyYXRlZCBvcmlnaW5hbCB2ZXJzaW9uIG9mIHNvdXJjZSBjb2RlLlxyXG4vL1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuT2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFwiX19lc01vZHVsZVwiLCB7IHZhbHVlOiB0cnVlIH0pO1xyXG5leHBvcnRzLkdldFRleHRXaWR0aCA9IHZvaWQgMDtcclxuLyoqXHJcbiAqIEdldFRleHRXaWR0aCByZXR1cm5zIHRoZSB3aWR0aCBvZiBhIHBpZWNlIG9mIHRleHQgZ2l2ZW4gaXRzIGZvbnQsIGZvbnRTaXplLCBhbmQgY29udGVudC5cclxuICogQHBhcmFtIGZvbnQ6IERldGVybWluZXMgZm9udCBvZiBnaXZlbiB0ZXh0XHJcbiAqIEBwYXJhbSBmb250U2l6ZTogRGV0ZXJtaW5lcyBzaXplIG9mIGdpdmVuIGZvbnRcclxuICogQHBhcmFtIHdvcmQ6IFRleHQgdG8gbWVhc3VyZVxyXG4gKi9cclxuZnVuY3Rpb24gR2V0VGV4dFdpZHRoKGZvbnQsIGZvbnRTaXplLCB3b3JkKSB7XHJcbiAgICB2YXIgdGV4dCA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoXCJzcGFuXCIpO1xyXG4gICAgZG9jdW1lbnQuYm9keS5hcHBlbmRDaGlsZCh0ZXh0KTtcclxuICAgIHRleHQuc3R5bGUuZm9udCA9IGZvbnQ7XHJcbiAgICB0ZXh0LnN0eWxlLmZvbnRTaXplID0gZm9udFNpemU7XHJcbiAgICB0ZXh0LnN0eWxlLmhlaWdodCA9ICdhdXRvJztcclxuICAgIHRleHQuc3R5bGUud2lkdGggPSAnYXV0byc7XHJcbiAgICB0ZXh0LnN0eWxlLnBvc2l0aW9uID0gJ2Fic29sdXRlJztcclxuICAgIHRleHQuc3R5bGUud2hpdGVTcGFjZSA9ICduby13cmFwJztcclxuICAgIHRleHQuaW5uZXJIVE1MID0gd29yZDtcclxuICAgIHZhciB3aWR0aCA9IE1hdGguY2VpbCh0ZXh0LmNsaWVudFdpZHRoKTtcclxuICAgIGRvY3VtZW50LmJvZHkucmVtb3ZlQ2hpbGQodGV4dCk7XHJcbiAgICByZXR1cm4gd2lkdGg7XHJcbn1cclxuZXhwb3J0cy5HZXRUZXh0V2lkdGggPSBHZXRUZXh0V2lkdGg7XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuLy8gIElzSW50ZWdlci50c3ggLSBHYnRjXHJcbi8vXHJcbi8vICBDb3B5cmlnaHQgwqkgMjAyMSwgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlLiAgQWxsIFJpZ2h0cyBSZXNlcnZlZC5cclxuLy9cclxuLy8gIExpY2Vuc2VkIHRvIHRoZSBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UgKEdQQSkgdW5kZXIgb25lIG9yIG1vcmUgY29udHJpYnV0b3IgbGljZW5zZSBhZ3JlZW1lbnRzLiBTZWVcclxuLy8gIHRoZSBOT1RJQ0UgZmlsZSBkaXN0cmlidXRlZCB3aXRoIHRoaXMgd29yayBmb3IgYWRkaXRpb25hbCBpbmZvcm1hdGlvbiByZWdhcmRpbmcgY29weXJpZ2h0IG93bmVyc2hpcC5cclxuLy8gIFRoZSBHUEEgbGljZW5zZXMgdGhpcyBmaWxlIHRvIHlvdSB1bmRlciB0aGUgTUlUIExpY2Vuc2UgKE1JVCksIHRoZSBcIkxpY2Vuc2VcIjsgeW91IG1heSBub3QgdXNlIHRoaXNcclxuLy8gIGZpbGUgZXhjZXB0IGluIGNvbXBsaWFuY2Ugd2l0aCB0aGUgTGljZW5zZS4gWW91IG1heSBvYnRhaW4gYSBjb3B5IG9mIHRoZSBMaWNlbnNlIGF0OlxyXG4vL1xyXG4vLyAgICAgIGh0dHA6Ly9vcGVuc291cmNlLm9yZy9saWNlbnNlcy9NSVRcclxuLy9cclxuLy8gIFVubGVzcyBhZ3JlZWQgdG8gaW4gd3JpdGluZywgdGhlIHN1YmplY3Qgc29mdHdhcmUgZGlzdHJpYnV0ZWQgdW5kZXIgdGhlIExpY2Vuc2UgaXMgZGlzdHJpYnV0ZWQgb24gYW5cclxuLy8gIFwiQVMtSVNcIiBCQVNJUywgV0lUSE9VVCBXQVJSQU5USUVTIE9SIENPTkRJVElPTlMgT0YgQU5ZIEtJTkQsIGVpdGhlciBleHByZXNzIG9yIGltcGxpZWQuIFJlZmVyIHRvIHRoZVxyXG4vLyAgTGljZW5zZSBmb3IgdGhlIHNwZWNpZmljIGxhbmd1YWdlIGdvdmVybmluZyBwZXJtaXNzaW9ucyBhbmQgbGltaXRhdGlvbnMuXHJcbi8vXHJcbi8vICBDb2RlIE1vZGlmaWNhdGlvbiBIaXN0b3J5OlxyXG4vLyAgLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG4vLyAgMDcvMTYvMjAyMSAtIEMuIExhY2tuZXJcclxuLy8gICAgICAgR2VuZXJhdGVkIG9yaWdpbmFsIHZlcnNpb24gb2Ygc291cmNlIGNvZGUuXHJcbi8vXHJcbi8vICoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJfX2VzTW9kdWxlXCIsIHsgdmFsdWU6IHRydWUgfSk7XHJcbmV4cG9ydHMuSXNJbnRlZ2VyID0gdm9pZCAwO1xyXG4vKipcclxuICogSXNJbnRlZ2VyIGNoZWNrcyBpZiB2YWx1ZSBwYXNzZWQgdGhyb3VnaCBpcyBhbiBpbnRlZ2VyLCByZXR1cm5pbmcgYSB0cnVlIG9yIGZhbHNlXHJcbiAqIEBwYXJhbSB2YWx1ZTogdmFsdWUgaXMgdGhlIGlucHV0IHBhc3NlZCB0aHJvdWdoIHRoZSBJc0ludGVnZXIgZnVuY3Rpb25cclxuICovXHJcbmZ1bmN0aW9uIElzSW50ZWdlcih2YWx1ZSkge1xyXG4gICAgdmFyIHJlZ2V4ID0gL14tP1swLTldKyQvO1xyXG4gICAgcmV0dXJuIHZhbHVlLnRvU3RyaW5nKCkubWF0Y2gocmVnZXgpICE9IG51bGw7XHJcbn1cclxuZXhwb3J0cy5Jc0ludGVnZXIgPSBJc0ludGVnZXI7XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuLy8gIElzTnVtYmVyLnRzeCAtIEdidGNcclxuLy9cclxuLy8gIENvcHlyaWdodCDCqSAyMDIxLCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxyXG4vL1xyXG4vLyAgTGljZW5zZWQgdG8gdGhlIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZSAoR1BBKSB1bmRlciBvbmUgb3IgbW9yZSBjb250cmlidXRvciBsaWNlbnNlIGFncmVlbWVudHMuIFNlZVxyXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxyXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xyXG4vLyAgZmlsZSBleGNlcHQgaW4gY29tcGxpYW5jZSB3aXRoIHRoZSBMaWNlbnNlLiBZb3UgbWF5IG9idGFpbiBhIGNvcHkgb2YgdGhlIExpY2Vuc2UgYXQ6XHJcbi8vXHJcbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxyXG4vL1xyXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxyXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXHJcbi8vICBMaWNlbnNlIGZvciB0aGUgc3BlY2lmaWMgbGFuZ3VhZ2UgZ292ZXJuaW5nIHBlcm1pc3Npb25zIGFuZCBsaW1pdGF0aW9ucy5cclxuLy9cclxuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XHJcbi8vICAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vICAwMS8wNy8yMDIxIC0gQmlsbHkgRXJuZXN0XHJcbi8vICAgICAgIEdlbmVyYXRlZCBvcmlnaW5hbCB2ZXJzaW9uIG9mIHNvdXJjZSBjb2RlLlxyXG4vL1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuT2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFwiX19lc01vZHVsZVwiLCB7IHZhbHVlOiB0cnVlIH0pO1xyXG5leHBvcnRzLklzTnVtYmVyID0gdm9pZCAwO1xyXG4vKipcclxuICogVGhpcyBmdW5jdGlvbiBjaGVja3MgaWYgYW55IHZhbHVlIGlzIGEgbnVtYmVyLCByZXR1cm5pbmcgdHJ1ZSBvciBmYWxzZVxyXG4gKiBAcGFyYW0gdmFsdWU6IHZhbHVlIGlzIHRoZSBpbnB1dCBwYXNzZWQgdGhyb3VnaCB0aGUgSXNOdW1iZXIgZnVuY3Rpb25cclxuICovXHJcbmZ1bmN0aW9uIElzTnVtYmVyKHZhbHVlKSB7XHJcbiAgICB2YXIgcmVnZXggPSAvXi0/WzAtOV0rKFxcLlswLTldKyk/JC87XHJcbiAgICByZXR1cm4gdmFsdWUudG9TdHJpbmcoKS5tYXRjaChyZWdleCkgIT0gbnVsbDtcclxufVxyXG5leHBvcnRzLklzTnVtYmVyID0gSXNOdW1iZXI7XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuLy8gIFJhbmRvbUNvbG9yLnRzeCAtIEdidGNcclxuLy9cclxuLy8gIENvcHlyaWdodCDCqSAyMDIxLCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxyXG4vL1xyXG4vLyAgTGljZW5zZWQgdG8gdGhlIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZSAoR1BBKSB1bmRlciBvbmUgb3IgbW9yZSBjb250cmlidXRvciBsaWNlbnNlIGFncmVlbWVudHMuIFNlZVxyXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxyXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xyXG4vLyAgZmlsZSBleGNlcHQgaW4gY29tcGxpYW5jZSB3aXRoIHRoZSBMaWNlbnNlLiBZb3UgbWF5IG9idGFpbiBhIGNvcHkgb2YgdGhlIExpY2Vuc2UgYXQ6XHJcbi8vXHJcbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxyXG4vL1xyXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxyXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXHJcbi8vICBMaWNlbnNlIGZvciB0aGUgc3BlY2lmaWMgbGFuZ3VhZ2UgZ292ZXJuaW5nIHBlcm1pc3Npb25zIGFuZCBsaW1pdGF0aW9ucy5cclxuLy9cclxuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XHJcbi8vICAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vICAwMS8xNS8yMDIxIC0gQy4gTGFja25lclxyXG4vLyAgICAgICBHZW5lcmF0ZWQgb3JpZ2luYWwgdmVyc2lvbiBvZiBzb3VyY2UgY29kZS5cclxuLy9cclxuLy8gKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcIl9fZXNNb2R1bGVcIiwgeyB2YWx1ZTogdHJ1ZSB9KTtcclxuZXhwb3J0cy5SYW5kb21Db2xvciA9IHZvaWQgMDtcclxuLyoqXHJcbiAqIFRoaXMgZnVuY3Rpb24gcmV0dXJucyBhIHJhbmRvbSBjb2xvclxyXG4gKi9cclxuZnVuY3Rpb24gUmFuZG9tQ29sb3IoKSB7XHJcbiAgICByZXR1cm4gJyMnICsgTWF0aC5yYW5kb20oKS50b1N0cmluZygxNikuc3Vic3RyKDIsIDYpLnRvVXBwZXJDYXNlKCk7XHJcbn1cclxuZXhwb3J0cy5SYW5kb21Db2xvciA9IFJhbmRvbUNvbG9yO1xyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxuLy8gKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbi8vICBpbmRleC50cyAtIEdidGNcclxuLy9cclxuLy8gIENvcHlyaWdodCDvv70gMjAyMSwgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlLiAgQWxsIFJpZ2h0cyBSZXNlcnZlZC5cclxuLy9cclxuLy8gIExpY2Vuc2VkIHRvIHRoZSBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UgKEdQQSkgdW5kZXIgb25lIG9yIG1vcmUgY29udHJpYnV0b3IgbGljZW5zZSBhZ3JlZW1lbnRzLiBTZWVcclxuLy8gIHRoZSBOT1RJQ0UgZmlsZSBkaXN0cmlidXRlZCB3aXRoIHRoaXMgd29yayBmb3IgYWRkaXRpb25hbCBpbmZvcm1hdGlvbiByZWdhcmRpbmcgY29weXJpZ2h0IG93bmVyc2hpcC5cclxuLy8gIFRoZSBHUEEgbGljZW5zZXMgdGhpcyBmaWxlIHRvIHlvdSB1bmRlciB0aGUgTUlUIExpY2Vuc2UgKE1JVCksIHRoZSBcIkxpY2Vuc2VcIjsgeW91IG1heSBub3QgdXNlIHRoaXNcclxuLy8gIGZpbGUgZXhjZXB0IGluIGNvbXBsaWFuY2Ugd2l0aCB0aGUgTGljZW5zZS4gWW91IG1heSBvYnRhaW4gYSBjb3B5IG9mIHRoZSBMaWNlbnNlIGF0OlxyXG4vL1xyXG4vLyAgICAgIGh0dHA6Ly9vcGVuc291cmNlLm9yZy9saWNlbnNlcy9NSVRcclxuLy9cclxuLy8gIFVubGVzcyBhZ3JlZWQgdG8gaW4gd3JpdGluZywgdGhlIHN1YmplY3Qgc29mdHdhcmUgZGlzdHJpYnV0ZWQgdW5kZXIgdGhlIExpY2Vuc2UgaXMgZGlzdHJpYnV0ZWQgb24gYW5cclxuLy8gIFwiQVMtSVNcIiBCQVNJUywgV0lUSE9VVCBXQVJSQU5USUVTIE9SIENPTkRJVElPTlMgT0YgQU5ZIEtJTkQsIGVpdGhlciBleHByZXNzIG9yIGltcGxpZWQuIFJlZmVyIHRvIHRoZVxyXG4vLyAgTGljZW5zZSBmb3IgdGhlIHNwZWNpZmljIGxhbmd1YWdlIGdvdmVybmluZyBwZXJtaXNzaW9ucyBhbmQgbGltaXRhdGlvbnMuXHJcbi8vXHJcbi8vICBodHRwczovL3N0YWNrb3ZlcmZsb3cuY29tL3F1ZXN0aW9ucy8xMDUwMzQvaG93LXRvLWNyZWF0ZS1hLWd1aWQtdXVpZFxyXG4vL1xyXG4vLyAgQ29kZSBNb2RpZmljYXRpb24gSGlzdG9yeTpcclxuLy8gIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuLy8gIDAxLzA0LzIwMjEgLSBCaWxseSBFcm5lc3RcclxuLy8gICAgICAgR2VuZXJhdGVkIG9yaWdpbmFsIHZlcnNpb24gb2Ygc291cmNlIGNvZGUuXHJcbi8vXHJcbi8vICoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJfX2VzTW9kdWxlXCIsIHsgdmFsdWU6IHRydWUgfSk7XHJcbmV4cG9ydHMuSXNJbnRlZ2VyID0gZXhwb3J0cy5Jc051bWJlciA9IGV4cG9ydHMuR2V0VGV4dEhlaWdodCA9IGV4cG9ydHMuUmFuZG9tQ29sb3IgPSBleHBvcnRzLkdldE5vZGVTaXplID0gZXhwb3J0cy5HZXRUZXh0V2lkdGggPSBleHBvcnRzLkNyZWF0ZUd1aWQgPSB2b2lkIDA7XHJcbnZhciBDcmVhdGVHdWlkXzEgPSByZXF1aXJlKFwiLi9DcmVhdGVHdWlkXCIpO1xyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJDcmVhdGVHdWlkXCIsIHsgZW51bWVyYWJsZTogdHJ1ZSwgZ2V0OiBmdW5jdGlvbiAoKSB7IHJldHVybiBDcmVhdGVHdWlkXzEuQ3JlYXRlR3VpZDsgfSB9KTtcclxudmFyIEdldFRleHRXaWR0aF8xID0gcmVxdWlyZShcIi4vR2V0VGV4dFdpZHRoXCIpO1xyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJHZXRUZXh0V2lkdGhcIiwgeyBlbnVtZXJhYmxlOiB0cnVlLCBnZXQ6IGZ1bmN0aW9uICgpIHsgcmV0dXJuIEdldFRleHRXaWR0aF8xLkdldFRleHRXaWR0aDsgfSB9KTtcclxudmFyIEdldFRleHRIZWlnaHRfMSA9IHJlcXVpcmUoXCIuL0dldFRleHRIZWlnaHRcIik7XHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcIkdldFRleHRIZWlnaHRcIiwgeyBlbnVtZXJhYmxlOiB0cnVlLCBnZXQ6IGZ1bmN0aW9uICgpIHsgcmV0dXJuIEdldFRleHRIZWlnaHRfMS5HZXRUZXh0SGVpZ2h0OyB9IH0pO1xyXG52YXIgR2V0Tm9kZVNpemVfMSA9IHJlcXVpcmUoXCIuL0dldE5vZGVTaXplXCIpO1xyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJHZXROb2RlU2l6ZVwiLCB7IGVudW1lcmFibGU6IHRydWUsIGdldDogZnVuY3Rpb24gKCkgeyByZXR1cm4gR2V0Tm9kZVNpemVfMS5HZXROb2RlU2l6ZTsgfSB9KTtcclxudmFyIFJhbmRvbUNvbG9yXzEgPSByZXF1aXJlKFwiLi9SYW5kb21Db2xvclwiKTtcclxuT2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFwiUmFuZG9tQ29sb3JcIiwgeyBlbnVtZXJhYmxlOiB0cnVlLCBnZXQ6IGZ1bmN0aW9uICgpIHsgcmV0dXJuIFJhbmRvbUNvbG9yXzEuUmFuZG9tQ29sb3I7IH0gfSk7XHJcbnZhciBJc051bWJlcl8xID0gcmVxdWlyZShcIi4vSXNOdW1iZXJcIik7XHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcIklzTnVtYmVyXCIsIHsgZW51bWVyYWJsZTogdHJ1ZSwgZ2V0OiBmdW5jdGlvbiAoKSB7IHJldHVybiBJc051bWJlcl8xLklzTnVtYmVyOyB9IH0pO1xyXG52YXIgSXNJbnRlZ2VyXzEgPSByZXF1aXJlKFwiLi9Jc0ludGVnZXJcIik7XHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcIklzSW50ZWdlclwiLCB7IGVudW1lcmFibGU6IHRydWUsIGdldDogZnVuY3Rpb24gKCkgeyByZXR1cm4gSXNJbnRlZ2VyXzEuSXNJbnRlZ2VyOyB9IH0pO1xyXG4iLCIvKipcbiAqIENvcHlyaWdodCAoYykgMjAxMy1wcmVzZW50LCBGYWNlYm9vaywgSW5jLlxuICpcbiAqIFRoaXMgc291cmNlIGNvZGUgaXMgbGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlIGZvdW5kIGluIHRoZVxuICogTElDRU5TRSBmaWxlIGluIHRoZSByb290IGRpcmVjdG9yeSBvZiB0aGlzIHNvdXJjZSB0cmVlLlxuICpcbiAqL1xuXG4ndXNlIHN0cmljdCc7XG5cbnZhciBfYXNzaWduID0gcmVxdWlyZSgnb2JqZWN0LWFzc2lnbicpO1xuXG52YXIgZW1wdHlPYmplY3QgPSByZXF1aXJlKCdmYmpzL2xpYi9lbXB0eU9iamVjdCcpO1xudmFyIF9pbnZhcmlhbnQgPSByZXF1aXJlKCdmYmpzL2xpYi9pbnZhcmlhbnQnKTtcblxuaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgdmFyIHdhcm5pbmcgPSByZXF1aXJlKCdmYmpzL2xpYi93YXJuaW5nJyk7XG59XG5cbnZhciBNSVhJTlNfS0VZID0gJ21peGlucyc7XG5cbi8vIEhlbHBlciBmdW5jdGlvbiB0byBhbGxvdyB0aGUgY3JlYXRpb24gb2YgYW5vbnltb3VzIGZ1bmN0aW9ucyB3aGljaCBkbyBub3Rcbi8vIGhhdmUgLm5hbWUgc2V0IHRvIHRoZSBuYW1lIG9mIHRoZSB2YXJpYWJsZSBiZWluZyBhc3NpZ25lZCB0by5cbmZ1bmN0aW9uIGlkZW50aXR5KGZuKSB7XG4gIHJldHVybiBmbjtcbn1cblxudmFyIFJlYWN0UHJvcFR5cGVMb2NhdGlvbk5hbWVzO1xuaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgUmVhY3RQcm9wVHlwZUxvY2F0aW9uTmFtZXMgPSB7XG4gICAgcHJvcDogJ3Byb3AnLFxuICAgIGNvbnRleHQ6ICdjb250ZXh0JyxcbiAgICBjaGlsZENvbnRleHQ6ICdjaGlsZCBjb250ZXh0J1xuICB9O1xufSBlbHNlIHtcbiAgUmVhY3RQcm9wVHlwZUxvY2F0aW9uTmFtZXMgPSB7fTtcbn1cblxuZnVuY3Rpb24gZmFjdG9yeShSZWFjdENvbXBvbmVudCwgaXNWYWxpZEVsZW1lbnQsIFJlYWN0Tm9vcFVwZGF0ZVF1ZXVlKSB7XG4gIC8qKlxuICAgKiBQb2xpY2llcyB0aGF0IGRlc2NyaWJlIG1ldGhvZHMgaW4gYFJlYWN0Q2xhc3NJbnRlcmZhY2VgLlxuICAgKi9cblxuICB2YXIgaW5qZWN0ZWRNaXhpbnMgPSBbXTtcblxuICAvKipcbiAgICogQ29tcG9zaXRlIGNvbXBvbmVudHMgYXJlIGhpZ2hlci1sZXZlbCBjb21wb25lbnRzIHRoYXQgY29tcG9zZSBvdGhlciBjb21wb3NpdGVcbiAgICogb3IgaG9zdCBjb21wb25lbnRzLlxuICAgKlxuICAgKiBUbyBjcmVhdGUgYSBuZXcgdHlwZSBvZiBgUmVhY3RDbGFzc2AsIHBhc3MgYSBzcGVjaWZpY2F0aW9uIG9mXG4gICAqIHlvdXIgbmV3IGNsYXNzIHRvIGBSZWFjdC5jcmVhdGVDbGFzc2AuIFRoZSBvbmx5IHJlcXVpcmVtZW50IG9mIHlvdXIgY2xhc3NcbiAgICogc3BlY2lmaWNhdGlvbiBpcyB0aGF0IHlvdSBpbXBsZW1lbnQgYSBgcmVuZGVyYCBtZXRob2QuXG4gICAqXG4gICAqICAgdmFyIE15Q29tcG9uZW50ID0gUmVhY3QuY3JlYXRlQ2xhc3Moe1xuICAgKiAgICAgcmVuZGVyOiBmdW5jdGlvbigpIHtcbiAgICogICAgICAgcmV0dXJuIDxkaXY+SGVsbG8gV29ybGQ8L2Rpdj47XG4gICAqICAgICB9XG4gICAqICAgfSk7XG4gICAqXG4gICAqIFRoZSBjbGFzcyBzcGVjaWZpY2F0aW9uIHN1cHBvcnRzIGEgc3BlY2lmaWMgcHJvdG9jb2wgb2YgbWV0aG9kcyB0aGF0IGhhdmVcbiAgICogc3BlY2lhbCBtZWFuaW5nIChlLmcuIGByZW5kZXJgKS4gU2VlIGBSZWFjdENsYXNzSW50ZXJmYWNlYCBmb3JcbiAgICogbW9yZSB0aGUgY29tcHJlaGVuc2l2ZSBwcm90b2NvbC4gQW55IG90aGVyIHByb3BlcnRpZXMgYW5kIG1ldGhvZHMgaW4gdGhlXG4gICAqIGNsYXNzIHNwZWNpZmljYXRpb24gd2lsbCBiZSBhdmFpbGFibGUgb24gdGhlIHByb3RvdHlwZS5cbiAgICpcbiAgICogQGludGVyZmFjZSBSZWFjdENsYXNzSW50ZXJmYWNlXG4gICAqIEBpbnRlcm5hbFxuICAgKi9cbiAgdmFyIFJlYWN0Q2xhc3NJbnRlcmZhY2UgPSB7XG4gICAgLyoqXG4gICAgICogQW4gYXJyYXkgb2YgTWl4aW4gb2JqZWN0cyB0byBpbmNsdWRlIHdoZW4gZGVmaW5pbmcgeW91ciBjb21wb25lbnQuXG4gICAgICpcbiAgICAgKiBAdHlwZSB7YXJyYXl9XG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgbWl4aW5zOiAnREVGSU5FX01BTlknLFxuXG4gICAgLyoqXG4gICAgICogQW4gb2JqZWN0IGNvbnRhaW5pbmcgcHJvcGVydGllcyBhbmQgbWV0aG9kcyB0aGF0IHNob3VsZCBiZSBkZWZpbmVkIG9uXG4gICAgICogdGhlIGNvbXBvbmVudCdzIGNvbnN0cnVjdG9yIGluc3RlYWQgb2YgaXRzIHByb3RvdHlwZSAoc3RhdGljIG1ldGhvZHMpLlxuICAgICAqXG4gICAgICogQHR5cGUge29iamVjdH1cbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBzdGF0aWNzOiAnREVGSU5FX01BTlknLFxuXG4gICAgLyoqXG4gICAgICogRGVmaW5pdGlvbiBvZiBwcm9wIHR5cGVzIGZvciB0aGlzIGNvbXBvbmVudC5cbiAgICAgKlxuICAgICAqIEB0eXBlIHtvYmplY3R9XG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgcHJvcFR5cGVzOiAnREVGSU5FX01BTlknLFxuXG4gICAgLyoqXG4gICAgICogRGVmaW5pdGlvbiBvZiBjb250ZXh0IHR5cGVzIGZvciB0aGlzIGNvbXBvbmVudC5cbiAgICAgKlxuICAgICAqIEB0eXBlIHtvYmplY3R9XG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgY29udGV4dFR5cGVzOiAnREVGSU5FX01BTlknLFxuXG4gICAgLyoqXG4gICAgICogRGVmaW5pdGlvbiBvZiBjb250ZXh0IHR5cGVzIHRoaXMgY29tcG9uZW50IHNldHMgZm9yIGl0cyBjaGlsZHJlbi5cbiAgICAgKlxuICAgICAqIEB0eXBlIHtvYmplY3R9XG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgY2hpbGRDb250ZXh0VHlwZXM6ICdERUZJTkVfTUFOWScsXG5cbiAgICAvLyA9PT09IERlZmluaXRpb24gbWV0aG9kcyA9PT09XG5cbiAgICAvKipcbiAgICAgKiBJbnZva2VkIHdoZW4gdGhlIGNvbXBvbmVudCBpcyBtb3VudGVkLiBWYWx1ZXMgaW4gdGhlIG1hcHBpbmcgd2lsbCBiZSBzZXQgb25cbiAgICAgKiBgdGhpcy5wcm9wc2AgaWYgdGhhdCBwcm9wIGlzIG5vdCBzcGVjaWZpZWQgKGkuZS4gdXNpbmcgYW4gYGluYCBjaGVjaykuXG4gICAgICpcbiAgICAgKiBUaGlzIG1ldGhvZCBpcyBpbnZva2VkIGJlZm9yZSBgZ2V0SW5pdGlhbFN0YXRlYCBhbmQgdGhlcmVmb3JlIGNhbm5vdCByZWx5XG4gICAgICogb24gYHRoaXMuc3RhdGVgIG9yIHVzZSBgdGhpcy5zZXRTdGF0ZWAuXG4gICAgICpcbiAgICAgKiBAcmV0dXJuIHtvYmplY3R9XG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgZ2V0RGVmYXVsdFByb3BzOiAnREVGSU5FX01BTllfTUVSR0VEJyxcblxuICAgIC8qKlxuICAgICAqIEludm9rZWQgb25jZSBiZWZvcmUgdGhlIGNvbXBvbmVudCBpcyBtb3VudGVkLiBUaGUgcmV0dXJuIHZhbHVlIHdpbGwgYmUgdXNlZFxuICAgICAqIGFzIHRoZSBpbml0aWFsIHZhbHVlIG9mIGB0aGlzLnN0YXRlYC5cbiAgICAgKlxuICAgICAqICAgZ2V0SW5pdGlhbFN0YXRlOiBmdW5jdGlvbigpIHtcbiAgICAgKiAgICAgcmV0dXJuIHtcbiAgICAgKiAgICAgICBpc09uOiBmYWxzZSxcbiAgICAgKiAgICAgICBmb29CYXo6IG5ldyBCYXpGb28oKVxuICAgICAqICAgICB9XG4gICAgICogICB9XG4gICAgICpcbiAgICAgKiBAcmV0dXJuIHtvYmplY3R9XG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgZ2V0SW5pdGlhbFN0YXRlOiAnREVGSU5FX01BTllfTUVSR0VEJyxcblxuICAgIC8qKlxuICAgICAqIEByZXR1cm4ge29iamVjdH1cbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBnZXRDaGlsZENvbnRleHQ6ICdERUZJTkVfTUFOWV9NRVJHRUQnLFxuXG4gICAgLyoqXG4gICAgICogVXNlcyBwcm9wcyBmcm9tIGB0aGlzLnByb3BzYCBhbmQgc3RhdGUgZnJvbSBgdGhpcy5zdGF0ZWAgdG8gcmVuZGVyIHRoZVxuICAgICAqIHN0cnVjdHVyZSBvZiB0aGUgY29tcG9uZW50LlxuICAgICAqXG4gICAgICogTm8gZ3VhcmFudGVlcyBhcmUgbWFkZSBhYm91dCB3aGVuIG9yIGhvdyBvZnRlbiB0aGlzIG1ldGhvZCBpcyBpbnZva2VkLCBzb1xuICAgICAqIGl0IG11c3Qgbm90IGhhdmUgc2lkZSBlZmZlY3RzLlxuICAgICAqXG4gICAgICogICByZW5kZXI6IGZ1bmN0aW9uKCkge1xuICAgICAqICAgICB2YXIgbmFtZSA9IHRoaXMucHJvcHMubmFtZTtcbiAgICAgKiAgICAgcmV0dXJuIDxkaXY+SGVsbG8sIHtuYW1lfSE8L2Rpdj47XG4gICAgICogICB9XG4gICAgICpcbiAgICAgKiBAcmV0dXJuIHtSZWFjdENvbXBvbmVudH1cbiAgICAgKiBAcmVxdWlyZWRcbiAgICAgKi9cbiAgICByZW5kZXI6ICdERUZJTkVfT05DRScsXG5cbiAgICAvLyA9PT09IERlbGVnYXRlIG1ldGhvZHMgPT09PVxuXG4gICAgLyoqXG4gICAgICogSW52b2tlZCB3aGVuIHRoZSBjb21wb25lbnQgaXMgaW5pdGlhbGx5IGNyZWF0ZWQgYW5kIGFib3V0IHRvIGJlIG1vdW50ZWQuXG4gICAgICogVGhpcyBtYXkgaGF2ZSBzaWRlIGVmZmVjdHMsIGJ1dCBhbnkgZXh0ZXJuYWwgc3Vic2NyaXB0aW9ucyBvciBkYXRhIGNyZWF0ZWRcbiAgICAgKiBieSB0aGlzIG1ldGhvZCBtdXN0IGJlIGNsZWFuZWQgdXAgaW4gYGNvbXBvbmVudFdpbGxVbm1vdW50YC5cbiAgICAgKlxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIGNvbXBvbmVudFdpbGxNb3VudDogJ0RFRklORV9NQU5ZJyxcblxuICAgIC8qKlxuICAgICAqIEludm9rZWQgd2hlbiB0aGUgY29tcG9uZW50IGhhcyBiZWVuIG1vdW50ZWQgYW5kIGhhcyBhIERPTSByZXByZXNlbnRhdGlvbi5cbiAgICAgKiBIb3dldmVyLCB0aGVyZSBpcyBubyBndWFyYW50ZWUgdGhhdCB0aGUgRE9NIG5vZGUgaXMgaW4gdGhlIGRvY3VtZW50LlxuICAgICAqXG4gICAgICogVXNlIHRoaXMgYXMgYW4gb3Bwb3J0dW5pdHkgdG8gb3BlcmF0ZSBvbiB0aGUgRE9NIHdoZW4gdGhlIGNvbXBvbmVudCBoYXNcbiAgICAgKiBiZWVuIG1vdW50ZWQgKGluaXRpYWxpemVkIGFuZCByZW5kZXJlZCkgZm9yIHRoZSBmaXJzdCB0aW1lLlxuICAgICAqXG4gICAgICogQHBhcmFtIHtET01FbGVtZW50fSByb290Tm9kZSBET00gZWxlbWVudCByZXByZXNlbnRpbmcgdGhlIGNvbXBvbmVudC5cbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBjb21wb25lbnREaWRNb3VudDogJ0RFRklORV9NQU5ZJyxcblxuICAgIC8qKlxuICAgICAqIEludm9rZWQgYmVmb3JlIHRoZSBjb21wb25lbnQgcmVjZWl2ZXMgbmV3IHByb3BzLlxuICAgICAqXG4gICAgICogVXNlIHRoaXMgYXMgYW4gb3Bwb3J0dW5pdHkgdG8gcmVhY3QgdG8gYSBwcm9wIHRyYW5zaXRpb24gYnkgdXBkYXRpbmcgdGhlXG4gICAgICogc3RhdGUgdXNpbmcgYHRoaXMuc2V0U3RhdGVgLiBDdXJyZW50IHByb3BzIGFyZSBhY2Nlc3NlZCB2aWEgYHRoaXMucHJvcHNgLlxuICAgICAqXG4gICAgICogICBjb21wb25lbnRXaWxsUmVjZWl2ZVByb3BzOiBmdW5jdGlvbihuZXh0UHJvcHMsIG5leHRDb250ZXh0KSB7XG4gICAgICogICAgIHRoaXMuc2V0U3RhdGUoe1xuICAgICAqICAgICAgIGxpa2VzSW5jcmVhc2luZzogbmV4dFByb3BzLmxpa2VDb3VudCA+IHRoaXMucHJvcHMubGlrZUNvdW50XG4gICAgICogICAgIH0pO1xuICAgICAqICAgfVxuICAgICAqXG4gICAgICogTk9URTogVGhlcmUgaXMgbm8gZXF1aXZhbGVudCBgY29tcG9uZW50V2lsbFJlY2VpdmVTdGF0ZWAuIEFuIGluY29taW5nIHByb3BcbiAgICAgKiB0cmFuc2l0aW9uIG1heSBjYXVzZSBhIHN0YXRlIGNoYW5nZSwgYnV0IHRoZSBvcHBvc2l0ZSBpcyBub3QgdHJ1ZS4gSWYgeW91XG4gICAgICogbmVlZCBpdCwgeW91IGFyZSBwcm9iYWJseSBsb29raW5nIGZvciBgY29tcG9uZW50V2lsbFVwZGF0ZWAuXG4gICAgICpcbiAgICAgKiBAcGFyYW0ge29iamVjdH0gbmV4dFByb3BzXG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgY29tcG9uZW50V2lsbFJlY2VpdmVQcm9wczogJ0RFRklORV9NQU5ZJyxcblxuICAgIC8qKlxuICAgICAqIEludm9rZWQgd2hpbGUgZGVjaWRpbmcgaWYgdGhlIGNvbXBvbmVudCBzaG91bGQgYmUgdXBkYXRlZCBhcyBhIHJlc3VsdCBvZlxuICAgICAqIHJlY2VpdmluZyBuZXcgcHJvcHMsIHN0YXRlIGFuZC9vciBjb250ZXh0LlxuICAgICAqXG4gICAgICogVXNlIHRoaXMgYXMgYW4gb3Bwb3J0dW5pdHkgdG8gYHJldHVybiBmYWxzZWAgd2hlbiB5b3UncmUgY2VydGFpbiB0aGF0IHRoZVxuICAgICAqIHRyYW5zaXRpb24gdG8gdGhlIG5ldyBwcm9wcy9zdGF0ZS9jb250ZXh0IHdpbGwgbm90IHJlcXVpcmUgYSBjb21wb25lbnRcbiAgICAgKiB1cGRhdGUuXG4gICAgICpcbiAgICAgKiAgIHNob3VsZENvbXBvbmVudFVwZGF0ZTogZnVuY3Rpb24obmV4dFByb3BzLCBuZXh0U3RhdGUsIG5leHRDb250ZXh0KSB7XG4gICAgICogICAgIHJldHVybiAhZXF1YWwobmV4dFByb3BzLCB0aGlzLnByb3BzKSB8fFxuICAgICAqICAgICAgICFlcXVhbChuZXh0U3RhdGUsIHRoaXMuc3RhdGUpIHx8XG4gICAgICogICAgICAgIWVxdWFsKG5leHRDb250ZXh0LCB0aGlzLmNvbnRleHQpO1xuICAgICAqICAgfVxuICAgICAqXG4gICAgICogQHBhcmFtIHtvYmplY3R9IG5leHRQcm9wc1xuICAgICAqIEBwYXJhbSB7P29iamVjdH0gbmV4dFN0YXRlXG4gICAgICogQHBhcmFtIHs/b2JqZWN0fSBuZXh0Q29udGV4dFxuICAgICAqIEByZXR1cm4ge2Jvb2xlYW59IFRydWUgaWYgdGhlIGNvbXBvbmVudCBzaG91bGQgdXBkYXRlLlxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIHNob3VsZENvbXBvbmVudFVwZGF0ZTogJ0RFRklORV9PTkNFJyxcblxuICAgIC8qKlxuICAgICAqIEludm9rZWQgd2hlbiB0aGUgY29tcG9uZW50IGlzIGFib3V0IHRvIHVwZGF0ZSBkdWUgdG8gYSB0cmFuc2l0aW9uIGZyb21cbiAgICAgKiBgdGhpcy5wcm9wc2AsIGB0aGlzLnN0YXRlYCBhbmQgYHRoaXMuY29udGV4dGAgdG8gYG5leHRQcm9wc2AsIGBuZXh0U3RhdGVgXG4gICAgICogYW5kIGBuZXh0Q29udGV4dGAuXG4gICAgICpcbiAgICAgKiBVc2UgdGhpcyBhcyBhbiBvcHBvcnR1bml0eSB0byBwZXJmb3JtIHByZXBhcmF0aW9uIGJlZm9yZSBhbiB1cGRhdGUgb2NjdXJzLlxuICAgICAqXG4gICAgICogTk9URTogWW91ICoqY2Fubm90KiogdXNlIGB0aGlzLnNldFN0YXRlKClgIGluIHRoaXMgbWV0aG9kLlxuICAgICAqXG4gICAgICogQHBhcmFtIHtvYmplY3R9IG5leHRQcm9wc1xuICAgICAqIEBwYXJhbSB7P29iamVjdH0gbmV4dFN0YXRlXG4gICAgICogQHBhcmFtIHs/b2JqZWN0fSBuZXh0Q29udGV4dFxuICAgICAqIEBwYXJhbSB7UmVhY3RSZWNvbmNpbGVUcmFuc2FjdGlvbn0gdHJhbnNhY3Rpb25cbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBjb21wb25lbnRXaWxsVXBkYXRlOiAnREVGSU5FX01BTlknLFxuXG4gICAgLyoqXG4gICAgICogSW52b2tlZCB3aGVuIHRoZSBjb21wb25lbnQncyBET00gcmVwcmVzZW50YXRpb24gaGFzIGJlZW4gdXBkYXRlZC5cbiAgICAgKlxuICAgICAqIFVzZSB0aGlzIGFzIGFuIG9wcG9ydHVuaXR5IHRvIG9wZXJhdGUgb24gdGhlIERPTSB3aGVuIHRoZSBjb21wb25lbnQgaGFzXG4gICAgICogYmVlbiB1cGRhdGVkLlxuICAgICAqXG4gICAgICogQHBhcmFtIHtvYmplY3R9IHByZXZQcm9wc1xuICAgICAqIEBwYXJhbSB7P29iamVjdH0gcHJldlN0YXRlXG4gICAgICogQHBhcmFtIHs/b2JqZWN0fSBwcmV2Q29udGV4dFxuICAgICAqIEBwYXJhbSB7RE9NRWxlbWVudH0gcm9vdE5vZGUgRE9NIGVsZW1lbnQgcmVwcmVzZW50aW5nIHRoZSBjb21wb25lbnQuXG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgY29tcG9uZW50RGlkVXBkYXRlOiAnREVGSU5FX01BTlknLFxuXG4gICAgLyoqXG4gICAgICogSW52b2tlZCB3aGVuIHRoZSBjb21wb25lbnQgaXMgYWJvdXQgdG8gYmUgcmVtb3ZlZCBmcm9tIGl0cyBwYXJlbnQgYW5kIGhhdmVcbiAgICAgKiBpdHMgRE9NIHJlcHJlc2VudGF0aW9uIGRlc3Ryb3llZC5cbiAgICAgKlxuICAgICAqIFVzZSB0aGlzIGFzIGFuIG9wcG9ydHVuaXR5IHRvIGRlYWxsb2NhdGUgYW55IGV4dGVybmFsIHJlc291cmNlcy5cbiAgICAgKlxuICAgICAqIE5PVEU6IFRoZXJlIGlzIG5vIGBjb21wb25lbnREaWRVbm1vdW50YCBzaW5jZSB5b3VyIGNvbXBvbmVudCB3aWxsIGhhdmUgYmVlblxuICAgICAqIGRlc3Ryb3llZCBieSB0aGF0IHBvaW50LlxuICAgICAqXG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgY29tcG9uZW50V2lsbFVubW91bnQ6ICdERUZJTkVfTUFOWScsXG5cbiAgICAvKipcbiAgICAgKiBSZXBsYWNlbWVudCBmb3IgKGRlcHJlY2F0ZWQpIGBjb21wb25lbnRXaWxsTW91bnRgLlxuICAgICAqXG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgVU5TQUZFX2NvbXBvbmVudFdpbGxNb3VudDogJ0RFRklORV9NQU5ZJyxcblxuICAgIC8qKlxuICAgICAqIFJlcGxhY2VtZW50IGZvciAoZGVwcmVjYXRlZCkgYGNvbXBvbmVudFdpbGxSZWNlaXZlUHJvcHNgLlxuICAgICAqXG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgVU5TQUZFX2NvbXBvbmVudFdpbGxSZWNlaXZlUHJvcHM6ICdERUZJTkVfTUFOWScsXG5cbiAgICAvKipcbiAgICAgKiBSZXBsYWNlbWVudCBmb3IgKGRlcHJlY2F0ZWQpIGBjb21wb25lbnRXaWxsVXBkYXRlYC5cbiAgICAgKlxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIFVOU0FGRV9jb21wb25lbnRXaWxsVXBkYXRlOiAnREVGSU5FX01BTlknLFxuXG4gICAgLy8gPT09PSBBZHZhbmNlZCBtZXRob2RzID09PT1cblxuICAgIC8qKlxuICAgICAqIFVwZGF0ZXMgdGhlIGNvbXBvbmVudCdzIGN1cnJlbnRseSBtb3VudGVkIERPTSByZXByZXNlbnRhdGlvbi5cbiAgICAgKlxuICAgICAqIEJ5IGRlZmF1bHQsIHRoaXMgaW1wbGVtZW50cyBSZWFjdCdzIHJlbmRlcmluZyBhbmQgcmVjb25jaWxpYXRpb24gYWxnb3JpdGhtLlxuICAgICAqIFNvcGhpc3RpY2F0ZWQgY2xpZW50cyBtYXkgd2lzaCB0byBvdmVycmlkZSB0aGlzLlxuICAgICAqXG4gICAgICogQHBhcmFtIHtSZWFjdFJlY29uY2lsZVRyYW5zYWN0aW9ufSB0cmFuc2FjdGlvblxuICAgICAqIEBpbnRlcm5hbFxuICAgICAqIEBvdmVycmlkYWJsZVxuICAgICAqL1xuICAgIHVwZGF0ZUNvbXBvbmVudDogJ09WRVJSSURFX0JBU0UnXG4gIH07XG5cbiAgLyoqXG4gICAqIFNpbWlsYXIgdG8gUmVhY3RDbGFzc0ludGVyZmFjZSBidXQgZm9yIHN0YXRpYyBtZXRob2RzLlxuICAgKi9cbiAgdmFyIFJlYWN0Q2xhc3NTdGF0aWNJbnRlcmZhY2UgPSB7XG4gICAgLyoqXG4gICAgICogVGhpcyBtZXRob2QgaXMgaW52b2tlZCBhZnRlciBhIGNvbXBvbmVudCBpcyBpbnN0YW50aWF0ZWQgYW5kIHdoZW4gaXRcbiAgICAgKiByZWNlaXZlcyBuZXcgcHJvcHMuIFJldHVybiBhbiBvYmplY3QgdG8gdXBkYXRlIHN0YXRlIGluIHJlc3BvbnNlIHRvXG4gICAgICogcHJvcCBjaGFuZ2VzLiBSZXR1cm4gbnVsbCB0byBpbmRpY2F0ZSBubyBjaGFuZ2UgdG8gc3RhdGUuXG4gICAgICpcbiAgICAgKiBJZiBhbiBvYmplY3QgaXMgcmV0dXJuZWQsIGl0cyBrZXlzIHdpbGwgYmUgbWVyZ2VkIGludG8gdGhlIGV4aXN0aW5nIHN0YXRlLlxuICAgICAqXG4gICAgICogQHJldHVybiB7b2JqZWN0IHx8IG51bGx9XG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgZ2V0RGVyaXZlZFN0YXRlRnJvbVByb3BzOiAnREVGSU5FX01BTllfTUVSR0VEJ1xuICB9O1xuXG4gIC8qKlxuICAgKiBNYXBwaW5nIGZyb20gY2xhc3Mgc3BlY2lmaWNhdGlvbiBrZXlzIHRvIHNwZWNpYWwgcHJvY2Vzc2luZyBmdW5jdGlvbnMuXG4gICAqXG4gICAqIEFsdGhvdWdoIHRoZXNlIGFyZSBkZWNsYXJlZCBsaWtlIGluc3RhbmNlIHByb3BlcnRpZXMgaW4gdGhlIHNwZWNpZmljYXRpb25cbiAgICogd2hlbiBkZWZpbmluZyBjbGFzc2VzIHVzaW5nIGBSZWFjdC5jcmVhdGVDbGFzc2AsIHRoZXkgYXJlIGFjdHVhbGx5IHN0YXRpY1xuICAgKiBhbmQgYXJlIGFjY2Vzc2libGUgb24gdGhlIGNvbnN0cnVjdG9yIGluc3RlYWQgb2YgdGhlIHByb3RvdHlwZS4gRGVzcGl0ZVxuICAgKiBiZWluZyBzdGF0aWMsIHRoZXkgbXVzdCBiZSBkZWZpbmVkIG91dHNpZGUgb2YgdGhlIFwic3RhdGljc1wiIGtleSB1bmRlclxuICAgKiB3aGljaCBhbGwgb3RoZXIgc3RhdGljIG1ldGhvZHMgYXJlIGRlZmluZWQuXG4gICAqL1xuICB2YXIgUkVTRVJWRURfU1BFQ19LRVlTID0ge1xuICAgIGRpc3BsYXlOYW1lOiBmdW5jdGlvbihDb25zdHJ1Y3RvciwgZGlzcGxheU5hbWUpIHtcbiAgICAgIENvbnN0cnVjdG9yLmRpc3BsYXlOYW1lID0gZGlzcGxheU5hbWU7XG4gICAgfSxcbiAgICBtaXhpbnM6IGZ1bmN0aW9uKENvbnN0cnVjdG9yLCBtaXhpbnMpIHtcbiAgICAgIGlmIChtaXhpbnMpIHtcbiAgICAgICAgZm9yICh2YXIgaSA9IDA7IGkgPCBtaXhpbnMubGVuZ3RoOyBpKyspIHtcbiAgICAgICAgICBtaXhTcGVjSW50b0NvbXBvbmVudChDb25zdHJ1Y3RvciwgbWl4aW5zW2ldKTtcbiAgICAgICAgfVxuICAgICAgfVxuICAgIH0sXG4gICAgY2hpbGRDb250ZXh0VHlwZXM6IGZ1bmN0aW9uKENvbnN0cnVjdG9yLCBjaGlsZENvbnRleHRUeXBlcykge1xuICAgICAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICAgICAgdmFsaWRhdGVUeXBlRGVmKENvbnN0cnVjdG9yLCBjaGlsZENvbnRleHRUeXBlcywgJ2NoaWxkQ29udGV4dCcpO1xuICAgICAgfVxuICAgICAgQ29uc3RydWN0b3IuY2hpbGRDb250ZXh0VHlwZXMgPSBfYXNzaWduKFxuICAgICAgICB7fSxcbiAgICAgICAgQ29uc3RydWN0b3IuY2hpbGRDb250ZXh0VHlwZXMsXG4gICAgICAgIGNoaWxkQ29udGV4dFR5cGVzXG4gICAgICApO1xuICAgIH0sXG4gICAgY29udGV4dFR5cGVzOiBmdW5jdGlvbihDb25zdHJ1Y3RvciwgY29udGV4dFR5cGVzKSB7XG4gICAgICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgICAgICB2YWxpZGF0ZVR5cGVEZWYoQ29uc3RydWN0b3IsIGNvbnRleHRUeXBlcywgJ2NvbnRleHQnKTtcbiAgICAgIH1cbiAgICAgIENvbnN0cnVjdG9yLmNvbnRleHRUeXBlcyA9IF9hc3NpZ24oXG4gICAgICAgIHt9LFxuICAgICAgICBDb25zdHJ1Y3Rvci5jb250ZXh0VHlwZXMsXG4gICAgICAgIGNvbnRleHRUeXBlc1xuICAgICAgKTtcbiAgICB9LFxuICAgIC8qKlxuICAgICAqIFNwZWNpYWwgY2FzZSBnZXREZWZhdWx0UHJvcHMgd2hpY2ggc2hvdWxkIG1vdmUgaW50byBzdGF0aWNzIGJ1dCByZXF1aXJlc1xuICAgICAqIGF1dG9tYXRpYyBtZXJnaW5nLlxuICAgICAqL1xuICAgIGdldERlZmF1bHRQcm9wczogZnVuY3Rpb24oQ29uc3RydWN0b3IsIGdldERlZmF1bHRQcm9wcykge1xuICAgICAgaWYgKENvbnN0cnVjdG9yLmdldERlZmF1bHRQcm9wcykge1xuICAgICAgICBDb25zdHJ1Y3Rvci5nZXREZWZhdWx0UHJvcHMgPSBjcmVhdGVNZXJnZWRSZXN1bHRGdW5jdGlvbihcbiAgICAgICAgICBDb25zdHJ1Y3Rvci5nZXREZWZhdWx0UHJvcHMsXG4gICAgICAgICAgZ2V0RGVmYXVsdFByb3BzXG4gICAgICAgICk7XG4gICAgICB9IGVsc2Uge1xuICAgICAgICBDb25zdHJ1Y3Rvci5nZXREZWZhdWx0UHJvcHMgPSBnZXREZWZhdWx0UHJvcHM7XG4gICAgICB9XG4gICAgfSxcbiAgICBwcm9wVHlwZXM6IGZ1bmN0aW9uKENvbnN0cnVjdG9yLCBwcm9wVHlwZXMpIHtcbiAgICAgIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgICAgIHZhbGlkYXRlVHlwZURlZihDb25zdHJ1Y3RvciwgcHJvcFR5cGVzLCAncHJvcCcpO1xuICAgICAgfVxuICAgICAgQ29uc3RydWN0b3IucHJvcFR5cGVzID0gX2Fzc2lnbih7fSwgQ29uc3RydWN0b3IucHJvcFR5cGVzLCBwcm9wVHlwZXMpO1xuICAgIH0sXG4gICAgc3RhdGljczogZnVuY3Rpb24oQ29uc3RydWN0b3IsIHN0YXRpY3MpIHtcbiAgICAgIG1peFN0YXRpY1NwZWNJbnRvQ29tcG9uZW50KENvbnN0cnVjdG9yLCBzdGF0aWNzKTtcbiAgICB9LFxuICAgIGF1dG9iaW5kOiBmdW5jdGlvbigpIHt9XG4gIH07XG5cbiAgZnVuY3Rpb24gdmFsaWRhdGVUeXBlRGVmKENvbnN0cnVjdG9yLCB0eXBlRGVmLCBsb2NhdGlvbikge1xuICAgIGZvciAodmFyIHByb3BOYW1lIGluIHR5cGVEZWYpIHtcbiAgICAgIGlmICh0eXBlRGVmLmhhc093blByb3BlcnR5KHByb3BOYW1lKSkge1xuICAgICAgICAvLyB1c2UgYSB3YXJuaW5nIGluc3RlYWQgb2YgYW4gX2ludmFyaWFudCBzbyBjb21wb25lbnRzXG4gICAgICAgIC8vIGRvbid0IHNob3cgdXAgaW4gcHJvZCBidXQgb25seSBpbiBfX0RFVl9fXG4gICAgICAgIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgICAgICAgd2FybmluZyhcbiAgICAgICAgICAgIHR5cGVvZiB0eXBlRGVmW3Byb3BOYW1lXSA9PT0gJ2Z1bmN0aW9uJyxcbiAgICAgICAgICAgICclczogJXMgdHlwZSBgJXNgIGlzIGludmFsaWQ7IGl0IG11c3QgYmUgYSBmdW5jdGlvbiwgdXN1YWxseSBmcm9tICcgK1xuICAgICAgICAgICAgICAnUmVhY3QuUHJvcFR5cGVzLicsXG4gICAgICAgICAgICBDb25zdHJ1Y3Rvci5kaXNwbGF5TmFtZSB8fCAnUmVhY3RDbGFzcycsXG4gICAgICAgICAgICBSZWFjdFByb3BUeXBlTG9jYXRpb25OYW1lc1tsb2NhdGlvbl0sXG4gICAgICAgICAgICBwcm9wTmFtZVxuICAgICAgICAgICk7XG4gICAgICAgIH1cbiAgICAgIH1cbiAgICB9XG4gIH1cblxuICBmdW5jdGlvbiB2YWxpZGF0ZU1ldGhvZE92ZXJyaWRlKGlzQWxyZWFkeURlZmluZWQsIG5hbWUpIHtcbiAgICB2YXIgc3BlY1BvbGljeSA9IFJlYWN0Q2xhc3NJbnRlcmZhY2UuaGFzT3duUHJvcGVydHkobmFtZSlcbiAgICAgID8gUmVhY3RDbGFzc0ludGVyZmFjZVtuYW1lXVxuICAgICAgOiBudWxsO1xuXG4gICAgLy8gRGlzYWxsb3cgb3ZlcnJpZGluZyBvZiBiYXNlIGNsYXNzIG1ldGhvZHMgdW5sZXNzIGV4cGxpY2l0bHkgYWxsb3dlZC5cbiAgICBpZiAoUmVhY3RDbGFzc01peGluLmhhc093blByb3BlcnR5KG5hbWUpKSB7XG4gICAgICBfaW52YXJpYW50KFxuICAgICAgICBzcGVjUG9saWN5ID09PSAnT1ZFUlJJREVfQkFTRScsXG4gICAgICAgICdSZWFjdENsYXNzSW50ZXJmYWNlOiBZb3UgYXJlIGF0dGVtcHRpbmcgdG8gb3ZlcnJpZGUgJyArXG4gICAgICAgICAgJ2Alc2AgZnJvbSB5b3VyIGNsYXNzIHNwZWNpZmljYXRpb24uIEVuc3VyZSB0aGF0IHlvdXIgbWV0aG9kIG5hbWVzICcgK1xuICAgICAgICAgICdkbyBub3Qgb3ZlcmxhcCB3aXRoIFJlYWN0IG1ldGhvZHMuJyxcbiAgICAgICAgbmFtZVxuICAgICAgKTtcbiAgICB9XG5cbiAgICAvLyBEaXNhbGxvdyBkZWZpbmluZyBtZXRob2RzIG1vcmUgdGhhbiBvbmNlIHVubGVzcyBleHBsaWNpdGx5IGFsbG93ZWQuXG4gICAgaWYgKGlzQWxyZWFkeURlZmluZWQpIHtcbiAgICAgIF9pbnZhcmlhbnQoXG4gICAgICAgIHNwZWNQb2xpY3kgPT09ICdERUZJTkVfTUFOWScgfHwgc3BlY1BvbGljeSA9PT0gJ0RFRklORV9NQU5ZX01FUkdFRCcsXG4gICAgICAgICdSZWFjdENsYXNzSW50ZXJmYWNlOiBZb3UgYXJlIGF0dGVtcHRpbmcgdG8gZGVmaW5lICcgK1xuICAgICAgICAgICdgJXNgIG9uIHlvdXIgY29tcG9uZW50IG1vcmUgdGhhbiBvbmNlLiBUaGlzIGNvbmZsaWN0IG1heSBiZSBkdWUgJyArXG4gICAgICAgICAgJ3RvIGEgbWl4aW4uJyxcbiAgICAgICAgbmFtZVxuICAgICAgKTtcbiAgICB9XG4gIH1cblxuICAvKipcbiAgICogTWl4aW4gaGVscGVyIHdoaWNoIGhhbmRsZXMgcG9saWN5IHZhbGlkYXRpb24gYW5kIHJlc2VydmVkXG4gICAqIHNwZWNpZmljYXRpb24ga2V5cyB3aGVuIGJ1aWxkaW5nIFJlYWN0IGNsYXNzZXMuXG4gICAqL1xuICBmdW5jdGlvbiBtaXhTcGVjSW50b0NvbXBvbmVudChDb25zdHJ1Y3Rvciwgc3BlYykge1xuICAgIGlmICghc3BlYykge1xuICAgICAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICAgICAgdmFyIHR5cGVvZlNwZWMgPSB0eXBlb2Ygc3BlYztcbiAgICAgICAgdmFyIGlzTWl4aW5WYWxpZCA9IHR5cGVvZlNwZWMgPT09ICdvYmplY3QnICYmIHNwZWMgIT09IG51bGw7XG5cbiAgICAgICAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICAgICAgICB3YXJuaW5nKFxuICAgICAgICAgICAgaXNNaXhpblZhbGlkLFxuICAgICAgICAgICAgXCIlczogWW91J3JlIGF0dGVtcHRpbmcgdG8gaW5jbHVkZSBhIG1peGluIHRoYXQgaXMgZWl0aGVyIG51bGwgXCIgK1xuICAgICAgICAgICAgICAnb3Igbm90IGFuIG9iamVjdC4gQ2hlY2sgdGhlIG1peGlucyBpbmNsdWRlZCBieSB0aGUgY29tcG9uZW50LCAnICtcbiAgICAgICAgICAgICAgJ2FzIHdlbGwgYXMgYW55IG1peGlucyB0aGV5IGluY2x1ZGUgdGhlbXNlbHZlcy4gJyArXG4gICAgICAgICAgICAgICdFeHBlY3RlZCBvYmplY3QgYnV0IGdvdCAlcy4nLFxuICAgICAgICAgICAgQ29uc3RydWN0b3IuZGlzcGxheU5hbWUgfHwgJ1JlYWN0Q2xhc3MnLFxuICAgICAgICAgICAgc3BlYyA9PT0gbnVsbCA/IG51bGwgOiB0eXBlb2ZTcGVjXG4gICAgICAgICAgKTtcbiAgICAgICAgfVxuICAgICAgfVxuXG4gICAgICByZXR1cm47XG4gICAgfVxuXG4gICAgX2ludmFyaWFudChcbiAgICAgIHR5cGVvZiBzcGVjICE9PSAnZnVuY3Rpb24nLFxuICAgICAgXCJSZWFjdENsYXNzOiBZb3UncmUgYXR0ZW1wdGluZyB0byBcIiArXG4gICAgICAgICd1c2UgYSBjb21wb25lbnQgY2xhc3Mgb3IgZnVuY3Rpb24gYXMgYSBtaXhpbi4gSW5zdGVhZCwganVzdCB1c2UgYSAnICtcbiAgICAgICAgJ3JlZ3VsYXIgb2JqZWN0LidcbiAgICApO1xuICAgIF9pbnZhcmlhbnQoXG4gICAgICAhaXNWYWxpZEVsZW1lbnQoc3BlYyksXG4gICAgICBcIlJlYWN0Q2xhc3M6IFlvdSdyZSBhdHRlbXB0aW5nIHRvIFwiICtcbiAgICAgICAgJ3VzZSBhIGNvbXBvbmVudCBhcyBhIG1peGluLiBJbnN0ZWFkLCBqdXN0IHVzZSBhIHJlZ3VsYXIgb2JqZWN0LidcbiAgICApO1xuXG4gICAgdmFyIHByb3RvID0gQ29uc3RydWN0b3IucHJvdG90eXBlO1xuICAgIHZhciBhdXRvQmluZFBhaXJzID0gcHJvdG8uX19yZWFjdEF1dG9CaW5kUGFpcnM7XG5cbiAgICAvLyBCeSBoYW5kbGluZyBtaXhpbnMgYmVmb3JlIGFueSBvdGhlciBwcm9wZXJ0aWVzLCB3ZSBlbnN1cmUgdGhlIHNhbWVcbiAgICAvLyBjaGFpbmluZyBvcmRlciBpcyBhcHBsaWVkIHRvIG1ldGhvZHMgd2l0aCBERUZJTkVfTUFOWSBwb2xpY3ksIHdoZXRoZXJcbiAgICAvLyBtaXhpbnMgYXJlIGxpc3RlZCBiZWZvcmUgb3IgYWZ0ZXIgdGhlc2UgbWV0aG9kcyBpbiB0aGUgc3BlYy5cbiAgICBpZiAoc3BlYy5oYXNPd25Qcm9wZXJ0eShNSVhJTlNfS0VZKSkge1xuICAgICAgUkVTRVJWRURfU1BFQ19LRVlTLm1peGlucyhDb25zdHJ1Y3Rvciwgc3BlYy5taXhpbnMpO1xuICAgIH1cblxuICAgIGZvciAodmFyIG5hbWUgaW4gc3BlYykge1xuICAgICAgaWYgKCFzcGVjLmhhc093blByb3BlcnR5KG5hbWUpKSB7XG4gICAgICAgIGNvbnRpbnVlO1xuICAgICAgfVxuXG4gICAgICBpZiAobmFtZSA9PT0gTUlYSU5TX0tFWSkge1xuICAgICAgICAvLyBXZSBoYXZlIGFscmVhZHkgaGFuZGxlZCBtaXhpbnMgaW4gYSBzcGVjaWFsIGNhc2UgYWJvdmUuXG4gICAgICAgIGNvbnRpbnVlO1xuICAgICAgfVxuXG4gICAgICB2YXIgcHJvcGVydHkgPSBzcGVjW25hbWVdO1xuICAgICAgdmFyIGlzQWxyZWFkeURlZmluZWQgPSBwcm90by5oYXNPd25Qcm9wZXJ0eShuYW1lKTtcbiAgICAgIHZhbGlkYXRlTWV0aG9kT3ZlcnJpZGUoaXNBbHJlYWR5RGVmaW5lZCwgbmFtZSk7XG5cbiAgICAgIGlmIChSRVNFUlZFRF9TUEVDX0tFWVMuaGFzT3duUHJvcGVydHkobmFtZSkpIHtcbiAgICAgICAgUkVTRVJWRURfU1BFQ19LRVlTW25hbWVdKENvbnN0cnVjdG9yLCBwcm9wZXJ0eSk7XG4gICAgICB9IGVsc2Uge1xuICAgICAgICAvLyBTZXR1cCBtZXRob2RzIG9uIHByb3RvdHlwZTpcbiAgICAgICAgLy8gVGhlIGZvbGxvd2luZyBtZW1iZXIgbWV0aG9kcyBzaG91bGQgbm90IGJlIGF1dG9tYXRpY2FsbHkgYm91bmQ6XG4gICAgICAgIC8vIDEuIEV4cGVjdGVkIFJlYWN0Q2xhc3MgbWV0aG9kcyAoaW4gdGhlIFwiaW50ZXJmYWNlXCIpLlxuICAgICAgICAvLyAyLiBPdmVycmlkZGVuIG1ldGhvZHMgKHRoYXQgd2VyZSBtaXhlZCBpbikuXG4gICAgICAgIHZhciBpc1JlYWN0Q2xhc3NNZXRob2QgPSBSZWFjdENsYXNzSW50ZXJmYWNlLmhhc093blByb3BlcnR5KG5hbWUpO1xuICAgICAgICB2YXIgaXNGdW5jdGlvbiA9IHR5cGVvZiBwcm9wZXJ0eSA9PT0gJ2Z1bmN0aW9uJztcbiAgICAgICAgdmFyIHNob3VsZEF1dG9CaW5kID1cbiAgICAgICAgICBpc0Z1bmN0aW9uICYmXG4gICAgICAgICAgIWlzUmVhY3RDbGFzc01ldGhvZCAmJlxuICAgICAgICAgICFpc0FscmVhZHlEZWZpbmVkICYmXG4gICAgICAgICAgc3BlYy5hdXRvYmluZCAhPT0gZmFsc2U7XG5cbiAgICAgICAgaWYgKHNob3VsZEF1dG9CaW5kKSB7XG4gICAgICAgICAgYXV0b0JpbmRQYWlycy5wdXNoKG5hbWUsIHByb3BlcnR5KTtcbiAgICAgICAgICBwcm90b1tuYW1lXSA9IHByb3BlcnR5O1xuICAgICAgICB9IGVsc2Uge1xuICAgICAgICAgIGlmIChpc0FscmVhZHlEZWZpbmVkKSB7XG4gICAgICAgICAgICB2YXIgc3BlY1BvbGljeSA9IFJlYWN0Q2xhc3NJbnRlcmZhY2VbbmFtZV07XG5cbiAgICAgICAgICAgIC8vIFRoZXNlIGNhc2VzIHNob3VsZCBhbHJlYWR5IGJlIGNhdWdodCBieSB2YWxpZGF0ZU1ldGhvZE92ZXJyaWRlLlxuICAgICAgICAgICAgX2ludmFyaWFudChcbiAgICAgICAgICAgICAgaXNSZWFjdENsYXNzTWV0aG9kICYmXG4gICAgICAgICAgICAgICAgKHNwZWNQb2xpY3kgPT09ICdERUZJTkVfTUFOWV9NRVJHRUQnIHx8XG4gICAgICAgICAgICAgICAgICBzcGVjUG9saWN5ID09PSAnREVGSU5FX01BTlknKSxcbiAgICAgICAgICAgICAgJ1JlYWN0Q2xhc3M6IFVuZXhwZWN0ZWQgc3BlYyBwb2xpY3kgJXMgZm9yIGtleSAlcyAnICtcbiAgICAgICAgICAgICAgICAnd2hlbiBtaXhpbmcgaW4gY29tcG9uZW50IHNwZWNzLicsXG4gICAgICAgICAgICAgIHNwZWNQb2xpY3ksXG4gICAgICAgICAgICAgIG5hbWVcbiAgICAgICAgICAgICk7XG5cbiAgICAgICAgICAgIC8vIEZvciBtZXRob2RzIHdoaWNoIGFyZSBkZWZpbmVkIG1vcmUgdGhhbiBvbmNlLCBjYWxsIHRoZSBleGlzdGluZ1xuICAgICAgICAgICAgLy8gbWV0aG9kcyBiZWZvcmUgY2FsbGluZyB0aGUgbmV3IHByb3BlcnR5LCBtZXJnaW5nIGlmIGFwcHJvcHJpYXRlLlxuICAgICAgICAgICAgaWYgKHNwZWNQb2xpY3kgPT09ICdERUZJTkVfTUFOWV9NRVJHRUQnKSB7XG4gICAgICAgICAgICAgIHByb3RvW25hbWVdID0gY3JlYXRlTWVyZ2VkUmVzdWx0RnVuY3Rpb24ocHJvdG9bbmFtZV0sIHByb3BlcnR5KTtcbiAgICAgICAgICAgIH0gZWxzZSBpZiAoc3BlY1BvbGljeSA9PT0gJ0RFRklORV9NQU5ZJykge1xuICAgICAgICAgICAgICBwcm90b1tuYW1lXSA9IGNyZWF0ZUNoYWluZWRGdW5jdGlvbihwcm90b1tuYW1lXSwgcHJvcGVydHkpO1xuICAgICAgICAgICAgfVxuICAgICAgICAgIH0gZWxzZSB7XG4gICAgICAgICAgICBwcm90b1tuYW1lXSA9IHByb3BlcnR5O1xuICAgICAgICAgICAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICAgICAgICAgICAgLy8gQWRkIHZlcmJvc2UgZGlzcGxheU5hbWUgdG8gdGhlIGZ1bmN0aW9uLCB3aGljaCBoZWxwcyB3aGVuIGxvb2tpbmdcbiAgICAgICAgICAgICAgLy8gYXQgcHJvZmlsaW5nIHRvb2xzLlxuICAgICAgICAgICAgICBpZiAodHlwZW9mIHByb3BlcnR5ID09PSAnZnVuY3Rpb24nICYmIHNwZWMuZGlzcGxheU5hbWUpIHtcbiAgICAgICAgICAgICAgICBwcm90b1tuYW1lXS5kaXNwbGF5TmFtZSA9IHNwZWMuZGlzcGxheU5hbWUgKyAnXycgKyBuYW1lO1xuICAgICAgICAgICAgICB9XG4gICAgICAgICAgICB9XG4gICAgICAgICAgfVxuICAgICAgICB9XG4gICAgICB9XG4gICAgfVxuICB9XG5cbiAgZnVuY3Rpb24gbWl4U3RhdGljU3BlY0ludG9Db21wb25lbnQoQ29uc3RydWN0b3IsIHN0YXRpY3MpIHtcbiAgICBpZiAoIXN0YXRpY3MpIHtcbiAgICAgIHJldHVybjtcbiAgICB9XG5cbiAgICBmb3IgKHZhciBuYW1lIGluIHN0YXRpY3MpIHtcbiAgICAgIHZhciBwcm9wZXJ0eSA9IHN0YXRpY3NbbmFtZV07XG4gICAgICBpZiAoIXN0YXRpY3MuaGFzT3duUHJvcGVydHkobmFtZSkpIHtcbiAgICAgICAgY29udGludWU7XG4gICAgICB9XG5cbiAgICAgIHZhciBpc1Jlc2VydmVkID0gbmFtZSBpbiBSRVNFUlZFRF9TUEVDX0tFWVM7XG4gICAgICBfaW52YXJpYW50KFxuICAgICAgICAhaXNSZXNlcnZlZCxcbiAgICAgICAgJ1JlYWN0Q2xhc3M6IFlvdSBhcmUgYXR0ZW1wdGluZyB0byBkZWZpbmUgYSByZXNlcnZlZCAnICtcbiAgICAgICAgICAncHJvcGVydHksIGAlc2AsIHRoYXQgc2hvdWxkblxcJ3QgYmUgb24gdGhlIFwic3RhdGljc1wiIGtleS4gRGVmaW5lIGl0ICcgK1xuICAgICAgICAgICdhcyBhbiBpbnN0YW5jZSBwcm9wZXJ0eSBpbnN0ZWFkOyBpdCB3aWxsIHN0aWxsIGJlIGFjY2Vzc2libGUgb24gdGhlICcgK1xuICAgICAgICAgICdjb25zdHJ1Y3Rvci4nLFxuICAgICAgICBuYW1lXG4gICAgICApO1xuXG4gICAgICB2YXIgaXNBbHJlYWR5RGVmaW5lZCA9IG5hbWUgaW4gQ29uc3RydWN0b3I7XG4gICAgICBpZiAoaXNBbHJlYWR5RGVmaW5lZCkge1xuICAgICAgICB2YXIgc3BlY1BvbGljeSA9IFJlYWN0Q2xhc3NTdGF0aWNJbnRlcmZhY2UuaGFzT3duUHJvcGVydHkobmFtZSlcbiAgICAgICAgICA/IFJlYWN0Q2xhc3NTdGF0aWNJbnRlcmZhY2VbbmFtZV1cbiAgICAgICAgICA6IG51bGw7XG5cbiAgICAgICAgX2ludmFyaWFudChcbiAgICAgICAgICBzcGVjUG9saWN5ID09PSAnREVGSU5FX01BTllfTUVSR0VEJyxcbiAgICAgICAgICAnUmVhY3RDbGFzczogWW91IGFyZSBhdHRlbXB0aW5nIHRvIGRlZmluZSAnICtcbiAgICAgICAgICAgICdgJXNgIG9uIHlvdXIgY29tcG9uZW50IG1vcmUgdGhhbiBvbmNlLiBUaGlzIGNvbmZsaWN0IG1heSBiZSAnICtcbiAgICAgICAgICAgICdkdWUgdG8gYSBtaXhpbi4nLFxuICAgICAgICAgIG5hbWVcbiAgICAgICAgKTtcblxuICAgICAgICBDb25zdHJ1Y3RvcltuYW1lXSA9IGNyZWF0ZU1lcmdlZFJlc3VsdEZ1bmN0aW9uKENvbnN0cnVjdG9yW25hbWVdLCBwcm9wZXJ0eSk7XG5cbiAgICAgICAgcmV0dXJuO1xuICAgICAgfVxuXG4gICAgICBDb25zdHJ1Y3RvcltuYW1lXSA9IHByb3BlcnR5O1xuICAgIH1cbiAgfVxuXG4gIC8qKlxuICAgKiBNZXJnZSB0d28gb2JqZWN0cywgYnV0IHRocm93IGlmIGJvdGggY29udGFpbiB0aGUgc2FtZSBrZXkuXG4gICAqXG4gICAqIEBwYXJhbSB7b2JqZWN0fSBvbmUgVGhlIGZpcnN0IG9iamVjdCwgd2hpY2ggaXMgbXV0YXRlZC5cbiAgICogQHBhcmFtIHtvYmplY3R9IHR3byBUaGUgc2Vjb25kIG9iamVjdFxuICAgKiBAcmV0dXJuIHtvYmplY3R9IG9uZSBhZnRlciBpdCBoYXMgYmVlbiBtdXRhdGVkIHRvIGNvbnRhaW4gZXZlcnl0aGluZyBpbiB0d28uXG4gICAqL1xuICBmdW5jdGlvbiBtZXJnZUludG9XaXRoTm9EdXBsaWNhdGVLZXlzKG9uZSwgdHdvKSB7XG4gICAgX2ludmFyaWFudChcbiAgICAgIG9uZSAmJiB0d28gJiYgdHlwZW9mIG9uZSA9PT0gJ29iamVjdCcgJiYgdHlwZW9mIHR3byA9PT0gJ29iamVjdCcsXG4gICAgICAnbWVyZ2VJbnRvV2l0aE5vRHVwbGljYXRlS2V5cygpOiBDYW5ub3QgbWVyZ2Ugbm9uLW9iamVjdHMuJ1xuICAgICk7XG5cbiAgICBmb3IgKHZhciBrZXkgaW4gdHdvKSB7XG4gICAgICBpZiAodHdvLmhhc093blByb3BlcnR5KGtleSkpIHtcbiAgICAgICAgX2ludmFyaWFudChcbiAgICAgICAgICBvbmVba2V5XSA9PT0gdW5kZWZpbmVkLFxuICAgICAgICAgICdtZXJnZUludG9XaXRoTm9EdXBsaWNhdGVLZXlzKCk6ICcgK1xuICAgICAgICAgICAgJ1RyaWVkIHRvIG1lcmdlIHR3byBvYmplY3RzIHdpdGggdGhlIHNhbWUga2V5OiBgJXNgLiBUaGlzIGNvbmZsaWN0ICcgK1xuICAgICAgICAgICAgJ21heSBiZSBkdWUgdG8gYSBtaXhpbjsgaW4gcGFydGljdWxhciwgdGhpcyBtYXkgYmUgY2F1c2VkIGJ5IHR3byAnICtcbiAgICAgICAgICAgICdnZXRJbml0aWFsU3RhdGUoKSBvciBnZXREZWZhdWx0UHJvcHMoKSBtZXRob2RzIHJldHVybmluZyBvYmplY3RzICcgK1xuICAgICAgICAgICAgJ3dpdGggY2xhc2hpbmcga2V5cy4nLFxuICAgICAgICAgIGtleVxuICAgICAgICApO1xuICAgICAgICBvbmVba2V5XSA9IHR3b1trZXldO1xuICAgICAgfVxuICAgIH1cbiAgICByZXR1cm4gb25lO1xuICB9XG5cbiAgLyoqXG4gICAqIENyZWF0ZXMgYSBmdW5jdGlvbiB0aGF0IGludm9rZXMgdHdvIGZ1bmN0aW9ucyBhbmQgbWVyZ2VzIHRoZWlyIHJldHVybiB2YWx1ZXMuXG4gICAqXG4gICAqIEBwYXJhbSB7ZnVuY3Rpb259IG9uZSBGdW5jdGlvbiB0byBpbnZva2UgZmlyc3QuXG4gICAqIEBwYXJhbSB7ZnVuY3Rpb259IHR3byBGdW5jdGlvbiB0byBpbnZva2Ugc2Vjb25kLlxuICAgKiBAcmV0dXJuIHtmdW5jdGlvbn0gRnVuY3Rpb24gdGhhdCBpbnZva2VzIHRoZSB0d28gYXJndW1lbnQgZnVuY3Rpb25zLlxuICAgKiBAcHJpdmF0ZVxuICAgKi9cbiAgZnVuY3Rpb24gY3JlYXRlTWVyZ2VkUmVzdWx0RnVuY3Rpb24ob25lLCB0d28pIHtcbiAgICByZXR1cm4gZnVuY3Rpb24gbWVyZ2VkUmVzdWx0KCkge1xuICAgICAgdmFyIGEgPSBvbmUuYXBwbHkodGhpcywgYXJndW1lbnRzKTtcbiAgICAgIHZhciBiID0gdHdvLmFwcGx5KHRoaXMsIGFyZ3VtZW50cyk7XG4gICAgICBpZiAoYSA9PSBudWxsKSB7XG4gICAgICAgIHJldHVybiBiO1xuICAgICAgfSBlbHNlIGlmIChiID09IG51bGwpIHtcbiAgICAgICAgcmV0dXJuIGE7XG4gICAgICB9XG4gICAgICB2YXIgYyA9IHt9O1xuICAgICAgbWVyZ2VJbnRvV2l0aE5vRHVwbGljYXRlS2V5cyhjLCBhKTtcbiAgICAgIG1lcmdlSW50b1dpdGhOb0R1cGxpY2F0ZUtleXMoYywgYik7XG4gICAgICByZXR1cm4gYztcbiAgICB9O1xuICB9XG5cbiAgLyoqXG4gICAqIENyZWF0ZXMgYSBmdW5jdGlvbiB0aGF0IGludm9rZXMgdHdvIGZ1bmN0aW9ucyBhbmQgaWdub3JlcyB0aGVpciByZXR1cm4gdmFsZXMuXG4gICAqXG4gICAqIEBwYXJhbSB7ZnVuY3Rpb259IG9uZSBGdW5jdGlvbiB0byBpbnZva2UgZmlyc3QuXG4gICAqIEBwYXJhbSB7ZnVuY3Rpb259IHR3byBGdW5jdGlvbiB0byBpbnZva2Ugc2Vjb25kLlxuICAgKiBAcmV0dXJuIHtmdW5jdGlvbn0gRnVuY3Rpb24gdGhhdCBpbnZva2VzIHRoZSB0d28gYXJndW1lbnQgZnVuY3Rpb25zLlxuICAgKiBAcHJpdmF0ZVxuICAgKi9cbiAgZnVuY3Rpb24gY3JlYXRlQ2hhaW5lZEZ1bmN0aW9uKG9uZSwgdHdvKSB7XG4gICAgcmV0dXJuIGZ1bmN0aW9uIGNoYWluZWRGdW5jdGlvbigpIHtcbiAgICAgIG9uZS5hcHBseSh0aGlzLCBhcmd1bWVudHMpO1xuICAgICAgdHdvLmFwcGx5KHRoaXMsIGFyZ3VtZW50cyk7XG4gICAgfTtcbiAgfVxuXG4gIC8qKlxuICAgKiBCaW5kcyBhIG1ldGhvZCB0byB0aGUgY29tcG9uZW50LlxuICAgKlxuICAgKiBAcGFyYW0ge29iamVjdH0gY29tcG9uZW50IENvbXBvbmVudCB3aG9zZSBtZXRob2QgaXMgZ29pbmcgdG8gYmUgYm91bmQuXG4gICAqIEBwYXJhbSB7ZnVuY3Rpb259IG1ldGhvZCBNZXRob2QgdG8gYmUgYm91bmQuXG4gICAqIEByZXR1cm4ge2Z1bmN0aW9ufSBUaGUgYm91bmQgbWV0aG9kLlxuICAgKi9cbiAgZnVuY3Rpb24gYmluZEF1dG9CaW5kTWV0aG9kKGNvbXBvbmVudCwgbWV0aG9kKSB7XG4gICAgdmFyIGJvdW5kTWV0aG9kID0gbWV0aG9kLmJpbmQoY29tcG9uZW50KTtcbiAgICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgICAgYm91bmRNZXRob2QuX19yZWFjdEJvdW5kQ29udGV4dCA9IGNvbXBvbmVudDtcbiAgICAgIGJvdW5kTWV0aG9kLl9fcmVhY3RCb3VuZE1ldGhvZCA9IG1ldGhvZDtcbiAgICAgIGJvdW5kTWV0aG9kLl9fcmVhY3RCb3VuZEFyZ3VtZW50cyA9IG51bGw7XG4gICAgICB2YXIgY29tcG9uZW50TmFtZSA9IGNvbXBvbmVudC5jb25zdHJ1Y3Rvci5kaXNwbGF5TmFtZTtcbiAgICAgIHZhciBfYmluZCA9IGJvdW5kTWV0aG9kLmJpbmQ7XG4gICAgICBib3VuZE1ldGhvZC5iaW5kID0gZnVuY3Rpb24obmV3VGhpcykge1xuICAgICAgICBmb3IgKFxuICAgICAgICAgIHZhciBfbGVuID0gYXJndW1lbnRzLmxlbmd0aCxcbiAgICAgICAgICAgIGFyZ3MgPSBBcnJheShfbGVuID4gMSA/IF9sZW4gLSAxIDogMCksXG4gICAgICAgICAgICBfa2V5ID0gMTtcbiAgICAgICAgICBfa2V5IDwgX2xlbjtcbiAgICAgICAgICBfa2V5KytcbiAgICAgICAgKSB7XG4gICAgICAgICAgYXJnc1tfa2V5IC0gMV0gPSBhcmd1bWVudHNbX2tleV07XG4gICAgICAgIH1cblxuICAgICAgICAvLyBVc2VyIGlzIHRyeWluZyB0byBiaW5kKCkgYW4gYXV0b2JvdW5kIG1ldGhvZDsgd2UgZWZmZWN0aXZlbHkgd2lsbFxuICAgICAgICAvLyBpZ25vcmUgdGhlIHZhbHVlIG9mIFwidGhpc1wiIHRoYXQgdGhlIHVzZXIgaXMgdHJ5aW5nIHRvIHVzZSwgc29cbiAgICAgICAgLy8gbGV0J3Mgd2Fybi5cbiAgICAgICAgaWYgKG5ld1RoaXMgIT09IGNvbXBvbmVudCAmJiBuZXdUaGlzICE9PSBudWxsKSB7XG4gICAgICAgICAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICAgICAgICAgIHdhcm5pbmcoXG4gICAgICAgICAgICAgIGZhbHNlLFxuICAgICAgICAgICAgICAnYmluZCgpOiBSZWFjdCBjb21wb25lbnQgbWV0aG9kcyBtYXkgb25seSBiZSBib3VuZCB0byB0aGUgJyArXG4gICAgICAgICAgICAgICAgJ2NvbXBvbmVudCBpbnN0YW5jZS4gU2VlICVzJyxcbiAgICAgICAgICAgICAgY29tcG9uZW50TmFtZVxuICAgICAgICAgICAgKTtcbiAgICAgICAgICB9XG4gICAgICAgIH0gZWxzZSBpZiAoIWFyZ3MubGVuZ3RoKSB7XG4gICAgICAgICAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICAgICAgICAgIHdhcm5pbmcoXG4gICAgICAgICAgICAgIGZhbHNlLFxuICAgICAgICAgICAgICAnYmluZCgpOiBZb3UgYXJlIGJpbmRpbmcgYSBjb21wb25lbnQgbWV0aG9kIHRvIHRoZSBjb21wb25lbnQuICcgK1xuICAgICAgICAgICAgICAgICdSZWFjdCBkb2VzIHRoaXMgZm9yIHlvdSBhdXRvbWF0aWNhbGx5IGluIGEgaGlnaC1wZXJmb3JtYW5jZSAnICtcbiAgICAgICAgICAgICAgICAnd2F5LCBzbyB5b3UgY2FuIHNhZmVseSByZW1vdmUgdGhpcyBjYWxsLiBTZWUgJXMnLFxuICAgICAgICAgICAgICBjb21wb25lbnROYW1lXG4gICAgICAgICAgICApO1xuICAgICAgICAgIH1cbiAgICAgICAgICByZXR1cm4gYm91bmRNZXRob2Q7XG4gICAgICAgIH1cbiAgICAgICAgdmFyIHJlYm91bmRNZXRob2QgPSBfYmluZC5hcHBseShib3VuZE1ldGhvZCwgYXJndW1lbnRzKTtcbiAgICAgICAgcmVib3VuZE1ldGhvZC5fX3JlYWN0Qm91bmRDb250ZXh0ID0gY29tcG9uZW50O1xuICAgICAgICByZWJvdW5kTWV0aG9kLl9fcmVhY3RCb3VuZE1ldGhvZCA9IG1ldGhvZDtcbiAgICAgICAgcmVib3VuZE1ldGhvZC5fX3JlYWN0Qm91bmRBcmd1bWVudHMgPSBhcmdzO1xuICAgICAgICByZXR1cm4gcmVib3VuZE1ldGhvZDtcbiAgICAgIH07XG4gICAgfVxuICAgIHJldHVybiBib3VuZE1ldGhvZDtcbiAgfVxuXG4gIC8qKlxuICAgKiBCaW5kcyBhbGwgYXV0by1ib3VuZCBtZXRob2RzIGluIGEgY29tcG9uZW50LlxuICAgKlxuICAgKiBAcGFyYW0ge29iamVjdH0gY29tcG9uZW50IENvbXBvbmVudCB3aG9zZSBtZXRob2QgaXMgZ29pbmcgdG8gYmUgYm91bmQuXG4gICAqL1xuICBmdW5jdGlvbiBiaW5kQXV0b0JpbmRNZXRob2RzKGNvbXBvbmVudCkge1xuICAgIHZhciBwYWlycyA9IGNvbXBvbmVudC5fX3JlYWN0QXV0b0JpbmRQYWlycztcbiAgICBmb3IgKHZhciBpID0gMDsgaSA8IHBhaXJzLmxlbmd0aDsgaSArPSAyKSB7XG4gICAgICB2YXIgYXV0b0JpbmRLZXkgPSBwYWlyc1tpXTtcbiAgICAgIHZhciBtZXRob2QgPSBwYWlyc1tpICsgMV07XG4gICAgICBjb21wb25lbnRbYXV0b0JpbmRLZXldID0gYmluZEF1dG9CaW5kTWV0aG9kKGNvbXBvbmVudCwgbWV0aG9kKTtcbiAgICB9XG4gIH1cblxuICB2YXIgSXNNb3VudGVkUHJlTWl4aW4gPSB7XG4gICAgY29tcG9uZW50RGlkTW91bnQ6IGZ1bmN0aW9uKCkge1xuICAgICAgdGhpcy5fX2lzTW91bnRlZCA9IHRydWU7XG4gICAgfVxuICB9O1xuXG4gIHZhciBJc01vdW50ZWRQb3N0TWl4aW4gPSB7XG4gICAgY29tcG9uZW50V2lsbFVubW91bnQ6IGZ1bmN0aW9uKCkge1xuICAgICAgdGhpcy5fX2lzTW91bnRlZCA9IGZhbHNlO1xuICAgIH1cbiAgfTtcblxuICAvKipcbiAgICogQWRkIG1vcmUgdG8gdGhlIFJlYWN0Q2xhc3MgYmFzZSBjbGFzcy4gVGhlc2UgYXJlIGFsbCBsZWdhY3kgZmVhdHVyZXMgYW5kXG4gICAqIHRoZXJlZm9yZSBub3QgYWxyZWFkeSBwYXJ0IG9mIHRoZSBtb2Rlcm4gUmVhY3RDb21wb25lbnQuXG4gICAqL1xuICB2YXIgUmVhY3RDbGFzc01peGluID0ge1xuICAgIC8qKlxuICAgICAqIFRPRE86IFRoaXMgd2lsbCBiZSBkZXByZWNhdGVkIGJlY2F1c2Ugc3RhdGUgc2hvdWxkIGFsd2F5cyBrZWVwIGEgY29uc2lzdGVudFxuICAgICAqIHR5cGUgc2lnbmF0dXJlIGFuZCB0aGUgb25seSB1c2UgY2FzZSBmb3IgdGhpcywgaXMgdG8gYXZvaWQgdGhhdC5cbiAgICAgKi9cbiAgICByZXBsYWNlU3RhdGU6IGZ1bmN0aW9uKG5ld1N0YXRlLCBjYWxsYmFjaykge1xuICAgICAgdGhpcy51cGRhdGVyLmVucXVldWVSZXBsYWNlU3RhdGUodGhpcywgbmV3U3RhdGUsIGNhbGxiYWNrKTtcbiAgICB9LFxuXG4gICAgLyoqXG4gICAgICogQ2hlY2tzIHdoZXRoZXIgb3Igbm90IHRoaXMgY29tcG9zaXRlIGNvbXBvbmVudCBpcyBtb3VudGVkLlxuICAgICAqIEByZXR1cm4ge2Jvb2xlYW59IFRydWUgaWYgbW91bnRlZCwgZmFsc2Ugb3RoZXJ3aXNlLlxuICAgICAqIEBwcm90ZWN0ZWRcbiAgICAgKiBAZmluYWxcbiAgICAgKi9cbiAgICBpc01vdW50ZWQ6IGZ1bmN0aW9uKCkge1xuICAgICAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICAgICAgd2FybmluZyhcbiAgICAgICAgICB0aGlzLl9fZGlkV2FybklzTW91bnRlZCxcbiAgICAgICAgICAnJXM6IGlzTW91bnRlZCBpcyBkZXByZWNhdGVkLiBJbnN0ZWFkLCBtYWtlIHN1cmUgdG8gY2xlYW4gdXAgJyArXG4gICAgICAgICAgICAnc3Vic2NyaXB0aW9ucyBhbmQgcGVuZGluZyByZXF1ZXN0cyBpbiBjb21wb25lbnRXaWxsVW5tb3VudCB0byAnICtcbiAgICAgICAgICAgICdwcmV2ZW50IG1lbW9yeSBsZWFrcy4nLFxuICAgICAgICAgICh0aGlzLmNvbnN0cnVjdG9yICYmIHRoaXMuY29uc3RydWN0b3IuZGlzcGxheU5hbWUpIHx8XG4gICAgICAgICAgICB0aGlzLm5hbWUgfHxcbiAgICAgICAgICAgICdDb21wb25lbnQnXG4gICAgICAgICk7XG4gICAgICAgIHRoaXMuX19kaWRXYXJuSXNNb3VudGVkID0gdHJ1ZTtcbiAgICAgIH1cbiAgICAgIHJldHVybiAhIXRoaXMuX19pc01vdW50ZWQ7XG4gICAgfVxuICB9O1xuXG4gIHZhciBSZWFjdENsYXNzQ29tcG9uZW50ID0gZnVuY3Rpb24oKSB7fTtcbiAgX2Fzc2lnbihcbiAgICBSZWFjdENsYXNzQ29tcG9uZW50LnByb3RvdHlwZSxcbiAgICBSZWFjdENvbXBvbmVudC5wcm90b3R5cGUsXG4gICAgUmVhY3RDbGFzc01peGluXG4gICk7XG5cbiAgLyoqXG4gICAqIENyZWF0ZXMgYSBjb21wb3NpdGUgY29tcG9uZW50IGNsYXNzIGdpdmVuIGEgY2xhc3Mgc3BlY2lmaWNhdGlvbi5cbiAgICogU2VlIGh0dHBzOi8vZmFjZWJvb2suZ2l0aHViLmlvL3JlYWN0L2RvY3MvdG9wLWxldmVsLWFwaS5odG1sI3JlYWN0LmNyZWF0ZWNsYXNzXG4gICAqXG4gICAqIEBwYXJhbSB7b2JqZWN0fSBzcGVjIENsYXNzIHNwZWNpZmljYXRpb24gKHdoaWNoIG11c3QgZGVmaW5lIGByZW5kZXJgKS5cbiAgICogQHJldHVybiB7ZnVuY3Rpb259IENvbXBvbmVudCBjb25zdHJ1Y3RvciBmdW5jdGlvbi5cbiAgICogQHB1YmxpY1xuICAgKi9cbiAgZnVuY3Rpb24gY3JlYXRlQ2xhc3Moc3BlYykge1xuICAgIC8vIFRvIGtlZXAgb3VyIHdhcm5pbmdzIG1vcmUgdW5kZXJzdGFuZGFibGUsIHdlJ2xsIHVzZSBhIGxpdHRsZSBoYWNrIGhlcmUgdG9cbiAgICAvLyBlbnN1cmUgdGhhdCBDb25zdHJ1Y3Rvci5uYW1lICE9PSAnQ29uc3RydWN0b3InLiBUaGlzIG1ha2VzIHN1cmUgd2UgZG9uJ3RcbiAgICAvLyB1bm5lY2Vzc2FyaWx5IGlkZW50aWZ5IGEgY2xhc3Mgd2l0aG91dCBkaXNwbGF5TmFtZSBhcyAnQ29uc3RydWN0b3InLlxuICAgIHZhciBDb25zdHJ1Y3RvciA9IGlkZW50aXR5KGZ1bmN0aW9uKHByb3BzLCBjb250ZXh0LCB1cGRhdGVyKSB7XG4gICAgICAvLyBUaGlzIGNvbnN0cnVjdG9yIGdldHMgb3ZlcnJpZGRlbiBieSBtb2Nrcy4gVGhlIGFyZ3VtZW50IGlzIHVzZWRcbiAgICAgIC8vIGJ5IG1vY2tzIHRvIGFzc2VydCBvbiB3aGF0IGdldHMgbW91bnRlZC5cblxuICAgICAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICAgICAgd2FybmluZyhcbiAgICAgICAgICB0aGlzIGluc3RhbmNlb2YgQ29uc3RydWN0b3IsXG4gICAgICAgICAgJ1NvbWV0aGluZyBpcyBjYWxsaW5nIGEgUmVhY3QgY29tcG9uZW50IGRpcmVjdGx5LiBVc2UgYSBmYWN0b3J5IG9yICcgK1xuICAgICAgICAgICAgJ0pTWCBpbnN0ZWFkLiBTZWU6IGh0dHBzOi8vZmIubWUvcmVhY3QtbGVnYWN5ZmFjdG9yeSdcbiAgICAgICAgKTtcbiAgICAgIH1cblxuICAgICAgLy8gV2lyZSB1cCBhdXRvLWJpbmRpbmdcbiAgICAgIGlmICh0aGlzLl9fcmVhY3RBdXRvQmluZFBhaXJzLmxlbmd0aCkge1xuICAgICAgICBiaW5kQXV0b0JpbmRNZXRob2RzKHRoaXMpO1xuICAgICAgfVxuXG4gICAgICB0aGlzLnByb3BzID0gcHJvcHM7XG4gICAgICB0aGlzLmNvbnRleHQgPSBjb250ZXh0O1xuICAgICAgdGhpcy5yZWZzID0gZW1wdHlPYmplY3Q7XG4gICAgICB0aGlzLnVwZGF0ZXIgPSB1cGRhdGVyIHx8IFJlYWN0Tm9vcFVwZGF0ZVF1ZXVlO1xuXG4gICAgICB0aGlzLnN0YXRlID0gbnVsbDtcblxuICAgICAgLy8gUmVhY3RDbGFzc2VzIGRvZXNuJ3QgaGF2ZSBjb25zdHJ1Y3RvcnMuIEluc3RlYWQsIHRoZXkgdXNlIHRoZVxuICAgICAgLy8gZ2V0SW5pdGlhbFN0YXRlIGFuZCBjb21wb25lbnRXaWxsTW91bnQgbWV0aG9kcyBmb3IgaW5pdGlhbGl6YXRpb24uXG5cbiAgICAgIHZhciBpbml0aWFsU3RhdGUgPSB0aGlzLmdldEluaXRpYWxTdGF0ZSA/IHRoaXMuZ2V0SW5pdGlhbFN0YXRlKCkgOiBudWxsO1xuICAgICAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICAgICAgLy8gV2UgYWxsb3cgYXV0by1tb2NrcyB0byBwcm9jZWVkIGFzIGlmIHRoZXkncmUgcmV0dXJuaW5nIG51bGwuXG4gICAgICAgIGlmIChcbiAgICAgICAgICBpbml0aWFsU3RhdGUgPT09IHVuZGVmaW5lZCAmJlxuICAgICAgICAgIHRoaXMuZ2V0SW5pdGlhbFN0YXRlLl9pc01vY2tGdW5jdGlvblxuICAgICAgICApIHtcbiAgICAgICAgICAvLyBUaGlzIGlzIHByb2JhYmx5IGJhZCBwcmFjdGljZS4gQ29uc2lkZXIgd2FybmluZyBoZXJlIGFuZFxuICAgICAgICAgIC8vIGRlcHJlY2F0aW5nIHRoaXMgY29udmVuaWVuY2UuXG4gICAgICAgICAgaW5pdGlhbFN0YXRlID0gbnVsbDtcbiAgICAgICAgfVxuICAgICAgfVxuICAgICAgX2ludmFyaWFudChcbiAgICAgICAgdHlwZW9mIGluaXRpYWxTdGF0ZSA9PT0gJ29iamVjdCcgJiYgIUFycmF5LmlzQXJyYXkoaW5pdGlhbFN0YXRlKSxcbiAgICAgICAgJyVzLmdldEluaXRpYWxTdGF0ZSgpOiBtdXN0IHJldHVybiBhbiBvYmplY3Qgb3IgbnVsbCcsXG4gICAgICAgIENvbnN0cnVjdG9yLmRpc3BsYXlOYW1lIHx8ICdSZWFjdENvbXBvc2l0ZUNvbXBvbmVudCdcbiAgICAgICk7XG5cbiAgICAgIHRoaXMuc3RhdGUgPSBpbml0aWFsU3RhdGU7XG4gICAgfSk7XG4gICAgQ29uc3RydWN0b3IucHJvdG90eXBlID0gbmV3IFJlYWN0Q2xhc3NDb21wb25lbnQoKTtcbiAgICBDb25zdHJ1Y3Rvci5wcm90b3R5cGUuY29uc3RydWN0b3IgPSBDb25zdHJ1Y3RvcjtcbiAgICBDb25zdHJ1Y3Rvci5wcm90b3R5cGUuX19yZWFjdEF1dG9CaW5kUGFpcnMgPSBbXTtcblxuICAgIGluamVjdGVkTWl4aW5zLmZvckVhY2gobWl4U3BlY0ludG9Db21wb25lbnQuYmluZChudWxsLCBDb25zdHJ1Y3RvcikpO1xuXG4gICAgbWl4U3BlY0ludG9Db21wb25lbnQoQ29uc3RydWN0b3IsIElzTW91bnRlZFByZU1peGluKTtcbiAgICBtaXhTcGVjSW50b0NvbXBvbmVudChDb25zdHJ1Y3Rvciwgc3BlYyk7XG4gICAgbWl4U3BlY0ludG9Db21wb25lbnQoQ29uc3RydWN0b3IsIElzTW91bnRlZFBvc3RNaXhpbik7XG5cbiAgICAvLyBJbml0aWFsaXplIHRoZSBkZWZhdWx0UHJvcHMgcHJvcGVydHkgYWZ0ZXIgYWxsIG1peGlucyBoYXZlIGJlZW4gbWVyZ2VkLlxuICAgIGlmIChDb25zdHJ1Y3Rvci5nZXREZWZhdWx0UHJvcHMpIHtcbiAgICAgIENvbnN0cnVjdG9yLmRlZmF1bHRQcm9wcyA9IENvbnN0cnVjdG9yLmdldERlZmF1bHRQcm9wcygpO1xuICAgIH1cblxuICAgIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgICAvLyBUaGlzIGlzIGEgdGFnIHRvIGluZGljYXRlIHRoYXQgdGhlIHVzZSBvZiB0aGVzZSBtZXRob2QgbmFtZXMgaXMgb2ssXG4gICAgICAvLyBzaW5jZSBpdCdzIHVzZWQgd2l0aCBjcmVhdGVDbGFzcy4gSWYgaXQncyBub3QsIHRoZW4gaXQncyBsaWtlbHkgYVxuICAgICAgLy8gbWlzdGFrZSBzbyB3ZSdsbCB3YXJuIHlvdSB0byB1c2UgdGhlIHN0YXRpYyBwcm9wZXJ0eSwgcHJvcGVydHlcbiAgICAgIC8vIGluaXRpYWxpemVyIG9yIGNvbnN0cnVjdG9yIHJlc3BlY3RpdmVseS5cbiAgICAgIGlmIChDb25zdHJ1Y3Rvci5nZXREZWZhdWx0UHJvcHMpIHtcbiAgICAgICAgQ29uc3RydWN0b3IuZ2V0RGVmYXVsdFByb3BzLmlzUmVhY3RDbGFzc0FwcHJvdmVkID0ge307XG4gICAgICB9XG4gICAgICBpZiAoQ29uc3RydWN0b3IucHJvdG90eXBlLmdldEluaXRpYWxTdGF0ZSkge1xuICAgICAgICBDb25zdHJ1Y3Rvci5wcm90b3R5cGUuZ2V0SW5pdGlhbFN0YXRlLmlzUmVhY3RDbGFzc0FwcHJvdmVkID0ge307XG4gICAgICB9XG4gICAgfVxuXG4gICAgX2ludmFyaWFudChcbiAgICAgIENvbnN0cnVjdG9yLnByb3RvdHlwZS5yZW5kZXIsXG4gICAgICAnY3JlYXRlQ2xhc3MoLi4uKTogQ2xhc3Mgc3BlY2lmaWNhdGlvbiBtdXN0IGltcGxlbWVudCBhIGByZW5kZXJgIG1ldGhvZC4nXG4gICAgKTtcblxuICAgIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgICB3YXJuaW5nKFxuICAgICAgICAhQ29uc3RydWN0b3IucHJvdG90eXBlLmNvbXBvbmVudFNob3VsZFVwZGF0ZSxcbiAgICAgICAgJyVzIGhhcyBhIG1ldGhvZCBjYWxsZWQgJyArXG4gICAgICAgICAgJ2NvbXBvbmVudFNob3VsZFVwZGF0ZSgpLiBEaWQgeW91IG1lYW4gc2hvdWxkQ29tcG9uZW50VXBkYXRlKCk/ICcgK1xuICAgICAgICAgICdUaGUgbmFtZSBpcyBwaHJhc2VkIGFzIGEgcXVlc3Rpb24gYmVjYXVzZSB0aGUgZnVuY3Rpb24gaXMgJyArXG4gICAgICAgICAgJ2V4cGVjdGVkIHRvIHJldHVybiBhIHZhbHVlLicsXG4gICAgICAgIHNwZWMuZGlzcGxheU5hbWUgfHwgJ0EgY29tcG9uZW50J1xuICAgICAgKTtcbiAgICAgIHdhcm5pbmcoXG4gICAgICAgICFDb25zdHJ1Y3Rvci5wcm90b3R5cGUuY29tcG9uZW50V2lsbFJlY2lldmVQcm9wcyxcbiAgICAgICAgJyVzIGhhcyBhIG1ldGhvZCBjYWxsZWQgJyArXG4gICAgICAgICAgJ2NvbXBvbmVudFdpbGxSZWNpZXZlUHJvcHMoKS4gRGlkIHlvdSBtZWFuIGNvbXBvbmVudFdpbGxSZWNlaXZlUHJvcHMoKT8nLFxuICAgICAgICBzcGVjLmRpc3BsYXlOYW1lIHx8ICdBIGNvbXBvbmVudCdcbiAgICAgICk7XG4gICAgICB3YXJuaW5nKFxuICAgICAgICAhQ29uc3RydWN0b3IucHJvdG90eXBlLlVOU0FGRV9jb21wb25lbnRXaWxsUmVjaWV2ZVByb3BzLFxuICAgICAgICAnJXMgaGFzIGEgbWV0aG9kIGNhbGxlZCBVTlNBRkVfY29tcG9uZW50V2lsbFJlY2lldmVQcm9wcygpLiAnICtcbiAgICAgICAgICAnRGlkIHlvdSBtZWFuIFVOU0FGRV9jb21wb25lbnRXaWxsUmVjZWl2ZVByb3BzKCk/JyxcbiAgICAgICAgc3BlYy5kaXNwbGF5TmFtZSB8fCAnQSBjb21wb25lbnQnXG4gICAgICApO1xuICAgIH1cblxuICAgIC8vIFJlZHVjZSB0aW1lIHNwZW50IGRvaW5nIGxvb2t1cHMgYnkgc2V0dGluZyB0aGVzZSBvbiB0aGUgcHJvdG90eXBlLlxuICAgIGZvciAodmFyIG1ldGhvZE5hbWUgaW4gUmVhY3RDbGFzc0ludGVyZmFjZSkge1xuICAgICAgaWYgKCFDb25zdHJ1Y3Rvci5wcm90b3R5cGVbbWV0aG9kTmFtZV0pIHtcbiAgICAgICAgQ29uc3RydWN0b3IucHJvdG90eXBlW21ldGhvZE5hbWVdID0gbnVsbDtcbiAgICAgIH1cbiAgICB9XG5cbiAgICByZXR1cm4gQ29uc3RydWN0b3I7XG4gIH1cblxuICByZXR1cm4gY3JlYXRlQ2xhc3M7XG59XG5cbm1vZHVsZS5leHBvcnRzID0gZmFjdG9yeTtcbiIsIi8qKlxuICogQ29weXJpZ2h0IChjKSAyMDEzLXByZXNlbnQsIEZhY2Vib29rLCBJbmMuXG4gKlxuICogVGhpcyBzb3VyY2UgY29kZSBpcyBsaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UgZm91bmQgaW4gdGhlXG4gKiBMSUNFTlNFIGZpbGUgaW4gdGhlIHJvb3QgZGlyZWN0b3J5IG9mIHRoaXMgc291cmNlIHRyZWUuXG4gKlxuICovXG5cbid1c2Ugc3RyaWN0JztcblxudmFyIFJlYWN0ID0gcmVxdWlyZSgncmVhY3QnKTtcbnZhciBmYWN0b3J5ID0gcmVxdWlyZSgnLi9mYWN0b3J5Jyk7XG5cbmlmICh0eXBlb2YgUmVhY3QgPT09ICd1bmRlZmluZWQnKSB7XG4gIHRocm93IEVycm9yKFxuICAgICdjcmVhdGUtcmVhY3QtY2xhc3MgY291bGQgbm90IGZpbmQgdGhlIFJlYWN0IG9iamVjdC4gSWYgeW91IGFyZSB1c2luZyBzY3JpcHQgdGFncywgJyArXG4gICAgICAnbWFrZSBzdXJlIHRoYXQgUmVhY3QgaXMgYmVpbmcgbG9hZGVkIGJlZm9yZSBjcmVhdGUtcmVhY3QtY2xhc3MuJ1xuICApO1xufVxuXG4vLyBIYWNrIHRvIGdyYWIgTm9vcFVwZGF0ZVF1ZXVlIGZyb20gaXNvbW9ycGhpYyBSZWFjdFxudmFyIFJlYWN0Tm9vcFVwZGF0ZVF1ZXVlID0gbmV3IFJlYWN0LkNvbXBvbmVudCgpLnVwZGF0ZXI7XG5cbm1vZHVsZS5leHBvcnRzID0gZmFjdG9yeShcbiAgUmVhY3QuQ29tcG9uZW50LFxuICBSZWFjdC5pc1ZhbGlkRWxlbWVudCxcbiAgUmVhY3ROb29wVXBkYXRlUXVldWVcbik7XG4iLCJleHBvcnRzID0gbW9kdWxlLmV4cG9ydHMgPSByZXF1aXJlKFwiLi4vLi4vY3NzLWxvYWRlci9kaXN0L3J1bnRpbWUvYXBpLmpzXCIpKGZhbHNlKTtcbi8vIE1vZHVsZVxuZXhwb3J0cy5wdXNoKFttb2R1bGUuaWQsIFwiLyohXFxuICogaHR0cHM6Ly9naXRodWIuY29tL1lvdUNhbkJvb2tNZS9yZWFjdC1kYXRldGltZVxcbiAqL1xcblxcbi5yZHQge1xcbiAgcG9zaXRpb246IHJlbGF0aXZlO1xcbn1cXG4ucmR0UGlja2VyIHtcXG4gIGRpc3BsYXk6IG5vbmU7XFxuICBwb3NpdGlvbjogYWJzb2x1dGU7XFxuICB3aWR0aDogMjUwcHg7XFxuICBwYWRkaW5nOiA0cHg7XFxuICBtYXJnaW4tdG9wOiAxcHg7XFxuICB6LWluZGV4OiA5OTk5OSAhaW1wb3J0YW50O1xcbiAgYmFja2dyb3VuZDogI2ZmZjtcXG4gIGJveC1zaGFkb3c6IDAgMXB4IDNweCByZ2JhKDAsMCwwLC4xKTtcXG4gIGJvcmRlcjogMXB4IHNvbGlkICNmOWY5Zjk7XFxufVxcbi5yZHRPcGVuIC5yZHRQaWNrZXIge1xcbiAgZGlzcGxheTogYmxvY2s7XFxufVxcbi5yZHRTdGF0aWMgLnJkdFBpY2tlciB7XFxuICBib3gtc2hhZG93OiBub25lO1xcbiAgcG9zaXRpb246IHN0YXRpYztcXG59XFxuXFxuLnJkdFBpY2tlciAucmR0VGltZVRvZ2dsZSB7XFxuICB0ZXh0LWFsaWduOiBjZW50ZXI7XFxufVxcblxcbi5yZHRQaWNrZXIgdGFibGUge1xcbiAgd2lkdGg6IDEwMCU7XFxuICBtYXJnaW46IDA7XFxufVxcbi5yZHRQaWNrZXIgdGQsXFxuLnJkdFBpY2tlciB0aCB7XFxuICB0ZXh0LWFsaWduOiBjZW50ZXI7XFxuICBoZWlnaHQ6IDI4cHg7XFxufVxcbi5yZHRQaWNrZXIgdGQge1xcbiAgY3Vyc29yOiBwb2ludGVyO1xcbn1cXG4ucmR0UGlja2VyIHRkLnJkdERheTpob3ZlcixcXG4ucmR0UGlja2VyIHRkLnJkdEhvdXI6aG92ZXIsXFxuLnJkdFBpY2tlciB0ZC5yZHRNaW51dGU6aG92ZXIsXFxuLnJkdFBpY2tlciB0ZC5yZHRTZWNvbmQ6aG92ZXIsXFxuLnJkdFBpY2tlciAucmR0VGltZVRvZ2dsZTpob3ZlciB7XFxuICBiYWNrZ3JvdW5kOiAjZWVlZWVlO1xcbiAgY3Vyc29yOiBwb2ludGVyO1xcbn1cXG4ucmR0UGlja2VyIHRkLnJkdE9sZCxcXG4ucmR0UGlja2VyIHRkLnJkdE5ldyB7XFxuICBjb2xvcjogIzk5OTk5OTtcXG59XFxuLnJkdFBpY2tlciB0ZC5yZHRUb2RheSB7XFxuICBwb3NpdGlvbjogcmVsYXRpdmU7XFxufVxcbi5yZHRQaWNrZXIgdGQucmR0VG9kYXk6YmVmb3JlIHtcXG4gIGNvbnRlbnQ6ICcnO1xcbiAgZGlzcGxheTogaW5saW5lLWJsb2NrO1xcbiAgYm9yZGVyLWxlZnQ6IDdweCBzb2xpZCB0cmFuc3BhcmVudDtcXG4gIGJvcmRlci1ib3R0b206IDdweCBzb2xpZCAjNDI4YmNhO1xcbiAgYm9yZGVyLXRvcC1jb2xvcjogcmdiYSgwLCAwLCAwLCAwLjIpO1xcbiAgcG9zaXRpb246IGFic29sdXRlO1xcbiAgYm90dG9tOiA0cHg7XFxuICByaWdodDogNHB4O1xcbn1cXG4ucmR0UGlja2VyIHRkLnJkdEFjdGl2ZSxcXG4ucmR0UGlja2VyIHRkLnJkdEFjdGl2ZTpob3ZlciB7XFxuICBiYWNrZ3JvdW5kLWNvbG9yOiAjNDI4YmNhO1xcbiAgY29sb3I6ICNmZmY7XFxuICB0ZXh0LXNoYWRvdzogMCAtMXB4IDAgcmdiYSgwLCAwLCAwLCAwLjI1KTtcXG59XFxuLnJkdFBpY2tlciB0ZC5yZHRBY3RpdmUucmR0VG9kYXk6YmVmb3JlIHtcXG4gIGJvcmRlci1ib3R0b20tY29sb3I6ICNmZmY7XFxufVxcbi5yZHRQaWNrZXIgdGQucmR0RGlzYWJsZWQsXFxuLnJkdFBpY2tlciB0ZC5yZHREaXNhYmxlZDpob3ZlciB7XFxuICBiYWNrZ3JvdW5kOiBub25lO1xcbiAgY29sb3I6ICM5OTk5OTk7XFxuICBjdXJzb3I6IG5vdC1hbGxvd2VkO1xcbn1cXG5cXG4ucmR0UGlja2VyIHRkIHNwYW4ucmR0T2xkIHtcXG4gIGNvbG9yOiAjOTk5OTk5O1xcbn1cXG4ucmR0UGlja2VyIHRkIHNwYW4ucmR0RGlzYWJsZWQsXFxuLnJkdFBpY2tlciB0ZCBzcGFuLnJkdERpc2FibGVkOmhvdmVyIHtcXG4gIGJhY2tncm91bmQ6IG5vbmU7XFxuICBjb2xvcjogIzk5OTk5OTtcXG4gIGN1cnNvcjogbm90LWFsbG93ZWQ7XFxufVxcbi5yZHRQaWNrZXIgdGgge1xcbiAgYm9yZGVyLWJvdHRvbTogMXB4IHNvbGlkICNmOWY5Zjk7XFxufVxcbi5yZHRQaWNrZXIgLmRvdyB7XFxuICB3aWR0aDogMTQuMjg1NyU7XFxuICBib3JkZXItYm90dG9tOiBub25lO1xcbiAgY3Vyc29yOiBkZWZhdWx0O1xcbn1cXG4ucmR0UGlja2VyIHRoLnJkdFN3aXRjaCB7XFxuICB3aWR0aDogMTAwcHg7XFxufVxcbi5yZHRQaWNrZXIgdGgucmR0TmV4dCxcXG4ucmR0UGlja2VyIHRoLnJkdFByZXYge1xcbiAgZm9udC1zaXplOiAyMXB4O1xcbiAgdmVydGljYWwtYWxpZ246IHRvcDtcXG59XFxuXFxuLnJkdFByZXYgc3BhbixcXG4ucmR0TmV4dCBzcGFuIHtcXG4gIGRpc3BsYXk6IGJsb2NrO1xcbiAgLXdlYmtpdC10b3VjaC1jYWxsb3V0OiBub25lOyAvKiBpT1MgU2FmYXJpICovXFxuICAtd2Via2l0LXVzZXItc2VsZWN0OiBub25lOyAgIC8qIENocm9tZS9TYWZhcmkvT3BlcmEgKi9cXG4gIC1raHRtbC11c2VyLXNlbGVjdDogbm9uZTsgICAgLyogS29ucXVlcm9yICovXFxuICAtbW96LXVzZXItc2VsZWN0OiBub25lOyAgICAgIC8qIEZpcmVmb3ggKi9cXG4gIC1tcy11c2VyLXNlbGVjdDogbm9uZTsgICAgICAgLyogSW50ZXJuZXQgRXhwbG9yZXIvRWRnZSAqL1xcbiAgdXNlci1zZWxlY3Q6IG5vbmU7XFxufVxcblxcbi5yZHRQaWNrZXIgdGgucmR0RGlzYWJsZWQsXFxuLnJkdFBpY2tlciB0aC5yZHREaXNhYmxlZDpob3ZlciB7XFxuICBiYWNrZ3JvdW5kOiBub25lO1xcbiAgY29sb3I6ICM5OTk5OTk7XFxuICBjdXJzb3I6IG5vdC1hbGxvd2VkO1xcbn1cXG4ucmR0UGlja2VyIHRoZWFkIHRyOmZpcnN0LWNoaWxkIHRoIHtcXG4gIGN1cnNvcjogcG9pbnRlcjtcXG59XFxuLnJkdFBpY2tlciB0aGVhZCB0cjpmaXJzdC1jaGlsZCB0aDpob3ZlciB7XFxuICBiYWNrZ3JvdW5kOiAjZWVlZWVlO1xcbn1cXG5cXG4ucmR0UGlja2VyIHRmb290IHtcXG4gIGJvcmRlci10b3A6IDFweCBzb2xpZCAjZjlmOWY5O1xcbn1cXG5cXG4ucmR0UGlja2VyIGJ1dHRvbiB7XFxuICBib3JkZXI6IG5vbmU7XFxuICBiYWNrZ3JvdW5kOiBub25lO1xcbiAgY3Vyc29yOiBwb2ludGVyO1xcbn1cXG4ucmR0UGlja2VyIGJ1dHRvbjpob3ZlciB7XFxuICBiYWNrZ3JvdW5kLWNvbG9yOiAjZWVlO1xcbn1cXG5cXG4ucmR0UGlja2VyIHRoZWFkIGJ1dHRvbiB7XFxuICB3aWR0aDogMTAwJTtcXG4gIGhlaWdodDogMTAwJTtcXG59XFxuXFxudGQucmR0TW9udGgsXFxudGQucmR0WWVhciB7XFxuICBoZWlnaHQ6IDUwcHg7XFxuICB3aWR0aDogMjUlO1xcbiAgY3Vyc29yOiBwb2ludGVyO1xcbn1cXG50ZC5yZHRNb250aDpob3ZlcixcXG50ZC5yZHRZZWFyOmhvdmVyIHtcXG4gIGJhY2tncm91bmQ6ICNlZWU7XFxufVxcblxcbi5yZHRDb3VudGVycyB7XFxuICBkaXNwbGF5OiBpbmxpbmUtYmxvY2s7XFxufVxcblxcbi5yZHRDb3VudGVycyA+IGRpdiB7XFxuICBmbG9hdDogbGVmdDtcXG59XFxuXFxuLnJkdENvdW50ZXIge1xcbiAgaGVpZ2h0OiAxMDBweDtcXG59XFxuXFxuLnJkdENvdW50ZXIge1xcbiAgd2lkdGg6IDQwcHg7XFxufVxcblxcbi5yZHRDb3VudGVyU2VwYXJhdG9yIHtcXG4gIGxpbmUtaGVpZ2h0OiAxMDBweDtcXG59XFxuXFxuLnJkdENvdW50ZXIgLnJkdEJ0biB7XFxuICBoZWlnaHQ6IDQwJTtcXG4gIGxpbmUtaGVpZ2h0OiA0MHB4O1xcbiAgY3Vyc29yOiBwb2ludGVyO1xcbiAgZGlzcGxheTogYmxvY2s7XFxuXFxuICAtd2Via2l0LXRvdWNoLWNhbGxvdXQ6IG5vbmU7IC8qIGlPUyBTYWZhcmkgKi9cXG4gIC13ZWJraXQtdXNlci1zZWxlY3Q6IG5vbmU7ICAgLyogQ2hyb21lL1NhZmFyaS9PcGVyYSAqL1xcbiAgLWtodG1sLXVzZXItc2VsZWN0OiBub25lOyAgICAvKiBLb25xdWVyb3IgKi9cXG4gIC1tb3otdXNlci1zZWxlY3Q6IG5vbmU7ICAgICAgLyogRmlyZWZveCAqL1xcbiAgLW1zLXVzZXItc2VsZWN0OiBub25lOyAgICAgICAvKiBJbnRlcm5ldCBFeHBsb3Jlci9FZGdlICovXFxuICB1c2VyLXNlbGVjdDogbm9uZTtcXG59XFxuLnJkdENvdW50ZXIgLnJkdEJ0bjpob3ZlciB7XFxuICBiYWNrZ3JvdW5kOiAjZWVlO1xcbn1cXG4ucmR0Q291bnRlciAucmR0Q291bnQge1xcbiAgaGVpZ2h0OiAyMCU7XFxuICBmb250LXNpemU6IDEuMmVtO1xcbn1cXG5cXG4ucmR0TWlsbGkge1xcbiAgdmVydGljYWwtYWxpZ246IG1pZGRsZTtcXG4gIHBhZGRpbmctbGVmdDogOHB4O1xcbiAgd2lkdGg6IDQ4cHg7XFxufVxcblxcbi5yZHRNaWxsaSBpbnB1dCB7XFxuICB3aWR0aDogMTAwJTtcXG4gIGZvbnQtc2l6ZTogMS4yZW07XFxuICBtYXJnaW4tdG9wOiAzN3B4O1xcbn1cXG5cXG4ucmR0VGltZSB0ZCB7XFxuICBjdXJzb3I6IGRlZmF1bHQ7XFxufVxcblwiLCBcIlwiXSk7XG4iLCJcInVzZSBzdHJpY3RcIjtcblxuLypcbiAgTUlUIExpY2Vuc2UgaHR0cDovL3d3dy5vcGVuc291cmNlLm9yZy9saWNlbnNlcy9taXQtbGljZW5zZS5waHBcbiAgQXV0aG9yIFRvYmlhcyBLb3BwZXJzIEBzb2tyYVxuKi9cbi8vIGNzcyBiYXNlIGNvZGUsIGluamVjdGVkIGJ5IHRoZSBjc3MtbG9hZGVyXG4vLyBlc2xpbnQtZGlzYWJsZS1uZXh0LWxpbmUgZnVuYy1uYW1lc1xubW9kdWxlLmV4cG9ydHMgPSBmdW5jdGlvbiAodXNlU291cmNlTWFwKSB7XG4gIHZhciBsaXN0ID0gW107IC8vIHJldHVybiB0aGUgbGlzdCBvZiBtb2R1bGVzIGFzIGNzcyBzdHJpbmdcblxuICBsaXN0LnRvU3RyaW5nID0gZnVuY3Rpb24gdG9TdHJpbmcoKSB7XG4gICAgcmV0dXJuIHRoaXMubWFwKGZ1bmN0aW9uIChpdGVtKSB7XG4gICAgICB2YXIgY29udGVudCA9IGNzc1dpdGhNYXBwaW5nVG9TdHJpbmcoaXRlbSwgdXNlU291cmNlTWFwKTtcblxuICAgICAgaWYgKGl0ZW1bMl0pIHtcbiAgICAgICAgcmV0dXJuIFwiQG1lZGlhIFwiLmNvbmNhdChpdGVtWzJdLCBcIntcIikuY29uY2F0KGNvbnRlbnQsIFwifVwiKTtcbiAgICAgIH1cblxuICAgICAgcmV0dXJuIGNvbnRlbnQ7XG4gICAgfSkuam9pbignJyk7XG4gIH07IC8vIGltcG9ydCBhIGxpc3Qgb2YgbW9kdWxlcyBpbnRvIHRoZSBsaXN0XG4gIC8vIGVzbGludC1kaXNhYmxlLW5leHQtbGluZSBmdW5jLW5hbWVzXG5cblxuICBsaXN0LmkgPSBmdW5jdGlvbiAobW9kdWxlcywgbWVkaWFRdWVyeSkge1xuICAgIGlmICh0eXBlb2YgbW9kdWxlcyA9PT0gJ3N0cmluZycpIHtcbiAgICAgIC8vIGVzbGludC1kaXNhYmxlLW5leHQtbGluZSBuby1wYXJhbS1yZWFzc2lnblxuICAgICAgbW9kdWxlcyA9IFtbbnVsbCwgbW9kdWxlcywgJyddXTtcbiAgICB9XG5cbiAgICB2YXIgYWxyZWFkeUltcG9ydGVkTW9kdWxlcyA9IHt9O1xuXG4gICAgZm9yICh2YXIgaSA9IDA7IGkgPCB0aGlzLmxlbmd0aDsgaSsrKSB7XG4gICAgICAvLyBlc2xpbnQtZGlzYWJsZS1uZXh0LWxpbmUgcHJlZmVyLWRlc3RydWN0dXJpbmdcbiAgICAgIHZhciBpZCA9IHRoaXNbaV1bMF07XG5cbiAgICAgIGlmIChpZCAhPSBudWxsKSB7XG4gICAgICAgIGFscmVhZHlJbXBvcnRlZE1vZHVsZXNbaWRdID0gdHJ1ZTtcbiAgICAgIH1cbiAgICB9XG5cbiAgICBmb3IgKHZhciBfaSA9IDA7IF9pIDwgbW9kdWxlcy5sZW5ndGg7IF9pKyspIHtcbiAgICAgIHZhciBpdGVtID0gbW9kdWxlc1tfaV07IC8vIHNraXAgYWxyZWFkeSBpbXBvcnRlZCBtb2R1bGVcbiAgICAgIC8vIHRoaXMgaW1wbGVtZW50YXRpb24gaXMgbm90IDEwMCUgcGVyZmVjdCBmb3Igd2VpcmQgbWVkaWEgcXVlcnkgY29tYmluYXRpb25zXG4gICAgICAvLyB3aGVuIGEgbW9kdWxlIGlzIGltcG9ydGVkIG11bHRpcGxlIHRpbWVzIHdpdGggZGlmZmVyZW50IG1lZGlhIHF1ZXJpZXMuXG4gICAgICAvLyBJIGhvcGUgdGhpcyB3aWxsIG5ldmVyIG9jY3VyIChIZXkgdGhpcyB3YXkgd2UgaGF2ZSBzbWFsbGVyIGJ1bmRsZXMpXG5cbiAgICAgIGlmIChpdGVtWzBdID09IG51bGwgfHwgIWFscmVhZHlJbXBvcnRlZE1vZHVsZXNbaXRlbVswXV0pIHtcbiAgICAgICAgaWYgKG1lZGlhUXVlcnkgJiYgIWl0ZW1bMl0pIHtcbiAgICAgICAgICBpdGVtWzJdID0gbWVkaWFRdWVyeTtcbiAgICAgICAgfSBlbHNlIGlmIChtZWRpYVF1ZXJ5KSB7XG4gICAgICAgICAgaXRlbVsyXSA9IFwiKFwiLmNvbmNhdChpdGVtWzJdLCBcIikgYW5kIChcIikuY29uY2F0KG1lZGlhUXVlcnksIFwiKVwiKTtcbiAgICAgICAgfVxuXG4gICAgICAgIGxpc3QucHVzaChpdGVtKTtcbiAgICAgIH1cbiAgICB9XG4gIH07XG5cbiAgcmV0dXJuIGxpc3Q7XG59O1xuXG5mdW5jdGlvbiBjc3NXaXRoTWFwcGluZ1RvU3RyaW5nKGl0ZW0sIHVzZVNvdXJjZU1hcCkge1xuICB2YXIgY29udGVudCA9IGl0ZW1bMV0gfHwgJyc7IC8vIGVzbGludC1kaXNhYmxlLW5leHQtbGluZSBwcmVmZXItZGVzdHJ1Y3R1cmluZ1xuXG4gIHZhciBjc3NNYXBwaW5nID0gaXRlbVszXTtcblxuICBpZiAoIWNzc01hcHBpbmcpIHtcbiAgICByZXR1cm4gY29udGVudDtcbiAgfVxuXG4gIGlmICh1c2VTb3VyY2VNYXAgJiYgdHlwZW9mIGJ0b2EgPT09ICdmdW5jdGlvbicpIHtcbiAgICB2YXIgc291cmNlTWFwcGluZyA9IHRvQ29tbWVudChjc3NNYXBwaW5nKTtcbiAgICB2YXIgc291cmNlVVJMcyA9IGNzc01hcHBpbmcuc291cmNlcy5tYXAoZnVuY3Rpb24gKHNvdXJjZSkge1xuICAgICAgcmV0dXJuIFwiLyojIHNvdXJjZVVSTD1cIi5jb25jYXQoY3NzTWFwcGluZy5zb3VyY2VSb290KS5jb25jYXQoc291cmNlLCBcIiAqL1wiKTtcbiAgICB9KTtcbiAgICByZXR1cm4gW2NvbnRlbnRdLmNvbmNhdChzb3VyY2VVUkxzKS5jb25jYXQoW3NvdXJjZU1hcHBpbmddKS5qb2luKCdcXG4nKTtcbiAgfVxuXG4gIHJldHVybiBbY29udGVudF0uam9pbignXFxuJyk7XG59IC8vIEFkYXB0ZWQgZnJvbSBjb252ZXJ0LXNvdXJjZS1tYXAgKE1JVClcblxuXG5mdW5jdGlvbiB0b0NvbW1lbnQoc291cmNlTWFwKSB7XG4gIC8vIGVzbGludC1kaXNhYmxlLW5leHQtbGluZSBuby11bmRlZlxuICB2YXIgYmFzZTY0ID0gYnRvYSh1bmVzY2FwZShlbmNvZGVVUklDb21wb25lbnQoSlNPTi5zdHJpbmdpZnkoc291cmNlTWFwKSkpKTtcbiAgdmFyIGRhdGEgPSBcInNvdXJjZU1hcHBpbmdVUkw9ZGF0YTphcHBsaWNhdGlvbi9qc29uO2NoYXJzZXQ9dXRmLTg7YmFzZTY0LFwiLmNvbmNhdChiYXNlNjQpO1xuICByZXR1cm4gXCIvKiMgXCIuY29uY2F0KGRhdGEsIFwiICovXCIpO1xufSIsIid1c2Ugc3RyaWN0JztcbnZhciB0b2tlbiA9ICclW2EtZjAtOV17Mn0nO1xudmFyIHNpbmdsZU1hdGNoZXIgPSBuZXcgUmVnRXhwKHRva2VuLCAnZ2knKTtcbnZhciBtdWx0aU1hdGNoZXIgPSBuZXcgUmVnRXhwKCcoJyArIHRva2VuICsgJykrJywgJ2dpJyk7XG5cbmZ1bmN0aW9uIGRlY29kZUNvbXBvbmVudHMoY29tcG9uZW50cywgc3BsaXQpIHtcblx0dHJ5IHtcblx0XHQvLyBUcnkgdG8gZGVjb2RlIHRoZSBlbnRpcmUgc3RyaW5nIGZpcnN0XG5cdFx0cmV0dXJuIGRlY29kZVVSSUNvbXBvbmVudChjb21wb25lbnRzLmpvaW4oJycpKTtcblx0fSBjYXRjaCAoZXJyKSB7XG5cdFx0Ly8gRG8gbm90aGluZ1xuXHR9XG5cblx0aWYgKGNvbXBvbmVudHMubGVuZ3RoID09PSAxKSB7XG5cdFx0cmV0dXJuIGNvbXBvbmVudHM7XG5cdH1cblxuXHRzcGxpdCA9IHNwbGl0IHx8IDE7XG5cblx0Ly8gU3BsaXQgdGhlIGFycmF5IGluIDIgcGFydHNcblx0dmFyIGxlZnQgPSBjb21wb25lbnRzLnNsaWNlKDAsIHNwbGl0KTtcblx0dmFyIHJpZ2h0ID0gY29tcG9uZW50cy5zbGljZShzcGxpdCk7XG5cblx0cmV0dXJuIEFycmF5LnByb3RvdHlwZS5jb25jYXQuY2FsbChbXSwgZGVjb2RlQ29tcG9uZW50cyhsZWZ0KSwgZGVjb2RlQ29tcG9uZW50cyhyaWdodCkpO1xufVxuXG5mdW5jdGlvbiBkZWNvZGUoaW5wdXQpIHtcblx0dHJ5IHtcblx0XHRyZXR1cm4gZGVjb2RlVVJJQ29tcG9uZW50KGlucHV0KTtcblx0fSBjYXRjaCAoZXJyKSB7XG5cdFx0dmFyIHRva2VucyA9IGlucHV0Lm1hdGNoKHNpbmdsZU1hdGNoZXIpO1xuXG5cdFx0Zm9yICh2YXIgaSA9IDE7IGkgPCB0b2tlbnMubGVuZ3RoOyBpKyspIHtcblx0XHRcdGlucHV0ID0gZGVjb2RlQ29tcG9uZW50cyh0b2tlbnMsIGkpLmpvaW4oJycpO1xuXG5cdFx0XHR0b2tlbnMgPSBpbnB1dC5tYXRjaChzaW5nbGVNYXRjaGVyKTtcblx0XHR9XG5cblx0XHRyZXR1cm4gaW5wdXQ7XG5cdH1cbn1cblxuZnVuY3Rpb24gY3VzdG9tRGVjb2RlVVJJQ29tcG9uZW50KGlucHV0KSB7XG5cdC8vIEtlZXAgdHJhY2sgb2YgYWxsIHRoZSByZXBsYWNlbWVudHMgYW5kIHByZWZpbGwgdGhlIG1hcCB3aXRoIHRoZSBgQk9NYFxuXHR2YXIgcmVwbGFjZU1hcCA9IHtcblx0XHQnJUZFJUZGJzogJ1xcdUZGRkRcXHVGRkZEJyxcblx0XHQnJUZGJUZFJzogJ1xcdUZGRkRcXHVGRkZEJ1xuXHR9O1xuXG5cdHZhciBtYXRjaCA9IG11bHRpTWF0Y2hlci5leGVjKGlucHV0KTtcblx0d2hpbGUgKG1hdGNoKSB7XG5cdFx0dHJ5IHtcblx0XHRcdC8vIERlY29kZSBhcyBiaWcgY2h1bmtzIGFzIHBvc3NpYmxlXG5cdFx0XHRyZXBsYWNlTWFwW21hdGNoWzBdXSA9IGRlY29kZVVSSUNvbXBvbmVudChtYXRjaFswXSk7XG5cdFx0fSBjYXRjaCAoZXJyKSB7XG5cdFx0XHR2YXIgcmVzdWx0ID0gZGVjb2RlKG1hdGNoWzBdKTtcblxuXHRcdFx0aWYgKHJlc3VsdCAhPT0gbWF0Y2hbMF0pIHtcblx0XHRcdFx0cmVwbGFjZU1hcFttYXRjaFswXV0gPSByZXN1bHQ7XG5cdFx0XHR9XG5cdFx0fVxuXG5cdFx0bWF0Y2ggPSBtdWx0aU1hdGNoZXIuZXhlYyhpbnB1dCk7XG5cdH1cblxuXHQvLyBBZGQgYCVDMmAgYXQgdGhlIGVuZCBvZiB0aGUgbWFwIHRvIG1ha2Ugc3VyZSBpdCBkb2VzIG5vdCByZXBsYWNlIHRoZSBjb21iaW5hdG9yIGJlZm9yZSBldmVyeXRoaW5nIGVsc2Vcblx0cmVwbGFjZU1hcFsnJUMyJ10gPSAnXFx1RkZGRCc7XG5cblx0dmFyIGVudHJpZXMgPSBPYmplY3Qua2V5cyhyZXBsYWNlTWFwKTtcblxuXHRmb3IgKHZhciBpID0gMDsgaSA8IGVudHJpZXMubGVuZ3RoOyBpKyspIHtcblx0XHQvLyBSZXBsYWNlIGFsbCBkZWNvZGVkIGNvbXBvbmVudHNcblx0XHR2YXIga2V5ID0gZW50cmllc1tpXTtcblx0XHRpbnB1dCA9IGlucHV0LnJlcGxhY2UobmV3IFJlZ0V4cChrZXksICdnJyksIHJlcGxhY2VNYXBba2V5XSk7XG5cdH1cblxuXHRyZXR1cm4gaW5wdXQ7XG59XG5cbm1vZHVsZS5leHBvcnRzID0gZnVuY3Rpb24gKGVuY29kZWRVUkkpIHtcblx0aWYgKHR5cGVvZiBlbmNvZGVkVVJJICE9PSAnc3RyaW5nJykge1xuXHRcdHRocm93IG5ldyBUeXBlRXJyb3IoJ0V4cGVjdGVkIGBlbmNvZGVkVVJJYCB0byBiZSBvZiB0eXBlIGBzdHJpbmdgLCBnb3QgYCcgKyB0eXBlb2YgZW5jb2RlZFVSSSArICdgJyk7XG5cdH1cblxuXHR0cnkge1xuXHRcdGVuY29kZWRVUkkgPSBlbmNvZGVkVVJJLnJlcGxhY2UoL1xcKy9nLCAnICcpO1xuXG5cdFx0Ly8gVHJ5IHRoZSBidWlsdCBpbiBkZWNvZGVyIGZpcnN0XG5cdFx0cmV0dXJuIGRlY29kZVVSSUNvbXBvbmVudChlbmNvZGVkVVJJKTtcblx0fSBjYXRjaCAoZXJyKSB7XG5cdFx0Ly8gRmFsbGJhY2sgdG8gYSBtb3JlIGFkdmFuY2VkIGRlY29kZXJcblx0XHRyZXR1cm4gY3VzdG9tRGVjb2RlVVJJQ29tcG9uZW50KGVuY29kZWRVUkkpO1xuXHR9XG59O1xuIiwiXCJ1c2Ugc3RyaWN0XCI7XG5cbi8qKlxuICogQ29weXJpZ2h0IChjKSAyMDEzLXByZXNlbnQsIEZhY2Vib29rLCBJbmMuXG4gKlxuICogVGhpcyBzb3VyY2UgY29kZSBpcyBsaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UgZm91bmQgaW4gdGhlXG4gKiBMSUNFTlNFIGZpbGUgaW4gdGhlIHJvb3QgZGlyZWN0b3J5IG9mIHRoaXMgc291cmNlIHRyZWUuXG4gKlxuICogXG4gKi9cblxuZnVuY3Rpb24gbWFrZUVtcHR5RnVuY3Rpb24oYXJnKSB7XG4gIHJldHVybiBmdW5jdGlvbiAoKSB7XG4gICAgcmV0dXJuIGFyZztcbiAgfTtcbn1cblxuLyoqXG4gKiBUaGlzIGZ1bmN0aW9uIGFjY2VwdHMgYW5kIGRpc2NhcmRzIGlucHV0czsgaXQgaGFzIG5vIHNpZGUgZWZmZWN0cy4gVGhpcyBpc1xuICogcHJpbWFyaWx5IHVzZWZ1bCBpZGlvbWF0aWNhbGx5IGZvciBvdmVycmlkYWJsZSBmdW5jdGlvbiBlbmRwb2ludHMgd2hpY2hcbiAqIGFsd2F5cyBuZWVkIHRvIGJlIGNhbGxhYmxlLCBzaW5jZSBKUyBsYWNrcyBhIG51bGwtY2FsbCBpZGlvbSBhbGEgQ29jb2EuXG4gKi9cbnZhciBlbXB0eUZ1bmN0aW9uID0gZnVuY3Rpb24gZW1wdHlGdW5jdGlvbigpIHt9O1xuXG5lbXB0eUZ1bmN0aW9uLnRoYXRSZXR1cm5zID0gbWFrZUVtcHR5RnVuY3Rpb247XG5lbXB0eUZ1bmN0aW9uLnRoYXRSZXR1cm5zRmFsc2UgPSBtYWtlRW1wdHlGdW5jdGlvbihmYWxzZSk7XG5lbXB0eUZ1bmN0aW9uLnRoYXRSZXR1cm5zVHJ1ZSA9IG1ha2VFbXB0eUZ1bmN0aW9uKHRydWUpO1xuZW1wdHlGdW5jdGlvbi50aGF0UmV0dXJuc051bGwgPSBtYWtlRW1wdHlGdW5jdGlvbihudWxsKTtcbmVtcHR5RnVuY3Rpb24udGhhdFJldHVybnNUaGlzID0gZnVuY3Rpb24gKCkge1xuICByZXR1cm4gdGhpcztcbn07XG5lbXB0eUZ1bmN0aW9uLnRoYXRSZXR1cm5zQXJndW1lbnQgPSBmdW5jdGlvbiAoYXJnKSB7XG4gIHJldHVybiBhcmc7XG59O1xuXG5tb2R1bGUuZXhwb3J0cyA9IGVtcHR5RnVuY3Rpb247IiwiLyoqXG4gKiBDb3B5cmlnaHQgKGMpIDIwMTMtcHJlc2VudCwgRmFjZWJvb2ssIEluYy5cbiAqXG4gKiBUaGlzIHNvdXJjZSBjb2RlIGlzIGxpY2Vuc2VkIHVuZGVyIHRoZSBNSVQgbGljZW5zZSBmb3VuZCBpbiB0aGVcbiAqIExJQ0VOU0UgZmlsZSBpbiB0aGUgcm9vdCBkaXJlY3Rvcnkgb2YgdGhpcyBzb3VyY2UgdHJlZS5cbiAqXG4gKi9cblxuJ3VzZSBzdHJpY3QnO1xuXG52YXIgZW1wdHlPYmplY3QgPSB7fTtcblxuaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgT2JqZWN0LmZyZWV6ZShlbXB0eU9iamVjdCk7XG59XG5cbm1vZHVsZS5leHBvcnRzID0gZW1wdHlPYmplY3Q7IiwiLyoqXG4gKiBDb3B5cmlnaHQgKGMpIDIwMTMtcHJlc2VudCwgRmFjZWJvb2ssIEluYy5cbiAqXG4gKiBUaGlzIHNvdXJjZSBjb2RlIGlzIGxpY2Vuc2VkIHVuZGVyIHRoZSBNSVQgbGljZW5zZSBmb3VuZCBpbiB0aGVcbiAqIExJQ0VOU0UgZmlsZSBpbiB0aGUgcm9vdCBkaXJlY3Rvcnkgb2YgdGhpcyBzb3VyY2UgdHJlZS5cbiAqXG4gKi9cblxuJ3VzZSBzdHJpY3QnO1xuXG4vKipcbiAqIFVzZSBpbnZhcmlhbnQoKSB0byBhc3NlcnQgc3RhdGUgd2hpY2ggeW91ciBwcm9ncmFtIGFzc3VtZXMgdG8gYmUgdHJ1ZS5cbiAqXG4gKiBQcm92aWRlIHNwcmludGYtc3R5bGUgZm9ybWF0IChvbmx5ICVzIGlzIHN1cHBvcnRlZCkgYW5kIGFyZ3VtZW50c1xuICogdG8gcHJvdmlkZSBpbmZvcm1hdGlvbiBhYm91dCB3aGF0IGJyb2tlIGFuZCB3aGF0IHlvdSB3ZXJlXG4gKiBleHBlY3RpbmcuXG4gKlxuICogVGhlIGludmFyaWFudCBtZXNzYWdlIHdpbGwgYmUgc3RyaXBwZWQgaW4gcHJvZHVjdGlvbiwgYnV0IHRoZSBpbnZhcmlhbnRcbiAqIHdpbGwgcmVtYWluIHRvIGVuc3VyZSBsb2dpYyBkb2VzIG5vdCBkaWZmZXIgaW4gcHJvZHVjdGlvbi5cbiAqL1xuXG52YXIgdmFsaWRhdGVGb3JtYXQgPSBmdW5jdGlvbiB2YWxpZGF0ZUZvcm1hdChmb3JtYXQpIHt9O1xuXG5pZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICB2YWxpZGF0ZUZvcm1hdCA9IGZ1bmN0aW9uIHZhbGlkYXRlRm9ybWF0KGZvcm1hdCkge1xuICAgIGlmIChmb3JtYXQgPT09IHVuZGVmaW5lZCkge1xuICAgICAgdGhyb3cgbmV3IEVycm9yKCdpbnZhcmlhbnQgcmVxdWlyZXMgYW4gZXJyb3IgbWVzc2FnZSBhcmd1bWVudCcpO1xuICAgIH1cbiAgfTtcbn1cblxuZnVuY3Rpb24gaW52YXJpYW50KGNvbmRpdGlvbiwgZm9ybWF0LCBhLCBiLCBjLCBkLCBlLCBmKSB7XG4gIHZhbGlkYXRlRm9ybWF0KGZvcm1hdCk7XG5cbiAgaWYgKCFjb25kaXRpb24pIHtcbiAgICB2YXIgZXJyb3I7XG4gICAgaWYgKGZvcm1hdCA9PT0gdW5kZWZpbmVkKSB7XG4gICAgICBlcnJvciA9IG5ldyBFcnJvcignTWluaWZpZWQgZXhjZXB0aW9uIG9jY3VycmVkOyB1c2UgdGhlIG5vbi1taW5pZmllZCBkZXYgZW52aXJvbm1lbnQgJyArICdmb3IgdGhlIGZ1bGwgZXJyb3IgbWVzc2FnZSBhbmQgYWRkaXRpb25hbCBoZWxwZnVsIHdhcm5pbmdzLicpO1xuICAgIH0gZWxzZSB7XG4gICAgICB2YXIgYXJncyA9IFthLCBiLCBjLCBkLCBlLCBmXTtcbiAgICAgIHZhciBhcmdJbmRleCA9IDA7XG4gICAgICBlcnJvciA9IG5ldyBFcnJvcihmb3JtYXQucmVwbGFjZSgvJXMvZywgZnVuY3Rpb24gKCkge1xuICAgICAgICByZXR1cm4gYXJnc1thcmdJbmRleCsrXTtcbiAgICAgIH0pKTtcbiAgICAgIGVycm9yLm5hbWUgPSAnSW52YXJpYW50IFZpb2xhdGlvbic7XG4gICAgfVxuXG4gICAgZXJyb3IuZnJhbWVzVG9Qb3AgPSAxOyAvLyB3ZSBkb24ndCBjYXJlIGFib3V0IGludmFyaWFudCdzIG93biBmcmFtZVxuICAgIHRocm93IGVycm9yO1xuICB9XG59XG5cbm1vZHVsZS5leHBvcnRzID0gaW52YXJpYW50OyIsIi8qKlxuICogQ29weXJpZ2h0IChjKSAyMDE0LXByZXNlbnQsIEZhY2Vib29rLCBJbmMuXG4gKlxuICogVGhpcyBzb3VyY2UgY29kZSBpcyBsaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UgZm91bmQgaW4gdGhlXG4gKiBMSUNFTlNFIGZpbGUgaW4gdGhlIHJvb3QgZGlyZWN0b3J5IG9mIHRoaXMgc291cmNlIHRyZWUuXG4gKlxuICovXG5cbid1c2Ugc3RyaWN0JztcblxudmFyIGVtcHR5RnVuY3Rpb24gPSByZXF1aXJlKCcuL2VtcHR5RnVuY3Rpb24nKTtcblxuLyoqXG4gKiBTaW1pbGFyIHRvIGludmFyaWFudCBidXQgb25seSBsb2dzIGEgd2FybmluZyBpZiB0aGUgY29uZGl0aW9uIGlzIG5vdCBtZXQuXG4gKiBUaGlzIGNhbiBiZSB1c2VkIHRvIGxvZyBpc3N1ZXMgaW4gZGV2ZWxvcG1lbnQgZW52aXJvbm1lbnRzIGluIGNyaXRpY2FsXG4gKiBwYXRocy4gUmVtb3ZpbmcgdGhlIGxvZ2dpbmcgY29kZSBmb3IgcHJvZHVjdGlvbiBlbnZpcm9ubWVudHMgd2lsbCBrZWVwIHRoZVxuICogc2FtZSBsb2dpYyBhbmQgZm9sbG93IHRoZSBzYW1lIGNvZGUgcGF0aHMuXG4gKi9cblxudmFyIHdhcm5pbmcgPSBlbXB0eUZ1bmN0aW9uO1xuXG5pZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICB2YXIgcHJpbnRXYXJuaW5nID0gZnVuY3Rpb24gcHJpbnRXYXJuaW5nKGZvcm1hdCkge1xuICAgIGZvciAodmFyIF9sZW4gPSBhcmd1bWVudHMubGVuZ3RoLCBhcmdzID0gQXJyYXkoX2xlbiA+IDEgPyBfbGVuIC0gMSA6IDApLCBfa2V5ID0gMTsgX2tleSA8IF9sZW47IF9rZXkrKykge1xuICAgICAgYXJnc1tfa2V5IC0gMV0gPSBhcmd1bWVudHNbX2tleV07XG4gICAgfVxuXG4gICAgdmFyIGFyZ0luZGV4ID0gMDtcbiAgICB2YXIgbWVzc2FnZSA9ICdXYXJuaW5nOiAnICsgZm9ybWF0LnJlcGxhY2UoLyVzL2csIGZ1bmN0aW9uICgpIHtcbiAgICAgIHJldHVybiBhcmdzW2FyZ0luZGV4KytdO1xuICAgIH0pO1xuICAgIGlmICh0eXBlb2YgY29uc29sZSAhPT0gJ3VuZGVmaW5lZCcpIHtcbiAgICAgIGNvbnNvbGUuZXJyb3IobWVzc2FnZSk7XG4gICAgfVxuICAgIHRyeSB7XG4gICAgICAvLyAtLS0gV2VsY29tZSB0byBkZWJ1Z2dpbmcgUmVhY3QgLS0tXG4gICAgICAvLyBUaGlzIGVycm9yIHdhcyB0aHJvd24gYXMgYSBjb252ZW5pZW5jZSBzbyB0aGF0IHlvdSBjYW4gdXNlIHRoaXMgc3RhY2tcbiAgICAgIC8vIHRvIGZpbmQgdGhlIGNhbGxzaXRlIHRoYXQgY2F1c2VkIHRoaXMgd2FybmluZyB0byBmaXJlLlxuICAgICAgdGhyb3cgbmV3IEVycm9yKG1lc3NhZ2UpO1xuICAgIH0gY2F0Y2ggKHgpIHt9XG4gIH07XG5cbiAgd2FybmluZyA9IGZ1bmN0aW9uIHdhcm5pbmcoY29uZGl0aW9uLCBmb3JtYXQpIHtcbiAgICBpZiAoZm9ybWF0ID09PSB1bmRlZmluZWQpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcignYHdhcm5pbmcoY29uZGl0aW9uLCBmb3JtYXQsIC4uLmFyZ3MpYCByZXF1aXJlcyBhIHdhcm5pbmcgJyArICdtZXNzYWdlIGFyZ3VtZW50Jyk7XG4gICAgfVxuXG4gICAgaWYgKGZvcm1hdC5pbmRleE9mKCdGYWlsZWQgQ29tcG9zaXRlIHByb3BUeXBlOiAnKSA9PT0gMCkge1xuICAgICAgcmV0dXJuOyAvLyBJZ25vcmUgQ29tcG9zaXRlQ29tcG9uZW50IHByb3B0eXBlIGNoZWNrLlxuICAgIH1cblxuICAgIGlmICghY29uZGl0aW9uKSB7XG4gICAgICBmb3IgKHZhciBfbGVuMiA9IGFyZ3VtZW50cy5sZW5ndGgsIGFyZ3MgPSBBcnJheShfbGVuMiA+IDIgPyBfbGVuMiAtIDIgOiAwKSwgX2tleTIgPSAyOyBfa2V5MiA8IF9sZW4yOyBfa2V5MisrKSB7XG4gICAgICAgIGFyZ3NbX2tleTIgLSAyXSA9IGFyZ3VtZW50c1tfa2V5Ml07XG4gICAgICB9XG5cbiAgICAgIHByaW50V2FybmluZy5hcHBseSh1bmRlZmluZWQsIFtmb3JtYXRdLmNvbmNhdChhcmdzKSk7XG4gICAgfVxuICB9O1xufVxuXG5tb2R1bGUuZXhwb3J0cyA9IHdhcm5pbmc7IiwiJ3VzZSBzdHJpY3QnO1xuXG5leHBvcnRzLl9fZXNNb2R1bGUgPSB0cnVlO1xudmFyIGNhblVzZURPTSA9IGV4cG9ydHMuY2FuVXNlRE9NID0gISEodHlwZW9mIHdpbmRvdyAhPT0gJ3VuZGVmaW5lZCcgJiYgd2luZG93LmRvY3VtZW50ICYmIHdpbmRvdy5kb2N1bWVudC5jcmVhdGVFbGVtZW50KTtcblxudmFyIGFkZEV2ZW50TGlzdGVuZXIgPSBleHBvcnRzLmFkZEV2ZW50TGlzdGVuZXIgPSBmdW5jdGlvbiBhZGRFdmVudExpc3RlbmVyKG5vZGUsIGV2ZW50LCBsaXN0ZW5lcikge1xuICByZXR1cm4gbm9kZS5hZGRFdmVudExpc3RlbmVyID8gbm9kZS5hZGRFdmVudExpc3RlbmVyKGV2ZW50LCBsaXN0ZW5lciwgZmFsc2UpIDogbm9kZS5hdHRhY2hFdmVudCgnb24nICsgZXZlbnQsIGxpc3RlbmVyKTtcbn07XG5cbnZhciByZW1vdmVFdmVudExpc3RlbmVyID0gZXhwb3J0cy5yZW1vdmVFdmVudExpc3RlbmVyID0gZnVuY3Rpb24gcmVtb3ZlRXZlbnRMaXN0ZW5lcihub2RlLCBldmVudCwgbGlzdGVuZXIpIHtcbiAgcmV0dXJuIG5vZGUucmVtb3ZlRXZlbnRMaXN0ZW5lciA/IG5vZGUucmVtb3ZlRXZlbnRMaXN0ZW5lcihldmVudCwgbGlzdGVuZXIsIGZhbHNlKSA6IG5vZGUuZGV0YWNoRXZlbnQoJ29uJyArIGV2ZW50LCBsaXN0ZW5lcik7XG59O1xuXG52YXIgZ2V0Q29uZmlybWF0aW9uID0gZXhwb3J0cy5nZXRDb25maXJtYXRpb24gPSBmdW5jdGlvbiBnZXRDb25maXJtYXRpb24obWVzc2FnZSwgY2FsbGJhY2spIHtcbiAgcmV0dXJuIGNhbGxiYWNrKHdpbmRvdy5jb25maXJtKG1lc3NhZ2UpKTtcbn07IC8vIGVzbGludC1kaXNhYmxlLWxpbmUgbm8tYWxlcnRcblxuLyoqXG4gKiBSZXR1cm5zIHRydWUgaWYgdGhlIEhUTUw1IGhpc3RvcnkgQVBJIGlzIHN1cHBvcnRlZC4gVGFrZW4gZnJvbSBNb2Rlcm5penIuXG4gKlxuICogaHR0cHM6Ly9naXRodWIuY29tL01vZGVybml6ci9Nb2Rlcm5penIvYmxvYi9tYXN0ZXIvTElDRU5TRVxuICogaHR0cHM6Ly9naXRodWIuY29tL01vZGVybml6ci9Nb2Rlcm5penIvYmxvYi9tYXN0ZXIvZmVhdHVyZS1kZXRlY3RzL2hpc3RvcnkuanNcbiAqIGNoYW5nZWQgdG8gYXZvaWQgZmFsc2UgbmVnYXRpdmVzIGZvciBXaW5kb3dzIFBob25lczogaHR0cHM6Ly9naXRodWIuY29tL3JlYWN0anMvcmVhY3Qtcm91dGVyL2lzc3Vlcy81ODZcbiAqL1xudmFyIHN1cHBvcnRzSGlzdG9yeSA9IGV4cG9ydHMuc3VwcG9ydHNIaXN0b3J5ID0gZnVuY3Rpb24gc3VwcG9ydHNIaXN0b3J5KCkge1xuICB2YXIgdWEgPSB3aW5kb3cubmF2aWdhdG9yLnVzZXJBZ2VudDtcblxuICBpZiAoKHVhLmluZGV4T2YoJ0FuZHJvaWQgMi4nKSAhPT0gLTEgfHwgdWEuaW5kZXhPZignQW5kcm9pZCA0LjAnKSAhPT0gLTEpICYmIHVhLmluZGV4T2YoJ01vYmlsZSBTYWZhcmknKSAhPT0gLTEgJiYgdWEuaW5kZXhPZignQ2hyb21lJykgPT09IC0xICYmIHVhLmluZGV4T2YoJ1dpbmRvd3MgUGhvbmUnKSA9PT0gLTEpIHJldHVybiBmYWxzZTtcblxuICByZXR1cm4gd2luZG93Lmhpc3RvcnkgJiYgJ3B1c2hTdGF0ZScgaW4gd2luZG93Lmhpc3Rvcnk7XG59O1xuXG4vKipcbiAqIFJldHVybnMgdHJ1ZSBpZiBicm93c2VyIGZpcmVzIHBvcHN0YXRlIG9uIGhhc2ggY2hhbmdlLlxuICogSUUxMCBhbmQgSUUxMSBkbyBub3QuXG4gKi9cbnZhciBzdXBwb3J0c1BvcFN0YXRlT25IYXNoQ2hhbmdlID0gZXhwb3J0cy5zdXBwb3J0c1BvcFN0YXRlT25IYXNoQ2hhbmdlID0gZnVuY3Rpb24gc3VwcG9ydHNQb3BTdGF0ZU9uSGFzaENoYW5nZSgpIHtcbiAgcmV0dXJuIHdpbmRvdy5uYXZpZ2F0b3IudXNlckFnZW50LmluZGV4T2YoJ1RyaWRlbnQnKSA9PT0gLTE7XG59O1xuXG4vKipcbiAqIFJldHVybnMgZmFsc2UgaWYgdXNpbmcgZ28obikgd2l0aCBoYXNoIGhpc3RvcnkgY2F1c2VzIGEgZnVsbCBwYWdlIHJlbG9hZC5cbiAqL1xudmFyIHN1cHBvcnRzR29XaXRob3V0UmVsb2FkVXNpbmdIYXNoID0gZXhwb3J0cy5zdXBwb3J0c0dvV2l0aG91dFJlbG9hZFVzaW5nSGFzaCA9IGZ1bmN0aW9uIHN1cHBvcnRzR29XaXRob3V0UmVsb2FkVXNpbmdIYXNoKCkge1xuICByZXR1cm4gd2luZG93Lm5hdmlnYXRvci51c2VyQWdlbnQuaW5kZXhPZignRmlyZWZveCcpID09PSAtMTtcbn07XG5cbi8qKlxuICogUmV0dXJucyB0cnVlIGlmIGEgZ2l2ZW4gcG9wc3RhdGUgZXZlbnQgaXMgYW4gZXh0cmFuZW91cyBXZWJLaXQgZXZlbnQuXG4gKiBBY2NvdW50cyBmb3IgdGhlIGZhY3QgdGhhdCBDaHJvbWUgb24gaU9TIGZpcmVzIHJlYWwgcG9wc3RhdGUgZXZlbnRzXG4gKiBjb250YWluaW5nIHVuZGVmaW5lZCBzdGF0ZSB3aGVuIHByZXNzaW5nIHRoZSBiYWNrIGJ1dHRvbi5cbiAqL1xudmFyIGlzRXh0cmFuZW91c1BvcHN0YXRlRXZlbnQgPSBleHBvcnRzLmlzRXh0cmFuZW91c1BvcHN0YXRlRXZlbnQgPSBmdW5jdGlvbiBpc0V4dHJhbmVvdXNQb3BzdGF0ZUV2ZW50KGV2ZW50KSB7XG4gIHJldHVybiBldmVudC5zdGF0ZSA9PT0gdW5kZWZpbmVkICYmIG5hdmlnYXRvci51c2VyQWdlbnQuaW5kZXhPZignQ3JpT1MnKSA9PT0gLTE7XG59OyIsIid1c2Ugc3RyaWN0JztcblxuZXhwb3J0cy5fX2VzTW9kdWxlID0gdHJ1ZTtcbmV4cG9ydHMubG9jYXRpb25zQXJlRXF1YWwgPSBleHBvcnRzLmNyZWF0ZUxvY2F0aW9uID0gdW5kZWZpbmVkO1xuXG52YXIgX2V4dGVuZHMgPSBPYmplY3QuYXNzaWduIHx8IGZ1bmN0aW9uICh0YXJnZXQpIHsgZm9yICh2YXIgaSA9IDE7IGkgPCBhcmd1bWVudHMubGVuZ3RoOyBpKyspIHsgdmFyIHNvdXJjZSA9IGFyZ3VtZW50c1tpXTsgZm9yICh2YXIga2V5IGluIHNvdXJjZSkgeyBpZiAoT2JqZWN0LnByb3RvdHlwZS5oYXNPd25Qcm9wZXJ0eS5jYWxsKHNvdXJjZSwga2V5KSkgeyB0YXJnZXRba2V5XSA9IHNvdXJjZVtrZXldOyB9IH0gfSByZXR1cm4gdGFyZ2V0OyB9O1xuXG52YXIgX3Jlc29sdmVQYXRobmFtZSA9IHJlcXVpcmUoJ3Jlc29sdmUtcGF0aG5hbWUnKTtcblxudmFyIF9yZXNvbHZlUGF0aG5hbWUyID0gX2ludGVyb3BSZXF1aXJlRGVmYXVsdChfcmVzb2x2ZVBhdGhuYW1lKTtcblxudmFyIF92YWx1ZUVxdWFsID0gcmVxdWlyZSgndmFsdWUtZXF1YWwnKTtcblxudmFyIF92YWx1ZUVxdWFsMiA9IF9pbnRlcm9wUmVxdWlyZURlZmF1bHQoX3ZhbHVlRXF1YWwpO1xuXG52YXIgX1BhdGhVdGlscyA9IHJlcXVpcmUoJy4vUGF0aFV0aWxzJyk7XG5cbmZ1bmN0aW9uIF9pbnRlcm9wUmVxdWlyZURlZmF1bHQob2JqKSB7IHJldHVybiBvYmogJiYgb2JqLl9fZXNNb2R1bGUgPyBvYmogOiB7IGRlZmF1bHQ6IG9iaiB9OyB9XG5cbnZhciBjcmVhdGVMb2NhdGlvbiA9IGV4cG9ydHMuY3JlYXRlTG9jYXRpb24gPSBmdW5jdGlvbiBjcmVhdGVMb2NhdGlvbihwYXRoLCBzdGF0ZSwga2V5LCBjdXJyZW50TG9jYXRpb24pIHtcbiAgdmFyIGxvY2F0aW9uID0gdm9pZCAwO1xuICBpZiAodHlwZW9mIHBhdGggPT09ICdzdHJpbmcnKSB7XG4gICAgLy8gVHdvLWFyZyBmb3JtOiBwdXNoKHBhdGgsIHN0YXRlKVxuICAgIGxvY2F0aW9uID0gKDAsIF9QYXRoVXRpbHMucGFyc2VQYXRoKShwYXRoKTtcbiAgICBsb2NhdGlvbi5zdGF0ZSA9IHN0YXRlO1xuICB9IGVsc2Uge1xuICAgIC8vIE9uZS1hcmcgZm9ybTogcHVzaChsb2NhdGlvbilcbiAgICBsb2NhdGlvbiA9IF9leHRlbmRzKHt9LCBwYXRoKTtcblxuICAgIGlmIChsb2NhdGlvbi5wYXRobmFtZSA9PT0gdW5kZWZpbmVkKSBsb2NhdGlvbi5wYXRobmFtZSA9ICcnO1xuXG4gICAgaWYgKGxvY2F0aW9uLnNlYXJjaCkge1xuICAgICAgaWYgKGxvY2F0aW9uLnNlYXJjaC5jaGFyQXQoMCkgIT09ICc/JykgbG9jYXRpb24uc2VhcmNoID0gJz8nICsgbG9jYXRpb24uc2VhcmNoO1xuICAgIH0gZWxzZSB7XG4gICAgICBsb2NhdGlvbi5zZWFyY2ggPSAnJztcbiAgICB9XG5cbiAgICBpZiAobG9jYXRpb24uaGFzaCkge1xuICAgICAgaWYgKGxvY2F0aW9uLmhhc2guY2hhckF0KDApICE9PSAnIycpIGxvY2F0aW9uLmhhc2ggPSAnIycgKyBsb2NhdGlvbi5oYXNoO1xuICAgIH0gZWxzZSB7XG4gICAgICBsb2NhdGlvbi5oYXNoID0gJyc7XG4gICAgfVxuXG4gICAgaWYgKHN0YXRlICE9PSB1bmRlZmluZWQgJiYgbG9jYXRpb24uc3RhdGUgPT09IHVuZGVmaW5lZCkgbG9jYXRpb24uc3RhdGUgPSBzdGF0ZTtcbiAgfVxuXG4gIHRyeSB7XG4gICAgbG9jYXRpb24ucGF0aG5hbWUgPSBkZWNvZGVVUkkobG9jYXRpb24ucGF0aG5hbWUpO1xuICB9IGNhdGNoIChlKSB7XG4gICAgaWYgKGUgaW5zdGFuY2VvZiBVUklFcnJvcikge1xuICAgICAgdGhyb3cgbmV3IFVSSUVycm9yKCdQYXRobmFtZSBcIicgKyBsb2NhdGlvbi5wYXRobmFtZSArICdcIiBjb3VsZCBub3QgYmUgZGVjb2RlZC4gJyArICdUaGlzIGlzIGxpa2VseSBjYXVzZWQgYnkgYW4gaW52YWxpZCBwZXJjZW50LWVuY29kaW5nLicpO1xuICAgIH0gZWxzZSB7XG4gICAgICB0aHJvdyBlO1xuICAgIH1cbiAgfVxuXG4gIGlmIChrZXkpIGxvY2F0aW9uLmtleSA9IGtleTtcblxuICBpZiAoY3VycmVudExvY2F0aW9uKSB7XG4gICAgLy8gUmVzb2x2ZSBpbmNvbXBsZXRlL3JlbGF0aXZlIHBhdGhuYW1lIHJlbGF0aXZlIHRvIGN1cnJlbnQgbG9jYXRpb24uXG4gICAgaWYgKCFsb2NhdGlvbi5wYXRobmFtZSkge1xuICAgICAgbG9jYXRpb24ucGF0aG5hbWUgPSBjdXJyZW50TG9jYXRpb24ucGF0aG5hbWU7XG4gICAgfSBlbHNlIGlmIChsb2NhdGlvbi5wYXRobmFtZS5jaGFyQXQoMCkgIT09ICcvJykge1xuICAgICAgbG9jYXRpb24ucGF0aG5hbWUgPSAoMCwgX3Jlc29sdmVQYXRobmFtZTIuZGVmYXVsdCkobG9jYXRpb24ucGF0aG5hbWUsIGN1cnJlbnRMb2NhdGlvbi5wYXRobmFtZSk7XG4gICAgfVxuICB9IGVsc2Uge1xuICAgIC8vIFdoZW4gdGhlcmUgaXMgbm8gcHJpb3IgbG9jYXRpb24gYW5kIHBhdGhuYW1lIGlzIGVtcHR5LCBzZXQgaXQgdG8gL1xuICAgIGlmICghbG9jYXRpb24ucGF0aG5hbWUpIHtcbiAgICAgIGxvY2F0aW9uLnBhdGhuYW1lID0gJy8nO1xuICAgIH1cbiAgfVxuXG4gIHJldHVybiBsb2NhdGlvbjtcbn07XG5cbnZhciBsb2NhdGlvbnNBcmVFcXVhbCA9IGV4cG9ydHMubG9jYXRpb25zQXJlRXF1YWwgPSBmdW5jdGlvbiBsb2NhdGlvbnNBcmVFcXVhbChhLCBiKSB7XG4gIHJldHVybiBhLnBhdGhuYW1lID09PSBiLnBhdGhuYW1lICYmIGEuc2VhcmNoID09PSBiLnNlYXJjaCAmJiBhLmhhc2ggPT09IGIuaGFzaCAmJiBhLmtleSA9PT0gYi5rZXkgJiYgKDAsIF92YWx1ZUVxdWFsMi5kZWZhdWx0KShhLnN0YXRlLCBiLnN0YXRlKTtcbn07IiwiJ3VzZSBzdHJpY3QnO1xuXG5leHBvcnRzLl9fZXNNb2R1bGUgPSB0cnVlO1xudmFyIGFkZExlYWRpbmdTbGFzaCA9IGV4cG9ydHMuYWRkTGVhZGluZ1NsYXNoID0gZnVuY3Rpb24gYWRkTGVhZGluZ1NsYXNoKHBhdGgpIHtcbiAgcmV0dXJuIHBhdGguY2hhckF0KDApID09PSAnLycgPyBwYXRoIDogJy8nICsgcGF0aDtcbn07XG5cbnZhciBzdHJpcExlYWRpbmdTbGFzaCA9IGV4cG9ydHMuc3RyaXBMZWFkaW5nU2xhc2ggPSBmdW5jdGlvbiBzdHJpcExlYWRpbmdTbGFzaChwYXRoKSB7XG4gIHJldHVybiBwYXRoLmNoYXJBdCgwKSA9PT0gJy8nID8gcGF0aC5zdWJzdHIoMSkgOiBwYXRoO1xufTtcblxudmFyIGhhc0Jhc2VuYW1lID0gZXhwb3J0cy5oYXNCYXNlbmFtZSA9IGZ1bmN0aW9uIGhhc0Jhc2VuYW1lKHBhdGgsIHByZWZpeCkge1xuICByZXR1cm4gbmV3IFJlZ0V4cCgnXicgKyBwcmVmaXggKyAnKFxcXFwvfFxcXFw/fCN8JCknLCAnaScpLnRlc3QocGF0aCk7XG59O1xuXG52YXIgc3RyaXBCYXNlbmFtZSA9IGV4cG9ydHMuc3RyaXBCYXNlbmFtZSA9IGZ1bmN0aW9uIHN0cmlwQmFzZW5hbWUocGF0aCwgcHJlZml4KSB7XG4gIHJldHVybiBoYXNCYXNlbmFtZShwYXRoLCBwcmVmaXgpID8gcGF0aC5zdWJzdHIocHJlZml4Lmxlbmd0aCkgOiBwYXRoO1xufTtcblxudmFyIHN0cmlwVHJhaWxpbmdTbGFzaCA9IGV4cG9ydHMuc3RyaXBUcmFpbGluZ1NsYXNoID0gZnVuY3Rpb24gc3RyaXBUcmFpbGluZ1NsYXNoKHBhdGgpIHtcbiAgcmV0dXJuIHBhdGguY2hhckF0KHBhdGgubGVuZ3RoIC0gMSkgPT09ICcvJyA/IHBhdGguc2xpY2UoMCwgLTEpIDogcGF0aDtcbn07XG5cbnZhciBwYXJzZVBhdGggPSBleHBvcnRzLnBhcnNlUGF0aCA9IGZ1bmN0aW9uIHBhcnNlUGF0aChwYXRoKSB7XG4gIHZhciBwYXRobmFtZSA9IHBhdGggfHwgJy8nO1xuICB2YXIgc2VhcmNoID0gJyc7XG4gIHZhciBoYXNoID0gJyc7XG5cbiAgdmFyIGhhc2hJbmRleCA9IHBhdGhuYW1lLmluZGV4T2YoJyMnKTtcbiAgaWYgKGhhc2hJbmRleCAhPT0gLTEpIHtcbiAgICBoYXNoID0gcGF0aG5hbWUuc3Vic3RyKGhhc2hJbmRleCk7XG4gICAgcGF0aG5hbWUgPSBwYXRobmFtZS5zdWJzdHIoMCwgaGFzaEluZGV4KTtcbiAgfVxuXG4gIHZhciBzZWFyY2hJbmRleCA9IHBhdGhuYW1lLmluZGV4T2YoJz8nKTtcbiAgaWYgKHNlYXJjaEluZGV4ICE9PSAtMSkge1xuICAgIHNlYXJjaCA9IHBhdGhuYW1lLnN1YnN0cihzZWFyY2hJbmRleCk7XG4gICAgcGF0aG5hbWUgPSBwYXRobmFtZS5zdWJzdHIoMCwgc2VhcmNoSW5kZXgpO1xuICB9XG5cbiAgcmV0dXJuIHtcbiAgICBwYXRobmFtZTogcGF0aG5hbWUsXG4gICAgc2VhcmNoOiBzZWFyY2ggPT09ICc/JyA/ICcnIDogc2VhcmNoLFxuICAgIGhhc2g6IGhhc2ggPT09ICcjJyA/ICcnIDogaGFzaFxuICB9O1xufTtcblxudmFyIGNyZWF0ZVBhdGggPSBleHBvcnRzLmNyZWF0ZVBhdGggPSBmdW5jdGlvbiBjcmVhdGVQYXRoKGxvY2F0aW9uKSB7XG4gIHZhciBwYXRobmFtZSA9IGxvY2F0aW9uLnBhdGhuYW1lLFxuICAgICAgc2VhcmNoID0gbG9jYXRpb24uc2VhcmNoLFxuICAgICAgaGFzaCA9IGxvY2F0aW9uLmhhc2g7XG5cblxuICB2YXIgcGF0aCA9IHBhdGhuYW1lIHx8ICcvJztcblxuICBpZiAoc2VhcmNoICYmIHNlYXJjaCAhPT0gJz8nKSBwYXRoICs9IHNlYXJjaC5jaGFyQXQoMCkgPT09ICc/JyA/IHNlYXJjaCA6ICc/JyArIHNlYXJjaDtcblxuICBpZiAoaGFzaCAmJiBoYXNoICE9PSAnIycpIHBhdGggKz0gaGFzaC5jaGFyQXQoMCkgPT09ICcjJyA/IGhhc2ggOiAnIycgKyBoYXNoO1xuXG4gIHJldHVybiBwYXRoO1xufTsiLCIndXNlIHN0cmljdCc7XG5cbmV4cG9ydHMuX19lc01vZHVsZSA9IHRydWU7XG5cbnZhciBfdHlwZW9mID0gdHlwZW9mIFN5bWJvbCA9PT0gXCJmdW5jdGlvblwiICYmIHR5cGVvZiBTeW1ib2wuaXRlcmF0b3IgPT09IFwic3ltYm9sXCIgPyBmdW5jdGlvbiAob2JqKSB7IHJldHVybiB0eXBlb2Ygb2JqOyB9IDogZnVuY3Rpb24gKG9iaikgeyByZXR1cm4gb2JqICYmIHR5cGVvZiBTeW1ib2wgPT09IFwiZnVuY3Rpb25cIiAmJiBvYmouY29uc3RydWN0b3IgPT09IFN5bWJvbCAmJiBvYmogIT09IFN5bWJvbC5wcm90b3R5cGUgPyBcInN5bWJvbFwiIDogdHlwZW9mIG9iajsgfTtcblxudmFyIF9leHRlbmRzID0gT2JqZWN0LmFzc2lnbiB8fCBmdW5jdGlvbiAodGFyZ2V0KSB7IGZvciAodmFyIGkgPSAxOyBpIDwgYXJndW1lbnRzLmxlbmd0aDsgaSsrKSB7IHZhciBzb3VyY2UgPSBhcmd1bWVudHNbaV07IGZvciAodmFyIGtleSBpbiBzb3VyY2UpIHsgaWYgKE9iamVjdC5wcm90b3R5cGUuaGFzT3duUHJvcGVydHkuY2FsbChzb3VyY2UsIGtleSkpIHsgdGFyZ2V0W2tleV0gPSBzb3VyY2Vba2V5XTsgfSB9IH0gcmV0dXJuIHRhcmdldDsgfTtcblxudmFyIF93YXJuaW5nID0gcmVxdWlyZSgnd2FybmluZycpO1xuXG52YXIgX3dhcm5pbmcyID0gX2ludGVyb3BSZXF1aXJlRGVmYXVsdChfd2FybmluZyk7XG5cbnZhciBfaW52YXJpYW50ID0gcmVxdWlyZSgnaW52YXJpYW50Jyk7XG5cbnZhciBfaW52YXJpYW50MiA9IF9pbnRlcm9wUmVxdWlyZURlZmF1bHQoX2ludmFyaWFudCk7XG5cbnZhciBfTG9jYXRpb25VdGlscyA9IHJlcXVpcmUoJy4vTG9jYXRpb25VdGlscycpO1xuXG52YXIgX1BhdGhVdGlscyA9IHJlcXVpcmUoJy4vUGF0aFV0aWxzJyk7XG5cbnZhciBfY3JlYXRlVHJhbnNpdGlvbk1hbmFnZXIgPSByZXF1aXJlKCcuL2NyZWF0ZVRyYW5zaXRpb25NYW5hZ2VyJyk7XG5cbnZhciBfY3JlYXRlVHJhbnNpdGlvbk1hbmFnZXIyID0gX2ludGVyb3BSZXF1aXJlRGVmYXVsdChfY3JlYXRlVHJhbnNpdGlvbk1hbmFnZXIpO1xuXG52YXIgX0RPTVV0aWxzID0gcmVxdWlyZSgnLi9ET01VdGlscycpO1xuXG5mdW5jdGlvbiBfaW50ZXJvcFJlcXVpcmVEZWZhdWx0KG9iaikgeyByZXR1cm4gb2JqICYmIG9iai5fX2VzTW9kdWxlID8gb2JqIDogeyBkZWZhdWx0OiBvYmogfTsgfVxuXG52YXIgUG9wU3RhdGVFdmVudCA9ICdwb3BzdGF0ZSc7XG52YXIgSGFzaENoYW5nZUV2ZW50ID0gJ2hhc2hjaGFuZ2UnO1xuXG52YXIgZ2V0SGlzdG9yeVN0YXRlID0gZnVuY3Rpb24gZ2V0SGlzdG9yeVN0YXRlKCkge1xuICB0cnkge1xuICAgIHJldHVybiB3aW5kb3cuaGlzdG9yeS5zdGF0ZSB8fCB7fTtcbiAgfSBjYXRjaCAoZSkge1xuICAgIC8vIElFIDExIHNvbWV0aW1lcyB0aHJvd3Mgd2hlbiBhY2Nlc3Npbmcgd2luZG93Lmhpc3Rvcnkuc3RhdGVcbiAgICAvLyBTZWUgaHR0cHM6Ly9naXRodWIuY29tL1JlYWN0VHJhaW5pbmcvaGlzdG9yeS9wdWxsLzI4OVxuICAgIHJldHVybiB7fTtcbiAgfVxufTtcblxuLyoqXG4gKiBDcmVhdGVzIGEgaGlzdG9yeSBvYmplY3QgdGhhdCB1c2VzIHRoZSBIVE1MNSBoaXN0b3J5IEFQSSBpbmNsdWRpbmdcbiAqIHB1c2hTdGF0ZSwgcmVwbGFjZVN0YXRlLCBhbmQgdGhlIHBvcHN0YXRlIGV2ZW50LlxuICovXG52YXIgY3JlYXRlQnJvd3Nlckhpc3RvcnkgPSBmdW5jdGlvbiBjcmVhdGVCcm93c2VySGlzdG9yeSgpIHtcbiAgdmFyIHByb3BzID0gYXJndW1lbnRzLmxlbmd0aCA+IDAgJiYgYXJndW1lbnRzWzBdICE9PSB1bmRlZmluZWQgPyBhcmd1bWVudHNbMF0gOiB7fTtcblxuICAoMCwgX2ludmFyaWFudDIuZGVmYXVsdCkoX0RPTVV0aWxzLmNhblVzZURPTSwgJ0Jyb3dzZXIgaGlzdG9yeSBuZWVkcyBhIERPTScpO1xuXG4gIHZhciBnbG9iYWxIaXN0b3J5ID0gd2luZG93Lmhpc3Rvcnk7XG4gIHZhciBjYW5Vc2VIaXN0b3J5ID0gKDAsIF9ET01VdGlscy5zdXBwb3J0c0hpc3RvcnkpKCk7XG4gIHZhciBuZWVkc0hhc2hDaGFuZ2VMaXN0ZW5lciA9ICEoMCwgX0RPTVV0aWxzLnN1cHBvcnRzUG9wU3RhdGVPbkhhc2hDaGFuZ2UpKCk7XG5cbiAgdmFyIF9wcm9wcyRmb3JjZVJlZnJlc2ggPSBwcm9wcy5mb3JjZVJlZnJlc2gsXG4gICAgICBmb3JjZVJlZnJlc2ggPSBfcHJvcHMkZm9yY2VSZWZyZXNoID09PSB1bmRlZmluZWQgPyBmYWxzZSA6IF9wcm9wcyRmb3JjZVJlZnJlc2gsXG4gICAgICBfcHJvcHMkZ2V0VXNlckNvbmZpcm0gPSBwcm9wcy5nZXRVc2VyQ29uZmlybWF0aW9uLFxuICAgICAgZ2V0VXNlckNvbmZpcm1hdGlvbiA9IF9wcm9wcyRnZXRVc2VyQ29uZmlybSA9PT0gdW5kZWZpbmVkID8gX0RPTVV0aWxzLmdldENvbmZpcm1hdGlvbiA6IF9wcm9wcyRnZXRVc2VyQ29uZmlybSxcbiAgICAgIF9wcm9wcyRrZXlMZW5ndGggPSBwcm9wcy5rZXlMZW5ndGgsXG4gICAgICBrZXlMZW5ndGggPSBfcHJvcHMka2V5TGVuZ3RoID09PSB1bmRlZmluZWQgPyA2IDogX3Byb3BzJGtleUxlbmd0aDtcblxuICB2YXIgYmFzZW5hbWUgPSBwcm9wcy5iYXNlbmFtZSA/ICgwLCBfUGF0aFV0aWxzLnN0cmlwVHJhaWxpbmdTbGFzaCkoKDAsIF9QYXRoVXRpbHMuYWRkTGVhZGluZ1NsYXNoKShwcm9wcy5iYXNlbmFtZSkpIDogJyc7XG5cbiAgdmFyIGdldERPTUxvY2F0aW9uID0gZnVuY3Rpb24gZ2V0RE9NTG9jYXRpb24oaGlzdG9yeVN0YXRlKSB7XG4gICAgdmFyIF9yZWYgPSBoaXN0b3J5U3RhdGUgfHwge30sXG4gICAgICAgIGtleSA9IF9yZWYua2V5LFxuICAgICAgICBzdGF0ZSA9IF9yZWYuc3RhdGU7XG5cbiAgICB2YXIgX3dpbmRvdyRsb2NhdGlvbiA9IHdpbmRvdy5sb2NhdGlvbixcbiAgICAgICAgcGF0aG5hbWUgPSBfd2luZG93JGxvY2F0aW9uLnBhdGhuYW1lLFxuICAgICAgICBzZWFyY2ggPSBfd2luZG93JGxvY2F0aW9uLnNlYXJjaCxcbiAgICAgICAgaGFzaCA9IF93aW5kb3ckbG9jYXRpb24uaGFzaDtcblxuXG4gICAgdmFyIHBhdGggPSBwYXRobmFtZSArIHNlYXJjaCArIGhhc2g7XG5cbiAgICAoMCwgX3dhcm5pbmcyLmRlZmF1bHQpKCFiYXNlbmFtZSB8fCAoMCwgX1BhdGhVdGlscy5oYXNCYXNlbmFtZSkocGF0aCwgYmFzZW5hbWUpLCAnWW91IGFyZSBhdHRlbXB0aW5nIHRvIHVzZSBhIGJhc2VuYW1lIG9uIGEgcGFnZSB3aG9zZSBVUkwgcGF0aCBkb2VzIG5vdCBiZWdpbiAnICsgJ3dpdGggdGhlIGJhc2VuYW1lLiBFeHBlY3RlZCBwYXRoIFwiJyArIHBhdGggKyAnXCIgdG8gYmVnaW4gd2l0aCBcIicgKyBiYXNlbmFtZSArICdcIi4nKTtcblxuICAgIGlmIChiYXNlbmFtZSkgcGF0aCA9ICgwLCBfUGF0aFV0aWxzLnN0cmlwQmFzZW5hbWUpKHBhdGgsIGJhc2VuYW1lKTtcblxuICAgIHJldHVybiAoMCwgX0xvY2F0aW9uVXRpbHMuY3JlYXRlTG9jYXRpb24pKHBhdGgsIHN0YXRlLCBrZXkpO1xuICB9O1xuXG4gIHZhciBjcmVhdGVLZXkgPSBmdW5jdGlvbiBjcmVhdGVLZXkoKSB7XG4gICAgcmV0dXJuIE1hdGgucmFuZG9tKCkudG9TdHJpbmcoMzYpLnN1YnN0cigyLCBrZXlMZW5ndGgpO1xuICB9O1xuXG4gIHZhciB0cmFuc2l0aW9uTWFuYWdlciA9ICgwLCBfY3JlYXRlVHJhbnNpdGlvbk1hbmFnZXIyLmRlZmF1bHQpKCk7XG5cbiAgdmFyIHNldFN0YXRlID0gZnVuY3Rpb24gc2V0U3RhdGUobmV4dFN0YXRlKSB7XG4gICAgX2V4dGVuZHMoaGlzdG9yeSwgbmV4dFN0YXRlKTtcblxuICAgIGhpc3RvcnkubGVuZ3RoID0gZ2xvYmFsSGlzdG9yeS5sZW5ndGg7XG5cbiAgICB0cmFuc2l0aW9uTWFuYWdlci5ub3RpZnlMaXN0ZW5lcnMoaGlzdG9yeS5sb2NhdGlvbiwgaGlzdG9yeS5hY3Rpb24pO1xuICB9O1xuXG4gIHZhciBoYW5kbGVQb3BTdGF0ZSA9IGZ1bmN0aW9uIGhhbmRsZVBvcFN0YXRlKGV2ZW50KSB7XG4gICAgLy8gSWdub3JlIGV4dHJhbmVvdXMgcG9wc3RhdGUgZXZlbnRzIGluIFdlYktpdC5cbiAgICBpZiAoKDAsIF9ET01VdGlscy5pc0V4dHJhbmVvdXNQb3BzdGF0ZUV2ZW50KShldmVudCkpIHJldHVybjtcblxuICAgIGhhbmRsZVBvcChnZXRET01Mb2NhdGlvbihldmVudC5zdGF0ZSkpO1xuICB9O1xuXG4gIHZhciBoYW5kbGVIYXNoQ2hhbmdlID0gZnVuY3Rpb24gaGFuZGxlSGFzaENoYW5nZSgpIHtcbiAgICBoYW5kbGVQb3AoZ2V0RE9NTG9jYXRpb24oZ2V0SGlzdG9yeVN0YXRlKCkpKTtcbiAgfTtcblxuICB2YXIgZm9yY2VOZXh0UG9wID0gZmFsc2U7XG5cbiAgdmFyIGhhbmRsZVBvcCA9IGZ1bmN0aW9uIGhhbmRsZVBvcChsb2NhdGlvbikge1xuICAgIGlmIChmb3JjZU5leHRQb3ApIHtcbiAgICAgIGZvcmNlTmV4dFBvcCA9IGZhbHNlO1xuICAgICAgc2V0U3RhdGUoKTtcbiAgICB9IGVsc2Uge1xuICAgICAgdmFyIGFjdGlvbiA9ICdQT1AnO1xuXG4gICAgICB0cmFuc2l0aW9uTWFuYWdlci5jb25maXJtVHJhbnNpdGlvblRvKGxvY2F0aW9uLCBhY3Rpb24sIGdldFVzZXJDb25maXJtYXRpb24sIGZ1bmN0aW9uIChvaykge1xuICAgICAgICBpZiAob2spIHtcbiAgICAgICAgICBzZXRTdGF0ZSh7IGFjdGlvbjogYWN0aW9uLCBsb2NhdGlvbjogbG9jYXRpb24gfSk7XG4gICAgICAgIH0gZWxzZSB7XG4gICAgICAgICAgcmV2ZXJ0UG9wKGxvY2F0aW9uKTtcbiAgICAgICAgfVxuICAgICAgfSk7XG4gICAgfVxuICB9O1xuXG4gIHZhciByZXZlcnRQb3AgPSBmdW5jdGlvbiByZXZlcnRQb3AoZnJvbUxvY2F0aW9uKSB7XG4gICAgdmFyIHRvTG9jYXRpb24gPSBoaXN0b3J5LmxvY2F0aW9uO1xuXG4gICAgLy8gVE9ETzogV2UgY291bGQgcHJvYmFibHkgbWFrZSB0aGlzIG1vcmUgcmVsaWFibGUgYnlcbiAgICAvLyBrZWVwaW5nIGEgbGlzdCBvZiBrZXlzIHdlJ3ZlIHNlZW4gaW4gc2Vzc2lvblN0b3JhZ2UuXG4gICAgLy8gSW5zdGVhZCwgd2UganVzdCBkZWZhdWx0IHRvIDAgZm9yIGtleXMgd2UgZG9uJ3Qga25vdy5cblxuICAgIHZhciB0b0luZGV4ID0gYWxsS2V5cy5pbmRleE9mKHRvTG9jYXRpb24ua2V5KTtcblxuICAgIGlmICh0b0luZGV4ID09PSAtMSkgdG9JbmRleCA9IDA7XG5cbiAgICB2YXIgZnJvbUluZGV4ID0gYWxsS2V5cy5pbmRleE9mKGZyb21Mb2NhdGlvbi5rZXkpO1xuXG4gICAgaWYgKGZyb21JbmRleCA9PT0gLTEpIGZyb21JbmRleCA9IDA7XG5cbiAgICB2YXIgZGVsdGEgPSB0b0luZGV4IC0gZnJvbUluZGV4O1xuXG4gICAgaWYgKGRlbHRhKSB7XG4gICAgICBmb3JjZU5leHRQb3AgPSB0cnVlO1xuICAgICAgZ28oZGVsdGEpO1xuICAgIH1cbiAgfTtcblxuICB2YXIgaW5pdGlhbExvY2F0aW9uID0gZ2V0RE9NTG9jYXRpb24oZ2V0SGlzdG9yeVN0YXRlKCkpO1xuICB2YXIgYWxsS2V5cyA9IFtpbml0aWFsTG9jYXRpb24ua2V5XTtcblxuICAvLyBQdWJsaWMgaW50ZXJmYWNlXG5cbiAgdmFyIGNyZWF0ZUhyZWYgPSBmdW5jdGlvbiBjcmVhdGVIcmVmKGxvY2F0aW9uKSB7XG4gICAgcmV0dXJuIGJhc2VuYW1lICsgKDAsIF9QYXRoVXRpbHMuY3JlYXRlUGF0aCkobG9jYXRpb24pO1xuICB9O1xuXG4gIHZhciBwdXNoID0gZnVuY3Rpb24gcHVzaChwYXRoLCBzdGF0ZSkge1xuICAgICgwLCBfd2FybmluZzIuZGVmYXVsdCkoISgodHlwZW9mIHBhdGggPT09ICd1bmRlZmluZWQnID8gJ3VuZGVmaW5lZCcgOiBfdHlwZW9mKHBhdGgpKSA9PT0gJ29iamVjdCcgJiYgcGF0aC5zdGF0ZSAhPT0gdW5kZWZpbmVkICYmIHN0YXRlICE9PSB1bmRlZmluZWQpLCAnWW91IHNob3VsZCBhdm9pZCBwcm92aWRpbmcgYSAybmQgc3RhdGUgYXJndW1lbnQgdG8gcHVzaCB3aGVuIHRoZSAxc3QgJyArICdhcmd1bWVudCBpcyBhIGxvY2F0aW9uLWxpa2Ugb2JqZWN0IHRoYXQgYWxyZWFkeSBoYXMgc3RhdGU7IGl0IGlzIGlnbm9yZWQnKTtcblxuICAgIHZhciBhY3Rpb24gPSAnUFVTSCc7XG4gICAgdmFyIGxvY2F0aW9uID0gKDAsIF9Mb2NhdGlvblV0aWxzLmNyZWF0ZUxvY2F0aW9uKShwYXRoLCBzdGF0ZSwgY3JlYXRlS2V5KCksIGhpc3RvcnkubG9jYXRpb24pO1xuXG4gICAgdHJhbnNpdGlvbk1hbmFnZXIuY29uZmlybVRyYW5zaXRpb25Ubyhsb2NhdGlvbiwgYWN0aW9uLCBnZXRVc2VyQ29uZmlybWF0aW9uLCBmdW5jdGlvbiAob2spIHtcbiAgICAgIGlmICghb2spIHJldHVybjtcblxuICAgICAgdmFyIGhyZWYgPSBjcmVhdGVIcmVmKGxvY2F0aW9uKTtcbiAgICAgIHZhciBrZXkgPSBsb2NhdGlvbi5rZXksXG4gICAgICAgICAgc3RhdGUgPSBsb2NhdGlvbi5zdGF0ZTtcblxuXG4gICAgICBpZiAoY2FuVXNlSGlzdG9yeSkge1xuICAgICAgICBnbG9iYWxIaXN0b3J5LnB1c2hTdGF0ZSh7IGtleToga2V5LCBzdGF0ZTogc3RhdGUgfSwgbnVsbCwgaHJlZik7XG5cbiAgICAgICAgaWYgKGZvcmNlUmVmcmVzaCkge1xuICAgICAgICAgIHdpbmRvdy5sb2NhdGlvbi5ocmVmID0gaHJlZjtcbiAgICAgICAgfSBlbHNlIHtcbiAgICAgICAgICB2YXIgcHJldkluZGV4ID0gYWxsS2V5cy5pbmRleE9mKGhpc3RvcnkubG9jYXRpb24ua2V5KTtcbiAgICAgICAgICB2YXIgbmV4dEtleXMgPSBhbGxLZXlzLnNsaWNlKDAsIHByZXZJbmRleCA9PT0gLTEgPyAwIDogcHJldkluZGV4ICsgMSk7XG5cbiAgICAgICAgICBuZXh0S2V5cy5wdXNoKGxvY2F0aW9uLmtleSk7XG4gICAgICAgICAgYWxsS2V5cyA9IG5leHRLZXlzO1xuXG4gICAgICAgICAgc2V0U3RhdGUoeyBhY3Rpb246IGFjdGlvbiwgbG9jYXRpb246IGxvY2F0aW9uIH0pO1xuICAgICAgICB9XG4gICAgICB9IGVsc2Uge1xuICAgICAgICAoMCwgX3dhcm5pbmcyLmRlZmF1bHQpKHN0YXRlID09PSB1bmRlZmluZWQsICdCcm93c2VyIGhpc3RvcnkgY2Fubm90IHB1c2ggc3RhdGUgaW4gYnJvd3NlcnMgdGhhdCBkbyBub3Qgc3VwcG9ydCBIVE1MNSBoaXN0b3J5Jyk7XG5cbiAgICAgICAgd2luZG93LmxvY2F0aW9uLmhyZWYgPSBocmVmO1xuICAgICAgfVxuICAgIH0pO1xuICB9O1xuXG4gIHZhciByZXBsYWNlID0gZnVuY3Rpb24gcmVwbGFjZShwYXRoLCBzdGF0ZSkge1xuICAgICgwLCBfd2FybmluZzIuZGVmYXVsdCkoISgodHlwZW9mIHBhdGggPT09ICd1bmRlZmluZWQnID8gJ3VuZGVmaW5lZCcgOiBfdHlwZW9mKHBhdGgpKSA9PT0gJ29iamVjdCcgJiYgcGF0aC5zdGF0ZSAhPT0gdW5kZWZpbmVkICYmIHN0YXRlICE9PSB1bmRlZmluZWQpLCAnWW91IHNob3VsZCBhdm9pZCBwcm92aWRpbmcgYSAybmQgc3RhdGUgYXJndW1lbnQgdG8gcmVwbGFjZSB3aGVuIHRoZSAxc3QgJyArICdhcmd1bWVudCBpcyBhIGxvY2F0aW9uLWxpa2Ugb2JqZWN0IHRoYXQgYWxyZWFkeSBoYXMgc3RhdGU7IGl0IGlzIGlnbm9yZWQnKTtcblxuICAgIHZhciBhY3Rpb24gPSAnUkVQTEFDRSc7XG4gICAgdmFyIGxvY2F0aW9uID0gKDAsIF9Mb2NhdGlvblV0aWxzLmNyZWF0ZUxvY2F0aW9uKShwYXRoLCBzdGF0ZSwgY3JlYXRlS2V5KCksIGhpc3RvcnkubG9jYXRpb24pO1xuXG4gICAgdHJhbnNpdGlvbk1hbmFnZXIuY29uZmlybVRyYW5zaXRpb25Ubyhsb2NhdGlvbiwgYWN0aW9uLCBnZXRVc2VyQ29uZmlybWF0aW9uLCBmdW5jdGlvbiAob2spIHtcbiAgICAgIGlmICghb2spIHJldHVybjtcblxuICAgICAgdmFyIGhyZWYgPSBjcmVhdGVIcmVmKGxvY2F0aW9uKTtcbiAgICAgIHZhciBrZXkgPSBsb2NhdGlvbi5rZXksXG4gICAgICAgICAgc3RhdGUgPSBsb2NhdGlvbi5zdGF0ZTtcblxuXG4gICAgICBpZiAoY2FuVXNlSGlzdG9yeSkge1xuICAgICAgICBnbG9iYWxIaXN0b3J5LnJlcGxhY2VTdGF0ZSh7IGtleToga2V5LCBzdGF0ZTogc3RhdGUgfSwgbnVsbCwgaHJlZik7XG5cbiAgICAgICAgaWYgKGZvcmNlUmVmcmVzaCkge1xuICAgICAgICAgIHdpbmRvdy5sb2NhdGlvbi5yZXBsYWNlKGhyZWYpO1xuICAgICAgICB9IGVsc2Uge1xuICAgICAgICAgIHZhciBwcmV2SW5kZXggPSBhbGxLZXlzLmluZGV4T2YoaGlzdG9yeS5sb2NhdGlvbi5rZXkpO1xuXG4gICAgICAgICAgaWYgKHByZXZJbmRleCAhPT0gLTEpIGFsbEtleXNbcHJldkluZGV4XSA9IGxvY2F0aW9uLmtleTtcblxuICAgICAgICAgIHNldFN0YXRlKHsgYWN0aW9uOiBhY3Rpb24sIGxvY2F0aW9uOiBsb2NhdGlvbiB9KTtcbiAgICAgICAgfVxuICAgICAgfSBlbHNlIHtcbiAgICAgICAgKDAsIF93YXJuaW5nMi5kZWZhdWx0KShzdGF0ZSA9PT0gdW5kZWZpbmVkLCAnQnJvd3NlciBoaXN0b3J5IGNhbm5vdCByZXBsYWNlIHN0YXRlIGluIGJyb3dzZXJzIHRoYXQgZG8gbm90IHN1cHBvcnQgSFRNTDUgaGlzdG9yeScpO1xuXG4gICAgICAgIHdpbmRvdy5sb2NhdGlvbi5yZXBsYWNlKGhyZWYpO1xuICAgICAgfVxuICAgIH0pO1xuICB9O1xuXG4gIHZhciBnbyA9IGZ1bmN0aW9uIGdvKG4pIHtcbiAgICBnbG9iYWxIaXN0b3J5LmdvKG4pO1xuICB9O1xuXG4gIHZhciBnb0JhY2sgPSBmdW5jdGlvbiBnb0JhY2soKSB7XG4gICAgcmV0dXJuIGdvKC0xKTtcbiAgfTtcblxuICB2YXIgZ29Gb3J3YXJkID0gZnVuY3Rpb24gZ29Gb3J3YXJkKCkge1xuICAgIHJldHVybiBnbygxKTtcbiAgfTtcblxuICB2YXIgbGlzdGVuZXJDb3VudCA9IDA7XG5cbiAgdmFyIGNoZWNrRE9NTGlzdGVuZXJzID0gZnVuY3Rpb24gY2hlY2tET01MaXN0ZW5lcnMoZGVsdGEpIHtcbiAgICBsaXN0ZW5lckNvdW50ICs9IGRlbHRhO1xuXG4gICAgaWYgKGxpc3RlbmVyQ291bnQgPT09IDEpIHtcbiAgICAgICgwLCBfRE9NVXRpbHMuYWRkRXZlbnRMaXN0ZW5lcikod2luZG93LCBQb3BTdGF0ZUV2ZW50LCBoYW5kbGVQb3BTdGF0ZSk7XG5cbiAgICAgIGlmIChuZWVkc0hhc2hDaGFuZ2VMaXN0ZW5lcikgKDAsIF9ET01VdGlscy5hZGRFdmVudExpc3RlbmVyKSh3aW5kb3csIEhhc2hDaGFuZ2VFdmVudCwgaGFuZGxlSGFzaENoYW5nZSk7XG4gICAgfSBlbHNlIGlmIChsaXN0ZW5lckNvdW50ID09PSAwKSB7XG4gICAgICAoMCwgX0RPTVV0aWxzLnJlbW92ZUV2ZW50TGlzdGVuZXIpKHdpbmRvdywgUG9wU3RhdGVFdmVudCwgaGFuZGxlUG9wU3RhdGUpO1xuXG4gICAgICBpZiAobmVlZHNIYXNoQ2hhbmdlTGlzdGVuZXIpICgwLCBfRE9NVXRpbHMucmVtb3ZlRXZlbnRMaXN0ZW5lcikod2luZG93LCBIYXNoQ2hhbmdlRXZlbnQsIGhhbmRsZUhhc2hDaGFuZ2UpO1xuICAgIH1cbiAgfTtcblxuICB2YXIgaXNCbG9ja2VkID0gZmFsc2U7XG5cbiAgdmFyIGJsb2NrID0gZnVuY3Rpb24gYmxvY2soKSB7XG4gICAgdmFyIHByb21wdCA9IGFyZ3VtZW50cy5sZW5ndGggPiAwICYmIGFyZ3VtZW50c1swXSAhPT0gdW5kZWZpbmVkID8gYXJndW1lbnRzWzBdIDogZmFsc2U7XG5cbiAgICB2YXIgdW5ibG9jayA9IHRyYW5zaXRpb25NYW5hZ2VyLnNldFByb21wdChwcm9tcHQpO1xuXG4gICAgaWYgKCFpc0Jsb2NrZWQpIHtcbiAgICAgIGNoZWNrRE9NTGlzdGVuZXJzKDEpO1xuICAgICAgaXNCbG9ja2VkID0gdHJ1ZTtcbiAgICB9XG5cbiAgICByZXR1cm4gZnVuY3Rpb24gKCkge1xuICAgICAgaWYgKGlzQmxvY2tlZCkge1xuICAgICAgICBpc0Jsb2NrZWQgPSBmYWxzZTtcbiAgICAgICAgY2hlY2tET01MaXN0ZW5lcnMoLTEpO1xuICAgICAgfVxuXG4gICAgICByZXR1cm4gdW5ibG9jaygpO1xuICAgIH07XG4gIH07XG5cbiAgdmFyIGxpc3RlbiA9IGZ1bmN0aW9uIGxpc3RlbihsaXN0ZW5lcikge1xuICAgIHZhciB1bmxpc3RlbiA9IHRyYW5zaXRpb25NYW5hZ2VyLmFwcGVuZExpc3RlbmVyKGxpc3RlbmVyKTtcbiAgICBjaGVja0RPTUxpc3RlbmVycygxKTtcblxuICAgIHJldHVybiBmdW5jdGlvbiAoKSB7XG4gICAgICBjaGVja0RPTUxpc3RlbmVycygtMSk7XG4gICAgICB1bmxpc3RlbigpO1xuICAgIH07XG4gIH07XG5cbiAgdmFyIGhpc3RvcnkgPSB7XG4gICAgbGVuZ3RoOiBnbG9iYWxIaXN0b3J5Lmxlbmd0aCxcbiAgICBhY3Rpb246ICdQT1AnLFxuICAgIGxvY2F0aW9uOiBpbml0aWFsTG9jYXRpb24sXG4gICAgY3JlYXRlSHJlZjogY3JlYXRlSHJlZixcbiAgICBwdXNoOiBwdXNoLFxuICAgIHJlcGxhY2U6IHJlcGxhY2UsXG4gICAgZ286IGdvLFxuICAgIGdvQmFjazogZ29CYWNrLFxuICAgIGdvRm9yd2FyZDogZ29Gb3J3YXJkLFxuICAgIGJsb2NrOiBibG9jayxcbiAgICBsaXN0ZW46IGxpc3RlblxuICB9O1xuXG4gIHJldHVybiBoaXN0b3J5O1xufTtcblxuZXhwb3J0cy5kZWZhdWx0ID0gY3JlYXRlQnJvd3Nlckhpc3Rvcnk7IiwiJ3VzZSBzdHJpY3QnO1xuXG5leHBvcnRzLl9fZXNNb2R1bGUgPSB0cnVlO1xuXG52YXIgX3dhcm5pbmcgPSByZXF1aXJlKCd3YXJuaW5nJyk7XG5cbnZhciBfd2FybmluZzIgPSBfaW50ZXJvcFJlcXVpcmVEZWZhdWx0KF93YXJuaW5nKTtcblxuZnVuY3Rpb24gX2ludGVyb3BSZXF1aXJlRGVmYXVsdChvYmopIHsgcmV0dXJuIG9iaiAmJiBvYmouX19lc01vZHVsZSA/IG9iaiA6IHsgZGVmYXVsdDogb2JqIH07IH1cblxudmFyIGNyZWF0ZVRyYW5zaXRpb25NYW5hZ2VyID0gZnVuY3Rpb24gY3JlYXRlVHJhbnNpdGlvbk1hbmFnZXIoKSB7XG4gIHZhciBwcm9tcHQgPSBudWxsO1xuXG4gIHZhciBzZXRQcm9tcHQgPSBmdW5jdGlvbiBzZXRQcm9tcHQobmV4dFByb21wdCkge1xuICAgICgwLCBfd2FybmluZzIuZGVmYXVsdCkocHJvbXB0ID09IG51bGwsICdBIGhpc3Rvcnkgc3VwcG9ydHMgb25seSBvbmUgcHJvbXB0IGF0IGEgdGltZScpO1xuXG4gICAgcHJvbXB0ID0gbmV4dFByb21wdDtcblxuICAgIHJldHVybiBmdW5jdGlvbiAoKSB7XG4gICAgICBpZiAocHJvbXB0ID09PSBuZXh0UHJvbXB0KSBwcm9tcHQgPSBudWxsO1xuICAgIH07XG4gIH07XG5cbiAgdmFyIGNvbmZpcm1UcmFuc2l0aW9uVG8gPSBmdW5jdGlvbiBjb25maXJtVHJhbnNpdGlvblRvKGxvY2F0aW9uLCBhY3Rpb24sIGdldFVzZXJDb25maXJtYXRpb24sIGNhbGxiYWNrKSB7XG4gICAgLy8gVE9ETzogSWYgYW5vdGhlciB0cmFuc2l0aW9uIHN0YXJ0cyB3aGlsZSB3ZSdyZSBzdGlsbCBjb25maXJtaW5nXG4gICAgLy8gdGhlIHByZXZpb3VzIG9uZSwgd2UgbWF5IGVuZCB1cCBpbiBhIHdlaXJkIHN0YXRlLiBGaWd1cmUgb3V0IHRoZVxuICAgIC8vIGJlc3Qgd2F5IHRvIGhhbmRsZSB0aGlzLlxuICAgIGlmIChwcm9tcHQgIT0gbnVsbCkge1xuICAgICAgdmFyIHJlc3VsdCA9IHR5cGVvZiBwcm9tcHQgPT09ICdmdW5jdGlvbicgPyBwcm9tcHQobG9jYXRpb24sIGFjdGlvbikgOiBwcm9tcHQ7XG5cbiAgICAgIGlmICh0eXBlb2YgcmVzdWx0ID09PSAnc3RyaW5nJykge1xuICAgICAgICBpZiAodHlwZW9mIGdldFVzZXJDb25maXJtYXRpb24gPT09ICdmdW5jdGlvbicpIHtcbiAgICAgICAgICBnZXRVc2VyQ29uZmlybWF0aW9uKHJlc3VsdCwgY2FsbGJhY2spO1xuICAgICAgICB9IGVsc2Uge1xuICAgICAgICAgICgwLCBfd2FybmluZzIuZGVmYXVsdCkoZmFsc2UsICdBIGhpc3RvcnkgbmVlZHMgYSBnZXRVc2VyQ29uZmlybWF0aW9uIGZ1bmN0aW9uIGluIG9yZGVyIHRvIHVzZSBhIHByb21wdCBtZXNzYWdlJyk7XG5cbiAgICAgICAgICBjYWxsYmFjayh0cnVlKTtcbiAgICAgICAgfVxuICAgICAgfSBlbHNlIHtcbiAgICAgICAgLy8gUmV0dXJuIGZhbHNlIGZyb20gYSB0cmFuc2l0aW9uIGhvb2sgdG8gY2FuY2VsIHRoZSB0cmFuc2l0aW9uLlxuICAgICAgICBjYWxsYmFjayhyZXN1bHQgIT09IGZhbHNlKTtcbiAgICAgIH1cbiAgICB9IGVsc2Uge1xuICAgICAgY2FsbGJhY2sodHJ1ZSk7XG4gICAgfVxuICB9O1xuXG4gIHZhciBsaXN0ZW5lcnMgPSBbXTtcblxuICB2YXIgYXBwZW5kTGlzdGVuZXIgPSBmdW5jdGlvbiBhcHBlbmRMaXN0ZW5lcihmbikge1xuICAgIHZhciBpc0FjdGl2ZSA9IHRydWU7XG5cbiAgICB2YXIgbGlzdGVuZXIgPSBmdW5jdGlvbiBsaXN0ZW5lcigpIHtcbiAgICAgIGlmIChpc0FjdGl2ZSkgZm4uYXBwbHkodW5kZWZpbmVkLCBhcmd1bWVudHMpO1xuICAgIH07XG5cbiAgICBsaXN0ZW5lcnMucHVzaChsaXN0ZW5lcik7XG5cbiAgICByZXR1cm4gZnVuY3Rpb24gKCkge1xuICAgICAgaXNBY3RpdmUgPSBmYWxzZTtcbiAgICAgIGxpc3RlbmVycyA9IGxpc3RlbmVycy5maWx0ZXIoZnVuY3Rpb24gKGl0ZW0pIHtcbiAgICAgICAgcmV0dXJuIGl0ZW0gIT09IGxpc3RlbmVyO1xuICAgICAgfSk7XG4gICAgfTtcbiAgfTtcblxuICB2YXIgbm90aWZ5TGlzdGVuZXJzID0gZnVuY3Rpb24gbm90aWZ5TGlzdGVuZXJzKCkge1xuICAgIGZvciAodmFyIF9sZW4gPSBhcmd1bWVudHMubGVuZ3RoLCBhcmdzID0gQXJyYXkoX2xlbiksIF9rZXkgPSAwOyBfa2V5IDwgX2xlbjsgX2tleSsrKSB7XG4gICAgICBhcmdzW19rZXldID0gYXJndW1lbnRzW19rZXldO1xuICAgIH1cblxuICAgIGxpc3RlbmVycy5mb3JFYWNoKGZ1bmN0aW9uIChsaXN0ZW5lcikge1xuICAgICAgcmV0dXJuIGxpc3RlbmVyLmFwcGx5KHVuZGVmaW5lZCwgYXJncyk7XG4gICAgfSk7XG4gIH07XG5cbiAgcmV0dXJuIHtcbiAgICBzZXRQcm9tcHQ6IHNldFByb21wdCxcbiAgICBjb25maXJtVHJhbnNpdGlvblRvOiBjb25maXJtVHJhbnNpdGlvblRvLFxuICAgIGFwcGVuZExpc3RlbmVyOiBhcHBlbmRMaXN0ZW5lcixcbiAgICBub3RpZnlMaXN0ZW5lcnM6IG5vdGlmeUxpc3RlbmVyc1xuICB9O1xufTtcblxuZXhwb3J0cy5kZWZhdWx0ID0gY3JlYXRlVHJhbnNpdGlvbk1hbmFnZXI7IiwiLyoqXG4gKiBDb3B5cmlnaHQgKGMpIDIwMTMtcHJlc2VudCwgRmFjZWJvb2ssIEluYy5cbiAqXG4gKiBUaGlzIHNvdXJjZSBjb2RlIGlzIGxpY2Vuc2VkIHVuZGVyIHRoZSBNSVQgbGljZW5zZSBmb3VuZCBpbiB0aGVcbiAqIExJQ0VOU0UgZmlsZSBpbiB0aGUgcm9vdCBkaXJlY3Rvcnkgb2YgdGhpcyBzb3VyY2UgdHJlZS5cbiAqL1xuXG4ndXNlIHN0cmljdCc7XG5cbi8qKlxuICogVXNlIGludmFyaWFudCgpIHRvIGFzc2VydCBzdGF0ZSB3aGljaCB5b3VyIHByb2dyYW0gYXNzdW1lcyB0byBiZSB0cnVlLlxuICpcbiAqIFByb3ZpZGUgc3ByaW50Zi1zdHlsZSBmb3JtYXQgKG9ubHkgJXMgaXMgc3VwcG9ydGVkKSBhbmQgYXJndW1lbnRzXG4gKiB0byBwcm92aWRlIGluZm9ybWF0aW9uIGFib3V0IHdoYXQgYnJva2UgYW5kIHdoYXQgeW91IHdlcmVcbiAqIGV4cGVjdGluZy5cbiAqXG4gKiBUaGUgaW52YXJpYW50IG1lc3NhZ2Ugd2lsbCBiZSBzdHJpcHBlZCBpbiBwcm9kdWN0aW9uLCBidXQgdGhlIGludmFyaWFudFxuICogd2lsbCByZW1haW4gdG8gZW5zdXJlIGxvZ2ljIGRvZXMgbm90IGRpZmZlciBpbiBwcm9kdWN0aW9uLlxuICovXG5cbnZhciBpbnZhcmlhbnQgPSBmdW5jdGlvbihjb25kaXRpb24sIGZvcm1hdCwgYSwgYiwgYywgZCwgZSwgZikge1xuICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgIGlmIChmb3JtYXQgPT09IHVuZGVmaW5lZCkge1xuICAgICAgdGhyb3cgbmV3IEVycm9yKCdpbnZhcmlhbnQgcmVxdWlyZXMgYW4gZXJyb3IgbWVzc2FnZSBhcmd1bWVudCcpO1xuICAgIH1cbiAgfVxuXG4gIGlmICghY29uZGl0aW9uKSB7XG4gICAgdmFyIGVycm9yO1xuICAgIGlmIChmb3JtYXQgPT09IHVuZGVmaW5lZCkge1xuICAgICAgZXJyb3IgPSBuZXcgRXJyb3IoXG4gICAgICAgICdNaW5pZmllZCBleGNlcHRpb24gb2NjdXJyZWQ7IHVzZSB0aGUgbm9uLW1pbmlmaWVkIGRldiBlbnZpcm9ubWVudCAnICtcbiAgICAgICAgJ2ZvciB0aGUgZnVsbCBlcnJvciBtZXNzYWdlIGFuZCBhZGRpdGlvbmFsIGhlbHBmdWwgd2FybmluZ3MuJ1xuICAgICAgKTtcbiAgICB9IGVsc2Uge1xuICAgICAgdmFyIGFyZ3MgPSBbYSwgYiwgYywgZCwgZSwgZl07XG4gICAgICB2YXIgYXJnSW5kZXggPSAwO1xuICAgICAgZXJyb3IgPSBuZXcgRXJyb3IoXG4gICAgICAgIGZvcm1hdC5yZXBsYWNlKC8lcy9nLCBmdW5jdGlvbigpIHsgcmV0dXJuIGFyZ3NbYXJnSW5kZXgrK107IH0pXG4gICAgICApO1xuICAgICAgZXJyb3IubmFtZSA9ICdJbnZhcmlhbnQgVmlvbGF0aW9uJztcbiAgICB9XG5cbiAgICBlcnJvci5mcmFtZXNUb1BvcCA9IDE7IC8vIHdlIGRvbid0IGNhcmUgYWJvdXQgaW52YXJpYW50J3Mgb3duIGZyYW1lXG4gICAgdGhyb3cgZXJyb3I7XG4gIH1cbn07XG5cbm1vZHVsZS5leHBvcnRzID0gaW52YXJpYW50O1xuIiwiLypcbm9iamVjdC1hc3NpZ25cbihjKSBTaW5kcmUgU29yaHVzXG5AbGljZW5zZSBNSVRcbiovXG5cbid1c2Ugc3RyaWN0Jztcbi8qIGVzbGludC1kaXNhYmxlIG5vLXVudXNlZC12YXJzICovXG52YXIgZ2V0T3duUHJvcGVydHlTeW1ib2xzID0gT2JqZWN0LmdldE93blByb3BlcnR5U3ltYm9scztcbnZhciBoYXNPd25Qcm9wZXJ0eSA9IE9iamVjdC5wcm90b3R5cGUuaGFzT3duUHJvcGVydHk7XG52YXIgcHJvcElzRW51bWVyYWJsZSA9IE9iamVjdC5wcm90b3R5cGUucHJvcGVydHlJc0VudW1lcmFibGU7XG5cbmZ1bmN0aW9uIHRvT2JqZWN0KHZhbCkge1xuXHRpZiAodmFsID09PSBudWxsIHx8IHZhbCA9PT0gdW5kZWZpbmVkKSB7XG5cdFx0dGhyb3cgbmV3IFR5cGVFcnJvcignT2JqZWN0LmFzc2lnbiBjYW5ub3QgYmUgY2FsbGVkIHdpdGggbnVsbCBvciB1bmRlZmluZWQnKTtcblx0fVxuXG5cdHJldHVybiBPYmplY3QodmFsKTtcbn1cblxuZnVuY3Rpb24gc2hvdWxkVXNlTmF0aXZlKCkge1xuXHR0cnkge1xuXHRcdGlmICghT2JqZWN0LmFzc2lnbikge1xuXHRcdFx0cmV0dXJuIGZhbHNlO1xuXHRcdH1cblxuXHRcdC8vIERldGVjdCBidWdneSBwcm9wZXJ0eSBlbnVtZXJhdGlvbiBvcmRlciBpbiBvbGRlciBWOCB2ZXJzaW9ucy5cblxuXHRcdC8vIGh0dHBzOi8vYnVncy5jaHJvbWl1bS5vcmcvcC92OC9pc3N1ZXMvZGV0YWlsP2lkPTQxMThcblx0XHR2YXIgdGVzdDEgPSBuZXcgU3RyaW5nKCdhYmMnKTsgIC8vIGVzbGludC1kaXNhYmxlLWxpbmUgbm8tbmV3LXdyYXBwZXJzXG5cdFx0dGVzdDFbNV0gPSAnZGUnO1xuXHRcdGlmIChPYmplY3QuZ2V0T3duUHJvcGVydHlOYW1lcyh0ZXN0MSlbMF0gPT09ICc1Jykge1xuXHRcdFx0cmV0dXJuIGZhbHNlO1xuXHRcdH1cblxuXHRcdC8vIGh0dHBzOi8vYnVncy5jaHJvbWl1bS5vcmcvcC92OC9pc3N1ZXMvZGV0YWlsP2lkPTMwNTZcblx0XHR2YXIgdGVzdDIgPSB7fTtcblx0XHRmb3IgKHZhciBpID0gMDsgaSA8IDEwOyBpKyspIHtcblx0XHRcdHRlc3QyWydfJyArIFN0cmluZy5mcm9tQ2hhckNvZGUoaSldID0gaTtcblx0XHR9XG5cdFx0dmFyIG9yZGVyMiA9IE9iamVjdC5nZXRPd25Qcm9wZXJ0eU5hbWVzKHRlc3QyKS5tYXAoZnVuY3Rpb24gKG4pIHtcblx0XHRcdHJldHVybiB0ZXN0MltuXTtcblx0XHR9KTtcblx0XHRpZiAob3JkZXIyLmpvaW4oJycpICE9PSAnMDEyMzQ1Njc4OScpIHtcblx0XHRcdHJldHVybiBmYWxzZTtcblx0XHR9XG5cblx0XHQvLyBodHRwczovL2J1Z3MuY2hyb21pdW0ub3JnL3AvdjgvaXNzdWVzL2RldGFpbD9pZD0zMDU2XG5cdFx0dmFyIHRlc3QzID0ge307XG5cdFx0J2FiY2RlZmdoaWprbG1ub3BxcnN0Jy5zcGxpdCgnJykuZm9yRWFjaChmdW5jdGlvbiAobGV0dGVyKSB7XG5cdFx0XHR0ZXN0M1tsZXR0ZXJdID0gbGV0dGVyO1xuXHRcdH0pO1xuXHRcdGlmIChPYmplY3Qua2V5cyhPYmplY3QuYXNzaWduKHt9LCB0ZXN0MykpLmpvaW4oJycpICE9PVxuXHRcdFx0XHQnYWJjZGVmZ2hpamtsbW5vcHFyc3QnKSB7XG5cdFx0XHRyZXR1cm4gZmFsc2U7XG5cdFx0fVxuXG5cdFx0cmV0dXJuIHRydWU7XG5cdH0gY2F0Y2ggKGVycikge1xuXHRcdC8vIFdlIGRvbid0IGV4cGVjdCBhbnkgb2YgdGhlIGFib3ZlIHRvIHRocm93LCBidXQgYmV0dGVyIHRvIGJlIHNhZmUuXG5cdFx0cmV0dXJuIGZhbHNlO1xuXHR9XG59XG5cbm1vZHVsZS5leHBvcnRzID0gc2hvdWxkVXNlTmF0aXZlKCkgPyBPYmplY3QuYXNzaWduIDogZnVuY3Rpb24gKHRhcmdldCwgc291cmNlKSB7XG5cdHZhciBmcm9tO1xuXHR2YXIgdG8gPSB0b09iamVjdCh0YXJnZXQpO1xuXHR2YXIgc3ltYm9scztcblxuXHRmb3IgKHZhciBzID0gMTsgcyA8IGFyZ3VtZW50cy5sZW5ndGg7IHMrKykge1xuXHRcdGZyb20gPSBPYmplY3QoYXJndW1lbnRzW3NdKTtcblxuXHRcdGZvciAodmFyIGtleSBpbiBmcm9tKSB7XG5cdFx0XHRpZiAoaGFzT3duUHJvcGVydHkuY2FsbChmcm9tLCBrZXkpKSB7XG5cdFx0XHRcdHRvW2tleV0gPSBmcm9tW2tleV07XG5cdFx0XHR9XG5cdFx0fVxuXG5cdFx0aWYgKGdldE93blByb3BlcnR5U3ltYm9scykge1xuXHRcdFx0c3ltYm9scyA9IGdldE93blByb3BlcnR5U3ltYm9scyhmcm9tKTtcblx0XHRcdGZvciAodmFyIGkgPSAwOyBpIDwgc3ltYm9scy5sZW5ndGg7IGkrKykge1xuXHRcdFx0XHRpZiAocHJvcElzRW51bWVyYWJsZS5jYWxsKGZyb20sIHN5bWJvbHNbaV0pKSB7XG5cdFx0XHRcdFx0dG9bc3ltYm9sc1tpXV0gPSBmcm9tW3N5bWJvbHNbaV1dO1xuXHRcdFx0XHR9XG5cdFx0XHR9XG5cdFx0fVxuXHR9XG5cblx0cmV0dXJuIHRvO1xufTtcbiIsIi8qKlxuICogQ29weXJpZ2h0IChjKSAyMDEzLXByZXNlbnQsIEZhY2Vib29rLCBJbmMuXG4gKlxuICogVGhpcyBzb3VyY2UgY29kZSBpcyBsaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UgZm91bmQgaW4gdGhlXG4gKiBMSUNFTlNFIGZpbGUgaW4gdGhlIHJvb3QgZGlyZWN0b3J5IG9mIHRoaXMgc291cmNlIHRyZWUuXG4gKi9cblxuJ3VzZSBzdHJpY3QnO1xuXG5pZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICB2YXIgaW52YXJpYW50ID0gcmVxdWlyZSgnZmJqcy9saWIvaW52YXJpYW50Jyk7XG4gIHZhciB3YXJuaW5nID0gcmVxdWlyZSgnZmJqcy9saWIvd2FybmluZycpO1xuICB2YXIgUmVhY3RQcm9wVHlwZXNTZWNyZXQgPSByZXF1aXJlKCcuL2xpYi9SZWFjdFByb3BUeXBlc1NlY3JldCcpO1xuICB2YXIgbG9nZ2VkVHlwZUZhaWx1cmVzID0ge307XG59XG5cbi8qKlxuICogQXNzZXJ0IHRoYXQgdGhlIHZhbHVlcyBtYXRjaCB3aXRoIHRoZSB0eXBlIHNwZWNzLlxuICogRXJyb3IgbWVzc2FnZXMgYXJlIG1lbW9yaXplZCBhbmQgd2lsbCBvbmx5IGJlIHNob3duIG9uY2UuXG4gKlxuICogQHBhcmFtIHtvYmplY3R9IHR5cGVTcGVjcyBNYXAgb2YgbmFtZSB0byBhIFJlYWN0UHJvcFR5cGVcbiAqIEBwYXJhbSB7b2JqZWN0fSB2YWx1ZXMgUnVudGltZSB2YWx1ZXMgdGhhdCBuZWVkIHRvIGJlIHR5cGUtY2hlY2tlZFxuICogQHBhcmFtIHtzdHJpbmd9IGxvY2F0aW9uIGUuZy4gXCJwcm9wXCIsIFwiY29udGV4dFwiLCBcImNoaWxkIGNvbnRleHRcIlxuICogQHBhcmFtIHtzdHJpbmd9IGNvbXBvbmVudE5hbWUgTmFtZSBvZiB0aGUgY29tcG9uZW50IGZvciBlcnJvciBtZXNzYWdlcy5cbiAqIEBwYXJhbSB7P0Z1bmN0aW9ufSBnZXRTdGFjayBSZXR1cm5zIHRoZSBjb21wb25lbnQgc3RhY2suXG4gKiBAcHJpdmF0ZVxuICovXG5mdW5jdGlvbiBjaGVja1Byb3BUeXBlcyh0eXBlU3BlY3MsIHZhbHVlcywgbG9jYXRpb24sIGNvbXBvbmVudE5hbWUsIGdldFN0YWNrKSB7XG4gIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgZm9yICh2YXIgdHlwZVNwZWNOYW1lIGluIHR5cGVTcGVjcykge1xuICAgICAgaWYgKHR5cGVTcGVjcy5oYXNPd25Qcm9wZXJ0eSh0eXBlU3BlY05hbWUpKSB7XG4gICAgICAgIHZhciBlcnJvcjtcbiAgICAgICAgLy8gUHJvcCB0eXBlIHZhbGlkYXRpb24gbWF5IHRocm93LiBJbiBjYXNlIHRoZXkgZG8sIHdlIGRvbid0IHdhbnQgdG9cbiAgICAgICAgLy8gZmFpbCB0aGUgcmVuZGVyIHBoYXNlIHdoZXJlIGl0IGRpZG4ndCBmYWlsIGJlZm9yZS4gU28gd2UgbG9nIGl0LlxuICAgICAgICAvLyBBZnRlciB0aGVzZSBoYXZlIGJlZW4gY2xlYW5lZCB1cCwgd2UnbGwgbGV0IHRoZW0gdGhyb3cuXG4gICAgICAgIHRyeSB7XG4gICAgICAgICAgLy8gVGhpcyBpcyBpbnRlbnRpb25hbGx5IGFuIGludmFyaWFudCB0aGF0IGdldHMgY2F1Z2h0LiBJdCdzIHRoZSBzYW1lXG4gICAgICAgICAgLy8gYmVoYXZpb3IgYXMgd2l0aG91dCB0aGlzIHN0YXRlbWVudCBleGNlcHQgd2l0aCBhIGJldHRlciBtZXNzYWdlLlxuICAgICAgICAgIGludmFyaWFudCh0eXBlb2YgdHlwZVNwZWNzW3R5cGVTcGVjTmFtZV0gPT09ICdmdW5jdGlvbicsICclczogJXMgdHlwZSBgJXNgIGlzIGludmFsaWQ7IGl0IG11c3QgYmUgYSBmdW5jdGlvbiwgdXN1YWxseSBmcm9tICcgKyAndGhlIGBwcm9wLXR5cGVzYCBwYWNrYWdlLCBidXQgcmVjZWl2ZWQgYCVzYC4nLCBjb21wb25lbnROYW1lIHx8ICdSZWFjdCBjbGFzcycsIGxvY2F0aW9uLCB0eXBlU3BlY05hbWUsIHR5cGVvZiB0eXBlU3BlY3NbdHlwZVNwZWNOYW1lXSk7XG4gICAgICAgICAgZXJyb3IgPSB0eXBlU3BlY3NbdHlwZVNwZWNOYW1lXSh2YWx1ZXMsIHR5cGVTcGVjTmFtZSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIG51bGwsIFJlYWN0UHJvcFR5cGVzU2VjcmV0KTtcbiAgICAgICAgfSBjYXRjaCAoZXgpIHtcbiAgICAgICAgICBlcnJvciA9IGV4O1xuICAgICAgICB9XG4gICAgICAgIHdhcm5pbmcoIWVycm9yIHx8IGVycm9yIGluc3RhbmNlb2YgRXJyb3IsICclczogdHlwZSBzcGVjaWZpY2F0aW9uIG9mICVzIGAlc2AgaXMgaW52YWxpZDsgdGhlIHR5cGUgY2hlY2tlciAnICsgJ2Z1bmN0aW9uIG11c3QgcmV0dXJuIGBudWxsYCBvciBhbiBgRXJyb3JgIGJ1dCByZXR1cm5lZCBhICVzLiAnICsgJ1lvdSBtYXkgaGF2ZSBmb3Jnb3R0ZW4gdG8gcGFzcyBhbiBhcmd1bWVudCB0byB0aGUgdHlwZSBjaGVja2VyICcgKyAnY3JlYXRvciAoYXJyYXlPZiwgaW5zdGFuY2VPZiwgb2JqZWN0T2YsIG9uZU9mLCBvbmVPZlR5cGUsIGFuZCAnICsgJ3NoYXBlIGFsbCByZXF1aXJlIGFuIGFyZ3VtZW50KS4nLCBjb21wb25lbnROYW1lIHx8ICdSZWFjdCBjbGFzcycsIGxvY2F0aW9uLCB0eXBlU3BlY05hbWUsIHR5cGVvZiBlcnJvcik7XG4gICAgICAgIGlmIChlcnJvciBpbnN0YW5jZW9mIEVycm9yICYmICEoZXJyb3IubWVzc2FnZSBpbiBsb2dnZWRUeXBlRmFpbHVyZXMpKSB7XG4gICAgICAgICAgLy8gT25seSBtb25pdG9yIHRoaXMgZmFpbHVyZSBvbmNlIGJlY2F1c2UgdGhlcmUgdGVuZHMgdG8gYmUgYSBsb3Qgb2YgdGhlXG4gICAgICAgICAgLy8gc2FtZSBlcnJvci5cbiAgICAgICAgICBsb2dnZWRUeXBlRmFpbHVyZXNbZXJyb3IubWVzc2FnZV0gPSB0cnVlO1xuXG4gICAgICAgICAgdmFyIHN0YWNrID0gZ2V0U3RhY2sgPyBnZXRTdGFjaygpIDogJyc7XG5cbiAgICAgICAgICB3YXJuaW5nKGZhbHNlLCAnRmFpbGVkICVzIHR5cGU6ICVzJXMnLCBsb2NhdGlvbiwgZXJyb3IubWVzc2FnZSwgc3RhY2sgIT0gbnVsbCA/IHN0YWNrIDogJycpO1xuICAgICAgICB9XG4gICAgICB9XG4gICAgfVxuICB9XG59XG5cbm1vZHVsZS5leHBvcnRzID0gY2hlY2tQcm9wVHlwZXM7XG4iLCIvKipcbiAqIENvcHlyaWdodCAoYykgMjAxMy1wcmVzZW50LCBGYWNlYm9vaywgSW5jLlxuICpcbiAqIFRoaXMgc291cmNlIGNvZGUgaXMgbGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlIGZvdW5kIGluIHRoZVxuICogTElDRU5TRSBmaWxlIGluIHRoZSByb290IGRpcmVjdG9yeSBvZiB0aGlzIHNvdXJjZSB0cmVlLlxuICovXG5cbid1c2Ugc3RyaWN0JztcblxudmFyIGVtcHR5RnVuY3Rpb24gPSByZXF1aXJlKCdmYmpzL2xpYi9lbXB0eUZ1bmN0aW9uJyk7XG52YXIgaW52YXJpYW50ID0gcmVxdWlyZSgnZmJqcy9saWIvaW52YXJpYW50Jyk7XG52YXIgd2FybmluZyA9IHJlcXVpcmUoJ2ZianMvbGliL3dhcm5pbmcnKTtcbnZhciBhc3NpZ24gPSByZXF1aXJlKCdvYmplY3QtYXNzaWduJyk7XG5cbnZhciBSZWFjdFByb3BUeXBlc1NlY3JldCA9IHJlcXVpcmUoJy4vbGliL1JlYWN0UHJvcFR5cGVzU2VjcmV0Jyk7XG52YXIgY2hlY2tQcm9wVHlwZXMgPSByZXF1aXJlKCcuL2NoZWNrUHJvcFR5cGVzJyk7XG5cbm1vZHVsZS5leHBvcnRzID0gZnVuY3Rpb24oaXNWYWxpZEVsZW1lbnQsIHRocm93T25EaXJlY3RBY2Nlc3MpIHtcbiAgLyogZ2xvYmFsIFN5bWJvbCAqL1xuICB2YXIgSVRFUkFUT1JfU1lNQk9MID0gdHlwZW9mIFN5bWJvbCA9PT0gJ2Z1bmN0aW9uJyAmJiBTeW1ib2wuaXRlcmF0b3I7XG4gIHZhciBGQVVYX0lURVJBVE9SX1NZTUJPTCA9ICdAQGl0ZXJhdG9yJzsgLy8gQmVmb3JlIFN5bWJvbCBzcGVjLlxuXG4gIC8qKlxuICAgKiBSZXR1cm5zIHRoZSBpdGVyYXRvciBtZXRob2QgZnVuY3Rpb24gY29udGFpbmVkIG9uIHRoZSBpdGVyYWJsZSBvYmplY3QuXG4gICAqXG4gICAqIEJlIHN1cmUgdG8gaW52b2tlIHRoZSBmdW5jdGlvbiB3aXRoIHRoZSBpdGVyYWJsZSBhcyBjb250ZXh0OlxuICAgKlxuICAgKiAgICAgdmFyIGl0ZXJhdG9yRm4gPSBnZXRJdGVyYXRvckZuKG15SXRlcmFibGUpO1xuICAgKiAgICAgaWYgKGl0ZXJhdG9yRm4pIHtcbiAgICogICAgICAgdmFyIGl0ZXJhdG9yID0gaXRlcmF0b3JGbi5jYWxsKG15SXRlcmFibGUpO1xuICAgKiAgICAgICAuLi5cbiAgICogICAgIH1cbiAgICpcbiAgICogQHBhcmFtIHs/b2JqZWN0fSBtYXliZUl0ZXJhYmxlXG4gICAqIEByZXR1cm4gez9mdW5jdGlvbn1cbiAgICovXG4gIGZ1bmN0aW9uIGdldEl0ZXJhdG9yRm4obWF5YmVJdGVyYWJsZSkge1xuICAgIHZhciBpdGVyYXRvckZuID0gbWF5YmVJdGVyYWJsZSAmJiAoSVRFUkFUT1JfU1lNQk9MICYmIG1heWJlSXRlcmFibGVbSVRFUkFUT1JfU1lNQk9MXSB8fCBtYXliZUl0ZXJhYmxlW0ZBVVhfSVRFUkFUT1JfU1lNQk9MXSk7XG4gICAgaWYgKHR5cGVvZiBpdGVyYXRvckZuID09PSAnZnVuY3Rpb24nKSB7XG4gICAgICByZXR1cm4gaXRlcmF0b3JGbjtcbiAgICB9XG4gIH1cblxuICAvKipcbiAgICogQ29sbGVjdGlvbiBvZiBtZXRob2RzIHRoYXQgYWxsb3cgZGVjbGFyYXRpb24gYW5kIHZhbGlkYXRpb24gb2YgcHJvcHMgdGhhdCBhcmVcbiAgICogc3VwcGxpZWQgdG8gUmVhY3QgY29tcG9uZW50cy4gRXhhbXBsZSB1c2FnZTpcbiAgICpcbiAgICogICB2YXIgUHJvcHMgPSByZXF1aXJlKCdSZWFjdFByb3BUeXBlcycpO1xuICAgKiAgIHZhciBNeUFydGljbGUgPSBSZWFjdC5jcmVhdGVDbGFzcyh7XG4gICAqICAgICBwcm9wVHlwZXM6IHtcbiAgICogICAgICAgLy8gQW4gb3B0aW9uYWwgc3RyaW5nIHByb3AgbmFtZWQgXCJkZXNjcmlwdGlvblwiLlxuICAgKiAgICAgICBkZXNjcmlwdGlvbjogUHJvcHMuc3RyaW5nLFxuICAgKlxuICAgKiAgICAgICAvLyBBIHJlcXVpcmVkIGVudW0gcHJvcCBuYW1lZCBcImNhdGVnb3J5XCIuXG4gICAqICAgICAgIGNhdGVnb3J5OiBQcm9wcy5vbmVPZihbJ05ld3MnLCdQaG90b3MnXSkuaXNSZXF1aXJlZCxcbiAgICpcbiAgICogICAgICAgLy8gQSBwcm9wIG5hbWVkIFwiZGlhbG9nXCIgdGhhdCByZXF1aXJlcyBhbiBpbnN0YW5jZSBvZiBEaWFsb2cuXG4gICAqICAgICAgIGRpYWxvZzogUHJvcHMuaW5zdGFuY2VPZihEaWFsb2cpLmlzUmVxdWlyZWRcbiAgICogICAgIH0sXG4gICAqICAgICByZW5kZXI6IGZ1bmN0aW9uKCkgeyAuLi4gfVxuICAgKiAgIH0pO1xuICAgKlxuICAgKiBBIG1vcmUgZm9ybWFsIHNwZWNpZmljYXRpb24gb2YgaG93IHRoZXNlIG1ldGhvZHMgYXJlIHVzZWQ6XG4gICAqXG4gICAqICAgdHlwZSA6PSBhcnJheXxib29sfGZ1bmN8b2JqZWN0fG51bWJlcnxzdHJpbmd8b25lT2YoWy4uLl0pfGluc3RhbmNlT2YoLi4uKVxuICAgKiAgIGRlY2wgOj0gUmVhY3RQcm9wVHlwZXMue3R5cGV9KC5pc1JlcXVpcmVkKT9cbiAgICpcbiAgICogRWFjaCBhbmQgZXZlcnkgZGVjbGFyYXRpb24gcHJvZHVjZXMgYSBmdW5jdGlvbiB3aXRoIHRoZSBzYW1lIHNpZ25hdHVyZS4gVGhpc1xuICAgKiBhbGxvd3MgdGhlIGNyZWF0aW9uIG9mIGN1c3RvbSB2YWxpZGF0aW9uIGZ1bmN0aW9ucy4gRm9yIGV4YW1wbGU6XG4gICAqXG4gICAqICB2YXIgTXlMaW5rID0gUmVhY3QuY3JlYXRlQ2xhc3Moe1xuICAgKiAgICBwcm9wVHlwZXM6IHtcbiAgICogICAgICAvLyBBbiBvcHRpb25hbCBzdHJpbmcgb3IgVVJJIHByb3AgbmFtZWQgXCJocmVmXCIuXG4gICAqICAgICAgaHJlZjogZnVuY3Rpb24ocHJvcHMsIHByb3BOYW1lLCBjb21wb25lbnROYW1lKSB7XG4gICAqICAgICAgICB2YXIgcHJvcFZhbHVlID0gcHJvcHNbcHJvcE5hbWVdO1xuICAgKiAgICAgICAgaWYgKHByb3BWYWx1ZSAhPSBudWxsICYmIHR5cGVvZiBwcm9wVmFsdWUgIT09ICdzdHJpbmcnICYmXG4gICAqICAgICAgICAgICAgIShwcm9wVmFsdWUgaW5zdGFuY2VvZiBVUkkpKSB7XG4gICAqICAgICAgICAgIHJldHVybiBuZXcgRXJyb3IoXG4gICAqICAgICAgICAgICAgJ0V4cGVjdGVkIGEgc3RyaW5nIG9yIGFuIFVSSSBmb3IgJyArIHByb3BOYW1lICsgJyBpbiAnICtcbiAgICogICAgICAgICAgICBjb21wb25lbnROYW1lXG4gICAqICAgICAgICAgICk7XG4gICAqICAgICAgICB9XG4gICAqICAgICAgfVxuICAgKiAgICB9LFxuICAgKiAgICByZW5kZXI6IGZ1bmN0aW9uKCkgey4uLn1cbiAgICogIH0pO1xuICAgKlxuICAgKiBAaW50ZXJuYWxcbiAgICovXG5cbiAgdmFyIEFOT05ZTU9VUyA9ICc8PGFub255bW91cz4+JztcblxuICAvLyBJbXBvcnRhbnQhXG4gIC8vIEtlZXAgdGhpcyBsaXN0IGluIHN5bmMgd2l0aCBwcm9kdWN0aW9uIHZlcnNpb24gaW4gYC4vZmFjdG9yeVdpdGhUaHJvd2luZ1NoaW1zLmpzYC5cbiAgdmFyIFJlYWN0UHJvcFR5cGVzID0ge1xuICAgIGFycmF5OiBjcmVhdGVQcmltaXRpdmVUeXBlQ2hlY2tlcignYXJyYXknKSxcbiAgICBib29sOiBjcmVhdGVQcmltaXRpdmVUeXBlQ2hlY2tlcignYm9vbGVhbicpLFxuICAgIGZ1bmM6IGNyZWF0ZVByaW1pdGl2ZVR5cGVDaGVja2VyKCdmdW5jdGlvbicpLFxuICAgIG51bWJlcjogY3JlYXRlUHJpbWl0aXZlVHlwZUNoZWNrZXIoJ251bWJlcicpLFxuICAgIG9iamVjdDogY3JlYXRlUHJpbWl0aXZlVHlwZUNoZWNrZXIoJ29iamVjdCcpLFxuICAgIHN0cmluZzogY3JlYXRlUHJpbWl0aXZlVHlwZUNoZWNrZXIoJ3N0cmluZycpLFxuICAgIHN5bWJvbDogY3JlYXRlUHJpbWl0aXZlVHlwZUNoZWNrZXIoJ3N5bWJvbCcpLFxuXG4gICAgYW55OiBjcmVhdGVBbnlUeXBlQ2hlY2tlcigpLFxuICAgIGFycmF5T2Y6IGNyZWF0ZUFycmF5T2ZUeXBlQ2hlY2tlcixcbiAgICBlbGVtZW50OiBjcmVhdGVFbGVtZW50VHlwZUNoZWNrZXIoKSxcbiAgICBpbnN0YW5jZU9mOiBjcmVhdGVJbnN0YW5jZVR5cGVDaGVja2VyLFxuICAgIG5vZGU6IGNyZWF0ZU5vZGVDaGVja2VyKCksXG4gICAgb2JqZWN0T2Y6IGNyZWF0ZU9iamVjdE9mVHlwZUNoZWNrZXIsXG4gICAgb25lT2Y6IGNyZWF0ZUVudW1UeXBlQ2hlY2tlcixcbiAgICBvbmVPZlR5cGU6IGNyZWF0ZVVuaW9uVHlwZUNoZWNrZXIsXG4gICAgc2hhcGU6IGNyZWF0ZVNoYXBlVHlwZUNoZWNrZXIsXG4gICAgZXhhY3Q6IGNyZWF0ZVN0cmljdFNoYXBlVHlwZUNoZWNrZXIsXG4gIH07XG5cbiAgLyoqXG4gICAqIGlubGluZWQgT2JqZWN0LmlzIHBvbHlmaWxsIHRvIGF2b2lkIHJlcXVpcmluZyBjb25zdW1lcnMgc2hpcCB0aGVpciBvd25cbiAgICogaHR0cHM6Ly9kZXZlbG9wZXIubW96aWxsYS5vcmcvZW4tVVMvZG9jcy9XZWIvSmF2YVNjcmlwdC9SZWZlcmVuY2UvR2xvYmFsX09iamVjdHMvT2JqZWN0L2lzXG4gICAqL1xuICAvKmVzbGludC1kaXNhYmxlIG5vLXNlbGYtY29tcGFyZSovXG4gIGZ1bmN0aW9uIGlzKHgsIHkpIHtcbiAgICAvLyBTYW1lVmFsdWUgYWxnb3JpdGhtXG4gICAgaWYgKHggPT09IHkpIHtcbiAgICAgIC8vIFN0ZXBzIDEtNSwgNy0xMFxuICAgICAgLy8gU3RlcHMgNi5iLTYuZTogKzAgIT0gLTBcbiAgICAgIHJldHVybiB4ICE9PSAwIHx8IDEgLyB4ID09PSAxIC8geTtcbiAgICB9IGVsc2Uge1xuICAgICAgLy8gU3RlcCA2LmE6IE5hTiA9PSBOYU5cbiAgICAgIHJldHVybiB4ICE9PSB4ICYmIHkgIT09IHk7XG4gICAgfVxuICB9XG4gIC8qZXNsaW50LWVuYWJsZSBuby1zZWxmLWNvbXBhcmUqL1xuXG4gIC8qKlxuICAgKiBXZSB1c2UgYW4gRXJyb3ItbGlrZSBvYmplY3QgZm9yIGJhY2t3YXJkIGNvbXBhdGliaWxpdHkgYXMgcGVvcGxlIG1heSBjYWxsXG4gICAqIFByb3BUeXBlcyBkaXJlY3RseSBhbmQgaW5zcGVjdCB0aGVpciBvdXRwdXQuIEhvd2V2ZXIsIHdlIGRvbid0IHVzZSByZWFsXG4gICAqIEVycm9ycyBhbnltb3JlLiBXZSBkb24ndCBpbnNwZWN0IHRoZWlyIHN0YWNrIGFueXdheSwgYW5kIGNyZWF0aW5nIHRoZW1cbiAgICogaXMgcHJvaGliaXRpdmVseSBleHBlbnNpdmUgaWYgdGhleSBhcmUgY3JlYXRlZCB0b28gb2Z0ZW4sIHN1Y2ggYXMgd2hhdFxuICAgKiBoYXBwZW5zIGluIG9uZU9mVHlwZSgpIGZvciBhbnkgdHlwZSBiZWZvcmUgdGhlIG9uZSB0aGF0IG1hdGNoZWQuXG4gICAqL1xuICBmdW5jdGlvbiBQcm9wVHlwZUVycm9yKG1lc3NhZ2UpIHtcbiAgICB0aGlzLm1lc3NhZ2UgPSBtZXNzYWdlO1xuICAgIHRoaXMuc3RhY2sgPSAnJztcbiAgfVxuICAvLyBNYWtlIGBpbnN0YW5jZW9mIEVycm9yYCBzdGlsbCB3b3JrIGZvciByZXR1cm5lZCBlcnJvcnMuXG4gIFByb3BUeXBlRXJyb3IucHJvdG90eXBlID0gRXJyb3IucHJvdG90eXBlO1xuXG4gIGZ1bmN0aW9uIGNyZWF0ZUNoYWluYWJsZVR5cGVDaGVja2VyKHZhbGlkYXRlKSB7XG4gICAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICAgIHZhciBtYW51YWxQcm9wVHlwZUNhbGxDYWNoZSA9IHt9O1xuICAgICAgdmFyIG1hbnVhbFByb3BUeXBlV2FybmluZ0NvdW50ID0gMDtcbiAgICB9XG4gICAgZnVuY3Rpb24gY2hlY2tUeXBlKGlzUmVxdWlyZWQsIHByb3BzLCBwcm9wTmFtZSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSwgc2VjcmV0KSB7XG4gICAgICBjb21wb25lbnROYW1lID0gY29tcG9uZW50TmFtZSB8fCBBTk9OWU1PVVM7XG4gICAgICBwcm9wRnVsbE5hbWUgPSBwcm9wRnVsbE5hbWUgfHwgcHJvcE5hbWU7XG5cbiAgICAgIGlmIChzZWNyZXQgIT09IFJlYWN0UHJvcFR5cGVzU2VjcmV0KSB7XG4gICAgICAgIGlmICh0aHJvd09uRGlyZWN0QWNjZXNzKSB7XG4gICAgICAgICAgLy8gTmV3IGJlaGF2aW9yIG9ubHkgZm9yIHVzZXJzIG9mIGBwcm9wLXR5cGVzYCBwYWNrYWdlXG4gICAgICAgICAgaW52YXJpYW50KFxuICAgICAgICAgICAgZmFsc2UsXG4gICAgICAgICAgICAnQ2FsbGluZyBQcm9wVHlwZXMgdmFsaWRhdG9ycyBkaXJlY3RseSBpcyBub3Qgc3VwcG9ydGVkIGJ5IHRoZSBgcHJvcC10eXBlc2AgcGFja2FnZS4gJyArXG4gICAgICAgICAgICAnVXNlIGBQcm9wVHlwZXMuY2hlY2tQcm9wVHlwZXMoKWAgdG8gY2FsbCB0aGVtLiAnICtcbiAgICAgICAgICAgICdSZWFkIG1vcmUgYXQgaHR0cDovL2ZiLm1lL3VzZS1jaGVjay1wcm9wLXR5cGVzJ1xuICAgICAgICAgICk7XG4gICAgICAgIH0gZWxzZSBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJyAmJiB0eXBlb2YgY29uc29sZSAhPT0gJ3VuZGVmaW5lZCcpIHtcbiAgICAgICAgICAvLyBPbGQgYmVoYXZpb3IgZm9yIHBlb3BsZSB1c2luZyBSZWFjdC5Qcm9wVHlwZXNcbiAgICAgICAgICB2YXIgY2FjaGVLZXkgPSBjb21wb25lbnROYW1lICsgJzonICsgcHJvcE5hbWU7XG4gICAgICAgICAgaWYgKFxuICAgICAgICAgICAgIW1hbnVhbFByb3BUeXBlQ2FsbENhY2hlW2NhY2hlS2V5XSAmJlxuICAgICAgICAgICAgLy8gQXZvaWQgc3BhbW1pbmcgdGhlIGNvbnNvbGUgYmVjYXVzZSB0aGV5IGFyZSBvZnRlbiBub3QgYWN0aW9uYWJsZSBleGNlcHQgZm9yIGxpYiBhdXRob3JzXG4gICAgICAgICAgICBtYW51YWxQcm9wVHlwZVdhcm5pbmdDb3VudCA8IDNcbiAgICAgICAgICApIHtcbiAgICAgICAgICAgIHdhcm5pbmcoXG4gICAgICAgICAgICAgIGZhbHNlLFxuICAgICAgICAgICAgICAnWW91IGFyZSBtYW51YWxseSBjYWxsaW5nIGEgUmVhY3QuUHJvcFR5cGVzIHZhbGlkYXRpb24gJyArXG4gICAgICAgICAgICAgICdmdW5jdGlvbiBmb3IgdGhlIGAlc2AgcHJvcCBvbiBgJXNgLiBUaGlzIGlzIGRlcHJlY2F0ZWQgJyArXG4gICAgICAgICAgICAgICdhbmQgd2lsbCB0aHJvdyBpbiB0aGUgc3RhbmRhbG9uZSBgcHJvcC10eXBlc2AgcGFja2FnZS4gJyArXG4gICAgICAgICAgICAgICdZb3UgbWF5IGJlIHNlZWluZyB0aGlzIHdhcm5pbmcgZHVlIHRvIGEgdGhpcmQtcGFydHkgUHJvcFR5cGVzICcgK1xuICAgICAgICAgICAgICAnbGlicmFyeS4gU2VlIGh0dHBzOi8vZmIubWUvcmVhY3Qtd2FybmluZy1kb250LWNhbGwtcHJvcHR5cGVzICcgKyAnZm9yIGRldGFpbHMuJyxcbiAgICAgICAgICAgICAgcHJvcEZ1bGxOYW1lLFxuICAgICAgICAgICAgICBjb21wb25lbnROYW1lXG4gICAgICAgICAgICApO1xuICAgICAgICAgICAgbWFudWFsUHJvcFR5cGVDYWxsQ2FjaGVbY2FjaGVLZXldID0gdHJ1ZTtcbiAgICAgICAgICAgIG1hbnVhbFByb3BUeXBlV2FybmluZ0NvdW50Kys7XG4gICAgICAgICAgfVxuICAgICAgICB9XG4gICAgICB9XG4gICAgICBpZiAocHJvcHNbcHJvcE5hbWVdID09IG51bGwpIHtcbiAgICAgICAgaWYgKGlzUmVxdWlyZWQpIHtcbiAgICAgICAgICBpZiAocHJvcHNbcHJvcE5hbWVdID09PSBudWxsKSB7XG4gICAgICAgICAgICByZXR1cm4gbmV3IFByb3BUeXBlRXJyb3IoJ1RoZSAnICsgbG9jYXRpb24gKyAnIGAnICsgcHJvcEZ1bGxOYW1lICsgJ2AgaXMgbWFya2VkIGFzIHJlcXVpcmVkICcgKyAoJ2luIGAnICsgY29tcG9uZW50TmFtZSArICdgLCBidXQgaXRzIHZhbHVlIGlzIGBudWxsYC4nKSk7XG4gICAgICAgICAgfVxuICAgICAgICAgIHJldHVybiBuZXcgUHJvcFR5cGVFcnJvcignVGhlICcgKyBsb2NhdGlvbiArICcgYCcgKyBwcm9wRnVsbE5hbWUgKyAnYCBpcyBtYXJrZWQgYXMgcmVxdWlyZWQgaW4gJyArICgnYCcgKyBjb21wb25lbnROYW1lICsgJ2AsIGJ1dCBpdHMgdmFsdWUgaXMgYHVuZGVmaW5lZGAuJykpO1xuICAgICAgICB9XG4gICAgICAgIHJldHVybiBudWxsO1xuICAgICAgfSBlbHNlIHtcbiAgICAgICAgcmV0dXJuIHZhbGlkYXRlKHByb3BzLCBwcm9wTmFtZSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSk7XG4gICAgICB9XG4gICAgfVxuXG4gICAgdmFyIGNoYWluZWRDaGVja1R5cGUgPSBjaGVja1R5cGUuYmluZChudWxsLCBmYWxzZSk7XG4gICAgY2hhaW5lZENoZWNrVHlwZS5pc1JlcXVpcmVkID0gY2hlY2tUeXBlLmJpbmQobnVsbCwgdHJ1ZSk7XG5cbiAgICByZXR1cm4gY2hhaW5lZENoZWNrVHlwZTtcbiAgfVxuXG4gIGZ1bmN0aW9uIGNyZWF0ZVByaW1pdGl2ZVR5cGVDaGVja2VyKGV4cGVjdGVkVHlwZSkge1xuICAgIGZ1bmN0aW9uIHZhbGlkYXRlKHByb3BzLCBwcm9wTmFtZSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSwgc2VjcmV0KSB7XG4gICAgICB2YXIgcHJvcFZhbHVlID0gcHJvcHNbcHJvcE5hbWVdO1xuICAgICAgdmFyIHByb3BUeXBlID0gZ2V0UHJvcFR5cGUocHJvcFZhbHVlKTtcbiAgICAgIGlmIChwcm9wVHlwZSAhPT0gZXhwZWN0ZWRUeXBlKSB7XG4gICAgICAgIC8vIGBwcm9wVmFsdWVgIGJlaW5nIGluc3RhbmNlIG9mLCBzYXksIGRhdGUvcmVnZXhwLCBwYXNzIHRoZSAnb2JqZWN0J1xuICAgICAgICAvLyBjaGVjaywgYnV0IHdlIGNhbiBvZmZlciBhIG1vcmUgcHJlY2lzZSBlcnJvciBtZXNzYWdlIGhlcmUgcmF0aGVyIHRoYW5cbiAgICAgICAgLy8gJ29mIHR5cGUgYG9iamVjdGAnLlxuICAgICAgICB2YXIgcHJlY2lzZVR5cGUgPSBnZXRQcmVjaXNlVHlwZShwcm9wVmFsdWUpO1xuXG4gICAgICAgIHJldHVybiBuZXcgUHJvcFR5cGVFcnJvcignSW52YWxpZCAnICsgbG9jYXRpb24gKyAnIGAnICsgcHJvcEZ1bGxOYW1lICsgJ2Agb2YgdHlwZSAnICsgKCdgJyArIHByZWNpc2VUeXBlICsgJ2Agc3VwcGxpZWQgdG8gYCcgKyBjb21wb25lbnROYW1lICsgJ2AsIGV4cGVjdGVkICcpICsgKCdgJyArIGV4cGVjdGVkVHlwZSArICdgLicpKTtcbiAgICAgIH1cbiAgICAgIHJldHVybiBudWxsO1xuICAgIH1cbiAgICByZXR1cm4gY3JlYXRlQ2hhaW5hYmxlVHlwZUNoZWNrZXIodmFsaWRhdGUpO1xuICB9XG5cbiAgZnVuY3Rpb24gY3JlYXRlQW55VHlwZUNoZWNrZXIoKSB7XG4gICAgcmV0dXJuIGNyZWF0ZUNoYWluYWJsZVR5cGVDaGVja2VyKGVtcHR5RnVuY3Rpb24udGhhdFJldHVybnNOdWxsKTtcbiAgfVxuXG4gIGZ1bmN0aW9uIGNyZWF0ZUFycmF5T2ZUeXBlQ2hlY2tlcih0eXBlQ2hlY2tlcikge1xuICAgIGZ1bmN0aW9uIHZhbGlkYXRlKHByb3BzLCBwcm9wTmFtZSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSkge1xuICAgICAgaWYgKHR5cGVvZiB0eXBlQ2hlY2tlciAhPT0gJ2Z1bmN0aW9uJykge1xuICAgICAgICByZXR1cm4gbmV3IFByb3BUeXBlRXJyb3IoJ1Byb3BlcnR5IGAnICsgcHJvcEZ1bGxOYW1lICsgJ2Agb2YgY29tcG9uZW50IGAnICsgY29tcG9uZW50TmFtZSArICdgIGhhcyBpbnZhbGlkIFByb3BUeXBlIG5vdGF0aW9uIGluc2lkZSBhcnJheU9mLicpO1xuICAgICAgfVxuICAgICAgdmFyIHByb3BWYWx1ZSA9IHByb3BzW3Byb3BOYW1lXTtcbiAgICAgIGlmICghQXJyYXkuaXNBcnJheShwcm9wVmFsdWUpKSB7XG4gICAgICAgIHZhciBwcm9wVHlwZSA9IGdldFByb3BUeXBlKHByb3BWYWx1ZSk7XG4gICAgICAgIHJldHVybiBuZXcgUHJvcFR5cGVFcnJvcignSW52YWxpZCAnICsgbG9jYXRpb24gKyAnIGAnICsgcHJvcEZ1bGxOYW1lICsgJ2Agb2YgdHlwZSAnICsgKCdgJyArIHByb3BUeXBlICsgJ2Agc3VwcGxpZWQgdG8gYCcgKyBjb21wb25lbnROYW1lICsgJ2AsIGV4cGVjdGVkIGFuIGFycmF5LicpKTtcbiAgICAgIH1cbiAgICAgIGZvciAodmFyIGkgPSAwOyBpIDwgcHJvcFZhbHVlLmxlbmd0aDsgaSsrKSB7XG4gICAgICAgIHZhciBlcnJvciA9IHR5cGVDaGVja2VyKHByb3BWYWx1ZSwgaSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSArICdbJyArIGkgKyAnXScsIFJlYWN0UHJvcFR5cGVzU2VjcmV0KTtcbiAgICAgICAgaWYgKGVycm9yIGluc3RhbmNlb2YgRXJyb3IpIHtcbiAgICAgICAgICByZXR1cm4gZXJyb3I7XG4gICAgICAgIH1cbiAgICAgIH1cbiAgICAgIHJldHVybiBudWxsO1xuICAgIH1cbiAgICByZXR1cm4gY3JlYXRlQ2hhaW5hYmxlVHlwZUNoZWNrZXIodmFsaWRhdGUpO1xuICB9XG5cbiAgZnVuY3Rpb24gY3JlYXRlRWxlbWVudFR5cGVDaGVja2VyKCkge1xuICAgIGZ1bmN0aW9uIHZhbGlkYXRlKHByb3BzLCBwcm9wTmFtZSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSkge1xuICAgICAgdmFyIHByb3BWYWx1ZSA9IHByb3BzW3Byb3BOYW1lXTtcbiAgICAgIGlmICghaXNWYWxpZEVsZW1lbnQocHJvcFZhbHVlKSkge1xuICAgICAgICB2YXIgcHJvcFR5cGUgPSBnZXRQcm9wVHlwZShwcm9wVmFsdWUpO1xuICAgICAgICByZXR1cm4gbmV3IFByb3BUeXBlRXJyb3IoJ0ludmFsaWQgJyArIGxvY2F0aW9uICsgJyBgJyArIHByb3BGdWxsTmFtZSArICdgIG9mIHR5cGUgJyArICgnYCcgKyBwcm9wVHlwZSArICdgIHN1cHBsaWVkIHRvIGAnICsgY29tcG9uZW50TmFtZSArICdgLCBleHBlY3RlZCBhIHNpbmdsZSBSZWFjdEVsZW1lbnQuJykpO1xuICAgICAgfVxuICAgICAgcmV0dXJuIG51bGw7XG4gICAgfVxuICAgIHJldHVybiBjcmVhdGVDaGFpbmFibGVUeXBlQ2hlY2tlcih2YWxpZGF0ZSk7XG4gIH1cblxuICBmdW5jdGlvbiBjcmVhdGVJbnN0YW5jZVR5cGVDaGVja2VyKGV4cGVjdGVkQ2xhc3MpIHtcbiAgICBmdW5jdGlvbiB2YWxpZGF0ZShwcm9wcywgcHJvcE5hbWUsIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUpIHtcbiAgICAgIGlmICghKHByb3BzW3Byb3BOYW1lXSBpbnN0YW5jZW9mIGV4cGVjdGVkQ2xhc3MpKSB7XG4gICAgICAgIHZhciBleHBlY3RlZENsYXNzTmFtZSA9IGV4cGVjdGVkQ2xhc3MubmFtZSB8fCBBTk9OWU1PVVM7XG4gICAgICAgIHZhciBhY3R1YWxDbGFzc05hbWUgPSBnZXRDbGFzc05hbWUocHJvcHNbcHJvcE5hbWVdKTtcbiAgICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKCdJbnZhbGlkICcgKyBsb2NhdGlvbiArICcgYCcgKyBwcm9wRnVsbE5hbWUgKyAnYCBvZiB0eXBlICcgKyAoJ2AnICsgYWN0dWFsQ2xhc3NOYW1lICsgJ2Agc3VwcGxpZWQgdG8gYCcgKyBjb21wb25lbnROYW1lICsgJ2AsIGV4cGVjdGVkICcpICsgKCdpbnN0YW5jZSBvZiBgJyArIGV4cGVjdGVkQ2xhc3NOYW1lICsgJ2AuJykpO1xuICAgICAgfVxuICAgICAgcmV0dXJuIG51bGw7XG4gICAgfVxuICAgIHJldHVybiBjcmVhdGVDaGFpbmFibGVUeXBlQ2hlY2tlcih2YWxpZGF0ZSk7XG4gIH1cblxuICBmdW5jdGlvbiBjcmVhdGVFbnVtVHlwZUNoZWNrZXIoZXhwZWN0ZWRWYWx1ZXMpIHtcbiAgICBpZiAoIUFycmF5LmlzQXJyYXkoZXhwZWN0ZWRWYWx1ZXMpKSB7XG4gICAgICBwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nID8gd2FybmluZyhmYWxzZSwgJ0ludmFsaWQgYXJndW1lbnQgc3VwcGxpZWQgdG8gb25lT2YsIGV4cGVjdGVkIGFuIGluc3RhbmNlIG9mIGFycmF5LicpIDogdm9pZCAwO1xuICAgICAgcmV0dXJuIGVtcHR5RnVuY3Rpb24udGhhdFJldHVybnNOdWxsO1xuICAgIH1cblxuICAgIGZ1bmN0aW9uIHZhbGlkYXRlKHByb3BzLCBwcm9wTmFtZSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSkge1xuICAgICAgdmFyIHByb3BWYWx1ZSA9IHByb3BzW3Byb3BOYW1lXTtcbiAgICAgIGZvciAodmFyIGkgPSAwOyBpIDwgZXhwZWN0ZWRWYWx1ZXMubGVuZ3RoOyBpKyspIHtcbiAgICAgICAgaWYgKGlzKHByb3BWYWx1ZSwgZXhwZWN0ZWRWYWx1ZXNbaV0pKSB7XG4gICAgICAgICAgcmV0dXJuIG51bGw7XG4gICAgICAgIH1cbiAgICAgIH1cblxuICAgICAgdmFyIHZhbHVlc1N0cmluZyA9IEpTT04uc3RyaW5naWZ5KGV4cGVjdGVkVmFsdWVzKTtcbiAgICAgIHJldHVybiBuZXcgUHJvcFR5cGVFcnJvcignSW52YWxpZCAnICsgbG9jYXRpb24gKyAnIGAnICsgcHJvcEZ1bGxOYW1lICsgJ2Agb2YgdmFsdWUgYCcgKyBwcm9wVmFsdWUgKyAnYCAnICsgKCdzdXBwbGllZCB0byBgJyArIGNvbXBvbmVudE5hbWUgKyAnYCwgZXhwZWN0ZWQgb25lIG9mICcgKyB2YWx1ZXNTdHJpbmcgKyAnLicpKTtcbiAgICB9XG4gICAgcmV0dXJuIGNyZWF0ZUNoYWluYWJsZVR5cGVDaGVja2VyKHZhbGlkYXRlKTtcbiAgfVxuXG4gIGZ1bmN0aW9uIGNyZWF0ZU9iamVjdE9mVHlwZUNoZWNrZXIodHlwZUNoZWNrZXIpIHtcbiAgICBmdW5jdGlvbiB2YWxpZGF0ZShwcm9wcywgcHJvcE5hbWUsIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUpIHtcbiAgICAgIGlmICh0eXBlb2YgdHlwZUNoZWNrZXIgIT09ICdmdW5jdGlvbicpIHtcbiAgICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKCdQcm9wZXJ0eSBgJyArIHByb3BGdWxsTmFtZSArICdgIG9mIGNvbXBvbmVudCBgJyArIGNvbXBvbmVudE5hbWUgKyAnYCBoYXMgaW52YWxpZCBQcm9wVHlwZSBub3RhdGlvbiBpbnNpZGUgb2JqZWN0T2YuJyk7XG4gICAgICB9XG4gICAgICB2YXIgcHJvcFZhbHVlID0gcHJvcHNbcHJvcE5hbWVdO1xuICAgICAgdmFyIHByb3BUeXBlID0gZ2V0UHJvcFR5cGUocHJvcFZhbHVlKTtcbiAgICAgIGlmIChwcm9wVHlwZSAhPT0gJ29iamVjdCcpIHtcbiAgICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKCdJbnZhbGlkICcgKyBsb2NhdGlvbiArICcgYCcgKyBwcm9wRnVsbE5hbWUgKyAnYCBvZiB0eXBlICcgKyAoJ2AnICsgcHJvcFR5cGUgKyAnYCBzdXBwbGllZCB0byBgJyArIGNvbXBvbmVudE5hbWUgKyAnYCwgZXhwZWN0ZWQgYW4gb2JqZWN0LicpKTtcbiAgICAgIH1cbiAgICAgIGZvciAodmFyIGtleSBpbiBwcm9wVmFsdWUpIHtcbiAgICAgICAgaWYgKHByb3BWYWx1ZS5oYXNPd25Qcm9wZXJ0eShrZXkpKSB7XG4gICAgICAgICAgdmFyIGVycm9yID0gdHlwZUNoZWNrZXIocHJvcFZhbHVlLCBrZXksIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUgKyAnLicgKyBrZXksIFJlYWN0UHJvcFR5cGVzU2VjcmV0KTtcbiAgICAgICAgICBpZiAoZXJyb3IgaW5zdGFuY2VvZiBFcnJvcikge1xuICAgICAgICAgICAgcmV0dXJuIGVycm9yO1xuICAgICAgICAgIH1cbiAgICAgICAgfVxuICAgICAgfVxuICAgICAgcmV0dXJuIG51bGw7XG4gICAgfVxuICAgIHJldHVybiBjcmVhdGVDaGFpbmFibGVUeXBlQ2hlY2tlcih2YWxpZGF0ZSk7XG4gIH1cblxuICBmdW5jdGlvbiBjcmVhdGVVbmlvblR5cGVDaGVja2VyKGFycmF5T2ZUeXBlQ2hlY2tlcnMpIHtcbiAgICBpZiAoIUFycmF5LmlzQXJyYXkoYXJyYXlPZlR5cGVDaGVja2VycykpIHtcbiAgICAgIHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicgPyB3YXJuaW5nKGZhbHNlLCAnSW52YWxpZCBhcmd1bWVudCBzdXBwbGllZCB0byBvbmVPZlR5cGUsIGV4cGVjdGVkIGFuIGluc3RhbmNlIG9mIGFycmF5LicpIDogdm9pZCAwO1xuICAgICAgcmV0dXJuIGVtcHR5RnVuY3Rpb24udGhhdFJldHVybnNOdWxsO1xuICAgIH1cblxuICAgIGZvciAodmFyIGkgPSAwOyBpIDwgYXJyYXlPZlR5cGVDaGVja2Vycy5sZW5ndGg7IGkrKykge1xuICAgICAgdmFyIGNoZWNrZXIgPSBhcnJheU9mVHlwZUNoZWNrZXJzW2ldO1xuICAgICAgaWYgKHR5cGVvZiBjaGVja2VyICE9PSAnZnVuY3Rpb24nKSB7XG4gICAgICAgIHdhcm5pbmcoXG4gICAgICAgICAgZmFsc2UsXG4gICAgICAgICAgJ0ludmFsaWQgYXJndW1lbnQgc3VwcGxpZWQgdG8gb25lT2ZUeXBlLiBFeHBlY3RlZCBhbiBhcnJheSBvZiBjaGVjayBmdW5jdGlvbnMsIGJ1dCAnICtcbiAgICAgICAgICAncmVjZWl2ZWQgJXMgYXQgaW5kZXggJXMuJyxcbiAgICAgICAgICBnZXRQb3N0Zml4Rm9yVHlwZVdhcm5pbmcoY2hlY2tlciksXG4gICAgICAgICAgaVxuICAgICAgICApO1xuICAgICAgICByZXR1cm4gZW1wdHlGdW5jdGlvbi50aGF0UmV0dXJuc051bGw7XG4gICAgICB9XG4gICAgfVxuXG4gICAgZnVuY3Rpb24gdmFsaWRhdGUocHJvcHMsIHByb3BOYW1lLCBjb21wb25lbnROYW1lLCBsb2NhdGlvbiwgcHJvcEZ1bGxOYW1lKSB7XG4gICAgICBmb3IgKHZhciBpID0gMDsgaSA8IGFycmF5T2ZUeXBlQ2hlY2tlcnMubGVuZ3RoOyBpKyspIHtcbiAgICAgICAgdmFyIGNoZWNrZXIgPSBhcnJheU9mVHlwZUNoZWNrZXJzW2ldO1xuICAgICAgICBpZiAoY2hlY2tlcihwcm9wcywgcHJvcE5hbWUsIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUsIFJlYWN0UHJvcFR5cGVzU2VjcmV0KSA9PSBudWxsKSB7XG4gICAgICAgICAgcmV0dXJuIG51bGw7XG4gICAgICAgIH1cbiAgICAgIH1cblxuICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKCdJbnZhbGlkICcgKyBsb2NhdGlvbiArICcgYCcgKyBwcm9wRnVsbE5hbWUgKyAnYCBzdXBwbGllZCB0byAnICsgKCdgJyArIGNvbXBvbmVudE5hbWUgKyAnYC4nKSk7XG4gICAgfVxuICAgIHJldHVybiBjcmVhdGVDaGFpbmFibGVUeXBlQ2hlY2tlcih2YWxpZGF0ZSk7XG4gIH1cblxuICBmdW5jdGlvbiBjcmVhdGVOb2RlQ2hlY2tlcigpIHtcbiAgICBmdW5jdGlvbiB2YWxpZGF0ZShwcm9wcywgcHJvcE5hbWUsIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUpIHtcbiAgICAgIGlmICghaXNOb2RlKHByb3BzW3Byb3BOYW1lXSkpIHtcbiAgICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKCdJbnZhbGlkICcgKyBsb2NhdGlvbiArICcgYCcgKyBwcm9wRnVsbE5hbWUgKyAnYCBzdXBwbGllZCB0byAnICsgKCdgJyArIGNvbXBvbmVudE5hbWUgKyAnYCwgZXhwZWN0ZWQgYSBSZWFjdE5vZGUuJykpO1xuICAgICAgfVxuICAgICAgcmV0dXJuIG51bGw7XG4gICAgfVxuICAgIHJldHVybiBjcmVhdGVDaGFpbmFibGVUeXBlQ2hlY2tlcih2YWxpZGF0ZSk7XG4gIH1cblxuICBmdW5jdGlvbiBjcmVhdGVTaGFwZVR5cGVDaGVja2VyKHNoYXBlVHlwZXMpIHtcbiAgICBmdW5jdGlvbiB2YWxpZGF0ZShwcm9wcywgcHJvcE5hbWUsIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUpIHtcbiAgICAgIHZhciBwcm9wVmFsdWUgPSBwcm9wc1twcm9wTmFtZV07XG4gICAgICB2YXIgcHJvcFR5cGUgPSBnZXRQcm9wVHlwZShwcm9wVmFsdWUpO1xuICAgICAgaWYgKHByb3BUeXBlICE9PSAnb2JqZWN0Jykge1xuICAgICAgICByZXR1cm4gbmV3IFByb3BUeXBlRXJyb3IoJ0ludmFsaWQgJyArIGxvY2F0aW9uICsgJyBgJyArIHByb3BGdWxsTmFtZSArICdgIG9mIHR5cGUgYCcgKyBwcm9wVHlwZSArICdgICcgKyAoJ3N1cHBsaWVkIHRvIGAnICsgY29tcG9uZW50TmFtZSArICdgLCBleHBlY3RlZCBgb2JqZWN0YC4nKSk7XG4gICAgICB9XG4gICAgICBmb3IgKHZhciBrZXkgaW4gc2hhcGVUeXBlcykge1xuICAgICAgICB2YXIgY2hlY2tlciA9IHNoYXBlVHlwZXNba2V5XTtcbiAgICAgICAgaWYgKCFjaGVja2VyKSB7XG4gICAgICAgICAgY29udGludWU7XG4gICAgICAgIH1cbiAgICAgICAgdmFyIGVycm9yID0gY2hlY2tlcihwcm9wVmFsdWUsIGtleSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSArICcuJyArIGtleSwgUmVhY3RQcm9wVHlwZXNTZWNyZXQpO1xuICAgICAgICBpZiAoZXJyb3IpIHtcbiAgICAgICAgICByZXR1cm4gZXJyb3I7XG4gICAgICAgIH1cbiAgICAgIH1cbiAgICAgIHJldHVybiBudWxsO1xuICAgIH1cbiAgICByZXR1cm4gY3JlYXRlQ2hhaW5hYmxlVHlwZUNoZWNrZXIodmFsaWRhdGUpO1xuICB9XG5cbiAgZnVuY3Rpb24gY3JlYXRlU3RyaWN0U2hhcGVUeXBlQ2hlY2tlcihzaGFwZVR5cGVzKSB7XG4gICAgZnVuY3Rpb24gdmFsaWRhdGUocHJvcHMsIHByb3BOYW1lLCBjb21wb25lbnROYW1lLCBsb2NhdGlvbiwgcHJvcEZ1bGxOYW1lKSB7XG4gICAgICB2YXIgcHJvcFZhbHVlID0gcHJvcHNbcHJvcE5hbWVdO1xuICAgICAgdmFyIHByb3BUeXBlID0gZ2V0UHJvcFR5cGUocHJvcFZhbHVlKTtcbiAgICAgIGlmIChwcm9wVHlwZSAhPT0gJ29iamVjdCcpIHtcbiAgICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKCdJbnZhbGlkICcgKyBsb2NhdGlvbiArICcgYCcgKyBwcm9wRnVsbE5hbWUgKyAnYCBvZiB0eXBlIGAnICsgcHJvcFR5cGUgKyAnYCAnICsgKCdzdXBwbGllZCB0byBgJyArIGNvbXBvbmVudE5hbWUgKyAnYCwgZXhwZWN0ZWQgYG9iamVjdGAuJykpO1xuICAgICAgfVxuICAgICAgLy8gV2UgbmVlZCB0byBjaGVjayBhbGwga2V5cyBpbiBjYXNlIHNvbWUgYXJlIHJlcXVpcmVkIGJ1dCBtaXNzaW5nIGZyb21cbiAgICAgIC8vIHByb3BzLlxuICAgICAgdmFyIGFsbEtleXMgPSBhc3NpZ24oe30sIHByb3BzW3Byb3BOYW1lXSwgc2hhcGVUeXBlcyk7XG4gICAgICBmb3IgKHZhciBrZXkgaW4gYWxsS2V5cykge1xuICAgICAgICB2YXIgY2hlY2tlciA9IHNoYXBlVHlwZXNba2V5XTtcbiAgICAgICAgaWYgKCFjaGVja2VyKSB7XG4gICAgICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKFxuICAgICAgICAgICAgJ0ludmFsaWQgJyArIGxvY2F0aW9uICsgJyBgJyArIHByb3BGdWxsTmFtZSArICdgIGtleSBgJyArIGtleSArICdgIHN1cHBsaWVkIHRvIGAnICsgY29tcG9uZW50TmFtZSArICdgLicgK1xuICAgICAgICAgICAgJ1xcbkJhZCBvYmplY3Q6ICcgKyBKU09OLnN0cmluZ2lmeShwcm9wc1twcm9wTmFtZV0sIG51bGwsICcgICcpICtcbiAgICAgICAgICAgICdcXG5WYWxpZCBrZXlzOiAnICsgIEpTT04uc3RyaW5naWZ5KE9iamVjdC5rZXlzKHNoYXBlVHlwZXMpLCBudWxsLCAnICAnKVxuICAgICAgICAgICk7XG4gICAgICAgIH1cbiAgICAgICAgdmFyIGVycm9yID0gY2hlY2tlcihwcm9wVmFsdWUsIGtleSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSArICcuJyArIGtleSwgUmVhY3RQcm9wVHlwZXNTZWNyZXQpO1xuICAgICAgICBpZiAoZXJyb3IpIHtcbiAgICAgICAgICByZXR1cm4gZXJyb3I7XG4gICAgICAgIH1cbiAgICAgIH1cbiAgICAgIHJldHVybiBudWxsO1xuICAgIH1cblxuICAgIHJldHVybiBjcmVhdGVDaGFpbmFibGVUeXBlQ2hlY2tlcih2YWxpZGF0ZSk7XG4gIH1cblxuICBmdW5jdGlvbiBpc05vZGUocHJvcFZhbHVlKSB7XG4gICAgc3dpdGNoICh0eXBlb2YgcHJvcFZhbHVlKSB7XG4gICAgICBjYXNlICdudW1iZXInOlxuICAgICAgY2FzZSAnc3RyaW5nJzpcbiAgICAgIGNhc2UgJ3VuZGVmaW5lZCc6XG4gICAgICAgIHJldHVybiB0cnVlO1xuICAgICAgY2FzZSAnYm9vbGVhbic6XG4gICAgICAgIHJldHVybiAhcHJvcFZhbHVlO1xuICAgICAgY2FzZSAnb2JqZWN0JzpcbiAgICAgICAgaWYgKEFycmF5LmlzQXJyYXkocHJvcFZhbHVlKSkge1xuICAgICAgICAgIHJldHVybiBwcm9wVmFsdWUuZXZlcnkoaXNOb2RlKTtcbiAgICAgICAgfVxuICAgICAgICBpZiAocHJvcFZhbHVlID09PSBudWxsIHx8IGlzVmFsaWRFbGVtZW50KHByb3BWYWx1ZSkpIHtcbiAgICAgICAgICByZXR1cm4gdHJ1ZTtcbiAgICAgICAgfVxuXG4gICAgICAgIHZhciBpdGVyYXRvckZuID0gZ2V0SXRlcmF0b3JGbihwcm9wVmFsdWUpO1xuICAgICAgICBpZiAoaXRlcmF0b3JGbikge1xuICAgICAgICAgIHZhciBpdGVyYXRvciA9IGl0ZXJhdG9yRm4uY2FsbChwcm9wVmFsdWUpO1xuICAgICAgICAgIHZhciBzdGVwO1xuICAgICAgICAgIGlmIChpdGVyYXRvckZuICE9PSBwcm9wVmFsdWUuZW50cmllcykge1xuICAgICAgICAgICAgd2hpbGUgKCEoc3RlcCA9IGl0ZXJhdG9yLm5leHQoKSkuZG9uZSkge1xuICAgICAgICAgICAgICBpZiAoIWlzTm9kZShzdGVwLnZhbHVlKSkge1xuICAgICAgICAgICAgICAgIHJldHVybiBmYWxzZTtcbiAgICAgICAgICAgICAgfVxuICAgICAgICAgICAgfVxuICAgICAgICAgIH0gZWxzZSB7XG4gICAgICAgICAgICAvLyBJdGVyYXRvciB3aWxsIHByb3ZpZGUgZW50cnkgW2ssdl0gdHVwbGVzIHJhdGhlciB0aGFuIHZhbHVlcy5cbiAgICAgICAgICAgIHdoaWxlICghKHN0ZXAgPSBpdGVyYXRvci5uZXh0KCkpLmRvbmUpIHtcbiAgICAgICAgICAgICAgdmFyIGVudHJ5ID0gc3RlcC52YWx1ZTtcbiAgICAgICAgICAgICAgaWYgKGVudHJ5KSB7XG4gICAgICAgICAgICAgICAgaWYgKCFpc05vZGUoZW50cnlbMV0pKSB7XG4gICAgICAgICAgICAgICAgICByZXR1cm4gZmFsc2U7XG4gICAgICAgICAgICAgICAgfVxuICAgICAgICAgICAgICB9XG4gICAgICAgICAgICB9XG4gICAgICAgICAgfVxuICAgICAgICB9IGVsc2Uge1xuICAgICAgICAgIHJldHVybiBmYWxzZTtcbiAgICAgICAgfVxuXG4gICAgICAgIHJldHVybiB0cnVlO1xuICAgICAgZGVmYXVsdDpcbiAgICAgICAgcmV0dXJuIGZhbHNlO1xuICAgIH1cbiAgfVxuXG4gIGZ1bmN0aW9uIGlzU3ltYm9sKHByb3BUeXBlLCBwcm9wVmFsdWUpIHtcbiAgICAvLyBOYXRpdmUgU3ltYm9sLlxuICAgIGlmIChwcm9wVHlwZSA9PT0gJ3N5bWJvbCcpIHtcbiAgICAgIHJldHVybiB0cnVlO1xuICAgIH1cblxuICAgIC8vIDE5LjQuMy41IFN5bWJvbC5wcm90b3R5cGVbQEB0b1N0cmluZ1RhZ10gPT09ICdTeW1ib2wnXG4gICAgaWYgKHByb3BWYWx1ZVsnQEB0b1N0cmluZ1RhZyddID09PSAnU3ltYm9sJykge1xuICAgICAgcmV0dXJuIHRydWU7XG4gICAgfVxuXG4gICAgLy8gRmFsbGJhY2sgZm9yIG5vbi1zcGVjIGNvbXBsaWFudCBTeW1ib2xzIHdoaWNoIGFyZSBwb2x5ZmlsbGVkLlxuICAgIGlmICh0eXBlb2YgU3ltYm9sID09PSAnZnVuY3Rpb24nICYmIHByb3BWYWx1ZSBpbnN0YW5jZW9mIFN5bWJvbCkge1xuICAgICAgcmV0dXJuIHRydWU7XG4gICAgfVxuXG4gICAgcmV0dXJuIGZhbHNlO1xuICB9XG5cbiAgLy8gRXF1aXZhbGVudCBvZiBgdHlwZW9mYCBidXQgd2l0aCBzcGVjaWFsIGhhbmRsaW5nIGZvciBhcnJheSBhbmQgcmVnZXhwLlxuICBmdW5jdGlvbiBnZXRQcm9wVHlwZShwcm9wVmFsdWUpIHtcbiAgICB2YXIgcHJvcFR5cGUgPSB0eXBlb2YgcHJvcFZhbHVlO1xuICAgIGlmIChBcnJheS5pc0FycmF5KHByb3BWYWx1ZSkpIHtcbiAgICAgIHJldHVybiAnYXJyYXknO1xuICAgIH1cbiAgICBpZiAocHJvcFZhbHVlIGluc3RhbmNlb2YgUmVnRXhwKSB7XG4gICAgICAvLyBPbGQgd2Via2l0cyAoYXQgbGVhc3QgdW50aWwgQW5kcm9pZCA0LjApIHJldHVybiAnZnVuY3Rpb24nIHJhdGhlciB0aGFuXG4gICAgICAvLyAnb2JqZWN0JyBmb3IgdHlwZW9mIGEgUmVnRXhwLiBXZSdsbCBub3JtYWxpemUgdGhpcyBoZXJlIHNvIHRoYXQgL2JsYS9cbiAgICAgIC8vIHBhc3NlcyBQcm9wVHlwZXMub2JqZWN0LlxuICAgICAgcmV0dXJuICdvYmplY3QnO1xuICAgIH1cbiAgICBpZiAoaXNTeW1ib2wocHJvcFR5cGUsIHByb3BWYWx1ZSkpIHtcbiAgICAgIHJldHVybiAnc3ltYm9sJztcbiAgICB9XG4gICAgcmV0dXJuIHByb3BUeXBlO1xuICB9XG5cbiAgLy8gVGhpcyBoYW5kbGVzIG1vcmUgdHlwZXMgdGhhbiBgZ2V0UHJvcFR5cGVgLiBPbmx5IHVzZWQgZm9yIGVycm9yIG1lc3NhZ2VzLlxuICAvLyBTZWUgYGNyZWF0ZVByaW1pdGl2ZVR5cGVDaGVja2VyYC5cbiAgZnVuY3Rpb24gZ2V0UHJlY2lzZVR5cGUocHJvcFZhbHVlKSB7XG4gICAgaWYgKHR5cGVvZiBwcm9wVmFsdWUgPT09ICd1bmRlZmluZWQnIHx8IHByb3BWYWx1ZSA9PT0gbnVsbCkge1xuICAgICAgcmV0dXJuICcnICsgcHJvcFZhbHVlO1xuICAgIH1cbiAgICB2YXIgcHJvcFR5cGUgPSBnZXRQcm9wVHlwZShwcm9wVmFsdWUpO1xuICAgIGlmIChwcm9wVHlwZSA9PT0gJ29iamVjdCcpIHtcbiAgICAgIGlmIChwcm9wVmFsdWUgaW5zdGFuY2VvZiBEYXRlKSB7XG4gICAgICAgIHJldHVybiAnZGF0ZSc7XG4gICAgICB9IGVsc2UgaWYgKHByb3BWYWx1ZSBpbnN0YW5jZW9mIFJlZ0V4cCkge1xuICAgICAgICByZXR1cm4gJ3JlZ2V4cCc7XG4gICAgICB9XG4gICAgfVxuICAgIHJldHVybiBwcm9wVHlwZTtcbiAgfVxuXG4gIC8vIFJldHVybnMgYSBzdHJpbmcgdGhhdCBpcyBwb3N0Zml4ZWQgdG8gYSB3YXJuaW5nIGFib3V0IGFuIGludmFsaWQgdHlwZS5cbiAgLy8gRm9yIGV4YW1wbGUsIFwidW5kZWZpbmVkXCIgb3IgXCJvZiB0eXBlIGFycmF5XCJcbiAgZnVuY3Rpb24gZ2V0UG9zdGZpeEZvclR5cGVXYXJuaW5nKHZhbHVlKSB7XG4gICAgdmFyIHR5cGUgPSBnZXRQcmVjaXNlVHlwZSh2YWx1ZSk7XG4gICAgc3dpdGNoICh0eXBlKSB7XG4gICAgICBjYXNlICdhcnJheSc6XG4gICAgICBjYXNlICdvYmplY3QnOlxuICAgICAgICByZXR1cm4gJ2FuICcgKyB0eXBlO1xuICAgICAgY2FzZSAnYm9vbGVhbic6XG4gICAgICBjYXNlICdkYXRlJzpcbiAgICAgIGNhc2UgJ3JlZ2V4cCc6XG4gICAgICAgIHJldHVybiAnYSAnICsgdHlwZTtcbiAgICAgIGRlZmF1bHQ6XG4gICAgICAgIHJldHVybiB0eXBlO1xuICAgIH1cbiAgfVxuXG4gIC8vIFJldHVybnMgY2xhc3MgbmFtZSBvZiB0aGUgb2JqZWN0LCBpZiBhbnkuXG4gIGZ1bmN0aW9uIGdldENsYXNzTmFtZShwcm9wVmFsdWUpIHtcbiAgICBpZiAoIXByb3BWYWx1ZS5jb25zdHJ1Y3RvciB8fCAhcHJvcFZhbHVlLmNvbnN0cnVjdG9yLm5hbWUpIHtcbiAgICAgIHJldHVybiBBTk9OWU1PVVM7XG4gICAgfVxuICAgIHJldHVybiBwcm9wVmFsdWUuY29uc3RydWN0b3IubmFtZTtcbiAgfVxuXG4gIFJlYWN0UHJvcFR5cGVzLmNoZWNrUHJvcFR5cGVzID0gY2hlY2tQcm9wVHlwZXM7XG4gIFJlYWN0UHJvcFR5cGVzLlByb3BUeXBlcyA9IFJlYWN0UHJvcFR5cGVzO1xuXG4gIHJldHVybiBSZWFjdFByb3BUeXBlcztcbn07XG4iLCIvKipcbiAqIENvcHlyaWdodCAoYykgMjAxMy1wcmVzZW50LCBGYWNlYm9vaywgSW5jLlxuICpcbiAqIFRoaXMgc291cmNlIGNvZGUgaXMgbGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlIGZvdW5kIGluIHRoZVxuICogTElDRU5TRSBmaWxlIGluIHRoZSByb290IGRpcmVjdG9yeSBvZiB0aGlzIHNvdXJjZSB0cmVlLlxuICovXG5cbmlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gIHZhciBSRUFDVF9FTEVNRU5UX1RZUEUgPSAodHlwZW9mIFN5bWJvbCA9PT0gJ2Z1bmN0aW9uJyAmJlxuICAgIFN5bWJvbC5mb3IgJiZcbiAgICBTeW1ib2wuZm9yKCdyZWFjdC5lbGVtZW50JykpIHx8XG4gICAgMHhlYWM3O1xuXG4gIHZhciBpc1ZhbGlkRWxlbWVudCA9IGZ1bmN0aW9uKG9iamVjdCkge1xuICAgIHJldHVybiB0eXBlb2Ygb2JqZWN0ID09PSAnb2JqZWN0JyAmJlxuICAgICAgb2JqZWN0ICE9PSBudWxsICYmXG4gICAgICBvYmplY3QuJCR0eXBlb2YgPT09IFJFQUNUX0VMRU1FTlRfVFlQRTtcbiAgfTtcblxuICAvLyBCeSBleHBsaWNpdGx5IHVzaW5nIGBwcm9wLXR5cGVzYCB5b3UgYXJlIG9wdGluZyBpbnRvIG5ldyBkZXZlbG9wbWVudCBiZWhhdmlvci5cbiAgLy8gaHR0cDovL2ZiLm1lL3Byb3AtdHlwZXMtaW4tcHJvZFxuICB2YXIgdGhyb3dPbkRpcmVjdEFjY2VzcyA9IHRydWU7XG4gIG1vZHVsZS5leHBvcnRzID0gcmVxdWlyZSgnLi9mYWN0b3J5V2l0aFR5cGVDaGVja2VycycpKGlzVmFsaWRFbGVtZW50LCB0aHJvd09uRGlyZWN0QWNjZXNzKTtcbn0gZWxzZSB7XG4gIC8vIEJ5IGV4cGxpY2l0bHkgdXNpbmcgYHByb3AtdHlwZXNgIHlvdSBhcmUgb3B0aW5nIGludG8gbmV3IHByb2R1Y3Rpb24gYmVoYXZpb3IuXG4gIC8vIGh0dHA6Ly9mYi5tZS9wcm9wLXR5cGVzLWluLXByb2RcbiAgbW9kdWxlLmV4cG9ydHMgPSByZXF1aXJlKCcuL2ZhY3RvcnlXaXRoVGhyb3dpbmdTaGltcycpKCk7XG59XG4iLCIvKipcbiAqIENvcHlyaWdodCAoYykgMjAxMy1wcmVzZW50LCBGYWNlYm9vaywgSW5jLlxuICpcbiAqIFRoaXMgc291cmNlIGNvZGUgaXMgbGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlIGZvdW5kIGluIHRoZVxuICogTElDRU5TRSBmaWxlIGluIHRoZSByb290IGRpcmVjdG9yeSBvZiB0aGlzIHNvdXJjZSB0cmVlLlxuICovXG5cbid1c2Ugc3RyaWN0JztcblxudmFyIFJlYWN0UHJvcFR5cGVzU2VjcmV0ID0gJ1NFQ1JFVF9ET19OT1RfUEFTU19USElTX09SX1lPVV9XSUxMX0JFX0ZJUkVEJztcblxubW9kdWxlLmV4cG9ydHMgPSBSZWFjdFByb3BUeXBlc1NlY3JldDtcbiIsIid1c2Ugc3RyaWN0JztcbnZhciBzdHJpY3RVcmlFbmNvZGUgPSByZXF1aXJlKCdzdHJpY3QtdXJpLWVuY29kZScpO1xudmFyIG9iamVjdEFzc2lnbiA9IHJlcXVpcmUoJ29iamVjdC1hc3NpZ24nKTtcbnZhciBkZWNvZGVDb21wb25lbnQgPSByZXF1aXJlKCdkZWNvZGUtdXJpLWNvbXBvbmVudCcpO1xuXG5mdW5jdGlvbiBlbmNvZGVyRm9yQXJyYXlGb3JtYXQob3B0cykge1xuXHRzd2l0Y2ggKG9wdHMuYXJyYXlGb3JtYXQpIHtcblx0XHRjYXNlICdpbmRleCc6XG5cdFx0XHRyZXR1cm4gZnVuY3Rpb24gKGtleSwgdmFsdWUsIGluZGV4KSB7XG5cdFx0XHRcdHJldHVybiB2YWx1ZSA9PT0gbnVsbCA/IFtcblx0XHRcdFx0XHRlbmNvZGUoa2V5LCBvcHRzKSxcblx0XHRcdFx0XHQnWycsXG5cdFx0XHRcdFx0aW5kZXgsXG5cdFx0XHRcdFx0J10nXG5cdFx0XHRcdF0uam9pbignJykgOiBbXG5cdFx0XHRcdFx0ZW5jb2RlKGtleSwgb3B0cyksXG5cdFx0XHRcdFx0J1snLFxuXHRcdFx0XHRcdGVuY29kZShpbmRleCwgb3B0cyksXG5cdFx0XHRcdFx0J109Jyxcblx0XHRcdFx0XHRlbmNvZGUodmFsdWUsIG9wdHMpXG5cdFx0XHRcdF0uam9pbignJyk7XG5cdFx0XHR9O1xuXG5cdFx0Y2FzZSAnYnJhY2tldCc6XG5cdFx0XHRyZXR1cm4gZnVuY3Rpb24gKGtleSwgdmFsdWUpIHtcblx0XHRcdFx0cmV0dXJuIHZhbHVlID09PSBudWxsID8gZW5jb2RlKGtleSwgb3B0cykgOiBbXG5cdFx0XHRcdFx0ZW5jb2RlKGtleSwgb3B0cyksXG5cdFx0XHRcdFx0J1tdPScsXG5cdFx0XHRcdFx0ZW5jb2RlKHZhbHVlLCBvcHRzKVxuXHRcdFx0XHRdLmpvaW4oJycpO1xuXHRcdFx0fTtcblxuXHRcdGRlZmF1bHQ6XG5cdFx0XHRyZXR1cm4gZnVuY3Rpb24gKGtleSwgdmFsdWUpIHtcblx0XHRcdFx0cmV0dXJuIHZhbHVlID09PSBudWxsID8gZW5jb2RlKGtleSwgb3B0cykgOiBbXG5cdFx0XHRcdFx0ZW5jb2RlKGtleSwgb3B0cyksXG5cdFx0XHRcdFx0Jz0nLFxuXHRcdFx0XHRcdGVuY29kZSh2YWx1ZSwgb3B0cylcblx0XHRcdFx0XS5qb2luKCcnKTtcblx0XHRcdH07XG5cdH1cbn1cblxuZnVuY3Rpb24gcGFyc2VyRm9yQXJyYXlGb3JtYXQob3B0cykge1xuXHR2YXIgcmVzdWx0O1xuXG5cdHN3aXRjaCAob3B0cy5hcnJheUZvcm1hdCkge1xuXHRcdGNhc2UgJ2luZGV4Jzpcblx0XHRcdHJldHVybiBmdW5jdGlvbiAoa2V5LCB2YWx1ZSwgYWNjdW11bGF0b3IpIHtcblx0XHRcdFx0cmVzdWx0ID0gL1xcWyhcXGQqKVxcXSQvLmV4ZWMoa2V5KTtcblxuXHRcdFx0XHRrZXkgPSBrZXkucmVwbGFjZSgvXFxbXFxkKlxcXSQvLCAnJyk7XG5cblx0XHRcdFx0aWYgKCFyZXN1bHQpIHtcblx0XHRcdFx0XHRhY2N1bXVsYXRvcltrZXldID0gdmFsdWU7XG5cdFx0XHRcdFx0cmV0dXJuO1xuXHRcdFx0XHR9XG5cblx0XHRcdFx0aWYgKGFjY3VtdWxhdG9yW2tleV0gPT09IHVuZGVmaW5lZCkge1xuXHRcdFx0XHRcdGFjY3VtdWxhdG9yW2tleV0gPSB7fTtcblx0XHRcdFx0fVxuXG5cdFx0XHRcdGFjY3VtdWxhdG9yW2tleV1bcmVzdWx0WzFdXSA9IHZhbHVlO1xuXHRcdFx0fTtcblxuXHRcdGNhc2UgJ2JyYWNrZXQnOlxuXHRcdFx0cmV0dXJuIGZ1bmN0aW9uIChrZXksIHZhbHVlLCBhY2N1bXVsYXRvcikge1xuXHRcdFx0XHRyZXN1bHQgPSAvKFxcW1xcXSkkLy5leGVjKGtleSk7XG5cdFx0XHRcdGtleSA9IGtleS5yZXBsYWNlKC9cXFtcXF0kLywgJycpO1xuXG5cdFx0XHRcdGlmICghcmVzdWx0KSB7XG5cdFx0XHRcdFx0YWNjdW11bGF0b3Jba2V5XSA9IHZhbHVlO1xuXHRcdFx0XHRcdHJldHVybjtcblx0XHRcdFx0fSBlbHNlIGlmIChhY2N1bXVsYXRvcltrZXldID09PSB1bmRlZmluZWQpIHtcblx0XHRcdFx0XHRhY2N1bXVsYXRvcltrZXldID0gW3ZhbHVlXTtcblx0XHRcdFx0XHRyZXR1cm47XG5cdFx0XHRcdH1cblxuXHRcdFx0XHRhY2N1bXVsYXRvcltrZXldID0gW10uY29uY2F0KGFjY3VtdWxhdG9yW2tleV0sIHZhbHVlKTtcblx0XHRcdH07XG5cblx0XHRkZWZhdWx0OlxuXHRcdFx0cmV0dXJuIGZ1bmN0aW9uIChrZXksIHZhbHVlLCBhY2N1bXVsYXRvcikge1xuXHRcdFx0XHRpZiAoYWNjdW11bGF0b3Jba2V5XSA9PT0gdW5kZWZpbmVkKSB7XG5cdFx0XHRcdFx0YWNjdW11bGF0b3Jba2V5XSA9IHZhbHVlO1xuXHRcdFx0XHRcdHJldHVybjtcblx0XHRcdFx0fVxuXG5cdFx0XHRcdGFjY3VtdWxhdG9yW2tleV0gPSBbXS5jb25jYXQoYWNjdW11bGF0b3Jba2V5XSwgdmFsdWUpO1xuXHRcdFx0fTtcblx0fVxufVxuXG5mdW5jdGlvbiBlbmNvZGUodmFsdWUsIG9wdHMpIHtcblx0aWYgKG9wdHMuZW5jb2RlKSB7XG5cdFx0cmV0dXJuIG9wdHMuc3RyaWN0ID8gc3RyaWN0VXJpRW5jb2RlKHZhbHVlKSA6IGVuY29kZVVSSUNvbXBvbmVudCh2YWx1ZSk7XG5cdH1cblxuXHRyZXR1cm4gdmFsdWU7XG59XG5cbmZ1bmN0aW9uIGtleXNTb3J0ZXIoaW5wdXQpIHtcblx0aWYgKEFycmF5LmlzQXJyYXkoaW5wdXQpKSB7XG5cdFx0cmV0dXJuIGlucHV0LnNvcnQoKTtcblx0fSBlbHNlIGlmICh0eXBlb2YgaW5wdXQgPT09ICdvYmplY3QnKSB7XG5cdFx0cmV0dXJuIGtleXNTb3J0ZXIoT2JqZWN0LmtleXMoaW5wdXQpKS5zb3J0KGZ1bmN0aW9uIChhLCBiKSB7XG5cdFx0XHRyZXR1cm4gTnVtYmVyKGEpIC0gTnVtYmVyKGIpO1xuXHRcdH0pLm1hcChmdW5jdGlvbiAoa2V5KSB7XG5cdFx0XHRyZXR1cm4gaW5wdXRba2V5XTtcblx0XHR9KTtcblx0fVxuXG5cdHJldHVybiBpbnB1dDtcbn1cblxuZnVuY3Rpb24gZXh0cmFjdChzdHIpIHtcblx0dmFyIHF1ZXJ5U3RhcnQgPSBzdHIuaW5kZXhPZignPycpO1xuXHRpZiAocXVlcnlTdGFydCA9PT0gLTEpIHtcblx0XHRyZXR1cm4gJyc7XG5cdH1cblx0cmV0dXJuIHN0ci5zbGljZShxdWVyeVN0YXJ0ICsgMSk7XG59XG5cbmZ1bmN0aW9uIHBhcnNlKHN0ciwgb3B0cykge1xuXHRvcHRzID0gb2JqZWN0QXNzaWduKHthcnJheUZvcm1hdDogJ25vbmUnfSwgb3B0cyk7XG5cblx0dmFyIGZvcm1hdHRlciA9IHBhcnNlckZvckFycmF5Rm9ybWF0KG9wdHMpO1xuXG5cdC8vIENyZWF0ZSBhbiBvYmplY3Qgd2l0aCBubyBwcm90b3R5cGVcblx0Ly8gaHR0cHM6Ly9naXRodWIuY29tL3NpbmRyZXNvcmh1cy9xdWVyeS1zdHJpbmcvaXNzdWVzLzQ3XG5cdHZhciByZXQgPSBPYmplY3QuY3JlYXRlKG51bGwpO1xuXG5cdGlmICh0eXBlb2Ygc3RyICE9PSAnc3RyaW5nJykge1xuXHRcdHJldHVybiByZXQ7XG5cdH1cblxuXHRzdHIgPSBzdHIudHJpbSgpLnJlcGxhY2UoL15bPyMmXS8sICcnKTtcblxuXHRpZiAoIXN0cikge1xuXHRcdHJldHVybiByZXQ7XG5cdH1cblxuXHRzdHIuc3BsaXQoJyYnKS5mb3JFYWNoKGZ1bmN0aW9uIChwYXJhbSkge1xuXHRcdHZhciBwYXJ0cyA9IHBhcmFtLnJlcGxhY2UoL1xcKy9nLCAnICcpLnNwbGl0KCc9Jyk7XG5cdFx0Ly8gRmlyZWZveCAocHJlIDQwKSBkZWNvZGVzIGAlM0RgIHRvIGA9YFxuXHRcdC8vIGh0dHBzOi8vZ2l0aHViLmNvbS9zaW5kcmVzb3JodXMvcXVlcnktc3RyaW5nL3B1bGwvMzdcblx0XHR2YXIga2V5ID0gcGFydHMuc2hpZnQoKTtcblx0XHR2YXIgdmFsID0gcGFydHMubGVuZ3RoID4gMCA/IHBhcnRzLmpvaW4oJz0nKSA6IHVuZGVmaW5lZDtcblxuXHRcdC8vIG1pc3NpbmcgYD1gIHNob3VsZCBiZSBgbnVsbGA6XG5cdFx0Ly8gaHR0cDovL3czLm9yZy9UUi8yMDEyL1dELXVybC0yMDEyMDUyNC8jY29sbGVjdC11cmwtcGFyYW1ldGVyc1xuXHRcdHZhbCA9IHZhbCA9PT0gdW5kZWZpbmVkID8gbnVsbCA6IGRlY29kZUNvbXBvbmVudCh2YWwpO1xuXG5cdFx0Zm9ybWF0dGVyKGRlY29kZUNvbXBvbmVudChrZXkpLCB2YWwsIHJldCk7XG5cdH0pO1xuXG5cdHJldHVybiBPYmplY3Qua2V5cyhyZXQpLnNvcnQoKS5yZWR1Y2UoZnVuY3Rpb24gKHJlc3VsdCwga2V5KSB7XG5cdFx0dmFyIHZhbCA9IHJldFtrZXldO1xuXHRcdGlmIChCb29sZWFuKHZhbCkgJiYgdHlwZW9mIHZhbCA9PT0gJ29iamVjdCcgJiYgIUFycmF5LmlzQXJyYXkodmFsKSkge1xuXHRcdFx0Ly8gU29ydCBvYmplY3Qga2V5cywgbm90IHZhbHVlc1xuXHRcdFx0cmVzdWx0W2tleV0gPSBrZXlzU29ydGVyKHZhbCk7XG5cdFx0fSBlbHNlIHtcblx0XHRcdHJlc3VsdFtrZXldID0gdmFsO1xuXHRcdH1cblxuXHRcdHJldHVybiByZXN1bHQ7XG5cdH0sIE9iamVjdC5jcmVhdGUobnVsbCkpO1xufVxuXG5leHBvcnRzLmV4dHJhY3QgPSBleHRyYWN0O1xuZXhwb3J0cy5wYXJzZSA9IHBhcnNlO1xuXG5leHBvcnRzLnN0cmluZ2lmeSA9IGZ1bmN0aW9uIChvYmosIG9wdHMpIHtcblx0dmFyIGRlZmF1bHRzID0ge1xuXHRcdGVuY29kZTogdHJ1ZSxcblx0XHRzdHJpY3Q6IHRydWUsXG5cdFx0YXJyYXlGb3JtYXQ6ICdub25lJ1xuXHR9O1xuXG5cdG9wdHMgPSBvYmplY3RBc3NpZ24oZGVmYXVsdHMsIG9wdHMpO1xuXG5cdGlmIChvcHRzLnNvcnQgPT09IGZhbHNlKSB7XG5cdFx0b3B0cy5zb3J0ID0gZnVuY3Rpb24gKCkge307XG5cdH1cblxuXHR2YXIgZm9ybWF0dGVyID0gZW5jb2RlckZvckFycmF5Rm9ybWF0KG9wdHMpO1xuXG5cdHJldHVybiBvYmogPyBPYmplY3Qua2V5cyhvYmopLnNvcnQob3B0cy5zb3J0KS5tYXAoZnVuY3Rpb24gKGtleSkge1xuXHRcdHZhciB2YWwgPSBvYmpba2V5XTtcblxuXHRcdGlmICh2YWwgPT09IHVuZGVmaW5lZCkge1xuXHRcdFx0cmV0dXJuICcnO1xuXHRcdH1cblxuXHRcdGlmICh2YWwgPT09IG51bGwpIHtcblx0XHRcdHJldHVybiBlbmNvZGUoa2V5LCBvcHRzKTtcblx0XHR9XG5cblx0XHRpZiAoQXJyYXkuaXNBcnJheSh2YWwpKSB7XG5cdFx0XHR2YXIgcmVzdWx0ID0gW107XG5cblx0XHRcdHZhbC5zbGljZSgpLmZvckVhY2goZnVuY3Rpb24gKHZhbDIpIHtcblx0XHRcdFx0aWYgKHZhbDIgPT09IHVuZGVmaW5lZCkge1xuXHRcdFx0XHRcdHJldHVybjtcblx0XHRcdFx0fVxuXG5cdFx0XHRcdHJlc3VsdC5wdXNoKGZvcm1hdHRlcihrZXksIHZhbDIsIHJlc3VsdC5sZW5ndGgpKTtcblx0XHRcdH0pO1xuXG5cdFx0XHRyZXR1cm4gcmVzdWx0LmpvaW4oJyYnKTtcblx0XHR9XG5cblx0XHRyZXR1cm4gZW5jb2RlKGtleSwgb3B0cykgKyAnPScgKyBlbmNvZGUodmFsLCBvcHRzKTtcblx0fSkuZmlsdGVyKGZ1bmN0aW9uICh4KSB7XG5cdFx0cmV0dXJuIHgubGVuZ3RoID4gMDtcblx0fSkuam9pbignJicpIDogJyc7XG59O1xuXG5leHBvcnRzLnBhcnNlVXJsID0gZnVuY3Rpb24gKHN0ciwgb3B0cykge1xuXHRyZXR1cm4ge1xuXHRcdHVybDogc3RyLnNwbGl0KCc/JylbMF0gfHwgJycsXG5cdFx0cXVlcnk6IHBhcnNlKGV4dHJhY3Qoc3RyKSwgb3B0cylcblx0fTtcbn07XG4iLCIndXNlIHN0cmljdCc7XG5cbnZhciBhc3NpZ24gPSByZXF1aXJlKCdvYmplY3QtYXNzaWduJyksXG5cdFByb3BUeXBlcyA9IHJlcXVpcmUoJ3Byb3AtdHlwZXMnKSxcblx0Y3JlYXRlQ2xhc3MgPSByZXF1aXJlKCdjcmVhdGUtcmVhY3QtY2xhc3MnKSxcblx0bW9tZW50ID0gcmVxdWlyZSgnbW9tZW50JyksXG5cdFJlYWN0ID0gcmVxdWlyZSgncmVhY3QnKSxcblx0Q2FsZW5kYXJDb250YWluZXIgPSByZXF1aXJlKCcuL3NyYy9DYWxlbmRhckNvbnRhaW5lcicpXG5cdDtcblxudmFyIHZpZXdNb2RlcyA9IE9iamVjdC5mcmVlemUoe1xuXHRZRUFSUzogJ3llYXJzJyxcblx0TU9OVEhTOiAnbW9udGhzJyxcblx0REFZUzogJ2RheXMnLFxuXHRUSU1FOiAndGltZScsXG59KTtcblxudmFyIFRZUEVTID0gUHJvcFR5cGVzO1xudmFyIERhdGV0aW1lID0gY3JlYXRlQ2xhc3Moe1xuXHRwcm9wVHlwZXM6IHtcblx0XHQvLyB2YWx1ZTogVFlQRVMub2JqZWN0IHwgVFlQRVMuc3RyaW5nLFxuXHRcdC8vIGRlZmF1bHRWYWx1ZTogVFlQRVMub2JqZWN0IHwgVFlQRVMuc3RyaW5nLFxuXHRcdC8vIHZpZXdEYXRlOiBUWVBFUy5vYmplY3QgfCBUWVBFUy5zdHJpbmcsXG5cdFx0b25Gb2N1czogVFlQRVMuZnVuYyxcblx0XHRvbkJsdXI6IFRZUEVTLmZ1bmMsXG5cdFx0b25DaGFuZ2U6IFRZUEVTLmZ1bmMsXG5cdFx0b25WaWV3TW9kZUNoYW5nZTogVFlQRVMuZnVuYyxcblx0XHRsb2NhbGU6IFRZUEVTLnN0cmluZyxcblx0XHR1dGM6IFRZUEVTLmJvb2wsXG5cdFx0aW5wdXQ6IFRZUEVTLmJvb2wsXG5cdFx0Ly8gZGF0ZUZvcm1hdDogVFlQRVMuc3RyaW5nIHwgVFlQRVMuYm9vbCxcblx0XHQvLyB0aW1lRm9ybWF0OiBUWVBFUy5zdHJpbmcgfCBUWVBFUy5ib29sLFxuXHRcdGlucHV0UHJvcHM6IFRZUEVTLm9iamVjdCxcblx0XHR0aW1lQ29uc3RyYWludHM6IFRZUEVTLm9iamVjdCxcblx0XHR2aWV3TW9kZTogVFlQRVMub25lT2YoW3ZpZXdNb2Rlcy5ZRUFSUywgdmlld01vZGVzLk1PTlRIUywgdmlld01vZGVzLkRBWVMsIHZpZXdNb2Rlcy5USU1FXSksXG5cdFx0aXNWYWxpZERhdGU6IFRZUEVTLmZ1bmMsXG5cdFx0b3BlbjogVFlQRVMuYm9vbCxcblx0XHRzdHJpY3RQYXJzaW5nOiBUWVBFUy5ib29sLFxuXHRcdGNsb3NlT25TZWxlY3Q6IFRZUEVTLmJvb2wsXG5cdFx0Y2xvc2VPblRhYjogVFlQRVMuYm9vbFxuXHR9LFxuXG5cdGdldEluaXRpYWxTdGF0ZTogZnVuY3Rpb24oKSB7XG5cdFx0dmFyIHN0YXRlID0gdGhpcy5nZXRTdGF0ZUZyb21Qcm9wcyggdGhpcy5wcm9wcyApO1xuXG5cdFx0aWYgKCBzdGF0ZS5vcGVuID09PSB1bmRlZmluZWQgKVxuXHRcdFx0c3RhdGUub3BlbiA9ICF0aGlzLnByb3BzLmlucHV0O1xuXG5cdFx0c3RhdGUuY3VycmVudFZpZXcgPSB0aGlzLnByb3BzLmRhdGVGb3JtYXQgP1xuXHRcdFx0KHRoaXMucHJvcHMudmlld01vZGUgfHwgc3RhdGUudXBkYXRlT24gfHwgdmlld01vZGVzLkRBWVMpIDogdmlld01vZGVzLlRJTUU7XG5cblx0XHRyZXR1cm4gc3RhdGU7XG5cdH0sXG5cblx0cGFyc2VEYXRlOiBmdW5jdGlvbiAoZGF0ZSwgZm9ybWF0cykge1xuXHRcdHZhciBwYXJzZWREYXRlO1xuXG5cdFx0aWYgKGRhdGUgJiYgdHlwZW9mIGRhdGUgPT09ICdzdHJpbmcnKVxuXHRcdFx0cGFyc2VkRGF0ZSA9IHRoaXMubG9jYWxNb21lbnQoZGF0ZSwgZm9ybWF0cy5kYXRldGltZSk7XG5cdFx0ZWxzZSBpZiAoZGF0ZSlcblx0XHRcdHBhcnNlZERhdGUgPSB0aGlzLmxvY2FsTW9tZW50KGRhdGUpO1xuXG5cdFx0aWYgKHBhcnNlZERhdGUgJiYgIXBhcnNlZERhdGUuaXNWYWxpZCgpKVxuXHRcdFx0cGFyc2VkRGF0ZSA9IG51bGw7XG5cblx0XHRyZXR1cm4gcGFyc2VkRGF0ZTtcblx0fSxcblxuXHRnZXRTdGF0ZUZyb21Qcm9wczogZnVuY3Rpb24oIHByb3BzICkge1xuXHRcdHZhciBmb3JtYXRzID0gdGhpcy5nZXRGb3JtYXRzKCBwcm9wcyApLFxuXHRcdFx0ZGF0ZSA9IHByb3BzLnZhbHVlIHx8IHByb3BzLmRlZmF1bHRWYWx1ZSxcblx0XHRcdHNlbGVjdGVkRGF0ZSwgdmlld0RhdGUsIHVwZGF0ZU9uLCBpbnB1dFZhbHVlXG5cdFx0XHQ7XG5cblx0XHRzZWxlY3RlZERhdGUgPSB0aGlzLnBhcnNlRGF0ZShkYXRlLCBmb3JtYXRzKTtcblxuXHRcdHZpZXdEYXRlID0gdGhpcy5wYXJzZURhdGUocHJvcHMudmlld0RhdGUsIGZvcm1hdHMpO1xuXG5cdFx0dmlld0RhdGUgPSBzZWxlY3RlZERhdGUgP1xuXHRcdFx0c2VsZWN0ZWREYXRlLmNsb25lKCkuc3RhcnRPZignbW9udGgnKSA6XG5cdFx0XHR2aWV3RGF0ZSA/IHZpZXdEYXRlLmNsb25lKCkuc3RhcnRPZignbW9udGgnKSA6IHRoaXMubG9jYWxNb21lbnQoKS5zdGFydE9mKCdtb250aCcpO1xuXG5cdFx0dXBkYXRlT24gPSB0aGlzLmdldFVwZGF0ZU9uKGZvcm1hdHMpO1xuXG5cdFx0aWYgKCBzZWxlY3RlZERhdGUgKVxuXHRcdFx0aW5wdXRWYWx1ZSA9IHNlbGVjdGVkRGF0ZS5mb3JtYXQoZm9ybWF0cy5kYXRldGltZSk7XG5cdFx0ZWxzZSBpZiAoIGRhdGUuaXNWYWxpZCAmJiAhZGF0ZS5pc1ZhbGlkKCkgKVxuXHRcdFx0aW5wdXRWYWx1ZSA9ICcnO1xuXHRcdGVsc2Vcblx0XHRcdGlucHV0VmFsdWUgPSBkYXRlIHx8ICcnO1xuXG5cdFx0cmV0dXJuIHtcblx0XHRcdHVwZGF0ZU9uOiB1cGRhdGVPbixcblx0XHRcdGlucHV0Rm9ybWF0OiBmb3JtYXRzLmRhdGV0aW1lLFxuXHRcdFx0dmlld0RhdGU6IHZpZXdEYXRlLFxuXHRcdFx0c2VsZWN0ZWREYXRlOiBzZWxlY3RlZERhdGUsXG5cdFx0XHRpbnB1dFZhbHVlOiBpbnB1dFZhbHVlLFxuXHRcdFx0b3BlbjogcHJvcHMub3BlblxuXHRcdH07XG5cdH0sXG5cblx0Z2V0VXBkYXRlT246IGZ1bmN0aW9uKCBmb3JtYXRzICkge1xuXHRcdGlmICggZm9ybWF0cy5kYXRlLm1hdGNoKC9bbExEXS8pICkge1xuXHRcdFx0cmV0dXJuIHZpZXdNb2Rlcy5EQVlTO1xuXHRcdH0gZWxzZSBpZiAoIGZvcm1hdHMuZGF0ZS5pbmRleE9mKCdNJykgIT09IC0xICkge1xuXHRcdFx0cmV0dXJuIHZpZXdNb2Rlcy5NT05USFM7XG5cdFx0fSBlbHNlIGlmICggZm9ybWF0cy5kYXRlLmluZGV4T2YoJ1knKSAhPT0gLTEgKSB7XG5cdFx0XHRyZXR1cm4gdmlld01vZGVzLllFQVJTO1xuXHRcdH1cblxuXHRcdHJldHVybiB2aWV3TW9kZXMuREFZUztcblx0fSxcblxuXHRnZXRGb3JtYXRzOiBmdW5jdGlvbiggcHJvcHMgKSB7XG5cdFx0dmFyIGZvcm1hdHMgPSB7XG5cdFx0XHRcdGRhdGU6IHByb3BzLmRhdGVGb3JtYXQgfHwgJycsXG5cdFx0XHRcdHRpbWU6IHByb3BzLnRpbWVGb3JtYXQgfHwgJydcblx0XHRcdH0sXG5cdFx0XHRsb2NhbGUgPSB0aGlzLmxvY2FsTW9tZW50KCBwcm9wcy5kYXRlLCBudWxsLCBwcm9wcyApLmxvY2FsZURhdGEoKVxuXHRcdFx0O1xuXG5cdFx0aWYgKCBmb3JtYXRzLmRhdGUgPT09IHRydWUgKSB7XG5cdFx0XHRmb3JtYXRzLmRhdGUgPSBsb2NhbGUubG9uZ0RhdGVGb3JtYXQoJ0wnKTtcblx0XHR9XG5cdFx0ZWxzZSBpZiAoIHRoaXMuZ2V0VXBkYXRlT24oZm9ybWF0cykgIT09IHZpZXdNb2Rlcy5EQVlTICkge1xuXHRcdFx0Zm9ybWF0cy50aW1lID0gJyc7XG5cdFx0fVxuXG5cdFx0aWYgKCBmb3JtYXRzLnRpbWUgPT09IHRydWUgKSB7XG5cdFx0XHRmb3JtYXRzLnRpbWUgPSBsb2NhbGUubG9uZ0RhdGVGb3JtYXQoJ0xUJyk7XG5cdFx0fVxuXG5cdFx0Zm9ybWF0cy5kYXRldGltZSA9IGZvcm1hdHMuZGF0ZSAmJiBmb3JtYXRzLnRpbWUgP1xuXHRcdFx0Zm9ybWF0cy5kYXRlICsgJyAnICsgZm9ybWF0cy50aW1lIDpcblx0XHRcdGZvcm1hdHMuZGF0ZSB8fCBmb3JtYXRzLnRpbWVcblx0XHQ7XG5cblx0XHRyZXR1cm4gZm9ybWF0cztcblx0fSxcblxuXHRjb21wb25lbnRXaWxsUmVjZWl2ZVByb3BzOiBmdW5jdGlvbiggbmV4dFByb3BzICkge1xuXHRcdHZhciBmb3JtYXRzID0gdGhpcy5nZXRGb3JtYXRzKCBuZXh0UHJvcHMgKSxcblx0XHRcdHVwZGF0ZWRTdGF0ZSA9IHt9XG5cdFx0O1xuXG5cdFx0aWYgKCBuZXh0UHJvcHMudmFsdWUgIT09IHRoaXMucHJvcHMudmFsdWUgfHxcblx0XHRcdGZvcm1hdHMuZGF0ZXRpbWUgIT09IHRoaXMuZ2V0Rm9ybWF0cyggdGhpcy5wcm9wcyApLmRhdGV0aW1lICkge1xuXHRcdFx0dXBkYXRlZFN0YXRlID0gdGhpcy5nZXRTdGF0ZUZyb21Qcm9wcyggbmV4dFByb3BzICk7XG5cdFx0fVxuXG5cdFx0aWYgKCB1cGRhdGVkU3RhdGUub3BlbiA9PT0gdW5kZWZpbmVkICkge1xuXHRcdFx0aWYgKCB0eXBlb2YgbmV4dFByb3BzLm9wZW4gIT09ICd1bmRlZmluZWQnICkge1xuXHRcdFx0XHR1cGRhdGVkU3RhdGUub3BlbiA9IG5leHRQcm9wcy5vcGVuO1xuXHRcdFx0fSBlbHNlIGlmICggdGhpcy5wcm9wcy5jbG9zZU9uU2VsZWN0ICYmIHRoaXMuc3RhdGUuY3VycmVudFZpZXcgIT09IHZpZXdNb2Rlcy5USU1FICkge1xuXHRcdFx0XHR1cGRhdGVkU3RhdGUub3BlbiA9IGZhbHNlO1xuXHRcdFx0fSBlbHNlIHtcblx0XHRcdFx0dXBkYXRlZFN0YXRlLm9wZW4gPSB0aGlzLnN0YXRlLm9wZW47XG5cdFx0XHR9XG5cdFx0fVxuXG5cdFx0aWYgKCBuZXh0UHJvcHMudmlld01vZGUgIT09IHRoaXMucHJvcHMudmlld01vZGUgKSB7XG5cdFx0XHR1cGRhdGVkU3RhdGUuY3VycmVudFZpZXcgPSBuZXh0UHJvcHMudmlld01vZGU7XG5cdFx0fVxuXG5cdFx0aWYgKCBuZXh0UHJvcHMubG9jYWxlICE9PSB0aGlzLnByb3BzLmxvY2FsZSApIHtcblx0XHRcdGlmICggdGhpcy5zdGF0ZS52aWV3RGF0ZSApIHtcblx0XHRcdFx0dmFyIHVwZGF0ZWRWaWV3RGF0ZSA9IHRoaXMuc3RhdGUudmlld0RhdGUuY2xvbmUoKS5sb2NhbGUoIG5leHRQcm9wcy5sb2NhbGUgKTtcblx0XHRcdFx0dXBkYXRlZFN0YXRlLnZpZXdEYXRlID0gdXBkYXRlZFZpZXdEYXRlO1xuXHRcdFx0fVxuXHRcdFx0aWYgKCB0aGlzLnN0YXRlLnNlbGVjdGVkRGF0ZSApIHtcblx0XHRcdFx0dmFyIHVwZGF0ZWRTZWxlY3RlZERhdGUgPSB0aGlzLnN0YXRlLnNlbGVjdGVkRGF0ZS5jbG9uZSgpLmxvY2FsZSggbmV4dFByb3BzLmxvY2FsZSApO1xuXHRcdFx0XHR1cGRhdGVkU3RhdGUuc2VsZWN0ZWREYXRlID0gdXBkYXRlZFNlbGVjdGVkRGF0ZTtcblx0XHRcdFx0dXBkYXRlZFN0YXRlLmlucHV0VmFsdWUgPSB1cGRhdGVkU2VsZWN0ZWREYXRlLmZvcm1hdCggZm9ybWF0cy5kYXRldGltZSApO1xuXHRcdFx0fVxuXHRcdH1cblxuXHRcdGlmICggbmV4dFByb3BzLnV0YyAhPT0gdGhpcy5wcm9wcy51dGMgKSB7XG5cdFx0XHRpZiAoIG5leHRQcm9wcy51dGMgKSB7XG5cdFx0XHRcdGlmICggdGhpcy5zdGF0ZS52aWV3RGF0ZSApXG5cdFx0XHRcdFx0dXBkYXRlZFN0YXRlLnZpZXdEYXRlID0gdGhpcy5zdGF0ZS52aWV3RGF0ZS5jbG9uZSgpLnV0YygpO1xuXHRcdFx0XHRpZiAoIHRoaXMuc3RhdGUuc2VsZWN0ZWREYXRlICkge1xuXHRcdFx0XHRcdHVwZGF0ZWRTdGF0ZS5zZWxlY3RlZERhdGUgPSB0aGlzLnN0YXRlLnNlbGVjdGVkRGF0ZS5jbG9uZSgpLnV0YygpO1xuXHRcdFx0XHRcdHVwZGF0ZWRTdGF0ZS5pbnB1dFZhbHVlID0gdXBkYXRlZFN0YXRlLnNlbGVjdGVkRGF0ZS5mb3JtYXQoIGZvcm1hdHMuZGF0ZXRpbWUgKTtcblx0XHRcdFx0fVxuXHRcdFx0fSBlbHNlIHtcblx0XHRcdFx0aWYgKCB0aGlzLnN0YXRlLnZpZXdEYXRlIClcblx0XHRcdFx0XHR1cGRhdGVkU3RhdGUudmlld0RhdGUgPSB0aGlzLnN0YXRlLnZpZXdEYXRlLmNsb25lKCkubG9jYWwoKTtcblx0XHRcdFx0aWYgKCB0aGlzLnN0YXRlLnNlbGVjdGVkRGF0ZSApIHtcblx0XHRcdFx0XHR1cGRhdGVkU3RhdGUuc2VsZWN0ZWREYXRlID0gdGhpcy5zdGF0ZS5zZWxlY3RlZERhdGUuY2xvbmUoKS5sb2NhbCgpO1xuXHRcdFx0XHRcdHVwZGF0ZWRTdGF0ZS5pbnB1dFZhbHVlID0gdXBkYXRlZFN0YXRlLnNlbGVjdGVkRGF0ZS5mb3JtYXQoZm9ybWF0cy5kYXRldGltZSk7XG5cdFx0XHRcdH1cblx0XHRcdH1cblx0XHR9XG5cblx0XHRpZiAoIG5leHRQcm9wcy52aWV3RGF0ZSAhPT0gdGhpcy5wcm9wcy52aWV3RGF0ZSApIHtcblx0XHRcdHVwZGF0ZWRTdGF0ZS52aWV3RGF0ZSA9IG1vbWVudChuZXh0UHJvcHMudmlld0RhdGUpO1xuXHRcdH1cblx0XHQvL3dlIHNob3VsZCBvbmx5IHNob3cgYSB2YWxpZCBkYXRlIGlmIHdlIGFyZSBwcm92aWRlZCBhIGlzVmFsaWREYXRlIGZ1bmN0aW9uLiBSZW1vdmVkIGluIDIuMTAuM1xuXHRcdC8qaWYgKHRoaXMucHJvcHMuaXNWYWxpZERhdGUpIHtcblx0XHRcdHVwZGF0ZWRTdGF0ZS52aWV3RGF0ZSA9IHVwZGF0ZWRTdGF0ZS52aWV3RGF0ZSB8fCB0aGlzLnN0YXRlLnZpZXdEYXRlO1xuXHRcdFx0d2hpbGUgKCF0aGlzLnByb3BzLmlzVmFsaWREYXRlKHVwZGF0ZWRTdGF0ZS52aWV3RGF0ZSkpIHtcblx0XHRcdFx0dXBkYXRlZFN0YXRlLnZpZXdEYXRlID0gdXBkYXRlZFN0YXRlLnZpZXdEYXRlLmFkZCgxLCAnZGF5Jyk7XG5cdFx0XHR9XG5cdFx0fSovXG5cdFx0dGhpcy5zZXRTdGF0ZSggdXBkYXRlZFN0YXRlICk7XG5cdH0sXG5cblx0b25JbnB1dENoYW5nZTogZnVuY3Rpb24oIGUgKSB7XG5cdFx0dmFyIHZhbHVlID0gZS50YXJnZXQgPT09IG51bGwgPyBlIDogZS50YXJnZXQudmFsdWUsXG5cdFx0XHRsb2NhbE1vbWVudCA9IHRoaXMubG9jYWxNb21lbnQoIHZhbHVlLCB0aGlzLnN0YXRlLmlucHV0Rm9ybWF0ICksXG5cdFx0XHR1cGRhdGUgPSB7IGlucHV0VmFsdWU6IHZhbHVlIH1cblx0XHRcdDtcblxuXHRcdGlmICggbG9jYWxNb21lbnQuaXNWYWxpZCgpICYmICF0aGlzLnByb3BzLnZhbHVlICkge1xuXHRcdFx0dXBkYXRlLnNlbGVjdGVkRGF0ZSA9IGxvY2FsTW9tZW50O1xuXHRcdFx0dXBkYXRlLnZpZXdEYXRlID0gbG9jYWxNb21lbnQuY2xvbmUoKS5zdGFydE9mKCdtb250aCcpO1xuXHRcdH0gZWxzZSB7XG5cdFx0XHR1cGRhdGUuc2VsZWN0ZWREYXRlID0gbnVsbDtcblx0XHR9XG5cblx0XHRyZXR1cm4gdGhpcy5zZXRTdGF0ZSggdXBkYXRlLCBmdW5jdGlvbigpIHtcblx0XHRcdHJldHVybiB0aGlzLnByb3BzLm9uQ2hhbmdlKCBsb2NhbE1vbWVudC5pc1ZhbGlkKCkgPyBsb2NhbE1vbWVudCA6IHRoaXMuc3RhdGUuaW5wdXRWYWx1ZSApO1xuXHRcdH0pO1xuXHR9LFxuXG5cdG9uSW5wdXRLZXk6IGZ1bmN0aW9uKCBlICkge1xuXHRcdGlmICggZS53aGljaCA9PT0gOSAmJiB0aGlzLnByb3BzLmNsb3NlT25UYWIgKSB7XG5cdFx0XHR0aGlzLmNsb3NlQ2FsZW5kYXIoKTtcblx0XHR9XG5cdH0sXG5cblx0c2hvd1ZpZXc6IGZ1bmN0aW9uKCB2aWV3ICkge1xuXHRcdHZhciBtZSA9IHRoaXM7XG5cdFx0cmV0dXJuIGZ1bmN0aW9uKCkge1xuXHRcdFx0bWUuc3RhdGUuY3VycmVudFZpZXcgIT09IHZpZXcgJiYgbWUucHJvcHMub25WaWV3TW9kZUNoYW5nZSggdmlldyApO1xuXHRcdFx0bWUuc2V0U3RhdGUoeyBjdXJyZW50VmlldzogdmlldyB9KTtcblx0XHR9O1xuXHR9LFxuXG5cdHNldERhdGU6IGZ1bmN0aW9uKCB0eXBlICkge1xuXHRcdHZhciBtZSA9IHRoaXMsXG5cdFx0XHRuZXh0Vmlld3MgPSB7XG5cdFx0XHRcdG1vbnRoOiB2aWV3TW9kZXMuREFZUyxcblx0XHRcdFx0eWVhcjogdmlld01vZGVzLk1PTlRIUyxcblx0XHRcdH1cblx0XHQ7XG5cdFx0cmV0dXJuIGZ1bmN0aW9uKCBlICkge1xuXHRcdFx0bWUuc2V0U3RhdGUoe1xuXHRcdFx0XHR2aWV3RGF0ZTogbWUuc3RhdGUudmlld0RhdGUuY2xvbmUoKVsgdHlwZSBdKCBwYXJzZUludChlLnRhcmdldC5nZXRBdHRyaWJ1dGUoJ2RhdGEtdmFsdWUnKSwgMTApICkuc3RhcnRPZiggdHlwZSApLFxuXHRcdFx0XHRjdXJyZW50VmlldzogbmV4dFZpZXdzWyB0eXBlIF1cblx0XHRcdH0pO1xuXHRcdFx0bWUucHJvcHMub25WaWV3TW9kZUNoYW5nZSggbmV4dFZpZXdzWyB0eXBlIF0gKTtcblx0XHR9O1xuXHR9LFxuXG5cdGFkZFRpbWU6IGZ1bmN0aW9uKCBhbW91bnQsIHR5cGUsIHRvU2VsZWN0ZWQgKSB7XG5cdFx0cmV0dXJuIHRoaXMudXBkYXRlVGltZSggJ2FkZCcsIGFtb3VudCwgdHlwZSwgdG9TZWxlY3RlZCApO1xuXHR9LFxuXG5cdHN1YnRyYWN0VGltZTogZnVuY3Rpb24oIGFtb3VudCwgdHlwZSwgdG9TZWxlY3RlZCApIHtcblx0XHRyZXR1cm4gdGhpcy51cGRhdGVUaW1lKCAnc3VidHJhY3QnLCBhbW91bnQsIHR5cGUsIHRvU2VsZWN0ZWQgKTtcblx0fSxcblxuXHR1cGRhdGVUaW1lOiBmdW5jdGlvbiggb3AsIGFtb3VudCwgdHlwZSwgdG9TZWxlY3RlZCApIHtcblx0XHR2YXIgbWUgPSB0aGlzO1xuXG5cdFx0cmV0dXJuIGZ1bmN0aW9uKCkge1xuXHRcdFx0dmFyIHVwZGF0ZSA9IHt9LFxuXHRcdFx0XHRkYXRlID0gdG9TZWxlY3RlZCA/ICdzZWxlY3RlZERhdGUnIDogJ3ZpZXdEYXRlJ1xuXHRcdFx0O1xuXG5cdFx0XHR1cGRhdGVbIGRhdGUgXSA9IG1lLnN0YXRlWyBkYXRlIF0uY2xvbmUoKVsgb3AgXSggYW1vdW50LCB0eXBlICk7XG5cblx0XHRcdG1lLnNldFN0YXRlKCB1cGRhdGUgKTtcblx0XHR9O1xuXHR9LFxuXG5cdGFsbG93ZWRTZXRUaW1lOiBbJ2hvdXJzJywgJ21pbnV0ZXMnLCAnc2Vjb25kcycsICdtaWxsaXNlY29uZHMnXSxcblx0c2V0VGltZTogZnVuY3Rpb24oIHR5cGUsIHZhbHVlICkge1xuXHRcdHZhciBpbmRleCA9IHRoaXMuYWxsb3dlZFNldFRpbWUuaW5kZXhPZiggdHlwZSApICsgMSxcblx0XHRcdHN0YXRlID0gdGhpcy5zdGF0ZSxcblx0XHRcdGRhdGUgPSAoc3RhdGUuc2VsZWN0ZWREYXRlIHx8IHN0YXRlLnZpZXdEYXRlKS5jbG9uZSgpLFxuXHRcdFx0bmV4dFR5cGVcblx0XHRcdDtcblxuXHRcdC8vIEl0IGlzIG5lZWRlZCB0byBzZXQgYWxsIHRoZSB0aW1lIHByb3BlcnRpZXNcblx0XHQvLyB0byBub3QgdG8gcmVzZXQgdGhlIHRpbWVcblx0XHRkYXRlWyB0eXBlIF0oIHZhbHVlICk7XG5cdFx0Zm9yICg7IGluZGV4IDwgdGhpcy5hbGxvd2VkU2V0VGltZS5sZW5ndGg7IGluZGV4KyspIHtcblx0XHRcdG5leHRUeXBlID0gdGhpcy5hbGxvd2VkU2V0VGltZVtpbmRleF07XG5cdFx0XHRkYXRlWyBuZXh0VHlwZSBdKCBkYXRlW25leHRUeXBlXSgpICk7XG5cdFx0fVxuXG5cdFx0aWYgKCAhdGhpcy5wcm9wcy52YWx1ZSApIHtcblx0XHRcdHRoaXMuc2V0U3RhdGUoe1xuXHRcdFx0XHRzZWxlY3RlZERhdGU6IGRhdGUsXG5cdFx0XHRcdGlucHV0VmFsdWU6IGRhdGUuZm9ybWF0KCBzdGF0ZS5pbnB1dEZvcm1hdCApXG5cdFx0XHR9KTtcblx0XHR9XG5cdFx0dGhpcy5wcm9wcy5vbkNoYW5nZSggZGF0ZSApO1xuXHR9LFxuXG5cdHVwZGF0ZVNlbGVjdGVkRGF0ZTogZnVuY3Rpb24oIGUsIGNsb3NlICkge1xuXHRcdHZhciB0YXJnZXQgPSBlLnRhcmdldCxcblx0XHRcdG1vZGlmaWVyID0gMCxcblx0XHRcdHZpZXdEYXRlID0gdGhpcy5zdGF0ZS52aWV3RGF0ZSxcblx0XHRcdGN1cnJlbnREYXRlID0gdGhpcy5zdGF0ZS5zZWxlY3RlZERhdGUgfHwgdmlld0RhdGUsXG5cdFx0XHRkYXRlXG5cdFx0XHQ7XG5cblx0XHRpZiAodGFyZ2V0LmNsYXNzTmFtZS5pbmRleE9mKCdyZHREYXknKSAhPT0gLTEpIHtcblx0XHRcdGlmICh0YXJnZXQuY2xhc3NOYW1lLmluZGV4T2YoJ3JkdE5ldycpICE9PSAtMSlcblx0XHRcdFx0bW9kaWZpZXIgPSAxO1xuXHRcdFx0ZWxzZSBpZiAodGFyZ2V0LmNsYXNzTmFtZS5pbmRleE9mKCdyZHRPbGQnKSAhPT0gLTEpXG5cdFx0XHRcdG1vZGlmaWVyID0gLTE7XG5cblx0XHRcdGRhdGUgPSB2aWV3RGF0ZS5jbG9uZSgpXG5cdFx0XHRcdC5tb250aCggdmlld0RhdGUubW9udGgoKSArIG1vZGlmaWVyIClcblx0XHRcdFx0LmRhdGUoIHBhcnNlSW50KCB0YXJnZXQuZ2V0QXR0cmlidXRlKCdkYXRhLXZhbHVlJyksIDEwICkgKTtcblx0XHR9IGVsc2UgaWYgKHRhcmdldC5jbGFzc05hbWUuaW5kZXhPZigncmR0TW9udGgnKSAhPT0gLTEpIHtcblx0XHRcdGRhdGUgPSB2aWV3RGF0ZS5jbG9uZSgpXG5cdFx0XHRcdC5tb250aCggcGFyc2VJbnQoIHRhcmdldC5nZXRBdHRyaWJ1dGUoJ2RhdGEtdmFsdWUnKSwgMTAgKSApXG5cdFx0XHRcdC5kYXRlKCBjdXJyZW50RGF0ZS5kYXRlKCkgKTtcblx0XHR9IGVsc2UgaWYgKHRhcmdldC5jbGFzc05hbWUuaW5kZXhPZigncmR0WWVhcicpICE9PSAtMSkge1xuXHRcdFx0ZGF0ZSA9IHZpZXdEYXRlLmNsb25lKClcblx0XHRcdFx0Lm1vbnRoKCBjdXJyZW50RGF0ZS5tb250aCgpIClcblx0XHRcdFx0LmRhdGUoIGN1cnJlbnREYXRlLmRhdGUoKSApXG5cdFx0XHRcdC55ZWFyKCBwYXJzZUludCggdGFyZ2V0LmdldEF0dHJpYnV0ZSgnZGF0YS12YWx1ZScpLCAxMCApICk7XG5cdFx0fVxuXG5cdFx0ZGF0ZS5ob3VycyggY3VycmVudERhdGUuaG91cnMoKSApXG5cdFx0XHQubWludXRlcyggY3VycmVudERhdGUubWludXRlcygpIClcblx0XHRcdC5zZWNvbmRzKCBjdXJyZW50RGF0ZS5zZWNvbmRzKCkgKVxuXHRcdFx0Lm1pbGxpc2Vjb25kcyggY3VycmVudERhdGUubWlsbGlzZWNvbmRzKCkgKTtcblxuXHRcdGlmICggIXRoaXMucHJvcHMudmFsdWUgKSB7XG5cdFx0XHR2YXIgb3BlbiA9ICEoIHRoaXMucHJvcHMuY2xvc2VPblNlbGVjdCAmJiBjbG9zZSApO1xuXHRcdFx0aWYgKCAhb3BlbiApIHtcblx0XHRcdFx0dGhpcy5wcm9wcy5vbkJsdXIoIGRhdGUgKTtcblx0XHRcdH1cblxuXHRcdFx0dGhpcy5zZXRTdGF0ZSh7XG5cdFx0XHRcdHNlbGVjdGVkRGF0ZTogZGF0ZSxcblx0XHRcdFx0dmlld0RhdGU6IGRhdGUuY2xvbmUoKS5zdGFydE9mKCdtb250aCcpLFxuXHRcdFx0XHRpbnB1dFZhbHVlOiBkYXRlLmZvcm1hdCggdGhpcy5zdGF0ZS5pbnB1dEZvcm1hdCApLFxuXHRcdFx0XHRvcGVuOiBvcGVuXG5cdFx0XHR9KTtcblx0XHR9IGVsc2Uge1xuXHRcdFx0aWYgKCB0aGlzLnByb3BzLmNsb3NlT25TZWxlY3QgJiYgY2xvc2UgKSB7XG5cdFx0XHRcdHRoaXMuY2xvc2VDYWxlbmRhcigpO1xuXHRcdFx0fVxuXHRcdH1cblxuXHRcdHRoaXMucHJvcHMub25DaGFuZ2UoIGRhdGUgKTtcblx0fSxcblxuXHRvcGVuQ2FsZW5kYXI6IGZ1bmN0aW9uKCBlICkge1xuXHRcdGlmICggIXRoaXMuc3RhdGUub3BlbiApIHtcblx0XHRcdHRoaXMuc2V0U3RhdGUoeyBvcGVuOiB0cnVlIH0sIGZ1bmN0aW9uKCkge1xuXHRcdFx0XHR0aGlzLnByb3BzLm9uRm9jdXMoIGUgKTtcblx0XHRcdH0pO1xuXHRcdH1cblx0fSxcblxuXHRjbG9zZUNhbGVuZGFyOiBmdW5jdGlvbigpIHtcblx0XHR0aGlzLnNldFN0YXRlKHsgb3BlbjogZmFsc2UgfSwgZnVuY3Rpb24gKCkge1xuXHRcdFx0dGhpcy5wcm9wcy5vbkJsdXIoIHRoaXMuc3RhdGUuc2VsZWN0ZWREYXRlIHx8IHRoaXMuc3RhdGUuaW5wdXRWYWx1ZSApO1xuXHRcdH0pO1xuXHR9LFxuXG5cdGhhbmRsZUNsaWNrT3V0c2lkZTogZnVuY3Rpb24oKSB7XG5cdFx0aWYgKCB0aGlzLnByb3BzLmlucHV0ICYmIHRoaXMuc3RhdGUub3BlbiAmJiAhdGhpcy5wcm9wcy5vcGVuICYmICF0aGlzLnByb3BzLmRpc2FibGVPbkNsaWNrT3V0c2lkZSApIHtcblx0XHRcdHRoaXMuc2V0U3RhdGUoeyBvcGVuOiBmYWxzZSB9LCBmdW5jdGlvbigpIHtcblx0XHRcdFx0dGhpcy5wcm9wcy5vbkJsdXIoIHRoaXMuc3RhdGUuc2VsZWN0ZWREYXRlIHx8IHRoaXMuc3RhdGUuaW5wdXRWYWx1ZSApO1xuXHRcdFx0fSk7XG5cdFx0fVxuXHR9LFxuXG5cdGxvY2FsTW9tZW50OiBmdW5jdGlvbiggZGF0ZSwgZm9ybWF0LCBwcm9wcyApIHtcblx0XHRwcm9wcyA9IHByb3BzIHx8IHRoaXMucHJvcHM7XG5cdFx0dmFyIG1vbWVudEZuID0gcHJvcHMudXRjID8gbW9tZW50LnV0YyA6IG1vbWVudDtcblx0XHR2YXIgbSA9IG1vbWVudEZuKCBkYXRlLCBmb3JtYXQsIHByb3BzLnN0cmljdFBhcnNpbmcgKTtcblx0XHRpZiAoIHByb3BzLmxvY2FsZSApXG5cdFx0XHRtLmxvY2FsZSggcHJvcHMubG9jYWxlICk7XG5cdFx0cmV0dXJuIG07XG5cdH0sXG5cblx0Y29tcG9uZW50UHJvcHM6IHtcblx0XHRmcm9tUHJvcHM6IFsndmFsdWUnLCAnaXNWYWxpZERhdGUnLCAncmVuZGVyRGF5JywgJ3JlbmRlck1vbnRoJywgJ3JlbmRlclllYXInLCAndGltZUNvbnN0cmFpbnRzJ10sXG5cdFx0ZnJvbVN0YXRlOiBbJ3ZpZXdEYXRlJywgJ3NlbGVjdGVkRGF0ZScsICd1cGRhdGVPbiddLFxuXHRcdGZyb21UaGlzOiBbJ3NldERhdGUnLCAnc2V0VGltZScsICdzaG93VmlldycsICdhZGRUaW1lJywgJ3N1YnRyYWN0VGltZScsICd1cGRhdGVTZWxlY3RlZERhdGUnLCAnbG9jYWxNb21lbnQnLCAnaGFuZGxlQ2xpY2tPdXRzaWRlJ11cblx0fSxcblxuXHRnZXRDb21wb25lbnRQcm9wczogZnVuY3Rpb24oKSB7XG5cdFx0dmFyIG1lID0gdGhpcyxcblx0XHRcdGZvcm1hdHMgPSB0aGlzLmdldEZvcm1hdHMoIHRoaXMucHJvcHMgKSxcblx0XHRcdHByb3BzID0ge2RhdGVGb3JtYXQ6IGZvcm1hdHMuZGF0ZSwgdGltZUZvcm1hdDogZm9ybWF0cy50aW1lfVxuXHRcdFx0O1xuXG5cdFx0dGhpcy5jb21wb25lbnRQcm9wcy5mcm9tUHJvcHMuZm9yRWFjaCggZnVuY3Rpb24oIG5hbWUgKSB7XG5cdFx0XHRwcm9wc1sgbmFtZSBdID0gbWUucHJvcHNbIG5hbWUgXTtcblx0XHR9KTtcblx0XHR0aGlzLmNvbXBvbmVudFByb3BzLmZyb21TdGF0ZS5mb3JFYWNoKCBmdW5jdGlvbiggbmFtZSApIHtcblx0XHRcdHByb3BzWyBuYW1lIF0gPSBtZS5zdGF0ZVsgbmFtZSBdO1xuXHRcdH0pO1xuXHRcdHRoaXMuY29tcG9uZW50UHJvcHMuZnJvbVRoaXMuZm9yRWFjaCggZnVuY3Rpb24oIG5hbWUgKSB7XG5cdFx0XHRwcm9wc1sgbmFtZSBdID0gbWVbIG5hbWUgXTtcblx0XHR9KTtcblxuXHRcdHJldHVybiBwcm9wcztcblx0fSxcblxuXHRyZW5kZXI6IGZ1bmN0aW9uKCkge1xuXHRcdC8vIFRPRE86IE1ha2UgYSBmdW5jdGlvbiBvciBjbGVhbiB1cCB0aGlzIGNvZGUsXG5cdFx0Ly8gbG9naWMgcmlnaHQgbm93IGlzIHJlYWxseSBoYXJkIHRvIGZvbGxvd1xuXHRcdHZhciBjbGFzc05hbWUgPSAncmR0JyArICh0aGlzLnByb3BzLmNsYXNzTmFtZSA/XG4gICAgICAgICAgICAgICAgICAoIEFycmF5LmlzQXJyYXkoIHRoaXMucHJvcHMuY2xhc3NOYW1lICkgP1xuICAgICAgICAgICAgICAgICAgJyAnICsgdGhpcy5wcm9wcy5jbGFzc05hbWUuam9pbiggJyAnICkgOiAnICcgKyB0aGlzLnByb3BzLmNsYXNzTmFtZSkgOiAnJyksXG5cdFx0XHRjaGlsZHJlbiA9IFtdO1xuXG5cdFx0aWYgKCB0aGlzLnByb3BzLmlucHV0ICkge1xuXHRcdFx0dmFyIGZpbmFsSW5wdXRQcm9wcyA9IGFzc2lnbih7XG5cdFx0XHRcdHR5cGU6ICd0ZXh0Jyxcblx0XHRcdFx0Y2xhc3NOYW1lOiAnZm9ybS1jb250cm9sJyxcblx0XHRcdFx0b25DbGljazogdGhpcy5vcGVuQ2FsZW5kYXIsXG5cdFx0XHRcdG9uRm9jdXM6IHRoaXMub3BlbkNhbGVuZGFyLFxuXHRcdFx0XHRvbkNoYW5nZTogdGhpcy5vbklucHV0Q2hhbmdlLFxuXHRcdFx0XHRvbktleURvd246IHRoaXMub25JbnB1dEtleSxcblx0XHRcdFx0dmFsdWU6IHRoaXMuc3RhdGUuaW5wdXRWYWx1ZSxcblx0XHRcdH0sIHRoaXMucHJvcHMuaW5wdXRQcm9wcyk7XG5cdFx0XHRpZiAoIHRoaXMucHJvcHMucmVuZGVySW5wdXQgKSB7XG5cdFx0XHRcdGNoaWxkcmVuID0gWyBSZWFjdC5jcmVhdGVFbGVtZW50KCdkaXYnLCB7IGtleTogJ2knIH0sIHRoaXMucHJvcHMucmVuZGVySW5wdXQoIGZpbmFsSW5wdXRQcm9wcywgdGhpcy5vcGVuQ2FsZW5kYXIsIHRoaXMuY2xvc2VDYWxlbmRhciApKSBdO1xuXHRcdFx0fSBlbHNlIHtcblx0XHRcdFx0Y2hpbGRyZW4gPSBbIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ2lucHV0JywgYXNzaWduKHsga2V5OiAnaScgfSwgZmluYWxJbnB1dFByb3BzICkpXTtcblx0XHRcdH1cblx0XHR9IGVsc2Uge1xuXHRcdFx0Y2xhc3NOYW1lICs9ICcgcmR0U3RhdGljJztcblx0XHR9XG5cblx0XHRpZiAoIHRoaXMuc3RhdGUub3BlbiApXG5cdFx0XHRjbGFzc05hbWUgKz0gJyByZHRPcGVuJztcblxuXHRcdHJldHVybiBSZWFjdC5jcmVhdGVFbGVtZW50KCAnZGl2JywgeyBjbGFzc05hbWU6IGNsYXNzTmFtZSB9LCBjaGlsZHJlbi5jb25jYXQoXG5cdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCAnZGl2Jyxcblx0XHRcdFx0eyBrZXk6ICdkdCcsIGNsYXNzTmFtZTogJ3JkdFBpY2tlcicgfSxcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCggQ2FsZW5kYXJDb250YWluZXIsIHsgdmlldzogdGhpcy5zdGF0ZS5jdXJyZW50Vmlldywgdmlld1Byb3BzOiB0aGlzLmdldENvbXBvbmVudFByb3BzKCksIG9uQ2xpY2tPdXRzaWRlOiB0aGlzLmhhbmRsZUNsaWNrT3V0c2lkZSB9KVxuXHRcdFx0KVxuXHRcdCkpO1xuXHR9XG59KTtcblxuRGF0ZXRpbWUuZGVmYXVsdFByb3BzID0ge1xuXHRjbGFzc05hbWU6ICcnLFxuXHRkZWZhdWx0VmFsdWU6ICcnLFxuXHRpbnB1dFByb3BzOiB7fSxcblx0aW5wdXQ6IHRydWUsXG5cdG9uRm9jdXM6IGZ1bmN0aW9uKCkge30sXG5cdG9uQmx1cjogZnVuY3Rpb24oKSB7fSxcblx0b25DaGFuZ2U6IGZ1bmN0aW9uKCkge30sXG5cdG9uVmlld01vZGVDaGFuZ2U6IGZ1bmN0aW9uKCkge30sXG5cdHRpbWVGb3JtYXQ6IHRydWUsXG5cdHRpbWVDb25zdHJhaW50czoge30sXG5cdGRhdGVGb3JtYXQ6IHRydWUsXG5cdHN0cmljdFBhcnNpbmc6IHRydWUsXG5cdGNsb3NlT25TZWxlY3Q6IGZhbHNlLFxuXHRjbG9zZU9uVGFiOiB0cnVlLFxuXHR1dGM6IGZhbHNlXG59O1xuXG4vLyBNYWtlIG1vbWVudCBhY2Nlc3NpYmxlIHRocm91Z2ggdGhlIERhdGV0aW1lIGNsYXNzXG5EYXRldGltZS5tb21lbnQgPSBtb21lbnQ7XG5cbm1vZHVsZS5leHBvcnRzID0gRGF0ZXRpbWU7XG4iLCJcbnZhciBjb250ZW50ID0gcmVxdWlyZShcIiEhLi4vLi4vY3NzLWxvYWRlci9kaXN0L2Nqcy5qcyEuL3JlYWN0LWRhdGV0aW1lLmNzc1wiKTtcblxuaWYodHlwZW9mIGNvbnRlbnQgPT09ICdzdHJpbmcnKSBjb250ZW50ID0gW1ttb2R1bGUuaWQsIGNvbnRlbnQsICcnXV07XG5cbnZhciB0cmFuc2Zvcm07XG52YXIgaW5zZXJ0SW50bztcblxuXG5cbnZhciBvcHRpb25zID0ge1wiaG1yXCI6dHJ1ZX1cblxub3B0aW9ucy50cmFuc2Zvcm0gPSB0cmFuc2Zvcm1cbm9wdGlvbnMuaW5zZXJ0SW50byA9IHVuZGVmaW5lZDtcblxudmFyIHVwZGF0ZSA9IHJlcXVpcmUoXCIhLi4vLi4vc3R5bGUtbG9hZGVyL2xpYi9hZGRTdHlsZXMuanNcIikoY29udGVudCwgb3B0aW9ucyk7XG5cbmlmKGNvbnRlbnQubG9jYWxzKSBtb2R1bGUuZXhwb3J0cyA9IGNvbnRlbnQubG9jYWxzO1xuXG5pZihtb2R1bGUuaG90KSB7XG5cdG1vZHVsZS5ob3QuYWNjZXB0KFwiISEuLi8uLi9jc3MtbG9hZGVyL2Rpc3QvY2pzLmpzIS4vcmVhY3QtZGF0ZXRpbWUuY3NzXCIsIGZ1bmN0aW9uKCkge1xuXHRcdHZhciBuZXdDb250ZW50ID0gcmVxdWlyZShcIiEhLi4vLi4vY3NzLWxvYWRlci9kaXN0L2Nqcy5qcyEuL3JlYWN0LWRhdGV0aW1lLmNzc1wiKTtcblxuXHRcdGlmKHR5cGVvZiBuZXdDb250ZW50ID09PSAnc3RyaW5nJykgbmV3Q29udGVudCA9IFtbbW9kdWxlLmlkLCBuZXdDb250ZW50LCAnJ11dO1xuXG5cdFx0dmFyIGxvY2FscyA9IChmdW5jdGlvbihhLCBiKSB7XG5cdFx0XHR2YXIga2V5LCBpZHggPSAwO1xuXG5cdFx0XHRmb3Ioa2V5IGluIGEpIHtcblx0XHRcdFx0aWYoIWIgfHwgYVtrZXldICE9PSBiW2tleV0pIHJldHVybiBmYWxzZTtcblx0XHRcdFx0aWR4Kys7XG5cdFx0XHR9XG5cblx0XHRcdGZvcihrZXkgaW4gYikgaWR4LS07XG5cblx0XHRcdHJldHVybiBpZHggPT09IDA7XG5cdFx0fShjb250ZW50LmxvY2FscywgbmV3Q29udGVudC5sb2NhbHMpKTtcblxuXHRcdGlmKCFsb2NhbHMpIHRocm93IG5ldyBFcnJvcignQWJvcnRpbmcgQ1NTIEhNUiBkdWUgdG8gY2hhbmdlZCBjc3MtbW9kdWxlcyBsb2NhbHMuJyk7XG5cblx0XHR1cGRhdGUobmV3Q29udGVudCk7XG5cdH0pO1xuXG5cdG1vZHVsZS5ob3QuZGlzcG9zZShmdW5jdGlvbigpIHsgdXBkYXRlKCk7IH0pO1xufSIsIid1c2Ugc3RyaWN0JztcbnZhciBwcm9wSXNFbnVtZXJhYmxlID0gT2JqZWN0LnByb3RvdHlwZS5wcm9wZXJ0eUlzRW51bWVyYWJsZTtcblxuZnVuY3Rpb24gVG9PYmplY3QodmFsKSB7XG5cdGlmICh2YWwgPT0gbnVsbCkge1xuXHRcdHRocm93IG5ldyBUeXBlRXJyb3IoJ09iamVjdC5hc3NpZ24gY2Fubm90IGJlIGNhbGxlZCB3aXRoIG51bGwgb3IgdW5kZWZpbmVkJyk7XG5cdH1cblxuXHRyZXR1cm4gT2JqZWN0KHZhbCk7XG59XG5cbmZ1bmN0aW9uIG93bkVudW1lcmFibGVLZXlzKG9iaikge1xuXHR2YXIga2V5cyA9IE9iamVjdC5nZXRPd25Qcm9wZXJ0eU5hbWVzKG9iaik7XG5cblx0aWYgKE9iamVjdC5nZXRPd25Qcm9wZXJ0eVN5bWJvbHMpIHtcblx0XHRrZXlzID0ga2V5cy5jb25jYXQoT2JqZWN0LmdldE93blByb3BlcnR5U3ltYm9scyhvYmopKTtcblx0fVxuXG5cdHJldHVybiBrZXlzLmZpbHRlcihmdW5jdGlvbiAoa2V5KSB7XG5cdFx0cmV0dXJuIHByb3BJc0VudW1lcmFibGUuY2FsbChvYmosIGtleSk7XG5cdH0pO1xufVxuXG5tb2R1bGUuZXhwb3J0cyA9IE9iamVjdC5hc3NpZ24gfHwgZnVuY3Rpb24gKHRhcmdldCwgc291cmNlKSB7XG5cdHZhciBmcm9tO1xuXHR2YXIga2V5cztcblx0dmFyIHRvID0gVG9PYmplY3QodGFyZ2V0KTtcblxuXHRmb3IgKHZhciBzID0gMTsgcyA8IGFyZ3VtZW50cy5sZW5ndGg7IHMrKykge1xuXHRcdGZyb20gPSBhcmd1bWVudHNbc107XG5cdFx0a2V5cyA9IG93bkVudW1lcmFibGVLZXlzKE9iamVjdChmcm9tKSk7XG5cblx0XHRmb3IgKHZhciBpID0gMDsgaSA8IGtleXMubGVuZ3RoOyBpKyspIHtcblx0XHRcdHRvW2tleXNbaV1dID0gZnJvbVtrZXlzW2ldXTtcblx0XHR9XG5cdH1cblxuXHRyZXR1cm4gdG87XG59O1xuIiwiJ3VzZSBzdHJpY3QnO1xuXG52YXIgUmVhY3QgPSByZXF1aXJlKCdyZWFjdCcpLFxuXHRjcmVhdGVDbGFzcyA9IHJlcXVpcmUoJ2NyZWF0ZS1yZWFjdC1jbGFzcycpLFxuXHREYXlzVmlldyA9IHJlcXVpcmUoJy4vRGF5c1ZpZXcnKSxcblx0TW9udGhzVmlldyA9IHJlcXVpcmUoJy4vTW9udGhzVmlldycpLFxuXHRZZWFyc1ZpZXcgPSByZXF1aXJlKCcuL1llYXJzVmlldycpLFxuXHRUaW1lVmlldyA9IHJlcXVpcmUoJy4vVGltZVZpZXcnKVxuXHQ7XG5cbnZhciBDYWxlbmRhckNvbnRhaW5lciA9IGNyZWF0ZUNsYXNzKHtcblx0dmlld0NvbXBvbmVudHM6IHtcblx0XHRkYXlzOiBEYXlzVmlldyxcblx0XHRtb250aHM6IE1vbnRoc1ZpZXcsXG5cdFx0eWVhcnM6IFllYXJzVmlldyxcblx0XHR0aW1lOiBUaW1lVmlld1xuXHR9LFxuXG5cdHJlbmRlcjogZnVuY3Rpb24oKSB7XG5cdFx0cmV0dXJuIFJlYWN0LmNyZWF0ZUVsZW1lbnQoIHRoaXMudmlld0NvbXBvbmVudHNbIHRoaXMucHJvcHMudmlldyBdLCB0aGlzLnByb3BzLnZpZXdQcm9wcyApO1xuXHR9XG59KTtcblxubW9kdWxlLmV4cG9ydHMgPSBDYWxlbmRhckNvbnRhaW5lcjtcbiIsIid1c2Ugc3RyaWN0JztcblxudmFyIFJlYWN0ID0gcmVxdWlyZSgncmVhY3QnKSxcblx0Y3JlYXRlQ2xhc3MgPSByZXF1aXJlKCdjcmVhdGUtcmVhY3QtY2xhc3MnKSxcblx0bW9tZW50ID0gcmVxdWlyZSgnbW9tZW50JyksXG5cdG9uQ2xpY2tPdXRzaWRlID0gcmVxdWlyZSgncmVhY3Qtb25jbGlja291dHNpZGUnKS5kZWZhdWx0XG5cdDtcblxudmFyIERhdGVUaW1lUGlja2VyRGF5cyA9IG9uQ2xpY2tPdXRzaWRlKCBjcmVhdGVDbGFzcyh7XG5cdHJlbmRlcjogZnVuY3Rpb24oKSB7XG5cdFx0dmFyIGZvb3RlciA9IHRoaXMucmVuZGVyRm9vdGVyKCksXG5cdFx0XHRkYXRlID0gdGhpcy5wcm9wcy52aWV3RGF0ZSxcblx0XHRcdGxvY2FsZSA9IGRhdGUubG9jYWxlRGF0YSgpLFxuXHRcdFx0dGFibGVDaGlsZHJlblxuXHRcdFx0O1xuXG5cdFx0dGFibGVDaGlsZHJlbiA9IFtcblx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoZWFkJywgeyBrZXk6ICd0aCcgfSwgW1xuXHRcdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0cicsIHsga2V5OiAnaCcgfSwgW1xuXHRcdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoJywgeyBrZXk6ICdwJywgY2xhc3NOYW1lOiAncmR0UHJldicsIG9uQ2xpY2s6IHRoaXMucHJvcHMuc3VidHJhY3RUaW1lKCAxLCAnbW9udGhzJyApfSwgUmVhY3QuY3JlYXRlRWxlbWVudCgnc3BhbicsIHt9LCAn4oC5JyApKSxcblx0XHRcdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0aCcsIHsga2V5OiAncycsIGNsYXNzTmFtZTogJ3JkdFN3aXRjaCcsIG9uQ2xpY2s6IHRoaXMucHJvcHMuc2hvd1ZpZXcoICdtb250aHMnICksIGNvbFNwYW46IDUsICdkYXRhLXZhbHVlJzogdGhpcy5wcm9wcy52aWV3RGF0ZS5tb250aCgpIH0sIGxvY2FsZS5tb250aHMoIGRhdGUgKSArICcgJyArIGRhdGUueWVhcigpICksXG5cdFx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGgnLCB7IGtleTogJ24nLCBjbGFzc05hbWU6ICdyZHROZXh0Jywgb25DbGljazogdGhpcy5wcm9wcy5hZGRUaW1lKCAxLCAnbW9udGhzJyApfSwgUmVhY3QuY3JlYXRlRWxlbWVudCgnc3BhbicsIHt9LCAn4oC6JyApKVxuXHRcdFx0XHRdKSxcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndHInLCB7IGtleTogJ2QnfSwgdGhpcy5nZXREYXlzT2ZXZWVrKCBsb2NhbGUgKS5tYXAoIGZ1bmN0aW9uKCBkYXksIGluZGV4ICkgeyByZXR1cm4gUmVhY3QuY3JlYXRlRWxlbWVudCgndGgnLCB7IGtleTogZGF5ICsgaW5kZXgsIGNsYXNzTmFtZTogJ2Rvdyd9LCBkYXkgKTsgfSkgKVxuXHRcdFx0XSksXG5cdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0Ym9keScsIHsga2V5OiAndGInIH0sIHRoaXMucmVuZGVyRGF5cygpKVxuXHRcdF07XG5cblx0XHRpZiAoIGZvb3RlciApXG5cdFx0XHR0YWJsZUNoaWxkcmVuLnB1c2goIGZvb3RlciApO1xuXG5cdFx0cmV0dXJuIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ2RpdicsIHsgY2xhc3NOYW1lOiAncmR0RGF5cycgfSxcblx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RhYmxlJywge30sIHRhYmxlQ2hpbGRyZW4gKVxuXHRcdCk7XG5cdH0sXG5cblx0LyoqXG5cdCAqIEdldCBhIGxpc3Qgb2YgdGhlIGRheXMgb2YgdGhlIHdlZWtcblx0ICogZGVwZW5kaW5nIG9uIHRoZSBjdXJyZW50IGxvY2FsZVxuXHQgKiBAcmV0dXJuIHthcnJheX0gQSBsaXN0IHdpdGggdGhlIHNob3J0bmFtZSBvZiB0aGUgZGF5c1xuXHQgKi9cblx0Z2V0RGF5c09mV2VlazogZnVuY3Rpb24oIGxvY2FsZSApIHtcblx0XHR2YXIgZGF5cyA9IGxvY2FsZS5fd2Vla2RheXNNaW4sXG5cdFx0XHRmaXJzdCA9IGxvY2FsZS5maXJzdERheU9mV2VlaygpLFxuXHRcdFx0ZG93ID0gW10sXG5cdFx0XHRpID0gMFxuXHRcdFx0O1xuXG5cdFx0ZGF5cy5mb3JFYWNoKCBmdW5jdGlvbiggZGF5ICkge1xuXHRcdFx0ZG93WyAoNyArICggaSsrICkgLSBmaXJzdCkgJSA3IF0gPSBkYXk7XG5cdFx0fSk7XG5cblx0XHRyZXR1cm4gZG93O1xuXHR9LFxuXG5cdHJlbmRlckRheXM6IGZ1bmN0aW9uKCkge1xuXHRcdHZhciBkYXRlID0gdGhpcy5wcm9wcy52aWV3RGF0ZSxcblx0XHRcdHNlbGVjdGVkID0gdGhpcy5wcm9wcy5zZWxlY3RlZERhdGUgJiYgdGhpcy5wcm9wcy5zZWxlY3RlZERhdGUuY2xvbmUoKSxcblx0XHRcdHByZXZNb250aCA9IGRhdGUuY2xvbmUoKS5zdWJ0cmFjdCggMSwgJ21vbnRocycgKSxcblx0XHRcdGN1cnJlbnRZZWFyID0gZGF0ZS55ZWFyKCksXG5cdFx0XHRjdXJyZW50TW9udGggPSBkYXRlLm1vbnRoKCksXG5cdFx0XHR3ZWVrcyA9IFtdLFxuXHRcdFx0ZGF5cyA9IFtdLFxuXHRcdFx0cmVuZGVyZXIgPSB0aGlzLnByb3BzLnJlbmRlckRheSB8fCB0aGlzLnJlbmRlckRheSxcblx0XHRcdGlzVmFsaWQgPSB0aGlzLnByb3BzLmlzVmFsaWREYXRlIHx8IHRoaXMuYWx3YXlzVmFsaWREYXRlLFxuXHRcdFx0Y2xhc3NlcywgaXNEaXNhYmxlZCwgZGF5UHJvcHMsIGN1cnJlbnREYXRlXG5cdFx0XHQ7XG5cblx0XHQvLyBHbyB0byB0aGUgbGFzdCB3ZWVrIG9mIHRoZSBwcmV2aW91cyBtb250aFxuXHRcdHByZXZNb250aC5kYXRlKCBwcmV2TW9udGguZGF5c0luTW9udGgoKSApLnN0YXJ0T2YoICd3ZWVrJyApO1xuXHRcdHZhciBsYXN0RGF5ID0gcHJldk1vbnRoLmNsb25lKCkuYWRkKCA0MiwgJ2QnICk7XG5cblx0XHR3aGlsZSAoIHByZXZNb250aC5pc0JlZm9yZSggbGFzdERheSApICkge1xuXHRcdFx0Y2xhc3NlcyA9ICdyZHREYXknO1xuXHRcdFx0Y3VycmVudERhdGUgPSBwcmV2TW9udGguY2xvbmUoKTtcblxuXHRcdFx0aWYgKCAoIHByZXZNb250aC55ZWFyKCkgPT09IGN1cnJlbnRZZWFyICYmIHByZXZNb250aC5tb250aCgpIDwgY3VycmVudE1vbnRoICkgfHwgKCBwcmV2TW9udGgueWVhcigpIDwgY3VycmVudFllYXIgKSApXG5cdFx0XHRcdGNsYXNzZXMgKz0gJyByZHRPbGQnO1xuXHRcdFx0ZWxzZSBpZiAoICggcHJldk1vbnRoLnllYXIoKSA9PT0gY3VycmVudFllYXIgJiYgcHJldk1vbnRoLm1vbnRoKCkgPiBjdXJyZW50TW9udGggKSB8fCAoIHByZXZNb250aC55ZWFyKCkgPiBjdXJyZW50WWVhciApIClcblx0XHRcdFx0Y2xhc3NlcyArPSAnIHJkdE5ldyc7XG5cblx0XHRcdGlmICggc2VsZWN0ZWQgJiYgcHJldk1vbnRoLmlzU2FtZSggc2VsZWN0ZWQsICdkYXknICkgKVxuXHRcdFx0XHRjbGFzc2VzICs9ICcgcmR0QWN0aXZlJztcblxuXHRcdFx0aWYgKCBwcmV2TW9udGguaXNTYW1lKCBtb21lbnQoKSwgJ2RheScgKSApXG5cdFx0XHRcdGNsYXNzZXMgKz0gJyByZHRUb2RheSc7XG5cblx0XHRcdGlzRGlzYWJsZWQgPSAhaXNWYWxpZCggY3VycmVudERhdGUsIHNlbGVjdGVkICk7XG5cdFx0XHRpZiAoIGlzRGlzYWJsZWQgKVxuXHRcdFx0XHRjbGFzc2VzICs9ICcgcmR0RGlzYWJsZWQnO1xuXG5cdFx0XHRkYXlQcm9wcyA9IHtcblx0XHRcdFx0a2V5OiBwcmV2TW9udGguZm9ybWF0KCAnTV9EJyApLFxuXHRcdFx0XHQnZGF0YS12YWx1ZSc6IHByZXZNb250aC5kYXRlKCksXG5cdFx0XHRcdGNsYXNzTmFtZTogY2xhc3Nlc1xuXHRcdFx0fTtcblxuXHRcdFx0aWYgKCAhaXNEaXNhYmxlZCApXG5cdFx0XHRcdGRheVByb3BzLm9uQ2xpY2sgPSB0aGlzLnVwZGF0ZVNlbGVjdGVkRGF0ZTtcblxuXHRcdFx0ZGF5cy5wdXNoKCByZW5kZXJlciggZGF5UHJvcHMsIGN1cnJlbnREYXRlLCBzZWxlY3RlZCApICk7XG5cblx0XHRcdGlmICggZGF5cy5sZW5ndGggPT09IDcgKSB7XG5cdFx0XHRcdHdlZWtzLnB1c2goIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RyJywgeyBrZXk6IHByZXZNb250aC5mb3JtYXQoICdNX0QnICl9LCBkYXlzICkgKTtcblx0XHRcdFx0ZGF5cyA9IFtdO1xuXHRcdFx0fVxuXG5cdFx0XHRwcmV2TW9udGguYWRkKCAxLCAnZCcgKTtcblx0XHR9XG5cblx0XHRyZXR1cm4gd2Vla3M7XG5cdH0sXG5cblx0dXBkYXRlU2VsZWN0ZWREYXRlOiBmdW5jdGlvbiggZXZlbnQgKSB7XG5cdFx0dGhpcy5wcm9wcy51cGRhdGVTZWxlY3RlZERhdGUoIGV2ZW50LCB0cnVlICk7XG5cdH0sXG5cblx0cmVuZGVyRGF5OiBmdW5jdGlvbiggcHJvcHMsIGN1cnJlbnREYXRlICkge1xuXHRcdHJldHVybiBSZWFjdC5jcmVhdGVFbGVtZW50KCd0ZCcsICBwcm9wcywgY3VycmVudERhdGUuZGF0ZSgpICk7XG5cdH0sXG5cblx0cmVuZGVyRm9vdGVyOiBmdW5jdGlvbigpIHtcblx0XHRpZiAoICF0aGlzLnByb3BzLnRpbWVGb3JtYXQgKVxuXHRcdFx0cmV0dXJuICcnO1xuXG5cdFx0dmFyIGRhdGUgPSB0aGlzLnByb3BzLnNlbGVjdGVkRGF0ZSB8fCB0aGlzLnByb3BzLnZpZXdEYXRlO1xuXG5cdFx0cmV0dXJuIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3Rmb290JywgeyBrZXk6ICd0Zid9LFxuXHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndHInLCB7fSxcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGQnLCB7IG9uQ2xpY2s6IHRoaXMucHJvcHMuc2hvd1ZpZXcoICd0aW1lJyApLCBjb2xTcGFuOiA3LCBjbGFzc05hbWU6ICdyZHRUaW1lVG9nZ2xlJyB9LCBkYXRlLmZvcm1hdCggdGhpcy5wcm9wcy50aW1lRm9ybWF0ICkpXG5cdFx0XHQpXG5cdFx0KTtcblx0fSxcblxuXHRhbHdheXNWYWxpZERhdGU6IGZ1bmN0aW9uKCkge1xuXHRcdHJldHVybiAxO1xuXHR9LFxuXG5cdGhhbmRsZUNsaWNrT3V0c2lkZTogZnVuY3Rpb24oKSB7XG5cdFx0dGhpcy5wcm9wcy5oYW5kbGVDbGlja091dHNpZGUoKTtcblx0fVxufSkpO1xuXG5tb2R1bGUuZXhwb3J0cyA9IERhdGVUaW1lUGlja2VyRGF5cztcbiIsIid1c2Ugc3RyaWN0JztcblxudmFyIFJlYWN0ID0gcmVxdWlyZSgncmVhY3QnKSxcblx0Y3JlYXRlQ2xhc3MgPSByZXF1aXJlKCdjcmVhdGUtcmVhY3QtY2xhc3MnKSxcblx0b25DbGlja091dHNpZGUgPSByZXF1aXJlKCdyZWFjdC1vbmNsaWNrb3V0c2lkZScpLmRlZmF1bHRcblx0O1xuXG52YXIgRGF0ZVRpbWVQaWNrZXJNb250aHMgPSBvbkNsaWNrT3V0c2lkZSggY3JlYXRlQ2xhc3Moe1xuXHRyZW5kZXI6IGZ1bmN0aW9uKCkge1xuXHRcdHJldHVybiBSZWFjdC5jcmVhdGVFbGVtZW50KCdkaXYnLCB7IGNsYXNzTmFtZTogJ3JkdE1vbnRocycgfSwgW1xuXHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGFibGUnLCB7IGtleTogJ2EnIH0sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoZWFkJywge30sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RyJywge30sIFtcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGgnLCB7IGtleTogJ3ByZXYnLCBjbGFzc05hbWU6ICdyZHRQcmV2Jywgb25DbGljazogdGhpcy5wcm9wcy5zdWJ0cmFjdFRpbWUoIDEsICd5ZWFycycgKX0sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3NwYW4nLCB7fSwgJ+KAuScgKSksXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoJywgeyBrZXk6ICd5ZWFyJywgY2xhc3NOYW1lOiAncmR0U3dpdGNoJywgb25DbGljazogdGhpcy5wcm9wcy5zaG93VmlldyggJ3llYXJzJyApLCBjb2xTcGFuOiAyLCAnZGF0YS12YWx1ZSc6IHRoaXMucHJvcHMudmlld0RhdGUueWVhcigpIH0sIHRoaXMucHJvcHMudmlld0RhdGUueWVhcigpICksXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoJywgeyBrZXk6ICduZXh0JywgY2xhc3NOYW1lOiAncmR0TmV4dCcsIG9uQ2xpY2s6IHRoaXMucHJvcHMuYWRkVGltZSggMSwgJ3llYXJzJyApfSwgUmVhY3QuY3JlYXRlRWxlbWVudCgnc3BhbicsIHt9LCAn4oC6JyApKVxuXHRcdFx0XSkpKSxcblx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RhYmxlJywgeyBrZXk6ICdtb250aHMnIH0sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3Rib2R5JywgeyBrZXk6ICdiJyB9LCB0aGlzLnJlbmRlck1vbnRocygpKSlcblx0XHRdKTtcblx0fSxcblxuXHRyZW5kZXJNb250aHM6IGZ1bmN0aW9uKCkge1xuXHRcdHZhciBkYXRlID0gdGhpcy5wcm9wcy5zZWxlY3RlZERhdGUsXG5cdFx0XHRtb250aCA9IHRoaXMucHJvcHMudmlld0RhdGUubW9udGgoKSxcblx0XHRcdHllYXIgPSB0aGlzLnByb3BzLnZpZXdEYXRlLnllYXIoKSxcblx0XHRcdHJvd3MgPSBbXSxcblx0XHRcdGkgPSAwLFxuXHRcdFx0bW9udGhzID0gW10sXG5cdFx0XHRyZW5kZXJlciA9IHRoaXMucHJvcHMucmVuZGVyTW9udGggfHwgdGhpcy5yZW5kZXJNb250aCxcblx0XHRcdGlzVmFsaWQgPSB0aGlzLnByb3BzLmlzVmFsaWREYXRlIHx8IHRoaXMuYWx3YXlzVmFsaWREYXRlLFxuXHRcdFx0Y2xhc3NlcywgcHJvcHMsIGN1cnJlbnRNb250aCwgaXNEaXNhYmxlZCwgbm9PZkRheXNJbk1vbnRoLCBkYXlzSW5Nb250aCwgdmFsaWREYXksXG5cdFx0XHQvLyBEYXRlIGlzIGlycmVsZXZhbnQgYmVjYXVzZSB3ZSdyZSBvbmx5IGludGVyZXN0ZWQgaW4gbW9udGhcblx0XHRcdGlycmVsZXZhbnREYXRlID0gMVxuXHRcdFx0O1xuXG5cdFx0d2hpbGUgKGkgPCAxMikge1xuXHRcdFx0Y2xhc3NlcyA9ICdyZHRNb250aCc7XG5cdFx0XHRjdXJyZW50TW9udGggPVxuXHRcdFx0XHR0aGlzLnByb3BzLnZpZXdEYXRlLmNsb25lKCkuc2V0KHsgeWVhcjogeWVhciwgbW9udGg6IGksIGRhdGU6IGlycmVsZXZhbnREYXRlIH0pO1xuXG5cdFx0XHRub09mRGF5c0luTW9udGggPSBjdXJyZW50TW9udGguZW5kT2YoICdtb250aCcgKS5mb3JtYXQoICdEJyApO1xuXHRcdFx0ZGF5c0luTW9udGggPSBBcnJheS5mcm9tKHsgbGVuZ3RoOiBub09mRGF5c0luTW9udGggfSwgZnVuY3Rpb24oIGUsIGkgKSB7XG5cdFx0XHRcdHJldHVybiBpICsgMTtcblx0XHRcdH0pO1xuXG5cdFx0XHR2YWxpZERheSA9IGRheXNJbk1vbnRoLmZpbmQoZnVuY3Rpb24oIGQgKSB7XG5cdFx0XHRcdHZhciBkYXkgPSBjdXJyZW50TW9udGguY2xvbmUoKS5zZXQoICdkYXRlJywgZCApO1xuXHRcdFx0XHRyZXR1cm4gaXNWYWxpZCggZGF5ICk7XG5cdFx0XHR9KTtcblxuXHRcdFx0aXNEaXNhYmxlZCA9ICggdmFsaWREYXkgPT09IHVuZGVmaW5lZCApO1xuXG5cdFx0XHRpZiAoIGlzRGlzYWJsZWQgKVxuXHRcdFx0XHRjbGFzc2VzICs9ICcgcmR0RGlzYWJsZWQnO1xuXG5cdFx0XHRpZiAoIGRhdGUgJiYgaSA9PT0gZGF0ZS5tb250aCgpICYmIHllYXIgPT09IGRhdGUueWVhcigpIClcblx0XHRcdFx0Y2xhc3NlcyArPSAnIHJkdEFjdGl2ZSc7XG5cblx0XHRcdHByb3BzID0ge1xuXHRcdFx0XHRrZXk6IGksXG5cdFx0XHRcdCdkYXRhLXZhbHVlJzogaSxcblx0XHRcdFx0Y2xhc3NOYW1lOiBjbGFzc2VzXG5cdFx0XHR9O1xuXG5cdFx0XHRpZiAoICFpc0Rpc2FibGVkIClcblx0XHRcdFx0cHJvcHMub25DbGljayA9ICggdGhpcy5wcm9wcy51cGRhdGVPbiA9PT0gJ21vbnRocycgP1xuXHRcdFx0XHRcdHRoaXMudXBkYXRlU2VsZWN0ZWRNb250aCA6IHRoaXMucHJvcHMuc2V0RGF0ZSggJ21vbnRoJyApICk7XG5cblx0XHRcdG1vbnRocy5wdXNoKCByZW5kZXJlciggcHJvcHMsIGksIHllYXIsIGRhdGUgJiYgZGF0ZS5jbG9uZSgpICkgKTtcblxuXHRcdFx0aWYgKCBtb250aHMubGVuZ3RoID09PSA0ICkge1xuXHRcdFx0XHRyb3dzLnB1c2goIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RyJywgeyBrZXk6IG1vbnRoICsgJ18nICsgcm93cy5sZW5ndGggfSwgbW9udGhzICkgKTtcblx0XHRcdFx0bW9udGhzID0gW107XG5cdFx0XHR9XG5cblx0XHRcdGkrKztcblx0XHR9XG5cblx0XHRyZXR1cm4gcm93cztcblx0fSxcblxuXHR1cGRhdGVTZWxlY3RlZE1vbnRoOiBmdW5jdGlvbiggZXZlbnQgKSB7XG5cdFx0dGhpcy5wcm9wcy51cGRhdGVTZWxlY3RlZERhdGUoIGV2ZW50ICk7XG5cdH0sXG5cblx0cmVuZGVyTW9udGg6IGZ1bmN0aW9uKCBwcm9wcywgbW9udGggKSB7XG5cdFx0dmFyIGxvY2FsTW9tZW50ID0gdGhpcy5wcm9wcy52aWV3RGF0ZTtcblx0XHR2YXIgbW9udGhTdHIgPSBsb2NhbE1vbWVudC5sb2NhbGVEYXRhKCkubW9udGhzU2hvcnQoIGxvY2FsTW9tZW50Lm1vbnRoKCBtb250aCApICk7XG5cdFx0dmFyIHN0ckxlbmd0aCA9IDM7XG5cdFx0Ly8gQmVjYXVzZSBzb21lIG1vbnRocyBhcmUgdXAgdG8gNSBjaGFyYWN0ZXJzIGxvbmcsIHdlIHdhbnQgdG9cblx0XHQvLyB1c2UgYSBmaXhlZCBzdHJpbmcgbGVuZ3RoIGZvciBjb25zaXN0ZW5jeVxuXHRcdHZhciBtb250aFN0ckZpeGVkTGVuZ3RoID0gbW9udGhTdHIuc3Vic3RyaW5nKCAwLCBzdHJMZW5ndGggKTtcblx0XHRyZXR1cm4gUmVhY3QuY3JlYXRlRWxlbWVudCgndGQnLCBwcm9wcywgY2FwaXRhbGl6ZSggbW9udGhTdHJGaXhlZExlbmd0aCApICk7XG5cdH0sXG5cblx0YWx3YXlzVmFsaWREYXRlOiBmdW5jdGlvbigpIHtcblx0XHRyZXR1cm4gMTtcblx0fSxcblxuXHRoYW5kbGVDbGlja091dHNpZGU6IGZ1bmN0aW9uKCkge1xuXHRcdHRoaXMucHJvcHMuaGFuZGxlQ2xpY2tPdXRzaWRlKCk7XG5cdH1cbn0pKTtcblxuZnVuY3Rpb24gY2FwaXRhbGl6ZSggc3RyICkge1xuXHRyZXR1cm4gc3RyLmNoYXJBdCggMCApLnRvVXBwZXJDYXNlKCkgKyBzdHIuc2xpY2UoIDEgKTtcbn1cblxubW9kdWxlLmV4cG9ydHMgPSBEYXRlVGltZVBpY2tlck1vbnRocztcbiIsIid1c2Ugc3RyaWN0JztcblxudmFyIFJlYWN0ID0gcmVxdWlyZSgncmVhY3QnKSxcblx0Y3JlYXRlQ2xhc3MgPSByZXF1aXJlKCdjcmVhdGUtcmVhY3QtY2xhc3MnKSxcblx0YXNzaWduID0gcmVxdWlyZSgnb2JqZWN0LWFzc2lnbicpLFxuXHRvbkNsaWNrT3V0c2lkZSA9IHJlcXVpcmUoJ3JlYWN0LW9uY2xpY2tvdXRzaWRlJykuZGVmYXVsdFxuXHQ7XG5cbnZhciBEYXRlVGltZVBpY2tlclRpbWUgPSBvbkNsaWNrT3V0c2lkZSggY3JlYXRlQ2xhc3Moe1xuXHRnZXRJbml0aWFsU3RhdGU6IGZ1bmN0aW9uKCkge1xuXHRcdHJldHVybiB0aGlzLmNhbGN1bGF0ZVN0YXRlKCB0aGlzLnByb3BzICk7XG5cdH0sXG5cblx0Y2FsY3VsYXRlU3RhdGU6IGZ1bmN0aW9uKCBwcm9wcyApIHtcblx0XHR2YXIgZGF0ZSA9IHByb3BzLnNlbGVjdGVkRGF0ZSB8fCBwcm9wcy52aWV3RGF0ZSxcblx0XHRcdGZvcm1hdCA9IHByb3BzLnRpbWVGb3JtYXQsXG5cdFx0XHRjb3VudGVycyA9IFtdXG5cdFx0XHQ7XG5cblx0XHRpZiAoIGZvcm1hdC50b0xvd2VyQ2FzZSgpLmluZGV4T2YoJ2gnKSAhPT0gLTEgKSB7XG5cdFx0XHRjb3VudGVycy5wdXNoKCdob3VycycpO1xuXHRcdFx0aWYgKCBmb3JtYXQuaW5kZXhPZignbScpICE9PSAtMSApIHtcblx0XHRcdFx0Y291bnRlcnMucHVzaCgnbWludXRlcycpO1xuXHRcdFx0XHRpZiAoIGZvcm1hdC5pbmRleE9mKCdzJykgIT09IC0xICkge1xuXHRcdFx0XHRcdGNvdW50ZXJzLnB1c2goJ3NlY29uZHMnKTtcblx0XHRcdFx0fVxuXHRcdFx0fVxuXHRcdH1cblxuXHRcdHZhciBob3VycyA9IGRhdGUuZm9ybWF0KCAnSCcgKTtcblx0XHRcblx0XHR2YXIgZGF5cGFydCA9IGZhbHNlO1xuXHRcdGlmICggdGhpcy5zdGF0ZSAhPT0gbnVsbCAmJiB0aGlzLnByb3BzLnRpbWVGb3JtYXQudG9Mb3dlckNhc2UoKS5pbmRleE9mKCAnIGEnICkgIT09IC0xICkge1xuXHRcdFx0aWYgKCB0aGlzLnByb3BzLnRpbWVGb3JtYXQuaW5kZXhPZiggJyBBJyApICE9PSAtMSApIHtcblx0XHRcdFx0ZGF5cGFydCA9ICggaG91cnMgPj0gMTIgKSA/ICdQTScgOiAnQU0nO1xuXHRcdFx0fSBlbHNlIHtcblx0XHRcdFx0ZGF5cGFydCA9ICggaG91cnMgPj0gMTIgKSA/ICdwbScgOiAnYW0nO1xuXHRcdFx0fVxuXHRcdH1cblxuXHRcdHJldHVybiB7XG5cdFx0XHRob3VyczogaG91cnMsXG5cdFx0XHRtaW51dGVzOiBkYXRlLmZvcm1hdCggJ21tJyApLFxuXHRcdFx0c2Vjb25kczogZGF0ZS5mb3JtYXQoICdzcycgKSxcblx0XHRcdG1pbGxpc2Vjb25kczogZGF0ZS5mb3JtYXQoICdTU1MnICksXG5cdFx0XHRkYXlwYXJ0OiBkYXlwYXJ0LFxuXHRcdFx0Y291bnRlcnM6IGNvdW50ZXJzXG5cdFx0fTtcblx0fSxcblxuXHRyZW5kZXJDb3VudGVyOiBmdW5jdGlvbiggdHlwZSApIHtcblx0XHRpZiAoIHR5cGUgIT09ICdkYXlwYXJ0JyApIHtcblx0XHRcdHZhciB2YWx1ZSA9IHRoaXMuc3RhdGVbIHR5cGUgXTtcblx0XHRcdGlmICggdHlwZSA9PT0gJ2hvdXJzJyAmJiB0aGlzLnByb3BzLnRpbWVGb3JtYXQudG9Mb3dlckNhc2UoKS5pbmRleE9mKCAnIGEnICkgIT09IC0xICkge1xuXHRcdFx0XHR2YWx1ZSA9ICggdmFsdWUgLSAxICkgJSAxMiArIDE7XG5cblx0XHRcdFx0aWYgKCB2YWx1ZSA9PT0gMCApIHtcblx0XHRcdFx0XHR2YWx1ZSA9IDEyO1xuXHRcdFx0XHR9XG5cdFx0XHR9XG5cdFx0XHRyZXR1cm4gUmVhY3QuY3JlYXRlRWxlbWVudCgnZGl2JywgeyBrZXk6IHR5cGUsIGNsYXNzTmFtZTogJ3JkdENvdW50ZXInIH0sIFtcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgnc3BhbicsIHsga2V5OiAndXAnLCBjbGFzc05hbWU6ICdyZHRCdG4nLCBvblRvdWNoU3RhcnQ6IHRoaXMub25TdGFydENsaWNraW5nKCdpbmNyZWFzZScsIHR5cGUpLCBvbk1vdXNlRG93bjogdGhpcy5vblN0YXJ0Q2xpY2tpbmcoICdpbmNyZWFzZScsIHR5cGUgKSwgb25Db250ZXh0TWVudTogdGhpcy5kaXNhYmxlQ29udGV4dE1lbnUgfSwgJ+KWsicgKSxcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgnZGl2JywgeyBrZXk6ICdjJywgY2xhc3NOYW1lOiAncmR0Q291bnQnIH0sIHZhbHVlICksXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3NwYW4nLCB7IGtleTogJ2RvJywgY2xhc3NOYW1lOiAncmR0QnRuJywgb25Ub3VjaFN0YXJ0OiB0aGlzLm9uU3RhcnRDbGlja2luZygnZGVjcmVhc2UnLCB0eXBlKSwgb25Nb3VzZURvd246IHRoaXMub25TdGFydENsaWNraW5nKCAnZGVjcmVhc2UnLCB0eXBlICksIG9uQ29udGV4dE1lbnU6IHRoaXMuZGlzYWJsZUNvbnRleHRNZW51IH0sICfilrwnIClcblx0XHRcdF0pO1xuXHRcdH1cblx0XHRyZXR1cm4gJyc7XG5cdH0sXG5cblx0cmVuZGVyRGF5UGFydDogZnVuY3Rpb24oKSB7XG5cdFx0cmV0dXJuIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ2RpdicsIHsga2V5OiAnZGF5UGFydCcsIGNsYXNzTmFtZTogJ3JkdENvdW50ZXInIH0sIFtcblx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3NwYW4nLCB7IGtleTogJ3VwJywgY2xhc3NOYW1lOiAncmR0QnRuJywgb25Ub3VjaFN0YXJ0OiB0aGlzLm9uU3RhcnRDbGlja2luZygndG9nZ2xlRGF5UGFydCcsICdob3VycycpLCBvbk1vdXNlRG93bjogdGhpcy5vblN0YXJ0Q2xpY2tpbmcoICd0b2dnbGVEYXlQYXJ0JywgJ2hvdXJzJyksIG9uQ29udGV4dE1lbnU6IHRoaXMuZGlzYWJsZUNvbnRleHRNZW51IH0sICfilrInICksXG5cdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCdkaXYnLCB7IGtleTogdGhpcy5zdGF0ZS5kYXlwYXJ0LCBjbGFzc05hbWU6ICdyZHRDb3VudCcgfSwgdGhpcy5zdGF0ZS5kYXlwYXJ0ICksXG5cdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCdzcGFuJywgeyBrZXk6ICdkbycsIGNsYXNzTmFtZTogJ3JkdEJ0bicsIG9uVG91Y2hTdGFydDogdGhpcy5vblN0YXJ0Q2xpY2tpbmcoJ3RvZ2dsZURheVBhcnQnLCAnaG91cnMnKSwgb25Nb3VzZURvd246IHRoaXMub25TdGFydENsaWNraW5nKCAndG9nZ2xlRGF5UGFydCcsICdob3VycycpLCBvbkNvbnRleHRNZW51OiB0aGlzLmRpc2FibGVDb250ZXh0TWVudSB9LCAn4pa8JyApXG5cdFx0XSk7XG5cdH0sXG5cblx0cmVuZGVyOiBmdW5jdGlvbigpIHtcblx0XHR2YXIgbWUgPSB0aGlzLFxuXHRcdFx0Y291bnRlcnMgPSBbXVxuXHRcdDtcblxuXHRcdHRoaXMuc3RhdGUuY291bnRlcnMuZm9yRWFjaCggZnVuY3Rpb24oIGMgKSB7XG5cdFx0XHRpZiAoIGNvdW50ZXJzLmxlbmd0aCApXG5cdFx0XHRcdGNvdW50ZXJzLnB1c2goIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ2RpdicsIHsga2V5OiAnc2VwJyArIGNvdW50ZXJzLmxlbmd0aCwgY2xhc3NOYW1lOiAncmR0Q291bnRlclNlcGFyYXRvcicgfSwgJzonICkgKTtcblx0XHRcdGNvdW50ZXJzLnB1c2goIG1lLnJlbmRlckNvdW50ZXIoIGMgKSApO1xuXHRcdH0pO1xuXG5cdFx0aWYgKCB0aGlzLnN0YXRlLmRheXBhcnQgIT09IGZhbHNlICkge1xuXHRcdFx0Y291bnRlcnMucHVzaCggbWUucmVuZGVyRGF5UGFydCgpICk7XG5cdFx0fVxuXG5cdFx0aWYgKCB0aGlzLnN0YXRlLmNvdW50ZXJzLmxlbmd0aCA9PT0gMyAmJiB0aGlzLnByb3BzLnRpbWVGb3JtYXQuaW5kZXhPZiggJ1MnICkgIT09IC0xICkge1xuXHRcdFx0Y291bnRlcnMucHVzaCggUmVhY3QuY3JlYXRlRWxlbWVudCgnZGl2JywgeyBjbGFzc05hbWU6ICdyZHRDb3VudGVyU2VwYXJhdG9yJywga2V5OiAnc2VwNScgfSwgJzonICkgKTtcblx0XHRcdGNvdW50ZXJzLnB1c2goXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ2RpdicsIHsgY2xhc3NOYW1lOiAncmR0Q291bnRlciByZHRNaWxsaScsIGtleTogJ20nIH0sXG5cdFx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgnaW5wdXQnLCB7IHZhbHVlOiB0aGlzLnN0YXRlLm1pbGxpc2Vjb25kcywgdHlwZTogJ3RleHQnLCBvbkNoYW5nZTogdGhpcy51cGRhdGVNaWxsaSB9IClcblx0XHRcdFx0XHQpXG5cdFx0XHRcdCk7XG5cdFx0fVxuXG5cdFx0cmV0dXJuIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ2RpdicsIHsgY2xhc3NOYW1lOiAncmR0VGltZScgfSxcblx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RhYmxlJywge30sIFtcblx0XHRcdFx0dGhpcy5yZW5kZXJIZWFkZXIoKSxcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGJvZHknLCB7IGtleTogJ2InfSwgUmVhY3QuY3JlYXRlRWxlbWVudCgndHInLCB7fSwgUmVhY3QuY3JlYXRlRWxlbWVudCgndGQnLCB7fSxcblx0XHRcdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCdkaXYnLCB7IGNsYXNzTmFtZTogJ3JkdENvdW50ZXJzJyB9LCBjb3VudGVycyApXG5cdFx0XHRcdCkpKVxuXHRcdFx0XSlcblx0XHQpO1xuXHR9LFxuXG5cdGNvbXBvbmVudFdpbGxNb3VudDogZnVuY3Rpb24oKSB7XG5cdFx0dmFyIG1lID0gdGhpcztcblx0XHRtZS50aW1lQ29uc3RyYWludHMgPSB7XG5cdFx0XHRob3Vyczoge1xuXHRcdFx0XHRtaW46IDAsXG5cdFx0XHRcdG1heDogMjMsXG5cdFx0XHRcdHN0ZXA6IDFcblx0XHRcdH0sXG5cdFx0XHRtaW51dGVzOiB7XG5cdFx0XHRcdG1pbjogMCxcblx0XHRcdFx0bWF4OiA1OSxcblx0XHRcdFx0c3RlcDogMVxuXHRcdFx0fSxcblx0XHRcdHNlY29uZHM6IHtcblx0XHRcdFx0bWluOiAwLFxuXHRcdFx0XHRtYXg6IDU5LFxuXHRcdFx0XHRzdGVwOiAxXG5cdFx0XHR9LFxuXHRcdFx0bWlsbGlzZWNvbmRzOiB7XG5cdFx0XHRcdG1pbjogMCxcblx0XHRcdFx0bWF4OiA5OTksXG5cdFx0XHRcdHN0ZXA6IDFcblx0XHRcdH1cblx0XHR9O1xuXHRcdFsnaG91cnMnLCAnbWludXRlcycsICdzZWNvbmRzJywgJ21pbGxpc2Vjb25kcyddLmZvckVhY2goIGZ1bmN0aW9uKCB0eXBlICkge1xuXHRcdFx0YXNzaWduKG1lLnRpbWVDb25zdHJhaW50c1sgdHlwZSBdLCBtZS5wcm9wcy50aW1lQ29uc3RyYWludHNbIHR5cGUgXSk7XG5cdFx0fSk7XG5cdFx0dGhpcy5zZXRTdGF0ZSggdGhpcy5jYWxjdWxhdGVTdGF0ZSggdGhpcy5wcm9wcyApICk7XG5cdH0sXG5cblx0Y29tcG9uZW50V2lsbFJlY2VpdmVQcm9wczogZnVuY3Rpb24oIG5leHRQcm9wcyApIHtcblx0XHR0aGlzLnNldFN0YXRlKCB0aGlzLmNhbGN1bGF0ZVN0YXRlKCBuZXh0UHJvcHMgKSApO1xuXHR9LFxuXG5cdHVwZGF0ZU1pbGxpOiBmdW5jdGlvbiggZSApIHtcblx0XHR2YXIgbWlsbGkgPSBwYXJzZUludCggZS50YXJnZXQudmFsdWUsIDEwICk7XG5cdFx0aWYgKCBtaWxsaSA9PT0gZS50YXJnZXQudmFsdWUgJiYgbWlsbGkgPj0gMCAmJiBtaWxsaSA8IDEwMDAgKSB7XG5cdFx0XHR0aGlzLnByb3BzLnNldFRpbWUoICdtaWxsaXNlY29uZHMnLCBtaWxsaSApO1xuXHRcdFx0dGhpcy5zZXRTdGF0ZSggeyBtaWxsaXNlY29uZHM6IG1pbGxpIH0gKTtcblx0XHR9XG5cdH0sXG5cblx0cmVuZGVySGVhZGVyOiBmdW5jdGlvbigpIHtcblx0XHRpZiAoICF0aGlzLnByb3BzLmRhdGVGb3JtYXQgKVxuXHRcdFx0cmV0dXJuIG51bGw7XG5cblx0XHR2YXIgZGF0ZSA9IHRoaXMucHJvcHMuc2VsZWN0ZWREYXRlIHx8IHRoaXMucHJvcHMudmlld0RhdGU7XG5cdFx0cmV0dXJuIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoZWFkJywgeyBrZXk6ICdoJyB9LCBSZWFjdC5jcmVhdGVFbGVtZW50KCd0cicsIHt9LFxuXHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGgnLCB7IGNsYXNzTmFtZTogJ3JkdFN3aXRjaCcsIGNvbFNwYW46IDQsIG9uQ2xpY2s6IHRoaXMucHJvcHMuc2hvd1ZpZXcoICdkYXlzJyApIH0sIGRhdGUuZm9ybWF0KCB0aGlzLnByb3BzLmRhdGVGb3JtYXQgKSApXG5cdFx0KSk7XG5cdH0sXG5cblx0b25TdGFydENsaWNraW5nOiBmdW5jdGlvbiggYWN0aW9uLCB0eXBlICkge1xuXHRcdHZhciBtZSA9IHRoaXM7XG5cblx0XHRyZXR1cm4gZnVuY3Rpb24oKSB7XG5cdFx0XHR2YXIgdXBkYXRlID0ge307XG5cdFx0XHR1cGRhdGVbIHR5cGUgXSA9IG1lWyBhY3Rpb24gXSggdHlwZSApO1xuXHRcdFx0bWUuc2V0U3RhdGUoIHVwZGF0ZSApO1xuXG5cdFx0XHRtZS50aW1lciA9IHNldFRpbWVvdXQoIGZ1bmN0aW9uKCkge1xuXHRcdFx0XHRtZS5pbmNyZWFzZVRpbWVyID0gc2V0SW50ZXJ2YWwoIGZ1bmN0aW9uKCkge1xuXHRcdFx0XHRcdHVwZGF0ZVsgdHlwZSBdID0gbWVbIGFjdGlvbiBdKCB0eXBlICk7XG5cdFx0XHRcdFx0bWUuc2V0U3RhdGUoIHVwZGF0ZSApO1xuXHRcdFx0XHR9LCA3MCk7XG5cdFx0XHR9LCA1MDApO1xuXG5cdFx0XHRtZS5tb3VzZVVwTGlzdGVuZXIgPSBmdW5jdGlvbigpIHtcblx0XHRcdFx0Y2xlYXJUaW1lb3V0KCBtZS50aW1lciApO1xuXHRcdFx0XHRjbGVhckludGVydmFsKCBtZS5pbmNyZWFzZVRpbWVyICk7XG5cdFx0XHRcdG1lLnByb3BzLnNldFRpbWUoIHR5cGUsIG1lLnN0YXRlWyB0eXBlIF0gKTtcblx0XHRcdFx0ZG9jdW1lbnQuYm9keS5yZW1vdmVFdmVudExpc3RlbmVyKCAnbW91c2V1cCcsIG1lLm1vdXNlVXBMaXN0ZW5lciApO1xuXHRcdFx0XHRkb2N1bWVudC5ib2R5LnJlbW92ZUV2ZW50TGlzdGVuZXIoICd0b3VjaGVuZCcsIG1lLm1vdXNlVXBMaXN0ZW5lciApO1xuXHRcdFx0fTtcblxuXHRcdFx0ZG9jdW1lbnQuYm9keS5hZGRFdmVudExpc3RlbmVyKCAnbW91c2V1cCcsIG1lLm1vdXNlVXBMaXN0ZW5lciApO1xuXHRcdFx0ZG9jdW1lbnQuYm9keS5hZGRFdmVudExpc3RlbmVyKCAndG91Y2hlbmQnLCBtZS5tb3VzZVVwTGlzdGVuZXIgKTtcblx0XHR9O1xuXHR9LFxuXG5cdGRpc2FibGVDb250ZXh0TWVudTogZnVuY3Rpb24oIGV2ZW50ICkge1xuXHRcdGV2ZW50LnByZXZlbnREZWZhdWx0KCk7XG5cdFx0cmV0dXJuIGZhbHNlO1xuXHR9LFxuXG5cdHBhZFZhbHVlczoge1xuXHRcdGhvdXJzOiAxLFxuXHRcdG1pbnV0ZXM6IDIsXG5cdFx0c2Vjb25kczogMixcblx0XHRtaWxsaXNlY29uZHM6IDNcblx0fSxcblxuXHR0b2dnbGVEYXlQYXJ0OiBmdW5jdGlvbiggdHlwZSApIHsgLy8gdHlwZSBpcyBhbHdheXMgJ2hvdXJzJ1xuXHRcdHZhciB2YWx1ZSA9IHBhcnNlSW50KCB0aGlzLnN0YXRlWyB0eXBlIF0sIDEwKSArIDEyO1xuXHRcdGlmICggdmFsdWUgPiB0aGlzLnRpbWVDb25zdHJhaW50c1sgdHlwZSBdLm1heCApXG5cdFx0XHR2YWx1ZSA9IHRoaXMudGltZUNvbnN0cmFpbnRzWyB0eXBlIF0ubWluICsgKCB2YWx1ZSAtICggdGhpcy50aW1lQ29uc3RyYWludHNbIHR5cGUgXS5tYXggKyAxICkgKTtcblx0XHRyZXR1cm4gdGhpcy5wYWQoIHR5cGUsIHZhbHVlICk7XG5cdH0sXG5cblx0aW5jcmVhc2U6IGZ1bmN0aW9uKCB0eXBlICkge1xuXHRcdHZhciB2YWx1ZSA9IHBhcnNlSW50KCB0aGlzLnN0YXRlWyB0eXBlIF0sIDEwKSArIHRoaXMudGltZUNvbnN0cmFpbnRzWyB0eXBlIF0uc3RlcDtcblx0XHRpZiAoIHZhbHVlID4gdGhpcy50aW1lQ29uc3RyYWludHNbIHR5cGUgXS5tYXggKVxuXHRcdFx0dmFsdWUgPSB0aGlzLnRpbWVDb25zdHJhaW50c1sgdHlwZSBdLm1pbiArICggdmFsdWUgLSAoIHRoaXMudGltZUNvbnN0cmFpbnRzWyB0eXBlIF0ubWF4ICsgMSApICk7XG5cdFx0cmV0dXJuIHRoaXMucGFkKCB0eXBlLCB2YWx1ZSApO1xuXHR9LFxuXG5cdGRlY3JlYXNlOiBmdW5jdGlvbiggdHlwZSApIHtcblx0XHR2YXIgdmFsdWUgPSBwYXJzZUludCggdGhpcy5zdGF0ZVsgdHlwZSBdLCAxMCkgLSB0aGlzLnRpbWVDb25zdHJhaW50c1sgdHlwZSBdLnN0ZXA7XG5cdFx0aWYgKCB2YWx1ZSA8IHRoaXMudGltZUNvbnN0cmFpbnRzWyB0eXBlIF0ubWluIClcblx0XHRcdHZhbHVlID0gdGhpcy50aW1lQ29uc3RyYWludHNbIHR5cGUgXS5tYXggKyAxIC0gKCB0aGlzLnRpbWVDb25zdHJhaW50c1sgdHlwZSBdLm1pbiAtIHZhbHVlICk7XG5cdFx0cmV0dXJuIHRoaXMucGFkKCB0eXBlLCB2YWx1ZSApO1xuXHR9LFxuXG5cdHBhZDogZnVuY3Rpb24oIHR5cGUsIHZhbHVlICkge1xuXHRcdHZhciBzdHIgPSB2YWx1ZSArICcnO1xuXHRcdHdoaWxlICggc3RyLmxlbmd0aCA8IHRoaXMucGFkVmFsdWVzWyB0eXBlIF0gKVxuXHRcdFx0c3RyID0gJzAnICsgc3RyO1xuXHRcdHJldHVybiBzdHI7XG5cdH0sXG5cblx0aGFuZGxlQ2xpY2tPdXRzaWRlOiBmdW5jdGlvbigpIHtcblx0XHR0aGlzLnByb3BzLmhhbmRsZUNsaWNrT3V0c2lkZSgpO1xuXHR9XG59KSk7XG5cbm1vZHVsZS5leHBvcnRzID0gRGF0ZVRpbWVQaWNrZXJUaW1lO1xuIiwiJ3VzZSBzdHJpY3QnO1xuXG52YXIgUmVhY3QgPSByZXF1aXJlKCdyZWFjdCcpLFxuXHRjcmVhdGVDbGFzcyA9IHJlcXVpcmUoJ2NyZWF0ZS1yZWFjdC1jbGFzcycpLFxuXHRvbkNsaWNrT3V0c2lkZSA9IHJlcXVpcmUoJ3JlYWN0LW9uY2xpY2tvdXRzaWRlJykuZGVmYXVsdFxuXHQ7XG5cbnZhciBEYXRlVGltZVBpY2tlclllYXJzID0gb25DbGlja091dHNpZGUoIGNyZWF0ZUNsYXNzKHtcblx0cmVuZGVyOiBmdW5jdGlvbigpIHtcblx0XHR2YXIgeWVhciA9IHBhcnNlSW50KCB0aGlzLnByb3BzLnZpZXdEYXRlLnllYXIoKSAvIDEwLCAxMCApICogMTA7XG5cblx0XHRyZXR1cm4gUmVhY3QuY3JlYXRlRWxlbWVudCgnZGl2JywgeyBjbGFzc05hbWU6ICdyZHRZZWFycycgfSwgW1xuXHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGFibGUnLCB7IGtleTogJ2EnIH0sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoZWFkJywge30sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RyJywge30sIFtcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGgnLCB7IGtleTogJ3ByZXYnLCBjbGFzc05hbWU6ICdyZHRQcmV2Jywgb25DbGljazogdGhpcy5wcm9wcy5zdWJ0cmFjdFRpbWUoIDEwLCAneWVhcnMnICl9LCBSZWFjdC5jcmVhdGVFbGVtZW50KCdzcGFuJywge30sICfigLknICkpLFxuXHRcdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0aCcsIHsga2V5OiAneWVhcicsIGNsYXNzTmFtZTogJ3JkdFN3aXRjaCcsIG9uQ2xpY2s6IHRoaXMucHJvcHMuc2hvd1ZpZXcoICd5ZWFycycgKSwgY29sU3BhbjogMiB9LCB5ZWFyICsgJy0nICsgKCB5ZWFyICsgOSApICksXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoJywgeyBrZXk6ICduZXh0JywgY2xhc3NOYW1lOiAncmR0TmV4dCcsIG9uQ2xpY2s6IHRoaXMucHJvcHMuYWRkVGltZSggMTAsICd5ZWFycycgKX0sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3NwYW4nLCB7fSwgJ+KAuicgKSlcblx0XHRcdF0pKSksXG5cdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0YWJsZScsIHsga2V5OiAneWVhcnMnIH0sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3Rib2R5JywgIHt9LCB0aGlzLnJlbmRlclllYXJzKCB5ZWFyICkpKVxuXHRcdF0pO1xuXHR9LFxuXG5cdHJlbmRlclllYXJzOiBmdW5jdGlvbiggeWVhciApIHtcblx0XHR2YXIgeWVhcnMgPSBbXSxcblx0XHRcdGkgPSAtMSxcblx0XHRcdHJvd3MgPSBbXSxcblx0XHRcdHJlbmRlcmVyID0gdGhpcy5wcm9wcy5yZW5kZXJZZWFyIHx8IHRoaXMucmVuZGVyWWVhcixcblx0XHRcdHNlbGVjdGVkRGF0ZSA9IHRoaXMucHJvcHMuc2VsZWN0ZWREYXRlLFxuXHRcdFx0aXNWYWxpZCA9IHRoaXMucHJvcHMuaXNWYWxpZERhdGUgfHwgdGhpcy5hbHdheXNWYWxpZERhdGUsXG5cdFx0XHRjbGFzc2VzLCBwcm9wcywgY3VycmVudFllYXIsIGlzRGlzYWJsZWQsIG5vT2ZEYXlzSW5ZZWFyLCBkYXlzSW5ZZWFyLCB2YWxpZERheSxcblx0XHRcdC8vIE1vbnRoIGFuZCBkYXRlIGFyZSBpcnJlbGV2YW50IGhlcmUgYmVjYXVzZVxuXHRcdFx0Ly8gd2UncmUgb25seSBpbnRlcmVzdGVkIGluIHRoZSB5ZWFyXG5cdFx0XHRpcnJlbGV2YW50TW9udGggPSAwLFxuXHRcdFx0aXJyZWxldmFudERhdGUgPSAxXG5cdFx0XHQ7XG5cblx0XHR5ZWFyLS07XG5cdFx0d2hpbGUgKGkgPCAxMSkge1xuXHRcdFx0Y2xhc3NlcyA9ICdyZHRZZWFyJztcblx0XHRcdGN1cnJlbnRZZWFyID0gdGhpcy5wcm9wcy52aWV3RGF0ZS5jbG9uZSgpLnNldChcblx0XHRcdFx0eyB5ZWFyOiB5ZWFyLCBtb250aDogaXJyZWxldmFudE1vbnRoLCBkYXRlOiBpcnJlbGV2YW50RGF0ZSB9ICk7XG5cblx0XHRcdC8vIE5vdCBzdXJlIHdoYXQgJ3JkdE9sZCcgaXMgZm9yLCBjb21tZW50aW5nIG91dCBmb3Igbm93IGFzIGl0J3Mgbm90IHdvcmtpbmcgcHJvcGVybHlcblx0XHRcdC8vIGlmICggaSA9PT0gLTEgfCBpID09PSAxMCApXG5cdFx0XHRcdC8vIGNsYXNzZXMgKz0gJyByZHRPbGQnO1xuXG5cdFx0XHRub09mRGF5c0luWWVhciA9IGN1cnJlbnRZZWFyLmVuZE9mKCAneWVhcicgKS5mb3JtYXQoICdEREQnICk7XG5cdFx0XHRkYXlzSW5ZZWFyID0gQXJyYXkuZnJvbSh7IGxlbmd0aDogbm9PZkRheXNJblllYXIgfSwgZnVuY3Rpb24oIGUsIGkgKSB7XG5cdFx0XHRcdHJldHVybiBpICsgMTtcblx0XHRcdH0pO1xuXG5cdFx0XHR2YWxpZERheSA9IGRheXNJblllYXIuZmluZChmdW5jdGlvbiggZCApIHtcblx0XHRcdFx0dmFyIGRheSA9IGN1cnJlbnRZZWFyLmNsb25lKCkuZGF5T2ZZZWFyKCBkICk7XG5cdFx0XHRcdHJldHVybiBpc1ZhbGlkKCBkYXkgKTtcblx0XHRcdH0pO1xuXG5cdFx0XHRpc0Rpc2FibGVkID0gKCB2YWxpZERheSA9PT0gdW5kZWZpbmVkICk7XG5cblx0XHRcdGlmICggaXNEaXNhYmxlZCApXG5cdFx0XHRcdGNsYXNzZXMgKz0gJyByZHREaXNhYmxlZCc7XG5cblx0XHRcdGlmICggc2VsZWN0ZWREYXRlICYmIHNlbGVjdGVkRGF0ZS55ZWFyKCkgPT09IHllYXIgKVxuXHRcdFx0XHRjbGFzc2VzICs9ICcgcmR0QWN0aXZlJztcblxuXHRcdFx0cHJvcHMgPSB7XG5cdFx0XHRcdGtleTogeWVhcixcblx0XHRcdFx0J2RhdGEtdmFsdWUnOiB5ZWFyLFxuXHRcdFx0XHRjbGFzc05hbWU6IGNsYXNzZXNcblx0XHRcdH07XG5cblx0XHRcdGlmICggIWlzRGlzYWJsZWQgKVxuXHRcdFx0XHRwcm9wcy5vbkNsaWNrID0gKCB0aGlzLnByb3BzLnVwZGF0ZU9uID09PSAneWVhcnMnID9cblx0XHRcdFx0XHR0aGlzLnVwZGF0ZVNlbGVjdGVkWWVhciA6IHRoaXMucHJvcHMuc2V0RGF0ZSgneWVhcicpICk7XG5cblx0XHRcdHllYXJzLnB1c2goIHJlbmRlcmVyKCBwcm9wcywgeWVhciwgc2VsZWN0ZWREYXRlICYmIHNlbGVjdGVkRGF0ZS5jbG9uZSgpICkpO1xuXG5cdFx0XHRpZiAoIHllYXJzLmxlbmd0aCA9PT0gNCApIHtcblx0XHRcdFx0cm93cy5wdXNoKCBSZWFjdC5jcmVhdGVFbGVtZW50KCd0cicsIHsga2V5OiBpIH0sIHllYXJzICkgKTtcblx0XHRcdFx0eWVhcnMgPSBbXTtcblx0XHRcdH1cblxuXHRcdFx0eWVhcisrO1xuXHRcdFx0aSsrO1xuXHRcdH1cblxuXHRcdHJldHVybiByb3dzO1xuXHR9LFxuXG5cdHVwZGF0ZVNlbGVjdGVkWWVhcjogZnVuY3Rpb24oIGV2ZW50ICkge1xuXHRcdHRoaXMucHJvcHMudXBkYXRlU2VsZWN0ZWREYXRlKCBldmVudCApO1xuXHR9LFxuXG5cdHJlbmRlclllYXI6IGZ1bmN0aW9uKCBwcm9wcywgeWVhciApIHtcblx0XHRyZXR1cm4gUmVhY3QuY3JlYXRlRWxlbWVudCgndGQnLCAgcHJvcHMsIHllYXIgKTtcblx0fSxcblxuXHRhbHdheXNWYWxpZERhdGU6IGZ1bmN0aW9uKCkge1xuXHRcdHJldHVybiAxO1xuXHR9LFxuXG5cdGhhbmRsZUNsaWNrT3V0c2lkZTogZnVuY3Rpb24oKSB7XG5cdFx0dGhpcy5wcm9wcy5oYW5kbGVDbGlja091dHNpZGUoKTtcblx0fVxufSkpO1xuXG5tb2R1bGUuZXhwb3J0cyA9IERhdGVUaW1lUGlja2VyWWVhcnM7XG4iLCJpbXBvcnQgeyBDb21wb25lbnQsIGNyZWF0ZUVsZW1lbnQgfSBmcm9tICdyZWFjdCc7XG5pbXBvcnQgeyBmaW5kRE9NTm9kZSB9IGZyb20gJ3JlYWN0LWRvbSc7XG5cbmZ1bmN0aW9uIF9pbmhlcml0c0xvb3NlKHN1YkNsYXNzLCBzdXBlckNsYXNzKSB7XG4gIHN1YkNsYXNzLnByb3RvdHlwZSA9IE9iamVjdC5jcmVhdGUoc3VwZXJDbGFzcy5wcm90b3R5cGUpO1xuICBzdWJDbGFzcy5wcm90b3R5cGUuY29uc3RydWN0b3IgPSBzdWJDbGFzcztcbiAgc3ViQ2xhc3MuX19wcm90b19fID0gc3VwZXJDbGFzcztcbn1cblxuZnVuY3Rpb24gX29iamVjdFdpdGhvdXRQcm9wZXJ0aWVzKHNvdXJjZSwgZXhjbHVkZWQpIHtcbiAgaWYgKHNvdXJjZSA9PSBudWxsKSByZXR1cm4ge307XG4gIHZhciB0YXJnZXQgPSB7fTtcbiAgdmFyIHNvdXJjZUtleXMgPSBPYmplY3Qua2V5cyhzb3VyY2UpO1xuICB2YXIga2V5LCBpO1xuXG4gIGZvciAoaSA9IDA7IGkgPCBzb3VyY2VLZXlzLmxlbmd0aDsgaSsrKSB7XG4gICAga2V5ID0gc291cmNlS2V5c1tpXTtcbiAgICBpZiAoZXhjbHVkZWQuaW5kZXhPZihrZXkpID49IDApIGNvbnRpbnVlO1xuICAgIHRhcmdldFtrZXldID0gc291cmNlW2tleV07XG4gIH1cblxuICBpZiAoT2JqZWN0LmdldE93blByb3BlcnR5U3ltYm9scykge1xuICAgIHZhciBzb3VyY2VTeW1ib2xLZXlzID0gT2JqZWN0LmdldE93blByb3BlcnR5U3ltYm9scyhzb3VyY2UpO1xuXG4gICAgZm9yIChpID0gMDsgaSA8IHNvdXJjZVN5bWJvbEtleXMubGVuZ3RoOyBpKyspIHtcbiAgICAgIGtleSA9IHNvdXJjZVN5bWJvbEtleXNbaV07XG4gICAgICBpZiAoZXhjbHVkZWQuaW5kZXhPZihrZXkpID49IDApIGNvbnRpbnVlO1xuICAgICAgaWYgKCFPYmplY3QucHJvdG90eXBlLnByb3BlcnR5SXNFbnVtZXJhYmxlLmNhbGwoc291cmNlLCBrZXkpKSBjb250aW51ZTtcbiAgICAgIHRhcmdldFtrZXldID0gc291cmNlW2tleV07XG4gICAgfVxuICB9XG5cbiAgcmV0dXJuIHRhcmdldDtcbn1cblxuLyoqXG4gKiBDaGVjayB3aGV0aGVyIHNvbWUgRE9NIG5vZGUgaXMgb3VyIENvbXBvbmVudCdzIG5vZGUuXG4gKi9cbmZ1bmN0aW9uIGlzTm9kZUZvdW5kKGN1cnJlbnQsIGNvbXBvbmVudE5vZGUsIGlnbm9yZUNsYXNzKSB7XG4gIGlmIChjdXJyZW50ID09PSBjb21wb25lbnROb2RlKSB7XG4gICAgcmV0dXJuIHRydWU7XG4gIH0gLy8gU1ZHIDx1c2UvPiBlbGVtZW50cyBkbyBub3QgdGVjaG5pY2FsbHkgcmVzaWRlIGluIHRoZSByZW5kZXJlZCBET00sIHNvXG4gIC8vIHRoZXkgZG8gbm90IGhhdmUgY2xhc3NMaXN0IGRpcmVjdGx5LCBidXQgdGhleSBvZmZlciBhIGxpbmsgdG8gdGhlaXJcbiAgLy8gY29ycmVzcG9uZGluZyBlbGVtZW50LCB3aGljaCBjYW4gaGF2ZSBjbGFzc0xpc3QuIFRoaXMgZXh0cmEgY2hlY2sgaXMgZm9yXG4gIC8vIHRoYXQgY2FzZS5cbiAgLy8gU2VlOiBodHRwOi8vd3d3LnczLm9yZy9UUi9TVkcxMS9zdHJ1Y3QuaHRtbCNJbnRlcmZhY2VTVkdVc2VFbGVtZW50XG4gIC8vIERpc2N1c3Npb246IGh0dHBzOi8vZ2l0aHViLmNvbS9Qb21heC9yZWFjdC1vbmNsaWNrb3V0c2lkZS9wdWxsLzE3XG5cblxuICBpZiAoY3VycmVudC5jb3JyZXNwb25kaW5nRWxlbWVudCkge1xuICAgIHJldHVybiBjdXJyZW50LmNvcnJlc3BvbmRpbmdFbGVtZW50LmNsYXNzTGlzdC5jb250YWlucyhpZ25vcmVDbGFzcyk7XG4gIH1cblxuICByZXR1cm4gY3VycmVudC5jbGFzc0xpc3QuY29udGFpbnMoaWdub3JlQ2xhc3MpO1xufVxuLyoqXG4gKiBUcnkgdG8gZmluZCBvdXIgbm9kZSBpbiBhIGhpZXJhcmNoeSBvZiBub2RlcywgcmV0dXJuaW5nIHRoZSBkb2N1bWVudFxuICogbm9kZSBhcyBoaWdoZXN0IG5vZGUgaWYgb3VyIG5vZGUgaXMgbm90IGZvdW5kIGluIHRoZSBwYXRoIHVwLlxuICovXG5cbmZ1bmN0aW9uIGZpbmRIaWdoZXN0KGN1cnJlbnQsIGNvbXBvbmVudE5vZGUsIGlnbm9yZUNsYXNzKSB7XG4gIGlmIChjdXJyZW50ID09PSBjb21wb25lbnROb2RlKSB7XG4gICAgcmV0dXJuIHRydWU7XG4gIH0gLy8gSWYgc291cmNlPWxvY2FsIHRoZW4gdGhpcyBldmVudCBjYW1lIGZyb20gJ3NvbWV3aGVyZSdcbiAgLy8gaW5zaWRlIGFuZCBzaG91bGQgYmUgaWdub3JlZC4gV2UgY291bGQgaGFuZGxlIHRoaXMgd2l0aFxuICAvLyBhIGxheWVyZWQgYXBwcm9hY2gsIHRvbywgYnV0IHRoYXQgcmVxdWlyZXMgZ29pbmcgYmFjayB0b1xuICAvLyB0aGlua2luZyBpbiB0ZXJtcyBvZiBEb20gbm9kZSBuZXN0aW5nLCBydW5uaW5nIGNvdW50ZXJcbiAgLy8gdG8gUmVhY3QncyAneW91IHNob3VsZG4ndCBjYXJlIGFib3V0IHRoZSBET00nIHBoaWxvc29waHkuXG5cblxuICB3aGlsZSAoY3VycmVudC5wYXJlbnROb2RlKSB7XG4gICAgaWYgKGlzTm9kZUZvdW5kKGN1cnJlbnQsIGNvbXBvbmVudE5vZGUsIGlnbm9yZUNsYXNzKSkge1xuICAgICAgcmV0dXJuIHRydWU7XG4gICAgfVxuXG4gICAgY3VycmVudCA9IGN1cnJlbnQucGFyZW50Tm9kZTtcbiAgfVxuXG4gIHJldHVybiBjdXJyZW50O1xufVxuLyoqXG4gKiBDaGVjayBpZiB0aGUgYnJvd3NlciBzY3JvbGxiYXIgd2FzIGNsaWNrZWRcbiAqL1xuXG5mdW5jdGlvbiBjbGlja2VkU2Nyb2xsYmFyKGV2dCkge1xuICByZXR1cm4gZG9jdW1lbnQuZG9jdW1lbnRFbGVtZW50LmNsaWVudFdpZHRoIDw9IGV2dC5jbGllbnRYIHx8IGRvY3VtZW50LmRvY3VtZW50RWxlbWVudC5jbGllbnRIZWlnaHQgPD0gZXZ0LmNsaWVudFk7XG59XG5cbi8vIGlkZWFsbHkgd2lsbCBnZXQgcmVwbGFjZWQgd2l0aCBleHRlcm5hbCBkZXBcbi8vIHdoZW4gcmFmcmV4L2RldGVjdC1wYXNzaXZlLWV2ZW50cyM0IGFuZCByYWZyZXgvZGV0ZWN0LXBhc3NpdmUtZXZlbnRzIzUgZ2V0IG1lcmdlZCBpblxudmFyIHRlc3RQYXNzaXZlRXZlbnRTdXBwb3J0ID0gZnVuY3Rpb24gdGVzdFBhc3NpdmVFdmVudFN1cHBvcnQoKSB7XG4gIGlmICh0eXBlb2Ygd2luZG93ID09PSAndW5kZWZpbmVkJyB8fCB0eXBlb2Ygd2luZG93LmFkZEV2ZW50TGlzdGVuZXIgIT09ICdmdW5jdGlvbicpIHtcbiAgICByZXR1cm47XG4gIH1cblxuICB2YXIgcGFzc2l2ZSA9IGZhbHNlO1xuICB2YXIgb3B0aW9ucyA9IE9iamVjdC5kZWZpbmVQcm9wZXJ0eSh7fSwgJ3Bhc3NpdmUnLCB7XG4gICAgZ2V0OiBmdW5jdGlvbiBnZXQoKSB7XG4gICAgICBwYXNzaXZlID0gdHJ1ZTtcbiAgICB9XG4gIH0pO1xuXG4gIHZhciBub29wID0gZnVuY3Rpb24gbm9vcCgpIHt9O1xuXG4gIHdpbmRvdy5hZGRFdmVudExpc3RlbmVyKCd0ZXN0UGFzc2l2ZUV2ZW50U3VwcG9ydCcsIG5vb3AsIG9wdGlvbnMpO1xuICB3aW5kb3cucmVtb3ZlRXZlbnRMaXN0ZW5lcigndGVzdFBhc3NpdmVFdmVudFN1cHBvcnQnLCBub29wLCBvcHRpb25zKTtcbiAgcmV0dXJuIHBhc3NpdmU7XG59O1xuXG5mdW5jdGlvbiBhdXRvSW5jKHNlZWQpIHtcbiAgaWYgKHNlZWQgPT09IHZvaWQgMCkge1xuICAgIHNlZWQgPSAwO1xuICB9XG5cbiAgcmV0dXJuIGZ1bmN0aW9uICgpIHtcbiAgICByZXR1cm4gKytzZWVkO1xuICB9O1xufVxuXG52YXIgdWlkID0gYXV0b0luYygpO1xuXG52YXIgcGFzc2l2ZUV2ZW50U3VwcG9ydDtcbnZhciBoYW5kbGVyc01hcCA9IHt9O1xudmFyIGVuYWJsZWRJbnN0YW5jZXMgPSB7fTtcbnZhciB0b3VjaEV2ZW50cyA9IFsndG91Y2hzdGFydCcsICd0b3VjaG1vdmUnXTtcbnZhciBJR05PUkVfQ0xBU1NfTkFNRSA9ICdpZ25vcmUtcmVhY3Qtb25jbGlja291dHNpZGUnO1xuLyoqXG4gKiBPcHRpb25zIGZvciBhZGRFdmVudEhhbmRsZXIgYW5kIHJlbW92ZUV2ZW50SGFuZGxlclxuICovXG5cbmZ1bmN0aW9uIGdldEV2ZW50SGFuZGxlck9wdGlvbnMoaW5zdGFuY2UsIGV2ZW50TmFtZSkge1xuICB2YXIgaGFuZGxlck9wdGlvbnMgPSBudWxsO1xuICB2YXIgaXNUb3VjaEV2ZW50ID0gdG91Y2hFdmVudHMuaW5kZXhPZihldmVudE5hbWUpICE9PSAtMTtcblxuICBpZiAoaXNUb3VjaEV2ZW50ICYmIHBhc3NpdmVFdmVudFN1cHBvcnQpIHtcbiAgICBoYW5kbGVyT3B0aW9ucyA9IHtcbiAgICAgIHBhc3NpdmU6ICFpbnN0YW5jZS5wcm9wcy5wcmV2ZW50RGVmYXVsdFxuICAgIH07XG4gIH1cblxuICByZXR1cm4gaGFuZGxlck9wdGlvbnM7XG59XG4vKipcbiAqIFRoaXMgZnVuY3Rpb24gZ2VuZXJhdGVzIHRoZSBIT0MgZnVuY3Rpb24gdGhhdCB5b3UnbGwgdXNlXG4gKiBpbiBvcmRlciB0byBpbXBhcnQgb25PdXRzaWRlQ2xpY2sgbGlzdGVuaW5nIHRvIGFuXG4gKiBhcmJpdHJhcnkgY29tcG9uZW50LiBJdCBnZXRzIGNhbGxlZCBhdCB0aGUgZW5kIG9mIHRoZVxuICogYm9vdHN0cmFwcGluZyBjb2RlIHRvIHlpZWxkIGFuIGluc3RhbmNlIG9mIHRoZVxuICogb25DbGlja091dHNpZGVIT0MgZnVuY3Rpb24gZGVmaW5lZCBpbnNpZGUgc2V0dXBIT0MoKS5cbiAqL1xuXG5cbmZ1bmN0aW9uIG9uQ2xpY2tPdXRzaWRlSE9DKFdyYXBwZWRDb21wb25lbnQsIGNvbmZpZykge1xuICB2YXIgX2NsYXNzLCBfdGVtcDtcblxuICB2YXIgY29tcG9uZW50TmFtZSA9IFdyYXBwZWRDb21wb25lbnQuZGlzcGxheU5hbWUgfHwgV3JhcHBlZENvbXBvbmVudC5uYW1lIHx8ICdDb21wb25lbnQnO1xuICByZXR1cm4gX3RlbXAgPSBfY2xhc3MgPVxuICAvKiNfX1BVUkVfXyovXG4gIGZ1bmN0aW9uIChfQ29tcG9uZW50KSB7XG4gICAgX2luaGVyaXRzTG9vc2Uob25DbGlja091dHNpZGUsIF9Db21wb25lbnQpO1xuXG4gICAgZnVuY3Rpb24gb25DbGlja091dHNpZGUocHJvcHMpIHtcbiAgICAgIHZhciBfdGhpcztcblxuICAgICAgX3RoaXMgPSBfQ29tcG9uZW50LmNhbGwodGhpcywgcHJvcHMpIHx8IHRoaXM7XG5cbiAgICAgIF90aGlzLl9fb3V0c2lkZUNsaWNrSGFuZGxlciA9IGZ1bmN0aW9uIChldmVudCkge1xuICAgICAgICBpZiAodHlwZW9mIF90aGlzLl9fY2xpY2tPdXRzaWRlSGFuZGxlclByb3AgPT09ICdmdW5jdGlvbicpIHtcbiAgICAgICAgICBfdGhpcy5fX2NsaWNrT3V0c2lkZUhhbmRsZXJQcm9wKGV2ZW50KTtcblxuICAgICAgICAgIHJldHVybjtcbiAgICAgICAgfVxuXG4gICAgICAgIHZhciBpbnN0YW5jZSA9IF90aGlzLmdldEluc3RhbmNlKCk7XG5cbiAgICAgICAgaWYgKHR5cGVvZiBpbnN0YW5jZS5wcm9wcy5oYW5kbGVDbGlja091dHNpZGUgPT09ICdmdW5jdGlvbicpIHtcbiAgICAgICAgICBpbnN0YW5jZS5wcm9wcy5oYW5kbGVDbGlja091dHNpZGUoZXZlbnQpO1xuICAgICAgICAgIHJldHVybjtcbiAgICAgICAgfVxuXG4gICAgICAgIGlmICh0eXBlb2YgaW5zdGFuY2UuaGFuZGxlQ2xpY2tPdXRzaWRlID09PSAnZnVuY3Rpb24nKSB7XG4gICAgICAgICAgaW5zdGFuY2UuaGFuZGxlQ2xpY2tPdXRzaWRlKGV2ZW50KTtcbiAgICAgICAgICByZXR1cm47XG4gICAgICAgIH1cblxuICAgICAgICB0aHJvdyBuZXcgRXJyb3IoXCJXcmFwcGVkQ29tcG9uZW50OiBcIiArIGNvbXBvbmVudE5hbWUgKyBcIiBsYWNrcyBhIGhhbmRsZUNsaWNrT3V0c2lkZShldmVudCkgZnVuY3Rpb24gZm9yIHByb2Nlc3Npbmcgb3V0c2lkZSBjbGljayBldmVudHMuXCIpO1xuICAgICAgfTtcblxuICAgICAgX3RoaXMuX19nZXRDb21wb25lbnROb2RlID0gZnVuY3Rpb24gKCkge1xuICAgICAgICB2YXIgaW5zdGFuY2UgPSBfdGhpcy5nZXRJbnN0YW5jZSgpO1xuXG4gICAgICAgIGlmIChjb25maWcgJiYgdHlwZW9mIGNvbmZpZy5zZXRDbGlja091dHNpZGVSZWYgPT09ICdmdW5jdGlvbicpIHtcbiAgICAgICAgICByZXR1cm4gY29uZmlnLnNldENsaWNrT3V0c2lkZVJlZigpKGluc3RhbmNlKTtcbiAgICAgICAgfVxuXG4gICAgICAgIGlmICh0eXBlb2YgaW5zdGFuY2Uuc2V0Q2xpY2tPdXRzaWRlUmVmID09PSAnZnVuY3Rpb24nKSB7XG4gICAgICAgICAgcmV0dXJuIGluc3RhbmNlLnNldENsaWNrT3V0c2lkZVJlZigpO1xuICAgICAgICB9XG5cbiAgICAgICAgcmV0dXJuIGZpbmRET01Ob2RlKGluc3RhbmNlKTtcbiAgICAgIH07XG5cbiAgICAgIF90aGlzLmVuYWJsZU9uQ2xpY2tPdXRzaWRlID0gZnVuY3Rpb24gKCkge1xuICAgICAgICBpZiAodHlwZW9mIGRvY3VtZW50ID09PSAndW5kZWZpbmVkJyB8fCBlbmFibGVkSW5zdGFuY2VzW190aGlzLl91aWRdKSB7XG4gICAgICAgICAgcmV0dXJuO1xuICAgICAgICB9XG5cbiAgICAgICAgaWYgKHR5cGVvZiBwYXNzaXZlRXZlbnRTdXBwb3J0ID09PSAndW5kZWZpbmVkJykge1xuICAgICAgICAgIHBhc3NpdmVFdmVudFN1cHBvcnQgPSB0ZXN0UGFzc2l2ZUV2ZW50U3VwcG9ydCgpO1xuICAgICAgICB9XG5cbiAgICAgICAgZW5hYmxlZEluc3RhbmNlc1tfdGhpcy5fdWlkXSA9IHRydWU7XG4gICAgICAgIHZhciBldmVudHMgPSBfdGhpcy5wcm9wcy5ldmVudFR5cGVzO1xuXG4gICAgICAgIGlmICghZXZlbnRzLmZvckVhY2gpIHtcbiAgICAgICAgICBldmVudHMgPSBbZXZlbnRzXTtcbiAgICAgICAgfVxuXG4gICAgICAgIGhhbmRsZXJzTWFwW190aGlzLl91aWRdID0gZnVuY3Rpb24gKGV2ZW50KSB7XG4gICAgICAgICAgaWYgKF90aGlzLmNvbXBvbmVudE5vZGUgPT09IG51bGwpIHJldHVybjtcblxuICAgICAgICAgIGlmIChfdGhpcy5wcm9wcy5wcmV2ZW50RGVmYXVsdCkge1xuICAgICAgICAgICAgZXZlbnQucHJldmVudERlZmF1bHQoKTtcbiAgICAgICAgICB9XG5cbiAgICAgICAgICBpZiAoX3RoaXMucHJvcHMuc3RvcFByb3BhZ2F0aW9uKSB7XG4gICAgICAgICAgICBldmVudC5zdG9wUHJvcGFnYXRpb24oKTtcbiAgICAgICAgICB9XG5cbiAgICAgICAgICBpZiAoX3RoaXMucHJvcHMuZXhjbHVkZVNjcm9sbGJhciAmJiBjbGlja2VkU2Nyb2xsYmFyKGV2ZW50KSkgcmV0dXJuO1xuICAgICAgICAgIHZhciBjdXJyZW50ID0gZXZlbnQudGFyZ2V0O1xuXG4gICAgICAgICAgaWYgKGZpbmRIaWdoZXN0KGN1cnJlbnQsIF90aGlzLmNvbXBvbmVudE5vZGUsIF90aGlzLnByb3BzLm91dHNpZGVDbGlja0lnbm9yZUNsYXNzKSAhPT0gZG9jdW1lbnQpIHtcbiAgICAgICAgICAgIHJldHVybjtcbiAgICAgICAgICB9XG5cbiAgICAgICAgICBfdGhpcy5fX291dHNpZGVDbGlja0hhbmRsZXIoZXZlbnQpO1xuICAgICAgICB9O1xuXG4gICAgICAgIGV2ZW50cy5mb3JFYWNoKGZ1bmN0aW9uIChldmVudE5hbWUpIHtcbiAgICAgICAgICBkb2N1bWVudC5hZGRFdmVudExpc3RlbmVyKGV2ZW50TmFtZSwgaGFuZGxlcnNNYXBbX3RoaXMuX3VpZF0sIGdldEV2ZW50SGFuZGxlck9wdGlvbnMoX3RoaXMsIGV2ZW50TmFtZSkpO1xuICAgICAgICB9KTtcbiAgICAgIH07XG5cbiAgICAgIF90aGlzLmRpc2FibGVPbkNsaWNrT3V0c2lkZSA9IGZ1bmN0aW9uICgpIHtcbiAgICAgICAgZGVsZXRlIGVuYWJsZWRJbnN0YW5jZXNbX3RoaXMuX3VpZF07XG4gICAgICAgIHZhciBmbiA9IGhhbmRsZXJzTWFwW190aGlzLl91aWRdO1xuXG4gICAgICAgIGlmIChmbiAmJiB0eXBlb2YgZG9jdW1lbnQgIT09ICd1bmRlZmluZWQnKSB7XG4gICAgICAgICAgdmFyIGV2ZW50cyA9IF90aGlzLnByb3BzLmV2ZW50VHlwZXM7XG5cbiAgICAgICAgICBpZiAoIWV2ZW50cy5mb3JFYWNoKSB7XG4gICAgICAgICAgICBldmVudHMgPSBbZXZlbnRzXTtcbiAgICAgICAgICB9XG5cbiAgICAgICAgICBldmVudHMuZm9yRWFjaChmdW5jdGlvbiAoZXZlbnROYW1lKSB7XG4gICAgICAgICAgICByZXR1cm4gZG9jdW1lbnQucmVtb3ZlRXZlbnRMaXN0ZW5lcihldmVudE5hbWUsIGZuLCBnZXRFdmVudEhhbmRsZXJPcHRpb25zKF90aGlzLCBldmVudE5hbWUpKTtcbiAgICAgICAgICB9KTtcbiAgICAgICAgICBkZWxldGUgaGFuZGxlcnNNYXBbX3RoaXMuX3VpZF07XG4gICAgICAgIH1cbiAgICAgIH07XG5cbiAgICAgIF90aGlzLmdldFJlZiA9IGZ1bmN0aW9uIChyZWYpIHtcbiAgICAgICAgcmV0dXJuIF90aGlzLmluc3RhbmNlUmVmID0gcmVmO1xuICAgICAgfTtcblxuICAgICAgX3RoaXMuX3VpZCA9IHVpZCgpO1xuICAgICAgcmV0dXJuIF90aGlzO1xuICAgIH1cbiAgICAvKipcbiAgICAgKiBBY2Nlc3MgdGhlIFdyYXBwZWRDb21wb25lbnQncyBpbnN0YW5jZS5cbiAgICAgKi9cblxuXG4gICAgdmFyIF9wcm90byA9IG9uQ2xpY2tPdXRzaWRlLnByb3RvdHlwZTtcblxuICAgIF9wcm90by5nZXRJbnN0YW5jZSA9IGZ1bmN0aW9uIGdldEluc3RhbmNlKCkge1xuICAgICAgaWYgKCFXcmFwcGVkQ29tcG9uZW50LnByb3RvdHlwZS5pc1JlYWN0Q29tcG9uZW50KSB7XG4gICAgICAgIHJldHVybiB0aGlzO1xuICAgICAgfVxuXG4gICAgICB2YXIgcmVmID0gdGhpcy5pbnN0YW5jZVJlZjtcbiAgICAgIHJldHVybiByZWYuZ2V0SW5zdGFuY2UgPyByZWYuZ2V0SW5zdGFuY2UoKSA6IHJlZjtcbiAgICB9O1xuXG4gICAgLyoqXG4gICAgICogQWRkIGNsaWNrIGxpc3RlbmVycyB0byB0aGUgY3VycmVudCBkb2N1bWVudCxcbiAgICAgKiBsaW5rZWQgdG8gdGhpcyBjb21wb25lbnQncyBzdGF0ZS5cbiAgICAgKi9cbiAgICBfcHJvdG8uY29tcG9uZW50RGlkTW91bnQgPSBmdW5jdGlvbiBjb21wb25lbnREaWRNb3VudCgpIHtcbiAgICAgIC8vIElmIHdlIGFyZSBpbiBhbiBlbnZpcm9ubWVudCB3aXRob3V0IGEgRE9NIHN1Y2hcbiAgICAgIC8vIGFzIHNoYWxsb3cgcmVuZGVyaW5nIG9yIHNuYXBzaG90cyB0aGVuIHdlIGV4aXRcbiAgICAgIC8vIGVhcmx5IHRvIHByZXZlbnQgYW55IHVuaGFuZGxlZCBlcnJvcnMgYmVpbmcgdGhyb3duLlxuICAgICAgaWYgKHR5cGVvZiBkb2N1bWVudCA9PT0gJ3VuZGVmaW5lZCcgfHwgIWRvY3VtZW50LmNyZWF0ZUVsZW1lbnQpIHtcbiAgICAgICAgcmV0dXJuO1xuICAgICAgfVxuXG4gICAgICB2YXIgaW5zdGFuY2UgPSB0aGlzLmdldEluc3RhbmNlKCk7XG5cbiAgICAgIGlmIChjb25maWcgJiYgdHlwZW9mIGNvbmZpZy5oYW5kbGVDbGlja091dHNpZGUgPT09ICdmdW5jdGlvbicpIHtcbiAgICAgICAgdGhpcy5fX2NsaWNrT3V0c2lkZUhhbmRsZXJQcm9wID0gY29uZmlnLmhhbmRsZUNsaWNrT3V0c2lkZShpbnN0YW5jZSk7XG5cbiAgICAgICAgaWYgKHR5cGVvZiB0aGlzLl9fY2xpY2tPdXRzaWRlSGFuZGxlclByb3AgIT09ICdmdW5jdGlvbicpIHtcbiAgICAgICAgICB0aHJvdyBuZXcgRXJyb3IoXCJXcmFwcGVkQ29tcG9uZW50OiBcIiArIGNvbXBvbmVudE5hbWUgKyBcIiBsYWNrcyBhIGZ1bmN0aW9uIGZvciBwcm9jZXNzaW5nIG91dHNpZGUgY2xpY2sgZXZlbnRzIHNwZWNpZmllZCBieSB0aGUgaGFuZGxlQ2xpY2tPdXRzaWRlIGNvbmZpZyBvcHRpb24uXCIpO1xuICAgICAgICB9XG4gICAgICB9XG5cbiAgICAgIHRoaXMuY29tcG9uZW50Tm9kZSA9IHRoaXMuX19nZXRDb21wb25lbnROb2RlKCk7IC8vIHJldHVybiBlYXJseSBzbyB3ZSBkb250IGluaXRpYXRlIG9uQ2xpY2tPdXRzaWRlXG5cbiAgICAgIGlmICh0aGlzLnByb3BzLmRpc2FibGVPbkNsaWNrT3V0c2lkZSkgcmV0dXJuO1xuICAgICAgdGhpcy5lbmFibGVPbkNsaWNrT3V0c2lkZSgpO1xuICAgIH07XG5cbiAgICBfcHJvdG8uY29tcG9uZW50RGlkVXBkYXRlID0gZnVuY3Rpb24gY29tcG9uZW50RGlkVXBkYXRlKCkge1xuICAgICAgdGhpcy5jb21wb25lbnROb2RlID0gdGhpcy5fX2dldENvbXBvbmVudE5vZGUoKTtcbiAgICB9O1xuICAgIC8qKlxuICAgICAqIFJlbW92ZSBhbGwgZG9jdW1lbnQncyBldmVudCBsaXN0ZW5lcnMgZm9yIHRoaXMgY29tcG9uZW50XG4gICAgICovXG5cblxuICAgIF9wcm90by5jb21wb25lbnRXaWxsVW5tb3VudCA9IGZ1bmN0aW9uIGNvbXBvbmVudFdpbGxVbm1vdW50KCkge1xuICAgICAgdGhpcy5kaXNhYmxlT25DbGlja091dHNpZGUoKTtcbiAgICB9O1xuICAgIC8qKlxuICAgICAqIENhbiBiZSBjYWxsZWQgdG8gZXhwbGljaXRseSBlbmFibGUgZXZlbnQgbGlzdGVuaW5nXG4gICAgICogZm9yIGNsaWNrcyBhbmQgdG91Y2hlcyBvdXRzaWRlIG9mIHRoaXMgZWxlbWVudC5cbiAgICAgKi9cblxuXG4gICAgLyoqXG4gICAgICogUGFzcy10aHJvdWdoIHJlbmRlclxuICAgICAqL1xuICAgIF9wcm90by5yZW5kZXIgPSBmdW5jdGlvbiByZW5kZXIoKSB7XG4gICAgICAvLyBlc2xpbnQtZGlzYWJsZS1uZXh0LWxpbmUgbm8tdW51c2VkLXZhcnNcbiAgICAgIHZhciBfcHJvcHMgPSB0aGlzLnByb3BzLFxuICAgICAgICAgIGV4Y2x1ZGVTY3JvbGxiYXIgPSBfcHJvcHMuZXhjbHVkZVNjcm9sbGJhcixcbiAgICAgICAgICBwcm9wcyA9IF9vYmplY3RXaXRob3V0UHJvcGVydGllcyhfcHJvcHMsIFtcImV4Y2x1ZGVTY3JvbGxiYXJcIl0pO1xuXG4gICAgICBpZiAoV3JhcHBlZENvbXBvbmVudC5wcm90b3R5cGUuaXNSZWFjdENvbXBvbmVudCkge1xuICAgICAgICBwcm9wcy5yZWYgPSB0aGlzLmdldFJlZjtcbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIHByb3BzLndyYXBwZWRSZWYgPSB0aGlzLmdldFJlZjtcbiAgICAgIH1cblxuICAgICAgcHJvcHMuZGlzYWJsZU9uQ2xpY2tPdXRzaWRlID0gdGhpcy5kaXNhYmxlT25DbGlja091dHNpZGU7XG4gICAgICBwcm9wcy5lbmFibGVPbkNsaWNrT3V0c2lkZSA9IHRoaXMuZW5hYmxlT25DbGlja091dHNpZGU7XG4gICAgICByZXR1cm4gY3JlYXRlRWxlbWVudChXcmFwcGVkQ29tcG9uZW50LCBwcm9wcyk7XG4gICAgfTtcblxuICAgIHJldHVybiBvbkNsaWNrT3V0c2lkZTtcbiAgfShDb21wb25lbnQpLCBfY2xhc3MuZGlzcGxheU5hbWUgPSBcIk9uQ2xpY2tPdXRzaWRlKFwiICsgY29tcG9uZW50TmFtZSArIFwiKVwiLCBfY2xhc3MuZGVmYXVsdFByb3BzID0ge1xuICAgIGV2ZW50VHlwZXM6IFsnbW91c2Vkb3duJywgJ3RvdWNoc3RhcnQnXSxcbiAgICBleGNsdWRlU2Nyb2xsYmFyOiBjb25maWcgJiYgY29uZmlnLmV4Y2x1ZGVTY3JvbGxiYXIgfHwgZmFsc2UsXG4gICAgb3V0c2lkZUNsaWNrSWdub3JlQ2xhc3M6IElHTk9SRV9DTEFTU19OQU1FLFxuICAgIHByZXZlbnREZWZhdWx0OiBmYWxzZSxcbiAgICBzdG9wUHJvcGFnYXRpb246IGZhbHNlXG4gIH0sIF9jbGFzcy5nZXRDbGFzcyA9IGZ1bmN0aW9uICgpIHtcbiAgICByZXR1cm4gV3JhcHBlZENvbXBvbmVudC5nZXRDbGFzcyA/IFdyYXBwZWRDb21wb25lbnQuZ2V0Q2xhc3MoKSA6IFdyYXBwZWRDb21wb25lbnQ7XG4gIH0sIF90ZW1wO1xufVxuXG5leHBvcnQgeyBJR05PUkVfQ0xBU1NfTkFNRSB9O1xuZXhwb3J0IGRlZmF1bHQgb25DbGlja091dHNpZGVIT0M7XG4iLCJmdW5jdGlvbiBpc0Fic29sdXRlKHBhdGhuYW1lKSB7XG4gIHJldHVybiBwYXRobmFtZS5jaGFyQXQoMCkgPT09ICcvJztcbn1cblxuLy8gQWJvdXQgMS41eCBmYXN0ZXIgdGhhbiB0aGUgdHdvLWFyZyB2ZXJzaW9uIG9mIEFycmF5I3NwbGljZSgpXG5mdW5jdGlvbiBzcGxpY2VPbmUobGlzdCwgaW5kZXgpIHtcbiAgZm9yICh2YXIgaSA9IGluZGV4LCBrID0gaSArIDEsIG4gPSBsaXN0Lmxlbmd0aDsgayA8IG47IGkgKz0gMSwgayArPSAxKSB7XG4gICAgbGlzdFtpXSA9IGxpc3Rba107XG4gIH1cblxuICBsaXN0LnBvcCgpO1xufVxuXG4vLyBUaGlzIGltcGxlbWVudGF0aW9uIGlzIGJhc2VkIGhlYXZpbHkgb24gbm9kZSdzIHVybC5wYXJzZVxuZnVuY3Rpb24gcmVzb2x2ZVBhdGhuYW1lKHRvKSB7XG4gIHZhciBmcm9tID0gYXJndW1lbnRzLmxlbmd0aCA+IDEgJiYgYXJndW1lbnRzWzFdICE9PSB1bmRlZmluZWQgPyBhcmd1bWVudHNbMV0gOiAnJztcblxuICB2YXIgdG9QYXJ0cyA9IHRvICYmIHRvLnNwbGl0KCcvJykgfHwgW107XG4gIHZhciBmcm9tUGFydHMgPSBmcm9tICYmIGZyb20uc3BsaXQoJy8nKSB8fCBbXTtcblxuICB2YXIgaXNUb0FicyA9IHRvICYmIGlzQWJzb2x1dGUodG8pO1xuICB2YXIgaXNGcm9tQWJzID0gZnJvbSAmJiBpc0Fic29sdXRlKGZyb20pO1xuICB2YXIgbXVzdEVuZEFicyA9IGlzVG9BYnMgfHwgaXNGcm9tQWJzO1xuXG4gIGlmICh0byAmJiBpc0Fic29sdXRlKHRvKSkge1xuICAgIC8vIHRvIGlzIGFic29sdXRlXG4gICAgZnJvbVBhcnRzID0gdG9QYXJ0cztcbiAgfSBlbHNlIGlmICh0b1BhcnRzLmxlbmd0aCkge1xuICAgIC8vIHRvIGlzIHJlbGF0aXZlLCBkcm9wIHRoZSBmaWxlbmFtZVxuICAgIGZyb21QYXJ0cy5wb3AoKTtcbiAgICBmcm9tUGFydHMgPSBmcm9tUGFydHMuY29uY2F0KHRvUGFydHMpO1xuICB9XG5cbiAgaWYgKCFmcm9tUGFydHMubGVuZ3RoKSByZXR1cm4gJy8nO1xuXG4gIHZhciBoYXNUcmFpbGluZ1NsYXNoID0gdm9pZCAwO1xuICBpZiAoZnJvbVBhcnRzLmxlbmd0aCkge1xuICAgIHZhciBsYXN0ID0gZnJvbVBhcnRzW2Zyb21QYXJ0cy5sZW5ndGggLSAxXTtcbiAgICBoYXNUcmFpbGluZ1NsYXNoID0gbGFzdCA9PT0gJy4nIHx8IGxhc3QgPT09ICcuLicgfHwgbGFzdCA9PT0gJyc7XG4gIH0gZWxzZSB7XG4gICAgaGFzVHJhaWxpbmdTbGFzaCA9IGZhbHNlO1xuICB9XG5cbiAgdmFyIHVwID0gMDtcbiAgZm9yICh2YXIgaSA9IGZyb21QYXJ0cy5sZW5ndGg7IGkgPj0gMDsgaS0tKSB7XG4gICAgdmFyIHBhcnQgPSBmcm9tUGFydHNbaV07XG5cbiAgICBpZiAocGFydCA9PT0gJy4nKSB7XG4gICAgICBzcGxpY2VPbmUoZnJvbVBhcnRzLCBpKTtcbiAgICB9IGVsc2UgaWYgKHBhcnQgPT09ICcuLicpIHtcbiAgICAgIHNwbGljZU9uZShmcm9tUGFydHMsIGkpO1xuICAgICAgdXArKztcbiAgICB9IGVsc2UgaWYgKHVwKSB7XG4gICAgICBzcGxpY2VPbmUoZnJvbVBhcnRzLCBpKTtcbiAgICAgIHVwLS07XG4gICAgfVxuICB9XG5cbiAgaWYgKCFtdXN0RW5kQWJzKSBmb3IgKDsgdXAtLTsgdXApIHtcbiAgICBmcm9tUGFydHMudW5zaGlmdCgnLi4nKTtcbiAgfWlmIChtdXN0RW5kQWJzICYmIGZyb21QYXJ0c1swXSAhPT0gJycgJiYgKCFmcm9tUGFydHNbMF0gfHwgIWlzQWJzb2x1dGUoZnJvbVBhcnRzWzBdKSkpIGZyb21QYXJ0cy51bnNoaWZ0KCcnKTtcblxuICB2YXIgcmVzdWx0ID0gZnJvbVBhcnRzLmpvaW4oJy8nKTtcblxuICBpZiAoaGFzVHJhaWxpbmdTbGFzaCAmJiByZXN1bHQuc3Vic3RyKC0xKSAhPT0gJy8nKSByZXN1bHQgKz0gJy8nO1xuXG4gIHJldHVybiByZXN1bHQ7XG59XG5cbmV4cG9ydCBkZWZhdWx0IHJlc29sdmVQYXRobmFtZTsiLCIndXNlIHN0cmljdCc7XG5tb2R1bGUuZXhwb3J0cyA9IGZ1bmN0aW9uIChzdHIpIHtcblx0cmV0dXJuIGVuY29kZVVSSUNvbXBvbmVudChzdHIpLnJlcGxhY2UoL1shJygpKl0vZywgZnVuY3Rpb24gKGMpIHtcblx0XHRyZXR1cm4gJyUnICsgYy5jaGFyQ29kZUF0KDApLnRvU3RyaW5nKDE2KS50b1VwcGVyQ2FzZSgpO1xuXHR9KTtcbn07XG4iLCIvKlxuXHRNSVQgTGljZW5zZSBodHRwOi8vd3d3Lm9wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL21pdC1saWNlbnNlLnBocFxuXHRBdXRob3IgVG9iaWFzIEtvcHBlcnMgQHNva3JhXG4qL1xuXG52YXIgc3R5bGVzSW5Eb20gPSB7fTtcblxudmFyXHRtZW1vaXplID0gZnVuY3Rpb24gKGZuKSB7XG5cdHZhciBtZW1vO1xuXG5cdHJldHVybiBmdW5jdGlvbiAoKSB7XG5cdFx0aWYgKHR5cGVvZiBtZW1vID09PSBcInVuZGVmaW5lZFwiKSBtZW1vID0gZm4uYXBwbHkodGhpcywgYXJndW1lbnRzKTtcblx0XHRyZXR1cm4gbWVtbztcblx0fTtcbn07XG5cbnZhciBpc09sZElFID0gbWVtb2l6ZShmdW5jdGlvbiAoKSB7XG5cdC8vIFRlc3QgZm9yIElFIDw9IDkgYXMgcHJvcG9zZWQgYnkgQnJvd3NlcmhhY2tzXG5cdC8vIEBzZWUgaHR0cDovL2Jyb3dzZXJoYWNrcy5jb20vI2hhY2stZTcxZDg2OTJmNjUzMzQxNzNmZWU3MTVjMjIyY2I4MDVcblx0Ly8gVGVzdHMgZm9yIGV4aXN0ZW5jZSBvZiBzdGFuZGFyZCBnbG9iYWxzIGlzIHRvIGFsbG93IHN0eWxlLWxvYWRlclxuXHQvLyB0byBvcGVyYXRlIGNvcnJlY3RseSBpbnRvIG5vbi1zdGFuZGFyZCBlbnZpcm9ubWVudHNcblx0Ly8gQHNlZSBodHRwczovL2dpdGh1Yi5jb20vd2VicGFjay1jb250cmliL3N0eWxlLWxvYWRlci9pc3N1ZXMvMTc3XG5cdHJldHVybiB3aW5kb3cgJiYgZG9jdW1lbnQgJiYgZG9jdW1lbnQuYWxsICYmICF3aW5kb3cuYXRvYjtcbn0pO1xuXG52YXIgZ2V0VGFyZ2V0ID0gZnVuY3Rpb24gKHRhcmdldCkge1xuICByZXR1cm4gZG9jdW1lbnQucXVlcnlTZWxlY3Rvcih0YXJnZXQpO1xufTtcblxudmFyIGdldEVsZW1lbnQgPSAoZnVuY3Rpb24gKGZuKSB7XG5cdHZhciBtZW1vID0ge307XG5cblx0cmV0dXJuIGZ1bmN0aW9uKHRhcmdldCkge1xuICAgICAgICAgICAgICAgIC8vIElmIHBhc3NpbmcgZnVuY3Rpb24gaW4gb3B0aW9ucywgdGhlbiB1c2UgaXQgZm9yIHJlc29sdmUgXCJoZWFkXCIgZWxlbWVudC5cbiAgICAgICAgICAgICAgICAvLyBVc2VmdWwgZm9yIFNoYWRvdyBSb290IHN0eWxlIGkuZVxuICAgICAgICAgICAgICAgIC8vIHtcbiAgICAgICAgICAgICAgICAvLyAgIGluc2VydEludG86IGZ1bmN0aW9uICgpIHsgcmV0dXJuIGRvY3VtZW50LnF1ZXJ5U2VsZWN0b3IoXCIjZm9vXCIpLnNoYWRvd1Jvb3QgfVxuICAgICAgICAgICAgICAgIC8vIH1cbiAgICAgICAgICAgICAgICBpZiAodHlwZW9mIHRhcmdldCA9PT0gJ2Z1bmN0aW9uJykge1xuICAgICAgICAgICAgICAgICAgICAgICAgcmV0dXJuIHRhcmdldCgpO1xuICAgICAgICAgICAgICAgIH1cbiAgICAgICAgICAgICAgICBpZiAodHlwZW9mIG1lbW9bdGFyZ2V0XSA9PT0gXCJ1bmRlZmluZWRcIikge1xuXHRcdFx0dmFyIHN0eWxlVGFyZ2V0ID0gZ2V0VGFyZ2V0LmNhbGwodGhpcywgdGFyZ2V0KTtcblx0XHRcdC8vIFNwZWNpYWwgY2FzZSB0byByZXR1cm4gaGVhZCBvZiBpZnJhbWUgaW5zdGVhZCBvZiBpZnJhbWUgaXRzZWxmXG5cdFx0XHRpZiAod2luZG93LkhUTUxJRnJhbWVFbGVtZW50ICYmIHN0eWxlVGFyZ2V0IGluc3RhbmNlb2Ygd2luZG93LkhUTUxJRnJhbWVFbGVtZW50KSB7XG5cdFx0XHRcdHRyeSB7XG5cdFx0XHRcdFx0Ly8gVGhpcyB3aWxsIHRocm93IGFuIGV4Y2VwdGlvbiBpZiBhY2Nlc3MgdG8gaWZyYW1lIGlzIGJsb2NrZWRcblx0XHRcdFx0XHQvLyBkdWUgdG8gY3Jvc3Mtb3JpZ2luIHJlc3RyaWN0aW9uc1xuXHRcdFx0XHRcdHN0eWxlVGFyZ2V0ID0gc3R5bGVUYXJnZXQuY29udGVudERvY3VtZW50LmhlYWQ7XG5cdFx0XHRcdH0gY2F0Y2goZSkge1xuXHRcdFx0XHRcdHN0eWxlVGFyZ2V0ID0gbnVsbDtcblx0XHRcdFx0fVxuXHRcdFx0fVxuXHRcdFx0bWVtb1t0YXJnZXRdID0gc3R5bGVUYXJnZXQ7XG5cdFx0fVxuXHRcdHJldHVybiBtZW1vW3RhcmdldF1cblx0fTtcbn0pKCk7XG5cbnZhciBzaW5nbGV0b24gPSBudWxsO1xudmFyXHRzaW5nbGV0b25Db3VudGVyID0gMDtcbnZhclx0c3R5bGVzSW5zZXJ0ZWRBdFRvcCA9IFtdO1xuXG52YXJcdGZpeFVybHMgPSByZXF1aXJlKFwiLi91cmxzXCIpO1xuXG5tb2R1bGUuZXhwb3J0cyA9IGZ1bmN0aW9uKGxpc3QsIG9wdGlvbnMpIHtcblx0aWYgKHR5cGVvZiBERUJVRyAhPT0gXCJ1bmRlZmluZWRcIiAmJiBERUJVRykge1xuXHRcdGlmICh0eXBlb2YgZG9jdW1lbnQgIT09IFwib2JqZWN0XCIpIHRocm93IG5ldyBFcnJvcihcIlRoZSBzdHlsZS1sb2FkZXIgY2Fubm90IGJlIHVzZWQgaW4gYSBub24tYnJvd3NlciBlbnZpcm9ubWVudFwiKTtcblx0fVxuXG5cdG9wdGlvbnMgPSBvcHRpb25zIHx8IHt9O1xuXG5cdG9wdGlvbnMuYXR0cnMgPSB0eXBlb2Ygb3B0aW9ucy5hdHRycyA9PT0gXCJvYmplY3RcIiA/IG9wdGlvbnMuYXR0cnMgOiB7fTtcblxuXHQvLyBGb3JjZSBzaW5nbGUtdGFnIHNvbHV0aW9uIG9uIElFNi05LCB3aGljaCBoYXMgYSBoYXJkIGxpbWl0IG9uIHRoZSAjIG9mIDxzdHlsZT5cblx0Ly8gdGFncyBpdCB3aWxsIGFsbG93IG9uIGEgcGFnZVxuXHRpZiAoIW9wdGlvbnMuc2luZ2xldG9uICYmIHR5cGVvZiBvcHRpb25zLnNpbmdsZXRvbiAhPT0gXCJib29sZWFuXCIpIG9wdGlvbnMuc2luZ2xldG9uID0gaXNPbGRJRSgpO1xuXG5cdC8vIEJ5IGRlZmF1bHQsIGFkZCA8c3R5bGU+IHRhZ3MgdG8gdGhlIDxoZWFkPiBlbGVtZW50XG4gICAgICAgIGlmICghb3B0aW9ucy5pbnNlcnRJbnRvKSBvcHRpb25zLmluc2VydEludG8gPSBcImhlYWRcIjtcblxuXHQvLyBCeSBkZWZhdWx0LCBhZGQgPHN0eWxlPiB0YWdzIHRvIHRoZSBib3R0b20gb2YgdGhlIHRhcmdldFxuXHRpZiAoIW9wdGlvbnMuaW5zZXJ0QXQpIG9wdGlvbnMuaW5zZXJ0QXQgPSBcImJvdHRvbVwiO1xuXG5cdHZhciBzdHlsZXMgPSBsaXN0VG9TdHlsZXMobGlzdCwgb3B0aW9ucyk7XG5cblx0YWRkU3R5bGVzVG9Eb20oc3R5bGVzLCBvcHRpb25zKTtcblxuXHRyZXR1cm4gZnVuY3Rpb24gdXBkYXRlIChuZXdMaXN0KSB7XG5cdFx0dmFyIG1heVJlbW92ZSA9IFtdO1xuXG5cdFx0Zm9yICh2YXIgaSA9IDA7IGkgPCBzdHlsZXMubGVuZ3RoOyBpKyspIHtcblx0XHRcdHZhciBpdGVtID0gc3R5bGVzW2ldO1xuXHRcdFx0dmFyIGRvbVN0eWxlID0gc3R5bGVzSW5Eb21baXRlbS5pZF07XG5cblx0XHRcdGRvbVN0eWxlLnJlZnMtLTtcblx0XHRcdG1heVJlbW92ZS5wdXNoKGRvbVN0eWxlKTtcblx0XHR9XG5cblx0XHRpZihuZXdMaXN0KSB7XG5cdFx0XHR2YXIgbmV3U3R5bGVzID0gbGlzdFRvU3R5bGVzKG5ld0xpc3QsIG9wdGlvbnMpO1xuXHRcdFx0YWRkU3R5bGVzVG9Eb20obmV3U3R5bGVzLCBvcHRpb25zKTtcblx0XHR9XG5cblx0XHRmb3IgKHZhciBpID0gMDsgaSA8IG1heVJlbW92ZS5sZW5ndGg7IGkrKykge1xuXHRcdFx0dmFyIGRvbVN0eWxlID0gbWF5UmVtb3ZlW2ldO1xuXG5cdFx0XHRpZihkb21TdHlsZS5yZWZzID09PSAwKSB7XG5cdFx0XHRcdGZvciAodmFyIGogPSAwOyBqIDwgZG9tU3R5bGUucGFydHMubGVuZ3RoOyBqKyspIGRvbVN0eWxlLnBhcnRzW2pdKCk7XG5cblx0XHRcdFx0ZGVsZXRlIHN0eWxlc0luRG9tW2RvbVN0eWxlLmlkXTtcblx0XHRcdH1cblx0XHR9XG5cdH07XG59O1xuXG5mdW5jdGlvbiBhZGRTdHlsZXNUb0RvbSAoc3R5bGVzLCBvcHRpb25zKSB7XG5cdGZvciAodmFyIGkgPSAwOyBpIDwgc3R5bGVzLmxlbmd0aDsgaSsrKSB7XG5cdFx0dmFyIGl0ZW0gPSBzdHlsZXNbaV07XG5cdFx0dmFyIGRvbVN0eWxlID0gc3R5bGVzSW5Eb21baXRlbS5pZF07XG5cblx0XHRpZihkb21TdHlsZSkge1xuXHRcdFx0ZG9tU3R5bGUucmVmcysrO1xuXG5cdFx0XHRmb3IodmFyIGogPSAwOyBqIDwgZG9tU3R5bGUucGFydHMubGVuZ3RoOyBqKyspIHtcblx0XHRcdFx0ZG9tU3R5bGUucGFydHNbal0oaXRlbS5wYXJ0c1tqXSk7XG5cdFx0XHR9XG5cblx0XHRcdGZvcig7IGogPCBpdGVtLnBhcnRzLmxlbmd0aDsgaisrKSB7XG5cdFx0XHRcdGRvbVN0eWxlLnBhcnRzLnB1c2goYWRkU3R5bGUoaXRlbS5wYXJ0c1tqXSwgb3B0aW9ucykpO1xuXHRcdFx0fVxuXHRcdH0gZWxzZSB7XG5cdFx0XHR2YXIgcGFydHMgPSBbXTtcblxuXHRcdFx0Zm9yKHZhciBqID0gMDsgaiA8IGl0ZW0ucGFydHMubGVuZ3RoOyBqKyspIHtcblx0XHRcdFx0cGFydHMucHVzaChhZGRTdHlsZShpdGVtLnBhcnRzW2pdLCBvcHRpb25zKSk7XG5cdFx0XHR9XG5cblx0XHRcdHN0eWxlc0luRG9tW2l0ZW0uaWRdID0ge2lkOiBpdGVtLmlkLCByZWZzOiAxLCBwYXJ0czogcGFydHN9O1xuXHRcdH1cblx0fVxufVxuXG5mdW5jdGlvbiBsaXN0VG9TdHlsZXMgKGxpc3QsIG9wdGlvbnMpIHtcblx0dmFyIHN0eWxlcyA9IFtdO1xuXHR2YXIgbmV3U3R5bGVzID0ge307XG5cblx0Zm9yICh2YXIgaSA9IDA7IGkgPCBsaXN0Lmxlbmd0aDsgaSsrKSB7XG5cdFx0dmFyIGl0ZW0gPSBsaXN0W2ldO1xuXHRcdHZhciBpZCA9IG9wdGlvbnMuYmFzZSA/IGl0ZW1bMF0gKyBvcHRpb25zLmJhc2UgOiBpdGVtWzBdO1xuXHRcdHZhciBjc3MgPSBpdGVtWzFdO1xuXHRcdHZhciBtZWRpYSA9IGl0ZW1bMl07XG5cdFx0dmFyIHNvdXJjZU1hcCA9IGl0ZW1bM107XG5cdFx0dmFyIHBhcnQgPSB7Y3NzOiBjc3MsIG1lZGlhOiBtZWRpYSwgc291cmNlTWFwOiBzb3VyY2VNYXB9O1xuXG5cdFx0aWYoIW5ld1N0eWxlc1tpZF0pIHN0eWxlcy5wdXNoKG5ld1N0eWxlc1tpZF0gPSB7aWQ6IGlkLCBwYXJ0czogW3BhcnRdfSk7XG5cdFx0ZWxzZSBuZXdTdHlsZXNbaWRdLnBhcnRzLnB1c2gocGFydCk7XG5cdH1cblxuXHRyZXR1cm4gc3R5bGVzO1xufVxuXG5mdW5jdGlvbiBpbnNlcnRTdHlsZUVsZW1lbnQgKG9wdGlvbnMsIHN0eWxlKSB7XG5cdHZhciB0YXJnZXQgPSBnZXRFbGVtZW50KG9wdGlvbnMuaW5zZXJ0SW50bylcblxuXHRpZiAoIXRhcmdldCkge1xuXHRcdHRocm93IG5ldyBFcnJvcihcIkNvdWxkbid0IGZpbmQgYSBzdHlsZSB0YXJnZXQuIFRoaXMgcHJvYmFibHkgbWVhbnMgdGhhdCB0aGUgdmFsdWUgZm9yIHRoZSAnaW5zZXJ0SW50bycgcGFyYW1ldGVyIGlzIGludmFsaWQuXCIpO1xuXHR9XG5cblx0dmFyIGxhc3RTdHlsZUVsZW1lbnRJbnNlcnRlZEF0VG9wID0gc3R5bGVzSW5zZXJ0ZWRBdFRvcFtzdHlsZXNJbnNlcnRlZEF0VG9wLmxlbmd0aCAtIDFdO1xuXG5cdGlmIChvcHRpb25zLmluc2VydEF0ID09PSBcInRvcFwiKSB7XG5cdFx0aWYgKCFsYXN0U3R5bGVFbGVtZW50SW5zZXJ0ZWRBdFRvcCkge1xuXHRcdFx0dGFyZ2V0Lmluc2VydEJlZm9yZShzdHlsZSwgdGFyZ2V0LmZpcnN0Q2hpbGQpO1xuXHRcdH0gZWxzZSBpZiAobGFzdFN0eWxlRWxlbWVudEluc2VydGVkQXRUb3AubmV4dFNpYmxpbmcpIHtcblx0XHRcdHRhcmdldC5pbnNlcnRCZWZvcmUoc3R5bGUsIGxhc3RTdHlsZUVsZW1lbnRJbnNlcnRlZEF0VG9wLm5leHRTaWJsaW5nKTtcblx0XHR9IGVsc2Uge1xuXHRcdFx0dGFyZ2V0LmFwcGVuZENoaWxkKHN0eWxlKTtcblx0XHR9XG5cdFx0c3R5bGVzSW5zZXJ0ZWRBdFRvcC5wdXNoKHN0eWxlKTtcblx0fSBlbHNlIGlmIChvcHRpb25zLmluc2VydEF0ID09PSBcImJvdHRvbVwiKSB7XG5cdFx0dGFyZ2V0LmFwcGVuZENoaWxkKHN0eWxlKTtcblx0fSBlbHNlIGlmICh0eXBlb2Ygb3B0aW9ucy5pbnNlcnRBdCA9PT0gXCJvYmplY3RcIiAmJiBvcHRpb25zLmluc2VydEF0LmJlZm9yZSkge1xuXHRcdHZhciBuZXh0U2libGluZyA9IGdldEVsZW1lbnQob3B0aW9ucy5pbnNlcnRJbnRvICsgXCIgXCIgKyBvcHRpb25zLmluc2VydEF0LmJlZm9yZSk7XG5cdFx0dGFyZ2V0Lmluc2VydEJlZm9yZShzdHlsZSwgbmV4dFNpYmxpbmcpO1xuXHR9IGVsc2Uge1xuXHRcdHRocm93IG5ldyBFcnJvcihcIltTdHlsZSBMb2FkZXJdXFxuXFxuIEludmFsaWQgdmFsdWUgZm9yIHBhcmFtZXRlciAnaW5zZXJ0QXQnICgnb3B0aW9ucy5pbnNlcnRBdCcpIGZvdW5kLlxcbiBNdXN0IGJlICd0b3AnLCAnYm90dG9tJywgb3IgT2JqZWN0LlxcbiAoaHR0cHM6Ly9naXRodWIuY29tL3dlYnBhY2stY29udHJpYi9zdHlsZS1sb2FkZXIjaW5zZXJ0YXQpXFxuXCIpO1xuXHR9XG59XG5cbmZ1bmN0aW9uIHJlbW92ZVN0eWxlRWxlbWVudCAoc3R5bGUpIHtcblx0aWYgKHN0eWxlLnBhcmVudE5vZGUgPT09IG51bGwpIHJldHVybiBmYWxzZTtcblx0c3R5bGUucGFyZW50Tm9kZS5yZW1vdmVDaGlsZChzdHlsZSk7XG5cblx0dmFyIGlkeCA9IHN0eWxlc0luc2VydGVkQXRUb3AuaW5kZXhPZihzdHlsZSk7XG5cdGlmKGlkeCA+PSAwKSB7XG5cdFx0c3R5bGVzSW5zZXJ0ZWRBdFRvcC5zcGxpY2UoaWR4LCAxKTtcblx0fVxufVxuXG5mdW5jdGlvbiBjcmVhdGVTdHlsZUVsZW1lbnQgKG9wdGlvbnMpIHtcblx0dmFyIHN0eWxlID0gZG9jdW1lbnQuY3JlYXRlRWxlbWVudChcInN0eWxlXCIpO1xuXG5cdGlmKG9wdGlvbnMuYXR0cnMudHlwZSA9PT0gdW5kZWZpbmVkKSB7XG5cdFx0b3B0aW9ucy5hdHRycy50eXBlID0gXCJ0ZXh0L2Nzc1wiO1xuXHR9XG5cblx0YWRkQXR0cnMoc3R5bGUsIG9wdGlvbnMuYXR0cnMpO1xuXHRpbnNlcnRTdHlsZUVsZW1lbnQob3B0aW9ucywgc3R5bGUpO1xuXG5cdHJldHVybiBzdHlsZTtcbn1cblxuZnVuY3Rpb24gY3JlYXRlTGlua0VsZW1lbnQgKG9wdGlvbnMpIHtcblx0dmFyIGxpbmsgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KFwibGlua1wiKTtcblxuXHRpZihvcHRpb25zLmF0dHJzLnR5cGUgPT09IHVuZGVmaW5lZCkge1xuXHRcdG9wdGlvbnMuYXR0cnMudHlwZSA9IFwidGV4dC9jc3NcIjtcblx0fVxuXHRvcHRpb25zLmF0dHJzLnJlbCA9IFwic3R5bGVzaGVldFwiO1xuXG5cdGFkZEF0dHJzKGxpbmssIG9wdGlvbnMuYXR0cnMpO1xuXHRpbnNlcnRTdHlsZUVsZW1lbnQob3B0aW9ucywgbGluayk7XG5cblx0cmV0dXJuIGxpbms7XG59XG5cbmZ1bmN0aW9uIGFkZEF0dHJzIChlbCwgYXR0cnMpIHtcblx0T2JqZWN0LmtleXMoYXR0cnMpLmZvckVhY2goZnVuY3Rpb24gKGtleSkge1xuXHRcdGVsLnNldEF0dHJpYnV0ZShrZXksIGF0dHJzW2tleV0pO1xuXHR9KTtcbn1cblxuZnVuY3Rpb24gYWRkU3R5bGUgKG9iaiwgb3B0aW9ucykge1xuXHR2YXIgc3R5bGUsIHVwZGF0ZSwgcmVtb3ZlLCByZXN1bHQ7XG5cblx0Ly8gSWYgYSB0cmFuc2Zvcm0gZnVuY3Rpb24gd2FzIGRlZmluZWQsIHJ1biBpdCBvbiB0aGUgY3NzXG5cdGlmIChvcHRpb25zLnRyYW5zZm9ybSAmJiBvYmouY3NzKSB7XG5cdCAgICByZXN1bHQgPSBvcHRpb25zLnRyYW5zZm9ybShvYmouY3NzKTtcblxuXHQgICAgaWYgKHJlc3VsdCkge1xuXHQgICAgXHQvLyBJZiB0cmFuc2Zvcm0gcmV0dXJucyBhIHZhbHVlLCB1c2UgdGhhdCBpbnN0ZWFkIG9mIHRoZSBvcmlnaW5hbCBjc3MuXG5cdCAgICBcdC8vIFRoaXMgYWxsb3dzIHJ1bm5pbmcgcnVudGltZSB0cmFuc2Zvcm1hdGlvbnMgb24gdGhlIGNzcy5cblx0ICAgIFx0b2JqLmNzcyA9IHJlc3VsdDtcblx0ICAgIH0gZWxzZSB7XG5cdCAgICBcdC8vIElmIHRoZSB0cmFuc2Zvcm0gZnVuY3Rpb24gcmV0dXJucyBhIGZhbHN5IHZhbHVlLCBkb24ndCBhZGQgdGhpcyBjc3MuXG5cdCAgICBcdC8vIFRoaXMgYWxsb3dzIGNvbmRpdGlvbmFsIGxvYWRpbmcgb2YgY3NzXG5cdCAgICBcdHJldHVybiBmdW5jdGlvbigpIHtcblx0ICAgIFx0XHQvLyBub29wXG5cdCAgICBcdH07XG5cdCAgICB9XG5cdH1cblxuXHRpZiAob3B0aW9ucy5zaW5nbGV0b24pIHtcblx0XHR2YXIgc3R5bGVJbmRleCA9IHNpbmdsZXRvbkNvdW50ZXIrKztcblxuXHRcdHN0eWxlID0gc2luZ2xldG9uIHx8IChzaW5nbGV0b24gPSBjcmVhdGVTdHlsZUVsZW1lbnQob3B0aW9ucykpO1xuXG5cdFx0dXBkYXRlID0gYXBwbHlUb1NpbmdsZXRvblRhZy5iaW5kKG51bGwsIHN0eWxlLCBzdHlsZUluZGV4LCBmYWxzZSk7XG5cdFx0cmVtb3ZlID0gYXBwbHlUb1NpbmdsZXRvblRhZy5iaW5kKG51bGwsIHN0eWxlLCBzdHlsZUluZGV4LCB0cnVlKTtcblxuXHR9IGVsc2UgaWYgKFxuXHRcdG9iai5zb3VyY2VNYXAgJiZcblx0XHR0eXBlb2YgVVJMID09PSBcImZ1bmN0aW9uXCIgJiZcblx0XHR0eXBlb2YgVVJMLmNyZWF0ZU9iamVjdFVSTCA9PT0gXCJmdW5jdGlvblwiICYmXG5cdFx0dHlwZW9mIFVSTC5yZXZva2VPYmplY3RVUkwgPT09IFwiZnVuY3Rpb25cIiAmJlxuXHRcdHR5cGVvZiBCbG9iID09PSBcImZ1bmN0aW9uXCIgJiZcblx0XHR0eXBlb2YgYnRvYSA9PT0gXCJmdW5jdGlvblwiXG5cdCkge1xuXHRcdHN0eWxlID0gY3JlYXRlTGlua0VsZW1lbnQob3B0aW9ucyk7XG5cdFx0dXBkYXRlID0gdXBkYXRlTGluay5iaW5kKG51bGwsIHN0eWxlLCBvcHRpb25zKTtcblx0XHRyZW1vdmUgPSBmdW5jdGlvbiAoKSB7XG5cdFx0XHRyZW1vdmVTdHlsZUVsZW1lbnQoc3R5bGUpO1xuXG5cdFx0XHRpZihzdHlsZS5ocmVmKSBVUkwucmV2b2tlT2JqZWN0VVJMKHN0eWxlLmhyZWYpO1xuXHRcdH07XG5cdH0gZWxzZSB7XG5cdFx0c3R5bGUgPSBjcmVhdGVTdHlsZUVsZW1lbnQob3B0aW9ucyk7XG5cdFx0dXBkYXRlID0gYXBwbHlUb1RhZy5iaW5kKG51bGwsIHN0eWxlKTtcblx0XHRyZW1vdmUgPSBmdW5jdGlvbiAoKSB7XG5cdFx0XHRyZW1vdmVTdHlsZUVsZW1lbnQoc3R5bGUpO1xuXHRcdH07XG5cdH1cblxuXHR1cGRhdGUob2JqKTtcblxuXHRyZXR1cm4gZnVuY3Rpb24gdXBkYXRlU3R5bGUgKG5ld09iaikge1xuXHRcdGlmIChuZXdPYmopIHtcblx0XHRcdGlmIChcblx0XHRcdFx0bmV3T2JqLmNzcyA9PT0gb2JqLmNzcyAmJlxuXHRcdFx0XHRuZXdPYmoubWVkaWEgPT09IG9iai5tZWRpYSAmJlxuXHRcdFx0XHRuZXdPYmouc291cmNlTWFwID09PSBvYmouc291cmNlTWFwXG5cdFx0XHQpIHtcblx0XHRcdFx0cmV0dXJuO1xuXHRcdFx0fVxuXG5cdFx0XHR1cGRhdGUob2JqID0gbmV3T2JqKTtcblx0XHR9IGVsc2Uge1xuXHRcdFx0cmVtb3ZlKCk7XG5cdFx0fVxuXHR9O1xufVxuXG52YXIgcmVwbGFjZVRleHQgPSAoZnVuY3Rpb24gKCkge1xuXHR2YXIgdGV4dFN0b3JlID0gW107XG5cblx0cmV0dXJuIGZ1bmN0aW9uIChpbmRleCwgcmVwbGFjZW1lbnQpIHtcblx0XHR0ZXh0U3RvcmVbaW5kZXhdID0gcmVwbGFjZW1lbnQ7XG5cblx0XHRyZXR1cm4gdGV4dFN0b3JlLmZpbHRlcihCb29sZWFuKS5qb2luKCdcXG4nKTtcblx0fTtcbn0pKCk7XG5cbmZ1bmN0aW9uIGFwcGx5VG9TaW5nbGV0b25UYWcgKHN0eWxlLCBpbmRleCwgcmVtb3ZlLCBvYmopIHtcblx0dmFyIGNzcyA9IHJlbW92ZSA/IFwiXCIgOiBvYmouY3NzO1xuXG5cdGlmIChzdHlsZS5zdHlsZVNoZWV0KSB7XG5cdFx0c3R5bGUuc3R5bGVTaGVldC5jc3NUZXh0ID0gcmVwbGFjZVRleHQoaW5kZXgsIGNzcyk7XG5cdH0gZWxzZSB7XG5cdFx0dmFyIGNzc05vZGUgPSBkb2N1bWVudC5jcmVhdGVUZXh0Tm9kZShjc3MpO1xuXHRcdHZhciBjaGlsZE5vZGVzID0gc3R5bGUuY2hpbGROb2RlcztcblxuXHRcdGlmIChjaGlsZE5vZGVzW2luZGV4XSkgc3R5bGUucmVtb3ZlQ2hpbGQoY2hpbGROb2Rlc1tpbmRleF0pO1xuXG5cdFx0aWYgKGNoaWxkTm9kZXMubGVuZ3RoKSB7XG5cdFx0XHRzdHlsZS5pbnNlcnRCZWZvcmUoY3NzTm9kZSwgY2hpbGROb2Rlc1tpbmRleF0pO1xuXHRcdH0gZWxzZSB7XG5cdFx0XHRzdHlsZS5hcHBlbmRDaGlsZChjc3NOb2RlKTtcblx0XHR9XG5cdH1cbn1cblxuZnVuY3Rpb24gYXBwbHlUb1RhZyAoc3R5bGUsIG9iaikge1xuXHR2YXIgY3NzID0gb2JqLmNzcztcblx0dmFyIG1lZGlhID0gb2JqLm1lZGlhO1xuXG5cdGlmKG1lZGlhKSB7XG5cdFx0c3R5bGUuc2V0QXR0cmlidXRlKFwibWVkaWFcIiwgbWVkaWEpXG5cdH1cblxuXHRpZihzdHlsZS5zdHlsZVNoZWV0KSB7XG5cdFx0c3R5bGUuc3R5bGVTaGVldC5jc3NUZXh0ID0gY3NzO1xuXHR9IGVsc2Uge1xuXHRcdHdoaWxlKHN0eWxlLmZpcnN0Q2hpbGQpIHtcblx0XHRcdHN0eWxlLnJlbW92ZUNoaWxkKHN0eWxlLmZpcnN0Q2hpbGQpO1xuXHRcdH1cblxuXHRcdHN0eWxlLmFwcGVuZENoaWxkKGRvY3VtZW50LmNyZWF0ZVRleHROb2RlKGNzcykpO1xuXHR9XG59XG5cbmZ1bmN0aW9uIHVwZGF0ZUxpbmsgKGxpbmssIG9wdGlvbnMsIG9iaikge1xuXHR2YXIgY3NzID0gb2JqLmNzcztcblx0dmFyIHNvdXJjZU1hcCA9IG9iai5zb3VyY2VNYXA7XG5cblx0Lypcblx0XHRJZiBjb252ZXJ0VG9BYnNvbHV0ZVVybHMgaXNuJ3QgZGVmaW5lZCwgYnV0IHNvdXJjZW1hcHMgYXJlIGVuYWJsZWRcblx0XHRhbmQgdGhlcmUgaXMgbm8gcHVibGljUGF0aCBkZWZpbmVkIHRoZW4gbGV0cyB0dXJuIGNvbnZlcnRUb0Fic29sdXRlVXJsc1xuXHRcdG9uIGJ5IGRlZmF1bHQuICBPdGhlcndpc2UgZGVmYXVsdCB0byB0aGUgY29udmVydFRvQWJzb2x1dGVVcmxzIG9wdGlvblxuXHRcdGRpcmVjdGx5XG5cdCovXG5cdHZhciBhdXRvRml4VXJscyA9IG9wdGlvbnMuY29udmVydFRvQWJzb2x1dGVVcmxzID09PSB1bmRlZmluZWQgJiYgc291cmNlTWFwO1xuXG5cdGlmIChvcHRpb25zLmNvbnZlcnRUb0Fic29sdXRlVXJscyB8fCBhdXRvRml4VXJscykge1xuXHRcdGNzcyA9IGZpeFVybHMoY3NzKTtcblx0fVxuXG5cdGlmIChzb3VyY2VNYXApIHtcblx0XHQvLyBodHRwOi8vc3RhY2tvdmVyZmxvdy5jb20vYS8yNjYwMzg3NVxuXHRcdGNzcyArPSBcIlxcbi8qIyBzb3VyY2VNYXBwaW5nVVJMPWRhdGE6YXBwbGljYXRpb24vanNvbjtiYXNlNjQsXCIgKyBidG9hKHVuZXNjYXBlKGVuY29kZVVSSUNvbXBvbmVudChKU09OLnN0cmluZ2lmeShzb3VyY2VNYXApKSkpICsgXCIgKi9cIjtcblx0fVxuXG5cdHZhciBibG9iID0gbmV3IEJsb2IoW2Nzc10sIHsgdHlwZTogXCJ0ZXh0L2Nzc1wiIH0pO1xuXG5cdHZhciBvbGRTcmMgPSBsaW5rLmhyZWY7XG5cblx0bGluay5ocmVmID0gVVJMLmNyZWF0ZU9iamVjdFVSTChibG9iKTtcblxuXHRpZihvbGRTcmMpIFVSTC5yZXZva2VPYmplY3RVUkwob2xkU3JjKTtcbn1cbiIsIlxuLyoqXG4gKiBXaGVuIHNvdXJjZSBtYXBzIGFyZSBlbmFibGVkLCBgc3R5bGUtbG9hZGVyYCB1c2VzIGEgbGluayBlbGVtZW50IHdpdGggYSBkYXRhLXVyaSB0b1xuICogZW1iZWQgdGhlIGNzcyBvbiB0aGUgcGFnZS4gVGhpcyBicmVha3MgYWxsIHJlbGF0aXZlIHVybHMgYmVjYXVzZSBub3cgdGhleSBhcmUgcmVsYXRpdmUgdG8gYVxuICogYnVuZGxlIGluc3RlYWQgb2YgdGhlIGN1cnJlbnQgcGFnZS5cbiAqXG4gKiBPbmUgc29sdXRpb24gaXMgdG8gb25seSB1c2UgZnVsbCB1cmxzLCBidXQgdGhhdCBtYXkgYmUgaW1wb3NzaWJsZS5cbiAqXG4gKiBJbnN0ZWFkLCB0aGlzIGZ1bmN0aW9uIFwiZml4ZXNcIiB0aGUgcmVsYXRpdmUgdXJscyB0byBiZSBhYnNvbHV0ZSBhY2NvcmRpbmcgdG8gdGhlIGN1cnJlbnQgcGFnZSBsb2NhdGlvbi5cbiAqXG4gKiBBIHJ1ZGltZW50YXJ5IHRlc3Qgc3VpdGUgaXMgbG9jYXRlZCBhdCBgdGVzdC9maXhVcmxzLmpzYCBhbmQgY2FuIGJlIHJ1biB2aWEgdGhlIGBucG0gdGVzdGAgY29tbWFuZC5cbiAqXG4gKi9cblxubW9kdWxlLmV4cG9ydHMgPSBmdW5jdGlvbiAoY3NzKSB7XG4gIC8vIGdldCBjdXJyZW50IGxvY2F0aW9uXG4gIHZhciBsb2NhdGlvbiA9IHR5cGVvZiB3aW5kb3cgIT09IFwidW5kZWZpbmVkXCIgJiYgd2luZG93LmxvY2F0aW9uO1xuXG4gIGlmICghbG9jYXRpb24pIHtcbiAgICB0aHJvdyBuZXcgRXJyb3IoXCJmaXhVcmxzIHJlcXVpcmVzIHdpbmRvdy5sb2NhdGlvblwiKTtcbiAgfVxuXG5cdC8vIGJsYW5rIG9yIG51bGw/XG5cdGlmICghY3NzIHx8IHR5cGVvZiBjc3MgIT09IFwic3RyaW5nXCIpIHtcblx0ICByZXR1cm4gY3NzO1xuICB9XG5cbiAgdmFyIGJhc2VVcmwgPSBsb2NhdGlvbi5wcm90b2NvbCArIFwiLy9cIiArIGxvY2F0aW9uLmhvc3Q7XG4gIHZhciBjdXJyZW50RGlyID0gYmFzZVVybCArIGxvY2F0aW9uLnBhdGhuYW1lLnJlcGxhY2UoL1xcL1teXFwvXSokLywgXCIvXCIpO1xuXG5cdC8vIGNvbnZlcnQgZWFjaCB1cmwoLi4uKVxuXHQvKlxuXHRUaGlzIHJlZ3VsYXIgZXhwcmVzc2lvbiBpcyBqdXN0IGEgd2F5IHRvIHJlY3Vyc2l2ZWx5IG1hdGNoIGJyYWNrZXRzIHdpdGhpblxuXHRhIHN0cmluZy5cblxuXHQgL3VybFxccypcXCggID0gTWF0Y2ggb24gdGhlIHdvcmQgXCJ1cmxcIiB3aXRoIGFueSB3aGl0ZXNwYWNlIGFmdGVyIGl0IGFuZCB0aGVuIGEgcGFyZW5zXG5cdCAgICggID0gU3RhcnQgYSBjYXB0dXJpbmcgZ3JvdXBcblx0ICAgICAoPzogID0gU3RhcnQgYSBub24tY2FwdHVyaW5nIGdyb3VwXG5cdCAgICAgICAgIFteKShdICA9IE1hdGNoIGFueXRoaW5nIHRoYXQgaXNuJ3QgYSBwYXJlbnRoZXNlc1xuXHQgICAgICAgICB8ICA9IE9SXG5cdCAgICAgICAgIFxcKCAgPSBNYXRjaCBhIHN0YXJ0IHBhcmVudGhlc2VzXG5cdCAgICAgICAgICAgICAoPzogID0gU3RhcnQgYW5vdGhlciBub24tY2FwdHVyaW5nIGdyb3Vwc1xuXHQgICAgICAgICAgICAgICAgIFteKShdKyAgPSBNYXRjaCBhbnl0aGluZyB0aGF0IGlzbid0IGEgcGFyZW50aGVzZXNcblx0ICAgICAgICAgICAgICAgICB8ICA9IE9SXG5cdCAgICAgICAgICAgICAgICAgXFwoICA9IE1hdGNoIGEgc3RhcnQgcGFyZW50aGVzZXNcblx0ICAgICAgICAgICAgICAgICAgICAgW14pKF0qICA9IE1hdGNoIGFueXRoaW5nIHRoYXQgaXNuJ3QgYSBwYXJlbnRoZXNlc1xuXHQgICAgICAgICAgICAgICAgIFxcKSAgPSBNYXRjaCBhIGVuZCBwYXJlbnRoZXNlc1xuXHQgICAgICAgICAgICAgKSAgPSBFbmQgR3JvdXBcbiAgICAgICAgICAgICAgKlxcKSA9IE1hdGNoIGFueXRoaW5nIGFuZCB0aGVuIGEgY2xvc2UgcGFyZW5zXG4gICAgICAgICAgKSAgPSBDbG9zZSBub24tY2FwdHVyaW5nIGdyb3VwXG4gICAgICAgICAgKiAgPSBNYXRjaCBhbnl0aGluZ1xuICAgICAgICkgID0gQ2xvc2UgY2FwdHVyaW5nIGdyb3VwXG5cdCBcXCkgID0gTWF0Y2ggYSBjbG9zZSBwYXJlbnNcblxuXHQgL2dpICA9IEdldCBhbGwgbWF0Y2hlcywgbm90IHRoZSBmaXJzdC4gIEJlIGNhc2UgaW5zZW5zaXRpdmUuXG5cdCAqL1xuXHR2YXIgZml4ZWRDc3MgPSBjc3MucmVwbGFjZSgvdXJsXFxzKlxcKCgoPzpbXikoXXxcXCgoPzpbXikoXSt8XFwoW14pKF0qXFwpKSpcXCkpKilcXCkvZ2ksIGZ1bmN0aW9uKGZ1bGxNYXRjaCwgb3JpZ1VybCkge1xuXHRcdC8vIHN0cmlwIHF1b3RlcyAoaWYgdGhleSBleGlzdClcblx0XHR2YXIgdW5xdW90ZWRPcmlnVXJsID0gb3JpZ1VybFxuXHRcdFx0LnRyaW0oKVxuXHRcdFx0LnJlcGxhY2UoL15cIiguKilcIiQvLCBmdW5jdGlvbihvLCAkMSl7IHJldHVybiAkMTsgfSlcblx0XHRcdC5yZXBsYWNlKC9eJyguKiknJC8sIGZ1bmN0aW9uKG8sICQxKXsgcmV0dXJuICQxOyB9KTtcblxuXHRcdC8vIGFscmVhZHkgYSBmdWxsIHVybD8gbm8gY2hhbmdlXG5cdFx0aWYgKC9eKCN8ZGF0YTp8aHR0cDpcXC9cXC98aHR0cHM6XFwvXFwvfGZpbGU6XFwvXFwvXFwvfFxccyokKS9pLnRlc3QodW5xdW90ZWRPcmlnVXJsKSkge1xuXHRcdCAgcmV0dXJuIGZ1bGxNYXRjaDtcblx0XHR9XG5cblx0XHQvLyBjb252ZXJ0IHRoZSB1cmwgdG8gYSBmdWxsIHVybFxuXHRcdHZhciBuZXdVcmw7XG5cblx0XHRpZiAodW5xdW90ZWRPcmlnVXJsLmluZGV4T2YoXCIvL1wiKSA9PT0gMCkge1xuXHRcdCAgXHQvL1RPRE86IHNob3VsZCB3ZSBhZGQgcHJvdG9jb2w/XG5cdFx0XHRuZXdVcmwgPSB1bnF1b3RlZE9yaWdVcmw7XG5cdFx0fSBlbHNlIGlmICh1bnF1b3RlZE9yaWdVcmwuaW5kZXhPZihcIi9cIikgPT09IDApIHtcblx0XHRcdC8vIHBhdGggc2hvdWxkIGJlIHJlbGF0aXZlIHRvIHRoZSBiYXNlIHVybFxuXHRcdFx0bmV3VXJsID0gYmFzZVVybCArIHVucXVvdGVkT3JpZ1VybDsgLy8gYWxyZWFkeSBzdGFydHMgd2l0aCAnLydcblx0XHR9IGVsc2Uge1xuXHRcdFx0Ly8gcGF0aCBzaG91bGQgYmUgcmVsYXRpdmUgdG8gY3VycmVudCBkaXJlY3Rvcnlcblx0XHRcdG5ld1VybCA9IGN1cnJlbnREaXIgKyB1bnF1b3RlZE9yaWdVcmwucmVwbGFjZSgvXlxcLlxcLy8sIFwiXCIpOyAvLyBTdHJpcCBsZWFkaW5nICcuLydcblx0XHR9XG5cblx0XHQvLyBzZW5kIGJhY2sgdGhlIGZpeGVkIHVybCguLi4pXG5cdFx0cmV0dXJuIFwidXJsKFwiICsgSlNPTi5zdHJpbmdpZnkobmV3VXJsKSArIFwiKVwiO1xuXHR9KTtcblxuXHQvLyBzZW5kIGJhY2sgdGhlIGZpeGVkIGNzc1xuXHRyZXR1cm4gZml4ZWRDc3M7XG59O1xuIiwidmFyIF90eXBlb2YgPSB0eXBlb2YgU3ltYm9sID09PSBcImZ1bmN0aW9uXCIgJiYgdHlwZW9mIFN5bWJvbC5pdGVyYXRvciA9PT0gXCJzeW1ib2xcIiA/IGZ1bmN0aW9uIChvYmopIHsgcmV0dXJuIHR5cGVvZiBvYmo7IH0gOiBmdW5jdGlvbiAob2JqKSB7IHJldHVybiBvYmogJiYgdHlwZW9mIFN5bWJvbCA9PT0gXCJmdW5jdGlvblwiICYmIG9iai5jb25zdHJ1Y3RvciA9PT0gU3ltYm9sICYmIG9iaiAhPT0gU3ltYm9sLnByb3RvdHlwZSA/IFwic3ltYm9sXCIgOiB0eXBlb2Ygb2JqOyB9O1xuXG5mdW5jdGlvbiB2YWx1ZUVxdWFsKGEsIGIpIHtcbiAgaWYgKGEgPT09IGIpIHJldHVybiB0cnVlO1xuXG4gIGlmIChhID09IG51bGwgfHwgYiA9PSBudWxsKSByZXR1cm4gZmFsc2U7XG5cbiAgaWYgKEFycmF5LmlzQXJyYXkoYSkpIHtcbiAgICByZXR1cm4gQXJyYXkuaXNBcnJheShiKSAmJiBhLmxlbmd0aCA9PT0gYi5sZW5ndGggJiYgYS5ldmVyeShmdW5jdGlvbiAoaXRlbSwgaW5kZXgpIHtcbiAgICAgIHJldHVybiB2YWx1ZUVxdWFsKGl0ZW0sIGJbaW5kZXhdKTtcbiAgICB9KTtcbiAgfVxuXG4gIHZhciBhVHlwZSA9IHR5cGVvZiBhID09PSAndW5kZWZpbmVkJyA/ICd1bmRlZmluZWQnIDogX3R5cGVvZihhKTtcbiAgdmFyIGJUeXBlID0gdHlwZW9mIGIgPT09ICd1bmRlZmluZWQnID8gJ3VuZGVmaW5lZCcgOiBfdHlwZW9mKGIpO1xuXG4gIGlmIChhVHlwZSAhPT0gYlR5cGUpIHJldHVybiBmYWxzZTtcblxuICBpZiAoYVR5cGUgPT09ICdvYmplY3QnKSB7XG4gICAgdmFyIGFWYWx1ZSA9IGEudmFsdWVPZigpO1xuICAgIHZhciBiVmFsdWUgPSBiLnZhbHVlT2YoKTtcblxuICAgIGlmIChhVmFsdWUgIT09IGEgfHwgYlZhbHVlICE9PSBiKSByZXR1cm4gdmFsdWVFcXVhbChhVmFsdWUsIGJWYWx1ZSk7XG5cbiAgICB2YXIgYUtleXMgPSBPYmplY3Qua2V5cyhhKTtcbiAgICB2YXIgYktleXMgPSBPYmplY3Qua2V5cyhiKTtcblxuICAgIGlmIChhS2V5cy5sZW5ndGggIT09IGJLZXlzLmxlbmd0aCkgcmV0dXJuIGZhbHNlO1xuXG4gICAgcmV0dXJuIGFLZXlzLmV2ZXJ5KGZ1bmN0aW9uIChrZXkpIHtcbiAgICAgIHJldHVybiB2YWx1ZUVxdWFsKGFba2V5XSwgYltrZXldKTtcbiAgICB9KTtcbiAgfVxuXG4gIHJldHVybiBmYWxzZTtcbn1cblxuZXhwb3J0IGRlZmF1bHQgdmFsdWVFcXVhbDsiLCIvKipcbiAqIENvcHlyaWdodCAyMDE0LTIwMTUsIEZhY2Vib29rLCBJbmMuXG4gKiBBbGwgcmlnaHRzIHJlc2VydmVkLlxuICpcbiAqIFRoaXMgc291cmNlIGNvZGUgaXMgbGljZW5zZWQgdW5kZXIgdGhlIEJTRC1zdHlsZSBsaWNlbnNlIGZvdW5kIGluIHRoZVxuICogTElDRU5TRSBmaWxlIGluIHRoZSByb290IGRpcmVjdG9yeSBvZiB0aGlzIHNvdXJjZSB0cmVlLiBBbiBhZGRpdGlvbmFsIGdyYW50XG4gKiBvZiBwYXRlbnQgcmlnaHRzIGNhbiBiZSBmb3VuZCBpbiB0aGUgUEFURU5UUyBmaWxlIGluIHRoZSBzYW1lIGRpcmVjdG9yeS5cbiAqL1xuXG4ndXNlIHN0cmljdCc7XG5cbi8qKlxuICogU2ltaWxhciB0byBpbnZhcmlhbnQgYnV0IG9ubHkgbG9ncyBhIHdhcm5pbmcgaWYgdGhlIGNvbmRpdGlvbiBpcyBub3QgbWV0LlxuICogVGhpcyBjYW4gYmUgdXNlZCB0byBsb2cgaXNzdWVzIGluIGRldmVsb3BtZW50IGVudmlyb25tZW50cyBpbiBjcml0aWNhbFxuICogcGF0aHMuIFJlbW92aW5nIHRoZSBsb2dnaW5nIGNvZGUgZm9yIHByb2R1Y3Rpb24gZW52aXJvbm1lbnRzIHdpbGwga2VlcCB0aGVcbiAqIHNhbWUgbG9naWMgYW5kIGZvbGxvdyB0aGUgc2FtZSBjb2RlIHBhdGhzLlxuICovXG5cbnZhciB3YXJuaW5nID0gZnVuY3Rpb24oKSB7fTtcblxuaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgd2FybmluZyA9IGZ1bmN0aW9uKGNvbmRpdGlvbiwgZm9ybWF0LCBhcmdzKSB7XG4gICAgdmFyIGxlbiA9IGFyZ3VtZW50cy5sZW5ndGg7XG4gICAgYXJncyA9IG5ldyBBcnJheShsZW4gPiAyID8gbGVuIC0gMiA6IDApO1xuICAgIGZvciAodmFyIGtleSA9IDI7IGtleSA8IGxlbjsga2V5KyspIHtcbiAgICAgIGFyZ3Nba2V5IC0gMl0gPSBhcmd1bWVudHNba2V5XTtcbiAgICB9XG4gICAgaWYgKGZvcm1hdCA9PT0gdW5kZWZpbmVkKSB7XG4gICAgICB0aHJvdyBuZXcgRXJyb3IoXG4gICAgICAgICdgd2FybmluZyhjb25kaXRpb24sIGZvcm1hdCwgLi4uYXJncylgIHJlcXVpcmVzIGEgd2FybmluZyAnICtcbiAgICAgICAgJ21lc3NhZ2UgYXJndW1lbnQnXG4gICAgICApO1xuICAgIH1cblxuICAgIGlmIChmb3JtYXQubGVuZ3RoIDwgMTAgfHwgKC9eW3NcXFddKiQvKS50ZXN0KGZvcm1hdCkpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcihcbiAgICAgICAgJ1RoZSB3YXJuaW5nIGZvcm1hdCBzaG91bGQgYmUgYWJsZSB0byB1bmlxdWVseSBpZGVudGlmeSB0aGlzICcgK1xuICAgICAgICAnd2FybmluZy4gUGxlYXNlLCB1c2UgYSBtb3JlIGRlc2NyaXB0aXZlIGZvcm1hdCB0aGFuOiAnICsgZm9ybWF0XG4gICAgICApO1xuICAgIH1cblxuICAgIGlmICghY29uZGl0aW9uKSB7XG4gICAgICB2YXIgYXJnSW5kZXggPSAwO1xuICAgICAgdmFyIG1lc3NhZ2UgPSAnV2FybmluZzogJyArXG4gICAgICAgIGZvcm1hdC5yZXBsYWNlKC8lcy9nLCBmdW5jdGlvbigpIHtcbiAgICAgICAgICByZXR1cm4gYXJnc1thcmdJbmRleCsrXTtcbiAgICAgICAgfSk7XG4gICAgICBpZiAodHlwZW9mIGNvbnNvbGUgIT09ICd1bmRlZmluZWQnKSB7XG4gICAgICAgIGNvbnNvbGUuZXJyb3IobWVzc2FnZSk7XG4gICAgICB9XG4gICAgICB0cnkge1xuICAgICAgICAvLyBUaGlzIGVycm9yIHdhcyB0aHJvd24gYXMgYSBjb252ZW5pZW5jZSBzbyB0aGF0IHlvdSBjYW4gdXNlIHRoaXMgc3RhY2tcbiAgICAgICAgLy8gdG8gZmluZCB0aGUgY2FsbHNpdGUgdGhhdCBjYXVzZWQgdGhpcyB3YXJuaW5nIHRvIGZpcmUuXG4gICAgICAgIHRocm93IG5ldyBFcnJvcihtZXNzYWdlKTtcbiAgICAgIH0gY2F0Y2goeCkge31cbiAgICB9XG4gIH07XG59XG5cbm1vZHVsZS5leHBvcnRzID0gd2FybmluZztcbiIsIi8vKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbi8vICBQZXJpb2RpY0RhdGFEaXNwbGF5MS50cyAtIEdidGNcclxuLy9cclxuLy8gIENvcHlyaWdodCDCqSAyMDE4LCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxyXG4vL1xyXG4vLyAgTGljZW5zZWQgdG8gdGhlIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZSAoR1BBKSB1bmRlciBvbmUgb3IgbW9yZSBjb250cmlidXRvciBsaWNlbnNlIGFncmVlbWVudHMuIFNlZVxyXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxyXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xyXG4vLyAgZmlsZSBleGNlcHQgaW4gY29tcGxpYW5jZSB3aXRoIHRoZSBMaWNlbnNlLiBZb3UgbWF5IG9idGFpbiBhIGNvcHkgb2YgdGhlIExpY2Vuc2UgYXQ6XHJcbi8vXHJcbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxyXG4vL1xyXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxyXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXHJcbi8vICBMaWNlbnNlIGZvciB0aGUgc3BlY2lmaWMgbGFuZ3VhZ2UgZ292ZXJuaW5nIHBlcm1pc3Npb25zIGFuZCBsaW1pdGF0aW9ucy5cclxuLy9cclxuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XHJcbi8vICAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vICAwNS8yNS8yMDE4IC0gQmlsbHkgRXJuZXN0XHJcbi8vICAgICAgIEdlbmVyYXRlZCBvcmlnaW5hbCB2ZXJzaW9uIG9mIHNvdXJjZSBjb2RlLlxyXG4vL1xyXG4vLyoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5pbXBvcnQgKiBhcyBtb21lbnQgZnJvbSAnbW9tZW50JztcclxuaW1wb3J0IHsgUGVyaW9kaWNEYXRhRGlzcGxheSB9IGZyb20gJy4vLi4vLi4vVFNYL2dsb2JhbCdcclxuXHJcbmV4cG9ydCBkZWZhdWx0IGNsYXNzIFBlcmlvZGljRGF0YURpc3BsYXlTZXJ2aWNlIHtcclxuICAgIGdldERhdGEobWV0ZXJJRDogbnVtYmVyLCBzdGFydERhdGU6IHN0cmluZywgZW5kRGF0ZTpzdHJpbmcsIHBpeGVsczogbnVtYmVyLCBtZWFzdXJlbWVudENoYXJhY3RlcmlzdGljSUQ6bnVtYmVyLCBtZWFzdXJlbWVudFR5cGVJRDpudW1iZXIsaGFybW9uaWNHcm91cDogbnVtYmVyLCB0eXBlKSB7XHJcbiAgICAgICAgcmV0dXJuICQuYWpheCh7XHJcbiAgICAgICAgICAgIHR5cGU6IFwiR0VUXCIsXHJcbiAgICAgICAgICAgIHVybDogYCR7d2luZG93LmxvY2F0aW9uLm9yaWdpbn0vYXBpL1BlcmlvZGljRGF0YURpc3BsYXkvR2V0RGF0YT9NZXRlcklEPSR7bWV0ZXJJRH1gICtcclxuICAgICAgICAgICAgICAgIGAmc3RhcnREYXRlPSR7bW9tZW50KHN0YXJ0RGF0ZSkuZm9ybWF0KCdZWVlZLU1NLUREJyl9YCArXHJcbiAgICAgICAgICAgICAgICBgJmVuZERhdGU9JHttb21lbnQoZW5kRGF0ZSkuZm9ybWF0KCdZWVlZLU1NLUREJyl9YCArXHJcbiAgICAgICAgICAgICAgICBgJnBpeGVscz0ke3BpeGVsc31gICtcclxuICAgICAgICAgICAgICAgIGAmTWVhc3VyZW1lbnRDaGFyYWN0ZXJpc3RpY0lEPSR7bWVhc3VyZW1lbnRDaGFyYWN0ZXJpc3RpY0lEfWAgKyBcclxuICAgICAgICAgICAgICAgIGAmTWVhc3VyZW1lbnRUeXBlSUQ9JHttZWFzdXJlbWVudFR5cGVJRH1gICtcclxuICAgICAgICAgICAgICAgIGAmSGFybW9uaWNHcm91cD0ke2hhcm1vbmljR3JvdXB9YCArXHJcbiAgICAgICAgICAgICAgICBgJnR5cGU9JHt0eXBlfWAsXHJcbiAgICAgICAgICAgIGNvbnRlbnRUeXBlOiBcImFwcGxpY2F0aW9uL2pzb247IGNoYXJzZXQ9dXRmLThcIixcclxuICAgICAgICAgICAgZGF0YVR5cGU6ICdqc29uJyxcclxuICAgICAgICAgICAgY2FjaGU6IHRydWUsXHJcbiAgICAgICAgICAgIGFzeW5jOiB0cnVlXHJcbiAgICAgICAgfSkgYXMgSlF1ZXJ5LmpxWEhSPFBlcmlvZGljRGF0YURpc3BsYXkuUmV0dXJuRGF0YT47XHJcbiAgICB9XHJcblxyXG4gICAgZ2V0TWV0ZXJzKCkge1xyXG4gICAgICAgIHJldHVybiAkLmFqYXgoe1xyXG4gICAgICAgICAgICB0eXBlOiBcIkdFVFwiLFxyXG4gICAgICAgICAgICB1cmw6IGAke3dpbmRvdy5sb2NhdGlvbi5vcmlnaW59L2FwaS9QZXJpb2RpY0RhdGFEaXNwbGF5L0dldE1ldGVyc2AsXHJcbiAgICAgICAgICAgIGNvbnRlbnRUeXBlOiBcImFwcGxpY2F0aW9uL2pzb247IGNoYXJzZXQ9dXRmLThcIixcclxuICAgICAgICAgICAgZGF0YVR5cGU6ICdqc29uJyxcclxuICAgICAgICAgICAgY2FjaGU6IHRydWUsXHJcbiAgICAgICAgICAgIGFzeW5jOiB0cnVlXHJcbiAgICAgICAgfSk7XHJcbiAgICB9XHJcblxyXG4gICAgZ2V0TWVhc3VyZW1lbnRDaGFyYWN0ZXJpc3RpY3MoZnJvbVN0ZXBDaGFuZ2VXZWJSZXBvcnQsIG1ldGVySUQpIHtcclxuICAgICAgICByZXR1cm4gJC5hamF4KHtcclxuICAgICAgICAgICAgdHlwZTogXCJHRVRcIixcclxuICAgICAgICAgICAgdXJsOiBgJHt3aW5kb3cubG9jYXRpb24ub3JpZ2lufS9hcGkvUGVyaW9kaWNEYXRhRGlzcGxheS9HZXRNZWFzdXJlbWVudENoYXJhY3RlcmlzdGljc2AgK1xyXG4gICAgICAgICAgICAgICAgYCR7KGZyb21TdGVwQ2hhbmdlV2ViUmVwb3J0ID8gYD9NZXRlcklEPSR7bWV0ZXJJRH1gIDogYGApfWAsXHJcbiAgICAgICAgICAgIGNvbnRlbnRUeXBlOiBcImFwcGxpY2F0aW9uL2pzb247IGNoYXJzZXQ9dXRmLThcIixcclxuICAgICAgICAgICAgZGF0YVR5cGU6ICdqc29uJyxcclxuICAgICAgICAgICAgY2FjaGU6IHRydWUsXHJcbiAgICAgICAgICAgIGFzeW5jOiB0cnVlXHJcbiAgICAgICAgfSkgYXMgSlF1ZXJ5LmpxWEhSPFBlcmlvZGljRGF0YURpc3BsYXkuTWVhc3VyZW1lbnRDaGFyYXRlcmlzdGljc1tdPjtcclxuICAgIH1cclxuXHJcblxyXG59IiwiLy8qKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuLy8gIFBlcmlvZGljRGF0YURpc3BsYXkxLnRzIC0gR2J0Y1xyXG4vL1xyXG4vLyAgQ29weXJpZ2h0IMKpIDIwMTgsIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZS4gIEFsbCBSaWdodHMgUmVzZXJ2ZWQuXHJcbi8vXHJcbi8vICBMaWNlbnNlZCB0byB0aGUgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlIChHUEEpIHVuZGVyIG9uZSBvciBtb3JlIGNvbnRyaWJ1dG9yIGxpY2Vuc2UgYWdyZWVtZW50cy4gU2VlXHJcbi8vICB0aGUgTk9USUNFIGZpbGUgZGlzdHJpYnV0ZWQgd2l0aCB0aGlzIHdvcmsgZm9yIGFkZGl0aW9uYWwgaW5mb3JtYXRpb24gcmVnYXJkaW5nIGNvcHlyaWdodCBvd25lcnNoaXAuXHJcbi8vICBUaGUgR1BBIGxpY2Vuc2VzIHRoaXMgZmlsZSB0byB5b3UgdW5kZXIgdGhlIE1JVCBMaWNlbnNlIChNSVQpLCB0aGUgXCJMaWNlbnNlXCI7IHlvdSBtYXkgbm90IHVzZSB0aGlzXHJcbi8vICBmaWxlIGV4Y2VwdCBpbiBjb21wbGlhbmNlIHdpdGggdGhlIExpY2Vuc2UuIFlvdSBtYXkgb2J0YWluIGEgY29weSBvZiB0aGUgTGljZW5zZSBhdDpcclxuLy9cclxuLy8gICAgICBodHRwOi8vb3BlbnNvdXJjZS5vcmcvbGljZW5zZXMvTUlUXHJcbi8vXHJcbi8vICBVbmxlc3MgYWdyZWVkIHRvIGluIHdyaXRpbmcsIHRoZSBzdWJqZWN0IHNvZnR3YXJlIGRpc3RyaWJ1dGVkIHVuZGVyIHRoZSBMaWNlbnNlIGlzIGRpc3RyaWJ1dGVkIG9uIGFuXHJcbi8vICBcIkFTLUlTXCIgQkFTSVMsIFdJVEhPVVQgV0FSUkFOVElFUyBPUiBDT05ESVRJT05TIE9GIEFOWSBLSU5ELCBlaXRoZXIgZXhwcmVzcyBvciBpbXBsaWVkLiBSZWZlciB0byB0aGVcclxuLy8gIExpY2Vuc2UgZm9yIHRoZSBzcGVjaWZpYyBsYW5ndWFnZSBnb3Zlcm5pbmcgcGVybWlzc2lvbnMgYW5kIGxpbWl0YXRpb25zLlxyXG4vL1xyXG4vLyAgQ29kZSBNb2RpZmljYXRpb24gSGlzdG9yeTpcclxuLy8gIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuLy8gIDA1LzI1LzIwMTggLSBCaWxseSBFcm5lc3RcclxuLy8gICAgICAgR2VuZXJhdGVkIG9yaWdpbmFsIHZlcnNpb24gb2Ygc291cmNlIGNvZGUuXHJcbi8vXHJcbi8vKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbmltcG9ydCAqIGFzIG1vbWVudCBmcm9tICdtb21lbnQnO1xyXG5pbXBvcnQgeyBQZXJpb2RpY0RhdGFEaXNwbGF5LCBUcmVuZGluZ2NEYXRhRGlzcGxheSB9IGZyb20gJy4vLi4vLi4vVFNYL2dsb2JhbCdcclxuXHJcbmV4cG9ydCBkZWZhdWx0IGNsYXNzIFRyZW5kaW5nRGF0YURpc3BsYXlTZXJ2aWNlIHtcclxuICAgIGdldERhdGEoY2hhbm5lbElEOiBudW1iZXIsIHN0YXJ0RGF0ZTogc3RyaW5nLCBlbmREYXRlOiBzdHJpbmcsIHBpeGVsczogbnVtYmVyKSB7XHJcbiAgICAgICAgcmV0dXJuICQuYWpheCh7XHJcbiAgICAgICAgICAgIHR5cGU6IFwiR0VUXCIsXHJcbiAgICAgICAgICAgIHVybDogYCR7d2luZG93LmxvY2F0aW9uLm9yaWdpbn0vYXBpL1RyZW5kaW5nRGF0YURpc3BsYXkvR2V0RGF0YT9DaGFubmVsSUQ9JHtjaGFubmVsSUR9YCArXHJcbiAgICAgICAgICAgICAgICBgJnN0YXJ0RGF0ZT0ke21vbWVudChzdGFydERhdGUpLmZvcm1hdCgnWVlZWS1NTS1ERFRISDptbScpfWAgK1xyXG4gICAgICAgICAgICAgICAgYCZlbmREYXRlPSR7bW9tZW50KGVuZERhdGUpLmZvcm1hdCgnWVlZWS1NTS1ERFRISDptbScpfWAgK1xyXG4gICAgICAgICAgICAgICAgYCZwaXhlbHM9JHtwaXhlbHN9YCxcclxuICAgICAgICAgICAgY29udGVudFR5cGU6IFwiYXBwbGljYXRpb24vanNvbjsgY2hhcnNldD11dGYtOFwiLFxyXG4gICAgICAgICAgICBkYXRhVHlwZTogJ2pzb24nLFxyXG4gICAgICAgICAgICBjYWNoZTogdHJ1ZSxcclxuICAgICAgICAgICAgYXN5bmM6IHRydWVcclxuICAgICAgICB9KSBhcyBKUXVlcnkuanFYSFI8UGVyaW9kaWNEYXRhRGlzcGxheS5SZXR1cm5EYXRhPjtcclxuICAgIH1cclxuICAgIGdldFBvc3REYXRhKG1lYXN1cmVtZW50czogVHJlbmRpbmdjRGF0YURpc3BsYXkuTWVhc3VyZW1lbnRbXSwgc3RhcnREYXRlOiBzdHJpbmcsIGVuZERhdGU6IHN0cmluZywgcGl4ZWxzOiBudW1iZXIpIHtcclxuICAgICAgICByZXR1cm4gJC5hamF4KHtcclxuICAgICAgICAgICAgdHlwZTogXCJQT1NUXCIsXHJcbiAgICAgICAgICAgIHVybDogYCR7d2luZG93LmxvY2F0aW9uLm9yaWdpbn0vYXBpL1RyZW5kaW5nRGF0YURpc3BsYXkvR2V0RGF0YWAsXHJcbiAgICAgICAgICAgIGNvbnRlbnRUeXBlOiBcImFwcGxpY2F0aW9uL2pzb247IGNoYXJzZXQ9dXRmLThcIixcclxuICAgICAgICAgICAgZGF0YVR5cGU6ICdqc29uJyxcclxuICAgICAgICAgICAgZGF0YTogSlNPTi5zdHJpbmdpZnkoe0NoYW5uZWxzOiBtZWFzdXJlbWVudHMubWFwKG1zID0+IG1zLklEKSwgU3RhcnREYXRlOiBzdGFydERhdGUsIEVuZERhdGU6IGVuZERhdGUsIFBpeGVsczogcGl4ZWxzIH0pLFxyXG4gICAgICAgICAgICBjYWNoZTogdHJ1ZSxcclxuICAgICAgICAgICAgYXN5bmM6IHRydWVcclxuICAgICAgICB9KSBhcyBKUXVlcnkuanFYSFI8VHJlbmRpbmdjRGF0YURpc3BsYXkuUmV0dXJuRGF0YT47XHJcbiAgICB9XHJcblxyXG5cclxuXHJcbiAgICBnZXRNZWFzdXJlbWVudHMobWV0ZXJJRDogbnVtYmVyKSB7XHJcbiAgICAgICAgcmV0dXJuICQuYWpheCh7XHJcbiAgICAgICAgICAgIHR5cGU6IFwiR0VUXCIsXHJcbiAgICAgICAgICAgIHVybDogYCR7d2luZG93LmxvY2F0aW9uLm9yaWdpbn0vYXBpL1RyZW5kaW5nRGF0YURpc3BsYXkvR2V0TWVhc3VyZW1lbnRzP01ldGVySUQ9JHttZXRlcklEfWAsXHJcbiAgICAgICAgICAgIGNvbnRlbnRUeXBlOiBcImFwcGxpY2F0aW9uL2pzb247IGNoYXJzZXQ9dXRmLThcIixcclxuICAgICAgICAgICAgZGF0YVR5cGU6ICdqc29uJyxcclxuICAgICAgICAgICAgY2FjaGU6IHRydWUsXHJcbiAgICAgICAgICAgIGFzeW5jOiB0cnVlXHJcbiAgICAgICAgfSk7XHJcbiAgICB9XHJcblxyXG5cclxufSIsIi8vKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbi8vICBEYXRlVGltZVJhbmdlUGlja2VyLnRzeCAtIEdidGNcclxuLy9cclxuLy8gIENvcHlyaWdodCDCqSAyMDE4LCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxyXG4vL1xyXG4vLyAgTGljZW5zZWQgdG8gdGhlIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZSAoR1BBKSB1bmRlciBvbmUgb3IgbW9yZSBjb250cmlidXRvciBsaWNlbnNlIGFncmVlbWVudHMuIFNlZVxyXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxyXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xyXG4vLyAgZmlsZSBleGNlcHQgaW4gY29tcGxpYW5jZSB3aXRoIHRoZSBMaWNlbnNlLiBZb3UgbWF5IG9idGFpbiBhIGNvcHkgb2YgdGhlIExpY2Vuc2UgYXQ6XHJcbi8vXHJcbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxyXG4vL1xyXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxyXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXHJcbi8vICBMaWNlbnNlIGZvciB0aGUgc3BlY2lmaWMgbGFuZ3VhZ2UgZ292ZXJuaW5nIHBlcm1pc3Npb25zIGFuZCBsaW1pdGF0aW9ucy5cclxuLy9cclxuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XHJcbi8vICAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vICAwNy8yMy8yMDE4IC0gQmlsbHkgRXJuZXN0XHJcbi8vICAgICAgIEdlbmVyYXRlZCBvcmlnaW5hbCB2ZXJzaW9uIG9mIHNvdXJjZSBjb2RlLlxyXG4vL1xyXG4vLyoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5cclxuaW1wb3J0ICogYXMgUmVhY3QgZnJvbSAncmVhY3QnO1xyXG5pbXBvcnQgKiBhcyBSZWFjdERPTSBmcm9tICdyZWFjdC1kb20nO1xyXG5pbXBvcnQgKiBhcyBfIGZyb20gXCJsb2Rhc2hcIjtcclxuaW1wb3J0ICogYXMgRGF0ZVRpbWUgZnJvbSBcInJlYWN0LWRhdGV0aW1lXCI7XHJcbmltcG9ydCAqIGFzIG1vbWVudCBmcm9tICdtb21lbnQnO1xyXG5pbXBvcnQgJ3JlYWN0LWRhdGV0aW1lL2Nzcy9yZWFjdC1kYXRldGltZS5jc3MnO1xyXG5cclxuZXhwb3J0IGRlZmF1bHQgY2xhc3MgRGF0ZVRpbWVSYW5nZVBpY2tlciBleHRlbmRzIFJlYWN0LkNvbXBvbmVudDxhbnksIGFueT57XHJcbiAgICBwcm9wczogeyBzdGFydERhdGU6IHN0cmluZywgZW5kRGF0ZTogc3RyaW5nLCBzdGF0ZVNldHRlcjogRnVuY3Rpb24gfVxyXG4gICAgc3RhdGU6IHsgc3RhcnREYXRlOiBtb21lbnQuTW9tZW50LCBlbmREYXRlOiBtb21lbnQuTW9tZW50IH1cclxuICAgIHN0YXRlU2V0dGVySWQ6IGFueTtcclxuICAgIGNvbnN0cnVjdG9yKHByb3BzKSB7XHJcbiAgICAgICAgc3VwZXIocHJvcHMpO1xyXG4gICAgICAgIHRoaXMuc3RhdGUgPSB7XHJcbiAgICAgICAgICAgIHN0YXJ0RGF0ZTogbW9tZW50KHRoaXMucHJvcHMuc3RhcnREYXRlKSxcclxuICAgICAgICAgICAgZW5kRGF0ZTogbW9tZW50KHRoaXMucHJvcHMuZW5kRGF0ZSlcclxuICAgICAgICB9O1xyXG4gICAgfVxyXG4gICAgcmVuZGVyKCkge1xyXG4gICAgICAgIHJldHVybiAoXHJcbiAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiY29udGFpbmVyXCIgc3R5bGU9e3t3aWR0aDogJ2luaGVyaXQnfX0+XHJcbiAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cInJvd1wiPlxyXG4gICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiZm9ybS1ncm91cFwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8RGF0ZVRpbWVcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGlzVmFsaWREYXRlPXsoZGF0ZSkgPT4geyByZXR1cm4gZGF0ZS5pc0JlZm9yZSh0aGlzLnN0YXRlLmVuZERhdGUpIH19XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB2YWx1ZT17dGhpcy5zdGF0ZS5zdGFydERhdGV9XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB0aW1lRm9ybWF0PVwiSEg6bW1cIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgb25DaGFuZ2U9eyh2YWx1ZSkgPT4gdGhpcy5zZXRTdGF0ZSh7IHN0YXJ0RGF0ZTogdmFsdWUgfSwgKCkgPT4gdGhpcy5zdGF0ZVNldHRlcigpKX0gLz5cclxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJyb3dcIj5cclxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImZvcm0tZ3JvdXBcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPERhdGVUaW1lXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBpc1ZhbGlkRGF0ZT17KGRhdGUpID0+IHsgcmV0dXJuIGRhdGUuaXNBZnRlcih0aGlzLnN0YXRlLnN0YXJ0RGF0ZSkgfX1cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHZhbHVlPXt0aGlzLnN0YXRlLmVuZERhdGV9XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB0aW1lRm9ybWF0PVwiSEg6bW1cIlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgb25DaGFuZ2U9eyh2YWx1ZSkgPT4gdGhpcy5zZXRTdGF0ZSh7IGVuZERhdGU6IHZhbHVlIH0sICgpID0+IHRoaXMuc3RhdGVTZXR0ZXIoKSl9IC8+XHJcbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgKTtcclxuICAgIH1cclxuXHJcbiAgICBjb21wb25lbnRXaWxsUmVjZWl2ZVByb3BzKG5leHRQcm9wcywgbmV4dENvbnRlbnQpIHtcclxuICAgICAgICBpZiAobmV4dFByb3BzLnN0YXJ0RGF0ZSAhPSB0aGlzLnN0YXRlLnN0YXJ0RGF0ZS5mb3JtYXQoJ1lZWVktTU0tRERUSEg6bW0nKSB8fCBuZXh0UHJvcHMuZW5kRGF0ZSAhPSB0aGlzLnN0YXRlLmVuZERhdGUuZm9ybWF0KCdZWVlZLU1NLUREVEhIOm1tJykpXHJcbiAgICAgICAgICAgIHRoaXMuc2V0U3RhdGUoeyBzdGFydERhdGU6IG1vbWVudCh0aGlzLnByb3BzLnN0YXJ0RGF0ZSksIGVuZERhdGU6IG1vbWVudCh0aGlzLnByb3BzLmVuZERhdGUpfSk7XHJcbiAgICB9XHJcblxyXG4gICAgc3RhdGVTZXR0ZXIoKSB7XHJcbiAgICAgICAgY2xlYXJUaW1lb3V0KHRoaXMuc3RhdGVTZXR0ZXJJZCk7XHJcbiAgICAgICAgdGhpcy5zdGF0ZVNldHRlcklkID0gc2V0VGltZW91dCgoKSA9PiB7XHJcbiAgICAgICAgICAgIHRoaXMucHJvcHMuc3RhdGVTZXR0ZXIoeyBzdGFydERhdGU6IHRoaXMuc3RhdGUuc3RhcnREYXRlLmZvcm1hdCgnWVlZWS1NTS1ERFRISDptbScpLCBlbmREYXRlOiB0aGlzLnN0YXRlLmVuZERhdGUuZm9ybWF0KCdZWVlZLU1NLUREVEhIOm1tJykgfSk7XHJcbiAgICAgICAgfSwgNTAwKTtcclxuICAgIH1cclxufVxyXG5cclxuXHJcbiIsIi8vKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbi8vICBNZWFzdXJlbWVudElucHV0LnRzeCAtIEdidGNcclxuLy9cclxuLy8gIENvcHlyaWdodCDCqSAyMDE4LCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxyXG4vL1xyXG4vLyAgTGljZW5zZWQgdG8gdGhlIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZSAoR1BBKSB1bmRlciBvbmUgb3IgbW9yZSBjb250cmlidXRvciBsaWNlbnNlIGFncmVlbWVudHMuIFNlZVxyXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxyXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xyXG4vLyAgZmlsZSBleGNlcHQgaW4gY29tcGxpYW5jZSB3aXRoIHRoZSBMaWNlbnNlLiBZb3UgbWF5IG9idGFpbiBhIGNvcHkgb2YgdGhlIExpY2Vuc2UgYXQ6XHJcbi8vXHJcbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxyXG4vL1xyXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxyXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXHJcbi8vICBMaWNlbnNlIGZvciB0aGUgc3BlY2lmaWMgbGFuZ3VhZ2UgZ292ZXJuaW5nIHBlcm1pc3Npb25zIGFuZCBsaW1pdGF0aW9ucy5cclxuLy9cclxuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XHJcbi8vICAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vICAwNy8xOS8yMDE4IC0gQmlsbHkgRXJuZXN0XHJcbi8vICAgICAgIEdlbmVyYXRlZCBvcmlnaW5hbCB2ZXJzaW9uIG9mIHNvdXJjZSBjb2RlLlxyXG4vL1xyXG4vLyoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5cclxuaW1wb3J0ICogYXMgUmVhY3QgZnJvbSAncmVhY3QnO1xyXG5pbXBvcnQgVHJlbmRpbmdEYXRhRGlzcGxheVNlcnZpY2UgZnJvbSAnLi8uLi9UUy9TZXJ2aWNlcy9UcmVuZGluZ0RhdGFEaXNwbGF5JztcclxuaW1wb3J0ICogYXMgXyBmcm9tIFwibG9kYXNoXCI7XHJcblxyXG5leHBvcnQgZGVmYXVsdCBjbGFzcyBNZWFzdXJlbWVudElucHV0IGV4dGVuZHMgUmVhY3QuQ29tcG9uZW50PHsgdmFsdWU6IG51bWJlciwgbWV0ZXJJRDogbnVtYmVyLCBvbkNoYW5nZTogRnVuY3Rpb24gfSwgeyBvcHRpb25zOiBhbnlbXSB9PntcclxuICAgIHRyZW5kaW5nRGF0YURpc3BsYXlTZXJ2aWNlOiBUcmVuZGluZ0RhdGFEaXNwbGF5U2VydmljZTtcclxuICAgIGNvbnN0cnVjdG9yKHByb3BzKSB7XHJcbiAgICAgICAgc3VwZXIocHJvcHMpO1xyXG4gICAgICAgIHRoaXMuc3RhdGUgPSB7XHJcbiAgICAgICAgICAgIG9wdGlvbnM6IFtdXHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICB0aGlzLnRyZW5kaW5nRGF0YURpc3BsYXlTZXJ2aWNlID0gbmV3IFRyZW5kaW5nRGF0YURpc3BsYXlTZXJ2aWNlKCk7XHJcbiAgICB9XHJcblxyXG4gICAgY29tcG9uZW50V2lsbFJlY2VpdmVQcm9wcyhuZXh0UHJvcHMpIHtcclxuICAgICAgICBpZih0aGlzLnByb3BzLm1ldGVySUQgIT0gbmV4dFByb3BzLm1ldGVySUQpXHJcbiAgICAgICAgICAgIHRoaXMuZ2V0RGF0YShuZXh0UHJvcHMubWV0ZXJJRCk7XHJcbiAgICB9XHJcblxyXG4gICAgY29tcG9uZW50RGlkTW91bnQoKSB7XHJcbiAgICAgICAgdGhpcy5nZXREYXRhKHRoaXMucHJvcHMubWV0ZXJJRCk7XHJcbiAgICB9XHJcblxyXG4gICAgZ2V0RGF0YShtZXRlcklEKSB7XHJcbiAgICAgICAgdGhpcy50cmVuZGluZ0RhdGFEaXNwbGF5U2VydmljZS5nZXRNZWFzdXJlbWVudHMobWV0ZXJJRCkuZG9uZShkYXRhID0+IHtcclxuICAgICAgICAgICAgaWYgKGRhdGEubGVuZ3RoID09IDApIHJldHVybjtcclxuXHJcbiAgICAgICAgICAgIHZhciB2YWx1ZSA9ICh0aGlzLnByb3BzLnZhbHVlID8gdGhpcy5wcm9wcy52YWx1ZSA6IGRhdGFbMF0uSUQpXHJcbiAgICAgICAgICAgIHZhciBvcHRpb25zID0gZGF0YS5tYXAoZCA9PiA8b3B0aW9uIGtleT17ZC5JRH0gdmFsdWU9e2QuSUR9PntkLk5hbWV9PC9vcHRpb24+KTtcclxuICAgICAgICAgICAgdGhpcy5zZXRTdGF0ZSh7IG9wdGlvbnMgfSk7XHJcbiAgICAgICAgfSk7XHJcblxyXG4gICAgfVxyXG5cclxuICAgIHJlbmRlcigpIHtcclxuICAgICAgICByZXR1cm4gKDxzZWxlY3QgY2xhc3NOYW1lPSdmb3JtLWNvbnRyb2wnIG9uQ2hhbmdlPXsoZSkgPT4geyB0aGlzLnByb3BzLm9uQ2hhbmdlKHsgbWVhc3VyZW1lbnRJRDogcGFyc2VJbnQoZS50YXJnZXQudmFsdWUpLCBtZWFzdXJlbWVudE5hbWU6IGUudGFyZ2V0LnNlbGVjdGVkT3B0aW9uc1swXS50ZXh0IH0pOyB9fSB2YWx1ZT17dGhpcy5wcm9wcy52YWx1ZX0+XHJcbiAgICAgICAgICAgIDxvcHRpb24gdmFsdWU9JzAnPjwvb3B0aW9uPlxyXG4gICAgICAgICAgICB7dGhpcy5zdGF0ZS5vcHRpb25zfVxyXG4gICAgICAgIDwvc2VsZWN0Pik7XHJcbiAgICB9XHJcblxyXG59IiwiLy8qKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuLy8gIE1ldGVySW5wdXQudHN4IC0gR2J0Y1xyXG4vL1xyXG4vLyAgQ29weXJpZ2h0IMKpIDIwMTgsIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZS4gIEFsbCBSaWdodHMgUmVzZXJ2ZWQuXHJcbi8vXHJcbi8vICBMaWNlbnNlZCB0byB0aGUgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlIChHUEEpIHVuZGVyIG9uZSBvciBtb3JlIGNvbnRyaWJ1dG9yIGxpY2Vuc2UgYWdyZWVtZW50cy4gU2VlXHJcbi8vICB0aGUgTk9USUNFIGZpbGUgZGlzdHJpYnV0ZWQgd2l0aCB0aGlzIHdvcmsgZm9yIGFkZGl0aW9uYWwgaW5mb3JtYXRpb24gcmVnYXJkaW5nIGNvcHlyaWdodCBvd25lcnNoaXAuXHJcbi8vICBUaGUgR1BBIGxpY2Vuc2VzIHRoaXMgZmlsZSB0byB5b3UgdW5kZXIgdGhlIE1JVCBMaWNlbnNlIChNSVQpLCB0aGUgXCJMaWNlbnNlXCI7IHlvdSBtYXkgbm90IHVzZSB0aGlzXHJcbi8vICBmaWxlIGV4Y2VwdCBpbiBjb21wbGlhbmNlIHdpdGggdGhlIExpY2Vuc2UuIFlvdSBtYXkgb2J0YWluIGEgY29weSBvZiB0aGUgTGljZW5zZSBhdDpcclxuLy9cclxuLy8gICAgICBodHRwOi8vb3BlbnNvdXJjZS5vcmcvbGljZW5zZXMvTUlUXHJcbi8vXHJcbi8vICBVbmxlc3MgYWdyZWVkIHRvIGluIHdyaXRpbmcsIHRoZSBzdWJqZWN0IHNvZnR3YXJlIGRpc3RyaWJ1dGVkIHVuZGVyIHRoZSBMaWNlbnNlIGlzIGRpc3RyaWJ1dGVkIG9uIGFuXHJcbi8vICBcIkFTLUlTXCIgQkFTSVMsIFdJVEhPVVQgV0FSUkFOVElFUyBPUiBDT05ESVRJT05TIE9GIEFOWSBLSU5ELCBlaXRoZXIgZXhwcmVzcyBvciBpbXBsaWVkLiBSZWZlciB0byB0aGVcclxuLy8gIExpY2Vuc2UgZm9yIHRoZSBzcGVjaWZpYyBsYW5ndWFnZSBnb3Zlcm5pbmcgcGVybWlzc2lvbnMgYW5kIGxpbWl0YXRpb25zLlxyXG4vL1xyXG4vLyAgQ29kZSBNb2RpZmljYXRpb24gSGlzdG9yeTpcclxuLy8gIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuLy8gIDA1LzI1LzIwMTggLSBCaWxseSBFcm5lc3RcclxuLy8gICAgICAgR2VuZXJhdGVkIG9yaWdpbmFsIHZlcnNpb24gb2Ygc291cmNlIGNvZGUuXHJcbi8vXHJcbi8vKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcblxyXG5pbXBvcnQgKiBhcyBSZWFjdCBmcm9tICdyZWFjdCc7XHJcbmltcG9ydCBQZXJpb2RpY0RhdGFEaXNwbGF5U2VydmljZSBmcm9tICcuLy4uL1RTL1NlcnZpY2VzL1BlcmlvZGljRGF0YURpc3BsYXknO1xyXG5pbXBvcnQgKiBhcyBfIGZyb20gXCJsb2Rhc2hcIjtcclxuXHJcbnR5cGUgbWV0ZXJzID0gIEFycmF5PHsgSUQ6IG51bWJlciwgTmFtZTogc3RyaW5nIH0+O1xyXG5cclxuZXhwb3J0IGRlZmF1bHQgY2xhc3MgTWV0ZXJJbnB1dCBleHRlbmRzIFJlYWN0LkNvbXBvbmVudDx7IHZhbHVlOiBudW1iZXIsIG9uQ2hhbmdlOiBGdW5jdGlvbiB9LCB7IG9wdGlvbnM6IEpTWC5FbGVtZW50W10gfT57XHJcbiAgICBwZXJpb2RpY0RhdGFEaXNwbGF5U2VydmljZTogUGVyaW9kaWNEYXRhRGlzcGxheVNlcnZpY2U7XHJcbiAgICBjb25zdHJ1Y3Rvcihwcm9wcykge1xyXG4gICAgICAgIHN1cGVyKHByb3BzKTtcclxuXHJcbiAgICAgICAgdGhpcy5zdGF0ZSA9IHtcclxuICAgICAgICAgICAgb3B0aW9uczogW11cclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIHRoaXMucGVyaW9kaWNEYXRhRGlzcGxheVNlcnZpY2UgPSBuZXcgUGVyaW9kaWNEYXRhRGlzcGxheVNlcnZpY2UoKTtcclxuICAgIH1cclxuXHJcbiAgICBjb21wb25lbnREaWRNb3VudCgpIHtcclxuXHJcbiAgICAgICAgdGhpcy5wZXJpb2RpY0RhdGFEaXNwbGF5U2VydmljZS5nZXRNZXRlcnMoKS5kb25lKGRhdGEgPT4ge1xyXG4gICAgICAgICAgICB0aGlzLnNldFN0YXRlKHsgb3B0aW9uczogZGF0YS5tYXAoZCA9PiA8b3B0aW9uIGtleT17ZC5JRH0gdmFsdWU9e2QuSUR9PntkLk5hbWV9PC9vcHRpb24+KSB9KTtcclxuICAgICAgICB9KTtcclxuICAgIH1cclxuXHJcbiAgICByZW5kZXIoKSB7XHJcbiAgICAgICAgcmV0dXJuIChcclxuICAgICAgICAgICAgPHNlbGVjdCBjbGFzc05hbWU9J2Zvcm0tY29udHJvbCcgb25DaGFuZ2U9eyhlKSA9PiB7IHRoaXMucHJvcHMub25DaGFuZ2UoeyBtZXRlcklEOiBwYXJzZUludChlLnRhcmdldC52YWx1ZSksIG1ldGVyTmFtZTogZS50YXJnZXQuc2VsZWN0ZWRPcHRpb25zWzBdLnRleHQsIG1lYXN1cmVtZW50SUQ6IG51bGwgfSk7IH19IHZhbHVlPXt0aGlzLnByb3BzLnZhbHVlfT5cclxuICAgICAgICAgICAgICAgIDxvcHRpb24gdmFsdWU9JzAnPjwvb3B0aW9uPlxyXG4gICAgICAgICAgICAgICAge3RoaXMuc3RhdGUub3B0aW9uc31cclxuICAgICAgICAgICAgPC9zZWxlY3Q+KTtcclxuICAgIH1cclxufSIsIi8vKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbi8vICBUcmVuZGluZ0NoYXJ0LnRzeCAtIEdidGNcclxuLy9cclxuLy8gIENvcHlyaWdodCDCqSAyMDE4LCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxyXG4vL1xyXG4vLyAgTGljZW5zZWQgdG8gdGhlIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZSAoR1BBKSB1bmRlciBvbmUgb3IgbW9yZSBjb250cmlidXRvciBsaWNlbnNlIGFncmVlbWVudHMuIFNlZVxyXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxyXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xyXG4vLyAgZmlsZSBleGNlcHQgaW4gY29tcGxpYW5jZSB3aXRoIHRoZSBMaWNlbnNlLiBZb3UgbWF5IG9idGFpbiBhIGNvcHkgb2YgdGhlIExpY2Vuc2UgYXQ6XHJcbi8vXHJcbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxyXG4vL1xyXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxyXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXHJcbi8vICBMaWNlbnNlIGZvciB0aGUgc3BlY2lmaWMgbGFuZ3VhZ2UgZ292ZXJuaW5nIHBlcm1pc3Npb25zIGFuZCBsaW1pdGF0aW9ucy5cclxuLy9cclxuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XHJcbi8vICAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vICAwNy8xOS8yMDE4IC0gQmlsbHkgRXJuZXN0XHJcbi8vICAgICAgIEdlbmVyYXRlZCBvcmlnaW5hbCB2ZXJzaW9uIG9mIHNvdXJjZSBjb2RlLlxyXG4vL1xyXG4vLyoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5cclxuaW1wb3J0ICogYXMgUmVhY3QgZnJvbSAncmVhY3QnO1xyXG4gaW1wb3J0ICogYXMgbW9tZW50IGZyb20gJ21vbWVudCc7XHJcbmltcG9ydCAqIGFzIF8gZnJvbSBcImxvZGFzaFwiO1xyXG5pbXBvcnQgJy4vLi4vZmxvdC9qcXVlcnkuZmxvdC5taW4uanMnO1xyXG5pbXBvcnQgJy4vLi4vZmxvdC9qcXVlcnkuZmxvdC5jcm9zc2hhaXIubWluLmpzJztcclxuaW1wb3J0ICcuLy4uL2Zsb3QvanF1ZXJ5LmZsb3QubmF2aWdhdGUubWluLmpzJztcclxuaW1wb3J0ICcuLy4uL2Zsb3QvanF1ZXJ5LmZsb3Quc2VsZWN0aW9uLm1pbi5qcyc7XHJcbmltcG9ydCAnLi8uLi9mbG90L2pxdWVyeS5mbG90LnRpbWUubWluLmpzJztcclxuaW1wb3J0ICcuLy4uL2Zsb3QvanF1ZXJ5LmZsb3QuYXhpc2xhYmVscy5qcyc7XHJcblxyXG5pbXBvcnQgeyBUcmVuZGluZ2NEYXRhRGlzcGxheSB9IGZyb20gJy4vZ2xvYmFsJ1xyXG5cclxuaW50ZXJmYWNlIFByb3BzIHsgc3RhcnREYXRlOiBzdHJpbmcsIGVuZERhdGU6IHN0cmluZywgZGF0YTogVHJlbmRpbmdjRGF0YURpc3BsYXkuTWVhc3VyZW1lbnRbXSwgYXhlczogVHJlbmRpbmdjRGF0YURpc3BsYXkuRmxvdEF4aXNbXSxzdGF0ZVNldHRlcjogRnVuY3Rpb24gfVxyXG5leHBvcnQgZGVmYXVsdCBjbGFzcyBUcmVuZGluZ0NoYXJ0IGV4dGVuZHMgUmVhY3QuQ29tcG9uZW50PFByb3BzLCB7fT57XHJcbiAgICBwbG90OiBhbnk7XHJcbiAgICB6b29tSWQ6IGFueTtcclxuICAgIGhvdmVyOiBudW1iZXI7XHJcbiAgICBzdGFydERhdGU6IHN0cmluZztcclxuICAgIGVuZERhdGU6IHN0cmluZztcclxuICAgIG9wdGlvbnM6IHsgY2FudmFzOiBib29sZWFuLCBsZWdlbmQ6IG9iamVjdCwgY3Jvc3NoYWlyOiBvYmplY3QsIHNlbGVjdGlvbjogb2JqZWN0LCBncmlkOiBvYmplY3QsIHhheGlzOiB7bW9kZTogc3RyaW5nLCB0aWNrTGVuZ3RoOiBudW1iZXIsIHJlc2VydmVTcGFjZTogYm9vbGVhbiwgdGlja3M6IEZ1bmN0aW9uLCB0aWNrRm9ybWF0dGVyOiBGdW5jdGlvbiwgbWF4OiBudW1iZXIsIG1pbjogbnVtYmVyfSwgeWF4aXM6IG9iamVjdCwgeWF4ZXM6IG9iamVjdFtdIH1cclxuXHJcbiAgICBjb25zdHJ1Y3Rvcihwcm9wcykge1xyXG4gICAgICAgIHN1cGVyKHByb3BzKTtcclxuXHJcbiAgICAgICAgdGhpcy5ob3ZlciA9IDA7XHJcbiAgICAgICAgdGhpcy5zdGFydERhdGUgPSBwcm9wcy5zdGFydERhdGU7XHJcbiAgICAgICAgdGhpcy5lbmREYXRlID0gcHJvcHMuZW5kRGF0ZTtcclxuXHJcbiAgICAgICAgdmFyIGN0cmwgPSB0aGlzO1xyXG5cclxuICAgICAgICB0aGlzLm9wdGlvbnMgPSB7XHJcbiAgICAgICAgICAgIGNhbnZhczogdHJ1ZSxcclxuICAgICAgICAgICAgbGVnZW5kOiB7IHNob3c6IHRydWUgfSxcclxuICAgICAgICAgICAgY3Jvc3NoYWlyOiB7IG1vZGU6IFwieFwiIH0sXHJcbiAgICAgICAgICAgIHNlbGVjdGlvbjogeyBtb2RlOiBcInhcIiB9LFxyXG4gICAgICAgICAgICBncmlkOiB7XHJcbiAgICAgICAgICAgICAgICBhdXRvSGlnaGxpZ2h0OiBmYWxzZSxcclxuICAgICAgICAgICAgICAgIGNsaWNrYWJsZTogdHJ1ZSxcclxuICAgICAgICAgICAgICAgIGhvdmVyYWJsZTogdHJ1ZSxcclxuICAgICAgICAgICAgICAgIG1hcmtpbmdzOiBbXVxyXG4gICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICB4YXhpczoge1xyXG4gICAgICAgICAgICAgICAgbW9kZTogXCJ0aW1lXCIsXHJcbiAgICAgICAgICAgICAgICB0aWNrTGVuZ3RoOiAxMCxcclxuICAgICAgICAgICAgICAgIHJlc2VydmVTcGFjZTogZmFsc2UsXHJcbiAgICAgICAgICAgICAgICB0aWNrczogZnVuY3Rpb24gKGF4aXMpIHtcclxuICAgICAgICAgICAgICAgICAgICB2YXIgdGlja3MgPSBbXSxcclxuICAgICAgICAgICAgICAgICAgICAgICAgc3RhcnQgPSBjdHJsLmZsb29ySW5CYXNlKGF4aXMubWluLCBheGlzLmRlbHRhKSxcclxuICAgICAgICAgICAgICAgICAgICAgICAgaSA9IDAsXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHYgPSBOdW1iZXIuTmFOLFxyXG4gICAgICAgICAgICAgICAgICAgICAgICBwcmV2O1xyXG5cclxuICAgICAgICAgICAgICAgICAgICBkbyB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHByZXYgPSB2O1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB2ID0gc3RhcnQgKyBpICogYXhpcy5kZWx0YTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgdGlja3MucHVzaCh2KTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgKytpO1xyXG4gICAgICAgICAgICAgICAgICAgIH0gd2hpbGUgKHYgPCBheGlzLm1heCAmJiB2ICE9IHByZXYpO1xyXG4gICAgICAgICAgICAgICAgICAgIHJldHVybiB0aWNrcztcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICB0aWNrRm9ybWF0dGVyOiBmdW5jdGlvbiAodmFsdWUsIGF4aXMpIHtcclxuICAgICAgICAgICAgICAgICAgICBpZiAoYXhpcy5kZWx0YSA+IDMgKiAyNCAqIDYwICogNjAgKiAxMDAwKVxyXG4gICAgICAgICAgICAgICAgICAgICAgICByZXR1cm4gbW9tZW50KHZhbHVlKS51dGMoKS5mb3JtYXQoXCJNTS9ERFwiKTtcclxuICAgICAgICAgICAgICAgICAgICBlbHNlXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHJldHVybiBtb21lbnQodmFsdWUpLnV0YygpLmZvcm1hdChcIk1NL0REIEhIOm1tXCIpO1xyXG4gICAgICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgICAgIG1heDogbnVsbCxcclxuICAgICAgICAgICAgICAgIG1pbjogbnVsbFxyXG4gICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICB5YXhpczoge1xyXG4gICAgICAgICAgICAgICAgbGFiZWxXaWR0aDogNTAsXHJcbiAgICAgICAgICAgICAgICBwYW5SYW5nZTogZmFsc2UsXHJcbiAgICAgICAgICAgICAgICAvL3RpY2tzOiAxLFxyXG4gICAgICAgICAgICAgICAgdGlja0xlbmd0aDogMTAsXHJcbiAgICAgICAgICAgICAgICB0aWNrRm9ybWF0dGVyOiBmdW5jdGlvbiAodmFsLCBheGlzKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWYgKGF4aXMuZGVsdGEgPiAxMDAwMDAwICYmICh2YWwgPiAxMDAwMDAwIHx8IHZhbCA8IC0xMDAwMDAwKSlcclxuICAgICAgICAgICAgICAgICAgICAgICAgcmV0dXJuICgodmFsIC8gMTAwMDAwMCkgfCAwKSArIFwiTVwiO1xyXG4gICAgICAgICAgICAgICAgICAgIGVsc2UgaWYgKGF4aXMuZGVsdGEgPiAxMDAwICYmICh2YWwgPiAxMDAwIHx8IHZhbCA8IC0xMDAwKSlcclxuICAgICAgICAgICAgICAgICAgICAgICAgcmV0dXJuICgodmFsIC8gMTAwMCkgfCAwKSArIFwiS1wiO1xyXG4gICAgICAgICAgICAgICAgICAgIGVsc2VcclxuICAgICAgICAgICAgICAgICAgICAgICAgcmV0dXJuIHZhbC50b0ZpeGVkKGF4aXMudGlja0RlY2ltYWxzKTtcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgeWF4ZXM6IFtdXHJcbiAgICAgICAgfVxyXG5cclxuICAgIH1cclxuXHJcblxyXG4gICAgY3JlYXRlRGF0YVJvd3MocHJvcHM6IFByb3BzKSB7XHJcbiAgICAgICAgLy8gaWYgc3RhcnQgYW5kIGVuZCBkYXRlIGFyZSBub3QgcHJvdmlkZWQgY2FsY3VsYXRlIHRoZW0gZnJvbSB0aGUgZGF0YSBzZXRcclxuICAgICAgICBpZiAodGhpcy5wbG90ICE9IHVuZGVmaW5lZCkgJCh0aGlzLnJlZnMuZ3JhcGgpLmNoaWxkcmVuKCkucmVtb3ZlKCk7XHJcblxyXG4gICAgICAgIHZhciBzdGFydFN0cmluZyA9IHRoaXMuc3RhcnREYXRlO1xyXG4gICAgICAgIHZhciBlbmRTdHJpbmcgPSB0aGlzLmVuZERhdGU7XHJcbiAgICAgICAgdmFyIG5ld1Zlc3NlbCA9IFtdO1xyXG5cclxuICAgICAgICBpZiAocHJvcHMuZGF0YSAhPSBudWxsKSB7XHJcbiAgICAgICAgICAgICQuZWFjaChwcm9wcy5kYXRhLCAoaSwgbWVhc3VyZW1lbnQpID0+IHtcclxuICAgICAgICAgICAgICAgIGlmKG1lYXN1cmVtZW50Lk1heGltdW0gJiYgbWVhc3VyZW1lbnQuRGF0YT8ubGVuZ3RoID4gMClcclxuICAgICAgICAgICAgICAgICAgICBuZXdWZXNzZWwucHVzaCh7IGxhYmVsOiBgJHttZWFzdXJlbWVudC5NZWFzdXJlbWVudE5hbWV9LU1heGAsIGRhdGE6IG1lYXN1cmVtZW50LkRhdGEubWFwKGQgPT4gW2QuVGltZSxkLk1heGltdW1dKSwgY29sb3I6IG1lYXN1cmVtZW50Lk1heENvbG9yLCB5YXhpczogbWVhc3VyZW1lbnQuQXhpcyB9KVxyXG4gICAgICAgICAgICAgICAgaWYgKG1lYXN1cmVtZW50LkF2ZXJhZ2UgJiYgbWVhc3VyZW1lbnQuRGF0YT8ubGVuZ3RoID4gMClcclxuICAgICAgICAgICAgICAgICAgICBuZXdWZXNzZWwucHVzaCh7IGxhYmVsOiBgJHttZWFzdXJlbWVudC5NZWFzdXJlbWVudE5hbWV9LUF2Z2AsIGRhdGE6IG1lYXN1cmVtZW50LkRhdGEubWFwKGQgPT4gW2QuVGltZSwgZC5BdmVyYWdlXSksIGNvbG9yOiBtZWFzdXJlbWVudC5BdmdDb2xvciwgeWF4aXM6IG1lYXN1cmVtZW50LkF4aXMgIH0pXHJcbiAgICAgICAgICAgICAgICBpZiAobWVhc3VyZW1lbnQuTWluaW11bSAmJiBtZWFzdXJlbWVudC5EYXRhPy5sZW5ndGggPiAwKVxyXG4gICAgICAgICAgICAgICAgICAgIG5ld1Zlc3NlbC5wdXNoKHsgbGFiZWw6IGAke21lYXN1cmVtZW50Lk1lYXN1cmVtZW50TmFtZX0tTWluYCwgZGF0YTogbWVhc3VyZW1lbnQuRGF0YS5tYXAoZCA9PiBbZC5UaW1lLCBkLk1pbmltdW1dKSwgY29sb3I6IG1lYXN1cmVtZW50Lk1pbkNvbG9yLCB5YXhpczogbWVhc3VyZW1lbnQuQXhpcyAgfSlcclxuXHJcbiAgICAgICAgICAgIH0pO1xyXG4gICAgICAgIH1cclxuICAgICAgICBuZXdWZXNzZWwucHVzaChbW3RoaXMuZ2V0TWlsbGlzZWNvbmRUaW1lKHN0YXJ0U3RyaW5nKSwgbnVsbF0sIFt0aGlzLmdldE1pbGxpc2Vjb25kVGltZShlbmRTdHJpbmcpLCBudWxsXV0pO1xyXG4gICAgICAgIHRoaXMub3B0aW9ucy54YXhpcy5tYXggPSB0aGlzLmdldE1pbGxpc2Vjb25kVGltZShlbmRTdHJpbmcpO1xyXG4gICAgICAgIHRoaXMub3B0aW9ucy54YXhpcy5taW4gPSB0aGlzLmdldE1pbGxpc2Vjb25kVGltZShzdGFydFN0cmluZyk7XHJcbiAgICAgICAgdGhpcy5vcHRpb25zLnlheGVzID0gdGhpcy5wcm9wcy5heGVzO1xyXG4gICAgICAgIHRoaXMucGxvdCA9ICgkIGFzIGFueSkucGxvdCgkKHRoaXMucmVmcy5ncmFwaCksIG5ld1Zlc3NlbCwgdGhpcy5vcHRpb25zKTtcclxuICAgICAgICB0aGlzLnBsb3RTZWxlY3RlZCgpO1xyXG4gICAgICAgIHRoaXMucGxvdFpvb20oKTtcclxuICAgICAgICB0aGlzLnBsb3RIb3ZlcigpO1xyXG4gICAgICAgIC8vdGhpcy5wbG90Q2xpY2soKTtcclxuICAgIH1cclxuXHJcbiAgICBjb21wb25lbnREaWRNb3VudCgpIHtcclxuICAgICAgICB0aGlzLmNyZWF0ZURhdGFSb3dzKHRoaXMucHJvcHMpO1xyXG4gICAgfVxyXG5cclxuICAgIGNvbXBvbmVudFdpbGxSZWNlaXZlUHJvcHMobmV4dFByb3BzKSB7XHJcbiAgICAgICAgdGhpcy5zdGFydERhdGUgPSBuZXh0UHJvcHMuc3RhcnREYXRlO1xyXG4gICAgICAgIHRoaXMuZW5kRGF0ZSA9IG5leHRQcm9wcy5lbmREYXRlO1xyXG4gICAgICAgIHRoaXMuY3JlYXRlRGF0YVJvd3MobmV4dFByb3BzKTtcclxuICAgIH1cclxuXHJcbiAgICByZW5kZXIoKSB7XHJcbiAgICAgICAgcmV0dXJuIDxkaXYgcmVmPXsnZ3JhcGgnfSBzdHlsZT17eyBoZWlnaHQ6ICdpbmhlcml0Jywgd2lkdGg6ICdpbmhlcml0J319PjwvZGl2PjtcclxuICAgIH1cclxuXHJcbiAgICAvLyByb3VuZCB0byBuZWFyYnkgbG93ZXIgbXVsdGlwbGUgb2YgYmFzZVxyXG4gICAgZmxvb3JJbkJhc2UobiwgYmFzZSkge1xyXG4gICAgICAgIHJldHVybiBiYXNlICogTWF0aC5mbG9vcihuIC8gYmFzZSk7XHJcbiAgICB9XHJcblxyXG4gICAgZ2V0TWlsbGlzZWNvbmRUaW1lKGRhdGUpIHtcclxuICAgICAgICB2YXIgbWlsbGlzZWNvbmRzID0gbW9tZW50LnV0YyhkYXRlKS52YWx1ZU9mKCk7XHJcbiAgICAgICAgcmV0dXJuIG1pbGxpc2Vjb25kcztcclxuICAgIH1cclxuXHJcbiAgICBnZXREYXRlU3RyaW5nKGZsb2F0KSB7XHJcbiAgICAgICAgdmFyIGRhdGUgPSBtb21lbnQudXRjKGZsb2F0KS5mb3JtYXQoJ1lZWVktTU0tRERUSEg6bW0nKTtcclxuICAgICAgICByZXR1cm4gZGF0ZTtcclxuICAgIH1cclxuXHJcbiAgICBwbG90U2VsZWN0ZWQoKSB7XHJcbiAgICAgICAgdmFyIGN0cmwgPSB0aGlzO1xyXG4gICAgICAgICQodGhpcy5yZWZzLmdyYXBoKS5vZmYoXCJwbG90c2VsZWN0ZWRcIik7XHJcbiAgICAgICAgJCh0aGlzLnJlZnMuZ3JhcGgpLmJpbmQoXCJwbG90c2VsZWN0ZWRcIiwgZnVuY3Rpb24gKGV2ZW50LCByYW5nZXMpIHtcclxuICAgICAgICAgICAgY3RybC5wcm9wcy5zdGF0ZVNldHRlcih7IHN0YXJ0RGF0ZTogY3RybC5nZXREYXRlU3RyaW5nKHJhbmdlcy54YXhpcy5mcm9tKSwgZW5kRGF0ZTogY3RybC5nZXREYXRlU3RyaW5nKHJhbmdlcy54YXhpcy50bykgfSk7XHJcbiAgICAgICAgfSk7XHJcbiAgICB9XHJcblxyXG4gICAgcGxvdFpvb20oKSB7XHJcbiAgICAgICAgdmFyIGN0cmwgPSB0aGlzO1xyXG4gICAgICAgICQodGhpcy5yZWZzLmdyYXBoKS5vZmYoXCJwbG90em9vbVwiKTtcclxuICAgICAgICAkKHRoaXMucmVmcy5ncmFwaCkuYmluZChcInBsb3R6b29tXCIsIGZ1bmN0aW9uIChldmVudCkge1xyXG4gICAgICAgICAgICB2YXIgbWluRGVsdGEgPSBudWxsO1xyXG4gICAgICAgICAgICB2YXIgbWF4RGVsdGEgPSA1O1xyXG4gICAgICAgICAgICB2YXIgeGF4aXMgPSBjdHJsLnBsb3QuZ2V0QXhlcygpLnhheGlzO1xyXG4gICAgICAgICAgICB2YXIgeGNlbnRlciA9IGN0cmwuaG92ZXI7XHJcbiAgICAgICAgICAgIHZhciB4bWluID0geGF4aXMub3B0aW9ucy5taW47XHJcbiAgICAgICAgICAgIHZhciB4bWF4ID0geGF4aXMub3B0aW9ucy5tYXg7XHJcbiAgICAgICAgICAgIHZhciBkYXRhbWluID0geGF4aXMuZGF0YW1pbjtcclxuICAgICAgICAgICAgdmFyIGRhdGFtYXggPSB4YXhpcy5kYXRhbWF4O1xyXG5cclxuICAgICAgICAgICAgdmFyIGRlbHRhTWFnbml0dWRlO1xyXG4gICAgICAgICAgICB2YXIgZGVsdGE7XHJcbiAgICAgICAgICAgIHZhciBmYWN0b3I7XHJcblxyXG4gICAgICAgICAgICBpZiAoeG1pbiA9PSBudWxsKVxyXG4gICAgICAgICAgICAgICAgeG1pbiA9IGRhdGFtaW47XHJcblxyXG4gICAgICAgICAgICBpZiAoeG1heCA9PSBudWxsKVxyXG4gICAgICAgICAgICAgICAgeG1heCA9IGRhdGFtYXg7XHJcblxyXG4gICAgICAgICAgICBpZiAoeG1pbiA9PSBudWxsIHx8IHhtYXggPT0gbnVsbClcclxuICAgICAgICAgICAgICAgIHJldHVybjtcclxuXHJcbiAgICAgICAgICAgIHhjZW50ZXIgPSBNYXRoLm1heCh4Y2VudGVyLCB4bWluKTtcclxuICAgICAgICAgICAgeGNlbnRlciA9IE1hdGgubWluKHhjZW50ZXIsIHhtYXgpO1xyXG5cclxuICAgICAgICAgICAgaWYgKChldmVudC5vcmlnaW5hbEV2ZW50IGFzIGFueSkud2hlZWxEZWx0YSAhPSB1bmRlZmluZWQpXHJcbiAgICAgICAgICAgICAgICBkZWx0YSA9IChldmVudC5vcmlnaW5hbEV2ZW50IGFzIGFueSkud2hlZWxEZWx0YTtcclxuICAgICAgICAgICAgZWxzZVxyXG4gICAgICAgICAgICAgICAgZGVsdGEgPSAtKGV2ZW50Lm9yaWdpbmFsRXZlbnQgYXMgYW55KS5kZXRhaWw7XHJcblxyXG4gICAgICAgICAgICBkZWx0YU1hZ25pdHVkZSA9IE1hdGguYWJzKGRlbHRhKTtcclxuXHJcbiAgICAgICAgICAgIGlmIChtaW5EZWx0YSA9PSBudWxsIHx8IGRlbHRhTWFnbml0dWRlIDwgbWluRGVsdGEpXHJcbiAgICAgICAgICAgICAgICBtaW5EZWx0YSA9IGRlbHRhTWFnbml0dWRlO1xyXG5cclxuICAgICAgICAgICAgZGVsdGFNYWduaXR1ZGUgLz0gbWluRGVsdGE7XHJcbiAgICAgICAgICAgIGRlbHRhTWFnbml0dWRlID0gTWF0aC5taW4oZGVsdGFNYWduaXR1ZGUsIG1heERlbHRhKTtcclxuICAgICAgICAgICAgZmFjdG9yID0gZGVsdGFNYWduaXR1ZGUgLyAxMDtcclxuXHJcbiAgICAgICAgICAgIGlmIChkZWx0YSA+IDApIHtcclxuICAgICAgICAgICAgICAgIHhtaW4gPSB4bWluICogKDEgLSBmYWN0b3IpICsgeGNlbnRlciAqIGZhY3RvcjtcclxuICAgICAgICAgICAgICAgIHhtYXggPSB4bWF4ICogKDEgLSBmYWN0b3IpICsgeGNlbnRlciAqIGZhY3RvcjtcclxuICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgIHhtaW4gPSAoeG1pbiAtIHhjZW50ZXIgKiBmYWN0b3IpIC8gKDEgLSBmYWN0b3IpO1xyXG4gICAgICAgICAgICAgICAgeG1heCA9ICh4bWF4IC0geGNlbnRlciAqIGZhY3RvcikgLyAoMSAtIGZhY3Rvcik7XHJcbiAgICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICAgIGlmICh4bWluID09IHhheGlzLm9wdGlvbnMueG1pbiAmJiB4bWF4ID09IHhheGlzLm9wdGlvbnMueG1heClcclxuICAgICAgICAgICAgICAgIHJldHVybjtcclxuXHJcbiAgICAgICAgICAgIGN0cmwuc3RhcnREYXRlID0gY3RybC5nZXREYXRlU3RyaW5nKHhtaW4pO1xyXG4gICAgICAgICAgICBjdHJsLmVuZERhdGUgPSBjdHJsLmdldERhdGVTdHJpbmcoeG1heCk7XHJcblxyXG4gICAgICAgICAgICBjdHJsLmNyZWF0ZURhdGFSb3dzKGN0cmwucHJvcHMpO1xyXG5cclxuICAgICAgICAgICAgY2xlYXJUaW1lb3V0KGN0cmwuem9vbUlkKTtcclxuXHJcbiAgICAgICAgICAgIGN0cmwuem9vbUlkID0gc2V0VGltZW91dCgoKSA9PiB7XHJcbiAgICAgICAgICAgICAgICBjdHJsLnByb3BzLnN0YXRlU2V0dGVyKHsgc3RhcnREYXRlOiBjdHJsLmdldERhdGVTdHJpbmcoeG1pbiksIGVuZERhdGU6IGN0cmwuZ2V0RGF0ZVN0cmluZyh4bWF4KSB9KTtcclxuICAgICAgICAgICAgfSwgMjUwKTtcclxuICAgICAgICB9KTtcclxuXHJcbiAgICB9XHJcblxyXG4gICAgcGxvdEhvdmVyKCkge1xyXG4gICAgICAgIHZhciBjdHJsID0gdGhpcztcclxuICAgICAgICAkKHRoaXMucmVmcy5ncmFwaCkub2ZmKFwicGxvdGhvdmVyXCIpO1xyXG4gICAgICAgICQodGhpcy5yZWZzLmdyYXBoKS5iaW5kKFwicGxvdGhvdmVyXCIsIGZ1bmN0aW9uIChldmVudCwgcG9zLCBpdGVtKSB7XHJcbiAgICAgICAgICAgIGN0cmwuaG92ZXIgPSBwb3MueDtcclxuICAgICAgICB9KTtcclxuICAgIH1cclxuXHJcblxyXG59IiwiLy8qKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuLy8gIFRyZW5kaW5nRGF0YURpc3BsYXkudHN4IC0gR2J0Y1xyXG4vL1xyXG4vLyAgQ29weXJpZ2h0IMKpIDIwMTgsIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZS4gIEFsbCBSaWdodHMgUmVzZXJ2ZWQuXHJcbi8vXHJcbi8vICBMaWNlbnNlZCB0byB0aGUgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlIChHUEEpIHVuZGVyIG9uZSBvciBtb3JlIGNvbnRyaWJ1dG9yIGxpY2Vuc2UgYWdyZWVtZW50cy4gU2VlXHJcbi8vICB0aGUgTk9USUNFIGZpbGUgZGlzdHJpYnV0ZWQgd2l0aCB0aGlzIHdvcmsgZm9yIGFkZGl0aW9uYWwgaW5mb3JtYXRpb24gcmVnYXJkaW5nIGNvcHlyaWdodCBvd25lcnNoaXAuXHJcbi8vICBUaGUgR1BBIGxpY2Vuc2VzIHRoaXMgZmlsZSB0byB5b3UgdW5kZXIgdGhlIE1JVCBMaWNlbnNlIChNSVQpLCB0aGUgXCJMaWNlbnNlXCI7IHlvdSBtYXkgbm90IHVzZSB0aGlzXHJcbi8vICBmaWxlIGV4Y2VwdCBpbiBjb21wbGlhbmNlIHdpdGggdGhlIExpY2Vuc2UuIFlvdSBtYXkgb2J0YWluIGEgY29weSBvZiB0aGUgTGljZW5zZSBhdDpcclxuLy9cclxuLy8gICAgICBodHRwOi8vb3BlbnNvdXJjZS5vcmcvbGljZW5zZXMvTUlUXHJcbi8vXHJcbi8vICBVbmxlc3MgYWdyZWVkIHRvIGluIHdyaXRpbmcsIHRoZSBzdWJqZWN0IHNvZnR3YXJlIGRpc3RyaWJ1dGVkIHVuZGVyIHRoZSBMaWNlbnNlIGlzIGRpc3RyaWJ1dGVkIG9uIGFuXHJcbi8vICBcIkFTLUlTXCIgQkFTSVMsIFdJVEhPVVQgV0FSUkFOVElFUyBPUiBDT05ESVRJT05TIE9GIEFOWSBLSU5ELCBlaXRoZXIgZXhwcmVzcyBvciBpbXBsaWVkLiBSZWZlciB0byB0aGVcclxuLy8gIExpY2Vuc2UgZm9yIHRoZSBzcGVjaWZpYyBsYW5ndWFnZSBnb3Zlcm5pbmcgcGVybWlzc2lvbnMgYW5kIGxpbWl0YXRpb25zLlxyXG4vL1xyXG4vLyAgQ29kZSBNb2RpZmljYXRpb24gSGlzdG9yeTpcclxuLy8gIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuLy8gIDA3LzE5LzIwMTggLSBCaWxseSBFcm5lc3RcclxuLy8gICAgICAgR2VuZXJhdGVkIG9yaWdpbmFsIHZlcnNpb24gb2Ygc291cmNlIGNvZGUuXHJcbi8vXHJcbi8vKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcblxyXG5pbXBvcnQgKiBhcyBSZWFjdCBmcm9tICdyZWFjdCc7XHJcbmltcG9ydCAqIGFzIFJlYWN0RE9NIGZyb20gJ3JlYWN0LWRvbSc7XHJcbmltcG9ydCBUcmVuZGluZ0RhdGFEaXNwbGF5U2VydmljZSBmcm9tICcuLy4uL1RTL1NlcnZpY2VzL1RyZW5kaW5nRGF0YURpc3BsYXknO1xyXG5pbXBvcnQgY3JlYXRlSGlzdG9yeSBmcm9tIFwiaGlzdG9yeS9jcmVhdGVCcm93c2VySGlzdG9yeVwiXHJcbmltcG9ydCAqIGFzIHF1ZXJ5U3RyaW5nIGZyb20gXCJxdWVyeS1zdHJpbmdcIjtcclxuaW1wb3J0ICogYXMgbW9tZW50IGZyb20gJ21vbWVudCc7XHJcbmltcG9ydCAqIGFzIF8gZnJvbSBcImxvZGFzaFwiO1xyXG5pbXBvcnQgTWV0ZXJJbnB1dCBmcm9tICcuL01ldGVySW5wdXQnO1xyXG5pbXBvcnQgTWVhc3VyZW1lbnRJbnB1dCBmcm9tICcuL01lYXN1cmVtZW50SW5wdXQnO1xyXG5pbXBvcnQgVHJlbmRpbmdDaGFydCBmcm9tICcuL1RyZW5kaW5nQ2hhcnQnO1xyXG5pbXBvcnQgRGF0ZVRpbWVSYW5nZVBpY2tlciBmcm9tICcuL0RhdGVUaW1lUmFuZ2VQaWNrZXInO1xyXG5pbXBvcnQgeyAgVHJlbmRpbmdjRGF0YURpc3BsYXkgfSBmcm9tICcuL2dsb2JhbCdcclxuaW1wb3J0IHsgUmFuZG9tQ29sb3IgfSBmcm9tICdAZ3BhLWdlbXN0b25lL2hlbHBlci1mdW5jdGlvbnMnO1xyXG5cclxuY29uc3QgVHJlbmRpbmdEYXRhRGlzcGxheSA9ICgpID0+IHtcclxuICAgIGxldCB0cmVuZGluZ0RhdGFEaXNwbGF5U2VydmljZSA9IG5ldyBUcmVuZGluZ0RhdGFEaXNwbGF5U2VydmljZSgpO1xyXG4gICAgY29uc3QgcmVzaXplSWQgPSBSZWFjdC51c2VSZWYobnVsbCk7XHJcbiAgICBjb25zdCBsb2FkZXIgPSBSZWFjdC51c2VSZWYobnVsbCk7XHJcblxyXG4gICAgbGV0IGhpc3RvcnkgPSBjcmVhdGVIaXN0b3J5KCk7XHJcblxyXG4gICAgbGV0IHF1ZXJ5ID0gcXVlcnlTdHJpbmcucGFyc2UoaGlzdG9yeVsnbG9jYXRpb24nXS5zZWFyY2gpO1xyXG5cclxuICAgIGNvbnN0IFttZWFzdXJlbWVudHMsIHNldE1lYXN1cmVtZW50c10gPSBSZWFjdC51c2VTdGF0ZTxUcmVuZGluZ2NEYXRhRGlzcGxheS5NZWFzdXJlbWVudFtdPihzZXNzaW9uU3RvcmFnZS5nZXRJdGVtKCdUcmVuZGluZ0RhdGFEaXNwbGF5LW1lYXN1cmVtZW50cycpID09IG51bGwgPyBbXSA6IEpTT04ucGFyc2Uoc2Vzc2lvblN0b3JhZ2UuZ2V0SXRlbSgnVHJlbmRpbmdEYXRhRGlzcGxheS1tZWFzdXJlbWVudHMnKSkpO1xyXG4gICAgY29uc3QgW3dpZHRoLCBzZXRXaWR0aF0gPSBSZWFjdC51c2VTdGF0ZTxudW1iZXI+KHdpbmRvdy5pbm5lcldpZHRoIC0gNDc1KTtcclxuICAgIGNvbnN0IFtzdGFydERhdGUsIHNldFN0YXJ0RGF0ZV0gPSBSZWFjdC51c2VTdGF0ZTxzdHJpbmc+KHF1ZXJ5WydzdGFydERhdGUnXSAhPSB1bmRlZmluZWQgPyBxdWVyeVsnc3RhcnREYXRlJ10gOiBtb21lbnQoKS5zdWJ0cmFjdCg3LCAnZGF5JykuZm9ybWF0KCdZWVlZLU1NLUREVEhIOm1tJykpO1xyXG4gICAgY29uc3QgW2VuZERhdGUsIHNldEVuZERhdGVdID0gUmVhY3QudXNlU3RhdGU8c3RyaW5nPihxdWVyeVsnZW5kRGF0ZSddICE9IHVuZGVmaW5lZCA/IHF1ZXJ5WydlbmREYXRlJ10gOiBtb21lbnQoKS5mb3JtYXQoJ1lZWVktTU0tRERUSEg6bW0nKSk7XHJcbiAgICBjb25zdCBbYXhlcywgc2V0QXhlc10gPSBSZWFjdC51c2VTdGF0ZTxUcmVuZGluZ2NEYXRhRGlzcGxheS5GbG90QXhpc1tdPihzZXNzaW9uU3RvcmFnZS5nZXRJdGVtKCdUcmVuZGluZ0RhdGFEaXNwbGF5LWF4ZXMnKSA9PSBudWxsID8gW3sgYXhpc0xhYmVsOiAnRGVmYXVsdCcsIGNvbG9yOiAnYmxhY2snLCBwb3NpdGlvbjogJ2xlZnQnIH1dIDogSlNPTi5wYXJzZShzZXNzaW9uU3RvcmFnZS5nZXRJdGVtKCdUcmVuZGluZ0RhdGFEaXNwbGF5LWF4ZXMnKSkpO1xyXG5cclxuICAgIFJlYWN0LnVzZUVmZmVjdCgoKSA9PiB7XHJcbiAgICAgICAgd2luZG93LmFkZEV2ZW50TGlzdGVuZXIoXCJyZXNpemVcIiwgaGFuZGxlU2NyZWVuU2l6ZUNoYW5nZS5iaW5kKHRoaXMpKTtcclxuICAgICAgICAvL2lmICh0aGlzLnN0YXRlLm1lYXN1cmVtZW50SUQgIT0gMCkgZ2V0RGF0YSgpO1xyXG5cclxuICAgICAgICBoaXN0b3J5WydsaXN0ZW4nXSgobG9jYXRpb24sIGFjdGlvbikgPT4ge1xyXG4gICAgICAgICAgICBsZXQgcXVlcnkgPSBxdWVyeVN0cmluZy5wYXJzZShoaXN0b3J5Wydsb2NhdGlvbiddLnNlYXJjaCk7XHJcbiAgICAgICAgICAgIHNldFN0YXJ0RGF0ZShxdWVyeVsnc3RhcnREYXRlJ10gIT0gdW5kZWZpbmVkID8gcXVlcnlbJ3N0YXJ0RGF0ZSddIDogbW9tZW50KCkuc3VidHJhY3QoNywgJ2RheScpLmZvcm1hdCgnWVlZWS1NTS1ERFRISDptbScpKTtcclxuICAgICAgICAgICAgc2V0RW5kRGF0ZShxdWVyeVsnZW5kRGF0ZSddICE9IHVuZGVmaW5lZCA/IHF1ZXJ5WydlbmREYXRlJ10gOiBtb21lbnQoKS5mb3JtYXQoJ1lZWVktTU0tRERUSEg6bW0nKSk7XHJcbiAgICAgICAgfSk7XHJcblxyXG4gICAgICAgIHJldHVybiAoKSA9PiAkKHdpbmRvdykub2ZmKCdyZXNpemUnKTtcclxuICAgIH0sIFtdKTtcclxuXHJcbiAgICBSZWFjdC51c2VFZmZlY3QoKCkgPT4ge1xyXG4gICAgICAgIGlmIChtZWFzdXJlbWVudHMubGVuZ3RoID09IDApIHJldHVybjtcclxuICAgICAgICBnZXREYXRhKCk7XHJcbiAgICB9LCBbbWVhc3VyZW1lbnRzLmxlbmd0aCwgc3RhcnREYXRlLCBlbmREYXRlXSk7XHJcblxyXG4gICAgUmVhY3QudXNlRWZmZWN0KCgpID0+IHtcclxuICAgICAgICBoaXN0b3J5WydwdXNoJ10oJ1RyZW5kaW5nRGF0YURpc3BsYXkuY3NodG1sPycgKyBxdWVyeVN0cmluZy5zdHJpbmdpZnkoe3N0YXJ0RGF0ZSwgZW5kRGF0ZX0sIHsgZW5jb2RlOiBmYWxzZSB9KSlcclxuICAgIH0sIFtzdGFydERhdGUsZW5kRGF0ZV0pO1xyXG5cclxuICAgIFJlYWN0LnVzZUVmZmVjdCgoKSA9PiB7XHJcbiAgICAgICAgc2Vzc2lvblN0b3JhZ2Uuc2V0SXRlbSgnVHJlbmRpbmdEYXRhRGlzcGxheS1tZWFzdXJlbWVudHMnLCBKU09OLnN0cmluZ2lmeShtZWFzdXJlbWVudHMubWFwKG1zID0+ICh7IElEOiBtcy5JRCwgTWV0ZXJJRDogbXMuTWV0ZXJJRCxNZXRlck5hbWU6IG1zLk1ldGVyTmFtZSxNZWFzdXJlbWVudE5hbWU6IG1zLk1lYXN1cmVtZW50TmFtZSxBdmVyYWdlOiBtcy5BdmVyYWdlLEF2Z0NvbG9yOiBtcy5BdmdDb2xvcixNYXhpbXVtOiBtcy5NYXhpbXVtLCBNYXhDb2xvcjogbXMuTWF4Q29sb3IsIE1pbmltdW06IG1zLk1pbmltdW0sIE1pbkNvbG9yOiBtcy5NaW5Db2xvciwgQXhpczogbXMuQXhpc30pKSkpXHJcbiAgICB9LCBbbWVhc3VyZW1lbnRzXSk7XHJcblxyXG4gICAgUmVhY3QudXNlRWZmZWN0KCgpID0+IHtcclxuICAgICAgICBzZXNzaW9uU3RvcmFnZS5zZXRJdGVtKCdUcmVuZGluZ0RhdGFEaXNwbGF5LWF4ZXMnLCBKU09OLnN0cmluZ2lmeShheGVzKSlcclxuICAgIH0sIFtheGVzXSk7XHJcblxyXG4gICAgZnVuY3Rpb24gZ2V0RGF0YSgpIHtcclxuICAgICAgICAkKGxvYWRlci5jdXJyZW50KS5zaG93KCk7XHJcbiAgICAgICAgdHJlbmRpbmdEYXRhRGlzcGxheVNlcnZpY2UuZ2V0UG9zdERhdGEobWVhc3VyZW1lbnRzLCBzdGFydERhdGUsIGVuZERhdGUsIHdpZHRoKS5kb25lKGRhdGEgPT4ge1xyXG4gICAgICAgICAgICBsZXQgbWVhcyA9WyAuLi5tZWFzdXJlbWVudHMgXTtcclxuICAgICAgICAgICAgZm9yIChsZXQga2V5IG9mIE9iamVjdC5rZXlzKGRhdGEpKSB7XHJcbiAgICAgICAgICAgICAgICBsZXQgaSA9IG1lYXMuZmluZEluZGV4KHggPT4geC5JRC50b1N0cmluZygpID09PSBrZXkpO1xyXG4gICAgICAgICAgICAgICAgbWVhc1tpXS5EYXRhID0gZGF0YVtrZXldO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIHNldE1lYXN1cmVtZW50cyhtZWFzKVxyXG5cclxuICAgICAgICAgICAgJChsb2FkZXIuY3VycmVudCkuaGlkZSgpXHJcbiAgICAgICAgfSk7XHJcbiAgICB9XHJcblxyXG5cclxuICAgIGZ1bmN0aW9uIGhhbmRsZVNjcmVlblNpemVDaGFuZ2UoKSB7XHJcbiAgICAgICAgY2xlYXJUaW1lb3V0KHRoaXMucmVzaXplSWQpO1xyXG4gICAgICAgIHRoaXMucmVzaXplSWQgPSBzZXRUaW1lb3V0KCgpID0+IHtcclxuICAgICAgICB9LCA1MDApO1xyXG4gICAgfVxyXG5cclxuICAgIGxldCBoZWlnaHQgPSB3aW5kb3cuaW5uZXJIZWlnaHQgLSAkKCcjbmF2YmFyJykuaGVpZ2h0KCk7XHJcbiAgICBsZXQgbWVudVdpZHRoID0gMjUwO1xyXG4gICAgbGV0IHNpZGVXaWR0aCA9IDQwMDtcclxuICAgIGxldCB0b3AgPSAkKCcjbmF2YmFyJykuaGVpZ2h0KCkgLSAzMDtcclxuICAgIHJldHVybiAoXHJcbiAgICAgICAgPGRpdj5cclxuICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJzY3JlZW5cIiBzdHlsZT17eyBoZWlnaHQ6IGhlaWdodCwgd2lkdGg6IHdpbmRvdy5pbm5lcldpZHRoLCBwb3NpdGlvbjogJ3JlbGF0aXZlJywgdG9wOiB0b3AgfX0+XHJcbiAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cInZlcnRpY2FsLW1lbnVcIiBzdHlsZT17e21heEhlaWdodDogaGVpZ2h0LCBvdmVyZmxvd1k6ICdhdXRvJyB9fT5cclxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImZvcm0tZ3JvdXBcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPGxhYmVsPlRpbWUgUmFuZ2U6IDwvbGFiZWw+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxEYXRlVGltZVJhbmdlUGlja2VyIHN0YXJ0RGF0ZT17c3RhcnREYXRlfSBlbmREYXRlPXtlbmREYXRlfSBzdGF0ZVNldHRlcj17KG9iaikgPT4ge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgaWYoc3RhcnREYXRlICE9IG9iai5zdGFydERhdGUpXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgc2V0U3RhcnREYXRlKG9iai5zdGFydERhdGUpO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgaWYgKGVuZERhdGUgIT0gb2JqLmVuZERhdGUpXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgc2V0RW5kRGF0ZShvYmouZW5kRGF0ZSk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIH19IC8+XHJcbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJmb3JtLWdyb3VwXCIgc3R5bGU9e3toZWlnaHQ6IDUwfX0+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgc3R5bGU9e3sgZmxvYXQ6ICdsZWZ0JyB9fSByZWY9e2xvYWRlcn0gaGlkZGVuPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBzdHlsZT17eyBib3JkZXI6ICc1cHggc29saWQgI2YzZjNmMycsIFdlYmtpdEFuaW1hdGlvbjogJ3NwaW4gMXMgbGluZWFyIGluZmluaXRlJywgYW5pbWF0aW9uOiAnc3BpbiAxcyBsaW5lYXIgaW5maW5pdGUnLCBib3JkZXJUb3A6ICc1cHggc29saWQgIzU1NScsIGJvcmRlclJhZGl1czogJzUwJScsIHdpZHRoOiAnMjVweCcsIGhlaWdodDogJzI1cHgnIH19PjwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPHNwYW4+TG9hZGluZy4uLjwvc3Bhbj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcblxyXG5cclxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImZvcm0tZ3JvdXBcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJwYW5lbC1ncm91cFwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJwYW5lbCBwYW5lbC1kZWZhdWx0XCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJwYW5lbC1oZWFkaW5nXCIgc3R5bGU9e3sgcG9zaXRpb246ICdyZWxhdGl2ZScsIGhlaWdodDogNjB9fT5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGg0IGNsYXNzTmFtZT1cInBhbmVsLXRpdGxlXCIgc3R5bGU9e3sgcG9zaXRpb246ICdhYnNvbHV0ZScsIGxlZnQ6IDE1LCB3aWR0aDogJzYwJScgfX0+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8YSBkYXRhLXRvZ2dsZT1cImNvbGxhcHNlXCIgaHJlZj1cIiNNZWFzdXJlbWVudENvbGxhcHNlXCI+TWVhc3VyZW1lbnRzOjwvYT5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9oND5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPEFkZE1lYXN1cmVtZW50IEFkZD17KG1zbnQpID0+IHNldE1lYXN1cmVtZW50cyhtZWFzdXJlbWVudHMuY29uY2F0KG1zbnQpKX0gLz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGlkPSdNZWFzdXJlbWVudENvbGxhcHNlJyBjbGFzc05hbWU9J3BhbmVsLWNvbGxhcHNlJz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHVsIGNsYXNzTmFtZT0nbGlzdC1ncm91cCc+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB7bWVhc3VyZW1lbnRzLm1hcCgobXMsIGkpID0+IDxNZWFzdXJlbWVudFJvdyBrZXk9e2l9IE1lYXN1cmVtZW50PXttc30gTWVhc3VyZW1lbnRzPXttZWFzdXJlbWVudHN9IEF4ZXM9e2F4ZXN9IEluZGV4PXtpfSBTZXRNZWFzdXJlbWVudHM9e3NldE1lYXN1cmVtZW50c30gLz4pfVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3VsPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuXHJcblxyXG4gICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiZm9ybS1ncm91cFwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cInBhbmVsLWdyb3VwXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cInBhbmVsIHBhbmVsLWRlZmF1bHRcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cInBhbmVsLWhlYWRpbmdcIiBzdHlsZT17eyBwb3NpdGlvbjogJ3JlbGF0aXZlJywgaGVpZ2h0OiA2MCB9fT5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGg0IGNsYXNzTmFtZT1cInBhbmVsLXRpdGxlXCIgc3R5bGU9e3sgcG9zaXRpb246ICdhYnNvbHV0ZScsIGxlZnQ6IDE1LCB3aWR0aDogJzYwJScgfX0+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8YSBkYXRhLXRvZ2dsZT1cImNvbGxhcHNlXCIgaHJlZj1cIiNheGVzQ29sbGFwc2VcIj5BeGVzOjwvYT5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9oND5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPEFkZEF4aXMgQWRkPXsoYXhpcykgPT4gc2V0QXhlcyhheGVzLmNvbmNhdChheGlzKSl9IC8+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBpZD0nYXhlc0NvbGxhcHNlJyBjbGFzc05hbWU9J3BhbmVsLWNvbGxhcHNlJz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHVsIGNsYXNzTmFtZT0nbGlzdC1ncm91cCc+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB7YXhlcy5tYXAoKGF4aXMsIGkpID0+IDxBeGlzUm93IGtleT17aX0gQXhlcz17YXhlc30gSW5kZXg9e2l9IFNldEF4ZXM9e3NldEF4ZXN9Lz4pfVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3VsPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuXHJcbiAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwid2F2ZWZvcm0tdmlld2VyXCIgc3R5bGU9e3sgd2lkdGg6IHdpbmRvdy5pbm5lcldpZHRoIC0gbWVudVdpZHRoLCBoZWlnaHQ6IGhlaWdodCwgZmxvYXQ6ICdsZWZ0JywgbGVmdDogMCB9fT5cclxuICAgICAgICAgICAgICAgICAgICA8VHJlbmRpbmdDaGFydCBzdGFydERhdGU9e3N0YXJ0RGF0ZX0gZW5kRGF0ZT17ZW5kRGF0ZX0gZGF0YT17bWVhc3VyZW1lbnRzfSBheGVzPXtheGVzfSBzdGF0ZVNldHRlcj17KG9iamVjdCkgPT4ge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBzZXRTdGFydERhdGUob2JqZWN0LnN0YXJ0RGF0ZSk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHNldEVuZERhdGUob2JqZWN0LmVuZERhdGUpO1xyXG4gICAgICAgICAgICAgICAgICAgIH19IC8+XHJcbiAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgPC9kaXY+XHJcbiAgICApO1xyXG59XHJcblxyXG5jb25zdCBBZGRNZWFzdXJlbWVudCA9IChwcm9wczogeyBBZGQ6IChtc250OlRyZW5kaW5nY0RhdGFEaXNwbGF5Lk1lYXN1cmVtZW50KSA9PiB2b2lkfSkgPT4ge1xyXG4gICAgY29uc3QgW21lYXN1cmVtZW50LCBzZXRNZWFzdXJlbWVudF0gPSBSZWFjdC51c2VTdGF0ZTxUcmVuZGluZ2NEYXRhRGlzcGxheS5NZWFzdXJlbWVudD4oeyBJRDogMCwgTWV0ZXJJRDogMCwgTWV0ZXJOYW1lOiAnJywgTWVhc3VyZW1lbnROYW1lOiAnJywgTWF4aW11bTogdHJ1ZSwgTWF4Q29sb3I6IFJhbmRvbUNvbG9yKCksIEF2ZXJhZ2U6IHRydWUsIEF2Z0NvbG9yOiBSYW5kb21Db2xvcigpLCBNaW5pbXVtOiB0cnVlLCBNaW5Db2xvcjogUmFuZG9tQ29sb3IoKSwgQXhpczogMSB9KTtcclxuXHJcbiAgICByZXR1cm4gKFxyXG4gICAgICAgIDw+XHJcbiAgICAgICAgICAgIDxidXR0b24gdHlwZT1cImJ1dHRvblwiIHN0eWxlPXt7IHBvc2l0aW9uOiAnYWJzb2x1dGUnLCByaWdodDogMTV9fSBjbGFzc05hbWU9XCJidG4gYnRuLWluZm9cIiBkYXRhLXRvZ2dsZT1cIm1vZGFsXCIgZGF0YS10YXJnZXQ9XCIjbWVhc3VyZW1lbnRNb2RhbFwiIG9uQ2xpY2s9eygpID0+IHtcclxuICAgICAgICAgICAgICAgIHNldE1lYXN1cmVtZW50KHsgSUQ6IDAsIE1ldGVySUQ6IDAsIE1ldGVyTmFtZTogJycsIE1lYXN1cmVtZW50TmFtZTogJycsIE1heGltdW06IHRydWUsIE1heENvbG9yOiBSYW5kb21Db2xvcigpLCBBdmVyYWdlOiB0cnVlLCBBdmdDb2xvcjogUmFuZG9tQ29sb3IoKSwgTWluaW11bTogdHJ1ZSwgTWluQ29sb3I6IFJhbmRvbUNvbG9yKCksIEF4aXM6IDEgfSk7XHJcbiAgICAgICAgICAgIH19PkFkZDwvYnV0dG9uPlxyXG4gICAgICAgICAgICA8ZGl2IGlkPVwibWVhc3VyZW1lbnRNb2RhbFwiIGNsYXNzTmFtZT1cIm1vZGFsIGZhZGVcIiByb2xlPVwiZGlhbG9nXCI+XHJcbiAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cIm1vZGFsLWRpYWxvZ1wiPlxyXG4gICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwibW9kYWwtY29udGVudFwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cIm1vZGFsLWhlYWRlclwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPGJ1dHRvbiB0eXBlPVwiYnV0dG9uXCIgY2xhc3NOYW1lPVwiY2xvc2VcIiBkYXRhLWRpc21pc3M9XCJtb2RhbFwiPiZ0aW1lczs8L2J1dHRvbj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxoNCBjbGFzc05hbWU9XCJtb2RhbC10aXRsZVwiPkFkZCBNZWFzdXJlbWVudDwvaDQ+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cIm1vZGFsLWJvZHlcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiZm9ybS1ncm91cFwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxsYWJlbD5NZXRlcjogPC9sYWJlbD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8TWV0ZXJJbnB1dCB2YWx1ZT17bWVhc3VyZW1lbnQuTWV0ZXJJRH0gb25DaGFuZ2U9eyhvYmopID0+IHNldE1lYXN1cmVtZW50KHsgLi4ubWVhc3VyZW1lbnQsIE1ldGVySUQ6IG9iai5tZXRlcklELCBNZXRlck5hbWU6IG9iai5tZXRlck5hbWUgfSl9IC8+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImZvcm0tZ3JvdXBcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8bGFiZWw+TWVhc3VyZW1lbnQ6IDwvbGFiZWw+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPE1lYXN1cmVtZW50SW5wdXQgbWV0ZXJJRD17bWVhc3VyZW1lbnQuTWV0ZXJJRH0gdmFsdWU9e21lYXN1cmVtZW50LklEfSBvbkNoYW5nZT17KG9iaikgPT4gc2V0TWVhc3VyZW1lbnQoeyAuLi5tZWFzdXJlbWVudCwgSUQ6IG9iai5tZWFzdXJlbWVudElELCBNZWFzdXJlbWVudE5hbWU6IG9iai5tZWFzdXJlbWVudE5hbWUgfSl9IC8+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cInJvd1wiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiY29sLWxnLTZcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJjaGVja2JveFwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGxhYmVsPjxpbnB1dCB0eXBlPVwiY2hlY2tib3hcIiBjaGVja2VkPXttZWFzdXJlbWVudC5NYXhpbXVtfSB2YWx1ZT17bWVhc3VyZW1lbnQuTWF4aW11bS50b1N0cmluZygpfSBvbkNoYW5nZT17KCkgPT4gc2V0TWVhc3VyZW1lbnQoeyAuLi5tZWFzdXJlbWVudCwgTWF4aW11bTogIW1lYXN1cmVtZW50Lk1heGltdW0gfSkgfS8+IE1heGltdW08L2xhYmVsPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImNvbC1sZy02XCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxpbnB1dCB0eXBlPVwiY29sb3JcIiBzdHlsZT17e3RleHRBbGlnbjonbGVmdCd9fSBjbGFzc05hbWU9XCJmb3JtLWNvbnRyb2xcIiB2YWx1ZT17bWVhc3VyZW1lbnQuTWF4Q29sb3J9IG9uQ2hhbmdlPXsoZXZ0KSA9PiBzZXRNZWFzdXJlbWVudCh7IC4uLm1lYXN1cmVtZW50LCBNYXhDb2xvcjogZXZ0LnRhcmdldC52YWx1ZSB9KX0gLz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwicm93XCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJjb2wtbGctNlwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImNoZWNrYm94XCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8bGFiZWw+PGlucHV0IHR5cGU9XCJjaGVja2JveFwiIGNoZWNrZWQ9e21lYXN1cmVtZW50LkF2ZXJhZ2V9IHZhbHVlPXttZWFzdXJlbWVudC5BdmVyYWdlLnRvU3RyaW5nKCl9IG9uQ2hhbmdlPXsoKSA9PiBzZXRNZWFzdXJlbWVudCh7IC4uLm1lYXN1cmVtZW50LCBBdmVyYWdlOiAhbWVhc3VyZW1lbnQuQXZlcmFnZSB9KX0gLz4gQXZlcmFnZTwvbGFiZWw+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiY29sLWxnLTZcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGlucHV0IHR5cGU9XCJjb2xvclwiIHN0eWxlPXt7IHRleHRBbGlnbjogJ2xlZnQnIH19IGNsYXNzTmFtZT1cImZvcm0tY29udHJvbFwiIHZhbHVlPXttZWFzdXJlbWVudC5BdmdDb2xvcn0gb25DaGFuZ2U9eyhldnQpID0+IHNldE1lYXN1cmVtZW50KHsgLi4ubWVhc3VyZW1lbnQsIEF2Z0NvbG9yOiBldnQudGFyZ2V0LnZhbHVlIH0pfSAvPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJyb3dcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImNvbC1sZy02XCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiY2hlY2tib3hcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxsYWJlbD48aW5wdXQgdHlwZT1cImNoZWNrYm94XCIgY2hlY2tlZD17bWVhc3VyZW1lbnQuTWluaW11bX0gdmFsdWU9e21lYXN1cmVtZW50Lk1pbmltdW0udG9TdHJpbmcoKX0gb25DaGFuZ2U9eygpID0+IHNldE1lYXN1cmVtZW50KHsgLi4ubWVhc3VyZW1lbnQsIE1pbmltdW06ICFtZWFzdXJlbWVudC5NaW5pbXVtIH0pfSAvPiBNaW5pbXVtPC9sYWJlbD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJjb2wtbGctNlwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8aW5wdXQgdHlwZT1cImNvbG9yXCIgc3R5bGU9e3sgdGV4dEFsaWduOiAnbGVmdCcgfX0gY2xhc3NOYW1lPVwiZm9ybS1jb250cm9sXCIgdmFsdWU9e21lYXN1cmVtZW50Lk1pbkNvbG9yfSBvbkNoYW5nZT17KGV2dCkgPT4gc2V0TWVhc3VyZW1lbnQoeyAuLi5tZWFzdXJlbWVudCwgTWluQ29sb3I6IGV2dC50YXJnZXQudmFsdWUgfSl9IC8+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJtb2RhbC1mb290ZXJcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxidXR0b24gdHlwZT1cImJ1dHRvblwiIGNsYXNzTmFtZT1cImJ0biBidG4tcHJpbWFyeVwiIGRhdGEtZGlzbWlzcz1cIm1vZGFsXCIgb25DbGljaz17KCkgPT4gcHJvcHMuQWRkKG1lYXN1cmVtZW50KSB9PkFkZDwvYnV0dG9uPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPGJ1dHRvbiB0eXBlPVwiYnV0dG9uXCIgY2xhc3NOYW1lPVwiYnRuIGJ0bi1kZWZhdWx0XCIgZGF0YS1kaXNtaXNzPVwibW9kYWxcIj5DYW5jZWw8L2J1dHRvbj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgPC9kaXY+XHJcblxyXG4gICAgICAgIDwvPlxyXG4gICAgKTtcclxufVxyXG5cclxuY29uc3QgTWVhc3VyZW1lbnRSb3cgPSAocHJvcHM6IHsgTWVhc3VyZW1lbnQ6IFRyZW5kaW5nY0RhdGFEaXNwbGF5Lk1lYXN1cmVtZW50LCBNZWFzdXJlbWVudHM6IFRyZW5kaW5nY0RhdGFEaXNwbGF5Lk1lYXN1cmVtZW50W10sIEluZGV4OiBudW1iZXIsIFNldE1lYXN1cmVtZW50czogKG1lYXN1cmVtZW50czogVHJlbmRpbmdjRGF0YURpc3BsYXkuTWVhc3VyZW1lbnRbXSkgPT4gdm9pZCwgQXhlczogVHJlbmRpbmdjRGF0YURpc3BsYXkuRmxvdEF4aXNbXX0pID0+IHtcclxuICAgIHJldHVybiAoXHJcbiAgICAgICAgPGxpIGNsYXNzTmFtZT0nbGlzdC1ncm91cC1pdGVtJyBrZXk9e3Byb3BzLk1lYXN1cmVtZW50LklEfT5cclxuICAgICAgICAgICAgPGRpdj57cHJvcHMuTWVhc3VyZW1lbnQuTWV0ZXJOYW1lfTwvZGl2PjxidXR0b24gdHlwZT1cImJ1dHRvblwiIHN0eWxlPXt7IHBvc2l0aW9uOiAncmVsYXRpdmUnLCB0b3A6IC0yMCB9fSBjbGFzc05hbWU9XCJjbG9zZVwiIG9uQ2xpY2s9eygpID0+IHtcclxuICAgICAgICAgICAgICAgIGxldCBtZWFzID0gWy4uLnByb3BzLk1lYXN1cmVtZW50c107XHJcbiAgICAgICAgICAgICAgICBtZWFzLnNwbGljZShwcm9wcy5JbmRleCwgMSk7XHJcbiAgICAgICAgICAgICAgICBwcm9wcy5TZXRNZWFzdXJlbWVudHMobWVhcylcclxuICAgICAgICAgICAgfX0+JnRpbWVzOzwvYnV0dG9uPlxyXG4gICAgICAgICAgICA8ZGl2Pntwcm9wcy5NZWFzdXJlbWVudC5NZWFzdXJlbWVudE5hbWV9PC9kaXY+XHJcbiAgICAgICAgICAgIDxkaXY+XHJcbiAgICAgICAgICAgICAgICA8c2VsZWN0IGNsYXNzTmFtZT0nZm9ybS1jb250cm9sJyB2YWx1ZT17cHJvcHMuTWVhc3VyZW1lbnQuQXhpc30gb25DaGFuZ2U9eyhldnQpID0+IHtcclxuICAgICAgICAgICAgICAgICAgICBsZXQgbWVhcyA9IFsuLi5wcm9wcy5NZWFzdXJlbWVudHNdO1xyXG4gICAgICAgICAgICAgICAgICAgIG1lYXNbcHJvcHMuSW5kZXhdLkF4aXMgPSBwYXJzZUludChldnQudGFyZ2V0LnZhbHVlKTtcclxuICAgICAgICAgICAgICAgICAgICBwcm9wcy5TZXRNZWFzdXJlbWVudHMobWVhcylcclxuICAgICAgICAgICAgICAgIH19PlxyXG4gICAgICAgICAgICAgICAgICAgIHtwcm9wcy5BeGVzLm1hcCgoYSwgaXgpID0+IDxvcHRpb24ga2V5PXtpeH0gdmFsdWU9e2l4ICsgMX0+e2EuYXhpc0xhYmVsfTwvb3B0aW9uPil9XHJcbiAgICAgICAgICAgICAgICA8L3NlbGVjdD5cclxuXHJcbiAgICAgICAgICAgIDwvZGl2PlxyXG5cclxuICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9J3Jvdyc+XHJcbiAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT0nY29sLWxnLTQnPlxyXG4gICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiY2hlY2tib3hcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxsYWJlbD48aW5wdXQgdHlwZT1cImNoZWNrYm94XCIgY2hlY2tlZD17cHJvcHMuTWVhc3VyZW1lbnQuTWF4aW11bX0gdmFsdWU9e3Byb3BzLk1lYXN1cmVtZW50Lk1heGltdW0udG9TdHJpbmcoKX0gb25DaGFuZ2U9eygpID0+IHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBsZXQgbWVhcyA9IFsuLi5wcm9wcy5NZWFzdXJlbWVudHNdO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIG1lYXNbcHJvcHMuSW5kZXhdLk1heGltdW0gPSAhbWVhc1twcm9wcy5JbmRleF0uTWF4aW11bTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBwcm9wcy5TZXRNZWFzdXJlbWVudHMobWVhcylcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIH19IC8+IE1heDwvbGFiZWw+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxpbnB1dCB0eXBlPVwiY29sb3JcIiBjbGFzc05hbWU9XCJmb3JtLWNvbnRyb2xcIiB2YWx1ZT17cHJvcHMuTWVhc3VyZW1lbnQuTWF4Q29sb3J9IG9uQ2hhbmdlPXsoZXZ0KSA9PiB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBsZXQgbWVhcyA9IFsuLi5wcm9wcy5NZWFzdXJlbWVudHNdO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgbWVhc1twcm9wcy5JbmRleF0uTWF4Q29sb3IgPSBldnQudGFyZ2V0LnZhbHVlO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgcHJvcHMuU2V0TWVhc3VyZW1lbnRzKG1lYXMpXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIH19IC8+XHJcbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPSdjb2wtbGctNCc+XHJcbiAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJjaGVja2JveFwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPGxhYmVsPjxpbnB1dCB0eXBlPVwiY2hlY2tib3hcIiBjaGVja2VkPXtwcm9wcy5NZWFzdXJlbWVudC5BdmVyYWdlfSB2YWx1ZT17cHJvcHMuTWVhc3VyZW1lbnQuQXZlcmFnZS50b1N0cmluZygpfSBvbkNoYW5nZT17KCkgPT4ge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGxldCBtZWFzID0gWy4uLnByb3BzLk1lYXN1cmVtZW50c107XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgbWVhc1twcm9wcy5JbmRleF0uQXZlcmFnZSA9ICFtZWFzW3Byb3BzLkluZGV4XS5BdmVyYWdlO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHByb3BzLlNldE1lYXN1cmVtZW50cyhtZWFzKVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgfX0gLz4gQXZnPC9sYWJlbD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPGlucHV0IHR5cGU9XCJjb2xvclwiIGNsYXNzTmFtZT1cImZvcm0tY29udHJvbFwiIHZhbHVlPXtwcm9wcy5NZWFzdXJlbWVudC5BdmdDb2xvcn0gb25DaGFuZ2U9eyhldnQpID0+IHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGxldCBtZWFzID0gWy4uLnByb3BzLk1lYXN1cmVtZW50c107XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBtZWFzW3Byb3BzLkluZGV4XS5BdmdDb2xvciA9IGV2dC50YXJnZXQudmFsdWU7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBwcm9wcy5TZXRNZWFzdXJlbWVudHMobWVhcylcclxuICAgICAgICAgICAgICAgICAgICAgICAgfX0gLz5cclxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9J2NvbC1sZy00Jz5cclxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cIlwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImNoZWNrYm94XCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8bGFiZWw+PGlucHV0IHR5cGU9XCJjaGVja2JveFwiIGNoZWNrZWQ9e3Byb3BzLk1lYXN1cmVtZW50Lk1pbmltdW19IHZhbHVlPXtwcm9wcy5NZWFzdXJlbWVudC5NaW5pbXVtLnRvU3RyaW5nKCl9IG9uQ2hhbmdlPXsoKSA9PiB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgbGV0IG1lYXMgPSBbLi4ucHJvcHMuTWVhc3VyZW1lbnRzXTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBtZWFzW3Byb3BzLkluZGV4XS5NaW5pbXVtID0gIW1lYXNbcHJvcHMuSW5kZXhdLk1pbmltdW07XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcHJvcHMuU2V0TWVhc3VyZW1lbnRzKG1lYXMpXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB9fSAvPiBNaW48L2xhYmVsPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cIlwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8aW5wdXQgdHlwZT1cImNvbG9yXCIgY2xhc3NOYW1lPVwiZm9ybS1jb250cm9sXCIgdmFsdWU9e3Byb3BzLk1lYXN1cmVtZW50Lk1pbkNvbG9yfSBvbkNoYW5nZT17KGV2dCkgPT4ge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgbGV0IG1lYXMgPSBbLi4ucHJvcHMuTWVhc3VyZW1lbnRzXTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIG1lYXNbcHJvcHMuSW5kZXhdLk1pbkNvbG9yID0gZXZ0LnRhcmdldC52YWx1ZTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHByb3BzLlNldE1lYXN1cmVtZW50cyhtZWFzKVxyXG4gICAgICAgICAgICAgICAgICAgICAgICB9fSAvPlxyXG4gICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgPC9kaXY+XHJcblxyXG4gICAgICAgICAgICA8L2Rpdj5cclxuXHJcbiAgICAgICAgPC9saT5cclxuXHJcbiAgICApO1xyXG59XHJcblxyXG5jb25zdCBBZGRBeGlzID0gKHByb3BzOiB7IEFkZDogKGF4aXM6IFRyZW5kaW5nY0RhdGFEaXNwbGF5LkZsb3RBeGlzKSA9PiB2b2lkIH0pID0+IHtcclxuICAgIGNvbnN0IFtheGlzLCBzZXRBeGlzXSA9IFJlYWN0LnVzZVN0YXRlPFRyZW5kaW5nY0RhdGFEaXNwbGF5LkZsb3RBeGlzPih7IHBvc2l0aW9uOiAnbGVmdCcsIGNvbG9yOiAnYmxhY2snLCBheGlzTGFiZWw6ICcnLCBheGlzTGFiZWxVc2VDYW52YXM6IHRydWUsIHNob3c6IHRydWUgfSk7XHJcblxyXG4gICAgcmV0dXJuIChcclxuICAgICAgICA8PlxyXG4gICAgICAgICAgICA8YnV0dG9uIHR5cGU9XCJidXR0b25cIiBzdHlsZT17eyBwb3NpdGlvbjogJ2Fic29sdXRlJywgcmlnaHQ6IDE1IH19ICBjbGFzc05hbWU9XCJidG4gYnRuLWluZm9cIiBkYXRhLXRvZ2dsZT1cIm1vZGFsXCIgZGF0YS10YXJnZXQ9XCIjYXhpc01vZGFsXCIgb25DbGljaz17KCkgPT4ge1xyXG4gICAgICAgICAgICAgICAgc2V0QXhpcyh7IHBvc2l0aW9uOiAnbGVmdCcsIGNvbG9yOiAnYmxhY2snLCBheGlzTGFiZWw6ICcnLCBheGlzTGFiZWxVc2VDYW52YXM6IHRydWUsIHNob3c6IHRydWUgfSk7XHJcbiAgICAgICAgICAgIH19PkFkZDwvYnV0dG9uPlxyXG4gICAgICAgICAgICA8ZGl2IGlkPVwiYXhpc01vZGFsXCIgY2xhc3NOYW1lPVwibW9kYWwgZmFkZVwiIHJvbGU9XCJkaWFsb2dcIj5cclxuICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwibW9kYWwtZGlhbG9nXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJtb2RhbC1jb250ZW50XCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwibW9kYWwtaGVhZGVyXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8YnV0dG9uIHR5cGU9XCJidXR0b25cIiBjbGFzc05hbWU9XCJjbG9zZVwiIGRhdGEtZGlzbWlzcz1cIm1vZGFsXCI+JnRpbWVzOzwvYnV0dG9uPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPGg0IGNsYXNzTmFtZT1cIm1vZGFsLXRpdGxlXCI+QWRkIEF4aXM8L2g0PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJtb2RhbC1ib2R5XCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImZvcm0tZ3JvdXBcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8bGFiZWw+TGFiZWw6IDwvbGFiZWw+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGlucHV0IHR5cGU9XCJ0ZXh0XCIgY2xhc3NOYW1lPVwiZm9ybS1jb250cm9sXCIgdmFsdWU9e2F4aXMuYXhpc0xhYmVsfSBvbkNoYW5nZT17KGV2dCkgPT4gc2V0QXhpcyh7IC4uLmF4aXMsIGF4aXNMYWJlbDogZXZ0LnRhcmdldC52YWx1ZSB9KX0gLz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiZm9ybS1ncm91cFwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxsYWJlbD5Qb3NpdGlvbjogPC9sYWJlbD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8c2VsZWN0IGNsYXNzTmFtZT0nZm9ybS1jb250cm9sJyB2YWx1ZT17YXhpcy5wb3NpdGlvbn0gb25DaGFuZ2U9eyhldnQpID0+IHNldEF4aXMoeyAuLi5heGlzLCBwb3NpdGlvbjogZXZ0LnRhcmdldC52YWx1ZSBhcyAnbGVmdCcgfCAncmlnaHQnfSl9PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8b3B0aW9uIHZhbHVlPSdsZWZ0Jz5sZWZ0PC9vcHRpb24+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxvcHRpb24gdmFsdWU9J3JpZ2h0Jz5yaWdodDwvb3B0aW9uPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvc2VsZWN0PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJtb2RhbC1mb290ZXJcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxidXR0b24gdHlwZT1cImJ1dHRvblwiIGNsYXNzTmFtZT1cImJ0biBidG4tcHJpbWFyeVwiIGRhdGEtZGlzbWlzcz1cIm1vZGFsXCIgb25DbGljaz17KCkgPT4gcHJvcHMuQWRkKGF4aXMpfT5BZGQ8L2J1dHRvbj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxidXR0b24gdHlwZT1cImJ1dHRvblwiIGNsYXNzTmFtZT1cImJ0biBidG4tZGVmYXVsdFwiIGRhdGEtZGlzbWlzcz1cIm1vZGFsXCI+Q2FuY2VsPC9idXR0b24+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgIDwvZGl2PlxyXG5cclxuICAgICAgICA8Lz5cclxuICAgICk7XHJcbn1cclxuXHJcbmNvbnN0IEF4aXNSb3cgPSAocHJvcHM6IHsgSW5kZXg6IG51bWJlciwgQXhlczogVHJlbmRpbmdjRGF0YURpc3BsYXkuRmxvdEF4aXNbXSwgU2V0QXhlczogKCBheGVzOiBUcmVuZGluZ2NEYXRhRGlzcGxheS5GbG90QXhpc1tdKSA9PiB2b2lkfSkgPT4ge1xyXG4gICAgcmV0dXJuIChcclxuICAgICAgICA8bGkgY2xhc3NOYW1lPSdsaXN0LWdyb3VwLWl0ZW0nIGtleT17cHJvcHMuSW5kZXh9PlxyXG4gICAgICAgICAgICA8ZGl2Pntwcm9wcy5BeGVzW3Byb3BzLkluZGV4XS5heGlzTGFiZWx9PC9kaXY+PGJ1dHRvbiB0eXBlPVwiYnV0dG9uXCIgc3R5bGU9e3sgcG9zaXRpb246ICdyZWxhdGl2ZScsIHRvcDogLTIwIH19IGNsYXNzTmFtZT1cImNsb3NlXCIgb25DbGljaz17KCkgPT4ge1xyXG4gICAgICAgICAgICAgICAgbGV0IGEgPSBbLi4ucHJvcHMuQXhlc107XHJcbiAgICAgICAgICAgICAgICBhLnNwbGljZShwcm9wcy5JbmRleCwgMSk7XHJcbiAgICAgICAgICAgICAgICBwcm9wcy5TZXRBeGVzKGEpXHJcbiAgICAgICAgICAgIH19PiZ0aW1lczs8L2J1dHRvbj5cclxuICAgICAgICAgICAgPGRpdj5cclxuICAgICAgICAgICAgICAgIDxzZWxlY3QgY2xhc3NOYW1lPSdmb3JtLWNvbnRyb2wnIHZhbHVlPXtwcm9wcy5BeGVzW3Byb3BzLkluZGV4XS5wb3NpdGlvbn0gb25DaGFuZ2U9eyhldnQpID0+IHtcclxuICAgICAgICAgICAgICAgICAgICBsZXQgYSA9IFsuLi5wcm9wcy5BeGVzXTtcclxuICAgICAgICAgICAgICAgICAgICBhW3Byb3BzLkluZGV4XS5wb3NpdGlvbiA9IGV2dC50YXJnZXQudmFsdWUgYXMgJ2xlZnQnIHwgJ3JpZ2h0JztcclxuICAgICAgICAgICAgICAgICAgICBwcm9wcy5TZXRBeGVzKGEpXHJcbiAgICAgICAgICAgICAgICB9fT5cclxuICAgICAgICAgICAgICAgICAgICA8b3B0aW9uIHZhbHVlPSdsZWZ0Jz5sZWZ0PC9vcHRpb24+XHJcbiAgICAgICAgICAgICAgICAgICAgPG9wdGlvbiB2YWx1ZT0ncmlnaHQnPnJpZ2h0PC9vcHRpb24+XHJcbiAgICAgICAgICAgICAgICA8L3NlbGVjdD5cclxuICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPSdyb3cnPlxyXG4gICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9J2NvbC1sZy02Jz5cclxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT0nZm9ybS1ncm91cCc+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxsYWJlbD5NaW48L2xhYmVsPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8aW5wdXQgY2xhc3NOYW1lPSdmb3JtLWNvbnRyb2wnIHR5cGU9XCJudW1iZXJcIiB2YWx1ZT17cHJvcHMuQXhlc1twcm9wcy5JbmRleF0/Lm1pbiA/PyAnJ30gb25DaGFuZ2U9eyhldnQpID0+IHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGxldCBheGVzID0gWy4uLnByb3BzLkF4ZXNdO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgaWYgKGV2dC50YXJnZXQudmFsdWUgPT0gJycpXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgZGVsZXRlIGF4ZXNbcHJvcHMuSW5kZXhdLm1pbjtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGVsc2VcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBheGVzW3Byb3BzLkluZGV4XS5taW4gPSBwYXJzZUZsb2F0KGV2dC50YXJnZXQudmFsdWUpO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgcHJvcHMuU2V0QXhlcyhheGVzKVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgfX0gLz4gXHJcbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPSdjb2wtbGctNic+XHJcbiAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9J2Zvcm0tZ3JvdXAnPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8bGFiZWw+TWF4PC9sYWJlbD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPGlucHV0IGNsYXNzTmFtZT0nZm9ybS1jb250cm9sJyB0eXBlPVwibnVtYmVyXCIgdmFsdWU9e3Byb3BzLkF4ZXNbcHJvcHMuSW5kZXhdPy5tYXggPz8gJyd9IG9uQ2hhbmdlPXsoZXZ0KSA9PiB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBsZXQgYXhlcyA9IFsuLi5wcm9wcy5BeGVzXTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGlmIChldnQudGFyZ2V0LnZhbHVlID09ICcnKVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGRlbGV0ZSBheGVzW3Byb3BzLkluZGV4XS5tYXg7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBlbHNlXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgYXhlc1twcm9wcy5JbmRleF0ubWF4ID0gcGFyc2VGbG9hdChldnQudGFyZ2V0LnZhbHVlKTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHByb3BzLlNldEF4ZXMoYXhlcylcclxuICAgICAgICAgICAgICAgICAgICAgICAgfX0gLz5cclxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgIDwvZGl2PlxyXG5cclxuICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgIDxidXR0b24gY2xhc3NOYW1lPSdidG4gYnRuLWluZm8nIG9uQ2xpY2s9eygpID0+IHtcclxuICAgICAgICAgICAgICAgIGxldCBheGVzID0gWy4uLnByb3BzLkF4ZXNdO1xyXG4gICAgICAgICAgICAgICAgZGVsZXRlIGF4ZXNbcHJvcHMuSW5kZXhdLm1heDtcclxuICAgICAgICAgICAgICAgIGRlbGV0ZSBheGVzW3Byb3BzLkluZGV4XS5taW47XHJcblxyXG4gICAgICAgICAgICAgICAgcHJvcHMuU2V0QXhlcyhheGVzKVxyXG5cclxuICAgICAgICAgICAgfX0gPlVzZSBEZWZhdWx0PC9idXR0b24+XHJcblxyXG4gICAgICAgIDwvbGk+XHJcblxyXG4pO1xyXG59XHJcblxyXG5SZWFjdERPTS5yZW5kZXIoPFRyZW5kaW5nRGF0YURpc3BsYXkgLz4sIGRvY3VtZW50LmdldEVsZW1lbnRCeUlkKCdib2R5Q29udGFpbmVyJykpOyIsIi8qXHJcbkF4aXMgTGFiZWxzIFBsdWdpbiBmb3IgZmxvdC5cclxuaHR0cDovL2dpdGh1Yi5jb20vbWFya3Jjb3RlL2Zsb3QtYXhpc2xhYmVsc1xyXG5PcmlnaW5hbCBjb2RlIGlzIENvcHlyaWdodCAoYykgMjAxMCBYdWFuIEx1by5cclxuT3JpZ2luYWwgY29kZSB3YXMgcmVsZWFzZWQgdW5kZXIgdGhlIEdQTHYzIGxpY2Vuc2UgYnkgWHVhbiBMdW8sIFNlcHRlbWJlciAyMDEwLlxyXG5PcmlnaW5hbCBjb2RlIHdhcyByZXJlbGVhc2VkIHVuZGVyIHRoZSBNSVQgbGljZW5zZSBieSBYdWFuIEx1bywgQXByaWwgMjAxMi5cclxuUGVybWlzc2lvbiBpcyBoZXJlYnkgZ3JhbnRlZCwgZnJlZSBvZiBjaGFyZ2UsIHRvIGFueSBwZXJzb24gb2J0YWluaW5nXHJcbmEgY29weSBvZiB0aGlzIHNvZnR3YXJlIGFuZCBhc3NvY2lhdGVkIGRvY3VtZW50YXRpb24gZmlsZXMgKHRoZVxyXG5cIlNvZnR3YXJlXCIpLCB0byBkZWFsIGluIHRoZSBTb2Z0d2FyZSB3aXRob3V0IHJlc3RyaWN0aW9uLCBpbmNsdWRpbmdcclxud2l0aG91dCBsaW1pdGF0aW9uIHRoZSByaWdodHMgdG8gdXNlLCBjb3B5LCBtb2RpZnksIG1lcmdlLCBwdWJsaXNoLFxyXG5kaXN0cmlidXRlLCBzdWJsaWNlbnNlLCBhbmQvb3Igc2VsbCBjb3BpZXMgb2YgdGhlIFNvZnR3YXJlLCBhbmQgdG9cclxucGVybWl0IHBlcnNvbnMgdG8gd2hvbSB0aGUgU29mdHdhcmUgaXMgZnVybmlzaGVkIHRvIGRvIHNvLCBzdWJqZWN0IHRvXHJcbnRoZSBmb2xsb3dpbmcgY29uZGl0aW9uczpcclxuVGhlIGFib3ZlIGNvcHlyaWdodCBub3RpY2UgYW5kIHRoaXMgcGVybWlzc2lvbiBub3RpY2Ugc2hhbGwgYmVcclxuaW5jbHVkZWQgaW4gYWxsIGNvcGllcyBvciBzdWJzdGFudGlhbCBwb3J0aW9ucyBvZiB0aGUgU29mdHdhcmUuXHJcblRIRSBTT0ZUV0FSRSBJUyBQUk9WSURFRCBcIkFTIElTXCIsIFdJVEhPVVQgV0FSUkFOVFkgT0YgQU5ZIEtJTkQsXHJcbkVYUFJFU1MgT1IgSU1QTElFRCwgSU5DTFVESU5HIEJVVCBOT1QgTElNSVRFRCBUTyBUSEUgV0FSUkFOVElFUyBPRlxyXG5NRVJDSEFOVEFCSUxJVFksIEZJVE5FU1MgRk9SIEEgUEFSVElDVUxBUiBQVVJQT1NFIEFORFxyXG5OT05JTkZSSU5HRU1FTlQuIElOIE5PIEVWRU5UIFNIQUxMIFRIRSBBVVRIT1JTIE9SIENPUFlSSUdIVCBIT0xERVJTIEJFXHJcbkxJQUJMRSBGT1IgQU5ZIENMQUlNLCBEQU1BR0VTIE9SIE9USEVSIExJQUJJTElUWSwgV0hFVEhFUiBJTiBBTiBBQ1RJT05cclxuT0YgQ09OVFJBQ1QsIFRPUlQgT1IgT1RIRVJXSVNFLCBBUklTSU5HIEZST00sIE9VVCBPRiBPUiBJTiBDT05ORUNUSU9OXHJcbldJVEggVEhFIFNPRlRXQVJFIE9SIFRIRSBVU0UgT1IgT1RIRVIgREVBTElOR1MgSU4gVEhFIFNPRlRXQVJFLlxyXG4gKi9cclxuXHJcbihmdW5jdGlvbiAoJCkge1xyXG4gICAgdmFyIG9wdGlvbnMgPSB7XHJcbiAgICAgIGF4aXNMYWJlbHM6IHtcclxuICAgICAgICBzaG93OiB0cnVlXHJcbiAgICAgIH1cclxuICAgIH07XHJcblxyXG4gICAgZnVuY3Rpb24gY2FudmFzU3VwcG9ydGVkKCkge1xyXG4gICAgICAgIHJldHVybiAhIWRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoJ2NhbnZhcycpLmdldENvbnRleHQ7XHJcbiAgICB9XHJcblxyXG4gICAgZnVuY3Rpb24gY2FudmFzVGV4dFN1cHBvcnRlZCgpIHtcclxuICAgICAgICBpZiAoIWNhbnZhc1N1cHBvcnRlZCgpKSB7XHJcbiAgICAgICAgICAgIHJldHVybiBmYWxzZTtcclxuICAgICAgICB9XHJcbiAgICAgICAgdmFyIGR1bW15X2NhbnZhcyA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoJ2NhbnZhcycpO1xyXG4gICAgICAgIHZhciBjb250ZXh0ID0gZHVtbXlfY2FudmFzLmdldENvbnRleHQoJzJkJyk7XHJcbiAgICAgICAgcmV0dXJuIHR5cGVvZiBjb250ZXh0LmZpbGxUZXh0ID09ICdmdW5jdGlvbic7XHJcbiAgICB9XHJcblxyXG4gICAgZnVuY3Rpb24gY3NzM1RyYW5zaXRpb25TdXBwb3J0ZWQoKSB7XHJcbiAgICAgICAgdmFyIGRpdiA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoJ2RpdicpO1xyXG4gICAgICAgIHJldHVybiB0eXBlb2YgZGl2LnN0eWxlLk1velRyYW5zaXRpb24gIT0gJ3VuZGVmaW5lZCcgICAgLy8gR2Vja29cclxuICAgICAgICAgICAgfHwgdHlwZW9mIGRpdi5zdHlsZS5PVHJhbnNpdGlvbiAhPSAndW5kZWZpbmVkJyAgICAgIC8vIE9wZXJhXHJcbiAgICAgICAgICAgIHx8IHR5cGVvZiBkaXYuc3R5bGUud2Via2l0VHJhbnNpdGlvbiAhPSAndW5kZWZpbmVkJyAvLyBXZWJLaXRcclxuICAgICAgICAgICAgfHwgdHlwZW9mIGRpdi5zdHlsZS50cmFuc2l0aW9uICE9ICd1bmRlZmluZWQnO1xyXG4gICAgfVxyXG5cclxuXHJcbiAgICBmdW5jdGlvbiBBeGlzTGFiZWwoYXhpc05hbWUsIHBvc2l0aW9uLCBwYWRkaW5nLCBwbG90LCBvcHRzKSB7XHJcbiAgICAgICAgdGhpcy5heGlzTmFtZSA9IGF4aXNOYW1lO1xyXG4gICAgICAgIHRoaXMucG9zaXRpb24gPSBwb3NpdGlvbjtcclxuICAgICAgICB0aGlzLnBhZGRpbmcgPSBwYWRkaW5nO1xyXG4gICAgICAgIHRoaXMucGxvdCA9IHBsb3Q7XHJcbiAgICAgICAgdGhpcy5vcHRzID0gb3B0cztcclxuICAgICAgICB0aGlzLndpZHRoID0gMDtcclxuICAgICAgICB0aGlzLmhlaWdodCA9IDA7XHJcbiAgICB9XHJcblxyXG4gICAgQXhpc0xhYmVsLnByb3RvdHlwZS5jbGVhbnVwID0gZnVuY3Rpb24oKSB7XHJcbiAgICB9O1xyXG5cclxuXHJcbiAgICBDYW52YXNBeGlzTGFiZWwucHJvdG90eXBlID0gbmV3IEF4aXNMYWJlbCgpO1xyXG4gICAgQ2FudmFzQXhpc0xhYmVsLnByb3RvdHlwZS5jb25zdHJ1Y3RvciA9IENhbnZhc0F4aXNMYWJlbDtcclxuICAgIGZ1bmN0aW9uIENhbnZhc0F4aXNMYWJlbChheGlzTmFtZSwgcG9zaXRpb24sIHBhZGRpbmcsIHBsb3QsIG9wdHMpIHtcclxuICAgICAgICBBeGlzTGFiZWwucHJvdG90eXBlLmNvbnN0cnVjdG9yLmNhbGwodGhpcywgYXhpc05hbWUsIHBvc2l0aW9uLCBwYWRkaW5nLFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBwbG90LCBvcHRzKTtcclxuICAgIH1cclxuXHJcbiAgICBDYW52YXNBeGlzTGFiZWwucHJvdG90eXBlLmNhbGN1bGF0ZVNpemUgPSBmdW5jdGlvbigpIHtcclxuICAgICAgICBpZiAoIXRoaXMub3B0cy5heGlzTGFiZWxGb250U2l6ZVBpeGVscylcclxuICAgICAgICAgICAgdGhpcy5vcHRzLmF4aXNMYWJlbEZvbnRTaXplUGl4ZWxzID0gMTQ7XHJcbiAgICAgICAgaWYgKCF0aGlzLm9wdHMuYXhpc0xhYmVsRm9udEZhbWlseSlcclxuICAgICAgICAgICAgdGhpcy5vcHRzLmF4aXNMYWJlbEZvbnRGYW1pbHkgPSAnc2Fucy1zZXJpZic7XHJcblxyXG4gICAgICAgIHZhciB0ZXh0V2lkdGggPSB0aGlzLm9wdHMuYXhpc0xhYmVsRm9udFNpemVQaXhlbHMgKyB0aGlzLnBhZGRpbmc7XHJcbiAgICAgICAgdmFyIHRleHRIZWlnaHQgPSB0aGlzLm9wdHMuYXhpc0xhYmVsRm9udFNpemVQaXhlbHMgKyB0aGlzLnBhZGRpbmc7XHJcbiAgICAgICAgaWYgKHRoaXMucG9zaXRpb24gPT0gJ2xlZnQnIHx8IHRoaXMucG9zaXRpb24gPT0gJ3JpZ2h0Jykge1xyXG4gICAgICAgICAgICB0aGlzLndpZHRoID0gdGhpcy5vcHRzLmF4aXNMYWJlbEZvbnRTaXplUGl4ZWxzICsgdGhpcy5wYWRkaW5nO1xyXG4gICAgICAgICAgICB0aGlzLmhlaWdodCA9IDA7XHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgdGhpcy53aWR0aCA9IDA7XHJcbiAgICAgICAgICAgIHRoaXMuaGVpZ2h0ID0gdGhpcy5vcHRzLmF4aXNMYWJlbEZvbnRTaXplUGl4ZWxzICsgdGhpcy5wYWRkaW5nO1xyXG4gICAgICAgIH1cclxuICAgIH07XHJcblxyXG4gICAgQ2FudmFzQXhpc0xhYmVsLnByb3RvdHlwZS5kcmF3ID0gZnVuY3Rpb24oYm94KSB7XHJcbiAgICAgICAgaWYgKCF0aGlzLm9wdHMuYXhpc0xhYmVsQ29sb3VyKVxyXG4gICAgICAgICAgICB0aGlzLm9wdHMuYXhpc0xhYmVsQ29sb3VyID0gJ2JsYWNrJztcclxuICAgICAgICB2YXIgY3R4ID0gdGhpcy5wbG90LmdldENhbnZhcygpLmdldENvbnRleHQoJzJkJyk7XHJcbiAgICAgICAgY3R4LnNhdmUoKTtcclxuICAgICAgICBjdHguZm9udCA9IHRoaXMub3B0cy5heGlzTGFiZWxGb250U2l6ZVBpeGVscyArICdweCAnICtcclxuICAgICAgICAgICAgdGhpcy5vcHRzLmF4aXNMYWJlbEZvbnRGYW1pbHk7XHJcbiAgICAgICAgY3R4LmZpbGxTdHlsZSA9IHRoaXMub3B0cy5heGlzTGFiZWxDb2xvdXI7XHJcbiAgICAgICAgdmFyIHdpZHRoID0gY3R4Lm1lYXN1cmVUZXh0KHRoaXMub3B0cy5heGlzTGFiZWwpLndpZHRoO1xyXG4gICAgICAgIHZhciBoZWlnaHQgPSB0aGlzLm9wdHMuYXhpc0xhYmVsRm9udFNpemVQaXhlbHM7XHJcbiAgICAgICAgdmFyIHgsIHksIGFuZ2xlID0gMDtcclxuICAgICAgICBpZiAodGhpcy5wb3NpdGlvbiA9PSAndG9wJykge1xyXG4gICAgICAgICAgICB4ID0gYm94LmxlZnQgKyBib3gud2lkdGgvMiAtIHdpZHRoLzI7XHJcbiAgICAgICAgICAgIHkgPSBib3gudG9wICsgaGVpZ2h0KjAuNzI7XHJcbiAgICAgICAgfSBlbHNlIGlmICh0aGlzLnBvc2l0aW9uID09ICdib3R0b20nKSB7XHJcbiAgICAgICAgICAgIHggPSBib3gubGVmdCArIGJveC53aWR0aC8yIC0gd2lkdGgvMjtcclxuICAgICAgICAgICAgeSA9IGJveC50b3AgKyBib3guaGVpZ2h0IC0gaGVpZ2h0KjAuNzI7XHJcbiAgICAgICAgfSBlbHNlIGlmICh0aGlzLnBvc2l0aW9uID09ICdsZWZ0Jykge1xyXG4gICAgICAgICAgICB4ID0gYm94LmxlZnQgKyBoZWlnaHQqMC43MjtcclxuICAgICAgICAgICAgeSA9IGJveC5oZWlnaHQvMiArIGJveC50b3AgKyB3aWR0aC8yO1xyXG4gICAgICAgICAgICBhbmdsZSA9IC1NYXRoLlBJLzI7XHJcbiAgICAgICAgfSBlbHNlIGlmICh0aGlzLnBvc2l0aW9uID09ICdyaWdodCcpIHtcclxuICAgICAgICAgICAgeCA9IGJveC5sZWZ0ICsgYm94LndpZHRoIC0gaGVpZ2h0KjAuNzI7XHJcbiAgICAgICAgICAgIHkgPSBib3guaGVpZ2h0LzIgKyBib3gudG9wIC0gd2lkdGgvMjtcclxuICAgICAgICAgICAgYW5nbGUgPSBNYXRoLlBJLzI7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIGN0eC50cmFuc2xhdGUoeCwgeSk7XHJcbiAgICAgICAgY3R4LnJvdGF0ZShhbmdsZSk7XHJcbiAgICAgICAgY3R4LmZpbGxUZXh0KHRoaXMub3B0cy5heGlzTGFiZWwsIDAsIDApO1xyXG4gICAgICAgIGN0eC5yZXN0b3JlKCk7XHJcbiAgICB9O1xyXG5cclxuXHJcbiAgICBIdG1sQXhpc0xhYmVsLnByb3RvdHlwZSA9IG5ldyBBeGlzTGFiZWwoKTtcclxuICAgIEh0bWxBeGlzTGFiZWwucHJvdG90eXBlLmNvbnN0cnVjdG9yID0gSHRtbEF4aXNMYWJlbDtcclxuICAgIGZ1bmN0aW9uIEh0bWxBeGlzTGFiZWwoYXhpc05hbWUsIHBvc2l0aW9uLCBwYWRkaW5nLCBwbG90LCBvcHRzKSB7XHJcbiAgICAgICAgQXhpc0xhYmVsLnByb3RvdHlwZS5jb25zdHJ1Y3Rvci5jYWxsKHRoaXMsIGF4aXNOYW1lLCBwb3NpdGlvbixcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcGFkZGluZywgcGxvdCwgb3B0cyk7XHJcbiAgICAgICAgdGhpcy5lbGVtID0gbnVsbDtcclxuICAgIH1cclxuXHJcbiAgICBIdG1sQXhpc0xhYmVsLnByb3RvdHlwZS5jYWxjdWxhdGVTaXplID0gZnVuY3Rpb24oKSB7XHJcbiAgICAgICAgdmFyIGVsZW0gPSAkKCc8ZGl2IGNsYXNzPVwiYXhpc0xhYmVsc1wiIHN0eWxlPVwicG9zaXRpb246YWJzb2x1dGU7XCI+JyArXHJcbiAgICAgICAgICAgICAgICAgICAgIHRoaXMub3B0cy5heGlzTGFiZWwgKyAnPC9kaXY+Jyk7XHJcbiAgICAgICAgdGhpcy5wbG90LmdldFBsYWNlaG9sZGVyKCkuYXBwZW5kKGVsZW0pO1xyXG4gICAgICAgIC8vIHN0b3JlIGhlaWdodCBhbmQgd2lkdGggb2YgbGFiZWwgaXRzZWxmLCBmb3IgdXNlIGluIGRyYXcoKVxyXG4gICAgICAgIHRoaXMubGFiZWxXaWR0aCA9IGVsZW0ub3V0ZXJXaWR0aCh0cnVlKTtcclxuICAgICAgICB0aGlzLmxhYmVsSGVpZ2h0ID0gZWxlbS5vdXRlckhlaWdodCh0cnVlKTtcclxuICAgICAgICBlbGVtLnJlbW92ZSgpO1xyXG5cclxuICAgICAgICB0aGlzLndpZHRoID0gdGhpcy5oZWlnaHQgPSAwO1xyXG4gICAgICAgIGlmICh0aGlzLnBvc2l0aW9uID09ICdsZWZ0JyB8fCB0aGlzLnBvc2l0aW9uID09ICdyaWdodCcpIHtcclxuICAgICAgICAgICAgdGhpcy53aWR0aCA9IHRoaXMubGFiZWxXaWR0aCArIHRoaXMucGFkZGluZztcclxuICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICB0aGlzLmhlaWdodCA9IHRoaXMubGFiZWxIZWlnaHQgKyB0aGlzLnBhZGRpbmc7XHJcbiAgICAgICAgfVxyXG4gICAgfTtcclxuXHJcbiAgICBIdG1sQXhpc0xhYmVsLnByb3RvdHlwZS5jbGVhbnVwID0gZnVuY3Rpb24oKSB7XHJcbiAgICAgICAgaWYgKHRoaXMuZWxlbSkge1xyXG4gICAgICAgICAgICB0aGlzLmVsZW0ucmVtb3ZlKCk7XHJcbiAgICAgICAgfVxyXG4gICAgfTtcclxuXHJcbiAgICBIdG1sQXhpc0xhYmVsLnByb3RvdHlwZS5kcmF3ID0gZnVuY3Rpb24oYm94KSB7XHJcbiAgICAgICAgdGhpcy5wbG90LmdldFBsYWNlaG9sZGVyKCkuZmluZCgnIycgKyB0aGlzLmF4aXNOYW1lICsgJ0xhYmVsJykucmVtb3ZlKCk7XHJcbiAgICAgICAgdGhpcy5lbGVtID0gJCgnPGRpdiBpZD1cIicgKyB0aGlzLmF4aXNOYW1lICtcclxuICAgICAgICAgICAgICAgICAgICAgICdMYWJlbFwiIFwiIGNsYXNzPVwiYXhpc0xhYmVsc1wiIHN0eWxlPVwicG9zaXRpb246YWJzb2x1dGU7XCI+J1xyXG4gICAgICAgICAgICAgICAgICAgICAgKyB0aGlzLm9wdHMuYXhpc0xhYmVsICsgJzwvZGl2PicpO1xyXG4gICAgICAgIHRoaXMucGxvdC5nZXRQbGFjZWhvbGRlcigpLmFwcGVuZCh0aGlzLmVsZW0pO1xyXG4gICAgICAgIGlmICh0aGlzLnBvc2l0aW9uID09ICd0b3AnKSB7XHJcbiAgICAgICAgICAgIHRoaXMuZWxlbS5jc3MoJ2xlZnQnLCBib3gubGVmdCArIGJveC53aWR0aC8yIC0gdGhpcy5sYWJlbFdpZHRoLzIgK1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICdweCcpO1xyXG4gICAgICAgICAgICB0aGlzLmVsZW0uY3NzKCd0b3AnLCBib3gudG9wICsgJ3B4Jyk7XHJcbiAgICAgICAgfSBlbHNlIGlmICh0aGlzLnBvc2l0aW9uID09ICdib3R0b20nKSB7XHJcbiAgICAgICAgICAgIHRoaXMuZWxlbS5jc3MoJ2xlZnQnLCBib3gubGVmdCArIGJveC53aWR0aC8yIC0gdGhpcy5sYWJlbFdpZHRoLzIgK1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICdweCcpO1xyXG4gICAgICAgICAgICB0aGlzLmVsZW0uY3NzKCd0b3AnLCBib3gudG9wICsgYm94LmhlaWdodCAtIHRoaXMubGFiZWxIZWlnaHQgK1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICdweCcpO1xyXG4gICAgICAgIH0gZWxzZSBpZiAodGhpcy5wb3NpdGlvbiA9PSAnbGVmdCcpIHtcclxuICAgICAgICAgICAgdGhpcy5lbGVtLmNzcygndG9wJywgYm94LnRvcCArIGJveC5oZWlnaHQvMiAtIHRoaXMubGFiZWxIZWlnaHQvMiArXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgJ3B4Jyk7XHJcbiAgICAgICAgICAgIHRoaXMuZWxlbS5jc3MoJ2xlZnQnLCBib3gubGVmdCArICdweCcpO1xyXG4gICAgICAgIH0gZWxzZSBpZiAodGhpcy5wb3NpdGlvbiA9PSAncmlnaHQnKSB7XHJcbiAgICAgICAgICAgIHRoaXMuZWxlbS5jc3MoJ3RvcCcsIGJveC50b3AgKyBib3guaGVpZ2h0LzIgLSB0aGlzLmxhYmVsSGVpZ2h0LzIgK1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICdweCcpO1xyXG4gICAgICAgICAgICB0aGlzLmVsZW0uY3NzKCdsZWZ0JywgYm94LmxlZnQgKyBib3gud2lkdGggLSB0aGlzLmxhYmVsV2lkdGggK1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICdweCcpO1xyXG4gICAgICAgIH1cclxuICAgIH07XHJcblxyXG5cclxuICAgIENzc1RyYW5zZm9ybUF4aXNMYWJlbC5wcm90b3R5cGUgPSBuZXcgSHRtbEF4aXNMYWJlbCgpO1xyXG4gICAgQ3NzVHJhbnNmb3JtQXhpc0xhYmVsLnByb3RvdHlwZS5jb25zdHJ1Y3RvciA9IENzc1RyYW5zZm9ybUF4aXNMYWJlbDtcclxuICAgIGZ1bmN0aW9uIENzc1RyYW5zZm9ybUF4aXNMYWJlbChheGlzTmFtZSwgcG9zaXRpb24sIHBhZGRpbmcsIHBsb3QsIG9wdHMpIHtcclxuICAgICAgICBIdG1sQXhpc0xhYmVsLnByb3RvdHlwZS5jb25zdHJ1Y3Rvci5jYWxsKHRoaXMsIGF4aXNOYW1lLCBwb3NpdGlvbixcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHBhZGRpbmcsIHBsb3QsIG9wdHMpO1xyXG4gICAgfVxyXG5cclxuICAgIENzc1RyYW5zZm9ybUF4aXNMYWJlbC5wcm90b3R5cGUuY2FsY3VsYXRlU2l6ZSA9IGZ1bmN0aW9uKCkge1xyXG4gICAgICAgIEh0bWxBeGlzTGFiZWwucHJvdG90eXBlLmNhbGN1bGF0ZVNpemUuY2FsbCh0aGlzKTtcclxuICAgICAgICB0aGlzLndpZHRoID0gdGhpcy5oZWlnaHQgPSAwO1xyXG4gICAgICAgIGlmICh0aGlzLnBvc2l0aW9uID09ICdsZWZ0JyB8fCB0aGlzLnBvc2l0aW9uID09ICdyaWdodCcpIHtcclxuICAgICAgICAgICAgdGhpcy53aWR0aCA9IHRoaXMubGFiZWxIZWlnaHQgKyB0aGlzLnBhZGRpbmc7XHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgdGhpcy5oZWlnaHQgPSB0aGlzLmxhYmVsSGVpZ2h0ICsgdGhpcy5wYWRkaW5nO1xyXG4gICAgICAgIH1cclxuICAgIH07XHJcblxyXG4gICAgQ3NzVHJhbnNmb3JtQXhpc0xhYmVsLnByb3RvdHlwZS50cmFuc2Zvcm1zID0gZnVuY3Rpb24oZGVncmVlcywgeCwgeSkge1xyXG4gICAgICAgIHZhciBzdHJhbnNmb3JtcyA9IHtcclxuICAgICAgICAgICAgJy1tb3otdHJhbnNmb3JtJzogJycsXHJcbiAgICAgICAgICAgICctd2Via2l0LXRyYW5zZm9ybSc6ICcnLFxyXG4gICAgICAgICAgICAnLW8tdHJhbnNmb3JtJzogJycsXHJcbiAgICAgICAgICAgICctbXMtdHJhbnNmb3JtJzogJydcclxuICAgICAgICB9O1xyXG4gICAgICAgIGlmICh4ICE9IDAgfHwgeSAhPSAwKSB7XHJcbiAgICAgICAgICAgIHZhciBzdGRUcmFuc2xhdGUgPSAnIHRyYW5zbGF0ZSgnICsgeCArICdweCwgJyArIHkgKyAncHgpJztcclxuICAgICAgICAgICAgc3RyYW5zZm9ybXNbJy1tb3otdHJhbnNmb3JtJ10gKz0gc3RkVHJhbnNsYXRlO1xyXG4gICAgICAgICAgICBzdHJhbnNmb3Jtc1snLXdlYmtpdC10cmFuc2Zvcm0nXSArPSBzdGRUcmFuc2xhdGU7XHJcbiAgICAgICAgICAgIHN0cmFuc2Zvcm1zWyctby10cmFuc2Zvcm0nXSArPSBzdGRUcmFuc2xhdGU7XHJcbiAgICAgICAgICAgIHN0cmFuc2Zvcm1zWyctbXMtdHJhbnNmb3JtJ10gKz0gc3RkVHJhbnNsYXRlO1xyXG4gICAgICAgIH1cclxuICAgICAgICBpZiAoZGVncmVlcyAhPSAwKSB7XHJcbiAgICAgICAgICAgIHZhciByb3RhdGlvbiA9IGRlZ3JlZXMgLyA5MDtcclxuICAgICAgICAgICAgdmFyIHN0ZFJvdGF0ZSA9ICcgcm90YXRlKCcgKyBkZWdyZWVzICsgJ2RlZyknO1xyXG4gICAgICAgICAgICBzdHJhbnNmb3Jtc1snLW1vei10cmFuc2Zvcm0nXSArPSBzdGRSb3RhdGU7XHJcbiAgICAgICAgICAgIHN0cmFuc2Zvcm1zWyctd2Via2l0LXRyYW5zZm9ybSddICs9IHN0ZFJvdGF0ZTtcclxuICAgICAgICAgICAgc3RyYW5zZm9ybXNbJy1vLXRyYW5zZm9ybSddICs9IHN0ZFJvdGF0ZTtcclxuICAgICAgICAgICAgc3RyYW5zZm9ybXNbJy1tcy10cmFuc2Zvcm0nXSArPSBzdGRSb3RhdGU7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIHZhciBzID0gJ3RvcDogMDsgbGVmdDogMDsgJztcclxuICAgICAgICBmb3IgKHZhciBwcm9wIGluIHN0cmFuc2Zvcm1zKSB7XHJcbiAgICAgICAgICAgIGlmIChzdHJhbnNmb3Jtc1twcm9wXSkge1xyXG4gICAgICAgICAgICAgICAgcyArPSBwcm9wICsgJzonICsgc3RyYW5zZm9ybXNbcHJvcF0gKyAnOyc7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICB9XHJcbiAgICAgICAgcyArPSAnOyc7XHJcbiAgICAgICAgcmV0dXJuIHM7XHJcbiAgICB9O1xyXG5cclxuICAgIENzc1RyYW5zZm9ybUF4aXNMYWJlbC5wcm90b3R5cGUuY2FsY3VsYXRlT2Zmc2V0cyA9IGZ1bmN0aW9uKGJveCkge1xyXG4gICAgICAgIHZhciBvZmZzZXRzID0geyB4OiAwLCB5OiAwLCBkZWdyZWVzOiAwIH07XHJcbiAgICAgICAgaWYgKHRoaXMucG9zaXRpb24gPT0gJ2JvdHRvbScpIHtcclxuICAgICAgICAgICAgb2Zmc2V0cy54ID0gYm94LmxlZnQgKyBib3gud2lkdGgvMiAtIHRoaXMubGFiZWxXaWR0aC8yO1xyXG4gICAgICAgICAgICBvZmZzZXRzLnkgPSBib3gudG9wICsgYm94LmhlaWdodCAtIHRoaXMubGFiZWxIZWlnaHQ7XHJcbiAgICAgICAgfSBlbHNlIGlmICh0aGlzLnBvc2l0aW9uID09ICd0b3AnKSB7XHJcbiAgICAgICAgICAgIG9mZnNldHMueCA9IGJveC5sZWZ0ICsgYm94LndpZHRoLzIgLSB0aGlzLmxhYmVsV2lkdGgvMjtcclxuICAgICAgICAgICAgb2Zmc2V0cy55ID0gYm94LnRvcDtcclxuICAgICAgICB9IGVsc2UgaWYgKHRoaXMucG9zaXRpb24gPT0gJ2xlZnQnKSB7XHJcbiAgICAgICAgICAgIG9mZnNldHMuZGVncmVlcyA9IC05MDtcclxuICAgICAgICAgICAgb2Zmc2V0cy54ID0gYm94LmxlZnQgLSB0aGlzLmxhYmVsV2lkdGgvMiArIHRoaXMubGFiZWxIZWlnaHQvMjtcclxuICAgICAgICAgICAgb2Zmc2V0cy55ID0gYm94LmhlaWdodC8yICsgYm94LnRvcDtcclxuICAgICAgICB9IGVsc2UgaWYgKHRoaXMucG9zaXRpb24gPT0gJ3JpZ2h0Jykge1xyXG4gICAgICAgICAgICBvZmZzZXRzLmRlZ3JlZXMgPSA5MDtcclxuICAgICAgICAgICAgb2Zmc2V0cy54ID0gYm94LmxlZnQgKyBib3gud2lkdGggLSB0aGlzLmxhYmVsV2lkdGgvMlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAtIHRoaXMubGFiZWxIZWlnaHQvMjtcclxuICAgICAgICAgICAgb2Zmc2V0cy55ID0gYm94LmhlaWdodC8yICsgYm94LnRvcDtcclxuICAgICAgICB9XHJcbiAgICAgICAgb2Zmc2V0cy54ID0gTWF0aC5yb3VuZChvZmZzZXRzLngpO1xyXG4gICAgICAgIG9mZnNldHMueSA9IE1hdGgucm91bmQob2Zmc2V0cy55KTtcclxuXHJcbiAgICAgICAgcmV0dXJuIG9mZnNldHM7XHJcbiAgICB9O1xyXG5cclxuICAgIENzc1RyYW5zZm9ybUF4aXNMYWJlbC5wcm90b3R5cGUuZHJhdyA9IGZ1bmN0aW9uKGJveCkge1xyXG4gICAgICAgIHRoaXMucGxvdC5nZXRQbGFjZWhvbGRlcigpLmZpbmQoXCIuXCIgKyB0aGlzLmF4aXNOYW1lICsgXCJMYWJlbFwiKS5yZW1vdmUoKTtcclxuICAgICAgICB2YXIgb2Zmc2V0cyA9IHRoaXMuY2FsY3VsYXRlT2Zmc2V0cyhib3gpO1xyXG4gICAgICAgIHRoaXMuZWxlbSA9ICQoJzxkaXYgY2xhc3M9XCJheGlzTGFiZWxzICcgKyB0aGlzLmF4aXNOYW1lICtcclxuICAgICAgICAgICAgICAgICAgICAgICdMYWJlbFwiIHN0eWxlPVwicG9zaXRpb246YWJzb2x1dGU7ICcgK1xyXG4gICAgICAgICAgICAgICAgICAgICAgdGhpcy50cmFuc2Zvcm1zKG9mZnNldHMuZGVncmVlcywgb2Zmc2V0cy54LCBvZmZzZXRzLnkpICtcclxuICAgICAgICAgICAgICAgICAgICAgICdcIj4nICsgdGhpcy5vcHRzLmF4aXNMYWJlbCArICc8L2Rpdj4nKTtcclxuICAgICAgICB0aGlzLnBsb3QuZ2V0UGxhY2Vob2xkZXIoKS5hcHBlbmQodGhpcy5lbGVtKTtcclxuICAgIH07XHJcblxyXG5cclxuICAgIEllVHJhbnNmb3JtQXhpc0xhYmVsLnByb3RvdHlwZSA9IG5ldyBDc3NUcmFuc2Zvcm1BeGlzTGFiZWwoKTtcclxuICAgIEllVHJhbnNmb3JtQXhpc0xhYmVsLnByb3RvdHlwZS5jb25zdHJ1Y3RvciA9IEllVHJhbnNmb3JtQXhpc0xhYmVsO1xyXG4gICAgZnVuY3Rpb24gSWVUcmFuc2Zvcm1BeGlzTGFiZWwoYXhpc05hbWUsIHBvc2l0aW9uLCBwYWRkaW5nLCBwbG90LCBvcHRzKSB7XHJcbiAgICAgICAgQ3NzVHJhbnNmb3JtQXhpc0xhYmVsLnByb3RvdHlwZS5jb25zdHJ1Y3Rvci5jYWxsKHRoaXMsIGF4aXNOYW1lLFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBwb3NpdGlvbiwgcGFkZGluZyxcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcGxvdCwgb3B0cyk7XHJcbiAgICAgICAgdGhpcy5yZXF1aXJlc1Jlc2l6ZSA9IGZhbHNlO1xyXG4gICAgfVxyXG5cclxuICAgIEllVHJhbnNmb3JtQXhpc0xhYmVsLnByb3RvdHlwZS50cmFuc2Zvcm1zID0gZnVuY3Rpb24oZGVncmVlcywgeCwgeSkge1xyXG4gICAgICAgIC8vIEkgZGlkbid0IGZlZWwgbGlrZSBsZWFybmluZyB0aGUgY3JhenkgTWF0cml4IHN0dWZmLCBzbyB0aGlzIHVzZXNcclxuICAgICAgICAvLyBhIGNvbWJpbmF0aW9uIG9mIHRoZSByb3RhdGlvbiB0cmFuc2Zvcm0gYW5kIENTUyBwb3NpdGlvbmluZy5cclxuICAgICAgICB2YXIgcyA9ICcnO1xyXG4gICAgICAgIGlmIChkZWdyZWVzICE9IDApIHtcclxuICAgICAgICAgICAgdmFyIHJvdGF0aW9uID0gZGVncmVlcy85MDtcclxuICAgICAgICAgICAgd2hpbGUgKHJvdGF0aW9uIDwgMCkge1xyXG4gICAgICAgICAgICAgICAgcm90YXRpb24gKz0gNDtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICBzICs9ICcgZmlsdGVyOiBwcm9naWQ6RFhJbWFnZVRyYW5zZm9ybS5NaWNyb3NvZnQuQmFzaWNJbWFnZShyb3RhdGlvbj0nICsgcm90YXRpb24gKyAnKTsgJztcclxuICAgICAgICAgICAgLy8gc2VlIGJlbG93XHJcbiAgICAgICAgICAgIHRoaXMucmVxdWlyZXNSZXNpemUgPSAodGhpcy5wb3NpdGlvbiA9PSAncmlnaHQnKTtcclxuICAgICAgICB9XHJcbiAgICAgICAgaWYgKHggIT0gMCkge1xyXG4gICAgICAgICAgICBzICs9ICdsZWZ0OiAnICsgeCArICdweDsgJztcclxuICAgICAgICB9XHJcbiAgICAgICAgaWYgKHkgIT0gMCkge1xyXG4gICAgICAgICAgICBzICs9ICd0b3A6ICcgKyB5ICsgJ3B4OyAnO1xyXG4gICAgICAgIH1cclxuICAgICAgICByZXR1cm4gcztcclxuICAgIH07XHJcblxyXG4gICAgSWVUcmFuc2Zvcm1BeGlzTGFiZWwucHJvdG90eXBlLmNhbGN1bGF0ZU9mZnNldHMgPSBmdW5jdGlvbihib3gpIHtcclxuICAgICAgICB2YXIgb2Zmc2V0cyA9IENzc1RyYW5zZm9ybUF4aXNMYWJlbC5wcm90b3R5cGUuY2FsY3VsYXRlT2Zmc2V0cy5jYWxsKFxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgIHRoaXMsIGJveCk7XHJcbiAgICAgICAgLy8gYWRqdXN0IHNvbWUgdmFsdWVzIHRvIHRha2UgaW50byBhY2NvdW50IGRpZmZlcmVuY2VzIGJldHdlZW5cclxuICAgICAgICAvLyBDU1MgYW5kIElFIHJvdGF0aW9ucy5cclxuICAgICAgICBpZiAodGhpcy5wb3NpdGlvbiA9PSAndG9wJykge1xyXG4gICAgICAgICAgICAvLyBGSVhNRTogbm90IHN1cmUgd2h5LCBidXQgcGxhY2luZyB0aGlzIGV4YWN0bHkgYXQgdGhlIHRvcCBjYXVzZXNcclxuICAgICAgICAgICAgLy8gdGhlIHRvcCBheGlzIGxhYmVsIHRvIGZsaXAgdG8gdGhlIGJvdHRvbS4uLlxyXG4gICAgICAgICAgICBvZmZzZXRzLnkgPSBib3gudG9wICsgMTtcclxuICAgICAgICB9IGVsc2UgaWYgKHRoaXMucG9zaXRpb24gPT0gJ2xlZnQnKSB7XHJcbiAgICAgICAgICAgIG9mZnNldHMueCA9IGJveC5sZWZ0O1xyXG4gICAgICAgICAgICBvZmZzZXRzLnkgPSBib3guaGVpZ2h0LzIgKyBib3gudG9wIC0gdGhpcy5sYWJlbFdpZHRoLzI7XHJcbiAgICAgICAgfSBlbHNlIGlmICh0aGlzLnBvc2l0aW9uID09ICdyaWdodCcpIHtcclxuICAgICAgICAgICAgb2Zmc2V0cy54ID0gYm94LmxlZnQgKyBib3gud2lkdGggLSB0aGlzLmxhYmVsSGVpZ2h0O1xyXG4gICAgICAgICAgICBvZmZzZXRzLnkgPSBib3guaGVpZ2h0LzIgKyBib3gudG9wIC0gdGhpcy5sYWJlbFdpZHRoLzI7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIHJldHVybiBvZmZzZXRzO1xyXG4gICAgfTtcclxuXHJcbiAgICBJZVRyYW5zZm9ybUF4aXNMYWJlbC5wcm90b3R5cGUuZHJhdyA9IGZ1bmN0aW9uKGJveCkge1xyXG4gICAgICAgIENzc1RyYW5zZm9ybUF4aXNMYWJlbC5wcm90b3R5cGUuZHJhdy5jYWxsKHRoaXMsIGJveCk7XHJcbiAgICAgICAgaWYgKHRoaXMucmVxdWlyZXNSZXNpemUpIHtcclxuICAgICAgICAgICAgdGhpcy5lbGVtID0gdGhpcy5wbG90LmdldFBsYWNlaG9sZGVyKCkuZmluZChcIi5cIiArIHRoaXMuYXhpc05hbWUgK1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIFwiTGFiZWxcIik7XHJcbiAgICAgICAgICAgIC8vIFNpbmNlIHdlIHVzZWQgQ1NTIHBvc2l0aW9uaW5nIGluc3RlYWQgb2YgdHJhbnNmb3JtcyBmb3JcclxuICAgICAgICAgICAgLy8gdHJhbnNsYXRpbmcgdGhlIGVsZW1lbnQsIGFuZCBzaW5jZSB0aGUgcG9zaXRpb25pbmcgaXMgZG9uZVxyXG4gICAgICAgICAgICAvLyBiZWZvcmUgYW55IHJvdGF0aW9ucywgd2UgaGF2ZSB0byByZXNldCB0aGUgd2lkdGggYW5kIGhlaWdodFxyXG4gICAgICAgICAgICAvLyBpbiBjYXNlIHRoZSBicm93c2VyIHdyYXBwZWQgdGhlIHRleHQgKHNwZWNpZmljYWxseSBmb3IgdGhlXHJcbiAgICAgICAgICAgIC8vIHkyYXhpcykuXHJcbiAgICAgICAgICAgIHRoaXMuZWxlbS5jc3MoJ3dpZHRoJywgdGhpcy5sYWJlbFdpZHRoKTtcclxuICAgICAgICAgICAgdGhpcy5lbGVtLmNzcygnaGVpZ2h0JywgdGhpcy5sYWJlbEhlaWdodCk7XHJcbiAgICAgICAgfVxyXG4gICAgfTtcclxuXHJcblxyXG4gICAgZnVuY3Rpb24gaW5pdChwbG90KSB7XHJcbiAgICAgICAgcGxvdC5ob29rcy5wcm9jZXNzT3B0aW9ucy5wdXNoKGZ1bmN0aW9uIChwbG90LCBvcHRpb25zKSB7XHJcblxyXG4gICAgICAgICAgICBpZiAoIW9wdGlvbnMuYXhpc0xhYmVscy5zaG93KVxyXG4gICAgICAgICAgICAgICAgcmV0dXJuO1xyXG5cclxuICAgICAgICAgICAgLy8gVGhpcyBpcyBraW5kIG9mIGEgaGFjay4gVGhlcmUgYXJlIG5vIGhvb2tzIGluIEZsb3QgYmV0d2VlblxyXG4gICAgICAgICAgICAvLyB0aGUgY3JlYXRpb24gYW5kIG1lYXN1cmluZyBvZiB0aGUgdGlja3MgKHNldFRpY2tzLCBtZWFzdXJlVGlja0xhYmVsc1xyXG4gICAgICAgICAgICAvLyBpbiBzZXR1cEdyaWQoKSApIGFuZCB0aGUgZHJhd2luZyBvZiB0aGUgdGlja3MgYW5kIHBsb3QgYm94XHJcbiAgICAgICAgICAgIC8vIChpbnNlcnRBeGlzTGFiZWxzIGluIHNldHVwR3JpZCgpICkuXHJcbiAgICAgICAgICAgIC8vXHJcbiAgICAgICAgICAgIC8vIFRoZXJlZm9yZSwgd2UgdXNlIGEgdHJpY2sgd2hlcmUgd2UgcnVuIHRoZSBkcmF3IHJvdXRpbmUgdHdpY2U6XHJcbiAgICAgICAgICAgIC8vIHRoZSBmaXJzdCB0aW1lIHRvIGdldCB0aGUgdGljayBtZWFzdXJlbWVudHMsIHNvIHRoYXQgd2UgY2FuIGNoYW5nZVxyXG4gICAgICAgICAgICAvLyB0aGVtLCBhbmQgdGhlbiBoYXZlIGl0IGRyYXcgaXQgYWdhaW4uXHJcbiAgICAgICAgICAgIHZhciBzZWNvbmRQYXNzID0gZmFsc2U7XHJcblxyXG4gICAgICAgICAgICB2YXIgYXhpc0xhYmVscyA9IHt9O1xyXG4gICAgICAgICAgICB2YXIgYXhpc09mZnNldENvdW50cyA9IHsgbGVmdDogMCwgcmlnaHQ6IDAsIHRvcDogMCwgYm90dG9tOiAwIH07XHJcblxyXG4gICAgICAgICAgICB2YXIgZGVmYXVsdFBhZGRpbmcgPSAyOyAgLy8gcGFkZGluZyBiZXR3ZWVuIGF4aXMgYW5kIHRpY2sgbGFiZWxzXHJcbiAgICAgICAgICAgIHBsb3QuaG9va3MuZHJhdy5wdXNoKGZ1bmN0aW9uIChwbG90LCBjdHgpIHtcclxuICAgICAgICAgICAgICAgIHZhciBoYXNBeGlzTGFiZWxzID0gZmFsc2U7XHJcbiAgICAgICAgICAgICAgICBpZiAoIXNlY29uZFBhc3MpIHtcclxuICAgICAgICAgICAgICAgICAgICAvLyBNRUFTVVJFIEFORCBTRVQgT1BUSU9OU1xyXG4gICAgICAgICAgICAgICAgICAgICQuZWFjaChwbG90LmdldEF4ZXMoKSwgZnVuY3Rpb24oYXhpc05hbWUsIGF4aXMpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgdmFyIG9wdHMgPSBheGlzLm9wdGlvbnMgLy8gRmxvdCAwLjdcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHx8IHBsb3QuZ2V0T3B0aW9ucygpW2F4aXNOYW1lXTsgLy8gRmxvdCAwLjZcclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIC8vIEhhbmRsZSByZWRyYXdzIGluaXRpYXRlZCBvdXRzaWRlIG9mIHRoaXMgcGx1Zy1pbi5cclxuICAgICAgICAgICAgICAgICAgICAgICAgaWYgKGF4aXNOYW1lIGluIGF4aXNMYWJlbHMpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGF4aXMubGFiZWxIZWlnaHQgPSBheGlzLmxhYmVsSGVpZ2h0IC1cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBheGlzTGFiZWxzW2F4aXNOYW1lXS5oZWlnaHQ7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBheGlzLmxhYmVsV2lkdGggPSBheGlzLmxhYmVsV2lkdGggLVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGF4aXNMYWJlbHNbYXhpc05hbWVdLndpZHRoO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgb3B0cy5sYWJlbEhlaWdodCA9IGF4aXMubGFiZWxIZWlnaHQ7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBvcHRzLmxhYmVsV2lkdGggPSBheGlzLmxhYmVsV2lkdGg7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBheGlzTGFiZWxzW2F4aXNOYW1lXS5jbGVhbnVwKCk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBkZWxldGUgYXhpc0xhYmVsc1theGlzTmFtZV07XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGlmICghb3B0cyB8fCAhb3B0cy5heGlzTGFiZWwgfHwgIWF4aXMuc2hvdylcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHJldHVybjtcclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGhhc0F4aXNMYWJlbHMgPSB0cnVlO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB2YXIgcmVuZGVyZXIgPSBudWxsO1xyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgaWYgKCFvcHRzLmF4aXNMYWJlbFVzZUh0bWwgJiZcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIG5hdmlnYXRvci5hcHBOYW1lID09ICdNaWNyb3NvZnQgSW50ZXJuZXQgRXhwbG9yZXInKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB2YXIgdWEgPSBuYXZpZ2F0b3IudXNlckFnZW50O1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgdmFyIHJlICA9IG5ldyBSZWdFeHAoXCJNU0lFIChbMC05XXsxLH1bXFwuMC05XXswLH0pXCIpO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgaWYgKHJlLmV4ZWModWEpICE9IG51bGwpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBydiA9IHBhcnNlRmxvYXQoUmVnRXhwLiQxKTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGlmIChydiA+PSA5ICYmICFvcHRzLmF4aXNMYWJlbFVzZUNhbnZhcyAmJiAhb3B0cy5heGlzTGFiZWxVc2VIdG1sKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcmVuZGVyZXIgPSBDc3NUcmFuc2Zvcm1BeGlzTGFiZWw7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB9IGVsc2UgaWYgKCFvcHRzLmF4aXNMYWJlbFVzZUNhbnZhcyAmJiAhb3B0cy5heGlzTGFiZWxVc2VIdG1sKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcmVuZGVyZXIgPSBJZVRyYW5zZm9ybUF4aXNMYWJlbDtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIH0gZWxzZSBpZiAob3B0cy5heGlzTGFiZWxVc2VDYW52YXMpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICByZW5kZXJlciA9IENhbnZhc0F4aXNMYWJlbDtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcmVuZGVyZXIgPSBIdG1sQXhpc0xhYmVsO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgaWYgKG9wdHMuYXhpc0xhYmVsVXNlSHRtbCB8fCAoIWNzczNUcmFuc2l0aW9uU3VwcG9ydGVkKCkgJiYgIWNhbnZhc1RleHRTdXBwb3J0ZWQoKSkgJiYgIW9wdHMuYXhpc0xhYmVsVXNlQ2FudmFzKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcmVuZGVyZXIgPSBIdG1sQXhpc0xhYmVsO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgfSBlbHNlIGlmIChvcHRzLmF4aXNMYWJlbFVzZUNhbnZhcyB8fCAhY3NzM1RyYW5zaXRpb25TdXBwb3J0ZWQoKSkge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHJlbmRlcmVyID0gQ2FudmFzQXhpc0xhYmVsO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICByZW5kZXJlciA9IENzc1RyYW5zZm9ybUF4aXNMYWJlbDtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgICAgICAgICAgfVxyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgdmFyIHBhZGRpbmcgPSBvcHRzLmF4aXNMYWJlbFBhZGRpbmcgPT09IHVuZGVmaW5lZCA/XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgZGVmYXVsdFBhZGRpbmcgOiBvcHRzLmF4aXNMYWJlbFBhZGRpbmc7XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICBheGlzTGFiZWxzW2F4aXNOYW1lXSA9IG5ldyByZW5kZXJlcihheGlzTmFtZSxcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgYXhpcy5wb3NpdGlvbiwgcGFkZGluZyxcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcGxvdCwgb3B0cyk7XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICAvLyBmbG90IGludGVycHJldHMgYXhpcy5sYWJlbEhlaWdodCBhbmQgLmxhYmVsV2lkdGggYXNcclxuICAgICAgICAgICAgICAgICAgICAgICAgLy8gdGhlIGhlaWdodCBhbmQgd2lkdGggb2YgdGhlIHRpY2sgbGFiZWxzLiBXZSBpbmNyZWFzZVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAvLyB0aGVzZSB2YWx1ZXMgdG8gbWFrZSByb29tIGZvciB0aGUgYXhpcyBsYWJlbCBhbmRcclxuICAgICAgICAgICAgICAgICAgICAgICAgLy8gcGFkZGluZy5cclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGF4aXNMYWJlbHNbYXhpc05hbWVdLmNhbGN1bGF0ZVNpemUoKTtcclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIC8vIEF4aXNMYWJlbC5oZWlnaHQgYW5kIC53aWR0aCBhcmUgdGhlIHNpemUgb2YgdGhlXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIC8vIGF4aXMgbGFiZWwgYW5kIHBhZGRpbmcuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIC8vIEp1c3Qgc2V0IG9wdHMgaGVyZSBiZWNhdXNlIGF4aXMgd2lsbCBiZSBzb3J0ZWQgb3V0IG9uXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIC8vIHRoZSByZWRyYXcuXHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICBvcHRzLmxhYmVsSGVpZ2h0ID0gYXhpcy5sYWJlbEhlaWdodCArXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBheGlzTGFiZWxzW2F4aXNOYW1lXS5oZWlnaHQ7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIG9wdHMubGFiZWxXaWR0aCA9IGF4aXMubGFiZWxXaWR0aCArXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBheGlzTGFiZWxzW2F4aXNOYW1lXS53aWR0aDtcclxuICAgICAgICAgICAgICAgICAgICB9KTtcclxuXHJcbiAgICAgICAgICAgICAgICAgICAgLy8gSWYgdGhlcmUgYXJlIGF4aXMgbGFiZWxzLCByZS1kcmF3IHdpdGggbmV3IGxhYmVsIHdpZHRocyBhbmRcclxuICAgICAgICAgICAgICAgICAgICAvLyBoZWlnaHRzLlxyXG5cclxuICAgICAgICAgICAgICAgICAgICBpZiAoaGFzQXhpc0xhYmVscykge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBzZWNvbmRQYXNzID0gdHJ1ZTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgcGxvdC5zZXR1cEdyaWQoKTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgcGxvdC5kcmF3KCk7XHJcbiAgICAgICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgICAgICBzZWNvbmRQYXNzID0gZmFsc2U7XHJcbiAgICAgICAgICAgICAgICAgICAgLy8gRFJBV1xyXG4gICAgICAgICAgICAgICAgICAgICQuZWFjaChwbG90LmdldEF4ZXMoKSwgZnVuY3Rpb24oYXhpc05hbWUsIGF4aXMpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgdmFyIG9wdHMgPSBheGlzLm9wdGlvbnMgLy8gRmxvdCAwLjdcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHx8IHBsb3QuZ2V0T3B0aW9ucygpW2F4aXNOYW1lXTsgLy8gRmxvdCAwLjZcclxuICAgICAgICAgICAgICAgICAgICAgICAgaWYgKCFvcHRzIHx8ICFvcHRzLmF4aXNMYWJlbCB8fCAhYXhpcy5zaG93KVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgcmV0dXJuO1xyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgYXhpc0xhYmVsc1theGlzTmFtZV0uZHJhdyhheGlzLmJveCk7XHJcbiAgICAgICAgICAgICAgICAgICAgfSk7XHJcbiAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIH0pO1xyXG4gICAgICAgIH0pO1xyXG4gICAgfVxyXG5cclxuXHJcbiAgICAkLnBsb3QucGx1Z2lucy5wdXNoKHtcclxuICAgICAgICBpbml0OiBpbml0LFxyXG4gICAgICAgIG9wdGlvbnM6IG9wdGlvbnMsXHJcbiAgICAgICAgbmFtZTogJ2F4aXNMYWJlbHMnLFxyXG4gICAgICAgIHZlcnNpb246ICcyLjAnXHJcbiAgICB9KTtcclxufSkoalF1ZXJ5KTsiLCIvKiBKYXZhc2NyaXB0IHBsb3R0aW5nIGxpYnJhcnkgZm9yIGpRdWVyeSwgdmVyc2lvbiAwLjguMy5cclxuXHJcbkNvcHlyaWdodCAoYykgMjAwNy0yMDE0IElPTEEgYW5kIE9sZSBMYXVyc2VuLlxyXG5MaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UuXHJcblxyXG4qL1xyXG4oZnVuY3Rpb24oJCl7dmFyIG9wdGlvbnM9e2Nyb3NzaGFpcjp7bW9kZTpudWxsLGNvbG9yOlwicmdiYSgxNzAsIDAsIDAsIDAuODApXCIsbGluZVdpZHRoOjF9fTtmdW5jdGlvbiBpbml0KHBsb3Qpe3ZhciBjcm9zc2hhaXI9e3g6LTEseTotMSxsb2NrZWQ6ZmFsc2V9O3Bsb3Quc2V0Q3Jvc3NoYWlyPWZ1bmN0aW9uIHNldENyb3NzaGFpcihwb3Mpe2lmKCFwb3MpY3Jvc3NoYWlyLng9LTE7ZWxzZXt2YXIgbz1wbG90LnAyYyhwb3MpO2Nyb3NzaGFpci54PU1hdGgubWF4KDAsTWF0aC5taW4oby5sZWZ0LHBsb3Qud2lkdGgoKSkpO2Nyb3NzaGFpci55PU1hdGgubWF4KDAsTWF0aC5taW4oby50b3AscGxvdC5oZWlnaHQoKSkpfXBsb3QudHJpZ2dlclJlZHJhd092ZXJsYXkoKX07cGxvdC5jbGVhckNyb3NzaGFpcj1wbG90LnNldENyb3NzaGFpcjtwbG90LmxvY2tDcm9zc2hhaXI9ZnVuY3Rpb24gbG9ja0Nyb3NzaGFpcihwb3Mpe2lmKHBvcylwbG90LnNldENyb3NzaGFpcihwb3MpO2Nyb3NzaGFpci5sb2NrZWQ9dHJ1ZX07cGxvdC51bmxvY2tDcm9zc2hhaXI9ZnVuY3Rpb24gdW5sb2NrQ3Jvc3NoYWlyKCl7Y3Jvc3NoYWlyLmxvY2tlZD1mYWxzZX07ZnVuY3Rpb24gb25Nb3VzZU91dChlKXtpZihjcm9zc2hhaXIubG9ja2VkKXJldHVybjtpZihjcm9zc2hhaXIueCE9LTEpe2Nyb3NzaGFpci54PS0xO3Bsb3QudHJpZ2dlclJlZHJhd092ZXJsYXkoKX19ZnVuY3Rpb24gb25Nb3VzZU1vdmUoZSl7aWYoY3Jvc3NoYWlyLmxvY2tlZClyZXR1cm47aWYocGxvdC5nZXRTZWxlY3Rpb24mJnBsb3QuZ2V0U2VsZWN0aW9uKCkpe2Nyb3NzaGFpci54PS0xO3JldHVybn12YXIgb2Zmc2V0PXBsb3Qub2Zmc2V0KCk7Y3Jvc3NoYWlyLng9TWF0aC5tYXgoMCxNYXRoLm1pbihlLnBhZ2VYLW9mZnNldC5sZWZ0LHBsb3Qud2lkdGgoKSkpO2Nyb3NzaGFpci55PU1hdGgubWF4KDAsTWF0aC5taW4oZS5wYWdlWS1vZmZzZXQudG9wLHBsb3QuaGVpZ2h0KCkpKTtwbG90LnRyaWdnZXJSZWRyYXdPdmVybGF5KCl9cGxvdC5ob29rcy5iaW5kRXZlbnRzLnB1c2goZnVuY3Rpb24ocGxvdCxldmVudEhvbGRlcil7aWYoIXBsb3QuZ2V0T3B0aW9ucygpLmNyb3NzaGFpci5tb2RlKXJldHVybjtldmVudEhvbGRlci5tb3VzZW91dChvbk1vdXNlT3V0KTtldmVudEhvbGRlci5tb3VzZW1vdmUob25Nb3VzZU1vdmUpfSk7cGxvdC5ob29rcy5kcmF3T3ZlcmxheS5wdXNoKGZ1bmN0aW9uKHBsb3QsY3R4KXt2YXIgYz1wbG90LmdldE9wdGlvbnMoKS5jcm9zc2hhaXI7aWYoIWMubW9kZSlyZXR1cm47dmFyIHBsb3RPZmZzZXQ9cGxvdC5nZXRQbG90T2Zmc2V0KCk7Y3R4LnNhdmUoKTtjdHgudHJhbnNsYXRlKHBsb3RPZmZzZXQubGVmdCxwbG90T2Zmc2V0LnRvcCk7aWYoY3Jvc3NoYWlyLnghPS0xKXt2YXIgYWRqPXBsb3QuZ2V0T3B0aW9ucygpLmNyb3NzaGFpci5saW5lV2lkdGglMj8uNTowO2N0eC5zdHJva2VTdHlsZT1jLmNvbG9yO2N0eC5saW5lV2lkdGg9Yy5saW5lV2lkdGg7Y3R4LmxpbmVKb2luPVwicm91bmRcIjtjdHguYmVnaW5QYXRoKCk7aWYoYy5tb2RlLmluZGV4T2YoXCJ4XCIpIT0tMSl7dmFyIGRyYXdYPU1hdGguZmxvb3IoY3Jvc3NoYWlyLngpK2FkajtjdHgubW92ZVRvKGRyYXdYLDApO2N0eC5saW5lVG8oZHJhd1gscGxvdC5oZWlnaHQoKSl9aWYoYy5tb2RlLmluZGV4T2YoXCJ5XCIpIT0tMSl7dmFyIGRyYXdZPU1hdGguZmxvb3IoY3Jvc3NoYWlyLnkpK2FkajtjdHgubW92ZVRvKDAsZHJhd1kpO2N0eC5saW5lVG8ocGxvdC53aWR0aCgpLGRyYXdZKX1jdHguc3Ryb2tlKCl9Y3R4LnJlc3RvcmUoKX0pO3Bsb3QuaG9va3Muc2h1dGRvd24ucHVzaChmdW5jdGlvbihwbG90LGV2ZW50SG9sZGVyKXtldmVudEhvbGRlci51bmJpbmQoXCJtb3VzZW91dFwiLG9uTW91c2VPdXQpO2V2ZW50SG9sZGVyLnVuYmluZChcIm1vdXNlbW92ZVwiLG9uTW91c2VNb3ZlKX0pfSQucGxvdC5wbHVnaW5zLnB1c2goe2luaXQ6aW5pdCxvcHRpb25zOm9wdGlvbnMsbmFtZTpcImNyb3NzaGFpclwiLHZlcnNpb246XCIxLjBcIn0pfSkoalF1ZXJ5KTsiLCIvKiBKYXZhc2NyaXB0IHBsb3R0aW5nIGxpYnJhcnkgZm9yIGpRdWVyeSwgdmVyc2lvbiAwLjguMy5cclxuXHJcbkNvcHlyaWdodCAoYykgMjAwNy0yMDE0IElPTEEgYW5kIE9sZSBMYXVyc2VuLlxyXG5MaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UuXHJcblxyXG4qL1xyXG4oZnVuY3Rpb24oJCl7JC5jb2xvcj17fTskLmNvbG9yLm1ha2U9ZnVuY3Rpb24ocixnLGIsYSl7dmFyIG89e307by5yPXJ8fDA7by5nPWd8fDA7by5iPWJ8fDA7by5hPWEhPW51bGw/YToxO28uYWRkPWZ1bmN0aW9uKGMsZCl7Zm9yKHZhciBpPTA7aTxjLmxlbmd0aDsrK2kpb1tjLmNoYXJBdChpKV0rPWQ7cmV0dXJuIG8ubm9ybWFsaXplKCl9O28uc2NhbGU9ZnVuY3Rpb24oYyxmKXtmb3IodmFyIGk9MDtpPGMubGVuZ3RoOysraSlvW2MuY2hhckF0KGkpXSo9ZjtyZXR1cm4gby5ub3JtYWxpemUoKX07by50b1N0cmluZz1mdW5jdGlvbigpe2lmKG8uYT49MSl7cmV0dXJuXCJyZ2IoXCIrW28ucixvLmcsby5iXS5qb2luKFwiLFwiKStcIilcIn1lbHNle3JldHVyblwicmdiYShcIitbby5yLG8uZyxvLmIsby5hXS5qb2luKFwiLFwiKStcIilcIn19O28ubm9ybWFsaXplPWZ1bmN0aW9uKCl7ZnVuY3Rpb24gY2xhbXAobWluLHZhbHVlLG1heCl7cmV0dXJuIHZhbHVlPG1pbj9taW46dmFsdWU+bWF4P21heDp2YWx1ZX1vLnI9Y2xhbXAoMCxwYXJzZUludChvLnIpLDI1NSk7by5nPWNsYW1wKDAscGFyc2VJbnQoby5nKSwyNTUpO28uYj1jbGFtcCgwLHBhcnNlSW50KG8uYiksMjU1KTtvLmE9Y2xhbXAoMCxvLmEsMSk7cmV0dXJuIG99O28uY2xvbmU9ZnVuY3Rpb24oKXtyZXR1cm4gJC5jb2xvci5tYWtlKG8ucixvLmIsby5nLG8uYSl9O3JldHVybiBvLm5vcm1hbGl6ZSgpfTskLmNvbG9yLmV4dHJhY3Q9ZnVuY3Rpb24oZWxlbSxjc3Mpe3ZhciBjO2Rve2M9ZWxlbS5jc3MoY3NzKS50b0xvd2VyQ2FzZSgpO2lmKGMhPVwiXCImJmMhPVwidHJhbnNwYXJlbnRcIilicmVhaztlbGVtPWVsZW0ucGFyZW50KCl9d2hpbGUoZWxlbS5sZW5ndGgmJiEkLm5vZGVOYW1lKGVsZW0uZ2V0KDApLFwiYm9keVwiKSk7aWYoYz09XCJyZ2JhKDAsIDAsIDAsIDApXCIpYz1cInRyYW5zcGFyZW50XCI7cmV0dXJuICQuY29sb3IucGFyc2UoYyl9OyQuY29sb3IucGFyc2U9ZnVuY3Rpb24oc3RyKXt2YXIgcmVzLG09JC5jb2xvci5tYWtlO2lmKHJlcz0vcmdiXFwoXFxzKihbMC05XXsxLDN9KVxccyosXFxzKihbMC05XXsxLDN9KVxccyosXFxzKihbMC05XXsxLDN9KVxccypcXCkvLmV4ZWMoc3RyKSlyZXR1cm4gbShwYXJzZUludChyZXNbMV0sMTApLHBhcnNlSW50KHJlc1syXSwxMCkscGFyc2VJbnQocmVzWzNdLDEwKSk7aWYocmVzPS9yZ2JhXFwoXFxzKihbMC05XXsxLDN9KVxccyosXFxzKihbMC05XXsxLDN9KVxccyosXFxzKihbMC05XXsxLDN9KVxccyosXFxzKihbMC05XSsoPzpcXC5bMC05XSspPylcXHMqXFwpLy5leGVjKHN0cikpcmV0dXJuIG0ocGFyc2VJbnQocmVzWzFdLDEwKSxwYXJzZUludChyZXNbMl0sMTApLHBhcnNlSW50KHJlc1szXSwxMCkscGFyc2VGbG9hdChyZXNbNF0pKTtpZihyZXM9L3JnYlxcKFxccyooWzAtOV0rKD86XFwuWzAtOV0rKT8pXFwlXFxzKixcXHMqKFswLTldKyg/OlxcLlswLTldKyk/KVxcJVxccyosXFxzKihbMC05XSsoPzpcXC5bMC05XSspPylcXCVcXHMqXFwpLy5leGVjKHN0cikpcmV0dXJuIG0ocGFyc2VGbG9hdChyZXNbMV0pKjIuNTUscGFyc2VGbG9hdChyZXNbMl0pKjIuNTUscGFyc2VGbG9hdChyZXNbM10pKjIuNTUpO2lmKHJlcz0vcmdiYVxcKFxccyooWzAtOV0rKD86XFwuWzAtOV0rKT8pXFwlXFxzKixcXHMqKFswLTldKyg/OlxcLlswLTldKyk/KVxcJVxccyosXFxzKihbMC05XSsoPzpcXC5bMC05XSspPylcXCVcXHMqLFxccyooWzAtOV0rKD86XFwuWzAtOV0rKT8pXFxzKlxcKS8uZXhlYyhzdHIpKXJldHVybiBtKHBhcnNlRmxvYXQocmVzWzFdKSoyLjU1LHBhcnNlRmxvYXQocmVzWzJdKSoyLjU1LHBhcnNlRmxvYXQocmVzWzNdKSoyLjU1LHBhcnNlRmxvYXQocmVzWzRdKSk7aWYocmVzPS8jKFthLWZBLUYwLTldezJ9KShbYS1mQS1GMC05XXsyfSkoW2EtZkEtRjAtOV17Mn0pLy5leGVjKHN0cikpcmV0dXJuIG0ocGFyc2VJbnQocmVzWzFdLDE2KSxwYXJzZUludChyZXNbMl0sMTYpLHBhcnNlSW50KHJlc1szXSwxNikpO2lmKHJlcz0vIyhbYS1mQS1GMC05XSkoW2EtZkEtRjAtOV0pKFthLWZBLUYwLTldKS8uZXhlYyhzdHIpKXJldHVybiBtKHBhcnNlSW50KHJlc1sxXStyZXNbMV0sMTYpLHBhcnNlSW50KHJlc1syXStyZXNbMl0sMTYpLHBhcnNlSW50KHJlc1szXStyZXNbM10sMTYpKTt2YXIgbmFtZT0kLnRyaW0oc3RyKS50b0xvd2VyQ2FzZSgpO2lmKG5hbWU9PVwidHJhbnNwYXJlbnRcIilyZXR1cm4gbSgyNTUsMjU1LDI1NSwwKTtlbHNle3Jlcz1sb29rdXBDb2xvcnNbbmFtZV18fFswLDAsMF07cmV0dXJuIG0ocmVzWzBdLHJlc1sxXSxyZXNbMl0pfX07dmFyIGxvb2t1cENvbG9ycz17YXF1YTpbMCwyNTUsMjU1XSxhenVyZTpbMjQwLDI1NSwyNTVdLGJlaWdlOlsyNDUsMjQ1LDIyMF0sYmxhY2s6WzAsMCwwXSxibHVlOlswLDAsMjU1XSxicm93bjpbMTY1LDQyLDQyXSxjeWFuOlswLDI1NSwyNTVdLGRhcmtibHVlOlswLDAsMTM5XSxkYXJrY3lhbjpbMCwxMzksMTM5XSxkYXJrZ3JleTpbMTY5LDE2OSwxNjldLGRhcmtncmVlbjpbMCwxMDAsMF0sZGFya2toYWtpOlsxODksMTgzLDEwN10sZGFya21hZ2VudGE6WzEzOSwwLDEzOV0sZGFya29saXZlZ3JlZW46Wzg1LDEwNyw0N10sZGFya29yYW5nZTpbMjU1LDE0MCwwXSxkYXJrb3JjaGlkOlsxNTMsNTAsMjA0XSxkYXJrcmVkOlsxMzksMCwwXSxkYXJrc2FsbW9uOlsyMzMsMTUwLDEyMl0sZGFya3Zpb2xldDpbMTQ4LDAsMjExXSxmdWNoc2lhOlsyNTUsMCwyNTVdLGdvbGQ6WzI1NSwyMTUsMF0sZ3JlZW46WzAsMTI4LDBdLGluZGlnbzpbNzUsMCwxMzBdLGtoYWtpOlsyNDAsMjMwLDE0MF0sbGlnaHRibHVlOlsxNzMsMjE2LDIzMF0sbGlnaHRjeWFuOlsyMjQsMjU1LDI1NV0sbGlnaHRncmVlbjpbMTQ0LDIzOCwxNDRdLGxpZ2h0Z3JleTpbMjExLDIxMSwyMTFdLGxpZ2h0cGluazpbMjU1LDE4MiwxOTNdLGxpZ2h0eWVsbG93OlsyNTUsMjU1LDIyNF0sbGltZTpbMCwyNTUsMF0sbWFnZW50YTpbMjU1LDAsMjU1XSxtYXJvb246WzEyOCwwLDBdLG5hdnk6WzAsMCwxMjhdLG9saXZlOlsxMjgsMTI4LDBdLG9yYW5nZTpbMjU1LDE2NSwwXSxwaW5rOlsyNTUsMTkyLDIwM10scHVycGxlOlsxMjgsMCwxMjhdLHZpb2xldDpbMTI4LDAsMTI4XSxyZWQ6WzI1NSwwLDBdLHNpbHZlcjpbMTkyLDE5MiwxOTJdLHdoaXRlOlsyNTUsMjU1LDI1NV0seWVsbG93OlsyNTUsMjU1LDBdfX0pKGpRdWVyeSk7KGZ1bmN0aW9uKCQpe3ZhciBoYXNPd25Qcm9wZXJ0eT1PYmplY3QucHJvdG90eXBlLmhhc093blByb3BlcnR5O2lmKCEkLmZuLmRldGFjaCl7JC5mbi5kZXRhY2g9ZnVuY3Rpb24oKXtyZXR1cm4gdGhpcy5lYWNoKGZ1bmN0aW9uKCl7aWYodGhpcy5wYXJlbnROb2RlKXt0aGlzLnBhcmVudE5vZGUucmVtb3ZlQ2hpbGQodGhpcyl9fSl9fWZ1bmN0aW9uIENhbnZhcyhjbHMsY29udGFpbmVyKXt2YXIgZWxlbWVudD1jb250YWluZXIuY2hpbGRyZW4oXCIuXCIrY2xzKVswXTtpZihlbGVtZW50PT1udWxsKXtlbGVtZW50PWRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoXCJjYW52YXNcIik7ZWxlbWVudC5jbGFzc05hbWU9Y2xzOyQoZWxlbWVudCkuY3NzKHtkaXJlY3Rpb246XCJsdHJcIixwb3NpdGlvbjpcImFic29sdXRlXCIsbGVmdDowLHRvcDowfSkuYXBwZW5kVG8oY29udGFpbmVyKTtpZighZWxlbWVudC5nZXRDb250ZXh0KXtpZih3aW5kb3cuR192bWxDYW52YXNNYW5hZ2VyKXtlbGVtZW50PXdpbmRvdy5HX3ZtbENhbnZhc01hbmFnZXIuaW5pdEVsZW1lbnQoZWxlbWVudCl9ZWxzZXt0aHJvdyBuZXcgRXJyb3IoXCJDYW52YXMgaXMgbm90IGF2YWlsYWJsZS4gSWYgeW91J3JlIHVzaW5nIElFIHdpdGggYSBmYWxsLWJhY2sgc3VjaCBhcyBFeGNhbnZhcywgdGhlbiB0aGVyZSdzIGVpdGhlciBhIG1pc3Rha2UgaW4geW91ciBjb25kaXRpb25hbCBpbmNsdWRlLCBvciB0aGUgcGFnZSBoYXMgbm8gRE9DVFlQRSBhbmQgaXMgcmVuZGVyaW5nIGluIFF1aXJrcyBNb2RlLlwiKX19fXRoaXMuZWxlbWVudD1lbGVtZW50O3ZhciBjb250ZXh0PXRoaXMuY29udGV4dD1lbGVtZW50LmdldENvbnRleHQoXCIyZFwiKTt2YXIgZGV2aWNlUGl4ZWxSYXRpbz13aW5kb3cuZGV2aWNlUGl4ZWxSYXRpb3x8MSxiYWNraW5nU3RvcmVSYXRpbz1jb250ZXh0LndlYmtpdEJhY2tpbmdTdG9yZVBpeGVsUmF0aW98fGNvbnRleHQubW96QmFja2luZ1N0b3JlUGl4ZWxSYXRpb3x8Y29udGV4dC5tc0JhY2tpbmdTdG9yZVBpeGVsUmF0aW98fGNvbnRleHQub0JhY2tpbmdTdG9yZVBpeGVsUmF0aW98fGNvbnRleHQuYmFja2luZ1N0b3JlUGl4ZWxSYXRpb3x8MTt0aGlzLnBpeGVsUmF0aW89ZGV2aWNlUGl4ZWxSYXRpby9iYWNraW5nU3RvcmVSYXRpbzt0aGlzLnJlc2l6ZShjb250YWluZXIud2lkdGgoKSxjb250YWluZXIuaGVpZ2h0KCkpO3RoaXMudGV4dENvbnRhaW5lcj1udWxsO3RoaXMudGV4dD17fTt0aGlzLl90ZXh0Q2FjaGU9e319Q2FudmFzLnByb3RvdHlwZS5yZXNpemU9ZnVuY3Rpb24od2lkdGgsaGVpZ2h0KXtpZih3aWR0aDw9MHx8aGVpZ2h0PD0wKXt0aHJvdyBuZXcgRXJyb3IoXCJJbnZhbGlkIGRpbWVuc2lvbnMgZm9yIHBsb3QsIHdpZHRoID0gXCIrd2lkdGgrXCIsIGhlaWdodCA9IFwiK2hlaWdodCl9dmFyIGVsZW1lbnQ9dGhpcy5lbGVtZW50LGNvbnRleHQ9dGhpcy5jb250ZXh0LHBpeGVsUmF0aW89dGhpcy5waXhlbFJhdGlvO2lmKHRoaXMud2lkdGghPXdpZHRoKXtlbGVtZW50LndpZHRoPXdpZHRoKnBpeGVsUmF0aW87ZWxlbWVudC5zdHlsZS53aWR0aD13aWR0aCtcInB4XCI7dGhpcy53aWR0aD13aWR0aH1pZih0aGlzLmhlaWdodCE9aGVpZ2h0KXtlbGVtZW50LmhlaWdodD1oZWlnaHQqcGl4ZWxSYXRpbztlbGVtZW50LnN0eWxlLmhlaWdodD1oZWlnaHQrXCJweFwiO3RoaXMuaGVpZ2h0PWhlaWdodH1jb250ZXh0LnJlc3RvcmUoKTtjb250ZXh0LnNhdmUoKTtjb250ZXh0LnNjYWxlKHBpeGVsUmF0aW8scGl4ZWxSYXRpbyl9O0NhbnZhcy5wcm90b3R5cGUuY2xlYXI9ZnVuY3Rpb24oKXt0aGlzLmNvbnRleHQuY2xlYXJSZWN0KDAsMCx0aGlzLndpZHRoLHRoaXMuaGVpZ2h0KX07Q2FudmFzLnByb3RvdHlwZS5yZW5kZXI9ZnVuY3Rpb24oKXt2YXIgY2FjaGU9dGhpcy5fdGV4dENhY2hlO2Zvcih2YXIgbGF5ZXJLZXkgaW4gY2FjaGUpe2lmKGhhc093blByb3BlcnR5LmNhbGwoY2FjaGUsbGF5ZXJLZXkpKXt2YXIgbGF5ZXI9dGhpcy5nZXRUZXh0TGF5ZXIobGF5ZXJLZXkpLGxheWVyQ2FjaGU9Y2FjaGVbbGF5ZXJLZXldO2xheWVyLmhpZGUoKTtmb3IodmFyIHN0eWxlS2V5IGluIGxheWVyQ2FjaGUpe2lmKGhhc093blByb3BlcnR5LmNhbGwobGF5ZXJDYWNoZSxzdHlsZUtleSkpe3ZhciBzdHlsZUNhY2hlPWxheWVyQ2FjaGVbc3R5bGVLZXldO2Zvcih2YXIga2V5IGluIHN0eWxlQ2FjaGUpe2lmKGhhc093blByb3BlcnR5LmNhbGwoc3R5bGVDYWNoZSxrZXkpKXt2YXIgcG9zaXRpb25zPXN0eWxlQ2FjaGVba2V5XS5wb3NpdGlvbnM7Zm9yKHZhciBpPTAscG9zaXRpb247cG9zaXRpb249cG9zaXRpb25zW2ldO2krKyl7aWYocG9zaXRpb24uYWN0aXZlKXtpZighcG9zaXRpb24ucmVuZGVyZWQpe2xheWVyLmFwcGVuZChwb3NpdGlvbi5lbGVtZW50KTtwb3NpdGlvbi5yZW5kZXJlZD10cnVlfX1lbHNle3Bvc2l0aW9ucy5zcGxpY2UoaS0tLDEpO2lmKHBvc2l0aW9uLnJlbmRlcmVkKXtwb3NpdGlvbi5lbGVtZW50LmRldGFjaCgpfX19aWYocG9zaXRpb25zLmxlbmd0aD09MCl7ZGVsZXRlIHN0eWxlQ2FjaGVba2V5XX19fX19bGF5ZXIuc2hvdygpfX19O0NhbnZhcy5wcm90b3R5cGUuZ2V0VGV4dExheWVyPWZ1bmN0aW9uKGNsYXNzZXMpe3ZhciBsYXllcj10aGlzLnRleHRbY2xhc3Nlc107aWYobGF5ZXI9PW51bGwpe2lmKHRoaXMudGV4dENvbnRhaW5lcj09bnVsbCl7dGhpcy50ZXh0Q29udGFpbmVyPSQoXCI8ZGl2IGNsYXNzPSdmbG90LXRleHQnPjwvZGl2PlwiKS5jc3Moe3Bvc2l0aW9uOlwiYWJzb2x1dGVcIix0b3A6MCxsZWZ0OjAsYm90dG9tOjAscmlnaHQ6MCxcImZvbnQtc2l6ZVwiOlwic21hbGxlclwiLGNvbG9yOlwiIzU0NTQ1NFwifSkuaW5zZXJ0QWZ0ZXIodGhpcy5lbGVtZW50KX1sYXllcj10aGlzLnRleHRbY2xhc3Nlc109JChcIjxkaXY+PC9kaXY+XCIpLmFkZENsYXNzKGNsYXNzZXMpLmNzcyh7cG9zaXRpb246XCJhYnNvbHV0ZVwiLHRvcDowLGxlZnQ6MCxib3R0b206MCxyaWdodDowfSkuYXBwZW5kVG8odGhpcy50ZXh0Q29udGFpbmVyKX1yZXR1cm4gbGF5ZXJ9O0NhbnZhcy5wcm90b3R5cGUuZ2V0VGV4dEluZm89ZnVuY3Rpb24obGF5ZXIsdGV4dCxmb250LGFuZ2xlLHdpZHRoKXt2YXIgdGV4dFN0eWxlLGxheWVyQ2FjaGUsc3R5bGVDYWNoZSxpbmZvO3RleHQ9XCJcIit0ZXh0O2lmKHR5cGVvZiBmb250PT09XCJvYmplY3RcIil7dGV4dFN0eWxlPWZvbnQuc3R5bGUrXCIgXCIrZm9udC52YXJpYW50K1wiIFwiK2ZvbnQud2VpZ2h0K1wiIFwiK2ZvbnQuc2l6ZStcInB4L1wiK2ZvbnQubGluZUhlaWdodCtcInB4IFwiK2ZvbnQuZmFtaWx5fWVsc2V7dGV4dFN0eWxlPWZvbnR9bGF5ZXJDYWNoZT10aGlzLl90ZXh0Q2FjaGVbbGF5ZXJdO2lmKGxheWVyQ2FjaGU9PW51bGwpe2xheWVyQ2FjaGU9dGhpcy5fdGV4dENhY2hlW2xheWVyXT17fX1zdHlsZUNhY2hlPWxheWVyQ2FjaGVbdGV4dFN0eWxlXTtpZihzdHlsZUNhY2hlPT1udWxsKXtzdHlsZUNhY2hlPWxheWVyQ2FjaGVbdGV4dFN0eWxlXT17fX1pbmZvPXN0eWxlQ2FjaGVbdGV4dF07aWYoaW5mbz09bnVsbCl7dmFyIGVsZW1lbnQ9JChcIjxkaXY+PC9kaXY+XCIpLmh0bWwodGV4dCkuY3NzKHtwb3NpdGlvbjpcImFic29sdXRlXCIsXCJtYXgtd2lkdGhcIjp3aWR0aCx0b3A6LTk5OTl9KS5hcHBlbmRUbyh0aGlzLmdldFRleHRMYXllcihsYXllcikpO2lmKHR5cGVvZiBmb250PT09XCJvYmplY3RcIil7ZWxlbWVudC5jc3Moe2ZvbnQ6dGV4dFN0eWxlLGNvbG9yOmZvbnQuY29sb3J9KX1lbHNlIGlmKHR5cGVvZiBmb250PT09XCJzdHJpbmdcIil7ZWxlbWVudC5hZGRDbGFzcyhmb250KX1pbmZvPXN0eWxlQ2FjaGVbdGV4dF09e3dpZHRoOmVsZW1lbnQub3V0ZXJXaWR0aCh0cnVlKSxoZWlnaHQ6ZWxlbWVudC5vdXRlckhlaWdodCh0cnVlKSxlbGVtZW50OmVsZW1lbnQscG9zaXRpb25zOltdfTtlbGVtZW50LmRldGFjaCgpfXJldHVybiBpbmZvfTtDYW52YXMucHJvdG90eXBlLmFkZFRleHQ9ZnVuY3Rpb24obGF5ZXIseCx5LHRleHQsZm9udCxhbmdsZSx3aWR0aCxoYWxpZ24sdmFsaWduKXt2YXIgaW5mbz10aGlzLmdldFRleHRJbmZvKGxheWVyLHRleHQsZm9udCxhbmdsZSx3aWR0aCkscG9zaXRpb25zPWluZm8ucG9zaXRpb25zO2lmKGhhbGlnbj09XCJjZW50ZXJcIil7eC09aW5mby53aWR0aC8yfWVsc2UgaWYoaGFsaWduPT1cInJpZ2h0XCIpe3gtPWluZm8ud2lkdGh9aWYodmFsaWduPT1cIm1pZGRsZVwiKXt5LT1pbmZvLmhlaWdodC8yfWVsc2UgaWYodmFsaWduPT1cImJvdHRvbVwiKXt5LT1pbmZvLmhlaWdodH1mb3IodmFyIGk9MCxwb3NpdGlvbjtwb3NpdGlvbj1wb3NpdGlvbnNbaV07aSsrKXtpZihwb3NpdGlvbi54PT14JiZwb3NpdGlvbi55PT15KXtwb3NpdGlvbi5hY3RpdmU9dHJ1ZTtyZXR1cm59fXBvc2l0aW9uPXthY3RpdmU6dHJ1ZSxyZW5kZXJlZDpmYWxzZSxlbGVtZW50OnBvc2l0aW9ucy5sZW5ndGg/aW5mby5lbGVtZW50LmNsb25lKCk6aW5mby5lbGVtZW50LHg6eCx5Onl9O3Bvc2l0aW9ucy5wdXNoKHBvc2l0aW9uKTtwb3NpdGlvbi5lbGVtZW50LmNzcyh7dG9wOk1hdGgucm91bmQoeSksbGVmdDpNYXRoLnJvdW5kKHgpLFwidGV4dC1hbGlnblwiOmhhbGlnbn0pfTtDYW52YXMucHJvdG90eXBlLnJlbW92ZVRleHQ9ZnVuY3Rpb24obGF5ZXIseCx5LHRleHQsZm9udCxhbmdsZSl7aWYodGV4dD09bnVsbCl7dmFyIGxheWVyQ2FjaGU9dGhpcy5fdGV4dENhY2hlW2xheWVyXTtpZihsYXllckNhY2hlIT1udWxsKXtmb3IodmFyIHN0eWxlS2V5IGluIGxheWVyQ2FjaGUpe2lmKGhhc093blByb3BlcnR5LmNhbGwobGF5ZXJDYWNoZSxzdHlsZUtleSkpe3ZhciBzdHlsZUNhY2hlPWxheWVyQ2FjaGVbc3R5bGVLZXldO2Zvcih2YXIga2V5IGluIHN0eWxlQ2FjaGUpe2lmKGhhc093blByb3BlcnR5LmNhbGwoc3R5bGVDYWNoZSxrZXkpKXt2YXIgcG9zaXRpb25zPXN0eWxlQ2FjaGVba2V5XS5wb3NpdGlvbnM7Zm9yKHZhciBpPTAscG9zaXRpb247cG9zaXRpb249cG9zaXRpb25zW2ldO2krKyl7cG9zaXRpb24uYWN0aXZlPWZhbHNlfX19fX19fWVsc2V7dmFyIHBvc2l0aW9ucz10aGlzLmdldFRleHRJbmZvKGxheWVyLHRleHQsZm9udCxhbmdsZSkucG9zaXRpb25zO2Zvcih2YXIgaT0wLHBvc2l0aW9uO3Bvc2l0aW9uPXBvc2l0aW9uc1tpXTtpKyspe2lmKHBvc2l0aW9uLng9PXgmJnBvc2l0aW9uLnk9PXkpe3Bvc2l0aW9uLmFjdGl2ZT1mYWxzZX19fX07ZnVuY3Rpb24gUGxvdChwbGFjZWhvbGRlcixkYXRhXyxvcHRpb25zXyxwbHVnaW5zKXt2YXIgc2VyaWVzPVtdLG9wdGlvbnM9e2NvbG9yczpbXCIjZWRjMjQwXCIsXCIjYWZkOGY4XCIsXCIjY2I0YjRiXCIsXCIjNGRhNzRkXCIsXCIjOTQ0MGVkXCJdLGxlZ2VuZDp7c2hvdzp0cnVlLG5vQ29sdW1uczoxLGxhYmVsRm9ybWF0dGVyOm51bGwsbGFiZWxCb3hCb3JkZXJDb2xvcjpcIiNjY2NcIixjb250YWluZXI6bnVsbCxwb3NpdGlvbjpcIm5lXCIsbWFyZ2luOjUsYmFja2dyb3VuZENvbG9yOm51bGwsYmFja2dyb3VuZE9wYWNpdHk6Ljg1LHNvcnRlZDpudWxsfSx4YXhpczp7c2hvdzpudWxsLHBvc2l0aW9uOlwiYm90dG9tXCIsbW9kZTpudWxsLGZvbnQ6bnVsbCxjb2xvcjpudWxsLHRpY2tDb2xvcjpudWxsLHRyYW5zZm9ybTpudWxsLGludmVyc2VUcmFuc2Zvcm06bnVsbCxtaW46bnVsbCxtYXg6bnVsbCxhdXRvc2NhbGVNYXJnaW46bnVsbCx0aWNrczpudWxsLHRpY2tGb3JtYXR0ZXI6bnVsbCxsYWJlbFdpZHRoOm51bGwsbGFiZWxIZWlnaHQ6bnVsbCxyZXNlcnZlU3BhY2U6bnVsbCx0aWNrTGVuZ3RoOm51bGwsYWxpZ25UaWNrc1dpdGhBeGlzOm51bGwsdGlja0RlY2ltYWxzOm51bGwsdGlja1NpemU6bnVsbCxtaW5UaWNrU2l6ZTpudWxsfSx5YXhpczp7YXV0b3NjYWxlTWFyZ2luOi4wMixwb3NpdGlvbjpcImxlZnRcIn0seGF4ZXM6W10seWF4ZXM6W10sc2VyaWVzOntwb2ludHM6e3Nob3c6ZmFsc2UscmFkaXVzOjMsbGluZVdpZHRoOjIsZmlsbDp0cnVlLGZpbGxDb2xvcjpcIiNmZmZmZmZcIixzeW1ib2w6XCJjaXJjbGVcIn0sbGluZXM6e2xpbmVXaWR0aDoyLGZpbGw6ZmFsc2UsZmlsbENvbG9yOm51bGwsc3RlcHM6ZmFsc2V9LGJhcnM6e3Nob3c6ZmFsc2UsbGluZVdpZHRoOjIsYmFyV2lkdGg6MSxmaWxsOnRydWUsZmlsbENvbG9yOm51bGwsYWxpZ246XCJsZWZ0XCIsaG9yaXpvbnRhbDpmYWxzZSx6ZXJvOnRydWV9LHNoYWRvd1NpemU6MyxoaWdobGlnaHRDb2xvcjpudWxsfSxncmlkOntzaG93OnRydWUsYWJvdmVEYXRhOmZhbHNlLGNvbG9yOlwiIzU0NTQ1NFwiLGJhY2tncm91bmRDb2xvcjpudWxsLGJvcmRlckNvbG9yOm51bGwsdGlja0NvbG9yOm51bGwsbWFyZ2luOjAsbGFiZWxNYXJnaW46NSxheGlzTWFyZ2luOjgsYm9yZGVyV2lkdGg6MixtaW5Cb3JkZXJNYXJnaW46bnVsbCxtYXJraW5nczpudWxsLG1hcmtpbmdzQ29sb3I6XCIjZjRmNGY0XCIsbWFya2luZ3NMaW5lV2lkdGg6MixjbGlja2FibGU6ZmFsc2UsaG92ZXJhYmxlOmZhbHNlLGF1dG9IaWdobGlnaHQ6dHJ1ZSxtb3VzZUFjdGl2ZVJhZGl1czoxMH0saW50ZXJhY3Rpb246e3JlZHJhd092ZXJsYXlJbnRlcnZhbDoxZTMvNjB9LGhvb2tzOnt9fSxzdXJmYWNlPW51bGwsb3ZlcmxheT1udWxsLGV2ZW50SG9sZGVyPW51bGwsY3R4PW51bGwsb2N0eD1udWxsLHhheGVzPVtdLHlheGVzPVtdLHBsb3RPZmZzZXQ9e2xlZnQ6MCxyaWdodDowLHRvcDowLGJvdHRvbTowfSxwbG90V2lkdGg9MCxwbG90SGVpZ2h0PTAsaG9va3M9e3Byb2Nlc3NPcHRpb25zOltdLHByb2Nlc3NSYXdEYXRhOltdLHByb2Nlc3NEYXRhcG9pbnRzOltdLHByb2Nlc3NPZmZzZXQ6W10sZHJhd0JhY2tncm91bmQ6W10sZHJhd1NlcmllczpbXSxkcmF3OltdLGJpbmRFdmVudHM6W10sZHJhd092ZXJsYXk6W10sc2h1dGRvd246W119LHBsb3Q9dGhpcztwbG90LnNldERhdGE9c2V0RGF0YTtwbG90LnNldHVwR3JpZD1zZXR1cEdyaWQ7cGxvdC5kcmF3PWRyYXc7cGxvdC5nZXRQbGFjZWhvbGRlcj1mdW5jdGlvbigpe3JldHVybiBwbGFjZWhvbGRlcn07cGxvdC5nZXRDYW52YXM9ZnVuY3Rpb24oKXtyZXR1cm4gc3VyZmFjZS5lbGVtZW50fTtwbG90LmdldFBsb3RPZmZzZXQ9ZnVuY3Rpb24oKXtyZXR1cm4gcGxvdE9mZnNldH07cGxvdC53aWR0aD1mdW5jdGlvbigpe3JldHVybiBwbG90V2lkdGh9O3Bsb3QuaGVpZ2h0PWZ1bmN0aW9uKCl7cmV0dXJuIHBsb3RIZWlnaHR9O3Bsb3Qub2Zmc2V0PWZ1bmN0aW9uKCl7dmFyIG89ZXZlbnRIb2xkZXIub2Zmc2V0KCk7by5sZWZ0Kz1wbG90T2Zmc2V0LmxlZnQ7by50b3ArPXBsb3RPZmZzZXQudG9wO3JldHVybiBvfTtwbG90LmdldERhdGE9ZnVuY3Rpb24oKXtyZXR1cm4gc2VyaWVzfTtwbG90LmdldEF4ZXM9ZnVuY3Rpb24oKXt2YXIgcmVzPXt9LGk7JC5lYWNoKHhheGVzLmNvbmNhdCh5YXhlcyksZnVuY3Rpb24oXyxheGlzKXtpZihheGlzKXJlc1theGlzLmRpcmVjdGlvbisoYXhpcy5uIT0xP2F4aXMubjpcIlwiKStcImF4aXNcIl09YXhpc30pO3JldHVybiByZXN9O3Bsb3QuZ2V0WEF4ZXM9ZnVuY3Rpb24oKXtyZXR1cm4geGF4ZXN9O3Bsb3QuZ2V0WUF4ZXM9ZnVuY3Rpb24oKXtyZXR1cm4geWF4ZXN9O3Bsb3QuYzJwPWNhbnZhc1RvQXhpc0Nvb3JkcztwbG90LnAyYz1heGlzVG9DYW52YXNDb29yZHM7cGxvdC5nZXRPcHRpb25zPWZ1bmN0aW9uKCl7cmV0dXJuIG9wdGlvbnN9O3Bsb3QuaGlnaGxpZ2h0PWhpZ2hsaWdodDtwbG90LnVuaGlnaGxpZ2h0PXVuaGlnaGxpZ2h0O3Bsb3QudHJpZ2dlclJlZHJhd092ZXJsYXk9dHJpZ2dlclJlZHJhd092ZXJsYXk7cGxvdC5wb2ludE9mZnNldD1mdW5jdGlvbihwb2ludCl7cmV0dXJue2xlZnQ6cGFyc2VJbnQoeGF4ZXNbYXhpc051bWJlcihwb2ludCxcInhcIiktMV0ucDJjKCtwb2ludC54KStwbG90T2Zmc2V0LmxlZnQsMTApLHRvcDpwYXJzZUludCh5YXhlc1theGlzTnVtYmVyKHBvaW50LFwieVwiKS0xXS5wMmMoK3BvaW50LnkpK3Bsb3RPZmZzZXQudG9wLDEwKX19O3Bsb3Quc2h1dGRvd249c2h1dGRvd247cGxvdC5kZXN0cm95PWZ1bmN0aW9uKCl7c2h1dGRvd24oKTtwbGFjZWhvbGRlci5yZW1vdmVEYXRhKFwicGxvdFwiKS5lbXB0eSgpO3Nlcmllcz1bXTtvcHRpb25zPW51bGw7c3VyZmFjZT1udWxsO292ZXJsYXk9bnVsbDtldmVudEhvbGRlcj1udWxsO2N0eD1udWxsO29jdHg9bnVsbDt4YXhlcz1bXTt5YXhlcz1bXTtob29rcz1udWxsO2hpZ2hsaWdodHM9W107cGxvdD1udWxsfTtwbG90LnJlc2l6ZT1mdW5jdGlvbigpe3ZhciB3aWR0aD1wbGFjZWhvbGRlci53aWR0aCgpLGhlaWdodD1wbGFjZWhvbGRlci5oZWlnaHQoKTtzdXJmYWNlLnJlc2l6ZSh3aWR0aCxoZWlnaHQpO292ZXJsYXkucmVzaXplKHdpZHRoLGhlaWdodCl9O3Bsb3QuaG9va3M9aG9va3M7aW5pdFBsdWdpbnMocGxvdCk7cGFyc2VPcHRpb25zKG9wdGlvbnNfKTtzZXR1cENhbnZhc2VzKCk7c2V0RGF0YShkYXRhXyk7c2V0dXBHcmlkKCk7ZHJhdygpO2JpbmRFdmVudHMoKTtmdW5jdGlvbiBleGVjdXRlSG9va3MoaG9vayxhcmdzKXthcmdzPVtwbG90XS5jb25jYXQoYXJncyk7Zm9yKHZhciBpPTA7aTxob29rLmxlbmd0aDsrK2kpaG9va1tpXS5hcHBseSh0aGlzLGFyZ3MpfWZ1bmN0aW9uIGluaXRQbHVnaW5zKCl7dmFyIGNsYXNzZXM9e0NhbnZhczpDYW52YXN9O2Zvcih2YXIgaT0wO2k8cGx1Z2lucy5sZW5ndGg7KytpKXt2YXIgcD1wbHVnaW5zW2ldO3AuaW5pdChwbG90LGNsYXNzZXMpO2lmKHAub3B0aW9ucykkLmV4dGVuZCh0cnVlLG9wdGlvbnMscC5vcHRpb25zKX19ZnVuY3Rpb24gcGFyc2VPcHRpb25zKG9wdHMpeyQuZXh0ZW5kKHRydWUsb3B0aW9ucyxvcHRzKTtpZihvcHRzJiZvcHRzLmNvbG9ycyl7b3B0aW9ucy5jb2xvcnM9b3B0cy5jb2xvcnN9aWYob3B0aW9ucy54YXhpcy5jb2xvcj09bnVsbClvcHRpb25zLnhheGlzLmNvbG9yPSQuY29sb3IucGFyc2Uob3B0aW9ucy5ncmlkLmNvbG9yKS5zY2FsZShcImFcIiwuMjIpLnRvU3RyaW5nKCk7aWYob3B0aW9ucy55YXhpcy5jb2xvcj09bnVsbClvcHRpb25zLnlheGlzLmNvbG9yPSQuY29sb3IucGFyc2Uob3B0aW9ucy5ncmlkLmNvbG9yKS5zY2FsZShcImFcIiwuMjIpLnRvU3RyaW5nKCk7aWYob3B0aW9ucy54YXhpcy50aWNrQ29sb3I9PW51bGwpb3B0aW9ucy54YXhpcy50aWNrQ29sb3I9b3B0aW9ucy5ncmlkLnRpY2tDb2xvcnx8b3B0aW9ucy54YXhpcy5jb2xvcjtpZihvcHRpb25zLnlheGlzLnRpY2tDb2xvcj09bnVsbClvcHRpb25zLnlheGlzLnRpY2tDb2xvcj1vcHRpb25zLmdyaWQudGlja0NvbG9yfHxvcHRpb25zLnlheGlzLmNvbG9yO2lmKG9wdGlvbnMuZ3JpZC5ib3JkZXJDb2xvcj09bnVsbClvcHRpb25zLmdyaWQuYm9yZGVyQ29sb3I9b3B0aW9ucy5ncmlkLmNvbG9yO2lmKG9wdGlvbnMuZ3JpZC50aWNrQ29sb3I9PW51bGwpb3B0aW9ucy5ncmlkLnRpY2tDb2xvcj0kLmNvbG9yLnBhcnNlKG9wdGlvbnMuZ3JpZC5jb2xvcikuc2NhbGUoXCJhXCIsLjIyKS50b1N0cmluZygpO3ZhciBpLGF4aXNPcHRpb25zLGF4aXNDb3VudCxmb250U2l6ZT1wbGFjZWhvbGRlci5jc3MoXCJmb250LXNpemVcIiksZm9udFNpemVEZWZhdWx0PWZvbnRTaXplPytmb250U2l6ZS5yZXBsYWNlKFwicHhcIixcIlwiKToxMyxmb250RGVmYXVsdHM9e3N0eWxlOnBsYWNlaG9sZGVyLmNzcyhcImZvbnQtc3R5bGVcIiksc2l6ZTpNYXRoLnJvdW5kKC44KmZvbnRTaXplRGVmYXVsdCksdmFyaWFudDpwbGFjZWhvbGRlci5jc3MoXCJmb250LXZhcmlhbnRcIiksd2VpZ2h0OnBsYWNlaG9sZGVyLmNzcyhcImZvbnQtd2VpZ2h0XCIpLGZhbWlseTpwbGFjZWhvbGRlci5jc3MoXCJmb250LWZhbWlseVwiKX07YXhpc0NvdW50PW9wdGlvbnMueGF4ZXMubGVuZ3RofHwxO2ZvcihpPTA7aTxheGlzQ291bnQ7KytpKXtheGlzT3B0aW9ucz1vcHRpb25zLnhheGVzW2ldO2lmKGF4aXNPcHRpb25zJiYhYXhpc09wdGlvbnMudGlja0NvbG9yKXtheGlzT3B0aW9ucy50aWNrQ29sb3I9YXhpc09wdGlvbnMuY29sb3J9YXhpc09wdGlvbnM9JC5leHRlbmQodHJ1ZSx7fSxvcHRpb25zLnhheGlzLGF4aXNPcHRpb25zKTtvcHRpb25zLnhheGVzW2ldPWF4aXNPcHRpb25zO2lmKGF4aXNPcHRpb25zLmZvbnQpe2F4aXNPcHRpb25zLmZvbnQ9JC5leHRlbmQoe30sZm9udERlZmF1bHRzLGF4aXNPcHRpb25zLmZvbnQpO2lmKCFheGlzT3B0aW9ucy5mb250LmNvbG9yKXtheGlzT3B0aW9ucy5mb250LmNvbG9yPWF4aXNPcHRpb25zLmNvbG9yfWlmKCFheGlzT3B0aW9ucy5mb250LmxpbmVIZWlnaHQpe2F4aXNPcHRpb25zLmZvbnQubGluZUhlaWdodD1NYXRoLnJvdW5kKGF4aXNPcHRpb25zLmZvbnQuc2l6ZSoxLjE1KX19fWF4aXNDb3VudD1vcHRpb25zLnlheGVzLmxlbmd0aHx8MTtmb3IoaT0wO2k8YXhpc0NvdW50OysraSl7YXhpc09wdGlvbnM9b3B0aW9ucy55YXhlc1tpXTtpZihheGlzT3B0aW9ucyYmIWF4aXNPcHRpb25zLnRpY2tDb2xvcil7YXhpc09wdGlvbnMudGlja0NvbG9yPWF4aXNPcHRpb25zLmNvbG9yfWF4aXNPcHRpb25zPSQuZXh0ZW5kKHRydWUse30sb3B0aW9ucy55YXhpcyxheGlzT3B0aW9ucyk7b3B0aW9ucy55YXhlc1tpXT1heGlzT3B0aW9ucztpZihheGlzT3B0aW9ucy5mb250KXtheGlzT3B0aW9ucy5mb250PSQuZXh0ZW5kKHt9LGZvbnREZWZhdWx0cyxheGlzT3B0aW9ucy5mb250KTtpZighYXhpc09wdGlvbnMuZm9udC5jb2xvcil7YXhpc09wdGlvbnMuZm9udC5jb2xvcj1heGlzT3B0aW9ucy5jb2xvcn1pZighYXhpc09wdGlvbnMuZm9udC5saW5lSGVpZ2h0KXtheGlzT3B0aW9ucy5mb250LmxpbmVIZWlnaHQ9TWF0aC5yb3VuZChheGlzT3B0aW9ucy5mb250LnNpemUqMS4xNSl9fX1pZihvcHRpb25zLnhheGlzLm5vVGlja3MmJm9wdGlvbnMueGF4aXMudGlja3M9PW51bGwpb3B0aW9ucy54YXhpcy50aWNrcz1vcHRpb25zLnhheGlzLm5vVGlja3M7aWYob3B0aW9ucy55YXhpcy5ub1RpY2tzJiZvcHRpb25zLnlheGlzLnRpY2tzPT1udWxsKW9wdGlvbnMueWF4aXMudGlja3M9b3B0aW9ucy55YXhpcy5ub1RpY2tzO2lmKG9wdGlvbnMueDJheGlzKXtvcHRpb25zLnhheGVzWzFdPSQuZXh0ZW5kKHRydWUse30sb3B0aW9ucy54YXhpcyxvcHRpb25zLngyYXhpcyk7b3B0aW9ucy54YXhlc1sxXS5wb3NpdGlvbj1cInRvcFwiO2lmKG9wdGlvbnMueDJheGlzLm1pbj09bnVsbCl7b3B0aW9ucy54YXhlc1sxXS5taW49bnVsbH1pZihvcHRpb25zLngyYXhpcy5tYXg9PW51bGwpe29wdGlvbnMueGF4ZXNbMV0ubWF4PW51bGx9fWlmKG9wdGlvbnMueTJheGlzKXtvcHRpb25zLnlheGVzWzFdPSQuZXh0ZW5kKHRydWUse30sb3B0aW9ucy55YXhpcyxvcHRpb25zLnkyYXhpcyk7b3B0aW9ucy55YXhlc1sxXS5wb3NpdGlvbj1cInJpZ2h0XCI7aWYob3B0aW9ucy55MmF4aXMubWluPT1udWxsKXtvcHRpb25zLnlheGVzWzFdLm1pbj1udWxsfWlmKG9wdGlvbnMueTJheGlzLm1heD09bnVsbCl7b3B0aW9ucy55YXhlc1sxXS5tYXg9bnVsbH19aWYob3B0aW9ucy5ncmlkLmNvbG9yZWRBcmVhcylvcHRpb25zLmdyaWQubWFya2luZ3M9b3B0aW9ucy5ncmlkLmNvbG9yZWRBcmVhcztpZihvcHRpb25zLmdyaWQuY29sb3JlZEFyZWFzQ29sb3Ipb3B0aW9ucy5ncmlkLm1hcmtpbmdzQ29sb3I9b3B0aW9ucy5ncmlkLmNvbG9yZWRBcmVhc0NvbG9yO2lmKG9wdGlvbnMubGluZXMpJC5leHRlbmQodHJ1ZSxvcHRpb25zLnNlcmllcy5saW5lcyxvcHRpb25zLmxpbmVzKTtpZihvcHRpb25zLnBvaW50cykkLmV4dGVuZCh0cnVlLG9wdGlvbnMuc2VyaWVzLnBvaW50cyxvcHRpb25zLnBvaW50cyk7aWYob3B0aW9ucy5iYXJzKSQuZXh0ZW5kKHRydWUsb3B0aW9ucy5zZXJpZXMuYmFycyxvcHRpb25zLmJhcnMpO2lmKG9wdGlvbnMuc2hhZG93U2l6ZSE9bnVsbClvcHRpb25zLnNlcmllcy5zaGFkb3dTaXplPW9wdGlvbnMuc2hhZG93U2l6ZTtpZihvcHRpb25zLmhpZ2hsaWdodENvbG9yIT1udWxsKW9wdGlvbnMuc2VyaWVzLmhpZ2hsaWdodENvbG9yPW9wdGlvbnMuaGlnaGxpZ2h0Q29sb3I7Zm9yKGk9MDtpPG9wdGlvbnMueGF4ZXMubGVuZ3RoOysraSlnZXRPckNyZWF0ZUF4aXMoeGF4ZXMsaSsxKS5vcHRpb25zPW9wdGlvbnMueGF4ZXNbaV07Zm9yKGk9MDtpPG9wdGlvbnMueWF4ZXMubGVuZ3RoOysraSlnZXRPckNyZWF0ZUF4aXMoeWF4ZXMsaSsxKS5vcHRpb25zPW9wdGlvbnMueWF4ZXNbaV07Zm9yKHZhciBuIGluIGhvb2tzKWlmKG9wdGlvbnMuaG9va3Nbbl0mJm9wdGlvbnMuaG9va3Nbbl0ubGVuZ3RoKWhvb2tzW25dPWhvb2tzW25dLmNvbmNhdChvcHRpb25zLmhvb2tzW25dKTtleGVjdXRlSG9va3MoaG9va3MucHJvY2Vzc09wdGlvbnMsW29wdGlvbnNdKX1mdW5jdGlvbiBzZXREYXRhKGQpe3Nlcmllcz1wYXJzZURhdGEoZCk7ZmlsbEluU2VyaWVzT3B0aW9ucygpO3Byb2Nlc3NEYXRhKCl9ZnVuY3Rpb24gcGFyc2VEYXRhKGQpe3ZhciByZXM9W107Zm9yKHZhciBpPTA7aTxkLmxlbmd0aDsrK2kpe3ZhciBzPSQuZXh0ZW5kKHRydWUse30sb3B0aW9ucy5zZXJpZXMpO2lmKGRbaV0uZGF0YSE9bnVsbCl7cy5kYXRhPWRbaV0uZGF0YTtkZWxldGUgZFtpXS5kYXRhOyQuZXh0ZW5kKHRydWUscyxkW2ldKTtkW2ldLmRhdGE9cy5kYXRhfWVsc2Ugcy5kYXRhPWRbaV07cmVzLnB1c2gocyl9cmV0dXJuIHJlc31mdW5jdGlvbiBheGlzTnVtYmVyKG9iaixjb29yZCl7dmFyIGE9b2JqW2Nvb3JkK1wiYXhpc1wiXTtpZih0eXBlb2YgYT09XCJvYmplY3RcIilhPWEubjtpZih0eXBlb2YgYSE9XCJudW1iZXJcIilhPTE7cmV0dXJuIGF9ZnVuY3Rpb24gYWxsQXhlcygpe3JldHVybiAkLmdyZXAoeGF4ZXMuY29uY2F0KHlheGVzKSxmdW5jdGlvbihhKXtyZXR1cm4gYX0pfWZ1bmN0aW9uIGNhbnZhc1RvQXhpc0Nvb3Jkcyhwb3Mpe3ZhciByZXM9e30saSxheGlzO2ZvcihpPTA7aTx4YXhlcy5sZW5ndGg7KytpKXtheGlzPXhheGVzW2ldO2lmKGF4aXMmJmF4aXMudXNlZClyZXNbXCJ4XCIrYXhpcy5uXT1heGlzLmMycChwb3MubGVmdCl9Zm9yKGk9MDtpPHlheGVzLmxlbmd0aDsrK2kpe2F4aXM9eWF4ZXNbaV07aWYoYXhpcyYmYXhpcy51c2VkKXJlc1tcInlcIitheGlzLm5dPWF4aXMuYzJwKHBvcy50b3ApfWlmKHJlcy54MSE9PXVuZGVmaW5lZClyZXMueD1yZXMueDE7aWYocmVzLnkxIT09dW5kZWZpbmVkKXJlcy55PXJlcy55MTtyZXR1cm4gcmVzfWZ1bmN0aW9uIGF4aXNUb0NhbnZhc0Nvb3Jkcyhwb3Mpe3ZhciByZXM9e30saSxheGlzLGtleTtmb3IoaT0wO2k8eGF4ZXMubGVuZ3RoOysraSl7YXhpcz14YXhlc1tpXTtpZihheGlzJiZheGlzLnVzZWQpe2tleT1cInhcIitheGlzLm47aWYocG9zW2tleV09PW51bGwmJmF4aXMubj09MSlrZXk9XCJ4XCI7aWYocG9zW2tleV0hPW51bGwpe3Jlcy5sZWZ0PWF4aXMucDJjKHBvc1trZXldKTticmVha319fWZvcihpPTA7aTx5YXhlcy5sZW5ndGg7KytpKXtheGlzPXlheGVzW2ldO2lmKGF4aXMmJmF4aXMudXNlZCl7a2V5PVwieVwiK2F4aXMubjtpZihwb3Nba2V5XT09bnVsbCYmYXhpcy5uPT0xKWtleT1cInlcIjtpZihwb3Nba2V5XSE9bnVsbCl7cmVzLnRvcD1heGlzLnAyYyhwb3Nba2V5XSk7YnJlYWt9fX1yZXR1cm4gcmVzfWZ1bmN0aW9uIGdldE9yQ3JlYXRlQXhpcyhheGVzLG51bWJlcil7aWYoIWF4ZXNbbnVtYmVyLTFdKWF4ZXNbbnVtYmVyLTFdPXtuOm51bWJlcixkaXJlY3Rpb246YXhlcz09eGF4ZXM/XCJ4XCI6XCJ5XCIsb3B0aW9uczokLmV4dGVuZCh0cnVlLHt9LGF4ZXM9PXhheGVzP29wdGlvbnMueGF4aXM6b3B0aW9ucy55YXhpcyl9O3JldHVybiBheGVzW251bWJlci0xXX1mdW5jdGlvbiBmaWxsSW5TZXJpZXNPcHRpb25zKCl7dmFyIG5lZWRlZENvbG9ycz1zZXJpZXMubGVuZ3RoLG1heEluZGV4PS0xLGk7Zm9yKGk9MDtpPHNlcmllcy5sZW5ndGg7KytpKXt2YXIgc2M9c2VyaWVzW2ldLmNvbG9yO2lmKHNjIT1udWxsKXtuZWVkZWRDb2xvcnMtLTtpZih0eXBlb2Ygc2M9PVwibnVtYmVyXCImJnNjPm1heEluZGV4KXttYXhJbmRleD1zY319fWlmKG5lZWRlZENvbG9yczw9bWF4SW5kZXgpe25lZWRlZENvbG9ycz1tYXhJbmRleCsxfXZhciBjLGNvbG9ycz1bXSxjb2xvclBvb2w9b3B0aW9ucy5jb2xvcnMsY29sb3JQb29sU2l6ZT1jb2xvclBvb2wubGVuZ3RoLHZhcmlhdGlvbj0wO2ZvcihpPTA7aTxuZWVkZWRDb2xvcnM7aSsrKXtjPSQuY29sb3IucGFyc2UoY29sb3JQb29sW2klY29sb3JQb29sU2l6ZV18fFwiIzY2NlwiKTtpZihpJWNvbG9yUG9vbFNpemU9PTAmJmkpe2lmKHZhcmlhdGlvbj49MCl7aWYodmFyaWF0aW9uPC41KXt2YXJpYXRpb249LXZhcmlhdGlvbi0uMn1lbHNlIHZhcmlhdGlvbj0wfWVsc2UgdmFyaWF0aW9uPS12YXJpYXRpb259Y29sb3JzW2ldPWMuc2NhbGUoXCJyZ2JcIiwxK3ZhcmlhdGlvbil9dmFyIGNvbG9yaT0wLHM7Zm9yKGk9MDtpPHNlcmllcy5sZW5ndGg7KytpKXtzPXNlcmllc1tpXTtpZihzLmNvbG9yPT1udWxsKXtzLmNvbG9yPWNvbG9yc1tjb2xvcmldLnRvU3RyaW5nKCk7Kytjb2xvcml9ZWxzZSBpZih0eXBlb2Ygcy5jb2xvcj09XCJudW1iZXJcIilzLmNvbG9yPWNvbG9yc1tzLmNvbG9yXS50b1N0cmluZygpO2lmKHMubGluZXMuc2hvdz09bnVsbCl7dmFyIHYsc2hvdz10cnVlO2Zvcih2IGluIHMpaWYoc1t2XSYmc1t2XS5zaG93KXtzaG93PWZhbHNlO2JyZWFrfWlmKHNob3cpcy5saW5lcy5zaG93PXRydWV9aWYocy5saW5lcy56ZXJvPT1udWxsKXtzLmxpbmVzLnplcm89ISFzLmxpbmVzLmZpbGx9cy54YXhpcz1nZXRPckNyZWF0ZUF4aXMoeGF4ZXMsYXhpc051bWJlcihzLFwieFwiKSk7cy55YXhpcz1nZXRPckNyZWF0ZUF4aXMoeWF4ZXMsYXhpc051bWJlcihzLFwieVwiKSl9fWZ1bmN0aW9uIHByb2Nlc3NEYXRhKCl7dmFyIHRvcFNlbnRyeT1OdW1iZXIuUE9TSVRJVkVfSU5GSU5JVFksYm90dG9tU2VudHJ5PU51bWJlci5ORUdBVElWRV9JTkZJTklUWSxmYWtlSW5maW5pdHk9TnVtYmVyLk1BWF9WQUxVRSxpLGosayxtLGxlbmd0aCxzLHBvaW50cyxwcyx4LHksYXhpcyx2YWwsZixwLGRhdGEsZm9ybWF0O2Z1bmN0aW9uIHVwZGF0ZUF4aXMoYXhpcyxtaW4sbWF4KXtpZihtaW48YXhpcy5kYXRhbWluJiZtaW4hPS1mYWtlSW5maW5pdHkpYXhpcy5kYXRhbWluPW1pbjtpZihtYXg+YXhpcy5kYXRhbWF4JiZtYXghPWZha2VJbmZpbml0eSlheGlzLmRhdGFtYXg9bWF4fSQuZWFjaChhbGxBeGVzKCksZnVuY3Rpb24oXyxheGlzKXtheGlzLmRhdGFtaW49dG9wU2VudHJ5O2F4aXMuZGF0YW1heD1ib3R0b21TZW50cnk7YXhpcy51c2VkPWZhbHNlfSk7Zm9yKGk9MDtpPHNlcmllcy5sZW5ndGg7KytpKXtzPXNlcmllc1tpXTtzLmRhdGFwb2ludHM9e3BvaW50czpbXX07ZXhlY3V0ZUhvb2tzKGhvb2tzLnByb2Nlc3NSYXdEYXRhLFtzLHMuZGF0YSxzLmRhdGFwb2ludHNdKX1mb3IoaT0wO2k8c2VyaWVzLmxlbmd0aDsrK2kpe3M9c2VyaWVzW2ldO2RhdGE9cy5kYXRhO2Zvcm1hdD1zLmRhdGFwb2ludHMuZm9ybWF0O2lmKCFmb3JtYXQpe2Zvcm1hdD1bXTtmb3JtYXQucHVzaCh7eDp0cnVlLG51bWJlcjp0cnVlLHJlcXVpcmVkOnRydWV9KTtmb3JtYXQucHVzaCh7eTp0cnVlLG51bWJlcjp0cnVlLHJlcXVpcmVkOnRydWV9KTtpZihzLmJhcnMuc2hvd3x8cy5saW5lcy5zaG93JiZzLmxpbmVzLmZpbGwpe3ZhciBhdXRvc2NhbGU9ISEocy5iYXJzLnNob3cmJnMuYmFycy56ZXJvfHxzLmxpbmVzLnNob3cmJnMubGluZXMuemVybyk7Zm9ybWF0LnB1c2goe3k6dHJ1ZSxudW1iZXI6dHJ1ZSxyZXF1aXJlZDpmYWxzZSxkZWZhdWx0VmFsdWU6MCxhdXRvc2NhbGU6YXV0b3NjYWxlfSk7aWYocy5iYXJzLmhvcml6b250YWwpe2RlbGV0ZSBmb3JtYXRbZm9ybWF0Lmxlbmd0aC0xXS55O2Zvcm1hdFtmb3JtYXQubGVuZ3RoLTFdLng9dHJ1ZX19cy5kYXRhcG9pbnRzLmZvcm1hdD1mb3JtYXR9aWYocy5kYXRhcG9pbnRzLnBvaW50c2l6ZSE9bnVsbCljb250aW51ZTtzLmRhdGFwb2ludHMucG9pbnRzaXplPWZvcm1hdC5sZW5ndGg7cHM9cy5kYXRhcG9pbnRzLnBvaW50c2l6ZTtwb2ludHM9cy5kYXRhcG9pbnRzLnBvaW50czt2YXIgaW5zZXJ0U3RlcHM9cy5saW5lcy5zaG93JiZzLmxpbmVzLnN0ZXBzO3MueGF4aXMudXNlZD1zLnlheGlzLnVzZWQ9dHJ1ZTtmb3Ioaj1rPTA7ajxkYXRhLmxlbmd0aDsrK2osays9cHMpe3A9ZGF0YVtqXTt2YXIgbnVsbGlmeT1wPT1udWxsO2lmKCFudWxsaWZ5KXtmb3IobT0wO208cHM7KyttKXt2YWw9cFttXTtmPWZvcm1hdFttXTtpZihmKXtpZihmLm51bWJlciYmdmFsIT1udWxsKXt2YWw9K3ZhbDtpZihpc05hTih2YWwpKXZhbD1udWxsO2Vsc2UgaWYodmFsPT1JbmZpbml0eSl2YWw9ZmFrZUluZmluaXR5O2Vsc2UgaWYodmFsPT0tSW5maW5pdHkpdmFsPS1mYWtlSW5maW5pdHl9aWYodmFsPT1udWxsKXtpZihmLnJlcXVpcmVkKW51bGxpZnk9dHJ1ZTtpZihmLmRlZmF1bHRWYWx1ZSE9bnVsbCl2YWw9Zi5kZWZhdWx0VmFsdWV9fXBvaW50c1trK21dPXZhbH19aWYobnVsbGlmeSl7Zm9yKG09MDttPHBzOysrbSl7dmFsPXBvaW50c1trK21dO2lmKHZhbCE9bnVsbCl7Zj1mb3JtYXRbbV07aWYoZi5hdXRvc2NhbGUhPT1mYWxzZSl7aWYoZi54KXt1cGRhdGVBeGlzKHMueGF4aXMsdmFsLHZhbCl9aWYoZi55KXt1cGRhdGVBeGlzKHMueWF4aXMsdmFsLHZhbCl9fX1wb2ludHNbayttXT1udWxsfX1lbHNle2lmKGluc2VydFN0ZXBzJiZrPjAmJnBvaW50c1trLXBzXSE9bnVsbCYmcG9pbnRzW2stcHNdIT1wb2ludHNba10mJnBvaW50c1trLXBzKzFdIT1wb2ludHNbaysxXSl7Zm9yKG09MDttPHBzOysrbSlwb2ludHNbaytwcyttXT1wb2ludHNbayttXTtwb2ludHNbaysxXT1wb2ludHNbay1wcysxXTtrKz1wc319fX1mb3IoaT0wO2k8c2VyaWVzLmxlbmd0aDsrK2kpe3M9c2VyaWVzW2ldO2V4ZWN1dGVIb29rcyhob29rcy5wcm9jZXNzRGF0YXBvaW50cyxbcyxzLmRhdGFwb2ludHNdKX1mb3IoaT0wO2k8c2VyaWVzLmxlbmd0aDsrK2kpe3M9c2VyaWVzW2ldO3BvaW50cz1zLmRhdGFwb2ludHMucG9pbnRzO3BzPXMuZGF0YXBvaW50cy5wb2ludHNpemU7Zm9ybWF0PXMuZGF0YXBvaW50cy5mb3JtYXQ7dmFyIHhtaW49dG9wU2VudHJ5LHltaW49dG9wU2VudHJ5LHhtYXg9Ym90dG9tU2VudHJ5LHltYXg9Ym90dG9tU2VudHJ5O2ZvcihqPTA7ajxwb2ludHMubGVuZ3RoO2orPXBzKXtpZihwb2ludHNbal09PW51bGwpY29udGludWU7Zm9yKG09MDttPHBzOysrbSl7dmFsPXBvaW50c1tqK21dO2Y9Zm9ybWF0W21dO2lmKCFmfHxmLmF1dG9zY2FsZT09PWZhbHNlfHx2YWw9PWZha2VJbmZpbml0eXx8dmFsPT0tZmFrZUluZmluaXR5KWNvbnRpbnVlO2lmKGYueCl7aWYodmFsPHhtaW4peG1pbj12YWw7aWYodmFsPnhtYXgpeG1heD12YWx9aWYoZi55KXtpZih2YWw8eW1pbil5bWluPXZhbDtpZih2YWw+eW1heCl5bWF4PXZhbH19fWlmKHMuYmFycy5zaG93KXt2YXIgZGVsdGE7c3dpdGNoKHMuYmFycy5hbGlnbil7Y2FzZVwibGVmdFwiOmRlbHRhPTA7YnJlYWs7Y2FzZVwicmlnaHRcIjpkZWx0YT0tcy5iYXJzLmJhcldpZHRoO2JyZWFrO2RlZmF1bHQ6ZGVsdGE9LXMuYmFycy5iYXJXaWR0aC8yfWlmKHMuYmFycy5ob3Jpem9udGFsKXt5bWluKz1kZWx0YTt5bWF4Kz1kZWx0YStzLmJhcnMuYmFyV2lkdGh9ZWxzZXt4bWluKz1kZWx0YTt4bWF4Kz1kZWx0YStzLmJhcnMuYmFyV2lkdGh9fXVwZGF0ZUF4aXMocy54YXhpcyx4bWluLHhtYXgpO3VwZGF0ZUF4aXMocy55YXhpcyx5bWluLHltYXgpfSQuZWFjaChhbGxBeGVzKCksZnVuY3Rpb24oXyxheGlzKXtpZihheGlzLmRhdGFtaW49PXRvcFNlbnRyeSlheGlzLmRhdGFtaW49bnVsbDtpZihheGlzLmRhdGFtYXg9PWJvdHRvbVNlbnRyeSlheGlzLmRhdGFtYXg9bnVsbH0pfWZ1bmN0aW9uIHNldHVwQ2FudmFzZXMoKXtwbGFjZWhvbGRlci5jc3MoXCJwYWRkaW5nXCIsMCkuY2hpbGRyZW4oKS5maWx0ZXIoZnVuY3Rpb24oKXtyZXR1cm4hJCh0aGlzKS5oYXNDbGFzcyhcImZsb3Qtb3ZlcmxheVwiKSYmISQodGhpcykuaGFzQ2xhc3MoXCJmbG90LWJhc2VcIil9KS5yZW1vdmUoKTtpZihwbGFjZWhvbGRlci5jc3MoXCJwb3NpdGlvblwiKT09XCJzdGF0aWNcIilwbGFjZWhvbGRlci5jc3MoXCJwb3NpdGlvblwiLFwicmVsYXRpdmVcIik7c3VyZmFjZT1uZXcgQ2FudmFzKFwiZmxvdC1iYXNlXCIscGxhY2Vob2xkZXIpO292ZXJsYXk9bmV3IENhbnZhcyhcImZsb3Qtb3ZlcmxheVwiLHBsYWNlaG9sZGVyKTtjdHg9c3VyZmFjZS5jb250ZXh0O29jdHg9b3ZlcmxheS5jb250ZXh0O2V2ZW50SG9sZGVyPSQob3ZlcmxheS5lbGVtZW50KS51bmJpbmQoKTt2YXIgZXhpc3Rpbmc9cGxhY2Vob2xkZXIuZGF0YShcInBsb3RcIik7aWYoZXhpc3Rpbmcpe2V4aXN0aW5nLnNodXRkb3duKCk7b3ZlcmxheS5jbGVhcigpfXBsYWNlaG9sZGVyLmRhdGEoXCJwbG90XCIscGxvdCl9ZnVuY3Rpb24gYmluZEV2ZW50cygpe2lmKG9wdGlvbnMuZ3JpZC5ob3ZlcmFibGUpe2V2ZW50SG9sZGVyLm1vdXNlbW92ZShvbk1vdXNlTW92ZSk7ZXZlbnRIb2xkZXIuYmluZChcIm1vdXNlbGVhdmVcIixvbk1vdXNlTGVhdmUpfWlmKG9wdGlvbnMuZ3JpZC5jbGlja2FibGUpZXZlbnRIb2xkZXIuY2xpY2sob25DbGljayk7ZXhlY3V0ZUhvb2tzKGhvb2tzLmJpbmRFdmVudHMsW2V2ZW50SG9sZGVyXSl9ZnVuY3Rpb24gc2h1dGRvd24oKXtpZihyZWRyYXdUaW1lb3V0KWNsZWFyVGltZW91dChyZWRyYXdUaW1lb3V0KTtldmVudEhvbGRlci51bmJpbmQoXCJtb3VzZW1vdmVcIixvbk1vdXNlTW92ZSk7ZXZlbnRIb2xkZXIudW5iaW5kKFwibW91c2VsZWF2ZVwiLG9uTW91c2VMZWF2ZSk7ZXZlbnRIb2xkZXIudW5iaW5kKFwiY2xpY2tcIixvbkNsaWNrKTtleGVjdXRlSG9va3MoaG9va3Muc2h1dGRvd24sW2V2ZW50SG9sZGVyXSl9ZnVuY3Rpb24gc2V0VHJhbnNmb3JtYXRpb25IZWxwZXJzKGF4aXMpe2Z1bmN0aW9uIGlkZW50aXR5KHgpe3JldHVybiB4fXZhciBzLG0sdD1heGlzLm9wdGlvbnMudHJhbnNmb3JtfHxpZGVudGl0eSxpdD1heGlzLm9wdGlvbnMuaW52ZXJzZVRyYW5zZm9ybTtpZihheGlzLmRpcmVjdGlvbj09XCJ4XCIpe3M9YXhpcy5zY2FsZT1wbG90V2lkdGgvTWF0aC5hYnModChheGlzLm1heCktdChheGlzLm1pbikpO209TWF0aC5taW4odChheGlzLm1heCksdChheGlzLm1pbikpfWVsc2V7cz1heGlzLnNjYWxlPXBsb3RIZWlnaHQvTWF0aC5hYnModChheGlzLm1heCktdChheGlzLm1pbikpO3M9LXM7bT1NYXRoLm1heCh0KGF4aXMubWF4KSx0KGF4aXMubWluKSl9aWYodD09aWRlbnRpdHkpYXhpcy5wMmM9ZnVuY3Rpb24ocCl7cmV0dXJuKHAtbSkqc307ZWxzZSBheGlzLnAyYz1mdW5jdGlvbihwKXtyZXR1cm4odChwKS1tKSpzfTtpZighaXQpYXhpcy5jMnA9ZnVuY3Rpb24oYyl7cmV0dXJuIG0rYy9zfTtlbHNlIGF4aXMuYzJwPWZ1bmN0aW9uKGMpe3JldHVybiBpdChtK2Mvcyl9fWZ1bmN0aW9uIG1lYXN1cmVUaWNrTGFiZWxzKGF4aXMpe3ZhciBvcHRzPWF4aXMub3B0aW9ucyx0aWNrcz1heGlzLnRpY2tzfHxbXSxsYWJlbFdpZHRoPW9wdHMubGFiZWxXaWR0aHx8MCxsYWJlbEhlaWdodD1vcHRzLmxhYmVsSGVpZ2h0fHwwLG1heFdpZHRoPWxhYmVsV2lkdGh8fChheGlzLmRpcmVjdGlvbj09XCJ4XCI/TWF0aC5mbG9vcihzdXJmYWNlLndpZHRoLyh0aWNrcy5sZW5ndGh8fDEpKTpudWxsKSxsZWdhY3lTdHlsZXM9YXhpcy5kaXJlY3Rpb24rXCJBeGlzIFwiK2F4aXMuZGlyZWN0aW9uK2F4aXMubitcIkF4aXNcIixsYXllcj1cImZsb3QtXCIrYXhpcy5kaXJlY3Rpb24rXCItYXhpcyBmbG90LVwiK2F4aXMuZGlyZWN0aW9uK2F4aXMubitcIi1heGlzIFwiK2xlZ2FjeVN0eWxlcyxmb250PW9wdHMuZm9udHx8XCJmbG90LXRpY2stbGFiZWwgdGlja0xhYmVsXCI7Zm9yKHZhciBpPTA7aTx0aWNrcy5sZW5ndGg7KytpKXt2YXIgdD10aWNrc1tpXTtpZighdC5sYWJlbCljb250aW51ZTt2YXIgaW5mbz1zdXJmYWNlLmdldFRleHRJbmZvKGxheWVyLHQubGFiZWwsZm9udCxudWxsLG1heFdpZHRoKTtsYWJlbFdpZHRoPU1hdGgubWF4KGxhYmVsV2lkdGgsaW5mby53aWR0aCk7bGFiZWxIZWlnaHQ9TWF0aC5tYXgobGFiZWxIZWlnaHQsaW5mby5oZWlnaHQpfWF4aXMubGFiZWxXaWR0aD1vcHRzLmxhYmVsV2lkdGh8fGxhYmVsV2lkdGg7YXhpcy5sYWJlbEhlaWdodD1vcHRzLmxhYmVsSGVpZ2h0fHxsYWJlbEhlaWdodH1mdW5jdGlvbiBhbGxvY2F0ZUF4aXNCb3hGaXJzdFBoYXNlKGF4aXMpe3ZhciBsdz1heGlzLmxhYmVsV2lkdGgsbGg9YXhpcy5sYWJlbEhlaWdodCxwb3M9YXhpcy5vcHRpb25zLnBvc2l0aW9uLGlzWEF4aXM9YXhpcy5kaXJlY3Rpb249PT1cInhcIix0aWNrTGVuZ3RoPWF4aXMub3B0aW9ucy50aWNrTGVuZ3RoLGF4aXNNYXJnaW49b3B0aW9ucy5ncmlkLmF4aXNNYXJnaW4scGFkZGluZz1vcHRpb25zLmdyaWQubGFiZWxNYXJnaW4saW5uZXJtb3N0PXRydWUsb3V0ZXJtb3N0PXRydWUsZmlyc3Q9dHJ1ZSxmb3VuZD1mYWxzZTskLmVhY2goaXNYQXhpcz94YXhlczp5YXhlcyxmdW5jdGlvbihpLGEpe2lmKGEmJihhLnNob3d8fGEucmVzZXJ2ZVNwYWNlKSl7aWYoYT09PWF4aXMpe2ZvdW5kPXRydWV9ZWxzZSBpZihhLm9wdGlvbnMucG9zaXRpb249PT1wb3Mpe2lmKGZvdW5kKXtvdXRlcm1vc3Q9ZmFsc2V9ZWxzZXtpbm5lcm1vc3Q9ZmFsc2V9fWlmKCFmb3VuZCl7Zmlyc3Q9ZmFsc2V9fX0pO2lmKG91dGVybW9zdCl7YXhpc01hcmdpbj0wfWlmKHRpY2tMZW5ndGg9PW51bGwpe3RpY2tMZW5ndGg9Zmlyc3Q/XCJmdWxsXCI6NX1pZighaXNOYU4oK3RpY2tMZW5ndGgpKXBhZGRpbmcrPSt0aWNrTGVuZ3RoO2lmKGlzWEF4aXMpe2xoKz1wYWRkaW5nO2lmKHBvcz09XCJib3R0b21cIil7cGxvdE9mZnNldC5ib3R0b20rPWxoK2F4aXNNYXJnaW47YXhpcy5ib3g9e3RvcDpzdXJmYWNlLmhlaWdodC1wbG90T2Zmc2V0LmJvdHRvbSxoZWlnaHQ6bGh9fWVsc2V7YXhpcy5ib3g9e3RvcDpwbG90T2Zmc2V0LnRvcCtheGlzTWFyZ2luLGhlaWdodDpsaH07cGxvdE9mZnNldC50b3ArPWxoK2F4aXNNYXJnaW59fWVsc2V7bHcrPXBhZGRpbmc7aWYocG9zPT1cImxlZnRcIil7YXhpcy5ib3g9e2xlZnQ6cGxvdE9mZnNldC5sZWZ0K2F4aXNNYXJnaW4sd2lkdGg6bHd9O3Bsb3RPZmZzZXQubGVmdCs9bHcrYXhpc01hcmdpbn1lbHNle3Bsb3RPZmZzZXQucmlnaHQrPWx3K2F4aXNNYXJnaW47YXhpcy5ib3g9e2xlZnQ6c3VyZmFjZS53aWR0aC1wbG90T2Zmc2V0LnJpZ2h0LHdpZHRoOmx3fX19YXhpcy5wb3NpdGlvbj1wb3M7YXhpcy50aWNrTGVuZ3RoPXRpY2tMZW5ndGg7YXhpcy5ib3gucGFkZGluZz1wYWRkaW5nO2F4aXMuaW5uZXJtb3N0PWlubmVybW9zdH1mdW5jdGlvbiBhbGxvY2F0ZUF4aXNCb3hTZWNvbmRQaGFzZShheGlzKXtpZihheGlzLmRpcmVjdGlvbj09XCJ4XCIpe2F4aXMuYm94LmxlZnQ9cGxvdE9mZnNldC5sZWZ0LWF4aXMubGFiZWxXaWR0aC8yO2F4aXMuYm94LndpZHRoPXN1cmZhY2Uud2lkdGgtcGxvdE9mZnNldC5sZWZ0LXBsb3RPZmZzZXQucmlnaHQrYXhpcy5sYWJlbFdpZHRofWVsc2V7YXhpcy5ib3gudG9wPXBsb3RPZmZzZXQudG9wLWF4aXMubGFiZWxIZWlnaHQvMjtheGlzLmJveC5oZWlnaHQ9c3VyZmFjZS5oZWlnaHQtcGxvdE9mZnNldC5ib3R0b20tcGxvdE9mZnNldC50b3ArYXhpcy5sYWJlbEhlaWdodH19ZnVuY3Rpb24gYWRqdXN0TGF5b3V0Rm9yVGhpbmdzU3RpY2tpbmdPdXQoKXt2YXIgbWluTWFyZ2luPW9wdGlvbnMuZ3JpZC5taW5Cb3JkZXJNYXJnaW4sYXhpcyxpO2lmKG1pbk1hcmdpbj09bnVsbCl7bWluTWFyZ2luPTA7Zm9yKGk9MDtpPHNlcmllcy5sZW5ndGg7KytpKW1pbk1hcmdpbj1NYXRoLm1heChtaW5NYXJnaW4sMiooc2VyaWVzW2ldLnBvaW50cy5yYWRpdXMrc2VyaWVzW2ldLnBvaW50cy5saW5lV2lkdGgvMikpfXZhciBtYXJnaW5zPXtsZWZ0Om1pbk1hcmdpbixyaWdodDptaW5NYXJnaW4sdG9wOm1pbk1hcmdpbixib3R0b206bWluTWFyZ2lufTskLmVhY2goYWxsQXhlcygpLGZ1bmN0aW9uKF8sYXhpcyl7aWYoYXhpcy5yZXNlcnZlU3BhY2UmJmF4aXMudGlja3MmJmF4aXMudGlja3MubGVuZ3RoKXtpZihheGlzLmRpcmVjdGlvbj09PVwieFwiKXttYXJnaW5zLmxlZnQ9TWF0aC5tYXgobWFyZ2lucy5sZWZ0LGF4aXMubGFiZWxXaWR0aC8yKTttYXJnaW5zLnJpZ2h0PU1hdGgubWF4KG1hcmdpbnMucmlnaHQsYXhpcy5sYWJlbFdpZHRoLzIpfWVsc2V7bWFyZ2lucy5ib3R0b209TWF0aC5tYXgobWFyZ2lucy5ib3R0b20sYXhpcy5sYWJlbEhlaWdodC8yKTttYXJnaW5zLnRvcD1NYXRoLm1heChtYXJnaW5zLnRvcCxheGlzLmxhYmVsSGVpZ2h0LzIpfX19KTtwbG90T2Zmc2V0LmxlZnQ9TWF0aC5jZWlsKE1hdGgubWF4KG1hcmdpbnMubGVmdCxwbG90T2Zmc2V0LmxlZnQpKTtwbG90T2Zmc2V0LnJpZ2h0PU1hdGguY2VpbChNYXRoLm1heChtYXJnaW5zLnJpZ2h0LHBsb3RPZmZzZXQucmlnaHQpKTtwbG90T2Zmc2V0LnRvcD1NYXRoLmNlaWwoTWF0aC5tYXgobWFyZ2lucy50b3AscGxvdE9mZnNldC50b3ApKTtwbG90T2Zmc2V0LmJvdHRvbT1NYXRoLmNlaWwoTWF0aC5tYXgobWFyZ2lucy5ib3R0b20scGxvdE9mZnNldC5ib3R0b20pKX1mdW5jdGlvbiBzZXR1cEdyaWQoKXt2YXIgaSxheGVzPWFsbEF4ZXMoKSxzaG93R3JpZD1vcHRpb25zLmdyaWQuc2hvdztmb3IodmFyIGEgaW4gcGxvdE9mZnNldCl7dmFyIG1hcmdpbj1vcHRpb25zLmdyaWQubWFyZ2lufHwwO3Bsb3RPZmZzZXRbYV09dHlwZW9mIG1hcmdpbj09XCJudW1iZXJcIj9tYXJnaW46bWFyZ2luW2FdfHwwfWV4ZWN1dGVIb29rcyhob29rcy5wcm9jZXNzT2Zmc2V0LFtwbG90T2Zmc2V0XSk7Zm9yKHZhciBhIGluIHBsb3RPZmZzZXQpe2lmKHR5cGVvZiBvcHRpb25zLmdyaWQuYm9yZGVyV2lkdGg9PVwib2JqZWN0XCIpe3Bsb3RPZmZzZXRbYV0rPXNob3dHcmlkP29wdGlvbnMuZ3JpZC5ib3JkZXJXaWR0aFthXTowfWVsc2V7cGxvdE9mZnNldFthXSs9c2hvd0dyaWQ/b3B0aW9ucy5ncmlkLmJvcmRlcldpZHRoOjB9fSQuZWFjaChheGVzLGZ1bmN0aW9uKF8sYXhpcyl7dmFyIGF4aXNPcHRzPWF4aXMub3B0aW9ucztheGlzLnNob3c9YXhpc09wdHMuc2hvdz09bnVsbD9heGlzLnVzZWQ6YXhpc09wdHMuc2hvdztheGlzLnJlc2VydmVTcGFjZT1heGlzT3B0cy5yZXNlcnZlU3BhY2U9PW51bGw/YXhpcy5zaG93OmF4aXNPcHRzLnJlc2VydmVTcGFjZTtzZXRSYW5nZShheGlzKX0pO2lmKHNob3dHcmlkKXt2YXIgYWxsb2NhdGVkQXhlcz0kLmdyZXAoYXhlcyxmdW5jdGlvbihheGlzKXtyZXR1cm4gYXhpcy5zaG93fHxheGlzLnJlc2VydmVTcGFjZX0pOyQuZWFjaChhbGxvY2F0ZWRBeGVzLGZ1bmN0aW9uKF8sYXhpcyl7c2V0dXBUaWNrR2VuZXJhdGlvbihheGlzKTtzZXRUaWNrcyhheGlzKTtzbmFwUmFuZ2VUb1RpY2tzKGF4aXMsYXhpcy50aWNrcyk7bWVhc3VyZVRpY2tMYWJlbHMoYXhpcyl9KTtmb3IoaT1hbGxvY2F0ZWRBeGVzLmxlbmd0aC0xO2k+PTA7LS1pKWFsbG9jYXRlQXhpc0JveEZpcnN0UGhhc2UoYWxsb2NhdGVkQXhlc1tpXSk7YWRqdXN0TGF5b3V0Rm9yVGhpbmdzU3RpY2tpbmdPdXQoKTskLmVhY2goYWxsb2NhdGVkQXhlcyxmdW5jdGlvbihfLGF4aXMpe2FsbG9jYXRlQXhpc0JveFNlY29uZFBoYXNlKGF4aXMpfSl9cGxvdFdpZHRoPXN1cmZhY2Uud2lkdGgtcGxvdE9mZnNldC5sZWZ0LXBsb3RPZmZzZXQucmlnaHQ7cGxvdEhlaWdodD1zdXJmYWNlLmhlaWdodC1wbG90T2Zmc2V0LmJvdHRvbS1wbG90T2Zmc2V0LnRvcDskLmVhY2goYXhlcyxmdW5jdGlvbihfLGF4aXMpe3NldFRyYW5zZm9ybWF0aW9uSGVscGVycyhheGlzKX0pO2lmKHNob3dHcmlkKXtkcmF3QXhpc0xhYmVscygpfWluc2VydExlZ2VuZCgpfWZ1bmN0aW9uIHNldFJhbmdlKGF4aXMpe3ZhciBvcHRzPWF4aXMub3B0aW9ucyxtaW49KyhvcHRzLm1pbiE9bnVsbD9vcHRzLm1pbjpheGlzLmRhdGFtaW4pLG1heD0rKG9wdHMubWF4IT1udWxsP29wdHMubWF4OmF4aXMuZGF0YW1heCksZGVsdGE9bWF4LW1pbjtpZihkZWx0YT09MCl7dmFyIHdpZGVuPW1heD09MD8xOi4wMTtpZihvcHRzLm1pbj09bnVsbCltaW4tPXdpZGVuO2lmKG9wdHMubWF4PT1udWxsfHxvcHRzLm1pbiE9bnVsbCltYXgrPXdpZGVufWVsc2V7dmFyIG1hcmdpbj1vcHRzLmF1dG9zY2FsZU1hcmdpbjtpZihtYXJnaW4hPW51bGwpe2lmKG9wdHMubWluPT1udWxsKXttaW4tPWRlbHRhKm1hcmdpbjtpZihtaW48MCYmYXhpcy5kYXRhbWluIT1udWxsJiZheGlzLmRhdGFtaW4+PTApbWluPTB9aWYob3B0cy5tYXg9PW51bGwpe21heCs9ZGVsdGEqbWFyZ2luO2lmKG1heD4wJiZheGlzLmRhdGFtYXghPW51bGwmJmF4aXMuZGF0YW1heDw9MCltYXg9MH19fWF4aXMubWluPW1pbjtheGlzLm1heD1tYXh9ZnVuY3Rpb24gc2V0dXBUaWNrR2VuZXJhdGlvbihheGlzKXt2YXIgb3B0cz1heGlzLm9wdGlvbnM7dmFyIG5vVGlja3M7aWYodHlwZW9mIG9wdHMudGlja3M9PVwibnVtYmVyXCImJm9wdHMudGlja3M+MClub1RpY2tzPW9wdHMudGlja3M7ZWxzZSBub1RpY2tzPS4zKk1hdGguc3FydChheGlzLmRpcmVjdGlvbj09XCJ4XCI/c3VyZmFjZS53aWR0aDpzdXJmYWNlLmhlaWdodCk7dmFyIGRlbHRhPShheGlzLm1heC1heGlzLm1pbikvbm9UaWNrcyxkZWM9LU1hdGguZmxvb3IoTWF0aC5sb2coZGVsdGEpL01hdGguTE4xMCksbWF4RGVjPW9wdHMudGlja0RlY2ltYWxzO2lmKG1heERlYyE9bnVsbCYmZGVjPm1heERlYyl7ZGVjPW1heERlY312YXIgbWFnbj1NYXRoLnBvdygxMCwtZGVjKSxub3JtPWRlbHRhL21hZ24sc2l6ZTtpZihub3JtPDEuNSl7c2l6ZT0xfWVsc2UgaWYobm9ybTwzKXtzaXplPTI7aWYobm9ybT4yLjI1JiYobWF4RGVjPT1udWxsfHxkZWMrMTw9bWF4RGVjKSl7c2l6ZT0yLjU7KytkZWN9fWVsc2UgaWYobm9ybTw3LjUpe3NpemU9NX1lbHNle3NpemU9MTB9c2l6ZSo9bWFnbjtpZihvcHRzLm1pblRpY2tTaXplIT1udWxsJiZzaXplPG9wdHMubWluVGlja1NpemUpe3NpemU9b3B0cy5taW5UaWNrU2l6ZX1heGlzLmRlbHRhPWRlbHRhO2F4aXMudGlja0RlY2ltYWxzPU1hdGgubWF4KDAsbWF4RGVjIT1udWxsP21heERlYzpkZWMpO2F4aXMudGlja1NpemU9b3B0cy50aWNrU2l6ZXx8c2l6ZTtpZihvcHRzLm1vZGU9PVwidGltZVwiJiYhYXhpcy50aWNrR2VuZXJhdG9yKXt0aHJvdyBuZXcgRXJyb3IoXCJUaW1lIG1vZGUgcmVxdWlyZXMgdGhlIGZsb3QudGltZSBwbHVnaW4uXCIpfWlmKCFheGlzLnRpY2tHZW5lcmF0b3Ipe2F4aXMudGlja0dlbmVyYXRvcj1mdW5jdGlvbihheGlzKXt2YXIgdGlja3M9W10sc3RhcnQ9Zmxvb3JJbkJhc2UoYXhpcy5taW4sYXhpcy50aWNrU2l6ZSksaT0wLHY9TnVtYmVyLk5hTixwcmV2O2Rve3ByZXY9djt2PXN0YXJ0K2kqYXhpcy50aWNrU2l6ZTt0aWNrcy5wdXNoKHYpOysraX13aGlsZSh2PGF4aXMubWF4JiZ2IT1wcmV2KTtyZXR1cm4gdGlja3N9O2F4aXMudGlja0Zvcm1hdHRlcj1mdW5jdGlvbih2YWx1ZSxheGlzKXt2YXIgZmFjdG9yPWF4aXMudGlja0RlY2ltYWxzP01hdGgucG93KDEwLGF4aXMudGlja0RlY2ltYWxzKToxO3ZhciBmb3JtYXR0ZWQ9XCJcIitNYXRoLnJvdW5kKHZhbHVlKmZhY3RvcikvZmFjdG9yO2lmKGF4aXMudGlja0RlY2ltYWxzIT1udWxsKXt2YXIgZGVjaW1hbD1mb3JtYXR0ZWQuaW5kZXhPZihcIi5cIik7dmFyIHByZWNpc2lvbj1kZWNpbWFsPT0tMT8wOmZvcm1hdHRlZC5sZW5ndGgtZGVjaW1hbC0xO2lmKHByZWNpc2lvbjxheGlzLnRpY2tEZWNpbWFscyl7cmV0dXJuKHByZWNpc2lvbj9mb3JtYXR0ZWQ6Zm9ybWF0dGVkK1wiLlwiKSsoXCJcIitmYWN0b3IpLnN1YnN0cigxLGF4aXMudGlja0RlY2ltYWxzLXByZWNpc2lvbil9fXJldHVybiBmb3JtYXR0ZWR9fWlmKCQuaXNGdW5jdGlvbihvcHRzLnRpY2tGb3JtYXR0ZXIpKWF4aXMudGlja0Zvcm1hdHRlcj1mdW5jdGlvbih2LGF4aXMpe3JldHVyblwiXCIrb3B0cy50aWNrRm9ybWF0dGVyKHYsYXhpcyl9O2lmKG9wdHMuYWxpZ25UaWNrc1dpdGhBeGlzIT1udWxsKXt2YXIgb3RoZXJBeGlzPShheGlzLmRpcmVjdGlvbj09XCJ4XCI/eGF4ZXM6eWF4ZXMpW29wdHMuYWxpZ25UaWNrc1dpdGhBeGlzLTFdO2lmKG90aGVyQXhpcyYmb3RoZXJBeGlzLnVzZWQmJm90aGVyQXhpcyE9YXhpcyl7dmFyIG5pY2VUaWNrcz1heGlzLnRpY2tHZW5lcmF0b3IoYXhpcyk7aWYobmljZVRpY2tzLmxlbmd0aD4wKXtpZihvcHRzLm1pbj09bnVsbClheGlzLm1pbj1NYXRoLm1pbihheGlzLm1pbixuaWNlVGlja3NbMF0pO2lmKG9wdHMubWF4PT1udWxsJiZuaWNlVGlja3MubGVuZ3RoPjEpYXhpcy5tYXg9TWF0aC5tYXgoYXhpcy5tYXgsbmljZVRpY2tzW25pY2VUaWNrcy5sZW5ndGgtMV0pfWF4aXMudGlja0dlbmVyYXRvcj1mdW5jdGlvbihheGlzKXt2YXIgdGlja3M9W10sdixpO2ZvcihpPTA7aTxvdGhlckF4aXMudGlja3MubGVuZ3RoOysraSl7dj0ob3RoZXJBeGlzLnRpY2tzW2ldLnYtb3RoZXJBeGlzLm1pbikvKG90aGVyQXhpcy5tYXgtb3RoZXJBeGlzLm1pbik7dj1heGlzLm1pbit2KihheGlzLm1heC1heGlzLm1pbik7dGlja3MucHVzaCh2KX1yZXR1cm4gdGlja3N9O2lmKCFheGlzLm1vZGUmJm9wdHMudGlja0RlY2ltYWxzPT1udWxsKXt2YXIgZXh0cmFEZWM9TWF0aC5tYXgoMCwtTWF0aC5mbG9vcihNYXRoLmxvZyhheGlzLmRlbHRhKS9NYXRoLkxOMTApKzEpLHRzPWF4aXMudGlja0dlbmVyYXRvcihheGlzKTtpZighKHRzLmxlbmd0aD4xJiYvXFwuLiowJC8udGVzdCgodHNbMV0tdHNbMF0pLnRvRml4ZWQoZXh0cmFEZWMpKSkpYXhpcy50aWNrRGVjaW1hbHM9ZXh0cmFEZWN9fX19ZnVuY3Rpb24gc2V0VGlja3MoYXhpcyl7dmFyIG90aWNrcz1heGlzLm9wdGlvbnMudGlja3MsdGlja3M9W107aWYob3RpY2tzPT1udWxsfHx0eXBlb2Ygb3RpY2tzPT1cIm51bWJlclwiJiZvdGlja3M+MCl0aWNrcz1heGlzLnRpY2tHZW5lcmF0b3IoYXhpcyk7ZWxzZSBpZihvdGlja3Mpe2lmKCQuaXNGdW5jdGlvbihvdGlja3MpKXRpY2tzPW90aWNrcyhheGlzKTtlbHNlIHRpY2tzPW90aWNrc312YXIgaSx2O2F4aXMudGlja3M9W107Zm9yKGk9MDtpPHRpY2tzLmxlbmd0aDsrK2kpe3ZhciBsYWJlbD1udWxsO3ZhciB0PXRpY2tzW2ldO2lmKHR5cGVvZiB0PT1cIm9iamVjdFwiKXt2PSt0WzBdO2lmKHQubGVuZ3RoPjEpbGFiZWw9dFsxXX1lbHNlIHY9K3Q7aWYobGFiZWw9PW51bGwpbGFiZWw9YXhpcy50aWNrRm9ybWF0dGVyKHYsYXhpcyk7aWYoIWlzTmFOKHYpKWF4aXMudGlja3MucHVzaCh7djp2LGxhYmVsOmxhYmVsfSl9fWZ1bmN0aW9uIHNuYXBSYW5nZVRvVGlja3MoYXhpcyx0aWNrcyl7aWYoYXhpcy5vcHRpb25zLmF1dG9zY2FsZU1hcmdpbiYmdGlja3MubGVuZ3RoPjApe2lmKGF4aXMub3B0aW9ucy5taW49PW51bGwpYXhpcy5taW49TWF0aC5taW4oYXhpcy5taW4sdGlja3NbMF0udik7aWYoYXhpcy5vcHRpb25zLm1heD09bnVsbCYmdGlja3MubGVuZ3RoPjEpYXhpcy5tYXg9TWF0aC5tYXgoYXhpcy5tYXgsdGlja3NbdGlja3MubGVuZ3RoLTFdLnYpfX1mdW5jdGlvbiBkcmF3KCl7c3VyZmFjZS5jbGVhcigpO2V4ZWN1dGVIb29rcyhob29rcy5kcmF3QmFja2dyb3VuZCxbY3R4XSk7dmFyIGdyaWQ9b3B0aW9ucy5ncmlkO2lmKGdyaWQuc2hvdyYmZ3JpZC5iYWNrZ3JvdW5kQ29sb3IpZHJhd0JhY2tncm91bmQoKTtpZihncmlkLnNob3cmJiFncmlkLmFib3ZlRGF0YSl7ZHJhd0dyaWQoKX1mb3IodmFyIGk9MDtpPHNlcmllcy5sZW5ndGg7KytpKXtleGVjdXRlSG9va3MoaG9va3MuZHJhd1NlcmllcyxbY3R4LHNlcmllc1tpXV0pO2RyYXdTZXJpZXMoc2VyaWVzW2ldKX1leGVjdXRlSG9va3MoaG9va3MuZHJhdyxbY3R4XSk7aWYoZ3JpZC5zaG93JiZncmlkLmFib3ZlRGF0YSl7ZHJhd0dyaWQoKX1zdXJmYWNlLnJlbmRlcigpO3RyaWdnZXJSZWRyYXdPdmVybGF5KCl9ZnVuY3Rpb24gZXh0cmFjdFJhbmdlKHJhbmdlcyxjb29yZCl7dmFyIGF4aXMsZnJvbSx0byxrZXksYXhlcz1hbGxBeGVzKCk7Zm9yKHZhciBpPTA7aTxheGVzLmxlbmd0aDsrK2kpe2F4aXM9YXhlc1tpXTtpZihheGlzLmRpcmVjdGlvbj09Y29vcmQpe2tleT1jb29yZCtheGlzLm4rXCJheGlzXCI7aWYoIXJhbmdlc1trZXldJiZheGlzLm49PTEpa2V5PWNvb3JkK1wiYXhpc1wiO2lmKHJhbmdlc1trZXldKXtmcm9tPXJhbmdlc1trZXldLmZyb207dG89cmFuZ2VzW2tleV0udG87YnJlYWt9fX1pZighcmFuZ2VzW2tleV0pe2F4aXM9Y29vcmQ9PVwieFwiP3hheGVzWzBdOnlheGVzWzBdO2Zyb209cmFuZ2VzW2Nvb3JkK1wiMVwiXTt0bz1yYW5nZXNbY29vcmQrXCIyXCJdfWlmKGZyb20hPW51bGwmJnRvIT1udWxsJiZmcm9tPnRvKXt2YXIgdG1wPWZyb207ZnJvbT10bzt0bz10bXB9cmV0dXJue2Zyb206ZnJvbSx0bzp0byxheGlzOmF4aXN9fWZ1bmN0aW9uIGRyYXdCYWNrZ3JvdW5kKCl7Y3R4LnNhdmUoKTtjdHgudHJhbnNsYXRlKHBsb3RPZmZzZXQubGVmdCxwbG90T2Zmc2V0LnRvcCk7Y3R4LmZpbGxTdHlsZT1nZXRDb2xvck9yR3JhZGllbnQob3B0aW9ucy5ncmlkLmJhY2tncm91bmRDb2xvcixwbG90SGVpZ2h0LDAsXCJyZ2JhKDI1NSwgMjU1LCAyNTUsIDApXCIpO2N0eC5maWxsUmVjdCgwLDAscGxvdFdpZHRoLHBsb3RIZWlnaHQpO2N0eC5yZXN0b3JlKCl9ZnVuY3Rpb24gZHJhd0dyaWQoKXt2YXIgaSxheGVzLGJ3LGJjO2N0eC5zYXZlKCk7Y3R4LnRyYW5zbGF0ZShwbG90T2Zmc2V0LmxlZnQscGxvdE9mZnNldC50b3ApO3ZhciBtYXJraW5ncz1vcHRpb25zLmdyaWQubWFya2luZ3M7aWYobWFya2luZ3Mpe2lmKCQuaXNGdW5jdGlvbihtYXJraW5ncykpe2F4ZXM9cGxvdC5nZXRBeGVzKCk7YXhlcy54bWluPWF4ZXMueGF4aXMubWluO2F4ZXMueG1heD1heGVzLnhheGlzLm1heDtheGVzLnltaW49YXhlcy55YXhpcy5taW47YXhlcy55bWF4PWF4ZXMueWF4aXMubWF4O21hcmtpbmdzPW1hcmtpbmdzKGF4ZXMpfWZvcihpPTA7aTxtYXJraW5ncy5sZW5ndGg7KytpKXt2YXIgbT1tYXJraW5nc1tpXSx4cmFuZ2U9ZXh0cmFjdFJhbmdlKG0sXCJ4XCIpLHlyYW5nZT1leHRyYWN0UmFuZ2UobSxcInlcIik7aWYoeHJhbmdlLmZyb209PW51bGwpeHJhbmdlLmZyb209eHJhbmdlLmF4aXMubWluO2lmKHhyYW5nZS50bz09bnVsbCl4cmFuZ2UudG89eHJhbmdlLmF4aXMubWF4O1xyXG5pZih5cmFuZ2UuZnJvbT09bnVsbCl5cmFuZ2UuZnJvbT15cmFuZ2UuYXhpcy5taW47aWYoeXJhbmdlLnRvPT1udWxsKXlyYW5nZS50bz15cmFuZ2UuYXhpcy5tYXg7aWYoeHJhbmdlLnRvPHhyYW5nZS5heGlzLm1pbnx8eHJhbmdlLmZyb20+eHJhbmdlLmF4aXMubWF4fHx5cmFuZ2UudG88eXJhbmdlLmF4aXMubWlufHx5cmFuZ2UuZnJvbT55cmFuZ2UuYXhpcy5tYXgpY29udGludWU7eHJhbmdlLmZyb209TWF0aC5tYXgoeHJhbmdlLmZyb20seHJhbmdlLmF4aXMubWluKTt4cmFuZ2UudG89TWF0aC5taW4oeHJhbmdlLnRvLHhyYW5nZS5heGlzLm1heCk7eXJhbmdlLmZyb209TWF0aC5tYXgoeXJhbmdlLmZyb20seXJhbmdlLmF4aXMubWluKTt5cmFuZ2UudG89TWF0aC5taW4oeXJhbmdlLnRvLHlyYW5nZS5heGlzLm1heCk7dmFyIHhlcXVhbD14cmFuZ2UuZnJvbT09PXhyYW5nZS50byx5ZXF1YWw9eXJhbmdlLmZyb209PT15cmFuZ2UudG87aWYoeGVxdWFsJiZ5ZXF1YWwpe2NvbnRpbnVlfXhyYW5nZS5mcm9tPU1hdGguZmxvb3IoeHJhbmdlLmF4aXMucDJjKHhyYW5nZS5mcm9tKSk7eHJhbmdlLnRvPU1hdGguZmxvb3IoeHJhbmdlLmF4aXMucDJjKHhyYW5nZS50bykpO3lyYW5nZS5mcm9tPU1hdGguZmxvb3IoeXJhbmdlLmF4aXMucDJjKHlyYW5nZS5mcm9tKSk7eXJhbmdlLnRvPU1hdGguZmxvb3IoeXJhbmdlLmF4aXMucDJjKHlyYW5nZS50bykpO2lmKHhlcXVhbHx8eWVxdWFsKXt2YXIgbGluZVdpZHRoPW0ubGluZVdpZHRofHxvcHRpb25zLmdyaWQubWFya2luZ3NMaW5lV2lkdGgsc3ViUGl4ZWw9bGluZVdpZHRoJTI/LjU6MDtjdHguYmVnaW5QYXRoKCk7Y3R4LnN0cm9rZVN0eWxlPW0uY29sb3J8fG9wdGlvbnMuZ3JpZC5tYXJraW5nc0NvbG9yO2N0eC5saW5lV2lkdGg9bGluZVdpZHRoO2lmKHhlcXVhbCl7Y3R4Lm1vdmVUbyh4cmFuZ2UudG8rc3ViUGl4ZWwseXJhbmdlLmZyb20pO2N0eC5saW5lVG8oeHJhbmdlLnRvK3N1YlBpeGVsLHlyYW5nZS50byl9ZWxzZXtjdHgubW92ZVRvKHhyYW5nZS5mcm9tLHlyYW5nZS50bytzdWJQaXhlbCk7Y3R4LmxpbmVUbyh4cmFuZ2UudG8seXJhbmdlLnRvK3N1YlBpeGVsKX1jdHguc3Ryb2tlKCl9ZWxzZXtjdHguZmlsbFN0eWxlPW0uY29sb3J8fG9wdGlvbnMuZ3JpZC5tYXJraW5nc0NvbG9yO2N0eC5maWxsUmVjdCh4cmFuZ2UuZnJvbSx5cmFuZ2UudG8seHJhbmdlLnRvLXhyYW5nZS5mcm9tLHlyYW5nZS5mcm9tLXlyYW5nZS50byl9fX1heGVzPWFsbEF4ZXMoKTtidz1vcHRpb25zLmdyaWQuYm9yZGVyV2lkdGg7Zm9yKHZhciBqPTA7ajxheGVzLmxlbmd0aDsrK2ope3ZhciBheGlzPWF4ZXNbal0sYm94PWF4aXMuYm94LHQ9YXhpcy50aWNrTGVuZ3RoLHgseSx4b2ZmLHlvZmY7aWYoIWF4aXMuc2hvd3x8YXhpcy50aWNrcy5sZW5ndGg9PTApY29udGludWU7Y3R4LmxpbmVXaWR0aD0xO2lmKGF4aXMuZGlyZWN0aW9uPT1cInhcIil7eD0wO2lmKHQ9PVwiZnVsbFwiKXk9YXhpcy5wb3NpdGlvbj09XCJ0b3BcIj8wOnBsb3RIZWlnaHQ7ZWxzZSB5PWJveC50b3AtcGxvdE9mZnNldC50b3ArKGF4aXMucG9zaXRpb249PVwidG9wXCI/Ym94LmhlaWdodDowKX1lbHNle3k9MDtpZih0PT1cImZ1bGxcIil4PWF4aXMucG9zaXRpb249PVwibGVmdFwiPzA6cGxvdFdpZHRoO2Vsc2UgeD1ib3gubGVmdC1wbG90T2Zmc2V0LmxlZnQrKGF4aXMucG9zaXRpb249PVwibGVmdFwiP2JveC53aWR0aDowKX1pZighYXhpcy5pbm5lcm1vc3Qpe2N0eC5zdHJva2VTdHlsZT1heGlzLm9wdGlvbnMuY29sb3I7Y3R4LmJlZ2luUGF0aCgpO3hvZmY9eW9mZj0wO2lmKGF4aXMuZGlyZWN0aW9uPT1cInhcIil4b2ZmPXBsb3RXaWR0aCsxO2Vsc2UgeW9mZj1wbG90SGVpZ2h0KzE7aWYoY3R4LmxpbmVXaWR0aD09MSl7aWYoYXhpcy5kaXJlY3Rpb249PVwieFwiKXt5PU1hdGguZmxvb3IoeSkrLjV9ZWxzZXt4PU1hdGguZmxvb3IoeCkrLjV9fWN0eC5tb3ZlVG8oeCx5KTtjdHgubGluZVRvKHgreG9mZix5K3lvZmYpO2N0eC5zdHJva2UoKX1jdHguc3Ryb2tlU3R5bGU9YXhpcy5vcHRpb25zLnRpY2tDb2xvcjtjdHguYmVnaW5QYXRoKCk7Zm9yKGk9MDtpPGF4aXMudGlja3MubGVuZ3RoOysraSl7dmFyIHY9YXhpcy50aWNrc1tpXS52O3hvZmY9eW9mZj0wO2lmKGlzTmFOKHYpfHx2PGF4aXMubWlufHx2PmF4aXMubWF4fHx0PT1cImZ1bGxcIiYmKHR5cGVvZiBidz09XCJvYmplY3RcIiYmYndbYXhpcy5wb3NpdGlvbl0+MHx8Ync+MCkmJih2PT1heGlzLm1pbnx8dj09YXhpcy5tYXgpKWNvbnRpbnVlO2lmKGF4aXMuZGlyZWN0aW9uPT1cInhcIil7eD1heGlzLnAyYyh2KTt5b2ZmPXQ9PVwiZnVsbFwiPy1wbG90SGVpZ2h0OnQ7aWYoYXhpcy5wb3NpdGlvbj09XCJ0b3BcIil5b2ZmPS15b2ZmfWVsc2V7eT1heGlzLnAyYyh2KTt4b2ZmPXQ9PVwiZnVsbFwiPy1wbG90V2lkdGg6dDtpZihheGlzLnBvc2l0aW9uPT1cImxlZnRcIil4b2ZmPS14b2ZmfWlmKGN0eC5saW5lV2lkdGg9PTEpe2lmKGF4aXMuZGlyZWN0aW9uPT1cInhcIil4PU1hdGguZmxvb3IoeCkrLjU7ZWxzZSB5PU1hdGguZmxvb3IoeSkrLjV9Y3R4Lm1vdmVUbyh4LHkpO2N0eC5saW5lVG8oeCt4b2ZmLHkreW9mZil9Y3R4LnN0cm9rZSgpfWlmKGJ3KXtiYz1vcHRpb25zLmdyaWQuYm9yZGVyQ29sb3I7aWYodHlwZW9mIGJ3PT1cIm9iamVjdFwifHx0eXBlb2YgYmM9PVwib2JqZWN0XCIpe2lmKHR5cGVvZiBidyE9PVwib2JqZWN0XCIpe2J3PXt0b3A6YncscmlnaHQ6YncsYm90dG9tOmJ3LGxlZnQ6Ynd9fWlmKHR5cGVvZiBiYyE9PVwib2JqZWN0XCIpe2JjPXt0b3A6YmMscmlnaHQ6YmMsYm90dG9tOmJjLGxlZnQ6YmN9fWlmKGJ3LnRvcD4wKXtjdHguc3Ryb2tlU3R5bGU9YmMudG9wO2N0eC5saW5lV2lkdGg9YncudG9wO2N0eC5iZWdpblBhdGgoKTtjdHgubW92ZVRvKDAtYncubGVmdCwwLWJ3LnRvcC8yKTtjdHgubGluZVRvKHBsb3RXaWR0aCwwLWJ3LnRvcC8yKTtjdHguc3Ryb2tlKCl9aWYoYncucmlnaHQ+MCl7Y3R4LnN0cm9rZVN0eWxlPWJjLnJpZ2h0O2N0eC5saW5lV2lkdGg9YncucmlnaHQ7Y3R4LmJlZ2luUGF0aCgpO2N0eC5tb3ZlVG8ocGxvdFdpZHRoK2J3LnJpZ2h0LzIsMC1idy50b3ApO2N0eC5saW5lVG8ocGxvdFdpZHRoK2J3LnJpZ2h0LzIscGxvdEhlaWdodCk7Y3R4LnN0cm9rZSgpfWlmKGJ3LmJvdHRvbT4wKXtjdHguc3Ryb2tlU3R5bGU9YmMuYm90dG9tO2N0eC5saW5lV2lkdGg9YncuYm90dG9tO2N0eC5iZWdpblBhdGgoKTtjdHgubW92ZVRvKHBsb3RXaWR0aCtidy5yaWdodCxwbG90SGVpZ2h0K2J3LmJvdHRvbS8yKTtjdHgubGluZVRvKDAscGxvdEhlaWdodCtidy5ib3R0b20vMik7Y3R4LnN0cm9rZSgpfWlmKGJ3LmxlZnQ+MCl7Y3R4LnN0cm9rZVN0eWxlPWJjLmxlZnQ7Y3R4LmxpbmVXaWR0aD1idy5sZWZ0O2N0eC5iZWdpblBhdGgoKTtjdHgubW92ZVRvKDAtYncubGVmdC8yLHBsb3RIZWlnaHQrYncuYm90dG9tKTtjdHgubGluZVRvKDAtYncubGVmdC8yLDApO2N0eC5zdHJva2UoKX19ZWxzZXtjdHgubGluZVdpZHRoPWJ3O2N0eC5zdHJva2VTdHlsZT1vcHRpb25zLmdyaWQuYm9yZGVyQ29sb3I7Y3R4LnN0cm9rZVJlY3QoLWJ3LzIsLWJ3LzIscGxvdFdpZHRoK2J3LHBsb3RIZWlnaHQrYncpfX1jdHgucmVzdG9yZSgpfWZ1bmN0aW9uIGRyYXdBeGlzTGFiZWxzKCl7JC5lYWNoKGFsbEF4ZXMoKSxmdW5jdGlvbihfLGF4aXMpe3ZhciBib3g9YXhpcy5ib3gsbGVnYWN5U3R5bGVzPWF4aXMuZGlyZWN0aW9uK1wiQXhpcyBcIitheGlzLmRpcmVjdGlvbitheGlzLm4rXCJBeGlzXCIsbGF5ZXI9XCJmbG90LVwiK2F4aXMuZGlyZWN0aW9uK1wiLWF4aXMgZmxvdC1cIitheGlzLmRpcmVjdGlvbitheGlzLm4rXCItYXhpcyBcIitsZWdhY3lTdHlsZXMsZm9udD1heGlzLm9wdGlvbnMuZm9udHx8XCJmbG90LXRpY2stbGFiZWwgdGlja0xhYmVsXCIsdGljayx4LHksaGFsaWduLHZhbGlnbjtzdXJmYWNlLnJlbW92ZVRleHQobGF5ZXIpO2lmKCFheGlzLnNob3d8fGF4aXMudGlja3MubGVuZ3RoPT0wKXJldHVybjtmb3IodmFyIGk9MDtpPGF4aXMudGlja3MubGVuZ3RoOysraSl7dGljaz1heGlzLnRpY2tzW2ldO2lmKCF0aWNrLmxhYmVsfHx0aWNrLnY8YXhpcy5taW58fHRpY2sudj5heGlzLm1heCljb250aW51ZTtpZihheGlzLmRpcmVjdGlvbj09XCJ4XCIpe2hhbGlnbj1cImNlbnRlclwiO3g9cGxvdE9mZnNldC5sZWZ0K2F4aXMucDJjKHRpY2sudik7aWYoYXhpcy5wb3NpdGlvbj09XCJib3R0b21cIil7eT1ib3gudG9wK2JveC5wYWRkaW5nfWVsc2V7eT1ib3gudG9wK2JveC5oZWlnaHQtYm94LnBhZGRpbmc7dmFsaWduPVwiYm90dG9tXCJ9fWVsc2V7dmFsaWduPVwibWlkZGxlXCI7eT1wbG90T2Zmc2V0LnRvcCtheGlzLnAyYyh0aWNrLnYpO2lmKGF4aXMucG9zaXRpb249PVwibGVmdFwiKXt4PWJveC5sZWZ0K2JveC53aWR0aC1ib3gucGFkZGluZztoYWxpZ249XCJyaWdodFwifWVsc2V7eD1ib3gubGVmdCtib3gucGFkZGluZ319c3VyZmFjZS5hZGRUZXh0KGxheWVyLHgseSx0aWNrLmxhYmVsLGZvbnQsbnVsbCxudWxsLGhhbGlnbix2YWxpZ24pfX0pfWZ1bmN0aW9uIGRyYXdTZXJpZXMoc2VyaWVzKXtpZihzZXJpZXMubGluZXMuc2hvdylkcmF3U2VyaWVzTGluZXMoc2VyaWVzKTtpZihzZXJpZXMuYmFycy5zaG93KWRyYXdTZXJpZXNCYXJzKHNlcmllcyk7aWYoc2VyaWVzLnBvaW50cy5zaG93KWRyYXdTZXJpZXNQb2ludHMoc2VyaWVzKX1mdW5jdGlvbiBkcmF3U2VyaWVzTGluZXMoc2VyaWVzKXtmdW5jdGlvbiBwbG90TGluZShkYXRhcG9pbnRzLHhvZmZzZXQseW9mZnNldCxheGlzeCxheGlzeSl7dmFyIHBvaW50cz1kYXRhcG9pbnRzLnBvaW50cyxwcz1kYXRhcG9pbnRzLnBvaW50c2l6ZSxwcmV2eD1udWxsLHByZXZ5PW51bGw7Y3R4LmJlZ2luUGF0aCgpO2Zvcih2YXIgaT1wcztpPHBvaW50cy5sZW5ndGg7aSs9cHMpe3ZhciB4MT1wb2ludHNbaS1wc10seTE9cG9pbnRzW2ktcHMrMV0seDI9cG9pbnRzW2ldLHkyPXBvaW50c1tpKzFdO2lmKHgxPT1udWxsfHx4Mj09bnVsbCljb250aW51ZTtpZih5MTw9eTImJnkxPGF4aXN5Lm1pbil7aWYoeTI8YXhpc3kubWluKWNvbnRpbnVlO3gxPShheGlzeS5taW4teTEpLyh5Mi15MSkqKHgyLXgxKSt4MTt5MT1heGlzeS5taW59ZWxzZSBpZih5Mjw9eTEmJnkyPGF4aXN5Lm1pbil7aWYoeTE8YXhpc3kubWluKWNvbnRpbnVlO3gyPShheGlzeS5taW4teTEpLyh5Mi15MSkqKHgyLXgxKSt4MTt5Mj1heGlzeS5taW59aWYoeTE+PXkyJiZ5MT5heGlzeS5tYXgpe2lmKHkyPmF4aXN5Lm1heCljb250aW51ZTt4MT0oYXhpc3kubWF4LXkxKS8oeTIteTEpKih4Mi14MSkreDE7eTE9YXhpc3kubWF4fWVsc2UgaWYoeTI+PXkxJiZ5Mj5heGlzeS5tYXgpe2lmKHkxPmF4aXN5Lm1heCljb250aW51ZTt4Mj0oYXhpc3kubWF4LXkxKS8oeTIteTEpKih4Mi14MSkreDE7eTI9YXhpc3kubWF4fWlmKHgxPD14MiYmeDE8YXhpc3gubWluKXtpZih4MjxheGlzeC5taW4pY29udGludWU7eTE9KGF4aXN4Lm1pbi14MSkvKHgyLXgxKSooeTIteTEpK3kxO3gxPWF4aXN4Lm1pbn1lbHNlIGlmKHgyPD14MSYmeDI8YXhpc3gubWluKXtpZih4MTxheGlzeC5taW4pY29udGludWU7eTI9KGF4aXN4Lm1pbi14MSkvKHgyLXgxKSooeTIteTEpK3kxO3gyPWF4aXN4Lm1pbn1pZih4MT49eDImJngxPmF4aXN4Lm1heCl7aWYoeDI+YXhpc3gubWF4KWNvbnRpbnVlO3kxPShheGlzeC5tYXgteDEpLyh4Mi14MSkqKHkyLXkxKSt5MTt4MT1heGlzeC5tYXh9ZWxzZSBpZih4Mj49eDEmJngyPmF4aXN4Lm1heCl7aWYoeDE+YXhpc3gubWF4KWNvbnRpbnVlO3kyPShheGlzeC5tYXgteDEpLyh4Mi14MSkqKHkyLXkxKSt5MTt4Mj1heGlzeC5tYXh9aWYoeDEhPXByZXZ4fHx5MSE9cHJldnkpY3R4Lm1vdmVUbyhheGlzeC5wMmMoeDEpK3hvZmZzZXQsYXhpc3kucDJjKHkxKSt5b2Zmc2V0KTtwcmV2eD14MjtwcmV2eT15MjtjdHgubGluZVRvKGF4aXN4LnAyYyh4MikreG9mZnNldCxheGlzeS5wMmMoeTIpK3lvZmZzZXQpfWN0eC5zdHJva2UoKX1mdW5jdGlvbiBwbG90TGluZUFyZWEoZGF0YXBvaW50cyxheGlzeCxheGlzeSl7dmFyIHBvaW50cz1kYXRhcG9pbnRzLnBvaW50cyxwcz1kYXRhcG9pbnRzLnBvaW50c2l6ZSxib3R0b209TWF0aC5taW4oTWF0aC5tYXgoMCxheGlzeS5taW4pLGF4aXN5Lm1heCksaT0wLHRvcCxhcmVhT3Blbj1mYWxzZSx5cG9zPTEsc2VnbWVudFN0YXJ0PTAsc2VnbWVudEVuZD0wO3doaWxlKHRydWUpe2lmKHBzPjAmJmk+cG9pbnRzLmxlbmd0aCtwcylicmVhaztpKz1wczt2YXIgeDE9cG9pbnRzW2ktcHNdLHkxPXBvaW50c1tpLXBzK3lwb3NdLHgyPXBvaW50c1tpXSx5Mj1wb2ludHNbaSt5cG9zXTtpZihhcmVhT3Blbil7aWYocHM+MCYmeDEhPW51bGwmJngyPT1udWxsKXtzZWdtZW50RW5kPWk7cHM9LXBzO3lwb3M9Mjtjb250aW51ZX1pZihwczwwJiZpPT1zZWdtZW50U3RhcnQrcHMpe2N0eC5maWxsKCk7YXJlYU9wZW49ZmFsc2U7cHM9LXBzO3lwb3M9MTtpPXNlZ21lbnRTdGFydD1zZWdtZW50RW5kK3BzO2NvbnRpbnVlfX1pZih4MT09bnVsbHx8eDI9PW51bGwpY29udGludWU7aWYoeDE8PXgyJiZ4MTxheGlzeC5taW4pe2lmKHgyPGF4aXN4Lm1pbiljb250aW51ZTt5MT0oYXhpc3gubWluLXgxKS8oeDIteDEpKih5Mi15MSkreTE7eDE9YXhpc3gubWlufWVsc2UgaWYoeDI8PXgxJiZ4MjxheGlzeC5taW4pe2lmKHgxPGF4aXN4Lm1pbiljb250aW51ZTt5Mj0oYXhpc3gubWluLXgxKS8oeDIteDEpKih5Mi15MSkreTE7eDI9YXhpc3gubWlufWlmKHgxPj14MiYmeDE+YXhpc3gubWF4KXtpZih4Mj5heGlzeC5tYXgpY29udGludWU7eTE9KGF4aXN4Lm1heC14MSkvKHgyLXgxKSooeTIteTEpK3kxO3gxPWF4aXN4Lm1heH1lbHNlIGlmKHgyPj14MSYmeDI+YXhpc3gubWF4KXtpZih4MT5heGlzeC5tYXgpY29udGludWU7eTI9KGF4aXN4Lm1heC14MSkvKHgyLXgxKSooeTIteTEpK3kxO3gyPWF4aXN4Lm1heH1pZighYXJlYU9wZW4pe2N0eC5iZWdpblBhdGgoKTtjdHgubW92ZVRvKGF4aXN4LnAyYyh4MSksYXhpc3kucDJjKGJvdHRvbSkpO2FyZWFPcGVuPXRydWV9aWYoeTE+PWF4aXN5Lm1heCYmeTI+PWF4aXN5Lm1heCl7Y3R4LmxpbmVUbyhheGlzeC5wMmMoeDEpLGF4aXN5LnAyYyhheGlzeS5tYXgpKTtjdHgubGluZVRvKGF4aXN4LnAyYyh4MiksYXhpc3kucDJjKGF4aXN5Lm1heCkpO2NvbnRpbnVlfWVsc2UgaWYoeTE8PWF4aXN5Lm1pbiYmeTI8PWF4aXN5Lm1pbil7Y3R4LmxpbmVUbyhheGlzeC5wMmMoeDEpLGF4aXN5LnAyYyhheGlzeS5taW4pKTtjdHgubGluZVRvKGF4aXN4LnAyYyh4MiksYXhpc3kucDJjKGF4aXN5Lm1pbikpO2NvbnRpbnVlfXZhciB4MW9sZD14MSx4Mm9sZD14MjtpZih5MTw9eTImJnkxPGF4aXN5Lm1pbiYmeTI+PWF4aXN5Lm1pbil7eDE9KGF4aXN5Lm1pbi15MSkvKHkyLXkxKSooeDIteDEpK3gxO3kxPWF4aXN5Lm1pbn1lbHNlIGlmKHkyPD15MSYmeTI8YXhpc3kubWluJiZ5MT49YXhpc3kubWluKXt4Mj0oYXhpc3kubWluLXkxKS8oeTIteTEpKih4Mi14MSkreDE7eTI9YXhpc3kubWlufWlmKHkxPj15MiYmeTE+YXhpc3kubWF4JiZ5Mjw9YXhpc3kubWF4KXt4MT0oYXhpc3kubWF4LXkxKS8oeTIteTEpKih4Mi14MSkreDE7eTE9YXhpc3kubWF4fWVsc2UgaWYoeTI+PXkxJiZ5Mj5heGlzeS5tYXgmJnkxPD1heGlzeS5tYXgpe3gyPShheGlzeS5tYXgteTEpLyh5Mi15MSkqKHgyLXgxKSt4MTt5Mj1heGlzeS5tYXh9aWYoeDEhPXgxb2xkKXtjdHgubGluZVRvKGF4aXN4LnAyYyh4MW9sZCksYXhpc3kucDJjKHkxKSl9Y3R4LmxpbmVUbyhheGlzeC5wMmMoeDEpLGF4aXN5LnAyYyh5MSkpO2N0eC5saW5lVG8oYXhpc3gucDJjKHgyKSxheGlzeS5wMmMoeTIpKTtpZih4MiE9eDJvbGQpe2N0eC5saW5lVG8oYXhpc3gucDJjKHgyKSxheGlzeS5wMmMoeTIpKTtjdHgubGluZVRvKGF4aXN4LnAyYyh4Mm9sZCksYXhpc3kucDJjKHkyKSl9fX1jdHguc2F2ZSgpO2N0eC50cmFuc2xhdGUocGxvdE9mZnNldC5sZWZ0LHBsb3RPZmZzZXQudG9wKTtjdHgubGluZUpvaW49XCJyb3VuZFwiO3ZhciBsdz1zZXJpZXMubGluZXMubGluZVdpZHRoLHN3PXNlcmllcy5zaGFkb3dTaXplO2lmKGx3PjAmJnN3PjApe2N0eC5saW5lV2lkdGg9c3c7Y3R4LnN0cm9rZVN0eWxlPVwicmdiYSgwLDAsMCwwLjEpXCI7dmFyIGFuZ2xlPU1hdGguUEkvMTg7cGxvdExpbmUoc2VyaWVzLmRhdGFwb2ludHMsTWF0aC5zaW4oYW5nbGUpKihsdy8yK3N3LzIpLE1hdGguY29zKGFuZ2xlKSoobHcvMitzdy8yKSxzZXJpZXMueGF4aXMsc2VyaWVzLnlheGlzKTtjdHgubGluZVdpZHRoPXN3LzI7cGxvdExpbmUoc2VyaWVzLmRhdGFwb2ludHMsTWF0aC5zaW4oYW5nbGUpKihsdy8yK3N3LzQpLE1hdGguY29zKGFuZ2xlKSoobHcvMitzdy80KSxzZXJpZXMueGF4aXMsc2VyaWVzLnlheGlzKX1jdHgubGluZVdpZHRoPWx3O2N0eC5zdHJva2VTdHlsZT1zZXJpZXMuY29sb3I7dmFyIGZpbGxTdHlsZT1nZXRGaWxsU3R5bGUoc2VyaWVzLmxpbmVzLHNlcmllcy5jb2xvciwwLHBsb3RIZWlnaHQpO2lmKGZpbGxTdHlsZSl7Y3R4LmZpbGxTdHlsZT1maWxsU3R5bGU7cGxvdExpbmVBcmVhKHNlcmllcy5kYXRhcG9pbnRzLHNlcmllcy54YXhpcyxzZXJpZXMueWF4aXMpfWlmKGx3PjApcGxvdExpbmUoc2VyaWVzLmRhdGFwb2ludHMsMCwwLHNlcmllcy54YXhpcyxzZXJpZXMueWF4aXMpO2N0eC5yZXN0b3JlKCl9ZnVuY3Rpb24gZHJhd1Nlcmllc1BvaW50cyhzZXJpZXMpe2Z1bmN0aW9uIHBsb3RQb2ludHMoZGF0YXBvaW50cyxyYWRpdXMsZmlsbFN0eWxlLG9mZnNldCxzaGFkb3csYXhpc3gsYXhpc3ksc3ltYm9sKXt2YXIgcG9pbnRzPWRhdGFwb2ludHMucG9pbnRzLHBzPWRhdGFwb2ludHMucG9pbnRzaXplO2Zvcih2YXIgaT0wO2k8cG9pbnRzLmxlbmd0aDtpKz1wcyl7dmFyIHg9cG9pbnRzW2ldLHk9cG9pbnRzW2krMV07aWYoeD09bnVsbHx8eDxheGlzeC5taW58fHg+YXhpc3gubWF4fHx5PGF4aXN5Lm1pbnx8eT5heGlzeS5tYXgpY29udGludWU7Y3R4LmJlZ2luUGF0aCgpO3g9YXhpc3gucDJjKHgpO3k9YXhpc3kucDJjKHkpK29mZnNldDtpZihzeW1ib2w9PVwiY2lyY2xlXCIpY3R4LmFyYyh4LHkscmFkaXVzLDAsc2hhZG93P01hdGguUEk6TWF0aC5QSSoyLGZhbHNlKTtlbHNlIHN5bWJvbChjdHgseCx5LHJhZGl1cyxzaGFkb3cpO2N0eC5jbG9zZVBhdGgoKTtpZihmaWxsU3R5bGUpe2N0eC5maWxsU3R5bGU9ZmlsbFN0eWxlO2N0eC5maWxsKCl9Y3R4LnN0cm9rZSgpfX1jdHguc2F2ZSgpO2N0eC50cmFuc2xhdGUocGxvdE9mZnNldC5sZWZ0LHBsb3RPZmZzZXQudG9wKTt2YXIgbHc9c2VyaWVzLnBvaW50cy5saW5lV2lkdGgsc3c9c2VyaWVzLnNoYWRvd1NpemUscmFkaXVzPXNlcmllcy5wb2ludHMucmFkaXVzLHN5bWJvbD1zZXJpZXMucG9pbnRzLnN5bWJvbDtpZihsdz09MClsdz0xZS00O2lmKGx3PjAmJnN3PjApe3ZhciB3PXN3LzI7Y3R4LmxpbmVXaWR0aD13O2N0eC5zdHJva2VTdHlsZT1cInJnYmEoMCwwLDAsMC4xKVwiO3Bsb3RQb2ludHMoc2VyaWVzLmRhdGFwb2ludHMscmFkaXVzLG51bGwsdyt3LzIsdHJ1ZSxzZXJpZXMueGF4aXMsc2VyaWVzLnlheGlzLHN5bWJvbCk7Y3R4LnN0cm9rZVN0eWxlPVwicmdiYSgwLDAsMCwwLjIpXCI7cGxvdFBvaW50cyhzZXJpZXMuZGF0YXBvaW50cyxyYWRpdXMsbnVsbCx3LzIsdHJ1ZSxzZXJpZXMueGF4aXMsc2VyaWVzLnlheGlzLHN5bWJvbCl9Y3R4LmxpbmVXaWR0aD1sdztjdHguc3Ryb2tlU3R5bGU9c2VyaWVzLmNvbG9yO3Bsb3RQb2ludHMoc2VyaWVzLmRhdGFwb2ludHMscmFkaXVzLGdldEZpbGxTdHlsZShzZXJpZXMucG9pbnRzLHNlcmllcy5jb2xvciksMCxmYWxzZSxzZXJpZXMueGF4aXMsc2VyaWVzLnlheGlzLHN5bWJvbCk7Y3R4LnJlc3RvcmUoKX1mdW5jdGlvbiBkcmF3QmFyKHgseSxiLGJhckxlZnQsYmFyUmlnaHQsZmlsbFN0eWxlQ2FsbGJhY2ssYXhpc3gsYXhpc3ksYyxob3Jpem9udGFsLGxpbmVXaWR0aCl7dmFyIGxlZnQscmlnaHQsYm90dG9tLHRvcCxkcmF3TGVmdCxkcmF3UmlnaHQsZHJhd1RvcCxkcmF3Qm90dG9tLHRtcDtpZihob3Jpem9udGFsKXtkcmF3Qm90dG9tPWRyYXdSaWdodD1kcmF3VG9wPXRydWU7ZHJhd0xlZnQ9ZmFsc2U7bGVmdD1iO3JpZ2h0PXg7dG9wPXkrYmFyTGVmdDtib3R0b209eStiYXJSaWdodDtpZihyaWdodDxsZWZ0KXt0bXA9cmlnaHQ7cmlnaHQ9bGVmdDtsZWZ0PXRtcDtkcmF3TGVmdD10cnVlO2RyYXdSaWdodD1mYWxzZX19ZWxzZXtkcmF3TGVmdD1kcmF3UmlnaHQ9ZHJhd1RvcD10cnVlO2RyYXdCb3R0b209ZmFsc2U7bGVmdD14K2JhckxlZnQ7cmlnaHQ9eCtiYXJSaWdodDtib3R0b209Yjt0b3A9eTtpZih0b3A8Ym90dG9tKXt0bXA9dG9wO3RvcD1ib3R0b207Ym90dG9tPXRtcDtkcmF3Qm90dG9tPXRydWU7ZHJhd1RvcD1mYWxzZX19aWYocmlnaHQ8YXhpc3gubWlufHxsZWZ0PmF4aXN4Lm1heHx8dG9wPGF4aXN5Lm1pbnx8Ym90dG9tPmF4aXN5Lm1heClyZXR1cm47aWYobGVmdDxheGlzeC5taW4pe2xlZnQ9YXhpc3gubWluO2RyYXdMZWZ0PWZhbHNlfWlmKHJpZ2h0PmF4aXN4Lm1heCl7cmlnaHQ9YXhpc3gubWF4O2RyYXdSaWdodD1mYWxzZX1pZihib3R0b208YXhpc3kubWluKXtib3R0b209YXhpc3kubWluO2RyYXdCb3R0b209ZmFsc2V9aWYodG9wPmF4aXN5Lm1heCl7dG9wPWF4aXN5Lm1heDtkcmF3VG9wPWZhbHNlfWxlZnQ9YXhpc3gucDJjKGxlZnQpO2JvdHRvbT1heGlzeS5wMmMoYm90dG9tKTtyaWdodD1heGlzeC5wMmMocmlnaHQpO3RvcD1heGlzeS5wMmModG9wKTtpZihmaWxsU3R5bGVDYWxsYmFjayl7Yy5maWxsU3R5bGU9ZmlsbFN0eWxlQ2FsbGJhY2soYm90dG9tLHRvcCk7Yy5maWxsUmVjdChsZWZ0LHRvcCxyaWdodC1sZWZ0LGJvdHRvbS10b3ApfWlmKGxpbmVXaWR0aD4wJiYoZHJhd0xlZnR8fGRyYXdSaWdodHx8ZHJhd1RvcHx8ZHJhd0JvdHRvbSkpe2MuYmVnaW5QYXRoKCk7Yy5tb3ZlVG8obGVmdCxib3R0b20pO2lmKGRyYXdMZWZ0KWMubGluZVRvKGxlZnQsdG9wKTtlbHNlIGMubW92ZVRvKGxlZnQsdG9wKTtpZihkcmF3VG9wKWMubGluZVRvKHJpZ2h0LHRvcCk7ZWxzZSBjLm1vdmVUbyhyaWdodCx0b3ApO2lmKGRyYXdSaWdodCljLmxpbmVUbyhyaWdodCxib3R0b20pO2Vsc2UgYy5tb3ZlVG8ocmlnaHQsYm90dG9tKTtpZihkcmF3Qm90dG9tKWMubGluZVRvKGxlZnQsYm90dG9tKTtlbHNlIGMubW92ZVRvKGxlZnQsYm90dG9tKTtjLnN0cm9rZSgpfX1mdW5jdGlvbiBkcmF3U2VyaWVzQmFycyhzZXJpZXMpe2Z1bmN0aW9uIHBsb3RCYXJzKGRhdGFwb2ludHMsYmFyTGVmdCxiYXJSaWdodCxmaWxsU3R5bGVDYWxsYmFjayxheGlzeCxheGlzeSl7dmFyIHBvaW50cz1kYXRhcG9pbnRzLnBvaW50cyxwcz1kYXRhcG9pbnRzLnBvaW50c2l6ZTtmb3IodmFyIGk9MDtpPHBvaW50cy5sZW5ndGg7aSs9cHMpe2lmKHBvaW50c1tpXT09bnVsbCljb250aW51ZTtkcmF3QmFyKHBvaW50c1tpXSxwb2ludHNbaSsxXSxwb2ludHNbaSsyXSxiYXJMZWZ0LGJhclJpZ2h0LGZpbGxTdHlsZUNhbGxiYWNrLGF4aXN4LGF4aXN5LGN0eCxzZXJpZXMuYmFycy5ob3Jpem9udGFsLHNlcmllcy5iYXJzLmxpbmVXaWR0aCl9fWN0eC5zYXZlKCk7Y3R4LnRyYW5zbGF0ZShwbG90T2Zmc2V0LmxlZnQscGxvdE9mZnNldC50b3ApO2N0eC5saW5lV2lkdGg9c2VyaWVzLmJhcnMubGluZVdpZHRoO2N0eC5zdHJva2VTdHlsZT1zZXJpZXMuY29sb3I7dmFyIGJhckxlZnQ7c3dpdGNoKHNlcmllcy5iYXJzLmFsaWduKXtjYXNlXCJsZWZ0XCI6YmFyTGVmdD0wO2JyZWFrO2Nhc2VcInJpZ2h0XCI6YmFyTGVmdD0tc2VyaWVzLmJhcnMuYmFyV2lkdGg7YnJlYWs7ZGVmYXVsdDpiYXJMZWZ0PS1zZXJpZXMuYmFycy5iYXJXaWR0aC8yfXZhciBmaWxsU3R5bGVDYWxsYmFjaz1zZXJpZXMuYmFycy5maWxsP2Z1bmN0aW9uKGJvdHRvbSx0b3Ape3JldHVybiBnZXRGaWxsU3R5bGUoc2VyaWVzLmJhcnMsc2VyaWVzLmNvbG9yLGJvdHRvbSx0b3ApfTpudWxsO3Bsb3RCYXJzKHNlcmllcy5kYXRhcG9pbnRzLGJhckxlZnQsYmFyTGVmdCtzZXJpZXMuYmFycy5iYXJXaWR0aCxmaWxsU3R5bGVDYWxsYmFjayxzZXJpZXMueGF4aXMsc2VyaWVzLnlheGlzKTtjdHgucmVzdG9yZSgpfWZ1bmN0aW9uIGdldEZpbGxTdHlsZShmaWxsb3B0aW9ucyxzZXJpZXNDb2xvcixib3R0b20sdG9wKXt2YXIgZmlsbD1maWxsb3B0aW9ucy5maWxsO2lmKCFmaWxsKXJldHVybiBudWxsO2lmKGZpbGxvcHRpb25zLmZpbGxDb2xvcilyZXR1cm4gZ2V0Q29sb3JPckdyYWRpZW50KGZpbGxvcHRpb25zLmZpbGxDb2xvcixib3R0b20sdG9wLHNlcmllc0NvbG9yKTt2YXIgYz0kLmNvbG9yLnBhcnNlKHNlcmllc0NvbG9yKTtjLmE9dHlwZW9mIGZpbGw9PVwibnVtYmVyXCI/ZmlsbDouNDtjLm5vcm1hbGl6ZSgpO3JldHVybiBjLnRvU3RyaW5nKCl9ZnVuY3Rpb24gaW5zZXJ0TGVnZW5kKCl7aWYob3B0aW9ucy5sZWdlbmQuY29udGFpbmVyIT1udWxsKXskKG9wdGlvbnMubGVnZW5kLmNvbnRhaW5lcikuaHRtbChcIlwiKX1lbHNle3BsYWNlaG9sZGVyLmZpbmQoXCIubGVnZW5kXCIpLnJlbW92ZSgpfWlmKCFvcHRpb25zLmxlZ2VuZC5zaG93KXtyZXR1cm59dmFyIGZyYWdtZW50cz1bXSxlbnRyaWVzPVtdLHJvd1N0YXJ0ZWQ9ZmFsc2UsbGY9b3B0aW9ucy5sZWdlbmQubGFiZWxGb3JtYXR0ZXIscyxsYWJlbDtmb3IodmFyIGk9MDtpPHNlcmllcy5sZW5ndGg7KytpKXtzPXNlcmllc1tpXTtpZihzLmxhYmVsKXtsYWJlbD1sZj9sZihzLmxhYmVsLHMpOnMubGFiZWw7aWYobGFiZWwpe2VudHJpZXMucHVzaCh7bGFiZWw6bGFiZWwsY29sb3I6cy5jb2xvcn0pfX19aWYob3B0aW9ucy5sZWdlbmQuc29ydGVkKXtpZigkLmlzRnVuY3Rpb24ob3B0aW9ucy5sZWdlbmQuc29ydGVkKSl7ZW50cmllcy5zb3J0KG9wdGlvbnMubGVnZW5kLnNvcnRlZCl9ZWxzZSBpZihvcHRpb25zLmxlZ2VuZC5zb3J0ZWQ9PVwicmV2ZXJzZVwiKXtlbnRyaWVzLnJldmVyc2UoKX1lbHNle3ZhciBhc2NlbmRpbmc9b3B0aW9ucy5sZWdlbmQuc29ydGVkIT1cImRlc2NlbmRpbmdcIjtlbnRyaWVzLnNvcnQoZnVuY3Rpb24oYSxiKXtyZXR1cm4gYS5sYWJlbD09Yi5sYWJlbD8wOmEubGFiZWw8Yi5sYWJlbCE9YXNjZW5kaW5nPzE6LTF9KX19Zm9yKHZhciBpPTA7aTxlbnRyaWVzLmxlbmd0aDsrK2kpe3ZhciBlbnRyeT1lbnRyaWVzW2ldO2lmKGklb3B0aW9ucy5sZWdlbmQubm9Db2x1bW5zPT0wKXtpZihyb3dTdGFydGVkKWZyYWdtZW50cy5wdXNoKFwiPC90cj5cIik7ZnJhZ21lbnRzLnB1c2goXCI8dHI+XCIpO3Jvd1N0YXJ0ZWQ9dHJ1ZX1mcmFnbWVudHMucHVzaCgnPHRkIGNsYXNzPVwibGVnZW5kQ29sb3JCb3hcIj48ZGl2IHN0eWxlPVwiYm9yZGVyOjFweCBzb2xpZCAnK29wdGlvbnMubGVnZW5kLmxhYmVsQm94Qm9yZGVyQ29sb3IrJztwYWRkaW5nOjFweFwiPjxkaXYgc3R5bGU9XCJ3aWR0aDo0cHg7aGVpZ2h0OjA7Ym9yZGVyOjVweCBzb2xpZCAnK2VudHJ5LmNvbG9yKyc7b3ZlcmZsb3c6aGlkZGVuXCI+PC9kaXY+PC9kaXY+PC90ZD4nKyc8dGQgY2xhc3M9XCJsZWdlbmRMYWJlbFwiPicrZW50cnkubGFiZWwrXCI8L3RkPlwiKX1pZihyb3dTdGFydGVkKWZyYWdtZW50cy5wdXNoKFwiPC90cj5cIik7aWYoZnJhZ21lbnRzLmxlbmd0aD09MClyZXR1cm47dmFyIHRhYmxlPSc8dGFibGUgc3R5bGU9XCJmb250LXNpemU6c21hbGxlcjtjb2xvcjonK29wdGlvbnMuZ3JpZC5jb2xvcisnXCI+JytmcmFnbWVudHMuam9pbihcIlwiKStcIjwvdGFibGU+XCI7aWYob3B0aW9ucy5sZWdlbmQuY29udGFpbmVyIT1udWxsKSQob3B0aW9ucy5sZWdlbmQuY29udGFpbmVyKS5odG1sKHRhYmxlKTtlbHNle3ZhciBwb3M9XCJcIixwPW9wdGlvbnMubGVnZW5kLnBvc2l0aW9uLG09b3B0aW9ucy5sZWdlbmQubWFyZ2luO2lmKG1bMF09PW51bGwpbT1bbSxtXTtpZihwLmNoYXJBdCgwKT09XCJuXCIpcG9zKz1cInRvcDpcIisobVsxXStwbG90T2Zmc2V0LnRvcCkrXCJweDtcIjtlbHNlIGlmKHAuY2hhckF0KDApPT1cInNcIilwb3MrPVwiYm90dG9tOlwiKyhtWzFdK3Bsb3RPZmZzZXQuYm90dG9tKStcInB4O1wiO2lmKHAuY2hhckF0KDEpPT1cImVcIilwb3MrPVwicmlnaHQ6XCIrKG1bMF0rcGxvdE9mZnNldC5yaWdodCkrXCJweDtcIjtlbHNlIGlmKHAuY2hhckF0KDEpPT1cIndcIilwb3MrPVwibGVmdDpcIisobVswXStwbG90T2Zmc2V0LmxlZnQpK1wicHg7XCI7dmFyIGxlZ2VuZD0kKCc8ZGl2IGNsYXNzPVwibGVnZW5kXCI+Jyt0YWJsZS5yZXBsYWNlKCdzdHlsZT1cIicsJ3N0eWxlPVwicG9zaXRpb246YWJzb2x1dGU7Jytwb3MrXCI7XCIpK1wiPC9kaXY+XCIpLmFwcGVuZFRvKHBsYWNlaG9sZGVyKTtpZihvcHRpb25zLmxlZ2VuZC5iYWNrZ3JvdW5kT3BhY2l0eSE9MCl7dmFyIGM9b3B0aW9ucy5sZWdlbmQuYmFja2dyb3VuZENvbG9yO2lmKGM9PW51bGwpe2M9b3B0aW9ucy5ncmlkLmJhY2tncm91bmRDb2xvcjtpZihjJiZ0eXBlb2YgYz09XCJzdHJpbmdcIiljPSQuY29sb3IucGFyc2UoYyk7ZWxzZSBjPSQuY29sb3IuZXh0cmFjdChsZWdlbmQsXCJiYWNrZ3JvdW5kLWNvbG9yXCIpO2MuYT0xO2M9Yy50b1N0cmluZygpfXZhciBkaXY9bGVnZW5kLmNoaWxkcmVuKCk7JCgnPGRpdiBzdHlsZT1cInBvc2l0aW9uOmFic29sdXRlO3dpZHRoOicrZGl2LndpZHRoKCkrXCJweDtoZWlnaHQ6XCIrZGl2LmhlaWdodCgpK1wicHg7XCIrcG9zK1wiYmFja2dyb3VuZC1jb2xvcjpcIitjKyc7XCI+IDwvZGl2PicpLnByZXBlbmRUbyhsZWdlbmQpLmNzcyhcIm9wYWNpdHlcIixvcHRpb25zLmxlZ2VuZC5iYWNrZ3JvdW5kT3BhY2l0eSl9fX12YXIgaGlnaGxpZ2h0cz1bXSxyZWRyYXdUaW1lb3V0PW51bGw7ZnVuY3Rpb24gZmluZE5lYXJieUl0ZW0obW91c2VYLG1vdXNlWSxzZXJpZXNGaWx0ZXIpe3ZhciBtYXhEaXN0YW5jZT1vcHRpb25zLmdyaWQubW91c2VBY3RpdmVSYWRpdXMsc21hbGxlc3REaXN0YW5jZT1tYXhEaXN0YW5jZSptYXhEaXN0YW5jZSsxLGl0ZW09bnVsbCxmb3VuZFBvaW50PWZhbHNlLGksaixwcztmb3IoaT1zZXJpZXMubGVuZ3RoLTE7aT49MDstLWkpe2lmKCFzZXJpZXNGaWx0ZXIoc2VyaWVzW2ldKSljb250aW51ZTt2YXIgcz1zZXJpZXNbaV0sYXhpc3g9cy54YXhpcyxheGlzeT1zLnlheGlzLHBvaW50cz1zLmRhdGFwb2ludHMucG9pbnRzLG14PWF4aXN4LmMycChtb3VzZVgpLG15PWF4aXN5LmMycChtb3VzZVkpLG1heHg9bWF4RGlzdGFuY2UvYXhpc3guc2NhbGUsbWF4eT1tYXhEaXN0YW5jZS9heGlzeS5zY2FsZTtwcz1zLmRhdGFwb2ludHMucG9pbnRzaXplO2lmKGF4aXN4Lm9wdGlvbnMuaW52ZXJzZVRyYW5zZm9ybSltYXh4PU51bWJlci5NQVhfVkFMVUU7aWYoYXhpc3kub3B0aW9ucy5pbnZlcnNlVHJhbnNmb3JtKW1heHk9TnVtYmVyLk1BWF9WQUxVRTtpZihzLmxpbmVzLnNob3d8fHMucG9pbnRzLnNob3cpe2ZvcihqPTA7ajxwb2ludHMubGVuZ3RoO2orPXBzKXt2YXIgeD1wb2ludHNbal0seT1wb2ludHNbaisxXTtpZih4PT1udWxsKWNvbnRpbnVlO2lmKHgtbXg+bWF4eHx8eC1teDwtbWF4eHx8eS1teT5tYXh5fHx5LW15PC1tYXh5KWNvbnRpbnVlO3ZhciBkeD1NYXRoLmFicyhheGlzeC5wMmMoeCktbW91c2VYKSxkeT1NYXRoLmFicyhheGlzeS5wMmMoeSktbW91c2VZKSxkaXN0PWR4KmR4K2R5KmR5O2lmKGRpc3Q8c21hbGxlc3REaXN0YW5jZSl7c21hbGxlc3REaXN0YW5jZT1kaXN0O2l0ZW09W2ksai9wc119fX1pZihzLmJhcnMuc2hvdyYmIWl0ZW0pe3ZhciBiYXJMZWZ0LGJhclJpZ2h0O3N3aXRjaChzLmJhcnMuYWxpZ24pe2Nhc2VcImxlZnRcIjpiYXJMZWZ0PTA7YnJlYWs7Y2FzZVwicmlnaHRcIjpiYXJMZWZ0PS1zLmJhcnMuYmFyV2lkdGg7YnJlYWs7ZGVmYXVsdDpiYXJMZWZ0PS1zLmJhcnMuYmFyV2lkdGgvMn1iYXJSaWdodD1iYXJMZWZ0K3MuYmFycy5iYXJXaWR0aDtmb3Ioaj0wO2o8cG9pbnRzLmxlbmd0aDtqKz1wcyl7dmFyIHg9cG9pbnRzW2pdLHk9cG9pbnRzW2orMV0sYj1wb2ludHNbaisyXTtpZih4PT1udWxsKWNvbnRpbnVlO2lmKHNlcmllc1tpXS5iYXJzLmhvcml6b250YWw/bXg8PU1hdGgubWF4KGIseCkmJm14Pj1NYXRoLm1pbihiLHgpJiZteT49eStiYXJMZWZ0JiZteTw9eStiYXJSaWdodDpteD49eCtiYXJMZWZ0JiZteDw9eCtiYXJSaWdodCYmbXk+PU1hdGgubWluKGIseSkmJm15PD1NYXRoLm1heChiLHkpKWl0ZW09W2ksai9wc119fX1pZihpdGVtKXtpPWl0ZW1bMF07aj1pdGVtWzFdO3BzPXNlcmllc1tpXS5kYXRhcG9pbnRzLnBvaW50c2l6ZTtyZXR1cm57ZGF0YXBvaW50OnNlcmllc1tpXS5kYXRhcG9pbnRzLnBvaW50cy5zbGljZShqKnBzLChqKzEpKnBzKSxkYXRhSW5kZXg6aixzZXJpZXM6c2VyaWVzW2ldLHNlcmllc0luZGV4Oml9fXJldHVybiBudWxsfWZ1bmN0aW9uIG9uTW91c2VNb3ZlKGUpe2lmKG9wdGlvbnMuZ3JpZC5ob3ZlcmFibGUpdHJpZ2dlckNsaWNrSG92ZXJFdmVudChcInBsb3Rob3ZlclwiLGUsZnVuY3Rpb24ocyl7cmV0dXJuIHNbXCJob3ZlcmFibGVcIl0hPWZhbHNlfSl9ZnVuY3Rpb24gb25Nb3VzZUxlYXZlKGUpe2lmKG9wdGlvbnMuZ3JpZC5ob3ZlcmFibGUpdHJpZ2dlckNsaWNrSG92ZXJFdmVudChcInBsb3Rob3ZlclwiLGUsZnVuY3Rpb24ocyl7cmV0dXJuIGZhbHNlfSl9ZnVuY3Rpb24gb25DbGljayhlKXt0cmlnZ2VyQ2xpY2tIb3ZlckV2ZW50KFwicGxvdGNsaWNrXCIsZSxmdW5jdGlvbihzKXtyZXR1cm4gc1tcImNsaWNrYWJsZVwiXSE9ZmFsc2V9KX1mdW5jdGlvbiB0cmlnZ2VyQ2xpY2tIb3ZlckV2ZW50KGV2ZW50bmFtZSxldmVudCxzZXJpZXNGaWx0ZXIpe3ZhciBvZmZzZXQ9ZXZlbnRIb2xkZXIub2Zmc2V0KCksY2FudmFzWD1ldmVudC5wYWdlWC1vZmZzZXQubGVmdC1wbG90T2Zmc2V0LmxlZnQsY2FudmFzWT1ldmVudC5wYWdlWS1vZmZzZXQudG9wLXBsb3RPZmZzZXQudG9wLHBvcz1jYW52YXNUb0F4aXNDb29yZHMoe2xlZnQ6Y2FudmFzWCx0b3A6Y2FudmFzWX0pO3Bvcy5wYWdlWD1ldmVudC5wYWdlWDtwb3MucGFnZVk9ZXZlbnQucGFnZVk7dmFyIGl0ZW09ZmluZE5lYXJieUl0ZW0oY2FudmFzWCxjYW52YXNZLHNlcmllc0ZpbHRlcik7aWYoaXRlbSl7aXRlbS5wYWdlWD1wYXJzZUludChpdGVtLnNlcmllcy54YXhpcy5wMmMoaXRlbS5kYXRhcG9pbnRbMF0pK29mZnNldC5sZWZ0K3Bsb3RPZmZzZXQubGVmdCwxMCk7aXRlbS5wYWdlWT1wYXJzZUludChpdGVtLnNlcmllcy55YXhpcy5wMmMoaXRlbS5kYXRhcG9pbnRbMV0pK29mZnNldC50b3ArcGxvdE9mZnNldC50b3AsMTApfWlmKG9wdGlvbnMuZ3JpZC5hdXRvSGlnaGxpZ2h0KXtmb3IodmFyIGk9MDtpPGhpZ2hsaWdodHMubGVuZ3RoOysraSl7dmFyIGg9aGlnaGxpZ2h0c1tpXTtpZihoLmF1dG89PWV2ZW50bmFtZSYmIShpdGVtJiZoLnNlcmllcz09aXRlbS5zZXJpZXMmJmgucG9pbnRbMF09PWl0ZW0uZGF0YXBvaW50WzBdJiZoLnBvaW50WzFdPT1pdGVtLmRhdGFwb2ludFsxXSkpdW5oaWdobGlnaHQoaC5zZXJpZXMsaC5wb2ludCl9aWYoaXRlbSloaWdobGlnaHQoaXRlbS5zZXJpZXMsaXRlbS5kYXRhcG9pbnQsZXZlbnRuYW1lKX1wbGFjZWhvbGRlci50cmlnZ2VyKGV2ZW50bmFtZSxbcG9zLGl0ZW1dKX1mdW5jdGlvbiB0cmlnZ2VyUmVkcmF3T3ZlcmxheSgpe3ZhciB0PW9wdGlvbnMuaW50ZXJhY3Rpb24ucmVkcmF3T3ZlcmxheUludGVydmFsO2lmKHQ9PS0xKXtkcmF3T3ZlcmxheSgpO3JldHVybn1pZighcmVkcmF3VGltZW91dClyZWRyYXdUaW1lb3V0PXNldFRpbWVvdXQoZHJhd092ZXJsYXksdCl9ZnVuY3Rpb24gZHJhd092ZXJsYXkoKXtyZWRyYXdUaW1lb3V0PW51bGw7b2N0eC5zYXZlKCk7b3ZlcmxheS5jbGVhcigpO29jdHgudHJhbnNsYXRlKHBsb3RPZmZzZXQubGVmdCxwbG90T2Zmc2V0LnRvcCk7dmFyIGksaGk7Zm9yKGk9MDtpPGhpZ2hsaWdodHMubGVuZ3RoOysraSl7aGk9aGlnaGxpZ2h0c1tpXTtpZihoaS5zZXJpZXMuYmFycy5zaG93KWRyYXdCYXJIaWdobGlnaHQoaGkuc2VyaWVzLGhpLnBvaW50KTtlbHNlIGRyYXdQb2ludEhpZ2hsaWdodChoaS5zZXJpZXMsaGkucG9pbnQpfW9jdHgucmVzdG9yZSgpO2V4ZWN1dGVIb29rcyhob29rcy5kcmF3T3ZlcmxheSxbb2N0eF0pfWZ1bmN0aW9uIGhpZ2hsaWdodChzLHBvaW50LGF1dG8pe2lmKHR5cGVvZiBzPT1cIm51bWJlclwiKXM9c2VyaWVzW3NdO2lmKHR5cGVvZiBwb2ludD09XCJudW1iZXJcIil7dmFyIHBzPXMuZGF0YXBvaW50cy5wb2ludHNpemU7cG9pbnQ9cy5kYXRhcG9pbnRzLnBvaW50cy5zbGljZShwcypwb2ludCxwcyoocG9pbnQrMSkpfXZhciBpPWluZGV4T2ZIaWdobGlnaHQocyxwb2ludCk7aWYoaT09LTEpe2hpZ2hsaWdodHMucHVzaCh7c2VyaWVzOnMscG9pbnQ6cG9pbnQsYXV0bzphdXRvfSk7dHJpZ2dlclJlZHJhd092ZXJsYXkoKX1lbHNlIGlmKCFhdXRvKWhpZ2hsaWdodHNbaV0uYXV0bz1mYWxzZX1mdW5jdGlvbiB1bmhpZ2hsaWdodChzLHBvaW50KXtpZihzPT1udWxsJiZwb2ludD09bnVsbCl7aGlnaGxpZ2h0cz1bXTt0cmlnZ2VyUmVkcmF3T3ZlcmxheSgpO3JldHVybn1pZih0eXBlb2Ygcz09XCJudW1iZXJcIilzPXNlcmllc1tzXTtpZih0eXBlb2YgcG9pbnQ9PVwibnVtYmVyXCIpe3ZhciBwcz1zLmRhdGFwb2ludHMucG9pbnRzaXplO3BvaW50PXMuZGF0YXBvaW50cy5wb2ludHMuc2xpY2UocHMqcG9pbnQscHMqKHBvaW50KzEpKX12YXIgaT1pbmRleE9mSGlnaGxpZ2h0KHMscG9pbnQpO2lmKGkhPS0xKXtoaWdobGlnaHRzLnNwbGljZShpLDEpO3RyaWdnZXJSZWRyYXdPdmVybGF5KCl9fWZ1bmN0aW9uIGluZGV4T2ZIaWdobGlnaHQocyxwKXtmb3IodmFyIGk9MDtpPGhpZ2hsaWdodHMubGVuZ3RoOysraSl7dmFyIGg9aGlnaGxpZ2h0c1tpXTtpZihoLnNlcmllcz09cyYmaC5wb2ludFswXT09cFswXSYmaC5wb2ludFsxXT09cFsxXSlyZXR1cm4gaX1yZXR1cm4tMX1mdW5jdGlvbiBkcmF3UG9pbnRIaWdobGlnaHQoc2VyaWVzLHBvaW50KXt2YXIgeD1wb2ludFswXSx5PXBvaW50WzFdLGF4aXN4PXNlcmllcy54YXhpcyxheGlzeT1zZXJpZXMueWF4aXMsaGlnaGxpZ2h0Q29sb3I9dHlwZW9mIHNlcmllcy5oaWdobGlnaHRDb2xvcj09PVwic3RyaW5nXCI/c2VyaWVzLmhpZ2hsaWdodENvbG9yOiQuY29sb3IucGFyc2Uoc2VyaWVzLmNvbG9yKS5zY2FsZShcImFcIiwuNSkudG9TdHJpbmcoKTtpZih4PGF4aXN4Lm1pbnx8eD5heGlzeC5tYXh8fHk8YXhpc3kubWlufHx5PmF4aXN5Lm1heClyZXR1cm47dmFyIHBvaW50UmFkaXVzPXNlcmllcy5wb2ludHMucmFkaXVzK3Nlcmllcy5wb2ludHMubGluZVdpZHRoLzI7b2N0eC5saW5lV2lkdGg9cG9pbnRSYWRpdXM7b2N0eC5zdHJva2VTdHlsZT1oaWdobGlnaHRDb2xvcjt2YXIgcmFkaXVzPTEuNSpwb2ludFJhZGl1czt4PWF4aXN4LnAyYyh4KTt5PWF4aXN5LnAyYyh5KTtvY3R4LmJlZ2luUGF0aCgpO2lmKHNlcmllcy5wb2ludHMuc3ltYm9sPT1cImNpcmNsZVwiKW9jdHguYXJjKHgseSxyYWRpdXMsMCwyKk1hdGguUEksZmFsc2UpO2Vsc2Ugc2VyaWVzLnBvaW50cy5zeW1ib2wob2N0eCx4LHkscmFkaXVzLGZhbHNlKTtvY3R4LmNsb3NlUGF0aCgpO29jdHguc3Ryb2tlKCl9ZnVuY3Rpb24gZHJhd0JhckhpZ2hsaWdodChzZXJpZXMscG9pbnQpe3ZhciBoaWdobGlnaHRDb2xvcj10eXBlb2Ygc2VyaWVzLmhpZ2hsaWdodENvbG9yPT09XCJzdHJpbmdcIj9zZXJpZXMuaGlnaGxpZ2h0Q29sb3I6JC5jb2xvci5wYXJzZShzZXJpZXMuY29sb3IpLnNjYWxlKFwiYVwiLC41KS50b1N0cmluZygpLGZpbGxTdHlsZT1oaWdobGlnaHRDb2xvcixiYXJMZWZ0O3N3aXRjaChzZXJpZXMuYmFycy5hbGlnbil7Y2FzZVwibGVmdFwiOmJhckxlZnQ9MDticmVhaztjYXNlXCJyaWdodFwiOmJhckxlZnQ9LXNlcmllcy5iYXJzLmJhcldpZHRoO2JyZWFrO2RlZmF1bHQ6YmFyTGVmdD0tc2VyaWVzLmJhcnMuYmFyV2lkdGgvMn1vY3R4LmxpbmVXaWR0aD1zZXJpZXMuYmFycy5saW5lV2lkdGg7b2N0eC5zdHJva2VTdHlsZT1oaWdobGlnaHRDb2xvcjtkcmF3QmFyKHBvaW50WzBdLHBvaW50WzFdLHBvaW50WzJdfHwwLGJhckxlZnQsYmFyTGVmdCtzZXJpZXMuYmFycy5iYXJXaWR0aCxmdW5jdGlvbigpe3JldHVybiBmaWxsU3R5bGV9LHNlcmllcy54YXhpcyxzZXJpZXMueWF4aXMsb2N0eCxzZXJpZXMuYmFycy5ob3Jpem9udGFsLHNlcmllcy5iYXJzLmxpbmVXaWR0aCl9ZnVuY3Rpb24gZ2V0Q29sb3JPckdyYWRpZW50KHNwZWMsYm90dG9tLHRvcCxkZWZhdWx0Q29sb3Ipe2lmKHR5cGVvZiBzcGVjPT1cInN0cmluZ1wiKXJldHVybiBzcGVjO2Vsc2V7dmFyIGdyYWRpZW50PWN0eC5jcmVhdGVMaW5lYXJHcmFkaWVudCgwLHRvcCwwLGJvdHRvbSk7Zm9yKHZhciBpPTAsbD1zcGVjLmNvbG9ycy5sZW5ndGg7aTxsOysraSl7dmFyIGM9c3BlYy5jb2xvcnNbaV07aWYodHlwZW9mIGMhPVwic3RyaW5nXCIpe3ZhciBjbz0kLmNvbG9yLnBhcnNlKGRlZmF1bHRDb2xvcik7aWYoYy5icmlnaHRuZXNzIT1udWxsKWNvPWNvLnNjYWxlKFwicmdiXCIsYy5icmlnaHRuZXNzKTtpZihjLm9wYWNpdHkhPW51bGwpY28uYSo9Yy5vcGFjaXR5O2M9Y28udG9TdHJpbmcoKX1ncmFkaWVudC5hZGRDb2xvclN0b3AoaS8obC0xKSxjKX1yZXR1cm4gZ3JhZGllbnR9fX0kLnBsb3Q9ZnVuY3Rpb24ocGxhY2Vob2xkZXIsZGF0YSxvcHRpb25zKXt2YXIgcGxvdD1uZXcgUGxvdCgkKHBsYWNlaG9sZGVyKSxkYXRhLG9wdGlvbnMsJC5wbG90LnBsdWdpbnMpO3JldHVybiBwbG90fTskLnBsb3QudmVyc2lvbj1cIjAuOC4zXCI7JC5wbG90LnBsdWdpbnM9W107JC5mbi5wbG90PWZ1bmN0aW9uKGRhdGEsb3B0aW9ucyl7cmV0dXJuIHRoaXMuZWFjaChmdW5jdGlvbigpeyQucGxvdCh0aGlzLGRhdGEsb3B0aW9ucyl9KX07ZnVuY3Rpb24gZmxvb3JJbkJhc2UobixiYXNlKXtyZXR1cm4gYmFzZSpNYXRoLmZsb29yKG4vYmFzZSl9fSkoalF1ZXJ5KTsiLCIvKiBKYXZhc2NyaXB0IHBsb3R0aW5nIGxpYnJhcnkgZm9yIGpRdWVyeSwgdmVyc2lvbiAwLjguMy5cclxuXHJcbkNvcHlyaWdodCAoYykgMjAwNy0yMDE0IElPTEEgYW5kIE9sZSBMYXVyc2VuLlxyXG5MaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UuXHJcblxyXG4qL1xyXG4oZnVuY3Rpb24oYSl7ZnVuY3Rpb24gZShoKXt2YXIgayxqPXRoaXMsbD1oLmRhdGF8fHt9O2lmKGwuZWxlbSlqPWguZHJhZ1RhcmdldD1sLmVsZW0saC5kcmFnUHJveHk9ZC5wcm94eXx8aixoLmN1cnNvck9mZnNldFg9bC5wYWdlWC1sLmxlZnQsaC5jdXJzb3JPZmZzZXRZPWwucGFnZVktbC50b3AsaC5vZmZzZXRYPWgucGFnZVgtaC5jdXJzb3JPZmZzZXRYLGgub2Zmc2V0WT1oLnBhZ2VZLWguY3Vyc29yT2Zmc2V0WTtlbHNlIGlmKGQuZHJhZ2dpbmd8fGwud2hpY2g+MCYmaC53aGljaCE9bC53aGljaHx8YShoLnRhcmdldCkuaXMobC5ub3QpKXJldHVybjtzd2l0Y2goaC50eXBlKXtjYXNlXCJtb3VzZWRvd25cIjpyZXR1cm4gYS5leHRlbmQobCxhKGopLm9mZnNldCgpLHtlbGVtOmosdGFyZ2V0OmgudGFyZ2V0LHBhZ2VYOmgucGFnZVgscGFnZVk6aC5wYWdlWX0pLGIuYWRkKGRvY3VtZW50LFwibW91c2Vtb3ZlIG1vdXNldXBcIixlLGwpLGkoaiwhMSksZC5kcmFnZ2luZz1udWxsLCExO2Nhc2UhZC5kcmFnZ2luZyYmXCJtb3VzZW1vdmVcIjppZihnKGgucGFnZVgtbC5wYWdlWCkrZyhoLnBhZ2VZLWwucGFnZVkpPGwuZGlzdGFuY2UpYnJlYWs7aC50YXJnZXQ9bC50YXJnZXQsaz1mKGgsXCJkcmFnc3RhcnRcIixqKSxrIT09ITEmJihkLmRyYWdnaW5nPWosZC5wcm94eT1oLmRyYWdQcm94eT1hKGt8fGopWzBdKTtjYXNlXCJtb3VzZW1vdmVcIjppZihkLmRyYWdnaW5nKXtpZihrPWYoaCxcImRyYWdcIixqKSxjLmRyb3AmJihjLmRyb3AuYWxsb3dlZD1rIT09ITEsYy5kcm9wLmhhbmRsZXIoaCkpLGshPT0hMSlicmVhaztoLnR5cGU9XCJtb3VzZXVwXCJ9Y2FzZVwibW91c2V1cFwiOmIucmVtb3ZlKGRvY3VtZW50LFwibW91c2Vtb3ZlIG1vdXNldXBcIixlKSxkLmRyYWdnaW5nJiYoYy5kcm9wJiZjLmRyb3AuaGFuZGxlcihoKSxmKGgsXCJkcmFnZW5kXCIsaikpLGkoaiwhMCksZC5kcmFnZ2luZz1kLnByb3h5PWwuZWxlbT0hMX1yZXR1cm4hMH1mdW5jdGlvbiBmKGIsYyxkKXtiLnR5cGU9Yzt2YXIgZT1hLmV2ZW50LmRpc3BhdGNoLmNhbGwoZCxiKTtyZXR1cm4gZT09PSExPyExOmV8fGIucmVzdWx0fWZ1bmN0aW9uIGcoYSl7cmV0dXJuIE1hdGgucG93KGEsMil9ZnVuY3Rpb24gaCgpe3JldHVybiBkLmRyYWdnaW5nPT09ITF9ZnVuY3Rpb24gaShhLGIpe2EmJihhLnVuc2VsZWN0YWJsZT1iP1wib2ZmXCI6XCJvblwiLGEub25zZWxlY3RzdGFydD1mdW5jdGlvbigpe3JldHVybiBifSxhLnN0eWxlJiYoYS5zdHlsZS5Nb3pVc2VyU2VsZWN0PWI/XCJcIjpcIm5vbmVcIikpfWEuZm4uZHJhZz1mdW5jdGlvbihhLGIsYyl7cmV0dXJuIGImJnRoaXMuYmluZChcImRyYWdzdGFydFwiLGEpLGMmJnRoaXMuYmluZChcImRyYWdlbmRcIixjKSxhP3RoaXMuYmluZChcImRyYWdcIixiP2I6YSk6dGhpcy50cmlnZ2VyKFwiZHJhZ1wiKX07dmFyIGI9YS5ldmVudCxjPWIuc3BlY2lhbCxkPWMuZHJhZz17bm90OlwiOmlucHV0XCIsZGlzdGFuY2U6MCx3aGljaDoxLGRyYWdnaW5nOiExLHNldHVwOmZ1bmN0aW9uKGMpe2M9YS5leHRlbmQoe2Rpc3RhbmNlOmQuZGlzdGFuY2Usd2hpY2g6ZC53aGljaCxub3Q6ZC5ub3R9LGN8fHt9KSxjLmRpc3RhbmNlPWcoYy5kaXN0YW5jZSksYi5hZGQodGhpcyxcIm1vdXNlZG93blwiLGUsYyksdGhpcy5hdHRhY2hFdmVudCYmdGhpcy5hdHRhY2hFdmVudChcIm9uZHJhZ3N0YXJ0XCIsaCl9LHRlYXJkb3duOmZ1bmN0aW9uKCl7Yi5yZW1vdmUodGhpcyxcIm1vdXNlZG93blwiLGUpLHRoaXM9PT1kLmRyYWdnaW5nJiYoZC5kcmFnZ2luZz1kLnByb3h5PSExKSxpKHRoaXMsITApLHRoaXMuZGV0YWNoRXZlbnQmJnRoaXMuZGV0YWNoRXZlbnQoXCJvbmRyYWdzdGFydFwiLGgpfX07Yy5kcmFnc3RhcnQ9Yy5kcmFnZW5kPXtzZXR1cDpmdW5jdGlvbigpe30sdGVhcmRvd246ZnVuY3Rpb24oKXt9fX0pKGpRdWVyeSk7KGZ1bmN0aW9uKGQpe2Z1bmN0aW9uIGUoYSl7dmFyIGI9YXx8d2luZG93LmV2ZW50LGM9W10uc2xpY2UuY2FsbChhcmd1bWVudHMsMSksZj0wLGU9MCxnPTAsYT1kLmV2ZW50LmZpeChiKTthLnR5cGU9XCJtb3VzZXdoZWVsXCI7Yi53aGVlbERlbHRhJiYoZj1iLndoZWVsRGVsdGEvMTIwKTtiLmRldGFpbCYmKGY9LWIuZGV0YWlsLzMpO2c9Zjt2b2lkIDAhPT1iLmF4aXMmJmIuYXhpcz09PWIuSE9SSVpPTlRBTF9BWElTJiYoZz0wLGU9LTEqZik7dm9pZCAwIT09Yi53aGVlbERlbHRhWSYmKGc9Yi53aGVlbERlbHRhWS8xMjApO3ZvaWQgMCE9PWIud2hlZWxEZWx0YVgmJihlPS0xKmIud2hlZWxEZWx0YVgvMTIwKTtjLnVuc2hpZnQoYSxmLGUsZyk7cmV0dXJuKGQuZXZlbnQuZGlzcGF0Y2h8fGQuZXZlbnQuaGFuZGxlKS5hcHBseSh0aGlzLGMpfXZhciBjPVtcIkRPTU1vdXNlU2Nyb2xsXCIsXCJtb3VzZXdoZWVsXCJdO2lmKGQuZXZlbnQuZml4SG9va3MpZm9yKHZhciBoPWMubGVuZ3RoO2g7KWQuZXZlbnQuZml4SG9va3NbY1stLWhdXT1kLmV2ZW50Lm1vdXNlSG9va3M7ZC5ldmVudC5zcGVjaWFsLm1vdXNld2hlZWw9e3NldHVwOmZ1bmN0aW9uKCl7aWYodGhpcy5hZGRFdmVudExpc3RlbmVyKWZvcih2YXIgYT1jLmxlbmd0aDthOyl0aGlzLmFkZEV2ZW50TGlzdGVuZXIoY1stLWFdLGUsITEpO2Vsc2UgdGhpcy5vbm1vdXNld2hlZWw9ZX0sdGVhcmRvd246ZnVuY3Rpb24oKXtpZih0aGlzLnJlbW92ZUV2ZW50TGlzdGVuZXIpZm9yKHZhciBhPWMubGVuZ3RoO2E7KXRoaXMucmVtb3ZlRXZlbnRMaXN0ZW5lcihjWy0tYV0sZSwhMSk7ZWxzZSB0aGlzLm9ubW91c2V3aGVlbD1udWxsfX07ZC5mbi5leHRlbmQoe21vdXNld2hlZWw6ZnVuY3Rpb24oYSl7cmV0dXJuIGE/dGhpcy5iaW5kKFwibW91c2V3aGVlbFwiLGEpOnRoaXMudHJpZ2dlcihcIm1vdXNld2hlZWxcIil9LHVubW91c2V3aGVlbDpmdW5jdGlvbihhKXtyZXR1cm4gdGhpcy51bmJpbmQoXCJtb3VzZXdoZWVsXCIsYSl9fSl9KShqUXVlcnkpOyhmdW5jdGlvbigkKXt2YXIgb3B0aW9ucz17eGF4aXM6e3pvb21SYW5nZTpudWxsLHBhblJhbmdlOm51bGx9LHpvb206e2ludGVyYWN0aXZlOmZhbHNlLHRyaWdnZXI6XCJkYmxjbGlja1wiLGFtb3VudDoxLjV9LHBhbjp7aW50ZXJhY3RpdmU6ZmFsc2UsY3Vyc29yOlwibW92ZVwiLGZyYW1lUmF0ZToyMH19O2Z1bmN0aW9uIGluaXQocGxvdCl7ZnVuY3Rpb24gb25ab29tQ2xpY2soZSx6b29tT3V0KXt2YXIgYz1wbG90Lm9mZnNldCgpO2MubGVmdD1lLnBhZ2VYLWMubGVmdDtjLnRvcD1lLnBhZ2VZLWMudG9wO2lmKHpvb21PdXQpcGxvdC56b29tT3V0KHtjZW50ZXI6Y30pO2Vsc2UgcGxvdC56b29tKHtjZW50ZXI6Y30pfWZ1bmN0aW9uIG9uTW91c2VXaGVlbChlLGRlbHRhKXtlLnByZXZlbnREZWZhdWx0KCk7b25ab29tQ2xpY2soZSxkZWx0YTwwKTtyZXR1cm4gZmFsc2V9dmFyIHByZXZDdXJzb3I9XCJkZWZhdWx0XCIscHJldlBhZ2VYPTAscHJldlBhZ2VZPTAscGFuVGltZW91dD1udWxsO2Z1bmN0aW9uIG9uRHJhZ1N0YXJ0KGUpe2lmKGUud2hpY2ghPTEpcmV0dXJuIGZhbHNlO3ZhciBjPXBsb3QuZ2V0UGxhY2Vob2xkZXIoKS5jc3MoXCJjdXJzb3JcIik7aWYoYylwcmV2Q3Vyc29yPWM7cGxvdC5nZXRQbGFjZWhvbGRlcigpLmNzcyhcImN1cnNvclwiLHBsb3QuZ2V0T3B0aW9ucygpLnBhbi5jdXJzb3IpO3ByZXZQYWdlWD1lLnBhZ2VYO3ByZXZQYWdlWT1lLnBhZ2VZfWZ1bmN0aW9uIG9uRHJhZyhlKXt2YXIgZnJhbWVSYXRlPXBsb3QuZ2V0T3B0aW9ucygpLnBhbi5mcmFtZVJhdGU7aWYocGFuVGltZW91dHx8IWZyYW1lUmF0ZSlyZXR1cm47cGFuVGltZW91dD1zZXRUaW1lb3V0KGZ1bmN0aW9uKCl7cGxvdC5wYW4oe2xlZnQ6cHJldlBhZ2VYLWUucGFnZVgsdG9wOnByZXZQYWdlWS1lLnBhZ2VZfSk7cHJldlBhZ2VYPWUucGFnZVg7cHJldlBhZ2VZPWUucGFnZVk7cGFuVGltZW91dD1udWxsfSwxL2ZyYW1lUmF0ZSoxZTMpfWZ1bmN0aW9uIG9uRHJhZ0VuZChlKXtpZihwYW5UaW1lb3V0KXtjbGVhclRpbWVvdXQocGFuVGltZW91dCk7cGFuVGltZW91dD1udWxsfXBsb3QuZ2V0UGxhY2Vob2xkZXIoKS5jc3MoXCJjdXJzb3JcIixwcmV2Q3Vyc29yKTtwbG90LnBhbih7bGVmdDpwcmV2UGFnZVgtZS5wYWdlWCx0b3A6cHJldlBhZ2VZLWUucGFnZVl9KX1mdW5jdGlvbiBiaW5kRXZlbnRzKHBsb3QsZXZlbnRIb2xkZXIpe3ZhciBvPXBsb3QuZ2V0T3B0aW9ucygpO2lmKG8uem9vbS5pbnRlcmFjdGl2ZSl7ZXZlbnRIb2xkZXJbby56b29tLnRyaWdnZXJdKG9uWm9vbUNsaWNrKTtldmVudEhvbGRlci5tb3VzZXdoZWVsKG9uTW91c2VXaGVlbCl9aWYoby5wYW4uaW50ZXJhY3RpdmUpe2V2ZW50SG9sZGVyLmJpbmQoXCJkcmFnc3RhcnRcIix7ZGlzdGFuY2U6MTB9LG9uRHJhZ1N0YXJ0KTtldmVudEhvbGRlci5iaW5kKFwiZHJhZ1wiLG9uRHJhZyk7ZXZlbnRIb2xkZXIuYmluZChcImRyYWdlbmRcIixvbkRyYWdFbmQpfX1wbG90Lnpvb21PdXQ9ZnVuY3Rpb24oYXJncyl7aWYoIWFyZ3MpYXJncz17fTtpZighYXJncy5hbW91bnQpYXJncy5hbW91bnQ9cGxvdC5nZXRPcHRpb25zKCkuem9vbS5hbW91bnQ7YXJncy5hbW91bnQ9MS9hcmdzLmFtb3VudDtwbG90Lnpvb20oYXJncyl9O3Bsb3Quem9vbT1mdW5jdGlvbihhcmdzKXtpZighYXJncylhcmdzPXt9O3ZhciBjPWFyZ3MuY2VudGVyLGFtb3VudD1hcmdzLmFtb3VudHx8cGxvdC5nZXRPcHRpb25zKCkuem9vbS5hbW91bnQsdz1wbG90LndpZHRoKCksaD1wbG90LmhlaWdodCgpO2lmKCFjKWM9e2xlZnQ6dy8yLHRvcDpoLzJ9O3ZhciB4Zj1jLmxlZnQvdyx5Zj1jLnRvcC9oLG1pbm1heD17eDp7bWluOmMubGVmdC14Zip3L2Ftb3VudCxtYXg6Yy5sZWZ0KygxLXhmKSp3L2Ftb3VudH0seTp7bWluOmMudG9wLXlmKmgvYW1vdW50LG1heDpjLnRvcCsoMS15ZikqaC9hbW91bnR9fTskLmVhY2gocGxvdC5nZXRBeGVzKCksZnVuY3Rpb24oXyxheGlzKXt2YXIgb3B0cz1heGlzLm9wdGlvbnMsbWluPW1pbm1heFtheGlzLmRpcmVjdGlvbl0ubWluLG1heD1taW5tYXhbYXhpcy5kaXJlY3Rpb25dLm1heCx6cj1vcHRzLnpvb21SYW5nZSxwcj1vcHRzLnBhblJhbmdlO2lmKHpyPT09ZmFsc2UpcmV0dXJuO21pbj1heGlzLmMycChtaW4pO21heD1heGlzLmMycChtYXgpO2lmKG1pbj5tYXgpe3ZhciB0bXA9bWluO21pbj1tYXg7bWF4PXRtcH1pZihwcil7aWYocHJbMF0hPW51bGwmJm1pbjxwclswXSl7bWluPXByWzBdfWlmKHByWzFdIT1udWxsJiZtYXg+cHJbMV0pe21heD1wclsxXX19dmFyIHJhbmdlPW1heC1taW47aWYoenImJih6clswXSE9bnVsbCYmcmFuZ2U8enJbMF0mJmFtb3VudD4xfHx6clsxXSE9bnVsbCYmcmFuZ2U+enJbMV0mJmFtb3VudDwxKSlyZXR1cm47b3B0cy5taW49bWluO29wdHMubWF4PW1heH0pO3Bsb3Quc2V0dXBHcmlkKCk7cGxvdC5kcmF3KCk7aWYoIWFyZ3MucHJldmVudEV2ZW50KXBsb3QuZ2V0UGxhY2Vob2xkZXIoKS50cmlnZ2VyKFwicGxvdHpvb21cIixbcGxvdCxhcmdzXSl9O3Bsb3QucGFuPWZ1bmN0aW9uKGFyZ3Mpe3ZhciBkZWx0YT17eDorYXJncy5sZWZ0LHk6K2FyZ3MudG9wfTtpZihpc05hTihkZWx0YS54KSlkZWx0YS54PTA7aWYoaXNOYU4oZGVsdGEueSkpZGVsdGEueT0wOyQuZWFjaChwbG90LmdldEF4ZXMoKSxmdW5jdGlvbihfLGF4aXMpe3ZhciBvcHRzPWF4aXMub3B0aW9ucyxtaW4sbWF4LGQ9ZGVsdGFbYXhpcy5kaXJlY3Rpb25dO21pbj1heGlzLmMycChheGlzLnAyYyhheGlzLm1pbikrZCksbWF4PWF4aXMuYzJwKGF4aXMucDJjKGF4aXMubWF4KStkKTt2YXIgcHI9b3B0cy5wYW5SYW5nZTtpZihwcj09PWZhbHNlKXJldHVybjtpZihwcil7aWYocHJbMF0hPW51bGwmJnByWzBdPm1pbil7ZD1wclswXS1taW47bWluKz1kO21heCs9ZH1pZihwclsxXSE9bnVsbCYmcHJbMV08bWF4KXtkPXByWzFdLW1heDttaW4rPWQ7bWF4Kz1kfX1vcHRzLm1pbj1taW47b3B0cy5tYXg9bWF4fSk7cGxvdC5zZXR1cEdyaWQoKTtwbG90LmRyYXcoKTtpZighYXJncy5wcmV2ZW50RXZlbnQpcGxvdC5nZXRQbGFjZWhvbGRlcigpLnRyaWdnZXIoXCJwbG90cGFuXCIsW3Bsb3QsYXJnc10pfTtmdW5jdGlvbiBzaHV0ZG93bihwbG90LGV2ZW50SG9sZGVyKXtldmVudEhvbGRlci51bmJpbmQocGxvdC5nZXRPcHRpb25zKCkuem9vbS50cmlnZ2VyLG9uWm9vbUNsaWNrKTtldmVudEhvbGRlci51bmJpbmQoXCJtb3VzZXdoZWVsXCIsb25Nb3VzZVdoZWVsKTtldmVudEhvbGRlci51bmJpbmQoXCJkcmFnc3RhcnRcIixvbkRyYWdTdGFydCk7ZXZlbnRIb2xkZXIudW5iaW5kKFwiZHJhZ1wiLG9uRHJhZyk7ZXZlbnRIb2xkZXIudW5iaW5kKFwiZHJhZ2VuZFwiLG9uRHJhZ0VuZCk7aWYocGFuVGltZW91dCljbGVhclRpbWVvdXQocGFuVGltZW91dCl9cGxvdC5ob29rcy5iaW5kRXZlbnRzLnB1c2goYmluZEV2ZW50cyk7cGxvdC5ob29rcy5zaHV0ZG93bi5wdXNoKHNodXRkb3duKX0kLnBsb3QucGx1Z2lucy5wdXNoKHtpbml0OmluaXQsb3B0aW9uczpvcHRpb25zLG5hbWU6XCJuYXZpZ2F0ZVwiLHZlcnNpb246XCIxLjNcIn0pfSkoalF1ZXJ5KTsiLCIvKiBKYXZhc2NyaXB0IHBsb3R0aW5nIGxpYnJhcnkgZm9yIGpRdWVyeSwgdmVyc2lvbiAwLjguMy5cclxuXHJcbkNvcHlyaWdodCAoYykgMjAwNy0yMDE0IElPTEEgYW5kIE9sZSBMYXVyc2VuLlxyXG5MaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UuXHJcblxyXG4qL1xyXG4oZnVuY3Rpb24oJCl7ZnVuY3Rpb24gaW5pdChwbG90KXt2YXIgc2VsZWN0aW9uPXtmaXJzdDp7eDotMSx5Oi0xfSxzZWNvbmQ6e3g6LTEseTotMX0sc2hvdzpmYWxzZSxhY3RpdmU6ZmFsc2V9O3ZhciBzYXZlZGhhbmRsZXJzPXt9O3ZhciBtb3VzZVVwSGFuZGxlcj1udWxsO2Z1bmN0aW9uIG9uTW91c2VNb3ZlKGUpe2lmKHNlbGVjdGlvbi5hY3RpdmUpe3VwZGF0ZVNlbGVjdGlvbihlKTtwbG90LmdldFBsYWNlaG9sZGVyKCkudHJpZ2dlcihcInBsb3RzZWxlY3RpbmdcIixbZ2V0U2VsZWN0aW9uKCldKX19ZnVuY3Rpb24gb25Nb3VzZURvd24oZSl7aWYoZS53aGljaCE9MSlyZXR1cm47ZG9jdW1lbnQuYm9keS5mb2N1cygpO2lmKGRvY3VtZW50Lm9uc2VsZWN0c3RhcnQhPT11bmRlZmluZWQmJnNhdmVkaGFuZGxlcnMub25zZWxlY3RzdGFydD09bnVsbCl7c2F2ZWRoYW5kbGVycy5vbnNlbGVjdHN0YXJ0PWRvY3VtZW50Lm9uc2VsZWN0c3RhcnQ7ZG9jdW1lbnQub25zZWxlY3RzdGFydD1mdW5jdGlvbigpe3JldHVybiBmYWxzZX19aWYoZG9jdW1lbnQub25kcmFnIT09dW5kZWZpbmVkJiZzYXZlZGhhbmRsZXJzLm9uZHJhZz09bnVsbCl7c2F2ZWRoYW5kbGVycy5vbmRyYWc9ZG9jdW1lbnQub25kcmFnO2RvY3VtZW50Lm9uZHJhZz1mdW5jdGlvbigpe3JldHVybiBmYWxzZX19c2V0U2VsZWN0aW9uUG9zKHNlbGVjdGlvbi5maXJzdCxlKTtzZWxlY3Rpb24uYWN0aXZlPXRydWU7bW91c2VVcEhhbmRsZXI9ZnVuY3Rpb24oZSl7b25Nb3VzZVVwKGUpfTskKGRvY3VtZW50KS5vbmUoXCJtb3VzZXVwXCIsbW91c2VVcEhhbmRsZXIpfWZ1bmN0aW9uIG9uTW91c2VVcChlKXttb3VzZVVwSGFuZGxlcj1udWxsO2lmKGRvY3VtZW50Lm9uc2VsZWN0c3RhcnQhPT11bmRlZmluZWQpZG9jdW1lbnQub25zZWxlY3RzdGFydD1zYXZlZGhhbmRsZXJzLm9uc2VsZWN0c3RhcnQ7aWYoZG9jdW1lbnQub25kcmFnIT09dW5kZWZpbmVkKWRvY3VtZW50Lm9uZHJhZz1zYXZlZGhhbmRsZXJzLm9uZHJhZztzZWxlY3Rpb24uYWN0aXZlPWZhbHNlO3VwZGF0ZVNlbGVjdGlvbihlKTtpZihzZWxlY3Rpb25Jc1NhbmUoKSl0cmlnZ2VyU2VsZWN0ZWRFdmVudCgpO2Vsc2V7cGxvdC5nZXRQbGFjZWhvbGRlcigpLnRyaWdnZXIoXCJwbG90dW5zZWxlY3RlZFwiLFtdKTtwbG90LmdldFBsYWNlaG9sZGVyKCkudHJpZ2dlcihcInBsb3RzZWxlY3RpbmdcIixbbnVsbF0pfXJldHVybiBmYWxzZX1mdW5jdGlvbiBnZXRTZWxlY3Rpb24oKXtpZighc2VsZWN0aW9uSXNTYW5lKCkpcmV0dXJuIG51bGw7aWYoIXNlbGVjdGlvbi5zaG93KXJldHVybiBudWxsO3ZhciByPXt9LGMxPXNlbGVjdGlvbi5maXJzdCxjMj1zZWxlY3Rpb24uc2Vjb25kOyQuZWFjaChwbG90LmdldEF4ZXMoKSxmdW5jdGlvbihuYW1lLGF4aXMpe2lmKGF4aXMudXNlZCl7dmFyIHAxPWF4aXMuYzJwKGMxW2F4aXMuZGlyZWN0aW9uXSkscDI9YXhpcy5jMnAoYzJbYXhpcy5kaXJlY3Rpb25dKTtyW25hbWVdPXtmcm9tOk1hdGgubWluKHAxLHAyKSx0bzpNYXRoLm1heChwMSxwMil9fX0pO3JldHVybiByfWZ1bmN0aW9uIHRyaWdnZXJTZWxlY3RlZEV2ZW50KCl7dmFyIHI9Z2V0U2VsZWN0aW9uKCk7cGxvdC5nZXRQbGFjZWhvbGRlcigpLnRyaWdnZXIoXCJwbG90c2VsZWN0ZWRcIixbcl0pO2lmKHIueGF4aXMmJnIueWF4aXMpcGxvdC5nZXRQbGFjZWhvbGRlcigpLnRyaWdnZXIoXCJzZWxlY3RlZFwiLFt7eDE6ci54YXhpcy5mcm9tLHkxOnIueWF4aXMuZnJvbSx4MjpyLnhheGlzLnRvLHkyOnIueWF4aXMudG99XSl9ZnVuY3Rpb24gY2xhbXAobWluLHZhbHVlLG1heCl7cmV0dXJuIHZhbHVlPG1pbj9taW46dmFsdWU+bWF4P21heDp2YWx1ZX1mdW5jdGlvbiBzZXRTZWxlY3Rpb25Qb3MocG9zLGUpe3ZhciBvPXBsb3QuZ2V0T3B0aW9ucygpO3ZhciBvZmZzZXQ9cGxvdC5nZXRQbGFjZWhvbGRlcigpLm9mZnNldCgpO3ZhciBwbG90T2Zmc2V0PXBsb3QuZ2V0UGxvdE9mZnNldCgpO3Bvcy54PWNsYW1wKDAsZS5wYWdlWC1vZmZzZXQubGVmdC1wbG90T2Zmc2V0LmxlZnQscGxvdC53aWR0aCgpKTtwb3MueT1jbGFtcCgwLGUucGFnZVktb2Zmc2V0LnRvcC1wbG90T2Zmc2V0LnRvcCxwbG90LmhlaWdodCgpKTtpZihvLnNlbGVjdGlvbi5tb2RlPT1cInlcIilwb3MueD1wb3M9PXNlbGVjdGlvbi5maXJzdD8wOnBsb3Qud2lkdGgoKTtpZihvLnNlbGVjdGlvbi5tb2RlPT1cInhcIilwb3MueT1wb3M9PXNlbGVjdGlvbi5maXJzdD8wOnBsb3QuaGVpZ2h0KCl9ZnVuY3Rpb24gdXBkYXRlU2VsZWN0aW9uKHBvcyl7aWYocG9zLnBhZ2VYPT1udWxsKXJldHVybjtzZXRTZWxlY3Rpb25Qb3Moc2VsZWN0aW9uLnNlY29uZCxwb3MpO2lmKHNlbGVjdGlvbklzU2FuZSgpKXtzZWxlY3Rpb24uc2hvdz10cnVlO3Bsb3QudHJpZ2dlclJlZHJhd092ZXJsYXkoKX1lbHNlIGNsZWFyU2VsZWN0aW9uKHRydWUpfWZ1bmN0aW9uIGNsZWFyU2VsZWN0aW9uKHByZXZlbnRFdmVudCl7aWYoc2VsZWN0aW9uLnNob3cpe3NlbGVjdGlvbi5zaG93PWZhbHNlO3Bsb3QudHJpZ2dlclJlZHJhd092ZXJsYXkoKTtpZighcHJldmVudEV2ZW50KXBsb3QuZ2V0UGxhY2Vob2xkZXIoKS50cmlnZ2VyKFwicGxvdHVuc2VsZWN0ZWRcIixbXSl9fWZ1bmN0aW9uIGV4dHJhY3RSYW5nZShyYW5nZXMsY29vcmQpe3ZhciBheGlzLGZyb20sdG8sa2V5LGF4ZXM9cGxvdC5nZXRBeGVzKCk7Zm9yKHZhciBrIGluIGF4ZXMpe2F4aXM9YXhlc1trXTtpZihheGlzLmRpcmVjdGlvbj09Y29vcmQpe2tleT1jb29yZCtheGlzLm4rXCJheGlzXCI7aWYoIXJhbmdlc1trZXldJiZheGlzLm49PTEpa2V5PWNvb3JkK1wiYXhpc1wiO2lmKHJhbmdlc1trZXldKXtmcm9tPXJhbmdlc1trZXldLmZyb207dG89cmFuZ2VzW2tleV0udG87YnJlYWt9fX1pZighcmFuZ2VzW2tleV0pe2F4aXM9Y29vcmQ9PVwieFwiP3Bsb3QuZ2V0WEF4ZXMoKVswXTpwbG90LmdldFlBeGVzKClbMF07ZnJvbT1yYW5nZXNbY29vcmQrXCIxXCJdO3RvPXJhbmdlc1tjb29yZCtcIjJcIl19aWYoZnJvbSE9bnVsbCYmdG8hPW51bGwmJmZyb20+dG8pe3ZhciB0bXA9ZnJvbTtmcm9tPXRvO3RvPXRtcH1yZXR1cm57ZnJvbTpmcm9tLHRvOnRvLGF4aXM6YXhpc319ZnVuY3Rpb24gc2V0U2VsZWN0aW9uKHJhbmdlcyxwcmV2ZW50RXZlbnQpe3ZhciBheGlzLHJhbmdlLG89cGxvdC5nZXRPcHRpb25zKCk7aWYoby5zZWxlY3Rpb24ubW9kZT09XCJ5XCIpe3NlbGVjdGlvbi5maXJzdC54PTA7c2VsZWN0aW9uLnNlY29uZC54PXBsb3Qud2lkdGgoKX1lbHNle3JhbmdlPWV4dHJhY3RSYW5nZShyYW5nZXMsXCJ4XCIpO3NlbGVjdGlvbi5maXJzdC54PXJhbmdlLmF4aXMucDJjKHJhbmdlLmZyb20pO3NlbGVjdGlvbi5zZWNvbmQueD1yYW5nZS5heGlzLnAyYyhyYW5nZS50byl9aWYoby5zZWxlY3Rpb24ubW9kZT09XCJ4XCIpe3NlbGVjdGlvbi5maXJzdC55PTA7c2VsZWN0aW9uLnNlY29uZC55PXBsb3QuaGVpZ2h0KCl9ZWxzZXtyYW5nZT1leHRyYWN0UmFuZ2UocmFuZ2VzLFwieVwiKTtzZWxlY3Rpb24uZmlyc3QueT1yYW5nZS5heGlzLnAyYyhyYW5nZS5mcm9tKTtzZWxlY3Rpb24uc2Vjb25kLnk9cmFuZ2UuYXhpcy5wMmMocmFuZ2UudG8pfXNlbGVjdGlvbi5zaG93PXRydWU7cGxvdC50cmlnZ2VyUmVkcmF3T3ZlcmxheSgpO2lmKCFwcmV2ZW50RXZlbnQmJnNlbGVjdGlvbklzU2FuZSgpKXRyaWdnZXJTZWxlY3RlZEV2ZW50KCl9ZnVuY3Rpb24gc2VsZWN0aW9uSXNTYW5lKCl7dmFyIG1pblNpemU9cGxvdC5nZXRPcHRpb25zKCkuc2VsZWN0aW9uLm1pblNpemU7cmV0dXJuIE1hdGguYWJzKHNlbGVjdGlvbi5zZWNvbmQueC1zZWxlY3Rpb24uZmlyc3QueCk+PW1pblNpemUmJk1hdGguYWJzKHNlbGVjdGlvbi5zZWNvbmQueS1zZWxlY3Rpb24uZmlyc3QueSk+PW1pblNpemV9cGxvdC5jbGVhclNlbGVjdGlvbj1jbGVhclNlbGVjdGlvbjtwbG90LnNldFNlbGVjdGlvbj1zZXRTZWxlY3Rpb247cGxvdC5nZXRTZWxlY3Rpb249Z2V0U2VsZWN0aW9uO3Bsb3QuaG9va3MuYmluZEV2ZW50cy5wdXNoKGZ1bmN0aW9uKHBsb3QsZXZlbnRIb2xkZXIpe3ZhciBvPXBsb3QuZ2V0T3B0aW9ucygpO2lmKG8uc2VsZWN0aW9uLm1vZGUhPW51bGwpe2V2ZW50SG9sZGVyLm1vdXNlbW92ZShvbk1vdXNlTW92ZSk7ZXZlbnRIb2xkZXIubW91c2Vkb3duKG9uTW91c2VEb3duKX19KTtwbG90Lmhvb2tzLmRyYXdPdmVybGF5LnB1c2goZnVuY3Rpb24ocGxvdCxjdHgpe2lmKHNlbGVjdGlvbi5zaG93JiZzZWxlY3Rpb25Jc1NhbmUoKSl7dmFyIHBsb3RPZmZzZXQ9cGxvdC5nZXRQbG90T2Zmc2V0KCk7dmFyIG89cGxvdC5nZXRPcHRpb25zKCk7Y3R4LnNhdmUoKTtjdHgudHJhbnNsYXRlKHBsb3RPZmZzZXQubGVmdCxwbG90T2Zmc2V0LnRvcCk7dmFyIGM9JC5jb2xvci5wYXJzZShvLnNlbGVjdGlvbi5jb2xvcik7Y3R4LnN0cm9rZVN0eWxlPWMuc2NhbGUoXCJhXCIsLjgpLnRvU3RyaW5nKCk7Y3R4LmxpbmVXaWR0aD0xO2N0eC5saW5lSm9pbj1vLnNlbGVjdGlvbi5zaGFwZTtjdHguZmlsbFN0eWxlPWMuc2NhbGUoXCJhXCIsLjQpLnRvU3RyaW5nKCk7dmFyIHg9TWF0aC5taW4oc2VsZWN0aW9uLmZpcnN0Lngsc2VsZWN0aW9uLnNlY29uZC54KSsuNSx5PU1hdGgubWluKHNlbGVjdGlvbi5maXJzdC55LHNlbGVjdGlvbi5zZWNvbmQueSkrLjUsdz1NYXRoLmFicyhzZWxlY3Rpb24uc2Vjb25kLngtc2VsZWN0aW9uLmZpcnN0LngpLTEsaD1NYXRoLmFicyhzZWxlY3Rpb24uc2Vjb25kLnktc2VsZWN0aW9uLmZpcnN0LnkpLTE7Y3R4LmZpbGxSZWN0KHgseSx3LGgpO2N0eC5zdHJva2VSZWN0KHgseSx3LGgpO2N0eC5yZXN0b3JlKCl9fSk7cGxvdC5ob29rcy5zaHV0ZG93bi5wdXNoKGZ1bmN0aW9uKHBsb3QsZXZlbnRIb2xkZXIpe2V2ZW50SG9sZGVyLnVuYmluZChcIm1vdXNlbW92ZVwiLG9uTW91c2VNb3ZlKTtldmVudEhvbGRlci51bmJpbmQoXCJtb3VzZWRvd25cIixvbk1vdXNlRG93bik7aWYobW91c2VVcEhhbmRsZXIpJChkb2N1bWVudCkudW5iaW5kKFwibW91c2V1cFwiLG1vdXNlVXBIYW5kbGVyKX0pfSQucGxvdC5wbHVnaW5zLnB1c2goe2luaXQ6aW5pdCxvcHRpb25zOntzZWxlY3Rpb246e21vZGU6bnVsbCxjb2xvcjpcIiNlOGNmYWNcIixzaGFwZTpcInJvdW5kXCIsbWluU2l6ZTo1fX0sbmFtZTpcInNlbGVjdGlvblwiLHZlcnNpb246XCIxLjFcIn0pfSkoalF1ZXJ5KTsiLCIvKiBKYXZhc2NyaXB0IHBsb3R0aW5nIGxpYnJhcnkgZm9yIGpRdWVyeSwgdmVyc2lvbiAwLjguMy5cclxuXHJcbkNvcHlyaWdodCAoYykgMjAwNy0yMDE0IElPTEEgYW5kIE9sZSBMYXVyc2VuLlxyXG5MaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UuXHJcblxyXG4qL1xyXG4oZnVuY3Rpb24oJCl7dmFyIG9wdGlvbnM9e3hheGlzOnt0aW1lem9uZTpudWxsLHRpbWVmb3JtYXQ6bnVsbCx0d2VsdmVIb3VyQ2xvY2s6ZmFsc2UsbW9udGhOYW1lczpudWxsfX07ZnVuY3Rpb24gZmxvb3JJbkJhc2UobixiYXNlKXtyZXR1cm4gYmFzZSpNYXRoLmZsb29yKG4vYmFzZSl9ZnVuY3Rpb24gZm9ybWF0RGF0ZShkLGZtdCxtb250aE5hbWVzLGRheU5hbWVzKXtpZih0eXBlb2YgZC5zdHJmdGltZT09XCJmdW5jdGlvblwiKXtyZXR1cm4gZC5zdHJmdGltZShmbXQpfXZhciBsZWZ0UGFkPWZ1bmN0aW9uKG4scGFkKXtuPVwiXCIrbjtwYWQ9XCJcIisocGFkPT1udWxsP1wiMFwiOnBhZCk7cmV0dXJuIG4ubGVuZ3RoPT0xP3BhZCtuOm59O3ZhciByPVtdO3ZhciBlc2NhcGU9ZmFsc2U7dmFyIGhvdXJzPWQuZ2V0SG91cnMoKTt2YXIgaXNBTT1ob3VyczwxMjtpZihtb250aE5hbWVzPT1udWxsKXttb250aE5hbWVzPVtcIkphblwiLFwiRmViXCIsXCJNYXJcIixcIkFwclwiLFwiTWF5XCIsXCJKdW5cIixcIkp1bFwiLFwiQXVnXCIsXCJTZXBcIixcIk9jdFwiLFwiTm92XCIsXCJEZWNcIl19aWYoZGF5TmFtZXM9PW51bGwpe2RheU5hbWVzPVtcIlN1blwiLFwiTW9uXCIsXCJUdWVcIixcIldlZFwiLFwiVGh1XCIsXCJGcmlcIixcIlNhdFwiXX12YXIgaG91cnMxMjtpZihob3Vycz4xMil7aG91cnMxMj1ob3Vycy0xMn1lbHNlIGlmKGhvdXJzPT0wKXtob3VyczEyPTEyfWVsc2V7aG91cnMxMj1ob3Vyc31mb3IodmFyIGk9MDtpPGZtdC5sZW5ndGg7KytpKXt2YXIgYz1mbXQuY2hhckF0KGkpO2lmKGVzY2FwZSl7c3dpdGNoKGMpe2Nhc2VcImFcIjpjPVwiXCIrZGF5TmFtZXNbZC5nZXREYXkoKV07YnJlYWs7Y2FzZVwiYlwiOmM9XCJcIittb250aE5hbWVzW2QuZ2V0TW9udGgoKV07YnJlYWs7Y2FzZVwiZFwiOmM9bGVmdFBhZChkLmdldERhdGUoKSk7YnJlYWs7Y2FzZVwiZVwiOmM9bGVmdFBhZChkLmdldERhdGUoKSxcIiBcIik7YnJlYWs7Y2FzZVwiaFwiOmNhc2VcIkhcIjpjPWxlZnRQYWQoaG91cnMpO2JyZWFrO2Nhc2VcIklcIjpjPWxlZnRQYWQoaG91cnMxMik7YnJlYWs7Y2FzZVwibFwiOmM9bGVmdFBhZChob3VyczEyLFwiIFwiKTticmVhaztjYXNlXCJtXCI6Yz1sZWZ0UGFkKGQuZ2V0TW9udGgoKSsxKTticmVhaztjYXNlXCJNXCI6Yz1sZWZ0UGFkKGQuZ2V0TWludXRlcygpKTticmVhaztjYXNlXCJxXCI6Yz1cIlwiKyhNYXRoLmZsb29yKGQuZ2V0TW9udGgoKS8zKSsxKTticmVhaztjYXNlXCJTXCI6Yz1sZWZ0UGFkKGQuZ2V0U2Vjb25kcygpKTticmVhaztjYXNlXCJ5XCI6Yz1sZWZ0UGFkKGQuZ2V0RnVsbFllYXIoKSUxMDApO2JyZWFrO2Nhc2VcIllcIjpjPVwiXCIrZC5nZXRGdWxsWWVhcigpO2JyZWFrO2Nhc2VcInBcIjpjPWlzQU0/XCJcIitcImFtXCI6XCJcIitcInBtXCI7YnJlYWs7Y2FzZVwiUFwiOmM9aXNBTT9cIlwiK1wiQU1cIjpcIlwiK1wiUE1cIjticmVhaztjYXNlXCJ3XCI6Yz1cIlwiK2QuZ2V0RGF5KCk7YnJlYWt9ci5wdXNoKGMpO2VzY2FwZT1mYWxzZX1lbHNle2lmKGM9PVwiJVwiKXtlc2NhcGU9dHJ1ZX1lbHNle3IucHVzaChjKX19fXJldHVybiByLmpvaW4oXCJcIil9ZnVuY3Rpb24gbWFrZVV0Y1dyYXBwZXIoZCl7ZnVuY3Rpb24gYWRkUHJveHlNZXRob2Qoc291cmNlT2JqLHNvdXJjZU1ldGhvZCx0YXJnZXRPYmosdGFyZ2V0TWV0aG9kKXtzb3VyY2VPYmpbc291cmNlTWV0aG9kXT1mdW5jdGlvbigpe3JldHVybiB0YXJnZXRPYmpbdGFyZ2V0TWV0aG9kXS5hcHBseSh0YXJnZXRPYmosYXJndW1lbnRzKX19dmFyIHV0Yz17ZGF0ZTpkfTtpZihkLnN0cmZ0aW1lIT11bmRlZmluZWQpe2FkZFByb3h5TWV0aG9kKHV0YyxcInN0cmZ0aW1lXCIsZCxcInN0cmZ0aW1lXCIpfWFkZFByb3h5TWV0aG9kKHV0YyxcImdldFRpbWVcIixkLFwiZ2V0VGltZVwiKTthZGRQcm94eU1ldGhvZCh1dGMsXCJzZXRUaW1lXCIsZCxcInNldFRpbWVcIik7dmFyIHByb3BzPVtcIkRhdGVcIixcIkRheVwiLFwiRnVsbFllYXJcIixcIkhvdXJzXCIsXCJNaWxsaXNlY29uZHNcIixcIk1pbnV0ZXNcIixcIk1vbnRoXCIsXCJTZWNvbmRzXCJdO2Zvcih2YXIgcD0wO3A8cHJvcHMubGVuZ3RoO3ArKyl7YWRkUHJveHlNZXRob2QodXRjLFwiZ2V0XCIrcHJvcHNbcF0sZCxcImdldFVUQ1wiK3Byb3BzW3BdKTthZGRQcm94eU1ldGhvZCh1dGMsXCJzZXRcIitwcm9wc1twXSxkLFwic2V0VVRDXCIrcHJvcHNbcF0pfXJldHVybiB1dGN9ZnVuY3Rpb24gZGF0ZUdlbmVyYXRvcih0cyxvcHRzKXtpZihvcHRzLnRpbWV6b25lPT1cImJyb3dzZXJcIil7cmV0dXJuIG5ldyBEYXRlKHRzKX1lbHNlIGlmKCFvcHRzLnRpbWV6b25lfHxvcHRzLnRpbWV6b25lPT1cInV0Y1wiKXtyZXR1cm4gbWFrZVV0Y1dyYXBwZXIobmV3IERhdGUodHMpKX1lbHNlIGlmKHR5cGVvZiB0aW1lem9uZUpTIT1cInVuZGVmaW5lZFwiJiZ0eXBlb2YgdGltZXpvbmVKUy5EYXRlIT1cInVuZGVmaW5lZFwiKXt2YXIgZD1uZXcgdGltZXpvbmVKUy5EYXRlO2Quc2V0VGltZXpvbmUob3B0cy50aW1lem9uZSk7ZC5zZXRUaW1lKHRzKTtyZXR1cm4gZH1lbHNle3JldHVybiBtYWtlVXRjV3JhcHBlcihuZXcgRGF0ZSh0cykpfX12YXIgdGltZVVuaXRTaXplPXtzZWNvbmQ6MWUzLG1pbnV0ZTo2MCoxZTMsaG91cjo2MCo2MCoxZTMsZGF5OjI0KjYwKjYwKjFlMyxtb250aDozMCoyNCo2MCo2MCoxZTMscXVhcnRlcjozKjMwKjI0KjYwKjYwKjFlMyx5ZWFyOjM2NS4yNDI1KjI0KjYwKjYwKjFlM307dmFyIGJhc2VTcGVjPVtbMSxcInNlY29uZFwiXSxbMixcInNlY29uZFwiXSxbNSxcInNlY29uZFwiXSxbMTAsXCJzZWNvbmRcIl0sWzMwLFwic2Vjb25kXCJdLFsxLFwibWludXRlXCJdLFsyLFwibWludXRlXCJdLFs1LFwibWludXRlXCJdLFsxMCxcIm1pbnV0ZVwiXSxbMzAsXCJtaW51dGVcIl0sWzEsXCJob3VyXCJdLFsyLFwiaG91clwiXSxbNCxcImhvdXJcIl0sWzgsXCJob3VyXCJdLFsxMixcImhvdXJcIl0sWzEsXCJkYXlcIl0sWzIsXCJkYXlcIl0sWzMsXCJkYXlcIl0sWy4yNSxcIm1vbnRoXCJdLFsuNSxcIm1vbnRoXCJdLFsxLFwibW9udGhcIl0sWzIsXCJtb250aFwiXV07dmFyIHNwZWNNb250aHM9YmFzZVNwZWMuY29uY2F0KFtbMyxcIm1vbnRoXCJdLFs2LFwibW9udGhcIl0sWzEsXCJ5ZWFyXCJdXSk7dmFyIHNwZWNRdWFydGVycz1iYXNlU3BlYy5jb25jYXQoW1sxLFwicXVhcnRlclwiXSxbMixcInF1YXJ0ZXJcIl0sWzEsXCJ5ZWFyXCJdXSk7ZnVuY3Rpb24gaW5pdChwbG90KXtwbG90Lmhvb2tzLnByb2Nlc3NPcHRpb25zLnB1c2goZnVuY3Rpb24ocGxvdCxvcHRpb25zKXskLmVhY2gocGxvdC5nZXRBeGVzKCksZnVuY3Rpb24oYXhpc05hbWUsYXhpcyl7dmFyIG9wdHM9YXhpcy5vcHRpb25zO2lmKG9wdHMubW9kZT09XCJ0aW1lXCIpe2F4aXMudGlja0dlbmVyYXRvcj1mdW5jdGlvbihheGlzKXt2YXIgdGlja3M9W107dmFyIGQ9ZGF0ZUdlbmVyYXRvcihheGlzLm1pbixvcHRzKTt2YXIgbWluU2l6ZT0wO3ZhciBzcGVjPW9wdHMudGlja1NpemUmJm9wdHMudGlja1NpemVbMV09PT1cInF1YXJ0ZXJcInx8b3B0cy5taW5UaWNrU2l6ZSYmb3B0cy5taW5UaWNrU2l6ZVsxXT09PVwicXVhcnRlclwiP3NwZWNRdWFydGVyczpzcGVjTW9udGhzO2lmKG9wdHMubWluVGlja1NpemUhPW51bGwpe2lmKHR5cGVvZiBvcHRzLnRpY2tTaXplPT1cIm51bWJlclwiKXttaW5TaXplPW9wdHMudGlja1NpemV9ZWxzZXttaW5TaXplPW9wdHMubWluVGlja1NpemVbMF0qdGltZVVuaXRTaXplW29wdHMubWluVGlja1NpemVbMV1dfX1mb3IodmFyIGk9MDtpPHNwZWMubGVuZ3RoLTE7KytpKXtpZihheGlzLmRlbHRhPChzcGVjW2ldWzBdKnRpbWVVbml0U2l6ZVtzcGVjW2ldWzFdXStzcGVjW2krMV1bMF0qdGltZVVuaXRTaXplW3NwZWNbaSsxXVsxXV0pLzImJnNwZWNbaV1bMF0qdGltZVVuaXRTaXplW3NwZWNbaV1bMV1dPj1taW5TaXplKXticmVha319dmFyIHNpemU9c3BlY1tpXVswXTt2YXIgdW5pdD1zcGVjW2ldWzFdO2lmKHVuaXQ9PVwieWVhclwiKXtpZihvcHRzLm1pblRpY2tTaXplIT1udWxsJiZvcHRzLm1pblRpY2tTaXplWzFdPT1cInllYXJcIil7c2l6ZT1NYXRoLmZsb29yKG9wdHMubWluVGlja1NpemVbMF0pfWVsc2V7dmFyIG1hZ249TWF0aC5wb3coMTAsTWF0aC5mbG9vcihNYXRoLmxvZyhheGlzLmRlbHRhL3RpbWVVbml0U2l6ZS55ZWFyKS9NYXRoLkxOMTApKTt2YXIgbm9ybT1heGlzLmRlbHRhL3RpbWVVbml0U2l6ZS55ZWFyL21hZ247aWYobm9ybTwxLjUpe3NpemU9MX1lbHNlIGlmKG5vcm08Myl7c2l6ZT0yfWVsc2UgaWYobm9ybTw3LjUpe3NpemU9NX1lbHNle3NpemU9MTB9c2l6ZSo9bWFnbn1pZihzaXplPDEpe3NpemU9MX19YXhpcy50aWNrU2l6ZT1vcHRzLnRpY2tTaXplfHxbc2l6ZSx1bml0XTt2YXIgdGlja1NpemU9YXhpcy50aWNrU2l6ZVswXTt1bml0PWF4aXMudGlja1NpemVbMV07dmFyIHN0ZXA9dGlja1NpemUqdGltZVVuaXRTaXplW3VuaXRdO2lmKHVuaXQ9PVwic2Vjb25kXCIpe2Quc2V0U2Vjb25kcyhmbG9vckluQmFzZShkLmdldFNlY29uZHMoKSx0aWNrU2l6ZSkpfWVsc2UgaWYodW5pdD09XCJtaW51dGVcIil7ZC5zZXRNaW51dGVzKGZsb29ySW5CYXNlKGQuZ2V0TWludXRlcygpLHRpY2tTaXplKSl9ZWxzZSBpZih1bml0PT1cImhvdXJcIil7ZC5zZXRIb3VycyhmbG9vckluQmFzZShkLmdldEhvdXJzKCksdGlja1NpemUpKX1lbHNlIGlmKHVuaXQ9PVwibW9udGhcIil7ZC5zZXRNb250aChmbG9vckluQmFzZShkLmdldE1vbnRoKCksdGlja1NpemUpKX1lbHNlIGlmKHVuaXQ9PVwicXVhcnRlclwiKXtkLnNldE1vbnRoKDMqZmxvb3JJbkJhc2UoZC5nZXRNb250aCgpLzMsdGlja1NpemUpKX1lbHNlIGlmKHVuaXQ9PVwieWVhclwiKXtkLnNldEZ1bGxZZWFyKGZsb29ySW5CYXNlKGQuZ2V0RnVsbFllYXIoKSx0aWNrU2l6ZSkpfWQuc2V0TWlsbGlzZWNvbmRzKDApO2lmKHN0ZXA+PXRpbWVVbml0U2l6ZS5taW51dGUpe2Quc2V0U2Vjb25kcygwKX1pZihzdGVwPj10aW1lVW5pdFNpemUuaG91cil7ZC5zZXRNaW51dGVzKDApfWlmKHN0ZXA+PXRpbWVVbml0U2l6ZS5kYXkpe2Quc2V0SG91cnMoMCl9aWYoc3RlcD49dGltZVVuaXRTaXplLmRheSo0KXtkLnNldERhdGUoMSl9aWYoc3RlcD49dGltZVVuaXRTaXplLm1vbnRoKjIpe2Quc2V0TW9udGgoZmxvb3JJbkJhc2UoZC5nZXRNb250aCgpLDMpKX1pZihzdGVwPj10aW1lVW5pdFNpemUucXVhcnRlcioyKXtkLnNldE1vbnRoKGZsb29ySW5CYXNlKGQuZ2V0TW9udGgoKSw2KSl9aWYoc3RlcD49dGltZVVuaXRTaXplLnllYXIpe2Quc2V0TW9udGgoMCl9dmFyIGNhcnJ5PTA7dmFyIHY9TnVtYmVyLk5hTjt2YXIgcHJldjtkb3twcmV2PXY7dj1kLmdldFRpbWUoKTt0aWNrcy5wdXNoKHYpO2lmKHVuaXQ9PVwibW9udGhcInx8dW5pdD09XCJxdWFydGVyXCIpe2lmKHRpY2tTaXplPDEpe2Quc2V0RGF0ZSgxKTt2YXIgc3RhcnQ9ZC5nZXRUaW1lKCk7ZC5zZXRNb250aChkLmdldE1vbnRoKCkrKHVuaXQ9PVwicXVhcnRlclwiPzM6MSkpO3ZhciBlbmQ9ZC5nZXRUaW1lKCk7ZC5zZXRUaW1lKHYrY2FycnkqdGltZVVuaXRTaXplLmhvdXIrKGVuZC1zdGFydCkqdGlja1NpemUpO2NhcnJ5PWQuZ2V0SG91cnMoKTtkLnNldEhvdXJzKDApfWVsc2V7ZC5zZXRNb250aChkLmdldE1vbnRoKCkrdGlja1NpemUqKHVuaXQ9PVwicXVhcnRlclwiPzM6MSkpfX1lbHNlIGlmKHVuaXQ9PVwieWVhclwiKXtkLnNldEZ1bGxZZWFyKGQuZ2V0RnVsbFllYXIoKSt0aWNrU2l6ZSl9ZWxzZXtkLnNldFRpbWUoditzdGVwKX19d2hpbGUodjxheGlzLm1heCYmdiE9cHJldik7cmV0dXJuIHRpY2tzfTtheGlzLnRpY2tGb3JtYXR0ZXI9ZnVuY3Rpb24odixheGlzKXt2YXIgZD1kYXRlR2VuZXJhdG9yKHYsYXhpcy5vcHRpb25zKTtpZihvcHRzLnRpbWVmb3JtYXQhPW51bGwpe3JldHVybiBmb3JtYXREYXRlKGQsb3B0cy50aW1lZm9ybWF0LG9wdHMubW9udGhOYW1lcyxvcHRzLmRheU5hbWVzKX12YXIgdXNlUXVhcnRlcnM9YXhpcy5vcHRpb25zLnRpY2tTaXplJiZheGlzLm9wdGlvbnMudGlja1NpemVbMV09PVwicXVhcnRlclwifHxheGlzLm9wdGlvbnMubWluVGlja1NpemUmJmF4aXMub3B0aW9ucy5taW5UaWNrU2l6ZVsxXT09XCJxdWFydGVyXCI7dmFyIHQ9YXhpcy50aWNrU2l6ZVswXSp0aW1lVW5pdFNpemVbYXhpcy50aWNrU2l6ZVsxXV07dmFyIHNwYW49YXhpcy5tYXgtYXhpcy5taW47dmFyIHN1ZmZpeD1vcHRzLnR3ZWx2ZUhvdXJDbG9jaz9cIiAlcFwiOlwiXCI7dmFyIGhvdXJDb2RlPW9wdHMudHdlbHZlSG91ckNsb2NrP1wiJUlcIjpcIiVIXCI7dmFyIGZtdDtpZih0PHRpbWVVbml0U2l6ZS5taW51dGUpe2ZtdD1ob3VyQ29kZStcIjolTTolU1wiK3N1ZmZpeH1lbHNlIGlmKHQ8dGltZVVuaXRTaXplLmRheSl7aWYoc3BhbjwyKnRpbWVVbml0U2l6ZS5kYXkpe2ZtdD1ob3VyQ29kZStcIjolTVwiK3N1ZmZpeH1lbHNle2ZtdD1cIiViICVkIFwiK2hvdXJDb2RlK1wiOiVNXCIrc3VmZml4fX1lbHNlIGlmKHQ8dGltZVVuaXRTaXplLm1vbnRoKXtmbXQ9XCIlYiAlZFwifWVsc2UgaWYodXNlUXVhcnRlcnMmJnQ8dGltZVVuaXRTaXplLnF1YXJ0ZXJ8fCF1c2VRdWFydGVycyYmdDx0aW1lVW5pdFNpemUueWVhcil7aWYoc3Bhbjx0aW1lVW5pdFNpemUueWVhcil7Zm10PVwiJWJcIn1lbHNle2ZtdD1cIiViICVZXCJ9fWVsc2UgaWYodXNlUXVhcnRlcnMmJnQ8dGltZVVuaXRTaXplLnllYXIpe2lmKHNwYW48dGltZVVuaXRTaXplLnllYXIpe2ZtdD1cIlElcVwifWVsc2V7Zm10PVwiUSVxICVZXCJ9fWVsc2V7Zm10PVwiJVlcIn12YXIgcnQ9Zm9ybWF0RGF0ZShkLGZtdCxvcHRzLm1vbnRoTmFtZXMsb3B0cy5kYXlOYW1lcyk7cmV0dXJuIHJ0fX19KX0pfSQucGxvdC5wbHVnaW5zLnB1c2goe2luaXQ6aW5pdCxvcHRpb25zOm9wdGlvbnMsbmFtZTpcInRpbWVcIix2ZXJzaW9uOlwiMS4wXCJ9KTskLnBsb3QuZm9ybWF0RGF0ZT1mb3JtYXREYXRlOyQucGxvdC5kYXRlR2VuZXJhdG9yPWRhdGVHZW5lcmF0b3J9KShqUXVlcnkpOyIsIm1vZHVsZS5leHBvcnRzID0gbW9tZW50OyIsIm1vZHVsZS5leHBvcnRzID0gUmVhY3Q7IiwibW9kdWxlLmV4cG9ydHMgPSBSZWFjdERPTTsiXSwic291cmNlUm9vdCI6IiJ9