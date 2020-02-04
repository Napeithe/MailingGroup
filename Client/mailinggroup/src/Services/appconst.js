export const serverUrl = 'https://localhost:5001'

export const endpoints = {
  account: {
    login: '/api/TokenAuth/authenticate',
    register: '/api/TokenAuth/register'
  },
  mailingGroup: {
    getAll: '/api/MailingGroup',
    update: '/api/MailingGroup',
    create: '/api/MailingGroup',
    get: '/api/MailingGroup',
    delete: '/api/MailingGroup'
  }
}
