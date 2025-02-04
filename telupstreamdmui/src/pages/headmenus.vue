<template>
  <div>
    <span class="vmtime">{{nowtime}}</span>
    <span class="vmdivider">
      <a-divider type="vertical" style="height:12px;background-color: #cccccc;" />
    </span>
    <span>
      <a-dropdown :trigger="['click']">
        <a class="vmdropdown" @click.prevent>
          {{ getusername() }}
          <DownOutlined />
        </a>
        <template #overlay>
          <a-menu>
            <a-menu-item>
              <UserOutlined />
              <a class="vmitem" href="javascript:;" @click="navtomenu('4', '#/profile')">修改密码</a>
            </a-menu-item>
            <a-menu-divider />
            <a-menu-item>
              <ExportOutlined />
              <a class="vmitem" href="javascript:;" @click="exit()">退出系统</a>
            </a-menu-item>
          </a-menu>
        </template>
      </a-dropdown>
    </span>
  </div>
</template>
<script setup>
import {MenuFoldOutlined,MenuUnfoldOutlined,UserOutlined,DownOutlined,ExportOutlined,TranslationOutlined} from '@ant-design/icons-vue'
import { ref,getCurrentInstance } from 'vue'

const nowtime = ref('')

const ginstance = getCurrentInstance()
const gconfig = ginstance.appContext.config.globalProperties

const navtomenu=(key,path)=>{
    if(key) gconfig.$uimenuselectedkey = key
    location.href=path
}

const exit = ()=> {
    gconfig.$exit()
}

const getusername = () => {
    var username = gconfig.$getusername()
    if(username){
        username = gconfig.$ismobile() ? username.length > 0x10 ? username.substr(0x00,0x10) + "..." : username : username
        return username
    }
}

setInterval(()=>{
    nowtime.value = new Date().toLocaleString('zh-CN', {timeZone:'Asia/Shanghai'})
},1000)

</script>
<style scoped>
  .vmtime {
    color: #666666;
    font-weight: 600;
  }

  .vmspan {
    padding-left: 15px;
  }

  .vmdivider {
    padding: 0 2px;
  }

  .vmdropdown {
    color: #666666;
  }

  .vmitem {
    padding-left: 10px;
  }
</style>
