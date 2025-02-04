import { createApp } from 'vue'
import App from './App.vue'
import Antd from 'ant-design-vue'
import 'ant-design-vue/dist/reset.css'
import fstorage from '@/components/fstorage.ts'
import globalsettings from '@/components/globalsettings.js'

import sidemenus from '@/pages/sidemenus.vue'
import headmenus from '@/pages/headmenus.vue'
import cntfooter from '@/pages/cntfooter.vue'

var app = createApp(App)
app.config.globalProperties.$storage = fstorage

app
  .use(Antd)
  .use(fstorage)
  .use(globalsettings)

app
  .component('ms-sidemenus', sidemenus)
  .component('ms-headmenus', headmenus)
  .component('ms-cntfooter', cntfooter)

app.mount('#app')
