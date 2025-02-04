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
          <span class="fright">
            <span class="fleft" :class="cond_devicesn?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_devicesn" size="small">{{cond_devicesn?'':'设备号'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_devicesn" style="margin-right:10px; width: 140px;">
              <a-input size="small" placeholder="设备号" v-model:value="cond_devicesn_val" />
            </span>
            <span class="fleft" :class="cond_tag?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_tag" size="small">{{cond_tag?'':'标签'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_tag" style="margin-right:10px; width: 140px;">
              <a-input size="small" placeholder="标签" v-model:value="cond_tag_val" />
            </span>
            <span class="fleft" :class="cond_status?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_status" size="small">{{cond_status?'':'工作状态'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_status" style="margin-right:10px; width: 100px;">
              <a-select size="small" v-model:value="cond_status_val">
                <a-select-option value="-1">全部状态</a-select-option>
                <a-select-option value="2">工作在线</a-select-option>
                <a-select-option value="1">闲置在线</a-select-option>
                <a-select-option value="0">离线</a-select-option>
              </a-select>
            </span>
            <span>
              <a-button type="primary" size="small" @click="search()">检索</a-button>
            </span>
          </span>
          <span class="fleft">
            <a-breadcrumb>
              <a-breadcrumb-item>系统</a-breadcrumb-item>
              <a-breadcrumb-item>设备管理</a-breadcrumb-item>
            </a-breadcrumb>
          </span>
          <span class="fleft breadcrumb-desc">
            本功能模块支持中控计算设备进行热插拔，系统会自动检测热插拔连接的移动设备硬件改动
          </span>
        </div>
        <div class="clear"></div>
        <a-divider />
        <div>
          <div>
            <a-table :columns="table_columns" :data-source="table_datasource" :pagination="table_pagination" size="small">
              <template #bodyCell="{column,record}">
                <template v-if="column.key=='deviceid'">
                  <a-tag :color="record.workstatus==0x00?'#aaaaaa':record.workstatus==0x01?'#87d068':'#108ee9'">
                    {{record.deviceid}}
                  </a-tag>
                </template>
                <template v-if="column.key=='workstatus'">
                  <a-badge :color="record.workstatus==0x00?'#aaaaaa':record.workstatus==0x01?'#87d068':'#108ee9'"
                           :text="record.workstatus==0x00?'离线':record.workstatus==0x01?'闲置在线':'工作中'">
                  </a-badge>
                </template>
                <template v-if="column.key=='opts'">
                  <span class="ml5" v-if="record.workstatus==0x01">
                    <a-button type="primary" size="small">工作迁入</a-button>
                  </span>
                  <span class="ml5" v-if="record.workstatus==0x02">
                    <a-button type="primary" danger size="small">闲置迁出</a-button>
                  </span>
                  <span class="ml5">
                    <a-button size="small">编辑信息</a-button>
                  </span>
                </template>
              </template>
            </a-table>
          </div>
          <div>

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
    ExportOutlined
  } from '@ant-design/icons-vue'
  import axios from 'axios'
  import { ref, getCurrentInstance, onMounted, onUnmounted } from 'vue'

  const currentviewtip = ref('设备管理')
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


  //#region

  const cond_devicesn = ref(false)
  const cond_devicesn_val = ref(null)

  const cond_tag = ref(false)
  const cond_tag_val = ref(null)

  const cond_status = ref(false)
  const cond_status_val = ref("-1")

  //#endregion

  const table_columns = ref([
    { key: "deviceid", dataIndex: "deviceid", name: "deviceid", title: "设备ID", width: 180 },
    { key: "tag", dataIndex: "tag", name: "tag", title: "标签", width: 280 },
    { key: "workstatus", dataIndex: "workstatus", name: "workstatus", title: "工作状态", width: 180 },
    { key: "checkintime", dataIndex: "checkintime", name: "checkintime", title: "迁入时间", width: 200 },
    { key: "checkouttime", dataIndex: "checkouttime", name: "checkouttime", title: "迁出时间", width: 200 },
    { key: "worktime", dataIndex: "worktime", name: "worktime", title: "工作时长" },
    { key: "onlinetime", dataIndex: "onlinetime", name: "onlinetime", title: "在线时长" },
    { key: "opts", dataIndex: "opts", name: "opts", title: "操作", align: 'right' },
  ])
  const table_datasource = ref([
    {
      deviceid: "MI3000F81EA0D1",
      tag: "小米 MI3 PLUS/256/M001",
      workstatus: 2,
      checkintime: "2024-12-25 12:11:22",
      checkouttime: "-",
      worktime: "5小时32分",
      onlinetime: "10小时37分",
      opts: ""
    },
  ])
  const table_pagination = ref({
    total: 200,
    current: 1,
    pageSize: gconfig.$default_devices_pagesize,
    pageSizeOptions: ['10', '20', '30', '50', '100'],

  })
  const table_pagination_change = (pagination) => {
    table_pagination.value.pageSize = gconfig.$default_devices_pagesize = pagination.pageSize

  }

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

    loaddevices()
  })

  onUnmounted(() => {
    //console.log("dashboard unmounted")
  })

  const logstate = ref(false)

  const search = () => {
    searchuname.value = searchunamedisplay.value
    loaddevices(0x01)
  }

  const switchlogstate = () => {
    logstate.value = !logstate.value
    if (logstate.value) {
      intervalhandle = setInterval(loadlogs, 30000)
    } else {
      clearInterval(intervalhandle)
    }
  }


  const loaddevices = (pageindex) => {
    var __currentpageindex = pageindex && pageindex > 0x00 ? pageindex : 0x01
    axios.get(`${gconfig.$backendbase}/device/list`, {
      params: {
        pageindex: __currentpageindex,
        pagesize: gconfig.$default_journals_pagesize,
        devicesn: cond_devicesn.value && cond_devicesn_val.value ? cond_devicesn_val.value : null,
        tag: cond_tag.value && cond_tag_val.value ? cond_tag_val.value : null,
        status: cond_status.value && cond_status_val.value ? cond_status_val.value : null,
        r: Math.random()
      },
      headers: gconfig.$getauthheaders()
    }).then(resp => {
      table_datasource.value = []
      if (resp.data.result) {
        if (resp.data.data.length > 0x00) {
          for (var i = 0x00; i < resp.data.data.length; i++) {
            var __dev = resp.data.data[i]
            table_datasource.value.push({
              deviceid: __dev.sn,
              tag: __dev.tag,
              workstatus: __dev.status,
              checkintime: __dev.worktime_checkin,
              checkouttime: __dev.worktime_checkout,
              worktime: __dev.worktime_total,
              onlinetime: __dev.onlinetime,
              regtime: __dev.regtime
            })
          }

          table_pagination.value.total = resp.data.records
          table_pagination.value.current = resp.data.pageindex
        }
      }
    }).catch(err => {
      gconfig.$axiosErrorHandle(err)
    })
  }

</script>

<style src="./../assets/common.css" scoped></style>
<style scoped>
  .ml5{margin-left:5px;}
  .breadcrumb-desc{
      margin-left: 10px;
      font-size: 12px;
      padding-top: 5px;
      color: #999999;
  }
  .cond-uncheck {
    margin-right: 0px;
  }

  .cond-checked {
    margin-right: 5px;
  }
</style>
