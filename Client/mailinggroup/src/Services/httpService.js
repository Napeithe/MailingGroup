import { serverUrl } from './appconst'
import { Modal } from 'antd'
import axios from 'axios'
import { logout, getToken } from './accountService'

const qs = require('qs')

const http = axios.create({
  baseURL: serverUrl,
  timeout: 30000,
  paramsSerializer: function (params) {
    return qs.stringify(params, {
      encode: false
    })
  }
})

http.interceptors.request.use(
  function (config) {
    const token = getToken()
    if (token) {
      config.headers.common.Authorization = 'Bearer ' + token
    }
    return config
  },
  function (error) {
    return Promise.reject(error)
  }
)

http.interceptors.response.use(
  response => {
    return response
  },
  error => {
    if (
      !!error.response &&
      !!error.response.data.error &&
      !!error.response.data.error.message &&
      error.response.data.error.details
    ) {
      Modal.error({
        title: error.response.data.error.message,
        content: error.response.data.error.details
      })
    } else if (
      !!error.response &&
      !!error.response.data.error &&
      !!error.response.data.error.message
    ) {
      Modal.error({
        title: 'Login failed',
        content: error.response.data.error.message
      })
    } else if (!error.response) {
      Modal.error({ content: 'Unknown Error' })
    } else if (error.response.status === 401) {
      logout()
      window.location.reload()
    }
    return Promise.reject(error)
  }
)

export default http
