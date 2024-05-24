import request from '@/utils/request'

export function getAlarmConfig() {
  return request({
    url: '/api/alarms/configs',
    method: 'get'
  })
}

export function createAlarmConfig(form) {
  return request({
    url: '/api/alarms/configs',
    method: 'post',
    data: form
  })
}
