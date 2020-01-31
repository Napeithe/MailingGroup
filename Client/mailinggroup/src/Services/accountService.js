import { endpoints } from './appconst'
import http from './httpService'

const itemKey = 'User'

export const registerUser = (userData) => {
  return http.post(endpoints.account.register, userData)
}

export const loginService = (loginForm) => {
  return http.post(endpoints.account.login, loginForm)
    .then(response => {
      localStorage.setItem(itemKey, JSON.stringify(response.data))
      return Promise.resolve()
    })
    .catch(err => {
      if (err.response.status === 401) {
        return Promise.reject(new Error('Incorect username or password'))
      } else {
        return Promise.reject(new Error(`Cannot sign in right now. Status code: ${err.response.status}`))
      }
    })
}

export const getToken = () => {
  const user = localStorage.getItem(itemKey)
  if (!user) {
    return null
  }

  return user.accessToken
}
