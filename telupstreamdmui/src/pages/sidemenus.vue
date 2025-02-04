<template>
  <div class="menucontainer">
    <div class="sitethumb">
      <div v-if="!collapsedstate.state" class="sitethumbtext noselect nositeicon">
        <div class="shorttitle">DMUM</div>
        <div class="shortsubtitle">流量充值中控系统</div>
      </div>
      <div v-else class="sitethumbicon" />
    </div>
    <a-menu id="sidemenus" mode="inline"
            v-model:selectedKeys="selectedKeys"
            class="menuitems"
            :items="items" @click="handleClick"></a-menu>
    <div class="pmcopyright" v-if="!collapsedstate.state">
      <a class="pmcopylink" :href="$siteurl">mingshenggroup.cn</a>
      <span>v1.0.0.1</span>
    </div>
    <div class="pmcopycenter" v-else>
      <a class="pmcopylink" :href="$siteurl">mingshenggroup.cn</a>
      <span>v1.0.0.1</span>
    </div>
  </div>
</template>

<script setup>
  import {
    UserOutlined, DollarOutlined, FileDoneOutlined, AreaChartOutlined,
    GlobalOutlined, GiftOutlined, TagsFilled, 
    TeamOutlined, SolutionOutlined, EyeOutlined,
    DesktopOutlined, AuditOutlined, ClusterOutlined, BookOutlined,
    FileProtectOutlined,UsergroupAddOutlined,ApartmentOutlined } from '@ant-design/icons-vue'
import {reactive,ref,h,defineProps, watchEffect,getCurrentInstance} from 'vue'



const collapsedstate = reactive({
    state: false
})
const props = defineProps(['selectedkey', 'collapsed'])
const selectedKeys =  ref([props.selectedkey?props.selectedkey:'1'])
const globalinstance = getCurrentInstance()
const menumap = {
    "1": "#/"
}

watchEffect(()=>{
    collapsedstate.state = props.collapsed
})

const collapsed = ref(props.collapsed?props.collapsed:false)

function getItem(label, key, icon, children, type) {
    return { key, icon, children, label, type }
}

const items = reactive([

    getItem('首页', 'mco', null, [
      { type: 'divider' },
      getItem('控制台', 'console', () => h(DesktopOutlined)),
    ], 'group'),
    getItem('台账', 'mjn', null, [
      { type: 'divider' },
      getItem('充值台账', 'journals', () => h(AuditOutlined)),
    ], 'group'),
    getItem('设备', 'mdv', null, [
      { type: 'divider' },
      getItem('设备管理', 'devices', () => h(ClusterOutlined)),
    ], 'group'),
  getItem('数据', 'msd', null, [
    { type: 'divider' },
    getItem('运行日志', 'logs', () => h(BookOutlined)),
    getItem('统计分析', 'statistics', () => h(AreaChartOutlined)),
  ], 'group'),
])

const handleClick = e => {
    globalinstance.appContext.config.globalProperties.$uimenuselectedkey = e.key
    location.href='#'+globalinstance.appContext.config.globalProperties.$menumappaths[e.key];
}

</script>

<style scoped>
  :deep(.ant-menu-item-group) {
    padding-top: 10px;
  }

  .sitethumb {
    width: 100%;
    overflow: hidden;
  }

  .shorttitle {
    font-size: 28px;
    line-height: 30px;
    font-weight: bolder;
  }

  .shortsubtitle {
    margin: 5px 0 0 0;
    font-size: 12px;
    line-height: 20px;
    font-weight: normal;
    color: #999999;
  }

  .nositeicon {
    margin-left: 20px;
  }

  .sitethumbicon {
    float: left;
    display: block;
    height: 48px;
    width: 48px;
    margin: 15px 0 0 15px;
    background-image: url(./../assets/logo.png);
    background-size: 48px 48px;
  }

  .sitethumbtext {
    float: left;
    text-align: left;
    padding: 15px 0 0 0;
    font-family: 'Microsoft YaHei UI';
  }

  .menucontainer {
    width: 100%;
    height: 100%;
    background-color: white;
  }

  .menuitems {
    padding: 20px 10px 0 10px;
    text-align: left;
  }

  .noselect {
    -webkit-touch-callout: none;
    -webkit-user-select: none;
    -khtml-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
  }

  .pmcopyright {
    position: fixed;
    z-index: 3;
    left: 15px;
    bottom: 15px;
    font-size: 11px;
    color: #cccccc;
    text-align: left;
  }

  .pmcopycenter {
    position: fixed;
    z-index: 3;
    width: 55px;
    left: 15px;
    bottom: 15px;
    font-size: 11px;
    color: #cccccc;
    text-align: center;
  }

  .pmcopylink {
    color: #cccccc;
    text-decoration: none;
    padding-right: 10px;
  }

    .pmcopylink:hover {
      color: #999999;
      text-decoration: underline;
    }
</style>
