const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const fs = require('fs');

module.exports = {
  entry: './src/index.js',
  output: {
    path: path.resolve(__dirname, 'dist'),
    filename: 'bundle.js',
  },
  module: {
    rules: [
      {
        test: /\.(js|jsx)$/,
        exclude: /node_modules/,
        use: 'babel-loader',
      },
      {
        test: /\.css$/, // Add this rule for CSS files
        use: ['style-loader', 'css-loader'], // Apply the CSS loaders
      },
      {
        test: /\.scss$/, // Rule for SCSS files
        use: ['style-loader', 'css-loader', 'sass-loader'], // Apply SCSS loaders
      },
    ],
  },
  resolve: {
    alias: {
      '@': path.resolve(__dirname, 'src/'),
    },
    extensions: ['.js', '.jsx'],
  },
  plugins: [
    new HtmlWebpackPlugin({
      template: './public/index.html',
    }),
  ],
  devServer: {
    static: path.resolve(__dirname, 'dist'),
    historyApiFallback: true,
    // host: 'localhost',
    // port: 3000, // Change the port if needed
    // server: {
    //   type: 'https', // ✅ This enables HTTPS
    // },
  },
};
