<template>
  <a-layout>
    <a-layout-sider v-if="winwidth>winwidthmodelimit&&!ismobile" v-model:collapsed="collapsed" :trigger="null" collapsible class="sidemenucontainer">
      <ms-sidemenus :selectedkey="$uimenuselectedkey" v-model:collapsed="collapsed" />
    </a-layout-sider>
    <a-drawer v-else width="60%" placement="left" :title="'仪表盘'" :open="drawstate" :closable="false" @close="closeDraw">
      <template #extra>
        <menu-fold-outlined class="trigger" @click="closeDraw"></menu-fold-outlined>
      </template>
      <ms-sidemenus :selectedkey="$uimenuselectedkey" :collapsed="true" />
    </a-drawer>
    <a-layout-header class="headcontainer">
      <div class="headright">
        <span class="headmenu noselect">
          <ms-headmenus />
        </span>
      </div>
      <div v-if="winwidth>winwidthmodelimit&&!ismobile">
        <span :class="winwidth>winwidthmodelimit&&collapsed?'pcleftcollapsed':'pcleft'"></span>
        <menu-unfold-outlined v-if="collapsed" class="trigger" @click="switchmenu"></menu-unfold-outlined>
        <menu-fold-outlined v-else class="trigger" @click="switchmenu"></menu-fold-outlined>
        <span style="margin-left:10px;color:#999999" class=noselect>{{currentviewtip}}</span>
      </div>
      <div v-else>
        <menu-unfold-outlined class="trigger" @click="switchmenu"></menu-unfold-outlined>
        <span style="margin-left:10px;color:#999999" class=noselect>{{currentviewtip}}</span>
      </div>
    </a-layout-header>
    <a-layout-content class="acontent" :class="winwidth>winwidthmodelimit&&!ismobile?collapsed?'acontentlite':'acontentfull':null">
      <div :class="!ismobile?(winwidth>winwidthmodelimit?'vcontentlite':'vcontentfull'):'vcontentfull'">
        <div>
          <span class="fleft">
            <a-breadcrumb>
              <a-breadcrumb-item>系统</a-breadcrumb-item>
              <a-breadcrumb-item>登录信息</a-breadcrumb-item>
            </a-breadcrumb>
          </span>
        </div>
        <div class="clear"></div>
        <a-divider />
        <div>
          <div v-if="false">
            <a-table :columns="table_columns" :data-source="table_datasource" :pagination="{ pageSize: 100 }" size="small">
              <template #bodyCell="{column,record}">
                <template v-if="column.key=='vaddr'">
                  <span>{{record.vaddr}}</span>
                  <span style="margin-left:5px">
                    <a :href="record.vaddr" target="_blank">
                      <ExportOutlined />
                    </a>
                  </span>
                </template>
                <template v-else-if="column.key=='tags'">
                  <a-tag v-for="tag in record.tags">
                    {{tag}}
                  </a-tag>
                </template>
              </template>
            </a-table>
          </div>
          <div style="width:50%; margin:0 auto; padding-top:20px;">
            <a-row style="margin-top:20px;">
              <a-col :span="8">
                <span style="line-height:30px;">当前登录密码</span>
              </a-col>
              <a-col :span="16">
                <a-input-password></a-input-password>
              </a-col>
            </a-row>
            <a-row style="margin-top:40px;">
              <a-col :span="8">
                <span style="line-height:30px;">新的登录密码</span>
              </a-col>
              <a-col :span="16">
                <a-input-password></a-input-password>
              </a-col>
            </a-row>
            <a-row style="margin-top:20px;">
              <a-col :span="8">
                <span style="line-height:30px;">确认登录密码</span>
              </a-col>
              <a-col :span="16">
                <a-input-password></a-input-password>
              </a-col>
            </a-row>
            <a-row style="margin-top:20px; margin-bottom:100px;">
              <a-col :span="8"></a-col>
              <a-col :span="16">
                <a-button type="primary" style="width:100%">更新当前登录密码</a-button>
              </a-col>
            </a-row>
          </div>
        </div>
        <ms-cntfooter />
      </div>
    </a-layout-content>
  </a-layout>
</template>
<script setup>
  import {
    MenuFoldOutlined, MenuUnfoldOutlined, CaretDownOutlined, ReadOutlined,
    CloudDownloadOutlined, HistoryOutlined, QuestionCircleOutlined, PlusOutlined,
    ExportOutlined, 
  } from '@ant-design/icons-vue'
  import axios from 'axios'
  import { ref, getCurrentInstance, onMounted, onUnmounted } from 'vue'

  const currentviewtip = ref('登录信息')
  const winwidthmodelimit = 1320
  const drawstate = ref(false)
  const winwidth = ref(0)

  const ginstance = getCurrentInstance()
  const gconfig = ginstance.appContext.config.globalProperties;
  const collapsed = ref(ginstance.appContext.config.globalProperties.$uimenucollapsed)
  const ismobile = ref(ginstance.appContext.config.globalProperties.$ismobile())

  var intervalhandle = null
  const updatetimestamp = ref("")
  const searchuname = ref("")
  const searchunamedisplay = ref("")

  const table_columns = ref([
    { key: "regtime", dataIndex: "regtime", name: "regtime", title: "访问时间", width: 180 },
    { key: "source", dataIndex: "source", name: "source", title: "节点源", width: 200 },
    { key: "user", dataIndex: "username", name: "username", title: "用户", width: 180 },
    { key: "tags", dataIndex: "tags", name: "tags", title: "标签", width: 200 },
    { key: "terinfo", dataIndex: "terinfo", name: "terinfo", title: "源IP端口", width: 200 },
    { key: "vaddr", dataIndex: "vaddr", name: "vaddr", title: "访问资源" },
  ])
  const table_datasource = ref([

  ])

  const whenWindowResize = () => { winwidth.value = window.innerWidth }
  const switchmenu = () => {
    if (!ismobile.value && winwidth.value > winwidthmodelimit) ginstance.appContext.config.globalProperties.$uimenucollapsed = collapsed.value = !collapsed.value
    else showDraw()
  }
  const showDraw = () => { drawstate.value = true }
  const closeDraw = () => { drawstate.value = false }

  onMounted(() => {
    whenWindowResize()
    window.addEventListener('resize', whenWindowResize)

    //loadlogs()
  })

  onUnmounted(() => {
    //console.log("dashboard unmounted")
  })

  const logstate = ref(false)

  const search = () => {
    searchuname.value = searchunamedisplay.value
    loadlogs()
  }

  const switchlogstate = () => {
    logstate.value = !logstate.value
    if (logstate.value) {
      intervalhandle = setInterval(loadlogs, 30000)
    } else {
      clearInterval(intervalhandle)
    }
  }


  const loadlogs = () => {
    axios.get(`${gconfig.$backendbase}/log/list`, {
      params: {
        uname: searchuname.value && searchuname.value.length > 0x00 ? searchuname.value : null,
        r: Math.random()
      },
      headers: gconfig.$getauthheaders()
    }).then(resp => {
      table_datasource.value = []
      if (resp.data.ret) {
        updatetimestamp.value = resp.data.timestamp
        if (resp.data.dat.length > 0x00) {
          for (var i = 0x00; i < resp.data.dat.length; i++) {
            var __log = resp.data.dat[i]
            table_datasource.value.push({
              id: __log.id,
              sourceid: __log.nodeid,
              source: __log.nodename,
              userid: __log.userid,
              username: __log.username,
              tags: __log.tags,
              vaddr: __log.url,
              terinfo: __log.source,
              regtime: __log.regtime
            })
          }
        }
      }
    }).catch(err => {
      gconfig.$axiosErrorHandle(err)
    })
  }

</script>

<style src="./../assets/common.css" scoped></style>
<style scoped>
</style>
