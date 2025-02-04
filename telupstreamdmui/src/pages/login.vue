<template>
  <div class="loginbg">
    <div>
      <video id="bgvdo" muted src="/static/loginbg.mp4" autoplay loop></video>
    </div>
    <div class="loginpad">
      <div class="loginform" :class="ismobile?'widthifmobile':'widthifpc'">
        <div class="padtitle noselect">流量充值中控系统</div>
        <div class="padsubtitle">明盛信息科技有限公司 系统版本号：v1.0.0.1</div>
        <div v-if="isenvchecked" class="loginarea">
          <div class="loginarea-line">
            <a-input ref="unameinput" v-model:value="unameval"
                     :disabled="isloginning" placeholder="请输入登录账户..."
                     @pressEnter="login()">
              <template #prefix>
                <MailOutlined />
              </template>
            </a-input>
          </div>
          <div class="loginarea-line">

          </div>
          <div class="loginarea-line">
            <a-input-password ref="upasswdinput" v-model:value="upasswdval"
                              :disabled="isloginning" placeholder="请输入登录密码.."
                              @pressEnter="login()">
              <template #prefix>
                <edit-outlined />
              </template>
            </a-input-password>
          </div>
          <div class="loginarea-line">
            <a-button type="primary" block="true" :loading="isloginning"
                      @click="login()">{{logintext}}</a-button>
          </div>
        </div>
        <div v-else class="loading">
          <LoadingOutlined />
          <a class="loading-text">正在进行本地环境检测，请稍候...</a>
        </div>
        <CPRArea type="signin" />
      </div>
    </div>
  </div>
</template>

<script setup>
  import { LoadingOutlined, UserOutlined, EditOutlined, CheckOutlined, MailOutlined } from '@ant-design/icons-vue'
  import { getCurrentInstance, ref, nextTick, onMounted } from 'vue'
  import axios from 'axios'
  import { notification, message } from 'ant-design-vue'

  const gconfig = getCurrentInstance().appContext.config.globalProperties;
  const ismobile = ref(gconfig.$ismobile())

  const unameinput = ref(null)
  const upasswdinput = ref(null)
  const unameval = ref('')
  const upasswdval = ref('')

  const state = ref(false)
  const isenvchecked = ref(false)
  const isloginning = ref(false)
  const logintext = ref(null)

  logintext.value = "登入系统"

  const login = () => {

    //location.href="/#/console"
    //return

    isloginning.value = true

    axios.post(`${gconfig.$backendbase}/auth/signin`,
      {
        username: unameval.value,
        password: upasswdval.value
      }).then(resp => {
        if (resp.data.result) {
          gconfig.$vtoken = resp.data.data
          if (gconfig.$vtoken) gconfig.$storage.set(gconfig.$storagekey_fep_vtoken, gconfig.$vtoken)
          gconfig.$uimenuselectedkey = "db"

          message.success("登录成功")

          setTimeout(() => { location.href = "#/console" }, 500)
        } else {
          isloginning.value = false
          message.error("未知的账户状态或密码错误，请联系管理员获取帮助")
        }
      }).catch(err => {
        isloginning.value = false
        message.error("系统状态异常，请联系管理员获取帮助")
        gconfig.$axiosErrorHandle(err)
      })

  }

  onMounted(() => {
    gconfig.$vtoken = gconfig.$storage.get(gconfig.$storagekey_fep_vtoken).value
    //console.log("vtoken: ",gconfig.$vtoken)
    if (gconfig.$vtoken) {
      axios.get(`${gconfig.$backendbase}/auth/verify`, {
        params: { r: Math.random() },
        headers: gconfig.$getauthheaders()
      }).then((resp) => {
        if (resp.data.result) {
          gconfig.$uimenuselectedkey = "db"
          setTimeout(() => { location.href = "#/console" }, 500)
        }
        else isenvchecked.value = true
      }).catch((error) => {
        isenvchecked.value = true
      })
    } else {
      isenvchecked.value = true
    }
  })

</script>

<style src="./../assets/common.css" scoped></style>
<style scoped>
  #bgvdo {
    position: fixed;
    right: 0;
    bottom: 0;
    min-width: 100%;
    min-height: 100%;
    width: auto;
    height: auto;
    z-index: -1;
  }
</style>
