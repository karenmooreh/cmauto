
import { message } from 'ant-design-vue'
var appinst = null;
const ismobile = function () {
  var __result = navigator.userAgent
    .match(/(phone|pad|pod|iPhone|iPod|ios|iPad|Android|Mobile|BlackBerry|IEMobile|MQQBrowser|JUC|Fennec|wOSBrowser|BrowserNG|WebOS|Symbian|Windows Phone)/i)
  return __result ? true : false;
}
const exit = function (appcontext) {
  var context = appcontext ? appcontext : appinst
  context.config.globalProperties.$vusername = ""
  context.config.globalProperties.$vtoken = ""
  context.config.globalProperties.$storage.remove(context.config.globalProperties.$storagekey_fep_vsignname)
  context.config.globalProperties.$storage.remove(context.config.globalProperties.$storagekey_fep_vtoken)

  context.config.globalProperties.$uimenuselectedkey = "2"
  location.href = "#/"
}
const getuserid = function () {
  appinst.config.globalProperties.$vuid =
    appinst.config.globalProperties.$storage.get(
      appinst.config.globalProperties.$storagekey_fep_vuid
    ).value
  return appinst.config.globalProperties.$vuid
}
const getusername = function () {
  appinst.config.globalProperties.$vusername =
    appinst.config.globalProperties.$storage.get(
      appinst.config.globalProperties.$storagekey_fep_vsignname
    ).value
  return appinst.config.globalProperties.$vusername
}

const getauthheaders = function (originheaders) {
  appinst.config.globalProperties.$vtoken =
    appinst.config.globalProperties.$storage.get(
      appinst.config.globalProperties.$storagekey_fep_vtoken
    ).value
  var result = { Authorization: `Bearer ${appinst.config.globalProperties.$vtoken}` }
  if (originheaders)
    for (var key in originheaders)
      result[key] = originheaders[key]
  return result
}

const axiosErrorHandle = function (error, defcallback) {

  if (error.code == "ERR_NETWORK") {
    message.error('未知的网络错误')
    location.href = '#/'
  } else if (error.response && error.response.status == 401) {
    message.warning('请重新登录')
    setTimeout(() => { location.href = '#/' }, 1000)
  } else if (defcallback && typeof defcallback == "function") {
    defcallback(error)
  }
}

const querystring = function (key) {
  const after = window.location.hash.split('?', 2)[1]
  if (after) {
    const reg = new RegExp('(^|&)' + key + '=([^&]*)(&|$)')
    const r = after.match(reg)
    return r != null ? decodeURIComponent(r[2]) : null
  }
}


export default {
  install: (app) => {
    appinst = app

    app.config.globalProperties.$backendbase =
      //"https://a.lvch.xyz"
      "http://127.0.0.1:7112"

    app.config.globalProperties.$storagekey_fep_vtoken = "fep_vtoken"
    app.config.globalProperties.$vtoken = ""
    app.config.globalProperties.$uimenuselectedkey = "db"
    app.config.globalProperties.$uimenucollapsed = false
    app.config.globalProperties.$menumappaths = {
      "null": "/",
      "console": "/console",
      "journals": "/journals",
      "devices": "/devices",
      "account": "/account",
      "logs": "/logs",
      "statistics": "/statistics",
    }

    app.config.globalProperties.$default_query_countperpage = 0x14;

    app.config.globalProperties.$default_journals_pagesize = 0x64;
    app.config.globalProperties.$default_devices_pagesize = 0x64;
    app.config.globalProperties.$default_logs_pagesize = 0x64;

    app.config.globalProperties["$ismobile"] = ismobile;
    app.config.globalProperties["$axiosErrorHandle"] = axiosErrorHandle;
    app.config.globalProperties["$getauthheaders"] = getauthheaders;
    app.config.globalProperties["$getuserid"] = getuserid;
    app.config.globalProperties["$getusername"] = getusername;
    app.config.globalProperties["$querystring"] = querystring;
    app.config.globalProperties["$exit"] = exit;
  }
}
