import request from '@/utils/request'

export function getApisixRequestLogList(pageIndex, pageSize) {
  return request({
    url: `/api/logs/apisix?pageIndex=${pageIndex}&pageSize=${pageSize}`,
    method: 'get'
  })
}

export function getApisixRequestLog(id) {
  return request({
    url: `/api/logs/apisix/${id}`,
    method: 'get'
  })
}
