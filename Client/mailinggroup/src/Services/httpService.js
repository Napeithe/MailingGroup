import { serverUrl } from './appconst'
import { Modal } from 'antd'
import axios from 'axios'

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
    // TODO add token

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
    if (!!error.response && !!error.response.data.error && !!error.response.data.error.message && error.response.data.error.details) {
      Modal.error({
        title: error.response.data.error.message,
        content: error.response.data.error.details
      })
    } else if (!!error.response && !!error.response.data.error && !!error.response.data.error.message) {
      Modal.error({
        title: 'Login failed',
        content: error.response.data.error.message
      })
    } else if (!error.response) {
      Modal.error({ content: 'Unknown Error' })
    }

    setTimeout(() => {}, 1000)

    return Promise.reject(error)
  }
)

export default http
