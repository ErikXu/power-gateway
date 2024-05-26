import request from '@/utils/request'

export function getBasicSetting() {
  return request({
    url: '/api/settings',
    method: 'get'
  })
}

export function saveBasicSetting(form) {
  return request({
    url: '/api/settings',
    method: 'post',
    data: form
  })
}
