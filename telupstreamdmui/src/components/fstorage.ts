class fstorage {
  set(key, value, date) {
    const data = {
      value: value,
      date: null
    }
    if (date) {
      switch (typeof date) {
        case 'string':
          data.date = new Date(date)
          break;
        case 'number':
          data.date = (new Date()).setSeconds((new Date()).getSeconds() + date)
          break;
        default:
          data.date = (new Date()).setSeconds((new Date()).getSeconds() + 0x278d00)
          break;
      }
    } else data.date = (new Date()).setSeconds((new Date()).getSeconds() + 0x278d00)

    localStorage.setItem(key, JSON.stringify(data))
  }
  get(key) {
    const value = localStorage.getItem(key)
    if (value) {
      const obj = JSON.parse(value)
      let d = (new Date()).valueOf()
      if (obj.date == 'permanent' || obj.date > d) {
        return { value: obj.value }
      } else {
        this.remove(key)
        return { value: null }
      }
    } else return { value: null }
  }
  remove(key) {
    localStorage.removeItem(key)
  }
  clear() {
    localStorage.clear()
  }
}
export default new fstorage()
