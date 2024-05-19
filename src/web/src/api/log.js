import request from '@/utils/request'

export function getApisixRequestLogList() {
  return request({
    url: '/api/logs/apisix',
    method: 'get'
  })
}

export function getApisixRequestLog(id) {
  return request({
    url: `/api/logs/apisix/${id}`,
    method: 'get'
  })
}
