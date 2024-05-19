<template>
  <div class="app-container">
    <h3>{{ $t('Request Log') }}</h3>
    <el-table
      :data="list"
      border
      fit
      highlight-current-row
      style="margin-top:10px;"
    >
      <el-table-column label="#" align="center" width="55">
        <template slot-scope="scope">
          {{ scope.$index + 1 }}
        </template>
      </el-table-column>
      <el-table-column :label="$t('Start Time')" align="left" width="160">
        <template slot-scope="{row}">
          <span>{{ row.startTime | simpleFormat }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Url')" align="left">
        <template slot-scope="{row}">
          <el-tag v-if="row.method === 'GET'" size="small" effect="dark" color="#61AFFE" style="border-color: #d9ecff;">{{ row.method }}</el-tag>
          <el-tag v-if="row.method === 'POST'" size="small" effect="dark" color="#49C990" style="border-color: #d9ecff;">{{ row.method }}</el-tag>
          <el-tag v-if="row.method === 'PUT'" size="small" effect="dark" color="#FCA130" style="border-color: #d9ecff;">{{ row.method }}</el-tag>
          <el-tag v-if="row.method === 'PATCH'" size="small" effect="dark" color="#50E3C2" style="border-color: #d9ecff;">{{ row.method }}</el-tag>
          <el-tag v-if="row.method === 'DELETE'" size="small" effect="dark" color="#F93E3E" style="border-color: #d9ecff;">{{ row.method }}</el-tag>
          <span> {{ row.url }} </span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Upstream')" align="center" width="140">
        <template slot-scope="{row}">
          <span>{{ row.upstream }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Latency')" align="center" width="120">
        <template slot-scope="{row}">
          <span>{{ row.latency.toFixed(2) }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Status')" align="center" width="80">
        <template slot-scope="{row}">
          <span>{{ row.status }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('Operation')" align="center" width="80">
        <template slot-scope="{row}">
          <el-button type="primary" size="mini" @click="detail(row)">
            {{ $t('Detail') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>
  </div>
</template>

<script>
import { getApisixRequestLogList } from '@/api/log'

export default {
  data() {
    return {
      list: []
    }
  },
  created() {
    this.fetchData()
  },
  methods: {
    fetchData() {
      getApisixRequestLogList().then(response => {
        this.list = response
      })
    },
    detail(row) {
      this.$router.push({ name: 'apisix-request-log-detail', params: { id: row.id }})
    }
  }
}
</script>

<i18n>
{
  "en": {
    "Request Log": "Request Log",
    "Start Time": "Start Time",
    "Url": "Url",
    "Upstream": "Upstream",
    "Latency": "Latency - ms",
    "Status": "Status",
    "Operation": "Operation",
    "Detail": "Detail"
  },
  "zh": {
    "Request Log": "请求日志",
    "Start Time": "请求时间",
    "Url": "请求地址",
    "Upstream": "上游",
    "Latency": "延时 - 毫秒",
    "Status": "状态码",
    "Operation": "操作",
    "Detail": "详情"
  }
}
</i18n>
