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
            <span class="fleft" :class="cond_recordtime?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_recordtime" size="small">{{cond_recordtime?'':'记录日期'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_recordtime" style="margin-right:10px; width:320px;">
              <a-range-picker placeholder="起止" v-model:value="cond_recordtime_val" size="small"
                              :allowEmpty="[true, true]" show-time @ok="accepttime_ok" />
            </span>
            <span class="fleft" :class="cond_source?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_source" size="small">{{cond_source?'':'日志对象'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_source" style="margin-right:10px; width: 140px;">
              <a-input size="small" placeholder="日志对象" v-model:value="cond_source_val" />
            </span>
            <span class="fleft" :class="cond_logtype?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_logtype" size="small">{{cond_logtype?'':'对象类型'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_logtype" style="margin-right:10px; width: 100px;">
              <a-select size="small" v-model:value="cond_logtype_val">
                <a-select-option value="-1">全部类型</a-select-option>
                <a-select-option value="0">系统日志</a-select-option>
                <a-select-option value="1">设备日志</a-select-option>
                <a-select-option value="2">充值号码</a-select-option>
                <a-select-option value="3">任务号码</a-select-option>
                <a-select-option value="255">其他类型</a-select-option>
              </a-select>
            </span>
            <span class="fleft" :class="cond_intro?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_intro" size="small">{{cond_intro?'':'日志摘要'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_intro" style="margin-right:10px; width: 140px;">
              <a-input size="small" placeholder="日志摘要" v-model:value="cond_intro_val" />
            </span>
            <span class="fleft" :class="cond_details?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_details" size="small">{{cond_details?'':'日志详情'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_details" style="margin-right:10px; width: 140px;">
              <a-input size="small" placeholder="日志详情" v-model:value="cond_details_val" />
            </span>
            <span>
              <a-button type="primary" size="small" @click="search()">检索</a-button>
            </span>
          </span>
          <span class="fright" style="margin-right:10px;">
            <a-switch :checked="logstate" @change="switchlogstate"
                      checked-children="实时日志" un-checked-children="历史日志" />
          </span>
          <span v-if="logstate" class="fright" style="margin-right:10px;">
            <span class="fleft" style="line-height:22px;">更新时间戳：</span>
            <span class="fleft">
              <a-tag color="#108ee9">{{updatetimestamp}}</a-tag>
            </span>
          </span>
          <span class="fleft">
            <a-breadcrumb>
              <a-breadcrumb-item>系统</a-breadcrumb-item>
              <a-breadcrumb-item>运行日志</a-breadcrumb-item>
            </a-breadcrumb>
          </span>
        </div>
        <div class="clear"></div>
        <a-divider />
        <div>
          <div>
            <a-table :columns="table_columns" :data-source="table_datasource" :pagination="table_pagination" size="small"
                     @change="table_pagination_change">
              <template #bodyCell="{column,record}">
                <template v-if="column.key=='type'">
                  <a-tag :color="record.type==0x00?'#333333':record.type==0x01?'#52c41a':record.type==0x02?'#55acee':record.type==0x03?'#3b5999':'#aaaaaa'">
                    {{record.type==0x00?'系统':record.type==0x01?'设备':record.type==0x02?'充值号码':record.type==0x03?'任务号码':'#aaaaaa'}}
                  </a-tag>
                </template>
                <template v-if="column.key=='source'">
                  <a-tag :color="record.type==0x00?'#333333':record.type==0x01?'#52c41a':record.type==0x02?'#55acee':record.type==0x03?'#3b5999':'#aaaaaa'">
                    {{record.source}}
                  </a-tag>
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

  const currentviewtip = ref('运行日志')
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


  //#regin

  const cond_recordtime = ref(false)
  const cond_recordtime_val = ref([null, null])

  const cond_source = ref(false)
  const cond_source_val = ref(null)

  const cond_logtype = ref(false)
  const cond_logtype_val = ref("-1")

  const cond_intro = ref(false)
  const cond_intro_val = ref(null)

  const cond_details = ref(false)
  const cond_details_val = ref(null)

  //#endregion

  const table_columns = ref([
    { key: "regtime", dataIndex: "regtime", name: "regtime", title: "记录时间", width: 180 },
    { key: "source", dataIndex: "source", name: "source", title: "对象", width: 200 },
    { key: "type", dataIndex: "type", name: "type", title: "对象类型" },
    { key: "title", dataIndex: "title", name: "title", title: "日志摘要" },
    { key: "details", dataIndex: "details", name: "details", title: "详情" },
  ])
  const table_datasource = ref([
    {
      regtime: "2024-12-25 12:22:37",
      source: "[SYSTEM0]",
      type: 0,
      title: "设备热插拔扫描",
      details: "已进行本轮热插拔接口扫描，未发现新增设备，未发现设备掉线"
    },
  ])
  const table_pagination = ref({
    total: 200,
    current: 1,
    pageSize: gconfig.$default_journals_pagesize,
    pageSizeOptions: ['10', '20', '30', '50', '100'],

  })
  const table_pagination_change = (pagination) => {
    table_pagination.value.pageSize = gconfig.$default_logs_pagesize = pagination.pageSize

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

    loadlogs(0x01)
  })

  onUnmounted(() => {
    //console.log("dashboard unmounted")
  })

  const logstate = ref(false)

  const search = () => {
    searchuname.value = searchunamedisplay.value
    loadlogs(0x01)
  }

  const switchlogstate = () => {
    logstate.value = !logstate.value
    if (logstate.value) {
      intervalhandle = setInterval(loadlogs, 30000)
    } else {
      clearInterval(intervalhandle)
    }
  }


  const loadlogs = (pageindex) => {
    var __currentpageindex = pageindex && pageindex > 0x00 ? pageindex : 0x01
    axios.get(`${gconfig.$backendbase}/log/list`, {
      params: {
        pageindex: __currentpageindex,
        pagesize: gconfig.$default_logs_pagesize,
        source: cond_source.value && cond_source_val.value ? cond_source_val.value : null,
        sourcetype: cond_logtype.value && cond_logtype_val.value && cond_logtype_val.value != "-1" ? cond_logtype_val.value : null,
        intro: cond_intro.value && cond_intro_val.value ? cond_intro_val.value : null,

        r: Math.random()
      },
      headers: gconfig.$getauthheaders()
    }).then(resp => {
      table_datasource.value = []
      if (resp.data.result) {
        updatetimestamp.value = resp.data.timestamp
        if (resp.data.data.length > 0x00) {
          for (var i = 0x00; i < resp.data.data.length; i++) {
            var __log = resp.data.data[i]
            table_datasource.value.push({
              id: __log.id,
              source: __log.source,
              type: __log.type,
              title: __log.intro,
              details: __log.details,
              regtime: __log.regtime
            })

            table_pagination.value.total = resp.data.records
            table_pagination.value.current = resp.data.pageindex
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
  .cond-uncheck {
    margin-right: 0px;
  }

  .cond-checked {
    margin-right: 5px;
  }
</style>
