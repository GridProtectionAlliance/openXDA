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
/*!*****************************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/CreateGuid.js ***!
  \*****************************************************************************************************************************/
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
/*!******************************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/GetNodeSize.js ***!
  \******************************************************************************************************************************/
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
/*!********************************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/GetTextHeight.js ***!
  \********************************************************************************************************************************/
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
/*!*******************************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/GetTextWidth.js ***!
  \*******************************************************************************************************************************/
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
/*!****************************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/IsInteger.js ***!
  \****************************************************************************************************************************/
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
/*!***************************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/IsNumber.js ***!
  \***************************************************************************************************************************/
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
/*!******************************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/RandomColor.js ***!
  \******************************************************************************************************************************/
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
/*!************************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/@gpa-gemstone/helper-functions/lib/index.js ***!
  \************************************************************************************************************************/
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
/*!**********************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/create-react-class/factory.js ***!
  \**********************************************************************************************************/
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
/*!********************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/create-react-class/index.js ***!
  \********************************************************************************************************/
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
/*!**************************************************************************************************************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/css-loader/dist/cjs.js!C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/css/react-datetime.css ***!
  \**************************************************************************************************************************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

exports = module.exports = __webpack_require__(/*! ../../css-loader/dist/runtime/api.js */ "../../node_modules/css-loader/dist/runtime/api.js")(false);
// Module
exports.push([module.i, "/*!\n * https://github.com/YouCanBookMe/react-datetime\n */\n\n.rdt {\n  position: relative;\n}\n.rdtPicker {\n  display: none;\n  position: absolute;\n  width: 250px;\n  padding: 4px;\n  margin-top: 1px;\n  z-index: 99999 !important;\n  background: #fff;\n  box-shadow: 0 1px 3px rgba(0,0,0,.1);\n  border: 1px solid #f9f9f9;\n}\n.rdtOpen .rdtPicker {\n  display: block;\n}\n.rdtStatic .rdtPicker {\n  box-shadow: none;\n  position: static;\n}\n\n.rdtPicker .rdtTimeToggle {\n  text-align: center;\n}\n\n.rdtPicker table {\n  width: 100%;\n  margin: 0;\n}\n.rdtPicker td,\n.rdtPicker th {\n  text-align: center;\n  height: 28px;\n}\n.rdtPicker td {\n  cursor: pointer;\n}\n.rdtPicker td.rdtDay:hover,\n.rdtPicker td.rdtHour:hover,\n.rdtPicker td.rdtMinute:hover,\n.rdtPicker td.rdtSecond:hover,\n.rdtPicker .rdtTimeToggle:hover {\n  background: #eeeeee;\n  cursor: pointer;\n}\n.rdtPicker td.rdtOld,\n.rdtPicker td.rdtNew {\n  color: #999999;\n}\n.rdtPicker td.rdtToday {\n  position: relative;\n}\n.rdtPicker td.rdtToday:before {\n  content: '';\n  display: inline-block;\n  border-left: 7px solid transparent;\n  border-bottom: 7px solid #428bca;\n  border-top-color: rgba(0, 0, 0, 0.2);\n  position: absolute;\n  bottom: 4px;\n  right: 4px;\n}\n.rdtPicker td.rdtActive,\n.rdtPicker td.rdtActive:hover {\n  background-color: #428bca;\n  color: #fff;\n  text-shadow: 0 -1px 0 rgba(0, 0, 0, 0.25);\n}\n.rdtPicker td.rdtActive.rdtToday:before {\n  border-bottom-color: #fff;\n}\n.rdtPicker td.rdtDisabled,\n.rdtPicker td.rdtDisabled:hover {\n  background: none;\n  color: #999999;\n  cursor: not-allowed;\n}\n\n.rdtPicker td span.rdtOld {\n  color: #999999;\n}\n.rdtPicker td span.rdtDisabled,\n.rdtPicker td span.rdtDisabled:hover {\n  background: none;\n  color: #999999;\n  cursor: not-allowed;\n}\n.rdtPicker th {\n  border-bottom: 1px solid #f9f9f9;\n}\n.rdtPicker .dow {\n  width: 14.2857%;\n  border-bottom: none;\n  cursor: default;\n}\n.rdtPicker th.rdtSwitch {\n  width: 100px;\n}\n.rdtPicker th.rdtNext,\n.rdtPicker th.rdtPrev {\n  font-size: 21px;\n  vertical-align: top;\n}\n\n.rdtPrev span,\n.rdtNext span {\n  display: block;\n  -webkit-touch-callout: none; /* iOS Safari */\n  -webkit-user-select: none;   /* Chrome/Safari/Opera */\n  -khtml-user-select: none;    /* Konqueror */\n  -moz-user-select: none;      /* Firefox */\n  -ms-user-select: none;       /* Internet Explorer/Edge */\n  user-select: none;\n}\n\n.rdtPicker th.rdtDisabled,\n.rdtPicker th.rdtDisabled:hover {\n  background: none;\n  color: #999999;\n  cursor: not-allowed;\n}\n.rdtPicker thead tr:first-child th {\n  cursor: pointer;\n}\n.rdtPicker thead tr:first-child th:hover {\n  background: #eeeeee;\n}\n\n.rdtPicker tfoot {\n  border-top: 1px solid #f9f9f9;\n}\n\n.rdtPicker button {\n  border: none;\n  background: none;\n  cursor: pointer;\n}\n.rdtPicker button:hover {\n  background-color: #eee;\n}\n\n.rdtPicker thead button {\n  width: 100%;\n  height: 100%;\n}\n\ntd.rdtMonth,\ntd.rdtYear {\n  height: 50px;\n  width: 25%;\n  cursor: pointer;\n}\ntd.rdtMonth:hover,\ntd.rdtYear:hover {\n  background: #eee;\n}\n\n.rdtCounters {\n  display: inline-block;\n}\n\n.rdtCounters > div {\n  float: left;\n}\n\n.rdtCounter {\n  height: 100px;\n}\n\n.rdtCounter {\n  width: 40px;\n}\n\n.rdtCounterSeparator {\n  line-height: 100px;\n}\n\n.rdtCounter .rdtBtn {\n  height: 40%;\n  line-height: 40px;\n  cursor: pointer;\n  display: block;\n\n  -webkit-touch-callout: none; /* iOS Safari */\n  -webkit-user-select: none;   /* Chrome/Safari/Opera */\n  -khtml-user-select: none;    /* Konqueror */\n  -moz-user-select: none;      /* Firefox */\n  -ms-user-select: none;       /* Internet Explorer/Edge */\n  user-select: none;\n}\n.rdtCounter .rdtBtn:hover {\n  background: #eee;\n}\n.rdtCounter .rdtCount {\n  height: 20%;\n  font-size: 1.2em;\n}\n\n.rdtMilli {\n  vertical-align: middle;\n  padding-left: 8px;\n  width: 48px;\n}\n\n.rdtMilli input {\n  width: 100%;\n  font-size: 1.2em;\n  margin-top: 37px;\n}\n\n.rdtTime td {\n  cursor: default;\n}\n", ""]);


/***/ }),

/***/ "../../node_modules/css-loader/dist/runtime/api.js":
/*!***********************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/css-loader/dist/runtime/api.js ***!
  \***********************************************************************************************************/
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
/*!**********************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/decode-uri-component/index.js ***!
  \**********************************************************************************************************/
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
/*!******************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/fbjs/lib/emptyFunction.js ***!
  \******************************************************************************************************/
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
/*!**************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/fbjs/lib/invariant.js ***!
  \**************************************************************************************************/
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
/*!************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/fbjs/lib/warning.js ***!
  \************************************************************************************************/
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
/*!************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/history/DOMUtils.js ***!
  \************************************************************************************************/
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
/*!*****************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/history/LocationUtils.js ***!
  \*****************************************************************************************************/
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
/*!*************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/history/PathUtils.js ***!
  \*************************************************************************************************/
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
/*!************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/history/createBrowserHistory.js ***!
  \************************************************************************************************************/
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
/*!***************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/history/createTransitionManager.js ***!
  \***************************************************************************************************************/
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
/*!*************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/invariant/browser.js ***!
  \*************************************************************************************************/
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
/*!***************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/object-assign/index.js ***!
  \***************************************************************************************************/
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
/*!*********************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/prop-types/checkPropTypes.js ***!
  \*********************************************************************************************************/
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
/*!******************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/prop-types/factoryWithTypeCheckers.js ***!
  \******************************************************************************************************************/
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
/*!************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/prop-types/index.js ***!
  \************************************************************************************************/
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
/*!*******************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/prop-types/lib/ReactPropTypesSecret.js ***!
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



var ReactPropTypesSecret = 'SECRET_DO_NOT_PASS_THIS_OR_YOU_WILL_BE_FIRED';

module.exports = ReactPropTypesSecret;


/***/ }),

/***/ "../../node_modules/query-string/index.js":
/*!**************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/query-string/index.js ***!
  \**************************************************************************************************/
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
/*!*******************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/DateTime.js ***!
  \*******************************************************************************************************/
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
/*!******************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/css/react-datetime.css ***!
  \******************************************************************************************************************/
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
/*!*******************************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/node_modules/object-assign/index.js ***!
  \*******************************************************************************************************************************/
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
/*!********************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/src/CalendarContainer.js ***!
  \********************************************************************************************************************/
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
/*!***********************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/src/DaysView.js ***!
  \***********************************************************************************************************/
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
/*!*************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/src/MonthsView.js ***!
  \*************************************************************************************************************/
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
/*!***********************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/src/TimeView.js ***!
  \***********************************************************************************************************/
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
/*!************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-datetime/src/YearsView.js ***!
  \************************************************************************************************************/
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
/*!*********************************************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/react-onclickoutside/dist/react-onclickoutside.es.js ***!
  \*********************************************************************************************************************************/
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
/*!******************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/resolve-pathname/index.js ***!
  \******************************************************************************************************/
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
/*!*******************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/strict-uri-encode/index.js ***!
  \*******************************************************************************************************/
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
/*!**********************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/style-loader/lib/addStyles.js ***!
  \**********************************************************************************************************/
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
/*!*****************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/style-loader/lib/urls.js ***!
  \*****************************************************************************************************/
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
/*!*************************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/value-equal/index.js ***!
  \*************************************************************************************************/
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
/*!***********************************************************************************************!*\
  !*** C:/Projects/openXDA/Source/Applications/openXDA/openXDA/node_modules/warning/browser.js ***!
  \***********************************************************************************************/
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
//# sourceMappingURL=data:application/json;charset=utf-8;base64,eyJ2ZXJzaW9uIjozLCJzb3VyY2VzIjpbIndlYnBhY2s6Ly8vd2VicGFjay9ib290c3RyYXAiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL0BncGEtZ2Vtc3RvbmUvaGVscGVyLWZ1bmN0aW9ucy9saWIvQ3JlYXRlR3VpZC5qcyIsIndlYnBhY2s6Ly8vQzovUHJvamVjdHMvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvQGdwYS1nZW1zdG9uZS9oZWxwZXItZnVuY3Rpb25zL2xpYi9HZXROb2RlU2l6ZS5qcyIsIndlYnBhY2s6Ly8vQzovUHJvamVjdHMvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvQGdwYS1nZW1zdG9uZS9oZWxwZXItZnVuY3Rpb25zL2xpYi9HZXRUZXh0SGVpZ2h0LmpzIiwid2VicGFjazovLy9DOi9Qcm9qZWN0cy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9AZ3BhLWdlbXN0b25lL2hlbHBlci1mdW5jdGlvbnMvbGliL0dldFRleHRXaWR0aC5qcyIsIndlYnBhY2s6Ly8vQzovUHJvamVjdHMvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvQGdwYS1nZW1zdG9uZS9oZWxwZXItZnVuY3Rpb25zL2xpYi9Jc0ludGVnZXIuanMiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL0BncGEtZ2Vtc3RvbmUvaGVscGVyLWZ1bmN0aW9ucy9saWIvSXNOdW1iZXIuanMiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL0BncGEtZ2Vtc3RvbmUvaGVscGVyLWZ1bmN0aW9ucy9saWIvUmFuZG9tQ29sb3IuanMiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL0BncGEtZ2Vtc3RvbmUvaGVscGVyLWZ1bmN0aW9ucy9saWIvaW5kZXguanMiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2NyZWF0ZS1yZWFjdC1jbGFzcy9mYWN0b3J5LmpzIiwid2VicGFjazovLy9DOi9Qcm9qZWN0cy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9jcmVhdGUtcmVhY3QtY2xhc3MvaW5kZXguanMiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3JlYWN0LWRhdGV0aW1lL2Nzcy9yZWFjdC1kYXRldGltZS5jc3MiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2Nzcy1sb2FkZXIvZGlzdC9ydW50aW1lL2FwaS5qcyIsIndlYnBhY2s6Ly8vQzovUHJvamVjdHMvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvZGVjb2RlLXVyaS1jb21wb25lbnQvaW5kZXguanMiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2ZianMvbGliL2VtcHR5RnVuY3Rpb24uanMiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL2ZianMvbGliL2ludmFyaWFudC5qcyIsIndlYnBhY2s6Ly8vQzovUHJvamVjdHMvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvZmJqcy9saWIvd2FybmluZy5qcyIsIndlYnBhY2s6Ly8vQzovUHJvamVjdHMvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvaGlzdG9yeS9ET01VdGlscy5qcyIsIndlYnBhY2s6Ly8vQzovUHJvamVjdHMvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvaGlzdG9yeS9Mb2NhdGlvblV0aWxzLmpzIiwid2VicGFjazovLy9DOi9Qcm9qZWN0cy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9oaXN0b3J5L1BhdGhVdGlscy5qcyIsIndlYnBhY2s6Ly8vQzovUHJvamVjdHMvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvaGlzdG9yeS9jcmVhdGVCcm93c2VySGlzdG9yeS5qcyIsIndlYnBhY2s6Ly8vQzovUHJvamVjdHMvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvaGlzdG9yeS9jcmVhdGVUcmFuc2l0aW9uTWFuYWdlci5qcyIsIndlYnBhY2s6Ly8vQzovUHJvamVjdHMvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvaW52YXJpYW50L2Jyb3dzZXIuanMiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL29iamVjdC1hc3NpZ24vaW5kZXguanMiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3Byb3AtdHlwZXMvY2hlY2tQcm9wVHlwZXMuanMiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3Byb3AtdHlwZXMvZmFjdG9yeVdpdGhUeXBlQ2hlY2tlcnMuanMiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3Byb3AtdHlwZXMvaW5kZXguanMiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3Byb3AtdHlwZXMvbGliL1JlYWN0UHJvcFR5cGVzU2VjcmV0LmpzIiwid2VicGFjazovLy9DOi9Qcm9qZWN0cy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9xdWVyeS1zdHJpbmcvaW5kZXguanMiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3JlYWN0LWRhdGV0aW1lL0RhdGVUaW1lLmpzIiwid2VicGFjazovLy9DOi9Qcm9qZWN0cy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9yZWFjdC1kYXRldGltZS9jc3MvcmVhY3QtZGF0ZXRpbWUuY3NzP2ZhYjEiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3JlYWN0LWRhdGV0aW1lL25vZGVfbW9kdWxlcy9vYmplY3QtYXNzaWduL2luZGV4LmpzIiwid2VicGFjazovLy9DOi9Qcm9qZWN0cy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9yZWFjdC1kYXRldGltZS9zcmMvQ2FsZW5kYXJDb250YWluZXIuanMiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3JlYWN0LWRhdGV0aW1lL3NyYy9EYXlzVmlldy5qcyIsIndlYnBhY2s6Ly8vQzovUHJvamVjdHMvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvcmVhY3QtZGF0ZXRpbWUvc3JjL01vbnRoc1ZpZXcuanMiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3JlYWN0LWRhdGV0aW1lL3NyYy9UaW1lVmlldy5qcyIsIndlYnBhY2s6Ly8vQzovUHJvamVjdHMvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvcmVhY3QtZGF0ZXRpbWUvc3JjL1llYXJzVmlldy5qcyIsIndlYnBhY2s6Ly8vQzovUHJvamVjdHMvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvcmVhY3Qtb25jbGlja291dHNpZGUvZGlzdC9yZWFjdC1vbmNsaWNrb3V0c2lkZS5lcy5qcyIsIndlYnBhY2s6Ly8vQzovUHJvamVjdHMvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvcmVzb2x2ZS1wYXRobmFtZS9pbmRleC5qcyIsIndlYnBhY2s6Ly8vQzovUHJvamVjdHMvb3BlblhEQS9Tb3VyY2UvQXBwbGljYXRpb25zL29wZW5YREEvb3BlblhEQS9ub2RlX21vZHVsZXMvc3RyaWN0LXVyaS1lbmNvZGUvaW5kZXguanMiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3N0eWxlLWxvYWRlci9saWIvYWRkU3R5bGVzLmpzIiwid2VicGFjazovLy9DOi9Qcm9qZWN0cy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy9zdHlsZS1sb2FkZXIvbGliL3VybHMuanMiLCJ3ZWJwYWNrOi8vL0M6L1Byb2plY3RzL29wZW5YREEvU291cmNlL0FwcGxpY2F0aW9ucy9vcGVuWERBL29wZW5YREEvbm9kZV9tb2R1bGVzL3ZhbHVlLWVxdWFsL2luZGV4LmpzIiwid2VicGFjazovLy9DOi9Qcm9qZWN0cy9vcGVuWERBL1NvdXJjZS9BcHBsaWNhdGlvbnMvb3BlblhEQS9vcGVuWERBL25vZGVfbW9kdWxlcy93YXJuaW5nL2Jyb3dzZXIuanMiLCJ3ZWJwYWNrOi8vLy4vVFMvU2VydmljZXMvUGVyaW9kaWNEYXRhRGlzcGxheS50cyIsIndlYnBhY2s6Ly8vLi9UUy9TZXJ2aWNlcy9UcmVuZGluZ0RhdGFEaXNwbGF5LnRzIiwid2VicGFjazovLy8uL1RTWC9EYXRlVGltZVJhbmdlUGlja2VyLnRzeCIsIndlYnBhY2s6Ly8vLi9UU1gvTWVhc3VyZW1lbnRJbnB1dC50c3giLCJ3ZWJwYWNrOi8vLy4vVFNYL01ldGVySW5wdXQudHN4Iiwid2VicGFjazovLy8uL1RTWC9UcmVuZGluZ0NoYXJ0LnRzeCIsIndlYnBhY2s6Ly8vLi9UU1gvVHJlbmRpbmdEYXRhRGlzcGxheS50c3giLCJ3ZWJwYWNrOi8vLy4vZmxvdC9qcXVlcnkuZmxvdC5heGlzbGFiZWxzLmpzIiwid2VicGFjazovLy8uL2Zsb3QvanF1ZXJ5LmZsb3QuY3Jvc3NoYWlyLm1pbi5qcyIsIndlYnBhY2s6Ly8vLi9mbG90L2pxdWVyeS5mbG90Lm1pbi5qcyIsIndlYnBhY2s6Ly8vLi9mbG90L2pxdWVyeS5mbG90Lm5hdmlnYXRlLm1pbi5qcyIsIndlYnBhY2s6Ly8vLi9mbG90L2pxdWVyeS5mbG90LnNlbGVjdGlvbi5taW4uanMiLCJ3ZWJwYWNrOi8vLy4vZmxvdC9qcXVlcnkuZmxvdC50aW1lLm1pbi5qcyIsIndlYnBhY2s6Ly8vZXh0ZXJuYWwgXCJtb21lbnRcIiIsIndlYnBhY2s6Ly8vZXh0ZXJuYWwgXCJSZWFjdFwiIiwid2VicGFjazovLy9leHRlcm5hbCBcIlJlYWN0RE9NXCIiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6IjtRQUFBO1FBQ0E7O1FBRUE7UUFDQTs7UUFFQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTs7UUFFQTtRQUNBOztRQUVBO1FBQ0E7O1FBRUE7UUFDQTtRQUNBOzs7UUFHQTtRQUNBOztRQUVBO1FBQ0E7O1FBRUE7UUFDQTtRQUNBO1FBQ0EsMENBQTBDLGdDQUFnQztRQUMxRTtRQUNBOztRQUVBO1FBQ0E7UUFDQTtRQUNBLHdEQUF3RCxrQkFBa0I7UUFDMUU7UUFDQSxpREFBaUQsY0FBYztRQUMvRDs7UUFFQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0E7UUFDQTtRQUNBO1FBQ0EseUNBQXlDLGlDQUFpQztRQUMxRSxnSEFBZ0gsbUJBQW1CLEVBQUU7UUFDckk7UUFDQTs7UUFFQTtRQUNBO1FBQ0E7UUFDQSwyQkFBMkIsMEJBQTBCLEVBQUU7UUFDdkQsaUNBQWlDLGVBQWU7UUFDaEQ7UUFDQTtRQUNBOztRQUVBO1FBQ0Esc0RBQXNELCtEQUErRDs7UUFFckg7UUFDQTs7O1FBR0E7UUFDQTs7Ozs7Ozs7Ozs7OztBQ2xGYTtBQUNiO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsaUZBQWlGO0FBQ2pGO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsOENBQThDLGNBQWM7QUFDNUQ7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsS0FBSztBQUNMO0FBQ0E7Ozs7Ozs7Ozs7Ozs7QUNyQ2E7QUFDYjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLGlGQUFpRjtBQUNqRjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsOENBQThDLGNBQWM7QUFDNUQ7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7Ozs7Ozs7Ozs7Ozs7QUM3Q2E7QUFDYjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLGlGQUFpRjtBQUNqRjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsOENBQThDLGNBQWM7QUFDNUQ7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7Ozs7Ozs7Ozs7Ozs7QUM3Q2E7QUFDYjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLGlGQUFpRjtBQUNqRjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsOENBQThDLGNBQWM7QUFDNUQ7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNB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QTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBLElBQUksSUFBcUM7QUFDekM7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLENBQUMsTUFBTSxFQUVOOztBQUVEO0FBQ0E7QUFDQTtBQUNBOztBQUVBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLFFBQVE7QUFDUjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLGNBQWM7QUFDZDtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQSxjQUFjO0FBQ2Q7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBLGNBQWM7QUFDZDtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0EsY0FBYztBQUNkO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQSxjQUFjO0FBQ2Q7QUFDQTtBQUNBOztBQUVBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsZ0JBQWdCO0FBQ2hCO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsZ0JBQWdCO0FBQ2hCO0FBQ0E7QUFDQTs7QUFFQTtBQUNBLGdCQUFnQjtBQUNoQjtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsK0JBQStCLEtBQUs7QUFDcEM7QUFDQTtBQUNBLGdCQUFnQjtBQUNoQjtBQUNBO0FBQ0E7O0FBRUE7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLGVBQWUsV0FBVztBQUMxQjtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsWUFB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O0FBQzdDO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxTQUFTLFVBQVUsS0FBcUM7QUFDeEQ7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsT0FBTztBQUNQO0FBQ0E7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxxQkFBcUIsc0JBQXNCO0FBQzNDO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBLE1BQU0sS0FBcUMsMEZBQTBGLFNBQU07QUFDM0k7QUFDQTs7QUFFQTtBQUNBO0FBQ0EscUJBQXFCLDJCQUEyQjtBQUNoRDtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQSxNQUFNLEtBQXFDLDhGQUE4RixTQUFNO0FBQy9JO0FBQ0E7O0FBRUEsbUJBQW1CLGdDQUFnQztBQUNuRDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQSxxQkFBcUIsZ0NBQWdDO0FBQ3JEO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSw2QkFBNkI7QUFDN0I7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxXQUFXO0FBQ1g7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7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QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLEVBQUU7O0FBRUY7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7QUFDQSxFQUFFOztBQUVGO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBLEVBQUU7O0FBRUY7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTs7QUFFQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBLEdBQUc7QUFDSDtBQUNBLEdBQUc7QUFDSDtBQUNBOztBQUVBO0FBQ0EsRUFBRTs7QUFFRjtBQUNBO0FBQ0E7QUFDQTtBQUNBLElBQUk7QUFDSjtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQSxFQUFFOztBQUVGO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBLElBQUk7QUFDSjtBQUNBLElBQUk7QUFDSjtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLElBQUk7QUFDSjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLEdBQUc7QUFDSDtBQUNBLEVBQUU7O0FBRUY7QUFDQTtBQUNBO0FBQ0EsYUFBYTtBQUNiOztBQUVBO0FBQ0E7QUFDQTtBQUNB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O0FBQ0E7QUFDQTs7QUFFQSxhQUFhLHVCQUF1QjtBQUNwQztBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0EsQ0FBQztBQUNEO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxHQUFHO0FBQ0g7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7O0FBR0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQSxHQUFHO0FBQ0g7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7O0FBR0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBLENBQUM7QUFDRDtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0Esd0NBQXdDO0FBQ3hDO0FBQ0E7QUFDQTtBQUNBLEdBQUc7O0FBRUg7O0FBRUE7QUFDQTtBQUNBO0FBQ0EsRUFBRTtBQUNGO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQSxvQkFBb0I7QUFDcEI7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7OztBQUdBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7O0FBRUE7QUFDQTtBQUNBOztBQUVBO0FBQ0E7O0FBRUE7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUEsZUFBZSw2REFBVztBQUMxQjs7QUFFQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7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QUksQ0FBQztZQUV2QixDQUFDLGtCQUF1QixDQUV2QixDQUVaLENBQUM7QUFDRixDQUFDO0FBRUQsUUFBUSxDQUFDLE1BQU0sQ0FBQyxvQkFBQyxtQkFBbUIsT0FBRyxFQUFFLFFBQVEsQ0FBQyxjQUFjLENBQUMsZUFBZSxDQUFDLENBQUMsQ0FBQzs7Ozs7Ozs7Ozs7O0FDdGJuRjtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7O0FBR0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7OztBQUdBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLFNBQVM7QUFDVDtBQUNBO0FBQ0EsU0FBUztBQUNUO0FBQ0E7QUFDQTtBQUNBLFNBQVM7QUFDVDtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7OztBQUdBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOztBQUVBO0FBQ0EsdUVBQXVFO0FBQ3ZFO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQTtBQUNBOztBQUVBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0EsNEVBQTRFO0FBQzVFO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLFNBQVM7QUFDVDtBQUNBO0FBQ0E7QUFDQTtBQUNBLFNBQVM7QUFDVDtBQUNBO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBOzs7QUFHQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLFNBQVM7QUFDVDtBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSx3QkFBd0IsU0FBUztBQUNqQztBQUNBO0FBQ0Esd0RBQXdEO0FBQ3hEO0FBQ0E7QUFDQSxlQUFlO0FBQ2Y7QUFDQTs7QUFFQTtBQUNBLHVCQUF1QjtBQUN2QjtBQUNBO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQTtBQUNBLFNBQVM7QUFDVDtBQUNBO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7O0FBRUE7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBLHVEQUF1RDtBQUN2RDtBQUNBO0FBQ0E7QUFDQTs7O0FBR0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQSxtR0FBbUc7QUFDbkc7QUFDQTtBQUNBO0FBQ0E7QUFDQSxvQ0FBb0M7QUFDcEM7QUFDQTtBQUNBLG1DQUFtQztBQUNuQztBQUNBO0FBQ0E7O0FBRUE7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0EsU0FBUztBQUNUO0FBQ0E7QUFDQSxTQUFTO0FBQ1Q7QUFDQTtBQUNBO0FBQ0E7QUFDQTs7QUFFQTtBQUNBO0FBQ0E7QUFDQTtBQUNBO0FBQ0E7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ZHVsZSBjYWNoZVxuIFx0dmFyIGluc3RhbGxlZE1vZHVsZXMgPSB7fTtcblxuIFx0Ly8gVGhlIHJlcXVpcmUgZnVuY3Rpb25cbiBcdGZ1bmN0aW9uIF9fd2VicGFja19yZXF1aXJlX18obW9kdWxlSWQpIHtcblxuIFx0XHQvLyBDaGVjayBpZiBtb2R1bGUgaXMgaW4gY2FjaGVcbiBcdFx0aWYoaW5zdGFsbGVkTW9kdWxlc1ttb2R1bGVJZF0pIHtcbiBcdFx0XHRyZXR1cm4gaW5zdGFsbGVkTW9kdWxlc1ttb2R1bGVJZF0uZXhwb3J0cztcbiBcdFx0fVxuIFx0XHQvLyBDcmVhdGUgYSBuZXcgbW9kdWxlIChhbmQgcHV0IGl0IGludG8gdGhlIGNhY2hlKVxuIFx0XHR2YXIgbW9kdWxlID0gaW5zdGFsbGVkTW9kdWxlc1ttb2R1bGVJZF0gPSB7XG4gXHRcdFx0aTogbW9kdWxlSWQsXG4gXHRcdFx0bDogZmFsc2UsXG4gXHRcdFx0ZXhwb3J0czoge31cbiBcdFx0fTtcblxuIFx0XHQvLyBFeGVjdXRlIHRoZSBtb2R1bGUgZnVuY3Rpb25cbiBcdFx0bW9kdWxlc1ttb2R1bGVJZF0uY2FsbChtb2R1bGUuZXhwb3J0cywgbW9kdWxlLCBtb2R1bGUuZXhwb3J0cywgX193ZWJwYWNrX3JlcXVpcmVfXyk7XG5cbiBcdFx0Ly8gRmxhZyB0aGUgbW9kdWxlIGFzIGxvYWRlZFxuIFx0XHRtb2R1bGUubCA9IHRydWU7XG5cbiBcdFx0Ly8gUmV0dXJuIHRoZSBleHBvcnRzIG9mIHRoZSBtb2R1bGVcbiBcdFx0cmV0dXJuIG1vZHVsZS5leHBvcnRzO1xuIFx0fVxuXG5cbiBcdC8vIGV4cG9zZSB0aGUgbW9kdWxlcyBvYmplY3QgKF9fd2VicGFja19tb2R1bGVzX18pXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLm0gPSBtb2R1bGVzO1xuXG4gXHQvLyBleHBvc2UgdGhlIG1vZHVsZSBjYWNoZVxuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5jID0gaW5zdGFsbGVkTW9kdWxlcztcblxuIFx0Ly8gZGVmaW5lIGdldHRlciBmdW5jdGlvbiBmb3IgaGFybW9ueSBleHBvcnRzXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLmQgPSBmdW5jdGlvbihleHBvcnRzLCBuYW1lLCBnZXR0ZXIpIHtcbiBcdFx0aWYoIV9fd2VicGFja19yZXF1aXJlX18ubyhleHBvcnRzLCBuYW1lKSkge1xuIFx0XHRcdE9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBuYW1lLCB7IGVudW1lcmFibGU6IHRydWUsIGdldDogZ2V0dGVyIH0pO1xuIFx0XHR9XG4gXHR9O1xuXG4gXHQvLyBkZWZpbmUgX19lc01vZHVsZSBvbiBleHBvcnRzXG4gXHRfX3dlYnBhY2tfcmVxdWlyZV9fLnIgPSBmdW5jdGlvbihleHBvcnRzKSB7XG4gXHRcdGlmKHR5cGVvZiBTeW1ib2wgIT09ICd1bmRlZmluZWQnICYmIFN5bWJvbC50b1N0cmluZ1RhZykge1xuIFx0XHRcdE9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBTeW1ib2wudG9TdHJpbmdUYWcsIHsgdmFsdWU6ICdNb2R1bGUnIH0pO1xuIFx0XHR9XG4gXHRcdE9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCAnX19lc01vZHVsZScsIHsgdmFsdWU6IHRydWUgfSk7XG4gXHR9O1xuXG4gXHQvLyBjcmVhdGUgYSBmYWtlIG5hbWVzcGFjZSBvYmplY3RcbiBcdC8vIG1vZGUgJiAxOiB2YWx1ZSBpcyBhIG1vZHVsZSBpZCwgcmVxdWlyZSBpdFxuIFx0Ly8gbW9kZSAmIDI6IG1lcmdlIGFsbCBwcm9wZXJ0aWVzIG9mIHZhbHVlIGludG8gdGhlIG5zXG4gXHQvLyBtb2RlICYgNDogcmV0dXJuIHZhbHVlIHdoZW4gYWxyZWFkeSBucyBvYmplY3RcbiBcdC8vIG1vZGUgJiA4fDE6IGJlaGF2ZSBsaWtlIHJlcXVpcmVcbiBcdF9fd2VicGFja19yZXF1aXJlX18udCA9IGZ1bmN0aW9uKHZhbHVlLCBtb2RlKSB7XG4gXHRcdGlmKG1vZGUgJiAxKSB2YWx1ZSA9IF9fd2VicGFja19yZXF1aXJlX18odmFsdWUpO1xuIFx0XHRpZihtb2RlICYgOCkgcmV0dXJuIHZhbHVlO1xuIFx0XHRpZigobW9kZSAmIDQpICYmIHR5cGVvZiB2YWx1ZSA9PT0gJ29iamVjdCcgJiYgdmFsdWUgJiYgdmFsdWUuX19lc01vZHVsZSkgcmV0dXJuIHZhbHVlO1xuIFx0XHR2YXIgbnMgPSBPYmplY3QuY3JlYXRlKG51bGwpO1xuIFx0XHRfX3dlYnBhY2tfcmVxdWlyZV9fLnIobnMpO1xuIFx0XHRPYmplY3QuZGVmaW5lUHJvcGVydHkobnMsICdkZWZhdWx0JywgeyBlbnVtZXJhYmxlOiB0cnVlLCB2YWx1ZTogdmFsdWUgfSk7XG4gXHRcdGlmKG1vZGUgJiAyICYmIHR5cGVvZiB2YWx1ZSAhPSAnc3RyaW5nJykgZm9yKHZhciBrZXkgaW4gdmFsdWUpIF9fd2VicGFja19yZXF1aXJlX18uZChucywga2V5LCBmdW5jdGlvbihrZXkpIHsgcmV0dXJuIHZhbHVlW2tleV07IH0uYmluZChudWxsLCBrZXkpKTtcbiBcdFx0cmV0dXJuIG5zO1xuIFx0fTtcblxuIFx0Ly8gZ2V0RGVmYXVsdEV4cG9ydCBmdW5jdGlvbiBmb3IgY29tcGF0aWJpbGl0eSB3aXRoIG5vbi1oYXJtb255IG1vZHVsZXNcbiBcdF9fd2VicGFja19yZXF1aXJlX18ubiA9IGZ1bmN0aW9uKG1vZHVsZSkge1xuIFx0XHR2YXIgZ2V0dGVyID0gbW9kdWxlICYmIG1vZHVsZS5fX2VzTW9kdWxlID9cbiBcdFx0XHRmdW5jdGlvbiBnZXREZWZhdWx0KCkgeyByZXR1cm4gbW9kdWxlWydkZWZhdWx0J107IH0gOlxuIFx0XHRcdGZ1bmN0aW9uIGdldE1vZHVsZUV4cG9ydHMoKSB7IHJldHVybiBtb2R1bGU7IH07XG4gXHRcdF9fd2VicGFja19yZXF1aXJlX18uZChnZXR0ZXIsICdhJywgZ2V0dGVyKTtcbiBcdFx0cmV0dXJuIGdldHRlcjtcbiBcdH07XG5cbiBcdC8vIE9iamVjdC5wcm90b3R5cGUuaGFzT3duUHJvcGVydHkuY2FsbFxuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5vID0gZnVuY3Rpb24ob2JqZWN0LCBwcm9wZXJ0eSkgeyByZXR1cm4gT2JqZWN0LnByb3RvdHlwZS5oYXNPd25Qcm9wZXJ0eS5jYWxsKG9iamVjdCwgcHJvcGVydHkpOyB9O1xuXG4gXHQvLyBfX3dlYnBhY2tfcHVibGljX3BhdGhfX1xuIFx0X193ZWJwYWNrX3JlcXVpcmVfXy5wID0gXCJcIjtcblxuXG4gXHQvLyBMb2FkIGVudHJ5IG1vZHVsZSBhbmQgcmV0dXJuIGV4cG9ydHNcbiBcdHJldHVybiBfX3dlYnBhY2tfcmVxdWlyZV9fKF9fd2VicGFja19yZXF1aXJlX18ucyA9IFwiLi9UU1gvVHJlbmRpbmdEYXRhRGlzcGxheS50c3hcIik7XG4iLCJcInVzZSBzdHJpY3RcIjtcclxuLy8gKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbi8vICBDcmVhdGVHdWlkLnRzIC0gR2J0Y1xyXG4vL1xyXG4vLyAgQ29weXJpZ2h0IMKpIDIwMjEsIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZS4gIEFsbCBSaWdodHMgUmVzZXJ2ZWQuXHJcbi8vXHJcbi8vICBMaWNlbnNlZCB0byB0aGUgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlIChHUEEpIHVuZGVyIG9uZSBvciBtb3JlIGNvbnRyaWJ1dG9yIGxpY2Vuc2UgYWdyZWVtZW50cy4gU2VlXHJcbi8vICB0aGUgTk9USUNFIGZpbGUgZGlzdHJpYnV0ZWQgd2l0aCB0aGlzIHdvcmsgZm9yIGFkZGl0aW9uYWwgaW5mb3JtYXRpb24gcmVnYXJkaW5nIGNvcHlyaWdodCBvd25lcnNoaXAuXHJcbi8vICBUaGUgR1BBIGxpY2Vuc2VzIHRoaXMgZmlsZSB0byB5b3UgdW5kZXIgdGhlIE1JVCBMaWNlbnNlIChNSVQpLCB0aGUgXCJMaWNlbnNlXCI7IHlvdSBtYXkgbm90IHVzZSB0aGlzXHJcbi8vICBmaWxlIGV4Y2VwdCBpbiBjb21wbGlhbmNlIHdpdGggdGhlIExpY2Vuc2UuIFlvdSBtYXkgb2J0YWluIGEgY29weSBvZiB0aGUgTGljZW5zZSBhdDpcclxuLy9cclxuLy8gICAgICBodHRwOi8vb3BlbnNvdXJjZS5vcmcvbGljZW5zZXMvTUlUXHJcbi8vXHJcbi8vICBVbmxlc3MgYWdyZWVkIHRvIGluIHdyaXRpbmcsIHRoZSBzdWJqZWN0IHNvZnR3YXJlIGRpc3RyaWJ1dGVkIHVuZGVyIHRoZSBMaWNlbnNlIGlzIGRpc3RyaWJ1dGVkIG9uIGFuXHJcbi8vICBcIkFTLUlTXCIgQkFTSVMsIFdJVEhPVVQgV0FSUkFOVElFUyBPUiBDT05ESVRJT05TIE9GIEFOWSBLSU5ELCBlaXRoZXIgZXhwcmVzcyBvciBpbXBsaWVkLiBSZWZlciB0byB0aGVcclxuLy8gIExpY2Vuc2UgZm9yIHRoZSBzcGVjaWZpYyBsYW5ndWFnZSBnb3Zlcm5pbmcgcGVybWlzc2lvbnMgYW5kIGxpbWl0YXRpb25zLlxyXG4vLyAgXHJcbi8vICBodHRwczovL3N0YWNrb3ZlcmZsb3cuY29tL3F1ZXN0aW9ucy8xMDUwMzQvaG93LXRvLWNyZWF0ZS1hLWd1aWQtdXVpZFxyXG4vL1xyXG4vLyAgQ29kZSBNb2RpZmljYXRpb24gSGlzdG9yeTpcclxuLy8gIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuLy8gIDAxLzA0LzIwMjEgLSBCaWxseSBFcm5lc3RcclxuLy8gICAgICAgR2VuZXJhdGVkIG9yaWdpbmFsIHZlcnNpb24gb2Ygc291cmNlIGNvZGUuXHJcbi8vXHJcbi8vICoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJfX2VzTW9kdWxlXCIsIHsgdmFsdWU6IHRydWUgfSk7XHJcbmV4cG9ydHMuQ3JlYXRlR3VpZCA9IHZvaWQgMDtcclxuLyoqXHJcbiAqIFRoaXMgZnVuY3Rpb24gZ2VuZXJhdGVzIGEgR1VJRFxyXG4gKi9cclxuZnVuY3Rpb24gQ3JlYXRlR3VpZCgpIHtcclxuICAgIHJldHVybiAneHh4eHh4eHgteHh4eC00eHh4LXl4eHgteHh4eHh4eHh4eHh4Jy5yZXBsYWNlKC9beHldL2csIGZ1bmN0aW9uIChjKSB7XHJcbiAgICAgICAgdmFyIHIgPSBNYXRoLnJhbmRvbSgpICogMTYgfCAwO1xyXG4gICAgICAgIHZhciB2ID0gYyA9PT0gJ3gnID8gciA6IChyICYgMHgzIHwgMHg4KTtcclxuICAgICAgICByZXR1cm4gdi50b1N0cmluZygxNik7XHJcbiAgICB9KTtcclxufVxyXG5leHBvcnRzLkNyZWF0ZUd1aWQgPSBDcmVhdGVHdWlkO1xyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxuLy8gKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbi8vICBHZXROb2RlU2l6ZS50c3ggLSBHYnRjXHJcbi8vXHJcbi8vICBDb3B5cmlnaHQgwqkgMjAyMSwgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlLiAgQWxsIFJpZ2h0cyBSZXNlcnZlZC5cclxuLy9cclxuLy8gIExpY2Vuc2VkIHRvIHRoZSBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UgKEdQQSkgdW5kZXIgb25lIG9yIG1vcmUgY29udHJpYnV0b3IgbGljZW5zZSBhZ3JlZW1lbnRzLiBTZWVcclxuLy8gIHRoZSBOT1RJQ0UgZmlsZSBkaXN0cmlidXRlZCB3aXRoIHRoaXMgd29yayBmb3IgYWRkaXRpb25hbCBpbmZvcm1hdGlvbiByZWdhcmRpbmcgY29weXJpZ2h0IG93bmVyc2hpcC5cclxuLy8gIFRoZSBHUEEgbGljZW5zZXMgdGhpcyBmaWxlIHRvIHlvdSB1bmRlciB0aGUgTUlUIExpY2Vuc2UgKE1JVCksIHRoZSBcIkxpY2Vuc2VcIjsgeW91IG1heSBub3QgdXNlIHRoaXNcclxuLy8gIGZpbGUgZXhjZXB0IGluIGNvbXBsaWFuY2Ugd2l0aCB0aGUgTGljZW5zZS4gWW91IG1heSBvYnRhaW4gYSBjb3B5IG9mIHRoZSBMaWNlbnNlIGF0OlxyXG4vL1xyXG4vLyAgICAgIGh0dHA6Ly9vcGVuc291cmNlLm9yZy9saWNlbnNlcy9NSVRcclxuLy9cclxuLy8gIFVubGVzcyBhZ3JlZWQgdG8gaW4gd3JpdGluZywgdGhlIHN1YmplY3Qgc29mdHdhcmUgZGlzdHJpYnV0ZWQgdW5kZXIgdGhlIExpY2Vuc2UgaXMgZGlzdHJpYnV0ZWQgb24gYW5cclxuLy8gIFwiQVMtSVNcIiBCQVNJUywgV0lUSE9VVCBXQVJSQU5USUVTIE9SIENPTkRJVElPTlMgT0YgQU5ZIEtJTkQsIGVpdGhlciBleHByZXNzIG9yIGltcGxpZWQuIFJlZmVyIHRvIHRoZVxyXG4vLyAgTGljZW5zZSBmb3IgdGhlIHNwZWNpZmljIGxhbmd1YWdlIGdvdmVybmluZyBwZXJtaXNzaW9ucyBhbmQgbGltaXRhdGlvbnMuXHJcbi8vXHJcbi8vICBDb2RlIE1vZGlmaWNhdGlvbiBIaXN0b3J5OlxyXG4vLyAgLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG4vLyAgMDEvMTUvMjAyMSAtIEMuIExhY2tuZXJcclxuLy8gICAgICAgR2VuZXJhdGVkIG9yaWdpbmFsIHZlcnNpb24gb2Ygc291cmNlIGNvZGUuXHJcbi8vXHJcbi8vICoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJfX2VzTW9kdWxlXCIsIHsgdmFsdWU6IHRydWUgfSk7XHJcbmV4cG9ydHMuR2V0Tm9kZVNpemUgPSB2b2lkIDA7XHJcbi8qKlxyXG4gKiBHZXROb2RlU2l6ZSByZXR1cm5zIHRoZSBkaW1lbnNpb25zIG9mIGFuIGh0bWwgZWxlbWVudFxyXG4gKiBAcGFyYW0gbm9kZTogYSBIVE1MIGVsZW1lbnQsIG9yIG51bGwgY2FuIGJlIHBhc3NlZCB0aHJvdWdoXHJcbiAqL1xyXG5mdW5jdGlvbiBHZXROb2RlU2l6ZShub2RlKSB7XHJcbiAgICBpZiAobm9kZSA9PT0gbnVsbClcclxuICAgICAgICByZXR1cm4ge1xyXG4gICAgICAgICAgICBoZWlnaHQ6IDAsXHJcbiAgICAgICAgICAgIHdpZHRoOiAwLFxyXG4gICAgICAgICAgICB0b3A6IDAsXHJcbiAgICAgICAgICAgIGxlZnQ6IDAsXHJcbiAgICAgICAgfTtcclxuICAgIHZhciBfYSA9IG5vZGUuZ2V0Qm91bmRpbmdDbGllbnRSZWN0KCksIGhlaWdodCA9IF9hLmhlaWdodCwgd2lkdGggPSBfYS53aWR0aCwgdG9wID0gX2EudG9wLCBsZWZ0ID0gX2EubGVmdDtcclxuICAgIHJldHVybiB7XHJcbiAgICAgICAgaGVpZ2h0OiBwYXJzZUludChoZWlnaHQudG9TdHJpbmcoKSwgMTApLFxyXG4gICAgICAgIHdpZHRoOiBwYXJzZUludCh3aWR0aC50b1N0cmluZygpLCAxMCksXHJcbiAgICAgICAgdG9wOiBwYXJzZUludCh0b3AudG9TdHJpbmcoKSwgMTApLFxyXG4gICAgICAgIGxlZnQ6IHBhcnNlSW50KGxlZnQudG9TdHJpbmcoKSwgMTApLFxyXG4gICAgfTtcclxufVxyXG5leHBvcnRzLkdldE5vZGVTaXplID0gR2V0Tm9kZVNpemU7XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuLy8gIEdldFRleHRIZWlnaHQudHN4IC0gR2J0Y1xyXG4vL1xyXG4vLyAgQ29weXJpZ2h0IMKpIDIwMjEsIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZS4gIEFsbCBSaWdodHMgUmVzZXJ2ZWQuXHJcbi8vXHJcbi8vICBMaWNlbnNlZCB0byB0aGUgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlIChHUEEpIHVuZGVyIG9uZSBvciBtb3JlIGNvbnRyaWJ1dG9yIGxpY2Vuc2UgYWdyZWVtZW50cy4gU2VlXHJcbi8vICB0aGUgTk9USUNFIGZpbGUgZGlzdHJpYnV0ZWQgd2l0aCB0aGlzIHdvcmsgZm9yIGFkZGl0aW9uYWwgaW5mb3JtYXRpb24gcmVnYXJkaW5nIGNvcHlyaWdodCBvd25lcnNoaXAuXHJcbi8vICBUaGUgR1BBIGxpY2Vuc2VzIHRoaXMgZmlsZSB0byB5b3UgdW5kZXIgdGhlIE1JVCBMaWNlbnNlIChNSVQpLCB0aGUgXCJMaWNlbnNlXCI7IHlvdSBtYXkgbm90IHVzZSB0aGlzXHJcbi8vICBmaWxlIGV4Y2VwdCBpbiBjb21wbGlhbmNlIHdpdGggdGhlIExpY2Vuc2UuIFlvdSBtYXkgb2J0YWluIGEgY29weSBvZiB0aGUgTGljZW5zZSBhdDpcclxuLy9cclxuLy8gICAgICBodHRwOi8vb3BlbnNvdXJjZS5vcmcvbGljZW5zZXMvTUlUXHJcbi8vXHJcbi8vICBVbmxlc3MgYWdyZWVkIHRvIGluIHdyaXRpbmcsIHRoZSBzdWJqZWN0IHNvZnR3YXJlIGRpc3RyaWJ1dGVkIHVuZGVyIHRoZSBMaWNlbnNlIGlzIGRpc3RyaWJ1dGVkIG9uIGFuXHJcbi8vICBcIkFTLUlTXCIgQkFTSVMsIFdJVEhPVVQgV0FSUkFOVElFUyBPUiBDT05ESVRJT05TIE9GIEFOWSBLSU5ELCBlaXRoZXIgZXhwcmVzcyBvciBpbXBsaWVkLiBSZWZlciB0byB0aGVcclxuLy8gIExpY2Vuc2UgZm9yIHRoZSBzcGVjaWZpYyBsYW5ndWFnZSBnb3Zlcm5pbmcgcGVybWlzc2lvbnMgYW5kIGxpbWl0YXRpb25zLlxyXG4vL1xyXG4vLyAgQ29kZSBNb2RpZmljYXRpb24gSGlzdG9yeTpcclxuLy8gIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuLy8gIDAzLzEyLzIwMjEgLSBjLiBMYWNrbmVyXHJcbi8vICAgICAgIEdlbmVyYXRlZCBvcmlnaW5hbCB2ZXJzaW9uIG9mIHNvdXJjZSBjb2RlLlxyXG4vL1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuT2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFwiX19lc01vZHVsZVwiLCB7IHZhbHVlOiB0cnVlIH0pO1xyXG5leHBvcnRzLkdldFRleHRIZWlnaHQgPSB2b2lkIDA7XHJcbi8qKlxyXG4gKiBUaGlzIGZ1bmN0aW9uIHJldHVybnMgdGhlIGhlaWdodCBvZiBhIHBpZWNlIG9mIHRleHQgZ2l2ZW4gYSBmb250LCBmb250c2l6ZSwgYW5kIGEgd29yZFxyXG4gKiBAcGFyYW0gZm9udDogRGV0ZXJtaW5lcyBmb250IG9mIGdpdmVuIHRleHRcclxuICogQHBhcmFtIGZvbnRTaXplOiBEZXRlcm1pbmVzIHNpemUgb2YgZ2l2ZW4gZm9udFxyXG4gKiBAcGFyYW0gd29yZDogVGV4dCB0byBtZWFzdXJlXHJcbiAqL1xyXG5mdW5jdGlvbiBHZXRUZXh0SGVpZ2h0KGZvbnQsIGZvbnRTaXplLCB3b3JkKSB7XHJcbiAgICB2YXIgdGV4dCA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoXCJzcGFuXCIpO1xyXG4gICAgZG9jdW1lbnQuYm9keS5hcHBlbmRDaGlsZCh0ZXh0KTtcclxuICAgIHRleHQuc3R5bGUuZm9udCA9IGZvbnQ7XHJcbiAgICB0ZXh0LnN0eWxlLmZvbnRTaXplID0gZm9udFNpemU7XHJcbiAgICB0ZXh0LnN0eWxlLmhlaWdodCA9ICdhdXRvJztcclxuICAgIHRleHQuc3R5bGUud2lkdGggPSAnYXV0byc7XHJcbiAgICB0ZXh0LnN0eWxlLnBvc2l0aW9uID0gJ2Fic29sdXRlJztcclxuICAgIHRleHQuc3R5bGUud2hpdGVTcGFjZSA9ICduby13cmFwJztcclxuICAgIHRleHQuaW5uZXJIVE1MID0gd29yZDtcclxuICAgIHZhciBoZWlnaHQgPSBNYXRoLmNlaWwodGV4dC5jbGllbnRIZWlnaHQpO1xyXG4gICAgZG9jdW1lbnQuYm9keS5yZW1vdmVDaGlsZCh0ZXh0KTtcclxuICAgIHJldHVybiBoZWlnaHQ7XHJcbn1cclxuZXhwb3J0cy5HZXRUZXh0SGVpZ2h0ID0gR2V0VGV4dEhlaWdodDtcclxuIiwiXCJ1c2Ugc3RyaWN0XCI7XHJcbi8vICoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG4vLyAgR2V0VGV4dFdpZHRoLnRzeCAtIEdidGNcclxuLy9cclxuLy8gIENvcHlyaWdodCDCqSAyMDIxLCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxyXG4vL1xyXG4vLyAgTGljZW5zZWQgdG8gdGhlIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZSAoR1BBKSB1bmRlciBvbmUgb3IgbW9yZSBjb250cmlidXRvciBsaWNlbnNlIGFncmVlbWVudHMuIFNlZVxyXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxyXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xyXG4vLyAgZmlsZSBleGNlcHQgaW4gY29tcGxpYW5jZSB3aXRoIHRoZSBMaWNlbnNlLiBZb3UgbWF5IG9idGFpbiBhIGNvcHkgb2YgdGhlIExpY2Vuc2UgYXQ6XHJcbi8vXHJcbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxyXG4vL1xyXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxyXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXHJcbi8vICBMaWNlbnNlIGZvciB0aGUgc3BlY2lmaWMgbGFuZ3VhZ2UgZ292ZXJuaW5nIHBlcm1pc3Npb25zIGFuZCBsaW1pdGF0aW9ucy5cclxuLy9cclxuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XHJcbi8vICAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vICAwMS8wNy8yMDIxIC0gQmlsbHkgRXJuZXN0XHJcbi8vICAgICAgIEdlbmVyYXRlZCBvcmlnaW5hbCB2ZXJzaW9uIG9mIHNvdXJjZSBjb2RlLlxyXG4vL1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuT2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFwiX19lc01vZHVsZVwiLCB7IHZhbHVlOiB0cnVlIH0pO1xyXG5leHBvcnRzLkdldFRleHRXaWR0aCA9IHZvaWQgMDtcclxuLyoqXHJcbiAqIEdldFRleHRXaWR0aCByZXR1cm5zIHRoZSB3aWR0aCBvZiBhIHBpZWNlIG9mIHRleHQgZ2l2ZW4gaXRzIGZvbnQsIGZvbnRTaXplLCBhbmQgY29udGVudC5cclxuICogQHBhcmFtIGZvbnQ6IERldGVybWluZXMgZm9udCBvZiBnaXZlbiB0ZXh0XHJcbiAqIEBwYXJhbSBmb250U2l6ZTogRGV0ZXJtaW5lcyBzaXplIG9mIGdpdmVuIGZvbnRcclxuICogQHBhcmFtIHdvcmQ6IFRleHQgdG8gbWVhc3VyZVxyXG4gKi9cclxuZnVuY3Rpb24gR2V0VGV4dFdpZHRoKGZvbnQsIGZvbnRTaXplLCB3b3JkKSB7XHJcbiAgICB2YXIgdGV4dCA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoXCJzcGFuXCIpO1xyXG4gICAgZG9jdW1lbnQuYm9keS5hcHBlbmRDaGlsZCh0ZXh0KTtcclxuICAgIHRleHQuc3R5bGUuZm9udCA9IGZvbnQ7XHJcbiAgICB0ZXh0LnN0eWxlLmZvbnRTaXplID0gZm9udFNpemU7XHJcbiAgICB0ZXh0LnN0eWxlLmhlaWdodCA9ICdhdXRvJztcclxuICAgIHRleHQuc3R5bGUud2lkdGggPSAnYXV0byc7XHJcbiAgICB0ZXh0LnN0eWxlLnBvc2l0aW9uID0gJ2Fic29sdXRlJztcclxuICAgIHRleHQuc3R5bGUud2hpdGVTcGFjZSA9ICduby13cmFwJztcclxuICAgIHRleHQuaW5uZXJIVE1MID0gd29yZDtcclxuICAgIHZhciB3aWR0aCA9IE1hdGguY2VpbCh0ZXh0LmNsaWVudFdpZHRoKTtcclxuICAgIGRvY3VtZW50LmJvZHkucmVtb3ZlQ2hpbGQodGV4dCk7XHJcbiAgICByZXR1cm4gd2lkdGg7XHJcbn1cclxuZXhwb3J0cy5HZXRUZXh0V2lkdGggPSBHZXRUZXh0V2lkdGg7XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuLy8gIElzSW50ZWdlci50c3ggLSBHYnRjXHJcbi8vXHJcbi8vICBDb3B5cmlnaHQgwqkgMjAyMSwgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlLiAgQWxsIFJpZ2h0cyBSZXNlcnZlZC5cclxuLy9cclxuLy8gIExpY2Vuc2VkIHRvIHRoZSBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UgKEdQQSkgdW5kZXIgb25lIG9yIG1vcmUgY29udHJpYnV0b3IgbGljZW5zZSBhZ3JlZW1lbnRzLiBTZWVcclxuLy8gIHRoZSBOT1RJQ0UgZmlsZSBkaXN0cmlidXRlZCB3aXRoIHRoaXMgd29yayBmb3IgYWRkaXRpb25hbCBpbmZvcm1hdGlvbiByZWdhcmRpbmcgY29weXJpZ2h0IG93bmVyc2hpcC5cclxuLy8gIFRoZSBHUEEgbGljZW5zZXMgdGhpcyBmaWxlIHRvIHlvdSB1bmRlciB0aGUgTUlUIExpY2Vuc2UgKE1JVCksIHRoZSBcIkxpY2Vuc2VcIjsgeW91IG1heSBub3QgdXNlIHRoaXNcclxuLy8gIGZpbGUgZXhjZXB0IGluIGNvbXBsaWFuY2Ugd2l0aCB0aGUgTGljZW5zZS4gWW91IG1heSBvYnRhaW4gYSBjb3B5IG9mIHRoZSBMaWNlbnNlIGF0OlxyXG4vL1xyXG4vLyAgICAgIGh0dHA6Ly9vcGVuc291cmNlLm9yZy9saWNlbnNlcy9NSVRcclxuLy9cclxuLy8gIFVubGVzcyBhZ3JlZWQgdG8gaW4gd3JpdGluZywgdGhlIHN1YmplY3Qgc29mdHdhcmUgZGlzdHJpYnV0ZWQgdW5kZXIgdGhlIExpY2Vuc2UgaXMgZGlzdHJpYnV0ZWQgb24gYW5cclxuLy8gIFwiQVMtSVNcIiBCQVNJUywgV0lUSE9VVCBXQVJSQU5USUVTIE9SIENPTkRJVElPTlMgT0YgQU5ZIEtJTkQsIGVpdGhlciBleHByZXNzIG9yIGltcGxpZWQuIFJlZmVyIHRvIHRoZVxyXG4vLyAgTGljZW5zZSBmb3IgdGhlIHNwZWNpZmljIGxhbmd1YWdlIGdvdmVybmluZyBwZXJtaXNzaW9ucyBhbmQgbGltaXRhdGlvbnMuXHJcbi8vXHJcbi8vICBDb2RlIE1vZGlmaWNhdGlvbiBIaXN0b3J5OlxyXG4vLyAgLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG4vLyAgMDcvMTYvMjAyMSAtIEMuIExhY2tuZXJcclxuLy8gICAgICAgR2VuZXJhdGVkIG9yaWdpbmFsIHZlcnNpb24gb2Ygc291cmNlIGNvZGUuXHJcbi8vXHJcbi8vICoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJfX2VzTW9kdWxlXCIsIHsgdmFsdWU6IHRydWUgfSk7XHJcbmV4cG9ydHMuSXNJbnRlZ2VyID0gdm9pZCAwO1xyXG4vKipcclxuICogSXNJbnRlZ2VyIGNoZWNrcyBpZiB2YWx1ZSBwYXNzZWQgdGhyb3VnaCBpcyBhbiBpbnRlZ2VyLCByZXR1cm5pbmcgYSB0cnVlIG9yIGZhbHNlXHJcbiAqIEBwYXJhbSB2YWx1ZTogdmFsdWUgaXMgdGhlIGlucHV0IHBhc3NlZCB0aHJvdWdoIHRoZSBJc0ludGVnZXIgZnVuY3Rpb25cclxuICovXHJcbmZ1bmN0aW9uIElzSW50ZWdlcih2YWx1ZSkge1xyXG4gICAgdmFyIHJlZ2V4ID0gL14tP1swLTldKyQvO1xyXG4gICAgcmV0dXJuIHZhbHVlLnRvU3RyaW5nKCkubWF0Y2gocmVnZXgpICE9IG51bGw7XHJcbn1cclxuZXhwb3J0cy5Jc0ludGVnZXIgPSBJc0ludGVnZXI7XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuLy8gIElzTnVtYmVyLnRzeCAtIEdidGNcclxuLy9cclxuLy8gIENvcHlyaWdodCDCqSAyMDIxLCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxyXG4vL1xyXG4vLyAgTGljZW5zZWQgdG8gdGhlIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZSAoR1BBKSB1bmRlciBvbmUgb3IgbW9yZSBjb250cmlidXRvciBsaWNlbnNlIGFncmVlbWVudHMuIFNlZVxyXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxyXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xyXG4vLyAgZmlsZSBleGNlcHQgaW4gY29tcGxpYW5jZSB3aXRoIHRoZSBMaWNlbnNlLiBZb3UgbWF5IG9idGFpbiBhIGNvcHkgb2YgdGhlIExpY2Vuc2UgYXQ6XHJcbi8vXHJcbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxyXG4vL1xyXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxyXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXHJcbi8vICBMaWNlbnNlIGZvciB0aGUgc3BlY2lmaWMgbGFuZ3VhZ2UgZ292ZXJuaW5nIHBlcm1pc3Npb25zIGFuZCBsaW1pdGF0aW9ucy5cclxuLy9cclxuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XHJcbi8vICAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vICAwMS8wNy8yMDIxIC0gQmlsbHkgRXJuZXN0XHJcbi8vICAgICAgIEdlbmVyYXRlZCBvcmlnaW5hbCB2ZXJzaW9uIG9mIHNvdXJjZSBjb2RlLlxyXG4vL1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuT2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFwiX19lc01vZHVsZVwiLCB7IHZhbHVlOiB0cnVlIH0pO1xyXG5leHBvcnRzLklzTnVtYmVyID0gdm9pZCAwO1xyXG4vKipcclxuICogVGhpcyBmdW5jdGlvbiBjaGVja3MgaWYgYW55IHZhbHVlIGlzIGEgbnVtYmVyLCByZXR1cm5pbmcgdHJ1ZSBvciBmYWxzZVxyXG4gKiBAcGFyYW0gdmFsdWU6IHZhbHVlIGlzIHRoZSBpbnB1dCBwYXNzZWQgdGhyb3VnaCB0aGUgSXNOdW1iZXIgZnVuY3Rpb25cclxuICovXHJcbmZ1bmN0aW9uIElzTnVtYmVyKHZhbHVlKSB7XHJcbiAgICB2YXIgcmVnZXggPSAvXi0/WzAtOV0rKFxcLlswLTldKyk/JC87XHJcbiAgICByZXR1cm4gdmFsdWUudG9TdHJpbmcoKS5tYXRjaChyZWdleCkgIT0gbnVsbDtcclxufVxyXG5leHBvcnRzLklzTnVtYmVyID0gSXNOdW1iZXI7XHJcbiIsIlwidXNlIHN0cmljdFwiO1xyXG4vLyAqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcclxuLy8gIFJhbmRvbUNvbG9yLnRzeCAtIEdidGNcclxuLy9cclxuLy8gIENvcHlyaWdodCDCqSAyMDIxLCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxyXG4vL1xyXG4vLyAgTGljZW5zZWQgdG8gdGhlIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZSAoR1BBKSB1bmRlciBvbmUgb3IgbW9yZSBjb250cmlidXRvciBsaWNlbnNlIGFncmVlbWVudHMuIFNlZVxyXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxyXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xyXG4vLyAgZmlsZSBleGNlcHQgaW4gY29tcGxpYW5jZSB3aXRoIHRoZSBMaWNlbnNlLiBZb3UgbWF5IG9idGFpbiBhIGNvcHkgb2YgdGhlIExpY2Vuc2UgYXQ6XHJcbi8vXHJcbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxyXG4vL1xyXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxyXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXHJcbi8vICBMaWNlbnNlIGZvciB0aGUgc3BlY2lmaWMgbGFuZ3VhZ2UgZ292ZXJuaW5nIHBlcm1pc3Npb25zIGFuZCBsaW1pdGF0aW9ucy5cclxuLy9cclxuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XHJcbi8vICAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vICAwMS8xNS8yMDIxIC0gQy4gTGFja25lclxyXG4vLyAgICAgICBHZW5lcmF0ZWQgb3JpZ2luYWwgdmVyc2lvbiBvZiBzb3VyY2UgY29kZS5cclxuLy9cclxuLy8gKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcIl9fZXNNb2R1bGVcIiwgeyB2YWx1ZTogdHJ1ZSB9KTtcclxuZXhwb3J0cy5SYW5kb21Db2xvciA9IHZvaWQgMDtcclxuLyoqXHJcbiAqIFRoaXMgZnVuY3Rpb24gcmV0dXJucyBhIHJhbmRvbSBjb2xvclxyXG4gKi9cclxuZnVuY3Rpb24gUmFuZG9tQ29sb3IoKSB7XHJcbiAgICByZXR1cm4gJyMnICsgTWF0aC5yYW5kb20oKS50b1N0cmluZygxNikuc3Vic3RyKDIsIDYpLnRvVXBwZXJDYXNlKCk7XHJcbn1cclxuZXhwb3J0cy5SYW5kb21Db2xvciA9IFJhbmRvbUNvbG9yO1xyXG4iLCJcInVzZSBzdHJpY3RcIjtcclxuLy8gKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXHJcbi8vICBpbmRleC50cyAtIEdidGNcclxuLy9cclxuLy8gIENvcHlyaWdodCDvv70gMjAyMSwgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlLiAgQWxsIFJpZ2h0cyBSZXNlcnZlZC5cclxuLy9cclxuLy8gIExpY2Vuc2VkIHRvIHRoZSBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UgKEdQQSkgdW5kZXIgb25lIG9yIG1vcmUgY29udHJpYnV0b3IgbGljZW5zZSBhZ3JlZW1lbnRzLiBTZWVcclxuLy8gIHRoZSBOT1RJQ0UgZmlsZSBkaXN0cmlidXRlZCB3aXRoIHRoaXMgd29yayBmb3IgYWRkaXRpb25hbCBpbmZvcm1hdGlvbiByZWdhcmRpbmcgY29weXJpZ2h0IG93bmVyc2hpcC5cclxuLy8gIFRoZSBHUEEgbGljZW5zZXMgdGhpcyBmaWxlIHRvIHlvdSB1bmRlciB0aGUgTUlUIExpY2Vuc2UgKE1JVCksIHRoZSBcIkxpY2Vuc2VcIjsgeW91IG1heSBub3QgdXNlIHRoaXNcclxuLy8gIGZpbGUgZXhjZXB0IGluIGNvbXBsaWFuY2Ugd2l0aCB0aGUgTGljZW5zZS4gWW91IG1heSBvYnRhaW4gYSBjb3B5IG9mIHRoZSBMaWNlbnNlIGF0OlxyXG4vL1xyXG4vLyAgICAgIGh0dHA6Ly9vcGVuc291cmNlLm9yZy9saWNlbnNlcy9NSVRcclxuLy9cclxuLy8gIFVubGVzcyBhZ3JlZWQgdG8gaW4gd3JpdGluZywgdGhlIHN1YmplY3Qgc29mdHdhcmUgZGlzdHJpYnV0ZWQgdW5kZXIgdGhlIExpY2Vuc2UgaXMgZGlzdHJpYnV0ZWQgb24gYW5cclxuLy8gIFwiQVMtSVNcIiBCQVNJUywgV0lUSE9VVCBXQVJSQU5USUVTIE9SIENPTkRJVElPTlMgT0YgQU5ZIEtJTkQsIGVpdGhlciBleHByZXNzIG9yIGltcGxpZWQuIFJlZmVyIHRvIHRoZVxyXG4vLyAgTGljZW5zZSBmb3IgdGhlIHNwZWNpZmljIGxhbmd1YWdlIGdvdmVybmluZyBwZXJtaXNzaW9ucyBhbmQgbGltaXRhdGlvbnMuXHJcbi8vXHJcbi8vICBodHRwczovL3N0YWNrb3ZlcmZsb3cuY29tL3F1ZXN0aW9ucy8xMDUwMzQvaG93LXRvLWNyZWF0ZS1hLWd1aWQtdXVpZFxyXG4vL1xyXG4vLyAgQ29kZSBNb2RpZmljYXRpb24gSGlzdG9yeTpcclxuLy8gIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuLy8gIDAxLzA0LzIwMjEgLSBCaWxseSBFcm5lc3RcclxuLy8gICAgICAgR2VuZXJhdGVkIG9yaWdpbmFsIHZlcnNpb24gb2Ygc291cmNlIGNvZGUuXHJcbi8vXHJcbi8vICoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJfX2VzTW9kdWxlXCIsIHsgdmFsdWU6IHRydWUgfSk7XHJcbmV4cG9ydHMuSXNJbnRlZ2VyID0gZXhwb3J0cy5Jc051bWJlciA9IGV4cG9ydHMuR2V0VGV4dEhlaWdodCA9IGV4cG9ydHMuUmFuZG9tQ29sb3IgPSBleHBvcnRzLkdldE5vZGVTaXplID0gZXhwb3J0cy5HZXRUZXh0V2lkdGggPSBleHBvcnRzLkNyZWF0ZUd1aWQgPSB2b2lkIDA7XHJcbnZhciBDcmVhdGVHdWlkXzEgPSByZXF1aXJlKFwiLi9DcmVhdGVHdWlkXCIpO1xyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJDcmVhdGVHdWlkXCIsIHsgZW51bWVyYWJsZTogdHJ1ZSwgZ2V0OiBmdW5jdGlvbiAoKSB7IHJldHVybiBDcmVhdGVHdWlkXzEuQ3JlYXRlR3VpZDsgfSB9KTtcclxudmFyIEdldFRleHRXaWR0aF8xID0gcmVxdWlyZShcIi4vR2V0VGV4dFdpZHRoXCIpO1xyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJHZXRUZXh0V2lkdGhcIiwgeyBlbnVtZXJhYmxlOiB0cnVlLCBnZXQ6IGZ1bmN0aW9uICgpIHsgcmV0dXJuIEdldFRleHRXaWR0aF8xLkdldFRleHRXaWR0aDsgfSB9KTtcclxudmFyIEdldFRleHRIZWlnaHRfMSA9IHJlcXVpcmUoXCIuL0dldFRleHRIZWlnaHRcIik7XHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcIkdldFRleHRIZWlnaHRcIiwgeyBlbnVtZXJhYmxlOiB0cnVlLCBnZXQ6IGZ1bmN0aW9uICgpIHsgcmV0dXJuIEdldFRleHRIZWlnaHRfMS5HZXRUZXh0SGVpZ2h0OyB9IH0pO1xyXG52YXIgR2V0Tm9kZVNpemVfMSA9IHJlcXVpcmUoXCIuL0dldE5vZGVTaXplXCIpO1xyXG5PYmplY3QuZGVmaW5lUHJvcGVydHkoZXhwb3J0cywgXCJHZXROb2RlU2l6ZVwiLCB7IGVudW1lcmFibGU6IHRydWUsIGdldDogZnVuY3Rpb24gKCkgeyByZXR1cm4gR2V0Tm9kZVNpemVfMS5HZXROb2RlU2l6ZTsgfSB9KTtcclxudmFyIFJhbmRvbUNvbG9yXzEgPSByZXF1aXJlKFwiLi9SYW5kb21Db2xvclwiKTtcclxuT2JqZWN0LmRlZmluZVByb3BlcnR5KGV4cG9ydHMsIFwiUmFuZG9tQ29sb3JcIiwgeyBlbnVtZXJhYmxlOiB0cnVlLCBnZXQ6IGZ1bmN0aW9uICgpIHsgcmV0dXJuIFJhbmRvbUNvbG9yXzEuUmFuZG9tQ29sb3I7IH0gfSk7XHJcbnZhciBJc051bWJlcl8xID0gcmVxdWlyZShcIi4vSXNOdW1iZXJcIik7XHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcIklzTnVtYmVyXCIsIHsgZW51bWVyYWJsZTogdHJ1ZSwgZ2V0OiBmdW5jdGlvbiAoKSB7IHJldHVybiBJc051bWJlcl8xLklzTnVtYmVyOyB9IH0pO1xyXG52YXIgSXNJbnRlZ2VyXzEgPSByZXF1aXJlKFwiLi9Jc0ludGVnZXJcIik7XHJcbk9iamVjdC5kZWZpbmVQcm9wZXJ0eShleHBvcnRzLCBcIklzSW50ZWdlclwiLCB7IGVudW1lcmFibGU6IHRydWUsIGdldDogZnVuY3Rpb24gKCkgeyByZXR1cm4gSXNJbnRlZ2VyXzEuSXNJbnRlZ2VyOyB9IH0pO1xyXG4iLCIvKipcbiAqIENvcHlyaWdodCAoYykgMjAxMy1wcmVzZW50LCBGYWNlYm9vaywgSW5jLlxuICpcbiAqIFRoaXMgc291cmNlIGNvZGUgaXMgbGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlIGZvdW5kIGluIHRoZVxuICogTElDRU5TRSBmaWxlIGluIHRoZSByb290IGRpcmVjdG9yeSBvZiB0aGlzIHNvdXJjZSB0cmVlLlxuICpcbiAqL1xuXG4ndXNlIHN0cmljdCc7XG5cbnZhciBfYXNzaWduID0gcmVxdWlyZSgnb2JqZWN0LWFzc2lnbicpO1xuXG4vLyAtLSBJbmxpbmVkIGZyb20gZmJqcyAtLVxuXG52YXIgZW1wdHlPYmplY3QgPSB7fTtcblxuaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgT2JqZWN0LmZyZWV6ZShlbXB0eU9iamVjdCk7XG59XG5cbnZhciB2YWxpZGF0ZUZvcm1hdCA9IGZ1bmN0aW9uIHZhbGlkYXRlRm9ybWF0KGZvcm1hdCkge307XG5cbmlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gIHZhbGlkYXRlRm9ybWF0ID0gZnVuY3Rpb24gdmFsaWRhdGVGb3JtYXQoZm9ybWF0KSB7XG4gICAgaWYgKGZvcm1hdCA9PT0gdW5kZWZpbmVkKSB7XG4gICAgICB0aHJvdyBuZXcgRXJyb3IoJ2ludmFyaWFudCByZXF1aXJlcyBhbiBlcnJvciBtZXNzYWdlIGFyZ3VtZW50Jyk7XG4gICAgfVxuICB9O1xufVxuXG5mdW5jdGlvbiBfaW52YXJpYW50KGNvbmRpdGlvbiwgZm9ybWF0LCBhLCBiLCBjLCBkLCBlLCBmKSB7XG4gIHZhbGlkYXRlRm9ybWF0KGZvcm1hdCk7XG5cbiAgaWYgKCFjb25kaXRpb24pIHtcbiAgICB2YXIgZXJyb3I7XG4gICAgaWYgKGZvcm1hdCA9PT0gdW5kZWZpbmVkKSB7XG4gICAgICBlcnJvciA9IG5ldyBFcnJvcignTWluaWZpZWQgZXhjZXB0aW9uIG9jY3VycmVkOyB1c2UgdGhlIG5vbi1taW5pZmllZCBkZXYgZW52aXJvbm1lbnQgJyArICdmb3IgdGhlIGZ1bGwgZXJyb3IgbWVzc2FnZSBhbmQgYWRkaXRpb25hbCBoZWxwZnVsIHdhcm5pbmdzLicpO1xuICAgIH0gZWxzZSB7XG4gICAgICB2YXIgYXJncyA9IFthLCBiLCBjLCBkLCBlLCBmXTtcbiAgICAgIHZhciBhcmdJbmRleCA9IDA7XG4gICAgICBlcnJvciA9IG5ldyBFcnJvcihmb3JtYXQucmVwbGFjZSgvJXMvZywgZnVuY3Rpb24gKCkge1xuICAgICAgICByZXR1cm4gYXJnc1thcmdJbmRleCsrXTtcbiAgICAgIH0pKTtcbiAgICAgIGVycm9yLm5hbWUgPSAnSW52YXJpYW50IFZpb2xhdGlvbic7XG4gICAgfVxuXG4gICAgZXJyb3IuZnJhbWVzVG9Qb3AgPSAxOyAvLyB3ZSBkb24ndCBjYXJlIGFib3V0IGludmFyaWFudCdzIG93biBmcmFtZVxuICAgIHRocm93IGVycm9yO1xuICB9XG59XG5cbnZhciB3YXJuaW5nID0gZnVuY3Rpb24oKXt9O1xuXG5pZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICB2YXIgcHJpbnRXYXJuaW5nID0gZnVuY3Rpb24gcHJpbnRXYXJuaW5nKGZvcm1hdCkge1xuICAgIGZvciAodmFyIF9sZW4gPSBhcmd1bWVudHMubGVuZ3RoLCBhcmdzID0gQXJyYXkoX2xlbiA+IDEgPyBfbGVuIC0gMSA6IDApLCBfa2V5ID0gMTsgX2tleSA8IF9sZW47IF9rZXkrKykge1xuICAgICAgYXJnc1tfa2V5IC0gMV0gPSBhcmd1bWVudHNbX2tleV07XG4gICAgfVxuXG4gICAgdmFyIGFyZ0luZGV4ID0gMDtcbiAgICB2YXIgbWVzc2FnZSA9ICdXYXJuaW5nOiAnICsgZm9ybWF0LnJlcGxhY2UoLyVzL2csIGZ1bmN0aW9uICgpIHtcbiAgICAgIHJldHVybiBhcmdzW2FyZ0luZGV4KytdO1xuICAgIH0pO1xuICAgIGlmICh0eXBlb2YgY29uc29sZSAhPT0gJ3VuZGVmaW5lZCcpIHtcbiAgICAgIGNvbnNvbGUuZXJyb3IobWVzc2FnZSk7XG4gICAgfVxuICAgIHRyeSB7XG4gICAgICAvLyAtLS0gV2VsY29tZSB0byBkZWJ1Z2dpbmcgUmVhY3QgLS0tXG4gICAgICAvLyBUaGlzIGVycm9yIHdhcyB0aHJvd24gYXMgYSBjb252ZW5pZW5jZSBzbyB0aGF0IHlvdSBjYW4gdXNlIHRoaXMgc3RhY2tcbiAgICAgIC8vIHRvIGZpbmQgdGhlIGNhbGxzaXRlIHRoYXQgY2F1c2VkIHRoaXMgd2FybmluZyB0byBmaXJlLlxuICAgICAgdGhyb3cgbmV3IEVycm9yKG1lc3NhZ2UpO1xuICAgIH0gY2F0Y2ggKHgpIHt9XG4gIH07XG5cbiAgd2FybmluZyA9IGZ1bmN0aW9uIHdhcm5pbmcoY29uZGl0aW9uLCBmb3JtYXQpIHtcbiAgICBpZiAoZm9ybWF0ID09PSB1bmRlZmluZWQpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcignYHdhcm5pbmcoY29uZGl0aW9uLCBmb3JtYXQsIC4uLmFyZ3MpYCByZXF1aXJlcyBhIHdhcm5pbmcgJyArICdtZXNzYWdlIGFyZ3VtZW50Jyk7XG4gICAgfVxuXG4gICAgaWYgKGZvcm1hdC5pbmRleE9mKCdGYWlsZWQgQ29tcG9zaXRlIHByb3BUeXBlOiAnKSA9PT0gMCkge1xuICAgICAgcmV0dXJuOyAvLyBJZ25vcmUgQ29tcG9zaXRlQ29tcG9uZW50IHByb3B0eXBlIGNoZWNrLlxuICAgIH1cblxuICAgIGlmICghY29uZGl0aW9uKSB7XG4gICAgICBmb3IgKHZhciBfbGVuMiA9IGFyZ3VtZW50cy5sZW5ndGgsIGFyZ3MgPSBBcnJheShfbGVuMiA+IDIgPyBfbGVuMiAtIDIgOiAwKSwgX2tleTIgPSAyOyBfa2V5MiA8IF9sZW4yOyBfa2V5MisrKSB7XG4gICAgICAgIGFyZ3NbX2tleTIgLSAyXSA9IGFyZ3VtZW50c1tfa2V5Ml07XG4gICAgICB9XG5cbiAgICAgIHByaW50V2FybmluZy5hcHBseSh1bmRlZmluZWQsIFtmb3JtYXRdLmNvbmNhdChhcmdzKSk7XG4gICAgfVxuICB9O1xufVxuXG4vLyAvLS0gSW5saW5lZCBmcm9tIGZianMgLS1cblxudmFyIE1JWElOU19LRVkgPSAnbWl4aW5zJztcblxuLy8gSGVscGVyIGZ1bmN0aW9uIHRvIGFsbG93IHRoZSBjcmVhdGlvbiBvZiBhbm9ueW1vdXMgZnVuY3Rpb25zIHdoaWNoIGRvIG5vdFxuLy8gaGF2ZSAubmFtZSBzZXQgdG8gdGhlIG5hbWUgb2YgdGhlIHZhcmlhYmxlIGJlaW5nIGFzc2lnbmVkIHRvLlxuZnVuY3Rpb24gaWRlbnRpdHkoZm4pIHtcbiAgcmV0dXJuIGZuO1xufVxuXG52YXIgUmVhY3RQcm9wVHlwZUxvY2F0aW9uTmFtZXM7XG5pZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICBSZWFjdFByb3BUeXBlTG9jYXRpb25OYW1lcyA9IHtcbiAgICBwcm9wOiAncHJvcCcsXG4gICAgY29udGV4dDogJ2NvbnRleHQnLFxuICAgIGNoaWxkQ29udGV4dDogJ2NoaWxkIGNvbnRleHQnXG4gIH07XG59IGVsc2Uge1xuICBSZWFjdFByb3BUeXBlTG9jYXRpb25OYW1lcyA9IHt9O1xufVxuXG5mdW5jdGlvbiBmYWN0b3J5KFJlYWN0Q29tcG9uZW50LCBpc1ZhbGlkRWxlbWVudCwgUmVhY3ROb29wVXBkYXRlUXVldWUpIHtcbiAgLyoqXG4gICAqIFBvbGljaWVzIHRoYXQgZGVzY3JpYmUgbWV0aG9kcyBpbiBgUmVhY3RDbGFzc0ludGVyZmFjZWAuXG4gICAqL1xuXG4gIHZhciBpbmplY3RlZE1peGlucyA9IFtdO1xuXG4gIC8qKlxuICAgKiBDb21wb3NpdGUgY29tcG9uZW50cyBhcmUgaGlnaGVyLWxldmVsIGNvbXBvbmVudHMgdGhhdCBjb21wb3NlIG90aGVyIGNvbXBvc2l0ZVxuICAgKiBvciBob3N0IGNvbXBvbmVudHMuXG4gICAqXG4gICAqIFRvIGNyZWF0ZSBhIG5ldyB0eXBlIG9mIGBSZWFjdENsYXNzYCwgcGFzcyBhIHNwZWNpZmljYXRpb24gb2ZcbiAgICogeW91ciBuZXcgY2xhc3MgdG8gYFJlYWN0LmNyZWF0ZUNsYXNzYC4gVGhlIG9ubHkgcmVxdWlyZW1lbnQgb2YgeW91ciBjbGFzc1xuICAgKiBzcGVjaWZpY2F0aW9uIGlzIHRoYXQgeW91IGltcGxlbWVudCBhIGByZW5kZXJgIG1ldGhvZC5cbiAgICpcbiAgICogICB2YXIgTXlDb21wb25lbnQgPSBSZWFjdC5jcmVhdGVDbGFzcyh7XG4gICAqICAgICByZW5kZXI6IGZ1bmN0aW9uKCkge1xuICAgKiAgICAgICByZXR1cm4gPGRpdj5IZWxsbyBXb3JsZDwvZGl2PjtcbiAgICogICAgIH1cbiAgICogICB9KTtcbiAgICpcbiAgICogVGhlIGNsYXNzIHNwZWNpZmljYXRpb24gc3VwcG9ydHMgYSBzcGVjaWZpYyBwcm90b2NvbCBvZiBtZXRob2RzIHRoYXQgaGF2ZVxuICAgKiBzcGVjaWFsIG1lYW5pbmcgKGUuZy4gYHJlbmRlcmApLiBTZWUgYFJlYWN0Q2xhc3NJbnRlcmZhY2VgIGZvclxuICAgKiBtb3JlIHRoZSBjb21wcmVoZW5zaXZlIHByb3RvY29sLiBBbnkgb3RoZXIgcHJvcGVydGllcyBhbmQgbWV0aG9kcyBpbiB0aGVcbiAgICogY2xhc3Mgc3BlY2lmaWNhdGlvbiB3aWxsIGJlIGF2YWlsYWJsZSBvbiB0aGUgcHJvdG90eXBlLlxuICAgKlxuICAgKiBAaW50ZXJmYWNlIFJlYWN0Q2xhc3NJbnRlcmZhY2VcbiAgICogQGludGVybmFsXG4gICAqL1xuICB2YXIgUmVhY3RDbGFzc0ludGVyZmFjZSA9IHtcbiAgICAvKipcbiAgICAgKiBBbiBhcnJheSBvZiBNaXhpbiBvYmplY3RzIHRvIGluY2x1ZGUgd2hlbiBkZWZpbmluZyB5b3VyIGNvbXBvbmVudC5cbiAgICAgKlxuICAgICAqIEB0eXBlIHthcnJheX1cbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBtaXhpbnM6ICdERUZJTkVfTUFOWScsXG5cbiAgICAvKipcbiAgICAgKiBBbiBvYmplY3QgY29udGFpbmluZyBwcm9wZXJ0aWVzIGFuZCBtZXRob2RzIHRoYXQgc2hvdWxkIGJlIGRlZmluZWQgb25cbiAgICAgKiB0aGUgY29tcG9uZW50J3MgY29uc3RydWN0b3IgaW5zdGVhZCBvZiBpdHMgcHJvdG90eXBlIChzdGF0aWMgbWV0aG9kcykuXG4gICAgICpcbiAgICAgKiBAdHlwZSB7b2JqZWN0fVxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIHN0YXRpY3M6ICdERUZJTkVfTUFOWScsXG5cbiAgICAvKipcbiAgICAgKiBEZWZpbml0aW9uIG9mIHByb3AgdHlwZXMgZm9yIHRoaXMgY29tcG9uZW50LlxuICAgICAqXG4gICAgICogQHR5cGUge29iamVjdH1cbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBwcm9wVHlwZXM6ICdERUZJTkVfTUFOWScsXG5cbiAgICAvKipcbiAgICAgKiBEZWZpbml0aW9uIG9mIGNvbnRleHQgdHlwZXMgZm9yIHRoaXMgY29tcG9uZW50LlxuICAgICAqXG4gICAgICogQHR5cGUge29iamVjdH1cbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBjb250ZXh0VHlwZXM6ICdERUZJTkVfTUFOWScsXG5cbiAgICAvKipcbiAgICAgKiBEZWZpbml0aW9uIG9mIGNvbnRleHQgdHlwZXMgdGhpcyBjb21wb25lbnQgc2V0cyBmb3IgaXRzIGNoaWxkcmVuLlxuICAgICAqXG4gICAgICogQHR5cGUge29iamVjdH1cbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBjaGlsZENvbnRleHRUeXBlczogJ0RFRklORV9NQU5ZJyxcblxuICAgIC8vID09PT0gRGVmaW5pdGlvbiBtZXRob2RzID09PT1cblxuICAgIC8qKlxuICAgICAqIEludm9rZWQgd2hlbiB0aGUgY29tcG9uZW50IGlzIG1vdW50ZWQuIFZhbHVlcyBpbiB0aGUgbWFwcGluZyB3aWxsIGJlIHNldCBvblxuICAgICAqIGB0aGlzLnByb3BzYCBpZiB0aGF0IHByb3AgaXMgbm90IHNwZWNpZmllZCAoaS5lLiB1c2luZyBhbiBgaW5gIGNoZWNrKS5cbiAgICAgKlxuICAgICAqIFRoaXMgbWV0aG9kIGlzIGludm9rZWQgYmVmb3JlIGBnZXRJbml0aWFsU3RhdGVgIGFuZCB0aGVyZWZvcmUgY2Fubm90IHJlbHlcbiAgICAgKiBvbiBgdGhpcy5zdGF0ZWAgb3IgdXNlIGB0aGlzLnNldFN0YXRlYC5cbiAgICAgKlxuICAgICAqIEByZXR1cm4ge29iamVjdH1cbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBnZXREZWZhdWx0UHJvcHM6ICdERUZJTkVfTUFOWV9NRVJHRUQnLFxuXG4gICAgLyoqXG4gICAgICogSW52b2tlZCBvbmNlIGJlZm9yZSB0aGUgY29tcG9uZW50IGlzIG1vdW50ZWQuIFRoZSByZXR1cm4gdmFsdWUgd2lsbCBiZSB1c2VkXG4gICAgICogYXMgdGhlIGluaXRpYWwgdmFsdWUgb2YgYHRoaXMuc3RhdGVgLlxuICAgICAqXG4gICAgICogICBnZXRJbml0aWFsU3RhdGU6IGZ1bmN0aW9uKCkge1xuICAgICAqICAgICByZXR1cm4ge1xuICAgICAqICAgICAgIGlzT246IGZhbHNlLFxuICAgICAqICAgICAgIGZvb0JhejogbmV3IEJhekZvbygpXG4gICAgICogICAgIH1cbiAgICAgKiAgIH1cbiAgICAgKlxuICAgICAqIEByZXR1cm4ge29iamVjdH1cbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBnZXRJbml0aWFsU3RhdGU6ICdERUZJTkVfTUFOWV9NRVJHRUQnLFxuXG4gICAgLyoqXG4gICAgICogQHJldHVybiB7b2JqZWN0fVxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIGdldENoaWxkQ29udGV4dDogJ0RFRklORV9NQU5ZX01FUkdFRCcsXG5cbiAgICAvKipcbiAgICAgKiBVc2VzIHByb3BzIGZyb20gYHRoaXMucHJvcHNgIGFuZCBzdGF0ZSBmcm9tIGB0aGlzLnN0YXRlYCB0byByZW5kZXIgdGhlXG4gICAgICogc3RydWN0dXJlIG9mIHRoZSBjb21wb25lbnQuXG4gICAgICpcbiAgICAgKiBObyBndWFyYW50ZWVzIGFyZSBtYWRlIGFib3V0IHdoZW4gb3IgaG93IG9mdGVuIHRoaXMgbWV0aG9kIGlzIGludm9rZWQsIHNvXG4gICAgICogaXQgbXVzdCBub3QgaGF2ZSBzaWRlIGVmZmVjdHMuXG4gICAgICpcbiAgICAgKiAgIHJlbmRlcjogZnVuY3Rpb24oKSB7XG4gICAgICogICAgIHZhciBuYW1lID0gdGhpcy5wcm9wcy5uYW1lO1xuICAgICAqICAgICByZXR1cm4gPGRpdj5IZWxsbywge25hbWV9ITwvZGl2PjtcbiAgICAgKiAgIH1cbiAgICAgKlxuICAgICAqIEByZXR1cm4ge1JlYWN0Q29tcG9uZW50fVxuICAgICAqIEByZXF1aXJlZFxuICAgICAqL1xuICAgIHJlbmRlcjogJ0RFRklORV9PTkNFJyxcblxuICAgIC8vID09PT0gRGVsZWdhdGUgbWV0aG9kcyA9PT09XG5cbiAgICAvKipcbiAgICAgKiBJbnZva2VkIHdoZW4gdGhlIGNvbXBvbmVudCBpcyBpbml0aWFsbHkgY3JlYXRlZCBhbmQgYWJvdXQgdG8gYmUgbW91bnRlZC5cbiAgICAgKiBUaGlzIG1heSBoYXZlIHNpZGUgZWZmZWN0cywgYnV0IGFueSBleHRlcm5hbCBzdWJzY3JpcHRpb25zIG9yIGRhdGEgY3JlYXRlZFxuICAgICAqIGJ5IHRoaXMgbWV0aG9kIG11c3QgYmUgY2xlYW5lZCB1cCBpbiBgY29tcG9uZW50V2lsbFVubW91bnRgLlxuICAgICAqXG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgY29tcG9uZW50V2lsbE1vdW50OiAnREVGSU5FX01BTlknLFxuXG4gICAgLyoqXG4gICAgICogSW52b2tlZCB3aGVuIHRoZSBjb21wb25lbnQgaGFzIGJlZW4gbW91bnRlZCBhbmQgaGFzIGEgRE9NIHJlcHJlc2VudGF0aW9uLlxuICAgICAqIEhvd2V2ZXIsIHRoZXJlIGlzIG5vIGd1YXJhbnRlZSB0aGF0IHRoZSBET00gbm9kZSBpcyBpbiB0aGUgZG9jdW1lbnQuXG4gICAgICpcbiAgICAgKiBVc2UgdGhpcyBhcyBhbiBvcHBvcnR1bml0eSB0byBvcGVyYXRlIG9uIHRoZSBET00gd2hlbiB0aGUgY29tcG9uZW50IGhhc1xuICAgICAqIGJlZW4gbW91bnRlZCAoaW5pdGlhbGl6ZWQgYW5kIHJlbmRlcmVkKSBmb3IgdGhlIGZpcnN0IHRpbWUuXG4gICAgICpcbiAgICAgKiBAcGFyYW0ge0RPTUVsZW1lbnR9IHJvb3ROb2RlIERPTSBlbGVtZW50IHJlcHJlc2VudGluZyB0aGUgY29tcG9uZW50LlxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIGNvbXBvbmVudERpZE1vdW50OiAnREVGSU5FX01BTlknLFxuXG4gICAgLyoqXG4gICAgICogSW52b2tlZCBiZWZvcmUgdGhlIGNvbXBvbmVudCByZWNlaXZlcyBuZXcgcHJvcHMuXG4gICAgICpcbiAgICAgKiBVc2UgdGhpcyBhcyBhbiBvcHBvcnR1bml0eSB0byByZWFjdCB0byBhIHByb3AgdHJhbnNpdGlvbiBieSB1cGRhdGluZyB0aGVcbiAgICAgKiBzdGF0ZSB1c2luZyBgdGhpcy5zZXRTdGF0ZWAuIEN1cnJlbnQgcHJvcHMgYXJlIGFjY2Vzc2VkIHZpYSBgdGhpcy5wcm9wc2AuXG4gICAgICpcbiAgICAgKiAgIGNvbXBvbmVudFdpbGxSZWNlaXZlUHJvcHM6IGZ1bmN0aW9uKG5leHRQcm9wcywgbmV4dENvbnRleHQpIHtcbiAgICAgKiAgICAgdGhpcy5zZXRTdGF0ZSh7XG4gICAgICogICAgICAgbGlrZXNJbmNyZWFzaW5nOiBuZXh0UHJvcHMubGlrZUNvdW50ID4gdGhpcy5wcm9wcy5saWtlQ291bnRcbiAgICAgKiAgICAgfSk7XG4gICAgICogICB9XG4gICAgICpcbiAgICAgKiBOT1RFOiBUaGVyZSBpcyBubyBlcXVpdmFsZW50IGBjb21wb25lbnRXaWxsUmVjZWl2ZVN0YXRlYC4gQW4gaW5jb21pbmcgcHJvcFxuICAgICAqIHRyYW5zaXRpb24gbWF5IGNhdXNlIGEgc3RhdGUgY2hhbmdlLCBidXQgdGhlIG9wcG9zaXRlIGlzIG5vdCB0cnVlLiBJZiB5b3VcbiAgICAgKiBuZWVkIGl0LCB5b3UgYXJlIHByb2JhYmx5IGxvb2tpbmcgZm9yIGBjb21wb25lbnRXaWxsVXBkYXRlYC5cbiAgICAgKlxuICAgICAqIEBwYXJhbSB7b2JqZWN0fSBuZXh0UHJvcHNcbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBjb21wb25lbnRXaWxsUmVjZWl2ZVByb3BzOiAnREVGSU5FX01BTlknLFxuXG4gICAgLyoqXG4gICAgICogSW52b2tlZCB3aGlsZSBkZWNpZGluZyBpZiB0aGUgY29tcG9uZW50IHNob3VsZCBiZSB1cGRhdGVkIGFzIGEgcmVzdWx0IG9mXG4gICAgICogcmVjZWl2aW5nIG5ldyBwcm9wcywgc3RhdGUgYW5kL29yIGNvbnRleHQuXG4gICAgICpcbiAgICAgKiBVc2UgdGhpcyBhcyBhbiBvcHBvcnR1bml0eSB0byBgcmV0dXJuIGZhbHNlYCB3aGVuIHlvdSdyZSBjZXJ0YWluIHRoYXQgdGhlXG4gICAgICogdHJhbnNpdGlvbiB0byB0aGUgbmV3IHByb3BzL3N0YXRlL2NvbnRleHQgd2lsbCBub3QgcmVxdWlyZSBhIGNvbXBvbmVudFxuICAgICAqIHVwZGF0ZS5cbiAgICAgKlxuICAgICAqICAgc2hvdWxkQ29tcG9uZW50VXBkYXRlOiBmdW5jdGlvbihuZXh0UHJvcHMsIG5leHRTdGF0ZSwgbmV4dENvbnRleHQpIHtcbiAgICAgKiAgICAgcmV0dXJuICFlcXVhbChuZXh0UHJvcHMsIHRoaXMucHJvcHMpIHx8XG4gICAgICogICAgICAgIWVxdWFsKG5leHRTdGF0ZSwgdGhpcy5zdGF0ZSkgfHxcbiAgICAgKiAgICAgICAhZXF1YWwobmV4dENvbnRleHQsIHRoaXMuY29udGV4dCk7XG4gICAgICogICB9XG4gICAgICpcbiAgICAgKiBAcGFyYW0ge29iamVjdH0gbmV4dFByb3BzXG4gICAgICogQHBhcmFtIHs/b2JqZWN0fSBuZXh0U3RhdGVcbiAgICAgKiBAcGFyYW0gez9vYmplY3R9IG5leHRDb250ZXh0XG4gICAgICogQHJldHVybiB7Ym9vbGVhbn0gVHJ1ZSBpZiB0aGUgY29tcG9uZW50IHNob3VsZCB1cGRhdGUuXG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgc2hvdWxkQ29tcG9uZW50VXBkYXRlOiAnREVGSU5FX09OQ0UnLFxuXG4gICAgLyoqXG4gICAgICogSW52b2tlZCB3aGVuIHRoZSBjb21wb25lbnQgaXMgYWJvdXQgdG8gdXBkYXRlIGR1ZSB0byBhIHRyYW5zaXRpb24gZnJvbVxuICAgICAqIGB0aGlzLnByb3BzYCwgYHRoaXMuc3RhdGVgIGFuZCBgdGhpcy5jb250ZXh0YCB0byBgbmV4dFByb3BzYCwgYG5leHRTdGF0ZWBcbiAgICAgKiBhbmQgYG5leHRDb250ZXh0YC5cbiAgICAgKlxuICAgICAqIFVzZSB0aGlzIGFzIGFuIG9wcG9ydHVuaXR5IHRvIHBlcmZvcm0gcHJlcGFyYXRpb24gYmVmb3JlIGFuIHVwZGF0ZSBvY2N1cnMuXG4gICAgICpcbiAgICAgKiBOT1RFOiBZb3UgKipjYW5ub3QqKiB1c2UgYHRoaXMuc2V0U3RhdGUoKWAgaW4gdGhpcyBtZXRob2QuXG4gICAgICpcbiAgICAgKiBAcGFyYW0ge29iamVjdH0gbmV4dFByb3BzXG4gICAgICogQHBhcmFtIHs/b2JqZWN0fSBuZXh0U3RhdGVcbiAgICAgKiBAcGFyYW0gez9vYmplY3R9IG5leHRDb250ZXh0XG4gICAgICogQHBhcmFtIHtSZWFjdFJlY29uY2lsZVRyYW5zYWN0aW9ufSB0cmFuc2FjdGlvblxuICAgICAqIEBvcHRpb25hbFxuICAgICAqL1xuICAgIGNvbXBvbmVudFdpbGxVcGRhdGU6ICdERUZJTkVfTUFOWScsXG5cbiAgICAvKipcbiAgICAgKiBJbnZva2VkIHdoZW4gdGhlIGNvbXBvbmVudCdzIERPTSByZXByZXNlbnRhdGlvbiBoYXMgYmVlbiB1cGRhdGVkLlxuICAgICAqXG4gICAgICogVXNlIHRoaXMgYXMgYW4gb3Bwb3J0dW5pdHkgdG8gb3BlcmF0ZSBvbiB0aGUgRE9NIHdoZW4gdGhlIGNvbXBvbmVudCBoYXNcbiAgICAgKiBiZWVuIHVwZGF0ZWQuXG4gICAgICpcbiAgICAgKiBAcGFyYW0ge29iamVjdH0gcHJldlByb3BzXG4gICAgICogQHBhcmFtIHs/b2JqZWN0fSBwcmV2U3RhdGVcbiAgICAgKiBAcGFyYW0gez9vYmplY3R9IHByZXZDb250ZXh0XG4gICAgICogQHBhcmFtIHtET01FbGVtZW50fSByb290Tm9kZSBET00gZWxlbWVudCByZXByZXNlbnRpbmcgdGhlIGNvbXBvbmVudC5cbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBjb21wb25lbnREaWRVcGRhdGU6ICdERUZJTkVfTUFOWScsXG5cbiAgICAvKipcbiAgICAgKiBJbnZva2VkIHdoZW4gdGhlIGNvbXBvbmVudCBpcyBhYm91dCB0byBiZSByZW1vdmVkIGZyb20gaXRzIHBhcmVudCBhbmQgaGF2ZVxuICAgICAqIGl0cyBET00gcmVwcmVzZW50YXRpb24gZGVzdHJveWVkLlxuICAgICAqXG4gICAgICogVXNlIHRoaXMgYXMgYW4gb3Bwb3J0dW5pdHkgdG8gZGVhbGxvY2F0ZSBhbnkgZXh0ZXJuYWwgcmVzb3VyY2VzLlxuICAgICAqXG4gICAgICogTk9URTogVGhlcmUgaXMgbm8gYGNvbXBvbmVudERpZFVubW91bnRgIHNpbmNlIHlvdXIgY29tcG9uZW50IHdpbGwgaGF2ZSBiZWVuXG4gICAgICogZGVzdHJveWVkIGJ5IHRoYXQgcG9pbnQuXG4gICAgICpcbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBjb21wb25lbnRXaWxsVW5tb3VudDogJ0RFRklORV9NQU5ZJyxcblxuICAgIC8qKlxuICAgICAqIFJlcGxhY2VtZW50IGZvciAoZGVwcmVjYXRlZCkgYGNvbXBvbmVudFdpbGxNb3VudGAuXG4gICAgICpcbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBVTlNBRkVfY29tcG9uZW50V2lsbE1vdW50OiAnREVGSU5FX01BTlknLFxuXG4gICAgLyoqXG4gICAgICogUmVwbGFjZW1lbnQgZm9yIChkZXByZWNhdGVkKSBgY29tcG9uZW50V2lsbFJlY2VpdmVQcm9wc2AuXG4gICAgICpcbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBVTlNBRkVfY29tcG9uZW50V2lsbFJlY2VpdmVQcm9wczogJ0RFRklORV9NQU5ZJyxcblxuICAgIC8qKlxuICAgICAqIFJlcGxhY2VtZW50IGZvciAoZGVwcmVjYXRlZCkgYGNvbXBvbmVudFdpbGxVcGRhdGVgLlxuICAgICAqXG4gICAgICogQG9wdGlvbmFsXG4gICAgICovXG4gICAgVU5TQUZFX2NvbXBvbmVudFdpbGxVcGRhdGU6ICdERUZJTkVfTUFOWScsXG5cbiAgICAvLyA9PT09IEFkdmFuY2VkIG1ldGhvZHMgPT09PVxuXG4gICAgLyoqXG4gICAgICogVXBkYXRlcyB0aGUgY29tcG9uZW50J3MgY3VycmVudGx5IG1vdW50ZWQgRE9NIHJlcHJlc2VudGF0aW9uLlxuICAgICAqXG4gICAgICogQnkgZGVmYXVsdCwgdGhpcyBpbXBsZW1lbnRzIFJlYWN0J3MgcmVuZGVyaW5nIGFuZCByZWNvbmNpbGlhdGlvbiBhbGdvcml0aG0uXG4gICAgICogU29waGlzdGljYXRlZCBjbGllbnRzIG1heSB3aXNoIHRvIG92ZXJyaWRlIHRoaXMuXG4gICAgICpcbiAgICAgKiBAcGFyYW0ge1JlYWN0UmVjb25jaWxlVHJhbnNhY3Rpb259IHRyYW5zYWN0aW9uXG4gICAgICogQGludGVybmFsXG4gICAgICogQG92ZXJyaWRhYmxlXG4gICAgICovXG4gICAgdXBkYXRlQ29tcG9uZW50OiAnT1ZFUlJJREVfQkFTRSdcbiAgfTtcblxuICAvKipcbiAgICogU2ltaWxhciB0byBSZWFjdENsYXNzSW50ZXJmYWNlIGJ1dCBmb3Igc3RhdGljIG1ldGhvZHMuXG4gICAqL1xuICB2YXIgUmVhY3RDbGFzc1N0YXRpY0ludGVyZmFjZSA9IHtcbiAgICAvKipcbiAgICAgKiBUaGlzIG1ldGhvZCBpcyBpbnZva2VkIGFmdGVyIGEgY29tcG9uZW50IGlzIGluc3RhbnRpYXRlZCBhbmQgd2hlbiBpdFxuICAgICAqIHJlY2VpdmVzIG5ldyBwcm9wcy4gUmV0dXJuIGFuIG9iamVjdCB0byB1cGRhdGUgc3RhdGUgaW4gcmVzcG9uc2UgdG9cbiAgICAgKiBwcm9wIGNoYW5nZXMuIFJldHVybiBudWxsIHRvIGluZGljYXRlIG5vIGNoYW5nZSB0byBzdGF0ZS5cbiAgICAgKlxuICAgICAqIElmIGFuIG9iamVjdCBpcyByZXR1cm5lZCwgaXRzIGtleXMgd2lsbCBiZSBtZXJnZWQgaW50byB0aGUgZXhpc3Rpbmcgc3RhdGUuXG4gICAgICpcbiAgICAgKiBAcmV0dXJuIHtvYmplY3QgfHwgbnVsbH1cbiAgICAgKiBAb3B0aW9uYWxcbiAgICAgKi9cbiAgICBnZXREZXJpdmVkU3RhdGVGcm9tUHJvcHM6ICdERUZJTkVfTUFOWV9NRVJHRUQnXG4gIH07XG5cbiAgLyoqXG4gICAqIE1hcHBpbmcgZnJvbSBjbGFzcyBzcGVjaWZpY2F0aW9uIGtleXMgdG8gc3BlY2lhbCBwcm9jZXNzaW5nIGZ1bmN0aW9ucy5cbiAgICpcbiAgICogQWx0aG91Z2ggdGhlc2UgYXJlIGRlY2xhcmVkIGxpa2UgaW5zdGFuY2UgcHJvcGVydGllcyBpbiB0aGUgc3BlY2lmaWNhdGlvblxuICAgKiB3aGVuIGRlZmluaW5nIGNsYXNzZXMgdXNpbmcgYFJlYWN0LmNyZWF0ZUNsYXNzYCwgdGhleSBhcmUgYWN0dWFsbHkgc3RhdGljXG4gICAqIGFuZCBhcmUgYWNjZXNzaWJsZSBvbiB0aGUgY29uc3RydWN0b3IgaW5zdGVhZCBvZiB0aGUgcHJvdG90eXBlLiBEZXNwaXRlXG4gICAqIGJlaW5nIHN0YXRpYywgdGhleSBtdXN0IGJlIGRlZmluZWQgb3V0c2lkZSBvZiB0aGUgXCJzdGF0aWNzXCIga2V5IHVuZGVyXG4gICAqIHdoaWNoIGFsbCBvdGhlciBzdGF0aWMgbWV0aG9kcyBhcmUgZGVmaW5lZC5cbiAgICovXG4gIHZhciBSRVNFUlZFRF9TUEVDX0tFWVMgPSB7XG4gICAgZGlzcGxheU5hbWU6IGZ1bmN0aW9uKENvbnN0cnVjdG9yLCBkaXNwbGF5TmFtZSkge1xuICAgICAgQ29uc3RydWN0b3IuZGlzcGxheU5hbWUgPSBkaXNwbGF5TmFtZTtcbiAgICB9LFxuICAgIG1peGluczogZnVuY3Rpb24oQ29uc3RydWN0b3IsIG1peGlucykge1xuICAgICAgaWYgKG1peGlucykge1xuICAgICAgICBmb3IgKHZhciBpID0gMDsgaSA8IG1peGlucy5sZW5ndGg7IGkrKykge1xuICAgICAgICAgIG1peFNwZWNJbnRvQ29tcG9uZW50KENvbnN0cnVjdG9yLCBtaXhpbnNbaV0pO1xuICAgICAgICB9XG4gICAgICB9XG4gICAgfSxcbiAgICBjaGlsZENvbnRleHRUeXBlczogZnVuY3Rpb24oQ29uc3RydWN0b3IsIGNoaWxkQ29udGV4dFR5cGVzKSB7XG4gICAgICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgICAgICB2YWxpZGF0ZVR5cGVEZWYoQ29uc3RydWN0b3IsIGNoaWxkQ29udGV4dFR5cGVzLCAnY2hpbGRDb250ZXh0Jyk7XG4gICAgICB9XG4gICAgICBDb25zdHJ1Y3Rvci5jaGlsZENvbnRleHRUeXBlcyA9IF9hc3NpZ24oXG4gICAgICAgIHt9LFxuICAgICAgICBDb25zdHJ1Y3Rvci5jaGlsZENvbnRleHRUeXBlcyxcbiAgICAgICAgY2hpbGRDb250ZXh0VHlwZXNcbiAgICAgICk7XG4gICAgfSxcbiAgICBjb250ZXh0VHlwZXM6IGZ1bmN0aW9uKENvbnN0cnVjdG9yLCBjb250ZXh0VHlwZXMpIHtcbiAgICAgIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgICAgIHZhbGlkYXRlVHlwZURlZihDb25zdHJ1Y3RvciwgY29udGV4dFR5cGVzLCAnY29udGV4dCcpO1xuICAgICAgfVxuICAgICAgQ29uc3RydWN0b3IuY29udGV4dFR5cGVzID0gX2Fzc2lnbihcbiAgICAgICAge30sXG4gICAgICAgIENvbnN0cnVjdG9yLmNvbnRleHRUeXBlcyxcbiAgICAgICAgY29udGV4dFR5cGVzXG4gICAgICApO1xuICAgIH0sXG4gICAgLyoqXG4gICAgICogU3BlY2lhbCBjYXNlIGdldERlZmF1bHRQcm9wcyB3aGljaCBzaG91bGQgbW92ZSBpbnRvIHN0YXRpY3MgYnV0IHJlcXVpcmVzXG4gICAgICogYXV0b21hdGljIG1lcmdpbmcuXG4gICAgICovXG4gICAgZ2V0RGVmYXVsdFByb3BzOiBmdW5jdGlvbihDb25zdHJ1Y3RvciwgZ2V0RGVmYXVsdFByb3BzKSB7XG4gICAgICBpZiAoQ29uc3RydWN0b3IuZ2V0RGVmYXVsdFByb3BzKSB7XG4gICAgICAgIENvbnN0cnVjdG9yLmdldERlZmF1bHRQcm9wcyA9IGNyZWF0ZU1lcmdlZFJlc3VsdEZ1bmN0aW9uKFxuICAgICAgICAgIENvbnN0cnVjdG9yLmdldERlZmF1bHRQcm9wcyxcbiAgICAgICAgICBnZXREZWZhdWx0UHJvcHNcbiAgICAgICAgKTtcbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIENvbnN0cnVjdG9yLmdldERlZmF1bHRQcm9wcyA9IGdldERlZmF1bHRQcm9wcztcbiAgICAgIH1cbiAgICB9LFxuICAgIHByb3BUeXBlczogZnVuY3Rpb24oQ29uc3RydWN0b3IsIHByb3BUeXBlcykge1xuICAgICAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICAgICAgdmFsaWRhdGVUeXBlRGVmKENvbnN0cnVjdG9yLCBwcm9wVHlwZXMsICdwcm9wJyk7XG4gICAgICB9XG4gICAgICBDb25zdHJ1Y3Rvci5wcm9wVHlwZXMgPSBfYXNzaWduKHt9LCBDb25zdHJ1Y3Rvci5wcm9wVHlwZXMsIHByb3BUeXBlcyk7XG4gICAgfSxcbiAgICBzdGF0aWNzOiBmdW5jdGlvbihDb25zdHJ1Y3Rvciwgc3RhdGljcykge1xuICAgICAgbWl4U3RhdGljU3BlY0ludG9Db21wb25lbnQoQ29uc3RydWN0b3IsIHN0YXRpY3MpO1xuICAgIH0sXG4gICAgYXV0b2JpbmQ6IGZ1bmN0aW9uKCkge31cbiAgfTtcblxuICBmdW5jdGlvbiB2YWxpZGF0ZVR5cGVEZWYoQ29uc3RydWN0b3IsIHR5cGVEZWYsIGxvY2F0aW9uKSB7XG4gICAgZm9yICh2YXIgcHJvcE5hbWUgaW4gdHlwZURlZikge1xuICAgICAgaWYgKHR5cGVEZWYuaGFzT3duUHJvcGVydHkocHJvcE5hbWUpKSB7XG4gICAgICAgIC8vIHVzZSBhIHdhcm5pbmcgaW5zdGVhZCBvZiBhbiBfaW52YXJpYW50IHNvIGNvbXBvbmVudHNcbiAgICAgICAgLy8gZG9uJ3Qgc2hvdyB1cCBpbiBwcm9kIGJ1dCBvbmx5IGluIF9fREVWX19cbiAgICAgICAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICAgICAgICB3YXJuaW5nKFxuICAgICAgICAgICAgdHlwZW9mIHR5cGVEZWZbcHJvcE5hbWVdID09PSAnZnVuY3Rpb24nLFxuICAgICAgICAgICAgJyVzOiAlcyB0eXBlIGAlc2AgaXMgaW52YWxpZDsgaXQgbXVzdCBiZSBhIGZ1bmN0aW9uLCB1c3VhbGx5IGZyb20gJyArXG4gICAgICAgICAgICAgICdSZWFjdC5Qcm9wVHlwZXMuJyxcbiAgICAgICAgICAgIENvbnN0cnVjdG9yLmRpc3BsYXlOYW1lIHx8ICdSZWFjdENsYXNzJyxcbiAgICAgICAgICAgIFJlYWN0UHJvcFR5cGVMb2NhdGlvbk5hbWVzW2xvY2F0aW9uXSxcbiAgICAgICAgICAgIHByb3BOYW1lXG4gICAgICAgICAgKTtcbiAgICAgICAgfVxuICAgICAgfVxuICAgIH1cbiAgfVxuXG4gIGZ1bmN0aW9uIHZhbGlkYXRlTWV0aG9kT3ZlcnJpZGUoaXNBbHJlYWR5RGVmaW5lZCwgbmFtZSkge1xuICAgIHZhciBzcGVjUG9saWN5ID0gUmVhY3RDbGFzc0ludGVyZmFjZS5oYXNPd25Qcm9wZXJ0eShuYW1lKVxuICAgICAgPyBSZWFjdENsYXNzSW50ZXJmYWNlW25hbWVdXG4gICAgICA6IG51bGw7XG5cbiAgICAvLyBEaXNhbGxvdyBvdmVycmlkaW5nIG9mIGJhc2UgY2xhc3MgbWV0aG9kcyB1bmxlc3MgZXhwbGljaXRseSBhbGxvd2VkLlxuICAgIGlmIChSZWFjdENsYXNzTWl4aW4uaGFzT3duUHJvcGVydHkobmFtZSkpIHtcbiAgICAgIF9pbnZhcmlhbnQoXG4gICAgICAgIHNwZWNQb2xpY3kgPT09ICdPVkVSUklERV9CQVNFJyxcbiAgICAgICAgJ1JlYWN0Q2xhc3NJbnRlcmZhY2U6IFlvdSBhcmUgYXR0ZW1wdGluZyB0byBvdmVycmlkZSAnICtcbiAgICAgICAgICAnYCVzYCBmcm9tIHlvdXIgY2xhc3Mgc3BlY2lmaWNhdGlvbi4gRW5zdXJlIHRoYXQgeW91ciBtZXRob2QgbmFtZXMgJyArXG4gICAgICAgICAgJ2RvIG5vdCBvdmVybGFwIHdpdGggUmVhY3QgbWV0aG9kcy4nLFxuICAgICAgICBuYW1lXG4gICAgICApO1xuICAgIH1cblxuICAgIC8vIERpc2FsbG93IGRlZmluaW5nIG1ldGhvZHMgbW9yZSB0aGFuIG9uY2UgdW5sZXNzIGV4cGxpY2l0bHkgYWxsb3dlZC5cbiAgICBpZiAoaXNBbHJlYWR5RGVmaW5lZCkge1xuICAgICAgX2ludmFyaWFudChcbiAgICAgICAgc3BlY1BvbGljeSA9PT0gJ0RFRklORV9NQU5ZJyB8fCBzcGVjUG9saWN5ID09PSAnREVGSU5FX01BTllfTUVSR0VEJyxcbiAgICAgICAgJ1JlYWN0Q2xhc3NJbnRlcmZhY2U6IFlvdSBhcmUgYXR0ZW1wdGluZyB0byBkZWZpbmUgJyArXG4gICAgICAgICAgJ2Alc2Agb24geW91ciBjb21wb25lbnQgbW9yZSB0aGFuIG9uY2UuIFRoaXMgY29uZmxpY3QgbWF5IGJlIGR1ZSAnICtcbiAgICAgICAgICAndG8gYSBtaXhpbi4nLFxuICAgICAgICBuYW1lXG4gICAgICApO1xuICAgIH1cbiAgfVxuXG4gIC8qKlxuICAgKiBNaXhpbiBoZWxwZXIgd2hpY2ggaGFuZGxlcyBwb2xpY3kgdmFsaWRhdGlvbiBhbmQgcmVzZXJ2ZWRcbiAgICogc3BlY2lmaWNhdGlvbiBrZXlzIHdoZW4gYnVpbGRpbmcgUmVhY3QgY2xhc3Nlcy5cbiAgICovXG4gIGZ1bmN0aW9uIG1peFNwZWNJbnRvQ29tcG9uZW50KENvbnN0cnVjdG9yLCBzcGVjKSB7XG4gICAgaWYgKCFzcGVjKSB7XG4gICAgICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgICAgICB2YXIgdHlwZW9mU3BlYyA9IHR5cGVvZiBzcGVjO1xuICAgICAgICB2YXIgaXNNaXhpblZhbGlkID0gdHlwZW9mU3BlYyA9PT0gJ29iamVjdCcgJiYgc3BlYyAhPT0gbnVsbDtcblxuICAgICAgICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgICAgICAgIHdhcm5pbmcoXG4gICAgICAgICAgICBpc01peGluVmFsaWQsXG4gICAgICAgICAgICBcIiVzOiBZb3UncmUgYXR0ZW1wdGluZyB0byBpbmNsdWRlIGEgbWl4aW4gdGhhdCBpcyBlaXRoZXIgbnVsbCBcIiArXG4gICAgICAgICAgICAgICdvciBub3QgYW4gb2JqZWN0LiBDaGVjayB0aGUgbWl4aW5zIGluY2x1ZGVkIGJ5IHRoZSBjb21wb25lbnQsICcgK1xuICAgICAgICAgICAgICAnYXMgd2VsbCBhcyBhbnkgbWl4aW5zIHRoZXkgaW5jbHVkZSB0aGVtc2VsdmVzLiAnICtcbiAgICAgICAgICAgICAgJ0V4cGVjdGVkIG9iamVjdCBidXQgZ290ICVzLicsXG4gICAgICAgICAgICBDb25zdHJ1Y3Rvci5kaXNwbGF5TmFtZSB8fCAnUmVhY3RDbGFzcycsXG4gICAgICAgICAgICBzcGVjID09PSBudWxsID8gbnVsbCA6IHR5cGVvZlNwZWNcbiAgICAgICAgICApO1xuICAgICAgICB9XG4gICAgICB9XG5cbiAgICAgIHJldHVybjtcbiAgICB9XG5cbiAgICBfaW52YXJpYW50KFxuICAgICAgdHlwZW9mIHNwZWMgIT09ICdmdW5jdGlvbicsXG4gICAgICBcIlJlYWN0Q2xhc3M6IFlvdSdyZSBhdHRlbXB0aW5nIHRvIFwiICtcbiAgICAgICAgJ3VzZSBhIGNvbXBvbmVudCBjbGFzcyBvciBmdW5jdGlvbiBhcyBhIG1peGluLiBJbnN0ZWFkLCBqdXN0IHVzZSBhICcgK1xuICAgICAgICAncmVndWxhciBvYmplY3QuJ1xuICAgICk7XG4gICAgX2ludmFyaWFudChcbiAgICAgICFpc1ZhbGlkRWxlbWVudChzcGVjKSxcbiAgICAgIFwiUmVhY3RDbGFzczogWW91J3JlIGF0dGVtcHRpbmcgdG8gXCIgK1xuICAgICAgICAndXNlIGEgY29tcG9uZW50IGFzIGEgbWl4aW4uIEluc3RlYWQsIGp1c3QgdXNlIGEgcmVndWxhciBvYmplY3QuJ1xuICAgICk7XG5cbiAgICB2YXIgcHJvdG8gPSBDb25zdHJ1Y3Rvci5wcm90b3R5cGU7XG4gICAgdmFyIGF1dG9CaW5kUGFpcnMgPSBwcm90by5fX3JlYWN0QXV0b0JpbmRQYWlycztcblxuICAgIC8vIEJ5IGhhbmRsaW5nIG1peGlucyBiZWZvcmUgYW55IG90aGVyIHByb3BlcnRpZXMsIHdlIGVuc3VyZSB0aGUgc2FtZVxuICAgIC8vIGNoYWluaW5nIG9yZGVyIGlzIGFwcGxpZWQgdG8gbWV0aG9kcyB3aXRoIERFRklORV9NQU5ZIHBvbGljeSwgd2hldGhlclxuICAgIC8vIG1peGlucyBhcmUgbGlzdGVkIGJlZm9yZSBvciBhZnRlciB0aGVzZSBtZXRob2RzIGluIHRoZSBzcGVjLlxuICAgIGlmIChzcGVjLmhhc093blByb3BlcnR5KE1JWElOU19LRVkpKSB7XG4gICAgICBSRVNFUlZFRF9TUEVDX0tFWVMubWl4aW5zKENvbnN0cnVjdG9yLCBzcGVjLm1peGlucyk7XG4gICAgfVxuXG4gICAgZm9yICh2YXIgbmFtZSBpbiBzcGVjKSB7XG4gICAgICBpZiAoIXNwZWMuaGFzT3duUHJvcGVydHkobmFtZSkpIHtcbiAgICAgICAgY29udGludWU7XG4gICAgICB9XG5cbiAgICAgIGlmIChuYW1lID09PSBNSVhJTlNfS0VZKSB7XG4gICAgICAgIC8vIFdlIGhhdmUgYWxyZWFkeSBoYW5kbGVkIG1peGlucyBpbiBhIHNwZWNpYWwgY2FzZSBhYm92ZS5cbiAgICAgICAgY29udGludWU7XG4gICAgICB9XG5cbiAgICAgIHZhciBwcm9wZXJ0eSA9IHNwZWNbbmFtZV07XG4gICAgICB2YXIgaXNBbHJlYWR5RGVmaW5lZCA9IHByb3RvLmhhc093blByb3BlcnR5KG5hbWUpO1xuICAgICAgdmFsaWRhdGVNZXRob2RPdmVycmlkZShpc0FscmVhZHlEZWZpbmVkLCBuYW1lKTtcblxuICAgICAgaWYgKFJFU0VSVkVEX1NQRUNfS0VZUy5oYXNPd25Qcm9wZXJ0eShuYW1lKSkge1xuICAgICAgICBSRVNFUlZFRF9TUEVDX0tFWVNbbmFtZV0oQ29uc3RydWN0b3IsIHByb3BlcnR5KTtcbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIC8vIFNldHVwIG1ldGhvZHMgb24gcHJvdG90eXBlOlxuICAgICAgICAvLyBUaGUgZm9sbG93aW5nIG1lbWJlciBtZXRob2RzIHNob3VsZCBub3QgYmUgYXV0b21hdGljYWxseSBib3VuZDpcbiAgICAgICAgLy8gMS4gRXhwZWN0ZWQgUmVhY3RDbGFzcyBtZXRob2RzIChpbiB0aGUgXCJpbnRlcmZhY2VcIikuXG4gICAgICAgIC8vIDIuIE92ZXJyaWRkZW4gbWV0aG9kcyAodGhhdCB3ZXJlIG1peGVkIGluKS5cbiAgICAgICAgdmFyIGlzUmVhY3RDbGFzc01ldGhvZCA9IFJlYWN0Q2xhc3NJbnRlcmZhY2UuaGFzT3duUHJvcGVydHkobmFtZSk7XG4gICAgICAgIHZhciBpc0Z1bmN0aW9uID0gdHlwZW9mIHByb3BlcnR5ID09PSAnZnVuY3Rpb24nO1xuICAgICAgICB2YXIgc2hvdWxkQXV0b0JpbmQgPVxuICAgICAgICAgIGlzRnVuY3Rpb24gJiZcbiAgICAgICAgICAhaXNSZWFjdENsYXNzTWV0aG9kICYmXG4gICAgICAgICAgIWlzQWxyZWFkeURlZmluZWQgJiZcbiAgICAgICAgICBzcGVjLmF1dG9iaW5kICE9PSBmYWxzZTtcblxuICAgICAgICBpZiAoc2hvdWxkQXV0b0JpbmQpIHtcbiAgICAgICAgICBhdXRvQmluZFBhaXJzLnB1c2gobmFtZSwgcHJvcGVydHkpO1xuICAgICAgICAgIHByb3RvW25hbWVdID0gcHJvcGVydHk7XG4gICAgICAgIH0gZWxzZSB7XG4gICAgICAgICAgaWYgKGlzQWxyZWFkeURlZmluZWQpIHtcbiAgICAgICAgICAgIHZhciBzcGVjUG9saWN5ID0gUmVhY3RDbGFzc0ludGVyZmFjZVtuYW1lXTtcblxuICAgICAgICAgICAgLy8gVGhlc2UgY2FzZXMgc2hvdWxkIGFscmVhZHkgYmUgY2F1Z2h0IGJ5IHZhbGlkYXRlTWV0aG9kT3ZlcnJpZGUuXG4gICAgICAgICAgICBfaW52YXJpYW50KFxuICAgICAgICAgICAgICBpc1JlYWN0Q2xhc3NNZXRob2QgJiZcbiAgICAgICAgICAgICAgICAoc3BlY1BvbGljeSA9PT0gJ0RFRklORV9NQU5ZX01FUkdFRCcgfHxcbiAgICAgICAgICAgICAgICAgIHNwZWNQb2xpY3kgPT09ICdERUZJTkVfTUFOWScpLFxuICAgICAgICAgICAgICAnUmVhY3RDbGFzczogVW5leHBlY3RlZCBzcGVjIHBvbGljeSAlcyBmb3Iga2V5ICVzICcgK1xuICAgICAgICAgICAgICAgICd3aGVuIG1peGluZyBpbiBjb21wb25lbnQgc3BlY3MuJyxcbiAgICAgICAgICAgICAgc3BlY1BvbGljeSxcbiAgICAgICAgICAgICAgbmFtZVxuICAgICAgICAgICAgKTtcblxuICAgICAgICAgICAgLy8gRm9yIG1ldGhvZHMgd2hpY2ggYXJlIGRlZmluZWQgbW9yZSB0aGFuIG9uY2UsIGNhbGwgdGhlIGV4aXN0aW5nXG4gICAgICAgICAgICAvLyBtZXRob2RzIGJlZm9yZSBjYWxsaW5nIHRoZSBuZXcgcHJvcGVydHksIG1lcmdpbmcgaWYgYXBwcm9wcmlhdGUuXG4gICAgICAgICAgICBpZiAoc3BlY1BvbGljeSA9PT0gJ0RFRklORV9NQU5ZX01FUkdFRCcpIHtcbiAgICAgICAgICAgICAgcHJvdG9bbmFtZV0gPSBjcmVhdGVNZXJnZWRSZXN1bHRGdW5jdGlvbihwcm90b1tuYW1lXSwgcHJvcGVydHkpO1xuICAgICAgICAgICAgfSBlbHNlIGlmIChzcGVjUG9saWN5ID09PSAnREVGSU5FX01BTlknKSB7XG4gICAgICAgICAgICAgIHByb3RvW25hbWVdID0gY3JlYXRlQ2hhaW5lZEZ1bmN0aW9uKHByb3RvW25hbWVdLCBwcm9wZXJ0eSk7XG4gICAgICAgICAgICB9XG4gICAgICAgICAgfSBlbHNlIHtcbiAgICAgICAgICAgIHByb3RvW25hbWVdID0gcHJvcGVydHk7XG4gICAgICAgICAgICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgICAgICAgICAgICAvLyBBZGQgdmVyYm9zZSBkaXNwbGF5TmFtZSB0byB0aGUgZnVuY3Rpb24sIHdoaWNoIGhlbHBzIHdoZW4gbG9va2luZ1xuICAgICAgICAgICAgICAvLyBhdCBwcm9maWxpbmcgdG9vbHMuXG4gICAgICAgICAgICAgIGlmICh0eXBlb2YgcHJvcGVydHkgPT09ICdmdW5jdGlvbicgJiYgc3BlYy5kaXNwbGF5TmFtZSkge1xuICAgICAgICAgICAgICAgIHByb3RvW25hbWVdLmRpc3BsYXlOYW1lID0gc3BlYy5kaXNwbGF5TmFtZSArICdfJyArIG5hbWU7XG4gICAgICAgICAgICAgIH1cbiAgICAgICAgICAgIH1cbiAgICAgICAgICB9XG4gICAgICAgIH1cbiAgICAgIH1cbiAgICB9XG4gIH1cblxuICBmdW5jdGlvbiBtaXhTdGF0aWNTcGVjSW50b0NvbXBvbmVudChDb25zdHJ1Y3Rvciwgc3RhdGljcykge1xuICAgIGlmICghc3RhdGljcykge1xuICAgICAgcmV0dXJuO1xuICAgIH1cblxuICAgIGZvciAodmFyIG5hbWUgaW4gc3RhdGljcykge1xuICAgICAgdmFyIHByb3BlcnR5ID0gc3RhdGljc1tuYW1lXTtcbiAgICAgIGlmICghc3RhdGljcy5oYXNPd25Qcm9wZXJ0eShuYW1lKSkge1xuICAgICAgICBjb250aW51ZTtcbiAgICAgIH1cblxuICAgICAgdmFyIGlzUmVzZXJ2ZWQgPSBuYW1lIGluIFJFU0VSVkVEX1NQRUNfS0VZUztcbiAgICAgIF9pbnZhcmlhbnQoXG4gICAgICAgICFpc1Jlc2VydmVkLFxuICAgICAgICAnUmVhY3RDbGFzczogWW91IGFyZSBhdHRlbXB0aW5nIHRvIGRlZmluZSBhIHJlc2VydmVkICcgK1xuICAgICAgICAgICdwcm9wZXJ0eSwgYCVzYCwgdGhhdCBzaG91bGRuXFwndCBiZSBvbiB0aGUgXCJzdGF0aWNzXCIga2V5LiBEZWZpbmUgaXQgJyArXG4gICAgICAgICAgJ2FzIGFuIGluc3RhbmNlIHByb3BlcnR5IGluc3RlYWQ7IGl0IHdpbGwgc3RpbGwgYmUgYWNjZXNzaWJsZSBvbiB0aGUgJyArXG4gICAgICAgICAgJ2NvbnN0cnVjdG9yLicsXG4gICAgICAgIG5hbWVcbiAgICAgICk7XG5cbiAgICAgIHZhciBpc0FscmVhZHlEZWZpbmVkID0gbmFtZSBpbiBDb25zdHJ1Y3RvcjtcbiAgICAgIGlmIChpc0FscmVhZHlEZWZpbmVkKSB7XG4gICAgICAgIHZhciBzcGVjUG9saWN5ID0gUmVhY3RDbGFzc1N0YXRpY0ludGVyZmFjZS5oYXNPd25Qcm9wZXJ0eShuYW1lKVxuICAgICAgICAgID8gUmVhY3RDbGFzc1N0YXRpY0ludGVyZmFjZVtuYW1lXVxuICAgICAgICAgIDogbnVsbDtcblxuICAgICAgICBfaW52YXJpYW50KFxuICAgICAgICAgIHNwZWNQb2xpY3kgPT09ICdERUZJTkVfTUFOWV9NRVJHRUQnLFxuICAgICAgICAgICdSZWFjdENsYXNzOiBZb3UgYXJlIGF0dGVtcHRpbmcgdG8gZGVmaW5lICcgK1xuICAgICAgICAgICAgJ2Alc2Agb24geW91ciBjb21wb25lbnQgbW9yZSB0aGFuIG9uY2UuIFRoaXMgY29uZmxpY3QgbWF5IGJlICcgK1xuICAgICAgICAgICAgJ2R1ZSB0byBhIG1peGluLicsXG4gICAgICAgICAgbmFtZVxuICAgICAgICApO1xuXG4gICAgICAgIENvbnN0cnVjdG9yW25hbWVdID0gY3JlYXRlTWVyZ2VkUmVzdWx0RnVuY3Rpb24oQ29uc3RydWN0b3JbbmFtZV0sIHByb3BlcnR5KTtcblxuICAgICAgICByZXR1cm47XG4gICAgICB9XG5cbiAgICAgIENvbnN0cnVjdG9yW25hbWVdID0gcHJvcGVydHk7XG4gICAgfVxuICB9XG5cbiAgLyoqXG4gICAqIE1lcmdlIHR3byBvYmplY3RzLCBidXQgdGhyb3cgaWYgYm90aCBjb250YWluIHRoZSBzYW1lIGtleS5cbiAgICpcbiAgICogQHBhcmFtIHtvYmplY3R9IG9uZSBUaGUgZmlyc3Qgb2JqZWN0LCB3aGljaCBpcyBtdXRhdGVkLlxuICAgKiBAcGFyYW0ge29iamVjdH0gdHdvIFRoZSBzZWNvbmQgb2JqZWN0XG4gICAqIEByZXR1cm4ge29iamVjdH0gb25lIGFmdGVyIGl0IGhhcyBiZWVuIG11dGF0ZWQgdG8gY29udGFpbiBldmVyeXRoaW5nIGluIHR3by5cbiAgICovXG4gIGZ1bmN0aW9uIG1lcmdlSW50b1dpdGhOb0R1cGxpY2F0ZUtleXMob25lLCB0d28pIHtcbiAgICBfaW52YXJpYW50KFxuICAgICAgb25lICYmIHR3byAmJiB0eXBlb2Ygb25lID09PSAnb2JqZWN0JyAmJiB0eXBlb2YgdHdvID09PSAnb2JqZWN0JyxcbiAgICAgICdtZXJnZUludG9XaXRoTm9EdXBsaWNhdGVLZXlzKCk6IENhbm5vdCBtZXJnZSBub24tb2JqZWN0cy4nXG4gICAgKTtcblxuICAgIGZvciAodmFyIGtleSBpbiB0d28pIHtcbiAgICAgIGlmICh0d28uaGFzT3duUHJvcGVydHkoa2V5KSkge1xuICAgICAgICBfaW52YXJpYW50KFxuICAgICAgICAgIG9uZVtrZXldID09PSB1bmRlZmluZWQsXG4gICAgICAgICAgJ21lcmdlSW50b1dpdGhOb0R1cGxpY2F0ZUtleXMoKTogJyArXG4gICAgICAgICAgICAnVHJpZWQgdG8gbWVyZ2UgdHdvIG9iamVjdHMgd2l0aCB0aGUgc2FtZSBrZXk6IGAlc2AuIFRoaXMgY29uZmxpY3QgJyArXG4gICAgICAgICAgICAnbWF5IGJlIGR1ZSB0byBhIG1peGluOyBpbiBwYXJ0aWN1bGFyLCB0aGlzIG1heSBiZSBjYXVzZWQgYnkgdHdvICcgK1xuICAgICAgICAgICAgJ2dldEluaXRpYWxTdGF0ZSgpIG9yIGdldERlZmF1bHRQcm9wcygpIG1ldGhvZHMgcmV0dXJuaW5nIG9iamVjdHMgJyArXG4gICAgICAgICAgICAnd2l0aCBjbGFzaGluZyBrZXlzLicsXG4gICAgICAgICAga2V5XG4gICAgICAgICk7XG4gICAgICAgIG9uZVtrZXldID0gdHdvW2tleV07XG4gICAgICB9XG4gICAgfVxuICAgIHJldHVybiBvbmU7XG4gIH1cblxuICAvKipcbiAgICogQ3JlYXRlcyBhIGZ1bmN0aW9uIHRoYXQgaW52b2tlcyB0d28gZnVuY3Rpb25zIGFuZCBtZXJnZXMgdGhlaXIgcmV0dXJuIHZhbHVlcy5cbiAgICpcbiAgICogQHBhcmFtIHtmdW5jdGlvbn0gb25lIEZ1bmN0aW9uIHRvIGludm9rZSBmaXJzdC5cbiAgICogQHBhcmFtIHtmdW5jdGlvbn0gdHdvIEZ1bmN0aW9uIHRvIGludm9rZSBzZWNvbmQuXG4gICAqIEByZXR1cm4ge2Z1bmN0aW9ufSBGdW5jdGlvbiB0aGF0IGludm9rZXMgdGhlIHR3byBhcmd1bWVudCBmdW5jdGlvbnMuXG4gICAqIEBwcml2YXRlXG4gICAqL1xuICBmdW5jdGlvbiBjcmVhdGVNZXJnZWRSZXN1bHRGdW5jdGlvbihvbmUsIHR3bykge1xuICAgIHJldHVybiBmdW5jdGlvbiBtZXJnZWRSZXN1bHQoKSB7XG4gICAgICB2YXIgYSA9IG9uZS5hcHBseSh0aGlzLCBhcmd1bWVudHMpO1xuICAgICAgdmFyIGIgPSB0d28uYXBwbHkodGhpcywgYXJndW1lbnRzKTtcbiAgICAgIGlmIChhID09IG51bGwpIHtcbiAgICAgICAgcmV0dXJuIGI7XG4gICAgICB9IGVsc2UgaWYgKGIgPT0gbnVsbCkge1xuICAgICAgICByZXR1cm4gYTtcbiAgICAgIH1cbiAgICAgIHZhciBjID0ge307XG4gICAgICBtZXJnZUludG9XaXRoTm9EdXBsaWNhdGVLZXlzKGMsIGEpO1xuICAgICAgbWVyZ2VJbnRvV2l0aE5vRHVwbGljYXRlS2V5cyhjLCBiKTtcbiAgICAgIHJldHVybiBjO1xuICAgIH07XG4gIH1cblxuICAvKipcbiAgICogQ3JlYXRlcyBhIGZ1bmN0aW9uIHRoYXQgaW52b2tlcyB0d28gZnVuY3Rpb25zIGFuZCBpZ25vcmVzIHRoZWlyIHJldHVybiB2YWxlcy5cbiAgICpcbiAgICogQHBhcmFtIHtmdW5jdGlvbn0gb25lIEZ1bmN0aW9uIHRvIGludm9rZSBmaXJzdC5cbiAgICogQHBhcmFtIHtmdW5jdGlvbn0gdHdvIEZ1bmN0aW9uIHRvIGludm9rZSBzZWNvbmQuXG4gICAqIEByZXR1cm4ge2Z1bmN0aW9ufSBGdW5jdGlvbiB0aGF0IGludm9rZXMgdGhlIHR3byBhcmd1bWVudCBmdW5jdGlvbnMuXG4gICAqIEBwcml2YXRlXG4gICAqL1xuICBmdW5jdGlvbiBjcmVhdGVDaGFpbmVkRnVuY3Rpb24ob25lLCB0d28pIHtcbiAgICByZXR1cm4gZnVuY3Rpb24gY2hhaW5lZEZ1bmN0aW9uKCkge1xuICAgICAgb25lLmFwcGx5KHRoaXMsIGFyZ3VtZW50cyk7XG4gICAgICB0d28uYXBwbHkodGhpcywgYXJndW1lbnRzKTtcbiAgICB9O1xuICB9XG5cbiAgLyoqXG4gICAqIEJpbmRzIGEgbWV0aG9kIHRvIHRoZSBjb21wb25lbnQuXG4gICAqXG4gICAqIEBwYXJhbSB7b2JqZWN0fSBjb21wb25lbnQgQ29tcG9uZW50IHdob3NlIG1ldGhvZCBpcyBnb2luZyB0byBiZSBib3VuZC5cbiAgICogQHBhcmFtIHtmdW5jdGlvbn0gbWV0aG9kIE1ldGhvZCB0byBiZSBib3VuZC5cbiAgICogQHJldHVybiB7ZnVuY3Rpb259IFRoZSBib3VuZCBtZXRob2QuXG4gICAqL1xuICBmdW5jdGlvbiBiaW5kQXV0b0JpbmRNZXRob2QoY29tcG9uZW50LCBtZXRob2QpIHtcbiAgICB2YXIgYm91bmRNZXRob2QgPSBtZXRob2QuYmluZChjb21wb25lbnQpO1xuICAgIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgICBib3VuZE1ldGhvZC5fX3JlYWN0Qm91bmRDb250ZXh0ID0gY29tcG9uZW50O1xuICAgICAgYm91bmRNZXRob2QuX19yZWFjdEJvdW5kTWV0aG9kID0gbWV0aG9kO1xuICAgICAgYm91bmRNZXRob2QuX19yZWFjdEJvdW5kQXJndW1lbnRzID0gbnVsbDtcbiAgICAgIHZhciBjb21wb25lbnROYW1lID0gY29tcG9uZW50LmNvbnN0cnVjdG9yLmRpc3BsYXlOYW1lO1xuICAgICAgdmFyIF9iaW5kID0gYm91bmRNZXRob2QuYmluZDtcbiAgICAgIGJvdW5kTWV0aG9kLmJpbmQgPSBmdW5jdGlvbihuZXdUaGlzKSB7XG4gICAgICAgIGZvciAoXG4gICAgICAgICAgdmFyIF9sZW4gPSBhcmd1bWVudHMubGVuZ3RoLFxuICAgICAgICAgICAgYXJncyA9IEFycmF5KF9sZW4gPiAxID8gX2xlbiAtIDEgOiAwKSxcbiAgICAgICAgICAgIF9rZXkgPSAxO1xuICAgICAgICAgIF9rZXkgPCBfbGVuO1xuICAgICAgICAgIF9rZXkrK1xuICAgICAgICApIHtcbiAgICAgICAgICBhcmdzW19rZXkgLSAxXSA9IGFyZ3VtZW50c1tfa2V5XTtcbiAgICAgICAgfVxuXG4gICAgICAgIC8vIFVzZXIgaXMgdHJ5aW5nIHRvIGJpbmQoKSBhbiBhdXRvYm91bmQgbWV0aG9kOyB3ZSBlZmZlY3RpdmVseSB3aWxsXG4gICAgICAgIC8vIGlnbm9yZSB0aGUgdmFsdWUgb2YgXCJ0aGlzXCIgdGhhdCB0aGUgdXNlciBpcyB0cnlpbmcgdG8gdXNlLCBzb1xuICAgICAgICAvLyBsZXQncyB3YXJuLlxuICAgICAgICBpZiAobmV3VGhpcyAhPT0gY29tcG9uZW50ICYmIG5ld1RoaXMgIT09IG51bGwpIHtcbiAgICAgICAgICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgICAgICAgICAgd2FybmluZyhcbiAgICAgICAgICAgICAgZmFsc2UsXG4gICAgICAgICAgICAgICdiaW5kKCk6IFJlYWN0IGNvbXBvbmVudCBtZXRob2RzIG1heSBvbmx5IGJlIGJvdW5kIHRvIHRoZSAnICtcbiAgICAgICAgICAgICAgICAnY29tcG9uZW50IGluc3RhbmNlLiBTZWUgJXMnLFxuICAgICAgICAgICAgICBjb21wb25lbnROYW1lXG4gICAgICAgICAgICApO1xuICAgICAgICAgIH1cbiAgICAgICAgfSBlbHNlIGlmICghYXJncy5sZW5ndGgpIHtcbiAgICAgICAgICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgICAgICAgICAgd2FybmluZyhcbiAgICAgICAgICAgICAgZmFsc2UsXG4gICAgICAgICAgICAgICdiaW5kKCk6IFlvdSBhcmUgYmluZGluZyBhIGNvbXBvbmVudCBtZXRob2QgdG8gdGhlIGNvbXBvbmVudC4gJyArXG4gICAgICAgICAgICAgICAgJ1JlYWN0IGRvZXMgdGhpcyBmb3IgeW91IGF1dG9tYXRpY2FsbHkgaW4gYSBoaWdoLXBlcmZvcm1hbmNlICcgK1xuICAgICAgICAgICAgICAgICd3YXksIHNvIHlvdSBjYW4gc2FmZWx5IHJlbW92ZSB0aGlzIGNhbGwuIFNlZSAlcycsXG4gICAgICAgICAgICAgIGNvbXBvbmVudE5hbWVcbiAgICAgICAgICAgICk7XG4gICAgICAgICAgfVxuICAgICAgICAgIHJldHVybiBib3VuZE1ldGhvZDtcbiAgICAgICAgfVxuICAgICAgICB2YXIgcmVib3VuZE1ldGhvZCA9IF9iaW5kLmFwcGx5KGJvdW5kTWV0aG9kLCBhcmd1bWVudHMpO1xuICAgICAgICByZWJvdW5kTWV0aG9kLl9fcmVhY3RCb3VuZENvbnRleHQgPSBjb21wb25lbnQ7XG4gICAgICAgIHJlYm91bmRNZXRob2QuX19yZWFjdEJvdW5kTWV0aG9kID0gbWV0aG9kO1xuICAgICAgICByZWJvdW5kTWV0aG9kLl9fcmVhY3RCb3VuZEFyZ3VtZW50cyA9IGFyZ3M7XG4gICAgICAgIHJldHVybiByZWJvdW5kTWV0aG9kO1xuICAgICAgfTtcbiAgICB9XG4gICAgcmV0dXJuIGJvdW5kTWV0aG9kO1xuICB9XG5cbiAgLyoqXG4gICAqIEJpbmRzIGFsbCBhdXRvLWJvdW5kIG1ldGhvZHMgaW4gYSBjb21wb25lbnQuXG4gICAqXG4gICAqIEBwYXJhbSB7b2JqZWN0fSBjb21wb25lbnQgQ29tcG9uZW50IHdob3NlIG1ldGhvZCBpcyBnb2luZyB0byBiZSBib3VuZC5cbiAgICovXG4gIGZ1bmN0aW9uIGJpbmRBdXRvQmluZE1ldGhvZHMoY29tcG9uZW50KSB7XG4gICAgdmFyIHBhaXJzID0gY29tcG9uZW50Ll9fcmVhY3RBdXRvQmluZFBhaXJzO1xuICAgIGZvciAodmFyIGkgPSAwOyBpIDwgcGFpcnMubGVuZ3RoOyBpICs9IDIpIHtcbiAgICAgIHZhciBhdXRvQmluZEtleSA9IHBhaXJzW2ldO1xuICAgICAgdmFyIG1ldGhvZCA9IHBhaXJzW2kgKyAxXTtcbiAgICAgIGNvbXBvbmVudFthdXRvQmluZEtleV0gPSBiaW5kQXV0b0JpbmRNZXRob2QoY29tcG9uZW50LCBtZXRob2QpO1xuICAgIH1cbiAgfVxuXG4gIHZhciBJc01vdW50ZWRQcmVNaXhpbiA9IHtcbiAgICBjb21wb25lbnREaWRNb3VudDogZnVuY3Rpb24oKSB7XG4gICAgICB0aGlzLl9faXNNb3VudGVkID0gdHJ1ZTtcbiAgICB9XG4gIH07XG5cbiAgdmFyIElzTW91bnRlZFBvc3RNaXhpbiA9IHtcbiAgICBjb21wb25lbnRXaWxsVW5tb3VudDogZnVuY3Rpb24oKSB7XG4gICAgICB0aGlzLl9faXNNb3VudGVkID0gZmFsc2U7XG4gICAgfVxuICB9O1xuXG4gIC8qKlxuICAgKiBBZGQgbW9yZSB0byB0aGUgUmVhY3RDbGFzcyBiYXNlIGNsYXNzLiBUaGVzZSBhcmUgYWxsIGxlZ2FjeSBmZWF0dXJlcyBhbmRcbiAgICogdGhlcmVmb3JlIG5vdCBhbHJlYWR5IHBhcnQgb2YgdGhlIG1vZGVybiBSZWFjdENvbXBvbmVudC5cbiAgICovXG4gIHZhciBSZWFjdENsYXNzTWl4aW4gPSB7XG4gICAgLyoqXG4gICAgICogVE9ETzogVGhpcyB3aWxsIGJlIGRlcHJlY2F0ZWQgYmVjYXVzZSBzdGF0ZSBzaG91bGQgYWx3YXlzIGtlZXAgYSBjb25zaXN0ZW50XG4gICAgICogdHlwZSBzaWduYXR1cmUgYW5kIHRoZSBvbmx5IHVzZSBjYXNlIGZvciB0aGlzLCBpcyB0byBhdm9pZCB0aGF0LlxuICAgICAqL1xuICAgIHJlcGxhY2VTdGF0ZTogZnVuY3Rpb24obmV3U3RhdGUsIGNhbGxiYWNrKSB7XG4gICAgICB0aGlzLnVwZGF0ZXIuZW5xdWV1ZVJlcGxhY2VTdGF0ZSh0aGlzLCBuZXdTdGF0ZSwgY2FsbGJhY2spO1xuICAgIH0sXG5cbiAgICAvKipcbiAgICAgKiBDaGVja3Mgd2hldGhlciBvciBub3QgdGhpcyBjb21wb3NpdGUgY29tcG9uZW50IGlzIG1vdW50ZWQuXG4gICAgICogQHJldHVybiB7Ym9vbGVhbn0gVHJ1ZSBpZiBtb3VudGVkLCBmYWxzZSBvdGhlcndpc2UuXG4gICAgICogQHByb3RlY3RlZFxuICAgICAqIEBmaW5hbFxuICAgICAqL1xuICAgIGlzTW91bnRlZDogZnVuY3Rpb24oKSB7XG4gICAgICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgICAgICB3YXJuaW5nKFxuICAgICAgICAgIHRoaXMuX19kaWRXYXJuSXNNb3VudGVkLFxuICAgICAgICAgICclczogaXNNb3VudGVkIGlzIGRlcHJlY2F0ZWQuIEluc3RlYWQsIG1ha2Ugc3VyZSB0byBjbGVhbiB1cCAnICtcbiAgICAgICAgICAgICdzdWJzY3JpcHRpb25zIGFuZCBwZW5kaW5nIHJlcXVlc3RzIGluIGNvbXBvbmVudFdpbGxVbm1vdW50IHRvICcgK1xuICAgICAgICAgICAgJ3ByZXZlbnQgbWVtb3J5IGxlYWtzLicsXG4gICAgICAgICAgKHRoaXMuY29uc3RydWN0b3IgJiYgdGhpcy5jb25zdHJ1Y3Rvci5kaXNwbGF5TmFtZSkgfHxcbiAgICAgICAgICAgIHRoaXMubmFtZSB8fFxuICAgICAgICAgICAgJ0NvbXBvbmVudCdcbiAgICAgICAgKTtcbiAgICAgICAgdGhpcy5fX2RpZFdhcm5Jc01vdW50ZWQgPSB0cnVlO1xuICAgICAgfVxuICAgICAgcmV0dXJuICEhdGhpcy5fX2lzTW91bnRlZDtcbiAgICB9XG4gIH07XG5cbiAgdmFyIFJlYWN0Q2xhc3NDb21wb25lbnQgPSBmdW5jdGlvbigpIHt9O1xuICBfYXNzaWduKFxuICAgIFJlYWN0Q2xhc3NDb21wb25lbnQucHJvdG90eXBlLFxuICAgIFJlYWN0Q29tcG9uZW50LnByb3RvdHlwZSxcbiAgICBSZWFjdENsYXNzTWl4aW5cbiAgKTtcblxuICAvKipcbiAgICogQ3JlYXRlcyBhIGNvbXBvc2l0ZSBjb21wb25lbnQgY2xhc3MgZ2l2ZW4gYSBjbGFzcyBzcGVjaWZpY2F0aW9uLlxuICAgKiBTZWUgaHR0cHM6Ly9mYWNlYm9vay5naXRodWIuaW8vcmVhY3QvZG9jcy90b3AtbGV2ZWwtYXBpLmh0bWwjcmVhY3QuY3JlYXRlY2xhc3NcbiAgICpcbiAgICogQHBhcmFtIHtvYmplY3R9IHNwZWMgQ2xhc3Mgc3BlY2lmaWNhdGlvbiAod2hpY2ggbXVzdCBkZWZpbmUgYHJlbmRlcmApLlxuICAgKiBAcmV0dXJuIHtmdW5jdGlvbn0gQ29tcG9uZW50IGNvbnN0cnVjdG9yIGZ1bmN0aW9uLlxuICAgKiBAcHVibGljXG4gICAqL1xuICBmdW5jdGlvbiBjcmVhdGVDbGFzcyhzcGVjKSB7XG4gICAgLy8gVG8ga2VlcCBvdXIgd2FybmluZ3MgbW9yZSB1bmRlcnN0YW5kYWJsZSwgd2UnbGwgdXNlIGEgbGl0dGxlIGhhY2sgaGVyZSB0b1xuICAgIC8vIGVuc3VyZSB0aGF0IENvbnN0cnVjdG9yLm5hbWUgIT09ICdDb25zdHJ1Y3RvcicuIFRoaXMgbWFrZXMgc3VyZSB3ZSBkb24ndFxuICAgIC8vIHVubmVjZXNzYXJpbHkgaWRlbnRpZnkgYSBjbGFzcyB3aXRob3V0IGRpc3BsYXlOYW1lIGFzICdDb25zdHJ1Y3RvcicuXG4gICAgdmFyIENvbnN0cnVjdG9yID0gaWRlbnRpdHkoZnVuY3Rpb24ocHJvcHMsIGNvbnRleHQsIHVwZGF0ZXIpIHtcbiAgICAgIC8vIFRoaXMgY29uc3RydWN0b3IgZ2V0cyBvdmVycmlkZGVuIGJ5IG1vY2tzLiBUaGUgYXJndW1lbnQgaXMgdXNlZFxuICAgICAgLy8gYnkgbW9ja3MgdG8gYXNzZXJ0IG9uIHdoYXQgZ2V0cyBtb3VudGVkLlxuXG4gICAgICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgICAgICB3YXJuaW5nKFxuICAgICAgICAgIHRoaXMgaW5zdGFuY2VvZiBDb25zdHJ1Y3RvcixcbiAgICAgICAgICAnU29tZXRoaW5nIGlzIGNhbGxpbmcgYSBSZWFjdCBjb21wb25lbnQgZGlyZWN0bHkuIFVzZSBhIGZhY3Rvcnkgb3IgJyArXG4gICAgICAgICAgICAnSlNYIGluc3RlYWQuIFNlZTogaHR0cHM6Ly9mYi5tZS9yZWFjdC1sZWdhY3lmYWN0b3J5J1xuICAgICAgICApO1xuICAgICAgfVxuXG4gICAgICAvLyBXaXJlIHVwIGF1dG8tYmluZGluZ1xuICAgICAgaWYgKHRoaXMuX19yZWFjdEF1dG9CaW5kUGFpcnMubGVuZ3RoKSB7XG4gICAgICAgIGJpbmRBdXRvQmluZE1ldGhvZHModGhpcyk7XG4gICAgICB9XG5cbiAgICAgIHRoaXMucHJvcHMgPSBwcm9wcztcbiAgICAgIHRoaXMuY29udGV4dCA9IGNvbnRleHQ7XG4gICAgICB0aGlzLnJlZnMgPSBlbXB0eU9iamVjdDtcbiAgICAgIHRoaXMudXBkYXRlciA9IHVwZGF0ZXIgfHwgUmVhY3ROb29wVXBkYXRlUXVldWU7XG5cbiAgICAgIHRoaXMuc3RhdGUgPSBudWxsO1xuXG4gICAgICAvLyBSZWFjdENsYXNzZXMgZG9lc24ndCBoYXZlIGNvbnN0cnVjdG9ycy4gSW5zdGVhZCwgdGhleSB1c2UgdGhlXG4gICAgICAvLyBnZXRJbml0aWFsU3RhdGUgYW5kIGNvbXBvbmVudFdpbGxNb3VudCBtZXRob2RzIGZvciBpbml0aWFsaXphdGlvbi5cblxuICAgICAgdmFyIGluaXRpYWxTdGF0ZSA9IHRoaXMuZ2V0SW5pdGlhbFN0YXRlID8gdGhpcy5nZXRJbml0aWFsU3RhdGUoKSA6IG51bGw7XG4gICAgICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgICAgICAvLyBXZSBhbGxvdyBhdXRvLW1vY2tzIHRvIHByb2NlZWQgYXMgaWYgdGhleSdyZSByZXR1cm5pbmcgbnVsbC5cbiAgICAgICAgaWYgKFxuICAgICAgICAgIGluaXRpYWxTdGF0ZSA9PT0gdW5kZWZpbmVkICYmXG4gICAgICAgICAgdGhpcy5nZXRJbml0aWFsU3RhdGUuX2lzTW9ja0Z1bmN0aW9uXG4gICAgICAgICkge1xuICAgICAgICAgIC8vIFRoaXMgaXMgcHJvYmFibHkgYmFkIHByYWN0aWNlLiBDb25zaWRlciB3YXJuaW5nIGhlcmUgYW5kXG4gICAgICAgICAgLy8gZGVwcmVjYXRpbmcgdGhpcyBjb252ZW5pZW5jZS5cbiAgICAgICAgICBpbml0aWFsU3RhdGUgPSBudWxsO1xuICAgICAgICB9XG4gICAgICB9XG4gICAgICBfaW52YXJpYW50KFxuICAgICAgICB0eXBlb2YgaW5pdGlhbFN0YXRlID09PSAnb2JqZWN0JyAmJiAhQXJyYXkuaXNBcnJheShpbml0aWFsU3RhdGUpLFxuICAgICAgICAnJXMuZ2V0SW5pdGlhbFN0YXRlKCk6IG11c3QgcmV0dXJuIGFuIG9iamVjdCBvciBudWxsJyxcbiAgICAgICAgQ29uc3RydWN0b3IuZGlzcGxheU5hbWUgfHwgJ1JlYWN0Q29tcG9zaXRlQ29tcG9uZW50J1xuICAgICAgKTtcblxuICAgICAgdGhpcy5zdGF0ZSA9IGluaXRpYWxTdGF0ZTtcbiAgICB9KTtcbiAgICBDb25zdHJ1Y3Rvci5wcm90b3R5cGUgPSBuZXcgUmVhY3RDbGFzc0NvbXBvbmVudCgpO1xuICAgIENvbnN0cnVjdG9yLnByb3RvdHlwZS5jb25zdHJ1Y3RvciA9IENvbnN0cnVjdG9yO1xuICAgIENvbnN0cnVjdG9yLnByb3RvdHlwZS5fX3JlYWN0QXV0b0JpbmRQYWlycyA9IFtdO1xuXG4gICAgaW5qZWN0ZWRNaXhpbnMuZm9yRWFjaChtaXhTcGVjSW50b0NvbXBvbmVudC5iaW5kKG51bGwsIENvbnN0cnVjdG9yKSk7XG5cbiAgICBtaXhTcGVjSW50b0NvbXBvbmVudChDb25zdHJ1Y3RvciwgSXNNb3VudGVkUHJlTWl4aW4pO1xuICAgIG1peFNwZWNJbnRvQ29tcG9uZW50KENvbnN0cnVjdG9yLCBzcGVjKTtcbiAgICBtaXhTcGVjSW50b0NvbXBvbmVudChDb25zdHJ1Y3RvciwgSXNNb3VudGVkUG9zdE1peGluKTtcblxuICAgIC8vIEluaXRpYWxpemUgdGhlIGRlZmF1bHRQcm9wcyBwcm9wZXJ0eSBhZnRlciBhbGwgbWl4aW5zIGhhdmUgYmVlbiBtZXJnZWQuXG4gICAgaWYgKENvbnN0cnVjdG9yLmdldERlZmF1bHRQcm9wcykge1xuICAgICAgQ29uc3RydWN0b3IuZGVmYXVsdFByb3BzID0gQ29uc3RydWN0b3IuZ2V0RGVmYXVsdFByb3BzKCk7XG4gICAgfVxuXG4gICAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICAgIC8vIFRoaXMgaXMgYSB0YWcgdG8gaW5kaWNhdGUgdGhhdCB0aGUgdXNlIG9mIHRoZXNlIG1ldGhvZCBuYW1lcyBpcyBvayxcbiAgICAgIC8vIHNpbmNlIGl0J3MgdXNlZCB3aXRoIGNyZWF0ZUNsYXNzLiBJZiBpdCdzIG5vdCwgdGhlbiBpdCdzIGxpa2VseSBhXG4gICAgICAvLyBtaXN0YWtlIHNvIHdlJ2xsIHdhcm4geW91IHRvIHVzZSB0aGUgc3RhdGljIHByb3BlcnR5LCBwcm9wZXJ0eVxuICAgICAgLy8gaW5pdGlhbGl6ZXIgb3IgY29uc3RydWN0b3IgcmVzcGVjdGl2ZWx5LlxuICAgICAgaWYgKENvbnN0cnVjdG9yLmdldERlZmF1bHRQcm9wcykge1xuICAgICAgICBDb25zdHJ1Y3Rvci5nZXREZWZhdWx0UHJvcHMuaXNSZWFjdENsYXNzQXBwcm92ZWQgPSB7fTtcbiAgICAgIH1cbiAgICAgIGlmIChDb25zdHJ1Y3Rvci5wcm90b3R5cGUuZ2V0SW5pdGlhbFN0YXRlKSB7XG4gICAgICAgIENvbnN0cnVjdG9yLnByb3RvdHlwZS5nZXRJbml0aWFsU3RhdGUuaXNSZWFjdENsYXNzQXBwcm92ZWQgPSB7fTtcbiAgICAgIH1cbiAgICB9XG5cbiAgICBfaW52YXJpYW50KFxuICAgICAgQ29uc3RydWN0b3IucHJvdG90eXBlLnJlbmRlcixcbiAgICAgICdjcmVhdGVDbGFzcyguLi4pOiBDbGFzcyBzcGVjaWZpY2F0aW9uIG11c3QgaW1wbGVtZW50IGEgYHJlbmRlcmAgbWV0aG9kLidcbiAgICApO1xuXG4gICAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICAgIHdhcm5pbmcoXG4gICAgICAgICFDb25zdHJ1Y3Rvci5wcm90b3R5cGUuY29tcG9uZW50U2hvdWxkVXBkYXRlLFxuICAgICAgICAnJXMgaGFzIGEgbWV0aG9kIGNhbGxlZCAnICtcbiAgICAgICAgICAnY29tcG9uZW50U2hvdWxkVXBkYXRlKCkuIERpZCB5b3UgbWVhbiBzaG91bGRDb21wb25lbnRVcGRhdGUoKT8gJyArXG4gICAgICAgICAgJ1RoZSBuYW1lIGlzIHBocmFzZWQgYXMgYSBxdWVzdGlvbiBiZWNhdXNlIHRoZSBmdW5jdGlvbiBpcyAnICtcbiAgICAgICAgICAnZXhwZWN0ZWQgdG8gcmV0dXJuIGEgdmFsdWUuJyxcbiAgICAgICAgc3BlYy5kaXNwbGF5TmFtZSB8fCAnQSBjb21wb25lbnQnXG4gICAgICApO1xuICAgICAgd2FybmluZyhcbiAgICAgICAgIUNvbnN0cnVjdG9yLnByb3RvdHlwZS5jb21wb25lbnRXaWxsUmVjaWV2ZVByb3BzLFxuICAgICAgICAnJXMgaGFzIGEgbWV0aG9kIGNhbGxlZCAnICtcbiAgICAgICAgICAnY29tcG9uZW50V2lsbFJlY2lldmVQcm9wcygpLiBEaWQgeW91IG1lYW4gY29tcG9uZW50V2lsbFJlY2VpdmVQcm9wcygpPycsXG4gICAgICAgIHNwZWMuZGlzcGxheU5hbWUgfHwgJ0EgY29tcG9uZW50J1xuICAgICAgKTtcbiAgICAgIHdhcm5pbmcoXG4gICAgICAgICFDb25zdHJ1Y3Rvci5wcm90b3R5cGUuVU5TQUZFX2NvbXBvbmVudFdpbGxSZWNpZXZlUHJvcHMsXG4gICAgICAgICclcyBoYXMgYSBtZXRob2QgY2FsbGVkIFVOU0FGRV9jb21wb25lbnRXaWxsUmVjaWV2ZVByb3BzKCkuICcgK1xuICAgICAgICAgICdEaWQgeW91IG1lYW4gVU5TQUZFX2NvbXBvbmVudFdpbGxSZWNlaXZlUHJvcHMoKT8nLFxuICAgICAgICBzcGVjLmRpc3BsYXlOYW1lIHx8ICdBIGNvbXBvbmVudCdcbiAgICAgICk7XG4gICAgfVxuXG4gICAgLy8gUmVkdWNlIHRpbWUgc3BlbnQgZG9pbmcgbG9va3VwcyBieSBzZXR0aW5nIHRoZXNlIG9uIHRoZSBwcm90b3R5cGUuXG4gICAgZm9yICh2YXIgbWV0aG9kTmFtZSBpbiBSZWFjdENsYXNzSW50ZXJmYWNlKSB7XG4gICAgICBpZiAoIUNvbnN0cnVjdG9yLnByb3RvdHlwZVttZXRob2ROYW1lXSkge1xuICAgICAgICBDb25zdHJ1Y3Rvci5wcm90b3R5cGVbbWV0aG9kTmFtZV0gPSBudWxsO1xuICAgICAgfVxuICAgIH1cblxuICAgIHJldHVybiBDb25zdHJ1Y3RvcjtcbiAgfVxuXG4gIHJldHVybiBjcmVhdGVDbGFzcztcbn1cblxubW9kdWxlLmV4cG9ydHMgPSBmYWN0b3J5O1xuIiwiLyoqXG4gKiBDb3B5cmlnaHQgKGMpIDIwMTMtcHJlc2VudCwgRmFjZWJvb2ssIEluYy5cbiAqXG4gKiBUaGlzIHNvdXJjZSBjb2RlIGlzIGxpY2Vuc2VkIHVuZGVyIHRoZSBNSVQgbGljZW5zZSBmb3VuZCBpbiB0aGVcbiAqIExJQ0VOU0UgZmlsZSBpbiB0aGUgcm9vdCBkaXJlY3Rvcnkgb2YgdGhpcyBzb3VyY2UgdHJlZS5cbiAqXG4gKi9cblxuJ3VzZSBzdHJpY3QnO1xuXG52YXIgUmVhY3QgPSByZXF1aXJlKCdyZWFjdCcpO1xudmFyIGZhY3RvcnkgPSByZXF1aXJlKCcuL2ZhY3RvcnknKTtcblxuaWYgKHR5cGVvZiBSZWFjdCA9PT0gJ3VuZGVmaW5lZCcpIHtcbiAgdGhyb3cgRXJyb3IoXG4gICAgJ2NyZWF0ZS1yZWFjdC1jbGFzcyBjb3VsZCBub3QgZmluZCB0aGUgUmVhY3Qgb2JqZWN0LiBJZiB5b3UgYXJlIHVzaW5nIHNjcmlwdCB0YWdzLCAnICtcbiAgICAgICdtYWtlIHN1cmUgdGhhdCBSZWFjdCBpcyBiZWluZyBsb2FkZWQgYmVmb3JlIGNyZWF0ZS1yZWFjdC1jbGFzcy4nXG4gICk7XG59XG5cbi8vIEhhY2sgdG8gZ3JhYiBOb29wVXBkYXRlUXVldWUgZnJvbSBpc29tb3JwaGljIFJlYWN0XG52YXIgUmVhY3ROb29wVXBkYXRlUXVldWUgPSBuZXcgUmVhY3QuQ29tcG9uZW50KCkudXBkYXRlcjtcblxubW9kdWxlLmV4cG9ydHMgPSBmYWN0b3J5KFxuICBSZWFjdC5Db21wb25lbnQsXG4gIFJlYWN0LmlzVmFsaWRFbGVtZW50LFxuICBSZWFjdE5vb3BVcGRhdGVRdWV1ZVxuKTtcbiIsImV4cG9ydHMgPSBtb2R1bGUuZXhwb3J0cyA9IHJlcXVpcmUoXCIuLi8uLi9jc3MtbG9hZGVyL2Rpc3QvcnVudGltZS9hcGkuanNcIikoZmFsc2UpO1xuLy8gTW9kdWxlXG5leHBvcnRzLnB1c2goW21vZHVsZS5pZCwgXCIvKiFcXG4gKiBodHRwczovL2dpdGh1Yi5jb20vWW91Q2FuQm9va01lL3JlYWN0LWRhdGV0aW1lXFxuICovXFxuXFxuLnJkdCB7XFxuICBwb3NpdGlvbjogcmVsYXRpdmU7XFxufVxcbi5yZHRQaWNrZXIge1xcbiAgZGlzcGxheTogbm9uZTtcXG4gIHBvc2l0aW9uOiBhYnNvbHV0ZTtcXG4gIHdpZHRoOiAyNTBweDtcXG4gIHBhZGRpbmc6IDRweDtcXG4gIG1hcmdpbi10b3A6IDFweDtcXG4gIHotaW5kZXg6IDk5OTk5ICFpbXBvcnRhbnQ7XFxuICBiYWNrZ3JvdW5kOiAjZmZmO1xcbiAgYm94LXNoYWRvdzogMCAxcHggM3B4IHJnYmEoMCwwLDAsLjEpO1xcbiAgYm9yZGVyOiAxcHggc29saWQgI2Y5ZjlmOTtcXG59XFxuLnJkdE9wZW4gLnJkdFBpY2tlciB7XFxuICBkaXNwbGF5OiBibG9jaztcXG59XFxuLnJkdFN0YXRpYyAucmR0UGlja2VyIHtcXG4gIGJveC1zaGFkb3c6IG5vbmU7XFxuICBwb3NpdGlvbjogc3RhdGljO1xcbn1cXG5cXG4ucmR0UGlja2VyIC5yZHRUaW1lVG9nZ2xlIHtcXG4gIHRleHQtYWxpZ246IGNlbnRlcjtcXG59XFxuXFxuLnJkdFBpY2tlciB0YWJsZSB7XFxuICB3aWR0aDogMTAwJTtcXG4gIG1hcmdpbjogMDtcXG59XFxuLnJkdFBpY2tlciB0ZCxcXG4ucmR0UGlja2VyIHRoIHtcXG4gIHRleHQtYWxpZ246IGNlbnRlcjtcXG4gIGhlaWdodDogMjhweDtcXG59XFxuLnJkdFBpY2tlciB0ZCB7XFxuICBjdXJzb3I6IHBvaW50ZXI7XFxufVxcbi5yZHRQaWNrZXIgdGQucmR0RGF5OmhvdmVyLFxcbi5yZHRQaWNrZXIgdGQucmR0SG91cjpob3ZlcixcXG4ucmR0UGlja2VyIHRkLnJkdE1pbnV0ZTpob3ZlcixcXG4ucmR0UGlja2VyIHRkLnJkdFNlY29uZDpob3ZlcixcXG4ucmR0UGlja2VyIC5yZHRUaW1lVG9nZ2xlOmhvdmVyIHtcXG4gIGJhY2tncm91bmQ6ICNlZWVlZWU7XFxuICBjdXJzb3I6IHBvaW50ZXI7XFxufVxcbi5yZHRQaWNrZXIgdGQucmR0T2xkLFxcbi5yZHRQaWNrZXIgdGQucmR0TmV3IHtcXG4gIGNvbG9yOiAjOTk5OTk5O1xcbn1cXG4ucmR0UGlja2VyIHRkLnJkdFRvZGF5IHtcXG4gIHBvc2l0aW9uOiByZWxhdGl2ZTtcXG59XFxuLnJkdFBpY2tlciB0ZC5yZHRUb2RheTpiZWZvcmUge1xcbiAgY29udGVudDogJyc7XFxuICBkaXNwbGF5OiBpbmxpbmUtYmxvY2s7XFxuICBib3JkZXItbGVmdDogN3B4IHNvbGlkIHRyYW5zcGFyZW50O1xcbiAgYm9yZGVyLWJvdHRvbTogN3B4IHNvbGlkICM0MjhiY2E7XFxuICBib3JkZXItdG9wLWNvbG9yOiByZ2JhKDAsIDAsIDAsIDAuMik7XFxuICBwb3NpdGlvbjogYWJzb2x1dGU7XFxuICBib3R0b206IDRweDtcXG4gIHJpZ2h0OiA0cHg7XFxufVxcbi5yZHRQaWNrZXIgdGQucmR0QWN0aXZlLFxcbi5yZHRQaWNrZXIgdGQucmR0QWN0aXZlOmhvdmVyIHtcXG4gIGJhY2tncm91bmQtY29sb3I6ICM0MjhiY2E7XFxuICBjb2xvcjogI2ZmZjtcXG4gIHRleHQtc2hhZG93OiAwIC0xcHggMCByZ2JhKDAsIDAsIDAsIDAuMjUpO1xcbn1cXG4ucmR0UGlja2VyIHRkLnJkdEFjdGl2ZS5yZHRUb2RheTpiZWZvcmUge1xcbiAgYm9yZGVyLWJvdHRvbS1jb2xvcjogI2ZmZjtcXG59XFxuLnJkdFBpY2tlciB0ZC5yZHREaXNhYmxlZCxcXG4ucmR0UGlja2VyIHRkLnJkdERpc2FibGVkOmhvdmVyIHtcXG4gIGJhY2tncm91bmQ6IG5vbmU7XFxuICBjb2xvcjogIzk5OTk5OTtcXG4gIGN1cnNvcjogbm90LWFsbG93ZWQ7XFxufVxcblxcbi5yZHRQaWNrZXIgdGQgc3Bhbi5yZHRPbGQge1xcbiAgY29sb3I6ICM5OTk5OTk7XFxufVxcbi5yZHRQaWNrZXIgdGQgc3Bhbi5yZHREaXNhYmxlZCxcXG4ucmR0UGlja2VyIHRkIHNwYW4ucmR0RGlzYWJsZWQ6aG92ZXIge1xcbiAgYmFja2dyb3VuZDogbm9uZTtcXG4gIGNvbG9yOiAjOTk5OTk5O1xcbiAgY3Vyc29yOiBub3QtYWxsb3dlZDtcXG59XFxuLnJkdFBpY2tlciB0aCB7XFxuICBib3JkZXItYm90dG9tOiAxcHggc29saWQgI2Y5ZjlmOTtcXG59XFxuLnJkdFBpY2tlciAuZG93IHtcXG4gIHdpZHRoOiAxNC4yODU3JTtcXG4gIGJvcmRlci1ib3R0b206IG5vbmU7XFxuICBjdXJzb3I6IGRlZmF1bHQ7XFxufVxcbi5yZHRQaWNrZXIgdGgucmR0U3dpdGNoIHtcXG4gIHdpZHRoOiAxMDBweDtcXG59XFxuLnJkdFBpY2tlciB0aC5yZHROZXh0LFxcbi5yZHRQaWNrZXIgdGgucmR0UHJldiB7XFxuICBmb250LXNpemU6IDIxcHg7XFxuICB2ZXJ0aWNhbC1hbGlnbjogdG9wO1xcbn1cXG5cXG4ucmR0UHJldiBzcGFuLFxcbi5yZHROZXh0IHNwYW4ge1xcbiAgZGlzcGxheTogYmxvY2s7XFxuICAtd2Via2l0LXRvdWNoLWNhbGxvdXQ6IG5vbmU7IC8qIGlPUyBTYWZhcmkgKi9cXG4gIC13ZWJraXQtdXNlci1zZWxlY3Q6IG5vbmU7ICAgLyogQ2hyb21lL1NhZmFyaS9PcGVyYSAqL1xcbiAgLWtodG1sLXVzZXItc2VsZWN0OiBub25lOyAgICAvKiBLb25xdWVyb3IgKi9cXG4gIC1tb3otdXNlci1zZWxlY3Q6IG5vbmU7ICAgICAgLyogRmlyZWZveCAqL1xcbiAgLW1zLXVzZXItc2VsZWN0OiBub25lOyAgICAgICAvKiBJbnRlcm5ldCBFeHBsb3Jlci9FZGdlICovXFxuICB1c2VyLXNlbGVjdDogbm9uZTtcXG59XFxuXFxuLnJkdFBpY2tlciB0aC5yZHREaXNhYmxlZCxcXG4ucmR0UGlja2VyIHRoLnJkdERpc2FibGVkOmhvdmVyIHtcXG4gIGJhY2tncm91bmQ6IG5vbmU7XFxuICBjb2xvcjogIzk5OTk5OTtcXG4gIGN1cnNvcjogbm90LWFsbG93ZWQ7XFxufVxcbi5yZHRQaWNrZXIgdGhlYWQgdHI6Zmlyc3QtY2hpbGQgdGgge1xcbiAgY3Vyc29yOiBwb2ludGVyO1xcbn1cXG4ucmR0UGlja2VyIHRoZWFkIHRyOmZpcnN0LWNoaWxkIHRoOmhvdmVyIHtcXG4gIGJhY2tncm91bmQ6ICNlZWVlZWU7XFxufVxcblxcbi5yZHRQaWNrZXIgdGZvb3Qge1xcbiAgYm9yZGVyLXRvcDogMXB4IHNvbGlkICNmOWY5Zjk7XFxufVxcblxcbi5yZHRQaWNrZXIgYnV0dG9uIHtcXG4gIGJvcmRlcjogbm9uZTtcXG4gIGJhY2tncm91bmQ6IG5vbmU7XFxuICBjdXJzb3I6IHBvaW50ZXI7XFxufVxcbi5yZHRQaWNrZXIgYnV0dG9uOmhvdmVyIHtcXG4gIGJhY2tncm91bmQtY29sb3I6ICNlZWU7XFxufVxcblxcbi5yZHRQaWNrZXIgdGhlYWQgYnV0dG9uIHtcXG4gIHdpZHRoOiAxMDAlO1xcbiAgaGVpZ2h0OiAxMDAlO1xcbn1cXG5cXG50ZC5yZHRNb250aCxcXG50ZC5yZHRZZWFyIHtcXG4gIGhlaWdodDogNTBweDtcXG4gIHdpZHRoOiAyNSU7XFxuICBjdXJzb3I6IHBvaW50ZXI7XFxufVxcbnRkLnJkdE1vbnRoOmhvdmVyLFxcbnRkLnJkdFllYXI6aG92ZXIge1xcbiAgYmFja2dyb3VuZDogI2VlZTtcXG59XFxuXFxuLnJkdENvdW50ZXJzIHtcXG4gIGRpc3BsYXk6IGlubGluZS1ibG9jaztcXG59XFxuXFxuLnJkdENvdW50ZXJzID4gZGl2IHtcXG4gIGZsb2F0OiBsZWZ0O1xcbn1cXG5cXG4ucmR0Q291bnRlciB7XFxuICBoZWlnaHQ6IDEwMHB4O1xcbn1cXG5cXG4ucmR0Q291bnRlciB7XFxuICB3aWR0aDogNDBweDtcXG59XFxuXFxuLnJkdENvdW50ZXJTZXBhcmF0b3Ige1xcbiAgbGluZS1oZWlnaHQ6IDEwMHB4O1xcbn1cXG5cXG4ucmR0Q291bnRlciAucmR0QnRuIHtcXG4gIGhlaWdodDogNDAlO1xcbiAgbGluZS1oZWlnaHQ6IDQwcHg7XFxuICBjdXJzb3I6IHBvaW50ZXI7XFxuICBkaXNwbGF5OiBibG9jaztcXG5cXG4gIC13ZWJraXQtdG91Y2gtY2FsbG91dDogbm9uZTsgLyogaU9TIFNhZmFyaSAqL1xcbiAgLXdlYmtpdC11c2VyLXNlbGVjdDogbm9uZTsgICAvKiBDaHJvbWUvU2FmYXJpL09wZXJhICovXFxuICAta2h0bWwtdXNlci1zZWxlY3Q6IG5vbmU7ICAgIC8qIEtvbnF1ZXJvciAqL1xcbiAgLW1vei11c2VyLXNlbGVjdDogbm9uZTsgICAgICAvKiBGaXJlZm94ICovXFxuICAtbXMtdXNlci1zZWxlY3Q6IG5vbmU7ICAgICAgIC8qIEludGVybmV0IEV4cGxvcmVyL0VkZ2UgKi9cXG4gIHVzZXItc2VsZWN0OiBub25lO1xcbn1cXG4ucmR0Q291bnRlciAucmR0QnRuOmhvdmVyIHtcXG4gIGJhY2tncm91bmQ6ICNlZWU7XFxufVxcbi5yZHRDb3VudGVyIC5yZHRDb3VudCB7XFxuICBoZWlnaHQ6IDIwJTtcXG4gIGZvbnQtc2l6ZTogMS4yZW07XFxufVxcblxcbi5yZHRNaWxsaSB7XFxuICB2ZXJ0aWNhbC1hbGlnbjogbWlkZGxlO1xcbiAgcGFkZGluZy1sZWZ0OiA4cHg7XFxuICB3aWR0aDogNDhweDtcXG59XFxuXFxuLnJkdE1pbGxpIGlucHV0IHtcXG4gIHdpZHRoOiAxMDAlO1xcbiAgZm9udC1zaXplOiAxLjJlbTtcXG4gIG1hcmdpbi10b3A6IDM3cHg7XFxufVxcblxcbi5yZHRUaW1lIHRkIHtcXG4gIGN1cnNvcjogZGVmYXVsdDtcXG59XFxuXCIsIFwiXCJdKTtcbiIsIlwidXNlIHN0cmljdFwiO1xuXG4vKlxuICBNSVQgTGljZW5zZSBodHRwOi8vd3d3Lm9wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL21pdC1saWNlbnNlLnBocFxuICBBdXRob3IgVG9iaWFzIEtvcHBlcnMgQHNva3JhXG4qL1xuLy8gY3NzIGJhc2UgY29kZSwgaW5qZWN0ZWQgYnkgdGhlIGNzcy1sb2FkZXJcbi8vIGVzbGludC1kaXNhYmxlLW5leHQtbGluZSBmdW5jLW5hbWVzXG5tb2R1bGUuZXhwb3J0cyA9IGZ1bmN0aW9uICh1c2VTb3VyY2VNYXApIHtcbiAgdmFyIGxpc3QgPSBbXTsgLy8gcmV0dXJuIHRoZSBsaXN0IG9mIG1vZHVsZXMgYXMgY3NzIHN0cmluZ1xuXG4gIGxpc3QudG9TdHJpbmcgPSBmdW5jdGlvbiB0b1N0cmluZygpIHtcbiAgICByZXR1cm4gdGhpcy5tYXAoZnVuY3Rpb24gKGl0ZW0pIHtcbiAgICAgIHZhciBjb250ZW50ID0gY3NzV2l0aE1hcHBpbmdUb1N0cmluZyhpdGVtLCB1c2VTb3VyY2VNYXApO1xuXG4gICAgICBpZiAoaXRlbVsyXSkge1xuICAgICAgICByZXR1cm4gXCJAbWVkaWEgXCIuY29uY2F0KGl0ZW1bMl0sIFwie1wiKS5jb25jYXQoY29udGVudCwgXCJ9XCIpO1xuICAgICAgfVxuXG4gICAgICByZXR1cm4gY29udGVudDtcbiAgICB9KS5qb2luKCcnKTtcbiAgfTsgLy8gaW1wb3J0IGEgbGlzdCBvZiBtb2R1bGVzIGludG8gdGhlIGxpc3RcbiAgLy8gZXNsaW50LWRpc2FibGUtbmV4dC1saW5lIGZ1bmMtbmFtZXNcblxuXG4gIGxpc3QuaSA9IGZ1bmN0aW9uIChtb2R1bGVzLCBtZWRpYVF1ZXJ5KSB7XG4gICAgaWYgKHR5cGVvZiBtb2R1bGVzID09PSAnc3RyaW5nJykge1xuICAgICAgLy8gZXNsaW50LWRpc2FibGUtbmV4dC1saW5lIG5vLXBhcmFtLXJlYXNzaWduXG4gICAgICBtb2R1bGVzID0gW1tudWxsLCBtb2R1bGVzLCAnJ11dO1xuICAgIH1cblxuICAgIHZhciBhbHJlYWR5SW1wb3J0ZWRNb2R1bGVzID0ge307XG5cbiAgICBmb3IgKHZhciBpID0gMDsgaSA8IHRoaXMubGVuZ3RoOyBpKyspIHtcbiAgICAgIC8vIGVzbGludC1kaXNhYmxlLW5leHQtbGluZSBwcmVmZXItZGVzdHJ1Y3R1cmluZ1xuICAgICAgdmFyIGlkID0gdGhpc1tpXVswXTtcblxuICAgICAgaWYgKGlkICE9IG51bGwpIHtcbiAgICAgICAgYWxyZWFkeUltcG9ydGVkTW9kdWxlc1tpZF0gPSB0cnVlO1xuICAgICAgfVxuICAgIH1cblxuICAgIGZvciAodmFyIF9pID0gMDsgX2kgPCBtb2R1bGVzLmxlbmd0aDsgX2krKykge1xuICAgICAgdmFyIGl0ZW0gPSBtb2R1bGVzW19pXTsgLy8gc2tpcCBhbHJlYWR5IGltcG9ydGVkIG1vZHVsZVxuICAgICAgLy8gdGhpcyBpbXBsZW1lbnRhdGlvbiBpcyBub3QgMTAwJSBwZXJmZWN0IGZvciB3ZWlyZCBtZWRpYSBxdWVyeSBjb21iaW5hdGlvbnNcbiAgICAgIC8vIHdoZW4gYSBtb2R1bGUgaXMgaW1wb3J0ZWQgbXVsdGlwbGUgdGltZXMgd2l0aCBkaWZmZXJlbnQgbWVkaWEgcXVlcmllcy5cbiAgICAgIC8vIEkgaG9wZSB0aGlzIHdpbGwgbmV2ZXIgb2NjdXIgKEhleSB0aGlzIHdheSB3ZSBoYXZlIHNtYWxsZXIgYnVuZGxlcylcblxuICAgICAgaWYgKGl0ZW1bMF0gPT0gbnVsbCB8fCAhYWxyZWFkeUltcG9ydGVkTW9kdWxlc1tpdGVtWzBdXSkge1xuICAgICAgICBpZiAobWVkaWFRdWVyeSAmJiAhaXRlbVsyXSkge1xuICAgICAgICAgIGl0ZW1bMl0gPSBtZWRpYVF1ZXJ5O1xuICAgICAgICB9IGVsc2UgaWYgKG1lZGlhUXVlcnkpIHtcbiAgICAgICAgICBpdGVtWzJdID0gXCIoXCIuY29uY2F0KGl0ZW1bMl0sIFwiKSBhbmQgKFwiKS5jb25jYXQobWVkaWFRdWVyeSwgXCIpXCIpO1xuICAgICAgICB9XG5cbiAgICAgICAgbGlzdC5wdXNoKGl0ZW0pO1xuICAgICAgfVxuICAgIH1cbiAgfTtcblxuICByZXR1cm4gbGlzdDtcbn07XG5cbmZ1bmN0aW9uIGNzc1dpdGhNYXBwaW5nVG9TdHJpbmcoaXRlbSwgdXNlU291cmNlTWFwKSB7XG4gIHZhciBjb250ZW50ID0gaXRlbVsxXSB8fCAnJzsgLy8gZXNsaW50LWRpc2FibGUtbmV4dC1saW5lIHByZWZlci1kZXN0cnVjdHVyaW5nXG5cbiAgdmFyIGNzc01hcHBpbmcgPSBpdGVtWzNdO1xuXG4gIGlmICghY3NzTWFwcGluZykge1xuICAgIHJldHVybiBjb250ZW50O1xuICB9XG5cbiAgaWYgKHVzZVNvdXJjZU1hcCAmJiB0eXBlb2YgYnRvYSA9PT0gJ2Z1bmN0aW9uJykge1xuICAgIHZhciBzb3VyY2VNYXBwaW5nID0gdG9Db21tZW50KGNzc01hcHBpbmcpO1xuICAgIHZhciBzb3VyY2VVUkxzID0gY3NzTWFwcGluZy5zb3VyY2VzLm1hcChmdW5jdGlvbiAoc291cmNlKSB7XG4gICAgICByZXR1cm4gXCIvKiMgc291cmNlVVJMPVwiLmNvbmNhdChjc3NNYXBwaW5nLnNvdXJjZVJvb3QpLmNvbmNhdChzb3VyY2UsIFwiICovXCIpO1xuICAgIH0pO1xuICAgIHJldHVybiBbY29udGVudF0uY29uY2F0KHNvdXJjZVVSTHMpLmNvbmNhdChbc291cmNlTWFwcGluZ10pLmpvaW4oJ1xcbicpO1xuICB9XG5cbiAgcmV0dXJuIFtjb250ZW50XS5qb2luKCdcXG4nKTtcbn0gLy8gQWRhcHRlZCBmcm9tIGNvbnZlcnQtc291cmNlLW1hcCAoTUlUKVxuXG5cbmZ1bmN0aW9uIHRvQ29tbWVudChzb3VyY2VNYXApIHtcbiAgLy8gZXNsaW50LWRpc2FibGUtbmV4dC1saW5lIG5vLXVuZGVmXG4gIHZhciBiYXNlNjQgPSBidG9hKHVuZXNjYXBlKGVuY29kZVVSSUNvbXBvbmVudChKU09OLnN0cmluZ2lmeShzb3VyY2VNYXApKSkpO1xuICB2YXIgZGF0YSA9IFwic291cmNlTWFwcGluZ1VSTD1kYXRhOmFwcGxpY2F0aW9uL2pzb247Y2hhcnNldD11dGYtODtiYXNlNjQsXCIuY29uY2F0KGJhc2U2NCk7XG4gIHJldHVybiBcIi8qIyBcIi5jb25jYXQoZGF0YSwgXCIgKi9cIik7XG59IiwiJ3VzZSBzdHJpY3QnO1xudmFyIHRva2VuID0gJyVbYS1mMC05XXsyfSc7XG52YXIgc2luZ2xlTWF0Y2hlciA9IG5ldyBSZWdFeHAoJygnICsgdG9rZW4gKyAnKXwoW14lXSs/KScsICdnaScpO1xudmFyIG11bHRpTWF0Y2hlciA9IG5ldyBSZWdFeHAoJygnICsgdG9rZW4gKyAnKSsnLCAnZ2knKTtcblxuZnVuY3Rpb24gZGVjb2RlQ29tcG9uZW50cyhjb21wb25lbnRzLCBzcGxpdCkge1xuXHR0cnkge1xuXHRcdC8vIFRyeSB0byBkZWNvZGUgdGhlIGVudGlyZSBzdHJpbmcgZmlyc3Rcblx0XHRyZXR1cm4gW2RlY29kZVVSSUNvbXBvbmVudChjb21wb25lbnRzLmpvaW4oJycpKV07XG5cdH0gY2F0Y2ggKGVycikge1xuXHRcdC8vIERvIG5vdGhpbmdcblx0fVxuXG5cdGlmIChjb21wb25lbnRzLmxlbmd0aCA9PT0gMSkge1xuXHRcdHJldHVybiBjb21wb25lbnRzO1xuXHR9XG5cblx0c3BsaXQgPSBzcGxpdCB8fCAxO1xuXG5cdC8vIFNwbGl0IHRoZSBhcnJheSBpbiAyIHBhcnRzXG5cdHZhciBsZWZ0ID0gY29tcG9uZW50cy5zbGljZSgwLCBzcGxpdCk7XG5cdHZhciByaWdodCA9IGNvbXBvbmVudHMuc2xpY2Uoc3BsaXQpO1xuXG5cdHJldHVybiBBcnJheS5wcm90b3R5cGUuY29uY2F0LmNhbGwoW10sIGRlY29kZUNvbXBvbmVudHMobGVmdCksIGRlY29kZUNvbXBvbmVudHMocmlnaHQpKTtcbn1cblxuZnVuY3Rpb24gZGVjb2RlKGlucHV0KSB7XG5cdHRyeSB7XG5cdFx0cmV0dXJuIGRlY29kZVVSSUNvbXBvbmVudChpbnB1dCk7XG5cdH0gY2F0Y2ggKGVycikge1xuXHRcdHZhciB0b2tlbnMgPSBpbnB1dC5tYXRjaChzaW5nbGVNYXRjaGVyKSB8fCBbXTtcblxuXHRcdGZvciAodmFyIGkgPSAxOyBpIDwgdG9rZW5zLmxlbmd0aDsgaSsrKSB7XG5cdFx0XHRpbnB1dCA9IGRlY29kZUNvbXBvbmVudHModG9rZW5zLCBpKS5qb2luKCcnKTtcblxuXHRcdFx0dG9rZW5zID0gaW5wdXQubWF0Y2goc2luZ2xlTWF0Y2hlcikgfHwgW107XG5cdFx0fVxuXG5cdFx0cmV0dXJuIGlucHV0O1xuXHR9XG59XG5cbmZ1bmN0aW9uIGN1c3RvbURlY29kZVVSSUNvbXBvbmVudChpbnB1dCkge1xuXHQvLyBLZWVwIHRyYWNrIG9mIGFsbCB0aGUgcmVwbGFjZW1lbnRzIGFuZCBwcmVmaWxsIHRoZSBtYXAgd2l0aCB0aGUgYEJPTWBcblx0dmFyIHJlcGxhY2VNYXAgPSB7XG5cdFx0JyVGRSVGRic6ICdcXHVGRkZEXFx1RkZGRCcsXG5cdFx0JyVGRiVGRSc6ICdcXHVGRkZEXFx1RkZGRCdcblx0fTtcblxuXHR2YXIgbWF0Y2ggPSBtdWx0aU1hdGNoZXIuZXhlYyhpbnB1dCk7XG5cdHdoaWxlIChtYXRjaCkge1xuXHRcdHRyeSB7XG5cdFx0XHQvLyBEZWNvZGUgYXMgYmlnIGNodW5rcyBhcyBwb3NzaWJsZVxuXHRcdFx0cmVwbGFjZU1hcFttYXRjaFswXV0gPSBkZWNvZGVVUklDb21wb25lbnQobWF0Y2hbMF0pO1xuXHRcdH0gY2F0Y2ggKGVycikge1xuXHRcdFx0dmFyIHJlc3VsdCA9IGRlY29kZShtYXRjaFswXSk7XG5cblx0XHRcdGlmIChyZXN1bHQgIT09IG1hdGNoWzBdKSB7XG5cdFx0XHRcdHJlcGxhY2VNYXBbbWF0Y2hbMF1dID0gcmVzdWx0O1xuXHRcdFx0fVxuXHRcdH1cblxuXHRcdG1hdGNoID0gbXVsdGlNYXRjaGVyLmV4ZWMoaW5wdXQpO1xuXHR9XG5cblx0Ly8gQWRkIGAlQzJgIGF0IHRoZSBlbmQgb2YgdGhlIG1hcCB0byBtYWtlIHN1cmUgaXQgZG9lcyBub3QgcmVwbGFjZSB0aGUgY29tYmluYXRvciBiZWZvcmUgZXZlcnl0aGluZyBlbHNlXG5cdHJlcGxhY2VNYXBbJyVDMiddID0gJ1xcdUZGRkQnO1xuXG5cdHZhciBlbnRyaWVzID0gT2JqZWN0LmtleXMocmVwbGFjZU1hcCk7XG5cblx0Zm9yICh2YXIgaSA9IDA7IGkgPCBlbnRyaWVzLmxlbmd0aDsgaSsrKSB7XG5cdFx0Ly8gUmVwbGFjZSBhbGwgZGVjb2RlZCBjb21wb25lbnRzXG5cdFx0dmFyIGtleSA9IGVudHJpZXNbaV07XG5cdFx0aW5wdXQgPSBpbnB1dC5yZXBsYWNlKG5ldyBSZWdFeHAoa2V5LCAnZycpLCByZXBsYWNlTWFwW2tleV0pO1xuXHR9XG5cblx0cmV0dXJuIGlucHV0O1xufVxuXG5tb2R1bGUuZXhwb3J0cyA9IGZ1bmN0aW9uIChlbmNvZGVkVVJJKSB7XG5cdGlmICh0eXBlb2YgZW5jb2RlZFVSSSAhPT0gJ3N0cmluZycpIHtcblx0XHR0aHJvdyBuZXcgVHlwZUVycm9yKCdFeHBlY3RlZCBgZW5jb2RlZFVSSWAgdG8gYmUgb2YgdHlwZSBgc3RyaW5nYCwgZ290IGAnICsgdHlwZW9mIGVuY29kZWRVUkkgKyAnYCcpO1xuXHR9XG5cblx0dHJ5IHtcblx0XHRlbmNvZGVkVVJJID0gZW5jb2RlZFVSSS5yZXBsYWNlKC9cXCsvZywgJyAnKTtcblxuXHRcdC8vIFRyeSB0aGUgYnVpbHQgaW4gZGVjb2RlciBmaXJzdFxuXHRcdHJldHVybiBkZWNvZGVVUklDb21wb25lbnQoZW5jb2RlZFVSSSk7XG5cdH0gY2F0Y2ggKGVycikge1xuXHRcdC8vIEZhbGxiYWNrIHRvIGEgbW9yZSBhZHZhbmNlZCBkZWNvZGVyXG5cdFx0cmV0dXJuIGN1c3RvbURlY29kZVVSSUNvbXBvbmVudChlbmNvZGVkVVJJKTtcblx0fVxufTtcbiIsIlwidXNlIHN0cmljdFwiO1xuXG4vKipcbiAqIENvcHlyaWdodCAoYykgMjAxMy1wcmVzZW50LCBGYWNlYm9vaywgSW5jLlxuICpcbiAqIFRoaXMgc291cmNlIGNvZGUgaXMgbGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlIGZvdW5kIGluIHRoZVxuICogTElDRU5TRSBmaWxlIGluIHRoZSByb290IGRpcmVjdG9yeSBvZiB0aGlzIHNvdXJjZSB0cmVlLlxuICpcbiAqIFxuICovXG5cbmZ1bmN0aW9uIG1ha2VFbXB0eUZ1bmN0aW9uKGFyZykge1xuICByZXR1cm4gZnVuY3Rpb24gKCkge1xuICAgIHJldHVybiBhcmc7XG4gIH07XG59XG5cbi8qKlxuICogVGhpcyBmdW5jdGlvbiBhY2NlcHRzIGFuZCBkaXNjYXJkcyBpbnB1dHM7IGl0IGhhcyBubyBzaWRlIGVmZmVjdHMuIFRoaXMgaXNcbiAqIHByaW1hcmlseSB1c2VmdWwgaWRpb21hdGljYWxseSBmb3Igb3ZlcnJpZGFibGUgZnVuY3Rpb24gZW5kcG9pbnRzIHdoaWNoXG4gKiBhbHdheXMgbmVlZCB0byBiZSBjYWxsYWJsZSwgc2luY2UgSlMgbGFja3MgYSBudWxsLWNhbGwgaWRpb20gYWxhIENvY29hLlxuICovXG52YXIgZW1wdHlGdW5jdGlvbiA9IGZ1bmN0aW9uIGVtcHR5RnVuY3Rpb24oKSB7fTtcblxuZW1wdHlGdW5jdGlvbi50aGF0UmV0dXJucyA9IG1ha2VFbXB0eUZ1bmN0aW9uO1xuZW1wdHlGdW5jdGlvbi50aGF0UmV0dXJuc0ZhbHNlID0gbWFrZUVtcHR5RnVuY3Rpb24oZmFsc2UpO1xuZW1wdHlGdW5jdGlvbi50aGF0UmV0dXJuc1RydWUgPSBtYWtlRW1wdHlGdW5jdGlvbih0cnVlKTtcbmVtcHR5RnVuY3Rpb24udGhhdFJldHVybnNOdWxsID0gbWFrZUVtcHR5RnVuY3Rpb24obnVsbCk7XG5lbXB0eUZ1bmN0aW9uLnRoYXRSZXR1cm5zVGhpcyA9IGZ1bmN0aW9uICgpIHtcbiAgcmV0dXJuIHRoaXM7XG59O1xuZW1wdHlGdW5jdGlvbi50aGF0UmV0dXJuc0FyZ3VtZW50ID0gZnVuY3Rpb24gKGFyZykge1xuICByZXR1cm4gYXJnO1xufTtcblxubW9kdWxlLmV4cG9ydHMgPSBlbXB0eUZ1bmN0aW9uOyIsIi8qKlxuICogQ29weXJpZ2h0IChjKSAyMDEzLXByZXNlbnQsIEZhY2Vib29rLCBJbmMuXG4gKlxuICogVGhpcyBzb3VyY2UgY29kZSBpcyBsaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UgZm91bmQgaW4gdGhlXG4gKiBMSUNFTlNFIGZpbGUgaW4gdGhlIHJvb3QgZGlyZWN0b3J5IG9mIHRoaXMgc291cmNlIHRyZWUuXG4gKlxuICovXG5cbid1c2Ugc3RyaWN0JztcblxuLyoqXG4gKiBVc2UgaW52YXJpYW50KCkgdG8gYXNzZXJ0IHN0YXRlIHdoaWNoIHlvdXIgcHJvZ3JhbSBhc3N1bWVzIHRvIGJlIHRydWUuXG4gKlxuICogUHJvdmlkZSBzcHJpbnRmLXN0eWxlIGZvcm1hdCAob25seSAlcyBpcyBzdXBwb3J0ZWQpIGFuZCBhcmd1bWVudHNcbiAqIHRvIHByb3ZpZGUgaW5mb3JtYXRpb24gYWJvdXQgd2hhdCBicm9rZSBhbmQgd2hhdCB5b3Ugd2VyZVxuICogZXhwZWN0aW5nLlxuICpcbiAqIFRoZSBpbnZhcmlhbnQgbWVzc2FnZSB3aWxsIGJlIHN0cmlwcGVkIGluIHByb2R1Y3Rpb24sIGJ1dCB0aGUgaW52YXJpYW50XG4gKiB3aWxsIHJlbWFpbiB0byBlbnN1cmUgbG9naWMgZG9lcyBub3QgZGlmZmVyIGluIHByb2R1Y3Rpb24uXG4gKi9cblxudmFyIHZhbGlkYXRlRm9ybWF0ID0gZnVuY3Rpb24gdmFsaWRhdGVGb3JtYXQoZm9ybWF0KSB7fTtcblxuaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgdmFsaWRhdGVGb3JtYXQgPSBmdW5jdGlvbiB2YWxpZGF0ZUZvcm1hdChmb3JtYXQpIHtcbiAgICBpZiAoZm9ybWF0ID09PSB1bmRlZmluZWQpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcignaW52YXJpYW50IHJlcXVpcmVzIGFuIGVycm9yIG1lc3NhZ2UgYXJndW1lbnQnKTtcbiAgICB9XG4gIH07XG59XG5cbmZ1bmN0aW9uIGludmFyaWFudChjb25kaXRpb24sIGZvcm1hdCwgYSwgYiwgYywgZCwgZSwgZikge1xuICB2YWxpZGF0ZUZvcm1hdChmb3JtYXQpO1xuXG4gIGlmICghY29uZGl0aW9uKSB7XG4gICAgdmFyIGVycm9yO1xuICAgIGlmIChmb3JtYXQgPT09IHVuZGVmaW5lZCkge1xuICAgICAgZXJyb3IgPSBuZXcgRXJyb3IoJ01pbmlmaWVkIGV4Y2VwdGlvbiBvY2N1cnJlZDsgdXNlIHRoZSBub24tbWluaWZpZWQgZGV2IGVudmlyb25tZW50ICcgKyAnZm9yIHRoZSBmdWxsIGVycm9yIG1lc3NhZ2UgYW5kIGFkZGl0aW9uYWwgaGVscGZ1bCB3YXJuaW5ncy4nKTtcbiAgICB9IGVsc2Uge1xuICAgICAgdmFyIGFyZ3MgPSBbYSwgYiwgYywgZCwgZSwgZl07XG4gICAgICB2YXIgYXJnSW5kZXggPSAwO1xuICAgICAgZXJyb3IgPSBuZXcgRXJyb3IoZm9ybWF0LnJlcGxhY2UoLyVzL2csIGZ1bmN0aW9uICgpIHtcbiAgICAgICAgcmV0dXJuIGFyZ3NbYXJnSW5kZXgrK107XG4gICAgICB9KSk7XG4gICAgICBlcnJvci5uYW1lID0gJ0ludmFyaWFudCBWaW9sYXRpb24nO1xuICAgIH1cblxuICAgIGVycm9yLmZyYW1lc1RvUG9wID0gMTsgLy8gd2UgZG9uJ3QgY2FyZSBhYm91dCBpbnZhcmlhbnQncyBvd24gZnJhbWVcbiAgICB0aHJvdyBlcnJvcjtcbiAgfVxufVxuXG5tb2R1bGUuZXhwb3J0cyA9IGludmFyaWFudDsiLCIvKipcbiAqIENvcHlyaWdodCAoYykgMjAxNC1wcmVzZW50LCBGYWNlYm9vaywgSW5jLlxuICpcbiAqIFRoaXMgc291cmNlIGNvZGUgaXMgbGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlIGZvdW5kIGluIHRoZVxuICogTElDRU5TRSBmaWxlIGluIHRoZSByb290IGRpcmVjdG9yeSBvZiB0aGlzIHNvdXJjZSB0cmVlLlxuICpcbiAqL1xuXG4ndXNlIHN0cmljdCc7XG5cbnZhciBlbXB0eUZ1bmN0aW9uID0gcmVxdWlyZSgnLi9lbXB0eUZ1bmN0aW9uJyk7XG5cbi8qKlxuICogU2ltaWxhciB0byBpbnZhcmlhbnQgYnV0IG9ubHkgbG9ncyBhIHdhcm5pbmcgaWYgdGhlIGNvbmRpdGlvbiBpcyBub3QgbWV0LlxuICogVGhpcyBjYW4gYmUgdXNlZCB0byBsb2cgaXNzdWVzIGluIGRldmVsb3BtZW50IGVudmlyb25tZW50cyBpbiBjcml0aWNhbFxuICogcGF0aHMuIFJlbW92aW5nIHRoZSBsb2dnaW5nIGNvZGUgZm9yIHByb2R1Y3Rpb24gZW52aXJvbm1lbnRzIHdpbGwga2VlcCB0aGVcbiAqIHNhbWUgbG9naWMgYW5kIGZvbGxvdyB0aGUgc2FtZSBjb2RlIHBhdGhzLlxuICovXG5cbnZhciB3YXJuaW5nID0gZW1wdHlGdW5jdGlvbjtcblxuaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgdmFyIHByaW50V2FybmluZyA9IGZ1bmN0aW9uIHByaW50V2FybmluZyhmb3JtYXQpIHtcbiAgICBmb3IgKHZhciBfbGVuID0gYXJndW1lbnRzLmxlbmd0aCwgYXJncyA9IEFycmF5KF9sZW4gPiAxID8gX2xlbiAtIDEgOiAwKSwgX2tleSA9IDE7IF9rZXkgPCBfbGVuOyBfa2V5KyspIHtcbiAgICAgIGFyZ3NbX2tleSAtIDFdID0gYXJndW1lbnRzW19rZXldO1xuICAgIH1cblxuICAgIHZhciBhcmdJbmRleCA9IDA7XG4gICAgdmFyIG1lc3NhZ2UgPSAnV2FybmluZzogJyArIGZvcm1hdC5yZXBsYWNlKC8lcy9nLCBmdW5jdGlvbiAoKSB7XG4gICAgICByZXR1cm4gYXJnc1thcmdJbmRleCsrXTtcbiAgICB9KTtcbiAgICBpZiAodHlwZW9mIGNvbnNvbGUgIT09ICd1bmRlZmluZWQnKSB7XG4gICAgICBjb25zb2xlLmVycm9yKG1lc3NhZ2UpO1xuICAgIH1cbiAgICB0cnkge1xuICAgICAgLy8gLS0tIFdlbGNvbWUgdG8gZGVidWdnaW5nIFJlYWN0IC0tLVxuICAgICAgLy8gVGhpcyBlcnJvciB3YXMgdGhyb3duIGFzIGEgY29udmVuaWVuY2Ugc28gdGhhdCB5b3UgY2FuIHVzZSB0aGlzIHN0YWNrXG4gICAgICAvLyB0byBmaW5kIHRoZSBjYWxsc2l0ZSB0aGF0IGNhdXNlZCB0aGlzIHdhcm5pbmcgdG8gZmlyZS5cbiAgICAgIHRocm93IG5ldyBFcnJvcihtZXNzYWdlKTtcbiAgICB9IGNhdGNoICh4KSB7fVxuICB9O1xuXG4gIHdhcm5pbmcgPSBmdW5jdGlvbiB3YXJuaW5nKGNvbmRpdGlvbiwgZm9ybWF0KSB7XG4gICAgaWYgKGZvcm1hdCA9PT0gdW5kZWZpbmVkKSB7XG4gICAgICB0aHJvdyBuZXcgRXJyb3IoJ2B3YXJuaW5nKGNvbmRpdGlvbiwgZm9ybWF0LCAuLi5hcmdzKWAgcmVxdWlyZXMgYSB3YXJuaW5nICcgKyAnbWVzc2FnZSBhcmd1bWVudCcpO1xuICAgIH1cblxuICAgIGlmIChmb3JtYXQuaW5kZXhPZignRmFpbGVkIENvbXBvc2l0ZSBwcm9wVHlwZTogJykgPT09IDApIHtcbiAgICAgIHJldHVybjsgLy8gSWdub3JlIENvbXBvc2l0ZUNvbXBvbmVudCBwcm9wdHlwZSBjaGVjay5cbiAgICB9XG5cbiAgICBpZiAoIWNvbmRpdGlvbikge1xuICAgICAgZm9yICh2YXIgX2xlbjIgPSBhcmd1bWVudHMubGVuZ3RoLCBhcmdzID0gQXJyYXkoX2xlbjIgPiAyID8gX2xlbjIgLSAyIDogMCksIF9rZXkyID0gMjsgX2tleTIgPCBfbGVuMjsgX2tleTIrKykge1xuICAgICAgICBhcmdzW19rZXkyIC0gMl0gPSBhcmd1bWVudHNbX2tleTJdO1xuICAgICAgfVxuXG4gICAgICBwcmludFdhcm5pbmcuYXBwbHkodW5kZWZpbmVkLCBbZm9ybWF0XS5jb25jYXQoYXJncykpO1xuICAgIH1cbiAgfTtcbn1cblxubW9kdWxlLmV4cG9ydHMgPSB3YXJuaW5nOyIsIid1c2Ugc3RyaWN0JztcblxuZXhwb3J0cy5fX2VzTW9kdWxlID0gdHJ1ZTtcbnZhciBjYW5Vc2VET00gPSBleHBvcnRzLmNhblVzZURPTSA9ICEhKHR5cGVvZiB3aW5kb3cgIT09ICd1bmRlZmluZWQnICYmIHdpbmRvdy5kb2N1bWVudCAmJiB3aW5kb3cuZG9jdW1lbnQuY3JlYXRlRWxlbWVudCk7XG5cbnZhciBhZGRFdmVudExpc3RlbmVyID0gZXhwb3J0cy5hZGRFdmVudExpc3RlbmVyID0gZnVuY3Rpb24gYWRkRXZlbnRMaXN0ZW5lcihub2RlLCBldmVudCwgbGlzdGVuZXIpIHtcbiAgcmV0dXJuIG5vZGUuYWRkRXZlbnRMaXN0ZW5lciA/IG5vZGUuYWRkRXZlbnRMaXN0ZW5lcihldmVudCwgbGlzdGVuZXIsIGZhbHNlKSA6IG5vZGUuYXR0YWNoRXZlbnQoJ29uJyArIGV2ZW50LCBsaXN0ZW5lcik7XG59O1xuXG52YXIgcmVtb3ZlRXZlbnRMaXN0ZW5lciA9IGV4cG9ydHMucmVtb3ZlRXZlbnRMaXN0ZW5lciA9IGZ1bmN0aW9uIHJlbW92ZUV2ZW50TGlzdGVuZXIobm9kZSwgZXZlbnQsIGxpc3RlbmVyKSB7XG4gIHJldHVybiBub2RlLnJlbW92ZUV2ZW50TGlzdGVuZXIgPyBub2RlLnJlbW92ZUV2ZW50TGlzdGVuZXIoZXZlbnQsIGxpc3RlbmVyLCBmYWxzZSkgOiBub2RlLmRldGFjaEV2ZW50KCdvbicgKyBldmVudCwgbGlzdGVuZXIpO1xufTtcblxudmFyIGdldENvbmZpcm1hdGlvbiA9IGV4cG9ydHMuZ2V0Q29uZmlybWF0aW9uID0gZnVuY3Rpb24gZ2V0Q29uZmlybWF0aW9uKG1lc3NhZ2UsIGNhbGxiYWNrKSB7XG4gIHJldHVybiBjYWxsYmFjayh3aW5kb3cuY29uZmlybShtZXNzYWdlKSk7XG59OyAvLyBlc2xpbnQtZGlzYWJsZS1saW5lIG5vLWFsZXJ0XG5cbi8qKlxuICogUmV0dXJucyB0cnVlIGlmIHRoZSBIVE1MNSBoaXN0b3J5IEFQSSBpcyBzdXBwb3J0ZWQuIFRha2VuIGZyb20gTW9kZXJuaXpyLlxuICpcbiAqIGh0dHBzOi8vZ2l0aHViLmNvbS9Nb2Rlcm5penIvTW9kZXJuaXpyL2Jsb2IvbWFzdGVyL0xJQ0VOU0VcbiAqIGh0dHBzOi8vZ2l0aHViLmNvbS9Nb2Rlcm5penIvTW9kZXJuaXpyL2Jsb2IvbWFzdGVyL2ZlYXR1cmUtZGV0ZWN0cy9oaXN0b3J5LmpzXG4gKiBjaGFuZ2VkIHRvIGF2b2lkIGZhbHNlIG5lZ2F0aXZlcyBmb3IgV2luZG93cyBQaG9uZXM6IGh0dHBzOi8vZ2l0aHViLmNvbS9yZWFjdGpzL3JlYWN0LXJvdXRlci9pc3N1ZXMvNTg2XG4gKi9cbnZhciBzdXBwb3J0c0hpc3RvcnkgPSBleHBvcnRzLnN1cHBvcnRzSGlzdG9yeSA9IGZ1bmN0aW9uIHN1cHBvcnRzSGlzdG9yeSgpIHtcbiAgdmFyIHVhID0gd2luZG93Lm5hdmlnYXRvci51c2VyQWdlbnQ7XG5cbiAgaWYgKCh1YS5pbmRleE9mKCdBbmRyb2lkIDIuJykgIT09IC0xIHx8IHVhLmluZGV4T2YoJ0FuZHJvaWQgNC4wJykgIT09IC0xKSAmJiB1YS5pbmRleE9mKCdNb2JpbGUgU2FmYXJpJykgIT09IC0xICYmIHVhLmluZGV4T2YoJ0Nocm9tZScpID09PSAtMSAmJiB1YS5pbmRleE9mKCdXaW5kb3dzIFBob25lJykgPT09IC0xKSByZXR1cm4gZmFsc2U7XG5cbiAgcmV0dXJuIHdpbmRvdy5oaXN0b3J5ICYmICdwdXNoU3RhdGUnIGluIHdpbmRvdy5oaXN0b3J5O1xufTtcblxuLyoqXG4gKiBSZXR1cm5zIHRydWUgaWYgYnJvd3NlciBmaXJlcyBwb3BzdGF0ZSBvbiBoYXNoIGNoYW5nZS5cbiAqIElFMTAgYW5kIElFMTEgZG8gbm90LlxuICovXG52YXIgc3VwcG9ydHNQb3BTdGF0ZU9uSGFzaENoYW5nZSA9IGV4cG9ydHMuc3VwcG9ydHNQb3BTdGF0ZU9uSGFzaENoYW5nZSA9IGZ1bmN0aW9uIHN1cHBvcnRzUG9wU3RhdGVPbkhhc2hDaGFuZ2UoKSB7XG4gIHJldHVybiB3aW5kb3cubmF2aWdhdG9yLnVzZXJBZ2VudC5pbmRleE9mKCdUcmlkZW50JykgPT09IC0xO1xufTtcblxuLyoqXG4gKiBSZXR1cm5zIGZhbHNlIGlmIHVzaW5nIGdvKG4pIHdpdGggaGFzaCBoaXN0b3J5IGNhdXNlcyBhIGZ1bGwgcGFnZSByZWxvYWQuXG4gKi9cbnZhciBzdXBwb3J0c0dvV2l0aG91dFJlbG9hZFVzaW5nSGFzaCA9IGV4cG9ydHMuc3VwcG9ydHNHb1dpdGhvdXRSZWxvYWRVc2luZ0hhc2ggPSBmdW5jdGlvbiBzdXBwb3J0c0dvV2l0aG91dFJlbG9hZFVzaW5nSGFzaCgpIHtcbiAgcmV0dXJuIHdpbmRvdy5uYXZpZ2F0b3IudXNlckFnZW50LmluZGV4T2YoJ0ZpcmVmb3gnKSA9PT0gLTE7XG59O1xuXG4vKipcbiAqIFJldHVybnMgdHJ1ZSBpZiBhIGdpdmVuIHBvcHN0YXRlIGV2ZW50IGlzIGFuIGV4dHJhbmVvdXMgV2ViS2l0IGV2ZW50LlxuICogQWNjb3VudHMgZm9yIHRoZSBmYWN0IHRoYXQgQ2hyb21lIG9uIGlPUyBmaXJlcyByZWFsIHBvcHN0YXRlIGV2ZW50c1xuICogY29udGFpbmluZyB1bmRlZmluZWQgc3RhdGUgd2hlbiBwcmVzc2luZyB0aGUgYmFjayBidXR0b24uXG4gKi9cbnZhciBpc0V4dHJhbmVvdXNQb3BzdGF0ZUV2ZW50ID0gZXhwb3J0cy5pc0V4dHJhbmVvdXNQb3BzdGF0ZUV2ZW50ID0gZnVuY3Rpb24gaXNFeHRyYW5lb3VzUG9wc3RhdGVFdmVudChldmVudCkge1xuICByZXR1cm4gZXZlbnQuc3RhdGUgPT09IHVuZGVmaW5lZCAmJiBuYXZpZ2F0b3IudXNlckFnZW50LmluZGV4T2YoJ0NyaU9TJykgPT09IC0xO1xufTsiLCIndXNlIHN0cmljdCc7XG5cbmV4cG9ydHMuX19lc01vZHVsZSA9IHRydWU7XG5leHBvcnRzLmxvY2F0aW9uc0FyZUVxdWFsID0gZXhwb3J0cy5jcmVhdGVMb2NhdGlvbiA9IHVuZGVmaW5lZDtcblxudmFyIF9leHRlbmRzID0gT2JqZWN0LmFzc2lnbiB8fCBmdW5jdGlvbiAodGFyZ2V0KSB7IGZvciAodmFyIGkgPSAxOyBpIDwgYXJndW1lbnRzLmxlbmd0aDsgaSsrKSB7IHZhciBzb3VyY2UgPSBhcmd1bWVudHNbaV07IGZvciAodmFyIGtleSBpbiBzb3VyY2UpIHsgaWYgKE9iamVjdC5wcm90b3R5cGUuaGFzT3duUHJvcGVydHkuY2FsbChzb3VyY2UsIGtleSkpIHsgdGFyZ2V0W2tleV0gPSBzb3VyY2Vba2V5XTsgfSB9IH0gcmV0dXJuIHRhcmdldDsgfTtcblxudmFyIF9yZXNvbHZlUGF0aG5hbWUgPSByZXF1aXJlKCdyZXNvbHZlLXBhdGhuYW1lJyk7XG5cbnZhciBfcmVzb2x2ZVBhdGhuYW1lMiA9IF9pbnRlcm9wUmVxdWlyZURlZmF1bHQoX3Jlc29sdmVQYXRobmFtZSk7XG5cbnZhciBfdmFsdWVFcXVhbCA9IHJlcXVpcmUoJ3ZhbHVlLWVxdWFsJyk7XG5cbnZhciBfdmFsdWVFcXVhbDIgPSBfaW50ZXJvcFJlcXVpcmVEZWZhdWx0KF92YWx1ZUVxdWFsKTtcblxudmFyIF9QYXRoVXRpbHMgPSByZXF1aXJlKCcuL1BhdGhVdGlscycpO1xuXG5mdW5jdGlvbiBfaW50ZXJvcFJlcXVpcmVEZWZhdWx0KG9iaikgeyByZXR1cm4gb2JqICYmIG9iai5fX2VzTW9kdWxlID8gb2JqIDogeyBkZWZhdWx0OiBvYmogfTsgfVxuXG52YXIgY3JlYXRlTG9jYXRpb24gPSBleHBvcnRzLmNyZWF0ZUxvY2F0aW9uID0gZnVuY3Rpb24gY3JlYXRlTG9jYXRpb24ocGF0aCwgc3RhdGUsIGtleSwgY3VycmVudExvY2F0aW9uKSB7XG4gIHZhciBsb2NhdGlvbiA9IHZvaWQgMDtcbiAgaWYgKHR5cGVvZiBwYXRoID09PSAnc3RyaW5nJykge1xuICAgIC8vIFR3by1hcmcgZm9ybTogcHVzaChwYXRoLCBzdGF0ZSlcbiAgICBsb2NhdGlvbiA9ICgwLCBfUGF0aFV0aWxzLnBhcnNlUGF0aCkocGF0aCk7XG4gICAgbG9jYXRpb24uc3RhdGUgPSBzdGF0ZTtcbiAgfSBlbHNlIHtcbiAgICAvLyBPbmUtYXJnIGZvcm06IHB1c2gobG9jYXRpb24pXG4gICAgbG9jYXRpb24gPSBfZXh0ZW5kcyh7fSwgcGF0aCk7XG5cbiAgICBpZiAobG9jYXRpb24ucGF0aG5hbWUgPT09IHVuZGVmaW5lZCkgbG9jYXRpb24ucGF0aG5hbWUgPSAnJztcblxuICAgIGlmIChsb2NhdGlvbi5zZWFyY2gpIHtcbiAgICAgIGlmIChsb2NhdGlvbi5zZWFyY2guY2hhckF0KDApICE9PSAnPycpIGxvY2F0aW9uLnNlYXJjaCA9ICc/JyArIGxvY2F0aW9uLnNlYXJjaDtcbiAgICB9IGVsc2Uge1xuICAgICAgbG9jYXRpb24uc2VhcmNoID0gJyc7XG4gICAgfVxuXG4gICAgaWYgKGxvY2F0aW9uLmhhc2gpIHtcbiAgICAgIGlmIChsb2NhdGlvbi5oYXNoLmNoYXJBdCgwKSAhPT0gJyMnKSBsb2NhdGlvbi5oYXNoID0gJyMnICsgbG9jYXRpb24uaGFzaDtcbiAgICB9IGVsc2Uge1xuICAgICAgbG9jYXRpb24uaGFzaCA9ICcnO1xuICAgIH1cblxuICAgIGlmIChzdGF0ZSAhPT0gdW5kZWZpbmVkICYmIGxvY2F0aW9uLnN0YXRlID09PSB1bmRlZmluZWQpIGxvY2F0aW9uLnN0YXRlID0gc3RhdGU7XG4gIH1cblxuICB0cnkge1xuICAgIGxvY2F0aW9uLnBhdGhuYW1lID0gZGVjb2RlVVJJKGxvY2F0aW9uLnBhdGhuYW1lKTtcbiAgfSBjYXRjaCAoZSkge1xuICAgIGlmIChlIGluc3RhbmNlb2YgVVJJRXJyb3IpIHtcbiAgICAgIHRocm93IG5ldyBVUklFcnJvcignUGF0aG5hbWUgXCInICsgbG9jYXRpb24ucGF0aG5hbWUgKyAnXCIgY291bGQgbm90IGJlIGRlY29kZWQuICcgKyAnVGhpcyBpcyBsaWtlbHkgY2F1c2VkIGJ5IGFuIGludmFsaWQgcGVyY2VudC1lbmNvZGluZy4nKTtcbiAgICB9IGVsc2Uge1xuICAgICAgdGhyb3cgZTtcbiAgICB9XG4gIH1cblxuICBpZiAoa2V5KSBsb2NhdGlvbi5rZXkgPSBrZXk7XG5cbiAgaWYgKGN1cnJlbnRMb2NhdGlvbikge1xuICAgIC8vIFJlc29sdmUgaW5jb21wbGV0ZS9yZWxhdGl2ZSBwYXRobmFtZSByZWxhdGl2ZSB0byBjdXJyZW50IGxvY2F0aW9uLlxuICAgIGlmICghbG9jYXRpb24ucGF0aG5hbWUpIHtcbiAgICAgIGxvY2F0aW9uLnBhdGhuYW1lID0gY3VycmVudExvY2F0aW9uLnBhdGhuYW1lO1xuICAgIH0gZWxzZSBpZiAobG9jYXRpb24ucGF0aG5hbWUuY2hhckF0KDApICE9PSAnLycpIHtcbiAgICAgIGxvY2F0aW9uLnBhdGhuYW1lID0gKDAsIF9yZXNvbHZlUGF0aG5hbWUyLmRlZmF1bHQpKGxvY2F0aW9uLnBhdGhuYW1lLCBjdXJyZW50TG9jYXRpb24ucGF0aG5hbWUpO1xuICAgIH1cbiAgfSBlbHNlIHtcbiAgICAvLyBXaGVuIHRoZXJlIGlzIG5vIHByaW9yIGxvY2F0aW9uIGFuZCBwYXRobmFtZSBpcyBlbXB0eSwgc2V0IGl0IHRvIC9cbiAgICBpZiAoIWxvY2F0aW9uLnBhdGhuYW1lKSB7XG4gICAgICBsb2NhdGlvbi5wYXRobmFtZSA9ICcvJztcbiAgICB9XG4gIH1cblxuICByZXR1cm4gbG9jYXRpb247XG59O1xuXG52YXIgbG9jYXRpb25zQXJlRXF1YWwgPSBleHBvcnRzLmxvY2F0aW9uc0FyZUVxdWFsID0gZnVuY3Rpb24gbG9jYXRpb25zQXJlRXF1YWwoYSwgYikge1xuICByZXR1cm4gYS5wYXRobmFtZSA9PT0gYi5wYXRobmFtZSAmJiBhLnNlYXJjaCA9PT0gYi5zZWFyY2ggJiYgYS5oYXNoID09PSBiLmhhc2ggJiYgYS5rZXkgPT09IGIua2V5ICYmICgwLCBfdmFsdWVFcXVhbDIuZGVmYXVsdCkoYS5zdGF0ZSwgYi5zdGF0ZSk7XG59OyIsIid1c2Ugc3RyaWN0JztcblxuZXhwb3J0cy5fX2VzTW9kdWxlID0gdHJ1ZTtcbnZhciBhZGRMZWFkaW5nU2xhc2ggPSBleHBvcnRzLmFkZExlYWRpbmdTbGFzaCA9IGZ1bmN0aW9uIGFkZExlYWRpbmdTbGFzaChwYXRoKSB7XG4gIHJldHVybiBwYXRoLmNoYXJBdCgwKSA9PT0gJy8nID8gcGF0aCA6ICcvJyArIHBhdGg7XG59O1xuXG52YXIgc3RyaXBMZWFkaW5nU2xhc2ggPSBleHBvcnRzLnN0cmlwTGVhZGluZ1NsYXNoID0gZnVuY3Rpb24gc3RyaXBMZWFkaW5nU2xhc2gocGF0aCkge1xuICByZXR1cm4gcGF0aC5jaGFyQXQoMCkgPT09ICcvJyA/IHBhdGguc3Vic3RyKDEpIDogcGF0aDtcbn07XG5cbnZhciBoYXNCYXNlbmFtZSA9IGV4cG9ydHMuaGFzQmFzZW5hbWUgPSBmdW5jdGlvbiBoYXNCYXNlbmFtZShwYXRoLCBwcmVmaXgpIHtcbiAgcmV0dXJuIG5ldyBSZWdFeHAoJ14nICsgcHJlZml4ICsgJyhcXFxcL3xcXFxcP3wjfCQpJywgJ2knKS50ZXN0KHBhdGgpO1xufTtcblxudmFyIHN0cmlwQmFzZW5hbWUgPSBleHBvcnRzLnN0cmlwQmFzZW5hbWUgPSBmdW5jdGlvbiBzdHJpcEJhc2VuYW1lKHBhdGgsIHByZWZpeCkge1xuICByZXR1cm4gaGFzQmFzZW5hbWUocGF0aCwgcHJlZml4KSA/IHBhdGguc3Vic3RyKHByZWZpeC5sZW5ndGgpIDogcGF0aDtcbn07XG5cbnZhciBzdHJpcFRyYWlsaW5nU2xhc2ggPSBleHBvcnRzLnN0cmlwVHJhaWxpbmdTbGFzaCA9IGZ1bmN0aW9uIHN0cmlwVHJhaWxpbmdTbGFzaChwYXRoKSB7XG4gIHJldHVybiBwYXRoLmNoYXJBdChwYXRoLmxlbmd0aCAtIDEpID09PSAnLycgPyBwYXRoLnNsaWNlKDAsIC0xKSA6IHBhdGg7XG59O1xuXG52YXIgcGFyc2VQYXRoID0gZXhwb3J0cy5wYXJzZVBhdGggPSBmdW5jdGlvbiBwYXJzZVBhdGgocGF0aCkge1xuICB2YXIgcGF0aG5hbWUgPSBwYXRoIHx8ICcvJztcbiAgdmFyIHNlYXJjaCA9ICcnO1xuICB2YXIgaGFzaCA9ICcnO1xuXG4gIHZhciBoYXNoSW5kZXggPSBwYXRobmFtZS5pbmRleE9mKCcjJyk7XG4gIGlmIChoYXNoSW5kZXggIT09IC0xKSB7XG4gICAgaGFzaCA9IHBhdGhuYW1lLnN1YnN0cihoYXNoSW5kZXgpO1xuICAgIHBhdGhuYW1lID0gcGF0aG5hbWUuc3Vic3RyKDAsIGhhc2hJbmRleCk7XG4gIH1cblxuICB2YXIgc2VhcmNoSW5kZXggPSBwYXRobmFtZS5pbmRleE9mKCc/Jyk7XG4gIGlmIChzZWFyY2hJbmRleCAhPT0gLTEpIHtcbiAgICBzZWFyY2ggPSBwYXRobmFtZS5zdWJzdHIoc2VhcmNoSW5kZXgpO1xuICAgIHBhdGhuYW1lID0gcGF0aG5hbWUuc3Vic3RyKDAsIHNlYXJjaEluZGV4KTtcbiAgfVxuXG4gIHJldHVybiB7XG4gICAgcGF0aG5hbWU6IHBhdGhuYW1lLFxuICAgIHNlYXJjaDogc2VhcmNoID09PSAnPycgPyAnJyA6IHNlYXJjaCxcbiAgICBoYXNoOiBoYXNoID09PSAnIycgPyAnJyA6IGhhc2hcbiAgfTtcbn07XG5cbnZhciBjcmVhdGVQYXRoID0gZXhwb3J0cy5jcmVhdGVQYXRoID0gZnVuY3Rpb24gY3JlYXRlUGF0aChsb2NhdGlvbikge1xuICB2YXIgcGF0aG5hbWUgPSBsb2NhdGlvbi5wYXRobmFtZSxcbiAgICAgIHNlYXJjaCA9IGxvY2F0aW9uLnNlYXJjaCxcbiAgICAgIGhhc2ggPSBsb2NhdGlvbi5oYXNoO1xuXG5cbiAgdmFyIHBhdGggPSBwYXRobmFtZSB8fCAnLyc7XG5cbiAgaWYgKHNlYXJjaCAmJiBzZWFyY2ggIT09ICc/JykgcGF0aCArPSBzZWFyY2guY2hhckF0KDApID09PSAnPycgPyBzZWFyY2ggOiAnPycgKyBzZWFyY2g7XG5cbiAgaWYgKGhhc2ggJiYgaGFzaCAhPT0gJyMnKSBwYXRoICs9IGhhc2guY2hhckF0KDApID09PSAnIycgPyBoYXNoIDogJyMnICsgaGFzaDtcblxuICByZXR1cm4gcGF0aDtcbn07IiwiJ3VzZSBzdHJpY3QnO1xuXG5leHBvcnRzLl9fZXNNb2R1bGUgPSB0cnVlO1xuXG52YXIgX3R5cGVvZiA9IHR5cGVvZiBTeW1ib2wgPT09IFwiZnVuY3Rpb25cIiAmJiB0eXBlb2YgU3ltYm9sLml0ZXJhdG9yID09PSBcInN5bWJvbFwiID8gZnVuY3Rpb24gKG9iaikgeyByZXR1cm4gdHlwZW9mIG9iajsgfSA6IGZ1bmN0aW9uIChvYmopIHsgcmV0dXJuIG9iaiAmJiB0eXBlb2YgU3ltYm9sID09PSBcImZ1bmN0aW9uXCIgJiYgb2JqLmNvbnN0cnVjdG9yID09PSBTeW1ib2wgJiYgb2JqICE9PSBTeW1ib2wucHJvdG90eXBlID8gXCJzeW1ib2xcIiA6IHR5cGVvZiBvYmo7IH07XG5cbnZhciBfZXh0ZW5kcyA9IE9iamVjdC5hc3NpZ24gfHwgZnVuY3Rpb24gKHRhcmdldCkgeyBmb3IgKHZhciBpID0gMTsgaSA8IGFyZ3VtZW50cy5sZW5ndGg7IGkrKykgeyB2YXIgc291cmNlID0gYXJndW1lbnRzW2ldOyBmb3IgKHZhciBrZXkgaW4gc291cmNlKSB7IGlmIChPYmplY3QucHJvdG90eXBlLmhhc093blByb3BlcnR5LmNhbGwoc291cmNlLCBrZXkpKSB7IHRhcmdldFtrZXldID0gc291cmNlW2tleV07IH0gfSB9IHJldHVybiB0YXJnZXQ7IH07XG5cbnZhciBfd2FybmluZyA9IHJlcXVpcmUoJ3dhcm5pbmcnKTtcblxudmFyIF93YXJuaW5nMiA9IF9pbnRlcm9wUmVxdWlyZURlZmF1bHQoX3dhcm5pbmcpO1xuXG52YXIgX2ludmFyaWFudCA9IHJlcXVpcmUoJ2ludmFyaWFudCcpO1xuXG52YXIgX2ludmFyaWFudDIgPSBfaW50ZXJvcFJlcXVpcmVEZWZhdWx0KF9pbnZhcmlhbnQpO1xuXG52YXIgX0xvY2F0aW9uVXRpbHMgPSByZXF1aXJlKCcuL0xvY2F0aW9uVXRpbHMnKTtcblxudmFyIF9QYXRoVXRpbHMgPSByZXF1aXJlKCcuL1BhdGhVdGlscycpO1xuXG52YXIgX2NyZWF0ZVRyYW5zaXRpb25NYW5hZ2VyID0gcmVxdWlyZSgnLi9jcmVhdGVUcmFuc2l0aW9uTWFuYWdlcicpO1xuXG52YXIgX2NyZWF0ZVRyYW5zaXRpb25NYW5hZ2VyMiA9IF9pbnRlcm9wUmVxdWlyZURlZmF1bHQoX2NyZWF0ZVRyYW5zaXRpb25NYW5hZ2VyKTtcblxudmFyIF9ET01VdGlscyA9IHJlcXVpcmUoJy4vRE9NVXRpbHMnKTtcblxuZnVuY3Rpb24gX2ludGVyb3BSZXF1aXJlRGVmYXVsdChvYmopIHsgcmV0dXJuIG9iaiAmJiBvYmouX19lc01vZHVsZSA/IG9iaiA6IHsgZGVmYXVsdDogb2JqIH07IH1cblxudmFyIFBvcFN0YXRlRXZlbnQgPSAncG9wc3RhdGUnO1xudmFyIEhhc2hDaGFuZ2VFdmVudCA9ICdoYXNoY2hhbmdlJztcblxudmFyIGdldEhpc3RvcnlTdGF0ZSA9IGZ1bmN0aW9uIGdldEhpc3RvcnlTdGF0ZSgpIHtcbiAgdHJ5IHtcbiAgICByZXR1cm4gd2luZG93Lmhpc3Rvcnkuc3RhdGUgfHwge307XG4gIH0gY2F0Y2ggKGUpIHtcbiAgICAvLyBJRSAxMSBzb21ldGltZXMgdGhyb3dzIHdoZW4gYWNjZXNzaW5nIHdpbmRvdy5oaXN0b3J5LnN0YXRlXG4gICAgLy8gU2VlIGh0dHBzOi8vZ2l0aHViLmNvbS9SZWFjdFRyYWluaW5nL2hpc3RvcnkvcHVsbC8yODlcbiAgICByZXR1cm4ge307XG4gIH1cbn07XG5cbi8qKlxuICogQ3JlYXRlcyBhIGhpc3Rvcnkgb2JqZWN0IHRoYXQgdXNlcyB0aGUgSFRNTDUgaGlzdG9yeSBBUEkgaW5jbHVkaW5nXG4gKiBwdXNoU3RhdGUsIHJlcGxhY2VTdGF0ZSwgYW5kIHRoZSBwb3BzdGF0ZSBldmVudC5cbiAqL1xudmFyIGNyZWF0ZUJyb3dzZXJIaXN0b3J5ID0gZnVuY3Rpb24gY3JlYXRlQnJvd3Nlckhpc3RvcnkoKSB7XG4gIHZhciBwcm9wcyA9IGFyZ3VtZW50cy5sZW5ndGggPiAwICYmIGFyZ3VtZW50c1swXSAhPT0gdW5kZWZpbmVkID8gYXJndW1lbnRzWzBdIDoge307XG5cbiAgKDAsIF9pbnZhcmlhbnQyLmRlZmF1bHQpKF9ET01VdGlscy5jYW5Vc2VET00sICdCcm93c2VyIGhpc3RvcnkgbmVlZHMgYSBET00nKTtcblxuICB2YXIgZ2xvYmFsSGlzdG9yeSA9IHdpbmRvdy5oaXN0b3J5O1xuICB2YXIgY2FuVXNlSGlzdG9yeSA9ICgwLCBfRE9NVXRpbHMuc3VwcG9ydHNIaXN0b3J5KSgpO1xuICB2YXIgbmVlZHNIYXNoQ2hhbmdlTGlzdGVuZXIgPSAhKDAsIF9ET01VdGlscy5zdXBwb3J0c1BvcFN0YXRlT25IYXNoQ2hhbmdlKSgpO1xuXG4gIHZhciBfcHJvcHMkZm9yY2VSZWZyZXNoID0gcHJvcHMuZm9yY2VSZWZyZXNoLFxuICAgICAgZm9yY2VSZWZyZXNoID0gX3Byb3BzJGZvcmNlUmVmcmVzaCA9PT0gdW5kZWZpbmVkID8gZmFsc2UgOiBfcHJvcHMkZm9yY2VSZWZyZXNoLFxuICAgICAgX3Byb3BzJGdldFVzZXJDb25maXJtID0gcHJvcHMuZ2V0VXNlckNvbmZpcm1hdGlvbixcbiAgICAgIGdldFVzZXJDb25maXJtYXRpb24gPSBfcHJvcHMkZ2V0VXNlckNvbmZpcm0gPT09IHVuZGVmaW5lZCA/IF9ET01VdGlscy5nZXRDb25maXJtYXRpb24gOiBfcHJvcHMkZ2V0VXNlckNvbmZpcm0sXG4gICAgICBfcHJvcHMka2V5TGVuZ3RoID0gcHJvcHMua2V5TGVuZ3RoLFxuICAgICAga2V5TGVuZ3RoID0gX3Byb3BzJGtleUxlbmd0aCA9PT0gdW5kZWZpbmVkID8gNiA6IF9wcm9wcyRrZXlMZW5ndGg7XG5cbiAgdmFyIGJhc2VuYW1lID0gcHJvcHMuYmFzZW5hbWUgPyAoMCwgX1BhdGhVdGlscy5zdHJpcFRyYWlsaW5nU2xhc2gpKCgwLCBfUGF0aFV0aWxzLmFkZExlYWRpbmdTbGFzaCkocHJvcHMuYmFzZW5hbWUpKSA6ICcnO1xuXG4gIHZhciBnZXRET01Mb2NhdGlvbiA9IGZ1bmN0aW9uIGdldERPTUxvY2F0aW9uKGhpc3RvcnlTdGF0ZSkge1xuICAgIHZhciBfcmVmID0gaGlzdG9yeVN0YXRlIHx8IHt9LFxuICAgICAgICBrZXkgPSBfcmVmLmtleSxcbiAgICAgICAgc3RhdGUgPSBfcmVmLnN0YXRlO1xuXG4gICAgdmFyIF93aW5kb3ckbG9jYXRpb24gPSB3aW5kb3cubG9jYXRpb24sXG4gICAgICAgIHBhdGhuYW1lID0gX3dpbmRvdyRsb2NhdGlvbi5wYXRobmFtZSxcbiAgICAgICAgc2VhcmNoID0gX3dpbmRvdyRsb2NhdGlvbi5zZWFyY2gsXG4gICAgICAgIGhhc2ggPSBfd2luZG93JGxvY2F0aW9uLmhhc2g7XG5cblxuICAgIHZhciBwYXRoID0gcGF0aG5hbWUgKyBzZWFyY2ggKyBoYXNoO1xuXG4gICAgKDAsIF93YXJuaW5nMi5kZWZhdWx0KSghYmFzZW5hbWUgfHwgKDAsIF9QYXRoVXRpbHMuaGFzQmFzZW5hbWUpKHBhdGgsIGJhc2VuYW1lKSwgJ1lvdSBhcmUgYXR0ZW1wdGluZyB0byB1c2UgYSBiYXNlbmFtZSBvbiBhIHBhZ2Ugd2hvc2UgVVJMIHBhdGggZG9lcyBub3QgYmVnaW4gJyArICd3aXRoIHRoZSBiYXNlbmFtZS4gRXhwZWN0ZWQgcGF0aCBcIicgKyBwYXRoICsgJ1wiIHRvIGJlZ2luIHdpdGggXCInICsgYmFzZW5hbWUgKyAnXCIuJyk7XG5cbiAgICBpZiAoYmFzZW5hbWUpIHBhdGggPSAoMCwgX1BhdGhVdGlscy5zdHJpcEJhc2VuYW1lKShwYXRoLCBiYXNlbmFtZSk7XG5cbiAgICByZXR1cm4gKDAsIF9Mb2NhdGlvblV0aWxzLmNyZWF0ZUxvY2F0aW9uKShwYXRoLCBzdGF0ZSwga2V5KTtcbiAgfTtcblxuICB2YXIgY3JlYXRlS2V5ID0gZnVuY3Rpb24gY3JlYXRlS2V5KCkge1xuICAgIHJldHVybiBNYXRoLnJhbmRvbSgpLnRvU3RyaW5nKDM2KS5zdWJzdHIoMiwga2V5TGVuZ3RoKTtcbiAgfTtcblxuICB2YXIgdHJhbnNpdGlvbk1hbmFnZXIgPSAoMCwgX2NyZWF0ZVRyYW5zaXRpb25NYW5hZ2VyMi5kZWZhdWx0KSgpO1xuXG4gIHZhciBzZXRTdGF0ZSA9IGZ1bmN0aW9uIHNldFN0YXRlKG5leHRTdGF0ZSkge1xuICAgIF9leHRlbmRzKGhpc3RvcnksIG5leHRTdGF0ZSk7XG5cbiAgICBoaXN0b3J5Lmxlbmd0aCA9IGdsb2JhbEhpc3RvcnkubGVuZ3RoO1xuXG4gICAgdHJhbnNpdGlvbk1hbmFnZXIubm90aWZ5TGlzdGVuZXJzKGhpc3RvcnkubG9jYXRpb24sIGhpc3RvcnkuYWN0aW9uKTtcbiAgfTtcblxuICB2YXIgaGFuZGxlUG9wU3RhdGUgPSBmdW5jdGlvbiBoYW5kbGVQb3BTdGF0ZShldmVudCkge1xuICAgIC8vIElnbm9yZSBleHRyYW5lb3VzIHBvcHN0YXRlIGV2ZW50cyBpbiBXZWJLaXQuXG4gICAgaWYgKCgwLCBfRE9NVXRpbHMuaXNFeHRyYW5lb3VzUG9wc3RhdGVFdmVudCkoZXZlbnQpKSByZXR1cm47XG5cbiAgICBoYW5kbGVQb3AoZ2V0RE9NTG9jYXRpb24oZXZlbnQuc3RhdGUpKTtcbiAgfTtcblxuICB2YXIgaGFuZGxlSGFzaENoYW5nZSA9IGZ1bmN0aW9uIGhhbmRsZUhhc2hDaGFuZ2UoKSB7XG4gICAgaGFuZGxlUG9wKGdldERPTUxvY2F0aW9uKGdldEhpc3RvcnlTdGF0ZSgpKSk7XG4gIH07XG5cbiAgdmFyIGZvcmNlTmV4dFBvcCA9IGZhbHNlO1xuXG4gIHZhciBoYW5kbGVQb3AgPSBmdW5jdGlvbiBoYW5kbGVQb3AobG9jYXRpb24pIHtcbiAgICBpZiAoZm9yY2VOZXh0UG9wKSB7XG4gICAgICBmb3JjZU5leHRQb3AgPSBmYWxzZTtcbiAgICAgIHNldFN0YXRlKCk7XG4gICAgfSBlbHNlIHtcbiAgICAgIHZhciBhY3Rpb24gPSAnUE9QJztcblxuICAgICAgdHJhbnNpdGlvbk1hbmFnZXIuY29uZmlybVRyYW5zaXRpb25Ubyhsb2NhdGlvbiwgYWN0aW9uLCBnZXRVc2VyQ29uZmlybWF0aW9uLCBmdW5jdGlvbiAob2spIHtcbiAgICAgICAgaWYgKG9rKSB7XG4gICAgICAgICAgc2V0U3RhdGUoeyBhY3Rpb246IGFjdGlvbiwgbG9jYXRpb246IGxvY2F0aW9uIH0pO1xuICAgICAgICB9IGVsc2Uge1xuICAgICAgICAgIHJldmVydFBvcChsb2NhdGlvbik7XG4gICAgICAgIH1cbiAgICAgIH0pO1xuICAgIH1cbiAgfTtcblxuICB2YXIgcmV2ZXJ0UG9wID0gZnVuY3Rpb24gcmV2ZXJ0UG9wKGZyb21Mb2NhdGlvbikge1xuICAgIHZhciB0b0xvY2F0aW9uID0gaGlzdG9yeS5sb2NhdGlvbjtcblxuICAgIC8vIFRPRE86IFdlIGNvdWxkIHByb2JhYmx5IG1ha2UgdGhpcyBtb3JlIHJlbGlhYmxlIGJ5XG4gICAgLy8ga2VlcGluZyBhIGxpc3Qgb2Yga2V5cyB3ZSd2ZSBzZWVuIGluIHNlc3Npb25TdG9yYWdlLlxuICAgIC8vIEluc3RlYWQsIHdlIGp1c3QgZGVmYXVsdCB0byAwIGZvciBrZXlzIHdlIGRvbid0IGtub3cuXG5cbiAgICB2YXIgdG9JbmRleCA9IGFsbEtleXMuaW5kZXhPZih0b0xvY2F0aW9uLmtleSk7XG5cbiAgICBpZiAodG9JbmRleCA9PT0gLTEpIHRvSW5kZXggPSAwO1xuXG4gICAgdmFyIGZyb21JbmRleCA9IGFsbEtleXMuaW5kZXhPZihmcm9tTG9jYXRpb24ua2V5KTtcblxuICAgIGlmIChmcm9tSW5kZXggPT09IC0xKSBmcm9tSW5kZXggPSAwO1xuXG4gICAgdmFyIGRlbHRhID0gdG9JbmRleCAtIGZyb21JbmRleDtcblxuICAgIGlmIChkZWx0YSkge1xuICAgICAgZm9yY2VOZXh0UG9wID0gdHJ1ZTtcbiAgICAgIGdvKGRlbHRhKTtcbiAgICB9XG4gIH07XG5cbiAgdmFyIGluaXRpYWxMb2NhdGlvbiA9IGdldERPTUxvY2F0aW9uKGdldEhpc3RvcnlTdGF0ZSgpKTtcbiAgdmFyIGFsbEtleXMgPSBbaW5pdGlhbExvY2F0aW9uLmtleV07XG5cbiAgLy8gUHVibGljIGludGVyZmFjZVxuXG4gIHZhciBjcmVhdGVIcmVmID0gZnVuY3Rpb24gY3JlYXRlSHJlZihsb2NhdGlvbikge1xuICAgIHJldHVybiBiYXNlbmFtZSArICgwLCBfUGF0aFV0aWxzLmNyZWF0ZVBhdGgpKGxvY2F0aW9uKTtcbiAgfTtcblxuICB2YXIgcHVzaCA9IGZ1bmN0aW9uIHB1c2gocGF0aCwgc3RhdGUpIHtcbiAgICAoMCwgX3dhcm5pbmcyLmRlZmF1bHQpKCEoKHR5cGVvZiBwYXRoID09PSAndW5kZWZpbmVkJyA/ICd1bmRlZmluZWQnIDogX3R5cGVvZihwYXRoKSkgPT09ICdvYmplY3QnICYmIHBhdGguc3RhdGUgIT09IHVuZGVmaW5lZCAmJiBzdGF0ZSAhPT0gdW5kZWZpbmVkKSwgJ1lvdSBzaG91bGQgYXZvaWQgcHJvdmlkaW5nIGEgMm5kIHN0YXRlIGFyZ3VtZW50IHRvIHB1c2ggd2hlbiB0aGUgMXN0ICcgKyAnYXJndW1lbnQgaXMgYSBsb2NhdGlvbi1saWtlIG9iamVjdCB0aGF0IGFscmVhZHkgaGFzIHN0YXRlOyBpdCBpcyBpZ25vcmVkJyk7XG5cbiAgICB2YXIgYWN0aW9uID0gJ1BVU0gnO1xuICAgIHZhciBsb2NhdGlvbiA9ICgwLCBfTG9jYXRpb25VdGlscy5jcmVhdGVMb2NhdGlvbikocGF0aCwgc3RhdGUsIGNyZWF0ZUtleSgpLCBoaXN0b3J5LmxvY2F0aW9uKTtcblxuICAgIHRyYW5zaXRpb25NYW5hZ2VyLmNvbmZpcm1UcmFuc2l0aW9uVG8obG9jYXRpb24sIGFjdGlvbiwgZ2V0VXNlckNvbmZpcm1hdGlvbiwgZnVuY3Rpb24gKG9rKSB7XG4gICAgICBpZiAoIW9rKSByZXR1cm47XG5cbiAgICAgIHZhciBocmVmID0gY3JlYXRlSHJlZihsb2NhdGlvbik7XG4gICAgICB2YXIga2V5ID0gbG9jYXRpb24ua2V5LFxuICAgICAgICAgIHN0YXRlID0gbG9jYXRpb24uc3RhdGU7XG5cblxuICAgICAgaWYgKGNhblVzZUhpc3RvcnkpIHtcbiAgICAgICAgZ2xvYmFsSGlzdG9yeS5wdXNoU3RhdGUoeyBrZXk6IGtleSwgc3RhdGU6IHN0YXRlIH0sIG51bGwsIGhyZWYpO1xuXG4gICAgICAgIGlmIChmb3JjZVJlZnJlc2gpIHtcbiAgICAgICAgICB3aW5kb3cubG9jYXRpb24uaHJlZiA9IGhyZWY7XG4gICAgICAgIH0gZWxzZSB7XG4gICAgICAgICAgdmFyIHByZXZJbmRleCA9IGFsbEtleXMuaW5kZXhPZihoaXN0b3J5LmxvY2F0aW9uLmtleSk7XG4gICAgICAgICAgdmFyIG5leHRLZXlzID0gYWxsS2V5cy5zbGljZSgwLCBwcmV2SW5kZXggPT09IC0xID8gMCA6IHByZXZJbmRleCArIDEpO1xuXG4gICAgICAgICAgbmV4dEtleXMucHVzaChsb2NhdGlvbi5rZXkpO1xuICAgICAgICAgIGFsbEtleXMgPSBuZXh0S2V5cztcblxuICAgICAgICAgIHNldFN0YXRlKHsgYWN0aW9uOiBhY3Rpb24sIGxvY2F0aW9uOiBsb2NhdGlvbiB9KTtcbiAgICAgICAgfVxuICAgICAgfSBlbHNlIHtcbiAgICAgICAgKDAsIF93YXJuaW5nMi5kZWZhdWx0KShzdGF0ZSA9PT0gdW5kZWZpbmVkLCAnQnJvd3NlciBoaXN0b3J5IGNhbm5vdCBwdXNoIHN0YXRlIGluIGJyb3dzZXJzIHRoYXQgZG8gbm90IHN1cHBvcnQgSFRNTDUgaGlzdG9yeScpO1xuXG4gICAgICAgIHdpbmRvdy5sb2NhdGlvbi5ocmVmID0gaHJlZjtcbiAgICAgIH1cbiAgICB9KTtcbiAgfTtcblxuICB2YXIgcmVwbGFjZSA9IGZ1bmN0aW9uIHJlcGxhY2UocGF0aCwgc3RhdGUpIHtcbiAgICAoMCwgX3dhcm5pbmcyLmRlZmF1bHQpKCEoKHR5cGVvZiBwYXRoID09PSAndW5kZWZpbmVkJyA/ICd1bmRlZmluZWQnIDogX3R5cGVvZihwYXRoKSkgPT09ICdvYmplY3QnICYmIHBhdGguc3RhdGUgIT09IHVuZGVmaW5lZCAmJiBzdGF0ZSAhPT0gdW5kZWZpbmVkKSwgJ1lvdSBzaG91bGQgYXZvaWQgcHJvdmlkaW5nIGEgMm5kIHN0YXRlIGFyZ3VtZW50IHRvIHJlcGxhY2Ugd2hlbiB0aGUgMXN0ICcgKyAnYXJndW1lbnQgaXMgYSBsb2NhdGlvbi1saWtlIG9iamVjdCB0aGF0IGFscmVhZHkgaGFzIHN0YXRlOyBpdCBpcyBpZ25vcmVkJyk7XG5cbiAgICB2YXIgYWN0aW9uID0gJ1JFUExBQ0UnO1xuICAgIHZhciBsb2NhdGlvbiA9ICgwLCBfTG9jYXRpb25VdGlscy5jcmVhdGVMb2NhdGlvbikocGF0aCwgc3RhdGUsIGNyZWF0ZUtleSgpLCBoaXN0b3J5LmxvY2F0aW9uKTtcblxuICAgIHRyYW5zaXRpb25NYW5hZ2VyLmNvbmZpcm1UcmFuc2l0aW9uVG8obG9jYXRpb24sIGFjdGlvbiwgZ2V0VXNlckNvbmZpcm1hdGlvbiwgZnVuY3Rpb24gKG9rKSB7XG4gICAgICBpZiAoIW9rKSByZXR1cm47XG5cbiAgICAgIHZhciBocmVmID0gY3JlYXRlSHJlZihsb2NhdGlvbik7XG4gICAgICB2YXIga2V5ID0gbG9jYXRpb24ua2V5LFxuICAgICAgICAgIHN0YXRlID0gbG9jYXRpb24uc3RhdGU7XG5cblxuICAgICAgaWYgKGNhblVzZUhpc3RvcnkpIHtcbiAgICAgICAgZ2xvYmFsSGlzdG9yeS5yZXBsYWNlU3RhdGUoeyBrZXk6IGtleSwgc3RhdGU6IHN0YXRlIH0sIG51bGwsIGhyZWYpO1xuXG4gICAgICAgIGlmIChmb3JjZVJlZnJlc2gpIHtcbiAgICAgICAgICB3aW5kb3cubG9jYXRpb24ucmVwbGFjZShocmVmKTtcbiAgICAgICAgfSBlbHNlIHtcbiAgICAgICAgICB2YXIgcHJldkluZGV4ID0gYWxsS2V5cy5pbmRleE9mKGhpc3RvcnkubG9jYXRpb24ua2V5KTtcblxuICAgICAgICAgIGlmIChwcmV2SW5kZXggIT09IC0xKSBhbGxLZXlzW3ByZXZJbmRleF0gPSBsb2NhdGlvbi5rZXk7XG5cbiAgICAgICAgICBzZXRTdGF0ZSh7IGFjdGlvbjogYWN0aW9uLCBsb2NhdGlvbjogbG9jYXRpb24gfSk7XG4gICAgICAgIH1cbiAgICAgIH0gZWxzZSB7XG4gICAgICAgICgwLCBfd2FybmluZzIuZGVmYXVsdCkoc3RhdGUgPT09IHVuZGVmaW5lZCwgJ0Jyb3dzZXIgaGlzdG9yeSBjYW5ub3QgcmVwbGFjZSBzdGF0ZSBpbiBicm93c2VycyB0aGF0IGRvIG5vdCBzdXBwb3J0IEhUTUw1IGhpc3RvcnknKTtcblxuICAgICAgICB3aW5kb3cubG9jYXRpb24ucmVwbGFjZShocmVmKTtcbiAgICAgIH1cbiAgICB9KTtcbiAgfTtcblxuICB2YXIgZ28gPSBmdW5jdGlvbiBnbyhuKSB7XG4gICAgZ2xvYmFsSGlzdG9yeS5nbyhuKTtcbiAgfTtcblxuICB2YXIgZ29CYWNrID0gZnVuY3Rpb24gZ29CYWNrKCkge1xuICAgIHJldHVybiBnbygtMSk7XG4gIH07XG5cbiAgdmFyIGdvRm9yd2FyZCA9IGZ1bmN0aW9uIGdvRm9yd2FyZCgpIHtcbiAgICByZXR1cm4gZ28oMSk7XG4gIH07XG5cbiAgdmFyIGxpc3RlbmVyQ291bnQgPSAwO1xuXG4gIHZhciBjaGVja0RPTUxpc3RlbmVycyA9IGZ1bmN0aW9uIGNoZWNrRE9NTGlzdGVuZXJzKGRlbHRhKSB7XG4gICAgbGlzdGVuZXJDb3VudCArPSBkZWx0YTtcblxuICAgIGlmIChsaXN0ZW5lckNvdW50ID09PSAxKSB7XG4gICAgICAoMCwgX0RPTVV0aWxzLmFkZEV2ZW50TGlzdGVuZXIpKHdpbmRvdywgUG9wU3RhdGVFdmVudCwgaGFuZGxlUG9wU3RhdGUpO1xuXG4gICAgICBpZiAobmVlZHNIYXNoQ2hhbmdlTGlzdGVuZXIpICgwLCBfRE9NVXRpbHMuYWRkRXZlbnRMaXN0ZW5lcikod2luZG93LCBIYXNoQ2hhbmdlRXZlbnQsIGhhbmRsZUhhc2hDaGFuZ2UpO1xuICAgIH0gZWxzZSBpZiAobGlzdGVuZXJDb3VudCA9PT0gMCkge1xuICAgICAgKDAsIF9ET01VdGlscy5yZW1vdmVFdmVudExpc3RlbmVyKSh3aW5kb3csIFBvcFN0YXRlRXZlbnQsIGhhbmRsZVBvcFN0YXRlKTtcblxuICAgICAgaWYgKG5lZWRzSGFzaENoYW5nZUxpc3RlbmVyKSAoMCwgX0RPTVV0aWxzLnJlbW92ZUV2ZW50TGlzdGVuZXIpKHdpbmRvdywgSGFzaENoYW5nZUV2ZW50LCBoYW5kbGVIYXNoQ2hhbmdlKTtcbiAgICB9XG4gIH07XG5cbiAgdmFyIGlzQmxvY2tlZCA9IGZhbHNlO1xuXG4gIHZhciBibG9jayA9IGZ1bmN0aW9uIGJsb2NrKCkge1xuICAgIHZhciBwcm9tcHQgPSBhcmd1bWVudHMubGVuZ3RoID4gMCAmJiBhcmd1bWVudHNbMF0gIT09IHVuZGVmaW5lZCA/IGFyZ3VtZW50c1swXSA6IGZhbHNlO1xuXG4gICAgdmFyIHVuYmxvY2sgPSB0cmFuc2l0aW9uTWFuYWdlci5zZXRQcm9tcHQocHJvbXB0KTtcblxuICAgIGlmICghaXNCbG9ja2VkKSB7XG4gICAgICBjaGVja0RPTUxpc3RlbmVycygxKTtcbiAgICAgIGlzQmxvY2tlZCA9IHRydWU7XG4gICAgfVxuXG4gICAgcmV0dXJuIGZ1bmN0aW9uICgpIHtcbiAgICAgIGlmIChpc0Jsb2NrZWQpIHtcbiAgICAgICAgaXNCbG9ja2VkID0gZmFsc2U7XG4gICAgICAgIGNoZWNrRE9NTGlzdGVuZXJzKC0xKTtcbiAgICAgIH1cblxuICAgICAgcmV0dXJuIHVuYmxvY2soKTtcbiAgICB9O1xuICB9O1xuXG4gIHZhciBsaXN0ZW4gPSBmdW5jdGlvbiBsaXN0ZW4obGlzdGVuZXIpIHtcbiAgICB2YXIgdW5saXN0ZW4gPSB0cmFuc2l0aW9uTWFuYWdlci5hcHBlbmRMaXN0ZW5lcihsaXN0ZW5lcik7XG4gICAgY2hlY2tET01MaXN0ZW5lcnMoMSk7XG5cbiAgICByZXR1cm4gZnVuY3Rpb24gKCkge1xuICAgICAgY2hlY2tET01MaXN0ZW5lcnMoLTEpO1xuICAgICAgdW5saXN0ZW4oKTtcbiAgICB9O1xuICB9O1xuXG4gIHZhciBoaXN0b3J5ID0ge1xuICAgIGxlbmd0aDogZ2xvYmFsSGlzdG9yeS5sZW5ndGgsXG4gICAgYWN0aW9uOiAnUE9QJyxcbiAgICBsb2NhdGlvbjogaW5pdGlhbExvY2F0aW9uLFxuICAgIGNyZWF0ZUhyZWY6IGNyZWF0ZUhyZWYsXG4gICAgcHVzaDogcHVzaCxcbiAgICByZXBsYWNlOiByZXBsYWNlLFxuICAgIGdvOiBnbyxcbiAgICBnb0JhY2s6IGdvQmFjayxcbiAgICBnb0ZvcndhcmQ6IGdvRm9yd2FyZCxcbiAgICBibG9jazogYmxvY2ssXG4gICAgbGlzdGVuOiBsaXN0ZW5cbiAgfTtcblxuICByZXR1cm4gaGlzdG9yeTtcbn07XG5cbmV4cG9ydHMuZGVmYXVsdCA9IGNyZWF0ZUJyb3dzZXJIaXN0b3J5OyIsIid1c2Ugc3RyaWN0JztcblxuZXhwb3J0cy5fX2VzTW9kdWxlID0gdHJ1ZTtcblxudmFyIF93YXJuaW5nID0gcmVxdWlyZSgnd2FybmluZycpO1xuXG52YXIgX3dhcm5pbmcyID0gX2ludGVyb3BSZXF1aXJlRGVmYXVsdChfd2FybmluZyk7XG5cbmZ1bmN0aW9uIF9pbnRlcm9wUmVxdWlyZURlZmF1bHQob2JqKSB7IHJldHVybiBvYmogJiYgb2JqLl9fZXNNb2R1bGUgPyBvYmogOiB7IGRlZmF1bHQ6IG9iaiB9OyB9XG5cbnZhciBjcmVhdGVUcmFuc2l0aW9uTWFuYWdlciA9IGZ1bmN0aW9uIGNyZWF0ZVRyYW5zaXRpb25NYW5hZ2VyKCkge1xuICB2YXIgcHJvbXB0ID0gbnVsbDtcblxuICB2YXIgc2V0UHJvbXB0ID0gZnVuY3Rpb24gc2V0UHJvbXB0KG5leHRQcm9tcHQpIHtcbiAgICAoMCwgX3dhcm5pbmcyLmRlZmF1bHQpKHByb21wdCA9PSBudWxsLCAnQSBoaXN0b3J5IHN1cHBvcnRzIG9ubHkgb25lIHByb21wdCBhdCBhIHRpbWUnKTtcblxuICAgIHByb21wdCA9IG5leHRQcm9tcHQ7XG5cbiAgICByZXR1cm4gZnVuY3Rpb24gKCkge1xuICAgICAgaWYgKHByb21wdCA9PT0gbmV4dFByb21wdCkgcHJvbXB0ID0gbnVsbDtcbiAgICB9O1xuICB9O1xuXG4gIHZhciBjb25maXJtVHJhbnNpdGlvblRvID0gZnVuY3Rpb24gY29uZmlybVRyYW5zaXRpb25Ubyhsb2NhdGlvbiwgYWN0aW9uLCBnZXRVc2VyQ29uZmlybWF0aW9uLCBjYWxsYmFjaykge1xuICAgIC8vIFRPRE86IElmIGFub3RoZXIgdHJhbnNpdGlvbiBzdGFydHMgd2hpbGUgd2UncmUgc3RpbGwgY29uZmlybWluZ1xuICAgIC8vIHRoZSBwcmV2aW91cyBvbmUsIHdlIG1heSBlbmQgdXAgaW4gYSB3ZWlyZCBzdGF0ZS4gRmlndXJlIG91dCB0aGVcbiAgICAvLyBiZXN0IHdheSB0byBoYW5kbGUgdGhpcy5cbiAgICBpZiAocHJvbXB0ICE9IG51bGwpIHtcbiAgICAgIHZhciByZXN1bHQgPSB0eXBlb2YgcHJvbXB0ID09PSAnZnVuY3Rpb24nID8gcHJvbXB0KGxvY2F0aW9uLCBhY3Rpb24pIDogcHJvbXB0O1xuXG4gICAgICBpZiAodHlwZW9mIHJlc3VsdCA9PT0gJ3N0cmluZycpIHtcbiAgICAgICAgaWYgKHR5cGVvZiBnZXRVc2VyQ29uZmlybWF0aW9uID09PSAnZnVuY3Rpb24nKSB7XG4gICAgICAgICAgZ2V0VXNlckNvbmZpcm1hdGlvbihyZXN1bHQsIGNhbGxiYWNrKTtcbiAgICAgICAgfSBlbHNlIHtcbiAgICAgICAgICAoMCwgX3dhcm5pbmcyLmRlZmF1bHQpKGZhbHNlLCAnQSBoaXN0b3J5IG5lZWRzIGEgZ2V0VXNlckNvbmZpcm1hdGlvbiBmdW5jdGlvbiBpbiBvcmRlciB0byB1c2UgYSBwcm9tcHQgbWVzc2FnZScpO1xuXG4gICAgICAgICAgY2FsbGJhY2sodHJ1ZSk7XG4gICAgICAgIH1cbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIC8vIFJldHVybiBmYWxzZSBmcm9tIGEgdHJhbnNpdGlvbiBob29rIHRvIGNhbmNlbCB0aGUgdHJhbnNpdGlvbi5cbiAgICAgICAgY2FsbGJhY2socmVzdWx0ICE9PSBmYWxzZSk7XG4gICAgICB9XG4gICAgfSBlbHNlIHtcbiAgICAgIGNhbGxiYWNrKHRydWUpO1xuICAgIH1cbiAgfTtcblxuICB2YXIgbGlzdGVuZXJzID0gW107XG5cbiAgdmFyIGFwcGVuZExpc3RlbmVyID0gZnVuY3Rpb24gYXBwZW5kTGlzdGVuZXIoZm4pIHtcbiAgICB2YXIgaXNBY3RpdmUgPSB0cnVlO1xuXG4gICAgdmFyIGxpc3RlbmVyID0gZnVuY3Rpb24gbGlzdGVuZXIoKSB7XG4gICAgICBpZiAoaXNBY3RpdmUpIGZuLmFwcGx5KHVuZGVmaW5lZCwgYXJndW1lbnRzKTtcbiAgICB9O1xuXG4gICAgbGlzdGVuZXJzLnB1c2gobGlzdGVuZXIpO1xuXG4gICAgcmV0dXJuIGZ1bmN0aW9uICgpIHtcbiAgICAgIGlzQWN0aXZlID0gZmFsc2U7XG4gICAgICBsaXN0ZW5lcnMgPSBsaXN0ZW5lcnMuZmlsdGVyKGZ1bmN0aW9uIChpdGVtKSB7XG4gICAgICAgIHJldHVybiBpdGVtICE9PSBsaXN0ZW5lcjtcbiAgICAgIH0pO1xuICAgIH07XG4gIH07XG5cbiAgdmFyIG5vdGlmeUxpc3RlbmVycyA9IGZ1bmN0aW9uIG5vdGlmeUxpc3RlbmVycygpIHtcbiAgICBmb3IgKHZhciBfbGVuID0gYXJndW1lbnRzLmxlbmd0aCwgYXJncyA9IEFycmF5KF9sZW4pLCBfa2V5ID0gMDsgX2tleSA8IF9sZW47IF9rZXkrKykge1xuICAgICAgYXJnc1tfa2V5XSA9IGFyZ3VtZW50c1tfa2V5XTtcbiAgICB9XG5cbiAgICBsaXN0ZW5lcnMuZm9yRWFjaChmdW5jdGlvbiAobGlzdGVuZXIpIHtcbiAgICAgIHJldHVybiBsaXN0ZW5lci5hcHBseSh1bmRlZmluZWQsIGFyZ3MpO1xuICAgIH0pO1xuICB9O1xuXG4gIHJldHVybiB7XG4gICAgc2V0UHJvbXB0OiBzZXRQcm9tcHQsXG4gICAgY29uZmlybVRyYW5zaXRpb25UbzogY29uZmlybVRyYW5zaXRpb25UbyxcbiAgICBhcHBlbmRMaXN0ZW5lcjogYXBwZW5kTGlzdGVuZXIsXG4gICAgbm90aWZ5TGlzdGVuZXJzOiBub3RpZnlMaXN0ZW5lcnNcbiAgfTtcbn07XG5cbmV4cG9ydHMuZGVmYXVsdCA9IGNyZWF0ZVRyYW5zaXRpb25NYW5hZ2VyOyIsIi8qKlxuICogQ29weXJpZ2h0IChjKSAyMDEzLXByZXNlbnQsIEZhY2Vib29rLCBJbmMuXG4gKlxuICogVGhpcyBzb3VyY2UgY29kZSBpcyBsaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UgZm91bmQgaW4gdGhlXG4gKiBMSUNFTlNFIGZpbGUgaW4gdGhlIHJvb3QgZGlyZWN0b3J5IG9mIHRoaXMgc291cmNlIHRyZWUuXG4gKi9cblxuJ3VzZSBzdHJpY3QnO1xuXG4vKipcbiAqIFVzZSBpbnZhcmlhbnQoKSB0byBhc3NlcnQgc3RhdGUgd2hpY2ggeW91ciBwcm9ncmFtIGFzc3VtZXMgdG8gYmUgdHJ1ZS5cbiAqXG4gKiBQcm92aWRlIHNwcmludGYtc3R5bGUgZm9ybWF0IChvbmx5ICVzIGlzIHN1cHBvcnRlZCkgYW5kIGFyZ3VtZW50c1xuICogdG8gcHJvdmlkZSBpbmZvcm1hdGlvbiBhYm91dCB3aGF0IGJyb2tlIGFuZCB3aGF0IHlvdSB3ZXJlXG4gKiBleHBlY3RpbmcuXG4gKlxuICogVGhlIGludmFyaWFudCBtZXNzYWdlIHdpbGwgYmUgc3RyaXBwZWQgaW4gcHJvZHVjdGlvbiwgYnV0IHRoZSBpbnZhcmlhbnRcbiAqIHdpbGwgcmVtYWluIHRvIGVuc3VyZSBsb2dpYyBkb2VzIG5vdCBkaWZmZXIgaW4gcHJvZHVjdGlvbi5cbiAqL1xuXG52YXIgaW52YXJpYW50ID0gZnVuY3Rpb24oY29uZGl0aW9uLCBmb3JtYXQsIGEsIGIsIGMsIGQsIGUsIGYpIHtcbiAgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgICBpZiAoZm9ybWF0ID09PSB1bmRlZmluZWQpIHtcbiAgICAgIHRocm93IG5ldyBFcnJvcignaW52YXJpYW50IHJlcXVpcmVzIGFuIGVycm9yIG1lc3NhZ2UgYXJndW1lbnQnKTtcbiAgICB9XG4gIH1cblxuICBpZiAoIWNvbmRpdGlvbikge1xuICAgIHZhciBlcnJvcjtcbiAgICBpZiAoZm9ybWF0ID09PSB1bmRlZmluZWQpIHtcbiAgICAgIGVycm9yID0gbmV3IEVycm9yKFxuICAgICAgICAnTWluaWZpZWQgZXhjZXB0aW9uIG9jY3VycmVkOyB1c2UgdGhlIG5vbi1taW5pZmllZCBkZXYgZW52aXJvbm1lbnQgJyArXG4gICAgICAgICdmb3IgdGhlIGZ1bGwgZXJyb3IgbWVzc2FnZSBhbmQgYWRkaXRpb25hbCBoZWxwZnVsIHdhcm5pbmdzLidcbiAgICAgICk7XG4gICAgfSBlbHNlIHtcbiAgICAgIHZhciBhcmdzID0gW2EsIGIsIGMsIGQsIGUsIGZdO1xuICAgICAgdmFyIGFyZ0luZGV4ID0gMDtcbiAgICAgIGVycm9yID0gbmV3IEVycm9yKFxuICAgICAgICBmb3JtYXQucmVwbGFjZSgvJXMvZywgZnVuY3Rpb24oKSB7IHJldHVybiBhcmdzW2FyZ0luZGV4KytdOyB9KVxuICAgICAgKTtcbiAgICAgIGVycm9yLm5hbWUgPSAnSW52YXJpYW50IFZpb2xhdGlvbic7XG4gICAgfVxuXG4gICAgZXJyb3IuZnJhbWVzVG9Qb3AgPSAxOyAvLyB3ZSBkb24ndCBjYXJlIGFib3V0IGludmFyaWFudCdzIG93biBmcmFtZVxuICAgIHRocm93IGVycm9yO1xuICB9XG59O1xuXG5tb2R1bGUuZXhwb3J0cyA9IGludmFyaWFudDtcbiIsIi8qXG5vYmplY3QtYXNzaWduXG4oYykgU2luZHJlIFNvcmh1c1xuQGxpY2Vuc2UgTUlUXG4qL1xuXG4ndXNlIHN0cmljdCc7XG4vKiBlc2xpbnQtZGlzYWJsZSBuby11bnVzZWQtdmFycyAqL1xudmFyIGdldE93blByb3BlcnR5U3ltYm9scyA9IE9iamVjdC5nZXRPd25Qcm9wZXJ0eVN5bWJvbHM7XG52YXIgaGFzT3duUHJvcGVydHkgPSBPYmplY3QucHJvdG90eXBlLmhhc093blByb3BlcnR5O1xudmFyIHByb3BJc0VudW1lcmFibGUgPSBPYmplY3QucHJvdG90eXBlLnByb3BlcnR5SXNFbnVtZXJhYmxlO1xuXG5mdW5jdGlvbiB0b09iamVjdCh2YWwpIHtcblx0aWYgKHZhbCA9PT0gbnVsbCB8fCB2YWwgPT09IHVuZGVmaW5lZCkge1xuXHRcdHRocm93IG5ldyBUeXBlRXJyb3IoJ09iamVjdC5hc3NpZ24gY2Fubm90IGJlIGNhbGxlZCB3aXRoIG51bGwgb3IgdW5kZWZpbmVkJyk7XG5cdH1cblxuXHRyZXR1cm4gT2JqZWN0KHZhbCk7XG59XG5cbmZ1bmN0aW9uIHNob3VsZFVzZU5hdGl2ZSgpIHtcblx0dHJ5IHtcblx0XHRpZiAoIU9iamVjdC5hc3NpZ24pIHtcblx0XHRcdHJldHVybiBmYWxzZTtcblx0XHR9XG5cblx0XHQvLyBEZXRlY3QgYnVnZ3kgcHJvcGVydHkgZW51bWVyYXRpb24gb3JkZXIgaW4gb2xkZXIgVjggdmVyc2lvbnMuXG5cblx0XHQvLyBodHRwczovL2J1Z3MuY2hyb21pdW0ub3JnL3AvdjgvaXNzdWVzL2RldGFpbD9pZD00MTE4XG5cdFx0dmFyIHRlc3QxID0gbmV3IFN0cmluZygnYWJjJyk7ICAvLyBlc2xpbnQtZGlzYWJsZS1saW5lIG5vLW5ldy13cmFwcGVyc1xuXHRcdHRlc3QxWzVdID0gJ2RlJztcblx0XHRpZiAoT2JqZWN0LmdldE93blByb3BlcnR5TmFtZXModGVzdDEpWzBdID09PSAnNScpIHtcblx0XHRcdHJldHVybiBmYWxzZTtcblx0XHR9XG5cblx0XHQvLyBodHRwczovL2J1Z3MuY2hyb21pdW0ub3JnL3AvdjgvaXNzdWVzL2RldGFpbD9pZD0zMDU2XG5cdFx0dmFyIHRlc3QyID0ge307XG5cdFx0Zm9yICh2YXIgaSA9IDA7IGkgPCAxMDsgaSsrKSB7XG5cdFx0XHR0ZXN0MlsnXycgKyBTdHJpbmcuZnJvbUNoYXJDb2RlKGkpXSA9IGk7XG5cdFx0fVxuXHRcdHZhciBvcmRlcjIgPSBPYmplY3QuZ2V0T3duUHJvcGVydHlOYW1lcyh0ZXN0MikubWFwKGZ1bmN0aW9uIChuKSB7XG5cdFx0XHRyZXR1cm4gdGVzdDJbbl07XG5cdFx0fSk7XG5cdFx0aWYgKG9yZGVyMi5qb2luKCcnKSAhPT0gJzAxMjM0NTY3ODknKSB7XG5cdFx0XHRyZXR1cm4gZmFsc2U7XG5cdFx0fVxuXG5cdFx0Ly8gaHR0cHM6Ly9idWdzLmNocm9taXVtLm9yZy9wL3Y4L2lzc3Vlcy9kZXRhaWw/aWQ9MzA1NlxuXHRcdHZhciB0ZXN0MyA9IHt9O1xuXHRcdCdhYmNkZWZnaGlqa2xtbm9wcXJzdCcuc3BsaXQoJycpLmZvckVhY2goZnVuY3Rpb24gKGxldHRlcikge1xuXHRcdFx0dGVzdDNbbGV0dGVyXSA9IGxldHRlcjtcblx0XHR9KTtcblx0XHRpZiAoT2JqZWN0LmtleXMoT2JqZWN0LmFzc2lnbih7fSwgdGVzdDMpKS5qb2luKCcnKSAhPT1cblx0XHRcdFx0J2FiY2RlZmdoaWprbG1ub3BxcnN0Jykge1xuXHRcdFx0cmV0dXJuIGZhbHNlO1xuXHRcdH1cblxuXHRcdHJldHVybiB0cnVlO1xuXHR9IGNhdGNoIChlcnIpIHtcblx0XHQvLyBXZSBkb24ndCBleHBlY3QgYW55IG9mIHRoZSBhYm92ZSB0byB0aHJvdywgYnV0IGJldHRlciB0byBiZSBzYWZlLlxuXHRcdHJldHVybiBmYWxzZTtcblx0fVxufVxuXG5tb2R1bGUuZXhwb3J0cyA9IHNob3VsZFVzZU5hdGl2ZSgpID8gT2JqZWN0LmFzc2lnbiA6IGZ1bmN0aW9uICh0YXJnZXQsIHNvdXJjZSkge1xuXHR2YXIgZnJvbTtcblx0dmFyIHRvID0gdG9PYmplY3QodGFyZ2V0KTtcblx0dmFyIHN5bWJvbHM7XG5cblx0Zm9yICh2YXIgcyA9IDE7IHMgPCBhcmd1bWVudHMubGVuZ3RoOyBzKyspIHtcblx0XHRmcm9tID0gT2JqZWN0KGFyZ3VtZW50c1tzXSk7XG5cblx0XHRmb3IgKHZhciBrZXkgaW4gZnJvbSkge1xuXHRcdFx0aWYgKGhhc093blByb3BlcnR5LmNhbGwoZnJvbSwga2V5KSkge1xuXHRcdFx0XHR0b1trZXldID0gZnJvbVtrZXldO1xuXHRcdFx0fVxuXHRcdH1cblxuXHRcdGlmIChnZXRPd25Qcm9wZXJ0eVN5bWJvbHMpIHtcblx0XHRcdHN5bWJvbHMgPSBnZXRPd25Qcm9wZXJ0eVN5bWJvbHMoZnJvbSk7XG5cdFx0XHRmb3IgKHZhciBpID0gMDsgaSA8IHN5bWJvbHMubGVuZ3RoOyBpKyspIHtcblx0XHRcdFx0aWYgKHByb3BJc0VudW1lcmFibGUuY2FsbChmcm9tLCBzeW1ib2xzW2ldKSkge1xuXHRcdFx0XHRcdHRvW3N5bWJvbHNbaV1dID0gZnJvbVtzeW1ib2xzW2ldXTtcblx0XHRcdFx0fVxuXHRcdFx0fVxuXHRcdH1cblx0fVxuXG5cdHJldHVybiB0bztcbn07XG4iLCIvKipcbiAqIENvcHlyaWdodCAoYykgMjAxMy1wcmVzZW50LCBGYWNlYm9vaywgSW5jLlxuICpcbiAqIFRoaXMgc291cmNlIGNvZGUgaXMgbGljZW5zZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlIGZvdW5kIGluIHRoZVxuICogTElDRU5TRSBmaWxlIGluIHRoZSByb290IGRpcmVjdG9yeSBvZiB0aGlzIHNvdXJjZSB0cmVlLlxuICovXG5cbid1c2Ugc3RyaWN0JztcblxuaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicpIHtcbiAgdmFyIGludmFyaWFudCA9IHJlcXVpcmUoJ2ZianMvbGliL2ludmFyaWFudCcpO1xuICB2YXIgd2FybmluZyA9IHJlcXVpcmUoJ2ZianMvbGliL3dhcm5pbmcnKTtcbiAgdmFyIFJlYWN0UHJvcFR5cGVzU2VjcmV0ID0gcmVxdWlyZSgnLi9saWIvUmVhY3RQcm9wVHlwZXNTZWNyZXQnKTtcbiAgdmFyIGxvZ2dlZFR5cGVGYWlsdXJlcyA9IHt9O1xufVxuXG4vKipcbiAqIEFzc2VydCB0aGF0IHRoZSB2YWx1ZXMgbWF0Y2ggd2l0aCB0aGUgdHlwZSBzcGVjcy5cbiAqIEVycm9yIG1lc3NhZ2VzIGFyZSBtZW1vcml6ZWQgYW5kIHdpbGwgb25seSBiZSBzaG93biBvbmNlLlxuICpcbiAqIEBwYXJhbSB7b2JqZWN0fSB0eXBlU3BlY3MgTWFwIG9mIG5hbWUgdG8gYSBSZWFjdFByb3BUeXBlXG4gKiBAcGFyYW0ge29iamVjdH0gdmFsdWVzIFJ1bnRpbWUgdmFsdWVzIHRoYXQgbmVlZCB0byBiZSB0eXBlLWNoZWNrZWRcbiAqIEBwYXJhbSB7c3RyaW5nfSBsb2NhdGlvbiBlLmcuIFwicHJvcFwiLCBcImNvbnRleHRcIiwgXCJjaGlsZCBjb250ZXh0XCJcbiAqIEBwYXJhbSB7c3RyaW5nfSBjb21wb25lbnROYW1lIE5hbWUgb2YgdGhlIGNvbXBvbmVudCBmb3IgZXJyb3IgbWVzc2FnZXMuXG4gKiBAcGFyYW0gez9GdW5jdGlvbn0gZ2V0U3RhY2sgUmV0dXJucyB0aGUgY29tcG9uZW50IHN0YWNrLlxuICogQHByaXZhdGVcbiAqL1xuZnVuY3Rpb24gY2hlY2tQcm9wVHlwZXModHlwZVNwZWNzLCB2YWx1ZXMsIGxvY2F0aW9uLCBjb21wb25lbnROYW1lLCBnZXRTdGFjaykge1xuICBpZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICAgIGZvciAodmFyIHR5cGVTcGVjTmFtZSBpbiB0eXBlU3BlY3MpIHtcbiAgICAgIGlmICh0eXBlU3BlY3MuaGFzT3duUHJvcGVydHkodHlwZVNwZWNOYW1lKSkge1xuICAgICAgICB2YXIgZXJyb3I7XG4gICAgICAgIC8vIFByb3AgdHlwZSB2YWxpZGF0aW9uIG1heSB0aHJvdy4gSW4gY2FzZSB0aGV5IGRvLCB3ZSBkb24ndCB3YW50IHRvXG4gICAgICAgIC8vIGZhaWwgdGhlIHJlbmRlciBwaGFzZSB3aGVyZSBpdCBkaWRuJ3QgZmFpbCBiZWZvcmUuIFNvIHdlIGxvZyBpdC5cbiAgICAgICAgLy8gQWZ0ZXIgdGhlc2UgaGF2ZSBiZWVuIGNsZWFuZWQgdXAsIHdlJ2xsIGxldCB0aGVtIHRocm93LlxuICAgICAgICB0cnkge1xuICAgICAgICAgIC8vIFRoaXMgaXMgaW50ZW50aW9uYWxseSBhbiBpbnZhcmlhbnQgdGhhdCBnZXRzIGNhdWdodC4gSXQncyB0aGUgc2FtZVxuICAgICAgICAgIC8vIGJlaGF2aW9yIGFzIHdpdGhvdXQgdGhpcyBzdGF0ZW1lbnQgZXhjZXB0IHdpdGggYSBiZXR0ZXIgbWVzc2FnZS5cbiAgICAgICAgICBpbnZhcmlhbnQodHlwZW9mIHR5cGVTcGVjc1t0eXBlU3BlY05hbWVdID09PSAnZnVuY3Rpb24nLCAnJXM6ICVzIHR5cGUgYCVzYCBpcyBpbnZhbGlkOyBpdCBtdXN0IGJlIGEgZnVuY3Rpb24sIHVzdWFsbHkgZnJvbSAnICsgJ3RoZSBgcHJvcC10eXBlc2AgcGFja2FnZSwgYnV0IHJlY2VpdmVkIGAlc2AuJywgY29tcG9uZW50TmFtZSB8fCAnUmVhY3QgY2xhc3MnLCBsb2NhdGlvbiwgdHlwZVNwZWNOYW1lLCB0eXBlb2YgdHlwZVNwZWNzW3R5cGVTcGVjTmFtZV0pO1xuICAgICAgICAgIGVycm9yID0gdHlwZVNwZWNzW3R5cGVTcGVjTmFtZV0odmFsdWVzLCB0eXBlU3BlY05hbWUsIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBudWxsLCBSZWFjdFByb3BUeXBlc1NlY3JldCk7XG4gICAgICAgIH0gY2F0Y2ggKGV4KSB7XG4gICAgICAgICAgZXJyb3IgPSBleDtcbiAgICAgICAgfVxuICAgICAgICB3YXJuaW5nKCFlcnJvciB8fCBlcnJvciBpbnN0YW5jZW9mIEVycm9yLCAnJXM6IHR5cGUgc3BlY2lmaWNhdGlvbiBvZiAlcyBgJXNgIGlzIGludmFsaWQ7IHRoZSB0eXBlIGNoZWNrZXIgJyArICdmdW5jdGlvbiBtdXN0IHJldHVybiBgbnVsbGAgb3IgYW4gYEVycm9yYCBidXQgcmV0dXJuZWQgYSAlcy4gJyArICdZb3UgbWF5IGhhdmUgZm9yZ290dGVuIHRvIHBhc3MgYW4gYXJndW1lbnQgdG8gdGhlIHR5cGUgY2hlY2tlciAnICsgJ2NyZWF0b3IgKGFycmF5T2YsIGluc3RhbmNlT2YsIG9iamVjdE9mLCBvbmVPZiwgb25lT2ZUeXBlLCBhbmQgJyArICdzaGFwZSBhbGwgcmVxdWlyZSBhbiBhcmd1bWVudCkuJywgY29tcG9uZW50TmFtZSB8fCAnUmVhY3QgY2xhc3MnLCBsb2NhdGlvbiwgdHlwZVNwZWNOYW1lLCB0eXBlb2YgZXJyb3IpO1xuICAgICAgICBpZiAoZXJyb3IgaW5zdGFuY2VvZiBFcnJvciAmJiAhKGVycm9yLm1lc3NhZ2UgaW4gbG9nZ2VkVHlwZUZhaWx1cmVzKSkge1xuICAgICAgICAgIC8vIE9ubHkgbW9uaXRvciB0aGlzIGZhaWx1cmUgb25jZSBiZWNhdXNlIHRoZXJlIHRlbmRzIHRvIGJlIGEgbG90IG9mIHRoZVxuICAgICAgICAgIC8vIHNhbWUgZXJyb3IuXG4gICAgICAgICAgbG9nZ2VkVHlwZUZhaWx1cmVzW2Vycm9yLm1lc3NhZ2VdID0gdHJ1ZTtcblxuICAgICAgICAgIHZhciBzdGFjayA9IGdldFN0YWNrID8gZ2V0U3RhY2soKSA6ICcnO1xuXG4gICAgICAgICAgd2FybmluZyhmYWxzZSwgJ0ZhaWxlZCAlcyB0eXBlOiAlcyVzJywgbG9jYXRpb24sIGVycm9yLm1lc3NhZ2UsIHN0YWNrICE9IG51bGwgPyBzdGFjayA6ICcnKTtcbiAgICAgICAgfVxuICAgICAgfVxuICAgIH1cbiAgfVxufVxuXG5tb2R1bGUuZXhwb3J0cyA9IGNoZWNrUHJvcFR5cGVzO1xuIiwiLyoqXG4gKiBDb3B5cmlnaHQgKGMpIDIwMTMtcHJlc2VudCwgRmFjZWJvb2ssIEluYy5cbiAqXG4gKiBUaGlzIHNvdXJjZSBjb2RlIGlzIGxpY2Vuc2VkIHVuZGVyIHRoZSBNSVQgbGljZW5zZSBmb3VuZCBpbiB0aGVcbiAqIExJQ0VOU0UgZmlsZSBpbiB0aGUgcm9vdCBkaXJlY3Rvcnkgb2YgdGhpcyBzb3VyY2UgdHJlZS5cbiAqL1xuXG4ndXNlIHN0cmljdCc7XG5cbnZhciBlbXB0eUZ1bmN0aW9uID0gcmVxdWlyZSgnZmJqcy9saWIvZW1wdHlGdW5jdGlvbicpO1xudmFyIGludmFyaWFudCA9IHJlcXVpcmUoJ2ZianMvbGliL2ludmFyaWFudCcpO1xudmFyIHdhcm5pbmcgPSByZXF1aXJlKCdmYmpzL2xpYi93YXJuaW5nJyk7XG52YXIgYXNzaWduID0gcmVxdWlyZSgnb2JqZWN0LWFzc2lnbicpO1xuXG52YXIgUmVhY3RQcm9wVHlwZXNTZWNyZXQgPSByZXF1aXJlKCcuL2xpYi9SZWFjdFByb3BUeXBlc1NlY3JldCcpO1xudmFyIGNoZWNrUHJvcFR5cGVzID0gcmVxdWlyZSgnLi9jaGVja1Byb3BUeXBlcycpO1xuXG5tb2R1bGUuZXhwb3J0cyA9IGZ1bmN0aW9uKGlzVmFsaWRFbGVtZW50LCB0aHJvd09uRGlyZWN0QWNjZXNzKSB7XG4gIC8qIGdsb2JhbCBTeW1ib2wgKi9cbiAgdmFyIElURVJBVE9SX1NZTUJPTCA9IHR5cGVvZiBTeW1ib2wgPT09ICdmdW5jdGlvbicgJiYgU3ltYm9sLml0ZXJhdG9yO1xuICB2YXIgRkFVWF9JVEVSQVRPUl9TWU1CT0wgPSAnQEBpdGVyYXRvcic7IC8vIEJlZm9yZSBTeW1ib2wgc3BlYy5cblxuICAvKipcbiAgICogUmV0dXJucyB0aGUgaXRlcmF0b3IgbWV0aG9kIGZ1bmN0aW9uIGNvbnRhaW5lZCBvbiB0aGUgaXRlcmFibGUgb2JqZWN0LlxuICAgKlxuICAgKiBCZSBzdXJlIHRvIGludm9rZSB0aGUgZnVuY3Rpb24gd2l0aCB0aGUgaXRlcmFibGUgYXMgY29udGV4dDpcbiAgICpcbiAgICogICAgIHZhciBpdGVyYXRvckZuID0gZ2V0SXRlcmF0b3JGbihteUl0ZXJhYmxlKTtcbiAgICogICAgIGlmIChpdGVyYXRvckZuKSB7XG4gICAqICAgICAgIHZhciBpdGVyYXRvciA9IGl0ZXJhdG9yRm4uY2FsbChteUl0ZXJhYmxlKTtcbiAgICogICAgICAgLi4uXG4gICAqICAgICB9XG4gICAqXG4gICAqIEBwYXJhbSB7P29iamVjdH0gbWF5YmVJdGVyYWJsZVxuICAgKiBAcmV0dXJuIHs/ZnVuY3Rpb259XG4gICAqL1xuICBmdW5jdGlvbiBnZXRJdGVyYXRvckZuKG1heWJlSXRlcmFibGUpIHtcbiAgICB2YXIgaXRlcmF0b3JGbiA9IG1heWJlSXRlcmFibGUgJiYgKElURVJBVE9SX1NZTUJPTCAmJiBtYXliZUl0ZXJhYmxlW0lURVJBVE9SX1NZTUJPTF0gfHwgbWF5YmVJdGVyYWJsZVtGQVVYX0lURVJBVE9SX1NZTUJPTF0pO1xuICAgIGlmICh0eXBlb2YgaXRlcmF0b3JGbiA9PT0gJ2Z1bmN0aW9uJykge1xuICAgICAgcmV0dXJuIGl0ZXJhdG9yRm47XG4gICAgfVxuICB9XG5cbiAgLyoqXG4gICAqIENvbGxlY3Rpb24gb2YgbWV0aG9kcyB0aGF0IGFsbG93IGRlY2xhcmF0aW9uIGFuZCB2YWxpZGF0aW9uIG9mIHByb3BzIHRoYXQgYXJlXG4gICAqIHN1cHBsaWVkIHRvIFJlYWN0IGNvbXBvbmVudHMuIEV4YW1wbGUgdXNhZ2U6XG4gICAqXG4gICAqICAgdmFyIFByb3BzID0gcmVxdWlyZSgnUmVhY3RQcm9wVHlwZXMnKTtcbiAgICogICB2YXIgTXlBcnRpY2xlID0gUmVhY3QuY3JlYXRlQ2xhc3Moe1xuICAgKiAgICAgcHJvcFR5cGVzOiB7XG4gICAqICAgICAgIC8vIEFuIG9wdGlvbmFsIHN0cmluZyBwcm9wIG5hbWVkIFwiZGVzY3JpcHRpb25cIi5cbiAgICogICAgICAgZGVzY3JpcHRpb246IFByb3BzLnN0cmluZyxcbiAgICpcbiAgICogICAgICAgLy8gQSByZXF1aXJlZCBlbnVtIHByb3AgbmFtZWQgXCJjYXRlZ29yeVwiLlxuICAgKiAgICAgICBjYXRlZ29yeTogUHJvcHMub25lT2YoWydOZXdzJywnUGhvdG9zJ10pLmlzUmVxdWlyZWQsXG4gICAqXG4gICAqICAgICAgIC8vIEEgcHJvcCBuYW1lZCBcImRpYWxvZ1wiIHRoYXQgcmVxdWlyZXMgYW4gaW5zdGFuY2Ugb2YgRGlhbG9nLlxuICAgKiAgICAgICBkaWFsb2c6IFByb3BzLmluc3RhbmNlT2YoRGlhbG9nKS5pc1JlcXVpcmVkXG4gICAqICAgICB9LFxuICAgKiAgICAgcmVuZGVyOiBmdW5jdGlvbigpIHsgLi4uIH1cbiAgICogICB9KTtcbiAgICpcbiAgICogQSBtb3JlIGZvcm1hbCBzcGVjaWZpY2F0aW9uIG9mIGhvdyB0aGVzZSBtZXRob2RzIGFyZSB1c2VkOlxuICAgKlxuICAgKiAgIHR5cGUgOj0gYXJyYXl8Ym9vbHxmdW5jfG9iamVjdHxudW1iZXJ8c3RyaW5nfG9uZU9mKFsuLi5dKXxpbnN0YW5jZU9mKC4uLilcbiAgICogICBkZWNsIDo9IFJlYWN0UHJvcFR5cGVzLnt0eXBlfSguaXNSZXF1aXJlZCk/XG4gICAqXG4gICAqIEVhY2ggYW5kIGV2ZXJ5IGRlY2xhcmF0aW9uIHByb2R1Y2VzIGEgZnVuY3Rpb24gd2l0aCB0aGUgc2FtZSBzaWduYXR1cmUuIFRoaXNcbiAgICogYWxsb3dzIHRoZSBjcmVhdGlvbiBvZiBjdXN0b20gdmFsaWRhdGlvbiBmdW5jdGlvbnMuIEZvciBleGFtcGxlOlxuICAgKlxuICAgKiAgdmFyIE15TGluayA9IFJlYWN0LmNyZWF0ZUNsYXNzKHtcbiAgICogICAgcHJvcFR5cGVzOiB7XG4gICAqICAgICAgLy8gQW4gb3B0aW9uYWwgc3RyaW5nIG9yIFVSSSBwcm9wIG5hbWVkIFwiaHJlZlwiLlxuICAgKiAgICAgIGhyZWY6IGZ1bmN0aW9uKHByb3BzLCBwcm9wTmFtZSwgY29tcG9uZW50TmFtZSkge1xuICAgKiAgICAgICAgdmFyIHByb3BWYWx1ZSA9IHByb3BzW3Byb3BOYW1lXTtcbiAgICogICAgICAgIGlmIChwcm9wVmFsdWUgIT0gbnVsbCAmJiB0eXBlb2YgcHJvcFZhbHVlICE9PSAnc3RyaW5nJyAmJlxuICAgKiAgICAgICAgICAgICEocHJvcFZhbHVlIGluc3RhbmNlb2YgVVJJKSkge1xuICAgKiAgICAgICAgICByZXR1cm4gbmV3IEVycm9yKFxuICAgKiAgICAgICAgICAgICdFeHBlY3RlZCBhIHN0cmluZyBvciBhbiBVUkkgZm9yICcgKyBwcm9wTmFtZSArICcgaW4gJyArXG4gICAqICAgICAgICAgICAgY29tcG9uZW50TmFtZVxuICAgKiAgICAgICAgICApO1xuICAgKiAgICAgICAgfVxuICAgKiAgICAgIH1cbiAgICogICAgfSxcbiAgICogICAgcmVuZGVyOiBmdW5jdGlvbigpIHsuLi59XG4gICAqICB9KTtcbiAgICpcbiAgICogQGludGVybmFsXG4gICAqL1xuXG4gIHZhciBBTk9OWU1PVVMgPSAnPDxhbm9ueW1vdXM+Pic7XG5cbiAgLy8gSW1wb3J0YW50IVxuICAvLyBLZWVwIHRoaXMgbGlzdCBpbiBzeW5jIHdpdGggcHJvZHVjdGlvbiB2ZXJzaW9uIGluIGAuL2ZhY3RvcnlXaXRoVGhyb3dpbmdTaGltcy5qc2AuXG4gIHZhciBSZWFjdFByb3BUeXBlcyA9IHtcbiAgICBhcnJheTogY3JlYXRlUHJpbWl0aXZlVHlwZUNoZWNrZXIoJ2FycmF5JyksXG4gICAgYm9vbDogY3JlYXRlUHJpbWl0aXZlVHlwZUNoZWNrZXIoJ2Jvb2xlYW4nKSxcbiAgICBmdW5jOiBjcmVhdGVQcmltaXRpdmVUeXBlQ2hlY2tlcignZnVuY3Rpb24nKSxcbiAgICBudW1iZXI6IGNyZWF0ZVByaW1pdGl2ZVR5cGVDaGVja2VyKCdudW1iZXInKSxcbiAgICBvYmplY3Q6IGNyZWF0ZVByaW1pdGl2ZVR5cGVDaGVja2VyKCdvYmplY3QnKSxcbiAgICBzdHJpbmc6IGNyZWF0ZVByaW1pdGl2ZVR5cGVDaGVja2VyKCdzdHJpbmcnKSxcbiAgICBzeW1ib2w6IGNyZWF0ZVByaW1pdGl2ZVR5cGVDaGVja2VyKCdzeW1ib2wnKSxcblxuICAgIGFueTogY3JlYXRlQW55VHlwZUNoZWNrZXIoKSxcbiAgICBhcnJheU9mOiBjcmVhdGVBcnJheU9mVHlwZUNoZWNrZXIsXG4gICAgZWxlbWVudDogY3JlYXRlRWxlbWVudFR5cGVDaGVja2VyKCksXG4gICAgaW5zdGFuY2VPZjogY3JlYXRlSW5zdGFuY2VUeXBlQ2hlY2tlcixcbiAgICBub2RlOiBjcmVhdGVOb2RlQ2hlY2tlcigpLFxuICAgIG9iamVjdE9mOiBjcmVhdGVPYmplY3RPZlR5cGVDaGVja2VyLFxuICAgIG9uZU9mOiBjcmVhdGVFbnVtVHlwZUNoZWNrZXIsXG4gICAgb25lT2ZUeXBlOiBjcmVhdGVVbmlvblR5cGVDaGVja2VyLFxuICAgIHNoYXBlOiBjcmVhdGVTaGFwZVR5cGVDaGVja2VyLFxuICAgIGV4YWN0OiBjcmVhdGVTdHJpY3RTaGFwZVR5cGVDaGVja2VyLFxuICB9O1xuXG4gIC8qKlxuICAgKiBpbmxpbmVkIE9iamVjdC5pcyBwb2x5ZmlsbCB0byBhdm9pZCByZXF1aXJpbmcgY29uc3VtZXJzIHNoaXAgdGhlaXIgb3duXG4gICAqIGh0dHBzOi8vZGV2ZWxvcGVyLm1vemlsbGEub3JnL2VuLVVTL2RvY3MvV2ViL0phdmFTY3JpcHQvUmVmZXJlbmNlL0dsb2JhbF9PYmplY3RzL09iamVjdC9pc1xuICAgKi9cbiAgLyplc2xpbnQtZGlzYWJsZSBuby1zZWxmLWNvbXBhcmUqL1xuICBmdW5jdGlvbiBpcyh4LCB5KSB7XG4gICAgLy8gU2FtZVZhbHVlIGFsZ29yaXRobVxuICAgIGlmICh4ID09PSB5KSB7XG4gICAgICAvLyBTdGVwcyAxLTUsIDctMTBcbiAgICAgIC8vIFN0ZXBzIDYuYi02LmU6ICswICE9IC0wXG4gICAgICByZXR1cm4geCAhPT0gMCB8fCAxIC8geCA9PT0gMSAvIHk7XG4gICAgfSBlbHNlIHtcbiAgICAgIC8vIFN0ZXAgNi5hOiBOYU4gPT0gTmFOXG4gICAgICByZXR1cm4geCAhPT0geCAmJiB5ICE9PSB5O1xuICAgIH1cbiAgfVxuICAvKmVzbGludC1lbmFibGUgbm8tc2VsZi1jb21wYXJlKi9cblxuICAvKipcbiAgICogV2UgdXNlIGFuIEVycm9yLWxpa2Ugb2JqZWN0IGZvciBiYWNrd2FyZCBjb21wYXRpYmlsaXR5IGFzIHBlb3BsZSBtYXkgY2FsbFxuICAgKiBQcm9wVHlwZXMgZGlyZWN0bHkgYW5kIGluc3BlY3QgdGhlaXIgb3V0cHV0LiBIb3dldmVyLCB3ZSBkb24ndCB1c2UgcmVhbFxuICAgKiBFcnJvcnMgYW55bW9yZS4gV2UgZG9uJ3QgaW5zcGVjdCB0aGVpciBzdGFjayBhbnl3YXksIGFuZCBjcmVhdGluZyB0aGVtXG4gICAqIGlzIHByb2hpYml0aXZlbHkgZXhwZW5zaXZlIGlmIHRoZXkgYXJlIGNyZWF0ZWQgdG9vIG9mdGVuLCBzdWNoIGFzIHdoYXRcbiAgICogaGFwcGVucyBpbiBvbmVPZlR5cGUoKSBmb3IgYW55IHR5cGUgYmVmb3JlIHRoZSBvbmUgdGhhdCBtYXRjaGVkLlxuICAgKi9cbiAgZnVuY3Rpb24gUHJvcFR5cGVFcnJvcihtZXNzYWdlKSB7XG4gICAgdGhpcy5tZXNzYWdlID0gbWVzc2FnZTtcbiAgICB0aGlzLnN0YWNrID0gJyc7XG4gIH1cbiAgLy8gTWFrZSBgaW5zdGFuY2VvZiBFcnJvcmAgc3RpbGwgd29yayBmb3IgcmV0dXJuZWQgZXJyb3JzLlxuICBQcm9wVHlwZUVycm9yLnByb3RvdHlwZSA9IEVycm9yLnByb3RvdHlwZTtcblxuICBmdW5jdGlvbiBjcmVhdGVDaGFpbmFibGVUeXBlQ2hlY2tlcih2YWxpZGF0ZSkge1xuICAgIGlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gICAgICB2YXIgbWFudWFsUHJvcFR5cGVDYWxsQ2FjaGUgPSB7fTtcbiAgICAgIHZhciBtYW51YWxQcm9wVHlwZVdhcm5pbmdDb3VudCA9IDA7XG4gICAgfVxuICAgIGZ1bmN0aW9uIGNoZWNrVHlwZShpc1JlcXVpcmVkLCBwcm9wcywgcHJvcE5hbWUsIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUsIHNlY3JldCkge1xuICAgICAgY29tcG9uZW50TmFtZSA9IGNvbXBvbmVudE5hbWUgfHwgQU5PTllNT1VTO1xuICAgICAgcHJvcEZ1bGxOYW1lID0gcHJvcEZ1bGxOYW1lIHx8IHByb3BOYW1lO1xuXG4gICAgICBpZiAoc2VjcmV0ICE9PSBSZWFjdFByb3BUeXBlc1NlY3JldCkge1xuICAgICAgICBpZiAodGhyb3dPbkRpcmVjdEFjY2Vzcykge1xuICAgICAgICAgIC8vIE5ldyBiZWhhdmlvciBvbmx5IGZvciB1c2VycyBvZiBgcHJvcC10eXBlc2AgcGFja2FnZVxuICAgICAgICAgIGludmFyaWFudChcbiAgICAgICAgICAgIGZhbHNlLFxuICAgICAgICAgICAgJ0NhbGxpbmcgUHJvcFR5cGVzIHZhbGlkYXRvcnMgZGlyZWN0bHkgaXMgbm90IHN1cHBvcnRlZCBieSB0aGUgYHByb3AtdHlwZXNgIHBhY2thZ2UuICcgK1xuICAgICAgICAgICAgJ1VzZSBgUHJvcFR5cGVzLmNoZWNrUHJvcFR5cGVzKClgIHRvIGNhbGwgdGhlbS4gJyArXG4gICAgICAgICAgICAnUmVhZCBtb3JlIGF0IGh0dHA6Ly9mYi5tZS91c2UtY2hlY2stcHJvcC10eXBlcydcbiAgICAgICAgICApO1xuICAgICAgICB9IGVsc2UgaWYgKHByb2Nlc3MuZW52Lk5PREVfRU5WICE9PSAncHJvZHVjdGlvbicgJiYgdHlwZW9mIGNvbnNvbGUgIT09ICd1bmRlZmluZWQnKSB7XG4gICAgICAgICAgLy8gT2xkIGJlaGF2aW9yIGZvciBwZW9wbGUgdXNpbmcgUmVhY3QuUHJvcFR5cGVzXG4gICAgICAgICAgdmFyIGNhY2hlS2V5ID0gY29tcG9uZW50TmFtZSArICc6JyArIHByb3BOYW1lO1xuICAgICAgICAgIGlmIChcbiAgICAgICAgICAgICFtYW51YWxQcm9wVHlwZUNhbGxDYWNoZVtjYWNoZUtleV0gJiZcbiAgICAgICAgICAgIC8vIEF2b2lkIHNwYW1taW5nIHRoZSBjb25zb2xlIGJlY2F1c2UgdGhleSBhcmUgb2Z0ZW4gbm90IGFjdGlvbmFibGUgZXhjZXB0IGZvciBsaWIgYXV0aG9yc1xuICAgICAgICAgICAgbWFudWFsUHJvcFR5cGVXYXJuaW5nQ291bnQgPCAzXG4gICAgICAgICAgKSB7XG4gICAgICAgICAgICB3YXJuaW5nKFxuICAgICAgICAgICAgICBmYWxzZSxcbiAgICAgICAgICAgICAgJ1lvdSBhcmUgbWFudWFsbHkgY2FsbGluZyBhIFJlYWN0LlByb3BUeXBlcyB2YWxpZGF0aW9uICcgK1xuICAgICAgICAgICAgICAnZnVuY3Rpb24gZm9yIHRoZSBgJXNgIHByb3Agb24gYCVzYC4gVGhpcyBpcyBkZXByZWNhdGVkICcgK1xuICAgICAgICAgICAgICAnYW5kIHdpbGwgdGhyb3cgaW4gdGhlIHN0YW5kYWxvbmUgYHByb3AtdHlwZXNgIHBhY2thZ2UuICcgK1xuICAgICAgICAgICAgICAnWW91IG1heSBiZSBzZWVpbmcgdGhpcyB3YXJuaW5nIGR1ZSB0byBhIHRoaXJkLXBhcnR5IFByb3BUeXBlcyAnICtcbiAgICAgICAgICAgICAgJ2xpYnJhcnkuIFNlZSBodHRwczovL2ZiLm1lL3JlYWN0LXdhcm5pbmctZG9udC1jYWxsLXByb3B0eXBlcyAnICsgJ2ZvciBkZXRhaWxzLicsXG4gICAgICAgICAgICAgIHByb3BGdWxsTmFtZSxcbiAgICAgICAgICAgICAgY29tcG9uZW50TmFtZVxuICAgICAgICAgICAgKTtcbiAgICAgICAgICAgIG1hbnVhbFByb3BUeXBlQ2FsbENhY2hlW2NhY2hlS2V5XSA9IHRydWU7XG4gICAgICAgICAgICBtYW51YWxQcm9wVHlwZVdhcm5pbmdDb3VudCsrO1xuICAgICAgICAgIH1cbiAgICAgICAgfVxuICAgICAgfVxuICAgICAgaWYgKHByb3BzW3Byb3BOYW1lXSA9PSBudWxsKSB7XG4gICAgICAgIGlmIChpc1JlcXVpcmVkKSB7XG4gICAgICAgICAgaWYgKHByb3BzW3Byb3BOYW1lXSA9PT0gbnVsbCkge1xuICAgICAgICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKCdUaGUgJyArIGxvY2F0aW9uICsgJyBgJyArIHByb3BGdWxsTmFtZSArICdgIGlzIG1hcmtlZCBhcyByZXF1aXJlZCAnICsgKCdpbiBgJyArIGNvbXBvbmVudE5hbWUgKyAnYCwgYnV0IGl0cyB2YWx1ZSBpcyBgbnVsbGAuJykpO1xuICAgICAgICAgIH1cbiAgICAgICAgICByZXR1cm4gbmV3IFByb3BUeXBlRXJyb3IoJ1RoZSAnICsgbG9jYXRpb24gKyAnIGAnICsgcHJvcEZ1bGxOYW1lICsgJ2AgaXMgbWFya2VkIGFzIHJlcXVpcmVkIGluICcgKyAoJ2AnICsgY29tcG9uZW50TmFtZSArICdgLCBidXQgaXRzIHZhbHVlIGlzIGB1bmRlZmluZWRgLicpKTtcbiAgICAgICAgfVxuICAgICAgICByZXR1cm4gbnVsbDtcbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIHJldHVybiB2YWxpZGF0ZShwcm9wcywgcHJvcE5hbWUsIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUpO1xuICAgICAgfVxuICAgIH1cblxuICAgIHZhciBjaGFpbmVkQ2hlY2tUeXBlID0gY2hlY2tUeXBlLmJpbmQobnVsbCwgZmFsc2UpO1xuICAgIGNoYWluZWRDaGVja1R5cGUuaXNSZXF1aXJlZCA9IGNoZWNrVHlwZS5iaW5kKG51bGwsIHRydWUpO1xuXG4gICAgcmV0dXJuIGNoYWluZWRDaGVja1R5cGU7XG4gIH1cblxuICBmdW5jdGlvbiBjcmVhdGVQcmltaXRpdmVUeXBlQ2hlY2tlcihleHBlY3RlZFR5cGUpIHtcbiAgICBmdW5jdGlvbiB2YWxpZGF0ZShwcm9wcywgcHJvcE5hbWUsIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUsIHNlY3JldCkge1xuICAgICAgdmFyIHByb3BWYWx1ZSA9IHByb3BzW3Byb3BOYW1lXTtcbiAgICAgIHZhciBwcm9wVHlwZSA9IGdldFByb3BUeXBlKHByb3BWYWx1ZSk7XG4gICAgICBpZiAocHJvcFR5cGUgIT09IGV4cGVjdGVkVHlwZSkge1xuICAgICAgICAvLyBgcHJvcFZhbHVlYCBiZWluZyBpbnN0YW5jZSBvZiwgc2F5LCBkYXRlL3JlZ2V4cCwgcGFzcyB0aGUgJ29iamVjdCdcbiAgICAgICAgLy8gY2hlY2ssIGJ1dCB3ZSBjYW4gb2ZmZXIgYSBtb3JlIHByZWNpc2UgZXJyb3IgbWVzc2FnZSBoZXJlIHJhdGhlciB0aGFuXG4gICAgICAgIC8vICdvZiB0eXBlIGBvYmplY3RgJy5cbiAgICAgICAgdmFyIHByZWNpc2VUeXBlID0gZ2V0UHJlY2lzZVR5cGUocHJvcFZhbHVlKTtcblxuICAgICAgICByZXR1cm4gbmV3IFByb3BUeXBlRXJyb3IoJ0ludmFsaWQgJyArIGxvY2F0aW9uICsgJyBgJyArIHByb3BGdWxsTmFtZSArICdgIG9mIHR5cGUgJyArICgnYCcgKyBwcmVjaXNlVHlwZSArICdgIHN1cHBsaWVkIHRvIGAnICsgY29tcG9uZW50TmFtZSArICdgLCBleHBlY3RlZCAnKSArICgnYCcgKyBleHBlY3RlZFR5cGUgKyAnYC4nKSk7XG4gICAgICB9XG4gICAgICByZXR1cm4gbnVsbDtcbiAgICB9XG4gICAgcmV0dXJuIGNyZWF0ZUNoYWluYWJsZVR5cGVDaGVja2VyKHZhbGlkYXRlKTtcbiAgfVxuXG4gIGZ1bmN0aW9uIGNyZWF0ZUFueVR5cGVDaGVja2VyKCkge1xuICAgIHJldHVybiBjcmVhdGVDaGFpbmFibGVUeXBlQ2hlY2tlcihlbXB0eUZ1bmN0aW9uLnRoYXRSZXR1cm5zTnVsbCk7XG4gIH1cblxuICBmdW5jdGlvbiBjcmVhdGVBcnJheU9mVHlwZUNoZWNrZXIodHlwZUNoZWNrZXIpIHtcbiAgICBmdW5jdGlvbiB2YWxpZGF0ZShwcm9wcywgcHJvcE5hbWUsIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUpIHtcbiAgICAgIGlmICh0eXBlb2YgdHlwZUNoZWNrZXIgIT09ICdmdW5jdGlvbicpIHtcbiAgICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKCdQcm9wZXJ0eSBgJyArIHByb3BGdWxsTmFtZSArICdgIG9mIGNvbXBvbmVudCBgJyArIGNvbXBvbmVudE5hbWUgKyAnYCBoYXMgaW52YWxpZCBQcm9wVHlwZSBub3RhdGlvbiBpbnNpZGUgYXJyYXlPZi4nKTtcbiAgICAgIH1cbiAgICAgIHZhciBwcm9wVmFsdWUgPSBwcm9wc1twcm9wTmFtZV07XG4gICAgICBpZiAoIUFycmF5LmlzQXJyYXkocHJvcFZhbHVlKSkge1xuICAgICAgICB2YXIgcHJvcFR5cGUgPSBnZXRQcm9wVHlwZShwcm9wVmFsdWUpO1xuICAgICAgICByZXR1cm4gbmV3IFByb3BUeXBlRXJyb3IoJ0ludmFsaWQgJyArIGxvY2F0aW9uICsgJyBgJyArIHByb3BGdWxsTmFtZSArICdgIG9mIHR5cGUgJyArICgnYCcgKyBwcm9wVHlwZSArICdgIHN1cHBsaWVkIHRvIGAnICsgY29tcG9uZW50TmFtZSArICdgLCBleHBlY3RlZCBhbiBhcnJheS4nKSk7XG4gICAgICB9XG4gICAgICBmb3IgKHZhciBpID0gMDsgaSA8IHByb3BWYWx1ZS5sZW5ndGg7IGkrKykge1xuICAgICAgICB2YXIgZXJyb3IgPSB0eXBlQ2hlY2tlcihwcm9wVmFsdWUsIGksIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUgKyAnWycgKyBpICsgJ10nLCBSZWFjdFByb3BUeXBlc1NlY3JldCk7XG4gICAgICAgIGlmIChlcnJvciBpbnN0YW5jZW9mIEVycm9yKSB7XG4gICAgICAgICAgcmV0dXJuIGVycm9yO1xuICAgICAgICB9XG4gICAgICB9XG4gICAgICByZXR1cm4gbnVsbDtcbiAgICB9XG4gICAgcmV0dXJuIGNyZWF0ZUNoYWluYWJsZVR5cGVDaGVja2VyKHZhbGlkYXRlKTtcbiAgfVxuXG4gIGZ1bmN0aW9uIGNyZWF0ZUVsZW1lbnRUeXBlQ2hlY2tlcigpIHtcbiAgICBmdW5jdGlvbiB2YWxpZGF0ZShwcm9wcywgcHJvcE5hbWUsIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUpIHtcbiAgICAgIHZhciBwcm9wVmFsdWUgPSBwcm9wc1twcm9wTmFtZV07XG4gICAgICBpZiAoIWlzVmFsaWRFbGVtZW50KHByb3BWYWx1ZSkpIHtcbiAgICAgICAgdmFyIHByb3BUeXBlID0gZ2V0UHJvcFR5cGUocHJvcFZhbHVlKTtcbiAgICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKCdJbnZhbGlkICcgKyBsb2NhdGlvbiArICcgYCcgKyBwcm9wRnVsbE5hbWUgKyAnYCBvZiB0eXBlICcgKyAoJ2AnICsgcHJvcFR5cGUgKyAnYCBzdXBwbGllZCB0byBgJyArIGNvbXBvbmVudE5hbWUgKyAnYCwgZXhwZWN0ZWQgYSBzaW5nbGUgUmVhY3RFbGVtZW50LicpKTtcbiAgICAgIH1cbiAgICAgIHJldHVybiBudWxsO1xuICAgIH1cbiAgICByZXR1cm4gY3JlYXRlQ2hhaW5hYmxlVHlwZUNoZWNrZXIodmFsaWRhdGUpO1xuICB9XG5cbiAgZnVuY3Rpb24gY3JlYXRlSW5zdGFuY2VUeXBlQ2hlY2tlcihleHBlY3RlZENsYXNzKSB7XG4gICAgZnVuY3Rpb24gdmFsaWRhdGUocHJvcHMsIHByb3BOYW1lLCBjb21wb25lbnROYW1lLCBsb2NhdGlvbiwgcHJvcEZ1bGxOYW1lKSB7XG4gICAgICBpZiAoIShwcm9wc1twcm9wTmFtZV0gaW5zdGFuY2VvZiBleHBlY3RlZENsYXNzKSkge1xuICAgICAgICB2YXIgZXhwZWN0ZWRDbGFzc05hbWUgPSBleHBlY3RlZENsYXNzLm5hbWUgfHwgQU5PTllNT1VTO1xuICAgICAgICB2YXIgYWN0dWFsQ2xhc3NOYW1lID0gZ2V0Q2xhc3NOYW1lKHByb3BzW3Byb3BOYW1lXSk7XG4gICAgICAgIHJldHVybiBuZXcgUHJvcFR5cGVFcnJvcignSW52YWxpZCAnICsgbG9jYXRpb24gKyAnIGAnICsgcHJvcEZ1bGxOYW1lICsgJ2Agb2YgdHlwZSAnICsgKCdgJyArIGFjdHVhbENsYXNzTmFtZSArICdgIHN1cHBsaWVkIHRvIGAnICsgY29tcG9uZW50TmFtZSArICdgLCBleHBlY3RlZCAnKSArICgnaW5zdGFuY2Ugb2YgYCcgKyBleHBlY3RlZENsYXNzTmFtZSArICdgLicpKTtcbiAgICAgIH1cbiAgICAgIHJldHVybiBudWxsO1xuICAgIH1cbiAgICByZXR1cm4gY3JlYXRlQ2hhaW5hYmxlVHlwZUNoZWNrZXIodmFsaWRhdGUpO1xuICB9XG5cbiAgZnVuY3Rpb24gY3JlYXRlRW51bVR5cGVDaGVja2VyKGV4cGVjdGVkVmFsdWVzKSB7XG4gICAgaWYgKCFBcnJheS5pc0FycmF5KGV4cGVjdGVkVmFsdWVzKSkge1xuICAgICAgcHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJyA/IHdhcm5pbmcoZmFsc2UsICdJbnZhbGlkIGFyZ3VtZW50IHN1cHBsaWVkIHRvIG9uZU9mLCBleHBlY3RlZCBhbiBpbnN0YW5jZSBvZiBhcnJheS4nKSA6IHZvaWQgMDtcbiAgICAgIHJldHVybiBlbXB0eUZ1bmN0aW9uLnRoYXRSZXR1cm5zTnVsbDtcbiAgICB9XG5cbiAgICBmdW5jdGlvbiB2YWxpZGF0ZShwcm9wcywgcHJvcE5hbWUsIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUpIHtcbiAgICAgIHZhciBwcm9wVmFsdWUgPSBwcm9wc1twcm9wTmFtZV07XG4gICAgICBmb3IgKHZhciBpID0gMDsgaSA8IGV4cGVjdGVkVmFsdWVzLmxlbmd0aDsgaSsrKSB7XG4gICAgICAgIGlmIChpcyhwcm9wVmFsdWUsIGV4cGVjdGVkVmFsdWVzW2ldKSkge1xuICAgICAgICAgIHJldHVybiBudWxsO1xuICAgICAgICB9XG4gICAgICB9XG5cbiAgICAgIHZhciB2YWx1ZXNTdHJpbmcgPSBKU09OLnN0cmluZ2lmeShleHBlY3RlZFZhbHVlcyk7XG4gICAgICByZXR1cm4gbmV3IFByb3BUeXBlRXJyb3IoJ0ludmFsaWQgJyArIGxvY2F0aW9uICsgJyBgJyArIHByb3BGdWxsTmFtZSArICdgIG9mIHZhbHVlIGAnICsgcHJvcFZhbHVlICsgJ2AgJyArICgnc3VwcGxpZWQgdG8gYCcgKyBjb21wb25lbnROYW1lICsgJ2AsIGV4cGVjdGVkIG9uZSBvZiAnICsgdmFsdWVzU3RyaW5nICsgJy4nKSk7XG4gICAgfVxuICAgIHJldHVybiBjcmVhdGVDaGFpbmFibGVUeXBlQ2hlY2tlcih2YWxpZGF0ZSk7XG4gIH1cblxuICBmdW5jdGlvbiBjcmVhdGVPYmplY3RPZlR5cGVDaGVja2VyKHR5cGVDaGVja2VyKSB7XG4gICAgZnVuY3Rpb24gdmFsaWRhdGUocHJvcHMsIHByb3BOYW1lLCBjb21wb25lbnROYW1lLCBsb2NhdGlvbiwgcHJvcEZ1bGxOYW1lKSB7XG4gICAgICBpZiAodHlwZW9mIHR5cGVDaGVja2VyICE9PSAnZnVuY3Rpb24nKSB7XG4gICAgICAgIHJldHVybiBuZXcgUHJvcFR5cGVFcnJvcignUHJvcGVydHkgYCcgKyBwcm9wRnVsbE5hbWUgKyAnYCBvZiBjb21wb25lbnQgYCcgKyBjb21wb25lbnROYW1lICsgJ2AgaGFzIGludmFsaWQgUHJvcFR5cGUgbm90YXRpb24gaW5zaWRlIG9iamVjdE9mLicpO1xuICAgICAgfVxuICAgICAgdmFyIHByb3BWYWx1ZSA9IHByb3BzW3Byb3BOYW1lXTtcbiAgICAgIHZhciBwcm9wVHlwZSA9IGdldFByb3BUeXBlKHByb3BWYWx1ZSk7XG4gICAgICBpZiAocHJvcFR5cGUgIT09ICdvYmplY3QnKSB7XG4gICAgICAgIHJldHVybiBuZXcgUHJvcFR5cGVFcnJvcignSW52YWxpZCAnICsgbG9jYXRpb24gKyAnIGAnICsgcHJvcEZ1bGxOYW1lICsgJ2Agb2YgdHlwZSAnICsgKCdgJyArIHByb3BUeXBlICsgJ2Agc3VwcGxpZWQgdG8gYCcgKyBjb21wb25lbnROYW1lICsgJ2AsIGV4cGVjdGVkIGFuIG9iamVjdC4nKSk7XG4gICAgICB9XG4gICAgICBmb3IgKHZhciBrZXkgaW4gcHJvcFZhbHVlKSB7XG4gICAgICAgIGlmIChwcm9wVmFsdWUuaGFzT3duUHJvcGVydHkoa2V5KSkge1xuICAgICAgICAgIHZhciBlcnJvciA9IHR5cGVDaGVja2VyKHByb3BWYWx1ZSwga2V5LCBjb21wb25lbnROYW1lLCBsb2NhdGlvbiwgcHJvcEZ1bGxOYW1lICsgJy4nICsga2V5LCBSZWFjdFByb3BUeXBlc1NlY3JldCk7XG4gICAgICAgICAgaWYgKGVycm9yIGluc3RhbmNlb2YgRXJyb3IpIHtcbiAgICAgICAgICAgIHJldHVybiBlcnJvcjtcbiAgICAgICAgICB9XG4gICAgICAgIH1cbiAgICAgIH1cbiAgICAgIHJldHVybiBudWxsO1xuICAgIH1cbiAgICByZXR1cm4gY3JlYXRlQ2hhaW5hYmxlVHlwZUNoZWNrZXIodmFsaWRhdGUpO1xuICB9XG5cbiAgZnVuY3Rpb24gY3JlYXRlVW5pb25UeXBlQ2hlY2tlcihhcnJheU9mVHlwZUNoZWNrZXJzKSB7XG4gICAgaWYgKCFBcnJheS5pc0FycmF5KGFycmF5T2ZUeXBlQ2hlY2tlcnMpKSB7XG4gICAgICBwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nID8gd2FybmluZyhmYWxzZSwgJ0ludmFsaWQgYXJndW1lbnQgc3VwcGxpZWQgdG8gb25lT2ZUeXBlLCBleHBlY3RlZCBhbiBpbnN0YW5jZSBvZiBhcnJheS4nKSA6IHZvaWQgMDtcbiAgICAgIHJldHVybiBlbXB0eUZ1bmN0aW9uLnRoYXRSZXR1cm5zTnVsbDtcbiAgICB9XG5cbiAgICBmb3IgKHZhciBpID0gMDsgaSA8IGFycmF5T2ZUeXBlQ2hlY2tlcnMubGVuZ3RoOyBpKyspIHtcbiAgICAgIHZhciBjaGVja2VyID0gYXJyYXlPZlR5cGVDaGVja2Vyc1tpXTtcbiAgICAgIGlmICh0eXBlb2YgY2hlY2tlciAhPT0gJ2Z1bmN0aW9uJykge1xuICAgICAgICB3YXJuaW5nKFxuICAgICAgICAgIGZhbHNlLFxuICAgICAgICAgICdJbnZhbGlkIGFyZ3VtZW50IHN1cHBsaWVkIHRvIG9uZU9mVHlwZS4gRXhwZWN0ZWQgYW4gYXJyYXkgb2YgY2hlY2sgZnVuY3Rpb25zLCBidXQgJyArXG4gICAgICAgICAgJ3JlY2VpdmVkICVzIGF0IGluZGV4ICVzLicsXG4gICAgICAgICAgZ2V0UG9zdGZpeEZvclR5cGVXYXJuaW5nKGNoZWNrZXIpLFxuICAgICAgICAgIGlcbiAgICAgICAgKTtcbiAgICAgICAgcmV0dXJuIGVtcHR5RnVuY3Rpb24udGhhdFJldHVybnNOdWxsO1xuICAgICAgfVxuICAgIH1cblxuICAgIGZ1bmN0aW9uIHZhbGlkYXRlKHByb3BzLCBwcm9wTmFtZSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSkge1xuICAgICAgZm9yICh2YXIgaSA9IDA7IGkgPCBhcnJheU9mVHlwZUNoZWNrZXJzLmxlbmd0aDsgaSsrKSB7XG4gICAgICAgIHZhciBjaGVja2VyID0gYXJyYXlPZlR5cGVDaGVja2Vyc1tpXTtcbiAgICAgICAgaWYgKGNoZWNrZXIocHJvcHMsIHByb3BOYW1lLCBjb21wb25lbnROYW1lLCBsb2NhdGlvbiwgcHJvcEZ1bGxOYW1lLCBSZWFjdFByb3BUeXBlc1NlY3JldCkgPT0gbnVsbCkge1xuICAgICAgICAgIHJldHVybiBudWxsO1xuICAgICAgICB9XG4gICAgICB9XG5cbiAgICAgIHJldHVybiBuZXcgUHJvcFR5cGVFcnJvcignSW52YWxpZCAnICsgbG9jYXRpb24gKyAnIGAnICsgcHJvcEZ1bGxOYW1lICsgJ2Agc3VwcGxpZWQgdG8gJyArICgnYCcgKyBjb21wb25lbnROYW1lICsgJ2AuJykpO1xuICAgIH1cbiAgICByZXR1cm4gY3JlYXRlQ2hhaW5hYmxlVHlwZUNoZWNrZXIodmFsaWRhdGUpO1xuICB9XG5cbiAgZnVuY3Rpb24gY3JlYXRlTm9kZUNoZWNrZXIoKSB7XG4gICAgZnVuY3Rpb24gdmFsaWRhdGUocHJvcHMsIHByb3BOYW1lLCBjb21wb25lbnROYW1lLCBsb2NhdGlvbiwgcHJvcEZ1bGxOYW1lKSB7XG4gICAgICBpZiAoIWlzTm9kZShwcm9wc1twcm9wTmFtZV0pKSB7XG4gICAgICAgIHJldHVybiBuZXcgUHJvcFR5cGVFcnJvcignSW52YWxpZCAnICsgbG9jYXRpb24gKyAnIGAnICsgcHJvcEZ1bGxOYW1lICsgJ2Agc3VwcGxpZWQgdG8gJyArICgnYCcgKyBjb21wb25lbnROYW1lICsgJ2AsIGV4cGVjdGVkIGEgUmVhY3ROb2RlLicpKTtcbiAgICAgIH1cbiAgICAgIHJldHVybiBudWxsO1xuICAgIH1cbiAgICByZXR1cm4gY3JlYXRlQ2hhaW5hYmxlVHlwZUNoZWNrZXIodmFsaWRhdGUpO1xuICB9XG5cbiAgZnVuY3Rpb24gY3JlYXRlU2hhcGVUeXBlQ2hlY2tlcihzaGFwZVR5cGVzKSB7XG4gICAgZnVuY3Rpb24gdmFsaWRhdGUocHJvcHMsIHByb3BOYW1lLCBjb21wb25lbnROYW1lLCBsb2NhdGlvbiwgcHJvcEZ1bGxOYW1lKSB7XG4gICAgICB2YXIgcHJvcFZhbHVlID0gcHJvcHNbcHJvcE5hbWVdO1xuICAgICAgdmFyIHByb3BUeXBlID0gZ2V0UHJvcFR5cGUocHJvcFZhbHVlKTtcbiAgICAgIGlmIChwcm9wVHlwZSAhPT0gJ29iamVjdCcpIHtcbiAgICAgICAgcmV0dXJuIG5ldyBQcm9wVHlwZUVycm9yKCdJbnZhbGlkICcgKyBsb2NhdGlvbiArICcgYCcgKyBwcm9wRnVsbE5hbWUgKyAnYCBvZiB0eXBlIGAnICsgcHJvcFR5cGUgKyAnYCAnICsgKCdzdXBwbGllZCB0byBgJyArIGNvbXBvbmVudE5hbWUgKyAnYCwgZXhwZWN0ZWQgYG9iamVjdGAuJykpO1xuICAgICAgfVxuICAgICAgZm9yICh2YXIga2V5IGluIHNoYXBlVHlwZXMpIHtcbiAgICAgICAgdmFyIGNoZWNrZXIgPSBzaGFwZVR5cGVzW2tleV07XG4gICAgICAgIGlmICghY2hlY2tlcikge1xuICAgICAgICAgIGNvbnRpbnVlO1xuICAgICAgICB9XG4gICAgICAgIHZhciBlcnJvciA9IGNoZWNrZXIocHJvcFZhbHVlLCBrZXksIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUgKyAnLicgKyBrZXksIFJlYWN0UHJvcFR5cGVzU2VjcmV0KTtcbiAgICAgICAgaWYgKGVycm9yKSB7XG4gICAgICAgICAgcmV0dXJuIGVycm9yO1xuICAgICAgICB9XG4gICAgICB9XG4gICAgICByZXR1cm4gbnVsbDtcbiAgICB9XG4gICAgcmV0dXJuIGNyZWF0ZUNoYWluYWJsZVR5cGVDaGVja2VyKHZhbGlkYXRlKTtcbiAgfVxuXG4gIGZ1bmN0aW9uIGNyZWF0ZVN0cmljdFNoYXBlVHlwZUNoZWNrZXIoc2hhcGVUeXBlcykge1xuICAgIGZ1bmN0aW9uIHZhbGlkYXRlKHByb3BzLCBwcm9wTmFtZSwgY29tcG9uZW50TmFtZSwgbG9jYXRpb24sIHByb3BGdWxsTmFtZSkge1xuICAgICAgdmFyIHByb3BWYWx1ZSA9IHByb3BzW3Byb3BOYW1lXTtcbiAgICAgIHZhciBwcm9wVHlwZSA9IGdldFByb3BUeXBlKHByb3BWYWx1ZSk7XG4gICAgICBpZiAocHJvcFR5cGUgIT09ICdvYmplY3QnKSB7XG4gICAgICAgIHJldHVybiBuZXcgUHJvcFR5cGVFcnJvcignSW52YWxpZCAnICsgbG9jYXRpb24gKyAnIGAnICsgcHJvcEZ1bGxOYW1lICsgJ2Agb2YgdHlwZSBgJyArIHByb3BUeXBlICsgJ2AgJyArICgnc3VwcGxpZWQgdG8gYCcgKyBjb21wb25lbnROYW1lICsgJ2AsIGV4cGVjdGVkIGBvYmplY3RgLicpKTtcbiAgICAgIH1cbiAgICAgIC8vIFdlIG5lZWQgdG8gY2hlY2sgYWxsIGtleXMgaW4gY2FzZSBzb21lIGFyZSByZXF1aXJlZCBidXQgbWlzc2luZyBmcm9tXG4gICAgICAvLyBwcm9wcy5cbiAgICAgIHZhciBhbGxLZXlzID0gYXNzaWduKHt9LCBwcm9wc1twcm9wTmFtZV0sIHNoYXBlVHlwZXMpO1xuICAgICAgZm9yICh2YXIga2V5IGluIGFsbEtleXMpIHtcbiAgICAgICAgdmFyIGNoZWNrZXIgPSBzaGFwZVR5cGVzW2tleV07XG4gICAgICAgIGlmICghY2hlY2tlcikge1xuICAgICAgICAgIHJldHVybiBuZXcgUHJvcFR5cGVFcnJvcihcbiAgICAgICAgICAgICdJbnZhbGlkICcgKyBsb2NhdGlvbiArICcgYCcgKyBwcm9wRnVsbE5hbWUgKyAnYCBrZXkgYCcgKyBrZXkgKyAnYCBzdXBwbGllZCB0byBgJyArIGNvbXBvbmVudE5hbWUgKyAnYC4nICtcbiAgICAgICAgICAgICdcXG5CYWQgb2JqZWN0OiAnICsgSlNPTi5zdHJpbmdpZnkocHJvcHNbcHJvcE5hbWVdLCBudWxsLCAnICAnKSArXG4gICAgICAgICAgICAnXFxuVmFsaWQga2V5czogJyArICBKU09OLnN0cmluZ2lmeShPYmplY3Qua2V5cyhzaGFwZVR5cGVzKSwgbnVsbCwgJyAgJylcbiAgICAgICAgICApO1xuICAgICAgICB9XG4gICAgICAgIHZhciBlcnJvciA9IGNoZWNrZXIocHJvcFZhbHVlLCBrZXksIGNvbXBvbmVudE5hbWUsIGxvY2F0aW9uLCBwcm9wRnVsbE5hbWUgKyAnLicgKyBrZXksIFJlYWN0UHJvcFR5cGVzU2VjcmV0KTtcbiAgICAgICAgaWYgKGVycm9yKSB7XG4gICAgICAgICAgcmV0dXJuIGVycm9yO1xuICAgICAgICB9XG4gICAgICB9XG4gICAgICByZXR1cm4gbnVsbDtcbiAgICB9XG5cbiAgICByZXR1cm4gY3JlYXRlQ2hhaW5hYmxlVHlwZUNoZWNrZXIodmFsaWRhdGUpO1xuICB9XG5cbiAgZnVuY3Rpb24gaXNOb2RlKHByb3BWYWx1ZSkge1xuICAgIHN3aXRjaCAodHlwZW9mIHByb3BWYWx1ZSkge1xuICAgICAgY2FzZSAnbnVtYmVyJzpcbiAgICAgIGNhc2UgJ3N0cmluZyc6XG4gICAgICBjYXNlICd1bmRlZmluZWQnOlxuICAgICAgICByZXR1cm4gdHJ1ZTtcbiAgICAgIGNhc2UgJ2Jvb2xlYW4nOlxuICAgICAgICByZXR1cm4gIXByb3BWYWx1ZTtcbiAgICAgIGNhc2UgJ29iamVjdCc6XG4gICAgICAgIGlmIChBcnJheS5pc0FycmF5KHByb3BWYWx1ZSkpIHtcbiAgICAgICAgICByZXR1cm4gcHJvcFZhbHVlLmV2ZXJ5KGlzTm9kZSk7XG4gICAgICAgIH1cbiAgICAgICAgaWYgKHByb3BWYWx1ZSA9PT0gbnVsbCB8fCBpc1ZhbGlkRWxlbWVudChwcm9wVmFsdWUpKSB7XG4gICAgICAgICAgcmV0dXJuIHRydWU7XG4gICAgICAgIH1cblxuICAgICAgICB2YXIgaXRlcmF0b3JGbiA9IGdldEl0ZXJhdG9yRm4ocHJvcFZhbHVlKTtcbiAgICAgICAgaWYgKGl0ZXJhdG9yRm4pIHtcbiAgICAgICAgICB2YXIgaXRlcmF0b3IgPSBpdGVyYXRvckZuLmNhbGwocHJvcFZhbHVlKTtcbiAgICAgICAgICB2YXIgc3RlcDtcbiAgICAgICAgICBpZiAoaXRlcmF0b3JGbiAhPT0gcHJvcFZhbHVlLmVudHJpZXMpIHtcbiAgICAgICAgICAgIHdoaWxlICghKHN0ZXAgPSBpdGVyYXRvci5uZXh0KCkpLmRvbmUpIHtcbiAgICAgICAgICAgICAgaWYgKCFpc05vZGUoc3RlcC52YWx1ZSkpIHtcbiAgICAgICAgICAgICAgICByZXR1cm4gZmFsc2U7XG4gICAgICAgICAgICAgIH1cbiAgICAgICAgICAgIH1cbiAgICAgICAgICB9IGVsc2Uge1xuICAgICAgICAgICAgLy8gSXRlcmF0b3Igd2lsbCBwcm92aWRlIGVudHJ5IFtrLHZdIHR1cGxlcyByYXRoZXIgdGhhbiB2YWx1ZXMuXG4gICAgICAgICAgICB3aGlsZSAoIShzdGVwID0gaXRlcmF0b3IubmV4dCgpKS5kb25lKSB7XG4gICAgICAgICAgICAgIHZhciBlbnRyeSA9IHN0ZXAudmFsdWU7XG4gICAgICAgICAgICAgIGlmIChlbnRyeSkge1xuICAgICAgICAgICAgICAgIGlmICghaXNOb2RlKGVudHJ5WzFdKSkge1xuICAgICAgICAgICAgICAgICAgcmV0dXJuIGZhbHNlO1xuICAgICAgICAgICAgICAgIH1cbiAgICAgICAgICAgICAgfVxuICAgICAgICAgICAgfVxuICAgICAgICAgIH1cbiAgICAgICAgfSBlbHNlIHtcbiAgICAgICAgICByZXR1cm4gZmFsc2U7XG4gICAgICAgIH1cblxuICAgICAgICByZXR1cm4gdHJ1ZTtcbiAgICAgIGRlZmF1bHQ6XG4gICAgICAgIHJldHVybiBmYWxzZTtcbiAgICB9XG4gIH1cblxuICBmdW5jdGlvbiBpc1N5bWJvbChwcm9wVHlwZSwgcHJvcFZhbHVlKSB7XG4gICAgLy8gTmF0aXZlIFN5bWJvbC5cbiAgICBpZiAocHJvcFR5cGUgPT09ICdzeW1ib2wnKSB7XG4gICAgICByZXR1cm4gdHJ1ZTtcbiAgICB9XG5cbiAgICAvLyAxOS40LjMuNSBTeW1ib2wucHJvdG90eXBlW0BAdG9TdHJpbmdUYWddID09PSAnU3ltYm9sJ1xuICAgIGlmIChwcm9wVmFsdWVbJ0BAdG9TdHJpbmdUYWcnXSA9PT0gJ1N5bWJvbCcpIHtcbiAgICAgIHJldHVybiB0cnVlO1xuICAgIH1cblxuICAgIC8vIEZhbGxiYWNrIGZvciBub24tc3BlYyBjb21wbGlhbnQgU3ltYm9scyB3aGljaCBhcmUgcG9seWZpbGxlZC5cbiAgICBpZiAodHlwZW9mIFN5bWJvbCA9PT0gJ2Z1bmN0aW9uJyAmJiBwcm9wVmFsdWUgaW5zdGFuY2VvZiBTeW1ib2wpIHtcbiAgICAgIHJldHVybiB0cnVlO1xuICAgIH1cblxuICAgIHJldHVybiBmYWxzZTtcbiAgfVxuXG4gIC8vIEVxdWl2YWxlbnQgb2YgYHR5cGVvZmAgYnV0IHdpdGggc3BlY2lhbCBoYW5kbGluZyBmb3IgYXJyYXkgYW5kIHJlZ2V4cC5cbiAgZnVuY3Rpb24gZ2V0UHJvcFR5cGUocHJvcFZhbHVlKSB7XG4gICAgdmFyIHByb3BUeXBlID0gdHlwZW9mIHByb3BWYWx1ZTtcbiAgICBpZiAoQXJyYXkuaXNBcnJheShwcm9wVmFsdWUpKSB7XG4gICAgICByZXR1cm4gJ2FycmF5JztcbiAgICB9XG4gICAgaWYgKHByb3BWYWx1ZSBpbnN0YW5jZW9mIFJlZ0V4cCkge1xuICAgICAgLy8gT2xkIHdlYmtpdHMgKGF0IGxlYXN0IHVudGlsIEFuZHJvaWQgNC4wKSByZXR1cm4gJ2Z1bmN0aW9uJyByYXRoZXIgdGhhblxuICAgICAgLy8gJ29iamVjdCcgZm9yIHR5cGVvZiBhIFJlZ0V4cC4gV2UnbGwgbm9ybWFsaXplIHRoaXMgaGVyZSBzbyB0aGF0IC9ibGEvXG4gICAgICAvLyBwYXNzZXMgUHJvcFR5cGVzLm9iamVjdC5cbiAgICAgIHJldHVybiAnb2JqZWN0JztcbiAgICB9XG4gICAgaWYgKGlzU3ltYm9sKHByb3BUeXBlLCBwcm9wVmFsdWUpKSB7XG4gICAgICByZXR1cm4gJ3N5bWJvbCc7XG4gICAgfVxuICAgIHJldHVybiBwcm9wVHlwZTtcbiAgfVxuXG4gIC8vIFRoaXMgaGFuZGxlcyBtb3JlIHR5cGVzIHRoYW4gYGdldFByb3BUeXBlYC4gT25seSB1c2VkIGZvciBlcnJvciBtZXNzYWdlcy5cbiAgLy8gU2VlIGBjcmVhdGVQcmltaXRpdmVUeXBlQ2hlY2tlcmAuXG4gIGZ1bmN0aW9uIGdldFByZWNpc2VUeXBlKHByb3BWYWx1ZSkge1xuICAgIGlmICh0eXBlb2YgcHJvcFZhbHVlID09PSAndW5kZWZpbmVkJyB8fCBwcm9wVmFsdWUgPT09IG51bGwpIHtcbiAgICAgIHJldHVybiAnJyArIHByb3BWYWx1ZTtcbiAgICB9XG4gICAgdmFyIHByb3BUeXBlID0gZ2V0UHJvcFR5cGUocHJvcFZhbHVlKTtcbiAgICBpZiAocHJvcFR5cGUgPT09ICdvYmplY3QnKSB7XG4gICAgICBpZiAocHJvcFZhbHVlIGluc3RhbmNlb2YgRGF0ZSkge1xuICAgICAgICByZXR1cm4gJ2RhdGUnO1xuICAgICAgfSBlbHNlIGlmIChwcm9wVmFsdWUgaW5zdGFuY2VvZiBSZWdFeHApIHtcbiAgICAgICAgcmV0dXJuICdyZWdleHAnO1xuICAgICAgfVxuICAgIH1cbiAgICByZXR1cm4gcHJvcFR5cGU7XG4gIH1cblxuICAvLyBSZXR1cm5zIGEgc3RyaW5nIHRoYXQgaXMgcG9zdGZpeGVkIHRvIGEgd2FybmluZyBhYm91dCBhbiBpbnZhbGlkIHR5cGUuXG4gIC8vIEZvciBleGFtcGxlLCBcInVuZGVmaW5lZFwiIG9yIFwib2YgdHlwZSBhcnJheVwiXG4gIGZ1bmN0aW9uIGdldFBvc3RmaXhGb3JUeXBlV2FybmluZyh2YWx1ZSkge1xuICAgIHZhciB0eXBlID0gZ2V0UHJlY2lzZVR5cGUodmFsdWUpO1xuICAgIHN3aXRjaCAodHlwZSkge1xuICAgICAgY2FzZSAnYXJyYXknOlxuICAgICAgY2FzZSAnb2JqZWN0JzpcbiAgICAgICAgcmV0dXJuICdhbiAnICsgdHlwZTtcbiAgICAgIGNhc2UgJ2Jvb2xlYW4nOlxuICAgICAgY2FzZSAnZGF0ZSc6XG4gICAgICBjYXNlICdyZWdleHAnOlxuICAgICAgICByZXR1cm4gJ2EgJyArIHR5cGU7XG4gICAgICBkZWZhdWx0OlxuICAgICAgICByZXR1cm4gdHlwZTtcbiAgICB9XG4gIH1cblxuICAvLyBSZXR1cm5zIGNsYXNzIG5hbWUgb2YgdGhlIG9iamVjdCwgaWYgYW55LlxuICBmdW5jdGlvbiBnZXRDbGFzc05hbWUocHJvcFZhbHVlKSB7XG4gICAgaWYgKCFwcm9wVmFsdWUuY29uc3RydWN0b3IgfHwgIXByb3BWYWx1ZS5jb25zdHJ1Y3Rvci5uYW1lKSB7XG4gICAgICByZXR1cm4gQU5PTllNT1VTO1xuICAgIH1cbiAgICByZXR1cm4gcHJvcFZhbHVlLmNvbnN0cnVjdG9yLm5hbWU7XG4gIH1cblxuICBSZWFjdFByb3BUeXBlcy5jaGVja1Byb3BUeXBlcyA9IGNoZWNrUHJvcFR5cGVzO1xuICBSZWFjdFByb3BUeXBlcy5Qcm9wVHlwZXMgPSBSZWFjdFByb3BUeXBlcztcblxuICByZXR1cm4gUmVhY3RQcm9wVHlwZXM7XG59O1xuIiwiLyoqXG4gKiBDb3B5cmlnaHQgKGMpIDIwMTMtcHJlc2VudCwgRmFjZWJvb2ssIEluYy5cbiAqXG4gKiBUaGlzIHNvdXJjZSBjb2RlIGlzIGxpY2Vuc2VkIHVuZGVyIHRoZSBNSVQgbGljZW5zZSBmb3VuZCBpbiB0aGVcbiAqIExJQ0VOU0UgZmlsZSBpbiB0aGUgcm9vdCBkaXJlY3Rvcnkgb2YgdGhpcyBzb3VyY2UgdHJlZS5cbiAqL1xuXG5pZiAocHJvY2Vzcy5lbnYuTk9ERV9FTlYgIT09ICdwcm9kdWN0aW9uJykge1xuICB2YXIgUkVBQ1RfRUxFTUVOVF9UWVBFID0gKHR5cGVvZiBTeW1ib2wgPT09ICdmdW5jdGlvbicgJiZcbiAgICBTeW1ib2wuZm9yICYmXG4gICAgU3ltYm9sLmZvcigncmVhY3QuZWxlbWVudCcpKSB8fFxuICAgIDB4ZWFjNztcblxuICB2YXIgaXNWYWxpZEVsZW1lbnQgPSBmdW5jdGlvbihvYmplY3QpIHtcbiAgICByZXR1cm4gdHlwZW9mIG9iamVjdCA9PT0gJ29iamVjdCcgJiZcbiAgICAgIG9iamVjdCAhPT0gbnVsbCAmJlxuICAgICAgb2JqZWN0LiQkdHlwZW9mID09PSBSRUFDVF9FTEVNRU5UX1RZUEU7XG4gIH07XG5cbiAgLy8gQnkgZXhwbGljaXRseSB1c2luZyBgcHJvcC10eXBlc2AgeW91IGFyZSBvcHRpbmcgaW50byBuZXcgZGV2ZWxvcG1lbnQgYmVoYXZpb3IuXG4gIC8vIGh0dHA6Ly9mYi5tZS9wcm9wLXR5cGVzLWluLXByb2RcbiAgdmFyIHRocm93T25EaXJlY3RBY2Nlc3MgPSB0cnVlO1xuICBtb2R1bGUuZXhwb3J0cyA9IHJlcXVpcmUoJy4vZmFjdG9yeVdpdGhUeXBlQ2hlY2tlcnMnKShpc1ZhbGlkRWxlbWVudCwgdGhyb3dPbkRpcmVjdEFjY2Vzcyk7XG59IGVsc2Uge1xuICAvLyBCeSBleHBsaWNpdGx5IHVzaW5nIGBwcm9wLXR5cGVzYCB5b3UgYXJlIG9wdGluZyBpbnRvIG5ldyBwcm9kdWN0aW9uIGJlaGF2aW9yLlxuICAvLyBodHRwOi8vZmIubWUvcHJvcC10eXBlcy1pbi1wcm9kXG4gIG1vZHVsZS5leHBvcnRzID0gcmVxdWlyZSgnLi9mYWN0b3J5V2l0aFRocm93aW5nU2hpbXMnKSgpO1xufVxuIiwiLyoqXG4gKiBDb3B5cmlnaHQgKGMpIDIwMTMtcHJlc2VudCwgRmFjZWJvb2ssIEluYy5cbiAqXG4gKiBUaGlzIHNvdXJjZSBjb2RlIGlzIGxpY2Vuc2VkIHVuZGVyIHRoZSBNSVQgbGljZW5zZSBmb3VuZCBpbiB0aGVcbiAqIExJQ0VOU0UgZmlsZSBpbiB0aGUgcm9vdCBkaXJlY3Rvcnkgb2YgdGhpcyBzb3VyY2UgdHJlZS5cbiAqL1xuXG4ndXNlIHN0cmljdCc7XG5cbnZhciBSZWFjdFByb3BUeXBlc1NlY3JldCA9ICdTRUNSRVRfRE9fTk9UX1BBU1NfVEhJU19PUl9ZT1VfV0lMTF9CRV9GSVJFRCc7XG5cbm1vZHVsZS5leHBvcnRzID0gUmVhY3RQcm9wVHlwZXNTZWNyZXQ7XG4iLCIndXNlIHN0cmljdCc7XG52YXIgc3RyaWN0VXJpRW5jb2RlID0gcmVxdWlyZSgnc3RyaWN0LXVyaS1lbmNvZGUnKTtcbnZhciBvYmplY3RBc3NpZ24gPSByZXF1aXJlKCdvYmplY3QtYXNzaWduJyk7XG52YXIgZGVjb2RlQ29tcG9uZW50ID0gcmVxdWlyZSgnZGVjb2RlLXVyaS1jb21wb25lbnQnKTtcblxuZnVuY3Rpb24gZW5jb2RlckZvckFycmF5Rm9ybWF0KG9wdHMpIHtcblx0c3dpdGNoIChvcHRzLmFycmF5Rm9ybWF0KSB7XG5cdFx0Y2FzZSAnaW5kZXgnOlxuXHRcdFx0cmV0dXJuIGZ1bmN0aW9uIChrZXksIHZhbHVlLCBpbmRleCkge1xuXHRcdFx0XHRyZXR1cm4gdmFsdWUgPT09IG51bGwgPyBbXG5cdFx0XHRcdFx0ZW5jb2RlKGtleSwgb3B0cyksXG5cdFx0XHRcdFx0J1snLFxuXHRcdFx0XHRcdGluZGV4LFxuXHRcdFx0XHRcdCddJ1xuXHRcdFx0XHRdLmpvaW4oJycpIDogW1xuXHRcdFx0XHRcdGVuY29kZShrZXksIG9wdHMpLFxuXHRcdFx0XHRcdCdbJyxcblx0XHRcdFx0XHRlbmNvZGUoaW5kZXgsIG9wdHMpLFxuXHRcdFx0XHRcdCddPScsXG5cdFx0XHRcdFx0ZW5jb2RlKHZhbHVlLCBvcHRzKVxuXHRcdFx0XHRdLmpvaW4oJycpO1xuXHRcdFx0fTtcblxuXHRcdGNhc2UgJ2JyYWNrZXQnOlxuXHRcdFx0cmV0dXJuIGZ1bmN0aW9uIChrZXksIHZhbHVlKSB7XG5cdFx0XHRcdHJldHVybiB2YWx1ZSA9PT0gbnVsbCA/IGVuY29kZShrZXksIG9wdHMpIDogW1xuXHRcdFx0XHRcdGVuY29kZShrZXksIG9wdHMpLFxuXHRcdFx0XHRcdCdbXT0nLFxuXHRcdFx0XHRcdGVuY29kZSh2YWx1ZSwgb3B0cylcblx0XHRcdFx0XS5qb2luKCcnKTtcblx0XHRcdH07XG5cblx0XHRkZWZhdWx0OlxuXHRcdFx0cmV0dXJuIGZ1bmN0aW9uIChrZXksIHZhbHVlKSB7XG5cdFx0XHRcdHJldHVybiB2YWx1ZSA9PT0gbnVsbCA/IGVuY29kZShrZXksIG9wdHMpIDogW1xuXHRcdFx0XHRcdGVuY29kZShrZXksIG9wdHMpLFxuXHRcdFx0XHRcdCc9Jyxcblx0XHRcdFx0XHRlbmNvZGUodmFsdWUsIG9wdHMpXG5cdFx0XHRcdF0uam9pbignJyk7XG5cdFx0XHR9O1xuXHR9XG59XG5cbmZ1bmN0aW9uIHBhcnNlckZvckFycmF5Rm9ybWF0KG9wdHMpIHtcblx0dmFyIHJlc3VsdDtcblxuXHRzd2l0Y2ggKG9wdHMuYXJyYXlGb3JtYXQpIHtcblx0XHRjYXNlICdpbmRleCc6XG5cdFx0XHRyZXR1cm4gZnVuY3Rpb24gKGtleSwgdmFsdWUsIGFjY3VtdWxhdG9yKSB7XG5cdFx0XHRcdHJlc3VsdCA9IC9cXFsoXFxkKilcXF0kLy5leGVjKGtleSk7XG5cblx0XHRcdFx0a2V5ID0ga2V5LnJlcGxhY2UoL1xcW1xcZCpcXF0kLywgJycpO1xuXG5cdFx0XHRcdGlmICghcmVzdWx0KSB7XG5cdFx0XHRcdFx0YWNjdW11bGF0b3Jba2V5XSA9IHZhbHVlO1xuXHRcdFx0XHRcdHJldHVybjtcblx0XHRcdFx0fVxuXG5cdFx0XHRcdGlmIChhY2N1bXVsYXRvcltrZXldID09PSB1bmRlZmluZWQpIHtcblx0XHRcdFx0XHRhY2N1bXVsYXRvcltrZXldID0ge307XG5cdFx0XHRcdH1cblxuXHRcdFx0XHRhY2N1bXVsYXRvcltrZXldW3Jlc3VsdFsxXV0gPSB2YWx1ZTtcblx0XHRcdH07XG5cblx0XHRjYXNlICdicmFja2V0Jzpcblx0XHRcdHJldHVybiBmdW5jdGlvbiAoa2V5LCB2YWx1ZSwgYWNjdW11bGF0b3IpIHtcblx0XHRcdFx0cmVzdWx0ID0gLyhcXFtcXF0pJC8uZXhlYyhrZXkpO1xuXHRcdFx0XHRrZXkgPSBrZXkucmVwbGFjZSgvXFxbXFxdJC8sICcnKTtcblxuXHRcdFx0XHRpZiAoIXJlc3VsdCkge1xuXHRcdFx0XHRcdGFjY3VtdWxhdG9yW2tleV0gPSB2YWx1ZTtcblx0XHRcdFx0XHRyZXR1cm47XG5cdFx0XHRcdH0gZWxzZSBpZiAoYWNjdW11bGF0b3Jba2V5XSA9PT0gdW5kZWZpbmVkKSB7XG5cdFx0XHRcdFx0YWNjdW11bGF0b3Jba2V5XSA9IFt2YWx1ZV07XG5cdFx0XHRcdFx0cmV0dXJuO1xuXHRcdFx0XHR9XG5cblx0XHRcdFx0YWNjdW11bGF0b3Jba2V5XSA9IFtdLmNvbmNhdChhY2N1bXVsYXRvcltrZXldLCB2YWx1ZSk7XG5cdFx0XHR9O1xuXG5cdFx0ZGVmYXVsdDpcblx0XHRcdHJldHVybiBmdW5jdGlvbiAoa2V5LCB2YWx1ZSwgYWNjdW11bGF0b3IpIHtcblx0XHRcdFx0aWYgKGFjY3VtdWxhdG9yW2tleV0gPT09IHVuZGVmaW5lZCkge1xuXHRcdFx0XHRcdGFjY3VtdWxhdG9yW2tleV0gPSB2YWx1ZTtcblx0XHRcdFx0XHRyZXR1cm47XG5cdFx0XHRcdH1cblxuXHRcdFx0XHRhY2N1bXVsYXRvcltrZXldID0gW10uY29uY2F0KGFjY3VtdWxhdG9yW2tleV0sIHZhbHVlKTtcblx0XHRcdH07XG5cdH1cbn1cblxuZnVuY3Rpb24gZW5jb2RlKHZhbHVlLCBvcHRzKSB7XG5cdGlmIChvcHRzLmVuY29kZSkge1xuXHRcdHJldHVybiBvcHRzLnN0cmljdCA/IHN0cmljdFVyaUVuY29kZSh2YWx1ZSkgOiBlbmNvZGVVUklDb21wb25lbnQodmFsdWUpO1xuXHR9XG5cblx0cmV0dXJuIHZhbHVlO1xufVxuXG5mdW5jdGlvbiBrZXlzU29ydGVyKGlucHV0KSB7XG5cdGlmIChBcnJheS5pc0FycmF5KGlucHV0KSkge1xuXHRcdHJldHVybiBpbnB1dC5zb3J0KCk7XG5cdH0gZWxzZSBpZiAodHlwZW9mIGlucHV0ID09PSAnb2JqZWN0Jykge1xuXHRcdHJldHVybiBrZXlzU29ydGVyKE9iamVjdC5rZXlzKGlucHV0KSkuc29ydChmdW5jdGlvbiAoYSwgYikge1xuXHRcdFx0cmV0dXJuIE51bWJlcihhKSAtIE51bWJlcihiKTtcblx0XHR9KS5tYXAoZnVuY3Rpb24gKGtleSkge1xuXHRcdFx0cmV0dXJuIGlucHV0W2tleV07XG5cdFx0fSk7XG5cdH1cblxuXHRyZXR1cm4gaW5wdXQ7XG59XG5cbmZ1bmN0aW9uIGV4dHJhY3Qoc3RyKSB7XG5cdHZhciBxdWVyeVN0YXJ0ID0gc3RyLmluZGV4T2YoJz8nKTtcblx0aWYgKHF1ZXJ5U3RhcnQgPT09IC0xKSB7XG5cdFx0cmV0dXJuICcnO1xuXHR9XG5cdHJldHVybiBzdHIuc2xpY2UocXVlcnlTdGFydCArIDEpO1xufVxuXG5mdW5jdGlvbiBwYXJzZShzdHIsIG9wdHMpIHtcblx0b3B0cyA9IG9iamVjdEFzc2lnbih7YXJyYXlGb3JtYXQ6ICdub25lJ30sIG9wdHMpO1xuXG5cdHZhciBmb3JtYXR0ZXIgPSBwYXJzZXJGb3JBcnJheUZvcm1hdChvcHRzKTtcblxuXHQvLyBDcmVhdGUgYW4gb2JqZWN0IHdpdGggbm8gcHJvdG90eXBlXG5cdC8vIGh0dHBzOi8vZ2l0aHViLmNvbS9zaW5kcmVzb3JodXMvcXVlcnktc3RyaW5nL2lzc3Vlcy80N1xuXHR2YXIgcmV0ID0gT2JqZWN0LmNyZWF0ZShudWxsKTtcblxuXHRpZiAodHlwZW9mIHN0ciAhPT0gJ3N0cmluZycpIHtcblx0XHRyZXR1cm4gcmV0O1xuXHR9XG5cblx0c3RyID0gc3RyLnRyaW0oKS5yZXBsYWNlKC9eWz8jJl0vLCAnJyk7XG5cblx0aWYgKCFzdHIpIHtcblx0XHRyZXR1cm4gcmV0O1xuXHR9XG5cblx0c3RyLnNwbGl0KCcmJykuZm9yRWFjaChmdW5jdGlvbiAocGFyYW0pIHtcblx0XHR2YXIgcGFydHMgPSBwYXJhbS5yZXBsYWNlKC9cXCsvZywgJyAnKS5zcGxpdCgnPScpO1xuXHRcdC8vIEZpcmVmb3ggKHByZSA0MCkgZGVjb2RlcyBgJTNEYCB0byBgPWBcblx0XHQvLyBodHRwczovL2dpdGh1Yi5jb20vc2luZHJlc29yaHVzL3F1ZXJ5LXN0cmluZy9wdWxsLzM3XG5cdFx0dmFyIGtleSA9IHBhcnRzLnNoaWZ0KCk7XG5cdFx0dmFyIHZhbCA9IHBhcnRzLmxlbmd0aCA+IDAgPyBwYXJ0cy5qb2luKCc9JykgOiB1bmRlZmluZWQ7XG5cblx0XHQvLyBtaXNzaW5nIGA9YCBzaG91bGQgYmUgYG51bGxgOlxuXHRcdC8vIGh0dHA6Ly93My5vcmcvVFIvMjAxMi9XRC11cmwtMjAxMjA1MjQvI2NvbGxlY3QtdXJsLXBhcmFtZXRlcnNcblx0XHR2YWwgPSB2YWwgPT09IHVuZGVmaW5lZCA/IG51bGwgOiBkZWNvZGVDb21wb25lbnQodmFsKTtcblxuXHRcdGZvcm1hdHRlcihkZWNvZGVDb21wb25lbnQoa2V5KSwgdmFsLCByZXQpO1xuXHR9KTtcblxuXHRyZXR1cm4gT2JqZWN0LmtleXMocmV0KS5zb3J0KCkucmVkdWNlKGZ1bmN0aW9uIChyZXN1bHQsIGtleSkge1xuXHRcdHZhciB2YWwgPSByZXRba2V5XTtcblx0XHRpZiAoQm9vbGVhbih2YWwpICYmIHR5cGVvZiB2YWwgPT09ICdvYmplY3QnICYmICFBcnJheS5pc0FycmF5KHZhbCkpIHtcblx0XHRcdC8vIFNvcnQgb2JqZWN0IGtleXMsIG5vdCB2YWx1ZXNcblx0XHRcdHJlc3VsdFtrZXldID0ga2V5c1NvcnRlcih2YWwpO1xuXHRcdH0gZWxzZSB7XG5cdFx0XHRyZXN1bHRba2V5XSA9IHZhbDtcblx0XHR9XG5cblx0XHRyZXR1cm4gcmVzdWx0O1xuXHR9LCBPYmplY3QuY3JlYXRlKG51bGwpKTtcbn1cblxuZXhwb3J0cy5leHRyYWN0ID0gZXh0cmFjdDtcbmV4cG9ydHMucGFyc2UgPSBwYXJzZTtcblxuZXhwb3J0cy5zdHJpbmdpZnkgPSBmdW5jdGlvbiAob2JqLCBvcHRzKSB7XG5cdHZhciBkZWZhdWx0cyA9IHtcblx0XHRlbmNvZGU6IHRydWUsXG5cdFx0c3RyaWN0OiB0cnVlLFxuXHRcdGFycmF5Rm9ybWF0OiAnbm9uZSdcblx0fTtcblxuXHRvcHRzID0gb2JqZWN0QXNzaWduKGRlZmF1bHRzLCBvcHRzKTtcblxuXHRpZiAob3B0cy5zb3J0ID09PSBmYWxzZSkge1xuXHRcdG9wdHMuc29ydCA9IGZ1bmN0aW9uICgpIHt9O1xuXHR9XG5cblx0dmFyIGZvcm1hdHRlciA9IGVuY29kZXJGb3JBcnJheUZvcm1hdChvcHRzKTtcblxuXHRyZXR1cm4gb2JqID8gT2JqZWN0LmtleXMob2JqKS5zb3J0KG9wdHMuc29ydCkubWFwKGZ1bmN0aW9uIChrZXkpIHtcblx0XHR2YXIgdmFsID0gb2JqW2tleV07XG5cblx0XHRpZiAodmFsID09PSB1bmRlZmluZWQpIHtcblx0XHRcdHJldHVybiAnJztcblx0XHR9XG5cblx0XHRpZiAodmFsID09PSBudWxsKSB7XG5cdFx0XHRyZXR1cm4gZW5jb2RlKGtleSwgb3B0cyk7XG5cdFx0fVxuXG5cdFx0aWYgKEFycmF5LmlzQXJyYXkodmFsKSkge1xuXHRcdFx0dmFyIHJlc3VsdCA9IFtdO1xuXG5cdFx0XHR2YWwuc2xpY2UoKS5mb3JFYWNoKGZ1bmN0aW9uICh2YWwyKSB7XG5cdFx0XHRcdGlmICh2YWwyID09PSB1bmRlZmluZWQpIHtcblx0XHRcdFx0XHRyZXR1cm47XG5cdFx0XHRcdH1cblxuXHRcdFx0XHRyZXN1bHQucHVzaChmb3JtYXR0ZXIoa2V5LCB2YWwyLCByZXN1bHQubGVuZ3RoKSk7XG5cdFx0XHR9KTtcblxuXHRcdFx0cmV0dXJuIHJlc3VsdC5qb2luKCcmJyk7XG5cdFx0fVxuXG5cdFx0cmV0dXJuIGVuY29kZShrZXksIG9wdHMpICsgJz0nICsgZW5jb2RlKHZhbCwgb3B0cyk7XG5cdH0pLmZpbHRlcihmdW5jdGlvbiAoeCkge1xuXHRcdHJldHVybiB4Lmxlbmd0aCA+IDA7XG5cdH0pLmpvaW4oJyYnKSA6ICcnO1xufTtcblxuZXhwb3J0cy5wYXJzZVVybCA9IGZ1bmN0aW9uIChzdHIsIG9wdHMpIHtcblx0cmV0dXJuIHtcblx0XHR1cmw6IHN0ci5zcGxpdCgnPycpWzBdIHx8ICcnLFxuXHRcdHF1ZXJ5OiBwYXJzZShleHRyYWN0KHN0ciksIG9wdHMpXG5cdH07XG59O1xuIiwiJ3VzZSBzdHJpY3QnO1xuXG52YXIgYXNzaWduID0gcmVxdWlyZSgnb2JqZWN0LWFzc2lnbicpLFxuXHRQcm9wVHlwZXMgPSByZXF1aXJlKCdwcm9wLXR5cGVzJyksXG5cdGNyZWF0ZUNsYXNzID0gcmVxdWlyZSgnY3JlYXRlLXJlYWN0LWNsYXNzJyksXG5cdG1vbWVudCA9IHJlcXVpcmUoJ21vbWVudCcpLFxuXHRSZWFjdCA9IHJlcXVpcmUoJ3JlYWN0JyksXG5cdENhbGVuZGFyQ29udGFpbmVyID0gcmVxdWlyZSgnLi9zcmMvQ2FsZW5kYXJDb250YWluZXInKVxuXHQ7XG5cbnZhciB2aWV3TW9kZXMgPSBPYmplY3QuZnJlZXplKHtcblx0WUVBUlM6ICd5ZWFycycsXG5cdE1PTlRIUzogJ21vbnRocycsXG5cdERBWVM6ICdkYXlzJyxcblx0VElNRTogJ3RpbWUnLFxufSk7XG5cbnZhciBUWVBFUyA9IFByb3BUeXBlcztcbnZhciBEYXRldGltZSA9IGNyZWF0ZUNsYXNzKHtcblx0cHJvcFR5cGVzOiB7XG5cdFx0Ly8gdmFsdWU6IFRZUEVTLm9iamVjdCB8IFRZUEVTLnN0cmluZyxcblx0XHQvLyBkZWZhdWx0VmFsdWU6IFRZUEVTLm9iamVjdCB8IFRZUEVTLnN0cmluZyxcblx0XHQvLyB2aWV3RGF0ZTogVFlQRVMub2JqZWN0IHwgVFlQRVMuc3RyaW5nLFxuXHRcdG9uRm9jdXM6IFRZUEVTLmZ1bmMsXG5cdFx0b25CbHVyOiBUWVBFUy5mdW5jLFxuXHRcdG9uQ2hhbmdlOiBUWVBFUy5mdW5jLFxuXHRcdG9uVmlld01vZGVDaGFuZ2U6IFRZUEVTLmZ1bmMsXG5cdFx0bG9jYWxlOiBUWVBFUy5zdHJpbmcsXG5cdFx0dXRjOiBUWVBFUy5ib29sLFxuXHRcdGlucHV0OiBUWVBFUy5ib29sLFxuXHRcdC8vIGRhdGVGb3JtYXQ6IFRZUEVTLnN0cmluZyB8IFRZUEVTLmJvb2wsXG5cdFx0Ly8gdGltZUZvcm1hdDogVFlQRVMuc3RyaW5nIHwgVFlQRVMuYm9vbCxcblx0XHRpbnB1dFByb3BzOiBUWVBFUy5vYmplY3QsXG5cdFx0dGltZUNvbnN0cmFpbnRzOiBUWVBFUy5vYmplY3QsXG5cdFx0dmlld01vZGU6IFRZUEVTLm9uZU9mKFt2aWV3TW9kZXMuWUVBUlMsIHZpZXdNb2Rlcy5NT05USFMsIHZpZXdNb2Rlcy5EQVlTLCB2aWV3TW9kZXMuVElNRV0pLFxuXHRcdGlzVmFsaWREYXRlOiBUWVBFUy5mdW5jLFxuXHRcdG9wZW46IFRZUEVTLmJvb2wsXG5cdFx0c3RyaWN0UGFyc2luZzogVFlQRVMuYm9vbCxcblx0XHRjbG9zZU9uU2VsZWN0OiBUWVBFUy5ib29sLFxuXHRcdGNsb3NlT25UYWI6IFRZUEVTLmJvb2xcblx0fSxcblxuXHRnZXRJbml0aWFsU3RhdGU6IGZ1bmN0aW9uKCkge1xuXHRcdHZhciBzdGF0ZSA9IHRoaXMuZ2V0U3RhdGVGcm9tUHJvcHMoIHRoaXMucHJvcHMgKTtcblxuXHRcdGlmICggc3RhdGUub3BlbiA9PT0gdW5kZWZpbmVkIClcblx0XHRcdHN0YXRlLm9wZW4gPSAhdGhpcy5wcm9wcy5pbnB1dDtcblxuXHRcdHN0YXRlLmN1cnJlbnRWaWV3ID0gdGhpcy5wcm9wcy5kYXRlRm9ybWF0ID9cblx0XHRcdCh0aGlzLnByb3BzLnZpZXdNb2RlIHx8IHN0YXRlLnVwZGF0ZU9uIHx8IHZpZXdNb2Rlcy5EQVlTKSA6IHZpZXdNb2Rlcy5USU1FO1xuXG5cdFx0cmV0dXJuIHN0YXRlO1xuXHR9LFxuXG5cdHBhcnNlRGF0ZTogZnVuY3Rpb24gKGRhdGUsIGZvcm1hdHMpIHtcblx0XHR2YXIgcGFyc2VkRGF0ZTtcblxuXHRcdGlmIChkYXRlICYmIHR5cGVvZiBkYXRlID09PSAnc3RyaW5nJylcblx0XHRcdHBhcnNlZERhdGUgPSB0aGlzLmxvY2FsTW9tZW50KGRhdGUsIGZvcm1hdHMuZGF0ZXRpbWUpO1xuXHRcdGVsc2UgaWYgKGRhdGUpXG5cdFx0XHRwYXJzZWREYXRlID0gdGhpcy5sb2NhbE1vbWVudChkYXRlKTtcblxuXHRcdGlmIChwYXJzZWREYXRlICYmICFwYXJzZWREYXRlLmlzVmFsaWQoKSlcblx0XHRcdHBhcnNlZERhdGUgPSBudWxsO1xuXG5cdFx0cmV0dXJuIHBhcnNlZERhdGU7XG5cdH0sXG5cblx0Z2V0U3RhdGVGcm9tUHJvcHM6IGZ1bmN0aW9uKCBwcm9wcyApIHtcblx0XHR2YXIgZm9ybWF0cyA9IHRoaXMuZ2V0Rm9ybWF0cyggcHJvcHMgKSxcblx0XHRcdGRhdGUgPSBwcm9wcy52YWx1ZSB8fCBwcm9wcy5kZWZhdWx0VmFsdWUsXG5cdFx0XHRzZWxlY3RlZERhdGUsIHZpZXdEYXRlLCB1cGRhdGVPbiwgaW5wdXRWYWx1ZVxuXHRcdFx0O1xuXG5cdFx0c2VsZWN0ZWREYXRlID0gdGhpcy5wYXJzZURhdGUoZGF0ZSwgZm9ybWF0cyk7XG5cblx0XHR2aWV3RGF0ZSA9IHRoaXMucGFyc2VEYXRlKHByb3BzLnZpZXdEYXRlLCBmb3JtYXRzKTtcblxuXHRcdHZpZXdEYXRlID0gc2VsZWN0ZWREYXRlID9cblx0XHRcdHNlbGVjdGVkRGF0ZS5jbG9uZSgpLnN0YXJ0T2YoJ21vbnRoJykgOlxuXHRcdFx0dmlld0RhdGUgPyB2aWV3RGF0ZS5jbG9uZSgpLnN0YXJ0T2YoJ21vbnRoJykgOiB0aGlzLmxvY2FsTW9tZW50KCkuc3RhcnRPZignbW9udGgnKTtcblxuXHRcdHVwZGF0ZU9uID0gdGhpcy5nZXRVcGRhdGVPbihmb3JtYXRzKTtcblxuXHRcdGlmICggc2VsZWN0ZWREYXRlIClcblx0XHRcdGlucHV0VmFsdWUgPSBzZWxlY3RlZERhdGUuZm9ybWF0KGZvcm1hdHMuZGF0ZXRpbWUpO1xuXHRcdGVsc2UgaWYgKCBkYXRlLmlzVmFsaWQgJiYgIWRhdGUuaXNWYWxpZCgpIClcblx0XHRcdGlucHV0VmFsdWUgPSAnJztcblx0XHRlbHNlXG5cdFx0XHRpbnB1dFZhbHVlID0gZGF0ZSB8fCAnJztcblxuXHRcdHJldHVybiB7XG5cdFx0XHR1cGRhdGVPbjogdXBkYXRlT24sXG5cdFx0XHRpbnB1dEZvcm1hdDogZm9ybWF0cy5kYXRldGltZSxcblx0XHRcdHZpZXdEYXRlOiB2aWV3RGF0ZSxcblx0XHRcdHNlbGVjdGVkRGF0ZTogc2VsZWN0ZWREYXRlLFxuXHRcdFx0aW5wdXRWYWx1ZTogaW5wdXRWYWx1ZSxcblx0XHRcdG9wZW46IHByb3BzLm9wZW5cblx0XHR9O1xuXHR9LFxuXG5cdGdldFVwZGF0ZU9uOiBmdW5jdGlvbiggZm9ybWF0cyApIHtcblx0XHRpZiAoIGZvcm1hdHMuZGF0ZS5tYXRjaCgvW2xMRF0vKSApIHtcblx0XHRcdHJldHVybiB2aWV3TW9kZXMuREFZUztcblx0XHR9IGVsc2UgaWYgKCBmb3JtYXRzLmRhdGUuaW5kZXhPZignTScpICE9PSAtMSApIHtcblx0XHRcdHJldHVybiB2aWV3TW9kZXMuTU9OVEhTO1xuXHRcdH0gZWxzZSBpZiAoIGZvcm1hdHMuZGF0ZS5pbmRleE9mKCdZJykgIT09IC0xICkge1xuXHRcdFx0cmV0dXJuIHZpZXdNb2Rlcy5ZRUFSUztcblx0XHR9XG5cblx0XHRyZXR1cm4gdmlld01vZGVzLkRBWVM7XG5cdH0sXG5cblx0Z2V0Rm9ybWF0czogZnVuY3Rpb24oIHByb3BzICkge1xuXHRcdHZhciBmb3JtYXRzID0ge1xuXHRcdFx0XHRkYXRlOiBwcm9wcy5kYXRlRm9ybWF0IHx8ICcnLFxuXHRcdFx0XHR0aW1lOiBwcm9wcy50aW1lRm9ybWF0IHx8ICcnXG5cdFx0XHR9LFxuXHRcdFx0bG9jYWxlID0gdGhpcy5sb2NhbE1vbWVudCggcHJvcHMuZGF0ZSwgbnVsbCwgcHJvcHMgKS5sb2NhbGVEYXRhKClcblx0XHRcdDtcblxuXHRcdGlmICggZm9ybWF0cy5kYXRlID09PSB0cnVlICkge1xuXHRcdFx0Zm9ybWF0cy5kYXRlID0gbG9jYWxlLmxvbmdEYXRlRm9ybWF0KCdMJyk7XG5cdFx0fVxuXHRcdGVsc2UgaWYgKCB0aGlzLmdldFVwZGF0ZU9uKGZvcm1hdHMpICE9PSB2aWV3TW9kZXMuREFZUyApIHtcblx0XHRcdGZvcm1hdHMudGltZSA9ICcnO1xuXHRcdH1cblxuXHRcdGlmICggZm9ybWF0cy50aW1lID09PSB0cnVlICkge1xuXHRcdFx0Zm9ybWF0cy50aW1lID0gbG9jYWxlLmxvbmdEYXRlRm9ybWF0KCdMVCcpO1xuXHRcdH1cblxuXHRcdGZvcm1hdHMuZGF0ZXRpbWUgPSBmb3JtYXRzLmRhdGUgJiYgZm9ybWF0cy50aW1lID9cblx0XHRcdGZvcm1hdHMuZGF0ZSArICcgJyArIGZvcm1hdHMudGltZSA6XG5cdFx0XHRmb3JtYXRzLmRhdGUgfHwgZm9ybWF0cy50aW1lXG5cdFx0O1xuXG5cdFx0cmV0dXJuIGZvcm1hdHM7XG5cdH0sXG5cblx0Y29tcG9uZW50V2lsbFJlY2VpdmVQcm9wczogZnVuY3Rpb24oIG5leHRQcm9wcyApIHtcblx0XHR2YXIgZm9ybWF0cyA9IHRoaXMuZ2V0Rm9ybWF0cyggbmV4dFByb3BzICksXG5cdFx0XHR1cGRhdGVkU3RhdGUgPSB7fVxuXHRcdDtcblxuXHRcdGlmICggbmV4dFByb3BzLnZhbHVlICE9PSB0aGlzLnByb3BzLnZhbHVlIHx8XG5cdFx0XHRmb3JtYXRzLmRhdGV0aW1lICE9PSB0aGlzLmdldEZvcm1hdHMoIHRoaXMucHJvcHMgKS5kYXRldGltZSApIHtcblx0XHRcdHVwZGF0ZWRTdGF0ZSA9IHRoaXMuZ2V0U3RhdGVGcm9tUHJvcHMoIG5leHRQcm9wcyApO1xuXHRcdH1cblxuXHRcdGlmICggdXBkYXRlZFN0YXRlLm9wZW4gPT09IHVuZGVmaW5lZCApIHtcblx0XHRcdGlmICggdHlwZW9mIG5leHRQcm9wcy5vcGVuICE9PSAndW5kZWZpbmVkJyApIHtcblx0XHRcdFx0dXBkYXRlZFN0YXRlLm9wZW4gPSBuZXh0UHJvcHMub3Blbjtcblx0XHRcdH0gZWxzZSBpZiAoIHRoaXMucHJvcHMuY2xvc2VPblNlbGVjdCAmJiB0aGlzLnN0YXRlLmN1cnJlbnRWaWV3ICE9PSB2aWV3TW9kZXMuVElNRSApIHtcblx0XHRcdFx0dXBkYXRlZFN0YXRlLm9wZW4gPSBmYWxzZTtcblx0XHRcdH0gZWxzZSB7XG5cdFx0XHRcdHVwZGF0ZWRTdGF0ZS5vcGVuID0gdGhpcy5zdGF0ZS5vcGVuO1xuXHRcdFx0fVxuXHRcdH1cblxuXHRcdGlmICggbmV4dFByb3BzLnZpZXdNb2RlICE9PSB0aGlzLnByb3BzLnZpZXdNb2RlICkge1xuXHRcdFx0dXBkYXRlZFN0YXRlLmN1cnJlbnRWaWV3ID0gbmV4dFByb3BzLnZpZXdNb2RlO1xuXHRcdH1cblxuXHRcdGlmICggbmV4dFByb3BzLmxvY2FsZSAhPT0gdGhpcy5wcm9wcy5sb2NhbGUgKSB7XG5cdFx0XHRpZiAoIHRoaXMuc3RhdGUudmlld0RhdGUgKSB7XG5cdFx0XHRcdHZhciB1cGRhdGVkVmlld0RhdGUgPSB0aGlzLnN0YXRlLnZpZXdEYXRlLmNsb25lKCkubG9jYWxlKCBuZXh0UHJvcHMubG9jYWxlICk7XG5cdFx0XHRcdHVwZGF0ZWRTdGF0ZS52aWV3RGF0ZSA9IHVwZGF0ZWRWaWV3RGF0ZTtcblx0XHRcdH1cblx0XHRcdGlmICggdGhpcy5zdGF0ZS5zZWxlY3RlZERhdGUgKSB7XG5cdFx0XHRcdHZhciB1cGRhdGVkU2VsZWN0ZWREYXRlID0gdGhpcy5zdGF0ZS5zZWxlY3RlZERhdGUuY2xvbmUoKS5sb2NhbGUoIG5leHRQcm9wcy5sb2NhbGUgKTtcblx0XHRcdFx0dXBkYXRlZFN0YXRlLnNlbGVjdGVkRGF0ZSA9IHVwZGF0ZWRTZWxlY3RlZERhdGU7XG5cdFx0XHRcdHVwZGF0ZWRTdGF0ZS5pbnB1dFZhbHVlID0gdXBkYXRlZFNlbGVjdGVkRGF0ZS5mb3JtYXQoIGZvcm1hdHMuZGF0ZXRpbWUgKTtcblx0XHRcdH1cblx0XHR9XG5cblx0XHRpZiAoIG5leHRQcm9wcy51dGMgIT09IHRoaXMucHJvcHMudXRjICkge1xuXHRcdFx0aWYgKCBuZXh0UHJvcHMudXRjICkge1xuXHRcdFx0XHRpZiAoIHRoaXMuc3RhdGUudmlld0RhdGUgKVxuXHRcdFx0XHRcdHVwZGF0ZWRTdGF0ZS52aWV3RGF0ZSA9IHRoaXMuc3RhdGUudmlld0RhdGUuY2xvbmUoKS51dGMoKTtcblx0XHRcdFx0aWYgKCB0aGlzLnN0YXRlLnNlbGVjdGVkRGF0ZSApIHtcblx0XHRcdFx0XHR1cGRhdGVkU3RhdGUuc2VsZWN0ZWREYXRlID0gdGhpcy5zdGF0ZS5zZWxlY3RlZERhdGUuY2xvbmUoKS51dGMoKTtcblx0XHRcdFx0XHR1cGRhdGVkU3RhdGUuaW5wdXRWYWx1ZSA9IHVwZGF0ZWRTdGF0ZS5zZWxlY3RlZERhdGUuZm9ybWF0KCBmb3JtYXRzLmRhdGV0aW1lICk7XG5cdFx0XHRcdH1cblx0XHRcdH0gZWxzZSB7XG5cdFx0XHRcdGlmICggdGhpcy5zdGF0ZS52aWV3RGF0ZSApXG5cdFx0XHRcdFx0dXBkYXRlZFN0YXRlLnZpZXdEYXRlID0gdGhpcy5zdGF0ZS52aWV3RGF0ZS5jbG9uZSgpLmxvY2FsKCk7XG5cdFx0XHRcdGlmICggdGhpcy5zdGF0ZS5zZWxlY3RlZERhdGUgKSB7XG5cdFx0XHRcdFx0dXBkYXRlZFN0YXRlLnNlbGVjdGVkRGF0ZSA9IHRoaXMuc3RhdGUuc2VsZWN0ZWREYXRlLmNsb25lKCkubG9jYWwoKTtcblx0XHRcdFx0XHR1cGRhdGVkU3RhdGUuaW5wdXRWYWx1ZSA9IHVwZGF0ZWRTdGF0ZS5zZWxlY3RlZERhdGUuZm9ybWF0KGZvcm1hdHMuZGF0ZXRpbWUpO1xuXHRcdFx0XHR9XG5cdFx0XHR9XG5cdFx0fVxuXG5cdFx0aWYgKCBuZXh0UHJvcHMudmlld0RhdGUgIT09IHRoaXMucHJvcHMudmlld0RhdGUgKSB7XG5cdFx0XHR1cGRhdGVkU3RhdGUudmlld0RhdGUgPSBtb21lbnQobmV4dFByb3BzLnZpZXdEYXRlKTtcblx0XHR9XG5cdFx0Ly93ZSBzaG91bGQgb25seSBzaG93IGEgdmFsaWQgZGF0ZSBpZiB3ZSBhcmUgcHJvdmlkZWQgYSBpc1ZhbGlkRGF0ZSBmdW5jdGlvbi4gUmVtb3ZlZCBpbiAyLjEwLjNcblx0XHQvKmlmICh0aGlzLnByb3BzLmlzVmFsaWREYXRlKSB7XG5cdFx0XHR1cGRhdGVkU3RhdGUudmlld0RhdGUgPSB1cGRhdGVkU3RhdGUudmlld0RhdGUgfHwgdGhpcy5zdGF0ZS52aWV3RGF0ZTtcblx0XHRcdHdoaWxlICghdGhpcy5wcm9wcy5pc1ZhbGlkRGF0ZSh1cGRhdGVkU3RhdGUudmlld0RhdGUpKSB7XG5cdFx0XHRcdHVwZGF0ZWRTdGF0ZS52aWV3RGF0ZSA9IHVwZGF0ZWRTdGF0ZS52aWV3RGF0ZS5hZGQoMSwgJ2RheScpO1xuXHRcdFx0fVxuXHRcdH0qL1xuXHRcdHRoaXMuc2V0U3RhdGUoIHVwZGF0ZWRTdGF0ZSApO1xuXHR9LFxuXG5cdG9uSW5wdXRDaGFuZ2U6IGZ1bmN0aW9uKCBlICkge1xuXHRcdHZhciB2YWx1ZSA9IGUudGFyZ2V0ID09PSBudWxsID8gZSA6IGUudGFyZ2V0LnZhbHVlLFxuXHRcdFx0bG9jYWxNb21lbnQgPSB0aGlzLmxvY2FsTW9tZW50KCB2YWx1ZSwgdGhpcy5zdGF0ZS5pbnB1dEZvcm1hdCApLFxuXHRcdFx0dXBkYXRlID0geyBpbnB1dFZhbHVlOiB2YWx1ZSB9XG5cdFx0XHQ7XG5cblx0XHRpZiAoIGxvY2FsTW9tZW50LmlzVmFsaWQoKSAmJiAhdGhpcy5wcm9wcy52YWx1ZSApIHtcblx0XHRcdHVwZGF0ZS5zZWxlY3RlZERhdGUgPSBsb2NhbE1vbWVudDtcblx0XHRcdHVwZGF0ZS52aWV3RGF0ZSA9IGxvY2FsTW9tZW50LmNsb25lKCkuc3RhcnRPZignbW9udGgnKTtcblx0XHR9IGVsc2Uge1xuXHRcdFx0dXBkYXRlLnNlbGVjdGVkRGF0ZSA9IG51bGw7XG5cdFx0fVxuXG5cdFx0cmV0dXJuIHRoaXMuc2V0U3RhdGUoIHVwZGF0ZSwgZnVuY3Rpb24oKSB7XG5cdFx0XHRyZXR1cm4gdGhpcy5wcm9wcy5vbkNoYW5nZSggbG9jYWxNb21lbnQuaXNWYWxpZCgpID8gbG9jYWxNb21lbnQgOiB0aGlzLnN0YXRlLmlucHV0VmFsdWUgKTtcblx0XHR9KTtcblx0fSxcblxuXHRvbklucHV0S2V5OiBmdW5jdGlvbiggZSApIHtcblx0XHRpZiAoIGUud2hpY2ggPT09IDkgJiYgdGhpcy5wcm9wcy5jbG9zZU9uVGFiICkge1xuXHRcdFx0dGhpcy5jbG9zZUNhbGVuZGFyKCk7XG5cdFx0fVxuXHR9LFxuXG5cdHNob3dWaWV3OiBmdW5jdGlvbiggdmlldyApIHtcblx0XHR2YXIgbWUgPSB0aGlzO1xuXHRcdHJldHVybiBmdW5jdGlvbigpIHtcblx0XHRcdG1lLnN0YXRlLmN1cnJlbnRWaWV3ICE9PSB2aWV3ICYmIG1lLnByb3BzLm9uVmlld01vZGVDaGFuZ2UoIHZpZXcgKTtcblx0XHRcdG1lLnNldFN0YXRlKHsgY3VycmVudFZpZXc6IHZpZXcgfSk7XG5cdFx0fTtcblx0fSxcblxuXHRzZXREYXRlOiBmdW5jdGlvbiggdHlwZSApIHtcblx0XHR2YXIgbWUgPSB0aGlzLFxuXHRcdFx0bmV4dFZpZXdzID0ge1xuXHRcdFx0XHRtb250aDogdmlld01vZGVzLkRBWVMsXG5cdFx0XHRcdHllYXI6IHZpZXdNb2Rlcy5NT05USFMsXG5cdFx0XHR9XG5cdFx0O1xuXHRcdHJldHVybiBmdW5jdGlvbiggZSApIHtcblx0XHRcdG1lLnNldFN0YXRlKHtcblx0XHRcdFx0dmlld0RhdGU6IG1lLnN0YXRlLnZpZXdEYXRlLmNsb25lKClbIHR5cGUgXSggcGFyc2VJbnQoZS50YXJnZXQuZ2V0QXR0cmlidXRlKCdkYXRhLXZhbHVlJyksIDEwKSApLnN0YXJ0T2YoIHR5cGUgKSxcblx0XHRcdFx0Y3VycmVudFZpZXc6IG5leHRWaWV3c1sgdHlwZSBdXG5cdFx0XHR9KTtcblx0XHRcdG1lLnByb3BzLm9uVmlld01vZGVDaGFuZ2UoIG5leHRWaWV3c1sgdHlwZSBdICk7XG5cdFx0fTtcblx0fSxcblxuXHRhZGRUaW1lOiBmdW5jdGlvbiggYW1vdW50LCB0eXBlLCB0b1NlbGVjdGVkICkge1xuXHRcdHJldHVybiB0aGlzLnVwZGF0ZVRpbWUoICdhZGQnLCBhbW91bnQsIHR5cGUsIHRvU2VsZWN0ZWQgKTtcblx0fSxcblxuXHRzdWJ0cmFjdFRpbWU6IGZ1bmN0aW9uKCBhbW91bnQsIHR5cGUsIHRvU2VsZWN0ZWQgKSB7XG5cdFx0cmV0dXJuIHRoaXMudXBkYXRlVGltZSggJ3N1YnRyYWN0JywgYW1vdW50LCB0eXBlLCB0b1NlbGVjdGVkICk7XG5cdH0sXG5cblx0dXBkYXRlVGltZTogZnVuY3Rpb24oIG9wLCBhbW91bnQsIHR5cGUsIHRvU2VsZWN0ZWQgKSB7XG5cdFx0dmFyIG1lID0gdGhpcztcblxuXHRcdHJldHVybiBmdW5jdGlvbigpIHtcblx0XHRcdHZhciB1cGRhdGUgPSB7fSxcblx0XHRcdFx0ZGF0ZSA9IHRvU2VsZWN0ZWQgPyAnc2VsZWN0ZWREYXRlJyA6ICd2aWV3RGF0ZSdcblx0XHRcdDtcblxuXHRcdFx0dXBkYXRlWyBkYXRlIF0gPSBtZS5zdGF0ZVsgZGF0ZSBdLmNsb25lKClbIG9wIF0oIGFtb3VudCwgdHlwZSApO1xuXG5cdFx0XHRtZS5zZXRTdGF0ZSggdXBkYXRlICk7XG5cdFx0fTtcblx0fSxcblxuXHRhbGxvd2VkU2V0VGltZTogWydob3VycycsICdtaW51dGVzJywgJ3NlY29uZHMnLCAnbWlsbGlzZWNvbmRzJ10sXG5cdHNldFRpbWU6IGZ1bmN0aW9uKCB0eXBlLCB2YWx1ZSApIHtcblx0XHR2YXIgaW5kZXggPSB0aGlzLmFsbG93ZWRTZXRUaW1lLmluZGV4T2YoIHR5cGUgKSArIDEsXG5cdFx0XHRzdGF0ZSA9IHRoaXMuc3RhdGUsXG5cdFx0XHRkYXRlID0gKHN0YXRlLnNlbGVjdGVkRGF0ZSB8fCBzdGF0ZS52aWV3RGF0ZSkuY2xvbmUoKSxcblx0XHRcdG5leHRUeXBlXG5cdFx0XHQ7XG5cblx0XHQvLyBJdCBpcyBuZWVkZWQgdG8gc2V0IGFsbCB0aGUgdGltZSBwcm9wZXJ0aWVzXG5cdFx0Ly8gdG8gbm90IHRvIHJlc2V0IHRoZSB0aW1lXG5cdFx0ZGF0ZVsgdHlwZSBdKCB2YWx1ZSApO1xuXHRcdGZvciAoOyBpbmRleCA8IHRoaXMuYWxsb3dlZFNldFRpbWUubGVuZ3RoOyBpbmRleCsrKSB7XG5cdFx0XHRuZXh0VHlwZSA9IHRoaXMuYWxsb3dlZFNldFRpbWVbaW5kZXhdO1xuXHRcdFx0ZGF0ZVsgbmV4dFR5cGUgXSggZGF0ZVtuZXh0VHlwZV0oKSApO1xuXHRcdH1cblxuXHRcdGlmICggIXRoaXMucHJvcHMudmFsdWUgKSB7XG5cdFx0XHR0aGlzLnNldFN0YXRlKHtcblx0XHRcdFx0c2VsZWN0ZWREYXRlOiBkYXRlLFxuXHRcdFx0XHRpbnB1dFZhbHVlOiBkYXRlLmZvcm1hdCggc3RhdGUuaW5wdXRGb3JtYXQgKVxuXHRcdFx0fSk7XG5cdFx0fVxuXHRcdHRoaXMucHJvcHMub25DaGFuZ2UoIGRhdGUgKTtcblx0fSxcblxuXHR1cGRhdGVTZWxlY3RlZERhdGU6IGZ1bmN0aW9uKCBlLCBjbG9zZSApIHtcblx0XHR2YXIgdGFyZ2V0ID0gZS50YXJnZXQsXG5cdFx0XHRtb2RpZmllciA9IDAsXG5cdFx0XHR2aWV3RGF0ZSA9IHRoaXMuc3RhdGUudmlld0RhdGUsXG5cdFx0XHRjdXJyZW50RGF0ZSA9IHRoaXMuc3RhdGUuc2VsZWN0ZWREYXRlIHx8IHZpZXdEYXRlLFxuXHRcdFx0ZGF0ZVxuXHRcdFx0O1xuXG5cdFx0aWYgKHRhcmdldC5jbGFzc05hbWUuaW5kZXhPZigncmR0RGF5JykgIT09IC0xKSB7XG5cdFx0XHRpZiAodGFyZ2V0LmNsYXNzTmFtZS5pbmRleE9mKCdyZHROZXcnKSAhPT0gLTEpXG5cdFx0XHRcdG1vZGlmaWVyID0gMTtcblx0XHRcdGVsc2UgaWYgKHRhcmdldC5jbGFzc05hbWUuaW5kZXhPZigncmR0T2xkJykgIT09IC0xKVxuXHRcdFx0XHRtb2RpZmllciA9IC0xO1xuXG5cdFx0XHRkYXRlID0gdmlld0RhdGUuY2xvbmUoKVxuXHRcdFx0XHQubW9udGgoIHZpZXdEYXRlLm1vbnRoKCkgKyBtb2RpZmllciApXG5cdFx0XHRcdC5kYXRlKCBwYXJzZUludCggdGFyZ2V0LmdldEF0dHJpYnV0ZSgnZGF0YS12YWx1ZScpLCAxMCApICk7XG5cdFx0fSBlbHNlIGlmICh0YXJnZXQuY2xhc3NOYW1lLmluZGV4T2YoJ3JkdE1vbnRoJykgIT09IC0xKSB7XG5cdFx0XHRkYXRlID0gdmlld0RhdGUuY2xvbmUoKVxuXHRcdFx0XHQubW9udGgoIHBhcnNlSW50KCB0YXJnZXQuZ2V0QXR0cmlidXRlKCdkYXRhLXZhbHVlJyksIDEwICkgKVxuXHRcdFx0XHQuZGF0ZSggY3VycmVudERhdGUuZGF0ZSgpICk7XG5cdFx0fSBlbHNlIGlmICh0YXJnZXQuY2xhc3NOYW1lLmluZGV4T2YoJ3JkdFllYXInKSAhPT0gLTEpIHtcblx0XHRcdGRhdGUgPSB2aWV3RGF0ZS5jbG9uZSgpXG5cdFx0XHRcdC5tb250aCggY3VycmVudERhdGUubW9udGgoKSApXG5cdFx0XHRcdC5kYXRlKCBjdXJyZW50RGF0ZS5kYXRlKCkgKVxuXHRcdFx0XHQueWVhciggcGFyc2VJbnQoIHRhcmdldC5nZXRBdHRyaWJ1dGUoJ2RhdGEtdmFsdWUnKSwgMTAgKSApO1xuXHRcdH1cblxuXHRcdGRhdGUuaG91cnMoIGN1cnJlbnREYXRlLmhvdXJzKCkgKVxuXHRcdFx0Lm1pbnV0ZXMoIGN1cnJlbnREYXRlLm1pbnV0ZXMoKSApXG5cdFx0XHQuc2Vjb25kcyggY3VycmVudERhdGUuc2Vjb25kcygpIClcblx0XHRcdC5taWxsaXNlY29uZHMoIGN1cnJlbnREYXRlLm1pbGxpc2Vjb25kcygpICk7XG5cblx0XHRpZiAoICF0aGlzLnByb3BzLnZhbHVlICkge1xuXHRcdFx0dmFyIG9wZW4gPSAhKCB0aGlzLnByb3BzLmNsb3NlT25TZWxlY3QgJiYgY2xvc2UgKTtcblx0XHRcdGlmICggIW9wZW4gKSB7XG5cdFx0XHRcdHRoaXMucHJvcHMub25CbHVyKCBkYXRlICk7XG5cdFx0XHR9XG5cblx0XHRcdHRoaXMuc2V0U3RhdGUoe1xuXHRcdFx0XHRzZWxlY3RlZERhdGU6IGRhdGUsXG5cdFx0XHRcdHZpZXdEYXRlOiBkYXRlLmNsb25lKCkuc3RhcnRPZignbW9udGgnKSxcblx0XHRcdFx0aW5wdXRWYWx1ZTogZGF0ZS5mb3JtYXQoIHRoaXMuc3RhdGUuaW5wdXRGb3JtYXQgKSxcblx0XHRcdFx0b3Blbjogb3BlblxuXHRcdFx0fSk7XG5cdFx0fSBlbHNlIHtcblx0XHRcdGlmICggdGhpcy5wcm9wcy5jbG9zZU9uU2VsZWN0ICYmIGNsb3NlICkge1xuXHRcdFx0XHR0aGlzLmNsb3NlQ2FsZW5kYXIoKTtcblx0XHRcdH1cblx0XHR9XG5cblx0XHR0aGlzLnByb3BzLm9uQ2hhbmdlKCBkYXRlICk7XG5cdH0sXG5cblx0b3BlbkNhbGVuZGFyOiBmdW5jdGlvbiggZSApIHtcblx0XHRpZiAoICF0aGlzLnN0YXRlLm9wZW4gKSB7XG5cdFx0XHR0aGlzLnNldFN0YXRlKHsgb3BlbjogdHJ1ZSB9LCBmdW5jdGlvbigpIHtcblx0XHRcdFx0dGhpcy5wcm9wcy5vbkZvY3VzKCBlICk7XG5cdFx0XHR9KTtcblx0XHR9XG5cdH0sXG5cblx0Y2xvc2VDYWxlbmRhcjogZnVuY3Rpb24oKSB7XG5cdFx0dGhpcy5zZXRTdGF0ZSh7IG9wZW46IGZhbHNlIH0sIGZ1bmN0aW9uICgpIHtcblx0XHRcdHRoaXMucHJvcHMub25CbHVyKCB0aGlzLnN0YXRlLnNlbGVjdGVkRGF0ZSB8fCB0aGlzLnN0YXRlLmlucHV0VmFsdWUgKTtcblx0XHR9KTtcblx0fSxcblxuXHRoYW5kbGVDbGlja091dHNpZGU6IGZ1bmN0aW9uKCkge1xuXHRcdGlmICggdGhpcy5wcm9wcy5pbnB1dCAmJiB0aGlzLnN0YXRlLm9wZW4gJiYgIXRoaXMucHJvcHMub3BlbiAmJiAhdGhpcy5wcm9wcy5kaXNhYmxlT25DbGlja091dHNpZGUgKSB7XG5cdFx0XHR0aGlzLnNldFN0YXRlKHsgb3BlbjogZmFsc2UgfSwgZnVuY3Rpb24oKSB7XG5cdFx0XHRcdHRoaXMucHJvcHMub25CbHVyKCB0aGlzLnN0YXRlLnNlbGVjdGVkRGF0ZSB8fCB0aGlzLnN0YXRlLmlucHV0VmFsdWUgKTtcblx0XHRcdH0pO1xuXHRcdH1cblx0fSxcblxuXHRsb2NhbE1vbWVudDogZnVuY3Rpb24oIGRhdGUsIGZvcm1hdCwgcHJvcHMgKSB7XG5cdFx0cHJvcHMgPSBwcm9wcyB8fCB0aGlzLnByb3BzO1xuXHRcdHZhciBtb21lbnRGbiA9IHByb3BzLnV0YyA/IG1vbWVudC51dGMgOiBtb21lbnQ7XG5cdFx0dmFyIG0gPSBtb21lbnRGbiggZGF0ZSwgZm9ybWF0LCBwcm9wcy5zdHJpY3RQYXJzaW5nICk7XG5cdFx0aWYgKCBwcm9wcy5sb2NhbGUgKVxuXHRcdFx0bS5sb2NhbGUoIHByb3BzLmxvY2FsZSApO1xuXHRcdHJldHVybiBtO1xuXHR9LFxuXG5cdGNvbXBvbmVudFByb3BzOiB7XG5cdFx0ZnJvbVByb3BzOiBbJ3ZhbHVlJywgJ2lzVmFsaWREYXRlJywgJ3JlbmRlckRheScsICdyZW5kZXJNb250aCcsICdyZW5kZXJZZWFyJywgJ3RpbWVDb25zdHJhaW50cyddLFxuXHRcdGZyb21TdGF0ZTogWyd2aWV3RGF0ZScsICdzZWxlY3RlZERhdGUnLCAndXBkYXRlT24nXSxcblx0XHRmcm9tVGhpczogWydzZXREYXRlJywgJ3NldFRpbWUnLCAnc2hvd1ZpZXcnLCAnYWRkVGltZScsICdzdWJ0cmFjdFRpbWUnLCAndXBkYXRlU2VsZWN0ZWREYXRlJywgJ2xvY2FsTW9tZW50JywgJ2hhbmRsZUNsaWNrT3V0c2lkZSddXG5cdH0sXG5cblx0Z2V0Q29tcG9uZW50UHJvcHM6IGZ1bmN0aW9uKCkge1xuXHRcdHZhciBtZSA9IHRoaXMsXG5cdFx0XHRmb3JtYXRzID0gdGhpcy5nZXRGb3JtYXRzKCB0aGlzLnByb3BzICksXG5cdFx0XHRwcm9wcyA9IHtkYXRlRm9ybWF0OiBmb3JtYXRzLmRhdGUsIHRpbWVGb3JtYXQ6IGZvcm1hdHMudGltZX1cblx0XHRcdDtcblxuXHRcdHRoaXMuY29tcG9uZW50UHJvcHMuZnJvbVByb3BzLmZvckVhY2goIGZ1bmN0aW9uKCBuYW1lICkge1xuXHRcdFx0cHJvcHNbIG5hbWUgXSA9IG1lLnByb3BzWyBuYW1lIF07XG5cdFx0fSk7XG5cdFx0dGhpcy5jb21wb25lbnRQcm9wcy5mcm9tU3RhdGUuZm9yRWFjaCggZnVuY3Rpb24oIG5hbWUgKSB7XG5cdFx0XHRwcm9wc1sgbmFtZSBdID0gbWUuc3RhdGVbIG5hbWUgXTtcblx0XHR9KTtcblx0XHR0aGlzLmNvbXBvbmVudFByb3BzLmZyb21UaGlzLmZvckVhY2goIGZ1bmN0aW9uKCBuYW1lICkge1xuXHRcdFx0cHJvcHNbIG5hbWUgXSA9IG1lWyBuYW1lIF07XG5cdFx0fSk7XG5cblx0XHRyZXR1cm4gcHJvcHM7XG5cdH0sXG5cblx0cmVuZGVyOiBmdW5jdGlvbigpIHtcblx0XHQvLyBUT0RPOiBNYWtlIGEgZnVuY3Rpb24gb3IgY2xlYW4gdXAgdGhpcyBjb2RlLFxuXHRcdC8vIGxvZ2ljIHJpZ2h0IG5vdyBpcyByZWFsbHkgaGFyZCB0byBmb2xsb3dcblx0XHR2YXIgY2xhc3NOYW1lID0gJ3JkdCcgKyAodGhpcy5wcm9wcy5jbGFzc05hbWUgP1xuICAgICAgICAgICAgICAgICAgKCBBcnJheS5pc0FycmF5KCB0aGlzLnByb3BzLmNsYXNzTmFtZSApID9cbiAgICAgICAgICAgICAgICAgICcgJyArIHRoaXMucHJvcHMuY2xhc3NOYW1lLmpvaW4oICcgJyApIDogJyAnICsgdGhpcy5wcm9wcy5jbGFzc05hbWUpIDogJycpLFxuXHRcdFx0Y2hpbGRyZW4gPSBbXTtcblxuXHRcdGlmICggdGhpcy5wcm9wcy5pbnB1dCApIHtcblx0XHRcdHZhciBmaW5hbElucHV0UHJvcHMgPSBhc3NpZ24oe1xuXHRcdFx0XHR0eXBlOiAndGV4dCcsXG5cdFx0XHRcdGNsYXNzTmFtZTogJ2Zvcm0tY29udHJvbCcsXG5cdFx0XHRcdG9uQ2xpY2s6IHRoaXMub3BlbkNhbGVuZGFyLFxuXHRcdFx0XHRvbkZvY3VzOiB0aGlzLm9wZW5DYWxlbmRhcixcblx0XHRcdFx0b25DaGFuZ2U6IHRoaXMub25JbnB1dENoYW5nZSxcblx0XHRcdFx0b25LZXlEb3duOiB0aGlzLm9uSW5wdXRLZXksXG5cdFx0XHRcdHZhbHVlOiB0aGlzLnN0YXRlLmlucHV0VmFsdWUsXG5cdFx0XHR9LCB0aGlzLnByb3BzLmlucHV0UHJvcHMpO1xuXHRcdFx0aWYgKCB0aGlzLnByb3BzLnJlbmRlcklucHV0ICkge1xuXHRcdFx0XHRjaGlsZHJlbiA9IFsgUmVhY3QuY3JlYXRlRWxlbWVudCgnZGl2JywgeyBrZXk6ICdpJyB9LCB0aGlzLnByb3BzLnJlbmRlcklucHV0KCBmaW5hbElucHV0UHJvcHMsIHRoaXMub3BlbkNhbGVuZGFyLCB0aGlzLmNsb3NlQ2FsZW5kYXIgKSkgXTtcblx0XHRcdH0gZWxzZSB7XG5cdFx0XHRcdGNoaWxkcmVuID0gWyBSZWFjdC5jcmVhdGVFbGVtZW50KCdpbnB1dCcsIGFzc2lnbih7IGtleTogJ2knIH0sIGZpbmFsSW5wdXRQcm9wcyApKV07XG5cdFx0XHR9XG5cdFx0fSBlbHNlIHtcblx0XHRcdGNsYXNzTmFtZSArPSAnIHJkdFN0YXRpYyc7XG5cdFx0fVxuXG5cdFx0aWYgKCB0aGlzLnN0YXRlLm9wZW4gKVxuXHRcdFx0Y2xhc3NOYW1lICs9ICcgcmR0T3Blbic7XG5cblx0XHRyZXR1cm4gUmVhY3QuY3JlYXRlRWxlbWVudCggJ2RpdicsIHsgY2xhc3NOYW1lOiBjbGFzc05hbWUgfSwgY2hpbGRyZW4uY29uY2F0KFxuXHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCggJ2RpdicsXG5cdFx0XHRcdHsga2V5OiAnZHQnLCBjbGFzc05hbWU6ICdyZHRQaWNrZXInIH0sXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoIENhbGVuZGFyQ29udGFpbmVyLCB7IHZpZXc6IHRoaXMuc3RhdGUuY3VycmVudFZpZXcsIHZpZXdQcm9wczogdGhpcy5nZXRDb21wb25lbnRQcm9wcygpLCBvbkNsaWNrT3V0c2lkZTogdGhpcy5oYW5kbGVDbGlja091dHNpZGUgfSlcblx0XHRcdClcblx0XHQpKTtcblx0fVxufSk7XG5cbkRhdGV0aW1lLmRlZmF1bHRQcm9wcyA9IHtcblx0Y2xhc3NOYW1lOiAnJyxcblx0ZGVmYXVsdFZhbHVlOiAnJyxcblx0aW5wdXRQcm9wczoge30sXG5cdGlucHV0OiB0cnVlLFxuXHRvbkZvY3VzOiBmdW5jdGlvbigpIHt9LFxuXHRvbkJsdXI6IGZ1bmN0aW9uKCkge30sXG5cdG9uQ2hhbmdlOiBmdW5jdGlvbigpIHt9LFxuXHRvblZpZXdNb2RlQ2hhbmdlOiBmdW5jdGlvbigpIHt9LFxuXHR0aW1lRm9ybWF0OiB0cnVlLFxuXHR0aW1lQ29uc3RyYWludHM6IHt9LFxuXHRkYXRlRm9ybWF0OiB0cnVlLFxuXHRzdHJpY3RQYXJzaW5nOiB0cnVlLFxuXHRjbG9zZU9uU2VsZWN0OiBmYWxzZSxcblx0Y2xvc2VPblRhYjogdHJ1ZSxcblx0dXRjOiBmYWxzZVxufTtcblxuLy8gTWFrZSBtb21lbnQgYWNjZXNzaWJsZSB0aHJvdWdoIHRoZSBEYXRldGltZSBjbGFzc1xuRGF0ZXRpbWUubW9tZW50ID0gbW9tZW50O1xuXG5tb2R1bGUuZXhwb3J0cyA9IERhdGV0aW1lO1xuIiwiXG52YXIgY29udGVudCA9IHJlcXVpcmUoXCIhIS4uLy4uL2Nzcy1sb2FkZXIvZGlzdC9janMuanMhLi9yZWFjdC1kYXRldGltZS5jc3NcIik7XG5cbmlmKHR5cGVvZiBjb250ZW50ID09PSAnc3RyaW5nJykgY29udGVudCA9IFtbbW9kdWxlLmlkLCBjb250ZW50LCAnJ11dO1xuXG52YXIgdHJhbnNmb3JtO1xudmFyIGluc2VydEludG87XG5cblxuXG52YXIgb3B0aW9ucyA9IHtcImhtclwiOnRydWV9XG5cbm9wdGlvbnMudHJhbnNmb3JtID0gdHJhbnNmb3JtXG5vcHRpb25zLmluc2VydEludG8gPSB1bmRlZmluZWQ7XG5cbnZhciB1cGRhdGUgPSByZXF1aXJlKFwiIS4uLy4uL3N0eWxlLWxvYWRlci9saWIvYWRkU3R5bGVzLmpzXCIpKGNvbnRlbnQsIG9wdGlvbnMpO1xuXG5pZihjb250ZW50LmxvY2FscykgbW9kdWxlLmV4cG9ydHMgPSBjb250ZW50LmxvY2FscztcblxuaWYobW9kdWxlLmhvdCkge1xuXHRtb2R1bGUuaG90LmFjY2VwdChcIiEhLi4vLi4vY3NzLWxvYWRlci9kaXN0L2Nqcy5qcyEuL3JlYWN0LWRhdGV0aW1lLmNzc1wiLCBmdW5jdGlvbigpIHtcblx0XHR2YXIgbmV3Q29udGVudCA9IHJlcXVpcmUoXCIhIS4uLy4uL2Nzcy1sb2FkZXIvZGlzdC9janMuanMhLi9yZWFjdC1kYXRldGltZS5jc3NcIik7XG5cblx0XHRpZih0eXBlb2YgbmV3Q29udGVudCA9PT0gJ3N0cmluZycpIG5ld0NvbnRlbnQgPSBbW21vZHVsZS5pZCwgbmV3Q29udGVudCwgJyddXTtcblxuXHRcdHZhciBsb2NhbHMgPSAoZnVuY3Rpb24oYSwgYikge1xuXHRcdFx0dmFyIGtleSwgaWR4ID0gMDtcblxuXHRcdFx0Zm9yKGtleSBpbiBhKSB7XG5cdFx0XHRcdGlmKCFiIHx8IGFba2V5XSAhPT0gYltrZXldKSByZXR1cm4gZmFsc2U7XG5cdFx0XHRcdGlkeCsrO1xuXHRcdFx0fVxuXG5cdFx0XHRmb3Ioa2V5IGluIGIpIGlkeC0tO1xuXG5cdFx0XHRyZXR1cm4gaWR4ID09PSAwO1xuXHRcdH0oY29udGVudC5sb2NhbHMsIG5ld0NvbnRlbnQubG9jYWxzKSk7XG5cblx0XHRpZighbG9jYWxzKSB0aHJvdyBuZXcgRXJyb3IoJ0Fib3J0aW5nIENTUyBITVIgZHVlIHRvIGNoYW5nZWQgY3NzLW1vZHVsZXMgbG9jYWxzLicpO1xuXG5cdFx0dXBkYXRlKG5ld0NvbnRlbnQpO1xuXHR9KTtcblxuXHRtb2R1bGUuaG90LmRpc3Bvc2UoZnVuY3Rpb24oKSB7IHVwZGF0ZSgpOyB9KTtcbn0iLCIndXNlIHN0cmljdCc7XG52YXIgcHJvcElzRW51bWVyYWJsZSA9IE9iamVjdC5wcm90b3R5cGUucHJvcGVydHlJc0VudW1lcmFibGU7XG5cbmZ1bmN0aW9uIFRvT2JqZWN0KHZhbCkge1xuXHRpZiAodmFsID09IG51bGwpIHtcblx0XHR0aHJvdyBuZXcgVHlwZUVycm9yKCdPYmplY3QuYXNzaWduIGNhbm5vdCBiZSBjYWxsZWQgd2l0aCBudWxsIG9yIHVuZGVmaW5lZCcpO1xuXHR9XG5cblx0cmV0dXJuIE9iamVjdCh2YWwpO1xufVxuXG5mdW5jdGlvbiBvd25FbnVtZXJhYmxlS2V5cyhvYmopIHtcblx0dmFyIGtleXMgPSBPYmplY3QuZ2V0T3duUHJvcGVydHlOYW1lcyhvYmopO1xuXG5cdGlmIChPYmplY3QuZ2V0T3duUHJvcGVydHlTeW1ib2xzKSB7XG5cdFx0a2V5cyA9IGtleXMuY29uY2F0KE9iamVjdC5nZXRPd25Qcm9wZXJ0eVN5bWJvbHMob2JqKSk7XG5cdH1cblxuXHRyZXR1cm4ga2V5cy5maWx0ZXIoZnVuY3Rpb24gKGtleSkge1xuXHRcdHJldHVybiBwcm9wSXNFbnVtZXJhYmxlLmNhbGwob2JqLCBrZXkpO1xuXHR9KTtcbn1cblxubW9kdWxlLmV4cG9ydHMgPSBPYmplY3QuYXNzaWduIHx8IGZ1bmN0aW9uICh0YXJnZXQsIHNvdXJjZSkge1xuXHR2YXIgZnJvbTtcblx0dmFyIGtleXM7XG5cdHZhciB0byA9IFRvT2JqZWN0KHRhcmdldCk7XG5cblx0Zm9yICh2YXIgcyA9IDE7IHMgPCBhcmd1bWVudHMubGVuZ3RoOyBzKyspIHtcblx0XHRmcm9tID0gYXJndW1lbnRzW3NdO1xuXHRcdGtleXMgPSBvd25FbnVtZXJhYmxlS2V5cyhPYmplY3QoZnJvbSkpO1xuXG5cdFx0Zm9yICh2YXIgaSA9IDA7IGkgPCBrZXlzLmxlbmd0aDsgaSsrKSB7XG5cdFx0XHR0b1trZXlzW2ldXSA9IGZyb21ba2V5c1tpXV07XG5cdFx0fVxuXHR9XG5cblx0cmV0dXJuIHRvO1xufTtcbiIsIid1c2Ugc3RyaWN0JztcblxudmFyIFJlYWN0ID0gcmVxdWlyZSgncmVhY3QnKSxcblx0Y3JlYXRlQ2xhc3MgPSByZXF1aXJlKCdjcmVhdGUtcmVhY3QtY2xhc3MnKSxcblx0RGF5c1ZpZXcgPSByZXF1aXJlKCcuL0RheXNWaWV3JyksXG5cdE1vbnRoc1ZpZXcgPSByZXF1aXJlKCcuL01vbnRoc1ZpZXcnKSxcblx0WWVhcnNWaWV3ID0gcmVxdWlyZSgnLi9ZZWFyc1ZpZXcnKSxcblx0VGltZVZpZXcgPSByZXF1aXJlKCcuL1RpbWVWaWV3Jylcblx0O1xuXG52YXIgQ2FsZW5kYXJDb250YWluZXIgPSBjcmVhdGVDbGFzcyh7XG5cdHZpZXdDb21wb25lbnRzOiB7XG5cdFx0ZGF5czogRGF5c1ZpZXcsXG5cdFx0bW9udGhzOiBNb250aHNWaWV3LFxuXHRcdHllYXJzOiBZZWFyc1ZpZXcsXG5cdFx0dGltZTogVGltZVZpZXdcblx0fSxcblxuXHRyZW5kZXI6IGZ1bmN0aW9uKCkge1xuXHRcdHJldHVybiBSZWFjdC5jcmVhdGVFbGVtZW50KCB0aGlzLnZpZXdDb21wb25lbnRzWyB0aGlzLnByb3BzLnZpZXcgXSwgdGhpcy5wcm9wcy52aWV3UHJvcHMgKTtcblx0fVxufSk7XG5cbm1vZHVsZS5leHBvcnRzID0gQ2FsZW5kYXJDb250YWluZXI7XG4iLCIndXNlIHN0cmljdCc7XG5cbnZhciBSZWFjdCA9IHJlcXVpcmUoJ3JlYWN0JyksXG5cdGNyZWF0ZUNsYXNzID0gcmVxdWlyZSgnY3JlYXRlLXJlYWN0LWNsYXNzJyksXG5cdG1vbWVudCA9IHJlcXVpcmUoJ21vbWVudCcpLFxuXHRvbkNsaWNrT3V0c2lkZSA9IHJlcXVpcmUoJ3JlYWN0LW9uY2xpY2tvdXRzaWRlJykuZGVmYXVsdFxuXHQ7XG5cbnZhciBEYXRlVGltZVBpY2tlckRheXMgPSBvbkNsaWNrT3V0c2lkZSggY3JlYXRlQ2xhc3Moe1xuXHRyZW5kZXI6IGZ1bmN0aW9uKCkge1xuXHRcdHZhciBmb290ZXIgPSB0aGlzLnJlbmRlckZvb3RlcigpLFxuXHRcdFx0ZGF0ZSA9IHRoaXMucHJvcHMudmlld0RhdGUsXG5cdFx0XHRsb2NhbGUgPSBkYXRlLmxvY2FsZURhdGEoKSxcblx0XHRcdHRhYmxlQ2hpbGRyZW5cblx0XHRcdDtcblxuXHRcdHRhYmxlQ2hpbGRyZW4gPSBbXG5cdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0aGVhZCcsIHsga2V5OiAndGgnIH0sIFtcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndHInLCB7IGtleTogJ2gnIH0sIFtcblx0XHRcdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0aCcsIHsga2V5OiAncCcsIGNsYXNzTmFtZTogJ3JkdFByZXYnLCBvbkNsaWNrOiB0aGlzLnByb3BzLnN1YnRyYWN0VGltZSggMSwgJ21vbnRocycgKX0sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3NwYW4nLCB7fSwgJ+KAuScgKSksXG5cdFx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGgnLCB7IGtleTogJ3MnLCBjbGFzc05hbWU6ICdyZHRTd2l0Y2gnLCBvbkNsaWNrOiB0aGlzLnByb3BzLnNob3dWaWV3KCAnbW9udGhzJyApLCBjb2xTcGFuOiA1LCAnZGF0YS12YWx1ZSc6IHRoaXMucHJvcHMudmlld0RhdGUubW9udGgoKSB9LCBsb2NhbGUubW9udGhzKCBkYXRlICkgKyAnICcgKyBkYXRlLnllYXIoKSApLFxuXHRcdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoJywgeyBrZXk6ICduJywgY2xhc3NOYW1lOiAncmR0TmV4dCcsIG9uQ2xpY2s6IHRoaXMucHJvcHMuYWRkVGltZSggMSwgJ21vbnRocycgKX0sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3NwYW4nLCB7fSwgJ+KAuicgKSlcblx0XHRcdFx0XSksXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RyJywgeyBrZXk6ICdkJ30sIHRoaXMuZ2V0RGF5c09mV2VlayggbG9jYWxlICkubWFwKCBmdW5jdGlvbiggZGF5LCBpbmRleCApIHsgcmV0dXJuIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoJywgeyBrZXk6IGRheSArIGluZGV4LCBjbGFzc05hbWU6ICdkb3cnfSwgZGF5ICk7IH0pIClcblx0XHRcdF0pLFxuXHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGJvZHknLCB7IGtleTogJ3RiJyB9LCB0aGlzLnJlbmRlckRheXMoKSlcblx0XHRdO1xuXG5cdFx0aWYgKCBmb290ZXIgKVxuXHRcdFx0dGFibGVDaGlsZHJlbi5wdXNoKCBmb290ZXIgKTtcblxuXHRcdHJldHVybiBSZWFjdC5jcmVhdGVFbGVtZW50KCdkaXYnLCB7IGNsYXNzTmFtZTogJ3JkdERheXMnIH0sXG5cdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0YWJsZScsIHt9LCB0YWJsZUNoaWxkcmVuIClcblx0XHQpO1xuXHR9LFxuXG5cdC8qKlxuXHQgKiBHZXQgYSBsaXN0IG9mIHRoZSBkYXlzIG9mIHRoZSB3ZWVrXG5cdCAqIGRlcGVuZGluZyBvbiB0aGUgY3VycmVudCBsb2NhbGVcblx0ICogQHJldHVybiB7YXJyYXl9IEEgbGlzdCB3aXRoIHRoZSBzaG9ydG5hbWUgb2YgdGhlIGRheXNcblx0ICovXG5cdGdldERheXNPZldlZWs6IGZ1bmN0aW9uKCBsb2NhbGUgKSB7XG5cdFx0dmFyIGRheXMgPSBsb2NhbGUuX3dlZWtkYXlzTWluLFxuXHRcdFx0Zmlyc3QgPSBsb2NhbGUuZmlyc3REYXlPZldlZWsoKSxcblx0XHRcdGRvdyA9IFtdLFxuXHRcdFx0aSA9IDBcblx0XHRcdDtcblxuXHRcdGRheXMuZm9yRWFjaCggZnVuY3Rpb24oIGRheSApIHtcblx0XHRcdGRvd1sgKDcgKyAoIGkrKyApIC0gZmlyc3QpICUgNyBdID0gZGF5O1xuXHRcdH0pO1xuXG5cdFx0cmV0dXJuIGRvdztcblx0fSxcblxuXHRyZW5kZXJEYXlzOiBmdW5jdGlvbigpIHtcblx0XHR2YXIgZGF0ZSA9IHRoaXMucHJvcHMudmlld0RhdGUsXG5cdFx0XHRzZWxlY3RlZCA9IHRoaXMucHJvcHMuc2VsZWN0ZWREYXRlICYmIHRoaXMucHJvcHMuc2VsZWN0ZWREYXRlLmNsb25lKCksXG5cdFx0XHRwcmV2TW9udGggPSBkYXRlLmNsb25lKCkuc3VidHJhY3QoIDEsICdtb250aHMnICksXG5cdFx0XHRjdXJyZW50WWVhciA9IGRhdGUueWVhcigpLFxuXHRcdFx0Y3VycmVudE1vbnRoID0gZGF0ZS5tb250aCgpLFxuXHRcdFx0d2Vla3MgPSBbXSxcblx0XHRcdGRheXMgPSBbXSxcblx0XHRcdHJlbmRlcmVyID0gdGhpcy5wcm9wcy5yZW5kZXJEYXkgfHwgdGhpcy5yZW5kZXJEYXksXG5cdFx0XHRpc1ZhbGlkID0gdGhpcy5wcm9wcy5pc1ZhbGlkRGF0ZSB8fCB0aGlzLmFsd2F5c1ZhbGlkRGF0ZSxcblx0XHRcdGNsYXNzZXMsIGlzRGlzYWJsZWQsIGRheVByb3BzLCBjdXJyZW50RGF0ZVxuXHRcdFx0O1xuXG5cdFx0Ly8gR28gdG8gdGhlIGxhc3Qgd2VlayBvZiB0aGUgcHJldmlvdXMgbW9udGhcblx0XHRwcmV2TW9udGguZGF0ZSggcHJldk1vbnRoLmRheXNJbk1vbnRoKCkgKS5zdGFydE9mKCAnd2VlaycgKTtcblx0XHR2YXIgbGFzdERheSA9IHByZXZNb250aC5jbG9uZSgpLmFkZCggNDIsICdkJyApO1xuXG5cdFx0d2hpbGUgKCBwcmV2TW9udGguaXNCZWZvcmUoIGxhc3REYXkgKSApIHtcblx0XHRcdGNsYXNzZXMgPSAncmR0RGF5Jztcblx0XHRcdGN1cnJlbnREYXRlID0gcHJldk1vbnRoLmNsb25lKCk7XG5cblx0XHRcdGlmICggKCBwcmV2TW9udGgueWVhcigpID09PSBjdXJyZW50WWVhciAmJiBwcmV2TW9udGgubW9udGgoKSA8IGN1cnJlbnRNb250aCApIHx8ICggcHJldk1vbnRoLnllYXIoKSA8IGN1cnJlbnRZZWFyICkgKVxuXHRcdFx0XHRjbGFzc2VzICs9ICcgcmR0T2xkJztcblx0XHRcdGVsc2UgaWYgKCAoIHByZXZNb250aC55ZWFyKCkgPT09IGN1cnJlbnRZZWFyICYmIHByZXZNb250aC5tb250aCgpID4gY3VycmVudE1vbnRoICkgfHwgKCBwcmV2TW9udGgueWVhcigpID4gY3VycmVudFllYXIgKSApXG5cdFx0XHRcdGNsYXNzZXMgKz0gJyByZHROZXcnO1xuXG5cdFx0XHRpZiAoIHNlbGVjdGVkICYmIHByZXZNb250aC5pc1NhbWUoIHNlbGVjdGVkLCAnZGF5JyApIClcblx0XHRcdFx0Y2xhc3NlcyArPSAnIHJkdEFjdGl2ZSc7XG5cblx0XHRcdGlmICggcHJldk1vbnRoLmlzU2FtZSggbW9tZW50KCksICdkYXknICkgKVxuXHRcdFx0XHRjbGFzc2VzICs9ICcgcmR0VG9kYXknO1xuXG5cdFx0XHRpc0Rpc2FibGVkID0gIWlzVmFsaWQoIGN1cnJlbnREYXRlLCBzZWxlY3RlZCApO1xuXHRcdFx0aWYgKCBpc0Rpc2FibGVkIClcblx0XHRcdFx0Y2xhc3NlcyArPSAnIHJkdERpc2FibGVkJztcblxuXHRcdFx0ZGF5UHJvcHMgPSB7XG5cdFx0XHRcdGtleTogcHJldk1vbnRoLmZvcm1hdCggJ01fRCcgKSxcblx0XHRcdFx0J2RhdGEtdmFsdWUnOiBwcmV2TW9udGguZGF0ZSgpLFxuXHRcdFx0XHRjbGFzc05hbWU6IGNsYXNzZXNcblx0XHRcdH07XG5cblx0XHRcdGlmICggIWlzRGlzYWJsZWQgKVxuXHRcdFx0XHRkYXlQcm9wcy5vbkNsaWNrID0gdGhpcy51cGRhdGVTZWxlY3RlZERhdGU7XG5cblx0XHRcdGRheXMucHVzaCggcmVuZGVyZXIoIGRheVByb3BzLCBjdXJyZW50RGF0ZSwgc2VsZWN0ZWQgKSApO1xuXG5cdFx0XHRpZiAoIGRheXMubGVuZ3RoID09PSA3ICkge1xuXHRcdFx0XHR3ZWVrcy5wdXNoKCBSZWFjdC5jcmVhdGVFbGVtZW50KCd0cicsIHsga2V5OiBwcmV2TW9udGguZm9ybWF0KCAnTV9EJyApfSwgZGF5cyApICk7XG5cdFx0XHRcdGRheXMgPSBbXTtcblx0XHRcdH1cblxuXHRcdFx0cHJldk1vbnRoLmFkZCggMSwgJ2QnICk7XG5cdFx0fVxuXG5cdFx0cmV0dXJuIHdlZWtzO1xuXHR9LFxuXG5cdHVwZGF0ZVNlbGVjdGVkRGF0ZTogZnVuY3Rpb24oIGV2ZW50ICkge1xuXHRcdHRoaXMucHJvcHMudXBkYXRlU2VsZWN0ZWREYXRlKCBldmVudCwgdHJ1ZSApO1xuXHR9LFxuXG5cdHJlbmRlckRheTogZnVuY3Rpb24oIHByb3BzLCBjdXJyZW50RGF0ZSApIHtcblx0XHRyZXR1cm4gUmVhY3QuY3JlYXRlRWxlbWVudCgndGQnLCAgcHJvcHMsIGN1cnJlbnREYXRlLmRhdGUoKSApO1xuXHR9LFxuXG5cdHJlbmRlckZvb3RlcjogZnVuY3Rpb24oKSB7XG5cdFx0aWYgKCAhdGhpcy5wcm9wcy50aW1lRm9ybWF0IClcblx0XHRcdHJldHVybiAnJztcblxuXHRcdHZhciBkYXRlID0gdGhpcy5wcm9wcy5zZWxlY3RlZERhdGUgfHwgdGhpcy5wcm9wcy52aWV3RGF0ZTtcblxuXHRcdHJldHVybiBSZWFjdC5jcmVhdGVFbGVtZW50KCd0Zm9vdCcsIHsga2V5OiAndGYnfSxcblx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RyJywge30sXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RkJywgeyBvbkNsaWNrOiB0aGlzLnByb3BzLnNob3dWaWV3KCAndGltZScgKSwgY29sU3BhbjogNywgY2xhc3NOYW1lOiAncmR0VGltZVRvZ2dsZScgfSwgZGF0ZS5mb3JtYXQoIHRoaXMucHJvcHMudGltZUZvcm1hdCApKVxuXHRcdFx0KVxuXHRcdCk7XG5cdH0sXG5cblx0YWx3YXlzVmFsaWREYXRlOiBmdW5jdGlvbigpIHtcblx0XHRyZXR1cm4gMTtcblx0fSxcblxuXHRoYW5kbGVDbGlja091dHNpZGU6IGZ1bmN0aW9uKCkge1xuXHRcdHRoaXMucHJvcHMuaGFuZGxlQ2xpY2tPdXRzaWRlKCk7XG5cdH1cbn0pKTtcblxubW9kdWxlLmV4cG9ydHMgPSBEYXRlVGltZVBpY2tlckRheXM7XG4iLCIndXNlIHN0cmljdCc7XG5cbnZhciBSZWFjdCA9IHJlcXVpcmUoJ3JlYWN0JyksXG5cdGNyZWF0ZUNsYXNzID0gcmVxdWlyZSgnY3JlYXRlLXJlYWN0LWNsYXNzJyksXG5cdG9uQ2xpY2tPdXRzaWRlID0gcmVxdWlyZSgncmVhY3Qtb25jbGlja291dHNpZGUnKS5kZWZhdWx0XG5cdDtcblxudmFyIERhdGVUaW1lUGlja2VyTW9udGhzID0gb25DbGlja091dHNpZGUoIGNyZWF0ZUNsYXNzKHtcblx0cmVuZGVyOiBmdW5jdGlvbigpIHtcblx0XHRyZXR1cm4gUmVhY3QuY3JlYXRlRWxlbWVudCgnZGl2JywgeyBjbGFzc05hbWU6ICdyZHRNb250aHMnIH0sIFtcblx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RhYmxlJywgeyBrZXk6ICdhJyB9LCBSZWFjdC5jcmVhdGVFbGVtZW50KCd0aGVhZCcsIHt9LCBSZWFjdC5jcmVhdGVFbGVtZW50KCd0cicsIHt9LCBbXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoJywgeyBrZXk6ICdwcmV2JywgY2xhc3NOYW1lOiAncmR0UHJldicsIG9uQ2xpY2s6IHRoaXMucHJvcHMuc3VidHJhY3RUaW1lKCAxLCAneWVhcnMnICl9LCBSZWFjdC5jcmVhdGVFbGVtZW50KCdzcGFuJywge30sICfigLknICkpLFxuXHRcdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0aCcsIHsga2V5OiAneWVhcicsIGNsYXNzTmFtZTogJ3JkdFN3aXRjaCcsIG9uQ2xpY2s6IHRoaXMucHJvcHMuc2hvd1ZpZXcoICd5ZWFycycgKSwgY29sU3BhbjogMiwgJ2RhdGEtdmFsdWUnOiB0aGlzLnByb3BzLnZpZXdEYXRlLnllYXIoKSB9LCB0aGlzLnByb3BzLnZpZXdEYXRlLnllYXIoKSApLFxuXHRcdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0aCcsIHsga2V5OiAnbmV4dCcsIGNsYXNzTmFtZTogJ3JkdE5leHQnLCBvbkNsaWNrOiB0aGlzLnByb3BzLmFkZFRpbWUoIDEsICd5ZWFycycgKX0sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3NwYW4nLCB7fSwgJ+KAuicgKSlcblx0XHRcdF0pKSksXG5cdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0YWJsZScsIHsga2V5OiAnbW9udGhzJyB9LCBSZWFjdC5jcmVhdGVFbGVtZW50KCd0Ym9keScsIHsga2V5OiAnYicgfSwgdGhpcy5yZW5kZXJNb250aHMoKSkpXG5cdFx0XSk7XG5cdH0sXG5cblx0cmVuZGVyTW9udGhzOiBmdW5jdGlvbigpIHtcblx0XHR2YXIgZGF0ZSA9IHRoaXMucHJvcHMuc2VsZWN0ZWREYXRlLFxuXHRcdFx0bW9udGggPSB0aGlzLnByb3BzLnZpZXdEYXRlLm1vbnRoKCksXG5cdFx0XHR5ZWFyID0gdGhpcy5wcm9wcy52aWV3RGF0ZS55ZWFyKCksXG5cdFx0XHRyb3dzID0gW10sXG5cdFx0XHRpID0gMCxcblx0XHRcdG1vbnRocyA9IFtdLFxuXHRcdFx0cmVuZGVyZXIgPSB0aGlzLnByb3BzLnJlbmRlck1vbnRoIHx8IHRoaXMucmVuZGVyTW9udGgsXG5cdFx0XHRpc1ZhbGlkID0gdGhpcy5wcm9wcy5pc1ZhbGlkRGF0ZSB8fCB0aGlzLmFsd2F5c1ZhbGlkRGF0ZSxcblx0XHRcdGNsYXNzZXMsIHByb3BzLCBjdXJyZW50TW9udGgsIGlzRGlzYWJsZWQsIG5vT2ZEYXlzSW5Nb250aCwgZGF5c0luTW9udGgsIHZhbGlkRGF5LFxuXHRcdFx0Ly8gRGF0ZSBpcyBpcnJlbGV2YW50IGJlY2F1c2Ugd2UncmUgb25seSBpbnRlcmVzdGVkIGluIG1vbnRoXG5cdFx0XHRpcnJlbGV2YW50RGF0ZSA9IDFcblx0XHRcdDtcblxuXHRcdHdoaWxlIChpIDwgMTIpIHtcblx0XHRcdGNsYXNzZXMgPSAncmR0TW9udGgnO1xuXHRcdFx0Y3VycmVudE1vbnRoID1cblx0XHRcdFx0dGhpcy5wcm9wcy52aWV3RGF0ZS5jbG9uZSgpLnNldCh7IHllYXI6IHllYXIsIG1vbnRoOiBpLCBkYXRlOiBpcnJlbGV2YW50RGF0ZSB9KTtcblxuXHRcdFx0bm9PZkRheXNJbk1vbnRoID0gY3VycmVudE1vbnRoLmVuZE9mKCAnbW9udGgnICkuZm9ybWF0KCAnRCcgKTtcblx0XHRcdGRheXNJbk1vbnRoID0gQXJyYXkuZnJvbSh7IGxlbmd0aDogbm9PZkRheXNJbk1vbnRoIH0sIGZ1bmN0aW9uKCBlLCBpICkge1xuXHRcdFx0XHRyZXR1cm4gaSArIDE7XG5cdFx0XHR9KTtcblxuXHRcdFx0dmFsaWREYXkgPSBkYXlzSW5Nb250aC5maW5kKGZ1bmN0aW9uKCBkICkge1xuXHRcdFx0XHR2YXIgZGF5ID0gY3VycmVudE1vbnRoLmNsb25lKCkuc2V0KCAnZGF0ZScsIGQgKTtcblx0XHRcdFx0cmV0dXJuIGlzVmFsaWQoIGRheSApO1xuXHRcdFx0fSk7XG5cblx0XHRcdGlzRGlzYWJsZWQgPSAoIHZhbGlkRGF5ID09PSB1bmRlZmluZWQgKTtcblxuXHRcdFx0aWYgKCBpc0Rpc2FibGVkIClcblx0XHRcdFx0Y2xhc3NlcyArPSAnIHJkdERpc2FibGVkJztcblxuXHRcdFx0aWYgKCBkYXRlICYmIGkgPT09IGRhdGUubW9udGgoKSAmJiB5ZWFyID09PSBkYXRlLnllYXIoKSApXG5cdFx0XHRcdGNsYXNzZXMgKz0gJyByZHRBY3RpdmUnO1xuXG5cdFx0XHRwcm9wcyA9IHtcblx0XHRcdFx0a2V5OiBpLFxuXHRcdFx0XHQnZGF0YS12YWx1ZSc6IGksXG5cdFx0XHRcdGNsYXNzTmFtZTogY2xhc3Nlc1xuXHRcdFx0fTtcblxuXHRcdFx0aWYgKCAhaXNEaXNhYmxlZCApXG5cdFx0XHRcdHByb3BzLm9uQ2xpY2sgPSAoIHRoaXMucHJvcHMudXBkYXRlT24gPT09ICdtb250aHMnID9cblx0XHRcdFx0XHR0aGlzLnVwZGF0ZVNlbGVjdGVkTW9udGggOiB0aGlzLnByb3BzLnNldERhdGUoICdtb250aCcgKSApO1xuXG5cdFx0XHRtb250aHMucHVzaCggcmVuZGVyZXIoIHByb3BzLCBpLCB5ZWFyLCBkYXRlICYmIGRhdGUuY2xvbmUoKSApICk7XG5cblx0XHRcdGlmICggbW9udGhzLmxlbmd0aCA9PT0gNCApIHtcblx0XHRcdFx0cm93cy5wdXNoKCBSZWFjdC5jcmVhdGVFbGVtZW50KCd0cicsIHsga2V5OiBtb250aCArICdfJyArIHJvd3MubGVuZ3RoIH0sIG1vbnRocyApICk7XG5cdFx0XHRcdG1vbnRocyA9IFtdO1xuXHRcdFx0fVxuXG5cdFx0XHRpKys7XG5cdFx0fVxuXG5cdFx0cmV0dXJuIHJvd3M7XG5cdH0sXG5cblx0dXBkYXRlU2VsZWN0ZWRNb250aDogZnVuY3Rpb24oIGV2ZW50ICkge1xuXHRcdHRoaXMucHJvcHMudXBkYXRlU2VsZWN0ZWREYXRlKCBldmVudCApO1xuXHR9LFxuXG5cdHJlbmRlck1vbnRoOiBmdW5jdGlvbiggcHJvcHMsIG1vbnRoICkge1xuXHRcdHZhciBsb2NhbE1vbWVudCA9IHRoaXMucHJvcHMudmlld0RhdGU7XG5cdFx0dmFyIG1vbnRoU3RyID0gbG9jYWxNb21lbnQubG9jYWxlRGF0YSgpLm1vbnRoc1Nob3J0KCBsb2NhbE1vbWVudC5tb250aCggbW9udGggKSApO1xuXHRcdHZhciBzdHJMZW5ndGggPSAzO1xuXHRcdC8vIEJlY2F1c2Ugc29tZSBtb250aHMgYXJlIHVwIHRvIDUgY2hhcmFjdGVycyBsb25nLCB3ZSB3YW50IHRvXG5cdFx0Ly8gdXNlIGEgZml4ZWQgc3RyaW5nIGxlbmd0aCBmb3IgY29uc2lzdGVuY3lcblx0XHR2YXIgbW9udGhTdHJGaXhlZExlbmd0aCA9IG1vbnRoU3RyLnN1YnN0cmluZyggMCwgc3RyTGVuZ3RoICk7XG5cdFx0cmV0dXJuIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RkJywgcHJvcHMsIGNhcGl0YWxpemUoIG1vbnRoU3RyRml4ZWRMZW5ndGggKSApO1xuXHR9LFxuXG5cdGFsd2F5c1ZhbGlkRGF0ZTogZnVuY3Rpb24oKSB7XG5cdFx0cmV0dXJuIDE7XG5cdH0sXG5cblx0aGFuZGxlQ2xpY2tPdXRzaWRlOiBmdW5jdGlvbigpIHtcblx0XHR0aGlzLnByb3BzLmhhbmRsZUNsaWNrT3V0c2lkZSgpO1xuXHR9XG59KSk7XG5cbmZ1bmN0aW9uIGNhcGl0YWxpemUoIHN0ciApIHtcblx0cmV0dXJuIHN0ci5jaGFyQXQoIDAgKS50b1VwcGVyQ2FzZSgpICsgc3RyLnNsaWNlKCAxICk7XG59XG5cbm1vZHVsZS5leHBvcnRzID0gRGF0ZVRpbWVQaWNrZXJNb250aHM7XG4iLCIndXNlIHN0cmljdCc7XG5cbnZhciBSZWFjdCA9IHJlcXVpcmUoJ3JlYWN0JyksXG5cdGNyZWF0ZUNsYXNzID0gcmVxdWlyZSgnY3JlYXRlLXJlYWN0LWNsYXNzJyksXG5cdGFzc2lnbiA9IHJlcXVpcmUoJ29iamVjdC1hc3NpZ24nKSxcblx0b25DbGlja091dHNpZGUgPSByZXF1aXJlKCdyZWFjdC1vbmNsaWNrb3V0c2lkZScpLmRlZmF1bHRcblx0O1xuXG52YXIgRGF0ZVRpbWVQaWNrZXJUaW1lID0gb25DbGlja091dHNpZGUoIGNyZWF0ZUNsYXNzKHtcblx0Z2V0SW5pdGlhbFN0YXRlOiBmdW5jdGlvbigpIHtcblx0XHRyZXR1cm4gdGhpcy5jYWxjdWxhdGVTdGF0ZSggdGhpcy5wcm9wcyApO1xuXHR9LFxuXG5cdGNhbGN1bGF0ZVN0YXRlOiBmdW5jdGlvbiggcHJvcHMgKSB7XG5cdFx0dmFyIGRhdGUgPSBwcm9wcy5zZWxlY3RlZERhdGUgfHwgcHJvcHMudmlld0RhdGUsXG5cdFx0XHRmb3JtYXQgPSBwcm9wcy50aW1lRm9ybWF0LFxuXHRcdFx0Y291bnRlcnMgPSBbXVxuXHRcdFx0O1xuXG5cdFx0aWYgKCBmb3JtYXQudG9Mb3dlckNhc2UoKS5pbmRleE9mKCdoJykgIT09IC0xICkge1xuXHRcdFx0Y291bnRlcnMucHVzaCgnaG91cnMnKTtcblx0XHRcdGlmICggZm9ybWF0LmluZGV4T2YoJ20nKSAhPT0gLTEgKSB7XG5cdFx0XHRcdGNvdW50ZXJzLnB1c2goJ21pbnV0ZXMnKTtcblx0XHRcdFx0aWYgKCBmb3JtYXQuaW5kZXhPZigncycpICE9PSAtMSApIHtcblx0XHRcdFx0XHRjb3VudGVycy5wdXNoKCdzZWNvbmRzJyk7XG5cdFx0XHRcdH1cblx0XHRcdH1cblx0XHR9XG5cblx0XHR2YXIgaG91cnMgPSBkYXRlLmZvcm1hdCggJ0gnICk7XG5cdFx0XG5cdFx0dmFyIGRheXBhcnQgPSBmYWxzZTtcblx0XHRpZiAoIHRoaXMuc3RhdGUgIT09IG51bGwgJiYgdGhpcy5wcm9wcy50aW1lRm9ybWF0LnRvTG93ZXJDYXNlKCkuaW5kZXhPZiggJyBhJyApICE9PSAtMSApIHtcblx0XHRcdGlmICggdGhpcy5wcm9wcy50aW1lRm9ybWF0LmluZGV4T2YoICcgQScgKSAhPT0gLTEgKSB7XG5cdFx0XHRcdGRheXBhcnQgPSAoIGhvdXJzID49IDEyICkgPyAnUE0nIDogJ0FNJztcblx0XHRcdH0gZWxzZSB7XG5cdFx0XHRcdGRheXBhcnQgPSAoIGhvdXJzID49IDEyICkgPyAncG0nIDogJ2FtJztcblx0XHRcdH1cblx0XHR9XG5cblx0XHRyZXR1cm4ge1xuXHRcdFx0aG91cnM6IGhvdXJzLFxuXHRcdFx0bWludXRlczogZGF0ZS5mb3JtYXQoICdtbScgKSxcblx0XHRcdHNlY29uZHM6IGRhdGUuZm9ybWF0KCAnc3MnICksXG5cdFx0XHRtaWxsaXNlY29uZHM6IGRhdGUuZm9ybWF0KCAnU1NTJyApLFxuXHRcdFx0ZGF5cGFydDogZGF5cGFydCxcblx0XHRcdGNvdW50ZXJzOiBjb3VudGVyc1xuXHRcdH07XG5cdH0sXG5cblx0cmVuZGVyQ291bnRlcjogZnVuY3Rpb24oIHR5cGUgKSB7XG5cdFx0aWYgKCB0eXBlICE9PSAnZGF5cGFydCcgKSB7XG5cdFx0XHR2YXIgdmFsdWUgPSB0aGlzLnN0YXRlWyB0eXBlIF07XG5cdFx0XHRpZiAoIHR5cGUgPT09ICdob3VycycgJiYgdGhpcy5wcm9wcy50aW1lRm9ybWF0LnRvTG93ZXJDYXNlKCkuaW5kZXhPZiggJyBhJyApICE9PSAtMSApIHtcblx0XHRcdFx0dmFsdWUgPSAoIHZhbHVlIC0gMSApICUgMTIgKyAxO1xuXG5cdFx0XHRcdGlmICggdmFsdWUgPT09IDAgKSB7XG5cdFx0XHRcdFx0dmFsdWUgPSAxMjtcblx0XHRcdFx0fVxuXHRcdFx0fVxuXHRcdFx0cmV0dXJuIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ2RpdicsIHsga2V5OiB0eXBlLCBjbGFzc05hbWU6ICdyZHRDb3VudGVyJyB9LCBbXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3NwYW4nLCB7IGtleTogJ3VwJywgY2xhc3NOYW1lOiAncmR0QnRuJywgb25Ub3VjaFN0YXJ0OiB0aGlzLm9uU3RhcnRDbGlja2luZygnaW5jcmVhc2UnLCB0eXBlKSwgb25Nb3VzZURvd246IHRoaXMub25TdGFydENsaWNraW5nKCAnaW5jcmVhc2UnLCB0eXBlICksIG9uQ29udGV4dE1lbnU6IHRoaXMuZGlzYWJsZUNvbnRleHRNZW51IH0sICfilrInICksXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ2RpdicsIHsga2V5OiAnYycsIGNsYXNzTmFtZTogJ3JkdENvdW50JyB9LCB2YWx1ZSApLFxuXHRcdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCdzcGFuJywgeyBrZXk6ICdkbycsIGNsYXNzTmFtZTogJ3JkdEJ0bicsIG9uVG91Y2hTdGFydDogdGhpcy5vblN0YXJ0Q2xpY2tpbmcoJ2RlY3JlYXNlJywgdHlwZSksIG9uTW91c2VEb3duOiB0aGlzLm9uU3RhcnRDbGlja2luZyggJ2RlY3JlYXNlJywgdHlwZSApLCBvbkNvbnRleHRNZW51OiB0aGlzLmRpc2FibGVDb250ZXh0TWVudSB9LCAn4pa8JyApXG5cdFx0XHRdKTtcblx0XHR9XG5cdFx0cmV0dXJuICcnO1xuXHR9LFxuXG5cdHJlbmRlckRheVBhcnQ6IGZ1bmN0aW9uKCkge1xuXHRcdHJldHVybiBSZWFjdC5jcmVhdGVFbGVtZW50KCdkaXYnLCB7IGtleTogJ2RheVBhcnQnLCBjbGFzc05hbWU6ICdyZHRDb3VudGVyJyB9LCBbXG5cdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCdzcGFuJywgeyBrZXk6ICd1cCcsIGNsYXNzTmFtZTogJ3JkdEJ0bicsIG9uVG91Y2hTdGFydDogdGhpcy5vblN0YXJ0Q2xpY2tpbmcoJ3RvZ2dsZURheVBhcnQnLCAnaG91cnMnKSwgb25Nb3VzZURvd246IHRoaXMub25TdGFydENsaWNraW5nKCAndG9nZ2xlRGF5UGFydCcsICdob3VycycpLCBvbkNvbnRleHRNZW51OiB0aGlzLmRpc2FibGVDb250ZXh0TWVudSB9LCAn4payJyApLFxuXHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgnZGl2JywgeyBrZXk6IHRoaXMuc3RhdGUuZGF5cGFydCwgY2xhc3NOYW1lOiAncmR0Q291bnQnIH0sIHRoaXMuc3RhdGUuZGF5cGFydCApLFxuXHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgnc3BhbicsIHsga2V5OiAnZG8nLCBjbGFzc05hbWU6ICdyZHRCdG4nLCBvblRvdWNoU3RhcnQ6IHRoaXMub25TdGFydENsaWNraW5nKCd0b2dnbGVEYXlQYXJ0JywgJ2hvdXJzJyksIG9uTW91c2VEb3duOiB0aGlzLm9uU3RhcnRDbGlja2luZyggJ3RvZ2dsZURheVBhcnQnLCAnaG91cnMnKSwgb25Db250ZXh0TWVudTogdGhpcy5kaXNhYmxlQ29udGV4dE1lbnUgfSwgJ+KWvCcgKVxuXHRcdF0pO1xuXHR9LFxuXG5cdHJlbmRlcjogZnVuY3Rpb24oKSB7XG5cdFx0dmFyIG1lID0gdGhpcyxcblx0XHRcdGNvdW50ZXJzID0gW11cblx0XHQ7XG5cblx0XHR0aGlzLnN0YXRlLmNvdW50ZXJzLmZvckVhY2goIGZ1bmN0aW9uKCBjICkge1xuXHRcdFx0aWYgKCBjb3VudGVycy5sZW5ndGggKVxuXHRcdFx0XHRjb3VudGVycy5wdXNoKCBSZWFjdC5jcmVhdGVFbGVtZW50KCdkaXYnLCB7IGtleTogJ3NlcCcgKyBjb3VudGVycy5sZW5ndGgsIGNsYXNzTmFtZTogJ3JkdENvdW50ZXJTZXBhcmF0b3InIH0sICc6JyApICk7XG5cdFx0XHRjb3VudGVycy5wdXNoKCBtZS5yZW5kZXJDb3VudGVyKCBjICkgKTtcblx0XHR9KTtcblxuXHRcdGlmICggdGhpcy5zdGF0ZS5kYXlwYXJ0ICE9PSBmYWxzZSApIHtcblx0XHRcdGNvdW50ZXJzLnB1c2goIG1lLnJlbmRlckRheVBhcnQoKSApO1xuXHRcdH1cblxuXHRcdGlmICggdGhpcy5zdGF0ZS5jb3VudGVycy5sZW5ndGggPT09IDMgJiYgdGhpcy5wcm9wcy50aW1lRm9ybWF0LmluZGV4T2YoICdTJyApICE9PSAtMSApIHtcblx0XHRcdGNvdW50ZXJzLnB1c2goIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ2RpdicsIHsgY2xhc3NOYW1lOiAncmR0Q291bnRlclNlcGFyYXRvcicsIGtleTogJ3NlcDUnIH0sICc6JyApICk7XG5cdFx0XHRjb3VudGVycy5wdXNoKFxuXHRcdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCdkaXYnLCB7IGNsYXNzTmFtZTogJ3JkdENvdW50ZXIgcmR0TWlsbGknLCBrZXk6ICdtJyB9LFxuXHRcdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ2lucHV0JywgeyB2YWx1ZTogdGhpcy5zdGF0ZS5taWxsaXNlY29uZHMsIHR5cGU6ICd0ZXh0Jywgb25DaGFuZ2U6IHRoaXMudXBkYXRlTWlsbGkgfSApXG5cdFx0XHRcdFx0KVxuXHRcdFx0XHQpO1xuXHRcdH1cblxuXHRcdHJldHVybiBSZWFjdC5jcmVhdGVFbGVtZW50KCdkaXYnLCB7IGNsYXNzTmFtZTogJ3JkdFRpbWUnIH0sXG5cdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0YWJsZScsIHt9LCBbXG5cdFx0XHRcdHRoaXMucmVuZGVySGVhZGVyKCksXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3Rib2R5JywgeyBrZXk6ICdiJ30sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RyJywge30sIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RkJywge30sXG5cdFx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgnZGl2JywgeyBjbGFzc05hbWU6ICdyZHRDb3VudGVycycgfSwgY291bnRlcnMgKVxuXHRcdFx0XHQpKSlcblx0XHRcdF0pXG5cdFx0KTtcblx0fSxcblxuXHRjb21wb25lbnRXaWxsTW91bnQ6IGZ1bmN0aW9uKCkge1xuXHRcdHZhciBtZSA9IHRoaXM7XG5cdFx0bWUudGltZUNvbnN0cmFpbnRzID0ge1xuXHRcdFx0aG91cnM6IHtcblx0XHRcdFx0bWluOiAwLFxuXHRcdFx0XHRtYXg6IDIzLFxuXHRcdFx0XHRzdGVwOiAxXG5cdFx0XHR9LFxuXHRcdFx0bWludXRlczoge1xuXHRcdFx0XHRtaW46IDAsXG5cdFx0XHRcdG1heDogNTksXG5cdFx0XHRcdHN0ZXA6IDFcblx0XHRcdH0sXG5cdFx0XHRzZWNvbmRzOiB7XG5cdFx0XHRcdG1pbjogMCxcblx0XHRcdFx0bWF4OiA1OSxcblx0XHRcdFx0c3RlcDogMVxuXHRcdFx0fSxcblx0XHRcdG1pbGxpc2Vjb25kczoge1xuXHRcdFx0XHRtaW46IDAsXG5cdFx0XHRcdG1heDogOTk5LFxuXHRcdFx0XHRzdGVwOiAxXG5cdFx0XHR9XG5cdFx0fTtcblx0XHRbJ2hvdXJzJywgJ21pbnV0ZXMnLCAnc2Vjb25kcycsICdtaWxsaXNlY29uZHMnXS5mb3JFYWNoKCBmdW5jdGlvbiggdHlwZSApIHtcblx0XHRcdGFzc2lnbihtZS50aW1lQ29uc3RyYWludHNbIHR5cGUgXSwgbWUucHJvcHMudGltZUNvbnN0cmFpbnRzWyB0eXBlIF0pO1xuXHRcdH0pO1xuXHRcdHRoaXMuc2V0U3RhdGUoIHRoaXMuY2FsY3VsYXRlU3RhdGUoIHRoaXMucHJvcHMgKSApO1xuXHR9LFxuXG5cdGNvbXBvbmVudFdpbGxSZWNlaXZlUHJvcHM6IGZ1bmN0aW9uKCBuZXh0UHJvcHMgKSB7XG5cdFx0dGhpcy5zZXRTdGF0ZSggdGhpcy5jYWxjdWxhdGVTdGF0ZSggbmV4dFByb3BzICkgKTtcblx0fSxcblxuXHR1cGRhdGVNaWxsaTogZnVuY3Rpb24oIGUgKSB7XG5cdFx0dmFyIG1pbGxpID0gcGFyc2VJbnQoIGUudGFyZ2V0LnZhbHVlLCAxMCApO1xuXHRcdGlmICggbWlsbGkgPT09IGUudGFyZ2V0LnZhbHVlICYmIG1pbGxpID49IDAgJiYgbWlsbGkgPCAxMDAwICkge1xuXHRcdFx0dGhpcy5wcm9wcy5zZXRUaW1lKCAnbWlsbGlzZWNvbmRzJywgbWlsbGkgKTtcblx0XHRcdHRoaXMuc2V0U3RhdGUoIHsgbWlsbGlzZWNvbmRzOiBtaWxsaSB9ICk7XG5cdFx0fVxuXHR9LFxuXG5cdHJlbmRlckhlYWRlcjogZnVuY3Rpb24oKSB7XG5cdFx0aWYgKCAhdGhpcy5wcm9wcy5kYXRlRm9ybWF0IClcblx0XHRcdHJldHVybiBudWxsO1xuXG5cdFx0dmFyIGRhdGUgPSB0aGlzLnByb3BzLnNlbGVjdGVkRGF0ZSB8fCB0aGlzLnByb3BzLnZpZXdEYXRlO1xuXHRcdHJldHVybiBSZWFjdC5jcmVhdGVFbGVtZW50KCd0aGVhZCcsIHsga2V5OiAnaCcgfSwgUmVhY3QuY3JlYXRlRWxlbWVudCgndHInLCB7fSxcblx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoJywgeyBjbGFzc05hbWU6ICdyZHRTd2l0Y2gnLCBjb2xTcGFuOiA0LCBvbkNsaWNrOiB0aGlzLnByb3BzLnNob3dWaWV3KCAnZGF5cycgKSB9LCBkYXRlLmZvcm1hdCggdGhpcy5wcm9wcy5kYXRlRm9ybWF0ICkgKVxuXHRcdCkpO1xuXHR9LFxuXG5cdG9uU3RhcnRDbGlja2luZzogZnVuY3Rpb24oIGFjdGlvbiwgdHlwZSApIHtcblx0XHR2YXIgbWUgPSB0aGlzO1xuXG5cdFx0cmV0dXJuIGZ1bmN0aW9uKCkge1xuXHRcdFx0dmFyIHVwZGF0ZSA9IHt9O1xuXHRcdFx0dXBkYXRlWyB0eXBlIF0gPSBtZVsgYWN0aW9uIF0oIHR5cGUgKTtcblx0XHRcdG1lLnNldFN0YXRlKCB1cGRhdGUgKTtcblxuXHRcdFx0bWUudGltZXIgPSBzZXRUaW1lb3V0KCBmdW5jdGlvbigpIHtcblx0XHRcdFx0bWUuaW5jcmVhc2VUaW1lciA9IHNldEludGVydmFsKCBmdW5jdGlvbigpIHtcblx0XHRcdFx0XHR1cGRhdGVbIHR5cGUgXSA9IG1lWyBhY3Rpb24gXSggdHlwZSApO1xuXHRcdFx0XHRcdG1lLnNldFN0YXRlKCB1cGRhdGUgKTtcblx0XHRcdFx0fSwgNzApO1xuXHRcdFx0fSwgNTAwKTtcblxuXHRcdFx0bWUubW91c2VVcExpc3RlbmVyID0gZnVuY3Rpb24oKSB7XG5cdFx0XHRcdGNsZWFyVGltZW91dCggbWUudGltZXIgKTtcblx0XHRcdFx0Y2xlYXJJbnRlcnZhbCggbWUuaW5jcmVhc2VUaW1lciApO1xuXHRcdFx0XHRtZS5wcm9wcy5zZXRUaW1lKCB0eXBlLCBtZS5zdGF0ZVsgdHlwZSBdICk7XG5cdFx0XHRcdGRvY3VtZW50LmJvZHkucmVtb3ZlRXZlbnRMaXN0ZW5lciggJ21vdXNldXAnLCBtZS5tb3VzZVVwTGlzdGVuZXIgKTtcblx0XHRcdFx0ZG9jdW1lbnQuYm9keS5yZW1vdmVFdmVudExpc3RlbmVyKCAndG91Y2hlbmQnLCBtZS5tb3VzZVVwTGlzdGVuZXIgKTtcblx0XHRcdH07XG5cblx0XHRcdGRvY3VtZW50LmJvZHkuYWRkRXZlbnRMaXN0ZW5lciggJ21vdXNldXAnLCBtZS5tb3VzZVVwTGlzdGVuZXIgKTtcblx0XHRcdGRvY3VtZW50LmJvZHkuYWRkRXZlbnRMaXN0ZW5lciggJ3RvdWNoZW5kJywgbWUubW91c2VVcExpc3RlbmVyICk7XG5cdFx0fTtcblx0fSxcblxuXHRkaXNhYmxlQ29udGV4dE1lbnU6IGZ1bmN0aW9uKCBldmVudCApIHtcblx0XHRldmVudC5wcmV2ZW50RGVmYXVsdCgpO1xuXHRcdHJldHVybiBmYWxzZTtcblx0fSxcblxuXHRwYWRWYWx1ZXM6IHtcblx0XHRob3VyczogMSxcblx0XHRtaW51dGVzOiAyLFxuXHRcdHNlY29uZHM6IDIsXG5cdFx0bWlsbGlzZWNvbmRzOiAzXG5cdH0sXG5cblx0dG9nZ2xlRGF5UGFydDogZnVuY3Rpb24oIHR5cGUgKSB7IC8vIHR5cGUgaXMgYWx3YXlzICdob3Vycydcblx0XHR2YXIgdmFsdWUgPSBwYXJzZUludCggdGhpcy5zdGF0ZVsgdHlwZSBdLCAxMCkgKyAxMjtcblx0XHRpZiAoIHZhbHVlID4gdGhpcy50aW1lQ29uc3RyYWludHNbIHR5cGUgXS5tYXggKVxuXHRcdFx0dmFsdWUgPSB0aGlzLnRpbWVDb25zdHJhaW50c1sgdHlwZSBdLm1pbiArICggdmFsdWUgLSAoIHRoaXMudGltZUNvbnN0cmFpbnRzWyB0eXBlIF0ubWF4ICsgMSApICk7XG5cdFx0cmV0dXJuIHRoaXMucGFkKCB0eXBlLCB2YWx1ZSApO1xuXHR9LFxuXG5cdGluY3JlYXNlOiBmdW5jdGlvbiggdHlwZSApIHtcblx0XHR2YXIgdmFsdWUgPSBwYXJzZUludCggdGhpcy5zdGF0ZVsgdHlwZSBdLCAxMCkgKyB0aGlzLnRpbWVDb25zdHJhaW50c1sgdHlwZSBdLnN0ZXA7XG5cdFx0aWYgKCB2YWx1ZSA+IHRoaXMudGltZUNvbnN0cmFpbnRzWyB0eXBlIF0ubWF4IClcblx0XHRcdHZhbHVlID0gdGhpcy50aW1lQ29uc3RyYWludHNbIHR5cGUgXS5taW4gKyAoIHZhbHVlIC0gKCB0aGlzLnRpbWVDb25zdHJhaW50c1sgdHlwZSBdLm1heCArIDEgKSApO1xuXHRcdHJldHVybiB0aGlzLnBhZCggdHlwZSwgdmFsdWUgKTtcblx0fSxcblxuXHRkZWNyZWFzZTogZnVuY3Rpb24oIHR5cGUgKSB7XG5cdFx0dmFyIHZhbHVlID0gcGFyc2VJbnQoIHRoaXMuc3RhdGVbIHR5cGUgXSwgMTApIC0gdGhpcy50aW1lQ29uc3RyYWludHNbIHR5cGUgXS5zdGVwO1xuXHRcdGlmICggdmFsdWUgPCB0aGlzLnRpbWVDb25zdHJhaW50c1sgdHlwZSBdLm1pbiApXG5cdFx0XHR2YWx1ZSA9IHRoaXMudGltZUNvbnN0cmFpbnRzWyB0eXBlIF0ubWF4ICsgMSAtICggdGhpcy50aW1lQ29uc3RyYWludHNbIHR5cGUgXS5taW4gLSB2YWx1ZSApO1xuXHRcdHJldHVybiB0aGlzLnBhZCggdHlwZSwgdmFsdWUgKTtcblx0fSxcblxuXHRwYWQ6IGZ1bmN0aW9uKCB0eXBlLCB2YWx1ZSApIHtcblx0XHR2YXIgc3RyID0gdmFsdWUgKyAnJztcblx0XHR3aGlsZSAoIHN0ci5sZW5ndGggPCB0aGlzLnBhZFZhbHVlc1sgdHlwZSBdIClcblx0XHRcdHN0ciA9ICcwJyArIHN0cjtcblx0XHRyZXR1cm4gc3RyO1xuXHR9LFxuXG5cdGhhbmRsZUNsaWNrT3V0c2lkZTogZnVuY3Rpb24oKSB7XG5cdFx0dGhpcy5wcm9wcy5oYW5kbGVDbGlja091dHNpZGUoKTtcblx0fVxufSkpO1xuXG5tb2R1bGUuZXhwb3J0cyA9IERhdGVUaW1lUGlja2VyVGltZTtcbiIsIid1c2Ugc3RyaWN0JztcblxudmFyIFJlYWN0ID0gcmVxdWlyZSgncmVhY3QnKSxcblx0Y3JlYXRlQ2xhc3MgPSByZXF1aXJlKCdjcmVhdGUtcmVhY3QtY2xhc3MnKSxcblx0b25DbGlja091dHNpZGUgPSByZXF1aXJlKCdyZWFjdC1vbmNsaWNrb3V0c2lkZScpLmRlZmF1bHRcblx0O1xuXG52YXIgRGF0ZVRpbWVQaWNrZXJZZWFycyA9IG9uQ2xpY2tPdXRzaWRlKCBjcmVhdGVDbGFzcyh7XG5cdHJlbmRlcjogZnVuY3Rpb24oKSB7XG5cdFx0dmFyIHllYXIgPSBwYXJzZUludCggdGhpcy5wcm9wcy52aWV3RGF0ZS55ZWFyKCkgLyAxMCwgMTAgKSAqIDEwO1xuXG5cdFx0cmV0dXJuIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ2RpdicsIHsgY2xhc3NOYW1lOiAncmR0WWVhcnMnIH0sIFtcblx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RhYmxlJywgeyBrZXk6ICdhJyB9LCBSZWFjdC5jcmVhdGVFbGVtZW50KCd0aGVhZCcsIHt9LCBSZWFjdC5jcmVhdGVFbGVtZW50KCd0cicsIHt9LCBbXG5cdFx0XHRcdFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RoJywgeyBrZXk6ICdwcmV2JywgY2xhc3NOYW1lOiAncmR0UHJldicsIG9uQ2xpY2s6IHRoaXMucHJvcHMuc3VidHJhY3RUaW1lKCAxMCwgJ3llYXJzJyApfSwgUmVhY3QuY3JlYXRlRWxlbWVudCgnc3BhbicsIHt9LCAn4oC5JyApKSxcblx0XHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGgnLCB7IGtleTogJ3llYXInLCBjbGFzc05hbWU6ICdyZHRTd2l0Y2gnLCBvbkNsaWNrOiB0aGlzLnByb3BzLnNob3dWaWV3KCAneWVhcnMnICksIGNvbFNwYW46IDIgfSwgeWVhciArICctJyArICggeWVhciArIDkgKSApLFxuXHRcdFx0XHRSZWFjdC5jcmVhdGVFbGVtZW50KCd0aCcsIHsga2V5OiAnbmV4dCcsIGNsYXNzTmFtZTogJ3JkdE5leHQnLCBvbkNsaWNrOiB0aGlzLnByb3BzLmFkZFRpbWUoIDEwLCAneWVhcnMnICl9LCBSZWFjdC5jcmVhdGVFbGVtZW50KCdzcGFuJywge30sICfigLonICkpXG5cdFx0XHRdKSkpLFxuXHRcdFx0UmVhY3QuY3JlYXRlRWxlbWVudCgndGFibGUnLCB7IGtleTogJ3llYXJzJyB9LCBSZWFjdC5jcmVhdGVFbGVtZW50KCd0Ym9keScsICB7fSwgdGhpcy5yZW5kZXJZZWFycyggeWVhciApKSlcblx0XHRdKTtcblx0fSxcblxuXHRyZW5kZXJZZWFyczogZnVuY3Rpb24oIHllYXIgKSB7XG5cdFx0dmFyIHllYXJzID0gW10sXG5cdFx0XHRpID0gLTEsXG5cdFx0XHRyb3dzID0gW10sXG5cdFx0XHRyZW5kZXJlciA9IHRoaXMucHJvcHMucmVuZGVyWWVhciB8fCB0aGlzLnJlbmRlclllYXIsXG5cdFx0XHRzZWxlY3RlZERhdGUgPSB0aGlzLnByb3BzLnNlbGVjdGVkRGF0ZSxcblx0XHRcdGlzVmFsaWQgPSB0aGlzLnByb3BzLmlzVmFsaWREYXRlIHx8IHRoaXMuYWx3YXlzVmFsaWREYXRlLFxuXHRcdFx0Y2xhc3NlcywgcHJvcHMsIGN1cnJlbnRZZWFyLCBpc0Rpc2FibGVkLCBub09mRGF5c0luWWVhciwgZGF5c0luWWVhciwgdmFsaWREYXksXG5cdFx0XHQvLyBNb250aCBhbmQgZGF0ZSBhcmUgaXJyZWxldmFudCBoZXJlIGJlY2F1c2Vcblx0XHRcdC8vIHdlJ3JlIG9ubHkgaW50ZXJlc3RlZCBpbiB0aGUgeWVhclxuXHRcdFx0aXJyZWxldmFudE1vbnRoID0gMCxcblx0XHRcdGlycmVsZXZhbnREYXRlID0gMVxuXHRcdFx0O1xuXG5cdFx0eWVhci0tO1xuXHRcdHdoaWxlIChpIDwgMTEpIHtcblx0XHRcdGNsYXNzZXMgPSAncmR0WWVhcic7XG5cdFx0XHRjdXJyZW50WWVhciA9IHRoaXMucHJvcHMudmlld0RhdGUuY2xvbmUoKS5zZXQoXG5cdFx0XHRcdHsgeWVhcjogeWVhciwgbW9udGg6IGlycmVsZXZhbnRNb250aCwgZGF0ZTogaXJyZWxldmFudERhdGUgfSApO1xuXG5cdFx0XHQvLyBOb3Qgc3VyZSB3aGF0ICdyZHRPbGQnIGlzIGZvciwgY29tbWVudGluZyBvdXQgZm9yIG5vdyBhcyBpdCdzIG5vdCB3b3JraW5nIHByb3Blcmx5XG5cdFx0XHQvLyBpZiAoIGkgPT09IC0xIHwgaSA9PT0gMTAgKVxuXHRcdFx0XHQvLyBjbGFzc2VzICs9ICcgcmR0T2xkJztcblxuXHRcdFx0bm9PZkRheXNJblllYXIgPSBjdXJyZW50WWVhci5lbmRPZiggJ3llYXInICkuZm9ybWF0KCAnREREJyApO1xuXHRcdFx0ZGF5c0luWWVhciA9IEFycmF5LmZyb20oeyBsZW5ndGg6IG5vT2ZEYXlzSW5ZZWFyIH0sIGZ1bmN0aW9uKCBlLCBpICkge1xuXHRcdFx0XHRyZXR1cm4gaSArIDE7XG5cdFx0XHR9KTtcblxuXHRcdFx0dmFsaWREYXkgPSBkYXlzSW5ZZWFyLmZpbmQoZnVuY3Rpb24oIGQgKSB7XG5cdFx0XHRcdHZhciBkYXkgPSBjdXJyZW50WWVhci5jbG9uZSgpLmRheU9mWWVhciggZCApO1xuXHRcdFx0XHRyZXR1cm4gaXNWYWxpZCggZGF5ICk7XG5cdFx0XHR9KTtcblxuXHRcdFx0aXNEaXNhYmxlZCA9ICggdmFsaWREYXkgPT09IHVuZGVmaW5lZCApO1xuXG5cdFx0XHRpZiAoIGlzRGlzYWJsZWQgKVxuXHRcdFx0XHRjbGFzc2VzICs9ICcgcmR0RGlzYWJsZWQnO1xuXG5cdFx0XHRpZiAoIHNlbGVjdGVkRGF0ZSAmJiBzZWxlY3RlZERhdGUueWVhcigpID09PSB5ZWFyIClcblx0XHRcdFx0Y2xhc3NlcyArPSAnIHJkdEFjdGl2ZSc7XG5cblx0XHRcdHByb3BzID0ge1xuXHRcdFx0XHRrZXk6IHllYXIsXG5cdFx0XHRcdCdkYXRhLXZhbHVlJzogeWVhcixcblx0XHRcdFx0Y2xhc3NOYW1lOiBjbGFzc2VzXG5cdFx0XHR9O1xuXG5cdFx0XHRpZiAoICFpc0Rpc2FibGVkIClcblx0XHRcdFx0cHJvcHMub25DbGljayA9ICggdGhpcy5wcm9wcy51cGRhdGVPbiA9PT0gJ3llYXJzJyA/XG5cdFx0XHRcdFx0dGhpcy51cGRhdGVTZWxlY3RlZFllYXIgOiB0aGlzLnByb3BzLnNldERhdGUoJ3llYXInKSApO1xuXG5cdFx0XHR5ZWFycy5wdXNoKCByZW5kZXJlciggcHJvcHMsIHllYXIsIHNlbGVjdGVkRGF0ZSAmJiBzZWxlY3RlZERhdGUuY2xvbmUoKSApKTtcblxuXHRcdFx0aWYgKCB5ZWFycy5sZW5ndGggPT09IDQgKSB7XG5cdFx0XHRcdHJvd3MucHVzaCggUmVhY3QuY3JlYXRlRWxlbWVudCgndHInLCB7IGtleTogaSB9LCB5ZWFycyApICk7XG5cdFx0XHRcdHllYXJzID0gW107XG5cdFx0XHR9XG5cblx0XHRcdHllYXIrKztcblx0XHRcdGkrKztcblx0XHR9XG5cblx0XHRyZXR1cm4gcm93cztcblx0fSxcblxuXHR1cGRhdGVTZWxlY3RlZFllYXI6IGZ1bmN0aW9uKCBldmVudCApIHtcblx0XHR0aGlzLnByb3BzLnVwZGF0ZVNlbGVjdGVkRGF0ZSggZXZlbnQgKTtcblx0fSxcblxuXHRyZW5kZXJZZWFyOiBmdW5jdGlvbiggcHJvcHMsIHllYXIgKSB7XG5cdFx0cmV0dXJuIFJlYWN0LmNyZWF0ZUVsZW1lbnQoJ3RkJywgIHByb3BzLCB5ZWFyICk7XG5cdH0sXG5cblx0YWx3YXlzVmFsaWREYXRlOiBmdW5jdGlvbigpIHtcblx0XHRyZXR1cm4gMTtcblx0fSxcblxuXHRoYW5kbGVDbGlja091dHNpZGU6IGZ1bmN0aW9uKCkge1xuXHRcdHRoaXMucHJvcHMuaGFuZGxlQ2xpY2tPdXRzaWRlKCk7XG5cdH1cbn0pKTtcblxubW9kdWxlLmV4cG9ydHMgPSBEYXRlVGltZVBpY2tlclllYXJzO1xuIiwiaW1wb3J0IHtjcmVhdGVFbGVtZW50LENvbXBvbmVudH1mcm9tJ3JlYWN0JztpbXBvcnQge2ZpbmRET01Ob2RlfWZyb20ncmVhY3QtZG9tJztmdW5jdGlvbiBfaW5oZXJpdHNMb29zZShzdWJDbGFzcywgc3VwZXJDbGFzcykge1xuICBzdWJDbGFzcy5wcm90b3R5cGUgPSBPYmplY3QuY3JlYXRlKHN1cGVyQ2xhc3MucHJvdG90eXBlKTtcbiAgc3ViQ2xhc3MucHJvdG90eXBlLmNvbnN0cnVjdG9yID0gc3ViQ2xhc3M7XG5cbiAgX3NldFByb3RvdHlwZU9mKHN1YkNsYXNzLCBzdXBlckNsYXNzKTtcbn1cblxuZnVuY3Rpb24gX3NldFByb3RvdHlwZU9mKG8sIHApIHtcbiAgX3NldFByb3RvdHlwZU9mID0gT2JqZWN0LnNldFByb3RvdHlwZU9mIHx8IGZ1bmN0aW9uIF9zZXRQcm90b3R5cGVPZihvLCBwKSB7XG4gICAgby5fX3Byb3RvX18gPSBwO1xuICAgIHJldHVybiBvO1xuICB9O1xuXG4gIHJldHVybiBfc2V0UHJvdG90eXBlT2YobywgcCk7XG59XG5cbmZ1bmN0aW9uIF9vYmplY3RXaXRob3V0UHJvcGVydGllc0xvb3NlKHNvdXJjZSwgZXhjbHVkZWQpIHtcbiAgaWYgKHNvdXJjZSA9PSBudWxsKSByZXR1cm4ge307XG4gIHZhciB0YXJnZXQgPSB7fTtcbiAgdmFyIHNvdXJjZUtleXMgPSBPYmplY3Qua2V5cyhzb3VyY2UpO1xuICB2YXIga2V5LCBpO1xuXG4gIGZvciAoaSA9IDA7IGkgPCBzb3VyY2VLZXlzLmxlbmd0aDsgaSsrKSB7XG4gICAga2V5ID0gc291cmNlS2V5c1tpXTtcbiAgICBpZiAoZXhjbHVkZWQuaW5kZXhPZihrZXkpID49IDApIGNvbnRpbnVlO1xuICAgIHRhcmdldFtrZXldID0gc291cmNlW2tleV07XG4gIH1cblxuICByZXR1cm4gdGFyZ2V0O1xufVxuXG5mdW5jdGlvbiBfYXNzZXJ0VGhpc0luaXRpYWxpemVkKHNlbGYpIHtcbiAgaWYgKHNlbGYgPT09IHZvaWQgMCkge1xuICAgIHRocm93IG5ldyBSZWZlcmVuY2VFcnJvcihcInRoaXMgaGFzbid0IGJlZW4gaW5pdGlhbGlzZWQgLSBzdXBlcigpIGhhc24ndCBiZWVuIGNhbGxlZFwiKTtcbiAgfVxuXG4gIHJldHVybiBzZWxmO1xufS8qKlxuICogQ2hlY2sgd2hldGhlciBzb21lIERPTSBub2RlIGlzIG91ciBDb21wb25lbnQncyBub2RlLlxuICovXG5mdW5jdGlvbiBpc05vZGVGb3VuZChjdXJyZW50LCBjb21wb25lbnROb2RlLCBpZ25vcmVDbGFzcykge1xuICBpZiAoY3VycmVudCA9PT0gY29tcG9uZW50Tm9kZSkge1xuICAgIHJldHVybiB0cnVlO1xuICB9IC8vIFNWRyA8dXNlLz4gZWxlbWVudHMgZG8gbm90IHRlY2huaWNhbGx5IHJlc2lkZSBpbiB0aGUgcmVuZGVyZWQgRE9NLCBzb1xuICAvLyB0aGV5IGRvIG5vdCBoYXZlIGNsYXNzTGlzdCBkaXJlY3RseSwgYnV0IHRoZXkgb2ZmZXIgYSBsaW5rIHRvIHRoZWlyXG4gIC8vIGNvcnJlc3BvbmRpbmcgZWxlbWVudCwgd2hpY2ggY2FuIGhhdmUgY2xhc3NMaXN0LiBUaGlzIGV4dHJhIGNoZWNrIGlzIGZvclxuICAvLyB0aGF0IGNhc2UuXG4gIC8vIFNlZTogaHR0cDovL3d3dy53My5vcmcvVFIvU1ZHMTEvc3RydWN0Lmh0bWwjSW50ZXJmYWNlU1ZHVXNlRWxlbWVudFxuICAvLyBEaXNjdXNzaW9uOiBodHRwczovL2dpdGh1Yi5jb20vUG9tYXgvcmVhY3Qtb25jbGlja291dHNpZGUvcHVsbC8xN1xuXG5cbiAgaWYgKGN1cnJlbnQuY29ycmVzcG9uZGluZ0VsZW1lbnQpIHtcbiAgICByZXR1cm4gY3VycmVudC5jb3JyZXNwb25kaW5nRWxlbWVudC5jbGFzc0xpc3QuY29udGFpbnMoaWdub3JlQ2xhc3MpO1xuICB9XG5cbiAgcmV0dXJuIGN1cnJlbnQuY2xhc3NMaXN0LmNvbnRhaW5zKGlnbm9yZUNsYXNzKTtcbn1cbi8qKlxuICogVHJ5IHRvIGZpbmQgb3VyIG5vZGUgaW4gYSBoaWVyYXJjaHkgb2Ygbm9kZXMsIHJldHVybmluZyB0aGUgZG9jdW1lbnRcbiAqIG5vZGUgYXMgaGlnaGVzdCBub2RlIGlmIG91ciBub2RlIGlzIG5vdCBmb3VuZCBpbiB0aGUgcGF0aCB1cC5cbiAqL1xuXG5mdW5jdGlvbiBmaW5kSGlnaGVzdChjdXJyZW50LCBjb21wb25lbnROb2RlLCBpZ25vcmVDbGFzcykge1xuICBpZiAoY3VycmVudCA9PT0gY29tcG9uZW50Tm9kZSkge1xuICAgIHJldHVybiB0cnVlO1xuICB9IC8vIElmIHNvdXJjZT1sb2NhbCB0aGVuIHRoaXMgZXZlbnQgY2FtZSBmcm9tICdzb21ld2hlcmUnXG4gIC8vIGluc2lkZSBhbmQgc2hvdWxkIGJlIGlnbm9yZWQuIFdlIGNvdWxkIGhhbmRsZSB0aGlzIHdpdGhcbiAgLy8gYSBsYXllcmVkIGFwcHJvYWNoLCB0b28sIGJ1dCB0aGF0IHJlcXVpcmVzIGdvaW5nIGJhY2sgdG9cbiAgLy8gdGhpbmtpbmcgaW4gdGVybXMgb2YgRG9tIG5vZGUgbmVzdGluZywgcnVubmluZyBjb3VudGVyXG4gIC8vIHRvIFJlYWN0J3MgJ3lvdSBzaG91bGRuJ3QgY2FyZSBhYm91dCB0aGUgRE9NJyBwaGlsb3NvcGh5LlxuICAvLyBBbHNvIGNvdmVyIHNoYWRvd1Jvb3Qgbm9kZSBieSBjaGVja2luZyBjdXJyZW50Lmhvc3RcblxuXG4gIHdoaWxlIChjdXJyZW50LnBhcmVudE5vZGUgfHwgY3VycmVudC5ob3N0KSB7XG4gICAgLy8gT25seSBjaGVjayBub3JtYWwgbm9kZSB3aXRob3V0IHNoYWRvd1Jvb3RcbiAgICBpZiAoY3VycmVudC5wYXJlbnROb2RlICYmIGlzTm9kZUZvdW5kKGN1cnJlbnQsIGNvbXBvbmVudE5vZGUsIGlnbm9yZUNsYXNzKSkge1xuICAgICAgcmV0dXJuIHRydWU7XG4gICAgfVxuXG4gICAgY3VycmVudCA9IGN1cnJlbnQucGFyZW50Tm9kZSB8fCBjdXJyZW50Lmhvc3Q7XG4gIH1cblxuICByZXR1cm4gY3VycmVudDtcbn1cbi8qKlxuICogQ2hlY2sgaWYgdGhlIGJyb3dzZXIgc2Nyb2xsYmFyIHdhcyBjbGlja2VkXG4gKi9cblxuZnVuY3Rpb24gY2xpY2tlZFNjcm9sbGJhcihldnQpIHtcbiAgcmV0dXJuIGRvY3VtZW50LmRvY3VtZW50RWxlbWVudC5jbGllbnRXaWR0aCA8PSBldnQuY2xpZW50WCB8fCBkb2N1bWVudC5kb2N1bWVudEVsZW1lbnQuY2xpZW50SGVpZ2h0IDw9IGV2dC5jbGllbnRZO1xufS8vIGlkZWFsbHkgd2lsbCBnZXQgcmVwbGFjZWQgd2l0aCBleHRlcm5hbCBkZXBcbi8vIHdoZW4gcmFmcmV4L2RldGVjdC1wYXNzaXZlLWV2ZW50cyM0IGFuZCByYWZyZXgvZGV0ZWN0LXBhc3NpdmUtZXZlbnRzIzUgZ2V0IG1lcmdlZCBpblxudmFyIHRlc3RQYXNzaXZlRXZlbnRTdXBwb3J0ID0gZnVuY3Rpb24gdGVzdFBhc3NpdmVFdmVudFN1cHBvcnQoKSB7XG4gIGlmICh0eXBlb2Ygd2luZG93ID09PSAndW5kZWZpbmVkJyB8fCB0eXBlb2Ygd2luZG93LmFkZEV2ZW50TGlzdGVuZXIgIT09ICdmdW5jdGlvbicpIHtcbiAgICByZXR1cm47XG4gIH1cblxuICB2YXIgcGFzc2l2ZSA9IGZhbHNlO1xuICB2YXIgb3B0aW9ucyA9IE9iamVjdC5kZWZpbmVQcm9wZXJ0eSh7fSwgJ3Bhc3NpdmUnLCB7XG4gICAgZ2V0OiBmdW5jdGlvbiBnZXQoKSB7XG4gICAgICBwYXNzaXZlID0gdHJ1ZTtcbiAgICB9XG4gIH0pO1xuXG4gIHZhciBub29wID0gZnVuY3Rpb24gbm9vcCgpIHt9O1xuXG4gIHdpbmRvdy5hZGRFdmVudExpc3RlbmVyKCd0ZXN0UGFzc2l2ZUV2ZW50U3VwcG9ydCcsIG5vb3AsIG9wdGlvbnMpO1xuICB3aW5kb3cucmVtb3ZlRXZlbnRMaXN0ZW5lcigndGVzdFBhc3NpdmVFdmVudFN1cHBvcnQnLCBub29wLCBvcHRpb25zKTtcbiAgcmV0dXJuIHBhc3NpdmU7XG59O2Z1bmN0aW9uIGF1dG9JbmMoc2VlZCkge1xuICBpZiAoc2VlZCA9PT0gdm9pZCAwKSB7XG4gICAgc2VlZCA9IDA7XG4gIH1cblxuICByZXR1cm4gZnVuY3Rpb24gKCkge1xuICAgIHJldHVybiArK3NlZWQ7XG4gIH07XG59XG5cbnZhciB1aWQgPSBhdXRvSW5jKCk7dmFyIHBhc3NpdmVFdmVudFN1cHBvcnQ7XG52YXIgaGFuZGxlcnNNYXAgPSB7fTtcbnZhciBlbmFibGVkSW5zdGFuY2VzID0ge307XG52YXIgdG91Y2hFdmVudHMgPSBbJ3RvdWNoc3RhcnQnLCAndG91Y2htb3ZlJ107XG52YXIgSUdOT1JFX0NMQVNTX05BTUUgPSAnaWdub3JlLXJlYWN0LW9uY2xpY2tvdXRzaWRlJztcbi8qKlxuICogT3B0aW9ucyBmb3IgYWRkRXZlbnRIYW5kbGVyIGFuZCByZW1vdmVFdmVudEhhbmRsZXJcbiAqL1xuXG5mdW5jdGlvbiBnZXRFdmVudEhhbmRsZXJPcHRpb25zKGluc3RhbmNlLCBldmVudE5hbWUpIHtcbiAgdmFyIGhhbmRsZXJPcHRpb25zID0ge307XG4gIHZhciBpc1RvdWNoRXZlbnQgPSB0b3VjaEV2ZW50cy5pbmRleE9mKGV2ZW50TmFtZSkgIT09IC0xO1xuXG4gIGlmIChpc1RvdWNoRXZlbnQgJiYgcGFzc2l2ZUV2ZW50U3VwcG9ydCkge1xuICAgIGhhbmRsZXJPcHRpb25zLnBhc3NpdmUgPSAhaW5zdGFuY2UucHJvcHMucHJldmVudERlZmF1bHQ7XG4gIH1cblxuICByZXR1cm4gaGFuZGxlck9wdGlvbnM7XG59XG4vKipcbiAqIFRoaXMgZnVuY3Rpb24gZ2VuZXJhdGVzIHRoZSBIT0MgZnVuY3Rpb24gdGhhdCB5b3UnbGwgdXNlXG4gKiBpbiBvcmRlciB0byBpbXBhcnQgb25PdXRzaWRlQ2xpY2sgbGlzdGVuaW5nIHRvIGFuXG4gKiBhcmJpdHJhcnkgY29tcG9uZW50LiBJdCBnZXRzIGNhbGxlZCBhdCB0aGUgZW5kIG9mIHRoZVxuICogYm9vdHN0cmFwcGluZyBjb2RlIHRvIHlpZWxkIGFuIGluc3RhbmNlIG9mIHRoZVxuICogb25DbGlja091dHNpZGVIT0MgZnVuY3Rpb24gZGVmaW5lZCBpbnNpZGUgc2V0dXBIT0MoKS5cbiAqL1xuXG5cbmZ1bmN0aW9uIG9uQ2xpY2tPdXRzaWRlSE9DKFdyYXBwZWRDb21wb25lbnQsIGNvbmZpZykge1xuICB2YXIgX2NsYXNzLCBfdGVtcDtcblxuICB2YXIgY29tcG9uZW50TmFtZSA9IFdyYXBwZWRDb21wb25lbnQuZGlzcGxheU5hbWUgfHwgV3JhcHBlZENvbXBvbmVudC5uYW1lIHx8ICdDb21wb25lbnQnO1xuICByZXR1cm4gX3RlbXAgPSBfY2xhc3MgPSAvKiNfX1BVUkVfXyovZnVuY3Rpb24gKF9Db21wb25lbnQpIHtcbiAgICBfaW5oZXJpdHNMb29zZShvbkNsaWNrT3V0c2lkZSwgX0NvbXBvbmVudCk7XG5cbiAgICBmdW5jdGlvbiBvbkNsaWNrT3V0c2lkZShwcm9wcykge1xuICAgICAgdmFyIF90aGlzO1xuXG4gICAgICBfdGhpcyA9IF9Db21wb25lbnQuY2FsbCh0aGlzLCBwcm9wcykgfHwgdGhpcztcblxuICAgICAgX3RoaXMuX19vdXRzaWRlQ2xpY2tIYW5kbGVyID0gZnVuY3Rpb24gKGV2ZW50KSB7XG4gICAgICAgIGlmICh0eXBlb2YgX3RoaXMuX19jbGlja091dHNpZGVIYW5kbGVyUHJvcCA9PT0gJ2Z1bmN0aW9uJykge1xuICAgICAgICAgIF90aGlzLl9fY2xpY2tPdXRzaWRlSGFuZGxlclByb3AoZXZlbnQpO1xuXG4gICAgICAgICAgcmV0dXJuO1xuICAgICAgICB9XG5cbiAgICAgICAgdmFyIGluc3RhbmNlID0gX3RoaXMuZ2V0SW5zdGFuY2UoKTtcblxuICAgICAgICBpZiAodHlwZW9mIGluc3RhbmNlLnByb3BzLmhhbmRsZUNsaWNrT3V0c2lkZSA9PT0gJ2Z1bmN0aW9uJykge1xuICAgICAgICAgIGluc3RhbmNlLnByb3BzLmhhbmRsZUNsaWNrT3V0c2lkZShldmVudCk7XG4gICAgICAgICAgcmV0dXJuO1xuICAgICAgICB9XG5cbiAgICAgICAgaWYgKHR5cGVvZiBpbnN0YW5jZS5oYW5kbGVDbGlja091dHNpZGUgPT09ICdmdW5jdGlvbicpIHtcbiAgICAgICAgICBpbnN0YW5jZS5oYW5kbGVDbGlja091dHNpZGUoZXZlbnQpO1xuICAgICAgICAgIHJldHVybjtcbiAgICAgICAgfVxuXG4gICAgICAgIHRocm93IG5ldyBFcnJvcihcIldyYXBwZWRDb21wb25lbnQ6IFwiICsgY29tcG9uZW50TmFtZSArIFwiIGxhY2tzIGEgaGFuZGxlQ2xpY2tPdXRzaWRlKGV2ZW50KSBmdW5jdGlvbiBmb3IgcHJvY2Vzc2luZyBvdXRzaWRlIGNsaWNrIGV2ZW50cy5cIik7XG4gICAgICB9O1xuXG4gICAgICBfdGhpcy5fX2dldENvbXBvbmVudE5vZGUgPSBmdW5jdGlvbiAoKSB7XG4gICAgICAgIHZhciBpbnN0YW5jZSA9IF90aGlzLmdldEluc3RhbmNlKCk7XG5cbiAgICAgICAgaWYgKGNvbmZpZyAmJiB0eXBlb2YgY29uZmlnLnNldENsaWNrT3V0c2lkZVJlZiA9PT0gJ2Z1bmN0aW9uJykge1xuICAgICAgICAgIHJldHVybiBjb25maWcuc2V0Q2xpY2tPdXRzaWRlUmVmKCkoaW5zdGFuY2UpO1xuICAgICAgICB9XG5cbiAgICAgICAgaWYgKHR5cGVvZiBpbnN0YW5jZS5zZXRDbGlja091dHNpZGVSZWYgPT09ICdmdW5jdGlvbicpIHtcbiAgICAgICAgICByZXR1cm4gaW5zdGFuY2Uuc2V0Q2xpY2tPdXRzaWRlUmVmKCk7XG4gICAgICAgIH1cblxuICAgICAgICByZXR1cm4gZmluZERPTU5vZGUoaW5zdGFuY2UpO1xuICAgICAgfTtcblxuICAgICAgX3RoaXMuZW5hYmxlT25DbGlja091dHNpZGUgPSBmdW5jdGlvbiAoKSB7XG4gICAgICAgIGlmICh0eXBlb2YgZG9jdW1lbnQgPT09ICd1bmRlZmluZWQnIHx8IGVuYWJsZWRJbnN0YW5jZXNbX3RoaXMuX3VpZF0pIHtcbiAgICAgICAgICByZXR1cm47XG4gICAgICAgIH1cblxuICAgICAgICBpZiAodHlwZW9mIHBhc3NpdmVFdmVudFN1cHBvcnQgPT09ICd1bmRlZmluZWQnKSB7XG4gICAgICAgICAgcGFzc2l2ZUV2ZW50U3VwcG9ydCA9IHRlc3RQYXNzaXZlRXZlbnRTdXBwb3J0KCk7XG4gICAgICAgIH1cblxuICAgICAgICBlbmFibGVkSW5zdGFuY2VzW190aGlzLl91aWRdID0gdHJ1ZTtcbiAgICAgICAgdmFyIGV2ZW50cyA9IF90aGlzLnByb3BzLmV2ZW50VHlwZXM7XG5cbiAgICAgICAgaWYgKCFldmVudHMuZm9yRWFjaCkge1xuICAgICAgICAgIGV2ZW50cyA9IFtldmVudHNdO1xuICAgICAgICB9XG5cbiAgICAgICAgaGFuZGxlcnNNYXBbX3RoaXMuX3VpZF0gPSBmdW5jdGlvbiAoZXZlbnQpIHtcbiAgICAgICAgICBpZiAoX3RoaXMuY29tcG9uZW50Tm9kZSA9PT0gbnVsbCkgcmV0dXJuO1xuXG4gICAgICAgICAgaWYgKF90aGlzLnByb3BzLnByZXZlbnREZWZhdWx0KSB7XG4gICAgICAgICAgICBldmVudC5wcmV2ZW50RGVmYXVsdCgpO1xuICAgICAgICAgIH1cblxuICAgICAgICAgIGlmIChfdGhpcy5wcm9wcy5zdG9wUHJvcGFnYXRpb24pIHtcbiAgICAgICAgICAgIGV2ZW50LnN0b3BQcm9wYWdhdGlvbigpO1xuICAgICAgICAgIH1cblxuICAgICAgICAgIGlmIChfdGhpcy5wcm9wcy5leGNsdWRlU2Nyb2xsYmFyICYmIGNsaWNrZWRTY3JvbGxiYXIoZXZlbnQpKSByZXR1cm47XG4gICAgICAgICAgdmFyIGN1cnJlbnQgPSBldmVudC5jb21wb3NlZCAmJiBldmVudC5jb21wb3NlZFBhdGggJiYgZXZlbnQuY29tcG9zZWRQYXRoKCkuc2hpZnQoKSB8fCBldmVudC50YXJnZXQ7XG5cbiAgICAgICAgICBpZiAoZmluZEhpZ2hlc3QoY3VycmVudCwgX3RoaXMuY29tcG9uZW50Tm9kZSwgX3RoaXMucHJvcHMub3V0c2lkZUNsaWNrSWdub3JlQ2xhc3MpICE9PSBkb2N1bWVudCkge1xuICAgICAgICAgICAgcmV0dXJuO1xuICAgICAgICAgIH1cblxuICAgICAgICAgIF90aGlzLl9fb3V0c2lkZUNsaWNrSGFuZGxlcihldmVudCk7XG4gICAgICAgIH07XG5cbiAgICAgICAgZXZlbnRzLmZvckVhY2goZnVuY3Rpb24gKGV2ZW50TmFtZSkge1xuICAgICAgICAgIGRvY3VtZW50LmFkZEV2ZW50TGlzdGVuZXIoZXZlbnROYW1lLCBoYW5kbGVyc01hcFtfdGhpcy5fdWlkXSwgZ2V0RXZlbnRIYW5kbGVyT3B0aW9ucyhfYXNzZXJ0VGhpc0luaXRpYWxpemVkKF90aGlzKSwgZXZlbnROYW1lKSk7XG4gICAgICAgIH0pO1xuICAgICAgfTtcblxuICAgICAgX3RoaXMuZGlzYWJsZU9uQ2xpY2tPdXRzaWRlID0gZnVuY3Rpb24gKCkge1xuICAgICAgICBkZWxldGUgZW5hYmxlZEluc3RhbmNlc1tfdGhpcy5fdWlkXTtcbiAgICAgICAgdmFyIGZuID0gaGFuZGxlcnNNYXBbX3RoaXMuX3VpZF07XG5cbiAgICAgICAgaWYgKGZuICYmIHR5cGVvZiBkb2N1bWVudCAhPT0gJ3VuZGVmaW5lZCcpIHtcbiAgICAgICAgICB2YXIgZXZlbnRzID0gX3RoaXMucHJvcHMuZXZlbnRUeXBlcztcblxuICAgICAgICAgIGlmICghZXZlbnRzLmZvckVhY2gpIHtcbiAgICAgICAgICAgIGV2ZW50cyA9IFtldmVudHNdO1xuICAgICAgICAgIH1cblxuICAgICAgICAgIGV2ZW50cy5mb3JFYWNoKGZ1bmN0aW9uIChldmVudE5hbWUpIHtcbiAgICAgICAgICAgIHJldHVybiBkb2N1bWVudC5yZW1vdmVFdmVudExpc3RlbmVyKGV2ZW50TmFtZSwgZm4sIGdldEV2ZW50SGFuZGxlck9wdGlvbnMoX2Fzc2VydFRoaXNJbml0aWFsaXplZChfdGhpcyksIGV2ZW50TmFtZSkpO1xuICAgICAgICAgIH0pO1xuICAgICAgICAgIGRlbGV0ZSBoYW5kbGVyc01hcFtfdGhpcy5fdWlkXTtcbiAgICAgICAgfVxuICAgICAgfTtcblxuICAgICAgX3RoaXMuZ2V0UmVmID0gZnVuY3Rpb24gKHJlZikge1xuICAgICAgICByZXR1cm4gX3RoaXMuaW5zdGFuY2VSZWYgPSByZWY7XG4gICAgICB9O1xuXG4gICAgICBfdGhpcy5fdWlkID0gdWlkKCk7XG4gICAgICByZXR1cm4gX3RoaXM7XG4gICAgfVxuICAgIC8qKlxuICAgICAqIEFjY2VzcyB0aGUgV3JhcHBlZENvbXBvbmVudCdzIGluc3RhbmNlLlxuICAgICAqL1xuXG5cbiAgICB2YXIgX3Byb3RvID0gb25DbGlja091dHNpZGUucHJvdG90eXBlO1xuXG4gICAgX3Byb3RvLmdldEluc3RhbmNlID0gZnVuY3Rpb24gZ2V0SW5zdGFuY2UoKSB7XG4gICAgICBpZiAoV3JhcHBlZENvbXBvbmVudC5wcm90b3R5cGUgJiYgIVdyYXBwZWRDb21wb25lbnQucHJvdG90eXBlLmlzUmVhY3RDb21wb25lbnQpIHtcbiAgICAgICAgcmV0dXJuIHRoaXM7XG4gICAgICB9XG5cbiAgICAgIHZhciByZWYgPSB0aGlzLmluc3RhbmNlUmVmO1xuICAgICAgcmV0dXJuIHJlZi5nZXRJbnN0YW5jZSA/IHJlZi5nZXRJbnN0YW5jZSgpIDogcmVmO1xuICAgIH07XG5cbiAgICAvKipcbiAgICAgKiBBZGQgY2xpY2sgbGlzdGVuZXJzIHRvIHRoZSBjdXJyZW50IGRvY3VtZW50LFxuICAgICAqIGxpbmtlZCB0byB0aGlzIGNvbXBvbmVudCdzIHN0YXRlLlxuICAgICAqL1xuICAgIF9wcm90by5jb21wb25lbnREaWRNb3VudCA9IGZ1bmN0aW9uIGNvbXBvbmVudERpZE1vdW50KCkge1xuICAgICAgLy8gSWYgd2UgYXJlIGluIGFuIGVudmlyb25tZW50IHdpdGhvdXQgYSBET00gc3VjaFxuICAgICAgLy8gYXMgc2hhbGxvdyByZW5kZXJpbmcgb3Igc25hcHNob3RzIHRoZW4gd2UgZXhpdFxuICAgICAgLy8gZWFybHkgdG8gcHJldmVudCBhbnkgdW5oYW5kbGVkIGVycm9ycyBiZWluZyB0aHJvd24uXG4gICAgICBpZiAodHlwZW9mIGRvY3VtZW50ID09PSAndW5kZWZpbmVkJyB8fCAhZG9jdW1lbnQuY3JlYXRlRWxlbWVudCkge1xuICAgICAgICByZXR1cm47XG4gICAgICB9XG5cbiAgICAgIHZhciBpbnN0YW5jZSA9IHRoaXMuZ2V0SW5zdGFuY2UoKTtcblxuICAgICAgaWYgKGNvbmZpZyAmJiB0eXBlb2YgY29uZmlnLmhhbmRsZUNsaWNrT3V0c2lkZSA9PT0gJ2Z1bmN0aW9uJykge1xuICAgICAgICB0aGlzLl9fY2xpY2tPdXRzaWRlSGFuZGxlclByb3AgPSBjb25maWcuaGFuZGxlQ2xpY2tPdXRzaWRlKGluc3RhbmNlKTtcblxuICAgICAgICBpZiAodHlwZW9mIHRoaXMuX19jbGlja091dHNpZGVIYW5kbGVyUHJvcCAhPT0gJ2Z1bmN0aW9uJykge1xuICAgICAgICAgIHRocm93IG5ldyBFcnJvcihcIldyYXBwZWRDb21wb25lbnQ6IFwiICsgY29tcG9uZW50TmFtZSArIFwiIGxhY2tzIGEgZnVuY3Rpb24gZm9yIHByb2Nlc3Npbmcgb3V0c2lkZSBjbGljayBldmVudHMgc3BlY2lmaWVkIGJ5IHRoZSBoYW5kbGVDbGlja091dHNpZGUgY29uZmlnIG9wdGlvbi5cIik7XG4gICAgICAgIH1cbiAgICAgIH1cblxuICAgICAgdGhpcy5jb21wb25lbnROb2RlID0gdGhpcy5fX2dldENvbXBvbmVudE5vZGUoKTsgLy8gcmV0dXJuIGVhcmx5IHNvIHdlIGRvbnQgaW5pdGlhdGUgb25DbGlja091dHNpZGVcblxuICAgICAgaWYgKHRoaXMucHJvcHMuZGlzYWJsZU9uQ2xpY2tPdXRzaWRlKSByZXR1cm47XG4gICAgICB0aGlzLmVuYWJsZU9uQ2xpY2tPdXRzaWRlKCk7XG4gICAgfTtcblxuICAgIF9wcm90by5jb21wb25lbnREaWRVcGRhdGUgPSBmdW5jdGlvbiBjb21wb25lbnREaWRVcGRhdGUoKSB7XG4gICAgICB0aGlzLmNvbXBvbmVudE5vZGUgPSB0aGlzLl9fZ2V0Q29tcG9uZW50Tm9kZSgpO1xuICAgIH1cbiAgICAvKipcbiAgICAgKiBSZW1vdmUgYWxsIGRvY3VtZW50J3MgZXZlbnQgbGlzdGVuZXJzIGZvciB0aGlzIGNvbXBvbmVudFxuICAgICAqL1xuICAgIDtcblxuICAgIF9wcm90by5jb21wb25lbnRXaWxsVW5tb3VudCA9IGZ1bmN0aW9uIGNvbXBvbmVudFdpbGxVbm1vdW50KCkge1xuICAgICAgdGhpcy5kaXNhYmxlT25DbGlja091dHNpZGUoKTtcbiAgICB9XG4gICAgLyoqXG4gICAgICogQ2FuIGJlIGNhbGxlZCB0byBleHBsaWNpdGx5IGVuYWJsZSBldmVudCBsaXN0ZW5pbmdcbiAgICAgKiBmb3IgY2xpY2tzIGFuZCB0b3VjaGVzIG91dHNpZGUgb2YgdGhpcyBlbGVtZW50LlxuICAgICAqL1xuICAgIDtcblxuICAgIC8qKlxuICAgICAqIFBhc3MtdGhyb3VnaCByZW5kZXJcbiAgICAgKi9cbiAgICBfcHJvdG8ucmVuZGVyID0gZnVuY3Rpb24gcmVuZGVyKCkge1xuICAgICAgLy8gZXNsaW50LWRpc2FibGUtbmV4dC1saW5lIG5vLXVudXNlZC12YXJzXG4gICAgICB2YXIgX3RoaXMkcHJvcHMgPSB0aGlzLnByb3BzO1xuICAgICAgICAgIF90aGlzJHByb3BzLmV4Y2x1ZGVTY3JvbGxiYXI7XG4gICAgICAgICAgdmFyIHByb3BzID0gX29iamVjdFdpdGhvdXRQcm9wZXJ0aWVzTG9vc2UoX3RoaXMkcHJvcHMsIFtcImV4Y2x1ZGVTY3JvbGxiYXJcIl0pO1xuXG4gICAgICBpZiAoV3JhcHBlZENvbXBvbmVudC5wcm90b3R5cGUgJiYgV3JhcHBlZENvbXBvbmVudC5wcm90b3R5cGUuaXNSZWFjdENvbXBvbmVudCkge1xuICAgICAgICBwcm9wcy5yZWYgPSB0aGlzLmdldFJlZjtcbiAgICAgIH0gZWxzZSB7XG4gICAgICAgIHByb3BzLndyYXBwZWRSZWYgPSB0aGlzLmdldFJlZjtcbiAgICAgIH1cblxuICAgICAgcHJvcHMuZGlzYWJsZU9uQ2xpY2tPdXRzaWRlID0gdGhpcy5kaXNhYmxlT25DbGlja091dHNpZGU7XG4gICAgICBwcm9wcy5lbmFibGVPbkNsaWNrT3V0c2lkZSA9IHRoaXMuZW5hYmxlT25DbGlja091dHNpZGU7XG4gICAgICByZXR1cm4gY3JlYXRlRWxlbWVudChXcmFwcGVkQ29tcG9uZW50LCBwcm9wcyk7XG4gICAgfTtcblxuICAgIHJldHVybiBvbkNsaWNrT3V0c2lkZTtcbiAgfShDb21wb25lbnQpLCBfY2xhc3MuZGlzcGxheU5hbWUgPSBcIk9uQ2xpY2tPdXRzaWRlKFwiICsgY29tcG9uZW50TmFtZSArIFwiKVwiLCBfY2xhc3MuZGVmYXVsdFByb3BzID0ge1xuICAgIGV2ZW50VHlwZXM6IFsnbW91c2Vkb3duJywgJ3RvdWNoc3RhcnQnXSxcbiAgICBleGNsdWRlU2Nyb2xsYmFyOiBjb25maWcgJiYgY29uZmlnLmV4Y2x1ZGVTY3JvbGxiYXIgfHwgZmFsc2UsXG4gICAgb3V0c2lkZUNsaWNrSWdub3JlQ2xhc3M6IElHTk9SRV9DTEFTU19OQU1FLFxuICAgIHByZXZlbnREZWZhdWx0OiBmYWxzZSxcbiAgICBzdG9wUHJvcGFnYXRpb246IGZhbHNlXG4gIH0sIF9jbGFzcy5nZXRDbGFzcyA9IGZ1bmN0aW9uICgpIHtcbiAgICByZXR1cm4gV3JhcHBlZENvbXBvbmVudC5nZXRDbGFzcyA/IFdyYXBwZWRDb21wb25lbnQuZ2V0Q2xhc3MoKSA6IFdyYXBwZWRDb21wb25lbnQ7XG4gIH0sIF90ZW1wO1xufWV4cG9ydCBkZWZhdWx0IG9uQ2xpY2tPdXRzaWRlSE9DO2V4cG9ydHtJR05PUkVfQ0xBU1NfTkFNRX07IiwiZnVuY3Rpb24gaXNBYnNvbHV0ZShwYXRobmFtZSkge1xuICByZXR1cm4gcGF0aG5hbWUuY2hhckF0KDApID09PSAnLyc7XG59XG5cbi8vIEFib3V0IDEuNXggZmFzdGVyIHRoYW4gdGhlIHR3by1hcmcgdmVyc2lvbiBvZiBBcnJheSNzcGxpY2UoKVxuZnVuY3Rpb24gc3BsaWNlT25lKGxpc3QsIGluZGV4KSB7XG4gIGZvciAodmFyIGkgPSBpbmRleCwgayA9IGkgKyAxLCBuID0gbGlzdC5sZW5ndGg7IGsgPCBuOyBpICs9IDEsIGsgKz0gMSkge1xuICAgIGxpc3RbaV0gPSBsaXN0W2tdO1xuICB9XG5cbiAgbGlzdC5wb3AoKTtcbn1cblxuLy8gVGhpcyBpbXBsZW1lbnRhdGlvbiBpcyBiYXNlZCBoZWF2aWx5IG9uIG5vZGUncyB1cmwucGFyc2VcbmZ1bmN0aW9uIHJlc29sdmVQYXRobmFtZSh0bykge1xuICB2YXIgZnJvbSA9IGFyZ3VtZW50cy5sZW5ndGggPiAxICYmIGFyZ3VtZW50c1sxXSAhPT0gdW5kZWZpbmVkID8gYXJndW1lbnRzWzFdIDogJyc7XG5cbiAgdmFyIHRvUGFydHMgPSB0byAmJiB0by5zcGxpdCgnLycpIHx8IFtdO1xuICB2YXIgZnJvbVBhcnRzID0gZnJvbSAmJiBmcm9tLnNwbGl0KCcvJykgfHwgW107XG5cbiAgdmFyIGlzVG9BYnMgPSB0byAmJiBpc0Fic29sdXRlKHRvKTtcbiAgdmFyIGlzRnJvbUFicyA9IGZyb20gJiYgaXNBYnNvbHV0ZShmcm9tKTtcbiAgdmFyIG11c3RFbmRBYnMgPSBpc1RvQWJzIHx8IGlzRnJvbUFicztcblxuICBpZiAodG8gJiYgaXNBYnNvbHV0ZSh0bykpIHtcbiAgICAvLyB0byBpcyBhYnNvbHV0ZVxuICAgIGZyb21QYXJ0cyA9IHRvUGFydHM7XG4gIH0gZWxzZSBpZiAodG9QYXJ0cy5sZW5ndGgpIHtcbiAgICAvLyB0byBpcyByZWxhdGl2ZSwgZHJvcCB0aGUgZmlsZW5hbWVcbiAgICBmcm9tUGFydHMucG9wKCk7XG4gICAgZnJvbVBhcnRzID0gZnJvbVBhcnRzLmNvbmNhdCh0b1BhcnRzKTtcbiAgfVxuXG4gIGlmICghZnJvbVBhcnRzLmxlbmd0aCkgcmV0dXJuICcvJztcblxuICB2YXIgaGFzVHJhaWxpbmdTbGFzaCA9IHZvaWQgMDtcbiAgaWYgKGZyb21QYXJ0cy5sZW5ndGgpIHtcbiAgICB2YXIgbGFzdCA9IGZyb21QYXJ0c1tmcm9tUGFydHMubGVuZ3RoIC0gMV07XG4gICAgaGFzVHJhaWxpbmdTbGFzaCA9IGxhc3QgPT09ICcuJyB8fCBsYXN0ID09PSAnLi4nIHx8IGxhc3QgPT09ICcnO1xuICB9IGVsc2Uge1xuICAgIGhhc1RyYWlsaW5nU2xhc2ggPSBmYWxzZTtcbiAgfVxuXG4gIHZhciB1cCA9IDA7XG4gIGZvciAodmFyIGkgPSBmcm9tUGFydHMubGVuZ3RoOyBpID49IDA7IGktLSkge1xuICAgIHZhciBwYXJ0ID0gZnJvbVBhcnRzW2ldO1xuXG4gICAgaWYgKHBhcnQgPT09ICcuJykge1xuICAgICAgc3BsaWNlT25lKGZyb21QYXJ0cywgaSk7XG4gICAgfSBlbHNlIGlmIChwYXJ0ID09PSAnLi4nKSB7XG4gICAgICBzcGxpY2VPbmUoZnJvbVBhcnRzLCBpKTtcbiAgICAgIHVwKys7XG4gICAgfSBlbHNlIGlmICh1cCkge1xuICAgICAgc3BsaWNlT25lKGZyb21QYXJ0cywgaSk7XG4gICAgICB1cC0tO1xuICAgIH1cbiAgfVxuXG4gIGlmICghbXVzdEVuZEFicykgZm9yICg7IHVwLS07IHVwKSB7XG4gICAgZnJvbVBhcnRzLnVuc2hpZnQoJy4uJyk7XG4gIH1pZiAobXVzdEVuZEFicyAmJiBmcm9tUGFydHNbMF0gIT09ICcnICYmICghZnJvbVBhcnRzWzBdIHx8ICFpc0Fic29sdXRlKGZyb21QYXJ0c1swXSkpKSBmcm9tUGFydHMudW5zaGlmdCgnJyk7XG5cbiAgdmFyIHJlc3VsdCA9IGZyb21QYXJ0cy5qb2luKCcvJyk7XG5cbiAgaWYgKGhhc1RyYWlsaW5nU2xhc2ggJiYgcmVzdWx0LnN1YnN0cigtMSkgIT09ICcvJykgcmVzdWx0ICs9ICcvJztcblxuICByZXR1cm4gcmVzdWx0O1xufVxuXG5leHBvcnQgZGVmYXVsdCByZXNvbHZlUGF0aG5hbWU7IiwiJ3VzZSBzdHJpY3QnO1xubW9kdWxlLmV4cG9ydHMgPSBmdW5jdGlvbiAoc3RyKSB7XG5cdHJldHVybiBlbmNvZGVVUklDb21wb25lbnQoc3RyKS5yZXBsYWNlKC9bIScoKSpdL2csIGZ1bmN0aW9uIChjKSB7XG5cdFx0cmV0dXJuICclJyArIGMuY2hhckNvZGVBdCgwKS50b1N0cmluZygxNikudG9VcHBlckNhc2UoKTtcblx0fSk7XG59O1xuIiwiLypcblx0TUlUIExpY2Vuc2UgaHR0cDovL3d3dy5vcGVuc291cmNlLm9yZy9saWNlbnNlcy9taXQtbGljZW5zZS5waHBcblx0QXV0aG9yIFRvYmlhcyBLb3BwZXJzIEBzb2tyYVxuKi9cblxudmFyIHN0eWxlc0luRG9tID0ge307XG5cbnZhclx0bWVtb2l6ZSA9IGZ1bmN0aW9uIChmbikge1xuXHR2YXIgbWVtbztcblxuXHRyZXR1cm4gZnVuY3Rpb24gKCkge1xuXHRcdGlmICh0eXBlb2YgbWVtbyA9PT0gXCJ1bmRlZmluZWRcIikgbWVtbyA9IGZuLmFwcGx5KHRoaXMsIGFyZ3VtZW50cyk7XG5cdFx0cmV0dXJuIG1lbW87XG5cdH07XG59O1xuXG52YXIgaXNPbGRJRSA9IG1lbW9pemUoZnVuY3Rpb24gKCkge1xuXHQvLyBUZXN0IGZvciBJRSA8PSA5IGFzIHByb3Bvc2VkIGJ5IEJyb3dzZXJoYWNrc1xuXHQvLyBAc2VlIGh0dHA6Ly9icm93c2VyaGFja3MuY29tLyNoYWNrLWU3MWQ4NjkyZjY1MzM0MTczZmVlNzE1YzIyMmNiODA1XG5cdC8vIFRlc3RzIGZvciBleGlzdGVuY2Ugb2Ygc3RhbmRhcmQgZ2xvYmFscyBpcyB0byBhbGxvdyBzdHlsZS1sb2FkZXJcblx0Ly8gdG8gb3BlcmF0ZSBjb3JyZWN0bHkgaW50byBub24tc3RhbmRhcmQgZW52aXJvbm1lbnRzXG5cdC8vIEBzZWUgaHR0cHM6Ly9naXRodWIuY29tL3dlYnBhY2stY29udHJpYi9zdHlsZS1sb2FkZXIvaXNzdWVzLzE3N1xuXHRyZXR1cm4gd2luZG93ICYmIGRvY3VtZW50ICYmIGRvY3VtZW50LmFsbCAmJiAhd2luZG93LmF0b2I7XG59KTtcblxudmFyIGdldFRhcmdldCA9IGZ1bmN0aW9uICh0YXJnZXQpIHtcbiAgcmV0dXJuIGRvY3VtZW50LnF1ZXJ5U2VsZWN0b3IodGFyZ2V0KTtcbn07XG5cbnZhciBnZXRFbGVtZW50ID0gKGZ1bmN0aW9uIChmbikge1xuXHR2YXIgbWVtbyA9IHt9O1xuXG5cdHJldHVybiBmdW5jdGlvbih0YXJnZXQpIHtcbiAgICAgICAgICAgICAgICAvLyBJZiBwYXNzaW5nIGZ1bmN0aW9uIGluIG9wdGlvbnMsIHRoZW4gdXNlIGl0IGZvciByZXNvbHZlIFwiaGVhZFwiIGVsZW1lbnQuXG4gICAgICAgICAgICAgICAgLy8gVXNlZnVsIGZvciBTaGFkb3cgUm9vdCBzdHlsZSBpLmVcbiAgICAgICAgICAgICAgICAvLyB7XG4gICAgICAgICAgICAgICAgLy8gICBpbnNlcnRJbnRvOiBmdW5jdGlvbiAoKSB7IHJldHVybiBkb2N1bWVudC5xdWVyeVNlbGVjdG9yKFwiI2Zvb1wiKS5zaGFkb3dSb290IH1cbiAgICAgICAgICAgICAgICAvLyB9XG4gICAgICAgICAgICAgICAgaWYgKHR5cGVvZiB0YXJnZXQgPT09ICdmdW5jdGlvbicpIHtcbiAgICAgICAgICAgICAgICAgICAgICAgIHJldHVybiB0YXJnZXQoKTtcbiAgICAgICAgICAgICAgICB9XG4gICAgICAgICAgICAgICAgaWYgKHR5cGVvZiBtZW1vW3RhcmdldF0gPT09IFwidW5kZWZpbmVkXCIpIHtcblx0XHRcdHZhciBzdHlsZVRhcmdldCA9IGdldFRhcmdldC5jYWxsKHRoaXMsIHRhcmdldCk7XG5cdFx0XHQvLyBTcGVjaWFsIGNhc2UgdG8gcmV0dXJuIGhlYWQgb2YgaWZyYW1lIGluc3RlYWQgb2YgaWZyYW1lIGl0c2VsZlxuXHRcdFx0aWYgKHdpbmRvdy5IVE1MSUZyYW1lRWxlbWVudCAmJiBzdHlsZVRhcmdldCBpbnN0YW5jZW9mIHdpbmRvdy5IVE1MSUZyYW1lRWxlbWVudCkge1xuXHRcdFx0XHR0cnkge1xuXHRcdFx0XHRcdC8vIFRoaXMgd2lsbCB0aHJvdyBhbiBleGNlcHRpb24gaWYgYWNjZXNzIHRvIGlmcmFtZSBpcyBibG9ja2VkXG5cdFx0XHRcdFx0Ly8gZHVlIHRvIGNyb3NzLW9yaWdpbiByZXN0cmljdGlvbnNcblx0XHRcdFx0XHRzdHlsZVRhcmdldCA9IHN0eWxlVGFyZ2V0LmNvbnRlbnREb2N1bWVudC5oZWFkO1xuXHRcdFx0XHR9IGNhdGNoKGUpIHtcblx0XHRcdFx0XHRzdHlsZVRhcmdldCA9IG51bGw7XG5cdFx0XHRcdH1cblx0XHRcdH1cblx0XHRcdG1lbW9bdGFyZ2V0XSA9IHN0eWxlVGFyZ2V0O1xuXHRcdH1cblx0XHRyZXR1cm4gbWVtb1t0YXJnZXRdXG5cdH07XG59KSgpO1xuXG52YXIgc2luZ2xldG9uID0gbnVsbDtcbnZhclx0c2luZ2xldG9uQ291bnRlciA9IDA7XG52YXJcdHN0eWxlc0luc2VydGVkQXRUb3AgPSBbXTtcblxudmFyXHRmaXhVcmxzID0gcmVxdWlyZShcIi4vdXJsc1wiKTtcblxubW9kdWxlLmV4cG9ydHMgPSBmdW5jdGlvbihsaXN0LCBvcHRpb25zKSB7XG5cdGlmICh0eXBlb2YgREVCVUcgIT09IFwidW5kZWZpbmVkXCIgJiYgREVCVUcpIHtcblx0XHRpZiAodHlwZW9mIGRvY3VtZW50ICE9PSBcIm9iamVjdFwiKSB0aHJvdyBuZXcgRXJyb3IoXCJUaGUgc3R5bGUtbG9hZGVyIGNhbm5vdCBiZSB1c2VkIGluIGEgbm9uLWJyb3dzZXIgZW52aXJvbm1lbnRcIik7XG5cdH1cblxuXHRvcHRpb25zID0gb3B0aW9ucyB8fCB7fTtcblxuXHRvcHRpb25zLmF0dHJzID0gdHlwZW9mIG9wdGlvbnMuYXR0cnMgPT09IFwib2JqZWN0XCIgPyBvcHRpb25zLmF0dHJzIDoge307XG5cblx0Ly8gRm9yY2Ugc2luZ2xlLXRhZyBzb2x1dGlvbiBvbiBJRTYtOSwgd2hpY2ggaGFzIGEgaGFyZCBsaW1pdCBvbiB0aGUgIyBvZiA8c3R5bGU+XG5cdC8vIHRhZ3MgaXQgd2lsbCBhbGxvdyBvbiBhIHBhZ2Vcblx0aWYgKCFvcHRpb25zLnNpbmdsZXRvbiAmJiB0eXBlb2Ygb3B0aW9ucy5zaW5nbGV0b24gIT09IFwiYm9vbGVhblwiKSBvcHRpb25zLnNpbmdsZXRvbiA9IGlzT2xkSUUoKTtcblxuXHQvLyBCeSBkZWZhdWx0LCBhZGQgPHN0eWxlPiB0YWdzIHRvIHRoZSA8aGVhZD4gZWxlbWVudFxuICAgICAgICBpZiAoIW9wdGlvbnMuaW5zZXJ0SW50bykgb3B0aW9ucy5pbnNlcnRJbnRvID0gXCJoZWFkXCI7XG5cblx0Ly8gQnkgZGVmYXVsdCwgYWRkIDxzdHlsZT4gdGFncyB0byB0aGUgYm90dG9tIG9mIHRoZSB0YXJnZXRcblx0aWYgKCFvcHRpb25zLmluc2VydEF0KSBvcHRpb25zLmluc2VydEF0ID0gXCJib3R0b21cIjtcblxuXHR2YXIgc3R5bGVzID0gbGlzdFRvU3R5bGVzKGxpc3QsIG9wdGlvbnMpO1xuXG5cdGFkZFN0eWxlc1RvRG9tKHN0eWxlcywgb3B0aW9ucyk7XG5cblx0cmV0dXJuIGZ1bmN0aW9uIHVwZGF0ZSAobmV3TGlzdCkge1xuXHRcdHZhciBtYXlSZW1vdmUgPSBbXTtcblxuXHRcdGZvciAodmFyIGkgPSAwOyBpIDwgc3R5bGVzLmxlbmd0aDsgaSsrKSB7XG5cdFx0XHR2YXIgaXRlbSA9IHN0eWxlc1tpXTtcblx0XHRcdHZhciBkb21TdHlsZSA9IHN0eWxlc0luRG9tW2l0ZW0uaWRdO1xuXG5cdFx0XHRkb21TdHlsZS5yZWZzLS07XG5cdFx0XHRtYXlSZW1vdmUucHVzaChkb21TdHlsZSk7XG5cdFx0fVxuXG5cdFx0aWYobmV3TGlzdCkge1xuXHRcdFx0dmFyIG5ld1N0eWxlcyA9IGxpc3RUb1N0eWxlcyhuZXdMaXN0LCBvcHRpb25zKTtcblx0XHRcdGFkZFN0eWxlc1RvRG9tKG5ld1N0eWxlcywgb3B0aW9ucyk7XG5cdFx0fVxuXG5cdFx0Zm9yICh2YXIgaSA9IDA7IGkgPCBtYXlSZW1vdmUubGVuZ3RoOyBpKyspIHtcblx0XHRcdHZhciBkb21TdHlsZSA9IG1heVJlbW92ZVtpXTtcblxuXHRcdFx0aWYoZG9tU3R5bGUucmVmcyA9PT0gMCkge1xuXHRcdFx0XHRmb3IgKHZhciBqID0gMDsgaiA8IGRvbVN0eWxlLnBhcnRzLmxlbmd0aDsgaisrKSBkb21TdHlsZS5wYXJ0c1tqXSgpO1xuXG5cdFx0XHRcdGRlbGV0ZSBzdHlsZXNJbkRvbVtkb21TdHlsZS5pZF07XG5cdFx0XHR9XG5cdFx0fVxuXHR9O1xufTtcblxuZnVuY3Rpb24gYWRkU3R5bGVzVG9Eb20gKHN0eWxlcywgb3B0aW9ucykge1xuXHRmb3IgKHZhciBpID0gMDsgaSA8IHN0eWxlcy5sZW5ndGg7IGkrKykge1xuXHRcdHZhciBpdGVtID0gc3R5bGVzW2ldO1xuXHRcdHZhciBkb21TdHlsZSA9IHN0eWxlc0luRG9tW2l0ZW0uaWRdO1xuXG5cdFx0aWYoZG9tU3R5bGUpIHtcblx0XHRcdGRvbVN0eWxlLnJlZnMrKztcblxuXHRcdFx0Zm9yKHZhciBqID0gMDsgaiA8IGRvbVN0eWxlLnBhcnRzLmxlbmd0aDsgaisrKSB7XG5cdFx0XHRcdGRvbVN0eWxlLnBhcnRzW2pdKGl0ZW0ucGFydHNbal0pO1xuXHRcdFx0fVxuXG5cdFx0XHRmb3IoOyBqIDwgaXRlbS5wYXJ0cy5sZW5ndGg7IGorKykge1xuXHRcdFx0XHRkb21TdHlsZS5wYXJ0cy5wdXNoKGFkZFN0eWxlKGl0ZW0ucGFydHNbal0sIG9wdGlvbnMpKTtcblx0XHRcdH1cblx0XHR9IGVsc2Uge1xuXHRcdFx0dmFyIHBhcnRzID0gW107XG5cblx0XHRcdGZvcih2YXIgaiA9IDA7IGogPCBpdGVtLnBhcnRzLmxlbmd0aDsgaisrKSB7XG5cdFx0XHRcdHBhcnRzLnB1c2goYWRkU3R5bGUoaXRlbS5wYXJ0c1tqXSwgb3B0aW9ucykpO1xuXHRcdFx0fVxuXG5cdFx0XHRzdHlsZXNJbkRvbVtpdGVtLmlkXSA9IHtpZDogaXRlbS5pZCwgcmVmczogMSwgcGFydHM6IHBhcnRzfTtcblx0XHR9XG5cdH1cbn1cblxuZnVuY3Rpb24gbGlzdFRvU3R5bGVzIChsaXN0LCBvcHRpb25zKSB7XG5cdHZhciBzdHlsZXMgPSBbXTtcblx0dmFyIG5ld1N0eWxlcyA9IHt9O1xuXG5cdGZvciAodmFyIGkgPSAwOyBpIDwgbGlzdC5sZW5ndGg7IGkrKykge1xuXHRcdHZhciBpdGVtID0gbGlzdFtpXTtcblx0XHR2YXIgaWQgPSBvcHRpb25zLmJhc2UgPyBpdGVtWzBdICsgb3B0aW9ucy5iYXNlIDogaXRlbVswXTtcblx0XHR2YXIgY3NzID0gaXRlbVsxXTtcblx0XHR2YXIgbWVkaWEgPSBpdGVtWzJdO1xuXHRcdHZhciBzb3VyY2VNYXAgPSBpdGVtWzNdO1xuXHRcdHZhciBwYXJ0ID0ge2NzczogY3NzLCBtZWRpYTogbWVkaWEsIHNvdXJjZU1hcDogc291cmNlTWFwfTtcblxuXHRcdGlmKCFuZXdTdHlsZXNbaWRdKSBzdHlsZXMucHVzaChuZXdTdHlsZXNbaWRdID0ge2lkOiBpZCwgcGFydHM6IFtwYXJ0XX0pO1xuXHRcdGVsc2UgbmV3U3R5bGVzW2lkXS5wYXJ0cy5wdXNoKHBhcnQpO1xuXHR9XG5cblx0cmV0dXJuIHN0eWxlcztcbn1cblxuZnVuY3Rpb24gaW5zZXJ0U3R5bGVFbGVtZW50IChvcHRpb25zLCBzdHlsZSkge1xuXHR2YXIgdGFyZ2V0ID0gZ2V0RWxlbWVudChvcHRpb25zLmluc2VydEludG8pXG5cblx0aWYgKCF0YXJnZXQpIHtcblx0XHR0aHJvdyBuZXcgRXJyb3IoXCJDb3VsZG4ndCBmaW5kIGEgc3R5bGUgdGFyZ2V0LiBUaGlzIHByb2JhYmx5IG1lYW5zIHRoYXQgdGhlIHZhbHVlIGZvciB0aGUgJ2luc2VydEludG8nIHBhcmFtZXRlciBpcyBpbnZhbGlkLlwiKTtcblx0fVxuXG5cdHZhciBsYXN0U3R5bGVFbGVtZW50SW5zZXJ0ZWRBdFRvcCA9IHN0eWxlc0luc2VydGVkQXRUb3Bbc3R5bGVzSW5zZXJ0ZWRBdFRvcC5sZW5ndGggLSAxXTtcblxuXHRpZiAob3B0aW9ucy5pbnNlcnRBdCA9PT0gXCJ0b3BcIikge1xuXHRcdGlmICghbGFzdFN0eWxlRWxlbWVudEluc2VydGVkQXRUb3ApIHtcblx0XHRcdHRhcmdldC5pbnNlcnRCZWZvcmUoc3R5bGUsIHRhcmdldC5maXJzdENoaWxkKTtcblx0XHR9IGVsc2UgaWYgKGxhc3RTdHlsZUVsZW1lbnRJbnNlcnRlZEF0VG9wLm5leHRTaWJsaW5nKSB7XG5cdFx0XHR0YXJnZXQuaW5zZXJ0QmVmb3JlKHN0eWxlLCBsYXN0U3R5bGVFbGVtZW50SW5zZXJ0ZWRBdFRvcC5uZXh0U2libGluZyk7XG5cdFx0fSBlbHNlIHtcblx0XHRcdHRhcmdldC5hcHBlbmRDaGlsZChzdHlsZSk7XG5cdFx0fVxuXHRcdHN0eWxlc0luc2VydGVkQXRUb3AucHVzaChzdHlsZSk7XG5cdH0gZWxzZSBpZiAob3B0aW9ucy5pbnNlcnRBdCA9PT0gXCJib3R0b21cIikge1xuXHRcdHRhcmdldC5hcHBlbmRDaGlsZChzdHlsZSk7XG5cdH0gZWxzZSBpZiAodHlwZW9mIG9wdGlvbnMuaW5zZXJ0QXQgPT09IFwib2JqZWN0XCIgJiYgb3B0aW9ucy5pbnNlcnRBdC5iZWZvcmUpIHtcblx0XHR2YXIgbmV4dFNpYmxpbmcgPSBnZXRFbGVtZW50KG9wdGlvbnMuaW5zZXJ0SW50byArIFwiIFwiICsgb3B0aW9ucy5pbnNlcnRBdC5iZWZvcmUpO1xuXHRcdHRhcmdldC5pbnNlcnRCZWZvcmUoc3R5bGUsIG5leHRTaWJsaW5nKTtcblx0fSBlbHNlIHtcblx0XHR0aHJvdyBuZXcgRXJyb3IoXCJbU3R5bGUgTG9hZGVyXVxcblxcbiBJbnZhbGlkIHZhbHVlIGZvciBwYXJhbWV0ZXIgJ2luc2VydEF0JyAoJ29wdGlvbnMuaW5zZXJ0QXQnKSBmb3VuZC5cXG4gTXVzdCBiZSAndG9wJywgJ2JvdHRvbScsIG9yIE9iamVjdC5cXG4gKGh0dHBzOi8vZ2l0aHViLmNvbS93ZWJwYWNrLWNvbnRyaWIvc3R5bGUtbG9hZGVyI2luc2VydGF0KVxcblwiKTtcblx0fVxufVxuXG5mdW5jdGlvbiByZW1vdmVTdHlsZUVsZW1lbnQgKHN0eWxlKSB7XG5cdGlmIChzdHlsZS5wYXJlbnROb2RlID09PSBudWxsKSByZXR1cm4gZmFsc2U7XG5cdHN0eWxlLnBhcmVudE5vZGUucmVtb3ZlQ2hpbGQoc3R5bGUpO1xuXG5cdHZhciBpZHggPSBzdHlsZXNJbnNlcnRlZEF0VG9wLmluZGV4T2Yoc3R5bGUpO1xuXHRpZihpZHggPj0gMCkge1xuXHRcdHN0eWxlc0luc2VydGVkQXRUb3Auc3BsaWNlKGlkeCwgMSk7XG5cdH1cbn1cblxuZnVuY3Rpb24gY3JlYXRlU3R5bGVFbGVtZW50IChvcHRpb25zKSB7XG5cdHZhciBzdHlsZSA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoXCJzdHlsZVwiKTtcblxuXHRpZihvcHRpb25zLmF0dHJzLnR5cGUgPT09IHVuZGVmaW5lZCkge1xuXHRcdG9wdGlvbnMuYXR0cnMudHlwZSA9IFwidGV4dC9jc3NcIjtcblx0fVxuXG5cdGFkZEF0dHJzKHN0eWxlLCBvcHRpb25zLmF0dHJzKTtcblx0aW5zZXJ0U3R5bGVFbGVtZW50KG9wdGlvbnMsIHN0eWxlKTtcblxuXHRyZXR1cm4gc3R5bGU7XG59XG5cbmZ1bmN0aW9uIGNyZWF0ZUxpbmtFbGVtZW50IChvcHRpb25zKSB7XG5cdHZhciBsaW5rID0gZG9jdW1lbnQuY3JlYXRlRWxlbWVudChcImxpbmtcIik7XG5cblx0aWYob3B0aW9ucy5hdHRycy50eXBlID09PSB1bmRlZmluZWQpIHtcblx0XHRvcHRpb25zLmF0dHJzLnR5cGUgPSBcInRleHQvY3NzXCI7XG5cdH1cblx0b3B0aW9ucy5hdHRycy5yZWwgPSBcInN0eWxlc2hlZXRcIjtcblxuXHRhZGRBdHRycyhsaW5rLCBvcHRpb25zLmF0dHJzKTtcblx0aW5zZXJ0U3R5bGVFbGVtZW50KG9wdGlvbnMsIGxpbmspO1xuXG5cdHJldHVybiBsaW5rO1xufVxuXG5mdW5jdGlvbiBhZGRBdHRycyAoZWwsIGF0dHJzKSB7XG5cdE9iamVjdC5rZXlzKGF0dHJzKS5mb3JFYWNoKGZ1bmN0aW9uIChrZXkpIHtcblx0XHRlbC5zZXRBdHRyaWJ1dGUoa2V5LCBhdHRyc1trZXldKTtcblx0fSk7XG59XG5cbmZ1bmN0aW9uIGFkZFN0eWxlIChvYmosIG9wdGlvbnMpIHtcblx0dmFyIHN0eWxlLCB1cGRhdGUsIHJlbW92ZSwgcmVzdWx0O1xuXG5cdC8vIElmIGEgdHJhbnNmb3JtIGZ1bmN0aW9uIHdhcyBkZWZpbmVkLCBydW4gaXQgb24gdGhlIGNzc1xuXHRpZiAob3B0aW9ucy50cmFuc2Zvcm0gJiYgb2JqLmNzcykge1xuXHQgICAgcmVzdWx0ID0gb3B0aW9ucy50cmFuc2Zvcm0ob2JqLmNzcyk7XG5cblx0ICAgIGlmIChyZXN1bHQpIHtcblx0ICAgIFx0Ly8gSWYgdHJhbnNmb3JtIHJldHVybnMgYSB2YWx1ZSwgdXNlIHRoYXQgaW5zdGVhZCBvZiB0aGUgb3JpZ2luYWwgY3NzLlxuXHQgICAgXHQvLyBUaGlzIGFsbG93cyBydW5uaW5nIHJ1bnRpbWUgdHJhbnNmb3JtYXRpb25zIG9uIHRoZSBjc3MuXG5cdCAgICBcdG9iai5jc3MgPSByZXN1bHQ7XG5cdCAgICB9IGVsc2Uge1xuXHQgICAgXHQvLyBJZiB0aGUgdHJhbnNmb3JtIGZ1bmN0aW9uIHJldHVybnMgYSBmYWxzeSB2YWx1ZSwgZG9uJ3QgYWRkIHRoaXMgY3NzLlxuXHQgICAgXHQvLyBUaGlzIGFsbG93cyBjb25kaXRpb25hbCBsb2FkaW5nIG9mIGNzc1xuXHQgICAgXHRyZXR1cm4gZnVuY3Rpb24oKSB7XG5cdCAgICBcdFx0Ly8gbm9vcFxuXHQgICAgXHR9O1xuXHQgICAgfVxuXHR9XG5cblx0aWYgKG9wdGlvbnMuc2luZ2xldG9uKSB7XG5cdFx0dmFyIHN0eWxlSW5kZXggPSBzaW5nbGV0b25Db3VudGVyKys7XG5cblx0XHRzdHlsZSA9IHNpbmdsZXRvbiB8fCAoc2luZ2xldG9uID0gY3JlYXRlU3R5bGVFbGVtZW50KG9wdGlvbnMpKTtcblxuXHRcdHVwZGF0ZSA9IGFwcGx5VG9TaW5nbGV0b25UYWcuYmluZChudWxsLCBzdHlsZSwgc3R5bGVJbmRleCwgZmFsc2UpO1xuXHRcdHJlbW92ZSA9IGFwcGx5VG9TaW5nbGV0b25UYWcuYmluZChudWxsLCBzdHlsZSwgc3R5bGVJbmRleCwgdHJ1ZSk7XG5cblx0fSBlbHNlIGlmIChcblx0XHRvYmouc291cmNlTWFwICYmXG5cdFx0dHlwZW9mIFVSTCA9PT0gXCJmdW5jdGlvblwiICYmXG5cdFx0dHlwZW9mIFVSTC5jcmVhdGVPYmplY3RVUkwgPT09IFwiZnVuY3Rpb25cIiAmJlxuXHRcdHR5cGVvZiBVUkwucmV2b2tlT2JqZWN0VVJMID09PSBcImZ1bmN0aW9uXCIgJiZcblx0XHR0eXBlb2YgQmxvYiA9PT0gXCJmdW5jdGlvblwiICYmXG5cdFx0dHlwZW9mIGJ0b2EgPT09IFwiZnVuY3Rpb25cIlxuXHQpIHtcblx0XHRzdHlsZSA9IGNyZWF0ZUxpbmtFbGVtZW50KG9wdGlvbnMpO1xuXHRcdHVwZGF0ZSA9IHVwZGF0ZUxpbmsuYmluZChudWxsLCBzdHlsZSwgb3B0aW9ucyk7XG5cdFx0cmVtb3ZlID0gZnVuY3Rpb24gKCkge1xuXHRcdFx0cmVtb3ZlU3R5bGVFbGVtZW50KHN0eWxlKTtcblxuXHRcdFx0aWYoc3R5bGUuaHJlZikgVVJMLnJldm9rZU9iamVjdFVSTChzdHlsZS5ocmVmKTtcblx0XHR9O1xuXHR9IGVsc2Uge1xuXHRcdHN0eWxlID0gY3JlYXRlU3R5bGVFbGVtZW50KG9wdGlvbnMpO1xuXHRcdHVwZGF0ZSA9IGFwcGx5VG9UYWcuYmluZChudWxsLCBzdHlsZSk7XG5cdFx0cmVtb3ZlID0gZnVuY3Rpb24gKCkge1xuXHRcdFx0cmVtb3ZlU3R5bGVFbGVtZW50KHN0eWxlKTtcblx0XHR9O1xuXHR9XG5cblx0dXBkYXRlKG9iaik7XG5cblx0cmV0dXJuIGZ1bmN0aW9uIHVwZGF0ZVN0eWxlIChuZXdPYmopIHtcblx0XHRpZiAobmV3T2JqKSB7XG5cdFx0XHRpZiAoXG5cdFx0XHRcdG5ld09iai5jc3MgPT09IG9iai5jc3MgJiZcblx0XHRcdFx0bmV3T2JqLm1lZGlhID09PSBvYmoubWVkaWEgJiZcblx0XHRcdFx0bmV3T2JqLnNvdXJjZU1hcCA9PT0gb2JqLnNvdXJjZU1hcFxuXHRcdFx0KSB7XG5cdFx0XHRcdHJldHVybjtcblx0XHRcdH1cblxuXHRcdFx0dXBkYXRlKG9iaiA9IG5ld09iaik7XG5cdFx0fSBlbHNlIHtcblx0XHRcdHJlbW92ZSgpO1xuXHRcdH1cblx0fTtcbn1cblxudmFyIHJlcGxhY2VUZXh0ID0gKGZ1bmN0aW9uICgpIHtcblx0dmFyIHRleHRTdG9yZSA9IFtdO1xuXG5cdHJldHVybiBmdW5jdGlvbiAoaW5kZXgsIHJlcGxhY2VtZW50KSB7XG5cdFx0dGV4dFN0b3JlW2luZGV4XSA9IHJlcGxhY2VtZW50O1xuXG5cdFx0cmV0dXJuIHRleHRTdG9yZS5maWx0ZXIoQm9vbGVhbikuam9pbignXFxuJyk7XG5cdH07XG59KSgpO1xuXG5mdW5jdGlvbiBhcHBseVRvU2luZ2xldG9uVGFnIChzdHlsZSwgaW5kZXgsIHJlbW92ZSwgb2JqKSB7XG5cdHZhciBjc3MgPSByZW1vdmUgPyBcIlwiIDogb2JqLmNzcztcblxuXHRpZiAoc3R5bGUuc3R5bGVTaGVldCkge1xuXHRcdHN0eWxlLnN0eWxlU2hlZXQuY3NzVGV4dCA9IHJlcGxhY2VUZXh0KGluZGV4LCBjc3MpO1xuXHR9IGVsc2Uge1xuXHRcdHZhciBjc3NOb2RlID0gZG9jdW1lbnQuY3JlYXRlVGV4dE5vZGUoY3NzKTtcblx0XHR2YXIgY2hpbGROb2RlcyA9IHN0eWxlLmNoaWxkTm9kZXM7XG5cblx0XHRpZiAoY2hpbGROb2Rlc1tpbmRleF0pIHN0eWxlLnJlbW92ZUNoaWxkKGNoaWxkTm9kZXNbaW5kZXhdKTtcblxuXHRcdGlmIChjaGlsZE5vZGVzLmxlbmd0aCkge1xuXHRcdFx0c3R5bGUuaW5zZXJ0QmVmb3JlKGNzc05vZGUsIGNoaWxkTm9kZXNbaW5kZXhdKTtcblx0XHR9IGVsc2Uge1xuXHRcdFx0c3R5bGUuYXBwZW5kQ2hpbGQoY3NzTm9kZSk7XG5cdFx0fVxuXHR9XG59XG5cbmZ1bmN0aW9uIGFwcGx5VG9UYWcgKHN0eWxlLCBvYmopIHtcblx0dmFyIGNzcyA9IG9iai5jc3M7XG5cdHZhciBtZWRpYSA9IG9iai5tZWRpYTtcblxuXHRpZihtZWRpYSkge1xuXHRcdHN0eWxlLnNldEF0dHJpYnV0ZShcIm1lZGlhXCIsIG1lZGlhKVxuXHR9XG5cblx0aWYoc3R5bGUuc3R5bGVTaGVldCkge1xuXHRcdHN0eWxlLnN0eWxlU2hlZXQuY3NzVGV4dCA9IGNzcztcblx0fSBlbHNlIHtcblx0XHR3aGlsZShzdHlsZS5maXJzdENoaWxkKSB7XG5cdFx0XHRzdHlsZS5yZW1vdmVDaGlsZChzdHlsZS5maXJzdENoaWxkKTtcblx0XHR9XG5cblx0XHRzdHlsZS5hcHBlbmRDaGlsZChkb2N1bWVudC5jcmVhdGVUZXh0Tm9kZShjc3MpKTtcblx0fVxufVxuXG5mdW5jdGlvbiB1cGRhdGVMaW5rIChsaW5rLCBvcHRpb25zLCBvYmopIHtcblx0dmFyIGNzcyA9IG9iai5jc3M7XG5cdHZhciBzb3VyY2VNYXAgPSBvYmouc291cmNlTWFwO1xuXG5cdC8qXG5cdFx0SWYgY29udmVydFRvQWJzb2x1dGVVcmxzIGlzbid0IGRlZmluZWQsIGJ1dCBzb3VyY2VtYXBzIGFyZSBlbmFibGVkXG5cdFx0YW5kIHRoZXJlIGlzIG5vIHB1YmxpY1BhdGggZGVmaW5lZCB0aGVuIGxldHMgdHVybiBjb252ZXJ0VG9BYnNvbHV0ZVVybHNcblx0XHRvbiBieSBkZWZhdWx0LiAgT3RoZXJ3aXNlIGRlZmF1bHQgdG8gdGhlIGNvbnZlcnRUb0Fic29sdXRlVXJscyBvcHRpb25cblx0XHRkaXJlY3RseVxuXHQqL1xuXHR2YXIgYXV0b0ZpeFVybHMgPSBvcHRpb25zLmNvbnZlcnRUb0Fic29sdXRlVXJscyA9PT0gdW5kZWZpbmVkICYmIHNvdXJjZU1hcDtcblxuXHRpZiAob3B0aW9ucy5jb252ZXJ0VG9BYnNvbHV0ZVVybHMgfHwgYXV0b0ZpeFVybHMpIHtcblx0XHRjc3MgPSBmaXhVcmxzKGNzcyk7XG5cdH1cblxuXHRpZiAoc291cmNlTWFwKSB7XG5cdFx0Ly8gaHR0cDovL3N0YWNrb3ZlcmZsb3cuY29tL2EvMjY2MDM4NzVcblx0XHRjc3MgKz0gXCJcXG4vKiMgc291cmNlTWFwcGluZ1VSTD1kYXRhOmFwcGxpY2F0aW9uL2pzb247YmFzZTY0LFwiICsgYnRvYSh1bmVzY2FwZShlbmNvZGVVUklDb21wb25lbnQoSlNPTi5zdHJpbmdpZnkoc291cmNlTWFwKSkpKSArIFwiICovXCI7XG5cdH1cblxuXHR2YXIgYmxvYiA9IG5ldyBCbG9iKFtjc3NdLCB7IHR5cGU6IFwidGV4dC9jc3NcIiB9KTtcblxuXHR2YXIgb2xkU3JjID0gbGluay5ocmVmO1xuXG5cdGxpbmsuaHJlZiA9IFVSTC5jcmVhdGVPYmplY3RVUkwoYmxvYik7XG5cblx0aWYob2xkU3JjKSBVUkwucmV2b2tlT2JqZWN0VVJMKG9sZFNyYyk7XG59XG4iLCJcbi8qKlxuICogV2hlbiBzb3VyY2UgbWFwcyBhcmUgZW5hYmxlZCwgYHN0eWxlLWxvYWRlcmAgdXNlcyBhIGxpbmsgZWxlbWVudCB3aXRoIGEgZGF0YS11cmkgdG9cbiAqIGVtYmVkIHRoZSBjc3Mgb24gdGhlIHBhZ2UuIFRoaXMgYnJlYWtzIGFsbCByZWxhdGl2ZSB1cmxzIGJlY2F1c2Ugbm93IHRoZXkgYXJlIHJlbGF0aXZlIHRvIGFcbiAqIGJ1bmRsZSBpbnN0ZWFkIG9mIHRoZSBjdXJyZW50IHBhZ2UuXG4gKlxuICogT25lIHNvbHV0aW9uIGlzIHRvIG9ubHkgdXNlIGZ1bGwgdXJscywgYnV0IHRoYXQgbWF5IGJlIGltcG9zc2libGUuXG4gKlxuICogSW5zdGVhZCwgdGhpcyBmdW5jdGlvbiBcImZpeGVzXCIgdGhlIHJlbGF0aXZlIHVybHMgdG8gYmUgYWJzb2x1dGUgYWNjb3JkaW5nIHRvIHRoZSBjdXJyZW50IHBhZ2UgbG9jYXRpb24uXG4gKlxuICogQSBydWRpbWVudGFyeSB0ZXN0IHN1aXRlIGlzIGxvY2F0ZWQgYXQgYHRlc3QvZml4VXJscy5qc2AgYW5kIGNhbiBiZSBydW4gdmlhIHRoZSBgbnBtIHRlc3RgIGNvbW1hbmQuXG4gKlxuICovXG5cbm1vZHVsZS5leHBvcnRzID0gZnVuY3Rpb24gKGNzcykge1xuICAvLyBnZXQgY3VycmVudCBsb2NhdGlvblxuICB2YXIgbG9jYXRpb24gPSB0eXBlb2Ygd2luZG93ICE9PSBcInVuZGVmaW5lZFwiICYmIHdpbmRvdy5sb2NhdGlvbjtcblxuICBpZiAoIWxvY2F0aW9uKSB7XG4gICAgdGhyb3cgbmV3IEVycm9yKFwiZml4VXJscyByZXF1aXJlcyB3aW5kb3cubG9jYXRpb25cIik7XG4gIH1cblxuXHQvLyBibGFuayBvciBudWxsP1xuXHRpZiAoIWNzcyB8fCB0eXBlb2YgY3NzICE9PSBcInN0cmluZ1wiKSB7XG5cdCAgcmV0dXJuIGNzcztcbiAgfVxuXG4gIHZhciBiYXNlVXJsID0gbG9jYXRpb24ucHJvdG9jb2wgKyBcIi8vXCIgKyBsb2NhdGlvbi5ob3N0O1xuICB2YXIgY3VycmVudERpciA9IGJhc2VVcmwgKyBsb2NhdGlvbi5wYXRobmFtZS5yZXBsYWNlKC9cXC9bXlxcL10qJC8sIFwiL1wiKTtcblxuXHQvLyBjb252ZXJ0IGVhY2ggdXJsKC4uLilcblx0Lypcblx0VGhpcyByZWd1bGFyIGV4cHJlc3Npb24gaXMganVzdCBhIHdheSB0byByZWN1cnNpdmVseSBtYXRjaCBicmFja2V0cyB3aXRoaW5cblx0YSBzdHJpbmcuXG5cblx0IC91cmxcXHMqXFwoICA9IE1hdGNoIG9uIHRoZSB3b3JkIFwidXJsXCIgd2l0aCBhbnkgd2hpdGVzcGFjZSBhZnRlciBpdCBhbmQgdGhlbiBhIHBhcmVuc1xuXHQgICAoICA9IFN0YXJ0IGEgY2FwdHVyaW5nIGdyb3VwXG5cdCAgICAgKD86ICA9IFN0YXJ0IGEgbm9uLWNhcHR1cmluZyBncm91cFxuXHQgICAgICAgICBbXikoXSAgPSBNYXRjaCBhbnl0aGluZyB0aGF0IGlzbid0IGEgcGFyZW50aGVzZXNcblx0ICAgICAgICAgfCAgPSBPUlxuXHQgICAgICAgICBcXCggID0gTWF0Y2ggYSBzdGFydCBwYXJlbnRoZXNlc1xuXHQgICAgICAgICAgICAgKD86ICA9IFN0YXJ0IGFub3RoZXIgbm9uLWNhcHR1cmluZyBncm91cHNcblx0ICAgICAgICAgICAgICAgICBbXikoXSsgID0gTWF0Y2ggYW55dGhpbmcgdGhhdCBpc24ndCBhIHBhcmVudGhlc2VzXG5cdCAgICAgICAgICAgICAgICAgfCAgPSBPUlxuXHQgICAgICAgICAgICAgICAgIFxcKCAgPSBNYXRjaCBhIHN0YXJ0IHBhcmVudGhlc2VzXG5cdCAgICAgICAgICAgICAgICAgICAgIFteKShdKiAgPSBNYXRjaCBhbnl0aGluZyB0aGF0IGlzbid0IGEgcGFyZW50aGVzZXNcblx0ICAgICAgICAgICAgICAgICBcXCkgID0gTWF0Y2ggYSBlbmQgcGFyZW50aGVzZXNcblx0ICAgICAgICAgICAgICkgID0gRW5kIEdyb3VwXG4gICAgICAgICAgICAgICpcXCkgPSBNYXRjaCBhbnl0aGluZyBhbmQgdGhlbiBhIGNsb3NlIHBhcmVuc1xuICAgICAgICAgICkgID0gQ2xvc2Ugbm9uLWNhcHR1cmluZyBncm91cFxuICAgICAgICAgICogID0gTWF0Y2ggYW55dGhpbmdcbiAgICAgICApICA9IENsb3NlIGNhcHR1cmluZyBncm91cFxuXHQgXFwpICA9IE1hdGNoIGEgY2xvc2UgcGFyZW5zXG5cblx0IC9naSAgPSBHZXQgYWxsIG1hdGNoZXMsIG5vdCB0aGUgZmlyc3QuICBCZSBjYXNlIGluc2Vuc2l0aXZlLlxuXHQgKi9cblx0dmFyIGZpeGVkQ3NzID0gY3NzLnJlcGxhY2UoL3VybFxccypcXCgoKD86W14pKF18XFwoKD86W14pKF0rfFxcKFteKShdKlxcKSkqXFwpKSopXFwpL2dpLCBmdW5jdGlvbihmdWxsTWF0Y2gsIG9yaWdVcmwpIHtcblx0XHQvLyBzdHJpcCBxdW90ZXMgKGlmIHRoZXkgZXhpc3QpXG5cdFx0dmFyIHVucXVvdGVkT3JpZ1VybCA9IG9yaWdVcmxcblx0XHRcdC50cmltKClcblx0XHRcdC5yZXBsYWNlKC9eXCIoLiopXCIkLywgZnVuY3Rpb24obywgJDEpeyByZXR1cm4gJDE7IH0pXG5cdFx0XHQucmVwbGFjZSgvXicoLiopJyQvLCBmdW5jdGlvbihvLCAkMSl7IHJldHVybiAkMTsgfSk7XG5cblx0XHQvLyBhbHJlYWR5IGEgZnVsbCB1cmw/IG5vIGNoYW5nZVxuXHRcdGlmICgvXigjfGRhdGE6fGh0dHA6XFwvXFwvfGh0dHBzOlxcL1xcL3xmaWxlOlxcL1xcL1xcL3xcXHMqJCkvaS50ZXN0KHVucXVvdGVkT3JpZ1VybCkpIHtcblx0XHQgIHJldHVybiBmdWxsTWF0Y2g7XG5cdFx0fVxuXG5cdFx0Ly8gY29udmVydCB0aGUgdXJsIHRvIGEgZnVsbCB1cmxcblx0XHR2YXIgbmV3VXJsO1xuXG5cdFx0aWYgKHVucXVvdGVkT3JpZ1VybC5pbmRleE9mKFwiLy9cIikgPT09IDApIHtcblx0XHQgIFx0Ly9UT0RPOiBzaG91bGQgd2UgYWRkIHByb3RvY29sP1xuXHRcdFx0bmV3VXJsID0gdW5xdW90ZWRPcmlnVXJsO1xuXHRcdH0gZWxzZSBpZiAodW5xdW90ZWRPcmlnVXJsLmluZGV4T2YoXCIvXCIpID09PSAwKSB7XG5cdFx0XHQvLyBwYXRoIHNob3VsZCBiZSByZWxhdGl2ZSB0byB0aGUgYmFzZSB1cmxcblx0XHRcdG5ld1VybCA9IGJhc2VVcmwgKyB1bnF1b3RlZE9yaWdVcmw7IC8vIGFscmVhZHkgc3RhcnRzIHdpdGggJy8nXG5cdFx0fSBlbHNlIHtcblx0XHRcdC8vIHBhdGggc2hvdWxkIGJlIHJlbGF0aXZlIHRvIGN1cnJlbnQgZGlyZWN0b3J5XG5cdFx0XHRuZXdVcmwgPSBjdXJyZW50RGlyICsgdW5xdW90ZWRPcmlnVXJsLnJlcGxhY2UoL15cXC5cXC8vLCBcIlwiKTsgLy8gU3RyaXAgbGVhZGluZyAnLi8nXG5cdFx0fVxuXG5cdFx0Ly8gc2VuZCBiYWNrIHRoZSBmaXhlZCB1cmwoLi4uKVxuXHRcdHJldHVybiBcInVybChcIiArIEpTT04uc3RyaW5naWZ5KG5ld1VybCkgKyBcIilcIjtcblx0fSk7XG5cblx0Ly8gc2VuZCBiYWNrIHRoZSBmaXhlZCBjc3Ncblx0cmV0dXJuIGZpeGVkQ3NzO1xufTtcbiIsInZhciBfdHlwZW9mID0gdHlwZW9mIFN5bWJvbCA9PT0gXCJmdW5jdGlvblwiICYmIHR5cGVvZiBTeW1ib2wuaXRlcmF0b3IgPT09IFwic3ltYm9sXCIgPyBmdW5jdGlvbiAob2JqKSB7IHJldHVybiB0eXBlb2Ygb2JqOyB9IDogZnVuY3Rpb24gKG9iaikgeyByZXR1cm4gb2JqICYmIHR5cGVvZiBTeW1ib2wgPT09IFwiZnVuY3Rpb25cIiAmJiBvYmouY29uc3RydWN0b3IgPT09IFN5bWJvbCAmJiBvYmogIT09IFN5bWJvbC5wcm90b3R5cGUgPyBcInN5bWJvbFwiIDogdHlwZW9mIG9iajsgfTtcblxuZnVuY3Rpb24gdmFsdWVFcXVhbChhLCBiKSB7XG4gIGlmIChhID09PSBiKSByZXR1cm4gdHJ1ZTtcblxuICBpZiAoYSA9PSBudWxsIHx8IGIgPT0gbnVsbCkgcmV0dXJuIGZhbHNlO1xuXG4gIGlmIChBcnJheS5pc0FycmF5KGEpKSB7XG4gICAgcmV0dXJuIEFycmF5LmlzQXJyYXkoYikgJiYgYS5sZW5ndGggPT09IGIubGVuZ3RoICYmIGEuZXZlcnkoZnVuY3Rpb24gKGl0ZW0sIGluZGV4KSB7XG4gICAgICByZXR1cm4gdmFsdWVFcXVhbChpdGVtLCBiW2luZGV4XSk7XG4gICAgfSk7XG4gIH1cblxuICB2YXIgYVR5cGUgPSB0eXBlb2YgYSA9PT0gJ3VuZGVmaW5lZCcgPyAndW5kZWZpbmVkJyA6IF90eXBlb2YoYSk7XG4gIHZhciBiVHlwZSA9IHR5cGVvZiBiID09PSAndW5kZWZpbmVkJyA/ICd1bmRlZmluZWQnIDogX3R5cGVvZihiKTtcblxuICBpZiAoYVR5cGUgIT09IGJUeXBlKSByZXR1cm4gZmFsc2U7XG5cbiAgaWYgKGFUeXBlID09PSAnb2JqZWN0Jykge1xuICAgIHZhciBhVmFsdWUgPSBhLnZhbHVlT2YoKTtcbiAgICB2YXIgYlZhbHVlID0gYi52YWx1ZU9mKCk7XG5cbiAgICBpZiAoYVZhbHVlICE9PSBhIHx8IGJWYWx1ZSAhPT0gYikgcmV0dXJuIHZhbHVlRXF1YWwoYVZhbHVlLCBiVmFsdWUpO1xuXG4gICAgdmFyIGFLZXlzID0gT2JqZWN0LmtleXMoYSk7XG4gICAgdmFyIGJLZXlzID0gT2JqZWN0LmtleXMoYik7XG5cbiAgICBpZiAoYUtleXMubGVuZ3RoICE9PSBiS2V5cy5sZW5ndGgpIHJldHVybiBmYWxzZTtcblxuICAgIHJldHVybiBhS2V5cy5ldmVyeShmdW5jdGlvbiAoa2V5KSB7XG4gICAgICByZXR1cm4gdmFsdWVFcXVhbChhW2tleV0sIGJba2V5XSk7XG4gICAgfSk7XG4gIH1cblxuICByZXR1cm4gZmFsc2U7XG59XG5cbmV4cG9ydCBkZWZhdWx0IHZhbHVlRXF1YWw7IiwiLyoqXG4gKiBDb3B5cmlnaHQgMjAxNC0yMDE1LCBGYWNlYm9vaywgSW5jLlxuICogQWxsIHJpZ2h0cyByZXNlcnZlZC5cbiAqXG4gKiBUaGlzIHNvdXJjZSBjb2RlIGlzIGxpY2Vuc2VkIHVuZGVyIHRoZSBCU0Qtc3R5bGUgbGljZW5zZSBmb3VuZCBpbiB0aGVcbiAqIExJQ0VOU0UgZmlsZSBpbiB0aGUgcm9vdCBkaXJlY3Rvcnkgb2YgdGhpcyBzb3VyY2UgdHJlZS4gQW4gYWRkaXRpb25hbCBncmFudFxuICogb2YgcGF0ZW50IHJpZ2h0cyBjYW4gYmUgZm91bmQgaW4gdGhlIFBBVEVOVFMgZmlsZSBpbiB0aGUgc2FtZSBkaXJlY3RvcnkuXG4gKi9cblxuJ3VzZSBzdHJpY3QnO1xuXG4vKipcbiAqIFNpbWlsYXIgdG8gaW52YXJpYW50IGJ1dCBvbmx5IGxvZ3MgYSB3YXJuaW5nIGlmIHRoZSBjb25kaXRpb24gaXMgbm90IG1ldC5cbiAqIFRoaXMgY2FuIGJlIHVzZWQgdG8gbG9nIGlzc3VlcyBpbiBkZXZlbG9wbWVudCBlbnZpcm9ubWVudHMgaW4gY3JpdGljYWxcbiAqIHBhdGhzLiBSZW1vdmluZyB0aGUgbG9nZ2luZyBjb2RlIGZvciBwcm9kdWN0aW9uIGVudmlyb25tZW50cyB3aWxsIGtlZXAgdGhlXG4gKiBzYW1lIGxvZ2ljIGFuZCBmb2xsb3cgdGhlIHNhbWUgY29kZSBwYXRocy5cbiAqL1xuXG52YXIgd2FybmluZyA9IGZ1bmN0aW9uKCkge307XG5cbmlmIChwcm9jZXNzLmVudi5OT0RFX0VOViAhPT0gJ3Byb2R1Y3Rpb24nKSB7XG4gIHdhcm5pbmcgPSBmdW5jdGlvbihjb25kaXRpb24sIGZvcm1hdCwgYXJncykge1xuICAgIHZhciBsZW4gPSBhcmd1bWVudHMubGVuZ3RoO1xuICAgIGFyZ3MgPSBuZXcgQXJyYXkobGVuID4gMiA/IGxlbiAtIDIgOiAwKTtcbiAgICBmb3IgKHZhciBrZXkgPSAyOyBrZXkgPCBsZW47IGtleSsrKSB7XG4gICAgICBhcmdzW2tleSAtIDJdID0gYXJndW1lbnRzW2tleV07XG4gICAgfVxuICAgIGlmIChmb3JtYXQgPT09IHVuZGVmaW5lZCkge1xuICAgICAgdGhyb3cgbmV3IEVycm9yKFxuICAgICAgICAnYHdhcm5pbmcoY29uZGl0aW9uLCBmb3JtYXQsIC4uLmFyZ3MpYCByZXF1aXJlcyBhIHdhcm5pbmcgJyArXG4gICAgICAgICdtZXNzYWdlIGFyZ3VtZW50J1xuICAgICAgKTtcbiAgICB9XG5cbiAgICBpZiAoZm9ybWF0Lmxlbmd0aCA8IDEwIHx8ICgvXltzXFxXXSokLykudGVzdChmb3JtYXQpKSB7XG4gICAgICB0aHJvdyBuZXcgRXJyb3IoXG4gICAgICAgICdUaGUgd2FybmluZyBmb3JtYXQgc2hvdWxkIGJlIGFibGUgdG8gdW5pcXVlbHkgaWRlbnRpZnkgdGhpcyAnICtcbiAgICAgICAgJ3dhcm5pbmcuIFBsZWFzZSwgdXNlIGEgbW9yZSBkZXNjcmlwdGl2ZSBmb3JtYXQgdGhhbjogJyArIGZvcm1hdFxuICAgICAgKTtcbiAgICB9XG5cbiAgICBpZiAoIWNvbmRpdGlvbikge1xuICAgICAgdmFyIGFyZ0luZGV4ID0gMDtcbiAgICAgIHZhciBtZXNzYWdlID0gJ1dhcm5pbmc6ICcgK1xuICAgICAgICBmb3JtYXQucmVwbGFjZSgvJXMvZywgZnVuY3Rpb24oKSB7XG4gICAgICAgICAgcmV0dXJuIGFyZ3NbYXJnSW5kZXgrK107XG4gICAgICAgIH0pO1xuICAgICAgaWYgKHR5cGVvZiBjb25zb2xlICE9PSAndW5kZWZpbmVkJykge1xuICAgICAgICBjb25zb2xlLmVycm9yKG1lc3NhZ2UpO1xuICAgICAgfVxuICAgICAgdHJ5IHtcbiAgICAgICAgLy8gVGhpcyBlcnJvciB3YXMgdGhyb3duIGFzIGEgY29udmVuaWVuY2Ugc28gdGhhdCB5b3UgY2FuIHVzZSB0aGlzIHN0YWNrXG4gICAgICAgIC8vIHRvIGZpbmQgdGhlIGNhbGxzaXRlIHRoYXQgY2F1c2VkIHRoaXMgd2FybmluZyB0byBmaXJlLlxuICAgICAgICB0aHJvdyBuZXcgRXJyb3IobWVzc2FnZSk7XG4gICAgICB9IGNhdGNoKHgpIHt9XG4gICAgfVxuICB9O1xufVxuXG5tb2R1bGUuZXhwb3J0cyA9IHdhcm5pbmc7XG4iLCIvLyoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxuLy8gIFBlcmlvZGljRGF0YURpc3BsYXkxLnRzIC0gR2J0Y1xuLy9cbi8vICBDb3B5cmlnaHQgwqkgMjAxOCwgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlLiAgQWxsIFJpZ2h0cyBSZXNlcnZlZC5cbi8vXG4vLyAgTGljZW5zZWQgdG8gdGhlIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZSAoR1BBKSB1bmRlciBvbmUgb3IgbW9yZSBjb250cmlidXRvciBsaWNlbnNlIGFncmVlbWVudHMuIFNlZVxuLy8gIHRoZSBOT1RJQ0UgZmlsZSBkaXN0cmlidXRlZCB3aXRoIHRoaXMgd29yayBmb3IgYWRkaXRpb25hbCBpbmZvcm1hdGlvbiByZWdhcmRpbmcgY29weXJpZ2h0IG93bmVyc2hpcC5cbi8vICBUaGUgR1BBIGxpY2Vuc2VzIHRoaXMgZmlsZSB0byB5b3UgdW5kZXIgdGhlIE1JVCBMaWNlbnNlIChNSVQpLCB0aGUgXCJMaWNlbnNlXCI7IHlvdSBtYXkgbm90IHVzZSB0aGlzXG4vLyAgZmlsZSBleGNlcHQgaW4gY29tcGxpYW5jZSB3aXRoIHRoZSBMaWNlbnNlLiBZb3UgbWF5IG9idGFpbiBhIGNvcHkgb2YgdGhlIExpY2Vuc2UgYXQ6XG4vL1xuLy8gICAgICBodHRwOi8vb3BlbnNvdXJjZS5vcmcvbGljZW5zZXMvTUlUXG4vL1xuLy8gIFVubGVzcyBhZ3JlZWQgdG8gaW4gd3JpdGluZywgdGhlIHN1YmplY3Qgc29mdHdhcmUgZGlzdHJpYnV0ZWQgdW5kZXIgdGhlIExpY2Vuc2UgaXMgZGlzdHJpYnV0ZWQgb24gYW5cbi8vICBcIkFTLUlTXCIgQkFTSVMsIFdJVEhPVVQgV0FSUkFOVElFUyBPUiBDT05ESVRJT05TIE9GIEFOWSBLSU5ELCBlaXRoZXIgZXhwcmVzcyBvciBpbXBsaWVkLiBSZWZlciB0byB0aGVcbi8vICBMaWNlbnNlIGZvciB0aGUgc3BlY2lmaWMgbGFuZ3VhZ2UgZ292ZXJuaW5nIHBlcm1pc3Npb25zIGFuZCBsaW1pdGF0aW9ucy5cbi8vXG4vLyAgQ29kZSBNb2RpZmljYXRpb24gSGlzdG9yeTpcbi8vICAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXG4vLyAgMDUvMjUvMjAxOCAtIEJpbGx5IEVybmVzdFxuLy8gICAgICAgR2VuZXJhdGVkIG9yaWdpbmFsIHZlcnNpb24gb2Ygc291cmNlIGNvZGUuXG4vL1xuLy8qKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcbmltcG9ydCAqIGFzIG1vbWVudCBmcm9tICdtb21lbnQnO1xuaW1wb3J0IHsgUGVyaW9kaWNEYXRhRGlzcGxheSB9IGZyb20gJy4vLi4vLi4vVFNYL2dsb2JhbCdcblxuZXhwb3J0IGRlZmF1bHQgY2xhc3MgUGVyaW9kaWNEYXRhRGlzcGxheVNlcnZpY2Uge1xuICAgIGdldERhdGEobWV0ZXJJRDogbnVtYmVyLCBzdGFydERhdGU6IHN0cmluZywgZW5kRGF0ZTpzdHJpbmcsIHBpeGVsczogbnVtYmVyLCBtZWFzdXJlbWVudENoYXJhY3RlcmlzdGljSUQ6bnVtYmVyLCBtZWFzdXJlbWVudFR5cGVJRDpudW1iZXIsaGFybW9uaWNHcm91cDogbnVtYmVyLCB0eXBlKSB7XG4gICAgICAgIHJldHVybiAkLmFqYXgoe1xuICAgICAgICAgICAgdHlwZTogXCJHRVRcIixcbiAgICAgICAgICAgIHVybDogYCR7d2luZG93LmxvY2F0aW9uLm9yaWdpbn0vYXBpL1BlcmlvZGljRGF0YURpc3BsYXkvR2V0RGF0YT9NZXRlcklEPSR7bWV0ZXJJRH1gICtcbiAgICAgICAgICAgICAgICBgJnN0YXJ0RGF0ZT0ke21vbWVudChzdGFydERhdGUpLmZvcm1hdCgnWVlZWS1NTS1ERCcpfWAgK1xuICAgICAgICAgICAgICAgIGAmZW5kRGF0ZT0ke21vbWVudChlbmREYXRlKS5mb3JtYXQoJ1lZWVktTU0tREQnKX1gICtcbiAgICAgICAgICAgICAgICBgJnBpeGVscz0ke3BpeGVsc31gICtcbiAgICAgICAgICAgICAgICBgJk1lYXN1cmVtZW50Q2hhcmFjdGVyaXN0aWNJRD0ke21lYXN1cmVtZW50Q2hhcmFjdGVyaXN0aWNJRH1gICsgXG4gICAgICAgICAgICAgICAgYCZNZWFzdXJlbWVudFR5cGVJRD0ke21lYXN1cmVtZW50VHlwZUlEfWAgK1xuICAgICAgICAgICAgICAgIGAmSGFybW9uaWNHcm91cD0ke2hhcm1vbmljR3JvdXB9YCArXG4gICAgICAgICAgICAgICAgYCZ0eXBlPSR7dHlwZX1gLFxuICAgICAgICAgICAgY29udGVudFR5cGU6IFwiYXBwbGljYXRpb24vanNvbjsgY2hhcnNldD11dGYtOFwiLFxuICAgICAgICAgICAgZGF0YVR5cGU6ICdqc29uJyxcbiAgICAgICAgICAgIGNhY2hlOiB0cnVlLFxuICAgICAgICAgICAgYXN5bmM6IHRydWVcbiAgICAgICAgfSkgYXMgSlF1ZXJ5LmpxWEhSPFBlcmlvZGljRGF0YURpc3BsYXkuUmV0dXJuRGF0YT47XG4gICAgfVxuXG4gICAgZ2V0TWV0ZXJzKCkge1xuICAgICAgICByZXR1cm4gJC5hamF4KHtcbiAgICAgICAgICAgIHR5cGU6IFwiR0VUXCIsXG4gICAgICAgICAgICB1cmw6IGAke3dpbmRvdy5sb2NhdGlvbi5vcmlnaW59L2FwaS9QZXJpb2RpY0RhdGFEaXNwbGF5L0dldE1ldGVyc2AsXG4gICAgICAgICAgICBjb250ZW50VHlwZTogXCJhcHBsaWNhdGlvbi9qc29uOyBjaGFyc2V0PXV0Zi04XCIsXG4gICAgICAgICAgICBkYXRhVHlwZTogJ2pzb24nLFxuICAgICAgICAgICAgY2FjaGU6IHRydWUsXG4gICAgICAgICAgICBhc3luYzogdHJ1ZVxuICAgICAgICB9KTtcbiAgICB9XG5cbiAgICBnZXRNZWFzdXJlbWVudENoYXJhY3RlcmlzdGljcyhtZXRlcklEKSB7XG4gICAgICAgIHJldHVybiAkLmFqYXgoe1xuICAgICAgICAgICAgdHlwZTogXCJHRVRcIixcbiAgICAgICAgICAgIHVybDogYCR7d2luZG93LmxvY2F0aW9uLm9yaWdpbn0vYXBpL1BlcmlvZGljRGF0YURpc3BsYXkvR2V0TWVhc3VyZW1lbnRDaGFyYWN0ZXJpc3RpY3NgLFxuICAgICAgICAgICAgY29udGVudFR5cGU6IFwiYXBwbGljYXRpb24vanNvbjsgY2hhcnNldD11dGYtOFwiLFxuICAgICAgICAgICAgZGF0YVR5cGU6ICdqc29uJyxcbiAgICAgICAgICAgIGNhY2hlOiB0cnVlLFxuICAgICAgICAgICAgYXN5bmM6IHRydWVcbiAgICAgICAgfSkgYXMgSlF1ZXJ5LmpxWEhSPFBlcmlvZGljRGF0YURpc3BsYXkuTWVhc3VyZW1lbnRDaGFyYXRlcmlzdGljc1tdPjtcbiAgICB9XG5cblxufSIsIi8vKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXG4vLyAgUGVyaW9kaWNEYXRhRGlzcGxheTEudHMgLSBHYnRjXG4vL1xuLy8gIENvcHlyaWdodCDCqSAyMDE4LCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxuLy9cbi8vICBMaWNlbnNlZCB0byB0aGUgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlIChHUEEpIHVuZGVyIG9uZSBvciBtb3JlIGNvbnRyaWJ1dG9yIGxpY2Vuc2UgYWdyZWVtZW50cy4gU2VlXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxuLy8gIFRoZSBHUEEgbGljZW5zZXMgdGhpcyBmaWxlIHRvIHlvdSB1bmRlciB0aGUgTUlUIExpY2Vuc2UgKE1JVCksIHRoZSBcIkxpY2Vuc2VcIjsgeW91IG1heSBub3QgdXNlIHRoaXNcbi8vICBmaWxlIGV4Y2VwdCBpbiBjb21wbGlhbmNlIHdpdGggdGhlIExpY2Vuc2UuIFlvdSBtYXkgb2J0YWluIGEgY29weSBvZiB0aGUgTGljZW5zZSBhdDpcbi8vXG4vLyAgICAgIGh0dHA6Ly9vcGVuc291cmNlLm9yZy9saWNlbnNlcy9NSVRcbi8vXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxuLy8gIFwiQVMtSVNcIiBCQVNJUywgV0lUSE9VVCBXQVJSQU5USUVTIE9SIENPTkRJVElPTlMgT0YgQU5ZIEtJTkQsIGVpdGhlciBleHByZXNzIG9yIGltcGxpZWQuIFJlZmVyIHRvIHRoZVxuLy8gIExpY2Vuc2UgZm9yIHRoZSBzcGVjaWZpYyBsYW5ndWFnZSBnb3Zlcm5pbmcgcGVybWlzc2lvbnMgYW5kIGxpbWl0YXRpb25zLlxuLy9cbi8vICBDb2RlIE1vZGlmaWNhdGlvbiBIaXN0b3J5OlxuLy8gIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cbi8vICAwNS8yNS8yMDE4IC0gQmlsbHkgRXJuZXN0XG4vLyAgICAgICBHZW5lcmF0ZWQgb3JpZ2luYWwgdmVyc2lvbiBvZiBzb3VyY2UgY29kZS5cbi8vXG4vLyoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxuaW1wb3J0ICogYXMgbW9tZW50IGZyb20gJ21vbWVudCc7XG5pbXBvcnQgeyBQZXJpb2RpY0RhdGFEaXNwbGF5LCBUcmVuZGluZ2NEYXRhRGlzcGxheSB9IGZyb20gJy4vLi4vLi4vVFNYL2dsb2JhbCdcblxuZXhwb3J0IGRlZmF1bHQgY2xhc3MgVHJlbmRpbmdEYXRhRGlzcGxheVNlcnZpY2Uge1xuICAgIGdldERhdGEoY2hhbm5lbElEOiBudW1iZXIsIHN0YXJ0RGF0ZTogc3RyaW5nLCBlbmREYXRlOiBzdHJpbmcsIHBpeGVsczogbnVtYmVyKSB7XG4gICAgICAgIHJldHVybiAkLmFqYXgoe1xuICAgICAgICAgICAgdHlwZTogXCJHRVRcIixcbiAgICAgICAgICAgIHVybDogYCR7d2luZG93LmxvY2F0aW9uLm9yaWdpbn0vYXBpL1RyZW5kaW5nRGF0YURpc3BsYXkvR2V0RGF0YT9DaGFubmVsSUQ9JHtjaGFubmVsSUR9YCArXG4gICAgICAgICAgICAgICAgYCZzdGFydERhdGU9JHttb21lbnQoc3RhcnREYXRlKS5mb3JtYXQoJ1lZWVktTU0tRERUSEg6bW0nKX1gICtcbiAgICAgICAgICAgICAgICBgJmVuZERhdGU9JHttb21lbnQoZW5kRGF0ZSkuZm9ybWF0KCdZWVlZLU1NLUREVEhIOm1tJyl9YCArXG4gICAgICAgICAgICAgICAgYCZwaXhlbHM9JHtwaXhlbHN9YCxcbiAgICAgICAgICAgIGNvbnRlbnRUeXBlOiBcImFwcGxpY2F0aW9uL2pzb247IGNoYXJzZXQ9dXRmLThcIixcbiAgICAgICAgICAgIGRhdGFUeXBlOiAnanNvbicsXG4gICAgICAgICAgICBjYWNoZTogdHJ1ZSxcbiAgICAgICAgICAgIGFzeW5jOiB0cnVlXG4gICAgICAgIH0pIGFzIEpRdWVyeS5qcVhIUjxQZXJpb2RpY0RhdGFEaXNwbGF5LlJldHVybkRhdGE+O1xuICAgIH1cbiAgICBnZXRQb3N0RGF0YShtZWFzdXJlbWVudHM6IFRyZW5kaW5nY0RhdGFEaXNwbGF5Lk1lYXN1cmVtZW50W10sIHN0YXJ0RGF0ZTogc3RyaW5nLCBlbmREYXRlOiBzdHJpbmcsIHBpeGVsczogbnVtYmVyKSB7XG4gICAgICAgIHJldHVybiAkLmFqYXgoe1xuICAgICAgICAgICAgdHlwZTogXCJQT1NUXCIsXG4gICAgICAgICAgICB1cmw6IGAke3dpbmRvdy5sb2NhdGlvbi5vcmlnaW59L2FwaS9UcmVuZGluZ0RhdGFEaXNwbGF5L0dldERhdGFgLFxuICAgICAgICAgICAgY29udGVudFR5cGU6IFwiYXBwbGljYXRpb24vanNvbjsgY2hhcnNldD11dGYtOFwiLFxuICAgICAgICAgICAgZGF0YVR5cGU6ICdqc29uJyxcbiAgICAgICAgICAgIGRhdGE6IEpTT04uc3RyaW5naWZ5KHtDaGFubmVsczogbWVhc3VyZW1lbnRzLm1hcChtcyA9PiBtcy5JRCksIFN0YXJ0RGF0ZTogc3RhcnREYXRlLCBFbmREYXRlOiBlbmREYXRlLCBQaXhlbHM6IHBpeGVscyB9KSxcbiAgICAgICAgICAgIGNhY2hlOiB0cnVlLFxuICAgICAgICAgICAgYXN5bmM6IHRydWVcbiAgICAgICAgfSkgYXMgSlF1ZXJ5LmpxWEhSPFRyZW5kaW5nY0RhdGFEaXNwbGF5LlJldHVybkRhdGE+O1xuICAgIH1cblxuXG5cbiAgICBnZXRNZWFzdXJlbWVudHMobWV0ZXJJRDogbnVtYmVyKSB7XG4gICAgICAgIHJldHVybiAkLmFqYXgoe1xuICAgICAgICAgICAgdHlwZTogXCJHRVRcIixcbiAgICAgICAgICAgIHVybDogYCR7d2luZG93LmxvY2F0aW9uLm9yaWdpbn0vYXBpL1RyZW5kaW5nRGF0YURpc3BsYXkvR2V0TWVhc3VyZW1lbnRzP01ldGVySUQ9JHttZXRlcklEfWAsXG4gICAgICAgICAgICBjb250ZW50VHlwZTogXCJhcHBsaWNhdGlvbi9qc29uOyBjaGFyc2V0PXV0Zi04XCIsXG4gICAgICAgICAgICBkYXRhVHlwZTogJ2pzb24nLFxuICAgICAgICAgICAgY2FjaGU6IHRydWUsXG4gICAgICAgICAgICBhc3luYzogdHJ1ZVxuICAgICAgICB9KTtcbiAgICB9XG5cblxufSIsIi8vKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXG4vLyAgRGF0ZVRpbWVSYW5nZVBpY2tlci50c3ggLSBHYnRjXG4vL1xuLy8gIENvcHlyaWdodCDCqSAyMDE4LCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxuLy9cbi8vICBMaWNlbnNlZCB0byB0aGUgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlIChHUEEpIHVuZGVyIG9uZSBvciBtb3JlIGNvbnRyaWJ1dG9yIGxpY2Vuc2UgYWdyZWVtZW50cy4gU2VlXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxuLy8gIFRoZSBHUEEgbGljZW5zZXMgdGhpcyBmaWxlIHRvIHlvdSB1bmRlciB0aGUgTUlUIExpY2Vuc2UgKE1JVCksIHRoZSBcIkxpY2Vuc2VcIjsgeW91IG1heSBub3QgdXNlIHRoaXNcbi8vICBmaWxlIGV4Y2VwdCBpbiBjb21wbGlhbmNlIHdpdGggdGhlIExpY2Vuc2UuIFlvdSBtYXkgb2J0YWluIGEgY29weSBvZiB0aGUgTGljZW5zZSBhdDpcbi8vXG4vLyAgICAgIGh0dHA6Ly9vcGVuc291cmNlLm9yZy9saWNlbnNlcy9NSVRcbi8vXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxuLy8gIFwiQVMtSVNcIiBCQVNJUywgV0lUSE9VVCBXQVJSQU5USUVTIE9SIENPTkRJVElPTlMgT0YgQU5ZIEtJTkQsIGVpdGhlciBleHByZXNzIG9yIGltcGxpZWQuIFJlZmVyIHRvIHRoZVxuLy8gIExpY2Vuc2UgZm9yIHRoZSBzcGVjaWZpYyBsYW5ndWFnZSBnb3Zlcm5pbmcgcGVybWlzc2lvbnMgYW5kIGxpbWl0YXRpb25zLlxuLy9cbi8vICBDb2RlIE1vZGlmaWNhdGlvbiBIaXN0b3J5OlxuLy8gIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cbi8vICAwNy8yMy8yMDE4IC0gQmlsbHkgRXJuZXN0XG4vLyAgICAgICBHZW5lcmF0ZWQgb3JpZ2luYWwgdmVyc2lvbiBvZiBzb3VyY2UgY29kZS5cbi8vXG4vLyoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxuXG5pbXBvcnQgKiBhcyBSZWFjdCBmcm9tICdyZWFjdCc7XG5pbXBvcnQgKiBhcyBSZWFjdERPTSBmcm9tICdyZWFjdC1kb20nO1xuaW1wb3J0ICogYXMgXyBmcm9tIFwibG9kYXNoXCI7XG5pbXBvcnQgKiBhcyBEYXRlVGltZSBmcm9tIFwicmVhY3QtZGF0ZXRpbWVcIjtcbmltcG9ydCAqIGFzIG1vbWVudCBmcm9tICdtb21lbnQnO1xuaW1wb3J0ICdyZWFjdC1kYXRldGltZS9jc3MvcmVhY3QtZGF0ZXRpbWUuY3NzJztcblxuZXhwb3J0IGRlZmF1bHQgY2xhc3MgRGF0ZVRpbWVSYW5nZVBpY2tlciBleHRlbmRzIFJlYWN0LkNvbXBvbmVudDxhbnksIGFueT57XG4gICAgcHJvcHM6IHsgc3RhcnREYXRlOiBzdHJpbmcsIGVuZERhdGU6IHN0cmluZywgc3RhdGVTZXR0ZXI6IEZ1bmN0aW9uIH1cbiAgICBzdGF0ZTogeyBzdGFydERhdGU6IG1vbWVudC5Nb21lbnQsIGVuZERhdGU6IG1vbWVudC5Nb21lbnQgfVxuICAgIHN0YXRlU2V0dGVySWQ6IGFueTtcbiAgICBjb25zdHJ1Y3Rvcihwcm9wcykge1xuICAgICAgICBzdXBlcihwcm9wcyk7XG4gICAgICAgIHRoaXMuc3RhdGUgPSB7XG4gICAgICAgICAgICBzdGFydERhdGU6IG1vbWVudCh0aGlzLnByb3BzLnN0YXJ0RGF0ZSksXG4gICAgICAgICAgICBlbmREYXRlOiBtb21lbnQodGhpcy5wcm9wcy5lbmREYXRlKVxuICAgICAgICB9O1xuICAgIH1cbiAgICByZW5kZXIoKSB7XG4gICAgICAgIHJldHVybiAoXG4gICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImNvbnRhaW5lclwiIHN0eWxlPXt7d2lkdGg6ICdpbmhlcml0J319PlxuICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwicm93XCI+XG4gICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiZm9ybS1ncm91cFwiPlxuICAgICAgICAgICAgICAgICAgICAgICAgPERhdGVUaW1lXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgaXNWYWxpZERhdGU9eyhkYXRlKSA9PiB7IHJldHVybiBkYXRlLmlzQmVmb3JlKHRoaXMuc3RhdGUuZW5kRGF0ZSkgfX1cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB2YWx1ZT17dGhpcy5zdGF0ZS5zdGFydERhdGV9XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgdGltZUZvcm1hdD1cIkhIOm1tXCJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBvbkNoYW5nZT17KHZhbHVlKSA9PiB0aGlzLnNldFN0YXRlKHsgc3RhcnREYXRlOiB2YWx1ZSB9LCAoKSA9PiB0aGlzLnN0YXRlU2V0dGVyKCkpfSAvPlxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cInJvd1wiPlxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImZvcm0tZ3JvdXBcIj5cbiAgICAgICAgICAgICAgICAgICAgICAgIDxEYXRlVGltZVxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGlzVmFsaWREYXRlPXsoZGF0ZSkgPT4geyByZXR1cm4gZGF0ZS5pc0FmdGVyKHRoaXMuc3RhdGUuc3RhcnREYXRlKSB9fVxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHZhbHVlPXt0aGlzLnN0YXRlLmVuZERhdGV9XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgdGltZUZvcm1hdD1cIkhIOm1tXCJcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBvbkNoYW5nZT17KHZhbHVlKSA9PiB0aGlzLnNldFN0YXRlKHsgZW5kRGF0ZTogdmFsdWUgfSwgKCkgPT4gdGhpcy5zdGF0ZVNldHRlcigpKX0gLz5cbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgKTtcbiAgICB9XG5cbiAgICBjb21wb25lbnRXaWxsUmVjZWl2ZVByb3BzKG5leHRQcm9wcywgbmV4dENvbnRlbnQpIHtcbiAgICAgICAgaWYgKG5leHRQcm9wcy5zdGFydERhdGUgIT0gdGhpcy5zdGF0ZS5zdGFydERhdGUuZm9ybWF0KCdZWVlZLU1NLUREVEhIOm1tJykgfHwgbmV4dFByb3BzLmVuZERhdGUgIT0gdGhpcy5zdGF0ZS5lbmREYXRlLmZvcm1hdCgnWVlZWS1NTS1ERFRISDptbScpKVxuICAgICAgICAgICAgdGhpcy5zZXRTdGF0ZSh7IHN0YXJ0RGF0ZTogbW9tZW50KHRoaXMucHJvcHMuc3RhcnREYXRlKSwgZW5kRGF0ZTogbW9tZW50KHRoaXMucHJvcHMuZW5kRGF0ZSl9KTtcbiAgICB9XG5cbiAgICBzdGF0ZVNldHRlcigpIHtcbiAgICAgICAgY2xlYXJUaW1lb3V0KHRoaXMuc3RhdGVTZXR0ZXJJZCk7XG4gICAgICAgIHRoaXMuc3RhdGVTZXR0ZXJJZCA9IHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgICAgICAgdGhpcy5wcm9wcy5zdGF0ZVNldHRlcih7IHN0YXJ0RGF0ZTogdGhpcy5zdGF0ZS5zdGFydERhdGUuZm9ybWF0KCdZWVlZLU1NLUREVEhIOm1tJyksIGVuZERhdGU6IHRoaXMuc3RhdGUuZW5kRGF0ZS5mb3JtYXQoJ1lZWVktTU0tRERUSEg6bW0nKSB9KTtcbiAgICAgICAgfSwgNTAwKTtcbiAgICB9XG59XG5cblxuIiwiLy8qKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcbi8vICBNZWFzdXJlbWVudElucHV0LnRzeCAtIEdidGNcbi8vXG4vLyAgQ29weXJpZ2h0IMKpIDIwMTgsIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZS4gIEFsbCBSaWdodHMgUmVzZXJ2ZWQuXG4vL1xuLy8gIExpY2Vuc2VkIHRvIHRoZSBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UgKEdQQSkgdW5kZXIgb25lIG9yIG1vcmUgY29udHJpYnV0b3IgbGljZW5zZSBhZ3JlZW1lbnRzLiBTZWVcbi8vICB0aGUgTk9USUNFIGZpbGUgZGlzdHJpYnV0ZWQgd2l0aCB0aGlzIHdvcmsgZm9yIGFkZGl0aW9uYWwgaW5mb3JtYXRpb24gcmVnYXJkaW5nIGNvcHlyaWdodCBvd25lcnNoaXAuXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xuLy8gIGZpbGUgZXhjZXB0IGluIGNvbXBsaWFuY2Ugd2l0aCB0aGUgTGljZW5zZS4gWW91IG1heSBvYnRhaW4gYSBjb3B5IG9mIHRoZSBMaWNlbnNlIGF0OlxuLy9cbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxuLy9cbi8vICBVbmxlc3MgYWdyZWVkIHRvIGluIHdyaXRpbmcsIHRoZSBzdWJqZWN0IHNvZnR3YXJlIGRpc3RyaWJ1dGVkIHVuZGVyIHRoZSBMaWNlbnNlIGlzIGRpc3RyaWJ1dGVkIG9uIGFuXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXG4vLyAgTGljZW5zZSBmb3IgdGhlIHNwZWNpZmljIGxhbmd1YWdlIGdvdmVybmluZyBwZXJtaXNzaW9ucyBhbmQgbGltaXRhdGlvbnMuXG4vL1xuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XG4vLyAgLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxuLy8gIDA3LzE5LzIwMTggLSBCaWxseSBFcm5lc3Rcbi8vICAgICAgIEdlbmVyYXRlZCBvcmlnaW5hbCB2ZXJzaW9uIG9mIHNvdXJjZSBjb2RlLlxuLy9cbi8vKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXG5cbmltcG9ydCAqIGFzIFJlYWN0IGZyb20gJ3JlYWN0JztcbmltcG9ydCBUcmVuZGluZ0RhdGFEaXNwbGF5U2VydmljZSBmcm9tICcuLy4uL1RTL1NlcnZpY2VzL1RyZW5kaW5nRGF0YURpc3BsYXknO1xuaW1wb3J0ICogYXMgXyBmcm9tIFwibG9kYXNoXCI7XG5cbmV4cG9ydCBkZWZhdWx0IGNsYXNzIE1lYXN1cmVtZW50SW5wdXQgZXh0ZW5kcyBSZWFjdC5Db21wb25lbnQ8eyB2YWx1ZTogbnVtYmVyLCBtZXRlcklEOiBudW1iZXIsIG9uQ2hhbmdlOiBGdW5jdGlvbiB9LCB7IG9wdGlvbnM6IGFueVtdIH0+e1xuICAgIHRyZW5kaW5nRGF0YURpc3BsYXlTZXJ2aWNlOiBUcmVuZGluZ0RhdGFEaXNwbGF5U2VydmljZTtcbiAgICBjb25zdHJ1Y3Rvcihwcm9wcykge1xuICAgICAgICBzdXBlcihwcm9wcyk7XG4gICAgICAgIHRoaXMuc3RhdGUgPSB7XG4gICAgICAgICAgICBvcHRpb25zOiBbXVxuICAgICAgICB9XG5cbiAgICAgICAgdGhpcy50cmVuZGluZ0RhdGFEaXNwbGF5U2VydmljZSA9IG5ldyBUcmVuZGluZ0RhdGFEaXNwbGF5U2VydmljZSgpO1xuICAgIH1cblxuICAgIGNvbXBvbmVudFdpbGxSZWNlaXZlUHJvcHMobmV4dFByb3BzKSB7XG4gICAgICAgIGlmKHRoaXMucHJvcHMubWV0ZXJJRCAhPSBuZXh0UHJvcHMubWV0ZXJJRClcbiAgICAgICAgICAgIHRoaXMuZ2V0RGF0YShuZXh0UHJvcHMubWV0ZXJJRCk7XG4gICAgfVxuXG4gICAgY29tcG9uZW50RGlkTW91bnQoKSB7XG4gICAgICAgIHRoaXMuZ2V0RGF0YSh0aGlzLnByb3BzLm1ldGVySUQpO1xuICAgIH1cblxuICAgIGdldERhdGEobWV0ZXJJRCkge1xuICAgICAgICB0aGlzLnRyZW5kaW5nRGF0YURpc3BsYXlTZXJ2aWNlLmdldE1lYXN1cmVtZW50cyhtZXRlcklEKS5kb25lKGRhdGEgPT4ge1xuICAgICAgICAgICAgaWYgKGRhdGEubGVuZ3RoID09IDApIHJldHVybjtcblxuICAgICAgICAgICAgdmFyIHZhbHVlID0gKHRoaXMucHJvcHMudmFsdWUgPyB0aGlzLnByb3BzLnZhbHVlIDogZGF0YVswXS5JRClcbiAgICAgICAgICAgIHZhciBvcHRpb25zID0gZGF0YS5tYXAoZCA9PiA8b3B0aW9uIGtleT17ZC5JRH0gdmFsdWU9e2QuSUR9PntkLk5hbWV9PC9vcHRpb24+KTtcbiAgICAgICAgICAgIHRoaXMuc2V0U3RhdGUoeyBvcHRpb25zIH0pO1xuICAgICAgICB9KTtcblxuICAgIH1cblxuICAgIHJlbmRlcigpIHtcbiAgICAgICAgcmV0dXJuICg8c2VsZWN0IGNsYXNzTmFtZT0nZm9ybS1jb250cm9sJyBvbkNoYW5nZT17KGUpID0+IHsgdGhpcy5wcm9wcy5vbkNoYW5nZSh7IG1lYXN1cmVtZW50SUQ6IHBhcnNlSW50KGUudGFyZ2V0LnZhbHVlKSwgbWVhc3VyZW1lbnROYW1lOiBlLnRhcmdldC5zZWxlY3RlZE9wdGlvbnNbMF0udGV4dCB9KTsgfX0gdmFsdWU9e3RoaXMucHJvcHMudmFsdWV9PlxuICAgICAgICAgICAgPG9wdGlvbiB2YWx1ZT0nMCc+PC9vcHRpb24+XG4gICAgICAgICAgICB7dGhpcy5zdGF0ZS5vcHRpb25zfVxuICAgICAgICA8L3NlbGVjdD4pO1xuICAgIH1cblxufSIsIi8vKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXG4vLyAgTWV0ZXJJbnB1dC50c3ggLSBHYnRjXG4vL1xuLy8gIENvcHlyaWdodCDCqSAyMDE4LCBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UuICBBbGwgUmlnaHRzIFJlc2VydmVkLlxuLy9cbi8vICBMaWNlbnNlZCB0byB0aGUgR3JpZCBQcm90ZWN0aW9uIEFsbGlhbmNlIChHUEEpIHVuZGVyIG9uZSBvciBtb3JlIGNvbnRyaWJ1dG9yIGxpY2Vuc2UgYWdyZWVtZW50cy4gU2VlXG4vLyAgdGhlIE5PVElDRSBmaWxlIGRpc3RyaWJ1dGVkIHdpdGggdGhpcyB3b3JrIGZvciBhZGRpdGlvbmFsIGluZm9ybWF0aW9uIHJlZ2FyZGluZyBjb3B5cmlnaHQgb3duZXJzaGlwLlxuLy8gIFRoZSBHUEEgbGljZW5zZXMgdGhpcyBmaWxlIHRvIHlvdSB1bmRlciB0aGUgTUlUIExpY2Vuc2UgKE1JVCksIHRoZSBcIkxpY2Vuc2VcIjsgeW91IG1heSBub3QgdXNlIHRoaXNcbi8vICBmaWxlIGV4Y2VwdCBpbiBjb21wbGlhbmNlIHdpdGggdGhlIExpY2Vuc2UuIFlvdSBtYXkgb2J0YWluIGEgY29weSBvZiB0aGUgTGljZW5zZSBhdDpcbi8vXG4vLyAgICAgIGh0dHA6Ly9vcGVuc291cmNlLm9yZy9saWNlbnNlcy9NSVRcbi8vXG4vLyAgVW5sZXNzIGFncmVlZCB0byBpbiB3cml0aW5nLCB0aGUgc3ViamVjdCBzb2Z0d2FyZSBkaXN0cmlidXRlZCB1bmRlciB0aGUgTGljZW5zZSBpcyBkaXN0cmlidXRlZCBvbiBhblxuLy8gIFwiQVMtSVNcIiBCQVNJUywgV0lUSE9VVCBXQVJSQU5USUVTIE9SIENPTkRJVElPTlMgT0YgQU5ZIEtJTkQsIGVpdGhlciBleHByZXNzIG9yIGltcGxpZWQuIFJlZmVyIHRvIHRoZVxuLy8gIExpY2Vuc2UgZm9yIHRoZSBzcGVjaWZpYyBsYW5ndWFnZSBnb3Zlcm5pbmcgcGVybWlzc2lvbnMgYW5kIGxpbWl0YXRpb25zLlxuLy9cbi8vICBDb2RlIE1vZGlmaWNhdGlvbiBIaXN0b3J5OlxuLy8gIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cbi8vICAwNS8yNS8yMDE4IC0gQmlsbHkgRXJuZXN0XG4vLyAgICAgICBHZW5lcmF0ZWQgb3JpZ2luYWwgdmVyc2lvbiBvZiBzb3VyY2UgY29kZS5cbi8vXG4vLyoqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKlxuXG5pbXBvcnQgKiBhcyBSZWFjdCBmcm9tICdyZWFjdCc7XG5pbXBvcnQgUGVyaW9kaWNEYXRhRGlzcGxheVNlcnZpY2UgZnJvbSAnLi8uLi9UUy9TZXJ2aWNlcy9QZXJpb2RpY0RhdGFEaXNwbGF5JztcbmltcG9ydCAqIGFzIF8gZnJvbSBcImxvZGFzaFwiO1xuXG50eXBlIG1ldGVycyA9ICBBcnJheTx7IElEOiBudW1iZXIsIE5hbWU6IHN0cmluZyB9PjtcblxuZXhwb3J0IGRlZmF1bHQgY2xhc3MgTWV0ZXJJbnB1dCBleHRlbmRzIFJlYWN0LkNvbXBvbmVudDx7IHZhbHVlOiBudW1iZXIsIG9uQ2hhbmdlOiBGdW5jdGlvbiB9LCB7IG9wdGlvbnM6IEpTWC5FbGVtZW50W10gfT57XG4gICAgcGVyaW9kaWNEYXRhRGlzcGxheVNlcnZpY2U6IFBlcmlvZGljRGF0YURpc3BsYXlTZXJ2aWNlO1xuICAgIGNvbnN0cnVjdG9yKHByb3BzKSB7XG4gICAgICAgIHN1cGVyKHByb3BzKTtcblxuICAgICAgICB0aGlzLnN0YXRlID0ge1xuICAgICAgICAgICAgb3B0aW9uczogW11cbiAgICAgICAgfVxuXG4gICAgICAgIHRoaXMucGVyaW9kaWNEYXRhRGlzcGxheVNlcnZpY2UgPSBuZXcgUGVyaW9kaWNEYXRhRGlzcGxheVNlcnZpY2UoKTtcbiAgICB9XG5cbiAgICBjb21wb25lbnREaWRNb3VudCgpIHtcblxuICAgICAgICB0aGlzLnBlcmlvZGljRGF0YURpc3BsYXlTZXJ2aWNlLmdldE1ldGVycygpLmRvbmUoZGF0YSA9PiB7XG4gICAgICAgICAgICB0aGlzLnNldFN0YXRlKHsgb3B0aW9uczogZGF0YS5tYXAoZCA9PiA8b3B0aW9uIGtleT17ZC5JRH0gdmFsdWU9e2QuSUR9PntkLk5hbWV9PC9vcHRpb24+KSB9KTtcbiAgICAgICAgfSk7XG4gICAgfVxuXG4gICAgcmVuZGVyKCkge1xuICAgICAgICByZXR1cm4gKFxuICAgICAgICAgICAgPHNlbGVjdCBjbGFzc05hbWU9J2Zvcm0tY29udHJvbCcgb25DaGFuZ2U9eyhlKSA9PiB7IHRoaXMucHJvcHMub25DaGFuZ2UoeyBtZXRlcklEOiBwYXJzZUludChlLnRhcmdldC52YWx1ZSksIG1ldGVyTmFtZTogZS50YXJnZXQuc2VsZWN0ZWRPcHRpb25zWzBdLnRleHQsIG1lYXN1cmVtZW50SUQ6IG51bGwgfSk7IH19IHZhbHVlPXt0aGlzLnByb3BzLnZhbHVlfT5cbiAgICAgICAgICAgICAgICA8b3B0aW9uIHZhbHVlPScwJz48L29wdGlvbj5cbiAgICAgICAgICAgICAgICB7dGhpcy5zdGF0ZS5vcHRpb25zfVxuICAgICAgICAgICAgPC9zZWxlY3Q+KTtcbiAgICB9XG59IiwiLy8qKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcbi8vICBUcmVuZGluZ0NoYXJ0LnRzeCAtIEdidGNcbi8vXG4vLyAgQ29weXJpZ2h0IMKpIDIwMTgsIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZS4gIEFsbCBSaWdodHMgUmVzZXJ2ZWQuXG4vL1xuLy8gIExpY2Vuc2VkIHRvIHRoZSBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UgKEdQQSkgdW5kZXIgb25lIG9yIG1vcmUgY29udHJpYnV0b3IgbGljZW5zZSBhZ3JlZW1lbnRzLiBTZWVcbi8vICB0aGUgTk9USUNFIGZpbGUgZGlzdHJpYnV0ZWQgd2l0aCB0aGlzIHdvcmsgZm9yIGFkZGl0aW9uYWwgaW5mb3JtYXRpb24gcmVnYXJkaW5nIGNvcHlyaWdodCBvd25lcnNoaXAuXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xuLy8gIGZpbGUgZXhjZXB0IGluIGNvbXBsaWFuY2Ugd2l0aCB0aGUgTGljZW5zZS4gWW91IG1heSBvYnRhaW4gYSBjb3B5IG9mIHRoZSBMaWNlbnNlIGF0OlxuLy9cbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxuLy9cbi8vICBVbmxlc3MgYWdyZWVkIHRvIGluIHdyaXRpbmcsIHRoZSBzdWJqZWN0IHNvZnR3YXJlIGRpc3RyaWJ1dGVkIHVuZGVyIHRoZSBMaWNlbnNlIGlzIGRpc3RyaWJ1dGVkIG9uIGFuXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXG4vLyAgTGljZW5zZSBmb3IgdGhlIHNwZWNpZmljIGxhbmd1YWdlIGdvdmVybmluZyBwZXJtaXNzaW9ucyBhbmQgbGltaXRhdGlvbnMuXG4vL1xuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XG4vLyAgLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxuLy8gIDA3LzE5LzIwMTggLSBCaWxseSBFcm5lc3Rcbi8vICAgICAgIEdlbmVyYXRlZCBvcmlnaW5hbCB2ZXJzaW9uIG9mIHNvdXJjZSBjb2RlLlxuLy9cbi8vKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXG5cbmltcG9ydCAqIGFzIFJlYWN0IGZyb20gJ3JlYWN0JztcbiBpbXBvcnQgKiBhcyBtb21lbnQgZnJvbSAnbW9tZW50JztcbmltcG9ydCAqIGFzIF8gZnJvbSBcImxvZGFzaFwiO1xuaW1wb3J0ICcuLy4uL2Zsb3QvanF1ZXJ5LmZsb3QubWluLmpzJztcbmltcG9ydCAnLi8uLi9mbG90L2pxdWVyeS5mbG90LmNyb3NzaGFpci5taW4uanMnO1xuaW1wb3J0ICcuLy4uL2Zsb3QvanF1ZXJ5LmZsb3QubmF2aWdhdGUubWluLmpzJztcbmltcG9ydCAnLi8uLi9mbG90L2pxdWVyeS5mbG90LnNlbGVjdGlvbi5taW4uanMnO1xuaW1wb3J0ICcuLy4uL2Zsb3QvanF1ZXJ5LmZsb3QudGltZS5taW4uanMnO1xuaW1wb3J0ICcuLy4uL2Zsb3QvanF1ZXJ5LmZsb3QuYXhpc2xhYmVscy5qcyc7XG5cbmltcG9ydCB7IFRyZW5kaW5nY0RhdGFEaXNwbGF5IH0gZnJvbSAnLi9nbG9iYWwnXG5cbmludGVyZmFjZSBQcm9wcyB7IHN0YXJ0RGF0ZTogc3RyaW5nLCBlbmREYXRlOiBzdHJpbmcsIGRhdGE6IFRyZW5kaW5nY0RhdGFEaXNwbGF5Lk1lYXN1cmVtZW50W10sIGF4ZXM6IFRyZW5kaW5nY0RhdGFEaXNwbGF5LkZsb3RBeGlzW10sc3RhdGVTZXR0ZXI6IEZ1bmN0aW9uIH1cbmV4cG9ydCBkZWZhdWx0IGNsYXNzIFRyZW5kaW5nQ2hhcnQgZXh0ZW5kcyBSZWFjdC5Db21wb25lbnQ8UHJvcHMsIHt9PntcbiAgICBwbG90OiBhbnk7XG4gICAgem9vbUlkOiBhbnk7XG4gICAgaG92ZXI6IG51bWJlcjtcbiAgICBzdGFydERhdGU6IHN0cmluZztcbiAgICBlbmREYXRlOiBzdHJpbmc7XG4gICAgb3B0aW9uczogeyBjYW52YXM6IGJvb2xlYW4sIGxlZ2VuZDogb2JqZWN0LCBjcm9zc2hhaXI6IG9iamVjdCwgc2VsZWN0aW9uOiBvYmplY3QsIGdyaWQ6IG9iamVjdCwgeGF4aXM6IHttb2RlOiBzdHJpbmcsIHRpY2tMZW5ndGg6IG51bWJlciwgcmVzZXJ2ZVNwYWNlOiBib29sZWFuLCB0aWNrczogRnVuY3Rpb24sIHRpY2tGb3JtYXR0ZXI6IEZ1bmN0aW9uLCBtYXg6IG51bWJlciwgbWluOiBudW1iZXJ9LCB5YXhpczogb2JqZWN0LCB5YXhlczogb2JqZWN0W10gfVxuXG4gICAgY29uc3RydWN0b3IocHJvcHMpIHtcbiAgICAgICAgc3VwZXIocHJvcHMpO1xuXG4gICAgICAgIHRoaXMuaG92ZXIgPSAwO1xuICAgICAgICB0aGlzLnN0YXJ0RGF0ZSA9IHByb3BzLnN0YXJ0RGF0ZTtcbiAgICAgICAgdGhpcy5lbmREYXRlID0gcHJvcHMuZW5kRGF0ZTtcblxuICAgICAgICB2YXIgY3RybCA9IHRoaXM7XG5cbiAgICAgICAgdGhpcy5vcHRpb25zID0ge1xuICAgICAgICAgICAgY2FudmFzOiB0cnVlLFxuICAgICAgICAgICAgbGVnZW5kOiB7IHNob3c6IHRydWUgfSxcbiAgICAgICAgICAgIGNyb3NzaGFpcjogeyBtb2RlOiBcInhcIiB9LFxuICAgICAgICAgICAgc2VsZWN0aW9uOiB7IG1vZGU6IFwieFwiIH0sXG4gICAgICAgICAgICBncmlkOiB7XG4gICAgICAgICAgICAgICAgYXV0b0hpZ2hsaWdodDogZmFsc2UsXG4gICAgICAgICAgICAgICAgY2xpY2thYmxlOiB0cnVlLFxuICAgICAgICAgICAgICAgIGhvdmVyYWJsZTogdHJ1ZSxcbiAgICAgICAgICAgICAgICBtYXJraW5nczogW11cbiAgICAgICAgICAgIH0sXG4gICAgICAgICAgICB4YXhpczoge1xuICAgICAgICAgICAgICAgIG1vZGU6IFwidGltZVwiLFxuICAgICAgICAgICAgICAgIHRpY2tMZW5ndGg6IDEwLFxuICAgICAgICAgICAgICAgIHJlc2VydmVTcGFjZTogZmFsc2UsXG4gICAgICAgICAgICAgICAgdGlja3M6IGZ1bmN0aW9uIChheGlzKSB7XG4gICAgICAgICAgICAgICAgICAgIHZhciB0aWNrcyA9IFtdLFxuICAgICAgICAgICAgICAgICAgICAgICAgc3RhcnQgPSBjdHJsLmZsb29ySW5CYXNlKGF4aXMubWluLCBheGlzLmRlbHRhKSxcbiAgICAgICAgICAgICAgICAgICAgICAgIGkgPSAwLFxuICAgICAgICAgICAgICAgICAgICAgICAgdiA9IE51bWJlci5OYU4sXG4gICAgICAgICAgICAgICAgICAgICAgICBwcmV2O1xuXG4gICAgICAgICAgICAgICAgICAgIGRvIHtcbiAgICAgICAgICAgICAgICAgICAgICAgIHByZXYgPSB2O1xuICAgICAgICAgICAgICAgICAgICAgICAgdiA9IHN0YXJ0ICsgaSAqIGF4aXMuZGVsdGE7XG4gICAgICAgICAgICAgICAgICAgICAgICB0aWNrcy5wdXNoKHYpO1xuICAgICAgICAgICAgICAgICAgICAgICAgKytpO1xuICAgICAgICAgICAgICAgICAgICB9IHdoaWxlICh2IDwgYXhpcy5tYXggJiYgdiAhPSBwcmV2KTtcbiAgICAgICAgICAgICAgICAgICAgcmV0dXJuIHRpY2tzO1xuICAgICAgICAgICAgICAgIH0sXG4gICAgICAgICAgICAgICAgdGlja0Zvcm1hdHRlcjogZnVuY3Rpb24gKHZhbHVlLCBheGlzKSB7XG4gICAgICAgICAgICAgICAgICAgIGlmIChheGlzLmRlbHRhID4gMyAqIDI0ICogNjAgKiA2MCAqIDEwMDApXG4gICAgICAgICAgICAgICAgICAgICAgICByZXR1cm4gbW9tZW50KHZhbHVlKS51dGMoKS5mb3JtYXQoXCJNTS9ERFwiKTtcbiAgICAgICAgICAgICAgICAgICAgZWxzZVxuICAgICAgICAgICAgICAgICAgICAgICAgcmV0dXJuIG1vbWVudCh2YWx1ZSkudXRjKCkuZm9ybWF0KFwiTU0vREQgSEg6bW1cIik7XG4gICAgICAgICAgICAgICAgfSxcbiAgICAgICAgICAgICAgICBtYXg6IG51bGwsXG4gICAgICAgICAgICAgICAgbWluOiBudWxsXG4gICAgICAgICAgICB9LFxuICAgICAgICAgICAgeWF4aXM6IHtcbiAgICAgICAgICAgICAgICBsYWJlbFdpZHRoOiA1MCxcbiAgICAgICAgICAgICAgICBwYW5SYW5nZTogZmFsc2UsXG4gICAgICAgICAgICAgICAgLy90aWNrczogMSxcbiAgICAgICAgICAgICAgICB0aWNrTGVuZ3RoOiAxMCxcbiAgICAgICAgICAgICAgICB0aWNrRm9ybWF0dGVyOiBmdW5jdGlvbiAodmFsLCBheGlzKSB7XG4gICAgICAgICAgICAgICAgICAgIGlmIChheGlzLmRlbHRhID4gMTAwMDAwMCAmJiAodmFsID4gMTAwMDAwMCB8fCB2YWwgPCAtMTAwMDAwMCkpXG4gICAgICAgICAgICAgICAgICAgICAgICByZXR1cm4gKCh2YWwgLyAxMDAwMDAwKSB8IDApICsgXCJNXCI7XG4gICAgICAgICAgICAgICAgICAgIGVsc2UgaWYgKGF4aXMuZGVsdGEgPiAxMDAwICYmICh2YWwgPiAxMDAwIHx8IHZhbCA8IC0xMDAwKSlcbiAgICAgICAgICAgICAgICAgICAgICAgIHJldHVybiAoKHZhbCAvIDEwMDApIHwgMCkgKyBcIktcIjtcbiAgICAgICAgICAgICAgICAgICAgZWxzZVxuICAgICAgICAgICAgICAgICAgICAgICAgcmV0dXJuIHZhbC50b0ZpeGVkKGF4aXMudGlja0RlY2ltYWxzKTtcbiAgICAgICAgICAgICAgICB9XG4gICAgICAgICAgICB9LFxuICAgICAgICAgICAgeWF4ZXM6IFtdXG4gICAgICAgIH1cblxuICAgIH1cblxuXG4gICAgY3JlYXRlRGF0YVJvd3MocHJvcHM6IFByb3BzKSB7XG4gICAgICAgIC8vIGlmIHN0YXJ0IGFuZCBlbmQgZGF0ZSBhcmUgbm90IHByb3ZpZGVkIGNhbGN1bGF0ZSB0aGVtIGZyb20gdGhlIGRhdGEgc2V0XG4gICAgICAgIGlmICh0aGlzLnBsb3QgIT0gdW5kZWZpbmVkKSAkKHRoaXMucmVmcy5ncmFwaCkuY2hpbGRyZW4oKS5yZW1vdmUoKTtcblxuICAgICAgICB2YXIgc3RhcnRTdHJpbmcgPSB0aGlzLnN0YXJ0RGF0ZTtcbiAgICAgICAgdmFyIGVuZFN0cmluZyA9IHRoaXMuZW5kRGF0ZTtcbiAgICAgICAgdmFyIG5ld1Zlc3NlbCA9IFtdO1xuXG4gICAgICAgIGlmIChwcm9wcy5kYXRhICE9IG51bGwpIHtcbiAgICAgICAgICAgICQuZWFjaChwcm9wcy5kYXRhLCAoaSwgbWVhc3VyZW1lbnQpID0+IHtcbiAgICAgICAgICAgICAgICBpZihtZWFzdXJlbWVudC5NYXhpbXVtICYmIG1lYXN1cmVtZW50LkRhdGE/Lmxlbmd0aCA+IDApXG4gICAgICAgICAgICAgICAgICAgIG5ld1Zlc3NlbC5wdXNoKHsgbGFiZWw6IGAke21lYXN1cmVtZW50Lk1lYXN1cmVtZW50TmFtZX0tTWF4YCwgZGF0YTogbWVhc3VyZW1lbnQuRGF0YS5tYXAoZCA9PiBbZC5UaW1lLGQuTWF4aW11bV0pLCBjb2xvcjogbWVhc3VyZW1lbnQuTWF4Q29sb3IsIHlheGlzOiBtZWFzdXJlbWVudC5BeGlzIH0pXG4gICAgICAgICAgICAgICAgaWYgKG1lYXN1cmVtZW50LkF2ZXJhZ2UgJiYgbWVhc3VyZW1lbnQuRGF0YT8ubGVuZ3RoID4gMClcbiAgICAgICAgICAgICAgICAgICAgbmV3VmVzc2VsLnB1c2goeyBsYWJlbDogYCR7bWVhc3VyZW1lbnQuTWVhc3VyZW1lbnROYW1lfS1BdmdgLCBkYXRhOiBtZWFzdXJlbWVudC5EYXRhLm1hcChkID0+IFtkLlRpbWUsIGQuQXZlcmFnZV0pLCBjb2xvcjogbWVhc3VyZW1lbnQuQXZnQ29sb3IsIHlheGlzOiBtZWFzdXJlbWVudC5BeGlzICB9KVxuICAgICAgICAgICAgICAgIGlmIChtZWFzdXJlbWVudC5NaW5pbXVtICYmIG1lYXN1cmVtZW50LkRhdGE/Lmxlbmd0aCA+IDApXG4gICAgICAgICAgICAgICAgICAgIG5ld1Zlc3NlbC5wdXNoKHsgbGFiZWw6IGAke21lYXN1cmVtZW50Lk1lYXN1cmVtZW50TmFtZX0tTWluYCwgZGF0YTogbWVhc3VyZW1lbnQuRGF0YS5tYXAoZCA9PiBbZC5UaW1lLCBkLk1pbmltdW1dKSwgY29sb3I6IG1lYXN1cmVtZW50Lk1pbkNvbG9yLCB5YXhpczogbWVhc3VyZW1lbnQuQXhpcyAgfSlcblxuICAgICAgICAgICAgfSk7XG4gICAgICAgIH1cbiAgICAgICAgbmV3VmVzc2VsLnB1c2goW1t0aGlzLmdldE1pbGxpc2Vjb25kVGltZShzdGFydFN0cmluZyksIG51bGxdLCBbdGhpcy5nZXRNaWxsaXNlY29uZFRpbWUoZW5kU3RyaW5nKSwgbnVsbF1dKTtcbiAgICAgICAgdGhpcy5vcHRpb25zLnhheGlzLm1heCA9IHRoaXMuZ2V0TWlsbGlzZWNvbmRUaW1lKGVuZFN0cmluZyk7XG4gICAgICAgIHRoaXMub3B0aW9ucy54YXhpcy5taW4gPSB0aGlzLmdldE1pbGxpc2Vjb25kVGltZShzdGFydFN0cmluZyk7XG4gICAgICAgIHRoaXMub3B0aW9ucy55YXhlcyA9IHRoaXMucHJvcHMuYXhlcztcbiAgICAgICAgdGhpcy5wbG90ID0gKCQgYXMgYW55KS5wbG90KCQodGhpcy5yZWZzLmdyYXBoKSwgbmV3VmVzc2VsLCB0aGlzLm9wdGlvbnMpO1xuICAgICAgICB0aGlzLnBsb3RTZWxlY3RlZCgpO1xuICAgICAgICB0aGlzLnBsb3Rab29tKCk7XG4gICAgICAgIHRoaXMucGxvdEhvdmVyKCk7XG4gICAgICAgIC8vdGhpcy5wbG90Q2xpY2soKTtcbiAgICB9XG5cbiAgICBjb21wb25lbnREaWRNb3VudCgpIHtcbiAgICAgICAgdGhpcy5jcmVhdGVEYXRhUm93cyh0aGlzLnByb3BzKTtcbiAgICB9XG5cbiAgICBjb21wb25lbnRXaWxsUmVjZWl2ZVByb3BzKG5leHRQcm9wcykge1xuICAgICAgICB0aGlzLnN0YXJ0RGF0ZSA9IG5leHRQcm9wcy5zdGFydERhdGU7XG4gICAgICAgIHRoaXMuZW5kRGF0ZSA9IG5leHRQcm9wcy5lbmREYXRlO1xuICAgICAgICB0aGlzLmNyZWF0ZURhdGFSb3dzKG5leHRQcm9wcyk7XG4gICAgfVxuXG4gICAgcmVuZGVyKCkge1xuICAgICAgICByZXR1cm4gPGRpdiByZWY9eydncmFwaCd9IHN0eWxlPXt7IGhlaWdodDogJ2luaGVyaXQnLCB3aWR0aDogJ2luaGVyaXQnfX0+PC9kaXY+O1xuICAgIH1cblxuICAgIC8vIHJvdW5kIHRvIG5lYXJieSBsb3dlciBtdWx0aXBsZSBvZiBiYXNlXG4gICAgZmxvb3JJbkJhc2UobiwgYmFzZSkge1xuICAgICAgICByZXR1cm4gYmFzZSAqIE1hdGguZmxvb3IobiAvIGJhc2UpO1xuICAgIH1cblxuICAgIGdldE1pbGxpc2Vjb25kVGltZShkYXRlKSB7XG4gICAgICAgIHZhciBtaWxsaXNlY29uZHMgPSBtb21lbnQudXRjKGRhdGUpLnZhbHVlT2YoKTtcbiAgICAgICAgcmV0dXJuIG1pbGxpc2Vjb25kcztcbiAgICB9XG5cbiAgICBnZXREYXRlU3RyaW5nKGZsb2F0KSB7XG4gICAgICAgIHZhciBkYXRlID0gbW9tZW50LnV0YyhmbG9hdCkuZm9ybWF0KCdZWVlZLU1NLUREVEhIOm1tJyk7XG4gICAgICAgIHJldHVybiBkYXRlO1xuICAgIH1cblxuICAgIHBsb3RTZWxlY3RlZCgpIHtcbiAgICAgICAgdmFyIGN0cmwgPSB0aGlzO1xuICAgICAgICAkKHRoaXMucmVmcy5ncmFwaCkub2ZmKFwicGxvdHNlbGVjdGVkXCIpO1xuICAgICAgICAkKHRoaXMucmVmcy5ncmFwaCkuYmluZChcInBsb3RzZWxlY3RlZFwiLCBmdW5jdGlvbiAoZXZlbnQsIHJhbmdlcykge1xuICAgICAgICAgICAgY3RybC5wcm9wcy5zdGF0ZVNldHRlcih7IHN0YXJ0RGF0ZTogY3RybC5nZXREYXRlU3RyaW5nKHJhbmdlcy54YXhpcy5mcm9tKSwgZW5kRGF0ZTogY3RybC5nZXREYXRlU3RyaW5nKHJhbmdlcy54YXhpcy50bykgfSk7XG4gICAgICAgIH0pO1xuICAgIH1cblxuICAgIHBsb3Rab29tKCkge1xuICAgICAgICB2YXIgY3RybCA9IHRoaXM7XG4gICAgICAgICQodGhpcy5yZWZzLmdyYXBoKS5vZmYoXCJwbG90em9vbVwiKTtcbiAgICAgICAgJCh0aGlzLnJlZnMuZ3JhcGgpLmJpbmQoXCJwbG90em9vbVwiLCBmdW5jdGlvbiAoZXZlbnQpIHtcbiAgICAgICAgICAgIHZhciBtaW5EZWx0YSA9IG51bGw7XG4gICAgICAgICAgICB2YXIgbWF4RGVsdGEgPSA1O1xuICAgICAgICAgICAgdmFyIHhheGlzID0gY3RybC5wbG90LmdldEF4ZXMoKS54YXhpcztcbiAgICAgICAgICAgIHZhciB4Y2VudGVyID0gY3RybC5ob3ZlcjtcbiAgICAgICAgICAgIHZhciB4bWluID0geGF4aXMub3B0aW9ucy5taW47XG4gICAgICAgICAgICB2YXIgeG1heCA9IHhheGlzLm9wdGlvbnMubWF4O1xuICAgICAgICAgICAgdmFyIGRhdGFtaW4gPSB4YXhpcy5kYXRhbWluO1xuICAgICAgICAgICAgdmFyIGRhdGFtYXggPSB4YXhpcy5kYXRhbWF4O1xuXG4gICAgICAgICAgICB2YXIgZGVsdGFNYWduaXR1ZGU7XG4gICAgICAgICAgICB2YXIgZGVsdGE7XG4gICAgICAgICAgICB2YXIgZmFjdG9yO1xuXG4gICAgICAgICAgICBpZiAoeG1pbiA9PSBudWxsKVxuICAgICAgICAgICAgICAgIHhtaW4gPSBkYXRhbWluO1xuXG4gICAgICAgICAgICBpZiAoeG1heCA9PSBudWxsKVxuICAgICAgICAgICAgICAgIHhtYXggPSBkYXRhbWF4O1xuXG4gICAgICAgICAgICBpZiAoeG1pbiA9PSBudWxsIHx8IHhtYXggPT0gbnVsbClcbiAgICAgICAgICAgICAgICByZXR1cm47XG5cbiAgICAgICAgICAgIHhjZW50ZXIgPSBNYXRoLm1heCh4Y2VudGVyLCB4bWluKTtcbiAgICAgICAgICAgIHhjZW50ZXIgPSBNYXRoLm1pbih4Y2VudGVyLCB4bWF4KTtcblxuICAgICAgICAgICAgaWYgKChldmVudC5vcmlnaW5hbEV2ZW50IGFzIGFueSkud2hlZWxEZWx0YSAhPSB1bmRlZmluZWQpXG4gICAgICAgICAgICAgICAgZGVsdGEgPSAoZXZlbnQub3JpZ2luYWxFdmVudCBhcyBhbnkpLndoZWVsRGVsdGE7XG4gICAgICAgICAgICBlbHNlXG4gICAgICAgICAgICAgICAgZGVsdGEgPSAtKGV2ZW50Lm9yaWdpbmFsRXZlbnQgYXMgYW55KS5kZXRhaWw7XG5cbiAgICAgICAgICAgIGRlbHRhTWFnbml0dWRlID0gTWF0aC5hYnMoZGVsdGEpO1xuXG4gICAgICAgICAgICBpZiAobWluRGVsdGEgPT0gbnVsbCB8fCBkZWx0YU1hZ25pdHVkZSA8IG1pbkRlbHRhKVxuICAgICAgICAgICAgICAgIG1pbkRlbHRhID0gZGVsdGFNYWduaXR1ZGU7XG5cbiAgICAgICAgICAgIGRlbHRhTWFnbml0dWRlIC89IG1pbkRlbHRhO1xuICAgICAgICAgICAgZGVsdGFNYWduaXR1ZGUgPSBNYXRoLm1pbihkZWx0YU1hZ25pdHVkZSwgbWF4RGVsdGEpO1xuICAgICAgICAgICAgZmFjdG9yID0gZGVsdGFNYWduaXR1ZGUgLyAxMDtcblxuICAgICAgICAgICAgaWYgKGRlbHRhID4gMCkge1xuICAgICAgICAgICAgICAgIHhtaW4gPSB4bWluICogKDEgLSBmYWN0b3IpICsgeGNlbnRlciAqIGZhY3RvcjtcbiAgICAgICAgICAgICAgICB4bWF4ID0geG1heCAqICgxIC0gZmFjdG9yKSArIHhjZW50ZXIgKiBmYWN0b3I7XG4gICAgICAgICAgICB9IGVsc2Uge1xuICAgICAgICAgICAgICAgIHhtaW4gPSAoeG1pbiAtIHhjZW50ZXIgKiBmYWN0b3IpIC8gKDEgLSBmYWN0b3IpO1xuICAgICAgICAgICAgICAgIHhtYXggPSAoeG1heCAtIHhjZW50ZXIgKiBmYWN0b3IpIC8gKDEgLSBmYWN0b3IpO1xuICAgICAgICAgICAgfVxuXG4gICAgICAgICAgICBpZiAoeG1pbiA9PSB4YXhpcy5vcHRpb25zLnhtaW4gJiYgeG1heCA9PSB4YXhpcy5vcHRpb25zLnhtYXgpXG4gICAgICAgICAgICAgICAgcmV0dXJuO1xuXG4gICAgICAgICAgICBjdHJsLnN0YXJ0RGF0ZSA9IGN0cmwuZ2V0RGF0ZVN0cmluZyh4bWluKTtcbiAgICAgICAgICAgIGN0cmwuZW5kRGF0ZSA9IGN0cmwuZ2V0RGF0ZVN0cmluZyh4bWF4KTtcblxuICAgICAgICAgICAgY3RybC5jcmVhdGVEYXRhUm93cyhjdHJsLnByb3BzKTtcblxuICAgICAgICAgICAgY2xlYXJUaW1lb3V0KGN0cmwuem9vbUlkKTtcblxuICAgICAgICAgICAgY3RybC56b29tSWQgPSBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgICAgICAgICAgICBjdHJsLnByb3BzLnN0YXRlU2V0dGVyKHsgc3RhcnREYXRlOiBjdHJsLmdldERhdGVTdHJpbmcoeG1pbiksIGVuZERhdGU6IGN0cmwuZ2V0RGF0ZVN0cmluZyh4bWF4KSB9KTtcbiAgICAgICAgICAgIH0sIDI1MCk7XG4gICAgICAgIH0pO1xuXG4gICAgfVxuXG4gICAgcGxvdEhvdmVyKCkge1xuICAgICAgICB2YXIgY3RybCA9IHRoaXM7XG4gICAgICAgICQodGhpcy5yZWZzLmdyYXBoKS5vZmYoXCJwbG90aG92ZXJcIik7XG4gICAgICAgICQodGhpcy5yZWZzLmdyYXBoKS5iaW5kKFwicGxvdGhvdmVyXCIsIGZ1bmN0aW9uIChldmVudCwgcG9zLCBpdGVtKSB7XG4gICAgICAgICAgICBjdHJsLmhvdmVyID0gcG9zLng7XG4gICAgICAgIH0pO1xuICAgIH1cblxuXG59IiwiLy8qKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKipcbi8vICBUcmVuZGluZ0RhdGFEaXNwbGF5LnRzeCAtIEdidGNcbi8vXG4vLyAgQ29weXJpZ2h0IMKpIDIwMTgsIEdyaWQgUHJvdGVjdGlvbiBBbGxpYW5jZS4gIEFsbCBSaWdodHMgUmVzZXJ2ZWQuXG4vL1xuLy8gIExpY2Vuc2VkIHRvIHRoZSBHcmlkIFByb3RlY3Rpb24gQWxsaWFuY2UgKEdQQSkgdW5kZXIgb25lIG9yIG1vcmUgY29udHJpYnV0b3IgbGljZW5zZSBhZ3JlZW1lbnRzLiBTZWVcbi8vICB0aGUgTk9USUNFIGZpbGUgZGlzdHJpYnV0ZWQgd2l0aCB0aGlzIHdvcmsgZm9yIGFkZGl0aW9uYWwgaW5mb3JtYXRpb24gcmVnYXJkaW5nIGNvcHlyaWdodCBvd25lcnNoaXAuXG4vLyAgVGhlIEdQQSBsaWNlbnNlcyB0aGlzIGZpbGUgdG8geW91IHVuZGVyIHRoZSBNSVQgTGljZW5zZSAoTUlUKSwgdGhlIFwiTGljZW5zZVwiOyB5b3UgbWF5IG5vdCB1c2UgdGhpc1xuLy8gIGZpbGUgZXhjZXB0IGluIGNvbXBsaWFuY2Ugd2l0aCB0aGUgTGljZW5zZS4gWW91IG1heSBvYnRhaW4gYSBjb3B5IG9mIHRoZSBMaWNlbnNlIGF0OlxuLy9cbi8vICAgICAgaHR0cDovL29wZW5zb3VyY2Uub3JnL2xpY2Vuc2VzL01JVFxuLy9cbi8vICBVbmxlc3MgYWdyZWVkIHRvIGluIHdyaXRpbmcsIHRoZSBzdWJqZWN0IHNvZnR3YXJlIGRpc3RyaWJ1dGVkIHVuZGVyIHRoZSBMaWNlbnNlIGlzIGRpc3RyaWJ1dGVkIG9uIGFuXG4vLyAgXCJBUy1JU1wiIEJBU0lTLCBXSVRIT1VUIFdBUlJBTlRJRVMgT1IgQ09ORElUSU9OUyBPRiBBTlkgS0lORCwgZWl0aGVyIGV4cHJlc3Mgb3IgaW1wbGllZC4gUmVmZXIgdG8gdGhlXG4vLyAgTGljZW5zZSBmb3IgdGhlIHNwZWNpZmljIGxhbmd1YWdlIGdvdmVybmluZyBwZXJtaXNzaW9ucyBhbmQgbGltaXRhdGlvbnMuXG4vL1xuLy8gIENvZGUgTW9kaWZpY2F0aW9uIEhpc3Rvcnk6XG4vLyAgLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxuLy8gIDA3LzE5LzIwMTggLSBCaWxseSBFcm5lc3Rcbi8vICAgICAgIEdlbmVyYXRlZCBvcmlnaW5hbCB2ZXJzaW9uIG9mIHNvdXJjZSBjb2RlLlxuLy9cbi8vKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqKioqXG5cbmltcG9ydCAqIGFzIFJlYWN0IGZyb20gJ3JlYWN0JztcbmltcG9ydCAqIGFzIFJlYWN0RE9NIGZyb20gJ3JlYWN0LWRvbSc7XG5pbXBvcnQgVHJlbmRpbmdEYXRhRGlzcGxheVNlcnZpY2UgZnJvbSAnLi8uLi9UUy9TZXJ2aWNlcy9UcmVuZGluZ0RhdGFEaXNwbGF5JztcbmltcG9ydCBjcmVhdGVIaXN0b3J5IGZyb20gXCJoaXN0b3J5L2NyZWF0ZUJyb3dzZXJIaXN0b3J5XCJcbmltcG9ydCAqIGFzIHF1ZXJ5U3RyaW5nIGZyb20gXCJxdWVyeS1zdHJpbmdcIjtcbmltcG9ydCAqIGFzIG1vbWVudCBmcm9tICdtb21lbnQnO1xuaW1wb3J0ICogYXMgXyBmcm9tIFwibG9kYXNoXCI7XG5pbXBvcnQgTWV0ZXJJbnB1dCBmcm9tICcuL01ldGVySW5wdXQnO1xuaW1wb3J0IE1lYXN1cmVtZW50SW5wdXQgZnJvbSAnLi9NZWFzdXJlbWVudElucHV0JztcbmltcG9ydCBUcmVuZGluZ0NoYXJ0IGZyb20gJy4vVHJlbmRpbmdDaGFydCc7XG5pbXBvcnQgRGF0ZVRpbWVSYW5nZVBpY2tlciBmcm9tICcuL0RhdGVUaW1lUmFuZ2VQaWNrZXInO1xuaW1wb3J0IHsgIFRyZW5kaW5nY0RhdGFEaXNwbGF5IH0gZnJvbSAnLi9nbG9iYWwnXG5pbXBvcnQgeyBSYW5kb21Db2xvciB9IGZyb20gJ0BncGEtZ2Vtc3RvbmUvaGVscGVyLWZ1bmN0aW9ucyc7XG5cbmNvbnN0IFRyZW5kaW5nRGF0YURpc3BsYXkgPSAoKSA9PiB7XG4gICAgbGV0IHRyZW5kaW5nRGF0YURpc3BsYXlTZXJ2aWNlID0gbmV3IFRyZW5kaW5nRGF0YURpc3BsYXlTZXJ2aWNlKCk7XG4gICAgY29uc3QgcmVzaXplSWQgPSBSZWFjdC51c2VSZWYobnVsbCk7XG4gICAgY29uc3QgbG9hZGVyID0gUmVhY3QudXNlUmVmKG51bGwpO1xuXG4gICAgbGV0IGhpc3RvcnkgPSBjcmVhdGVIaXN0b3J5KCk7XG5cbiAgICBsZXQgcXVlcnkgPSBxdWVyeVN0cmluZy5wYXJzZShoaXN0b3J5Wydsb2NhdGlvbiddLnNlYXJjaCk7XG5cbiAgICBjb25zdCBbbWVhc3VyZW1lbnRzLCBzZXRNZWFzdXJlbWVudHNdID0gUmVhY3QudXNlU3RhdGU8VHJlbmRpbmdjRGF0YURpc3BsYXkuTWVhc3VyZW1lbnRbXT4oc2Vzc2lvblN0b3JhZ2UuZ2V0SXRlbSgnVHJlbmRpbmdEYXRhRGlzcGxheS1tZWFzdXJlbWVudHMnKSA9PSBudWxsID8gW10gOiBKU09OLnBhcnNlKHNlc3Npb25TdG9yYWdlLmdldEl0ZW0oJ1RyZW5kaW5nRGF0YURpc3BsYXktbWVhc3VyZW1lbnRzJykpKTtcbiAgICBjb25zdCBbd2lkdGgsIHNldFdpZHRoXSA9IFJlYWN0LnVzZVN0YXRlPG51bWJlcj4od2luZG93LmlubmVyV2lkdGggLSA0NzUpO1xuICAgIGNvbnN0IFtzdGFydERhdGUsIHNldFN0YXJ0RGF0ZV0gPSBSZWFjdC51c2VTdGF0ZTxzdHJpbmc+KHF1ZXJ5WydzdGFydERhdGUnXSAhPSB1bmRlZmluZWQgPyBxdWVyeVsnc3RhcnREYXRlJ10gOiBtb21lbnQoKS5zdWJ0cmFjdCg3LCAnZGF5JykuZm9ybWF0KCdZWVlZLU1NLUREVEhIOm1tJykpO1xuICAgIGNvbnN0IFtlbmREYXRlLCBzZXRFbmREYXRlXSA9IFJlYWN0LnVzZVN0YXRlPHN0cmluZz4ocXVlcnlbJ2VuZERhdGUnXSAhPSB1bmRlZmluZWQgPyBxdWVyeVsnZW5kRGF0ZSddIDogbW9tZW50KCkuZm9ybWF0KCdZWVlZLU1NLUREVEhIOm1tJykpO1xuICAgIGNvbnN0IFtheGVzLCBzZXRBeGVzXSA9IFJlYWN0LnVzZVN0YXRlPFRyZW5kaW5nY0RhdGFEaXNwbGF5LkZsb3RBeGlzW10+KHNlc3Npb25TdG9yYWdlLmdldEl0ZW0oJ1RyZW5kaW5nRGF0YURpc3BsYXktYXhlcycpID09IG51bGwgPyBbeyBheGlzTGFiZWw6ICdEZWZhdWx0JywgY29sb3I6ICdibGFjaycsIHBvc2l0aW9uOiAnbGVmdCcgfV0gOiBKU09OLnBhcnNlKHNlc3Npb25TdG9yYWdlLmdldEl0ZW0oJ1RyZW5kaW5nRGF0YURpc3BsYXktYXhlcycpKSk7XG5cbiAgICBSZWFjdC51c2VFZmZlY3QoKCkgPT4ge1xuICAgICAgICB3aW5kb3cuYWRkRXZlbnRMaXN0ZW5lcihcInJlc2l6ZVwiLCBoYW5kbGVTY3JlZW5TaXplQ2hhbmdlLmJpbmQodGhpcykpO1xuICAgICAgICAvL2lmICh0aGlzLnN0YXRlLm1lYXN1cmVtZW50SUQgIT0gMCkgZ2V0RGF0YSgpO1xuXG4gICAgICAgIGhpc3RvcnlbJ2xpc3RlbiddKChsb2NhdGlvbiwgYWN0aW9uKSA9PiB7XG4gICAgICAgICAgICBsZXQgcXVlcnkgPSBxdWVyeVN0cmluZy5wYXJzZShoaXN0b3J5Wydsb2NhdGlvbiddLnNlYXJjaCk7XG4gICAgICAgICAgICBzZXRTdGFydERhdGUocXVlcnlbJ3N0YXJ0RGF0ZSddICE9IHVuZGVmaW5lZCA/IHF1ZXJ5WydzdGFydERhdGUnXSA6IG1vbWVudCgpLnN1YnRyYWN0KDcsICdkYXknKS5mb3JtYXQoJ1lZWVktTU0tRERUSEg6bW0nKSk7XG4gICAgICAgICAgICBzZXRFbmREYXRlKHF1ZXJ5WydlbmREYXRlJ10gIT0gdW5kZWZpbmVkID8gcXVlcnlbJ2VuZERhdGUnXSA6IG1vbWVudCgpLmZvcm1hdCgnWVlZWS1NTS1ERFRISDptbScpKTtcbiAgICAgICAgfSk7XG5cbiAgICAgICAgcmV0dXJuICgpID0+ICQod2luZG93KS5vZmYoJ3Jlc2l6ZScpO1xuICAgIH0sIFtdKTtcblxuICAgIFJlYWN0LnVzZUVmZmVjdCgoKSA9PiB7XG4gICAgICAgIGlmIChtZWFzdXJlbWVudHMubGVuZ3RoID09IDApIHJldHVybjtcbiAgICAgICAgZ2V0RGF0YSgpO1xuICAgIH0sIFttZWFzdXJlbWVudHMubGVuZ3RoLCBzdGFydERhdGUsIGVuZERhdGVdKTtcblxuICAgIFJlYWN0LnVzZUVmZmVjdCgoKSA9PiB7XG4gICAgICAgIGhpc3RvcnlbJ3B1c2gnXSgnVHJlbmRpbmdEYXRhRGlzcGxheS5jc2h0bWw/JyArIHF1ZXJ5U3RyaW5nLnN0cmluZ2lmeSh7c3RhcnREYXRlLCBlbmREYXRlfSwgeyBlbmNvZGU6IGZhbHNlIH0pKVxuICAgIH0sIFtzdGFydERhdGUsZW5kRGF0ZV0pO1xuXG4gICAgUmVhY3QudXNlRWZmZWN0KCgpID0+IHtcbiAgICAgICAgc2Vzc2lvblN0b3JhZ2Uuc2V0SXRlbSgnVHJlbmRpbmdEYXRhRGlzcGxheS1tZWFzdXJlbWVudHMnLCBKU09OLnN0cmluZ2lmeShtZWFzdXJlbWVudHMubWFwKG1zID0+ICh7IElEOiBtcy5JRCwgTWV0ZXJJRDogbXMuTWV0ZXJJRCxNZXRlck5hbWU6IG1zLk1ldGVyTmFtZSxNZWFzdXJlbWVudE5hbWU6IG1zLk1lYXN1cmVtZW50TmFtZSxBdmVyYWdlOiBtcy5BdmVyYWdlLEF2Z0NvbG9yOiBtcy5BdmdDb2xvcixNYXhpbXVtOiBtcy5NYXhpbXVtLCBNYXhDb2xvcjogbXMuTWF4Q29sb3IsIE1pbmltdW06IG1zLk1pbmltdW0sIE1pbkNvbG9yOiBtcy5NaW5Db2xvciwgQXhpczogbXMuQXhpc30pKSkpXG4gICAgfSwgW21lYXN1cmVtZW50c10pO1xuXG4gICAgUmVhY3QudXNlRWZmZWN0KCgpID0+IHtcbiAgICAgICAgc2Vzc2lvblN0b3JhZ2Uuc2V0SXRlbSgnVHJlbmRpbmdEYXRhRGlzcGxheS1heGVzJywgSlNPTi5zdHJpbmdpZnkoYXhlcykpXG4gICAgfSwgW2F4ZXNdKTtcblxuICAgIGZ1bmN0aW9uIGdldERhdGEoKSB7XG4gICAgICAgICQobG9hZGVyLmN1cnJlbnQpLnNob3coKTtcbiAgICAgICAgdHJlbmRpbmdEYXRhRGlzcGxheVNlcnZpY2UuZ2V0UG9zdERhdGEobWVhc3VyZW1lbnRzLCBzdGFydERhdGUsIGVuZERhdGUsIHdpZHRoKS5kb25lKGRhdGEgPT4ge1xuICAgICAgICAgICAgbGV0IG1lYXMgPVsgLi4ubWVhc3VyZW1lbnRzIF07XG4gICAgICAgICAgICBmb3IgKGxldCBrZXkgb2YgT2JqZWN0LmtleXMoZGF0YSkpIHtcbiAgICAgICAgICAgICAgICBsZXQgaSA9IG1lYXMuZmluZEluZGV4KHggPT4geC5JRC50b1N0cmluZygpID09PSBrZXkpO1xuICAgICAgICAgICAgICAgIG1lYXNbaV0uRGF0YSA9IGRhdGFba2V5XTtcbiAgICAgICAgICAgIH1cbiAgICAgICAgICAgIHNldE1lYXN1cmVtZW50cyhtZWFzKVxuXG4gICAgICAgICAgICAkKGxvYWRlci5jdXJyZW50KS5oaWRlKClcbiAgICAgICAgfSk7XG4gICAgfVxuXG5cbiAgICBmdW5jdGlvbiBoYW5kbGVTY3JlZW5TaXplQ2hhbmdlKCkge1xuICAgICAgICBjbGVhclRpbWVvdXQodGhpcy5yZXNpemVJZCk7XG4gICAgICAgIHRoaXMucmVzaXplSWQgPSBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgICAgfSwgNTAwKTtcbiAgICB9XG5cbiAgICBsZXQgaGVpZ2h0ID0gd2luZG93LmlubmVySGVpZ2h0IC0gJCgnI25hdmJhcicpLmhlaWdodCgpO1xuICAgIGxldCBtZW51V2lkdGggPSAyNTA7XG4gICAgbGV0IHNpZGVXaWR0aCA9IDQwMDtcbiAgICBsZXQgdG9wID0gJCgnI25hdmJhcicpLmhlaWdodCgpIC0gMzA7XG4gICAgcmV0dXJuIChcbiAgICAgICAgPGRpdj5cbiAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwic2NyZWVuXCIgc3R5bGU9e3sgaGVpZ2h0OiBoZWlnaHQsIHdpZHRoOiB3aW5kb3cuaW5uZXJXaWR0aCwgcG9zaXRpb246ICdyZWxhdGl2ZScsIHRvcDogdG9wIH19PlxuICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwidmVydGljYWwtbWVudVwiIHN0eWxlPXt7bWF4SGVpZ2h0OiBoZWlnaHQsIG92ZXJmbG93WTogJ2F1dG8nIH19PlxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImZvcm0tZ3JvdXBcIj5cbiAgICAgICAgICAgICAgICAgICAgICAgIDxsYWJlbD5UaW1lIFJhbmdlOiA8L2xhYmVsPlxuICAgICAgICAgICAgICAgICAgICAgICAgPERhdGVUaW1lUmFuZ2VQaWNrZXIgc3RhcnREYXRlPXtzdGFydERhdGV9IGVuZERhdGU9e2VuZERhdGV9IHN0YXRlU2V0dGVyPXsob2JqKSA9PiB7XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgaWYoc3RhcnREYXRlICE9IG9iai5zdGFydERhdGUpXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHNldFN0YXJ0RGF0ZShvYmouc3RhcnREYXRlKTtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBpZiAoZW5kRGF0ZSAhPSBvYmouZW5kRGF0ZSlcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgc2V0RW5kRGF0ZShvYmouZW5kRGF0ZSk7XG4gICAgICAgICAgICAgICAgICAgICAgICB9fSAvPlxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJmb3JtLWdyb3VwXCIgc3R5bGU9e3toZWlnaHQ6IDUwfX0+XG4gICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IHN0eWxlPXt7IGZsb2F0OiAnbGVmdCcgfX0gcmVmPXtsb2FkZXJ9IGhpZGRlbj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IHN0eWxlPXt7IGJvcmRlcjogJzVweCBzb2xpZCAjZjNmM2YzJywgV2Via2l0QW5pbWF0aW9uOiAnc3BpbiAxcyBsaW5lYXIgaW5maW5pdGUnLCBhbmltYXRpb246ICdzcGluIDFzIGxpbmVhciBpbmZpbml0ZScsIGJvcmRlclRvcDogJzVweCBzb2xpZCAjNTU1JywgYm9yZGVyUmFkaXVzOiAnNTAlJywgd2lkdGg6ICcyNXB4JywgaGVpZ2h0OiAnMjVweCcgfX0+PC9kaXY+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPHNwYW4+TG9hZGluZy4uLjwvc3Bhbj5cbiAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cblxuXG4gICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiZm9ybS1ncm91cFwiPlxuICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJwYW5lbC1ncm91cFwiPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwicGFuZWwgcGFuZWwtZGVmYXVsdFwiPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cInBhbmVsLWhlYWRpbmdcIiBzdHlsZT17eyBwb3NpdGlvbjogJ3JlbGF0aXZlJywgaGVpZ2h0OiA2MH19PlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGg0IGNsYXNzTmFtZT1cInBhbmVsLXRpdGxlXCIgc3R5bGU9e3sgcG9zaXRpb246ICdhYnNvbHV0ZScsIGxlZnQ6IDE1LCB3aWR0aDogJzYwJScgfX0+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGEgZGF0YS10b2dnbGU9XCJjb2xsYXBzZVwiIGhyZWY9XCIjTWVhc3VyZW1lbnRDb2xsYXBzZVwiPk1lYXN1cmVtZW50czo8L2E+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2g0PlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPEFkZE1lYXN1cmVtZW50IEFkZD17KG1zbnQpID0+IHNldE1lYXN1cmVtZW50cyhtZWFzdXJlbWVudHMuY29uY2F0KG1zbnQpKX0gLz5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgaWQ9J01lYXN1cmVtZW50Q29sbGFwc2UnIGNsYXNzTmFtZT0ncGFuZWwtY29sbGFwc2UnPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHVsIGNsYXNzTmFtZT0nbGlzdC1ncm91cCc+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAge21lYXN1cmVtZW50cy5tYXAoKG1zLCBpKSA9PiA8TWVhc3VyZW1lbnRSb3cga2V5PXtpfSBNZWFzdXJlbWVudD17bXN9IE1lYXN1cmVtZW50cz17bWVhc3VyZW1lbnRzfSBBeGVzPXtheGVzfSBJbmRleD17aX0gU2V0TWVhc3VyZW1lbnRzPXtzZXRNZWFzdXJlbWVudHN9IC8+KX1cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvdWw+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxuXG4gICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XG5cblxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImZvcm0tZ3JvdXBcIj5cbiAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwicGFuZWwtZ3JvdXBcIj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cInBhbmVsIHBhbmVsLWRlZmF1bHRcIj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJwYW5lbC1oZWFkaW5nXCIgc3R5bGU9e3sgcG9zaXRpb246ICdyZWxhdGl2ZScsIGhlaWdodDogNjAgfX0+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8aDQgY2xhc3NOYW1lPVwicGFuZWwtdGl0bGVcIiBzdHlsZT17eyBwb3NpdGlvbjogJ2Fic29sdXRlJywgbGVmdDogMTUsIHdpZHRoOiAnNjAlJyB9fT5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8YSBkYXRhLXRvZ2dsZT1cImNvbGxhcHNlXCIgaHJlZj1cIiNheGVzQ29sbGFwc2VcIj5BeGVzOjwvYT5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvaDQ+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8QWRkQXhpcyBBZGQ9eyhheGlzKSA9PiBzZXRBeGVzKGF4ZXMuY29uY2F0KGF4aXMpKX0gLz5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgaWQ9J2F4ZXNDb2xsYXBzZScgY2xhc3NOYW1lPSdwYW5lbC1jb2xsYXBzZSc+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8dWwgY2xhc3NOYW1lPSdsaXN0LWdyb3VwJz5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB7YXhlcy5tYXAoKGF4aXMsIGkpID0+IDxBeGlzUm93IGtleT17aX0gQXhlcz17YXhlc30gSW5kZXg9e2l9IFNldEF4ZXM9e3NldEF4ZXN9Lz4pfVxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC91bD5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XG5cbiAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cblxuICAgICAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwid2F2ZWZvcm0tdmlld2VyXCIgc3R5bGU9e3sgd2lkdGg6IHdpbmRvdy5pbm5lcldpZHRoIC0gbWVudVdpZHRoLCBoZWlnaHQ6IGhlaWdodCwgZmxvYXQ6ICdsZWZ0JywgbGVmdDogMCB9fT5cbiAgICAgICAgICAgICAgICAgICAgPFRyZW5kaW5nQ2hhcnQgc3RhcnREYXRlPXtzdGFydERhdGV9IGVuZERhdGU9e2VuZERhdGV9IGRhdGE9e21lYXN1cmVtZW50c30gYXhlcz17YXhlc30gc3RhdGVTZXR0ZXI9eyhvYmplY3QpID0+IHtcbiAgICAgICAgICAgICAgICAgICAgICAgIHNldFN0YXJ0RGF0ZShvYmplY3Quc3RhcnREYXRlKTtcbiAgICAgICAgICAgICAgICAgICAgICAgIHNldEVuZERhdGUob2JqZWN0LmVuZERhdGUpO1xuICAgICAgICAgICAgICAgICAgICB9fSAvPlxuICAgICAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgIDwvZGl2PlxuICAgICk7XG59XG5cbmNvbnN0IEFkZE1lYXN1cmVtZW50ID0gKHByb3BzOiB7IEFkZDogKG1zbnQ6VHJlbmRpbmdjRGF0YURpc3BsYXkuTWVhc3VyZW1lbnQpID0+IHZvaWR9KSA9PiB7XG4gICAgY29uc3QgW21lYXN1cmVtZW50LCBzZXRNZWFzdXJlbWVudF0gPSBSZWFjdC51c2VTdGF0ZTxUcmVuZGluZ2NEYXRhRGlzcGxheS5NZWFzdXJlbWVudD4oeyBJRDogMCwgTWV0ZXJJRDogMCwgTWV0ZXJOYW1lOiAnJywgTWVhc3VyZW1lbnROYW1lOiAnJywgTWF4aW11bTogdHJ1ZSwgTWF4Q29sb3I6IFJhbmRvbUNvbG9yKCksIEF2ZXJhZ2U6IHRydWUsIEF2Z0NvbG9yOiBSYW5kb21Db2xvcigpLCBNaW5pbXVtOiB0cnVlLCBNaW5Db2xvcjogUmFuZG9tQ29sb3IoKSwgQXhpczogMSB9KTtcblxuICAgIHJldHVybiAoXG4gICAgICAgIDw+XG4gICAgICAgICAgICA8YnV0dG9uIHR5cGU9XCJidXR0b25cIiBzdHlsZT17eyBwb3NpdGlvbjogJ2Fic29sdXRlJywgcmlnaHQ6IDE1fX0gY2xhc3NOYW1lPVwiYnRuIGJ0bi1pbmZvXCIgZGF0YS10b2dnbGU9XCJtb2RhbFwiIGRhdGEtdGFyZ2V0PVwiI21lYXN1cmVtZW50TW9kYWxcIiBvbkNsaWNrPXsoKSA9PiB7XG4gICAgICAgICAgICAgICAgc2V0TWVhc3VyZW1lbnQoeyBJRDogMCwgTWV0ZXJJRDogMCwgTWV0ZXJOYW1lOiAnJywgTWVhc3VyZW1lbnROYW1lOiAnJywgTWF4aW11bTogdHJ1ZSwgTWF4Q29sb3I6IFJhbmRvbUNvbG9yKCksIEF2ZXJhZ2U6IHRydWUsIEF2Z0NvbG9yOiBSYW5kb21Db2xvcigpLCBNaW5pbXVtOiB0cnVlLCBNaW5Db2xvcjogUmFuZG9tQ29sb3IoKSwgQXhpczogMSB9KTtcbiAgICAgICAgICAgIH19PkFkZDwvYnV0dG9uPlxuICAgICAgICAgICAgPGRpdiBpZD1cIm1lYXN1cmVtZW50TW9kYWxcIiBjbGFzc05hbWU9XCJtb2RhbCBmYWRlXCIgcm9sZT1cImRpYWxvZ1wiPlxuICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwibW9kYWwtZGlhbG9nXCI+XG4gICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwibW9kYWwtY29udGVudFwiPlxuICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJtb2RhbC1oZWFkZXJcIj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8YnV0dG9uIHR5cGU9XCJidXR0b25cIiBjbGFzc05hbWU9XCJjbG9zZVwiIGRhdGEtZGlzbWlzcz1cIm1vZGFsXCI+JnRpbWVzOzwvYnV0dG9uPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxoNCBjbGFzc05hbWU9XCJtb2RhbC10aXRsZVwiPkFkZCBNZWFzdXJlbWVudDwvaDQ+XG4gICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwibW9kYWwtYm9keVwiPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiZm9ybS1ncm91cFwiPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8bGFiZWw+TWV0ZXI6IDwvbGFiZWw+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxNZXRlcklucHV0IHZhbHVlPXttZWFzdXJlbWVudC5NZXRlcklEfSBvbkNoYW5nZT17KG9iaikgPT4gc2V0TWVhc3VyZW1lbnQoeyAuLi5tZWFzdXJlbWVudCwgTWV0ZXJJRDogb2JqLm1ldGVySUQsIE1ldGVyTmFtZTogb2JqLm1ldGVyTmFtZSB9KX0gLz5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cblxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiZm9ybS1ncm91cFwiPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8bGFiZWw+TWVhc3VyZW1lbnQ6IDwvbGFiZWw+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxNZWFzdXJlbWVudElucHV0IG1ldGVySUQ9e21lYXN1cmVtZW50Lk1ldGVySUR9IHZhbHVlPXttZWFzdXJlbWVudC5JRH0gb25DaGFuZ2U9eyhvYmopID0+IHNldE1lYXN1cmVtZW50KHsgLi4ubWVhc3VyZW1lbnQsIElEOiBvYmoubWVhc3VyZW1lbnRJRCwgTWVhc3VyZW1lbnROYW1lOiBvYmoubWVhc3VyZW1lbnROYW1lIH0pfSAvPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxuXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJyb3dcIj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJjb2wtbGctNlwiPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJjaGVja2JveFwiPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxsYWJlbD48aW5wdXQgdHlwZT1cImNoZWNrYm94XCIgY2hlY2tlZD17bWVhc3VyZW1lbnQuTWF4aW11bX0gdmFsdWU9e21lYXN1cmVtZW50Lk1heGltdW0udG9TdHJpbmcoKX0gb25DaGFuZ2U9eygpID0+IHNldE1lYXN1cmVtZW50KHsgLi4ubWVhc3VyZW1lbnQsIE1heGltdW06ICFtZWFzdXJlbWVudC5NYXhpbXVtIH0pIH0vPiBNYXhpbXVtPC9sYWJlbD5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJjb2wtbGctNlwiPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGlucHV0IHR5cGU9XCJjb2xvclwiIHN0eWxlPXt7dGV4dEFsaWduOidsZWZ0J319IGNsYXNzTmFtZT1cImZvcm0tY29udHJvbFwiIHZhbHVlPXttZWFzdXJlbWVudC5NYXhDb2xvcn0gb25DaGFuZ2U9eyhldnQpID0+IHNldE1lYXN1cmVtZW50KHsgLi4ubWVhc3VyZW1lbnQsIE1heENvbG9yOiBldnQudGFyZ2V0LnZhbHVlIH0pfSAvPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cblxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwicm93XCI+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiY29sLWxnLTZcIj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiY2hlY2tib3hcIj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8bGFiZWw+PGlucHV0IHR5cGU9XCJjaGVja2JveFwiIGNoZWNrZWQ9e21lYXN1cmVtZW50LkF2ZXJhZ2V9IHZhbHVlPXttZWFzdXJlbWVudC5BdmVyYWdlLnRvU3RyaW5nKCl9IG9uQ2hhbmdlPXsoKSA9PiBzZXRNZWFzdXJlbWVudCh7IC4uLm1lYXN1cmVtZW50LCBBdmVyYWdlOiAhbWVhc3VyZW1lbnQuQXZlcmFnZSB9KX0gLz4gQXZlcmFnZTwvbGFiZWw+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiY29sLWxnLTZcIj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxpbnB1dCB0eXBlPVwiY29sb3JcIiBzdHlsZT17eyB0ZXh0QWxpZ246ICdsZWZ0JyB9fSBjbGFzc05hbWU9XCJmb3JtLWNvbnRyb2xcIiB2YWx1ZT17bWVhc3VyZW1lbnQuQXZnQ29sb3J9IG9uQ2hhbmdlPXsoZXZ0KSA9PiBzZXRNZWFzdXJlbWVudCh7IC4uLm1lYXN1cmVtZW50LCBBdmdDb2xvcjogZXZ0LnRhcmdldC52YWx1ZSB9KX0gLz5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XG5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cInJvd1wiPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImNvbC1sZy02XCI+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImNoZWNrYm94XCI+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGxhYmVsPjxpbnB1dCB0eXBlPVwiY2hlY2tib3hcIiBjaGVja2VkPXttZWFzdXJlbWVudC5NaW5pbXVtfSB2YWx1ZT17bWVhc3VyZW1lbnQuTWluaW11bS50b1N0cmluZygpfSBvbkNoYW5nZT17KCkgPT4gc2V0TWVhc3VyZW1lbnQoeyAuLi5tZWFzdXJlbWVudCwgTWluaW11bTogIW1lYXN1cmVtZW50Lk1pbmltdW0gfSl9IC8+IE1pbmltdW08L2xhYmVsPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImNvbC1sZy02XCI+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8aW5wdXQgdHlwZT1cImNvbG9yXCIgc3R5bGU9e3sgdGV4dEFsaWduOiAnbGVmdCcgfX0gY2xhc3NOYW1lPVwiZm9ybS1jb250cm9sXCIgdmFsdWU9e21lYXN1cmVtZW50Lk1pbkNvbG9yfSBvbkNoYW5nZT17KGV2dCkgPT4gc2V0TWVhc3VyZW1lbnQoeyAuLi5tZWFzdXJlbWVudCwgTWluQ29sb3I6IGV2dC50YXJnZXQudmFsdWUgfSl9IC8+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxuXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XG5cbiAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJtb2RhbC1mb290ZXJcIj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8YnV0dG9uIHR5cGU9XCJidXR0b25cIiBjbGFzc05hbWU9XCJidG4gYnRuLXByaW1hcnlcIiBkYXRhLWRpc21pc3M9XCJtb2RhbFwiIG9uQ2xpY2s9eygpID0+IHByb3BzLkFkZChtZWFzdXJlbWVudCkgfT5BZGQ8L2J1dHRvbj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8YnV0dG9uIHR5cGU9XCJidXR0b25cIiBjbGFzc05hbWU9XCJidG4gYnRuLWRlZmF1bHRcIiBkYXRhLWRpc21pc3M9XCJtb2RhbFwiPkNhbmNlbDwvYnV0dG9uPlxuICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgPC9kaXY+XG5cbiAgICAgICAgPC8+XG4gICAgKTtcbn1cblxuY29uc3QgTWVhc3VyZW1lbnRSb3cgPSAocHJvcHM6IHsgTWVhc3VyZW1lbnQ6IFRyZW5kaW5nY0RhdGFEaXNwbGF5Lk1lYXN1cmVtZW50LCBNZWFzdXJlbWVudHM6IFRyZW5kaW5nY0RhdGFEaXNwbGF5Lk1lYXN1cmVtZW50W10sIEluZGV4OiBudW1iZXIsIFNldE1lYXN1cmVtZW50czogKG1lYXN1cmVtZW50czogVHJlbmRpbmdjRGF0YURpc3BsYXkuTWVhc3VyZW1lbnRbXSkgPT4gdm9pZCwgQXhlczogVHJlbmRpbmdjRGF0YURpc3BsYXkuRmxvdEF4aXNbXX0pID0+IHtcbiAgICByZXR1cm4gKFxuICAgICAgICA8bGkgY2xhc3NOYW1lPSdsaXN0LWdyb3VwLWl0ZW0nIGtleT17cHJvcHMuTWVhc3VyZW1lbnQuSUR9PlxuICAgICAgICAgICAgPGRpdj57cHJvcHMuTWVhc3VyZW1lbnQuTWV0ZXJOYW1lfTwvZGl2PjxidXR0b24gdHlwZT1cImJ1dHRvblwiIHN0eWxlPXt7IHBvc2l0aW9uOiAncmVsYXRpdmUnLCB0b3A6IC0yMCB9fSBjbGFzc05hbWU9XCJjbG9zZVwiIG9uQ2xpY2s9eygpID0+IHtcbiAgICAgICAgICAgICAgICBsZXQgbWVhcyA9IFsuLi5wcm9wcy5NZWFzdXJlbWVudHNdO1xuICAgICAgICAgICAgICAgIG1lYXMuc3BsaWNlKHByb3BzLkluZGV4LCAxKTtcbiAgICAgICAgICAgICAgICBwcm9wcy5TZXRNZWFzdXJlbWVudHMobWVhcylcbiAgICAgICAgICAgIH19PiZ0aW1lczs8L2J1dHRvbj5cbiAgICAgICAgICAgIDxkaXY+e3Byb3BzLk1lYXN1cmVtZW50Lk1lYXN1cmVtZW50TmFtZX08L2Rpdj5cbiAgICAgICAgICAgIDxkaXY+XG4gICAgICAgICAgICAgICAgPHNlbGVjdCBjbGFzc05hbWU9J2Zvcm0tY29udHJvbCcgdmFsdWU9e3Byb3BzLk1lYXN1cmVtZW50LkF4aXN9IG9uQ2hhbmdlPXsoZXZ0KSA9PiB7XG4gICAgICAgICAgICAgICAgICAgIGxldCBtZWFzID0gWy4uLnByb3BzLk1lYXN1cmVtZW50c107XG4gICAgICAgICAgICAgICAgICAgIG1lYXNbcHJvcHMuSW5kZXhdLkF4aXMgPSBwYXJzZUludChldnQudGFyZ2V0LnZhbHVlKTtcbiAgICAgICAgICAgICAgICAgICAgcHJvcHMuU2V0TWVhc3VyZW1lbnRzKG1lYXMpXG4gICAgICAgICAgICAgICAgfX0+XG4gICAgICAgICAgICAgICAgICAgIHtwcm9wcy5BeGVzLm1hcCgoYSwgaXgpID0+IDxvcHRpb24ga2V5PXtpeH0gdmFsdWU9e2l4ICsgMX0+e2EuYXhpc0xhYmVsfTwvb3B0aW9uPil9XG4gICAgICAgICAgICAgICAgPC9zZWxlY3Q+XG5cbiAgICAgICAgICAgIDwvZGl2PlxuXG4gICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT0ncm93Jz5cbiAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT0nY29sLWxnLTQnPlxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cIlwiPlxuICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJjaGVja2JveFwiPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxsYWJlbD48aW5wdXQgdHlwZT1cImNoZWNrYm94XCIgY2hlY2tlZD17cHJvcHMuTWVhc3VyZW1lbnQuTWF4aW11bX0gdmFsdWU9e3Byb3BzLk1lYXN1cmVtZW50Lk1heGltdW0udG9TdHJpbmcoKX0gb25DaGFuZ2U9eygpID0+IHtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgbGV0IG1lYXMgPSBbLi4ucHJvcHMuTWVhc3VyZW1lbnRzXTtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgbWVhc1twcm9wcy5JbmRleF0uTWF4aW11bSA9ICFtZWFzW3Byb3BzLkluZGV4XS5NYXhpbXVtO1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBwcm9wcy5TZXRNZWFzdXJlbWVudHMobWVhcylcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB9fSAvPiBNYXg8L2xhYmVsPlxuICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cIlwiPlxuICAgICAgICAgICAgICAgICAgICAgICAgPGlucHV0IHR5cGU9XCJjb2xvclwiIGNsYXNzTmFtZT1cImZvcm0tY29udHJvbFwiIHZhbHVlPXtwcm9wcy5NZWFzdXJlbWVudC5NYXhDb2xvcn0gb25DaGFuZ2U9eyhldnQpID0+IHtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBsZXQgbWVhcyA9IFsuLi5wcm9wcy5NZWFzdXJlbWVudHNdO1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIG1lYXNbcHJvcHMuSW5kZXhdLk1heENvbG9yID0gZXZ0LnRhcmdldC52YWx1ZTtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBwcm9wcy5TZXRNZWFzdXJlbWVudHMobWVhcylcbiAgICAgICAgICAgICAgICAgICAgICAgIH19IC8+XG4gICAgICAgICAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPSdjb2wtbGctNCc+XG4gICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiXCI+XG4gICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImNoZWNrYm94XCI+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPGxhYmVsPjxpbnB1dCB0eXBlPVwiY2hlY2tib3hcIiBjaGVja2VkPXtwcm9wcy5NZWFzdXJlbWVudC5BdmVyYWdlfSB2YWx1ZT17cHJvcHMuTWVhc3VyZW1lbnQuQXZlcmFnZS50b1N0cmluZygpfSBvbkNoYW5nZT17KCkgPT4ge1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBsZXQgbWVhcyA9IFsuLi5wcm9wcy5NZWFzdXJlbWVudHNdO1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBtZWFzW3Byb3BzLkluZGV4XS5BdmVyYWdlID0gIW1lYXNbcHJvcHMuSW5kZXhdLkF2ZXJhZ2U7XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHByb3BzLlNldE1lYXN1cmVtZW50cyhtZWFzKVxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIH19IC8+IEF2ZzwvbGFiZWw+XG4gICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiXCI+XG4gICAgICAgICAgICAgICAgICAgICAgICA8aW5wdXQgdHlwZT1cImNvbG9yXCIgY2xhc3NOYW1lPVwiZm9ybS1jb250cm9sXCIgdmFsdWU9e3Byb3BzLk1lYXN1cmVtZW50LkF2Z0NvbG9yfSBvbkNoYW5nZT17KGV2dCkgPT4ge1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGxldCBtZWFzID0gWy4uLnByb3BzLk1lYXN1cmVtZW50c107XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgbWVhc1twcm9wcy5JbmRleF0uQXZnQ29sb3IgPSBldnQudGFyZ2V0LnZhbHVlO1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHByb3BzLlNldE1lYXN1cmVtZW50cyhtZWFzKVxuICAgICAgICAgICAgICAgICAgICAgICAgfX0gLz5cbiAgICAgICAgICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9J2NvbC1sZy00Jz5cbiAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJcIj5cbiAgICAgICAgICAgICAgICAgICAgICAgIDxkaXYgY2xhc3NOYW1lPVwiY2hlY2tib3hcIj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8bGFiZWw+PGlucHV0IHR5cGU9XCJjaGVja2JveFwiIGNoZWNrZWQ9e3Byb3BzLk1lYXN1cmVtZW50Lk1pbmltdW19IHZhbHVlPXtwcm9wcy5NZWFzdXJlbWVudC5NaW5pbXVtLnRvU3RyaW5nKCl9IG9uQ2hhbmdlPXsoKSA9PiB7XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGxldCBtZWFzID0gWy4uLnByb3BzLk1lYXN1cmVtZW50c107XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIG1lYXNbcHJvcHMuSW5kZXhdLk1pbmltdW0gPSAhbWVhc1twcm9wcy5JbmRleF0uTWluaW11bTtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcHJvcHMuU2V0TWVhc3VyZW1lbnRzKG1lYXMpXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgfX0gLz4gTWluPC9sYWJlbD5cbiAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJcIj5cbiAgICAgICAgICAgICAgICAgICAgICAgIDxpbnB1dCB0eXBlPVwiY29sb3JcIiBjbGFzc05hbWU9XCJmb3JtLWNvbnRyb2xcIiB2YWx1ZT17cHJvcHMuTWVhc3VyZW1lbnQuTWluQ29sb3J9IG9uQ2hhbmdlPXsoZXZ0KSA9PiB7XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgbGV0IG1lYXMgPSBbLi4ucHJvcHMuTWVhc3VyZW1lbnRzXTtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBtZWFzW3Byb3BzLkluZGV4XS5NaW5Db2xvciA9IGV2dC50YXJnZXQudmFsdWU7XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgcHJvcHMuU2V0TWVhc3VyZW1lbnRzKG1lYXMpXG4gICAgICAgICAgICAgICAgICAgICAgICB9fSAvPlxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgICAgICAgICA8L2Rpdj5cblxuICAgICAgICAgICAgPC9kaXY+XG5cbiAgICAgICAgPC9saT5cblxuICAgICk7XG59XG5cbmNvbnN0IEFkZEF4aXMgPSAocHJvcHM6IHsgQWRkOiAoYXhpczogVHJlbmRpbmdjRGF0YURpc3BsYXkuRmxvdEF4aXMpID0+IHZvaWQgfSkgPT4ge1xuICAgIGNvbnN0IFtheGlzLCBzZXRBeGlzXSA9IFJlYWN0LnVzZVN0YXRlPFRyZW5kaW5nY0RhdGFEaXNwbGF5LkZsb3RBeGlzPih7IHBvc2l0aW9uOiAnbGVmdCcsIGNvbG9yOiAnYmxhY2snLCBheGlzTGFiZWw6ICcnLCBheGlzTGFiZWxVc2VDYW52YXM6IHRydWUsIHNob3c6IHRydWUgfSk7XG5cbiAgICByZXR1cm4gKFxuICAgICAgICA8PlxuICAgICAgICAgICAgPGJ1dHRvbiB0eXBlPVwiYnV0dG9uXCIgc3R5bGU9e3sgcG9zaXRpb246ICdhYnNvbHV0ZScsIHJpZ2h0OiAxNSB9fSAgY2xhc3NOYW1lPVwiYnRuIGJ0bi1pbmZvXCIgZGF0YS10b2dnbGU9XCJtb2RhbFwiIGRhdGEtdGFyZ2V0PVwiI2F4aXNNb2RhbFwiIG9uQ2xpY2s9eygpID0+IHtcbiAgICAgICAgICAgICAgICBzZXRBeGlzKHsgcG9zaXRpb246ICdsZWZ0JywgY29sb3I6ICdibGFjaycsIGF4aXNMYWJlbDogJycsIGF4aXNMYWJlbFVzZUNhbnZhczogdHJ1ZSwgc2hvdzogdHJ1ZSB9KTtcbiAgICAgICAgICAgIH19PkFkZDwvYnV0dG9uPlxuICAgICAgICAgICAgPGRpdiBpZD1cImF4aXNNb2RhbFwiIGNsYXNzTmFtZT1cIm1vZGFsIGZhZGVcIiByb2xlPVwiZGlhbG9nXCI+XG4gICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJtb2RhbC1kaWFsb2dcIj5cbiAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJtb2RhbC1jb250ZW50XCI+XG4gICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cIm1vZGFsLWhlYWRlclwiPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxidXR0b24gdHlwZT1cImJ1dHRvblwiIGNsYXNzTmFtZT1cImNsb3NlXCIgZGF0YS1kaXNtaXNzPVwibW9kYWxcIj4mdGltZXM7PC9idXR0b24+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPGg0IGNsYXNzTmFtZT1cIm1vZGFsLXRpdGxlXCI+QWRkIEF4aXM8L2g0PlxuICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cIm1vZGFsLWJvZHlcIj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cImZvcm0tZ3JvdXBcIj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPGxhYmVsPkxhYmVsOiA8L2xhYmVsPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8aW5wdXQgdHlwZT1cInRleHRcIiBjbGFzc05hbWU9XCJmb3JtLWNvbnRyb2xcIiB2YWx1ZT17YXhpcy5heGlzTGFiZWx9IG9uQ2hhbmdlPXsoZXZ0KSA9PiBzZXRBeGlzKHsgLi4uYXhpcywgYXhpc0xhYmVsOiBldnQudGFyZ2V0LnZhbHVlIH0pfSAvPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZGl2PlxuXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9XCJmb3JtLWdyb3VwXCI+XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxsYWJlbD5Qb3NpdGlvbjogPC9sYWJlbD5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgPHNlbGVjdCBjbGFzc05hbWU9J2Zvcm0tY29udHJvbCcgdmFsdWU9e2F4aXMucG9zaXRpb259IG9uQ2hhbmdlPXsoZXZ0KSA9PiBzZXRBeGlzKHsgLi4uYXhpcywgcG9zaXRpb246IGV2dC50YXJnZXQudmFsdWUgYXMgJ2xlZnQnIHwgJ3JpZ2h0J30pfT5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxvcHRpb24gdmFsdWU9J2xlZnQnPmxlZnQ8L29wdGlvbj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxvcHRpb24gdmFsdWU9J3JpZ2h0Jz5yaWdodDwvb3B0aW9uPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L3NlbGVjdD5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cblxuICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT1cIm1vZGFsLWZvb3RlclwiPlxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIDxidXR0b24gdHlwZT1cImJ1dHRvblwiIGNsYXNzTmFtZT1cImJ0biBidG4tcHJpbWFyeVwiIGRhdGEtZGlzbWlzcz1cIm1vZGFsXCIgb25DbGljaz17KCkgPT4gcHJvcHMuQWRkKGF4aXMpfT5BZGQ8L2J1dHRvbj5cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8YnV0dG9uIHR5cGU9XCJidXR0b25cIiBjbGFzc05hbWU9XCJidG4gYnRuLWRlZmF1bHRcIiBkYXRhLWRpc21pc3M9XCJtb2RhbFwiPkNhbmNlbDwvYnV0dG9uPlxuICAgICAgICAgICAgICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgPC9kaXY+XG5cbiAgICAgICAgPC8+XG4gICAgKTtcbn1cblxuY29uc3QgQXhpc1JvdyA9IChwcm9wczogeyBJbmRleDogbnVtYmVyLCBBeGVzOiBUcmVuZGluZ2NEYXRhRGlzcGxheS5GbG90QXhpc1tdLCBTZXRBeGVzOiAoIGF4ZXM6IFRyZW5kaW5nY0RhdGFEaXNwbGF5LkZsb3RBeGlzW10pID0+IHZvaWR9KSA9PiB7XG4gICAgcmV0dXJuIChcbiAgICAgICAgPGxpIGNsYXNzTmFtZT0nbGlzdC1ncm91cC1pdGVtJyBrZXk9e3Byb3BzLkluZGV4fT5cbiAgICAgICAgICAgIDxkaXY+e3Byb3BzLkF4ZXNbcHJvcHMuSW5kZXhdLmF4aXNMYWJlbH08L2Rpdj48YnV0dG9uIHR5cGU9XCJidXR0b25cIiBzdHlsZT17eyBwb3NpdGlvbjogJ3JlbGF0aXZlJywgdG9wOiAtMjAgfX0gY2xhc3NOYW1lPVwiY2xvc2VcIiBvbkNsaWNrPXsoKSA9PiB7XG4gICAgICAgICAgICAgICAgbGV0IGEgPSBbLi4ucHJvcHMuQXhlc107XG4gICAgICAgICAgICAgICAgYS5zcGxpY2UocHJvcHMuSW5kZXgsIDEpO1xuICAgICAgICAgICAgICAgIHByb3BzLlNldEF4ZXMoYSlcbiAgICAgICAgICAgIH19PiZ0aW1lczs8L2J1dHRvbj5cbiAgICAgICAgICAgIDxkaXY+XG4gICAgICAgICAgICAgICAgPHNlbGVjdCBjbGFzc05hbWU9J2Zvcm0tY29udHJvbCcgdmFsdWU9e3Byb3BzLkF4ZXNbcHJvcHMuSW5kZXhdLnBvc2l0aW9ufSBvbkNoYW5nZT17KGV2dCkgPT4ge1xuICAgICAgICAgICAgICAgICAgICBsZXQgYSA9IFsuLi5wcm9wcy5BeGVzXTtcbiAgICAgICAgICAgICAgICAgICAgYVtwcm9wcy5JbmRleF0ucG9zaXRpb24gPSBldnQudGFyZ2V0LnZhbHVlIGFzICdsZWZ0JyB8ICdyaWdodCc7XG4gICAgICAgICAgICAgICAgICAgIHByb3BzLlNldEF4ZXMoYSlcbiAgICAgICAgICAgICAgICB9fT5cbiAgICAgICAgICAgICAgICAgICAgPG9wdGlvbiB2YWx1ZT0nbGVmdCc+bGVmdDwvb3B0aW9uPlxuICAgICAgICAgICAgICAgICAgICA8b3B0aW9uIHZhbHVlPSdyaWdodCc+cmlnaHQ8L29wdGlvbj5cbiAgICAgICAgICAgICAgICA8L3NlbGVjdD5cbiAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9J3Jvdyc+XG4gICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9J2NvbC1sZy02Jz5cbiAgICAgICAgICAgICAgICAgICAgPGRpdiBjbGFzc05hbWU9J2Zvcm0tZ3JvdXAnPlxuICAgICAgICAgICAgICAgICAgICAgICAgPGxhYmVsPk1pbjwvbGFiZWw+XG4gICAgICAgICAgICAgICAgICAgICAgICA8aW5wdXQgY2xhc3NOYW1lPSdmb3JtLWNvbnRyb2wnIHR5cGU9XCJudW1iZXJcIiB2YWx1ZT17cHJvcHMuQXhlc1twcm9wcy5JbmRleF0/Lm1pbiA/PyAnJ30gb25DaGFuZ2U9eyhldnQpID0+IHtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBsZXQgYXhlcyA9IFsuLi5wcm9wcy5BeGVzXTtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBpZiAoZXZ0LnRhcmdldC52YWx1ZSA9PSAnJylcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgZGVsZXRlIGF4ZXNbcHJvcHMuSW5kZXhdLm1pbjtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBlbHNlXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGF4ZXNbcHJvcHMuSW5kZXhdLm1pbiA9IHBhcnNlRmxvYXQoZXZ0LnRhcmdldC52YWx1ZSk7XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgcHJvcHMuU2V0QXhlcyhheGVzKVxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIH19IC8+IFxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT0nY29sLWxnLTYnPlxuICAgICAgICAgICAgICAgICAgICA8ZGl2IGNsYXNzTmFtZT0nZm9ybS1ncm91cCc+XG4gICAgICAgICAgICAgICAgICAgICAgICA8bGFiZWw+TWF4PC9sYWJlbD5cbiAgICAgICAgICAgICAgICAgICAgICAgIDxpbnB1dCBjbGFzc05hbWU9J2Zvcm0tY29udHJvbCcgdHlwZT1cIm51bWJlclwiIHZhbHVlPXtwcm9wcy5BeGVzW3Byb3BzLkluZGV4XT8ubWF4ID8/ICcnfSBvbkNoYW5nZT17KGV2dCkgPT4ge1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGxldCBheGVzID0gWy4uLnByb3BzLkF4ZXNdO1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGlmIChldnQudGFyZ2V0LnZhbHVlID09ICcnKVxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBkZWxldGUgYXhlc1twcm9wcy5JbmRleF0ubWF4O1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGVsc2VcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgYXhlc1twcm9wcy5JbmRleF0ubWF4ID0gcGFyc2VGbG9hdChldnQudGFyZ2V0LnZhbHVlKTtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBwcm9wcy5TZXRBeGVzKGF4ZXMpXG4gICAgICAgICAgICAgICAgICAgICAgICB9fSAvPlxuICAgICAgICAgICAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgICAgICAgICA8L2Rpdj5cblxuICAgICAgICAgICAgPC9kaXY+XG4gICAgICAgICAgICA8YnV0dG9uIGNsYXNzTmFtZT0nYnRuIGJ0bi1pbmZvJyBvbkNsaWNrPXsoKSA9PiB7XG4gICAgICAgICAgICAgICAgbGV0IGF4ZXMgPSBbLi4ucHJvcHMuQXhlc107XG4gICAgICAgICAgICAgICAgZGVsZXRlIGF4ZXNbcHJvcHMuSW5kZXhdLm1heDtcbiAgICAgICAgICAgICAgICBkZWxldGUgYXhlc1twcm9wcy5JbmRleF0ubWluO1xuXG4gICAgICAgICAgICAgICAgcHJvcHMuU2V0QXhlcyhheGVzKVxuXG4gICAgICAgICAgICB9fSA+VXNlIERlZmF1bHQ8L2J1dHRvbj5cblxuICAgICAgICA8L2xpPlxuXG4pO1xufVxuXG5SZWFjdERPTS5yZW5kZXIoPFRyZW5kaW5nRGF0YURpc3BsYXkgLz4sIGRvY3VtZW50LmdldEVsZW1lbnRCeUlkKCdib2R5Q29udGFpbmVyJykpOyIsIi8qXG5BeGlzIExhYmVscyBQbHVnaW4gZm9yIGZsb3QuXG5odHRwOi8vZ2l0aHViLmNvbS9tYXJrcmNvdGUvZmxvdC1heGlzbGFiZWxzXG5PcmlnaW5hbCBjb2RlIGlzIENvcHlyaWdodCAoYykgMjAxMCBYdWFuIEx1by5cbk9yaWdpbmFsIGNvZGUgd2FzIHJlbGVhc2VkIHVuZGVyIHRoZSBHUEx2MyBsaWNlbnNlIGJ5IFh1YW4gTHVvLCBTZXB0ZW1iZXIgMjAxMC5cbk9yaWdpbmFsIGNvZGUgd2FzIHJlcmVsZWFzZWQgdW5kZXIgdGhlIE1JVCBsaWNlbnNlIGJ5IFh1YW4gTHVvLCBBcHJpbCAyMDEyLlxuUGVybWlzc2lvbiBpcyBoZXJlYnkgZ3JhbnRlZCwgZnJlZSBvZiBjaGFyZ2UsIHRvIGFueSBwZXJzb24gb2J0YWluaW5nXG5hIGNvcHkgb2YgdGhpcyBzb2Z0d2FyZSBhbmQgYXNzb2NpYXRlZCBkb2N1bWVudGF0aW9uIGZpbGVzICh0aGVcblwiU29mdHdhcmVcIiksIHRvIGRlYWwgaW4gdGhlIFNvZnR3YXJlIHdpdGhvdXQgcmVzdHJpY3Rpb24sIGluY2x1ZGluZ1xud2l0aG91dCBsaW1pdGF0aW9uIHRoZSByaWdodHMgdG8gdXNlLCBjb3B5LCBtb2RpZnksIG1lcmdlLCBwdWJsaXNoLFxuZGlzdHJpYnV0ZSwgc3VibGljZW5zZSwgYW5kL29yIHNlbGwgY29waWVzIG9mIHRoZSBTb2Z0d2FyZSwgYW5kIHRvXG5wZXJtaXQgcGVyc29ucyB0byB3aG9tIHRoZSBTb2Z0d2FyZSBpcyBmdXJuaXNoZWQgdG8gZG8gc28sIHN1YmplY3QgdG9cbnRoZSBmb2xsb3dpbmcgY29uZGl0aW9uczpcblRoZSBhYm92ZSBjb3B5cmlnaHQgbm90aWNlIGFuZCB0aGlzIHBlcm1pc3Npb24gbm90aWNlIHNoYWxsIGJlXG5pbmNsdWRlZCBpbiBhbGwgY29waWVzIG9yIHN1YnN0YW50aWFsIHBvcnRpb25zIG9mIHRoZSBTb2Z0d2FyZS5cblRIRSBTT0ZUV0FSRSBJUyBQUk9WSURFRCBcIkFTIElTXCIsIFdJVEhPVVQgV0FSUkFOVFkgT0YgQU5ZIEtJTkQsXG5FWFBSRVNTIE9SIElNUExJRUQsIElOQ0xVRElORyBCVVQgTk9UIExJTUlURUQgVE8gVEhFIFdBUlJBTlRJRVMgT0Zcbk1FUkNIQU5UQUJJTElUWSwgRklUTkVTUyBGT1IgQSBQQVJUSUNVTEFSIFBVUlBPU0UgQU5EXG5OT05JTkZSSU5HRU1FTlQuIElOIE5PIEVWRU5UIFNIQUxMIFRIRSBBVVRIT1JTIE9SIENPUFlSSUdIVCBIT0xERVJTIEJFXG5MSUFCTEUgRk9SIEFOWSBDTEFJTSwgREFNQUdFUyBPUiBPVEhFUiBMSUFCSUxJVFksIFdIRVRIRVIgSU4gQU4gQUNUSU9OXG5PRiBDT05UUkFDVCwgVE9SVCBPUiBPVEhFUldJU0UsIEFSSVNJTkcgRlJPTSwgT1VUIE9GIE9SIElOIENPTk5FQ1RJT05cbldJVEggVEhFIFNPRlRXQVJFIE9SIFRIRSBVU0UgT1IgT1RIRVIgREVBTElOR1MgSU4gVEhFIFNPRlRXQVJFLlxuICovXG5cbihmdW5jdGlvbiAoJCkge1xuICAgIHZhciBvcHRpb25zID0ge1xuICAgICAgYXhpc0xhYmVsczoge1xuICAgICAgICBzaG93OiB0cnVlXG4gICAgICB9XG4gICAgfTtcblxuICAgIGZ1bmN0aW9uIGNhbnZhc1N1cHBvcnRlZCgpIHtcbiAgICAgICAgcmV0dXJuICEhZG9jdW1lbnQuY3JlYXRlRWxlbWVudCgnY2FudmFzJykuZ2V0Q29udGV4dDtcbiAgICB9XG5cbiAgICBmdW5jdGlvbiBjYW52YXNUZXh0U3VwcG9ydGVkKCkge1xuICAgICAgICBpZiAoIWNhbnZhc1N1cHBvcnRlZCgpKSB7XG4gICAgICAgICAgICByZXR1cm4gZmFsc2U7XG4gICAgICAgIH1cbiAgICAgICAgdmFyIGR1bW15X2NhbnZhcyA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoJ2NhbnZhcycpO1xuICAgICAgICB2YXIgY29udGV4dCA9IGR1bW15X2NhbnZhcy5nZXRDb250ZXh0KCcyZCcpO1xuICAgICAgICByZXR1cm4gdHlwZW9mIGNvbnRleHQuZmlsbFRleHQgPT0gJ2Z1bmN0aW9uJztcbiAgICB9XG5cbiAgICBmdW5jdGlvbiBjc3MzVHJhbnNpdGlvblN1cHBvcnRlZCgpIHtcbiAgICAgICAgdmFyIGRpdiA9IGRvY3VtZW50LmNyZWF0ZUVsZW1lbnQoJ2RpdicpO1xuICAgICAgICByZXR1cm4gdHlwZW9mIGRpdi5zdHlsZS5Nb3pUcmFuc2l0aW9uICE9ICd1bmRlZmluZWQnICAgIC8vIEdlY2tvXG4gICAgICAgICAgICB8fCB0eXBlb2YgZGl2LnN0eWxlLk9UcmFuc2l0aW9uICE9ICd1bmRlZmluZWQnICAgICAgLy8gT3BlcmFcbiAgICAgICAgICAgIHx8IHR5cGVvZiBkaXYuc3R5bGUud2Via2l0VHJhbnNpdGlvbiAhPSAndW5kZWZpbmVkJyAvLyBXZWJLaXRcbiAgICAgICAgICAgIHx8IHR5cGVvZiBkaXYuc3R5bGUudHJhbnNpdGlvbiAhPSAndW5kZWZpbmVkJztcbiAgICB9XG5cblxuICAgIGZ1bmN0aW9uIEF4aXNMYWJlbChheGlzTmFtZSwgcG9zaXRpb24sIHBhZGRpbmcsIHBsb3QsIG9wdHMpIHtcbiAgICAgICAgdGhpcy5heGlzTmFtZSA9IGF4aXNOYW1lO1xuICAgICAgICB0aGlzLnBvc2l0aW9uID0gcG9zaXRpb247XG4gICAgICAgIHRoaXMucGFkZGluZyA9IHBhZGRpbmc7XG4gICAgICAgIHRoaXMucGxvdCA9IHBsb3Q7XG4gICAgICAgIHRoaXMub3B0cyA9IG9wdHM7XG4gICAgICAgIHRoaXMud2lkdGggPSAwO1xuICAgICAgICB0aGlzLmhlaWdodCA9IDA7XG4gICAgfVxuXG4gICAgQXhpc0xhYmVsLnByb3RvdHlwZS5jbGVhbnVwID0gZnVuY3Rpb24oKSB7XG4gICAgfTtcblxuXG4gICAgQ2FudmFzQXhpc0xhYmVsLnByb3RvdHlwZSA9IG5ldyBBeGlzTGFiZWwoKTtcbiAgICBDYW52YXNBeGlzTGFiZWwucHJvdG90eXBlLmNvbnN0cnVjdG9yID0gQ2FudmFzQXhpc0xhYmVsO1xuICAgIGZ1bmN0aW9uIENhbnZhc0F4aXNMYWJlbChheGlzTmFtZSwgcG9zaXRpb24sIHBhZGRpbmcsIHBsb3QsIG9wdHMpIHtcbiAgICAgICAgQXhpc0xhYmVsLnByb3RvdHlwZS5jb25zdHJ1Y3Rvci5jYWxsKHRoaXMsIGF4aXNOYW1lLCBwb3NpdGlvbiwgcGFkZGluZyxcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHBsb3QsIG9wdHMpO1xuICAgIH1cblxuICAgIENhbnZhc0F4aXNMYWJlbC5wcm90b3R5cGUuY2FsY3VsYXRlU2l6ZSA9IGZ1bmN0aW9uKCkge1xuICAgICAgICBpZiAoIXRoaXMub3B0cy5heGlzTGFiZWxGb250U2l6ZVBpeGVscylcbiAgICAgICAgICAgIHRoaXMub3B0cy5heGlzTGFiZWxGb250U2l6ZVBpeGVscyA9IDE0O1xuICAgICAgICBpZiAoIXRoaXMub3B0cy5heGlzTGFiZWxGb250RmFtaWx5KVxuICAgICAgICAgICAgdGhpcy5vcHRzLmF4aXNMYWJlbEZvbnRGYW1pbHkgPSAnc2Fucy1zZXJpZic7XG5cbiAgICAgICAgdmFyIHRleHRXaWR0aCA9IHRoaXMub3B0cy5heGlzTGFiZWxGb250U2l6ZVBpeGVscyArIHRoaXMucGFkZGluZztcbiAgICAgICAgdmFyIHRleHRIZWlnaHQgPSB0aGlzLm9wdHMuYXhpc0xhYmVsRm9udFNpemVQaXhlbHMgKyB0aGlzLnBhZGRpbmc7XG4gICAgICAgIGlmICh0aGlzLnBvc2l0aW9uID09ICdsZWZ0JyB8fCB0aGlzLnBvc2l0aW9uID09ICdyaWdodCcpIHtcbiAgICAgICAgICAgIHRoaXMud2lkdGggPSB0aGlzLm9wdHMuYXhpc0xhYmVsRm9udFNpemVQaXhlbHMgKyB0aGlzLnBhZGRpbmc7XG4gICAgICAgICAgICB0aGlzLmhlaWdodCA9IDA7XG4gICAgICAgIH0gZWxzZSB7XG4gICAgICAgICAgICB0aGlzLndpZHRoID0gMDtcbiAgICAgICAgICAgIHRoaXMuaGVpZ2h0ID0gdGhpcy5vcHRzLmF4aXNMYWJlbEZvbnRTaXplUGl4ZWxzICsgdGhpcy5wYWRkaW5nO1xuICAgICAgICB9XG4gICAgfTtcblxuICAgIENhbnZhc0F4aXNMYWJlbC5wcm90b3R5cGUuZHJhdyA9IGZ1bmN0aW9uKGJveCkge1xuICAgICAgICBpZiAoIXRoaXMub3B0cy5heGlzTGFiZWxDb2xvdXIpXG4gICAgICAgICAgICB0aGlzLm9wdHMuYXhpc0xhYmVsQ29sb3VyID0gJ2JsYWNrJztcbiAgICAgICAgdmFyIGN0eCA9IHRoaXMucGxvdC5nZXRDYW52YXMoKS5nZXRDb250ZXh0KCcyZCcpO1xuICAgICAgICBjdHguc2F2ZSgpO1xuICAgICAgICBjdHguZm9udCA9IHRoaXMub3B0cy5heGlzTGFiZWxGb250U2l6ZVBpeGVscyArICdweCAnICtcbiAgICAgICAgICAgIHRoaXMub3B0cy5heGlzTGFiZWxGb250RmFtaWx5O1xuICAgICAgICBjdHguZmlsbFN0eWxlID0gdGhpcy5vcHRzLmF4aXNMYWJlbENvbG91cjtcbiAgICAgICAgdmFyIHdpZHRoID0gY3R4Lm1lYXN1cmVUZXh0KHRoaXMub3B0cy5heGlzTGFiZWwpLndpZHRoO1xuICAgICAgICB2YXIgaGVpZ2h0ID0gdGhpcy5vcHRzLmF4aXNMYWJlbEZvbnRTaXplUGl4ZWxzO1xuICAgICAgICB2YXIgeCwgeSwgYW5nbGUgPSAwO1xuICAgICAgICBpZiAodGhpcy5wb3NpdGlvbiA9PSAndG9wJykge1xuICAgICAgICAgICAgeCA9IGJveC5sZWZ0ICsgYm94LndpZHRoLzIgLSB3aWR0aC8yO1xuICAgICAgICAgICAgeSA9IGJveC50b3AgKyBoZWlnaHQqMC43MjtcbiAgICAgICAgfSBlbHNlIGlmICh0aGlzLnBvc2l0aW9uID09ICdib3R0b20nKSB7XG4gICAgICAgICAgICB4ID0gYm94LmxlZnQgKyBib3gud2lkdGgvMiAtIHdpZHRoLzI7XG4gICAgICAgICAgICB5ID0gYm94LnRvcCArIGJveC5oZWlnaHQgLSBoZWlnaHQqMC43MjtcbiAgICAgICAgfSBlbHNlIGlmICh0aGlzLnBvc2l0aW9uID09ICdsZWZ0Jykge1xuICAgICAgICAgICAgeCA9IGJveC5sZWZ0ICsgaGVpZ2h0KjAuNzI7XG4gICAgICAgICAgICB5ID0gYm94LmhlaWdodC8yICsgYm94LnRvcCArIHdpZHRoLzI7XG4gICAgICAgICAgICBhbmdsZSA9IC1NYXRoLlBJLzI7XG4gICAgICAgIH0gZWxzZSBpZiAodGhpcy5wb3NpdGlvbiA9PSAncmlnaHQnKSB7XG4gICAgICAgICAgICB4ID0gYm94LmxlZnQgKyBib3gud2lkdGggLSBoZWlnaHQqMC43MjtcbiAgICAgICAgICAgIHkgPSBib3guaGVpZ2h0LzIgKyBib3gudG9wIC0gd2lkdGgvMjtcbiAgICAgICAgICAgIGFuZ2xlID0gTWF0aC5QSS8yO1xuICAgICAgICB9XG4gICAgICAgIGN0eC50cmFuc2xhdGUoeCwgeSk7XG4gICAgICAgIGN0eC5yb3RhdGUoYW5nbGUpO1xuICAgICAgICBjdHguZmlsbFRleHQodGhpcy5vcHRzLmF4aXNMYWJlbCwgMCwgMCk7XG4gICAgICAgIGN0eC5yZXN0b3JlKCk7XG4gICAgfTtcblxuXG4gICAgSHRtbEF4aXNMYWJlbC5wcm90b3R5cGUgPSBuZXcgQXhpc0xhYmVsKCk7XG4gICAgSHRtbEF4aXNMYWJlbC5wcm90b3R5cGUuY29uc3RydWN0b3IgPSBIdG1sQXhpc0xhYmVsO1xuICAgIGZ1bmN0aW9uIEh0bWxBeGlzTGFiZWwoYXhpc05hbWUsIHBvc2l0aW9uLCBwYWRkaW5nLCBwbG90LCBvcHRzKSB7XG4gICAgICAgIEF4aXNMYWJlbC5wcm90b3R5cGUuY29uc3RydWN0b3IuY2FsbCh0aGlzLCBheGlzTmFtZSwgcG9zaXRpb24sXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBwYWRkaW5nLCBwbG90LCBvcHRzKTtcbiAgICAgICAgdGhpcy5lbGVtID0gbnVsbDtcbiAgICB9XG5cbiAgICBIdG1sQXhpc0xhYmVsLnByb3RvdHlwZS5jYWxjdWxhdGVTaXplID0gZnVuY3Rpb24oKSB7XG4gICAgICAgIHZhciBlbGVtID0gJCgnPGRpdiBjbGFzcz1cImF4aXNMYWJlbHNcIiBzdHlsZT1cInBvc2l0aW9uOmFic29sdXRlO1wiPicgK1xuICAgICAgICAgICAgICAgICAgICAgdGhpcy5vcHRzLmF4aXNMYWJlbCArICc8L2Rpdj4nKTtcbiAgICAgICAgdGhpcy5wbG90LmdldFBsYWNlaG9sZGVyKCkuYXBwZW5kKGVsZW0pO1xuICAgICAgICAvLyBzdG9yZSBoZWlnaHQgYW5kIHdpZHRoIG9mIGxhYmVsIGl0c2VsZiwgZm9yIHVzZSBpbiBkcmF3KClcbiAgICAgICAgdGhpcy5sYWJlbFdpZHRoID0gZWxlbS5vdXRlcldpZHRoKHRydWUpO1xuICAgICAgICB0aGlzLmxhYmVsSGVpZ2h0ID0gZWxlbS5vdXRlckhlaWdodCh0cnVlKTtcbiAgICAgICAgZWxlbS5yZW1vdmUoKTtcblxuICAgICAgICB0aGlzLndpZHRoID0gdGhpcy5oZWlnaHQgPSAwO1xuICAgICAgICBpZiAodGhpcy5wb3NpdGlvbiA9PSAnbGVmdCcgfHwgdGhpcy5wb3NpdGlvbiA9PSAncmlnaHQnKSB7XG4gICAgICAgICAgICB0aGlzLndpZHRoID0gdGhpcy5sYWJlbFdpZHRoICsgdGhpcy5wYWRkaW5nO1xuICAgICAgICB9IGVsc2Uge1xuICAgICAgICAgICAgdGhpcy5oZWlnaHQgPSB0aGlzLmxhYmVsSGVpZ2h0ICsgdGhpcy5wYWRkaW5nO1xuICAgICAgICB9XG4gICAgfTtcblxuICAgIEh0bWxBeGlzTGFiZWwucHJvdG90eXBlLmNsZWFudXAgPSBmdW5jdGlvbigpIHtcbiAgICAgICAgaWYgKHRoaXMuZWxlbSkge1xuICAgICAgICAgICAgdGhpcy5lbGVtLnJlbW92ZSgpO1xuICAgICAgICB9XG4gICAgfTtcblxuICAgIEh0bWxBeGlzTGFiZWwucHJvdG90eXBlLmRyYXcgPSBmdW5jdGlvbihib3gpIHtcbiAgICAgICAgdGhpcy5wbG90LmdldFBsYWNlaG9sZGVyKCkuZmluZCgnIycgKyB0aGlzLmF4aXNOYW1lICsgJ0xhYmVsJykucmVtb3ZlKCk7XG4gICAgICAgIHRoaXMuZWxlbSA9ICQoJzxkaXYgaWQ9XCInICsgdGhpcy5heGlzTmFtZSArXG4gICAgICAgICAgICAgICAgICAgICAgJ0xhYmVsXCIgXCIgY2xhc3M9XCJheGlzTGFiZWxzXCIgc3R5bGU9XCJwb3NpdGlvbjphYnNvbHV0ZTtcIj4nXG4gICAgICAgICAgICAgICAgICAgICAgKyB0aGlzLm9wdHMuYXhpc0xhYmVsICsgJzwvZGl2PicpO1xuICAgICAgICB0aGlzLnBsb3QuZ2V0UGxhY2Vob2xkZXIoKS5hcHBlbmQodGhpcy5lbGVtKTtcbiAgICAgICAgaWYgKHRoaXMucG9zaXRpb24gPT0gJ3RvcCcpIHtcbiAgICAgICAgICAgIHRoaXMuZWxlbS5jc3MoJ2xlZnQnLCBib3gubGVmdCArIGJveC53aWR0aC8yIC0gdGhpcy5sYWJlbFdpZHRoLzIgK1xuICAgICAgICAgICAgICAgICAgICAgICAgICAncHgnKTtcbiAgICAgICAgICAgIHRoaXMuZWxlbS5jc3MoJ3RvcCcsIGJveC50b3AgKyAncHgnKTtcbiAgICAgICAgfSBlbHNlIGlmICh0aGlzLnBvc2l0aW9uID09ICdib3R0b20nKSB7XG4gICAgICAgICAgICB0aGlzLmVsZW0uY3NzKCdsZWZ0JywgYm94LmxlZnQgKyBib3gud2lkdGgvMiAtIHRoaXMubGFiZWxXaWR0aC8yICtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgJ3B4Jyk7XG4gICAgICAgICAgICB0aGlzLmVsZW0uY3NzKCd0b3AnLCBib3gudG9wICsgYm94LmhlaWdodCAtIHRoaXMubGFiZWxIZWlnaHQgK1xuICAgICAgICAgICAgICAgICAgICAgICAgICAncHgnKTtcbiAgICAgICAgfSBlbHNlIGlmICh0aGlzLnBvc2l0aW9uID09ICdsZWZ0Jykge1xuICAgICAgICAgICAgdGhpcy5lbGVtLmNzcygndG9wJywgYm94LnRvcCArIGJveC5oZWlnaHQvMiAtIHRoaXMubGFiZWxIZWlnaHQvMiArXG4gICAgICAgICAgICAgICAgICAgICAgICAgICdweCcpO1xuICAgICAgICAgICAgdGhpcy5lbGVtLmNzcygnbGVmdCcsIGJveC5sZWZ0ICsgJ3B4Jyk7XG4gICAgICAgIH0gZWxzZSBpZiAodGhpcy5wb3NpdGlvbiA9PSAncmlnaHQnKSB7XG4gICAgICAgICAgICB0aGlzLmVsZW0uY3NzKCd0b3AnLCBib3gudG9wICsgYm94LmhlaWdodC8yIC0gdGhpcy5sYWJlbEhlaWdodC8yICtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgJ3B4Jyk7XG4gICAgICAgICAgICB0aGlzLmVsZW0uY3NzKCdsZWZ0JywgYm94LmxlZnQgKyBib3gud2lkdGggLSB0aGlzLmxhYmVsV2lkdGggK1xuICAgICAgICAgICAgICAgICAgICAgICAgICAncHgnKTtcbiAgICAgICAgfVxuICAgIH07XG5cblxuICAgIENzc1RyYW5zZm9ybUF4aXNMYWJlbC5wcm90b3R5cGUgPSBuZXcgSHRtbEF4aXNMYWJlbCgpO1xuICAgIENzc1RyYW5zZm9ybUF4aXNMYWJlbC5wcm90b3R5cGUuY29uc3RydWN0b3IgPSBDc3NUcmFuc2Zvcm1BeGlzTGFiZWw7XG4gICAgZnVuY3Rpb24gQ3NzVHJhbnNmb3JtQXhpc0xhYmVsKGF4aXNOYW1lLCBwb3NpdGlvbiwgcGFkZGluZywgcGxvdCwgb3B0cykge1xuICAgICAgICBIdG1sQXhpc0xhYmVsLnByb3RvdHlwZS5jb25zdHJ1Y3Rvci5jYWxsKHRoaXMsIGF4aXNOYW1lLCBwb3NpdGlvbixcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBwYWRkaW5nLCBwbG90LCBvcHRzKTtcbiAgICB9XG5cbiAgICBDc3NUcmFuc2Zvcm1BeGlzTGFiZWwucHJvdG90eXBlLmNhbGN1bGF0ZVNpemUgPSBmdW5jdGlvbigpIHtcbiAgICAgICAgSHRtbEF4aXNMYWJlbC5wcm90b3R5cGUuY2FsY3VsYXRlU2l6ZS5jYWxsKHRoaXMpO1xuICAgICAgICB0aGlzLndpZHRoID0gdGhpcy5oZWlnaHQgPSAwO1xuICAgICAgICBpZiAodGhpcy5wb3NpdGlvbiA9PSAnbGVmdCcgfHwgdGhpcy5wb3NpdGlvbiA9PSAncmlnaHQnKSB7XG4gICAgICAgICAgICB0aGlzLndpZHRoID0gdGhpcy5sYWJlbEhlaWdodCArIHRoaXMucGFkZGluZztcbiAgICAgICAgfSBlbHNlIHtcbiAgICAgICAgICAgIHRoaXMuaGVpZ2h0ID0gdGhpcy5sYWJlbEhlaWdodCArIHRoaXMucGFkZGluZztcbiAgICAgICAgfVxuICAgIH07XG5cbiAgICBDc3NUcmFuc2Zvcm1BeGlzTGFiZWwucHJvdG90eXBlLnRyYW5zZm9ybXMgPSBmdW5jdGlvbihkZWdyZWVzLCB4LCB5KSB7XG4gICAgICAgIHZhciBzdHJhbnNmb3JtcyA9IHtcbiAgICAgICAgICAgICctbW96LXRyYW5zZm9ybSc6ICcnLFxuICAgICAgICAgICAgJy13ZWJraXQtdHJhbnNmb3JtJzogJycsXG4gICAgICAgICAgICAnLW8tdHJhbnNmb3JtJzogJycsXG4gICAgICAgICAgICAnLW1zLXRyYW5zZm9ybSc6ICcnXG4gICAgICAgIH07XG4gICAgICAgIGlmICh4ICE9IDAgfHwgeSAhPSAwKSB7XG4gICAgICAgICAgICB2YXIgc3RkVHJhbnNsYXRlID0gJyB0cmFuc2xhdGUoJyArIHggKyAncHgsICcgKyB5ICsgJ3B4KSc7XG4gICAgICAgICAgICBzdHJhbnNmb3Jtc1snLW1vei10cmFuc2Zvcm0nXSArPSBzdGRUcmFuc2xhdGU7XG4gICAgICAgICAgICBzdHJhbnNmb3Jtc1snLXdlYmtpdC10cmFuc2Zvcm0nXSArPSBzdGRUcmFuc2xhdGU7XG4gICAgICAgICAgICBzdHJhbnNmb3Jtc1snLW8tdHJhbnNmb3JtJ10gKz0gc3RkVHJhbnNsYXRlO1xuICAgICAgICAgICAgc3RyYW5zZm9ybXNbJy1tcy10cmFuc2Zvcm0nXSArPSBzdGRUcmFuc2xhdGU7XG4gICAgICAgIH1cbiAgICAgICAgaWYgKGRlZ3JlZXMgIT0gMCkge1xuICAgICAgICAgICAgdmFyIHJvdGF0aW9uID0gZGVncmVlcyAvIDkwO1xuICAgICAgICAgICAgdmFyIHN0ZFJvdGF0ZSA9ICcgcm90YXRlKCcgKyBkZWdyZWVzICsgJ2RlZyknO1xuICAgICAgICAgICAgc3RyYW5zZm9ybXNbJy1tb3otdHJhbnNmb3JtJ10gKz0gc3RkUm90YXRlO1xuICAgICAgICAgICAgc3RyYW5zZm9ybXNbJy13ZWJraXQtdHJhbnNmb3JtJ10gKz0gc3RkUm90YXRlO1xuICAgICAgICAgICAgc3RyYW5zZm9ybXNbJy1vLXRyYW5zZm9ybSddICs9IHN0ZFJvdGF0ZTtcbiAgICAgICAgICAgIHN0cmFuc2Zvcm1zWyctbXMtdHJhbnNmb3JtJ10gKz0gc3RkUm90YXRlO1xuICAgICAgICB9XG4gICAgICAgIHZhciBzID0gJ3RvcDogMDsgbGVmdDogMDsgJztcbiAgICAgICAgZm9yICh2YXIgcHJvcCBpbiBzdHJhbnNmb3Jtcykge1xuICAgICAgICAgICAgaWYgKHN0cmFuc2Zvcm1zW3Byb3BdKSB7XG4gICAgICAgICAgICAgICAgcyArPSBwcm9wICsgJzonICsgc3RyYW5zZm9ybXNbcHJvcF0gKyAnOyc7XG4gICAgICAgICAgICB9XG4gICAgICAgIH1cbiAgICAgICAgcyArPSAnOyc7XG4gICAgICAgIHJldHVybiBzO1xuICAgIH07XG5cbiAgICBDc3NUcmFuc2Zvcm1BeGlzTGFiZWwucHJvdG90eXBlLmNhbGN1bGF0ZU9mZnNldHMgPSBmdW5jdGlvbihib3gpIHtcbiAgICAgICAgdmFyIG9mZnNldHMgPSB7IHg6IDAsIHk6IDAsIGRlZ3JlZXM6IDAgfTtcbiAgICAgICAgaWYgKHRoaXMucG9zaXRpb24gPT0gJ2JvdHRvbScpIHtcbiAgICAgICAgICAgIG9mZnNldHMueCA9IGJveC5sZWZ0ICsgYm94LndpZHRoLzIgLSB0aGlzLmxhYmVsV2lkdGgvMjtcbiAgICAgICAgICAgIG9mZnNldHMueSA9IGJveC50b3AgKyBib3guaGVpZ2h0IC0gdGhpcy5sYWJlbEhlaWdodDtcbiAgICAgICAgfSBlbHNlIGlmICh0aGlzLnBvc2l0aW9uID09ICd0b3AnKSB7XG4gICAgICAgICAgICBvZmZzZXRzLnggPSBib3gubGVmdCArIGJveC53aWR0aC8yIC0gdGhpcy5sYWJlbFdpZHRoLzI7XG4gICAgICAgICAgICBvZmZzZXRzLnkgPSBib3gudG9wO1xuICAgICAgICB9IGVsc2UgaWYgKHRoaXMucG9zaXRpb24gPT0gJ2xlZnQnKSB7XG4gICAgICAgICAgICBvZmZzZXRzLmRlZ3JlZXMgPSAtOTA7XG4gICAgICAgICAgICBvZmZzZXRzLnggPSBib3gubGVmdCAtIHRoaXMubGFiZWxXaWR0aC8yICsgdGhpcy5sYWJlbEhlaWdodC8yO1xuICAgICAgICAgICAgb2Zmc2V0cy55ID0gYm94LmhlaWdodC8yICsgYm94LnRvcDtcbiAgICAgICAgfSBlbHNlIGlmICh0aGlzLnBvc2l0aW9uID09ICdyaWdodCcpIHtcbiAgICAgICAgICAgIG9mZnNldHMuZGVncmVlcyA9IDkwO1xuICAgICAgICAgICAgb2Zmc2V0cy54ID0gYm94LmxlZnQgKyBib3gud2lkdGggLSB0aGlzLmxhYmVsV2lkdGgvMlxuICAgICAgICAgICAgICAgICAgICAgICAgLSB0aGlzLmxhYmVsSGVpZ2h0LzI7XG4gICAgICAgICAgICBvZmZzZXRzLnkgPSBib3guaGVpZ2h0LzIgKyBib3gudG9wO1xuICAgICAgICB9XG4gICAgICAgIG9mZnNldHMueCA9IE1hdGgucm91bmQob2Zmc2V0cy54KTtcbiAgICAgICAgb2Zmc2V0cy55ID0gTWF0aC5yb3VuZChvZmZzZXRzLnkpO1xuXG4gICAgICAgIHJldHVybiBvZmZzZXRzO1xuICAgIH07XG5cbiAgICBDc3NUcmFuc2Zvcm1BeGlzTGFiZWwucHJvdG90eXBlLmRyYXcgPSBmdW5jdGlvbihib3gpIHtcbiAgICAgICAgdGhpcy5wbG90LmdldFBsYWNlaG9sZGVyKCkuZmluZChcIi5cIiArIHRoaXMuYXhpc05hbWUgKyBcIkxhYmVsXCIpLnJlbW92ZSgpO1xuICAgICAgICB2YXIgb2Zmc2V0cyA9IHRoaXMuY2FsY3VsYXRlT2Zmc2V0cyhib3gpO1xuICAgICAgICB0aGlzLmVsZW0gPSAkKCc8ZGl2IGNsYXNzPVwiYXhpc0xhYmVscyAnICsgdGhpcy5heGlzTmFtZSArXG4gICAgICAgICAgICAgICAgICAgICAgJ0xhYmVsXCIgc3R5bGU9XCJwb3NpdGlvbjphYnNvbHV0ZTsgJyArXG4gICAgICAgICAgICAgICAgICAgICAgdGhpcy50cmFuc2Zvcm1zKG9mZnNldHMuZGVncmVlcywgb2Zmc2V0cy54LCBvZmZzZXRzLnkpICtcbiAgICAgICAgICAgICAgICAgICAgICAnXCI+JyArIHRoaXMub3B0cy5heGlzTGFiZWwgKyAnPC9kaXY+Jyk7XG4gICAgICAgIHRoaXMucGxvdC5nZXRQbGFjZWhvbGRlcigpLmFwcGVuZCh0aGlzLmVsZW0pO1xuICAgIH07XG5cblxuICAgIEllVHJhbnNmb3JtQXhpc0xhYmVsLnByb3RvdHlwZSA9IG5ldyBDc3NUcmFuc2Zvcm1BeGlzTGFiZWwoKTtcbiAgICBJZVRyYW5zZm9ybUF4aXNMYWJlbC5wcm90b3R5cGUuY29uc3RydWN0b3IgPSBJZVRyYW5zZm9ybUF4aXNMYWJlbDtcbiAgICBmdW5jdGlvbiBJZVRyYW5zZm9ybUF4aXNMYWJlbChheGlzTmFtZSwgcG9zaXRpb24sIHBhZGRpbmcsIHBsb3QsIG9wdHMpIHtcbiAgICAgICAgQ3NzVHJhbnNmb3JtQXhpc0xhYmVsLnByb3RvdHlwZS5jb25zdHJ1Y3Rvci5jYWxsKHRoaXMsIGF4aXNOYW1lLFxuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcG9zaXRpb24sIHBhZGRpbmcsXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBwbG90LCBvcHRzKTtcbiAgICAgICAgdGhpcy5yZXF1aXJlc1Jlc2l6ZSA9IGZhbHNlO1xuICAgIH1cblxuICAgIEllVHJhbnNmb3JtQXhpc0xhYmVsLnByb3RvdHlwZS50cmFuc2Zvcm1zID0gZnVuY3Rpb24oZGVncmVlcywgeCwgeSkge1xuICAgICAgICAvLyBJIGRpZG4ndCBmZWVsIGxpa2UgbGVhcm5pbmcgdGhlIGNyYXp5IE1hdHJpeCBzdHVmZiwgc28gdGhpcyB1c2VzXG4gICAgICAgIC8vIGEgY29tYmluYXRpb24gb2YgdGhlIHJvdGF0aW9uIHRyYW5zZm9ybSBhbmQgQ1NTIHBvc2l0aW9uaW5nLlxuICAgICAgICB2YXIgcyA9ICcnO1xuICAgICAgICBpZiAoZGVncmVlcyAhPSAwKSB7XG4gICAgICAgICAgICB2YXIgcm90YXRpb24gPSBkZWdyZWVzLzkwO1xuICAgICAgICAgICAgd2hpbGUgKHJvdGF0aW9uIDwgMCkge1xuICAgICAgICAgICAgICAgIHJvdGF0aW9uICs9IDQ7XG4gICAgICAgICAgICB9XG4gICAgICAgICAgICBzICs9ICcgZmlsdGVyOiBwcm9naWQ6RFhJbWFnZVRyYW5zZm9ybS5NaWNyb3NvZnQuQmFzaWNJbWFnZShyb3RhdGlvbj0nICsgcm90YXRpb24gKyAnKTsgJztcbiAgICAgICAgICAgIC8vIHNlZSBiZWxvd1xuICAgICAgICAgICAgdGhpcy5yZXF1aXJlc1Jlc2l6ZSA9ICh0aGlzLnBvc2l0aW9uID09ICdyaWdodCcpO1xuICAgICAgICB9XG4gICAgICAgIGlmICh4ICE9IDApIHtcbiAgICAgICAgICAgIHMgKz0gJ2xlZnQ6ICcgKyB4ICsgJ3B4OyAnO1xuICAgICAgICB9XG4gICAgICAgIGlmICh5ICE9IDApIHtcbiAgICAgICAgICAgIHMgKz0gJ3RvcDogJyArIHkgKyAncHg7ICc7XG4gICAgICAgIH1cbiAgICAgICAgcmV0dXJuIHM7XG4gICAgfTtcblxuICAgIEllVHJhbnNmb3JtQXhpc0xhYmVsLnByb3RvdHlwZS5jYWxjdWxhdGVPZmZzZXRzID0gZnVuY3Rpb24oYm94KSB7XG4gICAgICAgIHZhciBvZmZzZXRzID0gQ3NzVHJhbnNmb3JtQXhpc0xhYmVsLnByb3RvdHlwZS5jYWxjdWxhdGVPZmZzZXRzLmNhbGwoXG4gICAgICAgICAgICAgICAgICAgICAgICAgIHRoaXMsIGJveCk7XG4gICAgICAgIC8vIGFkanVzdCBzb21lIHZhbHVlcyB0byB0YWtlIGludG8gYWNjb3VudCBkaWZmZXJlbmNlcyBiZXR3ZWVuXG4gICAgICAgIC8vIENTUyBhbmQgSUUgcm90YXRpb25zLlxuICAgICAgICBpZiAodGhpcy5wb3NpdGlvbiA9PSAndG9wJykge1xuICAgICAgICAgICAgLy8gRklYTUU6IG5vdCBzdXJlIHdoeSwgYnV0IHBsYWNpbmcgdGhpcyBleGFjdGx5IGF0IHRoZSB0b3AgY2F1c2VzXG4gICAgICAgICAgICAvLyB0aGUgdG9wIGF4aXMgbGFiZWwgdG8gZmxpcCB0byB0aGUgYm90dG9tLi4uXG4gICAgICAgICAgICBvZmZzZXRzLnkgPSBib3gudG9wICsgMTtcbiAgICAgICAgfSBlbHNlIGlmICh0aGlzLnBvc2l0aW9uID09ICdsZWZ0Jykge1xuICAgICAgICAgICAgb2Zmc2V0cy54ID0gYm94LmxlZnQ7XG4gICAgICAgICAgICBvZmZzZXRzLnkgPSBib3guaGVpZ2h0LzIgKyBib3gudG9wIC0gdGhpcy5sYWJlbFdpZHRoLzI7XG4gICAgICAgIH0gZWxzZSBpZiAodGhpcy5wb3NpdGlvbiA9PSAncmlnaHQnKSB7XG4gICAgICAgICAgICBvZmZzZXRzLnggPSBib3gubGVmdCArIGJveC53aWR0aCAtIHRoaXMubGFiZWxIZWlnaHQ7XG4gICAgICAgICAgICBvZmZzZXRzLnkgPSBib3guaGVpZ2h0LzIgKyBib3gudG9wIC0gdGhpcy5sYWJlbFdpZHRoLzI7XG4gICAgICAgIH1cbiAgICAgICAgcmV0dXJuIG9mZnNldHM7XG4gICAgfTtcblxuICAgIEllVHJhbnNmb3JtQXhpc0xhYmVsLnByb3RvdHlwZS5kcmF3ID0gZnVuY3Rpb24oYm94KSB7XG4gICAgICAgIENzc1RyYW5zZm9ybUF4aXNMYWJlbC5wcm90b3R5cGUuZHJhdy5jYWxsKHRoaXMsIGJveCk7XG4gICAgICAgIGlmICh0aGlzLnJlcXVpcmVzUmVzaXplKSB7XG4gICAgICAgICAgICB0aGlzLmVsZW0gPSB0aGlzLnBsb3QuZ2V0UGxhY2Vob2xkZXIoKS5maW5kKFwiLlwiICsgdGhpcy5heGlzTmFtZSArXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIFwiTGFiZWxcIik7XG4gICAgICAgICAgICAvLyBTaW5jZSB3ZSB1c2VkIENTUyBwb3NpdGlvbmluZyBpbnN0ZWFkIG9mIHRyYW5zZm9ybXMgZm9yXG4gICAgICAgICAgICAvLyB0cmFuc2xhdGluZyB0aGUgZWxlbWVudCwgYW5kIHNpbmNlIHRoZSBwb3NpdGlvbmluZyBpcyBkb25lXG4gICAgICAgICAgICAvLyBiZWZvcmUgYW55IHJvdGF0aW9ucywgd2UgaGF2ZSB0byByZXNldCB0aGUgd2lkdGggYW5kIGhlaWdodFxuICAgICAgICAgICAgLy8gaW4gY2FzZSB0aGUgYnJvd3NlciB3cmFwcGVkIHRoZSB0ZXh0IChzcGVjaWZpY2FsbHkgZm9yIHRoZVxuICAgICAgICAgICAgLy8geTJheGlzKS5cbiAgICAgICAgICAgIHRoaXMuZWxlbS5jc3MoJ3dpZHRoJywgdGhpcy5sYWJlbFdpZHRoKTtcbiAgICAgICAgICAgIHRoaXMuZWxlbS5jc3MoJ2hlaWdodCcsIHRoaXMubGFiZWxIZWlnaHQpO1xuICAgICAgICB9XG4gICAgfTtcblxuXG4gICAgZnVuY3Rpb24gaW5pdChwbG90KSB7XG4gICAgICAgIHBsb3QuaG9va3MucHJvY2Vzc09wdGlvbnMucHVzaChmdW5jdGlvbiAocGxvdCwgb3B0aW9ucykge1xuXG4gICAgICAgICAgICBpZiAoIW9wdGlvbnMuYXhpc0xhYmVscy5zaG93KVxuICAgICAgICAgICAgICAgIHJldHVybjtcblxuICAgICAgICAgICAgLy8gVGhpcyBpcyBraW5kIG9mIGEgaGFjay4gVGhlcmUgYXJlIG5vIGhvb2tzIGluIEZsb3QgYmV0d2VlblxuICAgICAgICAgICAgLy8gdGhlIGNyZWF0aW9uIGFuZCBtZWFzdXJpbmcgb2YgdGhlIHRpY2tzIChzZXRUaWNrcywgbWVhc3VyZVRpY2tMYWJlbHNcbiAgICAgICAgICAgIC8vIGluIHNldHVwR3JpZCgpICkgYW5kIHRoZSBkcmF3aW5nIG9mIHRoZSB0aWNrcyBhbmQgcGxvdCBib3hcbiAgICAgICAgICAgIC8vIChpbnNlcnRBeGlzTGFiZWxzIGluIHNldHVwR3JpZCgpICkuXG4gICAgICAgICAgICAvL1xuICAgICAgICAgICAgLy8gVGhlcmVmb3JlLCB3ZSB1c2UgYSB0cmljayB3aGVyZSB3ZSBydW4gdGhlIGRyYXcgcm91dGluZSB0d2ljZTpcbiAgICAgICAgICAgIC8vIHRoZSBmaXJzdCB0aW1lIHRvIGdldCB0aGUgdGljayBtZWFzdXJlbWVudHMsIHNvIHRoYXQgd2UgY2FuIGNoYW5nZVxuICAgICAgICAgICAgLy8gdGhlbSwgYW5kIHRoZW4gaGF2ZSBpdCBkcmF3IGl0IGFnYWluLlxuICAgICAgICAgICAgdmFyIHNlY29uZFBhc3MgPSBmYWxzZTtcblxuICAgICAgICAgICAgdmFyIGF4aXNMYWJlbHMgPSB7fTtcbiAgICAgICAgICAgIHZhciBheGlzT2Zmc2V0Q291bnRzID0geyBsZWZ0OiAwLCByaWdodDogMCwgdG9wOiAwLCBib3R0b206IDAgfTtcblxuICAgICAgICAgICAgdmFyIGRlZmF1bHRQYWRkaW5nID0gMjsgIC8vIHBhZGRpbmcgYmV0d2VlbiBheGlzIGFuZCB0aWNrIGxhYmVsc1xuICAgICAgICAgICAgcGxvdC5ob29rcy5kcmF3LnB1c2goZnVuY3Rpb24gKHBsb3QsIGN0eCkge1xuICAgICAgICAgICAgICAgIHZhciBoYXNBeGlzTGFiZWxzID0gZmFsc2U7XG4gICAgICAgICAgICAgICAgaWYgKCFzZWNvbmRQYXNzKSB7XG4gICAgICAgICAgICAgICAgICAgIC8vIE1FQVNVUkUgQU5EIFNFVCBPUFRJT05TXG4gICAgICAgICAgICAgICAgICAgICQuZWFjaChwbG90LmdldEF4ZXMoKSwgZnVuY3Rpb24oYXhpc05hbWUsIGF4aXMpIHtcbiAgICAgICAgICAgICAgICAgICAgICAgIHZhciBvcHRzID0gYXhpcy5vcHRpb25zIC8vIEZsb3QgMC43XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgfHwgcGxvdC5nZXRPcHRpb25zKClbYXhpc05hbWVdOyAvLyBGbG90IDAuNlxuXG4gICAgICAgICAgICAgICAgICAgICAgICAvLyBIYW5kbGUgcmVkcmF3cyBpbml0aWF0ZWQgb3V0c2lkZSBvZiB0aGlzIHBsdWctaW4uXG4gICAgICAgICAgICAgICAgICAgICAgICBpZiAoYXhpc05hbWUgaW4gYXhpc0xhYmVscykge1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGF4aXMubGFiZWxIZWlnaHQgPSBheGlzLmxhYmVsSGVpZ2h0IC1cbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgYXhpc0xhYmVsc1theGlzTmFtZV0uaGVpZ2h0O1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGF4aXMubGFiZWxXaWR0aCA9IGF4aXMubGFiZWxXaWR0aCAtXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGF4aXNMYWJlbHNbYXhpc05hbWVdLndpZHRoO1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIG9wdHMubGFiZWxIZWlnaHQgPSBheGlzLmxhYmVsSGVpZ2h0O1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIG9wdHMubGFiZWxXaWR0aCA9IGF4aXMubGFiZWxXaWR0aDtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBheGlzTGFiZWxzW2F4aXNOYW1lXS5jbGVhbnVwKCk7XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgZGVsZXRlIGF4aXNMYWJlbHNbYXhpc05hbWVdO1xuICAgICAgICAgICAgICAgICAgICAgICAgfVxuXG4gICAgICAgICAgICAgICAgICAgICAgICBpZiAoIW9wdHMgfHwgIW9wdHMuYXhpc0xhYmVsIHx8ICFheGlzLnNob3cpXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgcmV0dXJuO1xuXG4gICAgICAgICAgICAgICAgICAgICAgICBoYXNBeGlzTGFiZWxzID0gdHJ1ZTtcbiAgICAgICAgICAgICAgICAgICAgICAgIHZhciByZW5kZXJlciA9IG51bGw7XG5cbiAgICAgICAgICAgICAgICAgICAgICAgIGlmICghb3B0cy5heGlzTGFiZWxVc2VIdG1sICYmXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgbmF2aWdhdG9yLmFwcE5hbWUgPT0gJ01pY3Jvc29mdCBJbnRlcm5ldCBFeHBsb3JlcicpIHtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB2YXIgdWEgPSBuYXZpZ2F0b3IudXNlckFnZW50O1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHZhciByZSAgPSBuZXcgUmVnRXhwKFwiTVNJRSAoWzAtOV17MSx9W1xcLjAtOV17MCx9KVwiKTtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICBpZiAocmUuZXhlYyh1YSkgIT0gbnVsbCkge1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBydiA9IHBhcnNlRmxvYXQoUmVnRXhwLiQxKTtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB9XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgaWYgKHJ2ID49IDkgJiYgIW9wdHMuYXhpc0xhYmVsVXNlQ2FudmFzICYmICFvcHRzLmF4aXNMYWJlbFVzZUh0bWwpIHtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcmVuZGVyZXIgPSBDc3NUcmFuc2Zvcm1BeGlzTGFiZWw7XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgfSBlbHNlIGlmICghb3B0cy5heGlzTGFiZWxVc2VDYW52YXMgJiYgIW9wdHMuYXhpc0xhYmVsVXNlSHRtbCkge1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICByZW5kZXJlciA9IEllVHJhbnNmb3JtQXhpc0xhYmVsO1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIH0gZWxzZSBpZiAob3B0cy5heGlzTGFiZWxVc2VDYW52YXMpIHtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcmVuZGVyZXIgPSBDYW52YXNBeGlzTGFiZWw7XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgfSBlbHNlIHtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgcmVuZGVyZXIgPSBIdG1sQXhpc0xhYmVsO1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIH1cbiAgICAgICAgICAgICAgICAgICAgICAgIH0gZWxzZSB7XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgaWYgKG9wdHMuYXhpc0xhYmVsVXNlSHRtbCB8fCAoIWNzczNUcmFuc2l0aW9uU3VwcG9ydGVkKCkgJiYgIWNhbnZhc1RleHRTdXBwb3J0ZWQoKSkgJiYgIW9wdHMuYXhpc0xhYmVsVXNlQ2FudmFzKSB7XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHJlbmRlcmVyID0gSHRtbEF4aXNMYWJlbDtcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICB9IGVsc2UgaWYgKG9wdHMuYXhpc0xhYmVsVXNlQ2FudmFzIHx8ICFjc3MzVHJhbnNpdGlvblN1cHBvcnRlZCgpKSB7XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHJlbmRlcmVyID0gQ2FudmFzQXhpc0xhYmVsO1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIH0gZWxzZSB7XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHJlbmRlcmVyID0gQ3NzVHJhbnNmb3JtQXhpc0xhYmVsO1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIH1cbiAgICAgICAgICAgICAgICAgICAgICAgIH1cblxuICAgICAgICAgICAgICAgICAgICAgICAgdmFyIHBhZGRpbmcgPSBvcHRzLmF4aXNMYWJlbFBhZGRpbmcgPT09IHVuZGVmaW5lZCA/XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGRlZmF1bHRQYWRkaW5nIDogb3B0cy5heGlzTGFiZWxQYWRkaW5nO1xuXG4gICAgICAgICAgICAgICAgICAgICAgICBheGlzTGFiZWxzW2F4aXNOYW1lXSA9IG5ldyByZW5kZXJlcihheGlzTmFtZSxcbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIGF4aXMucG9zaXRpb24sIHBhZGRpbmcsXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBwbG90LCBvcHRzKTtcblxuICAgICAgICAgICAgICAgICAgICAgICAgLy8gZmxvdCBpbnRlcnByZXRzIGF4aXMubGFiZWxIZWlnaHQgYW5kIC5sYWJlbFdpZHRoIGFzXG4gICAgICAgICAgICAgICAgICAgICAgICAvLyB0aGUgaGVpZ2h0IGFuZCB3aWR0aCBvZiB0aGUgdGljayBsYWJlbHMuIFdlIGluY3JlYXNlXG4gICAgICAgICAgICAgICAgICAgICAgICAvLyB0aGVzZSB2YWx1ZXMgdG8gbWFrZSByb29tIGZvciB0aGUgYXhpcyBsYWJlbCBhbmRcbiAgICAgICAgICAgICAgICAgICAgICAgIC8vIHBhZGRpbmcuXG5cbiAgICAgICAgICAgICAgICAgICAgICAgIGF4aXNMYWJlbHNbYXhpc05hbWVdLmNhbGN1bGF0ZVNpemUoKTtcblxuICAgICAgICAgICAgICAgICAgICAgICAgLy8gQXhpc0xhYmVsLmhlaWdodCBhbmQgLndpZHRoIGFyZSB0aGUgc2l6ZSBvZiB0aGVcbiAgICAgICAgICAgICAgICAgICAgICAgIC8vIGF4aXMgbGFiZWwgYW5kIHBhZGRpbmcuXG4gICAgICAgICAgICAgICAgICAgICAgICAvLyBKdXN0IHNldCBvcHRzIGhlcmUgYmVjYXVzZSBheGlzIHdpbGwgYmUgc29ydGVkIG91dCBvblxuICAgICAgICAgICAgICAgICAgICAgICAgLy8gdGhlIHJlZHJhdy5cblxuICAgICAgICAgICAgICAgICAgICAgICAgb3B0cy5sYWJlbEhlaWdodCA9IGF4aXMubGFiZWxIZWlnaHQgK1xuICAgICAgICAgICAgICAgICAgICAgICAgICAgIGF4aXNMYWJlbHNbYXhpc05hbWVdLmhlaWdodDtcbiAgICAgICAgICAgICAgICAgICAgICAgIG9wdHMubGFiZWxXaWR0aCA9IGF4aXMubGFiZWxXaWR0aCArXG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgYXhpc0xhYmVsc1theGlzTmFtZV0ud2lkdGg7XG4gICAgICAgICAgICAgICAgICAgIH0pO1xuXG4gICAgICAgICAgICAgICAgICAgIC8vIElmIHRoZXJlIGFyZSBheGlzIGxhYmVscywgcmUtZHJhdyB3aXRoIG5ldyBsYWJlbCB3aWR0aHMgYW5kXG4gICAgICAgICAgICAgICAgICAgIC8vIGhlaWdodHMuXG5cbiAgICAgICAgICAgICAgICAgICAgaWYgKGhhc0F4aXNMYWJlbHMpIHtcbiAgICAgICAgICAgICAgICAgICAgICAgIHNlY29uZFBhc3MgPSB0cnVlO1xuICAgICAgICAgICAgICAgICAgICAgICAgcGxvdC5zZXR1cEdyaWQoKTtcbiAgICAgICAgICAgICAgICAgICAgICAgIHBsb3QuZHJhdygpO1xuICAgICAgICAgICAgICAgICAgICB9XG4gICAgICAgICAgICAgICAgfSBlbHNlIHtcbiAgICAgICAgICAgICAgICAgICAgc2Vjb25kUGFzcyA9IGZhbHNlO1xuICAgICAgICAgICAgICAgICAgICAvLyBEUkFXXG4gICAgICAgICAgICAgICAgICAgICQuZWFjaChwbG90LmdldEF4ZXMoKSwgZnVuY3Rpb24oYXhpc05hbWUsIGF4aXMpIHtcbiAgICAgICAgICAgICAgICAgICAgICAgIHZhciBvcHRzID0gYXhpcy5vcHRpb25zIC8vIEZsb3QgMC43XG4gICAgICAgICAgICAgICAgICAgICAgICAgICAgfHwgcGxvdC5nZXRPcHRpb25zKClbYXhpc05hbWVdOyAvLyBGbG90IDAuNlxuICAgICAgICAgICAgICAgICAgICAgICAgaWYgKCFvcHRzIHx8ICFvcHRzLmF4aXNMYWJlbCB8fCAhYXhpcy5zaG93KVxuICAgICAgICAgICAgICAgICAgICAgICAgICAgIHJldHVybjtcblxuICAgICAgICAgICAgICAgICAgICAgICAgYXhpc0xhYmVsc1theGlzTmFtZV0uZHJhdyhheGlzLmJveCk7XG4gICAgICAgICAgICAgICAgICAgIH0pO1xuICAgICAgICAgICAgICAgIH1cbiAgICAgICAgICAgIH0pO1xuICAgICAgICB9KTtcbiAgICB9XG5cblxuICAgICQucGxvdC5wbHVnaW5zLnB1c2goe1xuICAgICAgICBpbml0OiBpbml0LFxuICAgICAgICBvcHRpb25zOiBvcHRpb25zLFxuICAgICAgICBuYW1lOiAnYXhpc0xhYmVscycsXG4gICAgICAgIHZlcnNpb246ICcyLjAnXG4gICAgfSk7XG59KShqUXVlcnkpOyIsIi8qIEphdmFzY3JpcHQgcGxvdHRpbmcgbGlicmFyeSBmb3IgalF1ZXJ5LCB2ZXJzaW9uIDAuOC4zLlxuXG5Db3B5cmlnaHQgKGMpIDIwMDctMjAxNCBJT0xBIGFuZCBPbGUgTGF1cnNlbi5cbkxpY2Vuc2VkIHVuZGVyIHRoZSBNSVQgbGljZW5zZS5cblxuKi9cbihmdW5jdGlvbigkKXt2YXIgb3B0aW9ucz17Y3Jvc3NoYWlyOnttb2RlOm51bGwsY29sb3I6XCJyZ2JhKDE3MCwgMCwgMCwgMC44MClcIixsaW5lV2lkdGg6MX19O2Z1bmN0aW9uIGluaXQocGxvdCl7dmFyIGNyb3NzaGFpcj17eDotMSx5Oi0xLGxvY2tlZDpmYWxzZX07cGxvdC5zZXRDcm9zc2hhaXI9ZnVuY3Rpb24gc2V0Q3Jvc3NoYWlyKHBvcyl7aWYoIXBvcyljcm9zc2hhaXIueD0tMTtlbHNle3ZhciBvPXBsb3QucDJjKHBvcyk7Y3Jvc3NoYWlyLng9TWF0aC5tYXgoMCxNYXRoLm1pbihvLmxlZnQscGxvdC53aWR0aCgpKSk7Y3Jvc3NoYWlyLnk9TWF0aC5tYXgoMCxNYXRoLm1pbihvLnRvcCxwbG90LmhlaWdodCgpKSl9cGxvdC50cmlnZ2VyUmVkcmF3T3ZlcmxheSgpfTtwbG90LmNsZWFyQ3Jvc3NoYWlyPXBsb3Quc2V0Q3Jvc3NoYWlyO3Bsb3QubG9ja0Nyb3NzaGFpcj1mdW5jdGlvbiBsb2NrQ3Jvc3NoYWlyKHBvcyl7aWYocG9zKXBsb3Quc2V0Q3Jvc3NoYWlyKHBvcyk7Y3Jvc3NoYWlyLmxvY2tlZD10cnVlfTtwbG90LnVubG9ja0Nyb3NzaGFpcj1mdW5jdGlvbiB1bmxvY2tDcm9zc2hhaXIoKXtjcm9zc2hhaXIubG9ja2VkPWZhbHNlfTtmdW5jdGlvbiBvbk1vdXNlT3V0KGUpe2lmKGNyb3NzaGFpci5sb2NrZWQpcmV0dXJuO2lmKGNyb3NzaGFpci54IT0tMSl7Y3Jvc3NoYWlyLng9LTE7cGxvdC50cmlnZ2VyUmVkcmF3T3ZlcmxheSgpfX1mdW5jdGlvbiBvbk1vdXNlTW92ZShlKXtpZihjcm9zc2hhaXIubG9ja2VkKXJldHVybjtpZihwbG90LmdldFNlbGVjdGlvbiYmcGxvdC5nZXRTZWxlY3Rpb24oKSl7Y3Jvc3NoYWlyLng9LTE7cmV0dXJufXZhciBvZmZzZXQ9cGxvdC5vZmZzZXQoKTtjcm9zc2hhaXIueD1NYXRoLm1heCgwLE1hdGgubWluKGUucGFnZVgtb2Zmc2V0LmxlZnQscGxvdC53aWR0aCgpKSk7Y3Jvc3NoYWlyLnk9TWF0aC5tYXgoMCxNYXRoLm1pbihlLnBhZ2VZLW9mZnNldC50b3AscGxvdC5oZWlnaHQoKSkpO3Bsb3QudHJpZ2dlclJlZHJhd092ZXJsYXkoKX1wbG90Lmhvb2tzLmJpbmRFdmVudHMucHVzaChmdW5jdGlvbihwbG90LGV2ZW50SG9sZGVyKXtpZighcGxvdC5nZXRPcHRpb25zKCkuY3Jvc3NoYWlyLm1vZGUpcmV0dXJuO2V2ZW50SG9sZGVyLm1vdXNlb3V0KG9uTW91c2VPdXQpO2V2ZW50SG9sZGVyLm1vdXNlbW92ZShvbk1vdXNlTW92ZSl9KTtwbG90Lmhvb2tzLmRyYXdPdmVybGF5LnB1c2goZnVuY3Rpb24ocGxvdCxjdHgpe3ZhciBjPXBsb3QuZ2V0T3B0aW9ucygpLmNyb3NzaGFpcjtpZighYy5tb2RlKXJldHVybjt2YXIgcGxvdE9mZnNldD1wbG90LmdldFBsb3RPZmZzZXQoKTtjdHguc2F2ZSgpO2N0eC50cmFuc2xhdGUocGxvdE9mZnNldC5sZWZ0LHBsb3RPZmZzZXQudG9wKTtpZihjcm9zc2hhaXIueCE9LTEpe3ZhciBhZGo9cGxvdC5nZXRPcHRpb25zKCkuY3Jvc3NoYWlyLmxpbmVXaWR0aCUyPy41OjA7Y3R4LnN0cm9rZVN0eWxlPWMuY29sb3I7Y3R4LmxpbmVXaWR0aD1jLmxpbmVXaWR0aDtjdHgubGluZUpvaW49XCJyb3VuZFwiO2N0eC5iZWdpblBhdGgoKTtpZihjLm1vZGUuaW5kZXhPZihcInhcIikhPS0xKXt2YXIgZHJhd1g9TWF0aC5mbG9vcihjcm9zc2hhaXIueCkrYWRqO2N0eC5tb3ZlVG8oZHJhd1gsMCk7Y3R4LmxpbmVUbyhkcmF3WCxwbG90LmhlaWdodCgpKX1pZihjLm1vZGUuaW5kZXhPZihcInlcIikhPS0xKXt2YXIgZHJhd1k9TWF0aC5mbG9vcihjcm9zc2hhaXIueSkrYWRqO2N0eC5tb3ZlVG8oMCxkcmF3WSk7Y3R4LmxpbmVUbyhwbG90LndpZHRoKCksZHJhd1kpfWN0eC5zdHJva2UoKX1jdHgucmVzdG9yZSgpfSk7cGxvdC5ob29rcy5zaHV0ZG93bi5wdXNoKGZ1bmN0aW9uKHBsb3QsZXZlbnRIb2xkZXIpe2V2ZW50SG9sZGVyLnVuYmluZChcIm1vdXNlb3V0XCIsb25Nb3VzZU91dCk7ZXZlbnRIb2xkZXIudW5iaW5kKFwibW91c2Vtb3ZlXCIsb25Nb3VzZU1vdmUpfSl9JC5wbG90LnBsdWdpbnMucHVzaCh7aW5pdDppbml0LG9wdGlvbnM6b3B0aW9ucyxuYW1lOlwiY3Jvc3NoYWlyXCIsdmVyc2lvbjpcIjEuMFwifSl9KShqUXVlcnkpOyIsIi8qIEphdmFzY3JpcHQgcGxvdHRpbmcgbGlicmFyeSBmb3IgalF1ZXJ5LCB2ZXJzaW9uIDAuOC4zLlxuXG5Db3B5cmlnaHQgKGMpIDIwMDctMjAxNCBJT0xBIGFuZCBPbGUgTGF1cnNlbi5cbkxpY2Vuc2VkIHVuZGVyIHRoZSBNSVQgbGljZW5zZS5cblxuKi9cbihmdW5jdGlvbigkKXskLmNvbG9yPXt9OyQuY29sb3IubWFrZT1mdW5jdGlvbihyLGcsYixhKXt2YXIgbz17fTtvLnI9cnx8MDtvLmc9Z3x8MDtvLmI9Ynx8MDtvLmE9YSE9bnVsbD9hOjE7by5hZGQ9ZnVuY3Rpb24oYyxkKXtmb3IodmFyIGk9MDtpPGMubGVuZ3RoOysraSlvW2MuY2hhckF0KGkpXSs9ZDtyZXR1cm4gby5ub3JtYWxpemUoKX07by5zY2FsZT1mdW5jdGlvbihjLGYpe2Zvcih2YXIgaT0wO2k8Yy5sZW5ndGg7KytpKW9bYy5jaGFyQXQoaSldKj1mO3JldHVybiBvLm5vcm1hbGl6ZSgpfTtvLnRvU3RyaW5nPWZ1bmN0aW9uKCl7aWYoby5hPj0xKXtyZXR1cm5cInJnYihcIitbby5yLG8uZyxvLmJdLmpvaW4oXCIsXCIpK1wiKVwifWVsc2V7cmV0dXJuXCJyZ2JhKFwiK1tvLnIsby5nLG8uYixvLmFdLmpvaW4oXCIsXCIpK1wiKVwifX07by5ub3JtYWxpemU9ZnVuY3Rpb24oKXtmdW5jdGlvbiBjbGFtcChtaW4sdmFsdWUsbWF4KXtyZXR1cm4gdmFsdWU8bWluP21pbjp2YWx1ZT5tYXg/bWF4OnZhbHVlfW8ucj1jbGFtcCgwLHBhcnNlSW50KG8uciksMjU1KTtvLmc9Y2xhbXAoMCxwYXJzZUludChvLmcpLDI1NSk7by5iPWNsYW1wKDAscGFyc2VJbnQoby5iKSwyNTUpO28uYT1jbGFtcCgwLG8uYSwxKTtyZXR1cm4gb307by5jbG9uZT1mdW5jdGlvbigpe3JldHVybiAkLmNvbG9yLm1ha2Uoby5yLG8uYixvLmcsby5hKX07cmV0dXJuIG8ubm9ybWFsaXplKCl9OyQuY29sb3IuZXh0cmFjdD1mdW5jdGlvbihlbGVtLGNzcyl7dmFyIGM7ZG97Yz1lbGVtLmNzcyhjc3MpLnRvTG93ZXJDYXNlKCk7aWYoYyE9XCJcIiYmYyE9XCJ0cmFuc3BhcmVudFwiKWJyZWFrO2VsZW09ZWxlbS5wYXJlbnQoKX13aGlsZShlbGVtLmxlbmd0aCYmISQubm9kZU5hbWUoZWxlbS5nZXQoMCksXCJib2R5XCIpKTtpZihjPT1cInJnYmEoMCwgMCwgMCwgMClcIiljPVwidHJhbnNwYXJlbnRcIjtyZXR1cm4gJC5jb2xvci5wYXJzZShjKX07JC5jb2xvci5wYXJzZT1mdW5jdGlvbihzdHIpe3ZhciByZXMsbT0kLmNvbG9yLm1ha2U7aWYocmVzPS9yZ2JcXChcXHMqKFswLTldezEsM30pXFxzKixcXHMqKFswLTldezEsM30pXFxzKixcXHMqKFswLTldezEsM30pXFxzKlxcKS8uZXhlYyhzdHIpKXJldHVybiBtKHBhcnNlSW50KHJlc1sxXSwxMCkscGFyc2VJbnQocmVzWzJdLDEwKSxwYXJzZUludChyZXNbM10sMTApKTtpZihyZXM9L3JnYmFcXChcXHMqKFswLTldezEsM30pXFxzKixcXHMqKFswLTldezEsM30pXFxzKixcXHMqKFswLTldezEsM30pXFxzKixcXHMqKFswLTldKyg/OlxcLlswLTldKyk/KVxccypcXCkvLmV4ZWMoc3RyKSlyZXR1cm4gbShwYXJzZUludChyZXNbMV0sMTApLHBhcnNlSW50KHJlc1syXSwxMCkscGFyc2VJbnQocmVzWzNdLDEwKSxwYXJzZUZsb2F0KHJlc1s0XSkpO2lmKHJlcz0vcmdiXFwoXFxzKihbMC05XSsoPzpcXC5bMC05XSspPylcXCVcXHMqLFxccyooWzAtOV0rKD86XFwuWzAtOV0rKT8pXFwlXFxzKixcXHMqKFswLTldKyg/OlxcLlswLTldKyk/KVxcJVxccypcXCkvLmV4ZWMoc3RyKSlyZXR1cm4gbShwYXJzZUZsb2F0KHJlc1sxXSkqMi41NSxwYXJzZUZsb2F0KHJlc1syXSkqMi41NSxwYXJzZUZsb2F0KHJlc1szXSkqMi41NSk7aWYocmVzPS9yZ2JhXFwoXFxzKihbMC05XSsoPzpcXC5bMC05XSspPylcXCVcXHMqLFxccyooWzAtOV0rKD86XFwuWzAtOV0rKT8pXFwlXFxzKixcXHMqKFswLTldKyg/OlxcLlswLTldKyk/KVxcJVxccyosXFxzKihbMC05XSsoPzpcXC5bMC05XSspPylcXHMqXFwpLy5leGVjKHN0cikpcmV0dXJuIG0ocGFyc2VGbG9hdChyZXNbMV0pKjIuNTUscGFyc2VGbG9hdChyZXNbMl0pKjIuNTUscGFyc2VGbG9hdChyZXNbM10pKjIuNTUscGFyc2VGbG9hdChyZXNbNF0pKTtpZihyZXM9LyMoW2EtZkEtRjAtOV17Mn0pKFthLWZBLUYwLTldezJ9KShbYS1mQS1GMC05XXsyfSkvLmV4ZWMoc3RyKSlyZXR1cm4gbShwYXJzZUludChyZXNbMV0sMTYpLHBhcnNlSW50KHJlc1syXSwxNikscGFyc2VJbnQocmVzWzNdLDE2KSk7aWYocmVzPS8jKFthLWZBLUYwLTldKShbYS1mQS1GMC05XSkoW2EtZkEtRjAtOV0pLy5leGVjKHN0cikpcmV0dXJuIG0ocGFyc2VJbnQocmVzWzFdK3Jlc1sxXSwxNikscGFyc2VJbnQocmVzWzJdK3Jlc1syXSwxNikscGFyc2VJbnQocmVzWzNdK3Jlc1szXSwxNikpO3ZhciBuYW1lPSQudHJpbShzdHIpLnRvTG93ZXJDYXNlKCk7aWYobmFtZT09XCJ0cmFuc3BhcmVudFwiKXJldHVybiBtKDI1NSwyNTUsMjU1LDApO2Vsc2V7cmVzPWxvb2t1cENvbG9yc1tuYW1lXXx8WzAsMCwwXTtyZXR1cm4gbShyZXNbMF0scmVzWzFdLHJlc1syXSl9fTt2YXIgbG9va3VwQ29sb3JzPXthcXVhOlswLDI1NSwyNTVdLGF6dXJlOlsyNDAsMjU1LDI1NV0sYmVpZ2U6WzI0NSwyNDUsMjIwXSxibGFjazpbMCwwLDBdLGJsdWU6WzAsMCwyNTVdLGJyb3duOlsxNjUsNDIsNDJdLGN5YW46WzAsMjU1LDI1NV0sZGFya2JsdWU6WzAsMCwxMzldLGRhcmtjeWFuOlswLDEzOSwxMzldLGRhcmtncmV5OlsxNjksMTY5LDE2OV0sZGFya2dyZWVuOlswLDEwMCwwXSxkYXJra2hha2k6WzE4OSwxODMsMTA3XSxkYXJrbWFnZW50YTpbMTM5LDAsMTM5XSxkYXJrb2xpdmVncmVlbjpbODUsMTA3LDQ3XSxkYXJrb3JhbmdlOlsyNTUsMTQwLDBdLGRhcmtvcmNoaWQ6WzE1Myw1MCwyMDRdLGRhcmtyZWQ6WzEzOSwwLDBdLGRhcmtzYWxtb246WzIzMywxNTAsMTIyXSxkYXJrdmlvbGV0OlsxNDgsMCwyMTFdLGZ1Y2hzaWE6WzI1NSwwLDI1NV0sZ29sZDpbMjU1LDIxNSwwXSxncmVlbjpbMCwxMjgsMF0saW5kaWdvOls3NSwwLDEzMF0sa2hha2k6WzI0MCwyMzAsMTQwXSxsaWdodGJsdWU6WzE3MywyMTYsMjMwXSxsaWdodGN5YW46WzIyNCwyNTUsMjU1XSxsaWdodGdyZWVuOlsxNDQsMjM4LDE0NF0sbGlnaHRncmV5OlsyMTEsMjExLDIxMV0sbGlnaHRwaW5rOlsyNTUsMTgyLDE5M10sbGlnaHR5ZWxsb3c6WzI1NSwyNTUsMjI0XSxsaW1lOlswLDI1NSwwXSxtYWdlbnRhOlsyNTUsMCwyNTVdLG1hcm9vbjpbMTI4LDAsMF0sbmF2eTpbMCwwLDEyOF0sb2xpdmU6WzEyOCwxMjgsMF0sb3JhbmdlOlsyNTUsMTY1LDBdLHBpbms6WzI1NSwxOTIsMjAzXSxwdXJwbGU6WzEyOCwwLDEyOF0sdmlvbGV0OlsxMjgsMCwxMjhdLHJlZDpbMjU1LDAsMF0sc2lsdmVyOlsxOTIsMTkyLDE5Ml0sd2hpdGU6WzI1NSwyNTUsMjU1XSx5ZWxsb3c6WzI1NSwyNTUsMF19fSkoalF1ZXJ5KTsoZnVuY3Rpb24oJCl7dmFyIGhhc093blByb3BlcnR5PU9iamVjdC5wcm90b3R5cGUuaGFzT3duUHJvcGVydHk7aWYoISQuZm4uZGV0YWNoKXskLmZuLmRldGFjaD1mdW5jdGlvbigpe3JldHVybiB0aGlzLmVhY2goZnVuY3Rpb24oKXtpZih0aGlzLnBhcmVudE5vZGUpe3RoaXMucGFyZW50Tm9kZS5yZW1vdmVDaGlsZCh0aGlzKX19KX19ZnVuY3Rpb24gQ2FudmFzKGNscyxjb250YWluZXIpe3ZhciBlbGVtZW50PWNvbnRhaW5lci5jaGlsZHJlbihcIi5cIitjbHMpWzBdO2lmKGVsZW1lbnQ9PW51bGwpe2VsZW1lbnQ9ZG9jdW1lbnQuY3JlYXRlRWxlbWVudChcImNhbnZhc1wiKTtlbGVtZW50LmNsYXNzTmFtZT1jbHM7JChlbGVtZW50KS5jc3Moe2RpcmVjdGlvbjpcImx0clwiLHBvc2l0aW9uOlwiYWJzb2x1dGVcIixsZWZ0OjAsdG9wOjB9KS5hcHBlbmRUbyhjb250YWluZXIpO2lmKCFlbGVtZW50LmdldENvbnRleHQpe2lmKHdpbmRvdy5HX3ZtbENhbnZhc01hbmFnZXIpe2VsZW1lbnQ9d2luZG93Lkdfdm1sQ2FudmFzTWFuYWdlci5pbml0RWxlbWVudChlbGVtZW50KX1lbHNle3Rocm93IG5ldyBFcnJvcihcIkNhbnZhcyBpcyBub3QgYXZhaWxhYmxlLiBJZiB5b3UncmUgdXNpbmcgSUUgd2l0aCBhIGZhbGwtYmFjayBzdWNoIGFzIEV4Y2FudmFzLCB0aGVuIHRoZXJlJ3MgZWl0aGVyIGEgbWlzdGFrZSBpbiB5b3VyIGNvbmRpdGlvbmFsIGluY2x1ZGUsIG9yIHRoZSBwYWdlIGhhcyBubyBET0NUWVBFIGFuZCBpcyByZW5kZXJpbmcgaW4gUXVpcmtzIE1vZGUuXCIpfX19dGhpcy5lbGVtZW50PWVsZW1lbnQ7dmFyIGNvbnRleHQ9dGhpcy5jb250ZXh0PWVsZW1lbnQuZ2V0Q29udGV4dChcIjJkXCIpO3ZhciBkZXZpY2VQaXhlbFJhdGlvPXdpbmRvdy5kZXZpY2VQaXhlbFJhdGlvfHwxLGJhY2tpbmdTdG9yZVJhdGlvPWNvbnRleHQud2Via2l0QmFja2luZ1N0b3JlUGl4ZWxSYXRpb3x8Y29udGV4dC5tb3pCYWNraW5nU3RvcmVQaXhlbFJhdGlvfHxjb250ZXh0Lm1zQmFja2luZ1N0b3JlUGl4ZWxSYXRpb3x8Y29udGV4dC5vQmFja2luZ1N0b3JlUGl4ZWxSYXRpb3x8Y29udGV4dC5iYWNraW5nU3RvcmVQaXhlbFJhdGlvfHwxO3RoaXMucGl4ZWxSYXRpbz1kZXZpY2VQaXhlbFJhdGlvL2JhY2tpbmdTdG9yZVJhdGlvO3RoaXMucmVzaXplKGNvbnRhaW5lci53aWR0aCgpLGNvbnRhaW5lci5oZWlnaHQoKSk7dGhpcy50ZXh0Q29udGFpbmVyPW51bGw7dGhpcy50ZXh0PXt9O3RoaXMuX3RleHRDYWNoZT17fX1DYW52YXMucHJvdG90eXBlLnJlc2l6ZT1mdW5jdGlvbih3aWR0aCxoZWlnaHQpe2lmKHdpZHRoPD0wfHxoZWlnaHQ8PTApe3Rocm93IG5ldyBFcnJvcihcIkludmFsaWQgZGltZW5zaW9ucyBmb3IgcGxvdCwgd2lkdGggPSBcIit3aWR0aCtcIiwgaGVpZ2h0ID0gXCIraGVpZ2h0KX12YXIgZWxlbWVudD10aGlzLmVsZW1lbnQsY29udGV4dD10aGlzLmNvbnRleHQscGl4ZWxSYXRpbz10aGlzLnBpeGVsUmF0aW87aWYodGhpcy53aWR0aCE9d2lkdGgpe2VsZW1lbnQud2lkdGg9d2lkdGgqcGl4ZWxSYXRpbztlbGVtZW50LnN0eWxlLndpZHRoPXdpZHRoK1wicHhcIjt0aGlzLndpZHRoPXdpZHRofWlmKHRoaXMuaGVpZ2h0IT1oZWlnaHQpe2VsZW1lbnQuaGVpZ2h0PWhlaWdodCpwaXhlbFJhdGlvO2VsZW1lbnQuc3R5bGUuaGVpZ2h0PWhlaWdodCtcInB4XCI7dGhpcy5oZWlnaHQ9aGVpZ2h0fWNvbnRleHQucmVzdG9yZSgpO2NvbnRleHQuc2F2ZSgpO2NvbnRleHQuc2NhbGUocGl4ZWxSYXRpbyxwaXhlbFJhdGlvKX07Q2FudmFzLnByb3RvdHlwZS5jbGVhcj1mdW5jdGlvbigpe3RoaXMuY29udGV4dC5jbGVhclJlY3QoMCwwLHRoaXMud2lkdGgsdGhpcy5oZWlnaHQpfTtDYW52YXMucHJvdG90eXBlLnJlbmRlcj1mdW5jdGlvbigpe3ZhciBjYWNoZT10aGlzLl90ZXh0Q2FjaGU7Zm9yKHZhciBsYXllcktleSBpbiBjYWNoZSl7aWYoaGFzT3duUHJvcGVydHkuY2FsbChjYWNoZSxsYXllcktleSkpe3ZhciBsYXllcj10aGlzLmdldFRleHRMYXllcihsYXllcktleSksbGF5ZXJDYWNoZT1jYWNoZVtsYXllcktleV07bGF5ZXIuaGlkZSgpO2Zvcih2YXIgc3R5bGVLZXkgaW4gbGF5ZXJDYWNoZSl7aWYoaGFzT3duUHJvcGVydHkuY2FsbChsYXllckNhY2hlLHN0eWxlS2V5KSl7dmFyIHN0eWxlQ2FjaGU9bGF5ZXJDYWNoZVtzdHlsZUtleV07Zm9yKHZhciBrZXkgaW4gc3R5bGVDYWNoZSl7aWYoaGFzT3duUHJvcGVydHkuY2FsbChzdHlsZUNhY2hlLGtleSkpe3ZhciBwb3NpdGlvbnM9c3R5bGVDYWNoZVtrZXldLnBvc2l0aW9ucztmb3IodmFyIGk9MCxwb3NpdGlvbjtwb3NpdGlvbj1wb3NpdGlvbnNbaV07aSsrKXtpZihwb3NpdGlvbi5hY3RpdmUpe2lmKCFwb3NpdGlvbi5yZW5kZXJlZCl7bGF5ZXIuYXBwZW5kKHBvc2l0aW9uLmVsZW1lbnQpO3Bvc2l0aW9uLnJlbmRlcmVkPXRydWV9fWVsc2V7cG9zaXRpb25zLnNwbGljZShpLS0sMSk7aWYocG9zaXRpb24ucmVuZGVyZWQpe3Bvc2l0aW9uLmVsZW1lbnQuZGV0YWNoKCl9fX1pZihwb3NpdGlvbnMubGVuZ3RoPT0wKXtkZWxldGUgc3R5bGVDYWNoZVtrZXldfX19fX1sYXllci5zaG93KCl9fX07Q2FudmFzLnByb3RvdHlwZS5nZXRUZXh0TGF5ZXI9ZnVuY3Rpb24oY2xhc3Nlcyl7dmFyIGxheWVyPXRoaXMudGV4dFtjbGFzc2VzXTtpZihsYXllcj09bnVsbCl7aWYodGhpcy50ZXh0Q29udGFpbmVyPT1udWxsKXt0aGlzLnRleHRDb250YWluZXI9JChcIjxkaXYgY2xhc3M9J2Zsb3QtdGV4dCc+PC9kaXY+XCIpLmNzcyh7cG9zaXRpb246XCJhYnNvbHV0ZVwiLHRvcDowLGxlZnQ6MCxib3R0b206MCxyaWdodDowLFwiZm9udC1zaXplXCI6XCJzbWFsbGVyXCIsY29sb3I6XCIjNTQ1NDU0XCJ9KS5pbnNlcnRBZnRlcih0aGlzLmVsZW1lbnQpfWxheWVyPXRoaXMudGV4dFtjbGFzc2VzXT0kKFwiPGRpdj48L2Rpdj5cIikuYWRkQ2xhc3MoY2xhc3NlcykuY3NzKHtwb3NpdGlvbjpcImFic29sdXRlXCIsdG9wOjAsbGVmdDowLGJvdHRvbTowLHJpZ2h0OjB9KS5hcHBlbmRUbyh0aGlzLnRleHRDb250YWluZXIpfXJldHVybiBsYXllcn07Q2FudmFzLnByb3RvdHlwZS5nZXRUZXh0SW5mbz1mdW5jdGlvbihsYXllcix0ZXh0LGZvbnQsYW5nbGUsd2lkdGgpe3ZhciB0ZXh0U3R5bGUsbGF5ZXJDYWNoZSxzdHlsZUNhY2hlLGluZm87dGV4dD1cIlwiK3RleHQ7aWYodHlwZW9mIGZvbnQ9PT1cIm9iamVjdFwiKXt0ZXh0U3R5bGU9Zm9udC5zdHlsZStcIiBcIitmb250LnZhcmlhbnQrXCIgXCIrZm9udC53ZWlnaHQrXCIgXCIrZm9udC5zaXplK1wicHgvXCIrZm9udC5saW5lSGVpZ2h0K1wicHggXCIrZm9udC5mYW1pbHl9ZWxzZXt0ZXh0U3R5bGU9Zm9udH1sYXllckNhY2hlPXRoaXMuX3RleHRDYWNoZVtsYXllcl07aWYobGF5ZXJDYWNoZT09bnVsbCl7bGF5ZXJDYWNoZT10aGlzLl90ZXh0Q2FjaGVbbGF5ZXJdPXt9fXN0eWxlQ2FjaGU9bGF5ZXJDYWNoZVt0ZXh0U3R5bGVdO2lmKHN0eWxlQ2FjaGU9PW51bGwpe3N0eWxlQ2FjaGU9bGF5ZXJDYWNoZVt0ZXh0U3R5bGVdPXt9fWluZm89c3R5bGVDYWNoZVt0ZXh0XTtpZihpbmZvPT1udWxsKXt2YXIgZWxlbWVudD0kKFwiPGRpdj48L2Rpdj5cIikuaHRtbCh0ZXh0KS5jc3Moe3Bvc2l0aW9uOlwiYWJzb2x1dGVcIixcIm1heC13aWR0aFwiOndpZHRoLHRvcDotOTk5OX0pLmFwcGVuZFRvKHRoaXMuZ2V0VGV4dExheWVyKGxheWVyKSk7aWYodHlwZW9mIGZvbnQ9PT1cIm9iamVjdFwiKXtlbGVtZW50LmNzcyh7Zm9udDp0ZXh0U3R5bGUsY29sb3I6Zm9udC5jb2xvcn0pfWVsc2UgaWYodHlwZW9mIGZvbnQ9PT1cInN0cmluZ1wiKXtlbGVtZW50LmFkZENsYXNzKGZvbnQpfWluZm89c3R5bGVDYWNoZVt0ZXh0XT17d2lkdGg6ZWxlbWVudC5vdXRlcldpZHRoKHRydWUpLGhlaWdodDplbGVtZW50Lm91dGVySGVpZ2h0KHRydWUpLGVsZW1lbnQ6ZWxlbWVudCxwb3NpdGlvbnM6W119O2VsZW1lbnQuZGV0YWNoKCl9cmV0dXJuIGluZm99O0NhbnZhcy5wcm90b3R5cGUuYWRkVGV4dD1mdW5jdGlvbihsYXllcix4LHksdGV4dCxmb250LGFuZ2xlLHdpZHRoLGhhbGlnbix2YWxpZ24pe3ZhciBpbmZvPXRoaXMuZ2V0VGV4dEluZm8obGF5ZXIsdGV4dCxmb250LGFuZ2xlLHdpZHRoKSxwb3NpdGlvbnM9aW5mby5wb3NpdGlvbnM7aWYoaGFsaWduPT1cImNlbnRlclwiKXt4LT1pbmZvLndpZHRoLzJ9ZWxzZSBpZihoYWxpZ249PVwicmlnaHRcIil7eC09aW5mby53aWR0aH1pZih2YWxpZ249PVwibWlkZGxlXCIpe3ktPWluZm8uaGVpZ2h0LzJ9ZWxzZSBpZih2YWxpZ249PVwiYm90dG9tXCIpe3ktPWluZm8uaGVpZ2h0fWZvcih2YXIgaT0wLHBvc2l0aW9uO3Bvc2l0aW9uPXBvc2l0aW9uc1tpXTtpKyspe2lmKHBvc2l0aW9uLng9PXgmJnBvc2l0aW9uLnk9PXkpe3Bvc2l0aW9uLmFjdGl2ZT10cnVlO3JldHVybn19cG9zaXRpb249e2FjdGl2ZTp0cnVlLHJlbmRlcmVkOmZhbHNlLGVsZW1lbnQ6cG9zaXRpb25zLmxlbmd0aD9pbmZvLmVsZW1lbnQuY2xvbmUoKTppbmZvLmVsZW1lbnQseDp4LHk6eX07cG9zaXRpb25zLnB1c2gocG9zaXRpb24pO3Bvc2l0aW9uLmVsZW1lbnQuY3NzKHt0b3A6TWF0aC5yb3VuZCh5KSxsZWZ0Ok1hdGgucm91bmQoeCksXCJ0ZXh0LWFsaWduXCI6aGFsaWdufSl9O0NhbnZhcy5wcm90b3R5cGUucmVtb3ZlVGV4dD1mdW5jdGlvbihsYXllcix4LHksdGV4dCxmb250LGFuZ2xlKXtpZih0ZXh0PT1udWxsKXt2YXIgbGF5ZXJDYWNoZT10aGlzLl90ZXh0Q2FjaGVbbGF5ZXJdO2lmKGxheWVyQ2FjaGUhPW51bGwpe2Zvcih2YXIgc3R5bGVLZXkgaW4gbGF5ZXJDYWNoZSl7aWYoaGFzT3duUHJvcGVydHkuY2FsbChsYXllckNhY2hlLHN0eWxlS2V5KSl7dmFyIHN0eWxlQ2FjaGU9bGF5ZXJDYWNoZVtzdHlsZUtleV07Zm9yKHZhciBrZXkgaW4gc3R5bGVDYWNoZSl7aWYoaGFzT3duUHJvcGVydHkuY2FsbChzdHlsZUNhY2hlLGtleSkpe3ZhciBwb3NpdGlvbnM9c3R5bGVDYWNoZVtrZXldLnBvc2l0aW9ucztmb3IodmFyIGk9MCxwb3NpdGlvbjtwb3NpdGlvbj1wb3NpdGlvbnNbaV07aSsrKXtwb3NpdGlvbi5hY3RpdmU9ZmFsc2V9fX19fX19ZWxzZXt2YXIgcG9zaXRpb25zPXRoaXMuZ2V0VGV4dEluZm8obGF5ZXIsdGV4dCxmb250LGFuZ2xlKS5wb3NpdGlvbnM7Zm9yKHZhciBpPTAscG9zaXRpb247cG9zaXRpb249cG9zaXRpb25zW2ldO2krKyl7aWYocG9zaXRpb24ueD09eCYmcG9zaXRpb24ueT09eSl7cG9zaXRpb24uYWN0aXZlPWZhbHNlfX19fTtmdW5jdGlvbiBQbG90KHBsYWNlaG9sZGVyLGRhdGFfLG9wdGlvbnNfLHBsdWdpbnMpe3ZhciBzZXJpZXM9W10sb3B0aW9ucz17Y29sb3JzOltcIiNlZGMyNDBcIixcIiNhZmQ4ZjhcIixcIiNjYjRiNGJcIixcIiM0ZGE3NGRcIixcIiM5NDQwZWRcIl0sbGVnZW5kOntzaG93OnRydWUsbm9Db2x1bW5zOjEsbGFiZWxGb3JtYXR0ZXI6bnVsbCxsYWJlbEJveEJvcmRlckNvbG9yOlwiI2NjY1wiLGNvbnRhaW5lcjpudWxsLHBvc2l0aW9uOlwibmVcIixtYXJnaW46NSxiYWNrZ3JvdW5kQ29sb3I6bnVsbCxiYWNrZ3JvdW5kT3BhY2l0eTouODUsc29ydGVkOm51bGx9LHhheGlzOntzaG93Om51bGwscG9zaXRpb246XCJib3R0b21cIixtb2RlOm51bGwsZm9udDpudWxsLGNvbG9yOm51bGwsdGlja0NvbG9yOm51bGwsdHJhbnNmb3JtOm51bGwsaW52ZXJzZVRyYW5zZm9ybTpudWxsLG1pbjpudWxsLG1heDpudWxsLGF1dG9zY2FsZU1hcmdpbjpudWxsLHRpY2tzOm51bGwsdGlja0Zvcm1hdHRlcjpudWxsLGxhYmVsV2lkdGg6bnVsbCxsYWJlbEhlaWdodDpudWxsLHJlc2VydmVTcGFjZTpudWxsLHRpY2tMZW5ndGg6bnVsbCxhbGlnblRpY2tzV2l0aEF4aXM6bnVsbCx0aWNrRGVjaW1hbHM6bnVsbCx0aWNrU2l6ZTpudWxsLG1pblRpY2tTaXplOm51bGx9LHlheGlzOnthdXRvc2NhbGVNYXJnaW46LjAyLHBvc2l0aW9uOlwibGVmdFwifSx4YXhlczpbXSx5YXhlczpbXSxzZXJpZXM6e3BvaW50czp7c2hvdzpmYWxzZSxyYWRpdXM6MyxsaW5lV2lkdGg6MixmaWxsOnRydWUsZmlsbENvbG9yOlwiI2ZmZmZmZlwiLHN5bWJvbDpcImNpcmNsZVwifSxsaW5lczp7bGluZVdpZHRoOjIsZmlsbDpmYWxzZSxmaWxsQ29sb3I6bnVsbCxzdGVwczpmYWxzZX0sYmFyczp7c2hvdzpmYWxzZSxsaW5lV2lkdGg6MixiYXJXaWR0aDoxLGZpbGw6dHJ1ZSxmaWxsQ29sb3I6bnVsbCxhbGlnbjpcImxlZnRcIixob3Jpem9udGFsOmZhbHNlLHplcm86dHJ1ZX0sc2hhZG93U2l6ZTozLGhpZ2hsaWdodENvbG9yOm51bGx9LGdyaWQ6e3Nob3c6dHJ1ZSxhYm92ZURhdGE6ZmFsc2UsY29sb3I6XCIjNTQ1NDU0XCIsYmFja2dyb3VuZENvbG9yOm51bGwsYm9yZGVyQ29sb3I6bnVsbCx0aWNrQ29sb3I6bnVsbCxtYXJnaW46MCxsYWJlbE1hcmdpbjo1LGF4aXNNYXJnaW46OCxib3JkZXJXaWR0aDoyLG1pbkJvcmRlck1hcmdpbjpudWxsLG1hcmtpbmdzOm51bGwsbWFya2luZ3NDb2xvcjpcIiNmNGY0ZjRcIixtYXJraW5nc0xpbmVXaWR0aDoyLGNsaWNrYWJsZTpmYWxzZSxob3ZlcmFibGU6ZmFsc2UsYXV0b0hpZ2hsaWdodDp0cnVlLG1vdXNlQWN0aXZlUmFkaXVzOjEwfSxpbnRlcmFjdGlvbjp7cmVkcmF3T3ZlcmxheUludGVydmFsOjFlMy82MH0saG9va3M6e319LHN1cmZhY2U9bnVsbCxvdmVybGF5PW51bGwsZXZlbnRIb2xkZXI9bnVsbCxjdHg9bnVsbCxvY3R4PW51bGwseGF4ZXM9W10seWF4ZXM9W10scGxvdE9mZnNldD17bGVmdDowLHJpZ2h0OjAsdG9wOjAsYm90dG9tOjB9LHBsb3RXaWR0aD0wLHBsb3RIZWlnaHQ9MCxob29rcz17cHJvY2Vzc09wdGlvbnM6W10scHJvY2Vzc1Jhd0RhdGE6W10scHJvY2Vzc0RhdGFwb2ludHM6W10scHJvY2Vzc09mZnNldDpbXSxkcmF3QmFja2dyb3VuZDpbXSxkcmF3U2VyaWVzOltdLGRyYXc6W10sYmluZEV2ZW50czpbXSxkcmF3T3ZlcmxheTpbXSxzaHV0ZG93bjpbXX0scGxvdD10aGlzO3Bsb3Quc2V0RGF0YT1zZXREYXRhO3Bsb3Quc2V0dXBHcmlkPXNldHVwR3JpZDtwbG90LmRyYXc9ZHJhdztwbG90LmdldFBsYWNlaG9sZGVyPWZ1bmN0aW9uKCl7cmV0dXJuIHBsYWNlaG9sZGVyfTtwbG90LmdldENhbnZhcz1mdW5jdGlvbigpe3JldHVybiBzdXJmYWNlLmVsZW1lbnR9O3Bsb3QuZ2V0UGxvdE9mZnNldD1mdW5jdGlvbigpe3JldHVybiBwbG90T2Zmc2V0fTtwbG90LndpZHRoPWZ1bmN0aW9uKCl7cmV0dXJuIHBsb3RXaWR0aH07cGxvdC5oZWlnaHQ9ZnVuY3Rpb24oKXtyZXR1cm4gcGxvdEhlaWdodH07cGxvdC5vZmZzZXQ9ZnVuY3Rpb24oKXt2YXIgbz1ldmVudEhvbGRlci5vZmZzZXQoKTtvLmxlZnQrPXBsb3RPZmZzZXQubGVmdDtvLnRvcCs9cGxvdE9mZnNldC50b3A7cmV0dXJuIG99O3Bsb3QuZ2V0RGF0YT1mdW5jdGlvbigpe3JldHVybiBzZXJpZXN9O3Bsb3QuZ2V0QXhlcz1mdW5jdGlvbigpe3ZhciByZXM9e30saTskLmVhY2goeGF4ZXMuY29uY2F0KHlheGVzKSxmdW5jdGlvbihfLGF4aXMpe2lmKGF4aXMpcmVzW2F4aXMuZGlyZWN0aW9uKyhheGlzLm4hPTE/YXhpcy5uOlwiXCIpK1wiYXhpc1wiXT1heGlzfSk7cmV0dXJuIHJlc307cGxvdC5nZXRYQXhlcz1mdW5jdGlvbigpe3JldHVybiB4YXhlc307cGxvdC5nZXRZQXhlcz1mdW5jdGlvbigpe3JldHVybiB5YXhlc307cGxvdC5jMnA9Y2FudmFzVG9BeGlzQ29vcmRzO3Bsb3QucDJjPWF4aXNUb0NhbnZhc0Nvb3JkcztwbG90LmdldE9wdGlvbnM9ZnVuY3Rpb24oKXtyZXR1cm4gb3B0aW9uc307cGxvdC5oaWdobGlnaHQ9aGlnaGxpZ2h0O3Bsb3QudW5oaWdobGlnaHQ9dW5oaWdobGlnaHQ7cGxvdC50cmlnZ2VyUmVkcmF3T3ZlcmxheT10cmlnZ2VyUmVkcmF3T3ZlcmxheTtwbG90LnBvaW50T2Zmc2V0PWZ1bmN0aW9uKHBvaW50KXtyZXR1cm57bGVmdDpwYXJzZUludCh4YXhlc1theGlzTnVtYmVyKHBvaW50LFwieFwiKS0xXS5wMmMoK3BvaW50LngpK3Bsb3RPZmZzZXQubGVmdCwxMCksdG9wOnBhcnNlSW50KHlheGVzW2F4aXNOdW1iZXIocG9pbnQsXCJ5XCIpLTFdLnAyYygrcG9pbnQueSkrcGxvdE9mZnNldC50b3AsMTApfX07cGxvdC5zaHV0ZG93bj1zaHV0ZG93bjtwbG90LmRlc3Ryb3k9ZnVuY3Rpb24oKXtzaHV0ZG93bigpO3BsYWNlaG9sZGVyLnJlbW92ZURhdGEoXCJwbG90XCIpLmVtcHR5KCk7c2VyaWVzPVtdO29wdGlvbnM9bnVsbDtzdXJmYWNlPW51bGw7b3ZlcmxheT1udWxsO2V2ZW50SG9sZGVyPW51bGw7Y3R4PW51bGw7b2N0eD1udWxsO3hheGVzPVtdO3lheGVzPVtdO2hvb2tzPW51bGw7aGlnaGxpZ2h0cz1bXTtwbG90PW51bGx9O3Bsb3QucmVzaXplPWZ1bmN0aW9uKCl7dmFyIHdpZHRoPXBsYWNlaG9sZGVyLndpZHRoKCksaGVpZ2h0PXBsYWNlaG9sZGVyLmhlaWdodCgpO3N1cmZhY2UucmVzaXplKHdpZHRoLGhlaWdodCk7b3ZlcmxheS5yZXNpemUod2lkdGgsaGVpZ2h0KX07cGxvdC5ob29rcz1ob29rcztpbml0UGx1Z2lucyhwbG90KTtwYXJzZU9wdGlvbnMob3B0aW9uc18pO3NldHVwQ2FudmFzZXMoKTtzZXREYXRhKGRhdGFfKTtzZXR1cEdyaWQoKTtkcmF3KCk7YmluZEV2ZW50cygpO2Z1bmN0aW9uIGV4ZWN1dGVIb29rcyhob29rLGFyZ3Mpe2FyZ3M9W3Bsb3RdLmNvbmNhdChhcmdzKTtmb3IodmFyIGk9MDtpPGhvb2subGVuZ3RoOysraSlob29rW2ldLmFwcGx5KHRoaXMsYXJncyl9ZnVuY3Rpb24gaW5pdFBsdWdpbnMoKXt2YXIgY2xhc3Nlcz17Q2FudmFzOkNhbnZhc307Zm9yKHZhciBpPTA7aTxwbHVnaW5zLmxlbmd0aDsrK2kpe3ZhciBwPXBsdWdpbnNbaV07cC5pbml0KHBsb3QsY2xhc3Nlcyk7aWYocC5vcHRpb25zKSQuZXh0ZW5kKHRydWUsb3B0aW9ucyxwLm9wdGlvbnMpfX1mdW5jdGlvbiBwYXJzZU9wdGlvbnMob3B0cyl7JC5leHRlbmQodHJ1ZSxvcHRpb25zLG9wdHMpO2lmKG9wdHMmJm9wdHMuY29sb3JzKXtvcHRpb25zLmNvbG9ycz1vcHRzLmNvbG9yc31pZihvcHRpb25zLnhheGlzLmNvbG9yPT1udWxsKW9wdGlvbnMueGF4aXMuY29sb3I9JC5jb2xvci5wYXJzZShvcHRpb25zLmdyaWQuY29sb3IpLnNjYWxlKFwiYVwiLC4yMikudG9TdHJpbmcoKTtpZihvcHRpb25zLnlheGlzLmNvbG9yPT1udWxsKW9wdGlvbnMueWF4aXMuY29sb3I9JC5jb2xvci5wYXJzZShvcHRpb25zLmdyaWQuY29sb3IpLnNjYWxlKFwiYVwiLC4yMikudG9TdHJpbmcoKTtpZihvcHRpb25zLnhheGlzLnRpY2tDb2xvcj09bnVsbClvcHRpb25zLnhheGlzLnRpY2tDb2xvcj1vcHRpb25zLmdyaWQudGlja0NvbG9yfHxvcHRpb25zLnhheGlzLmNvbG9yO2lmKG9wdGlvbnMueWF4aXMudGlja0NvbG9yPT1udWxsKW9wdGlvbnMueWF4aXMudGlja0NvbG9yPW9wdGlvbnMuZ3JpZC50aWNrQ29sb3J8fG9wdGlvbnMueWF4aXMuY29sb3I7aWYob3B0aW9ucy5ncmlkLmJvcmRlckNvbG9yPT1udWxsKW9wdGlvbnMuZ3JpZC5ib3JkZXJDb2xvcj1vcHRpb25zLmdyaWQuY29sb3I7aWYob3B0aW9ucy5ncmlkLnRpY2tDb2xvcj09bnVsbClvcHRpb25zLmdyaWQudGlja0NvbG9yPSQuY29sb3IucGFyc2Uob3B0aW9ucy5ncmlkLmNvbG9yKS5zY2FsZShcImFcIiwuMjIpLnRvU3RyaW5nKCk7dmFyIGksYXhpc09wdGlvbnMsYXhpc0NvdW50LGZvbnRTaXplPXBsYWNlaG9sZGVyLmNzcyhcImZvbnQtc2l6ZVwiKSxmb250U2l6ZURlZmF1bHQ9Zm9udFNpemU/K2ZvbnRTaXplLnJlcGxhY2UoXCJweFwiLFwiXCIpOjEzLGZvbnREZWZhdWx0cz17c3R5bGU6cGxhY2Vob2xkZXIuY3NzKFwiZm9udC1zdHlsZVwiKSxzaXplOk1hdGgucm91bmQoLjgqZm9udFNpemVEZWZhdWx0KSx2YXJpYW50OnBsYWNlaG9sZGVyLmNzcyhcImZvbnQtdmFyaWFudFwiKSx3ZWlnaHQ6cGxhY2Vob2xkZXIuY3NzKFwiZm9udC13ZWlnaHRcIiksZmFtaWx5OnBsYWNlaG9sZGVyLmNzcyhcImZvbnQtZmFtaWx5XCIpfTtheGlzQ291bnQ9b3B0aW9ucy54YXhlcy5sZW5ndGh8fDE7Zm9yKGk9MDtpPGF4aXNDb3VudDsrK2kpe2F4aXNPcHRpb25zPW9wdGlvbnMueGF4ZXNbaV07aWYoYXhpc09wdGlvbnMmJiFheGlzT3B0aW9ucy50aWNrQ29sb3Ipe2F4aXNPcHRpb25zLnRpY2tDb2xvcj1heGlzT3B0aW9ucy5jb2xvcn1heGlzT3B0aW9ucz0kLmV4dGVuZCh0cnVlLHt9LG9wdGlvbnMueGF4aXMsYXhpc09wdGlvbnMpO29wdGlvbnMueGF4ZXNbaV09YXhpc09wdGlvbnM7aWYoYXhpc09wdGlvbnMuZm9udCl7YXhpc09wdGlvbnMuZm9udD0kLmV4dGVuZCh7fSxmb250RGVmYXVsdHMsYXhpc09wdGlvbnMuZm9udCk7aWYoIWF4aXNPcHRpb25zLmZvbnQuY29sb3Ipe2F4aXNPcHRpb25zLmZvbnQuY29sb3I9YXhpc09wdGlvbnMuY29sb3J9aWYoIWF4aXNPcHRpb25zLmZvbnQubGluZUhlaWdodCl7YXhpc09wdGlvbnMuZm9udC5saW5lSGVpZ2h0PU1hdGgucm91bmQoYXhpc09wdGlvbnMuZm9udC5zaXplKjEuMTUpfX19YXhpc0NvdW50PW9wdGlvbnMueWF4ZXMubGVuZ3RofHwxO2ZvcihpPTA7aTxheGlzQ291bnQ7KytpKXtheGlzT3B0aW9ucz1vcHRpb25zLnlheGVzW2ldO2lmKGF4aXNPcHRpb25zJiYhYXhpc09wdGlvbnMudGlja0NvbG9yKXtheGlzT3B0aW9ucy50aWNrQ29sb3I9YXhpc09wdGlvbnMuY29sb3J9YXhpc09wdGlvbnM9JC5leHRlbmQodHJ1ZSx7fSxvcHRpb25zLnlheGlzLGF4aXNPcHRpb25zKTtvcHRpb25zLnlheGVzW2ldPWF4aXNPcHRpb25zO2lmKGF4aXNPcHRpb25zLmZvbnQpe2F4aXNPcHRpb25zLmZvbnQ9JC5leHRlbmQoe30sZm9udERlZmF1bHRzLGF4aXNPcHRpb25zLmZvbnQpO2lmKCFheGlzT3B0aW9ucy5mb250LmNvbG9yKXtheGlzT3B0aW9ucy5mb250LmNvbG9yPWF4aXNPcHRpb25zLmNvbG9yfWlmKCFheGlzT3B0aW9ucy5mb250LmxpbmVIZWlnaHQpe2F4aXNPcHRpb25zLmZvbnQubGluZUhlaWdodD1NYXRoLnJvdW5kKGF4aXNPcHRpb25zLmZvbnQuc2l6ZSoxLjE1KX19fWlmKG9wdGlvbnMueGF4aXMubm9UaWNrcyYmb3B0aW9ucy54YXhpcy50aWNrcz09bnVsbClvcHRpb25zLnhheGlzLnRpY2tzPW9wdGlvbnMueGF4aXMubm9UaWNrcztpZihvcHRpb25zLnlheGlzLm5vVGlja3MmJm9wdGlvbnMueWF4aXMudGlja3M9PW51bGwpb3B0aW9ucy55YXhpcy50aWNrcz1vcHRpb25zLnlheGlzLm5vVGlja3M7aWYob3B0aW9ucy54MmF4aXMpe29wdGlvbnMueGF4ZXNbMV09JC5leHRlbmQodHJ1ZSx7fSxvcHRpb25zLnhheGlzLG9wdGlvbnMueDJheGlzKTtvcHRpb25zLnhheGVzWzFdLnBvc2l0aW9uPVwidG9wXCI7aWYob3B0aW9ucy54MmF4aXMubWluPT1udWxsKXtvcHRpb25zLnhheGVzWzFdLm1pbj1udWxsfWlmKG9wdGlvbnMueDJheGlzLm1heD09bnVsbCl7b3B0aW9ucy54YXhlc1sxXS5tYXg9bnVsbH19aWYob3B0aW9ucy55MmF4aXMpe29wdGlvbnMueWF4ZXNbMV09JC5leHRlbmQodHJ1ZSx7fSxvcHRpb25zLnlheGlzLG9wdGlvbnMueTJheGlzKTtvcHRpb25zLnlheGVzWzFdLnBvc2l0aW9uPVwicmlnaHRcIjtpZihvcHRpb25zLnkyYXhpcy5taW49PW51bGwpe29wdGlvbnMueWF4ZXNbMV0ubWluPW51bGx9aWYob3B0aW9ucy55MmF4aXMubWF4PT1udWxsKXtvcHRpb25zLnlheGVzWzFdLm1heD1udWxsfX1pZihvcHRpb25zLmdyaWQuY29sb3JlZEFyZWFzKW9wdGlvbnMuZ3JpZC5tYXJraW5ncz1vcHRpb25zLmdyaWQuY29sb3JlZEFyZWFzO2lmKG9wdGlvbnMuZ3JpZC5jb2xvcmVkQXJlYXNDb2xvcilvcHRpb25zLmdyaWQubWFya2luZ3NDb2xvcj1vcHRpb25zLmdyaWQuY29sb3JlZEFyZWFzQ29sb3I7aWYob3B0aW9ucy5saW5lcykkLmV4dGVuZCh0cnVlLG9wdGlvbnMuc2VyaWVzLmxpbmVzLG9wdGlvbnMubGluZXMpO2lmKG9wdGlvbnMucG9pbnRzKSQuZXh0ZW5kKHRydWUsb3B0aW9ucy5zZXJpZXMucG9pbnRzLG9wdGlvbnMucG9pbnRzKTtpZihvcHRpb25zLmJhcnMpJC5leHRlbmQodHJ1ZSxvcHRpb25zLnNlcmllcy5iYXJzLG9wdGlvbnMuYmFycyk7aWYob3B0aW9ucy5zaGFkb3dTaXplIT1udWxsKW9wdGlvbnMuc2VyaWVzLnNoYWRvd1NpemU9b3B0aW9ucy5zaGFkb3dTaXplO2lmKG9wdGlvbnMuaGlnaGxpZ2h0Q29sb3IhPW51bGwpb3B0aW9ucy5zZXJpZXMuaGlnaGxpZ2h0Q29sb3I9b3B0aW9ucy5oaWdobGlnaHRDb2xvcjtmb3IoaT0wO2k8b3B0aW9ucy54YXhlcy5sZW5ndGg7KytpKWdldE9yQ3JlYXRlQXhpcyh4YXhlcyxpKzEpLm9wdGlvbnM9b3B0aW9ucy54YXhlc1tpXTtmb3IoaT0wO2k8b3B0aW9ucy55YXhlcy5sZW5ndGg7KytpKWdldE9yQ3JlYXRlQXhpcyh5YXhlcyxpKzEpLm9wdGlvbnM9b3B0aW9ucy55YXhlc1tpXTtmb3IodmFyIG4gaW4gaG9va3MpaWYob3B0aW9ucy5ob29rc1tuXSYmb3B0aW9ucy5ob29rc1tuXS5sZW5ndGgpaG9va3Nbbl09aG9va3Nbbl0uY29uY2F0KG9wdGlvbnMuaG9va3Nbbl0pO2V4ZWN1dGVIb29rcyhob29rcy5wcm9jZXNzT3B0aW9ucyxbb3B0aW9uc10pfWZ1bmN0aW9uIHNldERhdGEoZCl7c2VyaWVzPXBhcnNlRGF0YShkKTtmaWxsSW5TZXJpZXNPcHRpb25zKCk7cHJvY2Vzc0RhdGEoKX1mdW5jdGlvbiBwYXJzZURhdGEoZCl7dmFyIHJlcz1bXTtmb3IodmFyIGk9MDtpPGQubGVuZ3RoOysraSl7dmFyIHM9JC5leHRlbmQodHJ1ZSx7fSxvcHRpb25zLnNlcmllcyk7aWYoZFtpXS5kYXRhIT1udWxsKXtzLmRhdGE9ZFtpXS5kYXRhO2RlbGV0ZSBkW2ldLmRhdGE7JC5leHRlbmQodHJ1ZSxzLGRbaV0pO2RbaV0uZGF0YT1zLmRhdGF9ZWxzZSBzLmRhdGE9ZFtpXTtyZXMucHVzaChzKX1yZXR1cm4gcmVzfWZ1bmN0aW9uIGF4aXNOdW1iZXIob2JqLGNvb3JkKXt2YXIgYT1vYmpbY29vcmQrXCJheGlzXCJdO2lmKHR5cGVvZiBhPT1cIm9iamVjdFwiKWE9YS5uO2lmKHR5cGVvZiBhIT1cIm51bWJlclwiKWE9MTtyZXR1cm4gYX1mdW5jdGlvbiBhbGxBeGVzKCl7cmV0dXJuICQuZ3JlcCh4YXhlcy5jb25jYXQoeWF4ZXMpLGZ1bmN0aW9uKGEpe3JldHVybiBhfSl9ZnVuY3Rpb24gY2FudmFzVG9BeGlzQ29vcmRzKHBvcyl7dmFyIHJlcz17fSxpLGF4aXM7Zm9yKGk9MDtpPHhheGVzLmxlbmd0aDsrK2kpe2F4aXM9eGF4ZXNbaV07aWYoYXhpcyYmYXhpcy51c2VkKXJlc1tcInhcIitheGlzLm5dPWF4aXMuYzJwKHBvcy5sZWZ0KX1mb3IoaT0wO2k8eWF4ZXMubGVuZ3RoOysraSl7YXhpcz15YXhlc1tpXTtpZihheGlzJiZheGlzLnVzZWQpcmVzW1wieVwiK2F4aXMubl09YXhpcy5jMnAocG9zLnRvcCl9aWYocmVzLngxIT09dW5kZWZpbmVkKXJlcy54PXJlcy54MTtpZihyZXMueTEhPT11bmRlZmluZWQpcmVzLnk9cmVzLnkxO3JldHVybiByZXN9ZnVuY3Rpb24gYXhpc1RvQ2FudmFzQ29vcmRzKHBvcyl7dmFyIHJlcz17fSxpLGF4aXMsa2V5O2ZvcihpPTA7aTx4YXhlcy5sZW5ndGg7KytpKXtheGlzPXhheGVzW2ldO2lmKGF4aXMmJmF4aXMudXNlZCl7a2V5PVwieFwiK2F4aXMubjtpZihwb3Nba2V5XT09bnVsbCYmYXhpcy5uPT0xKWtleT1cInhcIjtpZihwb3Nba2V5XSE9bnVsbCl7cmVzLmxlZnQ9YXhpcy5wMmMocG9zW2tleV0pO2JyZWFrfX19Zm9yKGk9MDtpPHlheGVzLmxlbmd0aDsrK2kpe2F4aXM9eWF4ZXNbaV07aWYoYXhpcyYmYXhpcy51c2VkKXtrZXk9XCJ5XCIrYXhpcy5uO2lmKHBvc1trZXldPT1udWxsJiZheGlzLm49PTEpa2V5PVwieVwiO2lmKHBvc1trZXldIT1udWxsKXtyZXMudG9wPWF4aXMucDJjKHBvc1trZXldKTticmVha319fXJldHVybiByZXN9ZnVuY3Rpb24gZ2V0T3JDcmVhdGVBeGlzKGF4ZXMsbnVtYmVyKXtpZighYXhlc1tudW1iZXItMV0pYXhlc1tudW1iZXItMV09e246bnVtYmVyLGRpcmVjdGlvbjpheGVzPT14YXhlcz9cInhcIjpcInlcIixvcHRpb25zOiQuZXh0ZW5kKHRydWUse30sYXhlcz09eGF4ZXM/b3B0aW9ucy54YXhpczpvcHRpb25zLnlheGlzKX07cmV0dXJuIGF4ZXNbbnVtYmVyLTFdfWZ1bmN0aW9uIGZpbGxJblNlcmllc09wdGlvbnMoKXt2YXIgbmVlZGVkQ29sb3JzPXNlcmllcy5sZW5ndGgsbWF4SW5kZXg9LTEsaTtmb3IoaT0wO2k8c2VyaWVzLmxlbmd0aDsrK2kpe3ZhciBzYz1zZXJpZXNbaV0uY29sb3I7aWYoc2MhPW51bGwpe25lZWRlZENvbG9ycy0tO2lmKHR5cGVvZiBzYz09XCJudW1iZXJcIiYmc2M+bWF4SW5kZXgpe21heEluZGV4PXNjfX19aWYobmVlZGVkQ29sb3JzPD1tYXhJbmRleCl7bmVlZGVkQ29sb3JzPW1heEluZGV4KzF9dmFyIGMsY29sb3JzPVtdLGNvbG9yUG9vbD1vcHRpb25zLmNvbG9ycyxjb2xvclBvb2xTaXplPWNvbG9yUG9vbC5sZW5ndGgsdmFyaWF0aW9uPTA7Zm9yKGk9MDtpPG5lZWRlZENvbG9ycztpKyspe2M9JC5jb2xvci5wYXJzZShjb2xvclBvb2xbaSVjb2xvclBvb2xTaXplXXx8XCIjNjY2XCIpO2lmKGklY29sb3JQb29sU2l6ZT09MCYmaSl7aWYodmFyaWF0aW9uPj0wKXtpZih2YXJpYXRpb248LjUpe3ZhcmlhdGlvbj0tdmFyaWF0aW9uLS4yfWVsc2UgdmFyaWF0aW9uPTB9ZWxzZSB2YXJpYXRpb249LXZhcmlhdGlvbn1jb2xvcnNbaV09Yy5zY2FsZShcInJnYlwiLDErdmFyaWF0aW9uKX12YXIgY29sb3JpPTAscztmb3IoaT0wO2k8c2VyaWVzLmxlbmd0aDsrK2kpe3M9c2VyaWVzW2ldO2lmKHMuY29sb3I9PW51bGwpe3MuY29sb3I9Y29sb3JzW2NvbG9yaV0udG9TdHJpbmcoKTsrK2NvbG9yaX1lbHNlIGlmKHR5cGVvZiBzLmNvbG9yPT1cIm51bWJlclwiKXMuY29sb3I9Y29sb3JzW3MuY29sb3JdLnRvU3RyaW5nKCk7aWYocy5saW5lcy5zaG93PT1udWxsKXt2YXIgdixzaG93PXRydWU7Zm9yKHYgaW4gcylpZihzW3ZdJiZzW3ZdLnNob3cpe3Nob3c9ZmFsc2U7YnJlYWt9aWYoc2hvdylzLmxpbmVzLnNob3c9dHJ1ZX1pZihzLmxpbmVzLnplcm89PW51bGwpe3MubGluZXMuemVybz0hIXMubGluZXMuZmlsbH1zLnhheGlzPWdldE9yQ3JlYXRlQXhpcyh4YXhlcyxheGlzTnVtYmVyKHMsXCJ4XCIpKTtzLnlheGlzPWdldE9yQ3JlYXRlQXhpcyh5YXhlcyxheGlzTnVtYmVyKHMsXCJ5XCIpKX19ZnVuY3Rpb24gcHJvY2Vzc0RhdGEoKXt2YXIgdG9wU2VudHJ5PU51bWJlci5QT1NJVElWRV9JTkZJTklUWSxib3R0b21TZW50cnk9TnVtYmVyLk5FR0FUSVZFX0lORklOSVRZLGZha2VJbmZpbml0eT1OdW1iZXIuTUFYX1ZBTFVFLGksaixrLG0sbGVuZ3RoLHMscG9pbnRzLHBzLHgseSxheGlzLHZhbCxmLHAsZGF0YSxmb3JtYXQ7ZnVuY3Rpb24gdXBkYXRlQXhpcyhheGlzLG1pbixtYXgpe2lmKG1pbjxheGlzLmRhdGFtaW4mJm1pbiE9LWZha2VJbmZpbml0eSlheGlzLmRhdGFtaW49bWluO2lmKG1heD5heGlzLmRhdGFtYXgmJm1heCE9ZmFrZUluZmluaXR5KWF4aXMuZGF0YW1heD1tYXh9JC5lYWNoKGFsbEF4ZXMoKSxmdW5jdGlvbihfLGF4aXMpe2F4aXMuZGF0YW1pbj10b3BTZW50cnk7YXhpcy5kYXRhbWF4PWJvdHRvbVNlbnRyeTtheGlzLnVzZWQ9ZmFsc2V9KTtmb3IoaT0wO2k8c2VyaWVzLmxlbmd0aDsrK2kpe3M9c2VyaWVzW2ldO3MuZGF0YXBvaW50cz17cG9pbnRzOltdfTtleGVjdXRlSG9va3MoaG9va3MucHJvY2Vzc1Jhd0RhdGEsW3Mscy5kYXRhLHMuZGF0YXBvaW50c10pfWZvcihpPTA7aTxzZXJpZXMubGVuZ3RoOysraSl7cz1zZXJpZXNbaV07ZGF0YT1zLmRhdGE7Zm9ybWF0PXMuZGF0YXBvaW50cy5mb3JtYXQ7aWYoIWZvcm1hdCl7Zm9ybWF0PVtdO2Zvcm1hdC5wdXNoKHt4OnRydWUsbnVtYmVyOnRydWUscmVxdWlyZWQ6dHJ1ZX0pO2Zvcm1hdC5wdXNoKHt5OnRydWUsbnVtYmVyOnRydWUscmVxdWlyZWQ6dHJ1ZX0pO2lmKHMuYmFycy5zaG93fHxzLmxpbmVzLnNob3cmJnMubGluZXMuZmlsbCl7dmFyIGF1dG9zY2FsZT0hIShzLmJhcnMuc2hvdyYmcy5iYXJzLnplcm98fHMubGluZXMuc2hvdyYmcy5saW5lcy56ZXJvKTtmb3JtYXQucHVzaCh7eTp0cnVlLG51bWJlcjp0cnVlLHJlcXVpcmVkOmZhbHNlLGRlZmF1bHRWYWx1ZTowLGF1dG9zY2FsZTphdXRvc2NhbGV9KTtpZihzLmJhcnMuaG9yaXpvbnRhbCl7ZGVsZXRlIGZvcm1hdFtmb3JtYXQubGVuZ3RoLTFdLnk7Zm9ybWF0W2Zvcm1hdC5sZW5ndGgtMV0ueD10cnVlfX1zLmRhdGFwb2ludHMuZm9ybWF0PWZvcm1hdH1pZihzLmRhdGFwb2ludHMucG9pbnRzaXplIT1udWxsKWNvbnRpbnVlO3MuZGF0YXBvaW50cy5wb2ludHNpemU9Zm9ybWF0Lmxlbmd0aDtwcz1zLmRhdGFwb2ludHMucG9pbnRzaXplO3BvaW50cz1zLmRhdGFwb2ludHMucG9pbnRzO3ZhciBpbnNlcnRTdGVwcz1zLmxpbmVzLnNob3cmJnMubGluZXMuc3RlcHM7cy54YXhpcy51c2VkPXMueWF4aXMudXNlZD10cnVlO2ZvcihqPWs9MDtqPGRhdGEubGVuZ3RoOysraixrKz1wcyl7cD1kYXRhW2pdO3ZhciBudWxsaWZ5PXA9PW51bGw7aWYoIW51bGxpZnkpe2ZvcihtPTA7bTxwczsrK20pe3ZhbD1wW21dO2Y9Zm9ybWF0W21dO2lmKGYpe2lmKGYubnVtYmVyJiZ2YWwhPW51bGwpe3ZhbD0rdmFsO2lmKGlzTmFOKHZhbCkpdmFsPW51bGw7ZWxzZSBpZih2YWw9PUluZmluaXR5KXZhbD1mYWtlSW5maW5pdHk7ZWxzZSBpZih2YWw9PS1JbmZpbml0eSl2YWw9LWZha2VJbmZpbml0eX1pZih2YWw9PW51bGwpe2lmKGYucmVxdWlyZWQpbnVsbGlmeT10cnVlO2lmKGYuZGVmYXVsdFZhbHVlIT1udWxsKXZhbD1mLmRlZmF1bHRWYWx1ZX19cG9pbnRzW2srbV09dmFsfX1pZihudWxsaWZ5KXtmb3IobT0wO208cHM7KyttKXt2YWw9cG9pbnRzW2srbV07aWYodmFsIT1udWxsKXtmPWZvcm1hdFttXTtpZihmLmF1dG9zY2FsZSE9PWZhbHNlKXtpZihmLngpe3VwZGF0ZUF4aXMocy54YXhpcyx2YWwsdmFsKX1pZihmLnkpe3VwZGF0ZUF4aXMocy55YXhpcyx2YWwsdmFsKX19fXBvaW50c1trK21dPW51bGx9fWVsc2V7aWYoaW5zZXJ0U3RlcHMmJms+MCYmcG9pbnRzW2stcHNdIT1udWxsJiZwb2ludHNbay1wc10hPXBvaW50c1trXSYmcG9pbnRzW2stcHMrMV0hPXBvaW50c1trKzFdKXtmb3IobT0wO208cHM7KyttKXBvaW50c1trK3BzK21dPXBvaW50c1trK21dO3BvaW50c1trKzFdPXBvaW50c1trLXBzKzFdO2srPXBzfX19fWZvcihpPTA7aTxzZXJpZXMubGVuZ3RoOysraSl7cz1zZXJpZXNbaV07ZXhlY3V0ZUhvb2tzKGhvb2tzLnByb2Nlc3NEYXRhcG9pbnRzLFtzLHMuZGF0YXBvaW50c10pfWZvcihpPTA7aTxzZXJpZXMubGVuZ3RoOysraSl7cz1zZXJpZXNbaV07cG9pbnRzPXMuZGF0YXBvaW50cy5wb2ludHM7cHM9cy5kYXRhcG9pbnRzLnBvaW50c2l6ZTtmb3JtYXQ9cy5kYXRhcG9pbnRzLmZvcm1hdDt2YXIgeG1pbj10b3BTZW50cnkseW1pbj10b3BTZW50cnkseG1heD1ib3R0b21TZW50cnkseW1heD1ib3R0b21TZW50cnk7Zm9yKGo9MDtqPHBvaW50cy5sZW5ndGg7ais9cHMpe2lmKHBvaW50c1tqXT09bnVsbCljb250aW51ZTtmb3IobT0wO208cHM7KyttKXt2YWw9cG9pbnRzW2orbV07Zj1mb3JtYXRbbV07aWYoIWZ8fGYuYXV0b3NjYWxlPT09ZmFsc2V8fHZhbD09ZmFrZUluZmluaXR5fHx2YWw9PS1mYWtlSW5maW5pdHkpY29udGludWU7aWYoZi54KXtpZih2YWw8eG1pbil4bWluPXZhbDtpZih2YWw+eG1heCl4bWF4PXZhbH1pZihmLnkpe2lmKHZhbDx5bWluKXltaW49dmFsO2lmKHZhbD55bWF4KXltYXg9dmFsfX19aWYocy5iYXJzLnNob3cpe3ZhciBkZWx0YTtzd2l0Y2gocy5iYXJzLmFsaWduKXtjYXNlXCJsZWZ0XCI6ZGVsdGE9MDticmVhaztjYXNlXCJyaWdodFwiOmRlbHRhPS1zLmJhcnMuYmFyV2lkdGg7YnJlYWs7ZGVmYXVsdDpkZWx0YT0tcy5iYXJzLmJhcldpZHRoLzJ9aWYocy5iYXJzLmhvcml6b250YWwpe3ltaW4rPWRlbHRhO3ltYXgrPWRlbHRhK3MuYmFycy5iYXJXaWR0aH1lbHNle3htaW4rPWRlbHRhO3htYXgrPWRlbHRhK3MuYmFycy5iYXJXaWR0aH19dXBkYXRlQXhpcyhzLnhheGlzLHhtaW4seG1heCk7dXBkYXRlQXhpcyhzLnlheGlzLHltaW4seW1heCl9JC5lYWNoKGFsbEF4ZXMoKSxmdW5jdGlvbihfLGF4aXMpe2lmKGF4aXMuZGF0YW1pbj09dG9wU2VudHJ5KWF4aXMuZGF0YW1pbj1udWxsO2lmKGF4aXMuZGF0YW1heD09Ym90dG9tU2VudHJ5KWF4aXMuZGF0YW1heD1udWxsfSl9ZnVuY3Rpb24gc2V0dXBDYW52YXNlcygpe3BsYWNlaG9sZGVyLmNzcyhcInBhZGRpbmdcIiwwKS5jaGlsZHJlbigpLmZpbHRlcihmdW5jdGlvbigpe3JldHVybiEkKHRoaXMpLmhhc0NsYXNzKFwiZmxvdC1vdmVybGF5XCIpJiYhJCh0aGlzKS5oYXNDbGFzcyhcImZsb3QtYmFzZVwiKX0pLnJlbW92ZSgpO2lmKHBsYWNlaG9sZGVyLmNzcyhcInBvc2l0aW9uXCIpPT1cInN0YXRpY1wiKXBsYWNlaG9sZGVyLmNzcyhcInBvc2l0aW9uXCIsXCJyZWxhdGl2ZVwiKTtzdXJmYWNlPW5ldyBDYW52YXMoXCJmbG90LWJhc2VcIixwbGFjZWhvbGRlcik7b3ZlcmxheT1uZXcgQ2FudmFzKFwiZmxvdC1vdmVybGF5XCIscGxhY2Vob2xkZXIpO2N0eD1zdXJmYWNlLmNvbnRleHQ7b2N0eD1vdmVybGF5LmNvbnRleHQ7ZXZlbnRIb2xkZXI9JChvdmVybGF5LmVsZW1lbnQpLnVuYmluZCgpO3ZhciBleGlzdGluZz1wbGFjZWhvbGRlci5kYXRhKFwicGxvdFwiKTtpZihleGlzdGluZyl7ZXhpc3Rpbmcuc2h1dGRvd24oKTtvdmVybGF5LmNsZWFyKCl9cGxhY2Vob2xkZXIuZGF0YShcInBsb3RcIixwbG90KX1mdW5jdGlvbiBiaW5kRXZlbnRzKCl7aWYob3B0aW9ucy5ncmlkLmhvdmVyYWJsZSl7ZXZlbnRIb2xkZXIubW91c2Vtb3ZlKG9uTW91c2VNb3ZlKTtldmVudEhvbGRlci5iaW5kKFwibW91c2VsZWF2ZVwiLG9uTW91c2VMZWF2ZSl9aWYob3B0aW9ucy5ncmlkLmNsaWNrYWJsZSlldmVudEhvbGRlci5jbGljayhvbkNsaWNrKTtleGVjdXRlSG9va3MoaG9va3MuYmluZEV2ZW50cyxbZXZlbnRIb2xkZXJdKX1mdW5jdGlvbiBzaHV0ZG93bigpe2lmKHJlZHJhd1RpbWVvdXQpY2xlYXJUaW1lb3V0KHJlZHJhd1RpbWVvdXQpO2V2ZW50SG9sZGVyLnVuYmluZChcIm1vdXNlbW92ZVwiLG9uTW91c2VNb3ZlKTtldmVudEhvbGRlci51bmJpbmQoXCJtb3VzZWxlYXZlXCIsb25Nb3VzZUxlYXZlKTtldmVudEhvbGRlci51bmJpbmQoXCJjbGlja1wiLG9uQ2xpY2spO2V4ZWN1dGVIb29rcyhob29rcy5zaHV0ZG93bixbZXZlbnRIb2xkZXJdKX1mdW5jdGlvbiBzZXRUcmFuc2Zvcm1hdGlvbkhlbHBlcnMoYXhpcyl7ZnVuY3Rpb24gaWRlbnRpdHkoeCl7cmV0dXJuIHh9dmFyIHMsbSx0PWF4aXMub3B0aW9ucy50cmFuc2Zvcm18fGlkZW50aXR5LGl0PWF4aXMub3B0aW9ucy5pbnZlcnNlVHJhbnNmb3JtO2lmKGF4aXMuZGlyZWN0aW9uPT1cInhcIil7cz1heGlzLnNjYWxlPXBsb3RXaWR0aC9NYXRoLmFicyh0KGF4aXMubWF4KS10KGF4aXMubWluKSk7bT1NYXRoLm1pbih0KGF4aXMubWF4KSx0KGF4aXMubWluKSl9ZWxzZXtzPWF4aXMuc2NhbGU9cGxvdEhlaWdodC9NYXRoLmFicyh0KGF4aXMubWF4KS10KGF4aXMubWluKSk7cz0tczttPU1hdGgubWF4KHQoYXhpcy5tYXgpLHQoYXhpcy5taW4pKX1pZih0PT1pZGVudGl0eSlheGlzLnAyYz1mdW5jdGlvbihwKXtyZXR1cm4ocC1tKSpzfTtlbHNlIGF4aXMucDJjPWZ1bmN0aW9uKHApe3JldHVybih0KHApLW0pKnN9O2lmKCFpdClheGlzLmMycD1mdW5jdGlvbihjKXtyZXR1cm4gbStjL3N9O2Vsc2UgYXhpcy5jMnA9ZnVuY3Rpb24oYyl7cmV0dXJuIGl0KG0rYy9zKX19ZnVuY3Rpb24gbWVhc3VyZVRpY2tMYWJlbHMoYXhpcyl7dmFyIG9wdHM9YXhpcy5vcHRpb25zLHRpY2tzPWF4aXMudGlja3N8fFtdLGxhYmVsV2lkdGg9b3B0cy5sYWJlbFdpZHRofHwwLGxhYmVsSGVpZ2h0PW9wdHMubGFiZWxIZWlnaHR8fDAsbWF4V2lkdGg9bGFiZWxXaWR0aHx8KGF4aXMuZGlyZWN0aW9uPT1cInhcIj9NYXRoLmZsb29yKHN1cmZhY2Uud2lkdGgvKHRpY2tzLmxlbmd0aHx8MSkpOm51bGwpLGxlZ2FjeVN0eWxlcz1heGlzLmRpcmVjdGlvbitcIkF4aXMgXCIrYXhpcy5kaXJlY3Rpb24rYXhpcy5uK1wiQXhpc1wiLGxheWVyPVwiZmxvdC1cIitheGlzLmRpcmVjdGlvbitcIi1heGlzIGZsb3QtXCIrYXhpcy5kaXJlY3Rpb24rYXhpcy5uK1wiLWF4aXMgXCIrbGVnYWN5U3R5bGVzLGZvbnQ9b3B0cy5mb250fHxcImZsb3QtdGljay1sYWJlbCB0aWNrTGFiZWxcIjtmb3IodmFyIGk9MDtpPHRpY2tzLmxlbmd0aDsrK2kpe3ZhciB0PXRpY2tzW2ldO2lmKCF0LmxhYmVsKWNvbnRpbnVlO3ZhciBpbmZvPXN1cmZhY2UuZ2V0VGV4dEluZm8obGF5ZXIsdC5sYWJlbCxmb250LG51bGwsbWF4V2lkdGgpO2xhYmVsV2lkdGg9TWF0aC5tYXgobGFiZWxXaWR0aCxpbmZvLndpZHRoKTtsYWJlbEhlaWdodD1NYXRoLm1heChsYWJlbEhlaWdodCxpbmZvLmhlaWdodCl9YXhpcy5sYWJlbFdpZHRoPW9wdHMubGFiZWxXaWR0aHx8bGFiZWxXaWR0aDtheGlzLmxhYmVsSGVpZ2h0PW9wdHMubGFiZWxIZWlnaHR8fGxhYmVsSGVpZ2h0fWZ1bmN0aW9uIGFsbG9jYXRlQXhpc0JveEZpcnN0UGhhc2UoYXhpcyl7dmFyIGx3PWF4aXMubGFiZWxXaWR0aCxsaD1heGlzLmxhYmVsSGVpZ2h0LHBvcz1heGlzLm9wdGlvbnMucG9zaXRpb24saXNYQXhpcz1heGlzLmRpcmVjdGlvbj09PVwieFwiLHRpY2tMZW5ndGg9YXhpcy5vcHRpb25zLnRpY2tMZW5ndGgsYXhpc01hcmdpbj1vcHRpb25zLmdyaWQuYXhpc01hcmdpbixwYWRkaW5nPW9wdGlvbnMuZ3JpZC5sYWJlbE1hcmdpbixpbm5lcm1vc3Q9dHJ1ZSxvdXRlcm1vc3Q9dHJ1ZSxmaXJzdD10cnVlLGZvdW5kPWZhbHNlOyQuZWFjaChpc1hBeGlzP3hheGVzOnlheGVzLGZ1bmN0aW9uKGksYSl7aWYoYSYmKGEuc2hvd3x8YS5yZXNlcnZlU3BhY2UpKXtpZihhPT09YXhpcyl7Zm91bmQ9dHJ1ZX1lbHNlIGlmKGEub3B0aW9ucy5wb3NpdGlvbj09PXBvcyl7aWYoZm91bmQpe291dGVybW9zdD1mYWxzZX1lbHNle2lubmVybW9zdD1mYWxzZX19aWYoIWZvdW5kKXtmaXJzdD1mYWxzZX19fSk7aWYob3V0ZXJtb3N0KXtheGlzTWFyZ2luPTB9aWYodGlja0xlbmd0aD09bnVsbCl7dGlja0xlbmd0aD1maXJzdD9cImZ1bGxcIjo1fWlmKCFpc05hTigrdGlja0xlbmd0aCkpcGFkZGluZys9K3RpY2tMZW5ndGg7aWYoaXNYQXhpcyl7bGgrPXBhZGRpbmc7aWYocG9zPT1cImJvdHRvbVwiKXtwbG90T2Zmc2V0LmJvdHRvbSs9bGgrYXhpc01hcmdpbjtheGlzLmJveD17dG9wOnN1cmZhY2UuaGVpZ2h0LXBsb3RPZmZzZXQuYm90dG9tLGhlaWdodDpsaH19ZWxzZXtheGlzLmJveD17dG9wOnBsb3RPZmZzZXQudG9wK2F4aXNNYXJnaW4saGVpZ2h0OmxofTtwbG90T2Zmc2V0LnRvcCs9bGgrYXhpc01hcmdpbn19ZWxzZXtsdys9cGFkZGluZztpZihwb3M9PVwibGVmdFwiKXtheGlzLmJveD17bGVmdDpwbG90T2Zmc2V0LmxlZnQrYXhpc01hcmdpbix3aWR0aDpsd307cGxvdE9mZnNldC5sZWZ0Kz1sdytheGlzTWFyZ2lufWVsc2V7cGxvdE9mZnNldC5yaWdodCs9bHcrYXhpc01hcmdpbjtheGlzLmJveD17bGVmdDpzdXJmYWNlLndpZHRoLXBsb3RPZmZzZXQucmlnaHQsd2lkdGg6bHd9fX1heGlzLnBvc2l0aW9uPXBvcztheGlzLnRpY2tMZW5ndGg9dGlja0xlbmd0aDtheGlzLmJveC5wYWRkaW5nPXBhZGRpbmc7YXhpcy5pbm5lcm1vc3Q9aW5uZXJtb3N0fWZ1bmN0aW9uIGFsbG9jYXRlQXhpc0JveFNlY29uZFBoYXNlKGF4aXMpe2lmKGF4aXMuZGlyZWN0aW9uPT1cInhcIil7YXhpcy5ib3gubGVmdD1wbG90T2Zmc2V0LmxlZnQtYXhpcy5sYWJlbFdpZHRoLzI7YXhpcy5ib3gud2lkdGg9c3VyZmFjZS53aWR0aC1wbG90T2Zmc2V0LmxlZnQtcGxvdE9mZnNldC5yaWdodCtheGlzLmxhYmVsV2lkdGh9ZWxzZXtheGlzLmJveC50b3A9cGxvdE9mZnNldC50b3AtYXhpcy5sYWJlbEhlaWdodC8yO2F4aXMuYm94LmhlaWdodD1zdXJmYWNlLmhlaWdodC1wbG90T2Zmc2V0LmJvdHRvbS1wbG90T2Zmc2V0LnRvcCtheGlzLmxhYmVsSGVpZ2h0fX1mdW5jdGlvbiBhZGp1c3RMYXlvdXRGb3JUaGluZ3NTdGlja2luZ091dCgpe3ZhciBtaW5NYXJnaW49b3B0aW9ucy5ncmlkLm1pbkJvcmRlck1hcmdpbixheGlzLGk7aWYobWluTWFyZ2luPT1udWxsKXttaW5NYXJnaW49MDtmb3IoaT0wO2k8c2VyaWVzLmxlbmd0aDsrK2kpbWluTWFyZ2luPU1hdGgubWF4KG1pbk1hcmdpbiwyKihzZXJpZXNbaV0ucG9pbnRzLnJhZGl1cytzZXJpZXNbaV0ucG9pbnRzLmxpbmVXaWR0aC8yKSl9dmFyIG1hcmdpbnM9e2xlZnQ6bWluTWFyZ2luLHJpZ2h0Om1pbk1hcmdpbix0b3A6bWluTWFyZ2luLGJvdHRvbTptaW5NYXJnaW59OyQuZWFjaChhbGxBeGVzKCksZnVuY3Rpb24oXyxheGlzKXtpZihheGlzLnJlc2VydmVTcGFjZSYmYXhpcy50aWNrcyYmYXhpcy50aWNrcy5sZW5ndGgpe2lmKGF4aXMuZGlyZWN0aW9uPT09XCJ4XCIpe21hcmdpbnMubGVmdD1NYXRoLm1heChtYXJnaW5zLmxlZnQsYXhpcy5sYWJlbFdpZHRoLzIpO21hcmdpbnMucmlnaHQ9TWF0aC5tYXgobWFyZ2lucy5yaWdodCxheGlzLmxhYmVsV2lkdGgvMil9ZWxzZXttYXJnaW5zLmJvdHRvbT1NYXRoLm1heChtYXJnaW5zLmJvdHRvbSxheGlzLmxhYmVsSGVpZ2h0LzIpO21hcmdpbnMudG9wPU1hdGgubWF4KG1hcmdpbnMudG9wLGF4aXMubGFiZWxIZWlnaHQvMil9fX0pO3Bsb3RPZmZzZXQubGVmdD1NYXRoLmNlaWwoTWF0aC5tYXgobWFyZ2lucy5sZWZ0LHBsb3RPZmZzZXQubGVmdCkpO3Bsb3RPZmZzZXQucmlnaHQ9TWF0aC5jZWlsKE1hdGgubWF4KG1hcmdpbnMucmlnaHQscGxvdE9mZnNldC5yaWdodCkpO3Bsb3RPZmZzZXQudG9wPU1hdGguY2VpbChNYXRoLm1heChtYXJnaW5zLnRvcCxwbG90T2Zmc2V0LnRvcCkpO3Bsb3RPZmZzZXQuYm90dG9tPU1hdGguY2VpbChNYXRoLm1heChtYXJnaW5zLmJvdHRvbSxwbG90T2Zmc2V0LmJvdHRvbSkpfWZ1bmN0aW9uIHNldHVwR3JpZCgpe3ZhciBpLGF4ZXM9YWxsQXhlcygpLHNob3dHcmlkPW9wdGlvbnMuZ3JpZC5zaG93O2Zvcih2YXIgYSBpbiBwbG90T2Zmc2V0KXt2YXIgbWFyZ2luPW9wdGlvbnMuZ3JpZC5tYXJnaW58fDA7cGxvdE9mZnNldFthXT10eXBlb2YgbWFyZ2luPT1cIm51bWJlclwiP21hcmdpbjptYXJnaW5bYV18fDB9ZXhlY3V0ZUhvb2tzKGhvb2tzLnByb2Nlc3NPZmZzZXQsW3Bsb3RPZmZzZXRdKTtmb3IodmFyIGEgaW4gcGxvdE9mZnNldCl7aWYodHlwZW9mIG9wdGlvbnMuZ3JpZC5ib3JkZXJXaWR0aD09XCJvYmplY3RcIil7cGxvdE9mZnNldFthXSs9c2hvd0dyaWQ/b3B0aW9ucy5ncmlkLmJvcmRlcldpZHRoW2FdOjB9ZWxzZXtwbG90T2Zmc2V0W2FdKz1zaG93R3JpZD9vcHRpb25zLmdyaWQuYm9yZGVyV2lkdGg6MH19JC5lYWNoKGF4ZXMsZnVuY3Rpb24oXyxheGlzKXt2YXIgYXhpc09wdHM9YXhpcy5vcHRpb25zO2F4aXMuc2hvdz1heGlzT3B0cy5zaG93PT1udWxsP2F4aXMudXNlZDpheGlzT3B0cy5zaG93O2F4aXMucmVzZXJ2ZVNwYWNlPWF4aXNPcHRzLnJlc2VydmVTcGFjZT09bnVsbD9heGlzLnNob3c6YXhpc09wdHMucmVzZXJ2ZVNwYWNlO3NldFJhbmdlKGF4aXMpfSk7aWYoc2hvd0dyaWQpe3ZhciBhbGxvY2F0ZWRBeGVzPSQuZ3JlcChheGVzLGZ1bmN0aW9uKGF4aXMpe3JldHVybiBheGlzLnNob3d8fGF4aXMucmVzZXJ2ZVNwYWNlfSk7JC5lYWNoKGFsbG9jYXRlZEF4ZXMsZnVuY3Rpb24oXyxheGlzKXtzZXR1cFRpY2tHZW5lcmF0aW9uKGF4aXMpO3NldFRpY2tzKGF4aXMpO3NuYXBSYW5nZVRvVGlja3MoYXhpcyxheGlzLnRpY2tzKTttZWFzdXJlVGlja0xhYmVscyhheGlzKX0pO2ZvcihpPWFsbG9jYXRlZEF4ZXMubGVuZ3RoLTE7aT49MDstLWkpYWxsb2NhdGVBeGlzQm94Rmlyc3RQaGFzZShhbGxvY2F0ZWRBeGVzW2ldKTthZGp1c3RMYXlvdXRGb3JUaGluZ3NTdGlja2luZ091dCgpOyQuZWFjaChhbGxvY2F0ZWRBeGVzLGZ1bmN0aW9uKF8sYXhpcyl7YWxsb2NhdGVBeGlzQm94U2Vjb25kUGhhc2UoYXhpcyl9KX1wbG90V2lkdGg9c3VyZmFjZS53aWR0aC1wbG90T2Zmc2V0LmxlZnQtcGxvdE9mZnNldC5yaWdodDtwbG90SGVpZ2h0PXN1cmZhY2UuaGVpZ2h0LXBsb3RPZmZzZXQuYm90dG9tLXBsb3RPZmZzZXQudG9wOyQuZWFjaChheGVzLGZ1bmN0aW9uKF8sYXhpcyl7c2V0VHJhbnNmb3JtYXRpb25IZWxwZXJzKGF4aXMpfSk7aWYoc2hvd0dyaWQpe2RyYXdBeGlzTGFiZWxzKCl9aW5zZXJ0TGVnZW5kKCl9ZnVuY3Rpb24gc2V0UmFuZ2UoYXhpcyl7dmFyIG9wdHM9YXhpcy5vcHRpb25zLG1pbj0rKG9wdHMubWluIT1udWxsP29wdHMubWluOmF4aXMuZGF0YW1pbiksbWF4PSsob3B0cy5tYXghPW51bGw/b3B0cy5tYXg6YXhpcy5kYXRhbWF4KSxkZWx0YT1tYXgtbWluO2lmKGRlbHRhPT0wKXt2YXIgd2lkZW49bWF4PT0wPzE6LjAxO2lmKG9wdHMubWluPT1udWxsKW1pbi09d2lkZW47aWYob3B0cy5tYXg9PW51bGx8fG9wdHMubWluIT1udWxsKW1heCs9d2lkZW59ZWxzZXt2YXIgbWFyZ2luPW9wdHMuYXV0b3NjYWxlTWFyZ2luO2lmKG1hcmdpbiE9bnVsbCl7aWYob3B0cy5taW49PW51bGwpe21pbi09ZGVsdGEqbWFyZ2luO2lmKG1pbjwwJiZheGlzLmRhdGFtaW4hPW51bGwmJmF4aXMuZGF0YW1pbj49MCltaW49MH1pZihvcHRzLm1heD09bnVsbCl7bWF4Kz1kZWx0YSptYXJnaW47aWYobWF4PjAmJmF4aXMuZGF0YW1heCE9bnVsbCYmYXhpcy5kYXRhbWF4PD0wKW1heD0wfX19YXhpcy5taW49bWluO2F4aXMubWF4PW1heH1mdW5jdGlvbiBzZXR1cFRpY2tHZW5lcmF0aW9uKGF4aXMpe3ZhciBvcHRzPWF4aXMub3B0aW9uczt2YXIgbm9UaWNrcztpZih0eXBlb2Ygb3B0cy50aWNrcz09XCJudW1iZXJcIiYmb3B0cy50aWNrcz4wKW5vVGlja3M9b3B0cy50aWNrcztlbHNlIG5vVGlja3M9LjMqTWF0aC5zcXJ0KGF4aXMuZGlyZWN0aW9uPT1cInhcIj9zdXJmYWNlLndpZHRoOnN1cmZhY2UuaGVpZ2h0KTt2YXIgZGVsdGE9KGF4aXMubWF4LWF4aXMubWluKS9ub1RpY2tzLGRlYz0tTWF0aC5mbG9vcihNYXRoLmxvZyhkZWx0YSkvTWF0aC5MTjEwKSxtYXhEZWM9b3B0cy50aWNrRGVjaW1hbHM7aWYobWF4RGVjIT1udWxsJiZkZWM+bWF4RGVjKXtkZWM9bWF4RGVjfXZhciBtYWduPU1hdGgucG93KDEwLC1kZWMpLG5vcm09ZGVsdGEvbWFnbixzaXplO2lmKG5vcm08MS41KXtzaXplPTF9ZWxzZSBpZihub3JtPDMpe3NpemU9MjtpZihub3JtPjIuMjUmJihtYXhEZWM9PW51bGx8fGRlYysxPD1tYXhEZWMpKXtzaXplPTIuNTsrK2RlY319ZWxzZSBpZihub3JtPDcuNSl7c2l6ZT01fWVsc2V7c2l6ZT0xMH1zaXplKj1tYWduO2lmKG9wdHMubWluVGlja1NpemUhPW51bGwmJnNpemU8b3B0cy5taW5UaWNrU2l6ZSl7c2l6ZT1vcHRzLm1pblRpY2tTaXplfWF4aXMuZGVsdGE9ZGVsdGE7YXhpcy50aWNrRGVjaW1hbHM9TWF0aC5tYXgoMCxtYXhEZWMhPW51bGw/bWF4RGVjOmRlYyk7YXhpcy50aWNrU2l6ZT1vcHRzLnRpY2tTaXplfHxzaXplO2lmKG9wdHMubW9kZT09XCJ0aW1lXCImJiFheGlzLnRpY2tHZW5lcmF0b3Ipe3Rocm93IG5ldyBFcnJvcihcIlRpbWUgbW9kZSByZXF1aXJlcyB0aGUgZmxvdC50aW1lIHBsdWdpbi5cIil9aWYoIWF4aXMudGlja0dlbmVyYXRvcil7YXhpcy50aWNrR2VuZXJhdG9yPWZ1bmN0aW9uKGF4aXMpe3ZhciB0aWNrcz1bXSxzdGFydD1mbG9vckluQmFzZShheGlzLm1pbixheGlzLnRpY2tTaXplKSxpPTAsdj1OdW1iZXIuTmFOLHByZXY7ZG97cHJldj12O3Y9c3RhcnQraSpheGlzLnRpY2tTaXplO3RpY2tzLnB1c2godik7KytpfXdoaWxlKHY8YXhpcy5tYXgmJnYhPXByZXYpO3JldHVybiB0aWNrc307YXhpcy50aWNrRm9ybWF0dGVyPWZ1bmN0aW9uKHZhbHVlLGF4aXMpe3ZhciBmYWN0b3I9YXhpcy50aWNrRGVjaW1hbHM/TWF0aC5wb3coMTAsYXhpcy50aWNrRGVjaW1hbHMpOjE7dmFyIGZvcm1hdHRlZD1cIlwiK01hdGgucm91bmQodmFsdWUqZmFjdG9yKS9mYWN0b3I7aWYoYXhpcy50aWNrRGVjaW1hbHMhPW51bGwpe3ZhciBkZWNpbWFsPWZvcm1hdHRlZC5pbmRleE9mKFwiLlwiKTt2YXIgcHJlY2lzaW9uPWRlY2ltYWw9PS0xPzA6Zm9ybWF0dGVkLmxlbmd0aC1kZWNpbWFsLTE7aWYocHJlY2lzaW9uPGF4aXMudGlja0RlY2ltYWxzKXtyZXR1cm4ocHJlY2lzaW9uP2Zvcm1hdHRlZDpmb3JtYXR0ZWQrXCIuXCIpKyhcIlwiK2ZhY3Rvcikuc3Vic3RyKDEsYXhpcy50aWNrRGVjaW1hbHMtcHJlY2lzaW9uKX19cmV0dXJuIGZvcm1hdHRlZH19aWYoJC5pc0Z1bmN0aW9uKG9wdHMudGlja0Zvcm1hdHRlcikpYXhpcy50aWNrRm9ybWF0dGVyPWZ1bmN0aW9uKHYsYXhpcyl7cmV0dXJuXCJcIitvcHRzLnRpY2tGb3JtYXR0ZXIodixheGlzKX07aWYob3B0cy5hbGlnblRpY2tzV2l0aEF4aXMhPW51bGwpe3ZhciBvdGhlckF4aXM9KGF4aXMuZGlyZWN0aW9uPT1cInhcIj94YXhlczp5YXhlcylbb3B0cy5hbGlnblRpY2tzV2l0aEF4aXMtMV07aWYob3RoZXJBeGlzJiZvdGhlckF4aXMudXNlZCYmb3RoZXJBeGlzIT1heGlzKXt2YXIgbmljZVRpY2tzPWF4aXMudGlja0dlbmVyYXRvcihheGlzKTtpZihuaWNlVGlja3MubGVuZ3RoPjApe2lmKG9wdHMubWluPT1udWxsKWF4aXMubWluPU1hdGgubWluKGF4aXMubWluLG5pY2VUaWNrc1swXSk7aWYob3B0cy5tYXg9PW51bGwmJm5pY2VUaWNrcy5sZW5ndGg+MSlheGlzLm1heD1NYXRoLm1heChheGlzLm1heCxuaWNlVGlja3NbbmljZVRpY2tzLmxlbmd0aC0xXSl9YXhpcy50aWNrR2VuZXJhdG9yPWZ1bmN0aW9uKGF4aXMpe3ZhciB0aWNrcz1bXSx2LGk7Zm9yKGk9MDtpPG90aGVyQXhpcy50aWNrcy5sZW5ndGg7KytpKXt2PShvdGhlckF4aXMudGlja3NbaV0udi1vdGhlckF4aXMubWluKS8ob3RoZXJBeGlzLm1heC1vdGhlckF4aXMubWluKTt2PWF4aXMubWluK3YqKGF4aXMubWF4LWF4aXMubWluKTt0aWNrcy5wdXNoKHYpfXJldHVybiB0aWNrc307aWYoIWF4aXMubW9kZSYmb3B0cy50aWNrRGVjaW1hbHM9PW51bGwpe3ZhciBleHRyYURlYz1NYXRoLm1heCgwLC1NYXRoLmZsb29yKE1hdGgubG9nKGF4aXMuZGVsdGEpL01hdGguTE4xMCkrMSksdHM9YXhpcy50aWNrR2VuZXJhdG9yKGF4aXMpO2lmKCEodHMubGVuZ3RoPjEmJi9cXC4uKjAkLy50ZXN0KCh0c1sxXS10c1swXSkudG9GaXhlZChleHRyYURlYykpKSlheGlzLnRpY2tEZWNpbWFscz1leHRyYURlY319fX1mdW5jdGlvbiBzZXRUaWNrcyhheGlzKXt2YXIgb3RpY2tzPWF4aXMub3B0aW9ucy50aWNrcyx0aWNrcz1bXTtpZihvdGlja3M9PW51bGx8fHR5cGVvZiBvdGlja3M9PVwibnVtYmVyXCImJm90aWNrcz4wKXRpY2tzPWF4aXMudGlja0dlbmVyYXRvcihheGlzKTtlbHNlIGlmKG90aWNrcyl7aWYoJC5pc0Z1bmN0aW9uKG90aWNrcykpdGlja3M9b3RpY2tzKGF4aXMpO2Vsc2UgdGlja3M9b3RpY2tzfXZhciBpLHY7YXhpcy50aWNrcz1bXTtmb3IoaT0wO2k8dGlja3MubGVuZ3RoOysraSl7dmFyIGxhYmVsPW51bGw7dmFyIHQ9dGlja3NbaV07aWYodHlwZW9mIHQ9PVwib2JqZWN0XCIpe3Y9K3RbMF07aWYodC5sZW5ndGg+MSlsYWJlbD10WzFdfWVsc2Ugdj0rdDtpZihsYWJlbD09bnVsbClsYWJlbD1heGlzLnRpY2tGb3JtYXR0ZXIodixheGlzKTtpZighaXNOYU4odikpYXhpcy50aWNrcy5wdXNoKHt2OnYsbGFiZWw6bGFiZWx9KX19ZnVuY3Rpb24gc25hcFJhbmdlVG9UaWNrcyhheGlzLHRpY2tzKXtpZihheGlzLm9wdGlvbnMuYXV0b3NjYWxlTWFyZ2luJiZ0aWNrcy5sZW5ndGg+MCl7aWYoYXhpcy5vcHRpb25zLm1pbj09bnVsbClheGlzLm1pbj1NYXRoLm1pbihheGlzLm1pbix0aWNrc1swXS52KTtpZihheGlzLm9wdGlvbnMubWF4PT1udWxsJiZ0aWNrcy5sZW5ndGg+MSlheGlzLm1heD1NYXRoLm1heChheGlzLm1heCx0aWNrc1t0aWNrcy5sZW5ndGgtMV0udil9fWZ1bmN0aW9uIGRyYXcoKXtzdXJmYWNlLmNsZWFyKCk7ZXhlY3V0ZUhvb2tzKGhvb2tzLmRyYXdCYWNrZ3JvdW5kLFtjdHhdKTt2YXIgZ3JpZD1vcHRpb25zLmdyaWQ7aWYoZ3JpZC5zaG93JiZncmlkLmJhY2tncm91bmRDb2xvcilkcmF3QmFja2dyb3VuZCgpO2lmKGdyaWQuc2hvdyYmIWdyaWQuYWJvdmVEYXRhKXtkcmF3R3JpZCgpfWZvcih2YXIgaT0wO2k8c2VyaWVzLmxlbmd0aDsrK2kpe2V4ZWN1dGVIb29rcyhob29rcy5kcmF3U2VyaWVzLFtjdHgsc2VyaWVzW2ldXSk7ZHJhd1NlcmllcyhzZXJpZXNbaV0pfWV4ZWN1dGVIb29rcyhob29rcy5kcmF3LFtjdHhdKTtpZihncmlkLnNob3cmJmdyaWQuYWJvdmVEYXRhKXtkcmF3R3JpZCgpfXN1cmZhY2UucmVuZGVyKCk7dHJpZ2dlclJlZHJhd092ZXJsYXkoKX1mdW5jdGlvbiBleHRyYWN0UmFuZ2UocmFuZ2VzLGNvb3JkKXt2YXIgYXhpcyxmcm9tLHRvLGtleSxheGVzPWFsbEF4ZXMoKTtmb3IodmFyIGk9MDtpPGF4ZXMubGVuZ3RoOysraSl7YXhpcz1heGVzW2ldO2lmKGF4aXMuZGlyZWN0aW9uPT1jb29yZCl7a2V5PWNvb3JkK2F4aXMubitcImF4aXNcIjtpZighcmFuZ2VzW2tleV0mJmF4aXMubj09MSlrZXk9Y29vcmQrXCJheGlzXCI7aWYocmFuZ2VzW2tleV0pe2Zyb209cmFuZ2VzW2tleV0uZnJvbTt0bz1yYW5nZXNba2V5XS50bzticmVha319fWlmKCFyYW5nZXNba2V5XSl7YXhpcz1jb29yZD09XCJ4XCI/eGF4ZXNbMF06eWF4ZXNbMF07ZnJvbT1yYW5nZXNbY29vcmQrXCIxXCJdO3RvPXJhbmdlc1tjb29yZCtcIjJcIl19aWYoZnJvbSE9bnVsbCYmdG8hPW51bGwmJmZyb20+dG8pe3ZhciB0bXA9ZnJvbTtmcm9tPXRvO3RvPXRtcH1yZXR1cm57ZnJvbTpmcm9tLHRvOnRvLGF4aXM6YXhpc319ZnVuY3Rpb24gZHJhd0JhY2tncm91bmQoKXtjdHguc2F2ZSgpO2N0eC50cmFuc2xhdGUocGxvdE9mZnNldC5sZWZ0LHBsb3RPZmZzZXQudG9wKTtjdHguZmlsbFN0eWxlPWdldENvbG9yT3JHcmFkaWVudChvcHRpb25zLmdyaWQuYmFja2dyb3VuZENvbG9yLHBsb3RIZWlnaHQsMCxcInJnYmEoMjU1LCAyNTUsIDI1NSwgMClcIik7Y3R4LmZpbGxSZWN0KDAsMCxwbG90V2lkdGgscGxvdEhlaWdodCk7Y3R4LnJlc3RvcmUoKX1mdW5jdGlvbiBkcmF3R3JpZCgpe3ZhciBpLGF4ZXMsYncsYmM7Y3R4LnNhdmUoKTtjdHgudHJhbnNsYXRlKHBsb3RPZmZzZXQubGVmdCxwbG90T2Zmc2V0LnRvcCk7dmFyIG1hcmtpbmdzPW9wdGlvbnMuZ3JpZC5tYXJraW5ncztpZihtYXJraW5ncyl7aWYoJC5pc0Z1bmN0aW9uKG1hcmtpbmdzKSl7YXhlcz1wbG90LmdldEF4ZXMoKTtheGVzLnhtaW49YXhlcy54YXhpcy5taW47YXhlcy54bWF4PWF4ZXMueGF4aXMubWF4O2F4ZXMueW1pbj1heGVzLnlheGlzLm1pbjtheGVzLnltYXg9YXhlcy55YXhpcy5tYXg7bWFya2luZ3M9bWFya2luZ3MoYXhlcyl9Zm9yKGk9MDtpPG1hcmtpbmdzLmxlbmd0aDsrK2kpe3ZhciBtPW1hcmtpbmdzW2ldLHhyYW5nZT1leHRyYWN0UmFuZ2UobSxcInhcIikseXJhbmdlPWV4dHJhY3RSYW5nZShtLFwieVwiKTtpZih4cmFuZ2UuZnJvbT09bnVsbCl4cmFuZ2UuZnJvbT14cmFuZ2UuYXhpcy5taW47aWYoeHJhbmdlLnRvPT1udWxsKXhyYW5nZS50bz14cmFuZ2UuYXhpcy5tYXg7XG5pZih5cmFuZ2UuZnJvbT09bnVsbCl5cmFuZ2UuZnJvbT15cmFuZ2UuYXhpcy5taW47aWYoeXJhbmdlLnRvPT1udWxsKXlyYW5nZS50bz15cmFuZ2UuYXhpcy5tYXg7aWYoeHJhbmdlLnRvPHhyYW5nZS5heGlzLm1pbnx8eHJhbmdlLmZyb20+eHJhbmdlLmF4aXMubWF4fHx5cmFuZ2UudG88eXJhbmdlLmF4aXMubWlufHx5cmFuZ2UuZnJvbT55cmFuZ2UuYXhpcy5tYXgpY29udGludWU7eHJhbmdlLmZyb209TWF0aC5tYXgoeHJhbmdlLmZyb20seHJhbmdlLmF4aXMubWluKTt4cmFuZ2UudG89TWF0aC5taW4oeHJhbmdlLnRvLHhyYW5nZS5heGlzLm1heCk7eXJhbmdlLmZyb209TWF0aC5tYXgoeXJhbmdlLmZyb20seXJhbmdlLmF4aXMubWluKTt5cmFuZ2UudG89TWF0aC5taW4oeXJhbmdlLnRvLHlyYW5nZS5heGlzLm1heCk7dmFyIHhlcXVhbD14cmFuZ2UuZnJvbT09PXhyYW5nZS50byx5ZXF1YWw9eXJhbmdlLmZyb209PT15cmFuZ2UudG87aWYoeGVxdWFsJiZ5ZXF1YWwpe2NvbnRpbnVlfXhyYW5nZS5mcm9tPU1hdGguZmxvb3IoeHJhbmdlLmF4aXMucDJjKHhyYW5nZS5mcm9tKSk7eHJhbmdlLnRvPU1hdGguZmxvb3IoeHJhbmdlLmF4aXMucDJjKHhyYW5nZS50bykpO3lyYW5nZS5mcm9tPU1hdGguZmxvb3IoeXJhbmdlLmF4aXMucDJjKHlyYW5nZS5mcm9tKSk7eXJhbmdlLnRvPU1hdGguZmxvb3IoeXJhbmdlLmF4aXMucDJjKHlyYW5nZS50bykpO2lmKHhlcXVhbHx8eWVxdWFsKXt2YXIgbGluZVdpZHRoPW0ubGluZVdpZHRofHxvcHRpb25zLmdyaWQubWFya2luZ3NMaW5lV2lkdGgsc3ViUGl4ZWw9bGluZVdpZHRoJTI/LjU6MDtjdHguYmVnaW5QYXRoKCk7Y3R4LnN0cm9rZVN0eWxlPW0uY29sb3J8fG9wdGlvbnMuZ3JpZC5tYXJraW5nc0NvbG9yO2N0eC5saW5lV2lkdGg9bGluZVdpZHRoO2lmKHhlcXVhbCl7Y3R4Lm1vdmVUbyh4cmFuZ2UudG8rc3ViUGl4ZWwseXJhbmdlLmZyb20pO2N0eC5saW5lVG8oeHJhbmdlLnRvK3N1YlBpeGVsLHlyYW5nZS50byl9ZWxzZXtjdHgubW92ZVRvKHhyYW5nZS5mcm9tLHlyYW5nZS50bytzdWJQaXhlbCk7Y3R4LmxpbmVUbyh4cmFuZ2UudG8seXJhbmdlLnRvK3N1YlBpeGVsKX1jdHguc3Ryb2tlKCl9ZWxzZXtjdHguZmlsbFN0eWxlPW0uY29sb3J8fG9wdGlvbnMuZ3JpZC5tYXJraW5nc0NvbG9yO2N0eC5maWxsUmVjdCh4cmFuZ2UuZnJvbSx5cmFuZ2UudG8seHJhbmdlLnRvLXhyYW5nZS5mcm9tLHlyYW5nZS5mcm9tLXlyYW5nZS50byl9fX1heGVzPWFsbEF4ZXMoKTtidz1vcHRpb25zLmdyaWQuYm9yZGVyV2lkdGg7Zm9yKHZhciBqPTA7ajxheGVzLmxlbmd0aDsrK2ope3ZhciBheGlzPWF4ZXNbal0sYm94PWF4aXMuYm94LHQ9YXhpcy50aWNrTGVuZ3RoLHgseSx4b2ZmLHlvZmY7aWYoIWF4aXMuc2hvd3x8YXhpcy50aWNrcy5sZW5ndGg9PTApY29udGludWU7Y3R4LmxpbmVXaWR0aD0xO2lmKGF4aXMuZGlyZWN0aW9uPT1cInhcIil7eD0wO2lmKHQ9PVwiZnVsbFwiKXk9YXhpcy5wb3NpdGlvbj09XCJ0b3BcIj8wOnBsb3RIZWlnaHQ7ZWxzZSB5PWJveC50b3AtcGxvdE9mZnNldC50b3ArKGF4aXMucG9zaXRpb249PVwidG9wXCI/Ym94LmhlaWdodDowKX1lbHNle3k9MDtpZih0PT1cImZ1bGxcIil4PWF4aXMucG9zaXRpb249PVwibGVmdFwiPzA6cGxvdFdpZHRoO2Vsc2UgeD1ib3gubGVmdC1wbG90T2Zmc2V0LmxlZnQrKGF4aXMucG9zaXRpb249PVwibGVmdFwiP2JveC53aWR0aDowKX1pZighYXhpcy5pbm5lcm1vc3Qpe2N0eC5zdHJva2VTdHlsZT1heGlzLm9wdGlvbnMuY29sb3I7Y3R4LmJlZ2luUGF0aCgpO3hvZmY9eW9mZj0wO2lmKGF4aXMuZGlyZWN0aW9uPT1cInhcIil4b2ZmPXBsb3RXaWR0aCsxO2Vsc2UgeW9mZj1wbG90SGVpZ2h0KzE7aWYoY3R4LmxpbmVXaWR0aD09MSl7aWYoYXhpcy5kaXJlY3Rpb249PVwieFwiKXt5PU1hdGguZmxvb3IoeSkrLjV9ZWxzZXt4PU1hdGguZmxvb3IoeCkrLjV9fWN0eC5tb3ZlVG8oeCx5KTtjdHgubGluZVRvKHgreG9mZix5K3lvZmYpO2N0eC5zdHJva2UoKX1jdHguc3Ryb2tlU3R5bGU9YXhpcy5vcHRpb25zLnRpY2tDb2xvcjtjdHguYmVnaW5QYXRoKCk7Zm9yKGk9MDtpPGF4aXMudGlja3MubGVuZ3RoOysraSl7dmFyIHY9YXhpcy50aWNrc1tpXS52O3hvZmY9eW9mZj0wO2lmKGlzTmFOKHYpfHx2PGF4aXMubWlufHx2PmF4aXMubWF4fHx0PT1cImZ1bGxcIiYmKHR5cGVvZiBidz09XCJvYmplY3RcIiYmYndbYXhpcy5wb3NpdGlvbl0+MHx8Ync+MCkmJih2PT1heGlzLm1pbnx8dj09YXhpcy5tYXgpKWNvbnRpbnVlO2lmKGF4aXMuZGlyZWN0aW9uPT1cInhcIil7eD1heGlzLnAyYyh2KTt5b2ZmPXQ9PVwiZnVsbFwiPy1wbG90SGVpZ2h0OnQ7aWYoYXhpcy5wb3NpdGlvbj09XCJ0b3BcIil5b2ZmPS15b2ZmfWVsc2V7eT1heGlzLnAyYyh2KTt4b2ZmPXQ9PVwiZnVsbFwiPy1wbG90V2lkdGg6dDtpZihheGlzLnBvc2l0aW9uPT1cImxlZnRcIil4b2ZmPS14b2ZmfWlmKGN0eC5saW5lV2lkdGg9PTEpe2lmKGF4aXMuZGlyZWN0aW9uPT1cInhcIil4PU1hdGguZmxvb3IoeCkrLjU7ZWxzZSB5PU1hdGguZmxvb3IoeSkrLjV9Y3R4Lm1vdmVUbyh4LHkpO2N0eC5saW5lVG8oeCt4b2ZmLHkreW9mZil9Y3R4LnN0cm9rZSgpfWlmKGJ3KXtiYz1vcHRpb25zLmdyaWQuYm9yZGVyQ29sb3I7aWYodHlwZW9mIGJ3PT1cIm9iamVjdFwifHx0eXBlb2YgYmM9PVwib2JqZWN0XCIpe2lmKHR5cGVvZiBidyE9PVwib2JqZWN0XCIpe2J3PXt0b3A6YncscmlnaHQ6YncsYm90dG9tOmJ3LGxlZnQ6Ynd9fWlmKHR5cGVvZiBiYyE9PVwib2JqZWN0XCIpe2JjPXt0b3A6YmMscmlnaHQ6YmMsYm90dG9tOmJjLGxlZnQ6YmN9fWlmKGJ3LnRvcD4wKXtjdHguc3Ryb2tlU3R5bGU9YmMudG9wO2N0eC5saW5lV2lkdGg9YncudG9wO2N0eC5iZWdpblBhdGgoKTtjdHgubW92ZVRvKDAtYncubGVmdCwwLWJ3LnRvcC8yKTtjdHgubGluZVRvKHBsb3RXaWR0aCwwLWJ3LnRvcC8yKTtjdHguc3Ryb2tlKCl9aWYoYncucmlnaHQ+MCl7Y3R4LnN0cm9rZVN0eWxlPWJjLnJpZ2h0O2N0eC5saW5lV2lkdGg9YncucmlnaHQ7Y3R4LmJlZ2luUGF0aCgpO2N0eC5tb3ZlVG8ocGxvdFdpZHRoK2J3LnJpZ2h0LzIsMC1idy50b3ApO2N0eC5saW5lVG8ocGxvdFdpZHRoK2J3LnJpZ2h0LzIscGxvdEhlaWdodCk7Y3R4LnN0cm9rZSgpfWlmKGJ3LmJvdHRvbT4wKXtjdHguc3Ryb2tlU3R5bGU9YmMuYm90dG9tO2N0eC5saW5lV2lkdGg9YncuYm90dG9tO2N0eC5iZWdpblBhdGgoKTtjdHgubW92ZVRvKHBsb3RXaWR0aCtidy5yaWdodCxwbG90SGVpZ2h0K2J3LmJvdHRvbS8yKTtjdHgubGluZVRvKDAscGxvdEhlaWdodCtidy5ib3R0b20vMik7Y3R4LnN0cm9rZSgpfWlmKGJ3LmxlZnQ+MCl7Y3R4LnN0cm9rZVN0eWxlPWJjLmxlZnQ7Y3R4LmxpbmVXaWR0aD1idy5sZWZ0O2N0eC5iZWdpblBhdGgoKTtjdHgubW92ZVRvKDAtYncubGVmdC8yLHBsb3RIZWlnaHQrYncuYm90dG9tKTtjdHgubGluZVRvKDAtYncubGVmdC8yLDApO2N0eC5zdHJva2UoKX19ZWxzZXtjdHgubGluZVdpZHRoPWJ3O2N0eC5zdHJva2VTdHlsZT1vcHRpb25zLmdyaWQuYm9yZGVyQ29sb3I7Y3R4LnN0cm9rZVJlY3QoLWJ3LzIsLWJ3LzIscGxvdFdpZHRoK2J3LHBsb3RIZWlnaHQrYncpfX1jdHgucmVzdG9yZSgpfWZ1bmN0aW9uIGRyYXdBeGlzTGFiZWxzKCl7JC5lYWNoKGFsbEF4ZXMoKSxmdW5jdGlvbihfLGF4aXMpe3ZhciBib3g9YXhpcy5ib3gsbGVnYWN5U3R5bGVzPWF4aXMuZGlyZWN0aW9uK1wiQXhpcyBcIitheGlzLmRpcmVjdGlvbitheGlzLm4rXCJBeGlzXCIsbGF5ZXI9XCJmbG90LVwiK2F4aXMuZGlyZWN0aW9uK1wiLWF4aXMgZmxvdC1cIitheGlzLmRpcmVjdGlvbitheGlzLm4rXCItYXhpcyBcIitsZWdhY3lTdHlsZXMsZm9udD1heGlzLm9wdGlvbnMuZm9udHx8XCJmbG90LXRpY2stbGFiZWwgdGlja0xhYmVsXCIsdGljayx4LHksaGFsaWduLHZhbGlnbjtzdXJmYWNlLnJlbW92ZVRleHQobGF5ZXIpO2lmKCFheGlzLnNob3d8fGF4aXMudGlja3MubGVuZ3RoPT0wKXJldHVybjtmb3IodmFyIGk9MDtpPGF4aXMudGlja3MubGVuZ3RoOysraSl7dGljaz1heGlzLnRpY2tzW2ldO2lmKCF0aWNrLmxhYmVsfHx0aWNrLnY8YXhpcy5taW58fHRpY2sudj5heGlzLm1heCljb250aW51ZTtpZihheGlzLmRpcmVjdGlvbj09XCJ4XCIpe2hhbGlnbj1cImNlbnRlclwiO3g9cGxvdE9mZnNldC5sZWZ0K2F4aXMucDJjKHRpY2sudik7aWYoYXhpcy5wb3NpdGlvbj09XCJib3R0b21cIil7eT1ib3gudG9wK2JveC5wYWRkaW5nfWVsc2V7eT1ib3gudG9wK2JveC5oZWlnaHQtYm94LnBhZGRpbmc7dmFsaWduPVwiYm90dG9tXCJ9fWVsc2V7dmFsaWduPVwibWlkZGxlXCI7eT1wbG90T2Zmc2V0LnRvcCtheGlzLnAyYyh0aWNrLnYpO2lmKGF4aXMucG9zaXRpb249PVwibGVmdFwiKXt4PWJveC5sZWZ0K2JveC53aWR0aC1ib3gucGFkZGluZztoYWxpZ249XCJyaWdodFwifWVsc2V7eD1ib3gubGVmdCtib3gucGFkZGluZ319c3VyZmFjZS5hZGRUZXh0KGxheWVyLHgseSx0aWNrLmxhYmVsLGZvbnQsbnVsbCxudWxsLGhhbGlnbix2YWxpZ24pfX0pfWZ1bmN0aW9uIGRyYXdTZXJpZXMoc2VyaWVzKXtpZihzZXJpZXMubGluZXMuc2hvdylkcmF3U2VyaWVzTGluZXMoc2VyaWVzKTtpZihzZXJpZXMuYmFycy5zaG93KWRyYXdTZXJpZXNCYXJzKHNlcmllcyk7aWYoc2VyaWVzLnBvaW50cy5zaG93KWRyYXdTZXJpZXNQb2ludHMoc2VyaWVzKX1mdW5jdGlvbiBkcmF3U2VyaWVzTGluZXMoc2VyaWVzKXtmdW5jdGlvbiBwbG90TGluZShkYXRhcG9pbnRzLHhvZmZzZXQseW9mZnNldCxheGlzeCxheGlzeSl7dmFyIHBvaW50cz1kYXRhcG9pbnRzLnBvaW50cyxwcz1kYXRhcG9pbnRzLnBvaW50c2l6ZSxwcmV2eD1udWxsLHByZXZ5PW51bGw7Y3R4LmJlZ2luUGF0aCgpO2Zvcih2YXIgaT1wcztpPHBvaW50cy5sZW5ndGg7aSs9cHMpe3ZhciB4MT1wb2ludHNbaS1wc10seTE9cG9pbnRzW2ktcHMrMV0seDI9cG9pbnRzW2ldLHkyPXBvaW50c1tpKzFdO2lmKHgxPT1udWxsfHx4Mj09bnVsbCljb250aW51ZTtpZih5MTw9eTImJnkxPGF4aXN5Lm1pbil7aWYoeTI8YXhpc3kubWluKWNvbnRpbnVlO3gxPShheGlzeS5taW4teTEpLyh5Mi15MSkqKHgyLXgxKSt4MTt5MT1heGlzeS5taW59ZWxzZSBpZih5Mjw9eTEmJnkyPGF4aXN5Lm1pbil7aWYoeTE8YXhpc3kubWluKWNvbnRpbnVlO3gyPShheGlzeS5taW4teTEpLyh5Mi15MSkqKHgyLXgxKSt4MTt5Mj1heGlzeS5taW59aWYoeTE+PXkyJiZ5MT5heGlzeS5tYXgpe2lmKHkyPmF4aXN5Lm1heCljb250aW51ZTt4MT0oYXhpc3kubWF4LXkxKS8oeTIteTEpKih4Mi14MSkreDE7eTE9YXhpc3kubWF4fWVsc2UgaWYoeTI+PXkxJiZ5Mj5heGlzeS5tYXgpe2lmKHkxPmF4aXN5Lm1heCljb250aW51ZTt4Mj0oYXhpc3kubWF4LXkxKS8oeTIteTEpKih4Mi14MSkreDE7eTI9YXhpc3kubWF4fWlmKHgxPD14MiYmeDE8YXhpc3gubWluKXtpZih4MjxheGlzeC5taW4pY29udGludWU7eTE9KGF4aXN4Lm1pbi14MSkvKHgyLXgxKSooeTIteTEpK3kxO3gxPWF4aXN4Lm1pbn1lbHNlIGlmKHgyPD14MSYmeDI8YXhpc3gubWluKXtpZih4MTxheGlzeC5taW4pY29udGludWU7eTI9KGF4aXN4Lm1pbi14MSkvKHgyLXgxKSooeTIteTEpK3kxO3gyPWF4aXN4Lm1pbn1pZih4MT49eDImJngxPmF4aXN4Lm1heCl7aWYoeDI+YXhpc3gubWF4KWNvbnRpbnVlO3kxPShheGlzeC5tYXgteDEpLyh4Mi14MSkqKHkyLXkxKSt5MTt4MT1heGlzeC5tYXh9ZWxzZSBpZih4Mj49eDEmJngyPmF4aXN4Lm1heCl7aWYoeDE+YXhpc3gubWF4KWNvbnRpbnVlO3kyPShheGlzeC5tYXgteDEpLyh4Mi14MSkqKHkyLXkxKSt5MTt4Mj1heGlzeC5tYXh9aWYoeDEhPXByZXZ4fHx5MSE9cHJldnkpY3R4Lm1vdmVUbyhheGlzeC5wMmMoeDEpK3hvZmZzZXQsYXhpc3kucDJjKHkxKSt5b2Zmc2V0KTtwcmV2eD14MjtwcmV2eT15MjtjdHgubGluZVRvKGF4aXN4LnAyYyh4MikreG9mZnNldCxheGlzeS5wMmMoeTIpK3lvZmZzZXQpfWN0eC5zdHJva2UoKX1mdW5jdGlvbiBwbG90TGluZUFyZWEoZGF0YXBvaW50cyxheGlzeCxheGlzeSl7dmFyIHBvaW50cz1kYXRhcG9pbnRzLnBvaW50cyxwcz1kYXRhcG9pbnRzLnBvaW50c2l6ZSxib3R0b209TWF0aC5taW4oTWF0aC5tYXgoMCxheGlzeS5taW4pLGF4aXN5Lm1heCksaT0wLHRvcCxhcmVhT3Blbj1mYWxzZSx5cG9zPTEsc2VnbWVudFN0YXJ0PTAsc2VnbWVudEVuZD0wO3doaWxlKHRydWUpe2lmKHBzPjAmJmk+cG9pbnRzLmxlbmd0aCtwcylicmVhaztpKz1wczt2YXIgeDE9cG9pbnRzW2ktcHNdLHkxPXBvaW50c1tpLXBzK3lwb3NdLHgyPXBvaW50c1tpXSx5Mj1wb2ludHNbaSt5cG9zXTtpZihhcmVhT3Blbil7aWYocHM+MCYmeDEhPW51bGwmJngyPT1udWxsKXtzZWdtZW50RW5kPWk7cHM9LXBzO3lwb3M9Mjtjb250aW51ZX1pZihwczwwJiZpPT1zZWdtZW50U3RhcnQrcHMpe2N0eC5maWxsKCk7YXJlYU9wZW49ZmFsc2U7cHM9LXBzO3lwb3M9MTtpPXNlZ21lbnRTdGFydD1zZWdtZW50RW5kK3BzO2NvbnRpbnVlfX1pZih4MT09bnVsbHx8eDI9PW51bGwpY29udGludWU7aWYoeDE8PXgyJiZ4MTxheGlzeC5taW4pe2lmKHgyPGF4aXN4Lm1pbiljb250aW51ZTt5MT0oYXhpc3gubWluLXgxKS8oeDIteDEpKih5Mi15MSkreTE7eDE9YXhpc3gubWlufWVsc2UgaWYoeDI8PXgxJiZ4MjxheGlzeC5taW4pe2lmKHgxPGF4aXN4Lm1pbiljb250aW51ZTt5Mj0oYXhpc3gubWluLXgxKS8oeDIteDEpKih5Mi15MSkreTE7eDI9YXhpc3gubWlufWlmKHgxPj14MiYmeDE+YXhpc3gubWF4KXtpZih4Mj5heGlzeC5tYXgpY29udGludWU7eTE9KGF4aXN4Lm1heC14MSkvKHgyLXgxKSooeTIteTEpK3kxO3gxPWF4aXN4Lm1heH1lbHNlIGlmKHgyPj14MSYmeDI+YXhpc3gubWF4KXtpZih4MT5heGlzeC5tYXgpY29udGludWU7eTI9KGF4aXN4Lm1heC14MSkvKHgyLXgxKSooeTIteTEpK3kxO3gyPWF4aXN4Lm1heH1pZighYXJlYU9wZW4pe2N0eC5iZWdpblBhdGgoKTtjdHgubW92ZVRvKGF4aXN4LnAyYyh4MSksYXhpc3kucDJjKGJvdHRvbSkpO2FyZWFPcGVuPXRydWV9aWYoeTE+PWF4aXN5Lm1heCYmeTI+PWF4aXN5Lm1heCl7Y3R4LmxpbmVUbyhheGlzeC5wMmMoeDEpLGF4aXN5LnAyYyhheGlzeS5tYXgpKTtjdHgubGluZVRvKGF4aXN4LnAyYyh4MiksYXhpc3kucDJjKGF4aXN5Lm1heCkpO2NvbnRpbnVlfWVsc2UgaWYoeTE8PWF4aXN5Lm1pbiYmeTI8PWF4aXN5Lm1pbil7Y3R4LmxpbmVUbyhheGlzeC5wMmMoeDEpLGF4aXN5LnAyYyhheGlzeS5taW4pKTtjdHgubGluZVRvKGF4aXN4LnAyYyh4MiksYXhpc3kucDJjKGF4aXN5Lm1pbikpO2NvbnRpbnVlfXZhciB4MW9sZD14MSx4Mm9sZD14MjtpZih5MTw9eTImJnkxPGF4aXN5Lm1pbiYmeTI+PWF4aXN5Lm1pbil7eDE9KGF4aXN5Lm1pbi15MSkvKHkyLXkxKSooeDIteDEpK3gxO3kxPWF4aXN5Lm1pbn1lbHNlIGlmKHkyPD15MSYmeTI8YXhpc3kubWluJiZ5MT49YXhpc3kubWluKXt4Mj0oYXhpc3kubWluLXkxKS8oeTIteTEpKih4Mi14MSkreDE7eTI9YXhpc3kubWlufWlmKHkxPj15MiYmeTE+YXhpc3kubWF4JiZ5Mjw9YXhpc3kubWF4KXt4MT0oYXhpc3kubWF4LXkxKS8oeTIteTEpKih4Mi14MSkreDE7eTE9YXhpc3kubWF4fWVsc2UgaWYoeTI+PXkxJiZ5Mj5heGlzeS5tYXgmJnkxPD1heGlzeS5tYXgpe3gyPShheGlzeS5tYXgteTEpLyh5Mi15MSkqKHgyLXgxKSt4MTt5Mj1heGlzeS5tYXh9aWYoeDEhPXgxb2xkKXtjdHgubGluZVRvKGF4aXN4LnAyYyh4MW9sZCksYXhpc3kucDJjKHkxKSl9Y3R4LmxpbmVUbyhheGlzeC5wMmMoeDEpLGF4aXN5LnAyYyh5MSkpO2N0eC5saW5lVG8oYXhpc3gucDJjKHgyKSxheGlzeS5wMmMoeTIpKTtpZih4MiE9eDJvbGQpe2N0eC5saW5lVG8oYXhpc3gucDJjKHgyKSxheGlzeS5wMmMoeTIpKTtjdHgubGluZVRvKGF4aXN4LnAyYyh4Mm9sZCksYXhpc3kucDJjKHkyKSl9fX1jdHguc2F2ZSgpO2N0eC50cmFuc2xhdGUocGxvdE9mZnNldC5sZWZ0LHBsb3RPZmZzZXQudG9wKTtjdHgubGluZUpvaW49XCJyb3VuZFwiO3ZhciBsdz1zZXJpZXMubGluZXMubGluZVdpZHRoLHN3PXNlcmllcy5zaGFkb3dTaXplO2lmKGx3PjAmJnN3PjApe2N0eC5saW5lV2lkdGg9c3c7Y3R4LnN0cm9rZVN0eWxlPVwicmdiYSgwLDAsMCwwLjEpXCI7dmFyIGFuZ2xlPU1hdGguUEkvMTg7cGxvdExpbmUoc2VyaWVzLmRhdGFwb2ludHMsTWF0aC5zaW4oYW5nbGUpKihsdy8yK3N3LzIpLE1hdGguY29zKGFuZ2xlKSoobHcvMitzdy8yKSxzZXJpZXMueGF4aXMsc2VyaWVzLnlheGlzKTtjdHgubGluZVdpZHRoPXN3LzI7cGxvdExpbmUoc2VyaWVzLmRhdGFwb2ludHMsTWF0aC5zaW4oYW5nbGUpKihsdy8yK3N3LzQpLE1hdGguY29zKGFuZ2xlKSoobHcvMitzdy80KSxzZXJpZXMueGF4aXMsc2VyaWVzLnlheGlzKX1jdHgubGluZVdpZHRoPWx3O2N0eC5zdHJva2VTdHlsZT1zZXJpZXMuY29sb3I7dmFyIGZpbGxTdHlsZT1nZXRGaWxsU3R5bGUoc2VyaWVzLmxpbmVzLHNlcmllcy5jb2xvciwwLHBsb3RIZWlnaHQpO2lmKGZpbGxTdHlsZSl7Y3R4LmZpbGxTdHlsZT1maWxsU3R5bGU7cGxvdExpbmVBcmVhKHNlcmllcy5kYXRhcG9pbnRzLHNlcmllcy54YXhpcyxzZXJpZXMueWF4aXMpfWlmKGx3PjApcGxvdExpbmUoc2VyaWVzLmRhdGFwb2ludHMsMCwwLHNlcmllcy54YXhpcyxzZXJpZXMueWF4aXMpO2N0eC5yZXN0b3JlKCl9ZnVuY3Rpb24gZHJhd1Nlcmllc1BvaW50cyhzZXJpZXMpe2Z1bmN0aW9uIHBsb3RQb2ludHMoZGF0YXBvaW50cyxyYWRpdXMsZmlsbFN0eWxlLG9mZnNldCxzaGFkb3csYXhpc3gsYXhpc3ksc3ltYm9sKXt2YXIgcG9pbnRzPWRhdGFwb2ludHMucG9pbnRzLHBzPWRhdGFwb2ludHMucG9pbnRzaXplO2Zvcih2YXIgaT0wO2k8cG9pbnRzLmxlbmd0aDtpKz1wcyl7dmFyIHg9cG9pbnRzW2ldLHk9cG9pbnRzW2krMV07aWYoeD09bnVsbHx8eDxheGlzeC5taW58fHg+YXhpc3gubWF4fHx5PGF4aXN5Lm1pbnx8eT5heGlzeS5tYXgpY29udGludWU7Y3R4LmJlZ2luUGF0aCgpO3g9YXhpc3gucDJjKHgpO3k9YXhpc3kucDJjKHkpK29mZnNldDtpZihzeW1ib2w9PVwiY2lyY2xlXCIpY3R4LmFyYyh4LHkscmFkaXVzLDAsc2hhZG93P01hdGguUEk6TWF0aC5QSSoyLGZhbHNlKTtlbHNlIHN5bWJvbChjdHgseCx5LHJhZGl1cyxzaGFkb3cpO2N0eC5jbG9zZVBhdGgoKTtpZihmaWxsU3R5bGUpe2N0eC5maWxsU3R5bGU9ZmlsbFN0eWxlO2N0eC5maWxsKCl9Y3R4LnN0cm9rZSgpfX1jdHguc2F2ZSgpO2N0eC50cmFuc2xhdGUocGxvdE9mZnNldC5sZWZ0LHBsb3RPZmZzZXQudG9wKTt2YXIgbHc9c2VyaWVzLnBvaW50cy5saW5lV2lkdGgsc3c9c2VyaWVzLnNoYWRvd1NpemUscmFkaXVzPXNlcmllcy5wb2ludHMucmFkaXVzLHN5bWJvbD1zZXJpZXMucG9pbnRzLnN5bWJvbDtpZihsdz09MClsdz0xZS00O2lmKGx3PjAmJnN3PjApe3ZhciB3PXN3LzI7Y3R4LmxpbmVXaWR0aD13O2N0eC5zdHJva2VTdHlsZT1cInJnYmEoMCwwLDAsMC4xKVwiO3Bsb3RQb2ludHMoc2VyaWVzLmRhdGFwb2ludHMscmFkaXVzLG51bGwsdyt3LzIsdHJ1ZSxzZXJpZXMueGF4aXMsc2VyaWVzLnlheGlzLHN5bWJvbCk7Y3R4LnN0cm9rZVN0eWxlPVwicmdiYSgwLDAsMCwwLjIpXCI7cGxvdFBvaW50cyhzZXJpZXMuZGF0YXBvaW50cyxyYWRpdXMsbnVsbCx3LzIsdHJ1ZSxzZXJpZXMueGF4aXMsc2VyaWVzLnlheGlzLHN5bWJvbCl9Y3R4LmxpbmVXaWR0aD1sdztjdHguc3Ryb2tlU3R5bGU9c2VyaWVzLmNvbG9yO3Bsb3RQb2ludHMoc2VyaWVzLmRhdGFwb2ludHMscmFkaXVzLGdldEZpbGxTdHlsZShzZXJpZXMucG9pbnRzLHNlcmllcy5jb2xvciksMCxmYWxzZSxzZXJpZXMueGF4aXMsc2VyaWVzLnlheGlzLHN5bWJvbCk7Y3R4LnJlc3RvcmUoKX1mdW5jdGlvbiBkcmF3QmFyKHgseSxiLGJhckxlZnQsYmFyUmlnaHQsZmlsbFN0eWxlQ2FsbGJhY2ssYXhpc3gsYXhpc3ksYyxob3Jpem9udGFsLGxpbmVXaWR0aCl7dmFyIGxlZnQscmlnaHQsYm90dG9tLHRvcCxkcmF3TGVmdCxkcmF3UmlnaHQsZHJhd1RvcCxkcmF3Qm90dG9tLHRtcDtpZihob3Jpem9udGFsKXtkcmF3Qm90dG9tPWRyYXdSaWdodD1kcmF3VG9wPXRydWU7ZHJhd0xlZnQ9ZmFsc2U7bGVmdD1iO3JpZ2h0PXg7dG9wPXkrYmFyTGVmdDtib3R0b209eStiYXJSaWdodDtpZihyaWdodDxsZWZ0KXt0bXA9cmlnaHQ7cmlnaHQ9bGVmdDtsZWZ0PXRtcDtkcmF3TGVmdD10cnVlO2RyYXdSaWdodD1mYWxzZX19ZWxzZXtkcmF3TGVmdD1kcmF3UmlnaHQ9ZHJhd1RvcD10cnVlO2RyYXdCb3R0b209ZmFsc2U7bGVmdD14K2JhckxlZnQ7cmlnaHQ9eCtiYXJSaWdodDtib3R0b209Yjt0b3A9eTtpZih0b3A8Ym90dG9tKXt0bXA9dG9wO3RvcD1ib3R0b207Ym90dG9tPXRtcDtkcmF3Qm90dG9tPXRydWU7ZHJhd1RvcD1mYWxzZX19aWYocmlnaHQ8YXhpc3gubWlufHxsZWZ0PmF4aXN4Lm1heHx8dG9wPGF4aXN5Lm1pbnx8Ym90dG9tPmF4aXN5Lm1heClyZXR1cm47aWYobGVmdDxheGlzeC5taW4pe2xlZnQ9YXhpc3gubWluO2RyYXdMZWZ0PWZhbHNlfWlmKHJpZ2h0PmF4aXN4Lm1heCl7cmlnaHQ9YXhpc3gubWF4O2RyYXdSaWdodD1mYWxzZX1pZihib3R0b208YXhpc3kubWluKXtib3R0b209YXhpc3kubWluO2RyYXdCb3R0b209ZmFsc2V9aWYodG9wPmF4aXN5Lm1heCl7dG9wPWF4aXN5Lm1heDtkcmF3VG9wPWZhbHNlfWxlZnQ9YXhpc3gucDJjKGxlZnQpO2JvdHRvbT1heGlzeS5wMmMoYm90dG9tKTtyaWdodD1heGlzeC5wMmMocmlnaHQpO3RvcD1heGlzeS5wMmModG9wKTtpZihmaWxsU3R5bGVDYWxsYmFjayl7Yy5maWxsU3R5bGU9ZmlsbFN0eWxlQ2FsbGJhY2soYm90dG9tLHRvcCk7Yy5maWxsUmVjdChsZWZ0LHRvcCxyaWdodC1sZWZ0LGJvdHRvbS10b3ApfWlmKGxpbmVXaWR0aD4wJiYoZHJhd0xlZnR8fGRyYXdSaWdodHx8ZHJhd1RvcHx8ZHJhd0JvdHRvbSkpe2MuYmVnaW5QYXRoKCk7Yy5tb3ZlVG8obGVmdCxib3R0b20pO2lmKGRyYXdMZWZ0KWMubGluZVRvKGxlZnQsdG9wKTtlbHNlIGMubW92ZVRvKGxlZnQsdG9wKTtpZihkcmF3VG9wKWMubGluZVRvKHJpZ2h0LHRvcCk7ZWxzZSBjLm1vdmVUbyhyaWdodCx0b3ApO2lmKGRyYXdSaWdodCljLmxpbmVUbyhyaWdodCxib3R0b20pO2Vsc2UgYy5tb3ZlVG8ocmlnaHQsYm90dG9tKTtpZihkcmF3Qm90dG9tKWMubGluZVRvKGxlZnQsYm90dG9tKTtlbHNlIGMubW92ZVRvKGxlZnQsYm90dG9tKTtjLnN0cm9rZSgpfX1mdW5jdGlvbiBkcmF3U2VyaWVzQmFycyhzZXJpZXMpe2Z1bmN0aW9uIHBsb3RCYXJzKGRhdGFwb2ludHMsYmFyTGVmdCxiYXJSaWdodCxmaWxsU3R5bGVDYWxsYmFjayxheGlzeCxheGlzeSl7dmFyIHBvaW50cz1kYXRhcG9pbnRzLnBvaW50cyxwcz1kYXRhcG9pbnRzLnBvaW50c2l6ZTtmb3IodmFyIGk9MDtpPHBvaW50cy5sZW5ndGg7aSs9cHMpe2lmKHBvaW50c1tpXT09bnVsbCljb250aW51ZTtkcmF3QmFyKHBvaW50c1tpXSxwb2ludHNbaSsxXSxwb2ludHNbaSsyXSxiYXJMZWZ0LGJhclJpZ2h0LGZpbGxTdHlsZUNhbGxiYWNrLGF4aXN4LGF4aXN5LGN0eCxzZXJpZXMuYmFycy5ob3Jpem9udGFsLHNlcmllcy5iYXJzLmxpbmVXaWR0aCl9fWN0eC5zYXZlKCk7Y3R4LnRyYW5zbGF0ZShwbG90T2Zmc2V0LmxlZnQscGxvdE9mZnNldC50b3ApO2N0eC5saW5lV2lkdGg9c2VyaWVzLmJhcnMubGluZVdpZHRoO2N0eC5zdHJva2VTdHlsZT1zZXJpZXMuY29sb3I7dmFyIGJhckxlZnQ7c3dpdGNoKHNlcmllcy5iYXJzLmFsaWduKXtjYXNlXCJsZWZ0XCI6YmFyTGVmdD0wO2JyZWFrO2Nhc2VcInJpZ2h0XCI6YmFyTGVmdD0tc2VyaWVzLmJhcnMuYmFyV2lkdGg7YnJlYWs7ZGVmYXVsdDpiYXJMZWZ0PS1zZXJpZXMuYmFycy5iYXJXaWR0aC8yfXZhciBmaWxsU3R5bGVDYWxsYmFjaz1zZXJpZXMuYmFycy5maWxsP2Z1bmN0aW9uKGJvdHRvbSx0b3Ape3JldHVybiBnZXRGaWxsU3R5bGUoc2VyaWVzLmJhcnMsc2VyaWVzLmNvbG9yLGJvdHRvbSx0b3ApfTpudWxsO3Bsb3RCYXJzKHNlcmllcy5kYXRhcG9pbnRzLGJhckxlZnQsYmFyTGVmdCtzZXJpZXMuYmFycy5iYXJXaWR0aCxmaWxsU3R5bGVDYWxsYmFjayxzZXJpZXMueGF4aXMsc2VyaWVzLnlheGlzKTtjdHgucmVzdG9yZSgpfWZ1bmN0aW9uIGdldEZpbGxTdHlsZShmaWxsb3B0aW9ucyxzZXJpZXNDb2xvcixib3R0b20sdG9wKXt2YXIgZmlsbD1maWxsb3B0aW9ucy5maWxsO2lmKCFmaWxsKXJldHVybiBudWxsO2lmKGZpbGxvcHRpb25zLmZpbGxDb2xvcilyZXR1cm4gZ2V0Q29sb3JPckdyYWRpZW50KGZpbGxvcHRpb25zLmZpbGxDb2xvcixib3R0b20sdG9wLHNlcmllc0NvbG9yKTt2YXIgYz0kLmNvbG9yLnBhcnNlKHNlcmllc0NvbG9yKTtjLmE9dHlwZW9mIGZpbGw9PVwibnVtYmVyXCI/ZmlsbDouNDtjLm5vcm1hbGl6ZSgpO3JldHVybiBjLnRvU3RyaW5nKCl9ZnVuY3Rpb24gaW5zZXJ0TGVnZW5kKCl7aWYob3B0aW9ucy5sZWdlbmQuY29udGFpbmVyIT1udWxsKXskKG9wdGlvbnMubGVnZW5kLmNvbnRhaW5lcikuaHRtbChcIlwiKX1lbHNle3BsYWNlaG9sZGVyLmZpbmQoXCIubGVnZW5kXCIpLnJlbW92ZSgpfWlmKCFvcHRpb25zLmxlZ2VuZC5zaG93KXtyZXR1cm59dmFyIGZyYWdtZW50cz1bXSxlbnRyaWVzPVtdLHJvd1N0YXJ0ZWQ9ZmFsc2UsbGY9b3B0aW9ucy5sZWdlbmQubGFiZWxGb3JtYXR0ZXIscyxsYWJlbDtmb3IodmFyIGk9MDtpPHNlcmllcy5sZW5ndGg7KytpKXtzPXNlcmllc1tpXTtpZihzLmxhYmVsKXtsYWJlbD1sZj9sZihzLmxhYmVsLHMpOnMubGFiZWw7aWYobGFiZWwpe2VudHJpZXMucHVzaCh7bGFiZWw6bGFiZWwsY29sb3I6cy5jb2xvcn0pfX19aWYob3B0aW9ucy5sZWdlbmQuc29ydGVkKXtpZigkLmlzRnVuY3Rpb24ob3B0aW9ucy5sZWdlbmQuc29ydGVkKSl7ZW50cmllcy5zb3J0KG9wdGlvbnMubGVnZW5kLnNvcnRlZCl9ZWxzZSBpZihvcHRpb25zLmxlZ2VuZC5zb3J0ZWQ9PVwicmV2ZXJzZVwiKXtlbnRyaWVzLnJldmVyc2UoKX1lbHNle3ZhciBhc2NlbmRpbmc9b3B0aW9ucy5sZWdlbmQuc29ydGVkIT1cImRlc2NlbmRpbmdcIjtlbnRyaWVzLnNvcnQoZnVuY3Rpb24oYSxiKXtyZXR1cm4gYS5sYWJlbD09Yi5sYWJlbD8wOmEubGFiZWw8Yi5sYWJlbCE9YXNjZW5kaW5nPzE6LTF9KX19Zm9yKHZhciBpPTA7aTxlbnRyaWVzLmxlbmd0aDsrK2kpe3ZhciBlbnRyeT1lbnRyaWVzW2ldO2lmKGklb3B0aW9ucy5sZWdlbmQubm9Db2x1bW5zPT0wKXtpZihyb3dTdGFydGVkKWZyYWdtZW50cy5wdXNoKFwiPC90cj5cIik7ZnJhZ21lbnRzLnB1c2goXCI8dHI+XCIpO3Jvd1N0YXJ0ZWQ9dHJ1ZX1mcmFnbWVudHMucHVzaCgnPHRkIGNsYXNzPVwibGVnZW5kQ29sb3JCb3hcIj48ZGl2IHN0eWxlPVwiYm9yZGVyOjFweCBzb2xpZCAnK29wdGlvbnMubGVnZW5kLmxhYmVsQm94Qm9yZGVyQ29sb3IrJztwYWRkaW5nOjFweFwiPjxkaXYgc3R5bGU9XCJ3aWR0aDo0cHg7aGVpZ2h0OjA7Ym9yZGVyOjVweCBzb2xpZCAnK2VudHJ5LmNvbG9yKyc7b3ZlcmZsb3c6aGlkZGVuXCI+PC9kaXY+PC9kaXY+PC90ZD4nKyc8dGQgY2xhc3M9XCJsZWdlbmRMYWJlbFwiPicrZW50cnkubGFiZWwrXCI8L3RkPlwiKX1pZihyb3dTdGFydGVkKWZyYWdtZW50cy5wdXNoKFwiPC90cj5cIik7aWYoZnJhZ21lbnRzLmxlbmd0aD09MClyZXR1cm47dmFyIHRhYmxlPSc8dGFibGUgc3R5bGU9XCJmb250LXNpemU6c21hbGxlcjtjb2xvcjonK29wdGlvbnMuZ3JpZC5jb2xvcisnXCI+JytmcmFnbWVudHMuam9pbihcIlwiKStcIjwvdGFibGU+XCI7aWYob3B0aW9ucy5sZWdlbmQuY29udGFpbmVyIT1udWxsKSQob3B0aW9ucy5sZWdlbmQuY29udGFpbmVyKS5odG1sKHRhYmxlKTtlbHNle3ZhciBwb3M9XCJcIixwPW9wdGlvbnMubGVnZW5kLnBvc2l0aW9uLG09b3B0aW9ucy5sZWdlbmQubWFyZ2luO2lmKG1bMF09PW51bGwpbT1bbSxtXTtpZihwLmNoYXJBdCgwKT09XCJuXCIpcG9zKz1cInRvcDpcIisobVsxXStwbG90T2Zmc2V0LnRvcCkrXCJweDtcIjtlbHNlIGlmKHAuY2hhckF0KDApPT1cInNcIilwb3MrPVwiYm90dG9tOlwiKyhtWzFdK3Bsb3RPZmZzZXQuYm90dG9tKStcInB4O1wiO2lmKHAuY2hhckF0KDEpPT1cImVcIilwb3MrPVwicmlnaHQ6XCIrKG1bMF0rcGxvdE9mZnNldC5yaWdodCkrXCJweDtcIjtlbHNlIGlmKHAuY2hhckF0KDEpPT1cIndcIilwb3MrPVwibGVmdDpcIisobVswXStwbG90T2Zmc2V0LmxlZnQpK1wicHg7XCI7dmFyIGxlZ2VuZD0kKCc8ZGl2IGNsYXNzPVwibGVnZW5kXCI+Jyt0YWJsZS5yZXBsYWNlKCdzdHlsZT1cIicsJ3N0eWxlPVwicG9zaXRpb246YWJzb2x1dGU7Jytwb3MrXCI7XCIpK1wiPC9kaXY+XCIpLmFwcGVuZFRvKHBsYWNlaG9sZGVyKTtpZihvcHRpb25zLmxlZ2VuZC5iYWNrZ3JvdW5kT3BhY2l0eSE9MCl7dmFyIGM9b3B0aW9ucy5sZWdlbmQuYmFja2dyb3VuZENvbG9yO2lmKGM9PW51bGwpe2M9b3B0aW9ucy5ncmlkLmJhY2tncm91bmRDb2xvcjtpZihjJiZ0eXBlb2YgYz09XCJzdHJpbmdcIiljPSQuY29sb3IucGFyc2UoYyk7ZWxzZSBjPSQuY29sb3IuZXh0cmFjdChsZWdlbmQsXCJiYWNrZ3JvdW5kLWNvbG9yXCIpO2MuYT0xO2M9Yy50b1N0cmluZygpfXZhciBkaXY9bGVnZW5kLmNoaWxkcmVuKCk7JCgnPGRpdiBzdHlsZT1cInBvc2l0aW9uOmFic29sdXRlO3dpZHRoOicrZGl2LndpZHRoKCkrXCJweDtoZWlnaHQ6XCIrZGl2LmhlaWdodCgpK1wicHg7XCIrcG9zK1wiYmFja2dyb3VuZC1jb2xvcjpcIitjKyc7XCI+IDwvZGl2PicpLnByZXBlbmRUbyhsZWdlbmQpLmNzcyhcIm9wYWNpdHlcIixvcHRpb25zLmxlZ2VuZC5iYWNrZ3JvdW5kT3BhY2l0eSl9fX12YXIgaGlnaGxpZ2h0cz1bXSxyZWRyYXdUaW1lb3V0PW51bGw7ZnVuY3Rpb24gZmluZE5lYXJieUl0ZW0obW91c2VYLG1vdXNlWSxzZXJpZXNGaWx0ZXIpe3ZhciBtYXhEaXN0YW5jZT1vcHRpb25zLmdyaWQubW91c2VBY3RpdmVSYWRpdXMsc21hbGxlc3REaXN0YW5jZT1tYXhEaXN0YW5jZSptYXhEaXN0YW5jZSsxLGl0ZW09bnVsbCxmb3VuZFBvaW50PWZhbHNlLGksaixwcztmb3IoaT1zZXJpZXMubGVuZ3RoLTE7aT49MDstLWkpe2lmKCFzZXJpZXNGaWx0ZXIoc2VyaWVzW2ldKSljb250aW51ZTt2YXIgcz1zZXJpZXNbaV0sYXhpc3g9cy54YXhpcyxheGlzeT1zLnlheGlzLHBvaW50cz1zLmRhdGFwb2ludHMucG9pbnRzLG14PWF4aXN4LmMycChtb3VzZVgpLG15PWF4aXN5LmMycChtb3VzZVkpLG1heHg9bWF4RGlzdGFuY2UvYXhpc3guc2NhbGUsbWF4eT1tYXhEaXN0YW5jZS9heGlzeS5zY2FsZTtwcz1zLmRhdGFwb2ludHMucG9pbnRzaXplO2lmKGF4aXN4Lm9wdGlvbnMuaW52ZXJzZVRyYW5zZm9ybSltYXh4PU51bWJlci5NQVhfVkFMVUU7aWYoYXhpc3kub3B0aW9ucy5pbnZlcnNlVHJhbnNmb3JtKW1heHk9TnVtYmVyLk1BWF9WQUxVRTtpZihzLmxpbmVzLnNob3d8fHMucG9pbnRzLnNob3cpe2ZvcihqPTA7ajxwb2ludHMubGVuZ3RoO2orPXBzKXt2YXIgeD1wb2ludHNbal0seT1wb2ludHNbaisxXTtpZih4PT1udWxsKWNvbnRpbnVlO2lmKHgtbXg+bWF4eHx8eC1teDwtbWF4eHx8eS1teT5tYXh5fHx5LW15PC1tYXh5KWNvbnRpbnVlO3ZhciBkeD1NYXRoLmFicyhheGlzeC5wMmMoeCktbW91c2VYKSxkeT1NYXRoLmFicyhheGlzeS5wMmMoeSktbW91c2VZKSxkaXN0PWR4KmR4K2R5KmR5O2lmKGRpc3Q8c21hbGxlc3REaXN0YW5jZSl7c21hbGxlc3REaXN0YW5jZT1kaXN0O2l0ZW09W2ksai9wc119fX1pZihzLmJhcnMuc2hvdyYmIWl0ZW0pe3ZhciBiYXJMZWZ0LGJhclJpZ2h0O3N3aXRjaChzLmJhcnMuYWxpZ24pe2Nhc2VcImxlZnRcIjpiYXJMZWZ0PTA7YnJlYWs7Y2FzZVwicmlnaHRcIjpiYXJMZWZ0PS1zLmJhcnMuYmFyV2lkdGg7YnJlYWs7ZGVmYXVsdDpiYXJMZWZ0PS1zLmJhcnMuYmFyV2lkdGgvMn1iYXJSaWdodD1iYXJMZWZ0K3MuYmFycy5iYXJXaWR0aDtmb3Ioaj0wO2o8cG9pbnRzLmxlbmd0aDtqKz1wcyl7dmFyIHg9cG9pbnRzW2pdLHk9cG9pbnRzW2orMV0sYj1wb2ludHNbaisyXTtpZih4PT1udWxsKWNvbnRpbnVlO2lmKHNlcmllc1tpXS5iYXJzLmhvcml6b250YWw/bXg8PU1hdGgubWF4KGIseCkmJm14Pj1NYXRoLm1pbihiLHgpJiZteT49eStiYXJMZWZ0JiZteTw9eStiYXJSaWdodDpteD49eCtiYXJMZWZ0JiZteDw9eCtiYXJSaWdodCYmbXk+PU1hdGgubWluKGIseSkmJm15PD1NYXRoLm1heChiLHkpKWl0ZW09W2ksai9wc119fX1pZihpdGVtKXtpPWl0ZW1bMF07aj1pdGVtWzFdO3BzPXNlcmllc1tpXS5kYXRhcG9pbnRzLnBvaW50c2l6ZTtyZXR1cm57ZGF0YXBvaW50OnNlcmllc1tpXS5kYXRhcG9pbnRzLnBvaW50cy5zbGljZShqKnBzLChqKzEpKnBzKSxkYXRhSW5kZXg6aixzZXJpZXM6c2VyaWVzW2ldLHNlcmllc0luZGV4Oml9fXJldHVybiBudWxsfWZ1bmN0aW9uIG9uTW91c2VNb3ZlKGUpe2lmKG9wdGlvbnMuZ3JpZC5ob3ZlcmFibGUpdHJpZ2dlckNsaWNrSG92ZXJFdmVudChcInBsb3Rob3ZlclwiLGUsZnVuY3Rpb24ocyl7cmV0dXJuIHNbXCJob3ZlcmFibGVcIl0hPWZhbHNlfSl9ZnVuY3Rpb24gb25Nb3VzZUxlYXZlKGUpe2lmKG9wdGlvbnMuZ3JpZC5ob3ZlcmFibGUpdHJpZ2dlckNsaWNrSG92ZXJFdmVudChcInBsb3Rob3ZlclwiLGUsZnVuY3Rpb24ocyl7cmV0dXJuIGZhbHNlfSl9ZnVuY3Rpb24gb25DbGljayhlKXt0cmlnZ2VyQ2xpY2tIb3ZlckV2ZW50KFwicGxvdGNsaWNrXCIsZSxmdW5jdGlvbihzKXtyZXR1cm4gc1tcImNsaWNrYWJsZVwiXSE9ZmFsc2V9KX1mdW5jdGlvbiB0cmlnZ2VyQ2xpY2tIb3ZlckV2ZW50KGV2ZW50bmFtZSxldmVudCxzZXJpZXNGaWx0ZXIpe3ZhciBvZmZzZXQ9ZXZlbnRIb2xkZXIub2Zmc2V0KCksY2FudmFzWD1ldmVudC5wYWdlWC1vZmZzZXQubGVmdC1wbG90T2Zmc2V0LmxlZnQsY2FudmFzWT1ldmVudC5wYWdlWS1vZmZzZXQudG9wLXBsb3RPZmZzZXQudG9wLHBvcz1jYW52YXNUb0F4aXNDb29yZHMoe2xlZnQ6Y2FudmFzWCx0b3A6Y2FudmFzWX0pO3Bvcy5wYWdlWD1ldmVudC5wYWdlWDtwb3MucGFnZVk9ZXZlbnQucGFnZVk7dmFyIGl0ZW09ZmluZE5lYXJieUl0ZW0oY2FudmFzWCxjYW52YXNZLHNlcmllc0ZpbHRlcik7aWYoaXRlbSl7aXRlbS5wYWdlWD1wYXJzZUludChpdGVtLnNlcmllcy54YXhpcy5wMmMoaXRlbS5kYXRhcG9pbnRbMF0pK29mZnNldC5sZWZ0K3Bsb3RPZmZzZXQubGVmdCwxMCk7aXRlbS5wYWdlWT1wYXJzZUludChpdGVtLnNlcmllcy55YXhpcy5wMmMoaXRlbS5kYXRhcG9pbnRbMV0pK29mZnNldC50b3ArcGxvdE9mZnNldC50b3AsMTApfWlmKG9wdGlvbnMuZ3JpZC5hdXRvSGlnaGxpZ2h0KXtmb3IodmFyIGk9MDtpPGhpZ2hsaWdodHMubGVuZ3RoOysraSl7dmFyIGg9aGlnaGxpZ2h0c1tpXTtpZihoLmF1dG89PWV2ZW50bmFtZSYmIShpdGVtJiZoLnNlcmllcz09aXRlbS5zZXJpZXMmJmgucG9pbnRbMF09PWl0ZW0uZGF0YXBvaW50WzBdJiZoLnBvaW50WzFdPT1pdGVtLmRhdGFwb2ludFsxXSkpdW5oaWdobGlnaHQoaC5zZXJpZXMsaC5wb2ludCl9aWYoaXRlbSloaWdobGlnaHQoaXRlbS5zZXJpZXMsaXRlbS5kYXRhcG9pbnQsZXZlbnRuYW1lKX1wbGFjZWhvbGRlci50cmlnZ2VyKGV2ZW50bmFtZSxbcG9zLGl0ZW1dKX1mdW5jdGlvbiB0cmlnZ2VyUmVkcmF3T3ZlcmxheSgpe3ZhciB0PW9wdGlvbnMuaW50ZXJhY3Rpb24ucmVkcmF3T3ZlcmxheUludGVydmFsO2lmKHQ9PS0xKXtkcmF3T3ZlcmxheSgpO3JldHVybn1pZighcmVkcmF3VGltZW91dClyZWRyYXdUaW1lb3V0PXNldFRpbWVvdXQoZHJhd092ZXJsYXksdCl9ZnVuY3Rpb24gZHJhd092ZXJsYXkoKXtyZWRyYXdUaW1lb3V0PW51bGw7b2N0eC5zYXZlKCk7b3ZlcmxheS5jbGVhcigpO29jdHgudHJhbnNsYXRlKHBsb3RPZmZzZXQubGVmdCxwbG90T2Zmc2V0LnRvcCk7dmFyIGksaGk7Zm9yKGk9MDtpPGhpZ2hsaWdodHMubGVuZ3RoOysraSl7aGk9aGlnaGxpZ2h0c1tpXTtpZihoaS5zZXJpZXMuYmFycy5zaG93KWRyYXdCYXJIaWdobGlnaHQoaGkuc2VyaWVzLGhpLnBvaW50KTtlbHNlIGRyYXdQb2ludEhpZ2hsaWdodChoaS5zZXJpZXMsaGkucG9pbnQpfW9jdHgucmVzdG9yZSgpO2V4ZWN1dGVIb29rcyhob29rcy5kcmF3T3ZlcmxheSxbb2N0eF0pfWZ1bmN0aW9uIGhpZ2hsaWdodChzLHBvaW50LGF1dG8pe2lmKHR5cGVvZiBzPT1cIm51bWJlclwiKXM9c2VyaWVzW3NdO2lmKHR5cGVvZiBwb2ludD09XCJudW1iZXJcIil7dmFyIHBzPXMuZGF0YXBvaW50cy5wb2ludHNpemU7cG9pbnQ9cy5kYXRhcG9pbnRzLnBvaW50cy5zbGljZShwcypwb2ludCxwcyoocG9pbnQrMSkpfXZhciBpPWluZGV4T2ZIaWdobGlnaHQocyxwb2ludCk7aWYoaT09LTEpe2hpZ2hsaWdodHMucHVzaCh7c2VyaWVzOnMscG9pbnQ6cG9pbnQsYXV0bzphdXRvfSk7dHJpZ2dlclJlZHJhd092ZXJsYXkoKX1lbHNlIGlmKCFhdXRvKWhpZ2hsaWdodHNbaV0uYXV0bz1mYWxzZX1mdW5jdGlvbiB1bmhpZ2hsaWdodChzLHBvaW50KXtpZihzPT1udWxsJiZwb2ludD09bnVsbCl7aGlnaGxpZ2h0cz1bXTt0cmlnZ2VyUmVkcmF3T3ZlcmxheSgpO3JldHVybn1pZih0eXBlb2Ygcz09XCJudW1iZXJcIilzPXNlcmllc1tzXTtpZih0eXBlb2YgcG9pbnQ9PVwibnVtYmVyXCIpe3ZhciBwcz1zLmRhdGFwb2ludHMucG9pbnRzaXplO3BvaW50PXMuZGF0YXBvaW50cy5wb2ludHMuc2xpY2UocHMqcG9pbnQscHMqKHBvaW50KzEpKX12YXIgaT1pbmRleE9mSGlnaGxpZ2h0KHMscG9pbnQpO2lmKGkhPS0xKXtoaWdobGlnaHRzLnNwbGljZShpLDEpO3RyaWdnZXJSZWRyYXdPdmVybGF5KCl9fWZ1bmN0aW9uIGluZGV4T2ZIaWdobGlnaHQocyxwKXtmb3IodmFyIGk9MDtpPGhpZ2hsaWdodHMubGVuZ3RoOysraSl7dmFyIGg9aGlnaGxpZ2h0c1tpXTtpZihoLnNlcmllcz09cyYmaC5wb2ludFswXT09cFswXSYmaC5wb2ludFsxXT09cFsxXSlyZXR1cm4gaX1yZXR1cm4tMX1mdW5jdGlvbiBkcmF3UG9pbnRIaWdobGlnaHQoc2VyaWVzLHBvaW50KXt2YXIgeD1wb2ludFswXSx5PXBvaW50WzFdLGF4aXN4PXNlcmllcy54YXhpcyxheGlzeT1zZXJpZXMueWF4aXMsaGlnaGxpZ2h0Q29sb3I9dHlwZW9mIHNlcmllcy5oaWdobGlnaHRDb2xvcj09PVwic3RyaW5nXCI/c2VyaWVzLmhpZ2hsaWdodENvbG9yOiQuY29sb3IucGFyc2Uoc2VyaWVzLmNvbG9yKS5zY2FsZShcImFcIiwuNSkudG9TdHJpbmcoKTtpZih4PGF4aXN4Lm1pbnx8eD5heGlzeC5tYXh8fHk8YXhpc3kubWlufHx5PmF4aXN5Lm1heClyZXR1cm47dmFyIHBvaW50UmFkaXVzPXNlcmllcy5wb2ludHMucmFkaXVzK3Nlcmllcy5wb2ludHMubGluZVdpZHRoLzI7b2N0eC5saW5lV2lkdGg9cG9pbnRSYWRpdXM7b2N0eC5zdHJva2VTdHlsZT1oaWdobGlnaHRDb2xvcjt2YXIgcmFkaXVzPTEuNSpwb2ludFJhZGl1czt4PWF4aXN4LnAyYyh4KTt5PWF4aXN5LnAyYyh5KTtvY3R4LmJlZ2luUGF0aCgpO2lmKHNlcmllcy5wb2ludHMuc3ltYm9sPT1cImNpcmNsZVwiKW9jdHguYXJjKHgseSxyYWRpdXMsMCwyKk1hdGguUEksZmFsc2UpO2Vsc2Ugc2VyaWVzLnBvaW50cy5zeW1ib2wob2N0eCx4LHkscmFkaXVzLGZhbHNlKTtvY3R4LmNsb3NlUGF0aCgpO29jdHguc3Ryb2tlKCl9ZnVuY3Rpb24gZHJhd0JhckhpZ2hsaWdodChzZXJpZXMscG9pbnQpe3ZhciBoaWdobGlnaHRDb2xvcj10eXBlb2Ygc2VyaWVzLmhpZ2hsaWdodENvbG9yPT09XCJzdHJpbmdcIj9zZXJpZXMuaGlnaGxpZ2h0Q29sb3I6JC5jb2xvci5wYXJzZShzZXJpZXMuY29sb3IpLnNjYWxlKFwiYVwiLC41KS50b1N0cmluZygpLGZpbGxTdHlsZT1oaWdobGlnaHRDb2xvcixiYXJMZWZ0O3N3aXRjaChzZXJpZXMuYmFycy5hbGlnbil7Y2FzZVwibGVmdFwiOmJhckxlZnQ9MDticmVhaztjYXNlXCJyaWdodFwiOmJhckxlZnQ9LXNlcmllcy5iYXJzLmJhcldpZHRoO2JyZWFrO2RlZmF1bHQ6YmFyTGVmdD0tc2VyaWVzLmJhcnMuYmFyV2lkdGgvMn1vY3R4LmxpbmVXaWR0aD1zZXJpZXMuYmFycy5saW5lV2lkdGg7b2N0eC5zdHJva2VTdHlsZT1oaWdobGlnaHRDb2xvcjtkcmF3QmFyKHBvaW50WzBdLHBvaW50WzFdLHBvaW50WzJdfHwwLGJhckxlZnQsYmFyTGVmdCtzZXJpZXMuYmFycy5iYXJXaWR0aCxmdW5jdGlvbigpe3JldHVybiBmaWxsU3R5bGV9LHNlcmllcy54YXhpcyxzZXJpZXMueWF4aXMsb2N0eCxzZXJpZXMuYmFycy5ob3Jpem9udGFsLHNlcmllcy5iYXJzLmxpbmVXaWR0aCl9ZnVuY3Rpb24gZ2V0Q29sb3JPckdyYWRpZW50KHNwZWMsYm90dG9tLHRvcCxkZWZhdWx0Q29sb3Ipe2lmKHR5cGVvZiBzcGVjPT1cInN0cmluZ1wiKXJldHVybiBzcGVjO2Vsc2V7dmFyIGdyYWRpZW50PWN0eC5jcmVhdGVMaW5lYXJHcmFkaWVudCgwLHRvcCwwLGJvdHRvbSk7Zm9yKHZhciBpPTAsbD1zcGVjLmNvbG9ycy5sZW5ndGg7aTxsOysraSl7dmFyIGM9c3BlYy5jb2xvcnNbaV07aWYodHlwZW9mIGMhPVwic3RyaW5nXCIpe3ZhciBjbz0kLmNvbG9yLnBhcnNlKGRlZmF1bHRDb2xvcik7aWYoYy5icmlnaHRuZXNzIT1udWxsKWNvPWNvLnNjYWxlKFwicmdiXCIsYy5icmlnaHRuZXNzKTtpZihjLm9wYWNpdHkhPW51bGwpY28uYSo9Yy5vcGFjaXR5O2M9Y28udG9TdHJpbmcoKX1ncmFkaWVudC5hZGRDb2xvclN0b3AoaS8obC0xKSxjKX1yZXR1cm4gZ3JhZGllbnR9fX0kLnBsb3Q9ZnVuY3Rpb24ocGxhY2Vob2xkZXIsZGF0YSxvcHRpb25zKXt2YXIgcGxvdD1uZXcgUGxvdCgkKHBsYWNlaG9sZGVyKSxkYXRhLG9wdGlvbnMsJC5wbG90LnBsdWdpbnMpO3JldHVybiBwbG90fTskLnBsb3QudmVyc2lvbj1cIjAuOC4zXCI7JC5wbG90LnBsdWdpbnM9W107JC5mbi5wbG90PWZ1bmN0aW9uKGRhdGEsb3B0aW9ucyl7cmV0dXJuIHRoaXMuZWFjaChmdW5jdGlvbigpeyQucGxvdCh0aGlzLGRhdGEsb3B0aW9ucyl9KX07ZnVuY3Rpb24gZmxvb3JJbkJhc2UobixiYXNlKXtyZXR1cm4gYmFzZSpNYXRoLmZsb29yKG4vYmFzZSl9fSkoalF1ZXJ5KTsiLCIvKiBKYXZhc2NyaXB0IHBsb3R0aW5nIGxpYnJhcnkgZm9yIGpRdWVyeSwgdmVyc2lvbiAwLjguMy5cblxuQ29weXJpZ2h0IChjKSAyMDA3LTIwMTQgSU9MQSBhbmQgT2xlIExhdXJzZW4uXG5MaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UuXG5cbiovXG4oZnVuY3Rpb24oYSl7ZnVuY3Rpb24gZShoKXt2YXIgayxqPXRoaXMsbD1oLmRhdGF8fHt9O2lmKGwuZWxlbSlqPWguZHJhZ1RhcmdldD1sLmVsZW0saC5kcmFnUHJveHk9ZC5wcm94eXx8aixoLmN1cnNvck9mZnNldFg9bC5wYWdlWC1sLmxlZnQsaC5jdXJzb3JPZmZzZXRZPWwucGFnZVktbC50b3AsaC5vZmZzZXRYPWgucGFnZVgtaC5jdXJzb3JPZmZzZXRYLGgub2Zmc2V0WT1oLnBhZ2VZLWguY3Vyc29yT2Zmc2V0WTtlbHNlIGlmKGQuZHJhZ2dpbmd8fGwud2hpY2g+MCYmaC53aGljaCE9bC53aGljaHx8YShoLnRhcmdldCkuaXMobC5ub3QpKXJldHVybjtzd2l0Y2goaC50eXBlKXtjYXNlXCJtb3VzZWRvd25cIjpyZXR1cm4gYS5leHRlbmQobCxhKGopLm9mZnNldCgpLHtlbGVtOmosdGFyZ2V0OmgudGFyZ2V0LHBhZ2VYOmgucGFnZVgscGFnZVk6aC5wYWdlWX0pLGIuYWRkKGRvY3VtZW50LFwibW91c2Vtb3ZlIG1vdXNldXBcIixlLGwpLGkoaiwhMSksZC5kcmFnZ2luZz1udWxsLCExO2Nhc2UhZC5kcmFnZ2luZyYmXCJtb3VzZW1vdmVcIjppZihnKGgucGFnZVgtbC5wYWdlWCkrZyhoLnBhZ2VZLWwucGFnZVkpPGwuZGlzdGFuY2UpYnJlYWs7aC50YXJnZXQ9bC50YXJnZXQsaz1mKGgsXCJkcmFnc3RhcnRcIixqKSxrIT09ITEmJihkLmRyYWdnaW5nPWosZC5wcm94eT1oLmRyYWdQcm94eT1hKGt8fGopWzBdKTtjYXNlXCJtb3VzZW1vdmVcIjppZihkLmRyYWdnaW5nKXtpZihrPWYoaCxcImRyYWdcIixqKSxjLmRyb3AmJihjLmRyb3AuYWxsb3dlZD1rIT09ITEsYy5kcm9wLmhhbmRsZXIoaCkpLGshPT0hMSlicmVhaztoLnR5cGU9XCJtb3VzZXVwXCJ9Y2FzZVwibW91c2V1cFwiOmIucmVtb3ZlKGRvY3VtZW50LFwibW91c2Vtb3ZlIG1vdXNldXBcIixlKSxkLmRyYWdnaW5nJiYoYy5kcm9wJiZjLmRyb3AuaGFuZGxlcihoKSxmKGgsXCJkcmFnZW5kXCIsaikpLGkoaiwhMCksZC5kcmFnZ2luZz1kLnByb3h5PWwuZWxlbT0hMX1yZXR1cm4hMH1mdW5jdGlvbiBmKGIsYyxkKXtiLnR5cGU9Yzt2YXIgZT1hLmV2ZW50LmRpc3BhdGNoLmNhbGwoZCxiKTtyZXR1cm4gZT09PSExPyExOmV8fGIucmVzdWx0fWZ1bmN0aW9uIGcoYSl7cmV0dXJuIE1hdGgucG93KGEsMil9ZnVuY3Rpb24gaCgpe3JldHVybiBkLmRyYWdnaW5nPT09ITF9ZnVuY3Rpb24gaShhLGIpe2EmJihhLnVuc2VsZWN0YWJsZT1iP1wib2ZmXCI6XCJvblwiLGEub25zZWxlY3RzdGFydD1mdW5jdGlvbigpe3JldHVybiBifSxhLnN0eWxlJiYoYS5zdHlsZS5Nb3pVc2VyU2VsZWN0PWI/XCJcIjpcIm5vbmVcIikpfWEuZm4uZHJhZz1mdW5jdGlvbihhLGIsYyl7cmV0dXJuIGImJnRoaXMuYmluZChcImRyYWdzdGFydFwiLGEpLGMmJnRoaXMuYmluZChcImRyYWdlbmRcIixjKSxhP3RoaXMuYmluZChcImRyYWdcIixiP2I6YSk6dGhpcy50cmlnZ2VyKFwiZHJhZ1wiKX07dmFyIGI9YS5ldmVudCxjPWIuc3BlY2lhbCxkPWMuZHJhZz17bm90OlwiOmlucHV0XCIsZGlzdGFuY2U6MCx3aGljaDoxLGRyYWdnaW5nOiExLHNldHVwOmZ1bmN0aW9uKGMpe2M9YS5leHRlbmQoe2Rpc3RhbmNlOmQuZGlzdGFuY2Usd2hpY2g6ZC53aGljaCxub3Q6ZC5ub3R9LGN8fHt9KSxjLmRpc3RhbmNlPWcoYy5kaXN0YW5jZSksYi5hZGQodGhpcyxcIm1vdXNlZG93blwiLGUsYyksdGhpcy5hdHRhY2hFdmVudCYmdGhpcy5hdHRhY2hFdmVudChcIm9uZHJhZ3N0YXJ0XCIsaCl9LHRlYXJkb3duOmZ1bmN0aW9uKCl7Yi5yZW1vdmUodGhpcyxcIm1vdXNlZG93blwiLGUpLHRoaXM9PT1kLmRyYWdnaW5nJiYoZC5kcmFnZ2luZz1kLnByb3h5PSExKSxpKHRoaXMsITApLHRoaXMuZGV0YWNoRXZlbnQmJnRoaXMuZGV0YWNoRXZlbnQoXCJvbmRyYWdzdGFydFwiLGgpfX07Yy5kcmFnc3RhcnQ9Yy5kcmFnZW5kPXtzZXR1cDpmdW5jdGlvbigpe30sdGVhcmRvd246ZnVuY3Rpb24oKXt9fX0pKGpRdWVyeSk7KGZ1bmN0aW9uKGQpe2Z1bmN0aW9uIGUoYSl7dmFyIGI9YXx8d2luZG93LmV2ZW50LGM9W10uc2xpY2UuY2FsbChhcmd1bWVudHMsMSksZj0wLGU9MCxnPTAsYT1kLmV2ZW50LmZpeChiKTthLnR5cGU9XCJtb3VzZXdoZWVsXCI7Yi53aGVlbERlbHRhJiYoZj1iLndoZWVsRGVsdGEvMTIwKTtiLmRldGFpbCYmKGY9LWIuZGV0YWlsLzMpO2c9Zjt2b2lkIDAhPT1iLmF4aXMmJmIuYXhpcz09PWIuSE9SSVpPTlRBTF9BWElTJiYoZz0wLGU9LTEqZik7dm9pZCAwIT09Yi53aGVlbERlbHRhWSYmKGc9Yi53aGVlbERlbHRhWS8xMjApO3ZvaWQgMCE9PWIud2hlZWxEZWx0YVgmJihlPS0xKmIud2hlZWxEZWx0YVgvMTIwKTtjLnVuc2hpZnQoYSxmLGUsZyk7cmV0dXJuKGQuZXZlbnQuZGlzcGF0Y2h8fGQuZXZlbnQuaGFuZGxlKS5hcHBseSh0aGlzLGMpfXZhciBjPVtcIkRPTU1vdXNlU2Nyb2xsXCIsXCJtb3VzZXdoZWVsXCJdO2lmKGQuZXZlbnQuZml4SG9va3MpZm9yKHZhciBoPWMubGVuZ3RoO2g7KWQuZXZlbnQuZml4SG9va3NbY1stLWhdXT1kLmV2ZW50Lm1vdXNlSG9va3M7ZC5ldmVudC5zcGVjaWFsLm1vdXNld2hlZWw9e3NldHVwOmZ1bmN0aW9uKCl7aWYodGhpcy5hZGRFdmVudExpc3RlbmVyKWZvcih2YXIgYT1jLmxlbmd0aDthOyl0aGlzLmFkZEV2ZW50TGlzdGVuZXIoY1stLWFdLGUsITEpO2Vsc2UgdGhpcy5vbm1vdXNld2hlZWw9ZX0sdGVhcmRvd246ZnVuY3Rpb24oKXtpZih0aGlzLnJlbW92ZUV2ZW50TGlzdGVuZXIpZm9yKHZhciBhPWMubGVuZ3RoO2E7KXRoaXMucmVtb3ZlRXZlbnRMaXN0ZW5lcihjWy0tYV0sZSwhMSk7ZWxzZSB0aGlzLm9ubW91c2V3aGVlbD1udWxsfX07ZC5mbi5leHRlbmQoe21vdXNld2hlZWw6ZnVuY3Rpb24oYSl7cmV0dXJuIGE/dGhpcy5iaW5kKFwibW91c2V3aGVlbFwiLGEpOnRoaXMudHJpZ2dlcihcIm1vdXNld2hlZWxcIil9LHVubW91c2V3aGVlbDpmdW5jdGlvbihhKXtyZXR1cm4gdGhpcy51bmJpbmQoXCJtb3VzZXdoZWVsXCIsYSl9fSl9KShqUXVlcnkpOyhmdW5jdGlvbigkKXt2YXIgb3B0aW9ucz17eGF4aXM6e3pvb21SYW5nZTpudWxsLHBhblJhbmdlOm51bGx9LHpvb206e2ludGVyYWN0aXZlOmZhbHNlLHRyaWdnZXI6XCJkYmxjbGlja1wiLGFtb3VudDoxLjV9LHBhbjp7aW50ZXJhY3RpdmU6ZmFsc2UsY3Vyc29yOlwibW92ZVwiLGZyYW1lUmF0ZToyMH19O2Z1bmN0aW9uIGluaXQocGxvdCl7ZnVuY3Rpb24gb25ab29tQ2xpY2soZSx6b29tT3V0KXt2YXIgYz1wbG90Lm9mZnNldCgpO2MubGVmdD1lLnBhZ2VYLWMubGVmdDtjLnRvcD1lLnBhZ2VZLWMudG9wO2lmKHpvb21PdXQpcGxvdC56b29tT3V0KHtjZW50ZXI6Y30pO2Vsc2UgcGxvdC56b29tKHtjZW50ZXI6Y30pfWZ1bmN0aW9uIG9uTW91c2VXaGVlbChlLGRlbHRhKXtlLnByZXZlbnREZWZhdWx0KCk7b25ab29tQ2xpY2soZSxkZWx0YTwwKTtyZXR1cm4gZmFsc2V9dmFyIHByZXZDdXJzb3I9XCJkZWZhdWx0XCIscHJldlBhZ2VYPTAscHJldlBhZ2VZPTAscGFuVGltZW91dD1udWxsO2Z1bmN0aW9uIG9uRHJhZ1N0YXJ0KGUpe2lmKGUud2hpY2ghPTEpcmV0dXJuIGZhbHNlO3ZhciBjPXBsb3QuZ2V0UGxhY2Vob2xkZXIoKS5jc3MoXCJjdXJzb3JcIik7aWYoYylwcmV2Q3Vyc29yPWM7cGxvdC5nZXRQbGFjZWhvbGRlcigpLmNzcyhcImN1cnNvclwiLHBsb3QuZ2V0T3B0aW9ucygpLnBhbi5jdXJzb3IpO3ByZXZQYWdlWD1lLnBhZ2VYO3ByZXZQYWdlWT1lLnBhZ2VZfWZ1bmN0aW9uIG9uRHJhZyhlKXt2YXIgZnJhbWVSYXRlPXBsb3QuZ2V0T3B0aW9ucygpLnBhbi5mcmFtZVJhdGU7aWYocGFuVGltZW91dHx8IWZyYW1lUmF0ZSlyZXR1cm47cGFuVGltZW91dD1zZXRUaW1lb3V0KGZ1bmN0aW9uKCl7cGxvdC5wYW4oe2xlZnQ6cHJldlBhZ2VYLWUucGFnZVgsdG9wOnByZXZQYWdlWS1lLnBhZ2VZfSk7cHJldlBhZ2VYPWUucGFnZVg7cHJldlBhZ2VZPWUucGFnZVk7cGFuVGltZW91dD1udWxsfSwxL2ZyYW1lUmF0ZSoxZTMpfWZ1bmN0aW9uIG9uRHJhZ0VuZChlKXtpZihwYW5UaW1lb3V0KXtjbGVhclRpbWVvdXQocGFuVGltZW91dCk7cGFuVGltZW91dD1udWxsfXBsb3QuZ2V0UGxhY2Vob2xkZXIoKS5jc3MoXCJjdXJzb3JcIixwcmV2Q3Vyc29yKTtwbG90LnBhbih7bGVmdDpwcmV2UGFnZVgtZS5wYWdlWCx0b3A6cHJldlBhZ2VZLWUucGFnZVl9KX1mdW5jdGlvbiBiaW5kRXZlbnRzKHBsb3QsZXZlbnRIb2xkZXIpe3ZhciBvPXBsb3QuZ2V0T3B0aW9ucygpO2lmKG8uem9vbS5pbnRlcmFjdGl2ZSl7ZXZlbnRIb2xkZXJbby56b29tLnRyaWdnZXJdKG9uWm9vbUNsaWNrKTtldmVudEhvbGRlci5tb3VzZXdoZWVsKG9uTW91c2VXaGVlbCl9aWYoby5wYW4uaW50ZXJhY3RpdmUpe2V2ZW50SG9sZGVyLmJpbmQoXCJkcmFnc3RhcnRcIix7ZGlzdGFuY2U6MTB9LG9uRHJhZ1N0YXJ0KTtldmVudEhvbGRlci5iaW5kKFwiZHJhZ1wiLG9uRHJhZyk7ZXZlbnRIb2xkZXIuYmluZChcImRyYWdlbmRcIixvbkRyYWdFbmQpfX1wbG90Lnpvb21PdXQ9ZnVuY3Rpb24oYXJncyl7aWYoIWFyZ3MpYXJncz17fTtpZighYXJncy5hbW91bnQpYXJncy5hbW91bnQ9cGxvdC5nZXRPcHRpb25zKCkuem9vbS5hbW91bnQ7YXJncy5hbW91bnQ9MS9hcmdzLmFtb3VudDtwbG90Lnpvb20oYXJncyl9O3Bsb3Quem9vbT1mdW5jdGlvbihhcmdzKXtpZighYXJncylhcmdzPXt9O3ZhciBjPWFyZ3MuY2VudGVyLGFtb3VudD1hcmdzLmFtb3VudHx8cGxvdC5nZXRPcHRpb25zKCkuem9vbS5hbW91bnQsdz1wbG90LndpZHRoKCksaD1wbG90LmhlaWdodCgpO2lmKCFjKWM9e2xlZnQ6dy8yLHRvcDpoLzJ9O3ZhciB4Zj1jLmxlZnQvdyx5Zj1jLnRvcC9oLG1pbm1heD17eDp7bWluOmMubGVmdC14Zip3L2Ftb3VudCxtYXg6Yy5sZWZ0KygxLXhmKSp3L2Ftb3VudH0seTp7bWluOmMudG9wLXlmKmgvYW1vdW50LG1heDpjLnRvcCsoMS15ZikqaC9hbW91bnR9fTskLmVhY2gocGxvdC5nZXRBeGVzKCksZnVuY3Rpb24oXyxheGlzKXt2YXIgb3B0cz1heGlzLm9wdGlvbnMsbWluPW1pbm1heFtheGlzLmRpcmVjdGlvbl0ubWluLG1heD1taW5tYXhbYXhpcy5kaXJlY3Rpb25dLm1heCx6cj1vcHRzLnpvb21SYW5nZSxwcj1vcHRzLnBhblJhbmdlO2lmKHpyPT09ZmFsc2UpcmV0dXJuO21pbj1heGlzLmMycChtaW4pO21heD1heGlzLmMycChtYXgpO2lmKG1pbj5tYXgpe3ZhciB0bXA9bWluO21pbj1tYXg7bWF4PXRtcH1pZihwcil7aWYocHJbMF0hPW51bGwmJm1pbjxwclswXSl7bWluPXByWzBdfWlmKHByWzFdIT1udWxsJiZtYXg+cHJbMV0pe21heD1wclsxXX19dmFyIHJhbmdlPW1heC1taW47aWYoenImJih6clswXSE9bnVsbCYmcmFuZ2U8enJbMF0mJmFtb3VudD4xfHx6clsxXSE9bnVsbCYmcmFuZ2U+enJbMV0mJmFtb3VudDwxKSlyZXR1cm47b3B0cy5taW49bWluO29wdHMubWF4PW1heH0pO3Bsb3Quc2V0dXBHcmlkKCk7cGxvdC5kcmF3KCk7aWYoIWFyZ3MucHJldmVudEV2ZW50KXBsb3QuZ2V0UGxhY2Vob2xkZXIoKS50cmlnZ2VyKFwicGxvdHpvb21cIixbcGxvdCxhcmdzXSl9O3Bsb3QucGFuPWZ1bmN0aW9uKGFyZ3Mpe3ZhciBkZWx0YT17eDorYXJncy5sZWZ0LHk6K2FyZ3MudG9wfTtpZihpc05hTihkZWx0YS54KSlkZWx0YS54PTA7aWYoaXNOYU4oZGVsdGEueSkpZGVsdGEueT0wOyQuZWFjaChwbG90LmdldEF4ZXMoKSxmdW5jdGlvbihfLGF4aXMpe3ZhciBvcHRzPWF4aXMub3B0aW9ucyxtaW4sbWF4LGQ9ZGVsdGFbYXhpcy5kaXJlY3Rpb25dO21pbj1heGlzLmMycChheGlzLnAyYyhheGlzLm1pbikrZCksbWF4PWF4aXMuYzJwKGF4aXMucDJjKGF4aXMubWF4KStkKTt2YXIgcHI9b3B0cy5wYW5SYW5nZTtpZihwcj09PWZhbHNlKXJldHVybjtpZihwcil7aWYocHJbMF0hPW51bGwmJnByWzBdPm1pbil7ZD1wclswXS1taW47bWluKz1kO21heCs9ZH1pZihwclsxXSE9bnVsbCYmcHJbMV08bWF4KXtkPXByWzFdLW1heDttaW4rPWQ7bWF4Kz1kfX1vcHRzLm1pbj1taW47b3B0cy5tYXg9bWF4fSk7cGxvdC5zZXR1cEdyaWQoKTtwbG90LmRyYXcoKTtpZighYXJncy5wcmV2ZW50RXZlbnQpcGxvdC5nZXRQbGFjZWhvbGRlcigpLnRyaWdnZXIoXCJwbG90cGFuXCIsW3Bsb3QsYXJnc10pfTtmdW5jdGlvbiBzaHV0ZG93bihwbG90LGV2ZW50SG9sZGVyKXtldmVudEhvbGRlci51bmJpbmQocGxvdC5nZXRPcHRpb25zKCkuem9vbS50cmlnZ2VyLG9uWm9vbUNsaWNrKTtldmVudEhvbGRlci51bmJpbmQoXCJtb3VzZXdoZWVsXCIsb25Nb3VzZVdoZWVsKTtldmVudEhvbGRlci51bmJpbmQoXCJkcmFnc3RhcnRcIixvbkRyYWdTdGFydCk7ZXZlbnRIb2xkZXIudW5iaW5kKFwiZHJhZ1wiLG9uRHJhZyk7ZXZlbnRIb2xkZXIudW5iaW5kKFwiZHJhZ2VuZFwiLG9uRHJhZ0VuZCk7aWYocGFuVGltZW91dCljbGVhclRpbWVvdXQocGFuVGltZW91dCl9cGxvdC5ob29rcy5iaW5kRXZlbnRzLnB1c2goYmluZEV2ZW50cyk7cGxvdC5ob29rcy5zaHV0ZG93bi5wdXNoKHNodXRkb3duKX0kLnBsb3QucGx1Z2lucy5wdXNoKHtpbml0OmluaXQsb3B0aW9uczpvcHRpb25zLG5hbWU6XCJuYXZpZ2F0ZVwiLHZlcnNpb246XCIxLjNcIn0pfSkoalF1ZXJ5KTsiLCIvKiBKYXZhc2NyaXB0IHBsb3R0aW5nIGxpYnJhcnkgZm9yIGpRdWVyeSwgdmVyc2lvbiAwLjguMy5cblxuQ29weXJpZ2h0IChjKSAyMDA3LTIwMTQgSU9MQSBhbmQgT2xlIExhdXJzZW4uXG5MaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UuXG5cbiovXG4oZnVuY3Rpb24oJCl7ZnVuY3Rpb24gaW5pdChwbG90KXt2YXIgc2VsZWN0aW9uPXtmaXJzdDp7eDotMSx5Oi0xfSxzZWNvbmQ6e3g6LTEseTotMX0sc2hvdzpmYWxzZSxhY3RpdmU6ZmFsc2V9O3ZhciBzYXZlZGhhbmRsZXJzPXt9O3ZhciBtb3VzZVVwSGFuZGxlcj1udWxsO2Z1bmN0aW9uIG9uTW91c2VNb3ZlKGUpe2lmKHNlbGVjdGlvbi5hY3RpdmUpe3VwZGF0ZVNlbGVjdGlvbihlKTtwbG90LmdldFBsYWNlaG9sZGVyKCkudHJpZ2dlcihcInBsb3RzZWxlY3RpbmdcIixbZ2V0U2VsZWN0aW9uKCldKX19ZnVuY3Rpb24gb25Nb3VzZURvd24oZSl7aWYoZS53aGljaCE9MSlyZXR1cm47ZG9jdW1lbnQuYm9keS5mb2N1cygpO2lmKGRvY3VtZW50Lm9uc2VsZWN0c3RhcnQhPT11bmRlZmluZWQmJnNhdmVkaGFuZGxlcnMub25zZWxlY3RzdGFydD09bnVsbCl7c2F2ZWRoYW5kbGVycy5vbnNlbGVjdHN0YXJ0PWRvY3VtZW50Lm9uc2VsZWN0c3RhcnQ7ZG9jdW1lbnQub25zZWxlY3RzdGFydD1mdW5jdGlvbigpe3JldHVybiBmYWxzZX19aWYoZG9jdW1lbnQub25kcmFnIT09dW5kZWZpbmVkJiZzYXZlZGhhbmRsZXJzLm9uZHJhZz09bnVsbCl7c2F2ZWRoYW5kbGVycy5vbmRyYWc9ZG9jdW1lbnQub25kcmFnO2RvY3VtZW50Lm9uZHJhZz1mdW5jdGlvbigpe3JldHVybiBmYWxzZX19c2V0U2VsZWN0aW9uUG9zKHNlbGVjdGlvbi5maXJzdCxlKTtzZWxlY3Rpb24uYWN0aXZlPXRydWU7bW91c2VVcEhhbmRsZXI9ZnVuY3Rpb24oZSl7b25Nb3VzZVVwKGUpfTskKGRvY3VtZW50KS5vbmUoXCJtb3VzZXVwXCIsbW91c2VVcEhhbmRsZXIpfWZ1bmN0aW9uIG9uTW91c2VVcChlKXttb3VzZVVwSGFuZGxlcj1udWxsO2lmKGRvY3VtZW50Lm9uc2VsZWN0c3RhcnQhPT11bmRlZmluZWQpZG9jdW1lbnQub25zZWxlY3RzdGFydD1zYXZlZGhhbmRsZXJzLm9uc2VsZWN0c3RhcnQ7aWYoZG9jdW1lbnQub25kcmFnIT09dW5kZWZpbmVkKWRvY3VtZW50Lm9uZHJhZz1zYXZlZGhhbmRsZXJzLm9uZHJhZztzZWxlY3Rpb24uYWN0aXZlPWZhbHNlO3VwZGF0ZVNlbGVjdGlvbihlKTtpZihzZWxlY3Rpb25Jc1NhbmUoKSl0cmlnZ2VyU2VsZWN0ZWRFdmVudCgpO2Vsc2V7cGxvdC5nZXRQbGFjZWhvbGRlcigpLnRyaWdnZXIoXCJwbG90dW5zZWxlY3RlZFwiLFtdKTtwbG90LmdldFBsYWNlaG9sZGVyKCkudHJpZ2dlcihcInBsb3RzZWxlY3RpbmdcIixbbnVsbF0pfXJldHVybiBmYWxzZX1mdW5jdGlvbiBnZXRTZWxlY3Rpb24oKXtpZighc2VsZWN0aW9uSXNTYW5lKCkpcmV0dXJuIG51bGw7aWYoIXNlbGVjdGlvbi5zaG93KXJldHVybiBudWxsO3ZhciByPXt9LGMxPXNlbGVjdGlvbi5maXJzdCxjMj1zZWxlY3Rpb24uc2Vjb25kOyQuZWFjaChwbG90LmdldEF4ZXMoKSxmdW5jdGlvbihuYW1lLGF4aXMpe2lmKGF4aXMudXNlZCl7dmFyIHAxPWF4aXMuYzJwKGMxW2F4aXMuZGlyZWN0aW9uXSkscDI9YXhpcy5jMnAoYzJbYXhpcy5kaXJlY3Rpb25dKTtyW25hbWVdPXtmcm9tOk1hdGgubWluKHAxLHAyKSx0bzpNYXRoLm1heChwMSxwMil9fX0pO3JldHVybiByfWZ1bmN0aW9uIHRyaWdnZXJTZWxlY3RlZEV2ZW50KCl7dmFyIHI9Z2V0U2VsZWN0aW9uKCk7cGxvdC5nZXRQbGFjZWhvbGRlcigpLnRyaWdnZXIoXCJwbG90c2VsZWN0ZWRcIixbcl0pO2lmKHIueGF4aXMmJnIueWF4aXMpcGxvdC5nZXRQbGFjZWhvbGRlcigpLnRyaWdnZXIoXCJzZWxlY3RlZFwiLFt7eDE6ci54YXhpcy5mcm9tLHkxOnIueWF4aXMuZnJvbSx4MjpyLnhheGlzLnRvLHkyOnIueWF4aXMudG99XSl9ZnVuY3Rpb24gY2xhbXAobWluLHZhbHVlLG1heCl7cmV0dXJuIHZhbHVlPG1pbj9taW46dmFsdWU+bWF4P21heDp2YWx1ZX1mdW5jdGlvbiBzZXRTZWxlY3Rpb25Qb3MocG9zLGUpe3ZhciBvPXBsb3QuZ2V0T3B0aW9ucygpO3ZhciBvZmZzZXQ9cGxvdC5nZXRQbGFjZWhvbGRlcigpLm9mZnNldCgpO3ZhciBwbG90T2Zmc2V0PXBsb3QuZ2V0UGxvdE9mZnNldCgpO3Bvcy54PWNsYW1wKDAsZS5wYWdlWC1vZmZzZXQubGVmdC1wbG90T2Zmc2V0LmxlZnQscGxvdC53aWR0aCgpKTtwb3MueT1jbGFtcCgwLGUucGFnZVktb2Zmc2V0LnRvcC1wbG90T2Zmc2V0LnRvcCxwbG90LmhlaWdodCgpKTtpZihvLnNlbGVjdGlvbi5tb2RlPT1cInlcIilwb3MueD1wb3M9PXNlbGVjdGlvbi5maXJzdD8wOnBsb3Qud2lkdGgoKTtpZihvLnNlbGVjdGlvbi5tb2RlPT1cInhcIilwb3MueT1wb3M9PXNlbGVjdGlvbi5maXJzdD8wOnBsb3QuaGVpZ2h0KCl9ZnVuY3Rpb24gdXBkYXRlU2VsZWN0aW9uKHBvcyl7aWYocG9zLnBhZ2VYPT1udWxsKXJldHVybjtzZXRTZWxlY3Rpb25Qb3Moc2VsZWN0aW9uLnNlY29uZCxwb3MpO2lmKHNlbGVjdGlvbklzU2FuZSgpKXtzZWxlY3Rpb24uc2hvdz10cnVlO3Bsb3QudHJpZ2dlclJlZHJhd092ZXJsYXkoKX1lbHNlIGNsZWFyU2VsZWN0aW9uKHRydWUpfWZ1bmN0aW9uIGNsZWFyU2VsZWN0aW9uKHByZXZlbnRFdmVudCl7aWYoc2VsZWN0aW9uLnNob3cpe3NlbGVjdGlvbi5zaG93PWZhbHNlO3Bsb3QudHJpZ2dlclJlZHJhd092ZXJsYXkoKTtpZighcHJldmVudEV2ZW50KXBsb3QuZ2V0UGxhY2Vob2xkZXIoKS50cmlnZ2VyKFwicGxvdHVuc2VsZWN0ZWRcIixbXSl9fWZ1bmN0aW9uIGV4dHJhY3RSYW5nZShyYW5nZXMsY29vcmQpe3ZhciBheGlzLGZyb20sdG8sa2V5LGF4ZXM9cGxvdC5nZXRBeGVzKCk7Zm9yKHZhciBrIGluIGF4ZXMpe2F4aXM9YXhlc1trXTtpZihheGlzLmRpcmVjdGlvbj09Y29vcmQpe2tleT1jb29yZCtheGlzLm4rXCJheGlzXCI7aWYoIXJhbmdlc1trZXldJiZheGlzLm49PTEpa2V5PWNvb3JkK1wiYXhpc1wiO2lmKHJhbmdlc1trZXldKXtmcm9tPXJhbmdlc1trZXldLmZyb207dG89cmFuZ2VzW2tleV0udG87YnJlYWt9fX1pZighcmFuZ2VzW2tleV0pe2F4aXM9Y29vcmQ9PVwieFwiP3Bsb3QuZ2V0WEF4ZXMoKVswXTpwbG90LmdldFlBeGVzKClbMF07ZnJvbT1yYW5nZXNbY29vcmQrXCIxXCJdO3RvPXJhbmdlc1tjb29yZCtcIjJcIl19aWYoZnJvbSE9bnVsbCYmdG8hPW51bGwmJmZyb20+dG8pe3ZhciB0bXA9ZnJvbTtmcm9tPXRvO3RvPXRtcH1yZXR1cm57ZnJvbTpmcm9tLHRvOnRvLGF4aXM6YXhpc319ZnVuY3Rpb24gc2V0U2VsZWN0aW9uKHJhbmdlcyxwcmV2ZW50RXZlbnQpe3ZhciBheGlzLHJhbmdlLG89cGxvdC5nZXRPcHRpb25zKCk7aWYoby5zZWxlY3Rpb24ubW9kZT09XCJ5XCIpe3NlbGVjdGlvbi5maXJzdC54PTA7c2VsZWN0aW9uLnNlY29uZC54PXBsb3Qud2lkdGgoKX1lbHNle3JhbmdlPWV4dHJhY3RSYW5nZShyYW5nZXMsXCJ4XCIpO3NlbGVjdGlvbi5maXJzdC54PXJhbmdlLmF4aXMucDJjKHJhbmdlLmZyb20pO3NlbGVjdGlvbi5zZWNvbmQueD1yYW5nZS5heGlzLnAyYyhyYW5nZS50byl9aWYoby5zZWxlY3Rpb24ubW9kZT09XCJ4XCIpe3NlbGVjdGlvbi5maXJzdC55PTA7c2VsZWN0aW9uLnNlY29uZC55PXBsb3QuaGVpZ2h0KCl9ZWxzZXtyYW5nZT1leHRyYWN0UmFuZ2UocmFuZ2VzLFwieVwiKTtzZWxlY3Rpb24uZmlyc3QueT1yYW5nZS5heGlzLnAyYyhyYW5nZS5mcm9tKTtzZWxlY3Rpb24uc2Vjb25kLnk9cmFuZ2UuYXhpcy5wMmMocmFuZ2UudG8pfXNlbGVjdGlvbi5zaG93PXRydWU7cGxvdC50cmlnZ2VyUmVkcmF3T3ZlcmxheSgpO2lmKCFwcmV2ZW50RXZlbnQmJnNlbGVjdGlvbklzU2FuZSgpKXRyaWdnZXJTZWxlY3RlZEV2ZW50KCl9ZnVuY3Rpb24gc2VsZWN0aW9uSXNTYW5lKCl7dmFyIG1pblNpemU9cGxvdC5nZXRPcHRpb25zKCkuc2VsZWN0aW9uLm1pblNpemU7cmV0dXJuIE1hdGguYWJzKHNlbGVjdGlvbi5zZWNvbmQueC1zZWxlY3Rpb24uZmlyc3QueCk+PW1pblNpemUmJk1hdGguYWJzKHNlbGVjdGlvbi5zZWNvbmQueS1zZWxlY3Rpb24uZmlyc3QueSk+PW1pblNpemV9cGxvdC5jbGVhclNlbGVjdGlvbj1jbGVhclNlbGVjdGlvbjtwbG90LnNldFNlbGVjdGlvbj1zZXRTZWxlY3Rpb247cGxvdC5nZXRTZWxlY3Rpb249Z2V0U2VsZWN0aW9uO3Bsb3QuaG9va3MuYmluZEV2ZW50cy5wdXNoKGZ1bmN0aW9uKHBsb3QsZXZlbnRIb2xkZXIpe3ZhciBvPXBsb3QuZ2V0T3B0aW9ucygpO2lmKG8uc2VsZWN0aW9uLm1vZGUhPW51bGwpe2V2ZW50SG9sZGVyLm1vdXNlbW92ZShvbk1vdXNlTW92ZSk7ZXZlbnRIb2xkZXIubW91c2Vkb3duKG9uTW91c2VEb3duKX19KTtwbG90Lmhvb2tzLmRyYXdPdmVybGF5LnB1c2goZnVuY3Rpb24ocGxvdCxjdHgpe2lmKHNlbGVjdGlvbi5zaG93JiZzZWxlY3Rpb25Jc1NhbmUoKSl7dmFyIHBsb3RPZmZzZXQ9cGxvdC5nZXRQbG90T2Zmc2V0KCk7dmFyIG89cGxvdC5nZXRPcHRpb25zKCk7Y3R4LnNhdmUoKTtjdHgudHJhbnNsYXRlKHBsb3RPZmZzZXQubGVmdCxwbG90T2Zmc2V0LnRvcCk7dmFyIGM9JC5jb2xvci5wYXJzZShvLnNlbGVjdGlvbi5jb2xvcik7Y3R4LnN0cm9rZVN0eWxlPWMuc2NhbGUoXCJhXCIsLjgpLnRvU3RyaW5nKCk7Y3R4LmxpbmVXaWR0aD0xO2N0eC5saW5lSm9pbj1vLnNlbGVjdGlvbi5zaGFwZTtjdHguZmlsbFN0eWxlPWMuc2NhbGUoXCJhXCIsLjQpLnRvU3RyaW5nKCk7dmFyIHg9TWF0aC5taW4oc2VsZWN0aW9uLmZpcnN0Lngsc2VsZWN0aW9uLnNlY29uZC54KSsuNSx5PU1hdGgubWluKHNlbGVjdGlvbi5maXJzdC55LHNlbGVjdGlvbi5zZWNvbmQueSkrLjUsdz1NYXRoLmFicyhzZWxlY3Rpb24uc2Vjb25kLngtc2VsZWN0aW9uLmZpcnN0LngpLTEsaD1NYXRoLmFicyhzZWxlY3Rpb24uc2Vjb25kLnktc2VsZWN0aW9uLmZpcnN0LnkpLTE7Y3R4LmZpbGxSZWN0KHgseSx3LGgpO2N0eC5zdHJva2VSZWN0KHgseSx3LGgpO2N0eC5yZXN0b3JlKCl9fSk7cGxvdC5ob29rcy5zaHV0ZG93bi5wdXNoKGZ1bmN0aW9uKHBsb3QsZXZlbnRIb2xkZXIpe2V2ZW50SG9sZGVyLnVuYmluZChcIm1vdXNlbW92ZVwiLG9uTW91c2VNb3ZlKTtldmVudEhvbGRlci51bmJpbmQoXCJtb3VzZWRvd25cIixvbk1vdXNlRG93bik7aWYobW91c2VVcEhhbmRsZXIpJChkb2N1bWVudCkudW5iaW5kKFwibW91c2V1cFwiLG1vdXNlVXBIYW5kbGVyKX0pfSQucGxvdC5wbHVnaW5zLnB1c2goe2luaXQ6aW5pdCxvcHRpb25zOntzZWxlY3Rpb246e21vZGU6bnVsbCxjb2xvcjpcIiNlOGNmYWNcIixzaGFwZTpcInJvdW5kXCIsbWluU2l6ZTo1fX0sbmFtZTpcInNlbGVjdGlvblwiLHZlcnNpb246XCIxLjFcIn0pfSkoalF1ZXJ5KTsiLCIvKiBKYXZhc2NyaXB0IHBsb3R0aW5nIGxpYnJhcnkgZm9yIGpRdWVyeSwgdmVyc2lvbiAwLjguMy5cblxuQ29weXJpZ2h0IChjKSAyMDA3LTIwMTQgSU9MQSBhbmQgT2xlIExhdXJzZW4uXG5MaWNlbnNlZCB1bmRlciB0aGUgTUlUIGxpY2Vuc2UuXG5cbiovXG4oZnVuY3Rpb24oJCl7dmFyIG9wdGlvbnM9e3hheGlzOnt0aW1lem9uZTpudWxsLHRpbWVmb3JtYXQ6bnVsbCx0d2VsdmVIb3VyQ2xvY2s6ZmFsc2UsbW9udGhOYW1lczpudWxsfX07ZnVuY3Rpb24gZmxvb3JJbkJhc2UobixiYXNlKXtyZXR1cm4gYmFzZSpNYXRoLmZsb29yKG4vYmFzZSl9ZnVuY3Rpb24gZm9ybWF0RGF0ZShkLGZtdCxtb250aE5hbWVzLGRheU5hbWVzKXtpZih0eXBlb2YgZC5zdHJmdGltZT09XCJmdW5jdGlvblwiKXtyZXR1cm4gZC5zdHJmdGltZShmbXQpfXZhciBsZWZ0UGFkPWZ1bmN0aW9uKG4scGFkKXtuPVwiXCIrbjtwYWQ9XCJcIisocGFkPT1udWxsP1wiMFwiOnBhZCk7cmV0dXJuIG4ubGVuZ3RoPT0xP3BhZCtuOm59O3ZhciByPVtdO3ZhciBlc2NhcGU9ZmFsc2U7dmFyIGhvdXJzPWQuZ2V0SG91cnMoKTt2YXIgaXNBTT1ob3VyczwxMjtpZihtb250aE5hbWVzPT1udWxsKXttb250aE5hbWVzPVtcIkphblwiLFwiRmViXCIsXCJNYXJcIixcIkFwclwiLFwiTWF5XCIsXCJKdW5cIixcIkp1bFwiLFwiQXVnXCIsXCJTZXBcIixcIk9jdFwiLFwiTm92XCIsXCJEZWNcIl19aWYoZGF5TmFtZXM9PW51bGwpe2RheU5hbWVzPVtcIlN1blwiLFwiTW9uXCIsXCJUdWVcIixcIldlZFwiLFwiVGh1XCIsXCJGcmlcIixcIlNhdFwiXX12YXIgaG91cnMxMjtpZihob3Vycz4xMil7aG91cnMxMj1ob3Vycy0xMn1lbHNlIGlmKGhvdXJzPT0wKXtob3VyczEyPTEyfWVsc2V7aG91cnMxMj1ob3Vyc31mb3IodmFyIGk9MDtpPGZtdC5sZW5ndGg7KytpKXt2YXIgYz1mbXQuY2hhckF0KGkpO2lmKGVzY2FwZSl7c3dpdGNoKGMpe2Nhc2VcImFcIjpjPVwiXCIrZGF5TmFtZXNbZC5nZXREYXkoKV07YnJlYWs7Y2FzZVwiYlwiOmM9XCJcIittb250aE5hbWVzW2QuZ2V0TW9udGgoKV07YnJlYWs7Y2FzZVwiZFwiOmM9bGVmdFBhZChkLmdldERhdGUoKSk7YnJlYWs7Y2FzZVwiZVwiOmM9bGVmdFBhZChkLmdldERhdGUoKSxcIiBcIik7YnJlYWs7Y2FzZVwiaFwiOmNhc2VcIkhcIjpjPWxlZnRQYWQoaG91cnMpO2JyZWFrO2Nhc2VcIklcIjpjPWxlZnRQYWQoaG91cnMxMik7YnJlYWs7Y2FzZVwibFwiOmM9bGVmdFBhZChob3VyczEyLFwiIFwiKTticmVhaztjYXNlXCJtXCI6Yz1sZWZ0UGFkKGQuZ2V0TW9udGgoKSsxKTticmVhaztjYXNlXCJNXCI6Yz1sZWZ0UGFkKGQuZ2V0TWludXRlcygpKTticmVhaztjYXNlXCJxXCI6Yz1cIlwiKyhNYXRoLmZsb29yKGQuZ2V0TW9udGgoKS8zKSsxKTticmVhaztjYXNlXCJTXCI6Yz1sZWZ0UGFkKGQuZ2V0U2Vjb25kcygpKTticmVhaztjYXNlXCJ5XCI6Yz1sZWZ0UGFkKGQuZ2V0RnVsbFllYXIoKSUxMDApO2JyZWFrO2Nhc2VcIllcIjpjPVwiXCIrZC5nZXRGdWxsWWVhcigpO2JyZWFrO2Nhc2VcInBcIjpjPWlzQU0/XCJcIitcImFtXCI6XCJcIitcInBtXCI7YnJlYWs7Y2FzZVwiUFwiOmM9aXNBTT9cIlwiK1wiQU1cIjpcIlwiK1wiUE1cIjticmVhaztjYXNlXCJ3XCI6Yz1cIlwiK2QuZ2V0RGF5KCk7YnJlYWt9ci5wdXNoKGMpO2VzY2FwZT1mYWxzZX1lbHNle2lmKGM9PVwiJVwiKXtlc2NhcGU9dHJ1ZX1lbHNle3IucHVzaChjKX19fXJldHVybiByLmpvaW4oXCJcIil9ZnVuY3Rpb24gbWFrZVV0Y1dyYXBwZXIoZCl7ZnVuY3Rpb24gYWRkUHJveHlNZXRob2Qoc291cmNlT2JqLHNvdXJjZU1ldGhvZCx0YXJnZXRPYmosdGFyZ2V0TWV0aG9kKXtzb3VyY2VPYmpbc291cmNlTWV0aG9kXT1mdW5jdGlvbigpe3JldHVybiB0YXJnZXRPYmpbdGFyZ2V0TWV0aG9kXS5hcHBseSh0YXJnZXRPYmosYXJndW1lbnRzKX19dmFyIHV0Yz17ZGF0ZTpkfTtpZihkLnN0cmZ0aW1lIT11bmRlZmluZWQpe2FkZFByb3h5TWV0aG9kKHV0YyxcInN0cmZ0aW1lXCIsZCxcInN0cmZ0aW1lXCIpfWFkZFByb3h5TWV0aG9kKHV0YyxcImdldFRpbWVcIixkLFwiZ2V0VGltZVwiKTthZGRQcm94eU1ldGhvZCh1dGMsXCJzZXRUaW1lXCIsZCxcInNldFRpbWVcIik7dmFyIHByb3BzPVtcIkRhdGVcIixcIkRheVwiLFwiRnVsbFllYXJcIixcIkhvdXJzXCIsXCJNaWxsaXNlY29uZHNcIixcIk1pbnV0ZXNcIixcIk1vbnRoXCIsXCJTZWNvbmRzXCJdO2Zvcih2YXIgcD0wO3A8cHJvcHMubGVuZ3RoO3ArKyl7YWRkUHJveHlNZXRob2QodXRjLFwiZ2V0XCIrcHJvcHNbcF0sZCxcImdldFVUQ1wiK3Byb3BzW3BdKTthZGRQcm94eU1ldGhvZCh1dGMsXCJzZXRcIitwcm9wc1twXSxkLFwic2V0VVRDXCIrcHJvcHNbcF0pfXJldHVybiB1dGN9ZnVuY3Rpb24gZGF0ZUdlbmVyYXRvcih0cyxvcHRzKXtpZihvcHRzLnRpbWV6b25lPT1cImJyb3dzZXJcIil7cmV0dXJuIG5ldyBEYXRlKHRzKX1lbHNlIGlmKCFvcHRzLnRpbWV6b25lfHxvcHRzLnRpbWV6b25lPT1cInV0Y1wiKXtyZXR1cm4gbWFrZVV0Y1dyYXBwZXIobmV3IERhdGUodHMpKX1lbHNlIGlmKHR5cGVvZiB0aW1lem9uZUpTIT1cInVuZGVmaW5lZFwiJiZ0eXBlb2YgdGltZXpvbmVKUy5EYXRlIT1cInVuZGVmaW5lZFwiKXt2YXIgZD1uZXcgdGltZXpvbmVKUy5EYXRlO2Quc2V0VGltZXpvbmUob3B0cy50aW1lem9uZSk7ZC5zZXRUaW1lKHRzKTtyZXR1cm4gZH1lbHNle3JldHVybiBtYWtlVXRjV3JhcHBlcihuZXcgRGF0ZSh0cykpfX12YXIgdGltZVVuaXRTaXplPXtzZWNvbmQ6MWUzLG1pbnV0ZTo2MCoxZTMsaG91cjo2MCo2MCoxZTMsZGF5OjI0KjYwKjYwKjFlMyxtb250aDozMCoyNCo2MCo2MCoxZTMscXVhcnRlcjozKjMwKjI0KjYwKjYwKjFlMyx5ZWFyOjM2NS4yNDI1KjI0KjYwKjYwKjFlM307dmFyIGJhc2VTcGVjPVtbMSxcInNlY29uZFwiXSxbMixcInNlY29uZFwiXSxbNSxcInNlY29uZFwiXSxbMTAsXCJzZWNvbmRcIl0sWzMwLFwic2Vjb25kXCJdLFsxLFwibWludXRlXCJdLFsyLFwibWludXRlXCJdLFs1LFwibWludXRlXCJdLFsxMCxcIm1pbnV0ZVwiXSxbMzAsXCJtaW51dGVcIl0sWzEsXCJob3VyXCJdLFsyLFwiaG91clwiXSxbNCxcImhvdXJcIl0sWzgsXCJob3VyXCJdLFsxMixcImhvdXJcIl0sWzEsXCJkYXlcIl0sWzIsXCJkYXlcIl0sWzMsXCJkYXlcIl0sWy4yNSxcIm1vbnRoXCJdLFsuNSxcIm1vbnRoXCJdLFsxLFwibW9udGhcIl0sWzIsXCJtb250aFwiXV07dmFyIHNwZWNNb250aHM9YmFzZVNwZWMuY29uY2F0KFtbMyxcIm1vbnRoXCJdLFs2LFwibW9udGhcIl0sWzEsXCJ5ZWFyXCJdXSk7dmFyIHNwZWNRdWFydGVycz1iYXNlU3BlYy5jb25jYXQoW1sxLFwicXVhcnRlclwiXSxbMixcInF1YXJ0ZXJcIl0sWzEsXCJ5ZWFyXCJdXSk7ZnVuY3Rpb24gaW5pdChwbG90KXtwbG90Lmhvb2tzLnByb2Nlc3NPcHRpb25zLnB1c2goZnVuY3Rpb24ocGxvdCxvcHRpb25zKXskLmVhY2gocGxvdC5nZXRBeGVzKCksZnVuY3Rpb24oYXhpc05hbWUsYXhpcyl7dmFyIG9wdHM9YXhpcy5vcHRpb25zO2lmKG9wdHMubW9kZT09XCJ0aW1lXCIpe2F4aXMudGlja0dlbmVyYXRvcj1mdW5jdGlvbihheGlzKXt2YXIgdGlja3M9W107dmFyIGQ9ZGF0ZUdlbmVyYXRvcihheGlzLm1pbixvcHRzKTt2YXIgbWluU2l6ZT0wO3ZhciBzcGVjPW9wdHMudGlja1NpemUmJm9wdHMudGlja1NpemVbMV09PT1cInF1YXJ0ZXJcInx8b3B0cy5taW5UaWNrU2l6ZSYmb3B0cy5taW5UaWNrU2l6ZVsxXT09PVwicXVhcnRlclwiP3NwZWNRdWFydGVyczpzcGVjTW9udGhzO2lmKG9wdHMubWluVGlja1NpemUhPW51bGwpe2lmKHR5cGVvZiBvcHRzLnRpY2tTaXplPT1cIm51bWJlclwiKXttaW5TaXplPW9wdHMudGlja1NpemV9ZWxzZXttaW5TaXplPW9wdHMubWluVGlja1NpemVbMF0qdGltZVVuaXRTaXplW29wdHMubWluVGlja1NpemVbMV1dfX1mb3IodmFyIGk9MDtpPHNwZWMubGVuZ3RoLTE7KytpKXtpZihheGlzLmRlbHRhPChzcGVjW2ldWzBdKnRpbWVVbml0U2l6ZVtzcGVjW2ldWzFdXStzcGVjW2krMV1bMF0qdGltZVVuaXRTaXplW3NwZWNbaSsxXVsxXV0pLzImJnNwZWNbaV1bMF0qdGltZVVuaXRTaXplW3NwZWNbaV1bMV1dPj1taW5TaXplKXticmVha319dmFyIHNpemU9c3BlY1tpXVswXTt2YXIgdW5pdD1zcGVjW2ldWzFdO2lmKHVuaXQ9PVwieWVhclwiKXtpZihvcHRzLm1pblRpY2tTaXplIT1udWxsJiZvcHRzLm1pblRpY2tTaXplWzFdPT1cInllYXJcIil7c2l6ZT1NYXRoLmZsb29yKG9wdHMubWluVGlja1NpemVbMF0pfWVsc2V7dmFyIG1hZ249TWF0aC5wb3coMTAsTWF0aC5mbG9vcihNYXRoLmxvZyhheGlzLmRlbHRhL3RpbWVVbml0U2l6ZS55ZWFyKS9NYXRoLkxOMTApKTt2YXIgbm9ybT1heGlzLmRlbHRhL3RpbWVVbml0U2l6ZS55ZWFyL21hZ247aWYobm9ybTwxLjUpe3NpemU9MX1lbHNlIGlmKG5vcm08Myl7c2l6ZT0yfWVsc2UgaWYobm9ybTw3LjUpe3NpemU9NX1lbHNle3NpemU9MTB9c2l6ZSo9bWFnbn1pZihzaXplPDEpe3NpemU9MX19YXhpcy50aWNrU2l6ZT1vcHRzLnRpY2tTaXplfHxbc2l6ZSx1bml0XTt2YXIgdGlja1NpemU9YXhpcy50aWNrU2l6ZVswXTt1bml0PWF4aXMudGlja1NpemVbMV07dmFyIHN0ZXA9dGlja1NpemUqdGltZVVuaXRTaXplW3VuaXRdO2lmKHVuaXQ9PVwic2Vjb25kXCIpe2Quc2V0U2Vjb25kcyhmbG9vckluQmFzZShkLmdldFNlY29uZHMoKSx0aWNrU2l6ZSkpfWVsc2UgaWYodW5pdD09XCJtaW51dGVcIil7ZC5zZXRNaW51dGVzKGZsb29ySW5CYXNlKGQuZ2V0TWludXRlcygpLHRpY2tTaXplKSl9ZWxzZSBpZih1bml0PT1cImhvdXJcIil7ZC5zZXRIb3VycyhmbG9vckluQmFzZShkLmdldEhvdXJzKCksdGlja1NpemUpKX1lbHNlIGlmKHVuaXQ9PVwibW9udGhcIil7ZC5zZXRNb250aChmbG9vckluQmFzZShkLmdldE1vbnRoKCksdGlja1NpemUpKX1lbHNlIGlmKHVuaXQ9PVwicXVhcnRlclwiKXtkLnNldE1vbnRoKDMqZmxvb3JJbkJhc2UoZC5nZXRNb250aCgpLzMsdGlja1NpemUpKX1lbHNlIGlmKHVuaXQ9PVwieWVhclwiKXtkLnNldEZ1bGxZZWFyKGZsb29ySW5CYXNlKGQuZ2V0RnVsbFllYXIoKSx0aWNrU2l6ZSkpfWQuc2V0TWlsbGlzZWNvbmRzKDApO2lmKHN0ZXA+PXRpbWVVbml0U2l6ZS5taW51dGUpe2Quc2V0U2Vjb25kcygwKX1pZihzdGVwPj10aW1lVW5pdFNpemUuaG91cil7ZC5zZXRNaW51dGVzKDApfWlmKHN0ZXA+PXRpbWVVbml0U2l6ZS5kYXkpe2Quc2V0SG91cnMoMCl9aWYoc3RlcD49dGltZVVuaXRTaXplLmRheSo0KXtkLnNldERhdGUoMSl9aWYoc3RlcD49dGltZVVuaXRTaXplLm1vbnRoKjIpe2Quc2V0TW9udGgoZmxvb3JJbkJhc2UoZC5nZXRNb250aCgpLDMpKX1pZihzdGVwPj10aW1lVW5pdFNpemUucXVhcnRlcioyKXtkLnNldE1vbnRoKGZsb29ySW5CYXNlKGQuZ2V0TW9udGgoKSw2KSl9aWYoc3RlcD49dGltZVVuaXRTaXplLnllYXIpe2Quc2V0TW9udGgoMCl9dmFyIGNhcnJ5PTA7dmFyIHY9TnVtYmVyLk5hTjt2YXIgcHJldjtkb3twcmV2PXY7dj1kLmdldFRpbWUoKTt0aWNrcy5wdXNoKHYpO2lmKHVuaXQ9PVwibW9udGhcInx8dW5pdD09XCJxdWFydGVyXCIpe2lmKHRpY2tTaXplPDEpe2Quc2V0RGF0ZSgxKTt2YXIgc3RhcnQ9ZC5nZXRUaW1lKCk7ZC5zZXRNb250aChkLmdldE1vbnRoKCkrKHVuaXQ9PVwicXVhcnRlclwiPzM6MSkpO3ZhciBlbmQ9ZC5nZXRUaW1lKCk7ZC5zZXRUaW1lKHYrY2FycnkqdGltZVVuaXRTaXplLmhvdXIrKGVuZC1zdGFydCkqdGlja1NpemUpO2NhcnJ5PWQuZ2V0SG91cnMoKTtkLnNldEhvdXJzKDApfWVsc2V7ZC5zZXRNb250aChkLmdldE1vbnRoKCkrdGlja1NpemUqKHVuaXQ9PVwicXVhcnRlclwiPzM6MSkpfX1lbHNlIGlmKHVuaXQ9PVwieWVhclwiKXtkLnNldEZ1bGxZZWFyKGQuZ2V0RnVsbFllYXIoKSt0aWNrU2l6ZSl9ZWxzZXtkLnNldFRpbWUoditzdGVwKX19d2hpbGUodjxheGlzLm1heCYmdiE9cHJldik7cmV0dXJuIHRpY2tzfTtheGlzLnRpY2tGb3JtYXR0ZXI9ZnVuY3Rpb24odixheGlzKXt2YXIgZD1kYXRlR2VuZXJhdG9yKHYsYXhpcy5vcHRpb25zKTtpZihvcHRzLnRpbWVmb3JtYXQhPW51bGwpe3JldHVybiBmb3JtYXREYXRlKGQsb3B0cy50aW1lZm9ybWF0LG9wdHMubW9udGhOYW1lcyxvcHRzLmRheU5hbWVzKX12YXIgdXNlUXVhcnRlcnM9YXhpcy5vcHRpb25zLnRpY2tTaXplJiZheGlzLm9wdGlvbnMudGlja1NpemVbMV09PVwicXVhcnRlclwifHxheGlzLm9wdGlvbnMubWluVGlja1NpemUmJmF4aXMub3B0aW9ucy5taW5UaWNrU2l6ZVsxXT09XCJxdWFydGVyXCI7dmFyIHQ9YXhpcy50aWNrU2l6ZVswXSp0aW1lVW5pdFNpemVbYXhpcy50aWNrU2l6ZVsxXV07dmFyIHNwYW49YXhpcy5tYXgtYXhpcy5taW47dmFyIHN1ZmZpeD1vcHRzLnR3ZWx2ZUhvdXJDbG9jaz9cIiAlcFwiOlwiXCI7dmFyIGhvdXJDb2RlPW9wdHMudHdlbHZlSG91ckNsb2NrP1wiJUlcIjpcIiVIXCI7dmFyIGZtdDtpZih0PHRpbWVVbml0U2l6ZS5taW51dGUpe2ZtdD1ob3VyQ29kZStcIjolTTolU1wiK3N1ZmZpeH1lbHNlIGlmKHQ8dGltZVVuaXRTaXplLmRheSl7aWYoc3BhbjwyKnRpbWVVbml0U2l6ZS5kYXkpe2ZtdD1ob3VyQ29kZStcIjolTVwiK3N1ZmZpeH1lbHNle2ZtdD1cIiViICVkIFwiK2hvdXJDb2RlK1wiOiVNXCIrc3VmZml4fX1lbHNlIGlmKHQ8dGltZVVuaXRTaXplLm1vbnRoKXtmbXQ9XCIlYiAlZFwifWVsc2UgaWYodXNlUXVhcnRlcnMmJnQ8dGltZVVuaXRTaXplLnF1YXJ0ZXJ8fCF1c2VRdWFydGVycyYmdDx0aW1lVW5pdFNpemUueWVhcil7aWYoc3Bhbjx0aW1lVW5pdFNpemUueWVhcil7Zm10PVwiJWJcIn1lbHNle2ZtdD1cIiViICVZXCJ9fWVsc2UgaWYodXNlUXVhcnRlcnMmJnQ8dGltZVVuaXRTaXplLnllYXIpe2lmKHNwYW48dGltZVVuaXRTaXplLnllYXIpe2ZtdD1cIlElcVwifWVsc2V7Zm10PVwiUSVxICVZXCJ9fWVsc2V7Zm10PVwiJVlcIn12YXIgcnQ9Zm9ybWF0RGF0ZShkLGZtdCxvcHRzLm1vbnRoTmFtZXMsb3B0cy5kYXlOYW1lcyk7cmV0dXJuIHJ0fX19KX0pfSQucGxvdC5wbHVnaW5zLnB1c2goe2luaXQ6aW5pdCxvcHRpb25zOm9wdGlvbnMsbmFtZTpcInRpbWVcIix2ZXJzaW9uOlwiMS4wXCJ9KTskLnBsb3QuZm9ybWF0RGF0ZT1mb3JtYXREYXRlOyQucGxvdC5kYXRlR2VuZXJhdG9yPWRhdGVHZW5lcmF0b3J9KShqUXVlcnkpOyIsIm1vZHVsZS5leHBvcnRzID0gbW9tZW50OyIsIm1vZHVsZS5leHBvcnRzID0gUmVhY3Q7IiwibW9kdWxlLmV4cG9ydHMgPSBSZWFjdERPTTsiXSwic291cmNlUm9vdCI6IiJ9