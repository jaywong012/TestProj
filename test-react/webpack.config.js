const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');

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
    alias:{
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
    static: './dist',
    historyApiFallback: true,
  },
};
