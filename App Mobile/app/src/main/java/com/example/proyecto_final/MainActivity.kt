package com.example.proyecto_final

import android.os.Bundle
import android.view.View
import androidx.appcompat.app.AppCompatActivity
import androidx.appcompat.content.res.AppCompatResources
import com.example.proyecto_final.adapter.ViewPagerFragmentAdapter
import com.example.proyecto_final.adapter.ViewPagerFragmentAdapter.Companion.FIRST_TAB
import com.example.proyecto_final.adapter.ViewPagerFragmentAdapter.Companion.SECOND_TAB
import com.google.android.material.tabs.TabLayoutMediator
import kotlinx.android.synthetic.main.principal_main_activity.*
import kotlinx.android.synthetic.main.view_interactions_tab.view.*

class MainActivity : AppCompatActivity() {
    private val viewPager by lazy { view_pager }
    val tabLayout by lazy { tab_layout }

    override fun onCreate(savedInstanceState: Bundle?) {
        setTheme(R.style.AppTheme)
        super.onCreate(savedInstanceState)
        setContentView(R.layout.principal_main_activity)

        viewPager.adapter = ViewPagerFragmentAdapter(supportFragmentManager, lifecycle)
        viewPager.isUserInputEnabled = false
        TabLayoutMediator(tabLayout, viewPager) { _, _ -> }.attach()

        setCustomViewTabLayout()
    }

    private fun setCustomViewTabLayout() {

        tabLayout.visibility = View.VISIBLE
        val customViewTab1 = View.inflate(baseContext, R.layout.view_interactions_tab, null)
        customViewTab1.counter.text = getString(R.string.map_id)
        customViewTab1.icon.setImageDrawable(
            AppCompatResources.getDrawable(
                baseContext,
                R.drawable.ic_mapa
            )
        )
        tabLayout.getTabAt(FIRST_TAB)?.customView = customViewTab1
        val customViewTab2 = View.inflate(baseContext, R.layout.view_interactions_tab, null)
        customViewTab2.counter.text = getString(R.string.settings_id)
        customViewTab2.icon.setImageDrawable(
            AppCompatResources.getDrawable(
                baseContext,
                R.drawable.ic_menu
            )
        )
        tabLayout.getTabAt(SECOND_TAB)?.customView = customViewTab2
    }


    fun swipeToTab(tabIndex: Int) {

        tabLayout.getTabAt(tabIndex)?.select()

        viewPager.currentItem = tabIndex

    }

}