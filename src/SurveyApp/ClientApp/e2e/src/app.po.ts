/*
 * Use the Page Object pattern to define the page under test.
 * See docs/coding-guide/e2e-tests.md for more info.
 */

import { Selector } from 'testcafe';

import { browser } from '../utils';

const elementWithIdOrClassName = Selector((value) => {
  return document.getElementById(value) || document.getElementsByClassName(value);
});

export class AppPage {
  constructor() {
    // Forces default language
    this.navigateTo();
    localStorage.setItem('language', 'en-US');
  }

  navigateTo() {
    return browser.goTo('/');
  }

  getParagraphText() {
    return Selector(elementWithIdOrClassName('app-root h1')).textContent;
  }
}
