"use strict";
const webpack = require("webpack");
const path = require("path");
const UglifyJsPlugin = require('uglifyjs-webpack-plugin');

module.exports = env => {
    //console.log(process.env.NODE_ENV);
    if (process.env.NODE_ENV == undefined) process.env.NODE_ENV = 'development';


    return {
        mode: process.env.NODE_ENV,
        context: path.resolve(__dirname, 'wwwroot', 'Scripts'),
        cache: true,
        entry: {
            PeriodicDataDisplay: "./TSX/PeriodicDataDisplay.tsx",
            TrendingDataDisplay: "./TSX/TrendingDataDisplay.tsx",
            DataQualitySummary: "./TSX/DataQualitySummary.tsx",
            PQTrendingWebReport: "./TSX/PQTrendingWebReport.tsx",
            StepChangeWebReport: "./TSX/StepChangeWebReport.tsx",
            SpectralDataDisplay: "./TSX/SpectralDataDisplay.tsx",
            SystemCenter: "./TSX/SystemCenter/SystemCenter.tsx"
        },
        output: {
            path: path.resolve(__dirname, 'wwwroot', 'Scripts'),
            filename: "[name].js"
        },
        // Enable sourcemaps for debugging webpack's output.
        devtool: "inline-source-map",
        resolve: {
            // Add '.ts' and '.tsx' as resolvable extensions.
            extensions: [".webpack.js", ".web.js", ".ts", ".tsx", ".js", ".css"]
        },
        module: {
            rules: [
                // All files with a '.ts' or '.tsx' extension will be handled by 'ts-loader'.
                { test: /\.tsx?$/, loader: "ts-loader" },
                {
                    test: /\.css$/,
                    include: /\./,
                    loaders: ['style-loader', 'css-loader'],
                },
                {
                    test: /\.js$/,
                    enforce: "pre",
                    loader: "source-map-loader"
                },
                { test: /\.(woff|woff2|ttf|eot|svg|png|gif)(\?v=[0-9]\.[0-9]\.[0-9])?$/, loader: "url-loader?limit=100000" }
            ]
        },
        externals: {
            jquery: 'jQuery',
            react: 'React',
            'react-dom': 'ReactDOM',
            moment: 'moment'

        },
        optimization: {
            minimizer: [new UglifyJsPlugin({
                test: /\.js(\?.*)?$/i,
                sourceMap: true
            })],
        },
    }
};