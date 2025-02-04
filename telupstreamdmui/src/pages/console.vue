<template>
  <a-layout>
    <a-layout-sider v-if="winwidth>winwidthmodelimit&&!ismobile" v-model:collapsed="collapsed" :trigger="null" collapsible class="sidemenucontainer">
      <ms-sidemenus ref="navref1" :selectedkey="$uimenuselectedkey" v-model:collapsed="collapsed" />
    </a-layout-sider>
    <a-drawer v-else width="60%" placement="left" :title="'控制台'" :open="drawstate" :closable="false" @close="closeDraw">
      <template #extra>
        <menu-fold-outlined class="trigger" @click="closeDraw"></menu-fold-outlined>
      </template>
      <ms-sidemenus ref="navref1" :selectedkey="$uimenuselectedkey" :collapsed="true" />
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
              <a-breadcrumb-item>仪表盘</a-breadcrumb-item>
            </a-breadcrumb>
          </span>
        </div>
        <div class="clear"></div>
        <a-divider orientation="left" orientation-margin="0px">
          <span class="divider-text">系统数据概况</span>
        </a-divider>
        <div>
          <a-flex ref="navref2" gap="middle" :vertical="ismobile">
            <div :style="{width:ismobile?'100%':'25%'}">
              <a-card size="small" title="当前工作设备">
                <span class="introtext mlr5">10</span>
                &nbsp;
                <span class="mlr5" style="color: #108ee9;font-weight:bold;">[工作]</span>
                &nbsp;
                <span class="introtext mlr5">/</span>
                &nbsp;
                <span class="introtext mlr5">10</span>
                &nbsp;
                <span class="mlr5" style="color: #87d068;font-weight:bold;">[在线]</span>
              </a-card>
            </div>
            <div :style="{width:ismobile?'100%':'25%'}">
              <a-card size="small" title="已处理订单总数">
                <span class="introtext">13,147</span>
                <span class="mlr5" style="color: #999999;font-weight:bold;">笔</span>
              </a-card>
            </div>
            <div :style="{width:ismobile?'100%':'25%'}">
              <a-card size="small" title="累计有效订单">
                <span class="introtext">12,311</span>
                <span class="mlr5" style="color: #999999;font-weight:bold;">笔</span>
              </a-card>
            </div>
            <div :style="{width:ismobile?'100%':'25%'}">
              <a-card size="small" title="失败及拦截订单">
                <span class="introtext">836</span>
                <span class="mlr5" style="color: #999999;font-weight:bold;">笔</span>
              </a-card>
            </div>
          </a-flex>
        </div>
        <div class="clear"></div>
        <a-divider orientation="left" orientation-margin="0px">
          <span class="divider-text">概述统计图表</span>
        </a-divider>
        <div>
          <v-chart ref="navref3" class="introchart" :option="chart_option" autoresize/>
        </div>
        <div class="clear"></div>
        <a-divider orientation="left" orientation-margin="0px">
          <span class="divider-text">实时任务信息</span>
        </a-divider>
        <div>
          <div>
            <a-table ref="navref4" :columns="table_columns" :data-source="table_datasource" :pagination="{ pageSize: 100 }" size="small">
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
          <div>

          </div>
        </div>
        <ms-cntfooter />
      </div>
    </a-layout-content>
    <a-tour v-model:current="navcurrent" :open="navopen" :steps="navsteps" @close="navhandleOpen(false)"></a-tour>
  </a-layout>
</template>
<script setup>
  import {
    MenuFoldOutlined, MenuUnfoldOutlined, CaretDownOutlined, ReadOutlined,
    CloudDownloadOutlined, HistoryOutlined, QuestionCircleOutlined, PlusOutlined,
    ExportOutlined
  } from '@ant-design/icons-vue'
  import axios from 'axios'
  import { ref, getCurrentInstance, onMounted, onUnmounted, provide, createVNode } from 'vue'
  import { use } from 'echarts/core'
  import { BarChart } from 'echarts/charts'
  import { CanvasRenderer } from 'echarts/renderers'
  import { TitleComponent, TooltipComponent, LegendComponent, GridComponent } from 'echarts/components'
  import VChart, { THEME_KEY } from 'vue-echarts'

  
  const navopen = ref(false)
  const navcurrent = ref(0)
  const navref1 = ref(null)
  const navref2 = ref(null)
  const navref3 = ref(null)
  const navref4 = ref(null)
  const navref5 = ref(null)
  const navsteps = [
    {
      title: '欢迎来到明盛流量充值中控系统',
      description: '中控系统是本地控制端系统，提供了以WEB为基础的跨平台设备管理界面。您是第一次使用本中控系统，请简单让我为您讲解系统功能。',
      placement: 'right',
    },
    {
      title: '这里是中控系统的功能导航',
      description: '中控系统导航提供了包括控制台（统览中控系统整体运行概况）、充值台账、设备管理、运行日志、统计分析（暂未设计）等功能模块。',
      placement: 'right',
      target: () => navref1.value && navref1.value.$el
    },
    {
      title: '控制台-系统数据概况',
      description: '在控制台页面的系统数据概况中，您可以看到当前控制系统的在线/工作设备数、已处理订单、有效及失败、拦截订单等数据。',
      placement: 'top',
      target: () => navref2.value && navref2.value.$el
    },
    {
      title: '控制台-概述统计图表',
      description: '在控制台页面的概述统计图表中，您可以看到当前控制系统按周统计各类状态的充值订单任务数据。',
      placement: 'top',
      target: () => navref3.value && navref3.value.$el
    },
    {
      title: '控制台-实时任务信息',
      description: '在控制台页面的实时任务信息中，您可以看到当前控制系统来自北向平台的订单来单处理数据，该处数据是实时的哦。',
      placement: 'top',
      target: () => navref4.value && navref4.value.$el
    },
    {
      title: '还请继续探索',
      description: '明盛流量充值中控系统的各类功能仍在建设中，欢迎各方提供宝贵建议及意见。',
      placement: 'top',
    }
  ]
  const navhandleOpen = val => {
    navopen.value = val
  }


  //provide(THEME_KEY, 'dark')

  use([
    CanvasRenderer,
    BarChart,
    TitleComponent,
    TooltipComponent,
    LegendComponent,
    GridComponent,
  ])

  const chart_option = ref({
    tooltip:{
      trigger: "axis",
      axisPointer: {
        type: "shadow"
      }
    },
    legend: {},
    grid: {
      left: '3%',
      right: '4%',
      bottom: '3%',
      containLabel: true
    },
    xAxis: [
      {
        type: 'category',
        data: ['周一', '周二', '周三', '周四', '周五', '周六', '周日']
      }
    ],
    yAxis: [
      {
        type: 'value'
      }
    ],
    series: [
      {
        name: '有效订单',
        type: 'bar',
        color: ["#8fd5f3"],
        stack: 'total',
        emphasis: { focus: 'series' },
        data: [120,111,183,187,180,156,133],
      },
      {
        name: '失效订单',
        type: 'bar',
        color: ["#ff7070"],
        stack: 'total',
        emphasis: { focus: 'series' },
        data: [23,21,73,87,10,16,13],
      },
      {
        name: '拦截订单',
        type: 'bar',
        color: ["#999999"],
        stack: 'total',
        emphasis: { focus: 'series' },
        data: [11,9,7,8,10,6,3],
      },
    ]
  })



  const currentviewtip = ref('仪表盘')
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
    { key: "regtime", dataIndex: "regtime", name: "regtime", title: "来单时间", width: 180 },
    { key: "taskno", dataIndex: "taskno", name: "taskno", title: "任务ID", width: 180 },
    { key: "deviceid", dataIndex: "deviceid", name: "deviceid", title: "设备ID", width: 200 },
    { key: "billingno", dataIndex: "billingno", name: "billingno", title: "充值订单号", width: 180 },
    { key: "phonenum", dataIndex: "phonenum", name: "phonenum", title: "手机号码", width: 200 },
    { key: "amount", dataIndex: "amount", name: "amount", title: "订单金额", width: 200 },
    { key: "taskphonenum", dataIndex: "taskphonenum", name: "taskphonenum", title: "任务号码" },
    { key: "taskphonecount", dataIndex: "taskphonecount", name: "taskphonecount", title: "号码日计次" },
    { key: "taskstatus", dataIndex: "taskstatus", name: "taskstatus", title: "任务状态" },
    { key: "starttime", dataIndex: "starttime", name: "starttime", title: "执行时间" },
  ])
  const table_datasource = ref([
    {
      regtime:"2024-12-25 13:21:33",
      taskno: "1599624303720241225132133157",
      deviceid: "A00000F81EA0DD",
      billingno: "20241225132133157",
      phonenum: "15996243037",
      amount: "10.00",
      taskphonenum: "18705194898",
      taskphonecount: 2,
      taskstatus: "充值中",
      starttime: "2024-12-25 13:22:12"
    },
    {
      regtime:"2024-12-25 13:21:33",
      taskno: "1599624303720241225132133157",
      deviceid: "A00000F81EA0D1",
      billingno: "20241225132133157",
      phonenum: "15051846979",
      amount: "10.00",
      taskphonenum: "13914736465",
      taskphonecount: 0,
      taskstatus: "充值中",
      starttime: "2024-12-25 13:22:12"
    },
    {
      regtime:"2024-12-25 13:21:33",
      taskno: "1599624303720241225132133157",
      deviceid: "A00000F81EA0D2",
      billingno: "20241225132133157",
      phonenum: "15996242305",
      amount: "10.00",
      taskphonenum: "15996340155",
      taskphonecount: 1,
      taskstatus: "充值中",
      starttime: "2024-12-25 13:22:12"
    },
    {
      regtime:"2024-12-25 13:21:33",
      taskno: "1599624303720241225132133157",
      deviceid: "A00000F81EA0D3",
      billingno: "20241225132133157",
      phonenum: "18251971548",
      amount: "10.00",
      taskphonenum: "15150645509",
      taskphonecount: 3,
      taskstatus: "充值中",
      starttime: "2024-12-25 13:22:12"
    },
    {
      regtime:"2024-12-25 13:21:33",
      taskno: "1599624303720241225132133157",
      deviceid: "A00000F81EA0D4",
      billingno: "20241225132133157",
      phonenum: "15996487162",
      amount: "10.00",
      taskphonenum: "18761863442",
      taskphonecount: 1,
      taskstatus: "充值中",
      starttime: "2024-12-25 13:22:12"
    },
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

    var __navreaded = gconfig.$storage.get("navreaded")
    if(!__navreaded.value) {
      gconfig.$storage.set("navreaded", true)
      navhandleOpen(true)
    }

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
  .divider-text{
      font-size:12px;
      font-weight:bold;
      color:#999;
  }
  .introchart{
      height:300px;
  }
  .introtext{
      font-size:30px;
      font-weight:bold;
  }
  .mlr5{
      margin:0 5px 0 5px;
  }
</style>
