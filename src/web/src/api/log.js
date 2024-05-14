import request from '@/utils/request'

export function getApisixRequestLogList() {
  return request({
    url: '/api/logs/apisix',
    method: 'get'
  })
}

