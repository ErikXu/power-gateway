import request from '@/utils/request'

export function getIpRequest1Munite(pageIndex, pageSize) {
  return request({
    url: `/api/requests/ip/1m?pageIndex=${pageIndex}&pageSize=${pageSize}`,
    method: 'get'
  })
}

export function getIpRequest1Hour(pageIndex, pageSize) {
  return request({
    url: `/api/requests/ip/1h?pageIndex=${pageIndex}&pageSize=${pageSize}`,
    method: 'get'
  })
}

export function getUserRequest1Munite(pageIndex, pageSize) {
  return request({
    url: `/api/requests/user/1m?pageIndex=${pageIndex}&pageSize=${pageSize}`,
    method: 'get'
  })
}

export function getUserRequest1Hour(pageIndex, pageSize) {
  return request({
    url: `/api/requests/user/1h?pageIndex=${pageIndex}&pageSize=${pageSize}`,
    method: 'get'
  })
}
