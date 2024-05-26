import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

/* Layout */
import Layout from '@/layout'
import { getLocale, LOCALE_MAP } from '@/i18n'

/**
 * Note: sub-menu only appear when route children.length >= 1
 * Detail see: https://panjiachen.github.io/vue-element-admin-site/guide/essentials/router-and-nav.html
 *
 * hidden: true                   if set true, item will not show in the sidebar(default is false)
 * alwaysShow: true               if set true, will always show the root menu
 *                                if not set alwaysShow, when item has more than one children route,
 *                                it will becomes nested mode, otherwise not show the root menu
 * redirect: noRedirect           if set noRedirect will no redirect in the breadcrumb
 * name:'router-name'             the name is used by <keep-alive> (must set!!!)
 * meta : {
    roles: ['admin','editor']    control the page roles (you can set multiple roles)
    title: 'title'               the name show in sidebar and breadcrumb (recommend set)
    icon: 'svg-name'/'el-icon-x' the icon show in the sidebar
    breadcrumb: false            if set false, the item will hidden in breadcrumb(default is true)
    activeMenu: '/example/list'  if set path, the sidebar will highlight the path you set
  }
 */

/**
 * constantRoutes
 * a base page that does not have permission requirements
 * all roles can be accessed
 */
export const constantRoutes = [
  {
    path: '/login',
    component: () => import('@/views/login/index'),
    hidden: true
  },

  {
    path: '/404',
    component: () => import('@/views/404'),
    hidden: true
  },
  {
    path: '/',
    component: Layout,
    redirect: '/apisix-request-log',
    children: [
      {
        path: 'apisix-request-log',
        name: 'apisix-request-log',
        component: () => import('@/views/apisix/request-log'),
        meta: { title: 'Request Log', icon: 'el-icon-document' }
      },
      {
        path: '/apisix-request-log/:id',
        name: 'apisix-request-log-detail',
        component: () => import('@/views/apisix/detail'),
        meta: { title: 'Request Log Detail' },
        hidden: true
      }]
  },

  {
    path: '/alarm',
    component: Layout,
    name: 'alarm',
    meta: { title: 'Alarm', icon: 'el-icon-folder' },
    children: [
      {
        path: 'config',
        name: 'config',
        component: () => import('@/views/alarm/config'),
        meta: { title: 'Alarm Config', icon: 'el-icon-document' }
      },
      {
        path: 'rule',
        name: 'rule',
        component: () => import('@/views/alarm/rule'),
        meta: { title: 'Alarm Rule', icon: 'el-icon-document' }
      }]
  },

  {
    path: '/setting',
    component: Layout,
    name: 'setting',
    meta: { title: 'Setting', icon: 'el-icon-folder' },
    children: [
      {
        path: 'basic',
        name: 'basic',
        component: () => import('@/views/setting/basic'),
        meta: { title: 'Basic Setting', icon: 'el-icon-document' }
      },
      {
        path: 'projection',
        name: 'projection',
        component: () => import('@/views/setting/projection'),
        meta: { title: 'Field Projection', icon: 'el-icon-document' }
      }]
  },

  // 404 page must be placed at the end !!!
  { path: '*', redirect: '/404', hidden: true }
]

const routerBase = LOCALE_MAP[getLocale()]

const createRouter = () => new Router({
  // mode: 'history', // require service support
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRoutes,
  base: routerBase
})

const router = createRouter()

// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
export function resetRouter() {
  const newRouter = createRouter()
  router.matcher = newRouter.matcher // reset router
}

export default router
