"use strict";
const webpack = require("webpack");

module.exports = {
    entry: {
        PeriodicDataDisplay: "./wwwroot/Scripts/TSX/PeriodicDataDisplay.tsx",
        TrendingDataDisplay: "./wwwroot/Scripts/TSX/TrendingDataDisplay.tsx",
        DataQualitySummary: "./wwwroot/Scripts/TSX/DataQualitySummary.tsx",
        PQTrendingWebReport: "./wwwroot/Scripts/TSX/PQTrendingWebReport.tsx",
        StepChangeWebReport: "./wwwroot/Scripts/TSX/StepChangeWebReport.tsx",
        SpectralDataDisplay: "./wwwroot/Scripts/TSX/SpectralDataDisplay.tsx"
    },
    output: {
        filename: "./wwwroot/Scripts/[name].js"
    },
    // Enable sourcemaps for debugging webpack's output.
    devtool: "source-map",

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

    }
};