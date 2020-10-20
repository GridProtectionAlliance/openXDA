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
                    loaders: ['style-loader', 'css-loader'],
                },
                {
                    test: /\.js$/,
                    enforce: "pre",
                    loader: "source-map-loader"
                },
                { test: /\.(woff|woff2|ttf|eot|svg|png|gif)(\?v=[0-9]\.[0-9]\.[0-9])?$/, loader: "url-loader?limit=100000" },
                {
                    test: /\.scss$/, use: [
                        { loader: "style-loader" },  // to inject the result into the DOM as a style block
                        { loader: "css-modules-typescript-loader" },  // to generate a .d.ts module next to the .scss file (also requires a declaration.d.ts with "declare modules '*.scss';" in it to tell TypeScript that "import styles from './styles.scss';" means to load the module "./styles.scss.d.td")
                        { loader: "css-loader", options: { modules: true } },  // to convert the resulting CSS to Javascript to be bundled (modules:true to rename CSS classes in output to cryptic identifiers, except if wrapped in a :global(...) pseudo class)
                        { loader: "sass-loader" },  // to convert SASS to CSS
                        // NOTE: The first build after adding/removing/renaming CSS classes fails, since the newly generated .d.ts typescript module is picked up only later
                    ]
                }, 

            ]
        },
        externals: {
            jquery: 'jQuery',
            react: 'React',
            'react-dom': 'ReactDOM',
            moment: 'moment',
            //ace: 'ace',
            d3: 'd3',
            'react-router-dom': 'ReactRouterDOM',
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