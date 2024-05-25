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

export function checkAlarmConfig(id) {
  return request({
    url: `/api/alarms/configs/${id}/check`,
    method: 'get'
  })
}

export function getAlarmRules() {
  return request({
    url: '/api/alarms/rules',
    method: 'get'
  })
}

export function createAlarmRule(form) {
  return request({
    url: '/api/alarms/rules',
    method: 'post',
    data: form
  })
}

export function getFieldOptions() {
  return request({
    url: '/api/alarms/option/fields',
    method: 'get'
  })
}

export function getOperatorOptions() {
  return request({
    url: '/api/alarms/option/operators',
    method: 'get'
  })
}

export function getConfigOptions() {
  return request({
    url: '/api/alarms/option/configs',
    method: 'get'
  })
}
