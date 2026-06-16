import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'
import path from 'node:path'

// Tailwind v4 não precisa de tailwind.config.js!
// Toda a configuração fica no CSS (src/index.css) usando @theme
export default defineConfig({
  plugins: [
    react(),
    tailwindcss(), // plugin oficial do Tailwind v4 para Vite — muito mais rápido que PostCSS
  ],
  resolve: {
    alias: {
      // @/ aponta para src/ — assim você importa @/lib/api ao invés de ../../lib/api
      '@': path.resolve(__dirname, './src'),
    },
  },
  server: {
    port: 3000,
    proxy: {
      // Toda requisição que começa com /api é repassada para o backend
      // O browser vê como localhost:3000/api (mesma origem) → sem CORS
      '/api': {
        target: 'http://localhost:5266',
        changeOrigin: true,
      },
    },
  },
})
