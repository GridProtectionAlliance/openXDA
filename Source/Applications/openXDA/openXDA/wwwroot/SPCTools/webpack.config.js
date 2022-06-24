"use strict";
const path = require("path");
module.exports = env => {
    if (process.env.NODE_ENV == undefined) process.env.NODE_ENV = 'development';


    return {
        mode: process.env.NODE_ENV,
        context: path.resolve(__dirname, ''),
        cache: true,
        entry: {
            SPCTools: "./Scripts/TSX/SPCTools.tsx"
        },

        output: {
            path: path.resolve(__dirname, 'Scripts', 'js'),
            publicPath: 'js/',
            filename: "[name].js",
            //chunkFilename: '[name].bundle.js'
        },
        // Enable sourcemaps for debugging webpack's output.
        devtool: "inline-source-map",
        resolve: {
            // Add '.ts' and '.tsx' as resolvable extensions.
            extensions: [".webpack.js", ".web.js", ".ts", ".tsx", ".js", ".css"],
            alias: {
            }
        },
        module: {
            rules: [
                // All files with a '.ts' or '.tsx' extension will be handled by 'ts-loader'.
                { test: /\.tsx?$/, loader: "ts-loader" },
                {
                    test: /\.css$/,
                    include: /\./,
                    use: [{ loader: 'style-loader' }, { loader: 'css-loader' }],
                },
                {
                    test: /\.js$/,
                    enforce: "pre",
                    loader: "source-map-loader"
                },
                { test: /\.(woff|woff2|ttf|eot|svg|png|gif)(\?v=[0-9]\.[0-9]\.[0-9])?$/, loader: "url-loader", options: { limit: 100000 } },
            ]
        },
        externals: {
            jquery: 'jQuery',
            react: 'React',
            'react-dom': 'ReactDOM',
            //ace: 'ace',
        },
        optimization: {
            //splitChunks: {
            //    chunks: 'all',
            //}
            //minimizer: [new UglifyJsPlugin({
            //    test: /\.js(\?.*)?$/i,
            //    sourceMap: true
            //})],
        },
    }
};