// vue.config.js
module.exports = {
  outputDir: "../wwwroot/client-app/",
  filenameHashing: false,
  devServer: {
    proxy: {
      '/api': {
        target: 'http://127.0.0.1:5000/',
        ws: true,
        changeOrigin: true
      },
    }
  },
}