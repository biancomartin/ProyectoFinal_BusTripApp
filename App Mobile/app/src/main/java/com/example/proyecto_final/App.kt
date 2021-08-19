package com.example.proyecto_final

import android.app.Application
import android.content.Context
import com.example.proyecto_final.di.viewModelModule
import com.example.di.networkingModule
import com.example.di.repositoryModule
import com.example.di.useCasesModule
import org.koin.android.ext.koin.androidContext
import org.koin.android.ext.koin.androidLogger
import org.koin.core.context.startKoin
import org.koin.core.logger.Level

class App : Application() {
    override fun onCreate() {
        super.onCreate()
        startKoin {
            androidContext(this@App)
            androidLogger(Level.DEBUG)
            modules(listOf(viewModelModule,useCasesModule,repositoryModule,networkingModule))
        }
    }

    override fun attachBaseContext(base: Context?) {
        super.attachBaseContext(base)
    }
}