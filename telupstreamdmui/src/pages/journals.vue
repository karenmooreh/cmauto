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
            <span class="fleft" :class="cond_billingno?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_billingno" size="small">{{cond_billingno?'':'订单号'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_billingno" style="margin-right:10px; width: 140px;">
              <a-input size="small" placeholder="订单号" v-model:value="cond_billingno_val" />
            </span>
            <span class="fleft" :class="cond_accepttime?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_accepttime" size="small">{{cond_accepttime?'':'来单日期'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_accepttime" style="margin-right:10px; width:320px;">
              <a-range-picker placeholder="起止" v-model:value="cond_accepttime_val" size="small"
                              :allowEmpty="[true, true]" show-time @ok="accepttime_ok" />
            </span>
            <span class="fleft" :class="cond_phonenum?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_phonenum" size="small">{{cond_phonenum?'':'充值号码'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_phonenum" style="margin-right:10px; width:100px;">
              <a-input size="small" placeholder="充值号码" v-model:value="cond_phonenum_val" />
            </span>
            <span class="fleft" :class="cond_amount?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_amount" size="small">{{cond_amount?'':'充值金额'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_amount" style="margin-right:10px; width:100px;">
              <a-input-number size="small" placeholder="充值金额" v-model:value="cond_amount_val" />
            </span>
            <span class="fleft" :class="cond_taskid?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_taskid" size="small">{{cond_taskid?'':'任务ID'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_taskid" style="margin-right:10px; width:100px;">
              <a-input size="small" placeholder="任务ID" v-model:value="cond_taskid_val" />
            </span>
            <span class="fleft" :class="cond_taskphonenum?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_taskphonenum" size="small">{{cond_taskphonenum?'':'任务号码'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_taskphonenum" style="margin-right:10px; width:100px;">
              <a-input size="small" placeholder="任务号码" v-model:value="cond_taskphonenum_val" />
            </span>
            <span class="fleft" :class="cond_devicesn?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_devicesn" size="small">{{cond_devicesn?'':'设备号'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_devicesn" style="margin-right:10px; width:100px;">
              <a-input size="small" placeholder="设备号" v-model:value="cond_devicesn_val" />
            </span>
            <span class="fleft" :class="cond_status?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_status" size="small">{{cond_status?'':'充值状态'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_status" style="margin-right:10px; width:100px;">
              <a-select size="small" v-model:value="cond_status_val">
                <a-select-option value="-1">全部状态</a-select-option>
                <a-select-option value="0">等待充值</a-select-option>
                <a-select-option value="1">正在充值</a-select-option>
                <a-select-option value="2">充值成功</a-select-option>
                <a-select-option value="3">拦截成功</a-select-option>
                <a-select-option value="4">充值失败</a-select-option>
              </a-select>
            </span>
            <span class="fleft" :class="cond_isptradeno?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_isptradeno" size="small">{{cond_isptradeno?'':'运营商回单'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_isptradeno" style="margin-right:10px; width:120px;">
              <a-input size="small" placeholder="运营商回单" v-model:value="cond_isptradeno_val" />
            </span>
            <span class="fleft" :class="cond_runtime?'cond-checked':'cond-uncheck'">
              <a-checkbox v-model:checked="cond_runtime" size="small">{{cond_runtime?'':'任务时间'}}</a-checkbox>
            </span>
            <span class="fleft" v-if="cond_runtime" style="margin-right:10px; width:320px;">
              <a-range-picker placeholder="起止" size="small" :allowEmpty="[true, true]"
                              v-model:value="cond_runtime_val" show-time @ok="runtime_ok" />
            </span>
            <span style="margin-left:20px;">
              <a-button type="primary" size="small" @click="search()">检索</a-button>
            </span>
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
              <a-breadcrumb-item>充值台账</a-breadcrumb-item>
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
                <template v-if="column.key=='taskstatus'">
                  <span>
                    <a-badge :color="record.taskstatus==0x00?'#aaaaaa':record.taskstatus==0x01?'blue':record.taskstatus==0x02?'green':record.taskstatus==0x03?'red':'#333333'"
                             :text="record.taskstatus==0x00?'等待充值':record.taskstatus==0x01?'充值中':record.taskstatus==0x02?'充值成功':record.taskstatus==0x03?'充值失败':'拦截成功'"/>
                  </span>
                </template>
                <template v-if="column.key=='amount'">
                  <span>
                    ￥ {{record.amount}}
                  </span>
                </template>
                <template v-if="column.key=='taskphonenum'">
                  <a-tag :color="record.taskcount==0x00?'blue':record.taskcount==0x01?'yellow':record.taskcount==0x02?'#ff6600':'#ff0000'"><b>{{record.taskphonenum}}</b> [计 <b>{{record.taskcount}}</b> 次]</a-tag>
                </template>
                <template v-if="column.key=='billingno'">
                  <a-tag :color="record.taskstatus==0x00?'#aaaaaa':record.taskstatus==0x01?'#2db7f5':record.taskstatus==0x02?'#87d068':record.taskstatus==0x03?'red':'#333333'">
                    {{record.billingno}}
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

  const currentviewtip = ref('充值台账')
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

  //search conditions
  //#region 

  const cond_billingno = ref(false)
  const cond_billingno_val = ref(null)

  const cond_accepttime = ref(false)
  const cond_accepttime_val = ref([null, null])

  const cond_phonenum = ref(false)
  const cond_phonenum_val = ref(null)

  const cond_amount = ref(false)
  const cond_amount_val = ref(null)

  const cond_taskid = ref(false)
  const cond_taskid_val = ref(null)

  const cond_taskphonenum = ref(false)
  const cond_taskphonenum_val = ref(null)

  const cond_devicesn = ref(false)
  const cond_devicesn_val = ref(null)

  const cond_status = ref(false)
  const cond_status_val = ref("-1")

  const cond_isptradeno = ref(false)
  const cond_isptradeno_val = ref(null)

  const cond_runtime = ref(false)
  const cond_runtime_val = ref([null, null])

  //#endregion

  const table_columns = ref([
    { key: "billingno", dataIndex: "billingno", name: "billingno", title: "订单号", width: 180 },
    { key: "regtime", dataIndex: "regtime", name: "regtime", title: "来单时间", width: 180 },
    { key: "phonenum", dataIndex: "phonenum", name: "phonenum", title: "充值号码" },
    { key: "amount", dataIndex: "amount", name: "amount", title: "充值金额" },
    { key: "taskno", dataIndex: "taskno", name: "taskno", title: "任务ID", width: 200 },
    { key: "deviceid", dataIndex: "deviceid", name: "deviceid", title: "设备ID" },
    { key: "taskphonenum", dataIndex: "taskphonenum", name: "taskphonenum", title: "任务号码" },
    { key: "taskstatus", dataIndex: "taskstatus", name: "taskstatus", title: "任务状态" },
    { key: "sptradeno", dataIndex: "sptradeno", name: "sptradeno", title: "运营商订单", width: 200 },
    { key: "starttime", dataIndex: "starttime", name: "starttime", title: "执行时间" },
  ])
  const table_datasource = ref([
    //{
    //  billingno: "20241225132133157",
    //  regtime: "2024-12-25 13:21:33",
    //  phonenum: "15051846979",
    //  amount: "10.00",
    //  taskno: "1599624303720241225132133157",
    //  deviceid: "A00000F81EA0D1",
    //  taskphonenum: "13914736465",
    //  taskstatus: 0,
    //  sptradeno: "",
    //  taskcount: 1,
    //  starttime: "-"
    //},
  ])
  const table_pagination = ref({
    total: 200,
    current: 1,
    pageSize: gconfig.$default_journals_pagesize,
    pageSizeOptions: ['10', '20', '30', '50', '100'],
    
  })
  const table_pagination_change = (pagination) => {
    table_pagination.value.pageSize = gconfig.$default_journals_pagesize = pagination.pageSize

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

    loadjournals(0x01)
  })

  onUnmounted(() => {
    //console.log("dashboard unmounted")
  })

  const logstate = ref(false)

  const search = () => {
    searchuname.value = searchunamedisplay.value
    loadjournals(table_pagination.value.current)
  }

  const switchlogstate = () => {
    logstate.value = !logstate.value
    if (logstate.value) {
      intervalhandle = setInterval(loadjournals, 30000)
    } else {
      clearInterval(intervalhandle)
    }
  }


  const loadjournals = (pageindex) => {
    var __currentpageindex = pageindex && pageindex > 0x00 ? pageindex : 0x01
    axios.get(`${gconfig.$backendbase}/task/list`, {
      params: {
        pageindex: __currentpageindex,
        pagesize: gconfig.$default_journals_pagesize,
        billingno: cond_billingno.value && cond_billingno_val.value ? cond_billingno_val.value : null,
        accepttime_start: cond_accepttime.value && cond_accepttime_val.value[0x00] ?
          `${cond_accepttime_val.value[0x00].$y}-${cond_accepttime_val.value[0x00].$M}-${cond_accepttime_val.value[0x00].$D} ${cond_accepttime_val.value[0x00].$H}:${cond_accepttime_val.value[0x00].$m}:${cond_accepttime_val.value[0x00].$s}` : null,
        accepttime_end: cond_accepttime.value && cond_accepttime_val.value[0x01] ?
          `${cond_accepttime_val.value[0x01].$y}-${cond_accepttime_val.value[0x01].$M}-${cond_accepttime_val.value[0x01].$D} ${cond_accepttime_val.value[0x01].$H}:${cond_accepttime_val.value[0x01].$m}:${cond_accepttime_val.value[0x01].$s}` : null,
        topup_phonenum: cond_phonenum.value && cond_phonenum_val.value ? cond_phonenum_val.value : null,
        topup_amount: cond_amount.value && cond_amount_val.value ? cond_amount_val.value : null,
        task_id: cond_taskid.value && cond_taskid_val.value ? cond_taskid_val.value : null,
        task_phonenum: cond_taskphonenum.value && cond_taskphonenum_val.value ? cond_taskphonenum_val.value : null,
        devicesn: cond_devicesn.value && cond_devicesn_val.value ? cond_devicesn_val.value : null,
        task_status: cond_status.value && cond_status_val.value ? cond_status_val.value : null,
        isp_tradeno: cond_isptradeno.value && cond_isptradeno_val.value ? cond_isptradeno_val.value : null,
        task_runtime_start: cond_runtime.value && cond_runtime_val.value[0x00] ?
          `${cond_runtime_val.value[0x00].$y}-${cond_runtime_val.value[0x00].$M}-${cond_runtime_val.value[0x00].$D} ${cond_runtime_val.value[0x00].$H}:${cond_runtime_val.value[0x00].$m}:${cond_runtime_val.value[0x00].$s}` : null,
        task_runtime_end: cond_runtime.value && cond_runtime_val.value[0x01] ?
          `${cond_runtime_val.value[0x01].$y}-${cond_runtime_val.value[0x01].$M}-${cond_runtime_val.value[0x01].$D} ${cond_runtime_val.value[0x01].$H}:${cond_runtime_val.value[0x01].$m}:${cond_runtime_val.value[0x01].$s}` : null,
        r: Math.random()
      },
      headers: gconfig.$getauthheaders()
    }).then(resp => {
      table_datasource.value = []
      if (resp.data.result) {

        if (resp.data.data.length > 0x00) {
          for (var i = 0x00; i < resp.data.data.length; i++) {
            var __task = resp.data.data[i]
            
            table_datasource.value.push({
              id: __task.id,
              billingno: __task.billingno,
              regtime: __task.accepttime,
              phonenum: __task.topup_phonenum,
              amount: __task.topup_amount,
              taskno: __task.task_id,
              deviceid: __task.devicesn,
              taskphonenum: __task.task_phonenum,
              taskstatus: __task.task_status,
              taskcount: 0x00,
              sptradeno: __task.isp_tradeno,
              starttime: __task.task_runtime
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
  .cond-uncheck{
      margin-right: 0px;
  }
  .cond-checked{
      margin-right: 5px;
  }
</style>
