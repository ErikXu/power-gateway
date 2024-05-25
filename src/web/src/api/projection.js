import request from '@/utils/request'

export function getProjectionList() {
  return request({
    url: '/api/projections',
    method: 'get'
  })
}

