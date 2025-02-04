<script setup>
  import { ref, computed } from 'vue'

  import page_login from '@/pages/login.vue'
  import page_console from '@/pages/console.vue'
  import page_journals from '@/pages/journals.vue'
  import page_devices from '@/pages/devices.vue'
  import page_statistics from '@/pages/statistics.vue'
  import page_logs from '@/pages/logs.vue'
  import page_profile from '@/pages/profile.vue'

  const routes = {
    "/": page_login,
    "/console": page_console,
    "/journals": page_journals,
    "/devices": page_devices,
    "/statistics": page_statistics,
    "/logs": page_logs,
    "/profile": page_profile,
  }

  const currentPath = ref(window.location.hash)
  window.addEventListener('hashchange', () => {
    currentPath.value = window.location.hash.split('?', 0x02)[0x00]
  })
  window.addEventListener('load', () => {
    currentPath.value = window.location.hash.split('?', 0x02)[0x00]
  })
  const currentView = computed(() => {
    return routes[currentPath.value.slice(1) || '/'] || none
  })
</script>
<script>
  export default {
    provide() {
      return {
        reload: this.reload
      }
    },
    data() {
      return {
        contentmode: 0x00,
        isRouteAlive: true
      }
    },
    methods: {
      reload() {
        this.isRouteAlive = false
        this.$nextTick(() => {
          this.isRouteAlive = true
        })
      }
    }
  }
</script>

<template>
  <div>
    <component :is="currentView" v-if="isRouteAlive" />
  </div>
</template>

<style>
  #app {
    font-family: Avenir, Helvetica, Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #2c3e50;
  }
</style>
