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
/*!************************************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/CreateGuid.js ***!
  \************************************************************************************************************************************************/
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
/*!*************************************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/GetNodeSize.js ***!
  \*************************************************************************************************************************************************/
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
/*!***************************************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/GetTextHeight.js ***!
  \***************************************************************************************************************************************************/
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
/*!**************************************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/GetTextWidth.js ***!
  \**************************************************************************************************************************************************/
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
/*!***********************************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/IsInteger.js ***!
  \***********************************************************************************************************************************************/
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
/*!**********************************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/IsNumber.js ***!
  \**********************************************************************************************************************************************/
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
/*!*************************************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/RandomColor.js ***!
  \*************************************************************************************************************************************************/
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
/*!*******************************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/index.js ***!
  \*******************************************************************************************************************************************/
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
/*!*****************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/create-react-class/factory.js ***!
  \*****************************************************************************************************************************/
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

// -- Inlined from fbjs --

var emptyObject = {};

if (true) {
  Object.freeze(emptyObject);
}

var validateFormat = function validateFormat(format) {};

if (true) {
  validateFormat = function validateFormat(format) {
    if (format === undefined) {
      throw new Error('invariant requires an error message argument');
    }
  };
}

function _invariant(condition, format, a, b, c, d, e, f) {
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

var warning = function(){};

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

// /-- Inlined from fbjs --

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
/*!***************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/create-react-class/index.js ***!
  \***************************************************************************************************************************/
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
/*!****************************************************************************************************************************************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/css-loader/dist/cjs.js!C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/css/react-datetime.css ***!
  \****************************************************************************************************************************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(/*! ../../css-loader/dist/runtime/api.js */ "../../node_modules/css-loader/dist/runtime/api.js")(false);
// Module
exports.push([module.i, "/*!\n * https://github.com/YouCanBookMe/react-datetime\n */\n\n.rdt {\n  position: relative;\n}\n.rdtPicker {\n  display: none;\n  position: absolute;\n  width: 250px;\n  padding: 4px;\n  margin-top: 1px;\n  z-index: 99999 !important;\n  background: #fff;\n  box-shadow: 0 1px 3px rgba(0,0,0,.1);\n  border: 1px solid #f9f9f9;\n}\n.rdtOpen .rdtPicker {\n  display: block;\n}\n.rdtStatic .rdtPicker {\n  box-shadow: none;\n  position: static;\n}\n\n.rdtPicker .rdtTimeToggle {\n  text-align: center;\n}\n\n.rdtPicker table {\n  width: 100%;\n  margin: 0;\n}\n.rdtPicker td,\n.rdtPicker th {\n  text-align: center;\n  height: 28px;\n}\n.rdtPicker td {\n  cursor: pointer;\n}\n.rdtPicker td.rdtDay:hover,\n.rdtPicker td.rdtHour:hover,\n.rdtPicker td.rdtMinute:hover,\n.rdtPicker td.rdtSecond:hover,\n.rdtPicker .rdtTimeToggle:hover {\n  background: #eeeeee;\n  cursor: pointer;\n}\n.rdtPicker td.rdtOld,\n.rdtPicker td.rdtNew {\n  color: #999999;\n}\n.rdtPicker td.rdtToday {\n  position: relative;\n}\n.rdtPicker td.rdtToday:before {\n  content: '';\n  display: inline-block;\n  border-left: 7px solid transparent;\n  border-bottom: 7px solid #428bca;\n  border-top-color: rgba(0, 0, 0, 0.2);\n  position: absolute;\n  bottom: 4px;\n  right: 4px;\n}\n.rdtPicker td.rdtActive,\n.rdtPicker td.rdtActive:hover {\n  background-color: #428bca;\n  color: #fff;\n  text-shadow: 0 -1px 0 rgba(0, 0, 0, 0.25);\n}\n.rdtPicker td.rdtActive.rdtToday:before {\n  border-bottom-color: #fff;\n}\n.rdtPicker td.rdtDisabled,\n.rdtPicker td.rdtDisabled:hover {\n  background: none;\n  color: #999999;\n  cursor: not-allowed;\n}\n\n.rdtPicker td span.rdtOld {\n  color: #999999;\n}\n.rdtPicker td span.rdtDisabled,\n.rdtPicker td span.rdtDisabled:hover {\n  background: none;\n  color: #999999;\n  cursor: not-allowed;\n}\n.rdtPicker th {\n  border-bottom: 1px solid #f9f9f9;\n}\n.rdtPicker .dow {\n  width: 14.2857%;\n  border-bottom: none;\n  cursor: default;\n}\n.rdtPicker th.rdtSwitch {\n  width: 100px;\n}\n.rdtPicker th.rdtNext,\n.rdtPicker th.rdtPrev {\n  font-size: 21px;\n  vertical-align: top;\n}\n\n.rdtPrev span,\n.rdtNext span {\n  display: block;\n  -webkit-touch-callout: none; /* iOS Safari */\n  -webkit-user-select: none;   /* Chrome/Safari/Opera */\n  -khtml-user-select: none;    /* Konqueror */\n  -moz-user-select: none;      /* Firefox */\n  -ms-user-select: none;       /* Internet Explorer/Edge */\n  user-select: none;\n}\n\n.rdtPicker th.rdtDisabled,\n.rdtPicker th.rdtDisabled:hover {\n  background: none;\n  color: #999999;\n  cursor: not-allowed;\n}\n.rdtPicker thead tr:first-child th {\n  cursor: pointer;\n}\n.rdtPicker thead tr:first-child th:hover {\n  background: #eeeeee;\n}\n\n.rdtPicker tfoot {\n  border-top: 1px solid #f9f9f9;\n}\n\n.rdtPicker button {\n  border: none;\n  background: none;\n  cursor: pointer;\n}\n.rdtPicker button:hover {\n  background-color: #eee;\n}\n\n.rdtPicker thead button {\n  width: 100%;\n  height: 100%;\n}\n\ntd.rdtMonth,\ntd.rdtYear {\n  height: 50px;\n  width: 25%;\n  cursor: pointer;\n}\ntd.rdtMonth:hover,\ntd.rdtYear:hover {\n  background: #eee;\n}\n\n.rdtCounters {\n  display: inline-block;\n}\n\n.rdtCounters > div {\n  float: left;\n}\n\n.rdtCounter {\n  height: 100px;\n}\n\n.rdtCounter {\n  width: 40px;\n}\n\n.rdtCounterSeparator {\n  line-height: 100px;\n}\n\n.rdtCounter .rdtBtn {\n  height: 40%;\n  line-height: 40px;\n  cursor: pointer;\n  display: block;\n\n  -webkit-touch-callout: none; /* iOS Safari */\n  -webkit-user-select: none;   /* Chrome/Safari/Opera */\n  -khtml-user-select: none;    /* Konqueror */\n  -moz-user-select: none;      /* Firefox */\n  -ms-user-select: none;       /* Internet Explorer/Edge */\n  user-select: none;\n}\n.rdtCounter .rdtBtn:hover {\n  background: #eee;\n}\n.rdtCounter .rdtCount {\n  height: 20%;\n  font-size: 1.2em;\n}\n\n.rdtMilli {\n  vertical-align: middle;\n  padding-left: 8px;\n  width: 48px;\n}\n\n.rdtMilli input {\n  width: 100%;\n  font-size: 1.2em;\n  margin-top: 37px;\n}\n\n.rdtTime td {\n  cursor: default;\n}\n", ""]);


/***/ }),

/***/ "../../node_modules/css-loader/dist/runtime/api.js":
/*!******************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/css-loader/dist/runtime/api.js ***!
  \******************************************************************************************************************************/
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
/*!*****************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/decode-uri-component/index.js ***!
  \*****************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

"use strict";

var token = '%[a-f0-9]{2}';
var singleMatcher = new RegExp('(' + token + ')|([^%]+?)', 'gi');
var multiMatcher = new RegExp('(' + token + ')+', 'gi');

function decodeComponents(components, split) {
	try {
		// Try to decode the entire string first
		return [decodeURIComponent(components.join(''))];
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
		var tokens = input.match(singleMatcher) || [];

		for (var i = 1; i < tokens.length; i++) {
			input = decodeComponents(tokens, i).join('');

			tokens = input.match(singleMatcher) || [];
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
/*!*************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/fbjs/lib/emptyFunction.js ***!
  \*************************************************************************************************************************/
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

/***/ "../../node_modules/fbjs/lib/invariant.js":
/*!*********************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/fbjs/lib/invariant.js ***!
  \*********************************************************************************************************************/
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
/*!*******************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/fbjs/lib/warning.js ***!
  \*******************************************************************************************************************/
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
/*!*******************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/history/DOMUtils.js ***!
  \*******************************************************************************************************************/
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
/*!************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/history/LocationUtils.js ***!
  \************************************************************************************************************************/
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
/*!********************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/history/PathUtils.js ***!
  \********************************************************************************************************************/
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
/*!*******************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/history/createBrowserHistory.js ***!
  \*******************************************************************************************************************************/
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
/*!**********************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/history/createTransitionManager.js ***!
  \**********************************************************************************************************************************/
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
/*!********************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/invariant/browser.js ***!
  \********************************************************************************************************************/
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
/*!**********************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/object-assign/index.js ***!
  \**********************************************************************************************************************/
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
/*!****************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/prop-types/checkPropTypes.js ***!
  \****************************************************************************************************************************/
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
/*!*************************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/prop-types/factoryWithTypeCheckers.js ***!
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
/*!*******************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/prop-types/index.js ***!
  \*******************************************************************************************************************/
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
/*!**************************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/prop-types/lib/ReactPropTypesSecret.js ***!
  \**************************************************************************************************************************************/
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
/*!*********************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/query-string/index.js ***!
  \*********************************************************************************************************************/
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
/*!**************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/DateTime.js ***!
  \**************************************************************************************************************************/
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
/*!*************************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/css/react-datetime.css ***!
  \*************************************************************************************************************************************/
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
/*!**************************************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/node_modules/object-assign/index.js ***!
  \**************************************************************************************************************************************************/
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
/*!***************************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/src/CalendarContainer.js ***!
  \***************************************************************************************************************************************/
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
/*!******************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/src/DaysView.js ***!
  \******************************************************************************************************************************/
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
/*!********************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/src/MonthsView.js ***!
  \********************************************************************************************************************************/
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
/*!******************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/src/TimeView.js ***!
  \******************************************************************************************************************************/
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
/*!*******************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/src/YearsView.js ***!
  \*******************************************************************************************************************************/
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
/*!****************************************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-onclickoutside/dist/react-onclickoutside.es.js ***!
  \****************************************************************************************************************************************************/
/*! exports provided: default, IGNORE_CLASS_NAME */
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

  _setPrototypeOf(subClass, superClass);
}

function _setPrototypeOf(o, p) {
  _setPrototypeOf = Object.setPrototypeOf || function _setPrototypeOf(o, p) {
    o.__proto__ = p;
    return o;
  };

  return _setPrototypeOf(o, p);
}

function _objectWithoutPropertiesLoose(source, excluded) {
  if (source == null) return {};
  var target = {};
  var sourceKeys = Object.keys(source);
  var key, i;

  for (i = 0; i < sourceKeys.length; i++) {
    key = sourceKeys[i];
    if (excluded.indexOf(key) >= 0) continue;
    target[key] = source[key];
  }

  return target;
}

function _assertThisInitialized(self) {
  if (self === void 0) {
    throw new ReferenceError("this hasn't been initialised - super() hasn't been called");
  }

  return self;
}/**
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
  // Also cover shadowRoot node by checking current.host


  while (current.parentNode || current.host) {
    // Only check normal node without shadowRoot
    if (current.parentNode && isNodeFound(current, componentNode, ignoreClass)) {
      return true;
    }

    current = current.parentNode || current.host;
  }

  return current;
}
/**
 * Check if the browser scrollbar was clicked
 */

function clickedScrollbar(evt) {
  return document.documentElement.clientWidth <= evt.clientX || document.documentElement.clientHeight <= evt.clientY;
}// ideally will get replaced with external dep
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
};function autoInc(seed) {
  if (seed === void 0) {
    seed = 0;
  }

  return function () {
    return ++seed;
  };
}

var uid = autoInc();var passiveEventSupport;
var handlersMap = {};
var enabledInstances = {};
var touchEvents = ['touchstart', 'touchmove'];
var IGNORE_CLASS_NAME = 'ignore-react-onclickoutside';
/**
 * Options for addEventHandler and removeEventHandler
 */

function getEventHandlerOptions(instance, eventName) {
  var handlerOptions = {};
  var isTouchEvent = touchEvents.indexOf(eventName) !== -1;

  if (isTouchEvent && passiveEventSupport) {
    handlerOptions.passive = !instance.props.preventDefault;
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
  return _temp = _class = /*#__PURE__*/function (_Component) {
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
          if (_this.initTimeStamp > event.timeStamp) return;

          if (_this.props.preventDefault) {
            event.preventDefault();
          }

          if (_this.props.stopPropagation) {
            event.stopPropagation();
          }

          if (_this.props.excludeScrollbar && clickedScrollbar(event)) return;
          var current = event.composed && event.composedPath && event.composedPath().shift() || event.target;

          if (findHighest(current, _this.componentNode, _this.props.outsideClickIgnoreClass) !== document) {
            return;
          }

          _this.__outsideClickHandler(event);
        };

        events.forEach(function (eventName) {
          document.addEventListener(eventName, handlersMap[_this._uid], getEventHandlerOptions(_assertThisInitialized(_this), eventName));
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
            return document.removeEventListener(eventName, fn, getEventHandlerOptions(_assertThisInitialized(_this), eventName));
          });
          delete handlersMap[_this._uid];
        }
      };

      _this.getRef = function (ref) {
        return _this.instanceRef = ref;
      };

      _this._uid = uid();
      _this.initTimeStamp = performance.now();
      return _this;
    }
    /**
     * Access the WrappedComponent's instance.
     */


    var _proto = onClickOutside.prototype;

    _proto.getInstance = function getInstance() {
      if (WrappedComponent.prototype && !WrappedComponent.prototype.isReactComponent) {
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
    }
    /**
     * Remove all document's event listeners for this component
     */
    ;

    _proto.componentWillUnmount = function componentWillUnmount() {
      this.disableOnClickOutside();
    }
    /**
     * Can be called to explicitly enable event listening
     * for clicks and touches outside of this element.
     */
    ;

    /**
     * Pass-through render
     */
    _proto.render = function render() {
      // eslint-disable-next-line no-unused-vars
      var _this$props = this.props;
          _this$props.excludeScrollbar;
          var props = _objectWithoutPropertiesLoose(_this$props, ["excludeScrollbar"]);

      if (WrappedComponent.prototype && WrappedComponent.prototype.isReactComponent) {
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
}/* harmony default export */ __webpack_exports__["default"] = (onClickOutsideHOC);

/***/ }),

/***/ "../../node_modules/resolve-pathname/index.js":
/*!*************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/resolve-pathname/index.js ***!
  \*************************************************************************************************************************/
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
/*!**************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/strict-uri-encode/index.js ***!
  \**************************************************************************************************************************/
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
/*!*****************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/style-loader/lib/addStyles.js ***!
  \*****************************************************************************************************************************/
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
/*!************************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/style-loader/lib/urls.js ***!
  \************************************************************************************************************************/
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
/*!********************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/value-equal/index.js ***!
  \********************************************************************************************************************/
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
/*!******************************************************************************************************************!*\
  !*** C:/Users/gcsantos/source/repos/openXDA/Source/Applications/openXDA/openXDA/node_modules/warning/browser.js ***!
  \******************************************************************************************************************/
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
    PeriodicDataDisplayService.prototype.getMeasurementCharacteristics = function (meterID) {
        return $.ajax({
            type: "GET",
            url: "".concat(window.location.origin, "/api/PeriodicDataDisplay/GetMeasurementCharacteristics"),
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
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly8vd2VicGFjay9ib290c3RyYXAiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2djc2FudG9zL3NvdXJjZS9yZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9AZ3BhLWdlbXN0b25lL2hlbHBlci1mdW5jdGlvbnMvbGliL0NyZWF0ZUd1aWQuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2djc2FudG9zL3NvdXJjZS9yZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9AZ3BhLWdlbXN0b25lL2hlbHBlci1mdW5jdGlvbnMvbGliL0dldE5vZGVTaXplLmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9nY3NhbnRvcy9zb3VyY2UvcmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvQGdwYS1nZW1zdG9uZS9oZWxwZXItZnVuY3Rpb25zL2xpYi9HZXRUZXh0SGVpZ2h0LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9nY3NhbnRvcy9zb3VyY2UvcmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvQGdwYS1nZW1zdG9uZS9oZWxwZXItZnVuY3Rpb25zL2xpYi9HZXRUZXh0V2lkdGguanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2djc2FudG9zL3NvdXJjZS9yZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9AZ3BhLWdlbXN0b25lL2hlbHBlci1mdW5jdGlvbnMvbGliL0lzSW50ZWdlci5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvZ2NzYW50b3Mvc291cmNlL3JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL0BncGEtZ2Vtc3RvbmUvaGVscGVyLWZ1bmN0aW9ucy9saWIvSXNOdW1iZXIuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2djc2FudG9zL3NvdXJjZS9yZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9AZ3BhLWdlbXN0b25lL2hlbHBlci1mdW5jdGlvbnMvbGliL1JhbmRvbUNvbG9yLmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9nY3NhbnRvcy9zb3VyY2UvcmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvQGdwYS1nZW1zdG9uZS9oZWxwZXItZnVuY3Rpb25zL2xpYi9pbmRleC5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvZ2NzYW50b3Mvc291cmNlL3JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2NyZWF0ZS1yZWFjdC1jbGFzcy9mYWN0b3J5LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9nY3NhbnRvcy9zb3VyY2UvcmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvY3JlYXRlLXJlYWN0LWNsYXNzL2luZGV4LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9nY3NhbnRvcy9zb3VyY2UvcmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvcmVhY3QtZGF0ZXRpbWUvY3NzL3JlYWN0LWRhdGV0aW1lLmNzcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvZ2NzYW50b3Mvc291cmNlL3JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2Nzcy1sb2FkZXIvZGlzdC9ydW50aW1lL2FwaS5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvZ2NzYW50b3Mvc291cmNlL3JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2RlY29kZS11cmktY29tcG9uZW50L2luZGV4LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9nY3NhbnRvcy9zb3VyY2UvcmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvZmJqcy9saWIvZW1wdHlGdW5jdGlvbi5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvZ2NzYW50b3Mvc291cmNlL3JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2ZianMvbGliL2ludmFyaWFudC5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvZ2NzYW50b3Mvc291cmNlL3JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2ZianMvbGliL3dhcm5pbmcuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2djc2FudG9zL3NvdXJjZS9yZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9oaXN0b3J5L0RPTVV0aWxzLmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9nY3NhbnRvcy9zb3VyY2UvcmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvaGlzdG9yeS9Mb2NhdGlvblV0aWxzLmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9nY3NhbnRvcy9zb3VyY2UvcmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvaGlzdG9yeS9QYXRoVXRpbHMuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2djc2FudG9zL3NvdXJjZS9yZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9oaXN0b3J5L2NyZWF0ZUJyb3dzZXJIaXN0b3J5LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9nY3NhbnRvcy9zb3VyY2UvcmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvaGlzdG9yeS9jcmVhdGVUcmFuc2l0aW9uTWFuYWdlci5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvZ2NzYW50b3Mvc291cmNlL3JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2ludmFyaWFudC9icm93c2VyLmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9nY3NhbnRvcy9zb3VyY2UvcmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvb2JqZWN0LWFzc2lnbi9pbmRleC5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvZ2NzYW50b3Mvc291cmNlL3JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3Byb3AtdHlwZXMvY2hlY2tQcm9wVHlwZXMuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2djc2FudG9zL3NvdXJjZS9yZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9wcm9wLXR5cGVzL2ZhY3RvcnlXaXRoVHlwZUNoZWNrZXJzLmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9nY3NhbnRvcy9zb3VyY2UvcmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvcHJvcC10eXBlcy9pbmRleC5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvZ2NzYW50b3Mvc291cmNlL3JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3Byb3AtdHlwZXMvbGliL1JlYWN0UHJvcFR5cGVzU2VjcmV0LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9nY3NhbnRvcy9zb3VyY2UvcmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvcXVlcnktc3RyaW5nL2luZGV4LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9nY3NhbnRvcy9zb3VyY2UvcmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvcmVhY3QtZGF0ZXRpbWUvRGF0ZVRpbWUuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2djc2FudG9zL3NvdXJjZS9yZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9yZWFjdC1kYXRldGltZS9jc3MvcmVhY3QtZGF0ZXRpbWUuY3NzPzk4ZTYiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2djc2FudG9zL3NvdXJjZS9yZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9yZWFjdC1kYXRldGltZS9ub2RlX21vZHVsZXMvb2JqZWN0LWFzc2lnbi9pbmRleC5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvZ2NzYW50b3Mvc291cmNlL3JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3JlYWN0LWRhdGV0aW1lL3NyYy9DYWxlbmRhckNvbnRhaW5lci5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvZ2NzYW50b3Mvc291cmNlL3JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3JlYWN0LWRhdGV0aW1lL3NyYy9EYXlzVmlldy5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvZ2NzYW50b3Mvc291cmNlL3JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3JlYWN0LWRhdGV0aW1lL3NyYy9Nb250aHNWaWV3LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9nY3NhbnRvcy9zb3VyY2UvcmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvcmVhY3QtZGF0ZXRpbWUvc3JjL1RpbWVWaWV3LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9nY3NhbnRvcy9zb3VyY2UvcmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvcmVhY3QtZGF0ZXRpbWUvc3JjL1llYXJzVmlldy5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvZ2NzYW50b3Mvc291cmNlL3JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3JlYWN0LW9uY2xpY2tvdXRzaWRlL2Rpc3QvcmVhY3Qtb25jbGlja291dHNpZGUuZXMuanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2djc2FudG9zL3NvdXJjZS9yZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9yZXNvbHZlLXBhdGhuYW1lL2luZGV4LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9nY3NhbnRvcy9zb3VyY2UvcmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvc3RyaWN0LXVyaS1lbmNvZGUvaW5kZXguanMiLCJ3ZWJwYWNrOi8vL0M6L1VzZXJzL2djc2FudG9zL3NvdXJjZS9yZXBvcy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9zdHlsZS1sb2FkZXIvbGliL2FkZFN0eWxlcy5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvZ2NzYW50b3Mvc291cmNlL3JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3N0eWxlLWxvYWRlci9saWIvdXJscy5qcyIsIndlYnBhY2s6Ly8vQzovVXNlcnMvZ2NzYW50b3Mvc291cmNlL3JlcG9zL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3ZhbHVlLWVxdWFsL2luZGV4LmpzIiwid2VicGFjazovLy9DOi9Vc2Vycy9nY3NhbnRvcy9zb3VyY2UvcmVwb3Mvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvd2FybmluZy9icm93c2VyLmpzIiwid2VicGFjazovLy8uL1RTL1NlcnZpY2VzL1BlcmlvZGljRGF0YURpc3BsYXkudHMiLCJ3ZWJwYWNrOi8vLy4vVFMvU2VydmljZXMvVHJlbmRpbmdEYXRhRGlzcGxheS50cyIsIndlYnBhY2s6Ly8vLi9UU1gvRGF0ZVRpbWVSYW5nZVBpY2tlci50c3giLCJ3ZWJwYWNrOi8vLy4vVFNYL01lYXN1cmVtZW50SW5wdXQudHN4Iiwid2VicGFjazovLy8uL1RTWC9NZXRlcklucHV0LnRzeCIsIndlYnBhY2s6Ly8vLi9UU1gvVHJlbmRpbmdDaGFydC50c3giLCJ3ZWJwYWNrOi8vLy4vVFNYL1RyZW5kaW5nRGF0YURpc3BsYXkudHN4Iiwid2VicGFjazovLy8uL2Zsb3QvanF1ZXJ5LmZsb3QuYXhpc2xhYmVscy5qcyIsIndlYnBhY2s6Ly8vLi9mbG90L2pxdWVyeS5mbG90LmNyb3NzaGFpci5taW4uanMiLCJ3ZWJwYWNrOi8vLy4vZmxvdC9qcXVlcnkuZmxvdC5taW4uanMiLCJ3ZWJwYWNrOi8vLy4vZmxvdC9qcXVlcnkuZmxvdC5uYXZpZ2F0ZS5taW4uanMiLCJ3ZWJwYWNrOi8vLy4vZmxvdC9qcXVlcnkuZmxvdC5zZWxlY3Rpb24ubWluLmpzIiwid2VicGFjazovLy8uL2Zsb3QvanF1ZXJ5LmZsb3QudGltZS5taW4uanMiLCJ3ZWJwYWNrOi8vL2V4dGVybmFsIFwibW9tZW50XCIiLCJ3ZWJwYWNrOi8vL2V4dGVybmFsIFwiUmVhY3RcIiIsIndlYnBhY2s6Ly8vZXh0ZXJuYWwgXCJSZWFjdERPTVwiIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7UUFBQTtRQUNBOztRQUVBO1FBQ0E7O1FBRUE7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7O1FBRUE7UUFDQTs7UUFFQTtRQUNBOztRQUVBO1FBQ0E7UUFDQTs7O1FBR0E7UUFDQTs7UUFFQTtRQUNBOztRQUVBO1FBQ0E7UUFDQTtRQUNBLDBDQUEwQyxnQ0FBZ0M7UUFDMUU7UUFDQTs7UUFFQTtRQUNBO1FBQ0E7UUFDQSx3REFBd0Qsa0JBQWtCO1FBQzFFO1FBQ0EsaURBQWlELGNBQWM7UUFDL0Q7O1FBRUE7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBLHlDQUF5QyxpQ0FBaUM7UUFDMUUsZ0hBQWdILG1CQUFtQixFQUFFO1FBQ3JJO1FBQ0E7O1FBRUE7UUFDQTtRQUNBO1FBQ0EsMkJBQTJCLDBCQUEwQixFQUFFO1FBQ3ZELGlDQUFpQyxlQUFlO1FBQ2hEO1FBQ0E7UUFDQTs7UUFFQTtRQUNBLHNEQUFzRCwrREFBK0Q7O1FBRXJIO1FBQ0E7OztRQUdBO1FBQ0E7Ozs7Ozs7Ozs7Ozs7QUNsRmE7QUFDYjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLGlGQUFpRjtBQUNqRjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLDhDQUE4QyxjQUFjO0FBQzVEO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLEtBQUs7QUFDTDtBQUNBOzs7Ozs7Ozs7Ozs7O0FDckNhO0FBQ2I7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxpRkFBaUY7QUFDakY7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLDhDQUE4QyxjQUFjO0FBQzVEO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOzs7Ozs7Ozs7Ozs7O0FDN0NhO0FBQ2I7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxpRkFBaUY7QUFDakY7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLDhDQUE4QyxjQUFjO0FBQzVEO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOzs7Ozs7Ozs7Ozs7O0FDN0NhO0FBQ2I7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxpRkFBaUY7QUFDakY7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLDhDQUE4QyxjQUFjO0FBQzVEO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOzs7Ozs7Ozs7Ozs7O0FDN0NhO0FBQ2I7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxpRkFBaUY7QUFDakY7QUFDQTtB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VUFBVSxJQUFxQztBQUMvQztBQUNBO0FBQ0E7QUFDQSxVQUFVO0FBQ1Y7QUFDQTtBQUNBO0FBQ0EsS0FBSztBQUNMO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsT0FBTztBQUNQO0FBQ0E7QUFDQSxLQUFLO0FBQ0w7QUFDQSxVQUFVLElBQXFDO0FBQy9DO0FBQ0E7QUFDQSx3Q0FBd0M7QUFDeEMsS0FBSztBQUNMO0FBQ0E7QUFDQSxLQUFLO0FBQ0w7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsWUFBWSxJQUFxQztBQUNqRDtBQUNBO0FBQ0EseUNBQXlDO0FBQ3pDO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLFVBQVUsSUFBcUM7QUFDL0M7QUFDQTs7QUFFQSxZQUFZLElBQXFDO0FBQ2pEO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0EsT0FBTztBQUNQO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0EsU0FBUztBQUNUO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFD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QUNBOztBQUVBO0FBQ0EscUJBQXFCLGdDQUFnQztBQUNyRDtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsNkJBQTZCO0FBQzdCO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsV0FBVztBQUNYO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsU0FBUztBQUNUO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLE9BQU87QUFDUDtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FB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Q0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0EsRUFBRTs7QUFFRjtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7O0FBRUE7O0FBRUE7QUFDQTtBQUNBOztBQUVBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsRUFBRTs7QUFFRjtBQUNBO0FBQ0E7QUFDQSxHQUFHO0FBQ0g7QUFDQSxHQUFHO0FBQ0g7QUFDQTs7QUFFQTtBQUNBLEVBQUU7O0FBRUY7QUFDQTtBQUNBO0FBQ0E7QUFDQSxJQUFJO0FBQ0o7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0EsRUFBRTs7QUFFRjtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQSxJQUFJO0FBQ0o7QUFDQSxJQUFJO0FBQ0o7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxJQUFJO0FBQ0o7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxHQUFHO0FBQ0g7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBLGFBQWE7QUFDYjs7QUFFQTtBQUNBO0FBQ0E7QUFDQSxHQUFHO0FBQ0g7QUFDQTs7QUFFQTtBQUNBO0FBQ0EsR0FBRztBQUNILEVBQUU7O0FBRUY7QUFDQTtBQUNBO0FBQ0E7QUFDQSxFQUFFOztBQUVG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QUFFQTtBQUNBLENBQUM7QUFDRDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsR0FBRztBQUNIO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7OztBQUdBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0EsR0FBRztBQUNIO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7OztBQUdBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQSxDQUFDO0FBQ0Q7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBLHdDQUF3QztBQUN4QztBQUNBO0FBQ0E7QUFDQSxHQUFHOztBQUVIOztBQUVBO0FBQ0E7QUFDQTtBQUNBLEVBQUU7QUFDRjtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUEsb0JBQW9CO0FBQ3BCO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOzs7QUFHQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBOztBQUVBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBOztBQUVBOztBQUVBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBLGVBQWUsNkRBQVc7QUFDMUI7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBLFNBQVM7QUFDVDs7QUFF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O0FBQ2pDO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxjQUFjOztBQUVkLGtEQUFrRCxzQkFBc0I7QUFDeEU7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBOztBQUVBO0FBQ0E7QUFDQTtBQUNBLEdBQUc7QUFDSDtBQUNBLEdBQUc7QUFDSDtBQUNBO0FBQ0E7QUFDQSxFQUFFO0FBQ0Y7QUFDQSxFQUFFO0FBQ0Y7QUFDQTtBQUNBLEVBQUU7QUFDRjtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBLEVBQUU7QUFDRjs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLE1BQU07QUFDTjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBOztBQUVBOztBQUVBO0FBQ0E7O0FBRUEsRUFBRTtBQUNGO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBLEVBQUU7QUFDRjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0EsR0FBRztBQUNIO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0EsQ0FBQzs7QUFFRDtBQUNBOztBQUVBO0FBQ0E7QUFDQSxFQUFFO0FBQ0Y7QUFDQTs7QUFFQTs7QUFFQTtBQUNBO0FBQ0EsR0FBRztBQUNIO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7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QUFDLG9CQUFDLG1CQUFtQixPQUFHLEVBQUUsUUFBUSxDQUFDLGNBQWMsQ0FBQyxlQUFlLENBQUMsQ0FBQyxDQUFDOzs7Ozs7Ozs7Ozs7QUN0Ym5GO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOzs7QUFHQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7O0FBR0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLFNBQVM7QUFDVDtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsU0FBUztBQUNUO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQTtBQUNBO0FBQ0EsU0FBUztBQUNUO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7O0FBR0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQSx1RUFBdUU7QUFDdkU7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBLFNBQVM7QUFDVDtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQSw0RUFBNEU7QUFDNUU7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsU0FBUztBQUNUO0FBQ0E7QUFDQTtB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LGlCQUFpQix3REFBd0QscUNBQXFDLEtBQUssbUZBQW1GLDJDQUEyQyxhQUFhLE9BQU8sZ0JBQWdCLE9BQU8sa0JBQWtCLE9BQU8sS0FBSyxRQUFRLFdBQVcsV0FBVyxRQUFRLHlDQUF5Qyw4QkFBOEIsc0JBQXNCLHFDQUFxQyxtQkFBbUIsbURBQW1ELHdCQUF3QixtREFBbUQsc0JBQXNCLCtDQUErQyx1QkFBdUIsK0NBQStDLHlCQUF5QixtREFBbUQsc0JBQXNCLHFEQUFxRCxxQkFBcUIsOEJBQThCLGdCQUFnQiw0QkFBNEIsZ0JBQWdCLDJCQUEyQixjQUFjLDZCQUE2QixhQUFhLCtCQUErQix3Q0FBd0MsaUNBQWlDLHdDQUF3Qyw0QkFBNEIsY0FBYyxZQUFZLGlCQUFpQixTQUFTLEdBQUcsT0FBTyxjQUFjLGNBQWMsbUNBQW1DLGVBQWUsYUFBYSxzQkFBc0IsK0NBQStDLG9CQUFvQiwwREFBMEQsbUJBQW1CLGNBQWMsS0FBSyx5REFBeUQsc0JBQXNCLHdDQUF3QyxLQUFLLG1CQUFtQiwyQkFBMkIsY0FBYyxvQ0FBb0Msb0NBQW9DLDBCQUEwQixtRUFBbUUsNklBQTZJLHNEQUFzRCwyQkFBMkIseUNBQXlDLDRDQUE0QyxRQUFRLDBCQUEwQiw2QkFBNkIsNEJBQTRCLDRCQUE0QiwwQkFBMEIsS0FBSyxvQ0FBb0MsOEJBQThCLFlBQVksZ0ZBQWdGLDJCQUEyQixTQUFTLEtBQUssYUFBYSwwQ0FBMEMsMkJBQTJCLFVBQVUsS0FBSyxjQUFjLEtBQUssU0FBUyx1REFBdUQsWUFBWSxFQUFFLEVBQUUscUJBQXFCLG9EQUFvRCxFQUFFLDZCQUE2QixtQ0FBbUMsVTs7Ozs7Ozs7Ozs7QUNOcGxOLHdCOzs7Ozs7Ozs7OztBQ0FBLHVCOzs7Ozs7Ozs7OztBQ0FBLDBCIiwiZmlsZSI6IlRyZW5kaW5nRGF0YURpc3BsYXkuanMiLCJzb3VyY2VzQ29udGVudCI6WyIgXHQvLyBUaGUgbW9kdWxlIGNhY2hlXG4gXHR2YXIgaW5zdGFsbGVkTW9kdWxlcyA9IHt9O1xuXG4gXHQvLyBUaGUgcmVxdWlyZSBmdW5jdGlvblxuIFx0ZnVuY3Rpb24gX193ZWJwYWNrX3JlcXVpcmVfXyhtb2R1bGVJZCkge1xuXG4gXHRcdC8vIENoZWNrIGlmIG1vZHVsZSBpcyBpbiBjYWNoZVxuIFx0XHRpZihpbnN0YWxsZWRNb2R1bGVzW21vZHVsZUlkXSkge1xuIFx0XHRcdHJldHVybiBpbnN0YWxsZWRNb2R1bGVzW21vZHVsZUlkXS5leHBvcnRzO1xuIFx0XHR9XG4gXHRcdC8vIENyZWF0ZSBhIG5ldyBtb2R1bGUgKGFuZCBwdXQgaXQgaW50byB0aGUgY2FjaGUpXG4gXHRcdHZhciBtb2R1bGUgPSBpbnN0YWxsZWRNb2R1bGVzW21vZHVsZUlkXSA9IHtcbiBcdFx0XHRpOiBtb2R1bGVJZCxcbiBcdFx0XHRsOiBmYWxzZSxcbiBcdFx0XHRleHBvcnRzOiB7fVxuIFx0XHR9O1xuXG4gXHRcdC8vIEV4ZWN1dGUgdGhlIG1vZHVsZSBmdW5jdGlvblxuIFx0XHRtb2R1bGVzW21vZHVsZUlkXS5jYWxsKG1vZHVsZS5leHBvcnRzLCBtb2R1bGUsIG1vZHVsZS5leHBvcnRzLCBfX3dlYnBhY2tfcmVxdWlyZV9fKTtcblxuIFx0XHQvLyBGbGFnIHRoZSBtb2R1bGUgYXMgbG9hZGVkXG4gXHRcdG1vZHVsZS5sID0gdHJ1ZTtcblxuIFx0XHQvLyBSZXR1cm4gdGhlIGV4cG9ydHMgb2YgdGhlIG1vZHVsZVxuIFx0XHRyZXR1cm4gbW9kdWxlLmV4cG9ydHM7XG4gXHR9XG5cblxuIFx0Ly8gZXhwb3NlIHRoZSBtb2R1bGVzIG9iamVjdCAoX193ZWJwYWNrX21vZHVsZXNfXylcbiBcdF9fd2VicGFja19yZXF1aXJlX18ubSA9IG1vZHVsZXM7XG5cbiBcdC8vIGV4cG9zZSB0aGUgbW9kdWxlIGNhY2hlXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLmMgPSBpbnN0YWxsZWRNb2R1bGVzO1xuXG4gXHQvLyBkZWZpbmUgZ2V0dGVyIGZ1bmN0aW9uIGZvciBoYXJtb255IGV4cG9ydHNcbiBcdF9fd2VicGFja19yZXF1aXJlX18uZCA9IGZ1bmN0aW9uKGV4cG9ydHMsIG5hbWUsIGdldHRlcikge1xuIFx0XHRpZighX193ZWJwYWNrX3JlcXVpcmVfXy5vKGV4cG9ydHMsIG5hbWUpKSB7XG4gXHRcdFx0T2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIG5hbWUsIHsgZW51bWVyYWJsZTogdHJ1ZSwgZ2V0OiBnZXR0ZXIgfSk7XG4gXHRcdH1cbiBcdH07XG5cbiBcdC8vIGRlZmluZSBfX2VzTW9kdWxlIG9uIGV4cG9ydHNcbiBcdF9fd2VicGFja19yZXF1aXJlX18uciA9IGZ1bmN0aW9uKGV4cG9ydHMpIHtcbiBcdFx0aWYodHlwZW9mIFN5bWJvbCAhPT0gJ3VuZGVmaW5lZCcgJiYgU3ltYm9sLnRvU3RyaW5nVGFnKSB7XG4gXHRcdFx0T2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFN5bWJvbC50b1N0cmluZ1RhZywgeyB2YWx1ZTogJ01vZHVsZScgfSk7XG4gXHRcdH1cbiBcdFx0T2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsICdfX2VzTW9kdWxlJywgeyB2YWx1ZTogdHJ1ZSB9KTtcbiBcdH07XG5cbiBcdC8vIGNyZWF0ZSBhIGZha2UgbmFtZXNwYWNlIG9iamVjdFxuIFx0Ly8gbW9kZSAmIDE6IHZhbHVlIGlzIGEgbW9kdWxlIGlkLCByZXF1aXJlIGl0XG4gXHQvLyBtb2RlICYgMjogbWVyZ2UgYWxsIHByb3BlcnRpZXMgb2YgdmFsdWUgaW50byB0aGUgbnNcbiBcdC8vIG1vZGUgJiA0OiByZXR1cm4gdmFsdWUgd2hlbiBhbHJlYWR5IG5zIG9iamVjdFxuIFx0Ly8gbW9kZSAmIDh8MTogYmVoYXZlIGxpa2UgcmVxdWlyZVxuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy50ID0gZnVuY3Rpb24odmFsdWUsIG1vZGUpIHtcbiBcdFx0aWYobW9kZSAmIDEpIHZhbHVlID0gX193ZWJwYWNrX3JlcXVpcmVfXyh2YWx1ZSk7XG4gXHRcdGlmKG1vZGUgJiA4KSByZXR1cm4gdmFsdWU7XG4gXHRcdGlmKChtb2RlICYgNCkgJiYgdHlwZW9mIHZhbHVlID09PSAnb2JqZWN0JyAmJiB2YWx1ZSAmJiB2YWx1ZS5fX2VzTW9kdWxlKSByZXR1cm4gdmFsdWU7XG4gXHRcdHZhciBucyA9IE9iamVjdC5jcmVhdGUobnVsbCk7XG4gXHRcdF9fd2VicGFja19yZXF1aXJlX18ucihucyk7XG4gXHRcdE9iamVjdC5kZWZpbmVQcm9wZXJ0eShucywgJ2RlZmF1bHQnLCB7IGVudW1lcmFibGU6IHRydWUsIHZhbHVlOiB2YWx1ZSB9KTtcbiBcdFx0aWYobW9kZSAmIDIgJiYgdHlwZW9mIHZhbHVlICE9ICdzdHJpbmcnKSBmb3IodmFyIGtleSBpbiB2YWx1ZSkgX193ZWJwYWNrX3JlcXVpcmVfXy5kKG5zLCBrZXksIGZ1bmN0aW9uKGtleSkgeyByZXR1cm4gdmFsdWVba2V5XTsgfS5iaW5kKG51bGwsIGtleSkpO1xuIFx0XHRyZXR1cm4gbnM7XG4gXHR9O1xuXG4gXHQvLyBnZXREZWZhdWx0RXhwb3J0IGZ1bmN0aW9uIGZvciBjb21wYXRpYmlsaXR5IHdpdGggbm9uLWhhcm1vbnkgbW9kdWxlc1xuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5uID0gZnVuY3Rpb24obW9kdWxlKSB7XG4gXHRcdHZhciBnZXR0ZXIgPSBtb2R1bGUgJiYgbW9kdWxlLl9fZXNNb2R1bGUgP1xuIFx0XHRcdGZ1bmN0aW9uIGdldERlZmF1bHQoKSB7IHJldHVybiBtb2R1bGVbJ2RlZmF1bHQnXTsgfSA6XG4gXHRcdFx0ZnVuY3Rpb24gZ2V0TW9kdWxlRXhwb3J0cygpIHsgcmV0dXJuIG1vZHVsZTsgfTtcbiBcdFx0X193ZWJwYWNrX3JlcXVpcmVfXy5kKGdldHRlciwgJ2EnLCBnZXR0ZXIpO1xuIFx0XHRyZXR1cm4gZ2V0dGVyO1xuIFx0fTtcblxuIFx0Ly8gT2JqZWN0LnByb3RvdHlwZS5oYXNPd25Qcm9wZXJ0eS5jYWxsXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLm8gPSBmdW5jdGlvbihvYmplY3QsIHByb3BlcnR5KSB7IHJldHVybiBPYmplY3QucHJvdG90eXBlLmhhc093blByb3BlcnR5LmNhbGwob2JqZWN0LCBwcm9wZXJ0eSk7IH07XG5cbiBcdC8vIF9fd2VicGFja19wdWJsaWNfcGF0aF9fXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLnAgPSBcIlwiO1xuXG5cbiBcdC8vIExvYWQgZW50cnkgbW9kdWxlIGFuZCByZXR1cm4gZXhwb3J0c1xuIFx0cmV0dXJuIF9fd2VicGFja19yZXF1aXJlX18oX193ZWJwYWNrX3JlcXVpcmVfXy5zID0gXCIuL1RTWC9UcmVuZGluZ0RhdGFEaXNwbGF5LnRzeFwiKTtcbiIsIlwidXNlIHN0cmljdFwiO1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuLy8gIENyZWF0ZUd1aWQudHMgLSBHYnRjXHJcbi8vXHJcbi8vICBDb3B5cmlnaHQgwqkgMjAyMSwgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlLiAgQWxsIFJpZ2h0cyBSZXNlcnZlZC5cclxuLy9cclxuLy8gIExpY2Vuc2VkIHRvIHRoZSBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UgKEdQQSkgdW5kZXIgb25lIG9yIG1vcmUgY29udHJpYnV0b3IgbGljZW5zZSBhZ3JlZW1lbnRzLiBTZWVcclxuLy8gIHRoZSBOT1RJQ0UgZmlsZSBkaXN0cmlidXRlZCB3aXRoIHRoaXMgd29yayBmb3IgYWRkaXRpb25hbCBpbmZvcm1hdGlvbiByZWdhcmRpbmcgY29weXJpZ2h0IG93bmVyc2hpcC5cclxuLy8gIFRoZSBHUEEgbGljZW5zZXMgdGhpcyBmaWxlIHRvIHlvdSB1bmRlciB0aGUgTUlUIExpY2Vuc2UgKE1JVCksIHRoZSBcIkxpY2Vuc2VcIjsgeW91IG1heSBub3QgdXNlIHRoaXNcclxuLy8gIGZpbGUgZXhjZXB0IGluIGNvbXBsaWFuY2Ugd2l0aCB0aGUgTGljZW5zZS4gWW91IG1heSBvYnRhaW4gYSBjb3B5IG9mIHRoZSBMaWNlbnNlIGF0OlxyXG4vL1xyXG4vLyAgICAgIGh0dHA6Ly9vcGVuc291cmNlLm9yZy9saWNlbnNlcy9NSVRcclxuLy9cclxuLy8gIFVubGVzcyBhZ3JlZWQgdG8gaW4gd3JpdGluZywgdGhlIHN1YmplY3Qgc29mdHdhcmUgZGlzdHJpYnV0ZWQgdW5kZXIgdGhlIExpY2Vuc2UgaXMgZGlzdHJpYnV0ZWQgb24gYW5cclxuLy8gIFwiQVMtSVNcIiBCQVNJUywgV0lUSE9VVCBXQVJSQU5USUVTIE9SIENPTkRJVElPTlMgT0YgQU5ZIEtJTkQsIGVpdGhlciBleHByZXNzIG9yIGltcGxpZWQuIFJlZmVyIHRvIHRoZVxyXG4vLyAgTGljZW5zZSBmb3IgdGhlIHNwZWNpZmljIGxhbmd1YWdlIGdvdmVybmluZyBwZXJtaXNzaW9ucyBhbmQgbGltaXRhdGlvbnMuXHJcbi8vICBcclxuLy8gIGh0dHBzOi8vc3RhY2tvdmVyZmxvdy5jb20vcXVlc3Rpb25zLzEwNTAzNC9ob3ctdG8tY3JlYXRlLWEtZ3VpZC11dWlkXHJcbi8vXHJcbi8vICBDb2RlIE1vZGlmaWNhdGlvbiBIaXN0b3J5OlxyXG4vLyAgLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG4vLyAgMDEvMDQvMjAyMSAtIEJpbGx5IEVybmVzdFxyXG4vLyAgICAgICBHZW5lcmF0ZWQgb3JpZ2luYWwgdmVyc2lvbiBvZiBzb3VyY2UgY29kZS5cclxuLy9cclxuLy8gKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcIl9fZXNNb2R1bGVcIiwgeyB2YWx1ZTogdHJ1ZSB9KTtcclxuZXhwb3J0cy5DcmVhdGVHdWlkID0gdm9pZCAwO1xyXG4vKipcclxuICogVGhpcyBmdW5jdGlvbiBnZW5lcmF0ZXMgYSBHVUlEXHJcbiAqL1xyXG5mdW5jdGlvbiBDcmVhdGVHdWlkKCkge1xyXG4gICAgcmV0dXJuICd4eHh4eHh4eC14eHh4LTR4eHgteXh4eC14eHh4eHh4eHh4eHgnLnJlcGxhY2UoL1t4eV0vZywgZnVuY3Rpb24gKGMpIHtcclxuICAgICAgICB2YXIgciA9IE1hdGgucmFuZG9tKCkgKiAxNiB8IDA7XHJcbiAgICAgICAgdmFyIHYgPSBjID09PSAneCcgPyByIDogKHIgJiAweDMgfCAweDgpO1xyXG4gICAgICAgIHJldHVybiB2LnRvU3RyaW5nKDE2KTtcclxuICAgIH0pO1xyXG59XHJcbmV4cG9ydHMuQ3JlYXRlR3VpZCA9IENyZWF0ZUd1aWQ7XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuLy8gIEdldE5vZGVTaXplLnRzeCAtIEdidGNcclxuLy9cclxuLy8gIENvcHlyaWdodCDCqSAyMDIxLCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxyXG4vL1xyXG4vLyAgTGljZW5zZWQgdG8gdGhlIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZSAoR1BBKSB1bmRlciBvbmUgb3IgbW9yZSBjb250cmlidXRvciBsaWNlbnNlIGFncmVlbWVudHMuIFNlZVxyXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxyXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xyXG4vLyAgZmlsZSBleGNlcHQgaW4gY29tcGxpYW5jZSB3aXRoIHRoZSBMaWNlbnNlLiBZb3UgbWF5IG9idGFpbiBhIGNvcHkgb2YgdGhlIExpY2Vuc2UgYXQ6XHJcbi8vXHJcbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxyXG4vL1xyXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxyXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXHJcbi8vICBMaWNlbnNlIGZvciB0aGUgc3BlY2lmaWMgbGFuZ3VhZ2UgZ292ZXJuaW5nIHBlcm1pc3Npb25zIGFuZCBsaW1pdGF0aW9ucy5cclxuLy9cclxuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XHJcbi8vICAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vICAwMS8xNS8yMDIxIC0gQy4gTGFja25lclxyXG4vLyAgICAgICBHZW5lcmF0ZWQgb3JpZ2luYWwgdmVyc2lvbiBvZiBzb3VyY2UgY29kZS5cclxuLy9cclxuLy8gKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcIl9fZXNNb2R1bGVcIiwgeyB2YWx1ZTogdHJ1ZSB9KTtcclxuZXhwb3J0cy5HZXROb2RlU2l6ZSA9IHZvaWQgMDtcclxuLyoqXHJcbiAqIEdldE5vZGVTaXplIHJldHVybnMgdGhlIGRpbWVuc2lvbnMgb2YgYW4gaHRtbCBlbGVtZW50XHJcbiAqIEBwYXJhbSBub2RlOiBhIEhUTUwgZWxlbWVudCwgb3IgbnVsbCBjYW4gYmUgcGFzc2VkIHRocm91Z2hcclxuICovXHJcbmZ1bmN0aW9uIEdldE5vZGVTaXplKG5vZGUpIHtcclxuICAgIGlmIChub2RlID09PSBudWxsKVxyXG4gICAgICAgIHJldHVybiB7XHJcbiAgICAgICAgICAgIGhlaWdodDogMCxcclxuICAgICAgICAgICAgd2lkdGg6IDAsXHJcbiAgICAgICAgICAgIHRvcDogMCxcclxuICAgICAgICAgICAgbGVmdDogMCxcclxuICAgICAgICB9O1xyXG4gICAgdmFyIF9hID0gbm9kZS5nZXRCb3VuZGluZ0NsaWVudFJlY3QoKSwgaGVpZ2h0ID0gX2EuaGVpZ2h0LCB3aWR0aCA9IF9hLndpZHRoLCB0b3AgPSBfYS50b3AsIGxlZnQgPSBfYS5sZWZ0O1xyXG4gICAgcmV0dXJuIHtcclxuICAgICAgICBoZWlnaHQ6IHBhcnNlSW50KGhlaWdodC50b1N0cmluZygpLCAxMCksXHJcbiAgICAgICAgd2lkdGg6IHBhcnNlSW50KHdpZHRoLnRvU3RyaW5nKCksIDEwKSxcclxuICAgICAgICB0b3A6IHBhcnNlSW50KHRvcC50b1N0cmluZygpLCAxMCksXHJcbiAgICAgICAgbGVmdDogcGFyc2VJbnQobGVmdC50b1N0cmluZygpLCAxMCksXHJcbiAgICB9O1xyXG59XHJcbmV4cG9ydHMuR2V0Tm9kZVNpemUgPSBHZXROb2RlU2l6ZTtcclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbi8vICoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG4vLyAgR2V0VGV4dEhlaWdodC50c3ggLSBHYnRjXHJcbi8vXHJcbi8vICBDb3B5cmlnaHQgwqkgMjAyMSwgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlLiAgQWxsIFJpZ2h0cyBSZXNlcnZlZC5cclxuLy9cclxuLy8gIExpY2Vuc2VkIHRvIHRoZSBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UgKEdQQSkgdW5kZXIgb25lIG9yIG1vcmUgY29udHJpYnV0b3IgbGljZW5zZSBhZ3JlZW1lbnRzLiBTZWVcclxuLy8gIHRoZSBOT1RJQ0UgZmlsZSBkaXN0cmlidXRlZCB3aXRoIHRoaXMgd29yayBmb3IgYWRkaXRpb25hbCBpbmZvcm1hdGlvbiByZWdhcmRpbmcgY29weXJpZ2h0IG93bmVyc2hpcC5cclxuLy8gIFRoZSBHUEEgbGljZW5zZXMgdGhpcyBmaWxlIHRvIHlvdSB1bmRlciB0aGUgTUlUIExpY2Vuc2UgKE1JVCksIHRoZSBcIkxpY2Vuc2VcIjsgeW91IG1heSBub3QgdXNlIHRoaXNcclxuLy8gIGZpbGUgZXhjZXB0IGluIGNvbXBsaWFuY2Ugd2l0aCB0aGUgTGljZW5zZS4gWW91IG1heSBvYnRhaW4gYSBjb3B5IG9mIHRoZSBMaWNlbnNlIGF0OlxyXG4vL1xyXG4vLyAgICAgIGh0dHA6Ly9vcGVuc291cmNlLm9yZy9saWNlbnNlcy9NSVRcclxuLy9cclxuLy8gIFVubGVzcyBhZ3JlZWQgdG8gaW4gd3JpdGluZywgdGhlIHN1YmplY3Qgc29mdHdhcmUgZGlzdHJpYnV0ZWQgdW5kZXIgdGhlIExpY2Vuc2UgaXMgZGlzdHJpYnV0ZWQgb24gYW5cclxuLy8gIFwiQVMtSVNcIiBCQVNJUywgV0lUSE9VVCBXQVJSQU5USUVTIE9SIENPTkRJVElPTlMgT0YgQU5ZIEtJTkQsIGVpdGhlciBleHByZXNzIG9yIGltcGxpZWQuIFJlZmVyIHRvIHRoZVxyXG4vLyAgTGljZW5zZSBmb3IgdGhlIHNwZWNpZmljIGxhbmd1YWdlIGdvdmVybmluZyBwZXJtaXNzaW9ucyBhbmQgbGltaXRhdGlvbnMuXHJcbi8vXHJcbi8vICBDb2RlIE1vZGlmaWNhdGlvbiBIaXN0b3J5OlxyXG4vLyAgLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG4vLyAgMDMvMTIvMjAyMSAtIGMuIExhY2tuZXJcclxuLy8gICAgICAgR2VuZXJhdGVkIG9yaWdpbmFsIHZlcnNpb24gb2Ygc291cmNlIGNvZGUuXHJcbi8vXHJcbi8vICoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJfX2VzTW9kdWxlXCIsIHsgdmFsdWU6IHRydWUgfSk7XHJcbmV4cG9ydHMuR2V0VGV4dEhlaWdodCA9IHZvaWQgMDtcclxuLyoqXHJcbiAqIFRoaXMgZnVuY3Rpb24gcmV0dXJucyB0aGUgaGVpZ2h0IG9mIGEgcGllY2Ugb2YgdGV4dCBnaXZlbiBhIGZvbnQsIGZvbnRzaXplLCBhbmQgYSB3b3JkXHJcbiAqIEBwYXJhbSBmb250OiBEZXRlcm1pbmVzIGZvbnQgb2YgZ2l2ZW4gdGV4dFxyXG4gKiBAcGFyYW0gZm9udFNpemU6IERldGVybWluZXMgc2l6ZSBvZiBnaXZlbiBmb250XHJcbiAqIEBwYXJhbSB3b3JkOiBUZXh0IHRvIG1lYXN1cmVcclxuICovXHJcbmZ1bmN0aW9uIEdldFRleHRIZWlnaHQoZm9udCwgZm9udFNpemUsIHdvcmQpIHtcclxuICAgIHZhciB0ZXh0ID0gZG9jdW1lbnQuY3JlYXRlRWxlbWVudChcInNwYW5cIik7XHJcbiAgICBkb2N1bWVudC5ib2R5LmFwcGVuZENoaWxkKHRleHQpO1xyXG4gICAgdGV4dC5zdHlsZS5mb250ID0gZm9udDtcclxuICAgIHRleHQuc3R5bGUuZm9udFNpemUgPSBmb250U2l6ZTtcclxuICAgIHRleHQuc3R5bGUuaGVpZ2h0ID0gJ2F1dG8nO1xyXG4gICAgdGV4dC5zdHlsZS53aWR0aCA9ICdhdXRvJztcclxuICAgIHRleHQuc3R5bGUucG9zaXRpb24gPSAnYWJzb2x1dGUnO1xyXG4gICAgdGV4dC5zdHlsZS53aGl0ZVNwYWNlID0gJ25vLXdyYXAnO1xyXG4gICAgdGV4dC5pbm5lckhUTUwgPSB3b3JkO1xyXG4gICAgdmFyIGhlaWdodCA9IE1hdGguY2VpbCh0ZXh0LmNsaWVudEhlaWdodCk7XHJcbiAgICBkb2N1bWVudC5ib2R5LnJlbW92ZUNoaWxkKHRleHQpO1xyXG4gICAgcmV0dXJuIGhlaWdodDtcclxufVxyXG5leHBvcnRzLkdldFRleHRIZWlnaHQgPSBHZXRUZXh0SGVpZ2h0O1xyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxuLy8gKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbi8vICBHZXRUZXh0V2lkdGgudHN4IC0gR2J0Y1xyXG4vL1xyXG4vLyAgQ29weXJpZ2h0IMKpIDIwMjEsIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZS4gIEFsbCBSaWdodHMgUmVzZXJ2ZWQuXHJcbi8vXHJcbi8vICBMaWNlbnNlZCB0byB0aGUgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlIChHUEEpIHVuZGVyIG9uZSBvciBtb3JlIGNvbnRyaWJ1dG9yIGxpY2Vuc2UgYWdyZWVtZW50cy4gU2VlXHJcbi8vICB0aGUgTk9USUNFIGZpbGUgZGlzdHJpYnV0ZWQgd2l0aCB0aGlzIHdvcmsgZm9yIGFkZGl0aW9uYWwgaW5mb3JtYXRpb24gcmVnYXJkaW5nIGNvcHlyaWdodCBvd25lcnNoaXAuXHJcbi8vICBUaGUgR1BBIGxpY2Vuc2VzIHRoaXMgZmlsZSB0byB5b3UgdW5kZXIgdGhlIE1JVCBMaWNlbnNlIChNSVQpLCB0aGUgXCJMaWNlbnNlXCI7IHlvdSBtYXkgbm90IHVzZSB0aGlzXHJcbi8vICBmaWxlIGV4Y2VwdCBpbiBjb21wbGlhbmNlIHdpdGggdGhlIExpY2Vuc2UuIFlvdSBtYXkgb2J0YWluIGEgY29weSBvZiB0aGUgTGljZW5zZSBhdDpcclxuLy9cclxuLy8gICAgICBodHRwOi8vb3BlbnNvdXJjZS5vcmcvbGljZW5zZXMvTUlUXHJcbi8vXHJcbi8vICBVbmxlc3MgYWdyZWVkIHRvIGluIHdyaXRpbmcsIHRoZSBzdWJqZWN0IHNvZnR3YXJlIGRpc3RyaWJ1dGVkIHVuZGVyIHRoZSBMaWNlbnNlIGlzIGRpc3RyaWJ1dGVkIG9uIGFuXHJcbi8vICBcIkFTLUlTXCIgQkFTSVMsIFdJVEhPVVQgV0FSUkFOVElFUyBPUiBDT05ESVRJT05TIE9GIEFOWSBLSU5ELCBlaXRoZXIgZXhwcmVzcyBvciBpbXBsaWVkLiBSZWZlciB0byB0aGVcclxuLy8gIExpY2Vuc2UgZm9yIHRoZSBzcGVjaWZpYyBsYW5ndWFnZSBnb3Zlcm5pbmcgcGVybWlzc2lvbnMgYW5kIGxpbWl0YXRpb25zLlxyXG4vL1xyXG4vLyAgQ29kZSBNb2RpZmljYXRpb24gSGlzdG9yeTpcclxuLy8gIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuLy8gIDAxLzA3LzIwMjEgLSBCaWxseSBFcm5lc3RcclxuLy8gICAgICAgR2VuZXJhdGVkIG9yaWdpbmFsIHZlcnNpb24gb2Ygc291cmNlIGNvZGUuXHJcbi8vXHJcbi8vICoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJfX2VzTW9kdWxlXCIsIHsgdmFsdWU6IHRydWUgfSk7XHJcbmV4cG9ydHMuR2V0VGV4dFdpZHRoID0gdm9pZCAwO1xyXG4vKipcclxuICogR2V0VGV4dFdpZHRoIHJldHVybnMgdGhlIHdpZHRoIG9mIGEgcGllY2Ugb2YgdGV4dCBnaXZlbiBpdHMgZm9udCwgZm9udFNpemUsIGFuZCBjb250ZW50LlxyXG4gKiBAcGFyYW0gZm9udDogRGV0ZXJtaW5lcyBmb250IG9mIGdpdmVuIHRleHRcclxuICogQHBhcmFtIGZvbnRTaXplOiBEZXRlcm1pbmVzIHNpemUgb2YgZ2l2ZW4gZm9udFxyXG4gKiBAcGFyYW0gd29yZDogVGV4dCB0byBtZWFzdXJlXHJcbiAqL1xyXG5mdW5jdGlvbiBHZXRUZXh0V2lkdGgoZm9udCwgZm9udFNpemUsIHdvcmQpIHtcclxuICAgIHZhciB0ZXh0ID0gZG9jdW1lbnQuY3JlYXRlRWxlbWVudChcInNwYW5cIik7XHJcbiAgICBkb2N1bWVudC5ib2R5LmFwcGVuZENoaWxkKHRleHQpO1xyXG4gICAgdGV4dC5zdHlsZS5mb250ID0gZm9udDtcclxuICAgIHRleHQuc3R5bGUuZm9udFNpemUgPSBmb250U2l6ZTtcclxuICAgIHRleHQuc3R5bGUuaGVpZ2h0ID0gJ2F1dG8nO1xyXG4gICAgdGV4dC5zdHlsZS53aWR0aCA9ICdhdXRvJztcclxuICAgIHRleHQuc3R5bGUucG9zaXRpb24gPSAnYWJzb2x1dGUnO1xyXG4gICAgdGV4dC5zdHlsZS53aGl0ZVNwYWNlID0gJ25vLXdyYXAnO1xyXG4gICAgdGV4dC5pbm5lckhUTUwgPSB3b3JkO1xyXG4gICAgdmFyIHdpZHRoID0gTWF0aC5jZWlsKHRleHQuY2xpZW50V2lkdGgpO1xyXG4gICAgZG9jdW1lbnQuYm9keS5yZW1vdmVDaGlsZCh0ZXh0KTtcclxuICAgIHJldHVybiB3aWR0aDtcclxufVxyXG5leHBvcnRzLkdldFRleHRXaWR0aCA9IEdldFRleHRXaWR0aDtcclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbi8vICoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG4vLyAgSXNJbnRlZ2VyLnRzeCAtIEdidGNcclxuLy9cclxuLy8gIENvcHlyaWdodCDCqSAyMDIxLCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxyXG4vL1xyXG4vLyAgTGljZW5zZWQgdG8gdGhlIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZSAoR1BBKSB1bmRlciBvbmUgb3IgbW9yZSBjb250cmlidXRvciBsaWNlbnNlIGFncmVlbWVudHMuIFNlZVxyXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxyXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xyXG4vLyAgZmlsZSBleGNlcHQgaW4gY29tcGxpYW5jZSB3aXRoIHRoZSBMaWNlbnNlLiBZb3UgbWF5IG9idGFpbiBhIGNvcHkgb2YgdGhlIExpY2Vuc2UgYXQ6XHJcbi8vXHJcbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxyXG4vL1xyXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxyXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXHJcbi8vICBMaWNlbnNlIGZvciB0aGUgc3BlY2lmaWMgbGFuZ3VhZ2UgZ292ZXJuaW5nIHBlcm1pc3Npb25zIGFuZCBsaW1pdGF0aW9ucy5cclxuLy9cclxuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XHJcbi8vICAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vICAwNy8xNi8yMDIxIC0gQy4gTGFja25lclxyXG4vLyAgICAgICBHZW5lcmF0ZWQgb3JpZ2luYWwgdmVyc2lvbiBvZiBzb3VyY2UgY29kZS5cclxuLy9cclxuLy8gKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcIl9fZXNNb2R1bGVcIiwgeyB2YWx1ZTogdHJ1ZSB9KTtcclxuZXhwb3J0cy5Jc0ludGVnZXIgPSB2b2lkIDA7XHJcbi8qKlxyXG4gKiBJc0ludGVnZXIgY2hlY2tzIGlmIHZhbHVlIHBhc3NlZCB0aHJvdWdoIGlzIGFuIGludGVnZXIsIHJldHVybmluZyBhIHRydWUgb3IgZmFsc2VcclxuICogQHBhcmFtIHZhbHVlOiB2YWx1ZSBpcyB0aGUgaW5wdXQgcGFzc2VkIHRocm91Z2ggdGhlIElzSW50ZWdlciBmdW5jdGlvblxyXG4gKi9cclxuZnVuY3Rpb24gSXNJbnRlZ2VyKHZhbHVlKSB7XHJcbiAgICB2YXIgcmVnZXggPSAvXi0/WzAtOV0rJC87XHJcbiAgICByZXR1cm4gdmFsdWUudG9TdHJpbmcoKS5tYXRjaChyZWdleCkgIT0gbnVsbDtcclxufVxyXG5leHBvcnRzLklzSW50ZWdlciA9IElzSW50ZWdlcjtcclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbi8vICoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG4vLyAgSXNOdW1iZXIudHN4IC0gR2J0Y1xyXG4vL1xyXG4vLyAgQ29weXJpZ2h0IMKpIDIwMjEsIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZS4gIEFsbCBSaWdodHMgUmVzZXJ2ZWQuXHJcbi8vXHJcbi8vICBMaWNlbnNlZCB0byB0aGUgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlIChHUEEpIHVuZGVyIG9uZSBvciBtb3JlIGNvbnRyaWJ1dG9yIGxpY2Vuc2UgYWdyZWVtZW50cy4gU2VlXHJcbi8vICB0aGUgTk9USUNFIGZpbGUgZGlzdHJpYnV0ZWQgd2l0aCB0aGlzIHdvcmsgZm9yIGFkZGl0aW9uYWwgaW5mb3JtYXRpb24gcmVnYXJkaW5nIGNvcHlyaWdodCBvd25lcnNoaXAuXHJcbi8vICBUaGUgR1BBIGxpY2Vuc2VzIHRoaXMgZmlsZSB0byB5b3UgdW5kZXIgdGhlIE1JVCBMaWNlbnNlIChNSVQpLCB0aGUgXCJMaWNlbnNlXCI7IHlvdSBtYXkgbm90IHVzZSB0aGlzXHJcbi8vICBmaWxlIGV4Y2VwdCBpbiBjb21wbGlhbmNlIHdpdGggdGhlIExpY2Vuc2UuIFlvdSBtYXkgb2J0YWluIGEgY29weSBvZiB0aGUgTGljZW5zZSBhdDpcclxuLy9cclxuLy8gICAgICBodHRwOi8vb3BlbnNvdXJjZS5vcmcvbGljZW5zZXMvTUlUXHJcbi8vXHJcbi8vICBVbmxlc3MgYWdyZWVkIHRvIGluIHdyaXRpbmcsIHRoZSBzdWJqZWN0IHNvZnR3YXJlIGRpc3RyaWJ1dGVkIHVuZGVyIHRoZSBMaWNlbnNlIGlzIGRpc3RyaWJ1dGVkIG9uIGFuXHJcbi8vICBcIkFTLUlTXCIgQkFTSVMsIFdJVEhPVVQgV0FSUkFOVElFUyBPUiBDT05ESVRJT05TIE9GIEFOWSBLSU5ELCBlaXRoZXIgZXhwcmVzcyBvciBpbXBsaWVkLiBSZWZlciB0byB0aGVcclxuLy8gIExpY2Vuc2UgZm9yIHRoZSBzcGVjaWZpYyBsYW5ndWFnZSBnb3Zlcm5pbmcgcGVybWlzc2lvbnMgYW5kIGxpbWl0YXRpb25zLlxyXG4vL1xyXG4vLyAgQ29kZSBNb2RpZmljYXRpb24gSGlzdG9yeTpcclxuLy8gIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuLy8gIDAxLzA3LzIwMjEgLSBCaWxseSBFcm5lc3RcclxuLy8gICAgICAgR2VuZXJhdGVkIG9yaWdpbmFsIHZlcnNpb24gb2Ygc291cmNlIGNvZGUuXHJcbi8vXHJcbi8vICoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJfX2VzTW9kdWxlXCIsIHsgdmFsdWU6IHRydWUgfSk7XHJcbmV4cG9ydHMuSXNOdW1iZXIgPSB2b2lkIDA7XHJcbi8qKlxyXG4gKiBUaGlzIGZ1bmN0aW9uIGNoZWNrcyBpZiBhbnkgdmFsdWUgaXMgYSBudW1iZXIsIHJldHVybmluZyB0cnVlIG9yIGZhbHNlXHJcbiAqIEBwYXJhbSB2YWx1ZTogdmFsdWUgaXMgdGhlIGlucHV0IHBhc3NlZCB0aHJvdWdoIHRoZSBJc051bWJlciBmdW5jdGlvblxyXG4gKi9cclxuZnVuY3Rpb24gSXNOdW1iZXIodmFsdWUpIHtcclxuICAgIHZhciByZWdleCA9IC9eLT9bMC05XSsoXFwuWzAtOV0rKT8kLztcclxuICAgIHJldHVybiB2YWx1ZS50b1N0cmluZygpLm1hdGNoKHJlZ2V4KSAhPSBudWxsO1xyXG59XHJcbmV4cG9ydHMuSXNOdW1iZXIgPSBJc051bWJlcjtcclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbi8vICoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG4vLyAgUmFuZG9tQ29sb3IudHN4IC0gR2J0Y1xyXG4vL1xyXG4vLyAgQ29weXJpZ2h0IMKpIDIwMjEsIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZS4gIEFsbCBSaWdodHMgUmVzZXJ2ZWQuXHJcbi8vXHJcbi8vICBMaWNlbnNlZCB0byB0aGUgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlIChHUEEpIHVuZGVyIG9uZSBvciBtb3JlIGNvbnRyaWJ1dG9yIGxpY2Vuc2UgYWdyZWVtZW50cy4gU2VlXHJcbi8vICB0aGUgTk9USUNFIGZpbGUgZGlzdHJpYnV0ZWQgd2l0aCB0aGlzIHdvcmsgZm9yIGFkZGl0aW9uYWwgaW5mb3JtYXRpb24gcmVnYXJkaW5nIGNvcHlyaWdodCBvd25lcnNoaXAuXHJcbi8vICBUaGUgR1BBIGxpY2Vuc2VzIHRoaXMgZmlsZSB0byB5b3UgdW5kZXIgdGhlIE1JVCBMaWNlbnNlIChNSVQpLCB0aGUgXCJMaWNlbnNlXCI7IHlvdSBtYXkgbm90IHVzZSB0aGlzXHJcbi8vICBmaWxlIGV4Y2VwdCBpbiBjb21wbGlhbmNlIHdpdGggdGhlIExpY2Vuc2UuIFlvdSBtYXkgb2J0YWluIGEgY29weSBvZiB0aGUgTGljZW5zZSBhdDpcclxuLy9cclxuLy8gICAgICBodHRwOi8vb3BlbnNvdXJjZS5vcmcvbGljZW5zZXMvTUlUXHJcbi8vXHJcbi8vICBVbmxlc3MgYWdyZWVkIHRvIGluIHdyaXRpbmcsIHRoZSBzdWJqZWN0IHNvZnR3YXJlIGRpc3RyaWJ1dGVkIHVuZGVyIHRoZSBMaWNlbnNlIGlzIGRpc3RyaWJ1dGVkIG9uIGFuXHJcbi8vICBcIkFTLUlTXCIgQkFTSVMsIFdJVEhPVVQgV0FSUkFOVElFUyBPUiBDT05ESVRJT05TIE9GIEFOWSBLSU5ELCBlaXRoZXIgZXhwcmVzcyBvciBpbXBsaWVkLiBSZWZlciB0byB0aGVcclxuLy8gIExpY2Vuc2UgZm9yIHRoZSBzcGVjaWZpYyBsYW5ndWFnZSBnb3Zlcm5pbmcgcGVybWlzc2lvbnMgYW5kIGxpbWl0YXRpb25zLlxyXG4vL1xyXG4vLyAgQ29kZSBNb2RpZmljYXRpb24gSGlzdG9yeTpcclxuLy8gIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuLy8gIDAxLzE1LzIwMjEgLSBDLiBMYWNrbmVyXHJcbi8vICAgICAgIEdlbmVyYXRlZCBvcmlnaW5hbCB2ZXJzaW9uIG9mIHNvdXJjZSBjb2RlLlxyXG4vL1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuT2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFwiX19lc01vZHVsZVwiLCB7IHZhbHVlOiB0cnVlIH0pO1xyXG5leHBvcnRzLlJhbmRvbUNvbG9yID0gdm9pZCAwO1xyXG4vKipcclxuICogVGhpcyBmdW5jdGlvbiByZXR1cm5zIGEgcmFuZG9tIGNvbG9yXHJcbiAqL1xyXG5mdW5jdGlvbiBSYW5kb21Db2xvcigpIHtcclxuICAgIHJldHVybiAnIycgKyBNYXRoLnJhbmRvbSgpLnRvU3RyaW5nKDE2KS5zdWJzdHIoMiwgNikudG9VcHBlckNhc2UoKTtcclxufVxyXG5leHBvcnRzLlJhbmRvbUNvbG9yID0gUmFuZG9tQ29sb3I7XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuLy8gIGluZGV4LnRzIC0gR2J0Y1xyXG4vL1xyXG4vLyAgQ29weXJpZ2h0IO+/vSAyMDIxLCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxyXG4vL1xyXG4vLyAgTGljZW5zZWQgdG8gdGhlIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZSAoR1BBKSB1bmRlciBvbmUgb3IgbW9yZSBjb250cmlidXRvciBsaWNlbnNlIGFncmVlbWVudHMuIFNlZVxyXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxyXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xyXG4vLyAgZmlsZSBleGNlcHQgaW4gY29tcGxpYW5jZSB3aXRoIHRoZSBMaWNlbnNlLiBZb3UgbWF5IG9idGFpbiBhIGNvcHkgb2YgdGhlIExpY2Vuc2UgYXQ6XHJcbi8vXHJcbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxyXG4vL1xyXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxyXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXHJcbi8vICBMaWNlbnNlIGZvciB0aGUgc3BlY2lmaWMgbGFuZ3VhZ2UgZ292ZXJuaW5nIHBlcm1pc3Npb25zIGFuZCBsaW1pdGF0aW9ucy5cclxuLy9cclxuLy8gIGh0dHBzOi8vc3RhY2tvdmVyZmxvdy5jb20vcXVlc3Rpb25zLzEwNTAzNC9ob3ctdG8tY3JlYXRlLWEtZ3VpZC11dWlkXHJcbi8vXHJcbi8vICBDb2RlIE1vZGlmaWNhdGlvbiBIaXN0b3J5OlxyXG4vLyAgLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG4vLyAgMDEvMDQvMjAyMSAtIEJpbGx5IEVybmVzdFxyXG4vLyAgICAgICBHZW5lcmF0ZWQgb3JpZ2luYWwgdmVyc2lvbiBvZiBzb3VyY2UgY29kZS5cclxuLy9cclxuLy8gKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcIl9fZXNNb2R1bGVcIiwgeyB2YWx1ZTogdHJ1ZSB9KTtcclxuZXhwb3J0cy5Jc0ludGVnZXIgPSBleHBvcnRzLklzTnVtYmVyID0gZXhwb3J0cy5HZXRUZXh0SGVpZ2h0ID0gZXhwb3J0cy5SYW5kb21Db2xvciA9IGV4cG9ydHMuR2V0Tm9kZVNpemUgPSBleHBvcnRzLkdldFRleHRXaWR0aCA9IGV4cG9ydHMuQ3JlYXRlR3VpZCA9IHZvaWQgMDtcclxudmFyIENyZWF0ZUd1aWRfMSA9IHJlcXVpcmUoXCIuL0NyZWF0ZUd1aWRcIik7XHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcIkNyZWF0ZUd1aWRcIiwgeyBlbnVtZXJhYmxlOiB0cnVlLCBnZXQ6IGZ1bmN0aW9uICgpIHsgcmV0dXJuIENyZWF0ZUd1aWRfMS5DcmVhdGVHdWlkOyB9IH0pO1xyXG52YXIgR2V0VGV4dFdpZHRoXzEgPSByZXF1aXJlKFwiLi9HZXRUZXh0V2lkdGhcIik7XHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcIkdldFRleHRXaWR0aFwiLCB7IGVudW1lcmFibGU6IHRydWUsIGdldDogZnVuY3Rpb24gKCkgeyByZXR1cm4gR2V0VGV4dFdpZHRoXzEuR2V0VGV4dFdpZHRoOyB9IH0pO1xyXG52YXIgR2V0VGV4dEhlaWdodF8xID0gcmVxdWlyZShcIi4vR2V0VGV4dEhlaWdodFwiKTtcclxuT2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFwiR2V0VGV4dEhlaWdodFwiLCB7IGVudW1lcmFibGU6IHRydWUsIGdldDogZnVuY3Rpb24gKCkgeyByZXR1cm4gR2V0VGV4dEhlaWdodF8xLkdldFRleHRIZWlnaHQ7IH0gfSk7XHJcbnZhciBHZXROb2RlU2l6ZV8xID0gcmVxdWlyZShcIi4vR2V0Tm9kZVNpemVcIik7XHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcIkdldE5vZGVTaXplXCIsIHsgZW51bWVyYWJsZTogdHJ1ZSwgZ2V0OiBmdW5jdGlvbiAoKSB7IHJldHVybiBHZXROb2RlU2l6ZV8xLkdldE5vZGVTaXplOyB9IH0pO1xyXG52YXIgUmFuZG9tQ29sb3JfMSA9IHJlcXVpcmUoXCIuL1JhbmRvbUNvbG9yXCIpO1xyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJSYW5kb21Db2xvclwiLCB7IGVudW1lcmFibGU6IHRydWUsIGdldDogZnVuY3Rpb24gKCkgeyByZXR1cm4gUmFuZG9tQ29sb3JfMS5SYW5kb21Db2xvcjsgfSB9KTtcclxudmFyIElzTnVtYmVyXzEgPSByZXF1aXJlKFwiLi9Jc051bWJlclwiKTtcclxuT2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFwiSXNOdW1iZXJcIiwgeyBlbnVtZXJhYmxlOiB0cnVlLCBnZXQ6IGZ1bmN0aW9uICgpIHsgcmV0dXJuIElzTnVtYmVyXzEuSXNOdW1iZXI7IH0gfSk7XHJcbnZhciBJc0ludGVnZXJfMSA9IHJlcXVpcmUoXCIuL0lzSW50ZWdlclwiKTtcclxuT2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFwiSXNJbnRlZ2VyXCIsIHsgZW51bWVyYWJsZTogdHJ1ZSwgZ2V0OiBmdW5jdGlvbiAoKSB7IHJldHVybiBJc0ludGVnZXJfMS5Jc0ludGVnZXI7IH0gfSk7XHJcbiIsIi8qKlxuICogQ29weXJpZ2h0IChjKSAyMDEzLXByZXNlbnQsIEZhY2Vib29rLCBJbmMuXG4gKlxuICogVGhpcyBzb3VyY2UgY29kZSBpcyBsaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UgZm91bmQgaW4gdGhlXG4gKiBMSUNFTlNFIGZpbGUgaW4gdGhlIHJvb3QgZGlyZWN0b3J5IG9mIHRoaXMgc291cmNlIHRyZWUuXG4gKlxuICovXG5cbid1c2Ugc3RyaWN0JztcblxudmFyIF9hc3NpZ24gPSByZXF1aXJlKCdvYmplY3QtYXNzaWduJyk7XG5cbi8vIC0tIElubGluZWQgZnJvbSBmYmpzIC0tXG5cbnZhciBlbXB0eU9iamVjdCA9IHt9O1xuXG5pZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICBPYmplY3QuZnJlZXplKGVtcHR5T2JqZWN0KTtcbn1cblxudmFyIHZhbGlkYXRlRm9ybWF0ID0gZnVuY3Rpb24gdmFsaWRhdGVGb3JtYXQoZm9ybWF0KSB7fTtcblxuaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgdmFsaWRhdGVGb3JtYXQgPSBmdW5jdGlvbiB2YWxpZGF0ZUZvcm1hdChmb3JtYXQpIHtcbiAgICBpZiAoZm9ybWF0ID09PSB1bmRlZmluZWQpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcignaW52YXJpYW50IHJlcXVpcmVzIGFuIGVycm9yIG1lc3NhZ2UgYXJndW1lbnQnKTtcbiAgICB9XG4gIH07XG59XG5cbmZ1bmN0aW9uIF9pbnZhcmlhbnQoY29uZGl0aW9uLCBmb3JtYXQsIGEsIGIsIGMsIGQsIGUsIGYpIHtcbiAgdmFsaWRhdGVGb3JtYXQoZm9ybWF0KTtcblxuICBpZiAoIWNvbmRpdGlvbikge1xuICAgIHZhciBlcnJvcjtcbiAgICBpZiAoZm9ybWF0ID09PSB1bmRlZmluZWQpIHtcbiAgICAgIGVycm9yID0gbmV3IEVycm9yKCdNaW5pZmllZCBleGNlcHRpb24gb2NjdXJyZWQ7IHVzZSB0aGUgbm9uLW1pbmlmaWVkIGRldiBlbnZpcm9ubWVudCAnICsgJ2ZvciB0aGUgZnVsbCBlcnJvciBtZXNzYWdlIGFuZCBhZGRpdGlvbmFsIGhlbHBmdWwgd2FybmluZ3MuJyk7XG4gICAgfSBlbHNlIHtcbiAgICAgIHZhciBhcmdzID0gW2EsIGIsIGMsIGQsIGUsIGZdO1xuICAgICAgdmFyIGFyZ0luZGV4ID0gMDtcbiAgICAgIGVycm9yID0gbmV3IEVycm9yKGZvcm1hdC5yZXBsYWNlKC8lcy9nLCBmdW5jdGlvbiAoKSB7XG4gICAgICAgIHJldHVybiBhcmdzW2FyZ0luZGV4KytdO1xuICAgICAgfSkpO1xuICAgICAgZXJyb3IubmFtZSA9ICdJbnZhcmlhbnQgVmlvbGF0aW9uJztcbiAgICB9XG5cbiAgICBlcnJvci5mcmFtZXNUb1BvcCA9IDE7IC8vIHdlIGRvbid0IGNhcmUgYWJvdXQgaW52YXJpYW50J3Mgb3duIGZyYW1lXG4gICAgdGhyb3cgZXJyb3I7XG4gIH1cbn1cblxudmFyIHdhcm5pbmcgPSBmdW5jdGlvbigpe307XG5cbmlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gIHZhciBwcmludFdhcm5pbmcgPSBmdW5jdGlvbiBwcmludFdhcm5pbmcoZm9ybWF0KSB7XG4gICAgZm9yICh2YXIgX2xlbiA9IGFyZ3VtZW50cy5sZW5ndGgsIGFyZ3MgPSBBcnJheShfbGVuID4gMSA/IF9sZW4gLSAxIDogMCksIF9rZXkgPSAxOyBfa2V5IDwgX2xlbjsgX2tleSsrKSB7XG4gICAgICBhcmdzW19rZXkgLSAxXSA9IGFyZ3VtZW50c1tfa2V5XTtcbiAgICB9XG5cbiAgICB2YXIgYXJnSW5kZXggPSAwO1xuICAgIHZhciBtZXNzYWdlID0gJ1dhcm5pbmc6ICcgKyBmb3JtYXQucmVwbGFjZSgvJXMvZywgZnVuY3Rpb24gKCkge1xuICAgICAgcmV0dXJuIGFyZ3NbYXJnSW5kZXgrK107XG4gICAgfSk7XG4gICAgaWYgKHR5cGVvZiBjb25zb2xlICE9PSAndW5kZWZpbmVkJykge1xuICAgICAgY29uc29sZS5lcnJvcihtZXNzYWdlKTtcbiAgICB9XG4gICAgdHJ5IHtcbiAgICAgIC8vIC0tLSBXZWxjb21lIHRvIGRlYnVnZ2luZyBSZWFjdCAtLS1cbiAgICAgIC8vIFRoaXMgZXJyb3Igd2FzIHRocm93biBhcyBhIGNvbnZlbmllbmNlIHNvIHRoYXQgeW91IGNhbiB1c2UgdGhpcyBzdGFja1xuICAgICAgLy8gdG8gZmluZCB0aGUgY2FsbHNpdGUgdGhhdCBjYXVzZWQgdGhpcyB3YXJuaW5nIHRvIGZpcmUuXG4gICAgICB0aHJvdyBuZXcgRXJyb3IobWVzc2FnZSk7XG4gICAgfSBjYXRjaCAoeCkge31cbiAgfTtcblxuICB3YXJuaW5nID0gZnVuY3Rpb24gd2FybmluZyhjb25kaXRpb24sIGZvcm1hdCkge1xuICAgIGlmIChmb3JtYXQgPT09IHVuZGVmaW5lZCkge1xuICAgICAgdGhyb3cgbmV3IEVycm9yKCdgd2FybmluZyhjb25kaXRpb24sIGZvcm1hdCwgLi4uYXJncylgIHJlcXVpcmVzIGEgd2FybmluZyAnICsgJ21lc3NhZ2UgYXJndW1lbnQnKTtcbiAgICB9XG5cbiAgICBpZiAoZm9ybWF0LmluZGV4T2YoJ0ZhaWxlZCBDb21wb3NpdGUgcHJvcFR5cGU6ICcpID09PSAwKSB7XG4gICAgICByZXR1cm47IC8vIElnbm9yZSBDb21wb3NpdGVDb21wb25lbnQgcHJvcHR5cGUgY2hlY2suXG4gICAgfVxuXG4gICAgaWYgKCFjb25kaXRpb24pIHtcbiAgICAgIGZvciAodmFyIF9sZW4yID0gYXJndW1lbnRzLmxlbmd0aCwgYXJncyA9IEFycmF5KF9sZW4yID4gMiA/IF9sZW4yIC0gMiA6IDApLCBfa2V5MiA9IDI7IF9rZXkyIDwgX2xlbjI7IF9rZXkyKyspIHtcbiAgICAgICAgYXJnc1tfa2V5MiAtIDJdID0gYXJndW1lbnRzW19rZXkyXTtcbiAgICAgIH1cblxuICAgICAgcHJpbnRXYXJuaW5nLmFwcGx5KHVuZGVmaW5lZCwgW2Zvcm1hdF0uY29uY2F0KGFyZ3MpKTtcbiAgICB9XG4gIH07XG59XG5cbi8vIC8tLSBJbmxpbmVkIGZyb20gZmJqcyAtLVxuXG52YXIgTUlYSU5TX0tFWSA9ICdtaXhpbnMnO1xuXG4vLyBIZWxwZXIgZnVuY3Rpb24gdG8gYWxsb3cgdGhlIGNyZWF0aW9uIG9mIGFub255bW91cyBmdW5jdGlvbnMgd2hpY2ggZG8gbm90XG4vLyBoYXZlIC5uYW1lIHNldCB0byB0aGUgbmFtZSBvZiB0aGUgdmFyaWFibGUgYmVpbmcgYXNzaWduZWQgdG8uXG5mdW5jdGlvbiBpZGVudGl0eShmbikge1xuICByZXR1cm4gZm47XG59XG5cbnZhciBSZWFjdFByb3BUeXBlTG9jYXRpb25OYW1lcztcbmlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gIFJlYWN0UHJvcFR5cGVMb2NhdGlvbk5hbWVzID0ge1xuICAgIHByb3A6ICdwcm9wJyxcbiAgICBjb250ZXh0OiAnY29udGV4dCcsXG4gICAgY2hpbGRDb250ZXh0OiAnY2hpbGQgY29udGV4dCdcbiAgfTtcbn0gZWxzZSB7XG4gIFJlYWN0UHJvcFR5cGVMb2NhdGlvbk5hbWVzID0ge307XG59XG5cbmZ1bmN0aW9uIGZhY3RvcnkoUmVhY3RDb21wb25lbnQsIGlzVmFsaWRFbGVtZW50LCBSZWFjdE5vb3BVcGRhdGVRdWV1ZSkge1xuICAvKipcbiAgICogUG9saWNpZXMgdGhhdCBkZXNjcmliZSBtZXRob2RzIGluIGBSZWFjdENsYXNzSW50ZXJmYWNlYC5cbiAgICovXG5cbiAgdmFyIGluamVjdGVkTWl4aW5zID0gW107XG5cbiAgLyoqXG4gICAqIENvbXBvc2l0ZSBjb21wb25lbnRzIGFyZSBoaWdoZXItbGV2ZWwgY29tcG9uZW50cyB0aGF0IGNvbXBvc2Ugb3RoZXIgY29tcG9zaXRlXG4gICAqIG9yIGhvc3QgY29tcG9uZW50cy5cbiAgICpcbiAgICogVG8gY3JlYXRlIGEgbmV3IHR5cGUgb2YgYFJlYWN0Q2xhc3NgLCBwYXNzIGEgc3BlY2lmaWNhdGlvbiBvZlxuICAgKiB5b3VyIG5ldyBjbGFzcyB0byBgUmVhY3QuY3JlYXRlQ2xhc3NgLiBUaGUgb25seSByZXF1aXJlbWVudCBvZiB5b3VyIGNsYXNzXG4gICAqIHNwZWNpZmljYXRpb24gaXMgdGhhdCB5b3UgaW1wbGVtZW50IGEgYHJlbmRlcmAgbWV0aG9kLlxuICAgKlxuICAgKiAgIHZhciBNeUNvbXBvbmVudCA9IFJlYWN0LmNyZWF0ZUNsYXNzKHtcbiAgICogICAgIHJlbmRlcjogZnVuY3Rpb24oKSB7XG4gICAqICAgICAgIHJldHVybiA8ZGl2PkhlbGxvIFdvcmxkPC9kaXY+O1xuICAgKiAgICAgfVxuICAgKiAgIH0pO1xuICAgKlxuICAgKiBUaGUgY2xhc3Mgc3BlY2lmaWNhdGlvbiBzdXBwb3J0cyBhIHNwZWNpZmljIHByb3RvY29sIG9mIG1ldGhvZHMgdGhhdCBoYXZlXG4gICAqIHNwZWNpYWwgbWVhbmluZyAoZS5nLiBgcmVuZGVyYCkuIFNlZSBgUmVhY3RDbGFzc0ludGVyZmFjZWAgZm9yXG4gICAqIG1vcmUgdGhlIGNvbXByZWhlbnNpdmUgcHJvdG9jb2wuIEFueSBvdGhlciBwcm9wZXJ0aWVzIGFuZCBtZXRob2RzIGluIHRoZVxuICAgKiBjbGFzcyBzcGVjaWZpY2F0aW9uIHdpbGwgYmUgYXZhaWxhYmxlIG9uIHRoZSBwcm90b3R5cGUuXG4gICAqXG4gICAqIEBpbnRlcmZhY2UgUmVhY3RDbGFzc0ludGVyZmFjZVxuICAgKiBAaW50ZXJuYWxcbiAgICovXG4gIHZhciBSZWFjdENsYXNzSW50ZXJmYWNlID0ge1xuICAgIC8qKlxuICAgICAqIEFuIGFycmF5IG9mIE1peGluIG9iamVjdHMgdG8gaW5jbHVkZSB3aGVuIGRlZmluaW5nIHlvdXIgY29tcG9uZW50LlxuICAgICAqXG4gICAgICogQHR5cGUge2FycmF5fVxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIG1peGluczogJ0RFRklORV9NQU5ZJyxcblxuICAgIC8qKlxuICAgICAqIEFuIG9iamVjdCBjb250YWluaW5nIHByb3BlcnRpZXMgYW5kIG1ldGhvZHMgdGhhdCBzaG91bGQgYmUgZGVmaW5lZCBvblxuICAgICAqIHRoZSBjb21wb25lbnQncyBjb25zdHJ1Y3RvciBpbnN0ZWFkIG9mIGl0cyBwcm90b3R5cGUgKHN0YXRpYyBtZXRob2RzKS5cbiAgICAgKlxuICAgICAqIEB0eXBlIHtvYmplY3R9XG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgc3RhdGljczogJ0RFRklORV9NQU5ZJyxcblxuICAgIC8qKlxuICAgICAqIERlZmluaXRpb24gb2YgcHJvcCB0eXBlcyBmb3IgdGhpcyBjb21wb25lbnQuXG4gICAgICpcbiAgICAgKiBAdHlwZSB7b2JqZWN0fVxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIHByb3BUeXBlczogJ0RFRklORV9NQU5ZJyxcblxuICAgIC8qKlxuICAgICAqIERlZmluaXRpb24gb2YgY29udGV4dCB0eXBlcyBmb3IgdGhpcyBjb21wb25lbnQuXG4gICAgICpcbiAgICAgKiBAdHlwZSB7b2JqZWN0fVxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIGNvbnRleHRUeXBlczogJ0RFRklORV9NQU5ZJyxcblxuICAgIC8qKlxuICAgICAqIERlZmluaXRpb24gb2YgY29udGV4dCB0eXBlcyB0aGlzIGNvbXBvbmVudCBzZXRzIGZvciBpdHMgY2hpbGRyZW4uXG4gICAgICpcbiAgICAgKiBAdHlwZSB7b2JqZWN0fVxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIGNoaWxkQ29udGV4dFR5cGVzOiAnREVGSU5FX01BTlknLFxuXG4gICAgLy8gPT09PSBEZWZpbml0aW9uIG1ldGhvZHMgPT09PVxuXG4gICAgLyoqXG4gICAgICogSW52b2tlZCB3aGVuIHRoZSBjb21wb25lbnQgaXMgbW91bnRlZC4gVmFsdWVzIGluIHRoZSBtYXBwaW5nIHdpbGwgYmUgc2V0IG9uXG4gICAgICogYHRoaXMucHJvcHNgIGlmIHRoYXQgcHJvcCBpcyBub3Qgc3BlY2lmaWVkIChpLmUuIHVzaW5nIGFuIGBpbmAgY2hlY2spLlxuICAgICAqXG4gICAgICogVGhpcyBtZXRob2QgaXMgaW52b2tlZCBiZWZvcmUgYGdldEluaXRpYWxTdGF0ZWAgYW5kIHRoZXJlZm9yZSBjYW5ub3QgcmVseVxuICAgICAqIG9uIGB0aGlzLnN0YXRlYCBvciB1c2UgYHRoaXMuc2V0U3RhdGVgLlxuICAgICAqXG4gICAgICogQHJldHVybiB7b2JqZWN0fVxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIGdldERlZmF1bHRQcm9wczogJ0RFRklORV9NQU5ZX01FUkdFRCcsXG5cbiAgICAvKipcbiAgICAgKiBJbnZva2VkIG9uY2UgYmVmb3JlIHRoZSBjb21wb25lbnQgaXMgbW91bnRlZC4gVGhlIHJldHVybiB2YWx1ZSB3aWxsIGJlIHVzZWRcbiAgICAgKiBhcyB0aGUgaW5pdGlhbCB2YWx1ZSBvZiBgdGhpcy5zdGF0ZWAuXG4gICAgICpcbiAgICAgKiAgIGdldEluaXRpYWxTdGF0ZTogZnVuY3Rpb24oKSB7XG4gICAgICogICAgIHJldHVybiB7XG4gICAgICogICAgICAgaXNPbjogZmFsc2UsXG4gICAgICogICAgICAgZm9vQmF6OiBuZXcgQmF6Rm9vKClcbiAgICAgKiAgICAgfVxuICAgICAqICAgfVxuICAgICAqXG4gICAgICogQHJldHVybiB7b2JqZWN0fVxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIGdldEluaXRpYWxTdGF0ZTogJ0RFRklORV9NQU5ZX01FUkdFRCcsXG5cbiAgICAvKipcbiAgICAgKiBAcmV0dXJuIHtvYmplY3R9XG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgZ2V0Q2hpbGRDb250ZXh0OiAnREVGSU5FX01BTllfTUVSR0VEJyxcblxuICAgIC8qKlxuICAgICAqIFVzZXMgcHJvcHMgZnJvbSBgdGhpcy5wcm9wc2AgYW5kIHN0YXRlIGZyb20gYHRoaXMuc3RhdGVgIHRvIHJlbmRlciB0aGVcbiAgICAgKiBzdHJ1Y3R1cmUgb2YgdGhlIGNvbXBvbmVudC5cbiAgICAgKlxuICAgICAqIE5vIGd1YXJhbnRlZXMgYXJlIG1hZGUgYWJvdXQgd2hlbiBvciBob3cgb2Z0ZW4gdGhpcyBtZXRob2QgaXMgaW52b2tlZCwgc29cbiAgICAgKiBpdCBtdXN0IG5vdCBoYXZlIHNpZGUgZWZmZWN0cy5cbiAgICAgKlxuICAgICAqICAgcmVuZGVyOiBmdW5jdGlvbigpIHtcbiAgICAgKiAgICAgdmFyIG5hbWUgPSB0aGlzLnByb3BzLm5hbWU7XG4gICAgICogICAgIHJldHVybiA8ZGl2PkhlbGxvLCB7bmFtZX0hPC9kaXY+O1xuICAgICAqICAgfVxuICAgICAqXG4gICAgICogQHJldHVybiB7UmVhY3RDb21wb25lbnR9XG4gICAgICogQHJlcXVpcmVkXG4gICAgICovXG4gICAgcmVuZGVyOiAnREVGSU5FX09OQ0UnLFxuXG4gICAgLy8gPT09PSBEZWxlZ2F0ZSBtZXRob2RzID09PT1cblxuICAgIC8qKlxuICAgICAqIEludm9rZWQgd2hlbiB0aGUgY29tcG9uZW50IGlzIGluaXRpYWxseSBjcmVhdGVkIGFuZCBhYm91dCB0byBiZSBtb3VudGVkLlxuICAgICAqIFRoaXMgbWF5IGhhdmUgc2lkZSBlZmZlY3RzLCBidXQgYW55IGV4dGVybmFsIHN1YnNjcmlwdGlvbnMgb3IgZGF0YSBjcmVhdGVkXG4gICAgICogYnkgdGhpcyBtZXRob2QgbXVzdCBiZSBjbGVhbmVkIHVwIGluIGBjb21wb25lbnRXaWxsVW5tb3VudGAuXG4gICAgICpcbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBjb21wb25lbnRXaWxsTW91bnQ6ICdERUZJTkVfTUFOWScsXG5cbiAgICAvKipcbiAgICAgKiBJbnZva2VkIHdoZW4gdGhlIGNvbXBvbmVudCBoYXMgYmVlbiBtb3VudGVkIGFuZCBoYXMgYSBET00gcmVwcmVzZW50YXRpb24uXG4gICAgICogSG93ZXZlciwgdGhlcmUgaXMgbm8gZ3VhcmFudGVlIHRoYXQgdGhlIERPTSBub2RlIGlzIGluIHRoZSBkb2N1bWVudC5cbiAgICAgKlxuICAgICAqIFVzZSB0aGlzIGFzIGFuIG9wcG9ydHVuaXR5IHRvIG9wZXJhdGUgb24gdGhlIERPTSB3aGVuIHRoZSBjb21wb25lbnQgaGFzXG4gICAgICogYmVlbiBtb3VudGVkIChpbml0aWFsaXplZCBhbmQgcmVuZGVyZWQpIGZvciB0aGUgZmlyc3QgdGltZS5cbiAgICAgKlxuICAgICAqIEBwYXJhbSB7RE9NRWxlbWVudH0gcm9vdE5vZGUgRE9NIGVsZW1lbnQgcmVwcmVzZW50aW5nIHRoZSBjb21wb25lbnQuXG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgY29tcG9uZW50RGlkTW91bnQ6ICdERUZJTkVfTUFOWScsXG5cbiAgICAvKipcbiAgICAgKiBJbnZva2VkIGJlZm9yZSB0aGUgY29tcG9uZW50IHJlY2VpdmVzIG5ldyBwcm9wcy5cbiAgICAgKlxuICAgICAqIFVzZSB0aGlzIGFzIGFuIG9wcG9ydHVuaXR5IHRvIHJlYWN0IHRvIGEgcHJvcCB0cmFuc2l0aW9uIGJ5IHVwZGF0aW5nIHRoZVxuICAgICAqIHN0YXRlIHVzaW5nIGB0aGlzLnNldFN0YXRlYC4gQ3VycmVudCBwcm9wcyBhcmUgYWNjZXNzZWQgdmlhIGB0aGlzLnByb3BzYC5cbiAgICAgKlxuICAgICAqICAgY29tcG9uZW50V2lsbFJlY2VpdmVQcm9wczogZnVuY3Rpb24obmV4dFByb3BzLCBuZXh0Q29udGV4dCkge1xuICAgICAqICAgICB0aGlzLnNldFN0YXRlKHtcbiAgICAgKiAgICAgICBsaWtlc0luY3JlYXNpbmc6IG5leHRQcm9wcy5saWtlQ291bnQgPiB0aGlzLnByb3BzLmxpa2VDb3VudFxuICAgICAqICAgICB9KTtcbiAgICAgKiAgIH1cbiAgICAgKlxuICAgICAqIE5PVEU6IFRoZXJlIGlzIG5vIGVxdWl2YWxlbnQgYGNvbXBvbmVudFdpbGxSZWNlaXZlU3RhdGVgLiBBbiBpbmNvbWluZyBwcm9wXG4gICAgICogdHJhbnNpdGlvbiBtYXkgY2F1c2UgYSBzdGF0ZSBjaGFuZ2UsIGJ1dCB0aGUgb3Bwb3NpdGUgaXMgbm90IHRydWUuIElmIHlvdVxuICAgICAqIG5lZWQgaXQsIHlvdSBhcmUgcHJvYmFibHkgbG9va2luZyBmb3IgYGNvbXBvbmVudFdpbGxVcGRhdGVgLlxuICAgICAqXG4gICAgICogQHBhcmFtIHtvYmplY3R9IG5leHRQcm9wc1xuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIGNvbXBvbmVudFdpbGxSZWNlaXZlUHJvcHM6ICdERUZJTkVfTUFOWScsXG5cbiAgICAvKipcbiAgICAgKiBJbnZva2VkIHdoaWxlIGRlY2lkaW5nIGlmIHRoZSBjb21wb25lbnQgc2hvdWxkIGJlIHVwZGF0ZWQgYXMgYSByZXN1bHQgb2ZcbiAgICAgKiByZWNlaXZpbmcgbmV3IHByb3BzLCBzdGF0ZSBhbmQvb3IgY29udGV4dC5cbiAgICAgKlxuICAgICAqIFVzZSB0aGlzIGFzIGFuIG9wcG9ydHVuaXR5IHRvIGByZXR1cm4gZmFsc2VgIHdoZW4geW91J3JlIGNlcnRhaW4gdGhhdCB0aGVcbiAgICAgKiB0cmFuc2l0aW9uIHRvIHRoZSBuZXcgcHJvcHMvc3RhdGUvY29udGV4dCB3aWxsIG5vdCByZXF1aXJlIGEgY29tcG9uZW50XG4gICAgICogdXBkYXRlLlxuICAgICAqXG4gICAgICogICBzaG91bGRDb21wb25lbnRVcGRhdGU6IGZ1bmN0aW9uKG5leHRQcm9wcywgbmV4dFN0YXRlLCBuZXh0Q29udGV4dCkge1xuICAgICAqICAgICByZXR1cm4gIWVxdWFsKG5leHRQcm9wcywgdGhpcy5wcm9wcykgfHxcbiAgICAgKiAgICAgICAhZXF1YWwobmV4dFN0YXRlLCB0aGlzLnN0YXRlKSB8fFxuICAgICAqICAgICAgICFlcXVhbChuZXh0Q29udGV4dCwgdGhpcy5jb250ZXh0KTtcbiAgICAgKiAgIH1cbiAgICAgKlxuICAgICAqIEBwYXJhbSB7b2JqZWN0fSBuZXh0UHJvcHNcbiAgICAgKiBAcGFyYW0gez9vYmplY3R9IG5leHRTdGF0ZVxuICAgICAqIEBwYXJhbSB7P29iamVjdH0gbmV4dENvbnRleHRcbiAgICAgKiBAcmV0dXJuIHtib29sZWFufSBUcnVlIGlmIHRoZSBjb21wb25lbnQgc2hvdWxkIHVwZGF0ZS5cbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBzaG91bGRDb21wb25lbnRVcGRhdGU6ICdERUZJTkVfT05DRScsXG5cbiAgICAvKipcbiAgICAgKiBJbnZva2VkIHdoZW4gdGhlIGNvbXBvbmVudCBpcyBhYm91dCB0byB1cGRhdGUgZHVlIHRvIGEgdHJhbnNpdGlvbiBmcm9tXG4gICAgICogYHRoaXMucHJvcHNgLCBgdGhpcy5zdGF0ZWAgYW5kIGB0aGlzLmNvbnRleHRgIHRvIGBuZXh0UHJvcHNgLCBgbmV4dFN0YXRlYFxuICAgICAqIGFuZCBgbmV4dENvbnRleHRgLlxuICAgICAqXG4gICAgICogVXNlIHRoaXMgYXMgYW4gb3Bwb3J0dW5pdHkgdG8gcGVyZm9ybSBwcmVwYXJhdGlvbiBiZWZvcmUgYW4gdXBkYXRlIG9jY3Vycy5cbiAgICAgKlxuICAgICAqIE5PVEU6IFlvdSAqKmNhbm5vdCoqIHVzZSBgdGhpcy5zZXRTdGF0ZSgpYCBpbiB0aGlzIG1ldGhvZC5cbiAgICAgKlxuICAgICAqIEBwYXJhbSB7b2JqZWN0fSBuZXh0UHJvcHNcbiAgICAgKiBAcGFyYW0gez9vYmplY3R9IG5leHRTdGF0ZVxuICAgICAqIEBwYXJhbSB7P29iamVjdH0gbmV4dENvbnRleHRcbiAgICAgKiBAcGFyYW0ge1JlYWN0UmVjb25jaWxlVHJhbnNhY3Rpb259IHRyYW5zYWN0aW9uXG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgY29tcG9uZW50V2lsbFVwZGF0ZTogJ0RFRklORV9NQU5ZJyxcblxuICAgIC8qKlxuICAgICAqIEludm9rZWQgd2hlbiB0aGUgY29tcG9uZW50J3MgRE9NIHJlcHJlc2VudGF0aW9uIGhhcyBiZWVuIHVwZGF0ZWQuXG4gICAgICpcbiAgICAgKiBVc2UgdGhpcyBhcyBhbiBvcHBvcnR1bml0eSB0byBvcGVyYXRlIG9uIHRoZSBET00gd2hlbiB0aGUgY29tcG9uZW50IGhhc1xuICAgICAqIGJlZW4gdXBkYXRlZC5cbiAgICAgKlxuICAgICAqIEBwYXJhbSB7b2JqZWN0fSBwcmV2UHJvcHNcbiAgICAgKiBAcGFyYW0gez9vYmplY3R9IHByZXZTdGF0ZVxuICAgICAqIEBwYXJhbSB7P29iamVjdH0gcHJldkNvbnRleHRcbiAgICAgKiBAcGFyYW0ge0RPTUVsZW1lbnR9IHJvb3ROb2RlIERPTSBlbGVtZW50IHJlcHJlc2VudGluZyB0aGUgY29tcG9uZW50LlxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIGNvbXBvbmVudERpZFVwZGF0ZTogJ0RFRklORV9NQU5ZJyxcblxuICAgIC8qKlxuICAgICAqIEludm9rZWQgd2hlbiB0aGUgY29tcG9uZW50IGlzIGFib3V0IHRvIGJlIHJlbW92ZWQgZnJvbSBpdHMgcGFyZW50IGFuZCBoYXZlXG4gICAgICogaXRzIERPTSByZXByZXNlbnRhdGlvbiBkZXN0cm95ZWQuXG4gICAgICpcbiAgICAgKiBVc2UgdGhpcyBhcyBhbiBvcHBvcnR1bml0eSB0byBkZWFsbG9jYXRlIGFueSBleHRlcm5hbCByZXNvdXJjZXMuXG4gICAgICpcbiAgICAgKiBOT1RFOiBUaGVyZSBpcyBubyBgY29tcG9uZW50RGlkVW5tb3VudGAgc2luY2UgeW91ciBjb21wb25lbnQgd2lsbCBoYXZlIGJlZW5cbiAgICAgKiBkZXN0cm95ZWQgYnkgdGhhdCBwb2ludC5cbiAgICAgKlxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIGNvbXBvbmVudFdpbGxVbm1vdW50OiAnREVGSU5FX01BTlknLFxuXG4gICAgLyoqXG4gICAgICogUmVwbGFjZW1lbnQgZm9yIChkZXByZWNhdGVkKSBgY29tcG9uZW50V2lsbE1vdW50YC5cbiAgICAgKlxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIFVOU0FGRV9jb21wb25lbnRXaWxsTW91bnQ6ICdERUZJTkVfTUFOWScsXG5cbiAgICAvKipcbiAgICAgKiBSZXBsYWNlbWVudCBmb3IgKGRlcHJlY2F0ZWQpIGBjb21wb25lbnRXaWxsUmVjZWl2ZVByb3BzYC5cbiAgICAgKlxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIFVOU0FGRV9jb21wb25lbnRXaWxsUmVjZWl2ZVByb3BzOiAnREVGSU5FX01BTlknLFxuXG4gICAgLyoqXG4gICAgICogUmVwbGFjZW1lbnQgZm9yIChkZXByZWNhdGVkKSBgY29tcG9uZW50V2lsbFVwZGF0ZWAuXG4gICAgICpcbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBVTlNBRkVfY29tcG9uZW50V2lsbFVwZGF0ZTogJ0RFRklORV9NQU5ZJyxcblxuICAgIC8vID09PT0gQWR2YW5jZWQgbWV0aG9kcyA9PT09XG5cbiAgICAvKipcbiAgICAgKiBVcGRhdGVzIHRoZSBjb21wb25lbnQncyBjdXJyZW50bHkgbW91bnRlZCBET00gcmVwcmVzZW50YXRpb24uXG4gICAgICpcbiAgICAgKiBCeSBkZWZhdWx0LCB0aGlzIGltcGxlbWVudHMgUmVhY3QncyByZW5kZXJpbmcgYW5kIHJlY29uY2lsaWF0aW9uIGFsZ29yaXRobS5cbiAgICAgKiBTb3BoaXN0aWNhdGVkIGNsaWVudHMgbWF5IHdpc2ggdG8gb3ZlcnJpZGUgdGhpcy5cbiAgICAgKlxuICAgICAqIEBwYXJhbSB7UmVhY3RSZWNvbmNpbGVUcmFuc2FjdGlvbn0gdHJhbnNhY3Rpb25cbiAgICAgKiBAaW50ZXJuYWxcbiAgICAgKiBAb3ZlcnJpZGFibGVcbiAgICAgKi9cbiAgICB1cGRhdGVDb21wb25lbnQ6ICdPVkVSUklERV9CQVNFJ1xuICB9O1xuXG4gIC8qKlxuICAgKiBTaW1pbGFyIHRvIFJlYWN0Q2xhc3NJbnRlcmZhY2UgYnV0IGZvciBzdGF0aWMgbWV0aG9kcy5cbiAgICovXG4gIHZhciBSZWFjdENsYXNzU3RhdGljSW50ZXJmYWNlID0ge1xuICAgIC8qKlxuICAgICAqIFRoaXMgbWV0aG9kIGlzIGludm9rZWQgYWZ0ZXIgYSBjb21wb25lbnQgaXMgaW5zdGFudGlhdGVkIGFuZCB3aGVuIGl0XG4gICAgICogcmVjZWl2ZXMgbmV3IHByb3BzLiBSZXR1cm4gYW4gb2JqZWN0IHRvIHVwZGF0ZSBzdGF0ZSBpbiByZXNwb25zZSB0b1xuICAgICAqIHByb3AgY2hhbmdlcy4gUmV0dXJuIG51bGwgdG8gaW5kaWNhdGUgbm8gY2hhbmdlIHRvIHN0YXRlLlxuICAgICAqXG4gICAgICogSWYgYW4gb2JqZWN0IGlzIHJldHVybmVkLCBpdHMga2V5cyB3aWxsIGJlIG1lcmdlZCBpbnRvIHRoZSBleGlzdGluZyBzdGF0ZS5cbiAgICAgKlxuICAgICAqIEByZXR1cm4ge29iamVjdCB8fCBudWxsfVxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIGdldERlcml2ZWRTdGF0ZUZyb21Qcm9wczogJ0RFRklORV9NQU5ZX01FUkdFRCdcbiAgfTtcblxuICAvKipcbiAgICogTWFwcGluZyBmcm9tIGNsYXNzIHNwZWNpZmljYXRpb24ga2V5cyB0byBzcGVjaWFsIHByb2Nlc3NpbmcgZnVuY3Rpb25zLlxuICAgKlxuICAgKiBBbHRob3VnaCB0aGVzZSBhcmUgZGVjbGFyZWQgbGlrZSBpbnN0YW5jZSBwcm9wZXJ0aWVzIGluIHRoZSBzcGVjaWZpY2F0aW9uXG4gICAqIHdoZW4gZGVmaW5pbmcgY2xhc3NlcyB1c2luZyBgUmVhY3QuY3JlYXRlQ2xhc3NgLCB0aGV5IGFyZSBhY3R1YWxseSBzdGF0aWNcbiAgICogYW5kIGFyZSBhY2Nlc3NpYmxlIG9uIHRoZSBjb25zdHJ1Y3RvciBpbnN0ZWFkIG9mIHRoZSBwcm90b3R5cGUuIERlc3BpdGVcbiAgICogYmVpbmcgc3RhdGljLCB0aGV5IG11c3QgYmUgZGVmaW5lZCBvdXRzaWRlIG9mIHRoZSBcInN0YXRpY3NcIiBrZXkgdW5kZXJcbiAgICogd2hpY2ggYWxsIG90aGVyIHN0YXRpYyBtZXRob2RzIGFyZSBkZWZpbmVkLlxuICAgKi9cbiAgdmFyIFJFU0VSVkVEX1NQRUNfS0VZUyA9IHtcbiAgICBkaXNwbGF5TmFtZTogZnVuY3Rpb24oQ29uc3RydWN0b3IsIGRpc3BsYXlOYW1lKSB7XG4gICAgICBDb25zdHJ1Y3Rvci5kaXNwbGF5TmFtZSA9IGRpc3BsYXlOYW1lO1xuICAgIH0sXG4gICAgbWl4aW5zOiBmdW5jdGlvbihDb25zdHJ1Y3RvciwgbWl4aW5zKSB7XG4gICAgICBpZiAobWl4aW5zKSB7XG4gICAgICAgIGZvciAodmFyIGkgPSAwOyBpIDwgbWl4aW5zLmxlbmd0aDsgaSsrKSB7XG4gICAgICAgICAgbWl4U3BlY0ludG9Db21wb25lbnQoQ29uc3RydWN0b3IsIG1peGluc1tpXSk7XG4gICAgICAgIH1cbiAgICAgIH1cbiAgICB9LFxuICAgIGNoaWxkQ29udGV4dFR5cGVzOiBmdW5jdGlvbihDb25zdHJ1Y3RvciwgY2hpbGRDb250ZXh0VHlwZXMpIHtcbiAgICAgIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgICAgIHZhbGlkYXRlVHlwZURlZihDb25zdHJ1Y3RvciwgY2hpbGRDb250ZXh0VHlwZXMsICdjaGlsZENvbnRleHQnKTtcbiAgICAgIH1cbiAgICAgIENvbnN0cnVjdG9yLmNoaWxkQ29udGV4dFR5cGVzID0gX2Fzc2lnbihcbiAgICAgICAge30sXG4gICAgICAgIENvbnN0cnVjdG9yLmNoaWxkQ29udGV4dFR5cGVzLFxuICAgICAgICBjaGlsZENvbnRleHRUeXBlc1xuICAgICAgKTtcbiAgICB9LFxuICAgIGNvbnRleHRUeXBlczogZnVuY3Rpb24oQ29uc3RydWN0b3IsIGNvbnRleHRUeXBlcykge1xuICAgICAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICAgICAgdmFsaWRhdGVUeXBlRGVmKENvbnN0cnVjdG9yLCBjb250ZXh0VHlwZXMsICdjb250ZXh0Jyk7XG4gICAgICB9XG4gICAgICBDb25zdHJ1Y3Rvci5jb250ZXh0VHlwZXMgPSBfYXNzaWduKFxuICAgICAgICB7fSxcbiAgICAgICAgQ29uc3RydWN0b3IuY29udGV4dFR5cGVzLFxuICAgICAgICBjb250ZXh0VHlwZXNcbiAgICAgICk7XG4gICAgfSxcbiAgICAvKipcbiAgICAgKiBTcGVjaWFsIGNhc2UgZ2V0RGVmYXVsdFByb3BzIHdoaWNoIHNob3VsZCBtb3ZlIGludG8gc3RhdGljcyBidXQgcmVxdWlyZXNcbiAgICAgKiBhdXRvbWF0aWMgbWVyZ2luZy5cbiAgICAgKi9cbiAgICBnZXREZWZhdWx0UHJvcHM6IGZ1bmN0aW9uKENvbnN0cnVjdG9yLCBnZXREZWZhdWx0UHJvcHMpIHtcbiAgICAgIGlmIChDb25zdHJ1Y3Rvci5nZXREZWZhdWx0UHJvcHMpIHtcbiAgICAgICAgQ29uc3RydWN0b3IuZ2V0RGVmYXVsdFByb3BzID0gY3JlYXRlTWVyZ2VkUmVzdWx0RnVuY3Rpb24oXG4gICAgICAgICAgQ29uc3RydWN0b3IuZ2V0RGVmYXVsdFByb3BzLFxuICAgICAgICAgIGdldERlZmF1bHRQcm9wc1xuICAgICAgICApO1xuICAgICAgfSBlbHNlIHtcbiAgICAgICAgQ29uc3RydWN0b3IuZ2V0RGVmYXVsdFByb3BzID0gZ2V0RGVmYXVsdFByb3BzO1xuICAgICAgfVxuICAgIH0sXG4gICAgcHJvcFR5cGVzOiBmdW5jdGlvbihDb25zdHJ1Y3RvciwgcHJvcFR5cGVzKSB7XG4gICAgICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgICAgICB2YWxpZGF0ZVR5cGVEZWYoQ29uc3RydWN0b3IsIHByb3BUeXBlcywgJ3Byb3AnKTtcbiAgICAgIH1cbiAgICAgIENvbnN0cnVjdG9yLnByb3BUeXBlcyA9IF9hc3NpZ24oe30sIENvbnN0cnVjdG9yLnByb3BUeXBlcywgcHJvcFR5cGVzKTtcbiAgICB9LFxuICAgIHN0YXRpY3M6IGZ1bmN0aW9uKENvbnN0cnVjdG9yLCBzdGF0aWNzKSB7XG4gICAgICBtaXhTdGF0aWNTcGVjSW50b0NvbXBvbmVudChDb25zdHJ1Y3Rvciwgc3RhdGljcyk7XG4gICAgfSxcbiAgICBhdXRvYmluZDogZnVuY3Rpb24oKSB7fVxuICB9O1xuXG4gIGZ1bmN0aW9uIHZhbGlkYXRlVHlwZURlZihDb25zdHJ1Y3RvciwgdHlwZURlZiwgbG9jYXRpb24pIHtcbiAgICBmb3IgKHZhciBwcm9wTmFtZSBpbiB0eXBlRGVmKSB7XG4gICAgICBpZiAodHlwZURlZi5oYXNPd25Qcm9wZXJ0eShwcm9wTmFtZSkpIHtcbiAgICAgICAgLy8gdXNlIGEgd2FybmluZyBpbnN0ZWFkIG9mIGFuIF9pbnZhcmlhbnQgc28gY29tcG9uZW50c1xuICAgICAgICAvLyBkb24ndCBzaG93IHVwIGluIHByb2QgYnV0IG9ubHkgaW4gX19ERVZfX1xuICAgICAgICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgICAgICAgIHdhcm5pbmcoXG4gICAgICAgICAgICB0eXBlb2YgdHlwZURlZltwcm9wTmFtZV0gPT09ICdmdW5jdGlvbicsXG4gICAgICAgICAgICAnJXM6ICVzIHR5cGUgYCVzYCBpcyBpbnZhbGlkOyBpdCBtdXN0IGJlIGEgZnVuY3Rpb24sIHVzdWFsbHkgZnJvbSAnICtcbiAgICAgICAgICAgICAgJ1JlYWN0LlByb3BUeXBlcy4nLFxuICAgICAgICAgICAgQ29uc3RydWN0b3IuZGlzcGxheU5hbWUgfHwgJ1JlYWN0Q2xhc3MnLFxuICAgICAgICAgICAgUmVhY3RQcm9wVHlwZUxvY2F0aW9uTmFtZXNbbG9jYXRpb25dLFxuICAgICAgICAgICAgcHJvcE5hbWVcbiAgICAgICAgICApO1xuICAgICAgICB9XG4gICAgICB9XG4gICAgfVxuICB9XG5cbiAgZnVuY3Rpb24gdmFsaWRhdGVNZXRob2RPdmVycmlkZShpc0FscmVhZHlEZWZpbmVkLCBuYW1lKSB7XG4gICAgdmFyIHNwZWNQb2xpY3kgPSBSZWFjdENsYXNzSW50ZXJmYWNlLmhhc093blByb3BlcnR5KG5hbWUpXG4gICAgICA/IFJlYWN0Q2xhc3NJbnRlcmZhY2VbbmFtZV1cbiAgICAgIDogbnVsbDtcblxuICAgIC8vIERpc2FsbG93IG92ZXJyaWRpbmcgb2YgYmFzZSBjbGFzcyBtZXRob2RzIHVubGVzcyBleHBsaWNpdGx5IGFsbG93ZWQuXG4gICAgaWYgKFJlYWN0Q2xhc3NNaXhpbi5oYXNPd25Qcm9wZXJ0eShuYW1lKSkge1xuICAgICAgX2ludmFyaWFudChcbiAgICAgICAgc3BlY1BvbGljeSA9PT0gJ09WRVJSSURFX0JBU0UnLFxuICAgICAgICAnUmVhY3RDbGFzc0ludGVyZmFjZTogWW91IGFyZSBhdHRlbXB0aW5nIHRvIG92ZXJyaWRlICcgK1xuICAgICAgICAgICdgJXNgIGZyb20geW91ciBjbGFzcyBzcGVjaWZpY2F0aW9uLiBFbnN1cmUgdGhhdCB5b3VyIG1ldGhvZCBuYW1lcyAnICtcbiAgICAgICAgICAnZG8gbm90IG92ZXJsYXAgd2l0aCBSZWFjdCBtZXRob2RzLicsXG4gICAgICAgIG5hbWVcbiAgICAgICk7XG4gICAgfVxuXG4gICAgLy8gRGlzYWxsb3cgZGVmaW5pbmcgbWV0aG9kcyBtb3JlIHRoYW4gb25jZSB1bmxlc3MgZXhwbGljaXRseSBhbGxvd2VkLlxuICAgIGlmIChpc0FscmVhZHlEZWZpbmVkKSB7XG4gICAgICBfaW52YXJpYW50KFxuICAgICAgICBzcGVjUG9saWN5ID09PSAnREVGSU5FX01BTlknIHx8IHNwZWNQb2xpY3kgPT09ICdERUZJTkVfTUFOWV9NRVJHRUQnLFxuICAgICAgICAnUmVhY3RDbGFzc0ludGVyZmFjZTogWW91IGFyZSBhdHRlbXB0aW5nIHRvIGRlZmluZSAnICtcbiAgICAgICAgICAnYCVzYCBvbiB5b3VyIGNvbXBvbmVudCBtb3JlIHRoYW4gb25jZS4gVGhpcyBjb25mbGljdCBtYXkgYmUgZHVlICcgK1xuICAgICAgICAgICd0byBhIG1peGluLicsXG4gICAgICAgIG5hbWVcbiAgICAgICk7XG4gICAgfVxuICB9XG5cbiAgLyoqXG4gICAqIE1peGluIGhlbHBlciB3aGljaCBoYW5kbGVzIHBvbGljeSB2YWxpZGF0aW9uIGFuZCByZXNlcnZlZFxuICAgKiBzcGVjaWZpY2F0aW9uIGtleXMgd2hlbiBidWlsZGluZyBSZWFjdCBjbGFzc2VzLlxuICAgKi9cbiAgZnVuY3Rpb24gbWl4U3BlY0ludG9Db21wb25lbnQoQ29uc3RydWN0b3IsIHNwZWMpIHtcbiAgICBpZiAoIXNwZWMpIHtcbiAgICAgIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgICAgIHZhciB0eXBlb2ZTcGVjID0gdHlwZW9mIHNwZWM7XG4gICAgICAgIHZhciBpc01peGluVmFsaWQgPSB0eXBlb2ZTcGVjID09PSAnb2JqZWN0JyAmJiBzcGVjICE9PSBudWxsO1xuXG4gICAgICAgIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgICAgICAgd2FybmluZyhcbiAgICAgICAgICAgIGlzTWl4aW5WYWxpZCxcbiAgICAgICAgICAgIFwiJXM6IFlvdSdyZSBhdHRlbXB0aW5nIHRvIGluY2x1ZGUgYSBtaXhpbiB0aGF0IGlzIGVpdGhlciBudWxsIFwiICtcbiAgICAgICAgICAgICAgJ29yIG5vdCBhbiBvYmplY3QuIENoZWNrIHRoZSBtaXhpbnMgaW5jbHVkZWQgYnkgdGhlIGNvbXBvbmVudCwgJyArXG4gICAgICAgICAgICAgICdhcyB3ZWxsIGFzIGFueSBtaXhpbnMgdGhleSBpbmNsdWRlIHRoZW1zZWx2ZXMuICcgK1xuICAgICAgICAgICAgICAnRXhwZWN0ZWQgb2JqZWN0IGJ1dCBnb3QgJXMuJyxcbiAgICAgICAgICAgIENvbnN0cnVjdG9yLmRpc3BsYXlOYW1lIHx8ICdSZWFjdENsYXNzJyxcbiAgICAgICAgICAgIHNwZWMgPT09IG51bGwgPyBudWxsIDogdHlwZW9mU3BlY1xuICAgICAgICAgICk7XG4gICAgICAgIH1cbiAgICAgIH1cblxuICAgICAgcmV0dXJuO1xuICAgIH1cblxuICAgIF9pbnZhcmlhbnQoXG4gICAgICB0eXBlb2Ygc3BlYyAhPT0gJ2Z1bmN0aW9uJyxcbiAgICAgIFwiUmVhY3RDbGFzczogWW91J3JlIGF0dGVtcHRpbmcgdG8gXCIgK1xuICAgICAgICAndXNlIGEgY29tcG9uZW50IGNsYXNzIG9yIGZ1bmN0aW9uIGFzIGEgbWl4aW4uIEluc3RlYWQsIGp1c3QgdXNlIGEgJyArXG4gICAgICAgICdyZWd1bGFyIG9iamVjdC4nXG4gICAgKTtcbiAgICBfaW52YXJpYW50KFxuICAgICAgIWlzVmFsaWRFbGVtZW50KHNwZWMpLFxuICAgICAgXCJSZWFjdENsYXNzOiBZb3UncmUgYXR0ZW1wdGluZyB0byBcIiArXG4gICAgICAgICd1c2UgYSBjb21wb25lbnQgYXMgYSBtaXhpbi4gSW5zdGVhZCwganVzdCB1c2UgYSByZWd1bGFyIG9iamVjdC4nXG4gICAgKTtcblxuICAgIHZhciBwcm90byA9IENvbnN0cnVjdG9yLnByb3RvdHlwZTtcbiAgICB2YXIgYXV0b0JpbmRQYWlycyA9IHByb3RvLl9fcmVhY3RBdXRvQmluZFBhaXJzO1xuXG4gICAgLy8gQnkgaGFuZGxpbmcgbWl4aW5zIGJlZm9yZSBhbnkgb3RoZXIgcHJvcGVydGllcywgd2UgZW5zdXJlIHRoZSBzYW1lXG4gICAgLy8gY2hhaW5pbmcgb3JkZXIgaXMgYXBwbGllZCB0byBtZXRob2RzIHdpdGggREVGSU5FX01BTlkgcG9saWN5LCB3aGV0aGVyXG4gICAgLy8gbWl4aW5zIGFyZSBsaXN0ZWQgYmVmb3JlIG9yIGFmdGVyIHRoZXNlIG1ldGhvZHMgaW4gdGhlIHNwZWMuXG4gICAgaWYgKHNwZWMuaGFzT3duUHJvcGVydHkoTUlYSU5TX0tFWSkpIHtcbiAgICAgIFJFU0VSVkVEX1NQRUNfS0VZUy5taXhpbnMoQ29uc3RydWN0b3IsIHNwZWMubWl4aW5zKTtcbiAgICB9XG5cbiAgICBmb3IgKHZhciBuYW1lIGluIHNwZWMpIHtcbiAgICAgIGlmICghc3BlYy5oYXNPd25Qcm9wZXJ0eShuYW1lKSkge1xuICAgICAgICBjb250aW51ZTtcbiAgICAgIH1cblxuICAgICAgaWYgKG5hbWUgPT09IE1JWElOU19LRVkpIHtcbiAgICAgICAgLy8gV2UgaGF2ZSBhbHJlYWR5IGhhbmRsZWQgbWl4aW5zIGluIGEgc3BlY2lhbCBjYXNlIGFib3ZlLlxuICAgICAgICBjb250aW51ZTtcbiAgICAgIH1cblxuICAgICAgdmFyIHByb3BlcnR5ID0gc3BlY1tuYW1lXTtcbiAgICAgIHZhciBpc0FscmVhZHlEZWZpbmVkID0gcHJvdG8uaGFzT3duUHJvcGVydHkobmFtZSk7XG4gICAgICB2YWxpZGF0ZU1ldGhvZE92ZXJyaWRlKGlzQWxyZWFkeURlZmluZWQsIG5hbWUpO1xuXG4gICAgICBpZiAoUkVTRVJWRURfU1BFQ19LRVlTLmhhc093blByb3BlcnR5KG5hbWUpKSB7XG4gICAgICAgIFJFU0VSVkVEX1NQRUNfS0VZU1tuYW1lXShDb25zdHJ1Y3RvciwgcHJvcGVydHkpO1xuICAgICAgfSBlbHNlIHtcbiAgICAgICAgLy8gU2V0dXAgbWV0aG9kcyBvbiBwcm90b3R5cGU6XG4gICAgICAgIC8vIFRoZSBmb2xsb3dpbmcgbWVtYmVyIG1ldGhvZHMgc2hvdWxkIG5vdCBiZSBhdXRvbWF0aWNhbGx5IGJvdW5kOlxuICAgICAgICAvLyAxLiBFeHBlY3RlZCBSZWFjdENsYXNzIG1ldGhvZHMgKGluIHRoZSBcImludGVyZmFjZVwiKS5cbiAgICAgICAgLy8gMi4gT3ZlcnJpZGRlbiBtZXRob2RzICh0aGF0IHdlcmUgbWl4ZWQgaW4pLlxuICAgICAgICB2YXIgaXNSZWFjdENsYXNzTWV0aG9kID0gUmVhY3RDbGFzc0ludGVyZmFjZS5oYXNPd25Qcm9wZXJ0eShuYW1lKTtcbiAgICAgICAgdmFyIGlzRnVuY3Rpb24gPSB0eXBlb2YgcHJvcGVydHkgPT09ICdmdW5jdGlvbic7XG4gICAgICAgIHZhciBzaG91bGRBdXRvQmluZCA9XG4gICAgICAgICAgaXNGdW5jdGlvbiAmJlxuICAgICAgICAgICFpc1JlYWN0Q2xhc3NNZXRob2QgJiZcbiAgICAgICAgICAhaXNBbHJlYWR5RGVmaW5lZCAmJlxuICAgICAgICAgIHNwZWMuYXV0b2JpbmQgIT09IGZhbHNlO1xuXG4gICAgICAgIGlmIChzaG91bGRBdXRvQmluZCkge1xuICAgICAgICAgIGF1dG9CaW5kUGFpcnMucHVzaChuYW1lLCBwcm9wZXJ0eSk7XG4gICAgICAgICAgcHJvdG9bbmFtZV0gPSBwcm9wZXJ0eTtcbiAgICAgICAgfSBlbHNlIHtcbiAgICAgICAgICBpZiAoaXNBbHJlYWR5RGVmaW5lZCkge1xuICAgICAgICAgICAgdmFyIHNwZWNQb2xpY3kgPSBSZWFjdENsYXNzSW50ZXJmYWNlW25hbWVdO1xuXG4gICAgICAgICAgICAvLyBUaGVzZSBjYXNlcyBzaG91bGQgYWxyZWFkeSBiZSBjYXVnaHQgYnkgdmFsaWRhdGVNZXRob2RPdmVycmlkZS5cbiAgICAgICAgICAgIF9pbnZhcmlhbnQoXG4gICAgICAgICAgICAgIGlzUmVhY3RDbGFzc01ldGhvZCAmJlxuICAgICAgICAgICAgICAgIChzcGVjUG9saWN5ID09PSAnREVGSU5FX01BTllfTUVSR0VEJyB8fFxuICAgICAgICAgICAgICAgICAgc3BlY1BvbGljeSA9PT0gJ0RFRklORV9NQU5ZJyksXG4gICAgICAgICAgICAgICdSZWFjdENsYXNzOiBVbmV4cGVjdGVkIHNwZWMgcG9saWN5ICVzIGZvciBrZXkgJXMgJyArXG4gICAgICAgICAgICAgICAgJ3doZW4gbWl4aW5nIGluIGNvbXBvbmVudCBzcGVjcy4nLFxuICAgICAgICAgICAgICBzcGVjUG9saWN5LFxuICAgICAgICAgICAgICBuYW1lXG4gICAgICAgICAgICApO1xuXG4gICAgICAgICAgICAvLyBGb3IgbWV0aG9kcyB3aGljaCBhcmUgZGVmaW5lZCBtb3JlIHRoYW4gb25jZSwgY2FsbCB0aGUgZXhpc3RpbmdcbiAgICAgICAgICAgIC8vIG1ldGhvZHMgYmVmb3JlIGNhbGxpbmcgdGhlIG5ldyBwcm9wZXJ0eSwgbWVyZ2luZyBpZiBhcHByb3ByaWF0ZS5cbiAgICAgICAgICAgIGlmIChzcGVjUG9saWN5ID09PSAnREVGSU5FX01BTllfTUVSR0VEJykge1xuICAgICAgICAgICAgICBwcm90b1tuYW1lXSA9IGNyZWF0ZU1lcmdlZFJlc3VsdEZ1bmN0aW9uKHByb3RvW25hbWVdLCBwcm9wZXJ0eSk7XG4gICAgICAgICAgICB9IGVsc2UgaWYgKHNwZWNQb2xpY3kgPT09ICdERUZJTkVfTUFOWScpIHtcbiAgICAgICAgICAgICAgcHJvdG9bbmFtZV0gPSBjcmVhdGVDaGFpbmVkRnVuY3Rpb24ocHJvdG9bbmFtZV0sIHByb3BlcnR5KTtcbiAgICAgICAgICAgIH1cbiAgICAgICAgICB9IGVsc2Uge1xuICAgICAgICAgICAgcHJvdG9bbmFtZV0gPSBwcm9wZXJ0eTtcbiAgICAgICAgICAgIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgICAgICAgICAgIC8vIEFkZCB2ZXJib3NlIGRpc3BsYXlOYW1lIHRvIHRoZSBmdW5jdGlvbiwgd2hpY2ggaGVscHMgd2hlbiBsb29raW5nXG4gICAgICAgICAgICAgIC8vIGF0IHByb2ZpbGluZyB0b29scy5cbiAgICAgICAgICAgICAgaWYgKHR5cGVvZiBwcm9wZXJ0eSA9PT0gJ2Z1bmN0aW9uJyAmJiBzcGVjLmRpc3BsYXlOYW1lKSB7XG4gICAgICAgICAgICAgICAgcHJvdG9bbmFtZV0uZGlzcGxheU5hbWUgPSBzcGVjLmRpc3BsYXlOYW1lICsgJ18nICsgbmFtZTtcbiAgICAgICAgICAgICAgfVxuICAgICAgICAgICAgfVxuICAgICAgICAgIH1cbiAgICAgICAgfVxuICAgICAgfVxuICAgIH1cbiAgfVxuXG4gIGZ1bmN0aW9uIG1peFN0YXRpY1NwZWNJbnRvQ29tcG9uZW50KENvbnN0cnVjdG9yLCBzdGF0aWNzKSB7XG4gICAgaWYgKCFzdGF0aWNzKSB7XG4gICAgICByZXR1cm47XG4gICAgfVxuXG4gICAgZm9yICh2YXIgbmFtZSBpbiBzdGF0aWNzKSB7XG4gICAgICB2YXIgcHJvcGVydHkgPSBzdGF0aWNzW25hbWVdO1xuICAgICAgaWYgKCFzdGF0aWNzLmhhc093blByb3BlcnR5KG5hbWUpKSB7XG4gICAgICAgIGNvbnRpbnVlO1xuICAgICAgfVxuXG4gICAgICB2YXIgaXNSZXNlcnZlZCA9IG5hbWUgaW4gUkVTRVJWRURfU1BFQ19LRVlTO1xuICAgICAgX2ludmFyaWFudChcbiAgICAgICAgIWlzUmVzZXJ2ZWQsXG4gICAgICAgICdSZWFjdENsYXNzOiBZb3UgYXJlIGF0dGVtcHRpbmcgdG8gZGVmaW5lIGEgcmVzZXJ2ZWQgJyArXG4gICAgICAgICAgJ3Byb3BlcnR5LCBgJXNgLCB0aGF0IHNob3VsZG5cXCd0IGJlIG9uIHRoZSBcInN0YXRpY3NcIiBrZXkuIERlZmluZSBpdCAnICtcbiAgICAgICAgICAnYXMgYW4gaW5zdGFuY2UgcHJvcGVydHkgaW5zdGVhZDsgaXQgd2lsbCBzdGlsbCBiZSBhY2Nlc3NpYmxlIG9uIHRoZSAnICtcbiAgICAgICAgICAnY29uc3RydWN0b3IuJyxcbiAgICAgICAgbmFtZVxuICAgICAgKTtcblxuICAgICAgdmFyIGlzQWxyZWFkeURlZmluZWQgPSBuYW1lIGluIENvbnN0cnVjdG9yO1xuICAgICAgaWYgKGlzQWxyZWFkeURlZmluZWQpIHtcbiAgICAgICAgdmFyIHNwZWNQb2xpY3kgPSBSZWFjdENsYXNzU3RhdGljSW50ZXJmYWNlLmhhc093blByb3BlcnR5KG5hbWUpXG4gICAgICAgICAgPyBSZWFjdENsYXNzU3RhdGljSW50ZXJmYWNlW25hbWVdXG4gICAgICAgICAgOiBudWxsO1xuXG4gICAgICAgIF9pbnZhcmlhbnQoXG4gICAgICAgICAgc3BlY1BvbGljeSA9PT0gJ0RFRklORV9NQU5ZX01FUkdFRCcsXG4gICAgICAgICAgJ1JlYWN0Q2xhc3M6IFlvdSBhcmUgYXR0ZW1wdGluZyB0byBkZWZpbmUgJyArXG4gICAgICAgICAgICAnYCVzYCBvbiB5b3VyIGNvbXBvbmVudCBtb3JlIHRoYW4gb25jZS4gVGhpcyBjb25mbGljdCBtYXkgYmUgJyArXG4gICAgICAgICAgICAnZHVlIHRvIGEgbWl4aW4uJyxcbiAgICAgICAgICBuYW1lXG4gICAgICAgICk7XG5cbiAgICAgICAgQ29uc3RydWN0b3JbbmFtZV0gPSBjcmVhdGVNZXJnZWRSZXN1bHRGdW5jdGlvbihDb25zdHJ1Y3RvcltuYW1lXSwgcHJvcGVydHkpO1xuXG4gICAgICAgIHJldHVybjtcbiAgICAgIH1cblxuICAgICAgQ29uc3RydWN0b3JbbmFtZV0gPSBwcm9wZXJ0eTtcbiAgICB9XG4gIH1cblxuICAvKipcbiAgICogTWVyZ2UgdHdvIG9iamVjdHMsIGJ1dCB0aHJvdyBpZiBib3RoIGNvbnRhaW4gdGhlIHNhbWUga2V5LlxuICAgKlxuICAgKiBAcGFyYW0ge29iamVjdH0gb25lIFRoZSBmaXJzdCBvYmplY3QsIHdoaWNoIGlzIG11dGF0ZWQuXG4gICAqIEBwYXJhbSB7b2JqZWN0fSB0d28gVGhlIHNlY29uZCBvYmplY3RcbiAgICogQHJldHVybiB7b2JqZWN0fSBvbmUgYWZ0ZXIgaXQgaGFzIGJlZW4gbXV0YXRlZCB0byBjb250YWluIGV2ZXJ5dGhpbmcgaW4gdHdvLlxuICAgKi9cbiAgZnVuY3Rpb24gbWVyZ2VJbnRvV2l0aE5vRHVwbGljYXRlS2V5cyhvbmUsIHR3bykge1xuICAgIF9pbnZhcmlhbnQoXG4gICAgICBvbmUgJiYgdHdvICYmIHR5cGVvZiBvbmUgPT09ICdvYmplY3QnICYmIHR5cGVvZiB0d28gPT09ICdvYmplY3QnLFxuICAgICAgJ21lcmdlSW50b1dpdGhOb0R1cGxpY2F0ZUtleXMoKTogQ2Fubm90IG1lcmdlIG5vbi1vYmplY3RzLidcbiAgICApO1xuXG4gICAgZm9yICh2YXIga2V5IGluIHR3bykge1xuICAgICAgaWYgKHR3by5oYXNPd25Qcm9wZXJ0eShrZXkpKSB7XG4gICAgICAgIF9pbnZhcmlhbnQoXG4gICAgICAgICAgb25lW2tleV0gPT09IHVuZGVmaW5lZCxcbiAgICAgICAgICAnbWVyZ2VJbnRvV2l0aE5vRHVwbGljYXRlS2V5cygpOiAnICtcbiAgICAgICAgICAgICdUcmllZCB0byBtZXJnZSB0d28gb2JqZWN0cyB3aXRoIHRoZSBzYW1lIGtleTogYCVzYC4gVGhpcyBjb25mbGljdCAnICtcbiAgICAgICAgICAgICdtYXkgYmUgZHVlIHRvIGEgbWl4aW47IGluIHBhcnRpY3VsYXIsIHRoaXMgbWF5IGJlIGNhdXNlZCBieSB0d28gJyArXG4gICAgICAgICAgICAnZ2V0SW5pdGlhbFN0YXRlKCkgb3IgZ2V0RGVmYXVsdFByb3BzKCkgbWV0aG9kcyByZXR1cm5pbmcgb2JqZWN0cyAnICtcbiAgICAgICAgICAgICd3aXRoIGNsYXNoaW5nIGtleXMuJyxcbiAgICAgICAgICBrZXlcbiAgICAgICAgKTtcbiAgICAgICAgb25lW2tleV0gPSB0d29ba2V5XTtcbiAgICAgIH1cbiAgICB9XG4gICAgcmV0dXJuIG9uZTtcbiAgfVxuXG4gIC8qKlxuICAgKiBDcmVhdGVzIGEgZnVuY3Rpb24gdGhhdCBpbnZva2VzIHR3byBmdW5jdGlvbnMgYW5kIG1lcmdlcyB0aGVpciByZXR1cm4gdmFsdWVzLlxuICAgKlxuICAgKiBAcGFyYW0ge2Z1bmN0aW9ufSBvbmUgRnVuY3Rpb24gdG8gaW52b2tlIGZpcnN0LlxuICAgKiBAcGFyYW0ge2Z1bmN0aW9ufSB0d28gRnVuY3Rpb24gdG8gaW52b2tlIHNlY29uZC5cbiAgICogQHJldHVybiB7ZnVuY3Rpb259IEZ1bmN0aW9uIHRoYXQgaW52b2tlcyB0aGUgdHdvIGFyZ3VtZW50IGZ1bmN0aW9ucy5cbiAgICogQHByaXZhdGVcbiAgICovXG4gIGZ1bmN0aW9uIGNyZWF0ZU1lcmdlZFJlc3VsdEZ1bmN0aW9uKG9uZSwgdHdvKSB7XG4gICAgcmV0dXJuIGZ1bmN0aW9uIG1lcmdlZFJlc3VsdCgpIHtcbiAgICAgIHZhciBhID0gb25lLmFwcGx5KHRoaXMsIGFyZ3VtZW50cyk7XG4gICAgICB2YXIgYiA9IHR3by5hcHBseSh0aGlzLCBhcmd1bWVudHMpO1xuICAgICAgaWYgKGEgPT0gbnVsbCkge1xuICAgICAgICByZXR1cm4gYjtcbiAgICAgIH0gZWxzZSBpZiAoYiA9PSBudWxsKSB7XG4gICAgICAgIHJldHVybiBhO1xuICAgICAgfVxuICAgICAgdmFyIGMgPSB7fTtcbiAgICAgIG1lcmdlSW50b1dpdGhOb0R1cGxpY2F0ZUtleXMoYywgYSk7XG4gICAgICBtZXJnZUludG9XaXRoTm9EdXBsaWNhdGVLZXlzKGMsIGIpO1xuICAgICAgcmV0dXJuIGM7XG4gICAgfTtcbiAgfVxuXG4gIC8qKlxuICAgKiBDcmVhdGVzIGEgZnVuY3Rpb24gdGhhdCBpbnZva2VzIHR3byBmdW5jdGlvbnMgYW5kIGlnbm9yZXMgdGhlaXIgcmV0dXJuIHZhbGVzLlxuICAgKlxuICAgKiBAcGFyYW0ge2Z1bmN0aW9ufSBvbmUgRnVuY3Rpb24gdG8gaW52b2tlIGZpcnN0LlxuICAgKiBAcGFyYW0ge2Z1bmN0aW9ufSB0d28gRnVuY3Rpb24gdG8gaW52b2tlIHNlY29uZC5cbiAgICogQHJldHVybiB7ZnVuY3Rpb259IEZ1bmN0aW9uIHRoYXQgaW52b2tlcyB0aGUgdHdvIGFyZ3VtZW50IGZ1bmN0aW9ucy5cbiAgICogQHByaXZhdGVcbiAgICovXG4gIGZ1bmN0aW9uIGNyZWF0ZUNoYWluZWRGdW5jdGlvbihvbmUsIHR3bykge1xuICAgIHJldHVybiBmdW5jdGlvbiBjaGFpbmVkRnVuY3Rpb24oKSB7XG4gICAgICBvbmUuYXBwbHkodGhpcywgYXJndW1lbnRzKTtcbiAgICAgIHR3by5hcHBseSh0aGlzLCBhcmd1bWVudHMpO1xuICAgIH07XG4gIH1cblxuICAvKipcbiAgICogQmluZHMgYSBtZXRob2QgdG8gdGhlIGNvbXBvbmVudC5cbiAgICpcbiAgICogQHBhcmFtIHtvYmplY3R9IGNvbXBvbmVudCBDb21wb25lbnQgd2hvc2UgbWV0aG9kIGlzIGdvaW5nIHRvIGJlIGJvdW5kLlxuICAgKiBAcGFyYW0ge2Z1bmN0aW9ufSBtZXRob2QgTWV0aG9kIHRvIGJlIGJvdW5kLlxuICAgKiBAcmV0dXJuIHtmdW5jdGlvbn0gVGhlIGJvdW5kIG1ldGhvZC5cbiAgICovXG4gIGZ1bmN0aW9uIGJpbmRBdXRvQmluZE1ldGhvZChjb21wb25lbnQsIG1ldGhvZCkge1xuICAgIHZhciBib3VuZE1ldGhvZCA9IG1ldGhvZC5iaW5kKGNvbXBvbmVudCk7XG4gICAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICAgIGJvdW5kTWV0aG9kLl9fcmVhY3RCb3VuZENvbnRleHQgPSBjb21wb25lbnQ7XG4gICAgICBib3VuZE1ldGhvZC5fX3JlYWN0Qm91bmRNZXRob2QgPSBtZXRob2Q7XG4gICAgICBib3VuZE1ldGhvZC5fX3JlYWN0Qm91bmRBcmd1bWVudHMgPSBudWxsO1xuICAgICAgdmFyIGNvbXBvbmVudE5hbWUgPSBjb21wb25lbnQuY29uc3RydWN0b3IuZGlzcGxheU5hbWU7XG4gICAgICB2YXIgX2JpbmQgPSBib3VuZE1ldGhvZC5iaW5kO1xuICAgICAgYm91bmRNZXRob2QuYmluZCA9IGZ1bmN0aW9uKG5ld1RoaXMpIHtcbiAgICAgICAgZm9yIChcbiAgICAgICAgICB2YXIgX2xlbiA9IGFyZ3VtZW50cy5sZW5ndGgsXG4gICAgICAgICAgICBhcmdzID0gQXJyYXkoX2xlbiA+IDEgPyBfbGVuIC0gMSA6IDApLFxuICAgICAgICAgICAgX2tleSA9IDE7XG4gICAgICAgICAgX2tleSA8IF9sZW47XG4gICAgICAgICAgX2tleSsrXG4gICAgICAgICkge1xuICAgICAgICAgIGFyZ3NbX2tleSAtIDFdID0gYXJndW1lbnRzW19rZXldO1xuICAgICAgICB9XG5cbiAgICAgICAgLy8gVXNlciBpcyB0cnlpbmcgdG8gYmluZCgpIGFuIGF1dG9ib3VuZCBtZXRob2Q7IHdlIGVmZmVjdGl2ZWx5IHdpbGxcbiAgICAgICAgLy8gaWdub3JlIHRoZSB2YWx1ZSBvZiBcInRoaXNcIiB0aGF0IHRoZSB1c2VyIGlzIHRyeWluZyB0byB1c2UsIHNvXG4gICAgICAgIC8vIGxldCdzIHdhcm4uXG4gICAgICAgIGlmIChuZXdUaGlzICE9PSBjb21wb25lbnQgJiYgbmV3VGhpcyAhPT0gbnVsbCkge1xuICAgICAgICAgIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgICAgICAgICB3YXJuaW5nKFxuICAgICAgICAgICAgICBmYWxzZSxcbiAgICAgICAgICAgICAgJ2JpbmQoKTogUmVhY3QgY29tcG9uZW50IG1ldGhvZHMgbWF5IG9ubHkgYmUgYm91bmQgdG8gdGhlICcgK1xuICAgICAgICAgICAgICAgICdjb21wb25lbnQgaW5zdGFuY2UuIFNlZSAlcycsXG4gICAgICAgICAgICAgIGNvbXBvbmVudE5hbWVcbiAgICAgICAgICAgICk7XG4gICAgICAgICAgfVxuICAgICAgICB9IGVsc2UgaWYgKCFhcmdzLmxlbmd0aCkge1xuICAgICAgICAgIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgICAgICAgICB3YXJuaW5nKFxuICAgICAgICAgICAgICBmYWxzZSxcbiAgICAgICAgICAgICAgJ2JpbmQoKTogWW91IGFyZSBiaW5kaW5nIGEgY29tcG9uZW50IG1ldGhvZCB0byB0aGUgY29tcG9uZW50LiAnICtcbiAgICAgICAgICAgICAgICAnUmVhY3QgZG9lcyB0aGlzIGZvciB5b3UgYXV0b21hdGljYWxseSBpbiBhIGhpZ2gtcGVyZm9ybWFuY2UgJyArXG4gICAgICAgICAgICAgICAgJ3dheSwgc28geW91IGNhbiBzYWZlbHkgcmVtb3ZlIHRoaXMgY2FsbC4gU2VlICVzJyxcbiAgICAgICAgICAgICAgY29tcG9uZW50TmFtZVxuICAgICAgICAgICAgKTtcbiAgICAgICAgICB9XG4gICAgICAgICAgcmV0dXJuIGJvdW5kTWV0aG9kO1xuICAgICAgICB9XG4gICAgICAgIHZhciByZWJvdW5kTWV0aG9kID0gX2JpbmQuYXBwbHkoYm91bmRNZXRob2QsIGFyZ3VtZW50cyk7XG4gICAgICAgIHJlYm91bmRNZXRob2QuX19yZWFjdEJvdW5kQ29udGV4dCA9IGNvbXBvbmVudDtcbiAgICAgICAgcmVib3VuZE1ldGhvZC5fX3JlYWN0Qm91bmRNZXRob2QgPSBtZXRob2Q7XG4gICAgICAgIHJlYm91bmRNZXRob2QuX19yZWFjdEJvdW5kQXJndW1lbnRzID0gYXJncztcbiAgICAgICAgcmV0dXJuIHJlYm91bmRNZXRob2Q7XG4gICAgICB9O1xuICAgIH1cbiAgICByZXR1cm4gYm91bmRNZXRob2Q7XG4gIH1cblxuICAvKipcbiAgICogQmluZHMgYWxsIGF1dG8tYm91bmQgbWV0aG9kcyBpbiBhIGNvbXBvbmVudC5cbiAgICpcbiAgICogQHBhcmFtIHtvYmplY3R9IGNvbXBvbmVudCBDb21wb25lbnQgd2hvc2UgbWV0aG9kIGlzIGdvaW5nIHRvIGJlIGJvdW5kLlxuICAgKi9cbiAgZnVuY3Rpb24gYmluZEF1dG9CaW5kTWV0aG9kcyhjb21wb25lbnQpIHtcbiAgICB2YXIgcGFpcnMgPSBjb21wb25lbnQuX19yZWFjdEF1dG9CaW5kUGFpcnM7XG4gICAgZm9yICh2YXIgaSA9IDA7IGkgPCBwYWlycy5sZW5ndGg7IGkgKz0gMikge1xuICAgICAgdmFyIGF1dG9CaW5kS2V5ID0gcGFpcnNbaV07XG4gICAgICB2YXIgbWV0aG9kID0gcGFpcnNbaSArIDFdO1xuICAgICAgY29tcG9uZW50W2F1dG9CaW5kS2V5XSA9IGJpbmRBdXRvQmluZE1ldGhvZChjb21wb25lbnQsIG1ldGhvZCk7XG4gICAgfVxuICB9XG5cbiAgdmFyIElzTW91bnRlZFByZU1peGluID0ge1xuICAgIGNvbXBvbmVudERpZE1vdW50OiBmdW5jdGlvbigpIHtcbiAgICAgIHRoaXMuX19pc01vdW50ZWQgPSB0cnVlO1xuICAgIH1cbiAgfTtcblxuICB2YXIgSXNNb3VudGVkUG9zdE1peGluID0ge1xuICAgIGNvbXBvbmVudFdpbGxVbm1vdW50OiBmdW5jdGlvbigpIHtcbiAgICAgIHRoaXMuX19pc01vdW50ZWQgPSBmYWxzZTtcbiAgICB9XG4gIH07XG5cbiAgLyoqXG4gICAqIEFkZCBtb3JlIHRvIHRoZSBSZWFjdENsYXNzIGJhc2UgY2xhc3MuIFRoZXNlIGFyZSBhbGwgbGVnYWN5IGZlYXR1cmVzIGFuZFxuICAgKiB0aGVyZWZvcmUgbm90IGFscmVhZHkgcGFydCBvZiB0aGUgbW9kZXJuIFJlYWN0Q29tcG9uZW50LlxuICAgKi9cbiAgdmFyIFJlYWN0Q2xhc3NNaXhpbiA9IHtcbiAgICAvKipcbiAgICAgKiBUT0RPOiBUaGlzIHdpbGwgYmUgZGVwcmVjYXRlZCBiZWNhdXNlIHN0YXRlIHNob3VsZCBhbHdheXMga2VlcCBhIGNvbnNpc3RlbnRcbiAgICAgKiB0eXBlIHNpZ25hdHVyZSBhbmQgdGhlIG9ubHkgdXNlIGNhc2UgZm9yIHRoaXMsIGlzIHRvIGF2b2lkIHRoYXQuXG4gICAgICovXG4gICAgcmVwbGFjZVN0YXRlOiBmdW5jdGlvbihuZXdTdGF0ZSwgY2FsbGJhY2spIHtcbiAgICAgIHRoaXMudXBkYXRlci5lbnF1ZXVlUmVwbGFjZVN0YXRlKHRoaXMsIG5ld1N0YXRlLCBjYWxsYmFjayk7XG4gICAgfSxcblxuICAgIC8qKlxuICAgICAqIENoZWNrcyB3aGV0aGVyIG9yIG5vdCB0aGlzIGNvbXBvc2l0ZSBjb21wb25lbnQgaXMgbW91bnRlZC5cbiAgICAgKiBAcmV0dXJuIHtib29sZWFufSBUcnVlIGlmIG1vdW50ZWQsIGZhbHNlIG90aGVyd2lzZS5cbiAgICAgKiBAcHJvdGVjdGVkXG4gICAgICogQGZpbmFsXG4gICAgICovXG4gICAgaXNNb3VudGVkOiBmdW5jdGlvbigpIHtcbiAgICAgIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgICAgIHdhcm5pbmcoXG4gICAgICAgICAgdGhpcy5fX2RpZFdhcm5Jc01vdW50ZWQsXG4gICAgICAgICAgJyVzOiBpc01vdW50ZWQgaXMgZGVwcmVjYXRlZC4gSW5zdGVhZCwgbWFrZSBzdXJlIHRvIGNsZWFuIHVwICcgK1xuICAgICAgICAgICAgJ3N1YnNjcmlwdGlvbnMgYW5kIHBlbmRpbmcgcmVxdWVzdHMgaW4gY29tcG9uZW50V2lsbFVubW91bnQgdG8gJyArXG4gICAgICAgICAgICAncHJldmVudCBtZW1vcnkgbGVha3MuJyxcbiAgICAgICAgICAodGhpcy5jb25zdHJ1Y3RvciAmJiB0aGlzLmNvbnN0cnVjdG9yLmRpc3BsYXlOYW1lKSB8fFxuICAgICAgICAgICAgdGhpcy5uYW1lIHx8XG4gICAgICAgICAgICAnQ29tcG9uZW50J1xuICAgICAgICApO1xuICAgICAgICB0aGlzLl9fZGlkV2FybklzTW91bnRlZCA9IHRydWU7XG4gICAgICB9XG4gICAgICByZXR1cm4gISF0aGlzLl9faXNNb3VudGVkO1xuICAgIH1cbiAgfTtcblxuICB2YXIgUmVhY3RDbGFzc0NvbXBvbmVudCA9IGZ1bmN0aW9uKCkge307XG4gIF9hc3NpZ24oXG4gICAgUmVhY3RDbGFzc0NvbXBvbmVudC5wcm90b3R5cGUsXG4gICAgUmVhY3RDb21wb25lbnQucHJvdG90eXBlLFxuICAgIFJlYWN0Q2xhc3NNaXhpblxuICApO1xuXG4gIC8qKlxuICAgKiBDcmVhdGVzIGEgY29tcG9zaXRlIGNvbXBvbmVudCBjbGFzcyBnaXZlbiBhIGNsYXNzIHNwZWNpZmljYXRpb24uXG4gICAqIFNlZSBodHRwczovL2ZhY2Vib29rLmdpdGh1Yi5pby9yZWFjdC9kb2NzL3RvcC1sZXZlbC1hcGkuaHRtbCNyZWFjdC5jcmVhdGVjbGFzc1xuICAgKlxuICAgKiBAcGFyYW0ge29iamVjdH0gc3BlYyBDbGFzcyBzcGVjaWZpY2F0aW9uICh3aGljaCBtdXN0IGRlZmluZSBgcmVuZGVyYCkuXG4gICAqIEByZXR1cm4ge2Z1bmN0aW9ufSBDb21wb25lbnQgY29uc3RydWN0b3IgZnVuY3Rpb24uXG4gICAqIEBwdWJsaWNcbiAgICovXG4gIGZ1bmN0aW9uIGNyZWF0ZUNsYXNzKHNwZWMpIHtcbiAgICAvLyBUbyBrZWVwIG91ciB3YXJuaW5ncyBtb3JlIHVuZGVyc3RhbmRhYmxlLCB3ZSdsbCB1c2UgYSBsaXR0bGUgaGFjayBoZXJlIHRvXG4gICAgLy8gZW5zdXJlIHRoYXQgQ29uc3RydWN0b3IubmFtZSAhPT0gJ0NvbnN0cnVjdG9yJy4gVGhpcyBtYWtlcyBzdXJlIHdlIGRvbid0XG4gICAgLy8gdW5uZWNlc3NhcmlseSBpZGVudGlmeSBhIGNsYXNzIHdpdGhvdXQgZGlzcGxheU5hbWUgYXMgJ0NvbnN0cnVjdG9yJy5cbiAgICB2YXIgQ29uc3RydWN0b3IgPSBpZGVudGl0eShmdW5jdGlvbihwcm9wcywgY29udGV4dCwgdXBkYXRlcikge1xuICAgICAgLy8gVGhpcyBjb25zdHJ1Y3RvciBnZXRzIG92ZXJyaWRkZW4gYnkgbW9ja3MuIFRoZSBhcmd1bWVudCBpcyB1c2VkXG4gICAgICAvLyBieSBtb2NrcyB0byBhc3NlcnQgb24gd2hhdCBnZXRzIG1vdW50ZWQuXG5cbiAgICAgIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgICAgIHdhcm5pbmcoXG4gICAgICAgICAgdGhpcyBpbnN0YW5jZW9mIENvbnN0cnVjdG9yLFxuICAgICAgICAgICdTb21ldGhpbmcgaXMgY2FsbGluZyBhIFJlYWN0IGNvbXBvbmVudCBkaXJlY3RseS4gVXNlIGEgZmFjdG9yeSBvciAnICtcbiAgICAgICAgICAgICdKU1ggaW5zdGVhZC4gU2VlOiBodHRwczovL2ZiLm1lL3JlYWN0LWxlZ2FjeWZhY3RvcnknXG4gICAgICAgICk7XG4gICAgICB9XG5cbiAgICAgIC8vIFdpcmUgdXAgYXV0by1iaW5kaW5nXG4gICAgICBpZiAodGhpcy5fX3JlYWN0QXV0b0JpbmRQYWlycy5sZW5ndGgpIHtcbiAgICAgICAgYmluZEF1dG9CaW5kTWV0aG9kcyh0aGlzKTtcbiAgICAgIH1cblxuICAgICAgdGhpcy5wcm9wcyA9IHByb3BzO1xuICAgICAgdGhpcy5jb250ZXh0ID0gY29udGV4dDtcbiAgICAgIHRoaXMucmVmcyA9IGVtcHR5T2JqZWN0O1xuICAgICAgdGhpcy51cGRhdGVyID0gdXBkYXRlciB8fCBSZWFjdE5vb3BVcGRhdGVRdWV1ZTtcblxuICAgICAgdGhpcy5zdGF0ZSA9IG51bGw7XG5cbiAgICAgIC8vIFJlYWN0Q2xhc3NlcyBkb2Vzbid0IGhhdmUgY29uc3RydWN0b3JzLiBJbnN0ZWFkLCB0aGV5IHVzZSB0aGVcbiAgICAgIC8vIGdldEluaXRpYWxTdGF0ZSBhbmQgY29tcG9uZW50V2lsbE1vdW50IG1ldGhvZHMgZm9yIGluaXRpYWxpemF0aW9uLlxuXG4gICAgICB2YXIgaW5pdGlhbFN0YXRlID0gdGhpcy5nZXRJbml0aWFsU3RhdGUgPyB0aGlzLmdldEluaXRpYWxTdGF0ZSgpIDogbnVsbDtcbiAgICAgIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgICAgIC8vIFdlIGFsbG93IGF1dG8tbW9ja3MgdG8gcHJvY2VlZCBhcyBpZiB0aGV5J3JlIHJldHVybmluZyBudWxsLlxuICAgICAgICBpZiAoXG4gICAgICAgICAgaW5pdGlhbFN0YXRlID09PSB1bmRlZmluZWQgJiZcbiAgICAgICAgICB0aGlzLmdldEluaXRpYWxTdGF0ZS5faXNNb2NrRnVuY3Rpb25cbiAgICAgICAgKSB7XG4gICAgICAgICAgLy8gVGhpcyBpcyBwcm9iYWJseSBiYWQgcHJhY3RpY2UuIENvbnNpZGVyIHdhcm5pbmcgaGVyZSBhbmRcbiAgICAgICAgICAvLyBkZXByZWNhdGluZyB0aGlzIGNvbnZlbmllbmNlLlxuICAgICAgICAgIGluaXRpYWxTdGF0ZSA9IG51bGw7XG4gICAgICAgIH1cbiAgICAgIH1cbiAgICAgIF9pbnZhcmlhbnQoXG4gICAgICAgIHR5cGVvZiBpbml0aWFsU3RhdGUgPT09ICdvYmplY3QnICYmICFBcnJheS5pc0FycmF5KGluaXRpYWxTdGF0ZSksXG4gICAgICAgICclcy5nZXRJbml0aWFsU3RhdGUoKTogbXVzdCByZXR1cm4gYW4gb2JqZWN0IG9yIG51bGwnLFxuICAgICAgICBDb25zdHJ1Y3Rvci5kaXNwbGF5TmFtZSB8fCAnUmVhY3RDb21wb3NpdGVDb21wb25lbnQnXG4gICAgICApO1xuXG4gICAgICB0aGlzLnN0YXRlID0gaW5pdGlhbFN0YXRlO1xuICAgIH0pO1xuICAgIENvbnN0cnVjdG9yLnByb3RvdHlwZSA9IG5ldyBSZWFjdENsYXNzQ29tcG9uZW50KCk7XG4gICAgQ29uc3RydWN0b3IucHJvdG90eXBlLmNvbnN0cnVjdG9yID0gQ29uc3RydWN0b3I7XG4gICAgQ29uc3RydWN0b3IucHJvdG90eXBlLl9fcmVhY3RBdXRvQmluZFBhaXJzID0gW107XG5cbiAgICBpbmplY3RlZE1peGlucy5mb3JFYWNoKG1peFNwZWNJbnRvQ29tcG9uZW50LmJpbmQobnVsbCwgQ29uc3RydWN0b3IpKTtcblxuICAgIG1peFNwZWNJbnRvQ29tcG9uZW50KENvbnN0cnVjdG9yLCBJc01vdW50ZWRQcmVNaXhpbik7XG4gICAgbWl4U3BlY0ludG9Db21wb25lbnQoQ29uc3RydWN0b3IsIHNwZWMpO1xuICAgIG1peFNwZWNJbnRvQ29tcG9uZW50KENvbnN0cnVjdG9yLCBJc01vdW50ZWRQb3N0TWl4aW4pO1xuXG4gICAgLy8gSW5pdGlhbGl6ZSB0aGUgZGVmYXVsdFByb3BzIHByb3BlcnR5IGFmdGVyIGFsbCBtaXhpbnMgaGF2ZSBiZWVuIG1lcmdlZC5cbiAgICBpZiAoQ29uc3RydWN0b3IuZ2V0RGVmYXVsdFByb3BzKSB7XG4gICAgICBDb25zdHJ1Y3Rvci5kZWZhdWx0UHJvcHMgPSBDb25zdHJ1Y3Rvci5nZXREZWZhdWx0UHJvcHMoKTtcbiAgICB9XG5cbiAgICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgICAgLy8gVGhpcyBpcyBhIHRhZyB0byBpbmRpY2F0ZSB0aGF0IHRoZSB1c2Ugb2YgdGhlc2UgbWV0aG9kIG5hbWVzIGlzIG9rLFxuICAgICAgLy8gc2luY2UgaXQncyB1c2VkIHdpdGggY3JlYXRlQ2xhc3MuIElmIGl0J3Mgbm90LCB0aGVuIGl0J3MgbGlrZWx5IGFcbiAgICAgIC8vIG1pc3Rha2Ugc28gd2UnbGwgd2FybiB5b3UgdG8gdXNlIHRoZSBzdGF0aWMgcHJvcGVydHksIHByb3BlcnR5XG4gICAgICAvLyBpbml0aWFsaXplciBvciBjb25zdHJ1Y3RvciByZXNwZWN0aXZlbHkuXG4gICAgICBpZiAoQ29uc3RydWN0b3IuZ2V0RGVmYXVsdFByb3BzKSB7XG4gICAgICAgIENvbnN0cnVjdG9yLmdldERlZmF1bHRQcm9wcy5pc1JlYWN0Q2xhc3NBcHByb3ZlZCA9IHt9O1xuICAgICAgfVxuICAgICAgaWYgKENvbnN0cnVjdG9yLnByb3RvdHlwZS5nZXRJbml0aWFsU3RhdGUpIHtcbiAgICAgICAgQ29uc3RydWN0b3IucHJvdG90eXBlLmdldEluaXRpYWxTdGF0ZS5pc1JlYWN0Q2xhc3NBcHByb3ZlZCA9IHt9O1xuICAgICAgfVxuICAgIH1cblxuICAgIF9pbnZhcmlhbnQoXG4gICAgICBDb25zdHJ1Y3Rvci5wcm90b3R5cGUucmVuZGVyLFxuICAgICAgJ2NyZWF0ZUNsYXNzKC4uLik6IENsYXNzIHNwZWNpZmljYXRpb24gbXVzdCBpbXBsZW1lbnQgYSBgcmVuZGVyYCBtZXRob2QuJ1xuICAgICk7XG5cbiAgICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgICAgd2FybmluZyhcbiAgICAgICAgIUNvbnN0cnVjdG9yLnByb3RvdHlwZS5jb21wb25lbnRTaG91bGRVcGRhdGUsXG4gICAgICAgICclcyBoYXMgYSBtZXRob2QgY2FsbGVkICcgK1xuICAgICAgICAgICdjb21wb25lbnRTaG91bGRVcGRhdGUoKS4gRGlkIHlvdSBtZWFuIHNob3VsZENvbXBvbmVudFVwZGF0ZSgpPyAnICtcbiAgICAgICAgICAnVGhlIG5hbWUgaXMgcGhyYXNlZCBhcyBhIHF1ZXN0aW9uIGJlY2F1c2UgdGhlIGZ1bmN0aW9uIGlzICcgK1xuICAgICAgICAgICdleHBlY3RlZCB0byByZXR1cm4gYSB2YWx1ZS4nLFxuICAgICAgICBzcGVjLmRpc3BsYXlOYW1lIHx8ICdBIGNvbXBvbmVudCdcbiAgICAgICk7XG4gICAgICB3YXJuaW5nKFxuICAgICAgICAhQ29uc3RydWN0b3IucHJvdG90eXBlLmNvbXBvbmVudFdpbGxSZWNpZXZlUHJvcHMsXG4gICAgICAgICclcyBoYXMgYSBtZXRob2QgY2FsbGVkICcgK1xuICAgICAgICAgICdjb21wb25lbnRXaWxsUmVjaWV2ZVByb3BzKCkuIERpZCB5b3UgbWVhbiBjb21wb25lbnRXaWxsUmVjZWl2ZVByb3BzKCk/JyxcbiAgICAgICAgc3BlYy5kaXNwbGF5TmFtZSB8fCAnQSBjb21wb25lbnQnXG4gICAgICApO1xuICAgICAgd2FybmluZyhcbiAgICAgICAgIUNvbnN0cnVjdG9yLnByb3RvdHlwZS5VTlNBRkVfY29tcG9uZW50V2lsbFJlY2lldmVQcm9wcyxcbiAgICAgICAgJyVzIGhhcyBhIG1ldGhvZCBjYWxsZWQgVU5TQUZFX2NvbXBvbmVudFdpbGxSZWNpZXZlUHJvcHMoKS4gJyArXG4gICAgICAgICAgJ0RpZCB5b3UgbWVhbiBVTlNBRkVfY29tcG9uZW50V2lsbFJlY2VpdmVQcm9wcygpPycsXG4gICAgICAgIHNwZWMuZGlzcGxheU5hbWUgfHwgJ0EgY29tcG9uZW50J1xuICAgICAgKTtcbiAgICB9XG5cbiAgICAvLyBSZWR1Y2UgdGltZSBzcGVudCBkb2luZyBsb29rdXBzIGJ5IHNldHRpbmcgdGhlc2Ugb24gdGhlIHByb3RvdHlwZS5cbiAgICBmb3IgKHZhciBtZXRob2ROYW1lIGluIFJlYWN0Q2xhc3NJbnRlcmZhY2UpIHtcbiAgICAgIGlmICghQ29uc3RydWN0b3IucHJvdG90eXBlW21ldGhvZE5hbWVdKSB7XG4gICAgICAgIENvbnN0cnVjdG9yLnByb3RvdHlwZVttZXRob2ROYW1lXSA9IG51bGw7XG4gICAgICB9XG4gICAgfVxuXG4gICAgcmV0dXJuIENvbnN0cnVjdG9yO1xuICB9XG5cbiAgcmV0dXJuIGNyZWF0ZUNsYXNzO1xufVxuXG5tb2R1bGUuZXhwb3J0cyA9IGZhY3Rvcnk7XG4iLCIvKipcbiAqIENvcHlyaWdodCAoYykgMjAxMy1wcmVzZW50LCBGYWNlYm9vaywgSW5jLlxuICpcbiAqIFRoaXMgc291cmNlIGNvZGUgaXMgbGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlIGZvdW5kIGluIHRoZVxuICogTElDRU5TRSBmaWxlIGluIHRoZSByb290IGRpcmVjdG9yeSBvZiB0aGlzIHNvdXJjZSB0cmVlLlxuICpcbiAqL1xuXG4ndXNlIHN0cmljdCc7XG5cbnZhciBSZWFjdCA9IHJlcXVpcmUoJ3JlYWN0Jyk7XG52YXIgZmFjdG9yeSA9IHJlcXVpcmUoJy4vZmFjdG9yeScpO1xuXG5pZiAodHlwZW9mIFJlYWN0ID09PSAndW5kZWZpbmVkJykge1xuICB0aHJvdyBFcnJvcihcbiAgICAnY3JlYXRlLXJlYWN0LWNsYXNzIGNvdWxkIG5vdCBmaW5kIHRoZSBSZWFjdCBvYmplY3QuIElmIHlvdSBhcmUgdXNpbmcgc2NyaXB0IHRhZ3MsICcgK1xuICAgICAgJ21ha2Ugc3VyZSB0aGF0IFJlYWN0IGlzIGJlaW5nIGxvYWRlZCBiZWZvcmUgY3JlYXRlLXJlYWN0LWNsYXNzLidcbiAgKTtcbn1cblxuLy8gSGFjayB0byBncmFiIE5vb3BVcGRhdGVRdWV1ZSBmcm9tIGlzb21vcnBoaWMgUmVhY3RcbnZhciBSZWFjdE5vb3BVcGRhdGVRdWV1ZSA9IG5ldyBSZWFjdC5Db21wb25lbnQoKS51cGRhdGVyO1xuXG5tb2R1bGUuZXhwb3J0cyA9IGZhY3RvcnkoXG4gIFJlYWN0LkNvbXBvbmVudCxcbiAgUmVhY3QuaXNWYWxpZEVsZW1lbnQsXG4gIFJlYWN0Tm9vcFVwZGF0ZVF1ZXVlXG4pO1xuIiwiZXhwb3J0cyA9IG1vZHVsZS5leHBvcnRzID0gcmVxdWlyZShcIi4uLy4uL2Nzcy1sb2FkZXIvZGlzdC9ydW50aW1lL2FwaS5qc1wiKShmYWxzZSk7XG4vLyBNb2R1bGVcbmV4cG9ydHMucHVzaChbbW9kdWxlLmlkLCBcIi8qIVxcbiAqIGh0dHBzOi8vZ2l0aHViLmNvbS9Zb3VDYW5Cb29rTWUvcmVhY3QtZGF0ZXRpbWVcXG4gKi9cXG5cXG4ucmR0IHtcXG4gIHBvc2l0aW9uOiByZWxhdGl2ZTtcXG59XFxuLnJkdFBpY2tlciB7XFxuICBkaXNwbGF5OiBub25lO1xcbiAgcG9zaXRpb246IGFic29sdXRlO1xcbiAgd2lkdGg6IDI1MHB4O1xcbiAgcGFkZGluZzogNHB4O1xcbiAgbWFyZ2luLXRvcDogMXB4O1xcbiAgei1pbmRleDogOTk5OTkgIWltcG9ydGFudDtcXG4gIGJhY2tncm91bmQ6ICNmZmY7XFxuICBib3gtc2hhZG93OiAwIDFweCAzcHggcmdiYSgwLDAsMCwuMSk7XFxuICBib3JkZXI6IDFweCBzb2xpZCAjZjlmOWY5O1xcbn1cXG4ucmR0T3BlbiAucmR0UGlja2VyIHtcXG4gIGRpc3BsYXk6IGJsb2NrO1xcbn1cXG4ucmR0U3RhdGljIC5yZHRQaWNrZXIge1xcbiAgYm94LXNoYWRvdzogbm9uZTtcXG4gIHBvc2l0aW9uOiBzdGF0aWM7XFxufVxcblxcbi5yZHRQaWNrZXIgLnJkdFRpbWVUb2dnbGUge1xcbiAgdGV4dC1hbGlnbjogY2VudGVyO1xcbn1cXG5cXG4ucmR0UGlja2VyIHRhYmxlIHtcXG4gIHdpZHRoOiAxMDAlO1xcbiAgbWFyZ2luOiAwO1xcbn1cXG4ucmR0UGlja2VyIHRkLFxcbi5yZHRQaWNrZXIgdGgge1xcbiAgdGV4dC1hbGlnbjogY2VudGVyO1xcbiAgaGVpZ2h0OiAyOHB4O1xcbn1cXG4ucmR0UGlja2VyIHRkIHtcXG4gIGN1cnNvcjogcG9pbnRlcjtcXG59XFxuLnJkdFBpY2tlciB0ZC5yZHREYXk6aG92ZXIsXFxuLnJkdFBpY2tlciB0ZC5yZHRIb3VyOmhvdmVyLFxcbi5yZHRQaWNrZXIgdGQucmR0TWludXRlOmhvdmVyLFxcbi5yZHRQaWNrZXIgdGQucmR0U2Vjb25kOmhvdmVyLFxcbi5yZHRQaWNrZXIgLnJkdFRpbWVUb2dnbGU6aG92ZXIge1xcbiAgYmFja2dyb3VuZDogI2VlZWVlZTtcXG4gIGN1cnNvcjogcG9pbnRlcjtcXG59XFxuLnJkdFBpY2tlciB0ZC5yZHRPbGQsXFxuLnJkdFBpY2tlciB0ZC5yZHROZXcge1xcbiAgY29sb3I6ICM5OTk5OTk7XFxufVxcbi5yZHRQaWNrZXIgdGQucmR0VG9kYXkge1xcbiAgcG9zaXRpb246IHJlbGF0aXZlO1xcbn1cXG4ucmR0UGlja2VyIHRkLnJkdFRvZGF5OmJlZm9yZSB7XFxuICBjb250ZW50OiAnJztcXG4gIGRpc3BsYXk6IGlubGluZS1ibG9jaztcXG4gIGJvcmRlci1sZWZ0OiA3cHggc29saWQgdHJhbnNwYXJlbnQ7XFxuICBib3JkZXItYm90dG9tOiA3cHggc29saWQgIzQyOGJjYTtcXG4gIGJvcmRlci10b3AtY29sb3I6IHJnYmEoMCwgMCwgMCwgMC4yKTtcXG4gIHBvc2l0aW9uOiBhYnNvbHV0ZTtcXG4gIGJvdHRvbTogNHB4O1xcbiAgcmlnaHQ6IDRweDtcXG59XFxuLnJkdFBpY2tlciB0ZC5yZHRBY3RpdmUsXFxuLnJkdFBpY2tlciB0ZC5yZHRBY3RpdmU6aG92ZXIge1xcbiAgYmFja2dyb3VuZC1jb2xvcjogIzQyOGJjYTtcXG4gIGNvbG9yOiAjZmZmO1xcbiAgdGV4dC1zaGFkb3c6IDAgLTFweCAwIHJnYmEoMCwgMCwgMCwgMC4yNSk7XFxufVxcbi5yZHRQaWNrZXIgdGQucmR0QWN0aXZlLnJkdFRvZGF5OmJlZm9yZSB7XFxuICBib3JkZXItYm90dG9tLWNvbG9yOiAjZmZmO1xcbn1cXG4ucmR0UGlja2VyIHRkLnJkdERpc2FibGVkLFxcbi5yZHRQaWNrZXIgdGQucmR0RGlzYWJsZWQ6aG92ZXIge1xcbiAgYmFja2dyb3VuZDogbm9uZTtcXG4gIGNvbG9yOiAjOTk5OTk5O1xcbiAgY3Vyc29yOiBub3QtYWxsb3dlZDtcXG59XFxuXFxuLnJkdFBpY2tlciB0ZCBzcGFuLnJkdE9sZCB7XFxuICBjb2xvcjogIzk5OTk5OTtcXG59XFxuLnJkdFBpY2tlciB0ZCBzcGFuLnJkdERpc2FibGVkLFxcbi5yZHRQaWNrZXIgdGQgc3Bhbi5yZHREaXNhYmxlZDpob3ZlciB7XFxuICBiYWNrZ3JvdW5kOiBub25lO1xcbiAgY29sb3I6ICM5OTk5OTk7XFxuICBjdXJzb3I6IG5vdC1hbGxvd2VkO1xcbn1cXG4ucmR0UGlja2VyIHRoIHtcXG4gIGJvcmRlci1ib3R0b206IDFweCBzb2xpZCAjZjlmOWY5O1xcbn1cXG4ucmR0UGlja2VyIC5kb3cge1xcbiAgd2lkdGg6IDE0LjI4NTclO1xcbiAgYm9yZGVyLWJvdHRvbTogbm9uZTtcXG4gIGN1cnNvcjogZGVmYXVsdDtcXG59XFxuLnJkdFBpY2tlciB0aC5yZHRTd2l0Y2gge1xcbiAgd2lkdGg6IDEwMHB4O1xcbn1cXG4ucmR0UGlja2VyIHRoLnJkdE5leHQsXFxuLnJkdFBpY2tlciB0aC5yZHRQcmV2IHtcXG4gIGZvbnQtc2l6ZTogMjFweDtcXG4gIHZlcnRpY2FsLWFsaWduOiB0b3A7XFxufVxcblxcbi5yZHRQcmV2IHNwYW4sXFxuLnJkdE5leHQgc3BhbiB7XFxuICBkaXNwbGF5OiBibG9jaztcXG4gIC13ZWJraXQtdG91Y2gtY2FsbG91dDogbm9uZTsgLyogaU9TIFNhZmFyaSAqL1xcbiAgLXdlYmtpdC11c2VyLXNlbGVjdDogbm9uZTsgICAvKiBDaHJvbWUvU2FmYXJpL09wZXJhICovXFxuICAta2h0bWwtdXNlci1zZWxlY3Q6IG5vbmU7ICAgIC8qIEtvbnF1ZXJvciAqL1xcbiAgLW1vei11c2VyLXNlbGVjdDogbm9uZTsgICAgICAvKiBGaXJlZm94ICovXFxuICAtbXMtdXNlci1zZWxlY3Q6IG5vbmU7ICAgICAgIC8qIEludGVybmV0IEV4cGxvcmVyL0VkZ2UgKi9cXG4gIHVzZXItc2VsZWN0OiBub25lO1xcbn1cXG5cXG4ucmR0UGlja2VyIHRoLnJkdERpc2FibGVkLFxcbi5yZHRQaWNrZXIgdGgucmR0RGlzYWJsZWQ6aG92ZXIge1xcbiAgYmFja2dyb3VuZDogbm9uZTtcXG4gIGNvbG9yOiAjOTk5OTk5O1xcbiAgY3Vyc29yOiBub3QtYWxsb3dlZDtcXG59XFxuLnJkdFBpY2tlciB0aGVhZCB0cjpmaXJzdC1jaGlsZCB0aCB7XFxuICBjdXJzb3I6IHBvaW50ZXI7XFxufVxcbi5yZHRQaWNrZXIgdGhlYWQgdHI6Zmlyc3QtY2hpbGQgdGg6aG92ZXIge1xcbiAgYmFja2dyb3VuZDogI2VlZWVlZTtcXG59XFxuXFxuLnJkdFBpY2tlciB0Zm9vdCB7XFxuICBib3JkZXItdG9wOiAxcHggc29saWQgI2Y5ZjlmOTtcXG59XFxuXFxuLnJkdFBpY2tlciBidXR0b24ge1xcbiAgYm9yZGVyOiBub25lO1xcbiAgYmFja2dyb3VuZDogbm9uZTtcXG4gIGN1cnNvcjogcG9pbnRlcjtcXG59XFxuLnJkdFBpY2tlciBidXR0b246aG92ZXIge1xcbiAgYmFja2dyb3VuZC1jb2xvcjogI2VlZTtcXG59XFxuXFxuLnJkdFBpY2tlciB0aGVhZCBidXR0b24ge1xcbiAgd2lkdGg6IDEwMCU7XFxuICBoZWlnaHQ6IDEwMCU7XFxufVxcblxcbnRkLnJkdE1vbnRoLFxcbnRkLnJkdFllYXIge1xcbiAgaGVpZ2h0OiA1MHB4O1xcbiAgd2lkdGg6IDI1JTtcXG4gIGN1cnNvcjogcG9pbnRlcjtcXG59XFxudGQucmR0TW9udGg6aG92ZXIsXFxudGQucmR0WWVhcjpob3ZlciB7XFxuICBiYWNrZ3JvdW5kOiAjZWVlO1xcbn1cXG5cXG4ucmR0Q291bnRlcnMge1xcbiAgZGlzcGxheTogaW5saW5lLWJsb2NrO1xcbn1cXG5cXG4ucmR0Q291bnRlcnMgPiBkaXYge1xcbiAgZmxvYXQ6IGxlZnQ7XFxufVxcblxcbi5yZHRDb3VudGVyIHtcXG4gIGhlaWdodDogMTAwcHg7XFxufVxcblxcbi5yZHRDb3VudGVyIHtcXG4gIHdpZHRoOiA0MHB4O1xcbn1cXG5cXG4ucmR0Q291bnRlclNlcGFyYXRvciB7XFxuICBsaW5lLWhlaWdodDogMTAwcHg7XFxufVxcblxcbi5yZHRDb3VudGVyIC5yZHRCdG4ge1xcbiAgaGVpZ2h0OiA0MCU7XFxuICBsaW5lLWhlaWdodDogNDBweDtcXG4gIGN1cnNvcjogcG9pbnRlcjtcXG4gIGRpc3BsYXk6IGJsb2NrO1xcblxcbiAgLXdlYmtpdC10b3VjaC1jYWxsb3V0OiBub25lOyAvKiBpT1MgU2FmYXJpICovXFxuICAtd2Via2l0LXVzZXItc2VsZWN0OiBub25lOyAgIC8qIENocm9tZS9TYWZhcmkvT3BlcmEgKi9cXG4gIC1raHRtbC11c2VyLXNlbGVjdDogbm9uZTsgICAgLyogS29ucXVlcm9yICovXFxuICAtbW96LXVzZXItc2VsZWN0OiBub25lOyAgICAgIC8qIEZpcmVmb3ggKi9cXG4gIC1tcy11c2VyLXNlbGVjdDogbm9uZTsgICAgICAgLyogSW50ZXJuZXQgRXhwbG9yZXIvRWRnZSAqL1xcbiAgdXNlci1zZWxlY3Q6IG5vbmU7XFxufVxcbi5yZHRDb3VudGVyIC5yZHRCdG46aG92ZXIge1xcbiAgYmFja2dyb3VuZDogI2VlZTtcXG59XFxuLnJkdENvdW50ZXIgLnJkdENvdW50IHtcXG4gIGhlaWdodDogMjAlO1xcbiAgZm9udC1zaXplOiAxLjJlbTtcXG59XFxuXFxuLnJkdE1pbGxpIHtcXG4gIHZlcnRpY2FsLWFsaWduOiBtaWRkbGU7XFxuICBwYWRkaW5nLWxlZnQ6IDhweDtcXG4gIHdpZHRoOiA0OHB4O1xcbn1cXG5cXG4ucmR0TWlsbGkgaW5wdXQge1xcbiAgd2lkdGg6IDEwMCU7XFxuICBmb250LXNpemU6IDEuMmVtO1xcbiAgbWFyZ2luLXRvcDogMzdweDtcXG59XFxuXFxuLnJkdFRpbWUgdGQge1xcbiAgY3Vyc29yOiBkZWZhdWx0O1xcbn1cXG5cIiwgXCJcIl0pO1xuIiwiXCJ1c2Ugc3RyaWN0XCI7XG5cbi8qXG4gIE1JVCBMaWNlbnNlIGh0dHA6Ly93d3cub3BlbnNvdXJjZS5vcmcvbGljZW5zZXMvbWl0LWxpY2Vuc2UucGhwXG4gIEF1dGhvciBUb2JpYXMgS29wcGVycyBAc29rcmFcbiovXG4vLyBjc3MgYmFzZSBjb2RlLCBpbmplY3RlZCBieSB0aGUgY3NzLWxvYWRlclxuLy8gZXNsaW50LWRpc2FibGUtbmV4dC1saW5lIGZ1bmMtbmFtZXNcbm1vZHVsZS5leHBvcnRzID0gZnVuY3Rpb24gKHVzZVNvdXJjZU1hcCkge1xuICB2YXIgbGlzdCA9IFtdOyAvLyByZXR1cm4gdGhlIGxpc3Qgb2YgbW9kdWxlcyBhcyBjc3Mgc3RyaW5nXG5cbiAgbGlzdC50b1N0cmluZyA9IGZ1bmN0aW9uIHRvU3RyaW5nKCkge1xuICAgIHJldHVybiB0aGlzLm1hcChmdW5jdGlvbiAoaXRlbSkge1xuICAgICAgdmFyIGNvbnRlbnQgPSBjc3NXaXRoTWFwcGluZ1RvU3RyaW5nKGl0ZW0sIHVzZVNvdXJjZU1hcCk7XG5cbiAgICAgIGlmIChpdGVtWzJdKSB7XG4gICAgICAgIHJldHVybiBcIkBtZWRpYSBcIi5jb25jYXQoaXRlbVsyXSwgXCJ7XCIpLmNvbmNhdChjb250ZW50LCBcIn1cIik7XG4gICAgICB9XG5cbiAgICAgIHJldHVybiBjb250ZW50O1xuICAgIH0pLmpvaW4oJycpO1xuICB9OyAvLyBpbXBvcnQgYSBsaXN0IG9mIG1vZHVsZXMgaW50byB0aGUgbGlzdFxuICAvLyBlc2xpbnQtZGlzYWJsZS1uZXh0LWxpbmUgZnVuYy1uYW1lc1xuXG5cbiAgbGlzdC5pID0gZnVuY3Rpb24gKG1vZHVsZXMsIG1lZGlhUXVlcnkpIHtcbiAgICBpZiAodHlwZW9mIG1vZHVsZXMgPT09ICdzdHJpbmcnKSB7XG4gICAgICAvLyBlc2xpbnQtZGlzYWJsZS1uZXh0LWxpbmUgbm8tcGFyYW0tcmVhc3NpZ25cbiAgICAgIG1vZHVsZXMgPSBbW251bGwsIG1vZHVsZXMsICcnXV07XG4gICAgfVxuXG4gICAgdmFyIGFscmVhZHlJbXBvcnRlZE1vZHVsZXMgPSB7fTtcblxuICAgIGZvciAodmFyIGkgPSAwOyBpIDwgdGhpcy5sZW5ndGg7IGkrKykge1xuICAgICAgLy8gZXNsaW50LWRpc2FibGUtbmV4dC1saW5lIHByZWZlci1kZXN0cnVjdHVyaW5nXG4gICAgICB2YXIgaWQgPSB0aGlzW2ldWzBdO1xuXG4gICAgICBpZiAoaWQgIT0gbnVsbCkge1xuICAgICAgICBhbHJlYWR5SW1wb3J0ZWRNb2R1bGVzW2lkXSA9IHRydWU7XG4gICAgICB9XG4gICAgfVxuXG4gICAgZm9yICh2YXIgX2kgPSAwOyBfaSA8IG1vZHVsZXMubGVuZ3RoOyBfaSsrKSB7XG4gICAgICB2YXIgaXRlbSA9IG1vZHVsZXNbX2ldOyAvLyBza2lwIGFscmVhZHkgaW1wb3J0ZWQgbW9kdWxlXG4gICAgICAvLyB0aGlzIGltcGxlbWVudGF0aW9uIGlzIG5vdCAxMDAlIHBlcmZlY3QgZm9yIHdlaXJkIG1lZGlhIHF1ZXJ5IGNvbWJpbmF0aW9uc1xuICAgICAgLy8gd2hlbiBhIG1vZHVsZSBpcyBpbXBvcnRlZCBtdWx0aXBsZSB0aW1lcyB3aXRoIGRpZmZlcmVudCBtZWRpYSBxdWVyaWVzLlxuICAgICAgLy8gSSBob3BlIHRoaXMgd2lsbCBuZXZlciBvY2N1ciAoSGV5IHRoaXMgd2F5IHdlIGhhdmUgc21hbGxlciBidW5kbGVzKVxuXG4gICAgICBpZiAoaXRlbVswXSA9PSBudWxsIHx8ICFhbHJlYWR5SW1wb3J0ZWRNb2R1bGVzW2l0ZW1bMF1dKSB7XG4gICAgICAgIGlmIChtZWRpYVF1ZXJ5ICYmICFpdGVtWzJdKSB7XG4gICAgICAgICAgaXRlbVsyXSA9IG1lZGlhUXVlcnk7XG4gICAgICAgIH0gZWxzZSBpZiAobWVkaWFRdWVyeSkge1xuICAgICAgICAgIGl0ZW1bMl0gPSBcIihcIi5jb25jYXQoaXRlbVsyXSwgXCIpIGFuZCAoXCIpLmNvbmNhdChtZWRpYVF1ZXJ5LCBcIilcIik7XG4gICAgICAgIH1cblxuICAgICAgICBsaXN0LnB1c2goaXRlbSk7XG4gICAgICB9XG4gICAgfVxuICB9O1xuXG4gIHJldHVybiBsaXN0O1xufTtcblxuZnVuY3Rpb24gY3NzV2l0aE1hcHBpbmdUb1N0cmluZyhpdGVtLCB1c2VTb3VyY2VNYXApIHtcbiAgdmFyIGNvbnRlbnQgPSBpdGVtWzFdIHx8ICcnOyAvLyBlc2xpbnQtZGlzYWJsZS1uZXh0LWxpbmUgcHJlZmVyLWRlc3RydWN0dXJpbmdcblxuICB2YXIgY3NzTWFwcGluZyA9IGl0ZW1bM107XG5cbiAgaWYgKCFjc3NNYXBwaW5nKSB7XG4gICAgcmV0dXJuIGNvbnRlbnQ7XG4gIH1cblxuICBpZiAodXNlU291cmNlTWFwICYmIHR5cGVvZiBidG9hID09PSAnZnVuY3Rpb24nKSB7XG4gICAgdmFyIHNvdXJjZU1hcHBpbmcgPSB0b0NvbW1lbnQoY3NzTWFwcGluZyk7XG4gICAgdmFyIHNvdXJjZVVSTHMgPSBjc3NNYXBwaW5nLnNvdXJjZXMubWFwKGZ1bmN0aW9uIChzb3VyY2UpIHtcbiAgICAgIHJldHVybiBcIi8qIyBzb3VyY2VVUkw9XCIuY29uY2F0KGNzc01hcHBpbmcuc291cmNlUm9vdCkuY29uY2F0KHNvdXJjZSwgXCIgKi9cIik7XG4gICAgfSk7XG4gICAgcmV0dXJuIFtjb250ZW50XS5jb25jYXQoc291cmNlVVJMcykuY29uY2F0KFtzb3VyY2VNYXBwaW5nXSkuam9pbignXFxuJyk7XG4gIH1cblxuICByZXR1cm4gW2NvbnRlbnRdLmpvaW4oJ1xcbicpO1xufSAvLyBBZGFwdGVkIGZyb20gY29udmVydC1zb3VyY2UtbWFwIChNSVQpXG5cblxuZnVuY3Rpb24gdG9Db21tZW50KHNvdXJjZU1hcCkge1xuICAvLyBlc2xpbnQtZGlzYWJsZS1uZXh0LWxpbmUgbm8tdW5kZWZcbiAgdmFyIGJhc2U2NCA9IGJ0b2EodW5lc2NhcGUoZW5jb2RlVVJJQ29tcG9uZW50KEpTT04uc3RyaW5naWZ5KHNvdXJjZU1hcCkpKSk7XG4gIHZhciBkYXRhID0gXCJzb3VyY2VNYXBwaW5nVVJMPWRhdGE6YXBwbGljYXRpb24vanNvbjtjaGFyc2V0PXV0Zi04O2Jhc2U2NCxcIi5jb25jYXQoYmFzZTY0KTtcbiAgcmV0dXJuIFwiLyojIFwiLmNvbmNhdChkYXRhLCBcIiAqL1wiKTtcbn0iLCIndXNlIHN0cmljdCc7XG52YXIgdG9rZW4gPSAnJVthLWYwLTldezJ9JztcbnZhciBzaW5nbGVNYXRjaGVyID0gbmV3IFJlZ0V4cCgnKCcgKyB0b2tlbiArICcpfChbXiVdKz8pJywgJ2dpJyk7XG52YXIgbXVsdGlNYXRjaGVyID0gbmV3IFJlZ0V4cCgnKCcgKyB0b2tlbiArICcpKycsICdnaScpO1xuXG5mdW5jdGlvbiBkZWNvZGVDb21wb25lbnRzKGNvbXBvbmVudHMsIHNwbGl0KSB7XG5cdHRyeSB7XG5cdFx0Ly8gVHJ5IHRvIGRlY29kZSB0aGUgZW50aXJlIHN0cmluZyBmaXJzdFxuXHRcdHJldHVybiBbZGVjb2RlVVJJQ29tcG9uZW50KGNvbXBvbmVudHMuam9pbignJykpXTtcblx0fSBjYXRjaCAoZXJyKSB7XG5cdFx0Ly8gRG8gbm90aGluZ1xuXHR9XG5cblx0aWYgKGNvbXBvbmVudHMubGVuZ3RoID09PSAxKSB7XG5cdFx0cmV0dXJuIGNvbXBvbmVudHM7XG5cdH1cblxuXHRzcGxpdCA9IHNwbGl0IHx8IDE7XG5cblx0Ly8gU3BsaXQgdGhlIGFycmF5IGluIDIgcGFydHNcblx0dmFyIGxlZnQgPSBjb21wb25lbnRzLnNsaWNlKDAsIHNwbGl0KTtcblx0dmFyIHJpZ2h0ID0gY29tcG9uZW50cy5zbGljZShzcGxpdCk7XG5cblx0cmV0dXJuIEFycmF5LnByb3RvdHlwZS5jb25jYXQuY2FsbChbXSwgZGVjb2RlQ29tcG9uZW50cyhsZWZ0KSwgZGVjb2RlQ29tcG9uZW50cyhyaWdodCkpO1xufVxuXG5mdW5jdGlvbiBkZWNvZGUoaW5wdXQpIHtcblx0dHJ5IHtcblx0XHRyZXR1cm4gZGVjb2RlVVJJQ29tcG9uZW50KGlucHV0KTtcblx0fSBjYXRjaCAoZXJyKSB7XG5cdFx0dmFyIHRva2VucyA9IGlucHV0Lm1hdGNoKHNpbmdsZU1hdGNoZXIpIHx8IFtdO1xuXG5cdFx0Zm9yICh2YXIgaSA9IDE7IGkgPCB0b2tlbnMubGVuZ3RoOyBpKyspIHtcblx0XHRcdGlucHV0ID0gZGVjb2RlQ29tcG9uZW50cyh0b2tlbnMsIGkpLmpvaW4oJycpO1xuXG5cdFx0XHR0b2tlbnMgPSBpbnB1dC5tYXRjaChzaW5nbGVNYXRjaGVyKSB8fCBbXTtcblx0XHR9XG5cblx0XHRyZXR1cm4gaW5wdXQ7XG5cdH1cbn1cblxuZnVuY3Rpb24gY3VzdG9tRGVjb2RlVVJJQ29tcG9uZW50KGlucHV0KSB7XG5cdC8vIEtlZXAgdHJhY2sgb2YgYWxsIHRoZSByZXBsYWNlbWVudHMgYW5kIHByZWZpbGwgdGhlIG1hcCB3aXRoIHRoZSBgQk9NYFxuXHR2YXIgcmVwbGFjZU1hcCA9IHtcblx0XHQnJUZFJUZGJzogJ1xcdUZGRkRcXHVGRkZEJyxcblx0XHQnJUZGJUZFJzogJ1xcdUZGRkRcXHVGRkZEJ1xuXHR9O1xuXG5cdHZhciBtYXRjaCA9IG11bHRpTWF0Y2hlci5leGVjKGlucHV0KTtcblx0d2hpbGUgKG1hdGNoKSB7XG5cdFx0dHJ5IHtcblx0XHRcdC8vIERlY29kZSBhcyBiaWcgY2h1bmtzIGFzIHBvc3NpYmxlXG5cdFx0XHRyZXBsYWNlTWFwW21hdGNoWzBdXSA9IGRlY29kZVVSSUNvbXBvbmVudChtYXRjaFswXSk7XG5cdFx0fSBjYXRjaCAoZXJyKSB7XG5cdFx0XHR2YXIgcmVzdWx0ID0gZGVjb2RlKG1hdGNoWzBdKTtcblxuXHRcdFx0aWYgKHJlc3VsdCAhPT0gbWF0Y2hbMF0pIHtcblx0XHRcdFx0cmVwbGFjZU1hcFttYXRjaFswXV0gPSByZXN1bHQ7XG5cdFx0XHR9XG5cdFx0fVxuXG5cdFx0bWF0Y2ggPSBtdWx0aU1hdGNoZXIuZXhlYyhpbnB1dCk7XG5cdH1cblxuXHQvLyBBZGQgYCVDMmAgYXQgdGhlIGVuZCBvZiB0aGUgbWFwIHRvIG1ha2Ugc3VyZSBpdCBkb2VzIG5vdCByZXBsYWNlIHRoZSBjb21iaW5hdG9yIGJlZm9yZSBldmVyeXRoaW5nIGVsc2Vcblx0cmVwbGFjZU1hcFsnJUMyJ10gPSAnXFx1RkZGRCc7XG5cblx0dmFyIGVudHJpZXMgPSBPYmplY3Qua2V5cyhyZXBsYWNlTWFwKTtcblxuXHRmb3IgKHZhciBpID0gMDsgaSA8IGVudHJpZXMubGVuZ3RoOyBpKyspIHtcblx0XHQvLyBSZXBsYWNlIGFsbCBkZWNvZGVkIGNvbXBvbmVudHNcblx0XHR2YXIga2V5ID0gZW50cmllc1tpXTtcblx0XHRpbnB1dCA9IGlucHV0LnJlcGxhY2UobmV3IFJlZ0V4cChrZXksICdnJyksIHJlcGxhY2VNYXBba2V5XSk7XG5cdH1cblxuXHRyZXR1cm4gaW5wdXQ7XG59XG5cbm1vZHVsZS5leHBvcnRzID0gZnVuY3Rpb24gKGVuY29kZWRVUkkpIHtcblx0aWYgKHR5cGVvZiBlbmNvZGVkVVJJICE9PSAnc3RyaW5nJykge1xuXHRcdHRocm93IG5ldyBUeXBlRXJyb3IoJ0V4cGVjdGVkIGBlbmNvZGVkVVJJYCB0byBiZSBvZiB0eXBlIGBzdHJpbmdgLCBnb3QgYCcgKyB0eXBlb2YgZW5jb2RlZFVSSSArICdgJyk7XG5cdH1cblxuXHR0cnkge1xuXHRcdGVuY29kZWRVUkkgPSBlbmNvZGVkVVJJLnJlcGxhY2UoL1xcKy9nLCAnICcpO1xuXG5cdFx0Ly8gVHJ5IHRoZSBidWlsdCBpbiBkZWNvZGVyIGZpcnN0XG5cdFx0cmV0dXJuIGRlY29kZVVSSUNvbXBvbmVudChlbmNvZGVkVVJJKTtcblx0fSBjYXRjaCAoZXJyKSB7XG5cdFx0Ly8gRmFsbGJhY2sgdG8gYSBtb3JlIGFkdmFuY2VkIGRlY29kZXJcblx0XHRyZXR1cm4gY3VzdG9tRGVjb2RlVVJJQ29tcG9uZW50KGVuY29kZWRVUkkpO1xuXHR9XG59O1xuIiwiXCJ1c2Ugc3RyaWN0XCI7XG5cbi8qKlxuICogQ29weXJpZ2h0IChjKSAyMDEzLXByZXNlbnQsIEZhY2Vib29rLCBJbmMuXG4gKlxuICogVGhpcyBzb3VyY2UgY29kZSBpcyBsaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UgZm91bmQgaW4gdGhlXG4gKiBMSUNFTlNFIGZpbGUgaW4gdGhlIHJvb3QgZGlyZWN0b3J5IG9mIHRoaXMgc291cmNlIHRyZWUuXG4gKlxuICogXG4gKi9cblxuZnVuY3Rpb24gbWFrZUVtcHR5RnVuY3Rpb24oYXJnKSB7XG4gIHJldHVybiBmdW5jdGlvbiAoKSB7XG4gICAgcmV0dXJuIGFyZztcbiAgfTtcbn1cblxuLyoqXG4gKiBUaGlzIGZ1bmN0aW9uIGFjY2VwdHMgYW5kIGRpc2NhcmRzIGlucHV0czsgaXQgaGFzIG5vIHNpZGUgZWZmZWN0cy4gVGhpcyBpc1xuICogcHJpbWFyaWx5IHVzZWZ1bCBpZGlvbWF0aWNhbGx5IGZvciBvdmVycmlkYWJsZSBmdW5jdGlvbiBlbmRwb2ludHMgd2hpY2hcbiAqIGFsd2F5cyBuZWVkIHRvIGJlIGNhbGxhYmxlLCBzaW5jZSBKUyBsYWNrcyBhIG51bGwtY2FsbCBpZGlvbSBhbGEgQ29jb2EuXG4gKi9cbnZhciBlbXB0eUZ1bmN0aW9uID0gZnVuY3Rpb24gZW1wdHlGdW5jdGlvbigpIHt9O1xuXG5lbXB0eUZ1bmN0aW9uLnRoYXRSZXR1cm5zID0gbWFrZUVtcHR5RnVuY3Rpb247XG5lbXB0eUZ1bmN0aW9uLnRoYXRSZXR1cm5zRmFsc2UgPSBtYWtlRW1wdHlGdW5jdGlvbihmYWxzZSk7XG5lbXB0eUZ1bmN0aW9uLnRoYXRSZXR1cm5zVHJ1ZSA9IG1ha2VFbXB0eUZ1bmN0aW9uKHRydWUpO1xuZW1wdHlGdW5jdGlvbi50aGF0UmV0dXJuc051bGwgPSBtYWtlRW1wdHlGdW5jdGlvbihudWxsKTtcbmVtcHR5RnVuY3Rpb24udGhhdFJldHVybnNUaGlzID0gZnVuY3Rpb24gKCkge1xuICByZXR1cm4gdGhpcztcbn07XG5lbXB0eUZ1bmN0aW9uLnRoYXRSZXR1cm5zQXJndW1lbnQgPSBmdW5jdGlvbiAoYXJnKSB7XG4gIHJldHVybiBhcmc7XG59O1xuXG5tb2R1bGUuZXhwb3J0cyA9IGVtcHR5RnVuY3Rpb247IiwiLyoqXG4gKiBDb3B5cmlnaHQgKGMpIDIwMTMtcHJlc2VudCwgRmFjZWJvb2ssIEluYy5cbiAqXG4gKiBUaGlzIHNvdXJjZSBjb2RlIGlzIGxpY2Vuc2VkIHVuZGVyIHRoZSBNSVQgbGljZW5zZSBmb3VuZCBpbiB0aGVcbiAqIExJQ0VOU0UgZmlsZSBpbiB0aGUgcm9vdCBkaXJlY3Rvcnkgb2YgdGhpcyBzb3VyY2UgdHJlZS5cbiAqXG4gKi9cblxuJ3VzZSBzdHJpY3QnO1xuXG4vKipcbiAqIFVzZSBpbnZhcmlhbnQoKSB0byBhc3NlcnQgc3RhdGUgd2hpY2ggeW91ciBwcm9ncmFtIGFzc3VtZXMgdG8gYmUgdHJ1ZS5cbiAqXG4gKiBQcm92aWRlIHNwcmludGYtc3R5bGUgZm9ybWF0IChvbmx5ICVzIGlzIHN1cHBvcnRlZCkgYW5kIGFyZ3VtZW50c1xuICogdG8gcHJvdmlkZSBpbmZvcm1hdGlvbiBhYm91dCB3aGF0IGJyb2tlIGFuZCB3aGF0IHlvdSB3ZXJlXG4gKiBleHBlY3RpbmcuXG4gKlxuICogVGhlIGludmFyaWFudCBtZXNzYWdlIHdpbGwgYmUgc3RyaXBwZWQgaW4gcHJvZHVjdGlvbiwgYnV0IHRoZSBpbnZhcmlhbnRcbiAqIHdpbGwgcmVtYWluIHRvIGVuc3VyZSBsb2dpYyBkb2VzIG5vdCBkaWZmZXIgaW4gcHJvZHVjdGlvbi5cbiAqL1xuXG52YXIgdmFsaWRhdGVGb3JtYXQgPSBmdW5jdGlvbiB2YWxpZGF0ZUZvcm1hdChmb3JtYXQpIHt9O1xuXG5pZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICB2YWxpZGF0ZUZvcm1hdCA9IGZ1bmN0aW9uIHZhbGlkYXRlRm9ybWF0KGZvcm1hdCkge1xuICAgIGlmIChmb3JtYXQgPT09IHVuZGVmaW5lZCkge1xuICAgICAgdGhyb3cgbmV3IEVycm9yKCdpbnZhcmlhbnQgcmVxdWlyZXMgYW4gZXJyb3IgbWVzc2FnZSBhcmd1bWVudCcpO1xuICAgIH1cbiAgfTtcbn1cblxuZnVuY3Rpb24gaW52YXJpYW50KGNvbmRpdGlvbiwgZm9ybWF0LCBhLCBiLCBjLCBkLCBlLCBmKSB7XG4gIHZhbGlkYXRlRm9ybWF0KGZvcm1hdCk7XG5cbiAgaWYgKCFjb25kaXRpb24pIHtcbiAgICB2YXIgZXJyb3I7XG4gICAgaWYgKGZvcm1hdCA9PT0gdW5kZWZpbmVkKSB7XG4gICAgICBlcnJvciA9IG5ldyBFcnJvcignTWluaWZpZWQgZXhjZXB0aW9uIG9jY3VycmVkOyB1c2UgdGhlIG5vbi1taW5pZmllZCBkZXYgZW52aXJvbm1lbnQgJyArICdmb3IgdGhlIGZ1bGwgZXJyb3IgbWVzc2FnZSBhbmQgYWRkaXRpb25hbCBoZWxwZnVsIHdhcm5pbmdzLicpO1xuICAgIH0gZWxzZSB7XG4gICAgICB2YXIgYXJncyA9IFthLCBiLCBjLCBkLCBlLCBmXTtcbiAgICAgIHZhciBhcmdJbmRleCA9IDA7XG4gICAgICBlcnJvciA9IG5ldyBFcnJvcihmb3JtYXQucmVwbGFjZSgvJXMvZywgZnVuY3Rpb24gKCkge1xuICAgICAgICByZXR1cm4gYXJnc1thcmdJbmRleCsrXTtcbiAgICAgIH0pKTtcbiAgICAgIGVycm9yLm5hbWUgPSAnSW52YXJpYW50IFZpb2xhdGlvbic7XG4gICAgfVxuXG4gICAgZXJyb3IuZnJhbWVzVG9Qb3AgPSAxOyAvLyB3ZSBkb24ndCBjYXJlIGFib3V0IGludmFyaWFudCdzIG93biBmcmFtZVxuICAgIHRocm93IGVycm9yO1xuICB9XG59XG5cbm1vZHVsZS5leHBvcnRzID0gaW52YXJpYW50OyIsIi8qKlxuICogQ29weXJpZ2h0IChjKSAyMDE0LXByZXNlbnQsIEZhY2Vib29rLCBJbmMuXG4gKlxuICogVGhpcyBzb3VyY2UgY29kZSBpcyBsaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UgZm91bmQgaW4gdGhlXG4gKiBMSUNFTlNFIGZpbGUgaW4gdGhlIHJvb3QgZGlyZWN0b3J5IG9mIHRoaXMgc291cmNlIHRyZWUuXG4gKlxuICovXG5cbid1c2Ugc3RyaWN0JztcblxudmFyIGVtcHR5RnVuY3Rpb24gPSByZXF1aXJlKCcuL2VtcHR5RnVuY3Rpb24nKTtcblxuLyoqXG4gKiBTaW1pbGFyIHRvIGludmFyaWFudCBidXQgb25seSBsb2dzIGEgd2FybmluZyBpZiB0aGUgY29uZGl0aW9uIGlzIG5vdCBtZXQuXG4gKiBUaGlzIGNhbiBiZSB1c2VkIHRvIGxvZyBpc3N1ZXMgaW4gZGV2ZWxvcG1lbnQgZW52aXJvbm1lbnRzIGluIGNyaXRpY2FsXG4gKiBwYXRocy4gUmVtb3ZpbmcgdGhlIGxvZ2dpbmcgY29kZSBmb3IgcHJvZHVjdGlvbiBlbnZpcm9ubWVudHMgd2lsbCBrZWVwIHRoZVxuICogc2FtZSBsb2dpYyBhbmQgZm9sbG93IHRoZSBzYW1lIGNvZGUgcGF0aHMuXG4gKi9cblxudmFyIHdhcm5pbmcgPSBlbXB0eUZ1bmN0aW9uO1xuXG5pZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICB2YXIgcHJpbnRXYXJuaW5nID0gZnVuY3Rpb24gcHJpbnRXYXJuaW5nKGZvcm1hdCkge1xuICAgIGZvciAodmFyIF9sZW4gPSBhcmd1bWVudHMubGVuZ3RoLCBhcmdzID0gQXJyYXkoX2xlbiA+IDEgPyBfbGVuIC0gMSA6IDApLCBfa2V5ID0gMTsgX2tleSA8IF9sZW47IF9rZXkrKykge1xuICAgICAgYXJnc1tfa2V5IC0gMV0gPSBhcmd1bWVudHNbX2tleV07XG4gICAgfVxuXG4gICAgdmFyIGFyZ0luZGV4ID0gMDtcbiAgICB2YXIgbWVzc2FnZSA9ICdXYXJuaW5nOiAnICsgZm9ybWF0LnJlcGxhY2UoLyVzL2csIGZ1bmN0aW9uICgpIHtcbiAgICAgIHJldHVybiBhcmdzW2FyZ0luZGV4KytdO1xuICAgIH0pO1xuICAgIGlmICh0eXBlb2YgY29uc29sZSAhPT0gJ3VuZGVmaW5lZCcpIHtcbiAgICAgIGNvbnNvbGUuZXJyb3IobWVzc2FnZSk7XG4gICAgfVxuICAgIHRyeSB7XG4gICAgICAvLyAtLS0gV2VsY29tZSB0byBkZWJ1Z2dpbmcgUmVhY3QgLS0tXG4gICAgICAvLyBUaGlzIGVycm9yIHdhcyB0aHJvd24gYXMgYSBjb252ZW5pZW5jZSBzbyB0aGF0IHlvdSBjYW4gdXNlIHRoaXMgc3RhY2tcbiAgICAgIC8vIHRvIGZpbmQgdGhlIGNhbGxzaXRlIHRoYXQgY2F1c2VkIHRoaXMgd2FybmluZyB0byBmaXJlLlxuICAgICAgdGhyb3cgbmV3IEVycm9yKG1lc3NhZ2UpO1xuICAgIH0gY2F0Y2ggKHgpIHt9XG4gIH07XG5cbiAgd2FybmluZyA9IGZ1bmN0aW9uIHdhcm5pbmcoY29uZGl0aW9uLCBmb3JtYXQpIHtcbiAgICBpZiAoZm9ybWF0ID09PSB1bmRlZmluZWQpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcignYHdhcm5pbmcoY29uZGl0aW9uLCBmb3JtYXQsIC4uLmFyZ3MpYCByZXF1aXJlcyBhIHdhcm5pbmcgJyArICdtZXNzYWdlIGFyZ3VtZW50Jyk7XG4gICAgfVxuXG4gICAgaWYgKGZvcm1hdC5pbmRleE9mKCdGYWlsZWQgQ29tcG9zaXRlIHByb3BUeXBlOiAnKSA9PT0gMCkge1xuICAgICAgcmV0dXJuOyAvLyBJZ25vcmUgQ29tcG9zaXRlQ29tcG9uZW50IHByb3B0eXBlIGNoZWNrLlxuICAgIH1cblxuICAgIGlmICghY29uZGl0aW9uKSB7XG4gICAgICBmb3IgKHZhciBfbGVuMiA9IGFyZ3VtZW50cy5sZW5ndGgsIGFyZ3MgPSBBcnJheShfbGVuMiA+IDIgPyBfbGVuMiAtIDIgOiAwKSwgX2tleTIgPSAyOyBfa2V5MiA8IF9sZW4yOyBfa2V5MisrKSB7XG4gICAgICAgIGFyZ3NbX2tleTIgLSAyXSA9IGFyZ3VtZW50c1tfa2V5Ml07XG4gICAgICB9XG5cbiAgICAgIHByaW50V2FybmluZy5hcHBseSh1bmRlZmluZWQsIFtmb3JtYXRdLmNvbmNhdChhcmdzKSk7XG4gICAgfVxuICB9O1xufVxuXG5tb2R1bGUuZXhwb3J0cyA9IHdhcm5pbmc7IiwiJ3VzZSBzdHJpY3QnO1xuXG5leHBvcnRzLl9fZXNNb2R1bGUgPSB0cnVlO1xudmFyIGNhblVzZURPTSA9IGV4cG9ydHMuY2FuVXNlRE9NID0gISEodHlwZW9mIHdpbmRvdyAhPT0gJ3VuZGVmaW5lZCcgJiYgd2luZG93LmRvY3VtZW50ICYmIHdpbmRvdy5kb2N1bWVudC5jcmVhdGVFbGVtZW50KTtcblxudmFyIGFkZEV2ZW50TGlzdGVuZXIgPSBleHBvcnRzLmFkZEV2ZW50TGlzdGVuZXIgPSBmdW5jdGlvbiBhZGRFdmVudExpc3RlbmVyKG5vZGUsIGV2ZW50LCBsaXN0ZW5lcikge1xuICByZXR1cm4gbm9kZS5hZGRFdmVudExpc3RlbmVyID8gbm9kZS5hZGRFdmVudExpc3RlbmVyKGV2ZW50LCBsaXN0ZW5lciwgZmFsc2UpIDogbm9kZS5hdHRhY2hFdmVudCgnb24nICsgZXZlbnQsIGxpc3RlbmVyKTtcbn07XG5cbnZhciByZW1vdmVFdmVudExpc3RlbmVyID0gZXhwb3J0cy5yZW1vdmVFdmVudExpc3RlbmVyID0gZnVuY3Rpb24gcmVtb3ZlRXZlbnRMaXN0ZW5lcihub2RlLCBldmVudCwgbGlzdGVuZXIpIHtcbiAgcmV0dXJuIG5vZGUucmVtb3ZlRXZlbnRMaXN0ZW5lciA/IG5vZGUucmVtb3ZlRXZlbnRMaXN0ZW5lcihldmVudCwgbGlzdGVuZXIsIGZhbHNlKSA6IG5vZGUuZGV0YWNoRXZlbnQoJ29uJyArIGV2ZW50LCBsaXN0ZW5lcik7XG59O1xuXG52YXIgZ2V0Q29uZmlybWF0aW9uID0gZXhwb3J0cy5nZXRDb25maXJtYXRpb24gPSBmdW5jdGlvbiBnZXRDb25maXJtYXRpb24obWVzc2FnZSwgY2FsbGJhY2spIHtcbiAgcmV0dXJuIGNhbGxiYWNrKHdpbmRvdy5jb25maXJtKG1lc3NhZ2UpKTtcbn07IC8vIGVzbGludC1kaXNhYmxlLWxpbmUgbm8tYWxlcnRcblxuLyoqXG4gKiBSZXR1cm5zIHRydWUgaWYgdGhlIEhUTUw1IGhpc3RvcnkgQVBJIGlzIHN1cHBvcnRlZC4gVGFrZW4gZnJvbSBNb2Rlcm5penIuXG4gKlxuICogaHR0cHM6Ly9naXRodWIuY29tL01vZGVybml6ci9Nb2Rlcm5penIvYmxvYi9tYXN0ZXIvTElDRU5TRVxuICogaHR0cHM6Ly9naXRodWIuY29tL01vZGVybml6ci9Nb2Rlcm5penIvYmxvYi9tYXN0ZXIvZmVhdHVyZS1kZXRlY3RzL2hpc3RvcnkuanNcbiAqIGNoYW5nZWQgdG8gYXZvaWQgZmFsc2UgbmVnYXRpdmVzIGZvciBXaW5kb3dzIFBob25lczogaHR0cHM6Ly9naXRodWIuY29tL3JlYWN0anMvcmVhY3Qtcm91dGVyL2lzc3Vlcy81ODZcbiAqL1xudmFyIHN1cHBvcnRzSGlzdG9yeSA9IGV4cG9ydHMuc3VwcG9ydHNIaXN0b3J5ID0gZnVuY3Rpb24gc3VwcG9ydHNIaXN0b3J5KCkge1xuICB2YXIgdWEgPSB3aW5kb3cubmF2aWdhdG9yLnVzZXJBZ2VudDtcblxuICBpZiAoKHVhLmluZGV4T2YoJ0FuZHJvaWQgMi4nKSAhPT0gLTEgfHwgdWEuaW5kZXhPZignQW5kcm9pZCA0LjAnKSAhPT0gLTEpICYmIHVhLmluZGV4T2YoJ01vYmlsZSBTYWZhcmknKSAhPT0gLTEgJiYgdWEuaW5kZXhPZignQ2hyb21lJykgPT09IC0xICYmIHVhLmluZGV4T2YoJ1dpbmRvd3MgUGhvbmUnKSA9PT0gLTEpIHJldHVybiBmYWxzZTtcblxuICByZXR1cm4gd2luZG93Lmhpc3RvcnkgJiYgJ3B1c2hTdGF0ZScgaW4gd2luZG93Lmhpc3Rvcnk7XG59O1xuXG4vKipcbiAqIFJldHVybnMgdHJ1ZSBpZiBicm93c2VyIGZpcmVzIHBvcHN0YXRlIG9uIGhhc2ggY2hhbmdlLlxuICogSUUxMCBhbmQgSUUxMSBkbyBub3QuXG4gKi9cbnZhciBzdXBwb3J0c1BvcFN0YXRlT25IYXNoQ2hhbmdlID0gZXhwb3J0cy5zdXBwb3J0c1BvcFN0YXRlT25IYXNoQ2hhbmdlID0gZnVuY3Rpb24gc3VwcG9ydHNQb3BTdGF0ZU9uSGFzaENoYW5nZSgpIHtcbiAgcmV0dXJuIHdpbmRvdy5uYXZpZ2F0b3IudXNlckFnZW50LmluZGV4T2YoJ1RyaWRlbnQnKSA9PT0gLTE7XG59O1xuXG4vKipcbiAqIFJldHVybnMgZmFsc2UgaWYgdXNpbmcgZ28obikgd2l0aCBoYXNoIGhpc3RvcnkgY2F1c2VzIGEgZnVsbCBwYWdlIHJlbG9hZC5cbiAqL1xudmFyIHN1cHBvcnRzR29XaXRob3V0UmVsb2FkVXNpbmdIYXNoID0gZXhwb3J0cy5zdXBwb3J0c0dvV2l0aG91dFJlbG9hZFVzaW5nSGFzaCA9IGZ1bmN0aW9uIHN1cHBvcnRzR29XaXRob3V0UmVsb2FkVXNpbmdIYXNoKCkge1xuICByZXR1cm4gd2luZG93Lm5hdmlnYXRvci51c2VyQWdlbnQuaW5kZXhPZignRmlyZWZveCcpID09PSAtMTtcbn07XG5cbi8qKlxuICogUmV0dXJucyB0cnVlIGlmIGEgZ2l2ZW4gcG9wc3RhdGUgZXZlbnQgaXMgYW4gZXh0cmFuZW91cyBXZWJLaXQgZXZlbnQuXG4gKiBBY2NvdW50cyBmb3IgdGhlIGZhY3QgdGhhdCBDaHJvbWUgb24gaU9TIGZpcmVzIHJlYWwgcG9wc3RhdGUgZXZlbnRzXG4gKiBjb250YWluaW5nIHVuZGVmaW5lZCBzdGF0ZSB3aGVuIHByZXNzaW5nIHRoZSBiYWNrIGJ1dHRvbi5cbiAqL1xudmFyIGlzRXh0cmFuZW91c1BvcHN0YXRlRXZlbnQgPSBleHBvcnRzLmlzRXh0cmFuZW91c1BvcHN0YXRlRXZlbnQgPSBmdW5jdGlvbiBpc0V4dHJhbmVvdXNQb3BzdGF0ZUV2ZW50KGV2ZW50KSB7XG4gIHJldHVybiBldmVudC5zdGF0ZSA9PT0gdW5kZWZpbmVkICYmIG5hdmlnYXRvci51c2VyQWdlbnQuaW5kZXhPZignQ3JpT1MnKSA9PT0gLTE7XG59OyIsIid1c2Ugc3RyaWN0JztcblxuZXhwb3J0cy5fX2VzTW9kdWxlID0gdHJ1ZTtcbmV4cG9ydHMubG9jYXRpb25zQXJlRXF1YWwgPSBleHBvcnRzLmNyZWF0ZUxvY2F0aW9uID0gdW5kZWZpbmVkO1xuXG52YXIgX2V4dGVuZHMgPSBPYmplY3QuYXNzaWduIHx8IGZ1bmN0aW9uICh0YXJnZXQpIHsgZm9yICh2YXIgaSA9IDE7IGkgPCBhcmd1bWVudHMubGVuZ3RoOyBpKyspIHsgdmFyIHNvdXJjZSA9IGFyZ3VtZW50c1tpXTsgZm9yICh2YXIga2V5IGluIHNvdXJjZSkgeyBpZiAoT2JqZWN0LnByb3RvdHlwZS5oYXNPd25Qcm9wZXJ0eS5jYWxsKHNvdXJjZSwga2V5KSkgeyB0YXJnZXRba2V5XSA9IHNvdXJjZVtrZXldOyB9IH0gfSByZXR1cm4gdGFyZ2V0OyB9O1xuXG52YXIgX3Jlc29sdmVQYXRobmFtZSA9IHJlcXVpcmUoJ3Jlc29sdmUtcGF0aG5hbWUnKTtcblxudmFyIF9yZXNvbHZlUGF0aG5hbWUyID0gX2ludGVyb3BSZXF1aXJlRGVmYXVsdChfcmVzb2x2ZVBhdGhuYW1lKTtcblxudmFyIF92YWx1ZUVxdWFsID0gcmVxdWlyZSgndmFsdWUtZXF1YWwnKTtcblxudmFyIF92YWx1ZUVxdWFsMiA9IF9pbnRlcm9wUmVxdWlyZURlZmF1bHQoX3ZhbHVlRXF1YWwpO1xuXG52YXIgX1BhdGhVdGlscyA9IHJlcXVpcmUoJy4vUGF0aFV0aWxzJyk7XG5cbmZ1bmN0aW9uIF9pbnRlcm9wUmVxdWlyZURlZmF1bHQob2JqKSB7IHJldHVybiBvYmogJiYgb2JqLl9fZXNNb2R1bGUgPyBvYmogOiB7IGRlZmF1bHQ6IG9iaiB9OyB9XG5cbnZhciBjcmVhdGVMb2NhdGlvbiA9IGV4cG9ydHMuY3JlYXRlTG9jYXRpb24gPSBmdW5jdGlvbiBjcmVhdGVMb2NhdGlvbihwYXRoLCBzdGF0ZSwga2V5LCBjdXJyZW50TG9jYXRpb24pIHtcbiAgdmFyIGxvY2F0aW9uID0gdm9pZCAwO1xuICBpZiAodHlwZW9mIHBhdGggPT09ICdzdHJpbmcnKSB7XG4gICAgLy8gVHdvLWFyZyBmb3JtOiBwdXNoKHBhdGgsIHN0YXRlKVxuICAgIGxvY2F0aW9uID0gKDAsIF9QYXRoVXRpbHMucGFyc2VQYXRoKShwYXRoKTtcbiAgICBsb2NhdGlvbi5zdGF0ZSA9IHN0YXRlO1xuICB9IGVsc2Uge1xuICAgIC8vIE9uZS1hcmcgZm9ybTogcHVzaChsb2NhdGlvbilcbiAgICBsb2NhdGlvbiA9IF9leHRlbmRzKHt9LCBwYXRoKTtcblxuICAgIGlmIChsb2NhdGlvbi5wYXRobmFtZSA9PT0gdW5kZWZpbmVkKSBsb2NhdGlvbi5wYXRobmFtZSA9ICcnO1xuXG4gICAgaWYgKGxvY2F0aW9uLnNlYXJjaCkge1xuICAgICAgaWYgKGxvY2F0aW9uLnNlYXJjaC5jaGFyQXQoMCkgIT09ICc/JykgbG9jYXRpb24uc2VhcmNoID0gJz8nICsgbG9jYXRpb24uc2VhcmNoO1xuICAgIH0gZWxzZSB7XG4gICAgICBsb2NhdGlvbi5zZWFyY2ggPSAnJztcbiAgICB9XG5cbiAgICBpZiAobG9jYXRpb24uaGFzaCkge1xuICAgICAgaWYgKGxvY2F0aW9uLmhhc2guY2hhckF0KDApICE9PSAnIycpIGxvY2F0aW9uLmhhc2ggPSAnIycgKyBsb2NhdGlvbi5oYXNoO1xuICAgIH0gZWxzZSB7XG4gICAgICBsb2NhdGlvbi5oYXNoID0gJyc7XG4gICAgfVxuXG4gICAgaWYgKHN0YXRlICE9PSB1bmRlZmluZWQgJiYgbG9jYXRpb24uc3RhdGUgPT09IHVuZGVmaW5lZCkgbG9jYXRpb24uc3RhdGUgPSBzdGF0ZTtcbiAgfVxuXG4gIHRyeSB7XG4gICAgbG9jYXRpb24ucGF0aG5hbWUgPSBkZWNvZGVVUkkobG9jYXRpb24ucGF0aG5hbWUpO1xuICB9IGNhdGNoIChlKSB7XG4gICAgaWYgKGUgaW5zdGFuY2VvZiBVUklFcnJvcikge1xuICAgICAgdGhyb3cgbmV3IFVSSUVycm9yKCdQYXRobmFtZSBcIicgKyBsb2NhdGlvbi5wYXRobmFtZSArICdcIiBjb3VsZCBub3QgYmUgZGVjb2RlZC4gJyArICdUaGlzIGlzIGxpa2VseSBjYXVzZWQgYnkgYW4gaW52YWxpZCBwZXJjZW50LWVuY29kaW5nLicpO1xuICAgIH0gZWxzZSB7XG4gICAgICB0aHJvdyBlO1xuICAgIH1cbiAgfVxuXG4gIGlmIChrZXkpIGxvY2F0aW9uLmtleSA9IGtleTtcblxuICBpZiAoY3VycmVudExvY2F0aW9uKSB7XG4gICAgLy8gUmVzb2x2ZSBpbmNvbXBsZXRlL3JlbGF0aXZlIHBhdGhuYW1lIHJlbGF0aXZlIHRvIGN1cnJlbnQgbG9jYXRpb24uXG4gICAgaWYgKCFsb2NhdGlvbi5wYXRobmFtZSkge1xuICAgICAgbG9jYXRpb24ucGF0aG5hbWUgPSBjdXJyZW50TG9jYXRpb24ucGF0aG5hbWU7XG4gICAgfSBlbHNlIGlmIChsb2NhdGlvbi5wYXRobmFtZS5jaGFyQXQoMCkgIT09ICcvJykge1xuICAgICAgbG9jYXRpb24ucGF0aG5hbWUgPSAoMCwgX3Jlc29sdmVQYXRobmFtZTIuZGVmYXVsdCkobG9jYXRpb24ucGF0aG5hbWUsIGN1cnJlbnRMb2NhdGlvbi5wYXRobmFtZSk7XG4gICAgfVxuICB9IGVsc2Uge1xuICAgIC8vIFdoZW4gdGhlcmUgaXMgbm8gcHJpb3IgbG9jYXRpb24gYW5kIHBhdGhuYW1lIGlzIGVtcHR5LCBzZXQgaXQgdG8gL1xuICAgIGlmICghbG9jYXRpb24ucGF0aG5hbWUpIHtcbiAgICAgIGxvY2F0aW9uLnBhdGhuYW1lID0gJy8nO1xuICAgIH1cbiAgfVxuXG4gIHJldHVybiBsb2NhdGlvbjtcbn07XG5cbnZhciBsb2NhdGlvbnNBcmVFcXVhbCA9IGV4cG9ydHMubG9jYXRpb25zQXJlRXF1YWwgPSBmdW5jdGlvbiBsb2NhdGlvbnNBcmVFcXVhbChhLCBiKSB7XG4gIHJldHVybiBhLnBhdGhuYW1lID09PSBiLnBhdGhuYW1lICYmIGEuc2VhcmNoID09PSBiLnNlYXJjaCAmJiBhLmhhc2ggPT09IGIuaGFzaCAmJiBhLmtleSA9PT0gYi5rZXkgJiYgKDAsIF92YWx1ZUVxdWFsMi5kZWZhdWx0KShhLnN0YXRlLCBiLnN0YXRlKTtcbn07IiwiJ3VzZSBzdHJpY3QnO1xuXG5leHBvcnRzLl9fZXNNb2R1bGUgPSB0cnVlO1xudmFyIGFkZExlYWRpbmdTbGFzaCA9IGV4cG9ydHMuYWRkTGVhZGluZ1NsYXNoID0gZnVuY3Rpb24gYWRkTGVhZGluZ1NsYXNoKHBhdGgpIHtcbiAgcmV0dXJuIHBhdGguY2hhckF0KDApID09PSAnLycgPyBwYXRoIDogJy8nICsgcGF0aDtcbn07XG5cbnZhciBzdHJpcExlYWRpbmdTbGFzaCA9IGV4cG9ydHMuc3RyaXBMZWFkaW5nU2xhc2ggPSBmdW5jdGlvbiBzdHJpcExlYWRpbmdTbGFzaChwYXRoKSB7XG4gIHJldHVybiBwYXRoLmNoYXJBdCgwKSA9PT0gJy8nID8gcGF0aC5zdWJzdHIoMSkgOiBwYXRoO1xufTtcblxudmFyIGhhc0Jhc2VuYW1lID0gZXhwb3J0cy5oYXNCYXNlbmFtZSA9IGZ1bmN0aW9uIGhhc0Jhc2VuYW1lKHBhdGgsIHByZWZpeCkge1xuICByZXR1cm4gbmV3IFJlZ0V4cCgnXicgKyBwcmVmaXggKyAnKFxcXFwvfFxcXFw/fCN8JCknLCAnaScpLnRlc3QocGF0aCk7XG59O1xuXG52YXIgc3RyaXBCYXNlbmFtZSA9IGV4cG9ydHMuc3RyaXBCYXNlbmFtZSA9IGZ1bmN0aW9uIHN0cmlwQmFzZW5hbWUocGF0aCwgcHJlZml4KSB7XG4gIHJldHVybiBoYXNCYXNlbmFtZShwYXRoLCBwcmVmaXgpID8gcGF0aC5zdWJzdHIocHJlZml4Lmxlbmd0aCkgOiBwYXRoO1xufTtcblxudmFyIHN0cmlwVHJhaWxpbmdTbGFzaCA9IGV4cG9ydHMuc3RyaXBUcmFpbGluZ1NsYXNoID0gZnVuY3Rpb24gc3RyaXBUcmFpbGluZ1NsYXNoKHBhdGgpIHtcbiAgcmV0dXJuIHBhdGguY2hhckF0KHBhdGgubGVuZ3RoIC0gMSkgPT09ICcvJyA/IHBhdGguc2xpY2UoMCwgLTEpIDogcGF0aDtcbn07XG5cbnZhciBwYXJzZVBhdGggPSBleHBvcnRzLnBhcnNlUGF0aCA9IGZ1bmN0aW9uIHBhcnNlUGF0aChwYXRoKSB7XG4gIHZhciBwYXRobmFtZSA9IHBhdGggfHwgJy8nO1xuICB2YXIgc2VhcmNoID0gJyc7XG4gIHZhciBoYXNoID0gJyc7XG5cbiAgdmFyIGhhc2hJbmRleCA9IHBhdGhuYW1lLmluZGV4T2YoJyMnKTtcbiAgaWYgKGhhc2hJbmRleCAhPT0gLTEpIHtcbiAgICBoYXNoID0gcGF0aG5hbWUuc3Vic3RyKGhhc2hJbmRleCk7XG4gICAgcGF0aG5hbWUgPSBwYXRobmFtZS5zdWJzdHIoMCwgaGFzaEluZGV4KTtcbiAgfVxuXG4gIHZhciBzZWFyY2hJbmRleCA9IHBhdGhuYW1lLmluZGV4T2YoJz8nKTtcbiAgaWYgKHNlYXJjaEluZGV4ICE9PSAtMSkge1xuICAgIHNlYXJjaCA9IHBhdGhuYW1lLnN1YnN0cihzZWFyY2hJbmRleCk7XG4gICAgcGF0aG5hbWUgPSBwYXRobmFtZS5zdWJzdHIoMCwgc2VhcmNoSW5kZXgpO1xuICB9XG5cbiAgcmV0dXJuIHtcbiAgICBwYXRobmFtZTogcGF0aG5hbWUsXG4gICAgc2VhcmNoOiBzZWFyY2ggPT09ICc/JyA/ICcnIDogc2VhcmNoLFxuICAgIGhhc2g6IGhhc2ggPT09ICcjJyA/ICcnIDogaGFzaFxuICB9O1xufTtcblxudmFyIGNyZWF0ZVBhdGggPSBleHBvcnRzLmNyZWF0ZVBhdGggPSBmdW5jdGlvbiBjcmVhdGVQYXRoKGxvY2F0aW9uKSB7XG4gIHZhciBwYXRobmFtZSA9IGxvY2F0aW9uLnBhdGhuYW1lLFxuICAgICAgc2VhcmNoID0gbG9jYXRpb24uc2VhcmNoLFxuICAgICAgaGFzaCA9IGxvY2F0aW9uLmhhc2g7XG5cblxuICB2YXIgcGF0aCA9IHBhdGhuYW1lIHx8ICcvJztcblxuICBpZiAoc2VhcmNoICYmIHNlYXJjaCAhPT0gJz8nKSBwYXRoICs9IHNlYXJjaC5jaGFyQXQoMCkgPT09ICc/JyA/IHNlYXJjaCA6ICc/JyArIHNlYXJjaDtcblxuICBpZiAoaGFzaCAmJiBoYXNoICE9PSAnIycpIHBhdGggKz0gaGFzaC5jaGFyQXQoMCkgPT09ICcjJyA/IGhhc2ggOiAnIycgKyBoYXNoO1xuXG4gIHJldHVybiBwYXRoO1xufTsiLCIndXNlIHN0cmljdCc7XG5cbmV4cG9ydHMuX19lc01vZHVsZSA9IHRydWU7XG5cbnZhciBfdHlwZW9mID0gdHlwZW9mIFN5bWJvbCA9PT0gXCJmdW5jdGlvblwiICYmIHR5cGVvZiBTeW1ib2wuaXRlcmF0b3IgPT09IFwic3ltYm9sXCIgPyBmdW5jdGlvbiAob2JqKSB7IHJldHVybiB0eXBlb2Ygb2JqOyB9IDogZnVuY3Rpb24gKG9iaikgeyByZXR1cm4gb2JqICYmIHR5cGVvZiBTeW1ib2wgPT09IFwiZnVuY3Rpb25cIiAmJiBvYmouY29uc3RydWN0b3IgPT09IFN5bWJvbCAmJiBvYmogIT09IFN5bWJvbC5wcm90b3R5cGUgPyBcInN5bWJvbFwiIDogdHlwZW9mIG9iajsgfTtcblxudmFyIF9leHRlbmRzID0gT2JqZWN0LmFzc2lnbiB8fCBmdW5jdGlvbiAodGFyZ2V0KSB7IGZvciAodmFyIGkgPSAxOyBpIDwgYXJndW1lbnRzLmxlbmd0aDsgaSsrKSB7IHZhciBzb3VyY2UgPSBhcmd1bWVudHNbaV07IGZvciAodmFyIGtleSBpbiBzb3VyY2UpIHsgaWYgKE9iamVjdC5wcm90b3R5cGUuaGFzT3duUHJvcGVydHkuY2FsbChzb3VyY2UsIGtleSkpIHsgdGFyZ2V0W2tleV0gPSBzb3VyY2Vba2V5XTsgfSB9IH0gcmV0dXJuIHRhcmdldDsgfTtcblxudmFyIF93YXJuaW5nID0gcmVxdWlyZSgnd2FybmluZycpO1xuXG52YXIgX3dhcm5pbmcyID0gX2ludGVyb3BSZXF1aXJlRGVmYXVsdChfd2FybmluZyk7XG5cbnZhciBfaW52YXJpYW50ID0gcmVxdWlyZSgnaW52YXJpYW50Jyk7XG5cbnZhciBfaW52YXJpYW50MiA9IF9pbnRlcm9wUmVxdWlyZURlZmF1bHQoX2ludmFyaWFudCk7XG5cbnZhciBfTG9jYXRpb25VdGlscyA9IHJlcXVpcmUoJy4vTG9jYXRpb25VdGlscycpO1xuXG52YXIgX1BhdGhVdGlscyA9IHJlcXVpcmUoJy4vUGF0aFV0aWxzJyk7XG5cbnZhciBfY3JlYXRlVHJhbnNpdGlvbk1hbmFnZXIgPSByZXF1aXJlKCcuL2NyZWF0ZVRyYW5zaXRpb25NYW5hZ2VyJyk7XG5cbnZhciBfY3JlYXRlVHJhbnNpdGlvbk1hbmFnZXIyID0gX2ludGVyb3BSZXF1aXJlRGVmYXVsdChfY3JlYXRlVHJhbnNpdGlvbk1hbmFnZXIpO1xuXG52YXIgX0RPTVV0aWxzID0gcmVxdWlyZSgnLi9ET01VdGlscycpO1xuXG5mdW5jdGlvbiBfaW50ZXJvcFJlcXVpcmVEZWZhdWx0KG9iaikgeyByZXR1cm4gb2JqICYmIG9iai5fX2VzTW9kdWxlID8gb2JqIDogeyBkZWZhdWx0OiBvYmogfTsgfVxuXG52YXIgUG9wU3RhdGVFdmVudCA9ICdwb3BzdGF0ZSc7XG52YXIgSGFzaENoYW5nZUV2ZW50ID0gJ2hhc2hjaGFuZ2UnO1xuXG52YXIgZ2V0SGlzdG9yeVN0YXRlID0gZnVuY3Rpb24gZ2V0SGlzdG9yeVN0YXRlKCkge1xuICB0cnkge1xuICAgIHJldHVybiB3aW5kb3cuaGlzdG9yeS5zdGF0ZSB8fCB7fTtcbiAgfSBjYXRjaCAoZSkge1xuICAgIC8vIElFIDExIHNvbWV0aW1lcyB0aHJvd3Mgd2hlbiBhY2Nlc3Npbmcgd2luZG93Lmhpc3Rvcnkuc3RhdGVcbiAgICAvLyBTZWUgaHR0cHM6Ly9naXRodWIuY29tL1JlYWN0VHJhaW5pbmcvaGlzdG9yeS9wdWxsLzI4OVxuICAgIHJldHVybiB7fTtcbiAgfVxufTtcblxuLyoqXG4gKiBDcmVhdGVzIGEgaGlzdG9yeSBvYmplY3QgdGhhdCB1c2VzIHRoZSBIVE1MNSBoaXN0b3J5IEFQSSBpbmNsdWRpbmdcbiAqIHB1c2hTdGF0ZSwgcmVwbGFjZVN0YXRlLCBhbmQgdGhlIHBvcHN0YXRlIGV2ZW50LlxuICovXG52YXIgY3JlYXRlQnJvd3Nlckhpc3RvcnkgPSBmdW5jdGlvbiBjcmVhdGVCcm93c2VySGlzdG9yeSgpIHtcbiAgdmFyIHByb3BzID0gYXJndW1lbnRzLmxlbmd0aCA+IDAgJiYgYXJndW1lbnRzWzBdICE9PSB1bmRlZmluZWQgPyBhcmd1bWVudHNbMF0gOiB7fTtcblxuICAoMCwgX2ludmFyaWFudDIuZGVmYXVsdCkoX0RPTVV0aWxzLmNhblVzZURPTSwgJ0Jyb3dzZXIgaGlzdG9yeSBuZWVkcyBhIERPTScpO1xuXG4gIHZhciBnbG9iYWxIaXN0b3J5ID0gd2luZG93Lmhpc3Rvcnk7XG4gIHZhciBjYW5Vc2VIaXN0b3J5ID0gKDAsIF9ET01VdGlscy5zdXBwb3J0c0hpc3RvcnkpKCk7XG4gIHZhciBuZWVkc0hhc2hDaGFuZ2VMaXN0ZW5lciA9ICEoMCwgX0RPTVV0aWxzLnN1cHBvcnRzUG9wU3RhdGVPbkhhc2hDaGFuZ2UpKCk7XG5cbiAgdmFyIF9wcm9wcyRmb3JjZVJlZnJlc2ggPSBwcm9wcy5mb3JjZVJlZnJlc2gsXG4gICAgICBmb3JjZVJlZnJlc2ggPSBfcHJvcHMkZm9yY2VSZWZyZXNoID09PSB1bmRlZmluZWQgPyBmYWxzZSA6IF9wcm9wcyRmb3JjZVJlZnJlc2gsXG4gICAgICBfcHJvcHMkZ2V0VXNlckNvbmZpcm0gPSBwcm9wcy5nZXRVc2VyQ29uZmlybWF0aW9uLFxuICAgICAgZ2V0VXNlckNvbmZpcm1hdGlvbiA9IF9wcm9wcyRnZXRVc2VyQ29uZmlybSA9PT0gdW5kZWZpbmVkID8gX0RPTVV0aWxzLmdldENvbmZpcm1hdGlvbiA6IF9wcm9wcyRnZXRVc2VyQ29uZmlybSxcbiAgICAgIF9wcm9wcyRrZXlMZW5ndGggPSBwcm9wcy5rZXlMZW5ndGgsXG4gICAgICBrZXlMZW5ndGggPSBfcHJvcHMka2V5TGVuZ3RoID09PSB1bmRlZmluZWQgPyA2IDogX3Byb3BzJGtleUxlbmd0aDtcblxuICB2YXIgYmFzZW5hbWUgPSBwcm9wcy5iYXNlbmFtZSA/ICgwLCBfUGF0aFV0aWxzLnN0cmlwVHJhaWxpbmdTbGFzaCkoKDAsIF9QYXRoVXRpbHMuYWRkTGVhZGluZ1NsYXNoKShwcm9wcy5iYXNlbmFtZSkpIDogJyc7XG5cbiAgdmFyIGdldERPTUxvY2F0aW9uID0gZnVuY3Rpb24gZ2V0RE9NTG9jYXRpb24oaGlzdG9yeVN0YXRlKSB7XG4gICAgdmFyIF9yZWYgPSBoaXN0b3J5U3RhdGUgfHwge30sXG4gICAgICAgIGtleSA9IF9yZWYua2V5LFxuICAgICAgICBzdGF0ZSA9IF9yZWYuc3RhdGU7XG5cbiAgICB2YXIgX3dpbmRvdyRsb2NhdGlvbiA9IHdpbmRvdy5sb2NhdGlvbixcbiAgICAgICAgcGF0aG5hbWUgPSBfd2luZG93JGxvY2F0aW9uLnBhdGhuYW1lLFxuICAgICAgICBzZWFyY2ggPSBfd2luZG93JGxvY2F0aW9uLnNlYXJjaCxcbiAgICAgICAgaGFzaCA9IF93aW5kb3ckbG9jYXRpb24uaGFzaDtcblxuXG4gICAgdmFyIHBhdGggPSBwYXRobmFtZSArIHNlYXJjaCArIGhhc2g7XG5cbiAgICAoMCwgX3dhcm5pbmcyLmRlZmF1bHQpKCFiYXNlbmFtZSB8fCAoMCwgX1BhdGhVdGlscy5oYXNCYXNlbmFtZSkocGF0aCwgYmFzZW5hbWUpLCAnWW91IGFyZSBhdHRlbXB0aW5nIHRvIHVzZSBhIGJhc2VuYW1lIG9uIGEgcGFnZSB3aG9zZSBVUkwgcGF0aCBkb2VzIG5vdCBiZWdpbiAnICsgJ3dpdGggdGhlIGJhc2VuYW1lLiBFeHBlY3RlZCBwYXRoIFwiJyArIHBhdGggKyAnXCIgdG8gYmVnaW4gd2l0aCBcIicgKyBiYXNlbmFtZSArICdcIi4nKTtcblxuICAgIGlmIChiYXNlbmFtZSkgcGF0aCA9ICgwLCBfUGF0aFV0aWxzLnN0cmlwQmFzZW5hbWUpKHBhdGgsIGJhc2VuYW1lKTtcblxuICAgIHJldHVybiAoMCwgX0xvY2F0aW9uVXRpbHMuY3JlYXRlTG9jYXRpb24pKHBhdGgsIHN0YXRlLCBrZXkpO1xuICB9O1xuXG4gIHZhciBjcmVhdGVLZXkgPSBmdW5jdGlvbiBjcmVhdGVLZXkoKSB7XG4gICAgcmV0dXJuIE1hdGgucmFuZG9tKCkudG9TdHJpbmcoMzYpLnN1YnN0cigyLCBrZXlMZW5ndGgpO1xuICB9O1xuXG4gIHZhciB0cmFuc2l0aW9uTWFuYWdlciA9ICgwLCBfY3JlYXRlVHJhbnNpdGlvbk1hbmFnZXIyLmRlZmF1bHQpKCk7XG5cbiAgdmFyIHNldFN0YXRlID0gZnVuY3Rpb24gc2V0U3RhdGUobmV4dFN0YXRlKSB7XG4gICAgX2V4dGVuZHMoaGlzdG9yeSwgbmV4dFN0YXRlKTtcblxuICAgIGhpc3RvcnkubGVuZ3RoID0gZ2xvYmFsSGlzdG9yeS5sZW5ndGg7XG5cbiAgICB0cmFuc2l0aW9uTWFuYWdlci5ub3RpZnlMaXN0ZW5lcnMoaGlzdG9yeS5sb2NhdGlvbiwgaGlzdG9yeS5hY3Rpb24pO1xuICB9O1xuXG4gIHZhciBoYW5kbGVQb3BTdGF0ZSA9IGZ1bmN0aW9uIGhhbmRsZVBvcFN0YXRlKGV2ZW50KSB7XG4gICAgLy8gSWdub3JlIGV4dHJhbmVvdXMgcG9wc3RhdGUgZXZlbnRzIGluIFdlYktpdC5cbiAgICBpZiAoKDAsIF9ET01VdGlscy5pc0V4dHJhbmVvdXNQb3BzdGF0ZUV2ZW50KShldmVudCkpIHJldHVybjtcblxuICAgIGhhbmRsZVBvcChnZXRET01Mb2NhdGlvbihldmVudC5zdGF0ZSkpO1xuICB9O1xuXG4gIHZhciBoYW5kbGVIYXNoQ2hhbmdlID0gZnVuY3Rpb24gaGFuZGxlSGFzaENoYW5nZSgpIHtcbiAgICBoYW5kbGVQb3AoZ2V0RE9NTG9jYXRpb24oZ2V0SGlzdG9yeVN0YXRlKCkpKTtcbiAgfTtcblxuICB2YXIgZm9yY2VOZXh0UG9wID0gZmFsc2U7XG5cbiAgdmFyIGhhbmRsZVBvcCA9IGZ1bmN0aW9uIGhhbmRsZVBvcChsb2NhdGlvbikge1xuICAgIGlmIChmb3JjZU5leHRQb3ApIHtcbiAgICAgIGZvcmNlTmV4dFBvcCA9IGZhbHNlO1xuICAgICAgc2V0U3RhdGUoKTtcbiAgICB9IGVsc2Uge1xuICAgICAgdmFyIGFjdGlvbiA9ICdQT1AnO1xuXG4gICAgICB0cmFuc2l0aW9uTWFuYWdlci5jb25maXJtVHJhbnNpdGlvblRvKGxvY2F0aW9uLCBhY3Rpb24sIGdldFVzZXJDb25maXJtYXRpb24sIGZ1bmN0aW9uIChvaykge1xuICAgICAgICBpZiAob2spIHtcbiAgICAgICAgICBzZXRTdGF0ZSh7IGFjdGlvbjogYWN0aW9uLCBsb2NhdGlvbjogbG9jYXRpb24gfSk7XG4gICAgICAgIH0gZWxzZSB7XG4gICAgICAgICAgcmV2ZXJ0UG9wKGxvY2F0aW9uKTtcbiAgICAgICAgfVxuICAgICAgfSk7XG4gICAgfVxuICB9O1xuXG4gIHZhciByZXZlcnRQb3AgPSBmdW5jdGlvbiByZXZlcnRQb3AoZnJvbUxvY2F0aW9uKSB7XG4gICAgdmFyIHRvTG9jYXRpb24gPSBoaXN0b3J5LmxvY2F0aW9uO1xuXG4gICAgLy8gVE9ETzogV2UgY291bGQgcHJvYmFibHkgbWFrZSB0aGlzIG1vcmUgcmVsaWFibGUgYnlcbiAgICAvLyBrZWVwaW5nIGEgbGlzdCBvZiBrZXlzIHdlJ3ZlIHNlZW4gaW4gc2Vzc2lvblN0b3JhZ2UuXG4gICAgLy8gSW5zdGVhZCwgd2UganVzdCBkZWZhdWx0IHRvIDAgZm9yIGtleXMgd2UgZG9uJ3Qga25vdy5cblxuICAgIHZhciB0b0luZGV4ID0gYWxsS2V5cy5pbmRleE9mKHRvTG9jYXRpb24ua2V5KTtcblxuICAgIGlmICh0b0luZGV4ID09PSAtMSkgdG9JbmRleCA9IDA7XG5cbiAgICB2YXIgZnJvbUluZGV4ID0gYWxsS2V5cy5pbmRleE9mKGZyb21Mb2NhdGlvbi5rZXkpO1xuXG4gICAgaWYgKGZyb21JbmRleCA9PT0gLTEpIGZyb21JbmRleCA9IDA7XG5cbiAgICB2YXIgZGVsdGEgPSB0b0luZGV4IC0gZnJvbUluZGV4O1xuXG4gICAgaWYgKGRlbHRhKSB7XG4gICAgICBmb3JjZU5leHRQb3AgPSB0cnVlO1xuICAgICAgZ28oZGVsdGEpO1xuICAgIH1cbiAgfTtcblxuICB2YXIgaW5pdGlhbExvY2F0aW9uID0gZ2V0RE9NTG9jYXRpb24oZ2V0SGlzdG9yeVN0YXRlKCkpO1xuICB2YXIgYWxsS2V5cyA9IFtpbml0aWFsTG9jYXRpb24ua2V5XTtcblxuICAvLyBQdWJsaWMgaW50ZXJmYWNlXG5cbiAgdmFyIGNyZWF0ZUhyZWYgPSBmdW5jdGlvbiBjcmVhdGVIcmVmKGxvY2F0aW9uKSB7XG4gICAgcmV0dXJuIGJhc2VuYW1lICsgKDAsIF9QYXRoVXRpbHMuY3JlYXRlUGF0aCkobG9jYXRpb24pO1xuICB9O1xuXG4gIHZhciBwdXNoID0gZnVuY3Rpb24gcHVzaChwYXRoLCBzdGF0ZSkge1xuICAgICgwLCBfd2FybmluZzIuZGVmYXVsdCkoISgodHlwZW9mIHBhdGggPT09ICd1bmRlZmluZWQnID8gJ3VuZGVmaW5lZCcgOiBfdHlwZW9mKHBhdGgpKSA9PT0gJ29iamVjdCcgJiYgcGF0aC5zdGF0ZSAhPT0gdW5kZWZpbmVkICYmIHN0YXRlICE9PSB1bmRlZmluZWQpLCAnWW91IHNob3VsZCBhdm9pZCBwcm92aWRpbmcgYSAybmQgc3RhdGUgYXJndW1lbnQgdG8gcHVzaCB3aGVuIHRoZSAxc3QgJyArICdhcmd1bWVudCBpcyBhIGxvY2F0aW9uLWxpa2Ugb2JqZWN0IHRoYXQgYWxyZWFkeSBoYXMgc3RhdGU7IGl0IGlzIGlnbm9yZWQnKTtcblxuICAgIHZhciBhY3Rpb24gPSAnUFVTSCc7XG4gICAgdmFyIGxvY2F0aW9uID0gKDAsIF9Mb2NhdGlvblV0aWxzLmNyZWF0ZUxvY2F0aW9uKShwYXRoLCBzdGF0ZSwgY3JlYXRlS2V5KCksIGhpc3RvcnkubG9jYXRpb24pO1xuXG4gICAgdHJhbnNpdGlvbk1hbmFnZXIuY29uZmlybVRyYW5zaXRpb25Ubyhsb2NhdGlvbiwgYWN0aW9uLCBnZXRVc2VyQ29uZmlybWF0aW9uLCBmdW5jdGlvbiAob2spIHtcbiAgICAgIGlmICghb2spIHJldHVybjtcblxuICAgICAgdmFyIGhyZWYgPSBjcmVhdGVIcmVmKGxvY2F0aW9uKTtcbiAgICAgIHZhciBrZXkgPSBsb2NhdGlvbi5rZXksXG4gICAgICAgICAgc3RhdGUgPSBsb2NhdGlvbi5zdGF0ZTtcblxuXG4gICAgICBpZiAoY2FuVXNlSGlzdG9yeSkge1xuICAgICAgICBnbG9iYWxIaXN0b3J5LnB1c2hTdGF0ZSh7IGtleToga2V5LCBzdGF0ZTogc3RhdGUgfSwgbnVsbCwgaHJlZik7XG5cbiAgICAgICAgaWYgKGZvcmNlUmVmcmVzaCkge1xuICAgICAgICAgIHdpbmRvdy5sb2NhdGlvbi5ocmVmID0gaHJlZjtcbiAgICAgICAgfSBlbHNlIHtcbiAgICAgICAgICB2YXIgcHJldkluZGV4ID0gYWxsS2V5cy5pbmRleE9mKGhpc3RvcnkubG9jYXRpb24ua2V5KTtcbiAgICAgICAgICB2YXIgbmV4dEtleXMgPSBhbGxLZXlzLnNsaWNlKDAsIHByZXZJbmRleCA9PT0gLTEgPyAwIDogcHJldkluZGV4ICsgMSk7XG5cbiAgICAgICAgICBuZXh0S2V5cy5wdXNoKGxvY2F0aW9uLmtleSk7XG4gICAgICAgICAgYWxsS2V5cyA9IG5leHRLZXlzO1xuXG4gICAgICAgICAgc2V0U3RhdGUoeyBhY3Rpb246IGFjdGlvbiwgbG9jYXRpb246IGxvY2F0aW9uIH0pO1xuICAgICAgICB9XG4gICAgICB9IGVsc2Uge1xuICAgICAgICAoMCwgX3dhcm5pbmcyLmRlZmF1bHQpKHN0YXRlID09PSB1bmRlZmluZWQsICdCcm93c2VyIGhpc3RvcnkgY2Fubm90IHB1c2ggc3RhdGUgaW4gYnJvd3NlcnMgdGhhdCBkbyBub3Qgc3VwcG9ydCBIVE1MNSBoaXN0b3J5Jyk7XG5cbiAgICAgICAgd2luZG93LmxvY2F0aW9uLmhyZWYgPSBocmVmO1xuICAgICAgfVxuICAgIH0pO1xuICB9O1xuXG4gIHZhciByZXBsYWNlID0gZnVuY3Rpb24gcmVwbGFjZShwYXRoLCBzdGF0ZSkge1xuICAgICgwLCBfd2FybmluZzIuZGVmYXVsdCkoISgodHlwZW9mIHBhdGggPT09ICd1bmRlZmluZWQnID8gJ3VuZGVmaW5lZCcgOiBfdHlwZW9mKHBhdGgpKSA9PT0gJ29iamVjdCcgJiYgcGF0aC5zdGF0ZSAhPT0gdW5kZWZpbmVkICYmIHN0YXRlICE9PSB1bmRlZmluZWQpLCAnWW91IHNob3VsZCBhdm9pZCBwcm92aWRpbmcgYSAybmQgc3RhdGUgYXJndW1lbnQgdG8gcmVwbGFjZSB3aGVuIHRoZSAxc3QgJyArICdhcmd1bWVudCBpcyBhIGxvY2F0aW9uLWxpa2Ugb2JqZWN0IHRoYXQgYWxyZWFkeSBoYXMgc3RhdGU7IGl0IGlzIGlnbm9yZWQnKTtcblxuICAgIHZhciBhY3Rpb24gPSAnUkVQTEFDRSc7XG4gICAgdmFyIGxvY2F0aW9uID0gKDAsIF9Mb2NhdGlvblV0aWxzLmNyZWF0ZUxvY2F0aW9uKShwYXRoLCBzdGF0ZSwgY3JlYXRlS2V5KCksIGhpc3RvcnkubG9jYXRpb24pO1xuXG4gICAgdHJhbnNpdGlvbk1hbmFnZXIuY29uZmlybVRyYW5zaXRpb25Ubyhsb2NhdGlvbiwgYWN0aW9uLCBnZXRVc2VyQ29uZmlybWF0aW9uLCBmdW5jdGlvbiAob2spIHtcbiAgICAgIGlmICghb2spIHJldHVybjtcblxuICAgICAgdmFyIGhyZWYgPSBjcmVhdGVIcmVmKGxvY2F0aW9uKTtcbiAgICAgIHZhciBrZXkgPSBsb2NhdGlvbi5rZXksXG4gICAgICAgICAgc3RhdGUgPSBsb2NhdGlvbi5zdGF0ZTtcblxuXG4gICAgICBpZiAoY2FuVXNlSGlzdG9yeSkge1xuICAgICAgICBnbG9iYWxIaXN0b3J5LnJlcGxhY2VTdGF0ZSh7IGtleToga2V5LCBzdGF0ZTogc3RhdGUgfSwgbnVsbCwgaHJlZik7XG5cbiAgICAgICAgaWYgKGZvcmNlUmVmcmVzaCkge1xuICAgICAgICAgIHdpbmRvdy5sb2NhdGlvbi5yZXBsYWNlKGhyZWYpO1xuICAgICAgICB9IGVsc2Uge1xuICAgICAgICAgIHZhciBwcmV2SW5kZXggPSBhbGxLZXlzLmluZGV4T2YoaGlzdG9yeS5sb2NhdGlvbi5rZXkpO1xuXG4gICAgICAgICAgaWYgKHByZXZJbmRleCAhPT0gLTEpIGFsbEtleXNbcHJldkluZGV4XSA9IGxvY2F0aW9uLmtleTtcblxuICAgICAgICAgIHNldFN0YXRlKHsgYWN0aW9uOiBhY3Rpb24sIGxvY2F0aW9uOiBsb2NhdGlvbiB9KTtcbiAgICAgICAgfVxuICAgICAgfSBlbHNlIHtcbiAgICAgICAgKDAsIF93YXJuaW5nMi5kZWZhdWx0KShzdGF0ZSA9PT0gdW5kZWZpbmVkLCAnQnJvd3NlciBoaXN0b3J5IGNhbm5vdCByZXBsYWNlIHN0YXRlIGluIGJyb3dzZXJzIHRoYXQgZG8gbm90IHN1cHBvcnQgSFRNTDUgaGlzdG9yeScpO1xuXG4gICAgICAgIHdpbmRvdy5sb2NhdGlvbi5yZXBsYWNlKGhyZWYpO1xuICAgICAgfVxuICAgIH0pO1xuICB9O1xuXG4gIHZhciBnbyA9IGZ1bmN0aW9uIGdvKG4pIHtcbiAgICBnbG9iYWxIaXN0b3J5LmdvKG4pO1xuICB9O1xuXG4gIHZhciBnb0JhY2sgPSBmdW5jdGlvbiBnb0JhY2soKSB7XG4gICAgcmV0dXJuIGdvKC0xKTtcbiAgfTtcblxuICB2YXIgZ29Gb3J3YXJkID0gZnVuY3Rpb24gZ29Gb3J3YXJkKCkge1xuICAgIHJldHVybiBnbygxKTtcbiAgfTtcblxuICB2YXIgbGlzdGVuZXJDb3VudCA9IDA7XG5cbiAgdmFyIGNoZWNrRE9NTGlzdGVuZXJzID0gZnVuY3Rpb24gY2hlY2tET01MaXN0ZW5lcnMoZGVsdGEpIHtcbiAgICBsaXN0ZW5lckNvdW50ICs9IGRlbHRhO1xuXG4gICAgaWYgKGxpc3RlbmVyQ291bnQgPT09IDEpIHtcbiAgICAgICgwLCBfRE9NVXRpbHMuYWRkRXZlbnRMaXN0ZW5lcikod2luZG93LCBQb3BTdGF0ZUV2ZW50LCBoYW5kbGVQb3BTdGF0ZSk7XG5cbiAgICAgIGlmIChuZWVkc0hhc2hDaGFuZ2VMaXN0ZW5lcikgKDAsIF9ET01VdGlscy5hZGRFdmVudExpc3RlbmVyKSh3aW5kb3csIEhhc2hDaGFuZ2VFdmVudCwgaGFuZGxlSGFzaENoYW5nZSk7XG4gICAgfSBlbHNlIGlmIChsaXN0ZW5lckNvdW50ID09PSAwKSB7XG4gICAgICAoMCwgX0RPTVV0aWxzLnJlbW92ZUV2ZW50TGlzdGVuZXIpKHdpbmRvdywgUG9wU3RhdGVFdmVudCwgaGFuZGxlUG9wU3RhdGUpO1xuXG4gICAgICBpZiAobmVlZHNIYXNoQ2hhbmdlTGlzdGVuZXIpICgwLCBfRE9NVXRpbHMucmVtb3ZlRXZlbnRMaXN0ZW5lcikod2luZG93LCBIYXNoQ2hhbmdlRXZlbnQsIGhhbmRsZUhhc2hDaGFuZ2UpO1xuICAgIH1cbiAgfTtcblxuICB2YXIgaXNCbG9ja2VkID0gZmFsc2U7XG5cbiAgdmFyIGJsb2NrID0gZnVuY3Rpb24gYmxvY2soKSB7XG4gICAgdmFyIHByb21wdCA9IGFyZ3VtZW50cy5sZW5ndGggPiAwICYmIGFyZ3VtZW50c1swXSAhPT0gdW5kZWZpbmVkID8gYXJndW1lbnRzWzBdIDogZmFsc2U7XG5cbiAgICB2YXIgdW5ibG9jayA9IHRyYW5zaXRpb25NYW5hZ2VyLnNldFByb21wdChwcm9tcHQpO1xuXG4gICAgaWYgKCFpc0Jsb2NrZWQpIHtcbiAgICAgIGNoZWNrRE9NTGlzdGVuZXJzKDEpO1xuICAgICAgaXNCbG9ja2VkID0gdHJ1ZTtcbiAgICB9XG5cbiAgICByZXR1cm4gZnVuY3Rpb24gKCkge1xuICAgICAgaWYgKGlzQmxvY2tlZCkge1xuICAgICAgICBpc0Jsb2NrZWQgPSBmYWxzZTtcbiAgICAgICAgY2hlY2tET01MaXN0ZW5lcnMoLTEpO1xuICAgICAgfVxuXG4gICAgICByZXR1cm4gdW5ibG9jaygpO1xuICAgIH07XG4gIH07XG5cbiAgdmFyIGxpc3RlbiA9IGZ1bmN0aW9uIGxpc3RlbihsaXN0ZW5lcikge1xuICAgIHZhciB1bmxpc3RlbiA9IHRyYW5zaXRpb25NYW5hZ2VyLmFwcGVuZExpc3RlbmVyKGxpc3RlbmVyKTtcbiAgICBjaGVja0RPTUxpc3RlbmVycygxKTtcblxuICAgIHJldHVybiBmdW5jdGlvbiAoKSB7XG4gICAgICBjaGVja0RPTUxpc3RlbmVycygtMSk7XG4gICAgICB1bmxpc3RlbigpO1xuICAgIH07XG4gIH07XG5cbiAgdmFyIGhpc3RvcnkgPSB7XG4gICAgbGVuZ3RoOiBnbG9iYWxIaXN0b3J5Lmxlbmd0aCxcbiAgICBhY3Rpb246ICdQT1AnLFxuICAgIGxvY2F0aW9uOiBpbml0aWFsTG9jYXRpb24sXG4gICAgY3JlYXRlSHJlZjogY3JlYXRlSHJlZixcbiAgICBwdXNoOiBwdXNoLFxuICAgIHJlcGxhY2U6IHJlcGxhY2UsXG4gICAgZ286IGdvLFxuICAgIGdvQmFjazogZ29CYWNrLFxuICAgIGdvRm9yd2FyZDogZ29Gb3J3YXJkLFxuICAgIGJsb2NrOiBibG9jayxcbiAgICBsaXN0ZW46IGxpc3RlblxuICB9O1xuXG4gIHJldHVybiBoaXN0b3J5O1xufTtcblxuZXhwb3J0cy5kZWZhdWx0ID0gY3JlYXRlQnJvd3Nlckhpc3Rvcnk7IiwiJ3VzZSBzdHJpY3QnO1xuXG5leHBvcnRzLl9fZXNNb2R1bGUgPSB0cnVlO1xuXG52YXIgX3dhcm5pbmcgPSByZXF1aXJlKCd3YXJuaW5nJyk7XG5cbnZhciBfd2FybmluZzIgPSBfaW50ZXJvcFJlcXVpcmVEZWZhdWx0KF93YXJuaW5nKTtcblxuZnVuY3Rpb24gX2ludGVyb3BSZXF1aXJlRGVmYXVsdChvYmopIHsgcmV0dXJuIG9iaiAmJiBvYmouX19lc01vZHVsZSA/IG9iaiA6IHsgZGVmYXVsdDogb2JqIH07IH1cblxudmFyIGNyZWF0ZVRyYW5zaXRpb25NYW5hZ2VyID0gZnVuY3Rpb24gY3JlYXRlVHJhbnNpdGlvbk1hbmFnZXIoKSB7XG4gIHZhciBwcm9tcHQgPSBudWxsO1xuXG4gIHZhciBzZXRQcm9tcHQgPSBmdW5jdGlvbiBzZXRQcm9tcHQobmV4dFByb21wdCkge1xuICAgICgwLCBfd2FybmluZzIuZGVmYXVsdCkocHJvbXB0ID09IG51bGwsICdBIGhpc3Rvcnkgc3VwcG9ydHMgb25seSBvbmUgcHJvbXB0IGF0IGEgdGltZScpO1xuXG4gICAgcHJvbXB0ID0gbmV4dFByb21wdDtcblxuICAgIHJldHVybiBmdW5jdGlvbiAoKSB7XG4gICAgICBpZiAocHJvbXB0ID09PSBuZXh0UHJvbXB0KSBwcm9tcHQgPSBudWxsO1xuICAgIH07XG4gIH07XG5cbiAgdmFyIGNvbmZpcm1UcmFuc2l0aW9uVG8gPSBmdW5jdGlvbiBjb25maXJtVHJhbnNpdGlvblRvKGxvY2F0aW9uLCBhY3Rpb24sIGdldFVzZXJDb25maXJtYXRpb24sIGNhbGxiYWNrKSB7XG4gICAgLy8gVE9ETzogSWYgYW5vdGhlciB0cmFuc2l0aW9uIHN0YXJ0cyB3aGlsZSB3ZSdyZSBzdGlsbCBjb25maXJtaW5nXG4gICAgLy8gdGhlIHByZXZpb3VzIG9uZSwgd2UgbWF5IGVuZCB1cCBpbiBhIHdlaXJkIHN0YXRlLiBGaWd1cmUgb3V0IHRoZVxuICAgIC8vIGJlc3Qgd2F5IHRvIGhhbmRsZSB0aGlzLlxuICAgIGlmIChwcm9tcHQgIT0gbnVsbCkge1xuICAgICAgdmFyIHJlc3VsdCA9IHR5cGVvZiBwcm9tcHQgPT09ICdmdW5jdGlvbicgPyBwcm9tcHQobG9jYXRpb24sIGFjdGlvbikgOiBwcm9tcHQ7XG5cbiAgICAgIGlmICh0eXBlb2YgcmVzdWx0ID09PSAnc3RyaW5nJykge1xuICAgICAgICBpZiAodHlwZW9mIGdldFVzZXJDb25maXJtYXRpb24gPT09ICdmdW5jdGlvbicpIHtcbiAgICAgICAgICBnZXRVc2VyQ29uZmlybWF0aW9uKHJlc3VsdCwgY2FsbGJhY2spO1xuICAgICAgICB9IGVsc2Uge1xuICAgICAgICAgICgwLCBfd2FybmluZzIuZGVmYXVsdCkoZmFsc2UsICdBIGhpc3RvcnkgbmVlZHMgYSBnZXRVc2VyQ29uZmlybWF0aW9uIGZ1bmN0aW9uIGluIG9yZGVyIHRvIHVzZSBhIHByb21wdCBtZXNzYWdlJyk7XG5cbiAgICAgICAgICBjYWxsYmFjayh0cnVlKTtcbiAgICAgICAgfVxuICAgICAgfSBlbHNlIHtcbiAgICAgICAgLy8gUmV0dXJuIGZhbHNlIGZyb20gYSB0cmFuc2l0aW9uIGhvb2sgdG8gY2FuY2VsIHRoZSB0cmFuc2l0aW9uLlxuICAgICAgICBjYWxsYmFjayhyZXN1bHQgIT09IGZhbHNlKTtcbiAgICAgIH1cbiAgICB9IGVsc2Uge1xuICAgICAgY2FsbGJhY2sodHJ1ZSk7XG4gICAgfVxuICB9O1xuXG4gIHZhciBsaXN0ZW5lcnMgPSBbXTtcblxuICB2YXIgYXBwZW5kTGlzdGVuZXIgPSBmdW5jdGlvbiBhcHBlbmRMaXN0ZW5lcihmbikge1xuICAgIHZhciBpc0FjdGl2ZSA9IHRydWU7XG5cbiAgICB2YXIgbGlzdGVuZXIgPSBmdW5jdGlvbiBsaXN0ZW5lcigpIHtcbiAgICAgIGlmIChpc0FjdGl2ZSkgZm4uYXBwbHkodW5kZWZpbmVkLCBhcmd1bWVudHMpO1xuICAgIH07XG5cbiAgICBsaXN0ZW5lcnMucHVzaChsaXN0ZW5lcik7XG5cbiAgICByZXR1cm4gZnVuY3Rpb24gKCkge1xuICAgICAgaXNBY3RpdmUgPSBmYWxzZTtcbiAgICAgIGxpc3RlbmVycyA9IGxpc3RlbmVycy5maWx0ZXIoZnVuY3Rpb24gKGl0ZW0pIHtcbiAgICAgICAgcmV0dXJuIGl0ZW0gIT09IGxpc3RlbmVyO1xuICAgICAgfSk7XG4gICAgfTtcbiAgfTtcblxuICB2YXIgbm90aWZ5TGlzdGVuZXJzID0gZnVuY3Rpb24gbm90aWZ5TGlzdGVuZXJzKCkge1xuICAgIGZvciAodmFyIF9sZW4gPSBhcmd1bWVudHMubGVuZ3RoLCBhcmdzID0gQXJyYXkoX2xlbiksIF9rZXkgPSAwOyBfa2V5IDwgX2xlbjsgX2tleSsrKSB7XG4gICAgICBhcmdzW19rZXldID0gYXJndW1lbnRzW19rZXldO1xuICAgIH1cblxuICAgIGxpc3RlbmVycy5mb3JFYWNoKGZ1bmN0aW9uIChsaXN0ZW5lcikge1xuICAgICAgcmV0dXJuIGxpc3RlbmVyLmFwcGx5KHVuZGVmaW5lZCwgYXJncyk7XG4gICAgfSk7XG4gIH07XG5cbiAgcmV0dXJuIHtcbiAgICBzZXRQcm9tcHQ6IHNldFByb21wdCxcbiAgICBjb25maXJtVHJhbnNpdGlvblRvOiBjb25maXJtVHJhbnNpdGlvblRvLFxuICAgIGFwcGVuZExpc3RlbmVyOiBhcHBlbmRMaXN0ZW5lcixcbiAgICBub3RpZnlMaXN0ZW5lcnM6IG5vdGlmeUxpc3RlbmVyc1xuICB9O1xufTtcblxuZXhwb3J0cy5kZWZhdWx0ID0gY3JlYXRlVHJhbnNpdGlvbk1hbmFnZXI7IiwiLyoqXG4gKiBDb3B5cmlnaHQgKGMpIDIwMTMtcHJlc2VudCwgRmFjZWJvb2ssIEluYy5cbiAqXG4gKiBUaGlzIHNvdXJjZSBjb2RlIGlzIGxpY2Vuc2VkIHVuZGVyIHRoZSBNSVQgbGljZW5zZSBmb3VuZCBpbiB0aGVcbiAqIExJQ0VOU0UgZmlsZSBpbiB0aGUgcm9vdCBkaXJlY3Rvcnkgb2YgdGhpcyBzb3VyY2UgdHJlZS5cbiAqL1xuXG4ndXNlIHN0cmljdCc7XG5cbi8qKlxuICogVXNlIGludmFyaWFudCgpIHRvIGFzc2VydCBzdGF0ZSB3aGljaCB5b3VyIHByb2dyYW0gYXNzdW1lcyB0byBiZSB0cnVlLlxuICpcbiAqIFByb3ZpZGUgc3ByaW50Zi1zdHlsZSBmb3JtYXQgKG9ubHkgJXMgaXMgc3VwcG9ydGVkKSBhbmQgYXJndW1lbnRzXG4gKiB0byBwcm92aWRlIGluZm9ybWF0aW9uIGFib3V0IHdoYXQgYnJva2UgYW5kIHdoYXQgeW91IHdlcmVcbiAqIGV4cGVjdGluZy5cbiAqXG4gKiBUaGUgaW52YXJpYW50IG1lc3NhZ2Ugd2lsbCBiZSBzdHJpcHBlZCBpbiBwcm9kdWN0aW9uLCBidXQgdGhlIGludmFyaWFudFxuICogd2lsbCByZW1haW4gdG8gZW5zdXJlIGxvZ2ljIGRvZXMgbm90IGRpZmZlciBpbiBwcm9kdWN0aW9uLlxuICovXG5cbnZhciBpbnZhcmlhbnQgPSBmdW5jdGlvbihjb25kaXRpb24sIGZvcm1hdCwgYSwgYiwgYywgZCwgZSwgZikge1xuICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgIGlmIChmb3JtYXQgPT09IHVuZGVmaW5lZCkge1xuICAgICAgdGhyb3cgbmV3IEVycm9yKCdpbnZhcmlhbnQgcmVxdWlyZXMgYW4gZXJyb3IgbWVzc2FnZSBhcmd1bWVudCcpO1xuICAgIH1cbiAgfVxuXG4gIGlmICghY29uZGl0aW9uKSB7XG4gICAgdmFyIGVycm9yO1xuICAgIGlmIChmb3JtYXQgPT09IHVuZGVmaW5lZCkge1xuICAgICAgZXJyb3IgPSBuZXcgRXJyb3IoXG4gICAgICAgICdNaW5pZmllZCBleGNlcHRpb24gb2NjdXJyZWQ7IHVzZSB0aGUgbm9uLW1pbmlmaWVkIGRldiBlbnZpcm9ubWVudCAnICtcbiAgICAgICAgJ2ZvciB0aGUgZnVsbCBlcnJvciBtZXNzYWdlIGFuZCBhZGRpdGlvbmFsIGhlbHBmdWwgd2FybmluZ3MuJ1xuICAgICAgKTtcbiAgICB9IGVsc2Uge1xuICAgICAgdmFyIGFyZ3MgPSBbYSwgYiwgYywgZCwgZSwgZl07XG4gICAgICB2YXIgYXJnSW5kZXggPSAwO1xuICAgICAgZXJyb3IgPSBuZXcgRXJyb3IoXG4gICAgICAgIGZvcm1hdC5yZXBsYWNlKC8lcy9nLCBmdW5jdGlvbigpIHsgcmV0dXJuIGFyZ3NbYXJnSW5kZXgrK107IH0pXG4gICAgICApO1xuICAgICAgZXJyb3IubmFtZSA9ICdJbnZhcmlhbnQgVmlvbGF0aW9uJztcbiAgICB9XG5cbiAgICBlcnJvci5mcmFtZXNUb1BvcCA9IDE7IC8vIHdlIGRvbid0IGNhcmUgYWJvdXQgaW52YXJpYW50J3Mgb3duIGZyYW1lXG4gICAgdGhyb3cgZXJyb3I7XG4gIH1cbn07XG5cbm1vZHVsZS5leHBvcnRzID0gaW52YXJpYW50O1xuIiwiLypcbm9iamVjdC1hc3NpZ25cbihjKSBTaW5kcmUgU29yaHVzXG5AbGljZW5zZSBNSVRcbiovXG5cbid1c2Ugc3RyaWN0Jztcbi8qIGVzbGludC1kaXNhYmxlIG5vLXVudXNlZC12YXJzICovXG52YXIgZ2V0T3duUHJvcGVydHlTeW1ib2xzID0gT2JqZWN0LmdldE93blByb3BlcnR5U3ltYm9scztcbnZhciBoYXNPd25Qcm9wZXJ0eSA9IE9iamVjdC5wcm90b3R5cGUuaGFzT3duUHJvcGVydHk7XG52YXIgcHJvcElzRW51bWVyYWJsZSA9IE9iamVjdC5wcm90b3R5cGUucHJvcGVydHlJc0VudW1lcmFibGU7XG5cbmZ1bmN0aW9uIHRvT2JqZWN0KHZhbCkge1xuXHRpZiAodmFsID09PSBudWxsIHx8IHZhbCA9PT0gdW5kZWZpbmVkKSB7XG5cdFx0dGhyb3cgbmV3IFR5cGVFcnJvcignT2JqZWN0LmFzc2lnbiBjYW5ub3QgYmUgY2FsbGVkIHdpdGggbnVsbCBvciB1bmRlZmluZWQnKTtcblx0fVxuXG5cdHJldHVybiBPYmplY3QodmFsKTtcbn1cblxuZnVuY3Rpb24gc2hvdWxkVXNlTmF0aXZlKCkge1xuXHR0cnkge1xuXHRcdGlmICghT2JqZWN0LmFzc2lnbikge1xuXHRcdFx0cmV0dXJuIGZhbHNlO1xuXHRcdH1cblxuXHRcdC8vIERldGVjdCBidWdneSBwcm9wZXJ0eSBlbnVtZXJhdGlvbiBvcmRlciBpbiBvbGRlciBWOCB2ZXJzaW9ucy5cblxuXHRcdC8vIGh0dHBzOi8vYnVncy5jaHJvbWl1bS5vcmcvcC92OC9pc3N1ZXMvZGV0YWlsP2lkPTQxMThcblx0XHR2YXIgdGVzdDEgPSBuZXcgU3RyaW5nKCdhYmMnKTsgIC8vIGVzbGludC1kaXNhYmxlLWxpbmUgbm8tbmV3LXdyYXBwZXJzXG5cdFx0dGVzdDFbNV0gPSAnZGUnO1xuXHRcdGlmIChPYmplY3QuZ2V0T3duUHJvcGVydHlOYW1lcyh0ZXN0MSlbMF0gPT09ICc1Jykge1xuXHRcdFx0cmV0dXJuIGZhbHNlO1xuXHRcdH1cblxuXHRcdC8vIGh0dHBzOi8vYnVncy5jaHJvbWl1bS5vcmcvcC92OC9pc3N1ZXMvZGV0YWlsP2lkPTMwNTZcblx0XHR2YXIgdGVzdDIgPSB7fTtcblx0XHRmb3IgKHZhciBpID0gMDsgaSA8IDEwOyBpKyspIHtcblx0XHRcdHRlc3QyWydfJyArIFN0cmluZy5mcm9tQ2hhckNvZGUoaSldID0gaTtcblx0XHR9XG5cdFx0dmFyIG9yZGVyMiA9IE9iamVjdC5nZXRPd25Qcm9wZXJ0eU5hbWVzKHRlc3QyKS5tYXAoZnVuY3Rpb24gKG4pIHtcblx0XHRcdHJldHVybiB0ZXN0MltuXTtcblx0XHR9KTtcblx0XHRpZiAob3JkZXIyLmpvaW4oJycpICE9PSAnMDEyMzQ1Njc4OScpIHtcblx0XHRcdHJldHVybiBmYWxzZTtcblx0XHR9XG5cblx0XHQvLyBodHRwczovL2J1Z3MuY2hyb21pdW0ub3JnL3AvdjgvaXNzdWVzL2RldGFpbD9pZD0zMDU2XG5cdFx0dmFyIHRlc3QzID0ge307XG5cdFx0J2FiY2RlZmdoaWprbG1ub3BxcnN0Jy5zcGxpdCgnJykuZm9yRWFjaChmdW5jdGlvbiAobGV0dGVyKSB7XG5cdFx0XHR0ZXN0M1tsZXR0ZXJdID0gbGV0dGVyO1xuXHRcdH0pO1xuXHRcdGlmIChPYmplY3Qua2V5cyhPYmplY3QuYXNzaWduKHt9LCB0ZXN0MykpLmpvaW4oJycpICE9PVxuXHRcdFx0XHQnYWJjZGVmZ2hpamtsbW5vcHFyc3QnKSB7XG5cdFx0XHRyZXR1cm4gZmFsc2U7XG5cdFx0fVxuXG5cdFx0cmV0dXJuIHRydWU7XG5cdH0gY2F0Y2ggKGVycikge1xuXHRcdC8vIFdlIGRvbid0IGV4cGVjdCBhbnkgb2YgdGhlIGFib3ZlIHRvIHRocm93LCBidXQgYmV0dGVyIHRvIGJlIHNhZmUuXG5cdFx0cmV0dXJuIGZhbHNlO1xuXHR9XG59XG5cbm1vZHVsZS5leHBvcnRzID0gc2hvdWxkVXNlTmF0aXZlKCkgPyBPYmplY3QuYXNzaWduIDogZnVuY3Rpb24gKHRhcmdldCwgc291cmNlKSB7XG5cdHZhciBmcm9tO1xuXHR2YXIgdG8gPSB0b09iamVjdCh0YXJnZXQpO1xuXHR2YXIgc3ltYm9scztcblxuXHRmb3IgKHZhciBzID0gMTsgcyA8IGFyZ3VtZW50cy5sZW5ndGg7IHMrKykge1xuXHRcdGZyb20gPSBPYmplY3QoYXJndW1lbnRzW3NdKTtcblxuXHRcdGZvciAodmFyIGtleSBpbiBmcm9tKSB7XG5cdFx0XHRpZiAoaGFzT3duUHJvcGVydHkuY2FsbChmcm9tLCBrZXkpKSB7XG5cdFx0XHRcdHRvW2tleV0gPSBmcm9tW2tleV07XG5cdFx0XHR9XG5cdFx0fVxuXG5cdFx0aWYgKGdldE93blByb3BlcnR5U3ltYm9scykge1xuXHRcdFx0c3ltYm9scyA9IGdldE93blByb3BlcnR5U3ltYm9scyhmcm9tKTtcblx0XHRcdGZvciAodmFyIGkgPSAwOyBpIDwgc3ltYm9scy5sZW5ndGg7IGkrKykge1xuXHRcdFx0XHRpZiAocHJvcElzRW51bWVyYWJsZS5jYWxsKGZyb20sIHN5bWJvbHNbaV0pKSB7XG5cdFx0XHRcdFx0dG9bc3ltYm9sc1tpXV0gPSBmcm9tW3N5bWJvbHNbaV1dO1xuXHRcdFx0XHR9XG5cdFx0XHR9XG5cdFx0fVxuXHR9XG5cblx0cmV0dXJuIHRvO1xufTtcbiIsIi8qKlxuICogQ29weXJpZ2h0IChjKSAyMDEzLXByZXNlbnQsIEZhY2Vib29rLCBJbmMuXG4gKlxuICogVGhpcyBzb3VyY2UgY29kZSBpcyBsaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UgZm91bmQgaW4gdGhlXG4gKiBMSUNFTlNFIGZpbGUgaW4gdGhlIHJvb3QgZGlyZWN0b3J5IG9mIHRoaXMgc291cmNlIHRyZWUuXG4gKi9cblxuJ3VzZSBzdHJpY3QnO1xuXG5pZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICB2YXIgaW52YXJpYW50ID0gcmVxdWlyZSgnZmJqcy9saWIvaW52YXJpYW50Jyk7XG4gIHZhciB3YXJuaW5nID0gcmVxdWlyZSgnZmJqcy9saWIvd2FybmluZycpO1xuICB2YXIgUmVhY3RQcm9wVHlwZXNTZWNyZXQgPSByZXF1aXJlKCcuL2xpYi9SZWFjdFByb3BUeXBlc1NlY3JldCcpO1xuICB2YXIgbG9nZ2VkVHlwZUZhaWx1cmVzID0ge307XG59XG5cbi8qKlxuICogQXNzZXJ0IHRoYXQgdGhlIHZhbHVlcyBtYXRjaCB3aXRoIHRoZSB0eXBlIHNwZWNzLlxuICogRXJyb3IgbWVzc2FnZXMgYXJlIG1lbW9yaXplZCBhbmQgd2lsbCBvbmx5IGJlIHNob3duIG9uY2UuXG4gKlxuICogQHBhcmFtIHtvYmplY3R9IHR5cGVTcGVjcyBNYXAgb2YgbmFtZSB0byBhIFJlYWN0UHJvcFR5cGVcbiAqIEBwYXJhbSB7b2JqZWN0fSB2YWx1ZXMgUnVudGltZSB2YWx1ZXMgdGhhdCBuZWVkIHRvIGJlIHR5cGUtY2hlY2tlZFxuICogQHBhcmFtIHtzdHJpbmd9IGxvY2F0aW9uIGUuZy4gXCJwcm9wXCIsIFwiY29udGV4dFwiLCBcImNoaWxkIGNvbnRleHRcIlxuICogQHBhcmFtIHtzdHJpbmd9IGNvbXBvbmVudE5hbWUgTmFtZSBvZiB0aGUgY29tcG9uZW50IGZvciBlcnJvciBtZXNzYWdlcy5cbiAqIEBwYXJhbSB7P0Z1bmN0aW9ufSBnZXRTdGFjayBSZXR1cm5zIHRoZSBjb21wb25lbnQgc3RhY2suXG4gKiBAcHJpdmF0ZVxuICovXG5mdW5jdGlvbiBjaGVja1Byb3BUeXBlcyh0eXBlU3BlY3MsIHZhbHVlcywgbG9jYXRpb24sIGNvbXBvbmVudE5hbWUsIGdldFN0YWNrKSB7XG4gIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgZm9yICh2YXIgdHlwZVNwZWNOYW1lIGluIHR5cGVTcGVjcykge1xuICAgICAgaWYgKHR5cGVTcGVjcy5oYXNPd25Qcm9wZXJ0eSh0eXBlU3BlY05hbWUpKSB7XG4gICAgICAgIHZhciBlcnJvcjtcbiAgICAgICAgLy8gUHJvcCB0eXBlIHZhbGlkYXRpb24gbWF5IHRocm93LiBJbiBjYXNlIHRoZXkgZG8sIHdlIGRvbid0IHdhbnQgdG9cbiAgICAgICAgLy8gZmFpbCB0aGUgcmVuZGVyIHBoYXNlIHdoZXJlIGl0IGRpZG4ndCBmYWlsIGJlZm9yZS4gU28gd2UgbG9nIGl0LlxuICAgICAgICAvLyBBZnRlciB0aGVzZSBoYXZlIGJlZW4gY2xlYW5lZCB1cCwgd2UnbGwgbGV0IHRoZW0gdGhyb3cuXG4gICAgICAgIHRyeSB7XG4gICAgICAgICAgLy8gVGhpcyBpcyBpbnRlbnRpb25hbGx5IGFuIGludmFyaWFudCB0aGF0IGdldHMgY2F1Z2h0LiBJdCdzIHRoZSBzYW1lXG4gICAgICAgICAgLy8gYmVoYXZpb3IgYXMgd2l0aG91dCB0aGlzIHN0YXRlbWVudCBleGNlcHQgd2l0aCBhIGJldHRlciBtZXNzYWdlLlxuICAgICAgICAgIGludmFyaWFudCh0eXBlb2YgdHlwZVNwZWNzW3R5cGVTcGVjTmFtZV0gPT09ICdmdW5jdGlvbicsICclczogJXMgdHlwZSBgJXNgIGlzIGludmFsaWQ7IGl0IG11c3QgYmUgYSBmdW5jdGlvbiwgdXN1YWxseSBmcm9tICcgKyAndGhlIGBwcm9wLXR5cGVzYCBwYWNrYWdlLCBidXQgcmVjZWl2ZWQgYCVzYC4nLCBjb21wb25lbnROYW1lIHx8ICdSZWFjdCBjbGFzcycsIGxvY2F0aW9uLCB0eXBlU3BlY05hbWUsIHR5cGVvZiB0eXBlU3BlY3NbdHlwZVNwZWNOYW1lXSk7XG4gICAgICAgICAgZXJyb3IgPSB0eXBlU3BlY3NbdHlwZVNwZWNOYW1lXSh2YWx1ZXMsIHR5cGVTcGVjTmFtZSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIG51bGwsIFJlYWN0UHJvcFR5cGVzU2VjcmV0KTtcbiAgICAgICAgfSBjYXRjaCAoZXgpIHtcbiAgICAgICAgICBlcnJvciA9IGV4O1xuICAgICAgICB9XG4gICAgICAgIHdhcm5pbmcoIWVycm9yIHx8IGVycm9yIGluc3RhbmNlb2YgRXJyb3IsICclczogdHlwZSBzcGVjaWZpY2F0aW9uIG9mICVzIGAlc2AgaXMgaW52YWxpZDsgdGhlIHR5cGUgY2hlY2tlciAnICsgJ2Z1bmN0aW9uIG11c3QgcmV0dXJuIGBudWxsYCBvciBhbiBgRXJyb3JgIGJ1dCByZXR1cm5lZCBhICVzLiAnICsgJ1lvdSBtYXkgaGF2ZSBmb3Jnb3R0ZW4gdG8gcGFzcyBhbiBhcmd1bWVudCB0byB0aGUgdHlwZSBjaGVja2VyICcgKyAnY3JlYXRvciAoYXJyYXlPZiwgaW5zdGFuY2VPZiwgb2JqZWN0T2YsIG9uZU9mLCBvbmVPZlR5cGUsIGFuZCAnICsgJ3NoYXBlIGFsbCByZXF1aXJlIGFuIGFyZ3VtZW50KS4nLCBjb21wb25lbnROYW1lIHx8ICdSZWFjdCBjbGFzcycsIGxvY2F0aW9uLCB0eXBlU3BlY05hbWUsIHR5cGVvZiBlcnJvcik7XG4gICAgICAgIGlmIChlcnJvciBpbnN0YW5jZW9mIEVycm9yICYmICEoZXJyb3IubWVzc2FnZSBpbiBsb2dnZWRUeXBlRmFpbHVyZXMpKSB7XG4gICAgICAgICAgLy8gT25seSBtb25pdG9yIHRoaXMgZmFpbHVyZSBvbmNlIGJlY2F1c2UgdGhlcmUgdGVuZHMgdG8gYmUgYSBsb3Qgb2YgdGhlXG4gICAgICAgICAgLy8gc2FtZSBlcnJvci5cbiAgICAgICAgICBsb2dnZWRUeXBlRmFpbHVyZXNbZXJyb3IubWVzc2FnZV0gPSB0cnVlO1xuXG4gICAgICAgICAgdmFyIHN0YWNrID0gZ2V0U3RhY2sgPyBnZXRTdGFjaygpIDogJyc7XG5cbiAgICAgICAgICB3YXJuaW5nKGZhbHNlLCAnRmFpbGVkICVzIHR5cGU6ICVzJXMnLCBsb2NhdGlvbiwgZXJyb3IubWVzc2FnZSwgc3RhY2sgIT0gbnVsbCA/IHN0YWNrIDogJycpO1xuICAgICAgICB9XG4gICAgICB9XG4gICAgfVxuICB9XG59XG5cbm1vZHVsZS5leHBvcnRzID0gY2hlY2tQcm9wVHlwZXM7XG4iLCIvKipcbiAqIENvcHlyaWdodCAoYykgMjAxMy1wcmVzZW50LCBGYWNlYm9vaywgSW5jLlxuICpcbiAqIFRoaXMgc291cmNlIGNvZGUgaXMgbGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlIGZvdW5kIGluIHRoZVxuICogTElDRU5TRSBmaWxlIGluIHRoZSByb290IGRpcmVjdG9yeSBvZiB0aGlzIHNvdXJjZSB0cmVlLlxuICovXG5cbid1c2Ugc3RyaWN0JztcblxudmFyIGVtcHR5RnVuY3Rpb24gPSByZXF1aXJlKCdmYmpzL2xpYi9lbXB0eUZ1bmN0aW9uJyk7XG52YXIgaW52YXJpYW50ID0gcmVxdWlyZSgnZmJqcy9saWIvaW52YXJpYW50Jyk7XG52YXIgd2FybmluZyA9IHJlcXVpcmUoJ2ZianMvbGliL3dhcm5pbmcnKTtcbnZhciBhc3NpZ24gPSByZXF1aXJlKCdvYmplY3QtYXNzaWduJyk7XG5cbnZhciBSZWFjdFByb3BUeXBlc1NlY3JldCA9IHJlcXVpcmUoJy4vbGliL1JlYWN0UHJvcFR5cGVzU2VjcmV0Jyk7XG52YXIgY2hlY2tQcm9wVHlwZXMgPSByZXF1aXJlKCcuL2NoZWNrUHJvcFR5cGVzJyk7XG5cbm1vZHVsZS5leHBvcnRzID0gZnVuY3Rpb24oaXNWYWxpZEVsZW1lbnQsIHRocm93T25EaXJlY3RBY2Nlc3MpIHtcbiAgLyogZ2xvYmFsIFN5bWJvbCAqL1xuICB2YXIgSVRFUkFUT1JfU1lNQk9MID0gdHlwZW9mIFN5bWJvbCA9PT0gJ2Z1bmN0aW9uJyAmJiBTeW1ib2wuaXRlcmF0b3I7XG4gIHZhciBGQVVYX0lURVJBVE9SX1NZTUJPTCA9ICdAQGl0ZXJhdG9yJzsgLy8gQmVmb3JlIFN5bWJvbCBzcGVjLlxuXG4gIC8qKlxuICAgKiBSZXR1cm5zIHRoZSBpdGVyYXRvciBtZXRob2QgZnVuY3Rpb24gY29udGFpbmVkIG9uIHRoZSBpdGVyYWJsZSBvYmplY3QuXG4gICAqXG4gICAqIEJlIHN1cmUgdG8gaW52b2tlIHRoZSBmdW5jdGlvbiB3aXRoIHRoZSBpdGVyYWJsZSBhcyBjb250ZXh0OlxuICAgKlxuICAgKiAgICAgdmFyIGl0ZXJhdG9yRm4gPSBnZXRJdGVyYXRvckZuKG15SXRlcmFibGUpO1xuICAgKiAgICAgaWYgKGl0ZXJhdG9yRm4pIHtcbiAgICogICAgICAgdmFyIGl0ZXJhdG9yID0gaXRlcmF0b3JGbi5jYWxsKG15SXRlcmFibGUpO1xuICAgKiAgICAgICAuLi5cbiAgICogICAgIH1cbiAgICpcbiAgICogQHBhcmFtIHs/b2JqZWN0fSBtYXliZUl0ZXJhYmxlXG4gICAqIEByZXR1cm4gez9mdW5jdGlvbn1cbiAgICovXG4gIGZ1bmN0aW9uIGdldEl0ZXJhdG9yRm4obWF5YmVJdGVyYWJsZSkge1xuICAgIHZhciBpdGVyYXRvckZuID0gbWF5YmVJdGVyYWJsZSAmJiAoSVRFUkFUT1JfU1lNQk9MICYmIG1heWJlSXRlcmFibGVbSVRFUkFUT1JfU1lNQk9MXSB8fCBtYXliZUl0ZXJhYmxlW0ZBVVhfSVRFUkFUT1JfU1lNQk9MXSk7XG4gICAgaWYgKHR5cGVvZiBpdGVyYXRvckZuID09PSAnZnVuY3Rpb24nKSB7XG4gICAgICByZXR1cm4gaXRlcmF0b3JGbjtcbiAgICB9XG4gIH1cblxuICAvKipcbiAgICogQ29sbGVjdGlvbiBvZiBtZXRob2RzIHRoYXQgYWxsb3cgZGVjbGFyYXRpb24gYW5kIHZhbGlkYXRpb24gb2YgcHJvcHMgdGhhdCBhcmVcbiAgICogc3VwcGxpZWQgdG8gUmVhY3QgY29tcG9uZW50cy4gRXhhbXBsZSB1c2FnZTpcbiAgICpcbiAgICogICB2YXIgUHJvcHMgPSByZXF1aXJlKCdSZWFjdFByb3BUeXBlcycpO1xuICAgKiAgIHZhciBNeUFydGljbGUgPSBSZWFjdC5jcmVhdGVDbGFzcyh7XG4gICAqICAgICBwcm9wVHlwZXM6IHtcbiAgICogICAgICAgLy8gQW4gb3B0aW9uYWwgc3RyaW5nIHByb3AgbmFtZWQgXCJkZXNjcmlwdGlvblwiLlxuICAgKiAgICAgICBkZXNjcmlwdGlvbjogUHJvcHMuc3RyaW5nLFxuICAgKlxuICAgKiAgICAgICAvLyBBIHJlcXVpcmVkIGVudW0gcHJvcCBuYW1lZCBcImNhdGVnb3J5XCIuXG4gICAqICAgICAgIGNhdGVnb3J5OiBQcm9wcy5vbmVPZihbJ05ld3MnLCdQaG90b3MnXSkuaXNSZXF1aXJlZCxcbiAgICpcbiAgICogICAgICAgLy8gQSBwcm9wIG5hbWVkIFwiZGlhbG9nXCIgdGhhdCByZXF1aXJlcyBhbiBpbnN0YW5jZSBvZiBEaWFsb2cuXG4gICAqICAgICAgIGRpYWxvZzogUHJvcHMuaW5zdGFuY2VPZihEaWFsb2cpLmlzUmVxdWlyZWRcbiAgICogICAgIH0sXG4gICAqICAgICByZW5kZXI6IGZ1bmN0aW9uKCkgeyAuLi4gfVxuICAgKiAgIH0pO1xuICAgKlxuICAgKiBBIG1vcmUgZm9ybWFsIHNwZWNpZmljYXRpb24gb2YgaG93IHRoZXNlIG1ldGhvZHMgYXJlIHVzZWQ6XG4gICAqXG4gICAqICAgdHlwZSA6PSBhcnJheXxib29sfGZ1bmN8b2JqZWN0fG51bWJlcnxzdHJpbmd8b25lT2YoWy4uLl0pfGluc3RhbmNlT2YoLi4uKVxuICAgKiAgIGRlY2wgOj0gUmVhY3RQcm9wVHlwZXMue3R5cGV9KC5pc1JlcXVpcmVkKT9cbiAgICpcbiAgICogRWFjaCBhbmQgZXZlcnkgZGVjbGFyYXRpb24gcHJvZHVjZXMgYSBmdW5jdGlvbiB3aXRoIHRoZSBzYW1lIHNpZ25hdHVyZS4gVGhpc1xuICAgKiBhbGxvd3MgdGhlIGNyZWF0aW9uIG9mIGN1c3RvbSB2YWxpZGF0aW9uIGZ1bmN0aW9ucy4gRm9yIGV4YW1wbGU6XG4gICAqXG4gICAqICB2YXIgTXlMaW5rID0gUmVhY3QuY3JlYXRlQ2xhc3Moe1xuICAgKiAgICBwcm9wVHlwZXM6IHtcbiAgICogICAgICAvLyBBbiBvcHRpb25hbCBzdHJpbmcgb3IgVVJJIHByb3AgbmFtZWQgXCJocmVmXCIuXG4gICAqICAgICAgaHJlZjogZnVuY3Rpb24ocHJvcHMsIHByb3BOYW1lLCBjb21wb25lbnROYW1lKSB7XG4gICAqICAgICAgICB2YXIgcHJvcFZhbHVlID0gcHJvcHNbcHJvcE5hbWVdO1xuICAgKiAgICAgICAgaWYgKHByb3BWYWx1ZSAhPSBudWxsICYmIHR5cGVvZiBwcm9wVmFsdWUgIT09ICdzdHJpbmcnICYmXG4gICAqICAgICAgICAgICAgIShwcm9wVmFsdWUgaW5zdGFuY2VvZiBVUkkpKSB7XG4gICAqICAgICAgICAgIHJldHVybiBuZXcgRXJyb3IoXG4gICAqICAgICAgICAgICAgJ0V4cGVjdGVkIGEgc3RyaW5nIG9yIGFuIFVSSSBmb3IgJyArIHByb3BOYW1lICsgJyBpbiAnICtcbiAgICogICAgICAgICAgICBjb21wb25lbnROYW1lXG4gICAqICAgICAgICAgICk7XG4gICAqICAgICAgICB9XG4gICAqICAgICAgfVxuICAgKiAgICB9LFxuICAgKiAgICByZW5kZXI6IGZ1bmN0aW9uKCkgey4uLn1cbiAgICogIH0pO1xuICAgKlxuICAgKiBAaW50ZXJuYWxcbiAgICovXG5cbiAgdmFyIEFOT05ZTU9VUyA9ICc8PGFub255bW91cz4+JztcblxuICAvLyBJbXBvcnRhbnQhXG4gIC8vIEtlZXAgdGhpcyBsaXN0IGluIHN5bmMgd2l0aCBwcm9kdWN0aW9uIHZlcnNpb24gaW4gYC4vZmFjdG9yeVdpdGhUaHJvd2luZ1NoaW1zLmpzYC5cbiAgdmFyIFJlYWN0UHJvcFR5cGVzID0ge1xuICAgIGFycmF5OiBjcmVhdGVQcmltaXRpdmVUeXBlQ2hlY2tlcignYXJyYXknKSxcbiAgICBib29sOiBjcmVhdGVQcmltaXRpdmVUeXBlQ2hlY2tlcignYm9vbGVhbicpLFxuICAgIGZ1bmM6IGNyZWF0ZVByaW1pdGl2ZVR5cGVDaGVja2VyKCdmdW5jdGlvbicpLFxuICAgIG51bWJlcjogY3JlYXRlUHJpbWl0aXZlVHlwZUNoZWNrZXIoJ251bWJlcicpLFxuICAgIG9iamVjdDogY3JlYXRlUHJpbWl0aXZlVHlwZUNoZWNrZXIoJ29iamVjdCcpLFxuICAgIHN0cmluZzogY3JlYXRlUHJpbWl0aXZlVHlwZUNoZWNrZXIoJ3N0cmluZycpLFxuICAgIHN5bWJvbDogY3JlYXRlUHJpbWl0aXZlVHlwZUNoZWNrZXIoJ3N5bWJvbCcpLFxuXG4gICAgYW55OiBjcmVhdGVBbnlUeXBlQ2hlY2tlcigpLFxuICAgIGFycmF5T2Y6IGNyZWF0ZUFycmF5T2ZUeXBlQ2hlY2tlcixcbiAgICBlbGVtZW50OiBjcmVhdGVFbGVtZW50VHlwZUNoZWNrZXIoKSxcbiAgICBpbnN0YW5jZU9mOiBjcmVhdGVJbnN0YW5jZVR5cGVDaGVja2VyLFxuICAgIG5vZGU6IGNyZWF0ZU5vZGVDaGVja2VyKCksXG4gICAgb2JqZWN0T2Y6IGNyZWF0ZU9iamVjdE9mVHlwZUNoZWNrZXIsXG4gICAgb25lT2Y6IGNyZWF0ZUVudW1UeXBlQ2hlY2tlcixcbiAgICBvbmVPZlR5cGU6IGNyZWF0ZVVuaW9uVHlwZUNoZWNrZXIsXG4gICAgc2hhcGU6IGNyZWF0ZVNoYXBlVHlwZUNoZWNrZXIsXG4gICAgZXhhY3Q6IGNyZWF0ZVN0cmljdFNoYXBlVHlwZUNoZWNrZXIsXG4gIH07XG5cbiAgLyoqXG4gICAqIGlubGluZWQgT2JqZWN0LmlzIHBvbHlmaWxsIHRvIGF2b2lkIHJlcXVpcmluZyBjb25zdW1lcnMgc2hpcCB0aGVpciBvd25cbiAgICogaHR0cHM6Ly9kZXZlbG9wZXIubW96aWxsYS5vcmcvZW4tVVMvZG9jcy9XZWIvSmF2YVNjcmlwdC9SZWZlcmVuY2UvR2xvYmFsX09iamVjdHMvT2JqZWN0L2lzXG4gICAqL1xuICAvKmVzbGludC1kaXNhYmxlIG5vLXNlbGYtY29tcGFyZSovXG4gIGZ1bmN0aW9uIGlzKHgsIHkpIHtcbiAgICAvLyBTYW1lVmFsdWUgYWxnb3JpdGhtXG4gICAgaWYgKHggPT09IHkpIHtcbiAgICAgIC8vIFN0ZXBzIDEtNSwgNy0xMFxuICAgICAgLy8gU3RlcHMgNi5iLTYuZTogKzAgIT0gLTBcbiAgICAgIHJldHVybiB4ICE9PSAwIHx8IDEgLyB4ID09PSAxIC8geTtcbiAgICB9IGVsc2Uge1xuICAgICAgLy8gU3RlcCA2LmE6IE5hTiA9PSBOYU5cbiAgICAgIHJldHVybiB4ICE9PSB4ICYmIHkgIT09IHk7XG4gICAgfVxuICB9XG4gIC8qZXNsaW50LWVuYWJsZSBuby1zZWxmLWNvbXBhcmUqL1xuXG4gIC8qKlxuICAgKiBXZSB1c2UgYW4gRXJyb3ItbGlrZSBvYmplY3QgZm9yIGJhY2t3YXJkIGNvbXBhdGliaWxpdHkgYXMgcGVvcGxlIG1heSBjYWxsXG4gICAqIFByb3BUeXBlcyBkaXJlY3RseSBhbmQgaW5zcGVjdCB0aGVpciBvdXRwdXQuIEhvd2V2ZXIsIHdlIGRvbid0IHVzZSByZWFsXG4gICAqIEVycm9ycyBhbnltb3JlLiBXZSBkb24ndCBpbnNwZWN0IHRoZWlyIHN0YWNrIGFueXdheSwgYW5kIGNyZWF0aW5nIHRoZW1cbiAgICogaXMgcHJvaGliaXRpdmVseSBleHBlbnNpdmUgaWYgdGhleSBhcmUgY3JlYXRlZCB0b28gb2Z0ZW4sIHN1Y2ggYXMgd2hhdFxuICAgKiBoYXBwZW5zIGluIG9uZU9mVHlwZSgpIGZvciBhbnkgdHlwZSBiZWZvcmUgdGhlIG9uZSB0aGF0IG1hdGNoZWQuXG4gICAqL1xuICBmdW5jdGlvbiBQcm9wVHlwZUVycm9yKG1lc3NhZ2UpIHtcbiAgICB0aGlzLm1lc3NhZ2UgPSBtZXNzYWdlO1xuICAgIHRoaXMuc3RhY2sgPSAnJztcbiAgfVxuICAvLyBNYWtlIGBpbnN0YW5jZW9mIEVycm9yYCBzdGlsbCB3b3JrIGZvciByZXR1cm5lZCBlcnJvcnMuXG4gIFByb3BUeXBlRXJyb3IucHJvdG90eXBlID0gRXJyb3IucHJvdG90eXBlO1xuXG4gIGZ1bmN0aW9uIGNyZWF0ZUNoYWluYWJsZVR5cGVDaGVja2VyKHZhbGlkYXRlKSB7XG4gICAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICAgIHZhciBtYW51YWxQcm9wVHlwZUNhbGxDYWNoZSA9IHt9O1xuICAgICAgdmFyIG1hbnVhbFByb3BUeXBlV2FybmluZ0NvdW50ID0gMDtcbiAgICB9XG4gICAgZnVuY3Rpb24gY2hlY2tUeXBlKGlzUmVxdWlyZWQsIHByb3BzLCBwcm9wTmFtZSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSwgc2VjcmV0KSB7XG4gICAgICBjb21wb25lbnROYW1lID0gY29tcG9uZW50TmFtZSB8fCBBTk9OWU1PVVM7XG4gICAgICBwcm9wRnVsbE5hbWUgPSBwcm9wRnVsbE5hbWUgfHwgcHJvcE5hbWU7XG5cbiAgICAgIGlmIChzZWNyZXQgIT09IFJlYWN0UHJvcFR5cGVzU2VjcmV0KSB7XG4gICAgICAgIGlmICh0aHJvd09uRGlyZWN0QWNjZXNzKSB7XG4gICAgICAgICAgLy8gTmV3IGJlaGF2aW9yIG9ubHkgZm9yIHVzZXJzIG9mIGBwcm9wLXR5cGVzYCBwYWNrYWdlXG4gICAgICAgICAgaW52YXJpYW50KFxuICAgICAgICAgICAgZmFsc2UsXG4gICAgICAgICAgICAnQ2FsbGluZyBQcm9wVHlwZXMgdmFsaWRhdG9ycyBkaXJlY3RseSBpcyBub3Qgc3VwcG9ydGVkIGJ5IHRoZSBgcHJvcC10eXBlc2AgcGFja2FnZS4gJyArXG4gICAgICAgICAgICAnVXNlIGBQcm9wVHlwZXMuY2hlY2tQcm9wVHlwZXMoKWAgdG8gY2FsbCB0aGVtLiAnICtcbiAgICAgICAgICAgICdSZWFkIG1vcmUgYXQgaHR0cDovL2ZiLm1lL3VzZS1jaGVjay1wcm9wLXR5cGVzJ1xuICAgICAgICAgICk7XG4gICAgICAgIH0gZWxzZSBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJyAmJiB0eXBlb2YgY29uc29sZSAhPT0gJ3VuZGVmaW5lZCcpIHtcbiAgICAgICAgICAvLyBPbGQgYmVoYXZpb3IgZm9yIHBlb3BsZSB1c2luZyBSZWFjdC5Qcm9wVHlwZXNcbiAgICAgICAgICB2YXIgY2FjaGVLZXkgPSBjb21wb25lbnROYW1lICsgJzonICsgcHJvcE5hbWU7XG4gICAgICAgICAgaWYgKFxuICAgICAgICAgICAgIW1hbnVhbFByb3BUeXBlQ2FsbENhY2hlW2NhY2hlS2V5XSAmJlxuICAgICAgICAgICAgLy8gQXZvaWQgc3BhbW1pbmcgdGhlIGNvbnNvbGUgYmVjYXVzZSB0aGV5IGFyZSBvZnRlbiBub3QgYWN0aW9uYWJsZSBleGNlcHQgZm9yIGxpYiBhdXRob3JzXG4gICAgICAgICAgICBtYW51YWxQcm9wVHlwZVdhcm5pbmdDb3VudCA8IDNcbiAgICAgICAgICApIHtcbiAgICAgICAgICAgIHdhcm5pbmcoXG4gICAgICAgICAgICAgIGZhbHNlLFxuICAgICAgICAgICAgICAnWW91IGFyZSBtYW51YWxseSBjYWxsaW5nIGEgUmVhY3QuUHJvcFR5cGVzIHZhbGlkYXRpb24gJyArXG4gICAgICAgICAgICAgICdmdW5jdGlvbiBmb3IgdGhlIGAlc2AgcHJvcCBvbiBgJXNgLiBUaGlzIGlzIGRlcHJlY2F0ZWQgJyArXG4gICAgICAgICAgICAgICdhbmQgd2lsbCB0aHJvdyBpbiB0aGUgc3RhbmRhbG9uZSBgcHJvcC10eXBlc2AgcGFja2FnZS4gJyArXG4gICAgICAgICAgICAgICdZb3UgbWF5IGJlIHNlZWluZyB0aGlzIHdhcm5pbmcgZHVlIHRvIGEgdGhpcmQtcGFydHkgUHJvcFR5cGVzICcgK1xuICAgICAgICAgICAgICAnbGlicmFyeS4gU2VlIGh0dHBzOi8vZmIubWUvcmVhY3Qtd2FybmluZy1kb250LWNhbGwtcHJvcHR5cGVzICcgKyAnZm9yIGRldGFpbHMuJyxcbiAgICAgICAgICAgICAgcHJvcEZ1bGxOYW1lLFxuICAgICAgICAgICAgICBjb21wb25lbnROYW1lXG4gICAgICAgICAgICApO1xuICAgICAgICAgICAgbWFudWFsUHJvcFR5cGVDYWxsQ2FjaGVbY2FjaGVLZXldID0gdHJ1ZTtcbiAgICAgICAgICAgIG1hbnVhbFByb3BUeXBlV2FybmluZ0NvdW50Kys7XG4gICAgICAgICAgfVxuICAgICAgICB9XG4gICAgICB9XG4gICAgICBpZiAocHJvcHNbcHJvcE5hbWVdID09IG51bGwpIHtcbiAgICAgICAgaWYgKGlzUmVxdWlyZWQpIHtcbiAgICAgICAgICBpZiAocHJvcHNbcHJvcE5hbWVdID09PSBudWxsKSB7XG4gICAgICAgICAgICByZXR1cm4gbmV3IFByb3BUeXBlRXJyb3IoJ1RoZSAnICsgbG9jYXRpb24gKyAnIGAnICsgcHJvcEZ1bGxOYW1lICsgJ2AgaXMgbWFya2VkIGFzIHJlcXVpcmVkICcgKyAoJ2luIGAnICsgY29tcG9uZW50TmFtZSArICdgLCBidXQgaXRzIHZhbHVlIGlzIGBudWxsYC4nKSk7XG4gICAgICAgICAgfVxuICAgICAgICAgIHJldHVybiBuZXcgUHJvcFR5cGVFcnJvcignVGhlICcgKyBsb2NhdGlvbiArICcgYCcgKyBwcm9wRnVsbE5hbWUgKyAnYCBpcyBtYXJrZWQgYXMgcmVxdWlyZWQgaW4gJyArICgnYCcgKyBjb21wb25lbnROYW1lICsgJ2AsIGJ1dCBpdHMgdmFsdWUgaXMgYHVuZGVmaW5lZGAuJykpO1xuICAgICAgICB9XG4gICAgICAgIHJldHVybiBudWxsO1xuICAgICAgfSBlbHNlIHtcbiAgICAgICAgcmV0dXJuIHZhbGlkYXRlKHByb3BzLCBwcm9wTmFtZSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSk7XG4gICAgICB9XG4gICAgfVxuXG4gICAgdmFyIGNoYWluZWRDaGVja1R5cGUgPSBjaGVja1R5cGUuYmluZChudWxsLCBmYWxzZSk7XG4gICAgY2hhaW5lZENoZWNrVHlwZS5pc1JlcXVpcmVkID0gY2hlY2tUeXBlLmJpbmQobnVsbCwgdHJ1ZSk7XG5cbiAgICByZXR1cm4gY2hhaW5lZENoZWNrVHlwZTtcbiAgfVxuXG4gIGZ1bmN0aW9uIGNyZWF0ZVByaW1pdGl2ZVR5cGVDaGVja2VyKGV4cGVjdGVkVHlwZSkge1xuICAgIGZ1bmN0aW9uIHZhbGlkYXRlKHByb3BzLCBwcm9wTmFtZSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSwgc2VjcmV0KSB7XG4gICAgICB2YXIgcHJvcFZhbHVlID0gcHJvcHNbcHJvcE5hbWVdO1xuICAgICAgdmFyIHByb3BUeXBlID0gZ2V0UHJvcFR5cGUocHJvcFZhbHVlKTtcbiAgICAgIGlmIChwcm9wVHlwZSAhPT0gZXhwZWN0ZWRUeXBlKSB7XG4gICAgICAgIC8vIGBwcm9wVmFsdWVgIGJlaW5nIGluc3RhbmNlIG9mLCBzYXksIGRhdGUvcmVnZXhwLCBwYXNzIHRoZSAnb2JqZWN0J1xuICAgICAgICAvLyBjaGVjaywgYnV0IHdlIGNhbiBvZmZlciBhIG1vcmUgcHJlY2lzZSBlcnJvciBtZXNzYWdlIGhlcmUgcmF0aGVyIHRoYW5cbiAgICAgICAgLy8gJ29mIHR5cGUgYG9iamVjdGAnLlxuICAgICAgICB2YXIgcHJlY2lzZVR5cGUgPSBnZXRQcmVjaXNlVHlwZShwcm9wVmFsdWUpO1xuXG4gICAgICAgIHJldHVybiBuZXcgUHJvcFR5cGVFcnJvcignSW52YWxpZCAnICsgbG9jYXRpb24gKyAnIGAnICsgcHJvcEZ1bGxOYW1lICsgJ2Agb2YgdHlwZSAnICsgKCdgJyArIHByZWNpc2VUeXBlICsgJ2Agc3VwcGxpZWQgdG8gYCcgKyBjb21wb25lbnROYW1lICsgJ2AsIGV4cGVjdGVkICcpICsgKCdgJyArIGV4cGVjdGVkVHlwZSArICdgLicpKTtcbiAgICAgIH1cbiAgICAgIHJldHVybiBudWxsO1xuICAgIH1cbiAgICByZXR1cm4gY3JlYXRlQ2hhaW5hYmxlVHlwZUNoZWNrZXIodmFsaWRhdGUpO1xuICB9XG5cbiAgZnVuY3Rpb24gY3JlYXRlQW55VHlwZUNoZWNrZXIoKSB7XG4gICAgcmV0dXJuIGNyZWF0ZUNoYWluYWJsZVR5cGVDaGVja2VyKGVtcHR5RnVuY3Rpb24udGhhdFJldHVybnNOdWxsKTtcbiAgfVxuXG4gIGZ1bmN0aW9uIGNyZWF0ZUFycmF5T2ZUeXBlQ2hlY2tlcih0eXBlQ2hlY2tlcikge1xuICAgIGZ1bmN0aW9uIHZhbGlkYXRlKHByb3BzLCBwcm9wTmFtZSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSkge1xuICAgICAgaWYgKHR5cGVvZiB0eXBlQ2hlY2tlciAhPT0gJ2Z1bmN0aW9uJykge1xuICAgICAgICByZXR1cm4gbmV3IFByb3BUeXBlRXJyb3IoJ1Byb3BlcnR5IGAnICsgcHJvcEZ1bGxOYW1lICsgJ2Agb2YgY29tcG9uZW50IGAnICsgY29tcG9uZW50TmFtZSArICdgIGhhcyBpbnZhbGlkIFByb3BUeXBlIG5vdGF0aW9uIGluc2lkZSBhcnJheU9mLicpO1xuICAgICAgfVxuICAgICAgdmFyIHByb3BWYWx1ZSA9IHByb3BzW3Byb3BOYW1lXTtcbiAgICAgIGlmICghQXJyYXkuaXNBcnJheShwcm9wVmFsdWUpKSB7XG4gICAgICAgIHZhciBwcm9wVHlwZSA9IGdldFByb3BUeXBlKHByb3BWYWx1ZSk7XG4gICAgICAgIHJldHVybiBuZXcgUHJvcFR5cGVFcnJvcignSW52YWxpZCAnICsgbG9jYXRpb24gKyAnIGAnICsgcHJvcEZ1bGxOYW1lICsgJ2Agb2YgdHlwZSAnICsgKCdgJyArIHByb3BUeXBlICsgJ2Agc3VwcGxpZWQgdG8gYCcgKyBjb21wb25lbnROYW1lICsgJ2AsIGV4cGVjdGVkIGFuIGFycmF5LicpKTtcbiAgICAgIH1cbiAgICAgIGZvciAodmFyIGkgPSAwOyBpIDwgcHJvcFZhbHVlLmxlbmd0aDsgaSsrKSB7XG4gICAgICAgIHZhciBlcnJvciA9IHR5cGVDaGVja2VyKHByb3BWYWx1ZSwgaSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSArICdbJyArIGkgKyAnXScsIFJlYWN0UHJvcFR5cGVzU2VjcmV0KTtcbiAgICAgICAgaWYgKGVycm9yIGluc3RhbmNlb2YgRXJyb3IpIHtcbiAgICAgICAgICByZXR1cm4gZXJyb3I7XG4gICAgICAgIH1cbiAgICAgIH1cbiAgICAgIHJldHVybiBudWxsO1xuICAgIH1cbiAgICByZXR1cm4gY3JlYXRlQ2hhaW5hYmxlVHlwZUNoZWNrZXIodmFsaWRhdGUpO1xuICB9XG5cbiAgZnVuY3Rpb24gY3JlYXRlRWxlbWVudFR5cGVDaGVja2VyKCkge1xuICAgIGZ1bmN0aW9uIHZhbGlkYXRlKHByb3BzLCBwcm9wTmFtZSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSkge1xuICAgICAgdmFyIHByb3BWYWx1ZSA9IHByb3BzW3Byb3BOYW1lXTtcbiAgICAgIGlmICghaXNWYWxpZEVsZW1lbnQocHJvcFZhbHVlKSkge1xuICAgICAgICB2YXIgcHJvcFR5cGUgPSBnZXRQcm9wVHlwZShwcm9wVmFsdWUpO1xuICAgICAgICByZXR1cm4gbmV3IFByb3BUeXBlRXJyb3IoJ0ludmFsaWQgJyArIGxvY2F0aW9uICsgJyBgJyArIHByb3BGdWxsTmFtZSArICdgIG9mIHR5cGUgJyArICgnYCcgKyBwcm9wVHlwZSArICdgIHN1cHBsaWVkIHRvIGAnICsgY29tcG9uZW50TmFtZSArICdgLCBleHBlY3RlZCBhIHNpbmdsZSBSZWFjdEVsZW1lbnQuJykpO1xuICAgICAgfVxuICAgICAgcmV0dXJuIG51bGw7XG4gICAgfVxuICAgIHJldHVybiBjcmVhdGVDaGFpbmFibGVUeXBlQ2hlY2tlcih2YWxpZGF0ZSk7XG4gIH1cblxuICBmdW5jdGlvbiBjcmVhdGVJbnN0YW5jZVR5cGVDaGVja2VyKGV4cGVjdGVkQ2xhc3MpIHtcbiAgICBmdW5jdGlvbiB2YWxpZGF0ZShwcm9wcywgcHJvcE5hbWUsIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUpIHtcbiAgICAgIGlmICghKHByb3BzW3Byb3BOYW1lXSBpbnN0YW5jZW9mIGV4cGVjdGVkQ2xhc3MpKSB7XG4gICAgICAgIHZhciBleHBlY3RlZENsYXNzTmFtZSA9IGV4cGVjdGVkQ2xhc3MubmFtZSB8fCBBTk9OWU1PVVM7XG4gICAgICAgIHZhciBhY3R1YWxDbGFzc05hbWUgPSBnZXRDbGFzc05hbWUocHJvcHNbcHJvcE5hbWVdKTtcbiAgICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKCdJbnZhbGlkICcgKyBsb2NhdGlvbiArICcgYCcgKyBwcm9wRnVsbE5hbWUgKyAnYCBvZiB0eXBlICcgKyAoJ2AnICsgYWN0dWFsQ2xhc3NOYW1lICsgJ2Agc3VwcGxpZWQgdG8gYCcgKyBjb21wb25lbnROYW1lICsgJ2AsIGV4cGVjdGVkICcpICsgKCdpbnN0YW5jZSBvZiBgJyArIGV4cGVjdGVkQ2xhc3NOYW1lICsgJ2AuJykpO1xuICAgICAgfVxuICAgICAgcmV0dXJuIG51bGw7XG4gICAgfVxuICAgIHJldHVybiBjcmVhdGVDaGFpbmFibGVUeXBlQ2hlY2tlcih2YWxpZGF0ZSk7XG4gIH1cblxuICBmdW5jdGlvbiBjcmVhdGVFbnVtVHlwZUNoZWNrZXIoZXhwZWN0ZWRWYWx1ZXMpIHtcbiAgICBpZiAoIUFycmF5LmlzQXJyYXkoZXhwZWN0ZWRWYWx1ZXMpKSB7XG4gICAgICBwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nID8gd2FybmluZyhmYWxzZSwgJ0ludmFsaWQgYXJndW1lbnQgc3VwcGxpZWQgdG8gb25lT2YsIGV4cGVjdGVkIGFuIGluc3RhbmNlIG9mIGFycmF5LicpIDogdm9pZCAwO1xuICAgICAgcmV0dXJuIGVtcHR5RnVuY3Rpb24udGhhdFJldHVybnNOdWxsO1xuICAgIH1cblxuICAgIGZ1bmN0aW9uIHZhbGlkYXRlKHByb3BzLCBwcm9wTmFtZSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSkge1xuICAgICAgdmFyIHByb3BWYWx1ZSA9IHByb3BzW3Byb3BOYW1lXTtcbiAgICAgIGZvciAodmFyIGkgPSAwOyBpIDwgZXhwZWN0ZWRWYWx1ZXMubGVuZ3RoOyBpKyspIHtcbiAgICAgICAgaWYgKGlzKHByb3BWYWx1ZSwgZXhwZWN0ZWRWYWx1ZXNbaV0pKSB7XG4gICAgICAgICAgcmV0dXJuIG51bGw7XG4gICAgICAgIH1cbiAgICAgIH1cblxuICAgICAgdmFyIHZhbHVlc1N0cmluZyA9IEpTT04uc3RyaW5naWZ5KGV4cGVjdGVkVmFsdWVzKTtcbiAgICAgIHJldHVybiBuZXcgUHJvcFR5cGVFcnJvcignSW52YWxpZCAnICsgbG9jYXRpb24gKyAnIGAnICsgcHJvcEZ1bGxOYW1lICsgJ2Agb2YgdmFsdWUgYCcgKyBwcm9wVmFsdWUgKyAnYCAnICsgKCdzdXBwbGllZCB0byBgJyArIGNvbXBvbmVudE5hbWUgKyAnYCwgZXhwZWN0ZWQgb25lIG9mICcgKyB2YWx1ZXNTdHJpbmcgKyAnLicpKTtcbiAgICB9XG4gICAgcmV0dXJuIGNyZWF0ZUNoYWluYWJsZVR5cGVDaGVja2VyKHZhbGlkYXRlKTtcbiAgfVxuXG4gIGZ1bmN0aW9uIGNyZWF0ZU9iamVjdE9mVHlwZUNoZWNrZXIodHlwZUNoZWNrZXIpIHtcbiAgICBmdW5jdGlvbiB2YWxpZGF0ZShwcm9wcywgcHJvcE5hbWUsIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUpIHtcbiAgICAgIGlmICh0eXBlb2YgdHlwZUNoZWNrZXIgIT09ICdmdW5jdGlvbicpIHtcbiAgICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKCdQcm9wZXJ0eSBgJyArIHByb3BGdWxsTmFtZSArICdgIG9mIGNvbXBvbmVudCBgJyArIGNvbXBvbmVudE5hbWUgKyAnYCBoYXMgaW52YWxpZCBQcm9wVHlwZSBub3RhdGlvbiBpbnNpZGUgb2JqZWN0T2YuJyk7XG4gICAgICB9XG4gICAgICB2YXIgcHJvcFZhbHVlID0gcHJvcHNbcHJvcE5hbWVdO1xuICAgICAgdmFyIHByb3BUeXBlID0gZ2V0UHJvcFR5cGUocHJvcFZhbHVlKTtcbiAgICAgIGlmIChwcm9wVHlwZSAhPT0gJ29iamVjdCcpIHtcbiAgICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKCdJbnZhbGlkICcgKyBsb2NhdGlvbiArICcgYCcgKyBwcm9wRnVsbE5hbWUgKyAnYCBvZiB0eXBlICcgKyAoJ2AnICsgcHJvcFR5cGUgKyAnYCBzdXBwbGllZCB0byBgJyArIGNvbXBvbmVudE5hbWUgKyAnYCwgZXhwZWN0ZWQgYW4gb2JqZWN0LicpKTtcbiAgICAgIH1cbiAgICAgIGZvciAodmFyIGtleSBpbiBwcm9wVmFsdWUpIHtcbiAgICAgICAgaWYgKHByb3BWYWx1ZS5oYXNPd25Qcm9wZXJ0eShrZXkpKSB7XG4gICAgICAgICAgdmFyIGVycm9yID0gdHlwZUNoZWNrZXIocHJvcFZhbHVlLCBrZXksIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUgKyAnLicgKyBrZXksIFJlYWN0UHJvcFR5cGVzU2VjcmV0KTtcbiAgICAgICAgICBpZiAoZXJyb3IgaW5zdGFuY2VvZiBFcnJvcikge1xuICAgICAgICAgICAgcmV0dXJuIGVycm9yO1xuICAgICAgICAgIH1cbiAgICAgICAgfVxuICAgICAgfVxuICAgICAgcmV0dXJuIG51bGw7XG4gICAgfVxuICAgIHJldHVybiBjcmVhdGVDaGFpbmFibGVUeXBlQ2hlY2tlcih2YWxpZGF0ZSk7XG4gIH1cblxuICBmdW5jdGlvbiBjcmVhdGVVbmlvblR5cGVDaGVja2VyKGFycmF5T2ZUeXBlQ2hlY2tlcnMpIHtcbiAgICBpZiAoIUFycmF5LmlzQXJyYXkoYXJyYXlPZlR5cGVDaGVja2VycykpIHtcbiAgICAgIHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicgPyB3YXJuaW5nKGZhbHNlLCAnSW52YWxpZCBhcmd1bWVudCBzdXBwbGllZCB0byBvbmVPZlR5cGUsIGV4cGVjdGVkIGFuIGluc3RhbmNlIG9mIGFycmF5LicpIDogdm9pZCAwO1xuICAgICAgcmV0dXJuIGVtcHR5RnVuY3Rpb24udGhhdFJldHVybnNOdWxsO1xuICAgIH1cblxuICAgIGZvciAodmFyIGkgPSAwOyBpIDwgYXJyYXlPZlR5cGVDaGVja2Vycy5sZW5ndGg7IGkrKykge1xuICAgICAgdmFyIGNoZWNrZXIgPSBhcnJheU9mVHlwZUNoZWNrZXJzW2ldO1xuICAgICAgaWYgKHR5cGVvZiBjaGVja2VyICE9PSAnZnVuY3Rpb24nKSB7XG4gICAgICAgIHdhcm5pbmcoXG4gICAgICAgICAgZmFsc2UsXG4gICAgICAgICAgJ0ludmFsaWQgYXJndW1lbnQgc3VwcGxpZWQgdG8gb25lT2ZUeXBlLiBFeHBlY3RlZCBhbiBhcnJheSBvZiBjaGVjayBmdW5jdGlvbnMsIGJ1dCAnICtcbiAgICAgICAgICAncmVjZWl2ZWQgJXMgYXQgaW5kZXggJXMuJyxcbiAgICAgICAgICBnZXRQb3N0Zml4Rm9yVHlwZVdhcm5pbmcoY2hlY2tlciksXG4gICAgICAgICAgaVxuICAgICAgICApO1xuICAgICAgICByZXR1cm4gZW1wdHlGdW5jdGlvbi50aGF0UmV0dXJuc051bGw7XG4gICAgICB9XG4gICAgfVxuXG4gICAgZnVuY3Rpb24gdmFsaWRhdGUocHJvcHMsIHByb3BOYW1lLCBjb21wb25lbnROYW1lLCBsb2NhdGlvbiwgcHJvcEZ1bGxOYW1lKSB7XG4gICAgICBmb3IgKHZhciBpID0gMDsgaSA8IGFycmF5T2ZUeXBlQ2hlY2tlcnMubGVuZ3RoOyBpKyspIHtcbiAgICAgICAgdmFyIGNoZWNrZXIgPSBhcnJheU9mVHlwZUNoZWNrZXJzW2ldO1xuICAgICAgICBpZiAoY2hlY2tlcihwcm9wcywgcHJvcE5hbWUsIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUsIFJlYWN0UHJvcFR5cGVzU2VjcmV0KSA9PSBudWxsKSB7XG4gICAgICAgICAgcmV0dXJuIG51bGw7XG4gICAgICAgIH1cbiAgICAgIH1cblxuICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKCdJbnZhbGlkICcgKyBsb2NhdGlvbiArICcgYCcgKyBwcm9wRnVsbE5hbWUgKyAnYCBzdXBwbGllZCB0byAnICsgKCdgJyArIGNvbXBvbmVudE5hbWUgKyAnYC4nKSk7XG4gICAgfVxuICAgIHJldHVybiBjcmVhdGVDaGFpbmFibGVUeXBlQ2hlY2tlcih2YWxpZGF0ZSk7XG4gIH1cblxuICBmdW5jdGlvbiBjcmVhdGVOb2RlQ2hlY2tlcigpIHtcbiAgICBmdW5jdGlvbiB2YWxpZGF0ZShwcm9wcywgcHJvcE5hbWUsIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUpIHtcbiAgICAgIGlmICghaXNOb2RlKHByb3BzW3Byb3BOYW1lXSkpIHtcbiAgICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKCdJbnZhbGlkICcgKyBsb2NhdGlvbiArICcgYCcgKyBwcm9wRnVsbE5hbWUgKyAnYCBzdXBwbGllZCB0byAnICsgKCdgJyArIGNvbXBvbmVudE5hbWUgKyAnYCwgZXhwZWN0ZWQgYSBSZWFjdE5vZGUuJykpO1xuICAgICAgfVxuICAgICAgcmV0dXJuIG51bGw7XG4gICAgfVxuICAgIHJldHVybiBjcmVhdGVDaGFpbmFibGVUeXBlQ2hlY2tlcih2YWxpZGF0ZSk7XG4gIH1cblxuICBmdW5jdGlvbiBjcmVhdGVTaGFwZVR5cGVDaGVja2VyKHNoYXBlVHlwZXMpIHtcbiAgICBmdW5jdGlvbiB2YWxpZGF0ZShwcm9wcywgcHJvcE5hbWUsIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUpIHtcbiAgICAgIHZhciBwcm9wVmFsdWUgPSBwcm9wc1twcm9wTmFtZV07XG4gICAgICB2YXIgcHJvcFR5cGUgPSBnZXRQcm9wVHlwZShwcm9wVmFsdWUpO1xuICAgICAgaWYgKHByb3BUeXBlICE9PSAnb2JqZWN0Jykge1xuICAgICAgICByZXR1cm4gbmV3IFByb3BUeXBlRXJyb3IoJ0ludmFsaWQgJyArIGxvY2F0aW9uICsgJyBgJyArIHByb3BGdWxsTmFtZSArICdgIG9mIHR5cGUgYCcgKyBwcm9wVHlwZSArICdgICcgKyAoJ3N1cHBsaWVkIHRvIGAnICsgY29tcG9uZW50TmFtZSArICdgLCBleHBlY3RlZCBgb2JqZWN0YC4nKSk7XG4gICAgICB9XG4gICAgICBmb3IgKHZhciBrZXkgaW4gc2hhcGVUeXBlcykge1xuICAgICAgICB2YXIgY2hlY2tlciA9IHNoYXBlVHlwZXNba2V5XTtcbiAgICAgICAgaWYgKCFjaGVja2VyKSB7XG4gICAgICAgICAgY29udGludWU7XG4gICAgICAgIH1cbiAgICAgICAgdmFyIGVycm9yID0gY2hlY2tlcihwcm9wVmFsdWUsIGtleSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSArICcuJyArIGtleSwgUmVhY3RQcm9wVHlwZXNTZWNyZXQpO1xuICAgICAgICBpZiAoZXJyb3IpIHtcbiAgICAgICAgICByZXR1cm4gZXJyb3I7XG4gICAgICAgIH1cbiAgICAgIH1cbiAgICAgIHJldHVybiBudWxsO1xuICAgIH1cbiAgICByZXR1cm4gY3JlYXRlQ2hhaW5hYmxlVHlwZUNoZWNrZXIodmFsaWRhdGUpO1xuICB9XG5cbiAgZnVuY3Rpb24gY3JlYXRlU3RyaWN0U2hhcGVUeXBlQ2hlY2tlcihzaGFwZVR5cGVzKSB7XG4gICAgZnVuY3Rpb24gdmFsaWRhdGUocHJvcHMsIHByb3BOYW1lLCBjb21wb25lbnROYW1lLCBsb2NhdGlvbiwgcHJvcEZ1bGxOYW1lKSB7XG4gICAgICB2YXIgcHJvcFZhbHVlID0gcHJvcHNbcHJvcE5hbWVdO1xuICAgICAgdmFyIHByb3BUeXBlID0gZ2V0UHJvcFR5cGUocHJvcFZhbHVlKTtcbiAgICAgIGlmIChwcm9wVHlwZSAhPT0gJ29iamVjdCcpIHtcbiAgICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKCdJbnZhbGlkICcgKyBsb2NhdGlvbiArICcgYCcgKyBwcm9wRnVsbE5hbWUgKyAnYCBvZiB0eXBlIGAnICsgcHJvcFR5cGUgKyAnYCAnICsgKCdzdXBwbGllZCB0byBgJyArIGNvbXBvbmVudE5hbWUgKyAnYCwgZXhwZWN0ZWQgYG9iamVjdGAuJykpO1xuICAgICAgfVxuICAgICAgLy8gV2UgbmVlZCB0byBjaGVjayBhbGwga2V5cyBpbiBjYXNlIHNvbWUgYXJlIHJlcXVpcmVkIGJ1dCBtaXNzaW5nIGZyb21cbiAgICAgIC8vIHByb3BzLlxuICAgICAgdmFyIGFsbEtleXMgPSBhc3NpZ24oe30sIHByb3BzW3Byb3BOYW1lXSwgc2hhcGVUeXBlcyk7XG4gICAgICBmb3IgKHZhciBrZXkgaW4gYWxsS2V5cykge1xuICAgICAgICB2YXIgY2hlY2tlciA9IHNoYXBlVHlwZXNba2V5XTtcbiAgICAgICAgaWYgKCFjaGVja2VyKSB7XG4gICAgICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKFxuICAgICAgICAgICAgJ0ludmFsaWQgJyArIGxvY2F0aW9uICsgJyBgJyArIHByb3BGdWxsTmFtZSArICdgIGtleSBgJyArIGtleSArICdgIHN1cHBsaWVkIHRvIGAnICsgY29tcG9uZW50TmFtZSArICdgLicgK1xuICAgICAgICAgICAgJ1xcbkJhZCBvYmplY3Q6ICcgKyBKU09OLnN0cmluZ2lmeShwcm9wc1twcm9wTmFtZV0sIG51bGwsICcgICcpICtcbiAgICAgICAgICAgICdcXG5WYWxpZCBrZXlzOiAnICsgIEpTT04uc3RyaW5naWZ5KE9iamVjdC5rZXlzKHNoYXBlVHlwZXMpLCBudWxsLCAnICAnKVxuICAgICAgICAgICk7XG4gICAgICAgIH1cbiAgICAgICAgdmFyIGVycm9yID0gY2hlY2tlcihwcm9wVmFsdWUsIGtleSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSArICcuJyArIGtleSwgUmVhY3RQcm9wVHlwZXNTZWNyZXQpO1xuICAgICAgICBpZiAoZXJyb3IpIHtcbiAgICAgICAgICByZXR1cm4gZXJyb3I7XG4gICAgICAgIH1cbiAgICAgIH1cbiAgICAgIHJldHVybiBudWxsO1xuICAgIH1cblxuICAgIHJldHVybiBjcmVhdGVDaGFpbmFibGVUeXBlQ2hlY2tlcih2YWxpZGF0ZSk7XG4gIH1cblxuICBmdW5jdGlvbiBpc05vZGUocHJvcFZhbHVlKSB7XG4gICAgc3dpdGNoICh0eXBlb2YgcHJvcFZhbHVlKSB7XG4gICAgICBjYXNlICdudW1iZXInOlxuICAgICAgY2FzZSAnc3RyaW5nJzpcbiAgICAgIGNhc2UgJ3VuZGVmaW5lZCc6XG4gICAgICAgIHJldHVybiB0cnVlO1xuICAgICAgY2FzZSAnYm9vbGVhbic6XG4gICAgICAgIHJldHVybiAhcHJvcFZhbHVlO1xuICAgICAgY2FzZSAnb2JqZWN0JzpcbiAgICAgICAgaWYgKEFycmF5LmlzQXJyYXkocHJvcFZhbHVlKSkge1xuICAgICAgICAgIHJldHVybiBwcm9wVmFsdWUuZXZlcnkoaXNOb2RlKTtcbiAgICAgICAgfVxuICAgICAgICBpZiAocHJvcFZhbHVlID09PSBudWxsIHx8IGlzVmFsaWRFbGVtZW50KHByb3BWYWx1ZSkpIHtcbiAgICAgICAgICByZXR1cm4gdHJ1ZTtcbiAgICAgICAgfVxuXG4gICAgICAgIHZhciBpdGVyYXRvckZuID0gZ2V0SXRlcmF0b3JGbihwcm9wVmFsdWUpO1xuICAgICAgICBpZiAoaXRlcmF0b3JGbikge1xuICAgICAgICAgIHZhciBpdGVyYXRvciA9IGl0ZXJhdG9yRm4uY2FsbChwcm9wVmFsdWUpO1xuICAgICAgICAgIHZhciBzdGVwO1xuICAgICAgICAgIGlmIChpdGVyYXRvckZuICE9PSBwcm9wVmFsdWUuZW50cmllcykge1xuICAgICAgICAgICAgd2hpbGUgKCEoc3RlcCA9IGl0ZXJhdG9yLm5leHQoKSkuZG9uZSkge1xuICAgICAgICAgICAgICBpZiAoIWlzTm9kZShzdGVwLnZhbHVlKSkge1xuICAgICAgICAgICAgICAgIHJldHVybiBmYWxzZTtcbiAgICAgICAgICAgICAgfVxuICAgICAgICAgICAgfVxuICAgICAgICAgIH0gZWxzZSB7XG4gICAgICAgICAgICAvLyBJdGVyYXRvciB3aWxsIHByb3ZpZGUgZW50cnkgW2ssdl0gdHVwbGVzIHJhdGhlciB0aGFuIHZhbHVlcy5cbiAgICAgICAgICAgIHdoaWxlICghKHN0ZXAgPSBpdGVyYXRvci5uZXh0KCkpLmRvbmUpIHtcbiAgICAgICAgICAgICAgdmFyIGVudHJ5ID0gc3RlcC52YWx1ZTtcbiAgICAgICAgICAgICAgaWYgKGVudHJ5KSB7XG4gICAgICAgICAgICAgICAgaWYgKCFpc05vZGUoZW50cnlbMV0pKSB7XG4gICAgICAgICAgICAgICAgICByZXR1cm4gZmFsc2U7XG4gICAgICAgICAgICAgICAgfVxuICAgICAgICAgICAgICB9XG4gICAgICAgICAgICB9XG4gICAgICAgICAgfVxuICAgICAgICB9IGVsc2Uge1xuICAgICAgICAgIHJldHVybiBmYWxzZTtcbiAgICAgICAgfVxuXG4gICAgICAgIHJldHVybiB0cnVlO1xuICAgICAgZGVmYXVsdDpcbiAgICAgICAgcmV0dXJuIGZhbHNlO1xuICAgIH1cbiAgfVxuXG4gIGZ1bmN0aW9uIGlzU3ltYm9sKHByb3BUeXBlLCBwcm9wVmFsdWUpIHtcbiAgICAvLyBOYXRpdmUgU3ltYm9sLlxuICAgIGlmIChwcm9wVHlwZSA9PT0gJ3N5bWJvbCcpIHtcbiAgICAgIHJldHVybiB0cnVlO1xuICAgIH1cblxuICAgIC8vIDE5LjQuMy41IFN5bWJvbC5wcm90b3R5cGVbQEB0b1N0cmluZ1RhZ10gPT09ICdTeW1ib2wnXG4gICAgaWYgKHByb3BWYWx1ZVsnQEB0b1N0cmluZ1RhZyddID09PSAnU3ltYm9sJykge1xuICAgICAgcmV0dXJuIHRydWU7XG4gICAgfVxuXG4gICAgLy8gRmFsbGJhY2sgZm9yIG5vbi1zcGVjIGNvbXBsaWFudCBTeW1ib2xzIHdoaWNoIGFyZSBwb2x5ZmlsbGVkLlxuICAgIGlmICh0eXBlb2YgU3ltYm9sID09PSAnZnVuY3Rpb24nICYmIHByb3BWYWx1ZSBpbnN0YW5jZW9mIFN5bWJvbCkge1xuICAgICAgcmV0dXJuIHRydWU7XG4gICAgfVxuXG4gICAgcmV0dXJuIGZhbHNlO1xuICB9XG5cbiAgLy8gRXF1aXZhbGVudCBvZiBgdHlwZW9mYCBidXQgd2l0aCBzcGVjaWFsIGhhbmRsaW5nIGZvciBhcnJheSBhbmQgcmVnZXhwLlxuICBmdW5jdGlvbiBnZXRQcm9wVHlwZShwcm9wVmFsdWUpIHtcbiAgICB2YXIgcHJvcFR5cGUgPSB0eXBlb2YgcHJvcFZhbHVlO1xuICAgIGlmIChBcnJheS5pc0FycmF5KHByb3BWYWx1ZSkpIHtcbiAgICAgIHJldHVybiAnYXJyYXknO1xuICAgIH1cbiAgICBpZiAocHJvcFZhbHVlIGluc3RhbmNlb2YgUmVnRXhwKSB7XG4gICAgICAvLyBPbGQgd2Via2l0cyAoYXQgbGVhc3QgdW50aWwgQW5kcm9pZCA0LjApIHJldHVybiAnZnVuY3Rpb24nIHJhdGhlciB0aGFuXG4gICAgICAvLyAnb2JqZWN0JyBmb3IgdHlwZW9mIGEgUmVnRXhwLiBXZSdsbCBub3JtYWxpemUgdGhpcyBoZXJlIHNvIHRoYXQgL2JsYS9cbiAgICAgIC8vIHBhc3NlcyBQcm9wVHlwZXMub2JqZWN0LlxuICAgICAgcmV0dXJuICdvYmplY3QnO1xuICAgIH1cbiAgICBpZiAoaXNTeW1ib2wocHJvcFR5cGUsIHByb3BWYWx1ZSkpIHtcbiAgICAgIHJldHVybiAnc3ltYm9sJztcbiAgICB9XG4gICAgcmV0dXJuIHByb3BUeXBlO1xuICB9XG5cbiAgLy8gVGhpcyBoYW5kbGVzIG1vcmUgdHlwZXMgdGhhbiBgZ2V0UHJvcFR5cGVgLiBPbmx5IHVzZWQgZm9yIGVycm9yIG1lc3NhZ2VzLlxuICAvLyBTZWUgYGNyZWF0ZVByaW1pdGl2ZVR5cGVDaGVja2VyYC5cbiAgZnVuY3Rpb24gZ2V0UHJlY2lzZVR5cGUocHJvcFZhbHVlKSB7XG4gICAgaWYgKHR5cGVvZiBwcm9wVmFsdWUgPT09ICd1bmRlZmluZWQnIHx8IHByb3BWYWx1ZSA9PT0gbnVsbCkge1xuICAgICAgcmV0dXJuICcnICsgcHJvcFZhbHVlO1xuICAgIH1cbiAgICB2YXIgcHJvcFR5cGUgPSBnZXRQcm9wVHlwZShwcm9wVmFsdWUpO1xuICAgIGlmIChwcm9wVHlwZSA9PT0gJ29iamVjdCcpIHtcbiAgICAgIGlmIChwcm9wVmFsdWUgaW5zdGFuY2VvZiBEYXRlKSB7XG4gICAgICAgIHJldHVybiAnZGF0ZSc7XG4gICAgICB9IGVsc2UgaWYgKHByb3BWYWx1ZSBpbnN0YW5jZW9mIFJlZ0V4cCkge1xuICAgICAgICByZXR1cm4gJ3JlZ2V4cCc7XG4gICAgICB9XG4gICAgfVxuICAgIHJldHVybiBwcm9wVHlwZTtcbiAgfVxuXG4gIC8vIFJldHVybnMgYSBzdHJpbmcgdGhhdCBpcyBwb3N0Zml4ZWQgdG8gYSB3YXJuaW5nIGFib3V0IGFuIGludmFsaWQgdHlwZS5cbiAgLy8gRm9yIGV4YW1wbGUsIFwidW5kZWZpbmVkXCIgb3IgXCJvZiB0eXBlIGFycmF5XCJcbiAgZnVuY3Rpb24gZ2V0UG9zdGZpeEZvclR5cGVXYXJuaW5nKHZhbHVlKSB7XG4gICAgdmFyIHR5cGUgPSBnZXRQcmVjaXNlVHlwZSh2YWx1ZSk7XG4gICAgc3dpdGNoICh0eXBlKSB7XG4gICAgICBjYXNlICdhcnJheSc6XG4gICAgICBjYXNlICdvYmplY3QnOlxuICAgICAgICByZXR1cm4gJ2FuICcgKyB0eXBlO1xuICAgICAgY2FzZSAnYm9vbGVhbic6XG4gICAgICBjYXNlICdkYXRlJzpcbiAgICAgIGNhc2UgJ3JlZ2V4cCc6XG4gICAgICAgIHJldHVybiAnYSAnICsgdHlwZTtcbiAgICAgIGRlZmF1bHQ6XG4gICAgICAgIHJldHVybiB0eXBlO1xuICAgIH1cbiAgfVxuXG4gIC8vIFJldHVybnMgY2xhc3MgbmFtZSBvZiB0aGUgb2JqZWN0LCBpZiBhbnkuXG4gIGZ1bmN0aW9uIGdldENsYXNzTmFtZShwcm9wVmFsdWUpIHtcbiAgICBpZiAoIXByb3BWYWx1ZS5jb25zdHJ1Y3RvciB8fCAhcHJvcFZhbHVlLmNvbnN0cnVjdG9yLm5hbWUpIHtcbiAgICAgIHJldHVybiBBTk9OWU1PVVM7XG4gICAgfVxuICAgIHJldHVybiBwcm9wVmFsdWUuY29uc3RydWN0b3IubmFtZTtcbiAgfVxuXG4gIFJlYWN0UHJvcFR5cGVzLmNoZWNrUHJvcFR5cGVzID0gY2hlY2tQcm9wVHlwZXM7XG4gIFJlYWN0UHJvcFR5cGVzLlByb3BUeXBlcyA9IFJlYWN0UHJvcFR5cGVzO1xuXG4gIHJldHVybiBSZWFjdFByb3BUeXBlcztcbn07XG4iLCIvKipcbiAqIENvcHlyaWdodCAoYykgMjAxMy1wcmVzZW50LCBGYWNlYm9vaywgSW5jLlxuICpcbiAqIFRoaXMgc291cmNlIGNvZGUgaXMgbGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlIGZvdW5kIGluIHRoZVxuICogTElDRU5TRSBmaWxlIGluIHRoZSByb290IGRpcmVjdG9yeSBvZiB0aGlzIHNvdXJjZSB0cmVlLlxuICovXG5cbmlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gIHZhciBSRUFDVF9FTEVNRU5UX1RZUEUgPSAodHlwZW9mIFN5bWJvbCA9PT0gJ2Z1bmN0aW9uJyAmJlxuICAgIFN5bWJvbC5mb3IgJiZcbiAgICBTeW1ib2wuZm9yKCdyZWFjdC5lbGVtZW50JykpIHx8XG4gICAgMHhlYWM3O1xuXG4gIHZhciBpc1ZhbGlkRWxlbWVudCA9IGZ1bmN0aW9uKG9iamVjdCkge1xuICAgIHJldHVybiB0eXBlb2Ygb2JqZWN0ID09PSAnb2JqZWN0JyAmJlxuICAgICAgb2JqZWN0ICE9PSBudWxsICYmXG4gICAgICBvYmplY3QuJCR0eXBlb2YgPT09IFJFQUNUX0VMRU1FTlRfVFlQRTtcbiAgfTtcblxuICAvLyBCeSBleHBsaWNpdGx5IHVzaW5nIGBwcm9wLXR5cGVzYCB5b3UgYXJlIG9wdGluZyBpbnRvIG5ldyBkZXZlbG9wbWVudCBiZWhhdmlvci5cbiAgLy8gaHR0cDovL2ZiLm1lL3Byb3AtdHlwZXMtaW4tcHJvZFxuICB2YXIgdGhyb3dPbkRpcmVjdEFjY2VzcyA9IHRydWU7XG4gIG1vZHVsZS5leHBvcnRzID0gcmVxdWlyZSgnLi9mYWN0b3J5V2l0aFR5cGVDaGVja2VycycpKGlzVmFsaWRFbGVtZW50LCB0aHJvd09uRGlyZWN0QWNjZXNzKTtcbn0gZWxzZSB7XG4gIC8vIEJ5IGV4cGxpY2l0bHkgdXNpbmcgYHByb3AtdHlwZXNgIHlvdSBhcmUgb3B0aW5nIGludG8gbmV3IHByb2R1Y3Rpb24gYmVoYXZpb3IuXG4gIC8vIGh0dHA6Ly9mYi5tZS9wcm9wLXR5cGVzLWluLXByb2RcbiAgbW9kdWxlLmV4cG9ydHMgPSByZXF1aXJlKCcuL2ZhY3RvcnlXaXRoVGhyb3dpbmdTaGltcycpKCk7XG59XG4iLCIvKipcbiAqIENvcHlyaWdodCAoYykgMjAxMy1wcmVzZW50LCBGYWNlYm9vaywgSW5jLlxuICpcbiAqIFRoaXMgc291cmNlIGNvZGUgaXMgbGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlIGZvdW5kIGluIHRoZVxuICogTElDRU5TRSBmaWxlIGluIHRoZSByb290IGRpcmVjdG9yeSBvZiB0aGlzIHNvdXJjZSB0cmVlLlxuICovXG5cbid1c2Ugc3RyaWN0JztcblxudmFyIFJlYWN0UHJvcFR5cGVzU2VjcmV0ID0gJ1NFQ1JFVF9ET19OT1RfUEFTU19USElTX09SX1lPVV9XSUxMX0JFX0ZJUkVEJztcblxubW9kdWxlLmV4cG9ydHMgPSBSZWFjdFByb3BUeXBlc1NlY3JldDtcbiIsIid1c2Ugc3RyaWN0JztcbnZhciBzdHJpY3RVcmlFbmNvZGUgPSByZXF1aXJlKCdzdHJpY3QtdXJpLWVuY29kZScpO1xudmFyIG9iamVjdEFzc2lnbiA9IHJlcXVpcmUoJ29iamVjdC1hc3NpZ24nKTtcbnZhciBkZWNvZGVDb21wb25lbnQgPSByZXF1aXJlKCdkZWNvZGUtdXJpLWNvbXBvbmVudCcpO1xuXG5mdW5jdGlvbiBlbmNvZGVyRm9yQXJyYXlGb3JtYXQob3B0cykge1xuXHRzd2l0Y2ggKG9wdHMuYXJyYXlGb3JtYXQpIHtcblx0XHRjYXNlICdpbmRleCc6XG5cdFx0XHRyZXR1cm4gZnVuY3Rpb24gKGtleSwgdmFsdWUsIGluZGV4KSB7XG5cdFx0XHRcdHJldHVybiB2YWx1ZSA9PT0gbnVsbCA/IFtcblx0XHRcdFx0XHRlbmNvZGUoa2V5LCBvcHRzKSxcblx0XHRcdFx0XHQnWycsXG5cdFx0XHRcdFx0aW5kZXgsXG5cdFx0XHRcdFx0J10nXG5cdFx0XHRcdF0uam9pbignJykgOiBbXG5cdFx0XHRcdFx0ZW5jb2RlKGtleSwgb3B0cyksXG5cdFx0XHRcdFx0J1snLFxuXHRcdFx0XHRcdGVuY29kZShpbmRleCwgb3B0cyksXG5cdFx0XHRcdFx0J109Jyxcblx0XHRcdFx0XHRlbmNvZGUodmFsdWUsIG9wdHMpXG5cdFx0XHRcdF0uam9pbignJyk7XG5cdFx0XHR9O1xuXG5cdFx0Y2FzZSAnYnJhY2tldCc6XG5cdFx0XHRyZXR1cm4gZnVuY3Rpb24gKGtleSwgdmFsdWUpIHtcblx0XHRcdFx0cmV0dXJuIHZhbHVlID09PSBudWxsID8gZW5jb2RlKGtleSwgb3B0cykgOiBbXG5cdFx0XHRcdFx0ZW5jb2RlKGtleSwgb3B0cyksXG5cdFx0XHRcdFx0J1tdPScsXG5cdFx0XHRcdFx0ZW5jb2RlKHZhbHVlLCBvcHRzKVxuXHRcdFx0XHRdLmpvaW4oJycpO1xuXHRcdFx0fTtcblxuXHRcdGRlZmF1bHQ6XG5cdFx0XHRyZXR1cm4gZnVuY3Rpb24gKGtleSwgdmFsdWUpIHtcblx0XHRcdFx0cmV0dXJuIHZhbHVlID09PSBudWxsID8gZW5jb2RlKGtleSwgb3B0cykgOiBbXG5cdFx0XHRcdFx0ZW5jb2RlKGtleSwgb3B0cyksXG5cdFx0XHRcdFx0Jz0nLFxuXHRcdFx0XHRcdGVuY29kZSh2YWx1ZSwgb3B0cylcblx0XHRcdFx0XS5qb2luKCcnKTtcblx0XHRcdH07XG5cdH1cbn1cblxuZnVuY3Rpb24gcGFyc2VyRm9yQXJyYXlGb3JtYXQob3B0cykge1xuXHR2YXIgcmVzdWx0O1xuXG5cdHN3aXRjaCAob3B0cy5hcnJheUZvcm1hdCkge1xuXHRcdGNhc2UgJ2luZGV4Jzpcblx0XHRcdHJldHVybiBmdW5jdGlvbiAoa2V5LCB2YWx1ZSwgYWNjdW11bGF0b3IpIHtcblx0XHRcdFx0cmVzdWx0ID0gL1xcWyhcXGQqKVxcXSQvLmV4ZWMoa2V5KTtcblxuXHRcdFx0XHRrZXkgPSBrZXkucmVwbGFjZSgvXFxbXFxkKlxcXSQvLCAnJyk7XG5cblx0XHRcdFx0aWYgKCFyZXN1bHQpIHtcblx0XHRcdFx0XHRhY2N1bXVsYXRvcltrZXldID0gdmFsdWU7XG5cdFx0XHRcdFx0cmV0dXJuO1xuXHRcdFx0XHR9XG5cblx0XHRcdFx0aWYgKGFjY3VtdWxhdG9yW2tleV0gPT09IHVuZGVmaW5lZCkge1xuXHRcdFx0XHRcdGFjY3VtdWxhdG9yW2tleV0gPSB7fTtcblx0XHRcdFx0fVxuXG5cdFx0XHRcdGFjY3VtdWxhdG9yW2tleV1bcmVzdWx0WzFdXSA9IHZhbHVlO1xuXHRcdFx0fTtcblxuXHRcdGNhc2UgJ2JyYWNrZXQnOlxuXHRcdFx0cmV0dXJuIGZ1bmN0aW9uIChrZXksIHZhbHVlLCBhY2N1bXVsYXRvcikge1xuXHRcdFx0XHRyZXN1bHQgPSAvKFxcW1xcXSkkLy5leGVjKGtleSk7XG5cdFx0XHRcdGtleSA9IGtleS5yZXBsYWNlKC9cXFtcXF0kLywgJycpO1xuXG5cdFx0XHRcdGlmICghcmVzdWx0KSB7XG5cdFx0XHRcdFx0YWNjdW11bGF0b3Jba2V5XSA9IHZhbHVlO1xuXHRcdFx0XHRcdHJldHVybjtcblx0XHRcdFx0fSBlbHNlIGlmIChhY2N1bXVsYXRvcltrZXldID09PSB1bmRlZmluZWQpIHtcblx0XHRcdFx0XHRhY2N1bXVsYXRvcltrZXldID0gW3ZhbHVlXTtcblx0XHRcdFx0XHRyZXR1cm47XG5cdFx0XHRcdH1cblxuXHRcdFx0XHRhY2N1bXVsYXRvcltrZXldID0gW10uY29uY2F0KGFjY3VtdWxhdG9yW2tleV0sIHZhbHVlKTtcblx0XHRcdH07XG5cblx0XHRkZWZhdWx0OlxuXHRcdFx0cmV0dXJuIGZ1bmN0aW9uIChrZXksIHZhbHVlLCBhY2N1bXVsYXRvcikge1xuXHRcdFx0XHRpZiAoYWNjdW11bGF0b3Jba2V5XSA9PT0gdW5kZWZpbmVkKSB7XG5cdFx0XHRcdFx0YWNjdW11bGF0b3Jba2V5XSA9IHZhbHVlO1xuXHRcdFx0XHRcdHJldHVybjtcblx0XHRcdFx0fVxuXG5cdFx0XHRcdGFjY3VtdWxhdG9yW2tleV0gPSBbXS5jb25jYXQoYWNjdW11bGF0b3Jba2V5XSwgdmFsdWUpO1xuXHRcdFx0fTtcblx0fVxufVxuXG5mdW5jdGlvbiBlbmNvZGUodmFsdWUsIG9wdHMpIHtcblx0aWYgKG9wdHMuZW5jb2RlKSB7XG5cdFx0cmV0dXJuIG9wdHMuc3RyaWN0ID8gc3RyaWN0VXJpRW5jb2RlKHZhbHVlKSA6IGVuY29kZVVSSUNvbXBvbmVudCh2YWx1ZSk7XG5cdH1cblxuXHRyZXR1cm4gdmFsdWU7XG59XG5cbmZ1bmN0aW9uIGtleXNTb3J0ZXIoaW5wdXQpIHtcblx0aWYgKEFycmF5LmlzQXJyYXkoaW5wdXQpKSB7XG5cdFx0cmV0dXJuIGlucHV0LnNvcnQoKTtcblx0fSBlbHNlIGlmICh0eXBlb2YgaW5wdXQgPT09ICdvYmplY3QnKSB7XG5cdFx0cmV0dXJuIGtleXNTb3J0ZXIoT2JqZWN0LmtleXMoaW5wdXQpKS5zb3J0KGZ1bmN0aW9uIChhLCBiKSB7XG5cdFx0XHRyZXR1cm4gTnVtYmVyKGEpIC0gTnVtYmVyKGIpO1xuXHRcdH0pLm1hcChmdW5jdGlvbiAoa2V5KSB7XG5cdFx0XHRyZXR1cm4gaW5wdXRba2V5XTtcblx0XHR9KTtcblx0fVxuXG5cdHJldHVybiBpbnB1dDtcbn1cblxuZnVuY3Rpb24gZXh0cmFjdChzdHIpIHtcblx0dmFyIHF1ZXJ5U3RhcnQgPSBzdHIuaW5kZXhPZignPycpO1xuXHRpZiAocXVlcnlTdGFydCA9PT0gLTEpIHtcblx0XHRyZXR1cm4gJyc7XG5cdH1cblx0cmV0dXJuIHN0ci5zbGljZShxdWVyeVN0YXJ0ICsgMSk7XG59XG5cbmZ1bmN0aW9uIHBhcnNlKHN0ciwgb3B0cykge1xuXHRvcHRzID0gb2JqZWN0QXNzaWduKHthcnJheUZvcm1hdDogJ25vbmUnfSwgb3B0cyk7XG5cblx0dmFyIGZvcm1hdHRlciA9IHBhcnNlckZvckFycmF5Rm9ybWF0KG9wdHMpO1xuXG5cdC8vIENyZWF0ZSBhbiBvYmplY3Qgd2l0aCBubyBwcm90b3R5cGVcblx0Ly8gaHR0cHM6Ly9naXRodWIuY29tL3NpbmRyZXNvcmh1cy9xdWVyeS1zdHJpbmcvaXNzdWVzLzQ3XG5cdHZhciByZXQgPSBPYmplY3QuY3JlYXRlKG51bGwpO1xuXG5cdGlmICh0eXBlb2Ygc3RyICE9PSAnc3RyaW5nJykge1xuXHRcdHJldHVybiByZXQ7XG5cdH1cblxuXHRzdHIgPSBzdHIudHJpbSgpLnJlcGxhY2UoL15bPyMmXS8sICcnKTtcblxuXHRpZiAoIXN0cikge1xuXHRcdHJldHVybiByZXQ7XG5cdH1cblxuXHRzdHIuc3BsaXQoJyYnKS5mb3JFYWNoKGZ1bmN0aW9uIChwYXJhbSkge1xuXHRcdHZhciBwYXJ0cyA9IHBhcmFtLnJlcGxhY2UoL1xcKy9nLCAnICcpLnNwbGl0KCc9Jyk7XG5cdFx0Ly8gRmlyZWZveCAocHJlIDQwKSBkZWNvZGVzIGAlM0RgIHRvIGA9YFxuXHRcdC8vIGh0dHBzOi8vZ2l0aHViLmNvbS9zaW5kcmVzb3JodXMvcXVlcnktc3RyaW5nL3B1bGwvMzdcblx0XHR2YXIga2V5ID0gcGFydHMuc2hpZnQoKTtcblx0XHR2YXIgdmFsID0gcGFydHMubGVuZ3RoID4gMCA/IHBhcnRzLmpvaW4oJz0nKSA6IHVuZGVmaW5lZDtcblxuXHRcdC8vIG1pc3NpbmcgYD1gIHNob3VsZCBiZSBgbnVsbGA6XG5cdFx0Ly8gaHR0cDovL3czLm9yZy9UUi8yMDEyL1dELXVybC0yMDEyMDUyNC8jY29sbGVjdC11cmwtcGFyYW1ldGVyc1xuXHRcdHZhbCA9IHZhbCA9PT0gdW5kZWZpbmVkID8gbnVsbCA6IGRlY29kZUNvbXBvbmVudCh2YWwpO1xuXG5cdFx0Zm9ybWF0dGVyKGRlY29kZUNvbXBvbmVudChrZXkpLCB2YWwsIHJldCk7XG5cdH0pO1xuXG5cdHJldHVybiBPYmplY3Qua2V5cyhyZXQpLnNvcnQoKS5yZWR1Y2UoZnVuY3Rpb24gKHJlc3VsdCwga2V5KSB7XG5cdFx0dmFyIHZhbCA9IHJldFtrZXldO1xuXHRcdGlmIChCb29sZWFuKHZhbCkgJiYgdHlwZW9mIHZhbCA9PT0gJ29iamVjdCcgJiYgIUFycmF5LmlzQXJyYXkodmFsKSkge1xuXHRcdFx0Ly8gU29ydCBvYmplY3Qga2V5cywgbm90IHZhbHVlc1xuXHRcdFx0cmVzdWx0W2tleV0gPSBrZXlzU29ydGVyKHZhbCk7XG5cdFx0fSBlbHNlIHtcblx0XHRcdHJlc3VsdFtrZXldID0gdmFsO1xuXHRcdH1cblxuXHRcdHJldHVybiByZXN1bHQ7XG5cdH0sIE9iamVjdC5jcmVhdGUobnVsbCkpO1xufVxuXG5leHBvcnRzLmV4dHJhY3QgPSBleHRyYWN0O1xuZXhwb3J0cy5wYXJzZSA9IHBhcnNlO1xuXG5leHBvcnRzLnN0cmluZ2lmeSA9IGZ1bmN0aW9uIChvYmosIG9wdHMpIHtcblx0dmFyIGRlZmF1bHRzID0ge1xuXHRcdGVuY29kZTogdHJ1ZSxcblx0XHRzdHJpY3Q6IHRydWUsXG5cdFx0YXJyYXlGb3JtYXQ6ICdub25lJ1xuXHR9O1xuXG5cdG9wdHMgPSBvYmplY3RBc3NpZ24oZGVmYXVsdHMsIG9wdHMpO1xuXG5cdGlmIChvcHRzLnNvcnQgPT09IGZhbHNlKSB7XG5cdFx0b3B0cy5zb3J0ID0gZnVuY3Rpb24gKCkge307XG5cdH1cblxuXHR2YXIgZm9ybWF0dGVyID0gZW5jb2RlckZvckFycmF5Rm9ybWF0KG9wdHMpO1xuXG5cdHJldHVybiBvYmogPyBPYmplY3Qua2V5cyhvYmopLnNvcnQob3B0cy5zb3J0KS5tYXAoZnVuY3Rpb24gKGtleSkge1xuXHRcdHZhciB2YWwgPSBvYmpba2V5XTtcblxuXHRcdGlmICh2YWwgPT09IHVuZGVmaW5lZCkge1xuXHRcdFx0cmV0dXJuICcnO1xuXHRcdH1cblxuXHRcdGlmICh2YWwgPT09IG51bGwpIHtcblx0XHRcdHJldHVybiBlbmNvZGUoa2V5LCBvcHRzKTtcblx0XHR9XG5cblx0XHRpZiAoQXJyYXkuaXNBcnJheSh2YWwpKSB7XG5cdFx0XHR2YXIgcmVzdWx0ID0gW107XG5cblx0XHRcdHZhbC5zbGljZSgpLmZvckVhY2goZnVuY3Rpb24gKHZhbDIpIHtcblx0XHRcdFx0aWYgKHZhbDIgPT09IHVuZGVmaW5lZCkge1xuXHRcdFx0XHRcdHJldHVybjtcblx0XHRcdFx0fVxuXG5cdFx0XHRcdHJlc3VsdC5wdXNoKGZvcm1hdHRlcihrZXksIHZhbDIsIHJlc3VsdC5sZW5ndGgpKTtcblx0XHRcdH0pO1xuXG5cdFx0XHRyZXR1cm4gcmVzdWx0LmpvaW4oJyYnKTtcblx0XHR9XG5cblx0XHRyZXR1cm4gZW5jb2RlKGtleSwgb3B0cykgKyAnPScgKyBlbmNvZGUodmFsLCBvcHRzKTtcblx0fSkuZmlsdGVyKGZ1bmN0aW9uICh4KSB7XG5cdFx0cmV0dXJuIHgubGVuZ3RoID4gMDtcblx0fSkuam9pbignJicpIDogJyc7XG59O1xuXG5leHBvcnRzLnBhcnNlVXJsID0gZnVuY3Rpb24gKHN0ciwgb3B0cykge1xuXHRyZXR1cm4ge1xuXHRcdHVybDogc3RyLnNwbGl0KCc/JylbMF0gfHwgJycsXG5cdFx0cXVlcnk6IHBhcnNlKGV4dHJhY3Qoc3RyKSwgb3B0cylcblx0fTtcbn07XG4iLCIndXNlIHN0cmljdCc7XG5cbnZhciBhc3NpZ24gPSByZXF1aXJlKCdvYmplY3QtYXNzaWduJyksXG5cdFByb3BUeXBlcyA9IHJlcXVpcmUoJ3Byb3AtdHlwZXMnKSxcblx0Y3JlYXRlQ2xhc3MgPSByZXF1aXJlKCdjcmVhdGUtcmVhY3QtY2xhc3MnKSxcblx0bW9tZW50ID0gcmVxdWlyZSgnbW9tZW50JyksXG5cdFJlYWN0ID0gcmVxdWlyZSgncmVhY3QnKSxcblx0Q2FsZW5kYXJDb250YWluZXIgPSByZXF1aXJlKCcuL3NyYy9DYWxlbmRhckNvbnRhaW5lcicpXG5cdDtcblxudmFyIHZpZXdNb2RlcyA9IE9iamVjdC5mcmVlemUoe1xuXHRZRUFSUzogJ3llYXJzJyxcblx0TU9OVEhTOiAnbW9udGhzJyxcblx0REFZUzogJ2RheXMnLFxuXHRUSU1FOiAndGltZScsXG59KTtcblxudmFyIFRZUEVTID0gUHJvcFR5cGVzO1xudmFyIERhdGV0aW1lID0gY3JlYXRlQ2xhc3Moe1xuXHRwcm9wVHlwZXM6IHtcblx0XHQvLyB2YWx1ZTogVFlQRVMub2JqZWN0IHwgVFlQRVMuc3RyaW5nLFxuXHRcdC8vIGRlZmF1bHRWYWx1ZTogVFlQRVMub2JqZWN0IHwgVFlQRVMuc3RyaW5nLFxuXHRcdC8vIHZpZXdEYXRlOiBUWVBFUy5vYmplY3QgfCBUWVBFUy5zdHJpbmcsXG5cdFx0b25Gb2N1czogVFlQRVMuZnVuYyxcblx0XHRvbkJsdXI6IFRZUEVTLmZ1bmMsXG5cdFx0b25DaGFuZ2U6IFRZUEVTLmZ1bmMsXG5cdFx0b25WaWV3TW9kZUNoYW5nZTogVFlQRVMuZnVuYyxcblx0XHRsb2NhbGU6IFRZUEVTLnN0cmluZyxcblx0XHR1dGM6IFRZUEVTLmJvb2wsXG5cdFx0aW5wdXQ6IFRZUEVTLmJvb2wsXG5cdFx0Ly8gZGF0ZUZvcm1hdDogVFlQRVMuc3RyaW5nIHwgVFlQRVMuYm9vbCxcblx0XHQvLyB0aW1lRm9ybWF0OiBUWVBFUy5zdHJpbmcgfCBUWVBFUy5ib29sLFxuXHRcdGlucHV0UHJvcHM6IFRZUEVTLm9iamVjdCxcblx0XHR0aW1lQ29uc3RyYWludHM6IFRZUEVTLm9iamVjdCxcblx0XHR2aWV3TW9kZTogVFlQRVMub25lT2YoW3ZpZXdNb2Rlcy5ZRUFSUywgdmlld01vZGVzLk1PTlRIUywgdmlld01vZGVzLkRBWVMsIHZpZXdNb2Rlcy5USU1FXSksXG5cdFx0aXNWYWxpZERhdGU6IFRZUEVTLmZ1bmMsXG5cdFx0b3BlbjogVFlQRVMuYm9vbCxcblx0XHRzdHJpY3RQYXJzaW5nOiBUWVBFUy5ib29sLFxuXHRcdGNsb3NlT25TZWxlY3Q6IFRZUEVTLmJvb2wsXG5cdFx0Y2xvc2VPblRhYjogVFlQRVMuYm9vbFxuXHR9LFxuXG5cdGdldEluaXRpYWxTdGF0ZTogZnVuY3Rpb24oKSB7XG5cdFx0dmFyIHN0YXRlID0gdGhpcy5nZXRTdGF0ZUZyb21Qcm9wcyggdGhpcy5wcm9wcyApO1xuXG5cdFx0aWYgKCBzdGF0ZS5vcGVuID09PSB1bmRlZmluZWQgKVxuXHRcdFx0c3RhdGUub3BlbiA9ICF0aGlzLnByb3BzLmlucHV0O1xuXG5cdFx0c3RhdGUuY3VycmVudFZpZXcgPSB0aGlzLnByb3BzLmRhdGVGb3JtYXQgP1xuXHRcdFx0KHRoaXMucHJvcHMudmlld01vZGUgfHwgc3RhdGUudXBkYXRlT24gfHwgdmlld01vZGVzLkRBWVMpIDogdmlld01vZGVzLlRJTUU7XG5cblx0XHRyZXR1cm4gc3RhdGU7XG5cdH0sXG5cblx0cGFyc2VEYXRlOiBmdW5jdGlvbiAoZGF0ZSwgZm9ybWF0cykge1xuXHRcdHZhciBwYXJzZWREYXRlO1xuXG5cdFx0aWYgKGRhdGUgJiYgdHlwZW9mIGRhdGUgPT09ICdzdHJpbmcnKVxuXHRcdFx0cGFyc2VkRGF0ZSA9IHRoaXMubG9jYWxNb21lbnQoZGF0ZSwgZm9ybWF0cy5kYXRldGltZSk7XG5cdFx0ZWxzZSBpZiAoZGF0ZSlcblx0XHRcdHBhcnNlZERhdGUgPSB0aGlzLmxvY2FsTW9tZW50KGRhdGUpO1xuXG5cdFx0aWYgKHBhcnNlZERhdGUgJiYgIXBhcnNlZERhdGUuaXNWYWxpZCgpKVxuXHRcdFx0cGFyc2VkRGF0ZSA9IG51bGw7XG5cblx0XHRyZXR1cm4gcGFyc2VkRGF0ZTtcblx0fSxcblxuXHRnZXRTdGF0ZUZyb21Qcm9wczogZnVuY3Rpb24oIHByb3BzICkge1xuXHRcdHZhciBmb3JtYXRzID0gdGhpcy5nZXRGb3JtYXRzKCBwcm9wcyApLFxuXHRcdFx0ZGF0ZSA9IHByb3BzLnZhbHVlIHx8IHByb3BzLmRlZmF1bHRWYWx1ZSxcblx0XHRcdHNlbGVjdGVkRGF0ZSwgdmlld0RhdGUsIHVwZGF0ZU9uLCBpbnB1dFZhbHVlXG5cdFx0XHQ7XG5cblx0XHRzZWxlY3RlZERhdGUgPSB0aGlzLnBhcnNlRGF0ZShkYXRlLCBmb3JtYXRzKTtcblxuXHRcdHZpZXdEYXRlID0gdGhpcy5wYXJzZURhdGUocHJvcHMudmlld0RhdGUsIGZvcm1hdHMpO1xuXG5cdFx0dmlld0RhdGUgPSBzZWxlY3RlZERhdGUgP1xuXHRcdFx0c2VsZWN0ZWREYXRlLmNsb25lKCkuc3RhcnRPZignbW9udGgnKSA6XG5cdFx0XHR2aWV3RGF0ZSA/IHZpZXdEYXRlLmNsb25lKCkuc3RhcnRPZignbW9udGgnKSA6IHRoaXMubG9jYWxNb21lbnQoKS5zdGFydE9mKCdtb250aCcpO1xuXG5cdFx0dXBkYXRlT24gPSB0aGlzLmdldFVwZGF0ZU9uKGZvcm1hdHMpO1xuXG5cdFx0aWYgKCBzZWxlY3RlZERhdGUgKVxuXHRcdFx0aW5wdXRWYWx1ZSA9IHNlbGVjdGVkRGF0ZS5mb3JtYXQoZm9ybWF0cy5kYXRldGltZSk7XG5cdFx0ZWxzZSBpZiAoIGRhdGUuaXNWYWxpZCAmJiAhZGF0ZS5pc1ZhbGlkKCkgKVxuXHRcdFx0aW5wdXRWYWx1ZSA9ICcnO1xuXHRcdGVsc2Vcblx0XHRcdGlucHV0VmFsdWUgPSBkYXRlIHx8ICcnO1xuXG5cdFx0cmV0dXJuIHtcblx0XHRcdHVwZGF0ZU9uOiB1cGRhdGVPbixcblx0XHRcdGlucHV0Rm9ybWF0OiBmb3JtYXRzLmRhdGV0aW1lLFxuXHRcdFx0dmlld0RhdGU6IHZpZXdEYXRlLFxuXHRcdFx0c2VsZWN0ZWREYXRlOiBzZWxlY3RlZERhdGUsXG5cdFx0XHRpbnB1dFZhbHVlOiBpbnB1dFZhbHVlLFxuXHRcdFx0b3BlbjogcHJvcHMub3BlblxuXHRcdH07XG5cdH0sXG5cblx0Z2V0VXBkYXRlT246IGZ1bmN0aW9uKCBmb3JtYXRzICkge1xuXHRcdGlmICggZm9ybWF0cy5kYXRlLm1hdGNoKC9bbExEXS8pICkge1xuXHRcdFx0cmV0dXJuIHZpZXdNb2Rlcy5EQVlTO1xuXHRcdH0gZWxzZSBpZiAoIGZvcm1hdHMuZGF0ZS5pbmRleE9mKCdNJykgIT09IC0xICkge1xuXHRcdFx0cmV0dXJuIHZpZXdNb2Rlcy5NT05USFM7XG5cdFx0fSBlbHNlIGlmICggZm9ybWF0cy5kYXRlLmluZGV4T2YoJ1knKSAhPT0gLTEgKSB7XG5cdFx0XHRyZXR1cm4gdmlld01vZGVzLllFQVJTO1xuXHRcdH1cblxuXHRcdHJldHVybiB2aWV3TW9kZXMuREFZUztcblx0fSxcblxuXHRnZXRGb3JtYXRzOiBmdW5jdGlvbiggcHJvcHMgKSB7XG5cdFx0dmFyIGZvcm1hdHMgPSB7XG5cdFx0XHRcdGRhdGU6IHByb3BzLmRhdGVGb3JtYXQgfHwgJycsXG5cdFx0XHRcdHRpbWU6IHByb3BzLnRpbWVGb3JtYXQgfHwgJydcblx0XHRcdH0sXG5cdFx0XHRsb2NhbGUgPSB0aGlzLmxvY2FsTW9tZW50KCBwcm9wcy5kYXRlLCBudWxsLCBwcm9wcyApLmxvY2FsZURhdGEoKVxuXHRcdFx0O1xuXG5cdFx0aWYgKCBmb3JtYXRzLmRhdGUgPT09IHRydWUgKSB7XG5cdFx0XHRmb3JtYXRzLmRhdGUgPSBsb2NhbGUubG9uZ0RhdGVGb3JtYXQoJ0wnKTtcblx0XHR9XG5cdFx0ZWxzZSBpZiAoIHRoaXMuZ2V0VXBkYXRlT24oZm9ybWF0cykgIT09IHZpZXdNb2Rlcy5EQVlTICkge1xuXHRcdFx0Zm9ybWF0cy50aW1lID0gJyc7XG5cdFx0fVxuXG5cdFx0aWYgKCBmb3JtYXRzLnRpbWUgPT09IHRydWUgKSB7XG5cdFx0XHRmb3JtYXRzLnRpbWUgPSBsb2NhbGUubG9uZ0RhdGVGb3JtYXQoJ0xUJyk7XG5cdFx0fVxuXG5cdFx0Zm9ybWF0cy5kYXRldGltZSA9IGZvcm1hdHMuZGF0ZSAmJiBmb3JtYXRzLnRpbWUgP1xuXHRcdFx0Zm9ybWF0cy5kYXRlICsgJyAnICsgZm9ybWF0cy50aW1lIDpcblx0XHRcdGZvcm1hdHMuZGF0ZSB8fCBmb3JtYXRzLnRpbWVcblx0XHQ7XG5cblx0XHRyZXR1cm4gZm9ybWF0cztcblx0fSxcblxuXHRjb21wb25lbnRXaWxsUmVjZWl2ZVByb3BzOiBmdW5jdGlvbiggbmV4dFByb3BzICkge1xuXHRcdHZhciBmb3JtYXRzID0gdGhpcy5nZXRGb3JtYXRzKCBuZXh0UHJvcHMgKSxcblx0XHRcdHVwZGF0ZWRTdGF0ZSA9IHt9XG5cdFx0O1xuXG5cdFx0aWYgKCBuZXh0UHJvcHMudmFsdWUgIT09IHRoaXMucHJvcHMudmFsdWUgfHxcblx0XHRcdGZvcm1hdHMuZGF0ZXRpbWUgIT09IHRoaXMuZ2V0Rm9ybWF0cyggdGhpcy5wcm9wcyApLmRhdGV0aW1lICkge1xuXHRcdFx0dXBkYXRlZFN0YXRlID0gdGhpcy5nZXRTdGF0ZUZyb21Qcm9wcyggbmV4dFByb3BzICk7XG5cdFx0fVxuXG5cdFx0aWYgKCB1cGRhdGVkU3RhdGUub3BlbiA9PT0gdW5kZWZpbmVkICkge1xuXHRcdFx0aWYgKCB0eXBlb2YgbmV4dFByb3BzLm9wZW4gIT09ICd1bmRlZmluZWQnICkge1xuXHRcdFx0XHR1cGRhdGVkU3RhdGUub3BlbiA9IG5leHRQcm9wcy5vcGVuO1xuXHRcdFx0fSBlbHNlIGlmICggdGhpcy5wcm9wcy5jbG9zZU9uU2VsZWN0ICYmIHRoaXMuc3RhdGUuY3VycmVudFZpZXcgIT09IHZpZXdNb2Rlcy5USU1FICkge1xuXHRcdFx0XHR1cGRhdGVkU3RhdGUub3BlbiA9IGZhbHNlO1xuXHRcdFx0fSBlbHNlIHtcblx0XHRcdFx0dXBkYXRlZFN0YXRlLm9wZW4gPSB0aGlzLnN0YXRlLm9wZW47XG5cdFx0XHR9XG5cdFx0fVxuXG5cdFx0aWYgKCBuZXh0UHJvcHMudmlld01vZGUgIT09IHRoaXMucHJvcHMudmlld01vZGUgKSB7XG5cdFx0XHR1cGRhdGVkU3RhdGUuY3VycmVudFZpZXcgPSBuZXh0UHJvcHMudmlld01vZGU7XG5cdFx0fVxuXG5cdFx0aWYgKCBuZXh0UHJvcHMubG9jYWxlICE9PSB0aGlzLnByb3BzLmxvY2FsZSApIHtcblx0XHRcdGlmICggdGhpcy5zdGF0ZS52aWV3RGF0ZSApIHtcblx0XHRcdFx0dmFyIHVwZGF0ZWRWaWV3RGF0ZSA9IHRoaXMuc3RhdGUudmlld0RhdGUuY2xvbmUoKS5sb2NhbGUoIG5leHRQcm9wcy5sb2NhbGUgKTtcblx0XHRcdFx0dXBkYXRlZFN0YXRlLnZpZXdEYXRlID0gdXBkYXRlZFZpZXdEYXRlO1xuXHRcdFx0fVxuXHRcdFx0aWYgKCB0aGlzLnN0YXRlLnNlbGVjdGVkRGF0ZSApIHtcblx0XHRcdFx0dmFyIHVwZGF0ZWRTZWxlY3RlZERhdGUgPSB0aGlzLnN0YXRlLnNlbGVjdGVkRGF0ZS5jbG9uZSgpLmxvY2FsZSggbmV4dFByb3BzLmxvY2FsZSApO1xuXHRcdFx0XHR1cGRhdGVkU3RhdGUuc2VsZWN0ZWREYXRlID0gdXBkYXRlZFNlbGVjdGVkRGF0ZTtcblx0XHRcdFx0dXBkYXRlZFN0YXRlLmlucHV0VmFsdWUgPSB1cGRhdGVkU2VsZWN0ZWREYXRlLmZvcm1hdCggZm9ybWF0cy5kYXRldGltZSApO1xuXHRcdFx0fVxuXHRcdH1cblxuXHRcdGlmICggbmV4dFByb3BzLnV0YyAhPT0gdGhpcy5wcm9wcy51dGMgKSB7XG5cdFx0XHRpZiAoIG5leHRQcm9wcy51dGMgKSB7XG5cdFx0XHRcdGlmICggdGhpcy5zdGF0ZS52aWV3RGF0ZSApXG5cdFx0XHRcdFx0dXBkYXRlZFN0YXRlLnZpZXdEYXRlID0gdGhpcy5zdGF0ZS52aWV3RGF0ZS5jbG9uZSgpLnV0YygpO1xuXHRcdFx0XHRpZiAoIHRoaXMuc3RhdGUuc2VsZWN0ZWREYXRlICkge1xuXHRcdFx0XHRcdHVwZGF0ZWRTdGF0ZS5zZWxlY3RlZERhdGUgPSB0aGlzLnN0YXRlLnNlbGVjdGVkRGF0ZS5jbG9uZSgpLnV0YygpO1xuXHRcdFx0XHRcdHVwZGF0ZWRTdGF0ZS5pbnB1dFZhbHVlID0gdXBkYXRlZFN0YXRlLnNlbGVjdGVkRGF0ZS5mb3JtYXQoIGZvcm1hdHMuZGF0ZXRpbWUgKTtcblx0XHRcdFx0fVxuXHRcdFx0fSBlbHNlIHtcblx0XHRcdFx0aWYgKCB0aGlzLnN0YXRlLnZpZXdEYXRlIClcblx0XHRcdFx0XHR1cGRhdGVkU3RhdGUudmlld0RhdGUgPSB0aGlzLnN0YXRlLnZpZXdEYXRlLmNsb25lKCkubG9jYWwoKTtcblx0XHRcdFx0aWYgKCB0aGlzLnN0YXRlLnNlbGVjdGVkRGF0ZSApIHtcblx0XHRcdFx0XHR1cGRhdGVkU3RhdGUuc2VsZWN0ZWREYXRlID0gdGhpcy5zdGF0ZS5zZWxlY3RlZERhdGUuY2xvbmUoKS5sb2NhbCgpO1xuXHRcdFx0XHRcdHVwZGF0ZWRTdGF0ZS5pbnB1dFZhbHVlID0gdXBkYXRlZFN0YXRlLnNlbGVjdGVkRGF0ZS5mb3JtYXQoZm9ybWF0cy5kYXRldGltZSk7XG5cdFx0XHRcdH1cblx0XHRcdH1cblx0XHR9XG5cblx0XHRpZiAoIG5leHRQcm9wcy52aWV3RGF0ZSAhPT0gdGhpcy5wcm9wcy52aWV3RGF0ZSApIHtcblx0XHRcdHVwZGF0ZWRTdGF0ZS52aWV3RGF0ZSA9IG1vbWVudChuZXh0UHJvcHMudmlld0RhdGUpO1xuXHRcdH1cblx0XHQvL3dlIHNob3VsZCBvbmx5IHNob3cgYSB2YWxpZCBkYXRlIGlmIHdlIGFyZSBwcm92aWRlZCBhIGlzVmFsaWREYXRlIGZ1bmN0aW9uLiBSZW1vdmVkIGluIDIuMTAuM1xuXHRcdC8qaWYgKHRoaXMucHJvcHMuaXNWYWxpZERhdGUpIHtcblx0XHRcdHVwZGF0ZWRTdGF0ZS52aWV3RGF0ZSA9IHVwZGF0ZWRTdGF0ZS52aWV3RGF0ZSB8fCB0aGlzLnN0YXRlLnZpZXdEYXRlO1xuXHRcdFx0d2hpbGUgKCF0aGlzLnByb3BzLmlzVmFsaWREYXRlKHVwZGF0ZWRTdGF0ZS52aWV3RGF0ZSkpIHtcblx0XHRcdFx0dXBkYXRlZFN0YXRlLnZpZXdEYXRlID0gdXBkYXRlZFN0YXRlLnZpZXdEYXRlLmFkZCgxLCAnZGF5Jyk7XG5cdFx0XHR9XG5cdFx0fSovXG5cdFx0dGhpcy5zZXRTdGF0ZSggdXBkYXRlZFN0YXRlICk7XG5cdH0sXG5cblx0b25JbnB1dENoYW5nZTogZnVuY3Rpb24oIGUgKSB7XG5cdFx0dmFyIHZhbHVlID0gZS50YXJnZXQgPT09IG51bGwgPyBlIDogZS50YXJnZXQudmFsdWUsXG5cdFx0XHRsb2NhbE1vbWVudCA9IHRoaXMubG9jYWxNb21lbnQoIHZhbHVlLCB0aGlzLnN0YXRlLmlucHV0Rm9ybWF0ICksXG5cdFx0XHR1cGRhdGUgPSB7IGlucHV0VmFsdWU6IHZhbHVlIH1cblx0XHRcdDtcblxuXHRcdGlmICggbG9jYWxNb21lbnQuaXNWYWxpZCgpICYmICF0aGlzLnByb3BzLnZhbHVlICkge1xuXHRcdFx0dXBkYXRlLnNlbGVjdGVkRGF0ZSA9IGxvY2FsTW9tZW50O1xuXHRcdFx0dXBkYXRlLnZpZXdEYXRlID0gbG9jYWxNb21lbnQuY2xvbmUoKS5zdGFydE9mKCdtb250aCcpO1xuXHRcdH0gZWxzZSB7XG5cdFx0XHR1cGRhdGUuc2VsZWN0ZWREYXRlID0gbnVsbDtcblx0XHR9XG5cblx0XHRyZXR1cm4gdGhpcy5zZXRTdGF0ZSggdXBkYXRlLCBmdW5jdGlvbigpIHtcblx0XHRcdHJldHVybiB0aGlzLnByb3BzLm9uQ2hhbmdlKCBsb2NhbE1vbWVudC5pc1ZhbGlkKCkgPyBsb2NhbE1vbWVudCA6IHRoaXMuc3RhdGUuaW5wdXRWYWx1ZSApO1xuXHRcdH0pO1xuXHR9LFxuXG5cdG9uSW5wdXRLZXk6IGZ1bmN0aW9uKCBlICkge1xuXHRcdGlmICggZS53aGljaCA9PT0gOSAmJiB0aGlzLnByb3BzLmNsb3NlT25UYWIgKSB7XG5cdFx0XHR0aGlzLmNsb3NlQ2FsZW5kYXIoKTtcblx0XHR9XG5cdH0sXG5cblx0c2hvd1ZpZXc6IGZ1bmN0aW9uKCB2aWV3ICkge1xuXHRcdHZhciBtZSA9IHRoaXM7XG5cdFx0cmV0dXJuIGZ1bmN0aW9uKCkge1xuXHRcdFx0bWUuc3RhdGUuY3VycmVudFZpZXcgIT09IHZpZXcgJiYgbWUucHJvcHMub25WaWV3TW9kZUNoYW5nZSggdmlldyApO1xuXHRcdFx0bWUuc2V0U3RhdGUoeyBjdXJyZW50VmlldzogdmlldyB9KTtcblx0XHR9O1xuXHR9LFxuXG5cdHNldERhdGU6IGZ1bmN0aW9uKCB0eXBlICkge1xuXHRcdHZhciBtZSA9IHRoaXMsXG5cdFx0XHRuZXh0Vmlld3MgPSB7XG5cdFx0XHRcdG1vbnRoOiB2aWV3TW9kZXMuREFZUyxcblx0XHRcdFx0eWVhcjogdmlld01vZGVzLk1PTlRIUyxcblx0XHRcdH1cblx0XHQ7XG5cdFx0cmV0dXJuIGZ1bmN0aW9uKCBlICkge1xuXHRcdFx0bWUuc2V0U3RhdGUoe1xuXHRcdFx0XHR2aWV3RGF0ZTogbWUuc3RhdGUudmlld0RhdGUuY2xvbmUoKVsgdHlwZSBdKCBwYXJzZUludChlLnRhcmdldC5nZXRBdHRyaWJ1dGUoJ2RhdGEtdmFsdWUnKSwgMTApICkuc3RhcnRPZiggdHlwZSApLFxuXHRcdFx0XHRjdXJyZW50VmlldzogbmV4dFZpZXdzWyB0eXBlIF1cblx0XHRcdH0pO1xuXHRcdFx0bWUucHJvcHMub25WaWV3TW9kZUNoYW5nZSggbmV4dFZpZXdzWyB0eXBlIF0gKTtcblx0XHR9O1xuXHR9LFxuXG5cdGFkZFRpbWU6IGZ1bmN0aW9uKCBhbW91bnQsIHR5cGUsIHRvU2VsZWN0ZWQgKSB7XG5cdFx0cmV0dXJuIHRoaXMudXBkYXRlVGltZSggJ2FkZCcsIGFtb3VudCwgdHlwZSwgdG9TZWxlY3RlZCApO1xuXHR9LFxuXG5cdHN1YnRyYWN0VGltZTogZnVuY3Rpb24oIGFtb3VudCwgdHlwZSwgdG9TZWxlY3RlZCApIHtcblx0XHRyZXR1cm4gdGhpcy51cGRhdGVUaW1lKCAnc3VidHJhY3QnLCBhbW91bnQsIHR5cGUsIHRvU2VsZWN0ZWQgKTtcblx0fSxcblxuXHR1cGRhdGVUaW1lOiBmdW5jdGlvbiggb3AsIGFtb3VudCwgdHlwZSwgdG9TZWxlY3RlZCApIHtcblx0XHR2YXIgbWUgPSB0aGlzO1xuXG5cdFx0cmV0dXJuIGZ1bmN0aW9uKCkge1xuXHRcdFx0dmFyIHVwZGF0ZSA9IHt9LFxuXHRcdFx0XHRkYXRlID0gdG9TZWxlY3RlZCA/ICdzZWxlY3RlZERhdGUnIDogJ3ZpZXdEYXRlJ1xuXHRcdFx0O1xuXG5cdFx0XHR1cGRhdGVbIGRhdGUgXSA9IG1lLnN0YXRlWyBkYXRlIF0uY2xvbmUoKVsgb3AgXSggYW1vdW50LCB0eXBlICk7XG5cblx0XHRcdG1lLnNldFN0YXRlKCB1cGRhdGUgKTtcblx0XHR9O1xuXHR9LFxuXG5cdGFsbG93ZWRTZXRUaW1lOiBbJ2hvdXJzJywgJ21pbnV0ZXMnLCAnc2Vjb25kcycsICdtaWxsaXNlY29uZHMnXSxcblx0c2V0VGltZTogZnVuY3Rpb24oIHR5cGUsIHZhbHVlICkge1xuXHRcdHZhciBpbmRleCA9IHRoaXMuYWxsb3dlZFNldFRpbWUuaW5kZXhPZiggdHlwZSApICsgMSxcblx0XHRcdHN0YXRlID0gdGhpcy5zdGF0ZSxcblx0XHRcdGRhdGUgPSAoc3RhdGUuc2VsZWN0ZWREYXRlIHx8IHN0YXRlLnZpZXdEYXRlKS5jbG9uZSgpLFxuXHRcdFx0bmV4dFR5cGVcblx0XHRcdDtcblxuXHRcdC8vIEl0IGlzIG5lZWRlZCB0byBzZXQgYWxsIHRoZSB0aW1lIHByb3BlcnRpZXNcblx0XHQvLyB0byBub3QgdG8gcmVzZXQgdGhlIHRpbWVcblx0XHRkYXRlWyB0eXBlIF0oIHZhbHVlICk7XG5cdFx0Zm9yICg7IGluZGV4IDwgdGhpcy5hbGxvd2VkU2V0VGltZS5sZW5ndGg7IGluZGV4KyspIHtcblx0XHRcdG5leHRUeXBlID0gdGhpcy5hbGxvd2VkU2V0VGltZVtpbmRleF07XG5cdFx0XHRkYXRlWyBuZXh0VHlwZSBdKCBkYXRlW25leHRUeXBlXSgpICk7XG5cdFx0fVxuXG5cdFx0aWYgKCAhdGhpcy5wcm9wcy52YWx1ZSApIHtcblx0XHRcdHRoaXMuc2V0U3RhdGUoe1xuXHRcdFx0XHRzZWxlY3RlZERhdGU6IGRhdGUsXG5cdFx0XHRcdGlucHV0VmFsdWU6IGRhdGUuZm9ybWF0KCBzdGF0ZS5pbnB1dEZvcm1hdCApXG5cdFx0XHR9KTtcblx0XHR9XG5cdFx0dGhpcy5wcm9wcy5vbkNoYW5nZSggZGF0ZSApO1xuXHR9LFxuXG5cdHVwZGF0ZVNlbGVjdGVkRGF0ZTogZnVuY3Rpb24oIGUsIGNsb3NlICkge1xuXHRcdHZhciB0YXJnZXQgPSBlLnRhcmdldCxcblx0XHRcdG1vZGlmaWVyID0gMCxcblx0XHRcdHZpZXdEYXRlID0gdGhpcy5zdGF0ZS52aWV3RGF0ZSxcblx0XHRcdGN1cnJlbnREYXRlID0gdGhpcy5zdGF0ZS5zZWxlY3RlZERhdGUgfHwgdmlld0RhdGUsXG5cdFx0XHRkYXRlXG5cdFx0XHQ7XG5cblx0XHRpZiAodGFyZ2V0LmNsYXNzTmFtZS5pbmRleE9mKCdyZHREYXknKSAhPT0gLTEpIHtcblx0XHRcdGlmICh0YXJnZXQuY2xhc3NOYW1lLmluZGV4T2YoJ3JkdE5ldycpICE9PSAtMSlcblx0XHRcdFx0bW9kaWZpZXIgPSAxO1xuXHRcdFx0ZWxzZSBpZiAodGFyZ2V0LmNsYXNzTmFtZS5pbmRleE9mKCdyZHRPbGQnKSAhPT0gLTEpXG5cdFx0XHRcdG1vZGlmaWVyID0gLTE7XG5cblx0XHRcdGRhdGUgPSB2aWV3RGF0ZS5jbG9uZSgpXG5cdFx0XHRcdC5tb250aCggdmlld0RhdGUubW9udGgoKSArIG1vZGlmaWVyIClcblx0XHRcdFx0LmRhdGUoIHBhcnNlSW50KCB0YXJnZXQuZ2V0QXR0cmlidXRlKCdkYXRhLXZhbHVlJyksIDEwICkgKTtcblx0XHR9IGVsc2UgaWYgKHRhcmdldC5jbGFzc05hbWUuaW5kZXhPZigncmR0TW9udGgnKSAhPT0gLTEpIHtcblx0XHRcdGRhdGUgPSB2aWV3RGF0ZS5jbG9uZSgpXG5cdFx0XHRcdC5tb250aCggcGFyc2VJbnQoIHRhcmdldC5nZXRBdHRyaWJ1dGUoJ2RhdGEtdmFsdWUnKSwgMTAgKSApXG5cdFx0XHRcdC5kYXRlKCBjdXJyZW50RGF0ZS5kYXRlKCkgKTtcblx0XHR9IGVsc2UgaWYgKHRhcmdldC5jbGFzc05hbWUuaW5kZXhPZigncmR0WWVhcicpICE9PSAtMSkge1xuXHRcdFx0ZGF0ZSA9IHZpZXdEYXRlLmNsb25lKClcblx0XHRcdFx0Lm1vbnRoKCBjdXJyZW50RGF0ZS5tb250aCgpIClcblx0XHRcdFx0LmRhdGUoIGN1cnJlbnREYXRlLmRhdGUoKSApXG5cdFx0XHRcdC55ZWFyKCBwYXJzZUludCggdGFyZ2V0LmdldEF0dHJpYnV0ZSgnZGF0YS12YWx1ZScpLCAxMCApICk7XG5cdFx0fVxuXG5cdFx0ZGF0ZS5ob3VycyggY3VycmVudERhdGUuaG91cnMoKSApXG5cdFx0XHQubWludXRlcyggY3VycmVudERhdGUubWludXRlcygpIClcblx0XHRcdC5zZWNvbmRzKCBjdXJyZW50RGF0ZS5zZWNvbmRzKCkgKVxuXHRcdFx0Lm1pbGxpc2Vjb25kcyggY3VycmVudERhdGUubWlsbGlzZWNvbmRzKCkgKTtcblxuXHRcdGlmICggIXRoaXMucHJvcHMudmFsdWUgKSB7XG5cdFx0XHR2YXIgb3BlbiA9ICEoIHRoaXMucHJvcHMuY2xvc2VPblNlbGVjdCAmJiBjbG9zZSApO1xuXHRcdFx0aWYgKCAhb3BlbiApIHtcblx0XHRcdFx0dGhpcy5wcm9wcy5vbkJsdXIoIGRhdGUgKTtcblx0XHRcdH1cblxuXHRcdFx0dGhpcy5zZXRTdGF0ZSh7XG5cdFx0XHRcdHNlbGVjdGVkRGF0ZTogZGF0ZSxcblx0XHRcdFx0dmlld0RhdGU6IGRhdGUuY2xvbmUoKS5zdGFydE9mKCdtb250aCcpLFxuXHRcdFx0XHRpbnB1dFZhbHVlOiBkYXRlLmZvcm1hdCggdGhpcy5zdGF0ZS5pbnB1dEZvcm1hdCApLFxuXHRcdFx0XHRvcGVuOiBvcGVuXG5cdFx0XHR9KTtcblx0XHR9IGVsc2Uge1xuXHRcdFx0aWYgKCB0aGlzLnByb3BzLmNsb3NlT25TZWxlY3QgJiYgY2xvc2UgKSB7XG5cdFx0XHRcdHRoaXMuY2xvc2VDYWxlbmRhcigpO1xuXHRcdFx0fVxuXHRcdH1cblxuXHRcdHRoaXMucHJvcHMub25DaGFuZ2UoIGRhdGUgKTtcblx0fSxcblxuXHRvcGVuQ2FsZW5kYXI6IGZ1bmN0aW9uKCBlICkge1xuXHRcdGlmICggIXRoaXMuc3RhdGUub3BlbiApIHtcblx0XHRcdHRoaXMuc2V0U3RhdGUoeyBvcGVuOiB0cnVlIH0sIGZ1bmN0aW9uKCkge1xuXHRcdFx0XHR0aGlzLnByb3BzLm9uRm9jdXMoIGUgKTtcblx0XHRcdH0pO1xuXHRcdH1cblx0fSxcblxuXHRjbG9zZUNhbGVuZGFyOiBmdW5jdGlvbigpIHtcblx0XHR0aGlzLnNldFN0YXRlKHsgb3BlbjogZmFsc2UgfSwgZnVuY3Rpb24gKCkge1xuXHRcdFx0dGhpcy5wcm9wcy5vbkJsdXIoIHRoaXMuc3RhdGUuc2VsZWN0ZWREYXRlIHx8IHRoaXMuc3RhdGUuaW5wdXRWYWx1ZSApO1xuXHRcdH0pO1xuXHR9LFxuXG5cdGhhbmRsZUNsaWNrT3V0c2lkZTogZnVuY3Rpb24oKSB7XG5cdFx0aWYgKCB0aGlzLnByb3BzLmlucHV0ICYmIHRoaXMuc3RhdGUub3BlbiAmJiAhdGhpcy5wcm9wcy5vcGVuICYmICF0aGlzLnByb3BzLmRpc2FibGVPbkNsaWNrT3V0c2lkZSApIHtcblx0XHRcdHRoaXMuc2V0U3RhdGUoeyBvcGVuOiBmYWxzZSB9LCBmdW5jdGlvbigpIHtcblx0XHRcdFx0dGhpcy5wcm9wcy5vbkJsdXIoIHRoaXMuc3RhdGUuc2VsZWN0ZWREYXRlIHx8IHRoaXMuc3RhdGUuaW5wdXRWYWx1ZSApO1xuXHRcdFx0fSk7XG5cdFx0fVxuXHR9LFxuXG5cdGxvY2FsTW9tZW50OiBmdW5jdGlvbiggZGF0ZSwgZm9ybWF0LCBwcm9wcyApIHtcblx0XHRwcm9wcyA9IHByb3BzIHx8IHRoaXMucHJvcHM7XG5cdFx0dmFyIG1vbWVudEZuID0gcHJvcHMudXRjID8gbW9tZW50LnV0YyA6IG1vbWVudDtcblx0XHR2YXIgbSA9IG1vbWVudEZuKCBkYXRlLCBmb3JtYXQsIHByb3BzLnN0cmljdFBhcnNpbmcgKTtcblx0XHRpZiAoIHByb3BzLmxvY2FsZSApXG5cdFx0XHRtLmxvY2FsZSggcHJvcHMubG9jYWxlICk7XG5cdFx0cmV0dXJuIG07XG5cdH0sXG5cblx0Y29tcG9uZW50UHJvcHM6IHtcblx0XHRmcm9tUHJvcHM6IFsndmFsdWUnLCAnaXNWYWxpZERhdGUnLCAncmVuZGVyRGF5JywgJ3JlbmRlck1vbnRoJywgJ3JlbmRlclllYXInLCAndGltZUNvbnN0cmFpbnRzJ10sXG5cdFx0ZnJvbVN0YXRlOiBbJ3ZpZXdEYXRlJywgJ3NlbGVjdGVkRGF0ZScsICd1cGRhdGVPbiddLFxuXHRcdGZyb21UaGlzOiBbJ3NldERhdGUnLCAnc2V0VGltZScsICdzaG93VmlldycsICdhZGRUaW1lJywgJ3N1YnRyYWN0VGltZScsICd1cGRhdGVTZWxlY3RlZERhdGUnLCAnbG9jYWxNb21lbnQnLCAnaGFuZGxlQ2xpY2tPdXRzaWRlJ11cblx0fSxcblxuXHRnZXRDb21wb25lbnRQcm9wczogZnVuY3Rpb24oKSB7XG5cdFx0dmFyIG1lID0gdGhpcyxcblx0XHRcdGZvcm1hdHMgPSB0aGlzLmdldEZvcm1hdHMoIHRoaXMucHJvcHMgKSxcblx0XHRcdHByb3BzID0ge2RhdGVGb3JtYXQ6IGZvcm1hdHMuZGF0ZSwgdGltZUZvcm1hdDogZm9ybWF0cy50aW1lfVxuXHRcdFx0O1xuXG5cdFx0dGhpcy5jb21wb25lbnRQcm9wcy5mcm9tUHJvcHMuZm9yRWFjaCggZnVuY3Rpb24oIG5hbWUgKSB7XG5cdFx0XHRwcm9wc1sgbmFtZSBdID0gbWUucHJvcHNbIG5hbWUgXTtcblx0XHR9KTtcblx0XHR0aGlzLmNvbXBvbmVudFByb3BzLmZyb21TdGF0ZS5mb3JFYWNoKCBmdW5jdGlvbiggbmFtZSApIHtcblx0XHRcdHByb3BzWyBuYW1lIF0gPSBtZS5zdGF0ZVsgbmFtZSBdO1xuXHRcdH0pO1xuXHRcdHRoaXMuY29tcG9uZW50UHJvcHMuZnJvbVRoaXMuZm9yRWFjaCggZnVuY3Rpb24oIG5hbWUgKSB7XG5cdFx0XHRwcm9wc1sgbmFtZSBdID0gbWVbIG5hbWUgXTtcblx0XHR9KTtcblxuXHRcdHJldHVybiBwcm9wcztcblx0fSxcblxuXHRyZW5kZXI6IGZ1bmN0aW9uKCkge1xuXHRcdC8vIFRPRE86IE1ha2UgYSBmdW5jdGlvbiBvciBjbGVhbiB1cCB0aGlzIGNvZGUsXG5cdFx0Ly8gbG9naWMgcmlnaHQgbm93IGlzIHJlYWxseSBoYXJkIHRvIGZvbGxvd1xuXHRcdHZhciBjbGFzc05hbWUgPSAncmR0JyArICh0aGlzLnByb3BzLmNsYXNzTmFtZSA/XG4gICAgICAgICAgICAgICAgICAoIEFycmF5LmlzQXJyYXkoIHRoaXMucHJvcHMuY2xhc3NOYW1lICkgP1xuICAgICAgICAgICAgICAgICAgJyAnICsgdGhpcy5wcm9wcy5jbGFzc05hbWUuam9pbiggJyAnICkgOiAnICcgKyB0aGlzLnByb3BzLmNsYXNzTmFtZSkgOiAnJyksXG5cdFx0XHRjaGlsZHJlbiA9IFtdO1xuXG5cdFx0aWYgKCB0aGlzLnByb3BzLmlucHV0ICkge1xuXHRcdFx0dmFyIGZpbmFsSW5wdXRQcm9wcyA9IGFzc2lnbih7XG5cdFx0XHRcdHR5cGU6ICd0ZXh0Jyxcblx0XHRcdFx0Y2xhc3NOYW1lOiAnZm9ybS1jb250cm9sJyxcblx0XHRcdFx0b25DbGljazogdGhpcy5vcGVuQ2FsZW5kYXIsXG5cdFx0XHRcdG9uRm9jdXM6IHRoaXMub3BlbkNhbGVuZGFyLFxuXHRcdFx0XHRvbkNoYW5nZTogdGhpcy5vbklucHV0Q2hhbmdlLFxuXHRcdFx0XHRvbktleURvd246IHRoaXMub25JbnB1dEtleSxcblx0XHRcdFx0dmFsdWU6IHRoaXMuc3RhdGUuaW5wdXRWYWx1ZSxcblx0XHRcdH0sIHRoaXMucHJvcHMuaW5wdXRQcm9wcyk7XG5cdFx0XHRpZiAoIHRoaXMucHJvcHMucmVuZGVySW5wdXQgKSB7XG5cdFx0XHRcdGNoaWxkcmVuID0gWyBSZWFjdC5jcmVhdGVFbGVtZW50KCdkaXYnLCB7IGtleTogJ2knIH0sIHRoaXMucHJvcHMucmVuZGVySW5wdXQoIGZpbmFsSW5wdXRQcm9wcywgdGhpcy5vcGVuQ2FsZW5kYXIsIHRoaXMuY2xvc2VDYWxlbmRhciApKSBdO1xuXHRcdFx0fSBlbHNlIHtcblx0XHRcdFx0Y2hpbGRyZW4gPSBbIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ2lucHV0JywgYXNzaWduKHsga2V5OiAnaScgfSwgZmluYWxJbnB1dFByb3BzICkpXTtcblx0XHRcdH1cblx0XHR9IGVsc2Uge1xuXHRcdFx0Y2xhc3NOYW1lICs9ICcgcmR0U3RhdGljJztcblx0XHR9XG5cblx0XHRpZiAoIHRoaXMuc3RhdGUub3BlbiApXG5cdFx0XHRjbGFzc05hbWUgKz0gJyByZHRPcGVuJztcblxuXHRcdHJldHVybiBSZWFjdC5jcmVhdGVFbGVtZW50KCAnZGl2JywgeyBjbGFzc05hbWU6IGNsYXNzTmFtZSB9LCBjaGlsZHJlbi5jb25jYXQoXG5cdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCAnZGl2Jyxcblx0XHRcdFx0eyBrZXk6ICdkdCcsIGNsYXNzTmFtZTogJ3JkdFBpY2tlcicgfSxcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCggQ2FsZW5kYXJDb250YWluZXIsIHsgdmlldzogdGhpcy5zdGF0ZS5jdXJyZW50Vmlldywgdmlld1Byb3BzOiB0aGlzLmdldENvbXBvbmVudFByb3BzKCksIG9uQ2xpY2tPdXRzaWRlOiB0aGlzLmhhbmRsZUNsaWNrT3V0c2lkZSB9KVxuXHRcdFx0KVxuXHRcdCkpO1xuXHR9XG59KTtcblxuRGF0ZXRpbWUuZGVmYXVsdFByb3BzID0ge1xuXHRjbGFzc05hbWU6ICcnLFxuXHRkZWZhdWx0VmFsdWU6ICcnLFxuXHRpbnB1dFByb3BzOiB7fSxcblx0aW5wdXQ6IHRydWUsXG5cdG9uRm9jdXM6IGZ1bmN0aW9uKCkge30sXG5cdG9uQmx1cjogZnVuY3Rpb24oKSB7fSxcblx0b25DaGFuZ2U6IGZ1bmN0aW9uKCkge30sXG5cdG9uVmlld01vZGVDaGFuZ2U6IGZ1bmN0aW9uKCkge30sXG5cdHRpbWVGb3JtYXQ6IHRydWUsXG5cdHRpbWVDb25zdHJhaW50czoge30sXG5cdGRhdGVGb3JtYXQ6IHRydWUsXG5cdHN0cmljdFBhcnNpbmc6IHRydWUsXG5cdGNsb3NlT25TZWxlY3Q6IGZhbHNlLFxuXHRjbG9zZU9uVGFiOiB0cnVlLFxuXHR1dGM6IGZhbHNlXG59O1xuXG4vLyBNYWtlIG1vbWVudCBhY2Nlc3NpYmxlIHRocm91Z2ggdGhlIERhdGV0aW1lIGNsYXNzXG5EYXRldGltZS5tb21lbnQgPSBtb21lbnQ7XG5cbm1vZHVsZS5leHBvcnRzID0gRGF0ZXRpbWU7XG4iLCJcbnZhciBjb250ZW50ID0gcmVxdWlyZShcIiEhLi4vLi4vY3NzLWxvYWRlci9kaXN0L2Nqcy5qcyEuL3JlYWN0LWRhdGV0aW1lLmNzc1wiKTtcblxuaWYodHlwZW9mIGNvbnRlbnQgPT09ICdzdHJpbmcnKSBjb250ZW50ID0gW1ttb2R1bGUuaWQsIGNvbnRlbnQsICcnXV07XG5cbnZhciB0cmFuc2Zvcm07XG52YXIgaW5zZXJ0SW50bztcblxuXG5cbnZhciBvcHRpb25zID0ge1wiaG1yXCI6dHJ1ZX1cblxub3B0aW9ucy50cmFuc2Zvcm0gPSB0cmFuc2Zvcm1cbm9wdGlvbnMuaW5zZXJ0SW50byA9IHVuZGVmaW5lZDtcblxudmFyIHVwZGF0ZSA9IHJlcXVpcmUoXCIhLi4vLi4vc3R5bGUtbG9hZGVyL2xpYi9hZGRTdHlsZXMuanNcIikoY29udGVudCwgb3B0aW9ucyk7XG5cbmlmKGNvbnRlbnQubG9jYWxzKSBtb2R1bGUuZXhwb3J0cyA9IGNvbnRlbnQubG9jYWxzO1xuXG5pZihtb2R1bGUuaG90KSB7XG5cdG1vZHVsZS5ob3QuYWNjZXB0KFwiISEuLi8uLi9jc3MtbG9hZGVyL2Rpc3QvY2pzLmpzIS4vcmVhY3QtZGF0ZXRpbWUuY3NzXCIsIGZ1bmN0aW9uKCkge1xuXHRcdHZhciBuZXdDb250ZW50ID0gcmVxdWlyZShcIiEhLi4vLi4vY3NzLWxvYWRlci9kaXN0L2Nqcy5qcyEuL3JlYWN0LWRhdGV0aW1lLmNzc1wiKTtcblxuXHRcdGlmKHR5cGVvZiBuZXdDb250ZW50ID09PSAnc3RyaW5nJykgbmV3Q29udGVudCA9IFtbbW9kdWxlLmlkLCBuZXdDb250ZW50LCAnJ11dO1xuXG5cdFx0dmFyIGxvY2FscyA9IChmdW5jdGlvbihhLCBiKSB7XG5cdFx0XHR2YXIga2V5LCBpZHggPSAwO1xuXG5cdFx0XHRmb3Ioa2V5IGluIGEpIHtcblx0XHRcdFx0aWYoIWIgfHwgYVtrZXldICE9PSBiW2tleV0pIHJldHVybiBmYWxzZTtcblx0XHRcdFx0aWR4Kys7XG5cdFx0XHR9XG5cblx0XHRcdGZvcihrZXkgaW4gYikgaWR4LS07XG5cblx0XHRcdHJldHVybiBpZHggPT09IDA7XG5cdFx0fShjb250ZW50LmxvY2FscywgbmV3Q29udGVudC5sb2NhbHMpKTtcblxuXHRcdGlmKCFsb2NhbHMpIHRocm93IG5ldyBFcnJvcignQWJvcnRpbmcgQ1NTIEhNUiBkdWUgdG8gY2hhbmdlZCBjc3MtbW9kdWxlcyBsb2NhbHMuJyk7XG5cblx0XHR1cGRhdGUobmV3Q29udGVudCk7XG5cdH0pO1xuXG5cdG1vZHVsZS5ob3QuZGlzcG9zZShmdW5jdGlvbigpIHsgdXBkYXRlKCk7IH0pO1xufSIsIid1c2Ugc3RyaWN0JztcbnZhciBwcm9wSXNFbnVtZXJhYmxlID0gT2JqZWN0LnByb3RvdHlwZS5wcm9wZXJ0eUlzRW51bWVyYWJsZTtcblxuZnVuY3Rpb24gVG9PYmplY3QodmFsKSB7XG5cdGlmICh2YWwgPT0gbnVsbCkge1xuXHRcdHRocm93IG5ldyBUeXBlRXJyb3IoJ09iamVjdC5hc3NpZ24gY2Fubm90IGJlIGNhbGxlZCB3aXRoIG51bGwgb3IgdW5kZWZpbmVkJyk7XG5cdH1cblxuXHRyZXR1cm4gT2JqZWN0KHZhbCk7XG59XG5cbmZ1bmN0aW9uIG93bkVudW1lcmFibGVLZXlzKG9iaikge1xuXHR2YXIga2V5cyA9IE9iamVjdC5nZXRPd25Qcm9wZXJ0eU5hbWVzKG9iaik7XG5cblx0aWYgKE9iamVjdC5nZXRPd25Qcm9wZXJ0eVN5bWJvbHMpIHtcblx0XHRrZXlzID0ga2V5cy5jb25jYXQoT2JqZWN0LmdldE93blByb3BlcnR5U3ltYm9scyhvYmopKTtcblx0fVxuXG5cdHJldHVybiBrZXlzLmZpbHRlcihmdW5jdGlvbiAoa2V5KSB7XG5cdFx0cmV0dXJuIHByb3BJc0VudW1lcmFibGUuY2FsbChvYmosIGtleSk7XG5cdH0pO1xufVxuXG5tb2R1bGUuZXhwb3J0cyA9IE9iamVjdC5hc3NpZ24gfHwgZnVuY3Rpb24gKHRhcmdldCwgc291cmNlKSB7XG5cdHZhciBmcm9tO1xuXHR2YXIga2V5cztcblx0dmFyIHRvID0gVG9PYmplY3QodGFyZ2V0KTtcblxuXHRmb3IgKHZhciBzID0gMTsgcyA8IGFyZ3VtZW50cy5sZW5ndGg7IHMrKykge1xuXHRcdGZyb20gPSBhcmd1bWVudHNbc107XG5cdFx0a2V5cyA9IG93bkVudW1lcmFibGVLZXlzKE9iamVjdChmcm9tKSk7XG5cblx0XHRmb3IgKHZhciBpID0gMDsgaSA8IGtleXMubGVuZ3RoOyBpKyspIHtcblx0XHRcdHRvW2tleXNbaV1dID0gZnJvbVtrZXlzW2ldXTtcblx0XHR9XG5cdH1cblxuXHRyZXR1cm4gdG87XG59O1xuIiwiJ3VzZSBzdHJpY3QnO1xuXG52YXIgUmVhY3QgPSByZXF1aXJlKCdyZWFjdCcpLFxuXHRjcmVhdGVDbGFzcyA9IHJlcXVpcmUoJ2NyZWF0ZS1yZWFjdC1jbGFzcycpLFxuXHREYXlzVmlldyA9IHJlcXVpcmUoJy4vRGF5c1ZpZXcnKSxcblx0TW9udGhzVmlldyA9IHJlcXVpcmUoJy4vTW9udGhzVmlldycpLFxuXHRZZWFyc1ZpZXcgPSByZXF1aXJlKCcuL1llYXJzVmlldycpLFxuXHRUaW1lVmlldyA9IHJlcXVpcmUoJy4vVGltZVZpZXcnKVxuXHQ7XG5cbnZhciBDYWxlbmRhckNvbnRhaW5lciA9IGNyZWF0ZUNsYXNzKHtcblx0dmlld0NvbXBvbmVudHM6IHtcblx0XHRkYXlzOiBEYXlzVmlldyxcblx0XHRtb250aHM6IE1vbnRoc1ZpZXcsXG5cdFx0eWVhcnM6IFllYXJzVmlldyxcblx0XHR0aW1lOiBUaW1lVmlld1xuXHR9LFxuXG5cdHJlbmRlcjogZnVuY3Rpb24oKSB7XG5cdFx0cmV0dXJuIFJlYWN0LmNyZWF0ZUVsZW1lbnQoIHRoaXMudmlld0NvbXBvbmVudHNbIHRoaXMucHJvcHMudmlldyBdLCB0aGlzLnByb3BzLnZpZXdQcm9wcyApO1xuXHR9XG59KTtcblxubW9kdWxlLmV4cG9ydHMgPSBDYWxlbmRhckNvbnRhaW5lcjtcbiIsIid1c2Ugc3RyaWN0JztcblxudmFyIFJlYWN0ID0gcmVxdWlyZSgncmVhY3QnKSxcblx0Y3JlYXRlQ2xhc3MgPSByZXF1aXJlKCdjcmVhdGUtcmVhY3QtY2xhc3MnKSxcblx0bW9tZW50ID0gcmVxdWlyZSgnbW9tZW50JyksXG5cdG9uQ2xpY2tPdXRzaWRlID0gcmVxdWlyZSgncmVhY3Qtb25jbGlja291dHNpZGUnKS5kZWZhdWx0XG5cdDtcblxudmFyIERhdGVUaW1lUGlja2VyRGF5cyA9IG9uQ2xpY2tPdXRzaWRlKCBjcmVhdGVDbGFzcyh7XG5cdHJlbmRlcjogZnVuY3Rpb24oKSB7XG5cdFx0dmFyIGZvb3RlciA9IHRoaXMucmVuZGVyRm9vdGVyKCksXG5cdFx0XHRkYXRlID0gdGhpcy5wcm9wcy52aWV3RGF0ZSxcblx0XHRcdGxvY2FsZSA9IGRhdGUubG9jYWxlRGF0YSgpLFxuXHRcdFx0dGFibGVDaGlsZHJlblxuXHRcdFx0O1xuXG5cdFx0dGFibGVDaGlsZHJlbiA9IFtcblx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoZWFkJywgeyBrZXk6ICd0aCcgfSwgW1xuXHRcdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0cicsIHsga2V5OiAnaCcgfSwgW1xuXHRcdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoJywgeyBrZXk6ICdwJywgY2xhc3NOYW1lOiAncmR0UHJldicsIG9uQ2xpY2s6IHRoaXMucHJvcHMuc3VidHJhY3RUaW1lKCAxLCAnbW9udGhzJyApfSwgUmVhY3QuY3JlYXRlRWxlbWVudCgnc3BhbicsIHt9LCAn4oC5JyApKSxcblx0XHRcdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0aCcsIHsga2V5OiAncycsIGNsYXNzTmFtZTogJ3JkdFN3aXRjaCcsIG9uQ2xpY2s6IHRoaXMucHJvcHMuc2hvd1ZpZXcoICdtb250aHMnICksIGNvbFNwYW46IDUsICdkYXRhLXZhbHVlJzogdGhpcy5wcm9wcy52aWV3RGF0ZS5tb250aCgpIH0sIGxvY2FsZS5tb250aHMoIGRhdGUgKSArICcgJyArIGRhdGUueWVhcigpICksXG5cdFx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGgnLCB7IGtleTogJ24nLCBjbGFzc05hbWU6ICdyZHROZXh0Jywgb25DbGljazogdGhpcy5wcm9wcy5hZGRUaW1lKCAxLCAnbW9udGhzJyApfSwgUmVhY3QuY3JlYXRlRWxlbWVudCgnc3BhbicsIHt9LCAn4oC6JyApKVxuXHRcdFx0XHRdKSxcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndHInLCB7IGtleTogJ2QnfSwgdGhpcy5nZXREYXlzT2ZXZWVrKCBsb2NhbGUgKS5tYXAoIGZ1bmN0aW9uKCBkYXksIGluZGV4ICkgeyByZXR1cm4gUmVhY3QuY3JlYXRlRWxlbWVudCgndGgnLCB7IGtleTogZGF5ICsgaW5kZXgsIGNsYXNzTmFtZTogJ2Rvdyd9LCBkYXkgKTsgfSkgKVxuXHRcdFx0XSksXG5cdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0Ym9keScsIHsga2V5OiAndGInIH0sIHRoaXMucmVuZGVyRGF5cygpKVxuXHRcdF07XG5cblx0XHRpZiAoIGZvb3RlciApXG5cdFx0XHR0YWJsZUNoaWxkcmVuLnB1c2goIGZvb3RlciApO1xuXG5cdFx0cmV0dXJuIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ2RpdicsIHsgY2xhc3NOYW1lOiAncmR0RGF5cycgfSxcblx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RhYmxlJywge30sIHRhYmxlQ2hpbGRyZW4gKVxuXHRcdCk7XG5cdH0sXG5cblx0LyoqXG5cdCAqIEdldCBhIGxpc3Qgb2YgdGhlIGRheXMgb2YgdGhlIHdlZWtcblx0ICogZGVwZW5kaW5nIG9uIHRoZSBjdXJyZW50IGxvY2FsZVxuXHQgKiBAcmV0dXJuIHthcnJheX0gQSBsaXN0IHdpdGggdGhlIHNob3J0bmFtZSBvZiB0aGUgZGF5c1xuXHQgKi9cblx0Z2V0RGF5c09mV2VlazogZnVuY3Rpb24oIGxvY2FsZSApIHtcblx0XHR2YXIgZGF5cyA9IGxvY2FsZS5fd2Vla2RheXNNaW4sXG5cdFx0XHRmaXJzdCA9IGxvY2FsZS5maXJzdERheU9mV2VlaygpLFxuXHRcdFx0ZG93ID0gW10sXG5cdFx0XHRpID0gMFxuXHRcdFx0O1xuXG5cdFx0ZGF5cy5mb3JFYWNoKCBmdW5jdGlvbiggZGF5ICkge1xuXHRcdFx0ZG93WyAoNyArICggaSsrICkgLSBmaXJzdCkgJSA3IF0gPSBkYXk7XG5cdFx0fSk7XG5cblx0XHRyZXR1cm4gZG93O1xuXHR9LFxuXG5cdHJlbmRlckRheXM6IGZ1bmN0aW9uKCkge1xuXHRcdHZhciBkYXRlID0gdGhpcy5wcm9wcy52aWV3RGF0ZSxcblx0XHRcdHNlbGVjdGVkID0gdGhpcy5wcm9wcy5zZWxlY3RlZERhdGUgJiYgdGhpcy5wcm9wcy5zZWxlY3RlZERhdGUuY2xvbmUoKSxcblx0XHRcdHByZXZNb250aCA9IGRhdGUuY2xvbmUoKS5zdWJ0cmFjdCggMSwgJ21vbnRocycgKSxcblx0XHRcdGN1cnJlbnRZZWFyID0gZGF0ZS55ZWFyKCksXG5cdFx0XHRjdXJyZW50TW9udGggPSBkYXRlLm1vbnRoKCksXG5cdFx0XHR3ZWVrcyA9IFtdLFxuXHRcdFx0ZGF5cyA9IFtdLFxuXHRcdFx0cmVuZGVyZXIgPSB0aGlzLnByb3BzLnJlbmRlckRheSB8fCB0aGlzLnJlbmRlckRheSxcblx0XHRcdGlzVmFsaWQgPSB0aGlzLnByb3BzLmlzVmFsaWREYXRlIHx8IHRoaXMuYWx3YXlzVmFsaWREYXRlLFxuXHRcdFx0Y2xhc3NlcywgaXNEaXNhYmxlZCwgZGF5UHJvcHMsIGN1cnJlbnREYXRlXG5cdFx0XHQ7XG5cblx0XHQvLyBHbyB0byB0aGUgbGFzdCB3ZWVrIG9mIHRoZSBwcmV2aW91cyBtb250aFxuXHRcdHByZXZNb250aC5kYXRlKCBwcmV2TW9udGguZGF5c0luTW9udGgoKSApLnN0YXJ0T2YoICd3ZWVrJyApO1xuXHRcdHZhciBsYXN0RGF5ID0gcHJldk1vbnRoLmNsb25lKCkuYWRkKCA0MiwgJ2QnICk7XG5cblx0XHR3aGlsZSAoIHByZXZNb250aC5pc0JlZm9yZSggbGFzdERheSApICkge1xuXHRcdFx0Y2xhc3NlcyA9ICdyZHREYXknO1xuXHRcdFx0Y3VycmVudERhdGUgPSBwcmV2TW9udGguY2xvbmUoKTtcblxuXHRcdFx0aWYgKCAoIHByZXZNb250aC55ZWFyKCkgPT09IGN1cnJlbnRZZWFyICYmIHByZXZNb250aC5tb250aCgpIDwgY3VycmVudE1vbnRoICkgfHwgKCBwcmV2TW9udGgueWVhcigpIDwgY3VycmVudFllYXIgKSApXG5cdFx0XHRcdGNsYXNzZXMgKz0gJyByZHRPbGQnO1xuXHRcdFx0ZWxzZSBpZiAoICggcHJldk1vbnRoLnllYXIoKSA9PT0gY3VycmVudFllYXIgJiYgcHJldk1vbnRoLm1vbnRoKCkgPiBjdXJyZW50TW9udGggKSB8fCAoIHByZXZNb250aC55ZWFyKCkgPiBjdXJyZW50WWVhciApIClcblx0XHRcdFx0Y2xhc3NlcyArPSAnIHJkdE5ldyc7XG5cblx0XHRcdGlmICggc2VsZWN0ZWQgJiYgcHJldk1vbnRoLmlzU2FtZSggc2VsZWN0ZWQsICdkYXknICkgKVxuXHRcdFx0XHRjbGFzc2VzICs9ICcgcmR0QWN0aXZlJztcblxuXHRcdFx0aWYgKCBwcmV2TW9udGguaXNTYW1lKCBtb21lbnQoKSwgJ2RheScgKSApXG5cdFx0XHRcdGNsYXNzZXMgKz0gJyByZHRUb2RheSc7XG5cblx0XHRcdGlzRGlzYWJsZWQgPSAhaXNWYWxpZCggY3VycmVudERhdGUsIHNlbGVjdGVkICk7XG5cdFx0XHRpZiAoIGlzRGlzYWJsZWQgKVxuXHRcdFx0XHRjbGFzc2VzICs9ICcgcmR0RGlzYWJsZWQnO1xuXG5cdFx0XHRkYXlQcm9wcyA9IHtcblx0XHRcdFx0a2V5OiBwcmV2TW9udGguZm9ybWF0KCAnTV9EJyApLFxuXHRcdFx0XHQnZGF0YS12YWx1ZSc6IHByZXZNb250aC5kYXRlKCksXG5cdFx0XHRcdGNsYXNzTmFtZTogY2xhc3Nlc1xuXHRcdFx0fTtcblxuXHRcdFx0aWYgKCAhaXNEaXNhYmxlZCApXG5cdFx0XHRcdGRheVByb3BzLm9uQ2xpY2sgPSB0aGlzLnVwZGF0ZVNlbGVjdGVkRGF0ZTtcblxuXHRcdFx0ZGF5cy5wdXNoKCByZW5kZXJlciggZGF5UHJvcHMsIGN1cnJlbnREYXRlLCBzZWxlY3RlZCApICk7XG5cblx0XHRcdGlmICggZGF5cy5sZW5ndGggPT09IDcgKSB7XG5cdFx0XHRcdHdlZWtzLnB1c2goIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RyJywgeyBrZXk6IHByZXZNb250aC5mb3JtYXQoICdNX0QnICl9LCBkYXlzICkgKTtcblx0XHRcdFx0ZGF5cyA9IFtdO1xuXHRcdFx0fVxuXG5cdFx0XHRwcmV2TW9udGguYWRkKCAxLCAnZCcgKTtcblx0XHR9XG5cblx0XHRyZXR1cm4gd2Vla3M7XG5cdH0sXG5cblx0dXBkYXRlU2VsZWN0ZWREYXRlOiBmdW5jdGlvbiggZXZlbnQgKSB7XG5cdFx0dGhpcy5wcm9wcy51cGRhdGVTZWxlY3RlZERhdGUoIGV2ZW50LCB0cnVlICk7XG5cdH0sXG5cblx0cmVuZGVyRGF5OiBmdW5jdGlvbiggcHJvcHMsIGN1cnJlbnREYXRlICkge1xuXHRcdHJldHVybiBSZWFjdC5jcmVhdGVFbGVtZW50KCd0ZCcsICBwcm9wcywgY3VycmVudERhdGUuZGF0ZSgpICk7XG5cdH0sXG5cblx0cmVuZGVyRm9vdGVyOiBmdW5jdGlvbigpIHtcblx0XHRpZiAoICF0aGlzLnByb3BzLnRpbWVGb3JtYXQgKVxuXHRcdFx0cmV0dXJuICcnO1xuXG5cdFx0dmFyIGRhdGUgPSB0aGlzLnByb3BzLnNlbGVjdGVkRGF0ZSB8fCB0aGlzLnByb3BzLnZpZXdEYXRlO1xuXG5cdFx0cmV0dXJuIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3Rmb290JywgeyBrZXk6ICd0Zid9LFxuXHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndHInLCB7fSxcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGQnLCB7IG9uQ2xpY2s6IHRoaXMucHJvcHMuc2hvd1ZpZXcoICd0aW1lJyApLCBjb2xTcGFuOiA3LCBjbGFzc05hbWU6ICdyZHRUaW1lVG9nZ2xlJyB9LCBkYXRlLmZvcm1hdCggdGhpcy5wcm9wcy50aW1lRm9ybWF0ICkpXG5cdFx0XHQpXG5cdFx0KTtcblx0fSxcblxuXHRhbHdheXNWYWxpZERhdGU6IGZ1bmN0aW9uKCkge1xuXHRcdHJldHVybiAxO1xuXHR9LFxuXG5cdGhhbmRsZUNsaWNrT3V0c2lkZTogZnVuY3Rpb24oKSB7XG5cdFx0dGhpcy5wcm9wcy5oYW5kbGVDbGlja091dHNpZGUoKTtcblx0fVxufSkpO1xuXG5tb2R1bGUuZXhwb3J0cyA9IERhdGVUaW1lUGlja2VyRGF5cztcbiIsIid1c2Ugc3RyaWN0JztcblxudmFyIFJlYWN0ID0gcmVxdWlyZSgncmVhY3QnKSxcblx0Y3JlYXRlQ2xhc3MgPSByZXF1aXJlKCdjcmVhdGUtcmVhY3QtY2xhc3MnKSxcblx0b25DbGlja091dHNpZGUgPSByZXF1aXJlKCdyZWFjdC1vbmNsaWNrb3V0c2lkZScpLmRlZmF1bHRcblx0O1xuXG52YXIgRGF0ZVRpbWVQaWNrZXJNb250aHMgPSBvbkNsaWNrT3V0c2lkZSggY3JlYXRlQ2xhc3Moe1xuXHRyZW5kZXI6IGZ1bmN0aW9uKCkge1xuXHRcdHJldHVybiBSZWFjdC5jcmVhdGVFbGVtZW50KCdkaXYnLCB7IGNsYXNzTmFtZTogJ3JkdE1vbnRocycgfSwgW1xuXHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGFibGUnLCB7IGtleTogJ2EnIH0sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoZWFkJywge30sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RyJywge30sIFtcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGgnLCB7IGtleTogJ3ByZXYnLCBjbGFzc05hbWU6ICdyZHRQcmV2Jywgb25DbGljazogdGhpcy5wcm9wcy5zdWJ0cmFjdFRpbWUoIDEsICd5ZWFycycgKX0sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3NwYW4nLCB7fSwgJ+KAuScgKSksXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoJywgeyBrZXk6ICd5ZWFyJywgY2xhc3NOYW1lOiAncmR0U3dpdGNoJywgb25DbGljazogdGhpcy5wcm9wcy5zaG93VmlldyggJ3llYXJzJyApLCBjb2xTcGFuOiAyLCAnZGF0YS12YWx1ZSc6IHRoaXMucHJvcHMudmlld0RhdGUueWVhcigpIH0sIHRoaXMucHJvcHMudmlld0RhdGUueWVhcigpICksXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoJywgeyBrZXk6ICduZXh0JywgY2xhc3NOYW1lOiAncmR0TmV4dCcsIG9uQ2xpY2s6IHRoaXMucHJvcHMuYWRkVGltZSggMSwgJ3llYXJzJyApfSwgUmVhY3QuY3JlYXRlRWxlbWVudCgnc3BhbicsIHt9LCAn4oC6JyApKVxuXHRcdFx0XSkpKSxcblx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RhYmxlJywgeyBrZXk6ICdtb250aHMnIH0sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3Rib2R5JywgeyBrZXk6ICdiJyB9LCB0aGlzLnJlbmRlck1vbnRocygpKSlcblx0XHRdKTtcblx0fSxcblxuXHRyZW5kZXJNb250aHM6IGZ1bmN0aW9uKCkge1xuXHRcdHZhciBkYXRlID0gdGhpcy5wcm9wcy5zZWxlY3RlZERhdGUsXG5cdFx0XHRtb250aCA9IHRoaXMucHJvcHMudmlld0RhdGUubW9udGgoKSxcblx0XHRcdHllYXIgPSB0aGlzLnByb3BzLnZpZXdEYXRlLnllYXIoKSxcblx0XHRcdHJvd3MgPSBbXSxcblx0XHRcdGkgPSAwLFxuXHRcdFx0bW9udGhzID0gW10sXG5cdFx0XHRyZW5kZXJlciA9IHRoaXMucHJvcHMucmVuZGVyTW9udGggfHwgdGhpcy5yZW5kZXJNb250aCxcblx0XHRcdGlzVmFsaWQgPSB0aGlzLnByb3BzLmlzVmFsaWREYXRlIHx8IHRoaXMuYWx3YXlzVmFsaWREYXRlLFxuXHRcdFx0Y2xhc3NlcywgcHJvcHMsIGN1cnJlbnRNb250aCwgaXNEaXNhYmxlZCwgbm9PZkRheXNJbk1vbnRoLCBkYXlzSW5Nb250aCwgdmFsaWREYXksXG5cdFx0XHQvLyBEYXRlIGlzIGlycmVsZXZhbnQgYmVjYXVzZSB3ZSdyZSBvbmx5IGludGVyZXN0ZWQgaW4gbW9udGhcblx0XHRcdGlycmVsZXZhbnREYXRlID0gMVxuXHRcdFx0O1xuXG5cdFx0d2hpbGUgKGkgPCAxMikge1xuXHRcdFx0Y2xhc3NlcyA9ICdyZHRNb250aCc7XG5cdFx0XHRjdXJyZW50TW9udGggPVxuXHRcdFx0XHR0aGlzLnByb3BzLnZpZXdEYXRlLmNsb25lKCkuc2V0KHsgeWVhcjogeWVhciwgbW9udGg6IGksIGRhdGU6IGlycmVsZXZhbnREYXRlIH0pO1xuXG5cdFx0XHRub09mRGF5c0luTW9udGggPSBjdXJyZW50TW9udGguZW5kT2YoICdtb250aCcgKS5mb3JtYXQoICdEJyApO1xuXHRcdFx0ZGF5c0luTW9udGggPSBBcnJheS5mcm9tKHsgbGVuZ3RoOiBub09mRGF5c0luTW9udGggfSwgZnVuY3Rpb24oIGUsIGkgKSB7XG5cdFx0XHRcdHJldHVybiBpICsgMTtcblx0XHRcdH0pO1xuXG5cdFx0XHR2YWxpZERheSA9IGRheXNJbk1vbnRoLmZpbmQoZnVuY3Rpb24oIGQgKSB7XG5cdFx0XHRcdHZhciBkYXkgPSBjdXJyZW50TW9udGguY2xvbmUoKS5zZXQoICdkYXRlJywgZCApO1xuXHRcdFx0XHRyZXR1cm4gaXNWYWxpZCggZGF5ICk7XG5cdFx0XHR9KTtcblxuXHRcdFx0aXNEaXNhYmxlZCA9ICggdmFsaWREYXkgPT09IHVuZGVmaW5lZCApO1xuXG5cdFx0XHRpZiAoIGlzRGlzYWJsZWQgKVxuXHRcdFx0XHRjbGFzc2VzICs9ICcgcmR0RGlzYWJsZWQnO1xuXG5cdFx0XHRpZiAoIGRhdGUgJiYgaSA9PT0gZGF0ZS5tb250aCgpICYmIHllYXIgPT09IGRhdGUueWVhcigpIClcblx0XHRcdFx0Y2xhc3NlcyArPSAnIHJkdEFjdGl2ZSc7XG5cblx0XHRcdHByb3BzID0ge1xuXHRcdFx0XHRrZXk6IGksXG5cdFx0XHRcdCdkYXRhLXZhbHVlJzogaSxcblx0XHRcdFx0Y2xhc3NOYW1lOiBjbGFzc2VzXG5cdFx0XHR9O1xuXG5cdFx0XHRpZiAoICFpc0Rpc2FibGVkIClcblx0XHRcdFx0cHJvcHMub25DbGljayA9ICggdGhpcy5wcm9wcy51cGRhdGVPbiA9PT0gJ21vbnRocycgP1xuXHRcdFx0XHRcdHRoaXMudXBkYXRlU2VsZWN0ZWRNb250aCA6IHRoaXMucHJvcHMuc2V0RGF0ZSggJ21vbnRoJyApICk7XG5cblx0XHRcdG1vbnRocy5wdXNoKCByZW5kZXJlciggcHJvcHMsIGksIHllYXIsIGRhdGUgJiYgZGF0ZS5jbG9uZSgpICkgKTtcblxuXHRcdFx0aWYgKCBtb250aHMubGVuZ3RoID09PSA0ICkge1xuXHRcdFx0XHRyb3dzLnB1c2goIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RyJywgeyBrZXk6IG1vbnRoICsgJ18nICsgcm93cy5sZW5ndGggfSwgbW9udGhzICkgKTtcblx0XHRcdFx0bW9udGhzID0gW107XG5cdFx0XHR9XG5cblx0XHRcdGkrKztcblx0XHR9XG5cblx0XHRyZXR1cm4gcm93cztcblx0fSxcblxuXHR1cGRhdGVTZWxlY3RlZE1vbnRoOiBmdW5jdGlvbiggZXZlbnQgKSB7XG5cdFx0dGhpcy5wcm9wcy51cGRhdGVTZWxlY3RlZERhdGUoIGV2ZW50ICk7XG5cdH0sXG5cblx0cmVuZGVyTW9udGg6IGZ1bmN0aW9uKCBwcm9wcywgbW9udGggKSB7XG5cdFx0dmFyIGxvY2FsTW9tZW50ID0gdGhpcy5wcm9wcy52aWV3RGF0ZTtcblx0XHR2YXIgbW9udGhTdHIgPSBsb2NhbE1vbWVudC5sb2NhbGVEYXRhKCkubW9udGhzU2hvcnQoIGxvY2FsTW9tZW50Lm1vbnRoKCBtb250aCApICk7XG5cdFx0dmFyIHN0ckxlbmd0aCA9IDM7XG5cdFx0Ly8gQmVjYXVzZSBzb21lIG1vbnRocyBhcmUgdXAgdG8gNSBjaGFyYWN0ZXJzIGxvbmcsIHdlIHdhbnQgdG9cblx0XHQvLyB1c2UgYSBmaXhlZCBzdHJpbmcgbGVuZ3RoIGZvciBjb25zaXN0ZW5jeVxuXHRcdHZhciBtb250aFN0ckZpeGVkTGVuZ3RoID0gbW9udGhTdHIuc3Vic3RyaW5nKCAwLCBzdHJMZW5ndGggKTtcblx0XHRyZXR1cm4gUmVhY3QuY3JlYXRlRWxlbWVudCgndGQnLCBwcm9wcywgY2FwaXRhbGl6ZSggbW9udGhTdHJGaXhlZExlbmd0aCApICk7XG5cdH0sXG5cblx0YWx3YXlzVmFsaWREYXRlOiBmdW5jdGlvbigpIHtcblx0XHRyZXR1cm4gMTtcblx0fSxcblxuXHRoYW5kbGVDbGlja091dHNpZGU6IGZ1bmN0aW9uKCkge1xuXHRcdHRoaXMucHJvcHMuaGFuZGxlQ2xpY2tPdXRzaWRlKCk7XG5cdH1cbn0pKTtcblxuZnVuY3Rpb24gY2FwaXRhbGl6ZSggc3RyICkge1xuXHRyZXR1cm4gc3RyLmNoYXJBdCggMCApLnRvVXBwZXJDYXNlKCkgKyBzdHIuc2xpY2UoIDEgKTtcbn1cblxubW9kdWxlLmV4cG9ydHMgPSBEYXRlVGltZVBpY2tlck1vbnRocztcbiIsIid1c2Ugc3RyaWN0JztcblxudmFyIFJlYWN0ID0gcmVxdWlyZSgncmVhY3QnKSxcblx0Y3JlYXRlQ2xhc3MgPSByZXF1aXJlKCdjcmVhdGUtcmVhY3QtY2xhc3MnKSxcblx0YXNzaWduID0gcmVxdWlyZSgnb2JqZWN0LWFzc2lnbicpLFxuXHRvbkNsaWNrT3V0c2lkZSA9IHJlcXVpcmUoJ3JlYWN0LW9uY2xpY2tvdXRzaWRlJykuZGVmYXVsdFxuXHQ7XG5cbnZhciBEYXRlVGltZVBpY2tlclRpbWUgPSBvbkNsaWNrT3V0c2lkZSggY3JlYXRlQ2xhc3Moe1xuXHRnZXRJbml0aWFsU3RhdGU6IGZ1bmN0aW9uKCkge1xuXHRcdHJldHVybiB0aGlzLmNhbGN1bGF0ZVN0YXRlKCB0aGlzLnByb3BzICk7XG5cdH0sXG5cblx0Y2FsY3VsYXRlU3RhdGU6IGZ1bmN0aW9uKCBwcm9wcyApIHtcblx0XHR2YXIgZGF0ZSA9IHByb3BzLnNlbGVjdGVkRGF0ZSB8fCBwcm9wcy52aWV3RGF0ZSxcblx0XHRcdGZvcm1hdCA9IHByb3BzLnRpbWVGb3JtYXQsXG5cdFx0XHRjb3VudGVycyA9IFtdXG5cdFx0XHQ7XG5cblx0XHRpZiAoIGZvcm1hdC50b0xvd2VyQ2FzZSgpLmluZGV4T2YoJ2gnKSAhPT0gLTEgKSB7XG5cdFx0XHRjb3VudGVycy5wdXNoKCdob3VycycpO1xuXHRcdFx0aWYgKCBmb3JtYXQuaW5kZXhPZignbScpICE9PSAtMSApIHtcblx0XHRcdFx0Y291bnRlcnMucHVzaCgnbWludXRlcycpO1xuXHRcdFx0XHRpZiAoIGZvcm1hdC5pbmRleE9mKCdzJykgIT09IC0xICkge1xuXHRcdFx0XHRcdGNvdW50ZXJzLnB1c2goJ3NlY29uZHMnKTtcblx0XHRcdFx0fVxuXHRcdFx0fVxuXHRcdH1cblxuXHRcdHZhciBob3VycyA9IGRhdGUuZm9ybWF0KCAnSCcgKTtcblx0XHRcblx0XHR2YXIgZGF5cGFydCA9IGZhbHNlO1xuXHRcdGlmICggdGhpcy5zdGF0ZSAhPT0gbnVsbCAmJiB0aGlzLnByb3BzLnRpbWVGb3JtYXQudG9Mb3dlckNhc2UoKS5pbmRleE9mKCAnIGEnICkgIT09IC0xICkge1xuXHRcdFx0aWYgKCB0aGlzLnByb3BzLnRpbWVGb3JtYXQuaW5kZXhPZiggJyBBJyApICE9PSAtMSApIHtcblx0XHRcdFx0ZGF5cGFydCA9ICggaG91cnMgPj0gMTIgKSA/ICdQTScgOiAnQU0nO1xuXHRcdFx0fSBlbHNlIHtcblx0XHRcdFx0ZGF5cGFydCA9ICggaG91cnMgPj0gMTIgKSA/ICdwbScgOiAnYW0nO1xuXHRcdFx0fVxuXHRcdH1cblxuXHRcdHJldHVybiB7XG5cdFx0XHRob3VyczogaG91cnMsXG5cdFx0XHRtaW51dGVzOiBkYXRlLmZvcm1hdCggJ21tJyApLFxuXHRcdFx0c2Vjb25kczogZGF0ZS5mb3JtYXQoICdzcycgKSxcblx0XHRcdG1pbGxpc2Vjb25kczogZGF0ZS5mb3JtYXQoICdTU1MnICksXG5cdFx0XHRkYXlwYXJ0OiBkYXlwYXJ0LFxuXHRcdFx0Y291bnRlcnM6IGNvdW50ZXJzXG5cdFx0fTtcblx0fSxcblxuXHRyZW5kZXJDb3VudGVyOiBmdW5jdGlvbiggdHlwZSApIHtcblx0XHRpZiAoIHR5cGUgIT09ICdkYXlwYXJ0JyApIHtcblx0XHRcdHZhciB2YWx1ZSA9IHRoaXMuc3RhdGVbIHR5cGUgXTtcblx0XHRcdGlmICggdHlwZSA9PT0gJ2hvdXJzJyAmJiB0aGlzLnByb3BzLnRpbWVGb3JtYXQudG9Mb3dlckNhc2UoKS5pbmRleE9mKCAnIGEnICkgIT09IC0xICkge1xuXHRcdFx0XHR2YWx1ZSA9ICggdmFsdWUgLSAxICkgJSAxMiArIDE7XG5cblx0XHRcdFx0aWYgKCB2YWx1ZSA9PT0gMCApIHtcblx0XHRcdFx0XHR2YWx1ZSA9IDEyO1xuXHRcdFx0XHR9XG5cdFx0XHR9XG5cdFx0XHRyZXR1cm4gUmVhY3QuY3JlYXRlRWxlbWVudCgnZGl2JywgeyBrZXk6IHR5cGUsIGNsYXNzTmFtZTogJ3JkdENvdW50ZXInIH0sIFtcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgnc3BhbicsIHsga2V5OiAndXAnLCBjbGFzc05hbWU6ICdyZHRCdG4nLCBvblRvdWNoU3RhcnQ6IHRoaXMub25TdGFydENsaWNraW5nKCdpbmNyZWFzZScsIHR5cGUpLCBvbk1vdXNlRG93bjogdGhpcy5vblN0YXJ0Q2xpY2tpbmcoICdpbmNyZWFzZScsIHR5cGUgKSwgb25Db250ZXh0TWVudTogdGhpcy5kaXNhYmxlQ29udGV4dE1lbnUgfSwgJ+KWsicgKSxcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgnZGl2JywgeyBrZXk6ICdjJywgY2xhc3NOYW1lOiAncmR0Q291bnQnIH0sIHZhbHVlICksXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3NwYW4nLCB7IGtleTogJ2RvJywgY2xhc3NOYW1lOiAncmR0QnRuJywgb25Ub3VjaFN0YXJ0OiB0aGlzLm9uU3RhcnRDbGlja2luZygnZGVjcmVhc2UnLCB0eXBlKSwgb25Nb3VzZURvd246IHRoaXMub25TdGFydENsaWNraW5nKCAnZGVjcmVhc2UnLCB0eXBlICksIG9uQ29udGV4dE1lbnU6IHRoaXMuZGlzYWJsZUNvbnRleHRNZW51IH0sICfilrwnIClcblx0XHRcdF0pO1xuXHRcdH1cblx0XHRyZXR1cm4gJyc7XG5cdH0sXG5cblx0cmVuZGVyRGF5UGFydDogZnVuY3Rpb24oKSB7XG5cdFx0cmV0dXJuIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ2RpdicsIHsga2V5OiAnZGF5UGFydCcsIGNsYXNzTmFtZTogJ3JkdENvdW50ZXInIH0sIFtcblx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3NwYW4nLCB7IGtleTogJ3VwJywgY2xhc3NOYW1lOiAncmR0QnRuJywgb25Ub3VjaFN0YXJ0OiB0aGlzLm9uU3RhcnRDbGlja2luZygndG9nZ2xlRGF5UGFydCcsICdob3VycycpLCBvbk1vdXNlRG93bjogdGhpcy5vblN0YXJ0Q2xpY2tpbmcoICd0b2dnbGVEYXlQYXJ0JywgJ2hvdXJzJyksIG9uQ29udGV4dE1lbnU6IHRoaXMuZGlzYWJsZUNvbnRleHRNZW51IH0sICfilrInICksXG5cdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCdkaXYnLCB7IGtleTogdGhpcy5zdGF0ZS5kYXlwYXJ0LCBjbGFzc05hbWU6ICdyZHRDb3VudCcgfSwgdGhpcy5zdGF0ZS5kYXlwYXJ0ICksXG5cdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCdzcGFuJywgeyBrZXk6ICdkbycsIGNsYXNzTmFtZTogJ3JkdEJ0bicsIG9uVG91Y2hTdGFydDogdGhpcy5vblN0YXJ0Q2xpY2tpbmcoJ3RvZ2dsZURheVBhcnQnLCAnaG91cnMnKSwgb25Nb3VzZURvd246IHRoaXMub25TdGFydENsaWNraW5nKCAndG9nZ2xlRGF5UGFydCcsICdob3VycycpLCBvbkNvbnRleHRNZW51OiB0aGlzLmRpc2FibGVDb250ZXh0TWVudSB9LCAn4pa8JyApXG5cdFx0XSk7XG5cdH0sXG5cblx0cmVuZGVyOiBmdW5jdGlvbigpIHtcblx0XHR2YXIgbWUgPSB0aGlzLFxuXHRcdFx0Y291bnRlcnMgPSBbXVxuXHRcdDtcblxuXHRcdHRoaXMuc3RhdGUuY291bnRlcnMuZm9yRWFjaCggZnVuY3Rpb24oIGMgKSB7XG5cdFx0XHRpZiAoIGNvdW50ZXJzLmxlbmd0aCApXG5cdFx0XHRcdGNvdW50ZXJzLnB1c2goIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ2RpdicsIHsga2V5OiAnc2VwJyArIGNvdW50ZXJzLmxlbmd0aCwgY2xhc3NOYW1lOiAncmR0Q291bnRlclNlcGFyYXRvcicgfSwgJzonICkgKTtcblx0XHRcdGNvdW50ZXJzLnB1c2goIG1lLnJlbmRlckNvdW50ZXIoIGMgKSApO1xuXHRcdH0pO1xuXG5cdFx0aWYgKCB0aGlzLnN0YXRlLmRheXBhcnQgIT09IGZhbHNlICkge1xuXHRcdFx0Y291bnRlcnMucHVzaCggbWUucmVuZGVyRGF5UGFydCgpICk7XG5cdFx0fVxuXG5cdFx0aWYgKCB0aGlzLnN0YXRlLmNvdW50ZXJzLmxlbmd0aCA9PT0gMyAmJiB0aGlzLnByb3BzLnRpbWVGb3JtYXQuaW5kZXhPZiggJ1MnICkgIT09IC0xICkge1xuXHRcdFx0Y291bnRlcnMucHVzaCggUmVhY3QuY3JlYXRlRWxlbWVudCgnZGl2JywgeyBjbGFzc05hbWU6ICdyZHRDb3VudGVyU2VwYXJhdG9yJywga2V5OiAnc2VwNScgfSwgJzonICkgKTtcblx0XHRcdGNvdW50ZXJzLnB1c2goXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ2RpdicsIHsgY2xhc3NOYW1lOiAncmR0Q291bnRlciByZHRNaWxsaScsIGtleTogJ20nIH0sXG5cdFx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgnaW5wdXQnLCB7IHZhbHVlOiB0aGlzLnN0YXRlLm1pbGxpc2Vjb25kcywgdHlwZTogJ3RleHQnLCBvbkNoYW5nZTogdGhpcy51cGRhdGVNaWxsaSB9IClcblx0XHRcdFx0XHQpXG5cdFx0XHRcdCk7XG5cdFx0fVxuXG5cdFx0cmV0dXJuIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ2RpdicsIHsgY2xhc3NOYW1lOiAncmR0VGltZScgfSxcblx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RhYmxlJywge30sIFtcblx0XHRcdFx0dGhpcy5yZW5kZXJIZWFkZXIoKSxcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGJvZHknLCB7IGtleTogJ2InfSwgUmVhY3QuY3JlYXRlRWxlbWVudCgndHInLCB7fSwgUmVhY3QuY3JlYXRlRWxlbWVudCgndGQnLCB7fSxcblx0XHRcdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCdkaXYnLCB7IGNsYXNzTmFtZTogJ3JkdENvdW50ZXJzJyB9LCBjb3VudGVycyApXG5cdFx0XHRcdCkpKVxuXHRcdFx0XSlcblx0XHQpO1xuXHR9LFxuXG5cdGNvbXBvbmVudFdpbGxNb3VudDogZnVuY3Rpb24oKSB7XG5cdFx0dmFyIG1lID0gdGhpcztcblx0XHRtZS50aW1lQ29uc3RyYWludHMgPSB7XG5cdFx0XHRob3Vyczoge1xuXHRcdFx0XHRtaW46IDAsXG5cdFx0XHRcdG1heDogMjMsXG5cdFx0XHRcdHN0ZXA6IDFcblx0XHRcdH0sXG5cdFx0XHRtaW51dGVzOiB7XG5cdFx0XHRcdG1pbjogMCxcblx0XHRcdFx0bWF4OiA1OSxcblx0XHRcdFx0c3RlcDogMVxuXHRcdFx0fSxcblx0XHRcdHNlY29uZHM6IHtcblx0XHRcdFx0bWluOiAwLFxuXHRcdFx0XHRtYXg6IDU5LFxuXHRcdFx0XHRzdGVwOiAxXG5cdFx0XHR9LFxuXHRcdFx0bWlsbGlzZWNvbmRzOiB7XG5cdFx0XHRcdG1pbjogMCxcblx0XHRcdFx0bWF4OiA5OTksXG5cdFx0XHRcdHN0ZXA6IDFcblx0XHRcdH1cblx0XHR9O1xuXHRcdFsnaG91cnMnLCAnbWludXRlcycsICdzZWNvbmRzJywgJ21pbGxpc2Vjb25kcyddLmZvckVhY2goIGZ1bmN0aW9uKCB0eXBlICkge1xuXHRcdFx0YXNzaWduKG1lLnRpbWVDb25zdHJhaW50c1sgdHlwZSBdLCBtZS5wcm9wcy50aW1lQ29uc3RyYWludHNbIHR5cGUgXSk7XG5cdFx0fSk7XG5cdFx0dGhpcy5zZXRTdGF0ZSggdGhpcy5jYWxjdWxhdGVTdGF0ZSggdGhpcy5wcm9wcyApICk7XG5cdH0sXG5cblx0Y29tcG9uZW50V2lsbFJlY2VpdmVQcm9wczogZnVuY3Rpb24oIG5leHRQcm9wcyApIHtcblx0XHR0aGlzLnNldFN0YXRlKCB0aGlzLmNhbGN1bGF0ZVN0YXRlKCBuZXh0UHJvcHMgKSApO1xuXHR9LFxuXG5cdHVwZGF0ZU1pbGxpOiBmdW5jdGlvbiggZSApIHtcblx0XHR2YXIgbWlsbGkgPSBwYXJzZUludCggZS50YXJnZXQudmFsdWUsIDEwICk7XG5cdFx0aWYgKCBtaWxsaSA9PT0gZS50YXJnZXQudmFsdWUgJiYgbWlsbGkgPj0gMCAmJiBtaWxsaSA8IDEwMDAgKSB7XG5cdFx0XHR0aGlzLnByb3BzLnNldFRpbWUoICdtaWxsaXNlY29uZHMnLCBtaWxsaSApO1xuXHRcdFx0dGhpcy5zZXRTdGF0ZSggeyBtaWxsaXNlY29uZHM6IG1pbGxpIH0gKTtcblx0XHR9XG5cdH0sXG5cblx0cmVuZGVySGVhZGVyOiBmdW5jdGlvbigpIHtcblx0XHRpZiAoICF0aGlzLnByb3BzLmRhdGVGb3JtYXQgKVxuXHRcdFx0cmV0dXJuIG51bGw7XG5cblx0XHR2YXIgZGF0ZSA9IHRoaXMucHJvcHMuc2VsZWN0ZWREYXRlIHx8IHRoaXMucHJvcHMudmlld0RhdGU7XG5cdFx0cmV0dXJuIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoZWFkJywgeyBrZXk6ICdoJyB9LCBSZWFjdC5jcmVhdGVFbGVtZW50KCd0cicsIHt9LFxuXHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGgnLCB7IGNsYXNzTmFtZTogJ3JkdFN3aXRjaCcsIGNvbFNwYW46IDQsIG9uQ2xpY2s6IHRoaXMucHJvcHMuc2hvd1ZpZXcoICdkYXlzJyApIH0sIGRhdGUuZm9ybWF0KCB0aGlzLnByb3BzLmRhdGVGb3JtYXQgKSApXG5cdFx0KSk7XG5cdH0sXG5cblx0b25TdGFydENsaWNraW5nOiBmdW5jdGlvbiggYWN0aW9uLCB0eXBlICkge1xuXHRcdHZhciBtZSA9IHRoaXM7XG5cblx0XHRyZXR1cm4gZnVuY3Rpb24oKSB7XG5cdFx0XHR2YXIgdXBkYXRlID0ge307XG5cdFx0XHR1cGRhdGVbIHR5cGUgXSA9IG1lWyBhY3Rpb24gXSggdHlwZSApO1xuXHRcdFx0bWUuc2V0U3RhdGUoIHVwZGF0ZSApO1xuXG5cdFx0XHRtZS50aW1lciA9IHNldFRpbWVvdXQoIGZ1bmN0aW9uKCkge1xuXHRcdFx0XHRtZS5pbmNyZWFzZVRpbWVyID0gc2V0SW50ZXJ2YWwoIGZ1bmN0aW9uKCkge1xuXHRcdFx0XHRcdHVwZGF0ZVsgdHlwZSBdID0gbWVbIGFjdGlvbiBdKCB0eXBlICk7XG5cdFx0XHRcdFx0bWUuc2V0U3RhdGUoIHVwZGF0ZSApO1xuXHRcdFx0XHR9LCA3MCk7XG5cdFx0XHR9LCA1MDApO1xuXG5cdFx0XHRtZS5tb3VzZVVwTGlzdGVuZXIgPSBmdW5jdGlvbigpIHtcblx0XHRcdFx0Y2xlYXJUaW1lb3V0KCBtZS50aW1lciApO1xuXHRcdFx0XHRjbGVhckludGVydmFsKCBtZS5pbmNyZWFzZVRpbWVyICk7XG5cdFx0XHRcdG1lLnByb3BzLnNldFRpbWUoIHR5cGUsIG1lLnN0YXRlWyB0eXBlIF0gKTtcblx0XHRcdFx0ZG9jdW1lbnQuYm9keS5yZW1vdmVFdmVudExpc3RlbmVyKCAnbW91c2V1cCcsIG1lLm1vdXNlVXBMaXN0ZW5lciApO1xuXHRcdFx0XHRkb2N1bWVudC5ib2R5LnJlbW92ZUV2ZW50TGlzdGVuZXIoICd0b3VjaGVuZCcsIG1lLm1vdXNlVXBMaXN0ZW5lciApO1xuXHRcdFx0fTtcblxuXHRcdFx0ZG9jdW1lbnQuYm9keS5hZGRFdmVudExpc3RlbmVyKCAnbW91c2V1cCcsIG1lLm1vdXNlVXBMaXN0ZW5lciApO1xuXHRcdFx0ZG9jdW1lbnQuYm9keS5hZGRFdmVudExpc3RlbmVyKCAndG91Y2hlbmQnLCBtZS5tb3VzZVVwTGlzdGVuZXIgKTtcblx0XHR9O1xuXHR9LFxuXG5cdGRpc2FibGVDb250ZXh0TWVudTogZnVuY3Rpb24oIGV2ZW50ICkge1xuXHRcdGV2ZW50LnByZXZlbnREZWZhdWx0KCk7XG5cdFx0cmV0dXJuIGZhbHNlO1xuXHR9LFxuXG5cdHBhZFZhbHVlczoge1xuXHRcdGhvdXJzOiAxLFxuXHRcdG1pbnV0ZXM6IDIsXG5cdFx0c2Vjb25kczogMixcblx0XHRtaWxsaXNlY29uZHM6IDNcblx0fSxcblxuXHR0b2dnbGVEYXlQYXJ0OiBmdW5jdGlvbiggdHlwZSApIHsgLy8gdHlwZSBpcyBhbHdheXMgJ2hvdXJzJ1xuXHRcdHZhciB2YWx1ZSA9IHBhcnNlSW50KCB0aGlzLnN0YXRlWyB0eXBlIF0sIDEwKSArIDEyO1xuXHRcdGlmICggdmFsdWUgPiB0aGlzLnRpbWVDb25zdHJhaW50c1sgdHlwZSBdLm1heCApXG5cdFx0XHR2YWx1ZSA9IHRoaXMudGltZUNvbnN0cmFpbnRzWyB0eXBlIF0ubWluICsgKCB2YWx1ZSAtICggdGhpcy50aW1lQ29uc3RyYWludHNbIHR5cGUgXS5tYXggKyAxICkgKTtcblx0XHRyZXR1cm4gdGhpcy5wYWQoIHR5cGUsIHZhbHVlICk7XG5cdH0sXG5cblx0aW5jcmVhc2U6IGZ1bmN0aW9uKCB0eXBlICkge1xuXHRcdHZhciB2YWx1ZSA9IHBhcnNlSW50KCB0aGlzLnN0YXRlWyB0eXBlIF0sIDEwKSArIHRoaXMudGltZUNvbnN0cmFpbnRzWyB0eXBlIF0uc3RlcDtcblx0XHRpZiAoIHZhbHVlID4gdGhpcy50aW1lQ29uc3RyYWludHNbIHR5cGUgXS5tYXggKVxuXHRcdFx0dmFsdWUgPSB0aGlzLnRpbWVDb25zdHJhaW50c1sgdHlwZSBdLm1pbiArICggdmFsdWUgLSAoIHRoaXMudGltZUNvbnN0cmFpbnRzWyB0eXBlIF0ubWF4ICsgMSApICk7XG5cdFx0cmV0dXJuIHRoaXMucGFkKCB0eXBlLCB2YWx1ZSApO1xuXHR9LFxuXG5cdGRlY3JlYXNlOiBmdW5jdGlvbiggdHlwZSApIHtcblx0XHR2YXIgdmFsdWUgPSBwYXJzZUludCggdGhpcy5zdGF0ZVsgdHlwZSBdLCAxMCkgLSB0aGlzLnRpbWVDb25zdHJhaW50c1sgdHlwZSBdLnN0ZXA7XG5cdFx0aWYgKCB2YWx1ZSA8IHRoaXMudGltZUNvbnN0cmFpbnRzWyB0eXBlIF0ubWluIClcblx0XHRcdHZhbHVlID0gdGhpcy50aW1lQ29uc3RyYWludHNbIHR5cGUgXS5tYXggKyAxIC0gKCB0aGlzLnRpbWVDb25zdHJhaW50c1sgdHlwZSBdLm1pbiAtIHZhbHVlICk7XG5cdFx0cmV0dXJuIHRoaXMucGFkKCB0eXBlLCB2YWx1ZSApO1xuXHR9LFxuXG5cdHBhZDogZnVuY3Rpb24oIHR5cGUsIHZhbHVlICkge1xuXHRcdHZhciBzdHIgPSB2YWx1ZSArICcnO1xuXHRcdHdoaWxlICggc3RyLmxlbmd0aCA8IHRoaXMucGFkVmFsdWVzWyB0eXBlIF0gKVxuXHRcdFx0c3RyID0gJzAnICsgc3RyO1xuXHRcdHJldHVybiBzdHI7XG5cdH0sXG5cblx0aGFuZGxlQ2xpY2tPdXRzaWRlOiBmdW5jdGlvbigpIHtcblx0XHR0aGlzLnByb3BzLmhhbmRsZUNsaWNrT3V0c2lkZSgpO1xuXHR9XG59KSk7XG5cbm1vZHVsZS5leHBvcnRzID0gRGF0ZVRpbWVQaWNrZXJUaW1lO1xuIiwiJ3VzZSBzdHJpY3QnO1xuXG52YXIgUmVhY3QgPSByZXF1aXJlKCdyZWFjdCcpLFxuXHRjcmVhdGVDbGFzcyA9IHJlcXVpcmUoJ2NyZWF0ZS1yZWFjdC1jbGFzcycpLFxuXHRvbkNsaWNrT3V0c2lkZSA9IHJlcXVpcmUoJ3JlYWN0LW9uY2xpY2tvdXRzaWRlJykuZGVmYXVsdFxuXHQ7XG5cbnZhciBEYXRlVGltZVBpY2tlclllYXJzID0gb25DbGlja091dHNpZGUoIGNyZWF0ZUNsYXNzKHtcblx0cmVuZGVyOiBmdW5jdGlvbigpIHtcblx0XHR2YXIgeWVhciA9IHBhcnNlSW50KCB0aGlzLnByb3BzLnZpZXdEYXRlLnllYXIoKSAvIDEwLCAxMCApICogMTA7XG5cblx0XHRyZXR1cm4gUmVhY3QuY3JlYXRlRWxlbWVudCgnZGl2JywgeyBjbGFzc05hbWU6ICdyZHRZZWFycycgfSwgW1xuXHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGFibGUnLCB7IGtleTogJ2EnIH0sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoZWFkJywge30sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RyJywge30sIFtcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGgnLCB7IGtleTogJ3ByZXYnLCBjbGFzc05hbWU6ICdyZHRQcmV2Jywgb25DbGljazogdGhpcy5wcm9wcy5zdWJ0cmFjdFRpbWUoIDEwLCAneWVhcnMnICl9LCBSZWFjdC5jcmVhdGVFbGVtZW50KCdzcGFuJywge30sICfigLknICkpLFxuXHRcdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0aCcsIHsga2V5OiAneWVhcicsIGNsYXNzTmFtZTogJ3JkdFN3aXRjaCcsIG9uQ2xpY2s6IHRoaXMucHJvcHMuc2hvd1ZpZXcoICd5ZWFycycgKSwgY29sU3BhbjogMiB9LCB5ZWFyICsgJy0nICsgKCB5ZWFyICsgOSApICksXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoJywgeyBrZXk6ICduZXh0JywgY2xhc3NOYW1lOiAncmR0TmV4dCcsIG9uQ2xpY2s6IHRoaXMucHJvcHMuYWRkVGltZSggMTAsICd5ZWFycycgKX0sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3NwYW4nLCB7fSwgJ+KAuicgKSlcblx0XHRcdF0pKSksXG5cdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0YWJsZScsIHsga2V5OiAneWVhcnMnIH0sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3Rib2R5JywgIHt9LCB0aGlzLnJlbmRlclllYXJzKCB5ZWFyICkpKVxuXHRcdF0pO1xuXHR9LFxuXG5cdHJlbmRlclllYXJzOiBmdW5jdGlvbiggeWVhciApIHtcblx0XHR2YXIgeWVhcnMgPSBbXSxcblx0XHRcdGkgPSAtMSxcblx0XHRcdHJvd3MgPSBbXSxcblx0XHRcdHJlbmRlcmVyID0gdGhpcy5wcm9wcy5yZW5kZXJZZWFyIHx8IHRoaXMucmVuZGVyWWVhcixcblx0XHRcdHNlbGVjdGVkRGF0ZSA9IHRoaXMucHJvcHMuc2VsZWN0ZWREYXRlLFxuXHRcdFx0aXNWYWxpZCA9IHRoaXMucHJvcHMuaXNWYWxpZERhdGUgfHwgdGhpcy5hbHdheXNWYWxpZERhdGUsXG5cdFx0XHRjbGFzc2VzLCBwcm9wcywgY3VycmVudFllYXIsIGlzRGlzYWJsZWQsIG5vT2ZEYXlzSW5ZZWFyLCBkYXlzSW5ZZWFyLCB2YWxpZERheSxcblx0XHRcdC8vIE1vbnRoIGFuZCBkYXRlIGFyZSBpcnJlbGV2YW50IGhlcmUgYmVjYXVzZVxuXHRcdFx0Ly8gd2UncmUgb25seSBpbnRlcmVzdGVkIGluIHRoZSB5ZWFyXG5cdFx0XHRpcnJlbGV2YW50TW9udGggPSAwLFxuXHRcdFx0aXJyZWxldmFudERhdGUgPSAxXG5cdFx0XHQ7XG5cblx0XHR5ZWFyLS07XG5cdFx0d2hpbGUgKGkgPCAxMSkge1xuXHRcdFx0Y2xhc3NlcyA9ICdyZHRZZWFyJztcblx0XHRcdGN1cnJlbnRZZWFyID0gdGhpcy5wcm9wcy52aWV3RGF0ZS5jbG9uZSgpLnNldChcblx0XHRcdFx0eyB5ZWFyOiB5ZWFyLCBtb250aDogaXJyZWxldmFudE1vbnRoLCBkYXRlOiBpcnJlbGV2YW50RGF0ZSB9ICk7XG5cblx0XHRcdC8vIE5vdCBzdXJlIHdoYXQgJ3JkdE9sZCcgaXMgZm9yLCBjb21tZW50aW5nIG91dCBmb3Igbm93IGFzIGl0J3Mgbm90IHdvcmtpbmcgcHJvcGVybHlcblx0XHRcdC8vIGlmICggaSA9PT0gLTEgfCBpID09PSAxMCApXG5cdFx0XHRcdC8vIGNsYXNzZXMgKz0gJyByZHRPbGQnO1xuXG5cdFx0XHRub09mRGF5c0luWWVhciA9IGN1cnJlbnRZZWFyLmVuZE9mKCAneWVhcicgKS5mb3JtYXQoICdEREQnICk7XG5cdFx0XHRkYXlzSW5ZZWFyID0gQXJyYXkuZnJvbSh7IGxlbmd0aDogbm9PZkRheXNJblllYXIgfSwgZnVuY3Rpb24oIGUsIGkgKSB7XG5cdFx0XHRcdHJldHVybiBpICsgMTtcblx0XHRcdH0pO1xuXG5cdFx0XHR2YWxpZERheSA9IGRheXNJblllYXIuZmluZChmdW5jdGlvbiggZCApIHtcblx0XHRcdFx0dmFyIGRheSA9IGN1cnJlbnRZZWFyLmNsb25lKCkuZGF5T2ZZZWFyKCBkICk7XG5cdFx0XHRcdHJldHVybiBpc1ZhbGlkKCBkYXkgKTtcblx0XHRcdH0pO1xuXG5cdFx0XHRpc0Rpc2FibGVkID0gKCB2YWxpZERheSA9PT0gdW5kZWZpbmVkICk7XG5cblx0XHRcdGlmICggaXNEaXNhYmxlZCApXG5cdFx0XHRcdGNsYXNzZXMgKz0gJyByZHREaXNhYmxlZCc7XG5cblx0XHRcdGlmICggc2VsZWN0ZWREYXRlICYmIHNlbGVjdGVkRGF0ZS55ZWFyKCkgPT09IHllYXIgKVxuXHRcdFx0XHRjbGFzc2VzICs9ICcgcmR0QWN0aXZlJztcblxuXHRcdFx0cHJvcHMgPSB7XG5cdFx0XHRcdGtleTogeWVhcixcblx0XHRcdFx0J2RhdGEtdmFsdWUnOiB5ZWFyLFxuXHRcdFx0XHRjbGFzc05hbWU6IGNsYXNzZXNcblx0XHRcdH07XG5cblx0XHRcdGlmICggIWlzRGlzYWJsZWQgKVxuXHRcdFx0XHRwcm9wcy5vbkNsaWNrID0gKCB0aGlzLnByb3BzLnVwZGF0ZU9uID09PSAneWVhcnMnID9cblx0XHRcdFx0XHR0aGlzLnVwZGF0ZVNlbGVjdGVkWWVhciA6IHRoaXMucHJvcHMuc2V0RGF0ZSgneWVhcicpICk7XG5cblx0XHRcdHllYXJzLnB1c2goIHJlbmRlcmVyKCBwcm9wcywgeWVhciwgc2VsZWN0ZWREYXRlICYmIHNlbGVjdGVkRGF0ZS5jbG9uZSgpICkpO1xuXG5cdFx0XHRpZiAoIHllYXJzLmxlbmd0aCA9PT0gNCApIHtcblx0XHRcdFx0cm93cy5wdXNoKCBSZWFjdC5jcmVhdGVFbGVtZW50KCd0cicsIHsga2V5OiBpIH0sIHllYXJzICkgKTtcblx0XHRcdFx0eWVhcnMgPSBbXTtcblx0XHRcdH1cblxuXHRcdFx0eWVhcisrO1xuXHRcdFx0aSsrO1xuXHRcdH1cblxuXHRcdHJldHVybiByb3dzO1xuXHR9LFxuXG5cdHVwZGF0ZVNlbGVjdGVkWWVhcjogZnVuY3Rpb24oIGV2ZW50ICkge1xuXHRcdHRoaXMucHJvcHMudXBkYXRlU2VsZWN0ZWREYXRlKCBldmVudCApO1xuXHR9LFxuXG5cdHJlbmRlclllYXI6IGZ1bmN0aW9uKCBwcm9wcywgeWVhciApIHtcblx0XHRyZXR1cm4gUmVhY3QuY3JlYXRlRWxlbWVudCgndGQnLCAgcHJvcHMsIHllYXIgKTtcblx0fSxcblxuXHRhbHdheXNWYWxpZERhdGU6IGZ1bmN0aW9uKCkge1xuXHRcdHJldHVybiAxO1xuXHR9LFxuXG5cdGhhbmRsZUNsaWNrT3V0c2lkZTogZnVuY3Rpb24oKSB7XG5cdFx0dGhpcy5wcm9wcy5oYW5kbGVDbGlja091dHNpZGUoKTtcblx0fVxufSkpO1xuXG5tb2R1bGUuZXhwb3J0cyA9IERhdGVUaW1lUGlja2VyWWVhcnM7XG4iLCJpbXBvcnQge2NyZWF0ZUVsZW1lbnQsQ29tcG9uZW50fWZyb20ncmVhY3QnO2ltcG9ydCB7ZmluZERPTU5vZGV9ZnJvbSdyZWFjdC1kb20nO2Z1bmN0aW9uIF9pbmhlcml0c0xvb3NlKHN1YkNsYXNzLCBzdXBlckNsYXNzKSB7XG4gIHN1YkNsYXNzLnByb3RvdHlwZSA9IE9iamVjdC5jcmVhdGUoc3VwZXJDbGFzcy5wcm90b3R5cGUpO1xuICBzdWJDbGFzcy5wcm90b3R5cGUuY29uc3RydWN0b3IgPSBzdWJDbGFzcztcblxuICBfc2V0UHJvdG90eXBlT2Yoc3ViQ2xhc3MsIHN1cGVyQ2xhc3MpO1xufVxuXG5mdW5jdGlvbiBfc2V0UHJvdG90eXBlT2YobywgcCkge1xuICBfc2V0UHJvdG90eXBlT2YgPSBPYmplY3Quc2V0UHJvdG90eXBlT2YgfHwgZnVuY3Rpb24gX3NldFByb3RvdHlwZU9mKG8sIHApIHtcbiAgICBvLl9fcHJvdG9fXyA9IHA7XG4gICAgcmV0dXJuIG87XG4gIH07XG5cbiAgcmV0dXJuIF9zZXRQcm90b3R5cGVPZihvLCBwKTtcbn1cblxuZnVuY3Rpb24gX29iamVjdFdpdGhvdXRQcm9wZXJ0aWVzTG9vc2Uoc291cmNlLCBleGNsdWRlZCkge1xuICBpZiAoc291cmNlID09IG51bGwpIHJldHVybiB7fTtcbiAgdmFyIHRhcmdldCA9IHt9O1xuICB2YXIgc291cmNlS2V5cyA9IE9iamVjdC5rZXlzKHNvdXJjZSk7XG4gIHZhciBrZXksIGk7XG5cbiAgZm9yIChpID0gMDsgaSA8IHNvdXJjZUtleXMubGVuZ3RoOyBpKyspIHtcbiAgICBrZXkgPSBzb3VyY2VLZXlzW2ldO1xuICAgIGlmIChleGNsdWRlZC5pbmRleE9mKGtleSkgPj0gMCkgY29udGludWU7XG4gICAgdGFyZ2V0W2tleV0gPSBzb3VyY2Vba2V5XTtcbiAgfVxuXG4gIHJldHVybiB0YXJnZXQ7XG59XG5cbmZ1bmN0aW9uIF9hc3NlcnRUaGlzSW5pdGlhbGl6ZWQoc2VsZikge1xuICBpZiAoc2VsZiA9PT0gdm9pZCAwKSB7XG4gICAgdGhyb3cgbmV3IFJlZmVyZW5jZUVycm9yKFwidGhpcyBoYXNuJ3QgYmVlbiBpbml0aWFsaXNlZCAtIHN1cGVyKCkgaGFzbid0IGJlZW4gY2FsbGVkXCIpO1xuICB9XG5cbiAgcmV0dXJuIHNlbGY7XG59LyoqXG4gKiBDaGVjayB3aGV0aGVyIHNvbWUgRE9NIG5vZGUgaXMgb3VyIENvbXBvbmVudCdzIG5vZGUuXG4gKi9cbmZ1bmN0aW9uIGlzTm9kZUZvdW5kKGN1cnJlbnQsIGNvbXBvbmVudE5vZGUsIGlnbm9yZUNsYXNzKSB7XG4gIGlmIChjdXJyZW50ID09PSBjb21wb25lbnROb2RlKSB7XG4gICAgcmV0dXJuIHRydWU7XG4gIH0gLy8gU1ZHIDx1c2UvPiBlbGVtZW50cyBkbyBub3QgdGVjaG5pY2FsbHkgcmVzaWRlIGluIHRoZSByZW5kZXJlZCBET00sIHNvXG4gIC8vIHRoZXkgZG8gbm90IGhhdmUgY2xhc3NMaXN0IGRpcmVjdGx5LCBidXQgdGhleSBvZmZlciBhIGxpbmsgdG8gdGhlaXJcbiAgLy8gY29ycmVzcG9uZGluZyBlbGVtZW50LCB3aGljaCBjYW4gaGF2ZSBjbGFzc0xpc3QuIFRoaXMgZXh0cmEgY2hlY2sgaXMgZm9yXG4gIC8vIHRoYXQgY2FzZS5cbiAgLy8gU2VlOiBodHRwOi8vd3d3LnczLm9yZy9UUi9TVkcxMS9zdHJ1Y3QuaHRtbCNJbnRlcmZhY2VTVkdVc2VFbGVtZW50XG4gIC8vIERpc2N1c3Npb246IGh0dHBzOi8vZ2l0aHViLmNvbS9Qb21heC9yZWFjdC1vbmNsaWNrb3V0c2lkZS9wdWxsLzE3XG5cblxuICBpZiAoY3VycmVudC5jb3JyZXNwb25kaW5nRWxlbWVudCkge1xuICAgIHJldHVybiBjdXJyZW50LmNvcnJlc3BvbmRpbmdFbGVtZW50LmNsYXNzTGlzdC5jb250YWlucyhpZ25vcmVDbGFzcyk7XG4gIH1cblxuICByZXR1cm4gY3VycmVudC5jbGFzc0xpc3QuY29udGFpbnMoaWdub3JlQ2xhc3MpO1xufVxuLyoqXG4gKiBUcnkgdG8gZmluZCBvdXIgbm9kZSBpbiBhIGhpZXJhcmNoeSBvZiBub2RlcywgcmV0dXJuaW5nIHRoZSBkb2N1bWVudFxuICogbm9kZSBhcyBoaWdoZXN0IG5vZGUgaWYgb3VyIG5vZGUgaXMgbm90IGZvdW5kIGluIHRoZSBwYXRoIHVwLlxuICovXG5cbmZ1bmN0aW9uIGZpbmRIaWdoZXN0KGN1cnJlbnQsIGNvbXBvbmVudE5vZGUsIGlnbm9yZUNsYXNzKSB7XG4gIGlmIChjdXJyZW50ID09PSBjb21wb25lbnROb2RlKSB7XG4gICAgcmV0dXJuIHRydWU7XG4gIH0gLy8gSWYgc291cmNlPWxvY2FsIHRoZW4gdGhpcyBldmVudCBjYW1lIGZyb20gJ3NvbWV3aGVyZSdcbiAgLy8gaW5zaWRlIGFuZCBzaG91bGQgYmUgaWdub3JlZC4gV2UgY291bGQgaGFuZGxlIHRoaXMgd2l0aFxuICAvLyBhIGxheWVyZWQgYXBwcm9hY2gsIHRvbywgYnV0IHRoYXQgcmVxdWlyZXMgZ29pbmcgYmFjayB0b1xuICAvLyB0aGlua2luZyBpbiB0ZXJtcyBvZiBEb20gbm9kZSBuZXN0aW5nLCBydW5uaW5nIGNvdW50ZXJcbiAgLy8gdG8gUmVhY3QncyAneW91IHNob3VsZG4ndCBjYXJlIGFib3V0IHRoZSBET00nIHBoaWxvc29waHkuXG4gIC8vIEFsc28gY292ZXIgc2hhZG93Um9vdCBub2RlIGJ5IGNoZWNraW5nIGN1cnJlbnQuaG9zdFxuXG5cbiAgd2hpbGUgKGN1cnJlbnQucGFyZW50Tm9kZSB8fCBjdXJyZW50Lmhvc3QpIHtcbiAgICAvLyBPbmx5IGNoZWNrIG5vcm1hbCBub2RlIHdpdGhvdXQgc2hhZG93Um9vdFxuICAgIGlmIChjdXJyZW50LnBhcmVudE5vZGUgJiYgaXNOb2RlRm91bmQoY3VycmVudCwgY29tcG9uZW50Tm9kZSwgaWdub3JlQ2xhc3MpKSB7XG4gICAgICByZXR1cm4gdHJ1ZTtcbiAgICB9XG5cbiAgICBjdXJyZW50ID0gY3VycmVudC5wYXJlbnROb2RlIHx8IGN1cnJlbnQuaG9zdDtcbiAgfVxuXG4gIHJldHVybiBjdXJyZW50O1xufVxuLyoqXG4gKiBDaGVjayBpZiB0aGUgYnJvd3NlciBzY3JvbGxiYXIgd2FzIGNsaWNrZWRcbiAqL1xuXG5mdW5jdGlvbiBjbGlja2VkU2Nyb2xsYmFyKGV2dCkge1xuICByZXR1cm4gZG9jdW1lbnQuZG9jdW1lbnRFbGVtZW50LmNsaWVudFdpZHRoIDw9IGV2dC5jbGllbnRYIHx8IGRvY3VtZW50LmRvY3VtZW50RWxlbWVudC5jbGllbnRIZWlnaHQgPD0gZXZ0LmNsaWVudFk7XG59Ly8gaWRlYWxseSB3aWxsIGdldCByZXBsYWNlZCB3aXRoIGV4dGVybmFsIGRlcFxuLy8gd2hlbiByYWZyZXgvZGV0ZWN0LXBhc3NpdmUtZXZlbnRzIzQgYW5kIHJhZnJleC9kZXRlY3QtcGFzc2l2ZS1ldmVudHMjNSBnZXQgbWVyZ2VkIGluXG52YXIgdGVzdFBhc3NpdmVFdmVudFN1cHBvcnQgPSBmdW5jdGlvbiB0ZXN0UGFzc2l2ZUV2ZW50U3VwcG9ydCgpIHtcbiAgaWYgKHR5cGVvZiB3aW5kb3cgPT09ICd1bmRlZmluZWQnIHx8IHR5cGVvZiB3aW5kb3cuYWRkRXZlbnRMaXN0ZW5lciAhPT0gJ2Z1bmN0aW9uJykge1xuICAgIHJldHVybjtcbiAgfVxuXG4gIHZhciBwYXNzaXZlID0gZmFsc2U7XG4gIHZhciBvcHRpb25zID0gT2JqZWN0LmRlZmluZVByb3BlcnR5KHt9LCAncGFzc2l2ZScsIHtcbiAgICBnZXQ6IGZ1bmN0aW9uIGdldCgpIHtcbiAgICAgIHBhc3NpdmUgPSB0cnVlO1xuICAgIH1cbiAgfSk7XG5cbiAgdmFyIG5vb3AgPSBmdW5jdGlvbiBub29wKCkge307XG5cbiAgd2luZG93LmFkZEV2ZW50TGlzdGVuZXIoJ3Rlc3RQYXNzaXZlRXZlbnRTdXBwb3J0Jywgbm9vcCwgb3B0aW9ucyk7XG4gIHdpbmRvdy5yZW1vdmVFdmVudExpc3RlbmVyKCd0ZXN0UGFzc2l2ZUV2ZW50U3VwcG9ydCcsIG5vb3AsIG9wdGlvbnMpO1xuICByZXR1cm4gcGFzc2l2ZTtcbn07ZnVuY3Rpb24gYXV0b0luYyhzZWVkKSB7XG4gIGlmIChzZWVkID09PSB2b2lkIDApIHtcbiAgICBzZWVkID0gMDtcbiAgfVxuXG4gIHJldHVybiBmdW5jdGlvbiAoKSB7XG4gICAgcmV0dXJuICsrc2VlZDtcbiAgfTtcbn1cblxudmFyIHVpZCA9IGF1dG9JbmMoKTt2YXIgcGFzc2l2ZUV2ZW50U3VwcG9ydDtcbnZhciBoYW5kbGVyc01hcCA9IHt9O1xudmFyIGVuYWJsZWRJbnN0YW5jZXMgPSB7fTtcbnZhciB0b3VjaEV2ZW50cyA9IFsndG91Y2hzdGFydCcsICd0b3VjaG1vdmUnXTtcbnZhciBJR05PUkVfQ0xBU1NfTkFNRSA9ICdpZ25vcmUtcmVhY3Qtb25jbGlja291dHNpZGUnO1xuLyoqXG4gKiBPcHRpb25zIGZvciBhZGRFdmVudEhhbmRsZXIgYW5kIHJlbW92ZUV2ZW50SGFuZGxlclxuICovXG5cbmZ1bmN0aW9uIGdldEV2ZW50SGFuZGxlck9wdGlvbnMoaW5zdGFuY2UsIGV2ZW50TmFtZSkge1xuICB2YXIgaGFuZGxlck9wdGlvbnMgPSB7fTtcbiAgdmFyIGlzVG91Y2hFdmVudCA9IHRvdWNoRXZlbnRzLmluZGV4T2YoZXZlbnROYW1lKSAhPT0gLTE7XG5cbiAgaWYgKGlzVG91Y2hFdmVudCAmJiBwYXNzaXZlRXZlbnRTdXBwb3J0KSB7XG4gICAgaGFuZGxlck9wdGlvbnMucGFzc2l2ZSA9ICFpbnN0YW5jZS5wcm9wcy5wcmV2ZW50RGVmYXVsdDtcbiAgfVxuXG4gIHJldHVybiBoYW5kbGVyT3B0aW9ucztcbn1cbi8qKlxuICogVGhpcyBmdW5jdGlvbiBnZW5lcmF0ZXMgdGhlIEhPQyBmdW5jdGlvbiB0aGF0IHlvdSdsbCB1c2VcbiAqIGluIG9yZGVyIHRvIGltcGFydCBvbk91dHNpZGVDbGljayBsaXN0ZW5pbmcgdG8gYW5cbiAqIGFyYml0cmFyeSBjb21wb25lbnQuIEl0IGdldHMgY2FsbGVkIGF0IHRoZSBlbmQgb2YgdGhlXG4gKiBib290c3RyYXBwaW5nIGNvZGUgdG8geWllbGQgYW4gaW5zdGFuY2Ugb2YgdGhlXG4gKiBvbkNsaWNrT3V0c2lkZUhPQyBmdW5jdGlvbiBkZWZpbmVkIGluc2lkZSBzZXR1cEhPQygpLlxuICovXG5cblxuZnVuY3Rpb24gb25DbGlja091dHNpZGVIT0MoV3JhcHBlZENvbXBvbmVudCwgY29uZmlnKSB7XG4gIHZhciBfY2xhc3MsIF90ZW1wO1xuXG4gIHZhciBjb21wb25lbnROYW1lID0gV3JhcHBlZENvbXBvbmVudC5kaXNwbGF5TmFtZSB8fCBXcmFwcGVkQ29tcG9uZW50Lm5hbWUgfHwgJ0NvbXBvbmVudCc7XG4gIHJldHVybiBfdGVtcCA9IF9jbGFzcyA9IC8qI19fUFVSRV9fKi9mdW5jdGlvbiAoX0NvbXBvbmVudCkge1xuICAgIF9pbmhlcml0c0xvb3NlKG9uQ2xpY2tPdXRzaWRlLCBfQ29tcG9uZW50KTtcblxuICAgIGZ1bmN0aW9uIG9uQ2xpY2tPdXRzaWRlKHByb3BzKSB7XG4gICAgICB2YXIgX3RoaXM7XG5cbiAgICAgIF90aGlzID0gX0NvbXBvbmVudC5jYWxsKHRoaXMsIHByb3BzKSB8fCB0aGlzO1xuXG4gICAgICBfdGhpcy5fX291dHNpZGVDbGlja0hhbmRsZXIgPSBmdW5jdGlvbiAoZXZlbnQpIHtcbiAgICAgICAgaWYgKHR5cGVvZiBfdGhpcy5fX2NsaWNrT3V0c2lkZUhhbmRsZXJQcm9wID09PSAnZnVuY3Rpb24nKSB7XG4gICAgICAgICAgX3RoaXMuX19jbGlja091dHNpZGVIYW5kbGVyUHJvcChldmVudCk7XG5cbiAgICAgICAgICByZXR1cm47XG4gICAgICAgIH1cblxuICAgICAgICB2YXIgaW5zdGFuY2UgPSBfdGhpcy5nZXRJbnN0YW5jZSgpO1xuXG4gICAgICAgIGlmICh0eXBlb2YgaW5zdGFuY2UucHJvcHMuaGFuZGxlQ2xpY2tPdXRzaWRlID09PSAnZnVuY3Rpb24nKSB7XG4gICAgICAgICAgaW5zdGFuY2UucHJvcHMuaGFuZGxlQ2xpY2tPdXRzaWRlKGV2ZW50KTtcbiAgICAgICAgICByZXR1cm47XG4gICAgICAgIH1cblxuICAgICAgICBpZiAodHlwZW9mIGluc3RhbmNlLmhhbmRsZUNsaWNrT3V0c2lkZSA9PT0gJ2Z1bmN0aW9uJykge1xuICAgICAgICAgIGluc3RhbmNlLmhhbmRsZUNsaWNrT3V0c2lkZShldmVudCk7XG4gICAgICAgICAgcmV0dXJuO1xuICAgICAgICB9XG5cbiAgICAgICAgdGhyb3cgbmV3IEVycm9yKFwiV3JhcHBlZENvbXBvbmVudDogXCIgKyBjb21wb25lbnROYW1lICsgXCIgbGFja3MgYSBoYW5kbGVDbGlja091dHNpZGUoZXZlbnQpIGZ1bmN0aW9uIGZvciBwcm9jZXNzaW5nIG91dHNpZGUgY2xpY2sgZXZlbnRzLlwiKTtcbiAgICAgIH07XG5cbiAgICAgIF90aGlzLl9fZ2V0Q29tcG9uZW50Tm9kZSA9IGZ1bmN0aW9uICgpIHtcbiAgICAgICAgdmFyIGluc3RhbmNlID0gX3RoaXMuZ2V0SW5zdGFuY2UoKTtcblxuICAgICAgICBpZiAoY29uZmlnICYmIHR5cGVvZiBjb25maWcuc2V0Q2xpY2tPdXRzaWRlUmVmID09PSAnZnVuY3Rpb24nKSB7XG4gICAgICAgICAgcmV0dXJuIGNvbmZpZy5zZXRDbGlja091dHNpZGVSZWYoKShpbnN0YW5jZSk7XG4gICAgICAgIH1cblxuICAgICAgICBpZiAodHlwZW9mIGluc3RhbmNlLnNldENsaWNrT3V0c2lkZVJlZiA9PT0gJ2Z1bmN0aW9uJykge1xuICAgICAgICAgIHJldHVybiBpbnN0YW5jZS5zZXRDbGlja091dHNpZGVSZWYoKTtcbiAgICAgICAgfVxuXG4gICAgICAgIHJldHVybiBmaW5kRE9NTm9kZShpbnN0YW5jZSk7XG4gICAgICB9O1xuXG4gICAgICBfdGhpcy5lbmFibGVPbkNsaWNrT3V0c2lkZSA9IGZ1bmN0aW9uICgpIHtcbiAgICAgICAgaWYgKHR5cGVvZiBkb2N1bWVudCA9PT0gJ3VuZGVmaW5lZCcgfHwgZW5hYmxlZEluc3RhbmNlc1tfdGhpcy5fdWlkXSkge1xuICAgICAgICAgIHJldHVybjtcbiAgICAgICAgfVxuXG4gICAgICAgIGlmICh0eXBlb2YgcGFzc2l2ZUV2ZW50U3VwcG9ydCA9PT0gJ3VuZGVmaW5lZCcpIHtcbiAgICAgICAgICBwYXNzaXZlRXZlbnRTdXBwb3J0ID0gdGVzdFBhc3NpdmVFdmVudFN1cHBvcnQoKTtcbiAgICAgICAgfVxuXG4gICAgICAgIGVuYWJsZWRJbnN0YW5jZXNbX3RoaXMuX3VpZF0gPSB0cnVlO1xuICAgICAgICB2YXIgZXZlbnRzID0gX3RoaXMucHJvcHMuZXZlbnRUeXBlcztcblxuICAgICAgICBpZiAoIWV2ZW50cy5mb3JFYWNoKSB7XG4gICAgICAgICAgZXZlbnRzID0gW2V2ZW50c107XG4gICAgICAgIH1cblxuICAgICAgICBoYW5kbGVyc01hcFtfdGhpcy5fdWlkXSA9IGZ1bmN0aW9uIChldmVudCkge1xuICAgICAgICAgIGlmIChfdGhpcy5jb21wb25lbnROb2RlID09PSBudWxsKSByZXR1cm47XG4gICAgICAgICAgaWYgKF90aGlzLmluaXRUaW1lU3RhbXAgPiBldmVudC50aW1lU3RhbXApIHJldHVybjtcblxuICAgICAgICAgIGlmIChfdGhpcy5wcm9wcy5wcmV2ZW50RGVmYXVsdCkge1xuICAgICAgICAgICAgZXZlbnQucHJldmVudERlZmF1bHQoKTtcbiAgICAgICAgICB9XG5cbiAgICAgICAgICBpZiAoX3RoaXMucHJvcHMuc3RvcFByb3BhZ2F0aW9uKSB7XG4gICAgICAgICAgICBldmVudC5zdG9wUHJvcGFnYXRpb24oKTtcbiAgICAgICAgICB9XG5cbiAgICAgICAgICBpZiAoX3RoaXMucHJvcHMuZXhjbHVkZVNjcm9sbGJhciAmJiBjbGlja2VkU2Nyb2xsYmFyKGV2ZW50KSkgcmV0dXJuO1xuICAgICAgICAgIHZhciBjdXJyZW50ID0gZXZlbnQuY29tcG9zZWQgJiYgZXZlbnQuY29tcG9zZWRQYXRoICYmIGV2ZW50LmNvbXBvc2VkUGF0aCgpLnNoaWZ0KCkgfHwgZXZlbnQudGFyZ2V0O1xuXG4gICAgICAgICAgaWYgKGZpbmRIaWdoZXN0KGN1cnJlbnQsIF90aGlzLmNvbXBvbmVudE5vZGUsIF90aGlzLnByb3BzLm91dHNpZGVDbGlja0lnbm9yZUNsYXNzKSAhPT0gZG9jdW1lbnQpIHtcbiAgICAgICAgICAgIHJldHVybjtcbiAgICAgICAgICB9XG5cbiAgICAgICAgICBfdGhpcy5fX291dHNpZGVDbGlja0hhbmRsZXIoZXZlbnQpO1xuICAgICAgICB9O1xuXG4gICAgICAgIGV2ZW50cy5mb3JFYWNoKGZ1bmN0aW9uIChldmVudE5hbWUpIHtcbiAgICAgICAgICBkb2N1bWVudC5hZGRFdmVudExpc3RlbmVyKGV2ZW50TmFtZSwgaGFuZGxlcnNNYXBbX3RoaXMuX3VpZF0sIGdldEV2ZW50SGFuZGxlck9wdGlvbnMoX2Fzc2VydFRoaXNJbml0aWFsaXplZChfdGhpcyksIGV2ZW50TmFtZSkpO1xuICAgICAgICB9KTtcbiAgICAgIH07XG5cbiAgICAgIF90aGlzLmRpc2FibGVPbkNsaWNrT3V0c2lkZSA9IGZ1bmN0aW9uICgpIHtcbiAgICAgICAgZGVsZXRlIGVuYWJsZWRJbnN0YW5jZXNbX3RoaXMuX3VpZF07XG4gICAgICAgIHZhciBmbiA9IGhhbmRsZXJzTWFwW190aGlzLl91aWRdO1xuXG4gICAgICAgIGlmIChmbiAmJiB0eXBlb2YgZG9jdW1lbnQgIT09ICd1bmRlZmluZWQnKSB7XG4gICAgICAgICAgdmFyIGV2ZW50cyA9IF90aGlzLnByb3BzLmV2ZW50VHlwZXM7XG5cbiAgICAgICAgICBpZiAoIWV2ZW50cy5mb3JFYWNoKSB7XG4gICAgICAgICAgICBldmVudHMgPSBbZXZlbnRzXTtcbiAgICAgICAgICB9XG5cbiAgICAgICAgICBldmVudHMuZm9yRWFjaChmdW5jdGlvbiAoZXZlbnROYW1lKSB7XG4gICAgICAgICAgICByZXR1cm4gZG9jdW1lbnQucmVtb3ZlRXZlbnRMaXN0ZW5lcihldmVudE5hbWUsIGZuLCBnZXRFdmVudEhhbmRsZXJPcHRpb25zKF9hc3NlcnRUaGlzSW5pdGlhbGl6ZWQoX3RoaXMpLCBldmVudE5hbWUpKTtcbiAgICAgICAgICB9KTtcbiAgICAgICAgICBkZWxldGUgaGFuZGxlcnNNYXBbX3RoaXMuX3VpZF07XG4gICAgICAgIH1cbiAgICAgIH07XG5cbiAgICAgIF90aGlzLmdldFJlZiA9IGZ1bmN0aW9uIChyZWYpIHtcbiAgICAgICAgcmV0dXJuIF90aGlzLmluc3RhbmNlUmVmID0gcmVmO1xuICAgICAgfTtcblxuICAgICAgX3RoaXMuX3VpZCA9IHVpZCgpO1xuICAgICAgX3RoaXMuaW5pdFRpbWVTdGFtcCA9IHBlcmZvcm1hbmNlLm5vdygpO1xuICAgICAgcmV0dXJuIF90aGlzO1xuICAgIH1cbiAgICAvKipcbiAgICAgKiBBY2Nlc3MgdGhlIFdyYXBwZWRDb21wb25lbnQncyBpbnN0YW5jZS5cbiAgICAgKi9cblxuXG4gICAgdmFyIF9wcm90byA9IG9uQ2xpY2tPdXRzaWRlLnByb3RvdHlwZTtcblxuICAgIF9wcm90by5nZXRJbnN0YW5jZSA9IGZ1bmN0aW9uIGdldEluc3RhbmNlKCkge1xuICAgICAgaWYgKFdyYXBwZWRDb21wb25lbnQucHJvdG90eXBlICYmICFXcmFwcGVkQ29tcG9uZW50LnByb3RvdHlwZS5pc1JlYWN0Q29tcG9uZW50KSB7XG4gICAgICAgIHJldHVybiB0aGlzO1xuICAgICAgfVxuXG4gICAgICB2YXIgcmVmID0gdGhpcy5pbnN0YW5jZVJlZjtcbiAgICAgIHJldHVybiByZWYuZ2V0SW5zdGFuY2UgPyByZWYuZ2V0SW5zdGFuY2UoKSA6IHJlZjtcbiAgICB9O1xuXG4gICAgLyoqXG4gICAgICogQWRkIGNsaWNrIGxpc3RlbmVycyB0byB0aGUgY3VycmVudCBkb2N1bWVudCxcbiAgICAgKiBsaW5rZWQgdG8gdGhpcyBjb21wb25lbnQncyBzdGF0ZS5cbiAgICAgKi9cbiAgICBfcHJvdG8uY29tcG9uZW50RGlkTW91bnQgPSBmdW5jdGlvbiBjb21wb25lbnREaWRNb3VudCgpIHtcbiAgICAgIC8vIElmIHdlIGFyZSBpbiBhbiBlbnZpcm9ubWVudCB3aXRob3V0IGEgRE9NIHN1Y2hcbiAgICAgIC8vIGFzIHNoYWxsb3cgcmVuZGVyaW5nIG9yIHNuYXBzaG90cyB0aGVuIHdlIGV4aXRcbiAgICAgIC8vIGVhcmx5IHRvIHByZXZlbnQgYW55IHVuaGFuZGxlZCBlcnJvcnMgYmVpbmcgdGhyb3duLlxuICAgICAgaWYgKHR5cGVvZiBkb2N1bWVudCA9PT0gJ3VuZGVmaW5lZCcgfHwgIWRvY3VtZW50LmNyZWF0ZUVsZW1lbnQpIHtcbiAgICAgICAgcmV0dXJuO1xuICAgICAgfVxuXG4gICAgICB2YXIgaW5zdGFuY2UgPSB0aGlzLmdldEluc3RhbmNlKCk7XG5cbiAgICAgIGlmIChjb25maWcgJiYgdHlwZW9mIGNvbmZpZy5oYW5kbGVDbGlja091dHNpZGUgPT09ICdmdW5jdGlvbicpIHtcbiAgICAgICAgdGhpcy5fX2NsaWNrT3V0c2lkZUhhbmRsZXJQcm9wID0gY29uZmlnLmhhbmRsZUNsaWNrT3V0c2lkZShpbnN0YW5jZSk7XG5cbiAgICAgICAgaWYgKHR5cGVvZiB0aGlzLl9fY2xpY2tPdXRzaWRlSGFuZGxlclByb3AgIT09ICdmdW5jdGlvbicpIHtcbiAgICAgICAgICB0aHJvdyBuZXcgRXJyb3IoXCJXcmFwcGVkQ29tcG9uZW50OiBcIiArIGNvbXBvbmVudE5hbWUgKyBcIiBsYWNrcyBhIGZ1bmN0aW9uIGZvciBwcm9jZXNzaW5nIG91dHNpZGUgY2xpY2sgZXZlbnRzIHNwZWNpZmllZCBieSB0aGUgaGFuZGxlQ2xpY2tPdXRzaWRlIGNvbmZpZyBvcHRpb24uXCIpO1xuICAgICAgICB9XG4gICAgICB9XG5cbiAgICAgIHRoaXMuY29tcG9uZW50Tm9kZSA9IHRoaXMuX19nZXRDb21wb25lbnROb2RlKCk7IC8vIHJldHVybiBlYXJseSBzbyB3ZSBkb250IGluaXRpYXRlIG9uQ2xpY2tPdXRzaWRlXG5cbiAgICAgIGlmICh0aGlzLnByb3BzLmRpc2FibGVPbkNsaWNrT3V0c2lkZSkgcmV0dXJuO1xuICAgICAgdGhpcy5lbmFibGVPbkNsaWNrT3V0c2lkZSgpO1xuICAgIH07XG5cbiAgICBfcHJvdG8uY29tcG9uZW50RGlkVXBkYXRlID0gZnVuY3Rpb24gY29tcG9uZW50RGlkVXBkYXRlKCkge1xuICAgICAgdGhpcy5jb21wb25lbnROb2RlID0gdGhpcy5fX2dldENvbXBvbmVudE5vZGUoKTtcbiAgICB9XG4gICAgLyoqXG4gICAgICogUmVtb3ZlIGFsbCBkb2N1bWVudCdzIGV2ZW50IGxpc3RlbmVycyBmb3IgdGhpcyBjb21wb25lbnRcbiAgICAgKi9cbiAgICA7XG5cbiAgICBfcHJvdG8uY29tcG9uZW50V2lsbFVubW91bnQgPSBmdW5jdGlvbiBjb21wb25lbnRXaWxsVW5tb3VudCgpIHtcbiAgICAgIHRoaXMuZGlzYWJsZU9uQ2xpY2tPdXRzaWRlKCk7XG4gICAgfVxuICAgIC8qKlxuICAgICAqIENhbiBiZSBjYWxsZWQgdG8gZXhwbGljaXRseSBlbmFibGUgZXZlbnQgbGlzdGVuaW5nXG4gICAgICogZm9yIGNsaWNrcyBhbmQgdG91Y2hlcyBvdXRzaWRlIG9mIHRoaXMgZWxlbWVudC5cbiAgICAgKi9cbiAgICA7XG5cbiAgICAvKipcbiAgICAgKiBQYXNzLXRocm91Z2ggcmVuZGVyXG4gICAgICovXG4gICAgX3Byb3RvLnJlbmRlciA9IGZ1bmN0aW9uIHJlbmRlcigpIHtcbiAgICAgIC8vIGVzbGludC1kaXNhYmxlLW5leHQtbGluZSBuby11bnVzZWQtdmFyc1xuICAgICAgdmFyIF90aGlzJHByb3BzID0gdGhpcy5wcm9wcztcbiAgICAgICAgICBfdGhpcyRwcm9wcy5leGNsdWRlU2Nyb2xsYmFyO1xuICAgICAgICAgIHZhciBwcm9wcyA9IF9vYmplY3RXaXRob3V0UHJvcGVydGllc0xvb3NlKF90aGlzJHByb3BzLCBbXCJleGNsdWRlU2Nyb2xsYmFyXCJdKTtcblxuICAgICAgaWYgKFdyYXBwZWRDb21wb25lbnQucHJvdG90eXBlICYmIFdyYXBwZWRDb21wb25lbnQucHJvdG90eXBlLmlzUmVhY3RDb21wb25lbnQpIHtcbiAgICAgICAgcHJvcHMucmVmID0gdGhpcy5nZXRSZWY7XG4gICAgICB9IGVsc2Uge1xuICAgICAgICBwcm9wcy53cmFwcGVkUmVmID0gdGhpcy5nZXRSZWY7XG4gICAgICB9XG5cbiAgICAgIHByb3BzLmRpc2FibGVPbkNsaWNrT3V0c2lkZSA9IHRoaXMuZGlzYWJsZU9uQ2xpY2tPdXRzaWRlO1xuICAgICAgcHJvcHMuZW5hYmxlT25DbGlja091dHNpZGUgPSB0aGlzLmVuYWJsZU9uQ2xpY2tPdXRzaWRlO1xuICAgICAgcmV0dXJuIGNyZWF0ZUVsZW1lbnQoV3JhcHBlZENvbXBvbmVudCwgcHJvcHMpO1xuICAgIH07XG5cbiAgICByZXR1cm4gb25DbGlja091dHNpZGU7XG4gIH0oQ29tcG9uZW50KSwgX2NsYXNzLmRpc3BsYXlOYW1lID0gXCJPbkNsaWNrT3V0c2lkZShcIiArIGNvbXBvbmVudE5hbWUgKyBcIilcIiwgX2NsYXNzLmRlZmF1bHRQcm9wcyA9IHtcbiAgICBldmVudFR5cGVzOiBbJ21vdXNlZG93bicsICd0b3VjaHN0YXJ0J10sXG4gICAgZXhjbHVkZVNjcm9sbGJhcjogY29uZmlnICYmIGNvbmZpZy5leGNsdWRlU2Nyb2xsYmFyIHx8IGZhbHNlLFxuICAgIG91dHNpZGVDbGlja0lnbm9yZUNsYXNzOiBJR05PUkVfQ0xBU1NfTkFNRSxcbiAgICBwcmV2ZW50RGVmYXVsdDogZmFsc2UsXG4gICAgc3RvcFByb3BhZ2F0aW9uOiBmYWxzZVxuICB9LCBfY2xhc3MuZ2V0Q2xhc3MgPSBmdW5jdGlvbiAoKSB7XG4gICAgcmV0dXJuIFdyYXBwZWRDb21wb25lbnQuZ2V0Q2xhc3MgPyBXcmFwcGVkQ29tcG9uZW50LmdldENsYXNzKCkgOiBXcmFwcGVkQ29tcG9uZW50O1xuICB9LCBfdGVtcDtcbn1leHBvcnQgZGVmYXVsdCBvbkNsaWNrT3V0c2lkZUhPQztleHBvcnR7SUdOT1JFX0NMQVNTX05BTUV9OyIsImZ1bmN0aW9uIGlzQWJzb2x1dGUocGF0aG5hbWUpIHtcbiAgcmV0dXJuIHBhdGhuYW1lLmNoYXJBdCgwKSA9PT0gJy8nO1xufVxuXG4vLyBBYm91dCAxLjV4IGZhc3RlciB0aGFuIHRoZSB0d28tYXJnIHZlcnNpb24gb2YgQXJyYXkjc3BsaWNlKClcbmZ1bmN0aW9uIHNwbGljZU9uZShsaXN0LCBpbmRleCkge1xuICBmb3IgKHZhciBpID0gaW5kZXgsIGsgPSBpICsgMSwgbiA9IGxpc3QubGVuZ3RoOyBrIDwgbjsgaSArPSAxLCBrICs9IDEpIHtcbiAgICBsaXN0W2ldID0gbGlzdFtrXTtcbiAgfVxuXG4gIGxpc3QucG9wKCk7XG59XG5cbi8vIFRoaXMgaW1wbGVtZW50YXRpb24gaXMgYmFzZWQgaGVhdmlseSBvbiBub2RlJ3MgdXJsLnBhcnNlXG5mdW5jdGlvbiByZXNvbHZlUGF0aG5hbWUodG8pIHtcbiAgdmFyIGZyb20gPSBhcmd1bWVudHMubGVuZ3RoID4gMSAmJiBhcmd1bWVudHNbMV0gIT09IHVuZGVmaW5lZCA/IGFyZ3VtZW50c1sxXSA6ICcnO1xuXG4gIHZhciB0b1BhcnRzID0gdG8gJiYgdG8uc3BsaXQoJy8nKSB8fCBbXTtcbiAgdmFyIGZyb21QYXJ0cyA9IGZyb20gJiYgZnJvbS5zcGxpdCgnLycpIHx8IFtdO1xuXG4gIHZhciBpc1RvQWJzID0gdG8gJiYgaXNBYnNvbHV0ZSh0byk7XG4gIHZhciBpc0Zyb21BYnMgPSBmcm9tICYmIGlzQWJzb2x1dGUoZnJvbSk7XG4gIHZhciBtdXN0RW5kQWJzID0gaXNUb0FicyB8fCBpc0Zyb21BYnM7XG5cbiAgaWYgKHRvICYmIGlzQWJzb2x1dGUodG8pKSB7XG4gICAgLy8gdG8gaXMgYWJzb2x1dGVcbiAgICBmcm9tUGFydHMgPSB0b1BhcnRzO1xuICB9IGVsc2UgaWYgKHRvUGFydHMubGVuZ3RoKSB7XG4gICAgLy8gdG8gaXMgcmVsYXRpdmUsIGRyb3AgdGhlIGZpbGVuYW1lXG4gICAgZnJvbVBhcnRzLnBvcCgpO1xuICAgIGZyb21QYXJ0cyA9IGZyb21QYXJ0cy5jb25jYXQodG9QYXJ0cyk7XG4gIH1cblxuICBpZiAoIWZyb21QYXJ0cy5sZW5ndGgpIHJldHVybiAnLyc7XG5cbiAgdmFyIGhhc1RyYWlsaW5nU2xhc2ggPSB2b2lkIDA7XG4gIGlmIChmcm9tUGFydHMubGVuZ3RoKSB7XG4gICAgdmFyIGxhc3QgPSBmcm9tUGFydHNbZnJvbVBhcnRzLmxlbmd0aCAtIDFdO1xuICAgIGhhc1RyYWlsaW5nU2xhc2ggPSBsYXN0ID09PSAnLicgfHwgbGFzdCA9PT0gJy4uJyB8fCBsYXN0ID09PSAnJztcbiAgfSBlbHNlIHtcbiAgICBoYXNUcmFpbGluZ1NsYXNoID0gZmFsc2U7XG4gIH1cblxuICB2YXIgdXAgPSAwO1xuICBmb3IgKHZhciBpID0gZnJvbVBhcnRzLmxlbmd0aDsgaSA+PSAwOyBpLS0pIHtcbiAgICB2YXIgcGFydCA9IGZyb21QYXJ0c1tpXTtcblxuICAgIGlmIChwYXJ0ID09PSAnLicpIHtcbiAgICAgIHNwbGljZU9uZShmcm9tUGFydHMsIGkpO1xuICAgIH0gZWxzZSBpZiAocGFydCA9PT0gJy4uJykge1xuICAgICAgc3BsaWNlT25lKGZyb21QYXJ0cywgaSk7XG4gICAgICB1cCsrO1xuICAgIH0gZWxzZSBpZiAodXApIHtcbiAgICAgIHNwbGljZU9uZShmcm9tUGFydHMsIGkpO1xuICAgICAgdXAtLTtcbiAgICB9XG4gIH1cblxuICBpZiAoIW11c3RFbmRBYnMpIGZvciAoOyB1cC0tOyB1cCkge1xuICAgIGZyb21QYXJ0cy51bnNoaWZ0KCcuLicpO1xuICB9aWYgKG11c3RFbmRBYnMgJiYgZnJvbVBhcnRzWzBdICE9PSAnJyAmJiAoIWZyb21QYXJ0c1swXSB8fCAhaXNBYnNvbHV0ZShmcm9tUGFydHNbMF0pKSkgZnJvbVBhcnRzLnVuc2hpZnQoJycpO1xuXG4gIHZhciByZXN1bHQgPSBmcm9tUGFydHMuam9pbignLycpO1xuXG4gIGlmIChoYXNUcmFpbGluZ1NsYXNoICYmIHJlc3VsdC5zdWJzdHIoLTEpICE9PSAnLycpIHJlc3VsdCArPSAnLyc7XG5cbiAgcmV0dXJuIHJlc3VsdDtcbn1cblxuZXhwb3J0IGRlZmF1bHQgcmVzb2x2ZVBhdGhuYW1lOyIsIid1c2Ugc3RyaWN0Jztcbm1vZHVsZS5leHBvcnRzID0gZnVuY3Rpb24gKHN0cikge1xuXHRyZXR1cm4gZW5jb2RlVVJJQ29tcG9uZW50KHN0cikucmVwbGFjZSgvWyEnKCkqXS9nLCBmdW5jdGlvbiAoYykge1xuXHRcdHJldHVybiAnJScgKyBjLmNoYXJDb2RlQXQoMCkudG9TdHJpbmcoMTYpLnRvVXBwZXJDYXNlKCk7XG5cdH0pO1xufTtcbiIsIi8qXG5cdE1JVCBMaWNlbnNlIGh0dHA6Ly93d3cub3BlbnNvdXJjZS5vcmcvbGljZW5zZXMvbWl0LWxpY2Vuc2UucGhwXG5cdEF1dGhvciBUb2JpYXMgS29wcGVycyBAc29rcmFcbiovXG5cbnZhciBzdHlsZXNJbkRvbSA9IHt9O1xuXG52YXJcdG1lbW9pemUgPSBmdW5jdGlvbiAoZm4pIHtcblx0dmFyIG1lbW87XG5cblx0cmV0dXJuIGZ1bmN0aW9uICgpIHtcblx0XHRpZiAodHlwZW9mIG1lbW8gPT09IFwidW5kZWZpbmVkXCIpIG1lbW8gPSBmbi5hcHBseSh0aGlzLCBhcmd1bWVudHMpO1xuXHRcdHJldHVybiBtZW1vO1xuXHR9O1xufTtcblxudmFyIGlzT2xkSUUgPSBtZW1vaXplKGZ1bmN0aW9uICgpIHtcblx0Ly8gVGVzdCBmb3IgSUUgPD0gOSBhcyBwcm9wb3NlZCBieSBCcm93c2VyaGFja3Ncblx0Ly8gQHNlZSBodHRwOi8vYnJvd3NlcmhhY2tzLmNvbS8jaGFjay1lNzFkODY5MmY2NTMzNDE3M2ZlZTcxNWMyMjJjYjgwNVxuXHQvLyBUZXN0cyBmb3IgZXhpc3RlbmNlIG9mIHN0YW5kYXJkIGdsb2JhbHMgaXMgdG8gYWxsb3cgc3R5bGUtbG9hZGVyXG5cdC8vIHRvIG9wZXJhdGUgY29ycmVjdGx5IGludG8gbm9uLXN0YW5kYXJkIGVudmlyb25tZW50c1xuXHQvLyBAc2VlIGh0dHBzOi8vZ2l0aHViLmNvbS93ZWJwYWNrLWNvbnRyaWIvc3R5bGUtbG9hZGVyL2lzc3Vlcy8xNzdcblx0cmV0dXJuIHdpbmRvdyAmJiBkb2N1bWVudCAmJiBkb2N1bWVudC5hbGwgJiYgIXdpbmRvdy5hdG9iO1xufSk7XG5cbnZhciBnZXRUYXJnZXQgPSBmdW5jdGlvbiAodGFyZ2V0KSB7XG4gIHJldHVybiBkb2N1bWVudC5xdWVyeVNlbGVjdG9yKHRhcmdldCk7XG59O1xuXG52YXIgZ2V0RWxlbWVudCA9IChmdW5jdGlvbiAoZm4pIHtcblx0dmFyIG1lbW8gPSB7fTtcblxuXHRyZXR1cm4gZnVuY3Rpb24odGFyZ2V0KSB7XG4gICAgICAgICAgICAgICAgLy8gSWYgcGFzc2luZyBmdW5jdGlvbiBpbiBvcHRpb25zLCB0aGVuIHVzZSBpdCBmb3IgcmVzb2x2ZSBcImhlYWRcIiBlbGVtZW50LlxuICAgICAgICAgICAgICAgIC8vIFVzZWZ1bCBmb3IgU2hhZG93IFJvb3Qgc3R5bGUgaS5lXG4gICAgICAgICAgICAgICAgLy8ge1xuICAgICAgICAgICAgICAgIC8vICAgaW5zZXJ0SW50bzogZnVuY3Rpb24gKCkgeyByZXR1cm4gZG9jdW1lbnQucXVlcnlTZWxlY3RvcihcIiNmb29cIikuc2hhZG93Um9vdCB9XG4gICAgICAgICAgICAgICAgLy8gfVxuICAgICAgICAgICAgICAgIGlmICh0eXBlb2YgdGFyZ2V0ID09PSAnZnVuY3Rpb24nKSB7XG4gICAgICAgICAgICAgICAgICAgICAgICByZXR1cm4gdGFyZ2V0KCk7XG4gICAgICAgICAgICAgICAgfVxuICAgICAgICAgICAgICAgIGlmICh0eXBlb2YgbWVtb1t0YXJnZXRdID09PSBcInVuZGVmaW5lZFwiKSB7XG5cdFx0XHR2YXIgc3R5bGVUYXJnZXQgPSBnZXRUYXJnZXQuY2FsbCh0aGlzLCB0YXJnZXQpO1xuXHRcdFx0Ly8gU3BlY2lhbCBjYXNlIHRvIHJldHVybiBoZWFkIG9mIGlmcmFtZSBpbnN0ZWFkIG9mIGlmcmFtZSBpdHNlbGZcblx0XHRcdGlmICh3aW5kb3cuSFRNTElGcmFtZUVsZW1lbnQgJiYgc3R5bGVUYXJnZXQgaW5zdGFuY2VvZiB3aW5kb3cuSFRNTElGcmFtZUVsZW1lbnQpIHtcblx0XHRcdFx0dHJ5IHtcblx0XHRcdFx0XHQvLyBUaGlzIHdpbGwgdGhyb3cgYW4gZXhjZXB0aW9uIGlmIGFjY2VzcyB0byBpZnJhbWUgaXMgYmxvY2tlZFxuXHRcdFx0XHRcdC8vIGR1ZSB0byBjcm9zcy1vcmlnaW4gcmVzdHJpY3Rpb25zXG5cdFx0XHRcdFx0c3R5bGVUYXJnZXQgPSBzdHlsZVRhcmdldC5jb250ZW50RG9jdW1lbnQuaGVhZDtcblx0XHRcdFx0fSBjYXRjaChlKSB7XG5cdFx0XHRcdFx0c3R5bGVUYXJnZXQgPSBudWxsO1xuXHRcdFx0XHR9XG5cdFx0XHR9XG5cdFx0XHRtZW1vW3RhcmdldF0gPSBzdHlsZVRhcmdldDtcblx0XHR9XG5cdFx0cmV0dXJuIG1lbW9bdGFyZ2V0XVxuXHR9O1xufSkoKTtcblxudmFyIHNpbmdsZXRvbiA9IG51bGw7XG52YXJcdHNpbmdsZXRvbkNvdW50ZXIgPSAwO1xudmFyXHRzdHlsZXNJbnNlcnRlZEF0VG9wID0gW107XG5cbnZhclx0Zml4VXJscyA9IHJlcXVpcmUoXCIuL3VybHNcIik7XG5cbm1vZHVsZS5leHBvcnRzID0gZnVuY3Rpb24obGlzdCwgb3B0aW9ucykge1xuXHRpZiAodHlwZW9mIERFQlVHICE9PSBcInVuZGVmaW5lZFwiICYmIERFQlVHKSB7XG5cdFx0aWYgKHR5cGVvZiBkb2N1bWVudCAhPT0gXCJvYmplY3RcIikgdGhyb3cgbmV3IEVycm9yKFwiVGhlIHN0eWxlLWxvYWRlciBjYW5ub3QgYmUgdXNlZCBpbiBhIG5vbi1icm93c2VyIGVudmlyb25tZW50XCIpO1xuXHR9XG5cblx0b3B0aW9ucyA9IG9wdGlvbnMgfHwge307XG5cblx0b3B0aW9ucy5hdHRycyA9IHR5cGVvZiBvcHRpb25zLmF0dHJzID09PSBcIm9iamVjdFwiID8gb3B0aW9ucy5hdHRycyA6IHt9O1xuXG5cdC8vIEZvcmNlIHNpbmdsZS10YWcgc29sdXRpb24gb24gSUU2LTksIHdoaWNoIGhhcyBhIGhhcmQgbGltaXQgb24gdGhlICMgb2YgPHN0eWxlPlxuXHQvLyB0YWdzIGl0IHdpbGwgYWxsb3cgb24gYSBwYWdlXG5cdGlmICghb3B0aW9ucy5zaW5nbGV0b24gJiYgdHlwZW9mIG9wdGlvbnMuc2luZ2xldG9uICE9PSBcImJvb2xlYW5cIikgb3B0aW9ucy5zaW5nbGV0b24gPSBpc09sZElFKCk7XG5cblx0Ly8gQnkgZGVmYXVsdCwgYWRkIDxzdHlsZT4gdGFncyB0byB0aGUgPGhlYWQ+IGVsZW1lbnRcbiAgICAgICAgaWYgKCFvcHRpb25zLmluc2VydEludG8pIG9wdGlvbnMuaW5zZXJ0SW50byA9IFwiaGVhZFwiO1xuXG5cdC8vIEJ5IGRlZmF1bHQsIGFkZCA8c3R5bGU+IHRhZ3MgdG8gdGhlIGJvdHRvbSBvZiB0aGUgdGFyZ2V0XG5cdGlmICghb3B0aW9ucy5pbnNlcnRBdCkgb3B0aW9ucy5pbnNlcnRBdCA9IFwiYm90dG9tXCI7XG5cblx0dmFyIHN0eWxlcyA9IGxpc3RUb1N0eWxlcyhsaXN0LCBvcHRpb25zKTtcblxuXHRhZGRTdHlsZXNUb0RvbShzdHlsZXMsIG9wdGlvbnMpO1xuXG5cdHJldHVybiBmdW5jdGlvbiB1cGRhdGUgKG5ld0xpc3QpIHtcblx0XHR2YXIgbWF5UmVtb3ZlID0gW107XG5cblx0XHRmb3IgKHZhciBpID0gMDsgaSA8IHN0eWxlcy5sZW5ndGg7IGkrKykge1xuXHRcdFx0dmFyIGl0ZW0gPSBzdHlsZXNbaV07XG5cdFx0XHR2YXIgZG9tU3R5bGUgPSBzdHlsZXNJbkRvbVtpdGVtLmlkXTtcblxuXHRcdFx0ZG9tU3R5bGUucmVmcy0tO1xuXHRcdFx0bWF5UmVtb3ZlLnB1c2goZG9tU3R5bGUpO1xuXHRcdH1cblxuXHRcdGlmKG5ld0xpc3QpIHtcblx0XHRcdHZhciBuZXdTdHlsZXMgPSBsaXN0VG9TdHlsZXMobmV3TGlzdCwgb3B0aW9ucyk7XG5cdFx0XHRhZGRTdHlsZXNUb0RvbShuZXdTdHlsZXMsIG9wdGlvbnMpO1xuXHRcdH1cblxuXHRcdGZvciAodmFyIGkgPSAwOyBpIDwgbWF5UmVtb3ZlLmxlbmd0aDsgaSsrKSB7XG5cdFx0XHR2YXIgZG9tU3R5bGUgPSBtYXlSZW1vdmVbaV07XG5cblx0XHRcdGlmKGRvbVN0eWxlLnJlZnMgPT09IDApIHtcblx0XHRcdFx0Zm9yICh2YXIgaiA9IDA7IGogPCBkb21TdHlsZS5wYXJ0cy5sZW5ndGg7IGorKykgZG9tU3R5bGUucGFydHNbal0oKTtcblxuXHRcdFx0XHRkZWxldGUgc3R5bGVzSW5Eb21bZG9tU3R5bGUuaWRdO1xuXHRcdFx0fVxuXHRcdH1cblx0fTtcbn07XG5cbmZ1bmN0aW9uIGFkZFN0eWxlc1RvRG9tIChzdHlsZXMsIG9wdGlvbnMpIHtcblx0Zm9yICh2YXIgaSA9IDA7IGkgPCBzdHlsZXMubGVuZ3RoOyBpKyspIHtcblx0XHR2YXIgaXRlbSA9IHN0eWxlc1tpXTtcblx0XHR2YXIgZG9tU3R5bGUgPSBzdHlsZXNJbkRvbVtpdGVtLmlkXTtcblxuXHRcdGlmKGRvbVN0eWxlKSB7XG5cdFx0XHRkb21TdHlsZS5yZWZzKys7XG5cblx0XHRcdGZvcih2YXIgaiA9IDA7IGogPCBkb21TdHlsZS5wYXJ0cy5sZW5ndGg7IGorKykge1xuXHRcdFx0XHRkb21TdHlsZS5wYXJ0c1tqXShpdGVtLnBhcnRzW2pdKTtcblx0XHRcdH1cblxuXHRcdFx0Zm9yKDsgaiA8IGl0ZW0ucGFydHMubGVuZ3RoOyBqKyspIHtcblx0XHRcdFx0ZG9tU3R5bGUucGFydHMucHVzaChhZGRTdHlsZShpdGVtLnBhcnRzW2pdLCBvcHRpb25zKSk7XG5cdFx0XHR9XG5cdFx0fSBlbHNlIHtcblx0XHRcdHZhciBwYXJ0cyA9IFtdO1xuXG5cdFx0XHRmb3IodmFyIGogPSAwOyBqIDwgaXRlbS5wYXJ0cy5sZW5ndGg7IGorKykge1xuXHRcdFx0XHRwYXJ0cy5wdXNoKGFkZFN0eWxlKGl0ZW0ucGFydHNbal0sIG9wdGlvbnMpKTtcblx0XHRcdH1cblxuXHRcdFx0c3R5bGVzSW5Eb21baXRlbS5pZF0gPSB7aWQ6IGl0ZW0uaWQsIHJlZnM6IDEsIHBhcnRzOiBwYXJ0c307XG5cdFx0fVxuXHR9XG59XG5cbmZ1bmN0aW9uIGxpc3RUb1N0eWxlcyAobGlzdCwgb3B0aW9ucykge1xuXHR2YXIgc3R5bGVzID0gW107XG5cdHZhciBuZXdTdHlsZXMgPSB7fTtcblxuXHRmb3IgKHZhciBpID0gMDsgaSA8IGxpc3QubGVuZ3RoOyBpKyspIHtcblx0XHR2YXIgaXRlbSA9IGxpc3RbaV07XG5cdFx0dmFyIGlkID0gb3B0aW9ucy5iYXNlID8gaXRlbVswXSArIG9wdGlvbnMuYmFzZSA6IGl0ZW1bMF07XG5cdFx0dmFyIGNzcyA9IGl0ZW1bMV07XG5cdFx0dmFyIG1lZGlhID0gaXRlbVsyXTtcblx0XHR2YXIgc291cmNlTWFwID0gaXRlbVszXTtcblx0XHR2YXIgcGFydCA9IHtjc3M6IGNzcywgbWVkaWE6IG1lZGlhLCBzb3VyY2VNYXA6IHNvdXJjZU1hcH07XG5cblx0XHRpZighbmV3U3R5bGVzW2lkXSkgc3R5bGVzLnB1c2gobmV3U3R5bGVzW2lkXSA9IHtpZDogaWQsIHBhcnRzOiBbcGFydF19KTtcblx0XHRlbHNlIG5ld1N0eWxlc1tpZF0ucGFydHMucHVzaChwYXJ0KTtcblx0fVxuXG5cdHJldHVybiBzdHlsZXM7XG59XG5cbmZ1bmN0aW9uIGluc2VydFN0eWxlRWxlbWVudCAob3B0aW9ucywgc3R5bGUpIHtcblx0dmFyIHRhcmdldCA9IGdldEVsZW1lbnQob3B0aW9ucy5pbnNlcnRJbnRvKVxuXG5cdGlmICghdGFyZ2V0KSB7XG5cdFx0dGhyb3cgbmV3IEVycm9yKFwiQ291bGRuJ3QgZmluZCBhIHN0eWxlIHRhcmdldC4gVGhpcyBwcm9iYWJseSBtZWFucyB0aGF0IHRoZSB2YWx1ZSBmb3IgdGhlICdpbnNlcnRJbnRvJyBwYXJhbWV0ZXIgaXMgaW52YWxpZC5cIik7XG5cdH1cblxuXHR2YXIgbGFzdFN0eWxlRWxlbWVudEluc2VydGVkQXRUb3AgPSBzdHlsZXNJbnNlcnRlZEF0VG9wW3N0eWxlc0luc2VydGVkQXRUb3AubGVuZ3RoIC0gMV07XG5cblx0aWYgKG9wdGlvbnMuaW5zZXJ0QXQgPT09IFwidG9wXCIpIHtcblx0XHRpZiAoIWxhc3RTdHlsZUVsZW1lbnRJbnNlcnRlZEF0VG9wKSB7XG5cdFx0XHR0YXJnZXQuaW5zZXJ0QmVmb3JlKHN0eWxlLCB0YXJnZXQuZmlyc3RDaGlsZCk7XG5cdFx0fSBlbHNlIGlmIChsYXN0U3R5bGVFbGVtZW50SW5zZXJ0ZWRBdFRvcC5uZXh0U2libGluZykge1xuXHRcdFx0dGFyZ2V0Lmluc2VydEJlZm9yZShzdHlsZSwgbGFzdFN0eWxlRWxlbWVudEluc2VydGVkQXRUb3AubmV4dFNpYmxpbmcpO1xuXHRcdH0gZWxzZSB7XG5cdFx0XHR0YXJnZXQuYXBwZW5kQ2hpbGQoc3R5bGUpO1xuXHRcdH1cblx0XHRzdHlsZXNJbnNlcnRlZEF0VG9wLnB1c2goc3R5bGUpO1xuXHR9IGVsc2UgaWYgKG9wdGlvbnMuaW5zZXJ0QXQgPT09IFwiYm90dG9tXCIpIHtcblx0XHR0YXJnZXQuYXBwZW5kQ2hpbGQoc3R5bGUpO1xuXHR9IGVsc2UgaWYgKHR5cGVvZiBvcHRpb25zLmluc2VydEF0ID09PSBcIm9iamVjdFwiICYmIG9wdGlvbnMuaW5zZXJ0QXQuYmVmb3JlKSB7XG5cdFx0dmFyIG5leHRTaWJsaW5nID0gZ2V0RWxlbWVudChvcHRpb25zLmluc2VydEludG8gKyBcIiBcIiArIG9wdGlvbnMuaW5zZXJ0QXQuYmVmb3JlKTtcblx0XHR0YXJnZXQuaW5zZXJ0QmVmb3JlKHN0eWxlLCBuZXh0U2libGluZyk7XG5cdH0gZWxzZSB7XG5cdFx0dGhyb3cgbmV3IEVycm9yKFwiW1N0eWxlIExvYWRlcl1cXG5cXG4gSW52YWxpZCB2YWx1ZSBmb3IgcGFyYW1ldGVyICdpbnNlcnRBdCcgKCdvcHRpb25zLmluc2VydEF0JykgZm91bmQuXFxuIE11c3QgYmUgJ3RvcCcsICdib3R0b20nLCBvciBPYmplY3QuXFxuIChodHRwczovL2dpdGh1Yi5jb20vd2VicGFjay1jb250cmliL3N0eWxlLWxvYWRlciNpbnNlcnRhdClcXG5cIik7XG5cdH1cbn1cblxuZnVuY3Rpb24gcmVtb3ZlU3R5bGVFbGVtZW50IChzdHlsZSkge1xuXHRpZiAoc3R5bGUucGFyZW50Tm9kZSA9PT0gbnVsbCkgcmV0dXJuIGZhbHNlO1xuXHRzdHlsZS5wYXJlbnROb2RlLnJlbW92ZUNoaWxkKHN0eWxlKTtcblxuXHR2YXIgaWR4ID0gc3R5bGVzSW5zZXJ0ZWRBdFRvcC5pbmRleE9mKHN0eWxlKTtcblx0aWYoaWR4ID49IDApIHtcblx0XHRzdHlsZXNJbnNlcnRlZEF0VG9wLnNwbGljZShpZHgsIDEpO1xuXHR9XG59XG5cbmZ1bmN0aW9uIGNyZWF0ZVN0eWxlRWxlbWVudCAob3B0aW9ucykge1xuXHR2YXIgc3R5bGUgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KFwic3R5bGVcIik7XG5cblx0aWYob3B0aW9ucy5hdHRycy50eXBlID09PSB1bmRlZmluZWQpIHtcblx0XHRvcHRpb25zLmF0dHJzLnR5cGUgPSBcInRleHQvY3NzXCI7XG5cdH1cblxuXHRhZGRBdHRycyhzdHlsZSwgb3B0aW9ucy5hdHRycyk7XG5cdGluc2VydFN0eWxlRWxlbWVudChvcHRpb25zLCBzdHlsZSk7XG5cblx0cmV0dXJuIHN0eWxlO1xufVxuXG5mdW5jdGlvbiBjcmVhdGVMaW5rRWxlbWVudCAob3B0aW9ucykge1xuXHR2YXIgbGluayA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoXCJsaW5rXCIpO1xuXG5cdGlmKG9wdGlvbnMuYXR0cnMudHlwZSA9PT0gdW5kZWZpbmVkKSB7XG5cdFx0b3B0aW9ucy5hdHRycy50eXBlID0gXCJ0ZXh0L2Nzc1wiO1xuXHR9XG5cdG9wdGlvbnMuYXR0cnMucmVsID0gXCJzdHlsZXNoZWV0XCI7XG5cblx0YWRkQXR0cnMobGluaywgb3B0aW9ucy5hdHRycyk7XG5cdGluc2VydFN0eWxlRWxlbWVudChvcHRpb25zLCBsaW5rKTtcblxuXHRyZXR1cm4gbGluaztcbn1cblxuZnVuY3Rpb24gYWRkQXR0cnMgKGVsLCBhdHRycykge1xuXHRPYmplY3Qua2V5cyhhdHRycykuZm9yRWFjaChmdW5jdGlvbiAoa2V5KSB7XG5cdFx0ZWwuc2V0QXR0cmlidXRlKGtleSwgYXR0cnNba2V5XSk7XG5cdH0pO1xufVxuXG5mdW5jdGlvbiBhZGRTdHlsZSAob2JqLCBvcHRpb25zKSB7XG5cdHZhciBzdHlsZSwgdXBkYXRlLCByZW1vdmUsIHJlc3VsdDtcblxuXHQvLyBJZiBhIHRyYW5zZm9ybSBmdW5jdGlvbiB3YXMgZGVmaW5lZCwgcnVuIGl0IG9uIHRoZSBjc3Ncblx0aWYgKG9wdGlvbnMudHJhbnNmb3JtICYmIG9iai5jc3MpIHtcblx0ICAgIHJlc3VsdCA9IG9wdGlvbnMudHJhbnNmb3JtKG9iai5jc3MpO1xuXG5cdCAgICBpZiAocmVzdWx0KSB7XG5cdCAgICBcdC8vIElmIHRyYW5zZm9ybSByZXR1cm5zIGEgdmFsdWUsIHVzZSB0aGF0IGluc3RlYWQgb2YgdGhlIG9yaWdpbmFsIGNzcy5cblx0ICAgIFx0Ly8gVGhpcyBhbGxvd3MgcnVubmluZyBydW50aW1lIHRyYW5zZm9ybWF0aW9ucyBvbiB0aGUgY3NzLlxuXHQgICAgXHRvYmouY3NzID0gcmVzdWx0O1xuXHQgICAgfSBlbHNlIHtcblx0ICAgIFx0Ly8gSWYgdGhlIHRyYW5zZm9ybSBmdW5jdGlvbiByZXR1cm5zIGEgZmFsc3kgdmFsdWUsIGRvbid0IGFkZCB0aGlzIGNzcy5cblx0ICAgIFx0Ly8gVGhpcyBhbGxvd3MgY29uZGl0aW9uYWwgbG9hZGluZyBvZiBjc3Ncblx0ICAgIFx0cmV0dXJuIGZ1bmN0aW9uKCkge1xuXHQgICAgXHRcdC8vIG5vb3Bcblx0ICAgIFx0fTtcblx0ICAgIH1cblx0fVxuXG5cdGlmIChvcHRpb25zLnNpbmdsZXRvbikge1xuXHRcdHZhciBzdHlsZUluZGV4ID0gc2luZ2xldG9uQ291bnRlcisrO1xuXG5cdFx0c3R5bGUgPSBzaW5nbGV0b24gfHwgKHNpbmdsZXRvbiA9IGNyZWF0ZVN0eWxlRWxlbWVudChvcHRpb25zKSk7XG5cblx0XHR1cGRhdGUgPSBhcHBseVRvU2luZ2xldG9uVGFnLmJpbmQobnVsbCwgc3R5bGUsIHN0eWxlSW5kZXgsIGZhbHNlKTtcblx0XHRyZW1vdmUgPSBhcHBseVRvU2luZ2xldG9uVGFnLmJpbmQobnVsbCwgc3R5bGUsIHN0eWxlSW5kZXgsIHRydWUpO1xuXG5cdH0gZWxzZSBpZiAoXG5cdFx0b2JqLnNvdXJjZU1hcCAmJlxuXHRcdHR5cGVvZiBVUkwgPT09IFwiZnVuY3Rpb25cIiAmJlxuXHRcdHR5cGVvZiBVUkwuY3JlYXRlT2JqZWN0VVJMID09PSBcImZ1bmN0aW9uXCIgJiZcblx0XHR0eXBlb2YgVVJMLnJldm9rZU9iamVjdFVSTCA9PT0gXCJmdW5jdGlvblwiICYmXG5cdFx0dHlwZW9mIEJsb2IgPT09IFwiZnVuY3Rpb25cIiAmJlxuXHRcdHR5cGVvZiBidG9hID09PSBcImZ1bmN0aW9uXCJcblx0KSB7XG5cdFx0c3R5bGUgPSBjcmVhdGVMaW5rRWxlbWVudChvcHRpb25zKTtcblx0XHR1cGRhdGUgPSB1cGRhdGVMaW5rLmJpbmQobnVsbCwgc3R5bGUsIG9wdGlvbnMpO1xuXHRcdHJlbW92ZSA9IGZ1bmN0aW9uICgpIHtcblx0XHRcdHJlbW92ZVN0eWxlRWxlbWVudChzdHlsZSk7XG5cblx0XHRcdGlmKHN0eWxlLmhyZWYpIFVSTC5yZXZva2VPYmplY3RVUkwoc3R5bGUuaHJlZik7XG5cdFx0fTtcblx0fSBlbHNlIHtcblx0XHRzdHlsZSA9IGNyZWF0ZVN0eWxlRWxlbWVudChvcHRpb25zKTtcblx0XHR1cGRhdGUgPSBhcHBseVRvVGFnLmJpbmQobnVsbCwgc3R5bGUpO1xuXHRcdHJlbW92ZSA9IGZ1bmN0aW9uICgpIHtcblx0XHRcdHJlbW92ZVN0eWxlRWxlbWVudChzdHlsZSk7XG5cdFx0fTtcblx0fVxuXG5cdHVwZGF0ZShvYmopO1xuXG5cdHJldHVybiBmdW5jdGlvbiB1cGRhdGVTdHlsZSAobmV3T2JqKSB7XG5cdFx0aWYgKG5ld09iaikge1xuXHRcdFx0aWYgKFxuXHRcdFx0XHRuZXdPYmouY3NzID09PSBvYmouY3NzICYmXG5cdFx0XHRcdG5ld09iai5tZWRpYSA9PT0gb2JqLm1lZGlhICYmXG5cdFx0XHRcdG5ld09iai5zb3VyY2VNYXAgPT09IG9iai5zb3VyY2VNYXBcblx0XHRcdCkge1xuXHRcdFx0XHRyZXR1cm47XG5cdFx0XHR9XG5cblx0XHRcdHVwZGF0ZShvYmogPSBuZXdPYmopO1xuXHRcdH0gZWxzZSB7XG5cdFx0XHRyZW1vdmUoKTtcblx0XHR9XG5cdH07XG59XG5cbnZhciByZXBsYWNlVGV4dCA9IChmdW5jdGlvbiAoKSB7XG5cdHZhciB0ZXh0U3RvcmUgPSBbXTtcblxuXHRyZXR1cm4gZnVuY3Rpb24gKGluZGV4LCByZXBsYWNlbWVudCkge1xuXHRcdHRleHRTdG9yZVtpbmRleF0gPSByZXBsYWNlbWVudDtcblxuXHRcdHJldHVybiB0ZXh0U3RvcmUuZmlsdGVyKEJvb2xlYW4pLmpvaW4oJ1xcbicpO1xuXHR9O1xufSkoKTtcblxuZnVuY3Rpb24gYXBwbHlUb1NpbmdsZXRvblRhZyAoc3R5bGUsIGluZGV4LCByZW1vdmUsIG9iaikge1xuXHR2YXIgY3NzID0gcmVtb3ZlID8gXCJcIiA6IG9iai5jc3M7XG5cblx0aWYgKHN0eWxlLnN0eWxlU2hlZXQpIHtcblx0XHRzdHlsZS5zdHlsZVNoZWV0LmNzc1RleHQgPSByZXBsYWNlVGV4dChpbmRleCwgY3NzKTtcblx0fSBlbHNlIHtcblx0XHR2YXIgY3NzTm9kZSA9IGRvY3VtZW50LmNyZWF0ZVRleHROb2RlKGNzcyk7XG5cdFx0dmFyIGNoaWxkTm9kZXMgPSBzdHlsZS5jaGlsZE5vZGVzO1xuXG5cdFx0aWYgKGNoaWxkTm9kZXNbaW5kZXhdKSBzdHlsZS5yZW1vdmVDaGlsZChjaGlsZE5vZGVzW2luZGV4XSk7XG5cblx0XHRpZiAoY2hpbGROb2Rlcy5sZW5ndGgpIHtcblx0XHRcdHN0eWxlLmluc2VydEJlZm9yZShjc3NOb2RlLCBjaGlsZE5vZGVzW2luZGV4XSk7XG5cdFx0fSBlbHNlIHtcblx0XHRcdHN0eWxlLmFwcGVuZENoaWxkKGNzc05vZGUpO1xuXHRcdH1cblx0fVxufVxuXG5mdW5jdGlvbiBhcHBseVRvVGFnIChzdHlsZSwgb2JqKSB7XG5cdHZhciBjc3MgPSBvYmouY3NzO1xuXHR2YXIgbWVkaWEgPSBvYmoubWVkaWE7XG5cblx0aWYobWVkaWEpIHtcblx0XHRzdHlsZS5zZXRBdHRyaWJ1dGUoXCJtZWRpYVwiLCBtZWRpYSlcblx0fVxuXG5cdGlmKHN0eWxlLnN0eWxlU2hlZXQpIHtcblx0XHRzdHlsZS5zdHlsZVNoZWV0LmNzc1RleHQgPSBjc3M7XG5cdH0gZWxzZSB7XG5cdFx0d2hpbGUoc3R5bGUuZmlyc3RDaGlsZCkge1xuXHRcdFx0c3R5bGUucmVtb3ZlQ2hpbGQoc3R5bGUuZmlyc3RDaGlsZCk7XG5cdFx0fVxuXG5cdFx0c3R5bGUuYXBwZW5kQ2hpbGQoZG9jdW1lbnQuY3JlYXRlVGV4dE5vZGUoY3NzKSk7XG5cdH1cbn1cblxuZnVuY3Rpb24gdXBkYXRlTGluayAobGluaywgb3B0aW9ucywgb2JqKSB7XG5cdHZhciBjc3MgPSBvYmouY3NzO1xuXHR2YXIgc291cmNlTWFwID0gb2JqLnNvdXJjZU1hcDtcblxuXHQvKlxuXHRcdElmIGNvbnZlcnRUb0Fic29sdXRlVXJscyBpc24ndCBkZWZpbmVkLCBidXQgc291cmNlbWFwcyBhcmUgZW5hYmxlZFxuXHRcdGFuZCB0aGVyZSBpcyBubyBwdWJsaWNQYXRoIGRlZmluZWQgdGhlbiBsZXRzIHR1cm4gY29udmVydFRvQWJzb2x1dGVVcmxzXG5cdFx0b24gYnkgZGVmYXVsdC4gIE90aGVyd2lzZSBkZWZhdWx0IHRvIHRoZSBjb252ZXJ0VG9BYnNvbHV0ZVVybHMgb3B0aW9uXG5cdFx0ZGlyZWN0bHlcblx0Ki9cblx0dmFyIGF1dG9GaXhVcmxzID0gb3B0aW9ucy5jb252ZXJ0VG9BYnNvbHV0ZVVybHMgPT09IHVuZGVmaW5lZCAmJiBzb3VyY2VNYXA7XG5cblx0aWYgKG9wdGlvbnMuY29udmVydFRvQWJzb2x1dGVVcmxzIHx8IGF1dG9GaXhVcmxzKSB7XG5cdFx0Y3NzID0gZml4VXJscyhjc3MpO1xuXHR9XG5cblx0aWYgKHNvdXJjZU1hcCkge1xuXHRcdC8vIGh0dHA6Ly9zdGFja292ZXJmbG93LmNvbS9hLzI2NjAzODc1XG5cdFx0Y3NzICs9IFwiXFxuLyojIHNvdXJjZU1hcHBpbmdVUkw9ZGF0YTphcHBsaWNhdGlvbi9qc29uO2Jhc2U2NCxcIiArIGJ0b2EodW5lc2NhcGUoZW5jb2RlVVJJQ29tcG9uZW50KEpTT04uc3RyaW5naWZ5KHNvdXJjZU1hcCkpKSkgKyBcIiAqL1wiO1xuXHR9XG5cblx0dmFyIGJsb2IgPSBuZXcgQmxvYihbY3NzXSwgeyB0eXBlOiBcInRleHQvY3NzXCIgfSk7XG5cblx0dmFyIG9sZFNyYyA9IGxpbmsuaHJlZjtcblxuXHRsaW5rLmhyZWYgPSBVUkwuY3JlYXRlT2JqZWN0VVJMKGJsb2IpO1xuXG5cdGlmKG9sZFNyYykgVVJMLnJldm9rZU9iamVjdFVSTChvbGRTcmMpO1xufVxuIiwiXG4vKipcbiAqIFdoZW4gc291cmNlIG1hcHMgYXJlIGVuYWJsZWQsIGBzdHlsZS1sb2FkZXJgIHVzZXMgYSBsaW5rIGVsZW1lbnQgd2l0aCBhIGRhdGEtdXJpIHRvXG4gKiBlbWJlZCB0aGUgY3NzIG9uIHRoZSBwYWdlLiBUaGlzIGJyZWFrcyBhbGwgcmVsYXRpdmUgdXJscyBiZWNhdXNlIG5vdyB0aGV5IGFyZSByZWxhdGl2ZSB0byBhXG4gKiBidW5kbGUgaW5zdGVhZCBvZiB0aGUgY3VycmVudCBwYWdlLlxuICpcbiAqIE9uZSBzb2x1dGlvbiBpcyB0byBvbmx5IHVzZSBmdWxsIHVybHMsIGJ1dCB0aGF0IG1heSBiZSBpbXBvc3NpYmxlLlxuICpcbiAqIEluc3RlYWQsIHRoaXMgZnVuY3Rpb24gXCJmaXhlc1wiIHRoZSByZWxhdGl2ZSB1cmxzIHRvIGJlIGFic29sdXRlIGFjY29yZGluZyB0byB0aGUgY3VycmVudCBwYWdlIGxvY2F0aW9uLlxuICpcbiAqIEEgcnVkaW1lbnRhcnkgdGVzdCBzdWl0ZSBpcyBsb2NhdGVkIGF0IGB0ZXN0L2ZpeFVybHMuanNgIGFuZCBjYW4gYmUgcnVuIHZpYSB0aGUgYG5wbSB0ZXN0YCBjb21tYW5kLlxuICpcbiAqL1xuXG5tb2R1bGUuZXhwb3J0cyA9IGZ1bmN0aW9uIChjc3MpIHtcbiAgLy8gZ2V0IGN1cnJlbnQgbG9jYXRpb25cbiAgdmFyIGxvY2F0aW9uID0gdHlwZW9mIHdpbmRvdyAhPT0gXCJ1bmRlZmluZWRcIiAmJiB3aW5kb3cubG9jYXRpb247XG5cbiAgaWYgKCFsb2NhdGlvbikge1xuICAgIHRocm93IG5ldyBFcnJvcihcImZpeFVybHMgcmVxdWlyZXMgd2luZG93LmxvY2F0aW9uXCIpO1xuICB9XG5cblx0Ly8gYmxhbmsgb3IgbnVsbD9cblx0aWYgKCFjc3MgfHwgdHlwZW9mIGNzcyAhPT0gXCJzdHJpbmdcIikge1xuXHQgIHJldHVybiBjc3M7XG4gIH1cblxuICB2YXIgYmFzZVVybCA9IGxvY2F0aW9uLnByb3RvY29sICsgXCIvL1wiICsgbG9jYXRpb24uaG9zdDtcbiAgdmFyIGN1cnJlbnREaXIgPSBiYXNlVXJsICsgbG9jYXRpb24ucGF0aG5hbWUucmVwbGFjZSgvXFwvW15cXC9dKiQvLCBcIi9cIik7XG5cblx0Ly8gY29udmVydCBlYWNoIHVybCguLi4pXG5cdC8qXG5cdFRoaXMgcmVndWxhciBleHByZXNzaW9uIGlzIGp1c3QgYSB3YXkgdG8gcmVjdXJzaXZlbHkgbWF0Y2ggYnJhY2tldHMgd2l0aGluXG5cdGEgc3RyaW5nLlxuXG5cdCAvdXJsXFxzKlxcKCAgPSBNYXRjaCBvbiB0aGUgd29yZCBcInVybFwiIHdpdGggYW55IHdoaXRlc3BhY2UgYWZ0ZXIgaXQgYW5kIHRoZW4gYSBwYXJlbnNcblx0ICAgKCAgPSBTdGFydCBhIGNhcHR1cmluZyBncm91cFxuXHQgICAgICg/OiAgPSBTdGFydCBhIG5vbi1jYXB0dXJpbmcgZ3JvdXBcblx0ICAgICAgICAgW14pKF0gID0gTWF0Y2ggYW55dGhpbmcgdGhhdCBpc24ndCBhIHBhcmVudGhlc2VzXG5cdCAgICAgICAgIHwgID0gT1Jcblx0ICAgICAgICAgXFwoICA9IE1hdGNoIGEgc3RhcnQgcGFyZW50aGVzZXNcblx0ICAgICAgICAgICAgICg/OiAgPSBTdGFydCBhbm90aGVyIG5vbi1jYXB0dXJpbmcgZ3JvdXBzXG5cdCAgICAgICAgICAgICAgICAgW14pKF0rICA9IE1hdGNoIGFueXRoaW5nIHRoYXQgaXNuJ3QgYSBwYXJlbnRoZXNlc1xuXHQgICAgICAgICAgICAgICAgIHwgID0gT1Jcblx0ICAgICAgICAgICAgICAgICBcXCggID0gTWF0Y2ggYSBzdGFydCBwYXJlbnRoZXNlc1xuXHQgICAgICAgICAgICAgICAgICAgICBbXikoXSogID0gTWF0Y2ggYW55dGhpbmcgdGhhdCBpc24ndCBhIHBhcmVudGhlc2VzXG5cdCAgICAgICAgICAgICAgICAgXFwpICA9IE1hdGNoIGEgZW5kIHBhcmVudGhlc2VzXG5cdCAgICAgICAgICAgICApICA9IEVuZCBHcm91cFxuICAgICAgICAgICAgICAqXFwpID0gTWF0Y2ggYW55dGhpbmcgYW5kIHRoZW4gYSBjbG9zZSBwYXJlbnNcbiAgICAgICAgICApICA9IENsb3NlIG5vbi1jYXB0dXJpbmcgZ3JvdXBcbiAgICAgICAgICAqICA9IE1hdGNoIGFueXRoaW5nXG4gICAgICAgKSAgPSBDbG9zZSBjYXB0dXJpbmcgZ3JvdXBcblx0IFxcKSAgPSBNYXRjaCBhIGNsb3NlIHBhcmVuc1xuXG5cdCAvZ2kgID0gR2V0IGFsbCBtYXRjaGVzLCBub3QgdGhlIGZpcnN0LiAgQmUgY2FzZSBpbnNlbnNpdGl2ZS5cblx0ICovXG5cdHZhciBmaXhlZENzcyA9IGNzcy5yZXBsYWNlKC91cmxcXHMqXFwoKCg/OlteKShdfFxcKCg/OlteKShdK3xcXChbXikoXSpcXCkpKlxcKSkqKVxcKS9naSwgZnVuY3Rpb24oZnVsbE1hdGNoLCBvcmlnVXJsKSB7XG5cdFx0Ly8gc3RyaXAgcXVvdGVzIChpZiB0aGV5IGV4aXN0KVxuXHRcdHZhciB1bnF1b3RlZE9yaWdVcmwgPSBvcmlnVXJsXG5cdFx0XHQudHJpbSgpXG5cdFx0XHQucmVwbGFjZSgvXlwiKC4qKVwiJC8sIGZ1bmN0aW9uKG8sICQxKXsgcmV0dXJuICQxOyB9KVxuXHRcdFx0LnJlcGxhY2UoL14nKC4qKSckLywgZnVuY3Rpb24obywgJDEpeyByZXR1cm4gJDE7IH0pO1xuXG5cdFx0Ly8gYWxyZWFkeSBhIGZ1bGwgdXJsPyBubyBjaGFuZ2Vcblx0XHRpZiAoL14oI3xkYXRhOnxodHRwOlxcL1xcL3xodHRwczpcXC9cXC98ZmlsZTpcXC9cXC9cXC98XFxzKiQpL2kudGVzdCh1bnF1b3RlZE9yaWdVcmwpKSB7XG5cdFx0ICByZXR1cm4gZnVsbE1hdGNoO1xuXHRcdH1cblxuXHRcdC8vIGNvbnZlcnQgdGhlIHVybCB0byBhIGZ1bGwgdXJsXG5cdFx0dmFyIG5ld1VybDtcblxuXHRcdGlmICh1bnF1b3RlZE9yaWdVcmwuaW5kZXhPZihcIi8vXCIpID09PSAwKSB7XG5cdFx0ICBcdC8vVE9ETzogc2hvdWxkIHdlIGFkZCBwcm90b2NvbD9cblx0XHRcdG5ld1VybCA9IHVucXVvdGVkT3JpZ1VybDtcblx0XHR9IGVsc2UgaWYgKHVucXVvdGVkT3JpZ1VybC5pbmRleE9mKFwiL1wiKSA9PT0gMCkge1xuXHRcdFx0Ly8gcGF0aCBzaG91bGQgYmUgcmVsYXRpdmUgdG8gdGhlIGJhc2UgdXJsXG5cdFx0XHRuZXdVcmwgPSBiYXNlVXJsICsgdW5xdW90ZWRPcmlnVXJsOyAvLyBhbHJlYWR5IHN0YXJ0cyB3aXRoICcvJ1xuXHRcdH0gZWxzZSB7XG5cdFx0XHQvLyBwYXRoIHNob3VsZCBiZSByZWxhdGl2ZSB0byBjdXJyZW50IGRpcmVjdG9yeVxuXHRcdFx0bmV3VXJsID0gY3VycmVudERpciArIHVucXVvdGVkT3JpZ1VybC5yZXBsYWNlKC9eXFwuXFwvLywgXCJcIik7IC8vIFN0cmlwIGxlYWRpbmcgJy4vJ1xuXHRcdH1cblxuXHRcdC8vIHNlbmQgYmFjayB0aGUgZml4ZWQgdXJsKC4uLilcblx0XHRyZXR1cm4gXCJ1cmwoXCIgKyBKU09OLnN0cmluZ2lmeShuZXdVcmwpICsgXCIpXCI7XG5cdH0pO1xuXG5cdC8vIHNlbmQgYmFjayB0aGUgZml4ZWQgY3NzXG5cdHJldHVybiBmaXhlZENzcztcbn07XG4iLCJ2YXIgX3R5cGVvZiA9IHR5cGVvZiBTeW1ib2wgPT09IFwiZnVuY3Rpb25cIiAmJiB0eXBlb2YgU3ltYm9sLml0ZXJhdG9yID09PSBcInN5bWJvbFwiID8gZnVuY3Rpb24gKG9iaikgeyByZXR1cm4gdHlwZW9mIG9iajsgfSA6IGZ1bmN0aW9uIChvYmopIHsgcmV0dXJuIG9iaiAmJiB0eXBlb2YgU3ltYm9sID09PSBcImZ1bmN0aW9uXCIgJiYgb2JqLmNvbnN0cnVjdG9yID09PSBTeW1ib2wgJiYgb2JqICE9PSBTeW1ib2wucHJvdG90eXBlID8gXCJzeW1ib2xcIiA6IHR5cGVvZiBvYmo7IH07XG5cbmZ1bmN0aW9uIHZhbHVlRXF1YWwoYSwgYikge1xuICBpZiAoYSA9PT0gYikgcmV0dXJuIHRydWU7XG5cbiAgaWYgKGEgPT0gbnVsbCB8fCBiID09IG51bGwpIHJldHVybiBmYWxzZTtcblxuICBpZiAoQXJyYXkuaXNBcnJheShhKSkge1xuICAgIHJldHVybiBBcnJheS5pc0FycmF5KGIpICYmIGEubGVuZ3RoID09PSBiLmxlbmd0aCAmJiBhLmV2ZXJ5KGZ1bmN0aW9uIChpdGVtLCBpbmRleCkge1xuICAgICAgcmV0dXJuIHZhbHVlRXF1YWwoaXRlbSwgYltpbmRleF0pO1xuICAgIH0pO1xuICB9XG5cbiAgdmFyIGFUeXBlID0gdHlwZW9mIGEgPT09ICd1bmRlZmluZWQnID8gJ3VuZGVmaW5lZCcgOiBfdHlwZW9mKGEpO1xuICB2YXIgYlR5cGUgPSB0eXBlb2YgYiA9PT0gJ3VuZGVmaW5lZCcgPyAndW5kZWZpbmVkJyA6IF90eXBlb2YoYik7XG5cbiAgaWYgKGFUeXBlICE9PSBiVHlwZSkgcmV0dXJuIGZhbHNlO1xuXG4gIGlmIChhVHlwZSA9PT0gJ29iamVjdCcpIHtcbiAgICB2YXIgYVZhbHVlID0gYS52YWx1ZU9mKCk7XG4gICAgdmFyIGJWYWx1ZSA9IGIudmFsdWVPZigpO1xuXG4gICAgaWYgKGFWYWx1ZSAhPT0gYSB8fCBiVmFsdWUgIT09IGIpIHJldHVybiB2YWx1ZUVxdWFsKGFWYWx1ZSwgYlZhbHVlKTtcblxuICAgIHZhciBhS2V5cyA9IE9iamVjdC5rZXlzKGEpO1xuICAgIHZhciBiS2V5cyA9IE9iamVjdC5rZXlzKGIpO1xuXG4gICAgaWYgKGFLZXlzLmxlbmd0aCAhPT0gYktleXMubGVuZ3RoKSByZXR1cm4gZmFsc2U7XG5cbiAgICByZXR1cm4gYUtleXMuZXZlcnkoZnVuY3Rpb24gKGtleSkge1xuICAgICAgcmV0dXJuIHZhbHVlRXF1YWwoYVtrZXldLCBiW2tleV0pO1xuICAgIH0pO1xuICB9XG5cbiAgcmV0dXJuIGZhbHNlO1xufVxuXG5leHBvcnQgZGVmYXVsdCB2YWx1ZUVxdWFsOyIsIi8qKlxuICogQ29weXJpZ2h0IDIwMTQtMjAxNSwgRmFjZWJvb2ssIEluYy5cbiAqIEFsbCByaWdodHMgcmVzZXJ2ZWQuXG4gKlxuICogVGhpcyBzb3VyY2UgY29kZSBpcyBsaWNlbnNlZCB1bmRlciB0aGUgQlNELXN0eWxlIGxpY2Vuc2UgZm91bmQgaW4gdGhlXG4gKiBMSUNFTlNFIGZpbGUgaW4gdGhlIHJvb3QgZGlyZWN0b3J5IG9mIHRoaXMgc291cmNlIHRyZWUuIEFuIGFkZGl0aW9uYWwgZ3JhbnRcbiAqIG9mIHBhdGVudCByaWdodHMgY2FuIGJlIGZvdW5kIGluIHRoZSBQQVRFTlRTIGZpbGUgaW4gdGhlIHNhbWUgZGlyZWN0b3J5LlxuICovXG5cbid1c2Ugc3RyaWN0JztcblxuLyoqXG4gKiBTaW1pbGFyIHRvIGludmFyaWFudCBidXQgb25seSBsb2dzIGEgd2FybmluZyBpZiB0aGUgY29uZGl0aW9uIGlzIG5vdCBtZXQuXG4gKiBUaGlzIGNhbiBiZSB1c2VkIHRvIGxvZyBpc3N1ZXMgaW4gZGV2ZWxvcG1lbnQgZW52aXJvbm1lbnRzIGluIGNyaXRpY2FsXG4gKiBwYXRocy4gUmVtb3ZpbmcgdGhlIGxvZ2dpbmcgY29kZSBmb3IgcHJvZHVjdGlvbiBlbnZpcm9ubWVudHMgd2lsbCBrZWVwIHRoZVxuICogc2FtZSBsb2dpYyBhbmQgZm9sbG93IHRoZSBzYW1lIGNvZGUgcGF0aHMuXG4gKi9cblxudmFyIHdhcm5pbmcgPSBmdW5jdGlvbigpIHt9O1xuXG5pZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICB3YXJuaW5nID0gZnVuY3Rpb24oY29uZGl0aW9uLCBmb3JtYXQsIGFyZ3MpIHtcbiAgICB2YXIgbGVuID0gYXJndW1lbnRzLmxlbmd0aDtcbiAgICBhcmdzID0gbmV3IEFycmF5KGxlbiA+IDIgPyBsZW4gLSAyIDogMCk7XG4gICAgZm9yICh2YXIga2V5ID0gMjsga2V5IDwgbGVuOyBrZXkrKykge1xuICAgICAgYXJnc1trZXkgLSAyXSA9IGFyZ3VtZW50c1trZXldO1xuICAgIH1cbiAgICBpZiAoZm9ybWF0ID09PSB1bmRlZmluZWQpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcihcbiAgICAgICAgJ2B3YXJuaW5nKGNvbmRpdGlvbiwgZm9ybWF0LCAuLi5hcmdzKWAgcmVxdWlyZXMgYSB3YXJuaW5nICcgK1xuICAgICAgICAnbWVzc2FnZSBhcmd1bWVudCdcbiAgICAgICk7XG4gICAgfVxuXG4gICAgaWYgKGZvcm1hdC5sZW5ndGggPCAxMCB8fCAoL15bc1xcV10qJC8pLnRlc3QoZm9ybWF0KSkge1xuICAgICAgdGhyb3cgbmV3IEVycm9yKFxuICAgICAgICAnVGhlIHdhcm5pbmcgZm9ybWF0IHNob3VsZCBiZSBhYmxlIHRvIHVuaXF1ZWx5IGlkZW50aWZ5IHRoaXMgJyArXG4gICAgICAgICd3YXJuaW5nLiBQbGVhc2UsIHVzZSBhIG1vcmUgZGVzY3JpcHRpdmUgZm9ybWF0IHRoYW46ICcgKyBmb3JtYXRcbiAgICAgICk7XG4gICAgfVxuXG4gICAgaWYgKCFjb25kaXRpb24pIHtcbiAgICAgIHZhciBhcmdJbmRleCA9IDA7XG4gICAgICB2YXIgbWVzc2FnZSA9ICdXYXJuaW5nOiAnICtcbiAgICAgICAgZm9ybWF0LnJlcGxhY2UoLyVzL2csIGZ1bmN0aW9uKCkge1xuICAgICAgICAgIHJldHVybiBhcmdzW2FyZ0luZGV4KytdO1xuICAgICAgICB9KTtcbiAgICAgIGlmICh0eXBlb2YgY29uc29sZSAhPT0gJ3VuZGVmaW5lZCcpIHtcbiAgICAgICAgY29uc29sZS5lcnJvcihtZXNzYWdlKTtcbiAgICAgIH1cbiAgICAgIHRyeSB7XG4gICAgICAgIC8vIFRoaXMgZXJyb3Igd2FzIHRocm93biBhcyBhIGNvbnZlbmllbmNlIHNvIHRoYXQgeW91IGNhbiB1c2UgdGhpcyBzdGFja1xuICAgICAgICAvLyB0byBmaW5kIHRoZSBjYWxsc2l0ZSB0aGF0IGNhdXNlZCB0aGlzIHdhcm5pbmcgdG8gZmlyZS5cbiAgICAgICAgdGhyb3cgbmV3IEVycm9yKG1lc3NhZ2UpO1xuICAgICAgfSBjYXRjaCh4KSB7fVxuICAgIH1cbiAgfTtcbn1cblxubW9kdWxlLmV4cG9ydHMgPSB3YXJuaW5nO1xuIiwiLy8qKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuLy8gIFBlcmlvZGljRGF0YURpc3BsYXkxLnRzIC0gR2J0Y1xyXG4vL1xyXG4vLyAgQ29weXJpZ2h0IMKpIDIwMTgsIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZS4gIEFsbCBSaWdodHMgUmVzZXJ2ZWQuXHJcbi8vXHJcbi8vICBMaWNlbnNlZCB0byB0aGUgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlIChHUEEpIHVuZGVyIG9uZSBvciBtb3JlIGNvbnRyaWJ1dG9yIGxpY2Vuc2UgYWdyZWVtZW50cy4gU2VlXHJcbi8vICB0aGUgTk9USUNFIGZpbGUgZGlzdHJpYnV0ZWQgd2l0aCB0aGlzIHdvcmsgZm9yIGFkZGl0aW9uYWwgaW5mb3JtYXRpb24gcmVnYXJkaW5nIGNvcHlyaWdodCBvd25lcnNoaXAuXHJcbi8vICBUaGUgR1BBIGxpY2Vuc2VzIHRoaXMgZmlsZSB0byB5b3UgdW5kZXIgdGhlIE1JVCBMaWNlbnNlIChNSVQpLCB0aGUgXCJMaWNlbnNlXCI7IHlvdSBtYXkgbm90IHVzZSB0aGlzXHJcbi8vICBmaWxlIGV4Y2VwdCBpbiBjb21wbGlhbmNlIHdpdGggdGhlIExpY2Vuc2UuIFlvdSBtYXkgb2J0YWluIGEgY29weSBvZiB0aGUgTGljZW5zZSBhdDpcclxuLy9cclxuLy8gICAgICBodHRwOi8vb3BlbnNvdXJjZS5vcmcvbGljZW5zZXMvTUlUXHJcbi8vXHJcbi8vICBVbmxlc3MgYWdyZWVkIHRvIGluIHdyaXRpbmcsIHRoZSBzdWJqZWN0IHNvZnR3YXJlIGRpc3RyaWJ1dGVkIHVuZGVyIHRoZSBMaWNlbnNlIGlzIGRpc3RyaWJ1dGVkIG9uIGFuXHJcbi8vICBcIkFTLUlTXCIgQkFTSVMsIFdJVEhPVVQgV0FSUkFOVElFUyBPUiBDT05ESVRJT05TIE9GIEFOWSBLSU5ELCBlaXRoZXIgZXhwcmVzcyBvciBpbXBsaWVkLiBSZWZlciB0byB0aGVcclxuLy8gIExpY2Vuc2UgZm9yIHRoZSBzcGVjaWZpYyBsYW5ndWFnZSBnb3Zlcm5pbmcgcGVybWlzc2lvbnMgYW5kIGxpbWl0YXRpb25zLlxyXG4vL1xyXG4vLyAgQ29kZSBNb2RpZmljYXRpb24gSGlzdG9yeTpcclxuLy8gIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuLy8gIDA1LzI1LzIwMTggLSBCaWxseSBFcm5lc3RcclxuLy8gICAgICAgR2VuZXJhdGVkIG9yaWdpbmFsIHZlcnNpb24gb2Ygc291cmNlIGNvZGUuXHJcbi8vXHJcbi8vKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbmltcG9ydCAqIGFzIG1vbWVudCBmcm9tICdtb21lbnQnO1xyXG5pbXBvcnQgeyBQZXJpb2RpY0RhdGFEaXNwbGF5IH0gZnJvbSAnLi8uLi8uLi9UU1gvZ2xvYmFsJ1xyXG5cclxuZXhwb3J0IGRlZmF1bHQgY2xhc3MgUGVyaW9kaWNEYXRhRGlzcGxheVNlcnZpY2Uge1xyXG4gICAgZ2V0RGF0YShtZXRlcklEOiBudW1iZXIsIHN0YXJ0RGF0ZTogc3RyaW5nLCBlbmREYXRlOnN0cmluZywgcGl4ZWxzOiBudW1iZXIsIG1lYXN1cmVtZW50Q2hhcmFjdGVyaXN0aWNJRDpudW1iZXIsIG1lYXN1cmVtZW50VHlwZUlEOm51bWJlcixoYXJtb25pY0dyb3VwOiBudW1iZXIsIHR5cGUpIHtcclxuICAgICAgICByZXR1cm4gJC5hamF4KHtcclxuICAgICAgICAgICAgdHlwZTogXCJHRVRcIixcclxuICAgICAgICAgICAgdXJsOiBgJHt3aW5kb3cubG9jYXRpb24ub3JpZ2lufS9hcGkvUGVyaW9kaWNEYXRhRGlzcGxheS9HZXREYXRhP01ldGVySUQ9JHttZXRlcklEfWAgK1xyXG4gICAgICAgICAgICAgICAgYCZzdGFydERhdGU9JHttb21lbnQoc3RhcnREYXRlKS5mb3JtYXQoJ1lZWVktTU0tREQnKX1gICtcclxuICAgICAgICAgICAgICAgIGAmZW5kRGF0ZT0ke21vbWVudChlbmREYXRlKS5mb3JtYXQoJ1lZWVktTU0tREQnKX1gICtcclxuICAgICAgICAgICAgICAgIGAmcGl4ZWxzPSR7cGl4ZWxzfWAgK1xyXG4gICAgICAgICAgICAgICAgYCZNZWFzdXJlbWVudENoYXJhY3RlcmlzdGljSUQ9JHttZWFzdXJlbWVudENoYXJhY3RlcmlzdGljSUR9YCArIFxyXG4gICAgICAgICAgICAgICAgYCZNZWFzdXJlbWVudFR5cGVJRD0ke21lYXN1cmVtZW50VHlwZUlEfWAgK1xyXG4gICAgICAgICAgICAgICAgYCZIYXJtb25pY0dyb3VwPSR7aGFybW9uaWNHcm91cH1gICtcclxuICAgICAgICAgICAgICAgIGAmdHlwZT0ke3R5cGV9YCxcclxuICAgICAgICAgICAgY29udGVudFR5cGU6IFwiYXBwbGljYXRpb24vanNvbjsgY2hhcnNldD11dGYtOFwiLFxyXG4gICAgICAgICAgICBkYXRhVHlwZTogJ2pzb24nLFxyXG4gICAgICAgICAgICBjYWNoZTogdHJ1ZSxcclxuICAgICAgICAgICAgYXN5bmM6IHRydWVcclxuICAgICAgICB9KSBhcyBKUXVlcnkuanFYSFI8UGVyaW9kaWNEYXRhRGlzcGxheS5SZXR1cm5EYXRhPjtcclxuICAgIH1cclxuXHJcbiAgICBnZXRNZXRlcnMoKSB7XHJcbiAgICAgICAgcmV0dXJuICQuYWpheCh7XHJcbiAgICAgICAgICAgIHR5cGU6IFwiR0VUXCIsXHJcbiAgICAgICAgICAgIHVybDogYCR7d2luZG93LmxvY2F0aW9uLm9yaWdpbn0vYXBpL1BlcmlvZGljRGF0YURpc3BsYXkvR2V0TWV0ZXJzYCxcclxuICAgICAgICAgICAgY29udGVudFR5cGU6IFwiYXBwbGljYXRpb24vanNvbjsgY2hhcnNldD11dGYtOFwiLFxyXG4gICAgICAgICAgICBkYXRhVHlwZTogJ2pzb24nLFxyXG4gICAgICAgICAgICBjYWNoZTogdHJ1ZSxcclxuICAgICAgICAgICAgYXN5bmM6IHRydWVcclxuICAgICAgICB9KTtcclxuICAgIH1cclxuXHJcbiAgICBnZXRNZWFzdXJlbWVudENoYXJhY3RlcmlzdGljcyhtZXRlcklEKSB7XHJcbiAgICAgICAgcmV0dXJuICQuYWpheCh7XHJcbiAgICAgICAgICAgIHR5cGU6IFwiR0VUXCIsXHJcbiAgICAgICAgICAgIHVybDogYCR7d2luZG93LmxvY2F0aW9uLm9yaWdpbn0vYXBpL1BlcmlvZGljRGF0YURpc3BsYXkvR2V0TWVhc3VyZW1lbnRDaGFyYWN0ZXJpc3RpY3NgLFxyXG4gICAgICAgICAgICBjb250ZW50VHlwZTogXCJhcHBsaWNhdGlvbi9qc29uOyBjaGFyc2V0PXV0Zi04XCIsXHJcbiAgICAgICAgICAgIGRhdGFUeXBlOiAnanNvbicsXHJcbiAgICAgICAgICAgIGNhY2hlOiB0cnVlLFxyXG4gICAgICAgICAgICBhc3luYzogdHJ1ZVxyXG4gICAgICAgIH0pIGFzIEpRdWVyeS5qcVhIUjxQZXJpb2RpY0RhdGFEaXNwbGF5Lk1lYXN1cmVtZW50Q2hhcmF0ZXJpc3RpY3NbXT47XHJcbiAgICB9XHJcblxyXG5cclxufSIsIi8vKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbi8vICBQZXJpb2RpY0RhdGFEaXNwbGF5MS50cyAtIEdidGNcclxuLy9cclxuLy8gIENvcHlyaWdodCDCqSAyMDE4LCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxyXG4vL1xyXG4vLyAgTGljZW5zZWQgdG8gdGhlIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZSAoR1BBKSB1bmRlciBvbmUgb3IgbW9yZSBjb250cmlidXRvciBsaWNlbnNlIGFncmVlbWVudHMuIFNlZVxyXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxyXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xyXG4vLyAgZmlsZSBleGNlcHQgaW4gY29tcGxpYW5jZSB3aXRoIHRoZSBMaWNlbnNlLiBZb3UgbWF5IG9idGFpbiBhIGNvcHkgb2YgdGhlIExpY2Vuc2UgYXQ6XHJcbi8vXHJcbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxyXG4vL1xyXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxyXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXHJcbi8vICBMaWNlbnNlIGZvciB0aGUgc3BlY2lmaWMgbGFuZ3VhZ2UgZ292ZXJuaW5nIHBlcm1pc3Npb25zIGFuZCBsaW1pdGF0aW9ucy5cclxuLy9cclxuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XHJcbi8vICAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vICAwNS8yNS8yMDE4IC0gQmlsbHkgRXJuZXN0XHJcbi8vICAgICAgIEdlbmVyYXRlZCBvcmlnaW5hbCB2ZXJzaW9uIG9mIHNvdXJjZSBjb2RlLlxyXG4vL1xyXG4vLyoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5pbXBvcnQgKiBhcyBtb21lbnQgZnJvbSAnbW9tZW50JztcclxuaW1wb3J0IHsgUGVyaW9kaWNEYXRhRGlzcGxheSwgVHJlbmRpbmdjRGF0YURpc3BsYXkgfSBmcm9tICcuLy4uLy4uL1RTWC9nbG9iYWwnXHJcblxyXG5leHBvcnQgZGVmYXVsdCBjbGFzcyBUcmVuZGluZ0RhdGFEaXNwbGF5U2VydmljZSB7XHJcbiAgICBnZXREYXRhKGNoYW5uZWxJRDogbnVtYmVyLCBzdGFydERhdGU6IHN0cmluZywgZW5kRGF0ZTogc3RyaW5nLCBwaXhlbHM6IG51bWJlcikge1xyXG4gICAgICAgIHJldHVybiAkLmFqYXgoe1xyXG4gICAgICAgICAgICB0eXBlOiBcIkdFVFwiLFxyXG4gICAgICAgICAgICB1cmw6IGAke3dpbmRvdy5sb2NhdGlvbi5vcmlnaW59L2FwaS9UcmVuZGluZ0RhdGFEaXNwbGF5L0dldERhdGE/Q2hhbm5lbElEPSR7Y2hhbm5lbElEfWAgK1xyXG4gICAgICAgICAgICAgICAgYCZzdGFydERhdGU9JHttb21lbnQoc3RhcnREYXRlKS5mb3JtYXQoJ1lZWVktTU0tRERUSEg6bW0nKX1gICtcclxuICAgICAgICAgICAgICAgIGAmZW5kRGF0ZT0ke21vbWVudChlbmREYXRlKS5mb3JtYXQoJ1lZWVktTU0tRERUSEg6bW0nKX1gICtcclxuICAgICAgICAgICAgICAgIGAmcGl4ZWxzPSR7cGl4ZWxzfWAsXHJcbiAgICAgICAgICAgIGNvbnRlbnRUeXBlOiBcImFwcGxpY2F0aW9uL2pzb247IGNoYXJzZXQ9dXRmLThcIixcclxuICAgICAgICAgICAgZGF0YVR5cGU6ICdqc29uJyxcclxuICAgICAgICAgICAgY2FjaGU6IHRydWUsXHJcbiAgICAgICAgICAgIGFzeW5jOiB0cnVlXHJcbiAgICAgICAgfSkgYXMgSlF1ZXJ5LmpxWEhSPFBlcmlvZGljRGF0YURpc3BsYXkuUmV0dXJuRGF0YT47XHJcbiAgICB9XHJcbiAgICBnZXRQb3N0RGF0YShtZWFzdXJlbWVudHM6IFRyZW5kaW5nY0RhdGFEaXNwbGF5Lk1lYXN1cmVtZW50W10sIHN0YXJ0RGF0ZTogc3RyaW5nLCBlbmREYXRlOiBzdHJpbmcsIHBpeGVsczogbnVtYmVyKSB7XHJcbiAgICAgICAgcmV0dXJuICQuYWpheCh7XHJcbiAgICAgICAgICAgIHR5cGU6IFwiUE9TVFwiLFxyXG4gICAgICAgICAgICB1cmw6IGAke3dpbmRvdy5sb2NhdGlvbi5vcmlnaW59L2FwaS9UcmVuZGluZ0RhdGFEaXNwbGF5L0dldERhdGFgLFxyXG4gICAgICAgICAgICBjb250ZW50VHlwZTogXCJhcHBsaWNhdGlvbi9qc29uOyBjaGFyc2V0PXV0Zi04XCIsXHJcbiAgICAgICAgICAgIGRhdGFUeXBlOiAnanNvbicsXHJcbiAgICAgICAgICAgIGRhdGE6IEpTT04uc3RyaW5naWZ5KHtDaGFubmVsczogbWVhc3VyZW1lbnRzLm1hcChtcyA9PiBtcy5JRCksIFN0YXJ0RGF0ZTogc3RhcnREYXRlLCBFbmREYXRlOiBlbmREYXRlLCBQaXhlbHM6IHBpeGVscyB9KSxcclxuICAgICAgICAgICAgY2FjaGU6IHRydWUsXHJcbiAgICAgICAgICAgIGFzeW5jOiB0cnVlXHJcbiAgICAgICAgfSkgYXMgSlF1ZXJ5LmpxWEhSPFRyZW5kaW5nY0RhdGFEaXNwbGF5LlJldHVybkRhdGE+O1xyXG4gICAgfVxyXG5cclxuXHJcblxyXG4gICAgZ2V0TWVhc3VyZW1lbnRzKG1ldGVySUQ6IG51bWJlcikge1xyXG4gICAgICAgIHJldHVybiAkLmFqYXgoe1xyXG4gICAgICAgICAgICB0eXBlOiBcIkdFVFwiLFxyXG4gICAgICAgICAgICB1cmw6IGAke3dpbmRvdy5sb2NhdGlvbi5vcmlnaW59L2FwaS9UcmVuZGluZ0RhdGFEaXNwbGF5L0dldE1lYXN1cmVtZW50cz9NZXRlcklEPSR7bWV0ZXJJRH1gLFxyXG4gICAgICAgICAgICBjb250ZW50VHlwZTogXCJhcHBsaWNhdGlvbi9qc29uOyBjaGFyc2V0PXV0Zi04XCIsXHJcbiAgICAgICAgICAgIGRhdGFUeXBlOiAnanNvbicsXHJcbiAgICAgICAgICAgIGNhY2hlOiB0cnVlLFxyXG4gICAgICAgICAgICBhc3luYzogdHJ1ZVxyXG4gICAgICAgIH0pO1xyXG4gICAgfVxyXG5cclxuXHJcbn0iLCIvLyoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG4vLyAgRGF0ZVRpbWVSYW5nZVBpY2tlci50c3ggLSBHYnRjXHJcbi8vXHJcbi8vICBDb3B5cmlnaHQgwqkgMjAxOCwgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlLiAgQWxsIFJpZ2h0cyBSZXNlcnZlZC5cclxuLy9cclxuLy8gIExpY2Vuc2VkIHRvIHRoZSBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UgKEdQQSkgdW5kZXIgb25lIG9yIG1vcmUgY29udHJpYnV0b3IgbGljZW5zZSBhZ3JlZW1lbnRzLiBTZWVcclxuLy8gIHRoZSBOT1RJQ0UgZmlsZSBkaXN0cmlidXRlZCB3aXRoIHRoaXMgd29yayBmb3IgYWRkaXRpb25hbCBpbmZvcm1hdGlvbiByZWdhcmRpbmcgY29weXJpZ2h0IG93bmVyc2hpcC5cclxuLy8gIFRoZSBHUEEgbGljZW5zZXMgdGhpcyBmaWxlIHRvIHlvdSB1bmRlciB0aGUgTUlUIExpY2Vuc2UgKE1JVCksIHRoZSBcIkxpY2Vuc2VcIjsgeW91IG1heSBub3QgdXNlIHRoaXNcclxuLy8gIGZpbGUgZXhjZXB0IGluIGNvbXBsaWFuY2Ugd2l0aCB0aGUgTGljZW5zZS4gWW91IG1heSBvYnRhaW4gYSBjb3B5IG9mIHRoZSBMaWNlbnNlIGF0OlxyXG4vL1xyXG4vLyAgICAgIGh0dHA6Ly9vcGVuc291cmNlLm9yZy9saWNlbnNlcy9NSVRcclxuLy9cclxuLy8gIFVubGVzcyBhZ3JlZWQgdG8gaW4gd3JpdGluZywgdGhlIHN1YmplY3Qgc29mdHdhcmUgZGlzdHJpYnV0ZWQgdW5kZXIgdGhlIExpY2Vuc2UgaXMgZGlzdHJpYnV0ZWQgb24gYW5cclxuLy8gIFwiQVMtSVNcIiBCQVNJUywgV0lUSE9VVCBXQVJSQU5USUVTIE9SIENPTkRJVElPTlMgT0YgQU5ZIEtJTkQsIGVpdGhlciBleHByZXNzIG9yIGltcGxpZWQuIFJlZmVyIHRvIHRoZVxyXG4vLyAgTGljZW5zZSBmb3IgdGhlIHNwZWNpZmljIGxhbmd1YWdlIGdvdmVybmluZyBwZXJtaXNzaW9ucyBhbmQgbGltaXRhdGlvbnMuXHJcbi8vXHJcbi8vICBDb2RlIE1vZGlmaWNhdGlvbiBIaXN0b3J5OlxyXG4vLyAgLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG4vLyAgMDcvMjMvMjAxOCAtIEJpbGx5IEVybmVzdFxyXG4vLyAgICAgICBHZW5lcmF0ZWQgb3JpZ2luYWwgdmVyc2lvbiBvZiBzb3VyY2UgY29kZS5cclxuLy9cclxuLy8qKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuXHJcbmltcG9ydCAqIGFzIFJlYWN0IGZyb20gJ3JlYWN0JztcclxuaW1wb3J0ICogYXMgUmVhY3RET00gZnJvbSAncmVhY3QtZG9tJztcclxuaW1wb3J0ICogYXMgXyBmcm9tIFwibG9kYXNoXCI7XHJcbmltcG9ydCAqIGFzIERhdGVUaW1lIGZyb20gXCJyZWFjdC1kYXRldGltZVwiO1xyXG5pbXBvcnQgKiBhcyBtb21lbnQgZnJvbSAnbW9tZW50JztcclxuaW1wb3J0ICdyZWFjdC1kYXRldGltZS9jc3MvcmVhY3QtZGF0ZXRpbWUuY3NzJztcclxuXHJcbmV4cG9ydCBkZWZhdWx0IGNsYXNzIERhdGVUaW1lUmFuZ2VQaWNrZXIgZXh0ZW5kcyBSZWFjdC5Db21wb25lbnQ8YW55LCBhbnk+e1xyXG4gICAgcHJvcHM6IHsgc3RhcnREYXRlOiBzdHJpbmcsIGVuZERhdGU6IHN0cmluZywgc3RhdGVTZXR0ZXI6IEZ1bmN0aW9uIH1cclxuICAgIHN0YXRlOiB7IHN0YXJ0RGF0ZTogbW9tZW50Lk1vbWVudCwgZW5kRGF0ZTogbW9tZW50Lk1vbWVudCB9XHJcbiAgICBzdGF0ZVNldHRlcklkOiBhbnk7XHJcbiAgICBjb25zdHJ1Y3Rvcihwcm9wcykge1xyXG4gICAgICAgIHN1cGVyKHByb3BzKTtcclxuICAgICAgICB0aGlzLnN0YXRlID0ge1xyXG4gICAgICAgICAgICBzdGFydERhdGU6IG1vbWVudCh0aGlzLnByb3BzLnN0YXJ0RGF0ZSksXHJcbiAgICAgICAgICAgIGVuZERhdGU6IG1vbWVudCh0aGlzLnByb3BzLmVuZERhdGUpXHJcbiAgICAgICAgfTtcclxuICAgIH1cclxuICAgIHJlbmRlcigpIHtcclxuICAgICAgICByZXR1cm4gKFxyXG4gICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImNvbnRhaW5lclwiIHN0eWxlPXt7d2lkdGg6ICdpbmhlcml0J319PlxyXG4gICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJyb3dcIj5cclxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImZvcm0tZ3JvdXBcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPERhdGVUaW1lXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBpc1ZhbGlkRGF0ZT17KGRhdGUpID0+IHsgcmV0dXJuIGRhdGUuaXNCZWZvcmUodGhpcy5zdGF0ZS5lbmREYXRlKSB9fVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgdmFsdWU9e3RoaXMuc3RhdGUuc3RhcnREYXRlfVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgdGltZUZvcm1hdD1cIkhIOm1tXCJcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIG9uQ2hhbmdlPXsodmFsdWUpID0+IHRoaXMuc2V0U3RhdGUoeyBzdGFydERhdGU6IHZhbHVlIH0sICgpID0+IHRoaXMuc3RhdGVTZXR0ZXIoKSl9IC8+XHJcbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwicm93XCI+XHJcbiAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJmb3JtLWdyb3VwXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxEYXRlVGltZVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgaXNWYWxpZERhdGU9eyhkYXRlKSA9PiB7IHJldHVybiBkYXRlLmlzQWZ0ZXIodGhpcy5zdGF0ZS5zdGFydERhdGUpIH19XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB2YWx1ZT17dGhpcy5zdGF0ZS5lbmREYXRlfVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgdGltZUZvcm1hdD1cIkhIOm1tXCJcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIG9uQ2hhbmdlPXsodmFsdWUpID0+IHRoaXMuc2V0U3RhdGUoeyBlbmREYXRlOiB2YWx1ZSB9LCAoKSA9PiB0aGlzLnN0YXRlU2V0dGVyKCkpfSAvPlxyXG4gICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICk7XHJcbiAgICB9XHJcblxyXG4gICAgY29tcG9uZW50V2lsbFJlY2VpdmVQcm9wcyhuZXh0UHJvcHMsIG5leHRDb250ZW50KSB7XHJcbiAgICAgICAgaWYgKG5leHRQcm9wcy5zdGFydERhdGUgIT0gdGhpcy5zdGF0ZS5zdGFydERhdGUuZm9ybWF0KCdZWVlZLU1NLUREVEhIOm1tJykgfHwgbmV4dFByb3BzLmVuZERhdGUgIT0gdGhpcy5zdGF0ZS5lbmREYXRlLmZvcm1hdCgnWVlZWS1NTS1ERFRISDptbScpKVxyXG4gICAgICAgICAgICB0aGlzLnNldFN0YXRlKHsgc3RhcnREYXRlOiBtb21lbnQodGhpcy5wcm9wcy5zdGFydERhdGUpLCBlbmREYXRlOiBtb21lbnQodGhpcy5wcm9wcy5lbmREYXRlKX0pO1xyXG4gICAgfVxyXG5cclxuICAgIHN0YXRlU2V0dGVyKCkge1xyXG4gICAgICAgIGNsZWFyVGltZW91dCh0aGlzLnN0YXRlU2V0dGVySWQpO1xyXG4gICAgICAgIHRoaXMuc3RhdGVTZXR0ZXJJZCA9IHNldFRpbWVvdXQoKCkgPT4ge1xyXG4gICAgICAgICAgICB0aGlzLnByb3BzLnN0YXRlU2V0dGVyKHsgc3RhcnREYXRlOiB0aGlzLnN0YXRlLnN0YXJ0RGF0ZS5mb3JtYXQoJ1lZWVktTU0tRERUSEg6bW0nKSwgZW5kRGF0ZTogdGhpcy5zdGF0ZS5lbmREYXRlLmZvcm1hdCgnWVlZWS1NTS1ERFRISDptbScpIH0pO1xyXG4gICAgICAgIH0sIDUwMCk7XHJcbiAgICB9XHJcbn1cclxuXHJcblxyXG4iLCIvLyoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG4vLyAgTWVhc3VyZW1lbnRJbnB1dC50c3ggLSBHYnRjXHJcbi8vXHJcbi8vICBDb3B5cmlnaHQgwqkgMjAxOCwgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlLiAgQWxsIFJpZ2h0cyBSZXNlcnZlZC5cclxuLy9cclxuLy8gIExpY2Vuc2VkIHRvIHRoZSBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UgKEdQQSkgdW5kZXIgb25lIG9yIG1vcmUgY29udHJpYnV0b3IgbGljZW5zZSBhZ3JlZW1lbnRzLiBTZWVcclxuLy8gIHRoZSBOT1RJQ0UgZmlsZSBkaXN0cmlidXRlZCB3aXRoIHRoaXMgd29yayBmb3IgYWRkaXRpb25hbCBpbmZvcm1hdGlvbiByZWdhcmRpbmcgY29weXJpZ2h0IG93bmVyc2hpcC5cclxuLy8gIFRoZSBHUEEgbGljZW5zZXMgdGhpcyBmaWxlIHRvIHlvdSB1bmRlciB0aGUgTUlUIExpY2Vuc2UgKE1JVCksIHRoZSBcIkxpY2Vuc2VcIjsgeW91IG1heSBub3QgdXNlIHRoaXNcclxuLy8gIGZpbGUgZXhjZXB0IGluIGNvbXBsaWFuY2Ugd2l0aCB0aGUgTGljZW5zZS4gWW91IG1heSBvYnRhaW4gYSBjb3B5IG9mIHRoZSBMaWNlbnNlIGF0OlxyXG4vL1xyXG4vLyAgICAgIGh0dHA6Ly9vcGVuc291cmNlLm9yZy9saWNlbnNlcy9NSVRcclxuLy9cclxuLy8gIFVubGVzcyBhZ3JlZWQgdG8gaW4gd3JpdGluZywgdGhlIHN1YmplY3Qgc29mdHdhcmUgZGlzdHJpYnV0ZWQgdW5kZXIgdGhlIExpY2Vuc2UgaXMgZGlzdHJpYnV0ZWQgb24gYW5cclxuLy8gIFwiQVMtSVNcIiBCQVNJUywgV0lUSE9VVCBXQVJSQU5USUVTIE9SIENPTkRJVElPTlMgT0YgQU5ZIEtJTkQsIGVpdGhlciBleHByZXNzIG9yIGltcGxpZWQuIFJlZmVyIHRvIHRoZVxyXG4vLyAgTGljZW5zZSBmb3IgdGhlIHNwZWNpZmljIGxhbmd1YWdlIGdvdmVybmluZyBwZXJtaXNzaW9ucyBhbmQgbGltaXRhdGlvbnMuXHJcbi8vXHJcbi8vICBDb2RlIE1vZGlmaWNhdGlvbiBIaXN0b3J5OlxyXG4vLyAgLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG4vLyAgMDcvMTkvMjAxOCAtIEJpbGx5IEVybmVzdFxyXG4vLyAgICAgICBHZW5lcmF0ZWQgb3JpZ2luYWwgdmVyc2lvbiBvZiBzb3VyY2UgY29kZS5cclxuLy9cclxuLy8qKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuXHJcbmltcG9ydCAqIGFzIFJlYWN0IGZyb20gJ3JlYWN0JztcclxuaW1wb3J0IFRyZW5kaW5nRGF0YURpc3BsYXlTZXJ2aWNlIGZyb20gJy4vLi4vVFMvU2VydmljZXMvVHJlbmRpbmdEYXRhRGlzcGxheSc7XHJcbmltcG9ydCAqIGFzIF8gZnJvbSBcImxvZGFzaFwiO1xyXG5cclxuZXhwb3J0IGRlZmF1bHQgY2xhc3MgTWVhc3VyZW1lbnRJbnB1dCBleHRlbmRzIFJlYWN0LkNvbXBvbmVudDx7IHZhbHVlOiBudW1iZXIsIG1ldGVySUQ6IG51bWJlciwgb25DaGFuZ2U6IEZ1bmN0aW9uIH0sIHsgb3B0aW9uczogYW55W10gfT57XHJcbiAgICB0cmVuZGluZ0RhdGFEaXNwbGF5U2VydmljZTogVHJlbmRpbmdEYXRhRGlzcGxheVNlcnZpY2U7XHJcbiAgICBjb25zdHJ1Y3Rvcihwcm9wcykge1xyXG4gICAgICAgIHN1cGVyKHByb3BzKTtcclxuICAgICAgICB0aGlzLnN0YXRlID0ge1xyXG4gICAgICAgICAgICBvcHRpb25zOiBbXVxyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgdGhpcy50cmVuZGluZ0RhdGFEaXNwbGF5U2VydmljZSA9IG5ldyBUcmVuZGluZ0RhdGFEaXNwbGF5U2VydmljZSgpO1xyXG4gICAgfVxyXG5cclxuICAgIGNvbXBvbmVudFdpbGxSZWNlaXZlUHJvcHMobmV4dFByb3BzKSB7XHJcbiAgICAgICAgaWYodGhpcy5wcm9wcy5tZXRlcklEICE9IG5leHRQcm9wcy5tZXRlcklEKVxyXG4gICAgICAgICAgICB0aGlzLmdldERhdGEobmV4dFByb3BzLm1ldGVySUQpO1xyXG4gICAgfVxyXG5cclxuICAgIGNvbXBvbmVudERpZE1vdW50KCkge1xyXG4gICAgICAgIHRoaXMuZ2V0RGF0YSh0aGlzLnByb3BzLm1ldGVySUQpO1xyXG4gICAgfVxyXG5cclxuICAgIGdldERhdGEobWV0ZXJJRCkge1xyXG4gICAgICAgIHRoaXMudHJlbmRpbmdEYXRhRGlzcGxheVNlcnZpY2UuZ2V0TWVhc3VyZW1lbnRzKG1ldGVySUQpLmRvbmUoZGF0YSA9PiB7XHJcbiAgICAgICAgICAgIGlmIChkYXRhLmxlbmd0aCA9PSAwKSByZXR1cm47XHJcblxyXG4gICAgICAgICAgICB2YXIgdmFsdWUgPSAodGhpcy5wcm9wcy52YWx1ZSA/IHRoaXMucHJvcHMudmFsdWUgOiBkYXRhWzBdLklEKVxyXG4gICAgICAgICAgICB2YXIgb3B0aW9ucyA9IGRhdGEubWFwKGQgPT4gPG9wdGlvbiBrZXk9e2QuSUR9IHZhbHVlPXtkLklEfT57ZC5OYW1lfTwvb3B0aW9uPik7XHJcbiAgICAgICAgICAgIHRoaXMuc2V0U3RhdGUoeyBvcHRpb25zIH0pO1xyXG4gICAgICAgIH0pO1xyXG5cclxuICAgIH1cclxuXHJcbiAgICByZW5kZXIoKSB7XHJcbiAgICAgICAgcmV0dXJuICg8c2VsZWN0IGNsYXNzTmFtZT0nZm9ybS1jb250cm9sJyBvbkNoYW5nZT17KGUpID0+IHsgdGhpcy5wcm9wcy5vbkNoYW5nZSh7IG1lYXN1cmVtZW50SUQ6IHBhcnNlSW50KGUudGFyZ2V0LnZhbHVlKSwgbWVhc3VyZW1lbnROYW1lOiBlLnRhcmdldC5zZWxlY3RlZE9wdGlvbnNbMF0udGV4dCB9KTsgfX0gdmFsdWU9e3RoaXMucHJvcHMudmFsdWV9PlxyXG4gICAgICAgICAgICA8b3B0aW9uIHZhbHVlPScwJz48L29wdGlvbj5cclxuICAgICAgICAgICAge3RoaXMuc3RhdGUub3B0aW9uc31cclxuICAgICAgICA8L3NlbGVjdD4pO1xyXG4gICAgfVxyXG5cclxufSIsIi8vKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbi8vICBNZXRlcklucHV0LnRzeCAtIEdidGNcclxuLy9cclxuLy8gIENvcHlyaWdodCDCqSAyMDE4LCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxyXG4vL1xyXG4vLyAgTGljZW5zZWQgdG8gdGhlIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZSAoR1BBKSB1bmRlciBvbmUgb3IgbW9yZSBjb250cmlidXRvciBsaWNlbnNlIGFncmVlbWVudHMuIFNlZVxyXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxyXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xyXG4vLyAgZmlsZSBleGNlcHQgaW4gY29tcGxpYW5jZSB3aXRoIHRoZSBMaWNlbnNlLiBZb3UgbWF5IG9idGFpbiBhIGNvcHkgb2YgdGhlIExpY2Vuc2UgYXQ6XHJcbi8vXHJcbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxyXG4vL1xyXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxyXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXHJcbi8vICBMaWNlbnNlIGZvciB0aGUgc3BlY2lmaWMgbGFuZ3VhZ2UgZ292ZXJuaW5nIHBlcm1pc3Npb25zIGFuZCBsaW1pdGF0aW9ucy5cclxuLy9cclxuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XHJcbi8vICAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vICAwNS8yNS8yMDE4IC0gQmlsbHkgRXJuZXN0XHJcbi8vICAgICAgIEdlbmVyYXRlZCBvcmlnaW5hbCB2ZXJzaW9uIG9mIHNvdXJjZSBjb2RlLlxyXG4vL1xyXG4vLyoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5cclxuaW1wb3J0ICogYXMgUmVhY3QgZnJvbSAncmVhY3QnO1xyXG5pbXBvcnQgUGVyaW9kaWNEYXRhRGlzcGxheVNlcnZpY2UgZnJvbSAnLi8uLi9UUy9TZXJ2aWNlcy9QZXJpb2RpY0RhdGFEaXNwbGF5JztcclxuaW1wb3J0ICogYXMgXyBmcm9tIFwibG9kYXNoXCI7XHJcblxyXG50eXBlIG1ldGVycyA9ICBBcnJheTx7IElEOiBudW1iZXIsIE5hbWU6IHN0cmluZyB9PjtcclxuXHJcbmV4cG9ydCBkZWZhdWx0IGNsYXNzIE1ldGVySW5wdXQgZXh0ZW5kcyBSZWFjdC5Db21wb25lbnQ8eyB2YWx1ZTogbnVtYmVyLCBvbkNoYW5nZTogRnVuY3Rpb24gfSwgeyBvcHRpb25zOiBKU1guRWxlbWVudFtdIH0+e1xyXG4gICAgcGVyaW9kaWNEYXRhRGlzcGxheVNlcnZpY2U6IFBlcmlvZGljRGF0YURpc3BsYXlTZXJ2aWNlO1xyXG4gICAgY29uc3RydWN0b3IocHJvcHMpIHtcclxuICAgICAgICBzdXBlcihwcm9wcyk7XHJcblxyXG4gICAgICAgIHRoaXMuc3RhdGUgPSB7XHJcbiAgICAgICAgICAgIG9wdGlvbnM6IFtdXHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICB0aGlzLnBlcmlvZGljRGF0YURpc3BsYXlTZXJ2aWNlID0gbmV3IFBlcmlvZGljRGF0YURpc3BsYXlTZXJ2aWNlKCk7XHJcbiAgICB9XHJcblxyXG4gICAgY29tcG9uZW50RGlkTW91bnQoKSB7XHJcblxyXG4gICAgICAgIHRoaXMucGVyaW9kaWNEYXRhRGlzcGxheVNlcnZpY2UuZ2V0TWV0ZXJzKCkuZG9uZShkYXRhID0+IHtcclxuICAgICAgICAgICAgdGhpcy5zZXRTdGF0ZSh7IG9wdGlvbnM6IGRhdGEubWFwKGQgPT4gPG9wdGlvbiBrZXk9e2QuSUR9IHZhbHVlPXtkLklEfT57ZC5OYW1lfTwvb3B0aW9uPikgfSk7XHJcbiAgICAgICAgfSk7XHJcbiAgICB9XHJcblxyXG4gICAgcmVuZGVyKCkge1xyXG4gICAgICAgIHJldHVybiAoXHJcbiAgICAgICAgICAgIDxzZWxlY3QgY2xhc3NOYW1lPSdmb3JtLWNvbnRyb2wnIG9uQ2hhbmdlPXsoZSkgPT4geyB0aGlzLnByb3BzLm9uQ2hhbmdlKHsgbWV0ZXJJRDogcGFyc2VJbnQoZS50YXJnZXQudmFsdWUpLCBtZXRlck5hbWU6IGUudGFyZ2V0LnNlbGVjdGVkT3B0aW9uc1swXS50ZXh0LCBtZWFzdXJlbWVudElEOiBudWxsIH0pOyB9fSB2YWx1ZT17dGhpcy5wcm9wcy52YWx1ZX0+XHJcbiAgICAgICAgICAgICAgICA8b3B0aW9uIHZhbHVlPScwJz48L29wdGlvbj5cclxuICAgICAgICAgICAgICAgIHt0aGlzLnN0YXRlLm9wdGlvbnN9XHJcbiAgICAgICAgICAgIDwvc2VsZWN0Pik7XHJcbiAgICB9XHJcbn0iLCIvLyoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG4vLyAgVHJlbmRpbmdDaGFydC50c3ggLSBHYnRjXHJcbi8vXHJcbi8vICBDb3B5cmlnaHQgwqkgMjAxOCwgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlLiAgQWxsIFJpZ2h0cyBSZXNlcnZlZC5cclxuLy9cclxuLy8gIExpY2Vuc2VkIHRvIHRoZSBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UgKEdQQSkgdW5kZXIgb25lIG9yIG1vcmUgY29udHJpYnV0b3IgbGljZW5zZSBhZ3JlZW1lbnRzLiBTZWVcclxuLy8gIHRoZSBOT1RJQ0UgZmlsZSBkaXN0cmlidXRlZCB3aXRoIHRoaXMgd29yayBmb3IgYWRkaXRpb25hbCBpbmZvcm1hdGlvbiByZWdhcmRpbmcgY29weXJpZ2h0IG93bmVyc2hpcC5cclxuLy8gIFRoZSBHUEEgbGljZW5zZXMgdGhpcyBmaWxlIHRvIHlvdSB1bmRlciB0aGUgTUlUIExpY2Vuc2UgKE1JVCksIHRoZSBcIkxpY2Vuc2VcIjsgeW91IG1heSBub3QgdXNlIHRoaXNcclxuLy8gIGZpbGUgZXhjZXB0IGluIGNvbXBsaWFuY2Ugd2l0aCB0aGUgTGljZW5zZS4gWW91IG1heSBvYnRhaW4gYSBjb3B5IG9mIHRoZSBMaWNlbnNlIGF0OlxyXG4vL1xyXG4vLyAgICAgIGh0dHA6Ly9vcGVuc291cmNlLm9yZy9saWNlbnNlcy9NSVRcclxuLy9cclxuLy8gIFVubGVzcyBhZ3JlZWQgdG8gaW4gd3JpdGluZywgdGhlIHN1YmplY3Qgc29mdHdhcmUgZGlzdHJpYnV0ZWQgdW5kZXIgdGhlIExpY2Vuc2UgaXMgZGlzdHJpYnV0ZWQgb24gYW5cclxuLy8gIFwiQVMtSVNcIiBCQVNJUywgV0lUSE9VVCBXQVJSQU5USUVTIE9SIENPTkRJVElPTlMgT0YgQU5ZIEtJTkQsIGVpdGhlciBleHByZXNzIG9yIGltcGxpZWQuIFJlZmVyIHRvIHRoZVxyXG4vLyAgTGljZW5zZSBmb3IgdGhlIHNwZWNpZmljIGxhbmd1YWdlIGdvdmVybmluZyBwZXJtaXNzaW9ucyBhbmQgbGltaXRhdGlvbnMuXHJcbi8vXHJcbi8vICBDb2RlIE1vZGlmaWNhdGlvbiBIaXN0b3J5OlxyXG4vLyAgLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG4vLyAgMDcvMTkvMjAxOCAtIEJpbGx5IEVybmVzdFxyXG4vLyAgICAgICBHZW5lcmF0ZWQgb3JpZ2luYWwgdmVyc2lvbiBvZiBzb3VyY2UgY29kZS5cclxuLy9cclxuLy8qKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuXHJcbmltcG9ydCAqIGFzIFJlYWN0IGZyb20gJ3JlYWN0JztcclxuIGltcG9ydCAqIGFzIG1vbWVudCBmcm9tICdtb21lbnQnO1xyXG5pbXBvcnQgKiBhcyBfIGZyb20gXCJsb2Rhc2hcIjtcclxuaW1wb3J0ICcuLy4uL2Zsb3QvanF1ZXJ5LmZsb3QubWluLmpzJztcclxuaW1wb3J0ICcuLy4uL2Zsb3QvanF1ZXJ5LmZsb3QuY3Jvc3NoYWlyLm1pbi5qcyc7XHJcbmltcG9ydCAnLi8uLi9mbG90L2pxdWVyeS5mbG90Lm5hdmlnYXRlLm1pbi5qcyc7XHJcbmltcG9ydCAnLi8uLi9mbG90L2pxdWVyeS5mbG90LnNlbGVjdGlvbi5taW4uanMnO1xyXG5pbXBvcnQgJy4vLi4vZmxvdC9qcXVlcnkuZmxvdC50aW1lLm1pbi5qcyc7XHJcbmltcG9ydCAnLi8uLi9mbG90L2pxdWVyeS5mbG90LmF4aXNsYWJlbHMuanMnO1xyXG5cclxuaW1wb3J0IHsgVHJlbmRpbmdjRGF0YURpc3BsYXkgfSBmcm9tICcuL2dsb2JhbCdcclxuXHJcbmludGVyZmFjZSBQcm9wcyB7IHN0YXJ0RGF0ZTogc3RyaW5nLCBlbmREYXRlOiBzdHJpbmcsIGRhdGE6IFRyZW5kaW5nY0RhdGFEaXNwbGF5Lk1lYXN1cmVtZW50W10sIGF4ZXM6IFRyZW5kaW5nY0RhdGFEaXNwbGF5LkZsb3RBeGlzW10sc3RhdGVTZXR0ZXI6IEZ1bmN0aW9uIH1cclxuZXhwb3J0IGRlZmF1bHQgY2xhc3MgVHJlbmRpbmdDaGFydCBleHRlbmRzIFJlYWN0LkNvbXBvbmVudDxQcm9wcywge30+e1xyXG4gICAgcGxvdDogYW55O1xyXG4gICAgem9vbUlkOiBhbnk7XHJcbiAgICBob3ZlcjogbnVtYmVyO1xyXG4gICAgc3RhcnREYXRlOiBzdHJpbmc7XHJcbiAgICBlbmREYXRlOiBzdHJpbmc7XHJcbiAgICBvcHRpb25zOiB7IGNhbnZhczogYm9vbGVhbiwgbGVnZW5kOiBvYmplY3QsIGNyb3NzaGFpcjogb2JqZWN0LCBzZWxlY3Rpb246IG9iamVjdCwgZ3JpZDogb2JqZWN0LCB4YXhpczoge21vZGU6IHN0cmluZywgdGlja0xlbmd0aDogbnVtYmVyLCByZXNlcnZlU3BhY2U6IGJvb2xlYW4sIHRpY2tzOiBGdW5jdGlvbiwgdGlja0Zvcm1hdHRlcjogRnVuY3Rpb24sIG1heDogbnVtYmVyLCBtaW46IG51bWJlcn0sIHlheGlzOiBvYmplY3QsIHlheGVzOiBvYmplY3RbXSB9XHJcblxyXG4gICAgY29uc3RydWN0b3IocHJvcHMpIHtcclxuICAgICAgICBzdXBlcihwcm9wcyk7XHJcblxyXG4gICAgICAgIHRoaXMuaG92ZXIgPSAwO1xyXG4gICAgICAgIHRoaXMuc3RhcnREYXRlID0gcHJvcHMuc3RhcnREYXRlO1xyXG4gICAgICAgIHRoaXMuZW5kRGF0ZSA9IHByb3BzLmVuZERhdGU7XHJcblxyXG4gICAgICAgIHZhciBjdHJsID0gdGhpcztcclxuXHJcbiAgICAgICAgdGhpcy5vcHRpb25zID0ge1xyXG4gICAgICAgICAgICBjYW52YXM6IHRydWUsXHJcbiAgICAgICAgICAgIGxlZ2VuZDogeyBzaG93OiB0cnVlIH0sXHJcbiAgICAgICAgICAgIGNyb3NzaGFpcjogeyBtb2RlOiBcInhcIiB9LFxyXG4gICAgICAgICAgICBzZWxlY3Rpb246IHsgbW9kZTogXCJ4XCIgfSxcclxuICAgICAgICAgICAgZ3JpZDoge1xyXG4gICAgICAgICAgICAgICAgYXV0b0hpZ2hsaWdodDogZmFsc2UsXHJcbiAgICAgICAgICAgICAgICBjbGlja2FibGU6IHRydWUsXHJcbiAgICAgICAgICAgICAgICBob3ZlcmFibGU6IHRydWUsXHJcbiAgICAgICAgICAgICAgICBtYXJraW5nczogW11cclxuICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgeGF4aXM6IHtcclxuICAgICAgICAgICAgICAgIG1vZGU6IFwidGltZVwiLFxyXG4gICAgICAgICAgICAgICAgdGlja0xlbmd0aDogMTAsXHJcbiAgICAgICAgICAgICAgICByZXNlcnZlU3BhY2U6IGZhbHNlLFxyXG4gICAgICAgICAgICAgICAgdGlja3M6IGZ1bmN0aW9uIChheGlzKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgdmFyIHRpY2tzID0gW10sXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHN0YXJ0ID0gY3RybC5mbG9vckluQmFzZShheGlzLm1pbiwgYXhpcy5kZWx0YSksXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGkgPSAwLFxyXG4gICAgICAgICAgICAgICAgICAgICAgICB2ID0gTnVtYmVyLk5hTixcclxuICAgICAgICAgICAgICAgICAgICAgICAgcHJldjtcclxuXHJcbiAgICAgICAgICAgICAgICAgICAgZG8ge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBwcmV2ID0gdjtcclxuICAgICAgICAgICAgICAgICAgICAgICAgdiA9IHN0YXJ0ICsgaSAqIGF4aXMuZGVsdGE7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHRpY2tzLnB1c2godik7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICsraTtcclxuICAgICAgICAgICAgICAgICAgICB9IHdoaWxlICh2IDwgYXhpcy5tYXggJiYgdiAhPSBwcmV2KTtcclxuICAgICAgICAgICAgICAgICAgICByZXR1cm4gdGlja3M7XHJcbiAgICAgICAgICAgICAgICB9LFxyXG4gICAgICAgICAgICAgICAgdGlja0Zvcm1hdHRlcjogZnVuY3Rpb24gKHZhbHVlLCBheGlzKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgaWYgKGF4aXMuZGVsdGEgPiAzICogMjQgKiA2MCAqIDYwICogMTAwMClcclxuICAgICAgICAgICAgICAgICAgICAgICAgcmV0dXJuIG1vbWVudCh2YWx1ZSkudXRjKCkuZm9ybWF0KFwiTU0vRERcIik7XHJcbiAgICAgICAgICAgICAgICAgICAgZWxzZVxyXG4gICAgICAgICAgICAgICAgICAgICAgICByZXR1cm4gbW9tZW50KHZhbHVlKS51dGMoKS5mb3JtYXQoXCJNTS9ERCBISDptbVwiKTtcclxuICAgICAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgICAgICBtYXg6IG51bGwsXHJcbiAgICAgICAgICAgICAgICBtaW46IG51bGxcclxuICAgICAgICAgICAgfSxcclxuICAgICAgICAgICAgeWF4aXM6IHtcclxuICAgICAgICAgICAgICAgIGxhYmVsV2lkdGg6IDUwLFxyXG4gICAgICAgICAgICAgICAgcGFuUmFuZ2U6IGZhbHNlLFxyXG4gICAgICAgICAgICAgICAgLy90aWNrczogMSxcclxuICAgICAgICAgICAgICAgIHRpY2tMZW5ndGg6IDEwLFxyXG4gICAgICAgICAgICAgICAgdGlja0Zvcm1hdHRlcjogZnVuY3Rpb24gKHZhbCwgYXhpcykge1xyXG4gICAgICAgICAgICAgICAgICAgIGlmIChheGlzLmRlbHRhID4gMTAwMDAwMCAmJiAodmFsID4gMTAwMDAwMCB8fCB2YWwgPCAtMTAwMDAwMCkpXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHJldHVybiAoKHZhbCAvIDEwMDAwMDApIHwgMCkgKyBcIk1cIjtcclxuICAgICAgICAgICAgICAgICAgICBlbHNlIGlmIChheGlzLmRlbHRhID4gMTAwMCAmJiAodmFsID4gMTAwMCB8fCB2YWwgPCAtMTAwMCkpXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHJldHVybiAoKHZhbCAvIDEwMDApIHwgMCkgKyBcIktcIjtcclxuICAgICAgICAgICAgICAgICAgICBlbHNlXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHJldHVybiB2YWwudG9GaXhlZChheGlzLnRpY2tEZWNpbWFscyk7XHJcbiAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIH0sXHJcbiAgICAgICAgICAgIHlheGVzOiBbXVxyXG4gICAgICAgIH1cclxuXHJcbiAgICB9XHJcblxyXG5cclxuICAgIGNyZWF0ZURhdGFSb3dzKHByb3BzOiBQcm9wcykge1xyXG4gICAgICAgIC8vIGlmIHN0YXJ0IGFuZCBlbmQgZGF0ZSBhcmUgbm90IHByb3ZpZGVkIGNhbGN1bGF0ZSB0aGVtIGZyb20gdGhlIGRhdGEgc2V0XHJcbiAgICAgICAgaWYgKHRoaXMucGxvdCAhPSB1bmRlZmluZWQpICQodGhpcy5yZWZzLmdyYXBoKS5jaGlsZHJlbigpLnJlbW92ZSgpO1xyXG5cclxuICAgICAgICB2YXIgc3RhcnRTdHJpbmcgPSB0aGlzLnN0YXJ0RGF0ZTtcclxuICAgICAgICB2YXIgZW5kU3RyaW5nID0gdGhpcy5lbmREYXRlO1xyXG4gICAgICAgIHZhciBuZXdWZXNzZWwgPSBbXTtcclxuXHJcbiAgICAgICAgaWYgKHByb3BzLmRhdGEgIT0gbnVsbCkge1xyXG4gICAgICAgICAgICAkLmVhY2gocHJvcHMuZGF0YSwgKGksIG1lYXN1cmVtZW50KSA9PiB7XHJcbiAgICAgICAgICAgICAgICBpZihtZWFzdXJlbWVudC5NYXhpbXVtICYmIG1lYXN1cmVtZW50LkRhdGE/Lmxlbmd0aCA+IDApXHJcbiAgICAgICAgICAgICAgICAgICAgbmV3VmVzc2VsLnB1c2goeyBsYWJlbDogYCR7bWVhc3VyZW1lbnQuTWVhc3VyZW1lbnROYW1lfS1NYXhgLCBkYXRhOiBtZWFzdXJlbWVudC5EYXRhLm1hcChkID0+IFtkLlRpbWUsZC5NYXhpbXVtXSksIGNvbG9yOiBtZWFzdXJlbWVudC5NYXhDb2xvciwgeWF4aXM6IG1lYXN1cmVtZW50LkF4aXMgfSlcclxuICAgICAgICAgICAgICAgIGlmIChtZWFzdXJlbWVudC5BdmVyYWdlICYmIG1lYXN1cmVtZW50LkRhdGE/Lmxlbmd0aCA+IDApXHJcbiAgICAgICAgICAgICAgICAgICAgbmV3VmVzc2VsLnB1c2goeyBsYWJlbDogYCR7bWVhc3VyZW1lbnQuTWVhc3VyZW1lbnROYW1lfS1BdmdgLCBkYXRhOiBtZWFzdXJlbWVudC5EYXRhLm1hcChkID0+IFtkLlRpbWUsIGQuQXZlcmFnZV0pLCBjb2xvcjogbWVhc3VyZW1lbnQuQXZnQ29sb3IsIHlheGlzOiBtZWFzdXJlbWVudC5BeGlzICB9KVxyXG4gICAgICAgICAgICAgICAgaWYgKG1lYXN1cmVtZW50Lk1pbmltdW0gJiYgbWVhc3VyZW1lbnQuRGF0YT8ubGVuZ3RoID4gMClcclxuICAgICAgICAgICAgICAgICAgICBuZXdWZXNzZWwucHVzaCh7IGxhYmVsOiBgJHttZWFzdXJlbWVudC5NZWFzdXJlbWVudE5hbWV9LU1pbmAsIGRhdGE6IG1lYXN1cmVtZW50LkRhdGEubWFwKGQgPT4gW2QuVGltZSwgZC5NaW5pbXVtXSksIGNvbG9yOiBtZWFzdXJlbWVudC5NaW5Db2xvciwgeWF4aXM6IG1lYXN1cmVtZW50LkF4aXMgIH0pXHJcblxyXG4gICAgICAgICAgICB9KTtcclxuICAgICAgICB9XHJcbiAgICAgICAgbmV3VmVzc2VsLnB1c2goW1t0aGlzLmdldE1pbGxpc2Vjb25kVGltZShzdGFydFN0cmluZyksIG51bGxdLCBbdGhpcy5nZXRNaWxsaXNlY29uZFRpbWUoZW5kU3RyaW5nKSwgbnVsbF1dKTtcclxuICAgICAgICB0aGlzLm9wdGlvbnMueGF4aXMubWF4ID0gdGhpcy5nZXRNaWxsaXNlY29uZFRpbWUoZW5kU3RyaW5nKTtcclxuICAgICAgICB0aGlzLm9wdGlvbnMueGF4aXMubWluID0gdGhpcy5nZXRNaWxsaXNlY29uZFRpbWUoc3RhcnRTdHJpbmcpO1xyXG4gICAgICAgIHRoaXMub3B0aW9ucy55YXhlcyA9IHRoaXMucHJvcHMuYXhlcztcclxuICAgICAgICB0aGlzLnBsb3QgPSAoJCBhcyBhbnkpLnBsb3QoJCh0aGlzLnJlZnMuZ3JhcGgpLCBuZXdWZXNzZWwsIHRoaXMub3B0aW9ucyk7XHJcbiAgICAgICAgdGhpcy5wbG90U2VsZWN0ZWQoKTtcclxuICAgICAgICB0aGlzLnBsb3Rab29tKCk7XHJcbiAgICAgICAgdGhpcy5wbG90SG92ZXIoKTtcclxuICAgICAgICAvL3RoaXMucGxvdENsaWNrKCk7XHJcbiAgICB9XHJcblxyXG4gICAgY29tcG9uZW50RGlkTW91bnQoKSB7XHJcbiAgICAgICAgdGhpcy5jcmVhdGVEYXRhUm93cyh0aGlzLnByb3BzKTtcclxuICAgIH1cclxuXHJcbiAgICBjb21wb25lbnRXaWxsUmVjZWl2ZVByb3BzKG5leHRQcm9wcykge1xyXG4gICAgICAgIHRoaXMuc3RhcnREYXRlID0gbmV4dFByb3BzLnN0YXJ0RGF0ZTtcclxuICAgICAgICB0aGlzLmVuZERhdGUgPSBuZXh0UHJvcHMuZW5kRGF0ZTtcclxuICAgICAgICB0aGlzLmNyZWF0ZURhdGFSb3dzKG5leHRQcm9wcyk7XHJcbiAgICB9XHJcblxyXG4gICAgcmVuZGVyKCkge1xyXG4gICAgICAgIHJldHVybiA8ZGl2IHJlZj17J2dyYXBoJ30gc3R5bGU9e3sgaGVpZ2h0OiAnaW5oZXJpdCcsIHdpZHRoOiAnaW5oZXJpdCd9fT48L2Rpdj47XHJcbiAgICB9XHJcblxyXG4gICAgLy8gcm91bmQgdG8gbmVhcmJ5IGxvd2VyIG11bHRpcGxlIG9mIGJhc2VcclxuICAgIGZsb29ySW5CYXNlKG4sIGJhc2UpIHtcclxuICAgICAgICByZXR1cm4gYmFzZSAqIE1hdGguZmxvb3IobiAvIGJhc2UpO1xyXG4gICAgfVxyXG5cclxuICAgIGdldE1pbGxpc2Vjb25kVGltZShkYXRlKSB7XHJcbiAgICAgICAgdmFyIG1pbGxpc2Vjb25kcyA9IG1vbWVudC51dGMoZGF0ZSkudmFsdWVPZigpO1xyXG4gICAgICAgIHJldHVybiBtaWxsaXNlY29uZHM7XHJcbiAgICB9XHJcblxyXG4gICAgZ2V0RGF0ZVN0cmluZyhmbG9hdCkge1xyXG4gICAgICAgIHZhciBkYXRlID0gbW9tZW50LnV0YyhmbG9hdCkuZm9ybWF0KCdZWVlZLU1NLUREVEhIOm1tJyk7XHJcbiAgICAgICAgcmV0dXJuIGRhdGU7XHJcbiAgICB9XHJcblxyXG4gICAgcGxvdFNlbGVjdGVkKCkge1xyXG4gICAgICAgIHZhciBjdHJsID0gdGhpcztcclxuICAgICAgICAkKHRoaXMucmVmcy5ncmFwaCkub2ZmKFwicGxvdHNlbGVjdGVkXCIpO1xyXG4gICAgICAgICQodGhpcy5yZWZzLmdyYXBoKS5iaW5kKFwicGxvdHNlbGVjdGVkXCIsIGZ1bmN0aW9uIChldmVudCwgcmFuZ2VzKSB7XHJcbiAgICAgICAgICAgIGN0cmwucHJvcHMuc3RhdGVTZXR0ZXIoeyBzdGFydERhdGU6IGN0cmwuZ2V0RGF0ZVN0cmluZyhyYW5nZXMueGF4aXMuZnJvbSksIGVuZERhdGU6IGN0cmwuZ2V0RGF0ZVN0cmluZyhyYW5nZXMueGF4aXMudG8pIH0pO1xyXG4gICAgICAgIH0pO1xyXG4gICAgfVxyXG5cclxuICAgIHBsb3Rab29tKCkge1xyXG4gICAgICAgIHZhciBjdHJsID0gdGhpcztcclxuICAgICAgICAkKHRoaXMucmVmcy5ncmFwaCkub2ZmKFwicGxvdHpvb21cIik7XHJcbiAgICAgICAgJCh0aGlzLnJlZnMuZ3JhcGgpLmJpbmQoXCJwbG90em9vbVwiLCBmdW5jdGlvbiAoZXZlbnQpIHtcclxuICAgICAgICAgICAgdmFyIG1pbkRlbHRhID0gbnVsbDtcclxuICAgICAgICAgICAgdmFyIG1heERlbHRhID0gNTtcclxuICAgICAgICAgICAgdmFyIHhheGlzID0gY3RybC5wbG90LmdldEF4ZXMoKS54YXhpcztcclxuICAgICAgICAgICAgdmFyIHhjZW50ZXIgPSBjdHJsLmhvdmVyO1xyXG4gICAgICAgICAgICB2YXIgeG1pbiA9IHhheGlzLm9wdGlvbnMubWluO1xyXG4gICAgICAgICAgICB2YXIgeG1heCA9IHhheGlzLm9wdGlvbnMubWF4O1xyXG4gICAgICAgICAgICB2YXIgZGF0YW1pbiA9IHhheGlzLmRhdGFtaW47XHJcbiAgICAgICAgICAgIHZhciBkYXRhbWF4ID0geGF4aXMuZGF0YW1heDtcclxuXHJcbiAgICAgICAgICAgIHZhciBkZWx0YU1hZ25pdHVkZTtcclxuICAgICAgICAgICAgdmFyIGRlbHRhO1xyXG4gICAgICAgICAgICB2YXIgZmFjdG9yO1xyXG5cclxuICAgICAgICAgICAgaWYgKHhtaW4gPT0gbnVsbClcclxuICAgICAgICAgICAgICAgIHhtaW4gPSBkYXRhbWluO1xyXG5cclxuICAgICAgICAgICAgaWYgKHhtYXggPT0gbnVsbClcclxuICAgICAgICAgICAgICAgIHhtYXggPSBkYXRhbWF4O1xyXG5cclxuICAgICAgICAgICAgaWYgKHhtaW4gPT0gbnVsbCB8fCB4bWF4ID09IG51bGwpXHJcbiAgICAgICAgICAgICAgICByZXR1cm47XHJcblxyXG4gICAgICAgICAgICB4Y2VudGVyID0gTWF0aC5tYXgoeGNlbnRlciwgeG1pbik7XHJcbiAgICAgICAgICAgIHhjZW50ZXIgPSBNYXRoLm1pbih4Y2VudGVyLCB4bWF4KTtcclxuXHJcbiAgICAgICAgICAgIGlmICgoZXZlbnQub3JpZ2luYWxFdmVudCBhcyBhbnkpLndoZWVsRGVsdGEgIT0gdW5kZWZpbmVkKVxyXG4gICAgICAgICAgICAgICAgZGVsdGEgPSAoZXZlbnQub3JpZ2luYWxFdmVudCBhcyBhbnkpLndoZWVsRGVsdGE7XHJcbiAgICAgICAgICAgIGVsc2VcclxuICAgICAgICAgICAgICAgIGRlbHRhID0gLShldmVudC5vcmlnaW5hbEV2ZW50IGFzIGFueSkuZGV0YWlsO1xyXG5cclxuICAgICAgICAgICAgZGVsdGFNYWduaXR1ZGUgPSBNYXRoLmFicyhkZWx0YSk7XHJcblxyXG4gICAgICAgICAgICBpZiAobWluRGVsdGEgPT0gbnVsbCB8fCBkZWx0YU1hZ25pdHVkZSA8IG1pbkRlbHRhKVxyXG4gICAgICAgICAgICAgICAgbWluRGVsdGEgPSBkZWx0YU1hZ25pdHVkZTtcclxuXHJcbiAgICAgICAgICAgIGRlbHRhTWFnbml0dWRlIC89IG1pbkRlbHRhO1xyXG4gICAgICAgICAgICBkZWx0YU1hZ25pdHVkZSA9IE1hdGgubWluKGRlbHRhTWFnbml0dWRlLCBtYXhEZWx0YSk7XHJcbiAgICAgICAgICAgIGZhY3RvciA9IGRlbHRhTWFnbml0dWRlIC8gMTA7XHJcblxyXG4gICAgICAgICAgICBpZiAoZGVsdGEgPiAwKSB7XHJcbiAgICAgICAgICAgICAgICB4bWluID0geG1pbiAqICgxIC0gZmFjdG9yKSArIHhjZW50ZXIgKiBmYWN0b3I7XHJcbiAgICAgICAgICAgICAgICB4bWF4ID0geG1heCAqICgxIC0gZmFjdG9yKSArIHhjZW50ZXIgKiBmYWN0b3I7XHJcbiAgICAgICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgICAgICB4bWluID0gKHhtaW4gLSB4Y2VudGVyICogZmFjdG9yKSAvICgxIC0gZmFjdG9yKTtcclxuICAgICAgICAgICAgICAgIHhtYXggPSAoeG1heCAtIHhjZW50ZXIgKiBmYWN0b3IpIC8gKDEgLSBmYWN0b3IpO1xyXG4gICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICBpZiAoeG1pbiA9PSB4YXhpcy5vcHRpb25zLnhtaW4gJiYgeG1heCA9PSB4YXhpcy5vcHRpb25zLnhtYXgpXHJcbiAgICAgICAgICAgICAgICByZXR1cm47XHJcblxyXG4gICAgICAgICAgICBjdHJsLnN0YXJ0RGF0ZSA9IGN0cmwuZ2V0RGF0ZVN0cmluZyh4bWluKTtcclxuICAgICAgICAgICAgY3RybC5lbmREYXRlID0gY3RybC5nZXREYXRlU3RyaW5nKHhtYXgpO1xyXG5cclxuICAgICAgICAgICAgY3RybC5jcmVhdGVEYXRhUm93cyhjdHJsLnByb3BzKTtcclxuXHJcbiAgICAgICAgICAgIGNsZWFyVGltZW91dChjdHJsLnpvb21JZCk7XHJcblxyXG4gICAgICAgICAgICBjdHJsLnpvb21JZCA9IHNldFRpbWVvdXQoKCkgPT4ge1xyXG4gICAgICAgICAgICAgICAgY3RybC5wcm9wcy5zdGF0ZVNldHRlcih7IHN0YXJ0RGF0ZTogY3RybC5nZXREYXRlU3RyaW5nKHhtaW4pLCBlbmREYXRlOiBjdHJsLmdldERhdGVTdHJpbmcoeG1heCkgfSk7XHJcbiAgICAgICAgICAgIH0sIDI1MCk7XHJcbiAgICAgICAgfSk7XHJcblxyXG4gICAgfVxyXG5cclxuICAgIHBsb3RIb3ZlcigpIHtcclxuICAgICAgICB2YXIgY3RybCA9IHRoaXM7XHJcbiAgICAgICAgJCh0aGlzLnJlZnMuZ3JhcGgpLm9mZihcInBsb3Rob3ZlclwiKTtcclxuICAgICAgICAkKHRoaXMucmVmcy5ncmFwaCkuYmluZChcInBsb3Rob3ZlclwiLCBmdW5jdGlvbiAoZXZlbnQsIHBvcywgaXRlbSkge1xyXG4gICAgICAgICAgICBjdHJsLmhvdmVyID0gcG9zLng7XHJcbiAgICAgICAgfSk7XHJcbiAgICB9XHJcblxyXG5cclxufSIsIi8vKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbi8vICBUcmVuZGluZ0RhdGFEaXNwbGF5LnRzeCAtIEdidGNcclxuLy9cclxuLy8gIENvcHlyaWdodCDCqSAyMDE4LCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxyXG4vL1xyXG4vLyAgTGljZW5zZWQgdG8gdGhlIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZSAoR1BBKSB1bmRlciBvbmUgb3IgbW9yZSBjb250cmlidXRvciBsaWNlbnNlIGFncmVlbWVudHMuIFNlZVxyXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxyXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xyXG4vLyAgZmlsZSBleGNlcHQgaW4gY29tcGxpYW5jZSB3aXRoIHRoZSBMaWNlbnNlLiBZb3UgbWF5IG9idGFpbiBhIGNvcHkgb2YgdGhlIExpY2Vuc2UgYXQ6XHJcbi8vXHJcbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxyXG4vL1xyXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxyXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXHJcbi8vICBMaWNlbnNlIGZvciB0aGUgc3BlY2lmaWMgbGFuZ3VhZ2UgZ292ZXJuaW5nIHBlcm1pc3Npb25zIGFuZCBsaW1pdGF0aW9ucy5cclxuLy9cclxuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XHJcbi8vICAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vICAwNy8xOS8yMDE4IC0gQmlsbHkgRXJuZXN0XHJcbi8vICAgICAgIEdlbmVyYXRlZCBvcmlnaW5hbCB2ZXJzaW9uIG9mIHNvdXJjZSBjb2RlLlxyXG4vL1xyXG4vLyoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5cclxuaW1wb3J0ICogYXMgUmVhY3QgZnJvbSAncmVhY3QnO1xyXG5pbXBvcnQgKiBhcyBSZWFjdERPTSBmcm9tICdyZWFjdC1kb20nO1xyXG5pbXBvcnQgVHJlbmRpbmdEYXRhRGlzcGxheVNlcnZpY2UgZnJvbSAnLi8uLi9UUy9TZXJ2aWNlcy9UcmVuZGluZ0RhdGFEaXNwbGF5JztcclxuaW1wb3J0IGNyZWF0ZUhpc3RvcnkgZnJvbSBcImhpc3RvcnkvY3JlYXRlQnJvd3Nlckhpc3RvcnlcIlxyXG5pbXBvcnQgKiBhcyBxdWVyeVN0cmluZyBmcm9tIFwicXVlcnktc3RyaW5nXCI7XHJcbmltcG9ydCAqIGFzIG1vbWVudCBmcm9tICdtb21lbnQnO1xyXG5pbXBvcnQgKiBhcyBfIGZyb20gXCJsb2Rhc2hcIjtcclxuaW1wb3J0IE1ldGVySW5wdXQgZnJvbSAnLi9NZXRlcklucHV0JztcclxuaW1wb3J0IE1lYXN1cmVtZW50SW5wdXQgZnJvbSAnLi9NZWFzdXJlbWVudElucHV0JztcclxuaW1wb3J0IFRyZW5kaW5nQ2hhcnQgZnJvbSAnLi9UcmVuZGluZ0NoYXJ0JztcclxuaW1wb3J0IERhdGVUaW1lUmFuZ2VQaWNrZXIgZnJvbSAnLi9EYXRlVGltZVJhbmdlUGlja2VyJztcclxuaW1wb3J0IHsgIFRyZW5kaW5nY0RhdGFEaXNwbGF5IH0gZnJvbSAnLi9nbG9iYWwnXHJcbmltcG9ydCB7IFJhbmRvbUNvbG9yIH0gZnJvbSAnQGdwYS1nZW1zdG9uZS9oZWxwZXItZnVuY3Rpb25zJztcclxuXHJcbmNvbnN0IFRyZW5kaW5nRGF0YURpc3BsYXkgPSAoKSA9PiB7XHJcbiAgICBsZXQgdHJlbmRpbmdEYXRhRGlzcGxheVNlcnZpY2UgPSBuZXcgVHJlbmRpbmdEYXRhRGlzcGxheVNlcnZpY2UoKTtcclxuICAgIGNvbnN0IHJlc2l6ZUlkID0gUmVhY3QudXNlUmVmKG51bGwpO1xyXG4gICAgY29uc3QgbG9hZGVyID0gUmVhY3QudXNlUmVmKG51bGwpO1xyXG5cclxuICAgIGxldCBoaXN0b3J5ID0gY3JlYXRlSGlzdG9yeSgpO1xyXG5cclxuICAgIGxldCBxdWVyeSA9IHF1ZXJ5U3RyaW5nLnBhcnNlKGhpc3RvcnlbJ2xvY2F0aW9uJ10uc2VhcmNoKTtcclxuXHJcbiAgICBjb25zdCBbbWVhc3VyZW1lbnRzLCBzZXRNZWFzdXJlbWVudHNdID0gUmVhY3QudXNlU3RhdGU8VHJlbmRpbmdjRGF0YURpc3BsYXkuTWVhc3VyZW1lbnRbXT4oc2Vzc2lvblN0b3JhZ2UuZ2V0SXRlbSgnVHJlbmRpbmdEYXRhRGlzcGxheS1tZWFzdXJlbWVudHMnKSA9PSBudWxsID8gW10gOiBKU09OLnBhcnNlKHNlc3Npb25TdG9yYWdlLmdldEl0ZW0oJ1RyZW5kaW5nRGF0YURpc3BsYXktbWVhc3VyZW1lbnRzJykpKTtcclxuICAgIGNvbnN0IFt3aWR0aCwgc2V0V2lkdGhdID0gUmVhY3QudXNlU3RhdGU8bnVtYmVyPih3aW5kb3cuaW5uZXJXaWR0aCAtIDQ3NSk7XHJcbiAgICBjb25zdCBbc3RhcnREYXRlLCBzZXRTdGFydERhdGVdID0gUmVhY3QudXNlU3RhdGU8c3RyaW5nPihxdWVyeVsnc3RhcnREYXRlJ10gIT0gdW5kZWZpbmVkID8gcXVlcnlbJ3N0YXJ0RGF0ZSddIDogbW9tZW50KCkuc3VidHJhY3QoNywgJ2RheScpLmZvcm1hdCgnWVlZWS1NTS1ERFRISDptbScpKTtcclxuICAgIGNvbnN0IFtlbmREYXRlLCBzZXRFbmREYXRlXSA9IFJlYWN0LnVzZVN0YXRlPHN0cmluZz4ocXVlcnlbJ2VuZERhdGUnXSAhPSB1bmRlZmluZWQgPyBxdWVyeVsnZW5kRGF0ZSddIDogbW9tZW50KCkuZm9ybWF0KCdZWVlZLU1NLUREVEhIOm1tJykpO1xyXG4gICAgY29uc3QgW2F4ZXMsIHNldEF4ZXNdID0gUmVhY3QudXNlU3RhdGU8VHJlbmRpbmdjRGF0YURpc3BsYXkuRmxvdEF4aXNbXT4oc2Vzc2lvblN0b3JhZ2UuZ2V0SXRlbSgnVHJlbmRpbmdEYXRhRGlzcGxheS1heGVzJykgPT0gbnVsbCA/IFt7IGF4aXNMYWJlbDogJ0RlZmF1bHQnLCBjb2xvcjogJ2JsYWNrJywgcG9zaXRpb246ICdsZWZ0JyB9XSA6IEpTT04ucGFyc2Uoc2Vzc2lvblN0b3JhZ2UuZ2V0SXRlbSgnVHJlbmRpbmdEYXRhRGlzcGxheS1heGVzJykpKTtcclxuXHJcbiAgICBSZWFjdC51c2VFZmZlY3QoKCkgPT4ge1xyXG4gICAgICAgIHdpbmRvdy5hZGRFdmVudExpc3RlbmVyKFwicmVzaXplXCIsIGhhbmRsZVNjcmVlblNpemVDaGFuZ2UuYmluZCh0aGlzKSk7XHJcbiAgICAgICAgLy9pZiAodGhpcy5zdGF0ZS5tZWFzdXJlbWVudElEICE9IDApIGdldERhdGEoKTtcclxuXHJcbiAgICAgICAgaGlzdG9yeVsnbGlzdGVuJ10oKGxvY2F0aW9uLCBhY3Rpb24pID0+IHtcclxuICAgICAgICAgICAgbGV0IHF1ZXJ5ID0gcXVlcnlTdHJpbmcucGFyc2UoaGlzdG9yeVsnbG9jYXRpb24nXS5zZWFyY2gpO1xyXG4gICAgICAgICAgICBzZXRTdGFydERhdGUocXVlcnlbJ3N0YXJ0RGF0ZSddICE9IHVuZGVmaW5lZCA/IHF1ZXJ5WydzdGFydERhdGUnXSA6IG1vbWVudCgpLnN1YnRyYWN0KDcsICdkYXknKS5mb3JtYXQoJ1lZWVktTU0tRERUSEg6bW0nKSk7XHJcbiAgICAgICAgICAgIHNldEVuZERhdGUocXVlcnlbJ2VuZERhdGUnXSAhPSB1bmRlZmluZWQgPyBxdWVyeVsnZW5kRGF0ZSddIDogbW9tZW50KCkuZm9ybWF0KCdZWVlZLU1NLUREVEhIOm1tJykpO1xyXG4gICAgICAgIH0pO1xyXG5cclxuICAgICAgICByZXR1cm4gKCkgPT4gJCh3aW5kb3cpLm9mZigncmVzaXplJyk7XHJcbiAgICB9LCBbXSk7XHJcblxyXG4gICAgUmVhY3QudXNlRWZmZWN0KCgpID0+IHtcclxuICAgICAgICBpZiAobWVhc3VyZW1lbnRzLmxlbmd0aCA9PSAwKSByZXR1cm47XHJcbiAgICAgICAgZ2V0RGF0YSgpO1xyXG4gICAgfSwgW21lYXN1cmVtZW50cy5sZW5ndGgsIHN0YXJ0RGF0ZSwgZW5kRGF0ZV0pO1xyXG5cclxuICAgIFJlYWN0LnVzZUVmZmVjdCgoKSA9PiB7XHJcbiAgICAgICAgaGlzdG9yeVsncHVzaCddKCdUcmVuZGluZ0RhdGFEaXNwbGF5LmNzaHRtbD8nICsgcXVlcnlTdHJpbmcuc3RyaW5naWZ5KHtzdGFydERhdGUsIGVuZERhdGV9LCB7IGVuY29kZTogZmFsc2UgfSkpXHJcbiAgICB9LCBbc3RhcnREYXRlLGVuZERhdGVdKTtcclxuXHJcbiAgICBSZWFjdC51c2VFZmZlY3QoKCkgPT4ge1xyXG4gICAgICAgIHNlc3Npb25TdG9yYWdlLnNldEl0ZW0oJ1RyZW5kaW5nRGF0YURpc3BsYXktbWVhc3VyZW1lbnRzJywgSlNPTi5zdHJpbmdpZnkobWVhc3VyZW1lbnRzLm1hcChtcyA9PiAoeyBJRDogbXMuSUQsIE1ldGVySUQ6IG1zLk1ldGVySUQsTWV0ZXJOYW1lOiBtcy5NZXRlck5hbWUsTWVhc3VyZW1lbnROYW1lOiBtcy5NZWFzdXJlbWVudE5hbWUsQXZlcmFnZTogbXMuQXZlcmFnZSxBdmdDb2xvcjogbXMuQXZnQ29sb3IsTWF4aW11bTogbXMuTWF4aW11bSwgTWF4Q29sb3I6IG1zLk1heENvbG9yLCBNaW5pbXVtOiBtcy5NaW5pbXVtLCBNaW5Db2xvcjogbXMuTWluQ29sb3IsIEF4aXM6IG1zLkF4aXN9KSkpKVxyXG4gICAgfSwgW21lYXN1cmVtZW50c10pO1xyXG5cclxuICAgIFJlYWN0LnVzZUVmZmVjdCgoKSA9PiB7XHJcbiAgICAgICAgc2Vzc2lvblN0b3JhZ2Uuc2V0SXRlbSgnVHJlbmRpbmdEYXRhRGlzcGxheS1heGVzJywgSlNPTi5zdHJpbmdpZnkoYXhlcykpXHJcbiAgICB9LCBbYXhlc10pO1xyXG5cclxuICAgIGZ1bmN0aW9uIGdldERhdGEoKSB7XHJcbiAgICAgICAgJChsb2FkZXIuY3VycmVudCkuc2hvdygpO1xyXG4gICAgICAgIHRyZW5kaW5nRGF0YURpc3BsYXlTZXJ2aWNlLmdldFBvc3REYXRhKG1lYXN1cmVtZW50cywgc3RhcnREYXRlLCBlbmREYXRlLCB3aWR0aCkuZG9uZShkYXRhID0+IHtcclxuICAgICAgICAgICAgbGV0IG1lYXMgPVsgLi4ubWVhc3VyZW1lbnRzIF07XHJcbiAgICAgICAgICAgIGZvciAobGV0IGtleSBvZiBPYmplY3Qua2V5cyhkYXRhKSkge1xyXG4gICAgICAgICAgICAgICAgbGV0IGkgPSBtZWFzLmZpbmRJbmRleCh4ID0+IHguSUQudG9TdHJpbmcoKSA9PT0ga2V5KTtcclxuICAgICAgICAgICAgICAgIG1lYXNbaV0uRGF0YSA9IGRhdGFba2V5XTtcclxuICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICBzZXRNZWFzdXJlbWVudHMobWVhcylcclxuXHJcbiAgICAgICAgICAgICQobG9hZGVyLmN1cnJlbnQpLmhpZGUoKVxyXG4gICAgICAgIH0pO1xyXG4gICAgfVxyXG5cclxuXHJcbiAgICBmdW5jdGlvbiBoYW5kbGVTY3JlZW5TaXplQ2hhbmdlKCkge1xyXG4gICAgICAgIGNsZWFyVGltZW91dCh0aGlzLnJlc2l6ZUlkKTtcclxuICAgICAgICB0aGlzLnJlc2l6ZUlkID0gc2V0VGltZW91dCgoKSA9PiB7XHJcbiAgICAgICAgfSwgNTAwKTtcclxuICAgIH1cclxuXHJcbiAgICBsZXQgaGVpZ2h0ID0gd2luZG93LmlubmVySGVpZ2h0IC0gJCgnI25hdmJhcicpLmhlaWdodCgpO1xyXG4gICAgbGV0IG1lbnVXaWR0aCA9IDI1MDtcclxuICAgIGxldCBzaWRlV2lkdGggPSA0MDA7XHJcbiAgICBsZXQgdG9wID0gJCgnI25hdmJhcicpLmhlaWdodCgpIC0gMzA7XHJcbiAgICByZXR1cm4gKFxyXG4gICAgICAgIDxkaXY+XHJcbiAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwic2NyZWVuXCIgc3R5bGU9e3sgaGVpZ2h0OiBoZWlnaHQsIHdpZHRoOiB3aW5kb3cuaW5uZXJXaWR0aCwgcG9zaXRpb246ICdyZWxhdGl2ZScsIHRvcDogdG9wIH19PlxyXG4gICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJ2ZXJ0aWNhbC1tZW51XCIgc3R5bGU9e3ttYXhIZWlnaHQ6IGhlaWdodCwgb3ZlcmZsb3dZOiAnYXV0bycgfX0+XHJcbiAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJmb3JtLWdyb3VwXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxsYWJlbD5UaW1lIFJhbmdlOiA8L2xhYmVsPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8RGF0ZVRpbWVSYW5nZVBpY2tlciBzdGFydERhdGU9e3N0YXJ0RGF0ZX0gZW5kRGF0ZT17ZW5kRGF0ZX0gc3RhdGVTZXR0ZXI9eyhvYmopID0+IHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGlmKHN0YXJ0RGF0ZSAhPSBvYmouc3RhcnREYXRlKVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHNldFN0YXJ0RGF0ZShvYmouc3RhcnREYXRlKTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGlmIChlbmREYXRlICE9IG9iai5lbmREYXRlKVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHNldEVuZERhdGUob2JqLmVuZERhdGUpO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB9fSAvPlxyXG4gICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiZm9ybS1ncm91cFwiIHN0eWxlPXt7aGVpZ2h0OiA1MH19PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IHN0eWxlPXt7IGZsb2F0OiAnbGVmdCcgfX0gcmVmPXtsb2FkZXJ9IGhpZGRlbj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgc3R5bGU9e3sgYm9yZGVyOiAnNXB4IHNvbGlkICNmM2YzZjMnLCBXZWJraXRBbmltYXRpb246ICdzcGluIDFzIGxpbmVhciBpbmZpbml0ZScsIGFuaW1hdGlvbjogJ3NwaW4gMXMgbGluZWFyIGluZmluaXRlJywgYm9yZGVyVG9wOiAnNXB4IHNvbGlkICM1NTUnLCBib3JkZXJSYWRpdXM6ICc1MCUnLCB3aWR0aDogJzI1cHgnLCBoZWlnaHQ6ICcyNXB4JyB9fT48L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxzcGFuPkxvYWRpbmcuLi48L3NwYW4+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG5cclxuXHJcbiAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJmb3JtLWdyb3VwXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwicGFuZWwtZ3JvdXBcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwicGFuZWwgcGFuZWwtZGVmYXVsdFwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwicGFuZWwtaGVhZGluZ1wiIHN0eWxlPXt7IHBvc2l0aW9uOiAncmVsYXRpdmUnLCBoZWlnaHQ6IDYwfX0+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxoNCBjbGFzc05hbWU9XCJwYW5lbC10aXRsZVwiIHN0eWxlPXt7IHBvc2l0aW9uOiAnYWJzb2x1dGUnLCBsZWZ0OiAxNSwgd2lkdGg6ICc2MCUnIH19PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGEgZGF0YS10b2dnbGU9XCJjb2xsYXBzZVwiIGhyZWY9XCIjTWVhc3VyZW1lbnRDb2xsYXBzZVwiPk1lYXN1cmVtZW50czo8L2E+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvaDQ+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxBZGRNZWFzdXJlbWVudCBBZGQ9eyhtc250KSA9PiBzZXRNZWFzdXJlbWVudHMobWVhc3VyZW1lbnRzLmNvbmNhdChtc250KSl9IC8+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBpZD0nTWVhc3VyZW1lbnRDb2xsYXBzZScgY2xhc3NOYW1lPSdwYW5lbC1jb2xsYXBzZSc+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx1bCBjbGFzc05hbWU9J2xpc3QtZ3JvdXAnPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAge21lYXN1cmVtZW50cy5tYXAoKG1zLCBpKSA9PiA8TWVhc3VyZW1lbnRSb3cga2V5PXtpfSBNZWFzdXJlbWVudD17bXN9IE1lYXN1cmVtZW50cz17bWVhc3VyZW1lbnRzfSBBeGVzPXtheGVzfSBJbmRleD17aX0gU2V0TWVhc3VyZW1lbnRzPXtzZXRNZWFzdXJlbWVudHN9IC8+KX1cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC91bD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcblxyXG5cclxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImZvcm0tZ3JvdXBcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJwYW5lbC1ncm91cFwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJwYW5lbCBwYW5lbC1kZWZhdWx0XCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJwYW5lbC1oZWFkaW5nXCIgc3R5bGU9e3sgcG9zaXRpb246ICdyZWxhdGl2ZScsIGhlaWdodDogNjAgfX0+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxoNCBjbGFzc05hbWU9XCJwYW5lbC10aXRsZVwiIHN0eWxlPXt7IHBvc2l0aW9uOiAnYWJzb2x1dGUnLCBsZWZ0OiAxNSwgd2lkdGg6ICc2MCUnIH19PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGEgZGF0YS10b2dnbGU9XCJjb2xsYXBzZVwiIGhyZWY9XCIjYXhlc0NvbGxhcHNlXCI+QXhlczo8L2E+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvaDQ+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxBZGRBeGlzIEFkZD17KGF4aXMpID0+IHNldEF4ZXMoYXhlcy5jb25jYXQoYXhpcykpfSAvPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgaWQ9J2F4ZXNDb2xsYXBzZScgY2xhc3NOYW1lPSdwYW5lbC1jb2xsYXBzZSc+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDx1bCBjbGFzc05hbWU9J2xpc3QtZ3JvdXAnPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAge2F4ZXMubWFwKChheGlzLCBpKSA9PiA8QXhpc1JvdyBrZXk9e2l9IEF4ZXM9e2F4ZXN9IEluZGV4PXtpfSBTZXRBeGVzPXtzZXRBeGVzfS8+KX1cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC91bD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcblxyXG4gICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cIndhdmVmb3JtLXZpZXdlclwiIHN0eWxlPXt7IHdpZHRoOiB3aW5kb3cuaW5uZXJXaWR0aCAtIG1lbnVXaWR0aCwgaGVpZ2h0OiBoZWlnaHQsIGZsb2F0OiAnbGVmdCcsIGxlZnQ6IDAgfX0+XHJcbiAgICAgICAgICAgICAgICAgICAgPFRyZW5kaW5nQ2hhcnQgc3RhcnREYXRlPXtzdGFydERhdGV9IGVuZERhdGU9e2VuZERhdGV9IGRhdGE9e21lYXN1cmVtZW50c30gYXhlcz17YXhlc30gc3RhdGVTZXR0ZXI9eyhvYmplY3QpID0+IHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgc2V0U3RhcnREYXRlKG9iamVjdC5zdGFydERhdGUpO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBzZXRFbmREYXRlKG9iamVjdC5lbmREYXRlKTtcclxuICAgICAgICAgICAgICAgICAgICB9fSAvPlxyXG4gICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgIDwvZGl2PlxyXG4gICAgKTtcclxufVxyXG5cclxuY29uc3QgQWRkTWVhc3VyZW1lbnQgPSAocHJvcHM6IHsgQWRkOiAobXNudDpUcmVuZGluZ2NEYXRhRGlzcGxheS5NZWFzdXJlbWVudCkgPT4gdm9pZH0pID0+IHtcclxuICAgIGNvbnN0IFttZWFzdXJlbWVudCwgc2V0TWVhc3VyZW1lbnRdID0gUmVhY3QudXNlU3RhdGU8VHJlbmRpbmdjRGF0YURpc3BsYXkuTWVhc3VyZW1lbnQ+KHsgSUQ6IDAsIE1ldGVySUQ6IDAsIE1ldGVyTmFtZTogJycsIE1lYXN1cmVtZW50TmFtZTogJycsIE1heGltdW06IHRydWUsIE1heENvbG9yOiBSYW5kb21Db2xvcigpLCBBdmVyYWdlOiB0cnVlLCBBdmdDb2xvcjogUmFuZG9tQ29sb3IoKSwgTWluaW11bTogdHJ1ZSwgTWluQ29sb3I6IFJhbmRvbUNvbG9yKCksIEF4aXM6IDEgfSk7XHJcblxyXG4gICAgcmV0dXJuIChcclxuICAgICAgICA8PlxyXG4gICAgICAgICAgICA8YnV0dG9uIHR5cGU9XCJidXR0b25cIiBzdHlsZT17eyBwb3NpdGlvbjogJ2Fic29sdXRlJywgcmlnaHQ6IDE1fX0gY2xhc3NOYW1lPVwiYnRuIGJ0bi1pbmZvXCIgZGF0YS10b2dnbGU9XCJtb2RhbFwiIGRhdGEtdGFyZ2V0PVwiI21lYXN1cmVtZW50TW9kYWxcIiBvbkNsaWNrPXsoKSA9PiB7XHJcbiAgICAgICAgICAgICAgICBzZXRNZWFzdXJlbWVudCh7IElEOiAwLCBNZXRlcklEOiAwLCBNZXRlck5hbWU6ICcnLCBNZWFzdXJlbWVudE5hbWU6ICcnLCBNYXhpbXVtOiB0cnVlLCBNYXhDb2xvcjogUmFuZG9tQ29sb3IoKSwgQXZlcmFnZTogdHJ1ZSwgQXZnQ29sb3I6IFJhbmRvbUNvbG9yKCksIE1pbmltdW06IHRydWUsIE1pbkNvbG9yOiBSYW5kb21Db2xvcigpLCBBeGlzOiAxIH0pO1xyXG4gICAgICAgICAgICB9fT5BZGQ8L2J1dHRvbj5cclxuICAgICAgICAgICAgPGRpdiBpZD1cIm1lYXN1cmVtZW50TW9kYWxcIiBjbGFzc05hbWU9XCJtb2RhbCBmYWRlXCIgcm9sZT1cImRpYWxvZ1wiPlxyXG4gICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJtb2RhbC1kaWFsb2dcIj5cclxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cIm1vZGFsLWNvbnRlbnRcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJtb2RhbC1oZWFkZXJcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxidXR0b24gdHlwZT1cImJ1dHRvblwiIGNsYXNzTmFtZT1cImNsb3NlXCIgZGF0YS1kaXNtaXNzPVwibW9kYWxcIj4mdGltZXM7PC9idXR0b24+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8aDQgY2xhc3NOYW1lPVwibW9kYWwtdGl0bGVcIj5BZGQgTWVhc3VyZW1lbnQ8L2g0PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJtb2RhbC1ib2R5XCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImZvcm0tZ3JvdXBcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8bGFiZWw+TWV0ZXI6IDwvbGFiZWw+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPE1ldGVySW5wdXQgdmFsdWU9e21lYXN1cmVtZW50Lk1ldGVySUR9IG9uQ2hhbmdlPXsob2JqKSA9PiBzZXRNZWFzdXJlbWVudCh7IC4uLm1lYXN1cmVtZW50LCBNZXRlcklEOiBvYmoubWV0ZXJJRCwgTWV0ZXJOYW1lOiBvYmoubWV0ZXJOYW1lIH0pfSAvPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJmb3JtLWdyb3VwXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGxhYmVsPk1lYXN1cmVtZW50OiA8L2xhYmVsPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxNZWFzdXJlbWVudElucHV0IG1ldGVySUQ9e21lYXN1cmVtZW50Lk1ldGVySUR9IHZhbHVlPXttZWFzdXJlbWVudC5JRH0gb25DaGFuZ2U9eyhvYmopID0+IHNldE1lYXN1cmVtZW50KHsgLi4ubWVhc3VyZW1lbnQsIElEOiBvYmoubWVhc3VyZW1lbnRJRCwgTWVhc3VyZW1lbnROYW1lOiBvYmoubWVhc3VyZW1lbnROYW1lIH0pfSAvPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJyb3dcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImNvbC1sZy02XCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiY2hlY2tib3hcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxsYWJlbD48aW5wdXQgdHlwZT1cImNoZWNrYm94XCIgY2hlY2tlZD17bWVhc3VyZW1lbnQuTWF4aW11bX0gdmFsdWU9e21lYXN1cmVtZW50Lk1heGltdW0udG9TdHJpbmcoKX0gb25DaGFuZ2U9eygpID0+IHNldE1lYXN1cmVtZW50KHsgLi4ubWVhc3VyZW1lbnQsIE1heGltdW06ICFtZWFzdXJlbWVudC5NYXhpbXVtIH0pIH0vPiBNYXhpbXVtPC9sYWJlbD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJjb2wtbGctNlwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8aW5wdXQgdHlwZT1cImNvbG9yXCIgc3R5bGU9e3t0ZXh0QWxpZ246J2xlZnQnfX0gY2xhc3NOYW1lPVwiZm9ybS1jb250cm9sXCIgdmFsdWU9e21lYXN1cmVtZW50Lk1heENvbG9yfSBvbkNoYW5nZT17KGV2dCkgPT4gc2V0TWVhc3VyZW1lbnQoeyAuLi5tZWFzdXJlbWVudCwgTWF4Q29sb3I6IGV2dC50YXJnZXQudmFsdWUgfSl9IC8+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cInJvd1wiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiY29sLWxnLTZcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJjaGVja2JveFwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGxhYmVsPjxpbnB1dCB0eXBlPVwiY2hlY2tib3hcIiBjaGVja2VkPXttZWFzdXJlbWVudC5BdmVyYWdlfSB2YWx1ZT17bWVhc3VyZW1lbnQuQXZlcmFnZS50b1N0cmluZygpfSBvbkNoYW5nZT17KCkgPT4gc2V0TWVhc3VyZW1lbnQoeyAuLi5tZWFzdXJlbWVudCwgQXZlcmFnZTogIW1lYXN1cmVtZW50LkF2ZXJhZ2UgfSl9IC8+IEF2ZXJhZ2U8L2xhYmVsPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImNvbC1sZy02XCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxpbnB1dCB0eXBlPVwiY29sb3JcIiBzdHlsZT17eyB0ZXh0QWxpZ246ICdsZWZ0JyB9fSBjbGFzc05hbWU9XCJmb3JtLWNvbnRyb2xcIiB2YWx1ZT17bWVhc3VyZW1lbnQuQXZnQ29sb3J9IG9uQ2hhbmdlPXsoZXZ0KSA9PiBzZXRNZWFzdXJlbWVudCh7IC4uLm1lYXN1cmVtZW50LCBBdmdDb2xvcjogZXZ0LnRhcmdldC52YWx1ZSB9KX0gLz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwicm93XCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJjb2wtbGctNlwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImNoZWNrYm94XCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8bGFiZWw+PGlucHV0IHR5cGU9XCJjaGVja2JveFwiIGNoZWNrZWQ9e21lYXN1cmVtZW50Lk1pbmltdW19IHZhbHVlPXttZWFzdXJlbWVudC5NaW5pbXVtLnRvU3RyaW5nKCl9IG9uQ2hhbmdlPXsoKSA9PiBzZXRNZWFzdXJlbWVudCh7IC4uLm1lYXN1cmVtZW50LCBNaW5pbXVtOiAhbWVhc3VyZW1lbnQuTWluaW11bSB9KX0gLz4gTWluaW11bTwvbGFiZWw+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiY29sLWxnLTZcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGlucHV0IHR5cGU9XCJjb2xvclwiIHN0eWxlPXt7IHRleHRBbGlnbjogJ2xlZnQnIH19IGNsYXNzTmFtZT1cImZvcm0tY29udHJvbFwiIHZhbHVlPXttZWFzdXJlbWVudC5NaW5Db2xvcn0gb25DaGFuZ2U9eyhldnQpID0+IHNldE1lYXN1cmVtZW50KHsgLi4ubWVhc3VyZW1lbnQsIE1pbkNvbG9yOiBldnQudGFyZ2V0LnZhbHVlIH0pfSAvPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwibW9kYWwtZm9vdGVyXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8YnV0dG9uIHR5cGU9XCJidXR0b25cIiBjbGFzc05hbWU9XCJidG4gYnRuLXByaW1hcnlcIiBkYXRhLWRpc21pc3M9XCJtb2RhbFwiIG9uQ2xpY2s9eygpID0+IHByb3BzLkFkZChtZWFzdXJlbWVudCkgfT5BZGQ8L2J1dHRvbj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxidXR0b24gdHlwZT1cImJ1dHRvblwiIGNsYXNzTmFtZT1cImJ0biBidG4tZGVmYXVsdFwiIGRhdGEtZGlzbWlzcz1cIm1vZGFsXCI+Q2FuY2VsPC9idXR0b24+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgIDwvZGl2PlxyXG5cclxuICAgICAgICA8Lz5cclxuICAgICk7XHJcbn1cclxuXHJcbmNvbnN0IE1lYXN1cmVtZW50Um93ID0gKHByb3BzOiB7IE1lYXN1cmVtZW50OiBUcmVuZGluZ2NEYXRhRGlzcGxheS5NZWFzdXJlbWVudCwgTWVhc3VyZW1lbnRzOiBUcmVuZGluZ2NEYXRhRGlzcGxheS5NZWFzdXJlbWVudFtdLCBJbmRleDogbnVtYmVyLCBTZXRNZWFzdXJlbWVudHM6IChtZWFzdXJlbWVudHM6IFRyZW5kaW5nY0RhdGFEaXNwbGF5Lk1lYXN1cmVtZW50W10pID0+IHZvaWQsIEF4ZXM6IFRyZW5kaW5nY0RhdGFEaXNwbGF5LkZsb3RBeGlzW119KSA9PiB7XHJcbiAgICByZXR1cm4gKFxyXG4gICAgICAgIDxsaSBjbGFzc05hbWU9J2xpc3QtZ3JvdXAtaXRlbScga2V5PXtwcm9wcy5NZWFzdXJlbWVudC5JRH0+XHJcbiAgICAgICAgICAgIDxkaXY+e3Byb3BzLk1lYXN1cmVtZW50Lk1ldGVyTmFtZX08L2Rpdj48YnV0dG9uIHR5cGU9XCJidXR0b25cIiBzdHlsZT17eyBwb3NpdGlvbjogJ3JlbGF0aXZlJywgdG9wOiAtMjAgfX0gY2xhc3NOYW1lPVwiY2xvc2VcIiBvbkNsaWNrPXsoKSA9PiB7XHJcbiAgICAgICAgICAgICAgICBsZXQgbWVhcyA9IFsuLi5wcm9wcy5NZWFzdXJlbWVudHNdO1xyXG4gICAgICAgICAgICAgICAgbWVhcy5zcGxpY2UocHJvcHMuSW5kZXgsIDEpO1xyXG4gICAgICAgICAgICAgICAgcHJvcHMuU2V0TWVhc3VyZW1lbnRzKG1lYXMpXHJcbiAgICAgICAgICAgIH19PiZ0aW1lczs8L2J1dHRvbj5cclxuICAgICAgICAgICAgPGRpdj57cHJvcHMuTWVhc3VyZW1lbnQuTWVhc3VyZW1lbnROYW1lfTwvZGl2PlxyXG4gICAgICAgICAgICA8ZGl2PlxyXG4gICAgICAgICAgICAgICAgPHNlbGVjdCBjbGFzc05hbWU9J2Zvcm0tY29udHJvbCcgdmFsdWU9e3Byb3BzLk1lYXN1cmVtZW50LkF4aXN9IG9uQ2hhbmdlPXsoZXZ0KSA9PiB7XHJcbiAgICAgICAgICAgICAgICAgICAgbGV0IG1lYXMgPSBbLi4ucHJvcHMuTWVhc3VyZW1lbnRzXTtcclxuICAgICAgICAgICAgICAgICAgICBtZWFzW3Byb3BzLkluZGV4XS5BeGlzID0gcGFyc2VJbnQoZXZ0LnRhcmdldC52YWx1ZSk7XHJcbiAgICAgICAgICAgICAgICAgICAgcHJvcHMuU2V0TWVhc3VyZW1lbnRzKG1lYXMpXHJcbiAgICAgICAgICAgICAgICB9fT5cclxuICAgICAgICAgICAgICAgICAgICB7cHJvcHMuQXhlcy5tYXAoKGEsIGl4KSA9PiA8b3B0aW9uIGtleT17aXh9IHZhbHVlPXtpeCArIDF9PnthLmF4aXNMYWJlbH08L29wdGlvbj4pfVxyXG4gICAgICAgICAgICAgICAgPC9zZWxlY3Q+XHJcblxyXG4gICAgICAgICAgICA8L2Rpdj5cclxuXHJcbiAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPSdyb3cnPlxyXG4gICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9J2NvbC1sZy00Jz5cclxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cIlwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImNoZWNrYm94XCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8bGFiZWw+PGlucHV0IHR5cGU9XCJjaGVja2JveFwiIGNoZWNrZWQ9e3Byb3BzLk1lYXN1cmVtZW50Lk1heGltdW19IHZhbHVlPXtwcm9wcy5NZWFzdXJlbWVudC5NYXhpbXVtLnRvU3RyaW5nKCl9IG9uQ2hhbmdlPXsoKSA9PiB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgbGV0IG1lYXMgPSBbLi4ucHJvcHMuTWVhc3VyZW1lbnRzXTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBtZWFzW3Byb3BzLkluZGV4XS5NYXhpbXVtID0gIW1lYXNbcHJvcHMuSW5kZXhdLk1heGltdW07XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcHJvcHMuU2V0TWVhc3VyZW1lbnRzKG1lYXMpXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB9fSAvPiBNYXg8L2xhYmVsPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cIlwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8aW5wdXQgdHlwZT1cImNvbG9yXCIgY2xhc3NOYW1lPVwiZm9ybS1jb250cm9sXCIgdmFsdWU9e3Byb3BzLk1lYXN1cmVtZW50Lk1heENvbG9yfSBvbkNoYW5nZT17KGV2dCkgPT4ge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgbGV0IG1lYXMgPSBbLi4ucHJvcHMuTWVhc3VyZW1lbnRzXTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIG1lYXNbcHJvcHMuSW5kZXhdLk1heENvbG9yID0gZXZ0LnRhcmdldC52YWx1ZTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHByb3BzLlNldE1lYXN1cmVtZW50cyhtZWFzKVxyXG4gICAgICAgICAgICAgICAgICAgICAgICB9fSAvPlxyXG4gICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT0nY29sLWxnLTQnPlxyXG4gICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiY2hlY2tib3hcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxsYWJlbD48aW5wdXQgdHlwZT1cImNoZWNrYm94XCIgY2hlY2tlZD17cHJvcHMuTWVhc3VyZW1lbnQuQXZlcmFnZX0gdmFsdWU9e3Byb3BzLk1lYXN1cmVtZW50LkF2ZXJhZ2UudG9TdHJpbmcoKX0gb25DaGFuZ2U9eygpID0+IHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBsZXQgbWVhcyA9IFsuLi5wcm9wcy5NZWFzdXJlbWVudHNdO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIG1lYXNbcHJvcHMuSW5kZXhdLkF2ZXJhZ2UgPSAhbWVhc1twcm9wcy5JbmRleF0uQXZlcmFnZTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBwcm9wcy5TZXRNZWFzdXJlbWVudHMobWVhcylcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIH19IC8+IEF2ZzwvbGFiZWw+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxpbnB1dCB0eXBlPVwiY29sb3JcIiBjbGFzc05hbWU9XCJmb3JtLWNvbnRyb2xcIiB2YWx1ZT17cHJvcHMuTWVhc3VyZW1lbnQuQXZnQ29sb3J9IG9uQ2hhbmdlPXsoZXZ0KSA9PiB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBsZXQgbWVhcyA9IFsuLi5wcm9wcy5NZWFzdXJlbWVudHNdO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgbWVhc1twcm9wcy5JbmRleF0uQXZnQ29sb3IgPSBldnQudGFyZ2V0LnZhbHVlO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgcHJvcHMuU2V0TWVhc3VyZW1lbnRzKG1lYXMpXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIH19IC8+XHJcbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPSdjb2wtbGctNCc+XHJcbiAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJjaGVja2JveFwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPGxhYmVsPjxpbnB1dCB0eXBlPVwiY2hlY2tib3hcIiBjaGVja2VkPXtwcm9wcy5NZWFzdXJlbWVudC5NaW5pbXVtfSB2YWx1ZT17cHJvcHMuTWVhc3VyZW1lbnQuTWluaW11bS50b1N0cmluZygpfSBvbkNoYW5nZT17KCkgPT4ge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGxldCBtZWFzID0gWy4uLnByb3BzLk1lYXN1cmVtZW50c107XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgbWVhc1twcm9wcy5JbmRleF0uTWluaW11bSA9ICFtZWFzW3Byb3BzLkluZGV4XS5NaW5pbXVtO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHByb3BzLlNldE1lYXN1cmVtZW50cyhtZWFzKVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgfX0gLz4gTWluPC9sYWJlbD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPGlucHV0IHR5cGU9XCJjb2xvclwiIGNsYXNzTmFtZT1cImZvcm0tY29udHJvbFwiIHZhbHVlPXtwcm9wcy5NZWFzdXJlbWVudC5NaW5Db2xvcn0gb25DaGFuZ2U9eyhldnQpID0+IHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGxldCBtZWFzID0gWy4uLnByb3BzLk1lYXN1cmVtZW50c107XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBtZWFzW3Byb3BzLkluZGV4XS5NaW5Db2xvciA9IGV2dC50YXJnZXQudmFsdWU7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBwcm9wcy5TZXRNZWFzdXJlbWVudHMobWVhcylcclxuICAgICAgICAgICAgICAgICAgICAgICAgfX0gLz5cclxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgIDwvZGl2PlxyXG5cclxuICAgICAgICAgICAgPC9kaXY+XHJcblxyXG4gICAgICAgIDwvbGk+XHJcblxyXG4gICAgKTtcclxufVxyXG5cclxuY29uc3QgQWRkQXhpcyA9IChwcm9wczogeyBBZGQ6IChheGlzOiBUcmVuZGluZ2NEYXRhRGlzcGxheS5GbG90QXhpcykgPT4gdm9pZCB9KSA9PiB7XHJcbiAgICBjb25zdCBbYXhpcywgc2V0QXhpc10gPSBSZWFjdC51c2VTdGF0ZTxUcmVuZGluZ2NEYXRhRGlzcGxheS5GbG90QXhpcz4oeyBwb3NpdGlvbjogJ2xlZnQnLCBjb2xvcjogJ2JsYWNrJywgYXhpc0xhYmVsOiAnJywgYXhpc0xhYmVsVXNlQ2FudmFzOiB0cnVlLCBzaG93OiB0cnVlIH0pO1xyXG5cclxuICAgIHJldHVybiAoXHJcbiAgICAgICAgPD5cclxuICAgICAgICAgICAgPGJ1dHRvbiB0eXBlPVwiYnV0dG9uXCIgc3R5bGU9e3sgcG9zaXRpb246ICdhYnNvbHV0ZScsIHJpZ2h0OiAxNSB9fSAgY2xhc3NOYW1lPVwiYnRuIGJ0bi1pbmZvXCIgZGF0YS10b2dnbGU9XCJtb2RhbFwiIGRhdGEtdGFyZ2V0PVwiI2F4aXNNb2RhbFwiIG9uQ2xpY2s9eygpID0+IHtcclxuICAgICAgICAgICAgICAgIHNldEF4aXMoeyBwb3NpdGlvbjogJ2xlZnQnLCBjb2xvcjogJ2JsYWNrJywgYXhpc0xhYmVsOiAnJywgYXhpc0xhYmVsVXNlQ2FudmFzOiB0cnVlLCBzaG93OiB0cnVlIH0pO1xyXG4gICAgICAgICAgICB9fT5BZGQ8L2J1dHRvbj5cclxuICAgICAgICAgICAgPGRpdiBpZD1cImF4aXNNb2RhbFwiIGNsYXNzTmFtZT1cIm1vZGFsIGZhZGVcIiByb2xlPVwiZGlhbG9nXCI+XHJcbiAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cIm1vZGFsLWRpYWxvZ1wiPlxyXG4gICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwibW9kYWwtY29udGVudFwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cIm1vZGFsLWhlYWRlclwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPGJ1dHRvbiB0eXBlPVwiYnV0dG9uXCIgY2xhc3NOYW1lPVwiY2xvc2VcIiBkYXRhLWRpc21pc3M9XCJtb2RhbFwiPiZ0aW1lczs8L2J1dHRvbj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxoNCBjbGFzc05hbWU9XCJtb2RhbC10aXRsZVwiPkFkZCBBeGlzPC9oND5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwibW9kYWwtYm9keVwiPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJmb3JtLWdyb3VwXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGxhYmVsPkxhYmVsOiA8L2xhYmVsPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxpbnB1dCB0eXBlPVwidGV4dFwiIGNsYXNzTmFtZT1cImZvcm0tY29udHJvbFwiIHZhbHVlPXtheGlzLmF4aXNMYWJlbH0gb25DaGFuZ2U9eyhldnQpID0+IHNldEF4aXMoeyAuLi5heGlzLCBheGlzTGFiZWw6IGV2dC50YXJnZXQudmFsdWUgfSl9IC8+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImZvcm0tZ3JvdXBcIj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8bGFiZWw+UG9zaXRpb246IDwvbGFiZWw+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHNlbGVjdCBjbGFzc05hbWU9J2Zvcm0tY29udHJvbCcgdmFsdWU9e2F4aXMucG9zaXRpb259IG9uQ2hhbmdlPXsoZXZ0KSA9PiBzZXRBeGlzKHsgLi4uYXhpcywgcG9zaXRpb246IGV2dC50YXJnZXQudmFsdWUgYXMgJ2xlZnQnIHwgJ3JpZ2h0J30pfT5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPG9wdGlvbiB2YWx1ZT0nbGVmdCc+bGVmdDwvb3B0aW9uPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8b3B0aW9uIHZhbHVlPSdyaWdodCc+cmlnaHQ8L29wdGlvbj5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3NlbGVjdD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwibW9kYWwtZm9vdGVyXCI+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8YnV0dG9uIHR5cGU9XCJidXR0b25cIiBjbGFzc05hbWU9XCJidG4gYnRuLXByaW1hcnlcIiBkYXRhLWRpc21pc3M9XCJtb2RhbFwiIG9uQ2xpY2s9eygpID0+IHByb3BzLkFkZChheGlzKX0+QWRkPC9idXR0b24+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8YnV0dG9uIHR5cGU9XCJidXR0b25cIiBjbGFzc05hbWU9XCJidG4gYnRuLWRlZmF1bHRcIiBkYXRhLWRpc21pc3M9XCJtb2RhbFwiPkNhbmNlbDwvYnV0dG9uPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cclxuICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICA8L2Rpdj5cclxuXHJcbiAgICAgICAgPC8+XHJcbiAgICApO1xyXG59XHJcblxyXG5jb25zdCBBeGlzUm93ID0gKHByb3BzOiB7IEluZGV4OiBudW1iZXIsIEF4ZXM6IFRyZW5kaW5nY0RhdGFEaXNwbGF5LkZsb3RBeGlzW10sIFNldEF4ZXM6ICggYXhlczogVHJlbmRpbmdjRGF0YURpc3BsYXkuRmxvdEF4aXNbXSkgPT4gdm9pZH0pID0+IHtcclxuICAgIHJldHVybiAoXHJcbiAgICAgICAgPGxpIGNsYXNzTmFtZT0nbGlzdC1ncm91cC1pdGVtJyBrZXk9e3Byb3BzLkluZGV4fT5cclxuICAgICAgICAgICAgPGRpdj57cHJvcHMuQXhlc1twcm9wcy5JbmRleF0uYXhpc0xhYmVsfTwvZGl2PjxidXR0b24gdHlwZT1cImJ1dHRvblwiIHN0eWxlPXt7IHBvc2l0aW9uOiAncmVsYXRpdmUnLCB0b3A6IC0yMCB9fSBjbGFzc05hbWU9XCJjbG9zZVwiIG9uQ2xpY2s9eygpID0+IHtcclxuICAgICAgICAgICAgICAgIGxldCBhID0gWy4uLnByb3BzLkF4ZXNdO1xyXG4gICAgICAgICAgICAgICAgYS5zcGxpY2UocHJvcHMuSW5kZXgsIDEpO1xyXG4gICAgICAgICAgICAgICAgcHJvcHMuU2V0QXhlcyhhKVxyXG4gICAgICAgICAgICB9fT4mdGltZXM7PC9idXR0b24+XHJcbiAgICAgICAgICAgIDxkaXY+XHJcbiAgICAgICAgICAgICAgICA8c2VsZWN0IGNsYXNzTmFtZT0nZm9ybS1jb250cm9sJyB2YWx1ZT17cHJvcHMuQXhlc1twcm9wcy5JbmRleF0ucG9zaXRpb259IG9uQ2hhbmdlPXsoZXZ0KSA9PiB7XHJcbiAgICAgICAgICAgICAgICAgICAgbGV0IGEgPSBbLi4ucHJvcHMuQXhlc107XHJcbiAgICAgICAgICAgICAgICAgICAgYVtwcm9wcy5JbmRleF0ucG9zaXRpb24gPSBldnQudGFyZ2V0LnZhbHVlIGFzICdsZWZ0JyB8ICdyaWdodCc7XHJcbiAgICAgICAgICAgICAgICAgICAgcHJvcHMuU2V0QXhlcyhhKVxyXG4gICAgICAgICAgICAgICAgfX0+XHJcbiAgICAgICAgICAgICAgICAgICAgPG9wdGlvbiB2YWx1ZT0nbGVmdCc+bGVmdDwvb3B0aW9uPlxyXG4gICAgICAgICAgICAgICAgICAgIDxvcHRpb24gdmFsdWU9J3JpZ2h0Jz5yaWdodDwvb3B0aW9uPlxyXG4gICAgICAgICAgICAgICAgPC9zZWxlY3Q+XHJcbiAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT0ncm93Jz5cclxuICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPSdjb2wtbGctNic+XHJcbiAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9J2Zvcm0tZ3JvdXAnPlxyXG4gICAgICAgICAgICAgICAgICAgICAgICA8bGFiZWw+TWluPC9sYWJlbD5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPGlucHV0IGNsYXNzTmFtZT0nZm9ybS1jb250cm9sJyB0eXBlPVwibnVtYmVyXCIgdmFsdWU9e3Byb3BzLkF4ZXNbcHJvcHMuSW5kZXhdPy5taW4gPz8gJyd9IG9uQ2hhbmdlPXsoZXZ0KSA9PiB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBsZXQgYXhlcyA9IFsuLi5wcm9wcy5BeGVzXTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGlmIChldnQudGFyZ2V0LnZhbHVlID09ICcnKVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGRlbGV0ZSBheGVzW3Byb3BzLkluZGV4XS5taW47XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBlbHNlXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgYXhlc1twcm9wcy5JbmRleF0ubWluID0gcGFyc2VGbG9hdChldnQudGFyZ2V0LnZhbHVlKTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHByb3BzLlNldEF4ZXMoYXhlcylcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIH19IC8+IFxyXG4gICAgICAgICAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT0nY29sLWxnLTYnPlxyXG4gICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPSdmb3JtLWdyb3VwJz5cclxuICAgICAgICAgICAgICAgICAgICAgICAgPGxhYmVsPk1heDwvbGFiZWw+XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIDxpbnB1dCBjbGFzc05hbWU9J2Zvcm0tY29udHJvbCcgdHlwZT1cIm51bWJlclwiIHZhbHVlPXtwcm9wcy5BeGVzW3Byb3BzLkluZGV4XT8ubWF4ID8/ICcnfSBvbkNoYW5nZT17KGV2dCkgPT4ge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgbGV0IGF4ZXMgPSBbLi4ucHJvcHMuQXhlc107XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBpZiAoZXZ0LnRhcmdldC52YWx1ZSA9PSAnJylcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBkZWxldGUgYXhlc1twcm9wcy5JbmRleF0ubWF4O1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgZWxzZVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGF4ZXNbcHJvcHMuSW5kZXhdLm1heCA9IHBhcnNlRmxvYXQoZXZ0LnRhcmdldC52YWx1ZSk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBwcm9wcy5TZXRBeGVzKGF4ZXMpXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIH19IC8+XHJcbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgICAgICAgICA8L2Rpdj5cclxuXHJcbiAgICAgICAgICAgIDwvZGl2PlxyXG4gICAgICAgICAgICA8YnV0dG9uIGNsYXNzTmFtZT0nYnRuIGJ0bi1pbmZvJyBvbkNsaWNrPXsoKSA9PiB7XHJcbiAgICAgICAgICAgICAgICBsZXQgYXhlcyA9IFsuLi5wcm9wcy5BeGVzXTtcclxuICAgICAgICAgICAgICAgIGRlbGV0ZSBheGVzW3Byb3BzLkluZGV4XS5tYXg7XHJcbiAgICAgICAgICAgICAgICBkZWxldGUgYXhlc1twcm9wcy5JbmRleF0ubWluO1xyXG5cclxuICAgICAgICAgICAgICAgIHByb3BzLlNldEF4ZXMoYXhlcylcclxuXHJcbiAgICAgICAgICAgIH19ID5Vc2UgRGVmYXVsdDwvYnV0dG9uPlxyXG5cclxuICAgICAgICA8L2xpPlxyXG5cclxuKTtcclxufVxyXG5cclxuUmVhY3RET00ucmVuZGVyKDxUcmVuZGluZ0RhdGFEaXNwbGF5IC8+LCBkb2N1bWVudC5nZXRFbGVtZW50QnlJZCgnYm9keUNvbnRhaW5lcicpKTsiLCIvKlxyXG5BeGlzIExhYmVscyBQbHVnaW4gZm9yIGZsb3QuXHJcbmh0dHA6Ly9naXRodWIuY29tL21hcmtyY290ZS9mbG90LWF4aXNsYWJlbHNcclxuT3JpZ2luYWwgY29kZSBpcyBDb3B5cmlnaHQgKGMpIDIwMTAgWHVhbiBMdW8uXHJcbk9yaWdpbmFsIGNvZGUgd2FzIHJlbGVhc2VkIHVuZGVyIHRoZSBHUEx2MyBsaWNlbnNlIGJ5IFh1YW4gTHVvLCBTZXB0ZW1iZXIgMjAxMC5cclxuT3JpZ2luYWwgY29kZSB3YXMgcmVyZWxlYXNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UgYnkgWHVhbiBMdW8sIEFwcmlsIDIwMTIuXHJcblBlcm1pc3Npb24gaXMgaGVyZWJ5IGdyYW50ZWQsIGZyZWUgb2YgY2hhcmdlLCB0byBhbnkgcGVyc29uIG9idGFpbmluZ1xyXG5hIGNvcHkgb2YgdGhpcyBzb2Z0d2FyZSBhbmQgYXNzb2NpYXRlZCBkb2N1bWVudGF0aW9uIGZpbGVzICh0aGVcclxuXCJTb2Z0d2FyZVwiKSwgdG8gZGVhbCBpbiB0aGUgU29mdHdhcmUgd2l0aG91dCByZXN0cmljdGlvbiwgaW5jbHVkaW5nXHJcbndpdGhvdXQgbGltaXRhdGlvbiB0aGUgcmlnaHRzIHRvIHVzZSwgY29weSwgbW9kaWZ5LCBtZXJnZSwgcHVibGlzaCxcclxuZGlzdHJpYnV0ZSwgc3VibGljZW5zZSwgYW5kL29yIHNlbGwgY29waWVzIG9mIHRoZSBTb2Z0d2FyZSwgYW5kIHRvXHJcbnBlcm1pdCBwZXJzb25zIHRvIHdob20gdGhlIFNvZnR3YXJlIGlzIGZ1cm5pc2hlZCB0byBkbyBzbywgc3ViamVjdCB0b1xyXG50aGUgZm9sbG93aW5nIGNvbmRpdGlvbnM6XHJcblRoZSBhYm92ZSBjb3B5cmlnaHQgbm90aWNlIGFuZCB0aGlzIHBlcm1pc3Npb24gbm90aWNlIHNoYWxsIGJlXHJcbmluY2x1ZGVkIGluIGFsbCBjb3BpZXMgb3Igc3Vic3RhbnRpYWwgcG9ydGlvbnMgb2YgdGhlIFNvZnR3YXJlLlxyXG5USEUgU09GVFdBUkUgSVMgUFJPVklERUQgXCJBUyBJU1wiLCBXSVRIT1VUIFdBUlJBTlRZIE9GIEFOWSBLSU5ELFxyXG5FWFBSRVNTIE9SIElNUExJRUQsIElOQ0xVRElORyBCVVQgTk9UIExJTUlURUQgVE8gVEhFIFdBUlJBTlRJRVMgT0ZcclxuTUVSQ0hBTlRBQklMSVRZLCBGSVRORVNTIEZPUiBBIFBBUlRJQ1VMQVIgUFVSUE9TRSBBTkRcclxuTk9OSU5GUklOR0VNRU5ULiBJTiBOTyBFVkVOVCBTSEFMTCBUSEUgQVVUSE9SUyBPUiBDT1BZUklHSFQgSE9MREVSUyBCRVxyXG5MSUFCTEUgRk9SIEFOWSBDTEFJTSwgREFNQUdFUyBPUiBPVEhFUiBMSUFCSUxJVFksIFdIRVRIRVIgSU4gQU4gQUNUSU9OXHJcbk9GIENPTlRSQUNULCBUT1JUIE9SIE9USEVSV0lTRSwgQVJJU0lORyBGUk9NLCBPVVQgT0YgT1IgSU4gQ09OTkVDVElPTlxyXG5XSVRIIFRIRSBTT0ZUV0FSRSBPUiBUSEUgVVNFIE9SIE9USEVSIERFQUxJTkdTIElOIFRIRSBTT0ZUV0FSRS5cclxuICovXHJcblxyXG4oZnVuY3Rpb24gKCQpIHtcclxuICAgIHZhciBvcHRpb25zID0ge1xyXG4gICAgICBheGlzTGFiZWxzOiB7XHJcbiAgICAgICAgc2hvdzogdHJ1ZVxyXG4gICAgICB9XHJcbiAgICB9O1xyXG5cclxuICAgIGZ1bmN0aW9uIGNhbnZhc1N1cHBvcnRlZCgpIHtcclxuICAgICAgICByZXR1cm4gISFkb2N1bWVudC5jcmVhdGVFbGVtZW50KCdjYW52YXMnKS5nZXRDb250ZXh0O1xyXG4gICAgfVxyXG5cclxuICAgIGZ1bmN0aW9uIGNhbnZhc1RleHRTdXBwb3J0ZWQoKSB7XHJcbiAgICAgICAgaWYgKCFjYW52YXNTdXBwb3J0ZWQoKSkge1xyXG4gICAgICAgICAgICByZXR1cm4gZmFsc2U7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIHZhciBkdW1teV9jYW52YXMgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KCdjYW52YXMnKTtcclxuICAgICAgICB2YXIgY29udGV4dCA9IGR1bW15X2NhbnZhcy5nZXRDb250ZXh0KCcyZCcpO1xyXG4gICAgICAgIHJldHVybiB0eXBlb2YgY29udGV4dC5maWxsVGV4dCA9PSAnZnVuY3Rpb24nO1xyXG4gICAgfVxyXG5cclxuICAgIGZ1bmN0aW9uIGNzczNUcmFuc2l0aW9uU3VwcG9ydGVkKCkge1xyXG4gICAgICAgIHZhciBkaXYgPSBkb2N1bWVudC5jcmVhdGVFbGVtZW50KCdkaXYnKTtcclxuICAgICAgICByZXR1cm4gdHlwZW9mIGRpdi5zdHlsZS5Nb3pUcmFuc2l0aW9uICE9ICd1bmRlZmluZWQnICAgIC8vIEdlY2tvXHJcbiAgICAgICAgICAgIHx8IHR5cGVvZiBkaXYuc3R5bGUuT1RyYW5zaXRpb24gIT0gJ3VuZGVmaW5lZCcgICAgICAvLyBPcGVyYVxyXG4gICAgICAgICAgICB8fCB0eXBlb2YgZGl2LnN0eWxlLndlYmtpdFRyYW5zaXRpb24gIT0gJ3VuZGVmaW5lZCcgLy8gV2ViS2l0XHJcbiAgICAgICAgICAgIHx8IHR5cGVvZiBkaXYuc3R5bGUudHJhbnNpdGlvbiAhPSAndW5kZWZpbmVkJztcclxuICAgIH1cclxuXHJcblxyXG4gICAgZnVuY3Rpb24gQXhpc0xhYmVsKGF4aXNOYW1lLCBwb3NpdGlvbiwgcGFkZGluZywgcGxvdCwgb3B0cykge1xyXG4gICAgICAgIHRoaXMuYXhpc05hbWUgPSBheGlzTmFtZTtcclxuICAgICAgICB0aGlzLnBvc2l0aW9uID0gcG9zaXRpb247XHJcbiAgICAgICAgdGhpcy5wYWRkaW5nID0gcGFkZGluZztcclxuICAgICAgICB0aGlzLnBsb3QgPSBwbG90O1xyXG4gICAgICAgIHRoaXMub3B0cyA9IG9wdHM7XHJcbiAgICAgICAgdGhpcy53aWR0aCA9IDA7XHJcbiAgICAgICAgdGhpcy5oZWlnaHQgPSAwO1xyXG4gICAgfVxyXG5cclxuICAgIEF4aXNMYWJlbC5wcm90b3R5cGUuY2xlYW51cCA9IGZ1bmN0aW9uKCkge1xyXG4gICAgfTtcclxuXHJcblxyXG4gICAgQ2FudmFzQXhpc0xhYmVsLnByb3RvdHlwZSA9IG5ldyBBeGlzTGFiZWwoKTtcclxuICAgIENhbnZhc0F4aXNMYWJlbC5wcm90b3R5cGUuY29uc3RydWN0b3IgPSBDYW52YXNBeGlzTGFiZWw7XHJcbiAgICBmdW5jdGlvbiBDYW52YXNBeGlzTGFiZWwoYXhpc05hbWUsIHBvc2l0aW9uLCBwYWRkaW5nLCBwbG90LCBvcHRzKSB7XHJcbiAgICAgICAgQXhpc0xhYmVsLnByb3RvdHlwZS5jb25zdHJ1Y3Rvci5jYWxsKHRoaXMsIGF4aXNOYW1lLCBwb3NpdGlvbiwgcGFkZGluZyxcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcGxvdCwgb3B0cyk7XHJcbiAgICB9XHJcblxyXG4gICAgQ2FudmFzQXhpc0xhYmVsLnByb3RvdHlwZS5jYWxjdWxhdGVTaXplID0gZnVuY3Rpb24oKSB7XHJcbiAgICAgICAgaWYgKCF0aGlzLm9wdHMuYXhpc0xhYmVsRm9udFNpemVQaXhlbHMpXHJcbiAgICAgICAgICAgIHRoaXMub3B0cy5heGlzTGFiZWxGb250U2l6ZVBpeGVscyA9IDE0O1xyXG4gICAgICAgIGlmICghdGhpcy5vcHRzLmF4aXNMYWJlbEZvbnRGYW1pbHkpXHJcbiAgICAgICAgICAgIHRoaXMub3B0cy5heGlzTGFiZWxGb250RmFtaWx5ID0gJ3NhbnMtc2VyaWYnO1xyXG5cclxuICAgICAgICB2YXIgdGV4dFdpZHRoID0gdGhpcy5vcHRzLmF4aXNMYWJlbEZvbnRTaXplUGl4ZWxzICsgdGhpcy5wYWRkaW5nO1xyXG4gICAgICAgIHZhciB0ZXh0SGVpZ2h0ID0gdGhpcy5vcHRzLmF4aXNMYWJlbEZvbnRTaXplUGl4ZWxzICsgdGhpcy5wYWRkaW5nO1xyXG4gICAgICAgIGlmICh0aGlzLnBvc2l0aW9uID09ICdsZWZ0JyB8fCB0aGlzLnBvc2l0aW9uID09ICdyaWdodCcpIHtcclxuICAgICAgICAgICAgdGhpcy53aWR0aCA9IHRoaXMub3B0cy5heGlzTGFiZWxGb250U2l6ZVBpeGVscyArIHRoaXMucGFkZGluZztcclxuICAgICAgICAgICAgdGhpcy5oZWlnaHQgPSAwO1xyXG4gICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgIHRoaXMud2lkdGggPSAwO1xyXG4gICAgICAgICAgICB0aGlzLmhlaWdodCA9IHRoaXMub3B0cy5heGlzTGFiZWxGb250U2l6ZVBpeGVscyArIHRoaXMucGFkZGluZztcclxuICAgICAgICB9XHJcbiAgICB9O1xyXG5cclxuICAgIENhbnZhc0F4aXNMYWJlbC5wcm90b3R5cGUuZHJhdyA9IGZ1bmN0aW9uKGJveCkge1xyXG4gICAgICAgIGlmICghdGhpcy5vcHRzLmF4aXNMYWJlbENvbG91cilcclxuICAgICAgICAgICAgdGhpcy5vcHRzLmF4aXNMYWJlbENvbG91ciA9ICdibGFjayc7XHJcbiAgICAgICAgdmFyIGN0eCA9IHRoaXMucGxvdC5nZXRDYW52YXMoKS5nZXRDb250ZXh0KCcyZCcpO1xyXG4gICAgICAgIGN0eC5zYXZlKCk7XHJcbiAgICAgICAgY3R4LmZvbnQgPSB0aGlzLm9wdHMuYXhpc0xhYmVsRm9udFNpemVQaXhlbHMgKyAncHggJyArXHJcbiAgICAgICAgICAgIHRoaXMub3B0cy5heGlzTGFiZWxGb250RmFtaWx5O1xyXG4gICAgICAgIGN0eC5maWxsU3R5bGUgPSB0aGlzLm9wdHMuYXhpc0xhYmVsQ29sb3VyO1xyXG4gICAgICAgIHZhciB3aWR0aCA9IGN0eC5tZWFzdXJlVGV4dCh0aGlzLm9wdHMuYXhpc0xhYmVsKS53aWR0aDtcclxuICAgICAgICB2YXIgaGVpZ2h0ID0gdGhpcy5vcHRzLmF4aXNMYWJlbEZvbnRTaXplUGl4ZWxzO1xyXG4gICAgICAgIHZhciB4LCB5LCBhbmdsZSA9IDA7XHJcbiAgICAgICAgaWYgKHRoaXMucG9zaXRpb24gPT0gJ3RvcCcpIHtcclxuICAgICAgICAgICAgeCA9IGJveC5sZWZ0ICsgYm94LndpZHRoLzIgLSB3aWR0aC8yO1xyXG4gICAgICAgICAgICB5ID0gYm94LnRvcCArIGhlaWdodCowLjcyO1xyXG4gICAgICAgIH0gZWxzZSBpZiAodGhpcy5wb3NpdGlvbiA9PSAnYm90dG9tJykge1xyXG4gICAgICAgICAgICB4ID0gYm94LmxlZnQgKyBib3gud2lkdGgvMiAtIHdpZHRoLzI7XHJcbiAgICAgICAgICAgIHkgPSBib3gudG9wICsgYm94LmhlaWdodCAtIGhlaWdodCowLjcyO1xyXG4gICAgICAgIH0gZWxzZSBpZiAodGhpcy5wb3NpdGlvbiA9PSAnbGVmdCcpIHtcclxuICAgICAgICAgICAgeCA9IGJveC5sZWZ0ICsgaGVpZ2h0KjAuNzI7XHJcbiAgICAgICAgICAgIHkgPSBib3guaGVpZ2h0LzIgKyBib3gudG9wICsgd2lkdGgvMjtcclxuICAgICAgICAgICAgYW5nbGUgPSAtTWF0aC5QSS8yO1xyXG4gICAgICAgIH0gZWxzZSBpZiAodGhpcy5wb3NpdGlvbiA9PSAncmlnaHQnKSB7XHJcbiAgICAgICAgICAgIHggPSBib3gubGVmdCArIGJveC53aWR0aCAtIGhlaWdodCowLjcyO1xyXG4gICAgICAgICAgICB5ID0gYm94LmhlaWdodC8yICsgYm94LnRvcCAtIHdpZHRoLzI7XHJcbiAgICAgICAgICAgIGFuZ2xlID0gTWF0aC5QSS8yO1xyXG4gICAgICAgIH1cclxuICAgICAgICBjdHgudHJhbnNsYXRlKHgsIHkpO1xyXG4gICAgICAgIGN0eC5yb3RhdGUoYW5nbGUpO1xyXG4gICAgICAgIGN0eC5maWxsVGV4dCh0aGlzLm9wdHMuYXhpc0xhYmVsLCAwLCAwKTtcclxuICAgICAgICBjdHgucmVzdG9yZSgpO1xyXG4gICAgfTtcclxuXHJcblxyXG4gICAgSHRtbEF4aXNMYWJlbC5wcm90b3R5cGUgPSBuZXcgQXhpc0xhYmVsKCk7XHJcbiAgICBIdG1sQXhpc0xhYmVsLnByb3RvdHlwZS5jb25zdHJ1Y3RvciA9IEh0bWxBeGlzTGFiZWw7XHJcbiAgICBmdW5jdGlvbiBIdG1sQXhpc0xhYmVsKGF4aXNOYW1lLCBwb3NpdGlvbiwgcGFkZGluZywgcGxvdCwgb3B0cykge1xyXG4gICAgICAgIEF4aXNMYWJlbC5wcm90b3R5cGUuY29uc3RydWN0b3IuY2FsbCh0aGlzLCBheGlzTmFtZSwgcG9zaXRpb24sXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHBhZGRpbmcsIHBsb3QsIG9wdHMpO1xyXG4gICAgICAgIHRoaXMuZWxlbSA9IG51bGw7XHJcbiAgICB9XHJcblxyXG4gICAgSHRtbEF4aXNMYWJlbC5wcm90b3R5cGUuY2FsY3VsYXRlU2l6ZSA9IGZ1bmN0aW9uKCkge1xyXG4gICAgICAgIHZhciBlbGVtID0gJCgnPGRpdiBjbGFzcz1cImF4aXNMYWJlbHNcIiBzdHlsZT1cInBvc2l0aW9uOmFic29sdXRlO1wiPicgK1xyXG4gICAgICAgICAgICAgICAgICAgICB0aGlzLm9wdHMuYXhpc0xhYmVsICsgJzwvZGl2PicpO1xyXG4gICAgICAgIHRoaXMucGxvdC5nZXRQbGFjZWhvbGRlcigpLmFwcGVuZChlbGVtKTtcclxuICAgICAgICAvLyBzdG9yZSBoZWlnaHQgYW5kIHdpZHRoIG9mIGxhYmVsIGl0c2VsZiwgZm9yIHVzZSBpbiBkcmF3KClcclxuICAgICAgICB0aGlzLmxhYmVsV2lkdGggPSBlbGVtLm91dGVyV2lkdGgodHJ1ZSk7XHJcbiAgICAgICAgdGhpcy5sYWJlbEhlaWdodCA9IGVsZW0ub3V0ZXJIZWlnaHQodHJ1ZSk7XHJcbiAgICAgICAgZWxlbS5yZW1vdmUoKTtcclxuXHJcbiAgICAgICAgdGhpcy53aWR0aCA9IHRoaXMuaGVpZ2h0ID0gMDtcclxuICAgICAgICBpZiAodGhpcy5wb3NpdGlvbiA9PSAnbGVmdCcgfHwgdGhpcy5wb3NpdGlvbiA9PSAncmlnaHQnKSB7XHJcbiAgICAgICAgICAgIHRoaXMud2lkdGggPSB0aGlzLmxhYmVsV2lkdGggKyB0aGlzLnBhZGRpbmc7XHJcbiAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgdGhpcy5oZWlnaHQgPSB0aGlzLmxhYmVsSGVpZ2h0ICsgdGhpcy5wYWRkaW5nO1xyXG4gICAgICAgIH1cclxuICAgIH07XHJcblxyXG4gICAgSHRtbEF4aXNMYWJlbC5wcm90b3R5cGUuY2xlYW51cCA9IGZ1bmN0aW9uKCkge1xyXG4gICAgICAgIGlmICh0aGlzLmVsZW0pIHtcclxuICAgICAgICAgICAgdGhpcy5lbGVtLnJlbW92ZSgpO1xyXG4gICAgICAgIH1cclxuICAgIH07XHJcblxyXG4gICAgSHRtbEF4aXNMYWJlbC5wcm90b3R5cGUuZHJhdyA9IGZ1bmN0aW9uKGJveCkge1xyXG4gICAgICAgIHRoaXMucGxvdC5nZXRQbGFjZWhvbGRlcigpLmZpbmQoJyMnICsgdGhpcy5heGlzTmFtZSArICdMYWJlbCcpLnJlbW92ZSgpO1xyXG4gICAgICAgIHRoaXMuZWxlbSA9ICQoJzxkaXYgaWQ9XCInICsgdGhpcy5heGlzTmFtZSArXHJcbiAgICAgICAgICAgICAgICAgICAgICAnTGFiZWxcIiBcIiBjbGFzcz1cImF4aXNMYWJlbHNcIiBzdHlsZT1cInBvc2l0aW9uOmFic29sdXRlO1wiPidcclxuICAgICAgICAgICAgICAgICAgICAgICsgdGhpcy5vcHRzLmF4aXNMYWJlbCArICc8L2Rpdj4nKTtcclxuICAgICAgICB0aGlzLnBsb3QuZ2V0UGxhY2Vob2xkZXIoKS5hcHBlbmQodGhpcy5lbGVtKTtcclxuICAgICAgICBpZiAodGhpcy5wb3NpdGlvbiA9PSAndG9wJykge1xyXG4gICAgICAgICAgICB0aGlzLmVsZW0uY3NzKCdsZWZ0JywgYm94LmxlZnQgKyBib3gud2lkdGgvMiAtIHRoaXMubGFiZWxXaWR0aC8yICtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAncHgnKTtcclxuICAgICAgICAgICAgdGhpcy5lbGVtLmNzcygndG9wJywgYm94LnRvcCArICdweCcpO1xyXG4gICAgICAgIH0gZWxzZSBpZiAodGhpcy5wb3NpdGlvbiA9PSAnYm90dG9tJykge1xyXG4gICAgICAgICAgICB0aGlzLmVsZW0uY3NzKCdsZWZ0JywgYm94LmxlZnQgKyBib3gud2lkdGgvMiAtIHRoaXMubGFiZWxXaWR0aC8yICtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAncHgnKTtcclxuICAgICAgICAgICAgdGhpcy5lbGVtLmNzcygndG9wJywgYm94LnRvcCArIGJveC5oZWlnaHQgLSB0aGlzLmxhYmVsSGVpZ2h0ICtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAncHgnKTtcclxuICAgICAgICB9IGVsc2UgaWYgKHRoaXMucG9zaXRpb24gPT0gJ2xlZnQnKSB7XHJcbiAgICAgICAgICAgIHRoaXMuZWxlbS5jc3MoJ3RvcCcsIGJveC50b3AgKyBib3guaGVpZ2h0LzIgLSB0aGlzLmxhYmVsSGVpZ2h0LzIgK1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICdweCcpO1xyXG4gICAgICAgICAgICB0aGlzLmVsZW0uY3NzKCdsZWZ0JywgYm94LmxlZnQgKyAncHgnKTtcclxuICAgICAgICB9IGVsc2UgaWYgKHRoaXMucG9zaXRpb24gPT0gJ3JpZ2h0Jykge1xyXG4gICAgICAgICAgICB0aGlzLmVsZW0uY3NzKCd0b3AnLCBib3gudG9wICsgYm94LmhlaWdodC8yIC0gdGhpcy5sYWJlbEhlaWdodC8yICtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAncHgnKTtcclxuICAgICAgICAgICAgdGhpcy5lbGVtLmNzcygnbGVmdCcsIGJveC5sZWZ0ICsgYm94LndpZHRoIC0gdGhpcy5sYWJlbFdpZHRoICtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAncHgnKTtcclxuICAgICAgICB9XHJcbiAgICB9O1xyXG5cclxuXHJcbiAgICBDc3NUcmFuc2Zvcm1BeGlzTGFiZWwucHJvdG90eXBlID0gbmV3IEh0bWxBeGlzTGFiZWwoKTtcclxuICAgIENzc1RyYW5zZm9ybUF4aXNMYWJlbC5wcm90b3R5cGUuY29uc3RydWN0b3IgPSBDc3NUcmFuc2Zvcm1BeGlzTGFiZWw7XHJcbiAgICBmdW5jdGlvbiBDc3NUcmFuc2Zvcm1BeGlzTGFiZWwoYXhpc05hbWUsIHBvc2l0aW9uLCBwYWRkaW5nLCBwbG90LCBvcHRzKSB7XHJcbiAgICAgICAgSHRtbEF4aXNMYWJlbC5wcm90b3R5cGUuY29uc3RydWN0b3IuY2FsbCh0aGlzLCBheGlzTmFtZSwgcG9zaXRpb24sXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBwYWRkaW5nLCBwbG90LCBvcHRzKTtcclxuICAgIH1cclxuXHJcbiAgICBDc3NUcmFuc2Zvcm1BeGlzTGFiZWwucHJvdG90eXBlLmNhbGN1bGF0ZVNpemUgPSBmdW5jdGlvbigpIHtcclxuICAgICAgICBIdG1sQXhpc0xhYmVsLnByb3RvdHlwZS5jYWxjdWxhdGVTaXplLmNhbGwodGhpcyk7XHJcbiAgICAgICAgdGhpcy53aWR0aCA9IHRoaXMuaGVpZ2h0ID0gMDtcclxuICAgICAgICBpZiAodGhpcy5wb3NpdGlvbiA9PSAnbGVmdCcgfHwgdGhpcy5wb3NpdGlvbiA9PSAncmlnaHQnKSB7XHJcbiAgICAgICAgICAgIHRoaXMud2lkdGggPSB0aGlzLmxhYmVsSGVpZ2h0ICsgdGhpcy5wYWRkaW5nO1xyXG4gICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgIHRoaXMuaGVpZ2h0ID0gdGhpcy5sYWJlbEhlaWdodCArIHRoaXMucGFkZGluZztcclxuICAgICAgICB9XHJcbiAgICB9O1xyXG5cclxuICAgIENzc1RyYW5zZm9ybUF4aXNMYWJlbC5wcm90b3R5cGUudHJhbnNmb3JtcyA9IGZ1bmN0aW9uKGRlZ3JlZXMsIHgsIHkpIHtcclxuICAgICAgICB2YXIgc3RyYW5zZm9ybXMgPSB7XHJcbiAgICAgICAgICAgICctbW96LXRyYW5zZm9ybSc6ICcnLFxyXG4gICAgICAgICAgICAnLXdlYmtpdC10cmFuc2Zvcm0nOiAnJyxcclxuICAgICAgICAgICAgJy1vLXRyYW5zZm9ybSc6ICcnLFxyXG4gICAgICAgICAgICAnLW1zLXRyYW5zZm9ybSc6ICcnXHJcbiAgICAgICAgfTtcclxuICAgICAgICBpZiAoeCAhPSAwIHx8IHkgIT0gMCkge1xyXG4gICAgICAgICAgICB2YXIgc3RkVHJhbnNsYXRlID0gJyB0cmFuc2xhdGUoJyArIHggKyAncHgsICcgKyB5ICsgJ3B4KSc7XHJcbiAgICAgICAgICAgIHN0cmFuc2Zvcm1zWyctbW96LXRyYW5zZm9ybSddICs9IHN0ZFRyYW5zbGF0ZTtcclxuICAgICAgICAgICAgc3RyYW5zZm9ybXNbJy13ZWJraXQtdHJhbnNmb3JtJ10gKz0gc3RkVHJhbnNsYXRlO1xyXG4gICAgICAgICAgICBzdHJhbnNmb3Jtc1snLW8tdHJhbnNmb3JtJ10gKz0gc3RkVHJhbnNsYXRlO1xyXG4gICAgICAgICAgICBzdHJhbnNmb3Jtc1snLW1zLXRyYW5zZm9ybSddICs9IHN0ZFRyYW5zbGF0ZTtcclxuICAgICAgICB9XHJcbiAgICAgICAgaWYgKGRlZ3JlZXMgIT0gMCkge1xyXG4gICAgICAgICAgICB2YXIgcm90YXRpb24gPSBkZWdyZWVzIC8gOTA7XHJcbiAgICAgICAgICAgIHZhciBzdGRSb3RhdGUgPSAnIHJvdGF0ZSgnICsgZGVncmVlcyArICdkZWcpJztcclxuICAgICAgICAgICAgc3RyYW5zZm9ybXNbJy1tb3otdHJhbnNmb3JtJ10gKz0gc3RkUm90YXRlO1xyXG4gICAgICAgICAgICBzdHJhbnNmb3Jtc1snLXdlYmtpdC10cmFuc2Zvcm0nXSArPSBzdGRSb3RhdGU7XHJcbiAgICAgICAgICAgIHN0cmFuc2Zvcm1zWyctby10cmFuc2Zvcm0nXSArPSBzdGRSb3RhdGU7XHJcbiAgICAgICAgICAgIHN0cmFuc2Zvcm1zWyctbXMtdHJhbnNmb3JtJ10gKz0gc3RkUm90YXRlO1xyXG4gICAgICAgIH1cclxuICAgICAgICB2YXIgcyA9ICd0b3A6IDA7IGxlZnQ6IDA7ICc7XHJcbiAgICAgICAgZm9yICh2YXIgcHJvcCBpbiBzdHJhbnNmb3Jtcykge1xyXG4gICAgICAgICAgICBpZiAoc3RyYW5zZm9ybXNbcHJvcF0pIHtcclxuICAgICAgICAgICAgICAgIHMgKz0gcHJvcCArICc6JyArIHN0cmFuc2Zvcm1zW3Byb3BdICsgJzsnO1xyXG4gICAgICAgICAgICB9XHJcbiAgICAgICAgfVxyXG4gICAgICAgIHMgKz0gJzsnO1xyXG4gICAgICAgIHJldHVybiBzO1xyXG4gICAgfTtcclxuXHJcbiAgICBDc3NUcmFuc2Zvcm1BeGlzTGFiZWwucHJvdG90eXBlLmNhbGN1bGF0ZU9mZnNldHMgPSBmdW5jdGlvbihib3gpIHtcclxuICAgICAgICB2YXIgb2Zmc2V0cyA9IHsgeDogMCwgeTogMCwgZGVncmVlczogMCB9O1xyXG4gICAgICAgIGlmICh0aGlzLnBvc2l0aW9uID09ICdib3R0b20nKSB7XHJcbiAgICAgICAgICAgIG9mZnNldHMueCA9IGJveC5sZWZ0ICsgYm94LndpZHRoLzIgLSB0aGlzLmxhYmVsV2lkdGgvMjtcclxuICAgICAgICAgICAgb2Zmc2V0cy55ID0gYm94LnRvcCArIGJveC5oZWlnaHQgLSB0aGlzLmxhYmVsSGVpZ2h0O1xyXG4gICAgICAgIH0gZWxzZSBpZiAodGhpcy5wb3NpdGlvbiA9PSAndG9wJykge1xyXG4gICAgICAgICAgICBvZmZzZXRzLnggPSBib3gubGVmdCArIGJveC53aWR0aC8yIC0gdGhpcy5sYWJlbFdpZHRoLzI7XHJcbiAgICAgICAgICAgIG9mZnNldHMueSA9IGJveC50b3A7XHJcbiAgICAgICAgfSBlbHNlIGlmICh0aGlzLnBvc2l0aW9uID09ICdsZWZ0Jykge1xyXG4gICAgICAgICAgICBvZmZzZXRzLmRlZ3JlZXMgPSAtOTA7XHJcbiAgICAgICAgICAgIG9mZnNldHMueCA9IGJveC5sZWZ0IC0gdGhpcy5sYWJlbFdpZHRoLzIgKyB0aGlzLmxhYmVsSGVpZ2h0LzI7XHJcbiAgICAgICAgICAgIG9mZnNldHMueSA9IGJveC5oZWlnaHQvMiArIGJveC50b3A7XHJcbiAgICAgICAgfSBlbHNlIGlmICh0aGlzLnBvc2l0aW9uID09ICdyaWdodCcpIHtcclxuICAgICAgICAgICAgb2Zmc2V0cy5kZWdyZWVzID0gOTA7XHJcbiAgICAgICAgICAgIG9mZnNldHMueCA9IGJveC5sZWZ0ICsgYm94LndpZHRoIC0gdGhpcy5sYWJlbFdpZHRoLzJcclxuICAgICAgICAgICAgICAgICAgICAgICAgLSB0aGlzLmxhYmVsSGVpZ2h0LzI7XHJcbiAgICAgICAgICAgIG9mZnNldHMueSA9IGJveC5oZWlnaHQvMiArIGJveC50b3A7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIG9mZnNldHMueCA9IE1hdGgucm91bmQob2Zmc2V0cy54KTtcclxuICAgICAgICBvZmZzZXRzLnkgPSBNYXRoLnJvdW5kKG9mZnNldHMueSk7XHJcblxyXG4gICAgICAgIHJldHVybiBvZmZzZXRzO1xyXG4gICAgfTtcclxuXHJcbiAgICBDc3NUcmFuc2Zvcm1BeGlzTGFiZWwucHJvdG90eXBlLmRyYXcgPSBmdW5jdGlvbihib3gpIHtcclxuICAgICAgICB0aGlzLnBsb3QuZ2V0UGxhY2Vob2xkZXIoKS5maW5kKFwiLlwiICsgdGhpcy5heGlzTmFtZSArIFwiTGFiZWxcIikucmVtb3ZlKCk7XHJcbiAgICAgICAgdmFyIG9mZnNldHMgPSB0aGlzLmNhbGN1bGF0ZU9mZnNldHMoYm94KTtcclxuICAgICAgICB0aGlzLmVsZW0gPSAkKCc8ZGl2IGNsYXNzPVwiYXhpc0xhYmVscyAnICsgdGhpcy5heGlzTmFtZSArXHJcbiAgICAgICAgICAgICAgICAgICAgICAnTGFiZWxcIiBzdHlsZT1cInBvc2l0aW9uOmFic29sdXRlOyAnICtcclxuICAgICAgICAgICAgICAgICAgICAgIHRoaXMudHJhbnNmb3JtcyhvZmZzZXRzLmRlZ3JlZXMsIG9mZnNldHMueCwgb2Zmc2V0cy55KSArXHJcbiAgICAgICAgICAgICAgICAgICAgICAnXCI+JyArIHRoaXMub3B0cy5heGlzTGFiZWwgKyAnPC9kaXY+Jyk7XHJcbiAgICAgICAgdGhpcy5wbG90LmdldFBsYWNlaG9sZGVyKCkuYXBwZW5kKHRoaXMuZWxlbSk7XHJcbiAgICB9O1xyXG5cclxuXHJcbiAgICBJZVRyYW5zZm9ybUF4aXNMYWJlbC5wcm90b3R5cGUgPSBuZXcgQ3NzVHJhbnNmb3JtQXhpc0xhYmVsKCk7XHJcbiAgICBJZVRyYW5zZm9ybUF4aXNMYWJlbC5wcm90b3R5cGUuY29uc3RydWN0b3IgPSBJZVRyYW5zZm9ybUF4aXNMYWJlbDtcclxuICAgIGZ1bmN0aW9uIEllVHJhbnNmb3JtQXhpc0xhYmVsKGF4aXNOYW1lLCBwb3NpdGlvbiwgcGFkZGluZywgcGxvdCwgb3B0cykge1xyXG4gICAgICAgIENzc1RyYW5zZm9ybUF4aXNMYWJlbC5wcm90b3R5cGUuY29uc3RydWN0b3IuY2FsbCh0aGlzLCBheGlzTmFtZSxcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcG9zaXRpb24sIHBhZGRpbmcsXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHBsb3QsIG9wdHMpO1xyXG4gICAgICAgIHRoaXMucmVxdWlyZXNSZXNpemUgPSBmYWxzZTtcclxuICAgIH1cclxuXHJcbiAgICBJZVRyYW5zZm9ybUF4aXNMYWJlbC5wcm90b3R5cGUudHJhbnNmb3JtcyA9IGZ1bmN0aW9uKGRlZ3JlZXMsIHgsIHkpIHtcclxuICAgICAgICAvLyBJIGRpZG4ndCBmZWVsIGxpa2UgbGVhcm5pbmcgdGhlIGNyYXp5IE1hdHJpeCBzdHVmZiwgc28gdGhpcyB1c2VzXHJcbiAgICAgICAgLy8gYSBjb21iaW5hdGlvbiBvZiB0aGUgcm90YXRpb24gdHJhbnNmb3JtIGFuZCBDU1MgcG9zaXRpb25pbmcuXHJcbiAgICAgICAgdmFyIHMgPSAnJztcclxuICAgICAgICBpZiAoZGVncmVlcyAhPSAwKSB7XHJcbiAgICAgICAgICAgIHZhciByb3RhdGlvbiA9IGRlZ3JlZXMvOTA7XHJcbiAgICAgICAgICAgIHdoaWxlIChyb3RhdGlvbiA8IDApIHtcclxuICAgICAgICAgICAgICAgIHJvdGF0aW9uICs9IDQ7XHJcbiAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgcyArPSAnIGZpbHRlcjogcHJvZ2lkOkRYSW1hZ2VUcmFuc2Zvcm0uTWljcm9zb2Z0LkJhc2ljSW1hZ2Uocm90YXRpb249JyArIHJvdGF0aW9uICsgJyk7ICc7XHJcbiAgICAgICAgICAgIC8vIHNlZSBiZWxvd1xyXG4gICAgICAgICAgICB0aGlzLnJlcXVpcmVzUmVzaXplID0gKHRoaXMucG9zaXRpb24gPT0gJ3JpZ2h0Jyk7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIGlmICh4ICE9IDApIHtcclxuICAgICAgICAgICAgcyArPSAnbGVmdDogJyArIHggKyAncHg7ICc7XHJcbiAgICAgICAgfVxyXG4gICAgICAgIGlmICh5ICE9IDApIHtcclxuICAgICAgICAgICAgcyArPSAndG9wOiAnICsgeSArICdweDsgJztcclxuICAgICAgICB9XHJcbiAgICAgICAgcmV0dXJuIHM7XHJcbiAgICB9O1xyXG5cclxuICAgIEllVHJhbnNmb3JtQXhpc0xhYmVsLnByb3RvdHlwZS5jYWxjdWxhdGVPZmZzZXRzID0gZnVuY3Rpb24oYm94KSB7XHJcbiAgICAgICAgdmFyIG9mZnNldHMgPSBDc3NUcmFuc2Zvcm1BeGlzTGFiZWwucHJvdG90eXBlLmNhbGN1bGF0ZU9mZnNldHMuY2FsbChcclxuICAgICAgICAgICAgICAgICAgICAgICAgICB0aGlzLCBib3gpO1xyXG4gICAgICAgIC8vIGFkanVzdCBzb21lIHZhbHVlcyB0byB0YWtlIGludG8gYWNjb3VudCBkaWZmZXJlbmNlcyBiZXR3ZWVuXHJcbiAgICAgICAgLy8gQ1NTIGFuZCBJRSByb3RhdGlvbnMuXHJcbiAgICAgICAgaWYgKHRoaXMucG9zaXRpb24gPT0gJ3RvcCcpIHtcclxuICAgICAgICAgICAgLy8gRklYTUU6IG5vdCBzdXJlIHdoeSwgYnV0IHBsYWNpbmcgdGhpcyBleGFjdGx5IGF0IHRoZSB0b3AgY2F1c2VzXHJcbiAgICAgICAgICAgIC8vIHRoZSB0b3AgYXhpcyBsYWJlbCB0byBmbGlwIHRvIHRoZSBib3R0b20uLi5cclxuICAgICAgICAgICAgb2Zmc2V0cy55ID0gYm94LnRvcCArIDE7XHJcbiAgICAgICAgfSBlbHNlIGlmICh0aGlzLnBvc2l0aW9uID09ICdsZWZ0Jykge1xyXG4gICAgICAgICAgICBvZmZzZXRzLnggPSBib3gubGVmdDtcclxuICAgICAgICAgICAgb2Zmc2V0cy55ID0gYm94LmhlaWdodC8yICsgYm94LnRvcCAtIHRoaXMubGFiZWxXaWR0aC8yO1xyXG4gICAgICAgIH0gZWxzZSBpZiAodGhpcy5wb3NpdGlvbiA9PSAncmlnaHQnKSB7XHJcbiAgICAgICAgICAgIG9mZnNldHMueCA9IGJveC5sZWZ0ICsgYm94LndpZHRoIC0gdGhpcy5sYWJlbEhlaWdodDtcclxuICAgICAgICAgICAgb2Zmc2V0cy55ID0gYm94LmhlaWdodC8yICsgYm94LnRvcCAtIHRoaXMubGFiZWxXaWR0aC8yO1xyXG4gICAgICAgIH1cclxuICAgICAgICByZXR1cm4gb2Zmc2V0cztcclxuICAgIH07XHJcblxyXG4gICAgSWVUcmFuc2Zvcm1BeGlzTGFiZWwucHJvdG90eXBlLmRyYXcgPSBmdW5jdGlvbihib3gpIHtcclxuICAgICAgICBDc3NUcmFuc2Zvcm1BeGlzTGFiZWwucHJvdG90eXBlLmRyYXcuY2FsbCh0aGlzLCBib3gpO1xyXG4gICAgICAgIGlmICh0aGlzLnJlcXVpcmVzUmVzaXplKSB7XHJcbiAgICAgICAgICAgIHRoaXMuZWxlbSA9IHRoaXMucGxvdC5nZXRQbGFjZWhvbGRlcigpLmZpbmQoXCIuXCIgKyB0aGlzLmF4aXNOYW1lICtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBcIkxhYmVsXCIpO1xyXG4gICAgICAgICAgICAvLyBTaW5jZSB3ZSB1c2VkIENTUyBwb3NpdGlvbmluZyBpbnN0ZWFkIG9mIHRyYW5zZm9ybXMgZm9yXHJcbiAgICAgICAgICAgIC8vIHRyYW5zbGF0aW5nIHRoZSBlbGVtZW50LCBhbmQgc2luY2UgdGhlIHBvc2l0aW9uaW5nIGlzIGRvbmVcclxuICAgICAgICAgICAgLy8gYmVmb3JlIGFueSByb3RhdGlvbnMsIHdlIGhhdmUgdG8gcmVzZXQgdGhlIHdpZHRoIGFuZCBoZWlnaHRcclxuICAgICAgICAgICAgLy8gaW4gY2FzZSB0aGUgYnJvd3NlciB3cmFwcGVkIHRoZSB0ZXh0IChzcGVjaWZpY2FsbHkgZm9yIHRoZVxyXG4gICAgICAgICAgICAvLyB5MmF4aXMpLlxyXG4gICAgICAgICAgICB0aGlzLmVsZW0uY3NzKCd3aWR0aCcsIHRoaXMubGFiZWxXaWR0aCk7XHJcbiAgICAgICAgICAgIHRoaXMuZWxlbS5jc3MoJ2hlaWdodCcsIHRoaXMubGFiZWxIZWlnaHQpO1xyXG4gICAgICAgIH1cclxuICAgIH07XHJcblxyXG5cclxuICAgIGZ1bmN0aW9uIGluaXQocGxvdCkge1xyXG4gICAgICAgIHBsb3QuaG9va3MucHJvY2Vzc09wdGlvbnMucHVzaChmdW5jdGlvbiAocGxvdCwgb3B0aW9ucykge1xyXG5cclxuICAgICAgICAgICAgaWYgKCFvcHRpb25zLmF4aXNMYWJlbHMuc2hvdylcclxuICAgICAgICAgICAgICAgIHJldHVybjtcclxuXHJcbiAgICAgICAgICAgIC8vIFRoaXMgaXMga2luZCBvZiBhIGhhY2suIFRoZXJlIGFyZSBubyBob29rcyBpbiBGbG90IGJldHdlZW5cclxuICAgICAgICAgICAgLy8gdGhlIGNyZWF0aW9uIGFuZCBtZWFzdXJpbmcgb2YgdGhlIHRpY2tzIChzZXRUaWNrcywgbWVhc3VyZVRpY2tMYWJlbHNcclxuICAgICAgICAgICAgLy8gaW4gc2V0dXBHcmlkKCkgKSBhbmQgdGhlIGRyYXdpbmcgb2YgdGhlIHRpY2tzIGFuZCBwbG90IGJveFxyXG4gICAgICAgICAgICAvLyAoaW5zZXJ0QXhpc0xhYmVscyBpbiBzZXR1cEdyaWQoKSApLlxyXG4gICAgICAgICAgICAvL1xyXG4gICAgICAgICAgICAvLyBUaGVyZWZvcmUsIHdlIHVzZSBhIHRyaWNrIHdoZXJlIHdlIHJ1biB0aGUgZHJhdyByb3V0aW5lIHR3aWNlOlxyXG4gICAgICAgICAgICAvLyB0aGUgZmlyc3QgdGltZSB0byBnZXQgdGhlIHRpY2sgbWVhc3VyZW1lbnRzLCBzbyB0aGF0IHdlIGNhbiBjaGFuZ2VcclxuICAgICAgICAgICAgLy8gdGhlbSwgYW5kIHRoZW4gaGF2ZSBpdCBkcmF3IGl0IGFnYWluLlxyXG4gICAgICAgICAgICB2YXIgc2Vjb25kUGFzcyA9IGZhbHNlO1xyXG5cclxuICAgICAgICAgICAgdmFyIGF4aXNMYWJlbHMgPSB7fTtcclxuICAgICAgICAgICAgdmFyIGF4aXNPZmZzZXRDb3VudHMgPSB7IGxlZnQ6IDAsIHJpZ2h0OiAwLCB0b3A6IDAsIGJvdHRvbTogMCB9O1xyXG5cclxuICAgICAgICAgICAgdmFyIGRlZmF1bHRQYWRkaW5nID0gMjsgIC8vIHBhZGRpbmcgYmV0d2VlbiBheGlzIGFuZCB0aWNrIGxhYmVsc1xyXG4gICAgICAgICAgICBwbG90Lmhvb2tzLmRyYXcucHVzaChmdW5jdGlvbiAocGxvdCwgY3R4KSB7XHJcbiAgICAgICAgICAgICAgICB2YXIgaGFzQXhpc0xhYmVscyA9IGZhbHNlO1xyXG4gICAgICAgICAgICAgICAgaWYgKCFzZWNvbmRQYXNzKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgLy8gTUVBU1VSRSBBTkQgU0VUIE9QVElPTlNcclxuICAgICAgICAgICAgICAgICAgICAkLmVhY2gocGxvdC5nZXRBeGVzKCksIGZ1bmN0aW9uKGF4aXNOYW1lLCBheGlzKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHZhciBvcHRzID0gYXhpcy5vcHRpb25zIC8vIEZsb3QgMC43XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB8fCBwbG90LmdldE9wdGlvbnMoKVtheGlzTmFtZV07IC8vIEZsb3QgMC42XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICAvLyBIYW5kbGUgcmVkcmF3cyBpbml0aWF0ZWQgb3V0c2lkZSBvZiB0aGlzIHBsdWctaW4uXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGlmIChheGlzTmFtZSBpbiBheGlzTGFiZWxzKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBheGlzLmxhYmVsSGVpZ2h0ID0gYXhpcy5sYWJlbEhlaWdodCAtXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgYXhpc0xhYmVsc1theGlzTmFtZV0uaGVpZ2h0O1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgYXhpcy5sYWJlbFdpZHRoID0gYXhpcy5sYWJlbFdpZHRoIC1cclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBheGlzTGFiZWxzW2F4aXNOYW1lXS53aWR0aDtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIG9wdHMubGFiZWxIZWlnaHQgPSBheGlzLmxhYmVsSGVpZ2h0O1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgb3B0cy5sYWJlbFdpZHRoID0gYXhpcy5sYWJlbFdpZHRoO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgYXhpc0xhYmVsc1theGlzTmFtZV0uY2xlYW51cCgpO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgZGVsZXRlIGF4aXNMYWJlbHNbYXhpc05hbWVdO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICB9XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICBpZiAoIW9wdHMgfHwgIW9wdHMuYXhpc0xhYmVsIHx8ICFheGlzLnNob3cpXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICByZXR1cm47XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICBoYXNBeGlzTGFiZWxzID0gdHJ1ZTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgdmFyIHJlbmRlcmVyID0gbnVsbDtcclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGlmICghb3B0cy5heGlzTGFiZWxVc2VIdG1sICYmXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBuYXZpZ2F0b3IuYXBwTmFtZSA9PSAnTWljcm9zb2Z0IEludGVybmV0IEV4cGxvcmVyJykge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgdmFyIHVhID0gbmF2aWdhdG9yLnVzZXJBZ2VudDtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHZhciByZSAgPSBuZXcgUmVnRXhwKFwiTVNJRSAoWzAtOV17MSx9W1xcLjAtOV17MCx9KVwiKTtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGlmIChyZS5leGVjKHVhKSAhPSBudWxsKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcnYgPSBwYXJzZUZsb2F0KFJlZ0V4cC4kMSk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBpZiAocnYgPj0gOSAmJiAhb3B0cy5heGlzTGFiZWxVc2VDYW52YXMgJiYgIW9wdHMuYXhpc0xhYmVsVXNlSHRtbCkge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHJlbmRlcmVyID0gQ3NzVHJhbnNmb3JtQXhpc0xhYmVsO1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgfSBlbHNlIGlmICghb3B0cy5heGlzTGFiZWxVc2VDYW52YXMgJiYgIW9wdHMuYXhpc0xhYmVsVXNlSHRtbCkge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHJlbmRlcmVyID0gSWVUcmFuc2Zvcm1BeGlzTGFiZWw7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB9IGVsc2UgaWYgKG9wdHMuYXhpc0xhYmVsVXNlQ2FudmFzKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcmVuZGVyZXIgPSBDYW52YXNBeGlzTGFiZWw7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB9IGVsc2Uge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHJlbmRlcmVyID0gSHRtbEF4aXNMYWJlbDtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgICAgICAgICAgfSBlbHNlIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGlmIChvcHRzLmF4aXNMYWJlbFVzZUh0bWwgfHwgKCFjc3MzVHJhbnNpdGlvblN1cHBvcnRlZCgpICYmICFjYW52YXNUZXh0U3VwcG9ydGVkKCkpICYmICFvcHRzLmF4aXNMYWJlbFVzZUNhbnZhcykge1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHJlbmRlcmVyID0gSHRtbEF4aXNMYWJlbDtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIH0gZWxzZSBpZiAob3B0cy5heGlzTGFiZWxVc2VDYW52YXMgfHwgIWNzczNUcmFuc2l0aW9uU3VwcG9ydGVkKCkpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICByZW5kZXJlciA9IENhbnZhc0F4aXNMYWJlbDtcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcmVuZGVyZXIgPSBDc3NUcmFuc2Zvcm1BeGlzTGFiZWw7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIH1cclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHZhciBwYWRkaW5nID0gb3B0cy5heGlzTGFiZWxQYWRkaW5nID09PSB1bmRlZmluZWQgP1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGRlZmF1bHRQYWRkaW5nIDogb3B0cy5heGlzTGFiZWxQYWRkaW5nO1xyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgYXhpc0xhYmVsc1theGlzTmFtZV0gPSBuZXcgcmVuZGVyZXIoYXhpc05hbWUsXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGF4aXMucG9zaXRpb24sIHBhZGRpbmcsXHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHBsb3QsIG9wdHMpO1xyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgLy8gZmxvdCBpbnRlcnByZXRzIGF4aXMubGFiZWxIZWlnaHQgYW5kIC5sYWJlbFdpZHRoIGFzXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIC8vIHRoZSBoZWlnaHQgYW5kIHdpZHRoIG9mIHRoZSB0aWNrIGxhYmVscy4gV2UgaW5jcmVhc2VcclxuICAgICAgICAgICAgICAgICAgICAgICAgLy8gdGhlc2UgdmFsdWVzIHRvIG1ha2Ugcm9vbSBmb3IgdGhlIGF4aXMgbGFiZWwgYW5kXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIC8vIHBhZGRpbmcuXHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICBheGlzTGFiZWxzW2F4aXNOYW1lXS5jYWxjdWxhdGVTaXplKCk7XHJcblxyXG4gICAgICAgICAgICAgICAgICAgICAgICAvLyBBeGlzTGFiZWwuaGVpZ2h0IGFuZCAud2lkdGggYXJlIHRoZSBzaXplIG9mIHRoZVxyXG4gICAgICAgICAgICAgICAgICAgICAgICAvLyBheGlzIGxhYmVsIGFuZCBwYWRkaW5nLlxyXG4gICAgICAgICAgICAgICAgICAgICAgICAvLyBKdXN0IHNldCBvcHRzIGhlcmUgYmVjYXVzZSBheGlzIHdpbGwgYmUgc29ydGVkIG91dCBvblxyXG4gICAgICAgICAgICAgICAgICAgICAgICAvLyB0aGUgcmVkcmF3LlxyXG5cclxuICAgICAgICAgICAgICAgICAgICAgICAgb3B0cy5sYWJlbEhlaWdodCA9IGF4aXMubGFiZWxIZWlnaHQgK1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgYXhpc0xhYmVsc1theGlzTmFtZV0uaGVpZ2h0O1xyXG4gICAgICAgICAgICAgICAgICAgICAgICBvcHRzLmxhYmVsV2lkdGggPSBheGlzLmxhYmVsV2lkdGggK1xyXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgYXhpc0xhYmVsc1theGlzTmFtZV0ud2lkdGg7XHJcbiAgICAgICAgICAgICAgICAgICAgfSk7XHJcblxyXG4gICAgICAgICAgICAgICAgICAgIC8vIElmIHRoZXJlIGFyZSBheGlzIGxhYmVscywgcmUtZHJhdyB3aXRoIG5ldyBsYWJlbCB3aWR0aHMgYW5kXHJcbiAgICAgICAgICAgICAgICAgICAgLy8gaGVpZ2h0cy5cclxuXHJcbiAgICAgICAgICAgICAgICAgICAgaWYgKGhhc0F4aXNMYWJlbHMpIHtcclxuICAgICAgICAgICAgICAgICAgICAgICAgc2Vjb25kUGFzcyA9IHRydWU7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHBsb3Quc2V0dXBHcmlkKCk7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHBsb3QuZHJhdygpO1xyXG4gICAgICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgIH0gZWxzZSB7XHJcbiAgICAgICAgICAgICAgICAgICAgc2Vjb25kUGFzcyA9IGZhbHNlO1xyXG4gICAgICAgICAgICAgICAgICAgIC8vIERSQVdcclxuICAgICAgICAgICAgICAgICAgICAkLmVhY2gocGxvdC5nZXRBeGVzKCksIGZ1bmN0aW9uKGF4aXNOYW1lLCBheGlzKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIHZhciBvcHRzID0gYXhpcy5vcHRpb25zIC8vIEZsb3QgMC43XHJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB8fCBwbG90LmdldE9wdGlvbnMoKVtheGlzTmFtZV07IC8vIEZsb3QgMC42XHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGlmICghb3B0cyB8fCAhb3B0cy5heGlzTGFiZWwgfHwgIWF4aXMuc2hvdylcclxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHJldHVybjtcclxuXHJcbiAgICAgICAgICAgICAgICAgICAgICAgIGF4aXNMYWJlbHNbYXhpc05hbWVdLmRyYXcoYXhpcy5ib3gpO1xyXG4gICAgICAgICAgICAgICAgICAgIH0pO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICB9KTtcclxuICAgICAgICB9KTtcclxuICAgIH1cclxuXHJcblxyXG4gICAgJC5wbG90LnBsdWdpbnMucHVzaCh7XHJcbiAgICAgICAgaW5pdDogaW5pdCxcclxuICAgICAgICBvcHRpb25zOiBvcHRpb25zLFxyXG4gICAgICAgIG5hbWU6ICdheGlzTGFiZWxzJyxcclxuICAgICAgICB2ZXJzaW9uOiAnMi4wJ1xyXG4gICAgfSk7XHJcbn0pKGpRdWVyeSk7IiwiLyogSmF2YXNjcmlwdCBwbG90dGluZyBsaWJyYXJ5IGZvciBqUXVlcnksIHZlcnNpb24gMC44LjMuXHJcblxyXG5Db3B5cmlnaHQgKGMpIDIwMDctMjAxNCBJT0xBIGFuZCBPbGUgTGF1cnNlbi5cclxuTGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlLlxyXG5cclxuKi9cclxuKGZ1bmN0aW9uKCQpe3ZhciBvcHRpb25zPXtjcm9zc2hhaXI6e21vZGU6bnVsbCxjb2xvcjpcInJnYmEoMTcwLCAwLCAwLCAwLjgwKVwiLGxpbmVXaWR0aDoxfX07ZnVuY3Rpb24gaW5pdChwbG90KXt2YXIgY3Jvc3NoYWlyPXt4Oi0xLHk6LTEsbG9ja2VkOmZhbHNlfTtwbG90LnNldENyb3NzaGFpcj1mdW5jdGlvbiBzZXRDcm9zc2hhaXIocG9zKXtpZighcG9zKWNyb3NzaGFpci54PS0xO2Vsc2V7dmFyIG89cGxvdC5wMmMocG9zKTtjcm9zc2hhaXIueD1NYXRoLm1heCgwLE1hdGgubWluKG8ubGVmdCxwbG90LndpZHRoKCkpKTtjcm9zc2hhaXIueT1NYXRoLm1heCgwLE1hdGgubWluKG8udG9wLHBsb3QuaGVpZ2h0KCkpKX1wbG90LnRyaWdnZXJSZWRyYXdPdmVybGF5KCl9O3Bsb3QuY2xlYXJDcm9zc2hhaXI9cGxvdC5zZXRDcm9zc2hhaXI7cGxvdC5sb2NrQ3Jvc3NoYWlyPWZ1bmN0aW9uIGxvY2tDcm9zc2hhaXIocG9zKXtpZihwb3MpcGxvdC5zZXRDcm9zc2hhaXIocG9zKTtjcm9zc2hhaXIubG9ja2VkPXRydWV9O3Bsb3QudW5sb2NrQ3Jvc3NoYWlyPWZ1bmN0aW9uIHVubG9ja0Nyb3NzaGFpcigpe2Nyb3NzaGFpci5sb2NrZWQ9ZmFsc2V9O2Z1bmN0aW9uIG9uTW91c2VPdXQoZSl7aWYoY3Jvc3NoYWlyLmxvY2tlZClyZXR1cm47aWYoY3Jvc3NoYWlyLnghPS0xKXtjcm9zc2hhaXIueD0tMTtwbG90LnRyaWdnZXJSZWRyYXdPdmVybGF5KCl9fWZ1bmN0aW9uIG9uTW91c2VNb3ZlKGUpe2lmKGNyb3NzaGFpci5sb2NrZWQpcmV0dXJuO2lmKHBsb3QuZ2V0U2VsZWN0aW9uJiZwbG90LmdldFNlbGVjdGlvbigpKXtjcm9zc2hhaXIueD0tMTtyZXR1cm59dmFyIG9mZnNldD1wbG90Lm9mZnNldCgpO2Nyb3NzaGFpci54PU1hdGgubWF4KDAsTWF0aC5taW4oZS5wYWdlWC1vZmZzZXQubGVmdCxwbG90LndpZHRoKCkpKTtjcm9zc2hhaXIueT1NYXRoLm1heCgwLE1hdGgubWluKGUucGFnZVktb2Zmc2V0LnRvcCxwbG90LmhlaWdodCgpKSk7cGxvdC50cmlnZ2VyUmVkcmF3T3ZlcmxheSgpfXBsb3QuaG9va3MuYmluZEV2ZW50cy5wdXNoKGZ1bmN0aW9uKHBsb3QsZXZlbnRIb2xkZXIpe2lmKCFwbG90LmdldE9wdGlvbnMoKS5jcm9zc2hhaXIubW9kZSlyZXR1cm47ZXZlbnRIb2xkZXIubW91c2VvdXQob25Nb3VzZU91dCk7ZXZlbnRIb2xkZXIubW91c2Vtb3ZlKG9uTW91c2VNb3ZlKX0pO3Bsb3QuaG9va3MuZHJhd092ZXJsYXkucHVzaChmdW5jdGlvbihwbG90LGN0eCl7dmFyIGM9cGxvdC5nZXRPcHRpb25zKCkuY3Jvc3NoYWlyO2lmKCFjLm1vZGUpcmV0dXJuO3ZhciBwbG90T2Zmc2V0PXBsb3QuZ2V0UGxvdE9mZnNldCgpO2N0eC5zYXZlKCk7Y3R4LnRyYW5zbGF0ZShwbG90T2Zmc2V0LmxlZnQscGxvdE9mZnNldC50b3ApO2lmKGNyb3NzaGFpci54IT0tMSl7dmFyIGFkaj1wbG90LmdldE9wdGlvbnMoKS5jcm9zc2hhaXIubGluZVdpZHRoJTI/LjU6MDtjdHguc3Ryb2tlU3R5bGU9Yy5jb2xvcjtjdHgubGluZVdpZHRoPWMubGluZVdpZHRoO2N0eC5saW5lSm9pbj1cInJvdW5kXCI7Y3R4LmJlZ2luUGF0aCgpO2lmKGMubW9kZS5pbmRleE9mKFwieFwiKSE9LTEpe3ZhciBkcmF3WD1NYXRoLmZsb29yKGNyb3NzaGFpci54KSthZGo7Y3R4Lm1vdmVUbyhkcmF3WCwwKTtjdHgubGluZVRvKGRyYXdYLHBsb3QuaGVpZ2h0KCkpfWlmKGMubW9kZS5pbmRleE9mKFwieVwiKSE9LTEpe3ZhciBkcmF3WT1NYXRoLmZsb29yKGNyb3NzaGFpci55KSthZGo7Y3R4Lm1vdmVUbygwLGRyYXdZKTtjdHgubGluZVRvKHBsb3Qud2lkdGgoKSxkcmF3WSl9Y3R4LnN0cm9rZSgpfWN0eC5yZXN0b3JlKCl9KTtwbG90Lmhvb2tzLnNodXRkb3duLnB1c2goZnVuY3Rpb24ocGxvdCxldmVudEhvbGRlcil7ZXZlbnRIb2xkZXIudW5iaW5kKFwibW91c2VvdXRcIixvbk1vdXNlT3V0KTtldmVudEhvbGRlci51bmJpbmQoXCJtb3VzZW1vdmVcIixvbk1vdXNlTW92ZSl9KX0kLnBsb3QucGx1Z2lucy5wdXNoKHtpbml0OmluaXQsb3B0aW9uczpvcHRpb25zLG5hbWU6XCJjcm9zc2hhaXJcIix2ZXJzaW9uOlwiMS4wXCJ9KX0pKGpRdWVyeSk7IiwiLyogSmF2YXNjcmlwdCBwbG90dGluZyBsaWJyYXJ5IGZvciBqUXVlcnksIHZlcnNpb24gMC44LjMuXHJcblxyXG5Db3B5cmlnaHQgKGMpIDIwMDctMjAxNCBJT0xBIGFuZCBPbGUgTGF1cnNlbi5cclxuTGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlLlxyXG5cclxuKi9cclxuKGZ1bmN0aW9uKCQpeyQuY29sb3I9e307JC5jb2xvci5tYWtlPWZ1bmN0aW9uKHIsZyxiLGEpe3ZhciBvPXt9O28ucj1yfHwwO28uZz1nfHwwO28uYj1ifHwwO28uYT1hIT1udWxsP2E6MTtvLmFkZD1mdW5jdGlvbihjLGQpe2Zvcih2YXIgaT0wO2k8Yy5sZW5ndGg7KytpKW9bYy5jaGFyQXQoaSldKz1kO3JldHVybiBvLm5vcm1hbGl6ZSgpfTtvLnNjYWxlPWZ1bmN0aW9uKGMsZil7Zm9yKHZhciBpPTA7aTxjLmxlbmd0aDsrK2kpb1tjLmNoYXJBdChpKV0qPWY7cmV0dXJuIG8ubm9ybWFsaXplKCl9O28udG9TdHJpbmc9ZnVuY3Rpb24oKXtpZihvLmE+PTEpe3JldHVyblwicmdiKFwiK1tvLnIsby5nLG8uYl0uam9pbihcIixcIikrXCIpXCJ9ZWxzZXtyZXR1cm5cInJnYmEoXCIrW28ucixvLmcsby5iLG8uYV0uam9pbihcIixcIikrXCIpXCJ9fTtvLm5vcm1hbGl6ZT1mdW5jdGlvbigpe2Z1bmN0aW9uIGNsYW1wKG1pbix2YWx1ZSxtYXgpe3JldHVybiB2YWx1ZTxtaW4/bWluOnZhbHVlPm1heD9tYXg6dmFsdWV9by5yPWNsYW1wKDAscGFyc2VJbnQoby5yKSwyNTUpO28uZz1jbGFtcCgwLHBhcnNlSW50KG8uZyksMjU1KTtvLmI9Y2xhbXAoMCxwYXJzZUludChvLmIpLDI1NSk7by5hPWNsYW1wKDAsby5hLDEpO3JldHVybiBvfTtvLmNsb25lPWZ1bmN0aW9uKCl7cmV0dXJuICQuY29sb3IubWFrZShvLnIsby5iLG8uZyxvLmEpfTtyZXR1cm4gby5ub3JtYWxpemUoKX07JC5jb2xvci5leHRyYWN0PWZ1bmN0aW9uKGVsZW0sY3NzKXt2YXIgYztkb3tjPWVsZW0uY3NzKGNzcykudG9Mb3dlckNhc2UoKTtpZihjIT1cIlwiJiZjIT1cInRyYW5zcGFyZW50XCIpYnJlYWs7ZWxlbT1lbGVtLnBhcmVudCgpfXdoaWxlKGVsZW0ubGVuZ3RoJiYhJC5ub2RlTmFtZShlbGVtLmdldCgwKSxcImJvZHlcIikpO2lmKGM9PVwicmdiYSgwLCAwLCAwLCAwKVwiKWM9XCJ0cmFuc3BhcmVudFwiO3JldHVybiAkLmNvbG9yLnBhcnNlKGMpfTskLmNvbG9yLnBhcnNlPWZ1bmN0aW9uKHN0cil7dmFyIHJlcyxtPSQuY29sb3IubWFrZTtpZihyZXM9L3JnYlxcKFxccyooWzAtOV17MSwzfSlcXHMqLFxccyooWzAtOV17MSwzfSlcXHMqLFxccyooWzAtOV17MSwzfSlcXHMqXFwpLy5leGVjKHN0cikpcmV0dXJuIG0ocGFyc2VJbnQocmVzWzFdLDEwKSxwYXJzZUludChyZXNbMl0sMTApLHBhcnNlSW50KHJlc1szXSwxMCkpO2lmKHJlcz0vcmdiYVxcKFxccyooWzAtOV17MSwzfSlcXHMqLFxccyooWzAtOV17MSwzfSlcXHMqLFxccyooWzAtOV17MSwzfSlcXHMqLFxccyooWzAtOV0rKD86XFwuWzAtOV0rKT8pXFxzKlxcKS8uZXhlYyhzdHIpKXJldHVybiBtKHBhcnNlSW50KHJlc1sxXSwxMCkscGFyc2VJbnQocmVzWzJdLDEwKSxwYXJzZUludChyZXNbM10sMTApLHBhcnNlRmxvYXQocmVzWzRdKSk7aWYocmVzPS9yZ2JcXChcXHMqKFswLTldKyg/OlxcLlswLTldKyk/KVxcJVxccyosXFxzKihbMC05XSsoPzpcXC5bMC05XSspPylcXCVcXHMqLFxccyooWzAtOV0rKD86XFwuWzAtOV0rKT8pXFwlXFxzKlxcKS8uZXhlYyhzdHIpKXJldHVybiBtKHBhcnNlRmxvYXQocmVzWzFdKSoyLjU1LHBhcnNlRmxvYXQocmVzWzJdKSoyLjU1LHBhcnNlRmxvYXQocmVzWzNdKSoyLjU1KTtpZihyZXM9L3JnYmFcXChcXHMqKFswLTldKyg/OlxcLlswLTldKyk/KVxcJVxccyosXFxzKihbMC05XSsoPzpcXC5bMC05XSspPylcXCVcXHMqLFxccyooWzAtOV0rKD86XFwuWzAtOV0rKT8pXFwlXFxzKixcXHMqKFswLTldKyg/OlxcLlswLTldKyk/KVxccypcXCkvLmV4ZWMoc3RyKSlyZXR1cm4gbShwYXJzZUZsb2F0KHJlc1sxXSkqMi41NSxwYXJzZUZsb2F0KHJlc1syXSkqMi41NSxwYXJzZUZsb2F0KHJlc1szXSkqMi41NSxwYXJzZUZsb2F0KHJlc1s0XSkpO2lmKHJlcz0vIyhbYS1mQS1GMC05XXsyfSkoW2EtZkEtRjAtOV17Mn0pKFthLWZBLUYwLTldezJ9KS8uZXhlYyhzdHIpKXJldHVybiBtKHBhcnNlSW50KHJlc1sxXSwxNikscGFyc2VJbnQocmVzWzJdLDE2KSxwYXJzZUludChyZXNbM10sMTYpKTtpZihyZXM9LyMoW2EtZkEtRjAtOV0pKFthLWZBLUYwLTldKShbYS1mQS1GMC05XSkvLmV4ZWMoc3RyKSlyZXR1cm4gbShwYXJzZUludChyZXNbMV0rcmVzWzFdLDE2KSxwYXJzZUludChyZXNbMl0rcmVzWzJdLDE2KSxwYXJzZUludChyZXNbM10rcmVzWzNdLDE2KSk7dmFyIG5hbWU9JC50cmltKHN0cikudG9Mb3dlckNhc2UoKTtpZihuYW1lPT1cInRyYW5zcGFyZW50XCIpcmV0dXJuIG0oMjU1LDI1NSwyNTUsMCk7ZWxzZXtyZXM9bG9va3VwQ29sb3JzW25hbWVdfHxbMCwwLDBdO3JldHVybiBtKHJlc1swXSxyZXNbMV0scmVzWzJdKX19O3ZhciBsb29rdXBDb2xvcnM9e2FxdWE6WzAsMjU1LDI1NV0sYXp1cmU6WzI0MCwyNTUsMjU1XSxiZWlnZTpbMjQ1LDI0NSwyMjBdLGJsYWNrOlswLDAsMF0sYmx1ZTpbMCwwLDI1NV0sYnJvd246WzE2NSw0Miw0Ml0sY3lhbjpbMCwyNTUsMjU1XSxkYXJrYmx1ZTpbMCwwLDEzOV0sZGFya2N5YW46WzAsMTM5LDEzOV0sZGFya2dyZXk6WzE2OSwxNjksMTY5XSxkYXJrZ3JlZW46WzAsMTAwLDBdLGRhcmtraGFraTpbMTg5LDE4MywxMDddLGRhcmttYWdlbnRhOlsxMzksMCwxMzldLGRhcmtvbGl2ZWdyZWVuOls4NSwxMDcsNDddLGRhcmtvcmFuZ2U6WzI1NSwxNDAsMF0sZGFya29yY2hpZDpbMTUzLDUwLDIwNF0sZGFya3JlZDpbMTM5LDAsMF0sZGFya3NhbG1vbjpbMjMzLDE1MCwxMjJdLGRhcmt2aW9sZXQ6WzE0OCwwLDIxMV0sZnVjaHNpYTpbMjU1LDAsMjU1XSxnb2xkOlsyNTUsMjE1LDBdLGdyZWVuOlswLDEyOCwwXSxpbmRpZ286Wzc1LDAsMTMwXSxraGFraTpbMjQwLDIzMCwxNDBdLGxpZ2h0Ymx1ZTpbMTczLDIxNiwyMzBdLGxpZ2h0Y3lhbjpbMjI0LDI1NSwyNTVdLGxpZ2h0Z3JlZW46WzE0NCwyMzgsMTQ0XSxsaWdodGdyZXk6WzIxMSwyMTEsMjExXSxsaWdodHBpbms6WzI1NSwxODIsMTkzXSxsaWdodHllbGxvdzpbMjU1LDI1NSwyMjRdLGxpbWU6WzAsMjU1LDBdLG1hZ2VudGE6WzI1NSwwLDI1NV0sbWFyb29uOlsxMjgsMCwwXSxuYXZ5OlswLDAsMTI4XSxvbGl2ZTpbMTI4LDEyOCwwXSxvcmFuZ2U6WzI1NSwxNjUsMF0scGluazpbMjU1LDE5MiwyMDNdLHB1cnBsZTpbMTI4LDAsMTI4XSx2aW9sZXQ6WzEyOCwwLDEyOF0scmVkOlsyNTUsMCwwXSxzaWx2ZXI6WzE5MiwxOTIsMTkyXSx3aGl0ZTpbMjU1LDI1NSwyNTVdLHllbGxvdzpbMjU1LDI1NSwwXX19KShqUXVlcnkpOyhmdW5jdGlvbigkKXt2YXIgaGFzT3duUHJvcGVydHk9T2JqZWN0LnByb3RvdHlwZS5oYXNPd25Qcm9wZXJ0eTtpZighJC5mbi5kZXRhY2gpeyQuZm4uZGV0YWNoPWZ1bmN0aW9uKCl7cmV0dXJuIHRoaXMuZWFjaChmdW5jdGlvbigpe2lmKHRoaXMucGFyZW50Tm9kZSl7dGhpcy5wYXJlbnROb2RlLnJlbW92ZUNoaWxkKHRoaXMpfX0pfX1mdW5jdGlvbiBDYW52YXMoY2xzLGNvbnRhaW5lcil7dmFyIGVsZW1lbnQ9Y29udGFpbmVyLmNoaWxkcmVuKFwiLlwiK2NscylbMF07aWYoZWxlbWVudD09bnVsbCl7ZWxlbWVudD1kb2N1bWVudC5jcmVhdGVFbGVtZW50KFwiY2FudmFzXCIpO2VsZW1lbnQuY2xhc3NOYW1lPWNsczskKGVsZW1lbnQpLmNzcyh7ZGlyZWN0aW9uOlwibHRyXCIscG9zaXRpb246XCJhYnNvbHV0ZVwiLGxlZnQ6MCx0b3A6MH0pLmFwcGVuZFRvKGNvbnRhaW5lcik7aWYoIWVsZW1lbnQuZ2V0Q29udGV4dCl7aWYod2luZG93Lkdfdm1sQ2FudmFzTWFuYWdlcil7ZWxlbWVudD13aW5kb3cuR192bWxDYW52YXNNYW5hZ2VyLmluaXRFbGVtZW50KGVsZW1lbnQpfWVsc2V7dGhyb3cgbmV3IEVycm9yKFwiQ2FudmFzIGlzIG5vdCBhdmFpbGFibGUuIElmIHlvdSdyZSB1c2luZyBJRSB3aXRoIGEgZmFsbC1iYWNrIHN1Y2ggYXMgRXhjYW52YXMsIHRoZW4gdGhlcmUncyBlaXRoZXIgYSBtaXN0YWtlIGluIHlvdXIgY29uZGl0aW9uYWwgaW5jbHVkZSwgb3IgdGhlIHBhZ2UgaGFzIG5vIERPQ1RZUEUgYW5kIGlzIHJlbmRlcmluZyBpbiBRdWlya3MgTW9kZS5cIil9fX10aGlzLmVsZW1lbnQ9ZWxlbWVudDt2YXIgY29udGV4dD10aGlzLmNvbnRleHQ9ZWxlbWVudC5nZXRDb250ZXh0KFwiMmRcIik7dmFyIGRldmljZVBpeGVsUmF0aW89d2luZG93LmRldmljZVBpeGVsUmF0aW98fDEsYmFja2luZ1N0b3JlUmF0aW89Y29udGV4dC53ZWJraXRCYWNraW5nU3RvcmVQaXhlbFJhdGlvfHxjb250ZXh0Lm1vekJhY2tpbmdTdG9yZVBpeGVsUmF0aW98fGNvbnRleHQubXNCYWNraW5nU3RvcmVQaXhlbFJhdGlvfHxjb250ZXh0Lm9CYWNraW5nU3RvcmVQaXhlbFJhdGlvfHxjb250ZXh0LmJhY2tpbmdTdG9yZVBpeGVsUmF0aW98fDE7dGhpcy5waXhlbFJhdGlvPWRldmljZVBpeGVsUmF0aW8vYmFja2luZ1N0b3JlUmF0aW87dGhpcy5yZXNpemUoY29udGFpbmVyLndpZHRoKCksY29udGFpbmVyLmhlaWdodCgpKTt0aGlzLnRleHRDb250YWluZXI9bnVsbDt0aGlzLnRleHQ9e307dGhpcy5fdGV4dENhY2hlPXt9fUNhbnZhcy5wcm90b3R5cGUucmVzaXplPWZ1bmN0aW9uKHdpZHRoLGhlaWdodCl7aWYod2lkdGg8PTB8fGhlaWdodDw9MCl7dGhyb3cgbmV3IEVycm9yKFwiSW52YWxpZCBkaW1lbnNpb25zIGZvciBwbG90LCB3aWR0aCA9IFwiK3dpZHRoK1wiLCBoZWlnaHQgPSBcIitoZWlnaHQpfXZhciBlbGVtZW50PXRoaXMuZWxlbWVudCxjb250ZXh0PXRoaXMuY29udGV4dCxwaXhlbFJhdGlvPXRoaXMucGl4ZWxSYXRpbztpZih0aGlzLndpZHRoIT13aWR0aCl7ZWxlbWVudC53aWR0aD13aWR0aCpwaXhlbFJhdGlvO2VsZW1lbnQuc3R5bGUud2lkdGg9d2lkdGgrXCJweFwiO3RoaXMud2lkdGg9d2lkdGh9aWYodGhpcy5oZWlnaHQhPWhlaWdodCl7ZWxlbWVudC5oZWlnaHQ9aGVpZ2h0KnBpeGVsUmF0aW87ZWxlbWVudC5zdHlsZS5oZWlnaHQ9aGVpZ2h0K1wicHhcIjt0aGlzLmhlaWdodD1oZWlnaHR9Y29udGV4dC5yZXN0b3JlKCk7Y29udGV4dC5zYXZlKCk7Y29udGV4dC5zY2FsZShwaXhlbFJhdGlvLHBpeGVsUmF0aW8pfTtDYW52YXMucHJvdG90eXBlLmNsZWFyPWZ1bmN0aW9uKCl7dGhpcy5jb250ZXh0LmNsZWFyUmVjdCgwLDAsdGhpcy53aWR0aCx0aGlzLmhlaWdodCl9O0NhbnZhcy5wcm90b3R5cGUucmVuZGVyPWZ1bmN0aW9uKCl7dmFyIGNhY2hlPXRoaXMuX3RleHRDYWNoZTtmb3IodmFyIGxheWVyS2V5IGluIGNhY2hlKXtpZihoYXNPd25Qcm9wZXJ0eS5jYWxsKGNhY2hlLGxheWVyS2V5KSl7dmFyIGxheWVyPXRoaXMuZ2V0VGV4dExheWVyKGxheWVyS2V5KSxsYXllckNhY2hlPWNhY2hlW2xheWVyS2V5XTtsYXllci5oaWRlKCk7Zm9yKHZhciBzdHlsZUtleSBpbiBsYXllckNhY2hlKXtpZihoYXNPd25Qcm9wZXJ0eS5jYWxsKGxheWVyQ2FjaGUsc3R5bGVLZXkpKXt2YXIgc3R5bGVDYWNoZT1sYXllckNhY2hlW3N0eWxlS2V5XTtmb3IodmFyIGtleSBpbiBzdHlsZUNhY2hlKXtpZihoYXNPd25Qcm9wZXJ0eS5jYWxsKHN0eWxlQ2FjaGUsa2V5KSl7dmFyIHBvc2l0aW9ucz1zdHlsZUNhY2hlW2tleV0ucG9zaXRpb25zO2Zvcih2YXIgaT0wLHBvc2l0aW9uO3Bvc2l0aW9uPXBvc2l0aW9uc1tpXTtpKyspe2lmKHBvc2l0aW9uLmFjdGl2ZSl7aWYoIXBvc2l0aW9uLnJlbmRlcmVkKXtsYXllci5hcHBlbmQocG9zaXRpb24uZWxlbWVudCk7cG9zaXRpb24ucmVuZGVyZWQ9dHJ1ZX19ZWxzZXtwb3NpdGlvbnMuc3BsaWNlKGktLSwxKTtpZihwb3NpdGlvbi5yZW5kZXJlZCl7cG9zaXRpb24uZWxlbWVudC5kZXRhY2goKX19fWlmKHBvc2l0aW9ucy5sZW5ndGg9PTApe2RlbGV0ZSBzdHlsZUNhY2hlW2tleV19fX19fWxheWVyLnNob3coKX19fTtDYW52YXMucHJvdG90eXBlLmdldFRleHRMYXllcj1mdW5jdGlvbihjbGFzc2VzKXt2YXIgbGF5ZXI9dGhpcy50ZXh0W2NsYXNzZXNdO2lmKGxheWVyPT1udWxsKXtpZih0aGlzLnRleHRDb250YWluZXI9PW51bGwpe3RoaXMudGV4dENvbnRhaW5lcj0kKFwiPGRpdiBjbGFzcz0nZmxvdC10ZXh0Jz48L2Rpdj5cIikuY3NzKHtwb3NpdGlvbjpcImFic29sdXRlXCIsdG9wOjAsbGVmdDowLGJvdHRvbTowLHJpZ2h0OjAsXCJmb250LXNpemVcIjpcInNtYWxsZXJcIixjb2xvcjpcIiM1NDU0NTRcIn0pLmluc2VydEFmdGVyKHRoaXMuZWxlbWVudCl9bGF5ZXI9dGhpcy50ZXh0W2NsYXNzZXNdPSQoXCI8ZGl2PjwvZGl2PlwiKS5hZGRDbGFzcyhjbGFzc2VzKS5jc3Moe3Bvc2l0aW9uOlwiYWJzb2x1dGVcIix0b3A6MCxsZWZ0OjAsYm90dG9tOjAscmlnaHQ6MH0pLmFwcGVuZFRvKHRoaXMudGV4dENvbnRhaW5lcil9cmV0dXJuIGxheWVyfTtDYW52YXMucHJvdG90eXBlLmdldFRleHRJbmZvPWZ1bmN0aW9uKGxheWVyLHRleHQsZm9udCxhbmdsZSx3aWR0aCl7dmFyIHRleHRTdHlsZSxsYXllckNhY2hlLHN0eWxlQ2FjaGUsaW5mbzt0ZXh0PVwiXCIrdGV4dDtpZih0eXBlb2YgZm9udD09PVwib2JqZWN0XCIpe3RleHRTdHlsZT1mb250LnN0eWxlK1wiIFwiK2ZvbnQudmFyaWFudCtcIiBcIitmb250LndlaWdodCtcIiBcIitmb250LnNpemUrXCJweC9cIitmb250LmxpbmVIZWlnaHQrXCJweCBcIitmb250LmZhbWlseX1lbHNle3RleHRTdHlsZT1mb250fWxheWVyQ2FjaGU9dGhpcy5fdGV4dENhY2hlW2xheWVyXTtpZihsYXllckNhY2hlPT1udWxsKXtsYXllckNhY2hlPXRoaXMuX3RleHRDYWNoZVtsYXllcl09e319c3R5bGVDYWNoZT1sYXllckNhY2hlW3RleHRTdHlsZV07aWYoc3R5bGVDYWNoZT09bnVsbCl7c3R5bGVDYWNoZT1sYXllckNhY2hlW3RleHRTdHlsZV09e319aW5mbz1zdHlsZUNhY2hlW3RleHRdO2lmKGluZm89PW51bGwpe3ZhciBlbGVtZW50PSQoXCI8ZGl2PjwvZGl2PlwiKS5odG1sKHRleHQpLmNzcyh7cG9zaXRpb246XCJhYnNvbHV0ZVwiLFwibWF4LXdpZHRoXCI6d2lkdGgsdG9wOi05OTk5fSkuYXBwZW5kVG8odGhpcy5nZXRUZXh0TGF5ZXIobGF5ZXIpKTtpZih0eXBlb2YgZm9udD09PVwib2JqZWN0XCIpe2VsZW1lbnQuY3NzKHtmb250OnRleHRTdHlsZSxjb2xvcjpmb250LmNvbG9yfSl9ZWxzZSBpZih0eXBlb2YgZm9udD09PVwic3RyaW5nXCIpe2VsZW1lbnQuYWRkQ2xhc3MoZm9udCl9aW5mbz1zdHlsZUNhY2hlW3RleHRdPXt3aWR0aDplbGVtZW50Lm91dGVyV2lkdGgodHJ1ZSksaGVpZ2h0OmVsZW1lbnQub3V0ZXJIZWlnaHQodHJ1ZSksZWxlbWVudDplbGVtZW50LHBvc2l0aW9uczpbXX07ZWxlbWVudC5kZXRhY2goKX1yZXR1cm4gaW5mb307Q2FudmFzLnByb3RvdHlwZS5hZGRUZXh0PWZ1bmN0aW9uKGxheWVyLHgseSx0ZXh0LGZvbnQsYW5nbGUsd2lkdGgsaGFsaWduLHZhbGlnbil7dmFyIGluZm89dGhpcy5nZXRUZXh0SW5mbyhsYXllcix0ZXh0LGZvbnQsYW5nbGUsd2lkdGgpLHBvc2l0aW9ucz1pbmZvLnBvc2l0aW9ucztpZihoYWxpZ249PVwiY2VudGVyXCIpe3gtPWluZm8ud2lkdGgvMn1lbHNlIGlmKGhhbGlnbj09XCJyaWdodFwiKXt4LT1pbmZvLndpZHRofWlmKHZhbGlnbj09XCJtaWRkbGVcIil7eS09aW5mby5oZWlnaHQvMn1lbHNlIGlmKHZhbGlnbj09XCJib3R0b21cIil7eS09aW5mby5oZWlnaHR9Zm9yKHZhciBpPTAscG9zaXRpb247cG9zaXRpb249cG9zaXRpb25zW2ldO2krKyl7aWYocG9zaXRpb24ueD09eCYmcG9zaXRpb24ueT09eSl7cG9zaXRpb24uYWN0aXZlPXRydWU7cmV0dXJufX1wb3NpdGlvbj17YWN0aXZlOnRydWUscmVuZGVyZWQ6ZmFsc2UsZWxlbWVudDpwb3NpdGlvbnMubGVuZ3RoP2luZm8uZWxlbWVudC5jbG9uZSgpOmluZm8uZWxlbWVudCx4OngseTp5fTtwb3NpdGlvbnMucHVzaChwb3NpdGlvbik7cG9zaXRpb24uZWxlbWVudC5jc3Moe3RvcDpNYXRoLnJvdW5kKHkpLGxlZnQ6TWF0aC5yb3VuZCh4KSxcInRleHQtYWxpZ25cIjpoYWxpZ259KX07Q2FudmFzLnByb3RvdHlwZS5yZW1vdmVUZXh0PWZ1bmN0aW9uKGxheWVyLHgseSx0ZXh0LGZvbnQsYW5nbGUpe2lmKHRleHQ9PW51bGwpe3ZhciBsYXllckNhY2hlPXRoaXMuX3RleHRDYWNoZVtsYXllcl07aWYobGF5ZXJDYWNoZSE9bnVsbCl7Zm9yKHZhciBzdHlsZUtleSBpbiBsYXllckNhY2hlKXtpZihoYXNPd25Qcm9wZXJ0eS5jYWxsKGxheWVyQ2FjaGUsc3R5bGVLZXkpKXt2YXIgc3R5bGVDYWNoZT1sYXllckNhY2hlW3N0eWxlS2V5XTtmb3IodmFyIGtleSBpbiBzdHlsZUNhY2hlKXtpZihoYXNPd25Qcm9wZXJ0eS5jYWxsKHN0eWxlQ2FjaGUsa2V5KSl7dmFyIHBvc2l0aW9ucz1zdHlsZUNhY2hlW2tleV0ucG9zaXRpb25zO2Zvcih2YXIgaT0wLHBvc2l0aW9uO3Bvc2l0aW9uPXBvc2l0aW9uc1tpXTtpKyspe3Bvc2l0aW9uLmFjdGl2ZT1mYWxzZX19fX19fX1lbHNle3ZhciBwb3NpdGlvbnM9dGhpcy5nZXRUZXh0SW5mbyhsYXllcix0ZXh0LGZvbnQsYW5nbGUpLnBvc2l0aW9ucztmb3IodmFyIGk9MCxwb3NpdGlvbjtwb3NpdGlvbj1wb3NpdGlvbnNbaV07aSsrKXtpZihwb3NpdGlvbi54PT14JiZwb3NpdGlvbi55PT15KXtwb3NpdGlvbi5hY3RpdmU9ZmFsc2V9fX19O2Z1bmN0aW9uIFBsb3QocGxhY2Vob2xkZXIsZGF0YV8sb3B0aW9uc18scGx1Z2lucyl7dmFyIHNlcmllcz1bXSxvcHRpb25zPXtjb2xvcnM6W1wiI2VkYzI0MFwiLFwiI2FmZDhmOFwiLFwiI2NiNGI0YlwiLFwiIzRkYTc0ZFwiLFwiIzk0NDBlZFwiXSxsZWdlbmQ6e3Nob3c6dHJ1ZSxub0NvbHVtbnM6MSxsYWJlbEZvcm1hdHRlcjpudWxsLGxhYmVsQm94Qm9yZGVyQ29sb3I6XCIjY2NjXCIsY29udGFpbmVyOm51bGwscG9zaXRpb246XCJuZVwiLG1hcmdpbjo1LGJhY2tncm91bmRDb2xvcjpudWxsLGJhY2tncm91bmRPcGFjaXR5Oi44NSxzb3J0ZWQ6bnVsbH0seGF4aXM6e3Nob3c6bnVsbCxwb3NpdGlvbjpcImJvdHRvbVwiLG1vZGU6bnVsbCxmb250Om51bGwsY29sb3I6bnVsbCx0aWNrQ29sb3I6bnVsbCx0cmFuc2Zvcm06bnVsbCxpbnZlcnNlVHJhbnNmb3JtOm51bGwsbWluOm51bGwsbWF4Om51bGwsYXV0b3NjYWxlTWFyZ2luOm51bGwsdGlja3M6bnVsbCx0aWNrRm9ybWF0dGVyOm51bGwsbGFiZWxXaWR0aDpudWxsLGxhYmVsSGVpZ2h0Om51bGwscmVzZXJ2ZVNwYWNlOm51bGwsdGlja0xlbmd0aDpudWxsLGFsaWduVGlja3NXaXRoQXhpczpudWxsLHRpY2tEZWNpbWFsczpudWxsLHRpY2tTaXplOm51bGwsbWluVGlja1NpemU6bnVsbH0seWF4aXM6e2F1dG9zY2FsZU1hcmdpbjouMDIscG9zaXRpb246XCJsZWZ0XCJ9LHhheGVzOltdLHlheGVzOltdLHNlcmllczp7cG9pbnRzOntzaG93OmZhbHNlLHJhZGl1czozLGxpbmVXaWR0aDoyLGZpbGw6dHJ1ZSxmaWxsQ29sb3I6XCIjZmZmZmZmXCIsc3ltYm9sOlwiY2lyY2xlXCJ9LGxpbmVzOntsaW5lV2lkdGg6MixmaWxsOmZhbHNlLGZpbGxDb2xvcjpudWxsLHN0ZXBzOmZhbHNlfSxiYXJzOntzaG93OmZhbHNlLGxpbmVXaWR0aDoyLGJhcldpZHRoOjEsZmlsbDp0cnVlLGZpbGxDb2xvcjpudWxsLGFsaWduOlwibGVmdFwiLGhvcml6b250YWw6ZmFsc2UsemVybzp0cnVlfSxzaGFkb3dTaXplOjMsaGlnaGxpZ2h0Q29sb3I6bnVsbH0sZ3JpZDp7c2hvdzp0cnVlLGFib3ZlRGF0YTpmYWxzZSxjb2xvcjpcIiM1NDU0NTRcIixiYWNrZ3JvdW5kQ29sb3I6bnVsbCxib3JkZXJDb2xvcjpudWxsLHRpY2tDb2xvcjpudWxsLG1hcmdpbjowLGxhYmVsTWFyZ2luOjUsYXhpc01hcmdpbjo4LGJvcmRlcldpZHRoOjIsbWluQm9yZGVyTWFyZ2luOm51bGwsbWFya2luZ3M6bnVsbCxtYXJraW5nc0NvbG9yOlwiI2Y0ZjRmNFwiLG1hcmtpbmdzTGluZVdpZHRoOjIsY2xpY2thYmxlOmZhbHNlLGhvdmVyYWJsZTpmYWxzZSxhdXRvSGlnaGxpZ2h0OnRydWUsbW91c2VBY3RpdmVSYWRpdXM6MTB9LGludGVyYWN0aW9uOntyZWRyYXdPdmVybGF5SW50ZXJ2YWw6MWUzLzYwfSxob29rczp7fX0sc3VyZmFjZT1udWxsLG92ZXJsYXk9bnVsbCxldmVudEhvbGRlcj1udWxsLGN0eD1udWxsLG9jdHg9bnVsbCx4YXhlcz1bXSx5YXhlcz1bXSxwbG90T2Zmc2V0PXtsZWZ0OjAscmlnaHQ6MCx0b3A6MCxib3R0b206MH0scGxvdFdpZHRoPTAscGxvdEhlaWdodD0wLGhvb2tzPXtwcm9jZXNzT3B0aW9uczpbXSxwcm9jZXNzUmF3RGF0YTpbXSxwcm9jZXNzRGF0YXBvaW50czpbXSxwcm9jZXNzT2Zmc2V0OltdLGRyYXdCYWNrZ3JvdW5kOltdLGRyYXdTZXJpZXM6W10sZHJhdzpbXSxiaW5kRXZlbnRzOltdLGRyYXdPdmVybGF5OltdLHNodXRkb3duOltdfSxwbG90PXRoaXM7cGxvdC5zZXREYXRhPXNldERhdGE7cGxvdC5zZXR1cEdyaWQ9c2V0dXBHcmlkO3Bsb3QuZHJhdz1kcmF3O3Bsb3QuZ2V0UGxhY2Vob2xkZXI9ZnVuY3Rpb24oKXtyZXR1cm4gcGxhY2Vob2xkZXJ9O3Bsb3QuZ2V0Q2FudmFzPWZ1bmN0aW9uKCl7cmV0dXJuIHN1cmZhY2UuZWxlbWVudH07cGxvdC5nZXRQbG90T2Zmc2V0PWZ1bmN0aW9uKCl7cmV0dXJuIHBsb3RPZmZzZXR9O3Bsb3Qud2lkdGg9ZnVuY3Rpb24oKXtyZXR1cm4gcGxvdFdpZHRofTtwbG90LmhlaWdodD1mdW5jdGlvbigpe3JldHVybiBwbG90SGVpZ2h0fTtwbG90Lm9mZnNldD1mdW5jdGlvbigpe3ZhciBvPWV2ZW50SG9sZGVyLm9mZnNldCgpO28ubGVmdCs9cGxvdE9mZnNldC5sZWZ0O28udG9wKz1wbG90T2Zmc2V0LnRvcDtyZXR1cm4gb307cGxvdC5nZXREYXRhPWZ1bmN0aW9uKCl7cmV0dXJuIHNlcmllc307cGxvdC5nZXRBeGVzPWZ1bmN0aW9uKCl7dmFyIHJlcz17fSxpOyQuZWFjaCh4YXhlcy5jb25jYXQoeWF4ZXMpLGZ1bmN0aW9uKF8sYXhpcyl7aWYoYXhpcylyZXNbYXhpcy5kaXJlY3Rpb24rKGF4aXMubiE9MT9heGlzLm46XCJcIikrXCJheGlzXCJdPWF4aXN9KTtyZXR1cm4gcmVzfTtwbG90LmdldFhBeGVzPWZ1bmN0aW9uKCl7cmV0dXJuIHhheGVzfTtwbG90LmdldFlBeGVzPWZ1bmN0aW9uKCl7cmV0dXJuIHlheGVzfTtwbG90LmMycD1jYW52YXNUb0F4aXNDb29yZHM7cGxvdC5wMmM9YXhpc1RvQ2FudmFzQ29vcmRzO3Bsb3QuZ2V0T3B0aW9ucz1mdW5jdGlvbigpe3JldHVybiBvcHRpb25zfTtwbG90LmhpZ2hsaWdodD1oaWdobGlnaHQ7cGxvdC51bmhpZ2hsaWdodD11bmhpZ2hsaWdodDtwbG90LnRyaWdnZXJSZWRyYXdPdmVybGF5PXRyaWdnZXJSZWRyYXdPdmVybGF5O3Bsb3QucG9pbnRPZmZzZXQ9ZnVuY3Rpb24ocG9pbnQpe3JldHVybntsZWZ0OnBhcnNlSW50KHhheGVzW2F4aXNOdW1iZXIocG9pbnQsXCJ4XCIpLTFdLnAyYygrcG9pbnQueCkrcGxvdE9mZnNldC5sZWZ0LDEwKSx0b3A6cGFyc2VJbnQoeWF4ZXNbYXhpc051bWJlcihwb2ludCxcInlcIiktMV0ucDJjKCtwb2ludC55KStwbG90T2Zmc2V0LnRvcCwxMCl9fTtwbG90LnNodXRkb3duPXNodXRkb3duO3Bsb3QuZGVzdHJveT1mdW5jdGlvbigpe3NodXRkb3duKCk7cGxhY2Vob2xkZXIucmVtb3ZlRGF0YShcInBsb3RcIikuZW1wdHkoKTtzZXJpZXM9W107b3B0aW9ucz1udWxsO3N1cmZhY2U9bnVsbDtvdmVybGF5PW51bGw7ZXZlbnRIb2xkZXI9bnVsbDtjdHg9bnVsbDtvY3R4PW51bGw7eGF4ZXM9W107eWF4ZXM9W107aG9va3M9bnVsbDtoaWdobGlnaHRzPVtdO3Bsb3Q9bnVsbH07cGxvdC5yZXNpemU9ZnVuY3Rpb24oKXt2YXIgd2lkdGg9cGxhY2Vob2xkZXIud2lkdGgoKSxoZWlnaHQ9cGxhY2Vob2xkZXIuaGVpZ2h0KCk7c3VyZmFjZS5yZXNpemUod2lkdGgsaGVpZ2h0KTtvdmVybGF5LnJlc2l6ZSh3aWR0aCxoZWlnaHQpfTtwbG90Lmhvb2tzPWhvb2tzO2luaXRQbHVnaW5zKHBsb3QpO3BhcnNlT3B0aW9ucyhvcHRpb25zXyk7c2V0dXBDYW52YXNlcygpO3NldERhdGEoZGF0YV8pO3NldHVwR3JpZCgpO2RyYXcoKTtiaW5kRXZlbnRzKCk7ZnVuY3Rpb24gZXhlY3V0ZUhvb2tzKGhvb2ssYXJncyl7YXJncz1bcGxvdF0uY29uY2F0KGFyZ3MpO2Zvcih2YXIgaT0wO2k8aG9vay5sZW5ndGg7KytpKWhvb2tbaV0uYXBwbHkodGhpcyxhcmdzKX1mdW5jdGlvbiBpbml0UGx1Z2lucygpe3ZhciBjbGFzc2VzPXtDYW52YXM6Q2FudmFzfTtmb3IodmFyIGk9MDtpPHBsdWdpbnMubGVuZ3RoOysraSl7dmFyIHA9cGx1Z2luc1tpXTtwLmluaXQocGxvdCxjbGFzc2VzKTtpZihwLm9wdGlvbnMpJC5leHRlbmQodHJ1ZSxvcHRpb25zLHAub3B0aW9ucyl9fWZ1bmN0aW9uIHBhcnNlT3B0aW9ucyhvcHRzKXskLmV4dGVuZCh0cnVlLG9wdGlvbnMsb3B0cyk7aWYob3B0cyYmb3B0cy5jb2xvcnMpe29wdGlvbnMuY29sb3JzPW9wdHMuY29sb3JzfWlmKG9wdGlvbnMueGF4aXMuY29sb3I9PW51bGwpb3B0aW9ucy54YXhpcy5jb2xvcj0kLmNvbG9yLnBhcnNlKG9wdGlvbnMuZ3JpZC5jb2xvcikuc2NhbGUoXCJhXCIsLjIyKS50b1N0cmluZygpO2lmKG9wdGlvbnMueWF4aXMuY29sb3I9PW51bGwpb3B0aW9ucy55YXhpcy5jb2xvcj0kLmNvbG9yLnBhcnNlKG9wdGlvbnMuZ3JpZC5jb2xvcikuc2NhbGUoXCJhXCIsLjIyKS50b1N0cmluZygpO2lmKG9wdGlvbnMueGF4aXMudGlja0NvbG9yPT1udWxsKW9wdGlvbnMueGF4aXMudGlja0NvbG9yPW9wdGlvbnMuZ3JpZC50aWNrQ29sb3J8fG9wdGlvbnMueGF4aXMuY29sb3I7aWYob3B0aW9ucy55YXhpcy50aWNrQ29sb3I9PW51bGwpb3B0aW9ucy55YXhpcy50aWNrQ29sb3I9b3B0aW9ucy5ncmlkLnRpY2tDb2xvcnx8b3B0aW9ucy55YXhpcy5jb2xvcjtpZihvcHRpb25zLmdyaWQuYm9yZGVyQ29sb3I9PW51bGwpb3B0aW9ucy5ncmlkLmJvcmRlckNvbG9yPW9wdGlvbnMuZ3JpZC5jb2xvcjtpZihvcHRpb25zLmdyaWQudGlja0NvbG9yPT1udWxsKW9wdGlvbnMuZ3JpZC50aWNrQ29sb3I9JC5jb2xvci5wYXJzZShvcHRpb25zLmdyaWQuY29sb3IpLnNjYWxlKFwiYVwiLC4yMikudG9TdHJpbmcoKTt2YXIgaSxheGlzT3B0aW9ucyxheGlzQ291bnQsZm9udFNpemU9cGxhY2Vob2xkZXIuY3NzKFwiZm9udC1zaXplXCIpLGZvbnRTaXplRGVmYXVsdD1mb250U2l6ZT8rZm9udFNpemUucmVwbGFjZShcInB4XCIsXCJcIik6MTMsZm9udERlZmF1bHRzPXtzdHlsZTpwbGFjZWhvbGRlci5jc3MoXCJmb250LXN0eWxlXCIpLHNpemU6TWF0aC5yb3VuZCguOCpmb250U2l6ZURlZmF1bHQpLHZhcmlhbnQ6cGxhY2Vob2xkZXIuY3NzKFwiZm9udC12YXJpYW50XCIpLHdlaWdodDpwbGFjZWhvbGRlci5jc3MoXCJmb250LXdlaWdodFwiKSxmYW1pbHk6cGxhY2Vob2xkZXIuY3NzKFwiZm9udC1mYW1pbHlcIil9O2F4aXNDb3VudD1vcHRpb25zLnhheGVzLmxlbmd0aHx8MTtmb3IoaT0wO2k8YXhpc0NvdW50OysraSl7YXhpc09wdGlvbnM9b3B0aW9ucy54YXhlc1tpXTtpZihheGlzT3B0aW9ucyYmIWF4aXNPcHRpb25zLnRpY2tDb2xvcil7YXhpc09wdGlvbnMudGlja0NvbG9yPWF4aXNPcHRpb25zLmNvbG9yfWF4aXNPcHRpb25zPSQuZXh0ZW5kKHRydWUse30sb3B0aW9ucy54YXhpcyxheGlzT3B0aW9ucyk7b3B0aW9ucy54YXhlc1tpXT1heGlzT3B0aW9ucztpZihheGlzT3B0aW9ucy5mb250KXtheGlzT3B0aW9ucy5mb250PSQuZXh0ZW5kKHt9LGZvbnREZWZhdWx0cyxheGlzT3B0aW9ucy5mb250KTtpZighYXhpc09wdGlvbnMuZm9udC5jb2xvcil7YXhpc09wdGlvbnMuZm9udC5jb2xvcj1heGlzT3B0aW9ucy5jb2xvcn1pZighYXhpc09wdGlvbnMuZm9udC5saW5lSGVpZ2h0KXtheGlzT3B0aW9ucy5mb250LmxpbmVIZWlnaHQ9TWF0aC5yb3VuZChheGlzT3B0aW9ucy5mb250LnNpemUqMS4xNSl9fX1heGlzQ291bnQ9b3B0aW9ucy55YXhlcy5sZW5ndGh8fDE7Zm9yKGk9MDtpPGF4aXNDb3VudDsrK2kpe2F4aXNPcHRpb25zPW9wdGlvbnMueWF4ZXNbaV07aWYoYXhpc09wdGlvbnMmJiFheGlzT3B0aW9ucy50aWNrQ29sb3Ipe2F4aXNPcHRpb25zLnRpY2tDb2xvcj1heGlzT3B0aW9ucy5jb2xvcn1heGlzT3B0aW9ucz0kLmV4dGVuZCh0cnVlLHt9LG9wdGlvbnMueWF4aXMsYXhpc09wdGlvbnMpO29wdGlvbnMueWF4ZXNbaV09YXhpc09wdGlvbnM7aWYoYXhpc09wdGlvbnMuZm9udCl7YXhpc09wdGlvbnMuZm9udD0kLmV4dGVuZCh7fSxmb250RGVmYXVsdHMsYXhpc09wdGlvbnMuZm9udCk7aWYoIWF4aXNPcHRpb25zLmZvbnQuY29sb3Ipe2F4aXNPcHRpb25zLmZvbnQuY29sb3I9YXhpc09wdGlvbnMuY29sb3J9aWYoIWF4aXNPcHRpb25zLmZvbnQubGluZUhlaWdodCl7YXhpc09wdGlvbnMuZm9udC5saW5lSGVpZ2h0PU1hdGgucm91bmQoYXhpc09wdGlvbnMuZm9udC5zaXplKjEuMTUpfX19aWYob3B0aW9ucy54YXhpcy5ub1RpY2tzJiZvcHRpb25zLnhheGlzLnRpY2tzPT1udWxsKW9wdGlvbnMueGF4aXMudGlja3M9b3B0aW9ucy54YXhpcy5ub1RpY2tzO2lmKG9wdGlvbnMueWF4aXMubm9UaWNrcyYmb3B0aW9ucy55YXhpcy50aWNrcz09bnVsbClvcHRpb25zLnlheGlzLnRpY2tzPW9wdGlvbnMueWF4aXMubm9UaWNrcztpZihvcHRpb25zLngyYXhpcyl7b3B0aW9ucy54YXhlc1sxXT0kLmV4dGVuZCh0cnVlLHt9LG9wdGlvbnMueGF4aXMsb3B0aW9ucy54MmF4aXMpO29wdGlvbnMueGF4ZXNbMV0ucG9zaXRpb249XCJ0b3BcIjtpZihvcHRpb25zLngyYXhpcy5taW49PW51bGwpe29wdGlvbnMueGF4ZXNbMV0ubWluPW51bGx9aWYob3B0aW9ucy54MmF4aXMubWF4PT1udWxsKXtvcHRpb25zLnhheGVzWzFdLm1heD1udWxsfX1pZihvcHRpb25zLnkyYXhpcyl7b3B0aW9ucy55YXhlc1sxXT0kLmV4dGVuZCh0cnVlLHt9LG9wdGlvbnMueWF4aXMsb3B0aW9ucy55MmF4aXMpO29wdGlvbnMueWF4ZXNbMV0ucG9zaXRpb249XCJyaWdodFwiO2lmKG9wdGlvbnMueTJheGlzLm1pbj09bnVsbCl7b3B0aW9ucy55YXhlc1sxXS5taW49bnVsbH1pZihvcHRpb25zLnkyYXhpcy5tYXg9PW51bGwpe29wdGlvbnMueWF4ZXNbMV0ubWF4PW51bGx9fWlmKG9wdGlvbnMuZ3JpZC5jb2xvcmVkQXJlYXMpb3B0aW9ucy5ncmlkLm1hcmtpbmdzPW9wdGlvbnMuZ3JpZC5jb2xvcmVkQXJlYXM7aWYob3B0aW9ucy5ncmlkLmNvbG9yZWRBcmVhc0NvbG9yKW9wdGlvbnMuZ3JpZC5tYXJraW5nc0NvbG9yPW9wdGlvbnMuZ3JpZC5jb2xvcmVkQXJlYXNDb2xvcjtpZihvcHRpb25zLmxpbmVzKSQuZXh0ZW5kKHRydWUsb3B0aW9ucy5zZXJpZXMubGluZXMsb3B0aW9ucy5saW5lcyk7aWYob3B0aW9ucy5wb2ludHMpJC5leHRlbmQodHJ1ZSxvcHRpb25zLnNlcmllcy5wb2ludHMsb3B0aW9ucy5wb2ludHMpO2lmKG9wdGlvbnMuYmFycykkLmV4dGVuZCh0cnVlLG9wdGlvbnMuc2VyaWVzLmJhcnMsb3B0aW9ucy5iYXJzKTtpZihvcHRpb25zLnNoYWRvd1NpemUhPW51bGwpb3B0aW9ucy5zZXJpZXMuc2hhZG93U2l6ZT1vcHRpb25zLnNoYWRvd1NpemU7aWYob3B0aW9ucy5oaWdobGlnaHRDb2xvciE9bnVsbClvcHRpb25zLnNlcmllcy5oaWdobGlnaHRDb2xvcj1vcHRpb25zLmhpZ2hsaWdodENvbG9yO2ZvcihpPTA7aTxvcHRpb25zLnhheGVzLmxlbmd0aDsrK2kpZ2V0T3JDcmVhdGVBeGlzKHhheGVzLGkrMSkub3B0aW9ucz1vcHRpb25zLnhheGVzW2ldO2ZvcihpPTA7aTxvcHRpb25zLnlheGVzLmxlbmd0aDsrK2kpZ2V0T3JDcmVhdGVBeGlzKHlheGVzLGkrMSkub3B0aW9ucz1vcHRpb25zLnlheGVzW2ldO2Zvcih2YXIgbiBpbiBob29rcylpZihvcHRpb25zLmhvb2tzW25dJiZvcHRpb25zLmhvb2tzW25dLmxlbmd0aClob29rc1tuXT1ob29rc1tuXS5jb25jYXQob3B0aW9ucy5ob29rc1tuXSk7ZXhlY3V0ZUhvb2tzKGhvb2tzLnByb2Nlc3NPcHRpb25zLFtvcHRpb25zXSl9ZnVuY3Rpb24gc2V0RGF0YShkKXtzZXJpZXM9cGFyc2VEYXRhKGQpO2ZpbGxJblNlcmllc09wdGlvbnMoKTtwcm9jZXNzRGF0YSgpfWZ1bmN0aW9uIHBhcnNlRGF0YShkKXt2YXIgcmVzPVtdO2Zvcih2YXIgaT0wO2k8ZC5sZW5ndGg7KytpKXt2YXIgcz0kLmV4dGVuZCh0cnVlLHt9LG9wdGlvbnMuc2VyaWVzKTtpZihkW2ldLmRhdGEhPW51bGwpe3MuZGF0YT1kW2ldLmRhdGE7ZGVsZXRlIGRbaV0uZGF0YTskLmV4dGVuZCh0cnVlLHMsZFtpXSk7ZFtpXS5kYXRhPXMuZGF0YX1lbHNlIHMuZGF0YT1kW2ldO3Jlcy5wdXNoKHMpfXJldHVybiByZXN9ZnVuY3Rpb24gYXhpc051bWJlcihvYmosY29vcmQpe3ZhciBhPW9ialtjb29yZCtcImF4aXNcIl07aWYodHlwZW9mIGE9PVwib2JqZWN0XCIpYT1hLm47aWYodHlwZW9mIGEhPVwibnVtYmVyXCIpYT0xO3JldHVybiBhfWZ1bmN0aW9uIGFsbEF4ZXMoKXtyZXR1cm4gJC5ncmVwKHhheGVzLmNvbmNhdCh5YXhlcyksZnVuY3Rpb24oYSl7cmV0dXJuIGF9KX1mdW5jdGlvbiBjYW52YXNUb0F4aXNDb29yZHMocG9zKXt2YXIgcmVzPXt9LGksYXhpcztmb3IoaT0wO2k8eGF4ZXMubGVuZ3RoOysraSl7YXhpcz14YXhlc1tpXTtpZihheGlzJiZheGlzLnVzZWQpcmVzW1wieFwiK2F4aXMubl09YXhpcy5jMnAocG9zLmxlZnQpfWZvcihpPTA7aTx5YXhlcy5sZW5ndGg7KytpKXtheGlzPXlheGVzW2ldO2lmKGF4aXMmJmF4aXMudXNlZClyZXNbXCJ5XCIrYXhpcy5uXT1heGlzLmMycChwb3MudG9wKX1pZihyZXMueDEhPT11bmRlZmluZWQpcmVzLng9cmVzLngxO2lmKHJlcy55MSE9PXVuZGVmaW5lZClyZXMueT1yZXMueTE7cmV0dXJuIHJlc31mdW5jdGlvbiBheGlzVG9DYW52YXNDb29yZHMocG9zKXt2YXIgcmVzPXt9LGksYXhpcyxrZXk7Zm9yKGk9MDtpPHhheGVzLmxlbmd0aDsrK2kpe2F4aXM9eGF4ZXNbaV07aWYoYXhpcyYmYXhpcy51c2VkKXtrZXk9XCJ4XCIrYXhpcy5uO2lmKHBvc1trZXldPT1udWxsJiZheGlzLm49PTEpa2V5PVwieFwiO2lmKHBvc1trZXldIT1udWxsKXtyZXMubGVmdD1heGlzLnAyYyhwb3Nba2V5XSk7YnJlYWt9fX1mb3IoaT0wO2k8eWF4ZXMubGVuZ3RoOysraSl7YXhpcz15YXhlc1tpXTtpZihheGlzJiZheGlzLnVzZWQpe2tleT1cInlcIitheGlzLm47aWYocG9zW2tleV09PW51bGwmJmF4aXMubj09MSlrZXk9XCJ5XCI7aWYocG9zW2tleV0hPW51bGwpe3Jlcy50b3A9YXhpcy5wMmMocG9zW2tleV0pO2JyZWFrfX19cmV0dXJuIHJlc31mdW5jdGlvbiBnZXRPckNyZWF0ZUF4aXMoYXhlcyxudW1iZXIpe2lmKCFheGVzW251bWJlci0xXSlheGVzW251bWJlci0xXT17bjpudW1iZXIsZGlyZWN0aW9uOmF4ZXM9PXhheGVzP1wieFwiOlwieVwiLG9wdGlvbnM6JC5leHRlbmQodHJ1ZSx7fSxheGVzPT14YXhlcz9vcHRpb25zLnhheGlzOm9wdGlvbnMueWF4aXMpfTtyZXR1cm4gYXhlc1tudW1iZXItMV19ZnVuY3Rpb24gZmlsbEluU2VyaWVzT3B0aW9ucygpe3ZhciBuZWVkZWRDb2xvcnM9c2VyaWVzLmxlbmd0aCxtYXhJbmRleD0tMSxpO2ZvcihpPTA7aTxzZXJpZXMubGVuZ3RoOysraSl7dmFyIHNjPXNlcmllc1tpXS5jb2xvcjtpZihzYyE9bnVsbCl7bmVlZGVkQ29sb3JzLS07aWYodHlwZW9mIHNjPT1cIm51bWJlclwiJiZzYz5tYXhJbmRleCl7bWF4SW5kZXg9c2N9fX1pZihuZWVkZWRDb2xvcnM8PW1heEluZGV4KXtuZWVkZWRDb2xvcnM9bWF4SW5kZXgrMX12YXIgYyxjb2xvcnM9W10sY29sb3JQb29sPW9wdGlvbnMuY29sb3JzLGNvbG9yUG9vbFNpemU9Y29sb3JQb29sLmxlbmd0aCx2YXJpYXRpb249MDtmb3IoaT0wO2k8bmVlZGVkQ29sb3JzO2krKyl7Yz0kLmNvbG9yLnBhcnNlKGNvbG9yUG9vbFtpJWNvbG9yUG9vbFNpemVdfHxcIiM2NjZcIik7aWYoaSVjb2xvclBvb2xTaXplPT0wJiZpKXtpZih2YXJpYXRpb24+PTApe2lmKHZhcmlhdGlvbjwuNSl7dmFyaWF0aW9uPS12YXJpYXRpb24tLjJ9ZWxzZSB2YXJpYXRpb249MH1lbHNlIHZhcmlhdGlvbj0tdmFyaWF0aW9ufWNvbG9yc1tpXT1jLnNjYWxlKFwicmdiXCIsMSt2YXJpYXRpb24pfXZhciBjb2xvcmk9MCxzO2ZvcihpPTA7aTxzZXJpZXMubGVuZ3RoOysraSl7cz1zZXJpZXNbaV07aWYocy5jb2xvcj09bnVsbCl7cy5jb2xvcj1jb2xvcnNbY29sb3JpXS50b1N0cmluZygpOysrY29sb3JpfWVsc2UgaWYodHlwZW9mIHMuY29sb3I9PVwibnVtYmVyXCIpcy5jb2xvcj1jb2xvcnNbcy5jb2xvcl0udG9TdHJpbmcoKTtpZihzLmxpbmVzLnNob3c9PW51bGwpe3ZhciB2LHNob3c9dHJ1ZTtmb3IodiBpbiBzKWlmKHNbdl0mJnNbdl0uc2hvdyl7c2hvdz1mYWxzZTticmVha31pZihzaG93KXMubGluZXMuc2hvdz10cnVlfWlmKHMubGluZXMuemVybz09bnVsbCl7cy5saW5lcy56ZXJvPSEhcy5saW5lcy5maWxsfXMueGF4aXM9Z2V0T3JDcmVhdGVBeGlzKHhheGVzLGF4aXNOdW1iZXIocyxcInhcIikpO3MueWF4aXM9Z2V0T3JDcmVhdGVBeGlzKHlheGVzLGF4aXNOdW1iZXIocyxcInlcIikpfX1mdW5jdGlvbiBwcm9jZXNzRGF0YSgpe3ZhciB0b3BTZW50cnk9TnVtYmVyLlBPU0lUSVZFX0lORklOSVRZLGJvdHRvbVNlbnRyeT1OdW1iZXIuTkVHQVRJVkVfSU5GSU5JVFksZmFrZUluZmluaXR5PU51bWJlci5NQVhfVkFMVUUsaSxqLGssbSxsZW5ndGgscyxwb2ludHMscHMseCx5LGF4aXMsdmFsLGYscCxkYXRhLGZvcm1hdDtmdW5jdGlvbiB1cGRhdGVBeGlzKGF4aXMsbWluLG1heCl7aWYobWluPGF4aXMuZGF0YW1pbiYmbWluIT0tZmFrZUluZmluaXR5KWF4aXMuZGF0YW1pbj1taW47aWYobWF4PmF4aXMuZGF0YW1heCYmbWF4IT1mYWtlSW5maW5pdHkpYXhpcy5kYXRhbWF4PW1heH0kLmVhY2goYWxsQXhlcygpLGZ1bmN0aW9uKF8sYXhpcyl7YXhpcy5kYXRhbWluPXRvcFNlbnRyeTtheGlzLmRhdGFtYXg9Ym90dG9tU2VudHJ5O2F4aXMudXNlZD1mYWxzZX0pO2ZvcihpPTA7aTxzZXJpZXMubGVuZ3RoOysraSl7cz1zZXJpZXNbaV07cy5kYXRhcG9pbnRzPXtwb2ludHM6W119O2V4ZWN1dGVIb29rcyhob29rcy5wcm9jZXNzUmF3RGF0YSxbcyxzLmRhdGEscy5kYXRhcG9pbnRzXSl9Zm9yKGk9MDtpPHNlcmllcy5sZW5ndGg7KytpKXtzPXNlcmllc1tpXTtkYXRhPXMuZGF0YTtmb3JtYXQ9cy5kYXRhcG9pbnRzLmZvcm1hdDtpZighZm9ybWF0KXtmb3JtYXQ9W107Zm9ybWF0LnB1c2goe3g6dHJ1ZSxudW1iZXI6dHJ1ZSxyZXF1aXJlZDp0cnVlfSk7Zm9ybWF0LnB1c2goe3k6dHJ1ZSxudW1iZXI6dHJ1ZSxyZXF1aXJlZDp0cnVlfSk7aWYocy5iYXJzLnNob3d8fHMubGluZXMuc2hvdyYmcy5saW5lcy5maWxsKXt2YXIgYXV0b3NjYWxlPSEhKHMuYmFycy5zaG93JiZzLmJhcnMuemVyb3x8cy5saW5lcy5zaG93JiZzLmxpbmVzLnplcm8pO2Zvcm1hdC5wdXNoKHt5OnRydWUsbnVtYmVyOnRydWUscmVxdWlyZWQ6ZmFsc2UsZGVmYXVsdFZhbHVlOjAsYXV0b3NjYWxlOmF1dG9zY2FsZX0pO2lmKHMuYmFycy5ob3Jpem9udGFsKXtkZWxldGUgZm9ybWF0W2Zvcm1hdC5sZW5ndGgtMV0ueTtmb3JtYXRbZm9ybWF0Lmxlbmd0aC0xXS54PXRydWV9fXMuZGF0YXBvaW50cy5mb3JtYXQ9Zm9ybWF0fWlmKHMuZGF0YXBvaW50cy5wb2ludHNpemUhPW51bGwpY29udGludWU7cy5kYXRhcG9pbnRzLnBvaW50c2l6ZT1mb3JtYXQubGVuZ3RoO3BzPXMuZGF0YXBvaW50cy5wb2ludHNpemU7cG9pbnRzPXMuZGF0YXBvaW50cy5wb2ludHM7dmFyIGluc2VydFN0ZXBzPXMubGluZXMuc2hvdyYmcy5saW5lcy5zdGVwcztzLnhheGlzLnVzZWQ9cy55YXhpcy51c2VkPXRydWU7Zm9yKGo9az0wO2o8ZGF0YS5sZW5ndGg7KytqLGsrPXBzKXtwPWRhdGFbal07dmFyIG51bGxpZnk9cD09bnVsbDtpZighbnVsbGlmeSl7Zm9yKG09MDttPHBzOysrbSl7dmFsPXBbbV07Zj1mb3JtYXRbbV07aWYoZil7aWYoZi5udW1iZXImJnZhbCE9bnVsbCl7dmFsPSt2YWw7aWYoaXNOYU4odmFsKSl2YWw9bnVsbDtlbHNlIGlmKHZhbD09SW5maW5pdHkpdmFsPWZha2VJbmZpbml0eTtlbHNlIGlmKHZhbD09LUluZmluaXR5KXZhbD0tZmFrZUluZmluaXR5fWlmKHZhbD09bnVsbCl7aWYoZi5yZXF1aXJlZCludWxsaWZ5PXRydWU7aWYoZi5kZWZhdWx0VmFsdWUhPW51bGwpdmFsPWYuZGVmYXVsdFZhbHVlfX1wb2ludHNbayttXT12YWx9fWlmKG51bGxpZnkpe2ZvcihtPTA7bTxwczsrK20pe3ZhbD1wb2ludHNbayttXTtpZih2YWwhPW51bGwpe2Y9Zm9ybWF0W21dO2lmKGYuYXV0b3NjYWxlIT09ZmFsc2Upe2lmKGYueCl7dXBkYXRlQXhpcyhzLnhheGlzLHZhbCx2YWwpfWlmKGYueSl7dXBkYXRlQXhpcyhzLnlheGlzLHZhbCx2YWwpfX19cG9pbnRzW2srbV09bnVsbH19ZWxzZXtpZihpbnNlcnRTdGVwcyYmaz4wJiZwb2ludHNbay1wc10hPW51bGwmJnBvaW50c1trLXBzXSE9cG9pbnRzW2tdJiZwb2ludHNbay1wcysxXSE9cG9pbnRzW2srMV0pe2ZvcihtPTA7bTxwczsrK20pcG9pbnRzW2srcHMrbV09cG9pbnRzW2srbV07cG9pbnRzW2srMV09cG9pbnRzW2stcHMrMV07ays9cHN9fX19Zm9yKGk9MDtpPHNlcmllcy5sZW5ndGg7KytpKXtzPXNlcmllc1tpXTtleGVjdXRlSG9va3MoaG9va3MucHJvY2Vzc0RhdGFwb2ludHMsW3Mscy5kYXRhcG9pbnRzXSl9Zm9yKGk9MDtpPHNlcmllcy5sZW5ndGg7KytpKXtzPXNlcmllc1tpXTtwb2ludHM9cy5kYXRhcG9pbnRzLnBvaW50cztwcz1zLmRhdGFwb2ludHMucG9pbnRzaXplO2Zvcm1hdD1zLmRhdGFwb2ludHMuZm9ybWF0O3ZhciB4bWluPXRvcFNlbnRyeSx5bWluPXRvcFNlbnRyeSx4bWF4PWJvdHRvbVNlbnRyeSx5bWF4PWJvdHRvbVNlbnRyeTtmb3Ioaj0wO2o8cG9pbnRzLmxlbmd0aDtqKz1wcyl7aWYocG9pbnRzW2pdPT1udWxsKWNvbnRpbnVlO2ZvcihtPTA7bTxwczsrK20pe3ZhbD1wb2ludHNbaittXTtmPWZvcm1hdFttXTtpZighZnx8Zi5hdXRvc2NhbGU9PT1mYWxzZXx8dmFsPT1mYWtlSW5maW5pdHl8fHZhbD09LWZha2VJbmZpbml0eSljb250aW51ZTtpZihmLngpe2lmKHZhbDx4bWluKXhtaW49dmFsO2lmKHZhbD54bWF4KXhtYXg9dmFsfWlmKGYueSl7aWYodmFsPHltaW4peW1pbj12YWw7aWYodmFsPnltYXgpeW1heD12YWx9fX1pZihzLmJhcnMuc2hvdyl7dmFyIGRlbHRhO3N3aXRjaChzLmJhcnMuYWxpZ24pe2Nhc2VcImxlZnRcIjpkZWx0YT0wO2JyZWFrO2Nhc2VcInJpZ2h0XCI6ZGVsdGE9LXMuYmFycy5iYXJXaWR0aDticmVhaztkZWZhdWx0OmRlbHRhPS1zLmJhcnMuYmFyV2lkdGgvMn1pZihzLmJhcnMuaG9yaXpvbnRhbCl7eW1pbis9ZGVsdGE7eW1heCs9ZGVsdGErcy5iYXJzLmJhcldpZHRofWVsc2V7eG1pbis9ZGVsdGE7eG1heCs9ZGVsdGErcy5iYXJzLmJhcldpZHRofX11cGRhdGVBeGlzKHMueGF4aXMseG1pbix4bWF4KTt1cGRhdGVBeGlzKHMueWF4aXMseW1pbix5bWF4KX0kLmVhY2goYWxsQXhlcygpLGZ1bmN0aW9uKF8sYXhpcyl7aWYoYXhpcy5kYXRhbWluPT10b3BTZW50cnkpYXhpcy5kYXRhbWluPW51bGw7aWYoYXhpcy5kYXRhbWF4PT1ib3R0b21TZW50cnkpYXhpcy5kYXRhbWF4PW51bGx9KX1mdW5jdGlvbiBzZXR1cENhbnZhc2VzKCl7cGxhY2Vob2xkZXIuY3NzKFwicGFkZGluZ1wiLDApLmNoaWxkcmVuKCkuZmlsdGVyKGZ1bmN0aW9uKCl7cmV0dXJuISQodGhpcykuaGFzQ2xhc3MoXCJmbG90LW92ZXJsYXlcIikmJiEkKHRoaXMpLmhhc0NsYXNzKFwiZmxvdC1iYXNlXCIpfSkucmVtb3ZlKCk7aWYocGxhY2Vob2xkZXIuY3NzKFwicG9zaXRpb25cIik9PVwic3RhdGljXCIpcGxhY2Vob2xkZXIuY3NzKFwicG9zaXRpb25cIixcInJlbGF0aXZlXCIpO3N1cmZhY2U9bmV3IENhbnZhcyhcImZsb3QtYmFzZVwiLHBsYWNlaG9sZGVyKTtvdmVybGF5PW5ldyBDYW52YXMoXCJmbG90LW92ZXJsYXlcIixwbGFjZWhvbGRlcik7Y3R4PXN1cmZhY2UuY29udGV4dDtvY3R4PW92ZXJsYXkuY29udGV4dDtldmVudEhvbGRlcj0kKG92ZXJsYXkuZWxlbWVudCkudW5iaW5kKCk7dmFyIGV4aXN0aW5nPXBsYWNlaG9sZGVyLmRhdGEoXCJwbG90XCIpO2lmKGV4aXN0aW5nKXtleGlzdGluZy5zaHV0ZG93bigpO292ZXJsYXkuY2xlYXIoKX1wbGFjZWhvbGRlci5kYXRhKFwicGxvdFwiLHBsb3QpfWZ1bmN0aW9uIGJpbmRFdmVudHMoKXtpZihvcHRpb25zLmdyaWQuaG92ZXJhYmxlKXtldmVudEhvbGRlci5tb3VzZW1vdmUob25Nb3VzZU1vdmUpO2V2ZW50SG9sZGVyLmJpbmQoXCJtb3VzZWxlYXZlXCIsb25Nb3VzZUxlYXZlKX1pZihvcHRpb25zLmdyaWQuY2xpY2thYmxlKWV2ZW50SG9sZGVyLmNsaWNrKG9uQ2xpY2spO2V4ZWN1dGVIb29rcyhob29rcy5iaW5kRXZlbnRzLFtldmVudEhvbGRlcl0pfWZ1bmN0aW9uIHNodXRkb3duKCl7aWYocmVkcmF3VGltZW91dCljbGVhclRpbWVvdXQocmVkcmF3VGltZW91dCk7ZXZlbnRIb2xkZXIudW5iaW5kKFwibW91c2Vtb3ZlXCIsb25Nb3VzZU1vdmUpO2V2ZW50SG9sZGVyLnVuYmluZChcIm1vdXNlbGVhdmVcIixvbk1vdXNlTGVhdmUpO2V2ZW50SG9sZGVyLnVuYmluZChcImNsaWNrXCIsb25DbGljayk7ZXhlY3V0ZUhvb2tzKGhvb2tzLnNodXRkb3duLFtldmVudEhvbGRlcl0pfWZ1bmN0aW9uIHNldFRyYW5zZm9ybWF0aW9uSGVscGVycyhheGlzKXtmdW5jdGlvbiBpZGVudGl0eSh4KXtyZXR1cm4geH12YXIgcyxtLHQ9YXhpcy5vcHRpb25zLnRyYW5zZm9ybXx8aWRlbnRpdHksaXQ9YXhpcy5vcHRpb25zLmludmVyc2VUcmFuc2Zvcm07aWYoYXhpcy5kaXJlY3Rpb249PVwieFwiKXtzPWF4aXMuc2NhbGU9cGxvdFdpZHRoL01hdGguYWJzKHQoYXhpcy5tYXgpLXQoYXhpcy5taW4pKTttPU1hdGgubWluKHQoYXhpcy5tYXgpLHQoYXhpcy5taW4pKX1lbHNle3M9YXhpcy5zY2FsZT1wbG90SGVpZ2h0L01hdGguYWJzKHQoYXhpcy5tYXgpLXQoYXhpcy5taW4pKTtzPS1zO209TWF0aC5tYXgodChheGlzLm1heCksdChheGlzLm1pbikpfWlmKHQ9PWlkZW50aXR5KWF4aXMucDJjPWZ1bmN0aW9uKHApe3JldHVybihwLW0pKnN9O2Vsc2UgYXhpcy5wMmM9ZnVuY3Rpb24ocCl7cmV0dXJuKHQocCktbSkqc307aWYoIWl0KWF4aXMuYzJwPWZ1bmN0aW9uKGMpe3JldHVybiBtK2Mvc307ZWxzZSBheGlzLmMycD1mdW5jdGlvbihjKXtyZXR1cm4gaXQobStjL3MpfX1mdW5jdGlvbiBtZWFzdXJlVGlja0xhYmVscyhheGlzKXt2YXIgb3B0cz1heGlzLm9wdGlvbnMsdGlja3M9YXhpcy50aWNrc3x8W10sbGFiZWxXaWR0aD1vcHRzLmxhYmVsV2lkdGh8fDAsbGFiZWxIZWlnaHQ9b3B0cy5sYWJlbEhlaWdodHx8MCxtYXhXaWR0aD1sYWJlbFdpZHRofHwoYXhpcy5kaXJlY3Rpb249PVwieFwiP01hdGguZmxvb3Ioc3VyZmFjZS53aWR0aC8odGlja3MubGVuZ3RofHwxKSk6bnVsbCksbGVnYWN5U3R5bGVzPWF4aXMuZGlyZWN0aW9uK1wiQXhpcyBcIitheGlzLmRpcmVjdGlvbitheGlzLm4rXCJBeGlzXCIsbGF5ZXI9XCJmbG90LVwiK2F4aXMuZGlyZWN0aW9uK1wiLWF4aXMgZmxvdC1cIitheGlzLmRpcmVjdGlvbitheGlzLm4rXCItYXhpcyBcIitsZWdhY3lTdHlsZXMsZm9udD1vcHRzLmZvbnR8fFwiZmxvdC10aWNrLWxhYmVsIHRpY2tMYWJlbFwiO2Zvcih2YXIgaT0wO2k8dGlja3MubGVuZ3RoOysraSl7dmFyIHQ9dGlja3NbaV07aWYoIXQubGFiZWwpY29udGludWU7dmFyIGluZm89c3VyZmFjZS5nZXRUZXh0SW5mbyhsYXllcix0LmxhYmVsLGZvbnQsbnVsbCxtYXhXaWR0aCk7bGFiZWxXaWR0aD1NYXRoLm1heChsYWJlbFdpZHRoLGluZm8ud2lkdGgpO2xhYmVsSGVpZ2h0PU1hdGgubWF4KGxhYmVsSGVpZ2h0LGluZm8uaGVpZ2h0KX1heGlzLmxhYmVsV2lkdGg9b3B0cy5sYWJlbFdpZHRofHxsYWJlbFdpZHRoO2F4aXMubGFiZWxIZWlnaHQ9b3B0cy5sYWJlbEhlaWdodHx8bGFiZWxIZWlnaHR9ZnVuY3Rpb24gYWxsb2NhdGVBeGlzQm94Rmlyc3RQaGFzZShheGlzKXt2YXIgbHc9YXhpcy5sYWJlbFdpZHRoLGxoPWF4aXMubGFiZWxIZWlnaHQscG9zPWF4aXMub3B0aW9ucy5wb3NpdGlvbixpc1hBeGlzPWF4aXMuZGlyZWN0aW9uPT09XCJ4XCIsdGlja0xlbmd0aD1heGlzLm9wdGlvbnMudGlja0xlbmd0aCxheGlzTWFyZ2luPW9wdGlvbnMuZ3JpZC5heGlzTWFyZ2luLHBhZGRpbmc9b3B0aW9ucy5ncmlkLmxhYmVsTWFyZ2luLGlubmVybW9zdD10cnVlLG91dGVybW9zdD10cnVlLGZpcnN0PXRydWUsZm91bmQ9ZmFsc2U7JC5lYWNoKGlzWEF4aXM/eGF4ZXM6eWF4ZXMsZnVuY3Rpb24oaSxhKXtpZihhJiYoYS5zaG93fHxhLnJlc2VydmVTcGFjZSkpe2lmKGE9PT1heGlzKXtmb3VuZD10cnVlfWVsc2UgaWYoYS5vcHRpb25zLnBvc2l0aW9uPT09cG9zKXtpZihmb3VuZCl7b3V0ZXJtb3N0PWZhbHNlfWVsc2V7aW5uZXJtb3N0PWZhbHNlfX1pZighZm91bmQpe2ZpcnN0PWZhbHNlfX19KTtpZihvdXRlcm1vc3Qpe2F4aXNNYXJnaW49MH1pZih0aWNrTGVuZ3RoPT1udWxsKXt0aWNrTGVuZ3RoPWZpcnN0P1wiZnVsbFwiOjV9aWYoIWlzTmFOKCt0aWNrTGVuZ3RoKSlwYWRkaW5nKz0rdGlja0xlbmd0aDtpZihpc1hBeGlzKXtsaCs9cGFkZGluZztpZihwb3M9PVwiYm90dG9tXCIpe3Bsb3RPZmZzZXQuYm90dG9tKz1saCtheGlzTWFyZ2luO2F4aXMuYm94PXt0b3A6c3VyZmFjZS5oZWlnaHQtcGxvdE9mZnNldC5ib3R0b20saGVpZ2h0OmxofX1lbHNle2F4aXMuYm94PXt0b3A6cGxvdE9mZnNldC50b3ArYXhpc01hcmdpbixoZWlnaHQ6bGh9O3Bsb3RPZmZzZXQudG9wKz1saCtheGlzTWFyZ2lufX1lbHNle2x3Kz1wYWRkaW5nO2lmKHBvcz09XCJsZWZ0XCIpe2F4aXMuYm94PXtsZWZ0OnBsb3RPZmZzZXQubGVmdCtheGlzTWFyZ2luLHdpZHRoOmx3fTtwbG90T2Zmc2V0LmxlZnQrPWx3K2F4aXNNYXJnaW59ZWxzZXtwbG90T2Zmc2V0LnJpZ2h0Kz1sdytheGlzTWFyZ2luO2F4aXMuYm94PXtsZWZ0OnN1cmZhY2Uud2lkdGgtcGxvdE9mZnNldC5yaWdodCx3aWR0aDpsd319fWF4aXMucG9zaXRpb249cG9zO2F4aXMudGlja0xlbmd0aD10aWNrTGVuZ3RoO2F4aXMuYm94LnBhZGRpbmc9cGFkZGluZztheGlzLmlubmVybW9zdD1pbm5lcm1vc3R9ZnVuY3Rpb24gYWxsb2NhdGVBeGlzQm94U2Vjb25kUGhhc2UoYXhpcyl7aWYoYXhpcy5kaXJlY3Rpb249PVwieFwiKXtheGlzLmJveC5sZWZ0PXBsb3RPZmZzZXQubGVmdC1heGlzLmxhYmVsV2lkdGgvMjtheGlzLmJveC53aWR0aD1zdXJmYWNlLndpZHRoLXBsb3RPZmZzZXQubGVmdC1wbG90T2Zmc2V0LnJpZ2h0K2F4aXMubGFiZWxXaWR0aH1lbHNle2F4aXMuYm94LnRvcD1wbG90T2Zmc2V0LnRvcC1heGlzLmxhYmVsSGVpZ2h0LzI7YXhpcy5ib3guaGVpZ2h0PXN1cmZhY2UuaGVpZ2h0LXBsb3RPZmZzZXQuYm90dG9tLXBsb3RPZmZzZXQudG9wK2F4aXMubGFiZWxIZWlnaHR9fWZ1bmN0aW9uIGFkanVzdExheW91dEZvclRoaW5nc1N0aWNraW5nT3V0KCl7dmFyIG1pbk1hcmdpbj1vcHRpb25zLmdyaWQubWluQm9yZGVyTWFyZ2luLGF4aXMsaTtpZihtaW5NYXJnaW49PW51bGwpe21pbk1hcmdpbj0wO2ZvcihpPTA7aTxzZXJpZXMubGVuZ3RoOysraSltaW5NYXJnaW49TWF0aC5tYXgobWluTWFyZ2luLDIqKHNlcmllc1tpXS5wb2ludHMucmFkaXVzK3Nlcmllc1tpXS5wb2ludHMubGluZVdpZHRoLzIpKX12YXIgbWFyZ2lucz17bGVmdDptaW5NYXJnaW4scmlnaHQ6bWluTWFyZ2luLHRvcDptaW5NYXJnaW4sYm90dG9tOm1pbk1hcmdpbn07JC5lYWNoKGFsbEF4ZXMoKSxmdW5jdGlvbihfLGF4aXMpe2lmKGF4aXMucmVzZXJ2ZVNwYWNlJiZheGlzLnRpY2tzJiZheGlzLnRpY2tzLmxlbmd0aCl7aWYoYXhpcy5kaXJlY3Rpb249PT1cInhcIil7bWFyZ2lucy5sZWZ0PU1hdGgubWF4KG1hcmdpbnMubGVmdCxheGlzLmxhYmVsV2lkdGgvMik7bWFyZ2lucy5yaWdodD1NYXRoLm1heChtYXJnaW5zLnJpZ2h0LGF4aXMubGFiZWxXaWR0aC8yKX1lbHNle21hcmdpbnMuYm90dG9tPU1hdGgubWF4KG1hcmdpbnMuYm90dG9tLGF4aXMubGFiZWxIZWlnaHQvMik7bWFyZ2lucy50b3A9TWF0aC5tYXgobWFyZ2lucy50b3AsYXhpcy5sYWJlbEhlaWdodC8yKX19fSk7cGxvdE9mZnNldC5sZWZ0PU1hdGguY2VpbChNYXRoLm1heChtYXJnaW5zLmxlZnQscGxvdE9mZnNldC5sZWZ0KSk7cGxvdE9mZnNldC5yaWdodD1NYXRoLmNlaWwoTWF0aC5tYXgobWFyZ2lucy5yaWdodCxwbG90T2Zmc2V0LnJpZ2h0KSk7cGxvdE9mZnNldC50b3A9TWF0aC5jZWlsKE1hdGgubWF4KG1hcmdpbnMudG9wLHBsb3RPZmZzZXQudG9wKSk7cGxvdE9mZnNldC5ib3R0b209TWF0aC5jZWlsKE1hdGgubWF4KG1hcmdpbnMuYm90dG9tLHBsb3RPZmZzZXQuYm90dG9tKSl9ZnVuY3Rpb24gc2V0dXBHcmlkKCl7dmFyIGksYXhlcz1hbGxBeGVzKCksc2hvd0dyaWQ9b3B0aW9ucy5ncmlkLnNob3c7Zm9yKHZhciBhIGluIHBsb3RPZmZzZXQpe3ZhciBtYXJnaW49b3B0aW9ucy5ncmlkLm1hcmdpbnx8MDtwbG90T2Zmc2V0W2FdPXR5cGVvZiBtYXJnaW49PVwibnVtYmVyXCI/bWFyZ2luOm1hcmdpblthXXx8MH1leGVjdXRlSG9va3MoaG9va3MucHJvY2Vzc09mZnNldCxbcGxvdE9mZnNldF0pO2Zvcih2YXIgYSBpbiBwbG90T2Zmc2V0KXtpZih0eXBlb2Ygb3B0aW9ucy5ncmlkLmJvcmRlcldpZHRoPT1cIm9iamVjdFwiKXtwbG90T2Zmc2V0W2FdKz1zaG93R3JpZD9vcHRpb25zLmdyaWQuYm9yZGVyV2lkdGhbYV06MH1lbHNle3Bsb3RPZmZzZXRbYV0rPXNob3dHcmlkP29wdGlvbnMuZ3JpZC5ib3JkZXJXaWR0aDowfX0kLmVhY2goYXhlcyxmdW5jdGlvbihfLGF4aXMpe3ZhciBheGlzT3B0cz1heGlzLm9wdGlvbnM7YXhpcy5zaG93PWF4aXNPcHRzLnNob3c9PW51bGw/YXhpcy51c2VkOmF4aXNPcHRzLnNob3c7YXhpcy5yZXNlcnZlU3BhY2U9YXhpc09wdHMucmVzZXJ2ZVNwYWNlPT1udWxsP2F4aXMuc2hvdzpheGlzT3B0cy5yZXNlcnZlU3BhY2U7c2V0UmFuZ2UoYXhpcyl9KTtpZihzaG93R3JpZCl7dmFyIGFsbG9jYXRlZEF4ZXM9JC5ncmVwKGF4ZXMsZnVuY3Rpb24oYXhpcyl7cmV0dXJuIGF4aXMuc2hvd3x8YXhpcy5yZXNlcnZlU3BhY2V9KTskLmVhY2goYWxsb2NhdGVkQXhlcyxmdW5jdGlvbihfLGF4aXMpe3NldHVwVGlja0dlbmVyYXRpb24oYXhpcyk7c2V0VGlja3MoYXhpcyk7c25hcFJhbmdlVG9UaWNrcyhheGlzLGF4aXMudGlja3MpO21lYXN1cmVUaWNrTGFiZWxzKGF4aXMpfSk7Zm9yKGk9YWxsb2NhdGVkQXhlcy5sZW5ndGgtMTtpPj0wOy0taSlhbGxvY2F0ZUF4aXNCb3hGaXJzdFBoYXNlKGFsbG9jYXRlZEF4ZXNbaV0pO2FkanVzdExheW91dEZvclRoaW5nc1N0aWNraW5nT3V0KCk7JC5lYWNoKGFsbG9jYXRlZEF4ZXMsZnVuY3Rpb24oXyxheGlzKXthbGxvY2F0ZUF4aXNCb3hTZWNvbmRQaGFzZShheGlzKX0pfXBsb3RXaWR0aD1zdXJmYWNlLndpZHRoLXBsb3RPZmZzZXQubGVmdC1wbG90T2Zmc2V0LnJpZ2h0O3Bsb3RIZWlnaHQ9c3VyZmFjZS5oZWlnaHQtcGxvdE9mZnNldC5ib3R0b20tcGxvdE9mZnNldC50b3A7JC5lYWNoKGF4ZXMsZnVuY3Rpb24oXyxheGlzKXtzZXRUcmFuc2Zvcm1hdGlvbkhlbHBlcnMoYXhpcyl9KTtpZihzaG93R3JpZCl7ZHJhd0F4aXNMYWJlbHMoKX1pbnNlcnRMZWdlbmQoKX1mdW5jdGlvbiBzZXRSYW5nZShheGlzKXt2YXIgb3B0cz1heGlzLm9wdGlvbnMsbWluPSsob3B0cy5taW4hPW51bGw/b3B0cy5taW46YXhpcy5kYXRhbWluKSxtYXg9KyhvcHRzLm1heCE9bnVsbD9vcHRzLm1heDpheGlzLmRhdGFtYXgpLGRlbHRhPW1heC1taW47aWYoZGVsdGE9PTApe3ZhciB3aWRlbj1tYXg9PTA/MTouMDE7aWYob3B0cy5taW49PW51bGwpbWluLT13aWRlbjtpZihvcHRzLm1heD09bnVsbHx8b3B0cy5taW4hPW51bGwpbWF4Kz13aWRlbn1lbHNle3ZhciBtYXJnaW49b3B0cy5hdXRvc2NhbGVNYXJnaW47aWYobWFyZ2luIT1udWxsKXtpZihvcHRzLm1pbj09bnVsbCl7bWluLT1kZWx0YSptYXJnaW47aWYobWluPDAmJmF4aXMuZGF0YW1pbiE9bnVsbCYmYXhpcy5kYXRhbWluPj0wKW1pbj0wfWlmKG9wdHMubWF4PT1udWxsKXttYXgrPWRlbHRhKm1hcmdpbjtpZihtYXg+MCYmYXhpcy5kYXRhbWF4IT1udWxsJiZheGlzLmRhdGFtYXg8PTApbWF4PTB9fX1heGlzLm1pbj1taW47YXhpcy5tYXg9bWF4fWZ1bmN0aW9uIHNldHVwVGlja0dlbmVyYXRpb24oYXhpcyl7dmFyIG9wdHM9YXhpcy5vcHRpb25zO3ZhciBub1RpY2tzO2lmKHR5cGVvZiBvcHRzLnRpY2tzPT1cIm51bWJlclwiJiZvcHRzLnRpY2tzPjApbm9UaWNrcz1vcHRzLnRpY2tzO2Vsc2Ugbm9UaWNrcz0uMypNYXRoLnNxcnQoYXhpcy5kaXJlY3Rpb249PVwieFwiP3N1cmZhY2Uud2lkdGg6c3VyZmFjZS5oZWlnaHQpO3ZhciBkZWx0YT0oYXhpcy5tYXgtYXhpcy5taW4pL25vVGlja3MsZGVjPS1NYXRoLmZsb29yKE1hdGgubG9nKGRlbHRhKS9NYXRoLkxOMTApLG1heERlYz1vcHRzLnRpY2tEZWNpbWFscztpZihtYXhEZWMhPW51bGwmJmRlYz5tYXhEZWMpe2RlYz1tYXhEZWN9dmFyIG1hZ249TWF0aC5wb3coMTAsLWRlYyksbm9ybT1kZWx0YS9tYWduLHNpemU7aWYobm9ybTwxLjUpe3NpemU9MX1lbHNlIGlmKG5vcm08Myl7c2l6ZT0yO2lmKG5vcm0+Mi4yNSYmKG1heERlYz09bnVsbHx8ZGVjKzE8PW1heERlYykpe3NpemU9Mi41OysrZGVjfX1lbHNlIGlmKG5vcm08Ny41KXtzaXplPTV9ZWxzZXtzaXplPTEwfXNpemUqPW1hZ247aWYob3B0cy5taW5UaWNrU2l6ZSE9bnVsbCYmc2l6ZTxvcHRzLm1pblRpY2tTaXplKXtzaXplPW9wdHMubWluVGlja1NpemV9YXhpcy5kZWx0YT1kZWx0YTtheGlzLnRpY2tEZWNpbWFscz1NYXRoLm1heCgwLG1heERlYyE9bnVsbD9tYXhEZWM6ZGVjKTtheGlzLnRpY2tTaXplPW9wdHMudGlja1NpemV8fHNpemU7aWYob3B0cy5tb2RlPT1cInRpbWVcIiYmIWF4aXMudGlja0dlbmVyYXRvcil7dGhyb3cgbmV3IEVycm9yKFwiVGltZSBtb2RlIHJlcXVpcmVzIHRoZSBmbG90LnRpbWUgcGx1Z2luLlwiKX1pZighYXhpcy50aWNrR2VuZXJhdG9yKXtheGlzLnRpY2tHZW5lcmF0b3I9ZnVuY3Rpb24oYXhpcyl7dmFyIHRpY2tzPVtdLHN0YXJ0PWZsb29ySW5CYXNlKGF4aXMubWluLGF4aXMudGlja1NpemUpLGk9MCx2PU51bWJlci5OYU4scHJldjtkb3twcmV2PXY7dj1zdGFydCtpKmF4aXMudGlja1NpemU7dGlja3MucHVzaCh2KTsrK2l9d2hpbGUodjxheGlzLm1heCYmdiE9cHJldik7cmV0dXJuIHRpY2tzfTtheGlzLnRpY2tGb3JtYXR0ZXI9ZnVuY3Rpb24odmFsdWUsYXhpcyl7dmFyIGZhY3Rvcj1heGlzLnRpY2tEZWNpbWFscz9NYXRoLnBvdygxMCxheGlzLnRpY2tEZWNpbWFscyk6MTt2YXIgZm9ybWF0dGVkPVwiXCIrTWF0aC5yb3VuZCh2YWx1ZSpmYWN0b3IpL2ZhY3RvcjtpZihheGlzLnRpY2tEZWNpbWFscyE9bnVsbCl7dmFyIGRlY2ltYWw9Zm9ybWF0dGVkLmluZGV4T2YoXCIuXCIpO3ZhciBwcmVjaXNpb249ZGVjaW1hbD09LTE/MDpmb3JtYXR0ZWQubGVuZ3RoLWRlY2ltYWwtMTtpZihwcmVjaXNpb248YXhpcy50aWNrRGVjaW1hbHMpe3JldHVybihwcmVjaXNpb24/Zm9ybWF0dGVkOmZvcm1hdHRlZCtcIi5cIikrKFwiXCIrZmFjdG9yKS5zdWJzdHIoMSxheGlzLnRpY2tEZWNpbWFscy1wcmVjaXNpb24pfX1yZXR1cm4gZm9ybWF0dGVkfX1pZigkLmlzRnVuY3Rpb24ob3B0cy50aWNrRm9ybWF0dGVyKSlheGlzLnRpY2tGb3JtYXR0ZXI9ZnVuY3Rpb24odixheGlzKXtyZXR1cm5cIlwiK29wdHMudGlja0Zvcm1hdHRlcih2LGF4aXMpfTtpZihvcHRzLmFsaWduVGlja3NXaXRoQXhpcyE9bnVsbCl7dmFyIG90aGVyQXhpcz0oYXhpcy5kaXJlY3Rpb249PVwieFwiP3hheGVzOnlheGVzKVtvcHRzLmFsaWduVGlja3NXaXRoQXhpcy0xXTtpZihvdGhlckF4aXMmJm90aGVyQXhpcy51c2VkJiZvdGhlckF4aXMhPWF4aXMpe3ZhciBuaWNlVGlja3M9YXhpcy50aWNrR2VuZXJhdG9yKGF4aXMpO2lmKG5pY2VUaWNrcy5sZW5ndGg+MCl7aWYob3B0cy5taW49PW51bGwpYXhpcy5taW49TWF0aC5taW4oYXhpcy5taW4sbmljZVRpY2tzWzBdKTtpZihvcHRzLm1heD09bnVsbCYmbmljZVRpY2tzLmxlbmd0aD4xKWF4aXMubWF4PU1hdGgubWF4KGF4aXMubWF4LG5pY2VUaWNrc1tuaWNlVGlja3MubGVuZ3RoLTFdKX1heGlzLnRpY2tHZW5lcmF0b3I9ZnVuY3Rpb24oYXhpcyl7dmFyIHRpY2tzPVtdLHYsaTtmb3IoaT0wO2k8b3RoZXJBeGlzLnRpY2tzLmxlbmd0aDsrK2kpe3Y9KG90aGVyQXhpcy50aWNrc1tpXS52LW90aGVyQXhpcy5taW4pLyhvdGhlckF4aXMubWF4LW90aGVyQXhpcy5taW4pO3Y9YXhpcy5taW4rdiooYXhpcy5tYXgtYXhpcy5taW4pO3RpY2tzLnB1c2godil9cmV0dXJuIHRpY2tzfTtpZighYXhpcy5tb2RlJiZvcHRzLnRpY2tEZWNpbWFscz09bnVsbCl7dmFyIGV4dHJhRGVjPU1hdGgubWF4KDAsLU1hdGguZmxvb3IoTWF0aC5sb2coYXhpcy5kZWx0YSkvTWF0aC5MTjEwKSsxKSx0cz1heGlzLnRpY2tHZW5lcmF0b3IoYXhpcyk7aWYoISh0cy5sZW5ndGg+MSYmL1xcLi4qMCQvLnRlc3QoKHRzWzFdLXRzWzBdKS50b0ZpeGVkKGV4dHJhRGVjKSkpKWF4aXMudGlja0RlY2ltYWxzPWV4dHJhRGVjfX19fWZ1bmN0aW9uIHNldFRpY2tzKGF4aXMpe3ZhciBvdGlja3M9YXhpcy5vcHRpb25zLnRpY2tzLHRpY2tzPVtdO2lmKG90aWNrcz09bnVsbHx8dHlwZW9mIG90aWNrcz09XCJudW1iZXJcIiYmb3RpY2tzPjApdGlja3M9YXhpcy50aWNrR2VuZXJhdG9yKGF4aXMpO2Vsc2UgaWYob3RpY2tzKXtpZigkLmlzRnVuY3Rpb24ob3RpY2tzKSl0aWNrcz1vdGlja3MoYXhpcyk7ZWxzZSB0aWNrcz1vdGlja3N9dmFyIGksdjtheGlzLnRpY2tzPVtdO2ZvcihpPTA7aTx0aWNrcy5sZW5ndGg7KytpKXt2YXIgbGFiZWw9bnVsbDt2YXIgdD10aWNrc1tpXTtpZih0eXBlb2YgdD09XCJvYmplY3RcIil7dj0rdFswXTtpZih0Lmxlbmd0aD4xKWxhYmVsPXRbMV19ZWxzZSB2PSt0O2lmKGxhYmVsPT1udWxsKWxhYmVsPWF4aXMudGlja0Zvcm1hdHRlcih2LGF4aXMpO2lmKCFpc05hTih2KSlheGlzLnRpY2tzLnB1c2goe3Y6dixsYWJlbDpsYWJlbH0pfX1mdW5jdGlvbiBzbmFwUmFuZ2VUb1RpY2tzKGF4aXMsdGlja3Mpe2lmKGF4aXMub3B0aW9ucy5hdXRvc2NhbGVNYXJnaW4mJnRpY2tzLmxlbmd0aD4wKXtpZihheGlzLm9wdGlvbnMubWluPT1udWxsKWF4aXMubWluPU1hdGgubWluKGF4aXMubWluLHRpY2tzWzBdLnYpO2lmKGF4aXMub3B0aW9ucy5tYXg9PW51bGwmJnRpY2tzLmxlbmd0aD4xKWF4aXMubWF4PU1hdGgubWF4KGF4aXMubWF4LHRpY2tzW3RpY2tzLmxlbmd0aC0xXS52KX19ZnVuY3Rpb24gZHJhdygpe3N1cmZhY2UuY2xlYXIoKTtleGVjdXRlSG9va3MoaG9va3MuZHJhd0JhY2tncm91bmQsW2N0eF0pO3ZhciBncmlkPW9wdGlvbnMuZ3JpZDtpZihncmlkLnNob3cmJmdyaWQuYmFja2dyb3VuZENvbG9yKWRyYXdCYWNrZ3JvdW5kKCk7aWYoZ3JpZC5zaG93JiYhZ3JpZC5hYm92ZURhdGEpe2RyYXdHcmlkKCl9Zm9yKHZhciBpPTA7aTxzZXJpZXMubGVuZ3RoOysraSl7ZXhlY3V0ZUhvb2tzKGhvb2tzLmRyYXdTZXJpZXMsW2N0eCxzZXJpZXNbaV1dKTtkcmF3U2VyaWVzKHNlcmllc1tpXSl9ZXhlY3V0ZUhvb2tzKGhvb2tzLmRyYXcsW2N0eF0pO2lmKGdyaWQuc2hvdyYmZ3JpZC5hYm92ZURhdGEpe2RyYXdHcmlkKCl9c3VyZmFjZS5yZW5kZXIoKTt0cmlnZ2VyUmVkcmF3T3ZlcmxheSgpfWZ1bmN0aW9uIGV4dHJhY3RSYW5nZShyYW5nZXMsY29vcmQpe3ZhciBheGlzLGZyb20sdG8sa2V5LGF4ZXM9YWxsQXhlcygpO2Zvcih2YXIgaT0wO2k8YXhlcy5sZW5ndGg7KytpKXtheGlzPWF4ZXNbaV07aWYoYXhpcy5kaXJlY3Rpb249PWNvb3JkKXtrZXk9Y29vcmQrYXhpcy5uK1wiYXhpc1wiO2lmKCFyYW5nZXNba2V5XSYmYXhpcy5uPT0xKWtleT1jb29yZCtcImF4aXNcIjtpZihyYW5nZXNba2V5XSl7ZnJvbT1yYW5nZXNba2V5XS5mcm9tO3RvPXJhbmdlc1trZXldLnRvO2JyZWFrfX19aWYoIXJhbmdlc1trZXldKXtheGlzPWNvb3JkPT1cInhcIj94YXhlc1swXTp5YXhlc1swXTtmcm9tPXJhbmdlc1tjb29yZCtcIjFcIl07dG89cmFuZ2VzW2Nvb3JkK1wiMlwiXX1pZihmcm9tIT1udWxsJiZ0byE9bnVsbCYmZnJvbT50byl7dmFyIHRtcD1mcm9tO2Zyb209dG87dG89dG1wfXJldHVybntmcm9tOmZyb20sdG86dG8sYXhpczpheGlzfX1mdW5jdGlvbiBkcmF3QmFja2dyb3VuZCgpe2N0eC5zYXZlKCk7Y3R4LnRyYW5zbGF0ZShwbG90T2Zmc2V0LmxlZnQscGxvdE9mZnNldC50b3ApO2N0eC5maWxsU3R5bGU9Z2V0Q29sb3JPckdyYWRpZW50KG9wdGlvbnMuZ3JpZC5iYWNrZ3JvdW5kQ29sb3IscGxvdEhlaWdodCwwLFwicmdiYSgyNTUsIDI1NSwgMjU1LCAwKVwiKTtjdHguZmlsbFJlY3QoMCwwLHBsb3RXaWR0aCxwbG90SGVpZ2h0KTtjdHgucmVzdG9yZSgpfWZ1bmN0aW9uIGRyYXdHcmlkKCl7dmFyIGksYXhlcyxidyxiYztjdHguc2F2ZSgpO2N0eC50cmFuc2xhdGUocGxvdE9mZnNldC5sZWZ0LHBsb3RPZmZzZXQudG9wKTt2YXIgbWFya2luZ3M9b3B0aW9ucy5ncmlkLm1hcmtpbmdzO2lmKG1hcmtpbmdzKXtpZigkLmlzRnVuY3Rpb24obWFya2luZ3MpKXtheGVzPXBsb3QuZ2V0QXhlcygpO2F4ZXMueG1pbj1heGVzLnhheGlzLm1pbjtheGVzLnhtYXg9YXhlcy54YXhpcy5tYXg7YXhlcy55bWluPWF4ZXMueWF4aXMubWluO2F4ZXMueW1heD1heGVzLnlheGlzLm1heDttYXJraW5ncz1tYXJraW5ncyhheGVzKX1mb3IoaT0wO2k8bWFya2luZ3MubGVuZ3RoOysraSl7dmFyIG09bWFya2luZ3NbaV0seHJhbmdlPWV4dHJhY3RSYW5nZShtLFwieFwiKSx5cmFuZ2U9ZXh0cmFjdFJhbmdlKG0sXCJ5XCIpO2lmKHhyYW5nZS5mcm9tPT1udWxsKXhyYW5nZS5mcm9tPXhyYW5nZS5heGlzLm1pbjtpZih4cmFuZ2UudG89PW51bGwpeHJhbmdlLnRvPXhyYW5nZS5heGlzLm1heDtcclxuaWYoeXJhbmdlLmZyb209PW51bGwpeXJhbmdlLmZyb209eXJhbmdlLmF4aXMubWluO2lmKHlyYW5nZS50bz09bnVsbCl5cmFuZ2UudG89eXJhbmdlLmF4aXMubWF4O2lmKHhyYW5nZS50bzx4cmFuZ2UuYXhpcy5taW58fHhyYW5nZS5mcm9tPnhyYW5nZS5heGlzLm1heHx8eXJhbmdlLnRvPHlyYW5nZS5heGlzLm1pbnx8eXJhbmdlLmZyb20+eXJhbmdlLmF4aXMubWF4KWNvbnRpbnVlO3hyYW5nZS5mcm9tPU1hdGgubWF4KHhyYW5nZS5mcm9tLHhyYW5nZS5heGlzLm1pbik7eHJhbmdlLnRvPU1hdGgubWluKHhyYW5nZS50byx4cmFuZ2UuYXhpcy5tYXgpO3lyYW5nZS5mcm9tPU1hdGgubWF4KHlyYW5nZS5mcm9tLHlyYW5nZS5heGlzLm1pbik7eXJhbmdlLnRvPU1hdGgubWluKHlyYW5nZS50byx5cmFuZ2UuYXhpcy5tYXgpO3ZhciB4ZXF1YWw9eHJhbmdlLmZyb209PT14cmFuZ2UudG8seWVxdWFsPXlyYW5nZS5mcm9tPT09eXJhbmdlLnRvO2lmKHhlcXVhbCYmeWVxdWFsKXtjb250aW51ZX14cmFuZ2UuZnJvbT1NYXRoLmZsb29yKHhyYW5nZS5heGlzLnAyYyh4cmFuZ2UuZnJvbSkpO3hyYW5nZS50bz1NYXRoLmZsb29yKHhyYW5nZS5heGlzLnAyYyh4cmFuZ2UudG8pKTt5cmFuZ2UuZnJvbT1NYXRoLmZsb29yKHlyYW5nZS5heGlzLnAyYyh5cmFuZ2UuZnJvbSkpO3lyYW5nZS50bz1NYXRoLmZsb29yKHlyYW5nZS5heGlzLnAyYyh5cmFuZ2UudG8pKTtpZih4ZXF1YWx8fHllcXVhbCl7dmFyIGxpbmVXaWR0aD1tLmxpbmVXaWR0aHx8b3B0aW9ucy5ncmlkLm1hcmtpbmdzTGluZVdpZHRoLHN1YlBpeGVsPWxpbmVXaWR0aCUyPy41OjA7Y3R4LmJlZ2luUGF0aCgpO2N0eC5zdHJva2VTdHlsZT1tLmNvbG9yfHxvcHRpb25zLmdyaWQubWFya2luZ3NDb2xvcjtjdHgubGluZVdpZHRoPWxpbmVXaWR0aDtpZih4ZXF1YWwpe2N0eC5tb3ZlVG8oeHJhbmdlLnRvK3N1YlBpeGVsLHlyYW5nZS5mcm9tKTtjdHgubGluZVRvKHhyYW5nZS50bytzdWJQaXhlbCx5cmFuZ2UudG8pfWVsc2V7Y3R4Lm1vdmVUbyh4cmFuZ2UuZnJvbSx5cmFuZ2UudG8rc3ViUGl4ZWwpO2N0eC5saW5lVG8oeHJhbmdlLnRvLHlyYW5nZS50bytzdWJQaXhlbCl9Y3R4LnN0cm9rZSgpfWVsc2V7Y3R4LmZpbGxTdHlsZT1tLmNvbG9yfHxvcHRpb25zLmdyaWQubWFya2luZ3NDb2xvcjtjdHguZmlsbFJlY3QoeHJhbmdlLmZyb20seXJhbmdlLnRvLHhyYW5nZS50by14cmFuZ2UuZnJvbSx5cmFuZ2UuZnJvbS15cmFuZ2UudG8pfX19YXhlcz1hbGxBeGVzKCk7Ync9b3B0aW9ucy5ncmlkLmJvcmRlcldpZHRoO2Zvcih2YXIgaj0wO2o8YXhlcy5sZW5ndGg7KytqKXt2YXIgYXhpcz1heGVzW2pdLGJveD1heGlzLmJveCx0PWF4aXMudGlja0xlbmd0aCx4LHkseG9mZix5b2ZmO2lmKCFheGlzLnNob3d8fGF4aXMudGlja3MubGVuZ3RoPT0wKWNvbnRpbnVlO2N0eC5saW5lV2lkdGg9MTtpZihheGlzLmRpcmVjdGlvbj09XCJ4XCIpe3g9MDtpZih0PT1cImZ1bGxcIil5PWF4aXMucG9zaXRpb249PVwidG9wXCI/MDpwbG90SGVpZ2h0O2Vsc2UgeT1ib3gudG9wLXBsb3RPZmZzZXQudG9wKyhheGlzLnBvc2l0aW9uPT1cInRvcFwiP2JveC5oZWlnaHQ6MCl9ZWxzZXt5PTA7aWYodD09XCJmdWxsXCIpeD1heGlzLnBvc2l0aW9uPT1cImxlZnRcIj8wOnBsb3RXaWR0aDtlbHNlIHg9Ym94LmxlZnQtcGxvdE9mZnNldC5sZWZ0KyhheGlzLnBvc2l0aW9uPT1cImxlZnRcIj9ib3gud2lkdGg6MCl9aWYoIWF4aXMuaW5uZXJtb3N0KXtjdHguc3Ryb2tlU3R5bGU9YXhpcy5vcHRpb25zLmNvbG9yO2N0eC5iZWdpblBhdGgoKTt4b2ZmPXlvZmY9MDtpZihheGlzLmRpcmVjdGlvbj09XCJ4XCIpeG9mZj1wbG90V2lkdGgrMTtlbHNlIHlvZmY9cGxvdEhlaWdodCsxO2lmKGN0eC5saW5lV2lkdGg9PTEpe2lmKGF4aXMuZGlyZWN0aW9uPT1cInhcIil7eT1NYXRoLmZsb29yKHkpKy41fWVsc2V7eD1NYXRoLmZsb29yKHgpKy41fX1jdHgubW92ZVRvKHgseSk7Y3R4LmxpbmVUbyh4K3hvZmYseSt5b2ZmKTtjdHguc3Ryb2tlKCl9Y3R4LnN0cm9rZVN0eWxlPWF4aXMub3B0aW9ucy50aWNrQ29sb3I7Y3R4LmJlZ2luUGF0aCgpO2ZvcihpPTA7aTxheGlzLnRpY2tzLmxlbmd0aDsrK2kpe3ZhciB2PWF4aXMudGlja3NbaV0udjt4b2ZmPXlvZmY9MDtpZihpc05hTih2KXx8djxheGlzLm1pbnx8dj5heGlzLm1heHx8dD09XCJmdWxsXCImJih0eXBlb2YgYnc9PVwib2JqZWN0XCImJmJ3W2F4aXMucG9zaXRpb25dPjB8fGJ3PjApJiYodj09YXhpcy5taW58fHY9PWF4aXMubWF4KSljb250aW51ZTtpZihheGlzLmRpcmVjdGlvbj09XCJ4XCIpe3g9YXhpcy5wMmModik7eW9mZj10PT1cImZ1bGxcIj8tcGxvdEhlaWdodDp0O2lmKGF4aXMucG9zaXRpb249PVwidG9wXCIpeW9mZj0teW9mZn1lbHNle3k9YXhpcy5wMmModik7eG9mZj10PT1cImZ1bGxcIj8tcGxvdFdpZHRoOnQ7aWYoYXhpcy5wb3NpdGlvbj09XCJsZWZ0XCIpeG9mZj0teG9mZn1pZihjdHgubGluZVdpZHRoPT0xKXtpZihheGlzLmRpcmVjdGlvbj09XCJ4XCIpeD1NYXRoLmZsb29yKHgpKy41O2Vsc2UgeT1NYXRoLmZsb29yKHkpKy41fWN0eC5tb3ZlVG8oeCx5KTtjdHgubGluZVRvKHgreG9mZix5K3lvZmYpfWN0eC5zdHJva2UoKX1pZihidyl7YmM9b3B0aW9ucy5ncmlkLmJvcmRlckNvbG9yO2lmKHR5cGVvZiBidz09XCJvYmplY3RcInx8dHlwZW9mIGJjPT1cIm9iamVjdFwiKXtpZih0eXBlb2YgYnchPT1cIm9iamVjdFwiKXtidz17dG9wOmJ3LHJpZ2h0OmJ3LGJvdHRvbTpidyxsZWZ0OmJ3fX1pZih0eXBlb2YgYmMhPT1cIm9iamVjdFwiKXtiYz17dG9wOmJjLHJpZ2h0OmJjLGJvdHRvbTpiYyxsZWZ0OmJjfX1pZihidy50b3A+MCl7Y3R4LnN0cm9rZVN0eWxlPWJjLnRvcDtjdHgubGluZVdpZHRoPWJ3LnRvcDtjdHguYmVnaW5QYXRoKCk7Y3R4Lm1vdmVUbygwLWJ3LmxlZnQsMC1idy50b3AvMik7Y3R4LmxpbmVUbyhwbG90V2lkdGgsMC1idy50b3AvMik7Y3R4LnN0cm9rZSgpfWlmKGJ3LnJpZ2h0PjApe2N0eC5zdHJva2VTdHlsZT1iYy5yaWdodDtjdHgubGluZVdpZHRoPWJ3LnJpZ2h0O2N0eC5iZWdpblBhdGgoKTtjdHgubW92ZVRvKHBsb3RXaWR0aCtidy5yaWdodC8yLDAtYncudG9wKTtjdHgubGluZVRvKHBsb3RXaWR0aCtidy5yaWdodC8yLHBsb3RIZWlnaHQpO2N0eC5zdHJva2UoKX1pZihidy5ib3R0b20+MCl7Y3R4LnN0cm9rZVN0eWxlPWJjLmJvdHRvbTtjdHgubGluZVdpZHRoPWJ3LmJvdHRvbTtjdHguYmVnaW5QYXRoKCk7Y3R4Lm1vdmVUbyhwbG90V2lkdGgrYncucmlnaHQscGxvdEhlaWdodCtidy5ib3R0b20vMik7Y3R4LmxpbmVUbygwLHBsb3RIZWlnaHQrYncuYm90dG9tLzIpO2N0eC5zdHJva2UoKX1pZihidy5sZWZ0PjApe2N0eC5zdHJva2VTdHlsZT1iYy5sZWZ0O2N0eC5saW5lV2lkdGg9YncubGVmdDtjdHguYmVnaW5QYXRoKCk7Y3R4Lm1vdmVUbygwLWJ3LmxlZnQvMixwbG90SGVpZ2h0K2J3LmJvdHRvbSk7Y3R4LmxpbmVUbygwLWJ3LmxlZnQvMiwwKTtjdHguc3Ryb2tlKCl9fWVsc2V7Y3R4LmxpbmVXaWR0aD1idztjdHguc3Ryb2tlU3R5bGU9b3B0aW9ucy5ncmlkLmJvcmRlckNvbG9yO2N0eC5zdHJva2VSZWN0KC1idy8yLC1idy8yLHBsb3RXaWR0aCtidyxwbG90SGVpZ2h0K2J3KX19Y3R4LnJlc3RvcmUoKX1mdW5jdGlvbiBkcmF3QXhpc0xhYmVscygpeyQuZWFjaChhbGxBeGVzKCksZnVuY3Rpb24oXyxheGlzKXt2YXIgYm94PWF4aXMuYm94LGxlZ2FjeVN0eWxlcz1heGlzLmRpcmVjdGlvbitcIkF4aXMgXCIrYXhpcy5kaXJlY3Rpb24rYXhpcy5uK1wiQXhpc1wiLGxheWVyPVwiZmxvdC1cIitheGlzLmRpcmVjdGlvbitcIi1heGlzIGZsb3QtXCIrYXhpcy5kaXJlY3Rpb24rYXhpcy5uK1wiLWF4aXMgXCIrbGVnYWN5U3R5bGVzLGZvbnQ9YXhpcy5vcHRpb25zLmZvbnR8fFwiZmxvdC10aWNrLWxhYmVsIHRpY2tMYWJlbFwiLHRpY2sseCx5LGhhbGlnbix2YWxpZ247c3VyZmFjZS5yZW1vdmVUZXh0KGxheWVyKTtpZighYXhpcy5zaG93fHxheGlzLnRpY2tzLmxlbmd0aD09MClyZXR1cm47Zm9yKHZhciBpPTA7aTxheGlzLnRpY2tzLmxlbmd0aDsrK2kpe3RpY2s9YXhpcy50aWNrc1tpXTtpZighdGljay5sYWJlbHx8dGljay52PGF4aXMubWlufHx0aWNrLnY+YXhpcy5tYXgpY29udGludWU7aWYoYXhpcy5kaXJlY3Rpb249PVwieFwiKXtoYWxpZ249XCJjZW50ZXJcIjt4PXBsb3RPZmZzZXQubGVmdCtheGlzLnAyYyh0aWNrLnYpO2lmKGF4aXMucG9zaXRpb249PVwiYm90dG9tXCIpe3k9Ym94LnRvcCtib3gucGFkZGluZ31lbHNle3k9Ym94LnRvcCtib3guaGVpZ2h0LWJveC5wYWRkaW5nO3ZhbGlnbj1cImJvdHRvbVwifX1lbHNle3ZhbGlnbj1cIm1pZGRsZVwiO3k9cGxvdE9mZnNldC50b3ArYXhpcy5wMmModGljay52KTtpZihheGlzLnBvc2l0aW9uPT1cImxlZnRcIil7eD1ib3gubGVmdCtib3gud2lkdGgtYm94LnBhZGRpbmc7aGFsaWduPVwicmlnaHRcIn1lbHNle3g9Ym94LmxlZnQrYm94LnBhZGRpbmd9fXN1cmZhY2UuYWRkVGV4dChsYXllcix4LHksdGljay5sYWJlbCxmb250LG51bGwsbnVsbCxoYWxpZ24sdmFsaWduKX19KX1mdW5jdGlvbiBkcmF3U2VyaWVzKHNlcmllcyl7aWYoc2VyaWVzLmxpbmVzLnNob3cpZHJhd1Nlcmllc0xpbmVzKHNlcmllcyk7aWYoc2VyaWVzLmJhcnMuc2hvdylkcmF3U2VyaWVzQmFycyhzZXJpZXMpO2lmKHNlcmllcy5wb2ludHMuc2hvdylkcmF3U2VyaWVzUG9pbnRzKHNlcmllcyl9ZnVuY3Rpb24gZHJhd1Nlcmllc0xpbmVzKHNlcmllcyl7ZnVuY3Rpb24gcGxvdExpbmUoZGF0YXBvaW50cyx4b2Zmc2V0LHlvZmZzZXQsYXhpc3gsYXhpc3kpe3ZhciBwb2ludHM9ZGF0YXBvaW50cy5wb2ludHMscHM9ZGF0YXBvaW50cy5wb2ludHNpemUscHJldng9bnVsbCxwcmV2eT1udWxsO2N0eC5iZWdpblBhdGgoKTtmb3IodmFyIGk9cHM7aTxwb2ludHMubGVuZ3RoO2krPXBzKXt2YXIgeDE9cG9pbnRzW2ktcHNdLHkxPXBvaW50c1tpLXBzKzFdLHgyPXBvaW50c1tpXSx5Mj1wb2ludHNbaSsxXTtpZih4MT09bnVsbHx8eDI9PW51bGwpY29udGludWU7aWYoeTE8PXkyJiZ5MTxheGlzeS5taW4pe2lmKHkyPGF4aXN5Lm1pbiljb250aW51ZTt4MT0oYXhpc3kubWluLXkxKS8oeTIteTEpKih4Mi14MSkreDE7eTE9YXhpc3kubWlufWVsc2UgaWYoeTI8PXkxJiZ5MjxheGlzeS5taW4pe2lmKHkxPGF4aXN5Lm1pbiljb250aW51ZTt4Mj0oYXhpc3kubWluLXkxKS8oeTIteTEpKih4Mi14MSkreDE7eTI9YXhpc3kubWlufWlmKHkxPj15MiYmeTE+YXhpc3kubWF4KXtpZih5Mj5heGlzeS5tYXgpY29udGludWU7eDE9KGF4aXN5Lm1heC15MSkvKHkyLXkxKSooeDIteDEpK3gxO3kxPWF4aXN5Lm1heH1lbHNlIGlmKHkyPj15MSYmeTI+YXhpc3kubWF4KXtpZih5MT5heGlzeS5tYXgpY29udGludWU7eDI9KGF4aXN5Lm1heC15MSkvKHkyLXkxKSooeDIteDEpK3gxO3kyPWF4aXN5Lm1heH1pZih4MTw9eDImJngxPGF4aXN4Lm1pbil7aWYoeDI8YXhpc3gubWluKWNvbnRpbnVlO3kxPShheGlzeC5taW4teDEpLyh4Mi14MSkqKHkyLXkxKSt5MTt4MT1heGlzeC5taW59ZWxzZSBpZih4Mjw9eDEmJngyPGF4aXN4Lm1pbil7aWYoeDE8YXhpc3gubWluKWNvbnRpbnVlO3kyPShheGlzeC5taW4teDEpLyh4Mi14MSkqKHkyLXkxKSt5MTt4Mj1heGlzeC5taW59aWYoeDE+PXgyJiZ4MT5heGlzeC5tYXgpe2lmKHgyPmF4aXN4Lm1heCljb250aW51ZTt5MT0oYXhpc3gubWF4LXgxKS8oeDIteDEpKih5Mi15MSkreTE7eDE9YXhpc3gubWF4fWVsc2UgaWYoeDI+PXgxJiZ4Mj5heGlzeC5tYXgpe2lmKHgxPmF4aXN4Lm1heCljb250aW51ZTt5Mj0oYXhpc3gubWF4LXgxKS8oeDIteDEpKih5Mi15MSkreTE7eDI9YXhpc3gubWF4fWlmKHgxIT1wcmV2eHx8eTEhPXByZXZ5KWN0eC5tb3ZlVG8oYXhpc3gucDJjKHgxKSt4b2Zmc2V0LGF4aXN5LnAyYyh5MSkreW9mZnNldCk7cHJldng9eDI7cHJldnk9eTI7Y3R4LmxpbmVUbyhheGlzeC5wMmMoeDIpK3hvZmZzZXQsYXhpc3kucDJjKHkyKSt5b2Zmc2V0KX1jdHguc3Ryb2tlKCl9ZnVuY3Rpb24gcGxvdExpbmVBcmVhKGRhdGFwb2ludHMsYXhpc3gsYXhpc3kpe3ZhciBwb2ludHM9ZGF0YXBvaW50cy5wb2ludHMscHM9ZGF0YXBvaW50cy5wb2ludHNpemUsYm90dG9tPU1hdGgubWluKE1hdGgubWF4KDAsYXhpc3kubWluKSxheGlzeS5tYXgpLGk9MCx0b3AsYXJlYU9wZW49ZmFsc2UseXBvcz0xLHNlZ21lbnRTdGFydD0wLHNlZ21lbnRFbmQ9MDt3aGlsZSh0cnVlKXtpZihwcz4wJiZpPnBvaW50cy5sZW5ndGgrcHMpYnJlYWs7aSs9cHM7dmFyIHgxPXBvaW50c1tpLXBzXSx5MT1wb2ludHNbaS1wcyt5cG9zXSx4Mj1wb2ludHNbaV0seTI9cG9pbnRzW2kreXBvc107aWYoYXJlYU9wZW4pe2lmKHBzPjAmJngxIT1udWxsJiZ4Mj09bnVsbCl7c2VnbWVudEVuZD1pO3BzPS1wczt5cG9zPTI7Y29udGludWV9aWYocHM8MCYmaT09c2VnbWVudFN0YXJ0K3BzKXtjdHguZmlsbCgpO2FyZWFPcGVuPWZhbHNlO3BzPS1wczt5cG9zPTE7aT1zZWdtZW50U3RhcnQ9c2VnbWVudEVuZCtwcztjb250aW51ZX19aWYoeDE9PW51bGx8fHgyPT1udWxsKWNvbnRpbnVlO2lmKHgxPD14MiYmeDE8YXhpc3gubWluKXtpZih4MjxheGlzeC5taW4pY29udGludWU7eTE9KGF4aXN4Lm1pbi14MSkvKHgyLXgxKSooeTIteTEpK3kxO3gxPWF4aXN4Lm1pbn1lbHNlIGlmKHgyPD14MSYmeDI8YXhpc3gubWluKXtpZih4MTxheGlzeC5taW4pY29udGludWU7eTI9KGF4aXN4Lm1pbi14MSkvKHgyLXgxKSooeTIteTEpK3kxO3gyPWF4aXN4Lm1pbn1pZih4MT49eDImJngxPmF4aXN4Lm1heCl7aWYoeDI+YXhpc3gubWF4KWNvbnRpbnVlO3kxPShheGlzeC5tYXgteDEpLyh4Mi14MSkqKHkyLXkxKSt5MTt4MT1heGlzeC5tYXh9ZWxzZSBpZih4Mj49eDEmJngyPmF4aXN4Lm1heCl7aWYoeDE+YXhpc3gubWF4KWNvbnRpbnVlO3kyPShheGlzeC5tYXgteDEpLyh4Mi14MSkqKHkyLXkxKSt5MTt4Mj1heGlzeC5tYXh9aWYoIWFyZWFPcGVuKXtjdHguYmVnaW5QYXRoKCk7Y3R4Lm1vdmVUbyhheGlzeC5wMmMoeDEpLGF4aXN5LnAyYyhib3R0b20pKTthcmVhT3Blbj10cnVlfWlmKHkxPj1heGlzeS5tYXgmJnkyPj1heGlzeS5tYXgpe2N0eC5saW5lVG8oYXhpc3gucDJjKHgxKSxheGlzeS5wMmMoYXhpc3kubWF4KSk7Y3R4LmxpbmVUbyhheGlzeC5wMmMoeDIpLGF4aXN5LnAyYyhheGlzeS5tYXgpKTtjb250aW51ZX1lbHNlIGlmKHkxPD1heGlzeS5taW4mJnkyPD1heGlzeS5taW4pe2N0eC5saW5lVG8oYXhpc3gucDJjKHgxKSxheGlzeS5wMmMoYXhpc3kubWluKSk7Y3R4LmxpbmVUbyhheGlzeC5wMmMoeDIpLGF4aXN5LnAyYyhheGlzeS5taW4pKTtjb250aW51ZX12YXIgeDFvbGQ9eDEseDJvbGQ9eDI7aWYoeTE8PXkyJiZ5MTxheGlzeS5taW4mJnkyPj1heGlzeS5taW4pe3gxPShheGlzeS5taW4teTEpLyh5Mi15MSkqKHgyLXgxKSt4MTt5MT1heGlzeS5taW59ZWxzZSBpZih5Mjw9eTEmJnkyPGF4aXN5Lm1pbiYmeTE+PWF4aXN5Lm1pbil7eDI9KGF4aXN5Lm1pbi15MSkvKHkyLXkxKSooeDIteDEpK3gxO3kyPWF4aXN5Lm1pbn1pZih5MT49eTImJnkxPmF4aXN5Lm1heCYmeTI8PWF4aXN5Lm1heCl7eDE9KGF4aXN5Lm1heC15MSkvKHkyLXkxKSooeDIteDEpK3gxO3kxPWF4aXN5Lm1heH1lbHNlIGlmKHkyPj15MSYmeTI+YXhpc3kubWF4JiZ5MTw9YXhpc3kubWF4KXt4Mj0oYXhpc3kubWF4LXkxKS8oeTIteTEpKih4Mi14MSkreDE7eTI9YXhpc3kubWF4fWlmKHgxIT14MW9sZCl7Y3R4LmxpbmVUbyhheGlzeC5wMmMoeDFvbGQpLGF4aXN5LnAyYyh5MSkpfWN0eC5saW5lVG8oYXhpc3gucDJjKHgxKSxheGlzeS5wMmMoeTEpKTtjdHgubGluZVRvKGF4aXN4LnAyYyh4MiksYXhpc3kucDJjKHkyKSk7aWYoeDIhPXgyb2xkKXtjdHgubGluZVRvKGF4aXN4LnAyYyh4MiksYXhpc3kucDJjKHkyKSk7Y3R4LmxpbmVUbyhheGlzeC5wMmMoeDJvbGQpLGF4aXN5LnAyYyh5MikpfX19Y3R4LnNhdmUoKTtjdHgudHJhbnNsYXRlKHBsb3RPZmZzZXQubGVmdCxwbG90T2Zmc2V0LnRvcCk7Y3R4LmxpbmVKb2luPVwicm91bmRcIjt2YXIgbHc9c2VyaWVzLmxpbmVzLmxpbmVXaWR0aCxzdz1zZXJpZXMuc2hhZG93U2l6ZTtpZihsdz4wJiZzdz4wKXtjdHgubGluZVdpZHRoPXN3O2N0eC5zdHJva2VTdHlsZT1cInJnYmEoMCwwLDAsMC4xKVwiO3ZhciBhbmdsZT1NYXRoLlBJLzE4O3Bsb3RMaW5lKHNlcmllcy5kYXRhcG9pbnRzLE1hdGguc2luKGFuZ2xlKSoobHcvMitzdy8yKSxNYXRoLmNvcyhhbmdsZSkqKGx3LzIrc3cvMiksc2VyaWVzLnhheGlzLHNlcmllcy55YXhpcyk7Y3R4LmxpbmVXaWR0aD1zdy8yO3Bsb3RMaW5lKHNlcmllcy5kYXRhcG9pbnRzLE1hdGguc2luKGFuZ2xlKSoobHcvMitzdy80KSxNYXRoLmNvcyhhbmdsZSkqKGx3LzIrc3cvNCksc2VyaWVzLnhheGlzLHNlcmllcy55YXhpcyl9Y3R4LmxpbmVXaWR0aD1sdztjdHguc3Ryb2tlU3R5bGU9c2VyaWVzLmNvbG9yO3ZhciBmaWxsU3R5bGU9Z2V0RmlsbFN0eWxlKHNlcmllcy5saW5lcyxzZXJpZXMuY29sb3IsMCxwbG90SGVpZ2h0KTtpZihmaWxsU3R5bGUpe2N0eC5maWxsU3R5bGU9ZmlsbFN0eWxlO3Bsb3RMaW5lQXJlYShzZXJpZXMuZGF0YXBvaW50cyxzZXJpZXMueGF4aXMsc2VyaWVzLnlheGlzKX1pZihsdz4wKXBsb3RMaW5lKHNlcmllcy5kYXRhcG9pbnRzLDAsMCxzZXJpZXMueGF4aXMsc2VyaWVzLnlheGlzKTtjdHgucmVzdG9yZSgpfWZ1bmN0aW9uIGRyYXdTZXJpZXNQb2ludHMoc2VyaWVzKXtmdW5jdGlvbiBwbG90UG9pbnRzKGRhdGFwb2ludHMscmFkaXVzLGZpbGxTdHlsZSxvZmZzZXQsc2hhZG93LGF4aXN4LGF4aXN5LHN5bWJvbCl7dmFyIHBvaW50cz1kYXRhcG9pbnRzLnBvaW50cyxwcz1kYXRhcG9pbnRzLnBvaW50c2l6ZTtmb3IodmFyIGk9MDtpPHBvaW50cy5sZW5ndGg7aSs9cHMpe3ZhciB4PXBvaW50c1tpXSx5PXBvaW50c1tpKzFdO2lmKHg9PW51bGx8fHg8YXhpc3gubWlufHx4PmF4aXN4Lm1heHx8eTxheGlzeS5taW58fHk+YXhpc3kubWF4KWNvbnRpbnVlO2N0eC5iZWdpblBhdGgoKTt4PWF4aXN4LnAyYyh4KTt5PWF4aXN5LnAyYyh5KStvZmZzZXQ7aWYoc3ltYm9sPT1cImNpcmNsZVwiKWN0eC5hcmMoeCx5LHJhZGl1cywwLHNoYWRvdz9NYXRoLlBJOk1hdGguUEkqMixmYWxzZSk7ZWxzZSBzeW1ib2woY3R4LHgseSxyYWRpdXMsc2hhZG93KTtjdHguY2xvc2VQYXRoKCk7aWYoZmlsbFN0eWxlKXtjdHguZmlsbFN0eWxlPWZpbGxTdHlsZTtjdHguZmlsbCgpfWN0eC5zdHJva2UoKX19Y3R4LnNhdmUoKTtjdHgudHJhbnNsYXRlKHBsb3RPZmZzZXQubGVmdCxwbG90T2Zmc2V0LnRvcCk7dmFyIGx3PXNlcmllcy5wb2ludHMubGluZVdpZHRoLHN3PXNlcmllcy5zaGFkb3dTaXplLHJhZGl1cz1zZXJpZXMucG9pbnRzLnJhZGl1cyxzeW1ib2w9c2VyaWVzLnBvaW50cy5zeW1ib2w7aWYobHc9PTApbHc9MWUtNDtpZihsdz4wJiZzdz4wKXt2YXIgdz1zdy8yO2N0eC5saW5lV2lkdGg9dztjdHguc3Ryb2tlU3R5bGU9XCJyZ2JhKDAsMCwwLDAuMSlcIjtwbG90UG9pbnRzKHNlcmllcy5kYXRhcG9pbnRzLHJhZGl1cyxudWxsLHcrdy8yLHRydWUsc2VyaWVzLnhheGlzLHNlcmllcy55YXhpcyxzeW1ib2wpO2N0eC5zdHJva2VTdHlsZT1cInJnYmEoMCwwLDAsMC4yKVwiO3Bsb3RQb2ludHMoc2VyaWVzLmRhdGFwb2ludHMscmFkaXVzLG51bGwsdy8yLHRydWUsc2VyaWVzLnhheGlzLHNlcmllcy55YXhpcyxzeW1ib2wpfWN0eC5saW5lV2lkdGg9bHc7Y3R4LnN0cm9rZVN0eWxlPXNlcmllcy5jb2xvcjtwbG90UG9pbnRzKHNlcmllcy5kYXRhcG9pbnRzLHJhZGl1cyxnZXRGaWxsU3R5bGUoc2VyaWVzLnBvaW50cyxzZXJpZXMuY29sb3IpLDAsZmFsc2Usc2VyaWVzLnhheGlzLHNlcmllcy55YXhpcyxzeW1ib2wpO2N0eC5yZXN0b3JlKCl9ZnVuY3Rpb24gZHJhd0Jhcih4LHksYixiYXJMZWZ0LGJhclJpZ2h0LGZpbGxTdHlsZUNhbGxiYWNrLGF4aXN4LGF4aXN5LGMsaG9yaXpvbnRhbCxsaW5lV2lkdGgpe3ZhciBsZWZ0LHJpZ2h0LGJvdHRvbSx0b3AsZHJhd0xlZnQsZHJhd1JpZ2h0LGRyYXdUb3AsZHJhd0JvdHRvbSx0bXA7aWYoaG9yaXpvbnRhbCl7ZHJhd0JvdHRvbT1kcmF3UmlnaHQ9ZHJhd1RvcD10cnVlO2RyYXdMZWZ0PWZhbHNlO2xlZnQ9YjtyaWdodD14O3RvcD15K2JhckxlZnQ7Ym90dG9tPXkrYmFyUmlnaHQ7aWYocmlnaHQ8bGVmdCl7dG1wPXJpZ2h0O3JpZ2h0PWxlZnQ7bGVmdD10bXA7ZHJhd0xlZnQ9dHJ1ZTtkcmF3UmlnaHQ9ZmFsc2V9fWVsc2V7ZHJhd0xlZnQ9ZHJhd1JpZ2h0PWRyYXdUb3A9dHJ1ZTtkcmF3Qm90dG9tPWZhbHNlO2xlZnQ9eCtiYXJMZWZ0O3JpZ2h0PXgrYmFyUmlnaHQ7Ym90dG9tPWI7dG9wPXk7aWYodG9wPGJvdHRvbSl7dG1wPXRvcDt0b3A9Ym90dG9tO2JvdHRvbT10bXA7ZHJhd0JvdHRvbT10cnVlO2RyYXdUb3A9ZmFsc2V9fWlmKHJpZ2h0PGF4aXN4Lm1pbnx8bGVmdD5heGlzeC5tYXh8fHRvcDxheGlzeS5taW58fGJvdHRvbT5heGlzeS5tYXgpcmV0dXJuO2lmKGxlZnQ8YXhpc3gubWluKXtsZWZ0PWF4aXN4Lm1pbjtkcmF3TGVmdD1mYWxzZX1pZihyaWdodD5heGlzeC5tYXgpe3JpZ2h0PWF4aXN4Lm1heDtkcmF3UmlnaHQ9ZmFsc2V9aWYoYm90dG9tPGF4aXN5Lm1pbil7Ym90dG9tPWF4aXN5Lm1pbjtkcmF3Qm90dG9tPWZhbHNlfWlmKHRvcD5heGlzeS5tYXgpe3RvcD1heGlzeS5tYXg7ZHJhd1RvcD1mYWxzZX1sZWZ0PWF4aXN4LnAyYyhsZWZ0KTtib3R0b209YXhpc3kucDJjKGJvdHRvbSk7cmlnaHQ9YXhpc3gucDJjKHJpZ2h0KTt0b3A9YXhpc3kucDJjKHRvcCk7aWYoZmlsbFN0eWxlQ2FsbGJhY2spe2MuZmlsbFN0eWxlPWZpbGxTdHlsZUNhbGxiYWNrKGJvdHRvbSx0b3ApO2MuZmlsbFJlY3QobGVmdCx0b3AscmlnaHQtbGVmdCxib3R0b20tdG9wKX1pZihsaW5lV2lkdGg+MCYmKGRyYXdMZWZ0fHxkcmF3UmlnaHR8fGRyYXdUb3B8fGRyYXdCb3R0b20pKXtjLmJlZ2luUGF0aCgpO2MubW92ZVRvKGxlZnQsYm90dG9tKTtpZihkcmF3TGVmdCljLmxpbmVUbyhsZWZ0LHRvcCk7ZWxzZSBjLm1vdmVUbyhsZWZ0LHRvcCk7aWYoZHJhd1RvcCljLmxpbmVUbyhyaWdodCx0b3ApO2Vsc2UgYy5tb3ZlVG8ocmlnaHQsdG9wKTtpZihkcmF3UmlnaHQpYy5saW5lVG8ocmlnaHQsYm90dG9tKTtlbHNlIGMubW92ZVRvKHJpZ2h0LGJvdHRvbSk7aWYoZHJhd0JvdHRvbSljLmxpbmVUbyhsZWZ0LGJvdHRvbSk7ZWxzZSBjLm1vdmVUbyhsZWZ0LGJvdHRvbSk7Yy5zdHJva2UoKX19ZnVuY3Rpb24gZHJhd1Nlcmllc0JhcnMoc2VyaWVzKXtmdW5jdGlvbiBwbG90QmFycyhkYXRhcG9pbnRzLGJhckxlZnQsYmFyUmlnaHQsZmlsbFN0eWxlQ2FsbGJhY2ssYXhpc3gsYXhpc3kpe3ZhciBwb2ludHM9ZGF0YXBvaW50cy5wb2ludHMscHM9ZGF0YXBvaW50cy5wb2ludHNpemU7Zm9yKHZhciBpPTA7aTxwb2ludHMubGVuZ3RoO2krPXBzKXtpZihwb2ludHNbaV09PW51bGwpY29udGludWU7ZHJhd0Jhcihwb2ludHNbaV0scG9pbnRzW2krMV0scG9pbnRzW2krMl0sYmFyTGVmdCxiYXJSaWdodCxmaWxsU3R5bGVDYWxsYmFjayxheGlzeCxheGlzeSxjdHgsc2VyaWVzLmJhcnMuaG9yaXpvbnRhbCxzZXJpZXMuYmFycy5saW5lV2lkdGgpfX1jdHguc2F2ZSgpO2N0eC50cmFuc2xhdGUocGxvdE9mZnNldC5sZWZ0LHBsb3RPZmZzZXQudG9wKTtjdHgubGluZVdpZHRoPXNlcmllcy5iYXJzLmxpbmVXaWR0aDtjdHguc3Ryb2tlU3R5bGU9c2VyaWVzLmNvbG9yO3ZhciBiYXJMZWZ0O3N3aXRjaChzZXJpZXMuYmFycy5hbGlnbil7Y2FzZVwibGVmdFwiOmJhckxlZnQ9MDticmVhaztjYXNlXCJyaWdodFwiOmJhckxlZnQ9LXNlcmllcy5iYXJzLmJhcldpZHRoO2JyZWFrO2RlZmF1bHQ6YmFyTGVmdD0tc2VyaWVzLmJhcnMuYmFyV2lkdGgvMn12YXIgZmlsbFN0eWxlQ2FsbGJhY2s9c2VyaWVzLmJhcnMuZmlsbD9mdW5jdGlvbihib3R0b20sdG9wKXtyZXR1cm4gZ2V0RmlsbFN0eWxlKHNlcmllcy5iYXJzLHNlcmllcy5jb2xvcixib3R0b20sdG9wKX06bnVsbDtwbG90QmFycyhzZXJpZXMuZGF0YXBvaW50cyxiYXJMZWZ0LGJhckxlZnQrc2VyaWVzLmJhcnMuYmFyV2lkdGgsZmlsbFN0eWxlQ2FsbGJhY2ssc2VyaWVzLnhheGlzLHNlcmllcy55YXhpcyk7Y3R4LnJlc3RvcmUoKX1mdW5jdGlvbiBnZXRGaWxsU3R5bGUoZmlsbG9wdGlvbnMsc2VyaWVzQ29sb3IsYm90dG9tLHRvcCl7dmFyIGZpbGw9ZmlsbG9wdGlvbnMuZmlsbDtpZighZmlsbClyZXR1cm4gbnVsbDtpZihmaWxsb3B0aW9ucy5maWxsQ29sb3IpcmV0dXJuIGdldENvbG9yT3JHcmFkaWVudChmaWxsb3B0aW9ucy5maWxsQ29sb3IsYm90dG9tLHRvcCxzZXJpZXNDb2xvcik7dmFyIGM9JC5jb2xvci5wYXJzZShzZXJpZXNDb2xvcik7Yy5hPXR5cGVvZiBmaWxsPT1cIm51bWJlclwiP2ZpbGw6LjQ7Yy5ub3JtYWxpemUoKTtyZXR1cm4gYy50b1N0cmluZygpfWZ1bmN0aW9uIGluc2VydExlZ2VuZCgpe2lmKG9wdGlvbnMubGVnZW5kLmNvbnRhaW5lciE9bnVsbCl7JChvcHRpb25zLmxlZ2VuZC5jb250YWluZXIpLmh0bWwoXCJcIil9ZWxzZXtwbGFjZWhvbGRlci5maW5kKFwiLmxlZ2VuZFwiKS5yZW1vdmUoKX1pZighb3B0aW9ucy5sZWdlbmQuc2hvdyl7cmV0dXJufXZhciBmcmFnbWVudHM9W10sZW50cmllcz1bXSxyb3dTdGFydGVkPWZhbHNlLGxmPW9wdGlvbnMubGVnZW5kLmxhYmVsRm9ybWF0dGVyLHMsbGFiZWw7Zm9yKHZhciBpPTA7aTxzZXJpZXMubGVuZ3RoOysraSl7cz1zZXJpZXNbaV07aWYocy5sYWJlbCl7bGFiZWw9bGY/bGYocy5sYWJlbCxzKTpzLmxhYmVsO2lmKGxhYmVsKXtlbnRyaWVzLnB1c2goe2xhYmVsOmxhYmVsLGNvbG9yOnMuY29sb3J9KX19fWlmKG9wdGlvbnMubGVnZW5kLnNvcnRlZCl7aWYoJC5pc0Z1bmN0aW9uKG9wdGlvbnMubGVnZW5kLnNvcnRlZCkpe2VudHJpZXMuc29ydChvcHRpb25zLmxlZ2VuZC5zb3J0ZWQpfWVsc2UgaWYob3B0aW9ucy5sZWdlbmQuc29ydGVkPT1cInJldmVyc2VcIil7ZW50cmllcy5yZXZlcnNlKCl9ZWxzZXt2YXIgYXNjZW5kaW5nPW9wdGlvbnMubGVnZW5kLnNvcnRlZCE9XCJkZXNjZW5kaW5nXCI7ZW50cmllcy5zb3J0KGZ1bmN0aW9uKGEsYil7cmV0dXJuIGEubGFiZWw9PWIubGFiZWw/MDphLmxhYmVsPGIubGFiZWwhPWFzY2VuZGluZz8xOi0xfSl9fWZvcih2YXIgaT0wO2k8ZW50cmllcy5sZW5ndGg7KytpKXt2YXIgZW50cnk9ZW50cmllc1tpXTtpZihpJW9wdGlvbnMubGVnZW5kLm5vQ29sdW1ucz09MCl7aWYocm93U3RhcnRlZClmcmFnbWVudHMucHVzaChcIjwvdHI+XCIpO2ZyYWdtZW50cy5wdXNoKFwiPHRyPlwiKTtyb3dTdGFydGVkPXRydWV9ZnJhZ21lbnRzLnB1c2goJzx0ZCBjbGFzcz1cImxlZ2VuZENvbG9yQm94XCI+PGRpdiBzdHlsZT1cImJvcmRlcjoxcHggc29saWQgJytvcHRpb25zLmxlZ2VuZC5sYWJlbEJveEJvcmRlckNvbG9yKyc7cGFkZGluZzoxcHhcIj48ZGl2IHN0eWxlPVwid2lkdGg6NHB4O2hlaWdodDowO2JvcmRlcjo1cHggc29saWQgJytlbnRyeS5jb2xvcisnO292ZXJmbG93OmhpZGRlblwiPjwvZGl2PjwvZGl2PjwvdGQ+JysnPHRkIGNsYXNzPVwibGVnZW5kTGFiZWxcIj4nK2VudHJ5LmxhYmVsK1wiPC90ZD5cIil9aWYocm93U3RhcnRlZClmcmFnbWVudHMucHVzaChcIjwvdHI+XCIpO2lmKGZyYWdtZW50cy5sZW5ndGg9PTApcmV0dXJuO3ZhciB0YWJsZT0nPHRhYmxlIHN0eWxlPVwiZm9udC1zaXplOnNtYWxsZXI7Y29sb3I6JytvcHRpb25zLmdyaWQuY29sb3IrJ1wiPicrZnJhZ21lbnRzLmpvaW4oXCJcIikrXCI8L3RhYmxlPlwiO2lmKG9wdGlvbnMubGVnZW5kLmNvbnRhaW5lciE9bnVsbCkkKG9wdGlvbnMubGVnZW5kLmNvbnRhaW5lcikuaHRtbCh0YWJsZSk7ZWxzZXt2YXIgcG9zPVwiXCIscD1vcHRpb25zLmxlZ2VuZC5wb3NpdGlvbixtPW9wdGlvbnMubGVnZW5kLm1hcmdpbjtpZihtWzBdPT1udWxsKW09W20sbV07aWYocC5jaGFyQXQoMCk9PVwiblwiKXBvcys9XCJ0b3A6XCIrKG1bMV0rcGxvdE9mZnNldC50b3ApK1wicHg7XCI7ZWxzZSBpZihwLmNoYXJBdCgwKT09XCJzXCIpcG9zKz1cImJvdHRvbTpcIisobVsxXStwbG90T2Zmc2V0LmJvdHRvbSkrXCJweDtcIjtpZihwLmNoYXJBdCgxKT09XCJlXCIpcG9zKz1cInJpZ2h0OlwiKyhtWzBdK3Bsb3RPZmZzZXQucmlnaHQpK1wicHg7XCI7ZWxzZSBpZihwLmNoYXJBdCgxKT09XCJ3XCIpcG9zKz1cImxlZnQ6XCIrKG1bMF0rcGxvdE9mZnNldC5sZWZ0KStcInB4O1wiO3ZhciBsZWdlbmQ9JCgnPGRpdiBjbGFzcz1cImxlZ2VuZFwiPicrdGFibGUucmVwbGFjZSgnc3R5bGU9XCInLCdzdHlsZT1cInBvc2l0aW9uOmFic29sdXRlOycrcG9zK1wiO1wiKStcIjwvZGl2PlwiKS5hcHBlbmRUbyhwbGFjZWhvbGRlcik7aWYob3B0aW9ucy5sZWdlbmQuYmFja2dyb3VuZE9wYWNpdHkhPTApe3ZhciBjPW9wdGlvbnMubGVnZW5kLmJhY2tncm91bmRDb2xvcjtpZihjPT1udWxsKXtjPW9wdGlvbnMuZ3JpZC5iYWNrZ3JvdW5kQ29sb3I7aWYoYyYmdHlwZW9mIGM9PVwic3RyaW5nXCIpYz0kLmNvbG9yLnBhcnNlKGMpO2Vsc2UgYz0kLmNvbG9yLmV4dHJhY3QobGVnZW5kLFwiYmFja2dyb3VuZC1jb2xvclwiKTtjLmE9MTtjPWMudG9TdHJpbmcoKX12YXIgZGl2PWxlZ2VuZC5jaGlsZHJlbigpOyQoJzxkaXYgc3R5bGU9XCJwb3NpdGlvbjphYnNvbHV0ZTt3aWR0aDonK2Rpdi53aWR0aCgpK1wicHg7aGVpZ2h0OlwiK2Rpdi5oZWlnaHQoKStcInB4O1wiK3BvcytcImJhY2tncm91bmQtY29sb3I6XCIrYysnO1wiPiA8L2Rpdj4nKS5wcmVwZW5kVG8obGVnZW5kKS5jc3MoXCJvcGFjaXR5XCIsb3B0aW9ucy5sZWdlbmQuYmFja2dyb3VuZE9wYWNpdHkpfX19dmFyIGhpZ2hsaWdodHM9W10scmVkcmF3VGltZW91dD1udWxsO2Z1bmN0aW9uIGZpbmROZWFyYnlJdGVtKG1vdXNlWCxtb3VzZVksc2VyaWVzRmlsdGVyKXt2YXIgbWF4RGlzdGFuY2U9b3B0aW9ucy5ncmlkLm1vdXNlQWN0aXZlUmFkaXVzLHNtYWxsZXN0RGlzdGFuY2U9bWF4RGlzdGFuY2UqbWF4RGlzdGFuY2UrMSxpdGVtPW51bGwsZm91bmRQb2ludD1mYWxzZSxpLGoscHM7Zm9yKGk9c2VyaWVzLmxlbmd0aC0xO2k+PTA7LS1pKXtpZighc2VyaWVzRmlsdGVyKHNlcmllc1tpXSkpY29udGludWU7dmFyIHM9c2VyaWVzW2ldLGF4aXN4PXMueGF4aXMsYXhpc3k9cy55YXhpcyxwb2ludHM9cy5kYXRhcG9pbnRzLnBvaW50cyxteD1heGlzeC5jMnAobW91c2VYKSxteT1heGlzeS5jMnAobW91c2VZKSxtYXh4PW1heERpc3RhbmNlL2F4aXN4LnNjYWxlLG1heHk9bWF4RGlzdGFuY2UvYXhpc3kuc2NhbGU7cHM9cy5kYXRhcG9pbnRzLnBvaW50c2l6ZTtpZihheGlzeC5vcHRpb25zLmludmVyc2VUcmFuc2Zvcm0pbWF4eD1OdW1iZXIuTUFYX1ZBTFVFO2lmKGF4aXN5Lm9wdGlvbnMuaW52ZXJzZVRyYW5zZm9ybSltYXh5PU51bWJlci5NQVhfVkFMVUU7aWYocy5saW5lcy5zaG93fHxzLnBvaW50cy5zaG93KXtmb3Ioaj0wO2o8cG9pbnRzLmxlbmd0aDtqKz1wcyl7dmFyIHg9cG9pbnRzW2pdLHk9cG9pbnRzW2orMV07aWYoeD09bnVsbCljb250aW51ZTtpZih4LW14Pm1heHh8fHgtbXg8LW1heHh8fHktbXk+bWF4eXx8eS1teTwtbWF4eSljb250aW51ZTt2YXIgZHg9TWF0aC5hYnMoYXhpc3gucDJjKHgpLW1vdXNlWCksZHk9TWF0aC5hYnMoYXhpc3kucDJjKHkpLW1vdXNlWSksZGlzdD1keCpkeCtkeSpkeTtpZihkaXN0PHNtYWxsZXN0RGlzdGFuY2Upe3NtYWxsZXN0RGlzdGFuY2U9ZGlzdDtpdGVtPVtpLGovcHNdfX19aWYocy5iYXJzLnNob3cmJiFpdGVtKXt2YXIgYmFyTGVmdCxiYXJSaWdodDtzd2l0Y2gocy5iYXJzLmFsaWduKXtjYXNlXCJsZWZ0XCI6YmFyTGVmdD0wO2JyZWFrO2Nhc2VcInJpZ2h0XCI6YmFyTGVmdD0tcy5iYXJzLmJhcldpZHRoO2JyZWFrO2RlZmF1bHQ6YmFyTGVmdD0tcy5iYXJzLmJhcldpZHRoLzJ9YmFyUmlnaHQ9YmFyTGVmdCtzLmJhcnMuYmFyV2lkdGg7Zm9yKGo9MDtqPHBvaW50cy5sZW5ndGg7ais9cHMpe3ZhciB4PXBvaW50c1tqXSx5PXBvaW50c1tqKzFdLGI9cG9pbnRzW2orMl07aWYoeD09bnVsbCljb250aW51ZTtpZihzZXJpZXNbaV0uYmFycy5ob3Jpem9udGFsP214PD1NYXRoLm1heChiLHgpJiZteD49TWF0aC5taW4oYix4KSYmbXk+PXkrYmFyTGVmdCYmbXk8PXkrYmFyUmlnaHQ6bXg+PXgrYmFyTGVmdCYmbXg8PXgrYmFyUmlnaHQmJm15Pj1NYXRoLm1pbihiLHkpJiZteTw9TWF0aC5tYXgoYix5KSlpdGVtPVtpLGovcHNdfX19aWYoaXRlbSl7aT1pdGVtWzBdO2o9aXRlbVsxXTtwcz1zZXJpZXNbaV0uZGF0YXBvaW50cy5wb2ludHNpemU7cmV0dXJue2RhdGFwb2ludDpzZXJpZXNbaV0uZGF0YXBvaW50cy5wb2ludHMuc2xpY2UoaipwcywoaisxKSpwcyksZGF0YUluZGV4Omosc2VyaWVzOnNlcmllc1tpXSxzZXJpZXNJbmRleDppfX1yZXR1cm4gbnVsbH1mdW5jdGlvbiBvbk1vdXNlTW92ZShlKXtpZihvcHRpb25zLmdyaWQuaG92ZXJhYmxlKXRyaWdnZXJDbGlja0hvdmVyRXZlbnQoXCJwbG90aG92ZXJcIixlLGZ1bmN0aW9uKHMpe3JldHVybiBzW1wiaG92ZXJhYmxlXCJdIT1mYWxzZX0pfWZ1bmN0aW9uIG9uTW91c2VMZWF2ZShlKXtpZihvcHRpb25zLmdyaWQuaG92ZXJhYmxlKXRyaWdnZXJDbGlja0hvdmVyRXZlbnQoXCJwbG90aG92ZXJcIixlLGZ1bmN0aW9uKHMpe3JldHVybiBmYWxzZX0pfWZ1bmN0aW9uIG9uQ2xpY2soZSl7dHJpZ2dlckNsaWNrSG92ZXJFdmVudChcInBsb3RjbGlja1wiLGUsZnVuY3Rpb24ocyl7cmV0dXJuIHNbXCJjbGlja2FibGVcIl0hPWZhbHNlfSl9ZnVuY3Rpb24gdHJpZ2dlckNsaWNrSG92ZXJFdmVudChldmVudG5hbWUsZXZlbnQsc2VyaWVzRmlsdGVyKXt2YXIgb2Zmc2V0PWV2ZW50SG9sZGVyLm9mZnNldCgpLGNhbnZhc1g9ZXZlbnQucGFnZVgtb2Zmc2V0LmxlZnQtcGxvdE9mZnNldC5sZWZ0LGNhbnZhc1k9ZXZlbnQucGFnZVktb2Zmc2V0LnRvcC1wbG90T2Zmc2V0LnRvcCxwb3M9Y2FudmFzVG9BeGlzQ29vcmRzKHtsZWZ0OmNhbnZhc1gsdG9wOmNhbnZhc1l9KTtwb3MucGFnZVg9ZXZlbnQucGFnZVg7cG9zLnBhZ2VZPWV2ZW50LnBhZ2VZO3ZhciBpdGVtPWZpbmROZWFyYnlJdGVtKGNhbnZhc1gsY2FudmFzWSxzZXJpZXNGaWx0ZXIpO2lmKGl0ZW0pe2l0ZW0ucGFnZVg9cGFyc2VJbnQoaXRlbS5zZXJpZXMueGF4aXMucDJjKGl0ZW0uZGF0YXBvaW50WzBdKStvZmZzZXQubGVmdCtwbG90T2Zmc2V0LmxlZnQsMTApO2l0ZW0ucGFnZVk9cGFyc2VJbnQoaXRlbS5zZXJpZXMueWF4aXMucDJjKGl0ZW0uZGF0YXBvaW50WzFdKStvZmZzZXQudG9wK3Bsb3RPZmZzZXQudG9wLDEwKX1pZihvcHRpb25zLmdyaWQuYXV0b0hpZ2hsaWdodCl7Zm9yKHZhciBpPTA7aTxoaWdobGlnaHRzLmxlbmd0aDsrK2kpe3ZhciBoPWhpZ2hsaWdodHNbaV07aWYoaC5hdXRvPT1ldmVudG5hbWUmJiEoaXRlbSYmaC5zZXJpZXM9PWl0ZW0uc2VyaWVzJiZoLnBvaW50WzBdPT1pdGVtLmRhdGFwb2ludFswXSYmaC5wb2ludFsxXT09aXRlbS5kYXRhcG9pbnRbMV0pKXVuaGlnaGxpZ2h0KGguc2VyaWVzLGgucG9pbnQpfWlmKGl0ZW0paGlnaGxpZ2h0KGl0ZW0uc2VyaWVzLGl0ZW0uZGF0YXBvaW50LGV2ZW50bmFtZSl9cGxhY2Vob2xkZXIudHJpZ2dlcihldmVudG5hbWUsW3BvcyxpdGVtXSl9ZnVuY3Rpb24gdHJpZ2dlclJlZHJhd092ZXJsYXkoKXt2YXIgdD1vcHRpb25zLmludGVyYWN0aW9uLnJlZHJhd092ZXJsYXlJbnRlcnZhbDtpZih0PT0tMSl7ZHJhd092ZXJsYXkoKTtyZXR1cm59aWYoIXJlZHJhd1RpbWVvdXQpcmVkcmF3VGltZW91dD1zZXRUaW1lb3V0KGRyYXdPdmVybGF5LHQpfWZ1bmN0aW9uIGRyYXdPdmVybGF5KCl7cmVkcmF3VGltZW91dD1udWxsO29jdHguc2F2ZSgpO292ZXJsYXkuY2xlYXIoKTtvY3R4LnRyYW5zbGF0ZShwbG90T2Zmc2V0LmxlZnQscGxvdE9mZnNldC50b3ApO3ZhciBpLGhpO2ZvcihpPTA7aTxoaWdobGlnaHRzLmxlbmd0aDsrK2kpe2hpPWhpZ2hsaWdodHNbaV07aWYoaGkuc2VyaWVzLmJhcnMuc2hvdylkcmF3QmFySGlnaGxpZ2h0KGhpLnNlcmllcyxoaS5wb2ludCk7ZWxzZSBkcmF3UG9pbnRIaWdobGlnaHQoaGkuc2VyaWVzLGhpLnBvaW50KX1vY3R4LnJlc3RvcmUoKTtleGVjdXRlSG9va3MoaG9va3MuZHJhd092ZXJsYXksW29jdHhdKX1mdW5jdGlvbiBoaWdobGlnaHQocyxwb2ludCxhdXRvKXtpZih0eXBlb2Ygcz09XCJudW1iZXJcIilzPXNlcmllc1tzXTtpZih0eXBlb2YgcG9pbnQ9PVwibnVtYmVyXCIpe3ZhciBwcz1zLmRhdGFwb2ludHMucG9pbnRzaXplO3BvaW50PXMuZGF0YXBvaW50cy5wb2ludHMuc2xpY2UocHMqcG9pbnQscHMqKHBvaW50KzEpKX12YXIgaT1pbmRleE9mSGlnaGxpZ2h0KHMscG9pbnQpO2lmKGk9PS0xKXtoaWdobGlnaHRzLnB1c2goe3NlcmllczpzLHBvaW50OnBvaW50LGF1dG86YXV0b30pO3RyaWdnZXJSZWRyYXdPdmVybGF5KCl9ZWxzZSBpZighYXV0byloaWdobGlnaHRzW2ldLmF1dG89ZmFsc2V9ZnVuY3Rpb24gdW5oaWdobGlnaHQocyxwb2ludCl7aWYocz09bnVsbCYmcG9pbnQ9PW51bGwpe2hpZ2hsaWdodHM9W107dHJpZ2dlclJlZHJhd092ZXJsYXkoKTtyZXR1cm59aWYodHlwZW9mIHM9PVwibnVtYmVyXCIpcz1zZXJpZXNbc107aWYodHlwZW9mIHBvaW50PT1cIm51bWJlclwiKXt2YXIgcHM9cy5kYXRhcG9pbnRzLnBvaW50c2l6ZTtwb2ludD1zLmRhdGFwb2ludHMucG9pbnRzLnNsaWNlKHBzKnBvaW50LHBzKihwb2ludCsxKSl9dmFyIGk9aW5kZXhPZkhpZ2hsaWdodChzLHBvaW50KTtpZihpIT0tMSl7aGlnaGxpZ2h0cy5zcGxpY2UoaSwxKTt0cmlnZ2VyUmVkcmF3T3ZlcmxheSgpfX1mdW5jdGlvbiBpbmRleE9mSGlnaGxpZ2h0KHMscCl7Zm9yKHZhciBpPTA7aTxoaWdobGlnaHRzLmxlbmd0aDsrK2kpe3ZhciBoPWhpZ2hsaWdodHNbaV07aWYoaC5zZXJpZXM9PXMmJmgucG9pbnRbMF09PXBbMF0mJmgucG9pbnRbMV09PXBbMV0pcmV0dXJuIGl9cmV0dXJuLTF9ZnVuY3Rpb24gZHJhd1BvaW50SGlnaGxpZ2h0KHNlcmllcyxwb2ludCl7dmFyIHg9cG9pbnRbMF0seT1wb2ludFsxXSxheGlzeD1zZXJpZXMueGF4aXMsYXhpc3k9c2VyaWVzLnlheGlzLGhpZ2hsaWdodENvbG9yPXR5cGVvZiBzZXJpZXMuaGlnaGxpZ2h0Q29sb3I9PT1cInN0cmluZ1wiP3Nlcmllcy5oaWdobGlnaHRDb2xvcjokLmNvbG9yLnBhcnNlKHNlcmllcy5jb2xvcikuc2NhbGUoXCJhXCIsLjUpLnRvU3RyaW5nKCk7aWYoeDxheGlzeC5taW58fHg+YXhpc3gubWF4fHx5PGF4aXN5Lm1pbnx8eT5heGlzeS5tYXgpcmV0dXJuO3ZhciBwb2ludFJhZGl1cz1zZXJpZXMucG9pbnRzLnJhZGl1cytzZXJpZXMucG9pbnRzLmxpbmVXaWR0aC8yO29jdHgubGluZVdpZHRoPXBvaW50UmFkaXVzO29jdHguc3Ryb2tlU3R5bGU9aGlnaGxpZ2h0Q29sb3I7dmFyIHJhZGl1cz0xLjUqcG9pbnRSYWRpdXM7eD1heGlzeC5wMmMoeCk7eT1heGlzeS5wMmMoeSk7b2N0eC5iZWdpblBhdGgoKTtpZihzZXJpZXMucG9pbnRzLnN5bWJvbD09XCJjaXJjbGVcIilvY3R4LmFyYyh4LHkscmFkaXVzLDAsMipNYXRoLlBJLGZhbHNlKTtlbHNlIHNlcmllcy5wb2ludHMuc3ltYm9sKG9jdHgseCx5LHJhZGl1cyxmYWxzZSk7b2N0eC5jbG9zZVBhdGgoKTtvY3R4LnN0cm9rZSgpfWZ1bmN0aW9uIGRyYXdCYXJIaWdobGlnaHQoc2VyaWVzLHBvaW50KXt2YXIgaGlnaGxpZ2h0Q29sb3I9dHlwZW9mIHNlcmllcy5oaWdobGlnaHRDb2xvcj09PVwic3RyaW5nXCI/c2VyaWVzLmhpZ2hsaWdodENvbG9yOiQuY29sb3IucGFyc2Uoc2VyaWVzLmNvbG9yKS5zY2FsZShcImFcIiwuNSkudG9TdHJpbmcoKSxmaWxsU3R5bGU9aGlnaGxpZ2h0Q29sb3IsYmFyTGVmdDtzd2l0Y2goc2VyaWVzLmJhcnMuYWxpZ24pe2Nhc2VcImxlZnRcIjpiYXJMZWZ0PTA7YnJlYWs7Y2FzZVwicmlnaHRcIjpiYXJMZWZ0PS1zZXJpZXMuYmFycy5iYXJXaWR0aDticmVhaztkZWZhdWx0OmJhckxlZnQ9LXNlcmllcy5iYXJzLmJhcldpZHRoLzJ9b2N0eC5saW5lV2lkdGg9c2VyaWVzLmJhcnMubGluZVdpZHRoO29jdHguc3Ryb2tlU3R5bGU9aGlnaGxpZ2h0Q29sb3I7ZHJhd0Jhcihwb2ludFswXSxwb2ludFsxXSxwb2ludFsyXXx8MCxiYXJMZWZ0LGJhckxlZnQrc2VyaWVzLmJhcnMuYmFyV2lkdGgsZnVuY3Rpb24oKXtyZXR1cm4gZmlsbFN0eWxlfSxzZXJpZXMueGF4aXMsc2VyaWVzLnlheGlzLG9jdHgsc2VyaWVzLmJhcnMuaG9yaXpvbnRhbCxzZXJpZXMuYmFycy5saW5lV2lkdGgpfWZ1bmN0aW9uIGdldENvbG9yT3JHcmFkaWVudChzcGVjLGJvdHRvbSx0b3AsZGVmYXVsdENvbG9yKXtpZih0eXBlb2Ygc3BlYz09XCJzdHJpbmdcIilyZXR1cm4gc3BlYztlbHNle3ZhciBncmFkaWVudD1jdHguY3JlYXRlTGluZWFyR3JhZGllbnQoMCx0b3AsMCxib3R0b20pO2Zvcih2YXIgaT0wLGw9c3BlYy5jb2xvcnMubGVuZ3RoO2k8bDsrK2kpe3ZhciBjPXNwZWMuY29sb3JzW2ldO2lmKHR5cGVvZiBjIT1cInN0cmluZ1wiKXt2YXIgY289JC5jb2xvci5wYXJzZShkZWZhdWx0Q29sb3IpO2lmKGMuYnJpZ2h0bmVzcyE9bnVsbCljbz1jby5zY2FsZShcInJnYlwiLGMuYnJpZ2h0bmVzcyk7aWYoYy5vcGFjaXR5IT1udWxsKWNvLmEqPWMub3BhY2l0eTtjPWNvLnRvU3RyaW5nKCl9Z3JhZGllbnQuYWRkQ29sb3JTdG9wKGkvKGwtMSksYyl9cmV0dXJuIGdyYWRpZW50fX19JC5wbG90PWZ1bmN0aW9uKHBsYWNlaG9sZGVyLGRhdGEsb3B0aW9ucyl7dmFyIHBsb3Q9bmV3IFBsb3QoJChwbGFjZWhvbGRlciksZGF0YSxvcHRpb25zLCQucGxvdC5wbHVnaW5zKTtyZXR1cm4gcGxvdH07JC5wbG90LnZlcnNpb249XCIwLjguM1wiOyQucGxvdC5wbHVnaW5zPVtdOyQuZm4ucGxvdD1mdW5jdGlvbihkYXRhLG9wdGlvbnMpe3JldHVybiB0aGlzLmVhY2goZnVuY3Rpb24oKXskLnBsb3QodGhpcyxkYXRhLG9wdGlvbnMpfSl9O2Z1bmN0aW9uIGZsb29ySW5CYXNlKG4sYmFzZSl7cmV0dXJuIGJhc2UqTWF0aC5mbG9vcihuL2Jhc2UpfX0pKGpRdWVyeSk7IiwiLyogSmF2YXNjcmlwdCBwbG90dGluZyBsaWJyYXJ5IGZvciBqUXVlcnksIHZlcnNpb24gMC44LjMuXHJcblxyXG5Db3B5cmlnaHQgKGMpIDIwMDctMjAxNCBJT0xBIGFuZCBPbGUgTGF1cnNlbi5cclxuTGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlLlxyXG5cclxuKi9cclxuKGZ1bmN0aW9uKGEpe2Z1bmN0aW9uIGUoaCl7dmFyIGssaj10aGlzLGw9aC5kYXRhfHx7fTtpZihsLmVsZW0paj1oLmRyYWdUYXJnZXQ9bC5lbGVtLGguZHJhZ1Byb3h5PWQucHJveHl8fGosaC5jdXJzb3JPZmZzZXRYPWwucGFnZVgtbC5sZWZ0LGguY3Vyc29yT2Zmc2V0WT1sLnBhZ2VZLWwudG9wLGgub2Zmc2V0WD1oLnBhZ2VYLWguY3Vyc29yT2Zmc2V0WCxoLm9mZnNldFk9aC5wYWdlWS1oLmN1cnNvck9mZnNldFk7ZWxzZSBpZihkLmRyYWdnaW5nfHxsLndoaWNoPjAmJmgud2hpY2ghPWwud2hpY2h8fGEoaC50YXJnZXQpLmlzKGwubm90KSlyZXR1cm47c3dpdGNoKGgudHlwZSl7Y2FzZVwibW91c2Vkb3duXCI6cmV0dXJuIGEuZXh0ZW5kKGwsYShqKS5vZmZzZXQoKSx7ZWxlbTpqLHRhcmdldDpoLnRhcmdldCxwYWdlWDpoLnBhZ2VYLHBhZ2VZOmgucGFnZVl9KSxiLmFkZChkb2N1bWVudCxcIm1vdXNlbW92ZSBtb3VzZXVwXCIsZSxsKSxpKGosITEpLGQuZHJhZ2dpbmc9bnVsbCwhMTtjYXNlIWQuZHJhZ2dpbmcmJlwibW91c2Vtb3ZlXCI6aWYoZyhoLnBhZ2VYLWwucGFnZVgpK2coaC5wYWdlWS1sLnBhZ2VZKTxsLmRpc3RhbmNlKWJyZWFrO2gudGFyZ2V0PWwudGFyZ2V0LGs9ZihoLFwiZHJhZ3N0YXJ0XCIsaiksayE9PSExJiYoZC5kcmFnZ2luZz1qLGQucHJveHk9aC5kcmFnUHJveHk9YShrfHxqKVswXSk7Y2FzZVwibW91c2Vtb3ZlXCI6aWYoZC5kcmFnZ2luZyl7aWYoaz1mKGgsXCJkcmFnXCIsaiksYy5kcm9wJiYoYy5kcm9wLmFsbG93ZWQ9ayE9PSExLGMuZHJvcC5oYW5kbGVyKGgpKSxrIT09ITEpYnJlYWs7aC50eXBlPVwibW91c2V1cFwifWNhc2VcIm1vdXNldXBcIjpiLnJlbW92ZShkb2N1bWVudCxcIm1vdXNlbW92ZSBtb3VzZXVwXCIsZSksZC5kcmFnZ2luZyYmKGMuZHJvcCYmYy5kcm9wLmhhbmRsZXIoaCksZihoLFwiZHJhZ2VuZFwiLGopKSxpKGosITApLGQuZHJhZ2dpbmc9ZC5wcm94eT1sLmVsZW09ITF9cmV0dXJuITB9ZnVuY3Rpb24gZihiLGMsZCl7Yi50eXBlPWM7dmFyIGU9YS5ldmVudC5kaXNwYXRjaC5jYWxsKGQsYik7cmV0dXJuIGU9PT0hMT8hMTplfHxiLnJlc3VsdH1mdW5jdGlvbiBnKGEpe3JldHVybiBNYXRoLnBvdyhhLDIpfWZ1bmN0aW9uIGgoKXtyZXR1cm4gZC5kcmFnZ2luZz09PSExfWZ1bmN0aW9uIGkoYSxiKXthJiYoYS51bnNlbGVjdGFibGU9Yj9cIm9mZlwiOlwib25cIixhLm9uc2VsZWN0c3RhcnQ9ZnVuY3Rpb24oKXtyZXR1cm4gYn0sYS5zdHlsZSYmKGEuc3R5bGUuTW96VXNlclNlbGVjdD1iP1wiXCI6XCJub25lXCIpKX1hLmZuLmRyYWc9ZnVuY3Rpb24oYSxiLGMpe3JldHVybiBiJiZ0aGlzLmJpbmQoXCJkcmFnc3RhcnRcIixhKSxjJiZ0aGlzLmJpbmQoXCJkcmFnZW5kXCIsYyksYT90aGlzLmJpbmQoXCJkcmFnXCIsYj9iOmEpOnRoaXMudHJpZ2dlcihcImRyYWdcIil9O3ZhciBiPWEuZXZlbnQsYz1iLnNwZWNpYWwsZD1jLmRyYWc9e25vdDpcIjppbnB1dFwiLGRpc3RhbmNlOjAsd2hpY2g6MSxkcmFnZ2luZzohMSxzZXR1cDpmdW5jdGlvbihjKXtjPWEuZXh0ZW5kKHtkaXN0YW5jZTpkLmRpc3RhbmNlLHdoaWNoOmQud2hpY2gsbm90OmQubm90fSxjfHx7fSksYy5kaXN0YW5jZT1nKGMuZGlzdGFuY2UpLGIuYWRkKHRoaXMsXCJtb3VzZWRvd25cIixlLGMpLHRoaXMuYXR0YWNoRXZlbnQmJnRoaXMuYXR0YWNoRXZlbnQoXCJvbmRyYWdzdGFydFwiLGgpfSx0ZWFyZG93bjpmdW5jdGlvbigpe2IucmVtb3ZlKHRoaXMsXCJtb3VzZWRvd25cIixlKSx0aGlzPT09ZC5kcmFnZ2luZyYmKGQuZHJhZ2dpbmc9ZC5wcm94eT0hMSksaSh0aGlzLCEwKSx0aGlzLmRldGFjaEV2ZW50JiZ0aGlzLmRldGFjaEV2ZW50KFwib25kcmFnc3RhcnRcIixoKX19O2MuZHJhZ3N0YXJ0PWMuZHJhZ2VuZD17c2V0dXA6ZnVuY3Rpb24oKXt9LHRlYXJkb3duOmZ1bmN0aW9uKCl7fX19KShqUXVlcnkpOyhmdW5jdGlvbihkKXtmdW5jdGlvbiBlKGEpe3ZhciBiPWF8fHdpbmRvdy5ldmVudCxjPVtdLnNsaWNlLmNhbGwoYXJndW1lbnRzLDEpLGY9MCxlPTAsZz0wLGE9ZC5ldmVudC5maXgoYik7YS50eXBlPVwibW91c2V3aGVlbFwiO2Iud2hlZWxEZWx0YSYmKGY9Yi53aGVlbERlbHRhLzEyMCk7Yi5kZXRhaWwmJihmPS1iLmRldGFpbC8zKTtnPWY7dm9pZCAwIT09Yi5heGlzJiZiLmF4aXM9PT1iLkhPUklaT05UQUxfQVhJUyYmKGc9MCxlPS0xKmYpO3ZvaWQgMCE9PWIud2hlZWxEZWx0YVkmJihnPWIud2hlZWxEZWx0YVkvMTIwKTt2b2lkIDAhPT1iLndoZWVsRGVsdGFYJiYoZT0tMSpiLndoZWVsRGVsdGFYLzEyMCk7Yy51bnNoaWZ0KGEsZixlLGcpO3JldHVybihkLmV2ZW50LmRpc3BhdGNofHxkLmV2ZW50LmhhbmRsZSkuYXBwbHkodGhpcyxjKX12YXIgYz1bXCJET01Nb3VzZVNjcm9sbFwiLFwibW91c2V3aGVlbFwiXTtpZihkLmV2ZW50LmZpeEhvb2tzKWZvcih2YXIgaD1jLmxlbmd0aDtoOylkLmV2ZW50LmZpeEhvb2tzW2NbLS1oXV09ZC5ldmVudC5tb3VzZUhvb2tzO2QuZXZlbnQuc3BlY2lhbC5tb3VzZXdoZWVsPXtzZXR1cDpmdW5jdGlvbigpe2lmKHRoaXMuYWRkRXZlbnRMaXN0ZW5lcilmb3IodmFyIGE9Yy5sZW5ndGg7YTspdGhpcy5hZGRFdmVudExpc3RlbmVyKGNbLS1hXSxlLCExKTtlbHNlIHRoaXMub25tb3VzZXdoZWVsPWV9LHRlYXJkb3duOmZ1bmN0aW9uKCl7aWYodGhpcy5yZW1vdmVFdmVudExpc3RlbmVyKWZvcih2YXIgYT1jLmxlbmd0aDthOyl0aGlzLnJlbW92ZUV2ZW50TGlzdGVuZXIoY1stLWFdLGUsITEpO2Vsc2UgdGhpcy5vbm1vdXNld2hlZWw9bnVsbH19O2QuZm4uZXh0ZW5kKHttb3VzZXdoZWVsOmZ1bmN0aW9uKGEpe3JldHVybiBhP3RoaXMuYmluZChcIm1vdXNld2hlZWxcIixhKTp0aGlzLnRyaWdnZXIoXCJtb3VzZXdoZWVsXCIpfSx1bm1vdXNld2hlZWw6ZnVuY3Rpb24oYSl7cmV0dXJuIHRoaXMudW5iaW5kKFwibW91c2V3aGVlbFwiLGEpfX0pfSkoalF1ZXJ5KTsoZnVuY3Rpb24oJCl7dmFyIG9wdGlvbnM9e3hheGlzOnt6b29tUmFuZ2U6bnVsbCxwYW5SYW5nZTpudWxsfSx6b29tOntpbnRlcmFjdGl2ZTpmYWxzZSx0cmlnZ2VyOlwiZGJsY2xpY2tcIixhbW91bnQ6MS41fSxwYW46e2ludGVyYWN0aXZlOmZhbHNlLGN1cnNvcjpcIm1vdmVcIixmcmFtZVJhdGU6MjB9fTtmdW5jdGlvbiBpbml0KHBsb3Qpe2Z1bmN0aW9uIG9uWm9vbUNsaWNrKGUsem9vbU91dCl7dmFyIGM9cGxvdC5vZmZzZXQoKTtjLmxlZnQ9ZS5wYWdlWC1jLmxlZnQ7Yy50b3A9ZS5wYWdlWS1jLnRvcDtpZih6b29tT3V0KXBsb3Quem9vbU91dCh7Y2VudGVyOmN9KTtlbHNlIHBsb3Quem9vbSh7Y2VudGVyOmN9KX1mdW5jdGlvbiBvbk1vdXNlV2hlZWwoZSxkZWx0YSl7ZS5wcmV2ZW50RGVmYXVsdCgpO29uWm9vbUNsaWNrKGUsZGVsdGE8MCk7cmV0dXJuIGZhbHNlfXZhciBwcmV2Q3Vyc29yPVwiZGVmYXVsdFwiLHByZXZQYWdlWD0wLHByZXZQYWdlWT0wLHBhblRpbWVvdXQ9bnVsbDtmdW5jdGlvbiBvbkRyYWdTdGFydChlKXtpZihlLndoaWNoIT0xKXJldHVybiBmYWxzZTt2YXIgYz1wbG90LmdldFBsYWNlaG9sZGVyKCkuY3NzKFwiY3Vyc29yXCIpO2lmKGMpcHJldkN1cnNvcj1jO3Bsb3QuZ2V0UGxhY2Vob2xkZXIoKS5jc3MoXCJjdXJzb3JcIixwbG90LmdldE9wdGlvbnMoKS5wYW4uY3Vyc29yKTtwcmV2UGFnZVg9ZS5wYWdlWDtwcmV2UGFnZVk9ZS5wYWdlWX1mdW5jdGlvbiBvbkRyYWcoZSl7dmFyIGZyYW1lUmF0ZT1wbG90LmdldE9wdGlvbnMoKS5wYW4uZnJhbWVSYXRlO2lmKHBhblRpbWVvdXR8fCFmcmFtZVJhdGUpcmV0dXJuO3BhblRpbWVvdXQ9c2V0VGltZW91dChmdW5jdGlvbigpe3Bsb3QucGFuKHtsZWZ0OnByZXZQYWdlWC1lLnBhZ2VYLHRvcDpwcmV2UGFnZVktZS5wYWdlWX0pO3ByZXZQYWdlWD1lLnBhZ2VYO3ByZXZQYWdlWT1lLnBhZ2VZO3BhblRpbWVvdXQ9bnVsbH0sMS9mcmFtZVJhdGUqMWUzKX1mdW5jdGlvbiBvbkRyYWdFbmQoZSl7aWYocGFuVGltZW91dCl7Y2xlYXJUaW1lb3V0KHBhblRpbWVvdXQpO3BhblRpbWVvdXQ9bnVsbH1wbG90LmdldFBsYWNlaG9sZGVyKCkuY3NzKFwiY3Vyc29yXCIscHJldkN1cnNvcik7cGxvdC5wYW4oe2xlZnQ6cHJldlBhZ2VYLWUucGFnZVgsdG9wOnByZXZQYWdlWS1lLnBhZ2VZfSl9ZnVuY3Rpb24gYmluZEV2ZW50cyhwbG90LGV2ZW50SG9sZGVyKXt2YXIgbz1wbG90LmdldE9wdGlvbnMoKTtpZihvLnpvb20uaW50ZXJhY3RpdmUpe2V2ZW50SG9sZGVyW28uem9vbS50cmlnZ2VyXShvblpvb21DbGljayk7ZXZlbnRIb2xkZXIubW91c2V3aGVlbChvbk1vdXNlV2hlZWwpfWlmKG8ucGFuLmludGVyYWN0aXZlKXtldmVudEhvbGRlci5iaW5kKFwiZHJhZ3N0YXJ0XCIse2Rpc3RhbmNlOjEwfSxvbkRyYWdTdGFydCk7ZXZlbnRIb2xkZXIuYmluZChcImRyYWdcIixvbkRyYWcpO2V2ZW50SG9sZGVyLmJpbmQoXCJkcmFnZW5kXCIsb25EcmFnRW5kKX19cGxvdC56b29tT3V0PWZ1bmN0aW9uKGFyZ3Mpe2lmKCFhcmdzKWFyZ3M9e307aWYoIWFyZ3MuYW1vdW50KWFyZ3MuYW1vdW50PXBsb3QuZ2V0T3B0aW9ucygpLnpvb20uYW1vdW50O2FyZ3MuYW1vdW50PTEvYXJncy5hbW91bnQ7cGxvdC56b29tKGFyZ3MpfTtwbG90Lnpvb209ZnVuY3Rpb24oYXJncyl7aWYoIWFyZ3MpYXJncz17fTt2YXIgYz1hcmdzLmNlbnRlcixhbW91bnQ9YXJncy5hbW91bnR8fHBsb3QuZ2V0T3B0aW9ucygpLnpvb20uYW1vdW50LHc9cGxvdC53aWR0aCgpLGg9cGxvdC5oZWlnaHQoKTtpZighYyljPXtsZWZ0OncvMix0b3A6aC8yfTt2YXIgeGY9Yy5sZWZ0L3cseWY9Yy50b3AvaCxtaW5tYXg9e3g6e21pbjpjLmxlZnQteGYqdy9hbW91bnQsbWF4OmMubGVmdCsoMS14Zikqdy9hbW91bnR9LHk6e21pbjpjLnRvcC15ZipoL2Ftb3VudCxtYXg6Yy50b3ArKDEteWYpKmgvYW1vdW50fX07JC5lYWNoKHBsb3QuZ2V0QXhlcygpLGZ1bmN0aW9uKF8sYXhpcyl7dmFyIG9wdHM9YXhpcy5vcHRpb25zLG1pbj1taW5tYXhbYXhpcy5kaXJlY3Rpb25dLm1pbixtYXg9bWlubWF4W2F4aXMuZGlyZWN0aW9uXS5tYXgsenI9b3B0cy56b29tUmFuZ2UscHI9b3B0cy5wYW5SYW5nZTtpZih6cj09PWZhbHNlKXJldHVybjttaW49YXhpcy5jMnAobWluKTttYXg9YXhpcy5jMnAobWF4KTtpZihtaW4+bWF4KXt2YXIgdG1wPW1pbjttaW49bWF4O21heD10bXB9aWYocHIpe2lmKHByWzBdIT1udWxsJiZtaW48cHJbMF0pe21pbj1wclswXX1pZihwclsxXSE9bnVsbCYmbWF4PnByWzFdKXttYXg9cHJbMV19fXZhciByYW5nZT1tYXgtbWluO2lmKHpyJiYoenJbMF0hPW51bGwmJnJhbmdlPHpyWzBdJiZhbW91bnQ+MXx8enJbMV0hPW51bGwmJnJhbmdlPnpyWzFdJiZhbW91bnQ8MSkpcmV0dXJuO29wdHMubWluPW1pbjtvcHRzLm1heD1tYXh9KTtwbG90LnNldHVwR3JpZCgpO3Bsb3QuZHJhdygpO2lmKCFhcmdzLnByZXZlbnRFdmVudClwbG90LmdldFBsYWNlaG9sZGVyKCkudHJpZ2dlcihcInBsb3R6b29tXCIsW3Bsb3QsYXJnc10pfTtwbG90LnBhbj1mdW5jdGlvbihhcmdzKXt2YXIgZGVsdGE9e3g6K2FyZ3MubGVmdCx5OithcmdzLnRvcH07aWYoaXNOYU4oZGVsdGEueCkpZGVsdGEueD0wO2lmKGlzTmFOKGRlbHRhLnkpKWRlbHRhLnk9MDskLmVhY2gocGxvdC5nZXRBeGVzKCksZnVuY3Rpb24oXyxheGlzKXt2YXIgb3B0cz1heGlzLm9wdGlvbnMsbWluLG1heCxkPWRlbHRhW2F4aXMuZGlyZWN0aW9uXTttaW49YXhpcy5jMnAoYXhpcy5wMmMoYXhpcy5taW4pK2QpLG1heD1heGlzLmMycChheGlzLnAyYyhheGlzLm1heCkrZCk7dmFyIHByPW9wdHMucGFuUmFuZ2U7aWYocHI9PT1mYWxzZSlyZXR1cm47aWYocHIpe2lmKHByWzBdIT1udWxsJiZwclswXT5taW4pe2Q9cHJbMF0tbWluO21pbis9ZDttYXgrPWR9aWYocHJbMV0hPW51bGwmJnByWzFdPG1heCl7ZD1wclsxXS1tYXg7bWluKz1kO21heCs9ZH19b3B0cy5taW49bWluO29wdHMubWF4PW1heH0pO3Bsb3Quc2V0dXBHcmlkKCk7cGxvdC5kcmF3KCk7aWYoIWFyZ3MucHJldmVudEV2ZW50KXBsb3QuZ2V0UGxhY2Vob2xkZXIoKS50cmlnZ2VyKFwicGxvdHBhblwiLFtwbG90LGFyZ3NdKX07ZnVuY3Rpb24gc2h1dGRvd24ocGxvdCxldmVudEhvbGRlcil7ZXZlbnRIb2xkZXIudW5iaW5kKHBsb3QuZ2V0T3B0aW9ucygpLnpvb20udHJpZ2dlcixvblpvb21DbGljayk7ZXZlbnRIb2xkZXIudW5iaW5kKFwibW91c2V3aGVlbFwiLG9uTW91c2VXaGVlbCk7ZXZlbnRIb2xkZXIudW5iaW5kKFwiZHJhZ3N0YXJ0XCIsb25EcmFnU3RhcnQpO2V2ZW50SG9sZGVyLnVuYmluZChcImRyYWdcIixvbkRyYWcpO2V2ZW50SG9sZGVyLnVuYmluZChcImRyYWdlbmRcIixvbkRyYWdFbmQpO2lmKHBhblRpbWVvdXQpY2xlYXJUaW1lb3V0KHBhblRpbWVvdXQpfXBsb3QuaG9va3MuYmluZEV2ZW50cy5wdXNoKGJpbmRFdmVudHMpO3Bsb3QuaG9va3Muc2h1dGRvd24ucHVzaChzaHV0ZG93bil9JC5wbG90LnBsdWdpbnMucHVzaCh7aW5pdDppbml0LG9wdGlvbnM6b3B0aW9ucyxuYW1lOlwibmF2aWdhdGVcIix2ZXJzaW9uOlwiMS4zXCJ9KX0pKGpRdWVyeSk7IiwiLyogSmF2YXNjcmlwdCBwbG90dGluZyBsaWJyYXJ5IGZvciBqUXVlcnksIHZlcnNpb24gMC44LjMuXHJcblxyXG5Db3B5cmlnaHQgKGMpIDIwMDctMjAxNCBJT0xBIGFuZCBPbGUgTGF1cnNlbi5cclxuTGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlLlxyXG5cclxuKi9cclxuKGZ1bmN0aW9uKCQpe2Z1bmN0aW9uIGluaXQocGxvdCl7dmFyIHNlbGVjdGlvbj17Zmlyc3Q6e3g6LTEseTotMX0sc2Vjb25kOnt4Oi0xLHk6LTF9LHNob3c6ZmFsc2UsYWN0aXZlOmZhbHNlfTt2YXIgc2F2ZWRoYW5kbGVycz17fTt2YXIgbW91c2VVcEhhbmRsZXI9bnVsbDtmdW5jdGlvbiBvbk1vdXNlTW92ZShlKXtpZihzZWxlY3Rpb24uYWN0aXZlKXt1cGRhdGVTZWxlY3Rpb24oZSk7cGxvdC5nZXRQbGFjZWhvbGRlcigpLnRyaWdnZXIoXCJwbG90c2VsZWN0aW5nXCIsW2dldFNlbGVjdGlvbigpXSl9fWZ1bmN0aW9uIG9uTW91c2VEb3duKGUpe2lmKGUud2hpY2ghPTEpcmV0dXJuO2RvY3VtZW50LmJvZHkuZm9jdXMoKTtpZihkb2N1bWVudC5vbnNlbGVjdHN0YXJ0IT09dW5kZWZpbmVkJiZzYXZlZGhhbmRsZXJzLm9uc2VsZWN0c3RhcnQ9PW51bGwpe3NhdmVkaGFuZGxlcnMub25zZWxlY3RzdGFydD1kb2N1bWVudC5vbnNlbGVjdHN0YXJ0O2RvY3VtZW50Lm9uc2VsZWN0c3RhcnQ9ZnVuY3Rpb24oKXtyZXR1cm4gZmFsc2V9fWlmKGRvY3VtZW50Lm9uZHJhZyE9PXVuZGVmaW5lZCYmc2F2ZWRoYW5kbGVycy5vbmRyYWc9PW51bGwpe3NhdmVkaGFuZGxlcnMub25kcmFnPWRvY3VtZW50Lm9uZHJhZztkb2N1bWVudC5vbmRyYWc9ZnVuY3Rpb24oKXtyZXR1cm4gZmFsc2V9fXNldFNlbGVjdGlvblBvcyhzZWxlY3Rpb24uZmlyc3QsZSk7c2VsZWN0aW9uLmFjdGl2ZT10cnVlO21vdXNlVXBIYW5kbGVyPWZ1bmN0aW9uKGUpe29uTW91c2VVcChlKX07JChkb2N1bWVudCkub25lKFwibW91c2V1cFwiLG1vdXNlVXBIYW5kbGVyKX1mdW5jdGlvbiBvbk1vdXNlVXAoZSl7bW91c2VVcEhhbmRsZXI9bnVsbDtpZihkb2N1bWVudC5vbnNlbGVjdHN0YXJ0IT09dW5kZWZpbmVkKWRvY3VtZW50Lm9uc2VsZWN0c3RhcnQ9c2F2ZWRoYW5kbGVycy5vbnNlbGVjdHN0YXJ0O2lmKGRvY3VtZW50Lm9uZHJhZyE9PXVuZGVmaW5lZClkb2N1bWVudC5vbmRyYWc9c2F2ZWRoYW5kbGVycy5vbmRyYWc7c2VsZWN0aW9uLmFjdGl2ZT1mYWxzZTt1cGRhdGVTZWxlY3Rpb24oZSk7aWYoc2VsZWN0aW9uSXNTYW5lKCkpdHJpZ2dlclNlbGVjdGVkRXZlbnQoKTtlbHNle3Bsb3QuZ2V0UGxhY2Vob2xkZXIoKS50cmlnZ2VyKFwicGxvdHVuc2VsZWN0ZWRcIixbXSk7cGxvdC5nZXRQbGFjZWhvbGRlcigpLnRyaWdnZXIoXCJwbG90c2VsZWN0aW5nXCIsW251bGxdKX1yZXR1cm4gZmFsc2V9ZnVuY3Rpb24gZ2V0U2VsZWN0aW9uKCl7aWYoIXNlbGVjdGlvbklzU2FuZSgpKXJldHVybiBudWxsO2lmKCFzZWxlY3Rpb24uc2hvdylyZXR1cm4gbnVsbDt2YXIgcj17fSxjMT1zZWxlY3Rpb24uZmlyc3QsYzI9c2VsZWN0aW9uLnNlY29uZDskLmVhY2gocGxvdC5nZXRBeGVzKCksZnVuY3Rpb24obmFtZSxheGlzKXtpZihheGlzLnVzZWQpe3ZhciBwMT1heGlzLmMycChjMVtheGlzLmRpcmVjdGlvbl0pLHAyPWF4aXMuYzJwKGMyW2F4aXMuZGlyZWN0aW9uXSk7cltuYW1lXT17ZnJvbTpNYXRoLm1pbihwMSxwMiksdG86TWF0aC5tYXgocDEscDIpfX19KTtyZXR1cm4gcn1mdW5jdGlvbiB0cmlnZ2VyU2VsZWN0ZWRFdmVudCgpe3ZhciByPWdldFNlbGVjdGlvbigpO3Bsb3QuZ2V0UGxhY2Vob2xkZXIoKS50cmlnZ2VyKFwicGxvdHNlbGVjdGVkXCIsW3JdKTtpZihyLnhheGlzJiZyLnlheGlzKXBsb3QuZ2V0UGxhY2Vob2xkZXIoKS50cmlnZ2VyKFwic2VsZWN0ZWRcIixbe3gxOnIueGF4aXMuZnJvbSx5MTpyLnlheGlzLmZyb20seDI6ci54YXhpcy50byx5MjpyLnlheGlzLnRvfV0pfWZ1bmN0aW9uIGNsYW1wKG1pbix2YWx1ZSxtYXgpe3JldHVybiB2YWx1ZTxtaW4/bWluOnZhbHVlPm1heD9tYXg6dmFsdWV9ZnVuY3Rpb24gc2V0U2VsZWN0aW9uUG9zKHBvcyxlKXt2YXIgbz1wbG90LmdldE9wdGlvbnMoKTt2YXIgb2Zmc2V0PXBsb3QuZ2V0UGxhY2Vob2xkZXIoKS5vZmZzZXQoKTt2YXIgcGxvdE9mZnNldD1wbG90LmdldFBsb3RPZmZzZXQoKTtwb3MueD1jbGFtcCgwLGUucGFnZVgtb2Zmc2V0LmxlZnQtcGxvdE9mZnNldC5sZWZ0LHBsb3Qud2lkdGgoKSk7cG9zLnk9Y2xhbXAoMCxlLnBhZ2VZLW9mZnNldC50b3AtcGxvdE9mZnNldC50b3AscGxvdC5oZWlnaHQoKSk7aWYoby5zZWxlY3Rpb24ubW9kZT09XCJ5XCIpcG9zLng9cG9zPT1zZWxlY3Rpb24uZmlyc3Q/MDpwbG90LndpZHRoKCk7aWYoby5zZWxlY3Rpb24ubW9kZT09XCJ4XCIpcG9zLnk9cG9zPT1zZWxlY3Rpb24uZmlyc3Q/MDpwbG90LmhlaWdodCgpfWZ1bmN0aW9uIHVwZGF0ZVNlbGVjdGlvbihwb3Mpe2lmKHBvcy5wYWdlWD09bnVsbClyZXR1cm47c2V0U2VsZWN0aW9uUG9zKHNlbGVjdGlvbi5zZWNvbmQscG9zKTtpZihzZWxlY3Rpb25Jc1NhbmUoKSl7c2VsZWN0aW9uLnNob3c9dHJ1ZTtwbG90LnRyaWdnZXJSZWRyYXdPdmVybGF5KCl9ZWxzZSBjbGVhclNlbGVjdGlvbih0cnVlKX1mdW5jdGlvbiBjbGVhclNlbGVjdGlvbihwcmV2ZW50RXZlbnQpe2lmKHNlbGVjdGlvbi5zaG93KXtzZWxlY3Rpb24uc2hvdz1mYWxzZTtwbG90LnRyaWdnZXJSZWRyYXdPdmVybGF5KCk7aWYoIXByZXZlbnRFdmVudClwbG90LmdldFBsYWNlaG9sZGVyKCkudHJpZ2dlcihcInBsb3R1bnNlbGVjdGVkXCIsW10pfX1mdW5jdGlvbiBleHRyYWN0UmFuZ2UocmFuZ2VzLGNvb3JkKXt2YXIgYXhpcyxmcm9tLHRvLGtleSxheGVzPXBsb3QuZ2V0QXhlcygpO2Zvcih2YXIgayBpbiBheGVzKXtheGlzPWF4ZXNba107aWYoYXhpcy5kaXJlY3Rpb249PWNvb3JkKXtrZXk9Y29vcmQrYXhpcy5uK1wiYXhpc1wiO2lmKCFyYW5nZXNba2V5XSYmYXhpcy5uPT0xKWtleT1jb29yZCtcImF4aXNcIjtpZihyYW5nZXNba2V5XSl7ZnJvbT1yYW5nZXNba2V5XS5mcm9tO3RvPXJhbmdlc1trZXldLnRvO2JyZWFrfX19aWYoIXJhbmdlc1trZXldKXtheGlzPWNvb3JkPT1cInhcIj9wbG90LmdldFhBeGVzKClbMF06cGxvdC5nZXRZQXhlcygpWzBdO2Zyb209cmFuZ2VzW2Nvb3JkK1wiMVwiXTt0bz1yYW5nZXNbY29vcmQrXCIyXCJdfWlmKGZyb20hPW51bGwmJnRvIT1udWxsJiZmcm9tPnRvKXt2YXIgdG1wPWZyb207ZnJvbT10bzt0bz10bXB9cmV0dXJue2Zyb206ZnJvbSx0bzp0byxheGlzOmF4aXN9fWZ1bmN0aW9uIHNldFNlbGVjdGlvbihyYW5nZXMscHJldmVudEV2ZW50KXt2YXIgYXhpcyxyYW5nZSxvPXBsb3QuZ2V0T3B0aW9ucygpO2lmKG8uc2VsZWN0aW9uLm1vZGU9PVwieVwiKXtzZWxlY3Rpb24uZmlyc3QueD0wO3NlbGVjdGlvbi5zZWNvbmQueD1wbG90LndpZHRoKCl9ZWxzZXtyYW5nZT1leHRyYWN0UmFuZ2UocmFuZ2VzLFwieFwiKTtzZWxlY3Rpb24uZmlyc3QueD1yYW5nZS5heGlzLnAyYyhyYW5nZS5mcm9tKTtzZWxlY3Rpb24uc2Vjb25kLng9cmFuZ2UuYXhpcy5wMmMocmFuZ2UudG8pfWlmKG8uc2VsZWN0aW9uLm1vZGU9PVwieFwiKXtzZWxlY3Rpb24uZmlyc3QueT0wO3NlbGVjdGlvbi5zZWNvbmQueT1wbG90LmhlaWdodCgpfWVsc2V7cmFuZ2U9ZXh0cmFjdFJhbmdlKHJhbmdlcyxcInlcIik7c2VsZWN0aW9uLmZpcnN0Lnk9cmFuZ2UuYXhpcy5wMmMocmFuZ2UuZnJvbSk7c2VsZWN0aW9uLnNlY29uZC55PXJhbmdlLmF4aXMucDJjKHJhbmdlLnRvKX1zZWxlY3Rpb24uc2hvdz10cnVlO3Bsb3QudHJpZ2dlclJlZHJhd092ZXJsYXkoKTtpZighcHJldmVudEV2ZW50JiZzZWxlY3Rpb25Jc1NhbmUoKSl0cmlnZ2VyU2VsZWN0ZWRFdmVudCgpfWZ1bmN0aW9uIHNlbGVjdGlvbklzU2FuZSgpe3ZhciBtaW5TaXplPXBsb3QuZ2V0T3B0aW9ucygpLnNlbGVjdGlvbi5taW5TaXplO3JldHVybiBNYXRoLmFicyhzZWxlY3Rpb24uc2Vjb25kLngtc2VsZWN0aW9uLmZpcnN0LngpPj1taW5TaXplJiZNYXRoLmFicyhzZWxlY3Rpb24uc2Vjb25kLnktc2VsZWN0aW9uLmZpcnN0LnkpPj1taW5TaXplfXBsb3QuY2xlYXJTZWxlY3Rpb249Y2xlYXJTZWxlY3Rpb247cGxvdC5zZXRTZWxlY3Rpb249c2V0U2VsZWN0aW9uO3Bsb3QuZ2V0U2VsZWN0aW9uPWdldFNlbGVjdGlvbjtwbG90Lmhvb2tzLmJpbmRFdmVudHMucHVzaChmdW5jdGlvbihwbG90LGV2ZW50SG9sZGVyKXt2YXIgbz1wbG90LmdldE9wdGlvbnMoKTtpZihvLnNlbGVjdGlvbi5tb2RlIT1udWxsKXtldmVudEhvbGRlci5tb3VzZW1vdmUob25Nb3VzZU1vdmUpO2V2ZW50SG9sZGVyLm1vdXNlZG93bihvbk1vdXNlRG93bil9fSk7cGxvdC5ob29rcy5kcmF3T3ZlcmxheS5wdXNoKGZ1bmN0aW9uKHBsb3QsY3R4KXtpZihzZWxlY3Rpb24uc2hvdyYmc2VsZWN0aW9uSXNTYW5lKCkpe3ZhciBwbG90T2Zmc2V0PXBsb3QuZ2V0UGxvdE9mZnNldCgpO3ZhciBvPXBsb3QuZ2V0T3B0aW9ucygpO2N0eC5zYXZlKCk7Y3R4LnRyYW5zbGF0ZShwbG90T2Zmc2V0LmxlZnQscGxvdE9mZnNldC50b3ApO3ZhciBjPSQuY29sb3IucGFyc2Uoby5zZWxlY3Rpb24uY29sb3IpO2N0eC5zdHJva2VTdHlsZT1jLnNjYWxlKFwiYVwiLC44KS50b1N0cmluZygpO2N0eC5saW5lV2lkdGg9MTtjdHgubGluZUpvaW49by5zZWxlY3Rpb24uc2hhcGU7Y3R4LmZpbGxTdHlsZT1jLnNjYWxlKFwiYVwiLC40KS50b1N0cmluZygpO3ZhciB4PU1hdGgubWluKHNlbGVjdGlvbi5maXJzdC54LHNlbGVjdGlvbi5zZWNvbmQueCkrLjUseT1NYXRoLm1pbihzZWxlY3Rpb24uZmlyc3QueSxzZWxlY3Rpb24uc2Vjb25kLnkpKy41LHc9TWF0aC5hYnMoc2VsZWN0aW9uLnNlY29uZC54LXNlbGVjdGlvbi5maXJzdC54KS0xLGg9TWF0aC5hYnMoc2VsZWN0aW9uLnNlY29uZC55LXNlbGVjdGlvbi5maXJzdC55KS0xO2N0eC5maWxsUmVjdCh4LHksdyxoKTtjdHguc3Ryb2tlUmVjdCh4LHksdyxoKTtjdHgucmVzdG9yZSgpfX0pO3Bsb3QuaG9va3Muc2h1dGRvd24ucHVzaChmdW5jdGlvbihwbG90LGV2ZW50SG9sZGVyKXtldmVudEhvbGRlci51bmJpbmQoXCJtb3VzZW1vdmVcIixvbk1vdXNlTW92ZSk7ZXZlbnRIb2xkZXIudW5iaW5kKFwibW91c2Vkb3duXCIsb25Nb3VzZURvd24pO2lmKG1vdXNlVXBIYW5kbGVyKSQoZG9jdW1lbnQpLnVuYmluZChcIm1vdXNldXBcIixtb3VzZVVwSGFuZGxlcil9KX0kLnBsb3QucGx1Z2lucy5wdXNoKHtpbml0OmluaXQsb3B0aW9uczp7c2VsZWN0aW9uOnttb2RlOm51bGwsY29sb3I6XCIjZThjZmFjXCIsc2hhcGU6XCJyb3VuZFwiLG1pblNpemU6NX19LG5hbWU6XCJzZWxlY3Rpb25cIix2ZXJzaW9uOlwiMS4xXCJ9KX0pKGpRdWVyeSk7IiwiLyogSmF2YXNjcmlwdCBwbG90dGluZyBsaWJyYXJ5IGZvciBqUXVlcnksIHZlcnNpb24gMC44LjMuXHJcblxyXG5Db3B5cmlnaHQgKGMpIDIwMDctMjAxNCBJT0xBIGFuZCBPbGUgTGF1cnNlbi5cclxuTGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlLlxyXG5cclxuKi9cclxuKGZ1bmN0aW9uKCQpe3ZhciBvcHRpb25zPXt4YXhpczp7dGltZXpvbmU6bnVsbCx0aW1lZm9ybWF0Om51bGwsdHdlbHZlSG91ckNsb2NrOmZhbHNlLG1vbnRoTmFtZXM6bnVsbH19O2Z1bmN0aW9uIGZsb29ySW5CYXNlKG4sYmFzZSl7cmV0dXJuIGJhc2UqTWF0aC5mbG9vcihuL2Jhc2UpfWZ1bmN0aW9uIGZvcm1hdERhdGUoZCxmbXQsbW9udGhOYW1lcyxkYXlOYW1lcyl7aWYodHlwZW9mIGQuc3RyZnRpbWU9PVwiZnVuY3Rpb25cIil7cmV0dXJuIGQuc3RyZnRpbWUoZm10KX12YXIgbGVmdFBhZD1mdW5jdGlvbihuLHBhZCl7bj1cIlwiK247cGFkPVwiXCIrKHBhZD09bnVsbD9cIjBcIjpwYWQpO3JldHVybiBuLmxlbmd0aD09MT9wYWQrbjpufTt2YXIgcj1bXTt2YXIgZXNjYXBlPWZhbHNlO3ZhciBob3Vycz1kLmdldEhvdXJzKCk7dmFyIGlzQU09aG91cnM8MTI7aWYobW9udGhOYW1lcz09bnVsbCl7bW9udGhOYW1lcz1bXCJKYW5cIixcIkZlYlwiLFwiTWFyXCIsXCJBcHJcIixcIk1heVwiLFwiSnVuXCIsXCJKdWxcIixcIkF1Z1wiLFwiU2VwXCIsXCJPY3RcIixcIk5vdlwiLFwiRGVjXCJdfWlmKGRheU5hbWVzPT1udWxsKXtkYXlOYW1lcz1bXCJTdW5cIixcIk1vblwiLFwiVHVlXCIsXCJXZWRcIixcIlRodVwiLFwiRnJpXCIsXCJTYXRcIl19dmFyIGhvdXJzMTI7aWYoaG91cnM+MTIpe2hvdXJzMTI9aG91cnMtMTJ9ZWxzZSBpZihob3Vycz09MCl7aG91cnMxMj0xMn1lbHNle2hvdXJzMTI9aG91cnN9Zm9yKHZhciBpPTA7aTxmbXQubGVuZ3RoOysraSl7dmFyIGM9Zm10LmNoYXJBdChpKTtpZihlc2NhcGUpe3N3aXRjaChjKXtjYXNlXCJhXCI6Yz1cIlwiK2RheU5hbWVzW2QuZ2V0RGF5KCldO2JyZWFrO2Nhc2VcImJcIjpjPVwiXCIrbW9udGhOYW1lc1tkLmdldE1vbnRoKCldO2JyZWFrO2Nhc2VcImRcIjpjPWxlZnRQYWQoZC5nZXREYXRlKCkpO2JyZWFrO2Nhc2VcImVcIjpjPWxlZnRQYWQoZC5nZXREYXRlKCksXCIgXCIpO2JyZWFrO2Nhc2VcImhcIjpjYXNlXCJIXCI6Yz1sZWZ0UGFkKGhvdXJzKTticmVhaztjYXNlXCJJXCI6Yz1sZWZ0UGFkKGhvdXJzMTIpO2JyZWFrO2Nhc2VcImxcIjpjPWxlZnRQYWQoaG91cnMxMixcIiBcIik7YnJlYWs7Y2FzZVwibVwiOmM9bGVmdFBhZChkLmdldE1vbnRoKCkrMSk7YnJlYWs7Y2FzZVwiTVwiOmM9bGVmdFBhZChkLmdldE1pbnV0ZXMoKSk7YnJlYWs7Y2FzZVwicVwiOmM9XCJcIisoTWF0aC5mbG9vcihkLmdldE1vbnRoKCkvMykrMSk7YnJlYWs7Y2FzZVwiU1wiOmM9bGVmdFBhZChkLmdldFNlY29uZHMoKSk7YnJlYWs7Y2FzZVwieVwiOmM9bGVmdFBhZChkLmdldEZ1bGxZZWFyKCklMTAwKTticmVhaztjYXNlXCJZXCI6Yz1cIlwiK2QuZ2V0RnVsbFllYXIoKTticmVhaztjYXNlXCJwXCI6Yz1pc0FNP1wiXCIrXCJhbVwiOlwiXCIrXCJwbVwiO2JyZWFrO2Nhc2VcIlBcIjpjPWlzQU0/XCJcIitcIkFNXCI6XCJcIitcIlBNXCI7YnJlYWs7Y2FzZVwid1wiOmM9XCJcIitkLmdldERheSgpO2JyZWFrfXIucHVzaChjKTtlc2NhcGU9ZmFsc2V9ZWxzZXtpZihjPT1cIiVcIil7ZXNjYXBlPXRydWV9ZWxzZXtyLnB1c2goYyl9fX1yZXR1cm4gci5qb2luKFwiXCIpfWZ1bmN0aW9uIG1ha2VVdGNXcmFwcGVyKGQpe2Z1bmN0aW9uIGFkZFByb3h5TWV0aG9kKHNvdXJjZU9iaixzb3VyY2VNZXRob2QsdGFyZ2V0T2JqLHRhcmdldE1ldGhvZCl7c291cmNlT2JqW3NvdXJjZU1ldGhvZF09ZnVuY3Rpb24oKXtyZXR1cm4gdGFyZ2V0T2JqW3RhcmdldE1ldGhvZF0uYXBwbHkodGFyZ2V0T2JqLGFyZ3VtZW50cyl9fXZhciB1dGM9e2RhdGU6ZH07aWYoZC5zdHJmdGltZSE9dW5kZWZpbmVkKXthZGRQcm94eU1ldGhvZCh1dGMsXCJzdHJmdGltZVwiLGQsXCJzdHJmdGltZVwiKX1hZGRQcm94eU1ldGhvZCh1dGMsXCJnZXRUaW1lXCIsZCxcImdldFRpbWVcIik7YWRkUHJveHlNZXRob2QodXRjLFwic2V0VGltZVwiLGQsXCJzZXRUaW1lXCIpO3ZhciBwcm9wcz1bXCJEYXRlXCIsXCJEYXlcIixcIkZ1bGxZZWFyXCIsXCJIb3Vyc1wiLFwiTWlsbGlzZWNvbmRzXCIsXCJNaW51dGVzXCIsXCJNb250aFwiLFwiU2Vjb25kc1wiXTtmb3IodmFyIHA9MDtwPHByb3BzLmxlbmd0aDtwKyspe2FkZFByb3h5TWV0aG9kKHV0YyxcImdldFwiK3Byb3BzW3BdLGQsXCJnZXRVVENcIitwcm9wc1twXSk7YWRkUHJveHlNZXRob2QodXRjLFwic2V0XCIrcHJvcHNbcF0sZCxcInNldFVUQ1wiK3Byb3BzW3BdKX1yZXR1cm4gdXRjfWZ1bmN0aW9uIGRhdGVHZW5lcmF0b3IodHMsb3B0cyl7aWYob3B0cy50aW1lem9uZT09XCJicm93c2VyXCIpe3JldHVybiBuZXcgRGF0ZSh0cyl9ZWxzZSBpZighb3B0cy50aW1lem9uZXx8b3B0cy50aW1lem9uZT09XCJ1dGNcIil7cmV0dXJuIG1ha2VVdGNXcmFwcGVyKG5ldyBEYXRlKHRzKSl9ZWxzZSBpZih0eXBlb2YgdGltZXpvbmVKUyE9XCJ1bmRlZmluZWRcIiYmdHlwZW9mIHRpbWV6b25lSlMuRGF0ZSE9XCJ1bmRlZmluZWRcIil7dmFyIGQ9bmV3IHRpbWV6b25lSlMuRGF0ZTtkLnNldFRpbWV6b25lKG9wdHMudGltZXpvbmUpO2Quc2V0VGltZSh0cyk7cmV0dXJuIGR9ZWxzZXtyZXR1cm4gbWFrZVV0Y1dyYXBwZXIobmV3IERhdGUodHMpKX19dmFyIHRpbWVVbml0U2l6ZT17c2Vjb25kOjFlMyxtaW51dGU6NjAqMWUzLGhvdXI6NjAqNjAqMWUzLGRheToyNCo2MCo2MCoxZTMsbW9udGg6MzAqMjQqNjAqNjAqMWUzLHF1YXJ0ZXI6MyozMCoyNCo2MCo2MCoxZTMseWVhcjozNjUuMjQyNSoyNCo2MCo2MCoxZTN9O3ZhciBiYXNlU3BlYz1bWzEsXCJzZWNvbmRcIl0sWzIsXCJzZWNvbmRcIl0sWzUsXCJzZWNvbmRcIl0sWzEwLFwic2Vjb25kXCJdLFszMCxcInNlY29uZFwiXSxbMSxcIm1pbnV0ZVwiXSxbMixcIm1pbnV0ZVwiXSxbNSxcIm1pbnV0ZVwiXSxbMTAsXCJtaW51dGVcIl0sWzMwLFwibWludXRlXCJdLFsxLFwiaG91clwiXSxbMixcImhvdXJcIl0sWzQsXCJob3VyXCJdLFs4LFwiaG91clwiXSxbMTIsXCJob3VyXCJdLFsxLFwiZGF5XCJdLFsyLFwiZGF5XCJdLFszLFwiZGF5XCJdLFsuMjUsXCJtb250aFwiXSxbLjUsXCJtb250aFwiXSxbMSxcIm1vbnRoXCJdLFsyLFwibW9udGhcIl1dO3ZhciBzcGVjTW9udGhzPWJhc2VTcGVjLmNvbmNhdChbWzMsXCJtb250aFwiXSxbNixcIm1vbnRoXCJdLFsxLFwieWVhclwiXV0pO3ZhciBzcGVjUXVhcnRlcnM9YmFzZVNwZWMuY29uY2F0KFtbMSxcInF1YXJ0ZXJcIl0sWzIsXCJxdWFydGVyXCJdLFsxLFwieWVhclwiXV0pO2Z1bmN0aW9uIGluaXQocGxvdCl7cGxvdC5ob29rcy5wcm9jZXNzT3B0aW9ucy5wdXNoKGZ1bmN0aW9uKHBsb3Qsb3B0aW9ucyl7JC5lYWNoKHBsb3QuZ2V0QXhlcygpLGZ1bmN0aW9uKGF4aXNOYW1lLGF4aXMpe3ZhciBvcHRzPWF4aXMub3B0aW9ucztpZihvcHRzLm1vZGU9PVwidGltZVwiKXtheGlzLnRpY2tHZW5lcmF0b3I9ZnVuY3Rpb24oYXhpcyl7dmFyIHRpY2tzPVtdO3ZhciBkPWRhdGVHZW5lcmF0b3IoYXhpcy5taW4sb3B0cyk7dmFyIG1pblNpemU9MDt2YXIgc3BlYz1vcHRzLnRpY2tTaXplJiZvcHRzLnRpY2tTaXplWzFdPT09XCJxdWFydGVyXCJ8fG9wdHMubWluVGlja1NpemUmJm9wdHMubWluVGlja1NpemVbMV09PT1cInF1YXJ0ZXJcIj9zcGVjUXVhcnRlcnM6c3BlY01vbnRocztpZihvcHRzLm1pblRpY2tTaXplIT1udWxsKXtpZih0eXBlb2Ygb3B0cy50aWNrU2l6ZT09XCJudW1iZXJcIil7bWluU2l6ZT1vcHRzLnRpY2tTaXplfWVsc2V7bWluU2l6ZT1vcHRzLm1pblRpY2tTaXplWzBdKnRpbWVVbml0U2l6ZVtvcHRzLm1pblRpY2tTaXplWzFdXX19Zm9yKHZhciBpPTA7aTxzcGVjLmxlbmd0aC0xOysraSl7aWYoYXhpcy5kZWx0YTwoc3BlY1tpXVswXSp0aW1lVW5pdFNpemVbc3BlY1tpXVsxXV0rc3BlY1tpKzFdWzBdKnRpbWVVbml0U2l6ZVtzcGVjW2krMV1bMV1dKS8yJiZzcGVjW2ldWzBdKnRpbWVVbml0U2l6ZVtzcGVjW2ldWzFdXT49bWluU2l6ZSl7YnJlYWt9fXZhciBzaXplPXNwZWNbaV1bMF07dmFyIHVuaXQ9c3BlY1tpXVsxXTtpZih1bml0PT1cInllYXJcIil7aWYob3B0cy5taW5UaWNrU2l6ZSE9bnVsbCYmb3B0cy5taW5UaWNrU2l6ZVsxXT09XCJ5ZWFyXCIpe3NpemU9TWF0aC5mbG9vcihvcHRzLm1pblRpY2tTaXplWzBdKX1lbHNle3ZhciBtYWduPU1hdGgucG93KDEwLE1hdGguZmxvb3IoTWF0aC5sb2coYXhpcy5kZWx0YS90aW1lVW5pdFNpemUueWVhcikvTWF0aC5MTjEwKSk7dmFyIG5vcm09YXhpcy5kZWx0YS90aW1lVW5pdFNpemUueWVhci9tYWduO2lmKG5vcm08MS41KXtzaXplPTF9ZWxzZSBpZihub3JtPDMpe3NpemU9Mn1lbHNlIGlmKG5vcm08Ny41KXtzaXplPTV9ZWxzZXtzaXplPTEwfXNpemUqPW1hZ259aWYoc2l6ZTwxKXtzaXplPTF9fWF4aXMudGlja1NpemU9b3B0cy50aWNrU2l6ZXx8W3NpemUsdW5pdF07dmFyIHRpY2tTaXplPWF4aXMudGlja1NpemVbMF07dW5pdD1heGlzLnRpY2tTaXplWzFdO3ZhciBzdGVwPXRpY2tTaXplKnRpbWVVbml0U2l6ZVt1bml0XTtpZih1bml0PT1cInNlY29uZFwiKXtkLnNldFNlY29uZHMoZmxvb3JJbkJhc2UoZC5nZXRTZWNvbmRzKCksdGlja1NpemUpKX1lbHNlIGlmKHVuaXQ9PVwibWludXRlXCIpe2Quc2V0TWludXRlcyhmbG9vckluQmFzZShkLmdldE1pbnV0ZXMoKSx0aWNrU2l6ZSkpfWVsc2UgaWYodW5pdD09XCJob3VyXCIpe2Quc2V0SG91cnMoZmxvb3JJbkJhc2UoZC5nZXRIb3VycygpLHRpY2tTaXplKSl9ZWxzZSBpZih1bml0PT1cIm1vbnRoXCIpe2Quc2V0TW9udGgoZmxvb3JJbkJhc2UoZC5nZXRNb250aCgpLHRpY2tTaXplKSl9ZWxzZSBpZih1bml0PT1cInF1YXJ0ZXJcIil7ZC5zZXRNb250aCgzKmZsb29ySW5CYXNlKGQuZ2V0TW9udGgoKS8zLHRpY2tTaXplKSl9ZWxzZSBpZih1bml0PT1cInllYXJcIil7ZC5zZXRGdWxsWWVhcihmbG9vckluQmFzZShkLmdldEZ1bGxZZWFyKCksdGlja1NpemUpKX1kLnNldE1pbGxpc2Vjb25kcygwKTtpZihzdGVwPj10aW1lVW5pdFNpemUubWludXRlKXtkLnNldFNlY29uZHMoMCl9aWYoc3RlcD49dGltZVVuaXRTaXplLmhvdXIpe2Quc2V0TWludXRlcygwKX1pZihzdGVwPj10aW1lVW5pdFNpemUuZGF5KXtkLnNldEhvdXJzKDApfWlmKHN0ZXA+PXRpbWVVbml0U2l6ZS5kYXkqNCl7ZC5zZXREYXRlKDEpfWlmKHN0ZXA+PXRpbWVVbml0U2l6ZS5tb250aCoyKXtkLnNldE1vbnRoKGZsb29ySW5CYXNlKGQuZ2V0TW9udGgoKSwzKSl9aWYoc3RlcD49dGltZVVuaXRTaXplLnF1YXJ0ZXIqMil7ZC5zZXRNb250aChmbG9vckluQmFzZShkLmdldE1vbnRoKCksNikpfWlmKHN0ZXA+PXRpbWVVbml0U2l6ZS55ZWFyKXtkLnNldE1vbnRoKDApfXZhciBjYXJyeT0wO3ZhciB2PU51bWJlci5OYU47dmFyIHByZXY7ZG97cHJldj12O3Y9ZC5nZXRUaW1lKCk7dGlja3MucHVzaCh2KTtpZih1bml0PT1cIm1vbnRoXCJ8fHVuaXQ9PVwicXVhcnRlclwiKXtpZih0aWNrU2l6ZTwxKXtkLnNldERhdGUoMSk7dmFyIHN0YXJ0PWQuZ2V0VGltZSgpO2Quc2V0TW9udGgoZC5nZXRNb250aCgpKyh1bml0PT1cInF1YXJ0ZXJcIj8zOjEpKTt2YXIgZW5kPWQuZ2V0VGltZSgpO2Quc2V0VGltZSh2K2NhcnJ5KnRpbWVVbml0U2l6ZS5ob3VyKyhlbmQtc3RhcnQpKnRpY2tTaXplKTtjYXJyeT1kLmdldEhvdXJzKCk7ZC5zZXRIb3VycygwKX1lbHNle2Quc2V0TW9udGgoZC5nZXRNb250aCgpK3RpY2tTaXplKih1bml0PT1cInF1YXJ0ZXJcIj8zOjEpKX19ZWxzZSBpZih1bml0PT1cInllYXJcIil7ZC5zZXRGdWxsWWVhcihkLmdldEZ1bGxZZWFyKCkrdGlja1NpemUpfWVsc2V7ZC5zZXRUaW1lKHYrc3RlcCl9fXdoaWxlKHY8YXhpcy5tYXgmJnYhPXByZXYpO3JldHVybiB0aWNrc307YXhpcy50aWNrRm9ybWF0dGVyPWZ1bmN0aW9uKHYsYXhpcyl7dmFyIGQ9ZGF0ZUdlbmVyYXRvcih2LGF4aXMub3B0aW9ucyk7aWYob3B0cy50aW1lZm9ybWF0IT1udWxsKXtyZXR1cm4gZm9ybWF0RGF0ZShkLG9wdHMudGltZWZvcm1hdCxvcHRzLm1vbnRoTmFtZXMsb3B0cy5kYXlOYW1lcyl9dmFyIHVzZVF1YXJ0ZXJzPWF4aXMub3B0aW9ucy50aWNrU2l6ZSYmYXhpcy5vcHRpb25zLnRpY2tTaXplWzFdPT1cInF1YXJ0ZXJcInx8YXhpcy5vcHRpb25zLm1pblRpY2tTaXplJiZheGlzLm9wdGlvbnMubWluVGlja1NpemVbMV09PVwicXVhcnRlclwiO3ZhciB0PWF4aXMudGlja1NpemVbMF0qdGltZVVuaXRTaXplW2F4aXMudGlja1NpemVbMV1dO3ZhciBzcGFuPWF4aXMubWF4LWF4aXMubWluO3ZhciBzdWZmaXg9b3B0cy50d2VsdmVIb3VyQ2xvY2s/XCIgJXBcIjpcIlwiO3ZhciBob3VyQ29kZT1vcHRzLnR3ZWx2ZUhvdXJDbG9jaz9cIiVJXCI6XCIlSFwiO3ZhciBmbXQ7aWYodDx0aW1lVW5pdFNpemUubWludXRlKXtmbXQ9aG91ckNvZGUrXCI6JU06JVNcIitzdWZmaXh9ZWxzZSBpZih0PHRpbWVVbml0U2l6ZS5kYXkpe2lmKHNwYW48Mip0aW1lVW5pdFNpemUuZGF5KXtmbXQ9aG91ckNvZGUrXCI6JU1cIitzdWZmaXh9ZWxzZXtmbXQ9XCIlYiAlZCBcIitob3VyQ29kZStcIjolTVwiK3N1ZmZpeH19ZWxzZSBpZih0PHRpbWVVbml0U2l6ZS5tb250aCl7Zm10PVwiJWIgJWRcIn1lbHNlIGlmKHVzZVF1YXJ0ZXJzJiZ0PHRpbWVVbml0U2l6ZS5xdWFydGVyfHwhdXNlUXVhcnRlcnMmJnQ8dGltZVVuaXRTaXplLnllYXIpe2lmKHNwYW48dGltZVVuaXRTaXplLnllYXIpe2ZtdD1cIiViXCJ9ZWxzZXtmbXQ9XCIlYiAlWVwifX1lbHNlIGlmKHVzZVF1YXJ0ZXJzJiZ0PHRpbWVVbml0U2l6ZS55ZWFyKXtpZihzcGFuPHRpbWVVbml0U2l6ZS55ZWFyKXtmbXQ9XCJRJXFcIn1lbHNle2ZtdD1cIlElcSAlWVwifX1lbHNle2ZtdD1cIiVZXCJ9dmFyIHJ0PWZvcm1hdERhdGUoZCxmbXQsb3B0cy5tb250aE5hbWVzLG9wdHMuZGF5TmFtZXMpO3JldHVybiBydH19fSl9KX0kLnBsb3QucGx1Z2lucy5wdXNoKHtpbml0OmluaXQsb3B0aW9uczpvcHRpb25zLG5hbWU6XCJ0aW1lXCIsdmVyc2lvbjpcIjEuMFwifSk7JC5wbG90LmZvcm1hdERhdGU9Zm9ybWF0RGF0ZTskLnBsb3QuZGF0ZUdlbmVyYXRvcj1kYXRlR2VuZXJhdG9yfSkoalF1ZXJ5KTsiLCJtb2R1bGUuZXhwb3J0cyA9IG1vbWVudDsiLCJtb2R1bGUuZXhwb3J0cyA9IFJlYWN0OyIsIm1vZHVsZS5leHBvcnRzID0gUmVhY3RET007Il0sInNvdXJjZVJvb3QiOiIifQ==