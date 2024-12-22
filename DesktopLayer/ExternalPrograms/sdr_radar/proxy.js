const express = require('express');
const cors = require('cors');
const { createProxyMiddleware } = require('http-proxy-middleware');

const app = express();

// CORS ayarlarını etkinleştir
app.use(cors());

// Proxy ayarları
app.use('/data', createProxyMiddleware({
    target: 'http://127.0.0.1:30003',
    changeOrigin: true,
    pathRewrite: {
        '^/data': '/data.json'
    }
}));

// Sunucuyu başlat
app.listen(3000, () => {
    console.log('Proxy server çalışıyor: http://localhost:3000');
}); 