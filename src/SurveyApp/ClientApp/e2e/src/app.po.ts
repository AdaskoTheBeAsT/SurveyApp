/*
 * Use the Page Object pattern to define the page under test.
 * See docs/coding-guide/e2e-tests.md for more info.
 */

import { Selector } from 'testcafe';

import { browser } from '../utils';

const elementWithIdOrClassName = Selector((value) => {
  //lgtm [js/unused-local-variable]
  return document.getElementById(value) || document.getElementsByClassName(value);
});

export class AppPage {
  constructor() {
    // localStorage.setItem('language', 'en-US');
  }

  navigateTo() {
    return browser.goTo('/');
  }

  getParagraphText() {
    return Selector('app-root h1').textContent;
  }
}
